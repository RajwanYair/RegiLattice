namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Backup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "backup-disable-file-history",
            Label = "Disable File History",
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
            Category = "Backup & Recovery",
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
