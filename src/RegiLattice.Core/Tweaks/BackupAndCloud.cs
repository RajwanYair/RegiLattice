namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Backup.cs ──
internal static class Backup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "backup-disable-file-history",
            Label = "Disable File History",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows File History backup feature via Group Policy. Useful when you use a third-party backup solution instead.",
            Tags = ["backup", "file-history", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-system-restore",
            Label = "Disable System Restore",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables System Restore and removes existing restore points. Frees disk space but removes the safety net. Use with caution.",
            Tags = ["backup", "system-restore", "disk", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "backup-vss-manual",
            Label = "Set Volume Shadow Copy to Manual",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the VSS service to Manual start. Reduces background I/O if you don't use System Restore or Previous Versions.",
            Tags = ["backup", "vss", "shadow-copy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
        },
        new TweakDef
        {
            Id = "backup-disable-backup-ui",
            Label = "Disable Windows Backup Settings Page",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hides the Windows Backup page in Settings via policy. Useful for managed environments that use different backup solutions.",
            Tags = ["backup", "settings", "policy", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup", "DisableBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup", "DisableBackupUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup", "DisableBackupUI", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-previous-versions",
            Label = "Disable Previous Versions Tab",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Previous Versions' tab from file/folder properties. Cleans up the context menu when VSS is not in use.",
            Tags = ["backup", "previous-versions", "explorer", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Stops Windows Error Reporting from collecting and sending crash data. Reduces disk I/O and network usage from WER telemetry uploads.",
            Tags = ["backup", "wer", "error-reporting", "privacy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-reliability-monitor",
            Label = "Disable Reliability Monitoring",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Reliability Monitor data collection by setting TimeStampInterval to 0. Reduces background I/O.",
            Tags = ["backup", "reliability", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0)],
        },
        new TweakDef
        {
            Id = "backup-bak-increase-shadow-storage",
            Label = "Increase Shadow Copy Storage Limit",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the maximum number of shadow copies to 64. Allows more restore points to be retained. Default: 16. Recommended: 64.",
            Tags = ["backup", "shadow-copy", "vss", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
        },
        new TweakDef
        {
            Id = "backup-bak-disable-restore-low-disk",
            Label = "Disable System Restore on Low Disk",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents System Restore from being automatically disabled when disk space is low. Keeps restore points available. Default: auto-disable. Recommended: disabled.",
            Tags = ["backup", "system-restore", "disk", "low-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSROnLowDisk", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSROnLowDisk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSROnLowDisk", 1)],
        },
        new TweakDef
        {
            Id = "backup-bak-set-backup-interval",
            Label = "Set Backup Schedule Interval to 24 Hours",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Windows Backup schedule interval to 24 hours. Ensures regular daily backups without excessive frequency. Default: not set. Recommended: 24 hours.",
            Tags = ["backup", "schedule", "interval", "frequency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "BackupFrequency", 24)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "BackupFrequency")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "BackupFrequency", 24)],
        },
        new TweakDef
        {
            Id = "backup-sr-frequency-unlimited",
            Label = "Allow Frequent System Restore Points",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the built-in 24-hour cooldown for System Restore point creation. Allows tools and scripts to create restore points at any time. Default: 1440 min limit. Recommended: unlimited (0).",
            Tags = ["backup", "system-restore", "frequency", "vss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "SystemRestorePointCreationFrequency", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "SystemRestorePointCreationFrequency"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore",
                    "SystemRestorePointCreationFrequency",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "backup-vss-auto-start",
            Label = "Set Volume Shadow Copy to Automatic Start",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the VSS (Volume Shadow Copy Service) start type to Automatic so shadow copies are always available on boot. Default: Manual. Recommended: Automatic on backup-heavy machines.",
            Tags = ["backup", "vss", "shadow-copy", "service", "auto"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS", "Start", 2)],
        },
        new TweakDef
        {
            Id = "backup-wer-reduce-queue",
            Label = "Limit Windows Error Reporting Queue to 1",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the Windows Error Reporting queue to 1 pending report, reducing disk usage from accumulated crash reports. Default: 50. Recommended: 1 for privacy.",
            Tags = ["backup", "wer", "error-reporting", "queue", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-backup-schedule-nag",
            Label = "Suppress Backup Schedule Balloon Notification",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                @"Suppresses the ""Set up Windows Backup"" balloon notification that appears in the system tray when no backup is configured. Default: shown. Recommended: suppressed.",
            Tags = ["backup", "notification", "balloon", "tray"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "NoBackupBalloonNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "NoBackupBalloonNotifications")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "NoBackupBalloonNotifications", 1),
            ],
        },
        new TweakDef
        {
            Id = "backup-disable-aedebug-auto",
            Label = "Disable Auto-Attach of JIT Debugger on Crash",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically launching a JIT (Just-In-Time) debugger when an application crashes. Suppresses the debugger prompt. Default: 1 (auto-attach). Recommended: 0 (no auto-attach).",
            Tags = ["backup", "crash", "debugger", "aedebug", "jit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "0")],
        },
        new TweakDef
        {
            Id = "backup-disable-system-image",
            Label = "Disable System Image Backup",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the legacy Windows 7 system image backup feature. This feature is deprecated in Windows 10/11. Default: available.",
            Tags = ["backup", "system-image", "deprecated", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableSystemBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableSystemBackupUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableSystemBackupUI", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-cloud-backup-settings",
            Label = "Disable Cloud Backup of Settings",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from backing up settings and preferences to the cloud. Default: enabled with Microsoft account.",
            Tags = ["backup", "cloud", "settings", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "backup-disable-auto-backup-scheduling",
            Label = "Disable Automatic Backup Scheduling",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the system from automatically scheduling File History backups. Manual backups still possible. Default: scheduled.",
            Tags = ["backup", "schedule", "automatic", "file-history"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "DisableAutoSchedule", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "DisableAutoSchedule")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory", "DisableAutoSchedule", 1)],
        },
        new TweakDef
        {
            Id = "backup-set-restore-point-frequency-1440",
            Label = "Set Restore Point Frequency to Daily",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits System Restore to create at most one restore point per day (1440 min). Prevents excessive disk usage. Default: no limit.",
            Tags = ["backup", "restore-point", "frequency", "daily"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore",
                    "SystemRestorePointCreationFrequency",
                    1440
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore",
                    "SystemRestorePointCreationFrequency"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore",
                    "SystemRestorePointCreationFrequency",
                    1440
                ),
            ],
        },
        new TweakDef
        {
            Id = "backup-disable-previous-versions-ui",
            Label = "Disable Previous Versions Tab",
            Category = "Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Previous Versions tab in file properties. Declutters the UI when VSS is not used. Default: visible.",
            Tags = ["backup", "previous-versions", "properties", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "NoPreviousVersionsPage", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-notifications",
            Label = "Disable Windows Backup Notifications",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Backup reminder notifications. Prevents periodic prompts to set up backup. Default: enabled.",
            Tags = ["backup", "notifications", "reminders", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableBackupUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client", "DisableBackupUI", 1)],
        },
    ];
}

// ── Merged from Recovery.cs ──────────────────────────────────────────────────

internal static class Recovery
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "recovery-disable-system-restore",
            Label = "Disable System Restore",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables System Restore on all drives via Group Policy. Saves disk space but removes rollback capability. Default: enabled.",
            Tags = ["recovery", "system-restore", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "recovery-limit-restore-disk-usage",
            Label = "Limit System Restore Disk Usage to 5%",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the disk space used by System Restore shadow copies to 5% of the drive. Saves space while preserving restore capability. Default: 10-15%.",
            Tags = ["recovery", "system-restore", "disk-space", "limit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
        },
        new TweakDef
        {
            Id = "recovery-increase-restore-frequency",
            Label = "System Restore: Daily Checkpoints",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets System Restore to create automatic restore points every 24 hours (86400 seconds). Default: 24 hours but may be skipped under load.",
            Tags = ["recovery", "system-restore", "checkpoint", "daily"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval", 86400),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 86400),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval", 86400),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-repair-prompt",
            Label = "Disable Automatic Repair at Boot",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic startup repair prompt after consecutive boot failures. Prevents boot loops on servers. Default: enabled.",
            Tags = ["recovery", "auto-repair", "boot", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-partition",
            Label = "Disable Windows Recovery Environment",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Recovery Environment (WinRE). Saves ~500 MB disk space but removes recovery boot option. Default: enabled.",
            Tags = ["recovery", "winre", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-problem-reports",
            Label = "Disable Problem Reports & Solutions",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic problem report generation and solution checks. Reduces background activity and telemetry. Default: enabled.",
            Tags = ["recovery", "problem-reports", "disable", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontShowUI"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-increase-max-shadow-copies",
            Label = "Increase Max Shadow Copy Storage",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the maximum number of shadow copy storage associations. Allows more restore points to be retained. Default: system-managed.",
            Tags = ["recovery", "shadow-copy", "restore-point", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
        },
        new TweakDef
        {
            Id = "recovery-enable-auto-recovery",
            Label = "Enable Automatic Recovery",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the Windows automatic recovery feature that detects startup failures and offers repair options. Default: enabled.",
            Tags = ["recovery", "auto-recovery", "startup", "repair"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 10)],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-restart-sign-on",
            Label = "Disable Auto-Restart Sign-On (ARSO)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic sign-on after restart/update. Prevents Windows from caching credentials and auto-logging in after reboot. Default: enabled.",
            Tags = ["recovery", "security", "arso", "sign-on", "restart"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-enable-last-known-good",
            Label = "Enable Last Known Good Configuration",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the system to report the last known good configuration to the boot manager. Helps recovery from bad driver installs. Default: system-managed.",
            Tags = ["recovery", "boot", "last-known-good", "driver"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-recovery-ui",
            Label = "Disable Windows Recovery UI",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the recovery UI displayed after consecutive boot failures. Use only on kiosk or server systems where manual intervention is not desired. Default: enabled.",
            Tags = ["recovery", "boot", "ui", "kiosk", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-crash-auto-reboot-timeout",
            Label = "Set Crash Reboot Timeout to 30s",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the timeout before automatic reboot after a crash to 30 seconds, giving time to read BSOD information. Default: immediate reboot.",
            Tags = ["recovery", "crash", "bsod", "timeout", "reboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
        },
        new TweakDef
        {
            Id = "recovery-enable-event-log-crash",
            Label = "Write Crash Events to System Log",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures crash/BSOD events are written to the Windows Event Log. Essential for post-crash diagnostics. Default: enabled.",
            Tags = ["recovery", "crash", "event-log", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-send-alert",
            Label = "Disable Admin Alert on Crash",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sending an administrative alert when a crash occurs. Reduces noise on standalone systems. Default: off.",
            Tags = ["recovery", "crash", "alert", "notification"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-automatic-managed-page-file",
            Label = "Disable Auto-Managed Page File",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic page file management. Allows manual control over page file size for advanced crash dump configuration. Default: auto-managed.",
            Tags = ["recovery", "page-file", "memory", "crash-dump", "advanced"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0),
            ],
        },
        // ── Sprint 18 — 10 new Recovery tweaks ────────────────────────────

        new TweakDef
        {
            Id = "recovery-enable-boot-logging",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows boot logging to %SystemRoot%\\ntbtlog.txt. Useful for diagnosing driver loading failures. Default: disabled.",
            Tags = ["recovery", "boot", "logging", "driver", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-auto-repair",
            Label = "Disable Automatic Repair (Windows RE)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic startup repair in Windows Recovery Environment. Prevents boot loops when auto-repair repeatedly fails. Default: enabled.",
            Tags = ["recovery", "winre", "auto-repair", "boot-loop", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair", 0)],
        },
        new TweakDef
        {
            Id = "recovery-set-dump-folder-path",
            Label = "Set Minidump Folder to C:\\Minidumps",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Redirects minidump files to C:\\Minidumps for easier collection and analysis. Default: %SystemRoot%\\Minidump.",
            Tags = ["recovery", "minidump", "folder", "path", "organisation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"C:\Minidumps")],
            RemoveOps =
            [
                RegOp.SetExpandString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"%SystemRoot%\Minidump"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"C:\Minidumps")],
        },
        new TweakDef
        {
            Id = "recovery-increase-crash-dump-count",
            Label = "Keep Last 50 Minidump Files",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the number of retained minidump files from 10 (default) to 50 for longer crash history. Default: 10 files.",
            Tags = ["recovery", "minidump", "retention", "crash-history", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount", 50)],
        },
        new TweakDef
        {
            Id = "recovery-disable-startup-repair-prompt",
            Label = "Disable Startup Repair Recommendation Prompt",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses the 'Your PC did not start correctly' startup repair recommendation. Default: prompt shown after improper shutdown.",
            Tags = ["recovery", "startup-repair", "prompt", "boot", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
        },
        new TweakDef
        {
            Id = "recovery-enable-system-failure-popup",
            Label = "Show Popup on System Failure",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Displays an alert dialog when a system failure occurs, rather than silently restarting. Helpful for attended servers. Default: no popup.",
            Tags = ["recovery", "system-failure", "popup", "alert", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 1)],
        },
    ];
}

// ── Merged from SystemRestore.cs ──────────────────────────────────────────────────

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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Id = "restore-disable-wer-logging",
            Label = "Disable Windows Error Reporting Logging",
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of local crash dump files retained per application to 3.",
            Tags = ["restore", "wer", "crash", "dump", "disk-space", "limit"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 3)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 3)],
        },
        new TweakDef
        {
            Id = "restore-disable-hiberfil",
            Label = "Disable Hibernate File (Reclaim Disk Space)",
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets a 20-second timeout for the WER crash report server to reduce startup delays on slow networks.",
            Tags = ["restore", "wer", "timeout", "performance", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs", 20)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "ResponseTimeoutSecs", 20)],
        },
        new TweakDef
        {
            Id = "restore-disable-bsod-alert-send",
            Label = "Disable BSOD Administrator Alert",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from sending a crash alert notification to connected network administrators on BSOD.",
            Tags = ["restore", "bsod", "alert", "notification", "network", "admin"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
    ];
}

internal static class CloudStorage
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cloud-disable-dropbox-autostart",
            Label = "Disable Dropbox Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Dropbox from starting automatically at login.",
            Tags = ["dropbox", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Dropbox"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "DropboxUpdate"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableAutoStart")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Dropbox")],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-update",
            Label = "Disable Dropbox Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Dropbox from automatically checking for and installing updates.",
            Tags = ["dropbox", "update", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-lan-sync",
            Label = "Disable Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Dropbox LAN Sync (peer-to-peer discovery on the local network). Reduces network chatter and improves privacy on shared networks.",
            Tags = ["dropbox", "lan", "network", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-autostart",
            Label = "Disable Google Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Google Drive (DriveFS) from starting at login.",
            Tags = ["gdrive", "google", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableAutoStart")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-update",
            Label = "Disable Google Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Google Drive from auto-updating via policy.",
            Tags = ["gdrive", "google", "update", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-gdrive-bandwidth-limit",
            Label = "Limit Google Drive Upload (1 MB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps Google Drive upload bandwidth at 1 MB/s to prevent saturating your internet connection during large syncs.",
            Tags = ["gdrive", "google", "bandwidth", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthRxKBPS", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS", 1024),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthRxKBPS"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS", 1024)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-autostart",
            Label = "Disable iCloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents iCloud Drive and iCloud Services from starting at login.",
            Tags = ["icloud", "apple", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudDrive"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudServices"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudDrive")],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photos",
            Label = "Disable iCloud Photo Stream Upload",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic photo stream uploads via iCloud for Windows.",
            Tags = ["icloud", "apple", "photos", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-autostart",
            Label = "Disable Box Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Box / Box Drive from starting automatically at login.",
            Tags = ["box", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Box"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "BoxDrive"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Box")],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-autostart",
            Label = "Disable MEGA Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGAsync from starting automatically at login.",
            Tags = ["mega", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
        },
        new TweakDef
        {
            Id = "cloud-disable-pcloud-autostart",
            Label = "Disable pCloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents pCloud Drive from starting automatically at login.",
            Tags = ["pcloud", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive")],
        },
        new TweakDef
        {
            Id = "cloud-disable-nextcloud-autostart",
            Label = "Disable Nextcloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Nextcloud desktop client from starting at login.",
            Tags = ["nextcloud", "autostart", "cloud", "opensource"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud")],
        },
        new TweakDef
        {
            Id = "cloud-disable-tresorit-autostart",
            Label = "Disable Tresorit Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Tresorit from starting automatically at login.",
            Tags = ["tresorit", "autostart", "cloud", "encrypted"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit")],
        },
        new TweakDef
        {
            Id = "cloud-disable-synccom-autostart",
            Label = "Disable Sync.com Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Sync.com desktop client from starting at login.",
            Tags = ["sync.com", "autostart", "cloud", "encrypted"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com")],
        },
        new TweakDef
        {
            Id = "cloud-disable-spideroak-autostart",
            Label = "Disable SpiderOak ONE Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents SpiderOak ONE backup from starting at login.",
            Tags = ["spideroak", "autostart", "cloud", "backup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE")],
        },
        new TweakDef
        {
            Id = "cloud-disable-amazondrive-autostart",
            Label = "Disable Amazon Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Amazon Drive from starting automatically at login.",
            Tags = ["amazon", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive")],
        },
        new TweakDef
        {
            Id = "cloud-dropbox-upload-throttle",
            Label = "Throttle Dropbox Upload (512 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Caps Dropbox upload bandwidth at 512 KB/s to prevent saturating your internet connection.",
            Tags = ["dropbox", "bandwidth", "throttle", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate", 512),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_style", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_style"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate", 512)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-telemetry",
            Label = "Disable Dropbox Telemetry",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Dropbox analytics and telemetry data collection.",
            Tags = ["dropbox", "telemetry", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1)],
        },
        new TweakDef
        {
            Id = "cloud-gdrive-cache-limit",
            Label = "Limit Google Drive Cache (10 GB)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the Google Drive File Stream local cache at 10 GB to recover disk space on smaller SSDs.",
            Tags = ["gdrive", "google", "cache", "disk", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-telemetry",
            Label = "Disable Google Drive Telemetry",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables crash reporting and usage stats for Google Drive.",
            Tags = ["gdrive", "google", "telemetry", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableUsageStats", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableUsageStats"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-update",
            Label = "Disable MEGA Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGAsync from automatically checking for updates.",
            Tags = ["mega", "update", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-update",
            Label = "Disable Box Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Box Drive from automatically installing updates.",
            Tags = ["box", "update", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Box\Box"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-drive",
            Label = "Disable iCloud Drive Integration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables iCloud Drive Windows integration. Prevents iCloud from syncing files in Explorer. Default: Enabled. Recommended: Disabled if not using Apple devices.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync",
            Label = "Disable iCloud Auto-Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables iCloud automatic synchronization via Group Policy. Default: Enabled. Recommended: Disabled if not using Apple services.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-creative-cloud-startup",
            Label = "Disable Adobe Creative Cloud Startup",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Adobe Creative Cloud startup sync via policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["cloud", "adobe", "creative-cloud", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photo-sync",
            Label = "Disable iCloud Photo Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables iCloud Photo Stream automatic upload to prevent photos from syncing to Apple cloud services. Default: enabled. Recommended: disabled on corporate machines.",
            Tags = ["cloud", "icloud", "photo", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-offline",
            Label = "Disable Google Drive Offline Mode",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Google Drive offline mode via policy. Prevents local caching of Drive files, reducing disk usage. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "google-drive", "offline", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
        },
        new TweakDef
        {
            Id = "cloud-block-dropbox-lan-sync",
            Label = "Block Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks Dropbox LAN sync discovery which broadcasts on the local network. Improves security on shared networks. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "dropbox", "lan", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-onedrive-files-on-demand",
            Label = "Disable OneDrive Files On-Demand Policy",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On-Demand feature via Group Policy. All files download fully. Default: on-demand.",
            Tags = ["cloud", "onedrive", "files-on-demand", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-google-drive-autostart",
            Label = "Disable Google Drive Autostart",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Google Drive from starting automatically at logon. Default: starts with Windows.",
            Tags = ["cloud", "google-drive", "startup", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "GoogleDriveFS",
                    "\"C:\\Program Files\\Google\\Drive File Stream\\launch.bat\""
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-auto-update",
            Label = "Disable Box Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Box Drive from automatically checking for updates. Default: auto-update enabled.",
            Tags = ["cloud", "box", "update", "auto-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-sync-autostart",
            Label = "Disable MEGA Sync Autostart",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGA Sync client from starting automatically at logon. Default: starts with Windows.",
            Tags = ["cloud", "mega", "sync", "startup", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "MEGAsync",
                    "\"C:\\Users\\%USERNAME%\\AppData\\Local\\MEGAsync\\MEGAsync.exe\""
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync-on-startup",
            Label = "Disable iCloud Sync on Startup",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Apple iCloud from starting automatically at login. Saves bandwidth and resources. Default: enabled.",
            Tags = ["cloud", "icloud", "sync", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "iCloudServices",
                    @"%ProgramFiles%\Common Files\Apple\Internet Services\iCloudServices.exe"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
        },
        new TweakDef
        {
            Id = "cloud-disable-suggestions",
            Label = "Disable Cloud Storage Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows suggestions to use cloud storage services. Prevents Microsoft account and OneDrive promotions. Default: enabled.",
            Tags = ["cloud", "suggestions", "promotions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "cloud-overlay-optimise",
            Label = "Optimise Cloud Sync Overlay Icons",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables cloud-optimized content delivery from Windows. Reduces background data usage and telemetry from cloud storage features. Default: enabled.",
            Tags = ["cloud", "overlay", "sync", "optimise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
        },
    ];
}

// ── Merged from CloudExperience.cs ──────────────────────────────────────────────────

internal static class CloudExperience
{
    private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE";

    private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string ContentDelivery = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";

    private const string WindowsUpdate = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    private const string OobeUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\OOBE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "oobe-disable-consumer-features",
            Label = "Disable Consumer Cloud Features and Spotlight Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "consumer", "cloud", "suggestions", "privacy"],
            Description =
                "Disables Windows consumer features such as Microsoft Spotlight "
                + "advertisements and app suggestions delivered through cloud content. "
                + "DisableWindowsConsumerFeatures=1.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-ads",
            Label = "Disable Lock Screen Spotlight Ads (Enterprise)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "ads", "policy"],
            Description =
                "Disables Windows Spotlight on the lock screen via the cloud content "
                + "policy key (DisableWindowsSpotlightFeatures=1). Prevents Microsoft "
                + "from rotating lock screen images and showing tips and ads.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-onboarding",
            Label = "Disable Post-OOBE Cloud Onboarding Prompts",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "onboarding", "cloud", "prompts", "enterprise"],
            Description =
                "Disables the post-login onboarding flow that invites users to connect "
                + "OneDrive, set up Microsoft 365, etc. Suitable for pre-imaged enterprise "
                + "deployments. SkipNotHerePrompts=1.",
            ApplyOps = [RegOp.SetDword(Oobe, "SkipNotHerePrompts", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "SkipNotHerePrompts")],
            DetectOps = [RegOp.CheckDword(Oobe, "SkipNotHerePrompts", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-silent-app-install",
            Label = "Disable Silent Background App Installation via CDM",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "silent install", "apps", "consumer"],
            Description =
                "Prevents the Content Delivery Manager from silently installing "
                + "suggested and sponsored apps in the background. "
                + "SilentInstalledAppsEnabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-tips",
            Label = "Disable OOBE and Start Tips (Welcome Messages)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "tips", "welcome", "onboarding"],
            Description =
                "Disables the 'Did you know' and welcome tips in the Start menu and "
                + "after Windows updates. SoftLandingEnabled=0 in Content Delivery Manager.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-roaming-profile-setup",
            Label = "Disable Roaming Profile Setup Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "roaming", "profile", "onedrive"],
            Description =
                "Suppresses the prompt to back up the Desktop, Documents, and Pictures "
                + "folders to OneDrive during OOBE. DesktopIconsPreference=1 "
                + "(keep local folders).",
            ApplyOps = [RegOp.SetDword(Oobe, "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(Oobe, "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-on-desktop",
            Label = "Disable Spotlight Wallpaper Rotation on Desktop",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "wallpaper", "desktop", "cloud"],
            Description =
                "Prevents Windows Spotlight from rotating the desktop wallpaper. "
                + "RotatingLockScreenEnabled=0. Keeps a fixed wallpaper instead of "
                + "Microsoft's rotating Bing images.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscription-content",
            Label = "Disable Subscription-Based Cloud Content in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "subscription", "content", "start menu", "ads"],
            Description =
                "Disables subscription-based recommended and promoted content in the "
                + "Start menu. SubscribedContent-338388Enabled=0. Removes the "
                + "'Get the most out of Windows' suggestions.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions in Start and Store",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "third party", "suggestions", "start", "ads"],
            Description =
                "Disables third-party sponsored app suggestions in the Start menu and " + "Microsoft Store. SubscribedContent-338389Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-welcome-experience",
            Label = "Disable 'What's New' Welcome Experience After Updates",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "welcome", "what's new", "update", "cloud"],
            Description =
                "Prevents Windows from showing the 'What's new in Windows' splash screen "
                + "after feature updates complete. ContentDelivery SubscribedContent-310093Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-oem-preinstalled-apps",
            Label = "Disable OEM Pre-Installed Application Install",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "oem", "preinstalled", "apps", "bloatware"],
            Description =
                "Disables the silent installation of OEM-branded applications delivered "
                + "through the Content Delivery Manager (OemPreInstalledAppsEnabled=0). "
                + "Prevents hardware vendors from adding apps post-setup.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-pre-installed-apps",
            Label = "Disable Pre-Installed App Install via ContentDelivery",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "preinstalled", "apps", "curation", "bloatware"],
            Description =
                "Disables automatic installation of curated pre-installed Windows apps "
                + "delivered via the Content Delivery Manager (PreInstalledAppsEnabled=0). "
                + "Reduces initial bloatware on clean installs.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-soft-landing",
            Label = "Disable Start Layout Soft-Landing Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "soft-landing", "layout"],
            Description =
                "Disables 'soft-landing' content — clickable tips and suggestions injected "
                + "into the Start menu and notification area after first login "
                + "(SoftLandingEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-start-system-pane-suggestions",
            Label = "Disable System Pane Suggestions in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "system pane", "ui"],
            Description =
                "Disables the rotating suggested app tiles displayed in the Windows Start " + "menu system pane (SystemPaneSuggestionsEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-content-delivery",
            Label = "Disable Content Delivery Manager (All CDM Sources)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "content delivery", "suggestions", "apps"],
            Description =
                "Master switch that disables all Content Delivery Manager activity "
                + "by setting ContentDeliveryAllowed=0. Prevents all app suggestions, "
                + "spotlight ads, and cloud-delivered content from being displayed.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338388",
            Label = "Disable Lock Screen Spotlight (SubscribedContent-338388)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "windows", "ads"],
            Description =
                "Disables Windows Spotlight on the lock screen by setting "
                + "SubscribedContent-338388Enabled=0 in the user's Content Delivery "
                + "Manager keys. Falls back to static wallpaper.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338389",
            Label = "Disable Lock Screen Spotlight Tips (SubscribedContent-338389)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "tips", "ads"],
            Description =
                "Disables the rotating lock screen tips/suggestions from Windows Spotlight "
                + "(SubscribedContent-338389Enabled=0). Stops Microsoft from delivering "
                + "promotional messages on the lock screen.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353694",
            Label = "Disable Start Menu App Suggestions (SubscribedContent-353694)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "apps", "ads"],
            Description =
                "Disables the 'Occasionally show suggestions in Start' setting "
                + "(SubscribedContent-353694Enabled=0). Stops ad tiles appearing "
                + "in the Start menu recommendations.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353696",
            Label = "Disable Timeline Suggested Content (SubscribedContent-353696)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "timeline", "suggestions", "content", "ads"],
            Description =
                "Disables cloud-delivered suggested activities in the Windows Timeline "
                + "and 'Recommended' section (SubscribedContent-353696Enabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimized Content in Settings",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "cloud", "content", "settings", "policy"],
            Description =
                "Prevents Windows from showing cloud-optimized content — rotating "
                + "Microsoft-curated links and suggestions — inside the Settings app "
                + "(DisableCloudOptimizedContent=1).",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableCloudOptimizedContent", 1)],
        },
    ];
}

// === Merged from: OneDrive.cs ===

internal static class OneDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "od-onedrive-upload-throttle",
            Label = "Throttle OneDrive Upload (1 MB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 1000 KB/s to prevent saturating your connection.",
            Tags = ["onedrive", "bandwidth", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal",
            Label = "Disable OneDrive Personal Account Sign-In",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from signing in with a personal Microsoft account in OneDrive.",
            Tags = ["onedrive", "personal", "signin", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-max-upload-rate",
            Label = "Limit OneDrive Upload Rate (125 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps OneDrive upload bandwidth at 125 KB/s to minimise network impact.",
            Tags = ["onedrive", "bandwidth", "upload", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125)],
        },
        new TweakDef
        {
            Id = "od-onedrive-max-download-rate",
            Label = "Limit OneDrive Download Rate (1000 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps OneDrive download bandwidth at 1000 KB/s to minimise network impact.",
            Tags = ["onedrive", "bandwidth", "download", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-office-collab",
            Label = "Disable Office Collaboration via OneDrive",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Office co-authoring feature that uses OneDrive sync.",
            Tags = ["onedrive", "office", "collaboration", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-silent-config",
            Label = "Enable Silent OneDrive Account Configuration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Silently configures OneDrive with the user's Windows credentials without prompts.",
            Tags = ["onedrive", "silent", "config", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-reduce-bandwidth",
            Label = "OneDrive Reduce Sync Traffic",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits OneDrive upload bandwidth to 50%. Prevents OneDrive from saturating network connection. Default: Unlimited. Recommended: 50% for shared networks.",
            Tags = ["onedrive", "bandwidth", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-fod-global",
            Label = "Disable OneDrive Files On-Demand (Global)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables OneDrive Files On-Demand globally via policy. Forces all files to be downloaded locally. Default: Enabled. Recommended: Disabled for offline use.",
            Tags = ["onedrive", "files-on-demand", "policy", "offline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal-sync",
            Label = "Disable Personal OneDrive Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables personal OneDrive file sync via DisableFileSyncNGSC policy. Prevents OneDrive from syncing any personal accounts. Default: Enabled. Recommended: Disabled on corporate machines.",
            Tags = ["onedrive", "sync", "personal", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-backup-prompt",
            Label = "Disable OneDrive Folder Backup Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks the OneDrive Known Folder Move (KFM) opt-in prompt that asks users to back up Desktop, Documents, and Pictures. Default: Enabled. Recommended: Disabled on managed machines.",
            Tags = ["onedrive", "backup", "kfm", "prompt"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-block-business-sync",
            Label = "Block OneDrive for Business Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks OneDrive from syncing with external or business SharePoint organizations. Default: Allowed. Recommended: Blocked on personal machines.",
            Tags = ["onedrive", "business", "sync", "external"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-collaboration",
            Label = "Disable OneDrive File Collaboration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables OneDrive OCSI co-authoring clients for real-time file collaboration. Default: Enabled. Recommended: Disabled for offline-only workflows.",
            Tags = ["onedrive", "collaboration", "coauthoring", "ocsi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal",
            Label = "Disable OneDrive Personal Account Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from adding personal OneDrive accounts. Only business accounts allowed. Default: allowed.",
            Tags = ["onedrive", "personal", "account", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-limit-upload-bandwidth",
            Label = "Limit OneDrive Upload Bandwidth to 512 KB/s",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 512 KB/s. Prevents OneDrive from saturating the network. Default: unlimited.",
            Tags = ["onedrive", "bandwidth", "upload", "throttle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 512)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 512)],
        },
        new TweakDef
        {
            Id = "od-disable-toast-notifications",
            Label = "Disable OneDrive Toast Notifications",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive sync error and activity toast notifications. Default: enabled.",
            Tags = ["onedrive", "notifications", "toast", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "od-block-external-sync",
            Label = "Block External OneDrive Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive from syncing with external organizations. Data stays in-org. Default: allowed.",
            Tags = ["onedrive", "external", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-set-max-file-size-5gb",
            Label = "Set OneDrive Max Upload File Size to 5 GB",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the maximum file size OneDrive will sync to 5 GB. Default: no limit.",
            Tags = ["onedrive", "file-size", "limit", "upload"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB", 5120)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB", 5120)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-ads",
            Label = "Disable OneDrive Ads & Promotions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables promotional ads and tips in OneDrive sync client. Prevents upgrade nag popups. Default: enabled.",
            Tags = ["onedrive", "ads", "promotions", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-autostart",
            Label = "Disable OneDrive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents OneDrive from automatically starting on Windows login. Frees memory and network bandwidth. Default: auto-starts.",
            Tags = ["onedrive", "autostart", "startup", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "OneDrive")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "OneDrive",
                    @"%LOCALAPPDATA%\Microsoft\OneDrive\OneDrive.exe /background"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "OneDrive")],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-fod",
            Label = "Disable OneDrive Files On Demand",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On Demand feature enterprise-wide via policy. All files are kept local. Default: enabled.",
            Tags = ["onedrive", "files-on-demand", "policy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-kfm",
            Label = "Disable OneDrive Known Folder Move",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks OneDrive from silently moving Desktop, Documents, Pictures to cloud. Prevents automatic folder redirection. Default: allowed.",
            Tags = ["onedrive", "kfm", "folder-move", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal-sync",
            Label = "Block OneDrive Personal Account Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive from syncing personal (non-work) accounts via policy. Only enterprise tenants allowed. Default: allowed.",
            Tags = ["onedrive", "personal", "sync", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-files-on-demand",
            Label = "Disable OneDrive Files On Demand (User)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables OneDrive Files On Demand at user level. Files are always downloaded fully. Avoids placeholder files. Default: enabled.",
            Tags = ["onedrive", "files-on-demand", "user", "local"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-kfm-opt-in-prompt",
            Label = "Block Known Folder Move Opt-In Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from prompting users to move Desktop/Documents/Pictures to OneDrive (Known Folder Move wizard).",
            Tags = ["onedrive", "known-folder-move", "kfm", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-kfm-silent-redirect",
            Label = "Block Silent Known Folder Redirect",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks OneDrive from silently redirecting known folders (Desktop, Documents, Pictures) to cloud storage without prompting.",
            Tags = ["onedrive", "known-folder-move", "kfm", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-delay-update-ring",
            Label = "Delay OneDrive Client Update Ring",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keeps the OneDrive sync client on the Deferred ring to avoid destabilising updates.",
            Tags = ["onedrive", "update", "ring", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-sharepoint-sync",
            Label = "Disable SharePoint Sync Library",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from syncing SharePoint document libraries to the local machine.",
            Tags = ["onedrive", "sharepoint", "sync", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-app-sync",
            Label = "Disable OneDrive Application Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from syncing application settings (AppData\\Roaming) to cloud storage.",
            Tags = ["onedrive", "appsync", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync", 1)],
        },
        new TweakDef
        {
            Id = "od-limit-mass-delete-threshold",
            Label = "OneDrive Mass-Delete Warning Threshold (50 files)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the threshold at which OneDrive warns before deleting large numbers of files to 50 (down from default of 200).",
            Tags = ["onedrive", "mass-delete", "safety", "warning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold", 50)],
        },
        new TweakDef
        {
            Id = "od-disable-hydration-on-access",
            Label = "Disable Auto-Hydration on File Access",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents OneDrive from automatically downloading cloud-only placeholder files when opened by an app. Avoids unexpected bandwidth usage.",
            Tags = ["onedrive", "hydration", "files-on-demand", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-auto-update",
            Label = "Disable OneDrive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents OneDrive client from auto-updating in the background. Useful in managed environments where updates are controlled centrally.",
            Tags = ["onedrive", "update", "autoupdate", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-file-explorer-hub",
            Label = "Remove OneDrive from File Explorer Left Panel",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the OneDrive folder entry from the File Explorer navigation pane without disabling the sync process.",
            Tags = ["onedrive", "explorer", "sidebar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "od-block-external-collab",
            Label = "Block OneDrive External Collaboration (Policy)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from sharing OneDrive files with users outside of the organisation via Group Policy.",
            Tags = ["onedrive", "external-sharing", "collaboration", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing", 1)],
        },
    ];
}

