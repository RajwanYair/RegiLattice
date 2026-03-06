"""Notifications tweaks -- Action Center, toasts, sounds, suggestions, quiet hours."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# -- Action Center / Explorer Policy ─────────────────────────────────────────

_EXPLORER_POLICY = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"

# -- Push Notifications ──────────────────────────────────────────────────────

_PUSH_KEY = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"

# -- Notification Settings ───────────────────────────────────────────────────

_NOTIF_SETTINGS = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"

# -- Content Delivery Manager ────────────────────────────────────────────────

_CDM = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"

# -- User Profile Engagement ─────────────────────────────────────────────────

_PROFILE_ENGAGE = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"

# -- Background Access Toast ─────────────────────────────────────────────────

_BG_ACCESS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion"
    r"\Notifications\Settings\Windows.SystemToast.BackgroundAccess"
)

# -- AutoPlay Handlers ───────────────────────────────────────────────────────

_AUTOPLAY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"


# ── 1. Disable Action Center ────────────────────────────────────────────────


def _apply_disable_action_center(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_EXPLORER_POLICY], "ActionCenter")
    SESSION.set_dword(_EXPLORER_POLICY, "DisableNotificationCenter", 1)
    SESSION.log("Notifications: disabled Action Center")


def _remove_disable_action_center(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_EXPLORER_POLICY], "ActionCenter_Remove")
    SESSION.delete_value(_EXPLORER_POLICY, "DisableNotificationCenter")
    SESSION.log("Notifications: re-enabled Action Center")


def _detect_disable_action_center() -> bool:
    return SESSION.read_dword(_EXPLORER_POLICY, "DisableNotificationCenter") == 1


# ── 2. Disable Toast Notifications ──────────────────────────────────────────


def _apply_disable_toast(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_PUSH_KEY], "ToastNotifications")
    SESSION.set_dword(_PUSH_KEY, "NoToastApplicationNotification", 1)
    SESSION.log("Notifications: disabled toast notifications")


def _remove_disable_toast(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_PUSH_KEY], "ToastNotifications_Remove")
    SESSION.delete_value(_PUSH_KEY, "NoToastApplicationNotification")
    SESSION.log("Notifications: re-enabled toast notifications")


def _detect_disable_toast() -> bool:
    return SESSION.read_dword(_PUSH_KEY, "NoToastApplicationNotification") == 1


# ── 3. Disable Lock Screen Notifications ────────────────────────────────────


def _apply_disable_lock_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NOTIF_SETTINGS], "LockScreenNotif")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK", 0)
    SESSION.log("Notifications: disabled lock screen notifications")


def _remove_disable_lock_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NOTIF_SETTINGS], "LockScreenNotif_Remove")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK", 1)
    SESSION.log("Notifications: re-enabled lock screen notifications")


def _detect_disable_lock_screen() -> bool:
    return SESSION.read_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK") == 0


# ── 4. Disable Notification Sounds ──────────────────────────────────────────


def _apply_disable_sounds(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NOTIF_SETTINGS], "NotifSounds")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 0)
    SESSION.log("Notifications: disabled notification sounds")


def _remove_disable_sounds(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NOTIF_SETTINGS], "NotifSounds_Remove")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 1)
    SESSION.log("Notifications: re-enabled notification sounds")


def _detect_disable_sounds() -> bool:
    return SESSION.read_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND") == 0


# ── 5. Disable Windows Suggestions / Tips ───────────────────────────────────


def _apply_disable_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "Suggestions")
    SESSION.set_dword(_CDM, "SubscribedContent-338389Enabled", 0)
    SESSION.log("Notifications: disabled Windows suggestions/tips")


def _remove_disable_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "Suggestions_Remove")
    SESSION.set_dword(_CDM, "SubscribedContent-338389Enabled", 1)
    SESSION.log("Notifications: re-enabled Windows suggestions/tips")


def _detect_disable_suggestions() -> bool:
    return SESSION.read_dword(_CDM, "SubscribedContent-338389Enabled") == 0


# ── 6. Disable Windows Welcome Experience ───────────────────────────────────


def _apply_disable_welcome(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "WelcomeExperience")
    SESSION.set_dword(_CDM, "SubscribedContent-310093Enabled", 0)
    SESSION.log("Notifications: disabled Windows welcome experience")


def _remove_disable_welcome(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "WelcomeExperience_Remove")
    SESSION.set_dword(_CDM, "SubscribedContent-310093Enabled", 1)
    SESSION.log("Notifications: re-enabled Windows welcome experience")


def _detect_disable_welcome() -> bool:
    return SESSION.read_dword(_CDM, "SubscribedContent-310093Enabled") == 0


# ── 7. Disable "Finish Setting Up" Reminders ────────────────────────────────


def _apply_disable_finish_setup(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_PROFILE_ENGAGE], "FinishSetup")
    SESSION.set_dword(_PROFILE_ENGAGE, "ScoobeSystemSettingEnabled", 0)
    SESSION.log("Notifications: disabled finish-setting-up reminders")


def _remove_disable_finish_setup(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_PROFILE_ENGAGE], "FinishSetup_Remove")
    SESSION.set_dword(_PROFILE_ENGAGE, "ScoobeSystemSettingEnabled", 1)
    SESSION.log("Notifications: re-enabled finish-setting-up reminders")


def _detect_disable_finish_setup() -> bool:
    return SESSION.read_dword(_PROFILE_ENGAGE, "ScoobeSystemSettingEnabled") == 0


# ── 8. Disable Suggested Apps in Start ──────────────────────────────────────


def _apply_disable_app_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "AppSuggestions")
    SESSION.set_dword(_CDM, "SubscribedContent-338388Enabled", 0)
    SESSION.log("Notifications: disabled suggested apps in Start menu")


def _remove_disable_app_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "AppSuggestions_Remove")
    SESSION.set_dword(_CDM, "SubscribedContent-338388Enabled", 1)
    SESSION.log("Notifications: re-enabled suggested apps in Start menu")


def _detect_disable_app_suggestions() -> bool:
    return SESSION.read_dword(_CDM, "SubscribedContent-338388Enabled") == 0


# ── 9. Auto-enable Quiet Hours ──────────────────────────────────────────────


def _apply_quiet_hours(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NOTIF_SETTINGS], "QuietHours")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_TOASTS_ENABLED", 0)
    SESSION.log("Notifications: enabled quiet hours (toasts suppressed)")


def _remove_quiet_hours(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NOTIF_SETTINGS], "QuietHours_Remove")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_TOASTS_ENABLED", 1)
    SESSION.log("Notifications: disabled quiet hours (toasts restored)")


def _detect_quiet_hours() -> bool:
    return SESSION.read_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_TOASTS_ENABLED") == 0


# ── 10. Disable Background-App Notifications ────────────────────────────────


def _apply_disable_startup_app_notif(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_BG_ACCESS], "BackgroundAccessNotif")
    SESSION.set_dword(_BG_ACCESS, "Enabled", 0)
    SESSION.log("Notifications: disabled background-app notifications")


def _remove_disable_startup_app_notif(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_BG_ACCESS], "BackgroundAccessNotif_Remove")
    SESSION.delete_value(_BG_ACCESS, "Enabled")
    SESSION.log("Notifications: re-enabled background-app notifications")


def _detect_disable_startup_app_notif() -> bool:
    return SESSION.read_dword(_BG_ACCESS, "Enabled") == 0


# ── 11. Disable AutoPlay Notifications ──────────────────────────────────────


def _apply_disable_autoplay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_AUTOPLAY], "AutoPlayNotif")
    SESSION.set_dword(_AUTOPLAY, "DisableAutoplay", 1)
    SESSION.log("Notifications: disabled AutoPlay notifications")


def _remove_disable_autoplay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_AUTOPLAY], "AutoPlayNotif_Remove")
    SESSION.set_dword(_AUTOPLAY, "DisableAutoplay", 0)
    SESSION.log("Notifications: re-enabled AutoPlay notifications")


def _detect_disable_autoplay() -> bool:
    return SESSION.read_dword(_AUTOPLAY, "DisableAutoplay") == 1


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="notif-disable-action-center",
        label="Disable Action Center",
        category="Notifications",
        apply_fn=_apply_disable_action_center,
        remove_fn=_remove_disable_action_center,
        detect_fn=_detect_disable_action_center,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_POLICY],
        description=("Disables the Windows Action Center sidebar. Default: enabled. Recommended: disabled."),
        tags=["notifications", "action-center", "sidebar"],
    ),
    TweakDef(
        id="notif-disable-toast",
        label="Disable Toast Notifications",
        category="Notifications",
        apply_fn=_apply_disable_toast,
        remove_fn=_remove_disable_toast,
        detect_fn=_detect_disable_toast,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PUSH_KEY],
        description=("Disables pop-up toast notifications from all applications. Default: enabled. Recommended: disabled."),
        tags=["notifications", "toast", "popup"],
    ),
    TweakDef(
        id="notif-disable-lock-screen",
        label="Disable Lock Screen Notifications",
        category="Notifications",
        apply_fn=_apply_disable_lock_screen,
        remove_fn=_remove_disable_lock_screen,
        detect_fn=_detect_disable_lock_screen,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_SETTINGS],
        description=("Prevents notifications from appearing on the lock screen. Default: enabled. Recommended: disabled."),
        tags=["notifications", "lock-screen", "privacy"],
    ),
    TweakDef(
        id="notif-disable-sounds",
        label="Disable Notification Sounds",
        category="Notifications",
        apply_fn=_apply_disable_sounds,
        remove_fn=_remove_disable_sounds,
        detect_fn=_detect_disable_sounds,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_SETTINGS],
        description=("Silences all notification sounds system-wide. Default: enabled. Recommended: disabled."),
        tags=["notifications", "sounds", "audio"],
    ),
    TweakDef(
        id="notif-disable-suggestions",
        label="Disable Windows Suggestions / Tips",
        category="Notifications",
        apply_fn=_apply_disable_suggestions,
        remove_fn=_remove_disable_suggestions,
        detect_fn=_detect_disable_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description=("Stops Windows from showing tips, tricks, and suggestion notifications. Default: enabled. Recommended: disabled."),
        tags=["notifications", "suggestions", "tips"],
    ),
    TweakDef(
        id="notif-disable-welcome",
        label="Disable Windows Welcome Experience",
        category="Notifications",
        apply_fn=_apply_disable_welcome,
        remove_fn=_remove_disable_welcome,
        detect_fn=_detect_disable_welcome,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description=("Disables the Windows welcome experience shown after updates. Default: enabled. Recommended: disabled."),
        tags=["notifications", "welcome", "updates"],
    ),
    TweakDef(
        id="notif-disable-finish-setup",
        label="Disable Finish Setting Up Reminders",
        category="Notifications",
        apply_fn=_apply_disable_finish_setup,
        remove_fn=_remove_disable_finish_setup,
        detect_fn=_detect_disable_finish_setup,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PROFILE_ENGAGE],
        description=("Suppresses the recurring 'finish setting up your device' reminders. Default: enabled. Recommended: disabled."),
        tags=["notifications", "setup", "reminders"],
    ),
    TweakDef(
        id="notif-disable-app-suggestions",
        label="Disable Suggested Apps in Start",
        category="Notifications",
        apply_fn=_apply_disable_app_suggestions,
        remove_fn=_remove_disable_app_suggestions,
        detect_fn=_detect_disable_app_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description=("Prevents Windows from suggesting apps in the Start menu. Default: enabled. Recommended: disabled."),
        tags=["notifications", "suggestions", "start-menu"],
    ),
    TweakDef(
        id="notif-quiet-hours-auto",
        label="Auto-enable Quiet Hours",
        category="Notifications",
        apply_fn=_apply_quiet_hours,
        remove_fn=_remove_quiet_hours,
        detect_fn=_detect_quiet_hours,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_SETTINGS],
        description=("Enables quiet hours (focus assist) to suppress all toast notifications. Default: disabled. Recommended: enabled."),
        tags=["notifications", "quiet-hours", "focus-assist"],
    ),
    TweakDef(
        id="notif-disable-startup-app-notif",
        label="Disable Background App Notifications",
        category="Notifications",
        apply_fn=_apply_disable_startup_app_notif,
        remove_fn=_remove_disable_startup_app_notif,
        detect_fn=_detect_disable_startup_app_notif,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_BG_ACCESS],
        description=("Disables 'apps are running in the background' system toast notifications. Default: enabled. Recommended: disabled."),
        tags=["notifications", "background", "startup"],
    ),
    TweakDef(
        id="notif-disable-autoplay",
        label="Disable AutoPlay Notifications",
        category="Notifications",
        apply_fn=_apply_disable_autoplay,
        remove_fn=_remove_disable_autoplay,
        detect_fn=_detect_disable_autoplay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_AUTOPLAY],
        description=("Disables AutoPlay notifications when removable media is inserted. Default: enabled. Recommended: disabled."),
        tags=["notifications", "autoplay", "media"],
    ),
]


# -- 12. Disable Taskbar Badge Notifications ─────────────────────────────────

_NOTIF_EXPLORER_ADV = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"


def _apply_disable_badge_icons(*, require_admin: bool = False) -> None:
    SESSION.backup([_NOTIF_EXPLORER_ADV], "BadgeIcons")
    SESSION.set_dword(_NOTIF_EXPLORER_ADV, "TaskbarBadges", 0)


def _remove_disable_badge_icons(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NOTIF_EXPLORER_ADV, "TaskbarBadges", 1)


def _detect_disable_badge_icons() -> bool:
    return SESSION.read_dword(_NOTIF_EXPLORER_ADV, "TaskbarBadges") == 0


# -- 13. Disable Push Toast Notifications ────────────────────────────────────

_PUSH_TOAST = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications"


def _apply_push_toast_off(*, require_admin: bool = False) -> None:
    SESSION.backup([_PUSH_TOAST], "PushToastOff")
    SESSION.set_dword(_PUSH_TOAST, "ToastEnabled", 0)


def _remove_push_toast_off(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PUSH_TOAST, "ToastEnabled", 1)


def _detect_push_toast_off() -> bool:
    return SESSION.read_dword(_PUSH_TOAST, "ToastEnabled") == 0


TWEAKS += [
    TweakDef(
        id="notif-disable-badge-icons",
        label="Disable Taskbar Badge Notifications",
        category="Notifications",
        apply_fn=_apply_disable_badge_icons,
        remove_fn=_remove_disable_badge_icons,
        detect_fn=_detect_disable_badge_icons,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_EXPLORER_ADV],
        description="Disables notification badge counters on taskbar app icons. Default: Enabled. Recommended: Disabled.",
        tags=["notifications", "badge", "taskbar", "icons"],
    ),
    TweakDef(
        id="notif-quiet-hours",
        label="Disable Push Toast Notifications",
        category="Notifications",
        apply_fn=_apply_push_toast_off,
        remove_fn=_remove_push_toast_off,
        detect_fn=_detect_push_toast_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PUSH_TOAST],
        description="Disables all push toast notifications globally. Default: Enabled. Recommended: Disabled for focus.",
        tags=["notifications", "toast", "push", "quiet-hours"],
    ),
]


# -- 14. Disable Notification Sounds ─────────────────────────────────────────


def _apply_disable_notif_sounds(*, require_admin: bool = False) -> None:
    SESSION.backup([_NOTIF_SETTINGS], "NotifSounds")
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 0)


def _remove_disable_notif_sounds(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 1)


def _detect_disable_notif_sounds() -> bool:
    return SESSION.read_dword(_NOTIF_SETTINGS, "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND") == 0


# -- 15. Set Notification Display Time to 3 Seconds ──────────────────────────

_ACCESSIBILITY_KEY = r"HKEY_CURRENT_USER\Control Panel\Accessibility"


def _apply_notif_display_time_3s(*, require_admin: bool = False) -> None:
    SESSION.backup([_ACCESSIBILITY_KEY], "NotifDisplayTime")
    SESSION.set_dword(_ACCESSIBILITY_KEY, "MessageDuration", 3)


def _remove_notif_display_time_3s(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_ACCESSIBILITY_KEY, "MessageDuration", 5)


def _detect_notif_display_time_3s() -> bool:
    return SESSION.read_dword(_ACCESSIBILITY_KEY, "MessageDuration") == 3


# -- 16. Disable Notification Banners ────────────────────────────────────────

_NOTIF_BANNER_POLICY = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"


def _apply_disable_notif_banners(*, require_admin: bool = False) -> None:
    SESSION.backup([_NOTIF_BANNER_POLICY], "NotifBanners")
    SESSION.set_dword(_NOTIF_BANNER_POLICY, "NoToastApplicationNotification", 1)


def _remove_disable_notif_banners(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_NOTIF_BANNER_POLICY, "NoToastApplicationNotification")


def _detect_disable_notif_banners() -> bool:
    return SESSION.read_dword(_NOTIF_BANNER_POLICY, "NoToastApplicationNotification") == 1


TWEAKS += [
    TweakDef(
        id="notif-silence-global-sounds",
        label="Silence Global Notification Sounds",
        category="Notifications",
        apply_fn=_apply_disable_notif_sounds,
        remove_fn=_remove_disable_notif_sounds,
        detect_fn=_detect_disable_notif_sounds,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_SETTINGS],
        description="Disables notification sounds via the global NOC sound setting. Default: Enabled. Recommended: Disabled.",
        tags=["notifications", "sounds", "audio", "focus", "global"],
    ),
    TweakDef(
        id="notif-display-time-3s",
        label="Set Notification Display Time to 3 Seconds",
        category="Notifications",
        apply_fn=_apply_notif_display_time_3s,
        remove_fn=_remove_notif_display_time_3s,
        detect_fn=_detect_notif_display_time_3s,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ACCESSIBILITY_KEY],
        description=(
            "Sets notification display duration to 3 seconds instead of the default 5. Reduces visual distraction. Default: 5s. Recommended: 3s."
        ),
        tags=["notifications", "display-time", "duration", "accessibility"],
    ),
    TweakDef(
        id="notif-disable-banners",
        label="Disable Notification Banners",
        category="Notifications",
        apply_fn=_apply_disable_notif_banners,
        remove_fn=_remove_disable_notif_banners,
        detect_fn=_detect_disable_notif_banners,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NOTIF_BANNER_POLICY],
        description=(
            "Disables toast notification banners (pop-ups) via policy. "
            "Notifications still appear in Action Center. "
            "Default: Enabled. Recommended: Disabled for focus."
        ),
        tags=["notifications", "banners", "toast", "policy"],
    ),
]
