// RegiLattice.Core — Tweaks/WinsNameResolutionPolicy.cs
// WINS / Name Resolution Policy — Sprint 544.
// Configures Group Policy for Windows Internet Name Service (WINS),
// NetBIOS name resolution, DNS suffix search lists, and the Name
// Resolution Policy Table (NRPT) for selective DNS routing.
// Category: "WINS Name Resolution Policy" | Slug: wins
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinsNameResolutionPolicy
{
    private const string DnsKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

    private const string NetbtKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wins-disable-netbios-over-tcp",
                Label = "WINS: Disable NetBIOS over TCP/IP (NBT)",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets NetbiosOptions=2 (disable) in NetBT parameters. Disables NetBIOS over TCP/IP name resolution, which removes the NetBIOS SS port (TCP 139) and datagram port (UDP 138) from all network interfaces. NetBIOS is a pre-DNS name resolution protocol that exposes the machine's NetBIOS name to the local LAN broadcast domain, enabling LAN-wide workgroup enumeration and NTLM relay attacks. Modern networks use DNS-only name resolution; NetBIOS is a legacy protocol and an attack surface that should be disabled in all hardened environments.",
                Tags = ["wins", "netbios", "nbt", "legacy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Disables NetBIOS name resolution. Legacy applications, network printers, and file shares discovered via NetBIOS broadcast (e.g., accessing \\\\COMPUTERNAME without DNS entry) will break.",
                ApplyOps = [RegOp.SetDword(NetbtKey, "NetbiosOptions", 2)],
                RemoveOps = [RegOp.DeleteValue(NetbtKey, "NetbiosOptions")],
                DetectOps = [RegOp.CheckDword(NetbtKey, "NetbiosOptions", 2)],
            },
            new TweakDef
            {
                Id = "wins-disable-node-type-broadcast",
                Label = "WINS: Disable NetBIOS Broadcast Node Type (Set to H-Node)",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets NodeType=8 (H-Node: WINS then broadcast) in NetBT parameters. Changes the NetBIOS name resolution order from B-Node (broadcast only) to H-Node (WINS server first, broadcast fallback). B-Node broadcasts NetBIOS name queries to the entire LAN broadcast domain, generating traffic across all hosts and leaking internal machine names. H-Node uses the configured WINS server first (unicast), falling back to broadcast only when WINS is unavailable. Even in environments without WINS servers, H-Node reduces unnecessary broadcast traffic.",
                Tags = ["wins", "netbios", "node-type", "broadcast", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sets H-Node for NetBIOS. If WINS servers are not configured, resolution falls back to broadcast (which already happens with default B-Node). No functional change for most modern environments.",
                ApplyOps = [RegOp.SetDword(NetbtKey, "NodeType", 8)],
                RemoveOps = [RegOp.DeleteValue(NetbtKey, "NodeType")],
                DetectOps = [RegOp.CheckDword(NetbtKey, "NodeType", 8)],
            },
            new TweakDef
            {
                Id = "wins-enable-devolution",
                Label = "WINS: Enable DNS Name Devolution for Short Name Resolution",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets UseDomainNameDevolution=1 in DNS client policy. Enables DNS name devolution which allows users to resolve hostnames using only the first component (e.g., 'server1' resolves to 'server1.corp.contoso.com') by appending the primary DNS suffix and its parent suffixes. Without devolution, users must type the fully qualified domain name. Devolution improves usability in domain environments while using DNS instead of NetBIOS broadcast for short-name resolution.",
                Tags = ["wins", "dns", "devolution", "short-name", "usability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enables DNS suffix devolution. On machines joined to deeply nested AD sub-domains, devolution can result in unexpected name resolution collisions. Test in complex multi-domain forests.",
                ApplyOps = [RegOp.SetDword(DnsKey, "UseDomainNameDevolution", 1)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "UseDomainNameDevolution")],
                DetectOps = [RegOp.CheckDword(DnsKey, "UseDomainNameDevolution", 1)],
            },
            new TweakDef
            {
                Id = "wins-disable-multicast-dns",
                Label = "WINS: Disable mDNS (Multicast DNS) for Enterprise Networks",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets EnableMulticast=0 in DNS client policy. Disables Multicast DNS (mDNS / Bonjour / .local resolution) which sends name queries to the multicast address 224.0.0.251. mDNS is designed for consumer peer-to-peer networks (Apple Bonjour, Google Cast discovery) and is inappropriate in enterprise environments where DNS servers manage all name resolution. mDNS multicast traffic consumes LAN bandwidth and multicast processing overhead on all hosts in the broadcast domain. Disabling it reduces unnecessary multicast traffic.",
                Tags = ["wins", "mdns", "multicast", "bonjour", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables mDNS resolution. Applications using .local resolution (Bonjour printers, Apple devices, Chromecasts) depend on mDNS. Only disable on managed enterprise machines where DNS handles all name resolution.",
                ApplyOps = [RegOp.SetDword(DnsKey, "EnableMulticast", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "EnableMulticast")],
                DetectOps = [RegOp.CheckDword(DnsKey, "EnableMulticast", 0)],
            },
            new TweakDef
            {
                Id = "wins-set-negative-ttl-cache",
                Label = "WINS: Reduce Negative DNS Cache TTL to 5 Seconds",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets NegativeCacheTime=5 in DNS client policy. Reduces the time Windows caches negative DNS results (NXDOMAIN responses — 'hostname not found') to 5 seconds. The default 5-minute negative TTL causes incorrectly cached NXDOMAIN entries to persist for 5 minutes, blocking access to newly provisioned hosts for that duration. This is a common issue in rapidly provisioned cloud environments where new VMs generate DNS entries that may temporarily return NXDOMAIN before replication completes.",
                Tags = ["wins", "dns", "negative-cache", "nxdomain", "ttl"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Shorter negative TTL increases DNS query load slightly as failed lookups are re-tried sooner. Benefit is faster recovery from transient DNS unavailability.",
                ApplyOps = [RegOp.SetDword(DnsKey, "NegativeCacheTime", 5)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "NegativeCacheTime")],
                DetectOps = [RegOp.CheckDword(DnsKey, "NegativeCacheTime", 5)],
            },
            new TweakDef
            {
                Id = "wins-disable-llmnr",
                Label = "WINS: Disable LLMNR (Link-Local Multicast Name Resolution)",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets EnableMulticast=0 and QueryPolicy=1 in DNS client (LLMNR) policy. Disables Link-Local Multicast Name Resolution (LLMNR) which is exploited in Responder/NBNSPoison MITM attacks. LLMNR broadcasts name queries when DNS fails; an attacker running Responder can answer any LLMNR query with a fraudulent response, redirecting authentication traffic to a rogue host and capturing NTLMv2 hashes for offline cracking. Disabling LLMNR is a top-10 Active Directory hardening recommendation in every security benchmark.",
                Tags = ["wins", "llmnr", "responder", "ntlm-relay", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Breaking change if any application depends on LLMNR for name resolution. On properly configured networks with working DNS, LLMNR is never used.",
                ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient",
                            "EnableMulticast",
                            0
                        ),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient",
                            "EnableMulticast"
                        ),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient",
                            "EnableMulticast",
                            0
                        ),
                    ],
            },
            new TweakDef
            {
                Id = "wins-set-dns-cache-timeout",
                Label = "WINS: Set DNS Cache Entry Maximum TTL to 1 Hour",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets MaxCacheTtl=3600 in DNS client policy. Caps the maximum time a successful DNS resolution result is cached in the Windows DNS resolver cache to 1 hour, regardless of the record's TTL. Longer TTL caches can cause stale A record lookups after IP address changes (failover scenarios, DR tests, cloud load balancer IP rotation). Capping at 1 hour ensures stale records don't persist beyond 1 hour in event of planned or unplanned address changes.",
                Tags = ["wins", "dns", "cache", "ttl", "failover"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "DNS records are re-resolved after at most 1 hour. This slightly increases DNS query load for long-TTL records but ensures nearly real-time failover detection for DNS-based HA.",
                ApplyOps = [RegOp.SetDword(DnsKey, "MaxCacheTtl", 3600)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "MaxCacheTtl")],
                DetectOps = [RegOp.CheckDword(DnsKey, "MaxCacheTtl", 3600)],
            },
            new TweakDef
            {
                Id = "wins-enable-register-adapters",
                Label = "WINS: Enable DNS Registration for All Network Adapters",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets RegisterAdapterName=1 in DNS client policy. Ensures that all network adapters (including VPN adapters, virtual NICs) register their IP addresses against the primary DNS server. By default, Windows may suppress DNS registration for secondary adapters or adapters with low metric values. Enabling registration for all adapters ensures that VPN tunnel IPs, management interfaces, and secondary NICs are resolvable by name from the corporate DNS server, which is essential for IT remote management tools.",
                Tags = ["wins", "dns", "registration", "adapter", "management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "All adapter IPs are registered in DNS. This may create multiple A records for a single hostname if multiple adapters have network connectivity. Configure DNS scavenging appropriately.",
                ApplyOps = [RegOp.SetDword(DnsKey, "RegisterAdapterName", 1)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "RegisterAdapterName")],
                DetectOps = [RegOp.CheckDword(DnsKey, "RegisterAdapterName", 1)],
            },
            new TweakDef
            {
                Id = "wins-disable-dns-compression",
                Label = "WINS: Disable DNS Query Payload Compression (Debug Mode)",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets DisableCompression=1 in DNS client policy. Disables DNS message compression in outbound DNS queries. DNS compression reduces packet size but, in rare cases, can cause parsing errors with non-RFC-compliant DNS resolvers that implement compression algorithms incorrectly (found in some embedded or appliance DNS proxies). Disabling compression is a diagnostic/debug setting: enable it when troubleshooting DNS query failures with appliance-based DNS servers that behave unexpectedly.",
                Tags = ["wins", "dns", "compression", "debug", "diagnostic"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "DNS queries are sent uncompressed (slightly larger packets). Only use as a diagnostic setting for troubleshooting specific DNS appliance compatibility issues.",
                ApplyOps = [RegOp.SetDword(DnsKey, "DisableCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableCompression")],
                DetectOps = [RegOp.CheckDword(DnsKey, "DisableCompression", 1)],
            },
            new TweakDef
            {
                Id = "wins-enable-smart-multi-homed",
                Label = "WINS: Enable Smart Multi-Homed DNS Registration",
                Category = "WINS Name Resolution Policy",
                Description =
                    "Sets EnableAutoConfig=1 in DNS client policy. Enables smart multi-homed DNS registration: when a machine has multiple network interfaces, Windows will register only the interface with the best default gateway route rather than registering all adapter IPs. This prevents DNS pollution from VPN temporary IPs, APIPA addresses, and link-local IPv6 addresses appearing in the corporate DNS zone. Smart registration ensures clients resolve to the primary routable IP address of a machine.",
                Tags = ["wins", "dns", "multi-homed", "registration", "smart"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Only the primary/best-route adapter IP is registered in DNS. Non-primary adapter IPs (VPN adapters, secondary NICs) are not registered when this is enabled.",
                ApplyOps = [RegOp.SetDword(DnsKey, "EnableAutoConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(DnsKey, "EnableAutoConfig")],
                DetectOps = [RegOp.CheckDword(DnsKey, "EnableAutoConfig", 1)],
            },
        ];
}
