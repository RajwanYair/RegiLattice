namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class Backup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "backup-disable-file-history",
            Label = "Disable File History",
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Id = "backup-disable-reliability-monitor",
            Label = "Disable Reliability Monitoring",
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Id = "backup-wer-reduce-queue",
            Label = "Limit Windows Error Reporting Queue to 1",
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
            Category = "Storage — Work Folders",
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
