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

_CMD_FILE = (
    'cmd.exe /k takeown /f "%1" && icacls "%1" /grant *S-1-3-4:F /t /c /l && pause'
)
_CMD_DIR = 'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1" /grant *S-1-3-4:F /t /c /q && pause'
_CMD_DRIVE = (
    'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1" /grant *S-1-3-4:F /t /c && pause'
)


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

_VERBOSE_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"
)


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
_PERF_MULTIMEDIA = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"
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


# ── Telemetry ───────────────────────────────────────────────────────────────

_TELEMETRY_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"
_TELEMETRY_DATA = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"
_TELEMETRY_KEYS = [_TELEMETRY_POLICY, _TELEMETRY_DATA]


def disable_telemetry(*, require_admin: bool = True) -> None:
    """Reduce Windows telemetry to Security level (minimum)."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableTelemetry")
    SESSION.backup(_TELEMETRY_KEYS, "Telemetry")

    SESSION.set_dword(_TELEMETRY_POLICY, "AllowTelemetry", 0)
    SESSION.set_dword(_TELEMETRY_DATA, "AllowTelemetry", 0)
    SESSION.set_dword(_TELEMETRY_POLICY, "DoNotShowFeedbackNotifications", 1)

    SESSION.log("Completed Add-DisableTelemetry")


def enable_telemetry(*, require_admin: bool = True) -> None:
    """Restore Windows telemetry to default (Full) level."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableTelemetry")
    SESSION.backup(_TELEMETRY_KEYS, "Telemetry_Remove")

    SESSION.set_dword(_TELEMETRY_POLICY, "AllowTelemetry", 3)
    SESSION.set_dword(_TELEMETRY_DATA, "AllowTelemetry", 3)
    SESSION.delete_value(_TELEMETRY_POLICY, "DoNotShowFeedbackNotifications")

    SESSION.log("Completed Remove-DisableTelemetry")


# ── Cortana ─────────────────────────────────────────────────────────────────

_CORTANA_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"


def disable_cortana(*, require_admin: bool = True) -> None:
    """Disable Cortana and web search to reduce background resource usage."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableCortana")
    SESSION.backup([_CORTANA_KEY], "Cortana")

    SESSION.set_dword(_CORTANA_KEY, "AllowCortana", 0)
    SESSION.set_dword(_CORTANA_KEY, "AllowSearchToUseLocation", 0)
    SESSION.set_dword(_CORTANA_KEY, "DisableWebSearch", 1)
    SESSION.set_dword(_CORTANA_KEY, "ConnectedSearchUseWeb", 0)

    SESSION.log("Completed Add-DisableCortana")


def enable_cortana(*, require_admin: bool = True) -> None:
    """Re-enable Cortana and web search."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableCortana")
    SESSION.backup([_CORTANA_KEY], "Cortana_Remove")

    SESSION.delete_value(_CORTANA_KEY, "AllowCortana")
    SESSION.delete_value(_CORTANA_KEY, "AllowSearchToUseLocation")
    SESSION.delete_value(_CORTANA_KEY, "DisableWebSearch")
    SESSION.delete_value(_CORTANA_KEY, "ConnectedSearchUseWeb")

    SESSION.log("Completed Remove-DisableCortana")


# ── Mouse Acceleration ──────────────────────────────────────────────────────

_MOUSE_KEY = r"HKEY_CURRENT_USER\Control Panel\Mouse"


def disable_mouse_accel(*, require_admin: bool = False) -> None:
    """Disable mouse acceleration for pixel-perfect 1:1 input."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableMouseAccel")
    SESSION.backup([_MOUSE_KEY], "MouseAccel")

    SESSION.set_string(_MOUSE_KEY, "MouseSpeed", "0")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold1", "0")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold2", "0")

    SESSION.log("Completed Add-DisableMouseAccel")


def enable_mouse_accel(*, require_admin: bool = False) -> None:
    """Re-enable mouse acceleration (Windows default)."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableMouseAccel")
    SESSION.backup([_MOUSE_KEY], "MouseAccel_Remove")

    SESSION.set_string(_MOUSE_KEY, "MouseSpeed", "1")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold1", "6")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold2", "10")

    SESSION.log("Completed Remove-DisableMouseAccel")


# ── Game DVR / Game Bar ─────────────────────────────────────────────────────

_GAMEDVR_STORE = r"HKEY_CURRENT_USER\System\GameConfigStore"
_GAMEDVR_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"
_GAMEDVR_USER = r"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"
_GAMEDVR_KEYS = [_GAMEDVR_STORE, _GAMEDVR_POLICY, _GAMEDVR_USER]


def disable_game_dvr(*, require_admin: bool = True) -> None:
    """Disable Game DVR / Game Bar to reduce gaming overhead."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableGameDVR")
    SESSION.backup(_GAMEDVR_KEYS, "GameDVR")

    SESSION.set_dword(_GAMEDVR_STORE, "GameDVR_Enabled", 0)
    SESSION.set_dword(_GAMEDVR_POLICY, "AllowGameDVR", 0)
    SESSION.set_dword(_GAMEDVR_USER, "AppCaptureEnabled", 0)

    SESSION.log("Completed Add-DisableGameDVR")


def enable_game_dvr(*, require_admin: bool = True) -> None:
    """Re-enable Game DVR / Game Bar."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableGameDVR")
    SESSION.backup(_GAMEDVR_KEYS, "GameDVR_Remove")

    SESSION.set_dword(_GAMEDVR_STORE, "GameDVR_Enabled", 1)
    SESSION.delete_value(_GAMEDVR_POLICY, "AllowGameDVR")
    SESSION.set_dword(_GAMEDVR_USER, "AppCaptureEnabled", 1)

    SESSION.log("Completed Remove-DisableGameDVR")


