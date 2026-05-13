namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyDeliveryOpt
{
    private const string DoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "delopt-http-only-mode",
                Label = "Delivery Optimization: HTTP-Only Download Mode",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Sets the Delivery Optimization download mode to HTTP-only (DODownloadMode=0). "
                    + "Disables all peer-to-peer content sharing, both within the local network and across the internet. "
                    + "All Windows Update, Microsoft Store, and app update traffic flows directly via HTTP. "
                    + "Recommended in environments with strict egress filtering or bandwidth auditing requirements.",
                Tags = ["delivery-optimization", "p2p", "windows-update", "bandwidth", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables P2P sharing; downloads may be slower on slow links but all traffic is direct.",
                ApplyOps = [RegOp.SetDword(DoKey, "DODownloadMode", 0)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DoKey, "DODownloadMode", 0)],
            },
            new TweakDef
            {
                Id = "delopt-lan-only-mode",
                Label = "Delivery Optimization: LAN Peers Only",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Restricts Delivery Optimization peer sharing to devices on the same local network (DODownloadMode=1). "
                    + "Peers are only found within the same subnet. "
                    + "Prevents content sharing across internet or WAN links while still benefiting from LAN caching.",
                Tags = ["delivery-optimization", "lan", "peer-caching", "windows-update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Allows LAN-local P2P caching but blocks internet-facing peer discovery.",
                ApplyOps = [RegOp.SetDword(DoKey, "DODownloadMode", 1)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DoKey, "DODownloadMode", 1)],
            },
            new TweakDef
            {
                Id = "delopt-limit-background-bandwidth",
                Label = "Delivery Optimization: Limit Background Download Bandwidth",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Limits Delivery Optimization background download bandwidth to 20% of total available bandwidth. "
                    + "Background downloads (outside business hours) are throttled, reducing interference with interactive traffic. "
                    + "Value is a percentage of the total available bandwidth (0 = unlimited).",
                Tags = ["delivery-optimization", "bandwidth", "throttle", "background-download"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cap on background update download bandwidth; foreground interactive traffic is unaffected.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOMaxBackgroundDownloadBandwidth", 20)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOMaxBackgroundDownloadBandwidth")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOMaxBackgroundDownloadBandwidth", 20)],
            },
            new TweakDef
            {
                Id = "delopt-limit-foreground-bandwidth",
                Label = "Delivery Optimization: Limit Foreground Download Bandwidth",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Caps foreground Delivery Optimization downloads (manual user-initiated updates) at 70% of bandwidth. "
                    + "Leaves headroom for interactive traffic while allowing updates to proceed reasonably quickly. "
                    + "Value is a percentage (0 = unlimited).",
                Tags = ["delivery-optimization", "bandwidth", "throttle", "foreground-download"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Foreground update downloads capped at 70%; other user traffic retains the remaining 30%.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOMaxForegroundDownloadBandwidth", 70)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOMaxForegroundDownloadBandwidth")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOMaxForegroundDownloadBandwidth", 70)],
            },
            new TweakDef
            {
                Id = "delopt-disable-upload",
                Label = "Delivery Optimization: Disable Upload to Internet Peers",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Sets the maximum Delivery Optimization upload rate to internet peers to 0 KB/s (off). "
                    + "Prevents this machine from sharing cached updates with other devices over the internet. "
                    + "Useful for metered connections or environments where outbound bandwidth is constrained.",
                Tags = ["delivery-optimization", "upload", "bandwidth", "metered"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "No update content is uploaded to internet P2P peers; download remains unaffected.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOMaxUploadBandwidth", 0)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOMaxUploadBandwidth")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOMaxUploadBandwidth", 0)],
            },
            new TweakDef
            {
                Id = "delopt-monthly-upload-cap",
                Label = "Delivery Optimization: Monthly Upload Data Cap (5 GB)",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Limits the total amount of data uploaded to Delivery Optimization peers per calendar month to 5 GB. "
                    + "After the cap is reached the device stops seeding until the next month. "
                    + "Protects metered or capped internet plans from silent data overuse by Windows Update.",
                Tags = ["delivery-optimization", "data-cap", "upload", "metered"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Caps monthly P2P upload at 5 GB; prevents unexpected data charges on capped plans.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOMonthlyUploadDataCap", 5)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOMonthlyUploadDataCap")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOMonthlyUploadDataCap", 5)],
            },
            new TweakDef
            {
                Id = "delopt-minimum-ram-requirement",
                Label = "Delivery Optimization: Require At Least 4 GB RAM",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Sets the minimum RAM threshold to 4 GB for Delivery Optimization peer caching participation. "
                    + "Devices with less RAM will not act as peers and will only download content via HTTP. "
                    + "Prevents memory pressure on low-spec machines from the DO caching service.",
                Tags = ["delivery-optimization", "ram", "peer-caching", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Devices below 4 GB RAM skip peer participation; reduces memory overhead on older hardware.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOMinRAMAllowed", 4096)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOMinRAMAllowed")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOMinRAMAllowed", 4096)],
            },
            new TweakDef
            {
                Id = "delopt-minimum-file-size",
                Label = "Delivery Optimization: Minimum File Size 100 MB for P2P",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Sets the minimum file size eligible for Delivery Optimization peer caching to 100 MB. "
                    + "Small files (patches, stub installers) are downloaded directly; only large update payloads use P2P. "
                    + "Reduces overhead and churn from cache-warming small files across the peer network.",
                Tags = ["delivery-optimization", "min-file-size", "peer-caching", "efficiency"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Only files ≥100 MB use P2P; small files always go direct, reducing unnecessary cache overhead.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOMinFileSizeToCache", 100)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOMinFileSizeToCache")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOMinFileSizeToCache", 100)],
            },
            new TweakDef
            {
                Id = "delopt-restrict-peer-subnet",
                Label = "Delivery Optimization: Restrict Peer Selection to Same Subnet",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Restricts Delivery Optimization peer selection to devices on the same IP subnet. "
                    + "Prevents peers from being discovered and contacted across routed VLAN or site boundaries. "
                    + "Ensures lateral data movement stays within the local broadcast domain.",
                Tags = ["delivery-optimization", "subnet", "peer-restriction", "vlan"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Peer discovery limited to local subnet; multi-site environments benefit from reduced cross-site traffic.",
                ApplyOps = [RegOp.SetDword(DoKey, "DORestrictPeerSelectionBy", 1)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DORestrictPeerSelectionBy")],
                DetectOps = [RegOp.CheckDword(DoKey, "DORestrictPeerSelectionBy", 1)],
            },
            new TweakDef
            {
                Id = "delopt-disable-vpn-peer-caching",
                Label = "Delivery Optimization: Disable Peer Caching over VPN",
                Category = "Network Optimization — Delivery Optimization",
                Description =
                    "Disables Delivery Optimization peer caching when the device is connected via VPN. "
                    + "Prevents Windows Update P2P data from traversing the VPN tunnel, reducing VPN load and "
                    + "avoiding potential leakage of cached update content through secure tunnels.",
                Tags = ["delivery-optimization", "vpn", "peer-caching", "network-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "On VPN connections, updates go direct rather than through P2P peers; reduces VPN tunnel load.",
                ApplyOps = [RegOp.SetDword(DoKey, "DOAllowVPNPeerCaching", 0)],
                RemoveOps = [RegOp.DeleteValue(DoKey, "DOAllowVPNPeerCaching")],
                DetectOps = [RegOp.CheckDword(DoKey, "DOAllowVPNPeerCaching", 0)],
            },
        ];
}
