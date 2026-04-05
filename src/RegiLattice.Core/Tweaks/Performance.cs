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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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

        // ── File System ──────────────────────────────────────────────────

        // ── Process Priority ─────────────────────────────────────────────

        // ── Multimedia / Gaming Scheduling ───────────────────────────────

        // ── Crash & Error Handling ───────────────────────────────────────

        // ── Boot & Logon ─────────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-auto-logon-last-user",
            Label = "Auto-Logon Last User (Skip Lock Screen)",
            Category = "System",
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

        // ── Power & Energy ───────────────────────────────────────────────

        // ── Visual Effects Minimal ───────────────────────────────────────

        // ── Misc Performance ─────────────────────────────────────────────

        // ── Network Buffer Tuning ────────────────────────────────────────

        // ── UI Responsiveness ────────────────────────────────────────────

    ];
}

// ── Merged from SystemTweaks.cs ──────────────────────────────────────────────────

internal static class SystemTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
            Category = "System",
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
            Category = "System",
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
            Id = "mem-set-iot-registry-quota",
            Label = "Increase Registry Size Limit",
            Category = "System",
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
            Id = "mem-disable-memory-compression",
            Label = "Disable Memory Compression",
            Category = "System",
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
            Category = "System",
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
            Id = "mem-set-io-page-lock-limit",
            Label = "Set I/O Page Lock Limit (64 MB)",
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "mem-set-session-pool-size",
            Label = "Optimize Session Pool Size",
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Category = "System",
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
            Id = "mem-set-paged-pool-quota",
            Label = "Disable Per-Process Paged Pool Quota",
            Category = "System",
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
        // ── merged from: Gpu.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "gpu-disable-nvidia-telemetry",
            Label = "Disable NVIDIA Telemetry",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables NVIDIA telemetry and usage data collection. Only applies if NVIDIA drivers are installed. Default: Enabled (opt-in). Recommended: Disabled.",
            Tags = ["gpu", "nvidia", "privacy", "telemetry"],
            IsApplicable = HardwareInfo.HasNvidiaGpu,
            ApplicabilityNote = "Requires NVIDIA GPU",
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID44231", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID64640", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID66610", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID44231"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID64640"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID66610"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference", 0)],
        },
        new TweakDef
        {
            Id = "gpu-nvidia-tdr-delay",
            Label = "Increase NVIDIA TDR Delay (8s)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the GPU TDR (Timeout Detection and Recovery) delay to 8 seconds. Prevents driver resets during heavy GPU workloads. Removal deletes the value, restoring the Windows default (2s). Default: 2s. Recommended: 8s.",
            Tags = ["gpu", "nvidia", "stability", "tdr"],
            IsApplicable = HardwareInfo.HasNvidiaGpu,
            ApplicabilityNote = "Requires NVIDIA GPU",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
        },
        new TweakDef
        {
            Id = "gpu-disable-gpu-preemption",
            Label = "Disable GPU Preemption (Low Latency)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables GPU preemption (EnablePreemption=0) for lower render latency. May improve frame times in GPU-bound scenarios but can affect multi-tasking and system responsiveness. Default: Enabled. Removal deletes the value.",
            Tags = ["gpu", "latency", "gaming", "preemption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-dx12-ultimate",
            Label = "Force DirectX 12 Ultimate",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces DirectX 12 mode for all compatible applications. Enables advanced features like mesh shaders and raytracing. Default: Auto. Recommended: Enabled for DX12-capable GPUs.",
            Tags = ["gpu", "directx", "dx12", "performance", "raytracing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
        },
        new TweakDef
        {
            Id = "gpu-wddm-scheduler",
            Label = "Optimize WDDM GPU Scheduler",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Optimizes WDDM flip queue length to 2 frames for reduced input latency. Trades slight throughput for lower frame queue depth. Default: 3. Recommended: 2 for competitive gaming.",
            Tags = ["gpu", "wddm", "flip-queue", "latency", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength", 2)],
        },
        new TweakDef
        {
            Id = "gpu-max-prerendered-frames",
            Label = "Set Max Pre-Rendered Frames to 1",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the flip queue size to 1, limiting pre-rendered frames. Reduces input lag at the cost of slightly lower throughput. Default: 3 frames. Recommended: 1 for competitive gaming.",
            Tags = ["gpu", "pre-rendered", "frames", "input-lag", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
        },
        new TweakDef
        {
            Id = "gpu-enable-dx12-async",
            Label = "Enable DirectX 12 Async Compute",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables D3D12 asynchronous command buffer reuse for improved GPU throughput. Most beneficial in DirectX 12 games with async compute shaders. Default: Not set. Recommended: Enabled for gaming.",
            Tags = ["gpu", "directx12", "async", "compute", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
        },
        new TweakDef
        {
            Id = "gpu-disable-shader-cache",
            Label = "Disable DirectX Shader Disk Cache",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DirectX on-disk shader cache. Reduces disk I/O; useful in scenarios where fresh shader compilation is preferred or disk space is constrained. Default: Enabled.",
            Tags = ["gpu", "shader", "cache", "disk", "directx"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-high-performance-power",
            Label = "Force GPU High Performance Power Plan",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces the GPU to maximum performance power mode. Disables GPU power saving. Default: adaptive.",
            Tags = ["gpu", "power", "performance", "high-performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported", 0)],
        },
        new TweakDef
        {
            Id = "gpu-disable-igpu-powersave",
            Label = "Disable iGPU Power Saving",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Intel integrated GPU power-saving features. Forces maximum iGPU performance. Default: power saving enabled.",
            Tags = ["gpu", "igpu", "intel", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-software-cursor",
            Label = "Force Software Cursor",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces software cursor rendering instead of hardware cursor. May fix cursor corruption issues on some GPUs. Default: hardware cursor.",
            Tags = ["gpu", "cursor", "software", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor", 1)],
        },
        new TweakDef
        {
            Id = "gpu-preemption-disable",
            Label = "Disable GPU Compute Preemption",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables GPU compute preemption at the DWM level. May improve GPGPU compute performance but can cause display hangs. Default: enabled.",
            Tags = ["gpu", "compute", "preemption", "dwm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy", 0)],
        },
        new TweakDef
        {
            Id = "gpu-wddm3-miracast",
            Label = "Disable Miracast (WDDM)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Miracast wireless display support at the driver level. Frees GPU resources used for Miracast. Default: enabled.",
            Tags = ["gpu", "miracast", "wddm", "wireless-display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC", 0)],
        },
        new TweakDef
        {
            Id = "gpu-tasks-gpu-priority",
            Label = "Boost GPU Priority for Games Profile",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GpuPriority=8 in the Multimedia SystemProfile Tasks\\Games key. Requests highest GPU scheduling priority for game processes via MMCSS. Default: 1 or 2.",
            Tags = ["gpu", "priority", "mmcss", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "gpu-hw-sched-policy",
            Label = "Set WDDM Scheduler Policy to Batch",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SchedulerPolicy=2 (batch mode) in GraphicsDrivers. Batches GPU work submissions to reduce context-switch overhead. Default: preemptive.",
            Tags = ["gpu", "wddm", "scheduler", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy", 2)],
        },
        new TweakDef
        {
            Id = "gpu-threading-optimization",
            Label = "Enable GPU Driver Threading Optimization",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ThreadedOptimizationFlags=1 in GraphicsDrivers to enable driver threading optimizations. Allows the GPU driver to use multiple CPU threads for command processing. Default: off.",
            Tags = ["gpu", "threading", "driver", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags", 1)],
        },
        new TweakDef
        {
            Id = "gpu-idle-power-engine-timeout",
            Label = "Set GPU Engine Timeout for Idle Power",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrEngineTimeout=13 seconds in GraphicsDrivers. Controls how long the GPU engine can be unresponsive before reset recovery. Default: 2 seconds.",
            Tags = ["gpu", "tdr", "timeout", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout", 13)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout", 13)],
        },
        new TweakDef
        {
            Id = "gpu-hdr-auto-color",
            Label = "Enable DWM Auto Color Management",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AutoColorManagement=1 in DWM registry key to enable automatic HDR/color management for connected displays that support it. Default: 0.",
            Tags = ["gpu", "hdr", "color", "dwm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement", 1)],
        },
        new TweakDef
        {
            Id = "gpu-tdr-limit-extend",
            Label = "Extend GPU TDR Limit Count to 10",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitCount=10 in GraphicsDrivers. Allows up to 10 TDR recoveries in 60 seconds before crashing. Useful for overclocked or compute workloads. Default: 5.",
            Tags = ["gpu", "tdr", "stability", "overclock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount", 10)],
        },
        new TweakDef
        {
            Id = "gpu-multi-adapter-alt",
            Label = "Set Multi-GPU Alternate Frame Rendering",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseNoPlatformUpdateMode=1 in GraphicsDrivers to hint drivers to avoid platform-specific update mode that may conflict with multi-GPU setups. Default: 0.",
            Tags = ["gpu", "multi-gpu", "adapter", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode", 1)],
        },
        new TweakDef
        {
            Id = "gpu-opengl-flip-interval",
            Label = "Set DirectDraw Flip Interval to 0",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FlipInterval=0 in DirectDraw settings to allow immediate buffer flips without vertical sync wait. Reduces display-pipeline latency for OpenGL/DDraw apps. Default: 1.",
            Tags = ["gpu", "opengl", "directdraw", "vsync"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval", 0)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-level-recover",
            Label = "Set GPU TDR Level to Recover (No Bugcheck)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLevel=3 so Windows recovers the GPU engine after a Timeout Detection & Recovery (TDR) event without triggering a bugcheck. Improves stability for overclocked or demanding GPU workloads. Default: 3 on most systems.",
            Tags = ["gpu", "tdr", "stability", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel", 3)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-debugging-off",
            Label = "Disable GPU TDR Crash Dump Generation",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables TDR debug crash dump generation by setting TdrDebugging=0. Prevents large crash dumps when the GPU recovers from a timeout, reducing disk I/O overhead during recovery. Default: 0.",
            Tags = ["gpu", "tdr", "dump", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging", 0)],
        },
        new TweakDef
        {
            Id = "gpu-enable-vrr-optimize",
            Label = "Enable Windows 11 VRR Optimisation",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Enables the Variable Refresh Rate (VRR) optimisation in DWM on Windows 11. Allows the desktop compositor to leverage VRR/FreeSync/G-Sync for smoother UI rendering. Default: off (requires supported display).",
            Tags = ["gpu", "vrr", "freesync", "gsync", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 1)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-ddi-delay",
            Label = "Set GPU TDR DDI Delay to 0 (Immediate)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrDdiDelay=0 so the DDI watchdog immediately detects when a DDI call exceeds the allowed time. Allows faster GPU error detection with less latency on recovery. Default: not set (uses kernel default).",
            Tags = ["gpu", "tdr", "ddi", "watchdog"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 0)],
        },
        new TweakDef
        {
            Id = "gpu-enable-hw-flip-queue",
            Label = "Enable GPU Hardware Flip Queue",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Enables the DirectX Graphics Kernel hardware flip queue via DxgkrnlEnableHwFlipQueue=1. Moves present queue management to hardware, reducing CPU involvement and frame delivery latency. Default: system-managed.",
            Tags = ["gpu", "flip-queue", "latency", "dx", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue", 1)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-limit-value",
            Label = "Increase GPU TDR Limit Count (Stability)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitValue=60 to allow up to 60 GPU timeouts within the TdrLimitTime window before triggering a full system bugcheck. More tolerant under heavy GPU load or overclocking. Default: 5.",
            Tags = ["gpu", "tdr", "limit", "stability", "overclocking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue", 60)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-limit-time",
            Label = "Extend GPU TDR Limit Time Window",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitTime=60 to extend the TDR limit counting window to 60 seconds. Combined with a higher TdrLimitValue this prevents bugchecks on systems that have occasional GPU hangs under load. Default: 60 (may vary).",
            Tags = ["gpu", "tdr", "limit", "time", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime", 60)],
        },
    ];
}

// ── merged from Startup.cs ──
internal static class Startup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "startup-disable-startup-delay",
            Label = "Disable Startup Delay",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the artificial startup delay for Run-key programs, allowing them to launch immediately at login.",
            Tags = ["startup", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-cortana-startup",
            Label = "Disable Cortana Startup",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Cortana from the HKCU Run key to prevent auto-start at login.",
            Tags = ["startup", "cortana", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CortanaUI"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Cortana"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CortanaUI")],
        },
        new TweakDef
        {
            Id = "startup-disable-login-background",
            Label = "Use Solid Color Login Background",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Replaces the Windows Spotlight / hero image on the login screen with a plain solid color background.",
            Tags = ["startup", "login", "appearance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-first-logon-animation",
            Label = "Disable First Login Animation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Hi / We're getting things ready' first-logon animation shown after a new user profile is created.",
            Tags = ["startup", "animation", "login", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-start-boot-numlock-on",
            Label = "Set Boot-Up Num Lock to On",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Num Lock at the Windows login screen by default. Default: Off. Recommended: On for desktop keyboards.",
            Tags = ["startup", "numlock", "keyboard", "boot"],
            RegistryKeys = [@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "startup-start-disable-app-restart",
            Label = "Disable Automatic App Restart on Login",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically restarting apps that were open before shutdown/restart. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "restart", "apps", "login", "winlogon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
        },
        new TweakDef
        {
            Id = "startup-set-boot-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the multi-boot OS selection timeout to 3 seconds instead of the default 30. Faster boot on single-OS machines.",
            Tags = ["startup", "boot", "timeout", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "startup-disable-boot-logo",
            Label = "Disable Boot Logo Display",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the Windows boot logo animation for faster POST-to-desktop times.",
            Tags = ["startup", "boot", "logo", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-narrator-at-login",
            Label = "Disable Narrator at Login Screen",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Narrator auto-start at the Windows login screen.",
            Tags = ["startup", "narrator", "accessibility", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger",
                    @"%SystemRoot%\System32\systray.exe"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger",
                    @"%SystemRoot%\System32\systray.exe"
                ),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables fast user switching at login. Simplifies the login screen and slightly reduces memory usage on shared PCs.",
            Tags = ["startup", "login", "user-switching", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-edge-prelaunch",
            Label = "Disable Edge Pre-Launch at Login",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Edge from pre-launching in the background at login. Reduces startup memory and CPU usage.",
            Tags = ["startup", "edge", "prelaunch", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Program Compatibility Assistant that checks applications for compatibility issues at launch.",
            Tags = ["startup", "compatibility", "performance", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
    ];
}

// ── merged from Boot.cs ────────────────────────────────────────
internal static class Boot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "boot-disable-secboot-check",
            Label = "Suppress Secure Boot Status Check",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses the Secure Boot status notification in Windows by setting UEFISecureBootEnabled to 0 in the registry.",
            Tags = ["boot", "security", "uefi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-anim",
            Label = "Disable Boot Animation/Spinner",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows boot animation/spinner for a faster perceived boot. The boot process skips the animated dots. Default: enabled. Recommended: disabled for faster boot.",
            Tags = ["boot", "animation", "performance", "spinner"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-enable-fast-startup",
            Label = "Enable Fast Startup (Hiberboot)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Windows Fast Startup which uses a hybrid shutdown with hibernation to speed up boot time. Default: Usually enabled. Recommended: Enabled for fast boot.",
            Tags = ["boot", "fast-startup", "hiberboot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-prefetch-optimized",
            Label = "Set Prefetch to Optimized Mode",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables both boot and application prefetching for optimal performance. Value 3 = boot + app prefetch. Default: 3. Recommended: 3 for SSDs and HDDs.",
            Tags = ["boot", "prefetch", "performance", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-clear-pagefile",
            Label = "Clear Pagefile at Shutdown",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Clears the virtual memory pagefile at every shutdown. Prevents sensitive data from being recovered from pagefile.sys. Note: significantly increases shutdown time on large systems. Default: not cleared. Recommended: Apply on secure workstations.",
            Tags = ["boot", "security", "pagefile", "shutdown", "privacy"],
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
            Id = "boot-disable-boot-ux",
            Label = "Disable Boot UI Animation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows boot animation (spinning dots). Shows a simple progress bar instead. Default: animated.",
            Tags = ["boot", "animation", "ui", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-timeout-5s",
            Label = "Set Boot Menu Timeout to 5 Seconds",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager menu timeout to 5 seconds for dual-boot systems. Default: 30 seconds.",
            Tags = ["boot", "timeout", "dual-boot", "menu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 30),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
        },
        new TweakDef
        {
            Id = "boot-verbose-status-messages",
            Label = "Enable Verbose Boot Status Messages",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot, shutdown, logon, and logoff. Default: hidden.",
            Tags = ["boot", "verbose", "status", "messages"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        // ── Command-based boot tweaks (bcdedit) ────────────────────────────
        new TweakDef
        {
            Id = "boot-bcd-quiet-boot",
            Label = "Enable Quiet Boot (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows quiet boot mode via bcdedit — suppresses the boot logo and status messages for faster boot appearance.",
            Tags = ["boot", "bcdedit", "quiet", "logo"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager timeout to 3 seconds via bcdedit. Speeds up boot when multi-boot options exist.",
            Tags = ["boot", "bcdedit", "timeout", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "3"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("3", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-disable-recovery",
            Label = "Disable Automatic Recovery (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic recovery/repair environment via bcdedit. Prevents boot loops but removes automatic repair capability.",
            Tags = ["boot", "bcdedit", "recovery", "repair", "server"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Disables automatic repair on boot failure.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "no"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "yes"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("recoveryenabled", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-driver-verifier-reset",
            Label = "Reset Driver Verifier (verifier)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Resets Driver Verifier settings to none. Useful after debugging driver issues when verifier was left enabled.",
            Tags = ["boot", "verifier", "driver", "diagnostic", "reset"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("verifier", ["/reset"]);
            },
            // NOTE: No RemoveAction — "reset" is a one-shot diagnostic action. There is no
            // meaningful inverse; re-enabling verifier requires choosing specific drivers.
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("verifier", ["/query"]);
                return stdout.Contains("No drivers", StringComparison.OrdinalIgnoreCase)
                    || stdout.Contains("not loaded", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── Restored stubs with real operations ──────────────────

        new TweakDef
        {
            Id = "boot-disable-auto-repair",
            Label = "Disable Automatic Startup Repair",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows from launching Automatic Repair after consecutive boot failures. Use with caution.",
            Tags = ["boot", "auto-repair", "recovery", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "System will not auto-recover from boot failures.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootstatuspolicy", "IgnoreAllFailures"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "bootstatuspolicy"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootstatuspolicy", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("IgnoreAllFailures", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-boot-logo",
            Label = "Disable Boot Logo (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows boot logo via bcdedit for a minimalist boot screen.",
            Tags = ["boot", "logo", "bcdedit", "ux"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "quietboot"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-driver-verifier",
            Label = "Disable Driver Verifier Flags",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears Driver Verifier flags in the registry. Useful after debugging when verifier causes boot loops.",
            Tags = ["boot", "verifier", "driver", "registry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-logo",
            Label = "Disable OEM Boot Logo",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the OEM manufacturer logo during boot via bcdedit nologo option.",
            Tags = ["boot", "logo", "oem", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{globalsettings}", "custom:16000067", "true"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{globalsettings}", "custom:16000067"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{globalsettings}"]);
                return stdout.Contains("16000067", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-winre",
            Label = "Disable WinRE Partition",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Recovery Environment. Frees recovery partition but removes repair tools.",
            Tags = ["boot", "winre", "recovery", "disk-space"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Removes access to Windows Recovery tools.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/disable"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/enable"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("reagentc", ["/info"]);
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-boot-timeout",
            Label = "Set Boot Timeout to 0 Seconds",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets BCD boot menu timeout to 0 seconds for instant boot-through. No OS selection screen shown.",
            Tags = ["boot", "timeout", "bcdedit", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "0"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("0", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-log",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables boot logging via bcdedit. Writes driver load info to %%SystemRoot%%\\ntbtlog.txt.",
            Tags = ["boot", "logging", "bcdedit", "diagnostic"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootlog", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-max-proc-count",
            Label = "Use All CPU Cores at Boot",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures msconfig-equivalent setting to use all processor cores during boot.",
            Tags = ["boot", "cpu", "cores", "performance", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "numproc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return !stdout.Contains("numproc", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-menu-timeout",
            Label = "Set Boot Menu Timeout to 10s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot menu display timeout to 10 seconds. Useful for dual-boot systems.",
            Tags = ["boot", "timeout", "menu", "dual-boot", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "10"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("10", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-verbose-boot",
            Label = "Enable Verbose Boot Messages",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot instead of the logo. Useful for debugging slow boot.",
            Tags = ["boot", "verbose", "diagnostic", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "on"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "off"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("sos", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-startup-gpo",
            Label = "Enable Fast Startup via Group Policy",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HiberbootEnabled=1 in the Windows System policy key to enforce fast startup at GPO level. Complements the standard fast startup registry setting. Default: not set.",
            Tags = ["boot", "fast-startup", "policy", "hibernate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-global-wait-timeout",
            Label = "Set Global Shutdown Wait Timeout to 5s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WaitForIdleState=5 in the system Timeout key. Controls how long Windows waits for the system to become idle before shutdown completes. Default: 2.",
            Tags = ["boot", "shutdown", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
        },
        new TweakDef
        {
            Id = "boot-menu-timeout-policy",
            Label = "Set Boot Menu Display Timeout Policy to 10s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BootTimeoutSeconds=10 in the Windows System policy key. Controls the boot menu display time at policy level. Default: not set (uses BCD value).",
            Tags = ["boot", "menu", "timeout", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
        },
        new TweakDef
        {
            Id = "boot-hyperv-launch-off",
            Label = "Disable Hyper-V Hypervisor Launch",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Runs 'bcdedit /set hypervisorlaunchtype off' to disable the Hyper-V hypervisor at boot. Improves native performance on bare-metal gaming/workstation installs. Default: auto.",
            Tags = ["boot", "hyper-v", "bcd", "performance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "auto"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-test-signing-off",
            Label = "Disable Test Signing Mode",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set testsigning off' to disable test-signing mode. Prevents unsigned test drivers from loading. Default: off.",
            Tags = ["boot", "bcd", "security", "test-signing"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "on"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("testsigning", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-report-ok",
            Label = "Enable Boot-OK Reporting to Winlogon",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ReportBootOk=1 in Winlogon to signal that the current boot is clean and should be saved as the last known good configuration. Default: 1.",
            Tags = ["boot", "winlogon", "last-known-good", "recovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
        },
        new TweakDef
        {
            Id = "boot-kernel-debug-filter",
            Label = "Suppress Kernel Debug Print Filter",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DEFAULT=0x0 in the Debug Print Filter to suppress kernel debug messages, reducing DbgPrint overhead on retail builds. Default: 0x8 or not set.",
            Tags = ["boot", "kernel", "debug", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
        },
        new TweakDef
        {
            Id = "boot-winre-policy-allow",
            Label = "Allow Windows Recovery Environment Policy",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWinRE=0 in WinRE policy to ensure the Windows Recovery Environment remains accessible. Prevents accidental policy lockout of recovery tools. Default: 0.",
            Tags = ["boot", "recovery", "winre", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
        },
        new TweakDef
        {
            Id = "boot-legacy-f8-menu",
            Label = "Enable Legacy F8 Boot Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set {bootmgr} displaybootmenu yes' to enable the legacy F8 boot menu. Allows access to safe mode and other startup options. Default: off on modern Windows.",
            Tags = ["boot", "bcd", "safe-mode", "f8"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "yes"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("displaybootmenu", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-nx-optin",
            Label = "Set Data Execution Prevention to OptIn",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set nx OptIn' to enable DEP (Data Execution Prevention) only for OS-protected processes. Balances security and compatibility. Default: OptIn.",
            Tags = ["boot", "bcd", "dep", "security"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("nx", StringComparison.OrdinalIgnoreCase) && stdout.Contains("OptIn", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-startup-app-delay",
            Label = "Disable Startup App Launch Delay",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets StartupDelayInMSec=0 to eliminate the artificial delay Windows introduces before launching registered startup applications. Speeds up the post-login experience. Default: 10-second delay.",
            Tags = ["boot", "startup", "delay", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-livedump",
            Label = "Disable Kernel Live Dump Collection",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Kernel Live Dump collection (EnableLiveDump=0). Live dumps are taken by heuristics without a full crash; disabling reduces unexpected disk I/O and performance spikes. Default: enabled.",
            Tags = ["boot", "dump", "kernel", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-enable-nmi-crash-dump",
            Label = "Enable NMI-Triggered Crash Dump",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables triggering a crash dump via a Non-Maskable Interrupt (NMI) button or debugger. Useful for generating a dump on a completely hung system that cannot respond to other input. Default: disabled.",
            Tags = ["boot", "nmi", "dump", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-bsod-beep",
            Label = "Disable System Beep on BSOD",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the PC speaker beep that Windows emits when a BSOD (blue screen of death) occurs. Reduces noise in server rooms or overnight unattended machines. Default: 1 (beep enabled).",
            Tags = ["boot", "bsod", "beep", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-always-keep-dump",
            Label = "Do Not Permanently Keep Memory Dump",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AlwaysKeepMemoryDump=0 so Windows does not permanently retain the memory dump even when low on disk. Lets the pagefile cleanup process remove the dump to free space. Default: 0.",
            Tags = ["boot", "dump", "disk", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-system-eventlog-size",
            Label = "Increase System Event Log Size to 50 MB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the System event log maximum size to 50 MB (52428800 bytes). Allows retention of more historical system events before wrapping. Default: 20 MB.",
            Tags = ["boot", "event-log", "system", "size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-status-display",
            Label = "Disable Boot Status / Spinner Display",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the display of boot status messages (spinner/dots) during startup by clearing DisplayStatusMessages. Produces a cleaner, faster-feeling boot sequence. Default: enabled.",
            Tags = ["boot", "ui", "spinner", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
        },
        // ── merged from: Services.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "svc-disable-sysmain-service",
            Label = "Disable SysMain (Superfetch)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service — beneficial on SSD systems.",
            Tags = ["services", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagsvc",
            Label = "Disable Diagnostic Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Diagnostic Service (DiagSvc) that runs troubleshooters.",
            Tags = ["services", "telemetry", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wbiosrvc",
            Label = "Disable Biometric Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Biometric Service (WbioSrvc). Useful if fingerprint/face login is not used.",
            Tags = ["services", "biometric", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remote-registry",
            Label = "Disable Remote Registry",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Remote Registry service which allows remote access to the Windows registry. Security hardening measure.",
            Tags = ["services", "security", "remote"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Geolocation Service for privacy.",
            Tags = ["services", "privacy", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-delivery-optimization-svc",
            Label = "Disable Delivery Optimization",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Delivery Optimization service which shares Windows Update data with other PCs on LAN and internet.",
            Tags = ["services", "update", "bandwidth", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fax",
            Label = "Disable Fax Service (Cleanup)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the legacy Fax service to free resources. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "fax", "legacy", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-smartcard",
            Label = "Disable Smart Card Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Smart Card service (SCardSvr) for smart-card readers. Safe to disable if no smart cards are used. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "smartcard", "scardsvr", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-link-tracking",
            Label = "Disable Distributed Link Tracking Client",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Distributed Link Tracking Client (TrkWks) that maintains NTFS file links across networked computers. Default: Manual. Recommended: Disabled for standalone PCs.",
            Tags = ["services", "link-tracking", "trkwks", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wallet",
            Label = "Disable Wallet Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Wallet Service used for NFC-based payments. Safe to disable if contactless payments are unused. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "wallet", "nfc", "payment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-secondary-logon",
            Label = "Disable Secondary Logon Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Secondary Logon (RunAs) service. Reduces privilege escalation surface. Default: manual.",
            Tags = ["services", "secondary-logon", "runas", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-xbox-live-networking",
            Label = "Disable Xbox Live Networking Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Xbox Live Networking service. Not needed if you don't use Xbox features. Default: manual.",
            Tags = ["services", "xbox", "networking", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-webclient",
            Label = "Disable WebClient (WebDAV) Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WebClient service (WebDAV). Reduces attack surface for NTLM relay. Default: manual.",
            Tags = ["services", "webclient", "webdav", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
        },
        // ── Command-based service tweaks (sc.exe) ──────────────────────────
        new TweakDef
        {
            Id = "svc-stop-xbox-services",
            Label = "Stop & Disable All Xbox Services (sc)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops and disables all Xbox-related services (XblAuthManager, XblGameSave, XboxGipSvc, XboxNetApiSvc) to free resources.",
            Tags = ["services", "xbox", "disable", "gaming", "resources"],
            KindHint = TweakKind.ServiceControl,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                foreach (var svc in new[] { "XblAuthManager", "XblGameSave", "XboxGipSvc", "XboxNetApiSvc" })
                {
                    Elevation.RunElevated("sc", ["stop", svc]);
                    Elevation.RunElevated("sc", ["config", svc, "start=", "disabled"]);
                }
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                foreach (var svc in new[] { "XblAuthManager", "XblGameSave", "XboxGipSvc", "XboxNetApiSvc" })
                {
                    Elevation.RunElevated("sc", ["config", svc, "start=", "demand"]);
                }
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("sc", ["qc", "XblAuthManager"]);
                return stdout.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "svc-stop-connected-devices",
            Label = "Stop & Disable Connected Devices Platform Service (sc)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Connected Devices Platform (CDP) service used for cross-device experiences, Timeline, and nearby sharing.",
            Tags = ["services", "cdp", "connected", "devices", "cross-device"],
            KindHint = TweakKind.ServiceControl,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("sc", ["stop", "CDPSvc"]);
                Elevation.RunElevated("sc", ["config", "CDPSvc", "start=", "disabled"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("sc", ["config", "CDPSvc", "start=", "auto"]);
                Elevation.RunElevated("sc", ["start", "CDPSvc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("sc", ["qc", "CDPSvc"]);
                return stdout.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "svc-disable-upnphost",
            Label = "Disable UPnP Device Host Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the UPnP Device Host service. Prevents this machine from acting as a discoverable UPnP host, reducing the attack surface on untrusted networks.",
            Tags = ["services", "upnp", "network", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fdphost",
            Label = "Disable Function Discovery Provider Host",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the fdPHost service. Stops Windows from using WS-Discovery and other protocols to automatically find networked printers and devices.",
            Tags = ["services", "fdphost", "discovery", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fdrespub",
            Label = "Disable Function Discovery Resource Publication Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the FDResPub service. Prevents this machine from advertising itself on the local network via WS-Discovery, removing it from the Network neighbourhood of other PCs.",
            Tags = ["services", "fdrespub", "publication", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-icssvc",
            Label = "Disable Internet Connection Sharing (ICS)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the SharedAccess (ICS) service. Removes Windows' built-in NAT router capability, preventing accidental or unauthorised sharing of the internet connection.",
            Tags = ["services", "ics", "sharing", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-mapbroker",
            Label = "Disable Downloaded Maps Manager Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the MapsBroker service. Stops Windows from periodically downloading offline map data updates in the background.",
            Tags = ["services", "maps", "offline", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remoteaccess",
            Label = "Disable Routing and Remote Access (RRAS) Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the RemoteAccess (RRAS) service. Stops Windows from acting as a software router/VPN server. Not needed on standard workstations.",
            Tags = ["services", "routing", "vpn", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wisvc",
            Label = "Disable Windows Insider Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the wisvc (Windows Insider Service). Prevents the device from being enrolled in Windows Insider preview flight deliveries and associated telemetry collection.",
            Tags = ["services", "insider", "preview", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-autotimesvc",
            Label = "Disable Cellular Time Synchronisation Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the autotimesvc (Cellular Time) service. This service syncs the system clock via mobile-broadband data — not needed on non-cellular or always-connected PCs.",
            Tags = ["services", "time", "cellular", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-napagent",
            Label = "Disable Network Access Protection (NAP) Agent",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the napagent (Network Access Protection Agent) service. NAP is deprecated since Windows Server 2012 R2 and the agent is unused on modern workstations.",
            Tags = ["services", "nap", "legacy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-tzautoupdate",
            Label = "Disable Automatic Time Zone Updater Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the tzautoupdate service. Prevents Windows from automatically adjusting the system time zone based on location data. Useful for servers and VMs where the time zone should be fixed.",
            Tags = ["services", "timezone", "automatic", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagnosticshub",
            Label = "Disable Diagnostics Hub Standard Collector Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the diagnosticshub.standardcollector.service. This service collects real-time diagnostic events from ETW providers for Visual Studio profiling sessions — not needed outside of profiling.",
            Tags = ["services", "diagnostics", "etw", "profiling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 4)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 3),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 4),
            ],
        },
    ];
}

