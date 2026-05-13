namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Sprint 648 — Netlogon secure channel and domain authentication policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\NetLogon and
///           HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters
/// Controls domain controller secure channel signing, sealing,
/// NT4 crypto restrictions, and DNS-only domain joining.
/// </summary>
[TweakModule]
internal static class PolicyNetLogon
{
    private const string GpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\NetLogon";
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-netlogon-dns-only-domain-join",
            Label = "Restrict Domain Join to DNS Registration",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowDNSOnlyJoin=1 in Netlogon parameters. "
                + "Prevents the domain join process from using WINS/NetBIOS name resolution to locate domain controllers. "
                + "Forces domain join operations to rely on DNS only, eliminating NetBIOS-based DC discovery that is vulnerable to spoofing.",
            Tags = ["netlogon", "domain-join", "dns", "netbios", "wins", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "DNS-only DC discovery; WINS-based environments may need DNS records updated.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AllowDNSOnlyJoin", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowDNSOnlyJoin")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AllowDNSOnlyJoin", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-avoid-pdc-on-wan",
            Label = "Avoid PDC Emulator on WAN for Authentication",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AvoidPdcOnWan=1 in Netlogon parameters. "
                + "Instructs the Netlogon service not to contact the PDC Emulator across slow WAN links during user authentication. "
                + "Reduces authentication delays at remote branch office sites where WAN latency to the PDC Emulator would cause login hangs.",
            Tags = ["netlogon", "pdc", "wan", "performance", "branch-office", "ad"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Improves login speed at WAN-connected sites; no security downside.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AvoidPdcOnWan", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AvoidPdcOnWan")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AvoidPdcOnWan", 1)],
        },
    ];
}
