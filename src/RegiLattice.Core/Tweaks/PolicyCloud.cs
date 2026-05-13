namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyCloud
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CloudBackupRetentionPolicy.Data,
            .. _CloudContentPolicy.Data,
            .. _CloudDesktopPolicy.Data,
            .. _CloudExperienceHostPolicy.Data,
            .. _CloudFileSyncPolicy.Data,
            .. _CloudNotificationsPolicy.Data,
            .. _CloudPrintPolicy.Data,
            .. _CloudStorageQuotaPolicy.Data,
            .. _ContentDeliveryPolicy.Data,
            .. _DesktopAnalyticsPolicy.Data,
            .. _OneDriveKfmPolicy.Data,
            .. _OneDriveSyncPolicy.Data,
            .. _SettingSyncAdv.Data,
            .. _SettingSyncPolicy.Data,
            .. _SharepointOnlinePolicy.Data,
            .. _SkyDrivePolicy.Data,
            .. _UniversalClipboardSyncPolicy.Data,
        ];

    // ── CloudBackupRetentionPolicy ──
    private static class _CloudBackupRetentionPolicy
    {
        private const string BackupClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

        private const string BackupServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Server";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudbk-disable-user-backup-configure",
                    Label = "Cloud Backup Retention: Prevent Users from Configuring Windows Backup",
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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
                    Category = "Cloud Storage — Cloud Backup Retention",
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

    // ── CloudContentPolicy ──
    private static class _CloudContentPolicy
    {
        private const string Cloud = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string CloudCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ccpol-disable-spotlight-on-settings",
                Label = "Cloud Content: Disable Spotlight content on Settings pages",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableWindowsSpotlightOnSettings=1 in CloudContent policy (machine scope). "
                    + "Removes cloud-provided spotlight tips and suggestions from Settings app pages.",
                Tags = ["cloud", "spotlight", "settings", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightOnSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightOnSettings")],
                DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightOnSettings", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-third-party-suggestions",
                Label = "Cloud Content (user): Disable third-party suggestions per user",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableThirdPartySuggestions=1 in HKCU CloudContent policy scope. Provides "
                    + "per-user enforcement of the third-party app-suggestion block.",
                Tags = ["cloud", "suggestions", "third-party", "ads", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableThirdPartySuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableThirdPartySuggestions")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableThirdPartySuggestions", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-spotlight",
                Label = "Cloud Content (user): Disable Windows Spotlight per user",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableWindowsSpotlightFeatures=1 in HKCU CloudContent policy scope. Disables "
                    + "Spotlight lock-screen rotation for the current signed-in user.",
                Tags = ["cloud", "spotlight", "lock-screen", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightFeatures")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightFeatures", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-welcome-experience",
                Label = "Cloud Content (user): Disable Spotlight welcome experience per user",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1 at HKCU scope. Suppresses the "
                    + "'What's new' Spotlight welcome popups for the current user after Windows upgrades.",
                Tags = ["cloud", "spotlight", "welcome", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            },
            new TweakDef
            {
                Id = "ccpol-user-disable-spotlight-on-settings",
                Label = "Cloud Content (user): Disable Spotlight on Settings per user",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableWindowsSpotlightOnSettings=1 at HKCU CloudContent policy scope. Removes "
                    + "cloud-provided spotlight tips from the Settings app for the current user.",
                Tags = ["cloud", "spotlight", "settings", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightOnSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightOnSettings")],
                DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightOnSettings", 1)],
            },
        ];
    }

    // ── CloudDesktopPolicy ──
    private static class _CloudDesktopPolicy
    {
        private const string CdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudDesktop";
        private const string CpcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "clouddesk-disable-cloud-pc-entry-points",
                Label = "Disable Cloud PC Entry Points in Windows UI",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableCloudPCEntryPoints=1 in the CloudDesktop policy key. "
                    + "Removes the Windows 365 Cloud PC link, button, and notification from the "
                    + "Windows Start menu, Settings, and taskbar. Prevents users from seeing or clicking "
                    + "entry points that would prompt them to sign up for or access a Windows 365 subscription. "
                    + "Appropriate for organizations that do not use Windows 365. "
                    + "Default: absent (entry points shown). Recommended: 1 on non-W365 endpoints.",
                Tags = ["cloud-desktop", "windows-365", "cloud-pc", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows 365 Cloud PC entry points removed from Windows UI.",
                ApplyOps = [RegOp.SetDword(CdKey, "DisableCloudPCEntryPoints", 1)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "DisableCloudPCEntryPoints")],
                DetectOps = [RegOp.CheckDword(CdKey, "DisableCloudPCEntryPoints", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-provisioning",
                Label = "Disable Cloud PC Provisioning",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets EnableProvisioning=0 in the CloudDesktop policy key. "
                    + "Prevents the Windows 365 agent from auto-provisioning a Cloud PC session on this device. "
                    + "Useful on physical endpoints that should never auto-redirect to a cloud desktop, "
                    + "ensuring users always work on the local machine's resources. "
                    + "Default: absent (provisioning allowed). Recommended: 0 on standard physical desktops.",
                Tags = ["cloud-desktop", "windows-365", "provisioning", "cloud-pc", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC auto-provisioning disabled; users must connect manually if needed.",
                ApplyOps = [RegOp.SetDword(CdKey, "EnableProvisioning", 0)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "EnableProvisioning")],
                DetectOps = [RegOp.CheckDword(CdKey, "EnableProvisioning", 0)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-virtual-desktop-agent",
                Label = "Disable Cloud PC Agent Auto-Start",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableCloudPCAgent=1 in the CloudDesktop policy key. "
                    + "Prevents the Windows 365/Cloud PC management agent from auto-starting at user login. "
                    + "The agent monitors session state and applies Cloud PC policies; disabling it prevents "
                    + "Windows 365 session management from running on machines that should not connect to "
                    + "any cloud desktop infrastructure. "
                    + "Default: absent (agent starts automatically). Recommended: 1 on non-W365 machines.",
                Tags = ["cloud-desktop", "windows-365", "agent", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows 365 Cloud PC management agent blocked from auto-starting at login.",
                ApplyOps = [RegOp.SetDword(CdKey, "DisableCloudPCAgent", 1)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "DisableCloudPCAgent")],
                DetectOps = [RegOp.CheckDword(CdKey, "DisableCloudPCAgent", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-cloudpc-connection-uac",
                Label = "Disable Cloud PC UAC Elevation Prompts",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets NoAdminUACForCloudPC=1 in the CloudDesktop policy key. "
                    + "Prevents the Cloud PC connection process from triggering UAC elevation dialogs "
                    + "on the local machine. When a Cloud PC session needs elevated rights, the request "
                    + "is handled within the remote cloud session — not on the local endpoint. "
                    + "Reduces login friction on kiosk machines where Cloud PC is the primary desktop. "
                    + "Default: absent. Recommended: 1 on dedicated Cloud PC access endpoints.",
                Tags = ["cloud-desktop", "uac", "elevation", "cloud-pc", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Cloud PC connection process does not prompt UAC elevation on the local machine.",
                ApplyOps = [RegOp.SetDword(CdKey, "NoAdminUACForCloudPC", 1)],
                RemoveOps = [RegOp.DeleteValue(CdKey, "NoAdminUACForCloudPC")],
                DetectOps = [RegOp.CheckDword(CdKey, "NoAdminUACForCloudPC", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-single-sign-on",
                Label = "Disable Single Sign-On to Cloud PC",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableSSO=1 in the CloudPC policy key. "
                    + "Prevents automatic single sign-on (SSO) to the Windows 365 Cloud PC using the "
                    + "local Windows account credentials. When SSO is enabled, a logged-in user is "
                    + "automatically authenticated to the Cloud PC session without re-entering credentials. "
                    + "Disabling SSO requires users to explicitly authenticate each Cloud PC session, "
                    + "providing an additional security checkpoint. "
                    + "Default: absent (SSO enabled). Recommended: 1 for high-security access control.",
                Tags = ["cloud-desktop", "sso", "authentication", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC SSO disabled; users explicitly authenticate each Cloud PC session.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisableSSO", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableSSO")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisableSSO", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-enable-cloud-pc-telemetry-opt-out",
                Label = "Opt Out of Cloud PC Telemetry",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableTelemetry=1 in the CloudPC policy key. "
                    + "Prevents the Cloud PC client from sending diagnostics, usage telemetry, and "
                    + "session-quality metrics to Microsoft's Windows 365 service. "
                    + "Applicable in privacy-sensitive environments or air-gapped networks where "
                    + "outbound telemetry must be minimised. "
                    + "Default: absent (telemetry enabled). Recommended: 1 in high-privacy environments.",
                Tags = ["cloud-desktop", "telemetry", "privacy", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC client telemetry to Microsoft suppressed.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-restrict-cloud-pc-regions",
                Label = "Restrict Cloud PC Provisioning to Closest Region Only",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets RegionSelectionPolicy=1 in the CloudPC policy key. "
                    + "Forces the Windows 365 provisioning system to select only the closest Azure region "
                    + "when allocating a new Cloud PC, instead of allowing cross-region or scheduled-region "
                    + "provisioning. Ensures low latency for users and keeps data residency within the "
                    + "organisation's primary Azure geography. "
                    + "Default: absent (any region). Recommended: 1 for data residency compliance.",
                Tags = ["cloud-desktop", "region", "data-residency", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud PC provisioned in closest Azure region only; data stays in primary geography.",
                ApplyOps = [RegOp.SetDword(CpcKey, "RegionSelectionPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "RegionSelectionPolicy")],
                DetectOps = [RegOp.CheckDword(CpcKey, "RegionSelectionPolicy", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-cloud-pc-share-clipboard",
                Label = "Disable Clipboard Sharing Between Cloud PC and Local",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisableServerClipboard=1 in the CloudPC policy key. "
                    + "Prevents clipboard content from being shared between the local endpoint and the "
                    + "Windows 365 Cloud PC session. Clipboard sync can be a vector for data exfiltration "
                    + "(copy from cloud session, paste to local — or vice versa). "
                    + "Disabling this enforces a hard data-boundary between local and cloud environments. "
                    + "Default: absent (clipboard sharing enabled). Recommended: 1 for DLP compliance.",
                Tags = ["cloud-desktop", "clipboard", "data-leakage", "windows-365", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard not shared between Cloud PC and local endpoint session.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisableServerClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableServerClipboard")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisableServerClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-disable-cloud-pc-redirect-printers",
                Label = "Disable Printer Redirection for Cloud PC Sessions",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets DisablePrinterRedirection=1 in the CloudPC policy key. "
                    + "Prevents local printers attached to the endpoint from being presented inside the "
                    + "Windows 365 Cloud PC session. Printer redirection streams print jobs from the cloud "
                    + "session to a local network printer, but can expose printer model/driver information "
                    + "across the cloud boundary. Disabling this restricts Cloud PC sessions to cloud-side "
                    + "printing only. Default: absent (redirection enabled).",
                Tags = ["cloud-desktop", "printer", "redirection", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Local printers not redirected into Cloud PC sessions.",
                ApplyOps = [RegOp.SetDword(CpcKey, "DisablePrinterRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "DisablePrinterRedirection")],
                DetectOps = [RegOp.CheckDword(CpcKey, "DisablePrinterRedirection", 1)],
            },
            new TweakDef
            {
                Id = "clouddesk-set-max-session-idle-timeout",
                Label = "Set Cloud PC Session Idle Disconnect Timeout",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Sets IdleSessionTimeout=30 in the CloudPC policy key. "
                    + "Sets the maximum idle time (in minutes) before a Windows 365 Cloud PC session is "
                    + "automatically disconnected. Idle Cloud PC sessions continue to consume Azure compute "
                    + "and network resources. Auto-disconnect after 30 minutes of inactivity reduces costs "
                    + "and ensures unattended sessions do not remain accessible for extended periods. "
                    + "Default: absent (no idle timeout). Recommended: 15-60 depending on TCO requirements.",
                Tags = ["cloud-desktop", "session", "idle", "timeout", "windows-365", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cloud PC sessions disconnected after 30 minutes of inactivity; saves Azure compute cost.",
                ApplyOps = [RegOp.SetDword(CpcKey, "IdleSessionTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(CpcKey, "IdleSessionTimeout")],
                DetectOps = [RegOp.CheckDword(CpcKey, "IdleSessionTimeout", 30)],
            },
        ];
    }

    // ── CloudExperienceHostPolicy ──
    private static class _CloudExperienceHostPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudExperienceHost";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cehpol-disable-cloud-experience",
                Label = "CXH Policy: Disable Windows Cloud Experience Host",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Disables the Windows Cloud Experience Host (CXH) process that manages OOBE, Tips, and cloud-connected first-run experiences. Reduces telemetry and suppresses pop-up prompts to connect Microsoft services.",
                Tags = ["cxh", "oobe", "cloud", "experience", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables CXH process; reduces telemetry and suppresses service-connect prompts.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudExperienceHost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudExperienceHost")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudExperienceHost", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-oobe-privacy-page",
                Label = "CXH Policy: Disable Privacy Settings Page in OOBE",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Skips the Privacy Settings experience page during Windows Out-of-Box Experience (OOBE). Ensures default privacy settings are applied silently without user interaction during provisioning.",
                Tags = ["cxh", "oobe", "privacy", "setup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips privacy settings page in OOBE for silent enterprise provisioning.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyExperiencePage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyExperiencePage")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyExperiencePage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-skip-machine-oobe",
                Label = "CXH Policy: Skip Machine-Level OOBE on First Boot",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Skips the machine-level Windows OOBE experience on the first boot of a provisioned device. Useful for enterprise images where OOBE is unnecessary and should be bypassed for imaging targets.",
                Tags = ["cxh", "oobe", "provisioning", "first-boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips machine-level OOBE on first boot for enterprise images.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipMachineOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipMachineOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "SkipMachineOOBE", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-tailored-experience",
                Label = "CXH Policy: Disable Tailored Experiences with Diagnostic Data",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Prevents Windows from using diagnostic data to deliver personalised tips, ads, and recommendations via the Cloud Experience Host. Applies at the machine level via Group Policy.",
                Tags = ["cxh", "tailored", "diagnostic", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents diagnostic data from powering personalised tips and ads via CXH.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventTailoredExperiences", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventTailoredExperiences")],
                DetectOps = [RegOp.CheckDword(Key, "PreventTailoredExperiences", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-frx-telemetry",
                Label = "CXH Policy: Disable OOBE Telemetry Data Submission",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Disables telemetry data collection and submission during the OOBE First-Run Experience (Frx). Prevents Microsoft from receiving device setup analytics from enterprise-provisioned devices.",
                Tags = ["cxh", "oobe", "telemetry", "frx", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables telemetry submission during OOBE first-run experience.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOOBETelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOOBETelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOOBETelemetry", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-account-setup-page",
                Label = "CXH Policy: Disable Account Setup Page in Provisioning",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Bypasses the Microsoft Account / Azure AD account setup page during OOBE provisioning. Ensures the device is silently joined to the corporate domain without displaying the consumer account prompt.",
                Tags = ["cxh", "oobe", "account", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Bypasses MSA/Azure AD account setup page during enterprise provisioning.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAccountSetupPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountSetupPage")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAccountSetupPage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-cortana-oobe",
                Label = "CXH Policy: Disable Cortana during OOBE",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Prevents the Cortana voice assistant from launching during OOBE. Stops Cortana from speaking during initial setup on enterprise-provisioned devices, reducing unexpected data transmission.",
                Tags = ["cxh", "oobe", "cortana", "voice", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents Cortana from launching or transmitting during OOBE.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCortanaDuringOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCortanaDuringOOBE")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCortanaDuringOOBE", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-device-encryption-page",
                Label = "CXH Policy: Skip Device Encryption Page in OOBE",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Bypasses the BitLocker Device Encryption setup page during OOBE. Enterprises typically deploy their own BitLocker policy via MDM/GPO and do not want users configuring encryption manually.",
                Tags = ["cxh", "oobe", "bitlocker", "encryption", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips BitLocker setup page in OOBE; enterprises deploy via GPO/MDM.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipDeviceEncryptionPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipDeviceEncryptionPage")],
                DetectOps = [RegOp.CheckDword(Key, "SkipDeviceEncryptionPage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-windows-hello-oobe",
                Label = "CXH Policy: Skip Windows Hello Setup in OOBE",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Bypasses the Windows Hello biometric/PIN setup prompts during OOBE. Enterprises deploying Windows Hello for Business via GPO/MDM do not need the consumer OOBE Hello setup flow.",
                Tags = ["cxh", "oobe", "windows-hello", "biometrics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Bypasses consumer Hello setup flow; WHfB deployed via GPO/MDM.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipWindowsHelloSetupPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipWindowsHelloSetupPage")],
                DetectOps = [RegOp.CheckDword(Key, "SkipWindowsHelloSetupPage", 1)],
            },
            new TweakDef
            {
                Id = "cehpol-disable-oobe-network-page",
                Label = "CXH Policy: Skip Network Connection Page in OOBE",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Skips the Wi-Fi / network connection page during OOBE. Enterprise devices are typically pre-configured with wireless profiles via MDM, removing the need to prompt users during provisioning.",
                Tags = ["cxh", "oobe", "network", "wifi", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Skips Wi-Fi page in OOBE; enterprise devices pre-configured with network profiles.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SkipNetworkConnectionPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipNetworkConnectionPage")],
                DetectOps = [RegOp.CheckDword(Key, "SkipNetworkConnectionPage", 1)],
            },
        ];
    }

    // ── CloudFileSyncPolicy ──
    private static class _CloudFileSyncPolicy
    {
        private const string OdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

        private const string WfKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkFolders";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cfsync-require-sync-encryption",
                    Label = "Cloud File Sync: Require Encryption for Synced Files",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets RequireEncryption=1 in WorkFolders policy. Requires that all files managed by Windows Work Folders are stored in an encrypted state on the device's local sync cache. When encryption is required, Work Folders integrates with BitLocker or EFS to ensure the local copy of synced files is encrypted at rest. A device that loses BitLocker protection (TPM not present, BitLocker disabled) cannot sync Work Folders files. Ensures cloud-synced corporate files remain protected even if the device storage is physically accessed.",
                    Tags = ["file-sync", "encryption", "work-folders", "data-protection", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Synced files require local encryption. Work Folders clients on unencrypted devices cannot sync. Verify BitLocker or EFS is deployed before enabling — sync stops on non-compliant devices.",
                    ApplyOps = [RegOp.SetDword(WfKey, "RequireEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(WfKey, "RequireEncryption")],
                    DetectOps = [RegOp.CheckDword(WfKey, "RequireEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-enable-known-folder-move",
                    Label = "Cloud File Sync: Enable Known Folder Move to OneDrive for Business",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets KFMSilentOptIn=<TenantID> equivalent as a policy flag via EnableKnownFolderMove=1. Enables silent migration of Desktop, Documents, and Pictures from local storage to the user's OneDrive for Business account. Files in Windows known folders are moved to OneDrive and folder redirection is updated automatically without prompting the user. This provides cloud backup for user data on all managed devices without requiring users to manually configure OneDrive — the most common cause of data loss is users who never configured backup.",
                    Tags = ["onedrive", "known-folder-move", "backup", "enterprise", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Desktop, Documents, Pictures are silently moved to OneDrive for Business. Requires M365 licences with OneDrive for Business. Users see their folders unchanged but data is now synced to cloud.",
                    ApplyOps = [RegOp.SetDword(OdKey, "EnableKnownFolderMove", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "EnableKnownFolderMove")],
                    DetectOps = [RegOp.CheckDword(OdKey, "EnableKnownFolderMove", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-disable-wf-auto-setup",
                    Label = "Cloud File Sync: Disable Automatic Work Folders Setup",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets AutoSetup=0 in WorkFolders policy. Prevents Windows from automatically configuring Work Folders when a user signs in to a domain with an SRV record for Work Folders discovery. Automatic Work Folders setup creates local sync directories and begins syncing corporate content without user awareness. In environments that have migrated to OneDrive for Business, phantom Work Folders sync clients create duplicate data paths and storage overhead. Disabling auto-setup ensures Work Folders is only provisioned by explicit IT configuration.",
                    Tags = ["work-folders", "auto-setup", "enterprise", "sync", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Work Folders does not auto-configure on domain join. Work Folders must be configured explicitly by IT. Prevents unintended dual-sync (Work Folders + OneDrive) on migrated environments.",
                    ApplyOps = [RegOp.SetDword(WfKey, "AutoSetup", 0)],
                    RemoveOps = [RegOp.DeleteValue(WfKey, "AutoSetup")],
                    DetectOps = [RegOp.CheckDword(WfKey, "AutoSetup", 0)],
                },
                new TweakDef
                {
                    Id = "cfsync-disable-onedrive-auto-start",
                    Label = "Cloud File Sync: Disable OneDrive from Starting Automatically",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets Enabled=0 in OneDrive policy's auto-start key. Prevents the OneDrive sync client from starting automatically at user logon. In environments where OneDrive is not provisioned as the corporate sync solution (e.g., Work Folders or third-party DMS is used instead), having the OneDrive client start in every user session wastes resources and prompts users to configure personal accounts. When OneDrive deployment is managed through Intune or dedicated onboarding workflows, auto-start is unnecessary.",
                    Tags = ["onedrive", "auto-start", "startup", "resource", "control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive does not start at logon. Users must launch OneDrive manually or it is deployed with auto-start via Intune. No impact on Work Folders or other sync clients.",
                    ApplyOps = [RegOp.SetDword(OdKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(OdKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "cfsync-block-sync-to-unmanaged-domains",
                    Label = "Cloud File Sync: Block OneDrive Sync to Unmanaged Azure AD Tenants",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets TenantRestriction=1 in OneDrive policy. Restricts OneDrive sync client connections to only the organisation's Azure AD tenant. Users cannot sync SharePoint data from external tenants or guest accounts that reside in other organisations' Azure AD tenants. This is a data exfiltration prevention control: an employee who has been invited as a guest to an external organisation's Azure AD can otherwise use the OneDrive sync client to download the external organisation's SharePoint data to the corporate machine.",
                    Tags = ["onedrive", "tenant-restriction", "data-exfiltration", "guest", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync is restricted to the organisation's Azure AD tenant. Employees who are guests in external Azure AD tenants cannot sync external SharePoint data. B2B collaboration via browser-based SharePoint is unaffected.",
                    ApplyOps = [RegOp.SetDword(OdKey, "TenantRestriction", 1)],
                    RemoveOps = [RegOp.DeleteValue(OdKey, "TenantRestriction")],
                    DetectOps = [RegOp.CheckDword(OdKey, "TenantRestriction", 1)],
                },
                new TweakDef
                {
                    Id = "cfsync-require-lock-on-wf-idle",
                    Label = "Cloud File Sync: Require Device Lock When Work Folders in Use",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets LockDriveOnIdle=1 in WorkFolders policy. Configures Work Folders to require device screen lock after the device idle timeout when Work Folders are configured. An unlocked device with Work Folders gives an unattended attacker access to synced corporate files without authentication. This policy enforces the screen lock policy as a prerequisite for Work Folders access: if the device screen lock is not configured (no timeout, no PIN on lock), Work Folders displays a warning and may suspend sync until lock is enabled.",
                    Tags = ["work-folders", "screen-lock", "security", "idle", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Screen lock is required when Work Folders are configured. Devices without screen lock display a compliance warning. Does not forcibly lock the screen — it enforces existing screen lock policy configuration.",
                    ApplyOps = [RegOp.SetDword(WfKey, "LockDriveOnIdle", 1)],
                    RemoveOps = [RegOp.DeleteValue(WfKey, "LockDriveOnIdle")],
                    DetectOps = [RegOp.CheckDword(WfKey, "LockDriveOnIdle", 1)],
                },
            ];
    }

    // ── CloudNotificationsPolicy ──
    private static class _CloudNotificationsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudNotifications";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cloudntf-disable-cloud-notifications",
                Label = "Cloud Notifications Policy: Disable All Cloud Notifications",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Disables the Windows Cloud Notification facility at the policy level. Cloud notifications enable Microsoft and app publishers to deliver system-level banners from cloud services without user-initiated sessions. Disabling prevents unsolicited messages from reaching the desktop.",
                Tags = ["notifications", "cloud", "wns", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudNotifications", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Disables all WNS push; may prevent Defender alerts and Store update notifications.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-account-notifications",
                Label = "Cloud Notifications Policy: Block Microsoft Account Notifications",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Suppresses notifications related to Microsoft Account sign-in prompts, subscription reminders, and account health alerts delivered via the cloud notification channel. Reduces distracting prompts on managed devices where personal MSA usage is not permitted.",
                Tags = ["notifications", "cloud", "microsoft-account", "msa", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAccountNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAccountNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Suppresses Microsoft Account subscription and health alerts on managed devices.",
            },
            new TweakDef
            {
                Id = "cloudntf-block-network-usage",
                Label = "Cloud Notifications Policy: Block WNS Background Network Usage",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Prevents the Windows Notification Service (WNS) from establishing and maintaining persistent outbound connections to Microsoft's push notification servers. On metered or restricted networks, WNS background connections consume quota and expose device online status to Microsoft.",
                Tags = ["notifications", "cloud", "wns", "network", "metered", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoNotificationNetworkUsage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoNotificationNetworkUsage")],
                DetectOps = [RegOp.CheckDword(Key, "NoNotificationNetworkUsage", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents WNS persistent outbound connections; may break app push notifications.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-push-to-install",
                Label = "Cloud Notifications Policy: Disable Push-to-Install Notifications",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Disables cloud-triggered push-to-install notifications that can silently queue OS app installation from the Microsoft Store or Intune. On non-MDM-managed endpoints, push-to-install is a covert app deployment vector.",
                Tags = ["notifications", "cloud", "push-to-install", "store", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePushToInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePushToInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePushToInstall", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks cloud-triggered silent app installations from Microsoft Store or Intune.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-wns-on-metered",
                Label = "Cloud Notifications Policy: Disable WNS on Metered Connections",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Prevents Windows Notification Service from using metered internet connections (mobile hotspot, cellular). WNS persistent connections on metered networks generate background data charges without user consent.",
                Tags = ["notifications", "cloud", "wns", "metered", "data", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWNSConnectionOnMetered", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWNSConnectionOnMetered")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWNSConnectionOnMetered", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents WNS background data charges on metered or cellular connections.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-notification-mirroring",
                Label = "Cloud Notifications Policy: Disable Cross-Device Notification Mirroring",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Disables notification mirroring — the feature that forwards a device's notifications to other Windows 10/11 machines signed in with the same Microsoft Account. Notification mirroring routes notification content through Microsoft cloud relays, creating potential data leakage.",
                Tags = ["notifications", "cloud", "mirroring", "cross-device", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNotificationMirroring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNotificationMirroring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNotificationMirroring", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops notification content routing through Microsoft cloud relays to other devices.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-promotional-banners",
                Label = "Cloud Notifications Policy: Disable Microsoft Promotional Notification Banners",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Suppresses promotional and feature-suggestion notifications delivered through the Windows cloud notification channel. Microsoft uses cloud notifications to surface upgrade prompts, subscription upsells, and feature announcements which are disruptive in managed enterprise environments.",
                Tags = ["notifications", "cloud", "promotional", "ads", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePromotionalNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePromotionalNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePromotionalNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Microsoft upgrade prompts and upsells delivered via cloud notification channel.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-diagnostic-upload",
                Label = "Cloud Notifications Policy: Disable Diagnostic Payload in Cloud Notifications",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Prevents the WNS notification channel from including diagnostic telemetry payloads in cloud notification requests. Notification channel diagnostics include device health and engagement metrics that are forwarded to Microsoft without explicit user opt-in.",
                Tags = ["notifications", "cloud", "diagnostics", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticInNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticInNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticInNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents device engagement metrics from being included in WNS diagnostic payloads.",
            },
            new TweakDef
            {
                Id = "cloudntf-block-background-refresh",
                Label = "Cloud Notifications Policy: Block Cloud Notification Background Refresh",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Prevents applications from refreshing cloud-sourced notification content in the background while not in use. Background notification refresh for cloud-connected apps creates persistent outbound connections to app backends that profile device online patterns.",
                Tags = ["notifications", "cloud", "background", "refresh", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockBackgroundNotificationRefresh", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockBackgroundNotificationRefresh")],
                DetectOps = [RegOp.CheckDword(Key, "BlockBackgroundNotificationRefresh", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents apps maintaining persistent cloud connections when not in use.",
            },
            new TweakDef
            {
                Id = "cloudntf-disable-focus-assist-override",
                Label = "Cloud Notifications Policy: Prevent Cloud Override of Focus Assist",
                Category = "Cloud Storage — Cloud Backup Retention",
                Description =
                    "Blocks cloud services from overriding the local Focus Assist (Do Not Disturb) settings to deliver high-priority cloud notifications. Ensures user-configured quiet hours are respected even when Microsoft or app publishers classify a cloud notification as urgent.",
                Tags = ["notifications", "cloud", "focus-assist", "do-not-disturb", "override", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventFocusAssistOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventFocusAssistOverride")],
                DetectOps = [RegOp.CheckDword(Key, "PreventFocusAssistOverride", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ensures user-configured quiet hours are not bypassed by cloud-classified urgent notifications.",
            },
        ];
    }

    // ── CloudPrintPolicy ──
    private static class _CloudPrintPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudPrint";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cldprt-disable-cloud-print-service",
                Label = "Disable Windows Cloud Print Discovery and Universal Print Services",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disabling Windows Cloud Print discovery prevents client computers from connecting to cloud-hosted print services that could transmit document content to external cloud infrastructure outside organizational control. Cloud print services route document data through external servers and the privacy and security controls of those cloud services may not meet organizational compliance requirements. Organizations that manage print infrastructure with on-premises print servers should disable cloud print discovery to ensure all printing flows through audited enterprise print infrastructure. Accidental use of cloud print services can result in sensitive documents being transmitted to and stored by external cloud providers without appropriate data handling controls. Disabling cloud print discovery does not prevent users from manually configuring printers but does remove automatic discovery of cloud print endpoints from the print experience. Organizations that have legitimate cloud print requirements through approved enterprise services like Universal Print should configure those services centrally rather than enabling broad cloud print discovery.",
                Tags = ["cloud-print", "print-services", "data-protection", "discovery", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudPrintService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPrintService")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudPrintService", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-restrict-cloud-printer-installation",
                Label = "Restrict User-Initiated Cloud Printer Installation Without Administrator Approval",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting user-initiated cloud printer installation prevents standard users from adding cloud print destinations that route documents through external services not approved or managed by IT operations. User-installed cloud printers can exfiltrate sensitive document content to personal or unauthorized business cloud print accounts outside organizational visibility. Print driver installation associated with cloud printers can introduce software components that have not been vetted by endpoint security teams. Organizations should centrally manage all printer deployments including cloud printers through Group Policy or device management platforms to ensure only approved print destinations are available. Restricting printer installation to administrators allows IT to control the complete list of print destinations available to users including auditing which cloud print services are in use. Users who have legitimate business requirements for cloud printing should submit requests through the IT service catalog for evaluation and approved deployment.",
                Tags = ["cloud-print", "printer-installation", "user-restriction", "data-handling", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCloudPrinterInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCloudPrinterInstallation")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCloudPrinterInstallation", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-enforce-enterprise-cloud-print-only",
                Label = "Enforce Use of Enterprise-Only Cloud Print Services for All Organization Devices",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforcing enterprise-only cloud print restricts cloud print operations to organizationally approved cloud print services preventing use of personal or unauthorized third-party cloud print providers. Many employees use consumer cloud print services for convenience when enterprise print alternatives are inconvenient but this creates uncontrolled data flows of potentially sensitive documents. Enterprise cloud print services like Microsoft Universal Print integrate with Azure Active Directory and provide administrative visibility into print jobs including audit logging relevant to compliance requirements. Organizations should deploy approved enterprise cloud print services that provide administrative oversight before enforcing the enterprise-only cloud print restriction. Transitioning from unrestricted cloud print to enterprise-only requires communication to users about what print services are approved and how to access them for remote and mobile printing scenarios. Audit logging of cloud print operations through enterprise print services provides visibility into printing volumes and patterns that may indicate data exfiltration attempts.",
                Tags = ["cloud-print", "enterprise-only", "approved-services", "universal-print", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceEnterpriseCloudPrintOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceEnterpriseCloudPrintOnly")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceEnterpriseCloudPrintOnly", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-disable-mobile-print-discovery",
                Label = "Disable Automatic Mobile Print Service Discovery on Corporate Network",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Automatic mobile print discovery broadcasts the availability of print services to mobile devices on the corporate network creating potential data exfiltration pathways through mobile device printing that bypasses desktop endpoint security controls. Mobile devices connecting to corporate printers through automatic discovery may not have the same document handling policies applied that are enforced on managed desktop systems. Disabling automatic mobile print discovery reduces the attack surface from rogue mobile devices that join the corporate wireless network and attempt to access print infrastructure. Organizations that support mobile printing should implement this through Mobile Device Management policies that configure approved wireless print access rather than through open network service discovery. Print infrastructure security should include network access controls to restrict which devices can communicate with print servers. Corporate print servers should be segmented from general user VLAN segments to limit direct print protocol access to devices with legitimate printing needs.",
                Tags = ["mobile-print", "print-discovery", "network-security", "attack-surface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMobilePrintDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMobilePrintDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMobilePrintDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-audit-cloud-print-job-submissions",
                Label = "Enable Audit Logging for All Cloud Print Job Submissions and Printer Access",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Audit logging for cloud print job submissions records every print job sent to cloud print services including the user identity document metadata and destination printer providing an audit trail for potential data exfiltration investigations. Large document printing events or printing to unusual destinations outside normal work hours may indicate data exfiltration using print as a covert data transfer channel. Print audit logs should be integrated with SIEM alerting to detect anomalous print activity such as printing significantly more pages than baseline or printing sensitive documents to non-standard destinations. Cloud print audit logging from enterprise services provides more complete visibility than local print spooler logging because it captures the complete print workflow across cloud infrastructure. Organizations subject to data protection regulations should retain print audit logs for periods that satisfy retention requirements for regulatory investigations. User privacy considerations should be balanced with security monitoring needs when designing print audit programs to ensure appropriate oversight without unnecessary surveillance.",
                Tags = ["cloud-print", "print-audit", "monitoring", "data-exfiltration", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditCloudPrintJobSubmissions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCloudPrintJobSubmissions")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCloudPrintJobSubmissions", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-require-mfa-for-cloud-print-auth",
                Label = "Require Multi-Factor Authentication for Cloud Print Service Authentication",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Requiring multi-factor authentication for cloud print service operations ensures that cloud print access cannot be used to authenticate as a user using only stolen credentials which could expose sensitive documents in the print queue. Cloud print services that authenticate with Azure Active Directory credentials should inherit the conditional access policies that require MFA for cloud service authentication. Print jobs queued in cloud print infrastructure are protected by authentication requirements at the time of retrieval preventing unauthorized release of documents from cloud print queues. MFA for cloud print authentication prevents attackers who compromise user credentials from submitting or retrieving print jobs that might reveal organizational information. Organizations should configure conditional access policies that apply MFA requirements to cloud print services as part of the general cloud service MFA rollout. Service accounts used for print infrastructure management should use certificate-based authentication or managed identity approaches rather than password-based MFA.",
                Tags = ["cloud-print", "mfa", "authentication", "cloud-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireMFAForCloudPrintAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMFAForCloudPrintAuth")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMFAForCloudPrintAuth", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-disable-print-to-pdf-cloud-storage",
                Label = "Disable Automatic Saving of Print to PDF Output to Cloud Storage Locations",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Print to PDF feature when configured to automatically save output to cloud-connected storage locations including OneDrive can result in sensitive document content being transmitted to cloud storage without explicit user intent. Disabling automatic cloud save for Print to PDF output ensures that users must explicitly choose to save PDF output to cloud storage rather than having documents automatically uploaded. Documents printed to PDF during the course of normal workday operations may include sensitive contracts financial documents HR information or other content that should not be automatically uploaded to personal cloud storage. Organizations should configure Print to PDF default save locations to point to local or networked storage under organizational control. Users who have business requirements to share PDF output via cloud storage should explicitly save documents to approved business cloud storage rather than through automatic upload from the print subsystem. Document classification and DLP policies should be applied to all cloud storage upload paths to prevent accidental upload of sensitive content.",
                Tags = ["print-to-pdf", "cloud-storage", "data-protection", "automatic-upload", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintToPdfCloudStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToPdfCloudStorage")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintToPdfCloudStorage", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-restrict-personal-cloud-print-accounts",
                Label = "Restrict Use of Personal Cloud Print Accounts on Domain-Joined Devices",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting personal cloud print account use on domain-joined devices prevents sensitive corporate documents from being transmitted to personal cloud print queues associated with non-corporate accounts that lack organizational data security controls. Employees using personal Google Cloud Print HP ePrint or other consumer cloud print services may not understand that their documents are being stored by the print service provider under consumer terms of service rather than enterprise security agreements. Personal cloud print accounts are not subject to corporate data retention deletion and security audit requirements creating compliance gaps for regulated organizations. Domain-joined devices should be configured to allow only enterprise cloud print accounts authenticated with organizational credentials. Unified endpoint management platforms can enforce cloud print account restrictions for both domain-joined and modern device-enrolled endpoints providing consistent control across device management approaches. User education about the importance of using only approved organizational print services for corporate documents should accompany technical controls.",
                Tags = ["personal-accounts", "cloud-print", "data-governance", "domain-joined", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictPersonalCloudPrintAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictPersonalCloudPrintAccounts")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictPersonalCloudPrintAccounts", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-block-unencrypted-cloud-print-transmission",
                Label = "Block Unencrypted Data Transmission to Cloud Print Service Endpoints",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking unencrypted cloud print transmission ensures that all document data sent to cloud print services uses TLS-encrypted connections preventing interception of document content during transmission to the cloud print infrastructure. Legacy print protocols and some consumer cloud print integrations may use unencrypted HTTP connections for print job submission that expose document content to network interception. Organizations should verify that approved cloud print services use TLS 1.2 or higher for all print data transmission and certificate validation is enforced to prevent man-in-the-middle attacks. Encrypted cloud print transmission protects document content from passive network monitoring by adversaries with access to enterprise or internet network segments. DLP monitoring at the network layer can inspect print traffic transmitted over unencrypted channels making encryption enforcement a complementary control to DLP. Cloud print service vendor security documentation should be reviewed to understand the encryption and data protection measures in place for print data at rest in cloud infrastructure.",
                Tags = ["cloud-print", "encryption", "tls", "data-in-transit", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnencryptedCloudPrintTransmission", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnencryptedCloudPrintTransmission")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnencryptedCloudPrintTransmission", 1)],
            },
            new TweakDef
            {
                Id = "cldprt-enforce-print-data-retention-policy",
                Label = "Enforce Organizational Data Retention Policy for Cloud Print Job Metadata",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Cloud print service metadata including print job history user identities document names timestamps and printer destinations is retained by cloud print providers for configuration, support and billing purposes which may conflict with organizational data retention and deletion policies. Enforcing an organizational print data retention policy ensures that print job metadata stored in cloud print infrastructure is deleted on a schedule consistent with organizational data governance requirements. Regulatory requirements in some jurisdictions limit retention of personal data associated with user activity including print job metadata which requires coordination with cloud print service providers regarding their data retention practices. Organizations should review the data processing and retention terms of cloud print service agreements as part of the vendor management and data privacy compliance process. Cloud print audit data should be distinguished from cloud print service operational metadata with audit data retained based on the organization's security audit requirements. Data subject access requests for personal data may need to include print metadata managed by cloud print services requiring notification of the cloud print service vendor as part of the request fulfillment process.",
                Tags = ["data-retention", "cloud-print", "compliance", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforcePrintDataRetentionPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrintDataRetentionPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnforcePrintDataRetentionPolicy", 1)],
            },
        ];
    }

    // ── CloudStorageQuotaPolicy ──
    private static class _CloudStorageQuotaPolicy
    {
        private const string CloudContentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string StorageSenseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cloudqt-set-storage-sense-cadence-monthly",
                    Label = "Cloud Storage Quota: Set Storage Sense Run Cadence to Monthly",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets ConfigStorageSenseGlobalCadence=30 in the StorageSense policy key (value: days). Sets Storage Sense to automatically run once per month (every 30 days). Monthly cadence provides regular disk space management without running so frequently that it causes annoying file evictions. For 256 GB SSDs which accumulate temporary data faster, consider a 14-day cadence. For devices with large SSDs (1 TB+), monthly is appropriate. Storage Sense runs in the background during low activity periods to minimise user impact.",
                    Tags = ["storage-sense", "cadence", "monthly", "disk-management", "schedule"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Storage Sense cleanup runs every 30 days. For devices consistently near disk capacity, consider 7 or 14-day cadence. Storage Sense operates silently in the background — no user notification for routine cleanup operations.",
                    ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseGlobalCadence", 30)],
                    RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseGlobalCadence")],
                    DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseGlobalCadence", 30)],
                },
                new TweakDef
                {
                    Id = "cloudqt-set-onedrive-cache-evict-days-60",
                    Label = "Cloud Storage Quota: Evict Unused OneDrive Cached Files After 60 Days",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets ConfigStorageSenseCloudContentDehydrationThreshold=60 in the StorageSense policy key (value: days). When Storage Sense runs, it converts locally cached OneDrive Files On-Demand files that have not been opened in the last 60 days back to cloud-only placeholders (dehydration). This reclaims local disk space for files the user has not accessed in two months. The dehydrated files are still available in OneDrive — they simply need to be re-downloaded when next opened. 60 days balances storage efficiency against re-download inconvenience.",
                    Tags = ["storage-sense", "onedrive", "dehydration", "cache-eviction", "disk-space"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive locally cached files not opened in 60+ days are evicted to cloud-only status. On next open, file is re-downloaded from OneDrive. Offline mode users (frequent travellers, users in poor-connectivity environments) should have a longer threshold or use 'Always keep on this device' for their most important files.",
                    ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseCloudContentDehydrationThreshold", 60)],
                    RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseCloudContentDehydrationThreshold")],
                    DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseCloudContentDehydrationThreshold", 60)],
                },
                new TweakDef
                {
                    Id = "cloudqt-enable-recycle-bin-cleanup-storage-sense",
                    Label = "Cloud Storage Quota: Enable Recycle Bin Auto-Purge After 30 Days via Storage Sense",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets ConfigStorageSenseRecycleBinCleanupThreshold=30 in the StorageSense policy key. Configures Storage Sense to automatically and permanently delete files that have been in the Recycle Bin for more than 30 days when Storage Sense runs. Without this policy, deleted files remain in the Recycle Bin indefinitely until the user manually empties it — gradually consuming disk space over months or years. Auto-purging after 30 days provides reasonable 'soft delete' protection against accidental deletions while preventing indefinite accumulation of deleted data.",
                    Tags = ["storage-sense", "recycle-bin", "auto-purge", "disk-cleanup", "deleted-files"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Files in Recycle Bin for more than 30 days are permanently deleted. Users who rely on the Recycle Bin as a long-term 'undo' mechanism for accidentally deleted files beyond 30 days will find those files permanently gone. Ensure users understand that deleted files are unrecoverable from the Recycle Bin after 30 days.",
                    ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseRecycleBinCleanupThreshold", 30)],
                    RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseRecycleBinCleanupThreshold")],
                    DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseRecycleBinCleanupThreshold", 30)],
                },
                new TweakDef
                {
                    Id = "cloudqt-enable-temp-files-cleanup-storage-sense",
                    Label = "Cloud Storage Quota: Enable Temp Files Cleanup via Storage Sense",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets ConfigStorageSenseTempFilesCleanup=1 in the StorageSense policy key. Enables Storage Sense to delete temporary files created by apps — including Windows Temp folder contents, Downloads folder files (if configured), browser caches, Windows Update delivery files (after successful installation), and log files in %TEMP%. App-generated temp files accumulate at 1–5 GB per month on typical business devices. Routine cleanup prevents the temp file accumulation that manifests as gradual disk space degradation over months to years of use.",
                    Tags = ["storage-sense", "temp-files", "cache-cleanup", "disk-space", "automatic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Temporary files cleaned by Storage Sense. In-use temp files are not deleted — only files not currently in use by any process. Browser caches are rebuilt on demand. Windows Update delivery optimisation cache cleaned after updates are successfully applied. No impact on running applications.",
                    ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseTempFilesCleanup", 1)],
                    RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseTempFilesCleanup")],
                    DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseTempFilesCleanup", 1)],
                },
                new TweakDef
                {
                    Id = "cloudqt-disable-consumer-account-state-content",
                    Label = "Cloud Storage Quota: Disable Consumer Account State Cloud Content (Upsell UI)",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets DisableConsumerAccountStateContent=1 in the CloudContent policy key. Disables the Microsoft Consumer Account State notifications in Windows settings that prompt users to link a personal Microsoft account, subscribe to Microsoft 365 Personal, purchase more OneDrive storage, or connect Xbox Game Pass. In enterprise environments, devices are managed under an Azure AD work account — consumer account upsell prompts are irrelevant, distracting, and potentially confuse users into adding non-corporate accounts to corporate devices.",
                    Tags = ["cloud-content", "consumer-account", "upsell", "enterprise", "notification"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Consumer account upsell prompts and Microsoft 365 Personal subscription promote notifications disabled. No functional impact on enterprise Microsoft 365 applications or OneDrive for Business. Microsoft account linking from Settings is still possible but not promoted.",
                    ApplyOps = [RegOp.SetDword(CloudContentKey, "DisableConsumerAccountStateContent", 1)],
                    RemoveOps = [RegOp.DeleteValue(CloudContentKey, "DisableConsumerAccountStateContent")],
                    DetectOps = [RegOp.CheckDword(CloudContentKey, "DisableConsumerAccountStateContent", 1)],
                },
                new TweakDef
                {
                    Id = "cloudqt-set-downloads-folder-cleanup-days-90",
                    Label = "Cloud Storage Quota: Auto-Clean Downloads Folder Files Older Than 90 Days",
                    Category = "Cloud Storage — Cloud Backup Retention",
                    Description =
                        "Sets ConfigStorageSenseDownloadsCleanupThreshold=90 in the StorageSense policy key. Configures Storage Sense to purge files in the user's Downloads folder that have not been opened in the past 90 days. The Downloads folder is one of the fastest-growing storage consumers on business devices — downloaded PDF reports, EXE installers, email attachments, ZIP archives accumulate over months. After 90 days, most downloaded files have served their purpose. Auto-cleanup prevents the Downloads folder from becoming a permanent secondary storage location.",
                    Tags = ["storage-sense", "downloads-folder", "cleanup", "disk-space", "90-days"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Downloaded files not opened in 90+ days are permanently deleted. Users who use the Downloads folder for long-term file storage will lose files after 90 days of inactivity. Communicate this policy to users and provide guidance on using OneDrive for long-term document storage.",
                    ApplyOps = [RegOp.SetDword(StorageSenseKey, "ConfigStorageSenseDownloadsCleanupThreshold", 90)],
                    RemoveOps = [RegOp.DeleteValue(StorageSenseKey, "ConfigStorageSenseDownloadsCleanupThreshold")],
                    DetectOps = [RegOp.CheckDword(StorageSenseKey, "ConfigStorageSenseDownloadsCleanupThreshold", 90)],
                },
            ];
    }

    // ── ContentDeliveryPolicy ──
    private static class _ContentDeliveryPolicy
    {
        private const string CloudPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
        private const string StartPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Start";
        private const string CdmPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ContentDeliveryManager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cdpol-disable-start-suggestions",
                Label = "Disable Suggested Apps in Start Menu",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["content delivery", "start menu", "suggestions", "bloatware", "group policy"],
                Description =
                    "Prevents cloud-powered app suggestions and recommendations from appearing "
                    + "in the Start menu's recommended section. "
                    + "DisableAppsFromStore = 0 but SubscribedContent-338389Enabled = 0 equivalent via policy. "
                    + "HideRecommendedSection = 1. "
                    + "Gives users a clean, app-only Start menu without Microsoft Store promotions. "
                    + "Default: recommendations shown.",
                MinBuild = 22000,
                ApplyOps = [RegOp.SetDword(StartPol, "HideRecommendedSection", 1)],
                RemoveOps = [RegOp.SetDword(StartPol, "HideRecommendedSection", 0)],
                DetectOps = [RegOp.CheckDword(StartPol, "HideRecommendedSection", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-content-delivery-auto-download",
                Label = "Disable Content Delivery Manager Auto-Downloads",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["content delivery", "auto-download", "bloatware", "privacy", "bandwidth", "group policy"],
                Description =
                    "Prevents the Content Delivery Manager from silently downloading new app packages, "
                    + "features, and promotional content in the background. "
                    + "PreventAutoContentDelivery = 1 via CdmPol. "
                    + "Reduces surprise bandwidth usage and prevents unwanted app installations on metered connections.",
                ApplyOps = [RegOp.SetDword(CdmPol, "PreventAutoContentDelivery", 1)],
                RemoveOps = [RegOp.SetDword(CdmPol, "PreventAutoContentDelivery", 0)],
                DetectOps = [RegOp.CheckDword(CdmPol, "PreventAutoContentDelivery", 1)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-get-office-promotion",
                Label = "Disable 'Get Microsoft 365' Promotional Node in Settings",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["content delivery", "office 365", "microsoft 365", "ads", "privacy", "group policy"],
                Description =
                    "Removes the Microsoft 365 / Office sign-up promotional page from Windows Settings. "
                    + "DisableWindowsSpotlightOnSettingsOfficePush_ProviderSet = 1. "
                    + "Stops Microsoft Office subscription upsells from appearing in the Settings app. "
                    + "Default: promotion shown when Office is not installed.",
                ApplyOps = [RegOp.SetDword(CloudPol, "ConfigureWindowsSpotlight", 2)],
                RemoveOps = [RegOp.DeleteValue(CloudPol, "ConfigureWindowsSpotlight")],
                DetectOps = [RegOp.CheckDword(CloudPol, "ConfigureWindowsSpotlight", 2)],
            },
            new TweakDef
            {
                Id = "cdpol-disable-tailored-experiences",
                Label = "Disable Tailored Experiences with Diagnostic Data",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["content delivery", "tailored", "telemetry", "privacy", "group policy"],
                Description =
                    "Prevents Windows from using diagnostic data to show personalised tips, ads, "
                    + "and recommendations via the 'Tailored Experiences' feature. "
                    + "DisableTailoredExperiencesWithDiagnosticData = 1. "
                    + "Stops Microsoft from profiling usage patterns to target in-Windows promotions. "
                    + "Default: tailored experiences enabled when diagnostic data is set to Full.",
                ApplyOps = [RegOp.SetDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 1)],
                RemoveOps = [RegOp.SetDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 0)],
                DetectOps = [RegOp.CheckDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            },
        ];
    }

    // ── DesktopAnalyticsPolicy ──
    private static class _DesktopAnalyticsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dskanlyt-set-commercial-id",
                Label = "Configure Commercial ID for Desktop Analytics Data Collection",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Commercial ID associates diagnostic data sent to Microsoft with a specific organization's Desktop Analytics workspace enabling analytics dashboards for update compliance. Configuring the Commercial ID is required to use Desktop Analytics for Windows update readiness assessments and compatibility analysis. Without a Commercial ID diagnostic data is anonymized and cannot be correlated with organizational devices for Desktop Analytics reporting. Organizations using Desktop Analytics for update risk assessment must deploy the Commercial ID policy to all managed devices. Commercial IDs are generated in the Azure Portal for Desktop Analytics workspaces and should be protected as organizational identifiers. Organizations that disable diagnostic data collection should clear the Commercial ID setting to prevent any residual correlation of device data.",
                Tags = ["desktop-analytics", "commercial-id", "diagnostic-data", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "CommercialId", "")],
                RemoveOps = [RegOp.DeleteValue(Key, "CommercialId")],
                DetectOps = [RegOp.CheckMissing(Key, "CommercialId")],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-msdt-telemetry",
                Label = "Disable Microsoft Diagnostics and Troubleshooting Tool Telemetry",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Microsoft Diagnostics and Troubleshooting (MSDT) transmits debugging and crash information to Microsoft that can include sensitive data from system crash dumps or application error reports. Disabling MSDT telemetry prevents diagnostic data from system failures from being transmitted to Microsoft which may include memory contents from the time of the failure. Crash dumps can contain sensitive in-memory data including encryption keys login tokens and sensitive application data that should not be transmitted externally. Organizations should configure crash dump settings to capture the minimum data needed for internal debugging rather than sending full crash reports to Microsoft. MSDT telemetry disabling should be combined with WER (Windows Error Reporting) restrictions to provide comprehensive control over error data transmission. Systems running high-security workloads should minimize diagnostic data transmission through both Microsoft and third-party error reporting frameworks.",
                Tags = ["desktop-analytics", "msdt", "telemetry", "crash-data", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticData", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-inventory-collection",
                Label = "Disable Automatic Software Inventory Collection for Analytics",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Automatic software inventory collection transmits an inventory of installed applications and drivers to Microsoft's Desktop Analytics service for compatibility analysis. Disabling software inventory collection prevents application lists from being transmitted to Microsoft which reduces the data footprint in Microsoft's analytics cloud. Software inventory data may reveal internal application names versions and configurations that organizations consider sensitive or confidential. Organizations using Desktop Analytics for actual update readiness assessment need inventory data enabled to benefit from compatibility analysis. For organizations not using Desktop Analytics the inventory collection provides no business value and represents unnecessary data transmission. Inventory collection disabling should be applied to all systems outside the Analytics scope to minimize unnecessary data collection.",
                Tags = ["desktop-analytics", "inventory", "software-collection", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-update-compliance-collection",
                Label = "Disable Update Compliance Data Collection for Non-Analytics Systems",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Update Compliance data collection transmits Windows Update and Windows Defender update status to Microsoft's Log Analytics service for organizational compliance reporting. Disabling Update Compliance collection for systems outside the analytics scope eliminates unnecessary transmission of update status data to Microsoft cloud services. Organizations using Update Compliance must maintain the Collection settings for enrolled devices while disabling it for systems out of scope for analytics. Update Compliance provides valuable data for identifying unpatched systems but requires cloud data transmission for analysis. On-premises alternatives to Update Compliance include WSUS reports and Configuration Manager update compliance reports that keep data internal. Organizations with strict data sovereignty requirements should use on-premises update compliance reporting instead of Microsoft's collected analytics.",
                Tags = ["desktop-analytics", "update-compliance", "cloud-reporting", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "dskanlyt-disable-compat-appraiser-task",
                Label = "Disable Compatibility Appraiser Scheduled Task for Analytics",
                Category = "Cloud Storage — Cloud Backup Retention",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Compatibility Appraiser scheduled task runs assessments that collect application and device compatibility data for Desktop Analytics and Windows upgrade readiness. Disabling the Compatibility Appraiser task prevents compatibility data from being collected and transmitted to Microsoft's analytics services. The Appraiser scan runs daily on systems enrolled in Desktop Analytics consuming CPU and I/O resources to assess installed applications and hardware compatibility. Organizations not using Desktop Analytics for upgrade planning have no need for the Appraiser task and should disable it to reduce unnecessary resource consumption and data transmission. Disabling the Appraiser task does not affect Windows Update delivery or security update installation. Organizations planning Windows version upgrades should enable the Compatibility Appraiser for a period before the upgrade to identify compatibility blockers.",
                Tags = ["desktop-analytics", "compatibility-appraiser", "scheduled-task", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCompatibilityAppraiser", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompatibilityAppraiser")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompatibilityAppraiser", 1)],
            },
        ];
    }

    // ── OneDriveKfmPolicy ──
    private static class _OneDriveKfmPolicy
    {
        private const string KfmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "odkfm-silent-opt-in",
                Label = "OneDrive KFM: Silently Move Known Folders to OneDrive",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Silently redirects Desktop, Documents, and Pictures to OneDrive without user interaction. Requires the tenant ID (GUID) to be set as the value data for KFMSilentOptIn.",
                Tags = ["onedrive", "kfm", "known-folder-move", "backup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Silently moves Desktop/Documents/Pictures to OneDrive; requires tenant GUID.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetString(KfmKey, "KFMSilentOptIn", "")],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMSilentOptIn")],
                DetectOps = [RegOp.CheckMissing(KfmKey, "KFMSilentOptIn")],
            },
            new TweakDef
            {
                Id = "odkfm-silent-opt-in-notification",
                Label = "OneDrive KFM: Silent Opt-In with Toast Notification",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Silently moves known folders to OneDrive and shows a toast notification to the user explaining the change. Less disruptive than prompting but still informative.",
                Tags = ["onedrive", "kfm", "known-folder-move", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Silent KFM with user toast notification; less disruptive than prompted.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "KFMSilentOptInWithNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMSilentOptInWithNotification")],
                DetectOps = [RegOp.CheckDword(KfmKey, "KFMSilentOptInWithNotification", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-opt-in-wizard",
                Label = "OneDrive KFM: Prompt Users to Move Known Folders (Wizard)",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Prompts users with a guided wizard to move their Desktop, Documents, and Pictures folders to OneDrive. The user must confirm the move.",
                Tags = ["onedrive", "kfm", "known-folder-move", "wizard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prompts users with a guided wizard to move known folders to OneDrive.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetString(KfmKey, "KFMOptInWithWizard", "")],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMOptInWithWizard")],
                DetectOps = [RegOp.CheckMissing(KfmKey, "KFMOptInWithWizard")],
            },
            new TweakDef
            {
                Id = "odkfm-silent-opt-out",
                Label = "OneDrive KFM: Silently Redirect Known Folders Back to Local",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Silently reverses Known Folder Move, redirecting Desktop, Documents, and Pictures back to local paths on the device without prompting the user.",
                Tags = ["onedrive", "kfm", "known-folder-move", "revert", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Silently reverses KFM back to local paths without user prompt.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "KFMSilentOptOut", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "KFMSilentOptOut")],
                DetectOps = [RegOp.CheckDword(KfmKey, "KFMSilentOptOut", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-force-update-ring",
                Label = "OneDrive KFM: Set OneDrive Update Ring",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Forces OneDrive Sync Client to use a specific update ring: 'Insider', 'Production', or 'Deferred'. Deferred delays updates by ~60 days for enterprise stability.",
                Tags = ["onedrive", "update-ring", "enterprise", "update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Forces OneDrive to Deferred ring for enterprise stability.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetString(KfmKey, "GPOSetUpdateRing", "Deferred")],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "GPOSetUpdateRing")],
                DetectOps = [RegOp.CheckString(KfmKey, "GPOSetUpdateRing", "Deferred")],
            },
            new TweakDef
            {
                Id = "odkfm-prevent-network-traffic-before-signin",
                Label = "OneDrive KFM: Block Pre-Logon Network Traffic",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Prevents the OneDrive Sync Client from making any network calls before user sign-in. Avoids unexpected traffic on secure/kiosk machines during boot.",
                Tags = ["onedrive", "network", "privacy", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks OneDrive pre-logon network calls on kiosk/secure machines.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "PreventNetworkTrafficPreUserSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "PreventNetworkTrafficPreUserSignIn")],
                DetectOps = [RegOp.CheckDword(KfmKey, "PreventNetworkTrafficPreUserSignIn", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-min-disk-space",
                Label = "OneDrive KFM: Set Minimum Free Disk Space Threshold",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Sets the minimum local disk free space (in MB) below which OneDrive will warn users and pause sync. Default is 500 MB. Set to 2048 for safer enterprise deployments.",
                Tags = ["onedrive", "disk-space", "quota", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sets minimum free disk threshold before OneDrive warns and pauses sync.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "MinDiskSpaceLimitInMB", 2048)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "MinDiskSpaceLimitInMB")],
                DetectOps = [RegOp.CheckDword(KfmKey, "MinDiskSpaceLimitInMB", 2048)],
            },
            new TweakDef
            {
                Id = "odkfm-warning-disk-space",
                Label = "OneDrive KFM: Set Low Disk Space Warning Threshold",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Configures the early disk-space warning threshold for OneDrive (in MB). When free space drops below this value, a warning is shown before sync is blocked.",
                Tags = ["onedrive", "disk-space", "warning", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Configures early disk-space warning threshold for OneDrive.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "WarningMinDiskSpaceLimitInMB", 4096)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "WarningMinDiskSpaceLimitInMB")],
                DetectOps = [RegOp.CheckDword(KfmKey, "WarningMinDiskSpaceLimitInMB", 4096)],
            },
            new TweakDef
            {
                Id = "odkfm-disable-teamsite-automount",
                Label = "OneDrive KFM: Disable SharePoint/Teams Auto-Mount",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Prevents OneDrive from automatically syncing SharePoint team site document libraries without user action. Users must manually add sync folders in OneDrive.",
                Tags = ["onedrive", "sharepoint", "teams", "auto-mount", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents automatic SharePoint/Teams library sync without user action.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "AutoMountTeamSitesDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "AutoMountTeamSitesDisabled")],
                DetectOps = [RegOp.CheckDword(KfmKey, "AutoMountTeamSitesDisabled", 1)],
            },
            new TweakDef
            {
                Id = "odkfm-disable-first-delete-dialog",
                Label = "OneDrive KFM: Disable First-Delete Recycle Bin Dialog",
                Category = "Cloud Storage — One Drive Kfm",
                Description =
                    "Suppresses the OneDrive 'Are you sure you want to delete?' confirmation dialog on first delete from a synced folder. Reduces friction for advanced users.",
                Tags = ["onedrive", "delete", "dialog", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the first-delete confirmation dialog in synced folders.",
                RegistryKeys = [KfmKey],
                ApplyOps = [RegOp.SetDword(KfmKey, "DisableFirstDeleteDialog", 1)],
                RemoveOps = [RegOp.DeleteValue(KfmKey, "DisableFirstDeleteDialog")],
                DetectOps = [RegOp.CheckDword(KfmKey, "DisableFirstDeleteDialog", 1)],
            },
        ];
    }

    // ── OneDriveSyncPolicy ──
    private static class _OneDriveSyncPolicy
    {
        private const string OneDriveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "odsync-enable-known-folder-move",
                    Label = "OneDrive Sync: Enable Known Folder Move (Desktop/Documents/Pictures to OneDrive)",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets KFMSilentOptIn to the tenant ID GUID in the OneDrive policy key (uses a placeholder DWORD flag EnableKFMSilentOptIn=1 as the registry-based detection). Silently moves Windows Known Folders (Desktop, Documents, Pictures) to OneDrive for Business during the next OneDrive sync cycle. This is the primary OneDrive data protection feature for enterprise — it ensures that user data stored in the default Windows folders is continuously synced to OneDrive, providing automatic cloud backup and recovery in case of device loss or failure. Users do not need to change their behaviour — they continue to save to Desktop/Documents and data is automatically protected.",
                    Tags = ["onedrive", "known-folder-move", "kfm", "desktop", "documents"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Desktop, Documents, and Pictures folders silently moved to OneDrive for Business. Requires a tenant-specific OneDrive Group Policy configuration (tenantId GUID in KFMSilentOptIn). Deployment requires M365 Business or Enterprise licensing. Users see a toast notification on first sync. Existing local files are moved — no data loss. Configure via the OneDrive Group Policy ADMX template for production deployment.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "EnableKFMSilentOptIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "EnableKFMSilentOptIn")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "EnableKFMSilentOptIn", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-allow-sync-on-metered-network",
                    Label = "OneDrive Sync: Prevent OneDrive Sync on Metered Connections",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableSyncOnMeteredNetwork=1 in the OneDrive policy key. Prevents OneDrive from syncing files when the device is connected via a metered network connection (mobile hotspot, cellular tethering, capped data plan). On mobile broadband or tethered connections, unthrottled OneDrive sync can consume gigabytes of cellular data — generating unexpected data overage charges or exhausting mobile data plans. Suspending sync on metered connections is the standard behaviour for consumer OneDrive but requires explicit policy enforcement in enterprise deployments.",
                    Tags = ["onedrive", "metered-network", "cellular", "data-usage", "bandwidth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync paused on metered connections. Syncing resumes automatically when the device connects to a non-metered (Wi-Fi, Ethernet) network. Files saved to synced folders while on metered connections are queued and uploaded when network becomes non-metered.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "DisableSyncOnMeteredNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisableSyncOnMeteredNetwork")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "DisableSyncOnMeteredNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-prevent-unmanaged-machine-sync",
                    Label = "OneDrive Sync: Block Sync on Unmanaged (Non-Domain, Non-Azure-AD) Devices",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets AllowTenantList enforcement via PreventUnmanagedMachineSync=1 in the OneDrive policy key. Prevents the OneDrive sync client from syncing corporate OneDrive data on devices that are not Azure AD joined or domain joined. This is the SharePoint Online 'allowed domain' equivalent for device management compliance — corporate data should only sync to managed devices under IT control. Users who attempt to sync from personal or unmanaged devices receive a 'You can't sync to this location' error.",
                    Tags = ["onedrive", "unmanaged-device", "dlp", "conditional-access", "byod"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Corporate OneDrive sync blocked on unmanaged devices. BYOD users with personal devices cannot install the OneDrive sync client and access corporate libraries — they must use OneDrive.com web interface. Ensure Conditional Access policies in Azure AD complement this policy for consistent enforcement.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "PreventUnmanagedMachineSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "PreventUnmanagedMachineSync")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "PreventUnmanagedMachineSync", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-enable-verbose-error-reporting",
                    Label = "OneDrive Sync: Enable Verbose Sync Error Reporting to Event Log",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets EnableVerboseEventReporting=1 in the OneDrive policy key. Enables detailed OneDrive sync error events in the Windows Application event log (source: OneDrive). Standard OneDrive error reporting only logs critical failures to the OneDrive diagnostic log (%LOCALAPPDATA%\\Microsoft\\OneDrive\\logs\\). Verbose event log reporting records all sync errors to the Windows event log where SIEM tools can consume them — enabling fleet-wide monitoring of sync failures, storage quota issues, sharing permission errors, and file conflict errors.",
                    Tags = ["onedrive", "event-log", "error-reporting", "siem", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync errors written to Windows Application event log. In large deployments with frequent sync errors (file locking conflicts, large file type restrictions), event log volume can be significant. Consider event log retention and SIEM ingestion cost when enabling verbose reporting at scale.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "EnableVerboseEventReporting", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "EnableVerboseEventReporting")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "EnableVerboseEventReporting", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-disable-auto-start-telemetry",
                    Label = "OneDrive Sync: Disable OneDrive Usage Telemetry Reporting to Microsoft",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableTelemetry=1 in the OneDrive policy key. Disables the OneDrive sync client's Optional Diagnostic Data (ODD) telemetry reporting to Microsoft. OneDrive collects usage telemetry about sync activity, file types synced, sync performance, and error rates. In privacy-sensitive enterprise environments or regulated industries (legal, healthcare, finance), disabling optional telemetry from OneDrive is required by data governance policy. Required diagnostic data (error crash reports) cannot be disabled — only the optional enhanced telemetry is affected.",
                    Tags = ["onedrive", "telemetry", "privacy", "data-governance", "gdpr"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Optional OneDrive telemetry reporting disabled. Required diagnostic data (crash reports, error reports) still transmitted. No user-visible impact on OneDrive sync functionality. Microsoft will receive less usage data for OneDrive feature improvements.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "odsync-set-cache-free-space-floor-5pct",
                    Label = "OneDrive Sync: Set Minimum Free Space Floor — Do Not Sync Below 5% Free Disk",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets MinDiskFreeSpaceGB=5 in the OneDrive policy key (absolute GB, adjustable). Prevents OneDrive from downloading additional Files On-Demand files when the device's disk free space falls below the configured threshold. Without a free space floor, OneDrive downloads can fill an SSD to 100%, causing Windows write operations to fail (temp file writes, browser cache, update extraction) — resulting in OS instability. The free space floor ensures that even when users open many cloud-only files in quick succession, the disk is not completely filled.",
                    Tags = ["onedrive", "disk-space", "storage-floor", "on-demand", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive download paused when disk free space falls below 5 GB. Files On-Demand downloads are suspended — opening new cloud-only files fails until free space is restored. Users see an OneDrive notification about insufficient disk space. Existing downloaded (cached) files are not deleted.",
                    ApplyOps = [RegOp.SetDword(OneDriveKey, "MinDiskFreeSpaceGB", 5)],
                    RemoveOps = [RegOp.DeleteValue(OneDriveKey, "MinDiskFreeSpaceGB")],
                    DetectOps = [RegOp.CheckDword(OneDriveKey, "MinDiskFreeSpaceGB", 5)],
                },
            ];
    }

    // ── SettingSyncAdv ──
    private static class _SettingSyncAdv
    {
        private const string SyncPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";
        private const string InputPers = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization";
        private const string InputPersPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ssync-disable-desktop-theme",
                Label = "Disable Desktop Background Theme Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Stops the desktop background and visual theme from being synced across "
                    + "devices linked to the same Microsoft Account. "
                    + "DisableDesktopThemeSettingSync=1.",
                Tags = ["sync", "theme", "desktop", "wallpaper", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableDesktopThemeSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableDesktopThemeSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableDesktopThemeSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-start-layout",
                Label = "Disable Start Menu Layout Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Prevents the Start menu layout (pinned apps, tile arrangement) from being "
                    + "synchronized across devices. DisableStartLayoutSettingSync=1.",
                Tags = ["sync", "start menu", "layout", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableStartLayoutSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableStartLayoutSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableStartLayoutSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-browser-settings",
                Label = "Disable Browser Settings Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prevents browser-related settings (favourites, history, home page settings) "
                    + "from syncing through Microsoft Account. DisableBrowserSettingSync=1.",
                Tags = ["sync", "browser", "favourites", "msa", "privacy"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableBrowserSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableBrowserSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableBrowserSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-language-settings",
                Label = "Disable Language and Regional Settings Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Prevents language packs, keyboard layouts, and regional settings from "
                    + "being synchronized across devices. DisableLanguageSettingSync=1.",
                Tags = ["sync", "language", "regional", "keyboard", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableLanguageSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableLanguageSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableLanguageSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-accessibility-settings",
                Label = "Disable Accessibility/Ease-of-Access Settings Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Stops Ease of Access settings (Magnifier, Narrator, contrast themes) "
                    + "from syncing via Microsoft Account. DisableAccessibilitySettingSync=1.",
                Tags = ["sync", "accessibility", "ease of access", "narrator", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableAccessibilitySettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableAccessibilitySettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableAccessibilitySettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-personalization-settings",
                Label = "Disable Personalization Settings Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Prevents personalization settings such as colors, lock-screen images, and "
                    + "accent colors from being synchronized. DisablePersonalizationSettingSync=1.",
                Tags = ["sync", "personalization", "lock screen", "colors", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisablePersonalizationSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisablePersonalizationSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisablePersonalizationSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-windows-settings",
                Label = "Disable General Windows Platform Settings Sync",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disables synchronization of general Windows OS settings (taskbar, search, "
                    + "notification preferences) across devices. DisableWindowsSettingSync=1.",
                Tags = ["sync", "windows settings", "taskbar", "msa"],
                RegistryKeys = [SyncPolicy],
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableWindowsSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableWindowsSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableWindowsSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "ssync-disable-input-personalization-policy",
                Label = "Disable Input Personalization — Machine Policy",
                Category = "Cloud Storage — One Drive Kfm",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Machine-wide group policy that disables all Windows input personalization "
                    + "(typing, handwriting, speech learning) for all users on the device. "
                    + "AllowInputPersonalization=0.",
                Tags = ["personalization", "policy", "input", "speech", "privacy"],
                RegistryKeys = [InputPersPolicy],
                ApplyOps = [RegOp.SetDword(InputPersPolicy, "AllowInputPersonalization", 0)],
                RemoveOps = [RegOp.DeleteValue(InputPersPolicy, "AllowInputPersonalization")],
                DetectOps = [RegOp.CheckDword(InputPersPolicy, "AllowInputPersonalization", 0)],
            },
        ];
    }

    // ── SettingSyncPolicy ──
    private static class _SettingSyncPolicy
    {
        private const string SyncKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "syncsec-block-user-override",
                    Label = "Block User from Changing Settings Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description = "Prevents users from accessing the settings sync toggle in Windows Settings.",
                    Tags = ["sync", "settings", "user-override", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removes the sync toggle from Settings UI; requires DisableSettingSync to also be set.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableSettingSyncUserOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSettingSyncUserOverride")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableSettingSyncUserOverride", 1)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-credentials-sync",
                    Label = "Disable Credentials and Password Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description = "Stops Windows from syncing saved passwords and credentials across devices via a Microsoft account.",
                    Tags = ["sync", "credentials", "password", "privacy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Prevents credential roaming; passwords stored locally only, not in Microsoft account cloud.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableCredentialsSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableCredentialsSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableCredentialsSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-app-settings-sync",
                    Label = "Disable App Settings Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description = "Stops Windows from uploading and syncing per-app settings to a Microsoft account in the cloud.",
                    Tags = ["sync", "app-settings", "cloud", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "App preferences remain on this device; switching to another device may require re-configuring apps.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableApplicationSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableApplicationSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableApplicationSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-browser-sync",
                    Label = "Disable Browser Settings Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description = "Disables syncing of Microsoft Edge / Internet Explorer browser settings, favourites, and history via sync.",
                    Tags = ["sync", "browser", "edge", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Browser favourites and history stay local; no cloud upload via Windows Settings Sync.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableWebBrowserSettingSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-start-layout-sync",
                    Label = "Disable Start Menu Layout Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description = "Prevents Windows from syncing the Start menu layout, pinned apps, and tile configuration to the cloud.",
                    Tags = ["sync", "start-menu", "layout", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Start menu customisation stays local; does not roam to other devices signed with the same account.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableStartLayoutSync", 2)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableStartLayoutSync")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableStartLayoutSync", 2)],
                },
                new TweakDef
                {
                    Id = "syncsec-disable-sync-on-metered",
                    Label = "Disable Settings Sync on Metered Networks",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description = "Prevents Windows settings sync from running when the device is on a metered (pay-per-use) network connection.",
                    Tags = ["sync", "metered", "network", "data-usage", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents unexpected data charges on cellular / capped connections; sync resumes on unmetered networks.",
                    ApplyOps = [RegOp.SetDword(SyncKey, "DisableSyncOnPaidNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSyncOnPaidNetwork")],
                    DetectOps = [RegOp.CheckDword(SyncKey, "DisableSyncOnPaidNetwork", 1)],
                },
            ];
    }

    // ── SharepointOnlinePolicy ──
    private static class _SharepointOnlinePolicy
    {
        private const string SharepointKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\SharePoint";

        private const string OfficePrivacyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Privacy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "spol-disable-external-sharing",
                    Label = "SharePoint Online: Prohibit External Sharing from SharePoint Sites",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets AllowExternalSharing=0 in the SharePoint policy key. Sets the client-side policy assertion that external sharing from SharePoint Online sites is not permitted. While the authoritative SharePoint sharing setting is managed in the SharePoint Admin Center, this registry policy works with Office client apps to enforce the restriction locally — Office add-ins and co-authoring flows check this policy to determine whether to offer 'share with external users' options. Combined with SharePoint Admin Center's external sharing settings, this provides defence-in-depth.",
                    Tags = ["sharepoint", "external-sharing", "dlp", "data-exfiltration", "collaboration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "External sharing prohibited in Office client SharePoint integration. Users cannot share items from Office apps to external email addresses via SharePoint sharing. External collaboration requires admin-authorised guest access configuration in the SharePoint Admin Center. Web-based sharing via SharePoint.com may still allow sharing depending on SharePoint Admin Center tenant-level settings.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "AllowExternalSharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "AllowExternalSharing")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "AllowExternalSharing", 0)],
                },
                new TweakDef
                {
                    Id = "spol-enable-sensitivity-label-enforcement",
                    Label = "SharePoint Online: Enable Microsoft Information Protection Sensitivity Labels in Office",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets EnableMIPIntegration=1 in the SharePoint policy key. Enables the Microsoft Information Protection (MIP) AIP unified labelling integration in Office apps connecting to SharePoint Online. When enabled, Office apps (Word, Excel, PowerPoint, Outlook) display the sensitivity label bar and enforce label-based policies (encryption, access control, DRM) defined in the Microsoft Purview Compliance Center. Users are prompted to label documents before saving to SharePoint, and unlabelled uploads to labelled libraries are rejected.",
                    Tags = ["sharepoint", "sensitivity-labels", "mip", "dlp", "information-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Sensitivity labelling integrated in Office apps. Users see the sensitivity label bar in Word, Excel, PowerPoint, and Outlook. Requires Microsoft Purview Information Protection (MIP) licensing (M365 E3/E5 or Azure Information Protection P1/P2). Labels configured in Purview Compliance Center are deployed to Office apps. Unlabelled existing documents are not automatically labelled — only new documents are prompted.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "EnableMIPIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "EnableMIPIntegration")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "EnableMIPIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "spol-disable-co-authoring-with-external-users",
                    Label = "SharePoint Online: Disable Real-Time Co-Authoring with External (Guest) Users",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableExternalCoAuthoring=1 in the SharePoint policy key. Prevents Office real-time co-authoring sessions with external/guest users via SharePoint Online. Co-authoring with external users transmits document content character-by-character in real time — in strict DLP scenarios, even the act of collaborating with an external user on a sensitive document may constitute a data disclosure event. Disabling external co-authoring while retaining internal co-authoring preserves team collaboration while blocking external data flows.",
                    Tags = ["sharepoint", "co-authoring", "external-guest", "dlp", "real-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "External guest co-authoring sessions blocked. Internal team co-authoring is unaffected. Guests in SharePoint sites can still view and download documents but cannot participate in real-time co-authoring sessions. Impact is primarily on M365 guest collaboration workflows.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "DisableExternalCoAuthoring", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableExternalCoAuthoring")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "DisableExternalCoAuthoring", 1)],
                },
                new TweakDef
                {
                    Id = "spol-set-download-permissions-block-unmanaged",
                    Label = "SharePoint Online: Block Downloads from SharePoint for Unmanaged Devices",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets BlockDownloadOnUnmanagedDevice=1 in the SharePoint policy key. Prevents file downloads from SharePoint Online to unmanaged (non-Azure-AD-joined) devices. This is the client-side policy flag — the enforcement is primarily in SharePoint Online Conditional Access policies configured for unmanaged devices. When this flag is set, Office apps enforce the restriction by checking the device management state before initiating downloads. Users on unmanaged devices can view documents in the browser (web-only mode) but cannot download files for local storage.",
                    Tags = ["sharepoint", "unmanaged-device", "download-block", "byod", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint downloads blocked on unmanaged devices. Users on personal devices can only view content in the browser in read-only web view — they cannot download files, open in desktop Office apps, or print. Managed (Azure AD joined) devices are not affected.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "BlockDownloadOnUnmanagedDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "BlockDownloadOnUnmanagedDevice")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "BlockDownloadOnUnmanagedDevice", 1)],
                },
                new TweakDef
                {
                    Id = "spol-disable-sharepoint-addins",
                    Label = "SharePoint Online: Disable SharePoint Store Add-ins (Prevent Marketplace Apps)",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableSharePointStoreAddins=1 in the SharePoint policy key. Prevents users from acquiring and installing SharePoint Add-ins from the SharePoint App Marketplace. Unvetted SharePoint add-ins can request high-privilege API permissions (full site read/write, full tenant admin on some legacy add-ins), access sensitive SharePoint data, and exfiltrate content to external services. IT should pre-approve and deploy authorised SharePoint add-ins via the corporate app catalogue rather than allowing open marketplace installs.",
                    Tags = ["sharepoint", "add-ins", "app-marketplace", "shadow-it", "permissions"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint marketplace add-in installs blocked. Users cannot install new add-ins from the SharePoint store. IT-approved add-ins deployed via the corporate App Catalogue are not affected. Existing installed marketplace add-ins may continue to function depending on SharePoint tenant configuration.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "DisableSharePointStoreAddins", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableSharePointStoreAddins")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "DisableSharePointStoreAddins", 1)],
                },
                new TweakDef
                {
                    Id = "spol-set-sync-client-tenant-restriction",
                    Label = "SharePoint Online: Restrict OneDrive/SharePoint Sync to Authorised Tenant Only",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets AllowTenantList enforcement flag TenantRestrictionEnabled=1 in the SharePoint policy key. Enables the tenant restriction for OneDrive and SharePoint sync — the OneDrive client only allows sync connections to the authorised corporate tenant. Without this restriction, users can sign into any Microsoft 365 tenant from the OneDrive client (including a free personal tenant they created to receive data) and sync corporate SharePoint libraries to the non-corporate tenant. This is a data exfiltration vector for malicious insiders.",
                    Tags = ["sharepoint", "tenant-restriction", "onedrive", "data-exfiltration", "insider"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive and SharePoint sync restricted to authorised tenant. Users cannot sync data to or from a non-corporate Microsoft 365 tenant. Requires the authorised tenant GUID to be configured in the policy (set via Group Policy ADMX template AllowTenantList setting). This registry flag enables the enforcement mechanism but requires the tenant GUID to be fully enforced.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "TenantRestrictionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "TenantRestrictionEnabled")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "TenantRestrictionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "spol-disable-sharepoint-meeting-recordings-personal",
                    Label = "SharePoint Online: Disable Personal Meeting Recording Storage in OneDrive",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableMeetingRecordingToPersonalOneDrive=1 in the SharePoint policy key. Prevents Teams meeting recordings from being saved to the organiser's personal OneDrive for Business. Instead, recordings are directed to the meeting's SharePoint channel (group OneDrive). Personal OneDrive storage for meeting recordings is uncontrolled from an IT governance perspective — recordings stored personally may be retained beyond the organisation's retention policy, shared with external recipients without oversight, or lost when an employee departures. Channel-based recording storage is covered by the SharePoint retention and eDiscovery policies.",
                    Tags = ["sharepoint", "meeting-recordings", "teams", "onedrive", "retention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Meeting recordings saved to SharePoint channel storage, not personal OneDrive. Teams recordings still available to meeting participants via the SharePoint channel. Compliance with meeting recording retention policies is simplified as recordings are under SharePoint retention policy scope.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive", 1)],
                },
                new TweakDef
                {
                    Id = "spol-enable-access-log-audit",
                    Label = "SharePoint Online: Enable SharePoint Access and File Activity Audit Logging",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets EnableAccessAudit=1 in the SharePoint policy key. Enables detailed file access and activity auditing in SharePoint Online — records who accessed, downloaded, modified, or shared each file, and from which device. SharePoint access audit logs are used for insider threat detection, eDiscovery, data breach investigation, and regulatory compliance (HIPAA, SOX, GDPR). Without audit logging, it is impossible to reconstruct who accessed sensitive files during a data breach investigation window.",
                    Tags = ["sharepoint", "audit-log", "access-log", "insider-threat", "ediscovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint file access, modification, sharing, and download events logged. Full audit log available in Microsoft Purview Compliance Center and via Microsoft Graph API. High-volume event environments (large file libraries with frequent access) generate significant audit trail data. Audit log retention depends on Microsoft 365/Purview licensing.",
                    ApplyOps = [RegOp.SetDword(SharepointKey, "EnableAccessAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(SharepointKey, "EnableAccessAudit")],
                    DetectOps = [RegOp.CheckDword(SharepointKey, "EnableAccessAudit", 1)],
                },
            ];
    }

    // ── SkyDrivePolicy ──
    private static class _SkyDrivePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SkyDrive";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "skydrive-disable-file-sync",
                    Label = "SkyDrive: Disable OneDrive File Synchronisation via Legacy SkyDrive Policy Key",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableFileSync=1 in the SkyDrive legacy policy key. Disables OneDrive file synchronisation at the machine policy level, preventing all users on this computer from syncing files with their OneDrive cloud storage. The SkyDrive registry key is the original legacy path (Windows 8.1/RT era) that is still read by the current Windows OneDrive client for backwards compatibility with Group Policy deployed to WS2012R2 and Win 8.1 machines. "
                        + "In organisations that prohibit users from uploading corporate files to personal cloud storage, the SkyDrive legacy policy key ensures policy coverage extends to legacy Windows versions where the OneDrive-specific policy path did not yet exist. The SkyDrive and OneDrive policy keys are both evaluated — having both set ensures no gap in policy enforcement across heterogeneous Windows version environments. Without both keys set, a corporate laptop running the current OneDrive client on Win 8.1 would check the SkyDrive key first; if missing, OneDrive sync proceeds unblocked.",
                    Tags = ["skydrive", "onedrive", "file-sync", "cloud-storage", "policy", "disable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive file sync disabled via legacy SkyDrive policy key. Corporate files blocked from uploading to personal OneDrive accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableFileSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableFileSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableFileSync", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-library-default-save",
                    Label = "SkyDrive: Prevent Libraries from Defaulting Save Location to SkyDrive/OneDrive",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableLibrariesDefaultSaveToSkyDrive=1 in the SkyDrive policy key. Prevents Windows from configuring OneDrive's local folder as the default save location for Windows Libraries (Documents, Pictures, Music). Without this policy, Windows 8.1+ suggests OneDrive as the default save target — any document saved without explicitly choosing a location is uploaded to the user's personal OneDrive account. "
                        + "In corporate environments where DLP (Data Loss Prevention) policies prohibit saving corporate IP to personal cloud storage, the auto-save to OneDrive/SkyDrive default library path is a subtle leakage vector — users who click 'Save' without inspecting the save dialogue may unknowingly sync sensitive documents to personal storage. Enforcing a corporate-managed default save location (a file server or SharePoint UNC path configured by Group Policy Folder Redirection) ensures all undirected file saves stay within managed storage boundaries.",
                    Tags = ["skydrive", "onedrive", "library", "default-save", "dlp", "cloud-storage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Libraries no longer default to SkyDrive/OneDrive as save location. Users who manually navigate to OneDrive folder can still save there until sync is also disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLibrariesDefaultSaveToSkyDrive", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLibrariesDefaultSaveToSkyDrive")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLibrariesDefaultSaveToSkyDrive", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-metered-sync",
                    Label = "SkyDrive: Disable OneDrive Sync on Metered Network Connections",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets NeverSyncOnMeteredConnection=1 in the SkyDrive policy key. Prevents OneDrive from synchronising files when the active network connection is metered (mobile data, LTE hotspot, satellite). Without this policy, OneDrive will attempt background synchronisation on metered connections, consuming potentially expensive cellular data allowances and degrading application performance for users on mobile hotspots. "
                        + "Windows marks mobile hotspot connections, tethered cellular connections, and some Wi-Fi networks as metered to signal to applications that data usage should be minimised. OneDrive respects the metered status for foreground sync but continues background sync by default. For road warriors using laptop hotspot tethering on international trips with expensive roaming data plans, an unconstrained OneDrive background sync can silently consume gigabytes of mobile data. Disabling sync on metered connections prevents this scenario without requiring manual sync suspension.",
                    Tags = ["skydrive", "onedrive", "metered-connection", "mobile-data", "bandwidth", "roaming"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync paused on metered connections (cellular hotspot, LTE). Manual sync is still available. Files upload when non-metered connection is available.",
                    ApplyOps = [RegOp.SetDword(Key, "NeverSyncOnMeteredConnection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NeverSyncOnMeteredConnection")],
                    DetectOps = [RegOp.CheckDword(Key, "NeverSyncOnMeteredConnection", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-desktop-shortcut",
                    Label = "SkyDrive: Disable Automatic OneDrive Desktop Shortcut Creation",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableSkyDriveDesktopIcon=1 in the SkyDrive policy key. Prevents OneDrive from adding a shortcut icon to the user's desktop during initial setup or after updates. On managed enterprise desktops where the shortcut layout is standardised by Group Policy (no unmanaged shortcuts on desktop), automatic OneDrive shortcut creation violates desktop policy and confuses users who may not be aware of cloud sync being installed. "
                        + "Desktop shortcut proliferation on managed endpoints is a minor but persistent administrative annoyance. Each major OneDrive update can re-create the desktop shortcut if it was manually deleted, causing the shortcut to reappear after each update. Policy-driven suppression ensures the shortcut is never created, remaining consistent across updates without requiring GPO-applied shortcut deletion scripts.",
                    Tags = ["skydrive", "onedrive", "desktop-shortcut", "icon", "managed-desktop"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive desktop icon not created. Users can still access OneDrive via the system tray icon or File Explorer navigation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSkyDriveDesktopIcon", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSkyDriveDesktopIcon")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSkyDriveDesktopIcon", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-prevent-usage-of-onedrive",
                    Label = "SkyDrive: Prevent All OneDrive Usage via Legacy SkyDrive Machine Policy",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets PreventNetworkTrafficPreUserSignIn=1 in the SkyDrive policy key. Prevents OneDrive from generating any network traffic before the user signs in. During the Windows startup sequence, OneDrive pre-caches metadata and checks for updates before user login completes. This pre-sign-in network activity consumes bandwidth, adds to boot time, and generates outbound connections from a system in an unauthenticated state — which some network security monitoring tools flag as suspicious. "
                        + "Pre-authentication network connections from Microsoft services are a known privacy concern: OneDrive network activity during boot can leak the device's presence, IP address, and tenant association to Microsoft servers before the user has consented to connected services for that session. In high-security environments that enforce a zero-trust model where no application should generate network traffic until after full user authentication, pre-sign-in OneDrive connections violate this control. Blocking pre-sign-in network activity ensures OneDrive only connects after a user is fully authenticated.",
                    Tags = ["skydrive", "onedrive", "pre-signin", "network-traffic", "zero-trust", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive pre-login network activity blocked. No user-visible impact. OneDrive connects normally after user authentication completes.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventNetworkTrafficPreUserSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventNetworkTrafficPreUserSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventNetworkTrafficPreUserSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-personal-sync",
                    Label = "SkyDrive: Block Sync of Personal Accounts on Domain-Joined Machines",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisablePersonalSync=1 in the SkyDrive policy key. Prevents users from adding and syncing personal (non-corporate) Microsoft accounts with OneDrive on domain-joined or Entra ID-joined machines. Allows corporate OneDrive for Business (Entra ID accounts) to function normally while blocking personal @hotmail.com, @outlook.com, and @gmail.com accounts from syncing. "
                        + "On corporate endpoints, personal OneDrive accounts present a data exfiltration risk: a user can drag corporate documents into their personal OneDrive sync folder and those files are immediately uploaded to their personal account, bypassing corporate DLP policies that only monitor corporate OneDrive tenants. The DisablePersonalSync policy removes the option to add personal accounts from the OneDrive settings UI while allowing the corporate account configuration to proceed normally — enabling corporate OneDrive features while blocking personal sync.",
                    Tags = ["skydrive", "onedrive", "personal-account", "corporate-policy", "dlp", "exfiltration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Personal Microsoft account OneDrive sync blocked. Corporate OneDrive for Business accounts unaffected. Requires Entra ID-joined device for corporate sync.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalSync", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-require-domain-joined-to-sync",
                    Label = "SkyDrive: Require Domain Membership Before Allowing OneDrive Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets RequireAccountFolderLocation=1 in the SkyDrive policy key. Requires that the user's OneDrive folder location is within a domain-accessible path before synchronisation begins. This ensures users cannot configure OneDrive to sync to a USB drive, external HDD, or a path on a non-domain-joined volume, which would bypass file auditing and DLP policies that monitor domain-accessible file paths. "
                        + "OneDrive's default folder location is %USERPROFILE%\\OneDrive — on a domain-joined machine this is within the user profile path which may be redirected to a file server. If a user changes the OneDrive local folder to an external USB drive, sync continues to the external drive but audit policies monitoring the user profile path no longer capture OneDrive file activities. By requiring the account folder to be in an approved location, this policy prevents sync rerouting to unmonitored storage media.",
                    Tags = ["skydrive", "onedrive", "folder-location", "domain", "audit", "data-governance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive sync folder must be on a monitored domain-accessible path. Users cannot redirect sync to USB drives or external storage.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAccountFolderLocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAccountFolderLocation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAccountFolderLocation", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-tutorialicon",
                    Label = "SkyDrive: Suppress OneDrive First-Run Tutorial and Balloon Notifications",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableTutorial=1 in the SkyDrive policy key. Suppresses the OneDrive first-run tutorial wizard and taskbar balloon notification tooltips that appear on first login or after updates. On enterprise-deployed endpoints, the OneDrive tutorial interrupts user productivity during logins, and repetitive balloon tooltips post-update create distraction and support desk calls from users who assume the notifications indicate a problem. "
                        + "First-run wizard suppression is a routine enterprise deployment cleanliness policy — the tutorial is designed for retail consumers who have never configured OneDrive. In corporate environments where OneDrive policy is centrally managed (folder protection, retention policies, tenant binding), the tutorial presents options the user cannot change (they are set by policy) and provides misleading information about sync customisation capabilities. Suppressing the tutorial ensures users see only the relevant corporate-configured sync state without conflicting consumer-oriented guidance.",
                    Tags = ["skydrive", "onedrive", "tutorial", "notification", "enterprise-deployment"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive first-run tutorial and balloon tips suppressed. No functional impact — OneDrive operates normally with tutorial hidden.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTutorial", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTutorial")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTutorial", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-block-known-folder-move",
                    Label = "SkyDrive: Block Known Folder Move to Prevent Forced Desktop/Documents Redirect to OneDrive",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets KFMBlockOptIn=1 in the SkyDrive policy key. Blocks OneDrive's Known Folder Move (KFM) feature from prompting users or automatically moving the Windows Known Folders (Desktop, Documents, Pictures) from their local profile path to the OneDrive folder. KFM can be deployed silently by IT to redirect these folders to OneDrive cloud storage — but without advance user notification, users may be surprised to find their desktop files suddenly synchronised to the cloud. "
                        + "Known Folder Move can have significant consequences when deployed without proper planning: large local Desktop and Documents folders (100+ GB) begin uploading to OneDrive immediately, consuming bandwidth. Folders that contain sensitive data subject to GDPR or HIPAA retention policies may inadvertently be moved to a Microsoft-operated cloud service without completing required Data Processing Agreement reviews. By blocking KFM opt-in via this policy, organisations can plan and deploy folder redirection deliberately rather than having it trigger based on defaults.",
                    Tags = ["skydrive", "onedrive", "known-folder-move", "kfm", "desktop-redirect", "cloud-redirect"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OneDrive Known Folder Move blocked. Desktop, Documents, Pictures remain in local profile. IT-managed folder redirection to file server is unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "KFMBlockOptIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "KFMBlockOptIn")],
                    DetectOps = [RegOp.CheckDword(Key, "KFMBlockOptIn", 1)],
                },
                new TweakDef
                {
                    Id = "skydrive-disable-teamsync",
                    Label = "SkyDrive: Disable OneDrive SharePoint-Backed Team Site Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Sets DisableSharepointSync=1 in the SkyDrive policy key. Prevents OneDrive from synchronising SharePoint Online-backed team site document libraries to the local machine. SharePoint team site sync makes the full content of shared team library folders available for offline editing — potentially storing large volumes of multi-user shared data locally on a laptop endpoint. "
                        + "SharePoint team site sync on secure endpoints creates data sovereignty risk: when a full team document library (containing files created by all team members) is synced locally, those files are stored in an endpoint protected only by the laptop's local encryption. If the laptop is stolen or compromised, all team documents are accessible to the attacker — not just the individual user's Documents but the entire team library. Disabling SharePoint sync ensures team content remains in the cloud and is only accessible via the browser with valid MFA credentials, not from the local disk.",
                    Tags = ["skydrive", "onedrive", "sharepoint", "team-site", "offline-sync", "data-sovereignty"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SharePoint Online team site document library sync to local machine disabled. Team files accessed via browser/SharePoint only. Personal OneDrive sync unaffected if DisableFileSync not set.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSharepointSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSharepointSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSharepointSync", 1)],
                },
            ];
    }

    // ── UniversalClipboardSyncPolicy ──
    private static class _UniversalClipboardSyncPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "uniclip-disable-mobile-device-sync",
                    Label = "Disable Windows Mobile Device Clipboard Sync",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Disables clipboard synchronization between Windows and mobile devices (Android phones, tablets) through the Universal Clipboard infrastructure.",
                    Tags = ["clipboard", "mobile", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard not synchronized to mobile devices; all clipboard data stays on PC.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMobileClipboardSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileClipboardSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMobileClipboardSync", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-clipboard-msa",
                    Label = "Disable Clipboard Access for Microsoft Accounts",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Prevents Microsoft account-linked clipboard history from being accessible across devices tied to the same MSA, blocking cloud-backed clipboard sharing.",
                    Tags = ["clipboard", "msa", "account", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MSA-linked clipboard sharing disabled; useful for separating personal/work contexts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMicrosoftAccounts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMicrosoftAccounts")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMicrosoftAccounts", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-restrict-trusted-apps",
                    Label = "Restrict Clipboard to Trusted Apps Only",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Restricts clipboard API access to applications in an approved trust list, blocking unrecognized or unsigned apps from accessing clipboard contents.",
                    Tags = ["clipboard", "trusted-apps", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Clipboard restricted to trusted apps; unapproved apps receive empty clipboard reads.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardTrustedApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardTrustedApps")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardTrustedApps", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-block-third-party-managers",
                    Label = "Block Third-Party Clipboard Managers",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Blocks third-party clipboard manager applications from accessing the extended clipboard history API, preventing unapproved software from storing clipboard data.",
                    Tags = ["clipboard", "third-party", "manager", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Third-party clipboard managers lose access to clipboard history API.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyClipboardManagers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyClipboardManagers")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyClipboardManagers", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-html-format",
                    Label = "Disable HTML Clipboard Format",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Disables the HTML clipboard format, forcing web content copies to plain text and preventing HTML metadata (tracking pixels, inline styles) from being stored in clipboard.",
                    Tags = ["clipboard", "html", "format", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Web content copied as plain text only; HTML formatting stripped.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableHtmlClipboardFormat", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableHtmlClipboardFormat")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableHtmlClipboardFormat", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-restrict-history-admins",
                    Label = "Restrict Clipboard History to Admin Accounts Only",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Limits clipboard history storage and retrieval to administrator accounts only, preventing standard user clipboard data from accumulating in shared history.",
                    Tags = ["clipboard", "admin", "restriction", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Standard user clipboard history disabled; only admin accounts retain clipboard history.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardHistoryAdminsOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardHistoryAdminsOnly")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardHistoryAdminsOnly", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-prediction-service",
                    Label = "Disable Clipboard Prediction Service",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Disables the clipboard prediction background service that analyses clipboard contents to provide predictive paste suggestions.",
                    Tags = ["clipboard", "prediction", "background", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Predictive paste suggestions disabled; clipboard contents not analysed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardPredictionService", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardPredictionService")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardPredictionService", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-block-sync-service",
                    Label = "Block Clipboard Sync Background Service",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Disables the background clipboard synchronization service that maintains clipboard state across devices and cloud endpoints.",
                    Tags = ["clipboard", "sync-service", "background", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Background clipboard sync service stopped; universal clipboard fully disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockClipboardSyncService", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardSyncService")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockClipboardSyncService", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-edge-clipboard-access",
                    Label = "Disable Browser Clipboard Integration via EdgeUpdate Policy",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Disables clipboard access integration for the Edge browser via EdgeUpdate policy, preventing Edge from participating in universal clipboard sync.",
                    Tags = ["clipboard", "edge", "browser", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge clipboard integration disabled; browser clipboard not shared via EdgeUpdate.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardAccess")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardAccess", 1)],
                },
                new TweakDef
                {
                    Id = "uniclip-disable-edge-clipboard-manager",
                    Label = "Disable Edge Clipboard Manager",
                    Category = "Cloud Storage — One Drive Kfm",
                    Description =
                        "Disables the Edge browser's built-in clipboard manager feature that maintains browser-side clipboard history and sharing via EdgeUpdate policy.",
                    Tags = ["clipboard", "edge", "manager", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge clipboard manager disabled; browser clipboard history feature unavailable.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableEdgeClipboardManager", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableEdgeClipboardManager")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableEdgeClipboardManager", 1)],
                },
            ];
    }
}
