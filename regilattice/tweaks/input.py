"""Input tweaks — Mouse, Keyboard, Sticky Keys."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

_MOUSE_KEY = r"HKEY_CURRENT_USER\Control Panel\Mouse"
_KEYBOARD_KEY = r"HKEY_CURRENT_USER\Control Panel\Keyboard"
_ACCESSIBILITY = r"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"
_FILTER_KEYS = r"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys"
_TOGGLE_KEYS = r"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"
_DESKTOP_KEY = r"HKEY_CURRENT_USER\Control Panel\Desktop"
_TOUCH_KB_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC"
_TOUCH_KB_AUTO = r"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"


def apply_disable_mouse_accel(*, require_admin: bool = False) -> None:
    SESSION.log("Starting Add-DisableMouseAcceleration")
    SESSION.backup([_MOUSE_KEY], "MouseAccel")
    SESSION.set_string(_MOUSE_KEY, "MouseSpeed", "0")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold1", "0")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold2", "0")
    SESSION.log("Completed Add-DisableMouseAcceleration")


def remove_disable_mouse_accel(*, require_admin: bool = False) -> None:
    SESSION.log("Starting Remove-DisableMouseAcceleration")
    SESSION.backup([_MOUSE_KEY], "MouseAccel_Remove")
    SESSION.set_string(_MOUSE_KEY, "MouseSpeed", "1")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold1", "6")
    SESSION.set_string(_MOUSE_KEY, "MouseThreshold2", "10")
    SESSION.log("Completed Remove-DisableMouseAcceleration")


def detect_disable_mouse_accel() -> bool:
    return SESSION.read_string(_MOUSE_KEY, "MouseSpeed") == "0"


# ── Increase Keyboard Repeat Rate ──────────────────────────────────────────


def _apply_fast_keyboard(*, require_admin: bool = False) -> None:
    SESSION.log("Input: set keyboard repeat rate to maximum")
    SESSION.backup([_KEYBOARD_KEY], "KeyboardRepeat")
    SESSION.set_string(_KEYBOARD_KEY, "KeyboardSpeed", "31")  # max = 31
    SESSION.set_string(_KEYBOARD_KEY, "KeyboardDelay", "0")   # shortest delay


def _remove_fast_keyboard(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEYBOARD_KEY, "KeyboardSpeed", "31")  # default
    SESSION.set_string(_KEYBOARD_KEY, "KeyboardDelay", "1")   # default


def _detect_fast_keyboard() -> bool:
    return SESSION.read_string(_KEYBOARD_KEY, "KeyboardDelay") == "0"


# ── Disable Sticky Keys Prompt ────────────────────────────────────────────


def _apply_disable_sticky_prompt(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable Sticky Keys 5x Shift prompt")
    SESSION.backup([_ACCESSIBILITY], "StickyKeysPrompt")
    SESSION.set_string(_ACCESSIBILITY, "Flags", "506")  # disable hotkey


def _remove_disable_sticky_prompt(*, require_admin: bool = False) -> None:
    SESSION.set_string(_ACCESSIBILITY, "Flags", "510")  # default (hotkey enabled)


def _detect_disable_sticky_prompt() -> bool:
    return SESSION.read_string(_ACCESSIBILITY, "Flags") == "506"


# ── Disable Touchpad Tap-to-Click (Performance) ─────────────────────────────────────────────

_TOUCHPAD = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"


def _apply_disable_touchpad_tap(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable touchpad tap-to-click")
    SESSION.backup([_TOUCHPAD], "TouchpadTap")
    SESSION.set_dword(_TOUCHPAD, "TapEnabled", 0)


def _remove_disable_touchpad_tap(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCHPAD, "TapEnabled", 1)


def _detect_disable_touchpad_tap() -> bool:
    return SESSION.read_dword(_TOUCHPAD, "TapEnabled") == 0


# ── Increase Mouse Hover Time (reduce accidental tooltips) ──────────────────────────────────

_MOUSE_HOVER = r"HKEY_CURRENT_USER\Control Panel\Mouse"


def _apply_increase_hover_time(*, require_admin: bool = False) -> None:
    SESSION.log("Input: increase mouse hover time to 1000ms")
    SESSION.backup([_MOUSE_HOVER], "MouseHover")
    SESSION.set_string(_MOUSE_HOVER, "MouseHoverTime", "1000")


def _remove_increase_hover_time(*, require_admin: bool = False) -> None:
    SESSION.set_string(_MOUSE_HOVER, "MouseHoverTime", "400")


def _detect_increase_hover_time() -> bool:
    return SESSION.read_string(_MOUSE_HOVER, "MouseHoverTime") == "1000"


# ── Disable Filter Keys ─────────────────────────────────────────────────


def _apply_disable_filter_keys(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable Filter Keys")
    SESSION.backup([_FILTER_KEYS], "FilterKeys")
    SESSION.set_string(_FILTER_KEYS, "Flags", "0")


def _remove_disable_filter_keys(*, require_admin: bool = False) -> None:
    SESSION.set_string(_FILTER_KEYS, "Flags", "126")


def _detect_disable_filter_keys() -> bool:
    return SESSION.read_string(_FILTER_KEYS, "Flags") == "0"


# ── Disable Toggle Keys Beep ────────────────────────────────────────────


def _apply_disable_toggle_keys(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable Toggle Keys beep")
    SESSION.backup([_TOGGLE_KEYS], "ToggleKeys")
    SESSION.set_string(_TOGGLE_KEYS, "Flags", "0")


def _remove_disable_toggle_keys(*, require_admin: bool = False) -> None:
    SESSION.set_string(_TOGGLE_KEYS, "Flags", "62")


def _detect_disable_toggle_keys() -> bool:
    return SESSION.read_string(_TOGGLE_KEYS, "Flags") == "0"


# ── Disable Enhanced Pointer Precision ──────────────────────────────────


def _apply_disable_epp(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable Enhanced Pointer Precision")
    SESSION.backup([_MOUSE_KEY], "EnhancedPointerPrecision")
    SESSION.set_string(_MOUSE_KEY, "MouseSpeed", "0")


def _remove_disable_epp(*, require_admin: bool = False) -> None:
    SESSION.set_string(_MOUSE_KEY, "MouseSpeed", "1")


def _detect_disable_epp() -> bool:
    return SESSION.read_string(_MOUSE_KEY, "MouseSpeed") == "0"


# ── Set Mouse Scroll to 5 Lines ─────────────────────────────────────────


def _apply_mouse_scroll_lines(*, require_admin: bool = False) -> None:
    SESSION.log("Input: set mouse scroll to 5 lines")
    SESSION.backup([_DESKTOP_KEY], "MouseScrollLines")
    SESSION.set_string(_DESKTOP_KEY, "WheelScrollLines", "5")


def _remove_mouse_scroll_lines(*, require_admin: bool = False) -> None:
    SESSION.set_string(_DESKTOP_KEY, "WheelScrollLines", "3")


def _detect_mouse_scroll_lines() -> bool:
    return SESSION.read_string(_DESKTOP_KEY, "WheelScrollLines") == "5"


# ── Set Keyboard Repeat Delay to Minimum ────────────────────────────────


def _apply_keyboard_delay_zero(*, require_admin: bool = False) -> None:
    SESSION.log("Input: set keyboard repeat delay to minimum")
    SESSION.backup([_KEYBOARD_KEY], "KeyboardDelayZero")
    SESSION.set_string(_KEYBOARD_KEY, "KeyboardDelay", "0")


def _remove_keyboard_delay_zero(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEYBOARD_KEY, "KeyboardDelay", "1")


def _detect_keyboard_delay_zero() -> bool:
    return SESSION.read_string(_KEYBOARD_KEY, "KeyboardDelay") == "0"


# ── Disable Touch Keyboard Auto-Launch ──────────────────────────────────


def _apply_disable_touch_keyboard(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Input: disable touch keyboard auto-launch")
    SESSION.backup([_TOUCH_KB_POLICY], "TouchKeyboard")
    SESSION.set_dword(_TOUCH_KB_POLICY, "EnableTouchKeyboard", 0)


def _remove_disable_touch_keyboard(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TOUCH_KB_POLICY, "EnableTouchKeyboard")


def _detect_disable_touch_keyboard() -> bool:
    return SESSION.read_dword(_TOUCH_KB_POLICY, "EnableTouchKeyboard") == 0


# ── Disable Touch Keyboard Auto-Invoke ──────────────────────────────────


def _apply_disable_touch_kb_auto(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable touch keyboard auto-invoke")
    SESSION.backup([_TOUCH_KB_AUTO], "TouchKbAutoInvoke")
    SESSION.set_dword(_TOUCH_KB_AUTO, "EnableDesktopModeAutoInvoke", 0)


def _remove_disable_touch_kb_auto(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCH_KB_AUTO, "EnableDesktopModeAutoInvoke", 1)


def _detect_disable_touch_kb_auto() -> bool:
    return SESSION.read_dword(_TOUCH_KB_AUTO, "EnableDesktopModeAutoInvoke") == 0


# ── Disable Sticky Keys Shortcut ────────────────────────────────────────


def _apply_disable_sticky_keys(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable Sticky Keys shortcut")
    SESSION.backup([_ACCESSIBILITY], "StickyKeysShortcut")
    SESSION.set_string(_ACCESSIBILITY, "Flags", "506")


def _remove_disable_sticky_keys(*, require_admin: bool = False) -> None:
    SESSION.set_string(_ACCESSIBILITY, "Flags", "510")


def _detect_disable_sticky_keys() -> bool:
    return SESSION.read_string(_ACCESSIBILITY, "Flags") == "506"


# ── Plugin registration ─────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-mouse-accel",
        label="Disable Mouse Acceleration",
        category="Input",
        apply_fn=apply_disable_mouse_accel,
        remove_fn=remove_disable_mouse_accel,
        detect_fn=detect_disable_mouse_accel,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MOUSE_KEY],
        description="Sets MouseSpeed/Threshold to zero for raw 1:1 input.",
        tags=["input", "mouse", "gaming"],
    ),
    TweakDef(
        id="fast-keyboard-repeat",
        label="Maximize Keyboard Repeat Rate",
        category="Input",
        apply_fn=_apply_fast_keyboard,
        remove_fn=_remove_fast_keyboard,
        detect_fn=_detect_fast_keyboard,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEYBOARD_KEY],
        description="Sets keyboard repeat speed to maximum and delay to shortest.",
        tags=["input", "keyboard", "performance"],
    ),
    TweakDef(
        id="disable-sticky-keys-prompt",
        label="Disable Sticky Keys Prompt (5x Shift)",
        category="Input",
        apply_fn=_apply_disable_sticky_prompt,
        remove_fn=_remove_disable_sticky_prompt,
        detect_fn=_detect_disable_sticky_prompt,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ACCESSIBILITY],
        description="Prevents the Sticky Keys dialog from appearing when pressing Shift 5 times.",
        tags=["input", "accessibility", "gaming"],
    ),
    TweakDef(
        id="disable-touchpad-tap",
        label="Disable Touchpad Tap-to-Click (Perf)",
        category="Input",
        apply_fn=_apply_disable_touchpad_tap,
        remove_fn=_remove_disable_touchpad_tap,
        detect_fn=_detect_disable_touchpad_tap,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCHPAD],
        description=(
            "Disables tap-to-click on precision touchpads to reduce "
            "accidental clicks and improve input accuracy. "
            "Default: Enabled. Recommended: Disabled for desktop users."
        ),
        tags=["input", "touchpad", "performance"],
    ),
    TweakDef(
        id="increase-hover-time",
        label="Increase Mouse Hover Time (1s)",
        category="Input",
        apply_fn=_apply_increase_hover_time,
        remove_fn=_remove_increase_hover_time,
        detect_fn=_detect_increase_hover_time,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MOUSE_HOVER],
        description=(
            "Increases mouse hover delay from 400ms to 1000ms. "
            "Reduces accidental tooltip popups. "
            "Options: 400ms (default) / 1000ms. Recommended: 1000ms."
        ),
        tags=["input", "mouse", "ux"],
    ),
    TweakDef(
        id="filter-keys",
        label="Disable Filter Keys",
        category="Input",
        apply_fn=_apply_disable_filter_keys,
        remove_fn=_remove_disable_filter_keys,
        detect_fn=_detect_disable_filter_keys,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FILTER_KEYS],
        description="Disables Filter Keys accessibility shortcut that can interfere with gaming.",
        tags=["input", "accessibility", "gaming"],
    ),
    TweakDef(
        id="toggle-keys",
        label="Disable Toggle Keys Beep",
        category="Input",
        apply_fn=_apply_disable_toggle_keys,
        remove_fn=_remove_disable_toggle_keys,
        detect_fn=_detect_disable_toggle_keys,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOGGLE_KEYS],
        description="Disables the Toggle Keys beep when pressing Num/Caps/Scroll Lock.",
        tags=["input", "accessibility", "gaming"],
    ),
    TweakDef(
        id="enhanced-pointer-precision",
        label="Disable Enhanced Pointer Precision",
        category="Input",
        apply_fn=_apply_disable_epp,
        remove_fn=_remove_disable_epp,
        detect_fn=_detect_disable_epp,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MOUSE_KEY],
        description="Disables mouse smoothing for raw 1:1 pointer input.",
        tags=["input", "mouse", "gaming"],
    ),
    TweakDef(
        id="mouse-scroll-lines",
        label="Set Mouse Scroll to 5 Lines",
        category="Input",
        apply_fn=_apply_mouse_scroll_lines,
        remove_fn=_remove_mouse_scroll_lines,
        detect_fn=_detect_mouse_scroll_lines,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DESKTOP_KEY],
        description="Sets mouse wheel scroll amount to 5 lines (default 3).",
        tags=["input", "mouse", "ux"],
    ),
    TweakDef(
        id="keyboard-delay-zero",
        label="Set Keyboard Repeat Delay to Minimum",
        category="Input",
        apply_fn=_apply_keyboard_delay_zero,
        remove_fn=_remove_keyboard_delay_zero,
        detect_fn=_detect_keyboard_delay_zero,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEYBOARD_KEY],
        description="Sets keyboard repeat delay to 0 (minimum) for faster key repeat.",
        tags=["input", "keyboard", "performance"],
    ),
    TweakDef(
        id="touch-keyboard-disable",
        label="Disable Touch Keyboard Auto-Launch",
        category="Input",
        apply_fn=_apply_disable_touch_keyboard,
        remove_fn=_remove_disable_touch_keyboard,
        detect_fn=_detect_disable_touch_keyboard,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TOUCH_KB_POLICY],
        description="Disables automatic touch keyboard launch via Group Policy.",
        tags=["input", "touch", "keyboard"],
    ),
    TweakDef(
        id="input-disable-touch-kb-auto",
        label="Disable Touch Keyboard Auto-Invoke",
        category="Input",
        apply_fn=_apply_disable_touch_kb_auto,
        remove_fn=_remove_disable_touch_kb_auto,
        detect_fn=_detect_disable_touch_kb_auto,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCH_KB_AUTO],
        description=(
            "Prevents the touch keyboard from automatically appearing on "
            "desktops without touchscreens. Default: Enabled. "
            "Recommended: Disabled."
        ),
        tags=["input", "touch", "keyboard", "performance"],
    ),
    TweakDef(
        id="input-disable-sticky-keys",
        label="Disable Sticky Keys Shortcut",
        category="Input",
        apply_fn=_apply_disable_sticky_keys,
        remove_fn=_remove_disable_sticky_keys,
        detect_fn=_detect_disable_sticky_keys,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ACCESSIBILITY],
        description=(
            "Disables the Sticky Keys shortcut (pressing Shift 5 times). "
            "Prevents accidental activation during gaming. "
            "Default: Enabled (510). Recommended: Disabled (506)."
        ),
        tags=["input", "sticky-keys", "gaming", "accessibility"],
    ),
]


# ── Disable Spell Check ──────────────────────────────────────────────────────


def _apply_disable_spell_check(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable auto spell checking")
    SESSION.backup([_TOUCH_KB_AUTO], "SpellCheck")
    SESSION.set_dword(_TOUCH_KB_AUTO, "EnableSpellchecking", 0)


def _remove_disable_spell_check(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCH_KB_AUTO, "EnableSpellchecking", 1)


def _detect_disable_spell_check() -> bool:
    return SESSION.read_dword(_TOUCH_KB_AUTO, "EnableSpellchecking") == 0


# ── Disable Text Suggestions ─────────────────────────────────────────────────


def _apply_disable_text_suggestions(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable text suggestions while typing")
    SESSION.backup([_TOUCH_KB_AUTO], "TextSuggestions")
    SESSION.set_dword(_TOUCH_KB_AUTO, "EnableTextPrediction", 0)


def _remove_disable_text_suggestions(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TOUCH_KB_AUTO, "EnableTextPrediction", 1)


def _detect_disable_text_suggestions() -> bool:
    return SESSION.read_dword(_TOUCH_KB_AUTO, "EnableTextPrediction") == 0


TWEAKS += [
    TweakDef(
        id="input-disable-spell-check",
        label="Disable Spell Checking",
        category="Input",
        apply_fn=_apply_disable_spell_check,
        remove_fn=_remove_disable_spell_check,
        detect_fn=_detect_disable_spell_check,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCH_KB_AUTO],
        description=(
            "Disables automatic spell checking in Windows text input. "
            "Reduces CPU usage from background spell-check processing. "
            "Default: Enabled. Recommended: Disabled for developers."
        ),
        tags=["input", "spell-check", "typing", "performance"],
    ),
    TweakDef(
        id="input-disable-text-suggestions",
        label="Disable Text Suggestions",
        category="Input",
        apply_fn=_apply_disable_text_suggestions,
        remove_fn=_remove_disable_text_suggestions,
        detect_fn=_detect_disable_text_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TOUCH_KB_AUTO],
        description=(
            "Disables text prediction and suggestions while typing. "
            "Prevents the suggestion bar from appearing above the keyboard. "
            "Default: Enabled. Recommended: Disabled for power users."
        ),
        tags=["input", "text-prediction", "suggestions", "typing"],
    ),
]


# ── Set Cursor Blink Rate ───────────────────────────────────────────────────


def _apply_set_cursor_blink_rate(*, require_admin: bool = False) -> None:
    SESSION.log("Input: set cursor blink rate to 400 ms (fast)")
    SESSION.backup([_DESKTOP_KEY], "CursorBlinkRate")
    SESSION.set_string(_DESKTOP_KEY, "CursorBlinkRate", "400")


def _remove_set_cursor_blink_rate(*, require_admin: bool = False) -> None:
    SESSION.set_string(_DESKTOP_KEY, "CursorBlinkRate", "530")


def _detect_set_cursor_blink_rate() -> bool:
    return SESSION.read_string(_DESKTOP_KEY, "CursorBlinkRate") == "400"


# ── Increase Double-Click Speed ──────────────────────────────────────────────


def _apply_increase_double_click_speed(*, require_admin: bool = False) -> None:
    SESSION.log("Input: increase double-click speed (lower threshold)")
    SESSION.backup([_MOUSE_KEY], "DoubleClickSpeed")
    SESSION.set_string(_MOUSE_KEY, "DoubleClickSpeed", "200")


def _remove_increase_double_click_speed(*, require_admin: bool = False) -> None:
    SESSION.set_string(_MOUSE_KEY, "DoubleClickSpeed", "500")


def _detect_increase_double_click_speed() -> bool:
    return SESSION.read_string(_MOUSE_KEY, "DoubleClickSpeed") == "200"


# ── Disable Touch Visual Feedback ────────────────────────────────────────────

_CURSOR_KEY = r"HKEY_CURRENT_USER\Control Panel\Cursors"


def _apply_disable_touch_feedback(*, require_admin: bool = False) -> None:
    SESSION.log("Input: disable touch and pen visual feedback")
    SESSION.backup([_CURSOR_KEY], "TouchFeedback")
    SESSION.set_dword(_CURSOR_KEY, "ContactVisualization", 0)
    SESSION.set_dword(_CURSOR_KEY, "GestureVisualization", 0)


def _remove_disable_touch_feedback(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CURSOR_KEY, "ContactVisualization", 1)
    SESSION.set_dword(_CURSOR_KEY, "GestureVisualization", 31)


def _detect_disable_touch_feedback() -> bool:
    return (
        SESSION.read_dword(_CURSOR_KEY, "ContactVisualization") == 0
        and SESSION.read_dword(_CURSOR_KEY, "GestureVisualization") == 0
    )


TWEAKS += [
    TweakDef(
        id="input-set-cursor-blink-rate",
        label="Set Fast Cursor Blink Rate",
        category="Input",
        apply_fn=_apply_set_cursor_blink_rate,
        remove_fn=_remove_set_cursor_blink_rate,
        detect_fn=_detect_set_cursor_blink_rate,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DESKTOP_KEY],
        description=(
            "Sets the cursor blink rate to 400 ms (faster than default). "
            "Makes the text cursor more visible and responsive. "
            "Default: 530 ms. Recommended: 400 ms for faster feedback."
        ),
        tags=["input", "cursor", "blink-rate", "typing", "ux"],
    ),
    TweakDef(
        id="input-increase-double-click-speed",
        label="Increase Double-Click Speed",
        category="Input",
        apply_fn=_apply_increase_double_click_speed,
        remove_fn=_remove_increase_double_click_speed,
        detect_fn=_detect_increase_double_click_speed,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MOUSE_KEY],
        description=(
            "Reduces the double-click detection interval to 200 ms for faster "
            "response. Requires quicker double-clicks but feels responsive. "
            "Default: 500 ms. Recommended: 200 ms for power users."
        ),
        tags=["input", "mouse", "double-click", "speed", "ux"],
    ),
    TweakDef(
        id="input-disable-touch-feedback",
        label="Disable Touch Visual Feedback",
        category="Input",
        apply_fn=_apply_disable_touch_feedback,
        remove_fn=_remove_disable_touch_feedback,
        detect_fn=_detect_disable_touch_feedback,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CURSOR_KEY],
        description=(
            "Disables visual feedback animations for touch and pen input. "
            "Removes the circle/ripple effect on touch and gesture indicators. "
            "Default: Enabled. Recommended: Disabled on non-touch devices."
        ),
        tags=["input", "touch", "pen", "feedback", "visual"],
    ),
]
