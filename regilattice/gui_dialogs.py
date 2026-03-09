"""Dialog windows and import/export helpers for RegiLattice GUI."""

from __future__ import annotations

import json
import re
import subprocess
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

__all__ = [
    "export_json_selection",
    "export_powershell",
    "import_json_selection",
    "open_psmodule_manager",
    "open_scoop_manager",
    "run_powershell_command",
    "show_about",
]

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

# ── Validation helpers ───────────────────────────────────────────────────────

# Only allow alphanumeric, hyphens, dots, underscores (covers all valid PS module / scoop app names)
_SAFE_PACKAGE_RE = re.compile(r"^[A-Za-z0-9._\-]+$")


def _validate_package_name(name: str) -> str:
    """Raise ValueError if *name* contains characters that could enable injection.

    Returns the validated name unchanged if safe.
    """
    if not name or not _SAFE_PACKAGE_RE.match(name):
        msg = f"Invalid package name {name!r}: only letters, digits, '.', '_', '-' allowed."
        raise ValueError(msg)
    return name


def run_powershell_command(command: str, *, timeout: int = 60) -> str:
    """Run a PowerShell command string and return stdout.

    The *command* string must **not** contain user-supplied data — callers are
    responsible for validating any runtime values (e.g. via :func:`_validate_package_name`)
    before interpolating them into the command string.

    Raises :class:`RuntimeError` on non-zero exit.
    """
    result = subprocess.run(
        ["powershell", "-NoProfile", "-NonInteractive", "-Command", command],
        capture_output=True,
        text=True,
        timeout=timeout,
    )
    if result.returncode != 0:
        raise RuntimeError(result.stderr.strip() or f"Exit code {result.returncode}")
    return result.stdout.strip()


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
    try:
        path = filedialog.askopenfilename(
            parent=root,
            title="Import Tweak Selection",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")],
        )
    except tk.TclError:
        return
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


# ── Export JSON ──────────────────────────────────────────────────────────────


def export_json_selection(selected: list[TweakDef], set_status: Callable[..., None], *, parent: tk.Misc | None = None) -> None:
    """Export selected tweak IDs as a JSON file for sharing/reimporting."""
    if not selected:
        messagebox.showinfo("Nothing Selected", "Select at least one tweak to export.")
        return
    try:
        path = filedialog.asksaveasfilename(
            parent=parent,
            title="Export Tweak Selection",
            defaultextension=".json",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")],
            initialfile="regilattice_selection.json",
        )
    except tk.TclError:
        return
    if not path:
        return
    data = {
        "version": __version__,
        "tweaks": [td.id for td in selected],
    }
    try:
        with open(path, "w", encoding="utf-8") as f:
            json.dump(data, f, indent=2)
        set_status(f"Exported {len(selected)} tweak IDs \u2192 {Path(path).name}", _OK_GREEN)
    except OSError as exc:
        messagebox.showerror("Export Error", str(exc))


# ── Export PowerShell ────────────────────────────────────────────────────────


def export_powershell(selected: list[TweakDef], set_status: Callable[..., None], *, parent: tk.Misc | None = None) -> None:
    """Export selected tweaks as a .ps1 script showing the registry changes."""
    if not selected:
        messagebox.showinfo("Nothing Selected", "Select at least one tweak to export.")
        return
    try:
        path = filedialog.asksaveasfilename(
            parent=parent,
            title="Export PowerShell Script",
            defaultextension=".ps1",
            filetypes=[("PowerShell", "*.ps1"), ("All files", "*.*")],
            initialfile="regilattice_tweaks.ps1",
        )
    except tk.TclError:
        return
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
        raw_name = install_var.get().strip()
        if not raw_name:
            return
        try:
            name = _validate_package_name(raw_name)
        except ValueError as exc:
            messagebox.showerror("Invalid Name", str(exc), parent=dlg)
            return
        status_lbl.configure(text=f"Installing {name}...")
        dlg.update()
        try:
            _install_scoop_app(name)
            status_lbl.configure(text=f"Installed {name} \u2714")
            _refresh_list()
            refresh_status_all()
        except RuntimeError as exc:
            messagebox.showerror("Install Error", str(exc), parent=dlg)

    def _remove_action() -> None:
        sel = listbox.curselection()  # type: ignore[no-untyped-call]
        if not sel:
            messagebox.showinfo("Select Package", "Select a package to remove.", parent=dlg)
            return
        raw_name = listbox.get(sel[0])
        try:
            name = _validate_package_name(raw_name)
        except ValueError as exc:
            messagebox.showerror("Invalid Name", str(exc), parent=dlg)
            return
        if not messagebox.askyesno("Confirm Remove", f"Remove '{name}'?", parent=dlg):
            return
        status_lbl.configure(text=f"Removing {name}...")
        dlg.update()
        try:
            _remove_scoop_app(name)
            status_lbl.configure(text=f"Removed {name} \u2714")
            _refresh_list()
            refresh_status_all()
        except RuntimeError as exc:
            messagebox.showerror("Remove Error", str(exc), parent=dlg)
            status_lbl.configure(text="Remove failed.")

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


