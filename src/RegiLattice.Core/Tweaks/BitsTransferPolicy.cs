// RegiLattice.Core — Tweaks/BitsTransferPolicy.cs
// Category: "BITS Transfer Policy" — Slug "bitspol"
// HKLM\SOFTWARE\Policies\Microsoft\Windows\BITS
// Background Intelligent Transfer Service (BITS) policy controls —
// limits job counts, file counts, bandwidth usage, and download time.

#nullable enable

using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class BitsTransferPolicy
{
    private const string BitsPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BITS";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "bitspol-job-inactivity-30d",
            Label = "Reduce BITS Job Inactivity Timeout to 30 Days",
            Category = "BITS Transfer Policy",
            Description =
                "Sets JobInactivityTimeout=2592000 (30 days in seconds) in BITS policy. The default allows stale BITS jobs to persist for 90 days. Reducing to 30 days reclaims disk space from incomplete download caches sooner and prevents long-running abandoned transfer jobs.",
            Tags = ["bits", "background", "transfer", "policy", "cleanup"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "JobInactivityTimeout", 2592000)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "JobInactivityTimeout")],
            DetectOps = [RegOp.CheckDword(BitsPol, "JobInactivityTimeout", 2592000)],
        },
        new TweakDef
        {
            Id = "bitspol-max-jobs-machine",
            Label = "Limit BITS Jobs to 50 per Machine",
            Category = "BITS Transfer Policy",
            Description =
                "Sets MaxJobsPerMachine=50 in BITS policy. Default is 300 concurrent jobs per computer. Limiting to 50 prevents BITS storms where many applications queue simultaneous background downloads, competing for network and I/O resources.",
            Tags = ["bits", "background", "transfer", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "MaxJobsPerMachine", 50)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxJobsPerMachine")],
            DetectOps = [RegOp.CheckDword(BitsPol, "MaxJobsPerMachine", 50)],
        },
        new TweakDef
        {
            Id = "bitspol-max-jobs-user",
            Label = "Limit BITS Jobs to 20 per User",
            Category = "BITS Transfer Policy",
            Description =
                "Sets MaxJobsPerUser=20 in BITS policy. Default is 60 concurrent jobs per user. Capping at 20 ensures no single user account can saturate BITS with background transfers, which is especially relevant for multi-user terminal server environments.",
            Tags = ["bits", "background", "transfer", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "MaxJobsPerUser", 20)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxJobsPerUser")],
            DetectOps = [RegOp.CheckDword(BitsPol, "MaxJobsPerUser", 20)],
        },
        new TweakDef
        {
            Id = "bitspol-max-files-per-job",
            Label = "Limit BITS Job to 100 Files",
            Category = "BITS Transfer Policy",
            Description =
                "Sets MaxJobFilesPerJob=100 in BITS policy. Default is 200 files per BITS job. Reducing to 100 limits the blast radius of a misbehaving application that creates overly large BITS jobs and helps ensure all jobs can complete without exhausting I/O queue depth.",
            Tags = ["bits", "background", "transfer", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "MaxJobFilesPerJob", 100)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxJobFilesPerJob")],
            DetectOps = [RegOp.CheckDword(BitsPol, "MaxJobFilesPerJob", 100)],
        },
        new TweakDef
        {
            Id = "bitspol-max-ranges-per-file",
            Label = "Limit BITS to 100 Byte Ranges per File",
            Category = "BITS Transfer Policy",
            Description =
                "Sets MaxRangesPerFile=100 in BITS policy. Default is 500 byte ranges per file. Each range costs memory in the BITS service process. Limiting ranges reduces BITS memory overhead on machines with many concurrent background downloads from multi-part servers.",
            Tags = ["bits", "background", "transfer", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "MaxRangesPerFile", 100)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxRangesPerFile")],
            DetectOps = [RegOp.CheckDword(BitsPol, "MaxRangesPerFile", 100)],
        },
        new TweakDef
        {
            Id = "bitspol-max-download-time-24h",
            Label = "Limit BITS Download Jobs to 24 Hours",
            Category = "BITS Transfer Policy",
            Description =
                "Sets MaxDownloadTime=86400 (24 hours in seconds) in BITS policy. By default BITS has no wall-clock limit on download jobs. Setting a 24-hour maximum prevents stalled or hung BITS jobs from occupying an active transfer slot indefinitely.",
            Tags = ["bits", "background", "transfer", "policy", "timeout"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "MaxDownloadTime", 86400)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxDownloadTime")],
            DetectOps = [RegOp.CheckDword(BitsPol, "MaxDownloadTime", 86400)],
        },
        new TweakDef
        {
            Id = "bitspol-internet-bandwidth-limit",
            Label = "Cap BITS Internet Bandwidth at 8 Mbps",
            Category = "BITS Transfer Policy",
            Description =
                "Sets MaxInternetBandwidth=8192 (Kbps) in BITS policy. Prevents BITS background downloads from monopolising internet bandwidth. 8 Mbps is sufficient for most Windows Update payloads while leaving headroom for interactive network traffic.",
            Tags = ["bits", "background", "transfer", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "MaxInternetBandwidth", 8192)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "MaxInternetBandwidth")],
            DetectOps = [RegOp.CheckDword(BitsPol, "MaxInternetBandwidth", 8192)],
        },
        new TweakDef
        {
            Id = "bitspol-enable-bandwidth-throttle",
            Label = "Enable BITS Bandwidth Throttling",
            Category = "BITS Transfer Policy",
            Description =
                "Sets EnableBITSMaxBandwidth=1 in BITS policy. Activates the BITS bandwidth throttle schedule, causing BITS to honour the MaxInternetBandwidth setting during the configured hours. Without this flag the bandwidth cap defined by the schedule has no effect.",
            Tags = ["bits", "background", "transfer", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "EnableBITSMaxBandwidth", 1)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "EnableBITSMaxBandwidth")],
            DetectOps = [RegOp.CheckDword(BitsPol, "EnableBITSMaxBandwidth", 1)],
        },
        new TweakDef
        {
            Id = "bitspol-disable-peercaching-client",
            Label = "Disable BITS Peer Caching (Client)",
            Category = "BITS Transfer Policy",
            Description =
                "Sets DisablePeerCachingClient=1 in BITS policy. Prevents this machine from downloading BITS content from peer computers on the LAN. Disabling peer-client ensures all BITS traffic goes through the legitimate server rather than potentially compromised peers.",
            Tags = ["bits", "peercache", "network", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "DisablePeerCachingClient", 1)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "DisablePeerCachingClient")],
            DetectOps = [RegOp.CheckDword(BitsPol, "DisablePeerCachingClient", 1)],
        },
        new TweakDef
        {
            Id = "bitspol-disable-peercaching-server",
            Label = "Disable BITS Peer Caching (Server)",
            Category = "BITS Transfer Policy",
            Description =
                "Sets DisablePeerCachingServer=1 in BITS policy. Prevents this machine from serving cached BITS content to other peers on the LAN. Disabling peer-server mode stops the machine from becoming an unintended content distribution node that consumes upload bandwidth.",
            Tags = ["bits", "peercache", "network", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(BitsPol, "DisablePeerCachingServer", 1)],
            RemoveOps = [RegOp.DeleteValue(BitsPol, "DisablePeerCachingServer")],
            DetectOps = [RegOp.CheckDword(BitsPol, "DisablePeerCachingServer", 1)],
        },
    ];
}
