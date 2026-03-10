"""Network and connectivity registry tweaks.

Covers IRPStackSize, DNS over HTTPS, network throttling,
Remote Desktop, and TCP/IP optimizations.
"""

from __future__ import annotations

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
_WIFI_SENSE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc"
    r"\wifinetworkmanager\config"
)
_NETBT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\NetBT\Parameters"
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


# ── Disable Wi-Fi Sense ────────────────────────────────────────────────────


def _apply_disable_wifi_sense(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable Wi-Fi Sense auto-connect")
    SESSION.backup([_WIFI_SENSE], "WiFiSense")
    SESSION.set_dword(_WIFI_SENSE, "AutoConnectAllowedOEM", 0)


def _remove_disable_wifi_sense(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WIFI_SENSE, "AutoConnectAllowedOEM")


def _detect_disable_wifi_sense() -> bool:
    return SESSION.read_dword(_WIFI_SENSE, "AutoConnectAllowedOEM") == 0


# ── Disable NetBIOS over TCP/IP ────────────────────────────────────────────


def _apply_disable_netbios(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable NetBIOS over TCP/IP")
    SESSION.backup([_NETBT], "NetBIOS")
    SESSION.set_dword(_NETBT, "NodeType", 2)  # 2 = P-node (no broadcast)
    SESSION.set_dword(_NETBT, "EnableLMHOSTS", 0)


def _remove_disable_netbios(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NETBT, "NodeType")
    SESSION.set_dword(_NETBT, "EnableLMHOSTS", 1)


def _detect_disable_netbios() -> bool:
    return SESSION.read_dword(_NETBT, "NodeType") == 2


# ── Disable LLMNR (Link-Local Multicast Name Resolution) ─────────────────────

_LLMNR = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"


def _apply_disable_llmnr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable LLMNR")
    SESSION.backup([_LLMNR], "LLMNR")
    SESSION.set_dword(_LLMNR, "EnableMulticast", 0)


def _remove_disable_llmnr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LLMNR, "EnableMulticast")


def _detect_disable_llmnr() -> bool:
    return SESSION.read_dword(_LLMNR, "EnableMulticast") == 0


# ── Disable WPAD (Web Proxy Auto-Discovery) ──────────────────────────────────

_WPAD = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad"


def _apply_disable_wpad(*, require_admin: bool = False) -> None:
    SESSION.log("Network: disable WPAD auto-proxy discovery")
    SESSION.backup([_WPAD], "WPAD")
    SESSION.set_dword(_WPAD, "WpadOverride", 1)


def _remove_disable_wpad(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_WPAD, "WpadOverride")


def _detect_disable_wpad() -> bool:
    return SESSION.read_dword(_WPAD, "WpadOverride") == 1


# ── Enable ECN (Explicit Congestion Notification) ────────────────────────────


def _apply_enable_ecn(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: enable TCP ECN")
    SESSION.backup([_TCPIP], "ECN")
    SESSION.set_dword(_TCPIP, "EnableECN", 1)


def _remove_enable_ecn(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "EnableECN")


def _detect_enable_ecn() -> bool:
    return SESSION.read_dword(_TCPIP, "EnableECN") == 1


# ── Disable SMBv1 Client ─────────────────────────────────────────────────────

_SMB1 = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"


def _apply_disable_smbv1(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable SMBv1 client")
    SESSION.backup([_SMB1], "SMBv1")
    SESSION.set_dword(_SMB1, "EnableSecuritySignature", 1)
    _svc = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\mrxsmb10"
    SESSION.set_dword(_svc, "Start", 4)  # Disable mrxsmb10 driver


def _remove_disable_smbv1(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _svc = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\mrxsmb10"
    SESSION.set_dword(_svc, "Start", 3)


def _detect_disable_smbv1() -> bool:
    _svc = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\mrxsmb10"
    return SESSION.read_dword(_svc, "Start") == 4


# ── Increase DNS Cache TTL ───────────────────────────────────────────────────


def _apply_increase_dns_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: increase DNS cache TTL to 86400s (24h)")
    SESSION.backup([_DNS_CLIENT], "DNSCache")
    SESSION.set_dword(_DNS_CLIENT, "MaxCacheTtl", 86400)
    SESSION.set_dword(_DNS_CLIENT, "MaxNegativeCacheTtl", 5)


def _remove_increase_dns_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_CLIENT, "MaxCacheTtl")
    SESSION.delete_value(_DNS_CLIENT, "MaxNegativeCacheTtl")


def _detect_increase_dns_cache() -> bool:
    return SESSION.read_dword(_DNS_CLIENT, "MaxCacheTtl") == 86400


# ── Increase Network Throttling Index ────────────────────────────────────────


def _apply_throttling_index(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: set network throttling index to 0xFFFFFFFF")
    SESSION.backup([_THROTTLE], "ThrottlingIndex")
    SESSION.set_dword(_THROTTLE, "NetworkThrottlingIndex", 0xFFFFFFFF)


def _remove_throttling_index(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_THROTTLE, "NetworkThrottlingIndex", 10)


def _detect_throttling_index() -> bool:
    val = SESSION.read_dword(_THROTTLE, "NetworkThrottlingIndex")
    return val is not None and val in (0xFFFFFFFF, -1)


# ── Increase ARP Cache Size ────────────────────────────────────────────────


def _apply_increase_arp_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: increase ARP cache size to 3600s")
    SESSION.backup([_TCPIP], "ARPCache")
    SESSION.set_dword(_TCPIP, "ArpCacheLife", 3600)
    SESSION.set_dword(_TCPIP, "ArpCacheMinReferencedLife", 3600)


def _remove_increase_arp_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "ArpCacheLife")
    SESSION.delete_value(_TCPIP, "ArpCacheMinReferencedLife")


def _detect_increase_arp_cache() -> bool:
    return SESSION.read_dword(_TCPIP, "ArpCacheLife") == 3600


# ── Enable Receive Side Scaling (RSS) ────────────────────────────────────

_NDIS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\NDIS\Parameters"
)


def _apply_rss_enable(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: enable Receive Side Scaling (RSS)")
    SESSION.backup([_NDIS, _TCPIP], "RSS")
    SESSION.set_dword(_NDIS, "RssBaseCpu", 1)
    SESSION.set_dword(_TCPIP, "EnableRSS", 1)


def _remove_rss_enable(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NDIS, "RssBaseCpu")
    SESSION.delete_value(_TCPIP, "EnableRSS")


def _detect_rss_enable() -> bool:
    return SESSION.read_dword(_TCPIP, "EnableRSS") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="net-increase-irpstack",
        label="Increase IRPStackSize",
        category="Network",
        apply_fn=_apply_irpstack,
        remove_fn=_remove_irpstack,
        detect_fn=_detect_irpstack,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LANMAN],
        description=("Increases the I/O Request Packet stack size to 32 for better network/file-sharing throughput."),
        tags=["network", "performance", "smb"],
    ),
    TweakDef(
        id="net-disable-network-throttle",
        label="Disable Network Throttling",
        category="Network",
        apply_fn=_apply_disable_throttle,
        remove_fn=_remove_disable_throttle,
        detect_fn=_detect_disable_throttle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_THROTTLE],
        description=("Removes the multimedia network throttling index, allowing full bandwidth usage during media playback."),
        tags=["network", "performance", "bandwidth"],
    ),
    TweakDef(
        id="net-enable-rdp",
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
        id="net-enable-dns-over-https",
        label="Enable DNS-over-HTTPS",
        category="Network",
        apply_fn=_apply_doh,
        remove_fn=_remove_doh,
        detect_fn=_detect_doh,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DNS_CLIENT],
        description=("Enables automatic DNS-over-HTTPS (DoH) for encrypted DNS resolution."),
        tags=["network", "privacy", "dns", "security"],
    ),
    TweakDef(
        id="net-increase-max-tcp",
        label="Increase Max TCP Connections",
        category="Network",
        apply_fn=_apply_max_connections,
        remove_fn=_remove_max_connections,
        detect_fn=_detect_max_connections,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCPIP, _AFD],
        description=("Increases the max user port to 65534, reduces TIME_WAIT delay, and enlarges default socket buffer sizes."),
        tags=["network", "performance", "tcp"],
    ),
    TweakDef(
        id="net-disable-wifi-sense",
        label="Disable Wi-Fi Sense",
        category="Network",
        apply_fn=_apply_disable_wifi_sense,
        remove_fn=_remove_disable_wifi_sense,
        detect_fn=_detect_disable_wifi_sense,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WIFI_SENSE],
        description="Disables Wi-Fi Sense auto-connect to suggested open hotspots.",
        tags=["network", "wifi", "privacy", "security"],
    ),
    TweakDef(
        id="net-disable-netbios",
        label="Disable NetBIOS over TCP/IP",
        category="Network",
        apply_fn=_apply_disable_netbios,
        remove_fn=_remove_disable_netbios,
        detect_fn=_detect_disable_netbios,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_NETBT],
        description="Disables NetBIOS name resolution and LMHOSTS lookup for security.",
        tags=["network", "security", "netbios"],
    ),
    TweakDef(
        id="net-disable-llmnr",
        label="Disable LLMNR",
        category="Network",
        apply_fn=_apply_disable_llmnr,
        remove_fn=_remove_disable_llmnr,
        detect_fn=_detect_disable_llmnr,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LLMNR],
        description=("Disables Link-Local Multicast Name Resolution. Mitigates LLMNR poisoning attacks on enterprise networks."),
        tags=["network", "security", "llmnr", "enterprise"],
    ),
    TweakDef(
        id="net-disable-wpad",
        label="Disable WPAD Auto-Proxy",
        category="Network",
        apply_fn=_apply_disable_wpad,
        remove_fn=_remove_disable_wpad,
        detect_fn=_detect_disable_wpad,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WPAD],
        description=("Disables Web Proxy Auto-Discovery (WPAD). Prevents rogue WPAD attacks on untrusted networks."),
        tags=["network", "security", "proxy", "wpad"],
    ),
    TweakDef(
        id="net-enable-ecn",
        label="Enable TCP ECN",
        category="Network",
        apply_fn=_apply_enable_ecn,
        remove_fn=_remove_enable_ecn,
        detect_fn=_detect_enable_ecn,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCPIP],
        description=("Enables Explicit Congestion Notification for smarter TCP congestion control without packet loss."),
        tags=["network", "performance", "ecn", "tcp"],
    ),
    TweakDef(
        id="net-disable-smbv1",
        label="Disable SMBv1 Client",
        category="Network",
        apply_fn=_apply_disable_smbv1,
        remove_fn=_remove_disable_smbv1,
        detect_fn=_detect_disable_smbv1,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SMB1],
        description=("Disables the legacy and insecure SMBv1 protocol. Protects against EternalBlue and similar exploits."),
        tags=["network", "security", "smb", "enterprise"],
    ),
    TweakDef(
        id="net-increase-dns-cache",
        label="Increase DNS Cache TTL (24h)",
        category="Network",
        apply_fn=_apply_increase_dns_cache,
        remove_fn=_remove_increase_dns_cache,
        detect_fn=_detect_increase_dns_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DNS_CLIENT],
        description=("Increases the DNS cache TTL to 24 hours and reduces negative cache to 5 seconds for faster repeat lookups."),
        tags=["network", "performance", "dns", "cache"],
    ),
    TweakDef(
        id="net-throttling-index",
        label="Increase Network Throttling Index",
        category="Network",
        apply_fn=_apply_throttling_index,
        remove_fn=_remove_throttling_index,
        detect_fn=_detect_throttling_index,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_THROTTLE],
        description=(
            "Disables network throttling during multimedia playback. "
            "Prevents Windows from limiting network bandwidth. "
            "Default: 10. Recommended: 0xFFFFFFFF (disabled)."
        ),
        tags=["network", "throttling", "performance", "bandwidth"],
    ),
    TweakDef(
        id="net-increase-arp-cache",
        label="Increase ARP Cache Size",
        category="Network",
        apply_fn=_apply_increase_arp_cache,
        remove_fn=_remove_increase_arp_cache,
        detect_fn=_detect_increase_arp_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCPIP],
        description=(
            "Increases the ARP cache lifetime to 3600 seconds (1 hour). "
            "Reduces ARP broadcast traffic on busy networks and speeds up "
            "repeated connections to known hosts. "
            "Default: 120s. Recommended: 3600s."
        ),
        tags=["network", "arp", "cache", "performance"],
    ),
    TweakDef(
        id="net-rss-enable",
        label="Enable Receive Side Scaling (RSS)",
        category="Network",
        apply_fn=_apply_rss_enable,
        remove_fn=_remove_rss_enable,
        detect_fn=_detect_rss_enable,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NDIS, _TCPIP],
        description=(
            "Enables Receive Side Scaling (RSS) to distribute network "
            "receive processing across multiple CPU cores. Improves "
            "throughput on multi-core systems with supported NICs. "
            "Default: OS-managed. Recommended: enabled."
        ),
        tags=["network", "rss", "performance", "throughput", "multicore"],
    ),
]


