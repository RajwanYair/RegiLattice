// RegiLattice.Core — Tweaks/WefSubscriptionPolicy.cs
// Windows Event Forwarding (WEF) advanced subscription, transport, and auth controls — Sprint 492.
// Category: "WEF Subscription Policy" | Slug: wefsubpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WefSubscriptionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wefsubpol-enable-event-forwarding",
                Label = "Enable Windows Event Forwarding Subscription Service",
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
                Category = "WEF Subscription Policy",
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
