"""Tweak-row and collapsible category-section widgets for RegiLattice GUI."""

from __future__ import annotations

import tkinter as tk
from collections.abc import Callable
from tkinter import ttk

from . import gui_theme as theme
from .corpguard import is_gpo_managed
from .gui_tooltip import Tooltip, build_tooltip_text, has_recommendation
from .tweaks import TweakDef, TweakResult, category_info, tweak_scope, tweak_status

__all__ = [
    "CategorySection",
    "TweakRow",
]

# ── Theme aliases ────────────────────────────────────────────────────────────

_ACCENT = theme.ACCENT
_BG_SURFACE = theme.BG_SURFACE
_FG = theme.FG
_FG_DIM = theme.FG_DIM
_OK_GREEN = theme.OK_GREEN
_WARN_YELLOW = theme.WARN_YELLOW
_ERR_RED = theme.ERR_RED
_PURPLE = theme.PURPLE
_GPO_ORANGE = theme.GPO_ORANGE
_TEAL = theme.TEAL

_STATUS_APPLIED = theme.STATUS_APPLIED
_STATUS_UNKNOWN = theme.STATUS_UNKNOWN
_STATUS_CORP_BLOCKED = theme.STATUS_CORP_BLOCKED
_STATUS_DEFAULT = theme.STATUS_DEFAULT

_FONT_XS = theme.FONT_XS
_FONT_XS_BOLD = theme.FONT_XS_BOLD
_FONT_CAT = theme.FONT_CAT


# ── Row widget ───────────────────────────────────────────────────────────────


