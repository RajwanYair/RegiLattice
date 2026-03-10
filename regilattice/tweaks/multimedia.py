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
            "Sets the system sound scheme to None, silencing all Windows event sounds. Default: .Default. Recommended: .None for quiet operation."
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
        description=("Disables the Windows startup/logon sound. Silences the chime played during boot. Default: Enabled. Recommended: Disabled."),
        tags=["multimedia", "startup", "sound", "boot"],
    ),
]


_KEY_WMP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"


# -- Disable AutoPlay Handlers (User) ---------------------------------------------


def _apply_disable_autoplay_handlers(*, require_admin: bool = True) -> None:
    SESSION.log("Multimedia: disable AutoPlay handlers at user level")
    SESSION.backup([_KEY_AUTOPLAY_CU], "AutoPlayHandlers")
    SESSION.set_dword(_KEY_AUTOPLAY_CU, "DisableAutoplay", 1)


def _remove_disable_autoplay_handlers(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_KEY_AUTOPLAY_CU, "DisableAutoplay")


def _detect_disable_autoplay_handlers() -> bool:
    return SESSION.read_dword(_KEY_AUTOPLAY_CU, "DisableAutoplay") == 1


# -- Disable Media Streaming (Policy) --------------------------------------------


def _apply_disable_media_streaming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Windows Media streaming via policy")
    SESSION.backup([_KEY_WMP_POLICY], "MediaStreaming")
    SESSION.set_dword(_KEY_WMP_POLICY, "PreventLibrarySharing", 1)


def _remove_disable_media_streaming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_WMP_POLICY, "PreventLibrarySharing")


def _detect_disable_media_streaming() -> bool:
    return SESSION.read_dword(_KEY_WMP_POLICY, "PreventLibrarySharing") == 1


TWEAKS += [
    TweakDef(
        id="media-disable-autoplay-handlers",
        label="Disable AutoPlay Handlers (User)",
        category="Multimedia",
        apply_fn=_apply_disable_autoplay_handlers,
        remove_fn=_remove_disable_autoplay_handlers,
        detect_fn=_detect_disable_autoplay_handlers,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_AUTOPLAY_CU],
        description=(
            "Disables AutoPlay handlers at the user level via DisableAutoplay DWORD. "
            "Prevents automatic launch of programs when media is inserted. "
            "Default: enabled. Recommended: disabled for security."
        ),
        tags=["multimedia", "autoplay", "handlers", "security"],
    ),
    TweakDef(
        id="media-disable-media-streaming",
        label="Disable Windows Media Streaming (Policy)",
        category="Multimedia",
        apply_fn=_apply_disable_media_streaming,
        remove_fn=_remove_disable_media_streaming,
        detect_fn=_detect_disable_media_streaming,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_WMP_POLICY],
        description=(
            "Disables Windows Media Player library sharing via policy. "
            "Prevents media streaming to other devices on the network. "
            "Default: allowed. Recommended: disabled for security."
        ),
        tags=["multimedia", "streaming", "media", "sharing", "policy"],
    ),
]


# -- 13. Disable WMP Network Sharing Service ──────────────────────────────────


def _apply_disable_wmp_net_sharing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable WMP network sharing service")
    SESSION.backup([_KEY_MEDIA_SVC], "WMPNetSharing")
    SESSION.set_dword(_KEY_MEDIA_SVC, "Start", 4)


def _remove_disable_wmp_net_sharing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_MEDIA_SVC, "Start", 3)


def _detect_disable_wmp_net_sharing() -> bool:
    return SESSION.read_dword(_KEY_MEDIA_SVC, "Start") == 4


# -- 14. Set Default Media Player Associations ─────────────────────────────────


def _apply_set_default_player_assoc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: set default media player preferences via policy")
    SESSION.backup([_KEY_WMP_POLICY], "DefaultPlayerAssoc")
    SESSION.set_dword(_KEY_WMP_POLICY, "SetupFirstRun", 0)
    SESSION.set_dword(_KEY_WMP_POLICY, "PlayerPrompt", 0)


def _remove_set_default_player_assoc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_WMP_POLICY, "SetupFirstRun")
    SESSION.delete_value(_KEY_WMP_POLICY, "PlayerPrompt")


def _detect_set_default_player_assoc() -> bool:
    return SESSION.read_dword(_KEY_WMP_POLICY, "SetupFirstRun") == 0 and SESSION.read_dword(_KEY_WMP_POLICY, "PlayerPrompt") == 0


# -- 15. Disable Windows Media DRM ────────────────────────────────────────────

