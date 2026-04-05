namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from EventLogging.cs ────────────────────────────────────────
internal static class EventLogging
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string EventLogKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evtlog-increase-security-log-size",
            Label = "Increase Security Event Log to 128 MB",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Security event log maximum size to 128 MB for better audit trail retention.",
            Tags = ["event-log", "security", "audit", "capacity"],
            RegistryKeys = [$@"{EventLogKey}\Security"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 134217728)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Security", "MaxSize", 134217728)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-shutdown-reason",
            Label = "Enable Shutdown Event Tracker",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prompts for a reason when shutting down or restarting the system. Useful for server/audit scenarios.",
            Tags = ["event-log", "shutdown", "audit", "server", "tracking"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-event-forwarding",
            Label = "Disable Windows Event Forwarding",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Event Forwarding subscription service. Prevents events from being sent to remote collectors.",
            Tags = ["event-log", "forwarding", "privacy", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager", "Enabled")],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-disable-event-tracing-autologger",
            Label = "Disable Autologger Event Tracing Sessions",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables several Autologger ETW sessions that perform background diagnostic tracing.",
            Tags = ["event-log", "etw", "autologger", "performance", "privacy"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-powershell-logging",
            Label = "Disable PowerShell Script Block Logging",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables PowerShell script block logging that records all executed scripts to the event log.",
            Tags = ["event-log", "powershell", "logging", "privacy", "performance"],
            SideEffects = "Reduces forensic capability for detecting malicious PowerShell scripts.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 0),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-enable-powershell-transcription",
            Label = "Enable PowerShell Transcription Logging",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables PowerShell transcription to log all input/output to a file for forensic auditing.",
            Tags = ["event-log", "powershell", "transcription", "auditing", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription", "EnableTranscripting", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription", "EnableTranscripting")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription", "EnableTranscripting", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-application-log",
            Label = "Limit Application Event Log Size",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Application event log maximum size to 1 MB and enables auto-overwrite to free disk space.",
            Tags = ["event-log", "disk", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "MaxSize", 1048576),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "Retention", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "MaxSize", 20971520),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "Retention", 0),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "MaxSize", 1048576)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-security-audit-logon",
            Label = "Disable Logon Failure Audit",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables auditing of failed logon attempts in the Security event log. Reduces event log spam on unattended machines.",
            Tags = ["event-log", "security", "audit", "logon"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "AuditBaseObjects", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "AuditBaseObjects")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "AuditBaseObjects", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-module-logging",
            Label = "Disable PowerShell Module Logging",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables PowerShell module logging, preventing every module command from being recorded in the event log.",
            Tags = ["event-log", "powershell", "module", "logging"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-windows-error-reporting-log",
            Label = "Disable WER Event Log Entries",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Error Reporting from writing crash and hang events to the Application event log.",
            Tags = ["event-log", "wer", "crash", "diagnostics"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-setup-log",
            Label = "Limit Setup Event Log Size",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the Setup event log to 1 MB with auto-overwrite, preventing unbounded growth on frequently updated systems.",
            Tags = ["event-log", "disk", "setup", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "MaxSize", 1048576),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "Retention", 0),
            ],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "MaxSize", 1048576)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-forwarded-log",
            Label = "Disable Forwarded Events Log",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Event Log Forwarding service (Wecsvc) used to forward events to a remote collector. Not needed on standalone PCs.",
            Tags = ["event-log", "forwarding", "network", "svc"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-dns-client-log",
            Label = "Disable DNS Resolver Event Tracing",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables DNS client operational event logging in the Microsoft-Windows-DNS-Client/Operational channel to reduce disk I/O.",
            Tags = ["event-log", "dns", "network", "tracing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational",
                    "Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-disable-kernel-event-tracing",
            Label = "Disable NT Kernel Logger Session",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the NT Kernel Logger ETW session to not auto-start, reducing baseline CPU and disk overhead.",
            Tags = ["event-log", "etw", "kernel", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger", "Start", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger", "Start", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger", "Start", 0)],
        },
    ];
}
