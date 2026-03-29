// RegiLattice.Core — Tweaks/Ps7ExecutionModePolicy.cs
// PowerShell 7 (pwsh) constrained language, execution policy, remote, and logging controls — Sprint 448.
// Category: "PS7 Execution Mode Policy" | Slug: ps7exec
// Registry: HKLM\SOFTWARE\Policies\Microsoft\PowerShellCore
//           HKLM\SOFTWARE\Policies\Microsoft\PowerShellCore\ScriptBlockLogging

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Ps7ExecutionModePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore\ScriptBlockLogging";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ps7exec-enable-constrained-language",
                Label = "Enable Constrained Language Mode in PowerShell 7",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Enables Constrained Language Mode (CLM) for PowerShell 7 (pwsh), restricting the .NET types and COM objects that scripts can use and mitigating fileless malware execution.",
                Tags = ["powershell", "ps7", "constrained-language", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "PS7 runs in Constrained Language Mode; complex scripts and .NET interop may break.",
                ApplyOps = [RegOp.SetDword(Key, "EnableConstrainedLanguageMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableConstrainedLanguageMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableConstrainedLanguageMode", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-set-allsigned-policy",
                Label = "Enforce AllSigned Execution Policy in PowerShell 7",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Sets the PowerShell 7 execution policy to AllSigned, requiring all scripts (including local scripts) to be digitally signed by a trusted publisher before execution.",
                Tags = ["powershell", "ps7", "execution-policy", "signing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Only code-signed PS7 scripts run; unsigned dev scripts require explicit bypass.",
                ApplyOps = [RegOp.SetDword(Key, "ExecutionPolicy", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "ExecutionPolicy", 2)],
            },
            new TweakDef
            {
                Id = "ps7exec-disable-remoting",
                Label = "Disable PowerShell 7 Remoting",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Disables PowerShell 7 remoting (WinRM/SSH transport) via policy, preventing pwsh from being used as a remote administration target.",
                Tags = ["powershell", "ps7", "remoting", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "PS7 remoting disabled; WinRM/SSH-based remote pwsh sessions blocked.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRemoting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRemoting", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-disable-implicit-remoting",
                Label = "Disable PS7 Implicit Remoting Module Import",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Disables implicit remoting module imports in PowerShell 7, preventing a script from automatically importing and executing remote commands from untrusted sources.",
                Tags = ["powershell", "ps7", "implicit-remoting", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Implicit remote module imports blocked; remote command invocation requires explicit setup.",
                ApplyOps = [RegOp.SetDword(Key, "DisableImplicitRemoting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableImplicitRemoting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableImplicitRemoting", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-require-signed-modules",
                Label = "Require Signed Module Manifests in PowerShell 7",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Requires all PowerShell 7 module manifests (.psd1) to be signed by a trusted publisher before the module can be loaded, blocking unsigned third-party modules.",
                Tags = ["powershell", "ps7", "modules", "signing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Unsigned PS7 modules cannot load; all modules must have trusted code signatures.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSignedModuleManifests", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedModuleManifests")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignedModuleManifests", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-block-ps-gallery",
                Label = "Block PowerShell Gallery Repository in PS7",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Disables access to the default PowerShell Gallery online repository in PowerShell 7, forcing module and script installation through an approved internal repository.",
                Tags = ["powershell", "ps7", "gallery", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PSGallery blocked; Install-Module will not reach the public gallery.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPSGallery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPSGallery")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPSGallery", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-enable-script-block-logging",
                Label = "Enable Script Block Logging in PowerShell 7",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Enables script block logging in PowerShell 7 to record all script blocks executed to the event log (Microsoft-Windows-PowerShell/Operational), supporting forensic analysis.",
                Tags = ["powershell", "ps7", "script-block-logging", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All executed PS7 script blocks logged; event log volume may increase significantly.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-enable-invocation-logging",
                Label = "Enable Script Block Invocation Logging in PS7",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Enables verbose script block invocation logging in PowerShell 7, capturing start and stop events for each script block execution for detailed forensic trails.",
                Tags = ["powershell", "ps7", "invocation-logging", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Verbose invocation events logged; high-traffic PS7 hosts will generate significant log volume.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockInvocationLogging")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-disable-telemetry",
                Label = "Disable PowerShell 7 Telemetry",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Disables the PowerShell 7 telemetry feature that sends usage statistics (command names, error categories, OS info) to Microsoft via opt-out environment variable enforcement at policy level.",
                Tags = ["powershell", "ps7", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PS7 telemetry fully disabled at policy level; no usage data sent.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "ps7exec-disable-update-notif",
                Label = "Disable PowerShell 7 Update Notifications",
                Category = "PS7 Execution Mode Policy",
                Description =
                    "Suppresses in-session PowerShell 7 update available notifications that prompt users to download newer versions, deferring updates to a managed patching process.",
                Tags = ["powershell", "ps7", "update", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "PS7 update banners not shown; version management via package manager.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUpdateNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUpdateNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUpdateNotifications", 1)],
            },
        ];
}
