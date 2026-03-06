"""Bluetooth registry tweaks.

Covers: Bluetooth power management, discoverability,
A2DP audio quality, and service startup.
"""

from __future__ import annotations

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
_BT_HFP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio"
_BT_OBEX = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv"
_BT_PAN = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan"
_BT_SERIAL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM"
_BT_A2DP_SINK = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp"
_BT_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth"
_BT_A2DP_PARAMS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters"


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


# ── Disable Bluetooth Handsfree Profile ──────────────────────────────────────


def _apply_disable_handsfree(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable Handsfree Profile (HFP)")
    SESSION.backup([_BT_HFP], "BTHandsfree")
    SESSION.set_dword(_BT_HFP, "Start", 4)  # 4 = Disabled


def _remove_disable_handsfree(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BT_HFP, "Start", 3)  # 3 = Manual


def _detect_disable_handsfree() -> bool:
    return SESSION.read_dword(_BT_HFP, "Start") == 4


# ── Disable Bluetooth OBEX File Transfer ─────────────────────────────────────


def _apply_disable_obex(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable OBEX file transfer service")
    SESSION.backup([_BT_OBEX], "BTObex")
    SESSION.set_dword(_BT_OBEX, "Start", 4)  # 4 = Disabled


def _remove_disable_obex(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BT_OBEX, "Start", 3)  # 3 = Manual


def _detect_disable_obex() -> bool:
    return SESSION.read_dword(_BT_OBEX, "Start") == 4


# ── Disable Bluetooth PAN Networking ─────────────────────────────────────────


def _apply_disable_pan(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable PAN networking")
    SESSION.backup([_BT_PAN], "BTPan")
    SESSION.set_dword(_BT_PAN, "Start", 4)  # 4 = Disabled


def _remove_disable_pan(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BT_PAN, "Start", 3)  # 3 = Manual


def _detect_disable_pan() -> bool:
    return SESSION.read_dword(_BT_PAN, "Start") == 4


# ── Disable Bluetooth Serial Port (COM) ──────────────────────────────────────


def _apply_disable_serial(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable Serial Port (BTHMODEM)")
    SESSION.backup([_BT_SERIAL], "BTSerial")
    SESSION.set_dword(_BT_SERIAL, "Start", 4)  # 4 = Disabled


def _remove_disable_serial(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BT_SERIAL, "Start", 3)  # 3 = Manual


def _detect_disable_serial() -> bool:
    return SESSION.read_dword(_BT_SERIAL, "Start") == 4


# ── Disable Bluetooth A2DP Sink (Receive Audio) ──────────────────────────────


def _apply_disable_a2dp_sink(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable A2DP Sink (receive audio)")
    SESSION.backup([_BT_A2DP_SINK], "BTA2dpSink")
    SESSION.set_dword(_BT_A2DP_SINK, "Start", 4)  # 4 = Disabled


def _remove_disable_a2dp_sink(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BT_A2DP_SINK, "Start", 3)  # 3 = Manual


def _detect_disable_a2dp_sink() -> bool:
    return SESSION.read_dword(_BT_A2DP_SINK, "Start") == 4


# ── Disable Bluetooth LE Advertising ─────────────────────────────────────────


def _apply_disable_le_advertising(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable BLE advertising")
    SESSION.backup([_BT_POLICY], "BLEAdvertising")
    SESSION.set_dword(_BT_POLICY, "AllowAdvertising", 0)


def _remove_disable_le_advertising(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_POLICY, "AllowAdvertising")


def _detect_disable_le_advertising() -> bool:
    return SESSION.read_dword(_BT_POLICY, "AllowAdvertising") == 0


# ── Bluetooth Audio Offload ──────────────────────────────────────────────────


def _apply_bt_audio_offload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: enable A2DP sideband audio offloading")
    SESSION.backup([_BT_A2DP_PARAMS], "BTAudioOffload")
    SESSION.set_dword(_BT_A2DP_PARAMS, "AllowSidebandAudio", 1)


def _remove_bt_audio_offload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_A2DP_PARAMS, "AllowSidebandAudio")


def _detect_bt_audio_offload() -> bool:
    return SESSION.read_dword(_BT_A2DP_PARAMS, "AllowSidebandAudio") == 1


# ── Disable Bluetooth LE Device Discovery ────────────────────────────────────


def _apply_disable_le_discovery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable Bluetooth LE device discovery")
    SESSION.backup([_BT_DISCOVER], "BTLEDiscovery")
    SESSION.set_dword(_BT_DISCOVER, "DisableLEDiscovery", 1)


def _remove_disable_le_discovery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_DISCOVER, "DisableLEDiscovery")


def _detect_disable_le_discovery() -> bool:
    return SESSION.read_dword(_BT_DISCOVER, "DisableLEDiscovery") == 1


# ── Disable Bluetooth Handsfree Telephony ────────────────────────────────────


def _apply_disable_handsfree_telephony(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable Handsfree telephony via BTHPORT parameter")
    SESSION.backup([_BT_PARAMS], "BTHandsfreeTelephony")
    SESSION.set_dword(_BT_PARAMS, "DisableHandsfree", 1)


def _remove_disable_handsfree_telephony(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_PARAMS, "DisableHandsfree")


def _detect_disable_handsfree_telephony() -> bool:
    return SESSION.read_dword(_BT_PARAMS, "DisableHandsfree") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="bt-disable-bt-power-mgmt",
        label="Disable Bluetooth Power Management",
        category="Bluetooth",
        apply_fn=_apply_disable_bt_power,
        remove_fn=_remove_disable_bt_power,
        detect_fn=_detect_disable_bt_power,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_PARAMS],
        description=("Prevents Windows from suspending the Bluetooth adapter to save power, reducing connection drops."),
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
        description=("Sets Bluetooth support service to manual start — saves resources on machines that rarely use Bluetooth."),
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
        description=("Increases the A2DP SBC bitpool range for higher-fidelity Bluetooth audio streaming."),
        tags=["bluetooth", "audio", "quality"],
    ),
    TweakDef(
        id="bt-disable-bt-discoverable",
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
    TweakDef(
        id="bt-disable-handsfree",
        label="Disable Bluetooth Handsfree Profile",
        category="Bluetooth",
        apply_fn=_apply_disable_handsfree,
        remove_fn=_remove_disable_handsfree,
        detect_fn=_detect_disable_handsfree,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_HFP],
        description=("Disables the Bluetooth Handsfree Profile (HFP) service, preventing hands-free audio device connections."),
        tags=["bluetooth", "handsfree", "services"],
    ),
    TweakDef(
        id="bt-disable-obex",
        label="Disable Bluetooth OBEX File Transfer",
        category="Bluetooth",
        apply_fn=_apply_disable_obex,
        remove_fn=_remove_disable_obex,
        detect_fn=_detect_disable_obex,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_OBEX],
        description=("Disables the Bluetooth OBEX service, blocking file transfer over Bluetooth to reduce attack surface."),
        tags=["bluetooth", "obex", "security", "file-transfer"],
    ),
    TweakDef(
        id="bt-disable-pan",
        label="Disable Bluetooth PAN Networking",
        category="Bluetooth",
        apply_fn=_apply_disable_pan,
        remove_fn=_remove_disable_pan,
        detect_fn=_detect_disable_pan,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_PAN],
        description=("Disables Bluetooth Personal Area Networking (PAN), preventing network sharing over Bluetooth."),
        tags=["bluetooth", "network", "pan", "security"],
    ),
    TweakDef(
        id="bt-disable-serial",
        label="Disable Bluetooth Serial Port (COM)",
        category="Bluetooth",
        apply_fn=_apply_disable_serial,
        remove_fn=_remove_disable_serial,
        detect_fn=_detect_disable_serial,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_SERIAL],
        description=("Disables the Bluetooth BTHMODEM serial port driver, preventing legacy COM-port connections over Bluetooth."),
        tags=["bluetooth", "serial", "com", "security"],
    ),
    TweakDef(
        id="bt-disable-a2dp-sink",
        label="Disable Bluetooth A2DP Sink (Receive Audio)",
        category="Bluetooth",
        apply_fn=_apply_disable_a2dp_sink,
        remove_fn=_remove_disable_a2dp_sink,
        detect_fn=_detect_disable_a2dp_sink,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_A2DP_SINK],
        description=("Disables the Bluetooth A2DP Sink service, preventing the PC from receiving audio streams over Bluetooth."),
        tags=["bluetooth", "a2dp", "audio", "security"],
    ),
    TweakDef(
        id="bt-disable-le-advertising",
        label="Disable Bluetooth LE Advertising",
        category="Bluetooth",
        apply_fn=_apply_disable_le_advertising,
        remove_fn=_remove_disable_le_advertising,
        detect_fn=_detect_disable_le_advertising,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_POLICY],
        description=(
            "Disables Bluetooth Low Energy advertising. Prevents the PC "
            "from broadcasting BLE presence. Improves privacy. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["bluetooth", "ble", "privacy", "advertising"],
    ),
    TweakDef(
        id="bt-audio-offload",
        label="Bluetooth Audio Offload",
        category="Bluetooth",
        apply_fn=_apply_bt_audio_offload,
        remove_fn=_remove_bt_audio_offload,
        detect_fn=_detect_bt_audio_offload,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_A2DP_PARAMS],
        description=(
            "Enables Bluetooth A2DP sideband audio offloading to the "
            "adapter. Reduces CPU usage for Bluetooth audio streaming. "
            "Default: Disabled. Recommended: Enabled."
        ),
        tags=["bluetooth", "audio", "performance", "offload"],
    ),
    TweakDef(
        id="bt-disable-le-discovery",
        label="Disable Bluetooth LE Device Discovery",
        category="Bluetooth",
        apply_fn=_apply_disable_le_discovery,
        remove_fn=_remove_disable_le_discovery,
        detect_fn=_detect_disable_le_discovery,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_DISCOVER],
        description=(
            "Disables Bluetooth Low Energy device discovery. "
            "Reduces power usage and limits BLE scanning surface. "
            "Default: Enabled. Recommended: Disabled if BLE not needed."
        ),
        tags=["bluetooth", "ble", "discovery", "security"],
    ),
    TweakDef(
        id="bt-disable-handsfree-telephony",
        label="Disable Bluetooth Handsfree Telephony",
        category="Bluetooth",
        apply_fn=_apply_disable_handsfree_telephony,
        remove_fn=_remove_disable_handsfree_telephony,
        detect_fn=_detect_disable_handsfree_telephony,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BT_PARAMS],
        description=(
            "Disables the Bluetooth Handsfree telephony profile via "
            "BTHPORT parameter. Prevents low-quality HFP audio mode. "
            "Default: Enabled. Recommended: Disabled for A2DP-only use."
        ),
        tags=["bluetooth", "handsfree", "telephony", "audio"],
    ),
]


