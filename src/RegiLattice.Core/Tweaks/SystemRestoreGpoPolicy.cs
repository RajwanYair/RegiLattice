// System Restore Group Policy — Sprint 147
// Slug "srgpo" — controls System Restore enablement, configuration locking,
// and Volume Shadow Copy service settings via Group Policy paths.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class SystemRestoreGpoPolicy
{
    private const string SrPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";

    private const string SrSettings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "srgpo-disable-sr-policy",
            Label = "System Restore: Disable System Restore via Group Policy",
            Category = "System Restore Policy",
            Description =
                "Sets DisableSR=1 in the SystemRestore policy key. Turns off System Restore for all "
                + "drives and prevents automatic restore point creation. Frees disk space used by VSC.",
            Tags = ["system-restore", "vss", "rollback", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrPol, "DisableSR", 1)],
            RemoveOps = [RegOp.DeleteValue(SrPol, "DisableSR")],
            DetectOps = [RegOp.CheckDword(SrPol, "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "srgpo-disable-config-policy",
            Label = "System Restore: Lock out System Restore configuration UI",
            Category = "System Restore Policy",
            Description =
                "Sets DisableConfig=1 in the SystemRestore policy key. Hides the 'Configure' button "
                + "on the System Protection tab, preventing users from enabling or adjusting SR settings.",
            Tags = ["system-restore", "lockout", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrPol, "DisableConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(SrPol, "DisableConfig")],
            DetectOps = [RegOp.CheckDword(SrPol, "DisableConfig", 1)],
        },
        new TweakDef
        {
            Id = "srgpo-set-rp-session-interval",
            Label = "System Restore: Set restore-point creation interval to 1 day",
            Category = "System Restore Policy",
            Description =
                "Sets RPSessionInterval=1 in SystemRestore settings. Limits automatic restore-point "
                + "creation frequency to once per day rather than every session start, saving disk space.",
            Tags = ["system-restore", "interval", "schedule", "optimization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "RPSessionInterval", 1)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "RPSessionInterval")],
            DetectOps = [RegOp.CheckDword(SrSettings, "RPSessionInterval", 1)],
        },
        new TweakDef
        {
            Id = "srgpo-set-rp-global-interval",
            Label = "System Restore: Set global restore-point creation interval (24 hr)",
            Category = "System Restore Policy",
            Description =
                "Sets RPGlobalInterval=1440 (minutes = 24 hours). Controls how often System Restore "
                + "creates scheduled restore points, capping frequency to once per 24-hour period.",
            Tags = ["system-restore", "interval", "global", "schedule"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "RPGlobalInterval", 1440)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "RPGlobalInterval")],
            DetectOps = [RegOp.CheckDword(SrSettings, "RPGlobalInterval", 1440)],
        },
        new TweakDef
        {
            Id = "srgpo-set-max-disk-usage",
            Label = "System Restore: Cap restore-point disk usage at 5 %",
            Category = "System Restore Policy",
            Description =
                "Sets DiskPercent=5 in SystemRestore settings. Limits the maximum disk space that "
                + "System Restore shadow copies may consume to 5 % of the system drive.",
            Tags = ["system-restore", "disk-space", "storage", "optimization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "DiskPercent", 5)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "DiskPercent")],
            DetectOps = [RegOp.CheckDword(SrSettings, "DiskPercent", 5)],
        },
        new TweakDef
        {
            Id = "srgpo-disable-system-checkpoint",
            Label = "System Restore: Disable automatic system checkpoints",
            Category = "System Restore Policy",
            Description =
                "Sets CreateSystemCheckPoints=0 in SystemRestore settings. Prevents Windows from "
                + "automatically creating restore points during system events such as updates.",
            Tags = ["system-restore", "checkpoint", "automatic", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "CreateSystemCheckPoints", 0)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "CreateSystemCheckPoints")],
            DetectOps = [RegOp.CheckDword(SrSettings, "CreateSystemCheckPoints", 0)],
        },
        new TweakDef
        {
            Id = "srgpo-disable-scan-checkpoint",
            Label = "System Restore: Disable restore point creation before scan/cleanup",
            Category = "System Restore Policy",
            Description =
                "Sets ScanInterval=0 in SystemRestore settings. Stops Windows Security (and legacy "
                + "Defender) from automatically creating a restore point before each full scan.",
            Tags = ["system-restore", "scan", "checkpoint", "defender"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "ScanInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "ScanInterval")],
            DetectOps = [RegOp.CheckDword(SrSettings, "ScanInterval", 0)],
        },
        new TweakDef
        {
            Id = "srgpo-disable-optimistic-restore",
            Label = "System Restore: Disable optimistic restore support",
            Category = "System Restore Policy",
            Description =
                "Sets OptimisticRestore=0 in SystemRestore settings. Disables the optimistic-restore "
                + "code path that tries to recover the system without a full restore after certain failures.",
            Tags = ["system-restore", "recovery", "optimization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "OptimisticRestore", 0)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "OptimisticRestore")],
            DetectOps = [RegOp.CheckDword(SrSettings, "OptimisticRestore", 0)],
        },
        new TweakDef
        {
            Id = "srgpo-disable-batch-restore-points",
            Label = "System Restore: Disable batch software-install restore point creation",
            Category = "System Restore Policy",
            Description =
                "Sets RestorePointCreationFrequency=0 in SystemRestore settings. Prevents batching of "
                + "multiple restore-point creation requests within a single install session.",
            Tags = ["system-restore", "batch", "software-install", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "RestorePointCreationFrequency", 0)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "RestorePointCreationFrequency")],
            DetectOps = [RegOp.CheckDword(SrSettings, "RestorePointCreationFrequency", 0)],
        },
        new TweakDef
        {
            Id = "srgpo-disable-incremental-rps",
            Label = "System Restore: Disable incremental restore point diff storage",
            Category = "System Restore Policy",
            Description =
                "Sets PreventIncrementalRestorations=1 in SystemRestore settings. Forces each restore "
                + "point to be a full snapshot rather than an incremental delta, ensuring clean rollback.",
            Tags = ["system-restore", "incremental", "snapshot", "storage"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SrSettings, "PreventIncrementalRestorations", 1)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "PreventIncrementalRestorations")],
            DetectOps = [RegOp.CheckDword(SrSettings, "PreventIncrementalRestorations", 1)],
        },
    ];
}
