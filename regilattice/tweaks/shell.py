"""Shell tweaks — context-menu helpers & Explorer shell settings."""

from __future__ import annotations

import subprocess

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_KEYS = [
    r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership\command",
    r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership\command",
    r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership\command",
]

_CMD_FILE = (
    'cmd.exe /k takeown /f "%1" && icacls "%1"' " /grant *S-1-3-4:F /t /c /l && pause"
)
_CMD_DIR = (
    'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1"'
    " /grant *S-1-3-4:F /t /c /q && pause"
)
_CMD_DRIVE = (
    'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1"'
    " /grant *S-1-3-4:F /t /c && pause"
)


def _add_context_entry(base: str, command: str) -> None:
    def _run(a: list[str]) -> subprocess.CompletedProcess[str]:
        return subprocess.run(["reg", *a], check=True, capture_output=True, text=True)

    _run(["add", base, "/f"])
    _run(["add", base, "/ve", "/d", "Take Ownership", "/f"])
    _run(["add", base, "/v", "NoWorkingDirectory", "/d", "", "/f"])
    _run(["add", base, "/v", "Extended", "/d", "", "/f"])
    _run(["add", f"{base}\\command", "/f"])
    _run(["add", f"{base}\\command", "/ve", "/d", command, "/f"])


# ── Functions ────────────────────────────────────────────────────────────────


