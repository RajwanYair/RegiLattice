namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// System Restore and shadow copy tweaks — configures System Protection,
/// restore point creation, Volume Shadow Copy, and Previous Versions.
/// </summary>
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
            Id = "restore-disable-system-restore",
            Label = "Disable System Restore",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables System Restore entirely. Saves disk space but removes ability to rollback system changes.",
            Tags = ["restore", "system-protection", "disk-space", "recovery"],
            RegistryKeys = [SppKey],
            ApplyOps = [RegOp.SetDword(SppKey, "DisableSR", 1)],
            RemoveOps = [RegOp.DeleteValue(SppKey, "DisableSR")],
            DetectOps = [RegOp.CheckDword(SppKey, "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-config-change-restore",
            Label = "Disable Restore Point on Config Changes",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic restore point creation when system configuration changes are made.",
            Tags = ["restore", "system-protection", "performance", "auto"],
            RegistryKeys = [SppKey],
            ApplyOps = [RegOp.SetDword(SppKey, "DisableConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(SppKey, "DisableConfig")],
            DetectOps = [RegOp.CheckDword(SppKey, "DisableConfig", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-max-frequency-daily",
            Label = "Limit Restore Points to Once Per Day",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets minimum interval between automatic restore points to 24 hours (1440 minutes). Reduces disk usage from frequent restore points.",
            Tags = ["restore", "system-protection", "performance", "frequency", "disk-space"],
            RegistryKeys = [SrKey],
            ApplyOps = [RegOp.SetDword(SrKey, "SystemRestorePointCreationFrequency", 1440)],
            RemoveOps = [RegOp.DeleteValue(SrKey, "SystemRestorePointCreationFrequency")],
            DetectOps = [RegOp.CheckDword(SrKey, "SystemRestorePointCreationFrequency", 1440)],
        },
        new TweakDef
        {
            Id = "restore-disable-vss-service",
            Label = "Set Volume Shadow Copy to Manual",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Volume Shadow Copy (VSS) service start type to manual. Saves resources but disables automatic backups.",
            Tags = ["restore", "vss", "shadow-copy", "service", "performance"],
            RegistryKeys = [VssKey],
            ApplyOps = [RegOp.SetDword(VssKey, "Start", 3)],
            RemoveOps = [RegOp.SetDword(VssKey, "Start", 2)],
            DetectOps = [RegOp.CheckDword(VssKey, "Start", 3)],
        },
        new TweakDef
        {
            Id = "restore-disable-previous-versions",
            Label = "Disable Previous Versions Tab",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Hides the Previous Versions tab in file/folder properties. Reduces VSS dependency.",
            Tags = ["restore", "previous-versions", "explorer", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableBackupRestore", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableBackupRestore"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableLocalPage"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PreviousVersions", "DisableBackupRestore", 1)],
        },
        new TweakDef
        {
            Id = "restore-enable-scheduled-points",
            Label = "Enable Scheduled Restore Points",
            Category = "System Restore",
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
            Category = "System Restore",
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
            Id = "restore-disable-wer-archive",
            Label = "Disable WER Report Archiving",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables archiving of sent Windows Error Reports. Reduces disk space usage.",
            Tags = ["restore", "wer", "error-reporting", "archive", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-consent-send-always",
            Label = "Auto-Send Error Reports (No Prompt)",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Automatically sends Windows Error Reports without prompting the user. Reduces interruptions.",
            Tags = ["restore", "wer", "error-reporting", "consent", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 4)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 4)],
        },
        new TweakDef
        {
            Id = "restore-disable-memory-dump",
            Label = "Disable Memory Dump File Creation",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables writing of memory dump files on system crash. Saves significant disk space.",
            Tags = ["restore", "crash", "dump", "disk-space", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
        },
        new TweakDef
        {
            Id = "restore-set-mini-dump-only",
            Label = "Set Crash Dump to Small Memory Dump",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets crash dump type to small memory dump (256 KB). Saves disk space while still capturing essential crash info.",
            Tags = ["restore", "crash", "dump", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "restore-overwrite-existing-dump",
            Label = "Overwrite Existing Crash Dump on New Crash",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Windows to overwrite the existing crash dump file on each new crash. Prevents unbounded disk usage.",
            Tags = ["restore", "crash", "dump", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
        },
        new TweakDef
        {
            Id = "restore-disable-auto-reboot-on-crash",
            Label = "Disable Auto-Reboot on Blue Screen",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically rebooting after a blue screen, allowing you to read the error message.",
            Tags = ["restore", "crash", "bsod", "reboot", "diagnostics"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "restore-disable-wer-logging",
            Label = "Disable Windows Error Reporting Logging",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Error Reporting event logging to reduce unnecessary disk I/O.",
            Tags = ["restore", "wer", "error-reporting", "logging", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-max-queue-5",
            Label = "Limit WER Report Queue to 5",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the Windows Error Reporting queue to 5 reports maximum. Prevents disk space waste from accumulated reports.",
            Tags = ["restore", "wer", "error-reporting", "queue", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 5)],
        },
        new TweakDef
        {
            Id = "restore-set-wer-max-archive-5",
            Label = "Limit WER Archive to 5 Reports",
            Category = "System Restore",
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
            Category = "System Restore",
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
            Category = "System Restore",
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
            Id = "restore-disable-wer-dump-type",
            Label = "Disable WER Full Crash Dump Collection",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets WER to collect mini dumps instead of full application crash dumps. Saves disk space.",
            Tags = ["restore", "wer", "crash", "dump", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 1)],
        },
        new TweakDef
        {
            Id = "restore-limit-wer-dump-count",
            Label = "Limit Local Crash Dumps to 3",
            Category = "System Restore",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of local crash dump files retained per application to 3.",
            Tags = ["restore", "wer", "crash", "dump", "disk-space", "limit"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 3)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 3)],
        },
    ];
}
