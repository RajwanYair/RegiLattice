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
import sys
import threading
import tkinter as tk
from pathlib import Path
from tkinter import filedialog, messagebox, ttk

from . import __version__
from . import gui_dialogs as dialogs
from . import gui_theme as theme
from .corpguard import CorporateNetworkError, assert_not_corporate, corp_guard_status, is_corporate_network, is_gpo_managed
from .gui_tooltip import build_tooltip_text, has_recommendation
from .gui_widgets import CategorySection, TweakRow
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary
from .tweaks import (
    TweakDef,
    TweakResult,
    all_tweaks,
    available_profiles,
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

# ── Theme aliases ────────────────────────────────────────────────────────────

_ACCENT = theme.ACCENT
_BG = theme.BG
_BG_SURFACE = theme.BG_SURFACE
_FG = theme.FG
_FG_DIM = theme.FG_DIM
_CARD_BG = theme.CARD_BG
_OK_GREEN = theme.OK_GREEN
_WARN_YELLOW = theme.WARN_YELLOW
_ERR_RED = theme.ERR_RED
_HEADER_BG = theme.HEADER_BG
_DIM_BG = theme.DIM_BG
_TEAL = theme.TEAL
_GPO_ORANGE = theme.GPO_ORANGE

_STATUS_APPLIED = theme.STATUS_APPLIED
_STATUS_NOT_APPLIED = theme.STATUS_NOT_APPLIED
_STATUS_UNKNOWN = theme.STATUS_UNKNOWN
_STATUS_CORP_BLOCKED = theme.STATUS_CORP_BLOCKED
_STATUS_DEFAULT = theme.STATUS_DEFAULT

_FONT = theme.FONT
_FONT_BOLD = theme.FONT_BOLD
_FONT_SM = theme.FONT_SM
_FONT_XS = theme.FONT_XS
_FONT_XS_BOLD = theme.FONT_XS_BOLD
_FONT_TITLE = theme.FONT_TITLE
_FONT_CAT = theme.FONT_CAT


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

        self._corp_blocked = False  # checked asynchronously after window shows
        self._tweak_rows: list[TweakRow] = []
        self._category_sections: list[CategorySection] = []
        self._cached_statuses: dict[str, TweakResult] = {}
        self._cached_rec_count: int | None = None  # computed once (static)
        self._cached_gpo_count: int | None = None  # computed once per session
        self._running = False  # True while a batch operation is active
        self._cancel = threading.Event()  # set to request cancellation
        self._loading = True  # True while progressive loading is in progress
        self._setup_styles()
        self._build_ui()
        self._bind_shortcuts()
        # Defer heavy work (corp check + row creation) so the window appears instantly
        self._root.after(50, self._deferred_init)

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
        total_tweaks = len(all_tweaks())
        total_cats = len(tweaks_by_category())

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

        # Corp banner placeholder — populated after async corp check
        self._corp_banner_frame = tk.Frame(self._root, bg=_BG)
        self._corp_banner_frame.pack(fill="x")

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

        # Theme selector dropdown
        theme_frame = ttk.Frame(toolbar)
        theme_frame.pack(side="right", padx=(8, 0))
        tk.Label(
            theme_frame,
            text="Theme:",
            fg=_FG_DIM,
            bg=_BG,
            font=_FONT_XS,
        ).pack(side="left", padx=(0, 4))
        self._theme_var = tk.StringVar(value=theme.current_theme())
        theme_names = theme.available_themes()
        ttk.OptionMenu(
            theme_frame,
            self._theme_var,
            self._theme_var.get(),
            *theme_names,
            command=lambda val: self._switch_theme(str(val)),
        ).pack(side="left")

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

        # Tweak rows are populated progressively in _deferred_init()

        # Action buttons (row 1: apply/remove)
        btn_frame = ttk.Frame(self._root)
        btn_frame.pack(fill="x", padx=16, pady=(0, 4))

        self._btn_apply = ttk.Button(
            btn_frame,
            text="▶  Apply Selected",
            style="Apply.TButton",
            command=lambda: self._dispatch("apply"),
        )
        self._btn_apply.pack(side="left", padx=(0, 6), expand=True, fill="x")

        self._btn_remove = ttk.Button(
            btn_frame,
            text="✖  Remove Selected",
            style="Remove.TButton",
            command=lambda: self._dispatch("remove"),
        )
        self._btn_remove.pack(side="left", padx=(0, 6), expand=True, fill="x")

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
        self._stat_blocked: tk.Label | None = None  # created after corp check if needed

        self._status_label = ttk.Label(
            status_frame,
            text="Loading tweaks\u2026",
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
        self._ctx_target: TweakRow | None = None
        # Context menu bindings are wired after rows are created in _deferred_init

    # ── Deferred heavy initialisation ────────────────────────────────────

    def _deferred_init(self) -> None:
        """Run after mainloop starts — corp check (background) + progressive row creation.

        Keeps the window visible and responsive while 1 200+ tweak rows load.
        The corporate environment check runs in a background thread so it never
        blocks the UI — tweak loading starts immediately.
        """
        # Start corp check in background — results applied via after() callback
        threading.Thread(target=self._corp_check_worker, daemon=True).start()

        self._set_status("Loading tweaks\u2026", _WARN_YELLOW)
        self._root.update_idletasks()

        grouped = tweaks_by_category()
        self._grouped_items = list(grouped.items())
        self._populate_batch(0)

    def _corp_check_worker(self) -> None:
        """Background thread: detect corporate environment and push result to UI."""
        is_corp = is_corporate_network()
        corp_info = corp_guard_status() if is_corp else None
        self._root.after(0, self._apply_corp_result, is_corp, corp_info)

    def _apply_corp_result(self, is_corp: bool, corp_info: str | None) -> None:
        """Apply corporate detection result on the main thread."""
        self._corp_blocked = is_corp
        if is_corp:
            info_text = corp_info or "corporate environment detected"
            self._corp_banner_frame.configure(bg=_ERR_RED)
            tk.Label(
                self._corp_banner_frame,
                text=f"  \U0001f6d1  Corporate network: {info_text} \u2014 non-corp-safe tweaks blocked",
                bg=_ERR_RED,
                fg="#1E1E2E",
                font=_FONT_BOLD,
                anchor="w",
                padx=12,
                pady=6,
            ).pack(fill="x")
            self._stat_blocked = tk.Label(self._stats_frame, text="\u25cf 0 Blocked", fg=_ERR_RED, bg=_BG, font=_FONT_XS)
            self._stat_blocked.pack(side="left", padx=(0, 12))
            # Disable non-corp-safe rows that were already created
            for row in self._tweak_rows:
                if not row.td.corp_safe:
                    row.mark_corp_blocked()

    _BATCH_SIZE = 4  # categories per batch — keeps UI fluid

    def _populate_batch(self, idx: int) -> None:
        """Create tweak rows for a batch of categories, then schedule the next batch."""
        end = min(idx + self._BATCH_SIZE, len(self._grouped_items))
        for cat_name, cat_tweaks in self._grouped_items[idx:end]:
            cat_rows: list[TweakRow] = []
            for td in cat_tweaks:
                row = TweakRow(
                    self._inner,
                    td,
                    corp_blocked=self._corp_blocked,
                    on_toggle=self._toggle_single,
                    defer_widgets=True,
                )
                self._tweak_rows.append(row)
                cat_rows.append(row)
            section = CategorySection(self._inner, cat_name, cat_rows)
            section.set_on_batch(self._batch_category)
            self._category_sections.append(section)

        pct = min(100, int(end / len(self._grouped_items) * 100))
        self._progress.configure(value=pct)
        self._set_status(f"Loading tweaks\u2026 {pct}%", _WARN_YELLOW)

        if end < len(self._grouped_items):
            self._root.after(1, self._populate_batch, end)
        else:
            self._finish_loading()

    def _finish_loading(self) -> None:
        """Wire up bindings and kick off status detection after all rows are created."""
        self._loading = False
        # Wire checkbox → selection counter
        for row in self._tweak_rows:
            row.var.trace_add("write", lambda *_: self._update_selection_count())
        # Right-click context menu
        for row in self._tweak_rows:
            row.frame.bind("<Button-3>", lambda e, r=row: self._show_context_menu(e, r))  # type: ignore[misc]
        self._progress.configure(value=0)
        total_tweaks = len(self._tweak_rows)
        total_cats = len(self._category_sections)
        self._set_status(
            f"Ready  \u2022  {total_tweaks} tweaks in {total_cats} categories  \u2022  Log: {SESSION.log_path}",
        )
        # Clean up temporary data
        del self._grouped_items
        # Kick off status detection
        self._initial_refresh()

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
        if self._loading:
            return
        query = self._search_var.get().strip()
        status_filter = self._status_filter_var.get()
        scope_filter = self._scope_filter_var.get()
        _filter_status = {"Applied": TweakResult.APPLIED, "Default": TweakResult.NOT_APPLIED, "Unknown": TweakResult.UNKNOWN}
        _filter_scope = {"User Only": "user", "Machine Only": "machine", "Both": "both"}

        # Use search_tweaks() for indexed text matching (faster on large tweak sets)
        matching_ids: set[str] | None = None
        if query:
            matching_ids = {td.id for td in search_tweaks(query)}

        for section in self._category_sections:
            visible = False
            target_status = _filter_status.get(status_filter)
            target_scope = _filter_scope.get(scope_filter, "")
            for row in section.rows:
                td = row.td
                text_match = matching_ids is None or td.id in matching_ids
                status_match = status_filter == "All" or self._cached_statuses.get(td.id, TweakResult.UNKNOWN) == target_status
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

    def _show_context_menu(self, event: tk.Event[tk.Misc], row: TweakRow) -> None:
        """Display a context menu for a tweak row."""
        self._ctx_target = row
        self._ctx_menu.delete(0, "end")
        td = row.td
        st = tweak_status(td)

        if st == TweakResult.APPLIED:
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
        dialogs.import_json_selection(
            self._root,
            self._tweak_rows,
            self._deselect_all,
            self._update_selection_count,
            self._set_status,
        )

    # ── Scoop Tools Manager ────────────────────────────────────────────

    def _open_scoop_manager(self) -> None:
        """Open a Scoop Tools manager dialog showing installed packages with install/remove."""
        dialogs.open_scoop_manager(self._root, self._refresh_status_all)

    # ── About dialog ─────────────────────────────────────────────────────

    def _show_about(self) -> None:
        """Show an About dialog with system and project info."""
        dialogs.show_about(self._corp_blocked)

    # ── Category batch actions ───────────────────────────────────────────

    def _batch_category(self, section: CategorySection, mode: str) -> None:
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

    def _initial_refresh(self) -> None:
        """Kick off status detection in a background thread so the window shows immediately."""
        self._set_status("Detecting tweak states\u2026", _WARN_YELLOW)

        def _worker() -> None:
            statuses = status_map(parallel=True, max_workers=8)
            self._root.after(0, lambda: self._apply_statuses(statuses))

        threading.Thread(target=_worker, daemon=True).start()

    def _refresh_status_all(self) -> None:
        """Re-detect every tweak and update dots, labels, counts, and stats.

        Uses parallel detection via thread-pool for faster refresh.
        """
        statuses = status_map(parallel=True, max_workers=8)
        self._apply_statuses(statuses)

    def _apply_statuses(self, statuses: dict[str, TweakResult]) -> None:
        """Push a pre-computed status dict into every UI row and update counters."""
        self._cached_statuses = statuses
        applied = 0
        default = 0
        unknown = 0
        blocked = 0
        for row in self._tweak_rows:
            if row.disabled_by_corp:
                blocked += 1
                continue
            st = statuses.get(row.td.id, TweakResult.UNKNOWN)
            # Update row directly from the cached status
            if st == TweakResult.APPLIED:
                colour = _STATUS_APPLIED
                text = "APPLIED"
                btn_text = "Disable \u2715"
                btn_bg = "#40543F"
                btn_fg = _OK_GREEN
                applied += 1
            elif st == TweakResult.NOT_APPLIED:
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
            row.tooltip.update_text(build_tooltip_text(row.td, st))
        for section in self._category_sections:
            section.update_count(statuses)
        # Update summary stats (rec/gpo counts are static — compute once)
        if self._cached_rec_count is None:
            self._cached_rec_count = sum(1 for r in self._tweak_rows if has_recommendation(r.td))
        if self._cached_gpo_count is None:
            self._cached_gpo_count = sum(1 for r in self._tweak_rows if r.td.registry_keys and is_gpo_managed(r.td.registry_keys))
        self._stat_applied.configure(text=f"\u25cf {applied} Applied")
        self._stat_default.configure(text=f"\u25cf {default} Default")
        self._stat_unknown.configure(text=f"\u25cf {unknown} Unknown")
        self._stat_rec.configure(text=f"\u2605 {self._cached_rec_count} Recommended")
        self._stat_gpo.configure(text=f"\u25cf {self._cached_gpo_count} GPO")
        if self._stat_blocked is not None:
            self._stat_blocked.configure(text=f"\u25cf {blocked} Blocked")

    # ── Individual toggle ────────────────────────────────────────────────

    def _toggle_single(self, row: TweakRow) -> None:
        """Toggle a single tweak: apply if not applied, remove if applied."""
        if not self._force_var.get():
            try:
                assert_not_corporate()
            except CorporateNetworkError as exc:
                messagebox.showwarning("Corporate Network", str(exc))
                return

        td = row.td
        st = tweak_status(td)
        action = "remove" if st == TweakResult.APPLIED else "apply"
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
        apply_cats = {c.lower() for c in info.apply_categories}
        for row in self._tweak_rows:
            if row.td.category.lower() in apply_cats and not row.disabled_by_corp:
                row.var.set(True)
        self._update_selection_count()
        count = sum(1 for r in self._tweak_rows if r.var.get())
        self._set_status(f"Profile '{profile_name}' selected ({count} tweaks)", _ACCENT)

    # ── Theme switching ──────────────────────────────────────────────────

    def _switch_theme(self, name: str) -> None:
        """Apply a new colour theme and reconfigure core UI elements."""
        # Update module-level theme
        theme.set_theme(name)
        # Re-read into local aliases used by _setup_styles and widget creation
        self._reload_theme_aliases()
        self._setup_styles()
        self._root.configure(bg=theme.BG)
        self._set_status(f"Theme \u2192 {name}", theme.ACCENT)

    @staticmethod
    def _reload_theme_aliases() -> None:
        """Refresh the module-level ``_*`` aliases after ``set_theme``."""
        global _ACCENT, _BG, _BG_SURFACE, _FG, _FG_DIM, _CARD_BG
        global _OK_GREEN, _WARN_YELLOW, _ERR_RED, _HEADER_BG, _DIM_BG, _TEAL, _GPO_ORANGE
        global _STATUS_APPLIED, _STATUS_NOT_APPLIED, _STATUS_UNKNOWN, _STATUS_CORP_BLOCKED, _STATUS_DEFAULT
        _ACCENT = theme.ACCENT
        _BG = theme.BG
        _BG_SURFACE = theme.BG_SURFACE
        _FG = theme.FG
        _FG_DIM = theme.FG_DIM
        _CARD_BG = theme.CARD_BG
        _OK_GREEN = theme.OK_GREEN
        _WARN_YELLOW = theme.WARN_YELLOW
        _ERR_RED = theme.ERR_RED
        _HEADER_BG = theme.HEADER_BG
        _DIM_BG = theme.DIM_BG
        _TEAL = theme.TEAL
        _GPO_ORANGE = theme.GPO_ORANGE
        _STATUS_APPLIED = theme.STATUS_APPLIED
        _STATUS_NOT_APPLIED = theme.STATUS_NOT_APPLIED
        _STATUS_UNKNOWN = theme.STATUS_UNKNOWN
        _STATUS_CORP_BLOCKED = theme.STATUS_CORP_BLOCKED
        _STATUS_DEFAULT = theme.STATUS_DEFAULT

    # ── Export as PowerShell ─────────────────────────────────────────────

    def _export_powershell(self) -> None:
        """Export selected tweaks as a .ps1 script showing the registry changes."""
        dialogs.export_powershell(self._selected_tweaks(), self._set_status)

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
        if self._running:
            self._cancel.set()
            self._set_status("Cancelling…", _WARN_YELLOW)
            return

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

        self._set_running(True)
        threading.Thread(target=self._run_tweaks, args=(selected, mode), daemon=True).start()

    def _set_running(self, running: bool) -> None:
        """Toggle UI lock during batch operations."""
        self._running = running
        state = "disabled" if running else "!disabled"
        self._btn_apply.state([state])  # type: ignore[no-untyped-call]
        self._btn_remove.state([state])  # type: ignore[no-untyped-call]
        if running:
            self._cancel.clear()

    def _run_tweaks(self, items: list[TweakDef], mode: str) -> None:
        total = len(items)
        errors: list[str] = []
        for i, td in enumerate(items, 1):
            if self._cancel.is_set():
                self._root.after(0, self._set_status, f"Cancelled after {i - 1}/{total}", _WARN_YELLOW)
                break
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
        else:
            # Only show full summary if not cancelled
            ok_count = total - len(errors)
            summary = f"{'Applied' if mode == 'apply' else 'Removed'} {ok_count}/{total} tweaks"
            if errors:
                summary += f"  •  {len(errors)} error(s)"
            colour = _OK_GREEN if not errors else _WARN_YELLOW
            self._root.after(0, self._set_status, summary, colour)

        self._root.after(0, self._progress.configure, {"value": 100})
        self._root.after(0, self._refresh_status_all)
        self._root.after(0, self._set_running, False)

        if errors:
            err_text = "\n".join(errors)
            ok_count = total - len(errors)
            summary = f"{'Applied' if mode == 'apply' else 'Removed'} {ok_count}/{total} tweaks  •  {len(errors)} error(s)"
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