# ── PowerShell Modules Manager ───────────────────────────────────────────────


def open_psmodule_manager(root: tk.Tk, refresh_status_all: Callable[[], None]) -> None:
    """Open a PowerShell Modules manager dialog for listing, installing and removing modules."""
    dlg = tk.Toplevel(root)
    dlg.title("PowerShell Modules Manager")
    dlg.geometry("720x580")
    dlg.configure(bg=_BG)
    dlg.transient(root)
    dlg.grab_set()

    # Header
    tk.Label(dlg, text="\U0001f9e9  PowerShell Modules Manager", bg=_BG, fg=_FG, font=_FONT_TITLE).pack(
        padx=16,
        pady=(12, 4),
        anchor="w",
    )

    # Scope selector (CurrentUser / AllUsers)
    scope_var = tk.StringVar(value="CurrentUser")
    scope_frame = tk.Frame(dlg, bg=_BG)
    scope_frame.pack(fill="x", padx=16, pady=(0, 4))
    tk.Label(scope_frame, text="Scope:", bg=_BG, fg=_FG_DIM, font=_FONT_SM).pack(side="left")
    for scope in ("CurrentUser", "AllUsers"):
        tk.Radiobutton(
            scope_frame,
            text=scope,
            variable=scope_var,
            value=scope,
            bg=_BG,
            fg=_FG,
            selectcolor=_CARD_BG,
            font=_FONT_SM,
            activebackground=_BG,
        ).pack(side="left", padx=(6, 0))

    # Status label
    status_lbl = tk.Label(dlg, text="Ready — click Refresh to list installed modules.", bg=_BG, fg=_FG_DIM, font=_FONT_SM)
    status_lbl.pack(padx=16, pady=(0, 4), anchor="w")

    # Module list
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

    def _run_ps(command: str) -> str:
        """Run a PowerShell command and return stdout. Raises RuntimeError on failure."""
        return run_powershell_command(command)

    def _refresh_list() -> None:
        status_lbl.configure(text="Scanning installed modules\u2026")
        dlg.update()
        listbox.delete(0, "end")
        try:
            scope = scope_var.get()
            scope_flag = f"-Scope {scope}" if scope == "CurrentUser" else ""
            raw = _run_ps(f"Get-InstalledModule {scope_flag} | Sort-Object Name | ForEach-Object {{{{ '{{0}}  v{{1}}' -f $_.Name, $_.Version }}}}")
            lines = [ln for ln in raw.splitlines() if ln.strip()]
            for ln in lines:
                listbox.insert("end", ln)
            status_lbl.configure(text=f"{len(lines)} module(s) installed ({scope})")
        except (RuntimeError, FileNotFoundError) as exc:
            status_lbl.configure(text=f"Error: {exc}")

    # Install / Remove controls
    ctrl = tk.Frame(dlg, bg=_BG)
    ctrl.pack(fill="x", padx=16, pady=(4, 8))

    install_var = tk.StringVar()
    tk.Label(ctrl, text="Module:", bg=_BG, fg=_FG, font=_FONT_SM).pack(side="left")
    entry = ttk.Entry(ctrl, textvariable=install_var, font=_FONT, width=24)
    entry.pack(side="left", padx=(4, 4))

    def _install_action() -> None:
        raw_name = install_var.get().strip()
        if not raw_name:
            return
        try:
            name = _validate_package_name(raw_name)
        except ValueError as exc:
            messagebox.showerror("Invalid Name", str(exc), parent=dlg)
            return
        scope = scope_var.get()
        status_lbl.configure(text=f"Installing {name}\u2026")
        dlg.update()
        try:
            _run_ps(f"Install-Module -Name '{name}' -Scope {scope} -Force -AllowClobber")
            status_lbl.configure(text=f"Installed {name} \u2714")
            _refresh_list()
            refresh_status_all()
        except (RuntimeError, FileNotFoundError) as exc:
            messagebox.showerror("Install Error", str(exc), parent=dlg)
            status_lbl.configure(text="Install failed.")

    def _remove_action() -> None:
        sel = listbox.curselection()  # type: ignore[no-untyped-call]
        if not sel:
            messagebox.showinfo("Select Module", "Select a module to remove.", parent=dlg)
            return
        raw_name = listbox.get(sel[0]).split()[0]  # strip version from display string
        try:
            name = _validate_package_name(raw_name)
        except ValueError as exc:
            messagebox.showerror("Invalid Name", str(exc), parent=dlg)
            return
        if not messagebox.askyesno("Confirm Remove", f"Uninstall all versions of '{name}'?", parent=dlg):
            return
        status_lbl.configure(text=f"Removing {name}\u2026")
        dlg.update()
        try:
            _run_ps(f"Uninstall-Module -Name '{name}' -AllVersions -Force")
            status_lbl.configure(text=f"Removed {name} \u2714")
            _refresh_list()
            refresh_status_all()
        except (RuntimeError, FileNotFoundError) as exc:
            messagebox.showerror("Remove Error", str(exc), parent=dlg)
            status_lbl.configure(text="Remove failed.")

    def _update_action() -> None:
        sel = listbox.curselection()  # type: ignore[no-untyped-call]
        if not sel:
            messagebox.showinfo("Select Module", "Select a module to update.", parent=dlg)
            return
        raw_name = listbox.get(sel[0]).split()[0]
        try:
            name = _validate_package_name(raw_name)
        except ValueError as exc:
            messagebox.showerror("Invalid Name", str(exc), parent=dlg)
            return
        scope = scope_var.get()
        status_lbl.configure(text=f"Updating {name}\u2026")
        dlg.update()
        try:
            _run_ps(f"Update-Module -Name '{name}' -Scope {scope} -Force")
            status_lbl.configure(text=f"Updated {name} \u2714")
            _refresh_list()
        except (RuntimeError, FileNotFoundError) as exc:
            messagebox.showerror("Update Error", str(exc), parent=dlg)
            status_lbl.configure(text="Update failed.")

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
        text="Update Selected",
        bg="#1E66F5",
        fg="white",
        font=_FONT_XS_BOLD,
        relief="flat",
        padx=8,
        command=lambda: threading.Thread(target=_update_action, daemon=True).start(),
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

    # Quick install popular PowerShell modules
    pop_frame = tk.LabelFrame(dlg, text="Quick Install Popular Modules", bg=_BG, fg=_FG_DIM, font=_FONT_XS)
    pop_frame.pack(fill="x", padx=16, pady=(0, 8))
    popular_modules = [
        "PSReadLine",
        "posh-git",
        "PowerShellGet",
        "Az",
        "Microsoft.Graph",
        "Terminal-Icons",
        "oh-my-posh",
        "PSScriptAnalyzer",
        "Pester",
        "SqlServer",
    ]
    for i, mod in enumerate(popular_modules):
        tk.Button(
            pop_frame,
            text=mod,
            bg=_CARD_BG,
            fg=_ACCENT,
            font=_FONT_XS,
            relief="flat",
            padx=4,
            pady=1,
            cursor="hand2",
            command=lambda m=mod: install_var.set(m),  # type: ignore[misc]
        ).grid(row=i // 5, column=i % 5, padx=2, pady=2, sticky="ew")


# ── About dialog ─────────────────────────────────────────────────────────────


def show_about(corp_blocked: bool, hw_summary: str = "") -> None:
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
    ]
    if hw_summary:
        info_lines += ["", "─── Hardware ───", hw_summary]
    info_lines += [
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
