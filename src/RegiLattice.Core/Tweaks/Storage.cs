namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Storage
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "stor-storage-disable-hibernation",
            Label = "Disable Hibernation File",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows hibernation file (hiberfil.sys) to reclaim disk space. The file can consume several GB. Sleep mode remains available. Default: enabled. Recommended: disabled on desktops and SSD-only machines.",
            Tags = ["storage", "hibernation", "disk", "space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-reserved",
            Label = "Disable Reserved Storage",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the 7 GB reserved storage partition that Windows keeps for updates and temp files. Frees disk space on small drives. Takes effect after the next feature update. Default: enabled. Recommended: disabled on space-constrained devices.",
            Tags = ["storage", "reserved", "disk", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-storage-sense",
            Label = "Disable Storage Sense Auto-Cleanup",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Storage Sense, the automatic disk cleanup feature that deletes temp files and Recycle Bin content on a schedule. Prevents unintended file removal. Default: enabled. Recommended: disabled if you manage cleanup manually.",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-recycle-confirm",
            Label = "Disable Recycle Bin Confirmation Dialog",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the confirmation prompt when deleting files to the Recycle Bin. Files still go to the Recycle Bin and can be restored. Default: enabled. Recommended: disabled for faster workflow.",
            Tags = ["storage", "recycle-bin", "confirmation", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ConfirmFileDelete", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-thumbs-db",
            Label = "Disable Thumbs.db on Network Folders",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from creating hidden Thumbs.db thumbnail cache files in network folders. Avoids lock conflicts and clutter on shared drives. Default: enabled (Thumbs.db created). Recommended: disabled.",
            Tags = ["storage", "thumbs", "network", "cache", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-compact-os",
            Label = "Enable Compact OS Compression Flag",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the Compact OS registry flag to prefer OS file compression. Can save 1-2 GB on the system drive. For full effect run 'compact /compactos:always' from an elevated prompt. Default: disabled. Recommended: enabled on small SSDs.",
            Tags = ["storage", "compact", "compression", "disk", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "CompactOsEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "CompactOsEnabled", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "CompactOsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-prefetch",
            Label = "Disable Prefetch and Superfetch",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Prefetch and Superfetch (SysMain) caching mechanisms. On SSD systems these provide negligible benefit and consume disk I/O. Default: enabled (3). Recommended: disabled on SSD-only machines.",
            Tags = ["storage", "prefetch", "superfetch", "sysmain", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnableSuperfetch", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 3),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnableSuperfetch", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-optimize-ntfs-memory",
            Label = "NTFS Memory Usage High",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets NtfsMemoryUsage to 2 (high), allowing NTFS to use more paged pool memory for caching. Improves file system performance on machines with ample RAM. Default: 1 (normal). Recommended: 2 on workstations with 16 GB+ RAM.",
            Tags = ["storage", "ntfs", "memory", "performance", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-last-access",
            Label = "Disable NTFS Last Access Timestamp",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables updating the last access timestamp on NTFS files and directories. Reduces disk writes and improves I/O performance on busy volumes. Default: system-managed (0x80000002). Recommended: disabled (0x80000003) on SSDs.",
            Tags = ["storage", "ntfs", "last-access", "timestamp", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 0)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-8dot3",
            Label = "Disable 8.3 Short Filename Creation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic creation of legacy 8.3 short filenames on NTFS volumes. Improves directory enumeration speed on volumes with many files. Default: enabled (0). Recommended: disabled unless legacy 16-bit apps are needed.",
            Tags = ["storage", "ntfs", "8dot3", "short-name", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Tells Windows to favor the system file cache over application working sets. Beneficial for file-server workloads and large sequential reads. Default: disabled (0). Recommended: enabled on file servers or 16 GB+ workstations.",
            Tags = ["storage", "cache", "memory", "file-server", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-enable-long-paths",
            Label = "Enable Win32 Long Path Support",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Win32 long path support, removing the 260-character path length limit for applications that declare long-path awareness in their manifest. Default: disabled. Recommended: enabled for developers and deep directory trees.",
            Tags = ["storage", "long-path", "260", "developer", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "LongPathsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "stor-storage-disable-defrag-boot",
            Label = "Disable Boot Defragmentation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic boot-time defragmentation of frequently used files. On SSD systems boot defrag provides no benefit and adds write wear. Default: enabled (Y). Recommended: disabled (N) on SSDs.",
            Tags = ["storage", "defrag", "boot", "ssd", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
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
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
        },
        new TweakDef
        {
            Id = "stor-enable-trim",
            Label = "Enable TRIM for SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables TRIM command for SSDs by setting DisableDeleteNotification to 0. Improves SSD longevity and performance. Default: Enabled. Recommended: Enabled.",
            Tags = ["storage", "ssd", "trim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "DisableDeleteNotification", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "DisableDeleteNotification", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "DisableDeleteNotification", 0)],
        },
        new TweakDef
        {
            Id = "stor-disable-defrag-ssd",
            Label = "Disable Defrag on SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables boot-time defragmentation optimization on SSDs. Defrag is unnecessary and harmful for SSDs. Default: Y. Recommended: N.",
            Tags = ["storage", "ssd", "defrag", "optimization"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
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
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", unchecked((int)0x80000001)),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", unchecked((int)0x80000002)),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 0)],
        },
    ];
}
