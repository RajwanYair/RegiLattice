#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 253 — Scripted Diagnostics & Troubleshooting Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\WinRM\WinRS
internal static class ScriptedDiagnosticsPolicy
{
    private const string SdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics";
    private const string SdProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy";
    private const string TshootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Troubleshooting\AllowRecommendations";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sdiag-disable-scripted-diagnostics",
            Label = "Disable Scripted Diagnostics Execution",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets ExecutionPolicy=0 in the ScriptedDiagnostics policy key. "
                + "Prevents Windows from executing scripted diagnostic packages (.diagpkg, .diag files), "
                + "including the automated troubleshooters triggered from 'Troubleshoot settings'. "
                + "Reduces data collection and prevents unintended automated changes. "
                + "Default: absent (diagnostics run). Recommended: 1 on managed or high-security systems.",
            Tags = ["diagnostics", "scripted", "troubleshooter", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Scripted diagnostic packages (.diagpkg) cannot execute; automated troubleshooters are blocked.",
            ApplyOps = [RegOp.SetDword(SdKey, "ExecutionPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "ExecutionPolicy")],
            DetectOps = [RegOp.CheckDword(SdKey, "ExecutionPolicy", 0)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-online-troubleshooters",
            Label = "Disable Online Troubleshooting Recommendations",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets EnabledPolicy=0 in the ScriptedDiagnosticsProvider Policy key. "
                + "Prevents Windows from downloading and applying troubleshooting recommendations from Microsoft's "
                + "online diagnostic database. Stops automatic remediation steps that could modify system settings. "
                + "Default: absent (online recommendations enabled). Recommended: 0 on air-gapped or privacy-sensitive systems.",
            Tags = ["diagnostics", "online", "recommendations", "troubleshooter", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Online troubleshooting recommendations from Microsoft not fetched or applied.",
            ApplyOps = [RegOp.SetDword(SdProv, "EnabledPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(SdProv, "EnabledPolicy")],
            DetectOps = [RegOp.CheckDword(SdProv, "EnabledPolicy", 0)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-recommended-troubleshooting",
            Label = "Disable Windows Recommended Troubleshooting",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets TurnOffWindowsErrorReportingServer=1 in the AllowRecommendations "
                + "Troubleshooting policy key. Disables the 'Recommended troubleshooting' feature "
                + "that automatically diagnoses and resolves common problems. Prevents Windows from "
                + "silently applying fixes based on crash data from Windows Error Reporting. "
                + "Default: absent. Recommended: 1 when automated fixes are undesired in production environments.",
            Tags = ["diagnostics", "recommended", "auto-fix", "troubleshooter", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Windows Recommended Troubleshooting feature disabled; no automatic problem fixes applied.",
            ApplyOps = [RegOp.SetDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
            RemoveOps = [RegOp.DeleteValue(TshootKey, "TurnOffWindowsErrorReportingServer")],
            DetectOps = [RegOp.CheckDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-automatic-maintenance-diagnostics",
            Label = "Disable Automatic Maintenance Diagnostics",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets EnableAutomatedTroubleshooting=0 in the ScriptedDiagnostics policy key. "
                + "Prevents Windows Automatic Maintenance from running scripted diagnostic jobs "
                + "in the background during maintenance windows. Avoids unexpected system changes from "
                + "background maintenance troubleshooters. "
                + "Default: absent (enabled). Recommended: 0 in change-controlled environments.",
            Tags = ["diagnostics", "maintenance", "automated", "background", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Scripted diagnostic jobs from Windows Automatic Maintenance are disabled.",
            ApplyOps = [RegOp.SetDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "EnableAutomatedTroubleshooting")],
            DetectOps = [RegOp.CheckDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-elevated-troubleshooter",
            Label = "Disable Elevated Scripted Troubleshooter Execution",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets RunAsHighestAvailablePrivilege=0 in the ScriptedDiagnostics policy key. "
                + "Prevents scripted diagnostic packages from automatically requesting elevation to "
                + "run with highest available privileges. Forces diagnostics to run as standard user "
                + "unless explicitly elevated by an administrator. "
                + "Default: absent (auto-elevation allowed). Recommended: 0 on principle-of-least-privilege systems.",
            Tags = ["diagnostics", "elevation", "uac", "privilege", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Diagnostic packages cannot auto-elevate; admin must explicitly run elevated troubleshooters.",
            ApplyOps = [RegOp.SetDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "RunAsHighestAvailablePrivilege")],
            DetectOps = [RegOp.CheckDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-results-upload",
            Label = "Disable Diagnostic Results Upload",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets AllowDiagnosticDataUpload=0 in the ScriptedDiagnostics policy key. "
                + "Prevents scripted diagnostic packages from uploading their results logs, "
                + "diagnostic data, or anonymised telemetry to Microsoft or third-party servers. "
                + "Default: absent (upload allowed). Recommended: 0 on air-gapped or privacy-sensitive systems.",
            Tags = ["diagnostics", "upload", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Diagnostic results not uploaded; data stays on-device.",
            ApplyOps = [RegOp.SetDword(SdKey, "AllowDiagnosticDataUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "AllowDiagnosticDataUpload")],
            DetectOps = [RegOp.CheckDword(SdKey, "AllowDiagnosticDataUpload", 0)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-user-initiated-troubleshooter",
            Label = "Block User-Initiated Troubleshooters",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets DisableUserDiagnostics=1 in the ScriptedDiagnostics policy key. "
                + "Prevents non-administrator users from launching troubleshooters from Settings "
                + "('Get help', 'Troubleshoot', 'Fix problems'). Only administrators can initiate "
                + "diagnostic packages. Useful on shared or terminal-server machines. "
                + "Default: absent (users can launch troubleshooters). Recommended: 1 on kiosk/terminal machines.",
            Tags = ["diagnostics", "user", "kiosk", "restrict", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Non-admin users cannot launch Windows troubleshooters from Settings.",
            ApplyOps = [RegOp.SetDword(SdKey, "DisableUserDiagnostics", 1)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "DisableUserDiagnostics")],
            DetectOps = [RegOp.CheckDword(SdKey, "DisableUserDiagnostics", 1)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-third-party-diagnostics",
            Label = "Block Third-Party Diagnostic Packages",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets AllowThirdPartyDiagnostics=0 in the ScriptedDiagnostics policy key. "
                + "Prevents Windows from running scripted diagnostic packages (.diagpkg) from publishers "
                + "other than Microsoft. Only Microsoft-signed diagnostic packages are permitted to run. "
                + "Default: absent (third-party packages allowed). Recommended: 0 to limit diagnostic execution surface.",
            Tags = ["diagnostics", "third-party", "packages", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Third-party diagnostic packages (.diagpkg) blocked; only Microsoft-signed packages run.",
            ApplyOps = [RegOp.SetDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "AllowThirdPartyDiagnostics")],
            DetectOps = [RegOp.CheckDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-scheduled-diagnostics",
            Label = "Disable Scheduled Diagnostic Tasks",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets DisableScheduledDiagnostics=1 in the ScriptedDiagnostics policy key. "
                + "Prevents the Scheduled Maintenance Diagnostics task scheduler jobs from creating "
                + "or running scripted diagnostic tasks in the background on a schedule. "
                + "Reduces background system load and unexpected modifications. "
                + "Default: absent (scheduled diagnostics run). Recommended: 1 on optimised/stable systems.",
            Tags = ["diagnostics", "scheduled", "maintenance", "task-scheduler", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Scheduled background diagnostic maintenance tasks are blocked.",
            ApplyOps = [RegOp.SetDword(SdKey, "DisableScheduledDiagnostics", 1)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "DisableScheduledDiagnostics")],
            DetectOps = [RegOp.CheckDword(SdKey, "DisableScheduledDiagnostics", 1)],
        },
        new TweakDef
        {
            Id = "sdiag-disable-troubleshooting-history",
            Label = "Disable Troubleshooting History Storage",
            Category = "Scripted Diagnostics Policy",
            Description =
                "Sets DisableTroubleshootingHistory=1 in the ScriptedDiagnostics policy key. "
                + "Prevents Windows from writing troubleshooter run results and histories to the "
                + "machine's troubleshooting log database. Reduces local data accumulation from "
                + "diagnostic activities. "
                + "Default: absent (history stored). Recommended: 1 on privacy-focused or ephemeral systems.",
            Tags = ["diagnostics", "history", "privacy", "logging", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Troubleshooter run history and results are not stored in the local database.",
            ApplyOps = [RegOp.SetDword(SdKey, "DisableTroubleshootingHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(SdKey, "DisableTroubleshootingHistory")],
            DetectOps = [RegOp.CheckDword(SdKey, "DisableTroubleshootingHistory", 1)],
        },
    ];
}
