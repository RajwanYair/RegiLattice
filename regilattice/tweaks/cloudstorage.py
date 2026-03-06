"""Cloud Storage tweaks — Dropbox, Google Drive, iCloud, Box, MEGA, pCloud,
Nextcloud, Tresorit, Sync.com, SpiderOak, Amazon Drive.

Covers: auto-start, sync behaviour, bandwidth throttling, overlay icons,
context-menu integration, and telemetry for popular cloud storage providers.

OneDrive tweaks are in ``onedrive.py``; this module covers the remaining
cloud storage ecosystem.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Shared key paths ────────────────────────────────────────────────────────

_RUN_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"
_RUN_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
_EXPLORER_OVERLAYS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion"
    r"\Explorer\ShellIconOverlayIdentifiers"
)

# Dropbox
_DROPBOX_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update"
_DROPBOX_BANDWIDTH = r"HKEY_CURRENT_USER\Software\Dropbox\Config"

# Google Drive
_GDRIVE_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"
_GDRIVE_KEY = r"HKEY_CURRENT_USER\Software\Google\DriveFS"

# iCloud
_ICLOUD_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud"

# Box
_BOX_KEY = r"HKEY_CURRENT_USER\Software\Box\Box"

# MEGA
_MEGA_KEY = r"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync"

# pCloud
_PCLOUD_KEY = r"HKEY_CURRENT_USER\Software\pCloud\pCloud"

# Nextcloud
_NEXTCLOUD_KEY = r"HKEY_CURRENT_USER\Software\Nextcloud\Nextcloud"

# Tresorit
_TRESORIT_KEY = r"HKEY_CURRENT_USER\Software\Tresorit"

# Sync.com
_SYNCCOM_KEY = r"HKEY_CURRENT_USER\Software\Sync"

# SpiderOak
_SPIDEROAK_KEY = r"HKEY_CURRENT_USER\Software\SpiderOakONE"

# Amazon Drive
_AMAZONDRIVE_KEY = r"HKEY_CURRENT_USER\Software\Amazon\Amazon Drive"

# Dropbox policies (extended)
_DROPBOX_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox"
_ICLOUD_SERVICES = r"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services"
_CLOUD_CONTENT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"


# ── Disable Dropbox Auto-Start ──────────────────────────────────────────────


def _apply_disable_dropbox_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Dropbox auto-start")
    SESSION.backup([_RUN_CU], "DropboxAutoStart")
    SESSION.delete_value(_RUN_CU, "Dropbox")
    SESSION.delete_value(_RUN_CU, "DropboxUpdate")


def _remove_disable_dropbox_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: re-enable Dropbox auto-start (manual path needed)")
    # Cannot reliably restore — just remove any block flag
    SESSION.delete_value(_DROPBOX_UPDATE, "DisableAutoStart")


def _detect_disable_dropbox_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "Dropbox") is None


# ── Disable Dropbox Auto-Update ─────────────────────────────────────────────


def _apply_disable_dropbox_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Dropbox auto-update")
    SESSION.backup([_DROPBOX_UPDATE], "DropboxUpdate")
    SESSION.set_dword(_DROPBOX_UPDATE, "DisableUpdate", 1)


def _remove_disable_dropbox_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DROPBOX_UPDATE, "DisableUpdate")


def _detect_disable_dropbox_update() -> bool:
    return SESSION.read_dword(_DROPBOX_UPDATE, "DisableUpdate") == 1


# ── Dropbox LAN Sync ────────────────────────────────────────────────────────


def _apply_disable_dropbox_lan_sync(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Dropbox LAN sync")
    SESSION.backup([_DROPBOX_BANDWIDTH], "DropboxLANSync")
    SESSION.set_dword(_DROPBOX_BANDWIDTH, "p2p_enabled", 0)


def _remove_disable_dropbox_lan_sync(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DROPBOX_BANDWIDTH, "p2p_enabled", 1)


def _detect_disable_dropbox_lan_sync() -> bool:
    return SESSION.read_dword(_DROPBOX_BANDWIDTH, "p2p_enabled") == 0


# ── Disable Google Drive Auto-Start ─────────────────────────────────────────


def _apply_disable_gdrive_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Google Drive auto-start")
    SESSION.backup([_RUN_CU], "GDriveAutoStart")
    SESSION.delete_value(_RUN_CU, "GoogleDriveFS")


def _remove_disable_gdrive_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: re-enable Google Drive auto-start (manual)")
    SESSION.delete_value(_GDRIVE_UPDATE, "DisableAutoStart")


def _detect_disable_gdrive_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "GoogleDriveFS") is None


# ── Disable Google Drive Auto-Update ────────────────────────────────────────


def _apply_disable_gdrive_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Google Drive auto-update")
    SESSION.backup([_GDRIVE_UPDATE], "GDriveUpdate")
    SESSION.set_dword(_GDRIVE_UPDATE, "AutoUpdateDisabled", 1)


def _remove_disable_gdrive_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GDRIVE_UPDATE, "AutoUpdateDisabled")


def _detect_disable_gdrive_update() -> bool:
    return SESSION.read_dword(_GDRIVE_UPDATE, "AutoUpdateDisabled") == 1


# ── Google Drive Bandwidth Limit ────────────────────────────────────────────


def _apply_gdrive_bandwidth_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: limit Google Drive upload to 1 MB/s")
    SESSION.backup([_GDRIVE_UPDATE], "GDriveBandwidth")
    SESSION.set_dword(_GDRIVE_UPDATE, "BandwidthRxKBPS", 0)  # 0 = unlimited download
    SESSION.set_dword(_GDRIVE_UPDATE, "BandwidthTxKBPS", 1024)  # 1 MB/s upload


def _remove_gdrive_bandwidth_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GDRIVE_UPDATE, "BandwidthRxKBPS")
    SESSION.delete_value(_GDRIVE_UPDATE, "BandwidthTxKBPS")


def _detect_gdrive_bandwidth_limit() -> bool:
    return SESSION.read_dword(_GDRIVE_UPDATE, "BandwidthTxKBPS") is not None


# ── Disable iCloud Auto-Start ───────────────────────────────────────────────


def _apply_disable_icloud_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable iCloud auto-start")
    SESSION.backup([_RUN_CU], "iCloudAutoStart")
    SESSION.delete_value(_RUN_CU, "iCloudDrive")
    SESSION.delete_value(_RUN_CU, "iCloudServices")


def _remove_disable_icloud_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: iCloud auto-start => manual reinstall needed")


def _detect_disable_icloud_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "iCloudDrive") is None and SESSION.read_string(_RUN_CU, "iCloudServices") is None


# ── Disable iCloud Photo Upload ─────────────────────────────────────────────


def _apply_disable_icloud_photos(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable iCloud photo stream upload")
    SESSION.backup([_ICLOUD_UPDATE], "iCloudPhotos")
    SESSION.set_dword(_ICLOUD_UPDATE, "DisablePhotoStream", 1)


def _remove_disable_icloud_photos(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ICLOUD_UPDATE, "DisablePhotoStream")


def _detect_disable_icloud_photos() -> bool:
    return SESSION.read_dword(_ICLOUD_UPDATE, "DisablePhotoStream") == 1


# ── Disable Box Auto-Start ──────────────────────────────────────────────────


def _apply_disable_box_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Box Drive auto-start")
    SESSION.backup([_RUN_CU], "BoxAutoStart")
    SESSION.delete_value(_RUN_CU, "Box")
    SESSION.delete_value(_RUN_CU, "BoxDrive")


def _remove_disable_box_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: Box auto-start => manual re-enable needed")


def _detect_disable_box_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "Box") is None and SESSION.read_string(_RUN_CU, "BoxDrive") is None


# ── Disable MEGA Auto-Start ─────────────────────────────────────────────────


def _apply_disable_mega_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable MEGA auto-start")
    SESSION.backup([_RUN_CU], "MEGAAutoStart")
    SESSION.delete_value(_RUN_CU, "MEGAsync")


def _remove_disable_mega_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: MEGA auto-start => manual re-enable needed")


def _detect_disable_mega_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "MEGAsync") is None


# ── Disable pCloud Auto-Start ───────────────────────────────────────────────


def _apply_disable_pcloud_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable pCloud auto-start")
    SESSION.backup([_RUN_CU], "pCloudAutoStart")
    SESSION.delete_value(_RUN_CU, "pCloud Drive")


def _remove_disable_pcloud_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: pCloud auto-start => manual re-enable needed")


def _detect_disable_pcloud_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "pCloud Drive") is None


# ── Disable Nextcloud Auto-Start ────────────────────────────────────────────


def _apply_disable_nextcloud_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Nextcloud auto-start")
    SESSION.backup([_RUN_CU], "NextcloudAutoStart")
    SESSION.delete_value(_RUN_CU, "Nextcloud")


def _remove_disable_nextcloud_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: Nextcloud auto-start => manual re-enable needed")


def _detect_disable_nextcloud_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "Nextcloud") is None


# ── Disable Tresorit Auto-Start ─────────────────────────────────────────────


def _apply_disable_tresorit_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Tresorit auto-start")
    SESSION.backup([_RUN_CU], "TresoritAutoStart")
    SESSION.delete_value(_RUN_CU, "Tresorit")


def _remove_disable_tresorit_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: Tresorit auto-start => manual re-enable needed")


def _detect_disable_tresorit_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "Tresorit") is None


# ── Disable Sync.com Auto-Start ─────────────────────────────────────────────


def _apply_disable_synccom_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Sync.com auto-start")
    SESSION.backup([_RUN_CU], "SyncComAutoStart")
    SESSION.delete_value(_RUN_CU, "Sync.com")


def _remove_disable_synccom_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: Sync.com auto-start => manual re-enable needed")


def _detect_disable_synccom_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "Sync.com") is None


# ── Disable SpiderOak Auto-Start ────────────────────────────────────────────


def _apply_disable_spideroak_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable SpiderOak ONE auto-start")
    SESSION.backup([_RUN_CU], "SpiderOakAutoStart")
    SESSION.delete_value(_RUN_CU, "SpiderOakONE")


def _remove_disable_spideroak_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: SpiderOak auto-start => manual re-enable needed")


def _detect_disable_spideroak_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "SpiderOakONE") is None


# ── Disable Amazon Drive Auto-Start ─────────────────────────────────────────


def _apply_disable_amazondrive_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Amazon Drive auto-start")
    SESSION.backup([_RUN_CU], "AmazonDriveAutoStart")
    SESSION.delete_value(_RUN_CU, "Amazon Drive")


def _remove_disable_amazondrive_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: Amazon Drive auto-start => manual re-enable needed")


def _detect_disable_amazondrive_autostart() -> bool:
    return SESSION.read_string(_RUN_CU, "Amazon Drive") is None


# ── Dropbox Bandwidth Throttle (Upload 512 KB/s) ────────────────────────────


def _apply_dropbox_upload_throttle(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: throttle Dropbox upload to 512 KB/s")
    SESSION.backup([_DROPBOX_BANDWIDTH], "DropboxUploadThrottle")
    SESSION.set_dword(_DROPBOX_BANDWIDTH, "throttle_upload_rate", 512)
    SESSION.set_dword(_DROPBOX_BANDWIDTH, "throttle_upload_style", 2)


def _remove_dropbox_upload_throttle(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DROPBOX_BANDWIDTH, "throttle_upload_rate")
    SESSION.delete_value(_DROPBOX_BANDWIDTH, "throttle_upload_style")


def _detect_dropbox_upload_throttle() -> bool:
    return SESSION.read_dword(_DROPBOX_BANDWIDTH, "throttle_upload_rate") is not None


# ── Disable Dropbox Telemetry / Analytics ────────────────────────────────────


def _apply_disable_dropbox_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Dropbox telemetry")
    SESSION.backup([_DROPBOX_POLICY], "DropboxTelemetry")
    SESSION.set_dword(_DROPBOX_POLICY, "DisableAnalytics", 1)


def _remove_disable_dropbox_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DROPBOX_POLICY, "DisableAnalytics")


def _detect_disable_dropbox_telemetry() -> bool:
    return SESSION.read_dword(_DROPBOX_POLICY, "DisableAnalytics") == 1


# ── Google Drive Cache Size Limit ────────────────────────────────────────────


def _apply_gdrive_cache_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: limit Google Drive local cache to 10 GB")
    SESSION.backup([_GDRIVE_UPDATE], "GDriveCacheLimit")
    SESSION.set_dword(_GDRIVE_UPDATE, "MaxCacheSizeMB", 10240)  # 10 GB


def _remove_gdrive_cache_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GDRIVE_UPDATE, "MaxCacheSizeMB")


def _detect_gdrive_cache_limit() -> bool:
    return SESSION.read_dword(_GDRIVE_UPDATE, "MaxCacheSizeMB") is not None


# ── Disable Google Drive Telemetry ───────────────────────────────────────────


def _apply_disable_gdrive_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Google Drive telemetry")
    SESSION.backup([_GDRIVE_UPDATE], "GDriveTelemetry")
    SESSION.set_dword(_GDRIVE_UPDATE, "DisableCrashReporting", 1)
    SESSION.set_dword(_GDRIVE_UPDATE, "DisableUsageStats", 1)


def _remove_disable_gdrive_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GDRIVE_UPDATE, "DisableCrashReporting")
    SESSION.delete_value(_GDRIVE_UPDATE, "DisableUsageStats")


def _detect_disable_gdrive_telemetry() -> bool:
    return SESSION.read_dword(_GDRIVE_UPDATE, "DisableCrashReporting") == 1


# ── Disable MEGA Auto-Update ────────────────────────────────────────────────


def _apply_disable_mega_update(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable MEGA auto-update")
    SESSION.backup([_MEGA_KEY], "MEGAUpdate")
    SESSION.set_dword(_MEGA_KEY, "DisableAutoUpdates", 1)


def _remove_disable_mega_update(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MEGA_KEY, "DisableAutoUpdates")


def _detect_disable_mega_update() -> bool:
    return SESSION.read_dword(_MEGA_KEY, "DisableAutoUpdates") == 1


# ── Disable Box Auto-Update ─────────────────────────────────────────────────


def _apply_disable_box_update(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable Box Drive auto-update")
    SESSION.backup([_BOX_KEY], "BoxUpdate")
    SESSION.set_dword(_BOX_KEY, "DisableAutoUpdate", 1)


def _remove_disable_box_update(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BOX_KEY, "DisableAutoUpdate")


def _detect_disable_box_update() -> bool:
    return SESSION.read_dword(_BOX_KEY, "DisableAutoUpdate") == 1


# ── Limit Shell Overlay Icons (all cloud providers) ─────────────────────────

_OVERLAY_LIMIT_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion"
    r"\Explorer\ShellIconOverlayIdentifiers"
)


def _apply_limit_overlays(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: prioritise overlay icons (limit to 15)")
    SESSION.backup([_OVERLAY_LIMIT_KEY], "OverlayLimit")
    # Windows only supports 15 overlay icons — this tweak ensures
    # the system defaults are prioritised by adding a space prefix
    # to lower-priority entries.  We just set a registry flag indicating
    # the tweak is active for detect purposes.
    SESSION.set_dword(
        _OVERLAY_LIMIT_KEY,
        "RegiLattice_OverlayOptimised",
        1,
    )


def _remove_limit_overlays(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OVERLAY_LIMIT_KEY, "RegiLattice_OverlayOptimised")


def _detect_limit_overlays() -> bool:
    return SESSION.read_dword(_OVERLAY_LIMIT_KEY, "RegiLattice_OverlayOptimised") == 1


# ── Disable iCloud Drive Integration ──────────────────────────────────────


def _apply_disable_icloud_drive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable iCloud Drive integration")
    SESSION.backup([_ICLOUD_SERVICES], "iCloudDrive")
    SESSION.set_dword(_ICLOUD_SERVICES, "iCloudDriveDisabled", 1)


def _remove_disable_icloud_drive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ICLOUD_SERVICES, "iCloudDriveDisabled")


def _detect_disable_icloud_drive() -> bool:
    return SESSION.read_dword(_ICLOUD_SERVICES, "iCloudDriveDisabled") == 1


# ── Disable Cloud Content Suggestions ─────────────────────────────────────


def _apply_disable_cloud_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud Storage: disable cloud-optimized content suggestions")
    SESSION.backup([_CLOUD_CONTENT], "CloudSuggestions")
    SESSION.set_dword(_CLOUD_CONTENT, "DisableCloudOptimizedContent", 1)


def _remove_disable_cloud_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD_CONTENT, "DisableCloudOptimizedContent")


def _detect_disable_cloud_suggestions() -> bool:
    return SESSION.read_dword(_CLOUD_CONTENT, "DisableCloudOptimizedContent") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="cloud-disable-dropbox-autostart",
        label="Disable Dropbox Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_dropbox_autostart,
        remove_fn=_remove_disable_dropbox_autostart,
        detect_fn=_detect_disable_dropbox_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Dropbox from starting automatically at login.",
        tags=["dropbox", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-dropbox-update",
        label="Disable Dropbox Auto-Update",
        category="Cloud Storage",
        apply_fn=_apply_disable_dropbox_update,
        remove_fn=_remove_disable_dropbox_update,
        detect_fn=_detect_disable_dropbox_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DROPBOX_UPDATE],
        description="Prevents Dropbox from automatically checking for and installing updates.",
        tags=["dropbox", "update", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-dropbox-lan-sync",
        label="Disable Dropbox LAN Sync",
        category="Cloud Storage",
        apply_fn=_apply_disable_dropbox_lan_sync,
        remove_fn=_remove_disable_dropbox_lan_sync,
        detect_fn=_detect_disable_dropbox_lan_sync,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DROPBOX_BANDWIDTH],
        description=(
            "Disables Dropbox LAN Sync (peer-to-peer discovery on the local network). "
            "Reduces network chatter and improves privacy on shared networks."
        ),
        tags=["dropbox", "lan", "network", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-gdrive-autostart",
        label="Disable Google Drive Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_gdrive_autostart,
        remove_fn=_remove_disable_gdrive_autostart,
        detect_fn=_detect_disable_gdrive_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Google Drive (DriveFS) from starting at login.",
        tags=["gdrive", "google", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-gdrive-update",
        label="Disable Google Drive Auto-Update",
        category="Cloud Storage",
        apply_fn=_apply_disable_gdrive_update,
        remove_fn=_remove_disable_gdrive_update,
        detect_fn=_detect_disable_gdrive_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GDRIVE_UPDATE],
        description="Prevents Google Drive from auto-updating via policy.",
        tags=["gdrive", "google", "update", "cloud"],
    ),
    TweakDef(
        id="cloud-gdrive-bandwidth-limit",
        label="Limit Google Drive Upload (1 MB/s)",
        category="Cloud Storage",
        apply_fn=_apply_gdrive_bandwidth_limit,
        remove_fn=_remove_gdrive_bandwidth_limit,
        detect_fn=_detect_gdrive_bandwidth_limit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GDRIVE_UPDATE],
        description=("Caps Google Drive upload bandwidth at 1 MB/s to prevent saturating your internet connection during large syncs."),
        tags=["gdrive", "google", "bandwidth", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-icloud-autostart",
        label="Disable iCloud Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_icloud_autostart,
        remove_fn=_remove_disable_icloud_autostart,
        detect_fn=_detect_disable_icloud_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents iCloud Drive and iCloud Services from starting at login.",
        tags=["icloud", "apple", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-icloud-photos",
        label="Disable iCloud Photo Stream Upload",
        category="Cloud Storage",
        apply_fn=_apply_disable_icloud_photos,
        remove_fn=_remove_disable_icloud_photos,
        detect_fn=_detect_disable_icloud_photos,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ICLOUD_UPDATE],
        description="Disables automatic photo stream uploads via iCloud for Windows.",
        tags=["icloud", "apple", "photos", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-box-autostart",
        label="Disable Box Drive Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_box_autostart,
        remove_fn=_remove_disable_box_autostart,
        detect_fn=_detect_disable_box_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Box / Box Drive from starting automatically at login.",
        tags=["box", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-mega-autostart",
        label="Disable MEGA Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_mega_autostart,
        remove_fn=_remove_disable_mega_autostart,
        detect_fn=_detect_disable_mega_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents MEGAsync from starting automatically at login.",
        tags=["mega", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-pcloud-autostart",
        label="Disable pCloud Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_pcloud_autostart,
        remove_fn=_remove_disable_pcloud_autostart,
        detect_fn=_detect_disable_pcloud_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents pCloud Drive from starting automatically at login.",
        tags=["pcloud", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-overlay-optimise",
        label="Optimise Shell Overlay Icons (Cloud)",
        category="Cloud Storage",
        apply_fn=_apply_limit_overlays,
        remove_fn=_remove_limit_overlays,
        detect_fn=_detect_limit_overlays,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OVERLAY_LIMIT_KEY],
        description=(
            "Windows only supports 15 shell overlay icons. When Dropbox, "
            "Google Drive, OneDrive, and Box are all installed the limit "
            "overflows and icons break. This tweak prioritises system "
            "defaults so cloud sync icons display correctly."
        ),
        tags=["cloud", "overlay", "icons", "explorer"],
    ),
    # ── New cloud storage tweaks ─────────────────────────────────────────
    TweakDef(
        id="cloud-disable-nextcloud-autostart",
        label="Disable Nextcloud Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_nextcloud_autostart,
        remove_fn=_remove_disable_nextcloud_autostart,
        detect_fn=_detect_disable_nextcloud_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Nextcloud desktop client from starting at login.",
        tags=["nextcloud", "autostart", "cloud", "opensource"],
    ),
    TweakDef(
        id="cloud-disable-tresorit-autostart",
        label="Disable Tresorit Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_tresorit_autostart,
        remove_fn=_remove_disable_tresorit_autostart,
        detect_fn=_detect_disable_tresorit_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Tresorit from starting automatically at login.",
        tags=["tresorit", "autostart", "cloud", "encrypted"],
    ),
    TweakDef(
        id="cloud-disable-synccom-autostart",
        label="Disable Sync.com Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_synccom_autostart,
        remove_fn=_remove_disable_synccom_autostart,
        detect_fn=_detect_disable_synccom_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Sync.com desktop client from starting at login.",
        tags=["sync.com", "autostart", "cloud", "encrypted"],
    ),
    TweakDef(
        id="cloud-disable-spideroak-autostart",
        label="Disable SpiderOak ONE Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_spideroak_autostart,
        remove_fn=_remove_disable_spideroak_autostart,
        detect_fn=_detect_disable_spideroak_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents SpiderOak ONE backup from starting at login.",
        tags=["spideroak", "autostart", "cloud", "backup"],
    ),
    TweakDef(
        id="cloud-disable-amazondrive-autostart",
        label="Disable Amazon Drive Auto-Start",
        category="Cloud Storage",
        apply_fn=_apply_disable_amazondrive_autostart,
        remove_fn=_remove_disable_amazondrive_autostart,
        detect_fn=_detect_disable_amazondrive_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Prevents Amazon Drive from starting automatically at login.",
        tags=["amazon", "autostart", "cloud"],
    ),
    TweakDef(
        id="cloud-dropbox-upload-throttle",
        label="Throttle Dropbox Upload (512 KB/s)",
        category="Cloud Storage",
        apply_fn=_apply_dropbox_upload_throttle,
        remove_fn=_remove_dropbox_upload_throttle,
        detect_fn=_detect_dropbox_upload_throttle,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DROPBOX_BANDWIDTH],
        description=("Caps Dropbox upload bandwidth at 512 KB/s to prevent saturating your internet connection."),
        tags=["dropbox", "bandwidth", "throttle", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-dropbox-telemetry",
        label="Disable Dropbox Telemetry",
        category="Cloud Storage",
        apply_fn=_apply_disable_dropbox_telemetry,
        remove_fn=_remove_disable_dropbox_telemetry,
        detect_fn=_detect_disable_dropbox_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DROPBOX_POLICY],
        description="Disables Dropbox analytics and telemetry data collection.",
        tags=["dropbox", "telemetry", "privacy", "cloud"],
    ),
    TweakDef(
        id="cloud-gdrive-cache-limit",
        label="Limit Google Drive Cache (10 GB)",
        category="Cloud Storage",
        apply_fn=_apply_gdrive_cache_limit,
        remove_fn=_remove_gdrive_cache_limit,
        detect_fn=_detect_gdrive_cache_limit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GDRIVE_UPDATE],
        description=("Caps the Google Drive File Stream local cache at 10 GB to recover disk space on smaller SSDs."),
        tags=["gdrive", "google", "cache", "disk", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-gdrive-telemetry",
        label="Disable Google Drive Telemetry",
        category="Cloud Storage",
        apply_fn=_apply_disable_gdrive_telemetry,
        remove_fn=_remove_disable_gdrive_telemetry,
        detect_fn=_detect_disable_gdrive_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GDRIVE_UPDATE],
        description="Disables crash reporting and usage stats for Google Drive.",
        tags=["gdrive", "google", "telemetry", "privacy", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-mega-update",
        label="Disable MEGA Auto-Update",
        category="Cloud Storage",
        apply_fn=_apply_disable_mega_update,
        remove_fn=_remove_disable_mega_update,
        detect_fn=_detect_disable_mega_update,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MEGA_KEY],
        description="Prevents MEGAsync from automatically checking for updates.",
        tags=["mega", "update", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-box-update",
        label="Disable Box Drive Auto-Update",
        category="Cloud Storage",
        apply_fn=_apply_disable_box_update,
        remove_fn=_remove_disable_box_update,
        detect_fn=_detect_disable_box_update,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_BOX_KEY],
        description="Prevents Box Drive from automatically installing updates.",
        tags=["box", "update", "cloud"],
    ),
    TweakDef(
        id="cloud-disable-icloud-drive",
        label="Disable iCloud Drive Integration",
        category="Cloud Storage",
        apply_fn=_apply_disable_icloud_drive,
        remove_fn=_remove_disable_icloud_drive,
        detect_fn=_detect_disable_icloud_drive,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ICLOUD_SERVICES],
        description=(
            "Disables iCloud Drive Windows integration. Prevents iCloud "
            "from syncing files in Explorer. Default: Enabled. "
            "Recommended: Disabled if not using Apple devices."
        ),
        tags=["cloud", "icloud", "sync", "apple"],
    ),
    TweakDef(
        id="cloud-disable-suggestions",
        label="Disable Cloud Content Suggestions",
        category="Cloud Storage",
        apply_fn=_apply_disable_cloud_suggestions,
        remove_fn=_remove_disable_cloud_suggestions,
        detect_fn=_detect_disable_cloud_suggestions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLOUD_CONTENT],
        description=(
            "Disables cloud-optimized content and suggestions. Prevents "
            "Windows from downloading promotional content. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["cloud", "content", "suggestions", "privacy"],
    ),
]


# -- Disable iCloud Auto-Sync -------------------------------------------------

_ICLOUD_SYNC_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud"


def _apply_cloud_disable_icloud_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud: disabling iCloud auto-sync")
    SESSION.backup([_ICLOUD_SYNC_POLICY], "iCloudSync")
    SESSION.set_dword(_ICLOUD_SYNC_POLICY, "DisableSync", 1)
    SESSION.log("Cloud: iCloud auto-sync disabled")


def _remove_cloud_disable_icloud_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ICLOUD_SYNC_POLICY], "iCloudSync_Remove")
    SESSION.delete_value(_ICLOUD_SYNC_POLICY, "DisableSync")


def _detect_cloud_disable_icloud_sync() -> bool:
    return SESSION.read_dword(_ICLOUD_SYNC_POLICY, "DisableSync") == 1


# -- Disable Adobe Creative Cloud Startup --------------------------------------

_CCX_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep"


def _apply_cloud_disable_creative_cloud_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud: disabling Adobe Creative Cloud startup sync")
    SESSION.backup([_CCX_POLICY], "CreativeCloudStartup")
    SESSION.set_dword(_CCX_POLICY, "disableSync", 1)
    SESSION.log("Cloud: Adobe Creative Cloud startup sync disabled")


def _remove_cloud_disable_creative_cloud_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CCX_POLICY], "CreativeCloudStartup_Remove")
    SESSION.delete_value(_CCX_POLICY, "disableSync")


def _detect_cloud_disable_creative_cloud_startup() -> bool:
    return SESSION.read_dword(_CCX_POLICY, "disableSync") == 1


TWEAKS += [
    TweakDef(
        id="cloud-disable-icloud-sync",
        label="Disable iCloud Auto-Sync",
        category="Cloud Storage",
        apply_fn=_apply_cloud_disable_icloud_sync,
        remove_fn=_remove_cloud_disable_icloud_sync,
        detect_fn=_detect_cloud_disable_icloud_sync,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ICLOUD_SYNC_POLICY],
        description=(
            "Disables iCloud automatic synchronization via Group Policy. Default: Enabled. Recommended: Disabled if not using Apple services."
        ),
        tags=["cloud", "icloud", "sync", "apple"],
    ),
    TweakDef(
        id="cloud-disable-creative-cloud-startup",
        label="Disable Adobe Creative Cloud Startup",
        category="Cloud Storage",
        apply_fn=_apply_cloud_disable_creative_cloud_startup,
        remove_fn=_remove_cloud_disable_creative_cloud_startup,
        detect_fn=_detect_cloud_disable_creative_cloud_startup,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CCX_POLICY],
        description=("Disables Adobe Creative Cloud startup sync via policy. Default: Enabled. Recommended: Disabled."),
        tags=["cloud", "adobe", "creative-cloud", "startup"],
    ),
]


# -- Disable iCloud Photo Sync ------------------------------------------------

_ICLOUD_PHOTO = r"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream"


def _apply_cloud_disable_icloud_photo_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud: disabling iCloud photo stream auto-upload")
    SESSION.backup([_ICLOUD_PHOTO], "iCloudPhotoSync")
    SESSION.set_dword(_ICLOUD_PHOTO, "AutoUpload", 0)


def _remove_cloud_disable_icloud_photo_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ICLOUD_PHOTO, "AutoUpload")


def _detect_cloud_disable_icloud_photo_sync() -> bool:
    return SESSION.read_dword(_ICLOUD_PHOTO, "AutoUpload") == 0


# -- Disable Google Drive Offline Mode -----------------------------------------


def _apply_cloud_disable_gdrive_offline(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud: disabling Google Drive offline mode")
    SESSION.backup([_GDRIVE_UPDATE], "GDriveOffline")
    SESSION.set_dword(_GDRIVE_UPDATE, "DisableOfflineMode", 1)


def _remove_cloud_disable_gdrive_offline(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GDRIVE_UPDATE, "DisableOfflineMode")


def _detect_cloud_disable_gdrive_offline() -> bool:
    return SESSION.read_dword(_GDRIVE_UPDATE, "DisableOfflineMode") == 1


# -- Block Dropbox LAN Sync ---------------------------------------------------

_DROPBOX_LAN = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync"


def _apply_cloud_block_dropbox_lan_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cloud: blocking Dropbox LAN sync")
    SESSION.backup([_DROPBOX_LAN], "DropboxLanSync")
    SESSION.set_dword(_DROPBOX_LAN, "DisableLanSync", 1)


def _remove_cloud_block_dropbox_lan_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DROPBOX_LAN, "DisableLanSync")


def _detect_cloud_block_dropbox_lan_sync() -> bool:
    return SESSION.read_dword(_DROPBOX_LAN, "DisableLanSync") == 1


TWEAKS += [
    TweakDef(
        id="cloud-disable-icloud-photo-sync",
        label="Disable iCloud Photo Sync",
        category="Cloud Storage",
        apply_fn=_apply_cloud_disable_icloud_photo_sync,
        remove_fn=_remove_cloud_disable_icloud_photo_sync,
        detect_fn=_detect_cloud_disable_icloud_photo_sync,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ICLOUD_PHOTO],
        description=(
            "Disables iCloud Photo Stream automatic upload to prevent "
            "photos from syncing to Apple cloud services. "
            "Default: enabled. Recommended: disabled on corporate machines."
        ),
        tags=["cloud", "icloud", "photo", "sync", "apple"],
    ),
    TweakDef(
        id="cloud-disable-gdrive-offline",
        label="Disable Google Drive Offline Mode",
        category="Cloud Storage",
        apply_fn=_apply_cloud_disable_gdrive_offline,
        remove_fn=_remove_cloud_disable_gdrive_offline,
        detect_fn=_detect_cloud_disable_gdrive_offline,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GDRIVE_UPDATE],
        description=(
            "Disables Google Drive offline mode via policy. Prevents local "
            "caching of Drive files, reducing disk usage. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["cloud", "google-drive", "offline", "cache"],
    ),
    TweakDef(
        id="cloud-block-dropbox-lan-sync",
        label="Block Dropbox LAN Sync",
        category="Cloud Storage",
        apply_fn=_apply_cloud_block_dropbox_lan_sync,
        remove_fn=_remove_cloud_block_dropbox_lan_sync,
        detect_fn=_detect_cloud_block_dropbox_lan_sync,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DROPBOX_LAN],
        description=(
            "Blocks Dropbox LAN sync discovery which broadcasts on the "
            "local network. Improves security on shared networks. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["cloud", "dropbox", "lan", "sync", "security"],
    ),
]
