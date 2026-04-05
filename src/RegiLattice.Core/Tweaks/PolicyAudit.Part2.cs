namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyAudit
{
    // ── EventSubscriptionPolicy ──
    private static class _EventSubscriptionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventCollector";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wecpol-enable-event-collector-service",
                    Label = "Enable Windows Event Collector Service",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Enables the Windows Event Collector service which accepts WinRM-based event forwarding subscriptions, allowing this machine to act as a centralised log collection point for multiple source machines.",
                    Tags = ["event-collector", "wec", "winrm", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Windows Event Collector service enabled; this machine accepts WEF subscriptions as a collector.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableEventCollector", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableEventCollector")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableEventCollector", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-require-https-on-collector",
                    Label = "Require HTTPS on Windows Event Collector Subscriptions",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Forces all incoming event forwarding connections to the Windows Event Collector to use HTTPS, blocking plain HTTP or unencrypted WinRM connections from source machines.",
                    Tags = ["event-collector", "https", "encryption", "wec", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Event Collector requires HTTPS; plain HTTP forwarding connections rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireHTTPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireHTTPS")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireHTTPS", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-limit-subscription-concurrency-100",
                    Label = "Limit Event Collector Concurrent Source Connections to 100",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets the maximum number of concurrent source machine connections to the Windows Event Collector to 100, preventing resource exhaustion on the collector from too many simultaneous forwarding sessions.",
                    Tags = ["event-collector", "concurrency", "resource-limit", "wec", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Event Collector limited to 100 concurrent source connections; excess source machines rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxConcurrentForwardingConnections", 100)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxConcurrentForwardingConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxConcurrentForwardingConnections", 100)],
                },
                new TweakDef
                {
                    Id = "wecpol-log-subscription-setup-failures",
                    Label = "Log Event Collector Subscription Setup Failures",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Enables detailed event log entries when Windows Event Collector subscription setup fails, providing diagnostics for misconfigurations such as authentication failures, network issues, and XPath query errors.",
                    Tags = ["event-collector", "diagnostics", "subscription-failure", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Subscription setup failure events logged; WEC configuration errors are diagnostic and visible.",
                    ApplyOps = [RegOp.SetDword(Key, "LogSubscriptionSetupFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogSubscriptionSetupFailures")],
                    DetectOps = [RegOp.CheckDword(Key, "LogSubscriptionSetupFailures", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-disable-legacy-event-subscription",
                    Label = "Disable Legacy Event Pull Subscription (Source-Initiated Only)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Disables collector-initiated (legacy pull) subscriptions, allowing only source-initiated (push) subscriptions where source machines connect to the collector, which works across NAT and firewall boundaries.",
                    Tags = ["event-collector", "pull-subscription", "source-initiated", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Collector-initiated pull subscriptions disabled; only source-initiated push subscriptions allowed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCollectorInitiatedSubscriptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCollectorInitiatedSubscriptions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCollectorInitiatedSubscriptions", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-audit-subscription-activity",
                    Label = "Audit All Event Collector Subscription Activity",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Enables detailed auditing of all Windows Event Collector subscription activities (created, modified, deleted, connected, disconnected) to the local Security event log for compliance and change tracking.",
                    Tags = ["event-collector", "audit", "subscription-activity", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All WEC subscription activities audited; subscription changes and connection events logged.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSubscriptionActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSubscriptionActivity")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSubscriptionActivity", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-set-heartbeat-interval-3600",
                    Label = "Set Event Collector Heartbeat Interval to 3600 Seconds",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets the Windows Event Collector heartbeat interval to 3600 seconds (one hour), reducing the frequency of heartbeat network traffic between source machines and the collector on stable networks.",
                    Tags = ["event-collector", "heartbeat", "network", "interval", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WEC heartbeat interval set to 1 hour; less heartbeat traffic on stable networks.",
                    ApplyOps = [RegOp.SetDword(Key, "HeartbeatIntervalSeconds", 3600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HeartbeatIntervalSeconds")],
                    DetectOps = [RegOp.CheckDword(Key, "HeartbeatIntervalSeconds", 3600)],
                },
                new TweakDef
                {
                    Id = "wecpol-restrict-subscription-management-to-admin",
                    Label = "Restrict Event Collector Subscription Management to Administrators",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Requires administrator privileges to create, modify, or delete Windows Event Collector subscriptions, preventing standard users or service accounts from altering the event collection pipeline.",
                    Tags = ["event-collector", "admin", "subscription-management", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Subscription management restricted to admins; standard users cannot create or modify WEC subscriptions.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForSubscriptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForSubscriptions")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForSubscriptions", 1)],
                },
                new TweakDef
                {
                    Id = "wecpol-set-max-event-buffer-1mb",
                    Label = "Set Event Collector Internal Buffer to 1 MB Per Subscription",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets the internal memory buffer used per Windows Event Collector subscription to 1 MB, providing sufficient queuing capacity for burst event delivery while limiting per-subscription memory consumption.",
                    Tags = ["event-collector", "buffer", "memory", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WEC per-subscription buffer set to 1 MB; moderate burst tolerance without excessive memory use.",
                    ApplyOps = [RegOp.SetDword(Key, "SubscriptionBufferSizeKB", 1024)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SubscriptionBufferSizeKB")],
                    DetectOps = [RegOp.CheckDword(Key, "SubscriptionBufferSizeKB", 1024)],
                },
                new TweakDef
                {
                    Id = "wecpol-disable-collector-telemetry",
                    Label = "Disable Windows Event Collector Telemetry to Microsoft",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Prevents the Windows Event Collector service from sending diagnostic and telemetry data about subscription health and performance to Microsoft, protecting internal event collection architecture from cloud disclosure.",
                    Tags = ["event-collector", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEC telemetry to Microsoft disabled; no subscription health stats sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCollectorTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCollectorTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCollectorTelemetry", 1)],
                },
            ];
    }

    // ── EventTracingPolicy ──
    private static class _EventTracingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventTracing";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "evttrc-disable-etw-telemetry",
                Label = "Disable ETW Telemetry Data Collection",
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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
                Category = "Security — Event Log Channel",
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

    // ── LogonEventsAuditPolicy ──
    private static class _LogonEventsAuditPolicy
    {
        private const string Key =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Logon-Logoff";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "logonaudit-audit-logon-success-failure",
                    Label = "Logon Audit: Enable Success+Failure Auditing for All Interactive and Network Logon Events",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditLogon=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 4624 (successful logon) with logon type, source IP, and authentication protocol, and 4625 (failed logon) with error code, source IP, and account name for every interactive (Type 2), network (Type 3), service (Type 5), batch (Type 4), and remote desktop (Type 10) logon and logon failure. "
                        + "Event 4624 and 4625 are the most fundamental SOC monitoring events — all lateral movement paths (SMB, RDP, WinRM, PsExec, WMI) generate logon events on the destination endpoint. Without logon auditing, there is no on-endpoint record of who authenticated, from where, and using what mechanism. The combination of 4624 (successful network logon) from an unexpected IP with 4648 (explicit credential use) from the same timeframe is a high-fidelity indicator for pass-the-hash lateral movement.",
                    Tags = ["logon-audit", "event-4624", "event-4625", "lateral-movement", "rdp", "smb"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "All logon success and failure events generated; lateral movement via SMB/RDP/WMI leaves on-endpoint Event 4624 traces.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditLogon", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditLogon")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditLogon", 3)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-logoff-events",
                    Label = "Logon Audit: Enable Logoff Event Auditing to Calculate Session Duration",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditLogoff=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4634 (account logoff) when an interactive or network session ends, enabling SIEM correlation to calculate session duration by pairing each 4624 logon event with its 4634 logoff counterpart. Session duration is an important context signal for anomalous access detection. "
                        + "Session duration analysis enables detection of anomalous access patterns. A network logon (4624 Type 3) that lasts 0.3 seconds followed by a logoff (4634) is consistent with automated tool access (PsExec command execution, SMB enumeration). A session from an external IP lasting 4 hours at 2 AM is anomalous for a finance analyst's account. Without logoff events, session duration calculations are impossible and the analyst must infer session end from other activity gaps in the log.",
                    Tags = ["logon-audit", "event-4634", "session-duration", "anomaly-detection", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Logoff events generated (4634); session duration calculable; anomalous session patterns detectable via logon/logoff correlation.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditLogoff", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditLogoff")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditLogoff", 1)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-account-lockout-logon",
                    Label = "Logon Audit: Enable Account Lockout Event Auditing at Logon (4740 on Destination)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditAccountLockout=1 (Success) in Advanced Audit Policy Logon/Logoff category (logon-side complement to the Account Management lockout setting). Generates Security event 4625 subtype failure events on the endpoint where a locked-out account attempts logon in addition to the domain controller-generated 4740. Provides per-endpoint lockout event rather than only DC-centric events. "
                        + "Domain controller-generated lockout events (4740) identify that an account locked out but report only the last DC that processed the lockout, not all the individual endpoints generating failed logon attempts that accumulated to the lockout threshold. Endpoint-generated 4625 Failure / Sub-status 0xC0000234 (account locked out at logon time) events pinpoint exactly which endpoints are producing the lockout-triggering authentication failures, enabling source system identification for spray attack forensics.",
                    Tags = ["logon-audit", "account-lockout", "4740", "4625", "spray-attack", "source-identification"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Per-endpoint lockout attempt events generated; spray attack source endpoints identifiable without relying only on DC events.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditAccountLockout", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditAccountLockout")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditAccountLockout", 1)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-network-policy-server",
                    Label = "Logon Audit: Enable Network Policy Server Radius/NPS Authentication Auditing",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditNetworkPolicyServer=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 6272 (NPS granted access), 6273 (NPS denied access), 6274 (NPS discarded request), 6275 (NPS discarded accounting request), 6276 (NPS quarantined client), 6277/6278 (NPS granted probation/revoked access) for RADIUS network access control decisions made by the local NPS role. "
                        + "Network Policy Server (NPS/RADIUS) is the authentication gateway for 802.1X network access control (wired and wireless NAC), VPN authentication, and DirectAccess. NPS audit events record every network access authentication decision — including which machine certificates or user credentials were validated, which NPS policy matched, and whether access was granted or denied. A compromised certificate used to authenticate to the corporate wireless network generates NPS event 6272 with the certificate thumbprint, enabling certificate abuse detection.",
                    Tags = ["logon-audit", "nps", "radius", "802.1x", "vpn", "nac"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "NPS/RADIUS authentication decisions audited; network access control events provide NAC bypass and certificate abuse detection.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditNetworkPolicyServer", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditNetworkPolicyServer")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditNetworkPolicyServer", 3)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-other-logon-logoff-events",
                    Label = "Logon Audit: Enable 'Other Logon/Logoff Events' for Session Reconnection Tracking",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditOtherLogonLogoffEvents=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 4649 (replay attack detected), 4778 (session reconnected to Window Station), 4779 (session disconnected from Window Station), 4800 (workstation locked), 4801 (workstation unlocked), 4802/4803 (screensaver invoked/dismissed), 5378 (credential delegation requested), 5632/5633 (wireless/wired 802.1X authentication). "
                        + "Events 4778/4779 (RDP/Terminal Services session reconnect and disconnect) are critical for RDP lateral movement forensics. Each reconnect event records the source IP, session ID, and account name separately from the initial logon event. Without other logon/logoff events, an attacker who uses RDP shadowing or session hijacking (connecting to an existing session without creating a new logon event) may not generate additional 4624 events. The 4778 reconnect event captures this post-logon session reuse.",
                    Tags = ["logon-audit", "other-logon", "4778", "4779", "rdp-session", "session-hijacking"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "RDP session reconnect/disconnect events (4778/4779) audited; RDP session hijacking and shadowing generate detectable events.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditOtherLogonLogoffEvents", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditOtherLogonLogoffEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditOtherLogonLogoffEvents", 3)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-explicit-credential-use",
                    Label = "Logon Audit: Enable Explicit Credential Use Auditing (RunAs, Over-Pass-the-Hash, WinRM)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditExplicitCredentialUse=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4648 (logon using explicit credentials) when a process uses a different set of credentials to create a new logon session — covering RunAs executions, WMI remote command execution using explicit credentials, WinRM with credential parameters, and Over-Pass-the-Hash (explicit logon using an injected NTLM hash). "
                        + "Event 4648 is a direct detection signal for Over-Pass-the-Hash and Overpass-the-Hash attacks. When Mimikatz performs an OverPTH (inject NTLM hash into a new logon session using explicit credential logon), Windows generates a 4648 event on the source machine. The combination of 4648 from Machine-A with 4624 Type 3 from Machine-B to Machine-A within the same second is a high-fidelity indicator of pass-the-hash lateral movement initiation from Machine-A.",
                    Tags = ["logon-audit", "explicit-credentials", "event-4648", "overpass-the-hash", "winrm", "runas"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Explicit credential use events (4648) generated; Over-Pass-the-Hash and RunAs credential abuse directly detectable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditExplicitCredentialUse", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditExplicitCredentialUse")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditExplicitCredentialUse", 3)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-special-logon-sensitive-groups",
                    Label = "Logon Audit: Enable Special Logon Auditing for Privileged Group Member Authentication",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditSpecialLogon=1 (Success) in Advanced Audit Policy Logon/Logoff category (logon-side complement to the Account Management special logon setting). Generates Security event 4964 whenever a user whose account is a member of the Special Groups list (typically Domain Admins, Enterprise Admins) authenticates interactively or via the network, providing privileged account authentication monitoring without the noise of universal 4624 auditing. "
                        + "Privileged account authentication monitoring serves as a low-effort approximation of Privileged Access Workstation (PAW) compliance enforcement. If Domain Admins should only authenticate from designated admin workstations, Event 4964 events where the source computer name is not in the approved PAW list indicate a policy violation — an admin authenticated from a regular user workstation. This SIEM rule requires only two data sources: the 4964 event and the approved PAW machine list.",
                    Tags = ["logon-audit", "event-4964", "domain-admins", "paw", "privileged-access", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Privileged group member logons generate Event 4964; admin authentication from non-PAW workstations detectable by SIEM.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSpecialLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSpecialLogon")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSpecialLogon", 1)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-group-membership-at-logon",
                    Label = "Logon Audit: Enable Group Membership Enumeration at Logon for Privilege Visibility",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditGroupMembership=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4627 which lists the full group membership of the logon token at logon time, complementing Event 4624 with the list of all security groups the logging-on user is a member of at the moment of logon. Enables detection of SID injection and Kerberos golden ticket attacks using extra group SIDs. "
                        + "Kerberos golden tickets can be crafted with extra group SIDs added to the PAC (Privileged Account Certificate) that were not in the account's actual group membership. When such a ticket is used for authentication, Windows generates a 4627 event showing the effective group membership of the logon token. By comparing 4627 group membership against the account's actual AD group membership, anomalous extra SIDs (e.g., Domain Admins SID for a non-admin account) are immediately visible as golden ticket indicators.",
                    Tags = ["logon-audit", "group-membership", "event-4627", "golden-ticket", "pac", "kerberos"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Logon-time group membership logged (4627); Kerberos golden ticket with extra group SIDs detectable via 4627/AD membership comparison.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditGroupMembership", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditGroupMembership")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditGroupMembership", 1)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-ipsec-extended-mode",
                    Label = "Logon Audit: Enable IPSec Extended Mode Auditing for Network Authentication Failures",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditIPSecExtendedMode=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events for IPSec IKEv2 extended mode negotiation (4978/4979/4980/4983/4984), recording Kerberos, certificate, or preshared-key authentication exchanges, useful in environments using IPSec machine authentication for network segmentation enforcement via Windows Firewall with Advanced Security rules. "
                        + "IPSec extended mode authentication provides machine-level authentication for encrypted connections between Windows endpoints in isolated network segments. Failure events from IPSec extended mode indicate endpoints attempting cross-segment communication that is blocked by IPSec policy — a potential indicator of lateral movement attempts that a compromised endpoint's attacker is trying to reach an isolated server segment. Extended mode failures highlight network segmentation policy violations in real time.",
                    Tags = ["logon-audit", "ipsec", "ike", "extended-mode", "network-segmentation", "firewall"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "IPSec extended mode authentication events audited; cross-segment communication failures generate events indicating lateral movement.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditIPSecExtendedMode", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditIPSecExtendedMode")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditIPSecExtendedMode", 3)],
                },
                new TweakDef
                {
                    Id = "logonaudit-audit-user-device-claims",
                    Label = "Logon Audit: Enable User and Device Claims Auditing for Dynamic Access Control",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditUserDeviceClaims=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4626 at logon time, which records the user and device claims embedded in the Kerberos authentication token when Dynamic Access Control (DAC) is used — providing visibility into the claims used for conditional access decisions in DAC-protected file server and classification label systems. "
                        + "Dynamic Access Control uses Kerberos claims (user department, device compliance state, classification clearance level) to make file access decisions on Windows Server file shares. A user whose Kerberos token contains an incorrect department claim (e.g., claim was modified at token issue time by a Kerberos token forgery attack) could gain access to files classified for a different department. Event 4626 records the actual claims present at logon time, enabling post-incident review of whether inappropriate access was gated on correct claim values.",
                    Tags = ["logon-audit", "claims", "dynamic-access-control", "event-4626", "kerberos", "dac"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "User/device Kerberos claims logged at logon (4626); Dynamic Access Control claim-based access decisions auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditUserDeviceClaims", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditUserDeviceClaims")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditUserDeviceClaims", 1)],
                },
            ];
    }

    // ── ObjectAccessPolicy ──
    private static class _ObjectAccessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ObjectAccess";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "objacs-enable-file-system-auditing",
                Label = "Enable File System Object Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "File system object access auditing records access to files and directories that have SACL entries configured for auditing. Enabling file system auditing generates security events for file access operations including read, write, create, and delete when the object's SACL requests auditing. File access auditing is essential for detecting unauthorized access to sensitive files and directories in enterprise environments. Security Event Log events 4663 and 4656 record file access with details about the user, process, file path, and access type. File system auditing log data supports DLP investigations, insider threat detection, and forensic analysis after security incidents. Organizations should configure SACLs on sensitive directories and enable this policy to ensure audit events are generated.",
                Tags = ["object-access", "file-system", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableFileSystemAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableFileSystemAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableFileSystemAuditing", 1)],
            },
            new TweakDef
            {
                Id = "objacs-enable-registry-auditing",
                Label = "Enable Registry Object Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Registry object access auditing records access to registry keys that have SACL entries configured requesting audit events. Enabling registry auditing generates security events for registry read and write operations on monitored keys. Registry modification auditing is critical for detecting persistence mechanisms that write to run keys, service configurations, and authentication providers. Security Event Log events 4663 and 4657 record registry access with account, key path, and operation type information. Registry auditing of sensitive keys like HKLM\\SYSTEM\\CurrentControlSet\\Services provides early warning of service-based persistence. Organizations should configure SACLs on high-value registry paths and ensure this policy is enabled for audit event generation.",
                Tags = ["object-access", "registry", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRegistryAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRegistryAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRegistryAuditing", 1)],
            },
            new TweakDef
            {
                Id = "objacs-enable-kernel-object-auditing",
                Label = "Enable Kernel Object Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Kernel object auditing records access to kernel objects such as mutexes, semaphores, and event objects that have SACL-based audit entries. Enabling kernel object auditing provides visibility into inter-process synchronization and communication through kernel objects. Malware commonly uses named kernel objects for synchronization and coordination between malicious processes in multi-stage attacks. Security events for kernel object access help identify attacker-created synchronization primitives used for process coordination. Kernel object auditing is lower volume than file system auditing but provides targeted visibility into process behavior. High-value kernel objects like named mutexes known to be used by specific malware families should be configured with SACLs.",
                Tags = ["object-access", "kernel", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableKernelObjectAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelObjectAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKernelObjectAuditing", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-sam-access",
                Label = "Enable SAM Database Object Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SAM database object access auditing records attempts to access local account credentials stored in the Security Account Manager database. Enabling SAM access auditing generates security events when processes attempt to open the SAM database for credential access. SAM database access is a common credential harvesting technique used by tools like Mimikatz and similar password dumping utilities. Security Event Log event 4661 records SAM object access with the requesting account and process identifier for forensic analysis. SAM access auditing helps detect credential dumping activity even when it occurs through APIs rather than raw disk access. Detecting SAM access events should be correlated with other artifacts like LSASS process access and unusual administrative tool execution.",
                Tags = ["object-access", "sam", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSAMAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSAMAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSAMAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-lsass-access",
                Label = "Enable LSASS Process Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "LSASS process object auditing records attempts by other processes to open handles to the LSASS process with credential-reading access rights. Enabling LSASS access auditing generates security events when processes attempt to read memory from the Local Security Authority Server Service. Credential dumping tools including Mimikatz, Procdump, and comsvcs.dll extraction all require opening LSASS with PROCESS_VM_READ permissions. Security Event Log event 4656 and 10 from Sysmon can detect LSASS credential access attempts from unauthorized processes. LSASS access detection is one of the most important detections for credential-based lateral movement in enterprise environments. Detecting LSASS access should trigger immediate investigation as legitimate software rarely accesses LSASS process memory.",
                Tags = ["object-access", "lsass", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditLSASSAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditLSASSAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditLSASSAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-enable-detailed-file-share-audit",
                Label = "Enable Detailed File Share Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Detailed file share auditing records individual file-level access within network shares rather than just share connection events. Enabling detailed file share auditing generates security events with specific file paths, access types, and requestor identities for all share file access. Standard file share auditing only records share connections but detailed auditing provides visibility into which specific files are accessed. Detailed file share audit events are more voluminous than connection-level events and may require additional log infrastructure capacity. Security Event Log event 5145 records detailed file share access with object name, access mask, and account information. Detailed file share auditing is valuable for DLP scenarios and post-incident investigation of data access patterns.",
                Tags = ["object-access", "file-share", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDetailedFileShareAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedFileShareAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDetailedFileShareAudit", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-removable-storage",
                Label = "Enable Removable Storage Access Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Removable storage object access auditing records all file access and write operations to USB drives, external hard drives, and other removable media. Enabling removable storage auditing generates security events when users read from or write to removable storage devices. Data exfiltration via USB is a persistent insider threat vector and removable storage auditing provides the evidence chain needed for investigation. Security Event Log event 4663 with object type Removable Storage records the file path, access type, and user for each removable storage operation. Removable storage audit events should be correlated with USB device connection events to identify devices connected for purpose of data exfiltration. Removable storage auditing is most valuable in combination with removable storage access restrictions to detect circumvention attempts.",
                Tags = ["object-access", "removable-storage", "usb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditRemovableStorageAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableStorageAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRemovableStorageAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-cert-services",
                Label = "Enable Certification Authority Object Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Certificate Authority object access auditing records access to CA database objects and certificate management operations. Enabling CA object auditing generates security events for certificate issuance, revocation, template access, and CA configuration changes. Unauthorized certificate issuance from enterprise CAs is a serious threat enabling creation of forged authentication certificates. Security Event Log events 4874, 4875, and related CA events record certificate operations with requestor identity and certificate details. CA object auditing is essential for detecting certificate-based attacks including unauthorized administrator certificate issuance for authentication bypass. CA audit events should be aggregated with other PKI infrastructure events for comprehensive monitoring.",
                Tags = ["object-access", "pki", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditCertificationServicesAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificationServicesAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCertificationServicesAccess", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-object-handle-manipulation",
                Label = "Enable Object Handle Manipulation Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Object handle manipulation auditing records when handles to auditable objects are created or closed providing a complete access lifecycle view. Enabling handle manipulation auditing generates security events for handle creation and close operations that bracket actual object access. Handle auditing provides context for other object access events by establishing when access windows opened and closed. Security Event Log event 4659 records object deletion after handle closure providing tracking for file deletion operations. Handle lifecycle auditing is used in detailed forensic analysis to reconstruct object access timelines. Handle manipulation events on critical objects like SAM, LSASS, and sensitive files provide complementary evidence for access investigations.",
                Tags = ["object-access", "handles", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditHandleManipulation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditHandleManipulation")],
                DetectOps = [RegOp.CheckDword(Key, "AuditHandleManipulation", 1)],
            },
            new TweakDef
            {
                Id = "objacs-audit-central-access-policy",
                Label = "Enable Central Access Policy Staging Auditing",
                Category = "Security — Event Log Channel",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Central Access Policy staging auditing records what central access policy would have done when applied to object access requests before policies are enforced. Enabling staging audit mode generates security events showing how new central access policies would affect access without blocking current users. CAP staging allows administrators to test Dynamic Access Control policies and identify unexpected effects before enforcement. Security Event Log events in staging mode identify which policy expressions matched and what access decisions would result. Staging audit data enables policy refinement to remove overly restrictive rules that would block legitimate access. Central access policy staging is essential for large enterprise DAC deployments where policy errors could affect many users.",
                Tags = ["object-access", "central-access", "dac", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditCentralAccessPolicyStaging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCentralAccessPolicyStaging")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCentralAccessPolicyStaging", 1)],
            },
        ];
    }

    // ── PrintAuditPolicy ──
    private static class _PrintAuditPolicy
    {
        private const string AudKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\AuditPrint";

        private const string PrtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtaud-enable-print-job-auditing",
                    Label = "Print Audit: Enable Print Job Audit Events",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditPrintJobs=1 in AuditPrint policy. Enables security audit events for every print job processed by the Windows print spooler. When enabled, the Windows Security event log receives Event ID 4624 (document print event) for each job including: user name, computer name, printer name, document name, job ID, number of pages, and bytes printed. This provides a complete record of document print activity — essential for data loss prevention auditing (detecting mass printing of PII), compliance (HIPAA, SOX printed document requirements), and forensic investigation.",
                    Tags = ["print-audit", "security-log", "dlp", "print-jobs", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Every print job generates a security audit event. Security event log volume increases — ensure the event log size is sufficient and logs are forwarded to a SIEM. Document names in the log may contain sensitive information from the job metadata.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditPrintJobs", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditPrintJobs")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditPrintJobs", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-printer-config-auditing",
                    Label = "Print Audit: Enable Printer Configuration Change Auditing",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditPrinterConfiguration=1 in AuditPrint policy. Enables audit events when printer configuration changes are made: printer added, printer deleted, default printer changed, printer properties modified, printer sharing enabled or disabled. Unauthorised printer configuration changes can be used by attackers to redirect print jobs (malicious printer substitution attack) or to create new printer shares for lateral movement. Configuration change auditing creates an immutable log of every printer infrastructure modification for forensic review.",
                    Tags = ["print-audit", "configuration", "printer-add", "security", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Printer configuration changes generate audit events. SIEM rules for suspicious printer configuration changes (printers added/modified by non-admin accounts) detect potential print spooler abuse. Minimal event volume in stable environments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditPrinterConfiguration", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditPrinterConfiguration")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditPrinterConfiguration", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-driver-install-auditing",
                    Label = "Print Audit: Enable Printer Driver Installation Auditing",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditDriverInstall=1 in AuditPrint policy. Enables audit events for printer driver installation and removal operations. Printer driver installations are a critical security event path — PrintNightmare and related exploits specifically used driver installation as the code execution vector. Auditing every driver install event provides a detection opportunity: SIEM rules can alert on driver installations by non-IT accounts, installations of unexpected driver names, or driver installs that occur at unusual times. Complements the restriction policies that require admin rights for driver installation.",
                    Tags = ["print-audit", "driver-install", "printnightmare", "security", "detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Printer driver installations and removals generate audit events. Alerts on unexpected driver installs are a high-fidelity PrintNightmare indicator. Negligible event volume in controlled environments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditDriverInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditDriverInstall")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditDriverInstall", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-print-server-connections",
                    Label = "Print Audit: Enable Audit for Print Server Connection Events",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditServerConnections=1 in AuditPrint policy. Enables audit events when clients connect to and disconnect from the print server's spooler service via RPC. Each connection event records the client machine name, user account, and connection timestamp. Print server connection auditing is particularly valuable for detecting exploitation of print spooler RPC vulnerabilities: an attacker scanning for PrintNightmare-vulnerable servers will generate connection events before any exploit payload is sent. The connection pattern (connection from unusual machines, outside business hours) is detectable.",
                    Tags = ["print-audit", "server-connections", "rpc", "security", "detection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print server RPC connection and disconnection events are logged. In environments with many print clients, this generates high event volume. Consider applying to high-value print servers only and forwarding to central SIEM for analysis.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditServerConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditServerConnections")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditServerConnections", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-set-print-log-max-7days",
                    Label = "Print Audit: Retain Print Audit Log for 7 Days",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditLogRetentionDays=7 in AuditPrint policy. Sets the minimum retention period for print audit log entries to 7 days. Print audit log retention of at least 7 days satisfies most operational investigation requirements: typical incident detection occurs within 24-48 hours, and 7 days provides sufficient lookback to correlate print events with the full timeline of an incident. Retaining logs beyond 30 days without SIEM export strains local storage on print servers. This policy sets the minimum — logs should be forwarded to a SIEM for long-term retention independently.",
                    Tags = ["print-audit", "log-retention", "compliance", "siem", "investigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print audit logs are retained locally for at minimum 7 days. SIEM forwarding is recommended for longer retention. Local disk space consumption is proportional to job volume.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditLogRetentionDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditLogRetentionDays")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditLogRetentionDays", 7)],
                },
                new TweakDef
                {
                    Id = "prtaud-disable-direct-printing-bypass",
                    Label = "Print Audit: Disable Direct Printing Bypass (Enforce Spooler Path)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets DisableDirectPrinting=1 in Printers policy. Prevents applications from sending print jobs directly to printer hardware ports, bypassing the Windows print spooler. Applications that print directly to a port (WriteFile to LPT1:, socket to port 9100, or direct Win32 printer I/O) bypass the entire print audit chain — no job events, no audit log, no DLP scanning. Enforcing the spooler path ensures all print output is intercepted, logged, and subject to print quota policies. Required for complete print audit coverage.",
                    Tags = ["direct-printing", "spooler-bypass", "dlp", "audit", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Applications that bypass the spooler with direct port I/O (legacy manufacturing, point-of-sale, label printers) may stop printing. Test with all applications that use non-standard printing methods before deploying. Standard Windows GDI/WDM/XPS printing paths are unaffected.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableDirectPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableDirectPrinting")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableDirectPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-page-count-tracking",
                    Label = "Print Audit: Enable Per-User Print Page Count Tracking",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets EnablePageTracking=1 in AuditPrint policy. Enables per-user print page count tracking in the Windows print spooler. Page count data is accumulated in the print quota subsystem and can be consumed by print accounting software, print management consoles, and quota enforcement systems. Without page tracking, print accountability is based on job counts rather than page volumes — a user printing 500-page documents daily appears identical to one printing 10 single-page emails. Page tracking is prerequisite to enforcing any meaningful print volume policy.",
                    Tags = ["print-audit", "page-tracking", "quota", "accounting", "usage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Per-user and per-printer page count data is tracked. Negligible overhead. Data is accessible via Print Management console and print accounting APIs. Does not enforce quotas by itself — pair with a print quota enforcement solution.",
                    ApplyOps = [RegOp.SetDword(AudKey, "EnablePageTracking", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "EnablePageTracking")],
                    DetectOps = [RegOp.CheckDword(AudKey, "EnablePageTracking", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-restrict-color-printing",
                    Label = "Print Audit: Restrict Colour Printing to Authorised Users",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets RestrictColorPrinting=1 in AuditPrint policy. Restricts colour printing capability on managed printers to users who are members of an authorised colour printing security group. All other users are limited to monochrome (black and white) output. Colour printing costs are typically 5-10× higher than monochrome per page. Unrestricted colour printing is a significant operational cost driver in large organisations. Restricting colour printing to users with a business need (design, marketing, executive) provides measurable cost reduction without impacting most users.",
                    Tags = ["print-audit", "colour-printing", "cost-control", "restriction", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Colour printing is restricted to authorised users. Unauthorised users print in monochrome regardless of printer capability. Colour authorisation group must be configured in print server properties. Significant toner cost reduction in large deployments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "RestrictColorPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "RestrictColorPrinting")],
                    DetectOps = [RegOp.CheckDword(AudKey, "RestrictColorPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-enable-secure-print-release",
                    Label = "Print Audit: Enable Secure Print Release (Hold-and-Release)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets EnableSecurePrint=1 in AuditPrint policy. Enables print job hold-and-release (secure print) mode: jobs are queued on the print server but not released to the physical printer until the user authenticates at the printer panel (PIN, smart card, or badge). Documents are not printed and left unattended on the printer tray — a significant physical security and confidentiality control. Sensitive documents printed to shared office printers routinely sit uncollected for minutes to hours. Secure print release eliminates physical information disclosure.",
                    Tags = ["print-audit", "secure-print", "hold-release", "physical-security", "confidentiality"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print jobs are held until the submitter authenticates at the printer. Requires printer hardware that supports hold-and-release (most enterprise MFPs). Users must approach the printer to release jobs. Uncollected jobs expire after the configured timeout.",
                    ApplyOps = [RegOp.SetDword(AudKey, "EnableSecurePrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "EnableSecurePrint")],
                    DetectOps = [RegOp.CheckDword(AudKey, "EnableSecurePrint", 1)],
                },
                new TweakDef
                {
                    Id = "prtaud-log-deleted-print-jobs",
                    Label = "Print Audit: Log Deleted and Cancelled Print Jobs",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditDeletedJobs=1 in AuditPrint policy. Enables audit events when print jobs are deleted or cancelled from the print queue. Print job deletion events capture the who (user account that cancelled), what (document name, printer, job ID), and when (timestamp). Deletions by accounts that did not submit the job indicate queue manipulation — an administrator (or attacker with elevated privileges) deleting another user's print job. This is relevant in secure print environments where deleted-before-release events indicate tampering with the print queue.",
                    Tags = ["print-audit", "deleted-jobs", "queue-manipulation", "security", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Cancelled and deleted print jobs generate audit events. SIEM correlation of the submitter vs. the deleting account detects queue manipulation. Negligible event volume in normal environments.",
                    ApplyOps = [RegOp.SetDword(AudKey, "AuditDeletedJobs", 1)],
                    RemoveOps = [RegOp.DeleteValue(AudKey, "AuditDeletedJobs")],
                    DetectOps = [RegOp.CheckDword(AudKey, "AuditDeletedJobs", 1)],
                },
            ];
    }

    // ── PrivilegeUseAuditPolicy ──
    private static class _PrivilegeUseAuditPolicy
    {
        private const string PrivKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Privilege Use";
        private const string AclKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Object Access";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "privaudit-audit-sensitive-privilege-use",
                    Label = "Privilege Audit: Enable Auditing of Sensitive Privilege Use (SeDebug, SeTcb, SeBackup)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets 'Audit Sensitive Privilege Use'=3 (Success+Failure) in the Advanced Audit Policy. Generates Security event 4673/4674 whenever a process invokes a sensitive privilege — SeDebugPrivilege (used by Mimikatz for LSASS dump), SeTcbPrivilege (act as operating system), SeBackupPrivilege (bypass file ACLs for backup), SeRestorePrivilege, SeTakeOwnershipPrivilege — providing direct detection signal for privilege-abuse attack techniques. "
                        + "SeDebugPrivilege invocation is a binary trigger for LSASS credential dumping — every major credential harvesting tool (Mimikatz, ProcDump LSASS, Task Manager LSASS dump) requires SeDebugPrivilege to access LSASS memory. Auditing sensitive privilege use generates Security event 4673 the instant any process invokes SeDebugPrivilege, providing near-real-time detection of credential theft attempts through SIEM correlation — typically one of the highest-fidelity, lowest-noise detection rules in an enterprise SIEM.",
                    Tags = ["privilege-audit", "sensitive-privilege", "sedebug", "mimikatz", "lsass", "credential-theft"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Sensitive privilege use events generated; SeDebugPrivilege (Mimikatz/LSASS dump) detection in near-real-time.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditSensitivePrivilegeUse", 3)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditSensitivePrivilegeUse")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditSensitivePrivilegeUse", 3)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-nonsensitive-privilege-use",
                    Label = "Privilege Audit: Enable Auditing of Non-Sensitive Privilege Use (SeShutdown, SeLoad)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets 'Audit Non-Sensitive Privilege Use'=1 (Success) in the Advanced Audit Policy. Generates Security event 4673/4674 for non-sensitive privilege invocations (SeShutdownPrivilege, SeUndockPrivilege, SeLoadDriverPrivilege, SeSystemtimePrivilege, SeTimeZonePrivilege, SeChangeNotifyPrivilege). Non-sensitive privilege events complement sensitive privilege events to provide a complete picture of privilege hierarchy escalation. "
                        + "SeLoadDriverPrivilege invocation is the second critical attack signal — attackers who load a signed-but-vulnerable driver as a vector for privilege escalation (BYOVD, Bring Your Own Vulnerable Driver) must invoke SeLoadDriverPrivilege to install the driver. Auditing this privilege provides detection for BYOVD attacks (used by Lazarus Group, BlackMatter ransomware) before the vulnerable driver is loaded and exploited.",
                    Tags = ["privilege-audit", "nonsensitive-privilege", "seloaddriver", "byovd", "driver-exploit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Non-sensitive privilege invocations audited; SeLoadDriverPrivilege (BYOVD attack vector) detectable.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditNonSensitivePrivilegeUse", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditNonSensitivePrivilegeUse")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditNonSensitivePrivilegeUse", 1)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-other-privilege-use-events",
                    Label = "Privilege Audit: Enable 'Other Privilege Use Events' for Complete Privilege Coverage",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets 'Audit Other Privilege Use Events'=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events for miscellaneous privilege use scenarios not captured by Sensitive or Non-Sensitive subcategories, including encrypted data recovery, user right assignments via Direct Access, and scheduled task privilege overrides. Completes the privilege use audit coverage across all three subcategories. "
                        + "The 'Other Privilege Use Events' subcategory captures edge-case privilege invocations that don't neatly fit the Sensitive/Non-Sensitive taxonomy — including cross-domain encrypted data access (EFS recovery) and some legacy DCOM privilege transitions. While individually lower-signal than SeDebugPrivilege events, collectively these events fill gaps in the privilege audit trail that sophisticated threat actors may attempt to exploit by routing privilege escalation through lesser-audited paths.",
                    Tags = ["privilege-audit", "other-privilege", "efs", "dcom", "complete-coverage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Other privilege use events audited; complete privilege audit coverage across all three subcategories.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditOtherPrivilegeUseEvents", 3)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditOtherPrivilegeUseEvents")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditOtherPrivilegeUseEvents", 3)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-file-system-failures",
                    Label = "Privilege Audit: Enable File System Access Failure Auditing for ACL Bypass Detection",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets 'Audit File System Failures'=2 (Failure) in the Advanced Audit Policy Object Access category. Generates Security event 4656/4663 (Failure) whenever a process is denied access to a file or folder due to DACL permissions, recording the file path, access type requested, requesting process, and user account — providing detection for access scanning and ACL enumeration attacks. "
                        + "Access failure events are high-signal early warning indicators for insider threat and lateral movement reconnaissance. A compromised account scanning the file system for accessible data will generate hundreds of access failure events as it attempts to read protected files and directories above its permission level. A volume spike in Event 4656 Failure events from a single user account is a reliable indicator of data access scanning or Shadow IT application attempting to read sensitive data repositories.",
                    Tags = ["privilege-audit", "file-system", "access-failure", "acl", "insider-threat", "scanning"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "File system access failure events generated; ACL bypass attempts and access scanning produce high-fidelity detection events.",
                    ApplyOps = [RegOp.SetDword(AclKey, "AuditFileSystem", 2)],
                    RemoveOps = [RegOp.DeleteValue(AclKey, "AuditFileSystem")],
                    DetectOps = [RegOp.CheckDword(AclKey, "AuditFileSystem", 2)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-registry-object-access",
                    Label = "Privilege Audit: Enable Sensitive Registry Key Access Auditing",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets 'Audit Registry Object Access'=3 (Success+Failure) in the Advanced Audit Policy Object Access category. Generates Security events 4656/4663 for registry key access operations on SACL-protected registry keys (keys with an assigned Security Audit ACL), enabling detection of access to AutoRun keys, service configuration keys, and other persistence mechanism registry locations. "
                        + "Registry-based persistence (Run keys, Services, COM hijacking targets) are the most common dwell-time persistence mechanisms. Auditing access to SACL-protected registry keys (HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Run, HKLM\\SYSTEM\\CurrentControlSet\\Services, HKLM\\SOFTWARE\\Classes\\CLSID) detects both initial persistence registration (write access) and the periodic re-invocation of persistence (read access at logon). When SACL-protected keys are configured on high-value locations, SIEM rules can alert on unexpected write access creating new persistence entries.",
                    Tags = ["privilege-audit", "registry", "sacl", "persistence", "run-keys", "com-hijacking"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "SACL-protected registry keys generate access events; persistence mechanism modifications detectable via event correlation.",
                    ApplyOps = [RegOp.SetDword(AclKey, "AuditRegistry", 3)],
                    RemoveOps = [RegOp.DeleteValue(AclKey, "AuditRegistry")],
                    DetectOps = [RegOp.CheckDword(AclKey, "AuditRegistry", 3)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-removable-storage-access",
                    Label = "Privilege Audit: Enable Removable Storage Access Audit Events for USB DLP",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets 'Audit Removable Storage'=3 (Success+Failure) in the Advanced Audit Policy Object Access category. Generates Security event 4663 for all read and write operations to removable storage devices (USB drives, SD cards, DVD writers), recording the file name, operation type, and user account for every file accessed on removable media — enabling DLP monitoring without a dedicated DLP agent. "
                        + "Removable storage audit provides per-file visibility of data access on USB drives. Where standard PnP audit (plug/unplug events) only shows that a device was connected, removable storage audit shows exactly which files were copied to or read from the device. This enables insider threat scenarios to be reconstructed precisely — ACME employee connected USB drive X at 14:32, copied 47 files totalling 2.3 GB from the SharePoint mapped drive, disconnected at 14:35 — from on-device event log evidence alone.",
                    Tags = ["privilege-audit", "removable-storage", "usb", "dlp", "insider-threat", "data-exfiltration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Per-file removable storage access audited; USB data exfiltration reconstructable at file level from Security event log.",
                    ApplyOps = [RegOp.SetDword(AclKey, "AuditRemovableStorage", 3)],
                    RemoveOps = [RegOp.DeleteValue(AclKey, "AuditRemovableStorage")],
                    DetectOps = [RegOp.CheckDword(AclKey, "AuditRemovableStorage", 3)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-token-right-adjustment",
                    Label = "Privilege Audit: Enable Token Privilege Adjustment Auditing for UAC Bypass Detection",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditTokenPrivilegeAdjustment=3 (Success+Failure) in the Windows System policy privilege section. Generates Security event 4703 (Token privilege adjustment) when a process enables or disables a privilege in its own access token, providing detection for UAC bypass techniques that involve enabling disabled privileges in a standard user token to perform privileged operations without triggering a UAC prompt. "
                        + "Many UAC bypass techniques (mockdirs, fodhelper, eventvwr, DLL UAC auto-elevations) work by enabling privileges that are present but disabled in the current token (e.g., SeImpersonatePrivilege, SeAssignPrimaryTokenPrivilege) through techniques that avoid the standard UAC elevation flow. Token privilege adjustment events (4703) generated when these operations occur provide a direct detection signal for UAC bypass patterns — especially in combination with process creation events showing the bypassed elevated process that spawns immediately after the token adjustment.",
                    Tags = ["privilege-audit", "token-adjustment", "uac-bypass", "event-4703", "impersonation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Token privilege adjustments generate Event 4703; UAC bypass techniques involving token privilege enabling detectable.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditTokenPrivilegeAdjustment", 3)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditTokenPrivilegeAdjustment")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditTokenPrivilegeAdjustment", 3)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-special-logon",
                    Label = "Privilege Audit: Enable Special Logon Auditing (Admin Equivalent or Special Group Logons)",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditSpecialLogon=1 (Success) in the Advanced Audit Policy Logon/Logoff category. Generates Security event 4964 (Special groups assigned to new logon) when an Entra ID or domain user whose account is a member of a Special Groups audit list logs on, providing targeted monitoring for high-privilege accounts without the event volume of full logon auditing for all users. "
                        + "Special Logon auditing enables selective privileged account monitoring. By configuring the Special Groups list to include Domain Admins, Enterprise Admins, Backup Operators, and other critical security groups, the enterprise gets immediate Security event notification every time any member of those groups authenticates to any endpoint in the domain — without generating Event 4624 for every employee logon. This powers 'privileged account logon monitoring' SIEM rules with precise scope and minimal noise.",
                    Tags = ["privilege-audit", "special-logon", "event-4964", "admin-monitoring", "privileged-accounts"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Special group logons generate Event 4964; privileged account authentication to any endpoint monitored in real time.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditSpecialLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditSpecialLogon")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditSpecialLogon", 1)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-sam-sam-access",
                    Label = "Privilege Audit: Enable SAM Database Access Auditing for Credential Database Protection",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditSAMAccess=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events when the Security Account Manager (SAM) database is accessed, providing detection for credential dumping techniques that target the local SAM database (offline dump of SYSTEM and SAM hive, volume shadow copy SAM extraction, or SecretsDump against local accounts). "
                        + "The SAM database contains the NTLM password hashes for all local Windows user accounts. SAM database access is a common post-exploitation step — after gaining SYSTEM privileges, threat actors extract SAM to harvest local account hashes for Pass-the-Hash attacks or for offline cracking. Auditing SAM access generates Security events whenever the SAM hive is opened with access beyond normal system operations, providing detection signals for credential harvesting operations against local accounts.",
                    Tags = ["privilege-audit", "sam", "credential-dumping", "ntlm", "pass-the-hash", "secretsdump"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "SAM database access audited; credential dumping attempts targeting local account hashes generate Security events.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditSAMAccess", 3)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditSAMAccess")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditSAMAccess", 3)],
                },
                new TweakDef
                {
                    Id = "privaudit-audit-lsa-secrets-access",
                    Label = "Privilege Audit: Enable LSA Secrets Access Auditing for Service Credential Protection",
                    Category = "Security — Event Log Channel",
                    Description =
                        "Sets AuditLSASecretsAccess=3 (Success+Failure) in the Advanced Audit Policy. Generates Security events when the Local Security Authority (LSA) secrets store is accessed, detecting attempts to harvest service account credentials and DPAPI master keys stored in the LSA secrets store by tools such as Mimikatz's lsadump::secrets command or reg.exe SYSTEM hive extraction. "
                        + "LSA secrets contain auto-logon account passwords, service account passwords for Windows services configured to run as domain accounts, DPAPI master key encryption keys, and cached domain credentials (DCC2 hashes). These are higher-value credentials than local SAM hashes because service account credentials are often over-provisioned domain accounts with access to multiple servers. Auditing LSA secrets access detects the critical early step of service account credential harvesting that enables subsequent lateral movement.",
                    Tags = ["privilege-audit", "lsa-secrets", "service-credentials", "dpapi", "mimikatz", "lateral-movement"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "LSA secrets access audited; service account credential harvesting (Mimikatz lsadump::secrets) generates detection events.",
                    ApplyOps = [RegOp.SetDword(PrivKey, "AuditLSASecretsAccess", 3)],
                    RemoveOps = [RegOp.DeleteValue(PrivKey, "AuditLSASecretsAccess")],
                    DetectOps = [RegOp.CheckDword(PrivKey, "AuditLSASecretsAccess", 3)],
                },
            ];
    }

    // ── ProcessCreationAuditPolicy ──
    private static class _ProcessCreationAuditPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pcaudit-enable-cmdline-in-process-creation-events",
                    Label = "Process Audit: Enable Full Command Line in Process Creation Security Events",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets ProcessCreationIncludeCmdLine_Enabled=1 in the Windows System policy. Enables Windows Security event 4688 (Process Creation) to include the full command-line argument string of the spawned process in the event, rather than only the process executable path. This allows SIEM systems to detect living-off-the-land attacks, fileless malware, and suspicious PowerShell invocations by analysing the full arguments of every process created. "
                        + "Process creation event 4688 without command-line inclusion only shows the executable path (e.g., powershell.exe), not the arguments (-EncodedCommand, -ExecutionPolicy Bypass, -WindowStyle Hidden). Without arguments visible, encoded PowerShell commands, Mimikatz execution via living-off-the-land binaries (LOLBins), and command injection attacks are almost entirely opaque in the Security event log. Command-line auditing is the foundational enabling control for advanced threat detection.",
                    Tags = ["process-audit", "cmdline", "process-creation", "event-4688", "siem", "lolbins"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Full command lines visible in Event 4688; SIEM can detect encoded/obfuscated PowerShell, LOLBins, and injection attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "ProcessCreationIncludeCmdLine_Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ProcessCreationIncludeCmdLine_Enabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ProcessCreationIncludeCmdLine_Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "pcaudit-enable-wmi-activity-auditing",
                    Label = "Process Audit: Enable WMI Activity Audit Log for Process-Level WMI Operations",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets EnableWMIActivityAudit=1 in the Windows System policy. Enables the Microsoft-Windows-WMI-Activity/Operational event log channel, causing WMI query execution, WMI provider invocations, and WMI subscription modifications to be logged. WMI is a primary lateral movement and persistence technique used by threat actors to execute code remotely without spawning a child process visible in process creation audit logs. "
                        + "WMI-based attacks (used in APT28, Carbanak, and most enterprise-targeted ransomware operators) execute payload code through the WMI provider host (WmiPrvSE.exe) as a child of svchost.exe, bypassing process creation rules that watch for powershell.exe or cmd.exe. WMI activity logging provides a parallel audit trail for WMI-executed commands that cannot be correlated from process creation events alone, enabling detection of WMI-based fileless lateral movement.",
                    Tags = ["process-audit", "wmi", "lateral-movement", "wmiprvse", "apt", "fileless"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "WMI operations logged in Activity event channel; WMI-based lateral movement and persistence detectable by EDR/SIEM.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableWMIActivityAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableWMIActivityAudit")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableWMIActivityAudit", 1)],
                },
                new TweakDef
                {
                    Id = "pcaudit-enable-psh-module-logging",
                    Label = "Process Audit: Enable PowerShell Module Logging for All Script Block Execution",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets EnableModuleLogging=1 in the Windows System policy. Enables PowerShell module logging, which records the full content of every PowerShell pipeline execution (all commands, scripts, and functions invoked) to the PowerShell event log (Microsoft-Windows-PowerShell/Operational, Event ID 4103), providing complete visibility into what code PowerShell executes even when scripts are obfuscated. "
                        + "PowerShell is the most commonly abused administrative tool for post-exploitation activities. Module logging captures the deobfuscated execution of AMSI-aware scripts — when a malicious actor uses encoded base64 commands or string manipulation to evade static detection, PowerShell must decode the payload before execution. Module logging captures the post-decode execution pipeline, revealing the actual malicious commands regardless of the obfuscation layering.",
                    Tags = ["process-audit", "powershell", "module-logging", "obfuscation", "amsi", "script-block"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "PowerShell module logging active; all PowerShell execution including decoded obfuscated commands visible in event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableModuleLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableModuleLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableModuleLogging", 1)],
                },
                new TweakDef
                {
                    Id = "pcaudit-enable-psh-script-block-logging",
                    Label = "Process Audit: Enable PowerShell Script Block Logging with Obfuscation Auto-Logging",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets EnableScriptBlockLogging=1 in the Windows System policy. Enables PowerShell Script Block logging (Event ID 4104), which captures every script block (function body, scriptblock literal, and processed script pipeline) executed by PowerShell into the event log. When combined with AMSI integration, suspicious script block content is automatically promoted to 'suspicious script block' events (4104 with level Warning) without requiring rule tuning. "
                        + "Script block logging is stronger than module logging because it operates at a lower level (the PowerShell engine's block compilation step) and captures the content of scripts before they are executed, even when the script is loaded from memory or piped from another command. Script block logging is complementary to AMSI — AMSI inspects content before execution for malware signatures; script block logging captures all execution for post-incident investigation.",
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
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets EnableTranscripting=1 in the Windows System policy. Enables PowerShell transcription, which writes a text transcript of every PowerShell session (all input commands and output) to a log file. When combined with a centralised transcript output directory (network share or DFS path), all PowerShell session activity from all endpoints is written to a central searchable store. "
                        + "PowerShell transcripts capture information that neither script block logging nor module logging captures: the full interactive session flow including the output returned by commands (e.g., the contents of Get-ChildItem output, netstat results captured by commands, or credentials visible in command output). While transcripts are more verbose than event log entries, they provide a continuous narrative of a PowerShell session that is invaluable for incident reconstruction when reconstructing what a threat actor did during a dwell-time period.",
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
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets EnableProtectedEventLogging=1 in the Windows System policy. Enables Protected Event Logging, which encrypts the content of sensitive PowerShell script block log entries (Event 4104) using a specified asymmetric public key certificate, so that the log content can only be read by the private key holder on the log analysis server, protecting sensitive command content (passwords, tokens) in the event log from local plaintext exposure. "
                        + "Standard PowerShell script block logging writes command content in plaintext to the event log. If an administrative PowerShell script processes credentials, API keys, or sensitive data, those values appear in the local Security event log in cleartext. Any process with read access to the Security event log (including some malware) can harvest these credentials from the log. Protected Event Logging encrypts sensitive entries, allowing detection while protecting the content from local extraction.",
                    Tags = ["process-audit", "powershell", "protected-event-logging", "encryption", "credentials", "log-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "PowerShell event log entries encrypted with PKI certificate; sensitive commands protected from local plaintext access.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableProtectedEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableProtectedEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableProtectedEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "pcaudit-enable-security-audit-process-termination",
                    Label = "Process Audit: Enable Security Audit Events for Process Termination (Event 4689)",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets AuditProcessTermination=1 in the Windows System policy. Enables Security event log event 4689 (A process has exited), which records the process name, PID, user account, and exit code when any process terminates. When correlated with Event 4688 (process creation), this enables calculation of exact process lifetimes, detection of very-short-lived suspicious processes, and analysis of process trees during incident investigation. "
                        + "Process termination audit enables detection of living-off-the-land binary (LOLBin) usage where a legitimately signed binary (e.g., certutil.exe, regsvr32.exe) is spawned, executes a malicious payload, and exits in milliseconds. Without process termination events, the SIEM only has the creation event and no end marker, making it impossible to calculate process lifetime or determine what happened between creation and exit. Short-lifetime processes (sub-second) that accomplish significant work are high-fidelity attack indicators.",
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
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets AuditPNPActivity=1 in the Windows System policy. Enables Security event log events 6416/6419/6420/6421/6423/6424 (Plug and Play activity) that record when new hardware devices are connected or disconnected from the system, including USB drives, network adapters, Bluetooth dongles, and other peripherals — recording the device ID, device type, and connecting user account. "
                        + "USB removable storage is a primary exfiltration vector and a common way to deliver malware (BadUSB, autorun malware). Without PnP audit events, there is no Security event log record of which USB devices were connected, to which endpoints, by which user, at what time. PnP audit events provide DLP and insider threat detection capability — a user who copies data to a USB drive that was connected to their endpoint for 3 minutes generates a complete audit trail of the connection without requiring endpoint DLP software.",
                    Tags = ["process-audit", "pnp", "usb", "device-connection", "exfiltration", "baduusb"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "USB/PnP device connections generate Security events; device connection history auditable for exfiltration detection.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditPNPActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditPNPActivity")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditPNPActivity", 1)],
                },
                new TweakDef
                {
                    Id = "pcaudit-enable-network-connection-events-sysmon-style",
                    Label = "Process Audit: Enable Network Connection Events in Windows Event Log Without Sysmon",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets AuditNetworkConnectionEvents=1 in the Windows System policy. Enables network connection audit events in the Security event log, recording each TCP/UDP connection attempt with the originating process ID, source address/port, and destination address/port, providing network process binding visibility without requiring Sysmon or third-party endpoint agents. "
                        + "Network connection logging is standard in Sysmon Event ID 3, but many enterprises cannot deploy Sysmon due to policy or operational constraints. Windows Security event log network connection auditing (when configured) provides a subset of the same visibility natively. Detecting beaconing to C2 infrastructure requires correlation of process creation events with the network connections those processes make. Without network connection events, process creation auditing alone cannot establish which external hosts a suspicious process contacted.",
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
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets SecurityEventLogMinSizeMB=512 in the Windows System policy. Enforces a minimum Security event log file size of 512 MB, ensuring that the on-device event log buffer is large enough to sustain at least 30 days of security audit event retention without log rotation truncating investigative evidence before it can be forwarded to a SIEM. "
                        + "The default Windows Security event log size is 20 MB. With process creation auditing, command-line auditing, and PnP auditing all enabled, a busy endpoint can generate several MB of Security events per hour. A 20 MB log buffer retains as little as a few hours of events. On endpoints without a SIEM agent forwarding events in real time, a 20 MB log means that events from an overnight incident may have been overwritten before the morning IT team investigates. A 512 MB buffer provides several weeks of local retention.",
                    Tags = ["process-audit", "event-log", "retention", "siem", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Security event log minimum size 512 MB; multi-week local retention for environments without real-time SIEM forwarding.",
                    ApplyOps = [RegOp.SetDword(Key, "SecurityEventLogMinSizeMB", 512)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SecurityEventLogMinSizeMB")],
                    DetectOps = [RegOp.CheckDword(Key, "SecurityEventLogMinSizeMB", 512)],
                },
            ];
    }

    // ── SecurityAuditPolicy ──
    private static class _SecurityAuditPolicy
    {
        private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        private const string LsaPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        private const string AuditPolicy = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Audit";

        private const string KerberosParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

        private const string NetLogon = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "audit-enable-verbose-audit-policy",
                Label = "Enable Verbose Security Audit Policy Subcategory",
                Category = "Security — Process Creation Audit",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["audit", "security", "logging", "policy"],
                Description =
                    "Enables subcategory-level audit policy (Win Vista+ feature) to override "
                    + "the coarser category-level audit settings. Required for detailed event "
                    + "log entries in Security Event Log.",
                ApplyOps = [RegOp.SetDword(LsaPolicy, "SCENoApplyLegacyAuditPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaPolicy, "SCENoApplyLegacyAuditPolicy")],
                DetectOps = [RegOp.CheckDword(LsaPolicy, "SCENoApplyLegacyAuditPolicy", 1)],
            },
            new TweakDef
            {
                Id = "audit-disable-ntlm-v1",
                Label = "Disable NTLMv1 Authentication (Require NTLMv2)",
                Category = "Security — Process Creation Audit",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["audit", "ntlm", "authentication", "security", "hardening"],
                Description =
                    "Sets LmCompatibilityLevel to 5 — send NTLMv2 only, refuse LM and NTLMv1. "
                    + "Prevents pass-the-hash attacks using weak NTLMv1 hashes. "
                    + "May break legacy apps/devices that only support NTLMv1.",
                ApplyOps = [RegOp.SetDword(Lsa, "LmCompatibilityLevel", 5)],
                RemoveOps = [RegOp.SetDword(Lsa, "LmCompatibilityLevel", 3)],
                DetectOps = [RegOp.CheckDword(Lsa, "LmCompatibilityLevel", 5)],
            },
            new TweakDef
            {
                Id = "audit-disable-lm-hash-storage",
                Label = "Disable LAN Manager Hash Storage",
                Category = "Security — Process Creation Audit",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "lm hash", "password", "security", "hardening"],
                Description =
                    "Prevents Windows from storing LAN Manager password hashes in the SAM "
                    + "database. LM hashes are cryptographically weak and can be cracked quickly. "
                    + "Required for PCI DSS and CIS Windows 11 compliance.",
                ApplyOps = [RegOp.SetDword(Lsa, "NoLMHash", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "NoLMHash")],
                DetectOps = [RegOp.CheckDword(Lsa, "NoLMHash", 1)],
            },
            new TweakDef
            {
                Id = "audit-restrict-anonymous-access",
                Label = "Restrict Anonymous SAM/LSA Access",
                Category = "Security — Process Creation Audit",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["audit", "anonymous", "sam", "lsa", "security"],
                Description =
                    "Disables anonymous access to lists of SAM accounts and LSA policy "
                    + "information via null sessions. Prevents unauthenticated enumeration "
                    + "of user accounts over the network.",
                ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymousSAM", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymousSAM")],
                DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymousSAM", 1)],
            },
            new TweakDef
            {
                Id = "audit-force-audit-policy-on-logon",
                Label = "Force Audit Policy Update on Every Logon",
                Category = "Security — Process Creation Audit",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["audit", "policy", "logon", "consistency"],
                Description =
                    "Forces Windows to re-apply the audit policy from the Security database "
                    + "at every user logon. Ensures audit settings are always current even "
                    + "if domain GPO has been applied between reboots.",
                ApplyOps = [RegOp.SetDword(Lsa, "ForceGuest", 0)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "ForceGuest")],
                DetectOps = [RegOp.CheckDword(Lsa, "ForceGuest", 0)],
            },
        ];
    }

    // ── SqlServerAuditPolicy ──
    private static class _SqlServerAuditPolicy
    {
        private const string InstanceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer";
        private const string NetLibKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\SuperSocketNetLib";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sqlaup-enable-full-login-audit",
                    Label = "Enable Full SQL Server Login Audit (Success + Failure)",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets AuditLevel=3 in the MSSQLServer instance key. Controls the level of SQL Server login auditing: 0=none, 1=success only, 2=failure only, 3=both success and failure. Full auditing (level 3) records every authentication attempt to the SQL error log, enabling detection of brute-force attacks and unauthorised access. Required by most security compliance frameworks (CIS SQL Server Benchmark, STIG).",
                    Tags = ["sql-server", "audit", "login", "compliance", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Logs every SQL login attempt; increases SQL error log size on high-connection-rate servers.",
                    ApplyOps = [RegOp.SetDword(InstanceKey, "AuditLevel", 3)],
                    RemoveOps = [RegOp.DeleteValue(InstanceKey, "AuditLevel")],
                    DetectOps = [RegOp.CheckDword(InstanceKey, "AuditLevel", 3)],
                },
                new TweakDef
                {
                    Id = "sqlaup-enforce-windows-auth-only",
                    Label = "Enforce Windows Authentication Only for SQL Server",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets LoginMode=1 in the MSSQLServer instance key. Restricts SQL Server to Windows Authentication (Integrated Security) mode only, disabling SQL Server login accounts (LoginMode=2 enables mixed mode). Windows Authentication uses Kerberos or NTLM, benefits from Active Directory password policies, is audited by Windows Security event logs, and eliminates the risk of weak SQL-only passwords.",
                    Tags = ["sql-server", "authentication", "windows-auth", "security", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "Disables SQL login accounts; applications using SQL usernames/passwords must be migrated to Windows Auth first.",
                    ApplyOps = [RegOp.SetDword(InstanceKey, "LoginMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(InstanceKey, "LoginMode")],
                    DetectOps = [RegOp.CheckDword(InstanceKey, "LoginMode", 1)],
                },
                new TweakDef
                {
                    Id = "sqlaup-disable-named-pipes",
                    Label = "Disable SQL Server Named Pipe Protocol",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets NpEnabled=0 in the SQL Server SuperSocketNetLib key. Disables the Named Pipes network protocol for SQL Server connections. Named Pipes traverses SMB and can expose the SQL Server service through Windows file-sharing ports (445/TCP). Disabling Named Pipes forces all connections through TCP/IP which can be precisely port-filtered by a firewall.",
                    Tags = ["sql-server", "network", "named-pipes", "protocol", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Drops Named Pipes support; local applications using np: connection strings must switch to tcp:.",
                    ApplyOps = [RegOp.SetDword(NetLibKey, "NpEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(NetLibKey, "NpEnabled")],
                    DetectOps = [RegOp.CheckDword(NetLibKey, "NpEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "sqlaup-disable-shared-memory",
                    Label = "Disable SQL Server Shared Memory Protocol",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets SmEnabled=0 in the SQL Server SuperSocketNetLib key. Disables the Shared Memory protocol that allows local processes to connect to SQL Server via memory-mapped communication. While convenient, Shared Memory connections bypass network-layer access controls entirely. Disabling it forces all connections (even local) through explicit TCP/IP, ensuring firewall rules and port-level controls apply uniformly.",
                    Tags = ["sql-server", "network", "shared-memory", "protocol", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Drops Shared Memory; local automated tools and T-SQL jobs using shared memory connections must use TCP instead.",
                    ApplyOps = [RegOp.SetDword(NetLibKey, "SmEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(NetLibKey, "SmEnabled")],
                    DetectOps = [RegOp.CheckDword(NetLibKey, "SmEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "sqlaup-enable-tcp-protocol",
                    Label = "Ensure SQL Server TCP/IP Protocol Is Enabled",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets TcpEnabled=1 in the SQL Server SuperSocketNetLib key. Guarantees the TCP/IP network protocol is active for SQL Server, which is the only protocol that can be properly firewalled and port-filtered. Combined with disabling Named Pipes and Shared Memory, this ensures all SQL Server traffic traverses TCP so network access controls are consistently applied.",
                    Tags = ["sql-server", "network", "tcp", "protocol", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Ensures TCP is enabled; no business impact if TCP was already active (the common default).",
                    ApplyOps = [RegOp.SetDword(NetLibKey, "TcpEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetLibKey, "TcpEnabled")],
                    DetectOps = [RegOp.CheckDword(NetLibKey, "TcpEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "sqlaup-hide-sql-instance",
                    Label = "Hide SQL Server Instance from Network Browsers",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets HideInstance=1 in the MSSQLServer key. Instructs SQL Server Browser to not return the instance name in response to network enumeration requests. When hidden, clients must supply the explicit server name and port; they cannot discover it through SQL Server Browser UDP broadcasts. This reduces the attack surface by preventing automated scanners from locating the SQL instance via port 1434 UDP enumeration.",
                    Tags = ["sql-server", "browser", "discovery", "network", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hides instance from SQL Browser; connection strings must specify host\\instance explicitly.",
                    ApplyOps = [RegOp.SetDword(InstanceKey, "HideInstance", 1)],
                    RemoveOps = [RegOp.DeleteValue(InstanceKey, "HideInstance")],
                    DetectOps = [RegOp.CheckDword(InstanceKey, "HideInstance", 1)],
                },
                new TweakDef
                {
                    Id = "sqlaup-disable-xp-cmdshell-flag",
                    Label = "Record xp_cmdshell Disabled State in Registry",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets XPCmdShellEnabled=0 in the MSSQLServer key. This registry flag indicates that the xp_cmdshell extended stored procedure (which executes OS shell commands from T-SQL) must remain disabled. While the authoritative control is sp_configure inside SQL Server, recording the intended state in the registry allows compliance scanning tools that audit registry keys to verify xp_cmdshell is disabled without querying the SQL instance directly.",
                    Tags = ["sql-server", "xp-cmdshell", "compliance", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Registry flag only; xp_cmdshell must also be disabled via sp_configure inside SQL Server for full protection.",
                    ApplyOps = [RegOp.SetDword(InstanceKey, "XPCmdShellEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(InstanceKey, "XPCmdShellEnabled")],
                    DetectOps = [RegOp.CheckDword(InstanceKey, "XPCmdShellEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "sqlaup-enable-error-reporting",
                    Label = "Enable SQL Server Error Log Verbosity",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets NumErrorLogs=10 in the MSSQLServer key. Controls how many SQL Server error log files are retained in rotation. Increasing from the default (6) to 10 prevents aggressive error log cycling that could make forensic investigation of incidents difficult. Retaining more log cycles ensures a longer audit trail is available when a security incident is discovered days or weeks after it occurred.",
                    Tags = ["sql-server", "error-log", "audit", "retention", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Retains 10 rotated error log files instead of 6; negligible additional disk usage.",
                    ApplyOps = [RegOp.SetDword(InstanceKey, "NumErrorLogs", 10)],
                    RemoveOps = [RegOp.DeleteValue(InstanceKey, "NumErrorLogs")],
                    DetectOps = [RegOp.CheckDword(InstanceKey, "NumErrorLogs", 10)],
                },
                new TweakDef
                {
                    Id = "sqlaup-disable-olap-remote-connect",
                    Label = "Disable SQL Server OLAP Remote Connections Flag",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets AllowRemoteConnections=0 in the SQL Server SuperSocketNetLib key. Disables incoming remote connections through the OLAP/Analysis Services network library path. When SQL Server Analysis Services is not deployed or when OLAP connectivity should be restricted to the local machine, disabling remote connections through this protocol handler reduces the network-exposed attack surface of the SQL Server installation.",
                    Tags = ["sql-server", "olap", "remote", "network", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks OLAP remote connections; Analysis Services remote clients must connect via explicit TCP/IP instead.",
                    ApplyOps = [RegOp.SetDword(NetLibKey, "AllowRemoteConnections", 0)],
                    RemoveOps = [RegOp.DeleteValue(NetLibKey, "AllowRemoteConnections")],
                    DetectOps = [RegOp.CheckDword(NetLibKey, "AllowRemoteConnections", 0)],
                },
                new TweakDef
                {
                    Id = "sqlaup-enable-sql-server-encryption",
                    Label = "Enable SQL Server Force Encryption Flag",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Sets ForceEncryption=1 in the SQL Server SuperSocketNetLib key. Instructs SQL Server to require encrypted connections (TLS/SSL) for all client connections. Without forced encryption, clients may connect without TLS, transmitting queries and data in plaintext across the network. This registry flag mirrors the Force Encryption option in SQL Server Configuration Manager and should be set alongside a valid server certificate.",
                    Tags = ["sql-server", "encryption", "tls", "network", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Forces TLS on all connections; client connection strings must trust the SQL Server certificate or connections will fail.",
                    ApplyOps = [RegOp.SetDword(NetLibKey, "ForceEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(NetLibKey, "ForceEncryption")],
                    DetectOps = [RegOp.CheckDword(NetLibKey, "ForceEncryption", 1)],
                },
            ];
    }

    // ── WefSubscriptionPolicy ──
    private static class _WefSubscriptionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wefsubpol-enable-event-forwarding",
                    Label = "Enable Windows Event Forwarding Subscription Service",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Enables the Windows Event Collector service subscription mechanism allowing this machine to forward events to a WEF collector server via WinRM, centralising log collection in a SIEM-compatible pipeline.",
                    Tags = ["wef", "event-forwarding", "winrm", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Event forwarding enabled; logs forwarded to WEF collector. Requires WinRM and collector configured.",
                    ApplyOps = [RegOp.SetDword(Key, "SubscriptionManagerEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SubscriptionManagerEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "SubscriptionManagerEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-use-https-transport",
                    Label = "Require HTTPS Transport for Event Forwarding",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Configures Windows Event Forwarding to use HTTPS (encrypted) transport instead of plain HTTP, ensuring that event data in transit between sources and the collector cannot be intercepted.",
                    Tags = ["wef", "https", "encryption", "transport", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WEF transport set to HTTPS; event forwarding encrypted. HTTP forwarding blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceHTTPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceHTTPS")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceHTTPS", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-require-kerberos-auth",
                    Label = "Require Kerberos Authentication for Event Forwarding",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Requires Kerberos authentication for Windows Event Forwarding connections, ensuring only domain-joined machines with valid Kerberos tickets can forward events.",
                    Tags = ["wef", "kerberos", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WEF requires Kerberos auth; only domain-joined machines with valid tickets can forward events.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireKerberosAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireKerberosAuth")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireKerberosAuth", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-set-max-batch-50",
                    Label = "Set Event Forwarding Maximum Batch Size to 50",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Limits the maximum number of events in a single Windows Event Forwarding delivery batch to 50, reducing peak network bandwidth bursts while ensuring timely delivery of security events.",
                    Tags = ["wef", "batch-size", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WEF batch size limited to 50; smaller burst bandwidth, slightly more delivery requests.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxEventBatchSize", 50)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxEventBatchSize")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxEventBatchSize", 50)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-push-delivery-mode",
                    Label = "Set Event Forwarding Delivery Mode to Push",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Configures Windows Event Forwarding to use push delivery mode where the source machine initiates delivery, enabling more timely forwarding of security events versus poll-based pull mode.",
                    Tags = ["wef", "delivery-mode", "push", "timeliness", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEF delivery mode set to push; events forwarded immediately rather than awaiting collector poll.",
                    ApplyOps = [RegOp.SetDword(Key, "DeliveryMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeliveryMode")],
                    DetectOps = [RegOp.CheckDword(Key, "DeliveryMode", 0)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-block-untrusted-collectors",
                    Label = "Block Event Forwarding to Untrusted Collector Servers",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Prevents event forwarding connections to collector servers whose certificates are not trusted by the local certificate store, blocking forwarding hijacks to rogue collection endpoints.",
                    Tags = ["wef", "trusted-collector", "certificate", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Event forwarding restricted to certificate-trusted collectors; rogue WEF endpoints blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUntrustedCollectors", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUntrustedCollectors")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUntrustedCollectors", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-retain-on-failure",
                    Label = "Retain Local Events When Forwarding Fails",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Configures the event forwarding pipeline to retain events in the local event log when the collector is unreachable, ensuring no security event loss during network outages.",
                    Tags = ["wef", "retention", "resilience", "no-drop", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Events retained locally on WEF failure; no event loss when collector is unreachable.",
                    ApplyOps = [RegOp.SetDword(Key, "RetainLocalOnForwardingFailure", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RetainLocalOnForwardingFailure")],
                    DetectOps = [RegOp.CheckDword(Key, "RetainLocalOnForwardingFailure", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-log-forwarding-failures",
                    Label = "Log Event Forwarding Connection Failures",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Enables event log recording of Windows Event Forwarding connection failures, making pipeline outages visible in the local System event log for diagnostics.",
                    Tags = ["wef", "failure-logging", "connectivity", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEF connection failure events logged locally; forwarding outages are detectable via event log.",
                    ApplyOps = [RegOp.SetDword(Key, "LogForwardingFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogForwardingFailures")],
                    DetectOps = [RegOp.CheckDword(Key, "LogForwardingFailures", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-disable-unauthenticated",
                    Label = "Disable Unauthenticated Windows Event Forwarding",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Blocks event forwarding sessions that do not supply authentication credentials, preventing anonymous forwarding connections that could be used to exfiltrate logs to external endpoints.",
                    Tags = ["wef", "authentication", "security", "anonymous", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Anonymous WEF forwarding disabled; all forwarding sessions must supply credentials.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUnauthenticatedForwarding", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUnauthenticatedForwarding")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUnauthenticatedForwarding", 1)],
                },
                new TweakDef
                {
                    Id = "wefsubpol-disable-health-telemetry",
                    Label = "Disable Event Forwarding Health Telemetry to Microsoft",
                    Category = "Security — Process Creation Audit",
                    Description =
                        "Prevents the Windows Event Forwarding service from sending health and reliability telemetry about the forwarding pipeline to Microsoft, keeping internal event collection topology out of cloud telemetry.",
                    Tags = ["wef", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WEF health telemetry to Microsoft disabled; forwarding topology not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableForwardingHealthTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableForwardingHealthTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableForwardingHealthTelemetry", 1)],
                },
            ];
    }
}
