"""Accessibility & visual comfort registry tweaks.

Covers: Sticky Keys suppression, dark mode, mouse pointer size,
animations, font smoothing, and scroll bar width.
"""

from __future__ import annotations

from typing import List

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


# ── Disable Sticky/Toggle/Filter Keys Popups ────────────────────────────────


def _apply_disable_accessibility_shortcuts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Accessibility: disable Sticky/Toggle/Filter key shortcuts")
    SESSION.backup([_ACCESS, _TOGGLE, _FILTER], "AccessibilityShortcuts")
    # Flags: 506 = off, 58 = off (shift×5, NumLock, etc.)
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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
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
]
