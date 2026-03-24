#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class DnsClientRegistrationPolicy
{
    private const string DnsCl = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dnscgpo-disable-dynamic-registration",
            Label = "Disable DNS Dynamic Registration",
            Category = "DNS Client Registration Policy",
            Description = "Disables dynamic DNS registration for all network adapters on this machine. Prevents the client from automatically publishing its IP addresses in DNS. Reduces DNS footprint in privacy-sensitive environments. Default: 1 (enabled). Recommended: 0.",
            Tags = ["dns", "dynamic-registration", "privacy", "network"],
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "RegistrationEnabled", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "RegistrationEnabled")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "RegistrationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-adapter-name-registration",
            Label = "Disable DNS Registration of Adapter Names",
            Category = "DNS Client Registration Policy",
            Description = "Prevents DNS registration of network adapter names (hostname-adapterX records). Only the primary hostname is registered, reducing DNS clutter and information exposure. Default: 0 (adapter names not registered by default). Recommended: 0.",
            Tags = ["dns", "registration", "adapter", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "RegisterAdapterName", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "RegisterAdapterName")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "RegisterAdapterName", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-reverse-lookup-registration",
            Label = "Disable DNS Reverse-Lookup (PTR) Registration",
            Category = "DNS Client Registration Policy",
            Description = "Prevents automatic DNS PTR record registration for this machine's IP addresses. Reduces network exposure by not publishing reverse-lookup mappings. Default: 1. Recommended: 0 for privacy-focused deployments.",
            Tags = ["dns", "ptr", "reverse-lookup", "registration", "privacy"],
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "RegisterReverseLookup", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "RegisterReverseLookup")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "RegisterReverseLookup", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-multicast-fqdn",
            Label = "Disable DNS Multicast FQDN Resolution",
            Category = "DNS Client Registration Policy",
            Description = "Disables use of multicast DNS (mDNS/LLMNR) for FQDN resolution. Reduces broadcast-based name-resolution that can leak hostname information on the local network. Default: 0 (not restricted). Recommended: 1.",
            Tags = ["dns", "multicast", "mdns", "llmnr", "privacy", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "AllowMulticastFQDNDiscovery", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "AllowMulticastFQDNDiscovery")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "AllowMulticastFQDNDiscovery", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-domain-name-devolution",
            Label = "Disable DNS Domain-Name Devolution",
            Category = "DNS Client Registration Policy",
            Description = "Disables DNS domain-name devolution (shortening multi-label names to try parent domain suffixes). Prevents unintended name resolution via parent domains when a query fails at the primary domain. Default: 1. Recommended: 0.",
            Tags = ["dns", "devolution", "name-resolution", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "UseDomainNameDevolution", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "UseDomainNameDevolution")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-append-primary-suffixes",
            Label = "Disable DNS Primary Suffix Appending",
            Category = "DNS Client Registration Policy",
            Description = "Disables automatic appending of the primary DNS suffix and parent suffixes when resolving single-label names. Reduces name-leakage and resolves-to-wrong-server scenarios in multi-domain environments. Default: 1. Recommended: 0.",
            Tags = ["dns", "suffix", "name-resolution", "security"],
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "AppendPrimarySuffixes", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "AppendPrimarySuffixes")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "AppendPrimarySuffixes", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-unicode-dns",
            Label = "Disable Unicode (IDN) DNS Name Resolution",
            Category = "DNS Client Registration Policy",
            Description = "Disables Internationalized Domain Name (IDN) / Unicode DNS resolution. Prevents IDN homograph attacks by refusing Unicode domain name lookups. Default: 1. Recommended: 0 in high-security environments.",
            Tags = ["dns", "idn", "unicode", "security", "homograph"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "AllowUnicodeDNSName", 0)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "AllowUnicodeDNSName")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "AllowUnicodeDNSName", 0)],
        },
        new TweakDef
        {
            Id = "dnscgpo-set-refresh-interval",
            Label = "Set DNS Registration Refresh Interval to 24h",
            Category = "DNS Client Registration Policy",
            Description = "Sets the DNS registration refresh interval to 86 400 seconds (24 hours). The default is every 24 hours but can be overridden by DHCP lease frequency. Explicit policy ensures stable refresh timing. Default: 86400. Recommended: 86400.",
            Tags = ["dns", "registration", "interval", "refresh"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "RegistrationRefreshInterval", 86400)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "RegistrationRefreshInterval")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "RegistrationRefreshInterval", 86400)],
        },
        new TweakDef
        {
            Id = "dnscgpo-disable-smart-name-resolution",
            Label = "Disable DNS Smart Multi-Homed Name Resolution",
            Category = "DNS Client Registration Policy",
            Description = "Disables DNS smart multi-homed name resolution, which forwards queries to all DNS servers on all adapters in parallel. Prevents DNS response spoofing via a rogue adapter on multi-homed machines. Default: not set. Recommended: 1.",
            Tags = ["dns", "multi-home", "smart", "name-resolution", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "DisableSmartNameResolution", 1)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "DisableSmartNameResolution")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dnscgpo-ttl-limit",
            Label = "Cap DNS Negative Cache TTL to 5 Seconds",
            Category = "DNS Client Registration Policy",
            Description = "Sets the maximum DNS negative cache TTL to 5 seconds. Short negative TTL reduces the window where a failed DNS lookup is cached, improving resilience during DNS failover events. Default: 900. Recommended: 5.",
            Tags = ["dns", "ttl", "cache", "negative-cache"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DnsCl],
            ApplyOps   = [RegOp.SetDword(DnsCl, "NegativeCacheTime", 5)],
            RemoveOps  = [RegOp.DeleteValue(DnsCl, "NegativeCacheTime")],
            DetectOps  = [RegOp.CheckDword(DnsCl, "NegativeCacheTime", 5)],
        },
    ];
}
