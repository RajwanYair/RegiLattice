namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Smart App Control ─────────────────────────────────────────────────────────
// Merged from SmartAppControl.cs (SAC, HVCI/Memory Integrity, VBS controls)

[TweakModule]
internal static class SmartAppControl
{
    // SAC state values: 0=Off, 1=Enforce, 2=Evaluate
    private const string SacState = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
    private const string VbsPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";
    private const string HvciPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";
    private const string IsgPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";
    private const string WdagPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Hvsi\EnableVirtualizationBasedSecurity";
    private const string AppGuard = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\HVSI";
    private const string SacFeedPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModel\StateChangeNotifications";
    private const string KdmaPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string CiDbg = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sac-disable-intelligent-security-graph",
            Label = "Disable ISG Cloud Lookup for Smart App Control",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["smart app control", "sac", "privacy", "cloud", "telemetry"],
            Description =
                "Prevents Smart App Control from querying Microsoft's Intelligent "
                + "Security Graph (ISG) cloud service to validate app reputation. "
                + "Improves privacy and prevents launch-time network calls, at the cost "
                + "of reduced SAC accuracy.",
            ApplyOps = [RegOp.SetDword(IsgPol, "EnableISGState", 0)],
            RemoveOps = [RegOp.DeleteValue(IsgPol, "EnableISGState")],
            DetectOps = [RegOp.CheckDword(IsgPol, "EnableISGState", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-wdag",
            Label = "Disable Windows Defender Application Guard (WDAG)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["wdag", "application guard", "security", "performance", "browser"],
            Description =
                "Disables WDAG — the containerised browser/Office environment that "
                + "uses Hyper-V to isolate untrusted documents. Frees significant RAM "
                + "and hypervisor overhead if you don't use Office or Edge WDAG mode.",
            ApplyOps = [RegOp.SetDword(AppGuard, "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(AppGuard, "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(AppGuard, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sac-enable-audit-only-code-integrity",
            Label = "Enable Code Integrity Audit Mode (Log Without Block)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "security", "audit", "wdac"],
            Description =
                "Switches Code Integrity enforcement to audit-only mode: violations "
                + "are logged to the event log but not blocked. Useful for assessing "
                + "impact before enforcing WDAC policies.",
            ApplyOps = [RegOp.SetDword(CiDbg, "AuditMode", 1)],
            RemoveOps = [RegOp.SetDword(CiDbg, "AuditMode", 0)],
            DetectOps = [RegOp.CheckDword(CiDbg, "AuditMode", 1)],
        },
        new TweakDef
        {
            Id = "sac-disable-state-change-notifications",
            Label = "Disable SAC State-Change Notifications",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "notifications"],
            Description =
                "Suppresses toast notifications that appear when Smart App Control "
                + "changes state (e.g., switches from Evaluation to Off). "
                + "Prevents distracting pop-ups on managed systems.",
            ApplyOps = [RegOp.SetDword(SacFeedPol, "DisableSacTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(SacFeedPol, "DisableSacTelemetry")],
            DetectOps = [RegOp.CheckDword(SacFeedPol, "DisableSacTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "sac-disable-bitlocker-check",
            Label = "Disable BitLocker Status Check for SAC",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "bitlocker", "security"],
            Description =
                "Stops Smart App Control from querying BitLocker encryption status "
                + "on every app launch, removing a redundant security check that "
                + "adds latency when BitLocker is already confirmed as active.",
            ApplyOps = [RegOp.SetDword(KdmaPol, "DisableSACBitLockerCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(KdmaPol, "DisableSACBitLockerCheck")],
            DetectOps = [RegOp.CheckDword(KdmaPol, "DisableSACBitLockerCheck", 1)],
        },
    ];
}
