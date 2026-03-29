// RegiLattice.Core — Tweaks/StorageBusPolicy.cs
// Storage bus (SATA, NVMe, SAS, USB storage) controller and power management policy — Sprint 491.
// Category: "Storage Bus Policy" | Slug: stobus
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageBus

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageBusPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageBus";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "stobus-disable-ahci-link-power",
                Label = "Disable AHCI SATA Link Power Management (HIPM/DIPM)",
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
                Category = "Storage Bus Policy",
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
