// RegiLattice.Core — Tweaks/DnsSecurePolicy.cs
// Sprint 300: DNS Secure Policy tweaks (10 tweaks)
// Category: "DNS Secure Policy" | Slug: dnssec
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DNSClient

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DnsSecurePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DNSClient";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dnssec-disable-multicast",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Multicast DNS enables DNS resolution on local network segments without requiring a DNS server, using the 224.0.0.251 multicast address. Disabling mDNS prevents Windows from resolving hostnames through the local-link multicast mechanism on .local domains. mDNS name resolution is vulnerable to spoofing attacks on local networks where any host can respond to multicast queries. Enterprise name resolution should be handled exclusively through managed internal DNS servers with validated zones. mDNS spoofing has been used in LANs to redirect clients to malicious hosts imitating legitimate services. Disabling mDNS forces all DNS resolution through unicast queries to configured DNS servers where responses can be validated.",
            Tags = ["dns", "multicast", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-netbios-name-res",
            Label = "Disable NetBIOS Name Resolution Fallback",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "NetBIOS name resolution is a legacy protocol that resolves short hostnames through broadcast packets when DNS resolution fails. Disabling NetBIOS name resolution fallback prevents Windows from falling back to NetBIOS broadcasts when DNS queries do not return results. NetBIOS broadcasts can be intercepted and spoofed using tools like Responder to capture authentication credentials. NBNS spoofing attacks are among the most common initial access techniques in enterprise environments. Modern Windows environments with properly configured DNS zones do not require NetBIOS for name resolution. Disabling NetBIOS name resolution fallback requires DNS to be properly configured to handle all required hostname lookups.",
            Tags = ["dns", "netbios", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-global-query-block",
            Label = "Enable DNS Global Query Block List",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The DNS global query block list prevents DNS resolution of specific hostnames that are commonly abused by attackers to bypass security controls. Enabling the global query block list causes the DNS client to refuse resolution of blocked hostnames. Certain hostnames like WPAD and ISATAP have been abused to force DNS poisoning and proxy autoconfiguration attacks. WPAD name collisions have been used to intercept enterprise HTTP traffic through auto-discovered malicious proxy configurations. The global query block list provides a defense against auto-configuration attacks using predictable hostname patterns. Enabling this list on DNS clients provides complementary protection alongside DNS server-side blocking of these names.",
            Tags = ["dns", "security", "block-list", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableGlobalQueryBlockList", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableGlobalQueryBlockList")],
            DetectOps = [RegOp.CheckDword(Key, "EnableGlobalQueryBlockList", 1)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-dns-devolution",
            Label = "Disable DNS Devolution",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "DNS devolution allows Windows to automatically append shorter suffixes derived from the primary DNS suffix when resolving names that fail to resolve. Disabling DNS devolution prevents automatic suffix shortening which can cause names to inadvertently resolve to external DNS servers. Devolution can cause internal hostnames to resolve to external internet hosts if the devolved name exists in public DNS. This creates a risk of internal application traffic being inadvertently directed to external and potentially attacker-controlled hosts. Enterprise name resolution should be explicit and predictable without automatic suffix manipulation. Disabling devolution ensures that DNS resolution follows only the explicitly configured search suffix list.",
            Tags = ["dns", "devolution", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DevolutionLevel", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DevolutionLevel")],
            DetectOps = [RegOp.CheckDword(Key, "DevolutionLevel", 0)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-cache-poisoning",
            Label = "Enable DNS Cache Poisoning Protection",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "DNS cache poisoning attacks inject fraudulent DNS records into the client resolver cache to redirect traffic to attacker-controlled servers. Enabling DNS cache poisoning protection activates additional validation of DNS responses before they are stored in the local cache. Socket port randomization and transaction ID randomization are key mitigations against DNS poisoning attacks. Hardened DNS client settings validate response characteristics and reject answers that do not match request parameters. Enterprise DNS clients should be hardened against cache poisoning especially when using internet-facing DNS resolvers. Cache poisoning protection adds minimal overhead to DNS resolution while substantially improving security against name hijacking attacks.",
            Tags = ["dns", "cache-poisoning", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableCachePoisoningProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableCachePoisoningProtection")],
            DetectOps = [RegOp.CheckDword(Key, "EnableCachePoisoningProtection", 1)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-resolution-failure-cache",
            Label = "Disable DNS Resolution Failure Cache",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The DNS negative cache stores the results of failed name resolution queries to prevent repeated DNS lookups for non-existent or temporarily unavailable names. Disabling the negative cache allows DNS lookups to retry on every query rather than returning cached NXDOMAIN from previous lookups. In enterprise environments with internally managed DNS zones, negative caching can prevent timely resolution of newly provisioned internal services. The negative cache can cause legitimate network connectivity issues when DNS changes propagate after server failures. Disabling negative caching is particularly useful in dynamic environments with frequent service provisioning and decommissioning. The trade-off is slightly increased DNS query volume from repeated lookups for absent names.",
            Tags = ["dns", "cache", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNegativeCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNegativeCache")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNegativeCache", 1)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-register-ptr",
            Label = "Disable DNS PTR Record Registration",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows DNS clients automatically register PTR reverse-lookup records in DNS to allow hostname resolution from IP addresses. Disabling PTR record registration prevents workstations from adding reverse-lookup entries to the DNS zone. Unauthorized or stale PTR records in reverse-lookup zones can cause security validation failures and introduce naming inconsistencies. Reverse DNS zones in enterprise environments are managed through infrastructure provisioning and should not be dynamically modified by clients. Dynamic PTR registration also discloses the hostname of enterprise endpoints to anyone with DNS query access to the reverse zone. Disabling PTR registration reduces the attack surface from reverse-lookup zone manipulation on managed endpoints.",
            Tags = ["dns", "ptr", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RegisterReverseLookup", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "RegisterReverseLookup")],
            DetectOps = [RegOp.CheckDword(Key, "RegisterReverseLookup", 0)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-suffix-search",
            Label = "Restrict DNS Suffix Search List",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The DNS suffix search list specifies domain suffixes appended to unqualified hostnames during DNS resolution on Windows clients. Restricting the search list to only approved enterprise domain suffixes prevents resolution via unauthorized or external DNS domains. DNS suffix search lists that include external domains can cause inadvertent DNS resolution of internal names through external authoritative servers. An overly permissive search list may leak internal hostname attempts to external resolvers. Enterprise DNS search lists should be set to only the organization's internal domain names through Group Policy. Restricting the search list is applied in conjunction with setting an explicit approved list of enterprise suffixes.",
            Tags = ["dns", "search-list", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AppendToMultiLabelName", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppendToMultiLabelName")],
            DetectOps = [RegOp.CheckDword(Key, "AppendToMultiLabelName", 0)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-over-tcp",
            Label = "Disable DNS Resolution Fallback over TCP",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "DNS clients fall back to TCP when UDP responses are truncated to accommodate large DNS responses such as those using DNSSEC. Disabling TCP fallback prevents the DNS client from retrying queries over TCP when UDP responses are truncated. In environments without DNSSEC or large TXT record needs, UDP responses are rarely truncated and TCP fallback is never triggered. Disabling TCP fallback may cause resolution failures for domains with very large DNS record sets that exceed UDP payload limits. Enterprise environments using DNSSEC validation should not disable TCP fallback as DNSSEC signatures require it. This setting is only appropriate for environments with very constrained DNS infrastructure where TCP is unavailable.",
            Tags = ["dns", "tcp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDnsOverTcpFallback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDnsOverTcpFallback")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDnsOverTcpFallback", 1)],
        },
        new TweakDef
        {
            Id = "dnssec-disable-updateregistration",
            Label = "Disable Dynamic DNS A Record Registration",
            Category = "DNS Secure Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows clients automatically register their IP-to-hostname A records with the DNS server through dynamic DNS updates. Disabling dynamic A record registration prevents workstations from automatically adding or updating their hostname entries in DNS. Dynamic DNS registration from client workstations can lead to DNS pollution if clients change IP addresses or names frequently. Enterprise DNS should be managed through DHCP server-side registration or infrastructure provisioning tools. Client-initiated DNS updates are harder to audit and control than server-managed DNS record maintenance. Disabling client-side DNS registration ensures that DNS records are only modified by authorized infrastructure management processes.",
            Tags = ["dns", "registration", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RegistrationEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "RegistrationEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "RegistrationEnabled", 0)],
        },
    ];
}
