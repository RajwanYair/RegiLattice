namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DnsNetworking
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dns-force-doh-policy",
            Label = "Force DNS-over-HTTPS (Policy)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets DoHPolicy=3 via Group Policy to require DNS-over-HTTPS. DNS queries that cannot use DoH will fail. Default: not set. Recommended: 3 (require DoH).",
            Tags = ["dns", "doh", "privacy", "encryption", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy", 3)],
        },
        new TweakDef
        {
            Id = "dns-disable-mdns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables mDNS responder (EnableMDNS=0). Reduces network chatter and attack surface on enterprise networks. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "mdns", "security", "network", "multicast"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-name-resolution",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from sending DNS queries to all adapters simultaneously. Stops DNS leaks on VPN split-tunnel setups. Default: not configured. Recommended: 1 (disabled).",
            Tags = ["dns", "privacy", "vpn", "network", "leak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-devolution",
            Label = "Disable DNS Devolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables DNS suffix devolution (stripping sub-domain labels). Prevents unintended DNS queries to parent domains. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "security", "network", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-lmhosts",
            Label = "Disable LMHOSTS Lookup",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LMHOSTS file lookup for NetBIOS name resolution. Reduces attack surface and legacy protocol overhead. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "netbios", "lmhosts", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-qos-reserve",
            Label = "Remove QoS Reserved Bandwidth",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets QoS non-best-effort bandwidth limit to 0%, reclaiming the 20% Windows reserves by default. Default: 20. Recommended: 0.",
            Tags = ["dns", "qos", "bandwidth", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
        },
        new TweakDef
        {
            Id = "dns-increase-socket-buffers",
            Label = "Increase Socket Buffer Sizes (256 KB)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases default receive/send socket buffers from 64 KB to 256 KB for better throughput on fast connections. Default: 65535. Recommended: 262144.",
            Tags = ["dns", "socket", "tcp", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 262144),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow", 262144),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 262144)],
        },
        new TweakDef
        {
            Id = "dns-tcp-keepalive-tuning",
            Label = "Tune TCP Keep-Alive Intervals",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets TCP KeepAliveTime to 5 minutes and KeepAliveInterval to 1 second for faster detection of dead connections. Default: KeepAliveTime=7200000 (2h). Recommended: 300000 (5m).",
            Tags = ["dns", "tcp", "keepalive", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 300000),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveInterval", 1000),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveInterval"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 300000)],
        },
        new TweakDef
        {
            Id = "dns-disable-ncsi-probes",
            Label = "Disable NCSI Active Probing",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Network Connectivity Status Indicator probes to Microsoft servers. Improves privacy but may affect captive portal detection. Default: not set. Recommended: 1 (disabled).",
            Tags = ["dns", "ncsi", "privacy", "network", "probe"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables LLMNR to prevent local name-resolution poisoning attacks. LLMNR responds to multicast queries on the local subnet and can be exploited for credential relay. Default: enabled. Recommended: disabled.",
            Tags = ["dns", "llmnr", "security", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets NetBT NodeType to 2 (P-node, point-to-point only) to disable broadcast-based NetBIOS name resolution. Mitigates NBNS spoofing. Default: 0 (B-node broadcast). Recommended: 2 (P-node).",
            Tags = ["dns", "netbios", "security", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-disable-wpad",
            Label = "Disable WPAD Auto-Discovery",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Web Proxy Auto-Discovery (WPAD) protocol to prevent automatic proxy configuration and WPAD poisoning attacks. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["dns", "wpad", "proxy", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-doh",
            Label = "Enable DNS-over-HTTPS",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNS-over-HTTPS (DoH) for encrypted DNS resolution. Requires a DoH-capable DNS server. Default: plaintext DNS.",
            Tags = ["dns", "doh", "encrypted", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
        },
        new TweakDef
        {
            Id = "dns-set-cache-size-512",
            Label = "Increase DNS Cache Size",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the DNS resolver cache maximum entry count from default to 512. Reduces repeat DNS lookups. Default: system-managed.",
            Tags = ["dns", "cache", "size", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
        },
        new TweakDef
        {
            Id = "dns-disable-multicast-dns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables mDNS (Bonjour-style) name resolution. Reduces network chatter on enterprise networks. Default: enabled.",
            Tags = ["dns", "mdns", "multicast", "bonjour"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "dns-set-negative-cache-ttl",
            Label = "Reduce Negative DNS Cache TTL",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the time negative DNS responses (NXDOMAIN) are cached to 5 seconds. Useful for DNS failover. Default: 5 minutes.",
            Tags = ["dns", "cache", "negative", "ttl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios-over-tcp",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NetBIOS over TCP/IP. Eliminates NBT broadcasts and reduces attack surface. May break legacy apps. Default: enabled.",
            Tags = ["dns", "netbios", "tcp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
    ];
}
