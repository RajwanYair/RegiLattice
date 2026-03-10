"""Touch & Pen tweaks — Stylus, pen, touch gestures, Ink Workspace.

Covers: Touch input settings, pen shortcuts, palm rejection, Ink Workspace,
handwriting personalisation, and precision touchpad gestures.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_TOUCH = r"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch"
_PEN_USER = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"
_INK_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"
_HANDWRITING = r"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization"
_TOUCHPAD = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"
_PEN_SHORTCUT = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen"
_TABLET_MODE = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell"
_TOUCH_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC"
_GESTURE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"
_FLICKS = r"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters"


# ── Disable Touch Visual Feedback ────────────────────────────────────────────


def _apply_disable_touch_visual(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable touch visual feedback (ripples)")
    SESSION.backup([_TOUCH], "TouchVisual")
    SESSION.set_dword(_TOUCH, "TouchGate", 0)


def _remove_disable_touch_visual(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCH, "TouchGate", 1)


def _detect_disable_touch_visual() -> bool:
    return SESSION.read_dword(_TOUCH, "TouchGate") == 0


# ── Disable Ink Workspace ───────────────────────────────────────────────────


def _apply_disable_ink_workspace(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Touch & Pen: disable Windows Ink Workspace via policy")
    SESSION.backup([_INK_POLICY], "InkWorkspace")
    SESSION.set_dword(_INK_POLICY, "AllowWindowsInkWorkspace", 0)


def _remove_disable_ink_workspace(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_INK_POLICY, "AllowWindowsInkWorkspace", 1)


def _detect_disable_ink_workspace() -> bool:
    return SESSION.read_dword(_INK_POLICY, "AllowWindowsInkWorkspace") == 0


# ── Disable Pen Button Shortcut ──────────────────────────────────────────────


def _apply_disable_pen_button(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable pen button shortcut (no Ink Workspace launch)")
    SESSION.backup([_PEN_USER], "PenButton")
    SESSION.set_dword(_PEN_USER, "PenWorkspaceButtonDesiredVisibility", 0)


def _remove_disable_pen_button(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PEN_USER, "PenWorkspaceButtonDesiredVisibility", 1)


def _detect_disable_pen_button() -> bool:
    return SESSION.read_dword(_PEN_USER, "PenWorkspaceButtonDesiredVisibility") == 0


# ── Disable Handwriting Personalisation ──────────────────────────────────────


def _apply_disable_handwriting(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable handwriting personalisation data collection")
    SESSION.backup([_HANDWRITING], "Handwriting")
    SESSION.set_dword(_HANDWRITING, "RestrictImplicitInkCollection", 1)


def _remove_disable_handwriting(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_HANDWRITING, "RestrictImplicitInkCollection", 0)


def _detect_disable_handwriting() -> bool:
    return SESSION.read_dword(_HANDWRITING, "RestrictImplicitInkCollection") == 1


# ── Disable Ink Workspace Suggestions ────────────────────────────────────────


def _apply_disable_ink_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Touch & Pen: disable Ink Workspace suggested apps")
    SESSION.backup([_INK_POLICY], "InkSuggestions")
    SESSION.set_dword(_INK_POLICY, "AllowSuggestedAppsInWindowsInkWorkspace", 0)


def _remove_disable_ink_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_INK_POLICY, "AllowSuggestedAppsInWindowsInkWorkspace")


def _detect_disable_ink_suggestions() -> bool:
    return SESSION.read_dword(_INK_POLICY, "AllowSuggestedAppsInWindowsInkWorkspace") == 0


# ── Set Pen Double-Click to Screenshot ───────────────────────────────────────


def _apply_pen_screenshot(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: set pen double-click to Screen Sketch")
    SESSION.backup([_PEN_SHORTCUT], "PenDoubleClick")
    SESSION.set_dword(_PEN_SHORTCUT, "DoubleClickAction", 4)


def _remove_pen_screenshot(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PEN_SHORTCUT, "DoubleClickAction", 0)


def _detect_pen_screenshot() -> bool:
    return SESSION.read_dword(_PEN_SHORTCUT, "DoubleClickAction") == 4


# ── Set Pen Long-Press to Ink Workspace ──────────────────────────────────────


def _apply_pen_longpress(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: set pen long-press to Ink Workspace")
    SESSION.backup([_PEN_SHORTCUT], "PenLongPress")
    SESSION.set_dword(_PEN_SHORTCUT, "LongPressAction", 3)


def _remove_pen_longpress(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PEN_SHORTCUT, "LongPressAction", 0)


def _detect_pen_longpress() -> bool:
    return SESSION.read_dword(_PEN_SHORTCUT, "LongPressAction") == 3


# ── Disable Tablet Mode Auto-Switch ─────────────────────────────────────────


def _apply_disable_tablet_auto(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable automatic tablet mode switching")
    SESSION.backup([_TABLET_MODE], "TabletAuto")
    SESSION.set_dword(_TABLET_MODE, "SignInMode", 1)
    SESSION.set_dword(_TABLET_MODE, "ConvertibleSlateModePromptPreference", 0)


def _remove_disable_tablet_auto(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_TABLET_MODE, "SignInMode")
    SESSION.delete_value(_TABLET_MODE, "ConvertibleSlateModePromptPreference")


def _detect_disable_tablet_auto() -> bool:
    return SESSION.read_dword(_TABLET_MODE, "SignInMode") == 1


# ── Disable Touch Handwriting Panel ─────────────────────────────────────────


def _apply_disable_handwriting_panel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Touch & Pen: disable touch handwriting panel via policy")
    SESSION.backup([_TOUCH_POLICY], "HandwritingPanel")
    SESSION.set_dword(_TOUCH_POLICY, "DisableHandwritingPanel", 1)


def _remove_disable_handwriting_panel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TOUCH_POLICY, "DisableHandwritingPanel")


def _detect_disable_handwriting_panel() -> bool:
    return SESSION.read_dword(_TOUCH_POLICY, "DisableHandwritingPanel") == 1


# ── Disable Precision Touchpad Three-Finger Gestures ────────────────────────


def _apply_disable_3finger(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable three-finger touchpad gestures")
    SESSION.backup([_TOUCHPAD], "ThreeFinger")
    SESSION.set_dword(_TOUCHPAD, "ThreeFingerSlideEnabled", 0)
    SESSION.set_dword(_TOUCHPAD, "ThreeFingerTapEnabled", 0)


def _remove_disable_3finger(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "ThreeFingerSlideEnabled", 1)
    SESSION.set_dword(_TOUCHPAD, "ThreeFingerTapEnabled", 1)


def _detect_disable_3finger() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "ThreeFingerSlideEnabled") == 0


# ── Disable Precision Touchpad Four-Finger Gestures ─────────────────────────


def _apply_disable_4finger(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable four-finger touchpad gestures")
    SESSION.backup([_TOUCHPAD], "FourFinger")
    SESSION.set_dword(_TOUCHPAD, "FourFingerSlideEnabled", 0)
    SESSION.set_dword(_TOUCHPAD, "FourFingerTapEnabled", 0)


def _remove_disable_4finger(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "FourFingerSlideEnabled", 1)
    SESSION.set_dword(_TOUCHPAD, "FourFingerTapEnabled", 1)


def _detect_disable_4finger() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "FourFingerSlideEnabled") == 0


# ── Disable Edge Swipe Gesture ──────────────────────────────────────────────


def _apply_disable_edge_swipe(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Touch & Pen: disable edge swipe gesture via policy")
    SESSION.backup([_GESTURE_POLICY], "EdgeSwipe")
    SESSION.set_dword(_GESTURE_POLICY, "DisableSwipe", 1)


def _remove_disable_edge_swipe(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GESTURE_POLICY, "DisableSwipe")


def _detect_disable_edge_swipe() -> bool:
    return SESSION.read_dword(_GESTURE_POLICY, "DisableSwipe") == 1


# ── Disable Pen Flicks ──────────────────────────────────────────────────────


def _apply_disable_flicks(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable pen flick gestures")
    SESSION.backup([_FLICKS], "PenFlicks")
    SESSION.set_dword(_FLICKS, "FlicksEnabled", 0)


def _remove_disable_flicks(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_FLICKS, "FlicksEnabled", 1)


def _detect_disable_flicks() -> bool:
    return SESSION.read_dword(_FLICKS, "FlicksEnabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="touch-disable-visual-feedback",
        label="Disable Touch Visual Feedback",
        category="Touch & Pen",
        apply_fn=_apply_disable_touch_visual,
        remove_fn=_remove_disable_touch_visual,
        detect_fn=_detect_disable_touch_visual,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCH],
        description="Removes the ripple animation when touching the screen. Reduces visual clutter on touch devices.",
        tags=["touch", "pen", "visual", "feedback"],
    ),
    TweakDef(
        id="touch-disable-ink-workspace",
        label="Disable Windows Ink Workspace",
        category="Touch & Pen",
        apply_fn=_apply_disable_ink_workspace,
        remove_fn=_remove_disable_ink_workspace,
        detect_fn=_detect_disable_ink_workspace,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_INK_POLICY],
        description="Disables the Windows Ink Workspace via Group Policy. Hides the Ink Workspace button and features.",
        tags=["touch", "pen", "ink", "workspace", "policy"],
    ),
    TweakDef(
        id="touch-disable-pen-button",
        label="Hide Pen Workspace Taskbar Button",
        category="Touch & Pen",
        apply_fn=_apply_disable_pen_button,
        remove_fn=_remove_disable_pen_button,
        detect_fn=_detect_disable_pen_button,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PEN_USER],
        description="Hides the Windows Ink Workspace button from the taskbar. Pen still works; only the shortcut button is hidden.",
        tags=["touch", "pen", "taskbar", "button"],
    ),
    TweakDef(
        id="touch-disable-handwriting",
        label="Disable Handwriting Personalisation",
        category="Touch & Pen",
        apply_fn=_apply_disable_handwriting,
        remove_fn=_remove_disable_handwriting,
        detect_fn=_detect_disable_handwriting,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_HANDWRITING],
        description="Stops Windows from collecting handwriting and inking data for personalisation. Recommended: Disabled for privacy.",
        tags=["touch", "pen", "handwriting", "privacy"],
    ),
    TweakDef(
        id="touch-disable-ink-suggestions",
        label="Disable Ink Work Suggested Apps",
        category="Touch & Pen",
        apply_fn=_apply_disable_ink_suggestions,
        remove_fn=_remove_disable_ink_suggestions,
        detect_fn=_detect_disable_ink_suggestions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_INK_POLICY],
        description="Removes suggested apps from the Windows Ink Workspace. Policy setting.",
        tags=["touch", "pen", "ink", "suggestions", "ads"],
    ),
    TweakDef(
        id="touch-pen-screenshot",
        label="Pen Double-Click: Screen Sketch",
        category="Touch & Pen",
        apply_fn=_apply_pen_screenshot,
        remove_fn=_remove_pen_screenshot,
        detect_fn=_detect_pen_screenshot,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PEN_SHORTCUT],
        description="Maps pen button double-click to Screen Sketch (screenshot annotation). Default: Nothing.",
        tags=["touch", "pen", "screenshot", "shortcut"],
    ),
    TweakDef(
        id="touch-pen-longpress",
        label="Pen Long-Press: Ink Workspace",
        category="Touch & Pen",
        apply_fn=_apply_pen_longpress,
        remove_fn=_remove_pen_longpress,
        detect_fn=_detect_pen_longpress,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PEN_SHORTCUT],
        description="Maps pen button long-press to open the Ink Workspace. Default: Nothing.",
        tags=["touch", "pen", "workspace", "shortcut"],
    ),
    TweakDef(
        id="touch-disable-tablet-auto",
        label="Disable Tablet Mode Auto-Switch",
        category="Touch & Pen",
        apply_fn=_apply_disable_tablet_auto,
        remove_fn=_remove_disable_tablet_auto,
        detect_fn=_detect_disable_tablet_auto,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TABLET_MODE],
        description="Prevents Windows from switching to tablet mode when a keyboard is detached or folded.",
        tags=["touch", "tablet", "mode", "convertible"],
    ),
    TweakDef(
        id="touch-disable-handwriting-panel",
        label="Disable Touch Handwriting Panel",
        category="Touch & Pen",
        apply_fn=_apply_disable_handwriting_panel,
        remove_fn=_remove_disable_handwriting_panel,
        detect_fn=_detect_disable_handwriting_panel,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TOUCH_POLICY],
        description="Disables the handwriting input panel that appears when tapping text fields with a pen. Policy setting.",
        tags=["touch", "pen", "handwriting", "panel", "policy"],
    ),
    TweakDef(
        id="touch-disable-3finger",
        label="Disable Three-Finger Gestures",
        category="Touch & Pen",
        apply_fn=_apply_disable_3finger,
        remove_fn=_remove_disable_3finger,
        detect_fn=_detect_disable_3finger,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description="Disables three-finger tap and slide gestures on precision touchpads (task view, volume, etc.).",
        tags=["touch", "touchpad", "gestures", "three-finger"],
    ),
    TweakDef(
        id="touch-disable-4finger",
        label="Disable Four-Finger Gestures",
        category="Touch & Pen",
        apply_fn=_apply_disable_4finger,
        remove_fn=_remove_disable_4finger,
        detect_fn=_detect_disable_4finger,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description="Disables four-finger tap and slide gestures on precision touchpads (desktop switch, etc.).",
        tags=["touch", "touchpad", "gestures", "four-finger"],
    ),
    TweakDef(
        id="touch-disable-edge-swipe",
        label="Disable Edge Swipe Gesture (Policy)",
        category="Touch & Pen",
        apply_fn=_apply_disable_edge_swipe,
        remove_fn=_remove_disable_edge_swipe,
        detect_fn=_detect_disable_edge_swipe,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GESTURE_POLICY],
        description="Disables the screen-edge swipe gesture that opens Action Centre / notification pane. Prevents accidental triggers.",
        tags=["touch", "edge", "swipe", "gesture", "policy"],
    ),
    TweakDef(
        id="touch-disable-flicks",
        label="Disable Pen Flick Gestures",
        category="Touch & Pen",
        apply_fn=_apply_disable_flicks,
        remove_fn=_remove_disable_flicks,
        detect_fn=_detect_disable_flicks,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FLICKS],
        description="Disables pen flick gestures (quick strokes for scroll, back, forward). Prevents accidental navigation.",
        tags=["touch", "pen", "flicks", "gestures"],
    ),
]


# ── Disable Touchpad Two-Finger Tap (Right-Click) ────────────────────────────


def _apply_disable_2finger_tap(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable two-finger tap right-click gesture")
    SESSION.backup([_TOUCHPAD], "TwoFingerTap")
    SESSION.set_dword(_TOUCHPAD, "TwoFingerTapEnabled", 0)


def _remove_disable_2finger_tap(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "TwoFingerTapEnabled", 1)


def _detect_disable_2finger_tap() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "TwoFingerTapEnabled") == 0


# ── Disable Touchpad Zoom Gesture ────────────────────────────────────────────


def _apply_disable_pinch_zoom(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable touchpad pinch-to-zoom gesture")
    SESSION.backup([_TOUCHPAD], "ZoomEnabled")
    SESSION.set_dword(_TOUCHPAD, "ZoomEnabled", 0)


def _remove_disable_pinch_zoom(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "ZoomEnabled", 1)


def _detect_disable_pinch_zoom() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "ZoomEnabled") == 0


# ── Enable Reverse Scrolling (Natural Scroll) ────────────────────────────────


def _apply_reverse_scroll(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: enable reverse (natural) touchpad scrolling")
    SESSION.backup([_TOUCHPAD], "ScrollDirection")
    SESSION.set_dword(_TOUCHPAD, "ScrollDirection", 0)  # 0 = reversed (natural), 1 = traditional


def _remove_reverse_scroll(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "ScrollDirection", 1)


def _detect_reverse_scroll() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "ScrollDirection") == 0


# ── Set Touchpad Sensitivity to High ─────────────────────────────────────────


def _apply_touchpad_sensitivity_high(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: set touchpad sensitivity to high")
    SESSION.backup([_TOUCHPAD], "TouchpadSensitivity")
    SESSION.set_dword(_TOUCHPAD, "AAPThreshold", 0)  # 0=most sensitive


def _remove_touchpad_sensitivity_high(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "AAPThreshold", 2)  # default


def _detect_touchpad_sensitivity_high() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "AAPThreshold") == 0


# ── Disable Palm Rejection ────────────────────────────────────────────────────

_PALM = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"


def _apply_disable_palm_rejection(*, require_admin: bool = False) -> None:
    SESSION.log("Touch & Pen: disable palm rejection on touchpad")
    SESSION.backup([_PALM], "PalmRejection")
    SESSION.set_dword(_PALM, "LeaveOnEnabled", 0)  # 0 = disable palm+leave detection


def _remove_disable_palm_rejection(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PALM, "LeaveOnEnabled", 1)


def _detect_disable_palm_rejection() -> bool:
    return SESSION.read_dword(_PALM, "LeaveOnEnabled") == 0


TWEAKS += [
    TweakDef(
        id="touch-disable-2finger-tap",
        label="Disable Two-Finger Tap (Right-Click)",
        category="Touch & Pen",
        apply_fn=_apply_disable_2finger_tap,
        remove_fn=_remove_disable_2finger_tap,
        detect_fn=_detect_disable_2finger_tap,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description=(
            "Disables two-finger tap as a right-click gesture on precision touchpads. "
            "Prevents accidental right-click menus while typing. Default: Enabled."
        ),
        tags=["touch", "touchpad", "two-finger", "right-click", "gesture"],
    ),
    TweakDef(
        id="touch-disable-pinch-zoom",
        label="Disable Touchpad Pinch-to-Zoom",
        category="Touch & Pen",
        apply_fn=_apply_disable_pinch_zoom,
        remove_fn=_remove_disable_pinch_zoom,
        detect_fn=_detect_disable_pinch_zoom,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description=(
            "Disables the two-finger pinch-to-zoom gesture on precision touchpads. "
            "Prevents accidental zoom changes. Default: Enabled."
        ),
        tags=["touch", "touchpad", "pinch", "zoom", "gesture"],
    ),
    TweakDef(
        id="touch-reverse-scroll",
        label="Enable Reverse (Natural) Scrolling",
        category="Touch & Pen",
        apply_fn=_apply_reverse_scroll,
        remove_fn=_remove_reverse_scroll,
        detect_fn=_detect_reverse_scroll,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description=(
            "Reverses touchpad scroll direction so content moves in the same direction as your fingers "
            "(natural/Mac-style scrolling). Default: Traditional."
        ),
        tags=["touch", "touchpad", "scroll", "direction", "natural"],
    ),
    TweakDef(
        id="touch-sensitivity-high",
        label="Set Touchpad Sensitivity to High",
        category="Touch & Pen",
        apply_fn=_apply_touchpad_sensitivity_high,
        remove_fn=_remove_touchpad_sensitivity_high,
        detect_fn=_detect_touchpad_sensitivity_high,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description=(
            "Sets precision touchpad sensitivity to maximum (AAPThreshold=0). "
            "Registers even the lightest touch. Default: Medium (2). Recommended: High for light-touch users."
        ),
        tags=["touch", "touchpad", "sensitivity", "threshold"],
    ),
    TweakDef(
        id="touch-disable-palm-rejection",
        label="Disable Touchpad Palm Rejection",
        category="Touch & Pen",
        apply_fn=_apply_disable_palm_rejection,
        remove_fn=_remove_disable_palm_rejection,
        detect_fn=_detect_disable_palm_rejection,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PALM],
        description=(
            "Disables touchpad palm rejection and cursor-leave detection. "
            "Useful when palm rejection causes missed inputs. Default: Enabled."
        ),
        tags=["touch", "touchpad", "palm", "rejection"],
    ),
]
