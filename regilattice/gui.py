"""Tkinter GUI for RegiLattice — Windows 11 style, plugin-driven, category-split.

Launch via ``python -m regilattice --gui`` or ``regilattice --gui``.

Features:
  * Auto-discovers tweaks from ``regilattice.tweaks`` plugin package
  * Live status detection (APPLIED / DEFAULT / UNKNOWN) with colour indicators
  * Multi-state tweaks show options via hover tooltip
  * Default behaviour / recommendation shown for undetected tweaks
  * Categories displayed as collapsible sections with tweak counts
  * Save-snapshot / restore-snapshot for undo
  * Corp-safe enforcement — non-corp-safe tweaks disabled on corp networks
  * Windows 11 dark Mica-like theme (Catppuccin Mocha palette)
"""

from __future__ import annotations

import contextlib
import functools
import sys
import threading
import tkinter as tk
from collections.abc import Callable
from pathlib import Path
from tkinter import filedialog, messagebox, ttk

from . import __version__
from .corpguard import CorporateNetworkError, assert_not_corporate, corp_guard_status, is_corporate_network, is_gpo_managed
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary
from .tweaks import (
    TweakDef,
    all_tweaks,
    available_profiles,
    category_info,
    profile_info,
    restore_snapshot,
    save_snapshot,
    search_tweaks,
    status_map,
    tweak_scope,
    tweak_status,
    tweaks_by_category,
)
from .tweaks.maintenance import create_restore_point

# ── Theme — Catppuccin Mocha / Windows 11 dark ──────────────────────────────

_ACCENT = "#89B4FA"  # Blue
_BG = "#1E1E2E"  # Base
_BG_SURFACE = "#24273A"  # Surface0
_FG = "#CDD6F4"  # Text
_FG_DIM = "#6C7086"  # Overlay0
_CARD_BG = "#313244"  # Surface1
_CARD_HOVER = "#45475A"  # Surface2
_OK_GREEN = "#A6E3A1"  # Green
_WARN_YELLOW = "#F9E2AF"  # Yellow
_ERR_RED = "#F38BA8"  # Red
_PURPLE = "#CBA6F7"  # Mauve
_HEADER_BG = "#181825"  # Crust
_BORDER = "#45475A"  # Surface2
_DIM_BG = "#585B70"  # Overlay2
_TEAL = "#94E2D5"  # Teal
_GPO_ORANGE = "#FAB387"  # Peach — Group Policy managed indicator

# Status indicator colours
_STATUS_APPLIED = _OK_GREEN
_STATUS_NOT_APPLIED = _FG_DIM
_STATUS_UNKNOWN = _WARN_YELLOW
_STATUS_CORP_BLOCKED = _ERR_RED
_STATUS_DEFAULT = "#89DCEB"  # Sky — tweak not in registry (Windows default)

# Fonts
_FONT = ("Segoe UI", 10)
_FONT_BOLD = ("Segoe UI Semibold", 10)
_FONT_SM = ("Segoe UI", 9)
_FONT_XS = ("Segoe UI", 8)
_FONT_XS_BOLD = ("Segoe UI", 8, "bold")
_FONT_TITLE = ("Segoe UI Semibold", 16)
_FONT_CAT = ("Segoe UI Semibold", 11)


# ── Tooltip helper ───────────────────────────────────────────────────────────


class _Tooltip:
    """Hover tooltip that follows the mouse cursor and supports text updates."""

    def __init__(self, widget: tk.Widget, text: str) -> None:
        self._widget = widget
        self._text = text
        self._tip: tk.Toplevel | None = None
        widget.bind("<Enter>", self._show)
        widget.bind("<Leave>", self._hide)
        widget.bind("<Motion>", self._move)

    def update_text(self, text: str) -> None:
        self._text = text

    def _show(self, event: tk.Event[tk.Misc]) -> None:
        if self._tip:
            return
        x = event.x_root + 14
        y = event.y_root + 10
        self._tip = tw = tk.Toplevel(self._widget)
        tw.wm_overrideredirect(True)
        tw.wm_geometry(f"+{x}+{y}")
        tw.wm_attributes("-topmost", True)
        lbl = tk.Label(
            tw,
            text=self._text,
            bg=_CARD_HOVER,
            fg=_FG,
            font=_FONT_SM,
            padx=10,
            pady=6,
            wraplength=440,
            justify="left",
        )
        lbl.pack()

    def _move(self, event: tk.Event[tk.Misc]) -> None:
        if self._tip:
            self._tip.wm_geometry(f"+{event.x_root + 14}+{event.y_root + 10}")

    def _hide(self, _: tk.Event[tk.Misc]) -> None:
        if self._tip:
            self._tip.destroy()
            self._tip = None


# ── Tooltip text builder ─────────────────────────────────────────────────────


@functools.lru_cache(maxsize=2048)
def _parse_description_metadata(description: str) -> tuple[str, str, str, str]:
    """Extract Default/Recommended/Options hints from description text.

    Returns (main_text, default_hint, recommendation_hint, options_hint).
    """
    main_parts: list[str] = []
    default_hint = ""
    rec_hint = ""
    options_hint = ""
    for sentence in description.replace(". ", ".\n").splitlines():
        s = sentence.strip()
        low = s.lower()
        if low.startswith("default:"):
            default_hint = s
        elif low.startswith("recommended:"):
            rec_hint = s
        elif low.startswith("options:") or low.startswith("values:"):
            options_hint = s
        else:
            main_parts.append(s)
    return " ".join(main_parts).strip(), default_hint, rec_hint, options_hint


def _has_recommendation(td: TweakDef) -> bool:
    """Return True if the tweak description contains a recommendation."""
    return "recommended:" in td.description.lower()


def _build_tooltip_text(td: TweakDef, status: str) -> str:
    """Build rich tooltip including description, current state, options, and recommendation."""
    parts: list[str] = []

    main_desc, default_hint, rec_hint, options_hint = _parse_description_metadata(td.description or "")
    if main_desc:
        parts.append(main_desc)

    parts.append("─" * 40)

    # Status line
    status_labels = {
        "applied": "✔ Currently: APPLIED (tweak is active)",
        "not applied": "○ Currently: DEFAULT (tweak is not active)",
        "unknown": "? Currently: UNKNOWN (cannot detect state)",
    }
    parts.append(status_labels.get(status, f"Currently: {status}"))

    # Default behaviour & recommendation
    if default_hint:
        parts.append(f"i {default_hint}")
    if rec_hint:
        parts.append(f"★ {rec_hint}")
    if options_hint:
        parts.append(f"☰ {options_hint}")

    parts.append("")

    # GPO status
    if td.registry_keys and is_gpo_managed(td.registry_keys):
        parts.append("🛡 Group Policy: MANAGED (value may be overridden by GPO)")

    # Admin / corp info
    if td.needs_admin:
        parts.append("⚠ Requires administrator privileges")
    if td.corp_safe:
        parts.append("✓ Safe for corporate environments")
    else:
        parts.append("⚠ Not recommended for corporate networks")

    # Tags
    if td.tags:
        parts.append(f"Tags: {', '.join(td.tags)}")

    # Registry keys hint
    if td.registry_keys:
        parts.append(f"\nRegistry: {td.registry_keys[0]}")
        if len(td.registry_keys) > 1:
            parts.append(f"  (+{len(td.registry_keys) - 1} more key(s))")

    return "\n".join(parts)


# ── Row widget ───────────────────────────────────────────────────────────────