# ── Disable Nagle's Algorithm ────────────────────────────────────────────────


def _apply_nagle_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable Nagle's algorithm (TcpNoDelay)")
    SESSION.backup([_TCPIP], "NagleOff")
    SESSION.set_dword(_TCPIP, "TcpNoDelay", 1)


def _remove_nagle_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "TcpNoDelay")


def _detect_nagle_off() -> bool:
    return SESSION.read_dword(_TCPIP, "TcpNoDelay") == 1


# ── Increase Max User Port Range ─────────────────────────────────────────────


def _apply_increase_max_port(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: increase max user port to 65534")
    SESSION.backup([_TCPIP], "MaxUserPort")
    SESSION.set_dword(_TCPIP, "MaxUserPort", 65534)


def _remove_increase_max_port(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "MaxUserPort")


def _detect_increase_max_port() -> bool:
    return SESSION.read_dword(_TCPIP, "MaxUserPort") == 65534


TWEAKS += [
    TweakDef(
        id="net-disable-nagle",
        label="Disable Nagle's Algorithm",
        category="Network",
        apply_fn=_apply_nagle_off,
        remove_fn=_remove_nagle_off,
        detect_fn=_detect_nagle_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TCPIP],
        description=(
            "Disables Nagle's algorithm via TcpNoDelay for lower network latency. "
            "Sends TCP packets immediately without buffering. "
            "Default: Enabled. Recommended: Disabled for gaming/real-time."
        ),
        tags=["network", "nagle", "tcp", "latency", "gaming"],
    ),
    TweakDef(
        id="net-increase-max-connections",
        label="Increase Max User Port Range",
        category="Network",
        apply_fn=_apply_increase_max_port,
        remove_fn=_remove_increase_max_port,
        detect_fn=_detect_increase_max_port,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TCPIP],
        description=(
            "Increases maximum ephemeral port range to 65534. Allows more "
            "concurrent outbound connections for high-throughput workloads. "
            "Default: 5000. Recommended: 65534 for servers."
        ),
        tags=["network", "port", "connections", "throughput", "server"],
    ),
]


