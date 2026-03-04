"""Windows Update registry tweaks.

Covers: delivery optimization, update deferral, driver updates,
restart policies, and feature update delays.
"""

from __future__ import annotations

from typing import List

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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
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
]