class _TweakRow:
    """Single tweak row: status dot + status text + checkbox + toggle button + tooltip."""

    def __init__(
        self,
        parent: ttk.Frame,
        td: TweakDef,
        *,
        corp_blocked: bool,
        on_toggle: Callable[[_TweakRow], None] | None = None,
    ) -> None:
        self.td = td
        self.var = tk.BooleanVar(value=False)
        self._corp_blocked = corp_blocked
        self._on_toggle = on_toggle
        self.disabled_by_corp = corp_blocked and not td.corp_safe

        self.frame = ttk.Frame(parent, style="Card.TFrame")
        # Don't pack yet — the _CategorySection controls packing

        # Status dot
        self.status_dot = tk.Label(
            self.frame,
            text="●",
            fg=_STATUS_UNKNOWN,
            bg=_CARD_BG,
            font=("Segoe UI", 12),
            width=2,
        )
        self.status_dot.pack(side="left", padx=(6, 0))

        # Status text label (APPLIED / DEFAULT / UNKNOWN)
        self.status_text = tk.Label(
            self.frame,
            text="…",
            fg=_STATUS_UNKNOWN,
            bg=_CARD_BG,
            font=_FONT_XS_BOLD,
            width=10,
            anchor="w",
        )
        self.status_text.pack(side="left", padx=(2, 4))

        # Checkbox for batch selection
        state = "disabled" if self.disabled_by_corp else "normal"
        self.cb = ttk.Checkbutton(
            self.frame,
            text=td.label,
            variable=self.var,
            state=state,
        )
        self.cb.pack(side="left", padx=(4, 4), pady=2)

        # Toggle button (individual enable/disable)
        self.toggle_btn = tk.Button(
            self.frame,
            text="⏳",
            font=_FONT_XS_BOLD,
            width=9,
            relief="flat",
            cursor="hand2",
            bd=0,
            padx=6,
            pady=1,
        )
        if self.disabled_by_corp:
            self.toggle_btn.configure(
                text="BLOCKED",
                bg=_CARD_BG,
                fg=_ERR_RED,
                state="disabled",
            )
        else:
            self.toggle_btn.configure(command=self.on_toggle_click)
        self.toggle_btn.pack(side="right", padx=(4, 8))

        # Tags (right-aligned, before toggle button)
        if self.disabled_by_corp:
            tk.Label(
                self.frame,
                text="CORP",
                fg=_ERR_RED,
                bg=_CARD_BG,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))
        if td.registry_keys and is_gpo_managed(td.registry_keys):
            tk.Label(
                self.frame,
                text="GPO",
                fg=_GPO_ORANGE,
                bg=_CARD_BG,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))
        if td.needs_admin:
            tk.Label(
                self.frame,
                text="ADMIN",
                fg=_FG_DIM,
                bg=_CARD_BG,
                font=("Segoe UI", 7),
            ).pack(side="right", padx=(0, 4))
        if _has_recommendation(td):
            tk.Label(
                self.frame,
                text="★ REC",
                fg=_TEAL,
                bg=_CARD_BG,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))

        # Scope badge (USER / MACHINE / BOTH)
        scope = tweak_scope(td)
        _scope_cfg = {"user": ("USER", _OK_GREEN), "machine": ("MACHINE", _ACCENT), "both": ("BOTH", _WARN_YELLOW)}
        _s_text, _s_color = _scope_cfg.get(scope, ("?", _FG_DIM))
        tk.Label(
            self.frame,
            text=_s_text,
            fg=_s_color,
            bg=_CARD_BG,
            font=("Segoe UI", 7, "bold"),
        ).pack(side="right", padx=(0, 4))

        # Tooltip — detailed description with state, options, recommendation
        self.tooltip = _Tooltip(self.frame, _build_tooltip_text(td, "unknown"))

        # Hover highlight effect
        self.frame.bind("<Enter>", self._on_enter)
        self.frame.bind("<Leave>", self._on_leave)

    def _on_enter(self, _: tk.Event[tk.Misc]) -> None:
        """Highlight row background on hover."""
        for w in self.frame.winfo_children():
            if isinstance(w, tk.Label):
                w.configure(bg=_CARD_HOVER)
        # ttk frame style can't be changed per-widget, but tk children can

    def _on_leave(self, _: tk.Event[tk.Misc]) -> None:
        """Restore row background on leave."""
        for w in self.frame.winfo_children():
            if isinstance(w, tk.Label):
                w.configure(bg=_CARD_BG)

    def on_toggle_click(self) -> None:
        if self._on_toggle:
            self._on_toggle(self)

    def pack_row(self) -> None:
        """Pack the row frame into its parent."""
        self.frame.pack(fill="x", padx=4, pady=2, ipady=3)

    def unpack_row(self) -> None:
        """Remove from display."""
        self.frame.pack_forget()

    def refresh_status(self) -> None:
        """Update the status dot, text label, toggle button, and tooltip."""
        st = tweak_status(self.td)
        if self.disabled_by_corp:
            colour = _STATUS_CORP_BLOCKED
            text = "BLOCKED"
            btn_text = "BLOCKED"
            btn_bg = _CARD_BG
            btn_fg = _ERR_RED
        elif st == "applied":
            colour = _STATUS_APPLIED
            text = "APPLIED"
            btn_text = "Disable ✕"
            btn_bg = "#40543F"
            btn_fg = _OK_GREEN
        elif st == "not applied":
            colour = _STATUS_DEFAULT
            text = "DEFAULT"
            btn_text = "Enable ✓"
            btn_bg = "#3B3552"
            btn_fg = _ACCENT
        else:
            colour = _STATUS_UNKNOWN
            text = "UNKNOWN"
            btn_text = "Enable ✓"
            btn_bg = "#3B3830"
            btn_fg = _WARN_YELLOW
        self.status_dot.configure(fg=colour)
        self.status_text.configure(text=text, fg=colour)
        if not self.disabled_by_corp:
            self.toggle_btn.configure(text=btn_text, bg=btn_bg, fg=btn_fg)
        # Update tooltip with current status
        self.tooltip.update_text(_build_tooltip_text(self.td, st))


# ── Category section (collapsible) ───────────────────────────────────────────


