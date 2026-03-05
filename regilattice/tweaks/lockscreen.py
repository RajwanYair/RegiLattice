"""Lock Screen & Login tweaks.

Covers lock screen ads, login background, network icon on lock screen,
lock screen camera, fast user switching, lock screen timeout, and
password-less sign-in options.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_PERSONALIZE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"
)
_LOGON = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
)
_LOCK_SCREEN = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"
)
_CONTENT_DELIVERY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)
_EXPLORER_ADV = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)
_WINLOGON = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Winlogon"
)
_LOCK_NET = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
)
_CAMERA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Camera"
)
_FUS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\System"
)
_POWER = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power"
    r"\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51"
)
_LOCK_APP = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\Personalization"
)
_BIOS_PWD = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Winlogon"
)
_SIGNIN_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion"
    r"\Policies\System"
)


# ── Disable Lock Screen Ads (Spotlight Tips) ─────────────────────────────────


def _apply_disable_lock_ads(*, require_admin: bool = False) -> None:
    SESSION.log("LockScreen: disable lock screen tips and ads")
    SESSION.backup([_CONTENT_DELIVERY], "LockAds")
    SESSION.set_dword(_CONTENT_DELIVERY, "RotatingLockScreenOverlayEnabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-338387Enabled", 0)


def _remove_disable_lock_ads(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT_DELIVERY, "RotatingLockScreenOverlayEnabled")
    SESSION.delete_value(_CONTENT_DELIVERY, "SubscribedContent-338387Enabled")


def _detect_disable_lock_ads() -> bool:
    return SESSION.read_dword(_CONTENT_DELIVERY, "RotatingLockScreenOverlayEnabled") == 0


# ── Disable Lock Screen Entirely ─────────────────────────────────────────────


def _apply_disable_lock_screen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: disable the lock screen entirely")
    SESSION.backup([_PERSONALIZE], "LockScreen")
    SESSION.set_dword(_PERSONALIZE, "NoLockScreen", 1)


def _remove_disable_lock_screen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PERSONALIZE, "NoLockScreen")


def _detect_disable_lock_screen() -> bool:
    return SESSION.read_dword(_PERSONALIZE, "NoLockScreen") == 1


# ── Disable Login Background Blur ────────────────────────────────────────────


def _apply_disable_login_blur(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: disable acrylic background blur on sign-in")
    SESSION.backup([_LOGON], "LoginBlur")
    SESSION.set_dword(_LOGON, "DisableAcrylicBackgroundOnLogon", 1)


def _remove_disable_login_blur(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOGON, "DisableAcrylicBackgroundOnLogon")


def _detect_disable_login_blur() -> bool:
    return SESSION.read_dword(_LOGON, "DisableAcrylicBackgroundOnLogon") == 1


# ── Hide Network Icon on Lock Screen ─────────────────────────────────────────


def _apply_hide_lock_network(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: hide network icon on lock screen")
    SESSION.backup([_LOCK_NET], "LockNetwork")
    SESSION.set_dword(_LOCK_NET, "DontDisplayNetworkSelectionUI", 1)


def _remove_hide_lock_network(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOCK_NET, "DontDisplayNetworkSelectionUI")


def _detect_hide_lock_network() -> bool:
    return SESSION.read_dword(_LOCK_NET, "DontDisplayNetworkSelectionUI") == 1


# ── Disable Lock Screen Camera ───────────────────────────────────────────────


def _apply_disable_lock_camera(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: disable camera access on lock screen")
    SESSION.backup([_CAMERA], "LockCamera")
    SESSION.set_dword(_CAMERA, "AllowCamera", 0)


def _remove_disable_lock_camera(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CAMERA, "AllowCamera")


def _detect_disable_lock_camera() -> bool:
    return SESSION.read_dword(_CAMERA, "AllowCamera") == 0


# ── Disable Fast User Switching ──────────────────────────────────────────────


def _apply_disable_fast_switch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: disable fast user switching")
    SESSION.backup([_FUS], "FastUserSwitch")
    SESSION.set_dword(_FUS, "HideFastUserSwitching", 1)


def _remove_disable_fast_switch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FUS, "HideFastUserSwitching")


def _detect_disable_fast_switch() -> bool:
    return SESSION.read_dword(_FUS, "HideFastUserSwitching") == 1


# ── Auto-Lock After Inactivity (10 min) ─────────────────────────────────────


def _apply_auto_lock_timeout(*, require_admin: bool = False) -> None:
    SESSION.log("LockScreen: set auto-lock after 600 seconds (10 min)")
    _screensaver = r"HKEY_CURRENT_USER\Control Panel\Desktop"
    SESSION.backup([_screensaver], "AutoLock")
    SESSION.set_string(_screensaver, "ScreenSaveTimeOut", "600")
    SESSION.set_string(_screensaver, "ScreenSaverIsSecure", "1")


def _remove_auto_lock_timeout(*, require_admin: bool = False) -> None:
    _screensaver = r"HKEY_CURRENT_USER\Control Panel\Desktop"
    SESSION.set_string(_screensaver, "ScreenSaveTimeOut", "0")
    SESSION.set_string(_screensaver, "ScreenSaverIsSecure", "0")


def _detect_auto_lock_timeout() -> bool:
    _screensaver = r"HKEY_CURRENT_USER\Control Panel\Desktop"
    val = SESSION.read_string(_screensaver, "ScreenSaveTimeOut")
    return val == "600"


# ── Disable First Sign-In Animation ─────────────────────────────────────────


def _apply_disable_first_login(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: disable first sign-in animation")
    SESSION.backup([_LOGON], "FirstLogin")
    SESSION.set_dword(_LOGON, "EnableFirstLogonAnimation", 0)


def _remove_disable_first_login(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOGON, "EnableFirstLogonAnimation")


def _detect_disable_first_login() -> bool:
    return SESSION.read_dword(_LOGON, "EnableFirstLogonAnimation") == 0


# ── Disable Last User Name Display ──────────────────────────────────────────


def _apply_hide_last_user(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: hide last logged-in username on login screen")
    SESSION.backup([_SIGNIN_POLICY], "HideLastUser")
    SESSION.set_dword(_SIGNIN_POLICY, "DontDisplayLastUserName", 1)


def _remove_hide_last_user(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SIGNIN_POLICY, "DontDisplayLastUserName", 0)


def _detect_hide_last_user() -> bool:
    return SESSION.read_dword(_SIGNIN_POLICY, "DontDisplayLastUserName") == 1


# ── Set Auto-Logon After Restart ─────────────────────────────────────────────


def _apply_auto_restart_logon(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: enable automatic restart sign-on (ARSO)")
    SESSION.backup([_WINLOGON], "ARSO")
    SESSION.set_dword(_WINLOGON, "ARSOUserConsent", 1)


def _remove_auto_restart_logon(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WINLOGON, "ARSOUserConsent")


def _detect_auto_restart_logon() -> bool:
    return SESSION.read_dword(_WINLOGON, "ARSOUserConsent") == 1


# ── Verbose Login Status Messages ────────────────────────────────────────────


def _apply_verbose_login(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("LockScreen: enable verbose logon status messages")
    SESSION.backup([_LOGON], "VerboseLogon")
    SESSION.set_dword(_LOGON, "VerboseStatus", 1)


def _remove_verbose_login(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOGON, "VerboseStatus")


def _detect_verbose_login() -> bool:
    return SESSION.read_dword(_LOGON, "VerboseStatus") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="lock-disable-ads",
        label="Disable Lock Screen Ads & Tips",
        category="Lock Screen & Login",
        apply_fn=_apply_disable_lock_ads,
        remove_fn=_remove_disable_lock_ads,
        detect_fn=_detect_disable_lock_ads,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=(
            "Disables Spotlight tips and rotating overlay ads on the "
            "lock screen. Default: enabled. Recommended: disabled."
        ),
        tags=["lockscreen", "ads", "spotlight", "tips"],
    ),
    TweakDef(
        id="lock-disable-lock-screen",
        label="Disable Lock Screen Entirely",
        category="Lock Screen & Login",
        apply_fn=_apply_disable_lock_screen,
        remove_fn=_remove_disable_lock_screen,
        detect_fn=_detect_disable_lock_screen,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PERSONALIZE],
        description=(
            "Completely disables the lock screen, going straight to "
            "the password/PIN prompt. "
            "Default: enabled. Recommended: disabled (home PCs)."
        ),
        tags=["lockscreen", "disable", "bypass", "login"],
    ),
    TweakDef(
        id="lock-disable-login-blur",
        label="Disable Login Background Blur",
        category="Lock Screen & Login",
        apply_fn=_apply_disable_login_blur,
        remove_fn=_remove_disable_login_blur,
        detect_fn=_detect_disable_login_blur,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOGON],
        description=(
            "Disables the acrylic (frosted glass) blur effect on the "
            "sign-in screen background. Shows the full wallpaper. "
            "Default: blurred. Recommended: disabled."
        ),
        tags=["lockscreen", "login", "blur", "acrylic", "background"],
    ),
    TweakDef(
        id="lock-hide-network-icon",
        label="Hide Network Icon on Lock Screen",
        category="Lock Screen & Login",
        apply_fn=_apply_hide_lock_network,
        remove_fn=_remove_hide_lock_network,
        detect_fn=_detect_hide_lock_network,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOCK_NET],
        description=(
            "Hides the network selection UI on the lock screen. "
            "Prevents unauthorized Wi-Fi changes. "
            "Default: shown. Recommended: hidden."
        ),
        tags=["lockscreen", "network", "wifi", "security"],
    ),
    TweakDef(
        id="lock-disable-camera",
        label="Disable Lock Screen Camera Access",
        category="Lock Screen & Login",
        apply_fn=_apply_disable_lock_camera,
        remove_fn=_remove_disable_lock_camera,
        detect_fn=_detect_disable_lock_camera,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CAMERA],
        description=(
            "Disables camera access from the lock screen. "
            "Default: allowed. Recommended: 0 (disabled)."
        ),
        tags=["lockscreen", "camera", "privacy", "security"],
    ),
    TweakDef(
        id="lock-disable-fast-user-switching",
        label="Disable Fast User Switching",
        category="Lock Screen & Login",
        apply_fn=_apply_disable_fast_switch,
        remove_fn=_remove_disable_fast_switch,
        detect_fn=_detect_disable_fast_switch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FUS],
        description=(
            "Hides the Fast User Switching button on the login screen. "
            "Useful for single-user or kiosk systems. "
            "Default: shown. Recommended: hidden (single user)."
        ),
        tags=["lockscreen", "user", "switching", "login"],
    ),
    TweakDef(
        id="lock-auto-lock-10min",
        label="Auto-Lock After 10 Minutes",
        category="Lock Screen & Login",
        apply_fn=_apply_auto_lock_timeout,
        remove_fn=_remove_auto_lock_timeout,
        detect_fn=_detect_auto_lock_timeout,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[r"HKEY_CURRENT_USER\Control Panel\Desktop"],
        description=(
            "Sets screen saver timeout to 600 seconds with secure "
            "lock enabled. Ensures idle lock for security. "
            "Default: disabled. Recommended: 600s."
        ),
        tags=["lockscreen", "timeout", "security", "idle"],
    ),
    TweakDef(
        id="lock-disable-first-login-animation",
        label="Disable First Sign-In Animation",
        category="Lock Screen & Login",
        apply_fn=_apply_disable_first_login,
        remove_fn=_remove_disable_first_login,
        detect_fn=_detect_disable_first_login,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOGON],
        description=(
            "Disables the 'Hi / We're getting things ready' animation "
            "on first login. Speeds up new profile setup. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["lockscreen", "animation", "login", "first-run"],
    ),
    TweakDef(
        id="lock-hide-last-username",
        label="Hide Last Logged-In Username",
        category="Lock Screen & Login",
        apply_fn=_apply_hide_last_user,
        remove_fn=_remove_hide_last_user,
        detect_fn=_detect_hide_last_user,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SIGNIN_POLICY],
        description=(
            "Hides the last logged-in username on the login screen. "
            "Users must type their username manually. "
            "Default: 0 (show). Recommended: 1 (hide) for security."
        ),
        tags=["lockscreen", "username", "security", "login"],
    ),
    TweakDef(
        id="lock-auto-restart-signon",
        label="Enable Auto Restart Sign-On (ARSO)",
        category="Lock Screen & Login",
        apply_fn=_apply_auto_restart_logon,
        remove_fn=_remove_auto_restart_logon,
        detect_fn=_detect_auto_restart_logon,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WINLOGON],
        description=(
            "Enables automatic sign-on after Windows Update restarts. "
            "Re-opens your apps after reboot. "
            "Default: not configured. Recommended: 1 (enabled)."
        ),
        tags=["lockscreen", "arso", "restart", "autologon"],
    ),
    TweakDef(
        id="lock-verbose-login-messages",
        label="Enable Verbose Logon Status Messages",
        category="Lock Screen & Login",
        apply_fn=_apply_verbose_login,
        remove_fn=_remove_verbose_login,
        detect_fn=_detect_verbose_login,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOGON],
        description=(
            "Shows detailed status messages during logon/logoff "
            "instead of generic 'Please wait'. "
            "Default: not set. Recommended: 1 (verbose)."
        ),
        tags=["lockscreen", "verbose", "status", "login", "debug"],
    ),
]


# -- 12. Disable Lock Screen Ads/Tips ────────────────────────────────────────

_SYS_LOGON_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"


def _apply_no_lock_ads(*, require_admin: bool = False) -> None:
    SESSION.backup([_CONTENT_DELIVERY], "LockScreenAds")
    SESSION.set_dword(_CONTENT_DELIVERY, "RotatingLockScreenEnabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "RotatingLockScreenOverlayEnabled", 0)


def _remove_no_lock_ads(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONTENT_DELIVERY, "RotatingLockScreenEnabled", 1)
    SESSION.set_dword(_CONTENT_DELIVERY, "RotatingLockScreenOverlayEnabled", 1)


def _detect_no_lock_ads() -> bool:
    return (
        SESSION.read_dword(_CONTENT_DELIVERY, "RotatingLockScreenEnabled") == 0
        and SESSION.read_dword(_CONTENT_DELIVERY, "RotatingLockScreenOverlayEnabled") == 0
    )


# -- 13. Disable First Sign-In Animation ─────────────────────────────────────


def _apply_no_signin_anim(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SYS_LOGON_POLICY], "SignInAnimation")
    SESSION.set_dword(_SYS_LOGON_POLICY, "EnableFirstLogonAnimation", 0)


def _remove_no_signin_anim(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SYS_LOGON_POLICY, "EnableFirstLogonAnimation", 1)


def _detect_no_signin_anim() -> bool:
    return SESSION.read_dword(_SYS_LOGON_POLICY, "EnableFirstLogonAnimation") == 0


TWEAKS += [
    TweakDef(
        id="lock-disable-lock-screen-ads",
        label="Disable Lock Screen Ads/Tips",
        category="Lock Screen & Login",
        apply_fn=_apply_no_lock_ads,
        remove_fn=_remove_no_lock_ads,
        detect_fn=_detect_no_lock_ads,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description="Disables rotating lock screen tips and advertising overlays. Default: Enabled. Recommended: Disabled.",
        tags=["lockscreen", "ads", "tips", "spotlight"],
    ),
    TweakDef(
        id="lock-disable-sign-in-animation",
        label="Disable First Sign-In Animation",
        category="Lock Screen & Login",
        apply_fn=_apply_no_signin_anim,
        remove_fn=_remove_no_signin_anim,
        detect_fn=_detect_no_signin_anim,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SYS_LOGON_POLICY],
        description="Disables the first sign-in animation after new user setup. Speeds up login. Default: Enabled. Recommended: Disabled.",
        tags=["lockscreen", "animation", "first-logon", "login"],
    ),
]
