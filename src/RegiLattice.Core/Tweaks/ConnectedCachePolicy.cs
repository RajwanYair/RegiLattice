// RegiLattice.Core — Tweaks/ConnectedCachePolicy.cs
// Microsoft Connected Cache (MCC) enterprise delivery policy controls — Sprint 371.
// Category: "Microsoft Connected Cache Policy" | Slug: mcc
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\ConnectedCache
// MinBuild: 19041 (Windows 10 2004+ / Windows 11 21H2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ConnectedCachePolicy
{
    private const string MccKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ConnectedCache";
    private const string MccClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ConnectedCache\Client";
    private const string DeliveryOptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "mcc-disable-connected-cache-client",
                Label = "Disable Microsoft Connected Cache Client",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Prevents Windows from acting as a client to Microsoft Connected Cache (MCC) servers. "
                    + "The device will not retrieve Windows Update, Delivery Optimization, or Microsoft 365 content from local MCC nodes.",
                Tags = ["mcc", "connected-cache", "delivery-optimization", "bandwidth", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Devices will download update content directly from Microsoft CDN instead of from an on-premises MCC node; "
                    + "may increase internet bandwidth consumption.",
                RegistryKeys = [MccKey],
                ApplyOps = [RegOp.SetDword(MccKey, "DisableMicrosoftConnectedCache", 1)],
                RemoveOps = [RegOp.DeleteValue(MccKey, "DisableMicrosoftConnectedCache")],
                DetectOps = [RegOp.CheckDword(MccKey, "DisableMicrosoftConnectedCache", 1)],
            },
            new TweakDef
            {
                Id = "mcc-restrict-to-enterprise-nodes",
                Label = "Restrict Connected Cache to Enterprise MCC Nodes Only",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Configures Windows to only retrieve cached content from administrator-specified "
                    + "Microsoft Connected Cache nodes, preventing download from unapproved or public MCC servers.",
                Tags = ["mcc", "connected-cache", "enterprise", "node-restriction", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Ensures update traffic goes through the organisation's approved cache infrastructure, "
                    + "supporting network segmentation and compliance.",
                RegistryKeys = [MccKey],
                ApplyOps = [RegOp.SetDword(MccKey, "RestrictToEnterpriseMCCNodes", 1)],
                RemoveOps = [RegOp.DeleteValue(MccKey, "RestrictToEnterpriseMCCNodes")],
                DetectOps = [RegOp.CheckDword(MccKey, "RestrictToEnterpriseMCCNodes", 1)],
            },
            new TweakDef
            {
                Id = "mcc-set-cache-node-hostname",
                Label = "Set Connected Cache Node Hostname (Configures MCC Server)",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Specifies the FQDN or IP address of the Microsoft Connected Cache node that this device should use "
                    + "as its primary cache source for Windows Update and Delivery Optimization content.",
                Tags = ["mcc", "connected-cache", "hostname", "enterprise", "configuration"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Directs update traffic to the organisation's on-premises MCC node, reducing internet egress for update downloads.",
                RegistryKeys = [MccClientKey],
                ApplyOps = [RegOp.SetString(MccClientKey, "MCCNodeHostname", "mcc-server.contoso.com")],
                RemoveOps = [RegOp.DeleteValue(MccClientKey, "MCCNodeHostname")],
                DetectOps = [RegOp.CheckString(MccClientKey, "MCCNodeHostname", "mcc-server.contoso.com")],
            },
            new TweakDef
            {
                Id = "mcc-block-p2p-delivery-optimization",
                Label = "Block Peer-to-Peer Delivery Optimization",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Sets the Delivery Optimization download mode to HTTP-only (mode 0), preventing Windows from using "
                    + "peer-to-peer transfers between devices on the local network or internet. "
                    + "Content is sourced from Microsoft CDN or MCC only.",
                Tags = ["mcc", "delivery-optimization", "p2p", "bandwidth", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Prevents devices from sharing update packages with each other; "
                    + "reduces lateral data movement and ensures organisational control over update source.",
                RegistryKeys = [DeliveryOptKey],
                ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DODownloadMode", 0)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DODownloadMode", 0)],
            },
            new TweakDef
            {
                Id = "mcc-limit-background-bandwidth-percent",
                Label = "Limit Delivery Optimization Background Bandwidth (50%)",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Caps background Delivery Optimization download transfers at 50% of the measured internet bandwidth, "
                    + "preventing bulk update downloads from saturating the connection during working hours.",
                Tags = ["mcc", "delivery-optimization", "bandwidth", "throttle", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Keeps 50% of bandwidth available for interactive use while background updates download. "
                    + "Adjust the value to match your SLA.",
                RegistryKeys = [DeliveryOptKey],
                ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DOPercentageMaxBackgroundBandwidth", 50)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DOPercentageMaxBackgroundBandwidth")],
                DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DOPercentageMaxBackgroundBandwidth", 50)],
            },
            new TweakDef
            {
                Id = "mcc-limit-foreground-bandwidth-percent",
                Label = "Limit Delivery Optimization Foreground Bandwidth (80%)",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Caps foreground Delivery Optimization download transfers at 80% of the measured bandwidth, "
                    + "allowing user-initiated downloads like Microsoft Store apps or Windows Updates to proceed quickly "
                    + "without total saturation.",
                Tags = ["mcc", "delivery-optimization", "bandwidth", "throttle", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Provides a headroom buffer for concurrent network use during explicit update downloads triggered by the user.",
                RegistryKeys = [DeliveryOptKey],
                ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DOPercentageMaxForegroundBandwidth", 80)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DOPercentageMaxForegroundBandwidth")],
                DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DOPercentageMaxForegroundBandwidth", 80)],
            },
            new TweakDef
            {
                Id = "mcc-set-cache-drive-size-gb",
                Label = "Set Connected Cache Storage Limit (20 GB)",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Sets the maximum disk space available to the Microsoft Connected Cache client for storing downloaded content packages. "
                    + "Prevents the cache from consuming unpredictable amounts of the system drive.",
                Tags = ["mcc", "connected-cache", "cache-size", "disk", "configuration"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Bounds cache growth to 20 GB; adjust based on available disk space and update volume in your environment.",
                RegistryKeys = [MccClientKey],
                ApplyOps = [RegOp.SetDword(MccClientKey, "MaxCacheSizeGB", 20)],
                RemoveOps = [RegOp.DeleteValue(MccClientKey, "MaxCacheSizeGB")],
                DetectOps = [RegOp.CheckDword(MccClientKey, "MaxCacheSizeGB", 20)],
            },
            new TweakDef
            {
                Id = "mcc-disable-upload-to-peers",
                Label = "Disable Upload of Cached Content to Peers",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Prevents the device from uploading locally cached Delivery Optimization content to other clients on the LAN or internet, "
                    + "configuring the node as a download-only client.",
                Tags = ["mcc", "delivery-optimization", "upload", "p2p", "bandwidth"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Ensures devices only consume bandwidth for their own downloads, not to serve other clients. "
                    + "Useful for metered-connection endpoints.",
                RegistryKeys = [DeliveryOptKey],
                ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DOMaxUploadBandwidth", 0)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DOMaxUploadBandwidth")],
                DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DOMaxUploadBandwidth", 0)],
            },
            new TweakDef
            {
                Id = "mcc-restrict-to-lan-peers-only",
                Label = "Restrict Delivery Optimization Peers to LAN Only",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Configures Delivery Optimization to allow peer-to-peer transfers only within the local area network (LAN), not over the internet. "
                    + "Prevents content sharing with devices outside the organisation's network boundary.",
                Tags = ["mcc", "delivery-optimization", "lan", "p2p", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits peer-to-peer transfers to LAN mode (mode 1) — devices on the same subnet; no internet peering occurs.",
                RegistryKeys = [DeliveryOptKey],
                ApplyOps = [RegOp.SetDword(DeliveryOptKey, "DODownloadMode", 1)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOptKey, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DeliveryOptKey, "DODownloadMode", 1)],
            },
            new TweakDef
            {
                Id = "mcc-disable-cache-on-metered-connection",
                Label = "Disable MCC Downloads on Metered Connections",
                Category = "Microsoft Connected Cache Policy",
                Description =
                    "Prevents Delivery Optimization and Microsoft Connected Cache downloads from occurring when the network connection "
                    + "is detected as metered (e.g., mobile data). Downloads resume automatically on unmetered connections.",
                Tags = ["mcc", "delivery-optimization", "metered", "data-saver", "mobile"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 19041,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Prevents unexpected data charges on mobile hotspots, cellular-connected laptops, "
                    + "or any connection marked metered in Windows.",
                RegistryKeys = [MccClientKey],
                ApplyOps = [RegOp.SetDword(MccClientKey, "DisableOnMeteredConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(MccClientKey, "DisableOnMeteredConnections")],
                DetectOps = [RegOp.CheckDword(MccClientKey, "DisableOnMeteredConnections", 1)],
            },
        ];
}
