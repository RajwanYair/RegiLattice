// RegiLattice.Core — Tweaks/SmartScreenAdvancedPolicy.cs
// Windows Defender SmartScreen advanced configuration, URL scan, and app reputation policy — Sprint 502.
// Category: "SmartScreen Advanced Policy" | Slug: ssadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmartScreenAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter";
    private const string ExplKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ssadv-enable-smartscreen-shell",
                Label = "Enable SmartScreen for Apps and Files in Windows Shell",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Enables Windows Defender SmartScreen reputation checks for executables and apps launched from Windows Explorer and the shell, blocking or warning on programmes whose reputation is unknown or known-malicious.",
                Tags = ["smartscreen", "app-reputation", "shell", "defender", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "SmartScreen enabled in shell; unknown or malicious executables blocked/warned before execution.",
                ApplyOps = [RegOp.SetString(ExplKey, "SmartScreenEnabled", "RequireAdmin")],
                RemoveOps = [RegOp.DeleteValue(ExplKey, "SmartScreenEnabled")],
                DetectOps = [RegOp.CheckString(ExplKey, "SmartScreenEnabled", "RequireAdmin")],
            },
            new TweakDef
            {
                Id = "ssadv-enable-smartscreen-store-apps",
                Label = "Enable SmartScreen for Microsoft Store Apps",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Enables SmartScreen reputation checks for Universal Windows Platform (UWP) apps installed from the Microsoft Store, checking each app against the SmartScreen database before allowing execution.",
                Tags = ["smartscreen", "store-apps", "uwp", "defender", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SmartScreen enabled for Store apps; UWP apps checked against reputation before execution.",
                ApplyOps = [RegOp.SetDword(Key, "EnableSmartScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSmartScreen")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSmartScreen", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-block-smartscreen-override",
                Label = "Block Users from Overriding SmartScreen Warnings",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Prevents users from clicking through SmartScreen warnings to run files that SmartScreen has flagged as unknown or malicious, transforming advisory warnings into hard blocks.",
                Tags = ["smartscreen", "override", "block", "defender", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "SmartScreen block enforced; users cannot click through unknown/malicious file warnings.",
                ApplyOps = [RegOp.SetDword(ExplKey, "PreventOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplKey, "PreventOverride")],
                DetectOps = [RegOp.CheckDword(ExplKey, "PreventOverride", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-enable-edge-phish-filter",
                Label = "Enable Edge Phishing Filter (SmartScreen for URLs)",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Enables the Edge/IE SmartScreen Phishing Filter that checks every visited URL against Microsoft's database of known phishing and malware distribution sites, warning or blocking access to malicious web pages.",
                Tags = ["smartscreen", "phishing-filter", "edge", "url", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Edge phishing filter enabled; visited URLs checked against Microsoft malicious site database.",
                ApplyOps = [RegOp.SetDword(EdgeKey, "EnabledV9", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnabledV9")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EnabledV9", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-block-edge-phish-override",
                Label = "Block Users from Overriding Edge Phishing Site Warnings",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Prevents users from clicking through Edge SmartScreen warnings about phishing sites, making phishing site warnings non-bypassable hard blocks to protect against social engineering attacks.",
                Tags = ["smartscreen", "phishing", "edge", "override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Edge phishing site blocks enforced; users cannot bypass SmartScreen warnings for malicious URLs.",
                ApplyOps = [RegOp.SetDword(EdgeKey, "PreventOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PreventOverride")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PreventOverride", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-configure-smartscreen-warn-mode",
                Label = "Set SmartScreen to Warn Instead of Block for Unknown Files",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Configures SmartScreen to show a warning for unknown-reputation files (allowing the user to proceed with admin approval) rather than silently blocking them, balancing security enforcement with operational flexibility.",
                Tags = ["smartscreen", "warn-mode", "unknown-files", "defender", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SmartScreen set to warn mode; unknown files show warning before execution, admin can approve.",
                ApplyOps = [RegOp.SetDword(ExplKey, "SmartScreenLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplKey, "SmartScreenLevel")],
                DetectOps = [RegOp.CheckDword(ExplKey, "SmartScreenLevel", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-enable-smartscreen-pwned-password",
                Label = "Enable SmartScreen Password Breach Detection",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Enables Windows SmartScreen enhanced phishing protection that detects when the user types a password that has been found in known data breaches, warning the user to change the compromised credential.",
                Tags = ["smartscreen", "password-breach", "phishing-protection", "pwned", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "SmartScreen password breach detection enabled; users warned when typing known-compromised passwords.",
                ApplyOps = [RegOp.SetDword(Key, "EnableWebContentEvaluationService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableWebContentEvaluationService")],
                DetectOps = [RegOp.CheckDword(Key, "EnableWebContentEvaluationService", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-log-smartscreen-blocks",
                Label = "Log SmartScreen Block and Warning Events",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Enables Security event log entries for each SmartScreen block or warning event, providing an audit trail of attempted execution of unknown or malicious files for security monitoring.",
                Tags = ["smartscreen", "event-log", "audit", "defender", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SmartScreen block/warn events logged; security team can audit attempted file execution on endpoints.",
                ApplyOps = [RegOp.SetDword(Key, "AuditSmartScreenEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSmartScreenEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSmartScreenEvents", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-disable-smartscreen-cloud-bypass",
                Label = "Disable SmartScreen Cloud Check Bypass on Network Failure",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Configures SmartScreen to block execution of unknown files even when it cannot reach the cloud reputation service (network unavailable), enforcing a fail-closed posture instead of allowing execution on cloud timeout.",
                Tags = ["smartscreen", "offline", "fail-closed", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "SmartScreen fails closed; unknown files blocked even when cloud service is unreachable.",
                ApplyOps = [RegOp.SetDword(ExplKey, "BlockCloudBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplKey, "BlockCloudBypass")],
                DetectOps = [RegOp.CheckDword(ExplKey, "BlockCloudBypass", 1)],
            },
            new TweakDef
            {
                Id = "ssadv-disable-smartscreen-telemetry",
                Label = "Disable SmartScreen Telemetry Reporting to Microsoft",
                Category = "SmartScreen Advanced Policy",
                Description =
                    "Prevents SmartScreen from sending telemetry about checked URLs, app names, and reputation query results to Microsoft, reducing cloud data disclosure while keeping local SmartScreen checks active.",
                Tags = ["smartscreen", "telemetry", "privacy", "microsoft", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SmartScreen telemetry disabled; app names and URL queries not sent to Microsoft telemetry pipeline.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSmartScreenTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartScreenTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSmartScreenTelemetry", 1)],
            },
        ];
}