# ══ Additional Network Tweaks (Sophia Script / WinUtil) ══════════════════

_TEREDO = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters"
_TCPIP6 = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"
_ISATAP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters"
_LMHOSTS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"


# -- Disable Teredo Tunneling ────────────────────────────────────────────


def _apply_disable_teredo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable Teredo tunneling")
    SESSION.backup([_TEREDO], "DisableTeredo")
    # Type 4 = disabled
    SESSION.set_dword(_TEREDO, "Type", 4)


def _remove_disable_teredo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TEREDO, "Type", 0)


def _detect_disable_teredo() -> bool:
    return SESSION.read_dword(_TEREDO, "Type") == 4


# -- Prefer IPv4 over IPv6 ──────────────────────────────────────────────


def _apply_prefer_ipv4(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: prefer IPv4 over IPv6 (DisabledComponents=0x20)")
    SESSION.backup([_TCPIP6], "PreferIPv4")
    SESSION.set_dword(_TCPIP6, "DisabledComponents", 0x20)


def _remove_prefer_ipv4(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TCPIP6, "DisabledComponents", 0)


def _detect_prefer_ipv4() -> bool:
    val = SESSION.read_dword(_TCPIP6, "DisabledComponents")
    return val is not None and (val & 0x20) != 0


# -- Disable ISATAP ──────────────────────────────────────────────────────


def _apply_disable_isatap(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable ISATAP adapter")
    SESSION.backup([_ISATAP], "DisableISATAP")
    SESSION.set_string(_ISATAP, "State", "Disabled")


def _remove_disable_isatap(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_ISATAP, "State", "Default")


def _detect_disable_isatap() -> bool:
    return SESSION.read_string(_ISATAP, "State") == "Disabled"


# -- Disable LMHOSTS Lookup ─────────────────────────────────────────────


def _apply_disable_lmhosts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable LMHOSTS lookup")
    SESSION.backup([_LMHOSTS], "DisableLMHOSTS")
    SESSION.set_dword(_LMHOSTS, "EnableLMHOSTS", 0)


def _remove_disable_lmhosts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LMHOSTS, "EnableLMHOSTS", 1)


def _detect_disable_lmhosts() -> bool:
    return SESSION.read_dword(_LMHOSTS, "EnableLMHOSTS") == 0


TWEAKS += [
    TweakDef(
        id="net-disable-teredo",
        label="Disable Teredo Tunneling",
        category="Network",
        apply_fn=_apply_disable_teredo,
        remove_fn=_remove_disable_teredo,
        detect_fn=_detect_disable_teredo,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TEREDO],
        description=("Disables Teredo IPv6 tunneling which is rarely used and can be a security risk. Default: enabled. Recommended: disabled."),
        tags=["network", "teredo", "ipv6", "tunneling", "security"],
    ),
    TweakDef(
        id="net-prefer-ipv4",
        label="Prefer IPv4 over IPv6",
        category="Network",
        apply_fn=_apply_prefer_ipv4,
        remove_fn=_remove_prefer_ipv4,
        detect_fn=_detect_prefer_ipv4,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TCPIP6],
        description=(
            "Configures Windows to prefer IPv4 over IPv6 for DNS resolution and "
            "connections. Useful for networks without proper IPv6 infrastructure. "
            "Default: IPv6 preferred. Recommended: IPv4 for compatibility."
        ),
        tags=["network", "ipv4", "ipv6", "dns", "priority"],
    ),
    TweakDef(
        id="net-disable-isatap",
        label="Disable ISATAP",
        category="Network",
        apply_fn=_apply_disable_isatap,
        remove_fn=_remove_disable_isatap,
        detect_fn=_detect_disable_isatap,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ISATAP],
        description=("Disables the ISATAP IPv6 transition adapter. Removes an unnecessary virtual adapter. Default: enabled. Recommended: disabled."),
        tags=["network", "isatap", "ipv6", "adapter", "security"],
    ),
    TweakDef(
        id="net-disable-lmhosts",
        label="Disable LMHOSTS Lookup",
        category="Network",
        apply_fn=_apply_disable_lmhosts,
        remove_fn=_remove_disable_lmhosts,
        detect_fn=_detect_disable_lmhosts,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LMHOSTS],
        description=(
            "Disables LMHOSTS file lookup for NetBIOS name resolution. "
            "Reduces legacy protocol attack surface. "
            "Default: enabled. Recommended: disabled on modern networks."
        ),
        tags=["network", "lmhosts", "netbios", "security", "legacy"],
    ),
]