# ── Disable Bluetooth Auto-Pairing ───────────────────────────────────────────

_BT_AUTOCONFIG = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth"
_BT_MS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"


def _apply_bt_auto_pair_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Bluetooth auto-pairing")
    SESSION.backup([_BT_AUTOCONFIG], "BTAutoPairing")
    SESSION.set_dword(_BT_AUTOCONFIG, "DisableAutoPairing", 1)


def _remove_bt_auto_pair_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_AUTOCONFIG, "DisableAutoPairing")


def _detect_bt_auto_pair_off() -> bool:
    return SESSION.read_dword(_BT_AUTOCONFIG, "DisableAutoPairing") == 1


# ── Disable Bluetooth Advertising (Policy) ──────────────────────────────────


def _apply_bt_ms_advertising_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Bluetooth advertising via policy")
    SESSION.backup([_BT_MS_POLICY], "BTAdvertisingPolicy")
    SESSION.set_dword(_BT_MS_POLICY, "AllowAdvertising", 0)


def _remove_bt_ms_advertising_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_MS_POLICY, "AllowAdvertising")


def _detect_bt_ms_advertising_off() -> bool:
    return SESSION.read_dword(_BT_MS_POLICY, "AllowAdvertising") == 0


TWEAKS += [
    TweakDef(
        id="bt-disable-auto-pair",
        label="Disable Bluetooth Auto-Pairing",
        category="Bluetooth",
        apply_fn=_apply_bt_auto_pair_off,
        remove_fn=_remove_bt_auto_pair_off,
        detect_fn=_detect_bt_auto_pair_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BT_AUTOCONFIG],
        description=(
            "Disables automatic Bluetooth device pairing. Devices must "
            "be paired manually for better security control. "
            "Default: Enabled. Recommended: Disabled for security."
        ),
        tags=["bluetooth", "auto-pair", "security", "pairing"],
    ),
    TweakDef(
        id="bt-disable-advertising",
        label="Disable Bluetooth Advertising (Policy)",
        category="Bluetooth",
        apply_fn=_apply_bt_ms_advertising_off,
        remove_fn=_remove_bt_ms_advertising_off,
        detect_fn=_detect_bt_ms_advertising_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BT_MS_POLICY],
        description=(
            "Disables Bluetooth advertising via Microsoft policy. "
            "Prevents the device from broadcasting its presence. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["bluetooth", "advertising", "privacy", "policy"],
    ),
]


