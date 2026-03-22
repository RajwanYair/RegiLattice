// RegiLattice.Core — Tweaks/ShadowCopyVss.cs
// Volume Shadow Copy Service (VSS) policy tweaks (Sprint 107).
// Slug: "vss-*" — controls VSS storage limits, automatic shadow copy scheduling,
// system restore via VSS, and VSS writer behaviour.
// Registry bases:
//   HKLM\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore
//   HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore
//   HKLM\SYSTEM\CurrentControlSet\Services\VSS\Settings

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ShadowCopyVss
{
    private const string VssSettings = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings";
    private const string SrPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
    private const string SrSettings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
    private const string VssDisks = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VolSnap";
    private const string VssWriters = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore\Cfg";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vss-enable-system-restore",
            Label = "VSS: Enable System Restore (Volume Shadow Copy)",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 5,
            RegistryKeys = [SrPolicy],
            Tags = ["vss", "shadow-copy", "system-restore", "backup", "recovery"],
            Description =
                "Sets DisableSR=0 and DisableConfig=0 in the SystemRestore policy. "
                + "Enables System Restore protection so that Windows can automatically create restore "
                + "points before significant changes (updates, driver installs). "
                + "Required for Automatic Repair to offer working restore points.",
            ApplyOps = [RegOp.SetDword(SrPolicy, "DisableSR", 0), RegOp.SetDword(SrPolicy, "DisableConfig", 0)],
            RemoveOps = [RegOp.DeleteValue(SrPolicy, "DisableSR"), RegOp.DeleteValue(SrPolicy, "DisableConfig")],
            DetectOps = [RegOp.CheckDword(SrPolicy, "DisableSR", 0), RegOp.CheckDword(SrPolicy, "DisableConfig", 0)],
        },
        new TweakDef
        {
            Id = "vss-disable-system-restore",
            Label = "VSS: Disable System Restore to Reclaim Disk Space",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 2,
            RegistryKeys = [SrPolicy],
            Tags = ["vss", "shadow-copy", "system-restore", "disk-space", "cleanup"],
            Description =
                "Sets DisableSR=1 in the SystemRestore policy. "
                + "Turns off Windows System Restore across all drives. "
                + "Reclaims disk space reserved for shadow copies (up to several GB). "
                + "WARNING: disabling System Restore removes the rollback mechanism for driver/update "
                + "failures. Only recommended for SSD-constrained machines with alternative backups.",
            ApplyOps = [RegOp.SetDword(SrPolicy, "DisableSR", 1)],
            RemoveOps = [RegOp.DeleteValue(SrPolicy, "DisableSR")],
            DetectOps = [RegOp.CheckDword(SrPolicy, "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "vss-set-restore-point-frequency-daily",
            Label = "VSS: Limit Automatic Restore Points to Once Per Day",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [SrSettings],
            Tags = ["vss", "shadow-copy", "system-restore", "disk-space", "performance"],
            Description =
                "Sets RPSessionInterval=1440 (minutes) in SystemRestore settings. "
                + "Prevents Windows from creating more than one restore point per day when apps "
                + "request it. Default is 0 (no interval restriction), which can create dozens of "
                + "restore points in a single app-install session and consume significant disk space.",
            ApplyOps = [RegOp.SetDword(SrSettings, "RPSessionInterval", 1440)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "RPSessionInterval")],
            DetectOps = [RegOp.CheckDword(SrSettings, "RPSessionInterval", 1440)],
        },
        new TweakDef
        {
            Id = "vss-limit-shadow-storage-15pct",
            Label = "VSS: Cap Shadow Copy Storage at 15% of Drive Capacity",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 4,
            RegistryKeys = [SrSettings],
            Tags = ["vss", "shadow-copy", "disk-space", "storage"],
            Description =
                "Sets DiskPercent=15 in SystemRestore settings. "
                + "Limits the maximum disk space that VSS/System Restore can use across all drives "
                + "to 15% of each volume's capacity. The Windows default varies (often up to 30%). "
                + "Lower value = fewer old restore points retained; reduces disk footprint.",
            ApplyOps = [RegOp.SetDword(SrSettings, "DiskPercent", 15)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "DiskPercent")],
            DetectOps = [RegOp.CheckDword(SrSettings, "DiskPercent", 15)],
        },
        new TweakDef
        {
            Id = "vss-increase-writer-timeout",
            Label = "VSS: Increase Writer Timeout to 120 Seconds",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [VssSettings],
            Tags = ["vss", "shadow-copy", "timeout", "backup", "reliability"],
            Description =
                "Sets MaxWriterTimeInSeconds=120 in VSS\\Settings. "
                + "Increases the maximum time the VSS coordinator waits for individual writers "
                + "before declaring a VSS snapshot failure. Default is 60 seconds. "
                + "Helps with slow VSS writers (e.g. SQL Server, Exchange) on loaded servers.",
            ApplyOps = [RegOp.SetDword(VssSettings, "MaxWriterTimeInSeconds", 120)],
            RemoveOps = [RegOp.DeleteValue(VssSettings, "MaxWriterTimeInSeconds")],
            DetectOps = [RegOp.CheckDword(VssSettings, "MaxWriterTimeInSeconds", 120)],
        },
        new TweakDef
        {
            Id = "vss-enable-unbuffered-writes",
            Label = "VSS: Enable Unbuffered Writes for Faster Shadow-Copy Creation",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 3,
            RegistryKeys = [VssSettings],
            Tags = ["vss", "shadow-copy", "performance", "backup"],
            Description =
                "Sets MaxFileBufferSize=0 in VSS\\Settings. "
                + "Switches VSS copy-on-write I/O from buffered to unbuffered (direct I/O) mode. "
                + "Can reduce shadow-copy overhead on fast NVMe/SSD arrays. "
                + "Not recommended on spinning-disk (HDD) systems — increases seek pressure.",
            ApplyOps = [RegOp.SetDword(VssSettings, "MaxFileBufferSize", 0)],
            RemoveOps = [RegOp.DeleteValue(VssSettings, "MaxFileBufferSize")],
            DetectOps = [RegOp.CheckDword(VssSettings, "MaxFileBufferSize", 0)],
        },
        new TweakDef
        {
            Id = "vss-disable-snapvol-for-fixed-drives",
            Label = "VSS: Disable VolSnap Auto-Snapshot on Fixed Drives",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 3,
            RegistryKeys = [VssDisks],
            Tags = ["vss", "shadow-copy", "volsnap", "disk-space", "performance"],
            Description =
                "Sets AllowSnapshotsOnFixedDrives=0 in VolSnap settings. "
                + "Prevents the VolSnap driver from creating automatic snapshots on fixed (non-removable) "
                + "drives. Releases snapshot storage immediately. Not recommended if you rely on "
                + "VSS-based backup tools (Veeam, Windows Server Backup, etc.).",
            ApplyOps = [RegOp.SetDword(VssDisks, "AllowSnapshotsOnFixedDrives", 0)],
            RemoveOps = [RegOp.DeleteValue(VssDisks, "AllowSnapshotsOnFixedDrives")],
            DetectOps = [RegOp.CheckDword(VssDisks, "AllowSnapshotsOnFixedDrives", 0)],
        },
        new TweakDef
        {
            Id = "vss-disallow-user-config",
            Label = "VSS: Hide System Restore Configuration from Standard Users",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [SrPolicy],
            Tags = ["vss", "shadow-copy", "system-restore", "security", "policy"],
            Description =
                "Sets DisableConfig=1 in the SystemRestore policy. "
                + "Hides the System Restore configuration tab in System Properties from standard users. "
                + "They cannot turn off system protection, create restore points manually, or change "
                + "shadow copy storage size. Admins retain full access.",
            ApplyOps = [RegOp.SetDword(SrPolicy, "DisableConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(SrPolicy, "DisableConfig")],
            DetectOps = [RegOp.CheckDword(SrPolicy, "DisableConfig", 1)],
        },
        new TweakDef
        {
            Id = "vss-set-min-restore-point-space-300mb",
            Label = "VSS: Set Minimum Shadow-Copy Reservation to 300 MB",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 4,
            RegistryKeys = [VssWriters],
            Tags = ["vss", "shadow-copy", "disk-space", "storage", "system-restore"],
            Description =
                "Sets MinDiskSpace=314572800 (300 MB in bytes) in SystemRestore\\Cfg. "
                + "Sets the minimum disk space reservation for the shadow copy provider. "
                + "If free space drops below this threshold, no new snapshots are created. "
                + "300 MB is tighter than the Windows default (1 GB), saving space on small SSDs.",
            ApplyOps = [RegOp.SetDword(VssWriters, "MinDiskSpace", 314572800)],
            RemoveOps = [RegOp.DeleteValue(VssWriters, "MinDiskSpace")],
            DetectOps = [RegOp.CheckDword(VssWriters, "MinDiskSpace", 314572800)],
        },
        new TweakDef
        {
            Id = "vss-disable-rp-before-critical-updates",
            Label = "VSS: Skip Automatic Restore Points Before Windows Updates",
            Category = "Volume Shadow Copy (VSS)",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 3,
            RegistryKeys = [SrSettings],
            Tags = ["vss", "shadow-copy", "windows-update", "disk-space", "performance"],
            Description =
                "Sets CreatePointBeforeCriticalPatches=0 in SystemRestore settings. "
                + "Prevents Windows from automatically creating a restore point immediately before "
                + "installing critical Windows Updates. Saves disk space on small SSDs at the cost of "
                + "losing the rollback safety net for a failed update.",
            ApplyOps = [RegOp.SetDword(SrSettings, "CreatePointBeforeCriticalPatches", 0)],
            RemoveOps = [RegOp.DeleteValue(SrSettings, "CreatePointBeforeCriticalPatches")],
            DetectOps = [RegOp.CheckDword(SrSettings, "CreatePointBeforeCriticalPatches", 0)],
        },
    ];
}
