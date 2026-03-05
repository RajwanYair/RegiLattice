"""Scheduled Tasks tweaks.

Disables or controls telemetry scheduled tasks, customer experience,
application compatibility, disk defrag scheduling, Windows Error Reporting,
and other background maintenance tasks via registry.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_CEIP = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"
)
_APP_COMPAT = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"
)
_DEFRAG = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"
)
_DEFRAG_SCHED = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Defrag"
)
_WER_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\Windows Error Reporting"
)
_WER_DISABLE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\Windows Error Reporting"
)
_MAINT = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Schedule\Maintenance"
)
_DIAG_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\ScheduledDiagnostics"
)
_AUTO_LOGGER = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\WMI\Autologger\AutoLogger-Diagtrack-Listener"
)
_TELEMETRY_SVC = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack"
)
_DMWAPPUSH = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice"
)
_CONSOLID = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\DataCollection"
)
_MAPS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps"
)
_DISK_DIAG = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI"
    r"\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}"
)


# ── Disable Customer Experience Improvement Program ──────────────────────────


def _apply_disable_ceip(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable Customer Experience Improvement Program")
    SESSION.backup([_CEIP], "CEIP")
    SESSION.set_dword(_CEIP, "CEIPEnable", 0)


def _remove_disable_ceip(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CEIP, "CEIPEnable")


def _detect_disable_ceip() -> bool:
    return SESSION.read_dword(_CEIP, "CEIPEnable") == 0


# ── Disable Application Compatibility Engine ─────────────────────────────────


def _apply_disable_appcompat(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable Application Compatibility Assistant")
    SESSION.backup([_APP_COMPAT], "AppCompat")
    SESSION.set_dword(_APP_COMPAT, "DisableEngine", 1)
    SESSION.set_dword(_APP_COMPAT, "AITEnable", 0)
    SESSION.set_dword(_APP_COMPAT, "DisablePCA", 1)


def _remove_disable_appcompat(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_APP_COMPAT, "DisableEngine")
    SESSION.delete_value(_APP_COMPAT, "AITEnable")
    SESSION.delete_value(_APP_COMPAT, "DisablePCA")


def _detect_disable_appcompat() -> bool:
    return SESSION.read_dword(_APP_COMPAT, "DisableEngine") == 1


# ── Disable Scheduled Defrag ─────────────────────────────────────────────────


def _apply_disable_sched_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable scheduled disk defragmentation")
    SESSION.backup([_DEFRAG_SCHED], "SchedDefrag")
    SESSION.set_dword(_DEFRAG_SCHED, "ScheduleEnabled", 0)


def _remove_disable_sched_defrag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DEFRAG_SCHED, "ScheduleEnabled")


def _detect_disable_sched_defrag() -> bool:
    return SESSION.read_dword(_DEFRAG_SCHED, "ScheduleEnabled") == 0


# ── Disable Windows Error Reporting Tasks ────────────────────────────────────


def _apply_disable_wer_tasks(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable Windows Error Reporting")
    SESSION.backup([_WER_POLICY, _WER_DISABLE], "WERTasks")
    SESSION.set_dword(_WER_POLICY, "Disabled", 1)
    SESSION.set_dword(_WER_DISABLE, "Disabled", 1)


def _remove_disable_wer_tasks(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WER_POLICY, "Disabled")
    SESSION.delete_value(_WER_DISABLE, "Disabled")


def _detect_disable_wer_tasks() -> bool:
    return SESSION.read_dword(_WER_POLICY, "Disabled") == 1


# ── Disable Auto Maintenance Wakeup ─────────────────────────────────────────


def _apply_disable_maint_wakeup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable automatic maintenance wakeup timer")
    SESSION.backup([_MAINT], "MaintWakeup")
    SESSION.set_dword(_MAINT, "WakeUp", 0)


def _remove_disable_maint_wakeup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MAINT, "WakeUp", 1)


def _detect_disable_maint_wakeup() -> bool:
    return SESSION.read_dword(_MAINT, "WakeUp") == 0


# ── Disable Scheduled Diagnostics ────────────────────────────────────────────


def _apply_disable_sched_diag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable scheduled diagnostics")
    SESSION.backup([_DIAG_POLICY], "SchedDiag")
    SESSION.set_dword(_DIAG_POLICY, "EnabledExecution", 0)


def _remove_disable_sched_diag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DIAG_POLICY, "EnabledExecution")


def _detect_disable_sched_diag() -> bool:
    return SESSION.read_dword(_DIAG_POLICY, "EnabledExecution") == 0


# ── Disable DiagTrack Autologger ─────────────────────────────────────────────


def _apply_disable_diagtrack_logger(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable DiagTrack autologger")
    SESSION.backup([_AUTO_LOGGER], "DiagTrackLogger")
    SESSION.set_dword(_AUTO_LOGGER, "Start", 0)


def _remove_disable_diagtrack_logger(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AUTO_LOGGER, "Start", 1)


def _detect_disable_diagtrack_logger() -> bool:
    return SESSION.read_dword(_AUTO_LOGGER, "Start") == 0


# ── Disable DiagTrack Service ────────────────────────────────────────────────


def _apply_disable_diagtrack_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable DiagTrack (Connected User Experiences) service")
    SESSION.backup([_TELEMETRY_SVC], "DiagTrackSvc")
    SESSION.set_dword(_TELEMETRY_SVC, "Start", 4)  # 4 = Disabled


def _remove_disable_diagtrack_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TELEMETRY_SVC, "Start", 2)  # 2 = Automatic


def _detect_disable_diagtrack_svc() -> bool:
    return SESSION.read_dword(_TELEMETRY_SVC, "Start") == 4


# ── Disable dmwappushsvc (WAP Push) ─────────────────────────────────────────


def _apply_disable_dmwappush(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable WAP Push Message Routing Service")
    SESSION.backup([_DMWAPPUSH], "DmwAppPush")
    SESSION.set_dword(_DMWAPPUSH, "Start", 4)


def _remove_disable_dmwappush(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DMWAPPUSH, "Start", 3)  # 3 = Manual


def _detect_disable_dmwappush() -> bool:
    return SESSION.read_dword(_DMWAPPUSH, "Start") == 4


# ── Disable Offline Maps Auto-Update Task ────────────────────────────────────


def _apply_disable_maps_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable automatic offline maps download")
    SESSION.backup([_MAPS], "MapsUpdate")
    SESSION.set_dword(_MAPS, "AutoDownloadAndUpdateMapData", 0)
    SESSION.set_dword(_MAPS, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)


def _remove_disable_maps_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MAPS, "AutoDownloadAndUpdateMapData")
    SESSION.delete_value(_MAPS, "AllowUntriggeredNetworkTrafficOnSettingsPage")


def _detect_disable_maps_update() -> bool:
    return SESSION.read_dword(_MAPS, "AutoDownloadAndUpdateMapData") == 0


# ── Disable Disk Diagnostics ────────────────────────────────────────────────


def _apply_disable_disk_diag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Tasks: disable disk diagnostic scheduled task data collection")
    SESSION.backup([_DISK_DIAG], "DiskDiag")
    SESSION.set_dword(_DISK_DIAG, "ScenarioExecutionEnabled", 0)


def _remove_disable_disk_diag(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISK_DIAG, "ScenarioExecutionEnabled")


def _detect_disable_disk_diag() -> bool:
    return SESSION.read_dword(_DISK_DIAG, "ScenarioExecutionEnabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="task-disable-ceip",
        label="Disable Customer Experience Improvement Program",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_ceip,
        remove_fn=_remove_disable_ceip,
        detect_fn=_detect_disable_ceip,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CEIP],
        description=(
            "Disables the Windows CEIP data collection task. "
            "Stops sending usage data to Microsoft. "
            "Default: enabled. Recommended: 0 (disabled)."
        ),
        tags=["tasks", "ceip", "telemetry", "privacy"],
    ),
    TweakDef(
        id="task-disable-appcompat",
        label="Disable Application Compatibility Assistant",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_appcompat,
        remove_fn=_remove_disable_appcompat,
        detect_fn=_detect_disable_appcompat,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_APP_COMPAT],
        description=(
            "Disables the Application Compatibility Engine, AIT agent, "
            "and Program Compatibility Assistant. Saves CPU on older PCs. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["tasks", "appcompat", "performance", "pca"],
    ),
    TweakDef(
        id="task-disable-scheduled-defrag",
        label="Disable Scheduled Defragmentation",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_sched_defrag,
        remove_fn=_remove_disable_sched_defrag,
        detect_fn=_detect_disable_sched_defrag,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DEFRAG_SCHED],
        description=(
            "Disables the scheduled disk defragmentation task. "
            "Recommended for SSD-only systems where defrag is unnecessary. "
            "Default: enabled. Recommended: disabled (SSD)."
        ),
        tags=["tasks", "defrag", "disk", "ssd", "performance"],
    ),
    TweakDef(
        id="task-disable-wer",
        label="Disable Windows Error Reporting Tasks",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_wer_tasks,
        remove_fn=_remove_disable_wer_tasks,
        detect_fn=_detect_disable_wer_tasks,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WER_POLICY, _WER_DISABLE],
        description=(
            "Disables WER crash report collection and upload tasks. "
            "Prevents sending crash data to Microsoft. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["tasks", "wer", "error", "crash", "privacy"],
    ),
    TweakDef(
        id="task-disable-maintenance-wakeup",
        label="Disable Maintenance Wakeup Timer",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_maint_wakeup,
        remove_fn=_remove_disable_maint_wakeup,
        detect_fn=_detect_disable_maint_wakeup,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MAINT],
        description=(
            "Prevents automatic maintenance from waking the PC at night. "
            "Default: 1 (wake up). Recommended: 0 (no wake)."
        ),
        tags=["tasks", "maintenance", "wakeup", "power", "sleep"],
    ),
    TweakDef(
        id="task-disable-scheduled-diagnostics",
        label="Disable Scheduled Diagnostics",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_sched_diag,
        remove_fn=_remove_disable_sched_diag,
        detect_fn=_detect_disable_sched_diag,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DIAG_POLICY],
        description=(
            "Disables the scheduled diagnostic data collection task. "
            "Default: enabled. Recommended: 0 (disabled)."
        ),
        tags=["tasks", "diagnostics", "telemetry", "privacy"],
    ),
    TweakDef(
        id="task-disable-diagtrack-autologger",
        label="Disable DiagTrack Autologger",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_diagtrack_logger,
        remove_fn=_remove_disable_diagtrack_logger,
        detect_fn=_detect_disable_diagtrack_logger,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_AUTO_LOGGER],
        description=(
            "Disables the DiagTrack ETW autologger at boot. "
            "Stops telemetry trace collection before login. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["tasks", "diagtrack", "etw", "telemetry", "boot"],
    ),
    TweakDef(
        id="task-disable-diagtrack-service",
        label="Disable DiagTrack Service",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_diagtrack_svc,
        remove_fn=_remove_disable_diagtrack_svc,
        detect_fn=_detect_disable_diagtrack_svc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TELEMETRY_SVC],
        description=(
            "Disables the Connected User Experiences and Telemetry "
            "(DiagTrack) service entirely. "
            "Default: 2 (automatic). Recommended: 4 (disabled)."
        ),
        tags=["tasks", "diagtrack", "service", "telemetry"],
    ),
    TweakDef(
        id="task-disable-dmwappush",
        label="Disable WAP Push Service (dmwappushsvc)",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_dmwappush,
        remove_fn=_remove_disable_dmwappush,
        detect_fn=_detect_disable_dmwappush,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DMWAPPUSH],
        description=(
            "Disables the WAP Push Message Routing Service used by "
            "telemetry for device management messages. "
            "Default: 3 (manual). Recommended: 4 (disabled)."
        ),
        tags=["tasks", "wappush", "telemetry", "service"],
    ),
    TweakDef(
        id="task-disable-maps-update",
        label="Disable Offline Maps Auto-Update",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_maps_update,
        remove_fn=_remove_disable_maps_update,
        detect_fn=_detect_disable_maps_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MAPS],
        description=(
            "Disables automatic download and update of offline maps data. "
            "Saves bandwidth and storage. "
            "Default: enabled. Recommended: 0 (disabled)."
        ),
        tags=["tasks", "maps", "bandwidth", "storage"],
    ),
    TweakDef(
        id="task-disable-disk-diagnostics",
        label="Disable Disk Diagnostics Data Collection",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_disk_diag,
        remove_fn=_remove_disable_disk_diag,
        detect_fn=_detect_disable_disk_diag,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DISK_DIAG],
        description=(
            "Disables the disk diagnostic data collector scheduled task. "
            "Default: enabled. Recommended: 0 (disabled)."
        ),
        tags=["tasks", "disk", "diagnostics", "telemetry"],
    ),
]


# -- 12. Disable Compatibility Appraiser ─────────────────────────────────────


def _apply_disable_compat_appr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_APP_COMPAT], "CompatAppraiser")
    SESSION.set_dword(_APP_COMPAT, "DisableUAR", 1)


def _remove_disable_compat_appr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_APP_COMPAT, "DisableUAR", 0)


def _detect_disable_compat_appr() -> bool:
    return SESSION.read_dword(_APP_COMPAT, "DisableUAR") == 1


# -- 13. Disable Automatic Maintenance ───────────────────────────────────────

_SCHED_MAINT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"


def _apply_disable_auto_maint(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SCHED_MAINT], "AutoMaintenance")
    SESSION.set_dword(_SCHED_MAINT, "MaintenanceDisabled", 1)


def _remove_disable_auto_maint(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SCHED_MAINT, "MaintenanceDisabled")


def _detect_disable_auto_maint() -> bool:
    return SESSION.read_dword(_SCHED_MAINT, "MaintenanceDisabled") == 1


TWEAKS += [
    TweakDef(
        id="task-disable-compat-appraiser",
        label="Disable Compatibility Appraiser",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_compat_appr,
        remove_fn=_remove_disable_compat_appr,
        detect_fn=_detect_disable_compat_appr,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_APP_COMPAT],
        description=(
            "Disables the Compatibility Appraiser that collects program telemetry. "
            "Reduces CPU and disk usage. Default: Enabled. Recommended: Disabled."
        ),
        tags=["tasks", "compatibility", "appraiser", "telemetry"],
    ),
    TweakDef(
        id="task-disable-maintenance",
        label="Disable Automatic Maintenance",
        category="Scheduled Tasks",
        apply_fn=_apply_disable_auto_maint,
        remove_fn=_remove_disable_auto_maint,
        detect_fn=_detect_disable_auto_maint,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SCHED_MAINT],
        description=(
            "Disables Windows automatic maintenance scheduler. Prevents background maintenance tasks. "
            "Default: Enabled. Recommended: Disabled for full user control."
        ),
        tags=["tasks", "maintenance", "scheduler", "background"],
    ),
]
