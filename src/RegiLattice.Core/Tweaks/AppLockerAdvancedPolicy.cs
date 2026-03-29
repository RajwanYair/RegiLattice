// RegiLattice.Core — Tweaks/AppLockerAdvancedPolicy.cs
// AppLocker publisher rules, DLL enforcement, and packaged app policy — Sprint 505.
// Category: "AppLocker Advanced Policy" | Slug: alockadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SrpV2

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppLockerAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";
    private const string ExeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Exe";
    private const string DllKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Dll";
    private const string MsiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Msi";
    private const string ScriptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Script";
    private const string AppxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Appx";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "alockadv-enable-applocker-audit-exe",
                Label = "Enable AppLocker Audit Mode for Executables",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Configures the AppLocker EXE rule collection to Audit mode, which logs every executable launch against rules (EventID 8004) without blocking it, enabling policy discovery before enforcement mode is activated.",
                Tags = ["applocker", "audit-mode", "exe", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AppLocker EXE collection in Audit mode; all executable launches logged without blocking.",
                ApplyOps = [RegOp.SetDword(ExeKey, "EnforcementMode", 1)],
                RemoveOps = [RegOp.DeleteValue(ExeKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(ExeKey, "EnforcementMode", 1)],
            },
            new TweakDef
            {
                Id = "alockadv-enable-applocker-enforce-exe",
                Label = "Enable AppLocker Enforce Mode for Executables",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Enables AppLocker enforcement for executables, blocking any EXE file that does not match an allowed publisher, path, or hash rule — converting the application control policy from advisory to blocking.",
                Tags = ["applocker", "enforce-mode", "exe", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "AppLocker EXE enforcement active; unlisted executables blocked. Requires allow-list rules to be in place.",
                ApplyOps = [RegOp.SetDword(ExeKey, "EnforcementMode", 2)],
                RemoveOps = [RegOp.DeleteValue(ExeKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(ExeKey, "EnforcementMode", 2)],
            },
            new TweakDef
            {
                Id = "alockadv-enable-applocker-dll-enforcement",
                Label = "Enable AppLocker DLL Enforcement",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Enables AppLocker DLL collection enforcement, which checks every DLL loaded by a process against AppLocker rules before allowing load, providing defence against DLL hijacking and side-loading attacks.",
                Tags = ["applocker", "dll", "dll-enforcement", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "AppLocker DLL enforcement active. All DLL loads checked — may impact performance significantly.",
                ApplyOps = [RegOp.SetDword(DllKey, "EnforcementMode", 2)],
                RemoveOps = [RegOp.DeleteValue(DllKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(DllKey, "EnforcementMode", 2)],
            },
            new TweakDef
            {
                Id = "alockadv-enable-applocker-script-enforcement",
                Label = "Enable AppLocker Script Enforcement",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Enables AppLocker Script collection enforcement for PowerShell (.ps1), batch (.cmd/.bat), VBScript (.vbs), and Windows Scripting Host files, blocking untrusted scripts from executing.",
                Tags = ["applocker", "script", "powershell", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "AppLocker script enforcement active; untrusted PS1/VBS/CMD scripts blocked from executing.",
                ApplyOps = [RegOp.SetDword(ScriptKey, "EnforcementMode", 2)],
                RemoveOps = [RegOp.DeleteValue(ScriptKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(ScriptKey, "EnforcementMode", 2)],
            },
            new TweakDef
            {
                Id = "alockadv-enable-applocker-appx-enforcement",
                Label = "Enable AppLocker Packaged App (AppX) Enforcement",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Enables AppLocker PackagedApp collection enforcement for MSIX/AppX Store-installed applications, blocking or auditing UWP apps that do not match configured publisher or package name rules.",
                Tags = ["applocker", "appx", "msix", "store-apps", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "AppLocker AppX enforcement active; unlisted Store apps blocked. Ensure allow rules cover required apps.",
                ApplyOps = [RegOp.SetDword(AppxKey, "EnforcementMode", 2)],
                RemoveOps = [RegOp.DeleteValue(AppxKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(AppxKey, "EnforcementMode", 2)],
            },
            new TweakDef
            {
                Id = "alockadv-enable-applocker-msi-enforcement",
                Label = "Enable AppLocker Windows Installer (MSI) Enforcement",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Enables AppLocker Windows Installer collection enforcement, blocking MSI and MSP installer execution that does not match publisher or path allow rules, preventing unauthorised software installation.",
                Tags = ["applocker", "msi", "installer", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "AppLocker MSI enforcement active; untrusted installers blocked. IT-managed MSIs must be allow-listed.",
                ApplyOps = [RegOp.SetDword(MsiKey, "EnforcementMode", 2)],
                RemoveOps = [RegOp.DeleteValue(MsiKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(MsiKey, "EnforcementMode", 2)],
            },
            new TweakDef
            {
                Id = "alockadv-enable-applocker-event-logging",
                Label = "Enable AppLocker Policy Enforcement Event Logging",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Configures the AppLocker event log to capture all rule enforcement (allowed, denied, audited) events in the Microsoft-Windows-AppLocker operational log for SOC and SIEM ingestion.",
                Tags = ["applocker", "event-log", "audit", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AppLocker enforcement events logged; all allow/deny decisions recorded in AppLocker operational log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableAppLockerEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAppLockerEventLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAppLockerEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "alockadv-block-override-by-user",
                Label = "Block Users from Overriding AppLocker Policy",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Prevents standard users from modifying AppLocker configuration via local policy, ensuring application control rules can only be changed via domain GPO or local administrator action.",
                Tags = ["applocker", "override", "standard-user", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AppLocker policy cannot be changed by standard users; changes require admin or domain GPO.",
                ApplyOps = [RegOp.SetDword(Key, "BlockUserPolicyOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUserPolicyOverride")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUserPolicyOverride", 1)],
            },
            new TweakDef
            {
                Id = "alockadv-allow-publisher-rules",
                Label = "Allow Publisher-Based Rules as Default AppLocker Allow Strategy",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Configures AppLocker to prefer publisher rules (signed certificate chains) over path rules, enabling software to be allowed based on an identified digital signature rather than a potentially spoofable file path.",
                Tags = ["applocker", "publisher-rules", "digital-signature", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Publisher-based allow rules preferred; signed software allowed by certificate chain rather than path.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultRuleStrategy", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultRuleStrategy")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultRuleStrategy", 2)],
            },
            new TweakDef
            {
                Id = "alockadv-disable-applocker-telemetry",
                Label = "Disable AppLocker Enforcement Telemetry to Microsoft",
                Category = "AppLocker Advanced Policy",
                Description =
                    "Prevents AppLocker from sending enforcement telemetry (blocked app names, hashes, publisher names) to Microsoft, protecting internal application inventory from cloud disclosure.",
                Tags = ["applocker", "telemetry", "privacy", "microsoft", "application-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AppLocker telemetry to Microsoft disabled; blocked app names and hashes not sent to cloud.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAppLockerTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppLockerTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppLockerTelemetry", 1)],
            },
        ];
}