# ── Set TCP Auto-Tuning to Restricted ────────────────────────────────────────


def _apply_tcp_autotune_restricted(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: set TCP receive window auto-tuning to restricted")
    SESSION.backup([_TCPIP], "TCPAutoTune")
    SESSION.set_dword(_TCPIP, "EnableAutoTuning", 0)


def _remove_tcp_autotune_restricted(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "EnableAutoTuning")


def _detect_tcp_autotune_restricted() -> bool:
    return SESSION.read_dword(_TCPIP, "EnableAutoTuning") == 0


TWEAKS += [
    TweakDef(
        id="net-tcp-autotune-restricted",
        label="Set TCP Auto-Tuning to Restricted",
        category="Network",
        apply_fn=_apply_tcp_autotune_restricted,
        remove_fn=_remove_tcp_autotune_restricted,
        detect_fn=_detect_tcp_autotune_restricted,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TCPIP],
        description=(
            "Sets TCP receive window auto-tuning to restricted mode. "
            "Can improve compatibility with older routers/firewalls. "
            "Default: normal. Recommended: restricted for problematic networks."
        ),
        tags=["network", "tcp", "autotune", "window", "restricted"],
    ),
]


# ── Enable SMB Packet Signing ────────────────────────────────────────────────

_SMB_PARAMS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"


