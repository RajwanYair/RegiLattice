// RegiLattice.Core — Tweaks/StorageManagementPolicy.cs
// Sprint 276: Storage Management Group Policy (10 tweaks)
// Category: "Storage Management Policy" | Slug: stormgmt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageManagement

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageManagementPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageManagement";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "stormgmt-disable-storage-spaces-ui",
            Label = "Disable Storage Spaces Configuration UI",
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 4,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
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
            Category = "Storage Management Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
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
