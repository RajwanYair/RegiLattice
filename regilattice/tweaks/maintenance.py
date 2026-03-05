"""Maintenance tweaks — Registry Auto-Backup, Restore Point."""

from __future__ import annotations

import subprocess

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Registry Auto-Backup ────────────────────────────────────────────────────

_REGBACK_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Configuration Manager"
)


def apply_regbackup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-RegistryBackup")
    SESSION.backup([_REGBACK_KEY], "RegAutoBackup")
    SESSION.set_dword(_REGBACK_KEY, "EnablePeriodicBackup", 1)
    SESSION.log("Completed Add-RegistryBackup")


def remove_regbackup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-RegistryBackup")
    SESSION.backup([_REGBACK_KEY], "RegAutoBackup_Remove")
    SESSION.set_dword(_REGBACK_KEY, "EnablePeriodicBackup", 0)
    SESSION.log("Completed Remove-RegistryBackup")


def detect_regbackup() -> bool:
    return SESSION.read_dword(_REGBACK_KEY, "EnablePeriodicBackup") == 1


# ── Disable Scheduled Defragmentation ──────────────────────────────────────

_DEFRAG_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"
)


def _apply_disable_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable scheduled defragmentation")
    SESSION.backup([_DEFRAG_KEY], "Defrag")
    SESSION.set_string(_DEFRAG_KEY, "Enable", "N")


def _remove_disable_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_DEFRAG_KEY, "Enable", "Y")


def _detect_disable_defrag() -> bool:
    return SESSION.read_string(_DEFRAG_KEY, "Enable") == "N"


# ── Disable Windows Error Dumps ───────────────────────────────────────────

_CRASH_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"
)


def _apply_disable_dumps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable crash memory dumps")
    SESSION.backup([_CRASH_KEY], "CrashDumps")
    SESSION.set_dword(_CRASH_KEY, "CrashDumpEnabled", 0)  # 0 = None
    SESSION.set_dword(_CRASH_KEY, "LogEvent", 0)


def _remove_disable_dumps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CRASH_KEY, "CrashDumpEnabled", 7)  # 7 = Automatic
    SESSION.set_dword(_CRASH_KEY, "LogEvent", 1)


def _detect_disable_dumps() -> bool:
    return SESSION.read_dword(_CRASH_KEY, "CrashDumpEnabled") == 0


# ── System Restore Point ────────────────────────────────────────────────────


def create_restore_point(*, require_admin: bool = True) -> None:
    """Create a Windows System Restore checkpoint via PowerShell."""
    assert_admin(require_admin)
    SESSION.log("Creating system restore point")
    cmd = (
        'powershell -NoProfile -Command "'
        "Checkpoint-Computer -Description 'RegiLattice' "
        "-RestorePointType 'MODIFY_SETTINGS'"
        '"'
    )
    subprocess.run(cmd, shell=True, check=False)
    SESSION.log("Restore point created")


# ── Disable Windows Tips / Suggestions ───────────────────────────────────────────────────────

_TIPS_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)


def _apply_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.log("Maintenance: disable Windows tips and suggestions")
    SESSION.backup([_TIPS_KEY], "WindowsTips")
    SESSION.set_dword(_TIPS_KEY, "SoftLandingEnabled", 0)
    SESSION.set_dword(_TIPS_KEY, "SubscribedContent-338389Enabled", 0)


def _remove_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TIPS_KEY, "SoftLandingEnabled", 1)
    SESSION.set_dword(_TIPS_KEY, "SubscribedContent-338389Enabled", 1)


def _detect_disable_tips() -> bool:
    return SESSION.read_dword(_TIPS_KEY, "SoftLandingEnabled") == 0


# ── Disable Reliability Monitor Tracking (Performance) ──────────────────────────────────────

_RELI_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability"


def _apply_disable_reliability_monitor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable reliability monitor tracking")
    SESSION.backup([_RELI_KEY], "ReliabilityMonitor")
    SESSION.set_dword(_RELI_KEY, "TimeStampInterval", 0)


def _remove_disable_reliability_monitor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RELI_KEY, "TimeStampInterval", 1)


