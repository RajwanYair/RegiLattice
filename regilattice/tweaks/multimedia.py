"""Multimedia tweaks -- AutoPlay, media sharing, screen saver, wallpaper quality, animations, sounds."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# -- Registry key constants ----------------------------------------------------

_KEY_AUTOPLAY_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"

_KEY_AUTOPLAY_POLICY_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"

_KEY_AUTORUN_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"

_KEY_MEDIA_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc"

_KEY_GAME_CONFIG_CU = r"HKEY_CURRENT_USER\System\GameConfigStore"

_KEY_GAMEBAR_CU = r"HKEY_CURRENT_USER\Software\Microsoft\GameBar"

_KEY_DESKTOP_CU = r"HKEY_CURRENT_USER\Control Panel\Desktop"

_KEY_WINMETRICS_CU = r"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"

_KEY_SOUND_SCHEME_CU = r"HKEY_CURRENT_USER\AppEvents\Schemes"

_KEY_BOOT_ANIM_LM = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Authentication\LogonUI\BootAnimation"
)


# -- 1. Disable AutoPlay (HKLM policy -- all drive types) ---------------------


def _apply_disable_autoplay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable AutoPlay for all drives via policy")
    SESSION.backup([_KEY_AUTOPLAY_CU, _KEY_AUTOPLAY_POLICY_LM], "AutoPlay")
    SESSION.set_dword(_KEY_AUTOPLAY_CU, "DisableAutoplay", 1)
    SESSION.set_dword(_KEY_AUTOPLAY_POLICY_LM, "NoDriveTypeAutoRun", 255)


def _remove_disable_autoplay(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable AutoPlay")
    SESSION.delete_value(_KEY_AUTOPLAY_CU, "DisableAutoplay")
    SESSION.delete_value(_KEY_AUTOPLAY_POLICY_LM, "NoDriveTypeAutoRun")


def _detect_disable_autoplay() -> bool:
    return SESSION.read_dword(_KEY_AUTOPLAY_POLICY_LM, "NoDriveTypeAutoRun") == 255


# -- 2. Disable AutoRun -------------------------------------------------------


def _apply_disable_autorun(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable AutoRun for all drives")
    SESSION.backup([_KEY_AUTORUN_LM], "AutoRun")
    SESSION.set_dword(_KEY_AUTORUN_LM, "NoAutorun", 1)


def _remove_disable_autorun(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable AutoRun")
    SESSION.delete_value(_KEY_AUTORUN_LM, "NoAutorun")


def _detect_disable_autorun() -> bool:
    return SESSION.read_dword(_KEY_AUTORUN_LM, "NoAutorun") == 1


# -- 3. Disable Media Sharing Service -----------------------------------------


def _apply_disable_media_sharing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Windows Media Player Network Sharing Service")
    SESSION.backup([_KEY_MEDIA_SVC], "MediaSharing")
    SESSION.set_dword(_KEY_MEDIA_SVC, "Start", 4)


def _remove_disable_media_sharing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable media sharing service")
    SESSION.set_dword(_KEY_MEDIA_SVC, "Start", 3)


def _detect_disable_media_sharing() -> bool:
    return SESSION.read_dword(_KEY_MEDIA_SVC, "Start") == 4


# -- 4. Disable Game DVR / Captures -------------------------------------------


def _apply_disable_game_dvr(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Game DVR captures")
    SESSION.backup([_KEY_GAME_CONFIG_CU], "GameDVR")
    SESSION.set_dword(_KEY_GAME_CONFIG_CU, "GameDVR_Enabled", 0)


def _remove_disable_game_dvr(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable Game DVR captures")
    SESSION.set_dword(_KEY_GAME_CONFIG_CU, "GameDVR_Enabled", 1)


def _detect_disable_game_dvr() -> bool:
    return SESSION.read_dword(_KEY_GAME_CONFIG_CU, "GameDVR_Enabled") == 0


# -- 5. Disable Game Bar Tips -------------------------------------------------


def _apply_disable_game_bar_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Game Bar startup tips")
    SESSION.backup([_KEY_GAMEBAR_CU], "GameBarTips")
    SESSION.set_dword(_KEY_GAMEBAR_CU, "ShowStartupPanel", 0)


def _remove_disable_game_bar_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable Game Bar startup tips")
    SESSION.set_dword(_KEY_GAMEBAR_CU, "ShowStartupPanel", 1)


def _detect_disable_game_bar_tips() -> bool:
    return SESSION.read_dword(_KEY_GAMEBAR_CU, "ShowStartupPanel") == 0


# -- 6. Disable Screen Saver --------------------------------------------------


def _apply_disable_screensaver(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable screen saver")
    SESSION.backup([_KEY_DESKTOP_CU], "ScreenSaver")
    SESSION.set_string(_KEY_DESKTOP_CU, "ScreenSaveActive", "0")


def _remove_disable_screensaver(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable screen saver")
    SESSION.set_string(_KEY_DESKTOP_CU, "ScreenSaveActive", "1")


def _detect_disable_screensaver() -> bool:
    return SESSION.read_string(_KEY_DESKTOP_CU, "ScreenSaveActive") == "0"


# -- 7. Set Wallpaper JPEG Quality to Maximum ---------------------------------


def _apply_set_wallpaper_quality(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: set wallpaper JPEG import quality to 100")
    SESSION.backup([_KEY_DESKTOP_CU], "WallpaperQuality")
    SESSION.set_dword(_KEY_DESKTOP_CU, "JPEGImportQuality", 100)


def _remove_set_wallpaper_quality(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: reset wallpaper quality to default")
    SESSION.delete_value(_KEY_DESKTOP_CU, "JPEGImportQuality")


def _detect_set_wallpaper_quality() -> bool:
    return SESSION.read_dword(_KEY_DESKTOP_CU, "JPEGImportQuality") == 100


# -- 8. Disable Window Min/Max Animations -------------------------------------


def _apply_disable_window_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable window minimize/maximize animations")
    SESSION.backup([_KEY_WINMETRICS_CU], "WindowAnimations")
    SESSION.set_string(_KEY_WINMETRICS_CU, "MinAnimate", "0")


def _remove_disable_window_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable window minimize/maximize animations")
    SESSION.set_string(_KEY_WINMETRICS_CU, "MinAnimate", "1")


def _detect_disable_window_animations() -> bool:
    return SESSION.read_string(_KEY_WINMETRICS_CU, "MinAnimate") == "0"


# -- 9. Disable System Sound Scheme -------------------------------------------


def _apply_disable_sound_scheme(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: set system sound scheme to None")
    SESSION.backup([_KEY_SOUND_SCHEME_CU], "SoundScheme")
    SESSION.set_string(_KEY_SOUND_SCHEME_CU, "", ".None")


def _remove_disable_sound_scheme(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: restore default system sound scheme")
    SESSION.set_string(_KEY_SOUND_SCHEME_CU, "", ".Default")


def _detect_disable_sound_scheme() -> bool:
    return SESSION.read_string(_KEY_SOUND_SCHEME_CU, "") == ".None"


# -- 10. Disable Windows Startup Sound ----------------------------------------


def _apply_disable_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Windows startup sound")
    SESSION.backup([_KEY_BOOT_ANIM_LM], "StartupSound")
    SESSION.set_dword(_KEY_BOOT_ANIM_LM, "DisableStartupSound", 1)


def _remove_disable_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: re-enable Windows startup sound")
    SESSION.delete_value(_KEY_BOOT_ANIM_LM, "DisableStartupSound")


def _detect_disable_startup_sound() -> bool:
    return SESSION.read_dword(_KEY_BOOT_ANIM_LM, "DisableStartupSound") == 1


# -- TWEAKS export -------------------------------------------------------------

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="media-disable-autoplay",
        label="Disable AutoPlay for All Drives",
        category="Multimedia",
        apply_fn=_apply_disable_autoplay,
        remove_fn=_remove_disable_autoplay,
        detect_fn=_detect_disable_autoplay,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_AUTOPLAY_CU, _KEY_AUTOPLAY_POLICY_LM],
        description=(
            "Disables AutoPlay for all media and removable drives via "
            "NoDriveTypeAutoRun policy. Prevents automatic execution of "
            "media content. Default: Enabled. Recommended: Disabled."
        ),
        tags=["multimedia", "autoplay", "security", "drives"],
    ),
    TweakDef(
        id="media-disable-autorun",
        label="Disable AutoRun for All Drives",
        category="Multimedia",
        apply_fn=_apply_disable_autorun,
        remove_fn=_remove_disable_autorun,
        detect_fn=_detect_disable_autorun,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_AUTORUN_LM],
        description=(
            "Disables AutoRun for all drive types, preventing automatic "
            "execution of autorun.inf files. Mitigates USB-based malware. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["multimedia", "autorun", "security", "usb"],
    ),
    TweakDef(
        id="media-disable-media-sharing",
        label="Disable Media Streaming/Sharing Service",
        category="Multimedia",
        apply_fn=_apply_disable_media_sharing,
        remove_fn=_remove_disable_media_sharing,
        detect_fn=_detect_disable_media_sharing,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_MEDIA_SVC],
        description=(
            "Disables the Windows Media Player Network Sharing Service "
            "(WMPNetworkSvc). Prevents DLNA media streaming to other "
            "devices. Default: Manual. Recommended: Disabled."
        ),
        tags=["multimedia", "sharing", "dlna", "service"],
    ),
    TweakDef(
        id="media-disable-game-dvr",
        label="Disable Game DVR Captures",
        category="Multimedia",
        apply_fn=_apply_disable_game_dvr,
        remove_fn=_remove_disable_game_dvr,
        detect_fn=_detect_disable_game_dvr,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_GAME_CONFIG_CU],
        description=(
            "Disables Game DVR background recording and captures. "
            "Frees GPU encoder resources and reduces disk I/O. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["multimedia", "game-dvr", "recording", "performance"],
    ),
    TweakDef(
        id="media-disable-game-bar-tips",
        label="Disable Game Bar Startup Tips",
        category="Multimedia",
        apply_fn=_apply_disable_game_bar_tips,
        remove_fn=_remove_disable_game_bar_tips,
        detect_fn=_detect_disable_game_bar_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_GAMEBAR_CU],
        description=(
            "Disables the Game Bar startup panel and tips overlay. "
            "Removes the popup that appears when launching games. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["multimedia", "gamebar", "tips", "overlay"],
    ),
    TweakDef(
        id="media-disable-screensaver",
        label="Disable Screen Saver",
        category="Multimedia",
        apply_fn=_apply_disable_screensaver,
        remove_fn=_remove_disable_screensaver,
        detect_fn=_detect_disable_screensaver,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP_CU],
        description=(
            "Disables the Windows screen saver. Prevents screen saver "
            "from activating during idle periods. "
            "Default: Enabled. Recommended: Disabled on desktops."
        ),
        tags=["multimedia", "screensaver", "display", "idle"],
    ),
    TweakDef(
        id="media-set-wallpaper-quality",
        label="Set Wallpaper JPEG Quality to Maximum",
        category="Multimedia",
        apply_fn=_apply_set_wallpaper_quality,
        remove_fn=_remove_set_wallpaper_quality,
        detect_fn=_detect_set_wallpaper_quality,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP_CU],
        description=(
            "Sets desktop wallpaper JPEG import quality to 100 percent. "
            "Prevents Windows from recompressing wallpaper images. "
            "Default: ~85. Recommended: 100."
        ),
        tags=["multimedia", "wallpaper", "quality", "display"],
    ),
    TweakDef(
        id="media-disable-window-animations",
        label="Disable Window Minimize/Maximize Animations",
        category="Multimedia",
        apply_fn=_apply_disable_window_animations,
        remove_fn=_remove_disable_window_animations,
        detect_fn=_detect_disable_window_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_WINMETRICS_CU],
        description=(
            "Disables minimize and maximize window animations. "
            "Makes window actions feel instant. "
            "Default: Enabled. Recommended: Disabled for responsiveness."
        ),
        tags=["multimedia", "animations", "performance", "windows"],
    ),
    TweakDef(
        id="media-disable-sound-scheme",
        label="Disable System Sound Scheme",
        category="Multimedia",
        apply_fn=_apply_disable_sound_scheme,
        remove_fn=_remove_disable_sound_scheme,
        detect_fn=_detect_disable_sound_scheme,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_SOUND_SCHEME_CU],
        description=(
            "Sets the system sound scheme to None, silencing all "
            "Windows event sounds. "
            "Default: .Default. Recommended: .None for quiet operation."
        ),
        tags=["multimedia", "sounds", "scheme", "quiet"],
    ),
    TweakDef(
        id="media-disable-startup-sound",
        label="Disable Windows Startup Sound",
        category="Multimedia",
        apply_fn=_apply_disable_startup_sound,
        remove_fn=_remove_disable_startup_sound,
        detect_fn=_detect_disable_startup_sound,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_BOOT_ANIM_LM],
        description=(
            "Disables the Windows startup/logon sound. "
            "Silences the chime played during boot. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["multimedia", "startup", "sound", "boot"],
    ),
]
