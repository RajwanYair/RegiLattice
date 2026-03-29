// RegiLattice.Core — Tweaks/WsdPrintDiscoveryPolicy.cs
// Web Services for Devices (WSD) print discovery, advertisement, and security policy — Sprint 475.
// Category: "WSD Print Discovery Policy" | Slug: wsdprt
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WSD

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WsdPrintDiscoveryPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WSD";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsdprt-disable-wsd-discovery",
                Label = "Disable WSD Printer Discovery",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Disables Web Services for Devices (WSD) printer discovery on the local network, preventing Windows from automatically detecting and adding WSD-compatible printers via SOAP-based device profile discovery.",
                Tags = ["wsd", "printing", "discovery", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSD printer discovery disabled; WSD-compatible printers must be added manually.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWSDDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWSDDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-disable-wsd-advertisement",
                Label = "Disable WSD Printer Advertisement from This Host",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Stops this Windows host from advertising locally-attached printers as WSD devices on the network, hiding accessible printers from other machines performing WSD discovery.",
                Tags = ["wsd", "printing", "advertisement", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSD advertisement stopped; this host's printers not visible via WSD to other network devices.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWSDAdvertisement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDAdvertisement")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWSDAdvertisement", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-require-auth-for-wsd-print",
                Label = "Require Authentication for WSD Print Access",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Requires user authentication before accepting WSD print operations from network clients, preventing unauthorised devices from submitting print jobs via WSD.",
                Tags = ["wsd", "printing", "authentication", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSD print requires auth; unauthenticated network print jobs via WSD rejected.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAuthForWSDPrint", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthForWSDPrint")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAuthForWSDPrint", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-block-wsd-on-public-network",
                Label = "Block WSD Printer Discovery on Public Networks",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Disables WSD printer discovery when the network location profile is set to Public, preventing printer discovery at coffeeshops, airports, or other untrusted networks.",
                Tags = ["wsd", "printing", "public-network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSD discovery disabled on public network profiles; printer discovery only active on Private/Domain profiles.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWSDOnPublicNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDOnPublicNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWSDOnPublicNetwork", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-limit-wsd-metadata-exposure",
                Label = "Limit WSD Device Metadata Exposure",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Restricts the metadata returned in WSD discovery responses, hiding detailed hardware model, firmware version, and network capability information that could aid reconnaissance.",
                Tags = ["wsd", "metadata", "privacy", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSD metadata limited; device model and firmware details not disclosed in discovery responses.",
                ApplyOps = [RegOp.SetDword(Key, "LimitWSDMetadataExposure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitWSDMetadataExposure")],
                DetectOps = [RegOp.CheckDword(Key, "LimitWSDMetadataExposure", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-block-wsd-eventing",
                Label = "Block WSD Eventing Subscriptions for Printers",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Disables WSD eventing subscriptions that allow remote clients to subscribe to printer status events (paper out, error, job complete) via WSD push notifications, reducing unsolicited outbound connections.",
                Tags = ["wsd", "printing", "eventing", "subscriptions", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "WSD printer event subscriptions blocked; remote clients cannot receive push status notifications.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWSDEventing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDEventing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWSDEventing", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-disable-wsd-scan",
                Label = "Disable WSD Scan (WSCN) Discovery",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Disables Windows Scan Communication Notifications (WSCN), preventing automatic discovery of WSD-compatible scanner devices over the network.",
                Tags = ["wsd", "scanner", "wscn", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSD scanner discovery disabled; network scanners must be added manually.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWSDScanDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDScanDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWSDScanDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-require-tls-for-wsd",
                Label = "Require TLS for WSD HTTPS Print Communication",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Forces WSD print data transmission to use HTTPS (SOAP over TLS), encrypting WSD messages and preventing plaintext interception of print content and printer control commands.",
                Tags = ["wsd", "tls", "https", "printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSD print traffic TLS-encrypted; unencrypted WSD HTTP connections rejected.",
                ApplyOps = [RegOp.SetDword(Key, "RequireTLSForWSD", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTLSForWSD")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTLSForWSD", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-block-wsd-cross-subnet",
                Label = "Block WSD Discovery Across Subnets",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Restricts WSD discovery to the local subnet only, preventing WSD multicast probes from being forwarded through routers and reaching printers in distant network segments.",
                Tags = ["wsd", "printing", "subnet", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSD discovery limited to local subnet; cross-router WSD discovery disabled.",
                ApplyOps = [RegOp.SetDword(Key, "BlockWSDCrossSubnet", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockWSDCrossSubnet")],
                DetectOps = [RegOp.CheckDword(Key, "BlockWSDCrossSubnet", 1)],
            },
            new TweakDef
            {
                Id = "wsdprt-audit-wsd-connections",
                Label = "Enable Audit Logging for WSD Printer Connections",
                Category = "WSD Print Discovery Policy",
                Description =
                    "Enables event log entries whenever a WSD printer is added, removed, or a print job is submitted via WSD, providing a discovery and usage trail for network printer monitoring.",
                Tags = ["wsd", "audit-log", "printing", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSD printer activity logged; connection and print events visible in event viewer.",
                ApplyOps = [RegOp.SetDword(Key, "AuditWSDPrinterConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditWSDPrinterConnections")],
                DetectOps = [RegOp.CheckDword(Key, "AuditWSDPrinterConnections", 1)],
            },
        ];
}
