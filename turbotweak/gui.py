"""Tkinter GUI for TurboTweak – selective tweak application.

Launch via ``python -m turbotweak --gui`` or ``turbotweak --gui``.
"""

from __future__ import annotations

import sys
import threading
import tkinter as tk
from tkinter import messagebox, ttk
from dataclasses import dataclass
from typing import Callable, List, Optional

from . import __version__, tweaks
from .corpguard import (
    CorporateNetworkError,
    assert_not_corporate,
    corp_guard_status,
    is_corporate_network,
)
from .registry import (
    SESSION,
    AdminRequirementError,
    is_windows,
    platform_summary,
)


# ── Tweak descriptor ────────────────────────────────────────────────────────

@dataclass
class TweakDef:
    """Describes one toggle-able tweak (apply + remove pair)."""

    label: str
    apply_fn: Callable[[], None]
    remove_fn: Callable[[], None]
    needs_admin: bool = True
    category: str = "General"


_TWEAKS: List[TweakDef] = [
    # ── Core tweaks ──────────────────────────────────────────────────────
    TweakDef(
        "Take Ownership Context Menu",
        tweaks.add_take_ownership,
        tweaks.remove_take_ownership,
        needs_admin=True,
        category="Shell",
    ),
    TweakDef(
        "Recent Folders in Quick Access",
        tweaks.add_recent_places,
        tweaks.remove_recent_places,
        needs_admin=False,
        category="Explorer",
    ),
    TweakDef(
        "Verbose Boot Messages",
        tweaks.enable_verbose_boot,
        tweaks.disable_verbose_boot,
        needs_admin=True,
        category="Boot",
    ),
    TweakDef(
        "Performance Tweaks (Visual Effects)",
        tweaks.apply_performance_tweaks,
        tweaks.remove_performance_tweaks,
        needs_admin=True,
        category="Performance",
    ),
    TweakDef(
        "Registry Auto-Backup Task",
        tweaks.enable_registry_backup,
        tweaks.disable_registry_backup,
        needs_admin=True,
        category="Maintenance",
    ),
    # ── New tweaks ───────────────────────────────────────────────────────
    TweakDef(
        "Disable Telemetry",
        tweaks.disable_telemetry,
        tweaks.enable_telemetry,
        needs_admin=True,
        category="Privacy",
    ),
    TweakDef(
        "Disable Cortana",
        tweaks.disable_cortana,
        tweaks.enable_cortana,
        needs_admin=True,
        category="Privacy",
    ),
    TweakDef(
        "Disable Mouse Acceleration",
        tweaks.disable_mouse_accel,
        tweaks.enable_mouse_accel,
        needs_admin=False,
        category="Input",
    ),
    TweakDef(
        "Disable Game DVR / Game Bar",
        tweaks.disable_game_dvr,
        tweaks.enable_game_dvr,
        needs_admin=True,
        category="Gaming",
    ),
    TweakDef(
        "Optimize SvcHost Split (RAM-based)",
        tweaks.optimize_svchost_split,
        tweaks.restore_svchost_split,
        needs_admin=True,
        category="Performance",
    ),
    TweakDef(
        "Disable NTFS Last Access Timestamp",
        tweaks.disable_last_access,
        tweaks.enable_last_access,
        needs_admin=True,
        category="Performance",
    ),
    TweakDef(
        "Enable Long Paths (260-char bypass)",
        tweaks.enable_long_paths,
        tweaks.disable_long_paths,
        needs_admin=True,
        category="System",
    ),
]


# ── GUI  ─────────────────────────────────────────────────────────────────────

_ACCENT = "#1E90FF"
_BG = "#1E1E2E"
_FG = "#CDD6F4"
_FG_DIM = "#6C7086"
_CARD_BG = "#313244"
_OK_GREEN = "#A6E3A1"
_WARN_YELLOW = "#F9E2AF"
_ERR_RED = "#F38BA8"
_HEADER_BG = "#181825"


