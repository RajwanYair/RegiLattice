// RegiLattice.Core — Tweaks/AuditPolicyAdvancedPolicy.cs
// Advanced Security Audit Policy and per-category audit configuration — Sprint 495.
// Category: "Audit Policy Advanced" | Slug: auditadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Audit

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AuditPolicyAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Audit";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "auditadv-force-subcategory-policy",
                Label = "Force Audit Policy Subcategory Settings Over Category",
                Category = "Audit Policy Advanced",
                Description =
                    "Forces Windows to use advanced audit policy subcategory settings (configured via auditpol.exe or Group Policy Advanced Audit) rather than the basic per-category settings from the local security policy, enabling fine-grained audit control.",
                Tags = ["audit", "audit-policy", "subcategory", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Advanced audit subcategory settings take precedence over basic category settings; fine-grained audit enabled.",
                ApplyOps = [RegOp.SetDword(Key, "SCENoApplyLegacyAuditPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SCENoApplyLegacyAuditPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "SCENoApplyLegacyAuditPolicy", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-pnp-activity-audit",
                Label = "Enable Plug and Play Activity Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of Plug and Play device connections and disconnections, generating Security event 6416 for each new external device plugged in, supporting exfiltration investigations via USB/Thunderbolt devices.",
                Tags = ["audit", "pnp", "usb", "device-connection", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PnP device audit enabled; every external device connection logged as Security event 6416.",
                ApplyOps = [RegOp.SetDword(Key, "AuditPNPActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPNPActivity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPNPActivity", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-removable-storage-audit",
                Label = "Enable Removable Storage Object Access Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of read and write access to removable storage devices, generating Security event 4663 entries for file access on USB drives, SD cards, and other removable media.",
                Tags = ["audit", "removable-storage", "file-access", "usb", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removable storage access audited; file reads and writes to USB/SD logged as Security event 4663.",
                ApplyOps = [RegOp.SetDword(Key, "AuditRemovableStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableStorage")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRemovableStorage", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-token-right-adjusted-audit",
                Label = "Enable Token Right Adjustment Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of privilege adjustments (token right changes) such as SeDebugPrivilege, SeLoadDriverPrivilege activations, generating Security event 4703 to track privilege escalation attempts.",
                Tags = ["audit", "privilege", "token-rights", "escalation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Token right adjustment audited; privilege escalation attempts logged as Security event 4703.",
                ApplyOps = [RegOp.SetDword(Key, "AuditTokenRightAdjusted", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditTokenRightAdjusted")],
                DetectOps = [RegOp.CheckDword(Key, "AuditTokenRightAdjusted", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-user-account-management-audit",
                Label = "Enable User Account Management Success and Failure Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables both success and failure auditing of user account management operations (account creation, modification, deletion, password reset, enable/disable) generating Security events 4720-4767 for compliance.",
                Tags = ["audit", "user-accounts", "account-management", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "User account management audited (success+failure); account lifecycle events logged for compliance.",
                ApplyOps = [RegOp.SetDword(Key, "AuditUserAccountManagement", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditUserAccountManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AuditUserAccountManagement", 3)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-sensitive-privilege-use-audit",
                Label = "Enable Sensitive Privilege Use Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of sensitive privilege use (e.g., acting as part of OS, taking ownership, restoring files), generating Security event 4673/4674 entries to detect abuse of powerful administrative rights.",
                Tags = ["audit", "privilege-use", "sensitive-privileges", "admin-abuse", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Sensitive privilege use audited; high-value privilege activations logged as Security events 4673/4674.",
                ApplyOps = [RegOp.SetDword(Key, "AuditSensitivePrivilegeUse", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSensitivePrivilegeUse")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSensitivePrivilegeUse", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-ipsec-driver-audit",
                Label = "Enable IPsec Driver Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of IPsec driver events including filter match, connection establishment, and connection drop events, supporting network security posture monitoring and VPN tunnel activity auditing.",
                Tags = ["audit", "ipsec", "vpn", "network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPsec driver events audited; filter matches and tunnel events logged for network security monitoring.",
                ApplyOps = [RegOp.SetDword(Key, "AuditIPsecDriver", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditIPsecDriver")],
                DetectOps = [RegOp.CheckDword(Key, "AuditIPsecDriver", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-wfp-audit",
                Label = "Enable Windows Filtering Platform (WFP) Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of Windows Filtering Platform connection permit and drop events, generating Security events 5031, 5152-5158 to support network activity analysis and firewall rule effectiveness reviews.",
                Tags = ["audit", "wfp", "firewall", "network", "connection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WFP connections audited; firewall permit and drop decisions logged as Security events 5152-5158.",
                ApplyOps = [RegOp.SetDword(Key, "AuditFilteringPlatform", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditFilteringPlatform")],
                DetectOps = [RegOp.CheckDword(Key, "AuditFilteringPlatform", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-enable-registry-audit",
                Label = "Enable Registry Object Access Audit",
                Category = "Audit Policy Advanced",
                Description =
                    "Enables auditing of registry key access and modifications when an object SACL is present, supporting post-incident forensics by recording which processes accessed security-sensitive registry keys.",
                Tags = ["audit", "registry", "object-access", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Registry object access audited; registry key reads/writes logged when SACL is set on the key.",
                ApplyOps = [RegOp.SetDword(Key, "AuditRegistryAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRegistryAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRegistryAccess", 1)],
            },
            new TweakDef
            {
                Id = "auditadv-disable-audit-policy-change-by-user",
                Label = "Block Audit Policy Changes by Non-Admin Users",
                Category = "Audit Policy Advanced",
                Description =
                    "Prevents non-administrator users from modifying audit policy settings via auditpol.exe or the Security Policy snap-in, ensuring the audit configuration cannot be weakened by standard users or compromised service accounts.",
                Tags = ["audit", "audit-policy", "tamper-protection", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Audit policy changes blocked for non-admins; security audit configuration is tamper-resistant.",
                ApplyOps = [RegOp.SetDword(Key, "BlockUserAuditPolicyChange", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUserAuditPolicyChange")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUserAuditPolicyChange", 1)],
            },
        ];
}
