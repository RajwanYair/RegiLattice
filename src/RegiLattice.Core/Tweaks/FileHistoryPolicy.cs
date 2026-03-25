#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 243 — File History & Backup Client Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\FileHistory
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\Backup\Client
internal static class FileHistoryPolicy
{
    private const string FhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory";
    private const string BkpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fhp-disable-file-history",
            Label = "Disable File History",
            Category = "File History Policy",
            Description =
                "Sets Disabled=1 in the File History policy key. "
                + "Turns off the File History backup service for all users on this machine. "
                + "The File History control panel applet will display the feature as disabled by policy. "
                + "Default: absent (File History available to users). Recommended: 1 on server or managed deployments using alternative backup solutions.",
            Tags = ["file-history", "backup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Disables File History; users lose automatic versioned file backup unless an alternative is configured.",
            ApplyOps = [RegOp.SetDword(FhKey, "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(FhKey, "Disabled")],
            DetectOps = [RegOp.CheckDword(FhKey, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "fhp-lock-onoff-switch",
            Label = "Lock File History On/Off Switch",
            Category = "File History Policy",
            Description =
                "Sets OnOffSwitchLocked=1 in the File History policy key. "
                + "Prevents users from enabling or disabling File History via the Control Panel or Settings. "
                + "The toggle is greyed out and displays 'Some settings are managed by your organization'. "
                + "Default: absent. Recommended: 1 when File History state must be centrally controlled.",
            Tags = ["file-history", "backup", "restriction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents users from toggling File History; does not change its enabled/disabled state.",
            ApplyOps = [RegOp.SetDword(FhKey, "OnOffSwitchLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(FhKey, "OnOffSwitchLocked")],
            DetectOps = [RegOp.CheckDword(FhKey, "OnOffSwitchLocked", 1)],
        },
        new TweakDef
        {
            Id = "fhp-backup-interval-daily",
            Label = "Set File History Backup Interval to Daily",
            Category = "File History Policy",
            Description =
                "Sets BackupInterval=86400 (24 hours in seconds) in the File History policy key. "
                + "Controls how frequently File History backs up changed files. "
                + "Default: 3600 (hourly). Recommended: 86400 on systems with large user profiles or limited backup storage.",
            Tags = ["file-history", "backup", "interval", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces File History backup frequency to once daily; decreases I/O and storage consumption.",
            ApplyOps = [RegOp.SetDword(FhKey, "BackupInterval", 86400)],
            RemoveOps = [RegOp.DeleteValue(FhKey, "BackupInterval")],
            DetectOps = [RegOp.CheckDword(FhKey, "BackupInterval", 86400)],
        },
        new TweakDef
        {
            Id = "fhp-retention-until-space-needed",
            Label = "File History: Keep Until Space Is Needed",
            Category = "File History Policy",
            Description =
                "Sets RetentionPolicy=1 in the File History policy key. "
                + "Configures File History to retain backup copies until the drive runs low on space, "
                + "at which point older versions are automatically removed. "
                + "Default: absent (user-configured). Recommended: 1 for space-constrained backup targets.",
            Tags = ["file-history", "backup", "retention", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Oldest backup versions are deleted automatically when backup drive space runs low.",
            ApplyOps = [RegOp.SetDword(FhKey, "RetentionPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(FhKey, "RetentionPolicy")],
            DetectOps = [RegOp.CheckDword(FhKey, "RetentionPolicy", 1)],
        },
        new TweakDef
        {
            Id = "fhp-retention-one-month",
            Label = "File History: Keep Versions for 1 Month",
            Category = "File History Policy",
            Description =
                "Sets RetentionPolicy=2 and RetentionTime=1 in the File History policy key. "
                + "Configures File History to keep only backup copies made within the past month; older versions are purged automatically. "
                + "Default: absent (keep forever). Recommended: on systems where 1-month recovery window is sufficient.",
            Tags = ["file-history", "backup", "retention", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Limits backup history to 1 month, freeing backup drive space over time.",
            ApplyOps = [RegOp.SetDword(FhKey, "RetentionPolicy", 2), RegOp.SetDword(FhKey, "RetentionTime", 1)],
            RemoveOps = [RegOp.DeleteValue(FhKey, "RetentionPolicy"), RegOp.DeleteValue(FhKey, "RetentionTime")],
            DetectOps = [RegOp.CheckDword(FhKey, "RetentionPolicy", 2)],
        },
        new TweakDef
        {
            Id = "fhp-prevent-data-degradation",
            Label = "Prevent File History Data Degradation",
            Category = "File History Policy",
            Description =
                "Sets DataDegradationPolicy=1 in the File History policy key. "
                + "Causes File History to stop backing up if the protection level would fall due to cache issues, "
                + "rather than silently continuing with degraded coverage. "
                + "Default: absent (degraded backup is allowed). Recommended: 1 to ensure backup integrity or alert on problems.",
            Tags = ["file-history", "backup", "integrity", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "File History halts if backup data integrity would be compromised rather than silently degrading.",
            ApplyOps = [RegOp.SetDword(FhKey, "DataDegradationPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(FhKey, "DataDegradationPolicy")],
            DetectOps = [RegOp.CheckDword(FhKey, "DataDegradationPolicy", 1)],
        },
        new TweakDef
        {
            Id = "fhp-disable-file-backup",
            Label = "Disable Windows Backup File Backup",
            Category = "File History Policy",
            Description =
                "Sets DisableFileBackup=1 in the Windows Backup client policy key. "
                + "Prevents users from performing file-level backups using the Windows Backup client. "
                + "The backup option is hidden from the Backup and Restore Control Panel applet. "
                + "Default: absent. Recommended: 1 when an enterprise backup solution replaces Windows Backup.",
            Tags = ["backup", "file-backup", "policy", "restriction"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Removes the Windows Backup file-backup option; enterprise backup tools are unaffected.",
            ApplyOps = [RegOp.SetDword(BkpKey, "DisableFileBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableFileBackup")],
            DetectOps = [RegOp.CheckDword(BkpKey, "DisableFileBackup", 1)],
        },
        new TweakDef
        {
            Id = "fhp-disable-system-backup",
            Label = "Disable Windows Backup System (Image) Backup",
            Category = "File History Policy",
            Description =
                "Sets DisableSystemBackup=1 in the Windows Backup client policy key. "
                + "Prevents users from creating system image backups using the Windows Backup client. "
                + "Default: absent. Recommended: 1 on managed devices where system images are managed centrally.",
            Tags = ["backup", "system-image", "policy", "restriction"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks Windows Backup system-imaging; system restore points via System Protection are unaffected.",
            ApplyOps = [RegOp.SetDword(BkpKey, "DisableSystemBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableSystemBackup")],
            DetectOps = [RegOp.CheckDword(BkpKey, "DisableSystemBackup", 1)],
        },
        new TweakDef
        {
            Id = "fhp-disable-restore-ui",
            Label = "Disable Windows Backup Restore UI",
            Category = "File History Policy",
            Description =
                "Sets DisableRestoreUI=1 in the Windows Backup client policy key. "
                + "Hides the 'Restore my files' and related controls from the Backup and Restore Control Panel applet. "
                + "Default: absent. Recommended: 1 when restore operations must pass through IT-managed tooling.",
            Tags = ["backup", "restore", "policy", "restriction"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes Windows Backup restore UI; backed-up files still exist but require IT tools to restore.",
            ApplyOps = [RegOp.SetDword(BkpKey, "DisableRestoreUI", 1)],
            RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableRestoreUI")],
            DetectOps = [RegOp.CheckDword(BkpKey, "DisableRestoreUI", 1)],
        },
        new TweakDef
        {
            Id = "fhp-disable-restored-ui",
            Label = "Disable Windows Backup 'Restore to Previous PC' UI",
            Category = "File History Policy",
            Description =
                "Sets DisableRestoredUI=1 in the Windows Backup client policy key. "
                + "Hides the Windows Easy Transfer / 'Restore files from a previous PC' experience "
                + "from the Backup and Restore applet. "
                + "Default: absent. Recommended: 1 on corporate builds where legacy data migration is handled by IT.",
            Tags = ["backup", "restore", "migration", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses the 'previous PC restore' migration experience; no data is changed.",
            ApplyOps = [RegOp.SetDword(BkpKey, "DisableRestoredUI", 1)],
            RemoveOps = [RegOp.DeleteValue(BkpKey, "DisableRestoredUI")],
            DetectOps = [RegOp.CheckDword(BkpKey, "DisableRestoredUI", 1)],
        },
    ];
}
