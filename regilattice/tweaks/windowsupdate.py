"""Windows Update registry tweaks.

Covers: delivery optimization, update deferral, driver updates,
restart policies, and feature update delays.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_WU = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"
_AU = rf"{_WU}\AU"
_DO = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\DeliveryOptimization"
)
_DRIVER = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\WindowsUpdate"
)
_RESTART = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\WindowsUpdate\AU"
)
_MEDIC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc"
_ORCHESTRATOR = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc"


# ── Disable Delivery Optimization (P2P Updates) ─────────────────────────────


def _apply_disable_do(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable Delivery Optimization (P2P)")
    SESSION.backup([_DO], "DeliveryOpt")
    SESSION.set_dword(_DO, "DODownloadMode", 0)  # 0 = HTTP only, no peering


def _remove_disable_do(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DO, "DODownloadMode")


def _detect_disable_do() -> bool:
    return SESSION.read_dword(_DO, "DODownloadMode") == 0


# ── Defer Quality Updates (30 days) ─────────────────────────────────────────


def _apply_defer_quality(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: defer quality updates 30 days")
    SESSION.backup([_WU], "DeferQuality")
    SESSION.set_dword(_WU, "DeferQualityUpdates", 1)
    SESSION.set_dword(_WU, "DeferQualityUpdatesPeriodInDays", 30)


def _remove_defer_quality(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WU, "DeferQualityUpdates")
    SESSION.delete_value(_WU, "DeferQualityUpdatesPeriodInDays")


def _detect_defer_quality() -> bool:
    return SESSION.read_dword(_WU, "DeferQualityUpdates") == 1


# ── Defer Feature Updates (90 days) ─────────────────────────────────────────


def _apply_defer_feature(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: defer feature updates 90 days")
    SESSION.backup([_WU], "DeferFeature")
    SESSION.set_dword(_WU, "DeferFeatureUpdates", 1)
    SESSION.set_dword(_WU, "DeferFeatureUpdatesPeriodInDays", 90)


def _remove_defer_feature(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WU, "DeferFeatureUpdates")
    SESSION.delete_value(_WU, "DeferFeatureUpdatesPeriodInDays")


def _detect_defer_feature() -> bool:
    return SESSION.read_dword(_WU, "DeferFeatureUpdates") == 1


# ── Disable Driver Updates via Windows Update ────────────────────────────────


def _apply_disable_driver_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: exclude driver updates")
    SESSION.backup([_DRIVER], "DriverUpdate")
    SESSION.set_dword(_DRIVER, "ExcludeWUDriversInQualityUpdate", 1)


def _remove_disable_driver_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DRIVER, "ExcludeWUDriversInQualityUpdate")


def _detect_disable_driver_updates() -> bool:
    return SESSION.read_dword(_DRIVER, "ExcludeWUDriversInQualityUpdate") == 1


# ── Disable Auto-Restart After Updates ───────────────────────────────────────


def _apply_no_auto_restart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable forced auto-restart")
    SESSION.backup([_RESTART], "NoAutoRestart")
    SESSION.set_dword(_RESTART, "NoAutoRebootWithLoggedOnUsers", 1)


def _remove_no_auto_restart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RESTART, "NoAutoRebootWithLoggedOnUsers")


def _detect_no_auto_restart() -> bool:
    return SESSION.read_dword(_RESTART, "NoAutoRebootWithLoggedOnUsers") == 1


# ── Notify-Only Updates (No Auto-Install) ────────────────────────────────────


def _apply_notify_only(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: set to notify before download")
    SESSION.backup([_AU], "NotifyOnly")
    SESSION.set_dword(_AU, "AUOptions", 2)  # 2 = Notify before download
    SESSION.set_dword(_AU, "NoAutoUpdate", 0)


def _remove_notify_only(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AU, "AUOptions", 3)  # 3 = Auto download, notify install
    SESSION.delete_value(_AU, "NoAutoUpdate")


def _detect_notify_only() -> bool:
    return SESSION.read_dword(_AU, "AUOptions") == 2


# ── Disable Windows Update Medic Service ──────────────────────────────────


def _apply_disable_medic(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable WaaS Medic Service")
    SESSION.backup([_MEDIC], "WUSMedic")
    SESSION.set_dword(_MEDIC, "Start", 4)  # Disabled


def _remove_disable_medic(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MEDIC, "Start", 3)  # Manual


def _detect_disable_medic() -> bool:
    return SESSION.read_dword(_MEDIC, "Start") == 4


# ── Disable Update Orchestrator Service ────────────────────────────────────


def _apply_disable_orchestrator(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable Update Orchestrator")
    SESSION.backup([_ORCHESTRATOR], "UsoSvc")
    SESSION.set_dword(_ORCHESTRATOR, "Start", 4)


def _remove_disable_orchestrator(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ORCHESTRATOR, "Start", 2)  # Automatic


def _detect_disable_orchestrator() -> bool:
    return SESSION.read_dword(_ORCHESTRATOR, "Start") == 4


# ── Set Active Hours (8 AM - 11 PM) ───────────────────────────────────────────


def _apply_active_hours(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: set active hours 8 AM - 11 PM")
    SESSION.backup([_AU], "ActiveHours")
    SESSION.set_dword(_AU, "SetActiveHours", 1)
    SESSION.set_dword(_AU, "ActiveHoursStart", 8)
    SESSION.set_dword(_AU, "ActiveHoursEnd", 23)


def _remove_active_hours(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_AU, "SetActiveHours")
    SESSION.delete_value(_AU, "ActiveHoursStart")
    SESSION.delete_value(_AU, "ActiveHoursEnd")


def _detect_active_hours() -> bool:
    return SESSION.read_dword(_AU, "SetActiveHours") == 1


# ── Disable MSRT (Malicious Software Removal Tool) ───────────────────────────

_MSRT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"


def _apply_disable_msrt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable MSRT delivery")
    SESSION.backup([_MSRT], "MSRT")
    SESSION.set_dword(_MSRT, "DontOfferThroughWUAU", 1)


def _remove_disable_msrt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MSRT, "DontOfferThroughWUAU")


def _detect_disable_msrt() -> bool:
    return SESSION.read_dword(_MSRT, "DontOfferThroughWUAU") == 1


# ── Target Specific Release Version ───────────────────────────────────────────


def _apply_target_release(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: pin to Windows 11 24H2")
    SESSION.backup([_WU], "TargetRelease")
    SESSION.set_dword(_WU, "TargetReleaseVersion", 1)
    SESSION.set_string(_WU, "TargetReleaseVersionInfo", "24H2")
    SESSION.set_string(_WU, "ProductVersion", "Windows 11")


def _remove_target_release(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WU, "TargetReleaseVersion")
    SESSION.delete_value(_WU, "TargetReleaseVersionInfo")
    SESSION.delete_value(_WU, "ProductVersion")


def _detect_target_release() -> bool:
    return SESSION.read_dword(_WU, "TargetReleaseVersion") == 1


# ── Disable Windows Store Auto-Update ─────────────────────────────────────────

_STORE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"


def _apply_disable_store_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable Store app auto-updates")
    SESSION.backup([_STORE], "StoreUpdate")
    SESSION.set_dword(_STORE, "AutoDownload", 2)  # 2 = Always off


def _remove_disable_store_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORE, "AutoDownload")


def _detect_disable_store_updates() -> bool:
    return SESSION.read_dword(_STORE, "AutoDownload") == 2


# ── Disable Update Restart Notifications ──────────────────────────────────────


def _apply_disable_update_notify(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: suppress restart notifications")
    SESSION.backup([_AU], "UpdateNotify")
    SESSION.set_dword(_AU, "NoAutoRebootWithLoggedOnUsers", 1)
    SESSION.set_dword(_AU, "SetAutoRestartNotificationDisable", 1)


def _remove_disable_update_notify(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_AU, "SetAutoRestartNotificationDisable")


def _detect_disable_update_notify() -> bool:
    return SESSION.read_dword(_AU, "SetAutoRestartNotificationDisable") == 1


# ── Disable Delivery Optimization Upload ─────────────────────────────────────


def _apply_disable_do_upload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: disable Delivery Optimization upload bandwidth")
    SESSION.backup([_DO], "DOUploadDisable")
    SESSION.set_dword(_DO, "DOMaxUploadBandwidth", 0)


def _remove_disable_do_upload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DO, "DOMaxUploadBandwidth")


def _detect_disable_do_upload() -> bool:
    return SESSION.read_dword(_DO, "DOMaxUploadBandwidth") == 0


# ── Disable Windows Update Auto-Restart ──────────────────────────────────────


def _apply_wu_no_auto_restart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Windows Update: block auto-restart with logged-on users")
    SESSION.backup([_AU], "WUNoAutoRestart")
    SESSION.set_dword(_AU, "NoAutoRebootWithLoggedOnUsers", 1)
    SESSION.set_dword(_AU, "AlwaysAutoRebootAtScheduledTime", 0)


def _remove_wu_no_auto_restart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_AU, "NoAutoRebootWithLoggedOnUsers")
    SESSION.delete_value(_AU, "AlwaysAutoRebootAtScheduledTime")


def _detect_wu_no_auto_restart() -> bool:
    return SESSION.read_dword(_AU, "NoAutoRebootWithLoggedOnUsers") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-delivery-optimization",
        label="Disable Delivery Optimization (P2P)",
        category="Windows Update",
        apply_fn=_apply_disable_do,
        remove_fn=_remove_disable_do,
        detect_fn=_detect_disable_do,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DO],
        description=(
            "Disables peer-to-peer update sharing, forcing updates "
            "to download only from Microsoft servers."
        ),
        tags=["update", "network", "p2p"],
    ),
    TweakDef(
        id="defer-quality-updates",
        label="Defer Quality Updates (30 days)",
        category="Windows Update",
        apply_fn=_apply_defer_quality,
        remove_fn=_remove_defer_quality,
        detect_fn=_detect_defer_quality,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WU],
        description="Defers quality (security/bug-fix) updates by 30 days.",
        tags=["update", "deferral"],
    ),
    TweakDef(
        id="defer-feature-updates",
        label="Defer Feature Updates (90 days)",
        category="Windows Update",
        apply_fn=_apply_defer_feature,
        remove_fn=_remove_defer_feature,
        detect_fn=_detect_defer_feature,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WU],
        description="Defers feature (major version) updates by 90 days.",
        tags=["update", "deferral"],
    ),
    TweakDef(
        id="disable-driver-updates",
        label="Exclude Drivers from Windows Update",
        category="Windows Update",
        apply_fn=_apply_disable_driver_updates,
        remove_fn=_remove_disable_driver_updates,
        detect_fn=_detect_disable_driver_updates,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DRIVER],
        description=(
            "Prevents Windows Update from installing driver updates, "
            "letting you manage drivers manually."
        ),
        tags=["update", "drivers"],
    ),
    TweakDef(
        id="no-auto-restart",
        label="Disable Forced Auto-Restart",
        category="Windows Update",
        apply_fn=_apply_no_auto_restart,
        remove_fn=_remove_no_auto_restart,
        detect_fn=_detect_no_auto_restart,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RESTART],
        description=(
            "Prevents Windows from automatically restarting while "
            "a user is logged in after update installation."
        ),
        tags=["update", "restart"],
    ),
    TweakDef(
        id="update-notify-only",
        label="Notify-Only Updates (No Auto-Install)",
        category="Windows Update",
        apply_fn=_apply_notify_only,
        remove_fn=_remove_notify_only,
        detect_fn=_detect_notify_only,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_AU],
        description=(
            "Sets Windows Update to notify before downloading, "
            "giving you full control over update timing."
        ),
        tags=["update", "control"],
    ),
    TweakDef(
        id="disable-wus-medic",
        label="Disable WaaS Medic Service",
        category="Windows Update",
        apply_fn=_apply_disable_medic,
        remove_fn=_remove_disable_medic,
        detect_fn=_detect_disable_medic,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_MEDIC],
        description="Disables the Windows Update Medic Service that re-enables disabled updates.",
        tags=["update", "service", "medic"],
    ),
    TweakDef(
        id="disable-update-orchestrator",
        label="Disable Update Orchestrator Service",
        category="Windows Update",
        apply_fn=_apply_disable_orchestrator,
        remove_fn=_remove_disable_orchestrator,
        detect_fn=_detect_disable_orchestrator,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ORCHESTRATOR],
        description="Disables the Update Orchestrator Service (UsoSvc) that manages update scans.",
        tags=["update", "service", "orchestrator"],
    ),
    TweakDef(
        id="set-active-hours",
        label="Set Active Hours (8 AM - 11 PM)",
        category="Windows Update",
        apply_fn=_apply_active_hours,
        remove_fn=_remove_active_hours,
        detect_fn=_detect_active_hours,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_AU],
        description="Sets Windows Update active hours to 8 AM - 11 PM to prevent restart during work.",
        tags=["update", "active-hours", "restart"],
    ),
    TweakDef(
        id="disable-msrt",
        label="Disable MSRT Delivery",
        category="Windows Update",
        apply_fn=_apply_disable_msrt,
        remove_fn=_remove_disable_msrt,
        detect_fn=_detect_disable_msrt,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MSRT],
        description="Prevents the Malicious Software Removal Tool from being offered via Windows Update.",
        tags=["update", "msrt", "security"],
    ),
    TweakDef(
        id="target-release-version",
        label="Pin to Windows 11 24H2",
        category="Windows Update",
        apply_fn=_apply_target_release,
        remove_fn=_remove_target_release,
        detect_fn=_detect_target_release,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WU],
        description="Pins the device to Windows 11 24H2 to prevent unwanted feature updates.",
        tags=["update", "feature", "pin", "24H2"],
    ),
    TweakDef(
        id="disable-store-auto-update",
        label="Disable Store App Auto-Updates",
        category="Windows Update",
        apply_fn=_apply_disable_store_updates,
        remove_fn=_remove_disable_store_updates,
        detect_fn=_detect_disable_store_updates,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORE],
        description="Disables automatic app updates from the Microsoft Store.",
        tags=["update", "store", "apps"],
    ),
    TweakDef(
        id="disable-update-notifications",
        label="Suppress Update Restart Notifications",
        category="Windows Update",
        apply_fn=_apply_disable_update_notify,
        remove_fn=_remove_disable_update_notify,
        detect_fn=_detect_disable_update_notify,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_AU],
        description="Suppresses the nagging restart-required notifications from Windows Update.",
        tags=["update", "notifications", "restart"],
    ),
    TweakDef(
        id="wu-disable-do-upload",
        label="Disable Delivery Optimization Upload",
        category="Windows Update",
        apply_fn=_apply_disable_do_upload,
        remove_fn=_remove_disable_do_upload,
        detect_fn=_detect_disable_do_upload,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DO],
        description=(
            "Disables Delivery Optimization peer-to-peer upload. Prevents your PC from "
            "serving update files to other PCs. Sets upload bandwidth to zero. "
            "Default: Unlimited. Recommended: Disabled."
        ),
        tags=["update", "delivery-optimization", "bandwidth", "performance"],
    ),
    TweakDef(
        id="wu-disable-auto-restart",
        label="Disable Windows Update Auto-Restart",
        category="Windows Update",
        apply_fn=_apply_wu_no_auto_restart,
        remove_fn=_remove_wu_no_auto_restart,
        detect_fn=_detect_wu_no_auto_restart,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_AU],
        description=(
            "Prevents Windows Update from automatically restarting the PC while users are "
            "logged on. Stops unexpected reboots during work. Default: Auto-restart. "
            "Recommended: Disabled."
        ),
        tags=["update", "restart", "reboot", "ux"],
    ),
]


# -- 16. Disable Driver Updates via WU ───────────────────────────────────────


def _apply_exclude_driver_wu(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WU], "NoDriverUpdates")
    SESSION.set_dword(_WU, "ExcludeWUDriversInQualityUpdate", 1)


def _remove_exclude_driver_wu(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WU, "ExcludeWUDriversInQualityUpdate", 0)


def _detect_exclude_driver_wu() -> bool:
    return SESSION.read_dword(_WU, "ExcludeWUDriversInQualityUpdate") == 1


# -- 17. Defer Quality Updates by 14 Days ───────────────────────────────────


def _apply_defer_quality_14d(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WU], "DeferQuality14d")
    SESSION.set_dword(_WU, "DeferQualityUpdatesPeriodInDays", 14)


def _remove_defer_quality_14d(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WU, "DeferQualityUpdatesPeriodInDays")


def _detect_defer_quality_14d() -> bool:
    return SESSION.read_dword(_WU, "DeferQualityUpdatesPeriodInDays") == 14


TWEAKS += [
    TweakDef(
        id="wu-disable-driver-updates",
        label="Disable Driver Updates via Windows Update",
        category="Windows Update",
        apply_fn=_apply_exclude_driver_wu,
        remove_fn=_remove_exclude_driver_wu,
        detect_fn=_detect_exclude_driver_wu,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WU],
        description=(
            "Excludes driver updates from Windows Update quality updates. "
            "Default: Included. Recommended: Excluded for driver stability."
        ),
        tags=["update", "driver", "exclude", "stability"],
    ),
    TweakDef(
        id="wu-defer-quality-updates",
        label="Defer Quality Updates by 14 Days",
        category="Windows Update",
        apply_fn=_apply_defer_quality_14d,
        remove_fn=_remove_defer_quality_14d,
        detect_fn=_detect_defer_quality_14d,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WU],
        description=(
            "Defers quality/security updates by 14 days to allow time for issue reports. "
            "Default: 0. Recommended: 14 for stability."
        ),
        tags=["update", "defer", "quality", "delay"],
    ),
]
