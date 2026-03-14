namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
            Id = "fs-increase-irp-stack",
            Label = "Increase IRP Stack Size",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Increases the I/O Request Packet (IRP) stack size for the LanmanServer service. Improves reliability and throughput for network file sharing with many concurrent connections. Default: 15. Recommended: 50 for file servers or heavy SMB workloads.",
            Tags = ["filesystem", "irp", "network", "smb", "file-sharing", "lanman"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 50)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 15)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 50)],
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
            Id = "fs-disable-content-indexing",
            Label = "Disable Content Indexing Service",
            Category = "File System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Search (WSearch) content indexing service via registry. Eliminates background I/O caused by index building on large volumes. Default: 2 (automatic start). Recommended: disabled (4) on servers or SSD-only machines.",
            Tags = ["filesystem", "indexing", "wsearch", "search", "io", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
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
    ];
}
