namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageSensePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "storsense-disable-storage-sense",
                Label = "Disable Storage Sense via Policy",
                Category = "Storage Sense Policy",
                Description =
                    "Prevents Storage Sense from running automatically, overriding any per-user Storage Sense settings. Useful in managed environments where disk cleanup is handled by separate tools.",
                Tags = ["storage sense", "cleanup", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops automatic disk cleanup; administrators must manage free space through other means.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowStorageSenseGlobal", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowStorageSenseGlobal")],
                DetectOps = [RegOp.CheckDword(Key, "AllowStorageSenseGlobal", 0)],
            },
            new TweakDef
            {
                Id = "storsense-disable-temp-file-cleanup",
                Label = "Disable Storage Sense Temporary File Cleanup",
                Category = "Storage Sense Policy",
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
                Category = "Storage Sense Policy",
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
                Category = "Storage Sense Policy",
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
                Category = "Storage Sense Policy",
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
                Id = "storsense-set-recycle-bin-30days",
                Label = "Set Storage Sense Recycle Bin Cleanup Threshold to 30 Days",
                Category = "Storage Sense Policy",
                Description =
                    "Configures Storage Sense to automatically empty Recycle Bin items that have been deleted for more than 30 days, providing consistent disk reclamation on managed devices.",
                Tags = ["storage sense", "recycle bin", "cleanup", "threshold", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Files in Recycle Bin older than 30 days are permanently deleted; warn users who rely on long-term recycle bin retention.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigStorageSenseRecycleBinCleanupThreshold", 30)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigStorageSenseRecycleBinCleanupThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigStorageSenseRecycleBinCleanupThreshold", 30)],
            },
            new TweakDef
            {
                Id = "storsense-set-downloads-cleanup-60days",
                Label = "Set Storage Sense Downloads Cleanup Threshold to 60 Days",
                Category = "Storage Sense Policy",
                Description =
                    "Configures Storage Sense to remove files from the Downloads folder that have not been opened in 60 days, helping reclaim disk space from accumulated stale downloads.",
                Tags = ["storage sense", "downloads", "cleanup", "threshold", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Downloads untouched for 60 days are deleted; users may lose files they intended to keep.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigStorageSenseDownloadsCleanupThreshold", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigStorageSenseDownloadsCleanupThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigStorageSenseDownloadsCleanupThreshold", 60)],
            },
            new TweakDef
            {
                Id = "storsense-set-cloud-dehydrate-60days",
                Label = "Set Storage Sense Cloud Dehydration Threshold to 60 Days",
                Category = "Storage Sense Policy",
                Description =
                    "Configures Storage Sense to dehydrate OneDrive files that have not been opened in 60 days, freeing local disk space while keeping files accessible via cloud sync.",
                Tags = ["storage sense", "onedrive", "cloud", "dehydration", "threshold", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Files unused for 60 days move to cloud-only; offline access requires re-download.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigStorageSenseCloudContentDehydrationThreshold", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigStorageSenseCloudContentDehydrationThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigStorageSenseCloudContentDehydrationThreshold", 60)],
            },
            new TweakDef
            {
                Id = "storsense-enforce-storage-policies",
                Label = "Enforce Storage Sense Policies on All Users",
                Category = "Storage Sense Policy",
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
            new TweakDef
            {
                Id = "storsense-set-run-cadence-weekly",
                Label = "Set Storage Sense Run Cadence to Weekly",
                Category = "Storage Sense Policy",
                Description =
                    "Configures Storage Sense to run automatically once per week, providing more frequent disk space reclamation for devices with high file turnover or limited storage.",
                Tags = ["storage sense", "cadence", "schedule", "weekly", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "More frequent cleanup than monthly; suitable for devices with limited storage capacity.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigStorageSenseGlobalCadence", 7)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigStorageSenseGlobalCadence")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigStorageSenseGlobalCadence", 7)],
            },
        ];
}
