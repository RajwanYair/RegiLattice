#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class AppLockerPolicy
{
    private const string SrpBase = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";
    private const string ExeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Exe";
    private const string MsiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Msi";
    private const string ScriptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Script";
    private const string DllKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Dll";
    private const string AppxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Appx";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "applocker-enforce-exe-rules",
            Label = "Enforce AppLocker EXE Rules",
            Category = "AppLocker Policy",
            Description = "Puts the AppLocker EXE rule collection into enforcement mode; unauthorised executables are blocked.",
            Tags = ["applocker", "exe", "enforce", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 2,
            ImpactNote = "Value 1 = enforce; unauthorised EXE files are blocked immediately. Requires well-tested AppLocker rules first.",
            ApplyOps = [RegOp.SetDword(ExeKey, "EnforcementMode", 1)],
            RemoveOps = [RegOp.DeleteValue(ExeKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(ExeKey, "EnforcementMode", 1)],
        },
        new TweakDef
        {
            Id = "applocker-audit-exe-rules",
            Label = "Set AppLocker EXE Rules to Audit Mode",
            Category = "AppLocker Policy",
            Description = "Puts the AppLocker EXE rule collection in audit-only mode; blocked events are logged but execution is allowed.",
            Tags = ["applocker", "exe", "audit", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Value 0 = audit only; events logged to Applications and Services\\ Microsoft\\ Windows\\ AppLocker.evtx.",
            ApplyOps = [RegOp.SetDword(ExeKey, "EnforcementMode", 0)],
            RemoveOps = [RegOp.DeleteValue(ExeKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(ExeKey, "EnforcementMode", 0)],
        },
        new TweakDef
        {
            Id = "applocker-enforce-msi-rules",
            Label = "Enforce AppLocker MSI / Windows Installer Rules",
            Category = "AppLocker Policy",
            Description = "Puts the AppLocker Windows Installer (MSI and MSP) rule collection into enforcement mode.",
            Tags = ["applocker", "msi", "installer", "enforce", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 2,
            ImpactNote = "Blocks unapproved MSI installations; users and scripts cannot install unauthorised packages.",
            ApplyOps = [RegOp.SetDword(MsiKey, "EnforcementMode", 1)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(MsiKey, "EnforcementMode", 1)],
        },
        new TweakDef
        {
            Id = "applocker-enforce-script-rules",
            Label = "Enforce AppLocker Script Rules",
            Category = "AppLocker Policy",
            Description = "Puts the AppLocker Script rule collection into enforcement mode; unapproved PowerShell, VBScript, and batch files are blocked.",
            Tags = ["applocker", "script", "powershell", "enforce", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 2,
            ImpactNote = "Blocks unapproved scripts across PowerShell, CMD, WSH, and HTA; test rules thoroughly before enforcing.",
            ApplyOps = [RegOp.SetDword(ScriptKey, "EnforcementMode", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(ScriptKey, "EnforcementMode", 1)],
        },
        new TweakDef
        {
            Id = "applocker-enforce-dll-rules",
            Label = "Enforce AppLocker DLL Rules",
            Category = "AppLocker Policy",
            Description = "Puts the AppLocker DLL rule collection into enforcement mode; unapproved DLLs and OCX files are blocked from loading.",
            Tags = ["applocker", "dll", "enforce", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 1,
            ImpactNote = "High-impact; blocks unauthorised DLLs. Requires extensive testing; misconfiguration can prevent OS boot.",
            ApplyOps = [RegOp.SetDword(DllKey, "EnforcementMode", 1)],
            RemoveOps = [RegOp.DeleteValue(DllKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(DllKey, "EnforcementMode", 1)],
        },
        new TweakDef
        {
            Id = "applocker-enforce-appx-rules",
            Label = "Enforce AppLocker Packaged App Rules",
            Category = "AppLocker Policy",
            Description = "Puts the AppLocker Packaged App (MSIX/AppX) rule collection into enforcement mode for UWP applications.",
            Tags = ["applocker", "appx", "msix", "uwp", "enforce", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks unapproved UWP and MSIX apps from launching; requires rules that allow business-critical apps.",
            ApplyOps = [RegOp.SetDword(AppxKey, "EnforcementMode", 1)],
            RemoveOps = [RegOp.DeleteValue(AppxKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(AppxKey, "EnforcementMode", 1)],
        },
        new TweakDef
        {
            Id = "applocker-enable-appid-service",
            Label = "Enable Application Identity Service for AppLocker",
            Category = "AppLocker Policy",
            Description = "Configures the Application Identity (AppIDSvc) service to start automatically, which is required for AppLocker enforcement.",
            Tags = ["applocker", "appid", "service", "policy", "application-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AppLocker will not enforce rules unless AppIDSvc is running; this tweak ensures it starts with Windows.",
            ApplyOps = [RegOp.SetDword(SrpBase, "AppIdSvcStartType", 2)],
            RemoveOps = [RegOp.DeleteValue(SrpBase, "AppIdSvcStartType")],
            DetectOps = [RegOp.CheckDword(SrpBase, "AppIdSvcStartType", 2)],
        },
        new TweakDef
        {
            Id = "applocker-enable-exe-auditing",
            Label = "Enable AppLocker EXE Execution Auditing",
            Category = "AppLocker Policy",
            Description = "Enables event log auditing for all AppLocker EXE allow and deny events for visibility without enforcement.",
            Tags = ["applocker", "exe", "audit", "event-log", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "All executable allow/deny events written to AppLocker event log; baseline for building allow-list rules.",
            ApplyOps = [RegOp.SetDword(SrpBase, "EnableCollectionLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(SrpBase, "EnableCollectionLogging")],
            DetectOps = [RegOp.CheckDword(SrpBase, "EnableCollectionLogging", 1)],
        },
        new TweakDef
        {
            Id = "applocker-block-user-rule-creation",
            Label = "Block Standard Users from Creating AppLocker Exceptions",
            Category = "AppLocker Policy",
            Description = "Prevents standard (non-administrator) users from creating AppLocker exception rules or publisher overrides.",
            Tags = ["applocker", "user-rules", "policy", "application-control", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Only administrators can add AppLocker exceptions; prevents users from bypassing application control policy.",
            ApplyOps = [RegOp.SetDword(SrpBase, "UsersCanCreateExceptions", 0)],
            RemoveOps = [RegOp.DeleteValue(SrpBase, "UsersCanCreateExceptions")],
            DetectOps = [RegOp.CheckDword(SrpBase, "UsersCanCreateExceptions", 0)],
        },
        new TweakDef
        {
            Id = "applocker-enable-performance-logging",
            Label = "Enable AppLocker Performance Event Logging",
            Category = "AppLocker Policy",
            Description = "Enables detailed performance telemetry logging for AppLocker rule evaluations to the event log.",
            Tags = ["applocker", "performance", "logging", "policy", "diagnostics"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Logs AppLocker evaluation metrics; minor event log overhead; useful for tuning rule sets.",
            ApplyOps = [RegOp.SetDword(SrpBase, "EnablePerformanceLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(SrpBase, "EnablePerformanceLogging")],
            DetectOps = [RegOp.CheckDword(SrpBase, "EnablePerformanceLogging", 1)],
        },
    ];
}
