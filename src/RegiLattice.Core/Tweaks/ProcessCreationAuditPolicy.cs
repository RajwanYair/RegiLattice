// RegiLattice.Core — Tweaks/ProcessCreationAuditPolicy.cs
// Process creation and command-line argument auditing Group Policy controls (Sprint 622).
// Category: "Process Creation Audit Policy" | Slug: pcaudit
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\System (process audit settings)

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ProcessCreationAuditPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "pcaudit-enable-cmdline-in-process-creation-events",
            Label = "Process Audit: Enable Full Command Line in Process Creation Security Events",
            Category = "Process Creation Audit Policy",
            Description = "Sets ProcessCreationIncludeCmdLine_Enabled=1 in the Windows System policy. Enables Windows Security event 4688 (Process Creation) to include the full command-line argument string of the spawned process in the event, rather than only the process executable path. This allows SIEM systems to detect living-off-the-land attacks, fileless malware, and suspicious PowerShell invocations by analysing the full arguments of every process created. " +
                "Process creation event 4688 without command-line inclusion only shows the executable path (e.g., powershell.exe), not the arguments (-EncodedCommand, -ExecutionPolicy Bypass, -WindowStyle Hidden). Without arguments visible, encoded PowerShell commands, Mimikatz execution via living-off-the-land binaries (LOLBins), and command injection attacks are almost entirely opaque in the Security event log. Command-line auditing is the foundational enabling control for advanced threat detection.",
            Tags = ["process-audit", "cmdline", "process-creation", "event-4688", "siem", "lolbins"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Full command lines visible in Event 4688; SIEM can detect encoded/obfuscated PowerShell, LOLBins, and injection attacks.",
            ApplyOps = [RegOp.SetDword(Key, "ProcessCreationIncludeCmdLine_Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ProcessCreationIncludeCmdLine_Enabled")],
            DetectOps = [RegOp.CheckDword(Key, "ProcessCreationIncludeCmdLine_Enabled", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-wmi-activity-auditing",
            Label = "Process Audit: Enable WMI Activity Audit Log for Process-Level WMI Operations",
            Category = "Process Creation Audit Policy",
            Description = "Sets EnableWMIActivityAudit=1 in the Windows System policy. Enables the Microsoft-Windows-WMI-Activity/Operational event log channel, causing WMI query execution, WMI provider invocations, and WMI subscription modifications to be logged. WMI is a primary lateral movement and persistence technique used by threat actors to execute code remotely without spawning a child process visible in process creation audit logs. " +
                "WMI-based attacks (used in APT28, Carbanak, and most enterprise-targeted ransomware operators) execute payload code through the WMI provider host (WmiPrvSE.exe) as a child of svchost.exe, bypassing process creation rules that watch for powershell.exe or cmd.exe. WMI activity logging provides a parallel audit trail for WMI-executed commands that cannot be correlated from process creation events alone, enabling detection of WMI-based fileless lateral movement.",
            Tags = ["process-audit", "wmi", "lateral-movement", "wmiprvse", "apt", "fileless"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "WMI operations logged in Activity event channel; WMI-based lateral movement and persistence detectable by EDR/SIEM.",
            ApplyOps = [RegOp.SetDword(Key, "EnableWMIActivityAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableWMIActivityAudit")],
            DetectOps = [RegOp.CheckDword(Key, "EnableWMIActivityAudit", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-psh-module-logging",
            Label = "Process Audit: Enable PowerShell Module Logging for All Script Block Execution",
            Category = "Process Creation Audit Policy",
            Description = "Sets EnableModuleLogging=1 in the Windows System policy. Enables PowerShell module logging, which records the full content of every PowerShell pipeline execution (all commands, scripts, and functions invoked) to the PowerShell event log (Microsoft-Windows-PowerShell/Operational, Event ID 4103), providing complete visibility into what code PowerShell executes even when scripts are obfuscated. " +
                "PowerShell is the most commonly abused administrative tool for post-exploitation activities. Module logging captures the deobfuscated execution of AMSI-aware scripts — when a malicious actor uses encoded base64 commands or string manipulation to evade static detection, PowerShell must decode the payload before execution. Module logging captures the post-decode execution pipeline, revealing the actual malicious commands regardless of the obfuscation layering.",
            Tags = ["process-audit", "powershell", "module-logging", "obfuscation", "amsi", "script-block"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "PowerShell module logging active; all PowerShell execution including decoded obfuscated commands visible in event log.",
            ApplyOps = [RegOp.SetDword(Key, "EnableModuleLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableModuleLogging", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-psh-script-block-logging",
            Label = "Process Audit: Enable PowerShell Script Block Logging with Obfuscation Auto-Logging",
            Category = "Process Creation Audit Policy",
            Description = "Sets EnableScriptBlockLogging=1 in the Windows System policy. Enables PowerShell Script Block logging (Event ID 4104), which captures every script block (function body, scriptblock literal, and processed script pipeline) executed by PowerShell into the event log. When combined with AMSI integration, suspicious script block content is automatically promoted to 'suspicious script block' events (4104 with level Warning) without requiring rule tuning. " +
                "Script block logging is stronger than module logging because it operates at a lower level (the PowerShell engine's block compilation step) and captures the content of scripts before they are executed, even when the script is loaded from memory or piped from another command. Script block logging is complementary to AMSI — AMSI inspects content before execution for malware signatures; script block logging captures all execution for post-incident investigation.",
            Tags = ["process-audit", "powershell", "script-block-logging", "event-4104", "amsi", "memory-only"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Script block logging active (Event 4104); all PowerShell script content including memory-only scripts captured.",
            ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockLogging", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-psh-transcription",
            Label = "Process Audit: Enable PowerShell Transcription to Centralised Audit Share",
            Category = "Process Creation Audit Policy",
            Description = "Sets EnableTranscripting=1 in the Windows System policy. Enables PowerShell transcription, which writes a text transcript of every PowerShell session (all input commands and output) to a log file. When combined with a centralised transcript output directory (network share or DFS path), all PowerShell session activity from all endpoints is written to a central searchable store. " +
                "PowerShell transcripts capture information that neither script block logging nor module logging captures: the full interactive session flow including the output returned by commands (e.g., the contents of Get-ChildItem output, netstat results captured by commands, or credentials visible in command output). While transcripts are more verbose than event log entries, they provide a continuous narrative of a PowerShell session that is invaluable for incident reconstruction when reconstructing what a threat actor did during a dwell-time period.",
            Tags = ["process-audit", "powershell", "transcription", "session-log", "incident-response"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "PowerShell transcription enabled; all PS session input and output logged to transcript file.",
            ApplyOps = [RegOp.SetDword(Key, "EnableTranscripting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTranscripting")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTranscripting", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-protected-event-logging",
            Label = "Process Audit: Enable Protected Event Logging for PowerShell Encrypted Log Entries",
            Category = "Process Creation Audit Policy",
            Description = "Sets EnableProtectedEventLogging=1 in the Windows System policy. Enables Protected Event Logging, which encrypts the content of sensitive PowerShell script block log entries (Event 4104) using a specified asymmetric public key certificate, so that the log content can only be read by the private key holder on the log analysis server, protecting sensitive command content (passwords, tokens) in the event log from local plaintext exposure. " +
                "Standard PowerShell script block logging writes command content in plaintext to the event log. If an administrative PowerShell script processes credentials, API keys, or sensitive data, those values appear in the local Security event log in cleartext. Any process with read access to the Security event log (including some malware) can harvest these credentials from the log. Protected Event Logging encrypts sensitive entries, allowing detection while protecting the content from local extraction.",
            Tags = ["process-audit", "powershell", "protected-event-logging", "encryption", "credentials", "log-protection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "PowerShell event log entries encrypted with PKI certificate; sensitive commands protected from local plaintext access.",
            ApplyOps = [RegOp.SetDword(Key, "EnableProtectedEventLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableProtectedEventLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableProtectedEventLogging", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-security-audit-process-termination",
            Label = "Process Audit: Enable Security Audit Events for Process Termination (Event 4689)",
            Category = "Process Creation Audit Policy",
            Description = "Sets AuditProcessTermination=1 in the Windows System policy. Enables Security event log event 4689 (A process has exited), which records the process name, PID, user account, and exit code when any process terminates. When correlated with Event 4688 (process creation), this enables calculation of exact process lifetimes, detection of very-short-lived suspicious processes, and analysis of process trees during incident investigation. " +
                "Process termination audit enables detection of living-off-the-land binary (LOLBin) usage where a legitimately signed binary (e.g., certutil.exe, regsvr32.exe) is spawned, executes a malicious payload, and exits in milliseconds. Without process termination events, the SIEM only has the creation event and no end marker, making it impossible to calculate process lifetime or determine what happened between creation and exit. Short-lifetime processes (sub-second) that accomplish significant work are high-fidelity attack indicators.",
            Tags = ["process-audit", "process-termination", "event-4689", "lolbins", "process-lifetime", "siem"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Process termination Events 4689 generated; process lifetimes calculable; short-lived LOLBin execution detectable.",
            ApplyOps = [RegOp.SetDword(Key, "AuditProcessTermination", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditProcessTermination")],
            DetectOps = [RegOp.CheckDword(Key, "AuditProcessTermination", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-pnp-activity-audit",
            Label = "Process Audit: Enable Plug-and-Play Device Connection/Disconnection Audit Events",
            Category = "Process Creation Audit Policy",
            Description = "Sets AuditPNPActivity=1 in the Windows System policy. Enables Security event log events 6416/6419/6420/6421/6423/6424 (Plug and Play activity) that record when new hardware devices are connected or disconnected from the system, including USB drives, network adapters, Bluetooth dongles, and other peripherals — recording the device ID, device type, and connecting user account. " +
                "USB removable storage is a primary exfiltration vector and a common way to deliver malware (BadUSB, autorun malware). Without PnP audit events, there is no Security event log record of which USB devices were connected, to which endpoints, by which user, at what time. PnP audit events provide DLP and insider threat detection capability — a user who copies data to a USB drive that was connected to their endpoint for 3 minutes generates a complete audit trail of the connection without requiring endpoint DLP software.",
            Tags = ["process-audit", "pnp", "usb", "device-connection", "exfiltration", "baduusb"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "USB/PnP device connections generate Security events; device connection history auditable for exfiltration detection.",
            ApplyOps = [RegOp.SetDword(Key, "AuditPNPActivity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditPNPActivity")],
            DetectOps = [RegOp.CheckDword(Key, "AuditPNPActivity", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-enable-network-connection-events-sysmon-style",
            Label = "Process Audit: Enable Network Connection Events in Windows Event Log Without Sysmon",
            Category = "Process Creation Audit Policy",
            Description = "Sets AuditNetworkConnectionEvents=1 in the Windows System policy. Enables network connection audit events in the Security event log, recording each TCP/UDP connection attempt with the originating process ID, source address/port, and destination address/port, providing network process binding visibility without requiring Sysmon or third-party endpoint agents. " +
                "Network connection logging is standard in Sysmon Event ID 3, but many enterprises cannot deploy Sysmon due to policy or operational constraints. Windows Security event log network connection auditing (when configured) provides a subset of the same visibility natively. Detecting beaconing to C2 infrastructure requires correlation of process creation events with the network connections those processes make. Without network connection events, process creation auditing alone cannot establish which external hosts a suspicious process contacted.",
            Tags = ["process-audit", "network-connections", "c2-detection", "beaconing", "sysmon-alternative"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Network connection events logged natively; C2 beaconing detectable without Sysmon deployment.",
            ApplyOps = [RegOp.SetDword(Key, "AuditNetworkConnectionEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditNetworkConnectionEvents")],
            DetectOps = [RegOp.CheckDword(Key, "AuditNetworkConnectionEvents", 1)],
        },
        new TweakDef
        {
            Id = "pcaudit-set-min-security-event-log-size-512mb",
            Label = "Process Audit: Set Minimum Security Event Log Size to 512 MB",
            Category = "Process Creation Audit Policy",
            Description = "Sets SecurityEventLogMinSizeMB=512 in the Windows System policy. Enforces a minimum Security event log file size of 512 MB, ensuring that the on-device event log buffer is large enough to sustain at least 30 days of security audit event retention without log rotation truncating investigative evidence before it can be forwarded to a SIEM. " +
                "The default Windows Security event log size is 20 MB. With process creation auditing, command-line auditing, and PnP auditing all enabled, a busy endpoint can generate several MB of Security events per hour. A 20 MB log buffer retains as little as a few hours of events. On endpoints without a SIEM agent forwarding events in real time, a 20 MB log means that events from an overnight incident may have been overwritten before the morning IT team investigates. A 512 MB buffer provides several weeks of local retention.",
            Tags = ["process-audit", "event-log", "retention", "siem", "forensics"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Security event log minimum size 512 MB; multi-week local retention for environments without real-time SIEM forwarding.",
            ApplyOps = [RegOp.SetDword(Key, "SecurityEventLogMinSizeMB", 512)],
            RemoveOps = [RegOp.DeleteValue(Key, "SecurityEventLogMinSizeMB")],
            DetectOps = [RegOp.CheckDword(Key, "SecurityEventLogMinSizeMB", 512)],
        },
    ];
}
