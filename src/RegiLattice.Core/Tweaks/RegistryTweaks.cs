namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RegistryTweaks
{
    private const string CfgMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "reg-set-hive-checkpoint-60s",
            Label = "Set Registry Hive Checkpoint Interval to 60s",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
