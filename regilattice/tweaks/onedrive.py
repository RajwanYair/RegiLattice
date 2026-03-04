"""OneDrive registry tweaks.

Covers: auto-start, Files On-Demand, sync throttling, insider updates.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_RUN = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"
_OD = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"
_OD_CU = r"HKEY_CURRENT_USER\Software\Microsoft\OneDrive"


# ── Disable OneDrive Auto-Start ─────────────────────────────────────────────


def _apply_disable_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable auto-start")
    SESSION.backup([_RUN], "ODAutoStart")
    SESSION.delete_value(_RUN, "OneDrive")
    SESSION.delete_value(_RUN, "OneDriveSetup")


def _remove_disable_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    # Re-enabling requires the actual OneDrive path — just remove the flag
    SESSION.delete_value(_OD_CU, "DisableAutoStart")


def _detect_disable_autostart() -> bool:
    return SESSION.read_string(_RUN, "OneDrive") is None


# ── Disable OneDrive Files On-Demand ─────────────────────────────────────────


def _apply_disable_fod(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable Files On-Demand")
    SESSION.backup([_OD], "ODFilesOnDemand")
    SESSION.set_dword(_OD, "FilesOnDemandEnabled", 0)


def _remove_disable_fod(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_OD, "FilesOnDemandEnabled", 1)


def _detect_disable_fod() -> bool:
    return SESSION.read_dword(_OD, "FilesOnDemandEnabled") == 0


# ── Disable OneDrive Sync Ads / Notifications ────────────────────────────────


def _apply_disable_ads(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable sync ads and upsell notifications")
    SESSION.backup([_OD], "ODAds")
    SESSION.set_dword(_OD, "PreventNetworkTrafficPreUserSignIn", 1)
    SESSION.set_dword(_OD, "DisablePersonalSync", 0)  # keep syncing, just no ads


def _remove_disable_ads(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "PreventNetworkTrafficPreUserSignIn")


def _detect_disable_ads() -> bool:
    return SESSION.read_dword(_OD, "PreventNetworkTrafficPreUserSignIn") == 1


# ── Limit OneDrive Upload Bandwidth ─────────────────────────────────────────

_OD_THROTTLE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"


def _apply_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: throttle upload to 1000 KB/s")
    SESSION.backup([_OD_THROTTLE], "ODThrottle")
    SESSION.set_dword(_OD_THROTTLE, "UploadBandwidthLimit", 1000)  # KB/s


def _remove_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD_THROTTLE, "UploadBandwidthLimit")


def _detect_throttle() -> bool:
    val = SESSION.read_dword(_OD_THROTTLE, "UploadBandwidthLimit")
    return val is not None and val > 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-onedrive-autostart",
        label="Disable OneDrive Auto-Start",
        category="OneDrive",
        apply_fn=_apply_disable_autostart,
        remove_fn=_remove_disable_autostart,
        detect_fn=_detect_disable_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN],
        description="Prevents OneDrive from starting automatically at login.",
        tags=["onedrive", "autostart", "startup"],
    ),
    TweakDef(
        id="disable-onedrive-fod",
        label="Disable OneDrive Files On-Demand",
        category="OneDrive",
        apply_fn=_apply_disable_fod,
        remove_fn=_remove_disable_fod,
        detect_fn=_detect_disable_fod,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description=(
            "Disables Files On-Demand — all files are downloaded locally "
            "instead of being cloud-only placeholders."
        ),
        tags=["onedrive", "sync", "disk"],
    ),
    TweakDef(
        id="disable-onedrive-ads",
        label="Disable OneDrive Ads / Upsell",
        category="OneDrive",
        apply_fn=_apply_disable_ads,
        remove_fn=_remove_disable_ads,
        detect_fn=_detect_disable_ads,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Hides OneDrive promotional and upsell notifications.",
        tags=["onedrive", "privacy", "ads"],
    ),
    TweakDef(
        id="onedrive-upload-throttle",
        label="Throttle OneDrive Upload (1 MB/s)",
        category="OneDrive",
        apply_fn=_apply_throttle,
        remove_fn=_remove_throttle,
        detect_fn=_detect_throttle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD_THROTTLE],
        description=(
            "Limits OneDrive upload bandwidth to 1000 KB/s to prevent "
            "saturating your connection."
        ),
        tags=["onedrive", "bandwidth", "network"],
    ),
]
