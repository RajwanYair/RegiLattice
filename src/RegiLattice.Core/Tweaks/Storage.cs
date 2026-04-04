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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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

// ── merged from SsdOptimization.cs ────────────────────────────────────────
internal static class SsdOptimization
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ssd-disable-superfetch",
            Label = "Disable Superfetch / SysMain on SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service which pre-loads apps into memory. Unnecessary on SSDs and reduces write wear.",
            Tags = ["ssd", "performance", "superfetch", "sysmain"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "ssd-disable-prefetch",
            Label = "Disable Prefetch on SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Prefetch feature. On SSDs, random read is fast enough that prefetching provides no benefit.",
            Tags = ["ssd", "performance", "prefetch"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ssd-disable-last-access-timestamp",
            Label = "Disable Last Access Timestamp (NTFS)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables NTFS last-access-time updates, reducing unnecessary SSD writes on every file read.",
            Tags = ["ssd", "performance", "ntfs", "filesystem"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disablelastaccess", "1"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disablelastaccess", "0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["behavior", "query", "disablelastaccess"]);
                return stdout.Contains("1", StringComparison.OrdinalIgnoreCase) || stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-enable-trim",
            Label = "Enable TRIM (Automatic Optimization)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Ensures TRIM is enabled for SSD garbage collection. TRIM informs the SSD which blocks are no longer in use.",
            Tags = ["ssd", "performance", "trim", "defrag"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disabledeletenotify", "0"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disabledeletenotify", "1"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["behavior", "query", "disabledeletenotify"]);
                return stdout.Contains("= 0", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "ssd-disable-defrag-schedule",
            Label = "Disable Scheduled Disk Defragmentation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables the scheduled defragmentation task. SSDs should use TRIM, not defragmentation.",
            Tags = ["ssd", "performance", "defrag", "scheduled-task"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-disable-windows-search-indexing",
            Label = "Disable Windows Search Indexing",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Search Indexer service. Reduces write amplification on SSDs. Search will still work but without instant results.",
            Tags = ["ssd", "performance", "indexing", "search"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "ssd-enable-write-caching",
            Label = "Enable Write Caching on SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables disk write caching for improved SSD write performance. Data is cached in volatile memory before being written to disk.",
            Tags = ["ssd", "performance", "write-cache", "disk"],
            SideEffects = "Risk of data loss on sudden power failure without UPS.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-Disk | Where-Object { $_.BusType -ne 'USB' } | ForEach-Object { "
                        + "Set-StorageSetting -NewDiskPolicy OnlineAll -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ssd-disable-hibernation-ssd",
            Label = "Disable Hibernation (SSD Wear Reduction)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables hibernation to avoid writing full RAM contents to SSD (hiberfil.sys). Reduces write wear and frees disk space equal to RAM size.",
            Tags = ["ssd", "performance", "hibernation", "disk-space"],
            SideEffects = "Hibernation and Fast Startup will be unavailable.",
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "off"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "on"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("Test-Path \"$env:SystemDrive\\hiberfil.sys\"");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-disable-8dot3-names",
            Label = "Disable 8.3 Short File Names (NTFS)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables legacy 8.3 short filename generation in NTFS. Reduces overhead on every file creation.",
            Tags = ["ssd", "performance", "ntfs", "filesystem"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["8dot3name", "set", "1"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["8dot3name", "set", "0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["8dot3name", "query"]);
                return stdout.Contains("1", StringComparison.Ordinal) || stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-increase-ntfs-memory-usage",
            Label = "Increase NTFS Memory Usage (Paged Pool)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases NTFS paged pool memory usage for better file system performance. Trades RAM for reduced disk I/O.",
            Tags = ["ssd", "performance", "ntfs", "memory"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "ssd-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Windows to use a large system cache, improving file system performance at the cost of higher memory usage.",
            Tags = ["ssd", "performance", "cache", "memory"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-boot-trace",
            Label = "Disable Boot Trace Logging",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables boot trace diagnostic logging that writes to disk during startup.",
            Tags = ["ssd", "performance", "boot", "trace", "io"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot", "Start", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot", "Start", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot", "Start", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-ntfs-compression",
            Label = "Disable NTFS Compression (System Drive)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS compression on the system drive. Compression adds CPU overhead and provides minimal benefit on fast SSDs.",
            Tags = ["ssd", "performance", "ntfs", "compression", "cpu"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableCompression", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableCompression")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableCompression", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-ntfs-encryption",
            Label = "Disable NTFS Encryption Paging",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS encrypted paging file. Reduces I/O overhead from encrypting page file writes.",
            Tags = ["ssd", "performance", "ntfs", "encryption", "paging"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
        },
        new TweakDef
        {
            Id = "ssd-set-io-priority-normal",
            Label = "Set Default I/O Priority to Normal",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default I/O priority for all processes to Normal. Prevents I/O starvation from low-priority background tasks.",
            Tags = ["ssd", "performance", "io-priority", "scheduling"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl", "ConvertibleSlateMode", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl", "ConvertibleSlateMode")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl", "ConvertibleSlateMode", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-low-disk-check",
            Label = "Disable Low Disk Space Check",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the periodic low disk space notification check. Reduces unnecessary disk I/O.",
            Tags = ["ssd", "performance", "notification", "disk-space"],
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
            Id = "ssd-increase-ntfs-mft-zone",
            Label = "Increase NTFS MFT Zone Reservation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the NTFS Master File Table zone reservation to reduce MFT fragmentation on SSDs with many small files.",
            Tags = ["ssd", "performance", "ntfs", "mft", "fragmentation"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 2)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 2)],
        },
        new TweakDef
        {
            Id = "ssd-disable-readyboost",
            Label = "Disable ReadyBoost Service",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the ReadyBoost service which uses USB flash drives as cache. Useless on systems with SSDs.",
            Tags = ["ssd", "performance", "readyboost", "service", "usb"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "ssd-disable-disk-perf-counters",
            Label = "Disable Disk Performance Counters",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables disk performance counters to reduce I/O overhead. Disable only if you don't monitor disk performance.",
            Tags = ["ssd", "performance", "counters", "io", "monitoring"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk", "Start", 4)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "ssd-disable-link-power-management",
            Label = "Disable AHCI Link Power Management",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AHCI Link Power Management (HIPM/DIPM) to prevent SSD latency spikes from power state transitions.",
            Tags = ["ssd", "performance", "power", "ahci"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableHIPM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableHIPM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableHIPM", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-dipm",
            Label = "Disable Device-Initiated Power Management",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DIPM (Device-Initiated Power Management) on SATA SSDs. Prevents the SSD from entering low-power states that add latency.",
            Tags = ["ssd", "performance", "power", "dipm"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableDIPM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableDIPM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableDIPM", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-idle-power-timeout",
            Label = "Disable Disk Idle Power Timeout",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the disk idle timeout to 0, preventing SSDs from entering sleep mode. Eliminates wake-up latency spikes.",
            Tags = ["ssd", "performance", "power", "timeout"],
            RegistryKeys =
            [
                $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
                    "ValueMax",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
                    "ValueMax"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
                    "ValueMax",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ssd-increase-mft-zone-4",
            Label = "Increase MFT Zone Reservation to 4",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reserves a larger contiguous area for the Master File Table, reducing MFT fragmentation on heavily used SSDs.",
            Tags = ["ssd", "ntfs", "mft", "fragmentation"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 4)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMftZoneReservation", 4)],
        },
        new TweakDef
        {
            Id = "ssd-disable-log-file-flush",
            Label = "Reduce NTFS Log File Flush Frequency",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Reduces the frequency of NTFS journal log file flushes. Decreases write amplification on SSDs at a small risk of data on power loss.",
            Tags = ["ssd", "ntfs", "log", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLogfileFlush", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLogfileFlush")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLogfileFlush", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-pagefile-encryption",
            Label = "Disable Page File Encryption",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables page file encryption that adds CPU overhead and write amplification on SSDs. Less secure but faster swap performance.",
            Tags = ["ssd", "pagefile", "encryption", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
        },
        new TweakDef
        {
            Id = "ssd-optimize-power-scheme",
            Label = "Optimize SSD Power Scheme Settings",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the active power scheme for SSDs by disabling aggressive power saving that causes latency on modern NVMe drives.",
            Tags = ["ssd", "power", "nvme", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-timestamp-on-directories",
            Label = "Disable Timestamp Updates on Directories",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS timestamp updates on directory access. Reduces unnecessary write operations during directory traversals.",
            Tags = ["ssd", "ntfs", "timestamp", "io"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
        },
        new TweakDef
        {
            Id = "ssd-enable-volatile-write-cache",
            Label = "Enable Volatile Write Cache on SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables volatile write caching on SSD controller. Improves sequential write performance but data may be lost on sudden power loss.",
            Tags = ["ssd", "write-cache", "performance", "risk"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Enum\SCSI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCache", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCache", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCache", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-content-indexing-global",
            Label = "Disable Content Indexing via Registry",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Search content indexing globally via registry. Reduces background I/O and write wear on SSDs.",
            Tags = ["ssd", "indexing", "search", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-pagefile-clear-shutdown",
            Label = "Disable Pagefile Wipe on Shutdown",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets ClearPageFileAtShutdown=0 so Windows does not zero-fill the pagefile on every shutdown. Eliminates a large sequential write that significantly increases SSD wear-out and lengthens shutdown time.",
            Tags = ["ssd", "pagefile", "shutdown", "performance", "wear"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 0),
            ],
        },
        new TweakDef
        {
            Id = "ssd-indexer-low-priority-io",
            Label = "Run Windows Search Indexer at Low I/O Priority",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LowPriorityIO=1 in the Windows Search gatherer parameters. The indexer uses background I/O priority, reducing I/O contention with interactive applications on SSDs.",
            Tags = ["ssd", "search", "indexer", "io-priority", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex", "LowPriorityIO", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex", "LowPriorityIO")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex", "LowPriorityIO", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-boot-auto-layout",
            Label = "Disable Boot File Auto-Layout Optimisation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableAutoLayout=0. On SSDs the auto-layout rearrangement of boot files provides no latency benefit because seek time is negligible. Eliminates the post-defrag layout phase.",
            Tags = ["ssd", "boot", "auto-layout", "defrag", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout", "EnableAutoLayout", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout", "EnableAutoLayout")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout", "EnableAutoLayout", 0)],
        },
        new TweakDef
        {
            Id = "ssd-set-disk-timeout-fast",
            Label = "Reduce Disk I/O Timeout to 20 s",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the disk service TimeOutValue to 20 seconds. SSDs respond orders of magnitude faster than HDDs; the default 45 s timeout means stalled SSD requests block the queue for far too long.",
            Tags = ["ssd", "timeout", "disk", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 20)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 20)],
        },
        new TweakDef
        {
            Id = "ssd-limit-io-page-lock-pool",
            Label = "Limit I/O Page-Lock Memory Pool to 1 MB",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IoPageLockLimit to 1 048 576 bytes (1 MB). Caps the amount of physical memory that can be locked for I/O transfers, keeping more RAM available for application buffers.",
            Tags = ["ssd", "memory", "io", "pool", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit", 1048576)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit")],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit", 1048576),
            ],
        },
        new TweakDef
        {
            Id = "ssd-disable-rac-sampling",
            Label = "Disable Reliability Activity Centre Sampling",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GeneralSamplingEnabled=0 in the RAC registry key. Stops the Reliability Analysis Component from periodically sampling system activity and writing entries to the RAC database on the SSD.",
            Tags = ["ssd", "reliability", "rac", "wear", "writes"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC", "GeneralSamplingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC", "GeneralSamplingEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC", "GeneralSamplingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ssd-ntfs-force-nonpaged-pool",
            Label = "Force NTFS Metadata into Non-Paged Pool",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NtfsForceNonPagedPoolAllocation=1. Keeps NTFS internal metadata structures in non-paged pool memory, eliminating paging I/O on the SSD for the file system's own working set.",
            Tags = ["ssd", "ntfs", "memory", "paged-pool", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsForceNonPagedPoolAllocation", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsForceNonPagedPoolAllocation")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsForceNonPagedPoolAllocation", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-smb-read-ahead",
            Label = "Disable SMB Read-Ahead for SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ReadAheadThreshold=0 in LanmanWorkstation parameters. Disables SMB client read-ahead prefetching. On SSDs random I/O is as fast as sequential, so pre-reading data wastes write bandwidth and cache without benefit.",
            Tags = ["ssd", "smb", "read-ahead", "network", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "ReadAheadThreshold", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "ReadAheadThreshold")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "ReadAheadThreshold", 0)],
        },
        new TweakDef
        {
            Id = "ssd-search-no-backoff-busy",
            Label = "Disable Windows Search I/O Backoff When Disk Busy",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BackOffIfDiskBusy=0 in Windows Search. By default the indexer backs off when disk utilisation is high. On SSDs concurrent I/O is fully supported; disabling backoff lets indexing keep pace.",
            Tags = ["ssd", "search", "indexer", "io", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search", "BackOffIfDiskBusy", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows Search", "BackOffIfDiskBusy")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search", "BackOffIfDiskBusy", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-auto-volume-mount",
            Label = "Disable Automatic New-Volume Mounting",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoAutoMount=1 in the MountMgr parameters. Prevents Windows from automatically assigning drive letters and mounting new storage volumes. Avoids unexpected I/O and AutoRun triggers when USB drives are inserted.",
            Tags = ["ssd", "mount", "volume", "autorun", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr", "NoAutoMount", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr", "NoAutoMount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr", "NoAutoMount", 1)],
        },
    ];
}

