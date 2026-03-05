"""Accessibility & visual comfort registry tweaks.

Covers: Sticky Keys suppression, dark mode, mouse pointer size,
animations, font smoothing, and scroll bar width.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_ACCESS = r"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"
_TOGGLE = r"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"
_FILTER = r"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response"
_THEME = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"
_DESKTOP = r"HKEY_CURRENT_USER\Control Panel\Desktop"
_WINDOW_METRICS = r"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"
_DWM = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"
_NARRATOR = r"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"
_HIGHCONTRAST = r"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast"
_MAGNIFIER = r"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier"
_MAGNIFIER_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"
_OSK = r"HKEY_CURRENT_USER\Software\Microsoft\Osk"
_OSK_TABLET = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode"
_KB_PREF = r"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference"
_SOUNDSENTRY = r"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry"


# ── Disable Sticky/Toggle/Filter Keys Popups ────────────────────────────────


def _apply_disable_accessibility_shortcuts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable Sticky/Toggle/Filter key shortcuts")
    SESSION.backup([_ACCESS, _TOGGLE, _FILTER], "AccessibilityShortcuts")
    # Flags: 506 = off, 58 = off (shiftx5, NumLock, etc.)
    SESSION.set_string(_ACCESS, "Flags", "506")
    SESSION.set_string(_TOGGLE, "Flags", "58")
    SESSION.set_string(_FILTER, "Flags", "122")


def _remove_disable_accessibility_shortcuts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_ACCESS, "Flags", "510")  # default
    SESSION.set_string(_TOGGLE, "Flags", "62")
    SESSION.set_string(_FILTER, "Flags", "126")


def _detect_disable_accessibility_shortcuts() -> bool:
    return SESSION.read_string(_ACCESS, "Flags") == "506"


# ── Force Dark Mode (Apps + System) ─────────────────────────────────────────


def _apply_dark_mode(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: enable system-wide dark mode")
    SESSION.backup([_THEME], "DarkMode")
    SESSION.set_dword(_THEME, "AppsUseLightTheme", 0)
    SESSION.set_dword(_THEME, "SystemUsesLightTheme", 0)


def _remove_dark_mode(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_THEME, "AppsUseLightTheme", 1)
    SESSION.set_dword(_THEME, "SystemUsesLightTheme", 1)


def _detect_dark_mode() -> bool:
    return SESSION.read_dword(_THEME, "AppsUseLightTheme") == 0


# ── Disable Window Animations ───────────────────────────────────────────────


def _apply_disable_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable window animations")
    SESSION.backup([_DWM, _DESKTOP], "Animations")
    SESSION.set_dword(_DWM, "EnableAeroPeek", 0)
    SESSION.set_string(_DESKTOP, "UserPreferencesMask", "90 12 03 80 10 00 00 00")


def _remove_disable_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DWM, "EnableAeroPeek", 1)
    SESSION.set_string(_DESKTOP, "UserPreferencesMask", "9E 3E 07 80 12 01 00 00")


def _detect_disable_animations() -> bool:
    return SESSION.read_dword(_DWM, "EnableAeroPeek") == 0


# ── Enable ClearType Font Smoothing ──────────────────────────────────────────


def _apply_cleartype(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: enable ClearType font smoothing")
    SESSION.backup([_DESKTOP], "ClearType")
    SESSION.set_string(_DESKTOP, "FontSmoothing", "2")
    SESSION.set_dword(_DESKTOP, "FontSmoothingType", 2)  # 2 = ClearType


def _remove_cleartype(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_DESKTOP, "FontSmoothing", "0")
    SESSION.set_dword(_DESKTOP, "FontSmoothingType", 0)


def _detect_cleartype() -> bool:
    return SESSION.read_string(_DESKTOP, "FontSmoothing") == "2"


# ── Increase Scroll Bar Width ────────────────────────────────────────────────


def _apply_wide_scrollbar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: increase scroll bar width to -400 (25px)")
    SESSION.backup([_WINDOW_METRICS], "ScrollBarWidth")
    SESSION.set_string(_WINDOW_METRICS, "ScrollWidth", "-400")
    SESSION.set_string(_WINDOW_METRICS, "ScrollHeight", "-400")


def _remove_wide_scrollbar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_WINDOW_METRICS, "ScrollWidth", "-255")  # default
    SESSION.set_string(_WINDOW_METRICS, "ScrollHeight", "-255")


def _detect_wide_scrollbar() -> bool:
    return SESSION.read_string(_WINDOW_METRICS, "ScrollWidth") == "-400"


# ── Disable Narrator at Login ───────────────────────────────────────────────


def _apply_disable_narrator(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable Narrator auto-start")
    SESSION.backup([_NARRATOR], "Narrator")
    SESSION.set_dword(_NARRATOR, "WinEnterLaunchEnabled", 0)
    SESSION.set_dword(_NARRATOR, "RunNarratorAtLogon", 0)


def _remove_disable_narrator(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NARRATOR, "WinEnterLaunchEnabled")
    SESSION.delete_value(_NARRATOR, "RunNarratorAtLogon")


def _detect_disable_narrator() -> bool:
    return SESSION.read_dword(_NARRATOR, "WinEnterLaunchEnabled") == 0


# ── Enable High Contrast Mode ───────────────────────────────────────────────


def _apply_high_contrast(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: enable high contrast mode")
    SESSION.backup([_HIGHCONTRAST], "HighContrast")
    SESSION.set_string(_HIGHCONTRAST, "Flags", "127")


def _remove_high_contrast(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_HIGHCONTRAST, "Flags", "126")  # default off


def _detect_high_contrast() -> bool:
    return SESSION.read_string(_HIGHCONTRAST, "Flags") == "127"


# ── Disable Narrator Win+Enter Hotkey ────────────────────────────────────────


def _apply_disable_narrator_hotkey(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable Narrator Win+Enter hotkey")
    SESSION.backup([_NARRATOR], "NarratorHotkey")
    SESSION.set_dword(_NARRATOR, "WinEnterLaunchEnabled", 0)


def _remove_disable_narrator_hotkey(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NARRATOR, "WinEnterLaunchEnabled", 1)


def _detect_disable_narrator_hotkey() -> bool:
    return SESSION.read_dword(_NARRATOR, "WinEnterLaunchEnabled") == 0


# ── Disable Magnifier Win+Plus Hotkey ────────────────────────────────────────


def _apply_disable_magnifier_hotkey(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable Magnifier Win+Plus hotkey")
    SESSION.backup([_MAGNIFIER, _MAGNIFIER_POLICY], "MagnifierHotkey")
    SESSION.set_dword(_MAGNIFIER, "RunningState", 0)
    SESSION.set_dword(_MAGNIFIER_POLICY, "DisableMagnifier", 1)


def _remove_disable_magnifier_hotkey(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MAGNIFIER_POLICY, "DisableMagnifier")
    SESSION.set_dword(_MAGNIFIER, "RunningState", 1)


def _detect_disable_magnifier_hotkey() -> bool:
    return (
        SESSION.read_dword(_MAGNIFIER, "RunningState") == 0
        and SESSION.read_dword(_MAGNIFIER_POLICY, "DisableMagnifier") == 1
    )


# ── Disable On-Screen Keyboard Auto-Launch ───────────────────────────────────


def _apply_disable_osk_auto_launch(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable On-Screen Keyboard auto-launch")
    SESSION.backup([_OSK, _OSK_TABLET], "OskAutoLaunch")
    SESSION.set_dword(_OSK, "ShowStartupPanel", 0)
    SESSION.set_dword(_OSK_TABLET, "OpenStandby", 0)


def _remove_disable_osk_auto_launch(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_OSK, "ShowStartupPanel", 1)
    SESSION.set_dword(_OSK_TABLET, "OpenStandby", 1)


def _detect_disable_osk_auto_launch() -> bool:
    return SESSION.read_dword(_OSK, "ShowStartupPanel") == 0


# ── Disable Menu Access Key Underlines ───────────────────────────────────────


def _apply_disable_underline_shortcuts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable menu access key underlines")
    SESSION.backup([_KB_PREF], "UnderlineShortcuts")
    SESSION.set_string(_KB_PREF, "On", "0")


def _remove_disable_underline_shortcuts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_KB_PREF, "On", "1")


def _detect_disable_underline_shortcuts() -> bool:
    return SESSION.read_string(_KB_PREF, "On") == "0"


# ── Disable Visual Sound Alerts ──────────────────────────────────────────────


def _apply_disable_sound_sentry(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable visual sound alerts (SoundSentry)")
    SESSION.backup([_SOUNDSENTRY], "SoundSentry")
    SESSION.set_string(_SOUNDSENTRY, "Flags", "0")


def _remove_disable_sound_sentry(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_SOUNDSENTRY, "Flags", "2")


def _detect_disable_sound_sentry() -> bool:
    return SESSION.read_string(_SOUNDSENTRY, "Flags") == "0"


# ── Disable Narrator Autostart ───────────────────────────────────────────────


def _apply_disable_narrator_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable Narrator autostart (Win+Enter)")
    SESSION.backup([_NARRATOR], "NarratorAutostart")
    SESSION.set_dword(_NARRATOR, "WinEnterLaunchEnabled", 0)


def _remove_disable_narrator_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NARRATOR, "WinEnterLaunchEnabled", 1)


def _detect_disable_narrator_autostart() -> bool:
    return SESSION.read_dword(_NARRATOR, "WinEnterLaunchEnabled") == 0


# ── Disable Magnifier Lens Mode ──────────────────────────────────────────────


def _apply_disable_magnifier_lens(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: set Magnifier to docked mode")
    SESSION.backup([_MAGNIFIER], "MagnifierLens")
    SESSION.set_dword(_MAGNIFIER, "MagnificationMode", 0)


def _remove_disable_magnifier_lens(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MAGNIFIER, "MagnificationMode", 2)


def _detect_disable_magnifier_lens() -> bool:
    return SESSION.read_dword(_MAGNIFIER, "MagnificationMode") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-accessibility-shortcuts",
        label="Disable Sticky/Toggle/Filter Keys",
        category="Accessibility",
        apply_fn=_apply_disable_accessibility_shortcuts,
        remove_fn=_remove_disable_accessibility_shortcuts,
        detect_fn=_detect_disable_accessibility_shortcuts,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ACCESS, _TOGGLE, _FILTER],
        description=(
            "Suppresses the Sticky Keys, Toggle Keys, and Filter Keys "
            "popups triggered by repeated key presses."
        ),
        tags=["accessibility", "keyboard", "gaming"],
    ),
    TweakDef(
        id="force-dark-mode",
        label="Force System-Wide Dark Mode",
        category="Accessibility",
        apply_fn=_apply_dark_mode,
        remove_fn=_remove_dark_mode,
        detect_fn=_detect_dark_mode,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_THEME],
        description=(
            "Enables dark mode for both Windows system chrome and "
            "applications via the Personalize theme registry."
        ),
        tags=["accessibility", "theme", "dark-mode"],
    ),
    TweakDef(
        id="disable-animations",
        label="Disable Window Animations",
        category="Accessibility",
        apply_fn=_apply_disable_animations,
        remove_fn=_remove_disable_animations,
        detect_fn=_detect_disable_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DWM, _DESKTOP],
        description=(
            "Disables desktop window animations (Aero Peek, minimize/"
            "maximize effects) for snappier UI."
        ),
        tags=["accessibility", "performance", "animation"],
    ),
    TweakDef(
        id="enable-cleartype",
        label="Enable ClearType Font Smoothing",
        category="Accessibility",
        apply_fn=_apply_cleartype,
        remove_fn=_remove_cleartype,
        detect_fn=_detect_cleartype,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DESKTOP],
        description="Enables ClearType sub-pixel font rendering for sharper text.",
        tags=["accessibility", "font", "display"],
    ),
    TweakDef(
        id="wide-scrollbar",
        label="Increase Scroll Bar Width",
        category="Accessibility",
        apply_fn=_apply_wide_scrollbar,
        remove_fn=_remove_wide_scrollbar,
        detect_fn=_detect_wide_scrollbar,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WINDOW_METRICS],
        description=(
            "Increases scroll bar width from default (17px) to 25px "
            "for easier targeting with mouse or touch."
        ),
        tags=["accessibility", "ui", "scrollbar"],
    ),
    TweakDef(
        id="disable-narrator",
        label="Disable Narrator Auto-Start",
        category="Accessibility",
        apply_fn=_apply_disable_narrator,
        remove_fn=_remove_disable_narrator,
        detect_fn=_detect_disable_narrator,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR],
        description="Prevents Narrator from starting at login or via Win+Enter.",
        tags=["accessibility", "narrator", "screen-reader"],
    ),
    TweakDef(
        id="high-contrast-mode",
        label="Enable High Contrast Mode",
        category="Accessibility",
        apply_fn=_apply_high_contrast,
        remove_fn=_remove_high_contrast,
        detect_fn=_detect_high_contrast,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HIGHCONTRAST],
        description="Enables high contrast mode for improved visual clarity.",
        tags=["accessibility", "contrast", "display"],
    ),
    TweakDef(
        id="disable-narrator-hotkey",
        label="Disable Narrator Win+Enter Hotkey",
        category="Accessibility",
        apply_fn=_apply_disable_narrator_hotkey,
        remove_fn=_remove_disable_narrator_hotkey,
        detect_fn=_detect_disable_narrator_hotkey,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR],
        description=(
            "Disables the Win+Enter hotkey that launches Narrator, "
            "preventing accidental activation."
        ),
        tags=["accessibility", "narrator", "hotkey"],
    ),
    TweakDef(
        id="disable-magnifier-hotkey",
        label="Disable Magnifier Win+Plus Hotkey",
        category="Accessibility",
        apply_fn=_apply_disable_magnifier_hotkey,
        remove_fn=_remove_disable_magnifier_hotkey,
        detect_fn=_detect_disable_magnifier_hotkey,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MAGNIFIER, _MAGNIFIER_POLICY],
        description=(
            "Disables the Win+Plus hotkey that launches Magnifier "
            "and prevents it from running."
        ),
        tags=["accessibility", "magnifier", "hotkey"],
    ),
    TweakDef(
        id="disable-osk-auto-launch",
        label="Disable On-Screen Keyboard Auto-Launch",
        category="Accessibility",
        apply_fn=_apply_disable_osk_auto_launch,
        remove_fn=_remove_disable_osk_auto_launch,
        detect_fn=_detect_disable_osk_auto_launch,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_OSK, _OSK_TABLET],
        description=(
            "Prevents the On-Screen Keyboard from automatically launching "
            "at startup or when entering tablet mode."
        ),
        tags=["accessibility", "keyboard", "osk", "tablet"],
    ),
    TweakDef(
        id="disable-underline-shortcuts",
        label="Disable Menu Access Key Underlines",
        category="Accessibility",
        apply_fn=_apply_disable_underline_shortcuts,
        remove_fn=_remove_disable_underline_shortcuts,
        detect_fn=_detect_disable_underline_shortcuts,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KB_PREF],
        description=(
            "Disables the underline indicators on menu access keys "
            "(keyboard shortcuts) for a cleaner UI."
        ),
        tags=["accessibility", "keyboard", "menu", "ui"],
    ),
    TweakDef(
        id="disable-sound-sentry",
        label="Disable Visual Sound Alerts",
        category="Accessibility",
        apply_fn=_apply_disable_sound_sentry,
        remove_fn=_remove_disable_sound_sentry,
        detect_fn=_detect_disable_sound_sentry,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SOUNDSENTRY],
        description=(
            "Disables SoundSentry visual alerts that flash the screen "
            "or window when a system sound plays."
        ),
        tags=["accessibility", "sound", "visual-alert"],
    ),
    TweakDef(
        id="access-disable-narrator-autostart",
        label="Disable Narrator Autostart",
        category="Accessibility",
        apply_fn=_apply_disable_narrator_autostart,
        remove_fn=_remove_disable_narrator_autostart,
        detect_fn=_detect_disable_narrator_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR],
        description=(
            "Prevents Narrator from launching with Win+Enter shortcut. "
            "Default: Enabled. Recommended: Disabled if not needed."
        ),
        tags=["accessibility", "narrator", "shortcut"],
    ),
    TweakDef(
        id="access-disable-magnifier",
        label="Disable Magnifier Lens Mode",
        category="Accessibility",
        apply_fn=_apply_disable_magnifier_lens,
        remove_fn=_remove_disable_magnifier_lens,
        detect_fn=_detect_disable_magnifier_lens,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MAGNIFIER],
        description=(
            "Sets Magnifier to docked mode (least intrusive). Prevents "
            "fullscreen magnification from activating accidentally. "
            "Default: Fullscreen (2). Recommended: Docked (0)."
        ),
        tags=["accessibility", "magnifier", "ux"],
    ),
]
