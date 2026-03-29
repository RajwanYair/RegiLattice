// RegiLattice.Core — Tweaks/AsrAttackSurfacePolicy.cs
// Attack Surface Reduction (ASR) Rules Policy — Sprint 532.
// Controls Windows Defender Exploit Guard Attack Surface Reduction rules via Group Policy.
// ASR rules block behaviors commonly used by malware: Office macros spawning child
// processes, obfuscated scripts, credential theft, and ransomware-style file modifications.
// Category: "Defender ASR Policy" | Slug: asr
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AsrAttackSurfacePolicy
{
    private const string AsrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules";

    private const string AsrBaseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "asr-block-office-child-process",
                Label = "ASR: Block Office Applications from Creating Child Processes",
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
                Category = "Defender ASR Policy",
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
