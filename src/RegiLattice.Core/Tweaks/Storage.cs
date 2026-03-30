namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Storage
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [

        new TweakDef
        {
            Id = "stor-storage-disable-reserved",
            Label = "Disable Reserved Storage",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the 7 GB reserved storage partition that Windows keeps for updates and temp files. Frees disk space on small drives. Takes effect after the next feature update. Default: enabled. Recommended: disabled on space-constrained devices.",
            Tags = ["storage", "reserved", "disk", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-storage-sense",
            Label = "Disable Storage Sense Auto-Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Storage Sense, the automatic disk cleanup feature that deletes temp files and Recycle Bin content on a schedule. Prevents unintended file removal. Default: enabled. Recommended: disabled if you manage cleanup manually.",
            Tags = ["storage", "cleanup", "storage-sense", "automatic"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-recycle-confirm",
            Label = "Disable Recycle Bin Confirmation Dialog",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the confirmation prompt when deleting files to the Recycle Bin. Files still go to the Recycle Bin and can be restored. Default: enabled. Recommended: disabled for faster workflow.",
            Tags = ["storage", "recycle-bin", "confirmation", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-thumbs-db",
            Label = "Disable Thumbs.db on Network Folders",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from creating hidden Thumbs.db thumbnail cache files in network folders. Avoids lock conflicts and clutter on shared drives. Default: enabled (Thumbs.db created). Recommended: disabled.",
            Tags = ["storage", "thumbs", "network", "cache", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1),
            ],
        },
        new TweakDef
        {
            Id = "stor-storage-compact-os",
            Label = "Enable Compact OS Compression Flag",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Compact OS registry flag to prefer OS file compression. Can save 1-2 GB on the system drive. For full effect run 'compact /compactos:always' from an elevated prompt. Default: disabled. Recommended: enabled on small SSDs.",
            Tags = ["storage", "compact", "compression", "disk", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "CompactOsEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "CompactOsEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "CompactOsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-prefetch",
            Label = "Disable Prefetch and Superfetch",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Prefetch and Superfetch (SysMain) caching mechanisms. On SSD systems these provide negligible benefit and consume disk I/O. Default: enabled (3). Recommended: disabled on SSD-only machines.",
            Tags = ["storage", "prefetch", "superfetch", "sysmain", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "stor-storage-optimize-ntfs-memory",
            Label = "NTFS Memory Usage High",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets NtfsMemoryUsage to 2 (high), allowing NTFS to use more paged pool memory for caching. Improves file system performance on machines with ample RAM. Default: 1 (normal). Recommended: 2 on workstations with 16 GB+ RAM.",
            Tags = ["storage", "ntfs", "memory", "performance", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-8dot3",
            Label = "Disable 8.3 Short Filename Creation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic creation of legacy 8.3 short filenames on NTFS volumes. Improves directory enumeration speed on volumes with many files. Default: enabled (0). Recommended: disabled unless legacy 16-bit apps are needed.",
            Tags = ["storage", "ntfs", "8dot3", "short-name", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
        },

        new TweakDef
        {
            Id = "stor-storage-enable-long-paths",
            Label = "Enable Win32 Long Path Support",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Win32 long path support, removing the 260-character path length limit for applications that declare long-path awareness in their manifest. Default: disabled. Recommended: enabled for developers and deep directory trees.",
            Tags = ["storage", "long-path", "260", "developer", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 1)],
        },

        new TweakDef
        {
            Id = "stor-disable-storage-sense",
            Label = "Disable Storage Sense (Quick)",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic Storage Sense cleanup. Default: Enabled. Recommended: Disabled for manual control.",
            Tags = ["storage", "storage-sense", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-reserved-storage",
            Label = "Disable Reserved Storage (Quick)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows reserved storage (~7 GB). Default: Enabled. Recommended: Disabled to reclaim space.",
            Tags = ["storage", "reserved", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
        },
        new TweakDef
        {
            Id = "stor-enable-trim",
            Label = "Enable TRIM for SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables TRIM command for SSDs by setting DisableDeleteNotification to 0. Improves SSD longevity and performance. Default: Enabled. Recommended: Enabled.",
            Tags = ["storage", "ssd", "trim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "DisableDeleteNotification", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "DisableDeleteNotification", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "DisableDeleteNotification", 0)],
        },

        new TweakDef
        {
            Id = "stor-disable-last-access",
            Label = "Disable Last Access Time Stamp Updates",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NTFS last access time stamp updates to reduce disk I/O. Default: Enabled. Recommended: Disabled for SSDs.",
            Tags = ["storage", "ntfs", "last-access", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem",
                    "NtfsDisableLastAccessUpdate",
                    unchecked((int)0x80000001)
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem",
                    "NtfsDisableLastAccessUpdate",
                    unchecked((int)0x80000002)
                ),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 0)],
        },
        new TweakDef
        {
            Id = "stor-enable-storage-sense",
            Label = "Enable Storage Sense Auto-Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Storage Sense to automatically clean up temp files, recycle bin, and downloads. Default: disabled.",
            Tags = ["storage", "sense", "cleanup", "auto"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
        },
        new TweakDef
        {
            Id = "stor-set-recycle-bin-max-10pct",
            Label = "Set Recycle Bin Max Size to 10%",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Limits the Recycle Bin to take no more than 10%% of drive space. Default: 10%% (but OEMs may set differently).",
            Tags = ["storage", "recycle-bin", "size", "limit"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket\Volume"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "NukeOnDelete", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "NukeOnDelete")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "NukeOnDelete", 0)],
        },

        new TweakDef
        {
            Id = "stor-disable-delivery-optimization-cache",
            Label = "Disable Delivery Optimization Cache",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Delivery Optimization to HTTP only (no LAN or Internet sharing). Reduces disk usage. Default: LAN.",
            Tags = ["storage", "delivery-optimization", "cache", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "stor-compact-os",
            Label = "Enable Compact OS Compression",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows system file compression (CompactOS). Saves ~2 GB on the OS drive. Default: varies by install.",
            Tags = ["storage", "compact", "compression", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEnableCompression", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEnableCompression")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEnableCompression", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-last-access",
            Label = "Disable NTFS Last Access Timestamp",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the NTFS last access timestamp update. Reduces disk I/O on every file read, beneficial for SSDs. Default: system managed.",
            Tags = ["storage", "ntfs", "last-access", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "stor-disable-disk-quotas",
            Label = "Disable Disk Quotas Enforcement",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS disk quota enforcement. Frees I/O overhead from quota tracking on every write operation.",
            Tags = ["storage", "ntfs", "quotas", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota", "Enable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota", "Enable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota", "Enable", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-volume-shadow-schedule",
            Label = "Disable Volume Shadow Copy Schedule",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables scheduled Volume Shadow Copy snapshots. Frees disk space used by shadow copies. System Restore still works on demand.",
            Tags = ["storage", "shadow-copy", "disk-space", "vss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DisableSR", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DisableSR", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "stor-disable-low-disk-space-warning",
            Label = "Disable Low Disk Space Warning",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the low disk space notification balloon. Useful for small partitions where the warning is a nuisance.",
            Tags = ["storage", "notifications", "disk-space", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
            ],
        },
        new TweakDef
        {
            Id = "stor-enable-write-cache-flush",
            Label = "Disable Write Cache Buffer Flushing",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables write cache buffer flushing on disks. Improves write performance but increases risk of data loss on power failure.",
            Tags = ["storage", "write-cache", "performance", "risk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\IDE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCacheFlush", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCacheFlush", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCacheFlush", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-thumbnail-cache-cleanup",
            Label = "Disable Thumbnail Cache Auto-Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from periodically clearing the thumbnail cache, avoiding slow folder icon generation on re-open.",
            Tags = ["storage", "thumbnails", "cache", "explorer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache", "Autorun", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache", "Autorun"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache", "Autorun", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-remote-diff-compression",
            Label = "Disable Remote Differential Compression",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Remote Differential Compression API feature used for network file transfers. Saves memory and CPU on standalone machines.",
            Tags = ["storage", "network", "rdc", "features"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC", "DisableMSRDC", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC", "DisableMSRDC")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC", "DisableMSRDC", 1)],
        },
        new TweakDef
        {
            Id = "stor-set-recycle-bin-max-5pct",
            Label = "Limit Recycle Bin to 5% of Drive",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Limits the Recycle Bin to consume at most 5% of the drive. Default is 10%. Recovers disk space on large drives.",
            Tags = ["storage", "recycle-bin", "disk-space", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket\Volume"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "MaxCapacity", 5)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "MaxCapacity", 10)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket", "MaxCapacity", 5)],
        },
        new TweakDef
        {
            Id = "stor-disable-windows-error-reporting-dump",
            Label = "Disable Error Reporting Dump Collection",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Error Reporting crash dump collection. Prevents multi-GB dump files accumulating in %LOCALAPPDATA%\\CrashDumps.",
            Tags = ["storage", "error-reporting", "dump", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "stor-disable-search-index-backoff",
            Label = "Disable Search Index I/O Backoff",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Search indexer I/O backoff logic. Completes indexing faster but uses more disk I/O during the process.",
            Tags = ["storage", "search", "indexing", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-offline-files-cache",
            Label = "Disable Offline Files Cache",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Offline Files (CSC) cache feature. Frees disk space and reduces background I/O from offline file syncing.",
            Tags = ["storage", "offline-files", "cache", "csc"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-wins-reserved-storage",
            Label = "Disable Windows Reserved Storage (Policy)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Reserved Storage that automatically sets aside ~7 GB for system updates. Frees space on small drives. Default: reserved storage enabled.",
            Tags = ["storage", "reserved", "disk-space", "windows-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReservedStorage"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReservedStorage", "ShippedWithReserves", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReservedStorage", "ShippedWithReserves", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReservedStorage", "ShippedWithReserves", 0)],
        },
        new TweakDef
        {
            Id = "stor-set-storage-sense-cadence-monthly",
            Label = "Set Storage Sense Cadence: Monthly",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Changes Storage Sense to only run once a month instead of on low-disk events. Reduces background cleanup interruptions. Default: run when low on disk.",
            Tags = ["storage", "storage-sense", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 30),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 30),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-storage-sense-cloud-clean",
            Label = "Disable Storage Sense Cloud File Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Storage Sense from moving locally available cloud files (OneDrive) to online-only. Keeps files accessible offline. Default: cloud cleanup enabled.",
            Tags = ["storage", "storage-sense", "onedrive", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "32", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "32"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "32", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-set-delivery-opt-limit",
            Label = "Limit Delivery Optimisation Download Bandwidth",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits Delivery Optimisation download bandwidth to 25% of available connection. Prevents Windows Updates from saturating the network. Default: unlimited.",
            Tags = ["storage", "delivery-optimisation", "bandwidth", "windows-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-volume-shadow-auto",
            Label = "Disable Automatic VSS Shadow Copies",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic creation of Volume Shadow Copy (VSS) snapshots. Frees disk space on low-capacity drives. Default: auto-creation on.",
            Tags = ["storage", "vss", "shadow-copy", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig", 1)],
        },
        new TweakDef
        {
            Id = "stor-disable-ntfs-tunnel-cache",
            Label = "Disable NTFS Filename Tunnel Cache",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables NTFS filename tunneling — a legacy feature that preserves old filenames when files are deleted and re-created. Reduces NTFS metadata overhead. Default: enabled.",
            Tags = ["storage", "ntfs", "filesystem", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-disk-performance-counter",
            Label = "Disable Disk Performance Counter",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the disk I/O performance counter tracking. Slightly reduces overhead from disk monitoring. Default: enabled when active monitoring tools exist.",
            Tags = ["storage", "performance", "counter", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Disk"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Disk", "EnableDiskIoPerfTag", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Disk", "EnableDiskIoPerfTag")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Disk", "EnableDiskIoPerfTag", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-storage-pool-auto-balance",
            Label = "Disable Storage Pool Auto-Balance",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic data rebalancing in Storage Spaces pools. Eliminates background I/O bursts from pool rebalancing. Default: auto-balance enabled.",
            Tags = ["storage", "storage-spaces", "pool", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageManagement"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageManagement", "AutoPoolBalance", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageManagement", "AutoPoolBalance")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageManagement", "AutoPoolBalance", 0)],
        },
        new TweakDef
        {
            Id = "stor-set-recycle-bin-pct-policy",
            Label = "Set Recycle Bin Size via Policy (10%)",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Limits the Recycle Bin maximum size to 10% of drive capacity. Prevents deleted files from silently consuming excessive disk space. Default: drive-based default (varies).",
            Tags = ["storage", "recycle-bin", "disk-space"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "RecycleBinDrivePercent", 10),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "RecycleBinDrivePercent"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "RecycleBinDrivePercent", 10),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-storage-sense-downloads-clean",
            Label = "Disable Storage Sense Downloads Folder Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Storage Sense from automatically deleting old files in the Downloads folder. Default: deletes downloads not opened in 30+ days.",
            Tags = ["storage", "storage-sense", "downloads", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "08", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "08"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "08", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-storage-sense-full",
            Label = "Disable Storage Sense Entirely (Full)",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Storage Sense feature which automatically frees disk space. Takes full manual control of storage cleanup. Default: Storage Sense enabled.",
            Tags = ["storage", "storage-sense", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-enable-cloud-files-cleanup",
            Label = "Enable Storage Sense Cloud File Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures Storage Sense to dehydrate locally cached OneDrive/cloud files not accessed in 30+ days. Frees local disk without deleting files. Default: cloud cleanup disabled.",
            Tags = ["storage", "storage-sense", "cloud", "onedrive", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "02", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "02", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "02", 1),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-recyclebin-auto-clean",
            Label = "Disable Storage Sense Recycle Bin Auto-Clean",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Storage Sense from emptying the Recycle Bin automatically. Items remain in the bin until manually deleted. Default: files older than 30 days are auto-deleted.",
            Tags = ["storage", "storage-sense", "recycle-bin"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "04", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "04", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "04", 0),
            ],
        },
        new TweakDef
        {
            Id = "stor-set-storage-sense-monthly",
            Label = "Set Storage Sense to Run Monthly",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Changes Storage Sense cadence from 'on low disk space' to monthly. Ensures periodic cleanup without triggering on temporary disk pressure. Default: run on low space (0).",
            Tags = ["storage", "storage-sense", "schedule"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "2048", 30),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "2048", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "2048", 30),
            ],
        },
        new TweakDef
        {
            Id = "stor-disable-link-tracking-service",
            Label = "Disable Distributed Link Tracking Client Service",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Distributed Link Tracking Client (TrkWks) service. Stops Windows from tracking moved/renamed NTFS files across volumes via object identifiers. Default: automatic start.",
            Tags = ["storage", "ntfs", "services", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
        },
        new TweakDef
        {
            Id = "stor-disable-distributed-link-tracking",
            Label = "Disable NTFS Distributed Link Tracking",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents NTFS from creating and maintaining object identifiers used by link tracking. Reduces background disk I/O from link maintenance. Default: link tracking enabled.",
            Tags = ["storage", "ntfs", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLinkTracking", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLinkTracking", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLinkTracking", 1)],
        },
        new TweakDef
        {
            Id = "stor-disable-vss-autostart",
            Label = "Set Volume Shadow Copy Service to Manual",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Changes the Volume Shadow Copy Service (VSS) from automatic to manual start. VSS starts on demand when backups or restore points are triggered. Default: automatic.",
            Tags = ["storage", "vss", "shadow-copy", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
        },
        new TweakDef
        {
            Id = "stor-disable-softprovider-svc",
            Label = "Set Software Shadow Copy Provider to Manual",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Microsoft Software Shadow Copy Provider (swprv) service to manual start. The service launches automatically when VSS requests a software-based shadow copy. Default: manual (confirmed).",
            Tags = ["storage", "vss", "shadow-copy", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\swprv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\swprv", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\swprv", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\swprv", "Start", 3)],
        },
        new TweakDef
        {
            Id = "stor-set-recyclebin-threshold-30days",
            Label = "Set Recycle Bin Auto-Clean Threshold to 30 Days",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures Storage Sense to only auto-empty Recycle Bin items older than 30 days. Prevents immediate auto-deletion while still reclaiming space over time. Default: 30 days (if recycle bin cleanup is enabled).",
            Tags = ["storage", "storage-sense", "recycle-bin", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "512", 30),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "512"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "512", 30),
            ],
        },
        new TweakDef
        {
            Id = "stor-set-downloads-no-auto-clean",
            Label = "Disable Storage Sense Downloads Auto-Clean",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures Storage Sense to never auto-delete files in the Downloads folder based on age (threshold=0). Complements the per-flag disable for full control. Default: 30-day threshold.",
            Tags = ["storage", "storage-sense", "downloads", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "1024", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "1024"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "1024", 0),
            ],
        },
    ];
}