def _apply_enable_smb_signing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: enable SMB packet signing for all connections")
    SESSION.backup([_SMB_PARAMS], "SmbSigning")
    SESSION.set_dword(_SMB_PARAMS, "RequireSecuritySignature", 1)
    SESSION.set_dword(_SMB_PARAMS, "EnableSecuritySignature", 1)


def _remove_enable_smb_signing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SMB_PARAMS, "RequireSecuritySignature", 0)
    SESSION.set_dword(_SMB_PARAMS, "EnableSecuritySignature", 0)


def _detect_enable_smb_signing() -> bool:
    return SESSION.read_dword(_SMB_PARAMS, "RequireSecuritySignature") == 1


# ── Disable Windows Network Location Wizard ──────────────────────────────────

_NET_LOC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff"


def _apply_disable_network_wizard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: suppress 'Set Network Location' wizard on new connections")
    SESSION.backup([_NET_LOC], "NetworkWizard")
    SESSION.set_dword(_NET_LOC, "(Default)", 0)


def _remove_disable_network_wizard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NET_LOC, "(Default)")


def _detect_disable_network_wizard() -> bool:
    return SESSION.key_exists(_NET_LOC)


# ── Increase TCP Initial Congestion Window ────────────────────────────────────

_TCPIP_IFACE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces"


