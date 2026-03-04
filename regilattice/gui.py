"""Tkinter GUI for RegiLattice — Windows 11 style, plugin-driven.

Launch via ``python -m regilattice --gui`` or ``regilattice --gui``.

Features:
  • Auto-discovers tweaks from ``regilattice.tweaks`` plugin package
  • Live status detection (applied / not applied / unknown)
  • Save-snapshot / restore-snapshot for undo
  • Corp-safe enforcement — non-corp-safe tweaks disabled on corp networks
  • Windows 11 dark Mica-like theme (Catppuccin Mocha palette)
"""

from __future__ import annotations

import sys
import threading
import tkinter as tk
from pathlib import Path
from tkinter import filedialog, messagebox, ttk
from typing import Callable, Dict, List, Optional, Tuple

from . import __version__
from .corpguard import (CorporateNetworkError, assert_not_corporate,
                        corp_guard_status, is_corporate_network)
from .registry import (SESSION, AdminRequirementError, is_windows,
                       platform_summary)
from .tweaks import (TweakDef, all_tweaks, load_snapshot, restore_snapshot,
                     save_snapshot, tweak_status, tweaks_by_category)
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

# Status indicator colours
_STATUS_APPLIED = _OK_GREEN
_STATUS_NOT_APPLIED = _FG_DIM
_STATUS_UNKNOWN = _WARN_YELLOW
_STATUS_CORP_BLOCKED = _ERR_RED


# ── Row widget ───────────────────────────────────────────────────────────────


