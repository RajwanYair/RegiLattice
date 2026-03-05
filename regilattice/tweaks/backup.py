"""Backup & Recovery tweaks — File History, shadow copies, system restore.

Covers: File History toggles, Volume Shadow Copy settings, System Restore
configuration, backup scheduling, and recovery environment options.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_FILE_HISTORY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory"
_SYSTEM_RESTORE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT"
    r"\SystemRestore"
)
_BACKUP = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup"
_BACKUP_CLIENT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"
_VSS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS"
_PREV_VERSIONS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Explorer"
)
_CRASH_CONTROL = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet"
    r"\Control\CrashControl"
)
_SHUTDOWN = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
_RELIABILITY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Reliability"
)
_WER = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft"
    r"\Windows\Windows Error Reporting"
)


# ── Disable File History ─────────────────────────────────────────────────────


def _apply_disable_file_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable File History")
    SESSION.backup([_FILE_HISTORY], "FileHistory")
    SESSION.set_dword(_FILE_HISTORY, "Disabled", 1)


def _remove_disable_file_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FILE_HISTORY, "Disabled")


def _detect_disable_file_history() -> bool:
    return SESSION.read_dword(_FILE_HISTORY, "Disabled") == 1


# ── Disable System Restore ──────────────────────────────────────────────────


def _apply_disable_system_restore(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable System Restore via policy")
    SESSION.backup([_SYSTEM_RESTORE], "SystemRestore")
    SESSION.set_dword(_SYSTEM_RESTORE, "DisableSR", 1)
    SESSION.set_dword(_SYSTEM_RESTORE, "DisableConfig", 1)


def _remove_disable_system_restore(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SYSTEM_RESTORE, "DisableSR")
    SESSION.delete_value(_SYSTEM_RESTORE, "DisableConfig")


def _detect_disable_system_restore() -> bool:
    return SESSION.read_dword(_SYSTEM_RESTORE, "DisableSR") == 1


# ── Set Volume Shadow Copy to Manual Start ───────────────────────────────────


def _apply_vss_manual(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: set Volume Shadow Copy (VSS) to manual start")
    SESSION.backup([_VSS], "VSS")
    SESSION.set_dword(_VSS, "Start", 3)  # 3 = Manual


def _remove_vss_manual(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VSS, "Start", 2)  # 2 = Automatic


def _detect_vss_manual() -> bool:
    return SESSION.read_dword(_VSS, "Start") == 3


# ── Disable Windows Backup UI ───────────────────────────────────────────────


def _apply_disable_backup_ui(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable Windows Backup Settings page")
    SESSION.backup([_BACKUP], "BackupUI")
    SESSION.set_dword(_BACKUP, "DisableBackupUI", 1)


def _remove_disable_backup_ui(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BACKUP, "DisableBackupUI")


def _detect_disable_backup_ui() -> bool:
    return SESSION.read_dword(_BACKUP, "DisableBackupUI") == 1


# ── Disable Windows Backup Notifications ─────────────────────────────────────


def _apply_disable_backup_notifications(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable Windows Backup reminder notifications")
    SESSION.backup([_BACKUP_CLIENT], "BackupNotifications")
    SESSION.set_dword(_BACKUP_CLIENT, "DisableBackupToDisk", 1)
    SESSION.set_dword(_BACKUP_CLIENT, "DisableBackupToNetwork", 1)
    SESSION.set_dword(_BACKUP_CLIENT, "DisableBackupToOptical", 1)
    SESSION.set_dword(_BACKUP_CLIENT, "DisableBackupLauncher", 1)


def _remove_disable_backup_notifications(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BACKUP_CLIENT, "DisableBackupToDisk")
    SESSION.delete_value(_BACKUP_CLIENT, "DisableBackupToNetwork")
    SESSION.delete_value(_BACKUP_CLIENT, "DisableBackupToOptical")
    SESSION.delete_value(_BACKUP_CLIENT, "DisableBackupLauncher")


def _detect_disable_backup_notifications() -> bool:
    return SESSION.read_dword(_BACKUP_CLIENT, "DisableBackupLauncher") == 1


# ── Disable Previous Versions Tab ───────────────────────────────────────────


def _apply_disable_previous_versions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable Previous Versions tab in file properties")
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"
    SESSION.backup([_key], "PreviousVersions")
    SESSION.set_dword(_key, "NoPreviousVersionsPage", 1)


def _remove_disable_previous_versions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"
    SESSION.delete_value(_key, "NoPreviousVersionsPage")


def _detect_disable_previous_versions() -> bool:
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"
    return SESSION.read_dword(_key, "NoPreviousVersionsPage") == 1


# ── Disable Automatic Repair on Boot ─────────────────────────────────────────


def _apply_disable_auto_repair(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable automatic reboot on crash")
    SESSION.backup([_CRASH_CONTROL], "AutoReboot")
    SESSION.set_dword(_CRASH_CONTROL, "AutoReboot", 0)


def _remove_disable_auto_repair(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH_CONTROL, "AutoReboot", 1)


def _detect_disable_auto_repair() -> bool:
    return SESSION.read_dword(_CRASH_CONTROL, "AutoReboot") == 0


# ── Set Recovery Console Timeout ─────────────────────────────────────────────


def _apply_recovery_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: set WaitToKillServiceTimeout to 2000 ms")
    SESSION.backup([_SHUTDOWN], "WaitToKillServiceTimeout")
    SESSION.set_string(_SHUTDOWN, "WaitToKillServiceTimeout", "2000")


def _remove_recovery_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_SHUTDOWN, "WaitToKillServiceTimeout", "5000")


def _detect_recovery_timeout() -> bool:
    return SESSION.read_string(_SHUTDOWN, "WaitToKillServiceTimeout") == "2000"


# ── Disable Crash Dump (Performance) ─────────────────────────────────────────


def _apply_disable_crash_dump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable crash dump")
    SESSION.backup([_CRASH_CONTROL], "CrashDumpEnabled")
    SESSION.set_dword(_CRASH_CONTROL, "CrashDumpEnabled", 0)


def _remove_disable_crash_dump(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH_CONTROL, "CrashDumpEnabled", 7)  # Automatic


def _detect_disable_crash_dump() -> bool:
    return SESSION.read_dword(_CRASH_CONTROL, "CrashDumpEnabled") == 0


# ── Disable Windows Error Reporting ──────────────────────────────────────────


def _apply_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable Windows Error Reporting")
    SESSION.backup([_WER], "WindowsErrorReporting")
    SESSION.set_dword(_WER, "Disabled", 1)


def _remove_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER, "Disabled")


def _detect_disable_error_reporting() -> bool:
    return SESSION.read_dword(_WER, "Disabled") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="backup-disable-file-history",
        label="Disable File History",
        category="Backup & Recovery",
        apply_fn=_apply_disable_file_history,
        remove_fn=_remove_disable_file_history,
        detect_fn=_detect_disable_file_history,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FILE_HISTORY],
        description=(
            "Disables Windows File History backup feature via Group Policy. "
            "Useful when you use a third-party backup solution instead."
        ),
        tags=["backup", "file-history", "policy"],
    ),
    TweakDef(
        id="backup-disable-system-restore",
        label="Disable System Restore",
        category="Backup & Recovery",
        apply_fn=_apply_disable_system_restore,
        remove_fn=_remove_disable_system_restore,
        detect_fn=_detect_disable_system_restore,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SYSTEM_RESTORE],
        description=(
            "Disables System Restore and removes existing restore points. "
            "Frees disk space but removes the safety net. Use with caution."
        ),
        tags=["backup", "system-restore", "disk", "policy"],
    ),
    TweakDef(
        id="backup-vss-manual",
        label="Set Volume Shadow Copy to Manual",
        category="Backup & Recovery",
        apply_fn=_apply_vss_manual,
        remove_fn=_remove_vss_manual,
        detect_fn=_detect_vss_manual,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VSS],
        description=(
            "Sets the VSS service to Manual start. Reduces background I/O "
            "if you don't use System Restore or Previous Versions."
        ),
        tags=["backup", "vss", "shadow-copy", "performance"],
    ),
    TweakDef(
        id="backup-disable-backup-ui",
        label="Disable Windows Backup Settings Page",
        category="Backup & Recovery",
        apply_fn=_apply_disable_backup_ui,
        remove_fn=_remove_disable_backup_ui,
        detect_fn=_detect_disable_backup_ui,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BACKUP],
        description=(
            "Hides the Windows Backup page in Settings via policy. "
            "Useful for managed environments that use different backup solutions."
        ),
        tags=["backup", "settings", "policy", "enterprise"],
    ),
    TweakDef(
        id="backup-disable-notifications",
        label="Disable Backup Reminder Notifications",
        category="Backup & Recovery",
        apply_fn=_apply_disable_backup_notifications,
        remove_fn=_remove_disable_backup_notifications,
        detect_fn=_detect_disable_backup_notifications,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BACKUP_CLIENT],
        description=(
            "Disables all Windows Backup reminder notifications. "
            "Stops nagging popups about configuring backup to disk, network, or optical."
        ),
        tags=["backup", "notifications", "nag", "policy"],
    ),
    TweakDef(
        id="backup-disable-previous-versions",
        label="Disable Previous Versions Tab",
        category="Backup & Recovery",
        apply_fn=_apply_disable_previous_versions,
        remove_fn=_remove_disable_previous_versions,
        detect_fn=_detect_disable_previous_versions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PREV_VERSIONS],
        description=(
            "Removes the 'Previous Versions' tab from file/folder properties. "
            "Cleans up the context menu when VSS is not in use."
        ),
        tags=["backup", "previous-versions", "explorer", "cleanup"],
    ),
    TweakDef(
        id="backup-disable-auto-repair",
        label="Disable Automatic Repair on Boot",
        category="Backup & Recovery",
        apply_fn=_apply_disable_auto_repair,
        remove_fn=_remove_disable_auto_repair,
        detect_fn=_detect_disable_auto_repair,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH_CONTROL],
        description=(
            "Prevents automatic reboot loops on crash by disabling "
            "the AutoReboot flag in CrashControl. Improves stability "
            "diagnostics by keeping the BSOD screen visible."
        ),
        tags=["backup", "recovery", "crash", "reboot", "performance"],
    ),
    TweakDef(
        id="backup-recovery-timeout",
        label="Set Recovery Console Timeout",
        category="Backup & Recovery",
        apply_fn=_apply_recovery_timeout,
        remove_fn=_remove_recovery_timeout,
        detect_fn=_detect_recovery_timeout,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SHUTDOWN],
        description=(
            "Sets WaitToKillServiceTimeout to 2000 ms (from default "
            "5000 ms). Speeds up shutdown by reducing the time Windows "
            "waits for services to stop gracefully."
        ),
        tags=["backup", "shutdown", "timeout", "performance"],
    ),
    TweakDef(
        id="backup-disable-crash-dump",
        label="Disable Crash Dump (Performance)",
        category="Backup & Recovery",
        apply_fn=_apply_disable_crash_dump,
        remove_fn=_remove_disable_crash_dump,
        detect_fn=_detect_disable_crash_dump,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH_CONTROL],
        description=(
            "Disables crash memory dumps (CrashDumpEnabled=0). Frees "
            "disk space and reduces overhead during crashes. Revert "
            "restores Automatic mode (7). Options: 0=None, 1=Complete, "
            "2=Kernel, 3=Small, 7=Automatic."
        ),
        tags=["backup", "crash-dump", "disk", "performance"],
    ),
    TweakDef(
        id="backup-disable-error-reporting",
        label="Disable Windows Error Reporting",
        category="Backup & Recovery",
        apply_fn=_apply_disable_error_reporting,
        remove_fn=_remove_disable_error_reporting,
        detect_fn=_detect_disable_error_reporting,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WER],
        description=(
            "Stops Windows Error Reporting from collecting and sending "
            "crash data. Reduces disk I/O and network usage from WER "
            "telemetry uploads."
        ),
        tags=["backup", "wer", "error-reporting", "privacy", "performance"],
    ),
]


# ── Disable Reliability Monitoring ───────────────────────────────────────────


def _apply_disable_reliability_monitor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable reliability monitoring")
    SESSION.backup([_RELIABILITY], "ReliabilityMonitor")
    SESSION.set_dword(_RELIABILITY, "TimeStampInterval", 0)


def _remove_disable_reliability_monitor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RELIABILITY, "TimeStampInterval")


def _detect_disable_reliability_monitor() -> bool:
    return SESSION.read_dword(_RELIABILITY, "TimeStampInterval") == 0


# ── Disable Auto-Reboot After Crash ─────────────────────────────────────────


def _apply_disable_auto_reboot_crash(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable auto-reboot after BSOD")
    SESSION.backup([_CRASH_CONTROL], "AutoRebootCrash")
    SESSION.set_dword(_CRASH_CONTROL, "AutoReboot", 0)


def _remove_disable_auto_reboot_crash(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH_CONTROL, "AutoReboot", 1)


def _detect_disable_auto_reboot_crash() -> bool:
    return SESSION.read_dword(_CRASH_CONTROL, "AutoReboot") == 0


TWEAKS += [
    TweakDef(
        id="backup-disable-reliability-monitor",
        label="Disable Reliability Monitoring",
        category="Backup & Recovery",
        apply_fn=_apply_disable_reliability_monitor,
        remove_fn=_remove_disable_reliability_monitor,
        detect_fn=_detect_disable_reliability_monitor,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RELIABILITY],
        description=(
            "Disables Windows Reliability Monitor data collection by "
            "setting TimeStampInterval to 0. Reduces background I/O."
        ),
        tags=["backup", "reliability", "performance"],
    ),
    TweakDef(
        id="backup-disable-auto-reboot-crash",
        label="Disable Auto-Reboot After BSOD",
        category="Backup & Recovery",
        apply_fn=_apply_disable_auto_reboot_crash,
        remove_fn=_remove_disable_auto_reboot_crash,
        detect_fn=_detect_disable_auto_reboot_crash,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CRASH_CONTROL],
        description=(
            "Prevents Windows from automatically rebooting after a blue "
            "screen crash, allowing you to read the error code."
        ),
        tags=["backup", "bsod", "crash", "reboot"],
    ),
]


# -- Increase Shadow Copy Storage Limit ----------------------------------------

_VSS_SETTINGS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\VSS\Settings"
)


def _apply_increase_shadow_storage(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: increasing shadow copy max storage to 64 copies")
    SESSION.backup([_VSS_SETTINGS], "ShadowCopyStorage")
    SESSION.set_dword(_VSS_SETTINGS, "MaxShadowCopies", 64)


def _remove_increase_shadow_storage(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSS_SETTINGS, "MaxShadowCopies")


def _detect_increase_shadow_storage() -> bool:
    return SESSION.read_dword(_VSS_SETTINGS, "MaxShadowCopies") == 64


# -- Disable System Restore on Low Disk ----------------------------------------


def _apply_disable_restore_low_disk(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: disable System Restore on low disk space")
    SESSION.backup([_SYSTEM_RESTORE], "RestoreLowDisk")
    SESSION.set_dword(_SYSTEM_RESTORE, "DisableSROnLowDisk", 1)


def _remove_disable_restore_low_disk(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SYSTEM_RESTORE, "DisableSROnLowDisk")


def _detect_disable_restore_low_disk() -> bool:
    return SESSION.read_dword(_SYSTEM_RESTORE, "DisableSROnLowDisk") == 1


# -- Set Backup Schedule Interval ----------------------------------------------


def _apply_set_backup_interval(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Backup: setting backup schedule interval to 24 hours")
    SESSION.backup([_BACKUP_CLIENT], "BackupInterval")
    SESSION.set_dword(_BACKUP_CLIENT, "BackupFrequency", 24)


def _remove_set_backup_interval(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BACKUP_CLIENT, "BackupFrequency")


def _detect_set_backup_interval() -> bool:
    return SESSION.read_dword(_BACKUP_CLIENT, "BackupFrequency") == 24


TWEAKS += [
    TweakDef(
        id="bak-increase-shadow-storage",
        label="Increase Shadow Copy Storage Limit",
        category="Backup & Recovery",
        apply_fn=_apply_increase_shadow_storage,
        remove_fn=_remove_increase_shadow_storage,
        detect_fn=_detect_increase_shadow_storage,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_VSS_SETTINGS],
        description=(
            "Increases the maximum number of shadow copies to 64. "
            "Allows more restore points to be retained. "
            "Default: 16. Recommended: 64."
        ),
        tags=["backup", "shadow-copy", "vss", "storage"],
    ),
    TweakDef(
        id="bak-disable-restore-low-disk",
        label="Disable System Restore on Low Disk",
        category="Backup & Recovery",
        apply_fn=_apply_disable_restore_low_disk,
        remove_fn=_remove_disable_restore_low_disk,
        detect_fn=_detect_disable_restore_low_disk,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SYSTEM_RESTORE],
        description=(
            "Prevents System Restore from being automatically disabled "
            "when disk space is low. Keeps restore points available. "
            "Default: auto-disable. Recommended: disabled."
        ),
        tags=["backup", "system-restore", "disk", "low-space"],
    ),
    TweakDef(
        id="bak-set-backup-interval",
        label="Set Backup Schedule Interval to 24 Hours",
        category="Backup & Recovery",
        apply_fn=_apply_set_backup_interval,
        remove_fn=_remove_set_backup_interval,
        detect_fn=_detect_set_backup_interval,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BACKUP_CLIENT],
        description=(
            "Sets the Windows Backup schedule interval to 24 hours. "
            "Ensures regular daily backups without excessive frequency. "
            "Default: not set. Recommended: 24 hours."
        ),
        tags=["backup", "schedule", "interval", "frequency"],
    ),
]