def _apply_increase_initial_cwnd(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: set TCP initial receive window to 64 KB")
    SESSION.backup([r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"], "InitialCwnd")
    SESSION.set_dword(r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPInitialRTT", 3)


def _remove_increase_initial_cwnd(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPInitialRTT")


def _detect_increase_initial_cwnd() -> bool:
    return SESSION.read_dword(r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPInitialRTT") == 3


# ── Disable Network Access to Named Pipes ────────────────────────────────────

_RESTRICT_ANON = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"


def _apply_restrict_anon_pipes(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: restrict anonymous access to named pipes and shares")
    SESSION.backup([_RESTRICT_ANON], "RestrictAnon")
    SESSION.set_dword(_RESTRICT_ANON, "RestrictAnonymous", 1)
    SESSION.set_dword(_RESTRICT_ANON, "RestrictAnonymousSAM", 1)


def _remove_restrict_anon_pipes(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RESTRICT_ANON, "RestrictAnonymous", 0)
    SESSION.set_dword(_RESTRICT_ANON, "RestrictAnonymousSAM", 0)


def _detect_restrict_anon_pipes() -> bool:
    return SESSION.read_dword(_RESTRICT_ANON, "RestrictAnonymous") == 1


TWEAKS += [
    TweakDef(
        id="net-enable-smb-signing",
        label="Require SMB Packet Signing",
        category="Network",
        apply_fn=_apply_enable_smb_signing,
        remove_fn=_remove_enable_smb_signing,
        detect_fn=_detect_enable_smb_signing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SMB_PARAMS],
        description=(
            "Enforces SMB packet signing on all server connections. "
            "Protects against NTLM relay and man-in-the-middle attacks on file shares. "
            "Default: Not required. Recommended: Enabled on corp networks."
        ),
        tags=["network", "smb", "signing", "security", "ntlm"],
    ),
    TweakDef(
        id="net-disable-network-wizard",
        label="Suppress Network Location Wizard",
        category="Network",
        apply_fn=_apply_disable_network_wizard,
        remove_fn=_remove_disable_network_wizard,
        detect_fn=_detect_disable_network_wizard,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NET_LOC],
        description=(
            "Suppresses the 'Set Network Location' dialog when connecting to a new network. "
            "Useful on headless/server machines. Default: Enabled (dialog appears)."
        ),
        tags=["network", "wizard", "dialog", "location", "server"],
    ),
    TweakDef(
        id="net-tcp-initial-rtt",
        label="Reduce TCP Initial RTT Estimate",
        category="Network",
        apply_fn=_apply_increase_initial_cwnd,
        remove_fn=_remove_increase_initial_cwnd,
        detect_fn=_detect_increase_initial_cwnd,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
        description=(
            "Sets TCPInitialRTT to 3 seconds, reducing the initial retransmission timeout "
            "to speed up TCP connection setup on fast local networks. Default: 3 (Windows default varies)."
        ),
        tags=["network", "tcp", "rtt", "latency", "performance"],
    ),
    TweakDef(
        id="net-restrict-anonymous",
        label="Restrict Anonymous Network Access",
        category="Network",
        apply_fn=_apply_restrict_anon_pipes,
        remove_fn=_remove_restrict_anon_pipes,
        detect_fn=_detect_restrict_anon_pipes,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RESTRICT_ANON],
        description=(
            "Prevents anonymous users from enumerating SAM accounts, shares, and named pipes. "
            "Security baseline hardening. Default: Off. Recommended: Enabled."
        ),
        tags=["network", "anonymous", "sam", "security", "hardening"],
    ),
]

# ── Extra network controls ───────────────────────────────────────────────────

_NET_KEEP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"
_SMB2_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation"
_WIFI_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\Local"
_TCP_CHIMNEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"
_NET_DEV = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"


def _apply_net_increase_keepalive_time(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NET_KEEP], "KeepAliveTime")
    SESSION.set_dword(_NET_KEEP, "KeepAliveTime", 300000)  # 5 minutes


def _remove_net_increase_keepalive_time(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NET_KEEP, "KeepAliveTime")


def _detect_net_increase_keepalive_time() -> bool:
    return SESSION.read_dword(_NET_KEEP, "KeepAliveTime") == 300000


def _apply_net_enable_smb2_signing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SMB2_POLICY], "SMB2Signing")
    SESSION.set_dword(_SMB2_POLICY, "RequireSecuritySignature", 1)


def _remove_net_enable_smb2_signing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SMB2_POLICY, "RequireSecuritySignature")


def _detect_net_enable_smb2_signing() -> bool:
    return SESSION.read_dword(_SMB2_POLICY, "RequireSecuritySignature") == 1


def _apply_net_disable_wifi_pnp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WIFI_POLICY], "WifiPnP")
    SESSION.set_dword(_WIFI_POLICY, "fBlockNonDomain", 1)


def _remove_net_disable_wifi_pnp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WIFI_POLICY, "fBlockNonDomain")


def _detect_net_disable_wifi_pnp() -> bool:
    return SESSION.read_dword(_WIFI_POLICY, "fBlockNonDomain") == 1


def _apply_net_disable_tcp_syn_attack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_TCP_CHIMNEY], "SYNAttack")
    SESSION.set_dword(_TCP_CHIMNEY, "TcpMaxHalfOpen", 100)
    SESSION.set_dword(_TCP_CHIMNEY, "TcpMaxHalfOpenRetried", 80)
    SESSION.set_dword(_TCP_CHIMNEY, "SynAttackProtect", 1)


def _remove_net_disable_tcp_syn_attack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCP_CHIMNEY, "TcpMaxHalfOpen")
    SESSION.delete_value(_TCP_CHIMNEY, "TcpMaxHalfOpenRetried")
    SESSION.delete_value(_TCP_CHIMNEY, "SynAttackProtect")


def _detect_net_disable_tcp_syn_attack() -> bool:
    return SESSION.read_dword(_TCP_CHIMNEY, "SynAttackProtect") == 1


def _apply_net_enable_tcp_timestamps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NET_DEV], "TCPTimestamps")
    SESSION.set_dword(_NET_DEV, "Tcp1323Opts", 1)


def _remove_net_enable_tcp_timestamps(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NET_DEV, "Tcp1323Opts")


def _detect_net_enable_tcp_timestamps() -> bool:
    return SESSION.read_dword(_NET_DEV, "Tcp1323Opts") == 1