def apply_take_ownership(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-TakeOwnership")
    SESSION.backup(_KEYS, "TakeOwnership")
    _add_context_entry(r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership", _CMD_FILE)
    _add_context_entry(r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership", _CMD_DIR)
    _add_context_entry(r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership", _CMD_DRIVE)
    SESSION.log("Completed Add-TakeOwnership")


def remove_take_ownership(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-TakeOwnership")
    top_keys = [k for k in _KEYS if not k.endswith("\\command")]
    SESSION.backup(top_keys, "TakeOwnership_Remove")
    for key in top_keys:
        subprocess.run(
            ["reg", "delete", key, "/f"],
            check=False,
            capture_output=True,
        )
    SESSION.log("Completed Remove-TakeOwnership")


def detect_take_ownership() -> bool:
    return SESSION.key_exists(_KEYS[0])


# ── Open Command Prompt Here (Context Menu) ───────────────────────────────

_CMD_HERE_KEY = r"HKEY_CLASSES_ROOT\Directory\Background\shell\cmd_here"
_CMD_HERE_CMD = rf"{_CMD_HERE_KEY}\command"
_CMD_KEYS = [_CMD_HERE_KEY, _CMD_HERE_CMD]


def _apply_cmd_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: add 'Open Command Prompt Here' context menu")
    SESSION.backup(_CMD_KEYS, "CmdHere")

    def _run(a: list[str]) -> subprocess.CompletedProcess[str]:
        return subprocess.run(["reg", *a], check=True, capture_output=True, text=True)

    _run(["add", _CMD_HERE_KEY, "/ve", "/d", "Open Command Prompt Here", "/f"])
    _run(["add", _CMD_HERE_KEY, "/v", "Icon", "/d", "cmd.exe", "/f"])
    _run(["add", _CMD_HERE_CMD, "/ve", "/d", 'cmd.exe /k cd /d "%V"', "/f"])


def _remove_cmd_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["reg", "delete", _CMD_HERE_KEY, "/f"],
        check=False, capture_output=True,
    )


def _detect_cmd_here() -> bool:
    return SESSION.key_exists(_CMD_HERE_KEY)


# ── Add Hash File Context Menu ────────────────────────────────────────────

_HASH_KEY = r"HKEY_CLASSES_ROOT\*\shell\GetFileHash"
_HASH_CMD = rf"{_HASH_KEY}\command"
_HASH_KEYS = [_HASH_KEY, _HASH_CMD]


def _apply_hash_context(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: add 'Get File Hash' context menu")
    SESSION.backup(_HASH_KEYS, "FileHash")

    def _run(a: list[str]) -> subprocess.CompletedProcess[str]:
        return subprocess.run(["reg", *a], check=True, capture_output=True, text=True)

    _run(["add", _HASH_KEY, "/ve", "/d", "Get File Hash (SHA256)", "/f"])
    _run(["add", _HASH_KEY, "/v", "Icon", "/d", "powershell.exe", "/f"])
    ps_cmd = (
        'powershell.exe -NoProfile -Command "'
        "Get-FileHash '%1' -Algorithm SHA256 | "
        "Format-List; pause"
        '"'
    )
    _run(["add", _HASH_CMD, "/ve", "/d", ps_cmd, "/f"])


def _remove_hash_context(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["reg", "delete", _HASH_KEY, "/f"],
        check=False, capture_output=True,
    )


def _detect_hash_context() -> bool:
    return SESSION.key_exists(_HASH_KEY)


# ── Open PowerShell Here (Context Menu) ──────────────────────────────────────────────────────

_PS_HERE_KEY = r"HKEY_CLASSES_ROOT\Directory\Background\shell\powershell_here"
_PS_HERE_CMD = rf"{_PS_HERE_KEY}\command"
_PS_HERE_KEYS = [_PS_HERE_KEY, _PS_HERE_CMD]


def _apply_ps_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: add 'Open PowerShell Here' context menu")
    SESSION.backup(_PS_HERE_KEYS, "PsHere")

    def _run(a: list[str]) -> subprocess.CompletedProcess[str]:
        return subprocess.run(["reg", *a], check=True, capture_output=True, text=True)

    _run(["add", _PS_HERE_KEY, "/ve", "/d", "Open PowerShell Here", "/f"])
    _run(["add", _PS_HERE_KEY, "/v", "Icon", "/d", "powershell.exe", "/f"])
    _run(["add", _PS_HERE_CMD, "/ve", "/d", 'powershell.exe -NoExit -Command "Set-Location \'%V\'"', "/f"])


def _remove_ps_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(["reg", "delete", _PS_HERE_KEY, "/f"], check=False, capture_output=True)


def _detect_ps_here() -> bool:
    return SESSION.key_exists(_PS_HERE_KEY)


# ── Open Windows Terminal Here (Context Menu) ────────────────────────────────────────────────

_WT_HERE_KEY = r"HKEY_CLASSES_ROOT\Directory\Background\shell\wt_here"
_WT_HERE_CMD = rf"{_WT_HERE_KEY}\command"


def _apply_wt_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: add 'Open Windows Terminal Here' context menu")
    SESSION.backup([_WT_HERE_KEY], "WtHere")

    def _run(a: list[str]) -> subprocess.CompletedProcess[str]:
        return subprocess.run(["reg", *a], check=True, capture_output=True, text=True)

    _run(["add", _WT_HERE_KEY, "/ve", "/d", "Open Terminal Here", "/f"])
    _run(["add", _WT_HERE_KEY, "/v", "Icon", "/d", "wt.exe", "/f"])
    _run(["add", _WT_HERE_CMD, "/ve", "/d", 'wt.exe -d "%V"', "/f"])


def _remove_wt_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(["reg", "delete", _WT_HERE_KEY, "/f"], check=False, capture_output=True)


def _detect_wt_here() -> bool:
    return SESSION.key_exists(_WT_HERE_KEY)


# ── Restore Classic Context Menu (Win11) ─────────────────────────────────────

_CLASSIC_CTX_KEY = (
    r"HKEY_CURRENT_USER\Software\Classes\CLSID"
    r"\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"
)


def _apply_classic_context_menu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: restore Windows 10 classic context menu")
    SESSION.backup([_CLASSIC_CTX_KEY], "ClassicContextMenu")
    SESSION.set_string(_CLASSIC_CTX_KEY, None, "")


def _remove_classic_context_menu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_tree(
        r"HKEY_CURRENT_USER\Software\Classes\CLSID"
        r"\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}"
    )


def _detect_classic_context_menu() -> bool:
    return SESSION.key_exists(_CLASSIC_CTX_KEY)


# ── Disable Recent Files in Quick Access ─────────────────────────────────────

_EXPLORER_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"
)


def _apply_disable_recent_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disable recent files in Quick Access")
    SESSION.backup([_EXPLORER_CU], "DisableRecentFiles")
    SESSION.set_dword(_EXPLORER_CU, "ShowRecent", 0)


def _remove_disable_recent_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_EXPLORER_CU], "DisableRecentFiles_Remove")
    SESSION.set_dword(_EXPLORER_CU, "ShowRecent", 1)


def _detect_disable_recent_files() -> bool:
    return SESSION.read_dword(_EXPLORER_CU, "ShowRecent") == 0


# ── Disable Frequent Folders in Quick Access ─────────────────────────────────


def _apply_disable_frequent_folders(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disable frequent folders in Quick Access")
    SESSION.backup([_EXPLORER_CU], "DisableFrequentFolders")
    SESSION.set_dword(_EXPLORER_CU, "ShowFrequent", 0)


def _remove_disable_frequent_folders(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_EXPLORER_CU], "DisableFrequentFolders_Remove")
    SESSION.set_dword(_EXPLORER_CU, "ShowFrequent", 1)


def _detect_disable_frequent_folders() -> bool:
    return SESSION.read_dword(_EXPLORER_CU, "ShowFrequent") == 0


# ── Enable Compact View in File Explorer ─────────────────────────────────────

_ADV = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
)


def _apply_compact_file_explorer(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: enable compact view in File Explorer")
    SESSION.backup([_ADV], "CompactFileExplorer")
    SESSION.set_dword(_ADV, "UseCompactMode", 1)


def _remove_compact_file_explorer(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "CompactFileExplorer_Remove")
    SESSION.set_dword(_ADV, "UseCompactMode", 0)


def _detect_compact_file_explorer() -> bool:
    return SESSION.read_dword(_ADV, "UseCompactMode") == 1


# ── Show File Extensions in Explorer ─────────────────────────────────────────


def _apply_show_file_extensions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: show file extensions")
    SESSION.backup([_ADV], "ShowFileExtensions")
    SESSION.set_dword(_ADV, "HideFileExt", 0)


def _remove_show_file_extensions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "ShowFileExtensions_Remove")
    SESSION.set_dword(_ADV, "HideFileExt", 1)


def _detect_show_file_extensions() -> bool:
    return SESSION.read_dword(_ADV, "HideFileExt") == 0


# ── Show Hidden Files in Explorer ────────────────────────────────────────────


def _apply_show_hidden_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: show hidden files")
    SESSION.backup([_ADV], "ShowHiddenFiles")
    SESSION.set_dword(_ADV, "Hidden", 1)


def _remove_show_hidden_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "ShowHiddenFiles_Remove")
    SESSION.set_dword(_ADV, "Hidden", 2)


