// RegiLattice.Core — Tweaks/DataUsageMeteringPolicy.cs
// Data usage metering and cellular/bandwidth management GPO controls — Sprint 205.
// Controls Windows data consumption tracking, metered connections, and cost management.
// Category: "Data Usage Metering Policy" | Slug: datuse
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataUsage

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DataUsageMeteringPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataUsage";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "datuse-disable-background-data",
                Label = "Block Background Data on Metered Connections",
                Category = "Data Usage Metering Policy",
                Description =
                    "Prevents Windows apps from using background data when the network connection is marked as metered. Reduces unintended data consumption on mobile broadband or limited-data plans. Default: not enforced. Recommended: 1.",
                Tags = ["data-usage", "metered", "background", "cellular", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Background app refresh, live tile updates, and sync are suspended on metered connections.",
                ApplyOps = [RegOp.SetDword(Key, "BackgroundDataUsage", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "BackgroundDataUsage")],
                DetectOps = [RegOp.CheckDword(Key, "BackgroundDataUsage", 0)],
            },
            new TweakDef
            {
                Id = "datuse-disable-automatic-roaming-data",
                Label = "Block Automatic Data Use While Roaming",
                Category = "Data Usage Metering Policy",
                Description =
                    "Prevents Windows from automatically sending or receiving data while the device is roaming on a cellular network. Eliminates surprise roaming charges. Default: not restricted. Recommended: 1.",
                Tags = ["data-usage", "roaming", "cellular", "cost", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Data is fully blocked when roaming; users must manually enable roaming data if needed.",
                ApplyOps = [RegOp.SetDword(Key, "AllowRoamingData", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowRoamingData")],
                DetectOps = [RegOp.CheckDword(Key, "AllowRoamingData", 0)],
            },
            new TweakDef
            {
                Id = "datuse-enforce-data-limit-warning",
                Label = "Enforce Data Usage Warning at 80% Limit",
                Category = "Data Usage Metering Policy",
                Description =
                    "Triggers a system notification when the device has consumed 80% of the configured data usage limit. Early warning helps users avoid plan overages. Default: not configured. Recommended: 80.",
                Tags = ["data-usage", "warning", "limit", "cellular", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "A notification appears when 80% of the data plan is consumed.",
                ApplyOps = [RegOp.SetDword(Key, "DataLimitWarningPercent", 80)],
                RemoveOps = [RegOp.DeleteValue(Key, "DataLimitWarningPercent")],
                DetectOps = [RegOp.CheckDword(Key, "DataLimitWarningPercent", 80)],
            },
            new TweakDef
            {
                Id = "datuse-disable-store-metered-updates",
                Label = "Block Microsoft Store Updates Over Metered Connections",
                Category = "Data Usage Metering Policy",
                Description =
                    "Prevents the Microsoft Store from downloading app updates automatically when connected via a metered network. Avoids large background downloads on limited plans. Default: not restricted. Recommended: 1.",
                Tags = ["data-usage", "store", "metered", "updates", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Microsoft Store auto-updates suspended on metered connections; updates occur only on unmetered Wi-Fi.",
                ApplyOps = [RegOp.SetDword(Key, "BlockStoreUpdatesOnMeteredConn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockStoreUpdatesOnMeteredConn")],
                DetectOps = [RegOp.CheckDword(Key, "BlockStoreUpdatesOnMeteredConn", 1)],
            },
            new TweakDef
            {
                Id = "datuse-disable-usage-telemetry-upload",
                Label = "Block Data Usage Telemetry Upload",
                Category = "Data Usage Metering Policy",
                Description =
                    "Stops Windows from uploading data usage statistics (per-app bandwidth consumption) to Microsoft telemetry services. Prevents sending usage patterns off-device. Default: upload enabled. Recommended: 1.",
                Tags = ["data-usage", "telemetry", "privacy", "upload", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Per-app data usage telemetry is not uploaded to Microsoft cloud services.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUsageTelemetryUpload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUsageTelemetryUpload")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUsageTelemetryUpload", 1)],
            },
            new TweakDef
            {
                Id = "datuse-set-default-metered",
                Label = "Mark New Wi-Fi Connections as Metered by Default",
                Category = "Data Usage Metering Policy",
                Description =
                    "Sets all new Wi-Fi connections to metered by default, automatically activating bandwidth-saving restrictions. Useful for laptop fleets that frequently connect to mobile hotspots. Default: not metered. Recommended: when roaming is common.",
                Tags = ["data-usage", "metered", "wifi", "default", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "All new Wi-Fi adapters default to metered; users can manually mark specific networks as unmetered.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultToMeteredConnection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultToMeteredConnection")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultToMeteredConnection", 1)],
            },
            new TweakDef
            {
                Id = "datuse-disable-cost-based-app-limits",
                Label = "Disable Cost-Based App Background Limits",
                Category = "Data Usage Metering Policy",
                Description =
                    "Disables the automatic cost-awareness throttling that restricts background-capable apps based on connection cost (e.g., fixed vs. variable plan). Allows apps to run unrestricted on all connections. Default: cost-aware throttling active. Recommended: 0 on unlimited plans.",
                Tags = ["data-usage", "cost", "background", "apps", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Apps no longer throttle themselves based on connection cost tier; background activity unrestricted.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCostBasedThrottling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCostBasedThrottling")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCostBasedThrottling", 1)],
            },
            new TweakDef
            {
                Id = "datuse-block-wifisense-hotspot-sharing",
                Label = "Block Wi-Fi Sense Hotspot Data Sharing",
                Category = "Data Usage Metering Policy",
                Description =
                    "Prevents Wi-Fi Sense from sharing mobile hotspot connection credentials with contacts and social networks. Stops unintended bandwidth sharing over a data plan. Default: not restricted. Recommended: 1.",
                Tags = ["data-usage", "wifi-sense", "hotspot", "sharing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Wi-Fi Sense hotspot credential sharing is disabled; mobile data is not shared with contacts.",
                ApplyOps = [RegOp.SetDword(Key, "BlockHotspotSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockHotspotSharing")],
                DetectOps = [RegOp.CheckDword(Key, "BlockHotspotSharing", 1)],
            },
            new TweakDef
            {
                Id = "datuse-monthly-data-limit-mb",
                Label = "Set Monthly Cellular Data Limit (5120 MB)",
                Category = "Data Usage Metering Policy",
                Description =
                    "Configures the monthly cellular data budget to 5120 MB (5 GB). Windows tracks usage against this limit and triggers warnings and restrictions when approaching/exceeding it. Default: not set. Recommended: set per plan size.",
                Tags = ["data-usage", "limit", "cellular", "budget", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Monthly data budget is 5 GB; Windows shows remaining budget and restricts usage near the limit.",
                ApplyOps = [RegOp.SetDword(Key, "MonthlyDataLimitMB", 5120)],
                RemoveOps = [RegOp.DeleteValue(Key, "MonthlyDataLimitMB")],
                DetectOps = [RegOp.CheckDword(Key, "MonthlyDataLimitMB", 5120)],
            },
            new TweakDef
            {
                Id = "datuse-reset-limit-on-cycle",
                Label = "Auto-Reset Data Counter on Billing Cycle",
                Category = "Data Usage Metering Policy",
                Description =
                    "Enables automatic reset of the data usage counter at the beginning of each billing cycle (configured per adapter). Ensures the usage counter aligns with the carrier billing period. Default: not configured. Recommended: 1.",
                Tags = ["data-usage", "reset", "billing-cycle", "cellular", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Data usage counter resets automatically each billing cycle; consistent with carrier accounting.",
                ApplyOps = [RegOp.SetDword(Key, "AutoResetOnBillingCycle", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoResetOnBillingCycle")],
                DetectOps = [RegOp.CheckDword(Key, "AutoResetOnBillingCycle", 1)],
            },
        ];
}