TWEAKS += [
    TweakDef(
        id="net-tcp-keepalive-5min",
        label="Set TCP Keep-Alive Time to 5 Minutes",
        category="Network",
        apply_fn=_apply_net_increase_keepalive_time,
        remove_fn=_remove_net_increase_keepalive_time,
        detect_fn=_detect_net_increase_keepalive_time,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NET_KEEP],
        description=(
            "Sets TCP keep-alive interval to 300,000ms (5 minutes) instead of the default 2 hours. "
            "Helps detect dead connections faster in long-lived TCP sessions. "
            "Default: 7,200,000ms. Recommended: 300,000ms."
        ),
        tags=["network", "tcp", "keepalive", "connection", "timeout"],
    ),
    TweakDef(
        id="net-smb2-require-signing",
        label="Require SMB2/3 Packet Signing (Workstation)",
        category="Network",
        apply_fn=_apply_net_enable_smb2_signing,
        remove_fn=_remove_net_enable_smb2_signing,
        detect_fn=_detect_net_enable_smb2_signing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SMB2_POLICY],
        description=(
            "Requires SMB packet signing on the workstation side for all SMB2/3 connections. "
            "Prevents SMB relay attacks. Default: Not required. Recommended: Required."
        ),
        tags=["network", "smb", "signing", "security", "relay"],
    ),
    TweakDef(
        id="net-block-non-domain-wifi",
        label="Block Non-Domain Wi-Fi Networks (Managed)",
        category="Network",
        apply_fn=_apply_net_disable_wifi_pnp,
        remove_fn=_remove_net_disable_wifi_pnp,
        detect_fn=_detect_net_disable_wifi_pnp,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WIFI_POLICY],
        description=(
            "Prevents Windows from connecting to non-domain Wi-Fi networks. "
            "Enforces network boundary for domain-joined machines. "
            "Default: Allowed. Recommended: Blocked on managed corporate devices."
        ),
        tags=["network", "wifi", "domain", "policy", "corporate"],
    ),
    TweakDef(
        id="net-tcp-syn-attack-protection",
        label="Enable TCP SYN Attack Protection",
        category="Network",
        apply_fn=_apply_net_disable_tcp_syn_attack,
        remove_fn=_remove_net_disable_tcp_syn_attack,
        detect_fn=_detect_net_disable_tcp_syn_attack,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCP_CHIMNEY],
        description=(
            "Enables SYN attack protection with conservative half-open connection limits. "
            "Mitigates SYN flood denial-of-service attacks on exposed systems. "
            "Default: Disabled. Recommended: Enabled on exposed systems."
        ),
        tags=["network", "tcp", "syn", "attack", "security", "dos"],
    ),
    TweakDef(
        id="net-tcp-timestamps",
        label="Enable TCP Timestamps and Window Scaling",
        category="Network",
        apply_fn=_apply_net_enable_tcp_timestamps,
        remove_fn=_remove_net_enable_tcp_timestamps,
        detect_fn=_detect_net_enable_tcp_timestamps,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NET_DEV],
        description=(
            "Enables TCP timestamps (RFC 1323) for more accurate RTT calculations. "
            "Improves TCP performance on high-bandwidth connections. "
            "Default: Not set. Recommended: Enabled."
        ),
        tags=["network", "tcp", "timestamps", "rfc1323", "performance"],
    ),
]


# ── Disable IPv6 Entirely ─────────────────────────────────────────────────────

_TCPIP6 = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"


