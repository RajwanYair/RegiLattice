// RegiLattice.Core — Tweaks/StorageSpacesPolicy.cs
// Windows Storage Spaces pool, virtual disk, and tiering policy controls — Sprint 487.
// Category: "Storage Spaces Policy" | Slug: sspol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageSpaces

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageSpacesPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSpaces";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sspol-disable-storage-spaces-ui",
                Label = "Disable Storage Spaces User Interface in Settings",
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Category = "Storage Spaces Policy",
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
                Id = "sspol-set-rebuild-priority-low",
                Label = "Set Storage Spaces Rebuild Priority to Low",
                Category = "Storage Spaces Policy",
                Description =
                    "Sets the Storage Spaces rebuild I/O priority to low, ensuring that pool resynchronisation after a drive failure occurs as a background low-priority task without impacting foreground workload I/O.",
                Tags = ["storage-spaces", "rebuild", "io-priority", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Pool rebuild priority set to low; rebuild takes longer but does not impact foreground I/O.",
                ApplyOps = [RegOp.SetDword(Key, "RebuildPriority", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "RebuildPriority")],
                DetectOps = [RegOp.CheckDword(Key, "RebuildPriority", 0)],
            },
            new TweakDef
            {
                Id = "sspol-disable-spaces-auto-rebuild",
                Label = "Disable Storage Spaces Automatic Rebuild on Drive Detection",
                Category = "Storage Spaces Policy",
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
