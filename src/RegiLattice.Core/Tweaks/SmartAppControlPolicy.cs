// RegiLattice.Core — Tweaks/SmartAppControlPolicy.cs
// Smart App Control Group Policy controls — Sprint 372.
// Category: "Smart App Control Policy" | Slug: sac
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartAppControl
// MinBuild: 22621 (Windows 11 22H2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmartAppControlPolicy
{
    private const string SacKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartAppControl";
    private const string WdCiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen";
    private const string SacStateKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "sac-block-policy-change",
            Label = "Block User Changes to Smart App Control State",
            Category = "Smart App Control Policy",
            Description = "Prevents users from changing the Smart App Control state (evaluation / on / off) via Windows Security settings. The state set by the administrator via policy is locked in place.",
            Tags = ["sac", "smart-app-control", "policy", "user-lock", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Ensures Smart App Control remains in its managed state regardless of user preferences.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "ConfigureSmartAppControl", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "ConfigureSmartAppControl")],
            DetectOps = [RegOp.CheckDword(SacKey, "ConfigureSmartAppControl", 1)],
        },
        new TweakDef
        {
            Id = "sac-enable-enforcement-mode",
            Label = "Set Smart App Control to Enforcement Mode",
            Category = "Smart App Control Policy",
            Description = "Forces Smart App Control into full Enforcement mode, blocking unsigned and reputation-negative apps from running. Moves the system out of Evaluation mode.",
            Tags = ["sac", "smart-app-control", "enforcement", "app-block", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Blocks all apps that do not have a valid code signature or positive Microsoft cloud reputation. Test on a pilot group; may block legitimate unsigned tools.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "SmartAppControlState", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "SmartAppControlState")],
            DetectOps = [RegOp.CheckDword(SacKey, "SmartAppControlState", 1)],
        },
        new TweakDef
        {
            Id = "sac-disable-evaluation-mode",
            Label = "Disable Smart App Control Evaluation Mode",
            Category = "Smart App Control Policy",
            Description = "Prevents Windows from running Smart App Control in Evaluation mode, which silently collects data about apps that would be blocked by enforcement. Requires choosing explicit On or Off state.",
            Tags = ["sac", "smart-app-control", "evaluation", "policy", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures the device is in a known enforcement state rather than the ambiguous evaluation state.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "DisableEvaluationMode", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "DisableEvaluationMode")],
            DetectOps = [RegOp.CheckDword(SacKey, "DisableEvaluationMode", 1)],
        },
        new TweakDef
        {
            Id = "sac-require-signed-publishers",
            Label = "Require Signed Publishers for All Executable Content",
            Category = "Smart App Control Policy",
            Description = "Configures Smart App Control to require a valid, traceable code-signing publisher certificate for all PE executables, MSI packages, and scripts. Unsigned content is blocked.",
            Tags = ["sac", "smart-app-control", "code-signing", "publisher", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Strong protection against unsigned malware; breaks all in-house tools that lack a valid code-signing certificate. Ensure all LOB apps are signed before enabling.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "RequireSignedPublishers", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "RequireSignedPublishers")],
            DetectOps = [RegOp.CheckDword(SacKey, "RequireSignedPublishers", 1)],
        },
        new TweakDef
        {
            Id = "sac-block-malicious-script-execution",
            Label = "Block Script Files Identified as Malicious by SAC",
            Category = "Smart App Control Policy",
            Description = "Enables Smart App Control to block script execution (JS, VBS, PS1, CMD) when the script file or publisher is identified as malicious by the Microsoft Intelligent Security Graph.",
            Tags = ["sac", "smart-app-control", "scripts", "malicious", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Stops script-based threats (LotL attacks) that use reputation-negative or anonymous scripts.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "BlockMaliciousScripts", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "BlockMaliciousScripts")],
            DetectOps = [RegOp.CheckDword(SacKey, "BlockMaliciousScripts", 1)],
        },
        new TweakDef
        {
            Id = "sac-audit-blocked-file-events",
            Label = "Enable Audit Events for SAC-Blocked Files",
            Category = "Smart App Control Policy",
            Description = "Configures Smart App Control to write an Windows event for every file that is blocked or audited, including the file hash, publisher, and reason for the block decision.",
            Tags = ["sac", "smart-app-control", "audit", "event-log", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Provides a forensic record of blocked app attempts, supporting SOC investigation and compliance reporting.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "AuditBlockedFileEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "AuditBlockedFileEvents")],
            DetectOps = [RegOp.CheckDword(SacKey, "AuditBlockedFileEvents", 1)],
        },
        new TweakDef
        {
            Id = "sac-disable-cloud-lookup",
            Label = "Disable Smart App Control Cloud Reputation Lookup",
            Category = "Smart App Control Policy",
            Description = "Prevents SAC from sending file hashes and metadata to the Microsoft Intelligent Security Graph cloud service for reputation evaluation. SAC falls back to local developer-mode checks only.",
            Tags = ["sac", "smart-app-control", "cloud", "privacy", "network-isolation"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Suitable for air-gapped or high-security environments; reduces SAC effectiveness as the cloud model is the primary signal source.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "DisableCloudReputationLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "DisableCloudReputationLookup")],
            DetectOps = [RegOp.CheckDword(SacKey, "DisableCloudReputationLookup", 1)],
        },
        new TweakDef
        {
            Id = "sac-extend-to-network-paths",
            Label = "Apply Smart App Control to Network-Path Executables",
            Category = "Smart App Control Policy",
            Description = "Extends Smart App Control enforcement to executables launched from UNC network paths and mapped drives, not just local storage. Prevents bypass by placing unsigned tools on a file share.",
            Tags = ["sac", "smart-app-control", "network", "unc-path", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Network-launched binaries are less commonly signed; pilot before enforcing to avoid blocking legitimate admin tools from network shares.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "ExtendToNetworkPaths", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "ExtendToNetworkPaths")],
            DetectOps = [RegOp.CheckDword(SacKey, "ExtendToNetworkPaths", 1)],
        },
        new TweakDef
        {
            Id = "sac-block-lolbas-abuse",
            Label = "Block Known LOLBAS Misuse via Smart App Control",
            Category = "Smart App Control Policy",
            Description = "Enables additional Smart App Control rules that block known Living-off-the-Land Binaries and Scripts (LOLBAS) from being used in patterns typically associated with attackers (e.g., certutil download, regsvr32 scriptlet).",
            Tags = ["sac", "smart-app-control", "lolbas", "living-off-land", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "May interfere with legitimate administrative use of tools like certutil, msiexec, or rundll32. Review the specific exclusions needed before enabling.",
            RegistryKeys = [SacKey],
            ApplyOps  = [RegOp.SetDword(SacKey, "BlockLolbasAbuse", 1)],
            RemoveOps = [RegOp.DeleteValue(SacKey, "BlockLolbasAbuse")],
            DetectOps = [RegOp.CheckDword(SacKey, "BlockLolbasAbuse", 1)],
        },
        new TweakDef
        {
            Id = "sac-enable-intelligent-security-graph",
            Label = "Enable Intelligent Security Graph Integration for SAC",
            Category = "Smart App Control Policy",
            Description = "Enables the Microsoft Intelligent Security Graph (ISG) integration for Smart App Control, allowing real-time reputation data from the Microsoft cloud threat intelligence service to inform allow/deny decisions.",
            Tags = ["sac", "smart-app-control", "isg", "cloud-intelligence", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "ISG provides continuously updated threat data; keeping it enabled ensures SAC decisions reflect the latest known-bad software intelligence.",
            RegistryKeys = [WdCiKey],
            ApplyOps  = [RegOp.SetDword(WdCiKey, "EnableIntelligentSecurityGraph", 1)],
            RemoveOps = [RegOp.DeleteValue(WdCiKey, "EnableIntelligentSecurityGraph")],
            DetectOps = [RegOp.CheckDword(WdCiKey, "EnableIntelligentSecurityGraph", 1)],
        },
    ];
}