def _detect_show_hidden_files() -> bool:
    return SESSION.read_dword(_ADV, "Hidden") == 1


# ── Disable Aero Shake ───────────────────────────────────────────────────────


def _apply_disable_aero_shake(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disable Aero Shake")
    SESSION.backup([_ADV], "DisableAeroShake")
    SESSION.set_dword(_ADV, "DisallowShaking", 1)


def _remove_disable_aero_shake(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "DisableAeroShake_Remove")
    SESSION.delete_value(_ADV, "DisallowShaking")


def _detect_disable_aero_shake() -> bool:
    return SESSION.read_dword(_ADV, "DisallowShaking") == 1


# ── Disable Snap Assist Flyout ───────────────────────────────────────────────


def _apply_disable_snap_flyout(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disable Snap Assist flyout")
    SESSION.backup([_ADV], "DisableSnapFlyout")
    SESSION.set_dword(_ADV, "SnapAssist", 0)


def _remove_disable_snap_flyout(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "DisableSnapFlyout_Remove")
    SESSION.set_dword(_ADV, "SnapAssist", 1)


def _detect_disable_snap_flyout() -> bool:
    return SESSION.read_dword(_ADV, "SnapAssist") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="take-ownership",
        label="Take Ownership Context Menu",
        category="Shell",
        apply_fn=apply_take_ownership,
        remove_fn=remove_take_ownership,
        detect_fn=detect_take_ownership,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_KEYS,
        description=(
            "Adds a 'Take Ownership' entry to the right-click context "
            "menu for files, folders, and drives."
        ),
        tags=["shell", "context-menu", "ownership"],
    ),
    TweakDef(
        id="open-cmd-here",
        label="'Open CMD Here' Context Menu",
        category="Shell",
        apply_fn=_apply_cmd_here,
        remove_fn=_remove_cmd_here,
        detect_fn=_detect_cmd_here,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_CMD_KEYS,
        description="Adds 'Open Command Prompt Here' to the folder background context menu.",
        tags=["shell", "context-menu", "cmd"],
    ),
    TweakDef(
        id="file-hash-context",
        label="'Get File Hash' Context Menu",
        category="Shell",
        apply_fn=_apply_hash_context,
        remove_fn=_remove_hash_context,
        detect_fn=_detect_hash_context,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_HASH_KEYS,
        description="Adds 'Get File Hash (SHA256)' to the right-click menu for any file.",
        tags=["shell", "context-menu", "hash", "security"],
    ),
    TweakDef(
        id="open-ps-here",
        label="'Open PowerShell Here' Context Menu",
        category="Shell",
        apply_fn=_apply_ps_here,
        remove_fn=_remove_ps_here,
        detect_fn=_detect_ps_here,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_PS_HERE_KEYS,
        description="Adds 'Open PowerShell Here' to the folder background context menu.",
        tags=["shell", "context-menu", "powershell"],
    ),
    TweakDef(
        id="open-wt-here",
        label="'Open Terminal Here' Context Menu",
        category="Shell",
        apply_fn=_apply_wt_here,
        remove_fn=_remove_wt_here,
        detect_fn=_detect_wt_here,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WT_HERE_KEY],
        description=(
            "Adds 'Open Terminal Here' to the folder background right-click menu. "
            "Requires Windows Terminal to be installed."
        ),
        tags=["shell", "context-menu", "terminal", "wt"],
    ),
    TweakDef(
        id="shell-classic-context-menu",
        label="Restore Classic Context Menu (Win11)",
        category="Shell",
        apply_fn=_apply_classic_context_menu,
        remove_fn=_remove_classic_context_menu,
        detect_fn=_detect_classic_context_menu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CLASSIC_CTX_KEY],
        description=(
            "Restores the full Windows 10-style right-click context menu on "
            "Windows 11 by disabling the modern truncated menu."
        ),
        tags=["shell", "context-menu", "win11", "classic"],
    ),
    TweakDef(
        id="shell-disable-recent-files",
        label="Disable Recent Files in Quick Access",
        category="Shell",
        apply_fn=_apply_disable_recent_files,
        remove_fn=_remove_disable_recent_files,
        detect_fn=_detect_disable_recent_files,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_CU],
        description="Prevents recently opened files from appearing in Quick Access.",
        tags=["shell", "explorer", "privacy", "recent"],
    ),
    TweakDef(
        id="disable-frequent-folders",
        label="Disable Frequent Folders in Quick Access",
        category="Shell",
        apply_fn=_apply_disable_frequent_folders,
        remove_fn=_remove_disable_frequent_folders,
        detect_fn=_detect_disable_frequent_folders,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_CU],
        description="Prevents frequently used folders from appearing in Quick Access.",
        tags=["shell", "explorer", "privacy", "frequent"],
    ),
    TweakDef(
        id="compact-file-explorer",
        label="Enable Compact View in File Explorer",
        category="Shell",
        apply_fn=_apply_compact_file_explorer,
        remove_fn=_remove_compact_file_explorer,
        detect_fn=_detect_compact_file_explorer,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Enables the compact layout in File Explorer, reducing padding between items.",
        tags=["shell", "explorer", "compact", "layout"],
    ),
    TweakDef(
        id="shell-show-file-extensions",
        label="Show File Extensions in Explorer",
        category="Shell",
        apply_fn=_apply_show_file_extensions,
        remove_fn=_remove_show_file_extensions,
        detect_fn=_detect_show_file_extensions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Displays file extensions (e.g. .txt, .exe) in File Explorer.",
        tags=["shell", "explorer", "extensions", "visibility"],
    ),
    TweakDef(
        id="shell-show-hidden-files",
        label="Show Hidden Files in Explorer",
        category="Shell",
        apply_fn=_apply_show_hidden_files,
        remove_fn=_remove_show_hidden_files,
        detect_fn=_detect_show_hidden_files,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Shows hidden files and folders in File Explorer.",
        tags=["shell", "explorer", "hidden", "visibility"],
    ),
    TweakDef(
        id="shell-disable-aero-shake",
        label="Disable Aero Shake",
        category="Shell",
        apply_fn=_apply_disable_aero_shake,
        remove_fn=_remove_disable_aero_shake,
        detect_fn=_detect_disable_aero_shake,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables Aero Shake (shaking a window to minimize others). "
            "Prevents accidental minimization. Default: Enabled. Recommended: Disabled."
        ),
        tags=["shell", "aero", "shake", "ux"],
    ),
    TweakDef(
        id="shell-disable-snap-flyout",
        label="Disable Snap Assist Flyout",
        category="Shell",
        apply_fn=_apply_disable_snap_flyout,
        remove_fn=_remove_disable_snap_flyout,
        detect_fn=_detect_disable_snap_flyout,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables the Snap Assist suggestion flyout when snapping windows. "
            "Windows still snap but without the layout suggestion popup. "
            "Default: Enabled. Recommended: Disabled for power users."
        ),
        tags=["shell", "snap", "flyout", "performance"],
    ),
]


