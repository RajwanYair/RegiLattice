"""Input tweaks — Mouse, Keyboard, Sticky Keys."""

from __future__ import annotations

from regilattice.registry import SESSION
from regilattice.tweaks import TweakDef

_MOUSE_KEY = r"HKEY_CURRENT_USER\Control Panel\Mouse"
_KEYBOARD_KEY = r"HKEY_CURRENT_USER\Control Panel\Keyboard"
_ACCESSIBILITY = r"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"


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
        label="Disable Sticky Keys Prompt (5× Shift)",
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
]
