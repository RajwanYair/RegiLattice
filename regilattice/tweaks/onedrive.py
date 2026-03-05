"""OneDrive registry tweaks.

Covers: auto-start, Files On-Demand, sync throttling, insider updates.
"""

from __future__ import annotations

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


def _apply_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: throttle upload to 1000 KB/s")
    SESSION.backup([_OD], "ODThrottle")
    SESSION.set_dword(_OD, "UploadBandwidthLimit", 1000)  # KB/s


def _remove_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "UploadBandwidthLimit")


def _detect_throttle() -> bool:
    val = SESSION.read_dword(_OD, "UploadBandwidthLimit")
    return val is not None and val > 0


# ── Disable OneDrive Personal Sync ────────────────────────────────────────


def _apply_disable_personal(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable personal (consumer) sync")
    SESSION.backup([_OD], "ODPersonal")
    SESSION.set_dword(_OD, "DisablePersonalSync", 1)


def _remove_disable_personal(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "DisablePersonalSync")


def _detect_disable_personal() -> bool:
    return SESSION.read_dword(_OD, "DisablePersonalSync") == 1


# ── Disable OneDrive Known Folder Move ─────────────────────────────────────


def _apply_disable_kfm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: block Known Folder Move (KFM) prompts")
    SESSION.backup([_OD], "ODKFM")
    SESSION.set_dword(_OD, "KFMBlockOptIn", 1)


def _remove_disable_kfm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "KFMBlockOptIn")


def _detect_disable_kfm() -> bool:
    return SESSION.read_dword(_OD, "KFMBlockOptIn") == 1


# ── Disable OneDrive Personal Account Sign-In ──────────────────────────────


def _apply_disable_personal_signin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable personal account sign-in")
    SESSION.backup([_OD], "ODPersonalSignIn")
    SESSION.set_dword(_OD, "DisablePersonalSync", 1)


def _remove_disable_personal_signin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "DisablePersonalSync")


def _detect_disable_personal_signin() -> bool:
    return SESSION.read_dword(_OD, "DisablePersonalSync") == 1


# ── Limit OneDrive Upload Rate (125 KB/s) ───────────────────────────────────


def _apply_max_upload_rate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: limit upload rate to 125 KB/s")
    SESSION.backup([_OD], "ODMaxUpload")
    SESSION.set_dword(_OD, "UploadBandwidthLimit", 125)


def _remove_max_upload_rate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "UploadBandwidthLimit")


def _detect_max_upload_rate() -> bool:
    return SESSION.read_dword(_OD, "UploadBandwidthLimit") == 125


# ── Limit OneDrive Download Rate (1000 KB/s) ────────────────────────────────


def _apply_max_download_rate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: limit download rate to 1000 KB/s")
    SESSION.backup([_OD], "ODMaxDownload")
    SESSION.set_dword(_OD, "DownloadBandwidthLimit", 1000)


def _remove_max_download_rate(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "DownloadBandwidthLimit")


def _detect_max_download_rate() -> bool:
    return SESSION.read_dword(_OD, "DownloadBandwidthLimit") == 1000


# ── Disable Office Collaboration via OneDrive ────────────────────────────────