_BT_AG = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAGAudio"


# ── Disable Bluetooth LE Background Scanning ───────────────────────────────


def _apply_bt_le_scan_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable LE background scanning")
    SESSION.backup([_BT_LE], "BtLeScan")
    SESSION.set_dword(_BT_LE, "ScanEnabled", 0)


def _remove_bt_le_scan_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_LE, "ScanEnabled")


def _detect_bt_le_scan_off() -> bool:
    return SESSION.read_dword(_BT_LE, "ScanEnabled") == 0


# ── Set Bluetooth Page Timeout ───────────────────────────────────────────────


def _apply_bt_page_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: set page timeout to 16384 slots (10s)")
    SESSION.backup([_BT_PARAMS], "BtPageTimeout")
    SESSION.set_dword(_BT_PARAMS, "PageTimeout", 16384)


def _remove_bt_page_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BT_PARAMS, "PageTimeout")


def _detect_bt_page_timeout() -> bool:
    return SESSION.read_dword(_BT_PARAMS, "PageTimeout") == 16384


# ── Disable Bluetooth Audio Gateway ─────────────────────────────────────────


def _apply_bt_ag_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Bluetooth: disable Audio Gateway service")
    SESSION.backup([_BT_AG], "BtAudioGateway")
    SESSION.set_dword(_BT_AG, "Start", 4)


