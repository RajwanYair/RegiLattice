"""Network and connectivity registry tweaks.

Covers IRPStackSize, DNS over HTTPS, network throttling,
Remote Desktop, and TCP/IP optimizations.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_LANMAN = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\LanmanServer\Parameters"
)
_TCPIP = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\Tcpip\Parameters"
)
_AFD = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\AFD\Parameters"
)
_THROTTLE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Multimedia\SystemProfile"
)
_TERM_SERVER = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Terminal Server"
)
_RDP_TCP = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Terminal Server\WinStations\RDP-Tcp"
)
_DNS_CLIENT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\Dnscache\Parameters"
)
_FW = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\SharedAccess\Parameters\FirewallPolicy\FirewallRules"
)


# ── Increase IRPStackSize ────────────────────────────────────────────────────


def _apply_irpstack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: increase IRPStackSize to 32")
    SESSION.backup([_LANMAN], "IRPStack")
    SESSION.set_dword(_LANMAN, "IRPStackSize", 32)


def _remove_irpstack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LANMAN, "IRPStackSize", 15)  # default


def _detect_irpstack() -> bool:
    val = SESSION.read_dword(_LANMAN, "IRPStackSize")
    return val is not None and val >= 32


# ── Disable Nagle Algorithm (Low-Latency TCP) ───────────────────────────────


def _apply_disable_nagle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable Nagle algorithm")
    SESSION.backup([_TCPIP], "Nagle")
    SESSION.set_dword(_TCPIP, "TcpAckFrequency", 1)
    SESSION.set_dword(_TCPIP, "TCPNoDelay", 1)


def _remove_disable_nagle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "TcpAckFrequency")
    SESSION.delete_value(_TCPIP, "TCPNoDelay")


def _detect_disable_nagle() -> bool:
    return SESSION.read_dword(_TCPIP, "TCPNoDelay") == 1


# ── Disable Network Throttling ──────────────────────────────────────────────


def _apply_disable_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable network throttling index")
    SESSION.backup([_THROTTLE], "NetThrottle")
    SESSION.set_dword(_THROTTLE, "NetworkThrottlingIndex", 0xFFFFFFFF)


def _remove_disable_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_THROTTLE, "NetworkThrottlingIndex", 10)  # default


def _detect_disable_throttle() -> bool:
    return SESSION.read_dword(_THROTTLE, "NetworkThrottlingIndex") == 0xFFFFFFFF


# ── Enable Remote Desktop ───────────────────────────────────────────────────


def _apply_enable_rdp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: enable Remote Desktop")
    SESSION.backup([_TERM_SERVER, _RDP_TCP, _FW], "RDP")
    SESSION.set_dword(_TERM_SERVER, "fDenyTSConnections", 0)
    SESSION.set_dword(_RDP_TCP, "UserAuthentication", 1)  # NLA required


def _remove_enable_rdp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TERM_SERVER, "fDenyTSConnections", 1)


def _detect_enable_rdp() -> bool:
    return SESSION.read_dword(_TERM_SERVER, "fDenyTSConnections") == 0


# ── Enable DNS over HTTPS (DoH) ─────────────────────────────────────────────


def _apply_doh(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: enable DNS-over-HTTPS")
    SESSION.backup([_DNS_CLIENT], "DoH")
    SESSION.set_dword(_DNS_CLIENT, "EnableAutoDoh", 2)  # 2 = automatic DoH


def _remove_doh(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_CLIENT, "EnableAutoDoh")


def _detect_doh() -> bool:
    return SESSION.read_dword(_DNS_CLIENT, "EnableAutoDoh") == 2


# ── Increase Max TCP Connections ─────────────────────────────────────────────


def _apply_max_connections(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: increase max simultaneous TCP connections")
    SESSION.backup([_TCPIP, _AFD], "MaxConnections")
    SESSION.set_dword(_TCPIP, "MaxUserPort", 65534)
    SESSION.set_dword(_TCPIP, "TcpTimedWaitDelay", 30)
    SESSION.set_dword(_AFD, "DefaultReceiveWindow", 65535)
    SESSION.set_dword(_AFD, "DefaultSendWindow", 65535)


def _remove_max_connections(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "MaxUserPort")
    SESSION.delete_value(_TCPIP, "TcpTimedWaitDelay")
    SESSION.delete_value(_AFD, "DefaultReceiveWindow")
    SESSION.delete_value(_AFD, "DefaultSendWindow")


def _detect_max_connections() -> bool:
    return SESSION.read_dword(_TCPIP, "MaxUserPort") == 65534


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="increase-irpstack",
        label="Increase IRPStackSize",
        category="Network",
        apply_fn=_apply_irpstack,
        remove_fn=_remove_irpstack,
        detect_fn=_detect_irpstack,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LANMAN],
        description=(
            "Increases the I/O Request Packet stack size to 32 for "
            "better network/file-sharing throughput."
        ),
        tags=["network", "performance", "smb"],
    ),
    TweakDef(
        id="disable-nagle",
        label="Disable Nagle Algorithm (Low Latency)",
        category="Network",
        apply_fn=_apply_disable_nagle,
        remove_fn=_remove_disable_nagle,
        detect_fn=_detect_disable_nagle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCPIP],
        description=(
            "Disables TCP Nagle algorithm (TcpAckFrequency=1, TCPNoDelay=1) "
            "for lower latency in games and remote sessions."
        ),
        tags=["network", "latency", "gaming"],
    ),
    TweakDef(
        id="disable-network-throttle",
        label="Disable Network Throttling",
        category="Network",
        apply_fn=_apply_disable_throttle,
        remove_fn=_remove_disable_throttle,
        detect_fn=_detect_disable_throttle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_THROTTLE],
        description=(
            "Removes the multimedia network throttling index, allowing "
            "full bandwidth usage during media playback."
        ),
        tags=["network", "performance", "bandwidth"],
    ),
    TweakDef(
        id="enable-rdp",
        label="Enable Remote Desktop",
        category="Network",
        apply_fn=_apply_enable_rdp,
        remove_fn=_remove_enable_rdp,
        detect_fn=_detect_enable_rdp,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TERM_SERVER, _RDP_TCP],
        description="Enables Remote Desktop with Network Level Authentication (NLA).",
        tags=["network", "remote", "rdp"],
    ),
    TweakDef(
        id="enable-dns-over-https",
        label="Enable DNS-over-HTTPS",
        category="Network",
        apply_fn=_apply_doh,
        remove_fn=_remove_doh,
        detect_fn=_detect_doh,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DNS_CLIENT],
        description=(
            "Enables automatic DNS-over-HTTPS (DoH) for encrypted DNS "
            "resolution."
        ),
        tags=["network", "privacy", "dns", "security"],
    ),
    TweakDef(
        id="increase-max-tcp",
        label="Increase Max TCP Connections",
        category="Network",
        apply_fn=_apply_max_connections,
        remove_fn=_remove_max_connections,
        detect_fn=_detect_max_connections,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCPIP, _AFD],
        description=(
            "Increases the max user port to 65534, reduces TIME_WAIT "
            "delay, and enlarges default socket buffer sizes."
        ),
        tags=["network", "performance", "tcp"],
    ),
]
