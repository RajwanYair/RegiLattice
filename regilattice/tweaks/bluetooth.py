"""Bluetooth registry tweaks.

Covers: Bluetooth power management, discoverability,
A2DP audio quality, and service startup.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_BT_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT"
_BT_PARAMS = rf"{_BT_SVC}\Parameters"
_BT_SUPPORT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\BthHFSrv"
)
_BT_AUDIO = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\BthA2dp\Parameters"
)


# ── Disable Bluetooth Power Management ───────────────────────────────────────


def _apply_disable_bt_power(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable power management (prevent sleep)")
    SESSION.backup([_BT_PARAMS], "BTPower")
    SESSION.set_dword(_BT_PARAMS, "AllowIdleIrpInD3", 0)


def _remove_disable_bt_power(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_PARAMS, "AllowIdleIrpInD3")


def _detect_disable_bt_power() -> bool:
    return SESSION.read_dword(_BT_PARAMS, "AllowIdleIrpInD3") == 0


# ── Disable Bluetooth Service Auto-Start ─────────────────────────────────────


def _apply_disable_bt_autostart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: set service to manual start")
    SESSION.backup([_BT_SVC, _BT_SUPPORT], "BTAutoStart")
    SESSION.set_dword(_BT_SVC, "Start", 3)  # 3 = Manual
    SESSION.set_dword(_BT_SUPPORT, "Start", 3)


def _remove_disable_bt_autostart(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BT_SVC, "Start", 3)  # stays manual, but re-enable support
    SESSION.set_dword(_BT_SUPPORT, "Start", 2)  # 2 = Automatic


def _detect_disable_bt_autostart() -> bool:
    return SESSION.read_dword(_BT_SUPPORT, "Start") == 3


# ── Improve Bluetooth A2DP Audio Quality ─────────────────────────────────────


def _apply_bt_audio_quality(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: set A2DP to high-quality SBC encoding")
    SESSION.backup([_BT_AUDIO], "BTAudioQuality")
    SESSION.set_dword(_BT_AUDIO, "MaxBitpool", 53)  # highest SBC quality
    SESSION.set_dword(_BT_AUDIO, "MinBitpool", 35)


def _remove_bt_audio_quality(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_AUDIO, "MaxBitpool")
    SESSION.delete_value(_BT_AUDIO, "MinBitpool")


def _detect_bt_audio_quality() -> bool:
    return SESSION.read_dword(_BT_AUDIO, "MaxBitpool") == 53


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-bt-power-mgmt",
        label="Disable Bluetooth Power Management",
        category="Bluetooth",
        apply_fn=_apply_disable_bt_power,
        remove_fn=_remove_disable_bt_power,
        detect_fn=_detect_disable_bt_power,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_PARAMS],
        description=(
            "Prevents Windows from suspending the Bluetooth adapter "
            "to save power, reducing connection drops."
        ),
        tags=["bluetooth", "power", "stability"],
    ),
    TweakDef(
        id="bt-manual-start",
        label="Bluetooth Service to Manual",
        category="Bluetooth",
        apply_fn=_apply_disable_bt_autostart,
        remove_fn=_remove_disable_bt_autostart,
        detect_fn=_detect_disable_bt_autostart,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_SVC, _BT_SUPPORT],
        description=(
            "Sets Bluetooth support service to manual start — saves "
            "resources on machines that rarely use Bluetooth."
        ),
        tags=["bluetooth", "services", "startup"],
    ),
    TweakDef(
        id="bt-high-quality-audio",
        label="Bluetooth A2DP High-Quality Audio",
        category="Bluetooth",
        apply_fn=_apply_bt_audio_quality,
        remove_fn=_remove_bt_audio_quality,
        detect_fn=_detect_bt_audio_quality,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_AUDIO],
        description=(
            "Increases the A2DP SBC bitpool range for higher-fidelity "
            "Bluetooth audio streaming."
        ),
        tags=["bluetooth", "audio", "quality"],
    ),
]
