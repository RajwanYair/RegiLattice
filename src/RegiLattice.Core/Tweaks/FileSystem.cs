namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: FileSystem.cs ===

[TweakModule]
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
