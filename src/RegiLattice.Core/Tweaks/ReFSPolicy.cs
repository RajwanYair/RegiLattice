// RegiLattice.Core — Tweaks/ReFSPolicy.cs
// Sprint 282: Resilient File System Group Policy (10 tweaks)
// Category: "ReFS Policy" | Slug: refs
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ReFSPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ReFS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "refs-disable-integrity-checking",
            Label = "Disable ReFS Integrity Checking",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableIntegrityChecking=1 in the ReFS policy key. Prevents ReFS "
                + "from performing continuous background data integrity scrubbing using "
                + "checksums stored alongside each file record. Integrity scrubbing "
                + "consumes additional I/O bandwidth on storage-constrained systems or "
                + "arrays that already provide checksumming at the hardware level. "
                + "Default: 0 (scrubbing enabled). Recommended: 1 only on redundant arrays.",
            Tags = ["refs", "integrity", "filesystem", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityChecking", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityChecking")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityChecking", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-integrity-streams",
            Label = "Disable ReFS Integrity Streams",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableIntegrityStreams=1 in the ReFS policy key. Turns off the "
                + "integrity stream feature that tags every file region with a checksum "
                + "entry in the volume metadata stream. Disabling integrity streams "
                + "reduces per-write metadata overhead and can improve sequential write "
                + "throughput by 10–20% on high-frequency write workloads at the cost "
                + "of silent corruption detection. Default: 0.",
            Tags = ["refs", "integrity-streams", "filesystem", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityStreams", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityStreams")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityStreams", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-auto-repair",
            Label = "Disable ReFS Automatic Repair",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets DisableAutoRepair=1 in the ReFS policy key. Prevents ReFS from "
                + "automatically correcting detected bad sectors or checksum mismatches "
                + "using parity or mirror redundancy without user intervention. On "
                + "non-redundant single-disk volumes the auto-repair feature can "
                + "silently mark corrupted data as repaired when no valid copy exists. "
                + "Default: 0. Recommended: 1 only on direct-attached single disks.",
            Tags = ["refs", "repair", "filesystem", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoRepair", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoRepair")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoRepair", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-short-name-creation",
            Label = "Disable ReFS Short Name Creation",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableShortNameCreation=1 in the ReFS policy key. Suppresses "
                + "automatic generation of 8.3 DOS-compatible short names alongside "
                + "long file names on ReFS volumes. 8.3 name creation adds measurable "
                + "overhead on directories with many files and is unnecessary for "
                + "modern Windows applications and tools that use long-name APIs. "
                + "Default: 0. Recommended: 1 on dedicated server or data volumes.",
            Tags = ["refs", "shortname", "8dot3", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableShortNameCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableShortNameCreation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableShortNameCreation", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-last-access-update",
            Label = "Disable ReFS Last-Access Timestamp Update",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableLastAccessUpdate=1 in the ReFS policy key. Disables the "
                + "last-access timestamp field update on every file read operation. "
                + "Updating the last-access time on each read generates a write "
                + "transaction to the file metadata in the global B-tree, causing "
                + "write amplification on read-heavy workloads such as media servers "
                + "and archive stores. Default: 0. Recommended: 1.",
            Tags = ["refs", "timestamp", "atime", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLastAccessUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLastAccessUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLastAccessUpdate", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-parity-logging",
            Label = "Disable ReFS Parity Write Logging",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 3,
            Description =
                "Sets DisableParityLogging=1 in the ReFS policy key. Suppresses the "
                + "write-ahead parity log that makes partial-stripe writes to parity "
                + "spaces resilient across power failures. Disabling this log improves "
                + "random-write throughput on parity storage spaces but opens a window "
                + "for parity corruption if a power loss occurs mid-stripe. "
                + "Default: 0. Not recommended unless UPS protection is confirmed.",
            Tags = ["refs", "parity", "wal", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableParityLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableParityLogging")],
            DetectOps = [RegOp.CheckDword(Key, "DisableParityLogging", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-metadata-checksum",
            Label = "Disable ReFS Metadata Checksum",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 3,
            Description =
                "Sets DisableMetadataChecksum=1 in the ReFS policy key. Prevents ReFS "
                + "from computing and verifying a checksum over each metadata B-tree "
                + "page on every access. Metadata checksumming is a primary ReFS "
                + "reliability feature; disabling it removes detection of metadata "
                + "corruption caused by hardware faults or bit-rot and is not "
                + "recommended on production data volumes. Default: 0.",
            Tags = ["refs", "metadata", "checksum", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMetadataChecksum", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMetadataChecksum")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMetadataChecksum", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-large-mft",
            Label = "Disable ReFS Large MFT Zone Reservation",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableLargeMft=1 in the ReFS policy key. Prevents ReFS from "
                + "pre-reserving a large zone in the volume B-tree for anticipated "
                + "metadata growth. Pre-reservation reduces free space visible to users "
                + "on smaller volumes; on volumes with predictable small file counts "
                + "the reservation is wasteful and can be released. "
                + "Default: 0. Recommended: 1 on volumes under 200 GB.",
            Tags = ["refs", "mft", "metadata", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLargeMft", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLargeMft")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLargeMft", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-delete-notify",
            Label = "Disable ReFS Delete Notification (TRIM)",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableDeleteNotify=1 in the ReFS policy key. Stops ReFS from "
                + "issuing TRIM or UNMAP commands to the underlying SSD or thin-"
                + "provisioned storage when files are deleted. TRIM commands can cause "
                + "high-latency stalls on some older firmware SSDs and thin-provisioned "
                + "SAN/NAS LUNs that must zero out freed blocks before re-allocation. "
                + "Default: 0. Recommended: 1 only for problematic storage hardware.",
            Tags = ["refs", "trim", "unmap", "ssd", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeleteNotify", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeleteNotify")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeleteNotify", 1)],
        },
        new TweakDef
        {
            Id = "refs-disable-compression",
            Label = "Disable ReFS Data Compression",
            Category = "ReFS Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableCompression=1 in the ReFS policy key. Prevents the ReFS "
                + "driver from enabling LZ4-based file compression on volumes where "
                + "compression has been set as the default state. Compression on "
                + "already-compressed media files (video, archives, encrypted files) "
                + "yields negative savings and wastes CPU cycles attempting "
                + "incompressible blocks. Default: 0. Recommended: 1 on media volumes.",
            Tags = ["refs", "compression", "lz4", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
        },
    ];
}
