namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Advanced system tuning tweaks — memory management, I/O scheduling,
/// kernel parameters, process priority, and OS-level performance knobs.
/// Sprint 25 — Phase 4/5 roadmap items.
/// </summary>
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
            Category = "System Optimization",
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
