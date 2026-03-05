"""DNS & Networking Advanced tweaks.

Covers DNS-over-HTTPS policy, DNS cache tuning, LLMNR, mDNS,
NetBIOS hardening, WPAD suppression, TCP auto-tuning, and ECN.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_DNS_CLIENT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\Dnscache\Parameters"
)
_DNS_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"
)
_TCPIP = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\Tcpip\Parameters"
)
_TCPIP6 = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\Tcpip6\Parameters"
)
_NETBT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\NetBT\Parameters"
)
_INET_SETTINGS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Internet Settings"
)
_WPAD_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Internet Settings\Wpad"
)
_MDNS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\Dnscache\Parameters"
)
_LMHOSTS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\NetBT\Parameters"
)
_QOS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched"
)
_THROTTLE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Multimedia\SystemProfile"
)
_AFD = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\AFD\Parameters"
)
_DNS_DOH_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"
)


# ── Force DNS-over-HTTPS via Group Policy ────────────────────────────────────


def _apply_force_doh_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: force DNS-over-HTTPS via policy (DoH=2)")
    SESSION.backup([_DNS_DOH_POLICY], "ForceDohPolicy")
    SESSION.set_dword(_DNS_DOH_POLICY, "DoHPolicy", 3)  # 3 = require DoH


def _remove_force_doh_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_DOH_POLICY, "DoHPolicy")


def _detect_force_doh_policy() -> bool:
    return SESSION.read_dword(_DNS_DOH_POLICY, "DoHPolicy") == 3


# ── Disable DNS Negative Cache ───────────────────────────────────────────────


def _apply_disable_neg_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable negative DNS cache (MaxNegativeCacheTtl=0)")
    SESSION.backup([_DNS_CLIENT], "DnsNegCache")
    SESSION.set_dword(_DNS_CLIENT, "MaxNegativeCacheTtl", 0)


def _remove_disable_neg_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_CLIENT, "MaxNegativeCacheTtl")


def _detect_disable_neg_cache() -> bool:
    return SESSION.read_dword(_DNS_CLIENT, "MaxNegativeCacheTtl") == 0


# ── Set DNS Cache Max Size ───────────────────────────────────────────────────


def _apply_dns_cache_size(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: increase DNS cache entry limit")
    SESSION.backup([_DNS_CLIENT], "DnsCacheSize")
    SESSION.set_dword(_DNS_CLIENT, "MaxCacheEntryTtlLimit", 86400)
    SESSION.set_dword(_DNS_CLIENT, "MaxSOACacheEntryTtlLimit", 300)


def _remove_dns_cache_size(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_CLIENT, "MaxCacheEntryTtlLimit")
    SESSION.delete_value(_DNS_CLIENT, "MaxSOACacheEntryTtlLimit")


def _detect_dns_cache_size() -> bool:
    return SESSION.read_dword(_DNS_CLIENT, "MaxCacheEntryTtlLimit") == 86400


# ── Disable mDNS (Multicast DNS) ────────────────────────────────────────────


def _apply_disable_mdns(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable multicast DNS (mDNS)")
    SESSION.backup([_MDNS], "mDNS")
    SESSION.set_dword(_MDNS, "EnableMDNS", 0)


def _remove_disable_mdns(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MDNS, "EnableMDNS")


def _detect_disable_mdns() -> bool:
    return SESSION.read_dword(_MDNS, "EnableMDNS") == 0


# ── Disable Smart Name Resolution (DNSSEC fallback) ─────────────────────────


def _apply_disable_smart_name(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable smart multi-homed name resolution")
    SESSION.backup([_DNS_POLICY], "SmartNameRes")
    SESSION.set_dword(_DNS_POLICY, "DisableSmartNameResolution", 1)


def _remove_disable_smart_name(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_POLICY, "DisableSmartNameResolution")


def _detect_disable_smart_name() -> bool:
    return SESSION.read_dword(_DNS_POLICY, "DisableSmartNameResolution") == 1


# ── Disable DNS Devolution ───────────────────────────────────────────────────


def _apply_disable_devolution(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable DNS devolution (suffix stripping)")
    SESSION.backup([_DNS_POLICY], "DnsDevolution")
    SESSION.set_dword(_DNS_POLICY, "UseDomainNameDevolution", 0)


def _remove_disable_devolution(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DNS_POLICY, "UseDomainNameDevolution")


def _detect_disable_devolution() -> bool:
    return SESSION.read_dword(_DNS_POLICY, "UseDomainNameDevolution") == 0


# ── Disable LMHOSTS Lookup ──────────────────────────────────────────────────


def _apply_disable_lmhosts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable LMHOSTS lookup")
    SESSION.backup([_LMHOSTS], "LmHosts")
    SESSION.set_dword(_LMHOSTS, "EnableLMHOSTS", 0)


def _remove_disable_lmhosts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LMHOSTS, "EnableLMHOSTS", 1)


def _detect_disable_lmhosts() -> bool:
    return SESSION.read_dword(_LMHOSTS, "EnableLMHOSTS") == 0


# ── Disable QoS Reserved Bandwidth ──────────────────────────────────────────


def _apply_disable_qos_reserve(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: remove QoS reserved bandwidth (set to 0)")
    SESSION.backup([_QOS], "QosBandwidth")
    SESSION.set_dword(_QOS, "NonBestEffortLimit", 0)


def _remove_disable_qos_reserve(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_QOS, "NonBestEffortLimit")


def _detect_disable_qos_reserve() -> bool:
    return SESSION.read_dword(_QOS, "NonBestEffortLimit") == 0


# ── Increase Receive/Send Socket Buffers ─────────────────────────────────────


def _apply_socket_buffers(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: increase default socket buffer sizes to 256 KB")
    SESSION.backup([_AFD], "SocketBuffers")
    SESSION.set_dword(_AFD, "DefaultReceiveWindow", 262144)
    SESSION.set_dword(_AFD, "DefaultSendWindow", 262144)


def _remove_socket_buffers(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_AFD, "DefaultReceiveWindow")
    SESSION.delete_value(_AFD, "DefaultSendWindow")


def _detect_socket_buffers() -> bool:
    return SESSION.read_dword(_AFD, "DefaultReceiveWindow") == 262144


# ── Disable IPv6 Transition Technologies ─────────────────────────────────────


def _apply_disable_ipv6_transition(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable IPv6 transition tunnels (6to4, ISATAP, Teredo)")
    SESSION.backup([_TCPIP6], "IPv6Transition")
    # Bit 0=Teredo, Bit 1=6to4, Bit 2=ISATAP; 0xFF disables all
    SESSION.set_dword(_TCPIP6, "DisabledComponents", 0xFF)


def _remove_disable_ipv6_transition(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TCPIP6, "DisabledComponents", 0)


def _detect_disable_ipv6_transition() -> bool:
    val = SESSION.read_dword(_TCPIP6, "DisabledComponents")
    return val is not None and val == 0xFF


# ── Set TCP Keep-Alive Interval ──────────────────────────────────────────────


def _apply_tcp_keepalive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: set TCP KeepAliveInterval to 1s, KeepAliveTime to 300s")
    SESSION.backup([_TCPIP], "TcpKeepAlive")
    SESSION.set_dword(_TCPIP, "KeepAliveTime", 300000)     # 5 min (ms)
    SESSION.set_dword(_TCPIP, "KeepAliveInterval", 1000)   # 1 s (ms)


def _remove_tcp_keepalive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TCPIP, "KeepAliveTime")
    SESSION.delete_value(_TCPIP, "KeepAliveInterval")


def _detect_tcp_keepalive() -> bool:
    return SESSION.read_dword(_TCPIP, "KeepAliveTime") == 300000


# ── Disable Network Location Awareness (NLA) NCSI Probes ────────────────────

_NCSI = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\NetworkConnectivityStatusIndicator"
)


def _apply_disable_ncsi(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable NCSI active probing")
    SESSION.backup([_NCSI], "NcsiProbe")
    SESSION.set_dword(_NCSI, "NoActiveProbe", 1)


def _remove_disable_ncsi(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NCSI, "NoActiveProbe")


def _detect_disable_ncsi() -> bool:
    return SESSION.read_dword(_NCSI, "NoActiveProbe") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="dns-force-doh-policy",
        label="Force DNS-over-HTTPS (Policy)",
        category="DNS & Networking Advanced",
        apply_fn=_apply_force_doh_policy,
        remove_fn=_remove_force_doh_policy,
        detect_fn=_detect_force_doh_policy,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DNS_DOH_POLICY],
        description=(
            "Sets DoHPolicy=3 via Group Policy to require DNS-over-HTTPS. "
            "DNS queries that cannot use DoH will fail. "
            "Default: not set. Recommended: 3 (require DoH)."
        ),
        tags=["dns", "doh", "privacy", "encryption", "network"],
    ),
    TweakDef(
        id="dns-disable-negative-cache",
        label="Disable DNS Negative Cache",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_neg_cache,
        remove_fn=_remove_disable_neg_cache,
        detect_fn=_detect_disable_neg_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DNS_CLIENT],
        description=(
            "Sets MaxNegativeCacheTtl=0 so failed DNS lookups are not cached. "
            "Useful when DNS records change frequently. "
            "Default: 5 seconds. Recommended: 0."
        ),
        tags=["dns", "cache", "network", "performance"],
    ),
    TweakDef(
        id="dns-increase-cache-entry-ttl",
        label="Increase DNS Cache Entry TTL",
        category="DNS & Networking Advanced",
        apply_fn=_apply_dns_cache_size,
        remove_fn=_remove_dns_cache_size,
        detect_fn=_detect_dns_cache_size,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DNS_CLIENT],
        description=(
            "Increases the maximum cache entry TTL to 24 hours (86400s) "
            "for faster repeat lookups. Reduces SOA cache to 300s. "
            "Default: 86400. Recommended: 86400."
        ),
        tags=["dns", "cache", "performance", "network"],
    ),
    TweakDef(
        id="dns-disable-mdns",
        label="Disable Multicast DNS (mDNS)",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_mdns,
        remove_fn=_remove_disable_mdns,
        detect_fn=_detect_disable_mdns,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MDNS],
        description=(
            "Disables mDNS responder (EnableMDNS=0). "
            "Reduces network chatter and attack surface on enterprise networks. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["dns", "mdns", "security", "network", "multicast"],
    ),
    TweakDef(
        id="dns-disable-smart-name-resolution",
        label="Disable Smart Multi-Homed Name Resolution",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_smart_name,
        remove_fn=_remove_disable_smart_name,
        detect_fn=_detect_disable_smart_name,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DNS_POLICY],
        description=(
            "Prevents Windows from sending DNS queries to all adapters "
            "simultaneously. Stops DNS leaks on VPN split-tunnel setups. "
            "Default: not configured. Recommended: 1 (disabled)."
        ),
        tags=["dns", "privacy", "vpn", "network", "leak"],
    ),
    TweakDef(
        id="dns-disable-devolution",
        label="Disable DNS Devolution",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_devolution,
        remove_fn=_remove_disable_devolution,
        detect_fn=_detect_disable_devolution,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DNS_POLICY],
        description=(
            "Disables DNS suffix devolution (stripping sub-domain labels). "
            "Prevents unintended DNS queries to parent domains. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["dns", "security", "network", "enterprise"],
    ),
    TweakDef(
        id="dns-disable-lmhosts",
        label="Disable LMHOSTS Lookup",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_lmhosts,
        remove_fn=_remove_disable_lmhosts,
        detect_fn=_detect_disable_lmhosts,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LMHOSTS],
        description=(
            "Disables LMHOSTS file lookup for NetBIOS name resolution. "
            "Reduces attack surface and legacy protocol overhead. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["dns", "netbios", "lmhosts", "security", "network"],
    ),
    TweakDef(
        id="dns-disable-qos-reserve",
        label="Remove QoS Reserved Bandwidth",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_qos_reserve,
        remove_fn=_remove_disable_qos_reserve,
        detect_fn=_detect_disable_qos_reserve,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_QOS],
        description=(
            "Sets QoS non-best-effort bandwidth limit to 0%, reclaiming "
            "the 20% Windows reserves by default. "
            "Default: 20. Recommended: 0."
        ),
        tags=["dns", "qos", "bandwidth", "performance", "network"],
    ),
    TweakDef(
        id="dns-increase-socket-buffers",
        label="Increase Socket Buffer Sizes (256 KB)",
        category="DNS & Networking Advanced",
        apply_fn=_apply_socket_buffers,
        remove_fn=_remove_socket_buffers,
        detect_fn=_detect_socket_buffers,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_AFD],
        description=(
            "Increases default receive/send socket buffers from 64 KB "
            "to 256 KB for better throughput on fast connections. "
            "Default: 65535. Recommended: 262144."
        ),
        tags=["dns", "socket", "tcp", "performance", "network"],
    ),
    TweakDef(
        id="dns-disable-ipv6-transition",
        label="Disable IPv6 Transition Technologies",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_ipv6_transition,
        remove_fn=_remove_disable_ipv6_transition,
        detect_fn=_detect_disable_ipv6_transition,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TCPIP6],
        description=(
            "Disables 6to4, ISATAP, and Teredo IPv6 tunneling adapters. "
            "Reduces attack surface from legacy transition mechanisms. "
            "Default: 0. Recommended: 0xFF (all disabled)."
        ),
        tags=["dns", "ipv6", "security", "network", "tunnel"],
    ),
    TweakDef(
        id="dns-tcp-keepalive-tuning",
        label="Tune TCP Keep-Alive Intervals",
        category="DNS & Networking Advanced",
        apply_fn=_apply_tcp_keepalive,
        remove_fn=_remove_tcp_keepalive,
        detect_fn=_detect_tcp_keepalive,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TCPIP],
        description=(
            "Sets TCP KeepAliveTime to 5 minutes and KeepAliveInterval "
            "to 1 second for faster detection of dead connections. "
            "Default: KeepAliveTime=7200000 (2h). Recommended: 300000 (5m)."
        ),
        tags=["dns", "tcp", "keepalive", "performance", "network"],
    ),
    TweakDef(
        id="dns-disable-ncsi-probes",
        label="Disable NCSI Active Probing",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_ncsi,
        remove_fn=_remove_disable_ncsi,
        detect_fn=_detect_disable_ncsi,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_NCSI],
        description=(
            "Disables Network Connectivity Status Indicator probes to "
            "Microsoft servers. Improves privacy but may affect captive "
            "portal detection. Default: not set. Recommended: 1 (disabled)."
        ),
        tags=["dns", "ncsi", "privacy", "network", "probe"],
    ),
]


# -- Disable LLMNR ---------------------------------------------------------


def _apply_disable_llmnr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable Link-Local Multicast Name Resolution (LLMNR)")
    SESSION.backup([_DNS_POLICY], "DisableLLMNR")
    SESSION.set_dword(_DNS_POLICY, "EnableMulticast", 0)


def _remove_disable_llmnr(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_DNS_POLICY], "DisableLLMNR_Remove")
    SESSION.delete_value(_DNS_POLICY, "EnableMulticast")


def _detect_disable_llmnr() -> bool:
    return SESSION.read_dword(_DNS_POLICY, "EnableMulticast") == 0


# -- Disable NetBIOS over TCP/IP -------------------------------------------


def _apply_disable_netbios(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("DNS: disable NetBIOS over TCP/IP (set NodeType=2 / P-node)")
    SESSION.backup([_NETBT], "DisableNetBIOS")
    SESSION.set_dword(_NETBT, "NodeType", 2)


def _remove_disable_netbios(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NETBT], "DisableNetBIOS_Remove")
    SESSION.delete_value(_NETBT, "NodeType")


def _detect_disable_netbios() -> bool:
    return SESSION.read_dword(_NETBT, "NodeType") == 2


TWEAKS += [
    TweakDef(
        id="dns-disable-llmnr",
        label="Disable LLMNR (Link-Local Multicast Name Resolution)",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_llmnr,
        remove_fn=_remove_disable_llmnr,
        detect_fn=_detect_disable_llmnr,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DNS_POLICY],
        description=(
            "Disables LLMNR to prevent local name-resolution poisoning attacks. "
            "LLMNR responds to multicast queries on the local subnet and can be "
            "exploited for credential relay. Default: enabled. Recommended: disabled."
        ),
        tags=["dns", "llmnr", "security", "network", "hardening"],
    ),
    TweakDef(
        id="dns-disable-netbios",
        label="Disable NetBIOS over TCP/IP",
        category="DNS & Networking Advanced",
        apply_fn=_apply_disable_netbios,
        remove_fn=_remove_disable_netbios,
        detect_fn=_detect_disable_netbios,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_NETBT],
        description=(
            "Sets NetBT NodeType to 2 (P-node, point-to-point only) to disable "
            "broadcast-based NetBIOS name resolution. Mitigates NBNS spoofing. "
            "Default: 0 (B-node broadcast). Recommended: 2 (P-node)."
        ),
        tags=["dns", "netbios", "security", "network", "hardening"],
    ),
]