def _detect_disable_reliability_monitor() -> bool:
    return SESSION.read_dword(_RELI_KEY, "TimeStampInterval") == 0


# ── Disable Automatic Maintenance Wake-Up ────────────────────────────────────

_MAINT_WAKEUP_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Schedule\Maintenance"
)


def _apply_disable_maintenance_wakeup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable automatic maintenance wake-up")
    SESSION.backup([_MAINT_WAKEUP_KEY], "MaintenanceWakeUp")
    SESSION.set_dword(_MAINT_WAKEUP_KEY, "WakeUp", 0)


def _remove_disable_maintenance_wakeup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MAINT_WAKEUP_KEY, "WakeUp", 1)


def _detect_disable_maintenance_wakeup() -> bool:
    return SESSION.read_dword(_MAINT_WAKEUP_KEY, "WakeUp") == 0


# ── Disable Disk Diagnostics ────────────────────────────────────────────────

_DISK_DIAG_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}"
)


def _apply_disable_disk_diagnostics(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable disk diagnostics")
    SESSION.backup([_DISK_DIAG_KEY], "DiskDiagnostics")
    SESSION.set_dword(_DISK_DIAG_KEY, "ScenarioExecutionEnabled", 0)


def _remove_disable_disk_diagnostics(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISK_DIAG_KEY, "ScenarioExecutionEnabled")


def _detect_disable_disk_diagnostics() -> bool:
    return SESSION.read_dword(_DISK_DIAG_KEY, "ScenarioExecutionEnabled") == 0


# ── Disable Windows Error Reporting ─────────────────────────────────────────

_WER_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\Windows Error Reporting"
)


def _apply_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable Windows Error Reporting")
    SESSION.backup([_WER_KEY], "ErrorReporting")
    SESSION.set_dword(_WER_KEY, "Disabled", 1)


def _remove_disable_error_reporting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WER_KEY, "Disabled", 0)


def _detect_disable_error_reporting() -> bool:
    return SESSION.read_dword(_WER_KEY, "Disabled") == 1


# ── Disable SuperFetch/SysMain Prefetch ─────────────────────────────────────

_PREFETCH_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management\PrefetchParameters"
)


def _apply_disable_superfetch_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable SuperFetch/SysMain prefetch")
    SESSION.backup([_PREFETCH_KEY], "SuperfetchPrefetch")
    SESSION.set_dword(_PREFETCH_KEY, "EnablePrefetcher", 0)
    SESSION.set_dword(_PREFETCH_KEY, "EnableSuperfetch", 0)


def _remove_disable_superfetch_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PREFETCH_KEY, "EnablePrefetcher", 3)
    SESSION.set_dword(_PREFETCH_KEY, "EnableSuperfetch", 3)


def _detect_disable_superfetch_prefetch() -> bool:
    return (
        SESSION.read_dword(_PREFETCH_KEY, "EnablePrefetcher") == 0
        and SESSION.read_dword(_PREFETCH_KEY, "EnableSuperfetch") == 0
    )


# ── Disable Program Compatibility Assistant ─────────────────────────────────

_PCA_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"
)


def _apply_disable_compatibility_assistant(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable Program Compatibility Assistant")
    SESSION.backup([_PCA_KEY], "CompatAssistant")
    SESSION.set_dword(_PCA_KEY, "DisablePCA", 1)


def _remove_disable_compatibility_assistant(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PCA_KEY, "DisablePCA")


def _detect_disable_compatibility_assistant() -> bool:
    return SESSION.read_dword(_PCA_KEY, "DisablePCA") == 1


# ── Disable Storage Sense Auto-Cleanup ──────────────────────────────────────

_STORAGE_SENSE_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\StorageSense\Parameters\StoragePolicy"
)
_STORAGE_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense"
)


def _apply_disable_storage_sense(*, require_admin: bool = False) -> None:
    SESSION.log("Maintenance: disable Storage Sense auto-cleanup")
    SESSION.backup([_STORAGE_SENSE_KEY], "StorageSense")
    SESSION.set_dword(_STORAGE_SENSE_KEY, "01", 0)


def _remove_disable_storage_sense(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_STORAGE_SENSE_KEY, "01", 1)


