// RegiLattice.Core — Tweaks/StorageHealthPolicy.cs
// Storage health monitoring, S.M.A.R.T., and disk maintenance GPO controls — Sprint 209.
// Controls storage health reporting, predictive failure alerting, and disk maintenance tasks.
// Category: "Storage Health Policy" | Slug: strhlt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageHealth

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageHealthPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageHealth";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "strhlt-enable-smart-monitoring",
                Label = "Enable S.M.A.R.T. Drive Health Monitoring",
                Category = "Storage Health Policy",
                Description =
                    "Enables Windows Storage Health monitoring of S.M.A.R.T. attributes on physical drives. Predictive failure alerts are surfaced in Action Center before a drive fails. Default: enabled on consumer; verify on server. Recommended: 1.",
                Tags = ["storage", "smart", "health", "monitoring", "predictive-failure", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "S.M.A.R.T. monitoring is active; predictive drive failure warnings surface before data loss.",
                ApplyOps = [RegOp.SetDword(Key, "AllowStorageHealthMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowStorageHealthMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "AllowStorageHealthMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-enable-failure-prediction-warnings",
                Label = "Enable Drive Failure Prediction Warnings",
                Category = "Storage Health Policy",
                Description =
                    "Configures Windows to display user-visible warnings when predictive S.M.A.R.T. analysis indicates an imminent drive failure. Prompts users to back up data before catastrophic loss. Default: warning shown. Recommended: 1.",
                Tags = ["storage", "smart", "warning", "failure", "alert", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "User will receive an Action Center alert when S.M.A.R.T. predicts drive failure within 24–72 hours.",
                ApplyOps = [RegOp.SetDword(Key, "NotifyOnDrivePredictedFailure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NotifyOnDrivePredictedFailure")],
                DetectOps = [RegOp.CheckDword(Key, "NotifyOnDrivePredictedFailure", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-enable-wmi-health-events",
                Label = "Enable Storage Health WMI Event Notifications",
                Category = "Storage Health Policy",
                Description =
                    "Enables Windows Storage Health to emit WMI MSFT_StorageAlert events when drive health degrades. Allows monitoring tools, SIEM agents, and scripts to receive drive health change events. Default: not configured. Recommended: 1 for managed environments.",
                Tags = ["storage", "wmi", "events", "monitoring", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WMI MSFT_StorageAlert events are emitted on health degradation; consumable by monitoring agents.",
                ApplyOps = [RegOp.SetDword(Key, "EnableWmiStorageEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableWmiStorageEvents")],
                DetectOps = [RegOp.CheckDword(Key, "EnableWmiStorageEvents", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-set-polling-interval-24h",
                Label = "Set Storage Health Polling Interval to 24 Hours",
                Category = "Storage Health Policy",
                Description =
                    "Sets the S.M.A.R.T. polling interval to 24 hours. Balances monitoring coverage against unnecessary disk spin-up on sleeping drives. Default: varies. Recommended: 86400 seconds (24 h) for desktops.",
                Tags = ["storage", "polling", "smart", "interval", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "S.M.A.R.T. attributes are polled once every 24 hours; does not wake sleeping HDDs more than once a day.",
                ApplyOps = [RegOp.SetDword(Key, "HealthPollingIntervalSec", 86400)],
                RemoveOps = [RegOp.DeleteValue(Key, "HealthPollingIntervalSec")],
                DetectOps = [RegOp.CheckDword(Key, "HealthPollingIntervalSec", 86400)],
            },
            new TweakDef
            {
                Id = "strhlt-enable-ssd-health-check",
                Label = "Enable SSD Wear Levelling Health Check",
                Category = "Storage Health Policy",
                Description =
                    "Enables dedicated wear-levelling and write endurance health checks for NVMe and SATA SSD storage devices. Surfaces SSD lifespan metrics in the Windows Storage Health API. Default: enabled. Recommended: 1 for SSD-centric deployments.",
                Tags = ["storage", "ssd", "nvme", "wear-levelling", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SSD wear indicator and available spare capacity are monitored; early warning before write exhaustion.",
                ApplyOps = [RegOp.SetDword(Key, "EnableSsdHealthCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSsdHealthCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSsdHealthCheck", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-disable-health-telemetry-upload",
                Label = "Block Storage Health Telemetry Upload to Microsoft",
                Category = "Storage Health Policy",
                Description =
                    "Prevents Windows from uploading storage health telemetry (drive model, firmware, S.M.A.R.T. attributes) to Microsoft's cloud analytics. Keeps drive hardware details on-premises. Default: upload enabled. Recommended: 1.",
                Tags = ["storage", "telemetry", "privacy", "upload", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "S.M.A.R.T. data and drive identifiers are not sent to Microsoft; local health monitoring continues.",
                ApplyOps = [RegOp.SetDword(Key, "DisableStorageHealthTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageHealthTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStorageHealthTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-enable-volume-health-check",
                Label = "Enable File System Volume Health Scan",
                Category = "Storage Health Policy",
                Description =
                    "Enables periodic scan of NTFS/ReFS volume health (metadata integrity, bad clusters, USN change journal). Detects file system corruption before it becomes unrecoverable. Default: not enforced via policy. Recommended: 1.",
                Tags = ["storage", "ntfs", "volume", "integrity", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Periodic file system integrity scans are enforced; corruption is detected early.",
                ApplyOps = [RegOp.SetDword(Key, "EnableVolumeHealthScan", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableVolumeHealthScan")],
                DetectOps = [RegOp.CheckDword(Key, "EnableVolumeHealthScan", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-enable-spaces-health-monitoring",
                Label = "Enable Storage Spaces Health Monitoring",
                Category = "Storage Health Policy",
                Description =
                    "Activates continuous health monitoring for Storage Spaces pools, virtual disks, and storage tiers. Surfaces degraded, warning, and failed component status to the Storage API. Default: enabled. Recommended: 1 on Storage Spaces deployments.",
                Tags = ["storage", "spaces", "pool", "raid", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Storage Spaces pool degradation (e.g., a drive removed or failed) surfaces as an alert.",
                ApplyOps = [RegOp.SetDword(Key, "EnableStorageSpacesHealth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableStorageSpacesHealth")],
                DetectOps = [RegOp.CheckDword(Key, "EnableStorageSpacesHealth", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-log-health-events",
                Label = "Write Storage Health Events to System Event Log",
                Category = "Storage Health Policy",
                Description =
                    "Directs Windows Storage Health alerts and state transitions to the System event log. Enables IT operations and SIEM tools to centrally collect drive health history. Default: event log writing not enforced. Recommended: 1.",
                Tags = ["storage", "event-log", "audit", "health", "monitoring", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Storage health state changes are written to the Windows System log; collectable by SIEM/ops monitoring.",
                ApplyOps = [RegOp.SetDword(Key, "LogHealthEventsToEventLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogHealthEventsToEventLog")],
                DetectOps = [RegOp.CheckDword(Key, "LogHealthEventsToEventLog", 1)],
            },
            new TweakDef
            {
                Id = "strhlt-alert-threshold-10pct-spare",
                Label = "Alert When SSD Available Spare Falls Below 10%",
                Category = "Storage Health Policy",
                Description =
                    "Configures the SSD available spare capacity alert threshold at 10%. An Action Center notification is shown when an NVMe drive's available spare cells drop below this level, indicating approaching end-of-write-life. Default: not configured. Recommended: 10.",
                Tags = ["storage", "ssd", "spare", "threshold", "alert", "health", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Alert fires when SSD spare capacity is below 10%; early warning before write endurance is exhausted.",
                ApplyOps = [RegOp.SetDword(Key, "SsdSpareAlertThresholdPercent", 10)],
                RemoveOps = [RegOp.DeleteValue(Key, "SsdSpareAlertThresholdPercent")],
                DetectOps = [RegOp.CheckDword(Key, "SsdSpareAlertThresholdPercent", 10)],
            },
        ];
}
