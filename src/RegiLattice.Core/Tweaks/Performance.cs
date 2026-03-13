namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Performance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "perf-disable-transparency",
            Label = "Disable Transparency Effects",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables window transparency/blur effects for snappier UI. Reduces GPU compositing overhead. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["performance", "visual", "transparency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-search-protocol-host",
            Label = "Disable SearchProtocolHost Priority Boost",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables SearchProtocolHost priority boost to reduce background CPU usage from Windows Search indexing.",
            Tags = ["performance", "search", "indexing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "perf-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables large system cache, allowing Windows to use more RAM for file caching and improving disk performance.",
            Tags = ["performance", "memory", "cache"],
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
            Id = "perf-disable-paging-executive",
            Label = "Disable Paging of Kernel to Disk",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keeps kernel and drivers in physical RAM instead of paging them to disk, improving system responsiveness.",
            Tags = ["performance", "memory", "kernel", "paging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "DisablePagingExecutive",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-optimize-processor-scheduling",
            Label = "Optimize for Programs (Not Services)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Win32PrioritySeparation to 38 (0x26): short variable quantum with maximum foreground boost. Prioritizes interactive desktop apps over background services and increases scheduler responsiveness. Default: 2. Recommended: 38 for desktops, 2 for servers.",
            Tags = ["performance", "cpu", "scheduling", "responsiveness", "priority", "foreground", "quantum"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
        },
        new TweakDef
        {
            Id = "perf-disable-ntfs-encryption",
            Label = "Disable NTFS Encryption (EFS) Service",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NTFS Encrypting File System to reduce filesystem overhead. Not recommended if EFS encryption is in use.",
            Tags = ["performance", "ntfs", "encryption", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableEncryption", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableEncryption", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableEncryption", 1)],
        },
        new TweakDef
        {
            Id = "perf-disable-last-access",
            Label = "Disable Last Access Timestamp",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables NTFS last access timestamp updates. Reduces disk I/O for every file read operation. Default: 0 (Enabled). Recommended: 1 (Disabled).",
            Tags = ["performance", "ntfs", "disk", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
        },
        new TweakDef
        {
            Id = "perf-disable-spectre-mitigations",
            Label = "Disable Spectre/Meltdown Mitigations",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Spectre and Meltdown CPU mitigations for maximum performance. WARNING: reduces security. Only use on trusted, isolated machines. Default: mitigations enabled. Recommended: keep enabled unless benchmarking.",
            Tags = ["performance", "spectre", "meltdown", "cpu", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride",
                    3
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverrideMask",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverrideMask"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride",
                    3
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-unpark-cpu-cores",
            Label = "Unpark All CPU Cores",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables CPU core parking so all cores remain active at all times. Reduces latency spikes in real-time and gaming workloads. Default: Windows-managed. Recommended: disabled for desktops and gaming rigs.",
            Tags = ["performance", "cpu", "core-parking", "latency", "gaming"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMin",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMin"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-modern-standby",
            Label = "Disable Modern Standby (S0)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Modern Standby (S0 Low Power Idle) and restores classic S3 sleep. Prevents wake-from-sleep issues and battery drain. Default: Modern Standby. Recommended: disabled on desktops.",
            Tags = ["performance", "standby", "sleep", "s3", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "PlatformAoAcOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "PlatformAoAcOverride")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "PlatformAoAcOverride", 0)],
        },
        new TweakDef
        {
            Id = "perf-multimedia-priority",
            Label = "Multimedia Gaming Priority (SystemProfile)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the multimedia system profile for maximum gaming priority (SystemResponsiveness=0, no network throttling). Default: balanced (20). Recommended: 0 for gaming.",
            Tags = ["performance", "multimedia", "gaming", "priority", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    "4294967295"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    20
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-prefetch",
            Label = "Disable Prefetch (SSD Systems)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Prefetcher which is unnecessary on SSD systems and wastes I/O cycles. Default: enabled (3). Recommended: disabled on SSDs.",
            Tags = ["performance", "prefetch", "ssd", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
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
            Id = "perf-large-pages",
            Label = "Enable Large Memory Pages",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables large page support for improved memory performance in memory-intensive applications. Default: disabled. Recommended: enabled.",
            Tags = ["performance", "memory", "large-pages", "ram"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum", 1),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-memory-compression",
            Label = "Disable Memory Page Combining",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables memory page combining (compression) to reduce CPU overhead on systems with ample RAM. Default: Enabled. Recommended: Disabled on 16 GB+ systems.",
            Tags = ["performance", "memory", "compression", "page-combining"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePageCombining", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePageCombining"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePageCombining", 1),
            ],
        },
        new TweakDef
        {
            Id = "perf-win32-priority-sep",
            Label = "Optimize Win32 Priority Separation for Performance",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets Win32PrioritySeparation=38 (hex 26) to give foreground programs 6x more CPU time than background processes. Maximises responsiveness. Default: 2. Recommended: 38 for gaming/workstations.",
            Tags = ["performance", "priority", "foreground", "cpu", "win32"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
        },
        new TweakDef
        {
            Id = "perf-games-io-priority",
            Label = "Set Highest IO Priority for Games Multimedia Profile",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Games multimedia system profile to highest scheduling priority (6) with no background-only restriction. Reduces stutter in games. Default: 2. Recommended: 6 for gaming.",
            Tags = ["performance", "gaming", "io", "priority", "multimedia"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Affinity",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    6
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Affinity"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    6
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-font-smoothing",
            Label = "Disable Font Smoothing (ClearType)",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables ClearType font smoothing to reduce GPU rendering overhead. Improves performance on low-end hardware. Default: Enabled. Recommended: Disabled for maximum performance.",
            Tags = ["performance", "font", "cleartype", "rendering", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            SideEffects = "Text will appear less smooth/anti-aliased.",
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
        },
        new TweakDef
        {
            Id = "perf-always-unload-dll",
            Label = "Always Unload DLLs on Process Exit",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces Windows to immediately unload unused DLLs from memory when processes exit. Frees RAM faster and reduces memory fragmentation. Default: Not set. Recommended: Enabled.",
            Tags = ["performance", "dll", "memory", "unload", "ram"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "AlwaysUnloadDLL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "AlwaysUnloadDLL")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "AlwaysUnloadDLL", 1)],
        },
        new TweakDef
        {
            Id = "perf-increase-icon-cache",
            Label = "Increase Explorer Icon Cache Size",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases Explorer's icon cache to 4096 entries. Reduces icon reloading delays when switching between folders with many files. Default: 500. Recommended: 4096 for large libraries.",
            Tags = ["performance", "explorer", "icon", "cache", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons", "4096")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons", "4096")],
        },
        new TweakDef
        {
            Id = "perf-clear-recent-docs-exit",
            Label = "Clear Recent Documents on Logoff",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Clears the recent documents list when the user logs off. Improves privacy and slightly speeds up logoff. Default: Not cleared. Recommended: Enabled for shared machines.",
            Tags = ["performance", "privacy", "recent-docs", "logoff", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-page-file-clearing",
            Label = "Disable Page File Clearing at Shutdown",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables clearing the page file at shutdown. Faster shutdowns. Default: not cleared.",
            Tags = ["performance", "pagefile", "shutdown", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-increase-system-responsiveness",
            Label = "Increase System Responsiveness Priority",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the system responsiveness to 0 (all resources for foreground). Default: 20.",
            Tags = ["performance", "responsiveness", "priority", "foreground"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    20
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-set-win32-priority-separation-26",
            Label = "Optimize Win32 Priority Separation for Programs",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Win32PrioritySeparation to 26 (short, variable, foreground boost). Optimal for desktop responsiveness. Default: 2.",
            Tags = ["performance", "priority", "foreground", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 26)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 26)],
        },
        new TweakDef
        {
            Id = "perf-disable-fast-startup",
            Label = "Disable Fast Startup (Hybrid Boot)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Fast Startup which saves kernel state to disk. Ensures clean boots and avoids driver issues. Default: enabled.",
            Tags = ["performance", "fast-startup", "hybrid-boot", "shutdown"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "perf-disable-background-apps",
            Label = "Disable Background Apps (Policy)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables background execution of UWP/Store apps via Group Policy. Reduces CPU and memory usage from idle apps. Default: allowed.",
            Tags = ["performance", "background", "uwp", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
        },
        new TweakDef
        {
            Id = "perf-disable-ntfs-last-access",
            Label = "Disable NTFS Last Access (System Managed)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables NTFS last access timestamp updates. Reduces disk writes and improves file system performance. Uses value 0x80000001 (system managed off). Default: system managed on.",
            Tags = ["performance", "ntfs", "filesystem", "disk"],
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
            Id = "perf-disable-thumbnails-network",
            Label = "Disable Thumbnails on Network Folders",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables thumbnail generation for files on network folders. Prevents slow Explorer loading when browsing network shares. Default: enabled.",
            Tags = ["performance", "thumbnails", "network", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbnailsOnNetworkFolders",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbnailsOnNetworkFolders"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbnailsOnNetworkFolders",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-window-animations",
            Label = "Disable Window Animations",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables window animations (minimize, maximize, open, close). Reduces visual overhead and improves perceived responsiveness. Default: enabled.",
            Tags = ["performance", "animations", "visual", "responsiveness"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "perf-gpu-hw-scheduling",
            Label = "Enable GPU Hardware Scheduling",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables hardware-accelerated GPU scheduling. Reduces latency by allowing the GPU to manage its own memory. Requires compatible GPU driver. Default: off.",
            Tags = ["performance", "gpu", "scheduling", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "perf-large-page-minimum",
            Label = "Set Large Page Minimum to 128MB",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets large page minimum allocation to 128MB. Improves memory performance for applications that support large pages. Default: not set.",
            Tags = ["performance", "memory", "large-pages", "allocation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "LargePageMinimum",
                    134217728
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "LargePageMinimum",
                    134217728
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-menu-show-delay",
            Label = "Set Menu Show Delay to 0ms",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the menu show delay to 0 milliseconds. Makes menus appear instantly without animation delay. Default: 400ms.",
            Tags = ["performance", "menu", "delay", "responsiveness"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
        },
        new TweakDef
        {
            Id = "perf-performance",
            Label = "Set Visual Effects to Best Performance",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows visual effects to 'Adjust for best performance'. Disables all animations, shadows, thumbnails. Default: Let Windows decide.",
            Tags = ["performance", "visual-effects", "animations", "system"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 0)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2),
            ],
        },
        new TweakDef
        {
            Id = "perf-reduce-hung-app-timeout",
            Label = "Reduce Hung App Timeout to 1000ms",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces the hung application timeout to 1000ms (1 second). Windows detects and prompts to close unresponsive apps faster. Default: 5000ms.",
            Tags = ["performance", "timeout", "hung-app", "responsiveness"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "1000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "1000")],
        },
        new TweakDef
        {
            Id = "perf-svchost-split",
            Label = "Reduce SvcHost Splitting Threshold",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SvcHostSplitThresholdInKB to match installed RAM. Reduces the number of svchost.exe processes on systems with ample memory. Default: auto.",
            Tags = ["performance", "svchost", "memory", "processes"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
        },
        new TweakDef
        {
            Id = "perf-disable-startup-delay",
            Label = "Disable Startup App Delay",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the artificial delay Windows applies before launching startup applications. Default delay is ~10 seconds.",
            Tags = ["performance", "startup", "delay", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-low-disk-warning",
            Label = "Disable Low Disk Space Warning",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the low disk space check and warning balloon notification. Prevents Explorer from scanning drives periodically.",
            Tags = ["performance", "disk", "notification", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
            ],
        },
        new TweakDef
        {
            Id = "perf-increase-irp-stack",
            Label = "Increase IRP Stack Size",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the I/O Request Packet stack size to 30, improving performance for complex I/O operations and network shares. Default: 15.",
            Tags = ["performance", "irp", "io", "network", "stack"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 30)],
        },
        new TweakDef
        {
            Id = "perf-disable-tips-notifications",
            Label = "Disable Tips and Suggestions Notifications",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows tips, suggestions, and Get Started notifications that consume background resources scanning user activity.",
            Tags = ["performance", "tips", "notifications", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-explorer-search-history",
            Label = "Disable Explorer Search History",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents File Explorer from storing search history, eliminating background indexing of recent search queries.",
            Tags = ["performance", "explorer", "search", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "perf-increase-file-system-cache",
            Label = "Increase File System Memory Cache",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Doubles the default I/O page lock limit to 2 GB, allowing more file system data to be cached in RAM. Best for systems with 16 GB+ RAM.",
            Tags = ["performance", "memory", "cache", "filesystem", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit", 2097152),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "IoPageLockLimit",
                    2097152
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-8dot3-name-creation",
            Label = "Disable 8.3 Short File Name Creation",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables legacy 8.3 (DOS) short file name generation on NTFS volumes. Reduces overhead on file creation and improves directory enumeration speed.",
            Tags = ["performance", "ntfs", "8dot3", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
        },
        new TweakDef
        {
            Id = "perf-increase-network-throttle",
            Label = "Increase Network Throttling Index",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the network throttling index to maximum (0xFFFFFFFF), allowing multimedia and gaming applications to use full network bandwidth.",
            Tags = ["performance", "network", "throttling", "multimedia", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-nagle-algorithm",
            Label = "Disable Nagle's Algorithm (Low Latency TCP)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Nagle's algorithm and enables TCP acknowledgement optimisation for lower network latency. Beneficial for gaming and real-time applications.",
            Tags = ["performance", "network", "nagle", "tcp", "latency", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
        },
        new TweakDef
        {
            Id = "perf-disable-power-throttling",
            Label = "Disable Power Throttling",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows power throttling that reduces CPU frequency for background and foreground apps. Ensures maximum CPU performance at all times.",
            Tags = ["performance", "power", "throttling", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
    ];
}
