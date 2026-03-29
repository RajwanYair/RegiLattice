// RegiLattice.Core — Tweaks/CloudBackupRetentionPolicy.cs
// Cloud Backup Retention and Recovery Policy — Sprint 587.
// Configures enterprise cloud backup retention rules, backup lifecycle
// management, recovery point objectives, backup version limits,
// and stale backup cleanup automation.
// Category: "Cloud Backup Retention Policy" | Slug: cloudbk
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Backup\Client
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\Backup\Server

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudBackupRetentionPolicy
{
    private const string BackupClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

    private const string BackupServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Server";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cloudbk-disable-user-backup-configure",
                Label = "Cloud Backup Retention: Prevent Users from Configuring Windows Backup",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets DisableBackupLauncher=1 in the Backup Client policy key. Prevents non-administrative users from configuring, starting, or modifying Windows Backup (previously Windows Server Backup client). In enterprise environments, backup targets, schedules, and retention settings must be under IT control — users who can configure their own backup destinations can write data to non-sanctioned cloud providers, bypassing DLP controls. Blocking user-initiated backup configuration ensures that all backup operations are managed by IT's corporate backup solution.",
                Tags = ["backup", "backup-configure", "user-restriction", "dlp", "data-governance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Users cannot configure or start Windows Backup. IT must ensure an alternative backup solution is deployed. Users will see 'Your organisation manages this' in Backup settings. Windows Server Backup (wbadmin) when run directly by users will fail — admin elevation required.",
                ApplyOps = [RegOp.SetDword(BackupClientKey, "DisableBackupLauncher", 1)],
                RemoveOps = [RegOp.DeleteValue(BackupClientKey, "DisableBackupLauncher")],
                DetectOps = [RegOp.CheckDword(BackupClientKey, "DisableBackupLauncher", 1)],
            },
            new TweakDef
            {
                Id = "cloudbk-disable-user-restore-access",
                Label = "Cloud Backup Retention: Prevent Users from Performing Self-Service Restore",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets DisableRestoreLauncher=1 in the Backup Client policy key. Prevents non-administrative users from initiating self-service data restore operations via Windows Backup or File History. While self-service restore sounds user-friendly, in regulated environments all data restore operations must be logged, authorised by IT, and recorded for audit trail purposes. Unlogged restores can introduce data from backup sets that contain sensitive versions of files. IT-controlled restore operations ensure proper chain-of-custody for restored data.",
                Tags = ["backup", "restore", "self-service", "audit-trail", "data-governance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Users cannot perform self-service restores. IT must provide a restore request process. Previous Versions (shadow copy restore from Explorer right-click → Restore Previous Versions) may also be affected depending on implementation. Document the IT-managed restore request workflow before deploying.",
                ApplyOps = [RegOp.SetDword(BackupClientKey, "DisableRestoreLauncher", 1)],
                RemoveOps = [RegOp.DeleteValue(BackupClientKey, "DisableRestoreLauncher")],
                DetectOps = [RegOp.CheckDword(BackupClientKey, "DisableRestoreLauncher", 1)],
            },
            new TweakDef
            {
                Id = "cloudbk-set-backup-retention-days-90",
                Label = "Cloud Backup Retention: Set Backup Retention Period to 90 Days",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets RetentionDays=90 in the Backup Server policy key. Sets the backup retention period to 90 days — backup snapshots older than 90 days are automatically purged from the backup store. A 90-day retention balances storage cost against recovery capability — most regulatory frameworks (SOC 2, ISO 27001, HIPAA) require backup retention of at least 30–90 days. For organisations subject to GDPR, 90 days is sufficient for most incident investigation windows while limiting the duration of personal data retained in backups.",
                Tags = ["backup", "retention", "90-days", "gdpr", "storage-lifecycle"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Backup data older than 90 days is purged. Verify that 90 days meets your organisation's regulatory and contractual backup retention obligations. Some industries require longer retention (e.g., financial services require 7 years). Adjust to match compliance requirements.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "RetentionDays", 90)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "RetentionDays")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "RetentionDays", 90)],
            },
            new TweakDef
            {
                Id = "cloudbk-set-max-backup-versions-30",
                Label = "Cloud Backup Retention: Set Maximum Backup Versions Retained to 30",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets MaxBackupVersions=30 in the Backup Server policy key. Limits the number of backup snapshot versions retained per backup job to 30 versions. Without a version cap, high-frequency backups (e.g., hourly file backups) can create hundreds of versions within the retention window — rapidly consuming backup storage. Capping at 30 versions creates a rolling window of 30 recovery points, which is sufficient for most incident recovery scenarios while preventing unbounded storage growth.",
                Tags = ["backup", "version-limit", "storage-cost", "recovery-points", "snapshot"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Maximum 30 backup versions retained per job. When the 30-version limit is reached, the oldest version is removed as each new version is added (FIFO rolling window). Ensure 30 versions × backup frequency covers your minimum recovery time objective (RTO). For hourly backups, 30 versions = 30 hours of recovery points.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "MaxBackupVersions", 30)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "MaxBackupVersions")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "MaxBackupVersions", 30)],
            },
            new TweakDef
            {
                Id = "cloudbk-enable-backup-encryption-required",
                Label = "Cloud Backup Retention: Require Encryption for All Backup Sets",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets RequireBackupEncryption=1 in the Backup Server policy key. Requires that all backup sets created or managed by Windows Backup are encrypted before writing to the backup destination. Unencrypted backups are a significant data exposure risk — if the backup destination (NAS, cloud storage, tape) is compromised or improperly secured, unencrypted backups provide direct access to production data without requiring the attacker to bypass OS-level access controls. Requiring backup encryption ensures that even if backup media is stolen or the backup storage is breached, the data is useless without the encryption key.",
                Tags = ["backup", "encryption", "at-rest", "data-breach", "backup-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "All backup sets must be encrypted. Backup jobs that write to destinations that do not support encryption (some legacy NAS devices, network shares without encryption) will fail until the destination or encryption method is configured. Ensure backup encryption key is backed up separately — loss of the backup encryption key makes the backup unrecoverable.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "RequireBackupEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "RequireBackupEncryption")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "RequireBackupEncryption", 1)],
            },
            new TweakDef
            {
                Id = "cloudbk-enable-backup-integrity-verification",
                Label = "Cloud Backup Retention: Enable Automatic Backup Integrity Verification",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets EnableBackupVerification=1 in the Backup Server policy key. Enables automatic post-backup integrity verification — after each backup job completes, Windows Backup performs a hash verification of the written backup data against the source. Corrupted backup sets that pass initial creation but fail verification are flagged for re-execution. Without integrity verification, a backup set that has bit-level corruption (due to hardware failures, network errors, or storage media degradation) may not be discovered until a restore is attempted — often after the original data has been lost.",
                Tags = ["backup", "integrity-check", "hash-verification", "silent-corruption", "restore-reliability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Backup verification hash check performed after each backup job. Increases backup job duration by approximately 10–20% (depending on backup size and storage I/O). Failed verification triggers re-backup and an alert. Backup storage infrastructure and network paths should be stable before enabling — frequent verification failures on unreliable networks generate a high alert volume.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "EnableBackupVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "EnableBackupVerification")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "EnableBackupVerification", 1)],
            },
            new TweakDef
            {
                Id = "cloudbk-set-backup-job-timeout-hours-6",
                Label = "Cloud Backup Retention: Set Backup Job Maximum Timeout to 6 Hours",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets BackupJobTimeoutHours=6 in the Backup Server policy key. Sets the maximum allowed duration for a single backup job to 6 hours before it is automatically terminated. Without a timeout, backup jobs that stall due to network issues, storage problems, or extremely large files can run indefinitely — consuming backup system resources, holding file locks, and blocking subsequent scheduled jobs. A 6-hour timeout terminates stalled backups, generates a failure alert for investigation, and allows the next scheduled backup job to run.",
                Tags = ["backup", "timeout", "job-management", "stalled-backup", "resource-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Backup jobs exceeding 6 hours are automatically terminated. For organisations with large data sets (multiple TB) backed up over slower links (direct-attached storage over 1 GbE), 6 hours may be insufficient. Calibrate the timeout to your largest expected backup job duration + 50% buffer.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "BackupJobTimeoutHours", 6)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "BackupJobTimeoutHours")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "BackupJobTimeoutHours", 6)],
            },
            new TweakDef
            {
                Id = "cloudbk-enable-backup-failure-alert",
                Label = "Cloud Backup Retention: Enable Automatic Alert on Backup Failure",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets EnableFailureAlert=1 in the Backup Server policy key. Enables the automatic generation of Windows event log alerts (Application log, source: Backup, Event ID 521) when a scheduled backup job fails. Backup failure alerting is a critical operational control — many organisations do not discover that their backup system has been silently failing for weeks or months until a restore is attempted. Proactive failure alerting via Windows Event Log (monitored by SIEM or SCOM) ensures that backup failures are detected and remediated within hours rather than months.",
                Tags = ["backup", "failure-alert", "event-log", "siem", "soc"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Application event log alert generated on each backup failure. Connect event 521 (Backup failure) to SIEM alerting rule and page-duty rotation. Backup failure alerts should be treated as P2 incidents with a 4-hour response SLA in regulated environments.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "EnableFailureAlert", 1)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "EnableFailureAlert")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "EnableFailureAlert", 1)],
            },
            new TweakDef
            {
                Id = "cloudbk-disable-backup-to-optical-media",
                Label = "Cloud Backup Retention: Disable Backup to Optical Media (DVD/BD)",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets DisableOpticalMediaBackup=1 in the Backup Client policy key. Prevents Windows Backup from using optical media (DVD, Blu-ray) as a backup destination. Optical media backups are insecure and impractical for enterprise environments — they lack encryption, version management, and are easily removable without audit trail. An employee can walk out with a DVD containing a full corporate data backup. Enterprise backup destinations should be network-based, access-controlled, and encrypted storage — optical media backup is a residual legacy capability.",
                Tags = ["backup", "optical-media", "dvd", "data-removal", "enterprise-controls"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Optical media (DVD/BD) cannot be used as a backup destination. Minimal enterprise impact — optical media backup has been deprecated in most environments since Windows Server 2012 R2.",
                ApplyOps = [RegOp.SetDword(BackupClientKey, "DisableOpticalMediaBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(BackupClientKey, "DisableOpticalMediaBackup")],
                DetectOps = [RegOp.CheckDword(BackupClientKey, "DisableOpticalMediaBackup", 1)],
            },
            new TweakDef
            {
                Id = "cloudbk-set-backup-network-bandwidth-pct-30",
                Label = "Cloud Backup Retention: Limit Backup Network Bandwidth to 30 Percent",
                Category = "Cloud Backup Retention Policy",
                Description =
                    "Sets MaxNetworkBandwidthPercent=30 in the Backup Server policy key. Throttles backup job network bandwidth consumption to a maximum of 30% of the available network bandwidth. Unthrottled backup jobs on a WAN or limited-bandwidth corporate uplink can saturate the connection, causing network performance degradation for end-users and business applications during backup windows. A 30% bandwidth cap ensures that backup operations, even when running during business hours, do not cause noticeable network performance impact.",
                Tags = ["backup", "bandwidth-throttle", "qos", "network-congestion", "business-hours"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Backup network bandwidth capped at 30%. For large backup sets on slow WAN links, this may cause backup jobs to exceed the 6-hour timeout. Calibrate bandwidth percentage against expected backup size and job timeout. For off-hours backup windows, consider a policy that increases bandwidth to 80% during 22:00–06:00.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "MaxNetworkBandwidthPercent", 30)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "MaxNetworkBandwidthPercent")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "MaxNetworkBandwidthPercent", 30)],
            },
        ];
}
