// RegiLattice.Core — Tweaks/LegacyProtocols.cs
// Disable deprecated/insecure legacy network protocols: LLMNR, mDNS, NetBIOS, LLTD, ISATAP.
// Slug: "legprot" — HKLM system services and DNS client parameters.
// Disabling these eliminates common broadcast-based attack surfaces.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LegacyProtocols
{
    private const string DnsClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

    private const string DnsClientSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters";

    private const string NetBtParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

    private const string LltdSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lltdsvc";

    private const string TeledoSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";

    private const string IpHlpSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\iphlpsvc\Parameters\Teredo";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "legprot-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["llmnr", "multicast", "name resolution", "responder", "security"],
            Description =
                "Disables LLMNR — a broadcast-based name resolution protocol exploited by "
                + "tools like Responder to perform NTLM credential capture via LLMNR poisoning. "
                + "EnableMulticast=0 in DNS Client policy. Non-domain devices should use DNS only.",
            ApplyOps = [RegOp.SetDword(DnsClient, "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsClient, "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(DnsClient, "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "legprot-disable-mdns",
            Label = "Disable mDNS (Multicast DNS / Bonjour)",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["mdns", "multicast dns", "bonjour", "zeroconf", "security"],
            Description =
                "Disables mDNS (Multicast DNS, also known as Bonjour or Zeroconf). "
                + "EnableMDNS=0 in Dnscache parameters. mDNS is only needed for zero-config "
                + "local discovery; on corporate or secure networks, use DNS and WINS instead.",
            ApplyOps = [RegOp.SetDword(DnsClientSvc, "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsClientSvc, "EnableMDNS")],
            DetectOps = [RegOp.CheckDword(DnsClientSvc, "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "legprot-disable-netbios",
            Label = "Disable NetBIOS over TCP/IP System-Wide",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["netbios", "nbns", "legacy protocol", "security"],
            Description =
                "Disables NetBIOS over TCP/IP at the system level. NetbiosOptions=2 sets "
                + "NetBIOS as disabled by default (overrides DHCP). NetBIOS is exploited "
                + "by NBNS poisoning attacks. WARNING: may break legacy app connectivity "
                + "that relies on NetBIOS name resolution on older Windows networks.",
            ApplyOps = [RegOp.SetDword(NetBtParams, "NetbiosOptions", 2)],
            RemoveOps = [RegOp.SetDword(NetBtParams, "NetbiosOptions", 0)],
            DetectOps = [RegOp.CheckDword(NetBtParams, "NetbiosOptions", 2)],
        },
        new TweakDef
        {
            Id = "legprot-disable-lltd",
            Label = "Disable LLTD (Link-Layer Topology Discovery)",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["lltd", "network discovery", "topology", "legacy"],
            Description =
                "Disables the Link-Layer Topology Discovery (LLTD) service — used to build "
                + "the Network Map in Windows Vista/7. Rarely needed on modern networks. "
                + "Setting Start=4 disables the lltdsvc service.",
            ApplyOps = [RegOp.SetDword(LltdSvc, "Start", 4)],
            RemoveOps = [RegOp.SetDword(LltdSvc, "Start", 3)],
            DetectOps = [RegOp.CheckDword(LltdSvc, "Start", 4)],
        },
        new TweakDef
        {
            Id = "legprot-disable-teredo",
            Label = "Disable Teredo IPv6 Tunneling",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["teredo", "ipv6", "tunnel", "legacy network"],
            Description =
                "Disables Teredo — an IPv6 tunneling mechanism that encapsulates IPv6 traffic "
                + "in IPv4 UDP packets. Teredo can bypass firewalls and creates unexpected "
                + "network exposure. DisabledComponents bitmask is set via TCP/IP v6 parameters.",
            ApplyOps = [RegOp.SetDword(TeledoSvc, "DisabledComponents", 1)],
            RemoveOps = [RegOp.DeleteValue(TeledoSvc, "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(TeledoSvc, "DisabledComponents", 1)],
        },
        new TweakDef
        {
            Id = "legprot-disable-isatap",
            Label = "Disable ISATAP IPv6 Tunneling",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["isatap", "ipv6", "tunnel", "legacy network"],
            Description =
                "Disables ISATAP (Intra-Site Automatic Tunnel Addressing Protocol) — an "
                + "IPv6 transition mechanism that tunnels IPv6 in IPv4 within an organization. "
                + "Modern networks use native IPv6; ISATAP adds unnecessary complexity and "
                + "attack surface. DisabledComponents+=4 in the bitmask.",
            ApplyOps = [RegOp.SetDword(TeledoSvc, "DisabledComponents", 4)],
            RemoveOps = [RegOp.DeleteValue(TeledoSvc, "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(TeledoSvc, "DisabledComponents", 4)],
        },
        new TweakDef
        {
            Id = "legprot-disable-6to4",
            Label = "Disable IPv6-to-IPv4 Transition (6to4)",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["6to4", "ipv6", "transition", "legacy protocol"],
            Description =
                "Disables the 6to4 service — an IPv6 transition technology that wraps IPv6 "
                + "packets in IPv4. Rarely used in modern networks and creates a potential "
                + "covert channel. DisabledComponents+=2 in TCP/IP v6 parameters.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
        },
        new TweakDef
        {
            Id = "legprot-disable-wins-client",
            Label = "Disable WINS Client Lookup",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["wins", "netbios", "name resolution", "legacy"],
            Description =
                "Disables WINS (Windows Internet Naming Service) client lookups. "
                + "EnableWins=0 in NetBT parameters. WINS is a legacy NetBIOS name resolution "
                + "service superseded by DNS. Disabling it on non-legacy environments "
                + "reduces name-resolution attack surface.",
            ApplyOps = [RegOp.SetDword(NetBtParams, "EnableWins", 0)],
            RemoveOps = [RegOp.DeleteValue(NetBtParams, "EnableWins")],
            DetectOps = [RegOp.CheckDword(NetBtParams, "EnableWins", 0)],
        },
        new TweakDef
        {
            Id = "legprot-enforce-dotnet-tls-strong-crypto",
            Label = "Enforce Strong Cryptography for .NET Framework",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["dotnet", "tls", "strong crypto", "schannel", "security"],
            Description =
                "Forces .NET Framework 4.x applications to use strong TLS (TLS 1.2+) instead "
                + "of defaulting to old protocols like SSL 3.0 or TLS 1.0. "
                + "SchUseStrongCrypto=1 in the .NET Framework registry. Critical for legacy .NET "
                + "apps that haven't been explicitly updated to use modern TLS.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319", "SchUseStrongCrypto", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319", "SchUseStrongCrypto", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319", "SchUseStrongCrypto"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319", "SchUseStrongCrypto"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319", "SchUseStrongCrypto", 1)],
        },
        new TweakDef
        {
            Id = "legprot-disable-llmnr-fallback",
            Label = "Disable LLMNR Name Resolution Fallback",
            Category = "Legacy Protocols",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["llmnr", "dns fallback", "name resolution", "responder", "security"],
            Description =
                "Prevents the DNS Client from falling back to LLMNR when DNS resolution fails. "
                + "QueryAdapterName=0 in Dnscache parameters stops adapter-specific LLMNR "
                + "queries, closing a secondary Responder attack vector beyond the primary "
                + "legprot-disable-llmnr policy setting.",
            ApplyOps = [RegOp.SetDword(DnsClientSvc, "QueryAdapterName", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsClientSvc, "QueryAdapterName")],
            DetectOps = [RegOp.CheckDword(DnsClientSvc, "QueryAdapterName", 0)],
        },
    ];
}
