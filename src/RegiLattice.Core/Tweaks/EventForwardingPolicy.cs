// RegiLattice.Core — Tweaks/EventForwardingPolicy.cs
// Windows Event Forwarding (WEF) machine-scope GPO controls — Sprint 200.
// Governs event collector subscriptions, delivery modes, channel encryption, and retention.
// Category: "Event Forwarding Policy" | Slug: evtfwd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventForwarding

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EventForwardingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventForwarding";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "evtfwd-enable-subscription-manager",
                Label = "Enable WEF Subscription Manager",
                Category = "Event Forwarding Policy",
                Description =
                    "Activates the Windows Event Forwarding subscription manager, allowing this source computer to forward events to a configured collector. Required for WEF operation. Default: 0. Recommended: 1 when WEF is deployed.",
                Tags = ["wef", "event-forwarding", "subscription", "siem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables centralized event forwarding to a SIEM or log collector.",
                ApplyOps = [RegOp.SetDword(Key, "SubscriptionManagerEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SubscriptionManagerEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SubscriptionManagerEnabled", 1)],
            },
            new TweakDef
            {
                Id = "evtfwd-require-encryption",
                Label = "Require Encrypted Event Forwarding Channel",
                Category = "Event Forwarding Policy",
                Description =
                    "Prevents event forwarding over unencrypted channels. All WEF traffic must use HTTPS or Kerberos-authenticated transport. Default: not enforced. Recommended: 1 for any production WEF deployment.",
                Tags = ["wef", "event-forwarding", "encryption", "https", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "No event data leaves the host in plaintext; WEF over HTTP is rejected.",
                ApplyOps = [RegOp.SetDword(Key, "AllowUnencryptedForwarding", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUnencryptedForwarding")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUnencryptedForwarding", 0)],
            },
            new TweakDef
            {
                Id = "evtfwd-require-kerberos-auth",
                Label = "Require Kerberos Authentication for WEF",
                Category = "Event Forwarding Policy",
                Description =
                    "Enforces Kerberos mutual authentication for all Windows Event Forwarding connections. Prevents relaying to an untrusted or spoofed collector endpoint. Default: 0. Recommended: 1 in domain environments.",
                Tags = ["wef", "event-forwarding", "kerberos", "authentication", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only domain-authenticated collectors accepted; prevents event data exfiltration via rogue collectors.",
                ApplyOps = [RegOp.SetDword(Key, "RequireKerberosAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireKerberosAuthentication")],
                DetectOps = [RegOp.CheckDword(Key, "RequireKerberosAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "evtfwd-limit-max-forward-rate",
                Label = "Limit Maximum Event Forwarding Rate",
                Category = "Event Forwarding Policy",
                Description =
                    "Caps the maximum rate at which events are forwarded to the collector at 1000 events per second. Prevents event flooding from overwhelming the collector during high-activity periods. Default: unlimited. Recommended: 1000.",
                Tags = ["wef", "event-forwarding", "rate-limit", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "High-activity hosts may drop events above the cap; increase limit on noisy source computers.",
                ApplyOps = [RegOp.SetDword(Key, "MaxForwardingRate", 1000)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxForwardingRate")],
                DetectOps = [RegOp.CheckDword(Key, "MaxForwardingRate", 1000)],
            },
            new TweakDef
            {
                Id = "evtfwd-set-retry-interval",
                Label = "Set WEF Connection Retry Interval",
                Category = "Event Forwarding Policy",
                Description =
                    "Configures the interval (in seconds) between connection retry attempts when the WEF collector is unreachable. Lower values detect recovery faster; higher values reduce network noise. Default: 300. Recommended: 60.",
                Tags = ["wef", "event-forwarding", "retry", "availability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Events may be delayed up to one retry interval duration if the collector is temporarily unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "RetryInterval", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "RetryInterval")],
                DetectOps = [RegOp.CheckDword(Key, "RetryInterval", 60)],
            },
            new TweakDef
            {
                Id = "evtfwd-set-heartbeat-interval",
                Label = "Set WEF Collector Heartbeat Interval",
                Category = "Event Forwarding Policy",
                Description =
                    "Sets the heartbeat keep-alive interval (seconds) for WEF collector connections. Ensures the subscription stays active and the collector knows the source is alive. Default: not set. Recommended: 3600 (1 hour).",
                Tags = ["wef", "event-forwarding", "heartbeat", "keepalive", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Inactive WEF subscriptions persist; collector receives periodic health signals from source.",
                ApplyOps = [RegOp.SetDword(Key, "HeartbeatInterval", 3600)],
                RemoveOps = [RegOp.DeleteValue(Key, "HeartbeatInterval")],
                DetectOps = [RegOp.CheckDword(Key, "HeartbeatInterval", 3600)],
            },
            new TweakDef
            {
                Id = "evtfwd-set-connection-timeout",
                Label = "Set WEF Connection Timeout",
                Category = "Event Forwarding Policy",
                Description =
                    "Sets the connection timeout (in seconds) for WEF collector connections. After this period without a response, the connection is dropped and retried. Default: 30. Recommended: 60.",
                Tags = ["wef", "event-forwarding", "timeout", "connection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Slow collector responses up to 60 seconds are tolerated before reconnection.",
                ApplyOps = [RegOp.SetDword(Key, "ConnectionTimeout", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConnectionTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "ConnectionTimeout", 60)],
            },
            new TweakDef
            {
                Id = "evtfwd-limit-max-queue-size",
                Label = "Limit WEF Local Event Queue Size",
                Category = "Event Forwarding Policy",
                Description =
                    "Caps the local event queue (held while the collector is unreachable) to 1024 MB. Prevents unbounded disk growth during extended collector outages. Default: unlimited. Recommended: 1024.",
                Tags = ["wef", "event-forwarding", "queue", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Events beyond the queue limit are dropped; increase limit on systems with strict audit requirements.",
                ApplyOps = [RegOp.SetDword(Key, "MaxQueueSizeMB", 1024)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxQueueSizeMB")],
                DetectOps = [RegOp.CheckDword(Key, "MaxQueueSizeMB", 1024)],
            },
            new TweakDef
            {
                Id = "evtfwd-use-minimize-bandwidth",
                Label = "Use Bandwidth-Minimising WEF Delivery Mode",
                Category = "Event Forwarding Policy",
                Description =
                    "Switches WEF delivery optimisation to minimise bandwidth consumption (batch mode). Events are grouped and sent less frequently but more efficiently. Default: 0 (normal). Recommended: 1 on constrained WAN links.",
                Tags = ["wef", "event-forwarding", "bandwidth", "delivery", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Event delivery may be delayed; latency vs bandwidth trade-off. Not suitable for real-time detection.",
                ApplyOps = [RegOp.SetDword(Key, "DeliveryOptimizationMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeliveryOptimizationMode")],
                DetectOps = [RegOp.CheckDword(Key, "DeliveryOptimizationMode", 1)],
            },
            new TweakDef
            {
                Id = "evtfwd-enable-event-consolidation",
                Label = "Enable WEF Event Consolidation at Source",
                Category = "Event Forwarding Policy",
                Description =
                    "Enables duplicate event consolidation on the source computer before forwarding. Repeated identical events within the batch window are sent once with a count. Reduces collector load. Default: 0. Recommended: 1.",
                Tags = ["wef", "event-forwarding", "consolidation", "deduplication", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Repeated events are collapsed; collector sees one entry with event count rather than flood of identical events.",
                ApplyOps = [RegOp.SetDword(Key, "EnableEventConsolidation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEventConsolidation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEventConsolidation", 1)],
            },
        ];
}
