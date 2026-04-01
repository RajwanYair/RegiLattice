namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Performance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
            Id = "perf-optimize-processor-scheduling",
            Label = "Optimize for Programs (Not Services)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Boosts foreground app CPU priority; noticeably snappier UI response and lower input latency.",
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
        new TweakDef
        {
            Id = "perf-disable-aero-peek",
            Label = "Disable Aero Peek",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Aero Peek preview that shows the desktop when hovering over the Show Desktop button. Eliminates the associated DWM composition overhead.",
            Tags = ["performance", "aero", "animations", "dwm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "perf-disable-aero-shake",
            Label = "Disable Aero Shake (Minimize Other Windows)",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Aero Shake gesture that minimises all background windows when you shake the active window. Prevents accidental mass-minimise.",
            Tags = ["performance", "aero", "shake", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "perf-increase-smb-max-cmds",
            Label = "Increase SMB Client Maximum Command Queue",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases MaxCmds to 32 in LanmanWorkstation parameters. Allows more simultaneous outstanding SMB requests, improving file-share throughput under multi-threaded workloads.",
            Tags = ["performance", "smb", "network", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "MaxCmds", 32)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "MaxCmds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "MaxCmds", 32)],
        },
        new TweakDef
        {
            Id = "perf-wer-never-consent",
            Label = "Never Send Windows Error Reports (Consent)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultConsent=4 (\"Never\") in the WER consent key. Even if WER is enabled, no reports are transmitted to Microsoft.",
            Tags = ["performance", "wer", "consent", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 4)],
        },
        new TweakDef
        {
            Id = "perf-disable-listview-shadow",
            Label = "Disable ListView Item Drop Shadows",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the drop-shadow rendering from ListView items in Explorer. Small rendering overhead eliminated on large file lists.",
            Tags = ["performance", "explorer", "ui", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow", 0)],
        },
        new TweakDef
        {
            Id = "perf-crash-log-event-off",
            Label = "Disable BSOD Event-Log Entry",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LogEvent=0 to prevent the CrashControl service from writing a System event log entry on each BSOD. Reduces disk writes during crash recovery.",
            Tags = ["performance", "bsod", "event-log", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 0)],
        },
        new TweakDef
        {
            Id = "perf-set-app-kill-timeout",
            Label = "Reduce Application Shutdown Wait to 5 s",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets WaitToKillAppTimeout to 5 000 ms. Unresponsive applications are terminated 5 seconds after shutdown is initiated instead of the default 20 s.",
            Tags = ["performance", "shutdown", "app", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", 5000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", 5000)],
        },
        new TweakDef
        {
            Id = "perf-disable-taskbar-animations",
            Label = "Disable Taskbar Animations",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables taskbar button animations (TaskbarAnimations=0). Reduces compositor workload from button hover, press, and window show/hide transitions.",
            Tags = ["performance", "taskbar", "animations", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "perf-disable-listview-alpha-select",
            Label = "Disable ListView Alpha-Select Highlight",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the translucent alpha-blend selection rectangle in Explorer list views (ListviewAlphaSelect=0). Eliminates the per-frame alpha compositing cost during rubber-band selection.",
            Tags = ["performance", "explorer", "ui", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 0),
            ],
        },
    ];
}

// ── Merged from SystemOptimization.cs ──────────────────────────────────────────────────

