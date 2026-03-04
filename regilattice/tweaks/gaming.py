"""Gaming tweaks — Game DVR / Game Bar."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

_GAMEDVR_CU = r"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"
_GAMEBAR_CU = r"HKEY_CURRENT_USER\System\GameConfigStore"
_GAMEDVR_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"
_GAMECONFIG_LM = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default"
    r"\ApplicationManagement\AllowGameDVR"
)

_KEYS = [_GAMEDVR_CU, _GAMEBAR_CU, _GAMEDVR_POLICY, _GAMECONFIG_LM]


def apply_disable_gamedvr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableGameDVR")
    SESSION.backup(_KEYS, "GameDVR")
    SESSION.set_dword(_GAMEDVR_CU, "AppCaptureEnabled", 0)
    SESSION.set_dword(_GAMEBAR_CU, "GameDVR_Enabled", 0)
    SESSION.set_dword(_GAMEDVR_POLICY, "AllowGameDVR", 0)
    SESSION.set_dword(_GAMECONFIG_LM, "value", 0)
    SESSION.log("Completed Add-DisableGameDVR")


def remove_disable_gamedvr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableGameDVR")
    SESSION.backup(_KEYS, "GameDVR_Remove")
    SESSION.delete_value(_GAMEDVR_CU, "AppCaptureEnabled")
    SESSION.set_dword(_GAMEBAR_CU, "GameDVR_Enabled", 1)
    SESSION.delete_value(_GAMEDVR_POLICY, "AllowGameDVR")
    SESSION.delete_value(_GAMECONFIG_LM, "value")
    SESSION.log("Completed Remove-DisableGameDVR")


def detect_disable_gamedvr() -> bool:
    return SESSION.read_dword(_GAMEBAR_CU, "GameDVR_Enabled") == 0


TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-gamedvr",
        label="Disable Game DVR / Game Bar",
        category="Gaming",
        apply_fn=apply_disable_gamedvr,
        remove_fn=remove_disable_gamedvr,
        detect_fn=detect_disable_gamedvr,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_KEYS,
        description=(
            "Disables Windows Game DVR, Game Bar overlay, and background "
            "recording for better gaming and benchmarking performance."
        ),
        tags=["gaming", "performance", "dvr"],
    ),
]
