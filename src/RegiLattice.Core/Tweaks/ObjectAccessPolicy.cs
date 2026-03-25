// RegiLattice.Core — Tweaks/ObjectAccessPolicy.cs
// Sprint 324: Object Access Policy tweaks (10 tweaks)
// Category: "Object Access Policy" | Slug: objacs
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ObjectAccess

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ObjectAccessPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ObjectAccess";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "objacs-enable-file-system-auditing",
            Label = "Enable File System Object Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "File system object access auditing records access to files and directories that have SACL entries configured for auditing. Enabling file system auditing generates security events for file access operations including read, write, create, and delete when the object's SACL requests auditing. File access auditing is essential for detecting unauthorized access to sensitive files and directories in enterprise environments. Security Event Log events 4663 and 4656 record file access with details about the user, process, file path, and access type. File system auditing log data supports DLP investigations, insider threat detection, and forensic analysis after security incidents. Organizations should configure SACLs on sensitive directories and enable this policy to ensure audit events are generated.",
            Tags = ["object-access", "file-system", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableFileSystemAuditing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFileSystemAuditing")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFileSystemAuditing", 1)],
        },
        new TweakDef
        {
            Id = "objacs-enable-registry-auditing",
            Label = "Enable Registry Object Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Registry object access auditing records access to registry keys that have SACL entries configured requesting audit events. Enabling registry auditing generates security events for registry read and write operations on monitored keys. Registry modification auditing is critical for detecting persistence mechanisms that write to run keys, service configurations, and authentication providers. Security Event Log events 4663 and 4657 record registry access with account, key path, and operation type information. Registry auditing of sensitive keys like HKLM\\SYSTEM\\CurrentControlSet\\Services provides early warning of service-based persistence. Organizations should configure SACLs on high-value registry paths and ensure this policy is enabled for audit event generation.",
            Tags = ["object-access", "registry", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRegistryAuditing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRegistryAuditing")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRegistryAuditing", 1)],
        },
        new TweakDef
        {
            Id = "objacs-enable-kernel-object-auditing",
            Label = "Enable Kernel Object Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Kernel object auditing records access to kernel objects such as mutexes, semaphores, and event objects that have SACL-based audit entries. Enabling kernel object auditing provides visibility into inter-process synchronization and communication through kernel objects. Malware commonly uses named kernel objects for synchronization and coordination between malicious processes in multi-stage attacks. Security events for kernel object access help identify attacker-created synchronization primitives used for process coordination. Kernel object auditing is lower volume than file system auditing but provides targeted visibility into process behavior. High-value kernel objects like named mutexes known to be used by specific malware families should be configured with SACLs.",
            Tags = ["object-access", "kernel", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableKernelObjectAuditing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelObjectAuditing")],
            DetectOps = [RegOp.CheckDword(Key, "EnableKernelObjectAuditing", 1)],
        },
        new TweakDef
        {
            Id = "objacs-audit-sam-access",
            Label = "Enable SAM Database Object Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SAM database object access auditing records attempts to access local account credentials stored in the Security Account Manager database. Enabling SAM access auditing generates security events when processes attempt to open the SAM database for credential access. SAM database access is a common credential harvesting technique used by tools like Mimikatz and similar password dumping utilities. Security Event Log event 4661 records SAM object access with the requesting account and process identifier for forensic analysis. SAM access auditing helps detect credential dumping activity even when it occurs through APIs rather than raw disk access. Detecting SAM access events should be correlated with other artifacts like LSASS process access and unusual administrative tool execution.",
            Tags = ["object-access", "sam", "credentials", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditSAMAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditSAMAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditSAMAccess", 1)],
        },
        new TweakDef
        {
            Id = "objacs-audit-lsass-access",
            Label = "Enable LSASS Process Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "LSASS process object auditing records attempts by other processes to open handles to the LSASS process with credential-reading access rights. Enabling LSASS access auditing generates security events when processes attempt to read memory from the Local Security Authority Server Service. Credential dumping tools including Mimikatz, Procdump, and comsvcs.dll extraction all require opening LSASS with PROCESS_VM_READ permissions. Security Event Log event 4656 and 10 from Sysmon can detect LSASS credential access attempts from unauthorized processes. LSASS access detection is one of the most important detections for credential-based lateral movement in enterprise environments. Detecting LSASS access should trigger immediate investigation as legitimate software rarely accesses LSASS process memory.",
            Tags = ["object-access", "lsass", "credentials", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditLSASSAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditLSASSAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditLSASSAccess", 1)],
        },
        new TweakDef
        {
            Id = "objacs-enable-detailed-file-share-audit",
            Label = "Enable Detailed File Share Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Detailed file share auditing records individual file-level access within network shares rather than just share connection events. Enabling detailed file share auditing generates security events with specific file paths, access types, and requestor identities for all share file access. Standard file share auditing only records share connections but detailed auditing provides visibility into which specific files are accessed. Detailed file share audit events are more voluminous than connection-level events and may require additional log infrastructure capacity. Security Event Log event 5145 records detailed file share access with object name, access mask, and account information. Detailed file share auditing is valuable for DLP scenarios and post-incident investigation of data access patterns.",
            Tags = ["object-access", "file-share", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDetailedFileShareAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedFileShareAudit")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDetailedFileShareAudit", 1)],
        },
        new TweakDef
        {
            Id = "objacs-audit-removable-storage",
            Label = "Enable Removable Storage Access Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Removable storage object access auditing records all file access and write operations to USB drives, external hard drives, and other removable media. Enabling removable storage auditing generates security events when users read from or write to removable storage devices. Data exfiltration via USB is a persistent insider threat vector and removable storage auditing provides the evidence chain needed for investigation. Security Event Log event 4663 with object type Removable Storage records the file path, access type, and user for each removable storage operation. Removable storage audit events should be correlated with USB device connection events to identify devices connected for purpose of data exfiltration. Removable storage auditing is most valuable in combination with removable storage access restrictions to detect circumvention attempts.",
            Tags = ["object-access", "removable-storage", "usb", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditRemovableStorageAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableStorageAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditRemovableStorageAccess", 1)],
        },
        new TweakDef
        {
            Id = "objacs-audit-cert-services",
            Label = "Enable Certification Authority Object Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Certificate Authority object access auditing records access to CA database objects and certificate management operations. Enabling CA object auditing generates security events for certificate issuance, revocation, template access, and CA configuration changes. Unauthorized certificate issuance from enterprise CAs is a serious threat enabling creation of forged authentication certificates. Security Event Log events 4874, 4875, and related CA events record certificate operations with requestor identity and certificate details. CA object auditing is essential for detecting certificate-based attacks including unauthorized administrator certificate issuance for authentication bypass. CA audit events should be aggregated with other PKI infrastructure events for comprehensive monitoring.",
            Tags = ["object-access", "pki", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditCertificationServicesAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificationServicesAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditCertificationServicesAccess", 1)],
        },
        new TweakDef
        {
            Id = "objacs-audit-object-handle-manipulation",
            Label = "Enable Object Handle Manipulation Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Object handle manipulation auditing records when handles to auditable objects are created or closed providing a complete access lifecycle view. Enabling handle manipulation auditing generates security events for handle creation and close operations that bracket actual object access. Handle auditing provides context for other object access events by establishing when access windows opened and closed. Security Event Log event 4659 records object deletion after handle closure providing tracking for file deletion operations. Handle lifecycle auditing is used in detailed forensic analysis to reconstruct object access timelines. Handle manipulation events on critical objects like SAM, LSASS, and sensitive files provide complementary evidence for access investigations.",
            Tags = ["object-access", "handles", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditHandleManipulation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditHandleManipulation")],
            DetectOps = [RegOp.CheckDword(Key, "AuditHandleManipulation", 1)],
        },
        new TweakDef
        {
            Id = "objacs-audit-central-access-policy",
            Label = "Enable Central Access Policy Staging Auditing",
            Category = "Object Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Central Access Policy staging auditing records what central access policy would have done when applied to object access requests before policies are enforced. Enabling staging audit mode generates security events showing how new central access policies would affect access without blocking current users. CAP staging allows administrators to test Dynamic Access Control policies and identify unexpected effects before enforcement. Security Event Log events in staging mode identify which policy expressions matched and what access decisions would result. Staging audit data enables policy refinement to remove overly restrictive rules that would block legitimate access. Central access policy staging is essential for large enterprise DAC deployments where policy errors could affect many users.",
            Tags = ["object-access", "central-access", "dac", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditCentralAccessPolicyStaging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditCentralAccessPolicyStaging")],
            DetectOps = [RegOp.CheckDword(Key, "AuditCentralAccessPolicyStaging", 1)],
        },
    ];
}
