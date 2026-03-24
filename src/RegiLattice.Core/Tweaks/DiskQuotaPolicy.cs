// RegiLattice.Core — Tweaks/DiskQuotaPolicy.cs
// Disk quota GPO controls — Sprint 214.
// Enforces NTFS disk quotas for all volumes to prevent storage exhaustion.
// Category: "Disk Quota Policy" | Slug: diskquota
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskQuota

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DiskQuotaPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskQuota";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "diskquota-enable-quota",
                Label = "Enable NTFS Disk Quotas",
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
                Description =
                    "Sets the default disk quota limit applied to new user accounts to 1 073 741 824 bytes (1 GiB). New users automatically receive this limit without admin intervention. The value is stored as a QWORD count of bytes. Default: no limit (-1 / unlimited). Recommended: set as appropriate for available storage.",
                Tags = ["disk", "quota", "limit", "default", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Each new user profile on NTFS volumes defaults to a 1 GiB hard quota.",
                ApplyOps =
                    [RegOp.SetQword(Key, "DefaultQuotaLimit", 1_073_741_824L)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultQuotaLimit")],
                DetectOps =
                    [],
            },
            new TweakDef
            {
                Id = "diskquota-set-default-warning-800mb",
                Label = "Set Default Per-User Warning Threshold to 800 MB",
                Category = "Disk Quota Policy",
                Description =
                    "Sets the warning threshold for new user accounts to 838 860 800 bytes (800 MiB). When a user reaches 80% of the 1 GiB default limit an event is logged before hitting the hard quota. Default: no warning threshold. Recommended: ~80% of the default quota limit.",
                Tags = ["disk", "quota", "warning", "threshold", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Warning event fires when a new user reaches 800 MiB of disk usage.",
                ApplyOps =
                    [RegOp.SetQword(Key, "DefaultQuotaThreshold", 838_860_800L)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultQuotaThreshold")],
                DetectOps =
                    [],
            },
            new TweakDef
            {
                Id = "diskquota-block-user-override",
                Label = "Prevent Users from Changing Quota Settings",
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
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
                Category = "Disk Quota Policy",
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
