"""Fonts tweaks — ClearType, font smoothing, caching, download policies, and rendering."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Registry key constants ───────────────────────────────────────────────────

_KEY_DESKTOP = r"HKEY_CURRENT_USER\Control Panel\Desktop"

_KEY_USER_FONTS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts"
)

_KEY_EDGE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"

_KEY_KERNEL = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Kernel"
)

_KEY_FONTCACHE_SVC = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache"
)

_KEY_FONTCACHE3_SVC = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache3.0.0.0"
)

_KEY_AVALON_DISPLAY1 = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1"
)

_KEY_AVALON_GRAPHICS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics"
)

_KEY_IE_SECURITY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft"
    r"\Windows\CurrentVersion\Internet Settings\Zones\3"
)

_KEY_FONT_PROVIDERS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"
)


# ── 1. Enable ClearType ─────────────────────────────────────────────────────


def _apply_enable_cleartype(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: enable ClearType font smoothing")
    SESSION.backup([_KEY_DESKTOP], "FontClearType")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothingType", "2")


def _remove_enable_cleartype(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothingType", "1")


def _detect_enable_cleartype() -> bool:
    return SESSION.read_string(_KEY_DESKTOP, "FontSmoothingType") == "2"


# ── 2. Enable Font Smoothing ────────────────────────────────────────────────


def _apply_enable_font_smoothing(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: enable font smoothing")
    SESSION.backup([_KEY_DESKTOP], "FontSmoothing")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "2")


def _remove_enable_font_smoothing(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "0")


def _detect_enable_font_smoothing() -> bool:
    return SESSION.read_string(_KEY_DESKTOP, "FontSmoothing") == "2"


# ── 3. Disable Font Antialiasing (performance) ──────────────────────────────


def _apply_disable_antialiasing(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: disable font antialiasing for performance")
    SESSION.backup([_KEY_DESKTOP], "FontAntialiasing")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothingType", "0")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "0")


def _remove_disable_antialiasing(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothingType", "2")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "2")


def _detect_disable_antialiasing() -> bool:
    return SESSION.read_string(_KEY_DESKTOP, "FontSmoothingType") == "0"


# ── 4. Set Default System Font to Segoe UI ──────────────────────────────────


def _apply_set_segoe_ui(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: set default system font to Segoe UI")
    SESSION.backup([_KEY_USER_FONTS], "DefaultFont")
    SESSION.set_string(
        _KEY_USER_FONTS, "Segoe UI (TrueType)", "segoeui.ttf"
    )
    SESSION.set_string(
        _KEY_USER_FONTS, "Segoe UI Bold (TrueType)", "segoeuib.ttf"
    )
    SESSION.set_string(
        _KEY_USER_FONTS, "Segoe UI Italic (TrueType)", "segoeuii.ttf"
    )
    SESSION.set_string(
        _KEY_USER_FONTS, "Segoe UI Bold Italic (TrueType)", "segoeuiz.ttf"
    )


def _remove_set_segoe_ui(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_USER_FONTS, "Segoe UI (TrueType)")
    SESSION.delete_value(_KEY_USER_FONTS, "Segoe UI Bold (TrueType)")
    SESSION.delete_value(_KEY_USER_FONTS, "Segoe UI Italic (TrueType)")
    SESSION.delete_value(_KEY_USER_FONTS, "Segoe UI Bold Italic (TrueType)")


def _detect_set_segoe_ui() -> bool:
    return (
        SESSION.read_string(_KEY_USER_FONTS, "Segoe UI (TrueType)")
        == "segoeui.ttf"
    )


# ── 5. Disable Font Download via Edge Policy ────────────────────────────────


def _apply_disable_font_download_edge(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: disable font download in Edge via policy")
    SESSION.backup([_KEY_EDGE_POLICY], "EdgeFontDownload")
    SESSION.set_dword(_KEY_EDGE_POLICY, "DefaultFontDownloadSetting", 2)


def _remove_disable_font_download_edge(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_EDGE_POLICY, "DefaultFontDownloadSetting")


def _detect_disable_font_download_edge() -> bool:
    return (
        SESSION.read_dword(_KEY_EDGE_POLICY, "DefaultFontDownloadSetting") == 2
    )


# ── 6. Block Untrusted Fonts ────────────────────────────────────────────────

# MitigationOptions bit field — setting value to block untrusted fonts.
# Value 1000000000000 (hex) = audit mode; 2000000000000 = block mode.
_BLOCK_UNTRUSTED_FONTS_VALUE = "2000000000000"


def _apply_block_untrusted_fonts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: block untrusted font loading")
    SESSION.backup([_KEY_KERNEL], "UntrustedFonts")
    SESSION.set_string(
        _KEY_KERNEL, "MitigationOptions", _BLOCK_UNTRUSTED_FONTS_VALUE
    )


def _remove_block_untrusted_fonts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_KERNEL, "MitigationOptions")


def _detect_block_untrusted_fonts() -> bool:
    return (
        SESSION.read_string(_KEY_KERNEL, "MitigationOptions")
        == _BLOCK_UNTRUSTED_FONTS_VALUE
    )


# ── 7. Disable Font Cache Service (FontCache) ───────────────────────────────


def _apply_disable_fontcache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: disable Windows Font Cache Service")
    SESSION.backup([_KEY_FONTCACHE_SVC], "FontCacheService")
    SESSION.set_dword(_KEY_FONTCACHE_SVC, "Start", 4)  # 4 = Disabled


def _remove_disable_fontcache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_FONTCACHE_SVC, "Start", 2)  # 2 = Automatic


def _detect_disable_fontcache() -> bool:
    return SESSION.read_dword(_KEY_FONTCACHE_SVC, "Start") == 4


# ── 8. Disable Font Cache 3.0 Service ───────────────────────────────────────


def _apply_disable_fontcache3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: disable Windows Font Cache 3.0 Service")
    SESSION.backup([_KEY_FONTCACHE3_SVC], "FontCache3Service")
    SESSION.set_dword(_KEY_FONTCACHE3_SVC, "Start", 4)  # 4 = Disabled


def _remove_disable_fontcache3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_FONTCACHE3_SVC, "Start", 3)  # 3 = Manual


def _detect_disable_fontcache3() -> bool:
    return SESSION.read_dword(_KEY_FONTCACHE3_SVC, "Start") == 4


# ── 9. Set ClearType Tuning Level ───────────────────────────────────────────


def _apply_cleartype_tuning(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: set ClearType tuning level to maximum")
    SESSION.backup([_KEY_AVALON_DISPLAY1], "ClearTypeTuning")
    SESSION.set_dword(_KEY_AVALON_DISPLAY1, "ClearTypeLevel", 100)


def _remove_cleartype_tuning(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_AVALON_DISPLAY1, "ClearTypeLevel")


def _detect_cleartype_tuning() -> bool:
    return SESSION.read_dword(_KEY_AVALON_DISPLAY1, "ClearTypeLevel") == 100


# ── 10. Enable Natural ClearType Contrast ───────────────────────────────────


def _apply_natural_cleartype(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: set natural ClearType text contrast level")
    SESSION.backup([_KEY_AVALON_DISPLAY1], "NaturalClearType")
    SESSION.set_dword(_KEY_AVALON_DISPLAY1, "TextContrastLevel", 1)


def _remove_natural_cleartype(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_AVALON_DISPLAY1, "TextContrastLevel")


def _detect_natural_cleartype() -> bool:
    return SESSION.read_dword(_KEY_AVALON_DISPLAY1, "TextContrastLevel") == 1


# ── 11. Enable WPF Hardware-Accelerated Text Rendering ──────────────────────


def _apply_wpf_hw_text(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: enable WPF hardware-accelerated text rendering")
    SESSION.backup([_KEY_AVALON_GRAPHICS], "WpfHwText")
    SESSION.set_dword(_KEY_AVALON_GRAPHICS, "DisableHWAcceleration", 0)


def _remove_wpf_hw_text(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_AVALON_GRAPHICS, "DisableHWAcceleration")


def _detect_wpf_hw_text() -> bool:
    return (
        SESSION.read_dword(_KEY_AVALON_GRAPHICS, "DisableHWAcceleration") == 0
    )


# ── 12. Block Font Downloads in Internet Explorer Zones ─────────────────────


def _apply_block_ie_font_download(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: block font downloads in Internet zone")
    SESSION.backup([_KEY_IE_SECURITY], "IEFontDownload")
    SESSION.set_dword(_KEY_IE_SECURITY, "1604", 3)  # 3 = Disable


def _remove_block_ie_font_download(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_IE_SECURITY, "1604", 0)  # 0 = Enable


def _detect_block_ie_font_download() -> bool:
    return SESSION.read_dword(_KEY_IE_SECURITY, "1604") == 3


# ── 13. Disable Font Streaming ──────────────────────────────────────────────


def _apply_disable_font_streaming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: disable cloud font streaming")
    SESSION.backup([_KEY_FONT_PROVIDERS], "FontStreaming")
    SESSION.set_dword(_KEY_FONT_PROVIDERS, "EnableFontProviders", 0)


def _remove_disable_font_streaming(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_FONT_PROVIDERS, "EnableFontProviders")


def _detect_disable_font_streaming() -> bool:
    return SESSION.read_dword(_KEY_FONT_PROVIDERS, "EnableFontProviders") == 0


# ── 14. Set ClearType Gamma ─────────────────────────────────────────────────


def _apply_cleartype_gamma(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: set ClearType gamma to 2200")
    SESSION.backup([_KEY_AVALON_DISPLAY1], "ClearTypeGamma")
    SESSION.set_dword(_KEY_AVALON_DISPLAY1, "GammaLevel", 2200)


def _remove_cleartype_gamma(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_AVALON_DISPLAY1, "GammaLevel")


def _detect_cleartype_gamma() -> bool:
    return SESSION.read_dword(_KEY_AVALON_DISPLAY1, "GammaLevel") == 2200


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="font-enable-cleartype",
        label="Enable ClearType Font Rendering",
        category="Fonts",
        apply_fn=_apply_enable_cleartype,
        remove_fn=_remove_enable_cleartype,
        detect_fn=_detect_enable_cleartype,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Enables ClearType sub-pixel rendering for sharper text on LCD "
            "displays (sets FontSmoothingType to 2)."
        ),
        tags=["fonts", "cleartype", "rendering", "display"],
    ),
    TweakDef(
        id="font-enable-smoothing",
        label="Enable Font Smoothing",
        category="Fonts",
        apply_fn=_apply_enable_font_smoothing,
        remove_fn=_remove_enable_font_smoothing,
        detect_fn=_detect_enable_font_smoothing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Activates font smoothing at the system level so all text "
            "benefits from anti-aliased rendering."
        ),
        tags=["fonts", "smoothing", "display"],
    ),
    TweakDef(
        id="font-disable-antialiasing",
        label="Disable Font Antialiasing (Performance)",
        category="Fonts",
        apply_fn=_apply_disable_antialiasing,
        remove_fn=_remove_disable_antialiasing,
        detect_fn=_detect_disable_antialiasing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Turns off all font smoothing and antialiasing for a minor "
            "performance gain — text will appear jagged on LCD displays."
        ),
        tags=["fonts", "antialiasing", "performance"],
    ),
    TweakDef(
        id="font-set-segoe-ui",
        label="Set Default System Font to Segoe UI",
        category="Fonts",
        apply_fn=_apply_set_segoe_ui,
        remove_fn=_remove_set_segoe_ui,
        detect_fn=_detect_set_segoe_ui,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_USER_FONTS],
        description=(
            "Registers Segoe UI and its variants as the per-user default "
            "font, overriding any previous user-level font substitution."
        ),
        tags=["fonts", "segoe", "default", "system"],
    ),
    TweakDef(
        id="font-disable-download-edge",
        label="Disable Font Download in Edge",
        category="Fonts",
        apply_fn=_apply_disable_font_download_edge,
        remove_fn=_remove_disable_font_download_edge,
        detect_fn=_detect_disable_font_download_edge,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_EDGE_POLICY],
        description=(
            "Prevents Microsoft Edge from downloading web fonts via the "
            "DefaultFontDownloadSetting policy (value 2 = block)."
        ),
        tags=["fonts", "edge", "download", "policy", "security"],
    ),
    TweakDef(
        id="font-block-untrusted",
        label="Block Untrusted Font Loading",
        category="Fonts",
        apply_fn=_apply_block_untrusted_fonts,
        remove_fn=_remove_block_untrusted_fonts,
        detect_fn=_detect_block_untrusted_fonts,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_KERNEL],
        description=(
            "Blocks loading of untrusted fonts from user-writable "
            "locations via the kernel MitigationOptions flag — hardens "
            "the system against font-based exploits."
        ),
        tags=["fonts", "untrusted", "security", "mitigation", "kernel"],
    ),
    TweakDef(
        id="font-disable-fontcache-service",
        label="Disable Font Cache Service",
        category="Fonts",
        apply_fn=_apply_disable_fontcache,
        remove_fn=_remove_disable_fontcache,
        detect_fn=_detect_disable_fontcache,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FONTCACHE_SVC],
        description=(
            "Disables the Windows Font Cache Service (FontCache). "
            "May reduce memory usage but can slow down font loading."
        ),
        tags=["fonts", "cache", "service", "performance"],
    ),
    TweakDef(
        id="font-disable-fontcache3-service",
        label="Disable Font Cache 3.0 Service",
        category="Fonts",
        apply_fn=_apply_disable_fontcache3,
        remove_fn=_remove_disable_fontcache3,
        detect_fn=_detect_disable_fontcache3,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FONTCACHE3_SVC],
        description=(
            "Disables the Windows Presentation Foundation Font Cache 3.0 "
            "Service used by WPF applications."
        ),
        tags=["fonts", "cache", "wpf", "service"],
    ),
    TweakDef(
        id="font-cleartype-tuning",
        label="Set ClearType Tuning to Maximum",
        category="Fonts",
        apply_fn=_apply_cleartype_tuning,
        remove_fn=_remove_cleartype_tuning,
        detect_fn=_detect_cleartype_tuning,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_AVALON_DISPLAY1],
        description=(
            "Sets the ClearType rendering level to 100 (maximum) for "
            "WPF and Avalon-based applications on the primary display."
        ),
        tags=["fonts", "cleartype", "tuning", "wpf", "rendering"],
    ),
    TweakDef(
        id="font-natural-cleartype-contrast",
        label="Enable Natural ClearType Contrast",
        category="Fonts",
        apply_fn=_apply_natural_cleartype,
        remove_fn=_remove_natural_cleartype,
        detect_fn=_detect_natural_cleartype,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_AVALON_DISPLAY1],
        description=(
            "Sets the WPF text contrast level to 1 for a more natural, "
            "softer ClearType appearance on the primary display."
        ),
        tags=["fonts", "cleartype", "contrast", "wpf", "rendering"],
    ),
    TweakDef(
        id="font-wpf-hw-text-rendering",
        label="Enable WPF Hardware Text Rendering",
        category="Fonts",
        apply_fn=_apply_wpf_hw_text,
        remove_fn=_remove_wpf_hw_text,
        detect_fn=_detect_wpf_hw_text,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_AVALON_GRAPHICS],
        description=(
            "Ensures WPF applications use GPU-accelerated text rendering "
            "by explicitly setting DisableHWAcceleration to 0."
        ),
        tags=["fonts", "wpf", "gpu", "hardware", "rendering"],
    ),
    TweakDef(
        id="font-block-ie-zone-download",
        label="Block Font Downloads in Internet Zone",
        category="Fonts",
        apply_fn=_apply_block_ie_font_download,
        remove_fn=_remove_block_ie_font_download,
        detect_fn=_detect_block_ie_font_download,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_IE_SECURITY],
        description=(
            "Blocks downloading of fonts in the Internet security zone "
            "(Zone 3) via the 1604 policy value — prevents drive-by "
            "font-based exploits in legacy applications."
        ),
        tags=["fonts", "internet", "zone", "download", "security", "policy"],
    ),
    TweakDef(
        id="fonts-disable-streaming",
        label="Disable Font Streaming",
        category="Fonts",
        apply_fn=_apply_disable_font_streaming,
        remove_fn=_remove_disable_font_streaming,
        detect_fn=_detect_disable_font_streaming,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_FONT_PROVIDERS],
        description=(
            "Disables cloud font streaming from Microsoft. Prevents "
            "background font downloads. Reduces network traffic. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["fonts", "streaming", "network", "performance"],
    ),
    TweakDef(
        id="fonts-cleartype-performance",
        label="Set ClearType Gamma",
        category="Fonts",
        apply_fn=_apply_cleartype_gamma,
        remove_fn=_remove_cleartype_gamma,
        detect_fn=_detect_cleartype_gamma,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_AVALON_DISPLAY1],
        description=(
            "Optimizes ClearType font rendering gamma to 2200 for better "
            "readability on LCD displays. Default: 1800. "
            "Recommended: 2200."
        ),
        tags=["fonts", "cleartype", "rendering", "display"],
    ),
]


# ── Disable Font Fallback ────────────────────────────────────────────────────

_KEY_FONT_SUBSTITUTES = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes"


def _apply_disable_font_fallback(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Fonts: disable font fallback (MS Shell Dlg -> Segoe UI)")
    SESSION.backup([_KEY_FONT_SUBSTITUTES], "FontFallback")
    SESSION.set_string(_KEY_FONT_SUBSTITUTES, "MS Shell Dlg", "Segoe UI")
    SESSION.set_string(_KEY_FONT_SUBSTITUTES, "MS Shell Dlg 2", "Segoe UI")


def _remove_disable_font_fallback(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_KEY_FONT_SUBSTITUTES, "MS Shell Dlg", "Microsoft Sans Serif")
    SESSION.set_string(_KEY_FONT_SUBSTITUTES, "MS Shell Dlg 2", "Tahoma")


def _detect_disable_font_fallback() -> bool:
    return SESSION.read_string(_KEY_FONT_SUBSTITUTES, "MS Shell Dlg") == "Segoe UI"


# ── Disable Font Anti-Aliasing ───────────────────────────────────────────────


def _apply_disable_font_antialiasing(*, require_admin: bool = False) -> None:
    SESSION.log("Fonts: disable font anti-aliasing smoothing")
    SESSION.backup([_KEY_DESKTOP], "FontAntiAliasing")
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "0")


def _remove_disable_font_antialiasing(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_DESKTOP, "FontSmoothing", "2")


def _detect_disable_font_antialiasing() -> bool:
    return SESSION.read_string(_KEY_DESKTOP, "FontSmoothing") == "0"


TWEAKS += [
    TweakDef(
        id="fonts-disable-font-fallback",
        label="Disable Font Fallback",
        category="Fonts",
        apply_fn=_apply_disable_font_fallback,
        remove_fn=_remove_disable_font_fallback,
        detect_fn=_detect_disable_font_fallback,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FONT_SUBSTITUTES],
        description=(
            "Overrides MS Shell Dlg font fallback to Segoe UI for consistent "
            "rendering across legacy and modern applications. "
            "Default: Microsoft Sans Serif. Recommended: Segoe UI."
        ),
        tags=["fonts", "fallback", "substitutes", "rendering"],
    ),
    TweakDef(
        id="fonts-disable-font-antialiasing",
        label="Disable Font Anti-Aliasing",
        category="Fonts",
        apply_fn=_apply_disable_font_antialiasing,
        remove_fn=_remove_disable_font_antialiasing,
        detect_fn=_detect_disable_font_antialiasing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DESKTOP],
        description=(
            "Disables font smoothing/anti-aliasing for sharper pixel-aligned text. "
            "May improve readability on low-DPI screens. "
            "Default: 2 (enabled). Recommended: Disabled for CRT/low-DPI."
        ),
        tags=["fonts", "antialiasing", "smoothing", "rendering"],
    ),
]