_KEY_WM_DRM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM"


def _apply_disable_wm_drm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Windows Media DRM online access")
    SESSION.backup([_KEY_WM_DRM], "WMDRM")
    SESSION.set_dword(_KEY_WM_DRM, "DisableOnline", 1)


def _remove_disable_wm_drm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_WM_DRM, "DisableOnline")


def _detect_disable_wm_drm() -> bool:
    return SESSION.read_dword(_KEY_WM_DRM, "DisableOnline") == 1


TWEAKS += [
    TweakDef(
        id="media-disable-wmp-network-sharing",
        label="Disable WMP Network Sharing Service",
        category="Multimedia",
        apply_fn=_apply_disable_wmp_net_sharing,
        remove_fn=_remove_disable_wmp_net_sharing,
        detect_fn=_detect_disable_wmp_net_sharing,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_MEDIA_SVC],
        description=(
            "Disables the Windows Media Player Network Sharing Service (Start=4). "
            "Prevents DLNA/UPnP media streaming entirely. "
            "Default: manual (3). Recommended: disabled (4) for security."
        ),
        tags=["multimedia", "wmp", "network", "sharing", "dlna"],
    ),
    TweakDef(
        id="media-set-default-player-assoc",
        label="Set Default Media Player Associations",
        category="Multimedia",
        apply_fn=_apply_set_default_player_assoc,
        remove_fn=_remove_set_default_player_assoc,
        detect_fn=_detect_set_default_player_assoc,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_WMP_POLICY],
        description=(
            "Suppresses WMP first-run setup and player prompt via policy. "
            "Prevents Windows Media Player from claiming file associations. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["multimedia", "player", "associations", "default", "wmp"],
    ),
    TweakDef(
        id="media-disable-wm-drm",
        label="Disable Windows Media DRM",
        category="Multimedia",
        apply_fn=_apply_disable_wm_drm,
        remove_fn=_remove_disable_wm_drm,
        detect_fn=_detect_disable_wm_drm,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_WM_DRM],
        description=(
            "Disables Windows Media DRM online license acquisition via policy. "
            "Prevents DRM phone-home for protected media content. "
            "Default: enabled. Recommended: disabled for privacy."
        ),
        tags=["multimedia", "drm", "wmdrm", "license", "privacy"],
    ),
]


# ── Disable Game Bar (Policy) ─────────────────────────────────────────────────

_KEY_GAMEBAR_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"


def _apply_disable_gamebar_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable Xbox Game Bar via policy")
    SESSION.backup([_KEY_GAMEBAR_POLICY], "GameBarPolicy")
    SESSION.set_dword(_KEY_GAMEBAR_POLICY, "AllowGameDVR", 0)


def _remove_disable_gamebar_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_GAMEBAR_POLICY, "AllowGameDVR")


def _detect_disable_gamebar_policy() -> bool:
    return SESSION.read_dword(_KEY_GAMEBAR_POLICY, "AllowGameDVR") == 0


# ── Instant Menu Show Delay ───────────────────────────────────────────────────

_KEY_MENU_DELAY = r"HKEY_CURRENT_USER\Control Panel\Desktop"


def _apply_menu_show_instant(*, require_admin: bool = False) -> None:
    SESSION.log("Multimedia: set menu show delay to 0 (instant)")
    SESSION.backup([_KEY_MENU_DELAY], "MenuDelay")
    SESSION.set_string(_KEY_MENU_DELAY, "MenuShowDelay", "0")


def _remove_menu_show_instant(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_MENU_DELAY, "MenuShowDelay", "400")


def _detect_menu_show_instant() -> bool:
    return SESSION.read_string(_KEY_MENU_DELAY, "MenuShowDelay") == "0"


# ── Disable Logon User-Tile Animation ─────────────────────────────────────────

_KEY_LOGON_ANIM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"


