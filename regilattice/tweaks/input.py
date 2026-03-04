"""Input tweaks — Mouse Acceleration."""

from __future__ import annotations

from regilattice.registry import SESSION
from regilattice.tweaks import TweakDef

_MOUSE_KEY = r"HKEY_CURRENT_USER\Control Panel\Mouse"


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
]
