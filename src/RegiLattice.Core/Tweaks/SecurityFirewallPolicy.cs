namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyFirewallProfiles
{
    private const string Domain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
    private const string Private = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
    private const string Public = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fw-policy-domain-allow-outbound",
            Label = "Allow Outbound by Default (Domain Profile)",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Domain profile Group Policy path. "
                + "Permits outbound connections by default when on a domain network, while still logging them. "
                + "Allows legitimate outbound traffic without requiring per-application outbound rules for normal domain operations.",
            Tags = ["firewall", "domain", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Maintains normal outbound connectivity on domain networks.",
            ApplyOps = [RegOp.SetDword(Domain, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Domain, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Domain, "DefaultOutboundAction", 0)],
        },
        new TweakDef
        {
            Id = "fw-policy-public-allow-outbound",
            Label = "Allow Outbound by Default (Public Profile)",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Public profile Group Policy path. "
                + "Permits outbound connections on public networks so users can browse the web and access cloud services normally. "
                + "Paired with strict inbound blocking to balance security and usability on untrusted networks.",
            Tags = ["firewall", "public", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Preserves outbound connectivity on public networks.",
            ApplyOps = [RegOp.SetDword(Public, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Public, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Public, "DefaultOutboundAction", 0)],
        },
    ];
}
