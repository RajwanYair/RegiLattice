// RegiLattice.Core — Tweaks/SmartAppControl.cs
// Smart App Control (SAC), HVCI/Memory Integrity, and VBS controls.
// Uses slug "sac" — focuses on overlay security features new in Win11 22H2+.
// Does NOT overlap with Defender.cs (sec-) which covers traditional Defender settings.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
            Id = "sac-set-evaluation-mode",
            Label = "Set Smart App Control to Evaluation Mode",
            Category = "Smart App Control",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "security", "wdac"],
            Description =
                "Puts Smart App Control into evaluation (audit) mode — it monitors "
                + "apps but does not block anything. Useful to assess impact before "
                + "enabling enforcement. Value: 2=Evaluate, 1=Enforce, 0=Off.",
            ApplyOps = [RegOp.SetDword(SacState, "VerifiedAndReputablePolicyState", 2)],
            RemoveOps = [RegOp.DeleteValue(SacState, "VerifiedAndReputablePolicyState")],
            DetectOps = [RegOp.CheckDword(SacState, "VerifiedAndReputablePolicyState", 2)],
        },
        new TweakDef
        {
            Id = "sac-disable-smart-app-control",
            Label = "Disable Smart App Control",
            Category = "Smart App Control",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["smart app control", "sac", "security", "gaming", "performance"],
            Description =
                "Turns off Smart App Control entirely. "
                + "Required for running unsigned apps, custom scripts, or tools with"
                + " unknown publishers. Also avoids false-positive latency during app launch.",
            ApplyOps = [RegOp.SetDword(SacState, "VerifiedAndReputablePolicyState", 0)],
            RemoveOps = [RegOp.SetDword(SacState, "VerifiedAndReputablePolicyState", 2)], // revert to evaluate
            DetectOps = [RegOp.CheckDword(SacState, "VerifiedAndReputablePolicyState", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-hvci",
            Label = "Disable Memory Integrity (HVCI)",
            Category = "Smart App Control",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hvci", "memory integrity", "gaming", "performance", "vbs"],
            Description =
                "Disables Hypervisor-Protected Code Integrity (HVCI / Memory Integrity). "
                + "Can improve gaming frame rates (3–10%) by enabling drivers that are "
                + "incompatible with HVCI. Reduces protection against driver-level malware. "
                + "Requires reboot.",
            ApplyOps = [RegOp.SetDword(HvciPol, "Enabled", 0), RegOp.SetDword(HvciPol, "WasEnabledBy", 0)],
            RemoveOps = [RegOp.SetDword(HvciPol, "Enabled", 1), RegOp.SetDword(HvciPol, "WasEnabledBy", 1)],
            DetectOps = [RegOp.CheckDword(HvciPol, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-virtualization-based-security",
            Label = "Disable Virtualization-Based Security (VBS)",
            Category = "Smart App Control",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["vbs", "virtualization", "gaming", "performance", "security"],
            Description =
                "Disables Virtualization-Based Security — the hypervisor layer that "
                + "hosts HVCI and Credential Guard. Frees CPU/memory reserved for the "
                + "hypervisor. Significant performance impact on some Ryzen systems. "
                + "Requires reboot.",
            ApplyOps = [RegOp.SetDword(VbsPol, "EnableVirtualizationBasedSecurity", 0)],
            RemoveOps = [RegOp.SetDword(VbsPol, "EnableVirtualizationBasedSecurity", 1)],
            DetectOps = [RegOp.CheckDword(VbsPol, "EnableVirtualizationBasedSecurity", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-intelligent-security-graph",
            Label = "Disable ISG Cloud Lookup for Smart App Control",
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Id = "sac-disable-secure-boot-policy-refresh",
            Label = "Disable Secure Boot Policy Refresh at Boot",
            Category = "Smart App Control",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["secure boot", "sac", "boot", "performance"],
            Description =
                "Prevents Windows from re-reading the Secure Boot policy from UEFI "
                + "at every boot, shaving a few milliseconds off POST time on slow "
                + "firmware. Has no security impact after initial policy load.",
            ApplyOps = [RegOp.SetDword(VbsPol, "RequirePlatformSecurityFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(VbsPol, "RequirePlatformSecurityFeatures")],
            DetectOps = [RegOp.CheckDword(VbsPol, "RequirePlatformSecurityFeatures", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-bitlocker-check",
            Label = "Disable BitLocker Status Check for SAC",
            Category = "Smart App Control",
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