# -- Disable Shake to Minimize -------------------------------------------------


def _apply_shell_disable_shake_minimize(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disabling shake to minimize")
    SESSION.backup([_ADV], "ShakeMinimize")
    SESSION.set_dword(_ADV, "DisallowShaking", 1)
    SESSION.log("Shell: shake to minimize disabled")


def _remove_shell_disable_shake_minimize(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "ShakeMinimize_Remove")
    SESSION.delete_value(_ADV, "DisallowShaking")


def _detect_shell_disable_shake_minimize() -> bool:
    return SESSION.read_dword(_ADV, "DisallowShaking") == 1


# -- Disable Snap Assist -------------------------------------------------------


def _apply_shell_disable_snap_assist(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disabling Snap Assist")
    SESSION.backup([_ADV], "SnapAssistDisable")
    SESSION.set_dword(_ADV, "SnapAssist", 0)
    SESSION.log("Shell: Snap Assist disabled")


def _remove_shell_disable_snap_assist(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "SnapAssistDisable_Remove")
    SESSION.set_dword(_ADV, "SnapAssist", 1)


def _detect_shell_disable_snap_assist() -> bool:
    return SESSION.read_dword(_ADV, "SnapAssist") == 0


TWEAKS += [
    TweakDef(
        id="shell-disable-shake-minimize",
        label="Disable Shake to Minimize",
        category="Shell",
        apply_fn=_apply_shell_disable_shake_minimize,
        remove_fn=_remove_shell_disable_shake_minimize,
        detect_fn=_detect_shell_disable_shake_minimize,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables Aero Shake gesture that minimizes all other windows. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["shell", "shake", "minimize", "ux"],
    ),
    TweakDef(
        id="shell-disable-snap-assist",
        label="Disable Snap Assist Suggestions",
        category="Shell",
        apply_fn=_apply_shell_disable_snap_assist,
        remove_fn=_remove_shell_disable_snap_assist,
        detect_fn=_detect_shell_disable_snap_assist,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables Snap Assist window arrangement suggestions. "
            "Default: Enabled. Recommended: Disabled for power users."
        ),
        tags=["shell", "snap", "assist", "multitasking"],
    ),
]


# -- Disable AutoPlay for All Media --------------------------------------------

_AUTOPLAY_POL = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"
_AUTOPLAY_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"


def _apply_shell_disable_autoplay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disable AutoPlay for all media")
    SESSION.backup([_AUTOPLAY_POL, _AUTOPLAY_LM], "AutoPlayDisable")
    SESSION.set_dword(_AUTOPLAY_POL, "NoDriveTypeAutoRun", 0xFF)
    SESSION.set_dword(_AUTOPLAY_LM, "NoDriveTypeAutoRun", 0xFF)


def _remove_shell_disable_autoplay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_AUTOPLAY_POL, "NoDriveTypeAutoRun")
    SESSION.delete_value(_AUTOPLAY_LM, "NoDriveTypeAutoRun")


