// RegiLattice.Core — Tweaks/DsObjectAccessAuditPolicy.cs
// Directory Services object access and LDAP query audit policy controls (Sprint 626).
// Category: "DS Object Access Audit Policy" | Slug: dsaudit
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies\DS Access

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DsObjectAccessAuditPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\DS Access";
    private const string DetailKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Detailed Tracking";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "dsaudit-audit-directory-service-access",
            Label = "DS Audit: Enable Directory Service Object Access Auditing (LDAP Reads to Sensitive AD Objects)",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditDirectoryServiceAccess=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security event 4661 for SACL-triggered access to Active Directory objects on domain controllers — user objects, group objects, GPO links, schema attributes, and AdminSDHolder-protected objects — providing on-DC audit records of all access to sensitive AD data. " +
                "Active Directory is the crown jewel of the enterprise identity infrastructure. Without directory service access auditing, an attacker who performs an LDAP dump of all user objects (including password hint attributes, lastLogon, adminCount, userAccountControl enumeration) leaves no Security event log trace on the domain controller. With SACL-protected sensitive AD objects (all adminCount=1 objects, GPO objects, schema), directory service access events generate on every LDAP read, enabling DCSync detection and AD reconnaissance identification.",
            Tags = ["ds-audit", "directory-service", "active-directory", "ldap", "dcsync", "sacl"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AD sensitive object SACL access events generated; DCSync attack (drsuapi replication) generates detection events.",
            ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceAccess", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceAccess", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-audit-directory-service-changes",
            Label = "DS Audit: Enable Directory Service Object Modification Auditing (AD Object Changes)",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditDirectoryServiceChanges=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security events 5136 (attribute modified), 5137 (object created), 5138 (object restored from tombstone), 5139 (object moved), 5141 (object deleted) for all changes to Active Directory objects, providing a granular changelog of AD modifications. " +
                "Event 5136 is the AD schema-level modification record — it captures every attribute write to every AD object (user, group, computer, GPO, schema). Without this auditing subcategory enabled on domain controllers, the SOC has no event log record of Group Policy Object (GPO) modifications, AdminSDHolder ACL changes, Service Principal Name (SPN) additions (Kerberoasting target creation), or Domain Trust modifications (trust injection). SOC SIEM rules for GPO modification, persistence SPN addition, and trust injection all depend on Event 5136.",
            Tags = ["ds-audit", "directory-service-changes", "event-5136", "gpo", "spn", "trust-injection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AD attribute changes logged (Event 5136); GPO modifications, SPN additions, and trust changes generate SOC detection events.",
            ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceChanges", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceChanges")],
            DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceChanges", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-audit-directory-service-replication",
            Label = "DS Audit: Enable Directory Service Replication Auditing for DCSync Detection",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditDirectoryServiceReplication=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates Security events 4928 (source naming context established — replication initiated), 4929 (source naming context removed), 4930 (source naming context modified), 4931 (destination naming context modified) for AD replication operations. Enables detection of DCSync attacks performed by non-DC machines invoking DS-Replication-Get-Changes privileges. " +
                "DCSync (Mimikatz's lsadump::dcsync) mimics the behaviour of a domain controller requesting replication from another DC to obtain all account password hashes without requiring local access to the DC. The attack uses DS-Replication-Get-Changes-All privileges. Replication audit events (4928) are generated on the target DC when the replication request arrives. A 4928 event from a client workstation (not a domain controller) is a high-fidelity DCSync detection signal.",
            Tags = ["ds-audit", "replication", "event-4928", "dcsync", "ds-replication", "mimikatz"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "DS replication events audited; DCSync attack from non-DC machines generates Event 4928 — high-fidelity detection signal.",
            ApplyOps = [RegOp.SetDword(Key, "AuditDirectoryServiceReplication", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditDirectoryServiceReplication")],
            DetectOps = [RegOp.CheckDword(Key, "AuditDirectoryServiceReplication", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-audit-detailed-replication",
            Label = "DS Audit: Enable Detailed Directory Service Replication Auditing",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditDetailedDirectoryServiceReplication=3 (Success+Failure) in Advanced Audit Policy DS Access category. Generates verbose Security events 4932/4933 (synchronization of a naming context has begun/ended) and 4934/4935/4937 (attribute of AD object replicated/failed/lingering object removed) for each object-level attribute synchronisation step during AD replication, providing attribute-granular replication change records. " +
                "Detailed replication auditing provides the object-level granularity missing from standard replication auditing. When a naming context replication session (Event 4928) encompasses thousands of object changes, the standard events identify that replication occurred but not which specific objects or attributes were synchronised. Detailed replication events (4932/4934) identify the specific objects replicated in each session, enabling investigation of which specific accounts were targeted in a DCSync attack session.",
            Tags = ["ds-audit", "detailed-replication", "event-4932", "naming-context", "dcsync-detail"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Detailed replication events generated; specific objects and attributes synchronised during DCSync sessions are identifiable.",
            ApplyOps = [RegOp.SetDword(Key, "AuditDetailedDirectoryServiceReplication", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditDetailedDirectoryServiceReplication")],
            DetectOps = [RegOp.CheckDword(Key, "AuditDetailedDirectoryServiceReplication", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-enable-dpapi-activity-audit",
            Label = "DS Audit: Enable DPAPI Activity Auditing for Master Key Access Monitoring",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditDPAPIActivity=3 (Success+Failure) in Advanced Audit Policy Detailed Tracking category. Generates Security events 4692 (DPAPI backup key was requested), 4693 (DPAPI data was decrypted), 4694 (DPAPI data was encrypted), 4695 (DPAPI data was decrypted in unprotected state) for all DPAPI encryption and decryption operations. Enables detection of DPAPI master key harvesting attacks. " +
                "DPAPI master key backup operations (Event 4692) are generated when a new DPAPI master key is created and its backup is sent to the domain controller for recovery purposes. In DPAPI masterkey harvesting attacks (used by NanoDump, SharpDPAPI), an attacker requests the DPAPI backup key from the domain controller to decrypt all locally cached DPAPI blobs across the enterprise. Event 4692 from an unexpected non-system principal is a binary indicator of DPAPI master key interception.",
            Tags = ["ds-audit", "dpapi", "event-4692", "master-key", "credential-decryption", "sharpdpapi"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "DPAPI master key access events (4692) generated; DPAPI backup key harvesting attack immediately detectable.",
            ApplyOps = [RegOp.SetDword(DetailKey, "AuditDPAPIActivity", 3)],
            RemoveOps = [RegOp.DeleteValue(DetailKey, "AuditDPAPIActivity")],
            DetectOps = [RegOp.CheckDword(DetailKey, "AuditDPAPIActivity", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-enable-rpc-events-audit",
            Label = "DS Audit: Enable RPC Events Auditing for Remote Service Call Monitoring",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditRPCEvents=3 (Success+Failure) in Advanced Audit Policy Detailed Tracking category. Generates Security event 5712 (RPC connection attempt) for remote procedure call connections with caller identity, target interface UUID, and endpoint information — enabling detection of RPC-based lateral movement techniques that use Windows RPC interfaces (MS-SAMR, MS-LSAD, MS-DRSR, MS-RPRN) to access remote system resources. " +
                "Remote Printer Spooler (MS-RPRN) exploitation (PrintNightmare) and RPC-based DCSync (MS-DRSR interface calls) are primary RPC-based attack techniques. Without RPC event auditing, there is no Security event log record of specific Windows RPC interface calls made to an endpoint. RPC event audit enables detection of PrintNightmare exploitation (unexpected MS-RPRN calls from non-print-server machines) and RPC-based credential access attempts targeting SAMR and LSAD interfaces.",
            Tags = ["ds-audit", "rpc", "event-5712", "printnightmare", "ms-rprn", "samr", "lsad"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "RPC connection events (5712) generated; PrintNightmare MS-RPRN and SAMR/LSAD-based credential access detectable.",
            ApplyOps = [RegOp.SetDword(DetailKey, "AuditRPCEvents", 3)],
            RemoveOps = [RegOp.DeleteValue(DetailKey, "AuditRPCEvents")],
            DetectOps = [RegOp.CheckDword(DetailKey, "AuditRPCEvents", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-enable-central-access-policy-staging",
            Label = "DS Audit: Enable Central Access Policy Staging Audit for DAC Rule Pre-Deployment Testing",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditCentralAccessPolicyStaging=1 (Success) in Advanced Audit Policy DS Access category. Generates Security event 4818 (proposed Central Access Policy does not grant the same access permissions as the current Central Access Policy) when a proposed Dynamic Access Control policy being tested in staging mode would grant different access than the currently active policy, identifying files that would change access before the policy is deployed. " +
                "Central Access Policy staging is the Windows DAC mechanism for safely testing new classification policies before deploying them to production. Without staging audit events, IT cannot determine the blast radius of a new DAC policy change — which files would gain new access grants, which would lose existing access. Event 4818 provides a non-destructive preview showing exactly which resources would receive different access treatment under the proposed policy vs the current policy.",
            Tags = ["ds-audit", "central-access-policy", "dac", "staging", "event-4818", "policy-testing"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "DAC staging audit events (4818) generated; policy impact assessment before deployment identifies access changes without risk.",
            ApplyOps = [RegOp.SetDword(Key, "AuditCentralAccessPolicyStaging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditCentralAccessPolicyStaging")],
            DetectOps = [RegOp.CheckDword(Key, "AuditCentralAccessPolicyStaging", 1)],
        },
        new TweakDef
        {
            Id = "dsaudit-enable-certificate-services-audit",
            Label = "DS Audit: Enable Active Directory Certificate Services Audit for CA Operation Monitoring",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditCertificationServices=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security events 4870/4871/4872/4873/4874/4875/4876/4877 for all Active Directory Certificate Services operations — certificate requests (approved, denied, pending), certificate revocations, certificate template modifications, and CA role service start/stop. Critical for detecting AD CS-based privilege escalation (ESC1–ESC8 attacks). " +
                "AD Certificate Services attacks (ESC1–ESC8, as catalogued by SpecterOps) enable low-privilege users to obtain certificates that can be used for domain admin authentication or persistent machine authentication bypass. Without CS audit events, a user who requests and receives a certificate through a misconfigured template (ESC1: SANs allowed by requester) generates no Security alert. Certificate request events (4886: certificate requested, 4887: certificate issued) record the subject, certificate template, and requester — enabling detection of privilege-elevating certificate requests.",
            Tags = ["ds-audit", "ad-cs", "certificate-services", "event-4887", "esc1", "privilege-escalation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AD CS operations audited; ESC1-ESC8 certificate template abuse and rogue CA operations generate Security events.",
            ApplyOps = [RegOp.SetDword(Key, "AuditCertificationServices", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificationServices")],
            DetectOps = [RegOp.CheckDword(Key, "AuditCertificationServices", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-audit-filtering-platform-connection",
            Label = "DS Audit: Enable Windows Filtering Platform Connection Auditing for Network Profiling",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditFilteringPlatformConnection=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security events 5031 (WFP application blocked), 5150/5151 (WFP packet blocked/dropped), 5156/5157 (WFP connection allowed/blocked by application) for Windows Filtering Platform (Windows Firewall) connection decisions, providing process-to-network socket binding records without requiring Sysmon Event ID 3. " +
                "WFP connection allowed/blocked events (5156/5157) provide the same process-to-network binding information as Sysmon Event 3 but natively through Windows Security event log. Organisations that cannot deploy Sysmon can achieve equivalent network visibility using WFP auditing. Event 5156 records the process making the connection, the destination IP/port, and the protocol — enabling detection of command-and-control beaconing, lateral movement SMB connections, and data exfiltration to external IP ranges.",
            Tags = ["ds-audit", "wfp", "windows-firewall", "event-5156", "c2-detection", "network-profiling"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "WFP connection events (5156) generated natively; C2 beaconing and lateral movement network connections logged without Sysmon.",
            ApplyOps = [RegOp.SetDword(Key, "AuditFilteringPlatformConnection", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditFilteringPlatformConnection")],
            DetectOps = [RegOp.CheckDword(Key, "AuditFilteringPlatformConnection", 3)],
        },
        new TweakDef
        {
            Id = "dsaudit-audit-handle-manipulation",
            Label = "DS Audit: Enable Handle Manipulation Auditing for LSASS Memory Access Detection",
            Category = "DS Object Access Audit Policy",
            Description = "Sets AuditHandleManipulation=3 (Success+Failure) in Advanced Audit Policy Object Access category. Generates Security event 4658 (handle to object closed) and event 4690 (attempt to duplicate handle to object) that complement the SACL-based object access events — specifically Event 4690 which records attempts to duplicate an open handle to a sensitive object (such as an LSASS process handle) to a different process. " +
                "Process handle duplication is an advanced LSASS dump technique used to avoid the more detectable direct process access calls. Tools like x64dump and some variants of Cobalt Strike's in-memory credential extraction duplicate an existing handle to the LSASS process (owned by csrss.exe or another trusted process) rather than opening a new handle from a suspicious process. Event 4690 captures this handle duplication attempt, providing detection for handle-based LSASS access that bypasses protection based solely on process open calls.",
            Tags = ["ds-audit", "handle-manipulation", "event-4690", "lsass", "handle-duplication", "credential-theft"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Handle duplication (Event 4690) audited; LSASS credential dump via handle duplication technique generates detection events.",
            ApplyOps = [RegOp.SetDword(Key, "AuditHandleManipulation", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditHandleManipulation")],
            DetectOps = [RegOp.CheckDword(Key, "AuditHandleManipulation", 3)],
        },
    ];
}
