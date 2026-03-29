// RegiLattice.Core — Tweaks/ScriptBlockLoggingAdvancedPolicy.cs
// Advanced PowerShell script block logging, audit, AMSI, and obfuscation detection — Sprint 450.
// Category: "Script Block Logging Advanced" | Slug: sbloga
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ScriptBlockLoggingAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sbloga-enable-script-block-logging",
                Label = "Enable Script Block Logging (Windows PS)",
                Category = "Script Block Logging Advanced",
                Description =
                    "Enables PowerShell script block logging for Windows PowerShell 5.1 via the dedicated ScriptBlockLogging policy key, recording all executed script blocks to the PowerShell/Operational event log.",
                Tags = ["powershell", "script-block-logging", "audit", "siem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Full script block content logged; essential for threat hunting but increases log volume.",
                ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "sbloga-enable-invocation-header",
                Label = "Enable Script Block Invocation Header Logging",
                Category = "Script Block Logging Advanced",
                Description =
                    "Enables logging of the start and stop events for each function/script-block invocation, providing timestamped execution boundaries in the event log.",
                Tags = ["powershell", "script-block-logging", "invocation", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Invocation start/stop events logged; detailed execution trail generated.",
                ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockInvocationLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockInvocationLogging", 1)],
            },
            new TweakDef
            {
                Id = "sbloga-enable-transcription",
                Label = "Enable PowerShell Transcript Logging",
                Category = "Script Block Logging Advanced",
                Description =
                    "Enables PowerShell session transcription that saves a full text copy of every PowerShell session to a transcript file on disk, providing a human-readable audit trail.",
                Tags = ["powershell", "transcription", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Every PS session recorded to transcript file; disk space usage increases with PS activity.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableTranscripting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableTranscripting")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableTranscripting", 1)],
            },
            new TweakDef
            {
                Id = "sbloga-enable-invocation-header-transcript",
                Label = "Include Invocation Header in PS Transcripts",
                Category = "Script Block Logging Advanced",
                Description =
                    "Adds invocation header information (command name, arguments, timestamps, username, process info) to PowerShell transcript files.",
                Tags = ["powershell", "transcription", "header", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Transcript files include rich header context for each command.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableInvocationHeader", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableInvocationHeader")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableInvocationHeader", 1)],
            },
            new TweakDef
            {
                Id = "sbloga-set-output-directory",
                Label = "Set Centralised PowerShell Transcript Directory",
                Category = "Script Block Logging Advanced",
                Description =
                    "Sets the PowerShell transcript output directory to a centralised network share or admin-controlled path so all endpoint transcripts are collected in one location.",
                Tags = ["powershell", "transcription", "directory", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Transcripts written to admin-specified path; ensure path is writable and monitored.",
                ApplyOps = [RegOp.SetString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                RemoveOps = [RegOp.DeleteValue(Key2, "OutputDirectory")],
                DetectOps = [RegOp.CheckString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
            },
            new TweakDef
            {
                Id = "sbloga-log-encoded-commands",
                Label = "Log Encoded PowerShell Command Executions",
                Category = "Script Block Logging Advanced",
                Description =
                    "Enables script block logging specifically targeting Base64-encoded commands (-EncodedCommand), which are commonly used by malware to obfuscate payloads.",
                Tags = ["powershell", "encoded-commands", "obfuscation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Encoded command executions captured in logs; key detection for fileless attacks.",
                ApplyOps = [RegOp.SetDword(Key, "LogEncodedCommands", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogEncodedCommands")],
                DetectOps = [RegOp.CheckDword(Key, "LogEncodedCommands", 1)],
            },
            new TweakDef
            {
                Id = "sbloga-log-dynamic-code",
                Label = "Log Dynamically Generated PowerShell Code",
                Category = "Script Block Logging Advanced",
                Description =
                    "Enables logging of dynamically generated PowerShell code (e.g., from Invoke-Expression or Add-Type), capturing obfuscated payloads that are assembled at runtime.",
                Tags = ["powershell", "dynamic-code", "invoke-expression", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Dynamically generated PS code blocks captured; critical for detecting memory-only malware.",
                ApplyOps = [RegOp.SetDword(Key, "LogDynamicCode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogDynamicCode")],
                DetectOps = [RegOp.CheckDword(Key, "LogDynamicCode", 1)],
            },
            new TweakDef
            {
                Id = "sbloga-set-max-log-size",
                Label = "Set PowerShell Operational Log Max Size to 512 MB",
                Category = "Script Block Logging Advanced",
                Description =
                    "Increases the Microsoft-Windows-PowerShell/Operational event log maximum size to 512 MB to prevent log overwriting (circular buffer) during high-volume script block logging.",
                Tags = ["powershell", "event-log", "size", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PS operational log grows to 512 MB max; older entries retained longer before cycling.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                ],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize")],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                ],
            },
            new TweakDef
            {
                Id = "sbloga-retain-on-clear",
                Label = "Retain PowerShell Log Archive on Clear",
                Category = "Script Block Logging Advanced",
                Description =
                    "Configures the PowerShell operational event log to archive before clearing when the log becomes full, preventing permanent log loss during log maintenance.",
                Tags = ["powershell", "event-log", "archive", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PS log archived to .evtx file before clearing; no historical data lost.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0),
                ],
            },
            new TweakDef
            {
                Id = "sbloga-block-clear-eventlog",
                Label = "Block Standard Users from Clearing Event Logs",
                Category = "Script Block Logging Advanced",
                Description =
                    "Restricts the ability to clear the PowerShell and Windows event logs to administrators only, preventing attackers with standard user access from clearing their tracks.",
                Tags = ["powershell", "event-log", "clear", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only admins can clear event logs; standard users get access denied.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1)],
            },
        ];
}