class _CategorySection:
    """A collapsible category section with a clickable header and tweak count."""

    def __init__(self, parent: ttk.Frame, name: str, rows: list[_TweakRow]) -> None:
        self.name = name
        self.rows = rows
        self.expanded = True
        self._parent = parent

        # Header bar — clickable to expand/collapse
        self.header = tk.Frame(parent, bg=_BG_SURFACE, cursor="hand2")
        self.header.pack(fill="x", pady=(8, 0), padx=4)

        self._arrow = tk.Label(
            self.header,
            text="▼",
            fg=_ACCENT,
            bg=_BG_SURFACE,
            font=_FONT_CAT,
        )
        self._arrow.pack(side="left", padx=(8, 4))

        self._title = tk.Label(
            self.header,
            text=name,
            fg=_ACCENT,
            bg=_BG_SURFACE,
            font=_FONT_CAT,
        )
        self._title.pack(side="left")

        # Count badge: (N/M applied)
        self._count_lbl = tk.Label(
            self.header,
            text=f"  ({len(rows)} tweaks)",
            fg=_FG_DIM,
            bg=_BG_SURFACE,
            font=_FONT_XS,
        )
        self._count_lbl.pack(side="left", padx=(4, 0))

        # Risk / scope badges from CategoryInfo metadata
        ci = category_info(name)
        if ci is not None:
            _risk_colors = {"high": _ERR_RED, "medium": _WARN_YELLOW, "low": _OK_GREEN}
            risk_fg = _risk_colors.get(ci.risk_level, _FG_DIM)
            tk.Label(
                self.header,
                text=ci.risk_level.upper(),
                fg=risk_fg,
                bg=_BG_SURFACE,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="left", padx=(6, 0))
            _scope_colors = {"user": _OK_GREEN, "machine": _PURPLE, "mixed": _WARN_YELLOW}
            scope_fg = _scope_colors.get(ci.scope, _FG_DIM)
            tk.Label(
                self.header,
                text=ci.scope.upper(),
                fg=scope_fg,
                bg=_BG_SURFACE,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="left", padx=(4, 0))
            if ci.profiles:
                tk.Label(
                    self.header,
                    text=", ".join(ci.profiles),
                    fg=_TEAL,
                    bg=_BG_SURFACE,
                    font=("Segoe UI", 7),
                ).pack(side="left", padx=(6, 0))

        # Per-category batch buttons
        self._btn_disable_all = tk.Button(
            self.header,
            text="Disable All",
            font=_FONT_XS,
            relief="flat",
            bg=_BG_SURFACE,
            fg=_ERR_RED,
            cursor="hand2",
            bd=0,
            padx=4,
        )
        self._btn_disable_all.pack(side="right", padx=(2, 8))
        self._btn_enable_all = tk.Button(
            self.header,
            text="Enable All",
            font=_FONT_XS,
            relief="flat",
            bg=_BG_SURFACE,
            fg=_OK_GREEN,
            cursor="hand2",
            bd=0,
            padx=4,
        )
        self._btn_enable_all.pack(side="right", padx=(2, 0))

        # Bind click on all header widgets
        for w in (self.header, self._arrow, self._title, self._count_lbl):
            w.bind("<Button-1>", self.toggle)

        # Content frame — holds the tweak rows
        self.content_frame = ttk.Frame(parent, style="TFrame")
        self.content_frame.pack(fill="x")

        # Re-parent each row frame into the content_frame and show
        for row in self.rows:
            row.frame.destroy()
            row.frame = ttk.Frame(self.content_frame, style="Card.TFrame")
            self._rebuild_row_widgets(row)
            row.pack_row()

    def _rebuild_row_widgets(self, row: _TweakRow) -> None:
        """Reconstruct row widgets inside the new parent frame."""
        td = row.td
        disabled = row.disabled_by_corp

        # Status dot
        row.status_dot = tk.Label(
            row.frame,
            text="●",
            fg=_STATUS_UNKNOWN,
            bg=_CARD_BG,
            font=("Segoe UI", 12),
            width=2,
        )
        row.status_dot.pack(side="left", padx=(6, 0))

        # Status text
        row.status_text = tk.Label(
            row.frame,
            text="…",
            fg=_STATUS_UNKNOWN,
            bg=_CARD_BG,
            font=_FONT_XS_BOLD,
            width=10,
            anchor="w",
        )
        row.status_text.pack(side="left", padx=(2, 4))

        # Checkbox
        state = "disabled" if disabled else "normal"
        row.cb = ttk.Checkbutton(
            row.frame,
            text=td.label,
            variable=row.var,
            state=state,
        )
        row.cb.pack(side="left", padx=(4, 4), pady=2)

        # Toggle button
        row.toggle_btn = tk.Button(
            row.frame,
            text="⏳",
            font=_FONT_XS_BOLD,
            width=9,
            relief="flat",
            cursor="hand2",
            bd=0,
            padx=6,
            pady=1,
        )
        if disabled:
            row.toggle_btn.configure(
                text="BLOCKED",
                bg=_CARD_BG,
                fg=_ERR_RED,
                state="disabled",
            )
        else:
            row.toggle_btn.configure(command=row.on_toggle_click)
        row.toggle_btn.pack(side="right", padx=(4, 8))

        # Tags
        if disabled:
            tk.Label(
                row.frame,
                text="CORP",
                fg=_ERR_RED,
                bg=_CARD_BG,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))
        if td.registry_keys and is_gpo_managed(td.registry_keys):
            tk.Label(
                row.frame,
                text="GPO",
                fg=_GPO_ORANGE,
                bg=_CARD_BG,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))
        if td.needs_admin:
            tk.Label(
                row.frame,
                text="ADMIN",
                fg=_FG_DIM,
                bg=_CARD_BG,
                font=("Segoe UI", 7),
            ).pack(side="right", padx=(0, 4))
        if _has_recommendation(td):
            tk.Label(
                row.frame,
                text="★ REC",
                fg=_TEAL,
                bg=_CARD_BG,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))

        # Scope badge (USER / MACHINE / BOTH)
        scope = tweak_scope(td)
        _scope_cfg = {"user": ("USER", _OK_GREEN), "machine": ("MACHINE", _ACCENT), "both": ("BOTH", _WARN_YELLOW)}
        _s_text, _s_color = _scope_cfg.get(scope, ("?", _FG_DIM))
        tk.Label(
            row.frame,
            text=_s_text,
            fg=_s_color,
            bg=_CARD_BG,
            font=("Segoe UI", 7, "bold"),
        ).pack(side="right", padx=(0, 4))

        # Tooltip
        row.tooltip = _Tooltip(row.frame, _build_tooltip_text(td, "unknown"))

    def toggle(self, _: tk.Event[tk.Misc] | None = None) -> None:
        self.expanded = not self.expanded
        if self.expanded:
            self._arrow.configure(text="▼")
            self._show_rows()
        else:
            self._arrow.configure(text="▶")
            self._hide_rows()

    def _show_rows(self) -> None:
        self.content_frame.pack(fill="x")
        for row in self.rows:
            row.pack_row()

    def _hide_rows(self) -> None:
        for row in self.rows:
            row.unpack_row()
        self.content_frame.pack_forget()

    def update_count(self) -> None:
        """Update the applied/total count in the header badge."""
        applied = sum(1 for r in self.rows if tweak_status(r.td) == "applied")
        total = len(self.rows)
        self._count_lbl.configure(text=f"  ({applied}/{total} applied)")

    def set_on_batch(self, callback: Callable[[_CategorySection, str], None]) -> None:
        """Wire the per-category Enable All / Disable All buttons."""
        self._btn_enable_all.configure(command=lambda: callback(self, "apply"))
        self._btn_disable_all.configure(command=lambda: callback(self, "remove"))

    def filter_rows(self, query: str) -> bool:
        """Show/hide rows matching query.  Returns True if any row is visible."""
        q = query.lower()
        visible = False
        for row in self.rows:
            td = row.td
            match = not q or any(q in f.lower() for f in [td.id, td.label, td.category, td.description, *td.tags])
            if match:
                row.pack_row()
                visible = True
            else:
                row.unpack_row()
        # Show/hide the whole section
        if visible or not q:
            self.header.pack(fill="x", pady=(8, 0), padx=4)
            if self.expanded:
                self.content_frame.pack(fill="x")
        else:
            self.header.pack_forget()
            self.content_frame.pack_forget()
        return visible


# ── Main GUI ─────────────────────────────────────────────────────────────────


