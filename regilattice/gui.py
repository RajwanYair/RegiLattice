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
import json
import sys
import threading
import tkinter as tk
from pathlib import Path
from tkinter import filedialog, messagebox, ttk
from types import ModuleType
from typing import Any

from . import __version__
from . import gui_dialogs as dialogs
from . import gui_theme as theme
from .corpguard import CorporateNetworkError, assert_not_corporate, corp_guard_status, is_corporate_network, is_gpo_managed
from .gui_tooltip import build_tooltip_text, has_recommendation
from .gui_widgets import CategorySection, TweakRow
from .hwinfo import detect_hardware
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

# ── Config persistence ───────────────────────────────────────────────────────
_CONFIG_DIR = Path.home() / ".regilattice"
_GEOMETRY_FILE = _CONFIG_DIR / "window.json"
_COLLAPSE_FILE = _CONFIG_DIR / "collapsed.json"
_PREFS_FILE = _CONFIG_DIR / "preferences.json"
_SEARCH_HISTORY_FILE = _CONFIG_DIR / "search_history.json"
_CATEGORY_ORDER_FILE = _CONFIG_DIR / "category_order.json"
_MAX_SEARCH_HISTORY = 20


# ── Optional system-tray deps ───────────────────────────────────────────────


def _try_import(module: str) -> ModuleType | None:
    """Import *module* returning ``None`` on failure (no auto-install)."""
    try:
        import importlib

        return importlib.import_module(module)
    except ImportError:
        return None


# ── Main GUI ─────────────────────────────────────────────────────────────────


