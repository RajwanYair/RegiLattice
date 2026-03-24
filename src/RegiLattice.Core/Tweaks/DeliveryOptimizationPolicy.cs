#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class DeliveryOptimizationPolicy
{
    private const string Do = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "doptpol-download-mode-local",
            Label = "Restrict DO Download Mode to Local PC Only",
            Category = "Delivery Optimization Policy",
            Description =
                "Sets Delivery Optimization download mode to 0 (HTTP only, no peering). Prevents Windows Update content from being shared with or downloaded from other PCs on the network or internet. Default: 1 (LAN). Recommended: 0 for enterprise/privacy.",
            Tags = ["delivery-optimization", "windows-update", "network", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue(Do, "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(Do, "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "doptpol-min-background-qos",
            Label = "Set DO Minimum Background Download QoS",
            Category = "Delivery Optimization Policy",
            Description =
                "Sets the minimum download speed (kbps) for Delivery Optimization background downloads to 500 kbps. Prevents DO from saturating network during background updates. Default: no limit. Recommended: 500.",
            Tags = ["delivery-optimization", "bandwidth", "background", "qos"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMinBackgroundQos", 500)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMinBackgroundQos")],
            DetectOps = [RegOp.CheckDword(Do, "DOMinBackgroundQos", 500)],
        },
        new TweakDef
        {
            Id = "doptpol-max-upload-bandwidth",
            Label = "Limit DO Upload Bandwidth to 10%",
            Category = "Delivery Optimization Policy",
            Description =
                "Caps Delivery Optimization upload bandwidth to 10% of available bandwidth. Prevents DO peering from consuming upstream bandwidth on metered or shared connections. Default: 0 (no limit). Recommended: 10.",
            Tags = ["delivery-optimization", "upload", "bandwidth", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMaxUploadBandwidth", 10)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMaxUploadBandwidth")],
            DetectOps = [RegOp.CheckDword(Do, "DOMaxUploadBandwidth", 10)],
        },
        new TweakDef
        {
            Id = "doptpol-max-cache-size",
            Label = "Limit DO Cache to 5% of Disk",
            Category = "Delivery Optimization Policy",
            Description =
                "Limits the Delivery Optimization disk cache to 5% of the drive. Prevents DO from consuming excessive disk space on smaller drives. Default: 20%. Recommended: 5.",
            Tags = ["delivery-optimization", "disk", "cache"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMaxCacheSize", 5)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMaxCacheSize")],
            DetectOps = [RegOp.CheckDword(Do, "DOMaxCacheSize", 5)],
        },
        new TweakDef
        {
            Id = "doptpol-absolute-max-cache-size",
            Label = "Cap DO Absolute Cache Size to 1 GB",
            Category = "Delivery Optimization Policy",
            Description =
                "Sets an absolute 1 024 MB cap on the Delivery Optimization cache regardless of disk size. Prevents DO from accumulating large caches on high-capacity drives. Default: 10 240 MB. Recommended: 1024.",
            Tags = ["delivery-optimization", "disk", "cache", "storage"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOAbsoluteMaxCacheSize", 1024)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOAbsoluteMaxCacheSize")],
            DetectOps = [RegOp.CheckDword(Do, "DOAbsoluteMaxCacheSize", 1024)],
        },
        new TweakDef
        {
            Id = "doptpol-min-disk-size-allowed",
            Label = "Require 32 GB Disk for DO Caching",
            Category = "Delivery Optimization Policy",
            Description =
                "Prevents Delivery Optimization caching on drives smaller than 32 GB. Protects limited-storage devices from DO disk pressure. Default: no minimum. Recommended: 32768 MB.",
            Tags = ["delivery-optimization", "disk", "cache", "storage"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMinDiskSizeAllowedToCaches", 32768)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMinDiskSizeAllowedToCaches")],
            DetectOps = [RegOp.CheckDword(Do, "DOMinDiskSizeAllowedToCaches", 32768)],
        },
        new TweakDef
        {
            Id = "doptpol-min-ram-allowed",
            Label = "Require 4 GB RAM for DO Peering",
            Category = "Delivery Optimization Policy",
            Description =
                "Prevents Delivery Optimization peer-to-peer upload on devices with less than 4 GB RAM. Avoids resource contention on low-memory devices. Default: 4 GB. Recommended: 4096 MB.",
            Tags = ["delivery-optimization", "memory", "peering", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMinRAMAllowedToPeer", 4096)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMinRAMAllowedToPeer")],
            DetectOps = [RegOp.CheckDword(Do, "DOMinRAMAllowedToPeer", 4096)],
        },
        new TweakDef
        {
            Id = "doptpol-min-file-size",
            Label = "Set DO Minimum File Size for Peering to 100 MB",
            Category = "Delivery Optimization Policy",
            Description =
                "Only enables DO peering for files ≥ 100 MB. Reduces peer overhead for small updates that are fast to download directly. Default: 100 MB. Recommended: 102400 kB.",
            Tags = ["delivery-optimization", "peering", "file-size"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMinFileSizeToCache", 102400)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMinFileSizeToCache")],
            DetectOps = [RegOp.CheckDword(Do, "DOMinFileSizeToCache", 102400)],
        },
        new TweakDef
        {
            Id = "doptpol-max-cache-age",
            Label = "Set DO Cache Expiry to 3 Days",
            Category = "Delivery Optimization Policy",
            Description =
                "Sets the maximum age of cached Delivery Optimization content to 259 200 seconds (3 days). Reclaims disk space from stale cached updates faster. Default: 259 200 seconds. Recommended: 259200.",
            Tags = ["delivery-optimization", "cache", "expiry", "disk"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOMaxCacheAge", 259200)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOMaxCacheAge")],
            DetectOps = [RegOp.CheckDword(Do, "DOMaxCacheAge", 259200)],
        },
        new TweakDef
        {
            Id = "doptpol-set-hours-limit-background",
            Label = "Limit DO Background Downloads to Off-Hours",
            Category = "Delivery Optimization Policy",
            Description =
                "Restricts Delivery Optimization background download activity to off-peak hours (22:00–06:00). Reduces DO network impact during business/active hours. Default: 0 (not set). Recommended: 1 (enabled).",
            Tags = ["delivery-optimization", "background", "schedule", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Do],
            ApplyOps = [RegOp.SetDword(Do, "DOSetHoursToLimitBackgroundDownloadBandwidth", 1)],
            RemoveOps = [RegOp.DeleteValue(Do, "DOSetHoursToLimitBackgroundDownloadBandwidth")],
            DetectOps = [RegOp.CheckDword(Do, "DOSetHoursToLimitBackgroundDownloadBandwidth", 1)],
        },
    ];
}