class _TweakRow:
    """Single tweak row: checkbox + status badge + toggle button + tags."""

    def __init__(
        self,
        parent: ttk.Frame,
        td: TweakDef,
        *,
        corp_blocked: bool,
        on_toggle: "Callable[[_TweakRow], None] | None" = None,
    ) -> None:
        self.td = td
        self.var = tk.BooleanVar(value=False)
        self._corp_blocked = corp_blocked
        self._on_toggle = on_toggle
        self._disabled_by_corp = corp_blocked and not td.corp_safe

        self.frame = ttk.Frame(parent, style="Card.TFrame")
        self.frame.pack(fill="x", padx=4, pady=2, ipady=3)

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

        # Status text label (ENABLED / DISABLED / UNKNOWN)
        self.status_text = tk.Label(
            self.frame,
            text="…",
            fg=_STATUS_UNKNOWN,
            bg=_CARD_BG,
            font=("Segoe UI", 8, "bold"),
            width=10,
            anchor="w",
        )
        self.status_text.pack(side="left", padx=(2, 4))

        # Checkbox for batch selection
        state = "disabled" if self._disabled_by_corp else "normal"
        self.cb = ttk.Checkbutton(
            self.frame, text=td.label, variable=self.var, state=state
        )
        self.cb.pack(side="left", padx=(4, 4), pady=2)

        # Toggle button (individual enable/disable)
        self._toggle_btn = tk.Button(
            self.frame,
            text="⏳",
            font=("Segoe UI", 8, "bold"),
            width=9,
            relief="flat",
            cursor="hand2",
            bd=0,
            padx=6,
            pady=1,
        )
        if self._disabled_by_corp:
            self._toggle_btn.configure(
                text="BLOCKED",
                bg=_CARD_BG,
                fg=_ERR_RED,
                state="disabled",
            )
        else:
            self._toggle_btn.configure(command=self._on_toggle_click)
        self._toggle_btn.pack(side="right", padx=(4, 8))

        # Tags (right-aligned, before toggle button)
        if self._disabled_by_corp:
            tk.Label(
                self.frame,
                text="CORP",
                fg=_ERR_RED,
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

        # Tooltip on hover
        if td.description:
            self._bind_tooltip(td.description)

    # -- toggle handler --
    def _on_toggle_click(self) -> None:
        if self._on_toggle:
            self._on_toggle(self)

    # tooltip helpers
    def _bind_tooltip(self, text: str) -> None:
        self._tip: Optional[tk.Toplevel] = None

        def show(event: tk.Event) -> None:  # type: ignore[type-arg]
            x = event.x_root + 12
            y = event.y_root + 8
            self._tip = tw = tk.Toplevel(self.frame)
            tw.wm_overrideredirect(True)
            tw.wm_geometry(f"+{x}+{y}")
            tk.Label(
                tw,
                text=text,
                bg="#45475A",
                fg=_FG,
                font=("Segoe UI", 9),
                padx=8,
                pady=4,
                wraplength=320,
                justify="left",
            ).pack()

        def hide(_: tk.Event) -> None:  # type: ignore[type-arg]
            if self._tip:
                self._tip.destroy()
                self._tip = None

        self.frame.bind("<Enter>", show)
        self.frame.bind("<Leave>", hide)

    def refresh_status(self) -> None:
        """Update the status dot, text label, and toggle button."""
        st = tweak_status(self.td)
        if self._disabled_by_corp:
            colour = _STATUS_CORP_BLOCKED
            text = "BLOCKED"
            btn_text = "BLOCKED"
            btn_bg = _CARD_BG
            btn_fg = _ERR_RED
        elif st == "applied":
            colour = _STATUS_APPLIED
            text = "ENABLED"
            btn_text = "Disable ✕"
            btn_bg = "#40543F"
            btn_fg = _OK_GREEN
        elif st == "not applied":
            colour = _STATUS_NOT_APPLIED
            text = "DISABLED"
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
        if not self._disabled_by_corp:
            self._toggle_btn.configure(text=btn_text, bg=btn_bg, fg=btn_fg)


# ── Main GUI ─────────────────────────────────────────────────────────────────


class RegiLatticeGUI:
    """Plugin-driven main application window."""

    def __init__(self) -> None:
        self._root = tk.Tk()
        self._root.title(f"RegiLattice  v{__version__}")
        self._root.geometry("800x860")
        self._root.minsize(600, 540)
        self._root.configure(bg=_BG)
        self._root.resizable(True, True)

        try:
            self._root.iconbitmap(default="")
        except Exception:
            pass

        # Windows 11: attempt DWM dark title bar
        self._apply_win11_dark_titlebar()

        self._corp_blocked = is_corporate_network()
        self._tweak_rows: List[_TweakRow] = []
        self._setup_styles()
        self._build_ui()
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
        except Exception:
            pass  # non-Windows or unsupported build

    # ── Styles ───────────────────────────────────────────────────────────

    def _setup_styles(self) -> None:
        style = ttk.Style(self._root)
        style.theme_use("clam")

        style.configure(".", background=_BG, foreground=_FG, font=("Segoe UI", 10))
        style.configure("TFrame", background=_BG)
        style.configure("Header.TFrame", background=_HEADER_BG)
        style.configure("Card.TFrame", background=_CARD_BG)
        style.configure(
            "TCheckbutton",
            background=_CARD_BG,
            foreground=_FG,
            font=("Segoe UI", 10),
            indicatorsize=16,
        )
        style.map(
            "TCheckbutton",
            background=[("active", _CARD_BG), ("disabled", _CARD_BG)],
            foreground=[("active", _FG), ("disabled", _DIM_BG)],
        )
        style.configure("TButton", padding=(14, 6), font=("Segoe UI Semibold", 10))
        style.configure("Apply.TButton", foreground="white", background="#40A02B")
        style.map("Apply.TButton", background=[("active", "#2E7D32")])
        style.configure("Remove.TButton", foreground="white", background="#E64A19")
        style.map("Remove.TButton", background=[("active", "#BF360C")])
        style.configure("Restore.TButton", foreground="white", background="#7C3AED")
        style.map("Restore.TButton", background=[("active", "#5B21B6")])
        style.configure("Snap.TButton", foreground="white", background="#1565C0")
        style.map("Snap.TButton", background=[("active", "#0D47A1")])
        style.configure("TLabel", background=_BG, foreground=_FG, font=("Segoe UI", 10))
        style.configure(
            "Title.TLabel",
            background=_HEADER_BG,
            foreground=_FG,
            font=("Segoe UI Semibold", 16),
        )
        style.configure(
            "Subtitle.TLabel",
            background=_HEADER_BG,
            foreground=_FG_DIM,
            font=("Segoe UI", 9),
        )
        style.configure(
            "Status.TLabel",
            background=_BG,
            foreground=_FG_DIM,
            font=("Segoe UI", 9),
        )
        style.configure(
            "Category.TLabel",
            background=_BG,
            foreground=_ACCENT,
            font=("Segoe UI Semibold", 11),
        )

    # ── UI construction ──────────────────────────────────────────────────

    def _build_ui(self) -> None:
        # Header
        header = ttk.Frame(self._root, style="Header.TFrame")
        header.pack(fill="x")
        ttk.Label(header, text="⚡ RegiLattice", style="Title.TLabel").pack(
            side="left", padx=16, pady=(12, 2)
        )
        ttk.Label(
            header,
            text=f"v{__version__}  |  {platform_summary()}",
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
                font=("Segoe UI Semibold", 10),
                anchor="w",
                padx=12,
                pady=6,
            ).pack(fill="x")

        # Toolbar
        toolbar = ttk.Frame(self._root)
        toolbar.pack(fill="x", padx=16, pady=(10, 0))

        ttk.Button(toolbar, text="Select All", command=self._select_all).pack(
            side="left", padx=(0, 4)
        )
        ttk.Button(toolbar, text="Deselect All", command=self._deselect_all).pack(
            side="left", padx=(0, 4)
        )
        ttk.Button(
            toolbar, text="↻ Refresh Status", command=self._refresh_status_all
        ).pack(side="left", padx=(0, 4))
        self._force_var = tk.BooleanVar(value=False)
        ttk.Checkbutton(
            toolbar,
            text="Force (bypass corp guard)",
            variable=self._force_var,
        ).pack(side="right", padx=(8, 0))

        # Legend
        legend = ttk.Frame(self._root)
        legend.pack(fill="x", padx=16, pady=(4, 0))
        for colour, label in [
            (_STATUS_APPLIED, "Enabled"),
            (_STATUS_NOT_APPLIED, "Disabled"),
            (_STATUS_UNKNOWN, "Unknown"),
            (_STATUS_CORP_BLOCKED, "Corp Blocked"),
        ]:
            tk.Label(legend, text="●", fg=colour, bg=_BG, font=("Segoe UI", 10)).pack(
                side="left", padx=(0, 2)
            )
            tk.Label(
                legend,
                text=label,
                fg=_FG_DIM,
                bg=_BG,
                font=("Segoe UI", 8),
            ).pack(side="left", padx=(0, 10))

        # Search bar
        search_frame = ttk.Frame(self._root)
        search_frame.pack(fill="x", padx=16, pady=(6, 0))
        ttk.Label(search_frame, text="🔍", style="TLabel").pack(side="left", padx=(0, 4))
        self._search_var = tk.StringVar()
        self._search_var.trace_add("write", lambda *_: self._filter_rows())
        self._search_entry = ttk.Entry(
            search_frame, textvariable=self._search_var, font=("Segoe UI", 10)
        )
        self._search_entry.pack(side="left", fill="x", expand=True)
        ttk.Button(
            search_frame, text="✕", width=3,
            command=lambda: self._search_var.set(""),
        ).pack(side="left", padx=(4, 0))

        # Scrollable tweak list
        container = ttk.Frame(self._root)
        container.pack(fill="both", expand=True, padx=16, pady=8)

        canvas = tk.Canvas(container, bg=_BG, highlightthickness=0)
        scrollbar = ttk.Scrollbar(container, orient="vertical", command=canvas.yview)
        self._inner = ttk.Frame(canvas, style="TFrame")

        self._inner.bind(
            "<Configure>",
            lambda _: canvas.configure(scrollregion=canvas.bbox("all")),
        )
        canvas.create_window((0, 0), window=self._inner, anchor="nw")
        canvas.configure(yscrollcommand=scrollbar.set)
        canvas.pack(side="left", fill="both", expand=True)
        scrollbar.pack(side="right", fill="y")

        def _on_mousewheel(event: tk.Event) -> None:  # type: ignore[type-arg]
            canvas.yview_scroll(int(-1 * (event.delta / 120)), "units")

        canvas.bind_all("<MouseWheel>", _on_mousewheel)

        # Populate rows from plugin registry grouped by category (sorted)
        self._category_labels: List[Tuple[str, ttk.Label]] = []
        grouped = tweaks_by_category()
        for cat_name, cat_tweaks in grouped.items():
            cat_lbl = ttk.Label(
                self._inner, text=f"  {cat_name}", style="Category.TLabel"
            )
            cat_lbl.pack(fill="x", pady=(10, 2), padx=4)
            self._category_labels.append((cat_name, cat_lbl))
            for td in cat_tweaks:
                row = _TweakRow(
                    self._inner,
                    td,
                    corp_blocked=self._corp_blocked,
                    on_toggle=self._toggle_single,
                )
                self._tweak_rows.append(row)

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

        ttk.Button(
            btn2,
            text="💾  Save Snapshot",
            style="Snap.TButton",
            command=self._save_snapshot,
        ).pack(side="left", padx=(0, 6), expand=True, fill="x")

        ttk.Button(
            btn2,
            text="⏪  Restore Snapshot",
            style="Snap.TButton",
            command=self._restore_snapshot,
        ).pack(side="left", padx=(0, 6), expand=True, fill="x")

        ttk.Button(
            btn2,
            text="🛡  Restore Point",
            style="Restore.TButton",
            command=lambda: self._dispatch("restore"),
        ).pack(side="left", expand=True, fill="x")

        # Progress + status
        status_frame = ttk.Frame(self._root)
        status_frame.pack(fill="x", padx=16, pady=(0, 8))

        self._progress = ttk.Progressbar(
            status_frame, orient="horizontal", mode="determinate"
        )
        self._progress.pack(fill="x", pady=(4, 2))

        self._status_label = ttk.Label(
            status_frame,
            text=f"Ready  •  Log: {SESSION.log_path}",
            style="Status.TLabel",
        )
        self._status_label.pack(fill="x")

    # ── Selection helpers ────────────────────────────────────────────────

    def _select_all(self) -> None:
        for row in self._tweak_rows:
            if not (self._corp_blocked and not row.td.corp_safe):
                row.var.set(True)

    def _deselect_all(self) -> None:
        for row in self._tweak_rows:
            row.var.set(False)

    def _filter_rows(self) -> None:
        """Show/hide rows based on search query."""
        query = self._search_var.get().strip().lower()
        visible_cats: set[str] = set()
        for row in self._tweak_rows:
            td = row.td
            match = not query or any(
                query in f.lower()
                for f in [td.id, td.label, td.category, td.description] + td.tags
            )
            if match:
                row.frame.pack(fill="x", padx=4, pady=2, ipady=3)
                visible_cats.add(td.category)
            else:
                row.frame.pack_forget()
        # Show/hide category labels
        for cat_name, cat_lbl in self._category_labels:
            if cat_name in visible_cats or not query:
                cat_lbl.pack(fill="x", pady=(10, 2), padx=4)
            else:
                cat_lbl.pack_forget()

    def _set_status(self, text: str, color: str = _FG_DIM) -> None:
        self._status_label.configure(text=text, foreground=color)

    def _selected_tweaks(self) -> List[TweakDef]:
        return [r.td for r in self._tweak_rows if r.var.get()]

    # ── Status refresh ───────────────────────────────────────────────────

    def _refresh_status_all(self) -> None:
        """Re-detect every tweak and update indicator dots + labels."""
        for row in self._tweak_rows:
            row.refresh_status()

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
        row._toggle_btn.configure(text="⏳…", state="disabled")

        def _worker() -> None:
            try:
                fn = td.apply_fn if action == "apply" else td.remove_fn
                fn()
                self._root.after(
                    0, self._set_status,
                    f"{td.label} — {'enabled' if action == 'apply' else 'disabled'} ✔",
                    _OK_GREEN,
                )
            except AdminRequirementError:
                self._root.after(
                    0, self._set_status,
                    f"{td.label}: admin required", _ERR_RED,
                )
            except Exception as exc:
                SESSION.log(f"[GUI] Toggle error {td.label}: {exc}")
                self._root.after(
                    0, self._set_status,
                    f"{td.label}: {exc}", _ERR_RED,
                )
            finally:
                self._root.after(0, row.refresh_status)
                self._root.after(0, lambda: row._toggle_btn.configure(state="normal"))

        threading.Thread(target=_worker, daemon=True).start()

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
        except Exception as exc:
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
                summary_parts: List[str] = []
                for action in set(results.values()):
                    count = sum(1 for v in results.values() if v == action)
                    summary_parts.append(f"{count} {action}")
                summary = "Restore: " + ", ".join(summary_parts)
                self._root.after(0, self._set_status, summary, _OK_GREEN)
                self._root.after(0, self._refresh_status_all)
            except Exception as exc:
                self._root.after(
                    0, lambda e=str(exc): messagebox.showerror("Restore Error", e)
                )

        threading.Thread(target=_worker, daemon=True).start()

    # ── Dispatch ─────────────────────────────────────────────────────────

    def _dispatch(self, mode: str) -> None:
        """Run tweaks in a background thread to keep the UI responsive."""

        # Corporate guard (unless forced)
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

        confirm_msg = (
            f"{'Apply' if mode == 'apply' else 'Remove'} "
            f"{len(selected)} selected tweak(s)?"
        )
        if not messagebox.askyesno("Confirm", confirm_msg):
            return

        threading.Thread(
            target=self._run_tweaks, args=(selected, mode), daemon=True
        ).start()

    def _run_tweaks(self, items: List[TweakDef], mode: str) -> None:
        total = len(items)
        errors: List[str] = []
        for i, td in enumerate(items, 1):
            pct = int(i / total * 100)
            self._root.after(0, self._progress.configure, {"value": pct})
            self._root.after(
                0,
                self._set_status,
                f"{'Applying' if mode == 'apply' else 'Removing'}: "
                f"{td.label}  ({i}/{total})",
                _ACCENT,
            )
            try:
                fn = td.apply_fn if mode == "apply" else td.remove_fn
                fn()
            except AdminRequirementError:
                errors.append(f"{td.label}: requires admin elevation")
            except Exception as exc:
                errors.append(f"{td.label}: {exc}")
                SESSION.log(f"[GUI] Error ({mode}) {td.label}: {exc}")

        ok_count = total - len(errors)
        summary = (
            f"{'Applied' if mode == 'apply' else 'Removed'} "
            f"{ok_count}/{total} tweaks"
        )
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
                lambda: messagebox.showwarning(
                    "Completed with Errors", f"{summary}\n\n{err_text}"
                ),
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
            self._root.after(
                0, self._set_status, "Restore point failed (admin)", _ERR_RED
            )
        except Exception as exc:
            err_msg = str(exc)
            self._root.after(0, lambda m=err_msg: messagebox.showerror("Error", m))
            self._root.after(
                0, self._set_status, f"Restore point error: {err_msg}", _ERR_RED
            )

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