class TweakRow:
    """Single tweak row: status dot + status text + checkbox + toggle button + tooltip."""

    def __init__(
        self,
        parent: ttk.Frame,
        td: TweakDef,
        *,
        corp_blocked: bool,
        on_toggle: Callable[[TweakRow], None] | None = None,
        defer_widgets: bool = False,
    ) -> None:
        self.td = td
        self.var = tk.BooleanVar(value=False)
        self._corp_blocked = corp_blocked
        self._on_toggle = on_toggle
        self.disabled_by_corp = corp_blocked and not td.corp_safe
        self._odd = False  # Set by CategorySection for zebra striping

        # Placeholder attributes — populated by build_widgets()
        self.frame: ttk.Frame | None = None
        self.status_dot: tk.Label | None = None
        self.status_text: tk.Label | None = None
        self.cb: ttk.Checkbutton | None = None
        self.toggle_btn: tk.Button | None = None
        self.tooltip: Tooltip | None = None

        if not defer_widgets:
            self.build_widgets(parent)

    def _row_bg(self) -> str:
        """Return the current background colour for this row (zebra-aware)."""
        return theme.CARD_BG_ALT if self._odd else theme.CARD_BG

    def build_widgets(self, parent: ttk.Frame) -> None:
        """Create (or recreate) all child widgets under *parent*."""
        _frame_style = "CardAlt.TFrame" if self._odd else "Card.TFrame"
        self.frame = ttk.Frame(parent, style=_frame_style)
        td = self.td
        _bg = self._row_bg()

        # Status dot
        self.status_dot = tk.Label(
            self.frame,
            text="\u25cf",
            fg=_STATUS_UNKNOWN,
            bg=_bg,
            font=("Segoe UI", 12),
            width=2,
        )
        self.status_dot.pack(side="left", padx=(6, 0))

        # Status text label (APPLIED / DEFAULT / UNKNOWN)
        self.status_text = tk.Label(
            self.frame,
            text="\u2026",
            fg=_STATUS_UNKNOWN,
            bg=_bg,
            font=_FONT_XS_BOLD,
            width=10,
            anchor="w",
        )
        self.status_text.pack(side="left", padx=(2, 4))

        # Checkbox for batch selection
        state = "disabled" if self.disabled_by_corp else "normal"
        _cb_style = "Alt.TCheckbutton" if self._odd else "TCheckbutton"
        self.cb = ttk.Checkbutton(
            self.frame,
            text=td.label,
            variable=self.var,
            state=state,
            style=_cb_style,
        )
        self.cb.pack(side="left", padx=(4, 4), pady=2)

        # Toggle button (individual enable/disable)
        self.toggle_btn = tk.Button(
            self.frame,
            text="\u23f3",
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
                bg=_bg,
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
                bg=_bg,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))
        if td.registry_keys and is_gpo_managed(td.registry_keys):
            tk.Label(
                self.frame,
                text="GPO",
                fg=_GPO_ORANGE,
                bg=_bg,
                font=("Segoe UI", 7, "bold"),
            ).pack(side="right", padx=(0, 4))
        if td.needs_admin:
            tk.Label(
                self.frame,
                text="ADMIN",
                fg=_FG_DIM,
                bg=_bg,
                font=("Segoe UI", 7),
            ).pack(side="right", padx=(0, 4))
        if has_recommendation(td):
            tk.Label(
                self.frame,
                text="\u2605 REC",
                fg=_TEAL,
                bg=_bg,
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
            bg=_bg,
            font=("Segoe UI", 7, "bold"),
        ).pack(side="right", padx=(0, 4))

        # Tooltip — detailed description with state, options, recommendation (lazy)
        self.tooltip = Tooltip(self.frame, text_fn=lambda: build_tooltip_text(td, TweakResult.UNKNOWN))

        # Hover highlight effect
        self.frame.bind("<Enter>", self._on_enter)
        self.frame.bind("<Leave>", self._on_leave)

    def _on_enter(self, _: tk.Event[tk.Misc]) -> None:
        """Highlight row background on hover."""
        if self.frame is None:
            return
        for w in self.frame.winfo_children():
            if isinstance(w, tk.Label):
                w.configure(bg=theme.CARD_HOVER, fg=theme.FG)

    def _on_leave(self, _: tk.Event[tk.Misc]) -> None:
        """Restore row background on leave."""
        if self.frame is None:
            return
        bg = self._row_bg()
        for w in self.frame.winfo_children():
            if isinstance(w, tk.Label):
                w.configure(bg=bg)

    def apply_theme(self) -> None:
        """Re-apply current theme colours to all child widgets."""
        if self.frame is None:
            return
        bg = self._row_bg()
        for w in self.frame.winfo_children():
            if isinstance(w, tk.Label):
                w.configure(bg=bg)
        if not self.disabled_by_corp and self.toggle_btn is not None:
            self.toggle_btn.configure(bg=bg)

    def on_toggle_click(self) -> None:
        if self._on_toggle:
            self._on_toggle(self)

    def mark_corp_blocked(self) -> None:
        """Retroactively disable this row when corporate detection completes after row creation."""
        self.disabled_by_corp = True
        self._corp_blocked = True
        self.var.set(False)
        if self.cb is not None:
            self.cb.configure(state="disabled")
        if self.toggle_btn is not None:
            self.toggle_btn.configure(text="BLOCKED", bg=self._row_bg(), fg=_ERR_RED, state="disabled")
        if self.status_dot is not None:
            self.status_dot.configure(fg=_STATUS_CORP_BLOCKED)
        if self.status_text is not None:
            self.status_text.configure(text="BLOCKED", fg=_STATUS_CORP_BLOCKED)

    def pack_row(self) -> None:
        """Pack the row frame into its parent."""
        if self.frame is None:
            return
        self.frame.pack(fill="x", padx=4, pady=2, ipady=3)

    def unpack_row(self) -> None:
        """Remove from display."""
        if self.frame is None:
            return
        self.frame.pack_forget()

    def refresh_status(self) -> None:
        """Update the status dot, text label, toggle button, and tooltip."""
        assert self.status_dot is not None
        assert self.status_text is not None
        assert self.toggle_btn is not None
        assert self.tooltip is not None
        st = tweak_status(self.td)
        if self.disabled_by_corp:
            colour = _STATUS_CORP_BLOCKED
            text = "BLOCKED"
            btn_text = "BLOCKED"
            btn_bg = self._row_bg()
            btn_fg = _ERR_RED
        elif st == TweakResult.APPLIED:
            colour = _STATUS_APPLIED
            text = "APPLIED"
            btn_text = "Disable \u2715"
            btn_bg = "#40543F"
            btn_fg = _OK_GREEN
        elif st == TweakResult.NOT_APPLIED:
            colour = _STATUS_DEFAULT
            text = "DEFAULT"
            btn_text = "Enable \u2713"
            btn_bg = "#3B3552"
            btn_fg = _ACCENT
        else:
            colour = _STATUS_UNKNOWN
            text = "UNKNOWN"
            btn_text = "Enable \u2713"
            btn_bg = "#3B3830"
            btn_fg = _WARN_YELLOW
        self.status_dot.configure(fg=colour)
        self.status_text.configure(text=text, fg=colour)
        if not self.disabled_by_corp:
            self.toggle_btn.configure(text=btn_text, bg=btn_bg, fg=btn_fg)
        # Update tooltip with current status
        self.tooltip.update_text(build_tooltip_text(self.td, st))


