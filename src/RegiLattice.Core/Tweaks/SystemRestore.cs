namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class SystemRestore
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string SrKey = $@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
    private const string SppKey = $@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
    private const string VssKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "restore-enable-scheduled-points",
            Label = "Enable Scheduled Restore Points",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the scheduled restore point creation task that creates weekly restore points.",
            Tags = ["restore", "system-protection", "scheduled", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent", "ScheduleEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent", "ScheduleEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SPP\CreateFilesPresent", "ScheduleEnabled", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-queue",
            Label = "Disable Windows Error Reporting Queue",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WER report queue that stores crash reports. Saves disk space and reduces I/O.",
            Tags = ["restore", "wer", "error-reporting", "disk-space", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-max-archive-5",
            Label = "Limit WER Archive to 5 Reports",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the WER archive to 5 stored reports. Older reports are automatically purged.",
            Tags = ["restore", "wer", "error-reporting", "archive", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxArchiveCount", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxArchiveCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxArchiveCount", 5)],
        },
        new TweakDef
        {
            Id = "restore-disable-auto-recovery-boot",
            Label = "Disable Automatic Recovery on Boot Failure",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the automatic boot repair that triggers after consecutive boot failures. Prevents unwanted repair loops.",
            Tags = ["restore", "boot", "recovery", "repair"],
            SideEffects = "Must manually repair if boot issues occur.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecute_Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecute_Disabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecute_Disabled", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-shadow-copy-optimisation",
            Label = "Disable VSS Disk Space Optimisation",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Volume Shadow Copy optimization that runs during idle periods. Reduces background I/O.",
            Tags = ["restore", "vss", "shadow-copy", "performance", "io"],
            RegistryKeys = [VssKey],
            ApplyOps = [RegOp.SetDword(VssKey, "MinDiffAreaFileSize", 3000)],
            RemoveOps = [RegOp.DeleteValue(VssKey, "MinDiffAreaFileSize")],
            DetectOps = [RegOp.CheckDword(VssKey, "MinDiffAreaFileSize", 3000)],
        },
        new TweakDef
        {
            Id = "restore-disable-hiberfil",
            Label = "Disable Hibernate File (Reclaim Disk Space)",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows hibernation support to reclaim the hiberfil.sys disk space (typically 5–10 GB).",
            Tags = ["restore", "hibernate", "disk-space", "power", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HibernateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HibernateEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "restore-suppress-wer-second-level-data",
            Label = "Suppress WER Second-Level Data Collection",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Error Reporting from uploading additional heap/module diagnostic data on crashes.",
            Tags = ["restore", "wer", "privacy", "telemetry", "crash", "data"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "NoSecondLevelData", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "NoSecondLevelData")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "NoSecondLevelData", 1)],
        },
        new TweakDef
        {
            Id = "restore-limit-wer-report-queue",
            Label = "Limit WER Report Queue to 2",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of pending crash reports queued for upload to 2, saving disk space.",
            Tags = ["restore", "wer", "disk-space", "queue", "crash"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue", "MaxQueueCount", 2)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportQueue", "MaxQueueCount", 2)],
        },
        new TweakDef
        {
            Id = "restore-limit-wer-archive-size",
            Label = "Limit WER Archive to 5 Files",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the number of archived crash reports retained locally to 5 files.",
            Tags = ["restore", "wer", "disk-space", "archive", "crash"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive", "MaxArchiveCount", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive", "MaxArchiveCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ReportArchive", "MaxArchiveCount", 5)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-throttle-bypass",
            Label = "Disable WER Network Throttle Bypass",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WER from bypassing network cost throttling when uploading reports on metered connections.",
            Tags = ["restore", "wer", "network", "throttle", "metered", "bandwidth"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "BypassNetworkCostThrottling", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "BypassNetworkCostThrottling")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "BypassNetworkCostThrottling", 0)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-response-timeout",
            Label = "Set WER Server Response Timeout (20 s)",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets a 20-second timeout for the WER crash report server to reduce startup delays on slow networks.",
            Tags = ["restore", "wer", "timeout", "performance", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs", 20)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs", 20)],
        },
    ];
}
