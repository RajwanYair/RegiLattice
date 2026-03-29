// RegiLattice.Core — Tweaks/DiskQuotaAdvancedPolicy.cs
// Advanced NTFS disk quota enforcement, per-volume reporting, and user overrides — Sprint 489.
// Category: "Disk Quota Advanced Policy" | Slug: dquota
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DiskQuotaAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DiskQuota";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "dquota-enable-quota-policy",
                Label = "Enable NTFS Disk Quota via Windows NT Policy Tree",
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
                Category = "Disk Quota Advanced Policy",
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