def _apply_net_disable_ipv6(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable all IPv6 tunneling components (DisabledComponents=255)")
    SESSION.backup([_TCPIP6], "DisableIPv6")
    SESSION.set_dword(_TCPIP6, "DisabledComponents", 255)


def _remove_net_disable_ipv6(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP6, "DisabledComponents")


def _detect_net_disable_ipv6() -> bool:
    return SESSION.read_dword(_TCPIP6, "DisabledComponents") == 255


# ── Disable Multicast DNS (mDNS) ──────────────────────────────────────────────

_DNS_PARAMS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"


def _apply_net_disable_mdns(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable multicast DNS (mDNS)")
    SESSION.backup([_DNS_PARAMS], "DisableMDNS")
    SESSION.set_dword(_DNS_PARAMS, "EnableMDNS", 0)


def _remove_net_disable_mdns(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_PARAMS, "EnableMDNS")


def _detect_net_disable_mdns() -> bool:
    val = SESSION.read_dword(_DNS_PARAMS, "EnableMDNS")
    return val is not None and val == 0


# ── Disable Peer-to-Peer Networking Service ───────────────────────────────────

_PNRP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc"


def _apply_net_disable_pnrp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable Peer Name Resolution Protocol (PNRP) service")
    SESSION.backup([_PNRP], "PNRPStart")
    SESSION.set_dword(_PNRP, "Start", 4)


def _remove_net_disable_pnrp(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PNRP, "Start", 3)


def _detect_net_disable_pnrp() -> bool:
    return SESSION.read_dword(_PNRP, "Start") == 4


# ── Disable WCN (Windows Connect Now) ────────────────────────────────────────

_WCN_POL = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars"


def _apply_net_disable_wcn(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable Windows Connect Now (WCN) registrar")
    SESSION.backup([_WCN_POL], "DisableWCN")
    SESSION.set_dword(_WCN_POL, "DisableWPDRegistrar", 1)
    SESSION.set_dword(_WCN_POL, "EnableRegistrars", 0)


def _remove_net_disable_wcn(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WCN_POL, "DisableWPDRegistrar")
    SESSION.delete_value(_WCN_POL, "EnableRegistrars")


def _detect_net_disable_wcn() -> bool:
    return SESSION.read_dword(_WCN_POL, "EnableRegistrars") == 0


# ── Disable TCP/IP Task Offload ───────────────────────────────────────────────

_OFFLOAD = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters"


def _apply_net_disable_task_offload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Network: disable TCP/IP task offload for compatibility")
    SESSION.backup([_OFFLOAD], "TCPOffload")
    SESSION.set_dword(_OFFLOAD, "DisableTaskOffload", 1)


def _remove_net_disable_task_offload(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OFFLOAD, "DisableTaskOffload")


def _detect_net_disable_task_offload() -> bool:
    return SESSION.read_dword(_OFFLOAD, "DisableTaskOffload") == 1


TWEAKS += [
    TweakDef(
        id="net-disable-ipv6",
        label="Disable All IPv6 Tunneling Components",
        category="Network",
        apply_fn=_apply_net_disable_ipv6,
        remove_fn=_remove_net_disable_ipv6,
        detect_fn=_detect_net_disable_ipv6,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TCPIP6],
        description=(
            "Sets DisabledComponents=255 to disable all IPv6 tunnel adapters "
            "(6to4, ISATAP, Teredo, etc.) at once. Reduces attack surface on IPv4-only networks. "
            "Default: Enabled. Recommended: Disabled on pure IPv4 networks."
        ),
        tags=["network", "ipv6", "tunnel", "6to4", "isatap", "teredo", "security"],
        depends_on=[],
        side_effects="IPv6 connectivity is fully disabled.",
    ),
    TweakDef(
        id="net-disable-mdns",
        label="Disable Multicast DNS (mDNS)",
        category="Network",
        apply_fn=_apply_net_disable_mdns,
        remove_fn=_remove_net_disable_mdns,
        detect_fn=_detect_net_disable_mdns,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DNS_PARAMS],
        description=(
            "Disables the mDNS responder used for Bonjour/zero-config hostname resolution. "
            "Eliminates local network name broadcast leakage. "
            "Default: Enabled. Recommended: Disabled on managed networks."
        ),
        tags=["network", "mdns", "bonjour", "dns", "privacy", "security"],
        depends_on=[],
        side_effects="Local .local hostname resolution via mDNS will stop working.",
    ),
    TweakDef(
        id="net-disable-pnrp",
        label="Disable Peer Name Resolution Protocol",
        category="Network",
        apply_fn=_apply_net_disable_pnrp,
        remove_fn=_remove_net_disable_pnrp,
        detect_fn=_detect_net_disable_pnrp,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PNRP],
        description=(
            "Disables the PNRP service used for peer-to-peer name resolution. "
            "Eliminates an infrequently used network service. "
            "Default: Manual. Recommended: Disabled."
        ),
        tags=["network", "pnrp", "p2p", "service", "security"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="net-disable-wcn",
        label="Disable Windows Connect Now (WCN)",
        category="Network",
        apply_fn=_apply_net_disable_wcn,
        remove_fn=_remove_net_disable_wcn,
        detect_fn=_detect_net_disable_wcn,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WCN_POL],
        description=(
            "Disables Windows Connect Now which broadcasts Wi-Fi credentials over USB and NFC. "
            "Policy-level control to prevent accidental credential sharing. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["network", "wcn", "wifi", "credentials", "policy", "security"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="net-disable-task-offload",
        label="Disable TCP/IP Task Offload",
        category="Network",
        apply_fn=_apply_net_disable_task_offload,
        remove_fn=_remove_net_disable_task_offload,
        detect_fn=_detect_net_disable_task_offload,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OFFLOAD],
        description=(
            "Disables TCP/IP task offloading to the NIC. Resolves connectivity issues "
            "caused by buggy NIC firmware or driver offload bugs. "
            "Default: Enabled. Recommended: Disabled for troubleshooting network issues."
        ),
        tags=["network", "tcp", "offload", "nic", "compatibility"],
        depends_on=[],
        side_effects="May slightly increase CPU usage for network processing.",
    ),
]
