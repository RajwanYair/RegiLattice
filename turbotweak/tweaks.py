"""Python equivalents for the PowerShell tweak modules.

Each public function mirrors one of the Add-*/Remove-*.ps1 scripts.
All functions:
 - Check admin rights when needed.
 - Back up affected keys before mutating them.
 - Log progress to ``TurboTweak.log``.
"""

from __future__ import annotations

import subprocess
from typing import List

from .registry import SESSION, assert_admin, is_windows

# ── Helpers ──────────────────────────────────────────────────────────────────


def _run_reg(args: List[str]) -> None:
    """Execute ``reg.exe`` with the given arguments."""
    subprocess.run(["reg", *args], check=True, capture_output=True, text=True)


def _add_context_entry(base: str, command: str) -> None:
    """Register a shell context-menu entry via ``reg.exe``."""
    _run_reg(["add", base, "/f"])
    _run_reg(["add", base, "/ve", "/d", "Take Ownership", "/f"])
    _run_reg(["add", base, "/v", "NoWorkingDirectory", "/d", "", "/f"])
    _run_reg(["add", base, "/v", "Extended", "/d", "", "/f"])
    _run_reg(["add", f"{base}\\command", "/f"])
    _run_reg(["add", f"{base}\\command", "/ve", "/d", command, "/f"])


# ── Take Ownership ──────────────────────────────────────────────────────────


_TAKE_OWNERSHIP_KEYS = [
    r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership\command",
    r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership\command",
    r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership\command",
]

_CMD_FILE = 'cmd.exe /k takeown /f "%1" && icacls "%1" /grant *S-1-3-4:F /t /c /l && pause'
_CMD_DIR = 'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1" /grant *S-1-3-4:F /t /c /q && pause'
_CMD_DRIVE = 'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1" /grant *S-1-3-4:F /t /c && pause'


def add_take_ownership(*, require_admin: bool = True) -> None:
    """Add *Take Ownership* right-click context menu for files, dirs, drives."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-TakeOwnership")
    SESSION.backup(_TAKE_OWNERSHIP_KEYS, "TakeOwnership")

    _add_context_entry(r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership", _CMD_FILE)
    _add_context_entry(r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership", _CMD_DIR)
    _add_context_entry(r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership", _CMD_DRIVE)

    SESSION.log("Completed Add-TakeOwnership")


def remove_take_ownership(*, require_admin: bool = True) -> None:
    """Remove the *Take Ownership* context-menu entries."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-TakeOwnership")

    top_keys = [k for k in _TAKE_OWNERSHIP_KEYS if not k.endswith("\\command")]
    SESSION.backup(top_keys, "TakeOwnership_Remove")

    for key in top_keys:
        subprocess.run(["reg", "delete", key, "/f"], check=False, capture_output=True)

    SESSION.log("Completed Remove-TakeOwnership")


# ── Recent Places ───────────────────────────────────────────────────────────

_CLSID = "{22877a6d-37a1-461a-91b0-dbda5aaebc99}"
_RECENT_KEYS = [
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{_CLSID}",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{_CLSID}\ShellFolder",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\Wow6432Node\CLSID\{_CLSID}",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\Wow6432Node\CLSID\{_CLSID}\ShellFolder",
]


def add_recent_places(*, require_admin: bool = False) -> None:
    """Restore the *Recent Places* shell folder in Explorer."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-RecentFolders")
    SESSION.backup(_RECENT_KEYS, "RecentFolders")

    SESSION.set_string(_RECENT_KEYS[0], None, "Recent Places")
    SESSION.set_dword(_RECENT_KEYS[1], "Attributes", 0x30040000)
    SESSION.set_string(_RECENT_KEYS[2], None, "Recent Places")
    SESSION.set_dword(_RECENT_KEYS[3], "Attributes", 0x30040000)

    SESSION.log("Completed Add-RecentFolders")


def remove_recent_places(*, require_admin: bool = False) -> None:
    """Remove the *Recent Places* shell-folder tweak."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-RecentFolders")

    parent_keys = [_RECENT_KEYS[0], _RECENT_KEYS[2]]
    SESSION.backup(parent_keys, "RecentFolders_Remove")

    for key in parent_keys:
        SESSION.delete_tree(key)

    SESSION.log("Completed Remove-RecentFolders")


# ── Verbose Boot ────────────────────────────────────────────────────────────

_VERBOSE_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"