class RegiLatticeGUI:
    """Plugin-driven main application window with collapsible category sections."""

    def __init__(self) -> None:
        self._root = tk.Tk()
        self._root.title(f"RegiLattice  v{__version__}")
        self._root.geometry("900x940")
        self._root.minsize(700, 600)
        self._root.configure(bg=_BG)
        self._root.resizable(True, True)

        with contextlib.suppress(tk.TclError, OSError):
            self._root.iconbitmap(default="")  # type: ignore[no-untyped-call]

        # Windows 11: attempt DWM dark title bar
        self._apply_win11_dark_titlebar()

        self._corp_blocked = is_corporate_network()
        self._tweak_rows: list[_TweakRow] = []
        self._category_sections: list[_CategorySection] = []
        self._setup_styles()
        self._build_ui()
        self._bind_shortcuts()
        self._refresh_status_all()

    # ── Windows 11 dark title bar ────────────────────────────────────────

    @staticmethod
    def _apply_win11_dark_titlebar() -> None:
        """Use DwmSetWindowAttribute to request Mica/dark title bar."""
        try:
            import ctypes

            hwnd = ctypes.windll.user32.GetForegroundWindow()
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20
            value = ctypes.c_int(1)
            ctypes.windll.dwmapi.DwmSetWindowAttribute(
                hwnd,
                DWMWA_USE_IMMERSIVE_DARK_MODE,
                ctypes.byref(value),
                ctypes.sizeof(value),
            )
        except (ImportError, OSError, AttributeError):
            pass  # non-Windows or unsupported build

    # ── Styles ───────────────────────────────────────────────────────────

    def _setup_styles(self) -> None:
        style = ttk.Style(self._root)
        style.theme_use("clam")

        style.configure(".", background=_BG, foreground=_FG, font=_FONT)
        style.configure("TFrame", background=_BG)
        style.configure("Header.TFrame", background=_HEADER_BG)
        style.configure("Card.TFrame", background=_CARD_BG)
        style.configure(
            "TCheckbutton",
            background=_CARD_BG,
            foreground=_FG,
            font=_FONT,
            indicatorsize=16,
        )
        style.map(
            "TCheckbutton",
            background=[("active", _CARD_BG), ("disabled", _CARD_BG)],
            foreground=[("active", _FG), ("disabled", _DIM_BG)],
        )
        style.configure("TButton", padding=(14, 6), font=_FONT_BOLD)
        style.configure("Apply.TButton", foreground="white", background="#40A02B")
        style.map("Apply.TButton", background=[("active", "#2E7D32")])
        style.configure("Remove.TButton", foreground="white", background="#E64A19")
        style.map("Remove.TButton", background=[("active", "#BF360C")])
        style.configure("Restore.TButton", foreground="white", background="#7C3AED")
        style.map("Restore.TButton", background=[("active", "#5B21B6")])
        style.configure("Snap.TButton", foreground="white", background="#1565C0")
        style.map("Snap.TButton", background=[("active", "#0D47A1")])
        style.configure("TLabel", background=_BG, foreground=_FG, font=_FONT)
        style.configure("Title.TLabel", background=_HEADER_BG, foreground=_FG, font=_FONT_TITLE)
        style.configure("Subtitle.TLabel", background=_HEADER_BG, foreground=_FG_DIM, font=_FONT_SM)
        style.configure("Status.TLabel", background=_BG, foreground=_FG_DIM, font=_FONT_SM)
        style.configure("Category.TLabel", background=_BG, foreground=_ACCENT, font=_FONT_CAT)

    # ── Keyboard shortcuts ──────────────────────────────────────────────

    def _bind_shortcuts(self) -> None:
        """Register global keyboard shortcuts."""
        self._root.bind("<Control-a>", lambda _: self._select_all())
        self._root.bind("<Control-d>", lambda _: self._deselect_all())
        self._root.bind("<Control-f>", lambda _: self._focus_search())
        self._root.bind("<Control-e>", lambda _: self._expand_all())
        self._root.bind("<Control-Shift-E>", lambda _: self._collapse_all())
        self._root.bind("<Control-r>", lambda _: self._refresh_status_all())
        self._root.bind("<Control-i>", lambda _: self._invert_selection())
        self._root.bind("<Control-l>", lambda _: self._toggle_log_panel())
        self._root.bind("<Escape>", lambda _: self._clear_search())

    def _focus_search(self) -> None:
        self._search_entry.focus_set()
        self._search_entry.select_range(0, "end")

    def _clear_search(self) -> None:
        self._search_var.set("")
        self._root.focus_set()

    # ── UI construction ──────────────────────────────────────────────────

    def _build_ui(self) -> None:
        grouped = tweaks_by_category()
        total_tweaks = len(all_tweaks())
        total_cats = len(grouped)

        # Header
        header = ttk.Frame(self._root, style="Header.TFrame")
        header.pack(fill="x")
        ttk.Label(header, text="⚡ RegiLattice", style="Title.TLabel").pack(
            side="left",
            padx=16,
            pady=(12, 2),
        )
        ttk.Label(
            header,
            text=f"v{__version__}  |  {platform_summary()}  |  {total_tweaks} tweaks · {total_cats} categories",
            style="Subtitle.TLabel",
        ).pack(side="left", padx=4, pady=(14, 2))

        # Corp banner
        if self._corp_blocked:
            corp_info = corp_guard_status() or "corporate environment detected"
            banner = tk.Frame(self._root, bg=_ERR_RED)
            banner.pack(fill="x")
            tk.Label(
                banner,
                text=f"  🛑  Corporate network: {corp_info} — non-corp-safe tweaks blocked",
                bg=_ERR_RED,
                fg="#1E1E2E",
                font=_FONT_BOLD,
                anchor="w",
                padx=12,
                pady=6,
            ).pack(fill="x")

        # Toolbar
        toolbar = ttk.Frame(self._root)
        toolbar.pack(fill="x", padx=16, pady=(10, 0))

        ttk.Button(toolbar, text="Select All", command=self._select_all).pack(side="left", padx=(0, 4))
        ttk.Button(toolbar, text="Deselect All", command=self._deselect_all).pack(side="left", padx=(0, 4))
        ttk.Button(toolbar, text="↻ Refresh", command=self._refresh_status_all).pack(side="left", padx=(0, 4))
        ttk.Button(toolbar, text="▼ Expand All", command=self._expand_all).pack(side="left", padx=(0, 4))
        ttk.Button(toolbar, text="▶ Collapse All", command=self._collapse_all).pack(side="left", padx=(0, 4))
        # Profile selector dropdown
        profile_frame = ttk.Frame(toolbar)
        profile_frame.pack(side="right", padx=(8, 0))
        tk.Label(
            profile_frame,
            text="Profile:",
            fg=_FG_DIM,
            bg=_BG,
            font=_FONT_XS,
        ).pack(side="left", padx=(0, 4))
        self._profile_var = tk.StringVar(value="(none)")
        profile_names = ["(none)"] + [p.title() for p in available_profiles()]
        profile_menu = ttk.OptionMenu(
            profile_frame,
            self._profile_var,
            self._profile_var.get(),
            *profile_names,
            command=lambda val: self._apply_profile_selection(str(val)),
        )
        profile_menu.pack(side="left")

        # Scope filter dropdown
        scope_frame = ttk.Frame(toolbar)
        scope_frame.pack(side="right", padx=(8, 0))
        tk.Label(
            scope_frame,
            text="Scope:",
            fg=_FG_DIM,
            bg=_BG,
            font=_FONT_XS,
        ).pack(side="left", padx=(0, 4))
        self._scope_filter_var = tk.StringVar(value="All")
        scope_menu = ttk.OptionMenu(
            scope_frame,
            self._scope_filter_var,
            "All",
            "All",
            "User Only",
            "Machine Only",
            "Both",
            command=lambda _: self._filter_rows(),
        )
        scope_menu.pack(side="left")

        # Status filter dropdown
        filter_frame = ttk.Frame(toolbar)
        filter_frame.pack(side="right", padx=(8, 0))
        tk.Label(
            filter_frame,
            text="Filter:",
            fg=_FG_DIM,
            bg=_BG,
            font=_FONT_XS,
        ).pack(side="left", padx=(0, 4))
        self._status_filter_var = tk.StringVar(value="All")
        filter_menu = ttk.OptionMenu(
            filter_frame,
            self._status_filter_var,
            "All",
            "All",
            "Applied",
            "Default",
            "Unknown",
            command=lambda _: self._filter_rows(),
        )
        filter_menu.pack(side="left")
        # Selection counter
        self._sel_count_label = tk.Label(
            toolbar,
            text="0 selected",
            fg=_ACCENT,
            bg=_BG,
            font=_FONT_SM,
        )
        self._sel_count_label.pack(side="left", padx=(10, 0))

        self._force_var = tk.BooleanVar(value=False)
        ttk.Checkbutton(toolbar, text="Force (bypass corp guard)", variable=self._force_var).pack(
            side="right",
            padx=(8, 0),
        )

        # Legend
        legend = ttk.Frame(self._root)
        legend.pack(fill="x", padx=16, pady=(4, 0))
        for colour, label in [
            (_STATUS_APPLIED, "Applied"),
            (_STATUS_DEFAULT, "Default"),
            (_STATUS_UNKNOWN, "Unknown"),
            (_STATUS_CORP_BLOCKED, "Corp Blocked"),
            (_GPO_ORANGE, "GPO Managed"),
        ]:
            tk.Label(legend, text="●", fg=colour, bg=_BG, font=_FONT).pack(side="left", padx=(0, 2))
            tk.Label(legend, text=label, fg=_FG_DIM, bg=_BG, font=_FONT_XS).pack(side="left", padx=(0, 10))

        # Keyboard shortcut hints
        tk.Label(
            legend,
            text="Ctrl+F Search | Ctrl+A Select | Ctrl+I Invert | Ctrl+L Log | Ctrl+E Expand | Esc Clear",
            fg=_FG_DIM,
            bg=_BG,
            font=("Segoe UI", 7),
        ).pack(side="right", padx=(10, 0))

        # Search bar
        search_frame = ttk.Frame(self._root)
        search_frame.pack(fill="x", padx=16, pady=(6, 0))
        ttk.Label(search_frame, text="🔍", style="TLabel").pack(side="left", padx=(0, 4))
        self._search_var = tk.StringVar()
        self._search_var.trace_add("write", lambda *_: self._filter_rows())
        self._search_entry = ttk.Entry(search_frame, textvariable=self._search_var, font=_FONT)
        self._search_entry.pack(side="left", fill="x", expand=True)
        ttk.Button(search_frame, text="✕", width=3, command=lambda: self._search_var.set("")).pack(
            side="left",
            padx=(4, 0),
        )

        # Scrollable tweak list
        container = ttk.Frame(self._root)
        container.pack(fill="both", expand=True, padx=16, pady=8)

        canvas = tk.Canvas(container, bg=_BG, highlightthickness=0)
        scrollbar = ttk.Scrollbar(container, orient="vertical", command=canvas.yview)
        self._inner = ttk.Frame(canvas, style="TFrame")

        self._inner.bind("<Configure>", lambda _: canvas.configure(scrollregion=canvas.bbox("all")))
        canvas.create_window((0, 0), window=self._inner, anchor="nw")
        canvas.configure(yscrollcommand=scrollbar.set)
        canvas.pack(side="left", fill="both", expand=True)
        scrollbar.pack(side="right", fill="y")

        def _on_mousewheel(event: tk.Event[tk.Misc]) -> None:
            canvas.yview_scroll(int(-1 * (event.delta / 120)), "units")

        canvas.bind_all("<MouseWheel>", _on_mousewheel)

        # Populate tweak rows grouped by category — each category is collapsible
        for cat_name, cat_tweaks in grouped.items():
            cat_rows: list[_TweakRow] = []
            for td in cat_tweaks:
                row = _TweakRow(
                    self._inner,
                    td,
                    corp_blocked=self._corp_blocked,
                    on_toggle=self._toggle_single,
                )
                self._tweak_rows.append(row)
                cat_rows.append(row)
            section = _CategorySection(self._inner, cat_name, cat_rows)
            section.set_on_batch(self._batch_category)
            self._category_sections.append(section)

        # Wire checkbox changes → selection counter update
        for row in self._tweak_rows:
            row.var.trace_add("write", lambda *_: self._update_selection_count())

        # Action buttons (row 1: apply/remove)
        btn_frame = ttk.Frame(self._root)
        btn_frame.pack(fill="x", padx=16, pady=(0, 4))

        ttk.Button(
            btn_frame,
            text="▶  Apply Selected",
            style="Apply.TButton",
            command=lambda: self._dispatch("apply"),
        ).pack(side="left", padx=(0, 6), expand=True, fill="x")

        ttk.Button(
            btn_frame,
            text="✖  Remove Selected",
            style="Remove.TButton",
            command=lambda: self._dispatch("remove"),
        ).pack(side="left", padx=(0, 6), expand=True, fill="x")

        # Action buttons (row 2: snapshot / restore / restore-point)
        btn2 = ttk.Frame(self._root)
        btn2.pack(fill="x", padx=16, pady=(0, 4))

        ttk.Button(btn2, text="💾  Save Snapshot", style="Snap.TButton", command=self._save_snapshot).pack(
            side="left",
            padx=(0, 6),
            expand=True,
            fill="x",
        )
        ttk.Button(btn2, text="⏪  Restore Snapshot", style="Snap.TButton", command=self._restore_snapshot).pack(
            side="left",
            padx=(0, 6),
            expand=True,
            fill="x",
        )
        ttk.Button(
            btn2,
            text="🛡  Restore Point",
            style="Restore.TButton",
            command=lambda: self._dispatch("restore"),
        ).pack(side="left", padx=(0, 6), expand=True, fill="x")

        # Export as PowerShell script
        ttk.Button(
            btn2,
            text="\U0001f4cb  Export PS1",
            style="Snap.TButton",
            command=self._export_powershell,
        ).pack(side="left", expand=True, fill="x")

        # Action buttons (row 3: import / log / about)
        btn3 = ttk.Frame(self._root)
        btn3.pack(fill="x", padx=16, pady=(0, 4))

        ttk.Button(btn3, text="\U0001f4c2  Import JSON", command=self._import_json_selection).pack(
            side="left",
            padx=(0, 6),
            expand=True,
            fill="x",
        )
        ttk.Button(btn3, text="\U0001f4e6  Scoop Manager", command=self._open_scoop_manager).pack(
            side="left",
            padx=(0, 6),
            expand=True,
            fill="x",
        )
        ttk.Button(btn3, text="\U0001f4dc  Toggle Log", command=self._toggle_log_panel).pack(
            side="left",
            padx=(0, 6),
            expand=True,
            fill="x",
        )
        ttk.Button(btn3, text="\u2139  About", command=self._show_about).pack(
            side="left",
            expand=True,
            fill="x",
        )

        # Progress + status bar
        status_frame = ttk.Frame(self._root)
        status_frame.pack(fill="x", padx=16, pady=(0, 4))

        self._progress = ttk.Progressbar(status_frame, orient="horizontal", mode="determinate")
        self._progress.pack(fill="x", pady=(4, 2))

        # Summary stats bar
        self._stats_frame = tk.Frame(status_frame, bg=_BG)
        self._stats_frame.pack(fill="x", pady=(2, 2))
        self._stat_applied = tk.Label(self._stats_frame, text="● 0 Applied", fg=_OK_GREEN, bg=_BG, font=_FONT_XS)
        self._stat_applied.pack(side="left", padx=(0, 12))
        self._stat_default = tk.Label(self._stats_frame, text="● 0 Default", fg=_STATUS_DEFAULT, bg=_BG, font=_FONT_XS)
        self._stat_default.pack(side="left", padx=(0, 12))
        self._stat_unknown = tk.Label(self._stats_frame, text="● 0 Unknown", fg=_WARN_YELLOW, bg=_BG, font=_FONT_XS)
        self._stat_unknown.pack(side="left", padx=(0, 12))
        self._stat_rec = tk.Label(self._stats_frame, text="★ 0 Recommended", fg=_TEAL, bg=_BG, font=_FONT_XS)
        self._stat_rec.pack(side="left", padx=(0, 12))
        self._stat_gpo = tk.Label(self._stats_frame, text="● 0 GPO", fg=_GPO_ORANGE, bg=_BG, font=_FONT_XS)
        self._stat_gpo.pack(side="left", padx=(0, 12))
        self._stat_blocked: tk.Label | None = None
        if self._corp_blocked:
            self._stat_blocked = tk.Label(self._stats_frame, text="● 0 Blocked", fg=_ERR_RED, bg=_BG, font=_FONT_XS)
            self._stat_blocked.pack(side="left", padx=(0, 12))

        self._status_label = ttk.Label(
            status_frame,
            text=f"Ready  \u2022  {total_tweaks} tweaks in {total_cats} categories  \u2022  Log: {SESSION.log_path}",
            style="Status.TLabel",
        )
        self._status_label.pack(fill="x")

        # Log viewer panel (hidden by default)
        self._log_visible = False
        self._log_frame = ttk.Frame(self._root)
        self._log_text = tk.Text(
            self._log_frame,
            bg="#11111B",
            fg=_FG,
            font=("Cascadia Code", 9),
            height=8,
            wrap="word",
            state="disabled",
            relief="flat",
            insertbackground=_FG,
            selectbackground=_ACCENT,
        )
        _log_scroll = ttk.Scrollbar(self._log_frame, orient="vertical", command=self._log_text.yview)
        self._log_text.configure(yscrollcommand=_log_scroll.set)
        self._log_text.pack(side="left", fill="both", expand=True)
        _log_scroll.pack(side="right", fill="y")

        # Right-click context menu for tweak rows
        self._ctx_menu = tk.Menu(self._root, tearoff=0, bg=_CARD_BG, fg=_FG, font=_FONT_SM)
        self._ctx_target: _TweakRow | None = None
        for row in self._tweak_rows:
            row.frame.bind("<Button-3>", lambda e, r=row: self._show_context_menu(e, r))  # type: ignore[misc]

    # ── Selection helpers ────────────────────────────────────────────────

    def _select_all(self) -> None:
        for row in self._tweak_rows:
            if not (self._corp_blocked and not row.td.corp_safe):
                row.var.set(True)
        self._update_selection_count()

    def _deselect_all(self) -> None:
        for row in self._tweak_rows:
            row.var.set(False)
        self._update_selection_count()

    def _collapse_all(self) -> None:
        for section in self._category_sections:
            if section.expanded:
                section.toggle()

    def _expand_all(self) -> None:
        for section in self._category_sections:
            if not section.expanded:
                section.toggle()

    def _filter_rows(self) -> None:
        """Show/hide rows based on search query, status filter, AND scope filter."""
        query = self._search_var.get().strip()
        status_filter = self._status_filter_var.get()
        scope_filter = self._scope_filter_var.get()
        _filter_status = {"Applied": "applied", "Default": "not applied", "Unknown": "unknown"}
        _filter_scope = {"User Only": "user", "Machine Only": "machine", "Both": "both"}

        # Use search_tweaks() for indexed text matching (faster on large tweak sets)
        matching_ids: set[str] | None = None
        if query:
            matching_ids = {td.id for td in search_tweaks(query)}

        for section in self._category_sections:
            visible = False
            target_status = _filter_status.get(status_filter, "")
            target_scope = _filter_scope.get(scope_filter, "")
            for row in section.rows:
                td = row.td
                text_match = matching_ids is None or td.id in matching_ids
                status_match = status_filter == "All" or tweak_status(td) == target_status
                scope_match = scope_filter == "All" or tweak_scope(td) == target_scope
                if text_match and status_match and scope_match:
                    row.pack_row()
                    visible = True
                else:
                    row.unpack_row()
            if visible:
                section.header.pack(fill="x", pady=(8, 0), padx=4)
                if section.expanded:
                    section.content_frame.pack(fill="x")
            else:
                section.header.pack_forget()
                section.content_frame.pack_forget()

    def _update_selection_count(self) -> None:
        """Update the selection counter in the toolbar."""
        count = sum(1 for r in self._tweak_rows if r.var.get())
        self._sel_count_label.configure(text=f"{count} selected")

    def _set_status(self, text: str, color: str = _FG_DIM) -> None:
        self._status_label.configure(text=text, foreground=color)

    def _selected_tweaks(self) -> list[TweakDef]:
        return [r.td for r in self._tweak_rows if r.var.get()]

    # ── Invert selection ─────────────────────────────────────────────────

    def _invert_selection(self) -> None:
        """Toggle the selection state of every tweak row."""
        for row in self._tweak_rows:
            if not row.disabled_by_corp:
                row.var.set(not row.var.get())
        self._update_selection_count()

    # ── Log panel ────────────────────────────────────────────────────────

    def _toggle_log_panel(self) -> None:
        """Show/hide the log viewer panel at the bottom."""
        if self._log_visible:
            self._log_frame.pack_forget()
            self._log_visible = False
        else:
            self._log_frame.pack(fill="both", padx=16, pady=(0, 4), expand=False)
            self._log_visible = True
            self._refresh_log()

    def _refresh_log(self) -> None:
        """Load the session log file into the log text widget."""
        log_path = SESSION.log_path
        content = ""
        try:
            with open(log_path, encoding="utf-8", errors="replace") as f:
                content = f.read()
        except OSError:
            content = f"(Could not read log file: {log_path})"
        self._log_text.configure(state="normal")
        self._log_text.delete("1.0", "end")
        self._log_text.insert("1.0", content)
        self._log_text.configure(state="disabled")
        self._log_text.see("end")

    # ── Right-click context menu ─────────────────────────────────────────

    def _show_context_menu(self, event: tk.Event[tk.Misc], row: _TweakRow) -> None:
        """Display a context menu for a tweak row."""
        self._ctx_target = row
        self._ctx_menu.delete(0, "end")
        td = row.td
        st = tweak_status(td)

        if st == "applied":
            self._ctx_menu.add_command(label="Disable this tweak", command=lambda: self._toggle_single(row))
        else:
            self._ctx_menu.add_command(label="Enable this tweak", command=lambda: self._toggle_single(row))

        self._ctx_menu.add_separator()
        self._ctx_menu.add_command(label=f"Copy ID: {td.id}", command=lambda: self._copy_to_clipboard(td.id))
        if td.registry_keys:
            self._ctx_menu.add_command(
                label="Copy Registry Key",
                command=lambda: self._copy_to_clipboard(td.registry_keys[0]),
            )
        self._ctx_menu.add_separator()
        self._ctx_menu.add_command(
            label="Select" if not row.var.get() else "Deselect",
            command=lambda: row.var.set(not row.var.get()),
        )
        self._ctx_menu.add_command(label="Select all in category", command=lambda: self._select_category(td.category))

        try:
            self._ctx_menu.tk_popup(event.x_root, event.y_root)
        finally:
            self._ctx_menu.grab_release()

    def _copy_to_clipboard(self, text: str) -> None:
        """Copy text to system clipboard."""
        self._root.clipboard_clear()
        self._root.clipboard_append(text)
        self._set_status(f"Copied: {text}", _ACCENT)

    def _select_category(self, category: str) -> None:
        """Select all tweaks in the given category."""
        for row in self._tweak_rows:
            if row.td.category == category and not row.disabled_by_corp:
                row.var.set(True)
        self._update_selection_count()

    # ── Import JSON ──────────────────────────────────────────────────────

    def _import_json_selection(self) -> None:
        """Import a JSON file containing a list of tweak IDs to select."""
        import json

        path = filedialog.askopenfilename(
            title="Import Tweak Selection",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")],
        )
        if not path:
            return
        try:
            with open(path, encoding="utf-8") as f:
                data = json.load(f)
        except (OSError, json.JSONDecodeError) as exc:
            messagebox.showerror("Import Error", str(exc))
            return

        if isinstance(data, dict):
            ids = set(data.get("tweaks", []))
        elif isinstance(data, list):
            ids = set(data)
        else:
            messagebox.showerror("Import Error", 'Expected a JSON list of tweak IDs or {"tweaks": [...]}.')
            return

        self._deselect_all()
        count = 0
        for row in self._tweak_rows:
            if row.td.id in ids and not row.disabled_by_corp:
                row.var.set(True)
                count += 1
        self._update_selection_count()
        self._set_status(f"Imported {count} tweaks from {Path(path).name}", _OK_GREEN)

    # ── Scoop Tools Manager ────────────────────────────────────────────

    def _open_scoop_manager(self) -> None:
        """Open a Scoop Tools manager dialog showing installed packages with install/remove."""
        from .tweaks.scoop_tools import _install_scoop_app, _remove_scoop_app, _scoop_installed, list_installed_scoop_apps

        dlg = tk.Toplevel(self._root)
        dlg.title("Scoop Tools Manager")
        dlg.geometry("620x520")
        dlg.configure(bg=_BG)
        dlg.transient(self._root)
        dlg.grab_set()

        # Header
        tk.Label(dlg, text="\U0001f4e6  Scoop Tools Manager", bg=_BG, fg=_FG, font=_FONT_TITLE).pack(
            padx=16,
            pady=(12, 4),
            anchor="w",
        )

        scoop_ok = _scoop_installed()
        if not scoop_ok:
            tk.Label(
                dlg,
                text="Scoop is not installed. Install via 'scoop-install' tweak first.",
                bg=_BG,
                fg=_ERR_RED,
                font=_FONT_BOLD,
            ).pack(padx=16, pady=8)
            return

        # Status label
        status_lbl = tk.Label(dlg, text="Loading installed packages...", bg=_BG, fg=_FG_DIM, font=_FONT_SM)
        status_lbl.pack(padx=16, pady=4, anchor="w")

        # Installed packages list
        list_frame = tk.Frame(dlg, bg=_BG)
        list_frame.pack(fill="both", expand=True, padx=16, pady=4)

        listbox = tk.Listbox(
            list_frame,
            bg=_CARD_BG,
            fg=_FG,
            font=_FONT,
            selectbackground=_ACCENT,
            selectforeground="#1E1E2E",
            relief="flat",
            highlightthickness=0,
        )
        scroll = ttk.Scrollbar(list_frame, orient="vertical", command=listbox.yview)
        listbox.configure(yscrollcommand=scroll.set)
        listbox.pack(side="left", fill="both", expand=True)
        scroll.pack(side="right", fill="y")

        def _refresh_list() -> None:
            listbox.delete(0, "end")
            status_lbl.configure(text="Scanning installed packages...")
            dlg.update()
            apps = list_installed_scoop_apps()
            for app in apps:
                listbox.insert("end", app)
            status_lbl.configure(text=f"{len(apps)} packages installed")

        # Install / Remove / Search controls
        ctrl = tk.Frame(dlg, bg=_BG)
        ctrl.pack(fill="x", padx=16, pady=(4, 8))

        install_var = tk.StringVar()
        tk.Label(ctrl, text="Package:", bg=_BG, fg=_FG, font=_FONT_SM).pack(side="left")
        entry = ttk.Entry(ctrl, textvariable=install_var, font=_FONT, width=20)
        entry.pack(side="left", padx=(4, 4))

        def _install_action() -> None:
            name = install_var.get().strip()
            if not name:
                return
            status_lbl.configure(text=f"Installing {name}...")
            dlg.update()
            try:
                _install_scoop_app(name)
                status_lbl.configure(text=f"Installed {name} \u2714")
                _refresh_list()
                self._refresh_status_all()
            except RuntimeError as exc:
                messagebox.showerror("Install Error", str(exc))

        def _remove_action() -> None:
            sel = listbox.curselection()  # type: ignore[no-untyped-call]
            if not sel:
                messagebox.showinfo("Select Package", "Select a package to remove.")
                return
            name = listbox.get(sel[0])
            if not messagebox.askyesno("Confirm Remove", f"Remove '{name}'?"):
                return
            status_lbl.configure(text=f"Removing {name}...")
            dlg.update()
            _remove_scoop_app(name)
            status_lbl.configure(text=f"Removed {name} \u2714")
            _refresh_list()
            self._refresh_status_all()

        tk.Button(
            ctrl,
            text="Install",
            bg="#40A02B",
            fg="white",
            font=_FONT_XS_BOLD,
            relief="flat",
            padx=8,
            command=lambda: threading.Thread(target=_install_action, daemon=True).start(),
        ).pack(side="left", padx=2)
        tk.Button(
            ctrl,
            text="Remove Selected",
            bg="#E64A19",
            fg="white",
            font=_FONT_XS_BOLD,
            relief="flat",
            padx=8,
            command=lambda: threading.Thread(target=_remove_action, daemon=True).start(),
        ).pack(side="left", padx=2)
        tk.Button(
            ctrl,
            text="\u21bb Refresh",
            bg=_CARD_BG,
            fg=_FG,
            font=_FONT_XS_BOLD,
            relief="flat",
            padx=8,
            command=lambda: threading.Thread(target=_refresh_list, daemon=True).start(),
        ).pack(side="left", padx=2)

        # Quick install popular tools
        pop_frame = tk.LabelFrame(dlg, text="Quick Install Popular Tools", bg=_BG, fg=_FG_DIM, font=_FONT_XS)
        pop_frame.pack(fill="x", padx=16, pady=(0, 8))
        popular = ["7zip", "git", "ripgrep", "fd", "bat", "fzf", "jq", "gsudo", "neovim", "starship"]
        for i, tool in enumerate(popular):
            tk.Button(
                pop_frame,
                text=tool,
                bg=_CARD_BG,
                fg=_ACCENT,
                font=_FONT_XS,
                relief="flat",
                padx=4,
                pady=1,
                cursor="hand2",
                command=lambda t=tool: install_var.set(t),  # type: ignore[misc]
            ).grid(row=i // 5, column=i % 5, padx=2, pady=2, sticky="ew")

        threading.Thread(target=_refresh_list, daemon=True).start()

    # ── About dialog ─────────────────────────────────────────────────────

    def _show_about(self) -> None:
        """Show an About dialog with system and project info."""
        total = len(all_tweaks())
        cats = len(tweaks_by_category())
        corp = "Yes" if self._corp_blocked else "No"
        info_lines = [
            f"RegiLattice  v{__version__}",
            "",
            f"Tweaks: {total}  |  Categories: {cats}",
            f"Platform: {platform_summary()}",
            f"Corporate: {corp}",
            f"Python: {sys.version.split()[0]}",
            "",
            f"Log: {SESSION.log_path}",
            "",
            "Keyboard Shortcuts:",
            "  Ctrl+A  Select All",
            "  Ctrl+D  Deselect All",
            "  Ctrl+I  Invert Selection",
            "  Ctrl+F  Focus Search",
            "  Ctrl+E  Expand All",
            "  Ctrl+L  Toggle Log Panel",
            "  Ctrl+R  Refresh Status",
            "  Esc     Clear Search",
        ]
        messagebox.showinfo("About RegiLattice", "\n".join(info_lines))

    # ── Category batch actions ───────────────────────────────────────────

    def _batch_category(self, section: _CategorySection, mode: str) -> None:
        """Enable All / Disable All for a single category."""
        tweaks = [r.td for r in section.rows if not r.disabled_by_corp]
        if not tweaks:
            return
        verb = "Apply" if mode == "apply" else "Remove"
        if not messagebox.askyesno(
            "Confirm",
            f"{verb} all {len(tweaks)} tweak(s) in '{section.name}'?",
        ):
            return
        threading.Thread(
            target=self._run_tweaks,
            args=(tweaks, mode),
            daemon=True,
        ).start()

    # ── Status refresh ───────────────────────────────────────────────────

    def _refresh_status_all(self) -> None:
        """Re-detect every tweak and update dots, labels, counts, and stats.

        Uses parallel detection via thread-pool for faster refresh.
        """
        # Batch-detect all statuses in parallel for speed
        statuses = status_map(parallel=True, max_workers=8)
        applied = 0
        default = 0
        unknown = 0
        blocked = 0
        for row in self._tweak_rows:
            if row.disabled_by_corp:
                blocked += 1
                continue
            st = statuses.get(row.td.id, "unknown")
            # Update row directly from the cached status
            if st == "applied":
                colour = _STATUS_APPLIED
                text = "APPLIED"
                btn_text = "Disable \u2715"
                btn_bg = "#40543F"
                btn_fg = _OK_GREEN
                applied += 1
            elif st == "not applied":
                colour = _STATUS_DEFAULT
                text = "DEFAULT"
                btn_text = "Enable \u2713"
                btn_bg = "#3B3552"
                btn_fg = _ACCENT
                default += 1
            else:
                colour = _STATUS_UNKNOWN
                text = "UNKNOWN"
                btn_text = "Enable \u2713"
                btn_bg = "#3B3830"
                btn_fg = _WARN_YELLOW
                unknown += 1
            row.status_dot.configure(fg=colour)
            row.status_text.configure(text=text, fg=colour)
            row.toggle_btn.configure(text=btn_text, bg=btn_bg, fg=btn_fg)
            row.tooltip.update_text(_build_tooltip_text(row.td, st))
        for section in self._category_sections:
            section.update_count()
        # Update summary stats
        rec_count = sum(1 for r in self._tweak_rows if _has_recommendation(r.td))
        gpo_count = sum(1 for r in self._tweak_rows if r.td.registry_keys and is_gpo_managed(r.td.registry_keys))
        self._stat_applied.configure(text=f"\u25cf {applied} Applied")
        self._stat_default.configure(text=f"\u25cf {default} Default")
        self._stat_unknown.configure(text=f"\u25cf {unknown} Unknown")
        self._stat_rec.configure(text=f"\u2605 {rec_count} Recommended")
        self._stat_gpo.configure(text=f"\u25cf {gpo_count} GPO")
        if self._stat_blocked is not None:
            self._stat_blocked.configure(text=f"\u25cf {blocked} Blocked")

    # ── Individual toggle ────────────────────────────────────────────────

    def _toggle_single(self, row: _TweakRow) -> None:
        """Toggle a single tweak: apply if not applied, remove if applied."""
        if not self._force_var.get():
            try:
                assert_not_corporate()
            except CorporateNetworkError as exc:
                messagebox.showwarning("Corporate Network", str(exc))
                return

        td = row.td
        st = tweak_status(td)
        action = "remove" if st == "applied" else "apply"
        verb = "Disable" if action == "remove" else "Enable"

        self._set_status(f"{verb}: {td.label}…", _ACCENT)
        row.toggle_btn.configure(text="⏳…", state="disabled")

        def _worker() -> None:
            try:
                fn = td.apply_fn if action == "apply" else td.remove_fn
                fn()
                self._root.after(
                    0,
                    self._set_status,
                    f"{td.label} — {'enabled' if action == 'apply' else 'disabled'} ✔",
                    _OK_GREEN,
                )
            except AdminRequirementError:
                self._root.after(0, self._set_status, f"{td.label}: admin required", _ERR_RED)
            except (OSError, RuntimeError, ValueError) as exc:
                SESSION.log(f"[GUI] Toggle error {td.label}: {exc}")
                self._root.after(0, self._set_status, f"{td.label}: {exc}", _ERR_RED)
            finally:
                self._root.after(0, row.refresh_status)
                self._root.after(0, lambda: row.toggle_btn.configure(state="normal"))
                # Update the category count after toggling
                for section in self._category_sections:
                    if section.name == td.category:
                        self._root.after(0, section.update_count)

        threading.Thread(target=_worker, daemon=True).start()

    # ── Profile selection ────────────────────────────────────────────────

    def _apply_profile_selection(self, profile_name: str) -> None:
        """Select tweaks matching the chosen profile."""
        self._deselect_all()
        if profile_name == "(none)":
            return
        key = profile_name.lower()
        info = profile_info(key)
        if info is None:
            return
        raw_cats = info.get("apply_categories")
        apply_cats: set[str] = set()
        if isinstance(raw_cats, (set, frozenset, list, tuple)):
            apply_cats = {str(c).lower() for c in raw_cats}
        for row in self._tweak_rows:
            if row.td.category.lower() in apply_cats and not row.disabled_by_corp:
                row.var.set(True)
        self._update_selection_count()
        count = sum(1 for r in self._tweak_rows if r.var.get())
        self._set_status(f"Profile '{profile_name}' selected ({count} tweaks)", _ACCENT)

    # ── Export as PowerShell ─────────────────────────────────────────────

    def _export_powershell(self) -> None:
        """Export selected tweaks as a .ps1 script showing the registry changes."""
        selected = self._selected_tweaks()
        if not selected:
            messagebox.showinfo("Nothing Selected", "Select at least one tweak to export.")
            return
        path = filedialog.asksaveasfilename(
            title="Export PowerShell Script",
            defaultextension=".ps1",
            filetypes=[("PowerShell", "*.ps1"), ("All files", "*.*")],
            initialfile="regilattice_tweaks.ps1",
        )
        if not path:
            return
        lines = [
            "# RegiLattice — Exported Tweaks",
            f"# Generated from RegiLattice v{__version__}",
            f"# Tweaks: {len(selected)}",
            "#",
            "# Run this script in an elevated PowerShell session.",
            "# WARNING: Modifying the registry can cause system instability.",
            "",
            "#Requires -RunAsAdministrator",
            "",
        ]
        for td in selected:
            lines.append(f"# ── {td.label} ({'admin' if td.needs_admin else 'user'}) ──")
            if td.description:
                lines.append(f"# {td.description}")
            for key in td.registry_keys:
                # Convert HKEY_ prefix to PS drive format
                ps_key = key.replace("HKEY_LOCAL_MACHINE", "HKLM:")
                ps_key = ps_key.replace("HKEY_CURRENT_USER", "HKCU:")
                lines.append(f"# Registry key: {ps_key}")
            lines.append(f"Write-Host 'Applying: {td.label}...'")
            lines.append("")
        lines.append("Write-Host 'Done! All tweaks applied.' -ForegroundColor Green")
        try:
            with open(path, "w", encoding="utf-8-sig") as f:
                f.write("\n".join(lines))
            self._set_status(f"Exported {len(selected)} tweaks → {path}", _OK_GREEN)
        except OSError as exc:
            messagebox.showerror("Export Error", str(exc))

    # ── Snapshot ─────────────────────────────────────────────────────────

    def _save_snapshot(self) -> None:
        path = filedialog.asksaveasfilename(
            title="Save Tweak Snapshot",
            defaultextension=".json",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")],
            initialfile="regilattice_snapshot.json",
        )
        if not path:
            return
        try:
            save_snapshot(Path(path))
            self._set_status(f"Snapshot saved → {path}", _OK_GREEN)
        except OSError as exc:
            messagebox.showerror("Save Error", str(exc))

    def _restore_snapshot(self) -> None:
        path = filedialog.askopenfilename(
            title="Open Tweak Snapshot",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")],
        )
        if not path:
            return

        if not messagebox.askyesno(
            "Confirm Restore",
            "This will revert tweaks to the state saved in the snapshot. Continue?",
        ):
            return

        self._set_status("Restoring snapshot…", _WARN_YELLOW)

        def _worker() -> None:
            try:
                results = restore_snapshot(Path(path), force_corp=self._force_var.get())
                summary_parts: list[str] = []
                for action in set(results.values()):
                    count = sum(1 for v in results.values() if v == action)
                    summary_parts.append(f"{count} {action}")
                summary = "Restore: " + ", ".join(summary_parts)
                self._root.after(0, self._set_status, summary, _OK_GREEN)
                self._root.after(0, self._refresh_status_all)
            except (OSError, ValueError, RuntimeError) as exc:
                _err = str(exc)
                self._root.after(0, lambda: messagebox.showerror("Restore Error", _err))

        threading.Thread(target=_worker, daemon=True).start()

    # ── Dispatch ─────────────────────────────────────────────────────────

    def _dispatch(self, mode: str) -> None:
        """Run tweaks in a background thread to keep the UI responsive."""
        if not self._force_var.get():
            try:
                assert_not_corporate()
            except CorporateNetworkError as exc:
                messagebox.showwarning("Corporate Network Detected", str(exc))
                return

        if mode == "restore":
            self._set_status("Creating restore point…", _WARN_YELLOW)
            threading.Thread(target=self._run_restore_point, daemon=True).start()
            return

        selected = self._selected_tweaks()
        if not selected:
            messagebox.showinfo("Nothing Selected", "Select at least one tweak.")
            return

        confirm_msg = f"{'Apply' if mode == 'apply' else 'Remove'} {len(selected)} selected tweak(s)?"
        if not messagebox.askyesno("Confirm", confirm_msg):
            return

        threading.Thread(target=self._run_tweaks, args=(selected, mode), daemon=True).start()

    def _run_tweaks(self, items: list[TweakDef], mode: str) -> None:
        total = len(items)
        errors: list[str] = []
        for i, td in enumerate(items, 1):
            pct = int(i / total * 100)
            self._root.after(0, self._progress.configure, {"value": pct})
            self._root.after(
                0,
                self._set_status,
                f"{'Applying' if mode == 'apply' else 'Removing'}: {td.label}  ({i}/{total})",
                _ACCENT,
            )
            try:
                fn = td.apply_fn if mode == "apply" else td.remove_fn
                fn()
            except AdminRequirementError:
                errors.append(f"{td.label}: requires admin elevation")
            except (OSError, RuntimeError, ValueError) as exc:
                errors.append(f"{td.label}: {exc}")
                SESSION.log(f"[GUI] Error ({mode}) {td.label}: {exc}")

        ok_count = total - len(errors)
        summary = f"{'Applied' if mode == 'apply' else 'Removed'} {ok_count}/{total} tweaks"
        if errors:
            summary += f"  •  {len(errors)} error(s)"
        colour = _OK_GREEN if not errors else _WARN_YELLOW

        self._root.after(0, self._set_status, summary, colour)
        self._root.after(0, self._progress.configure, {"value": 100})
        self._root.after(0, self._refresh_status_all)

        if errors:
            err_text = "\n".join(errors)
            self._root.after(
                0,
                lambda: messagebox.showwarning("Completed with Errors", f"{summary}\n\n{err_text}"),
            )

    def _run_restore_point(self) -> None:
        try:
            create_restore_point()
            self._root.after(0, self._set_status, "Restore point created ✔", _OK_GREEN)
        except AdminRequirementError:
            self._root.after(
                0,
                lambda: messagebox.showwarning(
                    "Admin Required",
                    "Creating a restore point requires admin elevation.",
                ),
            )
            self._root.after(0, self._set_status, "Restore point failed (admin)", _ERR_RED)
        except (OSError, RuntimeError) as exc:
            err_msg = str(exc)
            self._root.after(0, lambda: messagebox.showerror("Error", err_msg))
            self._root.after(0, self._set_status, f"Restore point error: {err_msg}", _ERR_RED)

    # ── Entry point ──────────────────────────────────────────────────────

    def run(self) -> None:
        """Start the main event loop."""
        self._root.mainloop()


def launch() -> None:
    """Convenience entry point used by CLI ``--gui``."""
    if not is_windows():
        print(f"⚠️  RegiLattice GUI requires Windows. Detected: {platform_summary()}")
        sys.exit(1)
    app = RegiLatticeGUI()
    app.run()
