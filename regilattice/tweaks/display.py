"""Display tweaks — DPI scaling, ClearType, dark mode, transparency, animations."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Registry key constants ───────────────────────────────────────────────────

_KEY_DESKTOP = r"HKEY_CURRENT_USER\Control Panel\Desktop"
_KEY_WINDOW_METRICS = r"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"
_KEY_PERSONALIZE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Themes\Personalize"
)
_KEY_DWM = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"
_KEY_VISUAL_FX = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\VisualEffects"
)
_KEY_EDGE_UI = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"
_KEY_SENSRSVC = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc"
)
_KEY_COLORS = r"HKEY_CURRENT_USER\Control Panel\Colors"


# ── Disable DPI Scaling Override ─────────────────────────────────────────────


def _apply_disable_dpi_scaling(*, require_admin: bool = False) -> None:
    SESSION.log("Display: disable DPI scaling override")
    SESSION.backup([_KEY_DESKTOP], "DpiScaling")
    SESSION.set_dword(_KEY_DESKTOP, "DpiScalingVer", 0x00001018)
    SESSION.set_dword(_KEY_DESKTOP, "Win8DpiScaling", 1)


def _remove_disable_dpi_scaling(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_DESKTOP, "DpiScalingVer")
    SESSION.set_dword(_KEY_DESKTOP, "Win8DpiScaling", 0)


def _detect_disable_dpi_scaling() -> bool:
    return SESSION.read_dword(_KEY_DESKTOP, "Win8DpiScaling") == 1


# ── Enable ClearType Font Smoothing ──────────────────────────────────────────


def _apply_enable_cleartype(*, require_admin: bool = False) -> None:
    SESSION.log("Display: enable ClearType font smoothing")
    SESSION.backup([_KEY_DESKTOP], "ClearType")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "2")
    SESSION.set_dword(_KEY_DESKTOP, "FontSmoothingType", 2)


def _remove_enable_cleartype(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "0")
    SESSION.set_dword(_KEY_DESKTOP, "FontSmoothingType", 0)


def _detect_enable_cleartype() -> bool:
    return (
        SESSION.read_string(_KEY_DESKTOP, "FontSmoothing") == "2"
        and SESSION.read_dword(_KEY_DESKTOP, "FontSmoothingType") == 2
    )


# ── Force Custom DPI (96 DPI / 100%) ────────────────────────────────────────


def _apply_force_96dpi(*, require_admin: bool = False) -> None:
    SESSION.log("Display: force 96 DPI (100% scaling)")
    SESSION.backup([_KEY_DESKTOP], "LogPixels")
    SESSION.set_dword(_KEY_DESKTOP, "LogPixels", 96)


def _remove_force_96dpi(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_DESKTOP, "LogPixels")


def _detect_force_96dpi() -> bool:
    return SESSION.read_dword(_KEY_DESKTOP, "LogPixels") == 96


# ── Dark Mode for Apps ──────────────────────────────────────────────────────


def _apply_dark_mode_apps(*, require_admin: bool = False) -> None:
    SESSION.log("Display: enable dark mode for apps")
    SESSION.backup([_KEY_PERSONALIZE], "DarkModeApps")
    SESSION.set_dword(_KEY_PERSONALIZE, "AppsUseLightTheme", 0)


def _remove_dark_mode_apps(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_KEY_PERSONALIZE, "AppsUseLightTheme", 1)


def _detect_dark_mode_apps() -> bool:
    return SESSION.read_dword(_KEY_PERSONALIZE, "AppsUseLightTheme") == 0


# ── Dark Mode for System ────────────────────────────────────────────────────


def _apply_dark_mode_system(*, require_admin: bool = False) -> None:
    SESSION.log("Display: enable dark mode for system")
    SESSION.backup([_KEY_PERSONALIZE], "DarkModeSystem")
    SESSION.set_dword(_KEY_PERSONALIZE, "SystemUsesLightTheme", 0)


def _remove_dark_mode_system(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_KEY_PERSONALIZE, "SystemUsesLightTheme", 1)


def _detect_dark_mode_system() -> bool:
    return SESSION.read_dword(_KEY_PERSONALIZE, "SystemUsesLightTheme") == 0


# ── Disable Transparency Effects ────────────────────────────────────────────


def _apply_disable_transparency(*, require_admin: bool = False) -> None:
    SESSION.log("Display: disable transparency effects")
    SESSION.backup([_KEY_PERSONALIZE], "Transparency")
    SESSION.set_dword(_KEY_PERSONALIZE, "EnableTransparency", 0)


def _remove_disable_transparency(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_KEY_PERSONALIZE, "EnableTransparency", 1)


def _detect_disable_transparency() -> bool:
    return SESSION.read_dword(_KEY_PERSONALIZE, "EnableTransparency") == 0


# ── Disable Window Animations ───────────────────────────────────────────────


def _apply_disable_animations(*, require_admin: bool = False) -> None:
    SESSION.log("Display: disable minimize/maximize animations")
    SESSION.backup([_KEY_WINDOW_METRICS], "MinAnimate")
    SESSION.set_string(_KEY_WINDOW_METRICS, "MinAnimate", "0")


def _remove_disable_animations(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_WINDOW_METRICS, "MinAnimate", "1")


def _detect_disable_animations() -> bool:
    return SESSION.read_string(_KEY_WINDOW_METRICS, "MinAnimate") == "0"


# ── Disable Wallpaper JPEG Compression ──────────────────────────────────────


def _apply_disable_wallpaper_compression(*, require_admin: bool = False) -> None:
    SESSION.log("Display: disable wallpaper JPEG compression")
    SESSION.backup([_KEY_DESKTOP], "WallpaperCompression")
    SESSION.set_dword(_KEY_DESKTOP, "JPEGImportQuality", 100)


def _remove_disable_wallpaper_compression(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_DESKTOP, "JPEGImportQuality")


def _detect_disable_wallpaper_compression() -> bool:
    return SESSION.read_dword(_KEY_DESKTOP, "JPEGImportQuality") == 100


# ── Enable Accent Color on Title Bars ───────────────────────────────────────


def _apply_accent_title_bars(*, require_admin: bool = False) -> None:
    SESSION.log("Display: show accent color on title bars and borders")
    SESSION.backup([_KEY_DWM], "AccentTitleBars")
    SESSION.set_dword(_KEY_DWM, "ColorPrevalence", 1)


def _remove_accent_title_bars(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_KEY_DWM, "ColorPrevalence", 0)


def _detect_accent_title_bars() -> bool:
    return SESSION.read_dword(_KEY_DWM, "ColorPrevalence") == 1


# ── Disable Screen Edge Swipe ───────────────────────────────────────────────


def _apply_disable_edge_swipe(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Display: disable screen edge swipe gestures")
    SESSION.backup([_KEY_EDGE_UI], "EdgeSwipe")
    SESSION.set_dword(_KEY_EDGE_UI, "AllowEdgeSwipe", 0)


def _remove_disable_edge_swipe(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_EDGE_UI, "AllowEdgeSwipe")


def _detect_disable_edge_swipe() -> bool:
    return SESSION.read_dword(_KEY_EDGE_UI, "AllowEdgeSwipe") == 0


# ── Disable Adaptive Brightness ─────────────────────────────────────────────


def _apply_disable_adaptive_brightness(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Display: disable adaptive brightness sensor service")
    SESSION.backup([_KEY_SENSRSVC], "AdaptiveBrightness")
    SESSION.set_dword(_KEY_SENSRSVC, "Start", 4)


def _remove_disable_adaptive_brightness(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_SENSRSVC, "Start", 3)


def _detect_disable_adaptive_brightness() -> bool:
    return SESSION.read_dword(_KEY_SENSRSVC, "Start") == 4


# ── Set Display Scaling DPI Override ─────────────────────────────────────────


def _apply_dpi_override(*, require_admin: bool = False) -> None:
    SESSION.log("Display: set DPI override to 96 DPI (100% scaling)")
    SESSION.backup([_KEY_DESKTOP], "DpiOverride")
    SESSION.set_dword(_KEY_DESKTOP, "Win8DpiScaling", 1)
    SESSION.set_dword(_KEY_DESKTOP, "LogPixels", 96)


def _remove_dpi_override(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_DESKTOP, "Win8DpiScaling")
    SESSION.delete_value(_KEY_DESKTOP, "LogPixels")


def _detect_dpi_override() -> bool:
    return SESSION.read_dword(_KEY_DESKTOP, "Win8DpiScaling") == 1


# ── Disable Adaptive Brightness (ICM Calibration) ───────────────────────────

_KEY_ICM_CALIBRATION = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\ICM\Calibration"
)


def _apply_disable_adaptive_brightness_icm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Display: disable adaptive brightness via ICM calibration")
    SESSION.backup([_KEY_ICM_CALIBRATION], "AdaptiveBrightnessICM")
    SESSION.set_dword(_KEY_ICM_CALIBRATION, "AdaptiveBrightness", 0)


def _remove_disable_adaptive_brightness_icm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_ICM_CALIBRATION, "AdaptiveBrightness")


def _detect_disable_adaptive_brightness_icm() -> bool:
    return SESSION.read_dword(_KEY_ICM_CALIBRATION, "AdaptiveBrightness") == 0


# ── Force Hardware Cursor (Disable Smooth Scroll) ───────────────────────────


def _apply_hardware_cursor(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Display: force hardware cursor (disable smooth scrolling)")
    SESSION.backup([_KEY_DESKTOP], "HardwareCursor")
    SESSION.set_string(_KEY_DESKTOP, "SmoothScroll", "0")


def _remove_hardware_cursor(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_KEY_DESKTOP, "SmoothScroll", "1")


def _detect_hardware_cursor() -> bool:
    return SESSION.read_string(_KEY_DESKTOP, "SmoothScroll") == "0"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="display-disable-dpi-scaling",
        label="Disable DPI Scaling Override",
        category="Display",
        apply_fn=_apply_disable_dpi_scaling,
        remove_fn=_remove_disable_dpi_scaling,
        detect_fn=_detect_disable_dpi_scaling,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Enables the Windows 8-style DPI scaling override, forcing "
            "the system DPI setting for all applications."
        ),
        tags=["display", "dpi", "scaling"],
    ),
    TweakDef(
        id="display-enable-cleartype",
        label="Enable ClearType Font Smoothing",
        category="Display",
        apply_fn=_apply_enable_cleartype,
        remove_fn=_remove_enable_cleartype,
        detect_fn=_detect_enable_cleartype,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Enables ClearType sub-pixel font rendering for sharper "
            "text on LCD screens."
        ),
        tags=["display", "cleartype", "font", "smoothing"],
    ),
    TweakDef(
        id="display-force-96dpi",
        label="Force 96 DPI (100% Scaling)",
        category="Display",
        apply_fn=_apply_force_96dpi,
        remove_fn=_remove_force_96dpi,
        detect_fn=_detect_force_96dpi,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Forces the display to use 96 DPI (100% scaling), disabling "
            "any high-DPI scaling that Windows may apply."
        ),
        tags=["display", "dpi", "scaling", "96dpi"],
    ),
    TweakDef(
        id="display-dark-mode-apps",
        label="Dark Mode for Apps",
        category="Display",
        apply_fn=_apply_dark_mode_apps,
        remove_fn=_remove_dark_mode_apps,
        detect_fn=_detect_dark_mode_apps,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_PERSONALIZE],
        description="Switches UWP and modern apps to their dark colour scheme.",
        tags=["display", "dark", "theme", "apps"],
    ),
    TweakDef(
        id="display-dark-mode-system",
        label="Dark Mode for System",
        category="Display",
        apply_fn=_apply_dark_mode_system,
        remove_fn=_remove_dark_mode_system,
        detect_fn=_detect_dark_mode_system,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_PERSONALIZE],
        description=(
            "Switches the Windows system theme (taskbar, Start menu, "
            "Action Center) to dark mode."
        ),
        tags=["display", "dark", "theme", "system"],
    ),
    TweakDef(
        id="display-disable-transparency",
        label="Disable Transparency Effects",
        category="Display",
        apply_fn=_apply_disable_transparency,
        remove_fn=_remove_disable_transparency,
        detect_fn=_detect_disable_transparency,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_PERSONALIZE],
        description=(
            "Disables the acrylic/blur transparency effects on the "
            "taskbar, Start menu, and window backgrounds."
        ),
        tags=["display", "transparency", "performance", "visual"],
    ),
    TweakDef(
        id="display-disable-animations",
        label="Disable Window Animations",
        category="Display",
        apply_fn=_apply_disable_animations,
        remove_fn=_remove_disable_animations,
        detect_fn=_detect_disable_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_WINDOW_METRICS],
        description=(
            "Disables minimize and maximize window animations for "
            "snappier window management."
        ),
        tags=["display", "animation", "performance", "visual"],
    ),
    TweakDef(
        id="display-disable-wallpaper-compression",
        label="Disable Wallpaper JPEG Compression",
        category="Display",
        apply_fn=_apply_disable_wallpaper_compression,
        remove_fn=_remove_disable_wallpaper_compression,
        detect_fn=_detect_disable_wallpaper_compression,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Sets wallpaper JPEG import quality to 100%, preventing "
            "Windows from compressing desktop wallpapers."
        ),
        tags=["display", "wallpaper", "quality", "compression"],
    ),
    TweakDef(
        id="display-accent-title-bars",
        label="Accent Color on Title Bars",
        category="Display",
        apply_fn=_apply_accent_title_bars,
        remove_fn=_remove_accent_title_bars,
        detect_fn=_detect_accent_title_bars,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DWM],
        description=(
            "Shows the Windows accent colour on title bars and "
            "window borders."
        ),
        tags=["display", "accent", "color", "titlebar", "dwm"],
    ),
    TweakDef(
        id="display-disable-edge-swipe",
        label="Disable Screen Edge Swipe",
        category="Display",
        apply_fn=_apply_disable_edge_swipe,
        remove_fn=_remove_disable_edge_swipe,
        detect_fn=_detect_disable_edge_swipe,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_EDGE_UI],
        description=(
            "Disables the screen edge swipe gesture that opens the "
            "Charms bar or Action Center on touch devices."
        ),
        tags=["display", "edge", "swipe", "gesture", "touch"],
    ),
    TweakDef(
        id="display-disable-adaptive-brightness",
        label="Disable Adaptive Brightness",
        category="Display",
        apply_fn=_apply_disable_adaptive_brightness,
        remove_fn=_remove_disable_adaptive_brightness,
        detect_fn=_detect_disable_adaptive_brightness,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_SENSRSVC],
        description=(
            "Disables the Adaptive Brightness sensor service. Prevents automatic screen brightness "
            "changes based on ambient light. Default: Enabled (3=Manual). Recommended: Disabled (4)."
        ),
        tags=["display", "brightness", "adaptive", "performance"],
    ),
    TweakDef(
        id="display-dpi-override",
        label="Set Display Scaling DPI Override",
        category="Display",
        apply_fn=_apply_dpi_override,
        remove_fn=_remove_dpi_override,
        detect_fn=_detect_dpi_override,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Forces display scaling to 100% (96 DPI) using the legacy DPI override. "
            "Disables DPI virtualization for crisp rendering. Default: System-managed. "
            "Recommended: 96 DPI for external monitors."
        ),
        tags=["display", "dpi", "scaling", "performance"],
    ),
    TweakDef(
        id="display-disable-adaptive-brightness-icm",
        label="Disable Adaptive Brightness (ICM)",
        category="Display",
        apply_fn=_apply_disable_adaptive_brightness_icm,
        remove_fn=_remove_disable_adaptive_brightness_icm,
        detect_fn=_detect_disable_adaptive_brightness_icm,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_ICM_CALIBRATION],
        description=(
            "Disables adaptive brightness via ICM display calibration. "
            "Prevents automatic brightness adjustments based on content. "
            "Default: Enabled. Recommended: Disabled for consistent brightness."
        ),
        tags=["display", "brightness", "icm", "calibration"],
    ),
    TweakDef(
        id="display-hardware-cursor",
        label="Force Hardware Cursor",
        category="Display",
        apply_fn=_apply_hardware_cursor,
        remove_fn=_remove_hardware_cursor,
        detect_fn=_detect_hardware_cursor,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Forces hardware cursor rendering and disables smooth scrolling. "
            "Reduces input lag and cursor rendering overhead. "
            "Default: Smooth scrolling on. Recommended: Hardware cursor for gaming."
        ),
        tags=["display", "cursor", "hardware", "performance"],
    ),
]


# ── Disable Window Transparency Effect ───────────────────────────────────────


def _apply_transparency_effect_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable window transparency effects")
    SESSION.backup([_KEY_PERSONALIZE], "TransparencyEffect")
    SESSION.set_dword(_KEY_PERSONALIZE, "EnableTransparency", 0)


def _remove_transparency_effect_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_PERSONALIZE, "EnableTransparency", 1)


def _detect_transparency_effect_off() -> bool:
    return SESSION.read_dword(_KEY_PERSONALIZE, "EnableTransparency") == 0


# ── Disable Minimize Animation ───────────────────────────────────────────────


def _apply_min_animate_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable window minimize/maximize animation")
    SESSION.backup([_KEY_WINDOW_METRICS], "MinAnimate")
    SESSION.set_string(_KEY_WINDOW_METRICS, "MinAnimate", "0")


def _remove_min_animate_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_KEY_WINDOW_METRICS, "MinAnimate", "1")


def _detect_min_animate_off() -> bool:
    return SESSION.read_string(_KEY_WINDOW_METRICS, "MinAnimate") == "0"


TWEAKS += [
    TweakDef(
        id="display-disable-transparency-effect",
        label="Disable Window Transparency Effect",
        category="Display",
        apply_fn=_apply_transparency_effect_off,
        remove_fn=_remove_transparency_effect_off,
        detect_fn=_detect_transparency_effect_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_PERSONALIZE],
        description=(
            "Disables window transparency and acrylic blur effects. "
            "Improves rendering performance on integrated GPUs. "
            "Default: Enabled. Recommended: Disabled for performance."
        ),
        tags=["display", "transparency", "acrylic", "performance"],
    ),
    TweakDef(
        id="display-disable-animation-effects",
        label="Disable Minimize/Maximize Animation",
        category="Display",
        apply_fn=_apply_min_animate_off,
        remove_fn=_remove_min_animate_off,
        detect_fn=_detect_min_animate_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_WINDOW_METRICS],
        description=(
            "Disables minimize and maximize window animations via "
            "WindowMetrics. Makes window switching feel instant. "
            "Default: Enabled. Recommended: Disabled for responsiveness."
        ),
        tags=["display", "animation", "minimize", "performance"],
    ),
]
