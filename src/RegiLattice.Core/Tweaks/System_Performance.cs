namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class Performance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "perf-disable-search-protocol-host",
            Label = "Disable SearchProtocolHost Priority Boost",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "perf-unpark-cpu-cores",
            Label = "Unpark All CPU Cores",
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "perf-disable-memory-compression",
            Label = "Disable Memory Page Combining",
            Category = "System 2",
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
            Id = "perf-always-unload-dll",
            Label = "Always Unload DLLs on Process Exit",
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "perf-disable-thumbnails-network",
            Label = "Disable Thumbnails on Network Folders",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "perf-disable-power-throttling",
            Label = "Disable Power Throttling",
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "perf-increase-smb-max-cmds",
            Label = "Increase SMB Client Maximum Command Queue",
            Category = "System 2",
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
            Id = "perf-disable-listview-shadow",
            Label = "Disable ListView Item Drop Shadows",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "perf-disable-listview-alpha-select",
            Label = "Disable ListView Alpha-Select Highlight",
            Category = "System 2",
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
