"""Startup program management registry tweaks.

Covers: suppressing common auto-start entries, controlling
startup delay, and managing Run/RunOnce keys.
"""

from __future__ import annotations

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
_BOOT_ANIMATION = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Authentication\LogonUI\BootAnimation"
)
_LOGON_SYSTEM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
_PERSONALIZATION = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"
_POLICIES_SYSTEM = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\System"
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
    return SESSION.read_string(_RUN_CU, "Skype") is None and SESSION.read_string(_RUN_CU, "Skype for Desktop") is None


# ── Disable Edge Auto-Start ─────────────────────────────────────────────────

_EDGE_STARTUP = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"


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


# ── Disable Teams Auto-Start ───────────────────────────────────────────────


def _apply_disable_teams(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Microsoft Teams auto-start")
    SESSION.backup([_RUN_CU], "TeamsAutoStart")
    SESSION.delete_value(_RUN_CU, "com.squirrel.Teams.Teams")
    SESSION.delete_value(_RUN_CU, "MicrosoftTeams")


def _remove_disable_teams(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: Teams auto-start removal is a no-op")


def _detect_disable_teams() -> bool:
    return SESSION.read_string(_RUN_CU, "com.squirrel.Teams.Teams") is None and SESSION.read_string(_RUN_CU, "MicrosoftTeams") is None


# ── Disable Cortana Startup ────────────────────────────────────────────────

_CORTANA_STARTUP = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\StartupApproved\Run"
)


def _apply_disable_cortana_startup(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Cortana startup task")
    SESSION.backup([_RUN_CU, _CORTANA_STARTUP], "CortanaStartup")
    SESSION.delete_value(_RUN_CU, "CortanaUI")
    SESSION.delete_value(_RUN_CU, "Cortana")


def _remove_disable_cortana_startup(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: Cortana startup removal is a no-op")


def _detect_disable_cortana_startup() -> bool:
    return SESSION.read_string(_RUN_CU, "CortanaUI") is None and SESSION.read_string(_RUN_CU, "Cortana") is None


# ── Disable Windows Startup Sound ───────────────────────────────────────────


def _apply_disable_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Windows startup sound")
    SESSION.backup([_BOOT_ANIMATION], "StartupSound")
    SESSION.set_dword(_BOOT_ANIMATION, "DisableStartupSound", 1)


def _remove_disable_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BOOT_ANIMATION, "DisableStartupSound", 0)


def _detect_disable_startup_sound() -> bool:
    return SESSION.read_dword(_BOOT_ANIMATION, "DisableStartupSound") == 1


# ── Use Solid Color Login Background ────────────────────────────────────────


def _apply_disable_login_background(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: use solid color login background")
    SESSION.backup([_LOGON_SYSTEM], "LoginBackground")
    SESSION.set_dword(_LOGON_SYSTEM, "DisableLogonBackgroundImage", 1)


def _remove_disable_login_background(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOGON_SYSTEM, "DisableLogonBackgroundImage")


def _detect_disable_login_background() -> bool:
    return SESSION.read_dword(_LOGON_SYSTEM, "DisableLogonBackgroundImage") == 1


# ── Skip Lock Screen ────────────────────────────────────────────────────────


def _apply_disable_lock_screen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: skip lock screen (go straight to login)")
    SESSION.backup([_PERSONALIZATION], "LockScreen")
    SESSION.set_dword(_PERSONALIZATION, "NoLockScreen", 1)


def _remove_disable_lock_screen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PERSONALIZATION, "NoLockScreen")


def _detect_disable_lock_screen() -> bool:
    return SESSION.read_dword(_PERSONALIZATION, "NoLockScreen") == 1


# ── Disable First Login Animation ───────────────────────────────────────────


def _apply_disable_first_logon_animation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable first login animation")
    SESSION.backup([_POLICIES_SYSTEM], "FirstLogonAnimation")
    SESSION.set_dword(_POLICIES_SYSTEM, "EnableFirstLogonAnimation", 0)


def _remove_disable_first_logon_animation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POLICIES_SYSTEM, "EnableFirstLogonAnimation", 1)


def _detect_disable_first_logon_animation() -> bool:
    return SESSION.read_dword(_POLICIES_SYSTEM, "EnableFirstLogonAnimation") == 0


# ── Disable Startup Delay (Zero) ────────────────────────────────────────────


def _apply_startup_no_delay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: remove startup delay for Startup folder apps")
    SESSION.backup([_STARTUP_DELAY], "StartupNoDelay")
    SESSION.set_dword(_STARTUP_DELAY, "StartupDelayInMSec", 0)


def _remove_startup_no_delay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STARTUP_DELAY, "StartupDelayInMSec")


