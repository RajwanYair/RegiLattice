// RegiLattice.Core — Tweaks/EventTracingPolicy.cs
// Sprint 317: Event Tracing Policy tweaks (10 tweaks)
// Category: "Event Tracing Policy" | Slug: evttrc
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventTracing

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EventTracingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventTracing";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evttrc-disable-etw-telemetry",
            Label = "Disable ETW Telemetry Data Collection",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Event Tracing for Windows collects detailed system telemetry including application performance data, error information, and system events. Disabling ETW telemetry data collection reduces the amount of diagnostic data written to trace log files and uploaded externally. ETW trace data collected from enterprise endpoints can contain sensitive operational information not appropriate for external transmission. Large ETW trace files can consume significant disk space and system resources on high-activity endpoints. Disabling unnecessary telemetry ETW sessions reduces system resource usage without impacting essential Windows operations. Security-critical ETW providers should be maintained while discretionary telemetry providers are disabled.",
            Tags = ["event-tracing", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryETW", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryETW")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryETW", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-restrict-etw-provider-registration",
            Label = "Restrict ETW Provider Registration to Admins",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "ETW providers are software components that write event data to trace sessions and can be registered by any running application. Restricting ETW provider registration to administrators prevents standard user applications from registering custom ETW providers. Malicious software can register ETW providers to intercept and monitor events from security-sensitive ETW sessions. An attacker-registered ETW provider can receive events from protected sessions if improperly isolated. Administrative registration requirements ensure that ETW providers are vetted before being allowed to participate in the tracing infrastructure. This restriction reduces the risk of unauthorized event interception through rogue provider registration.",
            Tags = ["event-tracing", "registration", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictProviderRegistration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictProviderRegistration")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictProviderRegistration", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-disable-process-trace-access",
            Label = "Disable Process-Wide ETW Trace Access",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "ETW trace access allows processes to consume events written by other processes and system components through trace listeners. Disabling process-wide trace access prevents standard user processes from reading events written by other applications and security components. Malicious processes with trace access can monitor security software activity, credential operations, and authentication events. Reading security-relevant ETW events can reveal information useful for evading detection and bypassing security controls. Trace consumption should be restricted to authorized security monitoring and diagnostic tools with appropriate permissions. This setting reduces information available to malicious processes for detection evasion and security control bypass.",
            Tags = ["event-tracing", "access", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictProcessTraceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictProcessTraceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictProcessTraceAccess", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-set-trace-buffer-size",
            Label = "Set Maximum ETW Trace Buffer Size",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "ETW trace sessions use memory buffers to temporarily hold events before writing to disk or consuming by listeners. Setting the maximum trace buffer size limits the memory that individual ETW sessions can consume on endpoints. Unbounded ETW buffer allocation can allow denial-of-service conditions where ETW sessions consume large amounts of system memory. High buffer limits on endpoints with many active trace sessions can significantly impact available memory for operating system and application use. Reasonable buffer limits ensure that ETW tracing provides diagnostics value without causing memory pressure. Buffer size limits should be set based on the number of concurrent trace sessions and available system memory.",
            Tags = ["event-tracing", "buffer", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxTraceBufferSize", 32)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxTraceBufferSize")],
            DetectOps = [RegOp.CheckDword(Key, "MaxTraceBufferSize", 32)],
        },
        new TweakDef
        {
            Id = "evttrc-disable-live-etw-consumption",
            Label = "Disable Unauthorized Live ETW Event Consumption",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Live ETW event consumption allows processes to read events in real time from active trace sessions as they are generated. Disabling unauthorized live consumption prevents non-privileged processes from subscribing to and receiving live ETW event streams. Live event streams from security-relevant ETW providers can reveal real-time authentication activity and security control states. Attackers with live ETW access can monitor the effect of their actions in real time to evade detection and optimize attack timing. Restricting live consumption to authorized monitoring processes reduces information disclosure risk from the ETW subsystem. Administrative and security monitoring tools should connect to ETW sessions through controlled interfaces rather than open consumption.",
            Tags = ["event-tracing", "consumption", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLiveConsumption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLiveConsumption")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLiveConsumption", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-enable-etw-audit-policy",
            Label = "Enable ETW Security Audit Logging",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "ETW security audit logging records significant ETW infrastructure events including session creation, provider registration, and access attempts. Enabling ETW audit logging provides visibility into ETW usage patterns that can indicate surveillance or data exfiltration through tracing. Monitoring ETW infrastructure events supports detection of malicious use of the tracing subsystem for reconnaissance. Security operations centers can correlate ETW audit events with other security indicators to identify suspicious monitoring activity. ETW audit events are written to the Windows Security event log and can be forwarded to SIEM infrastructure. Audit logging has minimal performance overhead and provides valuable data for both security monitoring and forensic investigation.",
            Tags = ["event-tracing", "audit", "logging", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditETWSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditETWSecurity")],
            DetectOps = [RegOp.CheckDword(Key, "AuditETWSecurity", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-disable-circular-buffer-overwrite",
            Label = "Disable ETW Circular Buffer Overwrite",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "ETW circular buffer mode overwrites the oldest events when the buffer fills up rather than halting event collection. Disabling circular buffer overwrite prevents critical security events from being silently lost when the buffer reaches capacity. In circular buffer mode an attacker who generates high volumes of noise events can cause important security events to be overwritten. Security investigation depends on having complete event records and overwritten events cannot be recovered for forensic analysis. Enterprise security event logging should use sequential or expanding buffers that retain all events rather than overwriting old ones. Log management infrastructure should be sized appropriately to handle event volumes without resorting to circular overwrite.",
            Tags = ["event-tracing", "buffer", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCircularBufferOverwrite", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCircularBufferOverwrite")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCircularBufferOverwrite", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-restrict-etw-logfile-access",
            Label = "Restrict ETW Log File Access Permissions",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "ETW log files written to disk can contain sensitive operational data about system and application activity during the trace session. Restricting ETW log file access permissions ensures that only authorized users and processes can read trace log files. Standard users with access to ETW log files can extract operational data about system activities including cryptographic operations and credential access. Log file access restrictions complement ETW session access controls to protect sensitive trace data at rest. ETW log files should be protected with the same access controls applied to other sensitive operational data. Access auditing should be enabled on ETW log directories to detect unauthorized read attempts.",
            Tags = ["event-tracing", "files", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLogFileAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLogFileAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLogFileAccess", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-disable-process-trace-auto-logger",
            Label = "Disable Unauthorized AutoLogger Sessions",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "AutoLogger sessions start automatically at system boot before any user authentication and collect events from the earliest stages of system startup. Disabling unauthorized AutoLogger sessions prevents malicious or unnecessary persistent trace sessions from running throughout system operation. AutoLogger sessions are created through registry keys and a malicious AutoLogger can monitor security-sensitive startup events. Persistent trace sessions consume memory and processing resources throughout the system lifetime even when not needed. Unauthorized AutoLogger sessions can be used for persistence by malicious software that registers a trace session during infection. Managing AutoLogger registrations ensures that only approved diagnostic sessions run during system startup.",
            Tags = ["event-tracing", "autologger", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictAutoLoggerCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictAutoLoggerCreation")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictAutoLoggerCreation", 1)],
        },
        new TweakDef
        {
            Id = "evttrc-set-event-log-file-size",
            Label = "Set Event Log Maximum File Size",
            Category = "Event Tracing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Event log file size limits determine the maximum amount of event data that is retained before the oldest events are overwritten or the log becomes full. Setting appropriate file size limits ensures that sufficient event history is retained for security investigations. Small event log file sizes cause frequent overwriting that can prevent investigation of incidents that occurred in the past. Event log size recommendations from NIST and CIS benchmarks specify minimum file sizes for different log types. The Security event log should be large enough to retain at minimum 7 days of events for common incident investigation timeframes. Log file size settings should be coordinated with centralized log forwarding to ensure events are captured before local overwrite.",
            Tags = ["event-tracing", "log-size", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxEventLogFileSize", 65536)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxEventLogFileSize")],
            DetectOps = [RegOp.CheckDword(Key, "MaxEventLogFileSize", 65536)],
        },
    ];
}
