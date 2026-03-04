"""Startup program management registry tweaks.

Covers: suppressing common auto-start entries, controlling
startup delay, and managing Run/RunOnce keys.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_RUN_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"
_RUN_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
_STARTUP_APPROVED_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\StartupApproved\Run"
)
_STARTUP_DELAY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Serialize"
)


# ── Disable Startup Delay ───────────────────────────────────────────────────


def _apply_disable_startup_delay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable startup delay for Run entries")
    SESSION.backup([_STARTUP_DELAY], "StartupDelay")
    SESSION.set_dword(_STARTUP_DELAY, "StartupDelayInMSec", 0)


def _remove_disable_startup_delay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STARTUP_DELAY, "StartupDelayInMSec")


def _detect_disable_startup_delay() -> bool:
    return SESSION.read_dword(_STARTUP_DELAY, "StartupDelayInMSec") == 0


# ── Disable Skype Auto-Start ────────────────────────────────────────────────


def _apply_disable_skype(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Skype auto-start")
    SESSION.backup([_RUN_CU], "SkypeAutoStart")
    SESSION.delete_value(_RUN_CU, "Skype")
    SESSION.delete_value(_RUN_CU, "Skype for Desktop")


def _remove_disable_skype(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    # Cannot reliably re-add — Skype path varies; just log
    SESSION.log("Startup: Skype auto-start removal is a no-op (manual action)")


def _detect_disable_skype() -> bool:
    return (
        SESSION.read_string(_RUN_CU, "Skype") is None
        and SESSION.read_string(_RUN_CU, "Skype for Desktop") is None
    )


# ── Disable Edge Auto-Start ─────────────────────────────────────────────────

_EDGE_STARTUP = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"
)


def _apply_disable_edge_autostart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Edge background & auto-start")
    SESSION.backup([_EDGE_STARTUP], "EdgeAutoStart")
    SESSION.set_dword(_EDGE_STARTUP, "StartupBoostEnabled", 0)
    SESSION.set_dword(_EDGE_STARTUP, "BackgroundModeEnabled", 0)


def _remove_disable_edge_autostart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_STARTUP, "StartupBoostEnabled")
    SESSION.delete_value(_EDGE_STARTUP, "BackgroundModeEnabled")


def _detect_disable_edge_autostart() -> bool:
    return SESSION.read_dword(_EDGE_STARTUP, "StartupBoostEnabled") == 0


# ── Disable Microsoft Store Auto-Install ─────────────────────────────────────

_CONTENT_DELIVERY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)


def _apply_disable_store_autoinstall(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Store auto-install of suggested apps")
    SESSION.backup([_CONTENT_DELIVERY], "StoreAutoInstall")
    SESSION.set_dword(_CONTENT_DELIVERY, "SilentInstalledAppsEnabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "ContentDeliveryAllowed", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "OemPreInstalledAppsEnabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "PreInstalledAppsEnabled", 0)


def _remove_disable_store_autoinstall(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CONTENT_DELIVERY, "SilentInstalledAppsEnabled", 1)
    SESSION.delete_value(_CONTENT_DELIVERY, "ContentDeliveryAllowed")
    SESSION.delete_value(_CONTENT_DELIVERY, "OemPreInstalledAppsEnabled")
    SESSION.delete_value(_CONTENT_DELIVERY, "PreInstalledAppsEnabled")


def _detect_disable_store_autoinstall() -> bool:
    return SESSION.read_dword(_CONTENT_DELIVERY, "SilentInstalledAppsEnabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-startup-delay",
        label="Disable Startup Delay",
        category="Startup",
        apply_fn=_apply_disable_startup_delay,
        remove_fn=_remove_disable_startup_delay,
        detect_fn=_detect_disable_startup_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STARTUP_DELAY],
        description=(
            "Removes the artificial startup delay for Run-key programs, "
            "allowing them to launch immediately at login."
        ),
        tags=["startup", "performance", "boot"],
    ),
    TweakDef(
        id="disable-skype-autostart",
        label="Disable Skype Auto-Start",
        category="Startup",
        apply_fn=_apply_disable_skype,
        remove_fn=_remove_disable_skype,
        detect_fn=_detect_disable_skype,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Removes Skype from the HKCU Run key to prevent auto-start.",
        tags=["startup", "skype"],
    ),
    TweakDef(
        id="disable-edge-autostart",
        label="Disable Edge Startup Boost & Background",
        category="Startup",
        apply_fn=_apply_disable_edge_autostart,
        remove_fn=_remove_disable_edge_autostart,
        detect_fn=_detect_disable_edge_autostart,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_STARTUP],
        description=(
            "Disables Edge's Startup Boost pre-launch and background "
            "mode to free memory and reduce startup load."
        ),
        tags=["startup", "edge", "performance"],
    ),
    TweakDef(
        id="disable-store-autoinstall",
        label="Disable Store Suggested App Install",
        category="Startup",
        apply_fn=_apply_disable_store_autoinstall,
        remove_fn=_remove_disable_store_autoinstall,
        detect_fn=_detect_disable_store_autoinstall,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=(
            "Prevents Windows from silently installing suggested apps "
            "and OEM bloatware from the Microsoft Store."
        ),
        tags=["startup", "bloatware", "store"],
    ),
]
