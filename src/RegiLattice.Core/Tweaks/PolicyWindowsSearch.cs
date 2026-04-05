namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 646 — PolicyNetworkIsolation (Network Isolation / AppContainer Policy)

internal static class PolicyWindowsSearch
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsepol-block-remote-query",
            Label = "Block Remote Cortana Query via Policy",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRemoteQuery=1 under the Windows Search Group Policy path. "
                + "Prevents Cortana from querying remote services for information when invoked. "
                + "Queries are processed locally only, reducing data exfiltration risk.",
            Tags = ["search", "cortana", "remote", "policy", "security"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Cortana remote queries; local processing only.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRemoteQuery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteQuery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRemoteQuery", 1)],
        },
    ];
}
