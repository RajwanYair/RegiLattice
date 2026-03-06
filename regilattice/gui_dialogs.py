"""Dialog windows and import/export helpers for RegiLattice GUI."""

from __future__ import annotations

import json
import sys
import threading
import tkinter as tk
from collections.abc import Callable, Sequence
from pathlib import Path
from tkinter import filedialog, messagebox, ttk
from typing import Any

from . import __version__
from . import gui_theme as theme
from .registry import SESSION, platform_summary
from .tweaks import TweakDef, all_tweaks, tweaks_by_category

# ── Theme aliases ────────────────────────────────────────────────────────────

_ACCENT = theme.ACCENT
_BG = theme.BG
_FG = theme.FG
_FG_DIM = theme.FG_DIM
_CARD_BG = theme.CARD_BG
_OK_GREEN = theme.OK_GREEN
_ERR_RED = theme.ERR_RED

_FONT = theme.FONT
_FONT_SM = theme.FONT_SM
_FONT_XS = theme.FONT_XS
_FONT_XS_BOLD = theme.FONT_XS_BOLD
_FONT_BOLD = theme.FONT_BOLD
_FONT_TITLE = theme.FONT_TITLE


# ── Import JSON ──────────────────────────────────────────────────────────────


def import_json_selection(
    root: tk.Tk,
    tweak_rows: Sequence[Any],
    deselect_all: Callable[[], None],
    update_selection_count: Callable[[], None],
    set_status: Callable[..., None],
) -> None:
    """Import a JSON file containing a list of tweak IDs to select.

    Parameters are kept generic to avoid circular imports with gui.py types.
    """
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

    deselect_all()
    count = 0
    for row in tweak_rows:
        if row.td.id in ids and not row.disabled_by_corp:
            row.var.set(True)
            count += 1
    update_selection_count()
    set_status(f"Imported {count} tweaks from {Path(path).name}", _OK_GREEN)


# ── Export PowerShell ────────────────────────────────────────────────────────


def export_powershell(selected: list[TweakDef], set_status: Callable[..., None]) -> None:
    """Export selected tweaks as a .ps1 script showing the registry changes."""
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
        "# RegiLattice \u2014 Exported Tweaks",
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
        lines.append(f"# \u2500\u2500 {td.label} ({'admin' if td.needs_admin else 'user'}) \u2500\u2500")
        if td.description:
            lines.append(f"# {td.description}")
        for key in td.registry_keys:
            ps_key = key.replace("HKEY_LOCAL_MACHINE", "HKLM:")
            ps_key = ps_key.replace("HKEY_CURRENT_USER", "HKCU:")
            lines.append(f"# Ensure key exists: {ps_key}")
            lines.append(f"if (-not (Test-Path '{ps_key}')) {{ New-Item -Path '{ps_key}' -Force | Out-Null }}")
        lines.append(f"Write-Host 'Applied: {td.label}' -ForegroundColor Cyan")
        lines.append("")
    lines.append("Write-Host 'Done! All tweaks applied.' -ForegroundColor Green")
    try:
        with open(path, "w", encoding="utf-8-sig") as f:
            f.write("\n".join(lines))
        set_status(f"Exported {len(selected)} tweaks \u2192 {path}", _OK_GREEN)
    except OSError as exc:
        messagebox.showerror("Export Error", str(exc))


# ── Scoop Tools Manager ─────────────────────────────────────────────────────


def open_scoop_manager(root: tk.Tk, refresh_status_all: Callable[[], None]) -> None:
    """Open a Scoop Tools manager dialog showing installed packages."""
    from .tweaks.scoop_tools import _install_scoop_app, _remove_scoop_app, _scoop_installed, list_installed_scoop_apps

    dlg = tk.Toplevel(root)
    dlg.title("Scoop Tools Manager")
    dlg.geometry("620x520")
    dlg.configure(bg=_BG)
    dlg.transient(root)
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
            refresh_status_all()
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
        refresh_status_all()

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


# ── About dialog ─────────────────────────────────────────────────────────────


def show_about(corp_blocked: bool) -> None:
    """Show an About dialog with system and project info."""
    total = len(all_tweaks())
    cats = len(tweaks_by_category())
    corp = "Yes" if corp_blocked else "No"
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
