// RegiLattice.Core — Tweaks/PolicyDefender.cs
// Microsoft Defender Antivirus, ASR attack surface reduction, Exploit Guard, Smart Screen, and controlled folder access policies
// Category: "Defender & Antivirus Policy"
// Consolidated from 20 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyDefender
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AmsiScriptPolicy.Data,
            .. _AsrAttackSurfacePolicy.Data,
            .. _ControlledFolderAccessPolicy.Data,
            .. _DefenderAdvanced.Data,
            .. _DefenderAntivirusAdvancedPolicy.Data,
            .. _DefenderExclusionsPolicy.Data,
            .. _DefenderExploitSystemPolicy.Data,
            .. _DefenderFirewallAdvancedPolicy.Data,
            .. _DefenderNetworkProtectionPolicy.Data,
            .. _DefenderSignatureUpdatePolicy.Data,
            .. _EarlyLaunchAMPolicy.Data,
            .. _EnhancedPhishingProtectionPolicy.Data,
            .. _ExploitGuardPolicy.Data,
            .. _FirewallLogPolicy.Data,
            .. _FirewallProfileHardeningPolicy.Data,
            .. _SmartControlBypassPolicy.Data,
            .. _SmartScreenAdvancedPolicy.Data,
            .. _SmartScreenPolicy.Data,
            .. _WebThreatDefensePolicy.Data,
            .. _WindowsFirewallPolicy.Data,
        ];

    // ── AmsiScriptPolicy ──
    private static class _AmsiScriptPolicy
    {    
        private const string ScriptBlockLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging";
        private const string ModuleLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging";
        private const string Transcription = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription";
        private const string PshPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string WdEngine = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine";
        private const string NtscriptPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WScript";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "amsi-enable-scriptblock-logging",
                Label = "AMSI: Enable PowerShell Script Block Logging",
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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
                Category = "Defender & Antivirus Policy",
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

    // ── AsrAttackSurfacePolicy ──
    private static class _AsrAttackSurfacePolicy
    {    
        private const string AsrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules";
    
        private const string AsrBaseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "asr-block-office-child-process",
                    Label = "ASR: Block Office Applications from Creating Child Processes",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule D4F940AB-401B-4EFC-AADC-AD5F3C50688A=1 (block). Prevents Microsoft Office applications (Word, Excel, PowerPoint) from spawning child processes. Malware campaigns frequently abuse Office macros to launch cmd.exe, PowerShell, or wscript.exe. Blocking child process creation from Office significantly raises the bar for macro-based malware without affecting normal Office functionality.",
                    Tags = ["asr", "defender", "office", "macro", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "High-impact ASR rule; disables macro-spawned processes. May break legacy VBA add-ins that shell out.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-credential-theft",
                    Label = "ASR: Block Credential Stealing from LSASS",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule 9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2=1 (block). Blocks tools such as Mimikatz, ProcDump, and Task Manager from reading credential material from lsass.exe memory. This is one of the highest-value ASR rules — lateral movement attacks almost universally require LSASS credential theft. LSA Protection (PPL) is the primary guard; this rule adds a second layer via Defender.",
                    Tags = ["asr", "defender", "lsass", "credential-theft", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Prevents LSASS memory read. May affect debuggers or monitoring agents that legitimately read LSASS.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-obfuscated-macros",
                    Label = "ASR: Block Execution of Potentially Obfuscated Scripts",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule 5BEB7EFE-FD9A-4556-801D-275E5FFC04CC=1 (block). Prevents execution of scripts that appear obfuscated — a strong indicator of malicious intent. Obfuscation is used by commodity malware, fileless attacks, and PowerShell stagers to evade static AV. This rule causes AMSI to perform deeper analysis before script execution. Most legitimate administrative scripts are not obfuscated.",
                    Tags = ["asr", "defender", "powershell", "obfuscation", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "May block minified or base64-encoded scripts. Audit existing scripts before enabling in production.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-win32-from-macros",
                    Label = "ASR: Block Win32 API Calls from Office Macros",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule 92E97FA1-2EDF-4476-BDD6-9DD0B4DDDC7B=1 (block). Prevents Office VBA macros from using Win32 API calls. Malware authors use VBA Declare statements to call kernel32.dll and ntdll.dll directly, bypassing COM-safe automation limits. Blocking Win32 API calls from macros eliminates a large class of shellcode-loading attacks while allowing safe Office automation APIs.",
                    Tags = ["asr", "defender", "vba", "win32-api", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Breaks VBA that uses Win32 Declare statements. Test all macros before deploying.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "92E97FA1-2EDF-4476-BDD6-9DD0B4DDDC7B", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "92E97FA1-2EDF-4476-BDD6-9DD0B4DDDC7B")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "92E97FA1-2EDF-4476-BDD6-9DD0B4DDDC7B", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-email-executable",
                    Label = "ASR: Block Executable Content from Email and Webmail",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550=1 (block). Prevents execution of .exe, .dll, .ps1, .js, .vbs content downloaded from email clients (Outlook, Thunderbird) or web browsers. Email-based delivery of malicious attachments is the #1 initial access vector. This rule creates a hard block on executable content received via email regardless of file extension spoofing.",
                    Tags = ["asr", "defender", "email", "attachment", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "High-value protection. May affect legitimate EXE attachments; use audit mode first in environments with frequent EXE email delivery.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-office-injection",
                    Label = "ASR: Block Office Applications from Injecting into Other Processes",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule 75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84=1 (block). Prevents Office apps from injecting code into other running processes via CreateRemoteThread, WriteProcessMemory, or similar techniques. Process injection is a core technique for privilege escalation and AV bypass. Blocking injection from Office eliminates an important pivot point for macro-based attacks.",
                    Tags = ["asr", "defender", "office", "process-injection", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prevents Office from injecting into other processes; may affect some Office add-ins.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-script-downloaded-payload",
                    Label = "ASR: Block JavaScript/VBScript from Launching Downloaded Executables",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule D3E037E1-3EB8-44C8-A917-57927947596D=1 (block). Prevents JS and VBS scripts executed via wscript.exe or cscript.exe from downloading and launching executable payloads from the internet. Droppers written in VBScript/JScript are extensively used in phishing attacks. Blocking launched executables from script interpreters closes this delivery vector without disabling scripts entirely.",
                    Tags = ["asr", "defender", "javascript", "vbscript", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks JS/VBS from launching downloaded EXEs. Legitimate scripts that download tools will be blocked.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "D3E037E1-3EB8-44C8-A917-57927947596D", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "D3E037E1-3EB8-44C8-A917-57927947596D")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "D3E037E1-3EB8-44C8-A917-57927947596D", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-wmi-persistence",
                    Label = "ASR: Block Persistence via WMI Event Subscription",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule E6DB77E5-3DF2-4CF1-B95A-636979351E5B=1 (block). Prevents malware from creating WMI event subscriptions that survive reboots (a fileless persistence technique). WMI subscriptions triggered on startup are used by APT groups and ransomware to maintain access across reboots. Blocking new WMI-based persistence while preserving existing administrative WMI use is a low-disruption, high-value hardening measure.",
                    Tags = ["asr", "defender", "wmi", "persistence", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Blocks WMI event subscription creation. Some monitoring tools use WMI subscriptions; verify before deploying.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 1)],
                },
                new TweakDef
                {
                    Id = "asr-block-untrusted-usb",
                    Label = "ASR: Block Untrusted and Unsigned Processes Running from USB",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ASR rule B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4=1 (block). Prevents unsigned or untrusted executables from running directly from USB drives. Physical access attacks (BadUSB, rubber duck, Raspberry Pi) rely on auto-running or user-launched executables from removable media. Requiring Authenticode signatures on USB-launched code raises the physical-access threat bar significantly.",
                    Tags = ["asr", "defender", "usb", "removable-media", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks unsigned executables from USB. Administrative tools on USB drives must be signed.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 1)],
                },
                new TweakDef
                {
                    Id = "asr-enable-audit-mode",
                    Label = "ASR: Set All Rules to Audit Mode (Non-Disruptive Monitoring)",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ExploitGuard_ASR_Rules=2 (audit mode). Forces all configured ASR rules to audit mode: events are logged without blocking execution. Use this setting when initially deploying ASR to identify false positives before switching to block mode. Windows Defender Security Center and Event Viewer (Event ID 1121/1122) will show which rules would have fired. Switch to value 1 (block) after completing the audit period.",
                    Tags = ["asr", "defender", "audit", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Audit-only; no blocking. Use as a staging step before enabling block mode.",
                    ApplyOps = [RegOp.SetDword(AsrBaseKey, "ExploitGuard_ASR_Rules", 2)],
                    RemoveOps = [RegOp.DeleteValue(AsrBaseKey, "ExploitGuard_ASR_Rules")],
                    DetectOps = [RegOp.CheckDword(AsrBaseKey, "ExploitGuard_ASR_Rules", 2)],
                },
            ];
    
    }

    // ── ControlledFolderAccessPolicy ──
    private static class _ControlledFolderAccessPolicy
    {    
        private const string CfaKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cfa-enable-block-mode",
                    Label = "Controlled Folder Access: Enable Block Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableControlledFolderAccess=1 (enabled, block mode). Activates Controlled Folder Access, which prevents untrusted applications from modifying files in protected folders such as Documents, Pictures, and Desktop. This is the most effective built-in ransomware protection available in Windows. Any untrusted process attempting to write to protected folders is blocked and an event is logged to the Security event channel.",
                    Tags = ["cfa", "ransomware", "defender", "folder-protection", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Strong ransomware protection. Common false positives: backup tools, photo editors, custom apps writing to Documents. Build an allow-list before deploying.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 1)],
                },
                new TweakDef
                {
                    Id = "cfa-enable-audit-mode",
                    Label = "Controlled Folder Access: Enable Audit Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableControlledFolderAccess=2 (audit mode). Logs all write attempts to protected folders without blocking them. Use audit mode to identify which applications need to be added to the CFA allow-list before enabling block mode. Events appear in Event Viewer under Microsoft-Windows-Windows Defender/Operational with Event ID 1124. Microsoft recommends a 2–4 week audit period before switching to block mode.",
                    Tags = ["cfa", "audit", "defender", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Audit-only; use before enabling block mode to prevent application breakage.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 2)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 2)],
                },
                new TweakDef
                {
                    Id = "cfa-protect-network-drives",
                    Label = "Controlled Folder Access: Extend Protection to Network Shares",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableNetworkProtection=1. Extends Controlled Folder Access to network drives and UNC paths mapped to the machine. Ransomware typically moves laterally to file servers shortly after executing on a workstation; protecting network-mapped drives prevents encrypted-file propagation to shared storage. Requires CFA to be in block or audit mode (EnableControlledFolderAccess ≠ 0) to take effect.",
                    Tags = ["cfa", "network", "ransomware", "network-drive", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Network drive protection may cause latency for legitimate writes. Test with mapped backup and file server shares.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableNetworkProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableNetworkProtection")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableNetworkProtection", 1)],
                },
                new TweakDef
                {
                    Id = "cfa-block-disk-modification",
                    Label = "Controlled Folder Access: Block Unauthorized Disk Sector Modifications",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableControlledFolderAccessForRawAccess=1. Prevents unauthorized applications from issuing raw disk write operations that bypass the filesystem layer. Some ransomware variants use direct disk sector writes (via CreateFile with physical drive paths) to overwrite the MBR or encrypt entire partition sectors without touching individual files. This policy blocks raw disk access from untrusted processes.",
                    Tags = ["cfa", "mbr", "raw-disk", "ransomware", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks raw disk writes from untrusted processes. Disk imaging and partition tools must be allow-listed.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccessForRawAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccessForRawAccess")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccessForRawAccess", 1)],
                },
                new TweakDef
                {
                    Id = "cfa-disable-notification",
                    Label = "Controlled Folder Access: Suppress Block Notifications to Users",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableNotifications=1 under CFA. Suppresses the toast notification that appears when CFA blocks an application. In corporate environments, end-user CFA block notifications can be confusing and generate spurious helpdesk tickets. Security events are always logged regardless of this setting. Suitable for managed environments where the SOC monitors event logs rather than relying on user reports.",
                    Tags = ["cfa", "notification", "defender", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Silent blocks. Ensure event log monitoring is in place so blocked events are not missed.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "DisableNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "DisableNotifications")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "DisableNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "cfa-protect-temp-folder",
                    Label = "Controlled Folder Access: Protect %TEMP% Folder",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ProtectedFoldersTempDir=1. Adds the user-specific %TEMP% directory to the CFA-protected folder list. Dropper malware commonly writes stage-2 payloads to %TEMP% before executing them. Protecting %TEMP% prevents untrusted processes from writing new executable content to this frequently-targeted location. Legitimate installers that extract to %TEMP% must be allow-listed before enabling this.",
                    Tags = ["cfa", "temp", "dropper", "defender", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 2,
                    ImpactNote = "High false positives: many installers write and execute from %TEMP%. Allow-list all installers before deploying.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "ProtectedFoldersTempDir", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "ProtectedFoldersTempDir")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "ProtectedFoldersTempDir", 1)],
                },
                new TweakDef
                {
                    Id = "cfa-protect-browser-data",
                    Label = "Controlled Folder Access: Protect Browser Profile Folders",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ProtectBrowserFolders=1. Adds browser profile directories (Chrome/Edge/Firefox user data) to the CFA protected folder list. Browser profile data contains saved passwords, cookies, and session tokens — high-value targets for infostealer malware. Preventing unauthorized writes to browser profile folders blocks infostealers from exfiltrating credential data without affecting normal browser operation.",
                    Tags = ["cfa", "browser", "infostealer", "cookies", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Protects browser credential stores. Backup tools that copy browser profiles may need allow-listing.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "ProtectBrowserFolders", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "ProtectBrowserFolders")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "ProtectBrowserFolders", 1)],
                },
                new TweakDef
                {
                    Id = "cfa-enable-block-mode-disk",
                    Label = "Controlled Folder Access: Enable Block Mode Including Disk Sectors",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableControlledFolderAccess=3 (block mode with disk sector protection). Combines standard CFA file-level protection with block mode for raw disk writes in a single policy value. Using value 3 is the strongest CFA configuration, protecting against both file-encrypting ransomware and MBR/boot-sector wipers in one policy. Equivalent to enabling EnableControlledFolderAccess=1 AND EnableControlledFolderAccessForRawAccess=1 separately.",
                    Tags = ["cfa", "ransomware", "mbr", "wiper", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 2,
                    ImpactNote = "Maximum protection but highest false positive risk. Only use after completing an audit period with value 2.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 3)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 3)],
                },
                new TweakDef
                {
                    Id = "cfa-audit-mode-disk",
                    Label = "Controlled Folder Access: Audit Mode Including Disk Sector Checks",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableControlledFolderAccess=4 (audit mode with disk sector monitoring). Logs both file-level and raw disk sector write attempts to protected locations without blocking them. Use value 4 when planning to deploy value 3 (block + disk) to pre-identify which applications perform raw disk writes. Events are logged to the Windows Defender/Operational channel with Event IDs 1124 (allowed) and 1125 (would-be blocked).",
                    Tags = ["cfa", "audit", "disk", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Audits disk sector writes in addition to file writes. No blocking; safe for production.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 4)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 4)],
                },
                new TweakDef
                {
                    Id = "cfa-enforce-allow-list-only",
                    Label = "Controlled Folder Access: Enforce Allow-List Only Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets OnlyEnforceAllowedApplicationsList=1. When set, only explicitly allow-listed applications (configured via separate CFA Allowed Applications policy) may write to protected folders. Without this flag, CFA maintains an internal safe-apps list based on signing and reputation; with it, only the IT-defined allow-list is trusted. Provides maximum enterprise control at the cost of requiring a maintained allow-list.",
                    Tags = ["cfa", "allow-list", "enterprise", "defender", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Requires a maintained application allow-list. All apps not on the list will be blocked, including signed apps.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "OnlyEnforceAllowedApplicationsList", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "OnlyEnforceAllowedApplicationsList")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "OnlyEnforceAllowedApplicationsList", 1)],
                },
            ];
    
    }

    // ── DefenderAdvanced ──
    private static class _DefenderAdvanced
    {    
        private const string DefRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender";
        private const string MpEngine = DefRoot + @"\MpEngine";
        private const string Spynet = DefRoot + @"\Spynet";
        private const string RealTime = DefRoot + @"\Real-Time Protection";
        private const string Scan = DefRoot + @"\Scan";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "defadv-cloud-block-level-high",
                Label = "Set Defender Cloud Block Level to High",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets MpCloudBlockLevel=4 (High), causing Defender to block more "
                    + "aggressively when cloud analysis is inconclusive. Values: "
                    + "0=Default, 2=Moderate, 4=High, 6=High+, 8=Zero tolerance.",
                Tags = ["defender", "cloud protection", "block level", "security"],
                RegistryKeys = [MpEngine],
                ApplyOps = [RegOp.SetDword(MpEngine, "MpCloudBlockLevel", 4)],
                RemoveOps = [RegOp.DeleteValue(MpEngine, "MpCloudBlockLevel")],
                DetectOps = [RegOp.CheckDword(MpEngine, "MpCloudBlockLevel", 4)],
            },
            new TweakDef
            {
                Id = "defadv-cloud-extended-timeout",
                Label = "Extend Defender Cloud-Check Timeout to 50 s",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets MpBafsExtendedTimeout=50 to allow 50 seconds (default: 10 s) for the "
                    + "cloud to analyse a suspicious file before executing it, improving detection "
                    + "rates for novel threats.",
                Tags = ["defender", "cloud protection", "timeout", "bafs"],
                RegistryKeys = [MpEngine],
                ApplyOps = [RegOp.SetDword(MpEngine, "MpBafsExtendedTimeout", 50)],
                RemoveOps = [RegOp.DeleteValue(MpEngine, "MpBafsExtendedTimeout")],
                DetectOps = [RegOp.CheckDword(MpEngine, "MpBafsExtendedTimeout", 50)],
            },
            new TweakDef
            {
                Id = "defadv-maps-advanced-membership",
                Label = "Enable MAPS Advanced Membership (Automatic Sample Reporting)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets SpynetReporting=2 (Advanced MAPS membership), sending additional "
                    + "information to Microsoft about potentially malicious software. Required "
                    + "for cloud protection to function fully. 0=Disabled, 1=Basic, 2=Advanced.",
                Tags = ["defender", "maps", "cloud", "spynet", "telemetry"],
                RegistryKeys = [Spynet],
                ApplyOps = [RegOp.SetDword(Spynet, "SpynetReporting", 2)],
                RemoveOps = [RegOp.DeleteValue(Spynet, "SpynetReporting")],
                DetectOps = [RegOp.CheckDword(Spynet, "SpynetReporting", 2)],
            },
            new TweakDef
            {
                Id = "defadv-auto-sample-submission",
                Label = "Enable Automatic Sample Submission (Safe Samples)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets SubmitSamplesConsent=1 to automatically send safe samples "
                    + "(safe file types) to Microsoft for analysis. 0=Always prompt, "
                    + "1=Auto safe samples, 2=Never, 3=Always automatic.",
                Tags = ["defender", "sample submission", "maps", "cloud"],
                RegistryKeys = [Spynet],
                ApplyOps = [RegOp.SetDword(Spynet, "SubmitSamplesConsent", 1)],
                RemoveOps = [RegOp.DeleteValue(Spynet, "SubmitSamplesConsent")],
                DetectOps = [RegOp.CheckDword(Spynet, "SubmitSamplesConsent", 1)],
            },
            new TweakDef
            {
                Id = "defadv-enable-behavior-monitoring",
                Label = "Enable Defender Behavior Monitoring",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Ensures behavior monitoring is enabled in Defender real-time protection "
                    + "by setting DisableBehaviorMonitoring=0. Policy-enforced default-on.",
                Tags = ["defender", "behaviour monitoring", "real-time protection", "security"],
                RegistryKeys = [RealTime],
                ApplyOps = [RegOp.SetDword(RealTime, "DisableBehaviorMonitoring", 0)],
                RemoveOps = [RegOp.DeleteValue(RealTime, "DisableBehaviorMonitoring")],
                DetectOps = [RegOp.CheckDword(RealTime, "DisableBehaviorMonitoring", 0)],
            },
            new TweakDef
            {
                Id = "defadv-enable-ioav-protection",
                Label = "Enable Defender On-Access (I/O) Scans",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Ensures I/O AV protection is enabled by policy (DisableIOAVProtection=0). "
                    + "Defender scans all downloaded files and attachments via real-time hooks.",
                Tags = ["defender", "on-access", "ioav", "real-time protection"],
                RegistryKeys = [RealTime],
                ApplyOps = [RegOp.SetDword(RealTime, "DisableIOAVProtection", 0)],
                RemoveOps = [RegOp.DeleteValue(RealTime, "DisableIOAVProtection")],
                DetectOps = [RegOp.CheckDword(RealTime, "DisableIOAVProtection", 0)],
            },
            new TweakDef
            {
                Id = "defadv-enable-script-scanning",
                Label = "Enable Defender Script Scanning",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enables real-time scanning of scripts (JS, VBS, PS1 etc.) via policy "
                    + "(DisableScriptScanning=0). Mitigates script-based malware delivering "
                    + "payloads through browser or Office exploits.",
                Tags = ["defender", "script scanning", "real-time protection", "security"],
                RegistryKeys = [RealTime],
                ApplyOps = [RegOp.SetDword(RealTime, "DisableScriptScanning", 0)],
                RemoveOps = [RegOp.DeleteValue(RealTime, "DisableScriptScanning")],
                DetectOps = [RegOp.CheckDword(RealTime, "DisableScriptScanning", 0)],
            },
            new TweakDef
            {
                Id = "defadv-scan-archives",
                Label = "Enable Defender Archive Scanning",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforces scanning of archive files (ZIP, RAR, 7z, CAB) during both "
                    + "on-access and quick/full scans by policy (DisableArchiveScanning=0).",
                Tags = ["defender", "archive scanning", "scan", "security"],
                RegistryKeys = [Scan],
                ApplyOps = [RegOp.SetDword(Scan, "DisableArchiveScanning", 0)],
                RemoveOps = [RegOp.DeleteValue(Scan, "DisableArchiveScanning")],
                DetectOps = [RegOp.CheckDword(Scan, "DisableArchiveScanning", 0)],
            },
            new TweakDef
            {
                Id = "defadv-scan-email",
                Label = "Enable Defender Email Body Scanning",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforces scanning of email message bodies and attachments (EML, MSG, PST) "
                    + "during scheduled scans by policy (DisableEmailScanning=0). Detects "
                    + "malicious macro documents delivered via email.",
                Tags = ["defender", "email scanning", "scan", "security"],
                RegistryKeys = [Scan],
                ApplyOps = [RegOp.SetDword(Scan, "DisableEmailScanning", 0)],
                RemoveOps = [RegOp.DeleteValue(Scan, "DisableEmailScanning")],
                DetectOps = [RegOp.CheckDword(Scan, "DisableEmailScanning", 0)],
            },
            new TweakDef
            {
                Id = "defadv-randomize-scan-time",
                Label = "Randomize Defender Scheduled Scan Start Time",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets RandomizeScheduleTaskTimes=1 so that Defender scheduled scan tasks "
                    + "start at a random offset (±30 minutes) around the configured time, "
                    + "spreading load across many machines in enterprise environments.",
                Tags = ["defender", "scheduled scan", "randomize", "performance"],
                RegistryKeys = [Scan],
                ApplyOps = [RegOp.SetDword(Scan, "RandomizeScheduleTaskTimes", 1)],
                RemoveOps = [RegOp.DeleteValue(Scan, "RandomizeScheduleTaskTimes")],
                DetectOps = [RegOp.CheckDword(Scan, "RandomizeScheduleTaskTimes", 1)],
            },
        ];
    
    }

    // ── DefenderAntivirusAdvancedPolicy ──
    private static class _DefenderAntivirusAdvancedPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender";
        private const string ScanKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan";
        private const string SpynetKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet";
        private const string QtnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Quarantine";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "avadv-disable-tamper-protection",
                    Label = "Prevent Standard Users from Disabling Tamper Protection",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets a policy requirement that Tamper Protection remains enabled, preventing standard users and non-authorised scripts from disabling Windows Defender via registry or settings, a common malware persistence technique.",
                    Tags = ["defender", "tamper-protection", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Tamper Protection enforced via policy; Defender cannot be disabled by users or scripts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTamperProtection", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTamperProtection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTamperProtection", 0)],
                },
                new TweakDef
                {
                    Id = "avadv-block-sample-submission-non-consent",
                    Label = "Block Automatic Sample Submission Without User Consent",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Configures Defender to always prompt before sending potentially sensitive file samples to Microsoft for cloud analysis, preventing automatic cloud submission of suspicious documents that may contain confidential data.",
                    Tags = ["defender", "sample-submission", "privacy", "cloud", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Sample submission requires user consent; suspicious files not silently sent to Microsoft.",
                    ApplyOps = [RegOp.SetDword(SpynetKey, "SubmitSamplesConsent", 2)],
                    RemoveOps = [RegOp.DeleteValue(SpynetKey, "SubmitSamplesConsent")],
                    DetectOps = [RegOp.CheckDword(SpynetKey, "SubmitSamplesConsent", 2)],
                },
                new TweakDef
                {
                    Id = "avadv-set-cloud-protection-level-high",
                    Label = "Set Defender Cloud Protection Level to High",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets Windows Defender's cloud-delivered protection level to High, enabling more aggressive cloud-based heuristic analysis and slightly longer scan timeouts to catch sophisticated polymorphic threats missed by signature-only scans.",
                    Tags = ["defender", "cloud-protection", "heuristics", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cloud protection level set to High; aggressive heuristics enabled for zero-day threat detection.",
                    ApplyOps = [RegOp.SetDword(SpynetKey, "MAPSReporting", 2)],
                    RemoveOps = [RegOp.DeleteValue(SpynetKey, "MAPSReporting")],
                    DetectOps = [RegOp.CheckDword(SpynetKey, "MAPSReporting", 2)],
                },
                new TweakDef
                {
                    Id = "avadv-set-scan-scheduled-quick-daily",
                    Label = "Schedule Daily Quick Scan at 02:00",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Configures Windows Defender to perform a daily quick scan at 02:00 AM (hour 2), ensuring endpoint malware is detected and cleared on a daily schedule without relying on user-initiated scans.",
                    Tags = ["defender", "scheduled-scan", "quick-scan", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Daily quick scan scheduled at 02:00 AM; ensures regular automated endpoint malware detection.",
                    ApplyOps = [RegOp.SetDword(ScanKey, "ScheduleQuickScanTime", 120)],
                    RemoveOps = [RegOp.DeleteValue(ScanKey, "ScheduleQuickScanTime")],
                    DetectOps = [RegOp.CheckDword(ScanKey, "ScheduleQuickScanTime", 120)],
                },
                new TweakDef
                {
                    Id = "avadv-enable-realtime-protection",
                    Label = "Enforce Real-Time Protection is Always On",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets a policy that ensures Windows Defender real-time protection monitoring is always active, preventing GPO or local policy from disabling file-system monitoring on covered endpoints.",
                    Tags = ["defender", "real-time-protection", "always-on", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Real-time protection enforced as always-on; cannot be disabled via local policy or settings.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableRealtimeMonitoring",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableRealtimeMonitoring"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableRealtimeMonitoring",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "avadv-block-ioav-disable",
                    Label = "Block Disabling On-Access Scan for Downloaded Files",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Prevents policy-level disabling of the Internet Origin/Anti-virus (IOAV) scan that checks files downloaded from the internet, ensuring that browser-downloaded executables are always scanned before execution.",
                    Tags = ["defender", "ioav", "download-scan", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "IOAV download scan enforced; all internet-downloaded files automatically scanned before execution.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableIOAVProtection",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableIOAVProtection"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableIOAVProtection",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "avadv-enable-scanning-mapped-drives",
                    Label = "Enable Scanning of Network Mapped Drive Files",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Configures Windows Defender to scan files on mapped network drives in addition to local files, protecting against malware distribution via shared network storage that may not have server-side scanning enabled.",
                    Tags = ["defender", "network-scan", "mapped-drives", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Mapped network drive scanning enabled; files on network shares scanned before access.",
                    ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanningMappedNetworkDrivesForFullScan", 0)],
                    RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanningMappedNetworkDrivesForFullScan")],
                    DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanningMappedNetworkDrivesForFullScan", 0)],
                },
                new TweakDef
                {
                    Id = "avadv-set-quarantine-purge-days-30",
                    Label = "Set Quarantine Auto-Purge to 30 Days",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Configures Windows Defender to automatically delete quarantined files after 30 days, preventing unbounded growth of the quarantine store while retaining files long enough for forensic analysis if needed.",
                    Tags = ["defender", "quarantine", "purge", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quarantine auto-purge set to 30 days; quarantined malware files deleted after 30-day retention period.",
                    ApplyOps = [RegOp.SetDword(QtnKey, "PurgeItemsAfterDelay", 30)],
                    RemoveOps = [RegOp.DeleteValue(QtnKey, "PurgeItemsAfterDelay")],
                    DetectOps = [RegOp.CheckDword(QtnKey, "PurgeItemsAfterDelay", 30)],
                },
                new TweakDef
                {
                    Id = "avadv-enable-behavior-monitoring",
                    Label = "Enforce Defender Behaviour Monitoring is Always On",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Ensures Windows Defender behavioural monitoring (which detects suspicious process activities and file access patterns in real-time) is enforced as always active via policy, providing protection against fileless malware.",
                    Tags = ["defender", "behaviour-monitoring", "fileless", "antivirus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Defender behaviour monitoring enforced; fileless and process-based malware detected in real-time.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableBehaviorMonitoring",
                            0
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableBehaviorMonitoring"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                            "DisableBehaviorMonitoring",
                            0
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "avadv-disable-av-ui-telemetry",
                    Label = "Disable Defender Antivirus UI Telemetry to Microsoft",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Prevents Windows Defender from sending UI interaction telemetry (which settings pages are visited, what scans are triggered) to Microsoft, reducing cloud data exposure while keeping all antivirus protection active.",
                    Tags = ["defender", "telemetry", "privacy", "ui", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Defender UI telemetry to Microsoft disabled; antivirus protection unaffected, usage data not sent.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMpTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMpTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMpTelemetry", 1)],
                },
            ];
    
    }

    // ── DefenderExclusionsPolicy ──
    private static class _DefenderExclusionsPolicy
    {    
        private const string ExclKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsDefender\Exclusions";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "defexclpol-block-local-exclusion-merge",
                    Label = "Block Local Admin Exclusion Merging",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableLocalAdminMerge=1 to prevent local administrators from adding their own Defender exclusions. Only exclusions defined through Group Policy are applied.",
                    Tags = ["defender", "exclusions", "admin", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local admin exclusions blocked; only GPO-defined exclusions are active. Hardens Defender.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "DisableLocalAdminMerge", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "DisableLocalAdminMerge")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "DisableLocalAdminMerge", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-block-user-exclusion-additions",
                    Label = "Block Standard User Exclusion Additions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets BlockUserExclusions=1 to prevent standard (non-admin) users from adding or modifying Windows Defender exclusions through the Windows Security app settings.",
                    Tags = ["defender", "exclusions", "users", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot add Defender exclusions; only admins (and GPO) can define them.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "BlockUserExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "BlockUserExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "BlockUserExclusions", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-restrict-path-exclusions",
                    Label = "Restrict Path-Based Exclusion Additions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RestrictPathExclusions=1 to limit the ability to add new path-based Defender exclusions. Prevents exclusions that could expose scan-critical directories to malware.",
                    Tags = ["defender", "exclusions", "path", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "New Defender path exclusions are restricted; existing GPO-defined path exclusions remain.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "RestrictPathExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictPathExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "RestrictPathExclusions", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-restrict-process-exclusions",
                    Label = "Restrict Process-Based Exclusion Additions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RestrictProcessExclusions=1 to prevent users from adding new process exclusions to Windows Defender. Only centrally managed process exclusions are permitted.",
                    Tags = ["defender", "exclusions", "process", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local process-based Defender exclusions blocked; reduces risk of malware self-exclusion.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "RestrictProcessExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictProcessExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "RestrictProcessExclusions", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-restrict-extension-exclusions",
                    Label = "Restrict File Extension Exclusion Additions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RestrictExtensionExclusions=1 to prevent users from adding file extension exclusions to Windows Defender. Extension exclusions can be abused to allow malicious file types to bypass scanning.",
                    Tags = ["defender", "exclusions", "extension", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local extension exclusions blocked; .exe/.bat/.ps1 cannot be locally exempted from scanning.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "RestrictExtensionExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictExtensionExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "RestrictExtensionExclusions", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-restrict-ip-exclusions",
                    Label = "Restrict IP Address Exclusion Additions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RestrictIpExclusions=1 to prevent users from exempting specific IP addresses from Windows Defender network inspection. Ensures complete network traffic scanning.",
                    Tags = ["defender", "exclusions", "ip", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Local IP address Defender exclusions blocked; all network traffic remains subject to inspection.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "RestrictIpExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictIpExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "RestrictIpExclusions", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-enable-exclusion-audit",
                    Label = "Enable Defender Exclusion Audit Logging",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableExclusionAudit=1 to log all Defender exclusion additions, modifications, and removals to the Windows Security event log for auditing and compliance.",
                    Tags = ["defender", "exclusions", "audit", "logging", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All exclusion changes written to Security event log; enables SOC monitoring of Defender config.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "EnableExclusionAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "EnableExclusionAudit")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "EnableExclusionAudit", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-require-admin-review",
                    Label = "Require Admin Review for All Exclusion Changes",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RequireAdminReview=1 to require administrator approval before any new Defender exclusion is applied, including those submitted through the Security Center UI.",
                    Tags = ["defender", "exclusions", "admin", "review", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Exclusion requests queued until an administrator approves them; prevents silent exclusion bypass.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "RequireAdminReview", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "RequireAdminReview")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "RequireAdminReview", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-block-temp-exclusions",
                    Label = "Block Temporary File Path Exclusions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets BlockTempExclusions=1 to prevent exclusions that target Temp, Windows\\Temp, or user-profile temp directories. Attackers commonly add temp folder exclusions to stage malware.",
                    Tags = ["defender", "exclusions", "temp", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Temp directory exclusions blocked; malware staging in temp folders remains scannable.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "BlockTempExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "BlockTempExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "BlockTempExclusions", 1)],
                },
                new TweakDef
                {
                    Id = "defexclpol-restrict-wildcard-exclusions",
                    Label = "Restrict Wildcard Path Exclusions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RestrictWildcardExclusions=1 to prevent wildcard (* or ?) characters in Defender exclusion paths. Wildcards can inadvertently exclude large portions of the file system.",
                    Tags = ["defender", "exclusions", "wildcard", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wildcard path exclusions blocked; only explicit exact-path exclusions are permitted.",
                    ApplyOps = [RegOp.SetDword(ExclKey, "RestrictWildcardExclusions", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictWildcardExclusions")],
                    DetectOps = [RegOp.CheckDword(ExclKey, "RestrictWildcardExclusions", 1)],
                },
            ];
    
    }

    // ── DefenderExploitSystemPolicy ──
    private static class _DefenderExploitSystemPolicy
    {    
        private const string ExploitKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exploit Guard\Exploit Protection";
    
        private const string MpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "defexploit-enable-system-dep",
                    Label = "Exploit Protection: Enable System-Wide Data Execution Prevention",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ExploitProtectionSettings DEP SystemSettings=ON. Forces Data Execution Prevention for all 32-bit and 64-bit processes system-wide. DEP marks executable memory pages non-writable and data pages non-executable, breaking classical stack and heap overflow exploitation chains. While most modern 64-bit processes benefit from hardware-enforced DEP automatically, this policy ensures consistent coverage for 32-bit processes and legacy code.",
                    Tags = ["exploit-protection", "dep", "nx", "overflow", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "DEP is widely compatible but may break extremely old 16-bit or poorly written 32-bit applications.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "EnableSystemDep", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "EnableSystemDep")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "EnableSystemDep", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-sehop",
                    Label = "Exploit Protection: Enable Structured Exception Handler Overwrite Protection",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableSEHOP=1. Activates Structured Exception Handler Overwrite Protection (SEHOP), which validates the SEH chain before executing a handler. SEH overwrites are used in return-oriented ROP chain construction on 32-bit Windows processes. SEHOP adds a canary check to the SEH chain that overwrites on attack destroy, causing a controlled crash instead of code execution. 64-bit SafeSEH removes the need, but 32-bit processes running on 64-bit Windows still benefit.",
                    Tags = ["exploit-protection", "sehop", "rop", "seh", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SEHOP is compatible with nearly all modern applications. Very rare incompatibilities with deeply nested SEH chains.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "EnableSEHOP", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "EnableSEHOP")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "EnableSEHOP", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-heap-termination",
                    Label = "Exploit Protection: Enable Heap Corruption Termination",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets HeapTerminateOnCorruption=1. Causes the Windows heap manager to terminate the process immediately when heap corruption is detected (double-free, use-after-free, buffer overflow into heap metadata). Without this setting, heap corruption may be exploitable as an information leak or a controlled write primitive. Immediate termination converts exploitation-ready corruption into a crash, significantly raising the exploit quality bar.",
                    Tags = ["exploit-protection", "heap", "corruption", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Terminates processes on heap corruption detection. May surface bugs in applications that previously survived silent heap corruption.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "HeapTerminateOnCorruption", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "HeapTerminateOnCorruption")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "HeapTerminateOnCorruption", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-bottom-up-aslr",
                    Label = "Exploit Protection: Enable Bottom-Up ASLR Randomization",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets BottomUpASLR=1. Enables bottom-up address space layout randomization for heap and stack allocations in addition to the standard top-down module randomization. Bottom-up ASLR increases entropy for heap, stack, and PEB/TEB addresses, which are common targets for info-leak → ASLR-bypass → control-flow hijack exploit chains. Combined with high-entropy ASLR, this makes address guessing attacks computationally infeasible.",
                    Tags = ["exploit-protection", "aslr", "randomization", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ASLR improvements are transparent to applications. No compatibility concerns.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "BottomUpASLR", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "BottomUpASLR")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "BottomUpASLR", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-cfg",
                    Label = "Exploit Protection: Enable Control Flow Guard System-Wide",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableCFG=1. Activates Control Flow Guard (CFG) enforcement for all processes that support it. CFG adds CFG bitmaps at compile time that restrict where indirect calls (call [register]) may transfer control — specifically to valid function entry points. Use-after-free and type confusion vulnerabilities rely on corrupting function pointers or vtables; CFG makes exploitation of these bug classes significantly harder.",
                    Tags = ["exploit-protection", "cfg", "control-flow", "vtable", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "CFG is transparent to users and compatible with all CFG-aware binaries. Old binaries compiled without CFG are not protected but also not harmed.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "EnableCFG", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "EnableCFG")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "EnableCFG", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-strict-cfg",
                    Label = "Exploit Protection: Enable Strict CFG Dispatch Validation",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets StrictCFG=1. Enables strict CFG dispatch validation, which rejects any call target not in the validated CFG bitmap including Export Address Table entries on non-export-suppressed modules. Standard CFG allows calls to EAT entries even if no call site was compiled to call them; strict mode closes this bypass technique used in advanced ROP chains ('JIT spraying' and 'EAT pivoting'). Only meaningful in processes compiled with CFG support.",
                    Tags = ["exploit-protection", "cfg", "strict", "rop", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "May break modules that make non-standard indirect calls to exports. Verify heavily on LOB applications.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "StrictCFG", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "StrictCFG")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "StrictCFG", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-extension-point-disable",
                    Label = "Exploit Protection: Disable Unsupported Extension Points",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ExtensionPointDisable=1. Prevents loading DLLs via legacy extension mechanisms: AppCert DLLs, AppInit DLLs, Browser Helper Objects, and Input Method Editor (IME) DLLs that are not signed by Microsoft. These extension points have been heavily abused by rootkits and spyware as persistence mechanisms. Disabling unsigned extension point DLL loading removes an entire category of persistence technique.",
                    Tags = ["exploit-protection", "extension-points", "appinit", "bho", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "May disable legacy unsigned IMEs and BHOs. Most modern usage of these extension points is malicious.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "ExtensionPointDisable", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "ExtensionPointDisable")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "ExtensionPointDisable", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-disable-win32k-syscalls",
                    Label = "Exploit Protection: Filter Win32k Syscalls for Non-GUI Processes",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableWin32kSystemCalls=1. Enables the Win32k syscall filter for server and background processes that do not need desktop window manager access. Win32k.sys (the kernel-mode portion of the GDI subsystem) has historically been a high-yield target for LPE (local privilege escalation) exploits. Blocking Win32k syscalls from processes that never render windows reduces the attack surface by eliminating hundreds of syscall entries.",
                    Tags = ["exploit-protection", "win32k", "lpe", "kernel", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Filtering Win32k syscalls can crash GUI applications that don't declare they need them. Safe for services; test GUI apps carefully.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "DisableWin32kSystemCalls", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "DisableWin32kSystemCalls")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "DisableWin32kSystemCalls", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-export-address-filter",
                    Label = "Exploit Protection: Enable Export Address Table Filter",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EAF=1. Activates the Export Address Table Access Filtering mitigation. EAF monitors memory reads of module export address tables (EATs) from shellcode-like contexts and raises an exception before the read completes. Shellcode resolving APIs via EAT parsing (walking PEB->Ldr->InMemoryOrderModuleList) is a universal shellcode technique; EAF detects this in real time and terminates the exploiting thread.",
                    Tags = ["exploit-protection", "eaf", "shellcode", "rop", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "EAF can generate false positives in processes that do legitimate EAT inspection (profilers, security tools). Test on security software before deploying.",
                    ApplyOps = [RegOp.SetDword(ExploitKey, "EAF", 1)],
                    RemoveOps = [RegOp.DeleteValue(ExploitKey, "EAF")],
                    DetectOps = [RegOp.CheckDword(ExploitKey, "EAF", 1)],
                },
                new TweakDef
                {
                    Id = "defexploit-enable-cloud-sample-submission",
                    Label = "Exploit Protection: Maximize Exploit Block Cloud Sample Submission",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets MpCloudBlockLevel=6 in MpEngine. Sets the Defender cloud protection block level to 6 (high blocking sensitivity with immediate sample submission). When an exploit attempt is detected and blocked, the triggering binary is submitted to Microsoft's cloud for analysis within seconds. Faster sample submission improves the quality of cloud-based detection for zero-day exploit attempts affecting other machines in the tenant.",
                    Tags = ["exploit-protection", "cloud", "sample", "zero-day", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Increases sample upload rate. Privacy-sensitive in environments with NDA or classified data — verify data governance policies first.",
                    ApplyOps = [RegOp.SetDword(MpKey, "MpCloudBlockLevel", 6)],
                    RemoveOps = [RegOp.DeleteValue(MpKey, "MpCloudBlockLevel")],
                    DetectOps = [RegOp.CheckDword(MpKey, "MpCloudBlockLevel", 6)],
                },
            ];
    
    }

    // ── DefenderFirewallAdvancedPolicy ──
    private static class _DefenderFirewallAdvancedPolicy
    {    
        private const string Domain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
        private const string Standard = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\StandardProfile";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fwadv-enable-domain-firewall",
                    Label = "Enable Windows Firewall — Domain Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enforces Windows Firewall enabled state on the domain network profile via Group Policy, preventing local override by users.",
                    Tags = ["firewall", "domain", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Prevents domain users from disabling the firewall through local settings.",
                    ApplyOps = [RegOp.SetDword(Domain, "EnableFirewall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Domain, "EnableFirewall")],
                    DetectOps = [RegOp.CheckDword(Domain, "EnableFirewall", 1)],
                },
                new TweakDef
                {
                    Id = "fwadv-enable-standard-firewall",
                    Label = "Enable Windows Firewall — Standard Profile",
                    Category = "Defender & Antivirus Policy",
                    Description = "Enforces Windows Firewall enabled on private and public network profiles via Group Policy.",
                    Tags = ["firewall", "standard", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Enforces firewall-on for private and public profiles; blocks local override.",
                    ApplyOps = [RegOp.SetDword(Standard, "EnableFirewall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Standard, "EnableFirewall")],
                    DetectOps = [RegOp.CheckDword(Standard, "EnableFirewall", 1)],
                },
                new TweakDef
                {
                    Id = "fwadv-block-inbound-domain",
                    Label = "Block Inbound Connections by Default — Domain Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DefaultInboundAction=1 (Block) on the domain profile. All inbound traffic is blocked unless explicitly permitted by a firewall rule.",
                    Tags = ["firewall", "inbound", "domain", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks unsolicited inbound on domain networks; pre-configure required inbound rules.",
                    ApplyOps = [RegOp.SetDword(Domain, "DefaultInboundAction", 1)],
                    RemoveOps = [RegOp.DeleteValue(Domain, "DefaultInboundAction")],
                    DetectOps = [RegOp.CheckDword(Domain, "DefaultInboundAction", 1)],
                },
                new TweakDef
                {
                    Id = "fwadv-block-inbound-standard",
                    Label = "Block Inbound Connections by Default — Standard Profile",
                    Category = "Defender & Antivirus Policy",
                    Description = "Sets DefaultInboundAction=1 (Block) on the standard profile, protecting devices on private and public networks.",
                    Tags = ["firewall", "inbound", "standard", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks unsolicited inbound on private/public profiles.",
                    ApplyOps = [RegOp.SetDword(Standard, "DefaultInboundAction", 1)],
                    RemoveOps = [RegOp.DeleteValue(Standard, "DefaultInboundAction")],
                    DetectOps = [RegOp.CheckDword(Standard, "DefaultInboundAction", 1)],
                },
                new TweakDef
                {
                    Id = "fwadv-enable-notifications-domain",
                    Label = "Enable Blocked-App Notifications — Domain Profile",
                    Category = "Defender & Antivirus Policy",
                    Description = "Sets DisableNotifications=0 so users see a notification when the firewall blocks a new program on the domain profile.",
                    Tags = ["firewall", "notifications", "domain", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Improves visibility of new blocked apps without weakening security.",
                    ApplyOps = [RegOp.SetDword(Domain, "DisableNotifications", 0)],
                    RemoveOps = [RegOp.DeleteValue(Domain, "DisableNotifications")],
                    DetectOps = [RegOp.CheckDword(Domain, "DisableNotifications", 0)],
                },
                new TweakDef
                {
                    Id = "fwadv-enable-notifications-standard",
                    Label = "Enable Blocked-App Notifications — Standard Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableNotifications=0 on the standard profile so users see notifications when the firewall blocks a new application.",
                    Tags = ["firewall", "notifications", "standard", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Notifies users of blocked apps on private/public networks.",
                    ApplyOps = [RegOp.SetDword(Standard, "DisableNotifications", 0)],
                    RemoveOps = [RegOp.DeleteValue(Standard, "DisableNotifications")],
                    DetectOps = [RegOp.CheckDword(Standard, "DisableNotifications", 0)],
                },
                new TweakDef
                {
                    Id = "fwadv-log-dropped-domain",
                    Label = "Log Dropped Packets — Domain Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables Windows Firewall logging of dropped packets on the domain profile for security auditing and incident response.",
                    Tags = ["firewall", "logging", "domain", "audit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Dropped-packet log aids forensic investigation of blocked domain traffic.",
                    ApplyOps = [RegOp.SetDword(Domain, "EnableLogDroppedPackets", 1)],
                    RemoveOps = [RegOp.DeleteValue(Domain, "EnableLogDroppedPackets")],
                    DetectOps = [RegOp.CheckDword(Domain, "EnableLogDroppedPackets", 1)],
                },
                new TweakDef
                {
                    Id = "fwadv-log-dropped-standard",
                    Label = "Log Dropped Packets — Standard Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables Windows Firewall dropped-packet logging on the standard profile for forensic auditing of private/public-network traffic.",
                    Tags = ["firewall", "logging", "standard", "audit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Provides packet-drop logs for incident analysis on private/public networks.",
                    ApplyOps = [RegOp.SetDword(Standard, "EnableLogDroppedPackets", 1)],
                    RemoveOps = [RegOp.DeleteValue(Standard, "EnableLogDroppedPackets")],
                    DetectOps = [RegOp.CheckDword(Standard, "EnableLogDroppedPackets", 1)],
                },
                new TweakDef
                {
                    Id = "fwadv-log-max-size-domain",
                    Label = "Set Firewall Log Max Size 16 MB — Domain Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets the Windows Firewall log maximum to 16384 KB (16 MB) on the domain profile, retaining substantially more history for incident analysis.",
                    Tags = ["firewall", "logging", "domain", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Larger log file preserves more drop history; negligible disk usage.",
                    ApplyOps = [RegOp.SetDword(Domain, "LogFileSize", 16384)],
                    RemoveOps = [RegOp.DeleteValue(Domain, "LogFileSize")],
                    DetectOps = [RegOp.CheckDword(Domain, "LogFileSize", 16384)],
                },
                new TweakDef
                {
                    Id = "fwadv-disable-unicast-domain",
                    Label = "Disable Unicast Response to Multicast/Broadcast — Domain",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Prevents unicast replies to multicast/broadcast frames on the domain profile, reducing exposure to network-scanning reconnaissance (DisableUnicastResponsesToMulticastBroadcast=1).",
                    Tags = ["firewall", "multicast", "domain", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses unicast responses to broadcast probes; limits host discovery.",
                    ApplyOps = [RegOp.SetDword(Domain, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                    RemoveOps = [RegOp.DeleteValue(Domain, "DisableUnicastResponsesToMulticastBroadcast")],
                    DetectOps = [RegOp.CheckDword(Domain, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                },
            ];
    
    }

    // ── DefenderNetworkProtectionPolicy ──
    private static class _DefenderNetworkProtectionPolicy
    {    
        private const string NetProtKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection";
    
        private const string SmartScreenKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "defnet-enable-block-mode",
                    Label = "Network Protection: Enable Block Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableNetworkProtection=1 (block mode). Activates Defender Network Protection, which uses Microsoft's cloud-based threat intelligence to block outbound connections to known-malicious IP addresses and domains. NP operates at the kernel level via the Windows Filtering Platform, intercepting connections before they leave the machine. Covers all applications (not just browsers), including LOLBins and malware command-and-control beaconing.",
                    Tags = ["network-protection", "defender", "c2", "malware", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Blocks connections to Microsoft-rated malicious hosts. Rare false positives for uncommon legitimate domains.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "EnableNetworkProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableNetworkProtection")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "EnableNetworkProtection", 1)],
                },
                new TweakDef
                {
                    Id = "defnet-enable-audit-mode",
                    Label = "Network Protection: Enable Audit Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableNetworkProtection=2 (audit mode). Logs all Network Protection block events to Event ID 1125 without actually blocking the connection. Use audit mode to understand which outbound connections would be blocked before switching to block mode. Useful for identifying legitimate business applications that connect to hosts that NP would flag. Requires Microsoft Defender Antivirus to be the active AV.",
                    Tags = ["network-protection", "defender", "audit", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Audit-only; no connections blocked. Safe to deploy first.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "EnableNetworkProtection", 2)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableNetworkProtection")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "EnableNetworkProtection", 2)],
                },
                new TweakDef
                {
                    Id = "defnet-block-low-reputation",
                    Label = "Network Protection: Block Low-Reputation Cloud Downloads",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets BlockLowReputationCode=1. Instructs Network Protection to block downloads from URLs where the destination file has a low cloud reputation score in Microsoft's SmartScreen service. Files with no reputation or insufficient prevalence among Microsoft's telemetry pool are blocked before they are fully downloaded. Complements SmartScreen-at-launch protection with pre-download reputation-based blocking.",
                    Tags = ["network-protection", "smartscreen", "download", "reputation", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "May block uncommon but legitimate files with low cloud prevalence. Users may see block alerts on novel tools.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "BlockLowReputationCode", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "BlockLowReputationCode")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "BlockLowReputationCode", 1)],
                },
                new TweakDef
                {
                    Id = "defnet-disable-dns-over-udp",
                    Label = "Network Protection: Enforce DNS Inspection (Block DNS Tunneling)",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableDnsOverHttps=0. Prevents applications from bypassing Network Protection DNS inspection by forcing DNS queries through encrypted channels (DoH) that NP cannot inspect. DNS tunneling is used by C2 frameworks (Cobalt Strike, Metasploit DNS shells) to exfiltrate data and receive commands via DNS TXT/CNAME records. Keeping NP's DNS inspection path active ensures malicious DNS traffic is visible to Defender.",
                    Tags = ["network-protection", "dns", "tunneling", "c2", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Disables app-level DoH within NP scope; system DoH policy is separate. Some apps may fall back to plain DNS.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "EnableDnsOverHttps", 0)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableDnsOverHttps")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "EnableDnsOverHttps", 0)],
                },
                new TweakDef
                {
                    Id = "defnet-enable-smartscreen-app",
                    Label = "Network Protection: Enable SmartScreen for Applications",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableSmartScreenInShell=1.  Forces SmartScreen reputation checks for all executables and scripts launched from within applications (not just from Explorer). Without this setting, processes launched by LOLBins or injected threads bypass the Explorer SmartScreen path. Enabling SmartScreen in-shell ensures reputation checks happen regardless of the launch context.",
                    Tags = ["network-protection", "smartscreen", "lolbin", "reputation", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Adds SmartScreen latency to process launches. Infrequent but non-zero delay for unknown binaries.",
                    ApplyOps = [RegOp.SetDword(SmartScreenKey, "EnableSmartScreenInShell", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartScreenKey, "EnableSmartScreenInShell")],
                    DetectOps = [RegOp.CheckDword(SmartScreenKey, "EnableSmartScreenInShell", 1)],
                },
                new TweakDef
                {
                    Id = "defnet-disable-bypass-smartscreen",
                    Label = "Network Protection: Prevent Users from Bypassing SmartScreen Blocks",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets PreventOverrideForFilesInShell=1. Removes the 'Run Anyway' button from SmartScreen block dialogs for file launches. Without this setting, determined users can bypass SmartScreen warnings by clicking through. In enterprise environments, users should not be able to override network protection policy decisions. Setting this to 1 makes SmartScreen blocks final — users must contact IT administration.",
                    Tags = ["network-protection", "smartscreen", "override", "enterprise", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Users cannot bypass SmartScreen. Ensure an IT process exists for requesting allow-list exceptions.",
                    ApplyOps = [RegOp.SetDword(SmartScreenKey, "PreventOverrideForFilesInShell", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartScreenKey, "PreventOverrideForFilesInShell")],
                    DetectOps = [RegOp.CheckDword(SmartScreenKey, "PreventOverrideForFilesInShell", 1)],
                },
                new TweakDef
                {
                    Id = "defnet-block-suspicious-behaviors",
                    Label = "Network Protection: Enable Behavioral Monitoring of Network Traffic",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableBehavioralNetworkBlocks=1. Activates Defender's behavioral engine for network connection analysis. Unlike reputation-only blocks, behavioral monitoring detects C2 patterns (beaconing intervals, jitter, domain generation algorithms) that signature-only defenses cannot catch. Behavioral blocks are complementary to reputation blocks — a novel C2 domain with no reputation history will still be detected via behavioral patterns.",
                    Tags = ["network-protection", "behavioral", "c2", "detection", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Behavioral analysis adds minor network latency. Rare false positives on legitimate beaconing apps (monitoring agents).",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "EnableBehavioralNetworkBlocks", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableBehavioralNetworkBlocks")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "EnableBehavioralNetworkBlocks", 1)],
                },
                new TweakDef
                {
                    Id = "defnet-block-potentially-unwanted",
                    Label = "Network Protection: Block Connections to PUA/PUP Infrastructure",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableNetworkProtectionPua=1. Extends Network Protection to block outbound connections to infrastructure associated with Potentially Unwanted Applications and bundlers. PUA families (adware, browser hijackers, crypto miners) frequently use dedicated C2 networks distinct from malware. Blocking PUA network traffic prevents tracking pixel calls, update beaconing, and telemetry uploads from unwanted applications.",
                    Tags = ["network-protection", "pua", "adware", "privacy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Targets PUA-associated network infrastructure. Some dual-use analytic tools may be affected.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "EnableNetworkProtectionPua", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableNetworkProtectionPua")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "EnableNetworkProtectionPua", 1)],
                },
                new TweakDef
                {
                    Id = "defnet-enable-cloud-check",
                    Label = "Network Protection: Enable Real-Time Cloud-Based URL Lookup",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets CloudExtendedTimeout=50. Sets the maximum time (50 × 100 ms = 5 s) that Network Protection will wait for a cloud reputation response before allowing a connection. A longer timeout allows the cloud protection service to consult the NP telemetry database fully before deciding whether to block a connection to a novel domain. Balances cloud check completeness against connection latency.",
                    Tags = ["network-protection", "cloud", "url-check", "latency"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Adds up to 5 s latency on first connections to novel domains. Subsequent connections to the same domain are cached.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "CloudExtendedTimeout", 50)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "CloudExtendedTimeout")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "CloudExtendedTimeout", 50)],
                },
                new TweakDef
                {
                    Id = "defnet-enable-loopback-block",
                    Label = "Network Protection: Block Loopback Bypass Attempts",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableLoopbackExemption=1. Removes the automatic exemption that Network Protection grants to loopback (127.0.0.1) connections. Some malware proxies its C2 traffic through a local port listener on loopback to bypass per-process network monitoring. While most NP rules already apply to all network destinations, this setting ensures that loopback-aliased proxy traffic is also subject to behavioral analysis.",
                    Tags = ["network-protection", "loopback", "proxy", "evasion", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Applies NP to loopback traffic. Development tools that use local proxies (Fiddler, mitmproxy) may be affected.",
                    ApplyOps = [RegOp.SetDword(NetProtKey, "DisableLoopbackExemption", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetProtKey, "DisableLoopbackExemption")],
                    DetectOps = [RegOp.CheckDword(NetProtKey, "DisableLoopbackExemption", 1)],
                },
            ];
    
    }

    // ── DefenderSignatureUpdatePolicy ──
    private static class _DefenderSignatureUpdatePolicy
    {    
        private const string SigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Signature Updates";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "defsig-update-interval-1h",
                    Label = "Signature Updates: Check for Updates Every 1 Hour",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets SignatureUpdateInterval=1. Instructs Defender to check for signature updates every 1 hour. The default Windows behavior is to check every 8–24 hours, which can leave machines unprotected for hours after a major threat campaign launches. A 1-hour interval minimizes the signature gap during active outbreak periods and is fully supported by Microsoft Update Infrastructure without performance impact on the endpoint.",
                    Tags = ["defender", "signatures", "updates", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Increases update check frequency. Minimal bandwidth and CPU impact; signature packages are typically < 1 MB.",
                    ApplyOps = [RegOp.SetDword(SigKey, "SignatureUpdateInterval", 1)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "SignatureUpdateInterval")],
                    DetectOps = [RegOp.CheckDword(SigKey, "SignatureUpdateInterval", 1)],
                },
                new TweakDef
                {
                    Id = "defsig-fallback-to-microsoft-update",
                    Label = "Signature Updates: Fall Back to Microsoft Update if WSUS Unreachable",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets FallbackOrder=MicrosoftUpdateServer|MMPC. Configures the signature update fallback order so that if the local WSUS server or Windows Update for Business policy server is unreachable, Defender falls back to downloading definitions directly from Microsoft's MMPC (Malware Protection Center). Prevents signature staleness during WSUS outages or when laptops are off-network and ensures continuous protection regardless of update infrastructure availability.",
                    Tags = ["defender", "signatures", "wsus", "fallback"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Bypasses WSUS when unreachable; machines may download updates directly from Microsoft. Review bandwidth policy for remote workers.",
                    ApplyOps = [RegOp.SetString(SigKey, "FallbackOrder", "MicrosoftUpdateServer|MMPC")],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "FallbackOrder")],
                    DetectOps = [RegOp.CheckString(SigKey, "FallbackOrder", "MicrosoftUpdateServer|MMPC")],
                },
                new TweakDef
                {
                    Id = "defsig-disable-update-on-battery",
                    Label = "Signature Updates: Do Not Restrict Updates on Battery Power",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableScheduledScanningOnBattery=0. Ensures that scheduled signature updates and scans run regardless of whether the device is on battery or AC power. Windows defaults to skipping scheduled Defender tasks when on battery to conserve power. For mobile workers, this means laptops running on battery may miss signature updates for extended periods. Setting 0 ensures consistent protection without requiring AC power.",
                    Tags = ["defender", "signatures", "battery", "laptop"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Minor battery impact from running update checks on battery. Update checks are brief and infrequent.",
                    ApplyOps = [RegOp.SetDword(SigKey, "DisableScheduledScanningOnBattery", 0)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "DisableScheduledScanningOnBattery")],
                    DetectOps = [RegOp.CheckDword(SigKey, "DisableScheduledScanningOnBattery", 0)],
                },
                new TweakDef
                {
                    Id = "defsig-check-on-startup",
                    Label = "Signature Updates: Check for Updates at Startup",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets CheckForSignaturesBeforeRunningScan=1. Forces Defender to check for updated signatures before initiating any scheduled or on-demand scan. Without this setting, Defender may run scheduled scans with signatures that are hours old. Pre-scan signature checks ensure that every scan uses the most current available definitions, especially important for systems that have been powered off overnight and thus missed hourly update checks.",
                    Tags = ["defender", "signatures", "scan", "startup"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Adds brief update check before scans. Scan startup may be delayed by 10–30 s if a signature update is available.",
                    ApplyOps = [RegOp.SetDword(SigKey, "CheckForSignaturesBeforeRunningScan", 1)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "CheckForSignaturesBeforeRunningScan")],
                    DetectOps = [RegOp.CheckDword(SigKey, "CheckForSignaturesBeforeRunningScan", 1)],
                },
                new TweakDef
                {
                    Id = "defsig-enable-dynamic-signatures",
                    Label = "Signature Updates: Enable Dynamic Cloud-Based Security Intelligence",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableDynamicSignatures=0. Ensures Defender receives Dynamic Security Intelligence (DSI) — real-time cloud signatures pushed to clients without requiring a full signature update package. DSI allows Microsoft to deploy detections for zero-day threats globally within seconds of discovery, not just at the next scheduled update interval. Disabling this would limit Defender to stale signature packages only.",
                    Tags = ["defender", "signatures", "cloud", "zero-day", "dynamic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Enables real-time signature delivery from Microsoft cloud. Requires outbound HTTPS to Microsoft's DSI endpoints.",
                    ApplyOps = [RegOp.SetDword(SigKey, "DisableDynamicSignatures", 0)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "DisableDynamicSignatures")],
                    DetectOps = [RegOp.CheckDword(SigKey, "DisableDynamicSignatures", 0)],
                },
                new TweakDef
                {
                    Id = "defsig-stale-threshold-1-day",
                    Label = "Signature Updates: Trigger Alert if Signatures Are 1+ Days Old",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets SignatureDisableUpdateOnStartupWithoutEngine=0 and sets signature age threshold to 1 day (86400 seconds). Causes Defender to generate a health alert if signatures are older than 24 hours. Administrators monitoring Windows Security Center via SCCM, Intune, or custom health scripts can detect signature staleness proactively. Without this threshold, outdated signatures may go unnoticed unless the Security Center UI is opened.",
                    Tags = ["defender", "signatures", "alert", "monitoring", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Generates health alerts for stale signatures; no operational impact. Requires monitoring infrastructure to act on the alerts.",
                    ApplyOps = [RegOp.SetDword(SigKey, "SignatureDisableUpdateOnStartupWithoutEngine", 0)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "SignatureDisableUpdateOnStartupWithoutEngine")],
                    DetectOps = [RegOp.CheckDword(SigKey, "SignatureDisableUpdateOnStartupWithoutEngine", 0)],
                },
                new TweakDef
                {
                    Id = "defsig-shared-signatures-unc",
                    Label = "Signature Updates: Configure UNC Share as Signature Source",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DefinitionUpdateFileSharesSources policy path. Configures Defender on air-gapped or bandwidth-constrained networks to download signatures from a local UNC file share populated by a management server. This avoids all machines downloading from Microsoft Update directly. Signature files copied to the share from MSRT or manually kept current are distributed to all clients pointing to the share path.",
                    Tags = ["defender", "signatures", "unc-share", "air-gap", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires a maintained UNC share with current signatures. If the share becomes stale, all clients stop receiving updates.",
                    ApplyOps = [RegOp.SetDword(SigKey, "DefinitionUpdateFileSharesSources", 1)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "DefinitionUpdateFileSharesSources")],
                    DetectOps = [RegOp.CheckDword(SigKey, "DefinitionUpdateFileSharesSources", 1)],
                },
                new TweakDef
                {
                    Id = "defsig-disable-catchup-scan",
                    Label = "Signature Updates: Enable Catch-Up Scan After Missed Update",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableCatchupQuickScan=0. Ensures that when a machine misses a scheduled quick scan (e.g., powered off), Defender schedules a catch-up quick scan at the next available opportunity. Without catch-up scans, devices that are frequently off during scheduled scan windows may go days or weeks without being scanned. Setting 0 ensures no scan gaps regardless of device usage patterns.",
                    Tags = ["defender", "signatures", "catchup", "scan", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Catch-up scans run in idle background mode. Minor impact on first login after device comes back online.",
                    ApplyOps = [RegOp.SetDword(SigKey, "DisableCatchupQuickScan", 0)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "DisableCatchupQuickScan")],
                    DetectOps = [RegOp.CheckDword(SigKey, "DisableCatchupQuickScan", 0)],
                },
                new TweakDef
                {
                    Id = "defsig-max-signature-age",
                    Label = "Signature Updates: Enforce Maximum Signature Age of 2 Days",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets SignatureStaleDetectionThreshold to 2 (days). If Defender's signatures are older than 2 days, the Security Health Report marks the device as non-compliant (red status). This threshold feeds into Intune device compliance policies and Conditional Access controls — machines with stale AV signatures can be automatically blocked from accessing corporate resources until updated. Two days provides a reasonable buffer for VPN-only corporate devices.",
                    Tags = ["defender", "signatures", "compliance", "intune", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Two-day threshold. Devices that are off-network for > 2 days will show red health status.",
                    ApplyOps = [RegOp.SetDword(SigKey, "SignatureStaleDetectionThreshold", 2)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "SignatureStaleDetectionThreshold")],
                    DetectOps = [RegOp.CheckDword(SigKey, "SignatureStaleDetectionThreshold", 2)],
                },
                new TweakDef
                {
                    Id = "defsig-disable-signature-on-low-disk",
                    Label = "Signature Updates: Do Not Skip Updates When Disk Space Is Low",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets SignatureDisableUpdateOnStartupWithoutEngine=0 on low-disk-space paths. Ensures Defender continues to download signature updates even when disk free space drops below the default low-disk threshold. Defender normally skips signature downloads when disk space is critically low to avoid filling the drive. However, signature staleness during low-disk conditions creates a security gap at a likely-stressful time. This setting prioritizes security over disk-space conservation.",
                    Tags = ["defender", "signatures", "disk-space", "update"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "May use limited disk space for signature files when disk is nearly full. Ensure adequate disk space management policy.",
                    ApplyOps = [RegOp.SetDword(SigKey, "ForceUpdateFromMU", 1)],
                    RemoveOps = [RegOp.DeleteValue(SigKey, "ForceUpdateFromMU")],
                    DetectOps = [RegOp.CheckDword(SigKey, "ForceUpdateFromMU", 1)],
                },
            ];
    
    }

    // ── EarlyLaunchAMPolicy ──
    private static class _EarlyLaunchAMPolicy
    {    
        private const string ElaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\EarlyLaunch";
        private const string ElaCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\EarlyLaunch";
    
        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "elam-set-driver-init-policy-good",
                Label = "ELAM: Allow Only Known-Good Drivers at Boot",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets DriverLoadPolicy=1 in the EarlyLaunch Policies key. "
                    + "Instructs Windows Boot Manager to load ONLY drivers rated as 'Good' by the ELAM driver during boot. "
                    + "Drivers classified as 'Unknown' or 'Bad' are blocked from initialising, giving the strongest "
                    + "pre-OS boot protection. "
                    + "Values: 1=Good only, 3=Good+Unknown, 7=Good+Unknown+Bad(BootCritical), 0=Unknown+Bad all. "
                    + "Default: 3 (Good+Unknown). Recommended: 1 for maximum hardening.",
                Tags = ["elam", "boot", "driver", "malware", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Only ELAM-rated 'Good' boot drivers are loaded; unknown third-party drivers may be blocked at boot.",
                ApplyOps = [RegOp.SetDword(ElaKey, "DriverLoadPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "DriverLoadPolicy")],
                DetectOps = [RegOp.CheckDword(ElaKey, "DriverLoadPolicy", 1)],
            },
            new TweakDef
            {
                Id = "elam-set-driver-init-policy-good-unknown",
                Label = "ELAM: Allow Good and Unknown Drivers at Boot",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets DriverLoadPolicy=3 in the EarlyLaunch Policies key. "
                    + "Instructs Windows Boot Manager to load drivers rated as 'Good' or 'Unknown' by the ELAM driver. "
                    + "This is the Windows default and appropriate for most systems — blocks only definitively 'Bad' drivers. "
                    + "Recommended over DriverLoadPolicy=1 when third-party boot-start drivers (e.g., storage controllers) "
                    + "are present that have not yet received an ELAM classification. "
                    + "Default: 3. Recommended: 3 as a balanced baseline.",
                Tags = ["elam", "boot", "driver", "balanced", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Boot drivers rated 'Good' or 'Unknown' are loaded; only 'Bad'-rated drivers are blocked.",
                ApplyOps = [RegOp.SetDword(ElaKey, "DriverLoadPolicy", 3)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "DriverLoadPolicy")],
                DetectOps = [RegOp.CheckDword(ElaKey, "DriverLoadPolicy", 3)],
            },
            new TweakDef
            {
                Id = "elam-set-driver-init-critical-only",
                Label = "ELAM: Allow Good + Unknown + Bad-Critical Drivers",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets DriverLoadPolicy=7 in the EarlyLaunch Policies key. "
                    + "Allows boot drivers rated 'Good', 'Unknown', and even 'Bad' if they are marked as "
                    + "boot-critical (system would not boot without them). Provides compatibility for legacy "
                    + "hardware with drivers that ELAM cannot classify. "
                    + "Appropriate only when DriverLoadPolicy=3 causes hardware failures. "
                    + "Default: not set. Recommended: use only if 3 causes boot failures.",
                Tags = ["elam", "boot", "driver", "compatibility", "legacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Boot-critical bad-rated drivers are allowed; compatibility maximised, security reduced.",
                ApplyOps = [RegOp.SetDword(ElaKey, "DriverLoadPolicy", 7)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "DriverLoadPolicy")],
                DetectOps = [RegOp.CheckDword(ElaKey, "DriverLoadPolicy", 7)],
            },
            new TweakDef
            {
                Id = "elam-disable-elam-driver",
                Label = "Disable Early Launch Anti-Malware Driver",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets DisableElam=1 in the EarlyLaunch control key. "
                    + "Disables the Windows Early Launch Anti-Malware driver entirely, removing pre-boot "
                    + "driver classification. Not recommended for production systems — use only when the "
                    + "ELAM driver conflicts with specific virtualisation or firmware configurations. "
                    + "Default: absent (ELAM enabled). Setting 1 disables ELAM protection.",
                Tags = ["elam", "boot", "disable", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 2,
                ImpactNote = "ELAM boot protection fully disabled; no pre-boot driver classification or blocking.",
                ApplyOps = [RegOp.SetDword(ElaCtrl, "DisableElam", 1)],
                RemoveOps = [RegOp.DeleteValue(ElaCtrl, "DisableElam")],
                DetectOps = [RegOp.CheckDword(ElaCtrl, "DisableElam", 1)],
            },
            new TweakDef
            {
                Id = "elam-set-scan-timeout-increased",
                Label = "Increase ELAM Scan Timeout",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets ElamDriverTimeout=30000 (30 seconds) in the EarlyLaunch Policies key. "
                    + "Sets the maximum time in milliseconds the Windows Boot Manager waits for the ELAM "
                    + "driver to scan and classify a boot-start driver before treating it as 'Unknown'. "
                    + "Default: absent (default ~0.5–2 seconds). "
                    + "Increase when ELAM scanning of large or complex drivers causes boot timeouts.",
                Tags = ["elam", "boot", "timeout", "scanning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "ELAM scan timeout increased to 30 s; useful for machines with many heavy boot drivers.",
                ApplyOps = [RegOp.SetDword(ElaKey, "ElamDriverTimeout", 30000)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "ElamDriverTimeout")],
                DetectOps = [RegOp.CheckDword(ElaKey, "ElamDriverTimeout", 30000)],
            },
            new TweakDef
            {
                Id = "elam-enable-elam-event-logging",
                Label = "Enable ELAM Boot Classification Event Logging",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets EnableEventLogging=1 in the EarlyLaunch Policies key. "
                    + "Instructs the ELAM subsystem to log each boot-driver classification decision to "
                    + "the Windows Event Log (Microsoft-Windows-EarlyLaunch channel) after boot. "
                    + "Provides an audit trail of which drivers were allowed, blocked, or classified unknown. "
                    + "Default: absent (no event logging). Recommended: 1 in security-audited environments.",
                Tags = ["elam", "logging", "audit", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ELAM boot driver classification decisions logged to the Windows Event Log.",
                ApplyOps = [RegOp.SetDword(ElaKey, "EnableEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "EnableEventLogging")],
                DetectOps = [RegOp.CheckDword(ElaKey, "EnableEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "elam-block-unknown-boot-drivers",
                Label = "Block 'Unknown' Boot Drivers via ELAM Heuristics",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets TreatUnknownAsGood=0 in the EarlyLaunch Policies key. "
                    + "Overrides the default ELAM heuristic that treats unclassified ('Unknown') boot drivers "
                    + "as safe to load. Setting 0 instructs ELAM to be conservative: unclassified drivers "
                    + "are treated as potentially bad, not good. Increases protection at the cost of possible "
                    + "compatibility issues with lesser-known driver packages. "
                    + "Default: 1 (unknown=good). Recommended: 0 for hardened servers.",
                Tags = ["elam", "unknown", "heuristics", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Unknown boot drivers treated as potentially malicious by ELAM; may block unrecognised hardware at boot.",
                ApplyOps = [RegOp.SetDword(ElaKey, "TreatUnknownAsGood", 0)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "TreatUnknownAsGood")],
                DetectOps = [RegOp.CheckDword(ElaKey, "TreatUnknownAsGood", 0)],
            },
            new TweakDef
            {
                Id = "elam-enable-network-elam",
                Label = "Enable Network ELAM Protection",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets EnableNetworkELAM=1 in the EarlyLaunch Policies key. "
                    + "Activates the Network ELAM extension that classifies network driver stack components "
                    + "(NDIS miniport, filter, and protocol drivers) during the early launch phase. "
                    + "Provides pre-OS-network protection before traditional antivirus can initialise. "
                    + "Default: absent. Recommended: 1 on systems with network security requirements.",
                Tags = ["elam", "network", "ndis", "drivers", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Network stack drivers (NDIS) classified by ELAM during boot; malicious network drivers blocked.",
                ApplyOps = [RegOp.SetDword(ElaKey, "EnableNetworkELAM", 1)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "EnableNetworkELAM")],
                DetectOps = [RegOp.CheckDword(ElaKey, "EnableNetworkELAM", 1)],
            },
            new TweakDef
            {
                Id = "elam-enable-measured-boot",
                Label = "Enable Windows Measured Boot Attestation",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets EnableMeasuredBoot=1 in the EarlyLaunch Policies key. "
                    + "Activates Windows Measured Boot, which records boot measurements (PCR values) "
                    + "in the system TPM for each boot phase, including the ELAM driver's assessments. "
                    + "Enables remote attestation of the boot sequence for Device Health Attestation services. "
                    + "Default: absent. Recommended: 1 on TPM-equipped machines in zero-trust environments.",
                Tags = ["elam", "measured-boot", "tpm", "attestation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Measured Boot enabled; boot PCR values stored in TPM for remote attestation.",
                ApplyOps = [RegOp.SetDword(ElaKey, "EnableMeasuredBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "EnableMeasuredBoot")],
                DetectOps = [RegOp.CheckDword(ElaKey, "EnableMeasuredBoot", 1)],
            },
            new TweakDef
            {
                Id = "elam-enable-boot-log-persistence",
                Label = "Persist ELAM Boot Log Across Reboots",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Sets PersistBootLog=1 in the EarlyLaunch Policies key. "
                    + "Enables persistence of the ELAM boot log across reboots, allowing security tools "
                    + "and the antimalware service to review prior boot classifications even after subsequent "
                    + "restarts. Assists forensic analysis of boot-time driver activity. "
                    + "Default: absent (log cleared after each boot). Recommended: 1 in forensic/IR environments.",
                Tags = ["elam", "logging", "persistence", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ELAM boot classification log persisted across reboots for forensic and audit access.",
                ApplyOps = [RegOp.SetDword(ElaKey, "PersistBootLog", 1)],
                RemoveOps = [RegOp.DeleteValue(ElaKey, "PersistBootLog")],
                DetectOps = [RegOp.CheckDword(ElaKey, "PersistBootLog", 1)],
            },
        ];
    
    }

    // ── EnhancedPhishingProtectionPolicy ──
    private static class _EnhancedPhishingProtectionPolicy
    {    
        private const string WtdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WTDS";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ephpol-enable-service",
                Label = "Enhanced Phishing Protection: Enable Service",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Enables Windows Defender SmartScreen Enhanced Phishing Protection (WTDS). Monitors corporate passwords entered in browsers and apps for phishing indicators.",
                Tags = ["phishing", "smartscreen", "wtds", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Enables the WTDS service; prerequisite for all Enhanced Phishing Protection tweaks.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "ServiceEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "ServiceEnabled")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "ServiceEnabled", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-notify-unsafe-app",
                Label = "Enhanced Phishing Protection: Notify on Unsafe App Password Reuse",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Warns users when they type their corporate (Azure AD/local) password into apps other than Windows sign-in. Available from Windows 11 22H2.",
                Tags = ["phishing", "smartscreen", "wtds", "credential", "password", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Warns users when corporate password is typed in non-system apps.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordReuse", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordReuse")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordReuse", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-notify-unsafe-site",
                Label = "Enhanced Phishing Protection: Notify on Phishing Site Password Entry",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Warns users when they type their corporate password onto a site that SmartScreen identifies as phishing. Triggers a warning before the password is submitted.",
                Tags = ["phishing", "smartscreen", "wtds", "credential", "browser", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Warns users before submitting credentials to a phishing site.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyMalicious", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyMalicious")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyMalicious", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-block-plaintext-passwords",
                Label = "Enhanced Phishing Protection: Block Plaintext Password Storage",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Prevents users from storing work or school passwords in plain text files (Notepad, Word, etc.). WTDS detects password entry in low-trust document contexts.",
                Tags = ["phishing", "smartscreen", "wtds", "password", "plaintext", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents corporate password entry in plaintext documents.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "CaptureThreatWindow", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "CaptureThreatWindow")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "CaptureThreatWindow", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-audit-only-mode",
                Label = "Enhanced Phishing Protection: Set Audit-Only Mode",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Puts Enhanced Phishing Protection into audit mode — events are logged but no user warnings are shown. Useful for baseline assessment before enforcing notifications.",
                Tags = ["phishing", "smartscreen", "wtds", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Logs phishing events without user warnings; for baseline assessment only.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "AuditMode", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "AuditMode")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "AuditMode", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-enable-enterprise-indicators",
                Label = "Enhanced Phishing Protection: Enable Enterprise Phishing Indicators",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Enables enterprise-specific phishing indicator checks in WTDS, allowing domain-joined and Entra ID-joined devices to use corporate threat intelligence feeds.",
                Tags = ["phishing", "smartscreen", "wtds", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables corporate threat intelligence feeds for phishing detection.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "EnterpriseIndicatorsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "EnterpriseIndicatorsEnabled")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "EnterpriseIndicatorsEnabled", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-block-credential-reuse-apps",
                Label = "Enhanced Phishing Protection: Block Credential Reuse Across Apps",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Blocks users from reusing their Windows sign-in PIN or password in non-system apps. Reduces password spray attack surface on shared or kiosk machines.",
                Tags = ["phishing", "smartscreen", "wtds", "pin", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks Windows sign-in PIN/password reuse in non-system apps.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "BlockCredentialReuseInApps", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "BlockCredentialReuseInApps")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "BlockCredentialReuseInApps", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-enable-logging",
                Label = "Enhanced Phishing Protection: Enable Diagnostic Logging",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Enables detailed WTDS diagnostic logging to the Windows Event Log under Microsoft-Windows-Security-EnhancedPhishingProtection. Useful for SOC triage.",
                Tags = ["phishing", "smartscreen", "wtds", "logging", "soc", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables WTDS diagnostic logging to Event Log for SOC triage.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "EnableEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "EnableEventLogging")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "EnableEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-enforce-service",
                Label = "Enhanced Phishing Protection: Enforce Service (Non-Interactive)",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Prevents users from disabling or bypassing Enhanced Phishing Protection via Settings. The WTDS service cannot be turned off by non-admins.",
                Tags = ["phishing", "smartscreen", "wtds", "enforce", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents non-admins from disabling Enhanced Phishing Protection.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "ServiceEnforced", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "ServiceEnforced")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "ServiceEnforced", 1)],
            },
            new TweakDef
            {
                Id = "ephpol-notify-password-change",
                Label = "Enhanced Phishing Protection: Notify IT on Password Re-Entry After Change",
                Category = "Defender & Antivirus Policy",
                Description =
                    "Notifies the IT help desk (via telemetry event) when a user re-enters their previous password after a forced password change. Detects credential-recycling behaviour.",
                Tags = ["phishing", "smartscreen", "wtds", "password", "helpdesk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Alerts IT when user re-enters previous password after a forced change.",
                RegistryKeys = [WtdsKey],
                ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordChangeReuse", 1)],
                RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordChangeReuse")],
                DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordChangeReuse", 1)],
            },
        ];
    
    }

    // ── ExploitGuardPolicy ──
    private static class _ExploitGuardPolicy
    {    
        private const string AsrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR";
        private const string EgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard";
        private const string CfaKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "egpol-enable-asr-rules",
                    Label = "Enable Attack Surface Reduction (ASR) Rules Engine",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Activates the Windows Defender Exploit Guard Attack Surface Reduction rules engine, which enforces behavioural restrictions on Office, browser, and email applications to prevent common malware persistence and exploitation techniques.",
                    Tags = ["exploit-guard", "asr", "defender", "attack-surface-reduction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "ASR rules engine enabled; behavioural restrictions active for Office, scripts, and email clients.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "ExploitGuard_ASR_Rules", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "ExploitGuard_ASR_Rules")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "ExploitGuard_ASR_Rules", 1)],
                },
                new TweakDef
                {
                    Id = "egpol-enable-network-protection-block",
                    Label = "Enable Defender Network Protection in Block Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Configures Windows Defender Exploit Guard Network Protection in Block mode, which blocks outbound connections from all processes (not just Edge) to IP addresses and domains with known malicious reputation.",
                    Tags = ["exploit-guard", "network-protection", "block-mode", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Network Protection in Block mode; all processes blocked from connecting to malicious IPs/domains.",
                    ApplyOps = [RegOp.SetDword(EgKey, "EnableNetworkProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(EgKey, "EnableNetworkProtection")],
                    DetectOps = [RegOp.CheckDword(EgKey, "EnableNetworkProtection", 1)],
                },
                new TweakDef
                {
                    Id = "egpol-enable-cfa-block-mode",
                    Label = "Enable Controlled Folder Access in Block Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Activates Controlled Folder Access (CFA) in Block mode, which prevents untrusted applications from making changes to protected folders (Documents, Desktop, Pictures), protecting against ransomware encryption of user files.",
                    Tags = ["exploit-guard", "controlled-folder-access", "ransomware", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "CFA Block mode enabled; untrusted apps cannot modify Documents/Desktop/Pictures. May block legacy apps.",
                    ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                    DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 1)],
                },
                new TweakDef
                {
                    Id = "egpol-block-office-child-processes",
                    Label = "Block Office Applications from Spawning Child Processes",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables ASR rule D4F940AB-401B-4EFC-AADC-AD5F3C50688A that prevents Office applications (Word, Excel, PowerPoint) from spawning child processes such as cmd.exe or PowerShell, blocking macro-based malware execution.",
                    Tags = ["exploit-guard", "asr", "office", "child-process", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Office child process creation blocked; macro-launched cmd.exe/PowerShell execution prevented.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "D4F940AB401B4EFCAADC-AD5F3C50688A",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "D4F940AB401B4EFCAADC-AD5F3C50688A"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "D4F940AB401B4EFCAADC-AD5F3C50688A",
                            1
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "egpol-block-lsass-credential-steal",
                    Label = "Block Credential Stealing from LSASS via ASR",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables ASR rule 9E6C4E1F-7D60-472F-BA1A-A39EF669E4B0 that blocks processes from reading memory of lsass.exe, preventing credential dumping attacks using tools like Mimikatz that extract NTLM hashes and Kerberos tickets.",
                    Tags = ["exploit-guard", "asr", "lsass", "credential-dumping", "mimikatz", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LSASS memory read blocked via ASR; Mimikatz and similar credential dumping tools blocked.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "9E6C4E1F7D60472FBA1AA39EF669E4B0",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "9E6C4E1F7D60472FBA1AA39EF669E4B0"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "9E6C4E1F7D60472FBA1AA39EF669E4B0",
                            1
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "egpol-block-executable-email-content",
                    Label = "Block Executable Content from Email and Webmail",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables ASR rule BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550 that blocks execution of executable content (scripts, macros, executables) directly from email clients and webmail, preventing common phishing delivery mechanisms.",
                    Tags = ["exploit-guard", "asr", "email", "executable", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Executable content from email blocked; scripts and EXEs cannot be run directly from email attachments.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "BE9BA2D953EA4CDC84E59B1EEEE46550",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "BE9BA2D953EA4CDC84E59B1EEEE46550"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "BE9BA2D953EA4CDC84E59B1EEEE46550",
                            1
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "egpol-block-wmi-persistence",
                    Label = "Block WMI Event Subscription Persistence via ASR",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables ASR rule E6DB77E5-3DF2-4CF1-B95A-636979351E5B that blocks WMI event subscriptions from being used for malware persistence, a common technique for advanced persistent threats to survive reboots.",
                    Tags = ["exploit-guard", "asr", "wmi", "persistence", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "WMI persistence via event subscription blocked; malware cannot survive reboots via WMI subscriptions.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "E6DB77E53DF24CF1B95A636979351E5B",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "E6DB77E53DF24CF1B95A636979351E5B"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules",
                            "E6DB77E53DF24CF1B95A636979351E5B",
                            1
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "egpol-enable-eaf-plus",
                    Label = "Enable Exploit Protection Export Address Filtering Plus (EAF+)",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables the Enhanced Export Address Filtering Plus (EAF+) mitigation on svchost.exe and browser processes, which filters access to Export Address Tables to prevent shellcode from locating API function pointers during exploitation.",
                    Tags = ["exploit-guard", "eaf", "exploit-protection", "shellcode", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "EAF+ mitigation enabled; shellcode API resolution from export tables blocked in browser and system processes.",
                    ApplyOps = [RegOp.SetDword(EgKey, "EnableExAFPlus", 1)],
                    RemoveOps = [RegOp.DeleteValue(EgKey, "EnableExAFPlus")],
                    DetectOps = [RegOp.CheckDword(EgKey, "EnableExAFPlus", 1)],
                },
                new TweakDef
                {
                    Id = "egpol-log-asr-events",
                    Label = "Enable ASR Block Event Logging",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables Security event log entries (EventID 1121, 1122, 1125, 1126) for ASR rule block and audit events, providing an audit trail of all blocked exploitation attempts on the endpoint.",
                    Tags = ["exploit-guard", "asr", "event-log", "audit", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ASR block events logged; all ASR and Network Protection blocks appear in the Security event log.",
                    ApplyOps = [RegOp.SetDword(AsrKey, "EnableASREventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(AsrKey, "EnableASREventLogging")],
                    DetectOps = [RegOp.CheckDword(AsrKey, "EnableASREventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "egpol-disable-eg-telemetry",
                    Label = "Disable Exploit Guard Telemetry Reporting to Microsoft",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Prevents Windows Defender Exploit Guard from sending detailed telemetry about blocked events (process names, paths, rule IDs) to Microsoft, reducing cloud data exposure while keeping local protection active.",
                    Tags = ["exploit-guard", "telemetry", "privacy", "microsoft", "defender", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Exploit Guard telemetry to Microsoft disabled; block details not sent to cloud while protection stays on.",
                    ApplyOps = [RegOp.SetDword(EgKey, "DisableExploitGuardTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(EgKey, "DisableExploitGuardTelemetry")],
                    DetectOps = [RegOp.CheckDword(EgKey, "DisableExploitGuardTelemetry", 1)],
                },
            ];
    
    }

    // ── FirewallLogPolicy ──
    private static class _FirewallLogPolicy
    {    
        private const string DomainLog =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging";
    
        private const string PrivateLog =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging";
    
        private const string PublicLog =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging";
    
        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "fwlog-domain-log-dropped",
                Label = "Log dropped packets — Domain firewall profile (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables logging of dropped packets in the Domain firewall profile via GPO policy. "
                    + "LogDroppedPackets=1. Default: not logged. Helps detect blocked connection attempts.",
                Tags = ["firewall", "logging", "dropped", "domain", "policy"],
                ApplyOps = [RegOp.SetDword(DomainLog, "LogDroppedPackets", 1)],
                RemoveOps = [RegOp.DeleteValue(DomainLog, "LogDroppedPackets")],
                DetectOps = [RegOp.CheckDword(DomainLog, "LogDroppedPackets", 1)],
            },
            new TweakDef
            {
                Id = "fwlog-domain-log-success",
                Label = "Log successful connections — Domain firewall profile (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables logging of allowed connections in the Domain firewall profile via GPO policy. "
                    + "LogSuccessfulConnections=1. Useful for network access auditing.",
                Tags = ["firewall", "logging", "success", "domain", "policy"],
                ApplyOps = [RegOp.SetDword(DomainLog, "LogSuccessfulConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(DomainLog, "LogSuccessfulConnections")],
                DetectOps = [RegOp.CheckDword(DomainLog, "LogSuccessfulConnections", 1)],
            },
            new TweakDef
            {
                Id = "fwlog-domain-log-size",
                Label = "Set Domain firewall log size to 16 MB (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum Domain profile firewall log file size to 16 MB via GPO policy. "
                    + "LogFileSize=16384 (KB). Default: 4096 KB (4 MB). Larger logs retain more history.",
                Tags = ["firewall", "logging", "size", "domain", "policy"],
                ApplyOps = [RegOp.SetDword(DomainLog, "LogFileSize", 16384)],
                RemoveOps = [RegOp.DeleteValue(DomainLog, "LogFileSize")],
                DetectOps = [RegOp.CheckDword(DomainLog, "LogFileSize", 16384)],
            },
            new TweakDef
            {
                Id = "fwlog-private-log-dropped",
                Label = "Log dropped packets — Private firewall profile (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables logging of dropped packets in the Private (home/work) firewall profile via GPO policy. "
                    + "LogDroppedPackets=1. Helps detect unsolicited connection attempts on private networks.",
                Tags = ["firewall", "logging", "dropped", "private", "policy"],
                ApplyOps = [RegOp.SetDword(PrivateLog, "LogDroppedPackets", 1)],
                RemoveOps = [RegOp.DeleteValue(PrivateLog, "LogDroppedPackets")],
                DetectOps = [RegOp.CheckDword(PrivateLog, "LogDroppedPackets", 1)],
            },
            new TweakDef
            {
                Id = "fwlog-private-log-success",
                Label = "Log successful connections — Private firewall profile (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables logging of allowed connections in the Private firewall profile via GPO policy. "
                    + "LogSuccessfulConnections=1. Useful for home/work network access auditing.",
                Tags = ["firewall", "logging", "success", "private", "policy"],
                ApplyOps = [RegOp.SetDword(PrivateLog, "LogSuccessfulConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(PrivateLog, "LogSuccessfulConnections")],
                DetectOps = [RegOp.CheckDword(PrivateLog, "LogSuccessfulConnections", 1)],
            },
            new TweakDef
            {
                Id = "fwlog-private-log-size",
                Label = "Set Private firewall log size to 16 MB (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum Private profile firewall log file size to 16 MB via GPO policy. "
                    + "LogFileSize=16384 (KB). Default: 4096 KB.",
                Tags = ["firewall", "logging", "size", "private", "policy"],
                ApplyOps = [RegOp.SetDword(PrivateLog, "LogFileSize", 16384)],
                RemoveOps = [RegOp.DeleteValue(PrivateLog, "LogFileSize")],
                DetectOps = [RegOp.CheckDword(PrivateLog, "LogFileSize", 16384)],
            },
            new TweakDef
            {
                Id = "fwlog-public-log-dropped",
                Label = "Log dropped packets — Public firewall profile (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables logging of dropped packets in the Public firewall profile via GPO policy. "
                    + "LogDroppedPackets=1. Critical for monitoring untrusted network environments.",
                Tags = ["firewall", "logging", "dropped", "public", "policy"],
                ApplyOps = [RegOp.SetDword(PublicLog, "LogDroppedPackets", 1)],
                RemoveOps = [RegOp.DeleteValue(PublicLog, "LogDroppedPackets")],
                DetectOps = [RegOp.CheckDword(PublicLog, "LogDroppedPackets", 1)],
            },
            new TweakDef
            {
                Id = "fwlog-public-log-success",
                Label = "Log successful connections — Public firewall profile (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables logging of allowed connections in the Public (untrusted) firewall profile via GPO policy. "
                    + "LogSuccessfulConnections=1. Reveals network access on public Wi-Fi/untrusted networks.",
                Tags = ["firewall", "logging", "success", "public", "policy"],
                ApplyOps = [RegOp.SetDword(PublicLog, "LogSuccessfulConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(PublicLog, "LogSuccessfulConnections")],
                DetectOps = [RegOp.CheckDword(PublicLog, "LogSuccessfulConnections", 1)],
            },
            new TweakDef
            {
                Id = "fwlog-public-log-size",
                Label = "Set Public firewall log size to 16 MB (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum Public profile firewall log file size to 16 MB via GPO policy. "
                    + "LogFileSize=16384 (KB). Default: 4096 KB. Larger logs help with incident investigation.",
                Tags = ["firewall", "logging", "size", "public", "policy"],
                ApplyOps = [RegOp.SetDword(PublicLog, "LogFileSize", 16384)],
                RemoveOps = [RegOp.DeleteValue(PublicLog, "LogFileSize")],
                DetectOps = [RegOp.CheckDword(PublicLog, "LogFileSize", 16384)],
            },
            new TweakDef
            {
                Id = "fwlog-domain-log-file-path",
                Label = "Set Domain firewall log file to pfirewall-domain.log (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Configures a distinct log file path for the Domain firewall profile via GPO policy. "
                    + "LogFilePath=%systemroot%\\system32\\LogFiles\\Firewall\\pfirewall-domain.log. "
                    + "Default: pfirewall.log (shared with all profiles).",
                Tags = ["firewall", "logging", "path", "domain", "policy"],
                ApplyOps =
                [
                    RegOp.SetExpandString(
                        DomainLog,
                        "LogFilePath",
                        @"%systemroot%\system32\LogFiles\Firewall\pfirewall-domain.log"
                    ),
                ],
                RemoveOps = [RegOp.DeleteValue(DomainLog, "LogFilePath")],
                DetectOps =
                [
                    RegOp.CheckString(
                        DomainLog,
                        "LogFilePath",
                        @"%systemroot%\system32\LogFiles\Firewall\pfirewall-domain.log"
                    ),
                ],
            },
        ];
    
    }

    // ── FirewallProfileHardeningPolicy ──
    private static class _FirewallProfileHardeningPolicy
    {    
        private const string DomainKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
        private const string PrivKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
        private const string PubKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";
        private const string DomLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging";
        private const string PubLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fwprof-stealth-mode-private",
                    Label = "Enable Firewall Stealth Mode on Private Networks",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables stealth mode on the Private network profile, causing blocked inbound connection attempts to be silently dropped rather than returning RST/ICMP-unreachable, hiding this machine from port-scanner reconnaissance.",
                    Tags = ["firewall", "stealth-mode", "private-profile", "port-scan", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Stealth mode on private networks; blocked ports silent. Machine harder to discover on home/office networks.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "DisableStealthMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "DisableStealthMode")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "DisableStealthMode", 0)],
                },
                new TweakDef
                {
                    Id = "fwprof-stealth-mode-domain",
                    Label = "Enable Firewall Stealth Mode on Domain Networks",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables stealth mode on the Domain network profile so that blocked inbound connection attempts are silently dropped on corporate networks, reducing noise and lateral-movement reconnaissance surface.",
                    Tags = ["firewall", "stealth-mode", "domain-profile", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Stealth mode on domain networks; blocked ports drop silently on corporate LAN.",
                    ApplyOps = [RegOp.SetDword(DomainKey, "DisableStealthMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(DomainKey, "DisableStealthMode")],
                    DetectOps = [RegOp.CheckDword(DomainKey, "DisableStealthMode", 0)],
                },
                new TweakDef
                {
                    Id = "fwprof-block-local-merge-private",
                    Label = "Block Local Firewall Rules from Overriding GPO on Private Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Prevents locally defined firewall rules from overriding Group Policy rules on the Private profile, ensuring GPO-deployed firewall rules cannot be undermined by applications or malware creating local exceptions.",
                    Tags = ["firewall", "local-merge", "gpo", "private-profile", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local rule merge blocked on private profile; only GPO rules apply. Local app exceptions cannot override.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AllowLocalPolicyMerge", 0)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AllowLocalPolicyMerge")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AllowLocalPolicyMerge", 0)],
                },
                new TweakDef
                {
                    Id = "fwprof-disable-notifications-private",
                    Label = "Disable Firewall Blocked App Notifications on Private Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Suppresses the Windows Firewall notification that prompts users to approve newly blocked applications on Private networks, preventing non-admin users from weakening firewall policy via approval notifications.",
                    Tags = ["firewall", "notification", "blocked-app", "private-profile", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocked app notifications suppressed on private profile; users cannot approve exceptions via notification.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "DisableNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "DisableNotifications")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "DisableNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "fwprof-log-dropped-packets-public",
                    Label = "Log Dropped Packets on Public Firewall Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables firewall logging of dropped packet events on the Public profile, recording all blocked inbound/outbound connection attempts on public networks for security incident investigation.",
                    Tags = ["firewall", "log-dropped", "public-profile", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Dropped packets logged on public profile; blocked connections recorded in public profile firewall log.",
                    ApplyOps = [RegOp.SetDword(PubLog, "LogDroppedPackets", 1)],
                    RemoveOps = [RegOp.DeleteValue(PubLog, "LogDroppedPackets")],
                    DetectOps = [RegOp.CheckDword(PubLog, "LogDroppedPackets", 1)],
                },
                new TweakDef
                {
                    Id = "fwprof-log-allowed-packets-public",
                    Label = "Log Allowed Connections on Public Firewall Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Enables firewall logging of successfully allowed connections on the Public profile, providing a record of all established connections on public networks for behavioural baselining and anomaly detection.",
                    Tags = ["firewall", "log-allowed", "public-profile", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Allowed connections logged on public profile; all successful outbound traffic recorded in firewall log.",
                    ApplyOps = [RegOp.SetDword(PubLog, "LogSuccessfulConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(PubLog, "LogSuccessfulConnections")],
                    DetectOps = [RegOp.CheckDword(PubLog, "LogSuccessfulConnections", 1)],
                },
                new TweakDef
                {
                    Id = "fwprof-set-log-max-size-domain",
                    Label = "Set Maximum Firewall Log Size to 32 MB on Domain Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets the maximum firewall log file size to 32768 KB (32 MB) on the Domain profile, ensuring sufficient log retention for forensic investigation without unbounded disk consumption.",
                    Tags = ["firewall", "log-size", "domain-profile", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Domain profile firewall log capped at 32 MB; adequate retention for investigation without disk overflow.",
                    ApplyOps = [RegOp.SetDword(DomLog, "LogFileSize", 32768)],
                    RemoveOps = [RegOp.DeleteValue(DomLog, "LogFileSize")],
                    DetectOps = [RegOp.CheckDword(DomLog, "LogFileSize", 32768)],
                },
                new TweakDef
                {
                    Id = "fwprof-set-log-max-size-public",
                    Label = "Set Maximum Firewall Log Size to 32 MB on Public Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets the maximum firewall log file size to 32768 KB (32 MB) on the Public profile, ensuring that firewall log entries on untrusted public networks are retained for post-incident forensic analysis.",
                    Tags = ["firewall", "log-size", "public-profile", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Public profile firewall log capped at 32 MB; public network activity retained for post-incident review.",
                    ApplyOps = [RegOp.SetDword(PubLog, "LogFileSize", 32768)],
                    RemoveOps = [RegOp.DeleteValue(PubLog, "LogFileSize")],
                    DetectOps = [RegOp.CheckDword(PubLog, "LogFileSize", 32768)],
                },
                new TweakDef
                {
                    Id = "fwprof-unicast-no-response-private",
                    Label = "Disable Unicast Responses to Multicast on Private Profile",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Prevents the firewall from sending unicast replies to multicast and broadcast packets on the Private profile, closing a live-host detection technique used by network scanners that evade ICMP filtering.",
                    Tags = ["firewall", "unicast-response", "multicast", "private-profile", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Unicast responses to multicast/broadcast disabled on private profile; host enumeration vector closed.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "DisableUnicastResponsesToMulticastBroadcast")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                },
                new TweakDef
                {
                    Id = "fwprof-block-ipsec-exempt-multicast",
                    Label = "Block IPsec Exemption for Multicast and Broadcast Traffic",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Removes the default IPsec exemption that allows multicast and broadcast traffic to bypass IPsec policy enforcement, ensuring all traffic — including multicast — is subject to IPsec rules on protected networks.",
                    Tags = ["firewall", "ipsec", "multicast", "exemption", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "IPsec multicast/broadcast exemption removed; multicast traffic subject to IPsec enforcement. May break mDNS.",
                    ApplyOps = [RegOp.SetDword(DomainKey, "IPsecExemptMulticast", 0)],
                    RemoveOps = [RegOp.DeleteValue(DomainKey, "IPsecExemptMulticast")],
                    DetectOps = [RegOp.CheckDword(DomainKey, "IPsecExemptMulticast", 0)],
                },
            ];
    
    }

    // ── SmartControlBypassPolicy ──
    private static class _SmartControlBypassPolicy
    {    
        private const string PsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
    
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    
        private const string WscriptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\WScript";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sacbyp-enable-powershell-constrained-mode",
                    Label = "Bypass Prevention: Enable PowerShell Constrained Language Mode",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableScripts=1 and ScriptBlockLogging=1 in PowerShell policy, and sets ConstrainedLanguageMode=constrained. PowerShell is the most commonly used Living-off-the-Land binary (LOLBin) for fileless malware and post-exploitation operations. Constrained Language Mode (CLM) is a PowerShell execution environment that restricts the .NET types and methods available to PowerShell scripts, preventing common attack patterns that use PowerShell to reflectively load .NET assemblies, access Win32 APIs via P/Invoke, or bypass AppLocker/WDAC. CLM is automatically entered when WDAC UMCI is active.",
                    Tags = ["powershell", "constrained-mode", "lolbin", "fileless", "bypass-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "PowerShell enters Constrained Language Mode for unprivileged sessions. Many .NET types and methods are unavailable. Scripts that use reflection, P/Invoke, or advanced .NET classes break in CLM. Admin-run sessions may remain in Full Language Mode depending on WDAC policy. Test all scheduled task and automation scripts before deploying.",
                    ApplyOps = [RegOp.SetDword(PsKey, "EnableScripts", 1), RegOp.SetDword(PsKey, "ScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsKey, "EnableScripts"), RegOp.DeleteValue(PsKey, "ScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(PsKey, "ScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-enable-powershell-transcription",
                    Label = "Bypass Prevention: Enable PowerShell Session Transcription",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableTranscripting=1 in PowerShell policy. Enables PowerShell transcription, which writes a full text log of every command run in every PowerShell session to a configured directory. Transcripts capture both input commands and output, providing a complete audit trail of PowerShell activity. Attackers who use PowerShell as a LOLBin leave a full transcript of their commands — enabling forensic reconstruction of the attack chain. Transcripts are written to %TMP% by default, but can be redirected to a network share via OutputDirectory policy.",
                    Tags = ["powershell", "transcription", "audit", "forensics", "lolbin"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "All PowerShell sessions are transcribed. Transcript files are written to the configured directory (default: %TMP%). Disk usage varies by activity level. Transcripts contain all command output including potential sensitive data — protect the transcript directory with appropriate ACLs.",
                    ApplyOps = [RegOp.SetDword(PsKey, "EnableTranscripting", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsKey, "EnableTranscripting")],
                    DetectOps = [RegOp.CheckDword(PsKey, "EnableTranscripting", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-enable-powershell-module-logging",
                    Label = "Bypass Prevention: Enable PowerShell Module Event Logging",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets EnableModuleLogging=1 in PowerShell policy. Enables module-level logging that records every pipeline execution event in the PowerShell event log. Module logging generates events in the Microsoft-Windows-PowerShell/Operational channel for every command, expression, and function call — regardless of whether the script is obfuscated, base64-encoded, or dynamically generated. Even heavily obfuscated PowerShell attack chains are recorded during execution. Module logging is one of the most effective detections for PowerShell-based attacks and is recommended by CISA, NSA, and MITRE ATT&CK.",
                    Tags = ["powershell", "module-logging", "event-log", "obfuscation", "detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "All PowerShell module pipeline executions are logged. Obfuscated code is logged after de-obfuscation by the PowerShell engine — attackers cannot evade module logging through obfuscation alone. High-volume PowerShell environments generate large event log volumes. Set a sufficient log retention size.",
                    ApplyOps = [RegOp.SetDword(PsKey, "EnableModuleLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(PsKey, "EnableModuleLogging")],
                    DetectOps = [RegOp.CheckDword(PsKey, "EnableModuleLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-require-signed-powershell-scripts",
                    Label = "Bypass Prevention: Require All PowerShell Scripts to be Digitally Signed",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets ExecutionPolicy=4 in PowerShell policy (AllSigned mode). Requires all PowerShell scripts — both local and remote — to be signed by a trusted code signing certificate. Unsigned scripts are blocked regardless of origin. AllSigned is stronger than RemoteSigned (which only requires signing for remotely downloaded scripts) because attackers who drop scripts locally via exploitation still cannot execute unsigned scripts. Combined with WDAC, AllSigned execution policy adds a complementary user-space policy enforcement layer for PowerShell scripts that WDAC's binary policy may not cover.",
                    Tags = ["powershell", "allsigned", "execution-policy", "code-signing", "script-blocking"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "All PowerShell scripts must be digitally signed. Scripts without a valid signature from a trusted CA are blocked. All corporate PowerShell scripts (scheduled tasks, admin tools, deployment scripts) must be signed before enabling. Interactive one-liners entered in a terminal session are not affected.",
                    ApplyOps = [RegOp.SetDword(PsKey, "ExecutionPolicy", 4)],
                    RemoveOps = [RegOp.DeleteValue(PsKey, "ExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(PsKey, "ExecutionPolicy", 4)],
                },
                new TweakDef
                {
                    Id = "sacbyp-disable-wscript-vbscript",
                    Label = "Bypass Prevention: Disable Windows Script Host (VBScript/JScript)",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets Enabled=0 in WScript policy. Disables the Windows Script Host (wscript.exe, cscript.exe) which is used to execute VBScript (.vbs) and JScript (.js) files. WSH is a legacy scripting environment that has no legitimate use in modern enterprise environments. It is extensively used by malware as a malware delivery mechanism — malware authors distribute .vbs or .js files via email attachments, and when clicked, these files execute via WSH without requiring any additional tools. Disabling WSH eliminates this entire attack vector.",
                    Tags = ["wscript", "vbscript", "jscript", "disable", "attack-surface-reduction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "wscript.exe and cscript.exe cannot execute VBScript or JScript files. Legacy VBScript-based login scripts or software deployment scripts will break. Audit WSH script usage before disabling. Most modern environments have zero legitimate WSH usage and will see no disruption.",
                    ApplyOps = [RegOp.SetDword(SysKey + @"\WScript", "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(SysKey + @"\WScript", "Enabled")],
                    DetectOps = [RegOp.CheckDword(SysKey + @"\WScript", "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "sacbyp-block-mshta-execution",
                    Label = "Bypass Prevention: Block mshta.exe HTA Script Execution",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets DisableMshtaExecution=1 in System policy. Blocks mshta.exe (Microsoft HTML Application Host) from executing HTML Application (.hta) files. HTA files run with full user-mode permissions and can execute arbitrary JavaScript/VBScript with access to all COM objects and WScript. MSHTA is one of the most common LOLBins — it is frequently used to bypass application whitelisting because mshta.exe is a legitimate signed Microsoft binary. Blocking HTA execution eliminates this bypass vector while having no impact on standard web browsing or Office applications.",
                    Tags = ["mshta", "hta", "lolbin", "bypass", "application-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "mshta.exe cannot execute .hta files. Attempts to open HTAs via double-click, command line, or browser navigation result in a blocked execution. Legacy HTA-based internal web apps or admin tools may break. Survey HTA usage via AppLocker or WDAC audit logs before blocking.",
                    ApplyOps = [RegOp.SetDword(SysKey, "DisableMshtaExecution", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "DisableMshtaExecution")],
                    DetectOps = [RegOp.CheckDword(SysKey, "DisableMshtaExecution", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-block-regsvr32-remote-script-load",
                    Label = "Bypass Prevention: Block regsvr32.exe Remote COM Script Loading",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets BlockRegsvr32RemoteLoad=1 in System policy. Blocks regsvr32.exe from loading and registering COM objects from remote URLs (the 'Squiblydoo' technique). The Squiblydoo bypass uses regsvr32.exe — which is allowed in nearly all application whitelist environments — to download and execute a remote Script Component (.sct) file, completely bypassing AppLocker and WDAC default rules. This is documented in ATT&CK as T1218.010. Disabling remote URL registration for regsvr32 neutralises this bypass while preserving the ability to register local DLLs normally.",
                    Tags = ["regsvr32", "squiblydoo", "com", "lolbin", "remote-execution"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "regsvr32.exe cannot load COM objects from remote URLs. Local DLL registration (regsvr32.exe mylib.dll) is not affected. The Squiblydoo ATT&CK technique (T1218.010) is neutralised. No legitimate corporate workflow uses regsvr32 with remote URLs.",
                    ApplyOps = [RegOp.SetDword(SysKey, "BlockRegsvr32RemoteLoad", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "BlockRegsvr32RemoteLoad")],
                    DetectOps = [RegOp.CheckDword(SysKey, "BlockRegsvr32RemoteLoad", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-disable-wmi-script-execution",
                    Label = "Bypass Prevention: Restrict WMI Script Execution Namespace",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets RestrictAnonymousWmiAccess=1 in System policy. Restricts anonymous and remote WMI script execution that bypasses application control by using WMI's scripting interface to spawn processes. WMI process execution via Win32_Process.Create() is widely used in malware and post-exploitation tools (including Impacket, wmiexec, PowerSploit) because WMI-spawned processes often bypass application whitelisting tools that focus on cmd.exe or PowerShell as parent processes. Restricting WMI script namespace access reduces this bypass surface.",
                    Tags = ["wmi", "script-bypass", "win32-process", "impacket", "lolbin"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Anonymous and script-initiated WMI process creation is restricted. WMI management from the SCCM client, PowerShell admin scripts, and monitoring tools may be affected if they use WMI namespace access. Evaluate WMI usage in your environment before enabling. Administrative WMI with explicit credentials is less affected.",
                    ApplyOps = [RegOp.SetDword(SysKey, "RestrictAnonymousWmiAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "RestrictAnonymousWmiAccess")],
                    DetectOps = [RegOp.CheckDword(SysKey, "RestrictAnonymousWmiAccess", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-enable-process-creation-auditing",
                    Label = "Bypass Prevention: Enable Process Creation Command-Line Auditing",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets AuditProcessCreation=1 in System policy. Enables auditing of process creation events with full command-line arguments (Security Event ID 4688). Command-line process creation auditing is essential for detecting LOLBin abuse: every invocation of powershell.exe, cmd.exe, mshta.exe, wscript.exe, certutil.exe, bitsadmin.exe, and other commonly abused binaries is logged with the full command line. SIEM rules can detect known malicious patterns (base64-encoded payloads, download cradles, MSHTA remote URLs) in real time by parsing Event ID 4688 command lines.",
                    Tags = ["process-creation", "audit", "command-line", "4688", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Process creation with full command-line arguments is logged as Security Event ID 4688. Command lines can contain sensitive data (passwords passed as arguments). Ensure security event log is forwarded to a secure SIEM with access controlled to security personnel only. Moderate log volume increase in busy environments.",
                    ApplyOps = [RegOp.SetDword(SysKey, "AuditProcessCreation", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "AuditProcessCreation")],
                    DetectOps = [RegOp.CheckDword(SysKey, "AuditProcessCreation", 1)],
                },
                new TweakDef
                {
                    Id = "sacbyp-block-certutil-download",
                    Label = "Bypass Prevention: Block certutil.exe Download and Decode Functions",
                    Category = "Defender & Antivirus Policy",
                    Description =
                        "Sets BlockCertutilDownload=1 in System policy. Restricts certutil.exe from being used as a download tool or base64 decoder. Certutil is a legitimate Windows certificate management tool that has been widely abused as a LOLBin to download files from the internet (certutil -urlcache -f http://...) and to decode base64-encoded payloads (certutil -decode). Both operations are used extensively in malware download stagers because certutil is whitelisted by nearly all application control solutions. Blocking these specific certutil sub-functions does not affect its legitimate certificate management role.",
                    Tags = ["certutil", "download", "base64-decode", "lolbin", "bypass-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "certutil.exe cannot be used to download files from URLs or decode base64 payloads. Legitimate certificate management functions (import, export, verify, display) are not affected. If your scripts use certutil for base64 decoding, replace with PowerShell [Convert]::FromBase64String() before enabling.",
                    ApplyOps = [RegOp.SetDword(SysKey, "BlockCertutilDownload", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "BlockCertutilDownload")],
                    DetectOps = [RegOp.CheckDword(SysKey, "BlockCertutilDownload", 1)],
                },
            ];
    
    }

    // ── SmartScreenAdvancedPolicy ──
    private static class _SmartScreenAdvancedPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter";
        private const string ExplKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ssadv-enable-smartscreen-shell",
                    Label = "Enable SmartScreen for Apps and Files in Windows Shell",
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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
                    Category = "Defender & Antivirus Policy",
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

    // ── SmartScreenPolicy ──
    private static class _SmartScreenPolicy
    {    
        private const string WinSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string DefSS = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen";
        private const string IEPhish = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter";
        private const string EdgePol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";
    
        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smartscr-enable-shell",
                Label = "Enable Windows SmartScreen (shell)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables Windows Defender SmartScreen for file and app checks in Explorer/shell via GPO. "
                    + "EnableSmartScreen=1. Default: enabled. Recommended: enforced via policy.",
                Tags = ["smartscreen", "shell", "security", "policy"],
                ApplyOps = [RegOp.SetDword(WinSys, "EnableSmartScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(WinSys, "EnableSmartScreen")],
                DetectOps = [RegOp.CheckDword(WinSys, "EnableSmartScreen", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-shell-block-level",
                Label = "Set SmartScreen shell level to Block",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Forces SmartScreen to block unknown/malicious files instead of just warning. " + "ShellSmartScreenLevel=Block. Default: Warn.",
                Tags = ["smartscreen", "shell", "block", "policy"],
                ApplyOps = [RegOp.SetString(WinSys, "ShellSmartScreenLevel", "Block")],
                RemoveOps = [RegOp.DeleteValue(WinSys, "ShellSmartScreenLevel")],
                DetectOps = [RegOp.CheckString(WinSys, "ShellSmartScreenLevel", "Block")],
            },
            new TweakDef
            {
                Id = "smartscr-app-install-control-enabled",
                Label = "Enable Defender SmartScreen app install control",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Activates the policy that controls app installation via Windows Defender SmartScreen. "
                    + "ConfigureAppInstallControlEnabled=1. Required before ConfigureAppInstallControl takes effect.",
                Tags = ["smartscreen", "defender", "app", "install", "policy"],
                ApplyOps = [RegOp.SetDword(DefSS, "ConfigureAppInstallControlEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DefSS, "ConfigureAppInstallControlEnabled")],
                DetectOps = [RegOp.CheckDword(DefSS, "ConfigureAppInstallControlEnabled", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-recommend-store-only",
                Label = "Warn on non-Store app installs (SmartScreen)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Configures SmartScreen to recommend the Microsoft Store and warn on non-Store app installs. "
                    + "ConfigureAppInstallControl=Recommend. Use StoreOnly to block all non-Store apps.",
                Tags = ["smartscreen", "defender", "store", "install", "policy"],
                ApplyOps = [RegOp.SetString(DefSS, "ConfigureAppInstallControl", "Recommend")],
                RemoveOps = [RegOp.DeleteValue(DefSS, "ConfigureAppInstallControl")],
                DetectOps = [RegOp.CheckString(DefSS, "ConfigureAppInstallControl", "Recommend")],
            },
            new TweakDef
            {
                Id = "smartscr-ie-phishing-filter",
                Label = "Enable IE/Edge Legacy SmartScreen phishing filter",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables the IE/Edge Legacy SmartScreen phishing filter via policy. "
                    + "EnabledV9=1. Blocks known phishing sites and malware downloads.",
                Tags = ["smartscreen", "ie", "phishing", "policy"],
                ApplyOps = [RegOp.SetDword(IEPhish, "EnabledV9", 1)],
                RemoveOps = [RegOp.DeleteValue(IEPhish, "EnabledV9")],
                DetectOps = [RegOp.CheckDword(IEPhish, "EnabledV9", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-ie-prevent-site-override",
                Label = "Prevent user bypassing SmartScreen for malicious sites",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents users from bypassing SmartScreen filter warnings for malicious websites in IE/Edge Legacy. " + "PreventOverride=1.",
                Tags = ["smartscreen", "ie", "override", "policy"],
                ApplyOps = [RegOp.SetDword(IEPhish, "PreventOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(IEPhish, "PreventOverride")],
                DetectOps = [RegOp.CheckDword(IEPhish, "PreventOverride", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-ie-prevent-app-rep-override",
                Label = "Prevent user bypassing SmartScreen for unknown apps",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents users from bypassing SmartScreen warnings for files with unknown application reputation. "
                    + "PreventOverrideAppRepUnknown=1.",
                Tags = ["smartscreen", "ie", "app-rep", "override", "policy"],
                ApplyOps = [RegOp.SetDword(IEPhish, "PreventOverrideAppRepUnknown", 1)],
                RemoveOps = [RegOp.DeleteValue(IEPhish, "PreventOverrideAppRepUnknown")],
                DetectOps = [RegOp.CheckDword(IEPhish, "PreventOverrideAppRepUnknown", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-edge-enable",
                Label = "Enable Microsoft Edge SmartScreen (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables Windows Defender SmartScreen in Microsoft Edge via managed GPO policy. "
                    + "SmartScreenEnabled=1. Recommended: always enforced.",
                Tags = ["smartscreen", "edge", "security", "policy"],
                ApplyOps = [RegOp.SetDword(EdgePol, "SmartScreenEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgePol, "SmartScreenEnabled")],
                DetectOps = [RegOp.CheckDword(EdgePol, "SmartScreenEnabled", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-edge-pua-block",
                Label = "Enable Edge SmartScreen PUA blocking (policy)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables Microsoft Edge SmartScreen detection and blocking of Potentially Unwanted Applications (PUA). " + "SmartScreenPuaEnabled=1.",
                Tags = ["smartscreen", "edge", "pua", "policy"],
                ApplyOps = [RegOp.SetDword(EdgePol, "SmartScreenPuaEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgePol, "SmartScreenPuaEnabled")],
                DetectOps = [RegOp.CheckDword(EdgePol, "SmartScreenPuaEnabled", 1)],
            },
            new TweakDef
            {
                Id = "smartscr-edge-force-enabled",
                Label = "Force-enable Edge SmartScreen (user cannot disable)",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Forces Edge SmartScreen on and prevents users from turning it off. "
                    + "SmartScreenForceEnabled=1. Stronger than SmartScreenEnabled alone.",
                Tags = ["smartscreen", "edge", "force", "policy"],
                ApplyOps = [RegOp.SetDword(EdgePol, "SmartScreenForceEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgePol, "SmartScreenForceEnabled")],
                DetectOps = [RegOp.CheckDword(EdgePol, "SmartScreenForceEnabled", 1)],
            },
        ];
    
    }

    // ── WebThreatDefensePolicy ──
    private static class _WebThreatDefensePolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebThreatDefense";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wtd-disable-service",
                Label = "Disable Web Threat Defense Service",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Sets ServiceEnabled=0 in the WebThreatDefense policy key. Disables the "
                    + "Windows Web Threat Defense service, which provides reputation-based "
                    + "protection for URLs and executables accessed via Edge and other browsers. "
                    + "The service contacts Microsoft cloud to evaluate link safety in real time. "
                    + "Default: 1 (service enabled). Recommended: 0 when using a third-party "
                    + "URL filtering solution or zero-trust network access proxy.",
                Tags = ["web-threat-defense", "smartscreen", "cloud", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ServiceEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ServiceEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ServiceEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wtd-lock-ui",
                Label = "Lock Web Threat Defense UI Toggle",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets UILockdown=1 in the WebThreatDefense policy key. Prevents end users "
                    + "from toggling the reputation-based protection setting in Windows Security "
                    + "→ App & browser control. The toggle remains visible but is greyed out. "
                    + "Ensures that the administrator-configured state cannot be changed without "
                    + "elevated privileges. Default: 0. Recommended: 1 in managed environments.",
                Tags = ["web-threat-defense", "ui", "lockdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UILockdown", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UILockdown")],
                DetectOps = [RegOp.CheckDword(Key, "UILockdown", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-phishing-filter",
                Label = "Disable Web Threat Defense Phishing Filter",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisablePhishingFilter=1 in the WebThreatDefense policy key. Stops "
                    + "real-time checks against Microsoft's phishing site list for URLs accessed "
                    + "in the browser. Environments using a network proxy, DNS sinkhole, or "
                    + "zero-trust access gateway that provides phishing protection at a lower "
                    + "level may find this check redundant. "
                    + "Default: 0. Recommended: only with compensating network-layer controls.",
                Tags = ["web-threat-defense", "phishing", "filter", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePhishingFilter", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhishingFilter")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhishingFilter", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-malicious-url-block",
                Label = "Disable Web Threat Defense Malicious URL Blocking",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableMaliciousURLBlock=1 in the WebThreatDefense policy key. "
                    + "Prevents Windows from blocking navigation to URLs that Microsoft's threat "
                    + "intelligence has flagged as distributing malware. In research, sandboxed, "
                    + "or security-testing environments that intentionally access known-bad URLs, "
                    + "this block is an impediment. "
                    + "Default: 0. Recommended: only in isolated research environments.",
                Tags = ["web-threat-defense", "malicious-url", "block", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMaliciousURLBlock", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMaliciousURLBlock")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMaliciousURLBlock", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-download-reputation",
                Label = "Disable Web Threat Defense Download Reputation",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableDownloadReputation=1 in the WebThreatDefense policy key. "
                    + "Disables reputation lookups for executables and archives downloaded from "
                    + "the internet. Without reputation checks, unsigned or newly-published files "
                    + "are no longer blocked automatically. "
                    + "Default: 0 (checks enabled). Recommended: 1 only in developer or "
                    + "air-gapped environments where cloud lookups are impractical.",
                Tags = ["web-threat-defense", "download", "reputation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDownloadReputation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDownloadReputation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDownloadReputation", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-cloud-check",
                Label = "Disable Web Threat Defense Cloud Lookup",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableCloudLookup=1 in the WebThreatDefense policy key. Prevents "
                    + "the WTD service from contacting Microsoft cloud endpoints to evaluate "
                    + "URL reputation at browse time. All evaluations fall back to local lists "
                    + "only. Reduces outgoing connections to Microsoft but degrades freshness "
                    + "of threat intelligence. "
                    + "Default: 0. Recommended: 1 in strict outbound firewall environments.",
                Tags = ["web-threat-defense", "cloud", "lookup", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudLookup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudLookup")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudLookup", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-behaviour-monitoring",
                Label = "Disable Web Threat Defense Behaviour Monitoring",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableBehaviourMonitoring=1 in the WebThreatDefense policy key. "
                    + "Disables heuristic behaviour analysis of browser sessions performed "
                    + "by the Web Threat Defense engine. Behaviour monitoring catches "
                    + "zero-day exploits that don't match static URL signatures but adds "
                    + "browser overhead. Default: 0. Recommended: 1 when browser performance "
                    + "is critical and alternative EDR covers exploit detection.",
                Tags = ["web-threat-defense", "behaviour", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBehaviourMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBehaviourMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBehaviourMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-telemetry-upload",
                Label = "Disable Web Threat Defense Telemetry Upload",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableTelemetryUpload=1 in the WebThreatDefense policy key. Prevents "
                    + "the WTD service from uploading URL visit patterns, block events, and "
                    + "engine statistics to Microsoft's security telemetry pipeline. This data "
                    + "helps improve threat intelligence but is transmitted outside the standard "
                    + "diagnostic data consent. Default: 0. Recommended: 1 for privacy.",
                Tags = ["web-threat-defense", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryUpload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryUpload")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryUpload", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-enhanced-protection",
                Label = "Disable Web Threat Defense Enhanced Protection Mode",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableEnhancedProtection=1 in the WebThreatDefense policy key. Opts "
                    + "the device out of Enhanced Protection mode, which sends more URL data to "
                    + "Microsoft for deeper analysis. Standard protection uses local models only; "
                    + "enhanced protection requires cloud connectivity and shares browsing context. "
                    + "Default: 0 (enhanced enabled when opted in). Recommended: 1 for "
                    + "privacy-first configurations.",
                Tags = ["web-threat-defense", "enhanced", "protection", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEnhancedProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEnhancedProtection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEnhancedProtection", 1)],
            },
            new TweakDef
            {
                Id = "wtd-disable-credential-warning",
                Label = "Disable Web Threat Defense Credential Entry Warning",
                Category = "Defender & Antivirus Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableCredentialWarning=1 in the WebThreatDefense policy key. Turns "
                    + "off the browser warning displayed when Windows detects that a user is "
                    + "entering corporate credentials on a potentially phishing or non-corporate "
                    + "site. In environments where users authenticate via SSO or SAML, these "
                    + "warnings can appear falsely on legitimate third-party login pages. "
                    + "Default: 0. Recommended: 1 when SSO eliminates manual credential entry.",
                Tags = ["web-threat-defense", "credential", "warning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCredentialWarning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCredentialWarning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCredentialWarning", 1)],
            },
        ];
    
    }

    // ── WindowsFirewallPolicy ──
    private static class _WindowsFirewallPolicy
    {    
        private const string FwBase = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall";
        private const string DomainKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
        private const string PrivateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
        private const string PublicKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";
    
        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "fwpol-enable-domain-profile",
                Label = "Enable Firewall on Domain Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Ensures Windows Defender Firewall is active on Domain network profile connections.",
                Tags = ["firewall", "domain", "profile", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Firewall enforced on domain-joined networks; prevents admin from disabling it without policy change.",
                ApplyOps = [RegOp.SetDword(DomainKey, "EnableFirewall", 1)],
                RemoveOps = [RegOp.DeleteValue(DomainKey, "EnableFirewall")],
                DetectOps = [RegOp.CheckDword(DomainKey, "EnableFirewall", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-enable-private-profile",
                Label = "Enable Firewall on Private Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Ensures Windows Defender Firewall is active on Private (trusted home/work) network profile connections.",
                Tags = ["firewall", "private", "profile", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Firewall enforced on private networks; required for defence-in-depth on non-domain devices.",
                ApplyOps = [RegOp.SetDword(PrivateKey, "EnableFirewall", 1)],
                RemoveOps = [RegOp.DeleteValue(PrivateKey, "EnableFirewall")],
                DetectOps = [RegOp.CheckDword(PrivateKey, "EnableFirewall", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-enable-public-profile",
                Label = "Enable Firewall on Public Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Ensures Windows Defender Firewall is active on Public (untrusted) network profile connections.",
                Tags = ["firewall", "public", "profile", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Firewall enforced on public networks; highest risk profile; critical for laptops on hotel/café Wi-Fi.",
                ApplyOps = [RegOp.SetDword(PublicKey, "EnableFirewall", 1)],
                RemoveOps = [RegOp.DeleteValue(PublicKey, "EnableFirewall")],
                DetectOps = [RegOp.CheckDword(PublicKey, "EnableFirewall", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-block-inbound-domain",
                Label = "Block All Inbound Connections on Domain Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Sets the default inbound action to block all inbound connections on the domain network profile.",
                Tags = ["firewall", "domain", "inbound", "block", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks all unsolicited inbound traffic on domain networks; explicit allow rules required for managed services.",
                ApplyOps = [RegOp.SetDword(DomainKey, "DefaultInboundAction", 1)],
                RemoveOps = [RegOp.DeleteValue(DomainKey, "DefaultInboundAction")],
                DetectOps = [RegOp.CheckDword(DomainKey, "DefaultInboundAction", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-block-inbound-public",
                Label = "Block All Inbound Connections on Public Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Sets the default action to block all inbound connections on the Public network profile.",
                Tags = ["firewall", "public", "inbound", "block", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Blocks all inbound on public networks; most secure setting for untrusted/mobile scenarios.",
                ApplyOps = [RegOp.SetDword(PublicKey, "DefaultInboundAction", 1)],
                RemoveOps = [RegOp.DeleteValue(PublicKey, "DefaultInboundAction")],
                DetectOps = [RegOp.CheckDword(PublicKey, "DefaultInboundAction", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-no-local-rules-domain",
                Label = "Prevent Local Firewall Rules on Domain Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Disallows local administrators from creating firewall allow-rules that bypass Group Policy domain profile rules.",
                Tags = ["firewall", "domain", "local-rules", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Only GP-delivered firewall rules apply on the Domain profile; local exceptions are ignored.",
                ApplyOps = [RegOp.SetDword(DomainKey, "AllowLocalPolicyMerge", 0)],
                RemoveOps = [RegOp.DeleteValue(DomainKey, "AllowLocalPolicyMerge")],
                DetectOps = [RegOp.CheckDword(DomainKey, "AllowLocalPolicyMerge", 0)],
            },
            new TweakDef
            {
                Id = "fwpol-no-local-rules-public",
                Label = "Prevent Local Firewall Rules on Public Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Disallows local firewall rule creation on the Public profile, enforcing only policy-delivered rules.",
                Tags = ["firewall", "public", "local-rules", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "GP rules are authoritative on public networks; local admin cannot weaken public profile protection.",
                ApplyOps = [RegOp.SetDword(PublicKey, "AllowLocalPolicyMerge", 0)],
                RemoveOps = [RegOp.DeleteValue(PublicKey, "AllowLocalPolicyMerge")],
                DetectOps = [RegOp.CheckDword(PublicKey, "AllowLocalPolicyMerge", 0)],
            },
            new TweakDef
            {
                Id = "fwpol-unicast-response-domain",
                Label = "Disable Unicast Response to Multicast on Domain Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Prevents the firewall from allowing unicast responses to multicast/broadcast packets on Domain networks.",
                Tags = ["firewall", "domain", "multicast", "unicast", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Reduces information disclosure via multicast; may affect some network discovery features.",
                ApplyOps = [RegOp.SetDword(DomainKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(DomainKey, "DisableUnicastResponsesToMulticastBroadcast")],
                DetectOps = [RegOp.CheckDword(DomainKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-unicast-response-public",
                Label = "Disable Unicast Response to Multicast on Public Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Prevents the firewall from sending unicast responses to multicast/broadcast probes on Public networks.",
                Tags = ["firewall", "public", "multicast", "unicast", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Helps hide the device on public Wi-Fi; reduces exposure to broadcast-based network enumeration.",
                ApplyOps = [RegOp.SetDword(PublicKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(PublicKey, "DisableUnicastResponsesToMulticastBroadcast")],
                DetectOps = [RegOp.CheckDword(PublicKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "fwpol-disable-notifications-public",
                Label = "Disable Firewall Notifications on Public Profile",
                Category = "Defender & Antivirus Policy",
                Description = "Suppresses Windows Defender Firewall blocked-connection notifications when on a Public network profile.",
                Tags = ["firewall", "public", "notifications", "policy", "ui"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks pop-up notifications for blocked connections; users won't see firewall alert dialogs.",
                ApplyOps = [RegOp.SetDword(PublicKey, "DisableNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(PublicKey, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(PublicKey, "DisableNotifications", 1)],
            },
        ];
    
    }

}
