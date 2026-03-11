namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DevDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dev-disable-last-access",
            Label = "Disable Last Access Time Update",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS last access timestamp updates. Reduces I/O for build-heavy workflows. Default: Enabled (volume managed).",
            Tags = ["dev-drive", "ntfs", "performance", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
        },
        new TweakDef
        {
            Id = "dev-disable-8dot3",
            Label = "Disable 8.3 Short Name Creation",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables legacy 8.3 short filename creation. Speeds up directory operations for large repos. Default: Enabled.",
            Tags = ["dev-drive", "ntfs", "8dot3", "performance"],
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
            Id = "dev-win32-long-paths",
            Label = "Enable Win32 Long Paths (>260 chars)",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables paths longer than 260 characters in Win32 applications. Essential for deep node_modules and cargo trees. Recommended.",
            Tags = ["dev-drive", "long-paths", "node", "development"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
        },
        new TweakDef
        {
            Id = "dev-exclude-build-tools",
            Label = "Exclude Build Tools from Defender",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Excludes common build processes (devenv, msbuild, node, python, cargo, rustc) from Defender real-time scanning. Can improve build times 10-30%. Recommended: Enabled for dev machines.",
            Tags = ["dev-drive", "defender", "build", "performance", "exclusion"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Defender\Exclusions\Processes"],
        },
        new TweakDef
        {
            Id = "dev-scan-cpu-limit",
            Label = "Limit Defender Scan CPU to 15%",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces Defender background scan CPU usage to 15% (default: 50%). Prevents compilation stalls during scheduled scans.",
            Tags = ["dev-drive", "defender", "cpu", "scan"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 15),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 50),
            ],
        },
        new TweakDef
        {
            Id = "dev-disable-filter-attach",
            Label = "Disable Anti-Malware Minifilter",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables real-time anti-malware minifilter driver (Dev Drive performance mode). Fastest I/O but reduces security. Only use on trusted dev volumes.",
            Tags = ["dev-drive", "minifilter", "performance", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "dev-large-fs-cache",
            Label = "Enable Large File System Cache",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables large system cache for file system operations. Improves build I/O at the cost of more RAM usage.",
            Tags = ["dev-drive", "cache", "memory", "performance"],
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
            Id = "dev-disable-efs-warning",
            Label = "Suppress EFS Encryption Warning",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the EFS encryption service prompt when Dev Drive volumes are created without encryption.",
            Tags = ["dev-drive", "efs", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService", 0)],
        },
        new TweakDef
        {
            Id = "dev-paged-pool-opt",
            Label = "Optimise Paged Pool for Builds",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Lets Windows auto-size paged pool (value 0 = system managed). Optimal for machines with 16+ GB RAM running large builds.",
            Tags = ["dev-drive", "paged-pool", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-devhome-telemetry",
            Label = "Disable Dev Home Telemetry",
            Category = "Dev Drive",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables diagnostic data collection by the Windows Dev Home app. Default: Enabled.",
            Tags = ["dev-drive", "dev-home", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-fs-compress",
            Label = "Disable NTFS Extended Char in 8.3",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extended character support in 8.3 filenames. Reduces file system overhead for dev volumes.",
            Tags = ["dev-drive", "ntfs", "8dot3", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-vbs",
            Label = "Disable VBS for Dev Performance",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Virtualization Based Security. Can improve compilation and linking speed by 5-15% but reduces security. Only for dedicated dev machines.",
            Tags = ["dev-drive", "vbs", "performance", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-memory-compression",
            Label = "Disable Paging Executive (Keep Code in RAM)",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from paging out kernel and driver code to disk. Improves compilation speed on systems with ≥16 GB RAM. Default: Disabled (0). Recommended: Enabled for dev.",
            Tags = ["dev-drive", "memory", "paging", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1)],
        },
        new TweakDef
        {
            Id = "dev-enable-host-cache",
            Label = "Enable SSD Write Buffer (StorAHCI)",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures StorAHCI burst size for improved sequential write throughput on AHCI SSDs. Can improve incremental build I/O performance.",
            Tags = ["dev-drive", "ssd", "ahci", "cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device"],
        },
        new TweakDef
        {
            Id = "dev-disable-search-svc",
            Label = "Disable Windows Search Indexer",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops the Windows Search service from indexing files. Reduces I/O contention during builds in large repositories. Default: Automatic. Recommended: Disabled on build servers.",
            Tags = ["dev-drive", "search", "indexing", "performance", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "dev-ntfs-write-cache",
            Label = "Expand NTFS I/O Page Lock (32 MB)",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets IoPageLockLimit to 32 MB for improved file I/O throughput during parallel builds. Default: System managed. Recommended: 32 MB on systems with ≥8 GB RAM.",
            Tags = ["dev-drive", "ntfs", "io", "cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-power-throttle",
            Label = "Disable Power Throttling (All Processes)",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows 10/11 power throttling for all processes globally. Ensures build tools always run at full CPU frequency. Default: Enabled. Recommended: Disabled for desktop dev machines.",
            Tags = ["dev-drive", "power", "throttle", "performance", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
    ];
}
