// RegiLattice.Core — Tweaks/DefenderExclusionsPolicy.cs
// Windows Defender Exclusions Group Policy — Sprint 190.
// Controls whether local administrators can define Defender exclusions,
// restricts wildcard exclusions, and enforces exclusion audit logging via GPO.
// Category: "Defender Exclusions Policy" | Slug: defexclpol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\WindowsDefender\Exclusions

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DefenderExclusionsPolicy
{
    private const string ExclKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsDefender\Exclusions";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "defexclpol-block-local-exclusion-merge",
                Label = "Block Local Admin Exclusion Merging",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets DisableLocalAdminMerge=1 to prevent local administrators from adding their own Defender exclusions. Only exclusions defined through Group Policy are applied.",
                Tags = ["defender", "exclusions", "admin", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local admin exclusions blocked; only GPO-defined exclusions are active. Hardens Defender.",
                ApplyOps = [RegOp.SetDword(ExclKey, "DisableLocalAdminMerge", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "DisableLocalAdminMerge")],
                DetectOps = [RegOp.CheckDword(ExclKey, "DisableLocalAdminMerge", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-block-user-exclusion-additions",
                Label = "Block Standard User Exclusion Additions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets BlockUserExclusions=1 to prevent standard (non-admin) users from adding or modifying Windows Defender exclusions through the Windows Security app settings.",
                Tags = ["defender", "exclusions", "users", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot add Defender exclusions; only admins (and GPO) can define them.",
                ApplyOps = [RegOp.SetDword(ExclKey, "BlockUserExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "BlockUserExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "BlockUserExclusions", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-restrict-path-exclusions",
                Label = "Restrict Path-Based Exclusion Additions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets RestrictPathExclusions=1 to limit the ability to add new path-based Defender exclusions. Prevents exclusions that could expose scan-critical directories to malware.",
                Tags = ["defender", "exclusions", "path", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "New Defender path exclusions are restricted; existing GPO-defined path exclusions remain.",
                ApplyOps = [RegOp.SetDword(ExclKey, "RestrictPathExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictPathExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "RestrictPathExclusions", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-restrict-process-exclusions",
                Label = "Restrict Process-Based Exclusion Additions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets RestrictProcessExclusions=1 to prevent users from adding new process exclusions to Windows Defender. Only centrally managed process exclusions are permitted.",
                Tags = ["defender", "exclusions", "process", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local process-based Defender exclusions blocked; reduces risk of malware self-exclusion.",
                ApplyOps = [RegOp.SetDword(ExclKey, "RestrictProcessExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictProcessExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "RestrictProcessExclusions", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-restrict-extension-exclusions",
                Label = "Restrict File Extension Exclusion Additions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets RestrictExtensionExclusions=1 to prevent users from adding file extension exclusions to Windows Defender. Extension exclusions can be abused to allow malicious file types to bypass scanning.",
                Tags = ["defender", "exclusions", "extension", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local extension exclusions blocked; .exe/.bat/.ps1 cannot be locally exempted from scanning.",
                ApplyOps = [RegOp.SetDword(ExclKey, "RestrictExtensionExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictExtensionExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "RestrictExtensionExclusions", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-restrict-ip-exclusions",
                Label = "Restrict IP Address Exclusion Additions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets RestrictIpExclusions=1 to prevent users from exempting specific IP addresses from Windows Defender network inspection. Ensures complete network traffic scanning.",
                Tags = ["defender", "exclusions", "ip", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Local IP address Defender exclusions blocked; all network traffic remains subject to inspection.",
                ApplyOps = [RegOp.SetDword(ExclKey, "RestrictIpExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictIpExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "RestrictIpExclusions", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-enable-exclusion-audit",
                Label = "Enable Defender Exclusion Audit Logging",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets EnableExclusionAudit=1 to log all Defender exclusion additions, modifications, and removals to the Windows Security event log for auditing and compliance.",
                Tags = ["defender", "exclusions", "audit", "logging", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All exclusion changes written to Security event log; enables SOC monitoring of Defender config.",
                ApplyOps = [RegOp.SetDword(ExclKey, "EnableExclusionAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "EnableExclusionAudit")],
                DetectOps = [RegOp.CheckDword(ExclKey, "EnableExclusionAudit", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-require-admin-review",
                Label = "Require Admin Review for All Exclusion Changes",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets RequireAdminReview=1 to require administrator approval before any new Defender exclusion is applied, including those submitted through the Security Center UI.",
                Tags = ["defender", "exclusions", "admin", "review", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Exclusion requests queued until an administrator approves them; prevents silent exclusion bypass.",
                ApplyOps = [RegOp.SetDword(ExclKey, "RequireAdminReview", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "RequireAdminReview")],
                DetectOps = [RegOp.CheckDword(ExclKey, "RequireAdminReview", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-block-temp-exclusions",
                Label = "Block Temporary File Path Exclusions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets BlockTempExclusions=1 to prevent exclusions that target Temp, Windows\\Temp, or user-profile temp directories. Attackers commonly add temp folder exclusions to stage malware.",
                Tags = ["defender", "exclusions", "temp", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Temp directory exclusions blocked; malware staging in temp folders remains scannable.",
                ApplyOps = [RegOp.SetDword(ExclKey, "BlockTempExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "BlockTempExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "BlockTempExclusions", 1)],
            },
            new TweakDef
            {
                Id = "defexclpol-restrict-wildcard-exclusions",
                Label = "Restrict Wildcard Path Exclusions",
                Category = "Defender Exclusions Policy",
                Description =
                    "Sets RestrictWildcardExclusions=1 to prevent wildcard (* or ?) characters in Defender exclusion paths. Wildcards can inadvertently exclude large portions of the file system.",
                Tags = ["defender", "exclusions", "wildcard", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Wildcard path exclusions blocked; only explicit exact-path exclusions are permitted.",
                ApplyOps = [RegOp.SetDword(ExclKey, "RestrictWildcardExclusions", 1)],
                RemoveOps = [RegOp.DeleteValue(ExclKey, "RestrictWildcardExclusions")],
                DetectOps = [RegOp.CheckDword(ExclKey, "RestrictWildcardExclusions", 1)],
            },
        ];
}
