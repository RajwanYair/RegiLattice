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
_BT_DISCOVER = rf"{_BT_PARAMS}\Devices"
_BT_LE = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\BthLEEnum\Parameters"
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


# ── Disable Bluetooth Discoverability ─────────────────────────────────────


def _apply_disable_bt_discover(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable discoverability")
    SESSION.backup([_BT_PARAMS], "BTDiscover")
    SESSION.set_dword(_BT_PARAMS, "AllowDiscovery", 0)


def _remove_disable_bt_discover(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_PARAMS, "AllowDiscovery")


def _detect_disable_bt_discover() -> bool:
    return SESSION.read_dword(_BT_PARAMS, "AllowDiscovery") == 0


# ── Bluetooth Low Energy Latency Optimization ────────────────────────────


def _apply_bt_le_latency(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: optimize BLE connection interval for low latency")
    SESSION.backup([_BT_LE], "BLELatency")
    SESSION.set_dword(_BT_LE, "MinimumConnectionInterval", 6)  # 7.5ms
    SESSION.set_dword(_BT_LE, "MaximumConnectionInterval", 10)  # 12.5ms


def _remove_bt_le_latency(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_LE, "MinimumConnectionInterval")
    SESSION.delete_value(_BT_LE, "MaximumConnectionInterval")


def _detect_bt_le_latency() -> bool:
    return SESSION.read_dword(_BT_LE, "MinimumConnectionInterval") == 6


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
    TweakDef(
        id="disable-bt-discoverable",
        label="Disable Bluetooth Discoverability",
        category="Bluetooth",
        apply_fn=_apply_disable_bt_discover,
        remove_fn=_remove_disable_bt_discover,
        detect_fn=_detect_disable_bt_discover,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_PARAMS],
        description="Prevents the Bluetooth adapter from being discoverable by nearby devices.",
        tags=["bluetooth", "security", "privacy"],
    ),
    TweakDef(
        id="bt-low-latency",
        label="Bluetooth LE Low-Latency Mode",
        category="Bluetooth",
        apply_fn=_apply_bt_le_latency,
        remove_fn=_remove_bt_le_latency,
        detect_fn=_detect_bt_le_latency,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_LE],
        description="Tightens BLE connection intervals for lower latency with peripherals.",
        tags=["bluetooth", "performance", "latency"],
    ),
]
