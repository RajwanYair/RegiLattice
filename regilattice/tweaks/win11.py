"""Windows 11 extra tweaks — Widgets, Snap, Start Menu, Notifications, etc.

These are additional Windows 11 registry tweaks beyond the original set,
covering UI debloating, notification management, and UX improvements.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Disable Widgets (News & Interests) ──────────────────────────────────────

_WIDGETS_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"
_WIDGETS_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
)
_WIDGETS_KEYS = [_WIDGETS_KEY, _WIDGETS_CU]


def apply_disable_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableWidgets")
    SESSION.backup(_WIDGETS_KEYS, "Widgets")
    SESSION.set_dword(_WIDGETS_KEY, "AllowNewsAndInterests", 0)
    SESSION.set_dword(_WIDGETS_CU, "TaskbarDa", 0)
    SESSION.log("Completed Add-DisableWidgets")


def remove_disable_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableWidgets")
    SESSION.backup(_WIDGETS_KEYS, "Widgets_Remove")
    SESSION.delete_value(_WIDGETS_KEY, "AllowNewsAndInterests")
    SESSION.set_dword(_WIDGETS_CU, "TaskbarDa", 1)
    SESSION.log("Completed Remove-DisableWidgets")


def detect_disable_widgets() -> bool:
    return SESSION.read_dword(_WIDGETS_CU, "TaskbarDa") == 0


# ── Disable Snap Assist ─────────────────────────────────────────────────────

_SNAP_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
)


def apply_disable_snap_assist(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableSnapAssist")
    SESSION.backup([_SNAP_KEY], "SnapAssist")
    SESSION.set_dword(_SNAP_KEY, "SnapAssist", 0)
    SESSION.set_dword(_SNAP_KEY, "EnableSnapAssistFlyout", 0)
    SESSION.set_dword(_SNAP_KEY, "EnableSnapBar", 0)
    SESSION.log("Completed Add-DisableSnapAssist")


def remove_disable_snap_assist(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableSnapAssist")
    SESSION.backup([_SNAP_KEY], "SnapAssist_Remove")
    SESSION.set_dword(_SNAP_KEY, "SnapAssist", 1)
    SESSION.delete_value(_SNAP_KEY, "EnableSnapAssistFlyout")
    SESSION.delete_value(_SNAP_KEY, "EnableSnapBar")
    SESSION.log("Completed Remove-DisableSnapAssist")


def detect_disable_snap_assist() -> bool:
    return SESSION.read_dword(_SNAP_KEY, "SnapAssist") == 0


# ── Classic Right-Click Context Menu ────────────────────────────────────────

_CTX_KEY = (
    r"HKEY_CURRENT_USER\Software\Classes\CLSID"
    r"\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"
)


def apply_classic_context_menu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-ClassicContextMenu")
    SESSION.backup([_CTX_KEY], "ClassicContextMenu")
    # Setting default value to empty string triggers classic menu
    SESSION.set_string(_CTX_KEY, None, "")
    SESSION.log("Completed Add-ClassicContextMenu")


def remove_classic_context_menu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-ClassicContextMenu")
    SESSION.backup([_CTX_KEY], "ClassicContextMenu_Remove")
    SESSION.delete_tree(_CTX_KEY)
    SESSION.log("Completed Remove-ClassicContextMenu")


def detect_classic_context_menu() -> bool:
    return SESSION.key_exists(_CTX_KEY)


# ── Disable Lock Screen Tips / Spotlight ─────────────────────────────────────

_LOCKSCREEN_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion"
    r"\ContentDeliveryManager"
)


def apply_disable_lockscreen_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableLockScreenTips")
    SESSION.backup([_LOCKSCREEN_KEY], "LockScreenTips")
    SESSION.set_dword(_LOCKSCREEN_KEY, "RotatingLockScreenOverlayEnabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SubscribedContent-338387Enabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SubscribedContent-338388Enabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SubscribedContent-338389Enabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SubscribedContent-353694Enabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SubscribedContent-353696Enabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SilentInstalledAppsEnabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SoftLandingEnabled", 0)
    SESSION.set_dword(_LOCKSCREEN_KEY, "SystemPaneSuggestionsEnabled", 0)
    SESSION.log("Completed Add-DisableLockScreenTips")


def remove_disable_lockscreen_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableLockScreenTips")
    SESSION.backup([_LOCKSCREEN_KEY], "LockScreenTips_Remove")
    for val in (
        "RotatingLockScreenOverlayEnabled",
        "SubscribedContent-338387Enabled",
        "SubscribedContent-338388Enabled",
        "SubscribedContent-338389Enabled",
        "SubscribedContent-353694Enabled",
        "SubscribedContent-353696Enabled",
        "SilentInstalledAppsEnabled",
        "SoftLandingEnabled",
        "SystemPaneSuggestionsEnabled",
    ):
        SESSION.delete_value(_LOCKSCREEN_KEY, val)
    SESSION.log("Completed Remove-DisableLockScreenTips")


def detect_disable_lockscreen_tips() -> bool:
    return SESSION.read_dword(_LOCKSCREEN_KEY, "SoftLandingEnabled") == 0


# ── Disable Windows Update Auto-Restart ──────────────────────────────────────

_WU_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"


def apply_disable_wu_autorestart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableWUAutoRestart")
    SESSION.backup([_WU_KEY], "WUAutoRestart")
    SESSION.set_dword(_WU_KEY, "NoAutoRebootWithLoggedOnUsers", 1)
    SESSION.set_dword(_WU_KEY, "AUOptions", 2)  # 2=notify before download
    SESSION.log("Completed Add-DisableWUAutoRestart")


def remove_disable_wu_autorestart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableWUAutoRestart")
    SESSION.backup([_WU_KEY], "WUAutoRestart_Remove")
    SESSION.delete_value(_WU_KEY, "NoAutoRebootWithLoggedOnUsers")
    SESSION.set_dword(_WU_KEY, "AUOptions", 4)  # 4=auto-install (default)
    SESSION.log("Completed Remove-DisableWUAutoRestart")


def detect_disable_wu_autorestart() -> bool:
    return SESSION.read_dword(_WU_KEY, "NoAutoRebootWithLoggedOnUsers") == 1


# ── Disable Bing Search in Start Menu ────────────────────────────────────────

_SEARCH_KEY = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"


def apply_disable_bing_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableBingSearch")
    SESSION.backup([_SEARCH_KEY], "BingSearch")
    SESSION.set_dword(_SEARCH_KEY, "DisableSearchBoxSuggestions", 1)
    SESSION.log("Completed Add-DisableBingSearch")


def remove_disable_bing_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableBingSearch")
    SESSION.backup([_SEARCH_KEY], "BingSearch_Remove")
    SESSION.delete_value(_SEARCH_KEY, "DisableSearchBoxSuggestions")
    SESSION.log("Completed Remove-DisableBingSearch")


def detect_disable_bing_search() -> bool:
    return SESSION.read_dword(_SEARCH_KEY, "DisableSearchBoxSuggestions") == 1


# ── Disable App Suggestions (Bloatware) in Start Menu ───────────────────────

_SUGGEST_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion"
    r"\ContentDeliveryManager"
)


def apply_disable_app_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableAppSuggestions")
    SESSION.backup([_SUGGEST_KEY], "AppSuggestions")
    SESSION.set_dword(_SUGGEST_KEY, "SubscribedContent-310093Enabled", 0)
    SESSION.set_dword(_SUGGEST_KEY, "SubscribedContent-338393Enabled", 0)
    SESSION.set_dword(_SUGGEST_KEY, "OemPreInstalledAppsEnabled", 0)
    SESSION.set_dword(_SUGGEST_KEY, "PreInstalledAppsEnabled", 0)
    SESSION.set_dword(_SUGGEST_KEY, "PreInstalledAppsEverEnabled", 0)
    SESSION.log("Completed Add-DisableAppSuggestions")


def remove_disable_app_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableAppSuggestions")
    SESSION.backup([_SUGGEST_KEY], "AppSuggestions_Remove")
    for val in (
        "SubscribedContent-310093Enabled",
        "SubscribedContent-338393Enabled",
        "OemPreInstalledAppsEnabled",
        "PreInstalledAppsEnabled",
        "PreInstalledAppsEverEnabled",
    ):
        SESSION.delete_value(_SUGGEST_KEY, val)
    SESSION.log("Completed Remove-DisableAppSuggestions")


def detect_disable_app_suggestions() -> bool:
    return SESSION.read_dword(_SUGGEST_KEY, "PreInstalledAppsEnabled") == 0


# ── Set Dark Mode ────────────────────────────────────────────────────────────

_PERSONALIZE_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"
)


def apply_dark_mode(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DarkMode")
    SESSION.backup([_PERSONALIZE_KEY], "DarkMode")
    SESSION.set_dword(_PERSONALIZE_KEY, "AppsUseLightTheme", 0)
    SESSION.set_dword(_PERSONALIZE_KEY, "SystemUsesLightTheme", 0)
    SESSION.log("Completed Add-DarkMode")


def remove_dark_mode(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DarkMode")
    SESSION.backup([_PERSONALIZE_KEY], "DarkMode_Remove")
    SESSION.set_dword(_PERSONALIZE_KEY, "AppsUseLightTheme", 1)
    SESSION.set_dword(_PERSONALIZE_KEY, "SystemUsesLightTheme", 1)
    SESSION.log("Completed Remove-DarkMode")


def detect_dark_mode() -> bool:
    return SESSION.read_dword(_PERSONALIZE_KEY, "AppsUseLightTheme") == 0


# ── Disable Notifications ───────────────────────────────────────────────────

_NOTIF_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion" r"\PushNotifications"
)
_TOAST_KEY = (
    r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion"
    r"\PushNotifications"
)


def apply_disable_notifications(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableNotifications")
    SESSION.backup([_NOTIF_KEY, _TOAST_KEY], "Notifications")
    SESSION.set_dword(_NOTIF_KEY, "ToastEnabled", 0)
    SESSION.set_dword(_TOAST_KEY, "NoToastApplicationNotification", 1)
    SESSION.log("Completed Add-DisableNotifications")


def remove_disable_notifications(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableNotifications")
    SESSION.backup([_NOTIF_KEY, _TOAST_KEY], "Notifications_Remove")
    SESSION.set_dword(_NOTIF_KEY, "ToastEnabled", 1)
    SESSION.delete_value(_TOAST_KEY, "NoToastApplicationNotification")
    SESSION.log("Completed Remove-DisableNotifications")


def detect_disable_notifications() -> bool:
    return SESSION.read_dword(_NOTIF_KEY, "ToastEnabled") == 0


# ── Disable Snap Layout Flyout ───────────────────────────────────────────────

_SNAP_FLYOUT_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
)


def apply_disable_snap_flyout(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Win11: disable snap layout flyout on maximize hover")
    SESSION.backup([_SNAP_FLYOUT_KEY], "SnapFlyout")
    SESSION.set_dword(_SNAP_FLYOUT_KEY, "EnableSnapAssistFlyout", 0)


def remove_disable_snap_flyout(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SNAP_FLYOUT_KEY, "EnableSnapAssistFlyout", 1)


def detect_disable_snap_flyout() -> bool:
    return SESSION.read_dword(_SNAP_FLYOUT_KEY, "EnableSnapAssistFlyout") == 0


# ── Disable Taskbar Chat Icon ────────────────────────────────────────────────

_CHAT_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
)


def apply_disable_chat_icon(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Win11: disable taskbar chat icon")
    SESSION.backup([_CHAT_KEY], "ChatIcon")
    SESSION.set_dword(_CHAT_KEY, "TaskbarMn", 0)


def remove_disable_chat_icon(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CHAT_KEY, "TaskbarMn", 1)


def detect_disable_chat_icon() -> bool:
    return SESSION.read_dword(_CHAT_KEY, "TaskbarMn") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-widgets",
        label="Disable Widgets (News & Interests)",
        category="Windows 11",
        apply_fn=apply_disable_widgets,
        remove_fn=remove_disable_widgets,
        detect_fn=detect_disable_widgets,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_WIDGETS_KEYS,
        description="Removes the Widgets button from the taskbar and disables the feed.",
        tags=["win11", "taskbar", "debloat"],
    ),
    TweakDef(
        id="disable-snap-assist",
        label="Disable Snap Assist & Flyout",
        category="Windows 11",
        apply_fn=apply_disable_snap_assist,
        remove_fn=remove_disable_snap_assist,
        detect_fn=detect_disable_snap_assist,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP_KEY],
        description="Disables the Snap Assist suggestion panel and Snap Bar when hovering maximize.",
        tags=["win11", "window-management", "ux"],
    ),
    TweakDef(
        id="classic-context-menu",
        label="Classic Right-Click Context Menu",
        category="Windows 11",
        apply_fn=apply_classic_context_menu,
        remove_fn=remove_classic_context_menu,
        detect_fn=detect_classic_context_menu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CTX_KEY],
        description=(
            "Restores the full Windows 10 right-click context menu, "
            "bypassing the truncated Windows 11 menu."
        ),
        tags=["win11", "context-menu", "ux"],
    ),
    TweakDef(
        id="disable-lockscreen-tips",
        label="Disable Lock Screen Tips & Spotlight",
        category="Windows 11",
        apply_fn=apply_disable_lockscreen_tips,
        remove_fn=remove_disable_lockscreen_tips,
        detect_fn=detect_disable_lockscreen_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LOCKSCREEN_KEY],
        description=(
            "Disables Windows Spotlight, lock screen tips, suggested apps, "
            "and silent app installs."
        ),
        tags=["win11", "lockscreen", "spotlight", "debloat"],
    ),
    TweakDef(
        id="disable-wu-autorestart",
        label="Disable Windows Update Auto-Restart",
        category="Windows 11",
        apply_fn=apply_disable_wu_autorestart,
        remove_fn=remove_disable_wu_autorestart,
        detect_fn=detect_disable_wu_autorestart,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WU_KEY],
        description=(
            "Prevents Windows Update from auto-rebooting while a user "
            "is logged in. Sets updates to notify-before-download."
        ),
        tags=["win11", "update", "reboot"],
    ),
    TweakDef(
        id="disable-bing-search",
        label="Disable Bing Search in Start Menu",
        category="Windows 11",
        apply_fn=apply_disable_bing_search,
        remove_fn=remove_disable_bing_search,
        detect_fn=detect_disable_bing_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_KEY],
        description="Prevents Start Menu from sending search queries to Bing.",
        tags=["win11", "search", "privacy"],
    ),
    TweakDef(
        id="disable-app-suggestions",
        label="Disable App Suggestions & Bloatware",
        category="Windows 11",
        apply_fn=apply_disable_app_suggestions,
        remove_fn=remove_disable_app_suggestions,
        detect_fn=detect_disable_app_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SUGGEST_KEY],
        description=(
            "Disables suggested apps in Start Menu and prevents "
            "OEM/pre-installed app promotions."
        ),
        tags=["win11", "start-menu", "debloat"],
    ),
    TweakDef(
        id="dark-mode",
        label="Enable System-Wide Dark Mode",
        category="Windows 11",
        apply_fn=apply_dark_mode,
        remove_fn=remove_dark_mode,
        detect_fn=detect_dark_mode,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PERSONALIZE_KEY],
        description="Sets both app and system theme to dark mode.",
        tags=["win11", "theme", "appearance"],
    ),
    TweakDef(
        id="disable-notifications",
        label="Disable Toast Notifications",
        category="Windows 11",
        apply_fn=apply_disable_notifications,
        remove_fn=remove_disable_notifications,
        detect_fn=detect_disable_notifications,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_KEY, _TOAST_KEY],
        description="Disables all toast/push notifications system-wide.",
        tags=["win11", "notifications", "focus"],
    ),
    TweakDef(
        id="disable-snap-flyout",
        label="Disable Snap Layout Flyout",
        category="Windows 11",
        apply_fn=apply_disable_snap_flyout,
        remove_fn=remove_disable_snap_flyout,
        detect_fn=detect_disable_snap_flyout,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SNAP_FLYOUT_KEY],
        description="Disables the snap layout flyout shown on maximize button hover.",
        tags=["win11", "snap", "ux"],
    ),
    TweakDef(
        id="disable-taskbar-chat",
        label="Disable Taskbar Chat Icon",
        category="Windows 11",
        apply_fn=apply_disable_chat_icon,
        remove_fn=remove_disable_chat_icon,
        detect_fn=detect_disable_chat_icon,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CHAT_KEY],
        description="Removes the Teams Chat icon from the Windows 11 taskbar.",
        tags=["win11", "taskbar", "teams"],
    ),
]
