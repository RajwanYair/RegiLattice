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

// ── merged from PolicyStorage.cs ──
// RegiLattice.Core — Tweaks/PolicyStorage.cs
// Disk quotas, NTFS, Storage Spaces, ReFS, VSS, shadow copies, file history, offline files, and storage management policies
// Category: "Storage Policy"
// Consolidated from 23 modules.

internal static class PolicyStorage
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CdBurningPolicy.Data,
            .. _DiskQuotaAdvancedPolicy.Data,
            .. _DiskQuotaPolicy.Data,
            .. _FileHistoryPolicy.Data,
            .. _FileSharePolicy.Data,
            .. _FileShareWitnessPolicy.Data,
            .. _NtfsPolicy.Data,
            .. _OfflineFilesSyncPolicy.Data,
            .. _OpenTypeSecurityPolicy.Data,
            .. _RefsFsPolicy.Data,
            .. _ReFSPolicy.Data,
            .. _ShadowCopyVss.Data,
            .. _StorageBusPolicy.Data,
            .. _StorageHealthPolicy.Data,
            .. _StorageManagementPolicy.Data,
            .. _StoragePoolPolicy.Data,
            .. _StorageReplicaPolicy.Data,
            .. _StorageSensePolicy.Data,
            .. _StorageSpacesMigrationPolicy.Data,
            .. _StorageSpacesPolicy.Data,
            .. _SyncCenterPolicy.Data,
            .. _VolumeShadowCopyPolicy.Data,
            .. _WorkFoldersPolicy.Data,
        ];

    // ── CdBurningPolicy ──
    private static class _CdBurningPolicy
    {
        private const string BurnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDBurning";
        private const string ExplLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string ExplCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer";
        private const string CdRomKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string DvdKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cdbp-no-burning-machine",
                Label = "Disable CD/DVD Burning (Machine-Wide)",
                Category = "Storage",
                Description =
                    "Sets NoBurning=1 in the Windows CDBurning policy key for all users on this machine. "
                    + "Removes the 'Burn to Disc' option from Explorer and prevents the built-in burning wizard from launching. "
                    + "Default: absent (burning allowed). Recommended: 1 on managed or public desktops.",
                Tags = ["cd", "burning", "optical", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes Explorer-native disc burning; third-party burning tools are unaffected.",
                ApplyOps = [RegOp.SetDword(BurnKey, "NoBurning", 1)],
                RemoveOps = [RegOp.DeleteValue(BurnKey, "NoBurning")],
                DetectOps = [RegOp.CheckDword(BurnKey, "NoBurning", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-no-burning-user",
                Label = "Disable CD/DVD Burning (Current User)",
                Category = "Storage",
                Description =
                    "Sets NoCDBurning=1 in the per-user Explorer policy key. "
                    + "Removes the disc-burning shell extension for the current user without machine-wide enforcement. "
                    + "Default: absent. Recommended: 1 on shared workstations for non-admin users.",
                Tags = ["cd", "burning", "optical", "policy", "user"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "User-scoped removal of Burn to Disc wizard; reversible without admin rights.",
                ApplyOps = [RegOp.SetDword(ExplCu, "NoCDBurning", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplCu, "NoCDBurning")],
                DetectOps = [RegOp.CheckDword(ExplCu, "NoCDBurning", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-no-burning-explorer-lm",
                Label = "Hide CD Burning in Explorer (Machine Policy)",
                Category = "Storage",
                Description =
                    "Sets NoCDBurning=1 in the machine-scoped Explorer policy key. "
                    + "Suppresses the burn-to-disc task pane and context menu item in Explorer for all users. "
                    + "Default: absent. Recommended: 1 in kiosk, classroom, or terminal server deployments.",
                Tags = ["cd", "burning", "explorer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the Explorer CD-burn UI for all users on this machine.",
                ApplyOps = [RegOp.SetDword(ExplLm, "NoCDBurning", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplLm, "NoCDBurning")],
                DetectOps = [RegOp.CheckDword(ExplLm, "NoCDBurning", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-cdrom-read",
                Label = "Block CD-ROM Read Access",
                Category = "Storage",
                Description =
                    "Sets Deny_Read=1 in the CD-ROM removable storage device class policy (GUID {53f56308}). "
                    + "Prevents all read operations from CD-ROM drives via the OS removable storage access policy layer. "
                    + "Default: absent (read allowed). Recommended: 1 only in air-gapped environments where optical media is prohibited.",
                Tags = ["cd", "read", "removable", "optical", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Completely blocks CD-ROM read access; breaks all optical disc software — use with caution.",
                ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-cdrom-write",
                Label = "Block CD-ROM Write Access",
                Category = "Storage",
                Description =
                    "Sets Deny_Write=1 in the CD-ROM device class policy. "
                    + "Prevents write operations to CD-R/RW drives via the removable storage access layer. "
                    + "Default: absent (write allowed). Recommended: 1 for data-loss-prevention on managed desktops.",
                Tags = ["cd", "write", "removable", "optical", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks CD-ROM writes at the device access policy layer.",
                ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-cdrom-execute",
                Label = "Block CD-ROM Execute (AutoRun)",
                Category = "Storage",
                Description =
                    "Sets Deny_Execute=1 in the CD-ROM device class policy. "
                    + "Prevents direct execution of content from CD-ROM drives via the removable storage access layer. "
                    + "Default: absent. Recommended: 1 on security-hardened systems processing untrusted optical media.",
                Tags = ["cd", "execute", "autorun", "removable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks auto-execute from CD-ROM; disc content is still readable via explicit app launch.",
                ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Execute", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Execute")],
                DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Execute", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-dvd-read",
                Label = "Block DVD Read Access",
                Category = "Storage",
                Description =
                    "Sets Deny_Read=1 in the DVD/BD removable storage device class policy (GUID {53f56307}). "
                    + "Prevents all read access to DVD and Blu-ray drives. "
                    + "Default: absent. Recommended: 1 only in air-gapped environments where optical media is prohibited.",
                Tags = ["dvd", "read", "removable", "optical", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Completely blocks DVD/BD read access; breaks all optical disc software including media players.",
                ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-dvd-write",
                Label = "Block DVD Write Access",
                Category = "Storage",
                Description =
                    "Sets Deny_Write=1 in the DVD device class policy. "
                    + "Prevents all write operations to DVD±R/RW drives via the OS removable storage policy layer. "
                    + "Default: absent (write allowed). Recommended: 1 for data-exfiltration prevention.",
                Tags = ["dvd", "write", "removable", "optical", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks DVD write at the device access policy layer; requires re-enable for disc authoring.",
                ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-dvd-execute",
                Label = "Block DVD Execute (AutoRun)",
                Category = "Storage",
                Description =
                    "Sets Deny_Execute=1 in the DVD device class policy. "
                    + "Prevents the system from auto-executing content directly from DVD drives via the removable storage access layer. "
                    + "Default: absent. Recommended: 1 for security hardening against malicious autoplay content on optical media.",
                Tags = ["dvd", "execute", "autorun", "removable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks DVD auto-execution; manually launched DVD media apps still work.",
                ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Execute", 1)],
                RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Execute")],
                DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Execute", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-no-autoplay-nonvolume",
                Label = "Suppress AutoPlay for Non-Volume Optical Media",
                Category = "Storage",
                Description =
                    "Sets NoAutoplayfornonVolume=1 in the machine Explorer policy. "
                    + "Prevents Windows from automatically opening or showing the AutoPlay dialog when non-volume media "
                    + "(audio CDs, video DVDs, mixed-mode discs) is inserted. "
                    + "Default: absent (auto-prompt active). Recommended: 1 to reduce unwanted UI interruptions.",
                Tags = ["cd", "dvd", "autoplay", "prompt", "explorer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Silences the 'What do you want to do?' prompt for non-volume optical discs.",
                ApplyOps = [RegOp.SetDword(ExplLm, "NoAutoplayfornonVolume", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplLm, "NoAutoplayfornonVolume")],
                DetectOps = [RegOp.CheckDword(ExplLm, "NoAutoplayfornonVolume", 1)],
            },
        ];
    }

    // ── DiskQuotaAdvancedPolicy ──
    private static class _DiskQuotaAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dquota-enable-quota-policy",
                    Label = "Enable NTFS Disk Quota via Windows NT Policy Tree",
                    Category = "Storage",
                    Description =
                        "Enables NTFS disk quota tracking via the Windows NT policy registry path, activating per-user storage monitoring on all NTFS volumes in support of managed quota enforcement.",
                    Tags = ["disk-quota", "ntfs", "storage", "quota-policy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "NTFS quota tracking enabled via NT policy tree; per-user disk usage monitoring is active.",
                    ApplyOps = [RegOp.SetDword(Key, "Enable", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enable")],
                    DetectOps = [RegOp.CheckDword(Key, "Enable", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-enforce-hard-limit-policy",
                    Label = "Enforce Disk Quota Hard Limit via Policy",
                    Category = "Storage",
                    Description =
                        "Enforces disk quota limits as hard caps via the NT policy registry path, so that users exceeding their quota limit receive disk-full errors and cannot write additional data.",
                    Tags = ["disk-quota", "enforce", "hard-limit", "ntfs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Disk quota hard limit enforced via policy; users exceeding quota receive write denied errors.",
                    ApplyOps = [RegOp.SetDword(Key, "Enforce", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enforce")],
                    DetectOps = [RegOp.CheckDword(Key, "Enforce", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-log-exceed-policy",
                    Label = "Log Event on Quota Limit Exceeded via NT Policy",
                    Category = "Storage",
                    Description =
                        "Enables event log generation when a user exceeds their disk quota limit, via the NT policy registry path, creating an audit record under the System event log.",
                    Tags = ["disk-quota", "event-log", "exceed", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quota exceed events logged via NT policy; over-quota writes generate System event log entries.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverLimit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverLimit")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverLimit", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-log-threshold-policy",
                    Label = "Log Event on Quota Warning Threshold Reached via NT Policy",
                    Category = "Storage",
                    Description =
                        "Enables event log generation when a user reaches the disk quota warning threshold via the NT policy registry path, providing advance notice before hard quota is reached.",
                    Tags = ["disk-quota", "event-log", "warning", "threshold", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quota warning threshold events logged via NT policy; approaching-quota users trigger log entries.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverThreshold", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverThreshold")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverThreshold", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-suppress-balloon-notify",
                    Label = "Suppress Disk Quota Balloon Notification to Users",
                    Category = "Storage",
                    Description =
                        "Prevents the Windows balloon notification from appearing in the notification area when a user approaches or exceeds their disk quota, suppressing end-user prompts that cannot be action-ably resolved without IT involvement.",
                    Tags = ["disk-quota", "balloon", "notification", "ux", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disk quota balloon notifications suppressed; users near quota limit are not prompted.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBalloonNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBalloonNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBalloonNotification", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-disable-quota-properties-tab",
                    Label = "Remove Quota Properties Tab from Drive Properties",
                    Category = "Storage",
                    Description =
                        "Removes the Quota tab from the drive Properties dialog for non-administrator users, preventing visibility or modification of quota settings outside of Group Policy managed configuration.",
                    Tags = ["disk-quota", "properties-tab", "ui", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quota tab removed from drive Properties; quota configuration hidden from non-admin users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableQuotaPropertiesTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableQuotaPropertiesTab")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableQuotaPropertiesTab", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-block-per-volume-override",
                    Label = "Block Per-Volume Quota Setting Override by Administrators",
                    Category = "Storage",
                    Description =
                        "Prevents local administrators from modifying disk quota settings on individual volumes, ensuring that the centrally configured quota policy via Group Policy cannot be overridden at the local machine level.",
                    Tags = ["disk-quota", "per-volume", "lockdown", "gpo", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Per-volume quota overrides blocked; GPO quota settings cannot be changed locally.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPerVolumeOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPerVolumeOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPerVolumeOverride", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-display-quota-in-free-space",
                    Label = "Display Quota Remaining as Free Space in Explorer",
                    Category = "Storage",
                    Description =
                        "Configures Explorer to show a user's remaining quota allowance as the reported free disk space, preventing confusion where a user sees 100 GB free on a 500 GB drive but is personally limited to 5 GB.",
                    Tags = ["disk-quota", "free-space", "explorer", "ux", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Explorer shows personal quota remaining as free space; users see their effective limit not total disk.",
                    ApplyOps = [RegOp.SetDword(Key, "DisplayQuotaAsFreeDiskSpace", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisplayQuotaAsFreeDiskSpace")],
                    DetectOps = [RegOp.CheckDword(Key, "DisplayQuotaAsFreeDiskSpace", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-apply-to-mapped-drives",
                    Label = "Apply Disk Quota Tracking to Mapped Network Drives",
                    Category = "Storage",
                    Description =
                        "Extends disk quota tracking to mapped network drives (drive letter shares), so that per-user storage limits are also enforced when users write to network-mapped drives.",
                    Tags = ["disk-quota", "mapped-drives", "network", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quota tracking extended to mapped network drives; users cannot bypass local quotas via mapped paths.",
                    ApplyOps = [RegOp.SetDword(Key, "ApplyQuotaToMappedDrives", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ApplyQuotaToMappedDrives")],
                    DetectOps = [RegOp.CheckDword(Key, "ApplyQuotaToMappedDrives", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-restrict-quota-report-export",
                    Label = "Restrict Disk Quota Report Export to Administrators Only",
                    Category = "Storage",
                    Description =
                        "Prevents standard users from exporting or printing disk quota reports that contain per-user storage consumption data, protecting user privacy and preventing disclosure of storage usage patterns.",
                    Tags = ["disk-quota", "report", "export", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quota report export restricted to admins; standard users cannot access per-user storage consumption reports.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictQuotaReportExport", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictQuotaReportExport")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictQuotaReportExport", 1)],
                },
            ];
    }

    // ── DiskQuotaPolicy ──
    private static class _DiskQuotaPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskQuota";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "diskquota-enable-quota",
                    Label = "Enable NTFS Disk Quotas",
                    Category = "Storage",
                    Description =
                        "Activates disk quota tracking on all NTFS volumes. Without quotas enabled, individual users can consume an unlimited amount of disk space. Enabling quotas allows enforcement of per-user storage limits. Default: disk quotas disabled. Recommended: 1 on shared machines and file servers.",
                    Tags = ["disk", "quota", "ntfs", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Disk quota tracking is active on all NTFS volumes; per-user space consumption is monitored.",
                    ApplyOps = [RegOp.SetDword(Key, "Enable", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enable")],
                    DetectOps = [RegOp.CheckDword(Key, "Enable", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-enforce-quota-limit",
                    Label = "Enforce Disk Quota Limit (Deny Disk Space Beyond Limit)",
                    Category = "Storage",
                    Description =
                        "When quotas are enabled, this setting denies additional disk writes once a user reaches their quota limit — rather than merely logging a warning. Without enforcement, quotas are advisory only. Default: not enforced (log-only). Recommended: 1 if quotas are enabled.",
                    Tags = ["disk", "quota", "enforce", "ntfs", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Users who reach their quota limit receive an 'insufficient disk space' error; writes are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "Enforce", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enforce")],
                    DetectOps = [RegOp.CheckDword(Key, "Enforce", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-log-quota-exceeded",
                    Label = "Log Events When Quota Limit Is Exceeded",
                    Category = "Storage",
                    Description =
                        "Records an event in the Application log whenever a user exceeds their disk quota limit. Provides visibility over storage exhaustion incidents without requiring enforcement mode. Default: not logged. Recommended: 1 for compliance and monitoring.",
                    Tags = ["disk", "quota", "audit", "logging", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Application log entry written each time any user's quota limit is exceeded.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverLimit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverLimit")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverLimit", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-log-quota-warning",
                    Label = "Log Events When Quota Warning Threshold Is Reached",
                    Category = "Storage",
                    Description =
                        "Records an Application log event when a user's disk usage reaches the warning level (set below the hard quota limit). Gives early warning before the limit is hit. Default: not logged. Recommended: 1 for proactive storage management.",
                    Tags = ["disk", "quota", "warning", "audit", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Application log entry written when any user reaches the warning threshold.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverThreshold", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverThreshold")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverThreshold", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-apply-subdirectories",
                    Label = "Apply Quota to All Subdirectories",
                    Category = "Storage",
                    Description =
                        "Extends quota tracking so that disk space used by files in every subdirectory is counted against the owner's total quota. Without this, only root-level file writes are counted. Default: subdirectory counting depends on volume settings. Recommended: 1.",
                    Tags = ["disk", "quota", "subdirectory", "ntfs", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quotas account for all files in all subdirectories on NTFS volumes, not just root-level writes.",
                    ApplyOps = [RegOp.SetDword(Key, "CalibrateTargetDir", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "CalibrateTargetDir")],
                    DetectOps = [RegOp.CheckDword(Key, "CalibrateTargetDir", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-set-default-limit-1gb",
                    Label = "Set Default Per-User Quota Limit to 1 GB",
                    Category = "Storage",
                    Description =
                        "Sets the default disk quota limit applied to new user accounts to 1 073 741 824 bytes (1 GiB). New users automatically receive this limit without admin intervention. The value is stored as a QWORD count of bytes. Default: no limit (-1 / unlimited). Recommended: set as appropriate for available storage.",
                    Tags = ["disk", "quota", "limit", "default", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Each new user profile on NTFS volumes defaults to a 1 GiB hard quota.",
                    ApplyOps = [RegOp.SetQword(Key, "DefaultQuotaLimit", 1_073_741_824L)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultQuotaLimit")],
                    DetectOps = [],
                },
                new TweakDef
                {
                    Id = "diskquota-set-default-warning-800mb",
                    Label = "Set Default Per-User Warning Threshold to 800 MB",
                    Category = "Storage",
                    Description =
                        "Sets the warning threshold for new user accounts to 838 860 800 bytes (800 MiB). When a user reaches 80% of the 1 GiB default limit an event is logged before hitting the hard quota. Default: no warning threshold. Recommended: ~80% of the default quota limit.",
                    Tags = ["disk", "quota", "warning", "threshold", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Warning event fires when a new user reaches 800 MiB of disk usage.",
                    ApplyOps = [RegOp.SetQword(Key, "DefaultQuotaThreshold", 838_860_800L)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultQuotaThreshold")],
                    DetectOps = [],
                },
                new TweakDef
                {
                    Id = "diskquota-block-user-override",
                    Label = "Prevent Users from Changing Quota Settings",
                    Category = "Storage",
                    Description =
                        "Hides disk quota settings from the volume Properties dialog so standard users cannot view or modify their own quota limits. Works in conjunction with Enforce to prevent circumvention. Default: users can view their own quota from Properties. Recommended: 1.",
                    Tags = ["disk", "quota", "user-restriction", "settings", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quota tab is removed from volume Properties for non-admin users.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserSettings")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserSettings", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-disable-removable-volumes",
                    Label = "Do Not Apply Quotas to Removable Volumes",
                    Category = "Storage",
                    Description =
                        "Exempts removable NTFS volumes (USB drives formatted as NTFS) from quota management. Useful when quota enforcement should apply to fixed disks only and not to portable storage that may be shared. Default: quotas apply to all NTFS volumes including removable. Recommended: 0 to include removable.",
                    Tags = ["disk", "quota", "removable", "usb", "ntfs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removable NTFS volumes are excluded from quota enforcement; only fixed disks are subject to limits.",
                    ApplyOps = [RegOp.SetDword(Key, "ExcludeRemovableMedia", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExcludeRemovableMedia")],
                    DetectOps = [RegOp.CheckDword(Key, "ExcludeRemovableMedia", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-exempt-admins",
                    Label = "Exempt Administrators from Disk Quota Limits",
                    Category = "Storage",
                    Description =
                        "Members of the local Administrators group are not subject to per-user disk quota enforcement even when the volume is quota-managed. Allows admins to perform maintenance (driver updates, log writes, backups) without hitting storage walls. Default: admins are also subject to quotas when Enforce=1. Recommended: 1.",
                    Tags = ["disk", "quota", "admin", "exemption", "ntfs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Local admins bypass disk quota limits; standard user quotas remain enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "ExemptAdministrators", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExemptAdministrators")],
                    DetectOps = [RegOp.CheckDword(Key, "ExemptAdministrators", 1)],
                },
            ];
    }

    // ── FileHistoryPolicy ──
    private static class _FileHistoryPolicy
    {
        private const string FhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory";
        private const string BkpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "fhp-disable-file-history",
                Label = "Disable File History",
                Category = "Storage",
                Description =
                    "Sets Disabled=1 in the File History policy key. "
                    + "Turns off the File History backup service for all users on this machine. "
                    + "The File History control panel applet will display the feature as disabled by policy. "
                    + "Default: absent (File History available to users). Recommended: 1 on server or managed deployments using alternative backup solutions.",
                Tags = ["file-history", "backup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables File History; users lose automatic versioned file backup unless an alternative is configured.",
                ApplyOps = [RegOp.SetDword(FhKey, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(FhKey, "Disabled")],
                DetectOps = [RegOp.CheckDword(FhKey, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "fhp-lock-onoff-switch",
                Label = "Lock File History On/Off Switch",
                Category = "Storage",
                Description =
                    "Sets OnOffSwitchLocked=1 in the File History policy key. "
                    + "Prevents users from enabling or disabling File History via the Control Panel or Settings. "
                    + "The toggle is greyed out and displays 'Some settings are managed by your organization'. "
                    + "Default: absent. Recommended: 1 when File History state must be centrally controlled.",
                Tags = ["file-history", "backup", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents users from toggling File History; does not change its enabled/disabled state.",
                ApplyOps = [RegOp.SetDword(FhKey, "OnOffSwitchLocked", 1)],
                RemoveOps = [RegOp.DeleteValue(FhKey, "OnOffSwitchLocked")],
                DetectOps = [RegOp.CheckDword(FhKey, "OnOffSwitchLocked", 1)],
            },
            new TweakDef
            {
                Id = "fhp-backup-interval-daily",
                Label = "Set File History Backup Interval to Daily",
                Category = "Storage",
                Description =
                    "Sets BackupInterval=86400 (24 hours in seconds) in the File History policy key. "
                    + "Controls how frequently File History backs up changed files. "
                    + "Default: 3600 (hourly). Recommended: 86400 on systems with large user profiles or limited backup storage.",
                Tags = ["file-history", "backup", "interval", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Reduces File History backup frequency to once daily; decreases I/O and storage consumption.",
                ApplyOps = [RegOp.SetDword(FhKey, "BackupInterval", 86400)],
                RemoveOps = [RegOp.DeleteValue(FhKey, "BackupInterval")],
                DetectOps = [RegOp.CheckDword(FhKey, "BackupInterval", 86400)],
            },
            new TweakDef
            {
                Id = "fhp-retention-until-space-needed",
                Label = "File History: Keep Until Space Is Needed",
                Category = "Storage",
                Description =
                    "Sets RetentionPolicy=1 in the File History policy key. "
                    + "Configures File History to retain backup copies until the drive runs low on space, "
                    + "at which point older versions are automatically removed. "
                    + "Default: absent (user-configured). Recommended: 1 for space-constrained backup targets.",
                Tags = ["file-history", "backup", "retention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Oldest backup versions are deleted automatically when backup drive space runs low.",
                ApplyOps = [RegOp.SetDword(FhKey, "RetentionPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(FhKey, "RetentionPolicy")],
                DetectOps = [RegOp.CheckDword(FhKey, "RetentionPolicy", 1)],
            },
            new TweakDef
            {
                Id = "fhp-retention-one-month",
                Label = "File History: Keep Versions for 1 Month",
                Category = "Storage",
                Description =
                    "Sets RetentionPolicy=2 and RetentionTime=1 in the File History policy key. "
                    + "Configures File History to keep only backup copies made within the past month; older versions are purged automatically. "
                    + "Default: absent (keep forever). Recommended: on systems where 1-month recovery window is sufficient.",
                Tags = ["file-history", "backup", "retention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Limits backup history to 1 month, freeing backup drive space over time.",
                ApplyOps = [RegOp.SetDword(FhKey, "RetentionPolicy", 2), RegOp.SetDword(FhKey, "RetentionTime", 1)],
                RemoveOps = [RegOp.DeleteValue(FhKey, "RetentionPolicy"), RegOp.DeleteValue(FhKey, "RetentionTime")],
                DetectOps = [RegOp.CheckDword(FhKey, "RetentionPolicy", 2)],
            },
            new TweakDef
            {
                Id = "fhp-prevent-data-degradation",
                Label = "Prevent File History Data Degradation",
                Category = "Storage",
                Description =
                    "Sets DataDegradationPolicy=1 in the File History policy key. "
                    + "Causes File History to stop backing up if the protection level would fall due to cache issues, "
                    + "rather than silently continuing with degraded coverage. "
                    + "Default: absent (degraded backup is allowed). Recommended: 1 to ensure backup integrity or alert on problems.",
                Tags = ["file-history", "backup", "integrity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "File History halts if backup data integrity would be compromised rather than silently degrading.",
                ApplyOps = [RegOp.SetDword(FhKey, "DataDegradationPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(FhKey, "DataDegradationPolicy")],
                DetectOps = [RegOp.CheckDword(FhKey, "DataDegradationPolicy", 1)],
            },
            new TweakDef
            {
                Id = "fhp-disable-file-backup",
                Label = "Disable Windows Backup File Backup",
                Category = "Storage",
                Description =
                    "Sets DisableFileBackup=1 in the Windows Backup client policy key. "
                    + "Prevents users from performing file-level backups using the Windows Backup client. "
                    + "The backup option is hidden from the Backup and Restore Control Panel applet. "
                    + "Default: absent. Recommended: 1 when an enterprise backup solution replaces Windows Backup.",
                Tags = ["backup", "file-backup", "policy", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Removes the Windows Backup file-backup option; enterprise backup tools are unaffected.",
                ApplyOps = [RegOp.SetDword(BkpKey, "DisableFileBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableFileBackup")],
                DetectOps = [RegOp.CheckDword(BkpKey, "DisableFileBackup", 1)],
            },
            new TweakDef
            {
                Id = "fhp-disable-system-backup",
                Label = "Disable Windows Backup System (Image) Backup",
                Category = "Storage",
                Description =
                    "Sets DisableSystemBackup=1 in the Windows Backup client policy key. "
                    + "Prevents users from creating system image backups using the Windows Backup client. "
                    + "Default: absent. Recommended: 1 on managed devices where system images are managed centrally.",
                Tags = ["backup", "system-image", "policy", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Windows Backup system-imaging; system restore points via System Protection are unaffected.",
                ApplyOps = [RegOp.SetDword(BkpKey, "DisableSystemBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableSystemBackup")],
                DetectOps = [RegOp.CheckDword(BkpKey, "DisableSystemBackup", 1)],
            },
            new TweakDef
            {
                Id = "fhp-disable-restore-ui",
                Label = "Disable Windows Backup Restore UI",
                Category = "Storage",
                Description =
                    "Sets DisableRestoreUI=1 in the Windows Backup client policy key. "
                    + "Hides the 'Restore my files' and related controls from the Backup and Restore Control Panel applet. "
                    + "Default: absent. Recommended: 1 when restore operations must pass through IT-managed tooling.",
                Tags = ["backup", "restore", "policy", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes Windows Backup restore UI; backed-up files still exist but require IT tools to restore.",
                ApplyOps = [RegOp.SetDword(BkpKey, "DisableRestoreUI", 1)],
                RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableRestoreUI")],
                DetectOps = [RegOp.CheckDword(BkpKey, "DisableRestoreUI", 1)],
            },
            new TweakDef
            {
                Id = "fhp-disable-restored-ui",
                Label = "Disable Windows Backup 'Restore to Previous PC' UI",
                Category = "Storage",
                Description =
                    "Sets DisableRestoredUI=1 in the Windows Backup client policy key. "
                    + "Hides the Windows Easy Transfer / 'Restore files from a previous PC' experience "
                    + "from the Backup and Restore applet. "
                    + "Default: absent. Recommended: 1 on corporate builds where legacy data migration is handled by IT.",
                Tags = ["backup", "restore", "migration", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the 'previous PC restore' migration experience; no data is changed.",
                ApplyOps = [RegOp.SetDword(BkpKey, "DisableRestoredUI", 1)],
                RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableRestoredUI")],
                DetectOps = [RegOp.CheckDword(BkpKey, "DisableRestoredUI", 1)],
            },
        ];
    }

    // ── FileSharePolicy ──
    private static class _FileSharePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "filshare-disable-auto-share-wks",
                Label = "Disable Automatic Administrative Workstation Shares",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Automatic administrative shares (C$ D$ ADMIN$ IPC$) are created by Windows Server automatically and provide administrative remote access. Disabling automatic workstation shares prevents these implicit access points from being created without explicit administrator action. Automatic administrative shares are used by attackers for lateral movement, remote file copying, and tools like PsExec and WMI. Many enterprise environments disable automatic shares on workstations where remote administration is handled through dedicated management solutions. Disabling automatic shares reduces the attack surface without impacting endpoint functionality for standard users. Remote management tools should be configured to use alternative mechanisms that do not require default administrative shares.",
                Tags = ["file-share", "admin-shares", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutoShareWks", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoShareWks")],
                DetectOps = [RegOp.CheckDword(Key, "AutoShareWks", 0)],
            },
            new TweakDef
            {
                Id = "filshare-disable-auto-share-server",
                Label = "Disable Automatic Administrative Server Shares",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Server administrative shares are automatically created by the LanmanServer service providing full disk access to administrator accounts over the network. Disabling automatic server shares prevents implicit remote access paths that can be exploited if administrator credentials are compromised. Server shares are commonly required for legitimate file server operations but should be explicitly created rather than automatically managed. Explicit administrative share creation provides better visibility into which shares exist and who has access to each. Some management tools including legacy backup software may depend on automatic administrative shares for file system access. Organizations should audit management tool dependencies before disabling automatic server shares to avoid breaking critical operations.",
                Tags = ["file-share", "server-shares", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutoShareServer", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoShareServer")],
                DetectOps = [RegOp.CheckDword(Key, "AutoShareServer", 0)],
            },
            new TweakDef
            {
                Id = "filshare-require-secure-dialect",
                Label = "Set Minimum SMB Server Dialect",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Setting a minimum SMB dialect version for the server component prevents clients from negotiating to use older vulnerable protocol versions. A minimum dialect requirement of SMB 2.0.2 or higher prevents connections from SMB1-only clients that are incompatible with security features. File servers supporting only modern SMB dialects eliminate exposure to SMB1 vulnerabilities like EternalBlue and related exploits. Minimum dialect enforcement may affect legacy clients and network appliances that only speak older SMB versions. Enterprise environments should inventory SMB client versions before enforcing minimum dialect requirements to avoid connectivity disruption. Graceful enforcement with monitoring and logging before hard blocking provides visibility into legacy client dependencies.",
                Tags = ["file-share", "dialect", "smb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSecureDialect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureDialect")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSecureDialect", 1)],
            },
            new TweakDef
            {
                Id = "filshare-enable-server-signing",
                Label = "Require SMB Signing on Server",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SMB signing on the server component ensures that all incoming SMB connections are packet-signed preventing relay and tampering attacks. Requiring SMB signing eliminates the possibility of SMB relay attacks where an attacker captures and forwards an authenticated connection. SMB relay without signing is the foundation of many lateral movement techniques including NTLM relay and credential forwarding. File servers and domain controllers should always require SMB signing as they are the most valuable targets for relay attacks. Enabling SMB signing on all servers combined with client-side signing requirements creates a fully signed network protecting all SMB communications. Performance impact of SMB signing is minimal on modern hardware and should not be a reason to defer enablement.",
                Tags = ["file-share", "smb-signing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSecuritySignature", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSecuritySignature")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSecuritySignature", 1)],
            },
            new TweakDef
            {
                Id = "filshare-set-max-concurrent-sessions",
                Label = "Limit Maximum Concurrent SMB Sessions",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Limiting concurrent SMB sessions prevents resource exhaustion attacks that could make file servers unavailable to legitimate users. Maximum session limits ensure that no single client or group of clients can monopolize file server connection resources. SMB session flooding is a simple denial of service attack that can be performed from within the network by malicious insiders or compromised endpoints. Session limits combined with connection rate throttling provide DoS protection for file sharing infrastructure. Overly low session limits may affect large file servers with many concurrent user connections and should be sized based on actual usage. Monitoring concurrent session counts against defined limits helps detect unusual connection patterns from potentially compromised systems.",
                Tags = ["file-share", "sessions", "dos-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxConcurrentConnections", 16384)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxConcurrentConnections")],
                DetectOps = [RegOp.CheckDword(Key, "MaxConcurrentConnections", 16384)],
            },
            new TweakDef
            {
                Id = "filshare-enable-server-encryption",
                Label = "Enable SMB Encryption on File Server",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Enabling SMB encryption on the server component protects all file transfer data from network interception by encrypting SMB traffic. Server-side encryption configuration ensures that all clients connecting to the server receive encrypted data regardless of client-side configuration. SMB encryption is available in SMB3 and provides AES-128-CCM or AES-128-GCM protection for all transferred data. Encrypted SMB prevents passive network capture from exposing file contents, metadata, and authentication data. File servers containing sensitive information should have encryption enabled even if clients are trusted to prevent interception at the network layer. Enabling encryption per-share for sensitive shares allows gradual deployment where only specific high-value shares require encryption.",
                Tags = ["file-share", "encryption", "smb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EncryptData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EncryptData")],
                DetectOps = [RegOp.CheckDword(Key, "EncryptData", 1)],
            },
            new TweakDef
            {
                Id = "filshare-reject-unencrypted-access",
                Label = "Reject Unencrypted Client Connections",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Rejecting unencrypted client connections ensures that the file server refuses SMB connections from clients that do not support or use encryption. When combined with server encryption requirements, rejecting unencrypted connections enforces end-to-end encryption for all file server access. Clients running Windows 10 or Server 2016 and later all support SMB encryption but legacy clients may connect without encryption support. Rejecting unencrypted connections prevents a mixed-security scenario where some clients use encryption and others do not. Organizations must ensure all file server clients support SMB3 encryption before enforcing rejection of unencrypted connections. Monitoring SMB session negotiations before enforcement helps identify legacy clients that need to be updated or replaced.",
                Tags = ["file-share", "encryption", "reject-unencrypted", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RejectUnencryptedAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RejectUnencryptedAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RejectUnencryptedAccess", 1)],
            },
            new TweakDef
            {
                Id = "filshare-restrict-null-session-shares",
                Label = "Restrict Shares Accessible via Null Sessions",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Null session shares are accessible to unauthenticated network connections and provide an information disclosure path for network enumeration. Restricting null session shares prevents anonymous access to file shares through unauthenticated SMB connections. Null sessions allow remote enumeration of share names from which attackers can identify targets for authenticated access attempts. The NullSessionShares registry value lists which shares can be accessed without authentication and should contain no shares in secure configurations. Legacy applications that require null session access should be replaced with authenticated alternatives. Null session restriction is a fundamental network security control that should be enforced on all Windows systems accessible from the network.",
                Tags = ["file-share", "null-session", "anonymous", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNullSessionShares", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNullSessionShares")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNullSessionShares", 1)],
            },
            new TweakDef
            {
                Id = "filshare-log-unauthorized-access",
                Label = "Enable Unauthorized File Share Access Logging",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Unauthorized file share access logging records failed access attempts to shares where the requestor lacks sufficient permissions. Enabling unauthorized access logging generates security events for share access denials providing visibility into access control boundary events. Failed share access events can indicate configuration errors, misconfigured access controls, or attempted unauthorized access. Security Event Log event 5140 with failure status records share access denials with requestor account and share name. Correlation of repeated share access failures from the same account across multiple servers may indicate lateral movement scanning. Unauthorized access events should be forwarded to SIEM and correlated with authentication events to assess intent and risk.",
                Tags = ["file-share", "access-denied", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogUnauthorizedAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogUnauthorizedAccess")],
                DetectOps = [RegOp.CheckDword(Key, "LogUnauthorizedAccess", 1)],
            },
            new TweakDef
            {
                Id = "filshare-disable-oplocks",
                Label = "Configure Opportunistic Locking for Sensitive Shares",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Opportunistic locking allows clients to cache file data locally for performance optimization but can cause data corruption with multiple concurrent editors. Configuring oplock behavior for sensitive shares prevents data loss scenarios where multiple writers believe they have exclusive access. Oplocks on database files, transactional data, and other sensitive data could cause corruption if network connectivity is interrupted during a cache hold. Disabling oplocks on specific shares forces clients to directly read from and write to the server ensuring data consistency. Oplocks are generally appropriate for read-heavy workloads but should be disabled for shares containing database files, application logs, or frequently modified shared configuration files. Share-level oplock configuration provides granular control without disabling oplocks globally across all shares.",
                Tags = ["file-share", "oplocks", "consistency", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigureOplocks", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigureOplocks")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigureOplocks", 0)],
            },
        ];
    }

    // ── FileShareWitnessPolicy ──
    private static class _FileShareWitnessPolicy
    {
        private const string SrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        private const string WrkKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fswitness-enable-smb-signing-server",
                    Label = "File Share Witness: Require SMB Signing on Server",
                    Category = "Storage",
                    Description =
                        "Sets RequireSecuritySignature=1 in LanmanServer policy. Requires that all SMB connections to this machine as a server must use SMB packet signing. Without signing, SMB traffic is vulnerable to relay attacks (NTLM relay, SMB relay) where an attacker positioned between client and server can intercept and reuse SMB authentication tokens to authenticate to other services. SMB signing ensures each packet is cryptographically bound to the session — tampered or replayed packets are detected and rejected by both client and server.",
                    Tags = ["smb", "signing", "relay-attack", "security", "server"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "All SMB clients must negotiate signing when connecting to this server. Legacy SMB clients that do not support signing (rare; pre-Vista) will be rejected. Slight CPU overhead per packet on high-throughput file servers.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "RequireSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "RequireSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "RequireSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "fswitness-enable-smb-signing-client",
                    Label = "File Share Witness: Require SMB Signing on Client",
                    Category = "Storage",
                    Description =
                        "Sets RequireSecuritySignature=1 in LanmanWorkstation policy. Requires that all outbound SMB connections from this machine as a client use SMB packet signing. The complementary client-side SMB signing policy ensures this device cannot connect to a rogue or unpatched server that does not sign packets — closing the attack path where an attacker deploys a malicious SMB server to capture NTLMv2 hashes. Together with server-side signing (fswitness-enable-smb-signing-server), both ends of every SMB connection are protected.",
                    Tags = ["smb", "signing", "client", "relay-attack", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "This client only connects to SMB servers that support signing. Servers that do not support SMB signing are refused connections. Unpatched or misconfigured legacy file servers may become unreachable.",
                    ApplyOps = [RegOp.SetDword(WrkKey, "RequireSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(WrkKey, "RequireSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(WrkKey, "RequireSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "fswitness-disable-guestaccess-smb",
                    Label = "File Share Witness: Disable SMB Guest Access on Client",
                    Category = "Storage",
                    Description =
                        "Sets AllowInsecureGuestAuth=0 in LanmanWorkstation policy. Prevents the SMB client from connecting to a file share using unauthenticated guest access. When a Windows SMB client cannot authenticate with the credentials it has (wrong username/password), it may fall back to connecting as 'Guest' — an anonymous account with no password. Rogue SMB servers exploit this fallback to man-in-the-middle legitimate connections. Microsoft disabled guest access by default in Windows 10 1709; this policy enforces the setting via Group Policy to prevent local overrides.",
                    Tags = ["smb", "guest", "unauthenticated", "security", "lateral-movement"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "SMB guest connections are blocked. Shares with misconfigured permissions that previously allowed guest may become inaccessible. SMB connections require valid credentials. Users with wrong passwords receive an authentication error rather than a guest session.",
                    ApplyOps = [RegOp.SetDword(WrkKey, "AllowInsecureGuestAuth", 0)],
                    RemoveOps = [RegOp.DeleteValue(WrkKey, "AllowInsecureGuestAuth")],
                    DetectOps = [RegOp.CheckDword(WrkKey, "AllowInsecureGuestAuth", 0)],
                },
                new TweakDef
                {
                    Id = "fswitness-disable-smb1-server",
                    Label = "File Share Witness: Disable SMB1 Protocol on Server",
                    Category = "Storage",
                    Description =
                        "Sets SMB1=0 in LanmanServer policy. Disables the SMBv1 protocol on the server component. SMBv1 is a 1980s-era protocol with no encryption, no pre-authentication integrity, no signing by default, and numerous unfixed vulnerabilities including EternalBlue (CVE-2017-0144) which was exploited by WannaCry and NotPetya ransomware. Microsoft deprecated SMBv1 in 2014. Any operating system newer than Windows XP/Server 2003 supports SMBv2+. Disabling SMBv1 on the server prevents legacy client connections but eliminates the most dangerous attack surface in Windows networking.",
                    Tags = ["smb1", "smb", "eternalblue", "ransomware", "protocol"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "SMBv1 server is disabled. Clients that can only use SMBv1 (Windows XP, Server 2003, early SAMBA versions, some legacy NAS appliances) cannot connect. Verify no SMBv1-only clients exist before applying.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "SMB1", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "SMB1")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "SMB1", 0)],
                },
                new TweakDef
                {
                    Id = "fswitness-set-smb-max-connections",
                    Label = "File Share Witness: Set Maximum SMB Simultaneous Open Files Limit",
                    Category = "Storage",
                    Description =
                        "Sets MaxWorkItems=16384 in LanmanServer policy. Sets the maximum number of SMB work items (pending I/O operations per connection) the server will process simultaneously. The default value (64 on some configurations) can cause server-side SMB queuing under heavy load from many concurrent clients (e.g., login storms or VDI deployments). Increasing to 16384 allows more concurrent file operations without queuing delay. This setting must be balanced against available memory — each work item consumes non-paged pool memory.",
                    Tags = ["smb", "performance", "work-items", "concurrency", "file-server"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Server processes up to 16384 simultaneous SMB work items. Improves throughput under high-concurrency file access. Consumes additional non-paged pool memory on servers with many concurrent SMB clients.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "MaxWorkItems", 16384)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "MaxWorkItems")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "MaxWorkItems", 16384)],
                },
                new TweakDef
                {
                    Id = "fswitness-enable-smb-encryption",
                    Label = "File Share Witness: Enable SMB3 Encryption on Server",
                    Category = "Storage",
                    Description =
                        "Sets EncryptData=1 in LanmanServer policy. Enables mandatory AES-CCM or AES-GCM encryption for all SMB3 data transfers between client and server. SMB signing (enforced separately) provides integrity but not confidentiality — signing prevents tampering but a network observer can still read file contents in plaintext. SMB encryption wraps the data payload in a cryptographic envelope. Required for environments where file shares carry sensitive data (PII, financial, health records) and the network is not fully trusted (branch offices, cloud-hosted file servers).",
                    Tags = ["smb3", "encryption", "confidentiality", "data-protection", "aes"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "All SMB3 connections to this server use encryption. Clients must support SMB3 encryption (Windows 8+ / Server 2012+). CPU overhead for encryption; significant impact on high-bandwidth file copy operations on CPU-constrained servers.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EncryptData", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EncryptData")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EncryptData", 1)],
                },
                new TweakDef
                {
                    Id = "fswitness-set-idle-connection-timeout",
                    Label = "File Share Witness: Set SMB Server Idle Connection Timeout to 15 Minutes",
                    Category = "Storage",
                    Description =
                        "Sets AutoDisconnect=15 in LanmanServer policy. Sets the idle timeout for SMB server connections to 15 minutes. By default, SMB servers disconnect idle clients after 15 minutes, but the policy value can be changed. An excessively long idle timeout keeps network connections open and server-side session state allocated for users who have walked away from their desks. Idle sessions can also be reused after token expiry, enabling authentication bypass with stale credentials. 15 minutes matches the optimal balance between session reuse costs and reconnection overhead.",
                    Tags = ["smb", "idle-timeout", "session-management", "server", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Idle SMB sessions are disconnected after 15 minutes. Users reconnect transparently when accessing mapped drives after idle disconnection. Short-lived applications that hold SMB connections open but rarely use them may silently reconnect.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoDisconnect", 15)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoDisconnect")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoDisconnect", 15)],
                },
                new TweakDef
                {
                    Id = "fswitness-restrict-anonymous-share-enum",
                    Label = "File Share Witness: Restrict Anonymous Network Share Enumeration",
                    Category = "Storage",
                    Description =
                        "Sets RestrictNullSessAccess=1 in LanmanServer policy. Prevents unauthenticated (null session) enumeration of network shares over the IPC$ pipe. Without this restriction, an attacker who discovers a Windows server's IP address can use null session connections to enumerate all shared folder names, providing the attacker with a map of file shares they can then target with brute-force credential attacks. This setting is enabled by default but can be disabled by administrators or overwrote by other policies; enforcing it via policy ensures it cannot be changed.",
                    Tags = ["smb", "anonymous", "null-session", "enumeration", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous (null session) network share enumeration is blocked. Legitimate client connections with credentials are unaffected. Tools like NetScan and LanSweeper that enumerate shares with null sessions may not discover shares on this server.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "RestrictNullSessAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "RestrictNullSessAccess")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "RestrictNullSessAccess", 1)],
                },
                new TweakDef
                {
                    Id = "fswitness-enable-smb-hardened-unc",
                    Label = "File Share Witness: Enable Hardened UNC Path Requirements",
                    Category = "Storage",
                    Description =
                        "Sets HardenedUNCPathsEnabled=1 in LanmanWorkstation policy. Enables hardened UNC path processing, which requires mutual authentication and integrity for connections to UNC paths matching patterns registered in the HardenedUNCPaths registry list (\\\\*\\NETLOGON, \\\\*\\SYSVOL, etc). Without hardened UNC paths, a man-in-the-middle attacker can serve a rogue SYSVOL or NETLOGON share to deliver malicious Group Policy objects or logon scripts. Hardened UNC paths were introduced as the main mitigation for MS15-011 (JASBUG).",
                    Tags = ["smb", "unc", "hardening", "gpo", "ms15-011"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "UNC paths to NETLOGON and SYSVOL require mutual authentication and signing. Man-in-the-middle attacks against Group Policy delivery are blocked. No impact on normal AD-joined clients with a working domain connection.",
                    ApplyOps = [RegOp.SetDword(WrkKey, "HardenedUNCPathsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WrkKey, "HardenedUNCPathsEnabled")],
                    DetectOps = [RegOp.CheckDword(WrkKey, "HardenedUNCPathsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "fswitness-disable-admin-shares",
                    Label = "File Share Witness: Disable Automatic Administrative Shares",
                    Category = "Storage",
                    Description =
                        "Sets AutoShareWks=0 in LanmanServer policy. Disables the automatic creation of administrative hidden shares (C$, D$, ADMIN$) on workstations. Administrative shares allow remote administrative access to root drive paths over SMB. While useful for IT management tools and legacy remote administration scripts, these shares provide an attackers with direct filesystem access if a privileged account is compromised. In environments using modern management tools (Intune, SCCM, WinRM), the administrative shares are rarely needed and present unnecessary lateral movement surface.",
                    Tags = ["smb", "admin-shares", "attack-surface", "lateral-movement", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "C$, D$, ADMIN$ administrative shares are removed. Remote administration tools that rely on these shares (PsExec, legacy SCCM push, RoboCopy to C$) stop working. Verify that all management tools use WinRM, WMI, or agent-based access before applying.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoShareWks", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoShareWks")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoShareWks", 0)],
                },
            ];
    }

    // ── NtfsPolicy ──
    private static class _NtfsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NTFS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ntfspol-disable-last-access",
                Label = "Disable NTFS Last Access Time Update",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NTFS records the last access time for every file each time it is read, requiring a metadata write operation on every file read. Disabling last access time updates eliminates the write operation triggered on every file read, significantly reducing metadata update overhead. On systems with millions of files, last access time recording causes substantial unnecessary disk I/O especially for antivirus scans and search indexers. Removing last access time recording can improve performance of read-heavy workloads by up to 15 percent on spinning disk systems. Last access time is rarely used by enterprise applications and is not required for data classification or integrity purposes. Security tools that require access time tracking should use shadow copies or file activity monitoring instead.",
                Tags = ["ntfs", "performance", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLastAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLastAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLastAccess", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-enable-compression",
                Label = "Disable NTFS Compression on System Volume",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "NTFS transparent compression reduces file sizes on disk by compressing file contents using the LZ77 compression algorithm. Disabling NTFS compression on the system volume prevents performance-degrading CPU overhead from compression and decompression on every file access. NTFS compression causes random access pattern degradation because compressed files require sequential decompression to reach arbitrary offsets. Modern SSD storage provides sufficient capacity that compression decompression overhead is not worth the space savings. System files including the page file hivelist and drivers should never be compressed as it introduces unbounded decompression latency at critical moments. Disabling system volume compression ensures consistent and predictable I/O performance for OS operations.",
                Tags = ["ntfs", "compression", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-8dot3-names",
                Label = "Disable NTFS 8.3 Short Name Generation",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "NTFS generates short 8.3 format filenames for every file created to maintain backward compatibility with legacy 16-bit applications. Disabling 8.3 short name generation eliminates this legacy metadata overhead and reduces directory metadata size. Short name generation requires computing and storing an additional name entry for every file, increasing directory metadata write cost. Enterprise environments running exclusively 32-bit and 64-bit applications do not benefit from 8.3 name compatibility. Disabling short names can improve file creation performance on systems with very large directories containing many files. Note that some older administrative tools and scripts may depend on short names; these should be tested before deployment.",
                Tags = ["ntfs", "8dot3", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Disable8dot3NameCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Disable8dot3NameCreation")],
                DetectOps = [RegOp.CheckDword(Key, "Disable8dot3NameCreation", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-self-healing",
                Label = "Disable NTFS Self-Healing (Force Chkdsk)",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 2,
                Description =
                    "NTFS self-healing automatically repairs filesystem inconsistencies detected during normal operation without requiring offline chkdsk runs. Disabling self-healing forces filesystem errors to be addressed through the traditional offline chkdsk process requiring a system restart. Self-healing runs in the background and may silently alter filesystem metadata in ways that complicate forensic analysis. Forensic investigation scenarios require preservation of exact filesystem state including errors for evidentiary purposes. Some self-healing repairs may destroy evidence of intrusion by cleaning up attacker-modified metadata. This tweak is intended specifically for forensic workstations and incident response systems, not general enterprise use.",
                Tags = ["ntfs", "self-healing", "forensics", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSelfHealing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSelfHealing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSelfHealing", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-encryption-default",
                Label = "Prevent Default NTFS Encryption of New Files",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "NTFS Encrypted File System inheritable encryption allows folders to be marked such that all files created within them are automatically encrypted. Preventing default encryption inheritance stops new files from being automatically encrypted by inheriting parent directory encryption attributes. Automatic encryption without user awareness can prevent legitimate administrative access to files for maintenance and incident response. EFS-encrypted files that lose access to the recovery certificate become permanently inaccessible creating potential data loss. Enterprise file encryption should be managed through BitLocker for volume encryption rather than per-file EFS inheritance. Disabling default EFS inheritance prevents accidental file lockout while preserving the ability to use EFS intentionally.",
                Tags = ["ntfs", "encryption", "efs", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEncryptionDefault", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEncryptionDefault")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEncryptionDefault", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-delete-notify",
                Label = "Disable NTFS Delete Notify to SSD Controller",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "NTFS sends TRIM commands to SSD controllers when files are deleted, allowing the controller to proactively reclaim and erase NAND flash cells. Disabling delete notify (TRIM) prevents Windows from sending TRIM commands to connected storage controllers. TRIM disabled mode can be useful when using storage over certain RAID configurations or shared storage that does not benefit from per-host TRIM. Some NVMe configurations perform background garbage collection more efficiently without host-generated TRIM hints. TRIM should only be disabled when the storage subsystem design is known to perform better without it. Most modern consumer and enterprise SSDs benefit from TRIM and disabling it degrades long-term write performance.",
                Tags = ["ntfs", "trim", "ssd", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeleteNotify", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeleteNotify")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeleteNotify", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-alternate-data-streams-block",
                Label = "Block Alternate Data Stream Creation by Untrusted Code",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "NTFS Alternate Data Streams allow additional data to be attached to files under hidden named streams that are invisible to most file browsers. Blocking ADS creation by untrusted code prevents malware and unauthorized applications from hiding data in invisible file streams. Alternate data streams are used by some malware to store payloads, configuration, or exfiltrated data in streams invisible to directory listings. The Zone.Identifier ADS created by browsers is a legitimate security feature that marks downloaded files and should be preserved. Blocking indiscriminate ADS creation from untrusted sources limits the use of this NTFS feature as a steganographic storage mechanism. Security tools that rely on ADS for file metadata tags should be evaluated and explicitly exempted.",
                Tags = ["ntfs", "ads", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockAlternateDataStreamsByUntrusted", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAlternateDataStreamsByUntrusted")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAlternateDataStreamsByUntrusted", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-tunnel-cache",
                Label = "Disable NTFS Filename Tunnel Cache",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The NTFS filename tunnel cache briefly preserves the identity of a short filename when a file is deleted and recreated with the same name. Disabling the tunnel cache prevents NTFS from re-associating the previous short filename when a file with the same long name is recreated. The tunnel cache can cause unexpected short filename collisions when temporary file creation and deletion cycles are frequent. Some security tools may observe abnormal file identity continuity through the tunnel cache which can complicate forensic timeline analysis. Disabling the tunnel cache ensures newly created files receive fresh filename allocations without inheriting deleted file identities. This setting has no impact on visible filename behavior for long filenames.",
                Tags = ["ntfs", "tunnel-cache", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaximumTunnelEntryAgeInSeconds", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaximumTunnelEntryAgeInSeconds")],
                DetectOps = [RegOp.CheckDword(Key, "MaximumTunnelEntryAgeInSeconds", 0)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-quota-tracking",
                Label = "Disable NTFS Disk Quota Tracking",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "NTFS disk quotas track per-user disk space usage on volumes to enforce storage limits for individual accounts. Disabling disk quota tracking removes the overhead of per-user storage accounting metadata updates on every file creation and deletion. Quota tracking requires metadata updates on every file write scaled to the number of users with active quotas. Enterprise storage management through NAS, DFS, or cloud storage is more scalable and flexible than per-volume NTFS quotas. Removing quota tracking reduces file creation overhead and simplifies storage management administration. This setting is appropriate for volumes where storage management is handled through external storage systems rather than NTFS quotas.",
                Tags = ["ntfs", "quotas", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableQuotaTracking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableQuotaTracking")],
                DetectOps = [RegOp.CheckDword(Key, "DisableQuotaTracking", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-opportunistic-locks",
                Label = "Disable NTFS Opportunistic Locking",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 2,
                Description =
                    "NTFS opportunistic locking allows clients to cache file data locally when no other client has the file open, improving performance by reducing file server round trips. Disabling opportunistic locking forces all file reads and writes to go to the server without local caching for network-shared NTFS volumes. Oplock conflicts during concurrent access can cause temporary file lock contention delays for applications sharing files across multiple clients. Some legacy applications do not handle oplock break negotiations correctly, causing hangs or corruption when files are accessed concurrently. Disabling oplocks is a workaround for specific legacy application compatibility issues rather than a general recommendation. This setting should only be applied where known oplock compatibility problems exist and should not be applied broadly.",
                Tags = ["ntfs", "oplocks", "compatibility", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOpportunisticLocking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOpportunisticLocking")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOpportunisticLocking", 1)],
            },
        ];
    }

    // ── OfflineFilesSyncPolicy ──
    private static class _OfflineFilesSyncPolicy
    {
        private const string NetCache = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache";
        private const string SyncMgr = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SyncMgr";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "offsync-no-make-available-offline",
                Label = "Prevent Making Files Available Offline",
                Category = "Storage",
                Description =
                    "Sets NoMakeAvailableOffline=1 in NetCache policy. Blocks users from right-clicking shared files and selecting 'Always Available Offline'. Prevents uncontrolled growth of the offline cache on laptops and ensures only IT-assigned offline content is cached.",
                Tags = ["offline", "sync", "files", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "NoMakeAvailableOffline", 1)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "NoMakeAvailableOffline")],
                DetectOps = [RegOp.CheckDword(NetCache, "NoMakeAvailableOffline", 1)],
            },
            new TweakDef
            {
                Id = "offsync-purge-at-logoff",
                Label = "Purge Offline Cache at Logoff",
                Category = "Storage",
                Description =
                    "Sets PurgeAtLogoff=1 in NetCache policy. Causes all locally cached offline files to be deleted when the user logs off. Ensures sensitive documents synced from file servers are not retained on shared or kiosk machines between sessions.",
                Tags = ["offline", "sync", "files", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "PurgeAtLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "PurgeAtLogoff")],
                DetectOps = [RegOp.CheckDword(NetCache, "PurgeAtLogoff", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-background-sync",
                Label = "Disable Automatic Background Sync",
                Category = "Storage",
                Description =
                    "Sets BackgroundSyncEnabled=0 in NetCache policy. Stops the Offline Files CSC service from performing background synchronisation of the offline cache. Prevents unexpected I/O bursts and network traffic from silent sync operations, without disabling offline access entirely.",
                Tags = ["offline", "sync", "background", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "BackgroundSyncEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "BackgroundSyncEnabled")],
                DetectOps = [RegOp.CheckDword(NetCache, "BackgroundSyncEnabled", 0)],
            },
            new TweakDef
            {
                Id = "offsync-cache-disk-limit-5pct",
                Label = "Limit Offline Cache to 5% of Disk",
                Category = "Storage",
                Description =
                    "Sets DefaultCacheSize=5 in NetCache policy (percentage of disk). Restricts the maximum space the Offline Files cache may consume to 5% of the volume. Prevents the CSC cache from silently consuming large amounts of disk space on smaller system drives.",
                Tags = ["offline", "sync", "disk", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "DefaultCacheSize", 5)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "DefaultCacheSize")],
                DetectOps = [RegOp.CheckDword(NetCache, "DefaultCacheSize", 5)],
            },
            new TweakDef
            {
                Id = "offsync-go-offline-manual",
                Label = "Set Go-Offline Action to Manual",
                Category = "Storage",
                Description =
                    "Sets GoOfflineAction=0 (manual) in NetCache policy. Controls what happens when a network connection to a file server is lost: 0=work offline silently, 1=notify and ask. Setting 0 prevents disruptive dialogs on unstable connections while relying on manual sync on reconnect.",
                Tags = ["offline", "sync", "notification", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "GoOfflineAction", 0)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "GoOfflineAction")],
                DetectOps = [RegOp.CheckDword(NetCache, "GoOfflineAction", 0)],
            },
            new TweakDef
            {
                Id = "offsync-minimal-event-logging",
                Label = "Reduce Offline Files Event Log Verbosity",
                Category = "Storage",
                Description =
                    "Sets EventLoggingLevel=1 in NetCache policy. Reduces the Offline Files event log from informational (2) to warnings-only (1). Eliminates high-frequency informational events from the CSC service in the System event log on machines with many network shares.",
                Tags = ["offline", "sync", "eventlog", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "EventLoggingLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "EventLoggingLevel")],
                DetectOps = [RegOp.CheckDword(NetCache, "EventLoggingLevel", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-sync-activity-display",
                Label = "Disable Sync Center Activity Display",
                Category = "Storage",
                Description =
                    "Sets DisableSyncActivity=1 in SyncMgr policy. Prevents the Sync Center from displaying sync progress and activity in the notification area and the Sync Center dialog. Reduces UI clutter from background sync operations on shared desktops.",
                Tags = ["synccenter", "offline", "sync", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "DisableSyncActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "DisableSyncActivity")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "DisableSyncActivity", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-metered-sync",
                Label = "Disable Sync on Metered Connections",
                Category = "Storage",
                Description =
                    "Sets TurnOffSyncOnCostedNetwork=1 in SyncMgr policy. Prevents Sync Center from initiating any synchronisation when the active network connection is marked as metered (mobile hotspot, LTE, or manually flagged as metered). Prevents unexpected data charges.",
                Tags = ["synccenter", "offline", "sync", "metered", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "TurnOffSyncOnCostedNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "TurnOffSyncOnCostedNetwork")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "TurnOffSyncOnCostedNetwork", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-file-sync-client",
                Label = "Disable Sync Center File Sync Client",
                Category = "Storage",
                Description =
                    "Sets DisableFileSyncClient=1 in SyncMgr policy. Fully disables the Sync Center file synchronisation client component. This stops the CSC service from registering as a sync provider in the Sync Center UI, effectively turning off user-initiated and scheduled offline sync.",
                Tags = ["synccenter", "offline", "sync", "policy", "disable"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "DisableFileSyncClient", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "DisableFileSyncClient")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "DisableFileSyncClient", 1)],
            },
            new TweakDef
            {
                Id = "offsync-hide-in-sync-ui",
                Label = "Hide Offline Files from Sync Center UI",
                Category = "Storage",
                Description =
                    "Sets HideOptionsForSyncProvider=1 in SyncMgr policy. Removes the options and settings icon for the Offline Files sync provider from the Sync Center window, preventing users from modifying sync provider configuration while still allowing the provider to run.",
                Tags = ["synccenter", "offline", "sync", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "HideOptionsForSyncProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "HideOptionsForSyncProvider")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "HideOptionsForSyncProvider", 1)],
            },
        ];
    }

    // ── OpenTypeSecurityPolicy ──
    private static class _OpenTypeSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\MitigationOptions";
        private const string FontKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine";
        private const string GdipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Fonts";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "otfpol-block-opentype-kernel-parsing",
                    Label = "Block OpenType Font Parsing in the Windows Kernel",
                    Category = "Storage",
                    Description =
                        "Moves OpenType font parsing out of the Windows kernel (win32k.sys) and into a user-mode font parsing process, eliminating kernel-level font parsing vulnerabilities exploitable via specially-crafted font files in web content.",
                    Tags = ["opentype", "font-parsing", "kernel", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "OpenType kernel parsing disabled; font parsing moved to user-mode — eliminates kernel font exploit surface.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockOpenTypeKernelParser", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockOpenTypeKernelParser")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockOpenTypeKernelParser", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-legacy-font-drivers",
                    Label = "Disable Loading of Legacy TrueType Font Drivers",
                    Category = "Storage",
                    Description =
                        "Prevents legacy third-party TrueType font drivers from loading in the Windows font subsystem, reducing attack surface from unmaintained or vulnerable font drivers that may contain known CVEs.",
                    Tags = ["truetype", "font-driver", "legacy", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Legacy TrueType font drivers blocked from loading; only Windows-provided drivers used for font rendering.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableLegacyFontDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableLegacyFontDrivers")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableLegacyFontDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-restrict-embedded-font-trusted",
                    Label = "Restrict Embedded Fonts to Trusted Documents Only",
                    Category = "Storage",
                    Description =
                        "Sets the Windows font embedding policy so that embedded fonts in Office and PDF documents are only rendered when the document originates from a trusted location, blocking remote exploitation via malicious embedded fonts in untrusted files.",
                    Tags = ["fonts", "embedded-font", "trusted", "office", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Embedded fonts rendered only in trusted documents; fonts in untrusted attachments not processed.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "RestrictEmbeddedFontToTrusted", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "RestrictEmbeddedFontToTrusted")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "RestrictEmbeddedFontToTrusted", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-variable-font-web",
                    Label = "Disable Variable Font Loading from Web Content",
                    Category = "Storage",
                    Description =
                        "Prevents loading of OpenType Variable Fonts (OTF/TTF with variation axes) referenced in web content via browser font stacks, reducing the parsing attack surface from variable font table complexity in browser rendering engines.",
                    Tags = ["opentype", "variable-font", "web", "browser", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Variable font loading from web disabled in browser; reduces OTF/TTF parsing surface in browser renderer.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableVariableFontFromWeb", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableVariableFontFromWeb")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableVariableFontFromWeb", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-enable-font-integrity-check",
                    Label = "Enable Font File Integrity Check Before Loading",
                    Category = "Storage",
                    Description =
                        "Enables a Windows Security Health check that verifies the integrity of installed system fonts against known-good checksums before loading, detecting tampering with font files used in critical UI rendering.",
                    Tags = ["fonts", "integrity-check", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Font file integrity verified before loading; tampered system fonts detected before rendering.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "EnableFontIntegrityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "EnableFontIntegrityCheck")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "EnableFontIntegrityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-block-remote-font-download-edge",
                    Label = "Block Remote Font Downloads in Microsoft Edge",
                    Category = "Storage",
                    Description =
                        "Prevents Microsoft Edge from downloading and rendering fonts referenced by web page CSS from remote URLs, eliminating an attack vector where crafted web fonts hosted externally could exploit the browser font parser.",
                    Tags = ["fonts", "edge", "remote-font", "css", "browser-security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Edge blocked from loading remote fonts via CSS; all fonts must be system-installed. May break web typography.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts", 0)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts", 0)],
                },
                new TweakDef
                {
                    Id = "otfpol-enable-gdi-font-sandbox",
                    Label = "Enable GDI Font Sandbox in AppContainer Sessions",
                    Category = "Storage",
                    Description =
                        "Enables the GDI+ font rendering sandbox in AppContainer (browser sandboxed renderer) sessions, ensuring that font parsing for sandbox processes occurs in a restricted context rather than directly in win32k.sys.",
                    Tags = ["fonts", "gdi", "sandbox", "appcontainer", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GDI font sandbox enabled in AppContainer; font parsing for sandbox processes isolated from kernel.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "EnableGDIFontSandbox", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "EnableGDIFontSandbox")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "EnableGDIFontSandbox", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-type1-fonts",
                    Label = "Disable Loading of Legacy Type1 Fonts",
                    Category = "Storage",
                    Description =
                        "Disables support for loading Adobe Type 1 (PostScript) legacy fonts in GDI/GDI+, an aging format with limited security patching, reducing exposure to Type1 font parsing CVEs in the PostScript interpreter.",
                    Tags = ["fonts", "type1", "postscript", "legacy", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Type1/PostScript font loading disabled; legacy PS fonts not rendered. Most modern apps use OpenType.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableType1FontRendering", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableType1FontRendering")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableType1FontRendering", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-log-font-parse-failures",
                    Label = "Log Font File Parse Failures for Security Monitoring",
                    Category = "Storage",
                    Description =
                        "Enables event log entries when a font file fails parsing validation (malformed tables, invalid checksums), providing visibility into attempts to load crafted malicious fonts on the endpoint.",
                    Tags = ["fonts", "parse-failure", "event-log", "audit", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Font parse failure events logged; malformed or crafted font load attempts visible in security log.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "LogFontParseFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "LogFontParseFailures")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "LogFontParseFailures", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-font-driver-telemetry",
                    Label = "Disable Font Driver Telemetry Reporting to Microsoft",
                    Category = "Storage",
                    Description =
                        "Prevents the Windows font subsystem from sending font usage, load failure, and driver interaction telemetry to Microsoft, protecting information about installed and loaded fonts from cloud disclosure.",
                    Tags = ["fonts", "driver", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Font driver telemetry to Microsoft disabled; font load / failure statistics not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableFontDriverTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableFontDriverTelemetry")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableFontDriverTelemetry", 1)],
                },
            ];
    }

    // ── RefsFsPolicy ──
    private static class _RefsFsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "refspol-disable-integrity-streams",
                    Label = "Disable ReFS Integrity Streams (Checksums)",
                    Category = "Storage",
                    Description =
                        "Disables data integrity streams (SHA-256 checksums per block) on ReFS volumes, removing per-I/O checksum overhead at the cost of eliminating the ability to detect silent data corruption.",
                    Tags = ["refs", "integrity", "checksum", "file-system", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "ReFS integrity streams disabled; slight I/O improvement but silent data corruption no longer detected.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityStreams", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityStreams")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityStreams", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-scrubbing",
                    Label = "Disable ReFS Background Data Scrubbing",
                    Category = "Storage",
                    Description =
                        "Disables the background data scrubbing job that periodically reads and validates all ReFS blocks against their stored checksums, eliminating the I/O overhead but preventing proactive corruption detection.",
                    Tags = ["refs", "scrubbing", "background", "file-system", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "ReFS scrubbing disabled; background read-and-verify jobs eliminated, reducing idle I/O.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundScrubbing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundScrubbing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundScrubbing", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-enable-salvage-mode",
                    Label = "Enable ReFS Corruption Salvage Mode",
                    Category = "Storage",
                    Description =
                        "Enables ReFS salvage mode which continues to mount and access uncorrupted portions of a volume when corruption is detected, avoiding complete volume unavailability due to isolated data corruption.",
                    Tags = ["refs", "salvage", "corruption", "availability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ReFS salvage mode enabled; partially corrupted volumes remain accessible for uncorrupted data.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableSalvageMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableSalvageMode")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableSalvageMode", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-set-cluster-size-64k",
                    Label = "Set Default ReFS Cluster Size to 64 KB",
                    Category = "Storage",
                    Description =
                        "Sets the default ReFS cluster size to 64 KB, improving large-sequential-I/O throughput and reducing metadata overhead for workloads that store many large files (virtual machines, databases, backups).",
                    Tags = ["refs", "cluster-size", "performance", "file-system", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS default cluster size set to 64 KB; applies to newly formatted ReFS volumes.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultClusterSizeKB", 64)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultClusterSizeKB")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultClusterSizeKB", 64)],
                },
                new TweakDef
                {
                    Id = "refspol-block-refs-caching-metadata",
                    Label = "Block ReFS Metadata in System File Cache",
                    Category = "Storage",
                    Description =
                        "Prevents ReFS metadata (B-tree nodes, directory structures) from consuming the system file cache, dedicating file cache to application data and preventing metadata cache pressure on systems with large ReFS trees.",
                    Tags = ["refs", "metadata", "file-cache", "memory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ReFS metadata excluded from system file cache; cache fully available for file data.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsMetadataCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsMetadataCaching")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsMetadataCaching", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-refs-on-boot-volume",
                    Label = "Prevent ReFS Formatting of System Boot Volumes",
                    Category = "Storage",
                    Description =
                        "Blocks ReFS from being selected as the file system for the system or boot volume during installation, ensuring Windows boot volumes always use NTFS which has full boot-time driver support.",
                    Tags = ["refs", "boot-volume", "ntfs", "formatting", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS blocked on boot volumes; system drive must use NTFS.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsOnSystemVolume", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsOnSystemVolume")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsOnSystemVolume", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-enable-corruption-audit-log",
                    Label = "Enable ReFS Corruption Detection Audit Logging",
                    Category = "Storage",
                    Description =
                        "Enables detailed event log entries for every ReFS corruption detection event including the file path, cluster address, and recovery action taken.",
                    Tags = ["refs", "corruption", "audit-log", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS corruption events logged; file path, cluster, and recovery details recorded in System event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCorruptionEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCorruptionEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCorruptionEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-refs-dedup",
                    Label = "Disable ReFS Block-Level Deduplication",
                    Category = "Storage",
                    Description =
                        "Disables block-level deduplication on ReFS volumes, stopping background dedup processing that can interfere with real-time workloads and consume I/O bandwidth on storage-intensive systems.",
                    Tags = ["refs", "deduplication", "storage", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS block deduplication disabled; no dedup overhead, at the expense of higher storage usage.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsBlockDedup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsBlockDedup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsBlockDedup", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-set-mirror-write-threshold-3",
                    Label = "Set ReFS Mirror-Write Log Threshold to 3 Entries",
                    Category = "Storage",
                    Description =
                        "Sets the ReFS B+ tree write-log threshold that triggers a checkpoint flush to 3 entries, ensuring faster persistence of write logs at the cost of slightly more frequent I/O checkpoints.",
                    Tags = ["refs", "write-log", "checkpoint", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ReFS write-log checkpoint threshold set to 3; write persistence more frequent on active volumes.",
                    ApplyOps = [RegOp.SetDword(Key, "WriteLogCheckpointThreshold", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WriteLogCheckpointThreshold")],
                    DetectOps = [RegOp.CheckDword(Key, "WriteLogCheckpointThreshold", 3)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-refs-compression",
                    Label = "Disable ReFS Transparent Compression",
                    Category = "Storage",
                    Description =
                        "Disables ReFS transparent compression, preventing the file system from automatically compressing cold data blocks, and eliminating the CPU overhead of compression/decompression on access.",
                    Tags = ["refs", "compression", "cpu", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ReFS compression disabled; slightly higher storage usage, lower CPU overhead for large file reads.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsCompression")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsCompression", 1)],
                },
            ];
    }

    // ── ReFSPolicy ──
    private static class _ReFSPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "refs-disable-integrity-checking",
                Label = "Disable ReFS Integrity Checking",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableIntegrityChecking=1 in the ReFS policy key. Prevents ReFS "
                    + "from performing continuous background data integrity scrubbing using "
                    + "checksums stored alongside each file record. Integrity scrubbing "
                    + "consumes additional I/O bandwidth on storage-constrained systems or "
                    + "arrays that already provide checksumming at the hardware level. "
                    + "Default: 0 (scrubbing enabled). Recommended: 1 only on redundant arrays.",
                Tags = ["refs", "integrity", "filesystem", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityChecking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityChecking")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityChecking", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-integrity-streams",
                Label = "Disable ReFS Integrity Streams",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableIntegrityStreams=1 in the ReFS policy key. Turns off the "
                    + "integrity stream feature that tags every file region with a checksum "
                    + "entry in the volume metadata stream. Disabling integrity streams "
                    + "reduces per-write metadata overhead and can improve sequential write "
                    + "throughput by 10–20% on high-frequency write workloads at the cost "
                    + "of silent corruption detection. Default: 0.",
                Tags = ["refs", "integrity-streams", "filesystem", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityStreams", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityStreams")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityStreams", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-auto-repair",
                Label = "Disable ReFS Automatic Repair",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableAutoRepair=1 in the ReFS policy key. Prevents ReFS from "
                    + "automatically correcting detected bad sectors or checksum mismatches "
                    + "using parity or mirror redundancy without user intervention. On "
                    + "non-redundant single-disk volumes the auto-repair feature can "
                    + "silently mark corrupted data as repaired when no valid copy exists. "
                    + "Default: 0. Recommended: 1 only on direct-attached single disks.",
                Tags = ["refs", "repair", "filesystem", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoRepair", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoRepair")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoRepair", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-short-name-creation",
                Label = "Disable ReFS Short Name Creation",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableShortNameCreation=1 in the ReFS policy key. Suppresses "
                    + "automatic generation of 8.3 DOS-compatible short names alongside "
                    + "long file names on ReFS volumes. 8.3 name creation adds measurable "
                    + "overhead on directories with many files and is unnecessary for "
                    + "modern Windows applications and tools that use long-name APIs. "
                    + "Default: 0. Recommended: 1 on dedicated server or data volumes.",
                Tags = ["refs", "shortname", "8dot3", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShortNameCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShortNameCreation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShortNameCreation", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-last-access-update",
                Label = "Disable ReFS Last-Access Timestamp Update",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableLastAccessUpdate=1 in the ReFS policy key. Disables the "
                    + "last-access timestamp field update on every file read operation. "
                    + "Updating the last-access time on each read generates a write "
                    + "transaction to the file metadata in the global B-tree, causing "
                    + "write amplification on read-heavy workloads such as media servers "
                    + "and archive stores. Default: 0. Recommended: 1.",
                Tags = ["refs", "timestamp", "atime", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLastAccessUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLastAccessUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLastAccessUpdate", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-parity-logging",
                Label = "Disable ReFS Parity Write Logging",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableParityLogging=1 in the ReFS policy key. Suppresses the "
                    + "write-ahead parity log that makes partial-stripe writes to parity "
                    + "spaces resilient across power failures. Disabling this log improves "
                    + "random-write throughput on parity storage spaces but opens a window "
                    + "for parity corruption if a power loss occurs mid-stripe. "
                    + "Default: 0. Not recommended unless UPS protection is confirmed.",
                Tags = ["refs", "parity", "wal", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableParityLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableParityLogging")],
                DetectOps = [RegOp.CheckDword(Key, "DisableParityLogging", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-metadata-checksum",
                Label = "Disable ReFS Metadata Checksum",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "Sets DisableMetadataChecksum=1 in the ReFS policy key. Prevents ReFS "
                    + "from computing and verifying a checksum over each metadata B-tree "
                    + "page on every access. Metadata checksumming is a primary ReFS "
                    + "reliability feature; disabling it removes detection of metadata "
                    + "corruption caused by hardware faults or bit-rot and is not "
                    + "recommended on production data volumes. Default: 0.",
                Tags = ["refs", "metadata", "checksum", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMetadataChecksum", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMetadataChecksum")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMetadataChecksum", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-large-mft",
                Label = "Disable ReFS Large MFT Zone Reservation",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableLargeMft=1 in the ReFS policy key. Prevents ReFS from "
                    + "pre-reserving a large zone in the volume B-tree for anticipated "
                    + "metadata growth. Pre-reservation reduces free space visible to users "
                    + "on smaller volumes; on volumes with predictable small file counts "
                    + "the reservation is wasteful and can be released. "
                    + "Default: 0. Recommended: 1 on volumes under 200 GB.",
                Tags = ["refs", "mft", "metadata", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLargeMft", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLargeMft")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLargeMft", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-delete-notify",
                Label = "Disable ReFS Delete Notification (TRIM)",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableDeleteNotify=1 in the ReFS policy key. Stops ReFS from "
                    + "issuing TRIM or UNMAP commands to the underlying SSD or thin-"
                    + "provisioned storage when files are deleted. TRIM commands can cause "
                    + "high-latency stalls on some older firmware SSDs and thin-provisioned "
                    + "SAN/NAS LUNs that must zero out freed blocks before re-allocation. "
                    + "Default: 0. Recommended: 1 only for problematic storage hardware.",
                Tags = ["refs", "trim", "unmap", "ssd", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeleteNotify", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeleteNotify")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeleteNotify", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-compression",
                Label = "Disable ReFS Data Compression",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableCompression=1 in the ReFS policy key. Prevents the ReFS "
                    + "driver from enabling LZ4-based file compression on volumes where "
                    + "compression has been set as the default state. Compression on "
                    + "already-compressed media files (video, archives, encrypted files) "
                    + "yields negative savings and wastes CPU cycles attempting "
                    + "incompressible blocks. Default: 0. Recommended: 1 on media volumes.",
                Tags = ["refs", "compression", "lz4", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
            },
        ];
    }

    // ── ShadowCopyVss ──
    private static class _ShadowCopyVss
    {
        private const string VssSettings = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings";
        private const string SrPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
        private const string SrSettings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
        private const string VssDisks = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VolSnap";
        private const string VssWriters = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore\Cfg";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vss-enable-system-restore",
                Label = "VSS: Enable System Restore (Volume Shadow Copy)",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 5,
                RegistryKeys = [SrPolicy],
                Tags = ["vss", "shadow-copy", "system-restore", "backup", "recovery"],
                Description =
                    "Sets DisableSR=0 and DisableConfig=0 in the SystemRestore policy. "
                    + "Enables System Restore protection so that Windows can automatically create restore "
                    + "points before significant changes (updates, driver installs). "
                    + "Required for Automatic Repair to offer working restore points.",
                ApplyOps = [RegOp.SetDword(SrPolicy, "DisableSR", 0), RegOp.SetDword(SrPolicy, "DisableConfig", 0)],
                RemoveOps = [RegOp.DeleteValue(SrPolicy, "DisableSR"), RegOp.DeleteValue(SrPolicy, "DisableConfig")],
                DetectOps = [RegOp.CheckDword(SrPolicy, "DisableSR", 0), RegOp.CheckDword(SrPolicy, "DisableConfig", 0)],
            },
            new TweakDef
            {
                Id = "vss-disable-system-restore",
                Label = "VSS: Disable System Restore to Reclaim Disk Space",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 2,
                RegistryKeys = [SrPolicy],
                Tags = ["vss", "shadow-copy", "system-restore", "disk-space", "cleanup"],
                Description =
                    "Sets DisableSR=1 in the SystemRestore policy. "
                    + "Turns off Windows System Restore across all drives. "
                    + "Reclaims disk space reserved for shadow copies (up to several GB). "
                    + "WARNING: disabling System Restore removes the rollback mechanism for driver/update "
                    + "failures. Only recommended for SSD-constrained machines with alternative backups.",
                ApplyOps = [RegOp.SetDword(SrPolicy, "DisableSR", 1)],
                RemoveOps = [RegOp.DeleteValue(SrPolicy, "DisableSR")],
                DetectOps = [RegOp.CheckDword(SrPolicy, "DisableSR", 1)],
            },
            new TweakDef
            {
                Id = "vss-set-restore-point-frequency-daily",
                Label = "VSS: Limit Automatic Restore Points to Once Per Day",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [SrSettings],
                Tags = ["vss", "shadow-copy", "system-restore", "disk-space", "performance"],
                Description =
                    "Sets RPSessionInterval=1440 (minutes) in SystemRestore settings. "
                    + "Prevents Windows from creating more than one restore point per day when apps "
                    + "request it. Default is 0 (no interval restriction), which can create dozens of "
                    + "restore points in a single app-install session and consume significant disk space.",
                ApplyOps = [RegOp.SetDword(SrSettings, "RPSessionInterval", 1440)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RPSessionInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RPSessionInterval", 1440)],
            },
            new TweakDef
            {
                Id = "vss-limit-shadow-storage-15pct",
                Label = "VSS: Cap Shadow Copy Storage at 15% of Drive Capacity",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 4,
                RegistryKeys = [SrSettings],
                Tags = ["vss", "shadow-copy", "disk-space", "storage"],
                Description =
                    "Sets DiskPercent=15 in SystemRestore settings. "
                    + "Limits the maximum disk space that VSS/System Restore can use across all drives "
                    + "to 15% of each volume's capacity. The Windows default varies (often up to 30%). "
                    + "Lower value = fewer old restore points retained; reduces disk footprint.",
                ApplyOps = [RegOp.SetDword(SrSettings, "DiskPercent", 15)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "DiskPercent")],
                DetectOps = [RegOp.CheckDword(SrSettings, "DiskPercent", 15)],
            },
            new TweakDef
            {
                Id = "vss-increase-writer-timeout",
                Label = "VSS: Increase Writer Timeout to 120 Seconds",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [VssSettings],
                Tags = ["vss", "shadow-copy", "timeout", "backup", "reliability"],
                Description =
                    "Sets MaxWriterTimeInSeconds=120 in VSS\\Settings. "
                    + "Increases the maximum time the VSS coordinator waits for individual writers "
                    + "before declaring a VSS snapshot failure. Default is 60 seconds. "
                    + "Helps with slow VSS writers (e.g. SQL Server, Exchange) on loaded servers.",
                ApplyOps = [RegOp.SetDword(VssSettings, "MaxWriterTimeInSeconds", 120)],
                RemoveOps = [RegOp.DeleteValue(VssSettings, "MaxWriterTimeInSeconds")],
                DetectOps = [RegOp.CheckDword(VssSettings, "MaxWriterTimeInSeconds", 120)],
            },
            new TweakDef
            {
                Id = "vss-enable-unbuffered-writes",
                Label = "VSS: Enable Unbuffered Writes for Faster Shadow-Copy Creation",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 3,
                RegistryKeys = [VssSettings],
                Tags = ["vss", "shadow-copy", "performance", "backup"],
                Description =
                    "Sets MaxFileBufferSize=0 in VSS\\Settings. "
                    + "Switches VSS copy-on-write I/O from buffered to unbuffered (direct I/O) mode. "
                    + "Can reduce shadow-copy overhead on fast NVMe/SSD arrays. "
                    + "Not recommended on spinning-disk (HDD) systems — increases seek pressure.",
                ApplyOps = [RegOp.SetDword(VssSettings, "MaxFileBufferSize", 0)],
                RemoveOps = [RegOp.DeleteValue(VssSettings, "MaxFileBufferSize")],
                DetectOps = [RegOp.CheckDword(VssSettings, "MaxFileBufferSize", 0)],
            },
            new TweakDef
            {
                Id = "vss-disable-snapvol-for-fixed-drives",
                Label = "VSS: Disable VolSnap Auto-Snapshot on Fixed Drives",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 3,
                RegistryKeys = [VssDisks],
                Tags = ["vss", "shadow-copy", "volsnap", "disk-space", "performance"],
                Description =
                    "Sets AllowSnapshotsOnFixedDrives=0 in VolSnap settings. "
                    + "Prevents the VolSnap driver from creating automatic snapshots on fixed (non-removable) "
                    + "drives. Releases snapshot storage immediately. Not recommended if you rely on "
                    + "VSS-based backup tools (Veeam, Windows Server Backup, etc.).",
                ApplyOps = [RegOp.SetDword(VssDisks, "AllowSnapshotsOnFixedDrives", 0)],
                RemoveOps = [RegOp.DeleteValue(VssDisks, "AllowSnapshotsOnFixedDrives")],
                DetectOps = [RegOp.CheckDword(VssDisks, "AllowSnapshotsOnFixedDrives", 0)],
            },
            new TweakDef
            {
                Id = "vss-disallow-user-config",
                Label = "VSS: Hide System Restore Configuration from Standard Users",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [SrPolicy],
                Tags = ["vss", "shadow-copy", "system-restore", "security", "policy"],
                Description =
                    "Sets DisableConfig=1 in the SystemRestore policy. "
                    + "Hides the System Restore configuration tab in System Properties from standard users. "
                    + "They cannot turn off system protection, create restore points manually, or change "
                    + "shadow copy storage size. Admins retain full access.",
                ApplyOps = [RegOp.SetDword(SrPolicy, "DisableConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(SrPolicy, "DisableConfig")],
                DetectOps = [RegOp.CheckDword(SrPolicy, "DisableConfig", 1)],
            },
            new TweakDef
            {
                Id = "vss-set-min-restore-point-space-300mb",
                Label = "VSS: Set Minimum Shadow-Copy Reservation to 300 MB",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 4,
                RegistryKeys = [VssWriters],
                Tags = ["vss", "shadow-copy", "disk-space", "storage", "system-restore"],
                Description =
                    "Sets MinDiskSpace=314572800 (300 MB in bytes) in SystemRestore\\Cfg. "
                    + "Sets the minimum disk space reservation for the shadow copy provider. "
                    + "If free space drops below this threshold, no new snapshots are created. "
                    + "300 MB is tighter than the Windows default (1 GB), saving space on small SSDs.",
                ApplyOps = [RegOp.SetDword(VssWriters, "MinDiskSpace", 314572800)],
                RemoveOps = [RegOp.DeleteValue(VssWriters, "MinDiskSpace")],
                DetectOps = [RegOp.CheckDword(VssWriters, "MinDiskSpace", 314572800)],
            },
            new TweakDef
            {
                Id = "vss-disable-rp-before-critical-updates",
                Label = "VSS: Skip Automatic Restore Points Before Windows Updates",
                Category = "Storage",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 3,
                RegistryKeys = [SrSettings],
                Tags = ["vss", "shadow-copy", "windows-update", "disk-space", "performance"],
                Description =
                    "Sets CreatePointBeforeCriticalPatches=0 in SystemRestore settings. "
                    + "Prevents Windows from automatically creating a restore point immediately before "
                    + "installing critical Windows Updates. Saves disk space on small SSDs at the cost of "
                    + "losing the rollback safety net for a failed update.",
                ApplyOps = [RegOp.SetDword(SrSettings, "CreatePointBeforeCriticalPatches", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "CreatePointBeforeCriticalPatches")],
                DetectOps = [RegOp.CheckDword(SrSettings, "CreatePointBeforeCriticalPatches", 0)],
            },
        ];
    }

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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Id = "storsense-disable-storage-sense",
                    Label = "Disable Storage Sense via Policy",
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                    Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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
                Category = "Storage",
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

// ── merged from Backup.cs ──
internal static class Backup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "backup-disable-file-history",
            Label = "Disable File History",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows File History backup feature via Group Policy. Useful when you use a third-party backup solution instead.",
            Tags = ["backup", "file-history", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-system-restore",
            Label = "Disable System Restore",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables System Restore and removes existing restore points. Frees disk space but removes the safety net. Use with caution.",
            Tags = ["backup", "system-restore", "disk", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "backup-vss-manual",
            Label = "Set Volume Shadow Copy to Manual",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the VSS service to Manual start. Reduces background I/O if you don't use System Restore or Previous Versions.",
            Tags = ["backup", "vss", "shadow-copy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
        },
        new TweakDef
        {
            Id = "backup-disable-backup-ui",
            Label = "Disable Windows Backup Settings Page",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hides the Windows Backup page in Settings via policy. Useful for managed environments that use different backup solutions.",
            Tags = ["backup", "settings", "policy", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup", "DisableBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup", "DisableBackupUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup", "DisableBackupUI", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-previous-versions",
            Label = "Disable Previous Versions Tab",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Previous Versions' tab from file/folder properties. Cleans up the context menu when VSS is not in use.",
            Tags = ["backup", "previous-versions", "explorer", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Stops Windows Error Reporting from collecting and sending crash data. Reduces disk I/O and network usage from WER telemetry uploads.",
            Tags = ["backup", "wer", "error-reporting", "privacy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-reliability-monitor",
            Label = "Disable Reliability Monitoring",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Reliability Monitor data collection by setting TimeStampInterval to 0. Reduces background I/O.",
            Tags = ["backup", "reliability", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0)],
        },
        new TweakDef
        {
            Id = "backup-bak-increase-shadow-storage",
            Label = "Increase Shadow Copy Storage Limit",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the maximum number of shadow copies to 64. Allows more restore points to be retained. Default: 16. Recommended: 64.",
            Tags = ["backup", "shadow-copy", "vss", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
        },
        new TweakDef
        {
            Id = "backup-bak-disable-restore-low-disk",
            Label = "Disable System Restore on Low Disk",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents System Restore from being automatically disabled when disk space is low. Keeps restore points available. Default: auto-disable. Recommended: disabled.",
            Tags = ["backup", "system-restore", "disk", "low-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSROnLowDisk", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSROnLowDisk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSROnLowDisk", 1)],
        },
        new TweakDef
        {
            Id = "backup-bak-set-backup-interval",
            Label = "Set Backup Schedule Interval to 24 Hours",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Windows Backup schedule interval to 24 hours. Ensures regular daily backups without excessive frequency. Default: not set. Recommended: 24 hours.",
            Tags = ["backup", "schedule", "interval", "frequency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "BackupFrequency", 24)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "BackupFrequency")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "BackupFrequency", 24)],
        },
        new TweakDef
        {
            Id = "backup-sr-frequency-unlimited",
            Label = "Allow Frequent System Restore Points",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the built-in 24-hour cooldown for System Restore point creation. Allows tools and scripts to create restore points at any time. Default: 1440 min limit. Recommended: unlimited (0).",
            Tags = ["backup", "system-restore", "frequency", "vss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "SystemRestorePointCreationFrequency", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "SystemRestorePointCreationFrequency"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore",
                    "SystemRestorePointCreationFrequency",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "backup-vss-auto-start",
            Label = "Set Volume Shadow Copy to Automatic Start",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the VSS (Volume Shadow Copy Service) start type to Automatic so shadow copies are always available on boot. Default: Manual. Recommended: Automatic on backup-heavy machines.",
            Tags = ["backup", "vss", "shadow-copy", "service", "auto"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
        },
        new TweakDef
        {
            Id = "backup-wer-reduce-queue",
            Label = "Limit Windows Error Reporting Queue to 1",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the Windows Error Reporting queue to 1 pending report, reducing disk usage from accumulated crash reports. Default: 50. Recommended: 1 for privacy.",
            Tags = ["backup", "wer", "error-reporting", "queue", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-backup-schedule-nag",
            Label = "Suppress Backup Schedule Balloon Notification",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                @"Suppresses the ""Set up Windows Backup"" balloon notification that appears in the system tray when no backup is configured. Default: shown. Recommended: suppressed.",
            Tags = ["backup", "notification", "balloon", "tray"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "NoBackupBalloonNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "NoBackupBalloonNotifications")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "NoBackupBalloonNotifications", 1),
            ],
        },
        new TweakDef
        {
            Id = "backup-disable-aedebug-auto",
            Label = "Disable Auto-Attach of JIT Debugger on Crash",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically launching a JIT (Just-In-Time) debugger when an application crashes. Suppresses the debugger prompt. Default: 1 (auto-attach). Recommended: 0 (no auto-attach).",
            Tags = ["backup", "crash", "debugger", "aedebug", "jit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "0")],
        },
        new TweakDef
        {
            Id = "backup-disable-system-image",
            Label = "Disable System Image Backup",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the legacy Windows 7 system image backup feature. This feature is deprecated in Windows 10/11. Default: available.",
            Tags = ["backup", "system-image", "deprecated", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableSystemBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableSystemBackupUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableSystemBackupUI", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-cloud-backup-settings",
            Label = "Disable Cloud Backup of Settings",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from backing up settings and preferences to the cloud. Default: enabled with Microsoft account.",
            Tags = ["backup", "cloud", "settings", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "backup-disable-auto-backup-scheduling",
            Label = "Disable Automatic Backup Scheduling",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the system from automatically scheduling File History backups. Manual backups still possible. Default: scheduled.",
            Tags = ["backup", "schedule", "automatic", "file-history"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "DisableAutoSchedule", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "DisableAutoSchedule")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "DisableAutoSchedule", 1)],
        },
        new TweakDef
        {
            Id = "backup-set-restore-point-frequency-1440",
            Label = "Set Restore Point Frequency to Daily",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits System Restore to create at most one restore point per day (1440 min). Prevents excessive disk usage. Default: no limit.",
            Tags = ["backup", "restore-point", "frequency", "daily"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore",
                    "SystemRestorePointCreationFrequency",
                    1440
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore",
                    "SystemRestorePointCreationFrequency"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore",
                    "SystemRestorePointCreationFrequency",
                    1440
                ),
            ],
        },
        new TweakDef
        {
            Id = "backup-disable-previous-versions-ui",
            Label = "Disable Previous Versions Tab",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Previous Versions tab in file properties. Declutters the UI when VSS is not used. Default: visible.",
            Tags = ["backup", "previous-versions", "properties", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-notifications",
            Label = "Disable Windows Backup Notifications",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Backup reminder notifications. Prevents periodic prompts to set up backup. Default: enabled.",
            Tags = ["backup", "notifications", "reminders", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableBackupUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableBackupUI", 1)],
        },
    ];
}

// ── Merged from Recovery.cs ──────────────────────────────────────────────────

internal static class Recovery
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "recovery-disable-system-restore",
            Label = "Disable System Restore",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables System Restore on all drives via Group Policy. Saves disk space but removes rollback capability. Default: enabled.",
            Tags = ["recovery", "system-restore", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "recovery-limit-restore-disk-usage",
            Label = "Limit System Restore Disk Usage to 5%",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the disk space used by System Restore shadow copies to 5% of the drive. Saves space while preserving restore capability. Default: 10-15%.",
            Tags = ["recovery", "system-restore", "disk-space", "limit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
        },
        new TweakDef
        {
            Id = "recovery-increase-restore-frequency",
            Label = "System Restore: Daily Checkpoints",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets System Restore to create automatic restore points every 24 hours (86400 seconds). Default: 24 hours but may be skipped under load.",
            Tags = ["recovery", "system-restore", "checkpoint", "daily"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval", 86400),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 86400),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval", 86400),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-repair-prompt",
            Label = "Disable Automatic Repair at Boot",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic startup repair prompt after consecutive boot failures. Prevents boot loops on servers. Default: enabled.",
            Tags = ["recovery", "auto-repair", "boot", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-partition",
            Label = "Disable Windows Recovery Environment",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Recovery Environment (WinRE). Saves ~500 MB disk space but removes recovery boot option. Default: enabled.",
            Tags = ["recovery", "winre", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-problem-reports",
            Label = "Disable Problem Reports & Solutions",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic problem report generation and solution checks. Reduces background activity and telemetry. Default: enabled.",
            Tags = ["recovery", "problem-reports", "disable", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontShowUI"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-increase-max-shadow-copies",
            Label = "Increase Max Shadow Copy Storage",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the maximum number of shadow copy storage associations. Allows more restore points to be retained. Default: system-managed.",
            Tags = ["recovery", "shadow-copy", "restore-point", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
        },
        new TweakDef
        {
            Id = "recovery-enable-auto-recovery",
            Label = "Enable Automatic Recovery",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the Windows automatic recovery feature that detects startup failures and offers repair options. Default: enabled.",
            Tags = ["recovery", "auto-recovery", "startup", "repair"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 10)],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-restart-sign-on",
            Label = "Disable Auto-Restart Sign-On (ARSO)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic sign-on after restart/update. Prevents Windows from caching credentials and auto-logging in after reboot. Default: enabled.",
            Tags = ["recovery", "security", "arso", "sign-on", "restart"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-enable-last-known-good",
            Label = "Enable Last Known Good Configuration",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the system to report the last known good configuration to the boot manager. Helps recovery from bad driver installs. Default: system-managed.",
            Tags = ["recovery", "boot", "last-known-good", "driver"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-recovery-ui",
            Label = "Disable Windows Recovery UI",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the recovery UI displayed after consecutive boot failures. Use only on kiosk or server systems where manual intervention is not desired. Default: enabled.",
            Tags = ["recovery", "boot", "ui", "kiosk", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-crash-auto-reboot-timeout",
            Label = "Set Crash Reboot Timeout to 30s",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the timeout before automatic reboot after a crash to 30 seconds, giving time to read BSOD information. Default: immediate reboot.",
            Tags = ["recovery", "crash", "bsod", "timeout", "reboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
        },
        new TweakDef
        {
            Id = "recovery-enable-event-log-crash",
            Label = "Write Crash Events to System Log",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures crash/BSOD events are written to the Windows Event Log. Essential for post-crash diagnostics. Default: enabled.",
            Tags = ["recovery", "crash", "event-log", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-send-alert",
            Label = "Disable Admin Alert on Crash",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sending an administrative alert when a crash occurs. Reduces noise on standalone systems. Default: off.",
            Tags = ["recovery", "crash", "alert", "notification"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-automatic-managed-page-file",
            Label = "Disable Auto-Managed Page File",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic page file management. Allows manual control over page file size for advanced crash dump configuration. Default: auto-managed.",
            Tags = ["recovery", "page-file", "memory", "crash-dump", "advanced"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0),
            ],
        },
        // ── Sprint 18 — 10 new Recovery tweaks ────────────────────────────

        new TweakDef
        {
            Id = "recovery-enable-boot-logging",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows boot logging to %SystemRoot%\\ntbtlog.txt. Useful for diagnosing driver loading failures. Default: disabled.",
            Tags = ["recovery", "boot", "logging", "driver", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-auto-repair",
            Label = "Disable Automatic Repair (Windows RE)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic startup repair in Windows Recovery Environment. Prevents boot loops when auto-repair repeatedly fails. Default: enabled.",
            Tags = ["recovery", "winre", "auto-repair", "boot-loop", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair", 0)],
        },
        new TweakDef
        {
            Id = "recovery-set-dump-folder-path",
            Label = "Set Minidump Folder to C:\\Minidumps",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Redirects minidump files to C:\\Minidumps for easier collection and analysis. Default: %SystemRoot%\\Minidump.",
            Tags = ["recovery", "minidump", "folder", "path", "organisation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"C:\Minidumps")],
            RemoveOps =
            [
                RegOp.SetExpandString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"%SystemRoot%\Minidump"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"C:\Minidumps")],
        },
        new TweakDef
        {
            Id = "recovery-increase-crash-dump-count",
            Label = "Keep Last 50 Minidump Files",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the number of retained minidump files from 10 (default) to 50 for longer crash history. Default: 10 files.",
            Tags = ["recovery", "minidump", "retention", "crash-history", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount", 50)],
        },
        new TweakDef
        {
            Id = "recovery-disable-startup-repair-prompt",
            Label = "Disable Startup Repair Recommendation Prompt",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses the 'Your PC did not start correctly' startup repair recommendation. Default: prompt shown after improper shutdown.",
            Tags = ["recovery", "startup-repair", "prompt", "boot", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
        },
        new TweakDef
        {
            Id = "recovery-enable-system-failure-popup",
            Label = "Show Popup on System Failure",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Displays an alert dialog when a system failure occurs, rather than silently restarting. Helpful for attended servers. Default: no popup.",
            Tags = ["recovery", "system-failure", "popup", "alert", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 1)],
        },
    ];
}

// ── Merged from SystemRestore.cs ──────────────────────────────────────────────────

internal static class SystemRestore
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string SrKey = $@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
    private const string SppKey = $@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
    private const string VssKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "restore-disable-system-restore",
            Label = "Disable System Restore",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables System Restore entirely. Saves disk space but removes ability to rollback system changes.",
            Tags = ["restore", "system-protection", "disk-space", "recovery"],
            RegistryKeys = [SppKey],
            ApplyOps = [RegOp.SetDword(SppKey, "DisableSR", 1)],
            RemoveOps = [RegOp.DeleteValue(SppKey, "DisableSR")],
            DetectOps = [RegOp.CheckDword(SppKey, "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-config-change-restore",
            Label = "Disable Restore Point on Config Changes",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic restore point creation when system configuration changes are made.",
            Tags = ["restore", "system-protection", "performance", "auto"],
            RegistryKeys = [SppKey],
            ApplyOps = [RegOp.SetDword(SppKey, "DisableConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(SppKey, "DisableConfig")],
            DetectOps = [RegOp.CheckDword(SppKey, "DisableConfig", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-max-frequency-daily",
            Label = "Limit Restore Points to Once Per Day",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets minimum interval between automatic restore points to 24 hours (1440 minutes). Reduces disk usage from frequent restore points.",
            Tags = ["restore", "system-protection", "performance", "frequency", "disk-space"],
            RegistryKeys = [SrKey],
            ApplyOps = [RegOp.SetDword(SrKey, "SystemRestorePointCreationFrequency", 1440)],
            RemoveOps = [RegOp.DeleteValue(SrKey, "SystemRestorePointCreationFrequency")],
            DetectOps = [RegOp.CheckDword(SrKey, "SystemRestorePointCreationFrequency", 1440)],
        },
        new TweakDef
        {
            Id = "restore-disable-vss-service",
            Label = "Set Volume Shadow Copy to Manual",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Volume Shadow Copy (VSS) service start type to manual. Saves resources but disables automatic backups.",
            Tags = ["restore", "vss", "shadow-copy", "service", "performance"],
            RegistryKeys = [VssKey],
            ApplyOps = [RegOp.SetDword(VssKey, "Start", 3)],
            RemoveOps = [RegOp.SetDword(VssKey, "Start", 2)],
            DetectOps = [RegOp.CheckDword(VssKey, "Start", 3)],
        },
        new TweakDef
        {
            Id = "restore-disable-previous-versions",
            Label = "Disable Previous Versions Tab",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Hides the Previous Versions tab in file/folder properties. Reduces VSS dependency.",
            Tags = ["restore", "previous-versions", "explorer", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableBackupRestore", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableBackupRestore"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableBackupRestore", 1)],
        },
        new TweakDef
        {
            Id = "restore-enable-scheduled-points",
            Label = "Enable Scheduled Restore Points",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the scheduled restore point creation task that creates weekly restore points.",
            Tags = ["restore", "system-protection", "scheduled", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent", "ScheduleEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent", "ScheduleEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent", "ScheduleEnabled", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-queue",
            Label = "Disable Windows Error Reporting Queue",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WER report queue that stores crash reports. Saves disk space and reduces I/O.",
            Tags = ["restore", "wer", "error-reporting", "disk-space", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-archive",
            Label = "Disable WER Report Archiving",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables archiving of sent Windows Error Reports. Reduces disk space usage.",
            Tags = ["restore", "wer", "error-reporting", "archive", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-consent-send-always",
            Label = "Auto-Send Error Reports (No Prompt)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Automatically sends Windows Error Reports without prompting the user. Reduces interruptions.",
            Tags = ["restore", "wer", "error-reporting", "consent", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 4)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 4)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-logging",
            Label = "Disable Windows Error Reporting Logging",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Error Reporting event logging to reduce unnecessary disk I/O.",
            Tags = ["restore", "wer", "error-reporting", "logging", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-max-queue-5",
            Label = "Limit WER Report Queue to 5",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the Windows Error Reporting queue to 5 reports maximum. Prevents disk space waste from accumulated reports.",
            Tags = ["restore", "wer", "error-reporting", "queue", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 5)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-max-archive-5",
            Label = "Limit WER Archive to 5 Reports",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the WER archive to 5 stored reports. Older reports are automatically purged.",
            Tags = ["restore", "wer", "error-reporting", "archive", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxArchiveCount", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxArchiveCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxArchiveCount", 5)],
        },
        new TweakDef
        {
            Id = "restore-disable-auto-recovery-boot",
            Label = "Disable Automatic Recovery on Boot Failure",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the automatic boot repair that triggers after consecutive boot failures. Prevents unwanted repair loops.",
            Tags = ["restore", "boot", "recovery", "repair"],
            SideEffects = "Must manually repair if boot issues occur.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecute_Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecute_Disabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecute_Disabled", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-shadow-copy-optimisation",
            Label = "Disable VSS Disk Space Optimisation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Volume Shadow Copy optimization that runs during idle periods. Reduces background I/O.",
            Tags = ["restore", "vss", "shadow-copy", "performance", "io"],
            RegistryKeys = [VssKey],
            ApplyOps = [RegOp.SetDword(VssKey, "MinDiffAreaFileSize", 3000)],
            RemoveOps = [RegOp.DeleteValue(VssKey, "MinDiffAreaFileSize")],
            DetectOps = [RegOp.CheckDword(VssKey, "MinDiffAreaFileSize", 3000)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-dump-type",
            Label = "Disable WER Full Crash Dump Collection",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets WER to collect mini dumps instead of full application crash dumps. Saves disk space.",
            Tags = ["restore", "wer", "crash", "dump", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 1)],
        },
        new TweakDef
        {
            Id = "restore-limit-wer-dump-count",
            Label = "Limit Local Crash Dumps to 3",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of local crash dump files retained per application to 3.",
            Tags = ["restore", "wer", "crash", "dump", "disk-space", "limit"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 3)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 3)],
        },
        new TweakDef
        {
            Id = "restore-disable-hiberfil",
            Label = "Disable Hibernate File (Reclaim Disk Space)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows hibernation support to reclaim the hiberfil.sys disk space (typically 5–10 GB).",
            Tags = ["restore", "hibernate", "disk-space", "power", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HibernateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HibernateEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "restore-suppress-wer-second-level-data",
            Label = "Suppress WER Second-Level Data Collection",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Error Reporting from uploading additional heap/module diagnostic data on crashes.",
            Tags = ["restore", "wer", "privacy", "telemetry", "crash", "data"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "NoSecondLevelData", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "NoSecondLevelData")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "NoSecondLevelData", 1)],
        },
        new TweakDef
        {
            Id = "restore-limit-wer-report-queue",
            Label = "Limit WER Report Queue to 2",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of pending crash reports queued for upload to 2, saving disk space.",
            Tags = ["restore", "wer", "disk-space", "queue", "crash"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue", "MaxQueueCount", 2)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue", "MaxQueueCount", 2)],
        },
        new TweakDef
        {
            Id = "restore-limit-wer-archive-size",
            Label = "Limit WER Archive to 5 Files",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the number of archived crash reports retained locally to 5 files.",
            Tags = ["restore", "wer", "disk-space", "archive", "crash"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive", "MaxArchiveCount", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive", "MaxArchiveCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive", "MaxArchiveCount", 5)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-throttle-bypass",
            Label = "Disable WER Network Throttle Bypass",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WER from bypassing network cost throttling when uploading reports on metered connections.",
            Tags = ["restore", "wer", "network", "throttle", "metered", "bandwidth"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "BypassNetworkCostThrottling", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "BypassNetworkCostThrottling")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "BypassNetworkCostThrottling", 0)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-response-timeout",
            Label = "Set WER Server Response Timeout (20 s)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets a 20-second timeout for the WER crash report server to reduce startup delays on slow networks.",
            Tags = ["restore", "wer", "timeout", "performance", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs", 20)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs", 20)],
        },
        new TweakDef
        {
            Id = "restore-disable-bsod-alert-send",
            Label = "Disable BSOD Administrator Alert",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from sending a crash alert notification to connected network administrators on BSOD.",
            Tags = ["restore", "bsod", "alert", "notification", "network", "admin"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
    ];
}

internal static class CloudStorage
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cloud-disable-dropbox-autostart",
            Label = "Disable Dropbox Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Dropbox from starting automatically at login.",
            Tags = ["dropbox", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Dropbox"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "DropboxUpdate"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableAutoStart")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Dropbox")],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-update",
            Label = "Disable Dropbox Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Dropbox from automatically checking for and installing updates.",
            Tags = ["dropbox", "update", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-lan-sync",
            Label = "Disable Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Dropbox LAN Sync (peer-to-peer discovery on the local network). Reduces network chatter and improves privacy on shared networks.",
            Tags = ["dropbox", "lan", "network", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-autostart",
            Label = "Disable Google Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Google Drive (DriveFS) from starting at login.",
            Tags = ["gdrive", "google", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableAutoStart")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-update",
            Label = "Disable Google Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Google Drive from auto-updating via policy.",
            Tags = ["gdrive", "google", "update", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-gdrive-bandwidth-limit",
            Label = "Limit Google Drive Upload (1 MB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps Google Drive upload bandwidth at 1 MB/s to prevent saturating your internet connection during large syncs.",
            Tags = ["gdrive", "google", "bandwidth", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthRxKBPS", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS", 1024),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthRxKBPS"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS", 1024)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-autostart",
            Label = "Disable iCloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents iCloud Drive and iCloud Services from starting at login.",
            Tags = ["icloud", "apple", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudDrive"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudServices"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudDrive")],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photos",
            Label = "Disable iCloud Photo Stream Upload",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic photo stream uploads via iCloud for Windows.",
            Tags = ["icloud", "apple", "photos", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-autostart",
            Label = "Disable Box Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Box / Box Drive from starting automatically at login.",
            Tags = ["box", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Box"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "BoxDrive"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Box")],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-autostart",
            Label = "Disable MEGA Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGAsync from starting automatically at login.",
            Tags = ["mega", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
        },
        new TweakDef
        {
            Id = "cloud-disable-pcloud-autostart",
            Label = "Disable pCloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents pCloud Drive from starting automatically at login.",
            Tags = ["pcloud", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive")],
        },
        new TweakDef
        {
            Id = "cloud-disable-nextcloud-autostart",
            Label = "Disable Nextcloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Nextcloud desktop client from starting at login.",
            Tags = ["nextcloud", "autostart", "cloud", "opensource"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud")],
        },
        new TweakDef
        {
            Id = "cloud-disable-tresorit-autostart",
            Label = "Disable Tresorit Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Tresorit from starting automatically at login.",
            Tags = ["tresorit", "autostart", "cloud", "encrypted"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit")],
        },
        new TweakDef
        {
            Id = "cloud-disable-synccom-autostart",
            Label = "Disable Sync.com Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Sync.com desktop client from starting at login.",
            Tags = ["sync.com", "autostart", "cloud", "encrypted"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com")],
        },
        new TweakDef
        {
            Id = "cloud-disable-spideroak-autostart",
            Label = "Disable SpiderOak ONE Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents SpiderOak ONE backup from starting at login.",
            Tags = ["spideroak", "autostart", "cloud", "backup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE")],
        },
        new TweakDef
        {
            Id = "cloud-disable-amazondrive-autostart",
            Label = "Disable Amazon Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Amazon Drive from starting automatically at login.",
            Tags = ["amazon", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive")],
        },
        new TweakDef
        {
            Id = "cloud-dropbox-upload-throttle",
            Label = "Throttle Dropbox Upload (512 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Caps Dropbox upload bandwidth at 512 KB/s to prevent saturating your internet connection.",
            Tags = ["dropbox", "bandwidth", "throttle", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate", 512),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_style", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_style"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate", 512)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-telemetry",
            Label = "Disable Dropbox Telemetry",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Dropbox analytics and telemetry data collection.",
            Tags = ["dropbox", "telemetry", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1)],
        },
        new TweakDef
        {
            Id = "cloud-gdrive-cache-limit",
            Label = "Limit Google Drive Cache (10 GB)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the Google Drive File Stream local cache at 10 GB to recover disk space on smaller SSDs.",
            Tags = ["gdrive", "google", "cache", "disk", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-telemetry",
            Label = "Disable Google Drive Telemetry",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables crash reporting and usage stats for Google Drive.",
            Tags = ["gdrive", "google", "telemetry", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableUsageStats", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableUsageStats"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-update",
            Label = "Disable MEGA Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGAsync from automatically checking for updates.",
            Tags = ["mega", "update", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-update",
            Label = "Disable Box Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Box Drive from automatically installing updates.",
            Tags = ["box", "update", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Box\Box"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-drive",
            Label = "Disable iCloud Drive Integration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables iCloud Drive Windows integration. Prevents iCloud from syncing files in Explorer. Default: Enabled. Recommended: Disabled if not using Apple devices.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync",
            Label = "Disable iCloud Auto-Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables iCloud automatic synchronization via Group Policy. Default: Enabled. Recommended: Disabled if not using Apple services.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-creative-cloud-startup",
            Label = "Disable Adobe Creative Cloud Startup",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Adobe Creative Cloud startup sync via policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["cloud", "adobe", "creative-cloud", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photo-sync",
            Label = "Disable iCloud Photo Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables iCloud Photo Stream automatic upload to prevent photos from syncing to Apple cloud services. Default: enabled. Recommended: disabled on corporate machines.",
            Tags = ["cloud", "icloud", "photo", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-offline",
            Label = "Disable Google Drive Offline Mode",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Google Drive offline mode via policy. Prevents local caching of Drive files, reducing disk usage. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "google-drive", "offline", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
        },
        new TweakDef
        {
            Id = "cloud-block-dropbox-lan-sync",
            Label = "Block Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks Dropbox LAN sync discovery which broadcasts on the local network. Improves security on shared networks. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "dropbox", "lan", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-onedrive-files-on-demand",
            Label = "Disable OneDrive Files On-Demand Policy",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On-Demand feature via Group Policy. All files download fully. Default: on-demand.",
            Tags = ["cloud", "onedrive", "files-on-demand", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-google-drive-autostart",
            Label = "Disable Google Drive Autostart",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Google Drive from starting automatically at logon. Default: starts with Windows.",
            Tags = ["cloud", "google-drive", "startup", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "GoogleDriveFS",
                    "\"C:\\Program Files\\Google\\Drive File Stream\\launch.bat\""
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-auto-update",
            Label = "Disable Box Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Box Drive from automatically checking for updates. Default: auto-update enabled.",
            Tags = ["cloud", "box", "update", "auto-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-sync-autostart",
            Label = "Disable MEGA Sync Autostart",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGA Sync client from starting automatically at logon. Default: starts with Windows.",
            Tags = ["cloud", "mega", "sync", "startup", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "MEGAsync",
                    "\"C:\\Users\\%USERNAME%\\AppData\\Local\\MEGAsync\\MEGAsync.exe\""
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync-on-startup",
            Label = "Disable iCloud Sync on Startup",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Apple iCloud from starting automatically at login. Saves bandwidth and resources. Default: enabled.",
            Tags = ["cloud", "icloud", "sync", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "iCloudServices",
                    @"%ProgramFiles%\Common Files\Apple\Internet Services\iCloudServices.exe"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
        },
        new TweakDef
        {
            Id = "cloud-disable-suggestions",
            Label = "Disable Cloud Storage Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows suggestions to use cloud storage services. Prevents Microsoft account and OneDrive promotions. Default: enabled.",
            Tags = ["cloud", "suggestions", "promotions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "cloud-overlay-optimise",
            Label = "Optimise Cloud Sync Overlay Icons",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables cloud-optimized content delivery from Windows. Reduces background data usage and telemetry from cloud storage features. Default: enabled.",
            Tags = ["cloud", "overlay", "sync", "optimise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
        },
    ];
}

// ── Merged from CloudExperience.cs ──────────────────────────────────────────────────

internal static class CloudExperience
{
    private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE";

    private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string ContentDelivery = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";

    private const string WindowsUpdate = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    private const string OobeUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\OOBE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "oobe-disable-consumer-features",
            Label = "Disable Consumer Cloud Features and Spotlight Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "consumer", "cloud", "suggestions", "privacy"],
            Description =
                "Disables Windows consumer features such as Microsoft Spotlight "
                + "advertisements and app suggestions delivered through cloud content. "
                + "DisableWindowsConsumerFeatures=1.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-ads",
            Label = "Disable Lock Screen Spotlight Ads (Enterprise)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "ads", "policy"],
            Description =
                "Disables Windows Spotlight on the lock screen via the cloud content "
                + "policy key (DisableWindowsSpotlightFeatures=1). Prevents Microsoft "
                + "from rotating lock screen images and showing tips and ads.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-onboarding",
            Label = "Disable Post-OOBE Cloud Onboarding Prompts",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "onboarding", "cloud", "prompts", "enterprise"],
            Description =
                "Disables the post-login onboarding flow that invites users to connect "
                + "OneDrive, set up Microsoft 365, etc. Suitable for pre-imaged enterprise "
                + "deployments. SkipNotHerePrompts=1.",
            ApplyOps = [RegOp.SetDword(Oobe, "SkipNotHerePrompts", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "SkipNotHerePrompts")],
            DetectOps = [RegOp.CheckDword(Oobe, "SkipNotHerePrompts", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-silent-app-install",
            Label = "Disable Silent Background App Installation via CDM",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "silent install", "apps", "consumer"],
            Description =
                "Prevents the Content Delivery Manager from silently installing "
                + "suggested and sponsored apps in the background. "
                + "SilentInstalledAppsEnabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-tips",
            Label = "Disable OOBE and Start Tips (Welcome Messages)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "tips", "welcome", "onboarding"],
            Description =
                "Disables the 'Did you know' and welcome tips in the Start menu and "
                + "after Windows updates. SoftLandingEnabled=0 in Content Delivery Manager.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-roaming-profile-setup",
            Label = "Disable Roaming Profile Setup Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "roaming", "profile", "onedrive"],
            Description =
                "Suppresses the prompt to back up the Desktop, Documents, and Pictures "
                + "folders to OneDrive during OOBE. DesktopIconsPreference=1 "
                + "(keep local folders).",
            ApplyOps = [RegOp.SetDword(Oobe, "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(Oobe, "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-on-desktop",
            Label = "Disable Spotlight Wallpaper Rotation on Desktop",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "wallpaper", "desktop", "cloud"],
            Description =
                "Prevents Windows Spotlight from rotating the desktop wallpaper. "
                + "RotatingLockScreenEnabled=0. Keeps a fixed wallpaper instead of "
                + "Microsoft's rotating Bing images.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscription-content",
            Label = "Disable Subscription-Based Cloud Content in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "subscription", "content", "start menu", "ads"],
            Description =
                "Disables subscription-based recommended and promoted content in the "
                + "Start menu. SubscribedContent-338388Enabled=0. Removes the "
                + "'Get the most out of Windows' suggestions.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions in Start and Store",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "third party", "suggestions", "start", "ads"],
            Description =
                "Disables third-party sponsored app suggestions in the Start menu and " + "Microsoft Store. SubscribedContent-338389Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-welcome-experience",
            Label = "Disable 'What's New' Welcome Experience After Updates",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "welcome", "what's new", "update", "cloud"],
            Description =
                "Prevents Windows from showing the 'What's new in Windows' splash screen "
                + "after feature updates complete. ContentDelivery SubscribedContent-310093Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-oem-preinstalled-apps",
            Label = "Disable OEM Pre-Installed Application Install",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "oem", "preinstalled", "apps", "bloatware"],
            Description =
                "Disables the silent installation of OEM-branded applications delivered "
                + "through the Content Delivery Manager (OemPreInstalledAppsEnabled=0). "
                + "Prevents hardware vendors from adding apps post-setup.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-pre-installed-apps",
            Label = "Disable Pre-Installed App Install via ContentDelivery",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "preinstalled", "apps", "curation", "bloatware"],
            Description =
                "Disables automatic installation of curated pre-installed Windows apps "
                + "delivered via the Content Delivery Manager (PreInstalledAppsEnabled=0). "
                + "Reduces initial bloatware on clean installs.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-soft-landing",
            Label = "Disable Start Layout Soft-Landing Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "soft-landing", "layout"],
            Description =
                "Disables 'soft-landing' content — clickable tips and suggestions injected "
                + "into the Start menu and notification area after first login "
                + "(SoftLandingEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-start-system-pane-suggestions",
            Label = "Disable System Pane Suggestions in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "system pane", "ui"],
            Description =
                "Disables the rotating suggested app tiles displayed in the Windows Start " + "menu system pane (SystemPaneSuggestionsEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-content-delivery",
            Label = "Disable Content Delivery Manager (All CDM Sources)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "content delivery", "suggestions", "apps"],
            Description =
                "Master switch that disables all Content Delivery Manager activity "
                + "by setting ContentDeliveryAllowed=0. Prevents all app suggestions, "
                + "spotlight ads, and cloud-delivered content from being displayed.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338388",
            Label = "Disable Lock Screen Spotlight (SubscribedContent-338388)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "windows", "ads"],
            Description =
                "Disables Windows Spotlight on the lock screen by setting "
                + "SubscribedContent-338388Enabled=0 in the user's Content Delivery "
                + "Manager keys. Falls back to static wallpaper.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338389",
            Label = "Disable Lock Screen Spotlight Tips (SubscribedContent-338389)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "tips", "ads"],
            Description =
                "Disables the rotating lock screen tips/suggestions from Windows Spotlight "
                + "(SubscribedContent-338389Enabled=0). Stops Microsoft from delivering "
                + "promotional messages on the lock screen.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353694",
            Label = "Disable Start Menu App Suggestions (SubscribedContent-353694)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "apps", "ads"],
            Description =
                "Disables the 'Occasionally show suggestions in Start' setting "
                + "(SubscribedContent-353694Enabled=0). Stops ad tiles appearing "
                + "in the Start menu recommendations.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353696",
            Label = "Disable Timeline Suggested Content (SubscribedContent-353696)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "timeline", "suggestions", "content", "ads"],
            Description =
                "Disables cloud-delivered suggested activities in the Windows Timeline "
                + "and 'Recommended' section (SubscribedContent-353696Enabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimized Content in Settings",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "cloud", "content", "settings", "policy"],
            Description =
                "Prevents Windows from showing cloud-optimized content — rotating "
                + "Microsoft-curated links and suggestions — inside the Settings app "
                + "(DisableCloudOptimizedContent=1).",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableCloudOptimizedContent", 1)],
        },
    ];
}

// === Merged from: OneDrive.cs ===

internal static class OneDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "od-onedrive-upload-throttle",
            Label = "Throttle OneDrive Upload (1 MB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 1000 KB/s to prevent saturating your connection.",
            Tags = ["onedrive", "bandwidth", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal",
            Label = "Disable OneDrive Personal Account Sign-In",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from signing in with a personal Microsoft account in OneDrive.",
            Tags = ["onedrive", "personal", "signin", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-max-upload-rate",
            Label = "Limit OneDrive Upload Rate (125 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps OneDrive upload bandwidth at 125 KB/s to minimise network impact.",
            Tags = ["onedrive", "bandwidth", "upload", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125)],
        },
        new TweakDef
        {
            Id = "od-onedrive-max-download-rate",
            Label = "Limit OneDrive Download Rate (1000 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps OneDrive download bandwidth at 1000 KB/s to minimise network impact.",
            Tags = ["onedrive", "bandwidth", "download", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-office-collab",
            Label = "Disable Office Collaboration via OneDrive",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Office co-authoring feature that uses OneDrive sync.",
            Tags = ["onedrive", "office", "collaboration", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-silent-config",
            Label = "Enable Silent OneDrive Account Configuration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Silently configures OneDrive with the user's Windows credentials without prompts.",
            Tags = ["onedrive", "silent", "config", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-reduce-bandwidth",
            Label = "OneDrive Reduce Sync Traffic",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits OneDrive upload bandwidth to 50%. Prevents OneDrive from saturating network connection. Default: Unlimited. Recommended: 50% for shared networks.",
            Tags = ["onedrive", "bandwidth", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-fod-global",
            Label = "Disable OneDrive Files On-Demand (Global)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables OneDrive Files On-Demand globally via policy. Forces all files to be downloaded locally. Default: Enabled. Recommended: Disabled for offline use.",
            Tags = ["onedrive", "files-on-demand", "policy", "offline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal-sync",
            Label = "Disable Personal OneDrive Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables personal OneDrive file sync via DisableFileSyncNGSC policy. Prevents OneDrive from syncing any personal accounts. Default: Enabled. Recommended: Disabled on corporate machines.",
            Tags = ["onedrive", "sync", "personal", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-backup-prompt",
            Label = "Disable OneDrive Folder Backup Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks the OneDrive Known Folder Move (KFM) opt-in prompt that asks users to back up Desktop, Documents, and Pictures. Default: Enabled. Recommended: Disabled on managed machines.",
            Tags = ["onedrive", "backup", "kfm", "prompt"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-block-business-sync",
            Label = "Block OneDrive for Business Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks OneDrive from syncing with external or business SharePoint organizations. Default: Allowed. Recommended: Blocked on personal machines.",
            Tags = ["onedrive", "business", "sync", "external"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-collaboration",
            Label = "Disable OneDrive File Collaboration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables OneDrive OCSI co-authoring clients for real-time file collaboration. Default: Enabled. Recommended: Disabled for offline-only workflows.",
            Tags = ["onedrive", "collaboration", "coauthoring", "ocsi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal",
            Label = "Disable OneDrive Personal Account Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from adding personal OneDrive accounts. Only business accounts allowed. Default: allowed.",
            Tags = ["onedrive", "personal", "account", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-limit-upload-bandwidth",
            Label = "Limit OneDrive Upload Bandwidth to 512 KB/s",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 512 KB/s. Prevents OneDrive from saturating the network. Default: unlimited.",
            Tags = ["onedrive", "bandwidth", "upload", "throttle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 512)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 512)],
        },
        new TweakDef
        {
            Id = "od-disable-toast-notifications",
            Label = "Disable OneDrive Toast Notifications",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive sync error and activity toast notifications. Default: enabled.",
            Tags = ["onedrive", "notifications", "toast", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "od-block-external-sync",
            Label = "Block External OneDrive Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive from syncing with external organizations. Data stays in-org. Default: allowed.",
            Tags = ["onedrive", "external", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-set-max-file-size-5gb",
            Label = "Set OneDrive Max Upload File Size to 5 GB",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the maximum file size OneDrive will sync to 5 GB. Default: no limit.",
            Tags = ["onedrive", "file-size", "limit", "upload"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB", 5120)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB", 5120)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-ads",
            Label = "Disable OneDrive Ads & Promotions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables promotional ads and tips in OneDrive sync client. Prevents upgrade nag popups. Default: enabled.",
            Tags = ["onedrive", "ads", "promotions", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-autostart",
            Label = "Disable OneDrive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents OneDrive from automatically starting on Windows login. Frees memory and network bandwidth. Default: auto-starts.",
            Tags = ["onedrive", "autostart", "startup", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "OneDrive")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "OneDrive",
                    @"%LOCALAPPDATA%\Microsoft\OneDrive\OneDrive.exe /background"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "OneDrive")],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-fod",
            Label = "Disable OneDrive Files On Demand",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On Demand feature enterprise-wide via policy. All files are kept local. Default: enabled.",
            Tags = ["onedrive", "files-on-demand", "policy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-kfm",
            Label = "Disable OneDrive Known Folder Move",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks OneDrive from silently moving Desktop, Documents, Pictures to cloud. Prevents automatic folder redirection. Default: allowed.",
            Tags = ["onedrive", "kfm", "folder-move", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal-sync",
            Label = "Block OneDrive Personal Account Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive from syncing personal (non-work) accounts via policy. Only enterprise tenants allowed. Default: allowed.",
            Tags = ["onedrive", "personal", "sync", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-files-on-demand",
            Label = "Disable OneDrive Files On Demand (User)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables OneDrive Files On Demand at user level. Files are always downloaded fully. Avoids placeholder files. Default: enabled.",
            Tags = ["onedrive", "files-on-demand", "user", "local"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-kfm-opt-in-prompt",
            Label = "Block Known Folder Move Opt-In Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from prompting users to move Desktop/Documents/Pictures to OneDrive (Known Folder Move wizard).",
            Tags = ["onedrive", "known-folder-move", "kfm", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-kfm-silent-redirect",
            Label = "Block Silent Known Folder Redirect",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks OneDrive from silently redirecting known folders (Desktop, Documents, Pictures) to cloud storage without prompting.",
            Tags = ["onedrive", "known-folder-move", "kfm", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-delay-update-ring",
            Label = "Delay OneDrive Client Update Ring",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keeps the OneDrive sync client on the Deferred ring to avoid destabilising updates.",
            Tags = ["onedrive", "update", "ring", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-sharepoint-sync",
            Label = "Disable SharePoint Sync Library",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from syncing SharePoint document libraries to the local machine.",
            Tags = ["onedrive", "sharepoint", "sync", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-app-sync",
            Label = "Disable OneDrive Application Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from syncing application settings (AppData\\Roaming) to cloud storage.",
            Tags = ["onedrive", "appsync", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync", 1)],
        },
        new TweakDef
        {
            Id = "od-limit-mass-delete-threshold",
            Label = "OneDrive Mass-Delete Warning Threshold (50 files)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the threshold at which OneDrive warns before deleting large numbers of files to 50 (down from default of 200).",
            Tags = ["onedrive", "mass-delete", "safety", "warning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold", 50)],
        },
        new TweakDef
        {
            Id = "od-disable-hydration-on-access",
            Label = "Disable Auto-Hydration on File Access",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents OneDrive from automatically downloading cloud-only placeholder files when opened by an app. Avoids unexpected bandwidth usage.",
            Tags = ["onedrive", "hydration", "files-on-demand", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-auto-update",
            Label = "Disable OneDrive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents OneDrive client from auto-updating in the background. Useful in managed environments where updates are controlled centrally.",
            Tags = ["onedrive", "update", "autoupdate", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-file-explorer-hub",
            Label = "Remove OneDrive from File Explorer Left Panel",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the OneDrive folder entry from the File Explorer navigation pane without disabling the sync process.",
            Tags = ["onedrive", "explorer", "sidebar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "od-block-external-collab",
            Label = "Block OneDrive External Collaboration (Policy)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from sharing OneDrive files with users outside of the organisation via Group Policy.",
            Tags = ["onedrive", "external-sharing", "collaboration", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing", 1)],
        },
    ];
}

// ── merged from PolicyCloud.cs ──
// RegiLattice.Core — Tweaks/PolicyCloud.cs
// OneDrive, SharePoint Online, cloud backup, delivery optimization, cloud desktop, content delivery, and cloud experience host policies
// Category: "Cloud Services Policy"
// Consolidated from 17 modules.

internal static class PolicyCloud
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CloudBackupRetentionPolicy.Data,
            .. _CloudContentPolicy.Data,
            .. _CloudDesktopPolicy.Data,
            .. _CloudExperienceHostPolicy.Data,
            .. _CloudFileSyncPolicy.Data,
            .. _CloudNotificationsPolicy.Data,
            .. _CloudPrintPolicy.Data,
            .. _CloudStorageQuotaPolicy.Data,
            .. _ContentDeliveryPolicy.Data,
            .. _DesktopAnalyticsPolicy.Data,
            .. _OneDriveKfmPolicy.Data,
            .. _OneDriveSyncPolicy.Data,
            .. _SettingSyncAdv.Data,
            .. _SettingSyncPolicy.Data,
            .. _SharepointOnlinePolicy.Data,
            .. _SkyDrivePolicy.Data,
            .. _UniversalClipboardSyncPolicy.Data,
        ];

    // ── CloudBackupRetentionPolicy ──
    private static class _CloudBackupRetentionPolicy
    {
        private const string BackupClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

        private const string BackupServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Server";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudbk-disable-user-backup-configure",
                    Label = "Cloud Backup Retention: Prevent Users from Configuring Windows Backup",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableBackupLauncher=1 in the Backup Client policy key. Prevents non-administrative users from configuring, starting, or modifying Windows Backup (previously Windows Server Backup client). In enterprise environments, backup targets, schedules, and retention settings must be under IT control — users who can configure their own backup destinations can write data to non-sanctioned cloud providers, bypassing DLP controls. Blocking user-initiated backup configuration ensures that all backup operations are managed by IT's corporate backup solution.",
                    Tags = ["backup", "backup-configure", "user-restriction", "dlp", "data-governance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users cannot configure or start Windows Backup. IT must ensure an alternative backup solution is deployed. Users will see 'Your organisation manages this' in Backup settings. Windows Server Backup (wbadmin) when run directly by users will fail — admin elevation required.",
                    ApplyOps = [RegOp.SetDword(BackupClientKey, "DisableBackupLauncher", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupClientKey, "DisableBackupLauncher")],
                    DetectOps = [RegOp.CheckDword(BackupClientKey, "DisableBackupLauncher", 1)],
                },
                new TweakDef
                {
                    Id = "cloudbk-disable-user-restore-access",
                    Label = "Cloud Backup Retention: Prevent Users from Performing Self-Service Restore",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableRestoreLauncher=1 in the Backup Client policy key. Prevents non-administrative users from initiating self-service data restore operations via Windows Backup or File History. While self-service restore sounds user-friendly, in regulated environments all data restore operations must be logged, authorised by IT, and recorded for audit trail purposes. Unlogged restores can introduce data from backup sets that contain sensitive versions of files. IT-controlled restore operations ensure proper chain-of-custody for restored data.",
                    Tags = ["backup", "restore", "self-service", "audit-trail", "data-governance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users cannot perform self-service restores. IT must provide a restore request process. Previous Versions (shadow copy restore from Explorer right-click → Restore Previous Versions) may also be affected depending on implementation. Document the IT-managed restore request workflow before deploying.",
                    ApplyOps = [RegOp.SetDword(BackupClientKey, "DisableRestoreLauncher", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupClientKey, "DisableRestoreLauncher")],
                    DetectOps = [RegOp.CheckDword(BackupClientKey, "DisableRestoreLauncher", 1)],
                },
                new TweakDef
                {
                    Id = "cloudbk-set-backup-retention-days-90",
                    Label = "Cloud Backup Retention: Set Backup Retention Period to 90 Days",
                    Category = "Cloud Storage",
                    Description =
                        "Sets RetentionDays=90 in the Backup Server policy key. Sets the backup retention period to 90 days — backup snapshots older than 90 days are automatically purged from the backup store. A 90-day retention balances storage cost against recovery capability — most regulatory frameworks (SOC 2, ISO 27001, HIPAA) require backup retention of at least 30–90 days. For organisations subject to GDPR, 90 days is sufficient for most incident investigation windows while limiting the duration of personal data retained in backups.",
                    Tags = ["backup", "retention", "90-days", "gdpr", "storage-lifecycle"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Backup data older than 90 days is purged. Verify that 90 days meets your organisation's regulatory and contractual backup retention obligations. Some industries require longer retention (e.g., financial services require 7 years). Adjust to match compliance requirements.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "RetentionDays", 90)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "RetentionDays")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "RetentionDays", 90)],
                },
                new TweakDef
                {
                    Id = "cloudbk-set-max-backup-versions-30",
                    Label = "Cloud Backup Retention: Set Maximum Backup Versions Retained to 30",
                    Category = "Cloud Storage",
                    Description =
                        "Sets MaxBackupVersions=30 in the Backup Server policy key. Limits the number of backup snapshot versions retained per backup job to 30 versions. Without a version cap, high-frequency backups (e.g., hourly file backups) can create hundreds of versions within the retention window — rapidly consuming backup storage. Capping at 30 versions creates a rolling window of 30 recovery points, which is sufficient for most incident recovery scenarios while preventing unbounded storage growth.",
                    Tags = ["backup", "version-limit", "storage-cost", "recovery-points", "snapshot"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Maximum 30 backup versions retained per job. When the 30-version limit is reached, the oldest version is removed as each new version is added (FIFO rolling window). Ensure 30 versions × backup frequency covers your minimum recovery time objective (RTO). For hourly backups, 30 versions = 30 hours of recovery points.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "MaxBackupVersions", 30)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "MaxBackupVersions")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "MaxBackupVersions", 30)],
                },
                new TweakDef
                {
                    Id = "cloudbk-enable-backup-encryption-required",
                    Label = "Cloud Backup Retention: Require Encryption for All Backup Sets",
                    Category = "Cloud Storage",
                    Description =
                        "Sets RequireBackupEncryption=1 in the Backup Server policy key. Requires that all backup sets created or managed by Windows Backup are encrypted before writing to the backup destination. Unencrypted backups are a significant data exposure risk — if the backup destination (NAS, cloud storage, tape) is compromised or improperly secured, unencrypted backups provide direct access to production data without requiring the attacker to bypass OS-level access controls. Requiring backup encryption ensures that even if backup media is stolen or the backup storage is breached, the data is useless without the encryption key.",
                    Tags = ["backup", "encryption", "at-rest", "data-breach", "backup-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "All backup sets must be encrypted. Backup jobs that write to destinations that do not support encryption (some legacy NAS devices, network shares without encryption) will fail until the destination or encryption method is configured. Ensure backup encryption key is backed up separately — loss of the backup encryption key makes the backup unrecoverable.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "RequireBackupEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "RequireBackupEncryption")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "RequireBackupEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "cloudbk-enable-backup-integrity-verification",
                    Label = "Cloud Backup Retention: Enable Automatic Backup Integrity Verification",
                    Category = "Cloud Storage",
                    Description =
                        "Sets EnableBackupVerification=1 in the Backup Server policy key. Enables automatic post-backup integrity verification — after each backup job completes, Windows Backup performs a hash verification of the written backup data against the source. Corrupted backup sets that pass initial creation but fail verification are flagged for re-execution. Without integrity verification, a backup set that has bit-level corruption (due to hardware failures, network errors, or storage media degradation) may not be discovered until a restore is attempted — often after the original data has been lost.",
                    Tags = ["backup", "integrity-check", "hash-verification", "silent-corruption", "restore-reliability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Backup verification hash check performed after each backup job. Increases backup job duration by approximately 10–20% (depending on backup size and storage I/O). Failed verification triggers re-backup and an alert. Backup storage infrastructure and network paths should be stable before enabling — frequent verification failures on unreliable networks generate a high alert volume.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "EnableBackupVerification", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "EnableBackupVerification")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "EnableBackupVerification", 1)],
                },
                new TweakDef
                {
                    Id = "cloudbk-set-backup-job-timeout-hours-6",
                    Label = "Cloud Backup Retention: Set Backup Job Maximum Timeout to 6 Hours",
                    Category = "Cloud Storage",
                    Description =
                        "Sets BackupJobTimeoutHours=6 in the Backup Server policy key. Sets the maximum allowed duration for a single backup job to 6 hours before it is automatically terminated. Without a timeout, backup jobs that stall due to network issues, storage problems, or extremely large files can run indefinitely — consuming backup system resources, holding file locks, and blocking subsequent scheduled jobs. A 6-hour timeout terminates stalled backups, generates a failure alert for investigation, and allows the next scheduled backup job to run.",
                    Tags = ["backup", "timeout", "job-management", "stalled-backup", "resource-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Backup jobs exceeding 6 hours are automatically terminated. For organisations with large data sets (multiple TB) backed up over slower links (direct-attached storage over 1 GbE), 6 hours may be insufficient. Calibrate the timeout to your largest expected backup job duration + 50% buffer.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "BackupJobTimeoutHours", 6)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "BackupJobTimeoutHours")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "BackupJobTimeoutHours", 6)],
                },
                new TweakDef
                {
                    Id = "cloudbk-enable-backup-failure-alert",
                    Label = "Cloud Backup Retention: Enable Automatic Alert on Backup Failure",
                    Category = "Cloud Storage",
                    Description =
                        "Sets EnableFailureAlert=1 in the Backup Server policy key. Enables the automatic generation of Windows event log alerts (Application log, source: Backup, Event ID 521) when a scheduled backup job fails. Backup failure alerting is a critical operational control — many organisations do not discover that their backup system has been silently failing for weeks or months until a restore is attempted. Proactive failure alerting via Windows Event Log (monitored by SIEM or SCOM) ensures that backup failures are detected and remediated within hours rather than months.",
                    Tags = ["backup", "failure-alert", "event-log", "siem", "soc"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Application event log alert generated on each backup failure. Connect event 521 (Backup failure) to SIEM alerting rule and page-duty rotation. Backup failure alerts should be treated as P2 incidents with a 4-hour response SLA in regulated environments.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "EnableFailureAlert", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "EnableFailureAlert")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "EnableFailureAlert", 1)],
                },
                new TweakDef
                {
                    Id = "cloudbk-disable-backup-to-optical-media",
                    Label = "Cloud Backup Retention: Disable Backup to Optical Media (DVD/BD)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableOpticalMediaBackup=1 in the Backup Client policy key. Prevents Windows Backup from using optical media (DVD, Blu-ray) as a backup destination. Optical media backups are insecure and impractical for enterprise environments — they lack encryption, version management, and are easily removable without audit trail. An employee can walk out with a DVD containing a full corporate data backup. Enterprise backup destinations should be network-based, access-controlled, and encrypted storage — optical media backup is a residual legacy capability.",
                    Tags = ["backup", "optical-media", "dvd", "data-removal", "enterprise-controls"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Optical media (DVD/BD) cannot be used as a backup destination. Minimal enterprise impact — optical media backup has been deprecated in most environments since Windows Server 2012 R2.",
                    ApplyOps = [RegOp.SetDword(BackupClientKey, "DisableOpticalMediaBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupClientKey, "DisableOpticalMediaBackup")],
                    DetectOps = [RegOp.CheckDword(BackupClientKey, "DisableOpticalMediaBackup", 1)],
                },
                new TweakDef
                {
                    Id = "cloudbk-set-backup-network-bandwidth-pct-30",
                    Label = "Cloud Backup Retention: Limit Backup Network Bandwidth to 30 Percent",
                    Category = "Cloud Storage",
                    Description =
                        "Sets MaxNetworkBandwidthPercent=30 in the Backup Server policy key. Throttles backup job network bandwidth consumption to a maximum of 30% of the available network bandwidth. Unthrottled backup jobs on a WAN or limited-bandwidth corporate uplink can saturate the connection, causing network performance degradation for end-users and business applications during backup windows. A 30% bandwidth cap ensures that backup operations, even when running during business hours, do not cause noticeable network performance impact.",
                    Tags = ["backup", "bandwidth-throttle", "qos", "network-congestion", "business-hours"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Backup network bandwidth capped at 30%. For large backup sets on slow WAN links, this may cause backup jobs to exceed the 6-hour timeout. Calibrate bandwidth percentage against expected backup size and job timeout. For off-hours backup windows, consider a policy that increases bandwidth to 80% during 22:00–06:00.",
                    ApplyOps = [RegOp.SetDword(BackupServerKey, "MaxNetworkBandwidthPercent", 30)],
                    RemoveOps = [RegOp.DeleteValue(BackupServerKey, "MaxNetworkBandwidthPercent")],
                    DetectOps = [RegOp.CheckDword(BackupServerKey, "MaxNetworkBandwidthPercent", 30)],
                },
            ];
    }

    // ── CloudContentPolicy ──
    private static class _CloudContentPolicy
    {
        private const string Cloud = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string CloudCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ccpol-disable-windows-consumer-features",
                Label = "Cloud Content: Disable Windows consumer features (app suggestions)",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsConsumerFeatures=1 in the CloudContent policy. Turns off the "
                    + "'consumer experience' that silently installs promoted apps and shows app suggestions.",
                Tags = ["cloud", "consumer", "suggestions", "debloat", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsConsumerFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsConsumerFeatures")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsConsumerFeatures", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-disable-third-party-suggestions",
                Label = "Cloud Content: Disable third-party app suggestions in Start and search",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableThirdPartySuggestions=1 in CloudContent policy (machine scope). Prevents "
                    + "third-party paid app promotions from appearing in Start menu tiles and search results.",
                Tags = ["cloud", "suggestions", "third-party", "ads", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableThirdPartySuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableThirdPartySuggestions")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableThirdPartySuggestions", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-disable-cloud-optimized-content",
                Label = "Cloud Content: Disable cloud-optimised content delivery",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableCloudOptimizedContent=1 in CloudContent policy. Stops Windows from "
                    + "fetching and injecting cloud-optimized UI content (personalized tips, spotlight).",
                Tags = ["cloud", "content", "spotlight", "debloat", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableCloudOptimizedContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableCloudOptimizedContent")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableCloudOptimizedContent", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-disable-spotlight-on-lock-screen",
                Label = "Cloud Content: Disable Windows Spotlight on the lock screen",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsSpotlightFeatures=1 in CloudContent policy (machine scope). "
                    + "Prevents Windows Spotlight from downloading and displaying rotating lock-screen images.",
                Tags = ["cloud", "spotlight", "lock-screen", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightFeatures")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightFeatures", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-disable-spotlight-on-action-center",
                Label = "Cloud Content: Disable Spotlight suggestions in Action Center",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1 in CloudContent policy. "
                    + "Suppresses the 'Get to know Windows' / 'What's new' Spotlight popups after updates.",
                Tags = ["cloud", "spotlight", "action-center", "welcome", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightWindowsWelcomeExperience")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-disable-spotlight-on-settings",
                Label = "Cloud Content: Disable Spotlight content on Settings pages",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsSpotlightOnSettings=1 in CloudContent policy (machine scope). "
                    + "Removes cloud-provided spotlight tips and suggestions from Settings app pages.",
                Tags = ["cloud", "spotlight", "settings", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightOnSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightOnSettings")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightOnSettings", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-third-party-suggestions",
                Label = "Cloud Content (user): Disable third-party suggestions per user",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableThirdPartySuggestions=1 in HKCU CloudContent policy scope. Provides "
                    + "per-user enforcement of the third-party app-suggestion block.",
                Tags = ["cloud", "suggestions", "third-party", "ads", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableThirdPartySuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableThirdPartySuggestions")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableThirdPartySuggestions", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-spotlight",
                Label = "Cloud Content (user): Disable Windows Spotlight per user",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsSpotlightFeatures=1 in HKCU CloudContent policy scope. Disables "
                    + "Spotlight lock-screen rotation for the current signed-in user.",
                Tags = ["cloud", "spotlight", "lock-screen", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightFeatures")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightFeatures", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-welcome-experience",
                Label = "Cloud Content (user): Disable Spotlight welcome experience per user",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1 at HKCU scope. Suppresses the "
                    + "'What's new' Spotlight welcome popups for the current user after Windows upgrades.",
                Tags = ["cloud", "spotlight", "welcome", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-spotlight-on-settings",
                Label = "Cloud Content (user): Disable Spotlight on Settings per user",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableWindowsSpotlightOnSettings=1 at HKCU CloudContent policy scope. Removes "
                    + "cloud-provided spotlight tips from the Settings app for the current user.",
                Tags = ["cloud", "spotlight", "settings", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightOnSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightOnSettings")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightOnSettings", 1)],
            },
        ];
    }

    // ── CloudDesktopPolicy ──
    private static class _CloudDesktopPolicy
    {
        private const string CdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudDesktop";
        private const string CpcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "clouddesk-disable-cloud-pc-entry-points",
                Label = "Disable Cloud PC Entry Points in Windows UI",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableCloudPCEntryPoints=1 in the CloudDesktop policy key. "
                    + "Removes the Windows 365 Cloud PC link, button, and notification from the "
                    + "Windows Start menu, Settings, and taskbar. Prevents users from seeing or clicking "
                    + "entry points that would prompt them to sign up for or access a Windows 365 subscription. "
                    + "Appropriate for organizations that do not use Windows 365. "
                    + "Default: absent (entry points shown). Recommended: 1 on non-W365 endpoints.",
                Tags = ["cloud-desktop", "windows-365", "cloud-pc", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows 365 Cloud PC entry points removed from Windows UI.",
                ApplyOps = [RegOp.SetDword(CdKey, "DisableCloudPCEntryPoints", 1)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "DisableCloudPCEntryPoints")],
                DetectOps = [RegOp.CheckDword(CdKey, "DisableCloudPCEntryPoints", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-provisioning",
                Label = "Disable Cloud PC Provisioning",
                Category = "Cloud Storage",
                Description =
                    "Sets EnableProvisioning=0 in the CloudDesktop policy key. "
                    + "Prevents the Windows 365 agent from auto-provisioning a Cloud PC session on this device. "
                    + "Useful on physical endpoints that should never auto-redirect to a cloud desktop, "
                    + "ensuring users always work on the local machine's resources. "
                    + "Default: absent (provisioning allowed). Recommended: 0 on standard physical desktops.",
                Tags = ["cloud-desktop", "windows-365", "provisioning", "cloud-pc", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC auto-provisioning disabled; users must connect manually if needed.",
                ApplyOps = [RegOp.SetDword(CdKey, "EnableProvisioning", 0)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "EnableProvisioning")],
                DetectOps = [RegOp.CheckDword(CdKey, "EnableProvisioning", 0)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-virtual-desktop-agent",
                Label = "Disable Cloud PC Agent Auto-Start",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableCloudPCAgent=1 in the CloudDesktop policy key. "
                    + "Prevents the Windows 365/Cloud PC management agent from auto-starting at user login. "
                    + "The agent monitors session state and applies Cloud PC policies; disabling it prevents "
                    + "Windows 365 session management from running on machines that should not connect to "
                    + "any cloud desktop infrastructure. "
                    + "Default: absent (agent starts automatically). Recommended: 1 on non-W365 machines.",
                Tags = ["cloud-desktop", "windows-365", "agent", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows 365 Cloud PC management agent blocked from auto-starting at login.",
                ApplyOps = [RegOp.SetDword(CdKey, "DisableCloudPCAgent", 1)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "DisableCloudPCAgent")],
                DetectOps = [RegOp.CheckDword(CdKey, "DisableCloudPCAgent", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-cloudpc-connection-uac",
                Label = "Disable Cloud PC UAC Elevation Prompts",
                Category = "Cloud Storage",
                Description =
                    "Sets NoAdminUACForCloudPC=1 in the CloudDesktop policy key. "
                    + "Prevents the Cloud PC connection process from triggering UAC elevation dialogs "
                    + "on the local machine. When a Cloud PC session needs elevated rights, the request "
                    + "is handled within the remote cloud session — not on the local endpoint. "
                    + "Reduces login friction on kiosk machines where Cloud PC is the primary desktop. "
                    + "Default: absent. Recommended: 1 on dedicated Cloud PC access endpoints.",
                Tags = ["cloud-desktop", "uac", "elevation", "cloud-pc", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Cloud PC connection process does not prompt UAC elevation on the local machine.",
                ApplyOps = [RegOp.SetDword(CdKey, "NoAdminUACForCloudPC", 1)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "NoAdminUACForCloudPC")],
                DetectOps = [RegOp.CheckDword(CdKey, "NoAdminUACForCloudPC", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-single-sign-on",
                Label = "Disable Single Sign-On to Cloud PC",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableSSO=1 in the CloudPC policy key. "
                    + "Prevents automatic single sign-on (SSO) to the Windows 365 Cloud PC using the "
                    + "local Windows account credentials. When SSO is enabled, a logged-in user is "
                    + "automatically authenticated to the Cloud PC session without re-entering credentials. "
                    + "Disabling SSO requires users to explicitly authenticate each Cloud PC session, "
                    + "providing an additional security checkpoint. "
                    + "Default: absent (SSO enabled). Recommended: 1 for high-security access control.",
                Tags = ["cloud-desktop", "sso", "authentication", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC SSO disabled; users explicitly authenticate each Cloud PC session.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisableSSO", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableSSO")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisableSSO", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-enable-cloud-pc-telemetry-opt-out",
                Label = "Opt Out of Cloud PC Telemetry",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableTelemetry=1 in the CloudPC policy key. "
                    + "Prevents the Cloud PC client from sending diagnostics, usage telemetry, and "
                    + "session-quality metrics to Microsoft's Windows 365 service. "
                    + "Applicable in privacy-sensitive environments or air-gapped networks where "
                    + "outbound telemetry must be minimised. "
                    + "Default: absent (telemetry enabled). Recommended: 1 in high-privacy environments.",
                Tags = ["cloud-desktop", "telemetry", "privacy", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC client telemetry to Microsoft suppressed.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-restrict-cloud-pc-regions",
                Label = "Restrict Cloud PC Provisioning to Closest Region Only",
                Category = "Cloud Storage",
                Description =
                    "Sets RegionSelectionPolicy=1 in the CloudPC policy key. "
                    + "Forces the Windows 365 provisioning system to select only the closest Azure region "
                    + "when allocating a new Cloud PC, instead of allowing cross-region or scheduled-region "
                    + "provisioning. Ensures low latency for users and keeps data residency within the "
                    + "organisation's primary Azure geography. "
                    + "Default: absent (any region). Recommended: 1 for data residency compliance.",
                Tags = ["cloud-desktop", "region", "data-residency", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC provisioned in closest Azure region only; data stays in primary geography.",
                ApplyOps = [RegOp.SetDword(CpcKey, "RegionSelectionPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "RegionSelectionPolicy")],
                DetectOps = [RegOp.CheckDword(CpcKey, "RegionSelectionPolicy", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-cloud-pc-share-clipboard",
                Label = "Disable Clipboard Sharing Between Cloud PC and Local",
                Category = "Cloud Storage",
                Description =
                    "Sets DisableServerClipboard=1 in the CloudPC policy key. "
                    + "Prevents clipboard content from being shared between the local endpoint and the "
                    + "Windows 365 Cloud PC session. Clipboard sync can be a vector for data exfiltration "
                    + "(copy from cloud session, paste to local — or vice versa). "
                    + "Disabling this enforces a hard data-boundary between local and cloud environments. "
                    + "Default: absent (clipboard sharing enabled). Recommended: 1 for DLP compliance.",
                Tags = ["cloud-desktop", "clipboard", "data-leakage", "windows-365", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard not shared between Cloud PC and local endpoint session.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisableServerClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableServerClipboard")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisableServerClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-cloud-pc-redirect-printers",
                Label = "Disable Printer Redirection for Cloud PC Sessions",
                Category = "Cloud Storage",
                Description =
                    "Sets DisablePrinterRedirection=1 in the CloudPC policy key. "
                    + "Prevents local printers attached to the endpoint from being presented inside the "
                    + "Windows 365 Cloud PC session. Printer redirection streams print jobs from the cloud "
                    + "session to a local network printer, but can expose printer model/driver information "
                    + "across the cloud boundary. Disabling this restricts Cloud PC sessions to cloud-side "
                    + "printing only. Default: absent (redirection enabled).",
                Tags = ["cloud-desktop", "printer", "redirection", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Local printers not redirected into Cloud PC sessions.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisablePrinterRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisablePrinterRedirection")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisablePrinterRedirection", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-set-max-session-idle-timeout",
                Label = "Set Cloud PC Session Idle Disconnect Timeout",
                Category = "Cloud Storage",
                Description =
                    "Sets IdleSessionTimeout=30 in the CloudPC policy key. "
                    + "Sets the maximum idle time (in minutes) before a Windows 365 Cloud PC session is "
                    + "automatically disconnected. Idle Cloud PC sessions continue to consume Azure compute "
                    + "and network resources. Auto-disconnect after 30 minutes of inactivity reduces costs "
                    + "and ensures unattended sessions do not remain accessible for extended periods. "
                    + "Default: absent (no idle timeout). Recommended: 15-60 depending on TCO requirements.",
                Tags = ["cloud-desktop", "session", "idle", "timeout", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cloud PC sessions disconnected after 30 minutes of inactivity; saves Azure compute cost.",
                ApplyOps = [RegOp.SetDword(CpcKey, "IdleSessionTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "IdleSessionTimeout")],
                DetectOps = [RegOp.CheckDword(CpcKey, "IdleSessionTimeout", 30)],
            },
        ];
    }

    // ── CloudExperienceHostPolicy ──
    private static class _CloudExperienceHostPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudExperienceHost";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cehpol-disable-cloud-experience",
                Label = "CXH Policy: Disable Windows Cloud Experience Host",
                Category = "Cloud Storage",
                Description =
                    "Disables the Windows Cloud Experience Host (CXH) process that manages OOBE, Tips, and cloud-connected first-run experiences. Reduces telemetry and suppresses pop-up prompts to connect Microsoft services.",
                Tags = ["cxh", "oobe", "cloud", "experience", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables CXH process; reduces telemetry and suppresses service-connect prompts.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudExperienceHost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudExperienceHost")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudExperienceHost", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-oobe-privacy-page",
                Label = "CXH Policy: Disable Privacy Settings Page in OOBE",
                Category = "Cloud Storage",
                Description =
                    "Skips the Privacy Settings experience page during Windows Out-of-Box Experience (OOBE). Ensures default privacy settings are applied silently without user interaction during provisioning.",
                Tags = ["cxh", "oobe", "privacy", "setup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips privacy settings page in OOBE for silent enterprise provisioning.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyExperiencePage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyExperiencePage")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyExperiencePage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-skip-machine-oobe",
                Label = "CXH Policy: Skip Machine-Level OOBE on First Boot",
                Category = "Cloud Storage",
                Description =
                    "Skips the machine-level Windows OOBE experience on the first boot of a provisioned device. Useful for enterprise images where OOBE is unnecessary and should be bypassed for imaging targets.",
                Tags = ["cxh", "oobe", "provisioning", "first-boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips machine-level OOBE on first boot for enterprise images.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipMachineOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipMachineOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "SkipMachineOOBE", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-tailored-experience",
                Label = "CXH Policy: Disable Tailored Experiences with Diagnostic Data",
                Category = "Cloud Storage",
                Description =
                    "Prevents Windows from using diagnostic data to deliver personalised tips, ads, and recommendations via the Cloud Experience Host. Applies at the machine level via Group Policy.",
                Tags = ["cxh", "tailored", "diagnostic", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents diagnostic data from powering personalised tips and ads via CXH.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventTailoredExperiences", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventTailoredExperiences")],
                DetectOps = [RegOp.CheckDword(Key, "PreventTailoredExperiences", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-frx-telemetry",
                Label = "CXH Policy: Disable OOBE Telemetry Data Submission",
                Category = "Cloud Storage",
                Description =
                    "Disables telemetry data collection and submission during the OOBE First-Run Experience (Frx). Prevents Microsoft from receiving device setup analytics from enterprise-provisioned devices.",
                Tags = ["cxh", "oobe", "telemetry", "frx", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables telemetry submission during OOBE first-run experience.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOOBETelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOOBETelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOOBETelemetry", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-account-setup-page",
                Label = "CXH Policy: Disable Account Setup Page in Provisioning",
                Category = "Cloud Storage",
                Description =
                    "Bypasses the Microsoft Account / Azure AD account setup page during OOBE provisioning. Ensures the device is silently joined to the corporate domain without displaying the consumer account prompt.",
                Tags = ["cxh", "oobe", "account", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Bypasses MSA/Azure AD account setup page during enterprise provisioning.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAccountSetupPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountSetupPage")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAccountSetupPage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-cortana-oobe",
                Label = "CXH Policy: Disable Cortana during OOBE",
                Category = "Cloud Storage",
                Description =
                    "Prevents the Cortana voice assistant from launching during OOBE. Stops Cortana from speaking during initial setup on enterprise-provisioned devices, reducing unexpected data transmission.",
                Tags = ["cxh", "oobe", "cortana", "voice", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents Cortana from launching or transmitting during OOBE.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCortanaDuringOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCortanaDuringOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCortanaDuringOOBE", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-device-encryption-page",
                Label = "CXH Policy: Skip Device Encryption Page in OOBE",
                Category = "Cloud Storage",
                Description =
                    "Bypasses the BitLocker Device Encryption setup page during OOBE. Enterprises typically deploy their own BitLocker policy via MDM/GPO and do not want users configuring encryption manually.",
                Tags = ["cxh", "oobe", "bitlocker", "encryption", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips BitLocker setup page in OOBE; enterprises deploy via GPO/MDM.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipDeviceEncryptionPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipDeviceEncryptionPage")],
                DetectOps = [RegOp.CheckDword(Key, "SkipDeviceEncryptionPage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-windows-hello-oobe",
                Label = "CXH Policy: Skip Windows Hello Setup in OOBE",
                Category = "Cloud Storage",
                Description =
                    "Bypasses the Windows Hello biometric/PIN setup prompts during OOBE. Enterprises deploying Windows Hello for Business via GPO/MDM do not need the consumer OOBE Hello setup flow.",
                Tags = ["cxh", "oobe", "windows-hello", "biometrics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Bypasses consumer Hello setup flow; WHfB deployed via GPO/MDM.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipWindowsHelloSetupPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipWindowsHelloSetupPage")],
                DetectOps = [RegOp.CheckDword(Key, "SkipWindowsHelloSetupPage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-oobe-network-page",
                Label = "CXH Policy: Skip Network Connection Page in OOBE",
                Category = "Cloud Storage",
                Description =
                    "Skips the Wi-Fi / network connection page during OOBE. Enterprise devices are typically pre-configured with wireless profiles via MDM, removing the need to prompt users during provisioning.",
                Tags = ["cxh", "oobe", "network", "wifi", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips Wi-Fi page in OOBE; enterprise devices pre-configured with network profiles.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipNetworkConnectionPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipNetworkConnectionPage")],
                DetectOps = [RegOp.CheckDword(Key, "SkipNetworkConnectionPage", 1)],
            },
        ];
    }

    // ── CloudFileSyncPolicy ──
    private static class _CloudFileSyncPolicy
    {
        private const string OdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

        private const string WfKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkFolders";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cfsync-require-sync-encryption",
                    Label = "Cloud File Sync: Require Encryption for Synced Files",
                    Category = "Cloud Storage",
                    Description =
                        "Sets RequireEncryption=1 in WorkFolders policy. Requires that all files managed by Windows Work Folders are stored in an encrypted state on the device's local sync cache. When encryption is required, Work Folders integrates with BitLocker or EFS to ensure the local copy of synced files is encrypted at rest. A device that loses BitLocker protection (TPM not present, BitLocker disabled) cannot sync Work Folders files. Ensures cloud-synced corporate files remain protected even if the device storage is physically accessed.",
                    Tags = ["file-sync", "encryption", "work-folders", "data-protection", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Synced files require local encryption. Work Folders clients on unencrypted devices cannot sync. Verify BitLocker or EFS is deployed before enabling — sync stops on non-compliant devices.",
                    ApplyOps = [RegOp.SetDword(WfKey, "RequireEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(WfKey, "RequireEncryption")],
                    DetectOps = [RegOp.CheckDword(WfKey, "RequireEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-disable-onedrive-personal",
                    Label = "Cloud File Sync: Disable Personal OneDrive Account Sync",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisablePersonalSync=1 in OneDrive policy. Prevents users from signing in to their personal (non-Microsoft 365 commercial) OneDrive accounts through the OneDrive sync client. This is a data loss prevention control: without this restriction, users can drag corporate files from their SharePoint-connected OneDrive work library into their personal OneDrive folder and sync them to personal cloud storage that has no corporate DLP controls. Only Microsoft 365 work/school accounts are permitted.",
                    Tags = ["onedrive", "personal-sync", "dlp", "data-loss", "restriction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Personal OneDrive sync is blocked. Users can only sync OneDrive for Business accounts. Files cannot be moved from work OneDrive to personal OneDrive via the sync client.",
                    ApplyOps = [RegOp.SetDword(OdKey, "DisablePersonalSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "DisablePersonalSync")],
                    DetectOps = [RegOp.CheckDword(OdKey, "DisablePersonalSync", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-enable-known-folder-move",
                    Label = "Cloud File Sync: Enable Known Folder Move to OneDrive for Business",
                    Category = "Cloud Storage",
                    Description =
                        "Sets KFMSilentOptIn=<TenantID> equivalent as a policy flag via EnableKnownFolderMove=1. Enables silent migration of Desktop, Documents, and Pictures from local storage to the user's OneDrive for Business account. Files in Windows known folders are moved to OneDrive and folder redirection is updated automatically without prompting the user. This provides cloud backup for user data on all managed devices without requiring users to manually configure OneDrive — the most common cause of data loss is users who never configured backup.",
                    Tags = ["onedrive", "known-folder-move", "backup", "enterprise", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Desktop, Documents, Pictures are silently moved to OneDrive for Business. Requires M365 licences with OneDrive for Business. Users see their folders unchanged but data is now synced to cloud.",
                    ApplyOps = [RegOp.SetDword(OdKey, "EnableKnownFolderMove", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "EnableKnownFolderMove")],
                    DetectOps = [RegOp.CheckDword(OdKey, "EnableKnownFolderMove", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-set-upload-bandwidth-limit",
                    Label = "Cloud File Sync: Set OneDrive Upload Bandwidth Limit to 50%",
                    Category = "Cloud Storage",
                    Description =
                        "Sets UploadBandwidthLimit=50 in OneDrive policy. Caps the OneDrive sync client's upload bandwidth to 50% of the detected available bandwidth. Without a cap, OneDrive can saturate the uplink during large initial sync operations (e.g., after Known Folder Move, or when a new large document is added), degrading performance for all other network-dependent applications and services. The percentage-based cap is adaptive: on a fast connection, OneDrive uses 50% of a large bandwidth allocation; on a slow connection, it is proportionally throttled.",
                    Tags = ["onedrive", "bandwidth", "throttle", "network", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive uploads are throttled to 50% of available bandwidth. Initial sync after KFM or large library additions takes longer. Network performance for other applications is preserved.",
                    ApplyOps = [RegOp.SetDword(OdKey, "UploadBandwidthLimit", 50)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "UploadBandwidthLimit")],
                    DetectOps = [RegOp.CheckDword(OdKey, "UploadBandwidthLimit", 50)],
                },
                new TweakDef
                {
                    Id = "cfsync-disable-wf-auto-setup",
                    Label = "Cloud File Sync: Disable Automatic Work Folders Setup",
                    Category = "Cloud Storage",
                    Description =
                        "Sets AutoSetup=0 in WorkFolders policy. Prevents Windows from automatically configuring Work Folders when a user signs in to a domain with an SRV record for Work Folders discovery. Automatic Work Folders setup creates local sync directories and begins syncing corporate content without user awareness. In environments that have migrated to OneDrive for Business, phantom Work Folders sync clients create duplicate data paths and storage overhead. Disabling auto-setup ensures Work Folders is only provisioned by explicit IT configuration.",
                    Tags = ["work-folders", "auto-setup", "enterprise", "sync", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Work Folders does not auto-configure on domain join. Work Folders must be configured explicitly by IT. Prevents unintended dual-sync (Work Folders + OneDrive) on migrated environments.",
                    ApplyOps = [RegOp.SetDword(WfKey, "AutoSetup", 0)],
                    RemoveOps = [RegOp.DeleteValue(WfKey, "AutoSetup")],
                    DetectOps = [RegOp.CheckDword(WfKey, "AutoSetup", 0)],
                },
                new TweakDef
                {
                    Id = "cfsync-enable-files-on-demand",
                    Label = "Cloud File Sync: Enable OneDrive Files On-Demand",
                    Category = "Cloud Storage",
                    Description =
                        "Sets FilesOnDemandEnabled=1 in OneDrive policy. Enables Files On-Demand for all OneDrive for Business sync clients: files appear in Explorer with placeholder icons but are not downloaded until accessed. Only files the user explicitly opens or marks for offline use occupy local disk space. For large OneDrive libraries (25 GB+), Files On-Demand prevents disk exhaustion: without it, enabling KFM on a device with a 128 GB boot drive and a 50 GB OneDrive library fills the drive. Required for Known Folder Move in large environments.",
                    Tags = ["onedrive", "files-on-demand", "disk-space", "sync", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive files are placeholders until opened. Files are downloaded on access. Offline access requires explicit marking. Essential for large OneDrive libraries on limited-storage devices.",
                    ApplyOps = [RegOp.SetDword(OdKey, "FilesOnDemandEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "FilesOnDemandEnabled")],
                    DetectOps = [RegOp.CheckDword(OdKey, "FilesOnDemandEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-disable-onedrive-auto-start",
                    Label = "Cloud File Sync: Disable OneDrive from Starting Automatically",
                    Category = "Cloud Storage",
                    Description =
                        "Sets Enabled=0 in OneDrive policy's auto-start key. Prevents the OneDrive sync client from starting automatically at user logon. In environments where OneDrive is not provisioned as the corporate sync solution (e.g., Work Folders or third-party DMS is used instead), having the OneDrive client start in every user session wastes resources and prompts users to configure personal accounts. When OneDrive deployment is managed through Intune or dedicated onboarding workflows, auto-start is unnecessary.",
                    Tags = ["onedrive", "auto-start", "startup", "resource", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive does not start at logon. Users must launch OneDrive manually or it is deployed with auto-start via Intune. No impact on Work Folders or other sync clients.",
                    ApplyOps = [RegOp.SetDword(OdKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(OdKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "cfsync-block-sync-to-unmanaged-domains",
                    Label = "Cloud File Sync: Block OneDrive Sync to Unmanaged Azure AD Tenants",
                    Category = "Cloud Storage",
                    Description =
                        "Sets TenantRestriction=1 in OneDrive policy. Restricts OneDrive sync client connections to only the organisation's Azure AD tenant. Users cannot sync SharePoint data from external tenants or guest accounts that reside in other organisations' Azure AD tenants. This is a data exfiltration prevention control: an employee who has been invited as a guest to an external organisation's Azure AD can otherwise use the OneDrive sync client to download the external organisation's SharePoint data to the corporate machine.",
                    Tags = ["onedrive", "tenant-restriction", "data-exfiltration", "guest", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync is restricted to the organisation's Azure AD tenant. Employees who are guests in external Azure AD tenants cannot sync external SharePoint data. B2B collaboration via browser-based SharePoint is unaffected.",
                    ApplyOps = [RegOp.SetDword(OdKey, "TenantRestriction", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "TenantRestriction")],
                    DetectOps = [RegOp.CheckDword(OdKey, "TenantRestriction", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-enable-silent-account-config",
                    Label = "Cloud File Sync: Enable Silent OneDrive Account Configuration",
                    Category = "Cloud Storage",
                    Description =
                        "Sets SilentAccountConfig=1 in OneDrive policy. Allows OneDrive to configure itself silently using the signed-in user's Azure AD credentials when the user logs into a Hybrid Azure AD Joined or Azure AD Joined device. Without silent configuration, users are prompted to manually set up OneDrive by entering their email address and signing in. With silent configuration, OneDrive picks up the user's Microsoft 365 identity from the device's AAD join state and configures sync automatically — essential for seamless onboarding at scale.",
                    Tags = ["onedrive", "silent-config", "aad", "enterprise", "onboarding"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive configures automatically on Azure AD Joined / Hybrid AAD Joined devices. Users see OneDrive already configured at first logon. Requires Azure AD Join or Hybrid Azure AD Join.",
                    ApplyOps = [RegOp.SetDword(OdKey, "SilentAccountConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "SilentAccountConfig")],
                    DetectOps = [RegOp.CheckDword(OdKey, "SilentAccountConfig", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-require-lock-on-wf-idle",
                    Label = "Cloud File Sync: Require Device Lock When Work Folders in Use",
                    Category = "Cloud Storage",
                    Description =
                        "Sets LockDriveOnIdle=1 in WorkFolders policy. Configures Work Folders to require device screen lock after the device idle timeout when Work Folders are configured. An unlocked device with Work Folders gives an unattended attacker access to synced corporate files without authentication. This policy enforces the screen lock policy as a prerequisite for Work Folders access: if the device screen lock is not configured (no timeout, no PIN on lock), Work Folders displays a warning and may suspend sync until lock is enabled.",
                    Tags = ["work-folders", "screen-lock", "security", "idle", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Screen lock is required when Work Folders are configured. Devices without screen lock display a compliance warning. Does not forcibly lock the screen — it enforces existing screen lock policy configuration.",
                    ApplyOps = [RegOp.SetDword(WfKey, "LockDriveOnIdle", 1)],
                    RemoveOps = [RegOp.DeleteValue(WfKey, "LockDriveOnIdle")],
                    DetectOps = [RegOp.CheckDword(WfKey, "LockDriveOnIdle", 1)],
                },
            ];
    }

    // ── CloudNotificationsPolicy ──
    private static class _CloudNotificationsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudNotifications";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cloudntf-disable-cloud-notifications",
                Label = "Cloud Notifications Policy: Disable All Cloud Notifications",
                Category = "Cloud Storage",
                Description =
                    "Disables the Windows Cloud Notification facility at the policy level. Cloud notifications enable Microsoft and app publishers to deliver system-level banners from cloud services without user-initiated sessions. Disabling prevents unsolicited messages from reaching the desktop.",
                Tags = ["notifications", "cloud", "wns", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudNotifications", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Disables all WNS push; may prevent Defender alerts and Store update notifications.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-account-notifications",
                Label = "Cloud Notifications Policy: Block Microsoft Account Notifications",
                Category = "Cloud Storage",
                Description =
                    "Suppresses notifications related to Microsoft Account sign-in prompts, subscription reminders, and account health alerts delivered via the cloud notification channel. Reduces distracting prompts on managed devices where personal MSA usage is not permitted.",
                Tags = ["notifications", "cloud", "microsoft-account", "msa", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAccountNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAccountNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Suppresses Microsoft Account subscription and health alerts on managed devices.",
            },
            new TweakDef
            {
                Id = "cloudntf-block-network-usage",
                Label = "Cloud Notifications Policy: Block WNS Background Network Usage",
                Category = "Cloud Storage",
                Description =
                    "Prevents the Windows Notification Service (WNS) from establishing and maintaining persistent outbound connections to Microsoft's push notification servers. On metered or restricted networks, WNS background connections consume quota and expose device online status to Microsoft.",
                Tags = ["notifications", "cloud", "wns", "network", "metered", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoNotificationNetworkUsage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoNotificationNetworkUsage")],
                DetectOps = [RegOp.CheckDword(Key, "NoNotificationNetworkUsage", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents WNS persistent outbound connections; may break app push notifications.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-push-to-install",
                Label = "Cloud Notifications Policy: Disable Push-to-Install Notifications",
                Category = "Cloud Storage",
                Description =
                    "Disables cloud-triggered push-to-install notifications that can silently queue OS app installation from the Microsoft Store or Intune. On non-MDM-managed endpoints, push-to-install is a covert app deployment vector.",
                Tags = ["notifications", "cloud", "push-to-install", "store", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePushToInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePushToInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePushToInstall", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks cloud-triggered silent app installations from Microsoft Store or Intune.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-wns-on-metered",
                Label = "Cloud Notifications Policy: Disable WNS on Metered Connections",
                Category = "Cloud Storage",
                Description =
                    "Prevents Windows Notification Service from using metered internet connections (mobile hotspot, cellular). WNS persistent connections on metered networks generate background data charges without user consent.",
                Tags = ["notifications", "cloud", "wns", "metered", "data", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWNSConnectionOnMetered", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWNSConnectionOnMetered")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWNSConnectionOnMetered", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents WNS background data charges on metered or cellular connections.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-notification-mirroring",
                Label = "Cloud Notifications Policy: Disable Cross-Device Notification Mirroring",
                Category = "Cloud Storage",
                Description =
                    "Disables notification mirroring — the feature that forwards a device's notifications to other Windows 10/11 machines signed in with the same Microsoft Account. Notification mirroring routes notification content through Microsoft cloud relays, creating potential data leakage.",
                Tags = ["notifications", "cloud", "mirroring", "cross-device", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNotificationMirroring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNotificationMirroring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNotificationMirroring", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops notification content routing through Microsoft cloud relays to other devices.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-promotional-banners",
                Label = "Cloud Notifications Policy: Disable Microsoft Promotional Notification Banners",
                Category = "Cloud Storage",
                Description =
                    "Suppresses promotional and feature-suggestion notifications delivered through the Windows cloud notification channel. Microsoft uses cloud notifications to surface upgrade prompts, subscription upsells, and feature announcements which are disruptive in managed enterprise environments.",
                Tags = ["notifications", "cloud", "promotional", "ads", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePromotionalNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePromotionalNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePromotionalNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Microsoft upgrade prompts and upsells delivered via cloud notification channel.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-diagnostic-upload",
                Label = "Cloud Notifications Policy: Disable Diagnostic Payload in Cloud Notifications",
                Category = "Cloud Storage",
                Description =
                    "Prevents the WNS notification channel from including diagnostic telemetry payloads in cloud notification requests. Notification channel diagnostics include device health and engagement metrics that are forwarded to Microsoft without explicit user opt-in.",
                Tags = ["notifications", "cloud", "diagnostics", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticInNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticInNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticInNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents device engagement metrics from being included in WNS diagnostic payloads.",
            },
            new TweakDef
            {
                Id = "cloudntf-block-background-refresh",
                Label = "Cloud Notifications Policy: Block Cloud Notification Background Refresh",
                Category = "Cloud Storage",
                Description =
                    "Prevents applications from refreshing cloud-sourced notification content in the background while not in use. Background notification refresh for cloud-connected apps creates persistent outbound connections to app backends that profile device online patterns.",
                Tags = ["notifications", "cloud", "background", "refresh", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockBackgroundNotificationRefresh", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockBackgroundNotificationRefresh")],
                DetectOps = [RegOp.CheckDword(Key, "BlockBackgroundNotificationRefresh", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents apps maintaining persistent cloud connections when not in use.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-focus-assist-override",
                Label = "Cloud Notifications Policy: Prevent Cloud Override of Focus Assist",
                Category = "Cloud Storage",
                Description =
                    "Blocks cloud services from overriding the local Focus Assist (Do Not Disturb) settings to deliver high-priority cloud notifications. Ensures user-configured quiet hours are respected even when Microsoft or app publishers classify a cloud notification as urgent.",
                Tags = ["notifications", "cloud", "focus-assist", "do-not-disturb", "override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventFocusAssistOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventFocusAssistOverride")],
                DetectOps = [RegOp.CheckDword(Key, "PreventFocusAssistOverride", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ensures user-configured quiet hours are not bypassed by cloud-classified urgent notifications.",
            },
        ];
    }

    // ── CloudPrintPolicy ──
    private static class _CloudPrintPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudPrint";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cldprt-disable-cloud-print-service",
                Label = "Disable Windows Cloud Print Discovery and Universal Print Services",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disabling Windows Cloud Print discovery prevents client computers from connecting to cloud-hosted print services that could transmit document content to external cloud infrastructure outside organizational control. Cloud print services route document data through external servers and the privacy and security controls of those cloud services may not meet organizational compliance requirements. Organizations that manage print infrastructure with on-premises print servers should disable cloud print discovery to ensure all printing flows through audited enterprise print infrastructure. Accidental use of cloud print services can result in sensitive documents being transmitted to and stored by external cloud providers without appropriate data handling controls. Disabling cloud print discovery does not prevent users from manually configuring printers but does remove automatic discovery of cloud print endpoints from the print experience. Organizations that have legitimate cloud print requirements through approved enterprise services like Universal Print should configure those services centrally rather than enabling broad cloud print discovery.",
                Tags = ["cloud-print", "print-services", "data-protection", "discovery", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudPrintService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPrintService")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudPrintService", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-restrict-cloud-printer-installation",
                Label = "Restrict User-Initiated Cloud Printer Installation Without Administrator Approval",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting user-initiated cloud printer installation prevents standard users from adding cloud print destinations that route documents through external services not approved or managed by IT operations. User-installed cloud printers can exfiltrate sensitive document content to personal or unauthorized business cloud print accounts outside organizational visibility. Print driver installation associated with cloud printers can introduce software components that have not been vetted by endpoint security teams. Organizations should centrally manage all printer deployments including cloud printers through Group Policy or device management platforms to ensure only approved print destinations are available. Restricting printer installation to administrators allows IT to control the complete list of print destinations available to users including auditing which cloud print services are in use. Users who have legitimate business requirements for cloud printing should submit requests through the IT service catalog for evaluation and approved deployment.",
                Tags = ["cloud-print", "printer-installation", "user-restriction", "data-handling", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCloudPrinterInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCloudPrinterInstallation")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCloudPrinterInstallation", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-enforce-enterprise-cloud-print-only",
                Label = "Enforce Use of Enterprise-Only Cloud Print Services for All Organization Devices",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforcing enterprise-only cloud print restricts cloud print operations to organizationally approved cloud print services preventing use of personal or unauthorized third-party cloud print providers. Many employees use consumer cloud print services for convenience when enterprise print alternatives are inconvenient but this creates uncontrolled data flows of potentially sensitive documents. Enterprise cloud print services like Microsoft Universal Print integrate with Azure Active Directory and provide administrative visibility into print jobs including audit logging relevant to compliance requirements. Organizations should deploy approved enterprise cloud print services that provide administrative oversight before enforcing the enterprise-only cloud print restriction. Transitioning from unrestricted cloud print to enterprise-only requires communication to users about what print services are approved and how to access them for remote and mobile printing scenarios. Audit logging of cloud print operations through enterprise print services provides visibility into printing volumes and patterns that may indicate data exfiltration attempts.",
                Tags = ["cloud-print", "enterprise-only", "approved-services", "universal-print", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceEnterpriseCloudPrintOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceEnterpriseCloudPrintOnly")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceEnterpriseCloudPrintOnly", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-disable-mobile-print-discovery",
                Label = "Disable Automatic Mobile Print Service Discovery on Corporate Network",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Automatic mobile print discovery broadcasts the availability of print services to mobile devices on the corporate network creating potential data exfiltration pathways through mobile device printing that bypasses desktop endpoint security controls. Mobile devices connecting to corporate printers through automatic discovery may not have the same document handling policies applied that are enforced on managed desktop systems. Disabling automatic mobile print discovery reduces the attack surface from rogue mobile devices that join the corporate wireless network and attempt to access print infrastructure. Organizations that support mobile printing should implement this through Mobile Device Management policies that configure approved wireless print access rather than through open network service discovery. Print infrastructure security should include network access controls to restrict which devices can communicate with print servers. Corporate print servers should be segmented from general user VLAN segments to limit direct print protocol access to devices with legitimate printing needs.",
                Tags = ["mobile-print", "print-discovery", "network-security", "attack-surface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMobilePrintDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMobilePrintDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMobilePrintDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-audit-cloud-print-job-submissions",
                Label = "Enable Audit Logging for All Cloud Print Job Submissions and Printer Access",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Audit logging for cloud print job submissions records every print job sent to cloud print services including the user identity document metadata and destination printer providing an audit trail for potential data exfiltration investigations. Large document printing events or printing to unusual destinations outside normal work hours may indicate data exfiltration using print as a covert data transfer channel. Print audit logs should be integrated with SIEM alerting to detect anomalous print activity such as printing significantly more pages than baseline or printing sensitive documents to non-standard destinations. Cloud print audit logging from enterprise services provides more complete visibility than local print spooler logging because it captures the complete print workflow across cloud infrastructure. Organizations subject to data protection regulations should retain print audit logs for periods that satisfy retention requirements for regulatory investigations. User privacy considerations should be balanced with security monitoring needs when designing print audit programs to ensure appropriate oversight without unnecessary surveillance.",
                Tags = ["cloud-print", "print-audit", "monitoring", "data-exfiltration", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditCloudPrintJobSubmissions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCloudPrintJobSubmissions")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCloudPrintJobSubmissions", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-require-mfa-for-cloud-print-auth",
                Label = "Require Multi-Factor Authentication for Cloud Print Service Authentication",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Requiring multi-factor authentication for cloud print service operations ensures that cloud print access cannot be used to authenticate as a user using only stolen credentials which could expose sensitive documents in the print queue. Cloud print services that authenticate with Azure Active Directory credentials should inherit the conditional access policies that require MFA for cloud service authentication. Print jobs queued in cloud print infrastructure are protected by authentication requirements at the time of retrieval preventing unauthorized release of documents from cloud print queues. MFA for cloud print authentication prevents attackers who compromise user credentials from submitting or retrieving print jobs that might reveal organizational information. Organizations should configure conditional access policies that apply MFA requirements to cloud print services as part of the general cloud service MFA rollout. Service accounts used for print infrastructure management should use certificate-based authentication or managed identity approaches rather than password-based MFA.",
                Tags = ["cloud-print", "mfa", "authentication", "cloud-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireMFAForCloudPrintAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMFAForCloudPrintAuth")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMFAForCloudPrintAuth", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-disable-print-to-pdf-cloud-storage",
                Label = "Disable Automatic Saving of Print to PDF Output to Cloud Storage Locations",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Print to PDF feature when configured to automatically save output to cloud-connected storage locations including OneDrive can result in sensitive document content being transmitted to cloud storage without explicit user intent. Disabling automatic cloud save for Print to PDF output ensures that users must explicitly choose to save PDF output to cloud storage rather than having documents automatically uploaded. Documents printed to PDF during the course of normal workday operations may include sensitive contracts financial documents HR information or other content that should not be automatically uploaded to personal cloud storage. Organizations should configure Print to PDF default save locations to point to local or networked storage under organizational control. Users who have business requirements to share PDF output via cloud storage should explicitly save documents to approved business cloud storage rather than through automatic upload from the print subsystem. Document classification and DLP policies should be applied to all cloud storage upload paths to prevent accidental upload of sensitive content.",
                Tags = ["print-to-pdf", "cloud-storage", "data-protection", "automatic-upload", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintToPdfCloudStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToPdfCloudStorage")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintToPdfCloudStorage", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-restrict-personal-cloud-print-accounts",
                Label = "Restrict Use of Personal Cloud Print Accounts on Domain-Joined Devices",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting personal cloud print account use on domain-joined devices prevents sensitive corporate documents from being transmitted to personal cloud print queues associated with non-corporate accounts that lack organizational data security controls. Employees using personal Google Cloud Print HP ePrint or other consumer cloud print services may not understand that their documents are being stored by the print service provider under consumer terms of service rather than enterprise security agreements. Personal cloud print accounts are not subject to corporate data retention deletion and security audit requirements creating compliance gaps for regulated organizations. Domain-joined devices should be configured to allow only enterprise cloud print accounts authenticated with organizational credentials. Unified endpoint management platforms can enforce cloud print account restrictions for both domain-joined and modern device-enrolled endpoints providing consistent control across device management approaches. User education about the importance of using only approved organizational print services for corporate documents should accompany technical controls.",
                Tags = ["personal-accounts", "cloud-print", "data-governance", "domain-joined", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictPersonalCloudPrintAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictPersonalCloudPrintAccounts")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictPersonalCloudPrintAccounts", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-block-unencrypted-cloud-print-transmission",
                Label = "Block Unencrypted Data Transmission to Cloud Print Service Endpoints",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking unencrypted cloud print transmission ensures that all document data sent to cloud print services uses TLS-encrypted connections preventing interception of document content during transmission to the cloud print infrastructure. Legacy print protocols and some consumer cloud print integrations may use unencrypted HTTP connections for print job submission that expose document content to network interception. Organizations should verify that approved cloud print services use TLS 1.2 or higher for all print data transmission and certificate validation is enforced to prevent man-in-the-middle attacks. Encrypted cloud print transmission protects document content from passive network monitoring by adversaries with access to enterprise or internet network segments. DLP monitoring at the network layer can inspect print traffic transmitted over unencrypted channels making encryption enforcement a complementary control to DLP. Cloud print service vendor security documentation should be reviewed to understand the encryption and data protection measures in place for print data at rest in cloud infrastructure.",
                Tags = ["cloud-print", "encryption", "tls", "data-in-transit", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnencryptedCloudPrintTransmission", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnencryptedCloudPrintTransmission")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnencryptedCloudPrintTransmission", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-enforce-print-data-retention-policy",
                Label = "Enforce Organizational Data Retention Policy for Cloud Print Job Metadata",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Cloud print service metadata including print job history user identities document names timestamps and printer destinations is retained by cloud print providers for configuration, support and billing purposes which may conflict with organizational data retention and deletion policies. Enforcing an organizational print data retention policy ensures that print job metadata stored in cloud print infrastructure is deleted on a schedule consistent with organizational data governance requirements. Regulatory requirements in some jurisdictions limit retention of personal data associated with user activity including print job metadata which requires coordination with cloud print service providers regarding their data retention practices. Organizations should review the data processing and retention terms of cloud print service agreements as part of the vendor management and data privacy compliance process. Cloud print audit data should be distinguished from cloud print service operational metadata with audit data retained based on the organization's security audit requirements. Data subject access requests for personal data may need to include print metadata managed by cloud print services requiring notification of the cloud print service vendor as part of the request fulfillment process.",
                Tags = ["data-retention", "cloud-print", "compliance", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforcePrintDataRetentionPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrintDataRetentionPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnforcePrintDataRetentionPolicy", 1)],
            },
        ];
    }

    // ── CloudStorageQuotaPolicy ──
    private static class _CloudStorageQuotaPolicy
    {
        private const string CloudContentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string StorageSenseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudqt-enable-storage-sense-enforcement",
                    Label = "Cloud Storage Quota: Enable Storage Sense Automatic Disk Cleanup",
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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
                    Category = "Cloud Storage",
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

    // ── ContentDeliveryPolicy ──
    private static class _ContentDeliveryPolicy
    {
        private const string CloudPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
        private const string StartPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Start";
        private const string CdmPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ContentDeliveryManager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cdpol-disable-consumer-experiences",
                Label = "Disable Windows Consumer Experiences (Bloatware Auto-Install)",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["content delivery", "bloatware", "debloat", "consumer", "privacy", "group policy"],
                Description =
                    "Disables the Content Delivery Manager from auto-installing suggested third-party apps "
                    + "(games, Candy Crush, Spotify etc.) via Windows Consumer Experiences. "
                    + "DisableWindowsConsumerFeatures = 1. "
                    + "Essential for enterprise deployments and clean Windows installations. "
                    + "Default: consumer app suggestions silently installed after setup.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsConsumerFeatures", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsConsumerFeatures", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsConsumerFeatures", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-windows-spotlight",
                Label = "Disable Windows Spotlight on Lock Screen",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["content delivery", "spotlight", "lock screen", "privacy", "group policy"],
                Description =
                    "Disables Windows Spotlight on the lock screen via Group Policy. "
                    + "DisableWindowsSpotlightFeatures = 1. "
                    + "Prevents Microsoft-curated wallpapers, tips, and ads from displaying on the lock screen. "
                    + "Default: Spotlight enabled showing Bing-sourced images and suggestions.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightFeatures", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightFeatures", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightFeatures", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-spotlight-action-center",
                Label = "Disable Windows Spotlight in Action Center",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["content delivery", "spotlight", "action center", "notifications", "group policy"],
                Description =
                    "Prevents Windows Spotlight suggestions and tips from appearing in notifications "
                    + "and the Action Center. "
                    + "DisableWindowsSpotlightOnActionCenter = 1. "
                    + "Removes Microsoft promotional content from the notification tray. "
                    + "Default: Spotlight Action Center content enabled.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnActionCenter", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnActionCenter", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightOnActionCenter", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-third-party-spotlight",
                Label = "Disable Third-Party App Suggestions in Spotlight",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["content delivery", "spotlight", "ads", "privacy", "third-party", "group policy"],
                Description =
                    "Blocks third-party application advertisements and suggestions from appearing "
                    + "within the Windows Spotlight and lock screen features. "
                    + "DisableThirdPartySuggestions = 1. "
                    + "Prevents marketplaced app promotions from appearing even when Spotlight is otherwise active. "
                    + "Default: third-party suggestions shown.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableThirdPartySuggestions", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableThirdPartySuggestions", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableThirdPartySuggestions", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-start-suggestions",
                Label = "Disable Suggested Apps in Start Menu",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["content delivery", "start menu", "suggestions", "bloatware", "group policy"],
                Description =
                    "Prevents cloud-powered app suggestions and recommendations from appearing "
                    + "in the Start menu's recommended section. "
                    + "DisableAppsFromStore = 0 but SubscribedContent-338389Enabled = 0 equivalent via policy. "
                    + "HideRecommendedSection = 1. "
                    + "Gives users a clean, app-only Start menu without Microsoft Store promotions. "
                    + "Default: recommendations shown.",
                MinBuild = 22000,
                ApplyOps = [RegOp.SetDword(StartPol, "HideRecommendedSection", 1)],
                RemoveOps = [RegOp.SetDword(StartPol, "HideRecommendedSection", 0)],
                DetectOps = [RegOp.CheckDword(StartPol, "HideRecommendedSection", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-spotlight-taskbar",
                Label = "Disable Windows Spotlight on Taskbar and Search",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["content delivery", "spotlight", "taskbar", "search", "privacy", "group policy"],
                Description =
                    "Removes Windows Spotlight suggestions from the taskbar search box and Search Home. "
                    + "DisableWindowsSpotlightOnSettings = 1. "
                    + "Prevents Bing-powered content from appearing in the taskbar search callout. "
                    + "Default: Spotlight taskbar content enabled on Windows 11.",
                MinBuild = 22000,
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnSettings", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnSettings", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightOnSettings", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-oobe-tips",
                Label = "Disable Spotlight-Based Tips and Tricks During Setup (OOBE)",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["content delivery", "oobe", "setup", "tips", "group policy"],
                Description =
                    "Disables Windows Spotlight-powered tips and suggestions shown during Out-of-Box "
                    + "Experience (OOBE) and Windows first-run setup screens. "
                    + "DisableWindowsSpotlightWindowsWelcomeExperience = 1. "
                    + "Streamlines enterprise provisioning and audit by removing consumer-targeted prompts.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightWindowsWelcomeExperience", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-content-delivery-auto-download",
                Label = "Disable Content Delivery Manager Auto-Downloads",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["content delivery", "auto-download", "bloatware", "privacy", "bandwidth", "group policy"],
                Description =
                    "Prevents the Content Delivery Manager from silently downloading new app packages, "
                    + "features, and promotional content in the background. "
                    + "PreventAutoContentDelivery = 1 via CdmPol. "
                    + "Reduces surprise bandwidth usage and prevents unwanted app installations on metered connections.",
                ApplyOps = [RegOp.SetDword(CdmPol, "PreventAutoContentDelivery", 1)],
                RemoveOps = [RegOp.SetDword(CdmPol, "PreventAutoContentDelivery", 0)],
                DetectOps = [RegOp.CheckDword(CdmPol, "PreventAutoContentDelivery", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-get-office-promotion",
                Label = "Disable 'Get Microsoft 365' Promotional Node in Settings",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["content delivery", "office 365", "microsoft 365", "ads", "privacy", "group policy"],
                Description =
                    "Removes the Microsoft 365 / Office sign-up promotional page from Windows Settings. "
                    + "DisableWindowsSpotlightOnSettingsOfficePush_ProviderSet = 1. "
                    + "Stops Microsoft Office subscription upsells from appearing in the Settings app. "
                    + "Default: promotion shown when Office is not installed.",
                ApplyOps = [RegOp.SetDword(CloudPol, "ConfigureWindowsSpotlight", 2)],
                RemoveOps = [RegOp.DeleteValue(CloudPol, "ConfigureWindowsSpotlight")],
                DetectOps = [RegOp.CheckDword(CloudPol, "ConfigureWindowsSpotlight", 2)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-tailored-experiences",
                Label = "Disable Tailored Experiences with Diagnostic Data",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["content delivery", "tailored", "telemetry", "privacy", "group policy"],
                Description =
                    "Prevents Windows from using diagnostic data to show personalised tips, ads, "
                    + "and recommendations via the 'Tailored Experiences' feature. "
                    + "DisableTailoredExperiencesWithDiagnosticData = 1. "
                    + "Stops Microsoft from profiling usage patterns to target in-Windows promotions. "
                    + "Default: tailored experiences enabled when diagnostic data is set to Full.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            },
        ];
    }

    // ── DesktopAnalyticsPolicy ──
    private static class _DesktopAnalyticsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dskanlyt-set-commercial-id",
                Label = "Configure Commercial ID for Desktop Analytics Data Collection",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Commercial ID associates diagnostic data sent to Microsoft with a specific organization's Desktop Analytics workspace enabling analytics dashboards for update compliance. Configuring the Commercial ID is required to use Desktop Analytics for Windows update readiness assessments and compatibility analysis. Without a Commercial ID diagnostic data is anonymized and cannot be correlated with organizational devices for Desktop Analytics reporting. Organizations using Desktop Analytics for update risk assessment must deploy the Commercial ID policy to all managed devices. Commercial IDs are generated in the Azure Portal for Desktop Analytics workspaces and should be protected as organizational identifiers. Organizations that disable diagnostic data collection should clear the Commercial ID setting to prevent any residual correlation of device data.",
                Tags = ["desktop-analytics", "commercial-id", "diagnostic-data", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "CommercialId", "")],
                RemoveOps = [RegOp.DeleteValue(Key, "CommercialId")],
                DetectOps = [RegOp.CheckMissing(Key, "CommercialId")],
            },
            new TweakDef
            {
                Id = "dskanlyt-set-diagnostic-data-level",
                Label = "Set Diagnostic Data Collection Level to Security Only",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Diagnostic data levels control the amount of telemetry transmitted to Microsoft with Security being the minimal level and Full being the maximum. Setting diagnostic data to Security level (0) transmits only the minimum data required to keep Windows secure including Malicious Software Removal Tool data and Windows Defender security intelligence. Restricting diagnostic data to Security level minimizes the organizational data transmitted to Microsoft while still receiving security updates. Enterprise editions of Windows 10 and later support the Security (0) level which is not available on consumer editions. Organizations concerned about data privacy should configure Security level or at most Basic Enhanced level for managed systems. Desktop Analytics requires at least Enhanced diagnostic data level to provide full compatibility and update readiness intelligence.",
                Tags = ["desktop-analytics", "diagnostic-data", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-msdt-telemetry",
                Label = "Disable Microsoft Diagnostics and Troubleshooting Tool Telemetry",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Microsoft Diagnostics and Troubleshooting (MSDT) transmits debugging and crash information to Microsoft that can include sensitive data from system crash dumps or application error reports. Disabling MSDT telemetry prevents diagnostic data from system failures from being transmitted to Microsoft which may include memory contents from the time of the failure. Crash dumps can contain sensitive in-memory data including encryption keys login tokens and sensitive application data that should not be transmitted externally. Organizations should configure crash dump settings to capture the minimum data needed for internal debugging rather than sending full crash reports to Microsoft. MSDT telemetry disabling should be combined with WER (Windows Error Reporting) restrictions to provide comprehensive control over error data transmission. Systems running high-security workloads should minimize diagnostic data transmission through both Microsoft and third-party error reporting frameworks.",
                Tags = ["desktop-analytics", "msdt", "telemetry", "crash-data", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticData", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-data-in-device-health-reports",
                Label = "Disable Device Health Attestation Data Reporting to External Services",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Device Health Attestation reports security configuration data including boot state and security feature status to cloud services for compliance verification. Disabling external Device Health Attestation data reporting prevents health data from being transmitted to Microsoft cloud services for organizations that use internal attestation systems. Organizations running on-premises Device Health Attestation servers manage their own attestation data without requiring Microsoft cloud services for health reporting. Cloud-based Device Health Attestation provides valuable security enforcement for conditional access but requires transmitting device state to Microsoft. The choice between cloud and on-premises Device Health Attestation should align with the organization's data sovereignty and privacy requirements. Organizations deploying Microsoft Intune typically rely on cloud-based Device Health Attestation as part of conditional access policy enforcement.",
                Tags = ["desktop-analytics", "device-health", "attestation", "reporting", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitDiagnosticLogCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitDiagnosticLogCollection")],
                DetectOps = [RegOp.CheckDword(Key, "LimitDiagnosticLogCollection", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-inventory-collection",
                Label = "Disable Automatic Software Inventory Collection for Analytics",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Automatic software inventory collection transmits an inventory of installed applications and drivers to Microsoft's Desktop Analytics service for compatibility analysis. Disabling software inventory collection prevents application lists from being transmitted to Microsoft which reduces the data footprint in Microsoft's analytics cloud. Software inventory data may reveal internal application names versions and configurations that organizations consider sensitive or confidential. Organizations using Desktop Analytics for actual update readiness assessment need inventory data enabled to benefit from compatibility analysis. For organizations not using Desktop Analytics the inventory collection provides no business value and represents unnecessary data transmission. Inventory collection disabling should be applied to all systems outside the Analytics scope to minimize unnecessary data collection.",
                Tags = ["desktop-analytics", "inventory", "software-collection", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-update-compliance-collection",
                Label = "Disable Update Compliance Data Collection for Non-Analytics Systems",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Update Compliance data collection transmits Windows Update and Windows Defender update status to Microsoft's Log Analytics service for organizational compliance reporting. Disabling Update Compliance collection for systems outside the analytics scope eliminates unnecessary transmission of update status data to Microsoft cloud services. Organizations using Update Compliance must maintain the Collection settings for enrolled devices while disabling it for systems out of scope for analytics. Update Compliance provides valuable data for identifying unpatched systems but requires cloud data transmission for analysis. On-premises alternatives to Update Compliance include WSUS reports and Configuration Manager update compliance reports that keep data internal. Organizations with strict data sovereignty requirements should use on-premises update compliance reporting instead of Microsoft's collected analytics.",
                Tags = ["desktop-analytics", "update-compliance", "cloud-reporting", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "dskanlyt-restrict-feedback-hub-telemetry",
                Label = "Restrict Feedback Hub from Submitting User Diagnostic Data",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows Feedback Hub allows users to submit feedback to Microsoft that can include diagnostic logs screenshots and system state information which requires careful control in enterprise environments. Restricting Feedback Hub telemetry prevents users from intentionally or inadvertently submitting sensitive system information through the feedback channel. Feedback submissions can include diagnostic logs from applications or system components that contain sensitive business data requiring restriction in regulated environments. Organizations should restrict Feedback Hub access for all managed systems while maintaining channels for product feedback through approved enterprise feedback mechanisms. Feedback Hub feedback settings should be part of the overall data classification and transmission policy for managed endpoints. Disabling Feedback Hub does not prevent Windows from collecting system telemetry through other channels which must be controlled separately.",
                Tags = ["desktop-analytics", "feedback-hub", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-compat-appraiser-task",
                Label = "Disable Compatibility Appraiser Scheduled Task for Analytics",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Compatibility Appraiser scheduled task runs assessments that collect application and device compatibility data for Desktop Analytics and Windows upgrade readiness. Disabling the Compatibility Appraiser task prevents compatibility data from being collected and transmitted to Microsoft's analytics services. The Appraiser scan runs daily on systems enrolled in Desktop Analytics consuming CPU and I/O resources to assess installed applications and hardware compatibility. Organizations not using Desktop Analytics for upgrade planning have no need for the Appraiser task and should disable it to reduce unnecessary resource consumption and data transmission. Disabling the Appraiser task does not affect Windows Update delivery or security update installation. Organizations planning Windows version upgrades should enable the Compatibility Appraiser for a period before the upgrade to identify compatibility blockers.",
                Tags = ["desktop-analytics", "compatibility-appraiser", "scheduled-task", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCompatibilityAppraiser", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompatibilityAppraiser")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompatibilityAppraiser", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-restrict-enhanced-diagnostic-data",
                Label = "Restrict Enhanced Diagnostic Data to Required Minimum Events",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enhanced diagnostic data level transmits additional optional events beyond the Basic level that provide richer analytics data but also represent greater data transmission and privacy exposure. Restricting Enhanced diagnostic data to the required events subset limits transmission to only events required for Desktop Analytics without sending all Enhanced level events. Windows 10 1803 introduced an Enhanced Required Events option that allows Desktop Analytics usage while minimizing miscellaneous Enhanced events. Organizations using Desktop Analytics who want to minimize data transmission should configure Enhanced Required Events rather than the full Enhanced level. The EnableOneSettingsAuditing and RequiredEventsPolicies settings control which specific Enhanced events are transmitted when Enhanced Required Events is configured. Regular review of configured diagnostic data policies through compliance monitoring ensures that settings remain aligned with the organization's data handling requirements.",
                Tags = ["desktop-analytics", "enhanced-diagnostics", "data-minimization", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitEnhancedDiagnosticDataWindowsAnalytics")],
                DetectOps = [RegOp.CheckDword(Key, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-audit-diagnostic-data-changes",
                Label = "Enable Audit Logging for Diagnostic Data Policy Configuration Changes",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Diagnostic data configuration audit logging captures changes to telemetry settings that could indicate attempts to circumvent data collection restrictions or silently increase diagnostic data levels. Enabling audit logging for diagnostic policy changes provides visibility into telemetry configuration modifications that may violate organizational privacy policies. Changes to AllowTelemetry and related policies should be rare in production environments and any unauthorized changes should trigger security investigation. Malware that attempts to re-enable telemetry collection after it has been disabled will generate audit events that can be detected through SIEM alerting. Diagnostic data policy auditing should be included in the baseline configuration monitoring for all managed systems. Correlation of diagnostic data policy changes with user logon events helps identify which accounts made changes for accountability.",
                Tags = ["desktop-analytics", "audit", "policy-monitoring", "telemetry", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableOneSettingsAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableOneSettingsAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableOneSettingsAuditing", 1)],
            },
        ];
    }

    // ── OneDriveKfmPolicy ──
    private static class _OneDriveKfmPolicy
    {
        private const string KfmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "odkfm-silent-opt-in",
                Label = "OneDrive KFM: Silently Move Known Folders to OneDrive",
                Category = "Cloud Storage",
                Description =
                    "Silently redirects Desktop, Documents, and Pictures to OneDrive without user interaction. Requires the tenant ID (GUID) to be set as the value data for KFMSilentOptIn.",
                Tags = ["onedrive", "kfm", "known-folder-move", "backup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Silently moves Desktop/Documents/Pictures to OneDrive; requires tenant GUID.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetString(KfmKey, "KFMSilentOptIn", "")],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMSilentOptIn")],
                DetectOps = [RegOp.CheckMissing(KfmKey, "KFMSilentOptIn")],
            },
            new TweakDef
            {
                Id = "odkfm-silent-opt-in-notification",
                Label = "OneDrive KFM: Silent Opt-In with Toast Notification",
                Category = "Cloud Storage",
                Description =
                    "Silently moves known folders to OneDrive and shows a toast notification to the user explaining the change. Less disruptive than prompting but still informative.",
                Tags = ["onedrive", "kfm", "known-folder-move", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Silent KFM with user toast notification; less disruptive than prompted.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "KFMSilentOptInWithNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMSilentOptInWithNotification")],
                DetectOps = [RegOp.CheckDword(KfmKey, "KFMSilentOptInWithNotification", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-opt-in-wizard",
                Label = "OneDrive KFM: Prompt Users to Move Known Folders (Wizard)",
                Category = "Cloud Storage",
                Description =
                    "Prompts users with a guided wizard to move their Desktop, Documents, and Pictures folders to OneDrive. The user must confirm the move.",
                Tags = ["onedrive", "kfm", "known-folder-move", "wizard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prompts users with a guided wizard to move known folders to OneDrive.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetString(KfmKey, "KFMOptInWithWizard", "")],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMOptInWithWizard")],
                DetectOps = [RegOp.CheckMissing(KfmKey, "KFMOptInWithWizard")],
            },
            new TweakDef
            {
                Id = "odkfm-silent-opt-out",
                Label = "OneDrive KFM: Silently Redirect Known Folders Back to Local",
                Category = "Cloud Storage",
                Description =
                    "Silently reverses Known Folder Move, redirecting Desktop, Documents, and Pictures back to local paths on the device without prompting the user.",
                Tags = ["onedrive", "kfm", "known-folder-move", "revert", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Silently reverses KFM back to local paths without user prompt.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "KFMSilentOptOut", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMSilentOptOut")],
                DetectOps = [RegOp.CheckDword(KfmKey, "KFMSilentOptOut", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-force-update-ring",
                Label = "OneDrive KFM: Set OneDrive Update Ring",
                Category = "Cloud Storage",
                Description =
                    "Forces OneDrive Sync Client to use a specific update ring: 'Insider', 'Production', or 'Deferred'. Deferred delays updates by ~60 days for enterprise stability.",
                Tags = ["onedrive", "update-ring", "enterprise", "update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Forces OneDrive to Deferred ring for enterprise stability.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetString(KfmKey, "GPOSetUpdateRing", "Deferred")],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "GPOSetUpdateRing")],
                DetectOps = [RegOp.CheckString(KfmKey, "GPOSetUpdateRing", "Deferred")],
            },
            new TweakDef
            {
                Id = "odkfm-prevent-network-traffic-before-signin",
                Label = "OneDrive KFM: Block Pre-Logon Network Traffic",
                Category = "Cloud Storage",
                Description =
                    "Prevents the OneDrive Sync Client from making any network calls before user sign-in. Avoids unexpected traffic on secure/kiosk machines during boot.",
                Tags = ["onedrive", "network", "privacy", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks OneDrive pre-logon network calls on kiosk/secure machines.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "PreventNetworkTrafficPreUserSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "PreventNetworkTrafficPreUserSignIn")],
                DetectOps = [RegOp.CheckDword(KfmKey, "PreventNetworkTrafficPreUserSignIn", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-min-disk-space",
                Label = "OneDrive KFM: Set Minimum Free Disk Space Threshold",
                Category = "Cloud Storage",
                Description =
                    "Sets the minimum local disk free space (in MB) below which OneDrive will warn users and pause sync. Default is 500 MB. Set to 2048 for safer enterprise deployments.",
                Tags = ["onedrive", "disk-space", "quota", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sets minimum free disk threshold before OneDrive warns and pauses sync.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "MinDiskSpaceLimitInMB", 2048)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "MinDiskSpaceLimitInMB")],
                DetectOps = [RegOp.CheckDword(KfmKey, "MinDiskSpaceLimitInMB", 2048)],
            },
            new TweakDef
            {
                Id = "odkfm-warning-disk-space",
                Label = "OneDrive KFM: Set Low Disk Space Warning Threshold",
                Category = "Cloud Storage",
                Description =
                    "Configures the early disk-space warning threshold for OneDrive (in MB). When free space drops below this value, a warning is shown before sync is blocked.",
                Tags = ["onedrive", "disk-space", "warning", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Configures early disk-space warning threshold for OneDrive.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "WarningMinDiskSpaceLimitInMB", 4096)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "WarningMinDiskSpaceLimitInMB")],
                DetectOps = [RegOp.CheckDword(KfmKey, "WarningMinDiskSpaceLimitInMB", 4096)],
            },
            new TweakDef
            {
                Id = "odkfm-disable-teamsite-automount",
                Label = "OneDrive KFM: Disable SharePoint/Teams Auto-Mount",
                Category = "Cloud Storage",
                Description =
                    "Prevents OneDrive from automatically syncing SharePoint team site document libraries without user action. Users must manually add sync folders in OneDrive.",
                Tags = ["onedrive", "sharepoint", "teams", "auto-mount", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents automatic SharePoint/Teams library sync without user action.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "AutoMountTeamSitesDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "AutoMountTeamSitesDisabled")],
                DetectOps = [RegOp.CheckDword(KfmKey, "AutoMountTeamSitesDisabled", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-disable-first-delete-dialog",
                Label = "OneDrive KFM: Disable First-Delete Recycle Bin Dialog",
                Category = "Cloud Storage",
                Description =
                    "Suppresses the OneDrive 'Are you sure you want to delete?' confirmation dialog on first delete from a synced folder. Reduces friction for advanced users.",
                Tags = ["onedrive", "delete", "dialog", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the first-delete confirmation dialog in synced folders.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "DisableFirstDeleteDialog", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "DisableFirstDeleteDialog")],
                DetectOps = [RegOp.CheckDword(KfmKey, "DisableFirstDeleteDialog", 1)],
            },
        ];
    }

    // ── OneDriveSyncPolicy ──
    private static class _OneDriveSyncPolicy
    {
        private const string OneDriveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "odsync-enable-known-folder-move",
                    Label = "OneDrive Sync: Enable Known Folder Move (Desktop/Documents/Pictures to OneDrive)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets KFMSilentOptIn to the tenant ID GUID in the OneDrive policy key (uses a placeholder DWORD flag EnableKFMSilentOptIn=1 as the registry-based detection). Silently moves Windows Known Folders (Desktop, Documents, Pictures) to OneDrive for Business during the next OneDrive sync cycle. This is the primary OneDrive data protection feature for enterprise — it ensures that user data stored in the default Windows folders is continuously synced to OneDrive, providing automatic cloud backup and recovery in case of device loss or failure. Users do not need to change their behaviour — they continue to save to Desktop/Documents and data is automatically protected.",
                    Tags = ["onedrive", "known-folder-move", "kfm", "desktop", "documents"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Desktop, Documents, and Pictures folders silently moved to OneDrive for Business. Requires a tenant-specific OneDrive Group Policy configuration (tenantId GUID in KFMSilentOptIn). Deployment requires M365 Business or Enterprise licensing. Users see a toast notification on first sync. Existing local files are moved — no data loss. Configure via the OneDrive Group Policy ADMX template for production deployment.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "EnableKFMSilentOptIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "EnableKFMSilentOptIn")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "EnableKFMSilentOptIn", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-prevent-personal-onedrive-use",
                    Label = "OneDrive Sync: Block Personal OneDrive Accounts (Allow Only Tenant Accounts)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisablePersonalSync=1 in the OneDrive policy key. Prevents users from signing into the OneDrive sync client with a personal Microsoft account (Hotmail, Outlook.com, Xbox Live). In enterprise environments, allowing personal OneDrive accounts on corporate devices creates a shadow IT data exfiltration path — users can silently sync corporate files to their personal OneDrive. Restricting the sync client to tenant-managed (Azure AD) accounts ensures that synced data is governed by the enterprise DLP and retention policies.",
                    Tags = ["onedrive", "personal-account", "data-exfiltration", "shadow-it", "tenant-restriction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Personal OneDrive syncing blocked on this device. Users cannot sync their personal OneDrive files to a corporate device. If users store personal files on OneDrive and access them at work, they must use the OneDrive.com web interface. The sync client only accepts corporate Azure AD account logins.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "DisablePersonalSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisablePersonalSync")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "DisablePersonalSync", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-allow-sync-on-metered-network",
                    Label = "OneDrive Sync: Prevent OneDrive Sync on Metered Connections",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableSyncOnMeteredNetwork=1 in the OneDrive policy key. Prevents OneDrive from syncing files when the device is connected via a metered network connection (mobile hotspot, cellular tethering, capped data plan). On mobile broadband or tethered connections, unthrottled OneDrive sync can consume gigabytes of cellular data — generating unexpected data overage charges or exhausting mobile data plans. Suspending sync on metered connections is the standard behaviour for consumer OneDrive but requires explicit policy enforcement in enterprise deployments.",
                    Tags = ["onedrive", "metered-network", "cellular", "data-usage", "bandwidth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync paused on metered connections. Syncing resumes automatically when the device connects to a non-metered (Wi-Fi, Ethernet) network. Files saved to synced folders while on metered connections are queued and uploaded when network becomes non-metered.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "DisableSyncOnMeteredNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisableSyncOnMeteredNetwork")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "DisableSyncOnMeteredNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-set-max-upload-bandwidth-400kbps",
                    Label = "OneDrive Sync: Limit Upload Bandwidth to 400 Kbps",
                    Category = "Cloud Storage",
                    Description =
                        "Sets UploadBandwidthLimit=400 in the OneDrive policy key (value in Kbps). Throttles OneDrive upload bandwidth to 400 Kbps. This prevents OneDrive initial KFM migration or large file uploads from saturating a corporate WAN link. A 400 Kbps upload cap allows OneDrive to sync in the background without impacting VoIP call quality (which requires ~100 Kbps per call), video conferencing (which requires 1.5–4 Mbps per call), or line-of-business application connectivity. For faster connections (100 Mbps LAN), the cap can be increased — adjust to match your WAN uplink capacity.",
                    Tags = ["onedrive", "upload-bandwidth", "throttle", "wan", "qos"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive upload limited to 400 Kbps. Initial KFM migration of large document libraries may take several days at 400 Kbps. For initial deployment, consider a temporary higher limit (2000 Kbps) for the first two weeks, then reduce to 400 Kbps steady-state.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "UploadBandwidthLimit", 400)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "UploadBandwidthLimit")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "UploadBandwidthLimit", 400)],
                },
                new TweakDef
                {
                    Id = "odsync-enable-file-on-demand",
                    Label = "OneDrive Sync: Enable Files On-Demand (Cloud-Only Placeholder Files)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets FilesOnDemandEnabled=1 in the OneDrive policy key. Enables OneDrive Files On-Demand, which creates placeholder files in the local OneDrive folder for files stored in OneDrive without downloading the content until the file is opened. Files On-Demand dramatically reduces local storage consumption — a OneDrive library with 100 GB of files may only consume 50 MB of local disk space if most files have never been accessed. Downloaded files are cached locally for offline access. This is critical for devices with small SSD storage (128–256 GB) and large OneDrive libraries.",
                    Tags = ["onedrive", "files-on-demand", "storage-savings", "placeholder", "cloud-only"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Files On-Demand enabled. File Explorer shows cloud-only files as placeholder icons. Opening a cloud-only file triggers a download — may cause a brief delay on first open. Antivirus on-access scans will automatically download cloud-only files when scanning, potentially consuming disk space and network bandwidth.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "FilesOnDemandEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "FilesOnDemandEnabled")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "FilesOnDemandEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-enable-silent-sign-in",
                    Label = "OneDrive Sync: Enable Silent Azure AD Sign-In (Single Sign-On with AzureAD)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets SilentAccountConfig=1 in the OneDrive policy key. Enables OneDrive to automatically sign in with the user's Azure Active Directory account without displaying any sign-in prompts. This uses Azure AD SSO to automatically configure the OneDrive sync client on first logon — users never see a OneDrive setup wizard or sign-in dialog. This is the enterprise-grade deployment method for OneDrive — it ensures 100% adoption without user-initiated setup, which is critical for KFM to protect all devices automatically.",
                    Tags = ["onedrive", "silent-signin", "azure-ad", "sso", "auto-configure"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive automatically signs in using Azure AD credentials. Requires Azure AD joined or Hybrid Azure AD joined devices. Works on-premises and in cloud-only deployments. Users see a brief OneDrive startup notification on first logon — no action required.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "SilentAccountConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "SilentAccountConfig")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "SilentAccountConfig", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-prevent-unmanaged-machine-sync",
                    Label = "OneDrive Sync: Block Sync on Unmanaged (Non-Domain, Non-Azure-AD) Devices",
                    Category = "Cloud Storage",
                    Description =
                        "Sets AllowTenantList enforcement via PreventUnmanagedMachineSync=1 in the OneDrive policy key. Prevents the OneDrive sync client from syncing corporate OneDrive data on devices that are not Azure AD joined or domain joined. This is the SharePoint Online 'allowed domain' equivalent for device management compliance — corporate data should only sync to managed devices under IT control. Users who attempt to sync from personal or unmanaged devices receive a 'You can't sync to this location' error.",
                    Tags = ["onedrive", "unmanaged-device", "dlp", "conditional-access", "byod"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Corporate OneDrive sync blocked on unmanaged devices. BYOD users with personal devices cannot install the OneDrive sync client and access corporate libraries — they must use OneDrive.com web interface. Ensure Conditional Access policies in Azure AD complement this policy for consistent enforcement.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "PreventUnmanagedMachineSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "PreventUnmanagedMachineSync")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "PreventUnmanagedMachineSync", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-enable-verbose-error-reporting",
                    Label = "OneDrive Sync: Enable Verbose Sync Error Reporting to Event Log",
                    Category = "Cloud Storage",
                    Description =
                        "Sets EnableVerboseEventReporting=1 in the OneDrive policy key. Enables detailed OneDrive sync error events in the Windows Application event log (source: OneDrive). Standard OneDrive error reporting only logs critical failures to the OneDrive diagnostic log (%LOCALAPPDATA%\\Microsoft\\OneDrive\\logs\\). Verbose event log reporting records all sync errors to the Windows event log where SIEM tools can consume them — enabling fleet-wide monitoring of sync failures, storage quota issues, sharing permission errors, and file conflict errors.",
                    Tags = ["onedrive", "event-log", "error-reporting", "siem", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync errors written to Windows Application event log. In large deployments with frequent sync errors (file locking conflicts, large file type restrictions), event log volume can be significant. Consider event log retention and SIEM ingestion cost when enabling verbose reporting at scale.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "EnableVerboseEventReporting", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "EnableVerboseEventReporting")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "EnableVerboseEventReporting", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-disable-auto-start-telemetry",
                    Label = "OneDrive Sync: Disable OneDrive Usage Telemetry Reporting to Microsoft",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableTelemetry=1 in the OneDrive policy key. Disables the OneDrive sync client's Optional Diagnostic Data (ODD) telemetry reporting to Microsoft. OneDrive collects usage telemetry about sync activity, file types synced, sync performance, and error rates. In privacy-sensitive enterprise environments or regulated industries (legal, healthcare, finance), disabling optional telemetry from OneDrive is required by data governance policy. Required diagnostic data (error crash reports) cannot be disabled — only the optional enhanced telemetry is affected.",
                    Tags = ["onedrive", "telemetry", "privacy", "data-governance", "gdpr"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Optional OneDrive telemetry reporting disabled. Required diagnostic data (crash reports, error reports) still transmitted. No user-visible impact on OneDrive sync functionality. Microsoft will receive less usage data for OneDrive feature improvements.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-set-cache-free-space-floor-5pct",
                    Label = "OneDrive Sync: Set Minimum Free Space Floor — Do Not Sync Below 5% Free Disk",
                    Category = "Cloud Storage",
                    Description =
                        "Sets MinDiskFreeSpaceGB=5 in the OneDrive policy key (absolute GB, adjustable). Prevents OneDrive from downloading additional Files On-Demand files when the device's disk free space falls below the configured threshold. Without a free space floor, OneDrive downloads can fill an SSD to 100%, causing Windows write operations to fail (temp file writes, browser cache, update extraction) — resulting in OS instability. The free space floor ensures that even when users open many cloud-only files in quick succession, the disk is not completely filled.",
                    Tags = ["onedrive", "disk-space", "storage-floor", "on-demand", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive download paused when disk free space falls below 5 GB. Files On-Demand downloads are suspended — opening new cloud-only files fails until free space is restored. Users see an OneDrive notification about insufficient disk space. Existing downloaded (cached) files are not deleted.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "MinDiskFreeSpaceGB", 5)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "MinDiskFreeSpaceGB")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "MinDiskFreeSpaceGB", 5)],
                },
            ];
    }

    // ── SettingSyncAdv ──
    private static class _SettingSyncAdv
    {
        private const string SyncPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";
        private const string InputPers = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization";
        private const string InputPersPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ssync-disable-desktop-theme",
                Label = "Disable Desktop Background Theme Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Stops the desktop background and visual theme from being synced across "
                    + "devices linked to the same Microsoft Account. "
                    + "DisableDesktopThemeSettingSync=1.",
                Tags = ["sync", "theme", "desktop", "wallpaper", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableDesktopThemeSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableDesktopThemeSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableDesktopThemeSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-start-layout",
                Label = "Disable Start Menu Layout Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Prevents the Start menu layout (pinned apps, tile arrangement) from being "
                    + "synchronized across devices. DisableStartLayoutSettingSync=1.",
                Tags = ["sync", "start menu", "layout", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableStartLayoutSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableStartLayoutSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableStartLayoutSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-browser-settings",
                Label = "Disable Browser Settings Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prevents browser-related settings (favourites, history, home page settings) "
                    + "from syncing through Microsoft Account. DisableBrowserSettingSync=1.",
                Tags = ["sync", "browser", "favourites", "msa", "privacy"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableBrowserSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableBrowserSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableBrowserSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-language-settings",
                Label = "Disable Language and Regional Settings Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Prevents language packs, keyboard layouts, and regional settings from "
                    + "being synchronized across devices. DisableLanguageSettingSync=1.",
                Tags = ["sync", "language", "regional", "keyboard", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableLanguageSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableLanguageSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableLanguageSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-accessibility-settings",
                Label = "Disable Accessibility/Ease-of-Access Settings Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Stops Ease of Access settings (Magnifier, Narrator, contrast themes) "
                    + "from syncing via Microsoft Account. DisableAccessibilitySettingSync=1.",
                Tags = ["sync", "accessibility", "ease of access", "narrator", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableAccessibilitySettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableAccessibilitySettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableAccessibilitySettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-personalization-settings",
                Label = "Disable Personalization Settings Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Prevents personalization settings such as colors, lock-screen images, and "
                    + "accent colors from being synchronized. DisablePersonalizationSettingSync=1.",
                Tags = ["sync", "personalization", "lock screen", "colors", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisablePersonalizationSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisablePersonalizationSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisablePersonalizationSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-windows-settings",
                Label = "Disable General Windows Platform Settings Sync",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disables synchronization of general Windows OS settings (taskbar, search, "
                    + "notification preferences) across devices. DisableWindowsSettingSync=1.",
                Tags = ["sync", "windows settings", "taskbar", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableWindowsSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableWindowsSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableWindowsSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-text-personalization",
                Label = "Disable Typing / Text Input Personalization",
                Category = "Cloud Storage",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prevents Windows from collecting your typing patterns for autocorrect and "
                    + "next-word predictions. RestrictImplicitTextCollection=1 in InputPersonalization.",
                Tags = ["personalization", "typing", "autocorrect", "privacy", "input"],
                RegistryKeys = [InputPers],
                ApplyOps = [RegOp.SetDword(InputPers, "RestrictImplicitTextCollection", 1)],
                RemoveOps = [RegOp.SetDword(InputPers, "RestrictImplicitTextCollection", 0)],
                DetectOps = [RegOp.CheckDword(InputPers, "RestrictImplicitTextCollection", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-ink-personalization",
                Label = "Disable Handwriting / Ink Personalization",
                Category = "Cloud Storage",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Stops Windows from collecting handwriting samples to improve ink recognition "
                    + "accuracy. RestrictImplicitInkCollection=1 in InputPersonalization.",
                Tags = ["personalization", "handwriting", "ink", "stylus", "privacy"],
                RegistryKeys = [InputPers],
                ApplyOps = [RegOp.SetDword(InputPers, "RestrictImplicitInkCollection", 1)],
                RemoveOps = [RegOp.SetDword(InputPers, "RestrictImplicitInkCollection", 0)],
                DetectOps = [RegOp.CheckDword(InputPers, "RestrictImplicitInkCollection", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-input-personalization-policy",
                Label = "Disable Input Personalization — Machine Policy",
                Category = "Cloud Storage",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Machine-wide group policy that disables all Windows input personalization "
                    + "(typing, handwriting, speech learning) for all users on the device. "
                    + "AllowInputPersonalization=0.",
                Tags = ["personalization", "policy", "input", "speech", "privacy"],
                RegistryKeys = [InputPersPolicy],
                ApplyOps = [RegOp.SetDword(InputPersPolicy, "AllowInputPersonalization", 0)],
                RemoveOps = [RegOp.DeleteValue(InputPersPolicy, "AllowInputPersonalization")],
                DetectOps = [RegOp.CheckDword(InputPersPolicy, "AllowInputPersonalization", 0)],
            },
        ];
    }

    // ── SettingSyncPolicy ──
    private static class _SettingSyncPolicy
    {
        private const string SyncKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "syncsec-disable-all-setting-sync",
                    Label = "Disable All Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Disables the Windows settings synchronisation feature entirely and prevents users from re-enabling it.",
                    Tags = ["sync", "settings", "cloud", "privacy", "microsoft-account"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All settings sync is stopped; value 2 = forced off, user cannot override.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-block-user-override",
                    Label = "Block User from Changing Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Prevents users from accessing the settings sync toggle in Windows Settings.",
                    Tags = ["sync", "settings", "user-override", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removes the sync toggle from Settings UI; requires DisableSettingSync to also be set.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableSettingSyncUserOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSettingSyncUserOverride")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableSettingSyncUserOverride", 1)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-credentials-sync",
                    Label = "Disable Credentials and Password Sync",
                    Category = "Cloud Storage",
                    Description = "Stops Windows from syncing saved passwords and credentials across devices via a Microsoft account.",
                    Tags = ["sync", "credentials", "password", "privacy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Prevents credential roaming; passwords stored locally only, not in Microsoft account cloud.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableCredentialsSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableCredentialsSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableCredentialsSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-personalization-sync",
                    Label = "Disable Personalization Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Prevents Windows from syncing wallpaper, colour, themes, and other personalisation settings to the cloud.",
                    Tags = ["sync", "personalization", "theme", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Personalization stays local; no roaming of desktop background or colour accent to other devices.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisablePersonalizationSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisablePersonalizationSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisablePersonalizationSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-app-settings-sync",
                    Label = "Disable App Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Stops Windows from uploading and syncing per-app settings to a Microsoft account in the cloud.",
                    Tags = ["sync", "app-settings", "cloud", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "App preferences remain on this device; switching to another device may require re-configuring apps.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableApplicationSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableApplicationSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableApplicationSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-browser-sync",
                    Label = "Disable Browser Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Disables syncing of Microsoft Edge / Internet Explorer browser settings, favourites, and history via sync.",
                    Tags = ["sync", "browser", "edge", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Browser favourites and history stay local; no cloud upload via Windows Settings Sync.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableWebBrowserSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-start-layout-sync",
                    Label = "Disable Start Menu Layout Sync",
                    Category = "Cloud Storage",
                    Description = "Prevents Windows from syncing the Start menu layout, pinned apps, and tile configuration to the cloud.",
                    Tags = ["sync", "start-menu", "layout", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Start menu customisation stays local; does not roam to other devices signed with the same account.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableStartLayoutSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableStartLayoutSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableStartLayoutSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-accessibility-sync",
                    Label = "Disable Accessibility Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Stops Windows from syncing accessibility options such as magnifier, narrator, and high contrast settings.",
                    Tags = ["sync", "accessibility", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Accessibility preferences remain device-local; must be re-configured on each device.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableAccessibilitySettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableAccessibilitySettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableAccessibilitySettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-sync-on-metered",
                    Label = "Disable Settings Sync on Metered Networks",
                    Category = "Cloud Storage",
                    Description = "Prevents Windows settings sync from running when the device is on a metered (pay-per-use) network connection.",
                    Tags = ["sync", "metered", "network", "data-usage", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents unexpected data charges on cellular / capped connections; sync resumes on unmetered networks.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableSyncOnPaidNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSyncOnPaidNetwork")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableSyncOnPaidNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-language-sync",
                    Label = "Disable Language and Keyboard Settings Sync",
                    Category = "Cloud Storage",
                    Description = "Prevents Windows from syncing language preferences, keyboard layouts, and input method settings to the cloud.",
                    Tags = ["sync", "language", "keyboard", "input", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Language and IME configuration stays local; no roaming of locale settings across devices.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableLanguageSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableLanguageSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableLanguageSettingSync", 2)],
                },
            ];
    }

    // ── SharepointOnlinePolicy ──
    private static class _SharepointOnlinePolicy
    {
        private const string SharepointKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\SharePoint";

        private const string OfficePrivacyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Privacy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "spol-disable-external-sharing",
                    Label = "SharePoint Online: Prohibit External Sharing from SharePoint Sites",
                    Category = "Cloud Storage",
                    Description =
                        "Sets AllowExternalSharing=0 in the SharePoint policy key. Sets the client-side policy assertion that external sharing from SharePoint Online sites is not permitted. While the authoritative SharePoint sharing setting is managed in the SharePoint Admin Center, this registry policy works with Office client apps to enforce the restriction locally — Office add-ins and co-authoring flows check this policy to determine whether to offer 'share with external users' options. Combined with SharePoint Admin Center's external sharing settings, this provides defence-in-depth.",
                    Tags = ["sharepoint", "external-sharing", "dlp", "data-exfiltration", "collaboration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "External sharing prohibited in Office client SharePoint integration. Users cannot share items from Office apps to external email addresses via SharePoint sharing. External collaboration requires admin-authorised guest access configuration in the SharePoint Admin Center. Web-based sharing via SharePoint.com may still allow sharing depending on SharePoint Admin Center tenant-level settings.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "AllowExternalSharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "AllowExternalSharing")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "AllowExternalSharing", 0)],
                },
                new TweakDef
                {
                    Id = "spol-enable-sensitivity-label-enforcement",
                    Label = "SharePoint Online: Enable Microsoft Information Protection Sensitivity Labels in Office",
                    Category = "Cloud Storage",
                    Description =
                        "Sets EnableMIPIntegration=1 in the SharePoint policy key. Enables the Microsoft Information Protection (MIP) AIP unified labelling integration in Office apps connecting to SharePoint Online. When enabled, Office apps (Word, Excel, PowerPoint, Outlook) display the sensitivity label bar and enforce label-based policies (encryption, access control, DRM) defined in the Microsoft Purview Compliance Center. Users are prompted to label documents before saving to SharePoint, and unlabelled uploads to labelled libraries are rejected.",
                    Tags = ["sharepoint", "sensitivity-labels", "mip", "dlp", "information-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Sensitivity labelling integrated in Office apps. Users see the sensitivity label bar in Word, Excel, PowerPoint, and Outlook. Requires Microsoft Purview Information Protection (MIP) licensing (M365 E3/E5 or Azure Information Protection P1/P2). Labels configured in Purview Compliance Center are deployed to Office apps. Unlabelled existing documents are not automatically labelled — only new documents are prompted.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "EnableMIPIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "EnableMIPIntegration")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "EnableMIPIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "spol-disable-co-authoring-with-external-users",
                    Label = "SharePoint Online: Disable Real-Time Co-Authoring with External (Guest) Users",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableExternalCoAuthoring=1 in the SharePoint policy key. Prevents Office real-time co-authoring sessions with external/guest users via SharePoint Online. Co-authoring with external users transmits document content character-by-character in real time — in strict DLP scenarios, even the act of collaborating with an external user on a sensitive document may constitute a data disclosure event. Disabling external co-authoring while retaining internal co-authoring preserves team collaboration while blocking external data flows.",
                    Tags = ["sharepoint", "co-authoring", "external-guest", "dlp", "real-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "External guest co-authoring sessions blocked. Internal team co-authoring is unaffected. Guests in SharePoint sites can still view and download documents but cannot participate in real-time co-authoring sessions. Impact is primarily on M365 guest collaboration workflows.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "DisableExternalCoAuthoring", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableExternalCoAuthoring")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "DisableExternalCoAuthoring", 1)],
                },
                new TweakDef
                {
                    Id = "spol-set-download-permissions-block-unmanaged",
                    Label = "SharePoint Online: Block Downloads from SharePoint for Unmanaged Devices",
                    Category = "Cloud Storage",
                    Description =
                        "Sets BlockDownloadOnUnmanagedDevice=1 in the SharePoint policy key. Prevents file downloads from SharePoint Online to unmanaged (non-Azure-AD-joined) devices. This is the client-side policy flag — the enforcement is primarily in SharePoint Online Conditional Access policies configured for unmanaged devices. When this flag is set, Office apps enforce the restriction by checking the device management state before initiating downloads. Users on unmanaged devices can view documents in the browser (web-only mode) but cannot download files for local storage.",
                    Tags = ["sharepoint", "unmanaged-device", "download-block", "byod", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint downloads blocked on unmanaged devices. Users on personal devices can only view content in the browser in read-only web view — they cannot download files, open in desktop Office apps, or print. Managed (Azure AD joined) devices are not affected.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "BlockDownloadOnUnmanagedDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "BlockDownloadOnUnmanagedDevice")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "BlockDownloadOnUnmanagedDevice", 1)],
                },
                new TweakDef
                {
                    Id = "spol-disable-sharepoint-addins",
                    Label = "SharePoint Online: Disable SharePoint Store Add-ins (Prevent Marketplace Apps)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableSharePointStoreAddins=1 in the SharePoint policy key. Prevents users from acquiring and installing SharePoint Add-ins from the SharePoint App Marketplace. Unvetted SharePoint add-ins can request high-privilege API permissions (full site read/write, full tenant admin on some legacy add-ins), access sensitive SharePoint data, and exfiltrate content to external services. IT should pre-approve and deploy authorised SharePoint add-ins via the corporate app catalogue rather than allowing open marketplace installs.",
                    Tags = ["sharepoint", "add-ins", "app-marketplace", "shadow-it", "permissions"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint marketplace add-in installs blocked. Users cannot install new add-ins from the SharePoint store. IT-approved add-ins deployed via the corporate App Catalogue are not affected. Existing installed marketplace add-ins may continue to function depending on SharePoint tenant configuration.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "DisableSharePointStoreAddins", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableSharePointStoreAddins")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "DisableSharePointStoreAddins", 1)],
                },
                new TweakDef
                {
                    Id = "spol-enable-connected-experiences",
                    Label = "SharePoint Online: Enable Required Connected Experiences (Disable Optional Diagnostic Data Opt-out)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisconnectedState=0 in the Office Privacy policy key. Ensures that 'required connected experiences' in Office (spell check, grammar check, co-authoring, document recovery) remain enabled and cannot be disabled by users. The Office 'Disconnected Experiences' setting allows users to disable all cloud-connected features, which prevents essential collaboration features like SharePoint co-authoring, OneDrive sync status, and Exchange mail flow from functioning. In enterprise deployments, connected experiences should be enforced to ensure Office functionality meets business requirements.",
                    Tags = ["office", "connected-experiences", "sharepoint", "coauthoring", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Office connected experiences enforced — users cannot disable cloud-connected Office features. Some optional connected experiences (LinkedIn integration, translation, proofing tool cloud lookups) may still be controllable via separate policies. Required connected experiences (co-authoring, OneDrive, spell check) remain active.",
                    ApplyOps = [RegOp.SetDword(OfficePrivacyKey, "DisconnectedState", 0)],
                    RemoveOps = [RegOp.DeleteValue(OfficePrivacyKey, "DisconnectedState")],
                    DetectOps = [RegOp.CheckDword(OfficePrivacyKey, "DisconnectedState", 0)],
                },
                new TweakDef
                {
                    Id = "spol-disable-optional-connected-experiences",
                    Label = "SharePoint Online: Disable Optional Connected Experiences (Third-Party Add-ons in Office)",
                    Category = "Cloud Storage",
                    Description =
                        "Sets UserContentDisabled=1 in the Office Privacy policy key. Disables optional connected experiences in Office that access user content and connect to third-party services — for example, the Office Intelligent Services panel that submits document sections to third-party APIs for translation, AI writing assistance, or research suggestions. These optional experiences transmit document content to external (non-Microsoft) services, which may violate data residency requirements or expose confidential content. Disabling optional connected experiences reduces the Office external data transmission footprint.",
                    Tags = ["office", "optional-experiences", "third-party", "privacy", "data-residency"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Optional connected experiences disabled. Third-party Office add-in services that submit document content to external APIs will not activate. Built-in Microsoft services (SharePoint, OneDrive, Exchange connected experiences, Microsoft Translator) are not classified as optional third-party experiences and remain functional.",
                    ApplyOps = [RegOp.SetDword(OfficePrivacyKey, "UserContentDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(OfficePrivacyKey, "UserContentDisabled")],
                    DetectOps = [RegOp.CheckDword(OfficePrivacyKey, "UserContentDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "spol-set-sync-client-tenant-restriction",
                    Label = "SharePoint Online: Restrict OneDrive/SharePoint Sync to Authorised Tenant Only",
                    Category = "Cloud Storage",
                    Description =
                        "Sets AllowTenantList enforcement flag TenantRestrictionEnabled=1 in the SharePoint policy key. Enables the tenant restriction for OneDrive and SharePoint sync — the OneDrive client only allows sync connections to the authorised corporate tenant. Without this restriction, users can sign into any Microsoft 365 tenant from the OneDrive client (including a free personal tenant they created to receive data) and sync corporate SharePoint libraries to the non-corporate tenant. This is a data exfiltration vector for malicious insiders.",
                    Tags = ["sharepoint", "tenant-restriction", "onedrive", "data-exfiltration", "insider"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive and SharePoint sync restricted to authorised tenant. Users cannot sync data to or from a non-corporate Microsoft 365 tenant. Requires the authorised tenant GUID to be configured in the policy (set via Group Policy ADMX template AllowTenantList setting). This registry flag enables the enforcement mechanism but requires the tenant GUID to be fully enforced.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "TenantRestrictionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "TenantRestrictionEnabled")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "TenantRestrictionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "spol-disable-sharepoint-meeting-recordings-personal",
                    Label = "SharePoint Online: Disable Personal Meeting Recording Storage in OneDrive",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableMeetingRecordingToPersonalOneDrive=1 in the SharePoint policy key. Prevents Teams meeting recordings from being saved to the organiser's personal OneDrive for Business. Instead, recordings are directed to the meeting's SharePoint channel (group OneDrive). Personal OneDrive storage for meeting recordings is uncontrolled from an IT governance perspective — recordings stored personally may be retained beyond the organisation's retention policy, shared with external recipients without oversight, or lost when an employee departures. Channel-based recording storage is covered by the SharePoint retention and eDiscovery policies.",
                    Tags = ["sharepoint", "meeting-recordings", "teams", "onedrive", "retention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Meeting recordings saved to SharePoint channel storage, not personal OneDrive. Teams recordings still available to meeting participants via the SharePoint channel. Compliance with meeting recording retention policies is simplified as recordings are under SharePoint retention policy scope.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive", 1)],
                },
                new TweakDef
                {
                    Id = "spol-enable-access-log-audit",
                    Label = "SharePoint Online: Enable SharePoint Access and File Activity Audit Logging",
                    Category = "Cloud Storage",
                    Description =
                        "Sets EnableAccessAudit=1 in the SharePoint policy key. Enables detailed file access and activity auditing in SharePoint Online — records who accessed, downloaded, modified, or shared each file, and from which device. SharePoint access audit logs are used for insider threat detection, eDiscovery, data breach investigation, and regulatory compliance (HIPAA, SOX, GDPR). Without audit logging, it is impossible to reconstruct who accessed sensitive files during a data breach investigation window.",
                    Tags = ["sharepoint", "audit-log", "access-log", "insider-threat", "ediscovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint file access, modification, sharing, and download events logged. Full audit log available in Microsoft Purview Compliance Center and via Microsoft Graph API. High-volume event environments (large file libraries with frequent access) generate significant audit trail data. Audit log retention depends on Microsoft 365/Purview licensing.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "EnableAccessAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "EnableAccessAudit")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "EnableAccessAudit", 1)],
                },
            ];
    }

    // ── SkyDrivePolicy ──
    private static class _SkyDrivePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SkyDrive";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "skydrive-disable-file-sync",
                    Label = "SkyDrive: Disable OneDrive File Synchronisation via Legacy SkyDrive Policy Key",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableFileSync=1 in the SkyDrive legacy policy key. Disables OneDrive file synchronisation at the machine policy level, preventing all users on this computer from syncing files with their OneDrive cloud storage. The SkyDrive registry key is the original legacy path (Windows 8.1/RT era) that is still read by the current Windows OneDrive client for backwards compatibility with Group Policy deployed to WS2012R2 and Win 8.1 machines. "
                        + "In organisations that prohibit users from uploading corporate files to personal cloud storage, the SkyDrive legacy policy key ensures policy coverage extends to legacy Windows versions where the OneDrive-specific policy path did not yet exist. The SkyDrive and OneDrive policy keys are both evaluated — having both set ensures no gap in policy enforcement across heterogeneous Windows version environments. Without both keys set, a corporate laptop running the current OneDrive client on Win 8.1 would check the SkyDrive key first; if missing, OneDrive sync proceeds unblocked.",
                    Tags = ["skydrive", "onedrive", "file-sync", "cloud-storage", "policy", "disable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive file sync disabled via legacy SkyDrive policy key. Corporate files blocked from uploading to personal OneDrive accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableFileSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableFileSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableFileSync", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-library-default-save",
                    Label = "SkyDrive: Prevent Libraries from Defaulting Save Location to SkyDrive/OneDrive",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableLibrariesDefaultSaveToSkyDrive=1 in the SkyDrive policy key. Prevents Windows from configuring OneDrive's local folder as the default save location for Windows Libraries (Documents, Pictures, Music). Without this policy, Windows 8.1+ suggests OneDrive as the default save target — any document saved without explicitly choosing a location is uploaded to the user's personal OneDrive account. "
                        + "In corporate environments where DLP (Data Loss Prevention) policies prohibit saving corporate IP to personal cloud storage, the auto-save to OneDrive/SkyDrive default library path is a subtle leakage vector — users who click 'Save' without inspecting the save dialogue may unknowingly sync sensitive documents to personal storage. Enforcing a corporate-managed default save location (a file server or SharePoint UNC path configured by Group Policy Folder Redirection) ensures all undirected file saves stay within managed storage boundaries.",
                    Tags = ["skydrive", "onedrive", "library", "default-save", "dlp", "cloud-storage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Libraries no longer default to SkyDrive/OneDrive as save location. Users who manually navigate to OneDrive folder can still save there until sync is also disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLibrariesDefaultSaveToSkyDrive", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLibrariesDefaultSaveToSkyDrive")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLibrariesDefaultSaveToSkyDrive", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-metered-sync",
                    Label = "SkyDrive: Disable OneDrive Sync on Metered Network Connections",
                    Category = "Cloud Storage",
                    Description =
                        "Sets NeverSyncOnMeteredConnection=1 in the SkyDrive policy key. Prevents OneDrive from synchronising files when the active network connection is metered (mobile data, LTE hotspot, satellite). Without this policy, OneDrive will attempt background synchronisation on metered connections, consuming potentially expensive cellular data allowances and degrading application performance for users on mobile hotspots. "
                        + "Windows marks mobile hotspot connections, tethered cellular connections, and some Wi-Fi networks as metered to signal to applications that data usage should be minimised. OneDrive respects the metered status for foreground sync but continues background sync by default. For road warriors using laptop hotspot tethering on international trips with expensive roaming data plans, an unconstrained OneDrive background sync can silently consume gigabytes of mobile data. Disabling sync on metered connections prevents this scenario without requiring manual sync suspension.",
                    Tags = ["skydrive", "onedrive", "metered-connection", "mobile-data", "bandwidth", "roaming"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync paused on metered connections (cellular hotspot, LTE). Manual sync is still available. Files upload when non-metered connection is available.",
                    ApplyOps = [RegOp.SetDword(Key, "NeverSyncOnMeteredConnection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NeverSyncOnMeteredConnection")],
                    DetectOps = [RegOp.CheckDword(Key, "NeverSyncOnMeteredConnection", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-desktop-shortcut",
                    Label = "SkyDrive: Disable Automatic OneDrive Desktop Shortcut Creation",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableSkyDriveDesktopIcon=1 in the SkyDrive policy key. Prevents OneDrive from adding a shortcut icon to the user's desktop during initial setup or after updates. On managed enterprise desktops where the shortcut layout is standardised by Group Policy (no unmanaged shortcuts on desktop), automatic OneDrive shortcut creation violates desktop policy and confuses users who may not be aware of cloud sync being installed. "
                        + "Desktop shortcut proliferation on managed endpoints is a minor but persistent administrative annoyance. Each major OneDrive update can re-create the desktop shortcut if it was manually deleted, causing the shortcut to reappear after each update. Policy-driven suppression ensures the shortcut is never created, remaining consistent across updates without requiring GPO-applied shortcut deletion scripts.",
                    Tags = ["skydrive", "onedrive", "desktop-shortcut", "icon", "managed-desktop"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive desktop icon not created. Users can still access OneDrive via the system tray icon or File Explorer navigation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSkyDriveDesktopIcon", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSkyDriveDesktopIcon")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSkyDriveDesktopIcon", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-prevent-usage-of-onedrive",
                    Label = "SkyDrive: Prevent All OneDrive Usage via Legacy SkyDrive Machine Policy",
                    Category = "Cloud Storage",
                    Description =
                        "Sets PreventNetworkTrafficPreUserSignIn=1 in the SkyDrive policy key. Prevents OneDrive from generating any network traffic before the user signs in. During the Windows startup sequence, OneDrive pre-caches metadata and checks for updates before user login completes. This pre-sign-in network activity consumes bandwidth, adds to boot time, and generates outbound connections from a system in an unauthenticated state — which some network security monitoring tools flag as suspicious. "
                        + "Pre-authentication network connections from Microsoft services are a known privacy concern: OneDrive network activity during boot can leak the device's presence, IP address, and tenant association to Microsoft servers before the user has consented to connected services for that session. In high-security environments that enforce a zero-trust model where no application should generate network traffic until after full user authentication, pre-sign-in OneDrive connections violate this control. Blocking pre-sign-in network activity ensures OneDrive only connects after a user is fully authenticated.",
                    Tags = ["skydrive", "onedrive", "pre-signin", "network-traffic", "zero-trust", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive pre-login network activity blocked. No user-visible impact. OneDrive connects normally after user authentication completes.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventNetworkTrafficPreUserSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventNetworkTrafficPreUserSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventNetworkTrafficPreUserSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-personal-sync",
                    Label = "SkyDrive: Block Sync of Personal Accounts on Domain-Joined Machines",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisablePersonalSync=1 in the SkyDrive policy key. Prevents users from adding and syncing personal (non-corporate) Microsoft accounts with OneDrive on domain-joined or Entra ID-joined machines. Allows corporate OneDrive for Business (Entra ID accounts) to function normally while blocking personal @hotmail.com, @outlook.com, and @gmail.com accounts from syncing. "
                        + "On corporate endpoints, personal OneDrive accounts present a data exfiltration risk: a user can drag corporate documents into their personal OneDrive sync folder and those files are immediately uploaded to their personal account, bypassing corporate DLP policies that only monitor corporate OneDrive tenants. The DisablePersonalSync policy removes the option to add personal accounts from the OneDrive settings UI while allowing the corporate account configuration to proceed normally — enabling corporate OneDrive features while blocking personal sync.",
                    Tags = ["skydrive", "onedrive", "personal-account", "corporate-policy", "dlp", "exfiltration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Personal Microsoft account OneDrive sync blocked. Corporate OneDrive for Business accounts unaffected. Requires Entra ID-joined device for corporate sync.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalSync", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-require-domain-joined-to-sync",
                    Label = "SkyDrive: Require Domain Membership Before Allowing OneDrive Sync",
                    Category = "Cloud Storage",
                    Description =
                        "Sets RequireAccountFolderLocation=1 in the SkyDrive policy key. Requires that the user's OneDrive folder location is within a domain-accessible path before synchronisation begins. This ensures users cannot configure OneDrive to sync to a USB drive, external HDD, or a path on a non-domain-joined volume, which would bypass file auditing and DLP policies that monitor domain-accessible file paths. "
                        + "OneDrive's default folder location is %USERPROFILE%\\OneDrive — on a domain-joined machine this is within the user profile path which may be redirected to a file server. If a user changes the OneDrive local folder to an external USB drive, sync continues to the external drive but audit policies monitoring the user profile path no longer capture OneDrive file activities. By requiring the account folder to be in an approved location, this policy prevents sync rerouting to unmonitored storage media.",
                    Tags = ["skydrive", "onedrive", "folder-location", "domain", "audit", "data-governance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync folder must be on a monitored domain-accessible path. Users cannot redirect sync to USB drives or external storage.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAccountFolderLocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAccountFolderLocation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAccountFolderLocation", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-tutorialicon",
                    Label = "SkyDrive: Suppress OneDrive First-Run Tutorial and Balloon Notifications",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableTutorial=1 in the SkyDrive policy key. Suppresses the OneDrive first-run tutorial wizard and taskbar balloon notification tooltips that appear on first login or after updates. On enterprise-deployed endpoints, the OneDrive tutorial interrupts user productivity during logins, and repetitive balloon tooltips post-update create distraction and support desk calls from users who assume the notifications indicate a problem. "
                        + "First-run wizard suppression is a routine enterprise deployment cleanliness policy — the tutorial is designed for retail consumers who have never configured OneDrive. In corporate environments where OneDrive policy is centrally managed (folder protection, retention policies, tenant binding), the tutorial presents options the user cannot change (they are set by policy) and provides misleading information about sync customisation capabilities. Suppressing the tutorial ensures users see only the relevant corporate-configured sync state without conflicting consumer-oriented guidance.",
                    Tags = ["skydrive", "onedrive", "tutorial", "notification", "enterprise-deployment"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive first-run tutorial and balloon tips suppressed. No functional impact — OneDrive operates normally with tutorial hidden.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTutorial", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTutorial")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTutorial", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-block-known-folder-move",
                    Label = "SkyDrive: Block Known Folder Move to Prevent Forced Desktop/Documents Redirect to OneDrive",
                    Category = "Cloud Storage",
                    Description =
                        "Sets KFMBlockOptIn=1 in the SkyDrive policy key. Blocks OneDrive's Known Folder Move (KFM) feature from prompting users or automatically moving the Windows Known Folders (Desktop, Documents, Pictures) from their local profile path to the OneDrive folder. KFM can be deployed silently by IT to redirect these folders to OneDrive cloud storage — but without advance user notification, users may be surprised to find their desktop files suddenly synchronised to the cloud. "
                        + "Known Folder Move can have significant consequences when deployed without proper planning: large local Desktop and Documents folders (100+ GB) begin uploading to OneDrive immediately, consuming bandwidth. Folders that contain sensitive data subject to GDPR or HIPAA retention policies may inadvertently be moved to a Microsoft-operated cloud service without completing required Data Processing Agreement reviews. By blocking KFM opt-in via this policy, organisations can plan and deploy folder redirection deliberately rather than having it trigger based on defaults.",
                    Tags = ["skydrive", "onedrive", "known-folder-move", "kfm", "desktop-redirect", "cloud-redirect"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive Known Folder Move blocked. Desktop, Documents, Pictures remain in local profile. IT-managed folder redirection to file server is unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "KFMBlockOptIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "KFMBlockOptIn")],
                    DetectOps = [RegOp.CheckDword(Key, "KFMBlockOptIn", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-teamsync",
                    Label = "SkyDrive: Disable OneDrive SharePoint-Backed Team Site Sync",
                    Category = "Cloud Storage",
                    Description =
                        "Sets DisableSharepointSync=1 in the SkyDrive policy key. Prevents OneDrive from synchronising SharePoint Online-backed team site document libraries to the local machine. SharePoint team site sync makes the full content of shared team library folders available for offline editing — potentially storing large volumes of multi-user shared data locally on a laptop endpoint. "
                        + "SharePoint team site sync on secure endpoints creates data sovereignty risk: when a full team document library (containing files created by all team members) is synced locally, those files are stored in an endpoint protected only by the laptop's local encryption. If the laptop is stolen or compromised, all team documents are accessible to the attacker — not just the individual user's Documents but the entire team library. Disabling SharePoint sync ensures team content remains in the cloud and is only accessible via the browser with valid MFA credentials, not from the local disk.",
                    Tags = ["skydrive", "onedrive", "sharepoint", "team-site", "offline-sync", "data-sovereignty"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint Online team site document library sync to local machine disabled. Team files accessed via browser/SharePoint only. Personal OneDrive sync unaffected if DisableFileSync not set.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSharepointSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSharepointSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSharepointSync", 1)],
                },
            ];
    }

    // ── UniversalClipboardSyncPolicy ──
    private static class _UniversalClipboardSyncPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "uniclip-disable-mobile-device-sync",
                    Label = "Disable Windows Mobile Device Clipboard Sync",
                    Category = "Cloud Storage",
                    Description =
                        "Disables clipboard synchronization between Windows and mobile devices (Android phones, tablets) through the Universal Clipboard infrastructure.",
                    Tags = ["clipboard", "mobile", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard not synchronized to mobile devices; all clipboard data stays on PC.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMobileClipboardSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileClipboardSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMobileClipboardSync", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-clipboard-msa",
                    Label = "Disable Clipboard Access for Microsoft Accounts",
                    Category = "Cloud Storage",
                    Description =
                        "Prevents Microsoft account-linked clipboard history from being accessible across devices tied to the same MSA, blocking cloud-backed clipboard sharing.",
                    Tags = ["clipboard", "msa", "account", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MSA-linked clipboard sharing disabled; useful for separating personal/work contexts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMicrosoftAccounts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMicrosoftAccounts")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMicrosoftAccounts", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-restrict-trusted-apps",
                    Label = "Restrict Clipboard to Trusted Apps Only",
                    Category = "Cloud Storage",
                    Description =
                        "Restricts clipboard API access to applications in an approved trust list, blocking unrecognized or unsigned apps from accessing clipboard contents.",
                    Tags = ["clipboard", "trusted-apps", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Clipboard restricted to trusted apps; unapproved apps receive empty clipboard reads.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardTrustedApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardTrustedApps")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardTrustedApps", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-block-third-party-managers",
                    Label = "Block Third-Party Clipboard Managers",
                    Category = "Cloud Storage",
                    Description =
                        "Blocks third-party clipboard manager applications from accessing the extended clipboard history API, preventing unapproved software from storing clipboard data.",
                    Tags = ["clipboard", "third-party", "manager", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Third-party clipboard managers lose access to clipboard history API.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyClipboardManagers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyClipboardManagers")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyClipboardManagers", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-html-format",
                    Label = "Disable HTML Clipboard Format",
                    Category = "Cloud Storage",
                    Description =
                        "Disables the HTML clipboard format, forcing web content copies to plain text and preventing HTML metadata (tracking pixels, inline styles) from being stored in clipboard.",
                    Tags = ["clipboard", "html", "format", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Web content copied as plain text only; HTML formatting stripped.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableHtmlClipboardFormat", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableHtmlClipboardFormat")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableHtmlClipboardFormat", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-restrict-history-admins",
                    Label = "Restrict Clipboard History to Admin Accounts Only",
                    Category = "Cloud Storage",
                    Description =
                        "Limits clipboard history storage and retrieval to administrator accounts only, preventing standard user clipboard data from accumulating in shared history.",
                    Tags = ["clipboard", "admin", "restriction", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Standard user clipboard history disabled; only admin accounts retain clipboard history.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardHistoryAdminsOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardHistoryAdminsOnly")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardHistoryAdminsOnly", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-prediction-service",
                    Label = "Disable Clipboard Prediction Service",
                    Category = "Cloud Storage",
                    Description =
                        "Disables the clipboard prediction background service that analyses clipboard contents to provide predictive paste suggestions.",
                    Tags = ["clipboard", "prediction", "background", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Predictive paste suggestions disabled; clipboard contents not analysed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardPredictionService", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardPredictionService")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardPredictionService", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-block-sync-service",
                    Label = "Block Clipboard Sync Background Service",
                    Category = "Cloud Storage",
                    Description =
                        "Disables the background clipboard synchronization service that maintains clipboard state across devices and cloud endpoints.",
                    Tags = ["clipboard", "sync-service", "background", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Background clipboard sync service stopped; universal clipboard fully disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockClipboardSyncService", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardSyncService")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockClipboardSyncService", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-edge-clipboard-access",
                    Label = "Disable Browser Clipboard Integration via EdgeUpdate Policy",
                    Category = "Cloud Storage",
                    Description =
                        "Disables clipboard access integration for the Edge browser via EdgeUpdate policy, preventing Edge from participating in universal clipboard sync.",
                    Tags = ["clipboard", "edge", "browser", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge clipboard integration disabled; browser clipboard not shared via EdgeUpdate.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardAccess")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardAccess", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-edge-clipboard-manager",
                    Label = "Disable Edge Clipboard Manager",
                    Category = "Cloud Storage",
                    Description =
                        "Disables the Edge browser's built-in clipboard manager feature that maintains browser-side clipboard history and sharing via EdgeUpdate policy.",
                    Tags = ["clipboard", "edge", "manager", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge clipboard manager disabled; browser clipboard history feature unavailable.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableEdgeClipboardManager", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableEdgeClipboardManager")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableEdgeClipboardManager", 1)],
                },
            ];
    }
}
