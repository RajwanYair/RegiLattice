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

// === Merged from: FileSystem.cs ===


internal static class FileSystem
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fs-disable-encryption-warning",
            Label = "Disable EFS Encryption Warning",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Encrypting File System (EFS) configuration warning dialog. Prevents prompts when EFS is not configured or not in use. Default: 0 (warning enabled). Recommended: disabled on machines not using EFS.",
            Tags = ["filesystem", "efs", "encryption", "warning", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 1)],
        },

        new TweakDef
        {
            Id = "fs-disable-remote-diff-compression",
            Label = "Disable Remote Differential Compression",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Remote Differential Compression (MSRDC) feature used during remote file sync. Reduces CPU overhead when RDC is unnecessary in local or fast-network environments. Default: 0 (enabled). Recommended: disabled on fast LANs or when RDC is not needed.",
            Tags = ["filesystem", "rdc", "remote", "compression", "sync", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC\Parameters", "DisableMSDC", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC\Parameters", "DisableMSDC", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC\Parameters", "DisableMSDC", 1)],
        },
        new TweakDef
        {
            Id = "fs-enable-dedup-memory",
            Label = "Set Higher Dedup Memory Usage",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the data deduplication service MaxMemory to 2048 MB for faster processing. Allows the dedup engine to use more RAM during optimization passes. Default: not set (engine default). Recommended: 2048 on servers with 16 GB+ RAM.",
            Tags = ["filesystem", "dedup", "deduplication", "memory", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dedup\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dedup\Parameters", "MaxMemory", 2048)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dedup\Parameters", "MaxMemory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dedup\Parameters", "MaxMemory", 2048)],
        },

        new TweakDef
        {
            Id = "fs-enable-case-sensitive",
            Label = "Enable Per-Directory Case Sensitivity",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the global flag allowing per-directory NTFS case sensitivity. Required for WSL interop and POSIX-compliant directory behavior on Windows. Default: 1 (case-insensitive). Recommended: 0 (case-sensitive) for WSL/developer use.",
            Tags = ["filesystem", "case-sensitive", "ntfs", "wsl", "posix", "developer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "ObCaseInsensitive", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "ObCaseInsensitive", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "ObCaseInsensitive", 0)],
        },
        new TweakDef
        {
            Id = "fs-disable-name-generation",
            Label = "Disable Extended Character 8.3 Name Generation",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic 8.3 short name generation for extended (Unicode) characters. Reduces NTFS overhead when creating files with non-ASCII names. Default: 1 (enabled). Recommended: disabled unless legacy 16-bit app compatibility is needed.",
            Tags = ["filesystem", "8dot3", "short-name", "unicode", "ntfs", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0),
            ],
        },
        new TweakDef
        {
            Id = "fs-increase-mft-zone",
            Label = "Increase NTFS MFT Zone Reservation",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Increases the NTFS Master File Table zone reservation from level 1 to level 2. Reserves more contiguous disk space for MFT growth, reducing fragmentation on busy volumes. Default: 1. Recommended: 2 for volumes with many small files.",
            Tags = ["filesystem", "mft", "ntfs", "fragmentation", "reservation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 2)],
        },
        new TweakDef
        {
            Id = "fs-disable-encrypt-paging",
            Label = "Disable Paging File Encryption",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables NTFS paging file encryption to reduce CPU overhead during memory paging. Improves paging performance at the cost of not encrypting swapped data at rest. Default: 0 (disabled). Recommended: keep disabled unless full-disk encryption is required.",
            Tags = ["filesystem", "paging", "encryption", "swap", "performance", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
        },
        new TweakDef
        {
            Id = "fs-disable-dos-devices",
            Label = "Disable DOS Device Mapping Protection",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Session Manager DOS device mapping protection mode. Allows legacy applications to create global DOS device names without restrictions. Default: 1 (protection enabled). Recommended: disabled only for legacy app compatibility.",
            Tags = ["filesystem", "dos", "device", "session-manager", "legacy", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "ProtectionMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "ProtectionMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "ProtectionMode", 0)],
        },
        new TweakDef
        {
            Id = "fs-set-additional-del-margin",
            Label = "Set Critical Disk Allocation Margin",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets an additional 1 MB reserved byte margin for critical NTFS disk allocations. Prevents low-disk-space write failures for system-critical operations. Default: not set (0). Recommended: 1048576 (1 MB) on volumes that approach capacity.",
            Tags = ["filesystem", "ntfs", "disk-space", "reserved", "allocation", "margin"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAdditionallyReservedBytes", 1048576)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAdditionallyReservedBytes")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAdditionallyReservedBytes", 1048576),
            ],
        },
        new TweakDef
        {
            Id = "fs-disable-azure-indexing",
            Label = "Disable Azure AD Cloud Content Indexing",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Azure AD / Entra ID cloud content indexing via Windows Search policy. Prevents cloud-sourced content from being indexed locally, reducing network and I/O usage. Default: not set (cloud search allowed). Recommended: disabled for privacy-focused setups.",
            Tags = ["filesystem", "azure", "cloud", "indexing", "search", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "fs-disable-8dot3-names",
            Label = "Disable 8.3 Short Filename Creation",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic 8.3 (DOS-compatible) short filename generation on NTFS. Reduces directory enumeration overhead and speeds up file creation. Default: 0 (enabled). Recommended: 1 (disabled).",
            Tags = ["filesystem", "ntfs", "8dot3", "performance", "filenames"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
        },
        new TweakDef
        {
            Id = "fs-increase-ntfs-memory",
            Label = "Increase NTFS Memory Usage",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets NtfsMemoryUsage to 2 (maximum) to allocate more paged pool for NTFS operations. Improves performance on file-heavy workloads. Default: 1. Recommended: 2 on systems with >=16 GB RAM.",
            Tags = ["filesystem", "ntfs", "memory", "performance", "paged-pool"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "fs-disable-tunneling",
            Label = "Disable NTFS Filename Tunneling",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables NTFS filename tunneling by setting MaximumTunnelEntries to 0. Prevents DOS-era filename compatibility caching. Default: 256. Recommended: 0 on modern systems.",
            Tags = ["filesystem", "ntfs", "tunneling", "performance", "legacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries", 0)],
        },
        new TweakDef
        {
            Id = "fs-enable-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the large system file cache (LargeSystemCache=1). Optimizes memory for file server workloads at the cost of app memory. Default: 0 (desktop). Recommended: 1 for NAS/file server roles.",
            Tags = ["filesystem", "cache", "memory", "server", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1),
            ],
        },
        new TweakDef
        {
            Id = "fs-disable-last-access-timestamp",
            Label = "Disable Last Access Timestamps",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables updating last-access timestamps on NTFS. Reduces disk writes and improves performance. Default: user managed.",
            Tags = ["filesystem", "ntfs", "last-access", "performance"],
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
                    unchecked((int)0x80000000)
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem",
                    "NtfsDisableLastAccessUpdate",
                    unchecked((int)0x80000001)
                ),
            ],
        },
        new TweakDef
        {
            Id = "fs-enable-ntfs-encryption",
            Label = "Enable NTFS Encryption Warnings",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables a warning when moving encrypted files to an unencrypted location. Prevents accidental decryption. Default: no warning.",
            Tags = ["filesystem", "ntfs", "encryption", "warning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoEncryptOnMove", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoEncryptOnMove")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoEncryptOnMove", 0)],
        },
        new TweakDef
        {
            Id = "fs-increase-ntfs-memory-usage",
            Label = "Increase NTFS Paged Pool Memory",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the NTFS paged pool memory allocation. Improves performance with many open files. Default: system managed.",
            Tags = ["filesystem", "ntfs", "memory", "paged-pool"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "fs-disable-delete-confirmation",
            Label = "Disable Delete Confirmation Dialog",
            Category = "File System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 'Are you sure you want to move this to Recycle Bin?' confirmation. Default: enabled.",
            Tags = ["filesystem", "delete", "confirmation", "dialog"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 0)],
        },
        new TweakDef
        {
            Id = "fs-enable-path-based-case-sensitivity",
            Label = "Enable Per-Directory Case Sensitivity",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables support for per-directory case sensitivity on NTFS via Windows Subsystem for Linux. Default: disabled.",
            Tags = ["filesystem", "case-sensitivity", "ntfs", "wsl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEnableDirCaseSensitivity", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEnableDirCaseSensitivity")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEnableDirCaseSensitivity", 1)],
        },
        new TweakDef
        {
            Id = "fs-disable-last-access-update",
            Label = "Disable Last Access Time Stamp",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables updating the last access time stamp on NTFS files. Reduces write operations and improves file system performance. Default: enabled.",
            Tags = ["filesystem", "last-access", "ntfs", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "fs-set-additional-critical-worker-threads",
            Label = "Increase Critical Worker Threads",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds additional critical worker threads for the file system. Improves I/O throughput on multi-core systems with many concurrent operations.",
            Tags = ["filesystem", "performance", "threads", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive",
                    "AdditionalCriticalWorkerThreads",
                    16
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive",
                    "AdditionalCriticalWorkerThreads"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive",
                    "AdditionalCriticalWorkerThreads",
                    16
                ),
            ],
        },
        new TweakDef
        {
            Id = "fs-set-additional-delayed-worker-threads",
            Label = "Increase Delayed Worker Threads",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adds additional delayed worker threads for background file system operations. Reduces queuing delays under heavy I/O.",
            Tags = ["filesystem", "performance", "threads", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive",
                    "AdditionalDelayedWorkerThreads",
                    16
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive", "AdditionalDelayedWorkerThreads"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive",
                    "AdditionalDelayedWorkerThreads",
                    16
                ),
            ],
        },
        new TweakDef
        {
            Id = "fs-disable-notification-change",
            Label = "Disable NTFS Change Notifications",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NTFS change notification tracking. Reduces kernel overhead from file watchers (may break live-reload tools).",
            Tags = ["filesystem", "ntfs", "notifications", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableChangeJournal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableChangeJournal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableChangeJournal", 1)],
        },
        new TweakDef
        {
            Id = "fs-optimize-path-cache",
            Label = "Increase File Path Cache Size",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the path cache size to speed up directory traversals, especially on deep file hierarchies like node_modules.",
            Tags = ["filesystem", "cache", "performance", "traversal"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "PathCache", 128)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "PathCache")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "PathCache", 128)],
        },
        new TweakDef
        {
            Id = "fs-enable-opportunistic-locking",
            Label = "Enable Opportunistic File Locking",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures opportunistic locking (oplock) is enabled for file I/O. Improves read/write performance for network and local files.",
            Tags = ["filesystem", "oplock", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EnableOplocks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EnableOplocks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EnableOplocks", 1)],
        },
        new TweakDef
        {
            Id = "fs-disable-ntfs-tunneling",
            Label = "Disable NTFS File Name Tunneling",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables NTFS file name tunneling. Tunneling preserves short/long name associations across rename/delete cycles. Disabling frees kernel memory.",
            Tags = ["filesystem", "ntfs", "tunneling", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "MaximumTunnelEntries", 0)],
        },

        new TweakDef
        {
            Id = "fs-enable-win32-long-paths-policy",
            Label = "Enable Long Paths via Group Policy",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables long path support (>260 chars) via the Group Policy registry key. Complements the manifest-level opt-in for Win32 applications.",
            Tags = ["filesystem", "long-paths", "policy", "developer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "fs-disable-transacted-installer-rollback",
            Label = "Disable Transactional NTFS Rollback",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Transactional NTFS (TxF) rollback log creation. Saves disk space and I/O for this rarely-used feature.",
            Tags = ["filesystem", "ntfs", "txf", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableTxfLog", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableTxfLog")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableTxfLog", 1)],
        },
        new TweakDef
        {
            Id = "fs-increase-file-handle-limit",
            Label = "Increase System File Handle Limit",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the system-wide file handle limit. Prevents 'too many open files' errors for applications with heavy file I/O.",
            Tags = ["filesystem", "handles", "limits", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Subsystems"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "ObjectDirectories", 16384)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "ObjectDirectories")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "ObjectDirectories", 16384)],
        },
        new TweakDef
        {
            Id = "fs-disable-admin-shares",
            Label = "Disable Administrative Shares (C$, ADMIN$)",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets AutoShareWks=0 so Windows does not automatically create hidden administrative shares (C$, D$, ADMIN$) on workstations. Reduces attack surface for lateral-movement over SMB.",
            Tags = ["filesystem", "smb", "shares", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "fs-disable-smb1-server",
            Label = "Disable SMB 1.0 Server Protocol",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SMB1=0 in LanmanServer parameters to disable the legacy SMB 1.0/CIFS protocol on the server side. SMBv1 is vulnerable to EternalBlue and similar exploits.",
            Tags = ["filesystem", "smb", "smb1", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 0)],
        },
        new TweakDef
        {
            Id = "fs-disable-file-history",
            Label = "Disable File History Backup Service (Policy)",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the FileHistory GPO policy Disabled=1. Prevents the File History background backup service from continuously monitoring and copying files, reducing drive writes on SSDs.",
            Tags = ["filesystem", "file-history", "backup", "writes", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "fs-disable-prev-versions-ui",
            Label = "Disable Previous Versions UI in File Explorer",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLocalPage=1 to remove the Previous Versions tab from file properties. Avoids shadow-copy enumeration overhead when browsing file properties.",
            Tags = ["filesystem", "previous-versions", "shadow-copy", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PreviousVersions"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage", 1)],
        },
        new TweakDef
        {
            Id = "fs-disable-dfs-client",
            Label = "Disable DFS Client Name Resolution",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets DfsDisable=1 to prevent the DFS client from attempting Distributed File System namespace resolution. Eliminates DFS-related DNS and SMB round-trips on workstations not joined to Active Directory.",
            Tags = ["filesystem", "dfs", "smb", "ad", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DFS", "DfsDisable", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DFS", "DfsDisable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DFS", "DfsDisable", 1)],
        },
        new TweakDef
        {
            Id = "fs-enable-extended-chars-8dot3",
            Label = "Allow Extended Characters in 8.3 Short Names",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NtfsAllowExtendedCharacterIn8dot3Name=1. Enables Unicode/extended character support in NTFS 8.3 short names, improving compatibility with legacy applications that read short names.",
            Tags = ["filesystem", "ntfs", "8dot3", "unicode", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 1),
            ],
        },
        new TweakDef
        {
            Id = "fs-set-smb-auto-disconnect",
            Label = "Set SMB Server Idle Disconnect Timeout to 5 Min",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AutoDisconnect=5 (minutes) in LanmanServer parameters. Reclaims SMB session resources 5 minutes after a client goes idle, freeing server handles and memory.",
            Tags = ["filesystem", "smb", "server", "connections", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoDisconnect", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoDisconnect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoDisconnect", 5)],
        },
        new TweakDef
        {
            Id = "fs-set-oplock-break-timeout",
            Label = "Set Oplock Break ACK Timeout to 35 s",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets OplockBreakTimeout to 35 seconds in LanmanWorkstation parameters. The client will wait up to 35 s for the server to acknowledge an opportunistic lock break before timing out the request.",
            Tags = ["filesystem", "smb", "oplock", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "OplockBreakTimeout", 35),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "OplockBreakTimeout"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "OplockBreakTimeout", 35),
            ],
        },
        new TweakDef
        {
            Id = "fs-disable-ntfs-compression-global",
            Label = "Disable NTFS Compression System-Wide",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NtfsCompressionDisabled=1. Prevents NTFS from applying per-file or per-directory compression attributes. Eliminates CPU cost from on-the-fly decompression reads on SSDs where storage is not a bottleneck.",
            Tags = ["filesystem", "ntfs", "compression", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsCompressionDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsCompressionDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsCompressionDisabled", 1)],
        },
        new TweakDef
        {
            Id = "fs-disable-low-disk-check",
            Label = "Disable Low Disk Space Balloon Warning",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLowDiskSpaceChecks=1 via Explorer policy. Suppresses the periodic low disk space balloon notification that appears in the system tray.",
            Tags = ["filesystem", "disk", "notification", "explorer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
            ],
        },
        new TweakDef
        {
            Id = "fs-disable-autorun-gpo",
            Label = "Disable AutoRun on All Drives (Policy)",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoAutorun=1 via Explorer policy to disable AutoRun on all drive types. Prevents automatic execution of content on USB drives, optical media, and external HDDs.",
            Tags = ["filesystem", "autorun", "security", "usb", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutorun", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutorun")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutorun", 1)],
        },
    ];
}