def _detect_disable_storage_sense() -> bool:
    return SESSION.read_dword(_STORAGE_SENSE_KEY, "01") == 0


# ── Disable Disk Cleanup Notifications ────────────────────────────────────


def _apply_disable_cleanup_nag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable Storage Sense and low disk space nag")
    SESSION.backup([_STORAGE_POLICY], "CleanupNag")
    SESSION.set_dword(_STORAGE_POLICY, "AllowStorageSenseGlobal", 0)


def _remove_disable_cleanup_nag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORAGE_POLICY, "AllowStorageSenseGlobal")


def _detect_disable_cleanup_nag() -> bool:
    return SESSION.read_dword(_STORAGE_POLICY, "AllowStorageSenseGlobal") == 0


# ── Disable Background Defragmentation ────────────────────────────────────


def _apply_disable_bg_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Maintenance: disable background defragmentation")
    SESSION.backup([_DEFRAG_KEY], "BgDefrag")
    SESSION.set_string(_DEFRAG_KEY, "Enable", "N")


def _remove_disable_bg_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_DEFRAG_KEY, "Enable", "Y")


def _detect_disable_bg_defrag() -> bool:
    return SESSION.read_string(_DEFRAG_KEY, "Enable") == "N"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="registry-autobackup",
        label="Enable Registry Auto-Backup",
        category="Maintenance",
        apply_fn=apply_regbackup,
        remove_fn=remove_regbackup,
        detect_fn=detect_regbackup,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_REGBACK_KEY],
        description=(
            "Enables Windows nightly registry hive backup to "
            r"C:\Windows\System32\config\RegBack."
        ),
        tags=["maintenance", "backup", "registry"],
    ),
    TweakDef(
        id="disable-defrag-schedule",
        label="Disable Scheduled Defragmentation",
        category="Maintenance",
        apply_fn=_apply_disable_defrag,
        remove_fn=_remove_disable_defrag,
        detect_fn=_detect_disable_defrag,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DEFRAG_KEY],
        description=(
            "Disables the Windows automatic disk defragmentation schedule. "
            "Recommended for SSD-only systems."
        ),
        tags=["maintenance", "disk", "defrag", "ssd"],
    ),
    TweakDef(
        id="disable-crash-dumps",
        label="Disable Crash Memory Dumps",
        category="Maintenance",
        apply_fn=_apply_disable_dumps,
        remove_fn=_remove_disable_dumps,
        detect_fn=_detect_disable_dumps,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CRASH_KEY],
        description=(
            "Disables crash memory dump files to save disk space "
            "and avoid large MEMORY.DMP writes."
        ),
        tags=["maintenance", "disk", "cleanup", "crash"],
    ),
    TweakDef(
        id="disable-tips-suggestions",
        label="Disable Windows Tips & Suggestions",
        category="Maintenance",
        apply_fn=_apply_disable_tips,
        remove_fn=_remove_disable_tips,
        detect_fn=_detect_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TIPS_KEY],
        description=(
            "Disables Windows tips, suggestions, and soft-landing notifications "
            "that clutter the notification area. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["maintenance", "notifications", "ux"],
    ),
    TweakDef(
        id="disable-reliability-monitor",
        label="Disable Reliability Monitor (Perf)",
        category="Maintenance",
        apply_fn=_apply_disable_reliability_monitor,
        remove_fn=_remove_disable_reliability_monitor,
        detect_fn=_detect_disable_reliability_monitor,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RELI_KEY],
        description=(
            "Disables reliability monitor event tracking. "
            "Saves background I/O and CPU overhead. "
            "Default: Enabled. Recommended: Disabled for performance."
        ),
        tags=["maintenance", "performance", "monitoring"],
    ),
    TweakDef(
        id="disable-maintenance-wakeup",
        label="Disable Automatic Maintenance Wake-Up",
        category="Maintenance",
        apply_fn=_apply_disable_maintenance_wakeup,
        remove_fn=_remove_disable_maintenance_wakeup,
        detect_fn=_detect_disable_maintenance_wakeup,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MAINT_WAKEUP_KEY],
        description=(
            "Prevents Windows from waking the PC to run automatic "
            "maintenance tasks. Default: Enabled. Recommended: Disabled."
        ),
        tags=["maintenance", "power", "wakeup"],
    ),
    TweakDef(
        id="disable-disk-diagnostics",
        label="Disable Disk Diagnostics",
        category="Maintenance",
        apply_fn=_apply_disable_disk_diagnostics,
        remove_fn=_remove_disable_disk_diagnostics,
        detect_fn=_detect_disable_disk_diagnostics,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DISK_DIAG_KEY],
        description=(
            "Disables the Windows Disk Diagnostic scenario via WDI policy. "
            "Reduces background disk analysis overhead."
        ),
        tags=["maintenance", "disk", "diagnostics"],
    ),
    TweakDef(
        id="disable-error-reporting",
        label="Disable Windows Error Reporting",
        category="Maintenance",
        apply_fn=_apply_disable_error_reporting,
        remove_fn=_remove_disable_error_reporting,
        detect_fn=_detect_disable_error_reporting,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WER_KEY],
        description=(
            "Disables Windows Error Reporting (WER). Stops automatic "
            "submission of crash data to Microsoft."
        ),
        tags=["maintenance", "privacy", "error-reporting"],
    ),
    TweakDef(
        id="disable-superfetch-prefetch",
        label="Disable SuperFetch/SysMain Prefetch",
        category="Maintenance",
        apply_fn=_apply_disable_superfetch_prefetch,
        remove_fn=_remove_disable_superfetch_prefetch,
        detect_fn=_detect_disable_superfetch_prefetch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PREFETCH_KEY],
        description=(
            "Disables the SuperFetch (SysMain) and Prefetcher services via "
            "registry. Recommended for SSD-only systems to reduce writes."
        ),
        tags=["maintenance", "performance", "ssd", "prefetch"],
    ),
    TweakDef(
        id="disable-compatibility-assistant",
        label="Disable Program Compatibility Assistant",
        category="Maintenance",
        apply_fn=_apply_disable_compatibility_assistant,
        remove_fn=_remove_disable_compatibility_assistant,
        detect_fn=_detect_disable_compatibility_assistant,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PCA_KEY],
        description=(
            "Disables the Program Compatibility Assistant (PCA) that "
            "checks for compatibility issues after running programs."
        ),
        tags=["maintenance", "compatibility", "pca"],
    ),
    TweakDef(
        id="disable-storage-sense",
        label="Disable Storage Sense Auto-Cleanup",
        category="Maintenance",
        apply_fn=_apply_disable_storage_sense,
        remove_fn=_remove_disable_storage_sense,
        detect_fn=_detect_disable_storage_sense,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STORAGE_SENSE_KEY],
        description=(
            "Disables Storage Sense automatic disk cleanup. Prevents "
            "Windows from automatically deleting temporary files."
        ),
        tags=["maintenance", "disk", "storage-sense", "cleanup"],
    ),
    TweakDef(
        id="maint-disable-cleanup-nag",
        label="Disable Disk Cleanup Notifications",
        category="Maintenance",
        apply_fn=_apply_disable_cleanup_nag,
        remove_fn=_remove_disable_cleanup_nag,
        detect_fn=_detect_disable_cleanup_nag,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_STORAGE_POLICY],
        description=(
            "Disables Storage Sense and low disk space notifications. "
            "Prevents automatic cleanup of temp files and downloads. "
            "Default: Enabled. Recommended: Disabled for manual control."
        ),
        tags=["maintenance", "storage", "cleanup", "notifications"],
    ),
    TweakDef(
        id="maint-disable-defrag",
        label="Disable Background Defragmentation",
        category="Maintenance",
        apply_fn=_apply_disable_bg_defrag,
        remove_fn=_remove_disable_bg_defrag,
        detect_fn=_detect_disable_bg_defrag,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DEFRAG_KEY],
        description=(
            "Disables background disk defragmentation and boot optimization. "
            "Reduces disk I/O on SSDs where defragmentation is unnecessary. "
            "Default: Enabled. Recommended: Disabled for SSDs."
        ),
        tags=["maintenance", "defrag", "performance", "ssd"],
    ),
]