def enable_verbose_boot(*, require_admin: bool = True) -> None:
    """Enable detailed boot/shutdown status messages."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-VerboseBoot")
    SESSION.backup([_VERBOSE_KEY], "VerboseBoot")
    SESSION.set_dword(_VERBOSE_KEY, "verbosestatus", 1)
    SESSION.log("Completed Add-VerboseBoot")


def disable_verbose_boot(*, require_admin: bool = True) -> None:
    """Disable verbose boot messages (restore default)."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-VerboseBoot")
    SESSION.backup([_VERBOSE_KEY], "VerboseBoot_Remove")
    SESSION.delete_value(_VERBOSE_KEY, "verbosestatus")
    SESSION.log("Completed Remove-VerboseBoot")


# ── Performance ─────────────────────────────────────────────────────────────

_PERF_SERIALIZE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"
)
_PERF_MULTIMEDIA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"
)
_PERF_KEYS = [_PERF_SERIALIZE, _PERF_MULTIMEDIA]


def apply_performance_tweaks(*, require_admin: bool = True) -> None:
    """Apply startup-delay, responsiveness, and network-throttle tweaks."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-Performance")
    SESSION.backup(_PERF_KEYS, "Performance")

    SESSION.set_dword(_PERF_SERIALIZE, "StartupDelayInMSec", 0)
    SESSION.set_dword(_PERF_SERIALIZE, "WaitforIdleState", 0)
    SESSION.set_dword(_PERF_MULTIMEDIA, "SystemResponsiveness", 10)
    SESSION.set_dword(_PERF_MULTIMEDIA, "NetworkThrottlingIndex", 0xFFFFFFFF)

    SESSION.log("Completed Add-Performance")


def remove_performance_tweaks(*, require_admin: bool = True) -> None:
    """Remove performance tweaks and restore Windows defaults."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-Performance")
    SESSION.backup(_PERF_KEYS, "Performance_Remove")

    SESSION.delete_value(_PERF_SERIALIZE, "StartupDelayInMSec")
    SESSION.delete_value(_PERF_SERIALIZE, "WaitforIdleState")
    SESSION.set_dword(_PERF_MULTIMEDIA, "SystemResponsiveness", 20)
    SESSION.set_dword(_PERF_MULTIMEDIA, "NetworkThrottlingIndex", 10)

    SESSION.log("Completed Remove-Performance")


# ── Registry Backup Service ─────────────────────────────────────────────────

_REG_BACKUP_KEY = (
    r"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control"
    r"\Session Manager\Configuration Manager"
)


def enable_registry_backup(*, require_admin: bool = True) -> None:
    """Enable the Windows built-in periodic registry backup engine."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-RegistryBackup")
    SESSION.backup([_REG_BACKUP_KEY], "RegistryBackup")
    SESSION.set_dword(_REG_BACKUP_KEY, "EnablePeriodicBackup", 1)
    SESSION.log("Completed Add-RegistryBackup")


def disable_registry_backup(*, require_admin: bool = True) -> None:
    """Disable the Windows periodic registry backup engine."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-RegistryBackup")
    SESSION.backup([_REG_BACKUP_KEY], "RegistryBackup_Remove")
    SESSION.set_dword(_REG_BACKUP_KEY, "EnablePeriodicBackup", 0)
    SESSION.log("Completed Remove-RegistryBackup")


# ── System Restore Point ───────────────────────────────────────────────────


def create_restore_point(description: str = "TurboTweak Pre-Tweaks") -> None:
    """Create a Windows system restore point."""
    if not is_windows():  # pragma: no cover — platform guard
        raise OSError("System restore points are only available on Windows.")

    subprocess.run(
        [
            "powershell",
            "-Command",
            f"Checkpoint-Computer -Description '{description}' -RestorePointType MODIFY_SETTINGS",
        ],
        check=True,
    )
    SESSION.log(f"Created system restore point: {description}")


# ── Batch operations ────────────────────────────────────────────────────────

_APPLY_ORDER = [
    add_take_ownership,
    add_recent_places,
    enable_verbose_boot,
    apply_performance_tweaks,
    enable_registry_backup,
]

_REMOVE_ORDER = [
    remove_take_ownership,
    remove_recent_places,
    disable_verbose_boot,
    remove_performance_tweaks,
    disable_registry_backup,
]


def apply_all(*, require_admin: bool = True) -> None:
    """Apply every available tweak."""
    for fn in _APPLY_ORDER:
        admin_needed = fn != add_recent_places and require_admin
        fn(require_admin=admin_needed)


def remove_all(*, require_admin: bool = True) -> None:
    """Remove every tweak and restore defaults."""
    for fn in _REMOVE_ORDER:
        admin_needed = fn != remove_recent_places and require_admin
        fn(require_admin=admin_needed)

