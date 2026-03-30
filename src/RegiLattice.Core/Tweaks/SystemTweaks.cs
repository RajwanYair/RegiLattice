namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SystemTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sys-disable-reserved-storage",
            Label = "Disable Reserved Storage (~7 GB)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Reserved Storage which holds ~7 GB for updates.",
            Tags = ["system", "disk", "storage", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
        },

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
            Id = "sys-disable-fast-startup",
            Label = "Disable Fast Startup (HiberBoot)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Fast Startup (hybrid shutdown). Ensures clean boot every time. Default: Enabled. Recommended: Disabled.",
            Tags = ["system", "fast-startup", "hiberboot", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
            Category = "Registry Management",
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
