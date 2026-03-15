namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Event logging and Windows audit policy tweaks — configures event log sizes,
/// audit policies, crash dump settings, and diagnostic logging levels.
/// </summary>
internal static class EventLogging
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string EventLogKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evtlog-increase-system-log-size",
            Label = "Increase System Event Log to 64 MB",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the System event log maximum size from 20 MB to 64 MB for better diagnostic retention.",
            Tags = ["event-log", "diagnostics", "system", "capacity"],
            RegistryKeys = [$@"{EventLogKey}\System"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 67108864)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\System", "MaxSize", 67108864)],
        },
        new TweakDef
        {
            Id = "evtlog-increase-security-log-size",
            Label = "Increase Security Event Log to 128 MB",
            Category = "Event Logging",
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
            Id = "evtlog-increase-application-log-size",
            Label = "Increase Application Event Log to 64 MB",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Application event log maximum size from 20 MB to 64 MB.",
            Tags = ["event-log", "application", "capacity"],
            RegistryKeys = [$@"{EventLogKey}\Application"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 67108864)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Application", "MaxSize", 67108864)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-powershell-script-block-logging",
            Label = "Enable PowerShell Script Block Logging",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables detailed PowerShell script block logging. Essential for security monitoring and incident response.",
            Tags = ["event-log", "powershell", "security", "audit", "logging"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 1),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-enable-powershell-module-logging",
            Label = "Enable PowerShell Module Logging",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables logging of PowerShell module invocations for security auditing.",
            Tags = ["event-log", "powershell", "security", "audit", "module"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-process-creation-audit",
            Label = "Enable Process Creation Auditing",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Includes command-line arguments in process creation audit events (Event ID 4688).",
            Tags = ["event-log", "security", "audit", "process", "forensics"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-set-crash-dump-mini",
            Label = "Set Crash Dumps to Mini Dump",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Windows to create small memory dumps (mini dumps) instead of full dumps. Saves disk space.",
            Tags = ["event-log", "crash", "dump", "disk-space", "diagnostics"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-auto-reboot-on-crash",
            Label = "Disable Auto-Reboot on Blue Screen",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents automatic reboot after a BSOD so you can read the error message and stop code.",
            Tags = ["event-log", "crash", "bsod", "diagnostics", "debug"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-verbose-boot-status",
            Label = "Enable Verbose Boot Status Messages",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot/shutdown instead of generic 'Please wait'. Helps diagnose slow boot issues.",
            Tags = ["event-log", "boot", "diagnostics", "verbose", "troubleshooting"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-shutdown-reason",
            Label = "Enable Shutdown Event Tracker",
            Category = "Event Logging",
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
            Id = "evtlog-log-retention-overwrite",
            Label = "Set Event Logs to Overwrite as Needed",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures all event logs to overwrite old entries when full. Prevents log overflow causing service failures.",
            Tags = ["event-log", "retention", "overwrite", "maintenance"],
            RegistryKeys = [$@"{EventLogKey}\Application", $@"{EventLogKey}\System", $@"{EventLogKey}\Security"],
            ApplyOps =
            [
                RegOp.SetDword($@"{EventLogKey}\Application", "Retention", 0),
                RegOp.SetDword($@"{EventLogKey}\System", "Retention", 0),
                RegOp.SetDword($@"{EventLogKey}\Security", "Retention", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{EventLogKey}\Application", "Retention"),
                RegOp.DeleteValue($@"{EventLogKey}\System", "Retention"),
                RegOp.DeleteValue($@"{EventLogKey}\Security", "Retention"),
            ],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Application", "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-event-forwarding",
            Label = "Disable Windows Event Forwarding",
            Category = "Event Logging",
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
            Id = "evtlog-set-app-log-32mb",
            Label = "Set Application Event Log to 32 MB",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Application event log maximum size to 32 MB for better diagnostic history.",
            Tags = ["event-log", "application", "size", "diagnostics"],
            RegistryKeys = [$@"{EventLogKey}\Application"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 33554432)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Application", "MaxSize", 33554432)],
        },
        new TweakDef
        {
            Id = "evtlog-set-system-log-32mb",
            Label = "Set System Event Log to 32 MB",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the System event log maximum size to 32 MB for better diagnostic history.",
            Tags = ["event-log", "system", "size", "diagnostics"],
            RegistryKeys = [$@"{EventLogKey}\System"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 33554432)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\System", "MaxSize", 33554432)],
        },
        new TweakDef
        {
            Id = "evtlog-set-security-log-64mb",
            Label = "Set Security Event Log to 64 MB",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Security event log maximum size to 64 MB. Critical for security auditing in hardened environments.",
            Tags = ["event-log", "security", "size", "auditing", "hardening"],
            RegistryKeys = [$@"{EventLogKey}\Security"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 67108864)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Security", "MaxSize", 67108864)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-event-tracing-autologger",
            Label = "Disable Autologger Event Tracing Sessions",
            Category = "Event Logging",
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
            Category = "Event Logging",
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
            Category = "Event Logging",
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
            Id = "evtlog-enable-command-line-auditing",
            Label = "Enable Process Command-Line Auditing",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Logs the full command line for process creation events (Event ID 4688). Critical for forensic analysis.",
            Tags = ["event-log", "auditing", "command-line", "forensics", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-disable-srum",
            Label = "Disable System Resource Usage Monitor (SRUM)",
            Category = "Event Logging",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables SRUM which tracks application resource usage, network activity, and energy data.",
            Tags = ["event-log", "srum", "tracking", "privacy", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
    ];
}
