// RegiLattice.Core — Tweaks/NtfsPolicy.cs
// Sprint 305: NTFS Policy tweaks (10 tweaks)
// Category: "NTFS Policy" | Slug: ntfspol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NTFS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NtfsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NTFS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ntfspol-disable-last-access",
            Label = "Disable NTFS Last Access Time Update",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "NTFS records the last access time for every file each time it is read, requiring a metadata write operation on every file read. Disabling last access time updates eliminates the write operation triggered on every file read, significantly reducing metadata update overhead. On systems with millions of files, last access time recording causes substantial unnecessary disk I/O especially for antivirus scans and search indexers. Removing last access time recording can improve performance of read-heavy workloads by up to 15 percent on spinning disk systems. Last access time is rarely used by enterprise applications and is not required for data classification or integrity purposes. Security tools that require access time tracking should use shadow copies or file activity monitoring instead.",
            Tags = ["ntfs", "performance", "filesystem", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLastAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLastAccess")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLastAccess", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-enable-compression",
            Label = "Disable NTFS Compression on System Volume",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "NTFS transparent compression reduces file sizes on disk by compressing file contents using the LZ77 compression algorithm. Disabling NTFS compression on the system volume prevents performance-degrading CPU overhead from compression and decompression on every file access. NTFS compression causes random access pattern degradation because compressed files require sequential decompression to reach arbitrary offsets. Modern SSD storage provides sufficient capacity that compression decompression overhead is not worth the space savings. System files including the page file hivelist and drivers should never be compressed as it introduces unbounded decompression latency at critical moments. Disabling system volume compression ensures consistent and predictable I/O performance for OS operations.",
            Tags = ["ntfs", "compression", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-8dot3-names",
            Label = "Disable NTFS 8.3 Short Name Generation",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "NTFS generates short 8.3 format filenames for every file created to maintain backward compatibility with legacy 16-bit applications. Disabling 8.3 short name generation eliminates this legacy metadata overhead and reduces directory metadata size. Short name generation requires computing and storing an additional name entry for every file, increasing directory metadata write cost. Enterprise environments running exclusively 32-bit and 64-bit applications do not benefit from 8.3 name compatibility. Disabling short names can improve file creation performance on systems with very large directories containing many files. Note that some older administrative tools and scripts may depend on short names; these should be tested before deployment.",
            Tags = ["ntfs", "8dot3", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Disable8dot3NameCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "Disable8dot3NameCreation")],
            DetectOps = [RegOp.CheckDword(Key, "Disable8dot3NameCreation", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-self-healing",
            Label = "Disable NTFS Self-Healing (Force Chkdsk)",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 2,
            Description =
                "NTFS self-healing automatically repairs filesystem inconsistencies detected during normal operation without requiring offline chkdsk runs. Disabling self-healing forces filesystem errors to be addressed through the traditional offline chkdsk process requiring a system restart. Self-healing runs in the background and may silently alter filesystem metadata in ways that complicate forensic analysis. Forensic investigation scenarios require preservation of exact filesystem state including errors for evidentiary purposes. Some self-healing repairs may destroy evidence of intrusion by cleaning up attacker-modified metadata. This tweak is intended specifically for forensic workstations and incident response systems, not general enterprise use.",
            Tags = ["ntfs", "self-healing", "forensics", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSelfHealing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSelfHealing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSelfHealing", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-encryption-default",
            Label = "Prevent Default NTFS Encryption of New Files",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "NTFS Encrypted File System inheritable encryption allows folders to be marked such that all files created within them are automatically encrypted. Preventing default encryption inheritance stops new files from being automatically encrypted by inheriting parent directory encryption attributes. Automatic encryption without user awareness can prevent legitimate administrative access to files for maintenance and incident response. EFS-encrypted files that lose access to the recovery certificate become permanently inaccessible creating potential data loss. Enterprise file encryption should be managed through BitLocker for volume encryption rather than per-file EFS inheritance. Disabling default EFS inheritance prevents accidental file lockout while preserving the ability to use EFS intentionally.",
            Tags = ["ntfs", "encryption", "efs", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEncryptionDefault", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEncryptionDefault")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEncryptionDefault", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-delete-notify",
            Label = "Disable NTFS Delete Notify to SSD Controller",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "NTFS sends TRIM commands to SSD controllers when files are deleted, allowing the controller to proactively reclaim and erase NAND flash cells. Disabling delete notify (TRIM) prevents Windows from sending TRIM commands to connected storage controllers. TRIM disabled mode can be useful when using storage over certain RAID configurations or shared storage that does not benefit from per-host TRIM. Some NVMe configurations perform background garbage collection more efficiently without host-generated TRIM hints. TRIM should only be disabled when the storage subsystem design is known to perform better without it. Most modern consumer and enterprise SSDs benefit from TRIM and disabling it degrades long-term write performance.",
            Tags = ["ntfs", "trim", "ssd", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeleteNotify", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeleteNotify")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeleteNotify", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-alternate-data-streams-block",
            Label = "Block Alternate Data Stream Creation by Untrusted Code",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "NTFS Alternate Data Streams allow additional data to be attached to files under hidden named streams that are invisible to most file browsers. Blocking ADS creation by untrusted code prevents malware and unauthorized applications from hiding data in invisible file streams. Alternate data streams are used by some malware to store payloads, configuration, or exfiltrated data in streams invisible to directory listings. The Zone.Identifier ADS created by browsers is a legitimate security feature that marks downloaded files and should be preserved. Blocking indiscriminate ADS creation from untrusted sources limits the use of this NTFS feature as a steganographic storage mechanism. Security tools that rely on ADS for file metadata tags should be evaluated and explicitly exempted.",
            Tags = ["ntfs", "ads", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockAlternateDataStreamsByUntrusted", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockAlternateDataStreamsByUntrusted")],
            DetectOps = [RegOp.CheckDword(Key, "BlockAlternateDataStreamsByUntrusted", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-tunnel-cache",
            Label = "Disable NTFS Filename Tunnel Cache",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The NTFS filename tunnel cache briefly preserves the identity of a short filename when a file is deleted and recreated with the same name. Disabling the tunnel cache prevents NTFS from re-associating the previous short filename when a file with the same long name is recreated. The tunnel cache can cause unexpected short filename collisions when temporary file creation and deletion cycles are frequent. Some security tools may observe abnormal file identity continuity through the tunnel cache which can complicate forensic timeline analysis. Disabling the tunnel cache ensures newly created files receive fresh filename allocations without inheriting deleted file identities. This setting has no impact on visible filename behavior for long filenames.",
            Tags = ["ntfs", "tunnel-cache", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaximumTunnelEntryAgeInSeconds", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaximumTunnelEntryAgeInSeconds")],
            DetectOps = [RegOp.CheckDword(Key, "MaximumTunnelEntryAgeInSeconds", 0)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-quota-tracking",
            Label = "Disable NTFS Disk Quota Tracking",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "NTFS disk quotas track per-user disk space usage on volumes to enforce storage limits for individual accounts. Disabling disk quota tracking removes the overhead of per-user storage accounting metadata updates on every file creation and deletion. Quota tracking requires metadata updates on every file write scaled to the number of users with active quotas. Enterprise storage management through NAS, DFS, or cloud storage is more scalable and flexible than per-volume NTFS quotas. Removing quota tracking reduces file creation overhead and simplifies storage management administration. This setting is appropriate for volumes where storage management is handled through external storage systems rather than NTFS quotas.",
            Tags = ["ntfs", "quotas", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableQuotaTracking", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableQuotaTracking")],
            DetectOps = [RegOp.CheckDword(Key, "DisableQuotaTracking", 1)],
        },
        new TweakDef
        {
            Id = "ntfspol-disable-opportunistic-locks",
            Label = "Disable NTFS Opportunistic Locking",
            Category = "NTFS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 2,
            Description =
                "NTFS opportunistic locking allows clients to cache file data locally when no other client has the file open, improving performance by reducing file server round trips. Disabling opportunistic locking forces all file reads and writes to go to the server without local caching for network-shared NTFS volumes. Oplock conflicts during concurrent access can cause temporary file lock contention delays for applications sharing files across multiple clients. Some legacy applications do not handle oplock break negotiations correctly, causing hangs or corruption when files are accessed concurrently. Disabling oplocks is a workaround for specific legacy application compatibility issues rather than a general recommendation. This setting should only be applied where known oplock compatibility problems exist and should not be applied broadly.",
            Tags = ["ntfs", "oplocks", "compatibility", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOpportunisticLocking", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOpportunisticLocking")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOpportunisticLocking", 1)],
        },
    ];
}
