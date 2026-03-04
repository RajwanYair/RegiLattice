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
_GAMEMODE_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\GameBar"
)
_FULLSCREEN_CU = (
    r"HKEY_CURRENT_USER\System\GameConfigStore"
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


# ── Disable Game Mode ────────────────────────────────────────────────────────


def _apply_disable_game_mode(*, require_admin: bool = False) -> None:
    SESSION.log("Gaming: disable Windows Game Mode")
    SESSION.backup([_GAMEMODE_CU], "GameMode")
    SESSION.set_dword(_GAMEMODE_CU, "AllowAutoGameMode", 0)
    SESSION.set_dword(_GAMEMODE_CU, "AutoGameModeEnabled", 0)


def _remove_disable_game_mode(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_GAMEMODE_CU, "AllowAutoGameMode")
    SESSION.set_dword(_GAMEMODE_CU, "AutoGameModeEnabled", 1)


def _detect_disable_game_mode() -> bool:
    return SESSION.read_dword(_GAMEMODE_CU, "AutoGameModeEnabled") == 0


# ── Disable Fullscreen Optimizations ───────────────────────────────────────


def _apply_disable_fso(*, require_admin: bool = False) -> None:
    SESSION.log("Gaming: disable fullscreen optimizations globally")
    SESSION.backup([_FULLSCREEN_CU], "FSO")
    SESSION.set_dword(_FULLSCREEN_CU, "GameDVR_FSEBehavior", 2)
    SESSION.set_dword(_FULLSCREEN_CU, "GameDVR_DXGIHonorFSEWindowsCompatible", 1)
    SESSION.set_dword(_FULLSCREEN_CU, "GameDVR_FSEBehaviorMode", 2)


def _remove_disable_fso(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_FULLSCREEN_CU, "GameDVR_FSEBehavior")
    SESSION.delete_value(_FULLSCREEN_CU, "GameDVR_DXGIHonorFSEWindowsCompatible")
    SESSION.delete_value(_FULLSCREEN_CU, "GameDVR_FSEBehaviorMode")


def _detect_disable_fso() -> bool:
    return SESSION.read_dword(_FULLSCREEN_CU, "GameDVR_FSEBehavior") == 2


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
    TweakDef(
        id="disable-game-mode",
        label="Disable Windows Game Mode",
        category="Gaming",
        apply_fn=_apply_disable_game_mode,
        remove_fn=_remove_disable_game_mode,
        detect_fn=_detect_disable_game_mode,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GAMEMODE_CU],
        description="Disables Windows Game Mode which can cause stutter in some games.",
        tags=["gaming", "performance", "game-mode"],
    ),
    TweakDef(
        id="disable-fullscreen-optimizations",
        label="Disable Fullscreen Optimizations",
        category="Gaming",
        apply_fn=_apply_disable_fso,
        remove_fn=_remove_disable_fso,
        detect_fn=_detect_disable_fso,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FULLSCREEN_CU],
        description=(
            "Disables Windows fullscreen optimizations (DX flip model) "
            "which can cause input lag in older games."
        ),
        tags=["gaming", "performance", "fullscreen"],
    ),
]
