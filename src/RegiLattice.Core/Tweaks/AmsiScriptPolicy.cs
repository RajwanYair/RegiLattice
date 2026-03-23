#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// AMSI & Script Block Policy — antimalware scan interface enforcement and PowerShell
// script block / module / transcription logging.
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine
internal static class AmsiScriptPolicy
{
    private const string ScriptBlockLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging";
    private const string ModuleLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging";
    private const string Transcription = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription";
    private const string PshPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
    private const string WdEngine = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine";
    private const string NtscriptPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WScript";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "amsi-enable-scriptblock-logging",
            Label = "AMSI: Enable PowerShell Script Block Logging",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [ScriptBlockLog],
            Tags = ["amsi", "powershell", "logging", "script-block", "security", "forensics"],
            Description =
                "Sets EnableScriptBlockLogging=1 in ScriptBlockLogging policy. Records all PowerShell script "
                + "block executions to Windows Event Log (Event ID 4104). "
                + "Essential for forensic analysis and detecting obfuscated malicious scripts. Default: disabled.",
            ApplyOps = [RegOp.SetDword(ScriptBlockLog, "EnableScriptBlockLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptBlockLog, "EnableScriptBlockLogging")],
            DetectOps = [RegOp.CheckDword(ScriptBlockLog, "EnableScriptBlockLogging", 1)],
        },
        new TweakDef
        {
            Id = "amsi-enable-scriptblock-invocation-logging",
            Label = "AMSI: Enable PowerShell Script Block Invocation Logging",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [ScriptBlockLog],
            Tags = ["amsi", "powershell", "logging", "invocation", "security", "forensics"],
            Description =
                "Sets EnableScriptBlockInvocationLogging=1. Logs the start and stop of every script block "
                + "invocation in addition to content logging. Captures when blocks are entered/exited. "
                + "Default: disabled. Recommended alongside script block content logging.",
            ApplyOps = [RegOp.SetDword(ScriptBlockLog, "EnableScriptBlockInvocationLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptBlockLog, "EnableScriptBlockInvocationLogging")],
            DetectOps = [RegOp.CheckDword(ScriptBlockLog, "EnableScriptBlockInvocationLogging", 1)],
        },
        new TweakDef
        {
            Id = "amsi-enable-module-logging",
            Label = "AMSI: Enable PowerShell Module Logging",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [ModuleLog],
            Tags = ["amsi", "powershell", "logging", "module", "security", "compliance"],
            Description =
                "Sets EnableModuleLogging=1 in ModuleLogging policy. Logs the complete output of pipeline "
                + "executions for all PowerShell modules. Enables auditing of all PS commands invoked. "
                + "Default: disabled. Generates high log volume but full command visibility.",
            ApplyOps = [RegOp.SetDword(ModuleLog, "EnableModuleLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ModuleLog, "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword(ModuleLog, "EnableModuleLogging", 1)],
        },
        new TweakDef
        {
            Id = "amsi-enable-transcription",
            Label = "AMSI: Enable PowerShell Transcription Logging",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Transcription],
            Tags = ["amsi", "powershell", "transcription", "logging", "audit", "security"],
            Description =
                "Sets EnableTranscripting=1 in Transcription policy. Writes all PowerShell input and output "
                + "to a text transcript file in a centrally configured directory. "
                + "Default: disabled. Essential for SIEM forwarding and post-incident analysis.",
            ApplyOps = [RegOp.SetDword(Transcription, "EnableTranscripting", 1)],
            RemoveOps = [RegOp.DeleteValue(Transcription, "EnableTranscripting")],
            DetectOps = [RegOp.CheckDword(Transcription, "EnableTranscripting", 1)],
        },
        new TweakDef
        {
            Id = "amsi-transcription-include-invocation-header",
            Label = "AMSI: Include Invocation Header in PowerShell Transcripts",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Transcription],
            Tags = ["amsi", "powershell", "transcription", "header", "forensics"],
            Description =
                "Sets EnableInvocationHeader=1 in Transcription policy. Adds timestamp and command path "
                + "to each entry in the PowerShell transcript. Makes forensic timeline reconstruction easier. "
                + "Default: no header. Recommended with transcription logging enabled.",
            ApplyOps = [RegOp.SetDword(Transcription, "EnableInvocationHeader", 1)],
            RemoveOps = [RegOp.DeleteValue(Transcription, "EnableInvocationHeader")],
            DetectOps = [RegOp.CheckDword(Transcription, "EnableInvocationHeader", 1)],
        },
        new TweakDef
        {
            Id = "amsi-enforce-constrained-language",
            Label = "AMSI: Enforce PowerShell Constrained Language Mode",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PshPolicy],
            Tags = ["amsi", "powershell", "constrained-language", "policy", "security"],
            Description =
                "Sets ConstrainedLanguageMode=1 in PowerShell policy. Restricts PS to safe language "
                + "features: no COM objects, no .NET type access, no reflection. Limits script attack surface. "
                + "Default: Full Language Mode. Recommended on machines with WDAC or AppLocker.",
            ApplyOps = [RegOp.SetDword(PshPolicy, "ConstrainedLanguageMode", 1)],
            RemoveOps = [RegOp.DeleteValue(PshPolicy, "ConstrainedLanguageMode")],
            DetectOps = [RegOp.CheckDword(PshPolicy, "ConstrainedLanguageMode", 1)],
        },
        new TweakDef
        {
            Id = "amsi-disable-wscript",
            Label = "AMSI: Disable Windows Script Host (WScript / CScript)",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [NtscriptPolicy],
            Tags = ["amsi", "wscript", "cscript", "vbscript", "jscript", "security", "lockdown"],
            Description =
                "Sets Enabled=0 in Windows Script Host policy. Disables wscript.exe and cscript.exe "
                + "for all users. Prevents execution of .vbs, .js, .wsf, and .wsh scripts. "
                + "Default: WSH enabled. Recommended unless legitimate WSH scripts are deployed.",
            ApplyOps = [RegOp.SetDword(NtscriptPolicy, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(NtscriptPolicy, "Enabled")],
            DetectOps = [RegOp.CheckDword(NtscriptPolicy, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "amsi-enable-cloud-protection-high",
            Label = "AMSI: Set Defender Cloud Protection to High Block Level",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WdEngine],
            Tags = ["amsi", "defender", "cloud-protection", "block-level", "security"],
            Description =
                "Sets MpCloudBlockLevel=2 in MpEngine policy. Sets Defender cloud-delivered protection "
                + "to 'High' block level. Higher values (2=High, 4=High+ Zero Tolerance) block more "
                + "aggressively. Default: 0 (Default). Recommended: 2 for balance.",
            ApplyOps = [RegOp.SetDword(WdEngine, "MpCloudBlockLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(WdEngine, "MpCloudBlockLevel")],
            DetectOps = [RegOp.CheckDword(WdEngine, "MpCloudBlockLevel", 2)],
        },
        new TweakDef
        {
            Id = "amsi-cloud-protection-timeout-extended",
            Label = "AMSI: Extend Defender Cloud Protection Scan Timeout to 50 Seconds",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WdEngine],
            Tags = ["amsi", "defender", "cloud-protection", "timeout", "security"],
            Description =
                "Sets MpBafsExtendedTimeout=50 in MpEngine policy. Allows Defender to wait up to 50 extra "
                + "seconds for cloud analysis before releasing a suspicious file. "
                + "Default: 10 seconds. Higher timeout improves detection of evasive malware.",
            ApplyOps = [RegOp.SetDword(WdEngine, "MpBafsExtendedTimeout", 50)],
            RemoveOps = [RegOp.DeleteValue(WdEngine, "MpBafsExtendedTimeout")],
            DetectOps = [RegOp.CheckDword(WdEngine, "MpBafsExtendedTimeout", 50)],
        },
        new TweakDef
        {
            Id = "amsi-disable-psh-v2",
            Label = "AMSI: Disable PowerShell 2.0 Engine (No AMSI Bypass Vector)",
            Category = "AMSI & Script Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PshPolicy],
            Tags = ["amsi", "powershell", "v2", "downgrade", "bypass", "security"],
            Description =
                "Sets EnableV2=0 in PowerShell policy. Prevents launching PowerShell with '-Version 2' "
                + "which bypasses AMSI, script block logging, and Constrained Language Mode. "
                + "Default: v2 engine can be invoked. Closing this bypass is a security requirement.",
            ApplyOps = [RegOp.SetDword(PshPolicy, "EnableV2", 0)],
            RemoveOps = [RegOp.DeleteValue(PshPolicy, "EnableV2")],
            DetectOps = [RegOp.CheckDword(PshPolicy, "EnableV2", 0)],
        },
    ];
}