# ── Category section (collapsible) ───────────────────────────────────────────


class CategorySection:
    """A collapsible category section with a clickable header and tweak count."""

    def __init__(self, parent: ttk.Frame, name: str, rows: list[TweakRow], *, expanded: bool = True) -> None:
        self.name = name
        self.rows = rows
        self.expanded = True  # set False below if expanded=False
        self._parent = parent
        self._on_collapse_change: Callable[[CategorySection], None] | None = None
        self._on_rows_built: Callable[[CategorySection], None] | None = None

        # Header bar — clickable to expand/collapse
        self.header = tk.Frame(parent, bg=_BG_SURFACE, cursor="hand2")
        self.header.pack(fill="x", pady=(8, 0), padx=4)

        self._arrow = tk.Label(
            self.header,
            text="\u25bc",
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

        # Reorder buttons (wired by gui.py via set_on_reorder)
        self._btn_down = tk.Button(
            self.header,
            text="\u25bc",
            font=("Segoe UI", 7),
            relief="flat",
            bg=_BG_SURFACE,
            fg=_FG_DIM,
            cursor="hand2",
            bd=0,
            padx=2,
        )
        self._btn_down.pack(side="right", padx=(2, 0))
        self._btn_up = tk.Button(
            self.header,
            text="\u25b2",
            font=("Segoe UI", 7),
            relief="flat",
            bg=_BG_SURFACE,
            fg=_FG_DIM,
            cursor="hand2",
            bd=0,
            padx=2,
        )
        self._btn_up.pack(side="right", padx=(6, 0))

        # Bind click on all header widgets
        for w in (self.header, self._arrow, self._title, self._count_lbl):
            w.bind("<Button-1>", self.toggle)

        # Content frame — holds the tweak rows
        self.content_frame = ttk.Frame(parent, style="TFrame")

        # Track whether widgets have been built yet (lazy build on first expand).
        self._widgets_built = False

        # Start expanded or collapsed depending on caller preference.
        if expanded:
            self._build_row_widgets()  # build immediately if starting expanded
            self.content_frame.pack(fill="x")
        else:
            self.expanded = False
            self._arrow.configure(text="\u25b6")
            # content_frame and row widgets built lazily on first toggle

    def _build_row_widgets(self) -> None:
        """Build all row widgets into content_frame (idempotent).

        Called on first expand so that collapsed sections never create Tk widgets,
        keeping startup time and memory proportional to the number of *visible* rows.
        After building, fires the registered post-build callback so callers (gui.py)
        can wire keyboard bindings and context menus for the newly created widgets.
        """
        if self._widgets_built:
            return
        for i, row in enumerate(self.rows):
            row._odd = bool(i % 2)
            if row.frame is not None:
                row.frame.destroy()
            row.build_widgets(self.content_frame)
            row.pack_row()
        self._widgets_built = True
        if self._on_rows_built is not None:
            self._on_rows_built(self)

    def set_on_rows_built(self, callback: Callable[[CategorySection], None]) -> None:
        """Register a callback fired once after *_build_row_widgets* completes."""
        self._on_rows_built = callback

    def set_on_collapse_change(self, callback: Callable[[CategorySection], None]) -> None:
        """Register a callback invoked when the section is expanded or collapsed."""
        self._on_collapse_change = callback

    def toggle(self, _: tk.Event[tk.Misc] | None = None) -> None:
        self.expanded = not self.expanded
        if self.expanded:
            self._arrow.configure(text="\u25bc")
            self._build_row_widgets()  # lazy: no-op after first expand
            self._show_rows()
        else:
            self._arrow.configure(text="\u25b6")
            self._hide_rows()
        if self._on_collapse_change is not None:
            self._on_collapse_change(self)

    def _show_rows(self) -> None:
        self.content_frame.pack(fill="x", after=self.header)
        for row in self.rows:
            row.pack_row()

    def _hide_rows(self) -> None:
        # Hide the container only — rows remain packed inside and keep their
        # filter state.  Avoids O(n_rows) pack_forget calls on collapse.
        self.content_frame.pack_forget()

    def update_count(self, statuses: dict[str, TweakResult] | None = None) -> None:
        """Update the applied/total count in the header badge.

        If *statuses* is provided, uses it for O(1) lookups instead of
        calling ``tweak_status()`` per row.
        """
        if statuses is not None:
            applied = sum(1 for r in self.rows if statuses.get(r.td.id) == TweakResult.APPLIED)
        else:
            applied = sum(1 for r in self.rows if tweak_status(r.td) == TweakResult.APPLIED)
        total = len(self.rows)
        self._count_lbl.configure(text=f"  ({applied}/{total} applied)")

    def set_on_batch(self, callback: Callable[[CategorySection, str], None]) -> None:
        """Wire the per-category Enable All / Disable All buttons."""
        self._btn_enable_all.configure(command=lambda: callback(self, "apply"))
        self._btn_disable_all.configure(command=lambda: callback(self, "remove"))

    def set_on_reorder(self, callback: Callable[[CategorySection, str], None]) -> None:
        """Wire the ↑/↓ reorder buttons.  *direction* is ``'up'`` or ``'down'``."""
        self._btn_up.configure(command=lambda: callback(self, "up"))
        self._btn_down.configure(command=lambda: callback(self, "down"))

    def filter_rows(self, query: str) -> bool:
        """Show/hide rows matching query.  Returns True if any row is visible."""
        q = query.lower()
        visible = False
        for row in self.rows:
            td = row.td
            match = not q or any(q in f.lower() for f in [td.id, td.label, td.category, td.description, *td.tags])
            if match:
                # Ensure widgets exist before trying to pack them
                if not self._widgets_built:
                    self._build_row_widgets()
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

    def apply_theme(self) -> None:
        """Re-apply current theme colours to all header widgets."""
        self.header.configure(bg=theme.BG_SURFACE)
        self._arrow.configure(fg=theme.ACCENT, bg=theme.BG_SURFACE)
        self._title.configure(fg=theme.ACCENT, bg=theme.BG_SURFACE)
        self._count_lbl.configure(fg=theme.FG_DIM, bg=theme.BG_SURFACE)
        self._btn_enable_all.configure(bg=theme.BG_SURFACE, fg=theme.OK_GREEN)
        self._btn_disable_all.configure(bg=theme.BG_SURFACE, fg=theme.ERR_RED)
        self._btn_up.configure(bg=theme.BG_SURFACE, fg=theme.FG_DIM)
        self._btn_down.configure(bg=theme.BG_SURFACE, fg=theme.FG_DIM)
        # Sweep all remaining children (dynamic risk/scope/profile badge labels)
        _fixed = {self._arrow, self._title, self._count_lbl, self._btn_enable_all, self._btn_disable_all, self._btn_up, self._btn_down}
        for w in self.header.winfo_children():
            if w not in _fixed and isinstance(w, (tk.Label, tk.Button)):
                w.configure(bg=theme.BG_SURFACE)