class RegiLatticeGUI:
    """Plugin-driven main application window with collapsible category sections."""

    def __init__(self) -> None:
        self._enable_dpi_awareness()
        self._root = tk.Tk()
        self._root.title(f"RegiLattice  v{__version__}")
        self._root.geometry(self._load_geometry() or "900x940")
        self._root.minsize(700, 600)
        self._root.configure(bg=_BG)
        self._root.resizable(True, True)

        with contextlib.suppress(tk.TclError, OSError):
            self._root.iconbitmap(default="")

        # Windows 11: attempt DWM dark title bar
        self._apply_win11_dark_titlebar()

        self._corp_blocked = False  # checked asynchronously after window shows
        self._tweak_rows: list[TweakRow] = []
        self._row_by_id: dict[str, TweakRow] = {}
        self._category_sections: list[CategorySection] = []
        self._cached_statuses: dict[str, TweakResult] = {}
        self._cached_rec_count: int | None = None  # computed once (static)
        self._cached_gpo_count: int | None = None  # computed once per session
        self._running = False  # True while a batch operation is active
        self._cancel = threading.Event()  # set to request cancellation
        self._loading = True  # True while progressive loading is in progress
        self._undo_stack: list[tuple[str, list[TweakDef]]] = []  # (mode, tweaks)
        self._session_changed: set[str] = set()  # tweak IDs modified this session
        self._last_clicked_row_idx: int | None = None  # for Shift+Click range select
        self._search_debounce_id: str | None = None  # after() id for search debounce
        self._tray_icon: Any = None  # pystray.Icon if available
        self._tray_available = False
        self._setup_styles()
        self._build_ui()
        self._bind_shortcuts()
        self._setup_tray()
        self._root.protocol("WM_DELETE_WINDOW", self._on_close)
        # Defer heavy work (corp check + row creation) so the window appears instantly
        self._root.after(50, self._deferred_init)

    # ── DPI awareness ──────────────────────────────────────────────────

    @staticmethod
    def _enable_dpi_awareness() -> None:
        """Tell Windows this process is DPI-aware (per-monitor v2 → system → fallback)."""
        try:
            import ctypes

            # Prefer Per-Monitor v2 (Windows 10 1703+)
            awareness = ctypes.c_int(2)  # PROCESS_PER_MONITOR_DPI_AWARE_V2
            result = ctypes.windll.shcore.SetProcessDpiAwarenessContext(ctypes.byref(awareness))
            if result == 0:
                # Fall back to system-level DPI awareness
                ctypes.windll.shcore.SetProcessDpiAwareness(1)  # PROCESS_SYSTEM_DPI_AWARE
        except (ImportError, OSError, AttributeError):
            pass  # non-Windows or unsupported

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
        style.configure("CardAlt.TFrame", background=theme.CARD_BG_ALT)
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
        style.configure(
            "Alt.TCheckbutton",
            background=theme.CARD_BG_ALT,
            foreground=_FG,
            font=_FONT,
            indicatorsize=16,
        )
        style.map(
            "Alt.TCheckbutton",
            background=[("active", theme.CARD_BG_ALT), ("disabled", theme.CARD_BG_ALT)],
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
        self._root.bind("<Control-z>", lambda _: self._undo_last())
        self._root.bind("<Escape>", lambda _: self._clear_search())
        self._root.bind("<Down>", lambda _: self._navigate_rows(1))
        self._root.bind("<Up>", lambda _: self._navigate_rows(-1))
        self._root.bind("<space>", self._toggle_focused_row)

    def _navigate_rows(self, direction: int) -> None:
        """Move keyboard focus between visible tweak rows."""
        visible = [r for r in self._tweak_rows if r.frame is not None and r.frame.winfo_ismapped()]
        if not visible:
            return
        current = getattr(self, "_focused_row_idx", -1)
        new_idx = max(0, min(len(visible) - 1, current + direction))
        self._focused_row_idx = new_idx
        row = visible[new_idx]
        row.frame.focus_set()
        row.frame.event_generate("<Enter>")
        # Scroll into view
        row.frame.update_idletasks()
        row.frame.winfo_toplevel()

    def _toggle_focused_row(self, event: tk.Event[tk.Misc]) -> None:
        """Toggle selection of the currently focused row via spacebar."""
        # Don't intercept space when typing in the search bar
        if event.widget == self._search_entry:
            return
        visible = [r for r in self._tweak_rows if r.frame is not None and r.frame.winfo_ismapped()]
        idx = getattr(self, "_focused_row_idx", -1)
        if 0 <= idx < len(visible):
            row = visible[idx]
            if not row.disabled_by_corp:
                row.var.set(not row.var.get())

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

        # ── Menu bar ────────────────────────────────────────────────────
        menubar = tk.Menu(self._root, tearoff=False)

        file_menu = tk.Menu(menubar, tearoff=False)
        file_menu.add_command(label="Import JSON\u2026", command=self._import_json_selection, accelerator="")
        file_menu.add_command(label="Export JSON\u2026", command=self._export_json_selection)
        file_menu.add_command(label="Export PowerShell\u2026", command=self._export_powershell)
        file_menu.add_command(label="Export Log\u2026", command=self._export_log)
        file_menu.add_separator()
        file_menu.add_command(label="Minimize to Tray", command=self._minimize_to_tray)
        file_menu.add_command(label="Exit", command=self._quit, accelerator="Alt+F4")
        menubar.add_cascade(label="File", menu=file_menu)

        edit_menu = tk.Menu(menubar, tearoff=False)
        edit_menu.add_command(label="Select All", command=self._select_all, accelerator="Ctrl+A")
        edit_menu.add_command(label="Deselect All", command=self._deselect_all, accelerator="Ctrl+D")
        edit_menu.add_command(label="Invert Selection", command=self._invert_selection, accelerator="Ctrl+I")
        edit_menu.add_separator()
        edit_menu.add_command(label="Undo Last", command=self._undo_last, accelerator="Ctrl+Z")
        menubar.add_cascade(label="Edit", menu=edit_menu)

        view_menu = tk.Menu(menubar, tearoff=False)
        view_menu.add_command(label="Expand All", command=self._expand_all, accelerator="Ctrl+E")
        view_menu.add_command(label="Collapse All", command=self._collapse_all, accelerator="Ctrl+Shift+E")
        view_menu.add_command(label="Toggle Log Panel", command=self._toggle_log_panel, accelerator="Ctrl+L")
        view_menu.add_command(label="Refresh Status", command=self._refresh_status_all, accelerator="Ctrl+R")
        menubar.add_cascade(label="View", menu=view_menu)

        help_menu = tk.Menu(menubar, tearoff=False)
        help_menu.add_command(label="About", command=self._show_about)
        menubar.add_cascade(label="Help", menu=help_menu)

        self._root.config(menu=menubar)

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
        # Hardware info — populated async by _hw_detect_worker
        self._hw_label = ttk.Label(header, text="", style="Subtitle.TLabel")
        self._hw_label.pack(side="right", padx=16, pady=(14, 2))

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
        theme_names = ["Auto", *theme.available_themes()]
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
            "Changed",
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

        # Search bar with history dropdown
        search_frame = ttk.Frame(self._root)
        search_frame.pack(fill="x", padx=16, pady=(6, 0))
        ttk.Label(search_frame, text="🔍", style="TLabel").pack(side="left", padx=(0, 4))
        self._search_var = tk.StringVar()
        self._search_var.trace_add("write", lambda *_: self._debounce_filter())
        self._search_history: list[str] = self._load_search_history()
        self._search_entry = ttk.Combobox(search_frame, textvariable=self._search_var, font=_FONT, values=self._search_history)
        self._search_entry.pack(side="left", fill="x", expand=True)
        self._search_entry.bind("<Return>", lambda _: self._commit_search())
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

        self._btn_undo = ttk.Button(
            btn_frame,
            text="\u21b6  Undo Last",
            style="Snap.TButton",
            command=self._undo_last,
        )
        self._btn_undo.pack(side="left", padx=(0, 6), expand=True, fill="x")
        self._btn_undo.state(["disabled"])

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
        ttk.Button(btn3, text="\U0001f4e4  Export JSON", command=self._export_json_selection).pack(
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
        ttk.Button(btn3, text="\U0001f4be  Export Log", command=self._export_log).pack(
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
        # Detect hardware in background so batch size is set before row loading
        threading.Thread(target=self._hw_detect_worker, daemon=True).start()

        # Start corp check in background — results applied via after() callback
        threading.Thread(target=self._corp_check_worker, daemon=True).start()

        self._set_status("Loading tweaks\u2026", _WARN_YELLOW)
        self._root.update_idletasks()

        grouped = tweaks_by_category()
        self._grouped_items = list(grouped.items())
        # Apply user-saved category order if present
        saved_order = self._load_category_order()
        if saved_order:
            order_map = {name: idx for idx, name in enumerate(saved_order)}
            self._grouped_items.sort(key=lambda item: order_map.get(item[0], len(saved_order)))
        self._populate_batch(0)

    def _hw_detect_worker(self) -> None:
        """Background thread: detect hardware and push adaptive tuning to UI."""
        try:
            hw = detect_hardware()
            self._root.after(0, self._apply_hw_result, hw)
        except Exception:
            pass  # keep default _BATCH_SIZE on failure

    def _apply_hw_result(self, hw: object) -> None:
        """Apply hardware detection on the main thread (update subtitle)."""
        from .hwinfo import HWProfile

        if not isinstance(hw, HWProfile):
            return
        # Store for About dialog
        self._hw_profile = hw
        # Update subtitle with hardware summary
        cpu_short = hw.cpu.name.split("@")[0].strip() if "@" in hw.cpu.name else hw.cpu.name
        if len(cpu_short) > 40:
            cpu_short = cpu_short[:37] + "\u2026"
        gpu_name = hw.gpus[0].name if hw.gpus else "No GPU"
        if len(gpu_name) > 30:
            gpu_name = gpu_name[:27] + "\u2026"
        ram_gb = hw.memory.total_mb // 1024 if hw.memory.total_mb else "?"
        hw_text = f"{cpu_short}  |  {gpu_name}  |  {ram_gb} GB RAM"
        if hasattr(self, "_hw_label"):
            self._hw_label.configure(text=hw_text)

    def _corp_check_worker(self) -> None:
        """Background thread: detect corporate environment and push result to UI."""
        is_corp = is_corporate_network()
        corp_info = corp_guard_status() if is_corp else None  # uses same cached results — no re-run
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

    _BATCH_SIZE = 4  # default — overridden by hwinfo in _deferred_init

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
                self._row_by_id[td.id] = row
                cat_rows.append(row)
            section = CategorySection(self._inner, cat_name, cat_rows)
            section.set_on_batch(self._batch_category)
            section.set_on_reorder(self._reorder_category)
            section.set_on_collapse_change(self._on_category_collapse_change)
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
        # Wire checkbox → selection counter + Shift+Click range select
        for i, row in enumerate(self._tweak_rows):
            row.var.trace_add("write", lambda *_args: self._update_selection_count())
            row.cb.bind("<Shift-Button-1>", lambda e, idx=i: self._on_shift_click(idx))  # type: ignore[misc]
            row.cb.bind("<Button-1>", lambda e, idx=i: self._on_row_click(idx), add=True)  # type: ignore[misc]
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
        # Restore collapsed categories from previous session
        self._restore_collapse_state()
        # Restore saved preferences (theme, profile, filters)
        self._restore_preferences()
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

    def _debounce_filter(self, delay_ms: int = 200) -> None:
        """Schedule _filter_rows after *delay_ms*, cancelling any pending call."""
        if self._search_debounce_id is not None:
            self._root.after_cancel(self._search_debounce_id)
        self._search_debounce_id = self._root.after(delay_ms, self._filter_rows)

    def _filter_rows(self) -> None:
        """Show/hide rows based on search query, status filter, AND scope filter.

        Supports prefix operators in the search bar:
          tag:<keyword>   — match tweaks whose tags contain keyword
          cat:<name>      — match tweaks whose category contains name
          scope:<u|m|b>   — match user / machine / both
          admin:yes|no    — match tweaks requiring (or not requiring) admin
        Multiple prefixes can be combined with plain text, e.g. "tag:privacy disable telemetry".
        """
        if self._loading:
            return
        raw_query = self._search_var.get().strip()
        status_filter = self._status_filter_var.get()
        scope_filter = self._scope_filter_var.get()
        _filter_status = {"Applied": TweakResult.APPLIED, "Default": TweakResult.NOT_APPLIED, "Unknown": TweakResult.UNKNOWN}
        _filter_scope = {"User Only": "user", "Machine Only": "machine", "Both": "both"}

        # Parse prefix operators out of the query
        text_parts: list[str] = []
        tag_filters: list[str] = []
        cat_filters: list[str] = []
        scope_op: str = ""
        admin_op: bool | None = None
        for token in raw_query.split():
            low = token.lower()
            if low.startswith("tag:") and len(low) > 4:
                tag_filters.append(low[4:])
            elif low.startswith("cat:") and len(low) > 4:
                cat_filters.append(low[4:])
            elif low.startswith("scope:") and len(low) > 6:
                scope_op = {"u": "user", "m": "machine", "b": "both", "user": "user", "machine": "machine", "both": "both"}.get(low[6:], "")
            elif low.startswith("admin:"):
                admin_op = low[6:] in ("yes", "true", "1")
            else:
                text_parts.append(token)

        text_query = " ".join(text_parts)

        # Use search_tweaks() for indexed text matching (faster on large tweak sets)
        matching_ids: set[str] | None = None
        if text_query:
            matching_ids = {td.id for td in search_tweaks(text_query)}

        target_status = _filter_status.get(status_filter)
        target_scope = _filter_scope.get(scope_filter, "")
        need_scope = scope_filter != "All"
        need_scope_op = bool(scope_op)

        for section in self._category_sections:
            visible = False
            for row in section.rows:
                td = row.td
                text_match = matching_ids is None or td.id in matching_ids
                if status_filter == "Changed":
                    status_match = td.id in self._session_changed
                else:
                    status_match = status_filter == "All" or self._cached_statuses.get(td.id, TweakResult.UNKNOWN) == target_status
                scope_match = not need_scope or tweak_scope(td) == target_scope
                # Prefix operator checks
                tag_match = all(any(tf in t.lower() for t in td.tags) for tf in tag_filters)
                cat_match = all(cf in td.category.lower() for cf in cat_filters)
                scope_op_match = not need_scope_op or tweak_scope(td) == scope_op
                admin_match = admin_op is None or td.needs_admin == admin_op
                if text_match and status_match and scope_match and tag_match and cat_match and scope_op_match and admin_match:
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

    # ── Shift+Click range select ─────────────────────────────────────────

    def _on_row_click(self, idx: int) -> None:
        """Track last-clicked row index for Shift+Click range selection."""
        self._last_clicked_row_idx = idx

    def _on_shift_click(self, idx: int) -> str:
        """Select range of rows between last click and current Shift+Click."""
        anchor = self._last_clicked_row_idx
        if anchor is None:
            self._last_clicked_row_idx = idx
            return ""
        lo, hi = sorted((anchor, idx))
        for i in range(lo, hi + 1):
            row = self._tweak_rows[i]
            if not row.disabled_by_corp:
                row.var.set(True)
        self._last_clicked_row_idx = idx
        self._update_selection_count()
        return "break"  # prevent default checkbox toggle

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
            self._auto_refresh_log()

    def _refresh_log(self) -> None:
        """Load the session log file into the log text widget and scroll to end."""
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

    def _auto_refresh_log(self) -> None:
        """Periodically refresh the log panel while it is visible."""
        if self._log_visible:
            self._refresh_log()
            self._root.after(2000, self._auto_refresh_log)

    def _export_log(self) -> None:
        """Save the session log to a user-chosen location."""
        path = filedialog.asksaveasfilename(
            title="Export Session Log",
            defaultextension=".log",
            filetypes=[("Log files", "*.log"), ("Text files", "*.txt"), ("All files", "*.*")],
            initialfile="RegiLattice_session.log",
        )
        if not path:
            return
        try:
            import shutil

            shutil.copy2(SESSION.log_path, path)
            self._set_status(f"Log exported to {Path(path).name}", _OK_GREEN)
        except OSError as exc:
            messagebox.showerror("Export Error", str(exc))

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

        if td.depends_on:
            deps = ", ".join(td.depends_on)
            self._ctx_menu.add_separator()
            self._ctx_menu.add_command(label=f"Depends on: {deps}", state="disabled")

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
        from .hwinfo import hardware_summary

        hw_text = ""
        if hasattr(self, "_hw_profile") and self._hw_profile is not None:
            hw_text = hardware_summary(self._hw_profile)
        dialogs.show_about(self._corp_blocked, hw_summary=hw_text)

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
        self._set_status("Detecting tweak states\u2026 0 %", _WARN_YELLOW)

        def _on_progress(done: int, total: int) -> None:
            pct = done * 100 // total if total else 100
            self._root.after(0, lambda p=pct: self._set_status(f"Detecting tweak states\u2026 {p} %", _WARN_YELLOW))  # type: ignore[misc]

        def _worker() -> None:
            statuses = status_map(parallel=True, progress_fn=_on_progress)
            self._root.after(0, lambda: self._apply_statuses(statuses))

        threading.Thread(target=_worker, daemon=True).start()

    def _refresh_status_all(self) -> None:
        """Re-detect every tweak in a background thread and update the UI."""
        self._set_status("Refreshing tweak states\u2026 0 %", _WARN_YELLOW)

        def _on_progress(done: int, total: int) -> None:
            pct = done * 100 // total if total else 100
            self._root.after(0, lambda p=pct: self._set_status(f"Refreshing tweak states\u2026 {p} %", _WARN_YELLOW))  # type: ignore[misc]

        def _worker() -> None:
            statuses = status_map(parallel=True, progress_fn=_on_progress)
            self._root.after(0, lambda: self._apply_statuses(statuses))

        threading.Thread(target=_worker, daemon=True).start()

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
        """Apply a new colour theme and reconfigure all UI elements."""
        resolved = theme.detect_system_theme() if name == "Auto" else name
        theme.set_theme(resolved)
        self._reload_theme_aliases()
        self._setup_styles()
        self._root.configure(bg=_BG)
        # Re-render all tweak rows and category sections with new colours
        for row in self._tweak_rows:
            row.apply_theme()
        for section in self._category_sections:
            section.apply_theme()
        # Re-apply cached statuses so row colours match the new theme
        if self._cached_statuses:
            self._apply_statuses(self._cached_statuses)
        self._set_status(f"Theme \u2192 {name}", _ACCENT)

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

    # ── Export as JSON ───────────────────────────────────────────────────

    def _export_json_selection(self) -> None:
        """Export selected tweak IDs as a JSON file for sharing/reimporting."""
        dialogs.export_json_selection(self._selected_tweaks(), self._set_status)

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

        action = "Apply" if mode == "apply" else "Remove"
        admin_count = sum(1 for td in selected if td.needs_admin)
        user_count = sum(1 for td in selected if not td.needs_admin)
        reg_keys = sum(len(td.registry_keys) for td in selected)
        cats = sorted({td.category for td in selected})

        summary_lines = [f"{action} {len(selected)} selected tweak(s)?"]
        summary_lines.append(f"\n  \u2022 {admin_count} admin  /  {user_count} user-level")
        summary_lines.append(f"  \u2022 {reg_keys} registry key(s) affected")
        summary_lines.append(f"  \u2022 {len(cats)} categor{'y' if len(cats) == 1 else 'ies'}: {', '.join(cats[:5])}")
        if len(cats) > 5:
            summary_lines.append(f"    \u2026 and {len(cats) - 5} more")
        if admin_count:
            summary_lines.append(f"\n\u26a0 {admin_count} tweak(s) require admin privileges.")
        confirm_msg = "\n".join(summary_lines)
        if not messagebox.askyesno("Confirm", confirm_msg):
            return

        self._set_running(True)
        threading.Thread(target=self._run_tweaks, args=(selected, mode), daemon=True).start()

    def _set_running(self, running: bool) -> None:
        """Toggle UI lock during batch operations."""
        self._running = running
        state = "disabled" if running else "!disabled"
        self._btn_apply.state([state])
        self._btn_remove.state([state])
        if running:
            self._cancel.clear()

    def _run_tweaks(self, items: list[TweakDef], mode: str) -> None:
        total = len(items)
        errors: list[str] = []
        succeeded: list[str] = []
        succeeded_defs: list[TweakDef] = []
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
                succeeded.append(td.label)
                succeeded_defs.append(td)
                self._session_changed.add(td.id)
            except AdminRequirementError:
                errors.append(f"{td.label}: requires admin elevation")
            except (OSError, RuntimeError, ValueError) as exc:
                errors.append(f"{td.label}: {exc}")
                SESSION.log(f"[GUI] Error ({mode}) {td.label}: {exc}")
            # Immediately update the individual row status on the UI thread
            row = self._row_by_id.get(td.id)
            if row is not None:
                self._root.after(0, row.refresh_status)
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

        # Push to undo stack so the user can reverse this batch
        if items:
            self._undo_stack.append((mode, list(items)))
            self._root.after(0, lambda: self._btn_undo.state(["!disabled"]))

        if errors and succeeded_defs:
            # Offer rollback of succeeded tweaks when batch had partial failures
            err_text = "\n".join(errors)
            ok_count = len(succeeded)
            summary = f"{'Applied' if mode == 'apply' else 'Removed'} {ok_count}/{total} tweaks  •  {len(errors)} error(s)"
            msg = f"{summary}\n\n{err_text}\n\nRollback the {ok_count} successful tweak(s)?"
            self._root.after(0, lambda: self._offer_rollback(msg, succeeded_defs, mode))
        elif errors:
            err_text = "\n".join(errors)
            summary = f"{'Applied' if mode == 'apply' else 'Removed'} 0/{total} tweaks  •  {len(errors)} error(s)"
            self._root.after(
                0,
                lambda: messagebox.showwarning("Completed with Errors", f"{summary}\n\n{err_text}"),
            )
        elif succeeded:
            action = "Applied" if mode == "apply" else "Removed"
            detail = "\n".join(f"  \u2714 {name}" for name in succeeded[:20])
            if len(succeeded) > 20:
                detail += f"\n  \u2026 and {len(succeeded) - 20} more"
            self._root.after(
                0,
                lambda: messagebox.showinfo("What Changed", f"{action} {len(succeeded)} tweak(s):\n\n{detail}"),
            )

    def _offer_rollback(self, msg: str, succeeded_defs: list[TweakDef], mode: str) -> None:
        """Show a dialog offering to rollback succeeded tweaks after partial failure."""
        if messagebox.askyesno("Partial Failure — Rollback?", msg):
            reverse = "remove" if mode == "apply" else "apply"
            self._set_status(f"Rolling back {len(succeeded_defs)} tweaks...", _WARN_YELLOW)
            self._dispatch_raw(succeeded_defs, reverse)

    def _undo_last(self) -> None:
        """Reverse the last batch operation by running the opposite mode."""
        if self._running or not self._undo_stack:
            return
        mode, items = self._undo_stack.pop()
        reverse = "remove" if mode == "apply" else "apply"
        if not self._undo_stack:
            self._btn_undo.state(["disabled"])
        self._set_status(f"Undoing: {mode} \u2192 {reverse} ({len(items)} tweaks)...", _ACCENT)
        self._dispatch_raw(items, reverse)

    def _dispatch_raw(self, items: list[TweakDef], mode: str) -> None:
        """Run a batch of tweaks in a background thread (used by undo)."""
        self._set_running(True)
        threading.Thread(target=self._run_tweaks, args=(items, mode), daemon=True).start()

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

    # ── Window geometry persistence ────────────────────────────────────

    @staticmethod
    def _load_geometry() -> str | None:
        """Return saved geometry string (WxH+X+Y), or None if unavailable."""
        try:
            data = json.loads(_GEOMETRY_FILE.read_text(encoding="utf-8"))
            geo = data.get("geometry", "")
            if isinstance(geo, str) and geo:
                return geo
        except (OSError, ValueError, KeyError):
            pass
        return None

    def _save_geometry(self) -> None:
        """Persist current window geometry to disk."""
        try:
            _CONFIG_DIR.mkdir(parents=True, exist_ok=True)
            data: dict[str, str] = {"geometry": self._root.geometry()}
            _GEOMETRY_FILE.write_text(json.dumps(data), encoding="utf-8")
        except OSError:
            pass

    def _on_close(self) -> None:
        """Handle WM_DELETE_WINDOW: minimize to tray if available, else quit."""
        if self._tray_available:
            self._minimize_to_tray()
            return
        self._quit()

    def _quit(self) -> None:
        """Save state, stop tray icon, and destroy the window."""
        self._save_geometry()
        self._save_collapse_state()
        self._save_preferences()
        self._save_search_history()
        self._stop_tray()
        self._root.destroy()

    # ── System tray ────────────────────────────────────────────────────

    def _setup_tray(self) -> None:
        """Initialise system tray icon if pystray + PIL are available."""
        pystray = _try_import("pystray")
        pil_image = _try_import("PIL.Image")
        pil_draw = _try_import("PIL.ImageDraw")
        if pystray is None or pil_image is None or pil_draw is None:
            return
        self._pystray = pystray
        self._pil_image = pil_image
        self._pil_draw = pil_draw
        self._tray_available = True

    def _create_tray_icon(self) -> Any:
        """Build a pystray.Icon with a right-click menu."""
        pystray = self._pystray
        MenuItem = pystray.MenuItem
        Menu = pystray.Menu

        profile_items = [
            MenuItem(
                p.title(),
                lambda _, name=p: self._root.after(0, self._apply_profile_selection, name.title()),
            )
            for p in available_profiles()
        ]

        menu = Menu(
            MenuItem("Show Window", lambda _icon, _item: self._root.after(0, self._restore_from_tray)),
            Menu.SEPARATOR,
            MenuItem("Apply Profile", Menu(*profile_items)),
            MenuItem("Refresh Status", lambda _icon, _item: self._root.after(0, self._refresh_status_all)),
            Menu.SEPARATOR,
            MenuItem("Exit", lambda _icon, _item: self._root.after(0, self._quit)),
        )

        img = self._pil_image.new("RGB", (64, 64), color="#89b4fa")
        draw = self._pil_draw.Draw(img)
        draw.rectangle([16, 16, 48, 48], fill="#1e1e2e")
        draw.text((22, 20), "R", fill="#89b4fa")

        icon = pystray.Icon("RegiLattice", img, "RegiLattice", menu)
        icon.default = menu.items[0]  # double-click → Show Window
        return icon

    def _minimize_to_tray(self) -> None:
        """Withdraw the window and show a system-tray icon."""
        if not self._tray_available:
            return
        self._save_geometry()
        self._root.withdraw()
        if self._tray_icon is None:
            self._tray_icon = self._create_tray_icon()
            threading.Thread(target=self._tray_icon.run, daemon=True).start()

    def _restore_from_tray(self) -> None:
        """Restore the window from the system tray."""
        self._root.deiconify()
        self._root.lift()
        self._root.focus_force()

    def _stop_tray(self) -> None:
        """Stop the tray icon if running."""
        if self._tray_icon is not None:
            with contextlib.suppress(Exception):
                self._tray_icon.stop()
            self._tray_icon = None

    # ── Category collapse persistence ──────────────────────────────────

    def _save_collapse_state(self) -> None:
        """Persist which categories are collapsed to disk."""
        collapsed = [s.name for s in self._category_sections if not s.expanded]
        if not collapsed:
            with contextlib.suppress(OSError):
                _COLLAPSE_FILE.unlink(missing_ok=True)
            return
        try:
            _CONFIG_DIR.mkdir(parents=True, exist_ok=True)
            _COLLAPSE_FILE.write_text(json.dumps(collapsed), encoding="utf-8")
        except OSError:
            pass

    def _restore_collapse_state(self) -> None:
        """Restore collapsed categories from previous session."""
        try:
            data = json.loads(_COLLAPSE_FILE.read_text(encoding="utf-8"))
            if not isinstance(data, list):
                return
            collapsed_set = set(data)
            for section in self._category_sections:
                if section.name in collapsed_set and section.expanded:
                    section.toggle()
        except (OSError, ValueError):
            pass

    def _on_category_collapse_change(self, _section: CategorySection) -> None:
        """Called when a category section is toggled — saves state to disk."""
        self._save_collapse_state()

    # ── Category reorder ───────────────────────────────────────────────

    def _reorder_category(self, section: CategorySection, direction: str) -> None:
        """Move a category section up or down and repack all sections."""
        idx = self._category_sections.index(section)
        if direction == "up" and idx == 0:
            return
        if direction == "down" and idx == len(self._category_sections) - 1:
            return
        swap = idx - 1 if direction == "up" else idx + 1
        self._category_sections[idx], self._category_sections[swap] = (
            self._category_sections[swap],
            self._category_sections[idx],
        )
        # Repack all headers and content frames in the new order
        for s in self._category_sections:
            s.header.pack_forget()
            s.content_frame.pack_forget()
        for s in self._category_sections:
            s.header.pack(fill="x", pady=(8, 0), padx=4)
            if s.expanded:
                s.content_frame.pack(fill="x")
        self._save_category_order()

    def _save_category_order(self) -> None:
        """Persist current category order to disk."""
        order = [s.name for s in self._category_sections]
        try:
            _CONFIG_DIR.mkdir(parents=True, exist_ok=True)
            _CATEGORY_ORDER_FILE.write_text(json.dumps(order), encoding="utf-8")
        except OSError:
            pass

    @staticmethod
    def _load_category_order() -> list[str]:
        """Load saved category order from disk."""
        try:
            data = json.loads(_CATEGORY_ORDER_FILE.read_text(encoding="utf-8"))
            if isinstance(data, list):
                return [str(x) for x in data]
        except (OSError, ValueError, json.JSONDecodeError):
            pass
        return []

    # ── Search history persistence ─────────────────────────────────────

    @staticmethod
    def _load_search_history() -> list[str]:
        """Load recent search queries from disk."""
        try:
            data = json.loads(_SEARCH_HISTORY_FILE.read_text(encoding="utf-8"))
            if isinstance(data, list):
                return [s for s in data if isinstance(s, str)][:_MAX_SEARCH_HISTORY]
        except (OSError, ValueError):
            pass
        return []

    def _commit_search(self) -> None:
        """Add current search query to history on Enter key press."""
        query = self._search_var.get().strip()
        if not query:
            return
        # Move to top if already present, otherwise prepend
        if query in self._search_history:
            self._search_history.remove(query)
        self._search_history.insert(0, query)
        self._search_history = self._search_history[:_MAX_SEARCH_HISTORY]
        self._search_entry.configure(values=self._search_history)
        self._save_search_history()

    def _save_search_history(self) -> None:
        """Persist search history to disk."""
        try:
            _CONFIG_DIR.mkdir(parents=True, exist_ok=True)
            _SEARCH_HISTORY_FILE.write_text(json.dumps(self._search_history), encoding="utf-8")
        except OSError:
            pass

    # ── GUI preferences persistence ────────────────────────────────────

    def _save_preferences(self) -> None:
        """Persist theme, profile, status filter, and scope filter to disk."""
        prefs = {
            "theme": self._theme_var.get(),
            "profile": self._profile_var.get(),
            "status_filter": self._status_filter_var.get(),
            "scope_filter": self._scope_filter_var.get(),
        }
        try:
            _CONFIG_DIR.mkdir(parents=True, exist_ok=True)
            _PREFS_FILE.write_text(json.dumps(prefs), encoding="utf-8")
        except OSError:
            pass

    def _restore_preferences(self) -> None:
        """Restore saved preferences from previous session."""
        try:
            data = json.loads(_PREFS_FILE.read_text(encoding="utf-8"))
            if not isinstance(data, dict):
                return
        except (OSError, ValueError):
            return

        if isinstance(data.get("theme"), str):
            saved_theme = data["theme"]
            valid = {"Auto", *theme.available_themes()}
            if saved_theme in valid:
                self._theme_var.set(saved_theme)
                self._switch_theme(saved_theme)

        if isinstance(data.get("profile"), str):
            self._profile_var.set(data["profile"])

        if isinstance(data.get("status_filter"), str):
            valid_filters = {"All", "Applied", "Default", "Unknown", "Changed"}
            if data["status_filter"] in valid_filters:
                self._status_filter_var.set(data["status_filter"])

        if isinstance(data.get("scope_filter"), str):
            valid_scopes = {"All", "User Only", "Machine Only", "Both"}
            if data["scope_filter"] in valid_scopes:
                self._scope_filter_var.set(data["scope_filter"])

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