def _detect_startup_no_delay() -> bool:
    return SESSION.read_dword(_STARTUP_DELAY, "StartupDelayInMSec") == 0


# ── Disable Startup App Tracking ────────────────────────────────────────────

_EXPLORER_ADVANCED = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)


def _apply_disable_app_tracking(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable startup app tracking")
    SESSION.backup([_EXPLORER_ADVANCED], "AppTracking")
    SESSION.set_dword(_EXPLORER_ADVANCED, "Start_TrackProgs", 0)


def _remove_disable_app_tracking(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_EXPLORER_ADVANCED, "Start_TrackProgs", 1)


def _detect_disable_app_tracking() -> bool:
    return SESSION.read_dword(_EXPLORER_ADVANCED, "Start_TrackProgs") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="startup-disable-startup-delay",
        label="Disable Startup Delay",
        category="Startup",
        apply_fn=_apply_disable_startup_delay,
        remove_fn=_remove_disable_startup_delay,
        detect_fn=_detect_disable_startup_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STARTUP_DELAY],
        description=("Removes the artificial startup delay for Run-key programs, allowing them to launch immediately at login."),
        tags=["startup", "performance", "boot"],
    ),
    TweakDef(
        id="startup-disable-skype-autostart",
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
        id="startup-disable-edge-autostart",
        label="Disable Edge Startup Boost & Background",
        category="Startup",
        apply_fn=_apply_disable_edge_autostart,
        remove_fn=_remove_disable_edge_autostart,
        detect_fn=_detect_disable_edge_autostart,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_STARTUP],
        description=("Disables Edge's Startup Boost pre-launch and background mode to free memory and reduce startup load."),
        tags=["startup", "edge", "performance"],
    ),
    TweakDef(
        id="startup-disable-store-autoinstall",
        label="Disable Store Suggested App Install",
        category="Startup",
        apply_fn=_apply_disable_store_autoinstall,
        remove_fn=_remove_disable_store_autoinstall,
        detect_fn=_detect_disable_store_autoinstall,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=("Prevents Windows from silently installing suggested apps and OEM bloatware from the Microsoft Store."),
        tags=["startup", "bloatware", "store"],
    ),
    TweakDef(
        id="startup-disable-teams",
        label="Disable Teams Auto-Start",
        category="Startup",
        apply_fn=_apply_disable_teams,
        remove_fn=_remove_disable_teams,
        detect_fn=_detect_disable_teams,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Removes Microsoft Teams from the HKCU Run key to prevent auto-start.",
        tags=["startup", "teams", "performance"],
    ),
    TweakDef(
        id="startup-disable-cortana-startup",
        label="Disable Cortana Startup",
        category="Startup",
        apply_fn=_apply_disable_cortana_startup,
        remove_fn=_remove_disable_cortana_startup,
        detect_fn=_detect_disable_cortana_startup,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN_CU],
        description="Removes Cortana from the HKCU Run key to prevent auto-start at login.",
        tags=["startup", "cortana", "performance"],
    ),
    TweakDef(
        id="startup-disable-startup-sound",
        label="Disable Windows Startup Sound",
        category="Startup",
        apply_fn=_apply_disable_startup_sound,
        remove_fn=_remove_disable_startup_sound,
        detect_fn=_detect_disable_startup_sound,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BOOT_ANIMATION],
        description="Silences the Windows startup sound on boot.",
        tags=["startup", "sound", "boot"],
    ),
    TweakDef(
        id="startup-disable-login-background",
        label="Use Solid Color Login Background",
        category="Startup",
        apply_fn=_apply_disable_login_background,
        remove_fn=_remove_disable_login_background,
        detect_fn=_detect_disable_login_background,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOGON_SYSTEM],
        description=("Replaces the Windows Spotlight / hero image on the login screen with a plain solid color background."),
        tags=["startup", "login", "appearance"],
    ),
    TweakDef(
        id="startup-disable-lock-screen",
        label="Skip Lock Screen (Go Straight to Login)",
        category="Startup",
        apply_fn=_apply_disable_lock_screen,
        remove_fn=_remove_disable_lock_screen,
        detect_fn=_detect_disable_lock_screen,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PERSONALIZATION],
        description=("Bypasses the lock screen so the machine goes directly to the password / PIN prompt on wake or boot."),
        tags=["startup", "lockscreen", "login"],
    ),
    TweakDef(
        id="startup-disable-first-logon-animation",
        label="Disable First Login Animation",
        category="Startup",
        apply_fn=_apply_disable_first_logon_animation,
        remove_fn=_remove_disable_first_logon_animation,
        detect_fn=_detect_disable_first_logon_animation,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POLICIES_SYSTEM],
        description=("Disables the 'Hi / We're getting things ready' first-logon animation shown after a new user profile is created."),
        tags=["startup", "animation", "login", "boot"],
    ),
    TweakDef(
        id="startup-disable-delay",
        label="Disable Startup Delay",
        category="Startup",
        apply_fn=_apply_startup_no_delay,
        remove_fn=_remove_startup_no_delay,
        detect_fn=_detect_startup_no_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STARTUP_DELAY],
        description=(
            "Removes the startup delay for programs in the Startup folder. Apps launch immediately "
            "at logon. Default: ~10s delay. Recommended: 0 (no delay)."
        ),
        tags=["startup", "delay", "performance", "boot"],
    ),
    TweakDef(
        id="startup-disable-app-tracking",
        label="Disable Startup App Tracking",
        category="Startup",
        apply_fn=_apply_disable_app_tracking,
        remove_fn=_remove_disable_app_tracking,
        detect_fn=_detect_disable_app_tracking,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_ADVANCED],
        description=(
            "Disables tracking of which programs are launched from Start menu. "
            "Improves privacy and reduces write I/O. Default: Enabled. Recommended: Disabled."
        ),
        tags=["startup", "tracking", "privacy", "performance"],
    ),
]


