namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DevDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [

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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 15)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 50)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 15)],
        },
        new TweakDef
        {
            Id = "dev-disable-filter-attach",
            Label = "Disable Anti-Malware Minifilter",
            Category = "Dev Drive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables real-time anti-malware minifilter driver (Dev Drive performance mode). Fastest I/O but reduces security. Only use on trusted dev volumes.",
            Tags = ["dev-drive", "minifilter", "performance", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring",
                    1
                ),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize", 0),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 1)],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0),
            ],
        },



        new TweakDef
        {
            Id = "dev-enable-developer-mode",
            Label = "Enable Developer Mode",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Windows Developer Mode. Allows sideloading apps and access to dev features. Default: disabled.",
            Tags = ["dev", "developer-mode", "sideload", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAllTrustedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAllTrustedApps", 1)],
        },
        new TweakDef
        {
            Id = "dev-enable-utf8-codepage",
            Label = "Enable UTF-8 System Codepage",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the system default codepage to UTF-8 (65001). Improves compatibility with international text. Default: system locale.",
            Tags = ["dev", "utf8", "codepage", "unicode"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "65001"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "OEMCP", "65001"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "1252"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "OEMCP", "437"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "65001")],
        },
        new TweakDef
        {
            Id = "dev-disable-realtime-protection-devdrive",
            Label = "Exclude Dev Drive from Realtime Scan",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures Defender to trust Dev Drive volumes for performance. Build times improve 10-30%. Default: scanned.",
            Tags = ["dev", "defender", "realtime", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "TrustDevDrive", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "TrustDevDrive")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "TrustDevDrive", 1)],
        },

        new TweakDef
        {
            Id = "dev-disable-last-access",
            Label = "Disable NTFS Last Access Timestamps",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS last-access timestamp updates. Reduces disk I/O overhead for file-heavy build operations. Default: enabled.",
            Tags = ["developer", "ntfs", "last-access", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
        },
        new TweakDef
        {
            Id = "dev-enable-host-cache",
            Label = "Enable Developer DNS Host Cache",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases DNS client cache size for developers running multiple services. Reduces DNS lookup latency. Default: standard cache.",
            Tags = ["developer", "dns", "cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400),
            ],
        },
        new TweakDef
        {
            Id = "dev-exclude-build-tools",
            Label = "Exclude Build Tools from Defender",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Adds common build tool processes (dotnet, msbuild, node) to Defender exclusions. Speeds up builds significantly. Default: scanned.",
            Tags = ["developer", "defender", "exclusion", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes", "dotnet.exe", "dotnet.exe"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes", "dotnet.exe")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes",
                    "dotnet.exe",
                    "dotnet.exe"
                ),
            ],
        },
        new TweakDef
        {
            Id = "dev-ntfs-write-cache",
            Label = "Enable NTFS Write Caching",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables NTFS memory-mapped I/O write caching. Improves write performance for build and compile operations. Default: varies.",
            Tags = ["developer", "ntfs", "write-cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },

        new TweakDef
        {
            Id = "dev-devdrive-filter-native",
            Label = "Set Dev Drive Filter to Native Antimalware Only",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Configures Dev Drive to accept only native antimalware filter drivers. Non-essential minifilters are excluded. Improves build performance.",
            Tags = ["devdrive", "filter", "performance", "win11"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlNative", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlNative")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlNative", 2)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-filter-core",
            Label = "Set Dev Drive Core Filter to Performance Mode",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Sets Dev Drive core filter control to performance mode. Reduces overhead from filter driver stacks. Default: standard mode.",
            Tags = ["devdrive", "filter", "core", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlCore", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlCore")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlCore", 2)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-ntfs-quota-off",
            Label = "Disable NTFS Quota Tracking on Dev Drive",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS per-user disk quota tracking. Reduces filesystem overhead for developer build volumes. Default: enabled.",
            Tags = ["devdrive", "ntfs", "quota", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableQuotaTracking", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableQuotaTracking", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableQuotaTracking", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-no-compress-policy",
            Label = "Disable NTFS Compression on Dev Drive (Policy)",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Prevents NTFS file compression on Dev Drive. Compression adds CPU overhead during heavy I/O build operations. Default: compression allowed.",
            Tags = ["devdrive", "ntfs", "compression", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowNtfsCompression", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowNtfsCompression")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowNtfsCompression", 0)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-no-smb-share",
            Label = "Disable SMB Sharing of Dev Drive (Policy)",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Prevents Dev Drive volumes from being shared via SMB. Limits access to local processes only for security. Default: sharing allowed.",
            Tags = ["devdrive", "smb", "security", "sharing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowSmbSharing", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowSmbSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowSmbSharing", 0)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-cache-size-64mb",
            Label = "Set Dev Drive Cache Size to 64 MB",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Sets the Dev Drive filter cache size to 64 MB. Larger cache improves performance for iterative build workloads. Default: 16 MB.",
            Tags = ["devdrive", "cache", "performance", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "CacheSizeMB", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "CacheSizeMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "CacheSizeMB", 64)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-disable-telemetry",
            Label = "Disable Dev Drive Telemetry",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Disables telemetry data collection for Dev Drive operations. Prevents usage metrics being sent to Microsoft. Default: enabled.",
            Tags = ["devdrive", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-security-trusted",
            Label = "Set Dev Drive Security to Trusted Mode",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 22621,
            Description =
                "Configures Dev Drive security trust level to allow looser security enforcement for local development environments. Default: standard.",
            Tags = ["devdrive", "security", "trust"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "SecurityLevel", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "SecurityLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "SecurityLevel", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-quota-user-off",
            Label = "Disable Per-User Disk Quota on Dev Drive",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Disables per-user disk quota enforcement on Dev Drive volumes. Prevents quota warnings during large builds. Default: system default.",
            Tags = ["devdrive", "quota", "user", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "DisableUserQuota", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "DisableUserQuota")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "DisableUserQuota", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-perf-mode-high",
            Label = "Enable Dev Drive High-Performance Mode",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Enables high-performance mode for Dev Drive I/O operations. Optimises scheduling for build-heavy workloads. Default: standard mode.",
            Tags = ["devdrive", "performance", "mode", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "PerformanceMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "PerformanceMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "PerformanceMode", 1)],
        },
        new TweakDef
        {
            Id = "dev-sandbox-disable-vgpu",
            Label = "Disable vGPU in Windows Sandbox",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Disables virtualised GPU (vGPU) inside Windows Sandbox. Reduces GPU overhead and sandbox startup time when GPU acceleration is not needed. Default: vGPU enabled.",
            Tags = ["dev", "sandbox", "vgpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowVGPU", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowVGPU")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowVGPU", 0)],
        },
        new TweakDef
        {
            Id = "dev-sandbox-disable-networking",
            Label = "Disable Networking in Windows Sandbox",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Disables network access inside Windows Sandbox for isolated testing environments. Useful when testing untrusted code or malware analysis. Default: networking enabled.",
            Tags = ["dev", "sandbox", "network", "isolation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking", 0)],
        },
        new TweakDef
        {
            Id = "dev-sandbox-enable-protected-client",
            Label = "Enable Protected Client Mode in Windows Sandbox",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Turns on Protected Client mode for Windows Sandbox, which runs the sandbox with reduced privileges and additional isolation. Increases security at the cost of some compatibility. Default: disabled.",
            Tags = ["dev", "sandbox", "security", "protected-client"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowProtectedClient", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowProtectedClient")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowProtectedClient", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-offline-files",
            Label = "Disable Offline Files (CSC) Service",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Client-Side Caching (CSC) service which handles offline file synchronisation. Removes overhead on dev workstations not using corporate file shares. Default: automatic start.",
            Tags = ["dev", "offline-files", "csc", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC", "Start", 4)],
        },
        new TweakDef
        {
            Id = "dev-disable-app-compat-engine",
            Label = "Disable Application Compatibility Engine",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Application Compatibility Engine which checks every process launch against a compatibility database. Speeds up process startup on dev machines with modern software only. Default: enabled.",
            Tags = ["dev", "compat", "performance", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-program-compat-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Program Compatibility Assistant that prompts users to run programs in compatibility mode. Removes nag prompts on dev workstations. Default: enabled.",
            Tags = ["dev", "compat", "assistant", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-program-compat-telemetry",
            Label = "Disable Application Telemetry (Compat)",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Application Impact Telemetry (AIT) which logs app usage and compatibility events. Reduces background disk and network activity from the compat engine. Default: enabled.",
            Tags = ["dev", "compat", "telemetry", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-wer-service-dev",
            Label = "Disable Windows Error Reporting Service (Dev)",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Windows Error Reporting (WerSvc) service to disabled. Stops crash dumps and error telemetry uploads from dev builds. Use debuggers instead for crash analysis. Default: manual start.",
            Tags = ["dev", "wer", "crash", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "dev-enable-console-ansi",
            Label = "Enable ANSI VT100 Colour in Console (Dev)",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Virtual Terminal Level processing in the Windows console so ANSI escape codes (colours, cursor movement) work in cmd.exe and PowerShell. Required for colour-aware CLI tools. Default: disabled in cmd.exe.",
            Tags = ["dev", "console", "ansi", "colour", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
        },
        new TweakDef
        {
            Id = "dev-set-high-res-timer",
            Label = "Enable High-Resolution Timer for Dev Workloads",
            Category = "Dev Drive / Developer Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Allows high-resolution timer requests globally via the kernel setting introduced in Win 11 22H2. Lets developer tools (profilers, benchmarks) achieve sub-millisecond timing precision without per-process requests. Default: off.",
            Tags = ["dev", "timer", "performance", "precision"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
        },
    ];
}