def _apply_disable_office_collab(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable Office collaboration")
    SESSION.backup([_OD], "ODOfficeCollab")
    SESSION.set_dword(_OD, "EnableAllOcsiClients", 0)


def _remove_disable_office_collab(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "EnableAllOcsiClients")


def _detect_disable_office_collab() -> bool:
    return SESSION.read_dword(_OD, "EnableAllOcsiClients") == 0


# ── Enable Silent OneDrive Account Configuration ────────────────────────────


def _apply_silent_config(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: enable silent account configuration")
    SESSION.backup([_OD], "ODSilentConfig")
    SESSION.set_dword(_OD, "SilentAccountConfig", 1)


def _remove_silent_config(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "SilentAccountConfig")


def _detect_silent_config() -> bool:
    return SESSION.read_dword(_OD, "SilentAccountConfig") == 1


# ── Disable OneDrive Files On-Demand (Policy) ─────────────────────────────


def _apply_disable_fod_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable Files On-Demand via policy")
    SESSION.backup([_OD], "ODFilesOnDemandPolicy")
    SESSION.set_dword(_OD, "FilesOnDemandEnabled", 0)


def _remove_disable_fod_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_OD, "FilesOnDemandEnabled", 1)


def _detect_disable_fod_policy() -> bool:
    return SESSION.read_dword(_OD, "FilesOnDemandEnabled") == 0


# ── OneDrive Reduce Sync Traffic ──────────────────────────────────────────


def _apply_reduce_bandwidth(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: limit upload bandwidth to 50%")
    SESSION.backup([_OD], "ODReduceBandwidth")
    SESSION.set_dword(_OD, "UploadBandwidthLimit", 50)


def _remove_reduce_bandwidth(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD, "UploadBandwidthLimit")


def _detect_reduce_bandwidth() -> bool:
    return SESSION.read_dword(_OD, "UploadBandwidthLimit") == 50


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
        registry_keys=[_OD],
        description=(
            "Limits OneDrive upload bandwidth to 1000 KB/s to prevent "
            "saturating your connection."
        ),
        tags=["onedrive", "bandwidth", "network"],
    ),
    TweakDef(
        id="disable-onedrive-personal-sync",
        label="Disable OneDrive Personal Sync",
        category="OneDrive",
        apply_fn=_apply_disable_personal,
        remove_fn=_remove_disable_personal,
        detect_fn=_detect_disable_personal,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Blocks OneDrive consumer (personal) account sync via policy.",
        tags=["onedrive", "sync", "policy"],
    ),
    TweakDef(
        id="disable-onedrive-kfm",
        label="Block OneDrive Known Folder Move",
        category="OneDrive",
        apply_fn=_apply_disable_kfm,
        remove_fn=_remove_disable_kfm,
        detect_fn=_detect_disable_kfm,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Prevents OneDrive from prompting to move Desktop, Documents & Pictures.",
        tags=["onedrive", "kfm", "folders"],
    ),
    TweakDef(
        id="onedrive-disable-personal",
        label="Disable OneDrive Personal Account Sign-In",
        category="OneDrive",
        apply_fn=_apply_disable_personal_signin,
        remove_fn=_remove_disable_personal_signin,
        detect_fn=_detect_disable_personal_signin,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Prevents users from signing in with a personal Microsoft account in OneDrive.",
        tags=["onedrive", "personal", "signin", "policy"],
    ),
    TweakDef(
        id="onedrive-max-upload-rate",
        label="Limit OneDrive Upload Rate (125 KB/s)",
        category="OneDrive",
        apply_fn=_apply_max_upload_rate,
        remove_fn=_remove_max_upload_rate,
        detect_fn=_detect_max_upload_rate,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Caps OneDrive upload bandwidth at 125 KB/s to minimise network impact.",
        tags=["onedrive", "bandwidth", "upload", "network"],
    ),
    TweakDef(
        id="onedrive-max-download-rate",
        label="Limit OneDrive Download Rate (1000 KB/s)",
        category="OneDrive",
        apply_fn=_apply_max_download_rate,
        remove_fn=_remove_max_download_rate,
        detect_fn=_detect_max_download_rate,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Caps OneDrive download bandwidth at 1000 KB/s to minimise network impact.",
        tags=["onedrive", "bandwidth", "download", "network"],
    ),
    TweakDef(
        id="onedrive-disable-office-collab",
        label="Disable Office Collaboration via OneDrive",
        category="OneDrive",
        apply_fn=_apply_disable_office_collab,
        remove_fn=_remove_disable_office_collab,
        detect_fn=_detect_disable_office_collab,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Disables the Office co-authoring feature that uses OneDrive sync.",
        tags=["onedrive", "office", "collaboration", "sync"],
    ),
    TweakDef(
        id="onedrive-silent-config",
        label="Enable Silent OneDrive Account Configuration",
        category="OneDrive",
        apply_fn=_apply_silent_config,
        remove_fn=_remove_silent_config,
        detect_fn=_detect_silent_config,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description="Silently configures OneDrive with the user's Windows credentials without prompts.",
        tags=["onedrive", "silent", "config", "policy"],
    ),
    TweakDef(
        id="onedrive-disable-files-on-demand",
        label="Disable OneDrive Files On-Demand",
        category="OneDrive",
        apply_fn=_apply_disable_fod_policy,
        remove_fn=_remove_disable_fod_policy,
        detect_fn=_detect_disable_fod_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description=(
            "Disables OneDrive Files On-Demand (cloud-only placeholders). "
            "All files will be downloaded locally. Default: Enabled. "
            "Recommended: Disabled for offline reliability."
        ),
        tags=["onedrive", "files-on-demand", "offline", "sync"],
    ),
    TweakDef(
        id="onedrive-reduce-bandwidth",
        label="OneDrive Reduce Sync Traffic",
        category="OneDrive",
        apply_fn=_apply_reduce_bandwidth,
        remove_fn=_remove_reduce_bandwidth,
        detect_fn=_detect_reduce_bandwidth,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OD],
        description=(
            "Limits OneDrive upload bandwidth to 50%. Prevents OneDrive "
            "from saturating network connection. Default: Unlimited. "
            "Recommended: 50% for shared networks."
        ),
        tags=["onedrive", "bandwidth", "network", "performance"],
    ),
]


