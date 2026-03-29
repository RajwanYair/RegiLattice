// RegiLattice.Core — Tweaks/DfsrPolicy.cs
// DFSR Replication Policy — Sprint 559.
// Configures Group Policy for Distributed File System Replication (DFS-R):
// bandwidth scheduling, staging area quotas, conflict resolution limits,
// diagnostic logging, and fault tolerance settings.
// Category: "DFSR Replication Policy" | Slug: dfsr
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DFSR

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DfsrPolicy
{
    private const string DfsrKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DFSR";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "dfsr-set-bandwidth-throttle-256kbps",
                Label = "DFSR Policy: Set Default Replication Bandwidth Throttle to 256 Kbps",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets BandwidthThrottle=256 in DFSR policy. Sets the default maximum DFS-R replication bandwidth to 256 Kbps per connection. Without bandwidth throttling, DFS-R can saturate WAN links during initial replication or large change storms, causing VoIP, RDP, and other latency-sensitive traffic to degrade. 256 Kbps is a conservative baseline for branch-office replication over typical enterprise MPLS links. DFS-R honors scheduled replication windows defined per-group in DFSR configuration; this policy sets the background rate as a safety cap.",
                Tags = ["dfsr", "replication", "bandwidth", "wan", "throttle"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DFS-R replication is capped at 256 Kbps per connection. Replication of large change sets takes longer. WAN bandwidth for other services is protected. Can be overridden per-connection group in DFSR replication group configuration.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "BandwidthThrottle", 256)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "BandwidthThrottle")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "BandwidthThrottle", 256)],
            },
            new TweakDef
            {
                Id = "dfsr-set-staging-cleanup-quota-512mb",
                Label = "DFSR Policy: Set Staging Area Cleanup Quota to 512 MB",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets StagingCleanupQuota=524288 in DFSR policy (512 MB in KB). Sets the staging area maximum size before DFS-R initiates cleanup of staged files. The staging area holds files being replicated in transit between source and destination. If the staging area fills completely, DFS-R stalls replication. The default staging area quota is often too small for environments with large Office files or CAD data. Conversely, a staging area that grows without bound can fill the volume. 512 MB provides a reasonable buffer before cleanup is triggered.",
                Tags = ["dfsr", "staging", "quota", "disk-space", "replication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DFS-R staging area cleanup triggers at 512 MB. Replication of files larger than the staging quota in a single transaction may need alternative replica placement. Prevents uncontrolled disk growth on DFS-R member servers.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "StagingCleanupQuota", 524288)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "StagingCleanupQuota")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "StagingCleanupQuota", 524288)],
            },
            new TweakDef
            {
                Id = "dfsr-enable-debug-logging",
                Label = "DFSR Policy: Enable DFS-R Debug Logging",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets DebugLogEnabled=1 in DFSR policy. Enables the DFS-R service's internal diagnostic log. The debug log records detailed DFS-R events including file change notifications, connection establishment, bandwidth negotiation, and conflict detection. Without debug logging, diagnosing DFS-R replication failures (missing files, stale replicas, split-brain situations) requires attaching debuggers or live tracing. Debug logs are stored in %SystemRoot%\\debug\\dfsr*.log and are rolled with configurable maximum file size. Essential for DFS-R health monitoring and incident investigation.",
                Tags = ["dfsr", "debug", "logging", "diagnostics", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DFS-R writes diagnostic logs to %SystemRoot%\\debug\\dfsr*.log. Minimal disk overhead (logs are rotated). Provides crucial data for replication issue diagnosis. Logs may contain file names and paths from replicated content.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "DebugLogEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "DebugLogEnabled")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "DebugLogEnabled", 1)],
            },
            new TweakDef
            {
                Id = "dfsr-set-max-conflict-files-1000",
                Label = "DFSR Policy: Set Maximum Conflict Files to 1,000",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets MaxConflictFiles=1000 in DFSR policy. Sets the maximum number of conflict files DFS-R can retain in the DfsrPrivate\\ConflictAndDeleted folder on each member. When two users edit the same file simultaneously on different DFS-R members, DFS-R keeps one version as the primary and moves the losing version to the ConflictAndDeleted folder for review. If too many conflicts accumulate without cleanup, DFS-R stops creating new conflict copies and silently discards them. 1,000 is a balanced limit; with the default file size cap, this represents a manageable administrator review queue.",
                Tags = ["dfsr", "conflict", "resolution", "files", "consistency"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Up to 1,000 conflict files are retained per DFS-R member. Conflict files older than the retention period are purged. Administrators should monitor ConflictAndDeleted folders for important file conflicts that users may need to resolve.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "MaxConflictFiles", 1000)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "MaxConflictFiles")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "MaxConflictFiles", 1000)],
            },
            new TweakDef
            {
                Id = "dfsr-enable-rdc-compression",
                Label = "DFSR Policy: Enable Remote Differential Compression",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets RdcEnabled=1 in DFSR policy. Enables Remote Differential Compression (RDC) for DFS-R replication traffic. RDC analyses replicated files and transfers only the changed byte ranges (blocks) rather than the complete file. For large documents where only a small portion changes (e.g., a one-line change in a 10 MB Excel file), RDC can reduce replication traffic by 90%+. Without RDC, every small change triggers a full file retransfer. RDC is especially valuable over low-bandwidth WAN links between branch offices.",
                Tags = ["dfsr", "rdc", "compression", "bandwidth", "differential"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only changed byte ranges of files are transferred during replication. Significantly reduces WAN bandwidth for DFS-R at the cost of slightly higher CPU usage on both source and destination during block comparison. Net positive for WAN-connected sites.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "RdcEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "RdcEnabled")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "RdcEnabled", 1)],
            },
            new TweakDef
            {
                Id = "dfsr-disable-auto-recovery",
                Label = "DFSR Policy: Disable Automatic DFS-R Error Recovery",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets AutoRecovery=0 in DFSR policy. Prevents DFS-R from automatically performing database recovery operations when it detects that the jet database has become inconsistent. Automatic recovery can cause DFS-R to re-replicate files from scratch (initial sync) which creates heavy WAN traffic spikes and can take hours to complete for large libraries. In managed environments, DFS-R database issues should be investigated and resolved by IT; automatic silent recovery can mask underlying storage or filesytem issues that need attention. Manual recovery is done via DFSRDIAG or deleting the DFSR database.",
                Tags = ["dfsr", "recovery", "database", "stability", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "DFS-R does not auto-recover from database inconsistency. A DFS-R database corruption causes replication to stop until IT intervenes. Alert monitoring for DFS-R event log errors is required. Prevents surprise WAN traffic from unplanned initial syncs.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "AutoRecovery", 0)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "AutoRecovery")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "AutoRecovery", 0)],
            },
            new TweakDef
            {
                Id = "dfsr-set-poll-interval-60min",
                Label = "DFSR Policy: Set DFSR Group Configuration Poll Interval to 60 Minutes",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets ConfigurationPollIntervalInMin=60 in DFSR policy. Sets the interval at which DFS-R members poll Active Directory for changes to their replication group configuration (member list, connection topology, bandwidth schedule). The default poll interval is 60 minutes but can be reduced in experimental or testing environments and forgotten in production. Frequent polling increases AD query load and can cause DFS-R to temporarily reset active connections during configuration refresh. 60 minutes is the recommended production interval — configuration changes are applied within the hour.",
                Tags = ["dfsr", "polling", "ad", "configuration", "interval"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "DFS-R polls AD for configuration changes every 60 minutes. Configuration changes (add member, update bandwidth schedule) take effect at the next poll. No impact on ongoing replication for existing connections.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "ConfigurationPollIntervalInMin", 60)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "ConfigurationPollIntervalInMin")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "ConfigurationPollIntervalInMin", 60)],
            },
            new TweakDef
            {
                Id = "dfsr-enable-stop-replication-on-low-disk",
                Label = "DFSR Policy: Stop Replication When Disk Is Under 1% Free",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets StopReplicationOnAutoRecovery=1 in DFSR policy. Configures DFS-R to stop incoming replication when the volume hosting the replicated folder falls below 1% free disk space. Without this protection, DFS-R will continue replicating files even as the disk fills to 100%, potentially causing the volume to fill completely — stopping all writes including system services, other applications, and file shares. A 1% threshold gives the DFS-R service enough runway to detect the condition and alert before the disk is completely full.",
                Tags = ["dfsr", "disk-space", "resilience", "fault-tolerance", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Incoming DFS-R replication stops if disk free space drops below 1%. Prevents disk-full conditions caused by DFS-R. Replication lag increases until disk space is recovered. Event log entries are written when replication is suspended.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "StopReplicationOnAutoRecovery", 1)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "StopReplicationOnAutoRecovery")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "StopReplicationOnAutoRecovery", 1)],
            },
            new TweakDef
            {
                Id = "dfsr-set-min-staging-age-3days",
                Label = "DFSR Policy: Set Minimum Staging File Age to 3 Days Before Cleanup",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets MinStagingAge=3 in DFSR policy. Sets the minimum number of days a staging file must remain in the staging area before it is eligible for deletion during staging area cleanup. Staging files are needed if replication fails and needs to be retried. If staging files are cleaned up too aggressively (before all members have acknowledged receipt), DFS-R must re-prepare the staged file from scratch on the next retry. 3 days provides a sufficient window for transient network outages (weekends, planned maintenance) to resolve without losing staging work.",
                Tags = ["dfsr", "staging", "cleanup", "retention", "replication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Staged files are retained for at least 3 days before cleanup eligibility. Staging area may hold more data than strictly minimum required. Reduces the need to re-stage large files after transient network outages.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "MinStagingAge", 3)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "MinStagingAge")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "MinStagingAge", 3)],
            },
            new TweakDef
            {
                Id = "dfsr-enable-preseed-support",
                Label = "DFSR Policy: Enable DFS-R Pre-seed Mode Support",
                Category = "DFSR Replication Policy",
                Description =
                    "Sets PreseedingEnabled=1 in DFSR policy. Enables DFS-R to use pre-existing content on a new member server as the seed for initial replication rather than transferring all data from scratch across the WAN. When adding a new branch-office DFS-R member, the typical initial sync requires transferring the entire replicated folder (potentially hundreds of gigabytes) over the WAN. Pre-seeding works by physically copying the data to the new member (via external drive or data centre transfer), then DFS-R detects the existing files and only replicates differences. Reduces initial sync WAN traffic by 99%+.",
                Tags = ["dfsr", "preseed", "initial-sync", "bandwidth", "branch"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DFS-R uses pre-existing content on new members to seed initial replication. First-time member addition to large replication groups requires physical data pre-seeding setup. No impact on ongoing replication of already-seeded members.",
                ApplyOps = [RegOp.SetDword(DfsrKey, "PreseedingEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DfsrKey, "PreseedingEnabled")],
                DetectOps = [RegOp.CheckDword(DfsrKey, "PreseedingEnabled", 1)],
            },
        ];
}
