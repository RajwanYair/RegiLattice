// RegiLattice.Core — Tweaks/EventSubscriptionPolicy.cs
// Windows Event Collector subscription management and XPath query policy — Sprint 494.
// Category: "Event Subscription Policy" | Slug: wecpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventCollector

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EventSubscriptionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventCollector";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wecpol-enable-event-collector-service",
                Label = "Enable Windows Event Collector Service",
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
                Category = "Event Subscription Policy",
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