def _remove_bt_ag_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_BT_AG], "BtAudioGateway_Remove")
    SESSION.set_dword(_BT_AG, "Start", 3)


def _detect_bt_ag_off() -> bool:
    return SESSION.read_dword(_BT_AG, "Start") == 4


TWEAKS += [
    TweakDef(
        id="bt-disable-le-scan",
        label="Disable Bluetooth LE Scanning",
        category="Bluetooth",
        apply_fn=_apply_bt_le_scan_off,
        remove_fn=_remove_bt_le_scan_off,
        detect_fn=_detect_bt_le_scan_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BT_LE],
        description=(
            "Disables Bluetooth Low Energy background scanning. "
            "Reduces power consumption and radio interference. "
            "Default: Enabled. Recommended: Disabled for battery life."
        ),
        tags=["bluetooth", "le", "scanning", "power", "battery"],
    ),
    TweakDef(
        id="bt-page-timeout",
        label="Set Bluetooth Page Timeout",
        category="Bluetooth",
        apply_fn=_apply_bt_page_timeout,
        remove_fn=_remove_bt_page_timeout,
        detect_fn=_detect_bt_page_timeout,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BT_PARAMS],
        description=(
            "Sets Bluetooth page timeout to 16384 slots (~10 seconds). "
            "Increases time allowed for device connection establishment. "
            "Default: 8192 slots. Recommended: 16384 for reliability."
        ),
        tags=["bluetooth", "page", "timeout", "connection", "reliability"],
    ),
    TweakDef(
        id="bt-disable-audio-gateway",
        label="Disable Bluetooth Audio Gateway",
        category="Bluetooth",
        apply_fn=_apply_bt_ag_off,
        remove_fn=_remove_bt_ag_off,
        detect_fn=_detect_bt_ag_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_BT_AG],
        description=(
            "Disables the Bluetooth Audio Gateway service driver. "
            "Prevents phone call audio routing through the PC. "
            "Default: Enabled. Recommended: Disabled if unused."
        ),
        tags=["bluetooth", "audio-gateway", "telephony", "service"],
    ),
]
