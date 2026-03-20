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
            Description =
                "Sets DoHPolicy=3 via Group Policy to require DNS-over-HTTPS. DNS queries that cannot use DoH will fail. Default: not set. Recommended: 3 (require DoH).",
            Tags = ["dns", "doh", "privacy", "encryption", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy", 3)],
        },
        new TweakDef
        {
            Id = "dns-disable-mdns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables mDNS responder (EnableMDNS=0). Reduces network chatter and attack surface on enterprise networks. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "mdns", "security", "network", "multicast"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-name-resolution",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from sending DNS queries to all adapters simultaneously. Stops DNS leaks on VPN split-tunnel setups. Default: not configured. Recommended: 1 (disabled).",
            Tags = ["dns", "privacy", "vpn", "network", "leak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-devolution",
            Label = "Disable DNS Devolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables DNS suffix devolution (stripping sub-domain labels). Prevents unintended DNS queries to parent domains. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "security", "network", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-lmhosts",
            Label = "Disable LMHOSTS Lookup",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables LMHOSTS file lookup for NetBIOS name resolution. Reduces attack surface and legacy protocol overhead. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "netbios", "lmhosts", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-qos-reserve",
            Label = "Remove QoS Reserved Bandwidth",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets QoS non-best-effort bandwidth limit to 0%, reclaiming the 20% Windows reserves by default. Default: 20. Recommended: 0.",
            Tags = ["dns", "qos", "bandwidth", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
        },
        new TweakDef
        {
            Id = "dns-increase-socket-buffers",
            Label = "Increase Socket Buffer Sizes (256 KB)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases default receive/send socket buffers from 64 KB to 256 KB for better throughput on fast connections. Default: 65535. Recommended: 262144.",
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
            Description =
                "Sets TCP KeepAliveTime to 5 minutes and KeepAliveInterval to 1 second for faster detection of dead connections. Default: KeepAliveTime=7200000 (2h). Recommended: 300000 (5m).",
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
            Description =
                "Disables Network Connectivity Status Indicator probes to Microsoft servers. Improves privacy but may affect captive portal detection. Default: not set. Recommended: 1 (disabled).",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
        },
        new TweakDef
        {
            Id = "dns-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables LLMNR to prevent local name-resolution poisoning attacks. LLMNR responds to multicast queries on the local subnet and can be exploited for credential relay. Default: enabled. Recommended: disabled.",
            Tags = ["dns", "llmnr", "security", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets NetBT NodeType to 2 (P-node, point-to-point only) to disable broadcast-based NetBIOS name resolution. Mitigates NBNS spoofing. Default: 0 (B-node broadcast). Recommended: 2 (P-node).",
            Tags = ["dns", "netbios", "security", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-disable-wpad",
            Label = "Disable WPAD Auto-Discovery",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Web Proxy Auto-Discovery (WPAD) protocol to prevent automatic proxy configuration and WPAD poisoning attacks. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["dns", "wpad", "proxy", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad")],
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
            Description =
                "Increases the DNS resolver cache maximum entry count from default to 512. Reduces repeat DNS lookups. Default: system-managed.",
            Tags = ["dns", "cache", "size", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400),
            ],
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
            Description =
                "Disables NetBIOS over TCP/IP. Eliminates NBT broadcasts and reduces attack surface. May break legacy apps. Default: enabled.",
            Tags = ["dns", "netbios", "tcp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-disable-ipv6-transition",
            Label = "Disable IPv6 Transition Technologies",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables IPv6 transition technologies (6to4, Teredo, ISATAP). Reduces attack surface and simplifies networking. Default: enabled.",
            Tags = ["dns", "ipv6", "transition", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-negative-cache",
            Label = "Disable DNS Negative Cache",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DNS negative response caching. Failed lookups are retried immediately instead of being cached. Useful for development. Default: cached.",
            Tags = ["dns", "negative", "cache", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 0)],
        },
        new TweakDef
        {
            Id = "dns-increase-cache-entry-ttl",
            Label = "Increase DNS Cache TTL",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the DNS cache entry TTL to 86400 seconds (24 hours). Reduces DNS lookup frequency. Improves browsing speed. Default: server-defined.",
            Tags = ["dns", "cache", "ttl", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
        },
        new TweakDef
        {
            Id = "dns-reduce-query-timeout",
            Label = "Reduce DNS Query Timeout",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces DNS query timeout to 2 seconds. Faster fallback to alternate DNS servers on poor connectivity. Default: varies.",
            Tags = ["dns", "query", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "QueryRetryInterval", 2000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "QueryRetryInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "QueryRetryInterval", 2000)],
        },
        new TweakDef
        {
            Id = "dns-disable-multicast-name-resolution",
            Label = "Disable Multicast Name Resolution (LLMNR)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Link-Local Multicast Name Resolution (LLMNR). Prevents LLMNR poisoning attacks. Default: enabled.",
            Tags = ["dns", "llmnr", "multicast", "security", "poisoning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-enable-doh-require",
            Label = "Require DNS over HTTPS (DoH)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            Description = "Requires all DNS queries to use DNS over HTTPS. Falls back to no resolution if DoH is unavailable. Default: disabled.",
            Tags = ["dns", "doh", "https", "privacy", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 3)],
        },
        new TweakDef
        {
            Id = "dns-disable-name-devolution-policy",
            Label = "Disable DNS Devolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DNS name devolution (searching parent domains). Prevents unintended DNS queries to parent zones. Default: enabled.",
            Tags = ["dns", "devolution", "security", "domain"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NetBIOS name resolution over TCP/IP. Mitigates NBT-NS poisoning attacks. Default: enabled.",
            Tags = ["dns", "netbios", "tcp", "security", "poisoning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-set-negative-cache-ttl-0",
            Label = "Disable Negative DNS Cache",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            Description =
                "Sets negative DNS cache TTL to 0, so failed lookups are not cached. Useful for dynamic environments. Default: 900 seconds.",
            Tags = ["dns", "cache", "negative", "ttl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-multihomed-policy",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            Description =
                "Disables Smart Multi-Homed Name Resolution which sends DNS queries to all adapters simultaneously. Security risk on VPN. Default: enabled.",
            Tags = ["dns", "smart", "multi-homed", "vpn", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-max-cache-ttl-86400",
            Label = "Limit DNS Cache TTL to 24 Hours",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            Description = "Caps the DNS cache entry TTL at 86400 seconds (24 hours). Prevents stale DNS entries. Default: unlimited.",
            Tags = ["dns", "cache", "ttl", "staleness"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
        },
        new TweakDef
        {
            Id = "dns-disable-parallel-queries",
            Label = "Disable Parallel DNS Queries",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            Description =
                "Prevents DNS resolver from sending parallel queries across all network adapters. Reduces DNS leakage on VPN. Default: enabled.",
            Tags = ["dns", "parallel", "vpn", "leak", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableParallelAandAAAA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableParallelAandAAAA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableParallelAandAAAA", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-dns-cache-locking",
            Label = "Enable DNS Cache Locking at 100%",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Locks cached DNS records for 100% of their TTL, preventing cache poisoning via premature overwrites. Default: 100.",
            Tags = ["dns", "cache", "locking", "poisoning", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtlEntryLockingPercent", 100),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtlEntryLockingPercent"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtlEntryLockingPercent", 100),
            ],
        },
        new TweakDef
        {
            Id = "dns-enable-dnssec-validation",
            Label = "Enable DNSSEC Validation",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNSSEC validation in the Windows DNS client. Verifies DNS response authenticity. Default: disabled.",
            Tags = ["dns", "dnssec", "validation", "security", "integrity"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableDnsSec", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableDnsSec")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableDnsSec", 1)],
        },
        // ── Sprint 18 — 10 new DNS & Networking Advanced tweaks ────────────
        new TweakDef
        {
            Id = "dns-disable-llmnr-via-policy",
            Label = "Disable LLMNR via Group Policy",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Link-Local Multicast Name Resolution (LLMNR) to prevent name resolution poisoning attacks. Default: enabled.",
            Tags = ["dns", "llmnr", "security", "poisoning", "multicast"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-set-dns-priority-v4",
            Label = "Prioritise IPv4 DNS over IPv6",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures prefix policy to prefer IPv4 name resolution over IPv6, reducing latency on networks without native v6. Default: IPv6 preferred.",
            Tags = ["dns", "ipv4", "ipv6", "priority", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 32)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 32)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-protocol-reorder",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from sending DNS queries to all adapters simultaneously. Reduces DNS leaks on VPN connections. Default: enabled.",
            Tags = ["dns", "multi-homed", "vpn", "leak", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-dns-filtering-platform",
            Label = "Enable DNS Client Diagnostic Logging",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables analytic logging in the DNS client for troubleshooting resolution issues. Default: disabled.",
            Tags = ["dns", "logging", "diagnostics", "troubleshooting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableLogging", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-wins-resolution",
            Label = "Disable WINS Name Resolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables legacy WINS/NBNS name resolution in the TCP/IP stack. Reduces NetBIOS attack surface. Default: enabled.",
            Tags = ["dns", "wins", "netbios", "legacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
        },
        new TweakDef
        {
            Id = "dns-set-max-negative-cache-ttl",
            Label = "Limit Negative DNS Cache TTL to 5 Seconds",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits how long failed DNS lookups are cached. Faster recovery after DNS changes or outages. Default: 900s (15 min).",
            Tags = ["dns", "cache", "negative-cache", "ttl", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
        },
        new TweakDef
        {
            Id = "dns-disable-devolution-fallback",
            Label = "Disable DNS Devolution",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows from walking up the domain suffix hierarchy when resolving names. Reduces information leakage. Default: enabled.",
            Tags = ["dns", "devolution", "suffix", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-force-fqdn-only",
            Label = "Require Fully Qualified Domain Names",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Forces the DNS client to only resolve fully qualified domain names. Prevents unqualified name lookups. Default: not required.",
            Tags = ["dns", "fqdn", "security", "resolution", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "QueryAdapterName", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "QueryAdapterName")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "QueryAdapterName", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-query-logging",
            Label = "Enable DNS Query ETW Logging",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables ETW-based DNS query logging for security monitoring and forensics. Default: disabled.",
            Tags = ["dns", "logging", "etw", "monitoring", "forensics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableDnsQueryLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableDnsQueryLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableDnsQueryLogging", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-parallel-adapter-queries",
            Label = "Disable Parallel DNS Queries Across Adapters",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents sending the same DNS query across multiple network adapters simultaneously. Reduces traffic and VPN DNS leaks. Default: enabled.",
            Tags = ["dns", "parallel", "adapters", "vpn", "leak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableParallelAandAAAA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableParallelAandAAAA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableParallelAandAAAA", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-socket-pool-size",
            Label = "Increase DNS Socket Pool Size",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the DNS resolver socket pool to 2 500 sockets (CERT-recommended). A larger pool randomises source ports, mitigating DNS cache-poisoning attacks.",
            Tags = ["dns", "security", "cache-poisoning", "socket", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "SocketPoolSize", 2500)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "SocketPoolSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "SocketPoolSize", 2500)],
        },
        new TweakDef
        {
            Id = "dns-disable-ptr-registration",
            Label = "Disable Reverse-Address (PTR) Registration",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the DNS client from dynamically registering PTR (reverse lookup) records. Reduces DNS noise and avoids exposing the hostname via reverse lookups.",
            Tags = ["dns", "ptr", "privacy", "registration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableReverseAddressRegistrations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableReverseAddressRegistrations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableReverseAddressRegistrations", 1)],
        },
        new TweakDef
        {
            Id = "dns-gpo-disable-dynamic-registration",
            Label = "Disable DNS Dynamic Update Registration (Policy)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Applies the DNSClient GPO policy to disable dynamic DNS registration. The workstation will not automatically update its A or AAAA records in DNS.",
            Tags = ["dns", "dynamic-update", "gpo", "privacy", "registration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableDynamicUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableDynamicUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableDynamicUpdate", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-max-udp-datagram",
            Label = "Increase Max DNS UDP Datagram Size",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Raises MaximumUdpDatagramSize to 4 096 bytes. Required to receive full DNSSEC-signed responses over UDP without falling back to TCP.",
            Tags = ["dns", "dnssec", "udp", "performance", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumUdpDatagramSize", 4096)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumUdpDatagramSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumUdpDatagramSize", 4096)],
        },
        new TweakDef
        {
            Id = "dns-set-server-priority-limit",
            Label = "Reduce DNS Server Priority Timeout",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets ServerPriorityTimeLimit to 400 ms. The resolver will try the next server in its list sooner when the current preferred server is slow.",
            Tags = ["dns", "timeout", "performance", "server-priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "ServerPriorityTimeLimit", 400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "ServerPriorityTimeLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "ServerPriorityTimeLimit", 400)],
        },
        new TweakDef
        {
            Id = "dns-set-cache-hash-table-size",
            Label = "Increase DNS Cache Hash Table Size",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets CacheHashTableSize to 4 096 buckets. A larger hash table reduces collision chains in the DNS cache, improving cache lookup speed under heavy load.",
            Tags = ["dns", "cache", "performance", "hash-table"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "CacheHashTableSize", 4096)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "CacheHashTableSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "CacheHashTableSize", 4096)],
        },
        new TweakDef
        {
            Id = "dns-set-max-hostname-ttl",
            Label = "Cap Hostname Cache TTL at 1 Hour",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MaxHostnameTtl to 3 600 seconds (1 hour). Prevents the DNS cache from holding stale hostname entries for excessively long periods.",
            Tags = ["dns", "cache", "ttl", "hostname"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxHostnameTtl", 3600)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxHostnameTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxHostnameTtl", 3600)],
        },
        new TweakDef
        {
            Id = "dns-set-address-query-timeout",
            Label = "Cap DNS Address Query Timeout",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MaxAddressQueryTimeout to 30 000 ms. Prevents the resolver from waiting indefinitely for an address record response.",
            Tags = ["dns", "timeout", "query", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxAddressQueryTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxAddressQueryTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxAddressQueryTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "dns-disable-adapter-name-reg",
            Label = "Disable Per-Adapter Name DNS Registration",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets RegisterAdapterName=0 to stop the DNS client from registering individual adapter names. Reduces DNS record clutter on multi-homed hosts.",
            Tags = ["dns", "registration", "adapter", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "RegisterAdapterName", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "RegisterAdapterName")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "RegisterAdapterName", 0)],
        },
        new TweakDef
        {
            Id = "dns-gpo-disable-ptr-update",
            Label = "Disable PTR Record Registration (Policy)",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Applies the DNSClient GPO policy to disable reverse-address (PTR) record registration. Provides a policy-enforced version of the Dnscache parameter equivalent.",
            Tags = ["dns", "ptr", "gpo", "privacy", "registration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableReverseAddressRegistrations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableReverseAddressRegistrations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableReverseAddressRegistrations", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-update-security-level",
            Label = "Require Secure DNS Dynamic Updates",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets UpdateSecurityLevel=256 so the DNS client only attempts GSS-API authenticated (secure) dynamic updates. Prevents unauthenticated update attempts on Active Directory-integrated zones.",
            Tags = ["dns", "security", "update", "gss", "ad"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "UpdateSecurityLevel", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "UpdateSecurityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "UpdateSecurityLevel", 256)],
        },
        new TweakDef
        {
            Id = "dns-set-max-dynamic-backoff",
            Label = "Cap DNS Dynamic Update Backoff Interval",
            Category = "DNS & Networking Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MaximumDynamicBackoff to 20 000 ms. Stops the DNS client from waiting more than 20 s between retry attempts for failed dynamic updates.",
            Tags = ["dns", "dynamic-update", "backoff", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumDynamicBackoff", 20000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumDynamicBackoff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumDynamicBackoff", 20000)],
        },
    ];
}
