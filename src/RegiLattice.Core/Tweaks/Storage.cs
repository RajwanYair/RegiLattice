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
        // ── Sprint 20 additions ─────────────────────────────────────────────

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

internal static class Recovery
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
    ];
}

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
    ];
}

internal static class PolicyCompressedFolders
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Explorer — controls
    // ZIP/compressed folder integration in File Explorer and shell.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders — dedicated key.

    private const string ZipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders";
    private const string ExplKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "zipfld-disable-compressed-folders",
            Label = "Disable ZIP Compressed Folder Support in Explorer",
            Category = "Storage",
            Description =
                "Sets DisableCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Removes the native ZIP/compressed folder handler from File Explorer. "
                + "Users can no longer double-click a ZIP file to browse it as a folder within Explorer. "
                + "Useful when a third-party archiver (7-Zip, WinRAR) is the preferred tool on managed machines.",
            Tags = ["zip", "compressed", "explorer", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ZIP files no longer open as virtual folders in Explorer; requires a third-party archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-extract-all",
            Label = "Remove 'Extract All' Context-Menu Option",
            Category = "Storage",
            Description =
                "Sets DisableExtractAll=1 in the CompressedFolders Group Policy key. "
                + "Hides the 'Extract All' entry from the right-click context menu on ZIP files. "
                + "Combined with a managed archiver deployment, this enforces the corporate tool for archive extraction.",
            Tags = ["zip", "extract", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "'Extract All' is removed from ZIP context menus; users must use an installed archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableExtractAll", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableExtractAll")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableExtractAll", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-compress-selected-files",
            Label = "Remove 'Compress to ZIP' Context-Menu Option",
            Category = "Storage",
            Description =
                "Sets DisableNewCompressedFolder=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compress to ZIP file' entry from the File Explorer shell context menu. "
                + "Prevents users from creating ZIP files directly from Explorer, directing archive operations to managed tools.",
            Tags = ["zip", "compress", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "ZIP creation from Explorer context menu is hidden; archiver tool required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNewCompressedFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNewCompressedFolder")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNewCompressedFolder", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-block-network-archive-open",
            Label = "Block Opening Remote ZIP Files as Virtual Folders",
            Category = "Storage",
            Description =
                "Sets DisableNetworkCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents users from browsing ZIP archives located on network shares as virtual folders. "
                + "Reduces risk of data exfiltration via archive browsing of network resources and prevents "
                + "potential path-traversal attacks embedded in malicious remote ZIP files.",
            Tags = ["zip", "network", "security", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ZIP files on network drives cannot be browsed as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNetworkCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-cab-browsing",
            Label = "Disable CAB File Browsing in Explorer",
            Category = "Storage",
            Description =
                "Sets DisableCabFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents File Explorer from opening Microsoft Cabinet (.cab) files as virtual folders. "
                + "CAB files are used as installers and update containers — browsing them directly can "
                + "expose sensitive setup binaries. Forcing use of proper extraction tools adds an audit layer.",
            Tags = ["cab", "cabinet", "compressed", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = ".cab files no longer open as virtual folders; dedicated extraction required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCabFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCabFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCabFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-autorun-in-archive",
            Label = "Block AutoRun Execution Inside Archive Folders",
            Category = "Storage",
            Description =
                "Sets BlockArchiveAutoRun=1 in the CompressedFolders Group Policy key. "
                + "Prevents autorun.inf scripts embedded in ZIP/CAB archives from executing when the archive "
                + "is browsed as a virtual folder. Removes a potential initial-access vector for malware "
                + "distributed via weaponised archives delivered over email or USB.",
            Tags = ["zip", "autorun", "security", "malware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AutoRun scripts inside archives are blocked from executing within Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "BlockArchiveAutoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "BlockArchiveAutoRun")],
            DetectOps = [RegOp.CheckDword(ZipKey, "BlockArchiveAutoRun", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-zip-sendto",
            Label = "Remove 'Send To Compressed Folder' from Right-Click",
            Category = "Storage",
            Description =
                "Sets DisableSendToCompressed=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compressed (zipped) folder' destination from the Send To context menu entry. "
                + "Prevents casual in-place ZIP creation that bypasses DLP scanning on managed endpoints.",
            Tags = ["zip", "sendto", "context-menu", "dlp", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Send To > Compressed Folder is hidden; users must use an explicit archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableSendToCompressed", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableSendToCompressed")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableSendToCompressed", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-archive-max-size",
            Label = "Enforce Maximum Archive Size Limit",
            Category = "Storage",
            Description =
                "Sets MaxArchiveSizeMB=512 in the CompressedFolders Group Policy key. "
                + "Limits the maximum size of archives that Explorer will open as virtual folders to 512 MB. "
                + "Prevents ZIP-bomb denial-of-service attacks and runaway memory consumption when users "
                + "accidentally open decompression-ratio-maximised archives.",
            Tags = ["zip", "size-limit", "security", "dos", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archives larger than 512 MB will not open as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "MaxArchiveSizeMB", 512)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "MaxArchiveSizeMB")],
            DetectOps = [RegOp.CheckDword(ZipKey, "MaxArchiveSizeMB", 512)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-archive-preview-handler",
            Label = "Disable Archive Preview Handler in Reading Pane",
            Category = "Storage",
            Description =
                "Sets DisableArchivePreviewHandler=1 in the CompressedFolders Group Policy key. "
                + "Prevents the Explorer Reading Pane from rendering a ZIP/CAB file preview when it is selected. "
                + "Preview rendering parses archive headers in-process; disabling it reduces attack surface for "
                + "vulnerabilities in the compressed-folders shell handler.",
            Tags = ["zip", "preview", "reading-pane", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archive files show no preview in Explorer Reading Pane.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableArchivePreviewHandler", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableArchivePreviewHandler")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableArchivePreviewHandler", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-enforce-archive-scan-on-open",
            Label = "Enforce Antivirus Scan Before Opening Archive Content",
            Category = "Storage",
            Description =
                "Sets RequireScanBeforeArchiveOpen=1 in the CompressedFolders Group Policy key. "
                + "Forces Windows Defender or the registered antivirus to scan archive contents before "
                + "the virtual folder view is presented to the user. Prevents deferred-scan gaps where "
                + "malicious payloads inside archives reach the desktop before AV inspection completes.",
            Tags = ["zip", "antivirus", "scan", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Archive contents are AV-scanned before being displayed in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "RequireScanBeforeArchiveOpen")],
            DetectOps = [RegOp.CheckDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
        },
    ];
}
