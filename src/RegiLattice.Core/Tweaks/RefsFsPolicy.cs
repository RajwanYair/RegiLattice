// RegiLattice.Core — Tweaks/RefsFsPolicy.cs
// Resilient File System (ReFS) volume, integrity, and cluster size policy controls — Sprint 488.
// Category: "ReFS File System Policy" | Slug: refspol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\ReFS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RefsFsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "refspol-disable-integrity-streams",
                Label = "Disable ReFS Integrity Streams (Checksums)",
                Category = "ReFS File System Policy",
                Description =
                    "Disables data integrity streams (SHA-256 checksums per block) on ReFS volumes, removing per-I/O checksum overhead at the cost of eliminating the ability to detect silent data corruption.",
                Tags = ["refs", "integrity", "checksum", "file-system", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "ReFS integrity streams disabled; slight I/O improvement but silent data corruption no longer detected.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityStreams", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityStreams")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityStreams", 1)],
            },
            new TweakDef
            {
                Id = "refspol-disable-scrubbing",
                Label = "Disable ReFS Background Data Scrubbing",
                Category = "ReFS File System Policy",
                Description =
                    "Disables the background data scrubbing job that periodically reads and validates all ReFS blocks against their stored checksums, eliminating the I/O overhead but preventing proactive corruption detection.",
                Tags = ["refs", "scrubbing", "background", "file-system", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "ReFS scrubbing disabled; background read-and-verify jobs eliminated, reducing idle I/O.",
                ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundScrubbing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundScrubbing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundScrubbing", 1)],
            },
            new TweakDef
            {
                Id = "refspol-enable-salvage-mode",
                Label = "Enable ReFS Corruption Salvage Mode",
                Category = "ReFS File System Policy",
                Description =
                    "Enables ReFS salvage mode which continues to mount and access uncorrupted portions of a volume when corruption is detected, avoiding complete volume unavailability due to isolated data corruption.",
                Tags = ["refs", "salvage", "corruption", "availability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ReFS salvage mode enabled; partially corrupted volumes remain accessible for uncorrupted data.",
                ApplyOps = [RegOp.SetDword(Key, "EnableSalvageMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSalvageMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSalvageMode", 1)],
            },
            new TweakDef
            {
                Id = "refspol-set-cluster-size-64k",
                Label = "Set Default ReFS Cluster Size to 64 KB",
                Category = "ReFS File System Policy",
                Description =
                    "Sets the default ReFS cluster size to 64 KB, improving large-sequential-I/O throughput and reducing metadata overhead for workloads that store many large files (virtual machines, databases, backups).",
                Tags = ["refs", "cluster-size", "performance", "file-system", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ReFS default cluster size set to 64 KB; applies to newly formatted ReFS volumes.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultClusterSizeKB", 64)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultClusterSizeKB")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultClusterSizeKB", 64)],
            },
            new TweakDef
            {
                Id = "refspol-block-refs-caching-metadata",
                Label = "Block ReFS Metadata in System File Cache",
                Category = "ReFS File System Policy",
                Description =
                    "Prevents ReFS metadata (B-tree nodes, directory structures) from consuming the system file cache, dedicating file cache to application data and preventing metadata cache pressure on systems with large ReFS trees.",
                Tags = ["refs", "metadata", "file-cache", "memory", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ReFS metadata excluded from system file cache; cache fully available for file data.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRefsMetadataCaching", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsMetadataCaching")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRefsMetadataCaching", 1)],
            },
            new TweakDef
            {
                Id = "refspol-disable-refs-on-boot-volume",
                Label = "Prevent ReFS Formatting of System Boot Volumes",
                Category = "ReFS File System Policy",
                Description =
                    "Blocks ReFS from being selected as the file system for the system or boot volume during installation, ensuring Windows boot volumes always use NTFS which has full boot-time driver support.",
                Tags = ["refs", "boot-volume", "ntfs", "formatting", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ReFS blocked on boot volumes; system drive must use NTFS.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRefsOnSystemVolume", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsOnSystemVolume")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRefsOnSystemVolume", 1)],
            },
            new TweakDef
            {
                Id = "refspol-enable-corruption-audit-log",
                Label = "Enable ReFS Corruption Detection Audit Logging",
                Category = "ReFS File System Policy",
                Description =
                    "Enables detailed event log entries for every ReFS corruption detection event including the file path, cluster address, and recovery action taken.",
                Tags = ["refs", "corruption", "audit-log", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ReFS corruption events logged; file path, cluster, and recovery details recorded in System event log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableCorruptionEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCorruptionEventLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCorruptionEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "refspol-disable-refs-dedup",
                Label = "Disable ReFS Block-Level Deduplication",
                Category = "ReFS File System Policy",
                Description =
                    "Disables block-level deduplication on ReFS volumes, stopping background dedup processing that can interfere with real-time workloads and consume I/O bandwidth on storage-intensive systems.",
                Tags = ["refs", "deduplication", "storage", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ReFS block deduplication disabled; no dedup overhead, at the expense of higher storage usage.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRefsBlockDedup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsBlockDedup")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRefsBlockDedup", 1)],
            },
            new TweakDef
            {
                Id = "refspol-set-mirror-write-threshold-3",
                Label = "Set ReFS Mirror-Write Log Threshold to 3 Entries",
                Category = "ReFS File System Policy",
                Description =
                    "Sets the ReFS B+ tree write-log threshold that triggers a checkpoint flush to 3 entries, ensuring faster persistence of write logs at the cost of slightly more frequent I/O checkpoints.",
                Tags = ["refs", "write-log", "checkpoint", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ReFS write-log checkpoint threshold set to 3; write persistence more frequent on active volumes.",
                ApplyOps = [RegOp.SetDword(Key, "WriteLogCheckpointThreshold", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "WriteLogCheckpointThreshold")],
                DetectOps = [RegOp.CheckDword(Key, "WriteLogCheckpointThreshold", 3)],
            },
            new TweakDef
            {
                Id = "refspol-disable-refs-compression",
                Label = "Disable ReFS Transparent Compression",
                Category = "ReFS File System Policy",
                Description =
                    "Disables ReFS transparent compression, preventing the file system from automatically compressing cold data blocks, and eliminating the CPU overhead of compression/decompression on access.",
                Tags = ["refs", "compression", "cpu", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "ReFS compression disabled; slightly higher storage usage, lower CPU overhead for large file reads.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRefsCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRefsCompression")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRefsCompression", 1)],
            },
        ];
}