# ── Disable Files On-Demand (global policy) ─────────────────────────────────


def _apply_disable_fod_global(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable Files On-Demand globally")
    SESSION.backup([_OD], "ODFodGlobal")
    SESSION.set_dword(_OD, "FilesOnDemandEnabled", 0)


def _remove_disable_fod_global(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_OD, "FilesOnDemandEnabled", 1)


def _detect_disable_fod_global() -> bool:
    return SESSION.read_dword(_OD, "FilesOnDemandEnabled") == 0


# ── Disable Personal OneDrive Sync ───────────────────────────────────────────

_OD_WINDOWS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OneDrive"


def _apply_disable_personal_sync_ngsc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("OneDrive: disable personal sync via DisableFileSyncNGSC")
    SESSION.backup([_OD_WINDOWS], "ODPersonalSync")
    SESSION.set_dword(_OD_WINDOWS, "DisableFileSyncNGSC", 1)


def _remove_disable_personal_sync_ngsc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OD_WINDOWS, "DisableFileSyncNGSC")


def _detect_disable_personal_sync_ngsc() -> bool:
    return SESSION.read_dword(_OD_WINDOWS, "DisableFileSyncNGSC") == 1


TWEAKS += [
    TweakDef(
        id="onedrive-disable-fod-global",
        label="Disable OneDrive Files On-Demand (Global)",
        category="OneDrive",
        apply_fn=_apply_disable_fod_global,
        remove_fn=_remove_disable_fod_global,
        detect_fn=_detect_disable_fod_global,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_OD],
        description=(
            "Disables OneDrive Files On-Demand globally via policy. "
            "Forces all files to be downloaded locally. "
            "Default: Enabled. Recommended: Disabled for offline use."
        ),
        tags=["onedrive", "files-on-demand", "policy", "offline"],
    ),
    TweakDef(
        id="onedrive-disable-personal-sync",
        label="Disable Personal OneDrive Sync",
        category="OneDrive",
        apply_fn=_apply_disable_personal_sync_ngsc,
        remove_fn=_remove_disable_personal_sync_ngsc,
        detect_fn=_detect_disable_personal_sync_ngsc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_OD_WINDOWS],
        description=(
            "Disables personal OneDrive file sync via DisableFileSyncNGSC policy. "
            "Prevents OneDrive from syncing any personal accounts. "
            "Default: Enabled. Recommended: Disabled on corporate machines."
        ),
        tags=["onedrive", "sync", "personal", "policy"],
    ),
]
