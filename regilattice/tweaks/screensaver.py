"""Screensaver & Lock tweaks.

Covers screensaver enable/disable, timeout, password protection,
secure desktop, and screensaver policies.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_SS_CU = r"HKEY_CURRENT_USER\Control Panel\Desktop"
_SS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"
_PERSONALIZE = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"
_SS_POLICY_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Control Panel\Desktop"
_LOCK_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"

# ── 1. Disable screensaver ───────────────────────────────────────────────────


def _apply_disable_screensaver(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveActive", "0")


def _remove_disable_screensaver(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveActive", "1")


def _detect_disable_screensaver() -> bool:
    return SESSION.read_string(_SS_CU, "ScreenSaveActive") == "0"


# ── 2. Set screensaver timeout (5 min) ───────────────────────────────────────


def _apply_ss_timeout_5m(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "300")


def _remove_ss_timeout_5m(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "900")


def _detect_ss_timeout_5m() -> bool:
    return SESSION.read_string(_SS_CU, "ScreenSaveTimeOut") == "300"


# ── 3. Set screensaver timeout (10 min) ──────────────────────────────────────


def _apply_ss_timeout_10m(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "600")


def _remove_ss_timeout_10m(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "900")


def _detect_ss_timeout_10m() -> bool:
    return SESSION.read_string(_SS_CU, "ScreenSaveTimeOut") == "600"


# ── 4. Require password on resume ────────────────────────────────────────────


def _apply_ss_password(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaverIsSecure", "1")


def _remove_ss_password(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaverIsSecure", "0")


def _detect_ss_password() -> bool:
    return SESSION.read_string(_SS_CU, "ScreenSaverIsSecure") == "1"


# ── 5. Set blank screensaver ─────────────────────────────────────────────────


def _apply_blank_screensaver(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "SCRNSAVE.EXE", "C:\\Windows\\System32\\scrnsave.scr")


def _remove_blank_screensaver(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_SS_CU, "SCRNSAVE.EXE")


def _detect_blank_screensaver() -> bool:
    val = SESSION.read_string(_SS_CU, "SCRNSAVE.EXE")
    return val is not None and "scrnsave.scr" in val.lower()


# ── 6. Force screensaver via policy ──────────────────────────────────────────


def _apply_force_ss_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_SS_POLICY, "ScreenSaveActive", "1")
    SESSION.set_string(_SS_POLICY, "ScreenSaveTimeOut", "600")
    SESSION.set_string(_SS_POLICY, "ScreenSaverIsSecure", "1")


def _remove_force_ss_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SS_POLICY, "ScreenSaveActive")
    SESSION.delete_value(_SS_POLICY, "ScreenSaveTimeOut")
    SESSION.delete_value(_SS_POLICY, "ScreenSaverIsSecure")


def _detect_force_ss_policy() -> bool:
    return SESSION.read_string(_SS_POLICY, "ScreenSaveActive") == "1"


# ── 7. Disable screensaver via user policy ───────────────────────────────────


def _apply_disable_ss_user_policy(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_POLICY_CU, "ScreenSaveActive", "0")


def _remove_disable_ss_user_policy(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_SS_POLICY_CU, "ScreenSaveActive")


def _detect_disable_ss_user_policy() -> bool:
    return SESSION.read_string(_SS_POLICY_CU, "ScreenSaveActive") == "0"


# ── 8. Enable secure desktop for UAC ─────────────────────────────────────────


def _apply_secure_desktop(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LOCK_POLICY, "PromptOnSecureDesktop", 1)


def _remove_secure_desktop(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LOCK_POLICY, "PromptOnSecureDesktop", 0)


def _detect_secure_desktop() -> bool:
    return SESSION.read_dword(_LOCK_POLICY, "PromptOnSecureDesktop") == 1


# ── 9. Disable lock screen slideshow ─────────────────────────────────────────

_LOCKSS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"


def _apply_disable_lock_slideshow(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LOCKSS, "NoLockScreenSlideshow", 1)


def _remove_disable_lock_slideshow(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOCKSS, "NoLockScreenSlideshow")


def _detect_disable_lock_slideshow() -> bool:
    return SESSION.read_dword(_LOCKSS, "NoLockScreenSlideshow") == 1


# ── 10. Disable screen saver change ──────────────────────────────────────────

_NO_SS_CHANGE = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System"


def _apply_prevent_ss_change(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NO_SS_CHANGE, "NoDispScrSavPage", 1)


def _remove_prevent_ss_change(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_NO_SS_CHANGE, "NoDispScrSavPage")


def _detect_prevent_ss_change() -> bool:
    return SESSION.read_dword(_NO_SS_CHANGE, "NoDispScrSavPage") == 1


# ── 11. Enable transparency effects ──────────────────────────────────────────


def _apply_enable_transparency(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PERSONALIZE, "EnableTransparency", 1)


def _remove_enable_transparency(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PERSONALIZE, "EnableTransparency", 0)


def _detect_enable_transparency() -> bool:
    return SESSION.read_dword(_PERSONALIZE, "EnableTransparency") == 1


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="ss-disable-screensaver",
        label="Disable Screensaver",
        category="Screensaver & Lock",
        apply_fn=_apply_disable_screensaver,
        remove_fn=_remove_disable_screensaver,
        detect_fn=_detect_disable_screensaver,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description="Disable the screensaver. Default: enabled. Recommended: keep enabled with password.",
        tags=["screensaver", "disable", "screen"],
    ),
    TweakDef(
        id="ss-timeout-5m",
        label="Screensaver Timeout: 5 Minutes",
        category="Screensaver & Lock",
        apply_fn=_apply_ss_timeout_5m,
        remove_fn=_remove_ss_timeout_5m,
        detect_fn=_detect_ss_timeout_5m,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description="Set screensaver to activate after 5 minutes. Default: 15 minutes.",
        tags=["screensaver", "timeout", "5min"],
    ),
    TweakDef(
        id="ss-timeout-10m",
        label="Screensaver Timeout: 10 Minutes",
        category="Screensaver & Lock",
        apply_fn=_apply_ss_timeout_10m,
        remove_fn=_remove_ss_timeout_10m,
        detect_fn=_detect_ss_timeout_10m,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description="Set screensaver to activate after 10 minutes. Default: 15 minutes.",
        tags=["screensaver", "timeout", "10min"],
    ),
    TweakDef(
        id="ss-require-password",
        label="Require Password After Screensaver",
        category="Screensaver & Lock",
        apply_fn=_apply_ss_password,
        remove_fn=_remove_ss_password,
        detect_fn=_detect_ss_password,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description="Require login password when resuming from screensaver. Default: not required. Recommended: enabled.",
        tags=["screensaver", "password", "lock", "security"],
    ),
    TweakDef(
        id="ss-blank-screensaver",
        label="Set Blank (Black) Screensaver",
        category="Screensaver & Lock",
        apply_fn=_apply_blank_screensaver,
        remove_fn=_remove_blank_screensaver,
        detect_fn=_detect_blank_screensaver,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description="Set screensaver to plain black screen. Default: none. Recommended: blank for OLED.",
        tags=["screensaver", "blank", "black", "oled"],
    ),
    TweakDef(
        id="ss-force-policy",
        label="Force Screensaver (Policy, 10 min)",
        category="Screensaver & Lock",
        apply_fn=_apply_force_ss_policy,
        remove_fn=_remove_force_ss_policy,
        detect_fn=_detect_force_ss_policy,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SS_POLICY],
        description="Force screensaver with 10-min timeout and password via machine policy. Default: not set.",
        tags=["screensaver", "policy", "force", "security"],
    ),
    TweakDef(
        id="ss-disable-user-policy",
        label="Disable Screensaver (User Policy)",
        category="Screensaver & Lock",
        apply_fn=_apply_disable_ss_user_policy,
        remove_fn=_remove_disable_ss_user_policy,
        detect_fn=_detect_disable_ss_user_policy,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_POLICY_CU],
        description="Disable screensaver via user-level policy key. Default: not set.",
        tags=["screensaver", "policy", "user", "disable"],
    ),
    TweakDef(
        id="ss-enable-secure-desktop",
        label="Enable Secure Desktop for UAC",
        category="Screensaver & Lock",
        apply_fn=_apply_secure_desktop,
        remove_fn=_remove_secure_desktop,
        detect_fn=_detect_secure_desktop,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOCK_POLICY],
        description="Show UAC prompts on secure desktop (anti-spoofing). Default: enabled. Recommended: enabled.",
        tags=["uac", "secure-desktop", "security"],
    ),
    TweakDef(
        id="ss-disable-lock-slideshow",
        label="Disable Lock Screen Slideshow",
        category="Screensaver & Lock",
        apply_fn=_apply_disable_lock_slideshow,
        remove_fn=_remove_disable_lock_slideshow,
        detect_fn=_detect_disable_lock_slideshow,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOCKSS],
        description="Disable slideshow on the lock screen. Default: enabled.",
        tags=["lock", "slideshow", "screen"],
    ),
    TweakDef(
        id="ss-prevent-screensaver-change",
        label="Prevent Screensaver Change",
        category="Screensaver & Lock",
        apply_fn=_apply_prevent_ss_change,
        remove_fn=_remove_prevent_ss_change,
        detect_fn=_detect_prevent_ss_change,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NO_SS_CHANGE],
        description="Prevent users from changing screensaver settings. Default: allowed.",
        tags=["screensaver", "policy", "restrict", "kiosk"],
    ),
    TweakDef(
        id="ss-enable-transparency",
        label="Enable Transparency Effects",
        category="Screensaver & Lock",
        apply_fn=_apply_enable_transparency,
        remove_fn=_remove_enable_transparency,
        detect_fn=_detect_enable_transparency,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PERSONALIZE],
        description="Enable window transparency/acrylic effects. Default: enabled.",
        tags=["transparency", "acrylic", "effects", "visual"],
    ),
]


# -- 12. Set Screensaver Timeout to 15 Minutes ──────────────────────────────


def _apply_ss_timeout_15m(*, require_admin: bool = False) -> None:
    SESSION.backup([_SS_CU], "SSTimeout15m")
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "900")


def _remove_ss_timeout_15m(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "600")


def _detect_ss_timeout_15m() -> bool:
    return SESSION.read_string(_SS_CU, "ScreenSaveTimeOut") == "900"


# -- 13. Set Screensaver Timeout to 30 Minutes ──────────────────────────────


def _apply_ss_timeout_30m(*, require_admin: bool = False) -> None:
    SESSION.backup([_SS_CU], "SSTimeout30m")
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "1800")


def _remove_ss_timeout_30m(*, require_admin: bool = False) -> None:
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "600")


def _detect_ss_timeout_30m() -> bool:
    return SESSION.read_string(_SS_CU, "ScreenSaveTimeOut") == "1800"


TWEAKS += [
    TweakDef(
        id="ss-set-timeout-15min",
        label="Set Screensaver Timeout to 15 Minutes",
        category="Screensaver & Lock",
        apply_fn=_apply_ss_timeout_15m,
        remove_fn=_remove_ss_timeout_15m,
        detect_fn=_detect_ss_timeout_15m,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description=(
            "Sets the screensaver activation timeout to 15 minutes (900 seconds). "
            "Default: 600. Recommended: 900 for balanced security and convenience."
        ),
        tags=["screensaver", "timeout", "15min", "lock"],
    ),
    TweakDef(
        id="ss-set-timeout-30min",
        label="Set Screensaver Timeout to 30 Minutes",
        category="Screensaver & Lock",
        apply_fn=_apply_ss_timeout_30m,
        remove_fn=_remove_ss_timeout_30m,
        detect_fn=_detect_ss_timeout_30m,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SS_CU],
        description=(
            "Sets the screensaver activation timeout to 30 minutes (1800 seconds). "
            "Default: 600. Recommended: 1800 for extended-focus workflows."
        ),
        tags=["screensaver", "timeout", "30min", "lock"],
    ),
]


# -- Set Screensaver Timeout to 10 Minutes (Policy) ----------------------------


def _apply_scr_timeout_10min(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Screensaver: set timeout to 10 minutes via policy")
    SESSION.backup([_SS_CU, _SS_POLICY], "ScrTimeout10min")
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "600")
    SESSION.set_string(_SS_POLICY, "ScreenSaveTimeOut", "600")


def _remove_scr_timeout_10min(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SS_POLICY, "ScreenSaveTimeOut")
    SESSION.set_string(_SS_CU, "ScreenSaveTimeOut", "900")


def _detect_scr_timeout_10min() -> bool:
    return SESSION.read_string(_SS_POLICY, "ScreenSaveTimeOut") == "600"


# -- Require Password on Resume (Policy) ----------------------------------------


def _apply_scr_password_resume(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Screensaver: require password on resume via policy")
    SESSION.backup([_SS_CU, _SS_POLICY], "ScrPasswordResume")
    SESSION.set_string(_SS_CU, "ScreenSaverIsSecure", "1")
    SESSION.set_string(_SS_POLICY, "ScreenSaverIsSecure", "1")


def _remove_scr_password_resume(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SS_POLICY, "ScreenSaverIsSecure")
    SESSION.set_string(_SS_CU, "ScreenSaverIsSecure", "0")


def _detect_scr_password_resume() -> bool:
    return SESSION.read_string(_SS_POLICY, "ScreenSaverIsSecure") == "1"


# -- Disable Screensaver Completely (Policy) ------------------------------------


def _apply_scr_disable_screensaver(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Screensaver: disable screensaver via policy")
    SESSION.backup([_SS_CU, _SS_POLICY], "ScrDisable")
    SESSION.set_string(_SS_CU, "ScreenSaveActive", "0")
    SESSION.set_string(_SS_POLICY, "ScreenSaveActive", "0")


def _remove_scr_disable_screensaver(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SS_POLICY, "ScreenSaveActive")
    SESSION.set_string(_SS_CU, "ScreenSaveActive", "1")


def _detect_scr_disable_screensaver() -> bool:
    return SESSION.read_string(_SS_POLICY, "ScreenSaveActive") == "0"


TWEAKS += [
    TweakDef(
        id="scr-timeout-10min",
        label="Set Screensaver Timeout to 10 Minutes (Policy)",
        category="Screensaver & Lock",
        apply_fn=_apply_scr_timeout_10min,
        remove_fn=_remove_scr_timeout_10min,
        detect_fn=_detect_scr_timeout_10min,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SS_CU, _SS_POLICY],
        description=(
            "Sets screensaver timeout to 10 minutes via machine policy. "
            "Enforced across all users. Default: varies. Recommended: 600 seconds."
        ),
        tags=["screensaver", "timeout", "10min", "policy"],
    ),
    TweakDef(
        id="scr-password-on-resume",
        label="Require Password on Resume (Policy)",
        category="Screensaver & Lock",
        apply_fn=_apply_scr_password_resume,
        remove_fn=_remove_scr_password_resume,
        detect_fn=_detect_scr_password_resume,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SS_CU, _SS_POLICY],
        description=(
            "Requires password entry when resuming from screensaver via policy. "
            "Enforces lock screen security. Default: varies. Recommended: enabled."
        ),
        tags=["screensaver", "password", "resume", "security"],
    ),
    TweakDef(
        id="scr-disable-screensaver",
        label="Disable Screensaver Completely (Policy)",
        category="Screensaver & Lock",
        apply_fn=_apply_scr_disable_screensaver,
        remove_fn=_remove_scr_disable_screensaver,
        detect_fn=_detect_scr_disable_screensaver,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SS_CU, _SS_POLICY],
        description=(
            "Disables the screensaver completely via machine policy. "
            "Prevents screensaver from activating on any user. "
            "Default: Enabled. Recommended: Disabled only for kiosks."
        ),
        tags=["screensaver", "disable", "policy"],
    ),
]
