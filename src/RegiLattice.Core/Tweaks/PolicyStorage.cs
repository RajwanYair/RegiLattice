namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyStorage
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CdBurningPolicy.Data,
            .. _DiskQuotaAdvancedPolicy.Data,
            .. _DiskQuotaPolicy.Data,
            .. _FileHistoryPolicy.Data,
            .. _FileSharePolicy.Data,
            .. _FileShareWitnessPolicy.Data,
            .. _NtfsPolicy.Data,
            .. _OfflineFilesSyncPolicy.Data,
            .. _OpenTypeSecurityPolicy.Data,
            .. _RefsFsPolicy.Data,
            .. _ReFSPolicy.Data,
            .. _ShadowCopyVss.Data,
            .. _StorageBusPolicy.Data,
            .. _StorageHealthPolicy.Data,
            .. _StorageManagementPolicy.Data,
            .. _StoragePoolPolicy.Data,
            .. _StorageReplicaPolicy.Data,
            .. _StorageSensePolicy.Data,
            .. _StorageSpacesMigrationPolicy.Data,
            .. _StorageSpacesPolicy.Data,
            .. _SyncCenterPolicy.Data,
            .. _VolumeShadowCopyPolicy.Data,
            .. _WorkFoldersPolicy.Data,
        ];

    // ── CdBurningPolicy ──
    private static class _CdBurningPolicy
    {
        private const string BurnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDBurning";
        private const string ExplLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string ExplCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer";
        private const string CdRomKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string DvdKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cdbp-no-burning-machine",
                Label = "Disable CD/DVD Burning (Machine-Wide)",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets NoBurning=1 in the Windows CDBurning policy key for all users on this machine. "
                    + "Removes the 'Burn to Disc' option from Explorer and prevents the built-in burning wizard from launching. "
                    + "Default: absent (burning allowed). Recommended: 1 on managed or public desktops.",
                Tags = ["cd", "burning", "optical", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes Explorer-native disc burning; third-party burning tools are unaffected.",
                ApplyOps = [RegOp.SetDword(BurnKey, "NoBurning", 1)],
                RemoveOps = [RegOp.DeleteValue(BurnKey, "NoBurning")],
                DetectOps = [RegOp.CheckDword(BurnKey, "NoBurning", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-no-burning-user",
                Label = "Disable CD/DVD Burning (Current User)",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets NoCDBurning=1 in the per-user Explorer policy key. "
                    + "Removes the disc-burning shell extension for the current user without machine-wide enforcement. "
                    + "Default: absent. Recommended: 1 on shared workstations for non-admin users.",
                Tags = ["cd", "burning", "optical", "policy", "user"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "User-scoped removal of Burn to Disc wizard; reversible without admin rights.",
                ApplyOps = [RegOp.SetDword(ExplCu, "NoCDBurning", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplCu, "NoCDBurning")],
                DetectOps = [RegOp.CheckDword(ExplCu, "NoCDBurning", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-no-burning-explorer-lm",
                Label = "Hide CD Burning in Explorer (Machine Policy)",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets NoCDBurning=1 in the machine-scoped Explorer policy key. "
                    + "Suppresses the burn-to-disc task pane and context menu item in Explorer for all users. "
                    + "Default: absent. Recommended: 1 in kiosk, classroom, or terminal server deployments.",
                Tags = ["cd", "burning", "explorer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the Explorer CD-burn UI for all users on this machine.",
                ApplyOps = [RegOp.SetDword(ExplLm, "NoCDBurning", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplLm, "NoCDBurning")],
                DetectOps = [RegOp.CheckDword(ExplLm, "NoCDBurning", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-cdrom-execute",
                Label = "Block CD-ROM Execute (AutoRun)",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets Deny_Execute=1 in the CD-ROM device class policy. "
                    + "Prevents direct execution of content from CD-ROM drives via the removable storage access layer. "
                    + "Default: absent. Recommended: 1 on security-hardened systems processing untrusted optical media.",
                Tags = ["cd", "execute", "autorun", "removable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks auto-execute from CD-ROM; disc content is still readable via explicit app launch.",
                ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Execute", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Execute")],
                DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Execute", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-dvd-read",
                Label = "Block DVD Read Access",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets Deny_Read=1 in the DVD/BD removable storage device class policy (GUID {53f56307}). "
                    + "Prevents all read access to DVD and Blu-ray drives. "
                    + "Default: absent. Recommended: 1 only in air-gapped environments where optical media is prohibited.",
                Tags = ["dvd", "read", "removable", "optical", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Completely blocks DVD/BD read access; breaks all optical disc software including media players.",
                ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-block-dvd-execute",
                Label = "Block DVD Execute (AutoRun)",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets Deny_Execute=1 in the DVD device class policy. "
                    + "Prevents the system from auto-executing content directly from DVD drives via the removable storage access layer. "
                    + "Default: absent. Recommended: 1 for security hardening against malicious autoplay content on optical media.",
                Tags = ["dvd", "execute", "autorun", "removable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks DVD auto-execution; manually launched DVD media apps still work.",
                ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Execute", 1)],
                RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Execute")],
                DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Execute", 1)],
            },
            new TweakDef
            {
                Id = "cdbp-no-autoplay-nonvolume",
                Label = "Suppress AutoPlay for Non-Volume Optical Media",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets NoAutoplayfornonVolume=1 in the machine Explorer policy. "
                    + "Prevents Windows from automatically opening or showing the AutoPlay dialog when non-volume media "
                    + "(audio CDs, video DVDs, mixed-mode discs) is inserted. "
                    + "Default: absent (auto-prompt active). Recommended: 1 to reduce unwanted UI interruptions.",
                Tags = ["cd", "dvd", "autoplay", "prompt", "explorer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Silences the 'What do you want to do?' prompt for non-volume optical discs.",
                ApplyOps = [RegOp.SetDword(ExplLm, "NoAutoplayfornonVolume", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplLm, "NoAutoplayfornonVolume")],
                DetectOps = [RegOp.CheckDword(ExplLm, "NoAutoplayfornonVolume", 1)],
            },
        ];
    }

    // ── DiskQuotaAdvancedPolicy ──
    private static class _DiskQuotaAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dquota-enable-quota-policy",
                    Label = "Enable NTFS Disk Quota via Windows NT Policy Tree",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enables NTFS disk quota tracking via the Windows NT policy registry path, activating per-user storage monitoring on all NTFS volumes in support of managed quota enforcement.",
                    Tags = ["disk-quota", "ntfs", "storage", "quota-policy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "NTFS quota tracking enabled via NT policy tree; per-user disk usage monitoring is active.",
                    ApplyOps = [RegOp.SetDword(Key, "Enable", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enable")],
                    DetectOps = [RegOp.CheckDword(Key, "Enable", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-enforce-hard-limit-policy",
                    Label = "Enforce Disk Quota Hard Limit via Policy",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enforces disk quota limits as hard caps via the NT policy registry path, so that users exceeding their quota limit receive disk-full errors and cannot write additional data.",
                    Tags = ["disk-quota", "enforce", "hard-limit", "ntfs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Disk quota hard limit enforced via policy; users exceeding quota receive write denied errors.",
                    ApplyOps = [RegOp.SetDword(Key, "Enforce", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enforce")],
                    DetectOps = [RegOp.CheckDword(Key, "Enforce", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-log-exceed-policy",
                    Label = "Log Event on Quota Limit Exceeded via NT Policy",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enables event log generation when a user exceeds their disk quota limit, via the NT policy registry path, creating an audit record under the System event log.",
                    Tags = ["disk-quota", "event-log", "exceed", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quota exceed events logged via NT policy; over-quota writes generate System event log entries.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverLimit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverLimit")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverLimit", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-log-threshold-policy",
                    Label = "Log Event on Quota Warning Threshold Reached via NT Policy",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enables event log generation when a user reaches the disk quota warning threshold via the NT policy registry path, providing advance notice before hard quota is reached.",
                    Tags = ["disk-quota", "event-log", "warning", "threshold", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quota warning threshold events logged via NT policy; approaching-quota users trigger log entries.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverThreshold", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverThreshold")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverThreshold", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-suppress-balloon-notify",
                    Label = "Suppress Disk Quota Balloon Notification to Users",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents the Windows balloon notification from appearing in the notification area when a user approaches or exceeds their disk quota, suppressing end-user prompts that cannot be action-ably resolved without IT involvement.",
                    Tags = ["disk-quota", "balloon", "notification", "ux", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disk quota balloon notifications suppressed; users near quota limit are not prompted.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBalloonNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBalloonNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBalloonNotification", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-disable-quota-properties-tab",
                    Label = "Remove Quota Properties Tab from Drive Properties",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Removes the Quota tab from the drive Properties dialog for non-administrator users, preventing visibility or modification of quota settings outside of Group Policy managed configuration.",
                    Tags = ["disk-quota", "properties-tab", "ui", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quota tab removed from drive Properties; quota configuration hidden from non-admin users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableQuotaPropertiesTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableQuotaPropertiesTab")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableQuotaPropertiesTab", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-block-per-volume-override",
                    Label = "Block Per-Volume Quota Setting Override by Administrators",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents local administrators from modifying disk quota settings on individual volumes, ensuring that the centrally configured quota policy via Group Policy cannot be overridden at the local machine level.",
                    Tags = ["disk-quota", "per-volume", "lockdown", "gpo", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Per-volume quota overrides blocked; GPO quota settings cannot be changed locally.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPerVolumeOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPerVolumeOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPerVolumeOverride", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-display-quota-in-free-space",
                    Label = "Display Quota Remaining as Free Space in Explorer",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Configures Explorer to show a user's remaining quota allowance as the reported free disk space, preventing confusion where a user sees 100 GB free on a 500 GB drive but is personally limited to 5 GB.",
                    Tags = ["disk-quota", "free-space", "explorer", "ux", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Explorer shows personal quota remaining as free space; users see their effective limit not total disk.",
                    ApplyOps = [RegOp.SetDword(Key, "DisplayQuotaAsFreeDiskSpace", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisplayQuotaAsFreeDiskSpace")],
                    DetectOps = [RegOp.CheckDword(Key, "DisplayQuotaAsFreeDiskSpace", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-apply-to-mapped-drives",
                    Label = "Apply Disk Quota Tracking to Mapped Network Drives",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Extends disk quota tracking to mapped network drives (drive letter shares), so that per-user storage limits are also enforced when users write to network-mapped drives.",
                    Tags = ["disk-quota", "mapped-drives", "network", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quota tracking extended to mapped network drives; users cannot bypass local quotas via mapped paths.",
                    ApplyOps = [RegOp.SetDword(Key, "ApplyQuotaToMappedDrives", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ApplyQuotaToMappedDrives")],
                    DetectOps = [RegOp.CheckDword(Key, "ApplyQuotaToMappedDrives", 1)],
                },
                new TweakDef
                {
                    Id = "dquota-restrict-quota-report-export",
                    Label = "Restrict Disk Quota Report Export to Administrators Only",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents standard users from exporting or printing disk quota reports that contain per-user storage consumption data, protecting user privacy and preventing disclosure of storage usage patterns.",
                    Tags = ["disk-quota", "report", "export", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quota report export restricted to admins; standard users cannot access per-user storage consumption reports.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictQuotaReportExport", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictQuotaReportExport")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictQuotaReportExport", 1)],
                },
            ];
    }

    // ── DiskQuotaPolicy ──
    private static class _DiskQuotaPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskQuota";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "diskquota-enable-quota",
                    Label = "Enable NTFS Disk Quotas",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Activates disk quota tracking on all NTFS volumes. Without quotas enabled, individual users can consume an unlimited amount of disk space. Enabling quotas allows enforcement of per-user storage limits. Default: disk quotas disabled. Recommended: 1 on shared machines and file servers.",
                    Tags = ["disk", "quota", "ntfs", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Disk quota tracking is active on all NTFS volumes; per-user space consumption is monitored.",
                    ApplyOps = [RegOp.SetDword(Key, "Enable", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enable")],
                    DetectOps = [RegOp.CheckDword(Key, "Enable", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-enforce-quota-limit",
                    Label = "Enforce Disk Quota Limit (Deny Disk Space Beyond Limit)",
                    Category = "Storage — Cd Burning",
                    Description =
                        "When quotas are enabled, this setting denies additional disk writes once a user reaches their quota limit — rather than merely logging a warning. Without enforcement, quotas are advisory only. Default: not enforced (log-only). Recommended: 1 if quotas are enabled.",
                    Tags = ["disk", "quota", "enforce", "ntfs", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Users who reach their quota limit receive an 'insufficient disk space' error; writes are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "Enforce", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enforce")],
                    DetectOps = [RegOp.CheckDword(Key, "Enforce", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-log-quota-exceeded",
                    Label = "Log Events When Quota Limit Is Exceeded",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Records an event in the Application log whenever a user exceeds their disk quota limit. Provides visibility over storage exhaustion incidents without requiring enforcement mode. Default: not logged. Recommended: 1 for compliance and monitoring.",
                    Tags = ["disk", "quota", "audit", "logging", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Application log entry written each time any user's quota limit is exceeded.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverLimit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverLimit")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverLimit", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-log-quota-warning",
                    Label = "Log Events When Quota Warning Threshold Is Reached",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Records an Application log event when a user's disk usage reaches the warning level (set below the hard quota limit). Gives early warning before the limit is hit. Default: not logged. Recommended: 1 for proactive storage management.",
                    Tags = ["disk", "quota", "warning", "audit", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Application log entry written when any user reaches the warning threshold.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEventOverThreshold", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEventOverThreshold")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEventOverThreshold", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-apply-subdirectories",
                    Label = "Apply Quota to All Subdirectories",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Extends quota tracking so that disk space used by files in every subdirectory is counted against the owner's total quota. Without this, only root-level file writes are counted. Default: subdirectory counting depends on volume settings. Recommended: 1.",
                    Tags = ["disk", "quota", "subdirectory", "ntfs", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quotas account for all files in all subdirectories on NTFS volumes, not just root-level writes.",
                    ApplyOps = [RegOp.SetDword(Key, "CalibrateTargetDir", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "CalibrateTargetDir")],
                    DetectOps = [RegOp.CheckDword(Key, "CalibrateTargetDir", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-set-default-limit-1gb",
                    Label = "Set Default Per-User Quota Limit to 1 GB",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Sets the default disk quota limit applied to new user accounts to 1 073 741 824 bytes (1 GiB). New users automatically receive this limit without admin intervention. The value is stored as a QWORD count of bytes. Default: no limit (-1 / unlimited). Recommended: set as appropriate for available storage.",
                    Tags = ["disk", "quota", "limit", "default", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Each new user profile on NTFS volumes defaults to a 1 GiB hard quota.",
                    ApplyOps = [RegOp.SetQword(Key, "DefaultQuotaLimit", 1_073_741_824L)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultQuotaLimit")],
                    DetectOps = [],
                },
                new TweakDef
                {
                    Id = "diskquota-set-default-warning-800mb",
                    Label = "Set Default Per-User Warning Threshold to 800 MB",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Sets the warning threshold for new user accounts to 838 860 800 bytes (800 MiB). When a user reaches 80% of the 1 GiB default limit an event is logged before hitting the hard quota. Default: no warning threshold. Recommended: ~80% of the default quota limit.",
                    Tags = ["disk", "quota", "warning", "threshold", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Warning event fires when a new user reaches 800 MiB of disk usage.",
                    ApplyOps = [RegOp.SetQword(Key, "DefaultQuotaThreshold", 838_860_800L)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultQuotaThreshold")],
                    DetectOps = [],
                },
                new TweakDef
                {
                    Id = "diskquota-block-user-override",
                    Label = "Prevent Users from Changing Quota Settings",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Hides disk quota settings from the volume Properties dialog so standard users cannot view or modify their own quota limits. Works in conjunction with Enforce to prevent circumvention. Default: users can view their own quota from Properties. Recommended: 1.",
                    Tags = ["disk", "quota", "user-restriction", "settings", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Quota tab is removed from volume Properties for non-admin users.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserSettings")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserSettings", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-disable-removable-volumes",
                    Label = "Do Not Apply Quotas to Removable Volumes",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Exempts removable NTFS volumes (USB drives formatted as NTFS) from quota management. Useful when quota enforcement should apply to fixed disks only and not to portable storage that may be shared. Default: quotas apply to all NTFS volumes including removable. Recommended: 0 to include removable.",
                    Tags = ["disk", "quota", "removable", "usb", "ntfs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removable NTFS volumes are excluded from quota enforcement; only fixed disks are subject to limits.",
                    ApplyOps = [RegOp.SetDword(Key, "ExcludeRemovableMedia", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExcludeRemovableMedia")],
                    DetectOps = [RegOp.CheckDword(Key, "ExcludeRemovableMedia", 1)],
                },
                new TweakDef
                {
                    Id = "diskquota-exempt-admins",
                    Label = "Exempt Administrators from Disk Quota Limits",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Members of the local Administrators group are not subject to per-user disk quota enforcement even when the volume is quota-managed. Allows admins to perform maintenance (driver updates, log writes, backups) without hitting storage walls. Default: admins are also subject to quotas when Enforce=1. Recommended: 1.",
                    Tags = ["disk", "quota", "admin", "exemption", "ntfs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Local admins bypass disk quota limits; standard user quotas remain enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "ExemptAdministrators", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExemptAdministrators")],
                    DetectOps = [RegOp.CheckDword(Key, "ExemptAdministrators", 1)],
                },
            ];
    }

    // ── FileHistoryPolicy ──
    private static class _FileHistoryPolicy
    {
        private const string FhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FileHistory";
        private const string BkpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "fhp-lock-onoff-switch",
                Label = "Lock File History On/Off Switch",
                Category = "Storage — Cd Burning",
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
                Category = "Storage — Cd Burning",
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
                Id = "fhp-retention-one-month",
                Label = "File History: Keep Versions for 1 Month",
                Category = "Storage — Cd Burning",
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
                Category = "Storage — Cd Burning",
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
                Category = "Storage — Cd Burning",
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
                Category = "Storage — Cd Burning",
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
                Category = "Storage — Cd Burning",
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
                Category = "Storage — Cd Burning",
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

    // ── FileSharePolicy ──
    private static class _FileSharePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "filshare-require-secure-dialect",
                Label = "Set Minimum SMB Server Dialect",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Setting a minimum SMB dialect version for the server component prevents clients from negotiating to use older vulnerable protocol versions. A minimum dialect requirement of SMB 2.0.2 or higher prevents connections from SMB1-only clients that are incompatible with security features. File servers supporting only modern SMB dialects eliminate exposure to SMB1 vulnerabilities like EternalBlue and related exploits. Minimum dialect enforcement may affect legacy clients and network appliances that only speak older SMB versions. Enterprise environments should inventory SMB client versions before enforcing minimum dialect requirements to avoid connectivity disruption. Graceful enforcement with monitoring and logging before hard blocking provides visibility into legacy client dependencies.",
                Tags = ["file-share", "dialect", "smb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSecureDialect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureDialect")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSecureDialect", 1)],
            },
            new TweakDef
            {
                Id = "filshare-set-max-concurrent-sessions",
                Label = "Limit Maximum Concurrent SMB Sessions",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Limiting concurrent SMB sessions prevents resource exhaustion attacks that could make file servers unavailable to legitimate users. Maximum session limits ensure that no single client or group of clients can monopolize file server connection resources. SMB session flooding is a simple denial of service attack that can be performed from within the network by malicious insiders or compromised endpoints. Session limits combined with connection rate throttling provide DoS protection for file sharing infrastructure. Overly low session limits may affect large file servers with many concurrent user connections and should be sized based on actual usage. Monitoring concurrent session counts against defined limits helps detect unusual connection patterns from potentially compromised systems.",
                Tags = ["file-share", "sessions", "dos-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxConcurrentConnections", 16384)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxConcurrentConnections")],
                DetectOps = [RegOp.CheckDword(Key, "MaxConcurrentConnections", 16384)],
            },
            new TweakDef
            {
                Id = "filshare-enable-server-encryption",
                Label = "Enable SMB Encryption on File Server",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Enabling SMB encryption on the server component protects all file transfer data from network interception by encrypting SMB traffic. Server-side encryption configuration ensures that all clients connecting to the server receive encrypted data regardless of client-side configuration. SMB encryption is available in SMB3 and provides AES-128-CCM or AES-128-GCM protection for all transferred data. Encrypted SMB prevents passive network capture from exposing file contents, metadata, and authentication data. File servers containing sensitive information should have encryption enabled even if clients are trusted to prevent interception at the network layer. Enabling encryption per-share for sensitive shares allows gradual deployment where only specific high-value shares require encryption.",
                Tags = ["file-share", "encryption", "smb", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EncryptData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EncryptData")],
                DetectOps = [RegOp.CheckDword(Key, "EncryptData", 1)],
            },
            new TweakDef
            {
                Id = "filshare-reject-unencrypted-access",
                Label = "Reject Unencrypted Client Connections",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Rejecting unencrypted client connections ensures that the file server refuses SMB connections from clients that do not support or use encryption. When combined with server encryption requirements, rejecting unencrypted connections enforces end-to-end encryption for all file server access. Clients running Windows 10 or Server 2016 and later all support SMB encryption but legacy clients may connect without encryption support. Rejecting unencrypted connections prevents a mixed-security scenario where some clients use encryption and others do not. Organizations must ensure all file server clients support SMB3 encryption before enforcing rejection of unencrypted connections. Monitoring SMB session negotiations before enforcement helps identify legacy clients that need to be updated or replaced.",
                Tags = ["file-share", "encryption", "reject-unencrypted", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RejectUnencryptedAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RejectUnencryptedAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RejectUnencryptedAccess", 1)],
            },
            new TweakDef
            {
                Id = "filshare-restrict-null-session-shares",
                Label = "Restrict Shares Accessible via Null Sessions",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Null session shares are accessible to unauthenticated network connections and provide an information disclosure path for network enumeration. Restricting null session shares prevents anonymous access to file shares through unauthenticated SMB connections. Null sessions allow remote enumeration of share names from which attackers can identify targets for authenticated access attempts. The NullSessionShares registry value lists which shares can be accessed without authentication and should contain no shares in secure configurations. Legacy applications that require null session access should be replaced with authenticated alternatives. Null session restriction is a fundamental network security control that should be enforced on all Windows systems accessible from the network.",
                Tags = ["file-share", "null-session", "anonymous", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNullSessionShares", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNullSessionShares")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNullSessionShares", 1)],
            },
            new TweakDef
            {
                Id = "filshare-log-unauthorized-access",
                Label = "Enable Unauthorized File Share Access Logging",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Unauthorized file share access logging records failed access attempts to shares where the requestor lacks sufficient permissions. Enabling unauthorized access logging generates security events for share access denials providing visibility into access control boundary events. Failed share access events can indicate configuration errors, misconfigured access controls, or attempted unauthorized access. Security Event Log event 5140 with failure status records share access denials with requestor account and share name. Correlation of repeated share access failures from the same account across multiple servers may indicate lateral movement scanning. Unauthorized access events should be forwarded to SIEM and correlated with authentication events to assess intent and risk.",
                Tags = ["file-share", "access-denied", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogUnauthorizedAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogUnauthorizedAccess")],
                DetectOps = [RegOp.CheckDword(Key, "LogUnauthorizedAccess", 1)],
            },
            new TweakDef
            {
                Id = "filshare-disable-oplocks",
                Label = "Configure Opportunistic Locking for Sensitive Shares",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Opportunistic locking allows clients to cache file data locally for performance optimization but can cause data corruption with multiple concurrent editors. Configuring oplock behavior for sensitive shares prevents data loss scenarios where multiple writers believe they have exclusive access. Oplocks on database files, transactional data, and other sensitive data could cause corruption if network connectivity is interrupted during a cache hold. Disabling oplocks on specific shares forces clients to directly read from and write to the server ensuring data consistency. Oplocks are generally appropriate for read-heavy workloads but should be disabled for shares containing database files, application logs, or frequently modified shared configuration files. Share-level oplock configuration provides granular control without disabling oplocks globally across all shares.",
                Tags = ["file-share", "oplocks", "consistency", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigureOplocks", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigureOplocks")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigureOplocks", 0)],
            },
        ];
    }

    // ── FileShareWitnessPolicy ──
    private static class _FileShareWitnessPolicy
    {
        private const string SrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        private const string WrkKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fswitness-disable-smb1-server",
                    Label = "File Share Witness: Disable SMB1 Protocol on Server",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Sets SMB1=0 in LanmanServer policy. Disables the SMBv1 protocol on the server component. SMBv1 is a 1980s-era protocol with no encryption, no pre-authentication integrity, no signing by default, and numerous unfixed vulnerabilities including EternalBlue (CVE-2017-0144) which was exploited by WannaCry and NotPetya ransomware. Microsoft deprecated SMBv1 in 2014. Any operating system newer than Windows XP/Server 2003 supports SMBv2+. Disabling SMBv1 on the server prevents legacy client connections but eliminates the most dangerous attack surface in Windows networking.",
                    Tags = ["smb1", "smb", "eternalblue", "ransomware", "protocol"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "SMBv1 server is disabled. Clients that can only use SMBv1 (Windows XP, Server 2003, early SAMBA versions, some legacy NAS appliances) cannot connect. Verify no SMBv1-only clients exist before applying.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "SMB1", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "SMB1")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "SMB1", 0)],
                },
                new TweakDef
                {
                    Id = "fswitness-set-smb-max-connections",
                    Label = "File Share Witness: Set Maximum SMB Simultaneous Open Files Limit",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Sets MaxWorkItems=16384 in LanmanServer policy. Sets the maximum number of SMB work items (pending I/O operations per connection) the server will process simultaneously. The default value (64 on some configurations) can cause server-side SMB queuing under heavy load from many concurrent clients (e.g., login storms or VDI deployments). Increasing to 16384 allows more concurrent file operations without queuing delay. This setting must be balanced against available memory — each work item consumes non-paged pool memory.",
                    Tags = ["smb", "performance", "work-items", "concurrency", "file-server"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Server processes up to 16384 simultaneous SMB work items. Improves throughput under high-concurrency file access. Consumes additional non-paged pool memory on servers with many concurrent SMB clients.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "MaxWorkItems", 16384)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "MaxWorkItems")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "MaxWorkItems", 16384)],
                },
                new TweakDef
                {
                    Id = "fswitness-enable-smb-hardened-unc",
                    Label = "File Share Witness: Enable Hardened UNC Path Requirements",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Sets HardenedUNCPathsEnabled=1 in LanmanWorkstation policy. Enables hardened UNC path processing, which requires mutual authentication and integrity for connections to UNC paths matching patterns registered in the HardenedUNCPaths registry list (\\\\*\\NETLOGON, \\\\*\\SYSVOL, etc). Without hardened UNC paths, a man-in-the-middle attacker can serve a rogue SYSVOL or NETLOGON share to deliver malicious Group Policy objects or logon scripts. Hardened UNC paths were introduced as the main mitigation for MS15-011 (JASBUG).",
                    Tags = ["smb", "unc", "hardening", "gpo", "ms15-011"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "UNC paths to NETLOGON and SYSVOL require mutual authentication and signing. Man-in-the-middle attacks against Group Policy delivery are blocked. No impact on normal AD-joined clients with a working domain connection.",
                    ApplyOps = [RegOp.SetDword(WrkKey, "HardenedUNCPathsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WrkKey, "HardenedUNCPathsEnabled")],
                    DetectOps = [RegOp.CheckDword(WrkKey, "HardenedUNCPathsEnabled", 1)],
                },
            ];
    }

    // ── NtfsPolicy ──
    private static class _NtfsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NTFS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ntfspol-disable-last-access",
                Label = "Disable NTFS Last Access Time Update",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NTFS records the last access time for every file each time it is read, requiring a metadata write operation on every file read. Disabling last access time updates eliminates the write operation triggered on every file read, significantly reducing metadata update overhead. On systems with millions of files, last access time recording causes substantial unnecessary disk I/O especially for antivirus scans and search indexers. Removing last access time recording can improve performance of read-heavy workloads by up to 15 percent on spinning disk systems. Last access time is rarely used by enterprise applications and is not required for data classification or integrity purposes. Security tools that require access time tracking should use shadow copies or file activity monitoring instead.",
                Tags = ["ntfs", "performance", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLastAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLastAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLastAccess", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-enable-compression",
                Label = "Disable NTFS Compression on System Volume",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "NTFS transparent compression reduces file sizes on disk by compressing file contents using the LZ77 compression algorithm. Disabling NTFS compression on the system volume prevents performance-degrading CPU overhead from compression and decompression on every file access. NTFS compression causes random access pattern degradation because compressed files require sequential decompression to reach arbitrary offsets. Modern SSD storage provides sufficient capacity that compression decompression overhead is not worth the space savings. System files including the page file hivelist and drivers should never be compressed as it introduces unbounded decompression latency at critical moments. Disabling system volume compression ensures consistent and predictable I/O performance for OS operations.",
                Tags = ["ntfs", "compression", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-8dot3-names",
                Label = "Disable NTFS 8.3 Short Name Generation",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "NTFS generates short 8.3 format filenames for every file created to maintain backward compatibility with legacy 16-bit applications. Disabling 8.3 short name generation eliminates this legacy metadata overhead and reduces directory metadata size. Short name generation requires computing and storing an additional name entry for every file, increasing directory metadata write cost. Enterprise environments running exclusively 32-bit and 64-bit applications do not benefit from 8.3 name compatibility. Disabling short names can improve file creation performance on systems with very large directories containing many files. Note that some older administrative tools and scripts may depend on short names; these should be tested before deployment.",
                Tags = ["ntfs", "8dot3", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Disable8dot3NameCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Disable8dot3NameCreation")],
                DetectOps = [RegOp.CheckDword(Key, "Disable8dot3NameCreation", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-self-healing",
                Label = "Disable NTFS Self-Healing (Force Chkdsk)",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 2,
                Description =
                    "NTFS self-healing automatically repairs filesystem inconsistencies detected during normal operation without requiring offline chkdsk runs. Disabling self-healing forces filesystem errors to be addressed through the traditional offline chkdsk process requiring a system restart. Self-healing runs in the background and may silently alter filesystem metadata in ways that complicate forensic analysis. Forensic investigation scenarios require preservation of exact filesystem state including errors for evidentiary purposes. Some self-healing repairs may destroy evidence of intrusion by cleaning up attacker-modified metadata. This tweak is intended specifically for forensic workstations and incident response systems, not general enterprise use.",
                Tags = ["ntfs", "self-healing", "forensics", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSelfHealing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSelfHealing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSelfHealing", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-encryption-default",
                Label = "Prevent Default NTFS Encryption of New Files",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "NTFS Encrypted File System inheritable encryption allows folders to be marked such that all files created within them are automatically encrypted. Preventing default encryption inheritance stops new files from being automatically encrypted by inheriting parent directory encryption attributes. Automatic encryption without user awareness can prevent legitimate administrative access to files for maintenance and incident response. EFS-encrypted files that lose access to the recovery certificate become permanently inaccessible creating potential data loss. Enterprise file encryption should be managed through BitLocker for volume encryption rather than per-file EFS inheritance. Disabling default EFS inheritance prevents accidental file lockout while preserving the ability to use EFS intentionally.",
                Tags = ["ntfs", "encryption", "efs", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEncryptionDefault", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEncryptionDefault")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEncryptionDefault", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-delete-notify",
                Label = "Disable NTFS Delete Notify to SSD Controller",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "NTFS sends TRIM commands to SSD controllers when files are deleted, allowing the controller to proactively reclaim and erase NAND flash cells. Disabling delete notify (TRIM) prevents Windows from sending TRIM commands to connected storage controllers. TRIM disabled mode can be useful when using storage over certain RAID configurations or shared storage that does not benefit from per-host TRIM. Some NVMe configurations perform background garbage collection more efficiently without host-generated TRIM hints. TRIM should only be disabled when the storage subsystem design is known to perform better without it. Most modern consumer and enterprise SSDs benefit from TRIM and disabling it degrades long-term write performance.",
                Tags = ["ntfs", "trim", "ssd", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeleteNotify", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeleteNotify")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeleteNotify", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-alternate-data-streams-block",
                Label = "Block Alternate Data Stream Creation by Untrusted Code",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "NTFS Alternate Data Streams allow additional data to be attached to files under hidden named streams that are invisible to most file browsers. Blocking ADS creation by untrusted code prevents malware and unauthorized applications from hiding data in invisible file streams. Alternate data streams are used by some malware to store payloads, configuration, or exfiltrated data in streams invisible to directory listings. The Zone.Identifier ADS created by browsers is a legitimate security feature that marks downloaded files and should be preserved. Blocking indiscriminate ADS creation from untrusted sources limits the use of this NTFS feature as a steganographic storage mechanism. Security tools that rely on ADS for file metadata tags should be evaluated and explicitly exempted.",
                Tags = ["ntfs", "ads", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockAlternateDataStreamsByUntrusted", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAlternateDataStreamsByUntrusted")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAlternateDataStreamsByUntrusted", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-tunnel-cache",
                Label = "Disable NTFS Filename Tunnel Cache",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The NTFS filename tunnel cache briefly preserves the identity of a short filename when a file is deleted and recreated with the same name. Disabling the tunnel cache prevents NTFS from re-associating the previous short filename when a file with the same long name is recreated. The tunnel cache can cause unexpected short filename collisions when temporary file creation and deletion cycles are frequent. Some security tools may observe abnormal file identity continuity through the tunnel cache which can complicate forensic timeline analysis. Disabling the tunnel cache ensures newly created files receive fresh filename allocations without inheriting deleted file identities. This setting has no impact on visible filename behavior for long filenames.",
                Tags = ["ntfs", "tunnel-cache", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaximumTunnelEntryAgeInSeconds", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaximumTunnelEntryAgeInSeconds")],
                DetectOps = [RegOp.CheckDword(Key, "MaximumTunnelEntryAgeInSeconds", 0)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-quota-tracking",
                Label = "Disable NTFS Disk Quota Tracking",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "NTFS disk quotas track per-user disk space usage on volumes to enforce storage limits for individual accounts. Disabling disk quota tracking removes the overhead of per-user storage accounting metadata updates on every file creation and deletion. Quota tracking requires metadata updates on every file write scaled to the number of users with active quotas. Enterprise storage management through NAS, DFS, or cloud storage is more scalable and flexible than per-volume NTFS quotas. Removing quota tracking reduces file creation overhead and simplifies storage management administration. This setting is appropriate for volumes where storage management is handled through external storage systems rather than NTFS quotas.",
                Tags = ["ntfs", "quotas", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableQuotaTracking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableQuotaTracking")],
                DetectOps = [RegOp.CheckDword(Key, "DisableQuotaTracking", 1)],
            },
            new TweakDef
            {
                Id = "ntfspol-disable-opportunistic-locks",
                Label = "Disable NTFS Opportunistic Locking",
                Category = "Storage — Cd Burning",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 2,
                Description =
                    "NTFS opportunistic locking allows clients to cache file data locally when no other client has the file open, improving performance by reducing file server round trips. Disabling opportunistic locking forces all file reads and writes to go to the server without local caching for network-shared NTFS volumes. Oplock conflicts during concurrent access can cause temporary file lock contention delays for applications sharing files across multiple clients. Some legacy applications do not handle oplock break negotiations correctly, causing hangs or corruption when files are accessed concurrently. Disabling oplocks is a workaround for specific legacy application compatibility issues rather than a general recommendation. This setting should only be applied where known oplock compatibility problems exist and should not be applied broadly.",
                Tags = ["ntfs", "oplocks", "compatibility", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOpportunisticLocking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOpportunisticLocking")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOpportunisticLocking", 1)],
            },
        ];
    }

    // ── OfflineFilesSyncPolicy ──
    private static class _OfflineFilesSyncPolicy
    {
        private const string NetCache = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache";
        private const string SyncMgr = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SyncMgr";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "offsync-no-make-available-offline",
                Label = "Prevent Making Files Available Offline",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets NoMakeAvailableOffline=1 in NetCache policy. Blocks users from right-clicking shared files and selecting 'Always Available Offline'. Prevents uncontrolled growth of the offline cache on laptops and ensures only IT-assigned offline content is cached.",
                Tags = ["offline", "sync", "files", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "NoMakeAvailableOffline", 1)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "NoMakeAvailableOffline")],
                DetectOps = [RegOp.CheckDword(NetCache, "NoMakeAvailableOffline", 1)],
            },
            new TweakDef
            {
                Id = "offsync-purge-at-logoff",
                Label = "Purge Offline Cache at Logoff",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets PurgeAtLogoff=1 in NetCache policy. Causes all locally cached offline files to be deleted when the user logs off. Ensures sensitive documents synced from file servers are not retained on shared or kiosk machines between sessions.",
                Tags = ["offline", "sync", "files", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "PurgeAtLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "PurgeAtLogoff")],
                DetectOps = [RegOp.CheckDword(NetCache, "PurgeAtLogoff", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-background-sync",
                Label = "Disable Automatic Background Sync",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets BackgroundSyncEnabled=0 in NetCache policy. Stops the Offline Files CSC service from performing background synchronisation of the offline cache. Prevents unexpected I/O bursts and network traffic from silent sync operations, without disabling offline access entirely.",
                Tags = ["offline", "sync", "background", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "BackgroundSyncEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "BackgroundSyncEnabled")],
                DetectOps = [RegOp.CheckDword(NetCache, "BackgroundSyncEnabled", 0)],
            },
            new TweakDef
            {
                Id = "offsync-cache-disk-limit-5pct",
                Label = "Limit Offline Cache to 5% of Disk",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets DefaultCacheSize=5 in NetCache policy (percentage of disk). Restricts the maximum space the Offline Files cache may consume to 5% of the volume. Prevents the CSC cache from silently consuming large amounts of disk space on smaller system drives.",
                Tags = ["offline", "sync", "disk", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "DefaultCacheSize", 5)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "DefaultCacheSize")],
                DetectOps = [RegOp.CheckDword(NetCache, "DefaultCacheSize", 5)],
            },
            new TweakDef
            {
                Id = "offsync-go-offline-manual",
                Label = "Set Go-Offline Action to Manual",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets GoOfflineAction=0 (manual) in NetCache policy. Controls what happens when a network connection to a file server is lost: 0=work offline silently, 1=notify and ask. Setting 0 prevents disruptive dialogs on unstable connections while relying on manual sync on reconnect.",
                Tags = ["offline", "sync", "notification", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "GoOfflineAction", 0)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "GoOfflineAction")],
                DetectOps = [RegOp.CheckDword(NetCache, "GoOfflineAction", 0)],
            },
            new TweakDef
            {
                Id = "offsync-minimal-event-logging",
                Label = "Reduce Offline Files Event Log Verbosity",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets EventLoggingLevel=1 in NetCache policy. Reduces the Offline Files event log from informational (2) to warnings-only (1). Eliminates high-frequency informational events from the CSC service in the System event log on machines with many network shares.",
                Tags = ["offline", "sync", "eventlog", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(NetCache, "EventLoggingLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(NetCache, "EventLoggingLevel")],
                DetectOps = [RegOp.CheckDword(NetCache, "EventLoggingLevel", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-sync-activity-display",
                Label = "Disable Sync Center Activity Display",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets DisableSyncActivity=1 in SyncMgr policy. Prevents the Sync Center from displaying sync progress and activity in the notification area and the Sync Center dialog. Reduces UI clutter from background sync operations on shared desktops.",
                Tags = ["synccenter", "offline", "sync", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "DisableSyncActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "DisableSyncActivity")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "DisableSyncActivity", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-metered-sync",
                Label = "Disable Sync on Metered Connections",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets TurnOffSyncOnCostedNetwork=1 in SyncMgr policy. Prevents Sync Center from initiating any synchronisation when the active network connection is marked as metered (mobile hotspot, LTE, or manually flagged as metered). Prevents unexpected data charges.",
                Tags = ["synccenter", "offline", "sync", "metered", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "TurnOffSyncOnCostedNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "TurnOffSyncOnCostedNetwork")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "TurnOffSyncOnCostedNetwork", 1)],
            },
            new TweakDef
            {
                Id = "offsync-disable-file-sync-client",
                Label = "Disable Sync Center File Sync Client",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets DisableFileSyncClient=1 in SyncMgr policy. Fully disables the Sync Center file synchronisation client component. This stops the CSC service from registering as a sync provider in the Sync Center UI, effectively turning off user-initiated and scheduled offline sync.",
                Tags = ["synccenter", "offline", "sync", "policy", "disable"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "DisableFileSyncClient", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "DisableFileSyncClient")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "DisableFileSyncClient", 1)],
            },
            new TweakDef
            {
                Id = "offsync-hide-in-sync-ui",
                Label = "Hide Offline Files from Sync Center UI",
                Category = "Storage — Cd Burning",
                Description =
                    "Sets HideOptionsForSyncProvider=1 in SyncMgr policy. Removes the options and settings icon for the Offline Files sync provider from the Sync Center window, preventing users from modifying sync provider configuration while still allowing the provider to run.",
                Tags = ["synccenter", "offline", "sync", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SyncMgr, "HideOptionsForSyncProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncMgr, "HideOptionsForSyncProvider")],
                DetectOps = [RegOp.CheckDword(SyncMgr, "HideOptionsForSyncProvider", 1)],
            },
        ];
    }

    // ── OpenTypeSecurityPolicy ──
    private static class _OpenTypeSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\MitigationOptions";
        private const string FontKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine";
        private const string GdipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Fonts";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "otfpol-block-opentype-kernel-parsing",
                    Label = "Block OpenType Font Parsing in the Windows Kernel",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Moves OpenType font parsing out of the Windows kernel (win32k.sys) and into a user-mode font parsing process, eliminating kernel-level font parsing vulnerabilities exploitable via specially-crafted font files in web content.",
                    Tags = ["opentype", "font-parsing", "kernel", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "OpenType kernel parsing disabled; font parsing moved to user-mode — eliminates kernel font exploit surface.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockOpenTypeKernelParser", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockOpenTypeKernelParser")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockOpenTypeKernelParser", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-legacy-font-drivers",
                    Label = "Disable Loading of Legacy TrueType Font Drivers",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents legacy third-party TrueType font drivers from loading in the Windows font subsystem, reducing attack surface from unmaintained or vulnerable font drivers that may contain known CVEs.",
                    Tags = ["truetype", "font-driver", "legacy", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Legacy TrueType font drivers blocked from loading; only Windows-provided drivers used for font rendering.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableLegacyFontDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableLegacyFontDrivers")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableLegacyFontDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-restrict-embedded-font-trusted",
                    Label = "Restrict Embedded Fonts to Trusted Documents Only",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Sets the Windows font embedding policy so that embedded fonts in Office and PDF documents are only rendered when the document originates from a trusted location, blocking remote exploitation via malicious embedded fonts in untrusted files.",
                    Tags = ["fonts", "embedded-font", "trusted", "office", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Embedded fonts rendered only in trusted documents; fonts in untrusted attachments not processed.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "RestrictEmbeddedFontToTrusted", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "RestrictEmbeddedFontToTrusted")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "RestrictEmbeddedFontToTrusted", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-variable-font-web",
                    Label = "Disable Variable Font Loading from Web Content",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents loading of OpenType Variable Fonts (OTF/TTF with variation axes) referenced in web content via browser font stacks, reducing the parsing attack surface from variable font table complexity in browser rendering engines.",
                    Tags = ["opentype", "variable-font", "web", "browser", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Variable font loading from web disabled in browser; reduces OTF/TTF parsing surface in browser renderer.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableVariableFontFromWeb", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableVariableFontFromWeb")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableVariableFontFromWeb", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-enable-font-integrity-check",
                    Label = "Enable Font File Integrity Check Before Loading",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enables a Windows Security Health check that verifies the integrity of installed system fonts against known-good checksums before loading, detecting tampering with font files used in critical UI rendering.",
                    Tags = ["fonts", "integrity-check", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Font file integrity verified before loading; tampered system fonts detected before rendering.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "EnableFontIntegrityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "EnableFontIntegrityCheck")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "EnableFontIntegrityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-block-remote-font-download-edge",
                    Label = "Block Remote Font Downloads in Microsoft Edge",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents Microsoft Edge from downloading and rendering fonts referenced by web page CSS from remote URLs, eliminating an attack vector where crafted web fonts hosted externally could exploit the browser font parser.",
                    Tags = ["fonts", "edge", "remote-font", "css", "browser-security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Edge blocked from loading remote fonts via CSS; all fonts must be system-installed. May break web typography.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts", 0)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts", 0)],
                },
                new TweakDef
                {
                    Id = "otfpol-enable-gdi-font-sandbox",
                    Label = "Enable GDI Font Sandbox in AppContainer Sessions",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enables the GDI+ font rendering sandbox in AppContainer (browser sandboxed renderer) sessions, ensuring that font parsing for sandbox processes occurs in a restricted context rather than directly in win32k.sys.",
                    Tags = ["fonts", "gdi", "sandbox", "appcontainer", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GDI font sandbox enabled in AppContainer; font parsing for sandbox processes isolated from kernel.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "EnableGDIFontSandbox", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "EnableGDIFontSandbox")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "EnableGDIFontSandbox", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-type1-fonts",
                    Label = "Disable Loading of Legacy Type1 Fonts",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Disables support for loading Adobe Type 1 (PostScript) legacy fonts in GDI/GDI+, an aging format with limited security patching, reducing exposure to Type1 font parsing CVEs in the PostScript interpreter.",
                    Tags = ["fonts", "type1", "postscript", "legacy", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Type1/PostScript font loading disabled; legacy PS fonts not rendered. Most modern apps use OpenType.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableType1FontRendering", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableType1FontRendering")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableType1FontRendering", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-log-font-parse-failures",
                    Label = "Log Font File Parse Failures for Security Monitoring",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Enables event log entries when a font file fails parsing validation (malformed tables, invalid checksums), providing visibility into attempts to load crafted malicious fonts on the endpoint.",
                    Tags = ["fonts", "parse-failure", "event-log", "audit", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Font parse failure events logged; malformed or crafted font load attempts visible in security log.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "LogFontParseFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "LogFontParseFailures")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "LogFontParseFailures", 1)],
                },
                new TweakDef
                {
                    Id = "otfpol-disable-font-driver-telemetry",
                    Label = "Disable Font Driver Telemetry Reporting to Microsoft",
                    Category = "Storage — Cd Burning",
                    Description =
                        "Prevents the Windows font subsystem from sending font usage, load failure, and driver interaction telemetry to Microsoft, protecting information about installed and loaded fonts from cloud disclosure.",
                    Tags = ["fonts", "driver", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Font driver telemetry to Microsoft disabled; font load / failure statistics not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(GdipKey, "DisableFontDriverTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdipKey, "DisableFontDriverTelemetry")],
                    DetectOps = [RegOp.CheckDword(GdipKey, "DisableFontDriverTelemetry", 1)],
                },
            ];
    }

    // ── RefsFsPolicy ──
    private static class _RefsFsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "refspol-disable-scrubbing",
                    Label = "Disable ReFS Background Data Scrubbing",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables the background data scrubbing job that periodically reads and validates all ReFS blocks against their stored checksums, eliminating the I/O overhead but preventing proactive corruption detection.",
                    Tags = ["refs", "scrubbing", "background", "file-system", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "ReFS scrubbing disabled; background read-and-verify jobs eliminated, reducing idle I/O.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundScrubbing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundScrubbing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundScrubbing", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-enable-salvage-mode",
                    Label = "Enable ReFS Corruption Salvage Mode",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Enables ReFS salvage mode which continues to mount and access uncorrupted portions of a volume when corruption is detected, avoiding complete volume unavailability due to isolated data corruption.",
                    Tags = ["refs", "salvage", "corruption", "availability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ReFS salvage mode enabled; partially corrupted volumes remain accessible for uncorrupted data.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableSalvageMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableSalvageMode")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableSalvageMode", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-set-cluster-size-64k",
                    Label = "Set Default ReFS Cluster Size to 64 KB",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets the default ReFS cluster size to 64 KB, improving large-sequential-I/O throughput and reducing metadata overhead for workloads that store many large files (virtual machines, databases, backups).",
                    Tags = ["refs", "cluster-size", "performance", "file-system", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS default cluster size set to 64 KB; applies to newly formatted ReFS volumes.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultClusterSizeKB", 64)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultClusterSizeKB")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultClusterSizeKB", 64)],
                },
                new TweakDef
                {
                    Id = "refspol-block-refs-caching-metadata",
                    Label = "Block ReFS Metadata in System File Cache",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Prevents ReFS metadata (B-tree nodes, directory structures) from consuming the system file cache, dedicating file cache to application data and preventing metadata cache pressure on systems with large ReFS trees.",
                    Tags = ["refs", "metadata", "file-cache", "memory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ReFS metadata excluded from system file cache; cache fully available for file data.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsMetadataCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsMetadataCaching")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsMetadataCaching", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-refs-on-boot-volume",
                    Label = "Prevent ReFS Formatting of System Boot Volumes",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Blocks ReFS from being selected as the file system for the system or boot volume during installation, ensuring Windows boot volumes always use NTFS which has full boot-time driver support.",
                    Tags = ["refs", "boot-volume", "ntfs", "formatting", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS blocked on boot volumes; system drive must use NTFS.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsOnSystemVolume", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsOnSystemVolume")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsOnSystemVolume", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-enable-corruption-audit-log",
                    Label = "Enable ReFS Corruption Detection Audit Logging",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Enables detailed event log entries for every ReFS corruption detection event including the file path, cluster address, and recovery action taken.",
                    Tags = ["refs", "corruption", "audit-log", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS corruption events logged; file path, cluster, and recovery details recorded in System event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCorruptionEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCorruptionEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCorruptionEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-refs-dedup",
                    Label = "Disable ReFS Block-Level Deduplication",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables block-level deduplication on ReFS volumes, stopping background dedup processing that can interfere with real-time workloads and consume I/O bandwidth on storage-intensive systems.",
                    Tags = ["refs", "deduplication", "storage", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ReFS block deduplication disabled; no dedup overhead, at the expense of higher storage usage.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsBlockDedup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsBlockDedup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsBlockDedup", 1)],
                },
                new TweakDef
                {
                    Id = "refspol-set-mirror-write-threshold-3",
                    Label = "Set ReFS Mirror-Write Log Threshold to 3 Entries",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Sets the ReFS B+ tree write-log threshold that triggers a checkpoint flush to 3 entries, ensuring faster persistence of write logs at the cost of slightly more frequent I/O checkpoints.",
                    Tags = ["refs", "write-log", "checkpoint", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ReFS write-log checkpoint threshold set to 3; write persistence more frequent on active volumes.",
                    ApplyOps = [RegOp.SetDword(Key, "WriteLogCheckpointThreshold", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WriteLogCheckpointThreshold")],
                    DetectOps = [RegOp.CheckDword(Key, "WriteLogCheckpointThreshold", 3)],
                },
                new TweakDef
                {
                    Id = "refspol-disable-refs-compression",
                    Label = "Disable ReFS Transparent Compression",
                    Category = "Storage — Refs Fs",
                    Description =
                        "Disables ReFS transparent compression, preventing the file system from automatically compressing cold data blocks, and eliminating the CPU overhead of compression/decompression on access.",
                    Tags = ["refs", "compression", "cpu", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ReFS compression disabled; slightly higher storage usage, lower CPU overhead for large file reads.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRefsCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsCompression")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRefsCompression", 1)],
                },
            ];
    }

    // ── ReFSPolicy ──
    private static class _ReFSPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "refs-disable-integrity-checking",
                Label = "Disable ReFS Integrity Checking",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableIntegrityChecking=1 in the ReFS policy key. Prevents ReFS "
                    + "from performing continuous background data integrity scrubbing using "
                    + "checksums stored alongside each file record. Integrity scrubbing "
                    + "consumes additional I/O bandwidth on storage-constrained systems or "
                    + "arrays that already provide checksumming at the hardware level. "
                    + "Default: 0 (scrubbing enabled). Recommended: 1 only on redundant arrays.",
                Tags = ["refs", "integrity", "filesystem", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityChecking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityChecking")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityChecking", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-integrity-streams",
                Label = "Disable ReFS Integrity Streams",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableIntegrityStreams=1 in the ReFS policy key. Turns off the "
                    + "integrity stream feature that tags every file region with a checksum "
                    + "entry in the volume metadata stream. Disabling integrity streams "
                    + "reduces per-write metadata overhead and can improve sequential write "
                    + "throughput by 10–20% on high-frequency write workloads at the cost "
                    + "of silent corruption detection. Default: 0.",
                Tags = ["refs", "integrity-streams", "filesystem", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityStreams", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityStreams")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityStreams", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-auto-repair",
                Label = "Disable ReFS Automatic Repair",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableAutoRepair=1 in the ReFS policy key. Prevents ReFS from "
                    + "automatically correcting detected bad sectors or checksum mismatches "
                    + "using parity or mirror redundancy without user intervention. On "
                    + "non-redundant single-disk volumes the auto-repair feature can "
                    + "silently mark corrupted data as repaired when no valid copy exists. "
                    + "Default: 0. Recommended: 1 only on direct-attached single disks.",
                Tags = ["refs", "repair", "filesystem", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoRepair", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoRepair")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoRepair", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-short-name-creation",
                Label = "Disable ReFS Short Name Creation",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableShortNameCreation=1 in the ReFS policy key. Suppresses "
                    + "automatic generation of 8.3 DOS-compatible short names alongside "
                    + "long file names on ReFS volumes. 8.3 name creation adds measurable "
                    + "overhead on directories with many files and is unnecessary for "
                    + "modern Windows applications and tools that use long-name APIs. "
                    + "Default: 0. Recommended: 1 on dedicated server or data volumes.",
                Tags = ["refs", "shortname", "8dot3", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableShortNameCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableShortNameCreation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableShortNameCreation", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-last-access-update",
                Label = "Disable ReFS Last-Access Timestamp Update",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableLastAccessUpdate=1 in the ReFS policy key. Disables the "
                    + "last-access timestamp field update on every file read operation. "
                    + "Updating the last-access time on each read generates a write "
                    + "transaction to the file metadata in the global B-tree, causing "
                    + "write amplification on read-heavy workloads such as media servers "
                    + "and archive stores. Default: 0. Recommended: 1.",
                Tags = ["refs", "timestamp", "atime", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLastAccessUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLastAccessUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLastAccessUpdate", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-parity-logging",
                Label = "Disable ReFS Parity Write Logging",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableParityLogging=1 in the ReFS policy key. Suppresses the "
                    + "write-ahead parity log that makes partial-stripe writes to parity "
                    + "spaces resilient across power failures. Disabling this log improves "
                    + "random-write throughput on parity storage spaces but opens a window "
                    + "for parity corruption if a power loss occurs mid-stripe. "
                    + "Default: 0. Not recommended unless UPS protection is confirmed.",
                Tags = ["refs", "parity", "wal", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableParityLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableParityLogging")],
                DetectOps = [RegOp.CheckDword(Key, "DisableParityLogging", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-metadata-checksum",
                Label = "Disable ReFS Metadata Checksum",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "Sets DisableMetadataChecksum=1 in the ReFS policy key. Prevents ReFS "
                    + "from computing and verifying a checksum over each metadata B-tree "
                    + "page on every access. Metadata checksumming is a primary ReFS "
                    + "reliability feature; disabling it removes detection of metadata "
                    + "corruption caused by hardware faults or bit-rot and is not "
                    + "recommended on production data volumes. Default: 0.",
                Tags = ["refs", "metadata", "checksum", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMetadataChecksum", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMetadataChecksum")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMetadataChecksum", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-large-mft",
                Label = "Disable ReFS Large MFT Zone Reservation",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableLargeMft=1 in the ReFS policy key. Prevents ReFS from "
                    + "pre-reserving a large zone in the volume B-tree for anticipated "
                    + "metadata growth. Pre-reservation reduces free space visible to users "
                    + "on smaller volumes; on volumes with predictable small file counts "
                    + "the reservation is wasteful and can be released. "
                    + "Default: 0. Recommended: 1 on volumes under 200 GB.",
                Tags = ["refs", "mft", "metadata", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLargeMft", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLargeMft")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLargeMft", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-delete-notify",
                Label = "Disable ReFS Delete Notification (TRIM)",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableDeleteNotify=1 in the ReFS policy key. Stops ReFS from "
                    + "issuing TRIM or UNMAP commands to the underlying SSD or thin-"
                    + "provisioned storage when files are deleted. TRIM commands can cause "
                    + "high-latency stalls on some older firmware SSDs and thin-provisioned "
                    + "SAN/NAS LUNs that must zero out freed blocks before re-allocation. "
                    + "Default: 0. Recommended: 1 only for problematic storage hardware.",
                Tags = ["refs", "trim", "unmap", "ssd", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeleteNotify", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeleteNotify")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeleteNotify", 1)],
            },
            new TweakDef
            {
                Id = "refs-disable-compression",
                Label = "Disable ReFS Data Compression",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableCompression=1 in the ReFS policy key. Prevents the ReFS "
                    + "driver from enabling LZ4-based file compression on volumes where "
                    + "compression has been set as the default state. Compression on "
                    + "already-compressed media files (video, archives, encrypted files) "
                    + "yields negative savings and wastes CPU cycles attempting "
                    + "incompressible blocks. Default: 0. Recommended: 1 on media volumes.",
                Tags = ["refs", "compression", "lz4", "filesystem", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
            },
        ];
    }

    // ── ShadowCopyVss ──
    private static class _ShadowCopyVss
    {
        private const string VssSettings = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings";
        private const string SrPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
        private const string SrSettings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";
        private const string VssDisks = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VolSnap";
        private const string VssWriters = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore\Cfg";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vss-increase-writer-timeout",
                Label = "VSS: Increase Writer Timeout to 120 Seconds",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [VssSettings],
                Tags = ["vss", "shadow-copy", "timeout", "backup", "reliability"],
                Description =
                    "Sets MaxWriterTimeInSeconds=120 in VSS\\Settings. "
                    + "Increases the maximum time the VSS coordinator waits for individual writers "
                    + "before declaring a VSS snapshot failure. Default is 60 seconds. "
                    + "Helps with slow VSS writers (e.g. SQL Server, Exchange) on loaded servers.",
                ApplyOps = [RegOp.SetDword(VssSettings, "MaxWriterTimeInSeconds", 120)],
                RemoveOps = [RegOp.DeleteValue(VssSettings, "MaxWriterTimeInSeconds")],
                DetectOps = [RegOp.CheckDword(VssSettings, "MaxWriterTimeInSeconds", 120)],
            },
            new TweakDef
            {
                Id = "vss-enable-unbuffered-writes",
                Label = "VSS: Enable Unbuffered Writes for Faster Shadow-Copy Creation",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 3,
                RegistryKeys = [VssSettings],
                Tags = ["vss", "shadow-copy", "performance", "backup"],
                Description =
                    "Sets MaxFileBufferSize=0 in VSS\\Settings. "
                    + "Switches VSS copy-on-write I/O from buffered to unbuffered (direct I/O) mode. "
                    + "Can reduce shadow-copy overhead on fast NVMe/SSD arrays. "
                    + "Not recommended on spinning-disk (HDD) systems — increases seek pressure.",
                ApplyOps = [RegOp.SetDword(VssSettings, "MaxFileBufferSize", 0)],
                RemoveOps = [RegOp.DeleteValue(VssSettings, "MaxFileBufferSize")],
                DetectOps = [RegOp.CheckDword(VssSettings, "MaxFileBufferSize", 0)],
            },
            new TweakDef
            {
                Id = "vss-disable-snapvol-for-fixed-drives",
                Label = "VSS: Disable VolSnap Auto-Snapshot on Fixed Drives",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 3,
                RegistryKeys = [VssDisks],
                Tags = ["vss", "shadow-copy", "volsnap", "disk-space", "performance"],
                Description =
                    "Sets AllowSnapshotsOnFixedDrives=0 in VolSnap settings. "
                    + "Prevents the VolSnap driver from creating automatic snapshots on fixed (non-removable) "
                    + "drives. Releases snapshot storage immediately. Not recommended if you rely on "
                    + "VSS-based backup tools (Veeam, Windows Server Backup, etc.).",
                ApplyOps = [RegOp.SetDword(VssDisks, "AllowSnapshotsOnFixedDrives", 0)],
                RemoveOps = [RegOp.DeleteValue(VssDisks, "AllowSnapshotsOnFixedDrives")],
                DetectOps = [RegOp.CheckDword(VssDisks, "AllowSnapshotsOnFixedDrives", 0)],
            },
            new TweakDef
            {
                Id = "vss-set-min-restore-point-space-300mb",
                Label = "VSS: Set Minimum Shadow-Copy Reservation to 300 MB",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 4,
                RegistryKeys = [VssWriters],
                Tags = ["vss", "shadow-copy", "disk-space", "storage", "system-restore"],
                Description =
                    "Sets MinDiskSpace=314572800 (300 MB in bytes) in SystemRestore\\Cfg. "
                    + "Sets the minimum disk space reservation for the shadow copy provider. "
                    + "If free space drops below this threshold, no new snapshots are created. "
                    + "300 MB is tighter than the Windows default (1 GB), saving space on small SSDs.",
                ApplyOps = [RegOp.SetDword(VssWriters, "MinDiskSpace", 314572800)],
                RemoveOps = [RegOp.DeleteValue(VssWriters, "MinDiskSpace")],
                DetectOps = [RegOp.CheckDword(VssWriters, "MinDiskSpace", 314572800)],
            },
            new TweakDef
            {
                Id = "vss-disable-rp-before-critical-updates",
                Label = "VSS: Skip Automatic Restore Points Before Windows Updates",
                Category = "Storage — Refs Fs",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 3,
                RegistryKeys = [SrSettings],
                Tags = ["vss", "shadow-copy", "windows-update", "disk-space", "performance"],
                Description =
                    "Sets CreatePointBeforeCriticalPatches=0 in SystemRestore settings. "
                    + "Prevents Windows from automatically creating a restore point immediately before "
                    + "installing critical Windows Updates. Saves disk space on small SSDs at the cost of "
                    + "losing the rollback safety net for a failed update.",
                ApplyOps = [RegOp.SetDword(SrSettings, "CreatePointBeforeCriticalPatches", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "CreatePointBeforeCriticalPatches")],
                DetectOps = [RegOp.CheckDword(SrSettings, "CreatePointBeforeCriticalPatches", 0)],
            },
        ];
    }
}
