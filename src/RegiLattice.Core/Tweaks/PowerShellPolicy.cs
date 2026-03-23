// RegiLattice.Core — Tweaks/PowerShellPolicy.cs
// PowerShell security policy GPO settings: ScriptBlockLogging, ModuleLogging,
// Transcription, PS2 disable, protected event logging, execution policy (Sprint 137).
// Slug "pspolicy" — distinct from PowerShellTweaks.cs (slug "ps") which targets
// miscellaneous service/registry tweaks, NOT PowerShell Group Policy paths.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PowerShellPolicy
{
    private const string PsRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
    private const string ScriptBlockLogging = PsRoot + @"\ScriptBlockLogging";
    private const string ModuleLogging = PsRoot + @"\ModuleLogging";
    private const string Transcription = PsRoot + @"\Transcription";
    private const string ProtectedEventLogging = PsRoot + @"\ProtectedEventLogging";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pspolicy-script-block-logging",
            Label = "Enable PowerShell Script Block Logging",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enables PowerShell Script Block Logging (GPO) so that all script blocks "
                + "executed by PowerShell are written to the Operational event log "
                + "(Microsoft-Windows-PowerShell/Operational). Essential for threat hunting.",
            Tags = ["powershell", "logging", "script block", "security", "audit"],
            RegistryKeys = [ScriptBlockLogging],
            ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockLogging")],
            DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-script-invocation-logging",
            Label = "Enable PowerShell Script Invocation Logging",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enables logging of script-block invocation start/stop events in addition "
                + "to the raw script text. Adds invocation sequence to event IDs 4104/4105/4106. "
                + "EnableScriptBlockInvocationLogging=1.",
            Tags = ["powershell", "logging", "invocation logging", "security"],
            RegistryKeys = [ScriptBlockLogging],
            ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockInvocationLogging")],
            DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-module-logging",
            Label = "Enable PowerShell Module Logging",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enables Module Logging, which records pipeline execution details for "
                + "specified (or all) PowerShell modules to the Windows event log. "
                + "Helps detect malicious module imports. EnableModuleLogging=1.",
            Tags = ["powershell", "module logging", "security", "audit"],
            RegistryKeys = [ModuleLogging],
            ApplyOps = [RegOp.SetDword(ModuleLogging, "EnableModuleLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ModuleLogging, "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword(ModuleLogging, "EnableModuleLogging", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-transcription-on",
            Label = "Enable PowerShell Transcription",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Turns on PowerShell Transcription, writing a plain-text record of every "
                + "PowerShell session (all commands and output) to a configured output "
                + "directory. EnableTranscripting=1.",
            Tags = ["powershell", "transcription", "logging", "audit"],
            RegistryKeys = [Transcription],
            ApplyOps = [RegOp.SetDword(Transcription, "EnableTranscripting", 1)],
            RemoveOps = [RegOp.DeleteValue(Transcription, "EnableTranscripting")],
            DetectOps = [RegOp.CheckDword(Transcription, "EnableTranscripting", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-transcription-header",
            Label = "Include Invocation Header in PowerShell Transcripts",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Adds timestamps, username, and invocation details to each PowerShell "
                + "transcript header, making audit review significantly easier. "
                + "EnableInvocationHeader=1.",
            Tags = ["powershell", "transcription", "header", "audit"],
            RegistryKeys = [Transcription],
            ApplyOps = [RegOp.SetDword(Transcription, "EnableInvocationHeader", 1)],
            RemoveOps = [RegOp.DeleteValue(Transcription, "EnableInvocationHeader")],
            DetectOps = [RegOp.CheckDword(Transcription, "EnableInvocationHeader", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-transcription-output-path",
            Label = "Set PowerShell Transcript Output Directory",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets the PowerShell transcript output directory to "
                + @"%SYSTEMROOT%\Logs\PowerShell so transcripts are centralised and "
                + "survive user profile deletion. OutputDirectory (REG_SZ).",
            Tags = ["powershell", "transcription", "output path", "audit"],
            RegistryKeys = [Transcription],
            ApplyOps = [RegOp.SetString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
            RemoveOps = [RegOp.DeleteValue(Transcription, "OutputDirectory")],
            DetectOps = [RegOp.CheckString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
        },
        new TweakDef
        {
            Id = "pspolicy-disable-ps2-engine",
            Label = "Disable PowerShell 2.0 Engine",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets EnablePS2=0 via GPO to prevent PowerShell from falling back to the "
                + "legacy v2 engine, which lacks script block logging and constrained language "
                + "mode — a known AMSI/logging bypass vector.",
            Tags = ["powershell", "ps2", "downgrade attack", "security", "amsi"],
            RegistryKeys = [PsRoot],
            ApplyOps = [RegOp.SetDword(PsRoot, "EnablePS2", 0)],
            RemoveOps = [RegOp.DeleteValue(PsRoot, "EnablePS2")],
            DetectOps = [RegOp.CheckDword(PsRoot, "EnablePS2", 0)],
        },
        new TweakDef
        {
            Id = "pspolicy-protected-event-logging",
            Label = "Enable Protected Event Logging",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Enables Protected Event Logging, which encrypts sensitive event log data "
                + "(such as PowerShell credentials in transcripts) using a CMS certificate, "
                + "so only authorized readers can decrypt. EnableProtectedEventLogging=1.",
            Tags = ["powershell", "event log", "encryption", "protected logging", "security"],
            RegistryKeys = [ProtectedEventLogging],
            ApplyOps = [RegOp.SetDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ProtectedEventLogging, "EnableProtectedEventLogging")],
            DetectOps = [RegOp.CheckDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-enable-scripts",
            Label = "Enable PowerShell Script Execution (GPO)",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets EnableScripts=1 via GPO, activating the policy-controlled execution "
                + "policy. Must be enabled before ExecutionPolicy can be enforced via "
                + "Group Policy. Set together with pspolicy-require-signed-scripts.",
            Tags = ["powershell", "execution policy", "scripts", "gpo"],
            RegistryKeys = [PsRoot],
            ApplyOps = [RegOp.SetDword(PsRoot, "EnableScripts", 1)],
            RemoveOps = [RegOp.DeleteValue(PsRoot, "EnableScripts")],
            DetectOps = [RegOp.CheckDword(PsRoot, "EnableScripts", 1)],
        },
        new TweakDef
        {
            Id = "pspolicy-require-signed-scripts",
            Label = "Require Signed PowerShell Scripts (AllSigned)",
            Category = "PowerShell Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Sets ExecutionPolicy=AllSigned via GPO, requiring that all scripts and "
                + "configuration files — including local scripts — be signed by a trusted "
                + "publisher. Strongest policy-level execution restriction.",
            Tags = ["powershell", "execution policy", "signed", "allsigned", "security"],
            RegistryKeys = [PsRoot],
            ApplyOps = [RegOp.SetString(PsRoot, "ExecutionPolicy", "AllSigned")],
            RemoveOps = [RegOp.DeleteValue(PsRoot, "ExecutionPolicy")],
            DetectOps = [RegOp.CheckString(PsRoot, "ExecutionPolicy", "AllSigned")],
        },
    ];
}
