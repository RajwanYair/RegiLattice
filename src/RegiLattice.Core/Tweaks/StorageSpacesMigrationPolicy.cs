// RegiLattice.Core — Tweaks/StorageSpacesMigrationPolicy.cs
// Storage Spaces Migration Policy — Sprint 560.
// Configures Group Policy for Windows Storage Spaces and the Storage Migration
// Service: pool tiering policy, drive retirement behaviour, volume
// migration orchestration, and fault domain management.
// Category: "Storage Spaces Migration Policy" | Slug: ssmig
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageSpaces
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageMigrationService

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageSpacesMigrationPolicy
{
    private const string SsKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSpaces";

    private const string SmsKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageMigrationService";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ssmig-require-storage-encryption",
                Label = "Storage Spaces Migration: Require Encryption on All Storage Pools",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets RequireEncryption=1 in StorageSpaces policy. Requires all Storage Spaces pools managed by Group Policy to be encrypted with BitLocker Drive Encryption. When a new pool is created or an existing pool is brought online, the policy mandates that its volumes are protected by BitLocker. Storage pools without encryption are flagged and can be quarantined by this policy. Protects against direct disk extraction attacks where physical drives removed from a Storage Spaces mirror could be read on another system.",
                Tags = ["storage-spaces", "encryption", "bitlocker", "data-protection", "pool"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "All Storage Spaces pools must be BitLocker-encrypted. Pools without encryption may have restricted write access or be unable to come online. Requires BitLocker policies and TPM to be configured.",
                ApplyOps = [RegOp.SetDword(SsKey, "RequireEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "RequireEncryption")],
                DetectOps = [RegOp.CheckDword(SsKey, "RequireEncryption", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-enable-auto-tiering",
                Label = "Storage Spaces Migration: Enable Automatic Tiering (SSD+HDD)",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets AutoTiering=1 in StorageSpaces policy. Enables automatic storage tiering in tiered Storage Spaces pools. Storage Spaces Direct and standard tiered pools can place hot (frequently accessed) data on NVMe or SSD tiers and cold data on HDD tiers automatically. Without this policy, tiering must be explicitly configured per-volume. Automatic tiering monitors access patterns over a 24-hour window and promotes/demotes data blocks accordingly. For mixed SSD+HDD pools, this significantly improves read performance for hot data without requiring manual optimisation.",
                Tags = ["storage-spaces", "tiering", "ssd", "hdd", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Hot data blocks are automatically promoted to SSD tier; cold data demoted to HDD. Requires a tiered Storage Spaces pool with both SSD and HDD tiers. Tiering optimisation runs as a background service during low-activity periods.",
                ApplyOps = [RegOp.SetDword(SsKey, "AutoTiering", 1)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "AutoTiering")],
                DetectOps = [RegOp.CheckDword(SsKey, "AutoTiering", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-enable-proactive-drive-retirement",
                Label = "Storage Spaces Migration: Enable Proactive Drive Retirement on SMART Failure",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets ProactiveDriveRetirement=1 in StorageSpaces policy. Enables Storage Spaces to automatically retire (mark as unavailable) a member drive when it reports SMART (Self-Monitoring, Analysis and Reporting Technology) drive health warnings indicating impending failure. When a drive is retired, Storage Spaces redistributes its data to healthy pool members if the pool has sufficient redundancy. Without proactive retirement, a failing drive stays in the pool until it actually fails — at which point data reconstruction is urgent and failure of a second drive during rebuild can cause data loss.",
                Tags = ["storage-spaces", "smart", "drive-failure", "resilience", "retirement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Drives with SMART warnings are proactively removed from the pool and rebuilt. Pool rebuilds consume IO bandwidth and time. Pool must have sufficient spare capacity or a hot spare drive. Recommended for all production Storage Spaces deployments.",
                ApplyOps = [RegOp.SetDword(SsKey, "ProactiveDriveRetirement", 1)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "ProactiveDriveRetirement")],
                DetectOps = [RegOp.CheckDword(SsKey, "ProactiveDriveRetirement", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-disable-sms-auto-credential-store",
                Label = "Storage Spaces Migration: Disable SMS Automatic Credential Storage",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets DisableCredentialStorage=1 in StorageMigrationService policy. Prevents the Storage Migration Service orchestrator from storing source server credentials in Windows Credential Manager. SMS requires credentials to access source servers for inventory and file transfer. By default these credentials are cached in Credential Manager for subsequent runs. Persistent credential storage creates a risk: an attacker who compromises the orchestrator server gains stored credentials to all migrated source servers. Disabling storage forces IT to supply credentials explicitly for each migration job.",
                Tags = ["storage-migration", "credentials", "security", "credential-manager", "sms"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SMS does not cache source server credentials. Migration job operators must enter credentials each time they run an inventory or transfer. Prevents credential theft if the orchestrator server is compromised.",
                ApplyOps = [RegOp.SetDword(SmsKey, "DisableCredentialStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(SmsKey, "DisableCredentialStorage")],
                DetectOps = [RegOp.CheckDword(SmsKey, "DisableCredentialStorage", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-enable-pool-fault-domains",
                Label = "Storage Spaces Migration: Enable Fault Domain Awareness in Pools",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets EnableFaultDomains=1 in StorageSpaces policy. Enables fault domain awareness when placing data in Storage Spaces pools. When fault domains are configured (chassis, rack, site), Storage Spaces places mirror data copies and parity stripes in separate fault domains. Without fault domain placement, a three-way mirror might place all three copies on drives in the same chassis — losing the chassis loses all copies. With fault domain placement, each mirror copy resides in a different chassis/rack/site, surviving physical failures that take out an entire enclosure.",
                Tags = ["storage-spaces", "fault-domain", "resilience", "ssd", "mirror"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Data copies are placed in separate fault domains (chassis/rack). Pools without configured fault domains are unaffected. Requires Storage Spaces Direct or multi-enclosure pool configuration with defined fault domains in Windows Admin Center or PowerShell.",
                ApplyOps = [RegOp.SetDword(SsKey, "EnableFaultDomains", 1)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "EnableFaultDomains")],
                DetectOps = [RegOp.CheckDword(SsKey, "EnableFaultDomains", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-require-sms-encrypted-transfer",
                Label = "Storage Spaces Migration: Require Encrypted File Transfer",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets RequireEncryptedTransfer=1 in StorageMigrationService policy. Requires the Storage Migration Service to use encrypted SMB3 connections when transferring files from source to destination servers. File migration traffic often traverses internal networks that may have insufficient network-level controls. Without encrypted transfer, an attacker with network capture capability on the migration network can read file contents as they are transferred. SMB3 encryption wraps all transfer traffic in AES-CCM, preventing interception during potentially hours-long migration windows.",
                Tags = ["storage-migration", "encryption", "smb3", "data-protection", "transfer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All SMS file transfers use SMB3 encryption. Source and destination servers must support SMB3 encryption (Server 2012+). Migration throughput reduced by ~15-25% due to encryption overhead; acceptable for most migrations.",
                ApplyOps = [RegOp.SetDword(SmsKey, "RequireEncryptedTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(SmsKey, "RequireEncryptedTransfer")],
                DetectOps = [RegOp.CheckDword(SmsKey, "RequireEncryptedTransfer", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-disable-pool-write-without-cache",
                Label = "Storage Spaces Migration: Disable Pool Writes Without Cache Drive",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets RequireCacheDrive=1 in StorageSpaces policy. Prevents Storage Spaces pools from accepting writes unless a cache drive (NVMe or SSD) is available and healthy. Without a write cache, Storage Spaces Direct clusters accept writes directly to spinning disk — dramatically increasing write latency and reducing IOps. In environments where performance targets depend on the write cache, silently running without a cache (e.g., after a cache drive fails and is removed) can cause application-level performance degradation that is difficult to diagnose. This policy makes the cache absence immediately apparent.",
                Tags = ["storage-spaces", "cache", "write-cache", "performance", "s2d"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Pool writes are rejected if no cache drive is healthy. Loss of a cache drive in a Storage Spaces Direct cluster causes writes to pause until a replacement cache drive is added. Requires monitoring and rapid cache drive replacement SLA.",
                ApplyOps = [RegOp.SetDword(SsKey, "RequireCacheDrive", 1)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "RequireCacheDrive")],
                DetectOps = [RegOp.CheckDword(SsKey, "RequireCacheDrive", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-enable-sms-audit-log",
                Label = "Storage Spaces Migration: Enable Storage Migration Service Audit Log",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets AuditLogEnabled=1 in StorageMigrationService policy. Enables audit logging for both the Storage Migration Service orchestrator and proxy agents. Audit log entries record: who initiated a migration job, what source servers were inventoried, which files were transferred, when cutover was performed, and which security groups were migrated. Without an audit trail, compliance requirements (SOC 2, ISO 27001) for data migration events cannot be satisfied. Log entries are written to the SMS event channel and optionally forwarded to a SIEM.",
                Tags = ["storage-migration", "audit", "logging", "compliance", "sms"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SMS records detailed audit events during inventory, transfer, and cutover operations. Events written to Application and Services Logs\\Microsoft\\Windows\\StorageMigrationService. Minimal performance impact.",
                ApplyOps = [RegOp.SetDword(SmsKey, "AuditLogEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SmsKey, "AuditLogEnabled")],
                DetectOps = [RegOp.CheckDword(SmsKey, "AuditLogEnabled", 1)],
            },
            new TweakDef
            {
                Id = "ssmig-set-rebuild-priority-high",
                Label = "Storage Spaces Migration: Set Pool Rebuild IO Priority to High",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets RebuildPriority=2 in StorageSpaces policy (2 = High). Sets the IO priority for Storage Spaces pool rebuild operations (resync after drive replacement) to High. By default, rebuild runs at Low priority to minimise impact on running workloads — but on a pool that has lost a drive, remaining data is exposed until rebuild completes. A two-drive failure during a slow Low-priority rebuild can cause data loss. Setting rebuild to High completes resync faster at the cost of higher IO contention, reducing the window of double-failure vulnerability.",
                Tags = ["storage-spaces", "rebuild", "resync", "priority", "resilience"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Pool resync after drive failure runs at high IO priority. Workload IO may experience higher latency during rebuild. Rebuild completes significantly faster, reducing the window of single-drive exposure. Consider scheduling high-priority rebuild during off-hours in production environments.",
                ApplyOps = [RegOp.SetDword(SsKey, "RebuildPriority", 2)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "RebuildPriority")],
                DetectOps = [RegOp.CheckDword(SsKey, "RebuildPriority", 2)],
            },
            new TweakDef
            {
                Id = "ssmig-disable-pool-repair-notification",
                Label = "Storage Spaces Migration: Disable Suppression of Pool Repair Notifications",
                Category = "Storage Spaces Migration Policy",
                Description =
                    "Sets SuppressRepairNotifications=0 in StorageSpaces policy. Ensures Action Center and Event Log notifications are generated when Storage Spaces repairs (resyncs) are running. By default, Storage Spaces emits user-visible notifications during repair operations. Some deployments suppress these notifications to avoid confusion for non-technical users. However, suppressing notifications also hides critical storage health events from IT administrators who monitor Action Center or event aggregators. This setting preserves notification delivery to ensure pool repair events are visible.",
                Tags = ["storage-spaces", "notifications", "repair", "monitoring", "health"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Storage Spaces repair and health notifications are not suppressed. IT administrators and monitoring systems receive timely notification of pool repair events. Action Center shows storage health warnings on servers where users are not present.",
                ApplyOps = [RegOp.SetDword(SsKey, "SuppressRepairNotifications", 0)],
                RemoveOps = [RegOp.DeleteValue(SsKey, "SuppressRepairNotifications")],
                DetectOps = [RegOp.CheckDword(SsKey, "SuppressRepairNotifications", 0)],
            },
        ];
}