def _detect_shell_disable_autoplay() -> bool:
    return SESSION.read_dword(_AUTOPLAY_POL, "NoDriveTypeAutoRun") == 0xFF


# -- Set Command Prompt Auto-Complete -------------------------------------------

_CMD_PROC = r"HKEY_CURRENT_USER\Software\Microsoft\Command Processor"


def _apply_shell_cmd_autocomplete(*, require_admin: bool = False) -> None:
    SESSION.log("Shell: enable command prompt tab auto-complete")
    SESSION.backup([_CMD_PROC], "CmdAutoComplete")
    SESSION.set_dword(_CMD_PROC, "CompletionChar", 0x9)
    SESSION.set_dword(_CMD_PROC, "PathCompletionChar", 0x9)


def _remove_shell_cmd_autocomplete(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CMD_PROC, "CompletionChar")
    SESSION.delete_value(_CMD_PROC, "PathCompletionChar")


def _detect_shell_cmd_autocomplete() -> bool:
    return SESSION.read_dword(_CMD_PROC, "CompletionChar") == 0x9


# -- Disable Windows Ink Workspace ----------------------------------------------

_INK_WS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"


def _apply_shell_disable_ink_workspace(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: disable Windows Ink Workspace")
    SESSION.backup([_INK_WS], "InkWorkspaceDisable")
    SESSION.set_dword(_INK_WS, "AllowWindowsInkWorkspace", 0)


def _remove_shell_disable_ink_workspace(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_INK_WS, "AllowWindowsInkWorkspace")


def _detect_shell_disable_ink_workspace() -> bool:
    return SESSION.read_dword(_INK_WS, "AllowWindowsInkWorkspace") == 0


TWEAKS += [
    TweakDef(
        id="shell-disable-autoplay",
        label="Disable AutoPlay for All Media",
        category="Shell",
        apply_fn=_apply_shell_disable_autoplay,
        remove_fn=_remove_shell_disable_autoplay,
        detect_fn=_detect_shell_disable_autoplay,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_AUTOPLAY_POL, _AUTOPLAY_LM],
        description=(
            "Disables AutoPlay for all drive types (USB, CD, network). "
            "Prevents automatic execution of media content. Default: Enabled. Recommended: Disabled."
        ),
        tags=["shell", "autoplay", "autorun", "security"],
    ),
    TweakDef(
        id="shell-cmd-autocomplete",
        label="Enable Command Prompt Tab Auto-Complete",
        category="Shell",
        apply_fn=_apply_shell_cmd_autocomplete,
        remove_fn=_remove_shell_cmd_autocomplete,
        detect_fn=_detect_shell_cmd_autocomplete,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CMD_PROC],
        description=(
            "Enables Tab key auto-completion for file and path names in cmd.exe. "
            "Sets CompletionChar and PathCompletionChar to Tab (0x9). Default: Disabled. Recommended: Enabled."
        ),
        tags=["shell", "cmd", "autocomplete", "productivity"],
    ),
    TweakDef(
        id="shell-disable-ink-workspace",
        label="Disable Windows Ink Workspace",
        category="Shell",
        apply_fn=_apply_shell_disable_ink_workspace,
        remove_fn=_remove_shell_disable_ink_workspace,
        detect_fn=_detect_shell_disable_ink_workspace,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_INK_WS],
        description=(
            "Disables Windows Ink Workspace (pen/touch drawing overlay). "
            "Frees resources on non-touch devices. Default: Enabled. Recommended: Disabled."
        ),
        tags=["shell", "ink", "workspace", "pen"],
    ),
]