class TurboTweakGUI:
    """Main application window."""

    def __init__(self) -> None:
        self._root = tk.Tk()
        self._root.title(f"TurboTweak  v{__version__}")
        self._root.geometry("720x780")
        self._root.minsize(540, 520)
        self._root.configure(bg=_BG)
        self._root.resizable(True, True)

        # Try to set the window icon (fail silently)
        try:
            self._root.iconbitmap(default="")
        except Exception:
            pass

        # ── Style ────────────────────────────────────────────────────────
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
            background=[("active", _CARD_BG)],
            foreground=[("active", _FG)],
        )
        style.configure(
            "TButton",
            padding=(14, 6),
            font=("Segoe UI Semibold", 10),
        )
        style.configure("Apply.TButton", foreground="white", background="#40A02B")
        style.map("Apply.TButton", background=[("active", "#2E7D32")])
        style.configure("Remove.TButton", foreground="white", background="#E64A19")
        style.map("Remove.TButton", background=[("active", "#BF360C")])
        style.configure("Restore.TButton", foreground="white", background="#7C3AED")
        style.map("Restore.TButton", background=[("active", "#5B21B6")])
        style.configure(
            "TLabel", background=_BG, foreground=_FG, font=("Segoe UI", 10)
        )
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
            font=("Segoe UI Semibold", 10),
        )
        style.configure(
            "CorpWarn.TLabel",
            background=_ERR_RED,
            foreground="#1E1E2E",
            font=("Segoe UI Semibold", 10),
        )

        # ── Header ───────────────────────────────────────────────────────
        header = ttk.Frame(self._root, style="Header.TFrame")
        header.pack(fill="x", padx=0, pady=0)

        ttk.Label(
            header,
            text="⚡ TurboTweak",
            style="Title.TLabel",
        ).pack(side="left", padx=16, pady=(12, 2))

        ttk.Label(
            header,
            text=f"v{__version__}  |  {platform_summary()}",
            style="Subtitle.TLabel",
        ).pack(side="left", padx=4, pady=(14, 2))

        # ── Corporate-guard banner ───────────────────────────────────────
        self._corp_blocked = is_corporate_network()
        self._corp_banner_frame: Optional[tk.Frame] = None
        if self._corp_blocked:
            corp_info = corp_guard_status() or "corporate environment detected"
            self._corp_banner_frame = tk.Frame(self._root, bg=_ERR_RED)
            self._corp_banner_frame.pack(fill="x")
            tk.Label(
                self._corp_banner_frame,
                text=f"  🛑  Corporate network detected: {corp_info} — tweaks are blocked",
                bg=_ERR_RED,
                fg="#1E1E2E",
                font=("Segoe UI Semibold", 10),
                anchor="w",
                padx=12,
                pady=6,
            ).pack(fill="x")

        # ── Select-all / deselect-all bar ────────────────────────────────
        toolbar = ttk.Frame(self._root)
        toolbar.pack(fill="x", padx=16, pady=(10, 0))

        ttk.Button(toolbar, text="Select All", command=self._select_all).pack(
            side="left", padx=(0, 4)
        )
        ttk.Button(toolbar, text="Deselect All", command=self._deselect_all).pack(
            side="left", padx=(0, 4)
        )
        self._force_var = tk.BooleanVar(value=False)
        force_cb = ttk.Checkbutton(
            toolbar,
            text="Force (bypass corp guard)",
            variable=self._force_var,
            style="TCheckbutton",
        )
        force_cb.pack(side="right", padx=(8, 0))

        # ── Scrollable tweak checklist ───────────────────────────────────
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

        # Mouse wheel scrolling
        def _on_mousewheel(event: tk.Event) -> None:  # type: ignore[type-arg]
            canvas.yview_scroll(int(-1 * (event.delta / 120)), "units")

        canvas.bind_all("<MouseWheel>", _on_mousewheel)

        # Populate checkboxes grouped by category
        self._vars: List[tk.BooleanVar] = []
        last_cat = ""
        for td in _TWEAKS:
            if td.category != last_cat:
                last_cat = td.category
                ttk.Label(
                    self._inner,
                    text=f"  {td.category}",
                    style="Category.TLabel",
                ).pack(fill="x", pady=(8, 2), padx=4)

            var = tk.BooleanVar(value=True)
            self._vars.append(var)

            row = ttk.Frame(self._inner, style="Card.TFrame")
            row.pack(fill="x", padx=4, pady=2, ipady=2)

            cb = ttk.Checkbutton(row, text=td.label, variable=var)
            cb.pack(side="left", padx=(8, 4), pady=2)

            if td.needs_admin:
                ttk.Label(
                    row,
                    text="(admin)",
                    foreground=_FG_DIM,
                    background=_CARD_BG,
                    font=("Segoe UI", 8),
                ).pack(side="right", padx=(0, 10))

        # ── Action buttons ───────────────────────────────────────────────
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

        ttk.Button(
            btn_frame,
            text="🛡  Restore Point",
            style="Restore.TButton",
            command=lambda: self._dispatch("restore"),
        ).pack(side="left", expand=True, fill="x")

        # ── Status / progress bar ────────────────────────────────────────
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

    # ── Helpers ──────────────────────────────────────────────────────────

    def _select_all(self) -> None:
        for v in self._vars:
            v.set(True)

    def _deselect_all(self) -> None:
        for v in self._vars:
            v.set(False)

    def _set_status(self, text: str, color: str = _FG_DIM) -> None:
        self._status_label.configure(text=text, foreground=color)

    def _selected_tweaks(self) -> List[TweakDef]:
        return [td for td, v in zip(_TWEAKS, self._vars) if v.get()]

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
            threading.Thread(
                target=self._run_restore_point, daemon=True
            ).start()
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
            self._root.after(
                0, self._progress.configure, {"value": pct}
            )
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
            except Exception as exc:
                errors.append(f"{td.label}: {exc}")
                SESSION.log(f"[GUI] Error ({mode}) {td.label}: {exc}")

        # Final status
        summary = f"{'Applied' if mode == 'apply' else 'Removed'} {total - len(errors)}/{total} tweaks"
        if errors:
            summary += f"  •  {len(errors)} error(s)"
        color = _OK_GREEN if not errors else _WARN_YELLOW

        self._root.after(0, self._set_status, summary, color)
        self._root.after(0, self._progress.configure, {"value": 100})

        if errors:
            err_text = "\n".join(errors)
            self._root.after(
                0,
                lambda: messagebox.showwarning(
                    "Completed with Errors",
                    f"{summary}\n\n{err_text}",
                ),
            )

    def _run_restore_point(self) -> None:
        try:
            tweaks.create_restore_point()
            self._root.after(
                0, self._set_status, "Restore point created ✔", _OK_GREEN
            )
        except AdminRequirementError:
            self._root.after(
                0,
                lambda: messagebox.showwarning(
                    "Admin Required",
                    "Creating a restore point requires admin elevation.",
                ),
            )
            self._root.after(0, self._set_status, "Restore point failed (admin)", _ERR_RED)
        except Exception as exc:  # noqa: BLE001
            err_msg = str(exc)
            self._root.after(
                0,
                lambda m=err_msg: messagebox.showerror(
                    "Error", m),
            )
            self._root.after(
                0, self._set_status,
                f"Restore point error: {err_msg}",
                _ERR_RED,
            )

    # ── Entry point ──────────────────────────────────────────────────────

    def run(self) -> None:
        """Start the main event loop."""
        self._root.mainloop()


def launch() -> None:
    """Convenience entry point used by CLI ``--gui``."""
    if not is_windows():
        print(f"⚠️  TurboTweak GUI requires Windows. Detected: {platform_summary()}")
        sys.exit(1)
    app = TurboTweakGUI()
    app.run()
