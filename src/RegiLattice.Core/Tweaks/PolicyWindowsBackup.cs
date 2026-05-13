namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyWindowsBackup
{
    private const string BackupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup";
    private const string ClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";
    private const string ServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Server";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wbackup-disable-backup-to-network",
            Label = "Disable Backup to Network Share",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBackupToNetwork=1 in Backup\\Client policy. Prevents users from "
                + "configuring Windows Backup to save backups to a network share. Ensures backups "
                + "are stored only on local or directly-attached storage.",
            Tags = ["backup", "network", "policy", "security"],
            RegistryKeys = [ClientKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Backup to network shares disabled; only local storage targets allowed.",
            ApplyOps = [RegOp.SetDword(ClientKey, "DisableBackupToNetwork", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableBackupToNetwork")],
            DetectOps = [RegOp.CheckDword(ClientKey, "DisableBackupToNetwork", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-disable-backup-to-optical",
            Label = "Disable Backup to Optical Media",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBackupToOptical=1 in Backup\\Client policy. Prevents users from "
                + "backing up to CD/DVD/Blu-ray media. Optical backups are slow and unreliable; "
                + "this forces users to use faster and more reliable storage targets.",
            Tags = ["backup", "optical", "dvd", "policy"],
            RegistryKeys = [ClientKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Backup to optical discs disabled; use USB or local drives instead.",
            ApplyOps = [RegOp.SetDword(ClientKey, "DisableBackupToOptical", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableBackupToOptical")],
            DetectOps = [RegOp.CheckDword(ClientKey, "DisableBackupToOptical", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-disable-restore-from-network",
            Label = "Disable Restore From Network",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRestoreFromNetwork=1 in Backup\\Client policy. Prevents Windows Backup "
                + "from restoring data from a network share. In secure environments, restores should "
                + "only come from verified local media.",
            Tags = ["backup", "restore", "network", "policy", "security"],
            RegistryKeys = [ClientKey],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Network restore blocked; only local backup media can be used for restore.",
            ApplyOps = [RegOp.SetDword(ClientKey, "DisableRestoreFromNetwork", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableRestoreFromNetwork")],
            DetectOps = [RegOp.CheckDword(ClientKey, "DisableRestoreFromNetwork", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-disable-system-backup",
            Label = "Disable System Image Backup (Client)",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSystemBackupUI=1 in Backup\\Client policy. Removes the system image "
                + "backup option from the Windows Backup UI. Prevents users from creating full "
                + "disk images that consume large amounts of storage.",
            Tags = ["backup", "system-image", "disk-space", "policy"],
            RegistryKeys = [ClientKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "System image backup option hidden; file-level backup still available.",
            ApplyOps = [RegOp.SetDword(ClientKey, "DisableSystemBackupUI", 1)],
            RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableSystemBackupUI")],
            DetectOps = [RegOp.CheckDword(ClientKey, "DisableSystemBackupUI", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-disable-cloud-backup",
            Label = "Disable Cloud Backup Targets",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableCloudBackup=1 in Backup policy. Prevents Windows Backup from using "
                + "cloud storage (OneDrive, Azure) as a backup target. Ensures all backups remain "
                + "on locally-controlled storage.",
            Tags = ["backup", "cloud", "onedrive", "policy", "privacy"],
            RegistryKeys = [BackupKey],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Cloud backup targets disabled; only local/attached storage available.",
            ApplyOps = [RegOp.SetDword(BackupKey, "DisableCloudBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableCloudBackup")],
            DetectOps = [RegOp.CheckDword(BackupKey, "DisableCloudBackup", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-disable-backup-notifications",
            Label = "Disable Windows Backup Notifications",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBackupNotifications=1 in Backup policy. Suppresses all Windows Backup "
                + "toast notifications (backup reminders, completion, failure alerts). Backup status "
                + "must be checked manually via Control Panel.",
            Tags = ["backup", "notifications", "quiet", "policy"],
            RegistryKeys = [BackupKey],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "No backup notifications; check status manually. Silent backup failures possible.",
            ApplyOps = [RegOp.SetDword(BackupKey, "DisableBackupNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableBackupNotifications")],
            DetectOps = [RegOp.CheckDword(BackupKey, "DisableBackupNotifications", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-server-disable-once-only",
            Label = "Disable One-Time Backup (Server)",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRunOnceBackup=1 in Backup\\Server policy. Prevents one-time ad-hoc "
                + "backups via Windows Server Backup. Only scheduled backups are allowed, ensuring "
                + "consistent backup cadence and preventing unscheduled I/O spikes.",
            Tags = ["backup", "server", "one-time", "policy"],
            RegistryKeys = [ServerKey],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Ad-hoc one-time backups blocked; scheduled backups unaffected.",
            ApplyOps = [RegOp.SetDword(ServerKey, "DisableRunOnceBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(ServerKey, "DisableRunOnceBackup")],
            DetectOps = [RegOp.CheckDword(ServerKey, "DisableRunOnceBackup", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-server-disable-bare-metal",
            Label = "Disable Bare Metal Recovery (Server)",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBareMetal=1 in Backup\\Server policy. Prevents bare-metal recovery "
                + "operations via Windows Server Backup. In environments with deployment tools "
                + "(SCCM, MDT), bare-metal restore is unnecessary and should be controlled.",
            Tags = ["backup", "server", "bare-metal", "recovery", "policy"],
            RegistryKeys = [ServerKey],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Bare-metal recovery via WB Server disabled; use deployment tools instead.",
            ApplyOps = [RegOp.SetDword(ServerKey, "DisableBareMetal", 1)],
            RemoveOps = [RegOp.DeleteValue(ServerKey, "DisableBareMetal")],
            DetectOps = [RegOp.CheckDword(ServerKey, "DisableBareMetal", 1)],
        },
        new TweakDef
        {
            Id = "wbackup-limit-backup-count",
            Label = "Limit Maximum Backup Versions to 5",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxBackupVersions=5 in Backup policy. Limits the number of backup versions "
                + "retained on the target drive. Prevents runaway backup storage consumption by "
                + "automatically pruning old versions.",
            Tags = ["backup", "versions", "disk-space", "policy"],
            RegistryKeys = [BackupKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Only 5 backup versions kept; older versions automatically purged.",
            ApplyOps = [RegOp.SetDword(BackupKey, "MaxBackupVersions", 5)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "MaxBackupVersions")],
            DetectOps = [RegOp.CheckDword(BackupKey, "MaxBackupVersions", 5)],
        },
        new TweakDef
        {
            Id = "wbackup-disable-backup-encryption-choice",
            Label = "Force Backup Encryption On",
            Category = "Backup — Windows Backup Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceEncryption=1 in Backup policy. Forces all Windows Backup operations to "
                + "encrypt backup data. Users cannot disable encryption. Protects backup media from "
                + "data exposure if the storage device is lost or stolen.",
            Tags = ["backup", "encryption", "security", "policy"],
            RegistryKeys = [BackupKey],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "All backups encrypted; users must remember/store the recovery key.",
            ApplyOps = [RegOp.SetDword(BackupKey, "ForceEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(BackupKey, "ForceEncryption")],
            DetectOps = [RegOp.CheckDword(BackupKey, "ForceEncryption", 1)],
        },
    ];
}