internal static class SystemOptimization
{
    private const string MemMgmt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
    private const string FileSystem = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem";
    private const string SessionMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";
    private const string PriorityCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl";
    private const string CrashCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";
    private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string Power = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";
    private const string Kernel = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";
    private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string WinErr = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Memory Management ────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Allocates more RAM to the file system cache for improved disk I/O on systems with plenty of memory.",
            Tags = ["optimization", "memory", "cache", "performance"],
            RegistryKeys = [MemMgmt],
            ApplyOps = [RegOp.SetDword(MemMgmt, "LargeSystemCache", 1)],
            RemoveOps = [RegOp.SetDword(MemMgmt, "LargeSystemCache", 0)],
            DetectOps = [RegOp.CheckDword(MemMgmt, "LargeSystemCache", 1)],
        },
        new TweakDef
        {
            Id = "sysopt-disable-paging-executive",
            Label = "Disable Paging Executive",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keeps kernel and driver code in physical memory instead of swapping to disk.",
            Tags = ["optimization", "memory", "paging", "kernel"],
            RegistryKeys = [MemMgmt],
            ApplyOps = [RegOp.SetDword(MemMgmt, "DisablePagingExecutive", 1)],
            RemoveOps = [RegOp.SetDword(MemMgmt, "DisablePagingExecutive", 0)],
            DetectOps = [RegOp.CheckDword(MemMgmt, "DisablePagingExecutive", 1)],
        },
        new TweakDef
        {
            Id = "sysopt-io-page-lock-limit",
            Label = "Set I/O Page Lock Limit (128 MB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the maximum memory that can be locked for I/O operations to 128 MB for better disk performance.",
            Tags = ["optimization", "io", "memory", "lock"],
            RegistryKeys = [MemMgmt],
            ApplyOps = [RegOp.SetDword(MemMgmt, "IoPageLockLimit", 134217728)],
            RemoveOps = [RegOp.DeleteValue(MemMgmt, "IoPageLockLimit")],
            DetectOps = [RegOp.CheckDword(MemMgmt, "IoPageLockLimit", 134217728)],
        },
        new TweakDef
        {
            Id = "sysopt-clear-pagefile-at-shutdown",
            Label = "Clear Page File at Shutdown",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Zeroes the page file on shutdown for security — prevents data leakage but extends shutdown time.",
            Tags = ["optimization", "security", "pagefile", "shutdown"],
            RegistryKeys = [MemMgmt],
            ApplyOps = [RegOp.SetDword(MemMgmt, "ClearPageFileAtShutdown", 1)],
            RemoveOps = [RegOp.SetDword(MemMgmt, "ClearPageFileAtShutdown", 0)],
            DetectOps = [RegOp.CheckDword(MemMgmt, "ClearPageFileAtShutdown", 1)],
        },
        new TweakDef
        {
            Id = "sysopt-second-level-data-cache",
            Label = "Set L2 Cache Size (1024 KB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Override the detected L2 cache size to 1024 KB for memory manager optimisation.",
            Tags = ["optimization", "cache", "l2", "cpu"],
            RegistryKeys = [MemMgmt],
            ApplyOps = [RegOp.SetDword(MemMgmt, "SecondLevelDataCache", 1024)],
            RemoveOps = [RegOp.DeleteValue(MemMgmt, "SecondLevelDataCache")],
            DetectOps = [RegOp.CheckDword(MemMgmt, "SecondLevelDataCache", 1024)],
        },
        // ── File System ──────────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-ntfs-disable-8dot3",
            Label = "Disable NTFS 8.3 Short Name Creation",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops creation of legacy 8.3 short filenames, improving NTFS performance on directories with many files.",
            Tags = ["optimization", "ntfs", "8dot3", "filesystem"],
            RegistryKeys = [FileSystem],
            ApplyOps = [RegOp.SetDword(FileSystem, "NtfsDisable8dot3NameCreation", 1)],
            RemoveOps = [RegOp.SetDword(FileSystem, "NtfsDisable8dot3NameCreation", 0)],
            DetectOps = [RegOp.CheckDword(FileSystem, "NtfsDisable8dot3NameCreation", 1)],
        },
        new TweakDef
        {
            Id = "sysopt-ntfs-disable-last-access",
            Label = "Disable NTFS Last Access Timestamp",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables updating the last access timestamp on files, reducing disk writes significantly.",
            Tags = ["optimization", "ntfs", "timestamp", "disk"],
            RegistryKeys = [FileSystem],
            ApplyOps = [RegOp.SetDword(FileSystem, "NtfsDisableLastAccessUpdate", unchecked((int)0x80000001u))],
            RemoveOps = [RegOp.SetDword(FileSystem, "NtfsDisableLastAccessUpdate", unchecked((int)0x80000000u))],
            DetectOps = [RegOp.CheckDword(FileSystem, "NtfsDisableLastAccessUpdate", unchecked((int)0x80000001u))],
        },
        new TweakDef
        {
            Id = "sysopt-long-paths-enabled",
            Label = "Enable Long Path Support (> 260 chars)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables file paths longer than the WIN32 MAX_PATH limit of 260 characters.",
            Tags = ["optimization", "path", "long", "filesystem"],
            RegistryKeys = [FileSystem],
            ApplyOps = [RegOp.SetDword(FileSystem, "LongPathsEnabled", 1)],
            RemoveOps = [RegOp.SetDword(FileSystem, "LongPathsEnabled", 0)],
            DetectOps = [RegOp.CheckDword(FileSystem, "LongPathsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "sysopt-ntfs-memory-usage-high",
            Label = "NTFS Memory Usage: Maximum for Performance",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets NTFS memory usage to maximum (2) for better performance on systems with ample RAM.",
            Tags = ["optimization", "ntfs", "memory", "performance"],
            RegistryKeys = [FileSystem],
            ApplyOps = [RegOp.SetDword(FileSystem, "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.SetDword(FileSystem, "NtfsMemoryUsage", 1)],
            DetectOps = [RegOp.CheckDword(FileSystem, "NtfsMemoryUsage", 2)],
        },
        // ── Process Priority ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-win32-priority-separation-fg",
            Label = "Optimise Foreground App Priority",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prioritises foreground applications with short, variable time slices for responsive desktop use.",
            Tags = ["optimization", "priority", "foreground", "responsiveness"],
            RegistryKeys = [PriorityCtrl],
            ApplyOps = [RegOp.SetDword(PriorityCtrl, "Win32PrioritySeparation", 38)],
            RemoveOps = [RegOp.SetDword(PriorityCtrl, "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(PriorityCtrl, "Win32PrioritySeparation", 38)],
        },
        new TweakDef
        {
            Id = "sysopt-win32-priority-separation-bg",
            Label = "Equal Priority for Foreground and Background",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Gives equal CPU priority to foreground and background processes — ideal for servers and background workloads.",
            Tags = ["optimization", "priority", "background", "server"],
            RegistryKeys = [PriorityCtrl],
            ApplyOps = [RegOp.SetDword(PriorityCtrl, "Win32PrioritySeparation", 24)],
            RemoveOps = [RegOp.SetDword(PriorityCtrl, "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(PriorityCtrl, "Win32PrioritySeparation", 24)],
        },
        // ── Multimedia / Gaming Scheduling ───────────────────────────────

        new TweakDef
        {
            Id = "sysopt-mmcss-system-responsible",
            Label = "MMCSS: Reserve 10% CPU for System",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits multimedia scheduling to 90% of CPU, ensuring 10% remains for system tasks.",
            Tags = ["optimization", "mmcss", "cpu", "multimedia"],
            RegistryKeys = [Kernel],
            ApplyOps = [RegOp.SetDword(Kernel, "SystemResponsiveness", 10)],
            RemoveOps = [RegOp.SetDword(Kernel, "SystemResponsiveness", 20)],
            DetectOps = [RegOp.CheckDword(Kernel, "SystemResponsiveness", 10)],
        },
        new TweakDef
        {
            Id = "sysopt-mmcss-gaming-mode",
            Label = "MMCSS: Gaming Mode (0% Reserve)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Allows multimedia/games to use up to 100% of CPU — minimises system reserve.",
            Tags = ["optimization", "mmcss", "gaming", "cpu"],
            RegistryKeys = [Kernel],
            ApplyOps = [RegOp.SetDword(Kernel, "SystemResponsiveness", 0)],
            RemoveOps = [RegOp.SetDword(Kernel, "SystemResponsiveness", 20)],
            DetectOps = [RegOp.CheckDword(Kernel, "SystemResponsiveness", 0)],
        },
        new TweakDef
        {
            Id = "sysopt-network-throttling-index",
            Label = "Disable Network Throttling for Gaming",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables multimedia-class network throttling to maximise network throughput during games.",
            Tags = ["optimization", "network", "throttling", "gaming"],
            RegistryKeys = [Kernel],
            ApplyOps = [RegOp.SetDword(Kernel, "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.SetDword(Kernel, "NetworkThrottlingIndex", 10)],
            DetectOps = [RegOp.CheckDword(Kernel, "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
        },
        // ── Crash & Error Handling ───────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-crash-dump-small",
            Label = "Set Small Memory Dump (256 KB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Uses small memory dumps on BSOD — saves disk space without losing essential crash info.",
            Tags = ["optimization", "crash", "dump", "bsod"],
            RegistryKeys = [CrashCtrl],
            ApplyOps = [RegOp.SetDword(CrashCtrl, "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword(CrashCtrl, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashCtrl, "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "sysopt-disable-auto-reboot-bsod",
            Label = "Disable Auto Reboot on BSOD",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents automatic restart after a blue screen, allowing time to read the error.",
            Tags = ["optimization", "crash", "reboot", "bsod"],
            RegistryKeys = [CrashCtrl],
            ApplyOps = [RegOp.SetDword(CrashCtrl, "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword(CrashCtrl, "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword(CrashCtrl, "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "sysopt-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sending error reports to Microsoft, reducing disk and network usage.",
            Tags = ["optimization", "error", "reporting", "privacy"],
            RegistryKeys = [WinErr],
            ApplyOps = [RegOp.SetDword(WinErr, "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(WinErr, "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(WinErr, "Disabled", 1)],
        },
        // ── Boot & Logon ─────────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-auto-logon-last-user",
            Label = "Auto-Logon Last User (Skip Lock Screen)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Automatically logs in the last user at boot, skipping the lock screen (not for shared PCs).",
            Tags = ["optimization", "logon", "auto", "boot"],
            RegistryKeys = [WinLogon],
            ApplyOps = [RegOp.SetDword(WinLogon, "AutoRestartShell", 1)],
            RemoveOps = [RegOp.SetDword(WinLogon, "AutoRestartShell", 0)],
            DetectOps = [RegOp.CheckDword(WinLogon, "AutoRestartShell", 1)],
        },

        // ── Security & LSA ───────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-lsa-protection",
            Label = "Enable LSA Protection (RunAsPPL)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Runs the Local Security Authority as a protected process to prevent credential dumping attacks.",
            Tags = ["optimization", "security", "lsa", "credential"],
            RegistryKeys = [Lsa],
            ApplyOps = [RegOp.SetDword(Lsa, "RunAsPPL", 1)],
            RemoveOps = [RegOp.SetDword(Lsa, "RunAsPPL", 0)],
            DetectOps = [RegOp.CheckDword(Lsa, "RunAsPPL", 1)],
        },
        // ── Power & Energy ───────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-disable-modern-standby",
            Label = "Disable Modern Standby (S0 Low Power)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reverts to legacy S3 sleep instead of Connected Standby — prevents background wake-ups.",
            Tags = ["optimization", "power", "standby", "sleep"],
            RegistryKeys = [Power],
            ApplyOps = [RegOp.SetDword(Power, "PlatformAoAcOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(Power, "PlatformAoAcOverride")],
            DetectOps = [RegOp.CheckDword(Power, "PlatformAoAcOverride", 0)],
        },
        new TweakDef
        {
            Id = "sysopt-disable-hibernate",
            Label = "Disable Hibernate (Save Disk Space)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables hibernate and removes the hiberfil.sys file, freeing up several GB of disk space.",
            Tags = ["optimization", "hibernate", "disk", "space"],
            RegistryKeys = [Power],
            ApplyOps = [RegOp.SetDword(Power, "HibernateEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Power, "HibernateEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Power, "HibernateEnabled", 0)],
        },
        // ── Visual Effects Minimal ───────────────────────────────────────


        // ── Misc Performance ─────────────────────────────────────────────


        new TweakDef
        {
            Id = "sysopt-smb-size-req-buffer",
            Label = "Increase SMB Request Buffer Size (16 KB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the SMB request buffer size for better file sharing performance.",
            Tags = ["optimization", "smb", "buffer", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SizReqBuf", 16384)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SizReqBuf")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SizReqBuf", 16384)],
        },
        new TweakDef
        {
            Id = "sysopt-disable-prefetch",
            Label = "Disable Prefetch",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Prefetch service — recommended for NVMe/SSD systems where it wastes I/O.",
            Tags = ["optimization", "prefetch", "ssd", "disk"],
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
            Id = "sysopt-disable-superfetch",
            Label = "Disable SuperFetch / SysMain",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (SuperFetch) service preloading — reduces RAM and disk usage on SSD systems.",
            Tags = ["optimization", "superfetch", "sysmain", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
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
                    "EnableSuperfetch",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "sysopt-disable-game-bar-presence",
            Label = "Disable Game Bar Presence Writer",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Game Bar presence writer for reduced background CPU usage.",
            Tags = ["optimization", "game-bar", "presence", "cpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar", "UseNexusForGameBarEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sysopt-disable-full-screen-optimizations",
            Label = "Disable Full-Screen Optimisations",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic full-screen optimisations that can cause input lag in older games.",
            Tags = ["optimization", "fullscreen", "gaming", "input-lag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2)],
        },
        // ── Network Buffer Tuning ────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-tcp-no-delay",
            Label = "Disable Nagle's Algorithm (TCP No Delay)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Nagle's algorithm for lower TCP latency — beneficial for gaming and real-time apps.",
            Tags = ["optimization", "tcp", "nagle", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
        },
        // ── UI Responsiveness ────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-low-disk-space-warning-off",
            Label = "Disable Low Disk Space Warning",
            Category = "Performance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the low disk space notification balloon for a clutter-free experience.",
            Tags = ["optimization", "disk", "notification", "warning"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
            ],
        },
    ];
}

// ── Merged from SystemTweaks.cs ──────────────────────────────────────────────────

internal static class SystemTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sys-high-timer-resolution",
            Label = "Enable High Timer Resolution (Perf)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables global high timer resolution (0.5ms) for improved scheduling accuracy. Benefits real-time audio, gaming, and latency-sensitive applications. Default: Disabled. Recommended: Enabled for gaming/audio.",
            Tags = ["system", "performance", "timer", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
        },

        new TweakDef
        {
            Id = "sys-disable-activity-history",
            Label = "Disable Activity History",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Activity History, preventing Windows from collecting and uploading activity data (timeline, app usage).",
            Tags = ["system", "privacy", "activity-history", "timeline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Clipboard History (Win+V). Prevents clipboard content from being stored and synced.",
            Tags = ["system", "privacy", "clipboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-admin-shares",
            Label = "Disable Administrative Shares (C$, ADMIN$)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables default administrative shares (C$, ADMIN$). Reduces lateral-movement attack surface on workstations.",
            Tags = ["system", "security", "network", "shares"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "sys-system-disable-auto-maintenance",
            Label = "Disable Automatic Maintenance",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows automatic maintenance tasks (defrag, diagnostics, updates). Prevents unexpected disk and CPU usage. Default: Enabled. Recommended: Disabled for manual control.",
            Tags = ["system", "maintenance", "performance", "scheduled"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "sys-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Error Reporting via Group Policy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["system", "error-reporting", "privacy", "wer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "sys-detailed-bsod",
            Label = "Enable Detailed Blue Screen Info",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Shows technical parameters on BSoD screens instead of just the QR code and sad-face. Useful for diagnosing crash causes. Default: disabled. Recommended: enabled.",
            Tags = ["system", "bsod", "crash", "diagnostic", "blue-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-wpbt",
            Label = "Disable WPBT (Vendor Bloatware Injection)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Platform Binary Table which allows vendors to inject software via UEFI firmware (e.g., Lenovo, HP bloatware). Default: enabled. Recommended: disabled.",
            Tags = ["system", "wpbt", "uefi", "bloatware", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
        },
        new TweakDef
        {
            Id = "sys-restore-frequency",
            Label = "Enable Unlimited System Restore Frequency",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets RPSessionInterval to 0, allowing unlimited system restore point creation frequency. Default: 1. Recommended: 0 for frequent snapshots.",
            Tags = ["system", "restore", "backup", "frequency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 0)],
        },

        new TweakDef
        {
            Id = "sys-enable-utc-hardware-clock",
            Label = "Set Hardware Clock to UTC",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Windows hardware clock (RTC) to UTC instead of local time. Fixes time drift in dual-boot with Linux. Default: local.",
            Tags = ["system", "clock", "utc", "dual-boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
        },

        new TweakDef
        {
            Id = "sys-disable-error-reporting-queue",
            Label = "Disable Windows Error Reporting Queue",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the WER queue that stores crash reports before upload. Saves disk space and reduces background IO. Default: enabled.",
            Tags = ["system", "error-reporting", "wer", "queue"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
        },

        new TweakDef
        {
            Id = "sys-pagefile-encrypt-off",
            Label = "Disable Pagefile Encryption",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NtfsEncryptPagingFile=0 to disable NTFS encryption of the pagefile. Reduces CPU overhead during memory paging operations. Default: 0 on most systems.",
            Tags = ["system", "pagefile", "ntfs", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
        },
        new TweakDef
        {
            Id = "sys-memory-limit-none",
            Label = "Remove System Memory Limit",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the PhysicalMemoryAllocationPolicy value from Memory Management to let Windows use all available RAM without a capped upper bound. Default: not set.",
            Tags = ["system", "memory", "ram", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalMemoryAllocationPolicy"
                ),
            ],
            // NOTE: No RemoveOps — this tweak deletes a cap value that was set by the user or
            // OEM. We cannot safely restore to an unknown prior value; re-enabling any limit
            // would require knowing what it was. Removal is intentionally one-directional.
            RemoveOps = [],
            DetectOps =
            [
                RegOp.CheckMissing(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalMemoryAllocationPolicy"
                ),
            ],
        },
        new TweakDef
        {
            Id = "sys-max-worker-threads",
            Label = "Set Max LanmanServer Worker Threads to 8192",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxWorkItems=8192 in LanmanServer parameters. Increases the number of simultaneous outstanding server-side requests for SMB workloads. Default: system varies.",
            Tags = ["system", "smb", "lanman", "threading"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "MaxWorkItems", 8192)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "MaxWorkItems")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "MaxWorkItems", 8192)],
        },
        new TweakDef
        {
            Id = "sys-io-priority-boost",
            Label = "Enable I/O Priority Boost for Foreground",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ITEPriority=3 in PriorityControl to give foreground processes an I/O priority boost (Normal+). Improves responsiveness under disk-heavy background workloads. Default: 3.",
            Tags = ["system", "io", "priority", "foreground"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority", 3)],
        },
        new TweakDef
        {
            Id = "sys-gdi-batch-limit",
            Label = "Set GDI Batch Limit to 256",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GDIBatchLimit=256 in the Session Manager key. Increases the GDI batch flush threshold, reducing context switches for apps that make many consecutive GDI calls. Default: 0 (unbatched).",
            Tags = ["system", "gdi", "graphics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit", 256)],
        },
        new TweakDef
        {
            Id = "sys-heap-decommit-threshold",
            Label = "Set Heap Decommit Free Block Threshold",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HeapDeCommitFreeBlockThreshold=0x40000 in Session Manager. Raises the size at which heap free blocks are returned to the OS, reducing per-process fragmentation overhead. Default: 0.",
            Tags = ["system", "heap", "memory", "fragmentation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold", 0x40000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold", 0x40000),
            ],
        },
        new TweakDef
        {
            Id = "sys-vm-write-watch-off",
            Label = "Disable VM Write Watch",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WriteWatch=0 in Memory Management to disable write-watch tracking. Reduces kernel overhead when this feature is not needed by the workload. Default: 0.",
            Tags = ["system", "vm", "memory", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
        },
        new TweakDef
        {
            Id = "sys-large-pages-enable",
            Label = "Enable Large Page Mappings",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LargePageMinimum=0 in Memory Management to allow large page (2MB) memory allocations when possible. Reduces TLB pressure for large working sets. Default: depends.",
            Tags = ["system", "large-pages", "memory", "tlb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum", 0),
            ],
        },
        new TweakDef
        {
            Id = "sys-idle-task-priority",
            Label = "Set Idle Task CPU Priority to Low",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IdleTaskPriority=1 in PriorityControl to ensure idle maintenance tasks run at lowest possible CPU priority. Prevents background tasks from stealing CPU time. Default: 1.",
            Tags = ["system", "idle", "priority", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority", 1)],
        },
    ];
}

// ── Registry Management ───────────────────────────────────────────────────────
// Merged from RegistryTweaks.cs (registry hive configuration tweaks)

internal static class RegistryTweaks
{
    private const string CfgMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "reg-set-hive-checkpoint-60s",
            Label = "Set Registry Hive Checkpoint Interval to 60s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the registry hive flush/checkpoint interval to 60 seconds. Reduces write pressure on disk while keeping hive state reasonably current. Default: system-defined.",
            Tags = ["registry", "hive", "checkpoint", "performance"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "CheckpointInterval", 60)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "CheckpointInterval")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "CheckpointInterval", 60)],
        },
        new TweakDef
        {
            Id = "reg-set-max-log-files",
            Label = "Set Max Registry Log Files to 20",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the maximum number of registry transaction log files retained. Default: system-defined (unlimited).",
            Tags = ["registry", "logs", "disk-space"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "MaxCountLogs", 20)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "MaxCountLogs")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "MaxCountLogs", 20)],
        },
        new TweakDef
        {
            Id = "reg-enable-hive-autorepair",
            Label = "Enable Registry Hive Auto-Repair",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables automatic repair of registry hives on corruption detection. Helps recover from partial writes. Default: may be disabled on some configurations.",
            Tags = ["registry", "hive", "repair", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "EnableAutoRepair", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "EnableAutoRepair")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "EnableAutoRepair", 1)],
        },
        new TweakDef
        {
            Id = "reg-set-hive-size-hint",
            Label = "Set Registry Hive Pre-Allocated Size to 2048 KB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hints to Windows that the registry hive should be pre-allocated at 2048 KB. Reduces hive file fragmentation over time. Default: 0 (no hint).",
            Tags = ["registry", "hive", "allocation", "fragmentation"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "MaxRegistrySizeHint", 2048)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "MaxRegistrySizeHint")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "MaxRegistrySizeHint", 2048)],
        },
        new TweakDef
        {
            Id = "reg-enable-reg-journal",
            Label = "Enable Registry Transaction Journal",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables transaction journalling for registry hive modifications. Improves crash consistency and recovery of registry state.",
            Tags = ["registry", "journal", "transaction", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "EnableJournal", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "EnableJournal")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "EnableJournal", 1)],
        },
        new TweakDef
        {
            Id = "reg-set-idle-time-limit",
            Label = "Set Registry Idle Flush Delay to 300s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the idle time in seconds before a lazy-flush of dirty registry hive pages is triggered. 300s defers writes during active sessions. Default: system-defined.",
            Tags = ["registry", "flush", "idle", "performance"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "IdleTimeInSeconds", 300)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "IdleTimeInSeconds")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "IdleTimeInSeconds", 300)],
        },
        new TweakDef
        {
            Id = "reg-enable-reg-shadow-mount",
            Label = "Enable Registry Shadow Mount for Offline Systems",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables shadow-mount mode for registry hives accessed by offline servicing tools. Useful for WinPE/deployment scenarios.",
            Tags = ["registry", "shadow", "offline", "servicing"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "EnableShadowMount", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "EnableShadowMount")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "EnableShadowMount", 1)],
        },
        new TweakDef
        {
            Id = "reg-disable-notify-overflow",
            Label = "Disable Registry Notification Overflow Dropping",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the registry from dropping change notifications when the internal notification queue overflows. Default: dropping enabled under load.",
            Tags = ["registry", "notification", "overflow", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "NotifyOverflowDropped", 0)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "NotifyOverflowDropped")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "NotifyOverflowDropped", 0)],
        },
        new TweakDef
        {
            Id = "reg-set-hive-prealloc",
            Label = "Set Registry Hive Pre-Allocation Block to 64 KB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the registry hive block Pre-Allocation adjustment to 64 KB. Reduces frequency of hive file growth operations. Default: 0.",
            Tags = ["registry", "hive", "prealloc", "performance"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "PreAllocationAdjustment", 65536)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "PreAllocationAdjustment")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "PreAllocationAdjustment", 65536)],
        },
        new TweakDef
        {
            Id = "reg-disable-log-overflow",
            Label = "Disable Registry Log Overflow Truncation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the registry manager from truncating transaction logs when they reach their size limit. Retains full history for crash recovery. Default: truncation enabled.",
            Tags = ["registry", "log", "truncation", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "DisableLogOverflow", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "DisableLogOverflow")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "DisableLogOverflow", 1)],
        },
    ];
}