# ── SvcHost Split Threshold ─────────────────────────────────────────────────

_SVCHOST_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"


def optimize_svchost_split(*, require_admin: bool = True) -> None:
    """Set SvcHost split threshold to match installed RAM."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-SvcHostSplit")
    SESSION.backup([_SVCHOST_KEY], "SvcHostSplit")

    import ctypes

    class MEMORYSTATUSEX(ctypes.Structure):
        _fields_ = [("dwLength", ctypes.c_ulong),
                     ("dwMemoryLoad", ctypes.c_ulong),
                     ("ullTotalPhys", ctypes.c_ulonglong),
                     ("ullAvailPhys", ctypes.c_ulonglong),
                     ("ullTotalPageFile", ctypes.c_ulonglong),
                     ("ullAvailPageFile", ctypes.c_ulonglong),
                     ("ullTotalVirtual", ctypes.c_ulonglong),
                     ("ullAvailVirtual", ctypes.c_ulonglong),
                     ("ullAvailExtendedVirtual", ctypes.c_ulonglong)]

    mem = MEMORYSTATUSEX()
    mem.dwLength = ctypes.sizeof(MEMORYSTATUSEX)
    ctypes.windll.kernel32.GlobalMemoryStatusEx(ctypes.byref(mem))  # type: ignore[attr-defined]
    ram_kb = int(mem.ullTotalPhys / 1024)

    SESSION.set_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB", ram_kb)
    SESSION.log(f"Completed Add-SvcHostSplit: {ram_kb} KB")


def restore_svchost_split(*, require_admin: bool = True) -> None:
    """Restore SvcHost split threshold to Windows default (~380 MB)."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-SvcHostSplit")
    SESSION.backup([_SVCHOST_KEY], "SvcHostSplit_Remove")

    SESSION.set_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB", 380000)
    SESSION.log("Completed Remove-SvcHostSplit")


# ── NTFS Last Access Timestamps ─────────────────────────────────────────────


def disable_last_access(*, require_admin: bool = True) -> None:
    """Disable NTFS last-access timestamp updates for I/O performance."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableLastAccess")

    subprocess.run(
        ["fsutil", "behavior", "set", "disablelastaccess", "1"],
        check=True, capture_output=True, text=True,
    )
    SESSION.log("Completed Add-DisableLastAccess")


def enable_last_access(*, require_admin: bool = True) -> None:
    """Re-enable NTFS last-access timestamp updates (system-managed default)."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableLastAccess")

    subprocess.run(
        ["fsutil", "behavior", "set", "disablelastaccess", "2"],
        check=True, capture_output=True, text=True,
    )
    SESSION.log("Completed Remove-DisableLastAccess")


# ── Win32 Long Paths ────────────────────────────────────────────────────────

_LONGPATH_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"


def enable_long_paths(*, require_admin: bool = True) -> None:
    """Enable Win32 long paths (remove 260-char path limit)."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-LongPaths")
    SESSION.backup([_LONGPATH_KEY], "LongPaths")

    SESSION.set_dword(_LONGPATH_KEY, "LongPathsEnabled", 1)
    SESSION.log("Completed Add-LongPaths")


def disable_long_paths(*, require_admin: bool = True) -> None:
    """Disable Win32 long paths (restore 260-char limit)."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-LongPaths")
    SESSION.backup([_LONGPATH_KEY], "LongPaths_Remove")

    SESSION.set_dword(_LONGPATH_KEY, "LongPathsEnabled", 0)
    SESSION.log("Completed Remove-LongPaths")


# ── Batch operations ────────────────────────────────────────────────────────

_APPLY_ORDER = [
    add_take_ownership,
    add_recent_places,
    enable_verbose_boot,
    apply_performance_tweaks,
    enable_registry_backup,
    disable_telemetry,
    disable_cortana,
    disable_mouse_accel,
    disable_game_dvr,
    optimize_svchost_split,
    disable_last_access,
    enable_long_paths,
]

_REMOVE_ORDER = [
    remove_take_ownership,
    remove_recent_places,
    disable_verbose_boot,
    remove_performance_tweaks,
    disable_registry_backup,
    enable_telemetry,
    enable_cortana,
    enable_mouse_accel,
    enable_game_dvr,
    restore_svchost_split,
    enable_last_access,
    disable_long_paths,
]

# Tweaks that do NOT require admin (HKCU-only)
_USER_TWEAKS = {add_recent_places, remove_recent_places,
                disable_mouse_accel, enable_mouse_accel}


def apply_all(*, require_admin: bool = True) -> None:
    """Apply every available tweak."""
    for fn in _APPLY_ORDER:
        admin_needed = fn not in _USER_TWEAKS and require_admin
        fn(require_admin=admin_needed)


def remove_all(*, require_admin: bool = True) -> None:
    """Remove every tweak and restore defaults."""
    for fn in _REMOVE_ORDER:
        admin_needed = fn not in _USER_TWEAKS and require_admin
        fn(require_admin=admin_needed)
