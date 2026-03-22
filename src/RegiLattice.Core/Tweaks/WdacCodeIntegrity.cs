// RegiLattice.Core — Tweaks/WdacCodeIntegrity.cs
// Individual Windows Defender Attack Surface Reduction (ASR) rule tweaks (Sprint 106).
// Slug: "wdac" — each tweak enables one specific ASR rule GUID in block mode.
// Distinct from AppLockerWdac.cs (application control policies) and Defender.cs (global ASR enable).
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules
// Values are GUID strings as DWORD names; value 1 = block, 2 = audit, 0 = disabled.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WdacCodeIntegrity
{
    private const string AsrRules =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wdac-asr-block-office-child",
            Label = "ASR: Block Office Applications from Creating Child Processes",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "office", "child-process", "security", "defender"],
            Description =
                "Enables ASR rule D4F940AB-401B-4EFC-AADC-AD5F3C50688A in block mode. "
                + "Prevents Microsoft Office applications (Word, Excel, PowerPoint, Outlook) from spawning "
                + "child processes such as cmd.exe, powershell.exe, or wscript.exe. "
                + "Blocks macro-based malware delivery that uses Office as a launch pad.",
            ApplyOps = [RegOp.SetDword(AsrRules, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-office-injection",
            Label = "ASR: Block Office Applications from Injecting Code into Other Processes",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "office", "code-injection", "security", "defender"],
            Description =
                "Enables ASR rule 75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84 in block mode. "
                + "Blocks Office applications from injecting shellcode or DLLs into other processes. "
                + "Stops process hollowing and DLL injection techniques used by macro malware to evade detection.",
            ApplyOps = [RegOp.SetDword(AsrRules, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-obfuscated-scripts",
            Label = "ASR: Block Execution of Obfuscated Scripts",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Tags = ["wdac", "asr", "obfuscation", "script", "powershell", "security"],
            Description =
                "Enables ASR rule 5BEB7EFE-FD9A-4556-801D-275E5FFC04CC in block mode. "
                + "Blocks execution of script files that appear obfuscated (high entropy, character substitution, "
                + "or encoded content). Effective against fileless malware delivered via PowerShell or VBScript "
                + "obfuscation. May occasionally trigger on legitimate heavily encoded scripts.",
            SideEffects = "Legitimate heavily obfuscated scripts may be blocked. Audit mode (value=2) first if unsure.",
            ApplyOps = [RegOp.SetDword(AsrRules, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-lsass-dump",
            Label = "ASR: Block Credential Stealing from LSASS",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "lsass", "credentials", "mimikatz", "security"],
            Description =
                "Enables ASR rule 9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2 in block mode. "
                + "Blocks attempts to extract credential hashes from lsass.exe memory — "
                + "the technique used by Mimikatz, ProcDump, and similar tools. "
                + "Complements RunAsPPL by blocking the dump attempt at the process context level.",
            ApplyOps = [RegOp.SetDword(AsrRules, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-ransomware",
            Label = "ASR: Advanced Protection Against Ransomware",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            Tags = ["wdac", "asr", "ransomware", "protection", "security"],
            Description =
                "Enables ASR rule C1DB55AB-C21A-4637-BB3F-A12568109D35 in block mode. "
                + "Engages advanced heuristics to detect and block ransomware-like behaviour: mass file encryption, "
                + "shadow copy deletion (vssadmin.exe), and low-level file I/O patterns matching known ransomware. "
                + "May produce false positives on backup and compression tools; test in audit mode first.",
            SideEffects = "Backup software and file archivers may be incorrectly flagged. Test in audit mode (value=2) first.",
            ApplyOps = [RegOp.SetDword(AsrRules, "C1DB55AB-C21A-4637-BB3F-A12568109D35", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "C1DB55AB-C21A-4637-BB3F-A12568109D35", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "C1DB55AB-C21A-4637-BB3F-A12568109D35", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-email-executable",
            Label = "ASR: Block Executable Content from Email Client and Webmail",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "email", "phishing", "malware", "security"],
            Description =
                "Enables ASR rule BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550 in block mode. "
                + "Blocks execution of executable files (.exe, .dll, .ps1, .vbs, .js, .bat) that arrive as "
                + "email attachments or are downloaded from webmail clients. "
                + "Closes one of the most common ransomware and phishing entry vectors (malicious email attachments).",
            ApplyOps = [RegOp.SetDword(AsrRules, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-wmi-persistence",
            Label = "ASR: Block WMI Event Subscription Persistence",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "wmi", "persistence", "apt", "security"],
            Description =
                "Enables ASR rule E6DB77E5-3DF2-4CF1-B95A-636979351E5B in block mode. "
                + "Blocks malware from creating permanent WMI event subscriptions that survive reboots. "
                + "WMI subscriptions are a widely-used Advanced Persistent Threat (APT) persistence mechanism "
                + "that allows code to run automatically when system events occur.",
            ApplyOps = [RegOp.SetDword(AsrRules, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-psexec-wmi",
            Label = "ASR: Block Process Creations from PSExec and WMI Commands",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 3,
            Tags = ["wdac", "asr", "psexec", "wmi", "lateral-movement", "security"],
            SideEffects = "Blocks legitimate IT operations using PSExec or WMI remoting for remote process creation.",
            Description =
                "Enables ASR rule D1E49AAC-8F56-4280-B9BA-993A6D77406C in block mode. "
                + "Stops process creation via PSExec and WMI commands — the two most common tools attackers use "
                + "for lateral movement after initial compromise. Disabling this rule is required if your "
                + "organisation uses PSExec/WMI for legitimate remote administration.",
            ApplyOps = [RegOp.SetDword(AsrRules, "D1E49AAC-8F56-4280-B9BA-993A6D77406C", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "D1E49AAC-8F56-4280-B9BA-993A6D77406C", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "D1E49AAC-8F56-4280-B9BA-993A6D77406C", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-usb-untrusted",
            Label = "ASR: Block Untrusted and Unsigned Processes from USB",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "usb", "removable", "unsigned", "security"],
            Description =
                "Enables ASR rule B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4 in block mode. "
                + "Blocks unsigned or untrusted executables launched from USB/removable drives. "
                + "Prevents BadUSB-style attacks and malware that auto-runs from inserted removable media.",
            ApplyOps = [RegOp.SetDword(AsrRules, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 1)],
        },
        new TweakDef
        {
            Id = "wdac-asr-block-adobe-child",
            Label = "ASR: Block Adobe Reader from Creating Child Processes",
            Category = "WDAC & Code Integrity",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdac", "asr", "adobe", "pdf", "child-process", "security"],
            Description =
                "Enables ASR rule 7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C in block mode. "
                + "Prevents Adobe Acrobat and Adobe Reader from spawning child processes. "
                + "Blocks PDF-based malware delivery using embedded scripts or exploit code that attempts to "
                + "launch command shells or download secondary payloads through the PDF reader.",
            ApplyOps = [RegOp.SetDword(AsrRules, "7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C", 1)],
            RemoveOps = [RegOp.SetDword(AsrRules, "7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C", 0)],
            DetectOps = [RegOp.CheckDword(AsrRules, "7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C", 1)],
        },
    ];
}