// ── merged from MemoryOptimization.cs ────────────────────────────────────────
internal static class MemoryOptimization
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mem-disable-paging-executive",
            Label = "Keep Kernel in RAM (Disable Paging Executive)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the kernel and drivers from being paged to disk. Requires 8 GB+ RAM.",
            Tags = ["memory", "performance", "kernel"],
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
            Id = "mem-enable-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Optimises the file system cache for server-like workloads (large file operations, databases).",
            Tags = ["memory", "performance", "cache", "server"],
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
            Id = "mem-clear-pagefile-on-shutdown",
            Label = "Clear Page File at Shutdown",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Zeros out the page file on shutdown. Prevents sensitive data from persisting in the page file.",
            Tags = ["memory", "security", "privacy"],
            SideEffects = "Shutdown takes slightly longer.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-iot-registry-quota",
            Label = "Increase Registry Size Limit",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the registry size limit to allow larger registry hives (useful for machines with many tweaks/software).",
            Tags = ["memory", "registry", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "RegistrySizeLimit", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "RegistrySizeLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "RegistrySizeLimit", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "mem-optimize-svchosts",
            Label = "Increase Svchost Split Threshold",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the SvcHostSplitThresholdInKB to a high value so Windows groups services into fewer svchost processes. Saves RAM.",
            Tags = ["memory", "performance", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
        },
        new TweakDef
        {
            Id = "mem-disable-memory-compression",
            Label = "Disable Memory Compression",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables Windows memory compression. Frees CPU cycles on systems with ample RAM (16 GB+).",
            Tags = ["memory", "performance", "cpu"],
            SideEffects = "May increase page file usage on low-RAM systems.",
            ApplyAction = _ => ShellRunner.RunPowerShell("Disable-MMAgent -MemoryCompression -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Enable-MMAgent -MemoryCompression -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MMAgent).MemoryCompression");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "mem-set-second-level-data-cache",
            Label = "Set L2 Cache Size Hint (1024 KB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the SecondLevelDataCache hint to 1024 KB to help Windows optimize memory management for modern CPUs.",
            Tags = ["memory", "performance", "cpu", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SecondLevelDataCache",
                    1024
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SecondLevelDataCache"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SecondLevelDataCache",
                    1024
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-disable-prefetch-boost",
            Label = "Reduce Prefetch Memory Usage",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces Prefetch/Superfetch memory consumption by lowering EnablePrefetcher to boot-only mode.",
            Tags = ["memory", "performance", "prefetch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    1
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
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-io-page-lock-limit",
            Label = "Set I/O Page Lock Limit (64 MB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the I/O page lock limit used for disk transfers. Improves large file copy performance.",
            Tags = ["memory", "performance", "io", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit", 67108864),
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
                    67108864
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-disable-page-combining",
            Label = "Disable Memory Page Combining",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables memory page combining (deduplication). Reduces CPU overhead on systems with enough RAM.",
            Tags = ["memory", "performance", "cpu"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Disable-MMAgent -PageCombining -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Enable-MMAgent -PageCombining -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MMAgent).PageCombining");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "mem-set-nonpaged-pool-limit",
            Label = "Set Non-Paged Pool Limit (256 MB)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the non-paged pool limit. Prevents drivers from exhausting non-paged memory.",
            Tags = ["memory", "stability", "kernel", "drivers"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "NonPagedPoolSize",
                    268435456
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "NonPagedPoolSize"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "NonPagedPoolSize",
                    268435456
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-disable-trim-on-memory-pressure",
            Label = "Disable Working Set Trim on Memory Pressure",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents aggressive working set trimming when memory pressure increases. Keeps apps responsive.",
            Tags = ["memory", "performance", "working-set"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LowMemoryThreshold", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LowMemoryThreshold"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LowMemoryThreshold", 0),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-system-pages",
            Label = "Set System PTE Pages to Maximum",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets SystemPages to 0 (auto-maximum), allowing Windows to use the maximum number of PTE pages for system resources.",
            Tags = ["memory", "performance", "kernel", "pte"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SystemPages", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SystemPages")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SystemPages", 0),
            ],
        },
        new TweakDef
        {
            Id = "mem-enable-large-pages",
            Label = "Enable Large Pages for Performance",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the system to prefer large memory pages (2 MB) which improves performance for memory-intensive applications.",
            Tags = ["memory", "performance", "large-pages", "database"],
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
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "mem-set-pool-usage-max",
            Label = "Set Pool Usage Maximum to 60%",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits paged and nonpaged pool usage to 60% of physical RAM, preventing runaway pool consumption from leaky drivers.",
            Tags = ["memory", "pool", "performance", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum", 60),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum", 60),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-session-pool-size",
            Label = "Optimize Session Pool Size",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the session paged pool to auto-tune (0) for optimal allocation based on available RAM and workload.",
            Tags = ["memory", "pool", "session", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SessionPoolSize", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SessionPoolSize"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SessionPoolSize", 0),
            ],
        },
        new TweakDef
        {
            Id = "mem-conservative-swap",
            Label = "Conservative Swap File Usage",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces Windows to exhaust physical RAM before using the page file, reducing disk I/O on systems with ample RAM.",
            Tags = ["memory", "swap", "pagefile", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ConservativeSwapfileUsage",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ConservativeSwapfileUsage"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ConservativeSwapfileUsage",
                    1
                ),
            ],
        },


        new TweakDef
        {
            Id = "mem-set-dirty-page-threshold",
            Label = "Set System Cache Dirty Page Threshold",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of dirty pages the system cache can accumulate before flushing to disk, reducing bursty I/O.",
            Tags = ["memory", "cache", "performance", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SystemCacheDirtyPageThreshold",
                    256
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SystemCacheDirtyPageThreshold"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SystemCacheDirtyPageThreshold",
                    256
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-heap-decommit",
            Label = "Optimize Heap Decommit Free Block Threshold",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the threshold for heap manager to decommit free blocks, returning memory to the OS faster.",
            Tags = ["memory", "heap", "performance", "decommit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold", 262144),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold", 262144),
            ],
        },
        new TweakDef
        {
            Id = "mem-enable-pae",
            Label = "Enable Physical Address Extension",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures Physical Address Extension (PAE) is enabled, allowing 32-bit Windows to use Data Execution Prevention and more RAM.",
            Tags = ["memory", "pae", "security", "hardware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalAddressExtension",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalAddressExtension"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalAddressExtension",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-disable-write-watch",
            Label = "Disable Write Watch for Faster Allocation",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables memory write watch tracking which adds overhead to memory allocation. Useful for high-throughput applications.",
            Tags = ["memory", "performance", "allocation", "write-watch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
        },
        new TweakDef
        {
            Id = "mem-set-paged-pool-quota",
            Label = "Disable Per-Process Paged Pool Quota",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables per-process paged pool quota enforcement, allowing processes to use more paged pool memory when available.",
            Tags = ["memory", "pool", "quota", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolQuota", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolQuota"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolQuota", 0),
            ],
        },
    ];
}