# -- Disable Last Known Good ---------------------------------------------------

_SESSION_MGR_CM = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Configuration Manager"
)


def _apply_startup_disable_last_known_good(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disabling Last Known Good")
    SESSION.backup([_SESSION_MGR_CM], "LastKnownGood")
    SESSION.set_dword(_SESSION_MGR_CM, "LastKnownGood", 0)
    SESSION.log("Startup: Last Known Good disabled")


def _remove_startup_disable_last_known_good(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SESSION_MGR_CM], "LastKnownGood_Remove")
    SESSION.delete_value(_SESSION_MGR_CM, "LastKnownGood")


def _detect_startup_disable_last_known_good() -> bool:
    return SESSION.read_dword(_SESSION_MGR_CM, "LastKnownGood") == 0


# -- Enable Verbose Boot Messages ----------------------------------------------


def _apply_startup_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: enabling verbose boot messages")
    SESSION.backup([_POLICIES_SYSTEM], "VerboseBoot")
    SESSION.set_dword(_POLICIES_SYSTEM, "VerboseStatus", 1)
    SESSION.log("Startup: verbose boot messages enabled")


def _remove_startup_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_POLICIES_SYSTEM], "VerboseBoot_Remove")
    SESSION.delete_value(_POLICIES_SYSTEM, "VerboseStatus")


def _detect_startup_verbose_boot() -> bool:
    return SESSION.read_dword(_POLICIES_SYSTEM, "VerboseStatus") == 1


TWEAKS += [
    TweakDef(
        id="startup-disable-last-known-good",
        label="Disable Last Known Good Boot Option",
        category="Startup",
        apply_fn=_apply_startup_disable_last_known_good,
        remove_fn=_remove_startup_disable_last_known_good,
        detect_fn=_detect_startup_disable_last_known_good,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SESSION_MGR_CM],
        description=("Disables the Last Known Good Configuration boot option. Default: Enabled. Recommended: Disabled for advanced users."),
        tags=["startup", "boot", "last-known-good"],
    ),
    TweakDef(
        id="startup-verbose-boot",
        label="Enable Verbose Boot Messages",
        category="Startup",
        apply_fn=_apply_startup_verbose_boot,
        remove_fn=_remove_startup_verbose_boot,
        detect_fn=_detect_startup_verbose_boot,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_POLICIES_SYSTEM],
        description=("Shows detailed status messages during boot and shutdown. Default: Disabled. Recommended: Enabled for troubleshooting."),
        tags=["startup", "boot", "verbose", "debug"],
    ),
]


# -- Disable Startup Delay for Apps ---------------------------------------------

_SERIALIZE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Serialize"
)


def _apply_start_disable_delay(*, require_admin: bool = False) -> None:
    SESSION.log("Startup: disable startup delay for desktop apps")
    SESSION.backup([_SERIALIZE], "StartDisableDelay")
    SESSION.set_dword(_SERIALIZE, "StartupDelayInMSec", 0)


def _remove_start_disable_delay(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_SERIALIZE, "StartupDelayInMSec")


def _detect_start_disable_delay() -> bool:
    return SESSION.read_dword(_SERIALIZE, "StartupDelayInMSec") == 0