def _apply_disable_logon_anim(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Multimedia: disable first-logon animation")
    SESSION.backup([_KEY_LOGON_ANIM], "LogonAnim")
    SESSION.set_dword(_KEY_LOGON_ANIM, "EnableFirstLogonAnimation", 0)


def _remove_disable_logon_anim(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_LOGON_ANIM, "EnableFirstLogonAnimation", 1)


def _detect_disable_logon_anim() -> bool:
    return SESSION.read_dword(_KEY_LOGON_ANIM, "EnableFirstLogonAnimation") == 0


# ── Disable Cursor Blink ─────────────────────────────────────────────────────

_KEY_CURSOR_DESKTOP = r"HKEY_CURRENT_USER\Control Panel\Desktop"


def _apply_disable_cursor_blink(*, require_admin: bool = False) -> None:
    SESSION.log("Multimedia: disable text cursor blinking (-1 = no blink)")
    SESSION.backup([_KEY_CURSOR_DESKTOP], "CursorBlink")
    SESSION.set_string(_KEY_CURSOR_DESKTOP, "CursorBlinkRate", "-1")


def _remove_disable_cursor_blink(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_CURSOR_DESKTOP, "CursorBlinkRate", "530")


def _detect_disable_cursor_blink() -> bool:
    return SESSION.read_string(_KEY_CURSOR_DESKTOP, "CursorBlinkRate") == "-1"


# ── Reduce Tooltip Delay ─────────────────────────────────────────────────────

_KEY_TOOLTIP_DELAY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"


def _apply_reduce_tooltip_delay(*, require_admin: bool = False) -> None:
    SESSION.log("Multimedia: reduce tooltip popup delay to 0 ms")
    SESSION.backup([_KEY_TOOLTIP_DELAY], "TooltipDelay")
    SESSION.set_dword(_KEY_TOOLTIP_DELAY, "ExtendedUIHoverTime", 0)


def _remove_reduce_tooltip_delay(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_TOOLTIP_DELAY, "ExtendedUIHoverTime")


def _detect_reduce_tooltip_delay() -> bool:
    return SESSION.read_dword(_KEY_TOOLTIP_DELAY, "ExtendedUIHoverTime") == 0


TWEAKS += [
    TweakDef(
        id="media-disable-gamebar-policy",
        label="Disable Xbox Game Bar (Policy)",
        category="Multimedia",
        apply_fn=_apply_disable_gamebar_policy,
        remove_fn=_remove_disable_gamebar_policy,
        detect_fn=_detect_disable_gamebar_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_GAMEBAR_POLICY],
        description=(
            "Disables the Xbox Game Bar via Group Policy (AllowGameDVR=0). "
            "Prevents Win+G from opening and removes the overlay entirely. "
            "Default: Allowed. Recommended: Disabled for non-gaming workstations."
        ),
        tags=["multimedia", "gamebar", "xbox", "policy", "performance"],
    ),
    TweakDef(
        id="media-menu-show-instant",
        label="Instant Context Menu (0 ms Delay)",
        category="Multimedia",
        apply_fn=_apply_menu_show_instant,
        remove_fn=_remove_menu_show_instant,
        detect_fn=_detect_menu_show_instant,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_MENU_DELAY],
        description=(
            "Sets MenuShowDelay to 0, making context menus and pop-up menus appear instantly. "
            "Default: 400 ms. Recommended: 0 for power users."
        ),
        tags=["multimedia", "menu", "delay", "performance", "ux"],
    ),
    TweakDef(
        id="media-disable-logon-anim",
        label="Disable First-Logon Animation",
        category="Multimedia",
        apply_fn=_apply_disable_logon_anim,
        remove_fn=_remove_disable_logon_anim,
        detect_fn=_detect_disable_logon_anim,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_LOGON_ANIM],
        description=(
            "Disables the animated user-tile / Getting Ready screen shown on first logon after updates. "
            "Speeds up login for domain and managed devices. Default: Enabled."
        ),
        tags=["multimedia", "logon", "animation", "boot", "performance"],
    ),
    TweakDef(
        id="media-disable-cursor-blink",
        label="Disable Text Cursor Blinking",
        category="Multimedia",
        apply_fn=_apply_disable_cursor_blink,
        remove_fn=_remove_disable_cursor_blink,
        detect_fn=_detect_disable_cursor_blink,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_CURSOR_DESKTOP],
        description=(
            "Disables cursor blinking in text fields (CursorBlinkRate=-1). "
            "Reduces visual distraction for users who find blinking cursors disruptive. Default: 530 ms."
        ),
        tags=["multimedia", "cursor", "blink", "accessibility", "ux"],
    ),
    TweakDef(
        id="media-reduce-tooltip-delay",
        label="Instant Tooltip Display (0 ms)",
        category="Multimedia",
        apply_fn=_apply_reduce_tooltip_delay,
        remove_fn=_remove_reduce_tooltip_delay,
        detect_fn=_detect_reduce_tooltip_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_TOOLTIP_DELAY],
        description=(
            "Sets extended UI hover time to 0, making Explorer tooltips appear immediately. "
            "Default: system default. Recommended: 0 for fast typists."
        ),
        tags=["multimedia", "tooltip", "delay", "explorer", "ux"],
    ),
]
