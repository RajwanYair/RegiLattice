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
    ];
}
