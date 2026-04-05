namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyStorage
{
    // ── StorageBusPolicy ──
    private static class _StorageBusPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageBus";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "stobus-disable-ahci-link-power",
                    Label = "Disable AHCI SATA Link Power Management (HIPM/DIPM)",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables Host-Initiated (HIPM) and Device-Initiated (DIPM) power management on SATA AHCI links, preventing drives from entering partial or slumber power states that increase seek latency when awoken.",
                    Tags = ["sata", "ahci", "power-management", "storage", "latency", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "AHCI link power management disabled; SATA drives remain fully active, eliminating resume latency.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAHCILinkPowerMgmt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAHCILinkPowerMgmt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAHCILinkPowerMgmt", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-disable-nvme-auto-power-state",
                    Label = "Disable NVMe Autonomous Power State Transitions",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables NVMe Autonomous Power State Transitions (APST) which allow NVMe drives to enter lower power states automatically, preventing latency spikes when the drive transitions back to full performance mode.",
                    Tags = ["nvme", "power-state", "apst", "storage", "latency", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "NVMe APST disabled; drive remains at PS0, removing latency penalty of power state transitions.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNVMeAutoPowerStateTransition", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNVMeAutoPowerStateTransition")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNVMeAutoPowerStateTransition", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-disable-usb-selective-suspend",
                    Label = "Disable USB Selective Suspend for Storage Devices",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables USB selective suspend specifically for USB-connected storage devices, preventing USB drives and SSDs from entering low-power suspended state and causing reconnect delays.",
                    Tags = ["usb", "selective-suspend", "storage", "latency", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "USB storage selective suspend disabled; USB drives remain active and immediately accessible.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUSBStorageSelectiveSuspend", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUSBStorageSelectiveSuspend")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUSBStorageSelectiveSuspend", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-disable-write-cache-buffer-flush",
                    Label = "Disable Write Cache Buffer Flushing on Disk",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables write-cache buffer flush on supported storage devices, allowing the drive's write cache to hold data longer for better write throughput at the cost of potential data loss on unexpected power loss.",
                    Tags = ["disk", "write-cache", "performance", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 2,
                    ImpactNote = "Write cache flush disabled; significantly improved write throughput but data may be lost on sudden power loss.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWriteCacheBufferFlush", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWriteCacheBufferFlush")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWriteCacheBufferFlush", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-set-io-scheduler-none",
                    Label = "Set I/O Scheduler to None (Passthrough) for NVMe",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Configures the storage I/O scheduler to passthrough mode for NVMe drives, removing queue depth reordering overhead and allowing the NVMe controller's own internal queue to handle command ordering.",
                    Tags = ["nvme", "io-scheduler", "performance", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NVMe I/O scheduler set to passthrough; NVMe internal queue manages command ordering directly.",
                    ApplyOps = [RegOp.SetDword(Key, "NVMeIOSchedulerNone", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NVMeIOSchedulerNone")],
                    DetectOps = [RegOp.CheckDword(Key, "NVMeIOSchedulerNone", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-disable-sata-hot-plug",
                    Label = "Disable SATA Hot-Plug Detection",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables SATA hot-plug detection on AHCI ports, preventing the OS from polling for newly inserted drives and reducing periodic interrupt overhead on systems with no hot-swappable SATA drives.",
                    Tags = ["sata", "hot-plug", "performance", "interrupt", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "SATA hot-plug disabled; no periodic hot-insert/remove polling. Cannot detect live SATA drive swaps.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSATAHotPlug", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSATAHotPlug")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSATAHotPlug", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-increase-nvme-queue-depth-32",
                    Label = "Increase Default NVMe Queue Depth to 32",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets the default NVMe submission queue depth to 32 entries, increasing I/O parallelism for high-throughput workloads that can saturate the default smaller queue depth.",
                    Tags = ["nvme", "queue-depth", "performance", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NVMe queue depth increased to 32; higher I/O parallelism for concurrent random workloads.",
                    ApplyOps = [RegOp.SetDword(Key, "NVMeDefaultQueueDepth", 32)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NVMeDefaultQueueDepth")],
                    DetectOps = [RegOp.CheckDword(Key, "NVMeDefaultQueueDepth", 32)],
                },
                new TweakDef
                {
                    Id = "stobus-disable-sata-aggressive-link-power",
                    Label = "Disable Aggressive SATA Link Power Management",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables aggressive SATA link power management (ALPM) that transitions the SATA PHY into slumber state after very short idle periods, preventing the deep slumber latency that affects interactive random I/O workloads.",
                    Tags = ["sata", "alpm", "power-management", "latency", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Aggressive SATA link power management disabled; SATA PHY stays active, no slumber resume delay.",
                    ApplyOps = [RegOp.SetDword(Key, "DisagegressiveSATALinkPowerMgmt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisagegressiveSATALinkPowerMgmt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisagegressiveSATALinkPowerMgmt", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-block-error-recovery-control",
                    Label = "Disable Storage Error Recovery Control Timeout",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables the storage bus Error Recovery Control (ERC) that forces commands to fail after a short timeout, allowing drives to take longer to recover from errors rather than being immediately flagged as failed.",
                    Tags = ["storage", "erc", "error-recovery", "reliability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Storage ERC timeout disabled; drives given longer time to self-recover before being marked as error.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStorageERC", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageERC")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStorageERC", 1)],
                },
                new TweakDef
                {
                    Id = "stobus-enable-storage-bus-health-log",
                    Label = "Enable Storage Bus Controller Health Event Logging",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Enables event log entries for storage bus controller health events including AHCI/NVMe errors, command timeouts, and bus reset events for proactive storage failure monitoring.",
                    Tags = ["storage", "event-log", "health", "controller", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Storage bus health events logged; controller errors and resets recorded in System event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableStorageBusHealthLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableStorageBusHealthLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableStorageBusHealthLogging", 1)],
                },
            ];
    }

    // ── StorageHealthPolicy ──
    private static class _StorageHealthPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageHealth";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "strhlt-enable-smart-monitoring",
                    Label = "Enable S.M.A.R.T. Drive Health Monitoring",
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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
                    Category = "Storage — Refs Fs",
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

    // ── StorageManagementPolicy ──
    private static class _StorageManagementPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageManagement";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "stormgmt-disable-storage-spaces-ui",
                Label = "Disable Storage Spaces Configuration UI",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableStorageSpacesPanel=1 in the StorageManagement policy key. "
                    + "Prevents non-administrator users from accessing the Storage Spaces "
                    + "configuration panel in Settings. Storage Spaces allows creating "
                    + "software RAID pools; unrestricted access on a shared machine could "
                    + "allow a user to inadvertently destroy volume pools or create "
                    + "unauthorised redundant mirrors. "
                    + "Default: 0. Recommended: 1 on managed workstations.",
                Tags = ["storage", "spaces", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStorageSpacesPanel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageSpacesPanel")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStorageSpacesPanel", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-storage-tiering",
                Label = "Disable Storage Tiering Policy",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableStorageTiering=1 in the StorageManagement policy key. Prevents "
                    + "the Windows Storage Tiers Optimization task from promoting hot data to "
                    + "fast SSD tiers or demoting cold data to HDD tiers in a tiered Storage "
                    + "Spaces pool. On workstations without tiered pools this policy has no "
                    + "effect; on pools it disables background data shuffling that generates "
                    + "unexpected I/O bursts at scheduling time. "
                    + "Default: 0. Recommended: 1 on non-tiered systems.",
                Tags = ["storage", "tiering", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStorageTiering", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageTiering")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStorageTiering", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-vs-notification",
                Label = "Disable Volume Shadow Copy Low-Disk Notifications",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableVSCNotification=1 in the StorageManagement policy key. "
                    + "Suppresses toast notifications that inform users when Volume Shadow Copy "
                    + "Service snapshots are about to be purged due to low available disk space. "
                    + "On systems with proactive disk monitoring and automated clean-up scripts "
                    + "these notifications are redundant and clutter the notification centre. "
                    + "Default: 0. Recommended: 1 on managed systems with monitoring tools.",
                Tags = ["storage", "vss", "notification", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVSCNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVSCNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVSCNotification", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-disk-cleanup-prompt",
                Label = "Disable Disk Cleanup Low-Space Prompt",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoLowDiskSpaceChecks=1 in the StorageManagement policy key. Stops "
                    + "the Windows shell from displaying balloon or toast notifications warning "
                    + "that a drive is running low on space. On servers and workstations with "
                    + "automated storage management tools, duplicated low-disk alerts from the "
                    + "shell add unnecessary noise. Monitoring agents provide the same signal "
                    + "with actionable context. Default: 0. Recommended: 1.",
                Tags = ["storage", "diskcleanup", "notification", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoLowDiskSpaceChecks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoLowDiskSpaceChecks")],
                DetectOps = [RegOp.CheckDword(Key, "NoLowDiskSpaceChecks", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-ntfs-tunneling",
                Label = "Disable NTFS File Name Tunneling",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets NtfsDisable8dot3NameCreation=1 in the StorageManagement policy key. "
                    + "Prevents NTFS from generating legacy 8.3 short file names (e.g., "
                    + "PROGRA~1) for new files. Short name generation requires extra I/O for "
                    + "each file-create operation so that NTFS can confirm uniqueness within "
                    + "the directory. Disabling it improves directory enumeration performance "
                    + "on volumes with large file counts. "
                    + "Default: 0. Recommended: 1 on modern systems.",
                Tags = ["ntfs", "8dot3", "short-name", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NtfsDisable8dot3NameCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NtfsDisable8dot3NameCreation")],
                DetectOps = [RegOp.CheckDword(Key, "NtfsDisable8dot3NameCreation", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-storage-diagnostics",
                Label = "Disable Storage Diagnostic Data Collection",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableStorageDiagnostics=1 in the StorageManagement policy key. "
                    + "Stops Windows from collecting storage-device health statistics (SMART "
                    + "data, I/O error rates, read/write latency histograms) and encrypting "
                    + "them into WER reports that are transmitted to Microsoft's reliability "
                    + "servers. Storage diagnostics reveal hardware identifiers and usage "
                    + "patterns. Default: 0. Recommended: 1.",
                Tags = ["storage", "diagnostics", "telemetry", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStorageDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageDiagnostics")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStorageDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-hot-spare-alert",
                Label = "Disable Storage Spaces Hot Spare Alert",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableHotSpareAlert=1 in the StorageManagement policy key. Silences "
                    + "the action-centre notification that fires when a Storage Spaces pool hot "
                    + "spare is consumed due to a disk failure. On enterprise systems that use "
                    + "dedicated monitoring dashboards, this Windows notification is a "
                    + "duplicate alert that does not provide actionable remediation steps. "
                    + "Default: 0. Recommended: 1 on monitored servers.",
                Tags = ["storage", "hotspare", "notification", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHotSpareAlert", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHotSpareAlert")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHotSpareAlert", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-data-deduplication",
                Label = "Disable Data Deduplication Policy",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableDataDeduplication=1 in the StorageManagement policy key. "
                    + "Prevents the Windows Data Deduplication role from being enabled on "
                    + "volumes managed by this policy. Deduplication background jobs run "
                    + "at low priority but read every file on the volume to build a chunk "
                    + "hash store, generating sustained I/O load that can interfere with "
                    + "latency-sensitive workloads. Default: 0. Recommended: 1.",
                Tags = ["storage", "deduplication", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDataDeduplication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDataDeduplication")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDataDeduplication", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-disk-management-snap",
                Label = "Restrict Disk Management Snap-In",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RestrictDiskMgmtSnap=1 in the StorageManagement policy key. Prevents "
                    + "standard users from launching the Disk Management MMC snap-in, which "
                    + "provides a GUI for creating, deleting, and formatting partitions. "
                    + "On shared workstations or kiosk machines, unrestricted access to Disk "
                    + "Management poses a data destruction risk. Administrators retain full "
                    + "access via UAC elevation. Default: 0. Recommended: 1.",
                Tags = ["storage", "diskmanagement", "mmc", "restrict", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDiskMgmtSnap", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDiskMgmtSnap")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDiskMgmtSnap", 1)],
            },
            new TweakDef
            {
                Id = "stormgmt-disable-low-disk-warning",
                Label = "Disable Persistent Low-Disk-Space Warning",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableLowDiskSpaceWarning=1 in the StorageManagement policy key. "
                    + "Removes the persistent yellow warning indicator displayed in File "
                    + "Explorer's navigation pane when a drive drops below the low-disk "
                    + "threshold. Systems with automated clean-up or thin-provisioned virtual "
                    + "disks frequently recover without user intervention; the persistent "
                    + "warning icon adds clutter without prompting the correct remediation. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["storage", "lowdisk", "warning", "explorer", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLowDiskSpaceWarning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLowDiskSpaceWarning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLowDiskSpaceWarning", 1)],
            },
        ];
    }

    // ── StoragePoolPolicy ──
    private static class _StoragePoolPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StoragePools";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "stpool-restrict-pool-creation",
                Label = "Restrict Storage Pool Creation to Administrators",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Storage pool creation allows users to combine physical disks into virtual storage with redundancy and flexible volume management. Restricting pool creation to administrators prevents standard users from creating storage pools that could bypass access controls or disk encryption policies. Non-administrative storage pool creation could allow users to aggregate organizational disk capacity in ways that circumvent management policies. Storage pools created by regular users may not be subject to the same encryption and access control requirements as administrator-managed storage. Administrator-only pool creation ensures that all storage pool configurations are reviewed and approved before implementation. Enterprise storage management should be centrally controlled to ensure consistent encryption, backup, and access control policies.",
                Tags = ["storage", "pool", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictPoolCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictPoolCreation")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictPoolCreation", 1)],
            },
            new TweakDef
            {
                Id = "stpool-enable-pool-encryption",
                Label = "Require Encryption for New Storage Pools",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Storage pool encryption protects data on pooled storage volumes when disks are removed from the system or accessed offline. Requiring encryption for new storage pools ensures that all pooled storage contains protected data consistent with enterprise encryption policies. Storage pools without encryption leave data accessible to anyone who can access the disk array outside of Windows access controls. BitLocker on virtual disks within storage pools provides encryption but pools themselves must be configured to enable transparent encryption. Encrypted storage pools ensure compliance with data protection regulations requiring encryption of data at rest regardless of storage media type. Organizations deploying storage pools for sensitive data must ensure pool-level encryption is part of the initial configuration.",
                Tags = ["storage", "pool", "encryption", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequirePoolEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePoolEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePoolEncryption", 1)],
            },
            new TweakDef
            {
                Id = "stpool-restrict-disk-addition",
                Label = "Restrict Disk Addition to Storage Pools",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting disk addition to authorized administrators prevents unauthorized expansion of storage pools that could dilute security controls. Only administrators with explicit authorization should be able to add physical disks to existing storage pools. Unauthorized disk addition could be used to expand storage pool capacity with uncontrolled disks that bypass encryption requirements. Rogue disk insertion is a threat in physical access scenarios where an attacker adds a disk to a pool to capture data written to the pool. Restricting disk addition requires administrator authorization for any physical disk changes to managed storage pools. Enterprise storage management tools should log disk addition events for audit purposes alongside this administrative restriction.",
                Tags = ["storage", "pool", "disk-management", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDiskAddition", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDiskAddition")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDiskAddition", 1)],
            },
            new TweakDef
            {
                Id = "stpool-disable-thin-provisioning",
                Label = "Disable Thin Provisioning for Storage Pools",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Thin provisioning allows storage pool virtual disks to be allocated beyond the physical capacity with storage committed as data is written. Disabling thin provisioning for managed storage prevents over-allocation scenarios that can lead to data loss when storage exhaustion occurs. Thin-provisioned pools can become fully consumed unexpectedly causing virtual disk failures when logical capacity exceeds physical storage. Fixed provisioning ensures that allocated storage is backed by physical capacity eliminating surprise storage exhaustion. Enterprise storage management should use fixed provisioning for critical workloads where data loss due to storage exhaustion is unacceptable. Monitoring storage pool utilization is still important for fixed pools but eliminates the risk of logical-to-physical storage mismatch.",
                Tags = ["storage", "pool", "provisioning", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableThinProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableThinProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableThinProvisioning", 1)],
            },
            new TweakDef
            {
                Id = "stpool-enable-integrity-streams",
                Label = "Enable Storage Pool Integrity Streams",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Storage pool integrity streams provide end-to-end data integrity verification using checksums to detect silent data corruption. Enabling integrity streams ensures that data read from storage pools is verified against stored checksums and corruption is detected. Silent data corruption can occur due to firmware bugs, electrical issues, or hardware failures causing data to be written incorrectly. Integrity streams in Storage Spaces are stored in a parallel data stream that tracks checksums for each data block. When corruption is detected in a mirrored pool integrity streams can automatically repair corrupted data blocks using the mirror copy. Organizations storing critical data in storage pools should enable integrity streams to protect against bit-rot and silent disk corruption.",
                Tags = ["storage", "pool", "integrity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityStreams", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityStreams")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityStreams", 1)],
            },
            new TweakDef
            {
                Id = "stpool-require-mirroring",
                Label = "Require Mirroring for Storage Pool Virtual Disks",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Storage pool mirroring maintains multiple copies of data across different physical disks providing protection against disk failure. Requiring mirroring ensures that all storage pool virtual disks have redundancy through two-way or three-way mirror configurations. Simple (no redundancy) storage pool disks provide no protection against disk failure and should not be used for critical data. Mirror resiliency in Storage Spaces ensures continued operation and data preservation when one or more disks fail in the pool. Organizations storing critical operational data in storage pools must use mirroring or parity configurations to meet availability requirements. Requiring mirroring through policy prevents accidental creation of simple virtual disks that lack redundancy for critical workloads.",
                Tags = ["storage", "pool", "mirroring", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireMirroredRedundancy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMirroredRedundancy")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMirroredRedundancy", 1)],
            },
            new TweakDef
            {
                Id = "stpool-enable-pool-health-monitoring",
                Label = "Enable Storage Pool Health Event Monitoring",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Storage pool health monitoring tracks the operational state of storage pools and virtual disks generating events for degraded or warning conditions. Enabling health monitoring ensures that disk failures, pool degradation, and capacity warnings generate Windows events for operational visibility. Pool health events allow administrators to respond to degraded mirror states before data loss occurs from a second disk failure. Storage Spaces health events are logged to the System event log and can be monitored through Windows Admin Center or SCOM. Automated alerting on pool health events reduces the response time to disk failures and prevents extended exposure to single-disk pools. Health monitoring combined with regular operational reviews ensures storage infrastructure operates within designed parameters.",
                Tags = ["storage", "pool", "health", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePoolHealthMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePoolHealthMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePoolHealthMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "stpool-restrict-pool-deletion",
                Label = "Restrict Storage Pool Deletion to Administrators",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting storage pool deletion prevents accidental or malicious removal of storage pools that would destroy all contained virtual disk data. Administrator-only pool deletion ensures that significant storage configuration changes require elevated authorization. Ransomware has been observed deleting or corrupting storage pool configurations to maximize damage and complicate recovery. Non-administrative deletion restriction reduces the impact of compromised standard user accounts on storage infrastructure. Pool deletion restrictions combined with confirmation dialogs provide additional safeguards against accidental data destruction. Regular backup of storage pool configuration alongside data backup ensures recovery capability after intentional or accidental deletion.",
                Tags = ["storage", "pool", "deletion", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictPoolDeletion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictPoolDeletion")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictPoolDeletion", 1)],
            },
            new TweakDef
            {
                Id = "stpool-disable-deduplication-auto",
                Label = "Disable Automatic Data Deduplication on Storage Pools",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Automatic data deduplication reduces storage consumption by identifying and eliminating duplicate data blocks but has CPU and memory implications. Disabling automatic deduplication prevents unscheduled deduplication operations that can impact system performance during business hours. Deduplication should be explicitly configured and scheduled by administrators rather than running automatically without capacity planning. Some data types including encrypted files and compressed media do not benefit from deduplication and the overhead is wasted on these workloads. Deduplication on storage pools with integrity streams also requires careful configuration to maintain both features correctly. Organizations using deduplication should enable it with explicit schedules during low-activity periods rather than automatic unscheduled runs.",
                Tags = ["storage", "pool", "deduplication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoDeduplication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDeduplication")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoDeduplication", 1)],
            },
            new TweakDef
            {
                Id = "stpool-audit-pool-changes",
                Label = "Enable Storage Pool Configuration Change Auditing",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Storage pool configuration change auditing records modifications to pool membership, virtual disk configurations, and resiliency settings. Enabling pool change auditing generates security events for all administrative changes to storage pool configurations. Unexpected changes to storage pool configurations may indicate unauthorized administrative access or insider threat activity. Audit records of pool configuration changes support change management processes and provide evidence for security investigations. Storage pool change events should be forwarded to SIEM infrastructure and correlated with administrator login events. Baseline documentation of storage pool configurations combined with change auditing enables rapid detection of unauthorized modifications.",
                Tags = ["storage", "pool", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditPoolConfigChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPoolConfigChanges")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPoolConfigChanges", 1)],
            },
        ];
    }

    // ── StorageReplicaPolicy ──
    private static class _StorageReplicaPolicy
    {
        private const string SrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageReplica";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "srep-set-replication-mode-async",
                    Label = "Storage Replica: Set Default Replication Mode to Asynchronous",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets ReplicationMode=1 in StorageReplica policy (1 = Asynchronous). Sets the default Storage Replica replication mode to asynchronous. In asynchronous mode, the source volume acknowledges writes to the application without waiting for the destination to confirm it has received and written the data — the replication lags behind the source (RPO > 0). Synchronous mode (the default for same-site replica pairs) forces writes to complete on both source and destination before acknowledgment — zero RPO but application write latency is increased by the round-trip to the destination. For WAN-linked DR sites, synchronous replication is impractical; asynchronous mode is required to avoid write latency spikes.",
                    Tags = ["storage-replica", "async", "replication-mode", "rpo", "dr"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Storage Replica uses asynchronous replication by default. The destination replica may lag behind the source by seconds to minutes depending on network latency and bandwidth. In a failover, the most recent data on the destination is used — any writes not yet replicated are lost.",
                    ApplyOps = [RegOp.SetDword(SrKey, "ReplicationMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "ReplicationMode")],
                    DetectOps = [RegOp.CheckDword(SrKey, "ReplicationMode", 1)],
                },
                new TweakDef
                {
                    Id = "srep-set-log-volume-size-8gb",
                    Label = "Storage Replica: Set Minimum Log Volume Size to 8 GB",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets MinLogSize=8192 in StorageReplica policy (MB). Sets the minimum log volume size for new Storage Replica partnerships to 8 GB. The SR log volume holds a circular write buffer that tracks changes that have been committed on the source but not yet replicated to the destination. If the log volume is too small, it can overflow during bursts of write activity — forcing Storage Replica to perform a full resync of the entire replicated volume. For volumes with heavy write workloads (SQL Server transaction logs, VMs with active guest IO), 8 GB provides headroom during network outages of several hours.",
                    Tags = ["storage-replica", "log", "circular-buffer", "resync", "sizing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Storage Replica log volumes are at least 8 GB. Prevents accidental log overflow and forced full resync. Log volumes are dedicated (no user data) and must be formatted as NTFS or ReFS.",
                    ApplyOps = [RegOp.SetDword(SrKey, "MinLogSize", 8192)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "MinLogSize")],
                    DetectOps = [RegOp.CheckDword(SrKey, "MinLogSize", 8192)],
                },
                new TweakDef
                {
                    Id = "srep-set-bandwidth-limit-100mbps",
                    Label = "Storage Replica: Set Replication Bandwidth Limit to 100 Mbps",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets BandwidthLimit=100 in StorageReplica policy (Mbps). Limits Storage Replica replication bandwidth to 100 Mbps. Without a bandwidth limit, Storage Replica can saturate available network links — particularly during initial sync of large volumes (1 TB+) which can overwhelm a 1 Gbps uplink if allowed to run unconstrained. 100 Mbps allows a 1 TB volume to be initially synced in approximately 22 hours while leaving 900 Mbps of uplink capacity for other traffic. This limit applies to both initial sync and ongoing delta replication.",
                    Tags = ["storage-replica", "bandwidth", "throttle", "network", "wan"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Replication traffic is capped at 100 Mbps. Initial sync of large volumes is slower but the production network is protected. Adjust based on network capacity and RPO requirements — a smaller limit increases replication lag.",
                    ApplyOps = [RegOp.SetDword(SrKey, "BandwidthLimit", 100)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "BandwidthLimit")],
                    DetectOps = [RegOp.CheckDword(SrKey, "BandwidthLimit", 100)],
                },
                new TweakDef
                {
                    Id = "srep-enable-consistent-replica-read",
                    Label = "Storage Replica: Enable Read Access on Destination Replica",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets AllowReplicaRead=1 in StorageReplica policy. Enables read-only access to the destination volume in a Storage Replica pair. By default, the destination volume is mounted offline (no read access) to ensure consistency — the SR log is continuously writing to it. With AllowReplicaRead enabled, SR temporarily snapshots the destination volume to provide a read-only mount point that users and applications can query — useful for offloading backup operations, reporting queries, or compliance snapshots to the replica without impacting the source production volume.",
                    Tags = ["storage-replica", "read-replica", "backup", "reporting", "snapshot"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Destination replica is accessible as a read-only snapshot. Backup and reporting workloads can be offloaded to the replica. The read-only snapshot is a point-in-time copy; it captures the state at the time of the snapshot creation.",
                    ApplyOps = [RegOp.SetDword(SrKey, "AllowReplicaRead", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "AllowReplicaRead")],
                    DetectOps = [RegOp.CheckDword(SrKey, "AllowReplicaRead", 1)],
                },
                new TweakDef
                {
                    Id = "srep-enable-encrypted-replication",
                    Label = "Storage Replica: Enable Encrypted Replication Traffic",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets EncryptionEnabled=1 in StorageReplica policy. Enables SMB3 encryption for all Storage Replica replication traffic between source and destination servers. Replication channels carry live production data (potentially including sensitive PII, financial records, health information) traversing internal networks or WAN links. Without encryption, a packet capture on any network segment in the replication path reveals the data content. AES-256-GCM encryption wraps all replication traffic, ensuring the data payload is unreadable to network observers.",
                    Tags = ["storage-replica", "encryption", "smb3", "aes", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "All replication traffic is AES-256-GCM encrypted. CPU overhead for encryption is approximately 5-15% additional CPU usage on high-throughput replicas. Both source and destination servers must support SMB3 encryption. Negligible impact when servers have AES-NI hardware acceleration.",
                    ApplyOps = [RegOp.SetDword(SrKey, "EncryptionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "EncryptionEnabled")],
                    DetectOps = [RegOp.CheckDword(SrKey, "EncryptionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "srep-set-io-buffer-size-32mb",
                    Label = "Storage Replica: Set Replication IO Buffer Size to 32 MB",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets IOBufferSize=32 in StorageReplica policy (MB). Sets the IO write buffer size used by Storage Replica for batching writes to the replication log. Larger IO buffers reduce the number of individual write operations to the SR log disk — improving throughput at the cost of increased memory usage. 32 MB is a practical default for most environments. Very small IO buffers cause excessive fragmented log writes, reducing sustained replication throughput to the available log disk IOps. For environments with NVMe log drives, increasing to 64 MB or more may improve throughput further.",
                    Tags = ["storage-replica", "io-buffer", "performance", "throughput", "log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SR uses 32 MB IO buffers for replication log writes. Improves log write throughput. Uses approximately 32 MB additional kernel non-paged pool memory per SR partnership. Adjust upward for NVMe log drives.",
                    ApplyOps = [RegOp.SetDword(SrKey, "IOBufferSize", 32)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "IOBufferSize")],
                    DetectOps = [RegOp.CheckDword(SrKey, "IOBufferSize", 32)],
                },
                new TweakDef
                {
                    Id = "srep-enable-replication-health-audit",
                    Label = "Storage Replica: Enable Replication Health Audit Logging",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets HealthAuditEnabled=1 in StorageReplica policy. Enables periodic health audit logging for Storage Replica partnerships. Health audit events record the current replication state, lag (delta between source and destination), log utilisation percentage, and any fault conditions for each SR group. Without health auditing, the current state of disaster recovery capability is only available by querying WMI — it is not proactively logged. Health audit events enable SIEM correlation to track RPO compliance: if replication lag exceeds the target RPO, an alert fires before a disaster is declared.",
                    Tags = ["storage-replica", "health", "audit", "monitoring", "rpo"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SR writes periodic health audit events to the Windows event log. Events include replication lag, log utilisation, and fault state. Use with event forwarding or a SIEM to alert on RPO violations. Minimal disk overhead.",
                    ApplyOps = [RegOp.SetDword(SrKey, "HealthAuditEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "HealthAuditEnabled")],
                    DetectOps = [RegOp.CheckDword(SrKey, "HealthAuditEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "srep-disable-auto-failover",
                    Label = "Storage Replica: Disable Automatic Failover",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets AutoFailover=0 in StorageReplica policy. Prevents Storage Replica from automatically promoting the destination volume when it detects that the source server is unavailable. Automatic failover can cause split-brain scenarios: if the source server is temporarily unreachable due to network issues (rather than truly offline), both source and destination may become active producers — writing data that diverges and cannot be automatically reconciled. In all DR scenarios, manual failover + human validation of data consistency is recommended before promoting the replica.",
                    Tags = ["storage-replica", "failover", "split-brain", "dr", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Failover to the destination replica requires manual intervention. The destination volume is not promoted automatically if the source becomes unreachable. Human verification of failure before failover prevents split-brain data divergence. RPO and RTO must account for manual failover time.",
                    ApplyOps = [RegOp.SetDword(SrKey, "AutoFailover", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "AutoFailover")],
                    DetectOps = [RegOp.CheckDword(SrKey, "AutoFailover", 0)],
                },
                new TweakDef
                {
                    Id = "srep-enable-log-compression",
                    Label = "Storage Replica: Enable Replication Log Compression",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets LogCompression=1 in StorageReplica policy. Enables transparent compression of Storage Replica log data before writing to the log volume. Compression reduces the amount of physically written data to the log disk, which is especially valuable when the log volume is an SSD with limited write endurance (TBW rating). For workloads with compressible data patterns (text files, compressed XML, Hyper-V VHD zero pages), log compression can reduce log write volume by 40-60%, extending SSD log drive lifetime. Decompression occurs before entries are sent to the destination.",
                    Tags = ["storage-replica", "compression", "log", "ssd-endurance", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SR log data is compressed before writing. Extends SSD write endurance on log drives. CPU overhead for compression is approximately 5-10% on high-throughput replicas. Incompressible data (already-compressed files) sees minimal benefit.",
                    ApplyOps = [RegOp.SetDword(SrKey, "LogCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "LogCompression")],
                    DetectOps = [RegOp.CheckDword(SrKey, "LogCompression", 1)],
                },
                new TweakDef
                {
                    Id = "srep-set-replication-port-5445",
                    Label = "Storage Replica: Set Replication Network Port to 5445",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets ReplicationPort=5445 in StorageReplica policy. Configures Storage Replica to use TCP port 5445 for replication traffic. The well-known Storage Replica port (5445) must be open in firewalls between source and destination servers. By explicitly setting the port via policy (rather than relying on the default), firewall rules can be precisely targeted and audited. Port 5445 is the standard SR port — SMB-based SR partners also require port 445 for SMB3 transport; this policy governs the SR control channel. Firewall rules for SR replication should permit TCP 5445 bidirectionally between SR members.",
                    Tags = ["storage-replica", "port", "firewall", "network", "configuration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "SR replication control channel uses TCP 5445. Firewall policy must permit TCP 5445 bidirectionally between SR source and destination. Changing from a custom port back to the default 5445 requires SR service restart and firewall rule update.",
                    ApplyOps = [RegOp.SetDword(SrKey, "ReplicationPort", 5445)],
                    RemoveOps = [RegOp.DeleteValue(SrKey, "ReplicationPort")],
                    DetectOps = [RegOp.CheckDword(SrKey, "ReplicationPort", 5445)],
                },
            ];
    }

    // ── StorageSensePolicy ──
    private static class _StorageSensePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "storsense-disable-temp-file-cleanup",
                    Label = "Disable Storage Sense Temporary File Cleanup",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Prevents Storage Sense from deleting temporary files, ensuring applications that rely on temp files across sessions are not disrupted by automatic cleanup.",
                    Tags = ["storage sense", "temp files", "cleanup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Preserves temp files across cleanup cycles; may result in higher disk usage over time.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "AllowStorageSenseTemporaryFiles", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowStorageSenseTemporaryFiles")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowStorageSenseTemporaryFiles", 0)],
                },
                new TweakDef
                {
                    Id = "storsense-disable-downloads-cleanup",
                    Label = "Disable Storage Sense Downloads Folder Cleanup",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Prevents Storage Sense from deleting files in the Downloads folder, protecting user-downloaded content from automatic removal based on age thresholds.",
                    Tags = ["storage sense", "downloads", "cleanup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Protects Downloads folder from automatic cleanup; users may accumulate large stale downloads.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "AllowStorageSenseDownloads", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowStorageSenseDownloads")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowStorageSenseDownloads", 0)],
                },
                new TweakDef
                {
                    Id = "storsense-disable-cloud-dehydration",
                    Label = "Disable Storage Sense OneDrive Cloud File Dehydration",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Prevents Storage Sense from automatically dehydrating (moving to cloud-only) OneDrive files that have not been opened recently, keeping local copies always accessible.",
                    Tags = ["storage sense", "onedrive", "cloud", "dehydration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents unexpected file dehydration; local copies remain accessible offline at cost of disk space.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "AllowStorageSenseOneDrive", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowStorageSenseOneDrive")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowStorageSenseOneDrive", 0)],
                },
                new TweakDef
                {
                    Id = "storsense-set-run-cadence-monthly",
                    Label = "Set Storage Sense Run Cadence to Monthly",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Configures Storage Sense to run automatically once per month rather than on a per-user-defined schedule, standardizing cleanup frequency across managed devices.",
                    Tags = ["storage sense", "cadence", "schedule", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Standardizes Storage Sense frequency to monthly; less disruptive than daily or weekly.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ConfigStorageSenseGlobal", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigStorageSenseGlobal")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigStorageSenseGlobal", 30)],
                },
                new TweakDef
                {
                    Id = "storsense-enforce-storage-policies",
                    Label = "Enforce Storage Sense Policies on All Users",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Enables enforcement of machine-wide Storage Sense policies, ensuring policy-configured thresholds and cadence settings take precedence over individual user preferences.",
                    Tags = ["storage sense", "enforce", "policy", "users"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Policy settings override user-configured Storage Sense preferences machine-wide.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "StoragePoliciesEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "StoragePoliciesEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "StoragePoliciesEnabled", 1)],
                },
            ];
    }

    // ── StorageSpacesMigrationPolicy ──
    private static class _StorageSpacesMigrationPolicy
    {
        private const string SsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSpaces";

        private const string SmsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageMigrationService";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ssmig-require-storage-encryption",
                    Label = "Storage Spaces Migration: Require Encryption on All Storage Pools",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets RequireEncryption=1 in StorageSpaces policy. Requires all Storage Spaces pools managed by Group Policy to be encrypted with BitLocker Drive Encryption. When a new pool is created or an existing pool is brought online, the policy mandates that its volumes are protected by BitLocker. Storage pools without encryption are flagged and can be quarantined by this policy. Protects against direct disk extraction attacks where physical drives removed from a Storage Spaces mirror could be read on another system.",
                    Tags = ["storage-spaces", "encryption", "bitlocker", "data-protection", "pool"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "All Storage Spaces pools must be BitLocker-encrypted. Pools without encryption may have restricted write access or be unable to come online. Requires BitLocker policies and TPM to be configured.",
                    ApplyOps = [RegOp.SetDword(SsKey, "RequireEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "RequireEncryption")],
                    DetectOps = [RegOp.CheckDword(SsKey, "RequireEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-enable-auto-tiering",
                    Label = "Storage Spaces Migration: Enable Automatic Tiering (SSD+HDD)",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets AutoTiering=1 in StorageSpaces policy. Enables automatic storage tiering in tiered Storage Spaces pools. Storage Spaces Direct and standard tiered pools can place hot (frequently accessed) data on NVMe or SSD tiers and cold data on HDD tiers automatically. Without this policy, tiering must be explicitly configured per-volume. Automatic tiering monitors access patterns over a 24-hour window and promotes/demotes data blocks accordingly. For mixed SSD+HDD pools, this significantly improves read performance for hot data without requiring manual optimisation.",
                    Tags = ["storage-spaces", "tiering", "ssd", "hdd", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Hot data blocks are automatically promoted to SSD tier; cold data demoted to HDD. Requires a tiered Storage Spaces pool with both SSD and HDD tiers. Tiering optimisation runs as a background service during low-activity periods.",
                    ApplyOps = [RegOp.SetDword(SsKey, "AutoTiering", 1)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "AutoTiering")],
                    DetectOps = [RegOp.CheckDword(SsKey, "AutoTiering", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-enable-proactive-drive-retirement",
                    Label = "Storage Spaces Migration: Enable Proactive Drive Retirement on SMART Failure",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets ProactiveDriveRetirement=1 in StorageSpaces policy. Enables Storage Spaces to automatically retire (mark as unavailable) a member drive when it reports SMART (Self-Monitoring, Analysis and Reporting Technology) drive health warnings indicating impending failure. When a drive is retired, Storage Spaces redistributes its data to healthy pool members if the pool has sufficient redundancy. Without proactive retirement, a failing drive stays in the pool until it actually fails — at which point data reconstruction is urgent and failure of a second drive during rebuild can cause data loss.",
                    Tags = ["storage-spaces", "smart", "drive-failure", "resilience", "retirement"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Drives with SMART warnings are proactively removed from the pool and rebuilt. Pool rebuilds consume IO bandwidth and time. Pool must have sufficient spare capacity or a hot spare drive. Recommended for all production Storage Spaces deployments.",
                    ApplyOps = [RegOp.SetDword(SsKey, "ProactiveDriveRetirement", 1)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "ProactiveDriveRetirement")],
                    DetectOps = [RegOp.CheckDword(SsKey, "ProactiveDriveRetirement", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-disable-sms-auto-credential-store",
                    Label = "Storage Spaces Migration: Disable SMS Automatic Credential Storage",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets DisableCredentialStorage=1 in StorageMigrationService policy. Prevents the Storage Migration Service orchestrator from storing source server credentials in Windows Credential Manager. SMS requires credentials to access source servers for inventory and file transfer. By default these credentials are cached in Credential Manager for subsequent runs. Persistent credential storage creates a risk: an attacker who compromises the orchestrator server gains stored credentials to all migrated source servers. Disabling storage forces IT to supply credentials explicitly for each migration job.",
                    Tags = ["storage-migration", "credentials", "security", "credential-manager", "sms"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SMS does not cache source server credentials. Migration job operators must enter credentials each time they run an inventory or transfer. Prevents credential theft if the orchestrator server is compromised.",
                    ApplyOps = [RegOp.SetDword(SmsKey, "DisableCredentialStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmsKey, "DisableCredentialStorage")],
                    DetectOps = [RegOp.CheckDword(SmsKey, "DisableCredentialStorage", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-enable-pool-fault-domains",
                    Label = "Storage Spaces Migration: Enable Fault Domain Awareness in Pools",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets EnableFaultDomains=1 in StorageSpaces policy. Enables fault domain awareness when placing data in Storage Spaces pools. When fault domains are configured (chassis, rack, site), Storage Spaces places mirror data copies and parity stripes in separate fault domains. Without fault domain placement, a three-way mirror might place all three copies on drives in the same chassis — losing the chassis loses all copies. With fault domain placement, each mirror copy resides in a different chassis/rack/site, surviving physical failures that take out an entire enclosure.",
                    Tags = ["storage-spaces", "fault-domain", "resilience", "ssd", "mirror"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Data copies are placed in separate fault domains (chassis/rack). Pools without configured fault domains are unaffected. Requires Storage Spaces Direct or multi-enclosure pool configuration with defined fault domains in Windows Admin Center or PowerShell.",
                    ApplyOps = [RegOp.SetDword(SsKey, "EnableFaultDomains", 1)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "EnableFaultDomains")],
                    DetectOps = [RegOp.CheckDword(SsKey, "EnableFaultDomains", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-require-sms-encrypted-transfer",
                    Label = "Storage Spaces Migration: Require Encrypted File Transfer",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets RequireEncryptedTransfer=1 in StorageMigrationService policy. Requires the Storage Migration Service to use encrypted SMB3 connections when transferring files from source to destination servers. File migration traffic often traverses internal networks that may have insufficient network-level controls. Without encrypted transfer, an attacker with network capture capability on the migration network can read file contents as they are transferred. SMB3 encryption wraps all transfer traffic in AES-CCM, preventing interception during potentially hours-long migration windows.",
                    Tags = ["storage-migration", "encryption", "smb3", "data-protection", "transfer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "All SMS file transfers use SMB3 encryption. Source and destination servers must support SMB3 encryption (Server 2012+). Migration throughput reduced by ~15-25% due to encryption overhead; acceptable for most migrations.",
                    ApplyOps = [RegOp.SetDword(SmsKey, "RequireEncryptedTransfer", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmsKey, "RequireEncryptedTransfer")],
                    DetectOps = [RegOp.CheckDword(SmsKey, "RequireEncryptedTransfer", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-disable-pool-write-without-cache",
                    Label = "Storage Spaces Migration: Disable Pool Writes Without Cache Drive",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets RequireCacheDrive=1 in StorageSpaces policy. Prevents Storage Spaces pools from accepting writes unless a cache drive (NVMe or SSD) is available and healthy. Without a write cache, Storage Spaces Direct clusters accept writes directly to spinning disk — dramatically increasing write latency and reducing IOps. In environments where performance targets depend on the write cache, silently running without a cache (e.g., after a cache drive fails and is removed) can cause application-level performance degradation that is difficult to diagnose. This policy makes the cache absence immediately apparent.",
                    Tags = ["storage-spaces", "cache", "write-cache", "performance", "s2d"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Pool writes are rejected if no cache drive is healthy. Loss of a cache drive in a Storage Spaces Direct cluster causes writes to pause until a replacement cache drive is added. Requires monitoring and rapid cache drive replacement SLA.",
                    ApplyOps = [RegOp.SetDword(SsKey, "RequireCacheDrive", 1)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "RequireCacheDrive")],
                    DetectOps = [RegOp.CheckDword(SsKey, "RequireCacheDrive", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-enable-sms-audit-log",
                    Label = "Storage Spaces Migration: Enable Storage Migration Service Audit Log",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets AuditLogEnabled=1 in StorageMigrationService policy. Enables audit logging for both the Storage Migration Service orchestrator and proxy agents. Audit log entries record: who initiated a migration job, what source servers were inventoried, which files were transferred, when cutover was performed, and which security groups were migrated. Without an audit trail, compliance requirements (SOC 2, ISO 27001) for data migration events cannot be satisfied. Log entries are written to the SMS event channel and optionally forwarded to a SIEM.",
                    Tags = ["storage-migration", "audit", "logging", "compliance", "sms"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SMS records detailed audit events during inventory, transfer, and cutover operations. Events written to Application and Services Logs\\Microsoft\\Windows\\StorageMigrationService. Minimal performance impact.",
                    ApplyOps = [RegOp.SetDword(SmsKey, "AuditLogEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmsKey, "AuditLogEnabled")],
                    DetectOps = [RegOp.CheckDword(SmsKey, "AuditLogEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "ssmig-set-rebuild-priority-high",
                    Label = "Storage Spaces Migration: Set Pool Rebuild IO Priority to High",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets RebuildPriority=2 in StorageSpaces policy (2 = High). Sets the IO priority for Storage Spaces pool rebuild operations (resync after drive replacement) to High. By default, rebuild runs at Low priority to minimise impact on running workloads — but on a pool that has lost a drive, remaining data is exposed until rebuild completes. A two-drive failure during a slow Low-priority rebuild can cause data loss. Setting rebuild to High completes resync faster at the cost of higher IO contention, reducing the window of double-failure vulnerability.",
                    Tags = ["storage-spaces", "rebuild", "resync", "priority", "resilience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Pool resync after drive failure runs at high IO priority. Workload IO may experience higher latency during rebuild. Rebuild completes significantly faster, reducing the window of single-drive exposure. Consider scheduling high-priority rebuild during off-hours in production environments.",
                    ApplyOps = [RegOp.SetDword(SsKey, "RebuildPriority", 2)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "RebuildPriority")],
                    DetectOps = [RegOp.CheckDword(SsKey, "RebuildPriority", 2)],
                },
                new TweakDef
                {
                    Id = "ssmig-disable-pool-repair-notification",
                    Label = "Storage Spaces Migration: Disable Suppression of Pool Repair Notifications",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Sets SuppressRepairNotifications=0 in StorageSpaces policy. Ensures Action Center and Event Log notifications are generated when Storage Spaces repairs (resyncs) are running. By default, Storage Spaces emits user-visible notifications during repair operations. Some deployments suppress these notifications to avoid confusion for non-technical users. However, suppressing notifications also hides critical storage health events from IT administrators who monitor Action Center or event aggregators. This setting preserves notification delivery to ensure pool repair events are visible.",
                    Tags = ["storage-spaces", "notifications", "repair", "monitoring", "health"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Storage Spaces repair and health notifications are not suppressed. IT administrators and monitoring systems receive timely notification of pool repair events. Action Center shows storage health warnings on servers where users are not present.",
                    ApplyOps = [RegOp.SetDword(SsKey, "SuppressRepairNotifications", 0)],
                    RemoveOps = [RegOp.DeleteValue(SsKey, "SuppressRepairNotifications")],
                    DetectOps = [RegOp.CheckDword(SsKey, "SuppressRepairNotifications", 0)],
                },
            ];
    }

    // ── StorageSpacesPolicy ──
    private static class _StorageSpacesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSpaces";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sspol-disable-storage-spaces-ui",
                    Label = "Disable Storage Spaces User Interface in Settings",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Removes the Storage Spaces page from Windows Settings, preventing non-administrator users from creating or modifying storage pools and virtual disks on the system.",
                    Tags = ["storage-spaces", "storage", "settings", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Storage Spaces Settings page removed; users cannot create or manage storage pools via Settings.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStorageSpacesUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageSpacesUI")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStorageSpacesUI", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-block-pool-creation",
                    Label = "Block Non-Admin Storage Pool Creation",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Prevents non-administrator accounts from creating new Storage Spaces pools, ensuring that RAID-like virtual disk configurations can only be created by administrators.",
                    Tags = ["storage-spaces", "storage-pool", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Storage pool creation restricted to administrators; standard users blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNonAdminPoolCreation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNonAdminPoolCreation")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNonAdminPoolCreation", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-disable-tiered-storage",
                    Label = "Disable Storage Spaces Automatic Tiering",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Disables automatic data movement between storage tiers (SSD vs HDD) in tiered Storage Spaces configurations, preventing the background tiering engine from consuming I/O bandwidth.",
                    Tags = ["storage-spaces", "tiering", "ssd", "hdd", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto-tiering disabled; hot/cold data migration between SSD and HDD tiers stopped.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticTiering", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticTiering")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticTiering", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-disable-pool-retirement-notification",
                    Label = "Disable Storage Pool Retirement Drive Notifications",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Suppresses the system tray notification that appears when a drive in a storage pool nears retirement threshold, preventing non-technical users from acting on storage health warnings they cannot address.",
                    Tags = ["storage-spaces", "notifications", "drive-health", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Pool drive retirement notifications suppressed; degraded pool drives go unnoticed by users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRetirementNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRetirementNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRetirementNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-require-admin-for-pool-deletion",
                    Label = "Require Admin Rights to Delete Storage Pools",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Requires administrator privileges to delete a Storage Spaces pool or remove virtual disks, preventing accidental or malicious destruction of RAID-protected storage.",
                    Tags = ["storage-spaces", "admin", "pool-deletion", "destructive", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Storage pool deletion requires admin; standard users cannot destroy storage pools.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForPoolDeletion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForPoolDeletion")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForPoolDeletion", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-disable-storage-sense-dedup",
                    Label = "Disable Storage Sense Storage Spaces Deduplication",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Disables deduplication passes run by Storage Sense on Storage Spaces volumes, halting background dedup processing that can spike I/O during business hours.",
                    Tags = ["storage-spaces", "storage-sense", "deduplication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Storage Sense dedup on Storage Spaces disabled; dedup jobs no longer run automatically.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStorageSenseDedup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStorageSenseDedup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStorageSenseDedup", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-block-spaces-over-usb",
                    Label = "Block Storage Spaces Pool Creation over USB Drives",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Prevents USB-connected drives from being included in a Storage Spaces pool, restricting Storage Spaces to internally connected drives (SATA, NVMe, SAS) where connectivity is more reliable.",
                    Tags = ["storage-spaces", "usb", "pool", "reliability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "USB drives excluded from Storage Spaces pools; only internal drives permitted for pool members.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUSBStorageInPool", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUSBStorageInPool")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUSBStorageInPool", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-enable-pool-health-audit",
                    Label = "Enable Storage Pool Health Event Logging",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Enables detailed event logging for storage pool health changes including drive failures, degraded parity, and rebuild completions, providing audit trail for storage infrastructure.",
                    Tags = ["storage-spaces", "health", "audit-log", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Pool health events logged; drive failures, rebuilds, and degraded states recorded in event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnablePoolHealthAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnablePoolHealthAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnablePoolHealthAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "sspol-disable-spaces-auto-rebuild",
                    Label = "Disable Storage Spaces Automatic Rebuild on Drive Detection",
                    Category = "Storage — Storage Sense",
                    Description =
                        "Prevents Storage Spaces from automatically beginning a pool rebuild when a replacement drive is detected, requiring an administrator to initiate the rebuild manually for controlled recovery.",
                    Tags = ["storage-spaces", "rebuild", "auto-detect", "admin-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto-rebuild disabled; admin must manually initiate rebuild after adding a replacement drive.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoRebuildOnDriveDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoRebuildOnDriveDetection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoRebuildOnDriveDetection", 1)],
                },
            ];
    }

    // ── SyncCenterPolicy ──
    private static class _SyncCenterPolicy
    {
        private const string SyncMgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SyncMgr";
        private const string OfflineKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OfflineFiles";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "syncctr-disable-sync-center",
                Label = "Disable Sync Center",
                Category = "Storage — Storage Sense",
                Description = "Prevents users from using Windows Sync Center to synchronize files with network share partnerships.",
                Tags = ["sync-center", "offline-files", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables Sync Center UI and background sync. Offline file access on synced shares stops working.",
                ApplyOps = [RegOp.SetDword(SyncMgrKey, "DisableSyncMgr", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgrKey, "DisableSyncMgr")],
                DetectOps = [RegOp.CheckDword(SyncMgrKey, "DisableSyncMgr", 1)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-setup-wizard",
                Label = "Disable Sync Center Setup Wizard",
                Category = "Storage — Storage Sense",
                Description = "Prevents the Offline Files setup wizard from running, blocking new sync partnership creation.",
                Tags = ["sync-center", "wizard", "offline-files", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks wizard-based setup; existing partnerships are unaffected.",
                ApplyOps = [RegOp.SetDword(SyncMgrKey, "DisableSyncScheduleCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgrKey, "DisableSyncScheduleCreation")],
                DetectOps = [RegOp.CheckDword(SyncMgrKey, "DisableSyncScheduleCreation", 1)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-offline-files",
                Label = "Disable Offline Files Feature",
                Category = "Storage — Storage Sense",
                Description = "Turns off the Offline Files feature entirely; files cannot be made available offline.",
                Tags = ["offline-files", "sync-center", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Fully disables Offline Files. Impacts roaming users who rely on network files when disconnected.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-user-configuration",
                Label = "Prevent Users from Configuring Offline Files",
                Category = "Storage — Storage Sense",
                Description = "Removes the ability for users to change Offline Files settings through the Control Panel.",
                Tags = ["offline-files", "user-config", "policy", "lockdown"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Admins retain control; users cannot disable or configure offline files themselves.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "NoConfigChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "NoConfigChanges")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "NoConfigChanges", 1)],
            },
            new TweakDef
            {
                Id = "syncctr-remove-folder-from-offline",
                Label = "Disable 'Make Available Offline' Context Menu Option",
                Category = "Storage — Storage Sense",
                Description = "Hides the 'Make Available Offline' option from the right-click context menu for network files.",
                Tags = ["offline-files", "context-menu", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "UI-only restriction; users cannot initiate offline caching via right-click.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "NoMakeAvailableOffline", 1)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "NoMakeAvailableOffline")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "NoMakeAvailableOffline", 1)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-slow-link-mode",
                Label = "Disable Slow-Link Mode for Offline Files",
                Category = "Storage — Storage Sense",
                Description = "Prevents Windows from automatically switching to offline mode on slow network connections.",
                Tags = ["offline-files", "slow-link", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Forces online mode even on slow links; may degrade performance but prevents unintended offline switching.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "SlowLinkEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "SlowLinkEnabled")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "SlowLinkEnabled", 0)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-background-sync",
                Label = "Disable Background Synchronisation of Offline Files",
                Category = "Storage — Storage Sense",
                Description = "Prevents Offline Files from running background sync jobs when the user is logged on.",
                Tags = ["offline-files", "background-sync", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "No background sync; saves bandwidth and CPU. Manual sync still works.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "BackgroundSyncEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "BackgroundSyncEnabled")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "BackgroundSyncEnabled", 0)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-logon-sync",
                Label = "Disable Logon Synchronisation of Offline Files",
                Category = "Storage — Storage Sense",
                Description = "Prevents Offline Files from performing a sync when the user logs on.",
                Tags = ["offline-files", "logon", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Faster logons; offline files may be stale until explicitly synced.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "SyncAtLogon", 0)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "SyncAtLogon")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "SyncAtLogon", 0)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-logoff-sync",
                Label = "Disable Logoff Synchronisation of Offline Files",
                Category = "Storage — Storage Sense",
                Description = "Prevents Offline Files from performing a sync when the user logs off.",
                Tags = ["offline-files", "logoff", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Faster logoffs; changes made offline will not be pushed back automatically on logoff.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "SyncAtLogoff", 0)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "SyncAtLogoff")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "SyncAtLogoff", 0)],
            },
            new TweakDef
            {
                Id = "syncctr-disable-reminders",
                Label = "Disable Offline Files Sync Reminder Notifications",
                Category = "Storage — Storage Sense",
                Description = "Suppresses balloon-tip reminders about Offline Files synchronisation status.",
                Tags = ["offline-files", "notifications", "reminders", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Reduces notification noise. Users will not be reminded to sync.",
                ApplyOps = [RegOp.SetDword(OfflineKey, "GoOfflineAction", 1)],
                RemoveOps = [RegOp.DeleteValue(OfflineKey, "GoOfflineAction")],
                DetectOps = [RegOp.CheckDword(OfflineKey, "GoOfflineAction", 1)],
            },
        ];
    }

    // ── VolumeShadowCopyPolicy ──
    private static class _VolumeShadowCopyPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VolumeShadowCopy";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vscpol-disable-vss",
                Label = "Disable Volume Shadow Copy Service",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "The Volume Shadow Copy Service creates point-in-time snapshots of volumes for backup and recovery purposes. Disabling VSS through policy prevents the creation of new shadow copies, which reduces disk space consumption from snapshot storage. This setting is appropriate for systems managed by third-party backup solutions that do not rely on VSS for consistency. Environments using Backup Exec, Veeam, or similar backup products may have their own snapshot mechanisms. Disabling VSS does not remove existing shadow copies but prevents new ones from being created after policy application. Administrators should ensure alternative backup coverage exists before applying this policy to production systems.",
                Tags = ["vss", "shadow-copy", "backup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVolumeShadowCopy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVolumeShadowCopy")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVolumeShadowCopy", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-zero-max-shadow-copies",
                Label = "Set Maximum Shadow Copies to Zero",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The maximum shadow copies setting limits how many shadow copies can accumulate on a volume before older ones are purged. Setting this to zero removes any policy-defined ceiling, defaulting to the operating system's built-in limit management. This prevents policy conflicts where an explicit maximum would be smaller than what the backup software requires. Backup applications managing their own snapshot lifecycle benefit from having no policy-imposed ceiling. The operating system continues to enforce its own resource-based limits regardless of this policy value. This setting should be coordinated with backup solution requirements to avoid unintended snapshot deletion.",
                Tags = ["vss", "shadow-copy", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxShadowCopies", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxShadowCopies")],
                DetectOps = [RegOp.CheckDword(Key, "MaxShadowCopies", 0)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-notifications",
                Label = "Disable Shadow Copy Notifications",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Volume Shadow Copy generates user-facing notifications about shadow copy creation, deletion, and storage space consumption. These notifications are rarely actionable by end users and create unnecessary interruptions to productivity. Disabling VSS notifications suppresses these system tray and action center alerts. Enterprise users do not need to be aware of shadow copy operations managed by the backup infrastructure. Silencing these notifications reduces cognitive overhead and support requests from confused users. All shadow copy operations continue normally in the background without any impact from suppressing the notifications.",
                Tags = ["vss", "notifications", "ui", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyNotifications", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-on-network-shares",
                Label = "Disable Shadow Copy on Network Shares",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Shadow copies of network shares create snapshots of remote file server content visible to users as previous versions in Windows Explorer. Disabling shadow copies on network shares prevents client systems from creating or caching snapshot metadata for mapped drives. File server shadow copies should be managed exclusively at the server level rather than by client machines. Network administrators maintaining file server shadow copies through centralized policies benefit from excluding client-side network share snapshots. Disabling this feature reduces network traffic associated with shadow copy metadata enumeration over SMB connections. Previous versions functionality on network shares is unaffected when shadow copies are managed centrally at the server.",
                Tags = ["vss", "shadow-copy", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyOnNetworkShares", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyOnNetworkShares")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyOnNetworkShares", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-diffarea-growth",
                Label = "Disable Shadow Copy Diff Area Growth",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "The shadow copy diff area stores the changed block data that makes shadow copies valid snapshots in time. Allowing the diff area to grow without bounds can exhaust disk space on busy volumes with high write rates. Disabling unrestricted diff area growth enforces storage constraints that protect the system drive from being filled by shadow copy data. Servers with high-frequency write workloads such as database transaction logs can quickly exhaust diff area space. Administrators should configure explicit diff area size limits appropriate to the volume's change rate rather than allowing unbounded growth. This setting protects system stability at the cost of potentially invalidating shadow copies when storage limits are insufficient.",
                Tags = ["vss", "shadow-copy", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDiffAreaGrowth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiffAreaGrowth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiffAreaGrowth", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-schedule",
                Label = "Disable Shadow Copy Schedule",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows can automatically create shadow copies on a schedule to provide regular recovery points for users. Disabling the shadow copy schedule prevents automatic periodic snapshot creation, reducing uncontrolled storage consumption. Enterprise backup solutions that create VSS snapshots during their own backup windows do not need the Windows scheduler to create additional snapshots. Scheduled shadow copies consume I/O resources during their creation window, which can impact application performance. Centralizing snapshot scheduling within the backup management console gives administrators precise control over snapshot timing and retention. Disabling the built-in schedule while maintaining enterprise backup schedules provides better overall resource management.",
                Tags = ["vss", "shadow-copy", "scheduling", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopySchedule", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopySchedule")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopySchedule", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-telemetry",
                Label = "Disable Shadow Copy Telemetry",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Volume Shadow Copy Service telemetry reports usage statistics and diagnostic information about shadow copy operations to Microsoft. This includes data about snapshot creation frequency, failure rates, and storage utilization. Disabling VSS telemetry prevents this operational data from being transmitted outside the enterprise network boundary. Organizations in regulated industries with data residency requirements benefit from eliminating telemetry streams. VSS functionality and shadow copy quality are not affected by disabling telemetry reporting. Administrators requiring VSS usage metrics can obtain this data through Windows Performance Monitor and event log analysis.",
                Tags = ["vss", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-previous-versions",
                Label = "Disable Previous Versions Feature",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "The Previous Versions feature exposes shadow copy snapshots to users through Windows Explorer's file properties dialog. Disabling Previous Versions removes this user-facing snapshot browsing capability, reducing the risk of unauthorized data recovery by end users. Organizations using enterprise backup solutions for data recovery workflows benefit from directing all restore requests through IT-managed channels. Preventing users from directly restoring files from shadow copies enforces proper change management and audit trail requirements. The underlying shadow copies are not deleted when Previous Versions is disabled; only the user interface for browsing them is suppressed. Administrators can still access shadow copies through administrative tools and backup consoles.",
                Tags = ["vss", "previous-versions", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePreviousVersions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePreviousVersions")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePreviousVersions", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-system-protection",
                Label = "Disable System Protection Shadow Copies",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "System Protection uses VSS to create automatic restore points before system configuration changes such as driver installations and Windows updates. Disabling system protection shadow copies prevents automatic restore point creation, freeing disk space consumed by these snapshots. Enterprise environments using enterprise backup solutions and change management processes have alternative recovery mechanisms. Automatic restore points can accumulate significant storage space on busy systems that receive frequent updates. Organizations with immutable infrastructure approaches that rebuild rather than restore systems benefit from disabling automatic restore points. Prior to disabling this feature, administrators should confirm that adequate recovery mechanisms exist through enterprise backup infrastructure.",
                Tags = ["vss", "system-protection", "restore-points", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSystemProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSystemProtection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSystemProtection", 1)],
            },
            new TweakDef
            {
                Id = "vscpol-disable-on-removable",
                Label = "Disable Shadow Copy on Removable Drives",
                Category = "Storage — Storage Sense",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Volume Shadow Copy can be configured to create shadow copies on removable storage media connected to the system. Shadow copies on removable drives consume the limited storage of USB drives and external hard disks. Disabling shadow copies on removable drives prevents VSS from creating snapshots on these transient storage devices. Data on removable drives is typically managed through endpoint DLP policies rather than shadow copy mechanisms. Creating shadow copies on removable media can delay write operations and reduce performance for removable storage workflows. This setting is appropriate for all enterprise environments where removable storage is controlled through USB restriction policies.",
                Tags = ["vss", "removable", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyOnRemovable", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyOnRemovable")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyOnRemovable", 1)],
            },
        ];
    }

    // ── WorkFoldersPolicy ──
    private static class _WorkFoldersPolicy
    {
        private const string WfLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkFolders";
        private const string WfCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WorkFolders";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wf-disable-work-folders",
                Label = "Disable Work Folders Sync (Machine)",
                Category = "Storage — Work Folders",
                Description =
                    "Sets SyncDisabled=1 in the machine-side Work Folders policy key. "
                    + "Prevents Work Folders sync from running for all users on this machine, "
                    + "blocking the sync client from connecting to corporate Work Folders servers. "
                    + "Default: absent (sync allowed). Recommended: 1 on machines where cloud sync must be fully controlled.",
                Tags = ["work-folders", "sync", "policy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All Work Folders sync disabled machine-wide; existing synced content remains but is not updated.",
                ApplyOps = [RegOp.SetDword(WfLm, "SyncDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "SyncDisabled")],
                DetectOps = [RegOp.CheckDword(WfLm, "SyncDisabled", 1)],
            },
            new TweakDef
            {
                Id = "wf-disable-work-folders-user",
                Label = "Disable Work Folders Sync (Current User)",
                Category = "Storage — Work Folders",
                Description =
                    "Sets SyncDisabled=1 in the per-user Work Folders policy key. "
                    + "Prevents Work Folders sync for the current user account without a machine-wide restriction. "
                    + "Default: absent. Recommended: 1 for individual user profiles on shared machines.",
                Tags = ["work-folders", "sync", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Work Folders sync disabled for the current user only; other users on the machine are unaffected.",
                ApplyOps = [RegOp.SetDword(WfCu, "SyncDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WfCu, "SyncDisabled")],
                DetectOps = [RegOp.CheckDword(WfCu, "SyncDisabled", 1)],
            },
            new TweakDef
            {
                Id = "wf-force-automatic-setup",
                Label = "Force Work Folders Setup Automatically",
                Category = "Storage — Work Folders",
                Description =
                    "Sets AutoProvision=1 in the machine Work Folders policy key. "
                    + "Forces Work Folders to be set up automatically using a server URL configured via MDM or GP, "
                    + "without requiring the user to manually configure the sync connection. "
                    + "Default: absent (manual setup). Recommended: 1 in enterprise deployments using MDM provisioning.",
                Tags = ["work-folders", "auto-provision", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Automatically provisions Work Folders on first logon if a ServerUrl is configured.",
                ApplyOps = [RegOp.SetDword(WfLm, "AutoProvision", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "AutoProvision")],
                DetectOps = [RegOp.CheckDword(WfLm, "AutoProvision", 1)],
            },
            new TweakDef
            {
                Id = "wf-block-server-url-change",
                Label = "Prevent Users Changing Work Folders Server URL",
                Category = "Storage — Work Folders",
                Description =
                    "Sets UserServerAddrLocked=1 in the machine Work Folders policy key. "
                    + "Locks the Work Folders server address, preventing users from reconfiguring or redirecting "
                    + "their sync client to a different server. Useful for enforcing corporate sync server usage. "
                    + "Default: absent. Recommended: 1 in managed environments with a designated Work Folders server.",
                Tags = ["work-folders", "server-url", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Users cannot change the Work Folders server URL; admin-configured URL is enforced.",
                ApplyOps = [RegOp.SetDword(WfLm, "UserServerAddrLocked", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "UserServerAddrLocked")],
                DetectOps = [RegOp.CheckDword(WfLm, "UserServerAddrLocked", 1)],
            },
            new TweakDef
            {
                Id = "wf-require-encryption",
                Label = "Require Work Folders Content Encryption",
                Category = "Storage — Work Folders",
                Description =
                    "Sets LocalFolderEncryptionEnabled=1 in the machine Work Folders policy key. "
                    + "Requires that all locally synced Work Folders content be encrypted at rest "
                    + "using EFS (Encrypting File System). Protects sensitive data in case of device theft. "
                    + "Default: absent. Recommended: 1 on portable devices (laptops) with sensitive data.",
                Tags = ["work-folders", "encryption", "efs", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Work Folders content encrypted with EFS; requires user profile EFS certificate and may slow file access.",
                ApplyOps = [RegOp.SetDword(WfLm, "LocalFolderEncryptionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "LocalFolderEncryptionEnabled")],
                DetectOps = [RegOp.CheckDword(WfLm, "LocalFolderEncryptionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wf-disable-work-folders-ui",
                Label = "Hide Work Folders from Navigation Pane",
                Category = "Storage — Work Folders",
                Description =
                    "Sets ExplorerNavigationPaneHideWorkFolders=1 in the machine Work Folders policy key. "
                    + "Removes Work Folders entry from the File Explorer navigation pane, "
                    + "preventing users from browsing or accessing the sync folder via Explorer's left panel. "
                    + "Default: absent. Recommended: 1 when Work Folders is deployed but the UI should not be visible.",
                Tags = ["work-folders", "explorer", "navigation", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Hides Work Folders from Explorer navigation; the sync folder still exists on disk.",
                ApplyOps = [RegOp.SetDword(WfLm, "ExplorerNavigationPaneHideWorkFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "ExplorerNavigationPaneHideWorkFolders")],
                DetectOps = [RegOp.CheckDword(WfLm, "ExplorerNavigationPaneHideWorkFolders", 1)],
            },
            new TweakDef
            {
                Id = "wf-prevent-use-work-folders",
                Label = "Prevent Users from Configuring Work Folders",
                Category = "Storage — Work Folders",
                Description =
                    "Sets PreventWorkFolderFromUse=1 in the machine Work Folders policy key. "
                    + "Blocks users from setting up or enrolling in Work Folders from PC Settings or Explorer. "
                    + "Differs from SyncDisabled in that it prevents initial setup rather than halting an existing sync. "
                    + "Default: absent. Recommended: 1 on machines where Work Folders must not be used.",
                Tags = ["work-folders", "prevent", "setup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks users from enrolling in Work Folders; does not affect already-synced folders.",
                ApplyOps = [RegOp.SetDword(WfLm, "PreventWorkFolderFromUse", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "PreventWorkFolderFromUse")],
                DetectOps = [RegOp.CheckDword(WfLm, "PreventWorkFolderFromUse", 1)],
            },
            new TweakDef
            {
                Id = "wf-prevent-change-sync-settings",
                Label = "Prevent Users Changing Work Folders Sync Settings",
                Category = "Storage — Work Folders",
                Description =
                    "Sets SyncSettingsLocked=1 in the per-user Work Folders policy key. "
                    + "Locks Work Folders sync settings for the current user, preventing changes to "
                    + "sync frequency, bandwidth usage, and folder location from the Settings UI. "
                    + "Default: absent. Recommended: 1 in managed deployments with standardised sync policies.",
                Tags = ["work-folders", "sync-settings", "lock", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "User cannot change Work Folders sync settings; current settings remain in effect.",
                ApplyOps = [RegOp.SetDword(WfCu, "SyncSettingsLocked", 1)],
                RemoveOps = [RegOp.DeleteValue(WfCu, "SyncSettingsLocked")],
                DetectOps = [RegOp.CheckDword(WfCu, "SyncSettingsLocked", 1)],
            },
            new TweakDef
            {
                Id = "wf-disable-background-sync",
                Label = "Disable Work Folders Background Sync",
                Category = "Storage — Work Folders",
                Description =
                    "Sets BackgroundSyncDisabled=1 in the machine Work Folders policy key. "
                    + "Prevents Work Folders from syncing in the background while the user is away, "
                    + "reducing network traffic and battery usage on mobile devices. "
                    + "Default: absent (background sync enabled). Recommended: 1 on metered connections or battery-sensitive devices.",
                Tags = ["work-folders", "background-sync", "battery", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Work Folders only syncs on user demand; background sync bandwidth and battery usage eliminated.",
                ApplyOps = [RegOp.SetDword(WfLm, "BackgroundSyncDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "BackgroundSyncDisabled")],
                DetectOps = [RegOp.CheckDword(WfLm, "BackgroundSyncDisabled", 1)],
            },
            new TweakDef
            {
                Id = "wf-set-sync-interval",
                Label = "Set Work Folders Minimum Sync Interval to 15 Minutes",
                Category = "Storage — Work Folders",
                Description =
                    "Sets MinSyncInterval=15 in the machine Work Folders policy key. "
                    + "Configures the minimum time between automatic sync cycles to 15 minutes, "
                    + "reducing sync frequency to lower network utilisation on busy or metered connections. "
                    + "Default: absent (OS default ~1 minute). Recommended: 15 on bandwidth-constrained networks.",
                Tags = ["work-folders", "sync-interval", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sync runs at most every 15 minutes; reduces background network traffic on metered links.",
                ApplyOps = [RegOp.SetDword(WfLm, "MinSyncInterval", 15)],
                RemoveOps = [RegOp.DeleteValue(WfLm, "MinSyncInterval")],
                DetectOps = [RegOp.CheckDword(WfLm, "MinSyncInterval", 15)],
            },
        ];
    }
}