# -- Set Boot-Up Num Lock to On ------------------------------------------------

_KEYBOARD_DEFAULT = r"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"


def _apply_start_boot_numlock(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: set Num Lock on at boot")
    SESSION.backup([_KEYBOARD_DEFAULT], "BootNumLock")
    SESSION.set_string(_KEYBOARD_DEFAULT, "InitialKeyboardIndicators", "2")


def _remove_start_boot_numlock(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_KEYBOARD_DEFAULT, "InitialKeyboardIndicators", "0")


def _detect_start_boot_numlock() -> bool:
    return SESSION.read_string(_KEYBOARD_DEFAULT, "InitialKeyboardIndicators") == "2"


# -- Disable Windows Tips on Startup --------------------------------------------

_CONTENT_DELIVERY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)


def _apply_start_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.log("Startup: disable Windows tips and suggestions")
    SESSION.backup([_CONTENT_DELIVERY], "StartDisableTips")
    SESSION.set_dword(_CONTENT_DELIVERY, "SoftLandingEnabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-338389Enabled", 0)


def _remove_start_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT_DELIVERY, "SoftLandingEnabled")
    SESSION.delete_value(_CONTENT_DELIVERY, "SubscribedContent-338389Enabled")


def _detect_start_disable_tips() -> bool:
    return SESSION.read_dword(_CONTENT_DELIVERY, "SoftLandingEnabled") == 0


TWEAKS += [
    TweakDef(
        id="startup-start-disable-startup-delay",
        label="Disable Startup Delay for Apps",
        category="Startup",
        apply_fn=_apply_start_disable_delay,
        remove_fn=_remove_start_disable_delay,
        detect_fn=_detect_start_disable_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SERIALIZE],
        description=(
            "Removes the artificial startup delay for desktop applications. Apps launch immediately at logon. Default: ~10s delay. Recommended: 0."
        ),
        tags=["startup", "delay", "performance", "boot"],
    ),
    TweakDef(
        id="startup-start-boot-numlock-on",
        label="Set Boot-Up Num Lock to On",
        category="Startup",
        apply_fn=_apply_start_boot_numlock,
        remove_fn=_remove_start_boot_numlock,
        detect_fn=_detect_start_boot_numlock,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEYBOARD_DEFAULT],
        description=("Enables Num Lock at the Windows login screen by default. Default: Off. Recommended: On for desktop keyboards."),
        tags=["startup", "numlock", "keyboard", "boot"],
    ),
    TweakDef(
        id="startup-start-disable-tips",
        label="Disable Windows Tips on Startup",
        category="Startup",
        apply_fn=_apply_start_disable_tips,
        remove_fn=_remove_start_disable_tips,
        detect_fn=_detect_start_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=(
            "Disables Windows tips, tricks, and suggestions that appear after login. "
            "Reduces startup distractions. Default: Enabled. Recommended: Disabled."
        ),
        tags=["startup", "tips", "suggestions", "notifications"],
    ),
]


# ══ Additional Startup Tweaks ══════════════════════════════════════════

_WINLOGON_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT"
    r"\CurrentVersion\Winlogon"
)


def _apply_start_disable_app_restart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable automatic app restart on login")
    SESSION.backup([_WINLOGON_CU], "StartAppRestart")
    SESSION.set_dword(_WINLOGON_CU, "RestartApps", 0)


def _remove_start_disable_app_restart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WINLOGON_CU, "RestartApps")


def _detect_start_disable_app_restart() -> bool:
    return SESSION.read_dword(_WINLOGON_CU, "RestartApps") == 0


def _apply_start_disable_welcome_experience(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Startup: disable Windows welcome experience")
    SESSION.backup([_CONTENT_DELIVERY], "StartWelcomeExp")
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-310093Enabled", 0)


def _remove_start_disable_welcome_experience(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CONTENT_DELIVERY, "SubscribedContent-310093Enabled")


def _detect_start_disable_welcome_experience() -> bool:
    return SESSION.read_dword(_CONTENT_DELIVERY, "SubscribedContent-310093Enabled") == 0


TWEAKS += [
    TweakDef(
        id="startup-start-disable-app-restart",
        label="Disable Automatic App Restart on Login",
        category="Startup",
        apply_fn=_apply_start_disable_app_restart,
        remove_fn=_remove_start_disable_app_restart,
        detect_fn=_detect_start_disable_app_restart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WINLOGON_CU],
        description=(
            "Prevents Windows from automatically restarting apps that were open before shutdown/restart. Default: Enabled. Recommended: Disabled."
        ),
        tags=["startup", "restart", "apps", "login", "winlogon"],
    ),
    TweakDef(
        id="startup-start-disable-welcome-experience",
        label="Disable Windows Welcome Experience",
        category="Startup",
        apply_fn=_apply_start_disable_welcome_experience,
        remove_fn=_remove_start_disable_welcome_experience,
        detect_fn=_detect_start_disable_welcome_experience,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=(
            "Disables the Windows welcome experience that shows after updates "
            "with feature highlights and suggestions. Default: Enabled. Recommended: Disabled."
        ),
        tags=["startup", "welcome", "experience", "updates", "nag"],
    ),
]

