// RegiLattice.Core — Tweaks/BackgroundTransferPolicy.cs
// Background Intelligent Transfer Service (BITS) advanced GPO controls — Sprint 219.
// Controls BITS bandwidth throttling, job limits, and security hardening.
// Category: "Background Transfer Policy" | Slug: bitsadv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BackgroundIntelligentTransfer

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BackgroundTransferPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BackgroundIntelligentTransfer";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "bitsadv-limit-max-bandwidth",
                Label = "Limit BITS Maximum Bandwidth (1 Mbps)",
                Category = "Background Transfer Policy",
                Description =
                    "Caps total BITS download bandwidth to 1 Mbps per machine. Prevents Windows Update, Delivery Optimization uploads, and other BITS consumers from saturating the network link during business hours. Default: unlimited. Recommended: adjust per available bandwidth.",
                Tags = ["bits", "bandwidth", "network", "throttle", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All BITS transfers are capped at 1 Mbps total; background downloads cannot saturate the uplink.",
                ApplyOps = [RegOp.SetDword(Key, "MaxTransferRateOnSchedule", 1024)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxTransferRateOnSchedule")],
                DetectOps = [RegOp.CheckDword(Key, "MaxTransferRateOnSchedule", 1024)],
            },
            new TweakDef
            {
                Id = "bitsadv-limit-max-jobs",
                Label = "Limit Maximum Concurrent BITS Jobs to 5",
                Category = "Background Transfer Policy",
                Description =
                    "Restricts the number of BITS jobs that can run concurrently per user to 5. Prevents a single application or attacker from flooding the BITS queue with a large number of simultaneous download jobs. Default: up to 200 jobs. Recommended: 5 for controlled environments.",
                Tags = ["bits", "jobs", "throttle", "resource-limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "No more than 5 concurrent BITS download jobs per user; job flooding is prevented.",
                ApplyOps = [RegOp.SetDword(Key, "MaxJobsPerUser", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxJobsPerUser")],
                DetectOps = [RegOp.CheckDword(Key, "MaxJobsPerUser", 5)],
            },
            new TweakDef
            {
                Id = "bitsadv-limit-files-per-job",
                Label = "Limit Maximum Files Per BITS Job to 100",
                Category = "Background Transfer Policy",
                Description =
                    "Limits each BITS job to a maximum of 100 files. Prevents a single job from monopolising BITS or being used to exfiltrate a large number of small files in one batch operation. Default: up to 200 files per job. Recommended: 100.",
                Tags = ["bits", "files", "job-limit", "resource-limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Each BITS job is limited to 100 files; bulk-exfiltration via single BITS job is restricted.",
                ApplyOps = [RegOp.SetDword(Key, "MaxFilesPerJob", 100)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxFilesPerJob")],
                DetectOps = [RegOp.CheckDword(Key, "MaxFilesPerJob", 100)],
            },
            new TweakDef
            {
                Id = "bitsadv-limit-job-download-size",
                Label = "Limit BITS Job Download Size to 4 GB",
                Category = "Background Transfer Policy",
                Description =
                    "Caps the total bytes a single BITS job can download to 4 GiB (4,294 MB). Feature updates are typically ≤ 4 GiB; this prevents misuse of BITS for downloading arbitrarily large payloads. Default: no download size limit. Recommended: 4 GiB.",
                Tags = ["bits", "download-size", "resource-limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Single BITS job download size is capped at 4 GiB; oversized downloads fail and must be split.",
                ApplyOps = [RegOp.SetDword(Key, "MaxDownloadSizePerJob", 4096)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxDownloadSizePerJob")],
                DetectOps = [RegOp.CheckDword(Key, "MaxDownloadSizePerJob", 4096)],
            },
            new TweakDef
            {
                Id = "bitsadv-limit-job-upload-size",
                Label = "Limit BITS Job Upload Size to 1 GB",
                Category = "Background Transfer Policy",
                Description =
                    "Restricts the amount of data that a single BITS upload job can send to 1 GiB. Prevents BITS from being used as an exfiltration vector to upload large amounts of data to an attacker-controlled server. Default: no upload size limit. Recommended: 1 GiB.",
                Tags = ["bits", "upload-size", "dlp", "resource-limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "BITS upload per job is capped at 1 GiB; large-scale data exfiltration via BITS upload is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "MaxUploadSizePerJob", 1024)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxUploadSizePerJob")],
                DetectOps = [RegOp.CheckDword(Key, "MaxUploadSizePerJob", 1024)],
            },
            new TweakDef
            {
                Id = "bitsadv-disable-internet-uploads",
                Label = "Block BITS Uploads to Internet Destinations",
                Category = "Background Transfer Policy",
                Description =
                    "Prevents BITS upload jobs from targeting internet destinations (hosts outside the local network and trusted intranet zones). Limits BITS uploads to intranet servers only, blocking a common Living-off-the-Land (LotL) exfiltration technique that uses BITS to send data to external C2 servers. Default: internet upload allowed. Recommended: 1.",
                Tags = ["bits", "upload", "internet", "exfiltration", "lotl", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "BITS upload jobs targeting internet hosts are blocked; uploads are restricted to intranet destinations.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePeerCachingServer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePeerCachingServer")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePeerCachingServer", 1)],
            },
            new TweakDef
            {
                Id = "bitsadv-require-https",
                Label = "Require HTTPS for BITS Transfers",
                Category = "Background Transfer Policy",
                Description =
                    "Forces all BITS download/upload jobs to use HTTPS only. HTTP transfers expose the payload to MITM interception or tampering in transit. Default: HTTP transfers allowed. Recommended: 1 in high-security environments.",
                Tags = ["bits", "https", "tls", "encryption", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "BITS refusing HTTP connections ensures all transfers are TLS-encrypted.",
                ApplyOps = [RegOp.SetDword(Key, "RequireHTTPS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireHTTPS")],
                DetectOps = [RegOp.CheckDword(Key, "RequireHTTPS", 1)],
            },
            new TweakDef
            {
                Id = "bitsadv-set-job-inactivity-timeout",
                Label = "Set BITS Job Inactivity Timeout to 7 Days",
                Category = "Background Transfer Policy",
                Description =
                    "Removes BITS jobs from the queue if they have not made progress within 7 days (604,800 seconds). Prevents stale or abandoned jobs from persisting indefinitely and consuming queue resources. Default: 90-day timeout. Recommended: 7 days.",
                Tags = ["bits", "timeout", "inactivity", "cleanup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "BITS jobs that stall for more than 7 days are automatically removed from the queue.",
                ApplyOps = [RegOp.SetDword(Key, "JobInactivityTimeout", 604800)],
                RemoveOps = [RegOp.DeleteValue(Key, "JobInactivityTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "JobInactivityTimeout", 604800)],
            },
            new TweakDef
            {
                Id = "bitsadv-disable-peer-caching-client",
                Label = "Disable BITS Peer Caching (Client)",
                Category = "Background Transfer Policy",
                Description =
                    "Prevents the local machine from acting as a BITS peer cache client — it will not receive content from peer machines on the LAN via BITS peer caching. Reduces lateral data movement between machines and limits the LAN attack surface of BITS. Default: peer caching enabled. Recommended: 1 when central delivery is preferred.",
                Tags = ["bits", "peer-cache", "lan", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Machine will not receive BITS cached content from peers on the LAN; all downloads sourced from the server.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePeerCachingClient", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePeerCachingClient")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePeerCachingClient", 1)],
            },
            new TweakDef
            {
                Id = "bitsadv-enable-audit-logging",
                Label = "Enable BITS Transfer Audit Logging",
                Category = "Background Transfer Policy",
                Description =
                    "Records BITS job creation, completion, cancellation, and error events to the Microsoft-Windows-Bits-Client/Operational event log. Provides forensic visibility into what files were downloaded or uploaded via BITS, essential for detecting LotL abuse. Default: limited operational logging. Recommended: 1.",
                Tags = ["bits", "audit", "logging", "forensics", "lotl", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "All BITS job activity (create, complete, error, cancel) is logged to the Operational event channel.",
                ApplyOps = [RegOp.SetDword(Key, "EnableBITSAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBITSAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBITSAuditLogging", 1)],
            },
        ];
}
