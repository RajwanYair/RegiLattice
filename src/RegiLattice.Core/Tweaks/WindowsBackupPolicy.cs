#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class WindowsBackupPolicy
{
    private const string BackupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup";
    private const string ClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "backup-disable-backup",
            Label = "Disable Windows Backup",
            Category = "Windows Backup Policy",
            Description = "Disables the Windows Backup feature and prevents users from initiating backups through the control panel.",
            Tags = ["backup", "windows-backup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Windows Backup is disabled; use third-party or enterprise backup solutions instead.",
            ApplyOps = [RegOp.SetDword(BackupKey, "DisableBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableBackup")],
            DetectOps = [RegOp.CheckDword(BackupKey, "DisableBackup", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-restore",
            Label = "Disable Windows Backup Restore",
            Category = "Windows Backup Policy",
            Description = "Prevents users from using the Windows Backup restore feature to recover files or system state.",
            Tags = ["backup", "restore", "windows-backup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Restore via Windows Backup UI is blocked; enterprise recovery tools still function.",
            ApplyOps = [RegOp.SetDword(BackupKey, "DisableRestore", 1)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableRestore")],
            DetectOps = [RegOp.CheckDword(BackupKey, "DisableRestore", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-catalog-viewer",
            Label = "Disable Windows Backup Catalog Viewer",
            Category = "Windows Backup Policy",
            Description = "Removes access to the Windows Backup catalog viewer preventing browsing of historical backup sets.",
            Tags = ["backup", "catalog", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Catalog browser is hidden from users; backup files on disk are unaffected.",
            ApplyOps = [RegOp.SetDword(BackupKey, "DisableCatalogViewer", 1)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableCatalogViewer")],
            DetectOps = [RegOp.CheckDword(BackupKey, "DisableCatalogViewer", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-system-backup",
            Label = "Disable Windows System Backup",
            Category = "Windows Backup Policy",
            Description = "Prevents users from creating system image or system files backups through the Windows Backup UI.",
            Tags = ["backup", "system-backup", "image", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "System image creation is blocked; critical for environments using enterprise imaging solutions.",
            ApplyOps = [RegOp.SetDword(ClientKey, "NoBackupSysFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "NoBackupSysFiles")],
            DetectOps = [RegOp.CheckDword(ClientKey, "NoBackupSysFiles", 1)],
        },
        new TweakDef
        {
            Id = "backup-suppress-backup-progress-ui",
            Label = "Suppress Windows Backup Progress Dialog",
            Category = "Windows Backup Policy",
            Description = "Hides the backup progress window and toast notifications that appear during Windows Backup operations.",
            Tags = ["backup", "ui", "progress", "notifications", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Silent backup mode; no visible progress indicator; check event logs to verify backup completion.",
            ApplyOps = [RegOp.SetDword(ClientKey, "NoProgressUI", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "NoProgressUI")],
            DetectOps = [RegOp.CheckDword(ClientKey, "NoProgressUI", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-online-backup",
            Label = "Disable Online Backup Services Integration",
            Category = "Windows Backup Policy",
            Description = "Removes the online backup provider options from the Windows Backup configuration wizard.",
            Tags = ["backup", "online", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cloud backup provider options are removed from the UI; local backup to drives still available.",
            ApplyOps = [RegOp.SetDword(ClientKey, "NoOnlineBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "NoOnlineBackup")],
            DetectOps = [RegOp.CheckDword(ClientKey, "NoOnlineBackup", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-network-backup",
            Label = "Disable Backup to Network Locations",
            Category = "Windows Backup Policy",
            Description = "Blocks Windows Backup from saving backup sets to network shares or mapped drives.",
            Tags = ["backup", "network", "share", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents backup data exfiltration to network shares; local drives only for Windows Backup.",
            ApplyOps = [RegOp.SetDword(ClientKey, "NoNetworkBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "NoNetworkBackup")],
            DetectOps = [RegOp.CheckDword(ClientKey, "NoNetworkBackup", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-backup-over-metered",
            Label = "Disable Windows Backup on Metered Connections",
            Category = "Windows Backup Policy",
            Description = "Prevents Windows Backup from running over metered (pay-per-use) network connections.",
            Tags = ["backup", "metered", "network", "data-usage", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Backup paused on metered connections; resumes automatically on unmetered networks.",
            ApplyOps = [RegOp.SetDword(ClientKey, "DisableBackupOnMeteredConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableBackupOnMeteredConnections")],
            DetectOps = [RegOp.CheckDword(ClientKey, "DisableBackupOnMeteredConnections", 1)],
        },
        new TweakDef
        {
            Id = "backup-disable-scheduled-backup",
            Label = "Disable Scheduled Windows Backup",
            Category = "Windows Backup Policy",
            Description = "Prevents Windows from running scheduled background backups automatically on a configured schedule.",
            Tags = ["backup", "scheduled", "task", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Automatic scheduled backups are disabled; manual backup invocation still works unless DisableBackup is also set.",
            ApplyOps = [RegOp.SetDword(ClientKey, "NoScheduledBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "NoScheduledBackup")],
            DetectOps = [RegOp.CheckDword(ClientKey, "NoScheduledBackup", 1)],
        },
        new TweakDef
        {
            Id = "backup-hide-control-panel-link",
            Label = "Hide Windows Backup Control Panel Link",
            Category = "Windows Backup Policy",
            Description = "Removes the Windows Backup entry from the Control Panel and System & Security settings page.",
            Tags = ["backup", "control-panel", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Backup settings UI is hidden; the underlying feature may still be invoked by command line or scripts.",
            ApplyOps = [RegOp.SetDword(ClientKey, "HideControlPanelLink", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "HideControlPanelLink")],
            DetectOps = [RegOp.CheckDword(ClientKey, "HideControlPanelLink", 1)],
        },
    ];
}
