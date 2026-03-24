// RegiLattice.Core — Tweaks/AppCompatibilityPolicy.cs
// Application Compatibility GPO controls — Sprint 220.
// Controls Program Compatibility Assistant, telemetry collection, and SDB shims.
// Category: "App Compatibility Policy" | Slug: appcompat
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ApplicationCompatibility

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppCompatibilityPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ApplicationCompatibility";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "appcompat-disable-pca",
                Label = "Disable Program Compatibility Assistant",
                Category = "App Compatibility Policy",
                Description =
                    "Disables the Program Compatibility Assistant (PCA) that monitors application launches and prompts users to run programs in a compatibility mode when failure is detected. PCA interactions can generate telemetry events and prompt users into changing application settings. Default: PCA enabled. Recommended: 1 on locked-down managed desktops.",
                Tags = ["app-compat", "pca", "assistant", "compatibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PCA prompts are suppressed; compatibility dialogs after application crashes or installation failures are hidden.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePCA", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePCA")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePCA", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-disable-engine",
                Label = "Disable Application Compatibility Engine",
                Category = "App Compatibility Policy",
                Description =
                    "Turns off the Windows application compatibility shim engine. Shims intercept Win32 API calls and transparently modify behaviour for legacy applications. Disabling the engine prevents any shim from being applied and removes the attack surface of the shim infrastructure. Caution: may break some legacy applications. Default: engine enabled. Recommended: 1 where all deployed apps are tested on current Windows.",
                Tags = ["app-compat", "shim", "engine", "legacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Compatibility shims are disabled; legacy application behaviour changes may surface. Test before deployment.",
                ApplyOps = [RegOp.SetDword(Key, "DisableEngine", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEngine")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEngine", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-disable-removal-program",
                Label = "Disable Application Compatibility Removal Program",
                Category = "App Compatibility Policy",
                Description =
                    "Suppresses the prompt that appears when a program known to be incompatible with the current version of Windows is detected. The prompt normally says 'This program might not have installed correctly' and can lead to unintended re-installation attempts. Default: prompt enabled. Recommended: 1 on managed fleets.",
                Tags = ["app-compat", "removal", "prompt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Post-install compatibility prompts are suppressed; users are not prompted to reinstall known incompatible programs.",
                ApplyOps = [RegOp.SetDword(Key, "DisableInventory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInventory")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInventory", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-disable-sdb-lookup-online",
                Label = "Disable Online SDB Look-up for App Compatibility",
                Category = "App Compatibility Policy",
                Description =
                    "Prevents Windows from querying Microsoft's online SDB (Shim Data Base) to check whether a running application has a known compatibility fix that should be applied. Reduces outbound network calls and removes a subtle data channel where app hashes are sent. Default: online SDB lookup enabled. Recommended: 1.",
                Tags = ["app-compat", "sdb", "shim", "online-lookup", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows does not query Microsoft's online shim database; no app fingerprints are sent to Microsoft.",
                ApplyOps = [RegOp.SetDword(Key, "DisableProgramCompatibilityWizard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableProgramCompatibilityWizard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableProgramCompatibilityWizard", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-disable-telemetry",
                Label = "Disable Application Compatibility Telemetry Upload",
                Category = "App Compatibility Policy",
                Description =
                    "Blocks Windows from uploading application compatibility telemetry events (crash data, failed launch events, installer outcomes) to Microsoft Watson servers. Reduces the PCA telemetry data channel. Default: telemetry uploaded. Recommended: 1.",
                Tags = ["app-compat", "telemetry", "privacy", "watson", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Application compatibility telemetry is not uploaded to Microsoft; no app execution data is sent.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUAR", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUAR")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUAR", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-allow-only-approved-shims",
                Label = "Allow Only IT-Approved Compatibility Shims",
                Category = "App Compatibility Policy",
                Description =
                    "Restricts SDB shim loading so that only shims from administrator-supplied SDB files can be applied. Prevents attackers from installing malicious custom shims (a known persistence technique) by blocking user-context SDB installs. Default: any SDB files can be installed. Recommended: 1.",
                Tags = ["app-compat", "shim", "sdb", "security", "persistence", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only administrator-installed SDB shims are applied; user-context malicious shim installs are blocked.",
                ApplyOps = [RegOp.SetDword(Key, "AllowUserShims", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUserShims")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUserShims", 0)],
            },
            new TweakDef
            {
                Id = "appcompat-block-sdb-user-install",
                Label = "Block Users from Installing SDB Files",
                Category = "App Compatibility Policy",
                Description =
                    "Prevents standard users from registering application compatibility database (.sdb) files in the registry. SDB-based persistence (Shim-Based Patch Injection) is a known attack technique. Restricting SDB installs to administrators significantly reduces this attack vector. Default: no restriction. Recommended: 1.",
                Tags = ["app-compat", "sdb", "persistence", "user-restriction", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only admins can register SDB files; non-admin SDB shim persistence technique is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUserSDBInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserSDBInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserSDBInstall", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-disable-compatibility-chooser-ui",
                Label = "Disable Compatibility Chooser UI in Right-Click Menu",
                Category = "App Compatibility Policy",
                Description =
                    "Removes the 'Troubleshoot Compatibility' option from the Explorer right-click context menu for executable files. Prevents users from launching the Program Compatibility Troubleshooter which could change per-user compatibility settings. Default: option shown. Recommended: 1 on managed desktops.",
                Tags = ["app-compat", "context-menu", "ui", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "'Troubleshoot Compatibility' is removed from Explorer context menus; compatibility settings cannot be changed by users.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCompatChooserUI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompatChooserUI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompatChooserUI", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-log-shim-events",
                Label = "Log Application Compatibility Shim Events",
                Category = "App Compatibility Policy",
                Description =
                    "Enables logging of shim application events (which SDB was applied, which process, and which API was intercepted) to the Application Compatibility event log channel. Provides forensic visibility into shim activity for threat hunting. Default: shim events not logged. Recommended: 1.",
                Tags = ["app-compat", "shim", "audit", "logging", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Shim loading events are written to the event log; suspicious shim-based persistence is detectable.",
                ApplyOps = [RegOp.SetDword(Key, "EnableCompatibilityLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCompatibilityLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCompatibilityLogging", 1)],
            },
            new TweakDef
            {
                Id = "appcompat-disable-switches-per-process",
                Label = "Disable Per-Process Compatibility Settings Override",
                Category = "App Compatibility Policy",
                Description =
                    "Prevents per-user compatibility settings stored in HKCU from overriding machine-wide app compatibility configuration. Ensures that even if a user manually sets a compatibility mode for an application, the system policy takes precedence. Default: per-user HKCU overrides allowed. Recommended: 1 on managed desktops.",
                Tags = ["app-compat", "per-process", "override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "User-level per-exe compatibility settings cannot override machine-scope policy; HKCU compatibility flags are ignored.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSwitchesPerProcess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSwitchesPerProcess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSwitchesPerProcess", 1)],
            },
        ];
}
