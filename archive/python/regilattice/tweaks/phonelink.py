"""Phone Link tweaks — Microsoft Phone Link (Your Phone) app settings.

Covers: Phone Link synchronisation, notifications, cross-device features,
call handling, photos sync, and app telemetry.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_PL_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink"
_CROSS_DEVICE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
_CDP_USER = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"
_CDP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP"
_PL_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc"
_CROSS_DEVICE_EXP = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass"


# ── Disable Phone Link entirely ─────────────────────────────────────────────


def _apply_disable_phonelink(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Phone Link: disable via policy")
    SESSION.backup([_PL_POLICY], "PhoneLinkDisable")
    SESSION.set_dword(_PL_POLICY, "PhoneLinkEnabled", 0)


def _remove_disable_phonelink(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PL_POLICY, "PhoneLinkEnabled")


def _detect_disable_phonelink() -> bool:
    return SESSION.read_dword(_PL_POLICY, "PhoneLinkEnabled") == 0


# ── Disable Cross-Device Experience ──────────────────────────────────────────


def _apply_disable_cross_device(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Phone Link: disable cross-device experience")
    SESSION.backup([_CROSS_DEVICE], "CrossDevice")
    SESSION.set_dword(_CROSS_DEVICE, "EnableCdp", 0)


def _remove_disable_cross_device(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CROSS_DEVICE, "EnableCdp")


def _detect_disable_cross_device() -> bool:
    return SESSION.read_dword(_CROSS_DEVICE, "EnableCdp") == 0


# ── Disable Cross-Device Clipboard ──────────────────────────────────────────


def _apply_disable_cross_clipboard(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable cross-device clipboard")
    SESSION.backup([_CDP_USER], "CrossDeviceClipboard")
    SESSION.set_dword(_CDP_USER, "RomeSdkChannelUserAuthzPolicy", 0)


def _remove_disable_cross_clipboard(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CDP_USER, "RomeSdkChannelUserAuthzPolicy")


def _detect_disable_cross_clipboard() -> bool:
    return SESSION.read_dword(_CDP_USER, "RomeSdkChannelUserAuthzPolicy") == 0


# ── Disable Phone Service ───────────────────────────────────────────────────


def _apply_disable_phone_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Phone Link: disable PhoneSvc service")
    SESSION.backup([_PL_SVC], "PhoneSvc")
    SESSION.set_dword(_PL_SVC, "Start", 4)


def _remove_disable_phone_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PL_SVC, "Start", 3)


def _detect_disable_phone_svc() -> bool:
    return SESSION.read_dword(_PL_SVC, "Start") == 4


# ── Disable Cross-Device Notifications ──────────────────────────────────────


def _apply_disable_cross_notifications(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable cross-device notifications")
    SESSION.backup([_CDP_USER], "CrossNotify")
    SESSION.set_dword(_CDP_USER, "EnableNotificationSync", 0)


def _remove_disable_cross_notifications(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CDP_USER, "EnableNotificationSync", 1)


def _detect_disable_cross_notifications() -> bool:
    return SESSION.read_dword(_CDP_USER, "EnableNotificationSync") == 0


# ── Disable CDP Platform via Policy ─────────────────────────────────────────


def _apply_disable_cdp_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Phone Link: disable CDP platform via policy")
    SESSION.backup([_CDP_POLICY], "CDPPolicy")
    SESSION.set_dword(_CDP_POLICY, "EnableCDP", 0)


def _remove_disable_cdp_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDP_POLICY, "EnableCDP")


def _detect_disable_cdp_policy() -> bool:
    return SESSION.read_dword(_CDP_POLICY, "EnableCDP") == 0


# ── Disable Cross-Device App Launch ─────────────────────────────────────────


def _apply_disable_app_launch(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable cross-device app launch")
    SESSION.backup([_CDP_USER], "CrossAppLaunch")
    SESSION.set_dword(_CDP_USER, "EnableRemoteLaunch", 0)


def _remove_disable_app_launch(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CDP_USER, "EnableRemoteLaunch", 1)


def _detect_disable_app_launch() -> bool:
    return SESSION.read_dword(_CDP_USER, "EnableRemoteLaunch") == 0


# ── Disable Cross-Device SMS ────────────────────────────────────────────────


def _apply_disable_cross_sms(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable cross-device SMS relay")
    SESSION.backup([_CDP_USER], "CrossSMS")
    SESSION.set_dword(_CDP_USER, "EnableSmsSync", 0)


def _remove_disable_cross_sms(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CDP_USER, "EnableSmsSync", 1)


def _detect_disable_cross_sms() -> bool:
    return SESSION.read_dword(_CDP_USER, "EnableSmsSync") == 0


# ── Disable Nearby Share ────────────────────────────────────────────────────


def _apply_disable_nearby_share(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable nearby sharing")
    SESSION.backup([_CDP_USER], "NearbyShare")
    SESSION.set_dword(_CDP_USER, "NearShareChannelUserAuthzPolicy", 0)


def _remove_disable_nearby_share(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CDP_USER, "NearShareChannelUserAuthzPolicy", 2)


def _detect_disable_nearby_share() -> bool:
    return SESSION.read_dword(_CDP_USER, "NearShareChannelUserAuthzPolicy") == 0


# ── Disable SmartGlass (Xbox Companion) ──────────────────────────────────────


def _apply_disable_smartglass(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable SmartGlass companion")
    SESSION.backup([_CROSS_DEVICE_EXP], "SmartGlass")
    SESSION.set_dword(_CROSS_DEVICE_EXP, "UserAuthPolicy", 0)


def _remove_disable_smartglass(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CROSS_DEVICE_EXP, "UserAuthPolicy", 1)


def _detect_disable_smartglass() -> bool:
    return SESSION.read_dword(_CROSS_DEVICE_EXP, "UserAuthPolicy") == 0


# ── Disable CDP User Activity Upload ────────────────────────────────────────


def _apply_disable_activity_upload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Phone Link: disable activity history upload")
    SESSION.backup([_CDP_POLICY], "ActivityUpload")
    SESSION.set_dword(_CDP_POLICY, "EnableActivityFeed", 0)
    SESSION.set_dword(_CDP_POLICY, "UploadUserActivities", 0)


def _remove_disable_activity_upload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDP_POLICY, "EnableActivityFeed")
    SESSION.delete_value(_CDP_POLICY, "UploadUserActivities")


def _detect_disable_activity_upload() -> bool:
    return SESSION.read_dword(_CDP_POLICY, "UploadUserActivities") == 0


# ── Disable Cross-Device Resume ─────────────────────────────────────────────


def _apply_disable_cross_resume(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable cross-device resume feature")
    SESSION.backup([_CDP_USER], "CrossResume")
    SESSION.set_dword(_CDP_USER, "EnableShare", 0)


def _remove_disable_cross_resume(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CDP_USER, "EnableShare", 1)


def _detect_disable_cross_resume() -> bool:
    return SESSION.read_dword(_CDP_USER, "EnableShare") == 0


# ── Disable Phone Link Suggestions ──────────────────────────────────────────


def _apply_disable_pl_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Phone Link: disable Phone Link suggestions and promos")
    SESSION.backup([_PL_POLICY], "PhoneLinkSuggestions")
    SESSION.set_dword(_PL_POLICY, "ShowPhoneLinkSuggestions", 0)


def _remove_disable_pl_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PL_POLICY, "ShowPhoneLinkSuggestions")


def _detect_disable_pl_suggestions() -> bool:
    return SESSION.read_dword(_PL_POLICY, "ShowPhoneLinkSuggestions") == 0


# ── Disable cross-device Bluetooth relay ─────────────────────────────────────


def _apply_disable_bt_relay(*, require_admin: bool = False) -> None:
    SESSION.log("Phone Link: disable Bluetooth phone relay")
    SESSION.backup([_CDP_USER], "BTRelay")
    SESSION.set_dword(_CDP_USER, "BluetoothLastUsedSend", 0)


def _remove_disable_bt_relay(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CDP_USER, "BluetoothLastUsedSend")


def _detect_disable_bt_relay() -> bool:
    return SESSION.read_dword(_CDP_USER, "BluetoothLastUsedSend") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="phone-disable-phonelink",
        label="Disable Phone Link (Policy)",
        category="Phone Link",
        apply_fn=_apply_disable_phonelink,
        remove_fn=_remove_disable_phonelink,
        detect_fn=_detect_disable_phonelink,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PL_POLICY],
        description="Disables Microsoft Phone Link (Your Phone) app via Group Policy. Default: Enabled. Recommended: Disabled for privacy.",
        tags=["phone-link", "your-phone", "privacy", "disable"],
    ),
    TweakDef(
        id="phone-disable-cross-device",
        label="Disable Cross-Device Experience",
        category="Phone Link",
        apply_fn=_apply_disable_cross_device,
        remove_fn=_remove_disable_cross_device,
        detect_fn=_detect_disable_cross_device,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CROSS_DEVICE],
        description="Disables the Connected Devices Platform (CDP) that powers cross-device features. Default: Enabled.",
        tags=["phone-link", "cross-device", "cdp", "privacy"],
    ),
    TweakDef(
        id="phone-disable-cross-clipboard",
        label="Disable Cross-Device Clipboard",
        category="Phone Link",
        apply_fn=_apply_disable_cross_clipboard,
        remove_fn=_remove_disable_cross_clipboard,
        detect_fn=_detect_disable_cross_clipboard,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Stops clipboard data from being shared between Windows and linked phone/tablet. Default: Enabled.",
        tags=["phone-link", "clipboard", "cross-device", "privacy"],
    ),
    TweakDef(
        id="phone-disable-phone-svc",
        label="Disable Phone Service (PhoneSvc)",
        category="Phone Link",
        apply_fn=_apply_disable_phone_svc,
        remove_fn=_remove_disable_phone_svc,
        detect_fn=_detect_disable_phone_svc,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PL_SVC],
        description="Disables the Windows Phone service (telephony state management). Frees resources if Phone Link is not used.",
        tags=["phone-link", "service", "disable"],
    ),
    TweakDef(
        id="phone-disable-cross-notifications",
        label="Disable Cross-Device Notifications",
        category="Phone Link",
        apply_fn=_apply_disable_cross_notifications,
        remove_fn=_remove_disable_cross_notifications,
        detect_fn=_detect_disable_cross_notifications,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Stops phone notifications from appearing on your Windows desktop. Default: Enabled.",
        tags=["phone-link", "notifications", "cross-device"],
    ),
    TweakDef(
        id="phone-disable-cdp-policy",
        label="Disable CDP Platform (Policy)",
        category="Phone Link",
        apply_fn=_apply_disable_cdp_policy,
        remove_fn=_remove_disable_cdp_policy,
        detect_fn=_detect_disable_cdp_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CDP_POLICY],
        description="Disables the Connected Devices Platform entirely via machine policy. Blocks all cross-device features.",
        tags=["phone-link", "cdp", "policy", "privacy"],
    ),
    TweakDef(
        id="phone-disable-app-launch",
        label="Disable Cross-Device App Launch",
        category="Phone Link",
        apply_fn=_apply_disable_app_launch,
        remove_fn=_remove_disable_app_launch,
        detect_fn=_detect_disable_app_launch,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Prevents apps from being launched remotely across devices. Default: Enabled.",
        tags=["phone-link", "app-launch", "cross-device"],
    ),
    TweakDef(
        id="phone-disable-cross-sms",
        label="Disable Cross-Device SMS",
        category="Phone Link",
        apply_fn=_apply_disable_cross_sms,
        remove_fn=_remove_disable_cross_sms,
        detect_fn=_detect_disable_cross_sms,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Disables SMS message sync between phone and PC. Default: Enabled.",
        tags=["phone-link", "sms", "sync", "privacy"],
    ),
    TweakDef(
        id="phone-disable-nearby-share",
        label="Disable Nearby Share",
        category="Phone Link",
        apply_fn=_apply_disable_nearby_share,
        remove_fn=_remove_disable_nearby_share,
        detect_fn=_detect_disable_nearby_share,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Disables Windows Nearby Sharing feature (file/link transfer to nearby devices). Default: Enabled.",
        tags=["phone-link", "nearby-share", "sharing"],
    ),
    TweakDef(
        id="phone-disable-smartglass",
        label="Disable SmartGlass Companion",
        category="Phone Link",
        apply_fn=_apply_disable_smartglass,
        remove_fn=_remove_disable_smartglass,
        detect_fn=_detect_disable_smartglass,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CROSS_DEVICE_EXP],
        description="Disables Xbox SmartGlass companion features for cross-device gaming.",
        tags=["phone-link", "smartglass", "xbox", "cross-device"],
    ),
    TweakDef(
        id="phone-disable-activity-upload",
        label="Disable Activity History Upload",
        category="Phone Link",
        apply_fn=_apply_disable_activity_upload,
        remove_fn=_remove_disable_activity_upload,
        detect_fn=_detect_disable_activity_upload,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CDP_POLICY],
        description="Stops Windows from uploading activity history to Microsoft cloud for Timeline and cross-device resume. Recommended.",
        tags=["phone-link", "activity", "timeline", "privacy"],
    ),
    TweakDef(
        id="phone-disable-cross-resume",
        label="Disable Cross-Device Resume",
        category="Phone Link",
        apply_fn=_apply_disable_cross_resume,
        remove_fn=_remove_disable_cross_resume,
        detect_fn=_detect_disable_cross_resume,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Disables the ability to resume activities on other devices. Default: Enabled.",
        tags=["phone-link", "cross-device", "resume"],
    ),
    TweakDef(
        id="phone-disable-suggestions",
        label="Disable Phone Link Suggestions",
        category="Phone Link",
        apply_fn=_apply_disable_pl_suggestions,
        remove_fn=_remove_disable_pl_suggestions,
        detect_fn=_detect_disable_pl_suggestions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PL_POLICY],
        description="Disables Phone Link promotional suggestions and tips in Windows. Default: Enabled.",
        tags=["phone-link", "suggestions", "ads"],
    ),
    TweakDef(
        id="phone-disable-bt-relay",
        label="Disable Bluetooth Phone Relay",
        category="Phone Link",
        apply_fn=_apply_disable_bt_relay,
        remove_fn=_remove_disable_bt_relay,
        detect_fn=_detect_disable_bt_relay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDP_USER],
        description="Disables Bluetooth relay used for phone-to-PC communication in Phone Link.",
        tags=["phone-link", "bluetooth", "relay"],
    ),
]


# ── Disable Phone Link Wi-Fi Direct ──────────────────────────────────────────

_WIFI_DIRECT = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"


def _apply_disable_wifidirect(*, require_admin: bool = False) -> None:
    SESSION.log("PhoneLink: disable Wi-Fi Direct transport")
    SESSION.backup([_WIFI_DIRECT], "WifiDirect")
    SESSION.set_dword(_WIFI_DIRECT, "WifiDirectEnabled", 0)


def _remove_disable_wifidirect(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_WIFI_DIRECT, "WifiDirectEnabled", 1)


def _detect_disable_wifidirect() -> bool:
    return SESSION.read_dword(_WIFI_DIRECT, "WifiDirectEnabled") == 0


# ── Disable Windows Timeline Activity Upload ─────────────────────────────────

_TIMELINE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_disable_timeline(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("PhoneLink: disable Windows Timeline activity history")
    SESSION.backup([_TIMELINE_POLICY], "Timeline")
    SESSION.set_dword(_TIMELINE_POLICY, "EnableActivityFeed", 0)
    SESSION.set_dword(_TIMELINE_POLICY, "AllowCrossDeviceClipboard", 0)


def _remove_disable_timeline(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TIMELINE_POLICY, "EnableActivityFeed", 1)
    SESSION.set_dword(_TIMELINE_POLICY, "AllowCrossDeviceClipboard", 1)


def _detect_disable_timeline() -> bool:
    return SESSION.read_dword(_TIMELINE_POLICY, "EnableActivityFeed") == 0


# ── Disable Phone Link Photos Auto-Sync ──────────────────────────────────────

_PL_PHOTOS = r"HKEY_CURRENT_USER\Software\Microsoft\YourPhone"


def _apply_disable_photos_sync(*, require_admin: bool = False) -> None:
    SESSION.log("PhoneLink: disable automatic phone photos sync")
    SESSION.backup([_PL_PHOTOS], "PhotosSync")
    SESSION.set_dword(_PL_PHOTOS, "DisablePhotoSync", 1)


def _remove_disable_photos_sync(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PL_PHOTOS, "DisablePhotoSync", 0)


def _detect_disable_photos_sync() -> bool:
    return SESSION.read_dword(_PL_PHOTOS, "DisablePhotoSync") == 1


# ── Disable Phone Link App Notifications ─────────────────────────────────────

_PL_NOTIF = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP\SettingsPage"


def _apply_disable_pl_notifications(*, require_admin: bool = False) -> None:
    SESSION.log("PhoneLink: disable cross-device app notifications")
    SESSION.backup([_PL_NOTIF], "PlNotifications")
    SESSION.set_dword(_PL_NOTIF, "AppNotificationsEnabled", 0)


def _remove_disable_pl_notifications(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PL_NOTIF, "AppNotificationsEnabled", 1)


def _detect_disable_pl_notifications() -> bool:
    return SESSION.read_dword(_PL_NOTIF, "AppNotificationsEnabled") == 0


TWEAKS += [
    TweakDef(
        id="phone-disable-wifidirect",
        label="Disable Phone Link Wi-Fi Direct",
        category="Phone Link",
        apply_fn=_apply_disable_wifidirect,
        remove_fn=_remove_disable_wifidirect,
        detect_fn=_detect_disable_wifidirect,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WIFI_DIRECT],
        description=(
            "Disables Wi-Fi Direct transport used by Phone Link for high-speed "
            "cross-device data transfer. Reduces background radio use. Default: Enabled."
        ),
        tags=["phone-link", "wifi", "wifi-direct", "network", "privacy"],
    ),
    TweakDef(
        id="phone-disable-timeline",
        label="Disable Windows Timeline Activity Feed",
        category="Phone Link",
        apply_fn=_apply_disable_timeline,
        remove_fn=_remove_disable_timeline,
        detect_fn=_detect_disable_timeline,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TIMELINE_POLICY],
        description=(
            "Disables Windows Timeline activity feed and cross-device clipboard via policy. "
            "Stops syncing browsing and app activity to Microsoft cloud. Default: Enabled."
        ),
        tags=["phone-link", "timeline", "activity", "privacy", "cloud"],
    ),
    TweakDef(
        id="phone-disable-photos-sync",
        label="Disable Phone Photos Auto-Sync",
        category="Phone Link",
        apply_fn=_apply_disable_photos_sync,
        remove_fn=_remove_disable_photos_sync,
        detect_fn=_detect_disable_photos_sync,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PL_PHOTOS],
        description=(
            "Disables automatic phone photo synchronisation via the Phone Link app. "
            "Prevents background syncing of photos to the PC. Default: Enabled."
        ),
        tags=["phone-link", "photos", "sync", "privacy", "storage"],
    ),
    TweakDef(
        id="phone-disable-app-notifications",
        label="Disable Phone Link App Notifications",
        category="Phone Link",
        apply_fn=_apply_disable_pl_notifications,
        remove_fn=_remove_disable_pl_notifications,
        detect_fn=_detect_disable_pl_notifications,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PL_NOTIF],
        description=(
            "Disables cross-device app notifications relayed from mobile to PC via Phone Link. "
            "Reduces notification noise and background connectivity. Default: Enabled."
        ),
        tags=["phone-link", "notifications", "apps", "privacy"],
    ),
]