# ── Additional startup tweaks ─────────────────────────────────────────────────

_STARTUP_INK = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"
_STARTUP_GAMEBAR = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR"
_STARTUP_APPHOST = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_startup_disable_ink_workspace(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STARTUP_INK], "StartupInkWorkspace")
    SESSION.set_dword(_STARTUP_INK, "PenWorkspaceButtonDesiredVisibility", 0)


def _remove_startup_disable_ink_workspace(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STARTUP_INK], "StartupInkWorkspace_Remove")
    SESSION.delete_value(_STARTUP_INK, "PenWorkspaceButtonDesiredVisibility")


def _detect_startup_disable_ink_workspace() -> bool:
    return SESSION.read_dword(_STARTUP_INK, "PenWorkspaceButtonDesiredVisibility") == 0


def _apply_startup_disable_gamebar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STARTUP_GAMEBAR], "StartupGameBar")
    SESSION.set_dword(_STARTUP_GAMEBAR, "AppCaptureEnabled", 0)


def _remove_startup_disable_gamebar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STARTUP_GAMEBAR], "StartupGameBar_Remove")
    SESSION.delete_value(_STARTUP_GAMEBAR, "AppCaptureEnabled")


def _detect_startup_disable_gamebar() -> bool:
    return SESSION.read_dword(_STARTUP_GAMEBAR, "AppCaptureEnabled") == 0


def _apply_startup_disable_suggested_apps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STARTUP_APPHOST], "StartupSuggestedApps")
    SESSION.set_dword(_STARTUP_APPHOST, "EnableSmartScreen", 0)
    SESSION.set_dword(_STARTUP_APPHOST, "DisableAppInstalls", 1)


def _remove_startup_disable_suggested_apps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STARTUP_APPHOST], "StartupSuggestedApps_Remove")
    SESSION.delete_value(_STARTUP_APPHOST, "EnableSmartScreen")
    SESSION.delete_value(_STARTUP_APPHOST, "DisableAppInstalls")


def _detect_startup_disable_suggested_apps() -> bool:
    return SESSION.read_dword(_STARTUP_APPHOST, "DisableAppInstalls") == 1


TWEAKS += [
    TweakDef(
        id="startup-disable-ink-workspace",
        label="Hide Windows Ink Workspace Button",
        category="Startup",
        apply_fn=_apply_startup_disable_ink_workspace,
        remove_fn=_remove_startup_disable_ink_workspace,
        detect_fn=_detect_startup_disable_ink_workspace,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STARTUP_INK],
        description=(
            "Hides the Windows Ink Workspace button from the taskbar. Default: hidden on non-pen devices. Recommended: hidden for clean taskbar."
        ),
        tags=["startup", "ink", "pen", "taskbar", "ux"],
    ),
    TweakDef(
        id="startup-disable-gamebar-capture",
        label="Disable Xbox Game Bar App Capture",
        category="Startup",
        apply_fn=_apply_startup_disable_gamebar,
        remove_fn=_remove_startup_disable_gamebar,
        detect_fn=_detect_startup_disable_gamebar,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_STARTUP_GAMEBAR],
        description=(
            "Disables Xbox Game Bar app capture (screen recording background task). "
            "Reduces startup and background overhead for non-gaming machines. "
            "Default: enabled. Recommended: disabled on non-gaming workstations."
        ),
        tags=["startup", "gamebar", "xbox", "capture", "performance"],
    ),
    TweakDef(
        id="startup-disable-suggested-app-installs",
        label="Disable Suggested App Install Prompts at Startup",
        category="Startup",
        apply_fn=_apply_startup_disable_suggested_apps,
        remove_fn=_remove_startup_disable_suggested_apps,
        detect_fn=_detect_startup_disable_suggested_apps,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_STARTUP_APPHOST],
        description=(
            "Disables automatic suggested app installation prompts on startup "
            "via system policy. Prevents Store-pushed app suggestions. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["startup", "apps", "suggestions", "store", "bloatware"],
    ),
]
