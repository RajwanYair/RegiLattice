// RegiLattice.Core — Tweaks/StorageReplicaPolicy.cs
// Storage Replica Policy — Sprint 561.
// Configures Group Policy for Windows Storage Replica: replication mode
// (sync/async), log volume sizing, bandwidth throttling, failover
// controls, and disaster recovery configuration.
// Category: "Storage Replica Policy" | Slug: srep
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\StorageReplica

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class StorageReplicaPolicy
{
    private const string SrKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageReplica";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "srep-set-replication-mode-async",
                Label = "Storage Replica: Set Default Replication Mode to Asynchronous",
                Category = "Storage Replica Policy",
                Description =
                    "Sets ReplicationMode=1 in StorageReplica policy (1 = Asynchronous). Sets the default Storage Replica replication mode to asynchronous. In asynchronous mode, the source volume acknowledges writes to the application without waiting for the destination to confirm it has received and written the data — the replication lags behind the source (RPO > 0). Synchronous mode (the default for same-site replica pairs) forces writes to complete on both source and destination before acknowledgment — zero RPO but application write latency is increased by the round-trip to the destination. For WAN-linked DR sites, synchronous replication is impractical; asynchronous mode is required to avoid write latency spikes.",
                Tags = ["storage-replica", "async", "replication-mode", "rpo", "dr"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Storage Replica uses asynchronous replication by default. The destination replica may lag behind the source by seconds to minutes depending on network latency and bandwidth. In a failover, the most recent data on the destination is used — any writes not yet replicated are lost.",
                ApplyOps = [RegOp.SetDword(SrKey, "ReplicationMode", 1)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "ReplicationMode")],
                DetectOps = [RegOp.CheckDword(SrKey, "ReplicationMode", 1)],
            },
            new TweakDef
            {
                Id = "srep-set-log-volume-size-8gb",
                Label = "Storage Replica: Set Minimum Log Volume Size to 8 GB",
                Category = "Storage Replica Policy",
                Description =
                    "Sets MinLogSize=8192 in StorageReplica policy (MB). Sets the minimum log volume size for new Storage Replica partnerships to 8 GB. The SR log volume holds a circular write buffer that tracks changes that have been committed on the source but not yet replicated to the destination. If the log volume is too small, it can overflow during bursts of write activity — forcing Storage Replica to perform a full resync of the entire replicated volume. For volumes with heavy write workloads (SQL Server transaction logs, VMs with active guest IO), 8 GB provides headroom during network outages of several hours.",
                Tags = ["storage-replica", "log", "circular-buffer", "resync", "sizing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Storage Replica log volumes are at least 8 GB. Prevents accidental log overflow and forced full resync. Log volumes are dedicated (no user data) and must be formatted as NTFS or ReFS.",
                ApplyOps = [RegOp.SetDword(SrKey, "MinLogSize", 8192)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "MinLogSize")],
                DetectOps = [RegOp.CheckDword(SrKey, "MinLogSize", 8192)],
            },
            new TweakDef
            {
                Id = "srep-set-bandwidth-limit-100mbps",
                Label = "Storage Replica: Set Replication Bandwidth Limit to 100 Mbps",
                Category = "Storage Replica Policy",
                Description =
                    "Sets BandwidthLimit=100 in StorageReplica policy (Mbps). Limits Storage Replica replication bandwidth to 100 Mbps. Without a bandwidth limit, Storage Replica can saturate available network links — particularly during initial sync of large volumes (1 TB+) which can overwhelm a 1 Gbps uplink if allowed to run unconstrained. 100 Mbps allows a 1 TB volume to be initially synced in approximately 22 hours while leaving 900 Mbps of uplink capacity for other traffic. This limit applies to both initial sync and ongoing delta replication.",
                Tags = ["storage-replica", "bandwidth", "throttle", "network", "wan"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Replication traffic is capped at 100 Mbps. Initial sync of large volumes is slower but the production network is protected. Adjust based on network capacity and RPO requirements — a smaller limit increases replication lag.",
                ApplyOps = [RegOp.SetDword(SrKey, "BandwidthLimit", 100)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "BandwidthLimit")],
                DetectOps = [RegOp.CheckDword(SrKey, "BandwidthLimit", 100)],
            },
            new TweakDef
            {
                Id = "srep-enable-consistent-replica-read",
                Label = "Storage Replica: Enable Read Access on Destination Replica",
                Category = "Storage Replica Policy",
                Description =
                    "Sets AllowReplicaRead=1 in StorageReplica policy. Enables read-only access to the destination volume in a Storage Replica pair. By default, the destination volume is mounted offline (no read access) to ensure consistency — the SR log is continuously writing to it. With AllowReplicaRead enabled, SR temporarily snapshots the destination volume to provide a read-only mount point that users and applications can query — useful for offloading backup operations, reporting queries, or compliance snapshots to the replica without impacting the source production volume.",
                Tags = ["storage-replica", "read-replica", "backup", "reporting", "snapshot"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Destination replica is accessible as a read-only snapshot. Backup and reporting workloads can be offloaded to the replica. The read-only snapshot is a point-in-time copy; it captures the state at the time of the snapshot creation.",
                ApplyOps = [RegOp.SetDword(SrKey, "AllowReplicaRead", 1)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "AllowReplicaRead")],
                DetectOps = [RegOp.CheckDword(SrKey, "AllowReplicaRead", 1)],
            },
            new TweakDef
            {
                Id = "srep-enable-encrypted-replication",
                Label = "Storage Replica: Enable Encrypted Replication Traffic",
                Category = "Storage Replica Policy",
                Description =
                    "Sets EncryptionEnabled=1 in StorageReplica policy. Enables SMB3 encryption for all Storage Replica replication traffic between source and destination servers. Replication channels carry live production data (potentially including sensitive PII, financial records, health information) traversing internal networks or WAN links. Without encryption, a packet capture on any network segment in the replication path reveals the data content. AES-256-GCM encryption wraps all replication traffic, ensuring the data payload is unreadable to network observers.",
                Tags = ["storage-replica", "encryption", "smb3", "aes", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All replication traffic is AES-256-GCM encrypted. CPU overhead for encryption is approximately 5-15% additional CPU usage on high-throughput replicas. Both source and destination servers must support SMB3 encryption. Negligible impact when servers have AES-NI hardware acceleration.",
                ApplyOps = [RegOp.SetDword(SrKey, "EncryptionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "EncryptionEnabled")],
                DetectOps = [RegOp.CheckDword(SrKey, "EncryptionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "srep-set-io-buffer-size-32mb",
                Label = "Storage Replica: Set Replication IO Buffer Size to 32 MB",
                Category = "Storage Replica Policy",
                Description =
                    "Sets IOBufferSize=32 in StorageReplica policy (MB). Sets the IO write buffer size used by Storage Replica for batching writes to the replication log. Larger IO buffers reduce the number of individual write operations to the SR log disk — improving throughput at the cost of increased memory usage. 32 MB is a practical default for most environments. Very small IO buffers cause excessive fragmented log writes, reducing sustained replication throughput to the available log disk IOps. For environments with NVMe log drives, increasing to 64 MB or more may improve throughput further.",
                Tags = ["storage-replica", "io-buffer", "performance", "throughput", "log"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SR uses 32 MB IO buffers for replication log writes. Improves log write throughput. Uses approximately 32 MB additional kernel non-paged pool memory per SR partnership. Adjust upward for NVMe log drives.",
                ApplyOps = [RegOp.SetDword(SrKey, "IOBufferSize", 32)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "IOBufferSize")],
                DetectOps = [RegOp.CheckDword(SrKey, "IOBufferSize", 32)],
            },
            new TweakDef
            {
                Id = "srep-enable-replication-health-audit",
                Label = "Storage Replica: Enable Replication Health Audit Logging",
                Category = "Storage Replica Policy",
                Description =
                    "Sets HealthAuditEnabled=1 in StorageReplica policy. Enables periodic health audit logging for Storage Replica partnerships. Health audit events record the current replication state, lag (delta between source and destination), log utilisation percentage, and any fault conditions for each SR group. Without health auditing, the current state of disaster recovery capability is only available by querying WMI — it is not proactively logged. Health audit events enable SIEM correlation to track RPO compliance: if replication lag exceeds the target RPO, an alert fires before a disaster is declared.",
                Tags = ["storage-replica", "health", "audit", "monitoring", "rpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SR writes periodic health audit events to the Windows event log. Events include replication lag, log utilisation, and fault state. Use with event forwarding or a SIEM to alert on RPO violations. Minimal disk overhead.",
                ApplyOps = [RegOp.SetDword(SrKey, "HealthAuditEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "HealthAuditEnabled")],
                DetectOps = [RegOp.CheckDword(SrKey, "HealthAuditEnabled", 1)],
            },
            new TweakDef
            {
                Id = "srep-disable-auto-failover",
                Label = "Storage Replica: Disable Automatic Failover",
                Category = "Storage Replica Policy",
                Description =
                    "Sets AutoFailover=0 in StorageReplica policy. Prevents Storage Replica from automatically promoting the destination volume when it detects that the source server is unavailable. Automatic failover can cause split-brain scenarios: if the source server is temporarily unreachable due to network issues (rather than truly offline), both source and destination may become active producers — writing data that diverges and cannot be automatically reconciled. In all DR scenarios, manual failover + human validation of data consistency is recommended before promoting the replica.",
                Tags = ["storage-replica", "failover", "split-brain", "dr", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Failover to the destination replica requires manual intervention. The destination volume is not promoted automatically if the source becomes unreachable. Human verification of failure before failover prevents split-brain data divergence. RPO and RTO must account for manual failover time.",
                ApplyOps = [RegOp.SetDword(SrKey, "AutoFailover", 0)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "AutoFailover")],
                DetectOps = [RegOp.CheckDword(SrKey, "AutoFailover", 0)],
            },
            new TweakDef
            {
                Id = "srep-enable-log-compression",
                Label = "Storage Replica: Enable Replication Log Compression",
                Category = "Storage Replica Policy",
                Description =
                    "Sets LogCompression=1 in StorageReplica policy. Enables transparent compression of Storage Replica log data before writing to the log volume. Compression reduces the amount of physically written data to the log disk, which is especially valuable when the log volume is an SSD with limited write endurance (TBW rating). For workloads with compressible data patterns (text files, compressed XML, Hyper-V VHD zero pages), log compression can reduce log write volume by 40-60%, extending SSD log drive lifetime. Decompression occurs before entries are sent to the destination.",
                Tags = ["storage-replica", "compression", "log", "ssd-endurance", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SR log data is compressed before writing. Extends SSD write endurance on log drives. CPU overhead for compression is approximately 5-10% on high-throughput replicas. Incompressible data (already-compressed files) sees minimal benefit.",
                ApplyOps = [RegOp.SetDword(SrKey, "LogCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "LogCompression")],
                DetectOps = [RegOp.CheckDword(SrKey, "LogCompression", 1)],
            },
            new TweakDef
            {
                Id = "srep-set-replication-port-5445",
                Label = "Storage Replica: Set Replication Network Port to 5445",
                Category = "Storage Replica Policy",
                Description =
                    "Sets ReplicationPort=5445 in StorageReplica policy. Configures Storage Replica to use TCP port 5445 for replication traffic. The well-known Storage Replica port (5445) must be open in firewalls between source and destination servers. By explicitly setting the port via policy (rather than relying on the default), firewall rules can be precisely targeted and audited. Port 5445 is the standard SR port — SMB-based SR partners also require port 445 for SMB3 transport; this policy governs the SR control channel. Firewall rules for SR replication should permit TCP 5445 bidirectionally between SR members.",
                Tags = ["storage-replica", "port", "firewall", "network", "configuration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "SR replication control channel uses TCP 5445. Firewall policy must permit TCP 5445 bidirectionally between SR source and destination. Changing from a custom port back to the default 5445 requires SR service restart and firewall rule update.",
                ApplyOps = [RegOp.SetDword(SrKey, "ReplicationPort", 5445)],
                RemoveOps = [RegOp.DeleteValue(SrKey, "ReplicationPort")],
                DetectOps = [RegOp.CheckDword(SrKey, "ReplicationPort", 5445)],
            },
        ];
}
