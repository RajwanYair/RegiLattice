// RegiLattice.Core — Tweaks/StoragePoolPolicy.cs
// Sprint 325: Storage Pool Policy tweaks (10 tweaks)
// Category: "Storage Pool Policy" | Slug: stpool
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StoragePools

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StoragePoolPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StoragePools";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "stpool-restrict-pool-creation",
            Label = "Restrict Storage Pool Creation to Administrators",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Storage pool creation allows users to combine physical disks into virtual storage with redundancy and flexible volume management. Restricting pool creation to administrators prevents standard users from creating storage pools that could bypass access controls or disk encryption policies. Non-administrative storage pool creation could allow users to aggregate organizational disk capacity in ways that circumvent management policies. Storage pools created by regular users may not be subject to the same encryption and access control requirements as administrator-managed storage. Administrator-only pool creation ensures that all storage pool configurations are reviewed and approved before implementation. Enterprise storage management should be centrally controlled to ensure consistent encryption, backup, and access control policies.",
            Tags = ["storage", "pool", "access-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictPoolCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictPoolCreation")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictPoolCreation", 1)],
        },
        new TweakDef
        {
            Id = "stpool-enable-pool-encryption",
            Label = "Require Encryption for New Storage Pools",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Storage pool encryption protects data on pooled storage volumes when disks are removed from the system or accessed offline. Requiring encryption for new storage pools ensures that all pooled storage contains protected data consistent with enterprise encryption policies. Storage pools without encryption leave data accessible to anyone who can access the disk array outside of Windows access controls. BitLocker on virtual disks within storage pools provides encryption but pools themselves must be configured to enable transparent encryption. Encrypted storage pools ensure compliance with data protection regulations requiring encryption of data at rest regardless of storage media type. Organizations deploying storage pools for sensitive data must ensure pool-level encryption is part of the initial configuration.",
            Tags = ["storage", "pool", "encryption", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePoolEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePoolEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePoolEncryption", 1)],
        },
        new TweakDef
        {
            Id = "stpool-restrict-disk-addition",
            Label = "Restrict Disk Addition to Storage Pools",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting disk addition to authorized administrators prevents unauthorized expansion of storage pools that could dilute security controls. Only administrators with explicit authorization should be able to add physical disks to existing storage pools. Unauthorized disk addition could be used to expand storage pool capacity with uncontrolled disks that bypass encryption requirements. Rogue disk insertion is a threat in physical access scenarios where an attacker adds a disk to a pool to capture data written to the pool. Restricting disk addition requires administrator authorization for any physical disk changes to managed storage pools. Enterprise storage management tools should log disk addition events for audit purposes alongside this administrative restriction.",
            Tags = ["storage", "pool", "disk-management", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDiskAddition", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDiskAddition")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDiskAddition", 1)],
        },
        new TweakDef
        {
            Id = "stpool-disable-thin-provisioning",
            Label = "Disable Thin Provisioning for Storage Pools",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Thin provisioning allows storage pool virtual disks to be allocated beyond the physical capacity with storage committed as data is written. Disabling thin provisioning for managed storage prevents over-allocation scenarios that can lead to data loss when storage exhaustion occurs. Thin-provisioned pools can become fully consumed unexpectedly causing virtual disk failures when logical capacity exceeds physical storage. Fixed provisioning ensures that allocated storage is backed by physical capacity eliminating surprise storage exhaustion. Enterprise storage management should use fixed provisioning for critical workloads where data loss due to storage exhaustion is unacceptable. Monitoring storage pool utilization is still important for fixed pools but eliminates the risk of logical-to-physical storage mismatch.",
            Tags = ["storage", "pool", "provisioning", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableThinProvisioning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableThinProvisioning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableThinProvisioning", 1)],
        },
        new TweakDef
        {
            Id = "stpool-enable-integrity-streams",
            Label = "Enable Storage Pool Integrity Streams",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Storage pool integrity streams provide end-to-end data integrity verification using checksums to detect silent data corruption. Enabling integrity streams ensures that data read from storage pools is verified against stored checksums and corruption is detected. Silent data corruption can occur due to firmware bugs, electrical issues, or hardware failures causing data to be written incorrectly. Integrity streams in Storage Spaces are stored in a parallel data stream that tracks checksums for each data block. When corruption is detected in a mirrored pool integrity streams can automatically repair corrupted data blocks using the mirror copy. Organizations storing critical data in storage pools should enable integrity streams to protect against bit-rot and silent disk corruption.",
            Tags = ["storage", "pool", "integrity", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityStreams", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityStreams")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityStreams", 1)],
        },
        new TweakDef
        {
            Id = "stpool-require-mirroring",
            Label = "Require Mirroring for Storage Pool Virtual Disks",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Storage pool mirroring maintains multiple copies of data across different physical disks providing protection against disk failure. Requiring mirroring ensures that all storage pool virtual disks have redundancy through two-way or three-way mirror configurations. Simple (no redundancy) storage pool disks provide no protection against disk failure and should not be used for critical data. Mirror resiliency in Storage Spaces ensures continued operation and data preservation when one or more disks fail in the pool. Organizations storing critical operational data in storage pools must use mirroring or parity configurations to meet availability requirements. Requiring mirroring through policy prevents accidental creation of simple virtual disks that lack redundancy for critical workloads.",
            Tags = ["storage", "pool", "mirroring", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireMirroredRedundancy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireMirroredRedundancy")],
            DetectOps = [RegOp.CheckDword(Key, "RequireMirroredRedundancy", 1)],
        },
        new TweakDef
        {
            Id = "stpool-enable-pool-health-monitoring",
            Label = "Enable Storage Pool Health Event Monitoring",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Storage pool health monitoring tracks the operational state of storage pools and virtual disks generating events for degraded or warning conditions. Enabling health monitoring ensures that disk failures, pool degradation, and capacity warnings generate Windows events for operational visibility. Pool health events allow administrators to respond to degraded mirror states before data loss occurs from a second disk failure. Storage Spaces health events are logged to the System event log and can be monitored through Windows Admin Center or SCOM. Automated alerting on pool health events reduces the response time to disk failures and prevents extended exposure to single-disk pools. Health monitoring combined with regular operational reviews ensures storage infrastructure operates within designed parameters.",
            Tags = ["storage", "pool", "health", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePoolHealthMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePoolHealthMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePoolHealthMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "stpool-restrict-pool-deletion",
            Label = "Restrict Storage Pool Deletion to Administrators",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting storage pool deletion prevents accidental or malicious removal of storage pools that would destroy all contained virtual disk data. Administrator-only pool deletion ensures that significant storage configuration changes require elevated authorization. Ransomware has been observed deleting or corrupting storage pool configurations to maximize damage and complicate recovery. Non-administrative deletion restriction reduces the impact of compromised standard user accounts on storage infrastructure. Pool deletion restrictions combined with confirmation dialogs provide additional safeguards against accidental data destruction. Regular backup of storage pool configuration alongside data backup ensures recovery capability after intentional or accidental deletion.",
            Tags = ["storage", "pool", "deletion", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictPoolDeletion", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictPoolDeletion")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictPoolDeletion", 1)],
        },
        new TweakDef
        {
            Id = "stpool-disable-deduplication-auto",
            Label = "Disable Automatic Data Deduplication on Storage Pools",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Automatic data deduplication reduces storage consumption by identifying and eliminating duplicate data blocks but has CPU and memory implications. Disabling automatic deduplication prevents unscheduled deduplication operations that can impact system performance during business hours. Deduplication should be explicitly configured and scheduled by administrators rather than running automatically without capacity planning. Some data types including encrypted files and compressed media do not benefit from deduplication and the overhead is wasted on these workloads. Deduplication on storage pools with integrity streams also requires careful configuration to maintain both features correctly. Organizations using deduplication should enable it with explicit schedules during low-activity periods rather than automatic unscheduled runs.",
            Tags = ["storage", "pool", "deduplication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoDeduplication", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDeduplication")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoDeduplication", 1)],
        },
        new TweakDef
        {
            Id = "stpool-audit-pool-changes",
            Label = "Enable Storage Pool Configuration Change Auditing",
            Category = "Storage Pool Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Storage pool configuration change auditing records modifications to pool membership, virtual disk configurations, and resiliency settings. Enabling pool change auditing generates security events for all administrative changes to storage pool configurations. Unexpected changes to storage pool configurations may indicate unauthorized administrative access or insider threat activity. Audit records of pool configuration changes support change management processes and provide evidence for security investigations. Storage pool change events should be forwarded to SIEM infrastructure and correlated with administrator login events. Baseline documentation of storage pool configurations combined with change auditing enables rapid detection of unauthorized modifications.",
            Tags = ["storage", "pool", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditPoolConfigChanges", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditPoolConfigChanges")],
            DetectOps = [RegOp.CheckDword(Key, "AuditPoolConfigChanges", 1)],
        },
    ];
}
