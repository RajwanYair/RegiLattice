// RegiLattice.Core — Tweaks/AdReplicationPolicy.cs
// Active Directory Replication & Site-Link Security Policy — Sprint 580.
// Configures AD replication partner authentication, replication traffic
// encryption, site-link cost hardening, and replication metadata protection.
// Category: "AD Replication Policy" | Slug: adrep
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System
//           HKLM\SYSTEM\CurrentControlSet\Services\NTDS\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AdReplicationPolicy
{
    private const string NtdsPolicyKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NTDS\Parameters";

    private const string SystemPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "adrep-require-ntds-replication-sign-seal",
                Label = "AD Replication: Require NTDS Replication Traffic Sign-and-Seal",
                Category = "AD Replication Policy",
                Description =
                    "Sets ReplicationSignAndSeal=1 in NTDS\\Parameters. Requires that Active Directory replication traffic between domain controllers is both signed (integrity-protected) and sealed (encrypted). AD replication carries all directory changes — new accounts, password updates, group membership changes, and computer policy settings. If replication traffic is unprotected, an attacker who can perform a man-in-the-middle attack on DC-to-DC traffic can inject or modify replication data, potentially escalating privileges by injecting account changes. Sign-and-seal ensures all replication traffic is authenticated and encrypted.",
                Tags = ["ntds", "replication", "sign-seal", "dc-to-dc", "encryption"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "NTDS replication traffic requires sign-and-seal. Additional CPU overhead for encryption on DCs; negligible for modern hardware. Old DCs (pre-Windows 2000) cannot participate in signed/sealed replication — only relevant for very old mixed-mode domains.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "ReplicationSignAndSeal", 1)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "ReplicationSignAndSeal")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "ReplicationSignAndSeal", 1)],
            },
            new TweakDef
            {
                Id = "adrep-set-tomb-stone-lifetime-180days",
                Label = "AD Replication: Set Active Directory Tombstone Lifetime to 180 Days",
                Category = "AD Replication Policy",
                Description =
                    "Sets TombstoneLifetime=180 in NTDS\\Parameters (units: days). Sets the AD tombstone lifetime to 180 days. When an object is deleted in AD, it becomes a tombstone — a marker that propagates the deletion to all DCs before the tombstone is permanently removed. If a DC is offline longer than the tombstone lifetime, it must be forcibly re-joined to the domain (a USN rollback scenario) or reinstalled. 60 days (the old default) is insufficient for quarterly disaster recovery testing cycles. 180 days ensures that DCs recovered from quarterly backup snapshots are still within the tombstone window and can be safely re-brought online without forced rejoin.",
                Tags = ["ntds", "tombstone", "backup-recovery", "deleted-objects", "replication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Tombstone lifetime extended to 180 days. Deleted AD objects remain as tombstones for 180 days before permanent removal. Domain controllers that have been offline for more than 180 days will require forced rejoin. Increases the size of the deleted-objects container in the AD database.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "TombstoneLifetime", 180)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "TombstoneLifetime")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "TombstoneLifetime", 180)],
            },
            new TweakDef
            {
                Id = "adrep-enable-strict-replication-consistency",
                Label = "AD Replication: Enable Strict Replication Consistency Mode",
                Category = "AD Replication Policy",
                Description =
                    "Sets Strict Replication Consistency=1 in NTDS\\Parameters. Enables strict replication consistency, which causes NTDS to disable replication from a replication partner that has an out-of-date replication topology (i.e., has missed more than MaxConsistencyCheckPercent of updates). Without strict consistency, AD will attempt to 'loose' replicate with lagged partners even if that results in duplicate GUID conflicts or lingering objects. In lingering object scenarios (DCs that have been offline past the tombstone lifetime), strict mode prevents corrupted data from being silently re-introduced into the directory by an out-of-date DC.",
                Tags = ["ntds", "replication-consistency", "lingering-objects", "strict-mode", "integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Strict replication consistency enforced. DCs with excessive replication lag will have replication blocked until the issue is resolved. May generate replication errors (Event ID 1388) in environments with intermittent DC connectivity. Monitor NTDS events after enabling — address any replication lag issues before enforcing.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "Strict Replication Consistency", 1)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "Strict Replication Consistency")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "Strict Replication Consistency", 1)],
            },
            new TweakDef
            {
                Id = "adrep-enable-dns-consistency-check",
                Label = "AD Replication: Enable DNS Consistency Check During Promotion",
                Category = "AD Replication Policy",
                Description =
                    "Sets DnsAvoidRegisterRecords=0 in NTDS\\Parameters. Ensures that all required DNS records for the domain controller are registered during or after promotion, and that the DNS consistency check is not bypassed. DC promotion attempts with unresolvable DNS names or misconfigured DNS zones that bypass the DNS check can result in DCs that are partially functional but not properly reachable by other DCs — leading to intermittent replication failures that are hard to diagnose. Ensuring DNS consistency is enforced catches DNS misconfigurations at promotion time rather than as production replication failures.",
                Tags = ["ntds", "dns", "consistency-check", "promotion", "dc-registration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "DNS record registration for this DC is enabled (DnsAvoidRegisterRecords=0). DC registers all required DNS SRV and A records. In split-DNS environments, verify the DC can register records in both internal and external DNS zones as appropriate.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "DnsAvoidRegisterRecords", 0)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "DnsAvoidRegisterRecords")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "DnsAvoidRegisterRecords", 0)],
            },
            new TweakDef
            {
                Id = "adrep-restrict-ad-single-object-recovery",
                Label = "AD Replication: Enable AD Recycle Bin (Prevent Immediate Object Purge)",
                Category = "AD Replication Policy",
                Description =
                    "Sets EnabledScopes=1 in NTDS\\Parameters. Enables the Active Directory Recycle Bin feature flag on this DC. The AD Recycle Bin preserves deleted objects (with all attributes including group memberships) for the deleted-object lifetime (default 180 days), making it possible to restore accidentally deleted user accounts, OUs, or groups without authoritative restore from backup. Without the Recycle Bin, deleted objects immediately lose most attributes and recovery requires authoritative NTDS restore or backup-based object recovery. This is a forest-level feature that must be enabled via PowerShell on the Schema Master (Enable-ADOptionalFeature) — this policy flag enables the local DC to participate.",
                Tags = ["ntds", "recycle-bin", "object-recovery", "deleted-objects", "resilience"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Local DC participates in AD Recycle Bin. Requires the AD Recycle Bin optional feature to be enabled at the forest level first (Enable-ADOptionalFeature on Schema Master). Once enabled, cannot be reversed without forest recovery. Increases NTDS.dit database size due to full attribute retention for deleted objects.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "EnabledScopes", 1)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "EnabledScopes")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "EnabledScopes", 1)],
            },
            new TweakDef
            {
                Id = "adrep-set-max-replication-failures-5",
                Label = "AD Replication: Alert on More Than 5 Consecutive Replication Failures",
                Category = "AD Replication Policy",
                Description =
                    "Sets MaxConsistencyCheckPercent=5 in NTDS\\Parameters. Sets the threshold at which consecutive replication failures from a partner trigger a consistency check alert to 5 failures. By default, NTDS tolerates a high number of consecutive replication failures before logging a critical event or taking action. Setting a lower threshold ensures that replication health degradation is detected and reported early — critical for catching incidents where an attacker disrupts replication to prevent domain-wide propagation of security policy changes or account lockouts.",
                Tags = ["ntds", "replication-failure", "alerting", "consistency", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Replication failure alerting triggers after 5 consecutive failures from a single replication partner. Event ID 1308 logged. In environments with intermittent network connectivity between sites, this threshold may generate false-positive events. Monitor and tune based on the expected replication reliability in your site-link topology.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "MaxConsistencyCheckPercent", 5)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "MaxConsistencyCheckPercent")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "MaxConsistencyCheckPercent", 5)],
            },
            new TweakDef
            {
                Id = "adrep-enable-ad-audit-log-policy-access",
                Label = "AD Replication: Enable Audit of AD DS Access Policy Operations",
                Category = "AD Replication Policy",
                Description =
                    "Sets AuditPolicySubcategory=1 in NTDS\\Parameters. Enables auditing of Active Directory Service access subcategory events. These events include access to sensitive AD objects (krbtgt account reads, Domain Admins group modifications, Schema changes, replication metadata access), NTDS database file access, and NTDS parameter changes. Without this audit, an attacker who accesses sensitive AD objects (e.g., DCSync — requesting replication metadata from a DC to extract all password hashes) leaves no event log trail. With audit enabled, DCSync attempts generate replication audit events (EventID 4662, 4928) that can be detected by SIEM.",
                Tags = ["ntds", "audit", "dcsync", "replication-access", "event-4662"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "AD DS access auditing enabled. DCSync-like replication requests (DS-Replication-Get-Changes-All) will generate Event ID 4662 in the Security log with GUID of the GUID accessed. SOC can detect DCSync attacks in real-time. May generate significant event log volume in large forests with active AD replication.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "AuditPolicySubcategory", 1)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "AuditPolicySubcategory")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "AuditPolicySubcategory", 1)],
            },
            new TweakDef
            {
                Id = "adrep-enable-ntds-encrypted-communication",
                Label = "AD Replication: Enable NTDS RPC Encrypted Communication Channel",
                Category = "AD Replication Policy",
                Description =
                    "Sets EncryptRpcCommunication=1 in NTDS\\Parameters. Enables RPC encryption for AD replication traffic between domain controllers. AD DS replication uses Microsoft RPC over TCP for inter-DC communication. Enabling RPC encryption ensures that the payload of replication packets (directory object changes, attribute updates, password hash data) is encrypted in transit between DCs. This is layered protection on top of sign-and-seal — even if sign-and-seal at the NTDS layer is bypassed, the RPC transport layer encryption provides an additional barrier.",
                Tags = ["ntds", "rpc", "encryption", "replication", "transport-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "RPC encryption enabled for NTDS replication traffic. Additional CPU overhead from AES encryption of replication packets. On modern DC hardware (Xeon, EPYC) AES-NI instructions keep overhead below 1%. Verify no inter-DC firewall rules block the dynamic RPC port range (49152–65535) after enabling.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "EncryptRpcCommunication", 1)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "EncryptRpcCommunication")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "EncryptRpcCommunication", 1)],
            },
            new TweakDef
            {
                Id = "adrep-set-ad-query-policy-max-objects-10000",
                Label = "AD Replication: Cap AD DS Directory Queries to 10000 Objects Per Operation",
                Category = "AD Replication Policy",
                Description =
                    "Sets MaxTempTableSize=10000 in NTDS\\Parameters (units: objects). Limits the number of objects returned in a single AD query operation to 10,000. Unrestricted AD queries can consume significant DC CPU and memory — an attacker with LDAP read access who issues an unbounded subtree search against the entire domain partition can cause a DC denial-of-service by forcing it to process a millions-of-results query. Setting a per-query object cap ensures that even large LDAP clients must paginate, distributing the query load over time and preventing single-query DC saturation.",
                Tags = ["ntds", "query-limit", "dos-mitigation", "ldap", "pagination"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "AD directory queries capped at 10,000 objects per operation. Clients requiring more than 10,000 results must use LDAP paged results control. Modern enterprise applications (Azure AD Connect, ADMT, Quest Migration Manager) handle paging natively. Custom scripts using unbounded LDAP searches must be updated.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "MaxTempTableSize", 10000)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "MaxTempTableSize")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "MaxTempTableSize", 10000)],
            },
            new TweakDef
            {
                Id = "adrep-enable-rid-master-audit",
                Label = "AD Replication: Enable RID (Relative Identifier) Pool Allocation Audit",
                Category = "AD Replication Policy",
                Description =
                    "Sets AuditRidAllocation=1 in NTDS\\Parameters. Enables auditing of RID (Relative Identifier) pool allocation requests. Domain controllers request blocks of RIDs from the RID Master FSMO role to assign unique object SIDs when creating new AD objects. An unusually high rate of RID pool requests from a specific DC (e.g., thousands of allocations per day) may indicate automated object creation — a technique used by ransomware operators to create new privileged accounts en masse or by red teams performing domain object flooding. Auditing RID allocation enables detection of anomalous object creation bursts.",
                Tags = ["ntds", "rid", "rid-master", "object-creation", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "RID pool allocation requests are audited. Generates Event ID 16657 in Directory Service log when a DC requests a new RID pool. Baseline normal RID allocation frequency for your environment (typically 1 pool every few months per active DC). Alert on abnormal frequency as potential ransomware or red-team indicator.",
                ApplyOps = [RegOp.SetDword(NtdsPolicyKey, "AuditRidAllocation", 1)],
                RemoveOps = [RegOp.DeleteValue(NtdsPolicyKey, "AuditRidAllocation")],
                DetectOps = [RegOp.CheckDword(NtdsPolicyKey, "AuditRidAllocation", 1)],
            },
        ];
}
