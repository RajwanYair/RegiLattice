// RegiLattice.Core — Tweaks/CloudStorageQuotaPolicy.cs
// Cloud Storage Quota and Usage Governance Policy — Sprint 590.
// Configures quota notifications, local cache size limits, storage
// provider restrictions, and cloud storage usage reporting.
// Category: "Cloud Storage Quota Policy" | Slug: cloudqt
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageSense

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudStorageQuotaPolicy
{
    private const string CloudContentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string StorageSenseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cloudqt-enable-storage-sense-enforcement",
                Label = "Cloud Storage Quota: Enable Storage Sense Automatic Disk Cleanup",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets AllowStorageSenseGlobal=1 in the StorageSense policy key. Enables Windows Storage Sense — the automatic disk space cleanup feature that removes temporary files, empties the recycle bin, removes locally cached cloud-only files (OneDrive Files On-Demand) that have not been used recently, and cleans up Windows Update delivery files. Storage Sense proactively prevents the disk-fill scenario that causes OS instability on devices with limited SSD storage. Without Storage Sense, devices gradually accumulate gigabytes of recoverable temporary data that is never cleaned up automatically.",
                Tags = ["storage-sense", "disk-cleanup", "temp-files", "onedrive-cache", "automatic"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Storage Sense runs automatically on a configured cadence. Recycle bin emptied, temp files removed, and unused OneDrive cache files evicted back to cloud-only status. Files evicted from OneDrive local cache will need to be re-downloaded when next accessed. Configure the cadence and thresholds using additional StorageSense policy keys.",
                ApplyOps = [RegOp.SetDword(StorageSenseKey, "AllowStorageSenseGlobal", 1)],
                RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "AllowStorageSenseGlobal")],
                DetectOps = [RegOp.CheckDword(StorageSenseKey, "AllowStorageSenseGlobal", 1)],
            },
            new TweakDef
            {
                Id = "cloudqt-set-storage-sense-cadence-monthly",
                Label = "Cloud Storage Quota: Set Storage Sense Run Cadence to Monthly",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets ConfigStorageSenseGlobalCadence=30 in the StorageSense policy key (value: days). Sets Storage Sense to automatically run once per month (every 30 days). Monthly cadence provides regular disk space management without running so frequently that it causes annoying file evictions. For 256 GB SSDs which accumulate temporary data faster, consider a 14-day cadence. For devices with large SSDs (1 TB+), monthly is appropriate. Storage Sense runs in the background during low activity periods to minimise user impact.",
                Tags = ["storage-sense", "cadence", "monthly", "disk-management", "schedule"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Storage Sense cleanup runs every 30 days. For devices consistently near disk capacity, consider 7 or 14-day cadence. Storage Sense operates silently in the background — no user notification for routine cleanup operations.",
                ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseGlobalCadence", 30)],
                RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseGlobalCadence")],
                DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseGlobalCadence", 30)],
            },
            new TweakDef
            {
                Id = "cloudqt-set-onedrive-cache-evict-days-60",
                Label = "Cloud Storage Quota: Evict Unused OneDrive Cached Files After 60 Days",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets ConfigStorageSenseCloudContentDehydrationThreshold=60 in the StorageSense policy key (value: days). When Storage Sense runs, it converts locally cached OneDrive Files On-Demand files that have not been opened in the last 60 days back to cloud-only placeholders (dehydration). This reclaims local disk space for files the user has not accessed in two months. The dehydrated files are still available in OneDrive — they simply need to be re-downloaded when next opened. 60 days balances storage efficiency against re-download inconvenience.",
                Tags = ["storage-sense", "onedrive", "dehydration", "cache-eviction", "disk-space"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive locally cached files not opened in 60+ days are evicted to cloud-only status. On next open, file is re-downloaded from OneDrive. Offline mode users (frequent travellers, users in poor-connectivity environments) should have a longer threshold or use 'Always keep on this device' for their most important files.",
                ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseCloudContentDehydrationThreshold", 60)],
                RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseCloudContentDehydrationThreshold")],
                DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseCloudContentDehydrationThreshold", 60)],
            },
            new TweakDef
            {
                Id = "cloudqt-disable-third-party-cloud-storage-promotion",
                Label = "Cloud Storage Quota: Disable Third-Party Cloud Storage Provider Promotions in Windows",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets DisableThirdPartySuggestions=1 in the CloudContent policy key. Prevents Windows from promoting or integrating third-party cloud storage providers (Dropbox, Box, Google Drive, iCloud) in Windows Explorer, Save As dialogs, and File Picker. In enterprise environments with an approved cloud storage platform (OneDrive for Business), promoting third-party alternatives creates shadow IT risk — users connecting personal Dropbox or Google Drive accounts to corporate devices create uncontrolled data sync paths outside DLP policy coverage. Disabling third-party promotions reinforces the corporate cloud storage platform strategy.",
                Tags = ["cloud-storage", "third-party", "dropbox", "shadow-it", "dlp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Third-party cloud storage provider promotions removed from Windows Explorer and Save As dialogs. Third-party sync clients (Dropbox, Box, Google Drive) already installed continue to function — this only prevents new promotions. To block the sync clients themselves, use AppLocker or Windows Defender Application Control.",
                ApplyOps = [RegOp.SetDword(CloudContentKey, "DisableThirdPartySuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContentKey, "DisableThirdPartySuggestions")],
                DetectOps = [RegOp.CheckDword(CloudContentKey, "DisableThirdPartySuggestions", 1)],
            },
            new TweakDef
            {
                Id = "cloudqt-disable-windows-consumer-cloud-features",
                Label = "Cloud Storage Quota: Disable Windows Consumer Cloud Features (Spotlight, Store Suggestions)",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets DisableWindowsConsumerFeatures=1 in the CloudContent policy key (also DisableSoftLanding=1). Disables Windows consumer-facing cloud features including Windows Spotlight ads on the lock screen, Start menu app suggestions from the Microsoft Store, Microsoft 365 family subscription promotions, and automatic installation of consumer app recommendations. These features generate unsolicited network traffic to Microsoft content delivery networks and may install apps (Family features, consumer apps) that are not appropriate in enterprise environments.",
                Tags = ["cloud-content", "spotlight", "consumer-features", "enterprise", "store-suggestions"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Windows consumer cloud features disabled. Lock screen Spotlight images replaced with static photos. Start menu suggestions and promoted apps removed. Spontaneously installed consumer apps (Candy Crush, Phone Link, etc.) are blocked. No functional impact on enterprise applications or OneDrive for Business.",
                ApplyOps = [RegOp.SetDword(CloudContentKey, "DisableWindowsConsumerFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContentKey, "DisableWindowsConsumerFeatures")],
                DetectOps = [RegOp.CheckDword(CloudContentKey, "DisableWindowsConsumerFeatures", 1)],
            },
            new TweakDef
            {
                Id = "cloudqt-enable-recycle-bin-cleanup-storage-sense",
                Label = "Cloud Storage Quota: Enable Recycle Bin Auto-Purge After 30 Days via Storage Sense",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets ConfigStorageSenseRecycleBinCleanupThreshold=30 in the StorageSense policy key. Configures Storage Sense to automatically and permanently delete files that have been in the Recycle Bin for more than 30 days when Storage Sense runs. Without this policy, deleted files remain in the Recycle Bin indefinitely until the user manually empties it — gradually consuming disk space over months or years. Auto-purging after 30 days provides reasonable 'soft delete' protection against accidental deletions while preventing indefinite accumulation of deleted data.",
                Tags = ["storage-sense", "recycle-bin", "auto-purge", "disk-cleanup", "deleted-files"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Files in Recycle Bin for more than 30 days are permanently deleted. Users who rely on the Recycle Bin as a long-term 'undo' mechanism for accidentally deleted files beyond 30 days will find those files permanently gone. Ensure users understand that deleted files are unrecoverable from the Recycle Bin after 30 days.",
                ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseRecycleBinCleanupThreshold", 30)],
                RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseRecycleBinCleanupThreshold")],
                DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseRecycleBinCleanupThreshold", 30)],
            },
            new TweakDef
            {
                Id = "cloudqt-enable-temp-files-cleanup-storage-sense",
                Label = "Cloud Storage Quota: Enable Temp Files Cleanup via Storage Sense",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets ConfigStorageSenseTempFilesCleanup=1 in the StorageSense policy key. Enables Storage Sense to delete temporary files created by apps — including Windows Temp folder contents, Downloads folder files (if configured), browser caches, Windows Update delivery files (after successful installation), and log files in %TEMP%. App-generated temp files accumulate at 1–5 GB per month on typical business devices. Routine cleanup prevents the temp file accumulation that manifests as gradual disk space degradation over months to years of use.",
                Tags = ["storage-sense", "temp-files", "cache-cleanup", "disk-space", "automatic"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Temporary files cleaned by Storage Sense. In-use temp files are not deleted — only files not currently in use by any process. Browser caches are rebuilt on demand. Windows Update delivery optimisation cache cleaned after updates are successfully applied. No impact on running applications.",
                ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseTempFilesCleanup", 1)],
                RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseTempFilesCleanup")],
                DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseTempFilesCleanup", 1)],
            },
            new TweakDef
            {
                Id = "cloudqt-disable-consumer-account-state-content",
                Label = "Cloud Storage Quota: Disable Consumer Account State Cloud Content (Upsell UI)",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets DisableConsumerAccountStateContent=1 in the CloudContent policy key. Disables the Microsoft Consumer Account State notifications in Windows settings that prompt users to link a personal Microsoft account, subscribe to Microsoft 365 Personal, purchase more OneDrive storage, or connect Xbox Game Pass. In enterprise environments, devices are managed under an Azure AD work account — consumer account upsell prompts are irrelevant, distracting, and potentially confuse users into adding non-corporate accounts to corporate devices.",
                Tags = ["cloud-content", "consumer-account", "upsell", "enterprise", "notification"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Consumer account upsell prompts and Microsoft 365 Personal subscription promote notifications disabled. No functional impact on enterprise Microsoft 365 applications or OneDrive for Business. Microsoft account linking from Settings is still possible but not promoted.",
                ApplyOps = [RegOp.SetDword(CloudContentKey, "DisableConsumerAccountStateContent", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContentKey, "DisableConsumerAccountStateContent")],
                DetectOps = [RegOp.CheckDword(CloudContentKey, "DisableConsumerAccountStateContent", 1)],
            },
            new TweakDef
            {
                Id = "cloudqt-set-downloads-folder-cleanup-days-90",
                Label = "Cloud Storage Quota: Auto-Clean Downloads Folder Files Older Than 90 Days",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets ConfigStorageSenseDownloadsCleanupThreshold=90 in the StorageSense policy key. Configures Storage Sense to purge files in the user's Downloads folder that have not been opened in the past 90 days. The Downloads folder is one of the fastest-growing storage consumers on business devices — downloaded PDF reports, EXE installers, email attachments, ZIP archives accumulate over months. After 90 days, most downloaded files have served their purpose. Auto-cleanup prevents the Downloads folder from becoming a permanent secondary storage location.",
                Tags = ["storage-sense", "downloads-folder", "cleanup", "disk-space", "90-days"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Downloaded files not opened in 90+ days are permanently deleted. Users who use the Downloads folder for long-term file storage will lose files after 90 days of inactivity. Communicate this policy to users and provide guidance on using OneDrive for long-term document storage.",
                ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseDownloadsCleanupThreshold", 90)],
                RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseDownloadsCleanupThreshold")],
                DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseDownloadsCleanupThreshold", 90)],
            },
            new TweakDef
            {
                Id = "cloudqt-disable-cloud-content-during-oobe",
                Label = "Cloud Storage Quota: Disable Cloud Content and Subscription Promotions During OOBE",
                Category = "Cloud Storage Quota Policy",
                Description =
                    "Sets DisableCloudOptimizedContent=1 in the CloudContent policy key. Prevents Windows Out-of-Box Experience (OOBE) from displaying cloud-delivered promotional content — Microsoft 365 subscription upsells, recommended apps, tailored welcome screens based on consumer signals, and first-run experience tiles sourced from Microsoft CDN. During enterprise device provisioning (Windows Autopilot, MDM enrolment), cloud-optimised content creates deployment inconsistency and may delay the provisioning flow by waiting for network-delivered content. A clean, policy-defined OOBE without cloud content ensures predictable provisioning.",
                Tags = ["cloud-content", "oobe", "provisioning", "autopilot", "enterprise-deployment"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Cloud-delivered OOBE promotional content disabled. OOBE uses static configured content only. Windows Autopilot and MDM enrollment flows are unaffected functionally. First-run welcome screens show default Windows UI rather than personalized or promoted content.",
                ApplyOps = [RegOp.SetDword(CloudContentKey, "DisableCloudOptimizedContent", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContentKey, "DisableCloudOptimizedContent")],
                DetectOps = [RegOp.CheckDword(CloudContentKey, "DisableCloudOptimizedContent", 1)],
            },
        ];
}
