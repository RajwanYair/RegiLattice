namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyDNSSecurity
{
    private const string DnsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";
    private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager\config";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "net-dns-policy-prefer-local-responses",
            Label = "Prefer Local DNS Responses Over Cached External",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AddrConfigControl=1 in DNS client policy. "
                + "Configures the DNS client to prefer locally produced name resolution results over cached external responses. "
                + "Reduces the window for DNS cache poisoning attacks by ensuring addresses from the local DNS zone take priority over potentially stale or tampered cached entries.",
            Tags = ["dns", "local", "cache-poisoning", "resolution", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces risk from cached DNS poisoning; favors local authoritative answers.",
            ApplyOps = [RegOp.SetDword(DnsKey, "AddrConfigControl", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "AddrConfigControl")],
            DetectOps = [RegOp.CheckDword(DnsKey, "AddrConfigControl", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-update-top-domain-zones",
            Label = "Allow DNS Updates to Top-Level Domain Zones",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UpdateTopLevelDomainZones=0 in DNS client policy. "
                + "Prevents the DNS client from attempting dynamic DNS registration in top-level domain zones (e.g., .com, .net). "
                + "Eliminates noise from failed or unintended TLD zone update requests that may expose internal host information to external authoritative DNS servers.",
            Tags = ["dns", "dynamic-update", "tld", "zone", "registration"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops TLD dynamic DNS registration attempts; no impact on local DNS.",
            ApplyOps = [RegOp.SetDword(DnsKey, "UpdateTopLevelDomainZones", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "UpdateTopLevelDomainZones")],
            DetectOps = [RegOp.CheckDword(DnsKey, "UpdateTopLevelDomainZones", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-use-name-resolution-policy",
            Label = "Enforce Name Resolution Policy Table (NRPT)",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableNRPT=0 in DNS client policy. "
                + "Ensures the Name Resolution Policy Table (NRPT) is active and consulted for every DNS query. "
                + "The NRPT allows per-namespace DNS settings including DNSSEC enforcement and DirectAccess DNS routing. "
                + "Disabling it silently bypasses all namespace-specific security rules.",
            Tags = ["dns", "nrpt", "dnssec", "direct-access", "namespace", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Activates NRPT for per-namespace DNS policy enforcement.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DisableNRPT", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableNRPT")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DisableNRPT", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-attempt-autodial",
            Label = "Disable DNS Auto-Dial Connections",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseAdapterSpecificDomainSuffix=0 in DNS client policy. "
                + "Prevents the DNS client from attempting adapter-specific domain suffix resolution when the primary DNS server is unreachable. "
                + "Avoids sending hostname queries to ISP-controlled DNS servers when the corporate DNS is unreachable, "
                + "preventing corporate hostname leakage to external resolvers.",
            Tags = ["dns", "suffix", "adapter", "fallback", "corporate", "leak"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks adapter-specific suffix fallback; may affect resolution on secondary NICs.",
            ApplyOps = [RegOp.SetDword(DnsKey, "UseAdapterSpecificDomainSuffix", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "UseAdapterSpecificDomainSuffix")],
            DetectOps = [RegOp.CheckDword(DnsKey, "UseAdapterSpecificDomainSuffix", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-devolution-level",
            Label = "Restrict DNS Devolution Level",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DevolveLevel=1 in DNS client policy. "
                + "Limits DNS name devolution to only one step above the fully qualified domain name. "
                + "DNS devolution automatically strips labels from unresolved host names and retries (e.g., 'server' -> 'server.corp' -> 'server.com'). "
                + "Leaving devolution unconstrained risks single-label queries resolving against external TLD registrations.",
            Tags = ["dns", "devolution", "name-resolution", "suffix", "tld"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Limits suffix devolution; unqualified names may not resolve in some configurations.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DevolveLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DevolveLevel")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DevolveLevel", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-disable-hosts-file-resolution",
            Label = "Limit Hosts File Priority in Name Resolution",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HostsFileBypassFlag=0 in DNS client policy. "
                + "Ensures the DNS client does not bypass the standard resolution order to exclusively use the HOSTS file. "
                + "Attackers who obtain write access to %SYSTEMROOT%\\System32\\drivers\\etc\\hosts can redirect any hostname; "
                + "normalizing the resolution stack reduces the impact of hosts-file modification attacks.",
            Tags = ["dns", "hosts-file", "resolution-order", "security", "redirect"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Standard hosts-file behaviour preserved; no bypass of DNS resolution order.",
            ApplyOps = [RegOp.SetDword(DnsKey, "HostsFileBypassFlag", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "HostsFileBypassFlag")],
            DetectOps = [RegOp.CheckDword(DnsKey, "HostsFileBypassFlag", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-register-ptr-records",
            Label = "Enable Auto-Registration of PTR Records",
            Category = "Network — Win Inet",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RegisterReverseLookup=1 in DNS client policy. "
                + "Ensures the DNS client registers PTR (reverse lookup) records in addition to A/AAAA forward records during dynamic DNS registration. "
                + "Reverse records are required by many security monitoring tools, intrusion detection systems, and firewall devices for host identification.",
            Tags = ["dns", "ptr", "reverse-lookup", "registration", "monitoring"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures reverse DNS records exist; required for many security monitoring tools.",
            ApplyOps = [RegOp.SetDword(DnsKey, "RegisterReverseLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "RegisterReverseLookup")],
            DetectOps = [RegOp.CheckDword(DnsKey, "RegisterReverseLookup", 1)],
        },
    ];
}
