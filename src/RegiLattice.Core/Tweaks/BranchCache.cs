// RegiLattice.Core — Tweaks/BranchCache.cs
// BranchCache enterprise peer-content caching policies (Sprint 129, T8.2).
// Slug "bc" — HKLM PeerDist/Service & PeerDist/ContentFetch & PeerDist/Retrieval.
// Distinct from NetworkOptimization.cs (TCP-level) and WindowsUpdateAdvanced.cs (Delivery Optimization).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BranchCache
{
    private const string Svc = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Service";
    private const string Fetch = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\ContentFetch";
    private const string Retrieval = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Retrieval";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "bc-enable-distributed-mode",
            Label = "Enable BranchCache in Distributed Cache Mode",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "network", "caching", "distributed", "performance"],
            Description =
                "Enables BranchCache and configures it in Distributed Cache mode. Clients that have "
                + "downloaded content from the main-office content server cache files locally and serve "
                + "them to other clients on the same subnet. Reduces WAN bandwidth usage. Requires Win10+.",
            ApplyOps = [RegOp.SetDword(Svc, "Enable", 1), RegOp.SetDword(Svc, "PeerDistributionMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Svc, "Enable"), RegOp.DeleteValue(Svc, "PeerDistributionMode")],
            DetectOps = [RegOp.CheckDword(Svc, "PeerDistributionMode", 1)],
        },
        new TweakDef
        {
            Id = "bc-enable-hosted-mode",
            Label = "Enable BranchCache in Hosted Cache Mode",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "network", "caching", "hosted", "server"],
            Description =
                "Enables BranchCache and configures it in Hosted Cache mode. Content is cached on a "
                + "dedicated server at the branch office; clients upload content to the server and peers "
                + "download from it. Requires a Windows Server running the BranchCache hosted cache role.",
            ApplyOps = [RegOp.SetDword(Svc, "Enable", 1), RegOp.SetDword(Svc, "PeerDistributionMode", 2)],
            RemoveOps = [RegOp.DeleteValue(Svc, "Enable"), RegOp.DeleteValue(Svc, "PeerDistributionMode")],
            DetectOps = [RegOp.CheckDword(Svc, "PeerDistributionMode", 2)],
        },
        new TweakDef
        {
            Id = "bc-set-cache-25pct",
            Label = "Set BranchCache Cache to 25% of Disk",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "cache", "disk", "size", "performance"],
            Description =
                "Sets the BranchCache client cache to occupy 25% of the total disk capacity. A larger "
                + "cache improves hit rates at the cost of disk space. Windows default is 5%. Applies to "
                + "the partition where Windows is installed.",
            ApplyOps = [RegOp.SetDword(Svc, "MaxCacheSizeAsPercentageOfDiskSpace", 25)],
            RemoveOps = [RegOp.DeleteValue(Svc, "MaxCacheSizeAsPercentageOfDiskSpace")],
            DetectOps = [RegOp.CheckDword(Svc, "MaxCacheSizeAsPercentageOfDiskSpace", 25)],
        },
        new TweakDef
        {
            Id = "bc-cap-cache-5gb",
            Label = "Cap BranchCache Absolute Cache at 5 GB",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "cache", "disk", "quota", "limit"],
            Description =
                "Sets an absolute upper limit of 5 GB on the BranchCache client cache regardless of disk "
                + "percentage. Prevents the cache from consuming excessive space on large drives. Works in "
                + "conjunction with the percentage setting — whichever is smaller wins.",
            ApplyOps = [RegOp.SetDword(Svc, "MaxCacheSizeInGB", 5)],
            RemoveOps = [RegOp.DeleteValue(Svc, "MaxCacheSizeInGB")],
            DetectOps = [RegOp.CheckDword(Svc, "MaxCacheSizeInGB", 5)],
        },
        new TweakDef
        {
            Id = "bc-use-sha256-hashes",
            Label = "Use SHA-256 for BranchCache Content Hashes",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "security", "sha256", "hash", "integrity"],
            Description =
                "Configures BranchCache to use SHA-256 (hash version 2) for content verification instead "
                + "of the default SHA-1. Provides stronger content integrity guarantees. The content server "
                + "and ALL clients must support the same hash version to inter-operate.",
            ApplyOps = [RegOp.SetDword(Svc, "HashVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(Svc, "HashVersion")],
            DetectOps = [RegOp.CheckDword(Svc, "HashVersion", 2)],
        },
        new TweakDef
        {
            Id = "bc-enable-firewall-exceptions",
            Label = "Enable BranchCache Automatic Firewall Exceptions",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "firewall", "network", "ports", "automation"],
            Description =
                "Automatically configures Windows Firewall to allow BranchCache traffic on standard ports "
                + "(TCP 80 for HTTP mode, TCP/UDP 3702 and TCP 443 for WS-Discovery). Eliminates manual "
                + "firewall rule creation at branch offices.",
            ApplyOps = [RegOp.SetDword(Svc, "FirewallPortSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Svc, "FirewallPortSettings")],
            DetectOps = [RegOp.CheckDword(Svc, "FirewallPortSettings", 1)],
        },
        new TweakDef
        {
            Id = "bc-set-retrieval-latency-5s",
            Label = "Set BranchCache Retrieval Segment TTL to 5 Seconds",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "latency", "performance", "timeout", "retrieval"],
            Description =
                "Limits BranchCache content retrieval segment time-to-live (TTL) to 5 seconds before "
                + "falling back to the origin content server. Lower values reduce wait time when cache peers "
                + "are slow or unresponsive. Default: 0 (no cap). Applies to distributed mode only.",
            ApplyOps = [RegOp.SetDword(Retrieval, "SegmentTTL", 5)],
            RemoveOps = [RegOp.DeleteValue(Retrieval, "SegmentTTL")],
            DetectOps = [RegOp.CheckDword(Retrieval, "SegmentTTL", 5)],
        },
        new TweakDef
        {
            Id = "bc-enable-hash-publication-smb",
            Label = "Enable BranchCache Hash Publication for SMB Shares",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "smb", "hash", "publication", "server"],
            Description =
                "Enables content hash publication for SMB file shares, making content pre-hashed and "
                + "immediately available for BranchCache clients without generating hashes on-demand. "
                + "Best applied to file servers at the main office. Sets HashPublicationForPeerDist=1 "
                + "and HashSupportVersion=2 for SHA-256.",
            ApplyOps = [RegOp.SetDword(Svc, "HashPublicationForPeerDist", 1), RegOp.SetDword(Svc, "HashSupportVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(Svc, "HashPublicationForPeerDist"), RegOp.DeleteValue(Svc, "HashSupportVersion")],
            DetectOps = [RegOp.CheckDword(Svc, "HashPublicationForPeerDist", 1)],
        },
        new TweakDef
        {
            Id = "bc-prefer-hosted-cache-server",
            Label = "Prefer BranchCache Hosted Cache over Auto-Discovery",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "hosted", "discovery", "server", "preference"],
            Description =
                "Configures clients to prefer the designated hosted cache server over automatic network "
                + "discovery. Reduces broadcast traffic and ensures clients use the authoritative branch "
                + "cache. Should be used together with the hosted cache mode setting.",
            ApplyOps = [RegOp.SetDword(Svc, "UseHostedCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Svc, "UseHostedCache")],
            DetectOps = [RegOp.CheckDword(Svc, "UseHostedCache", 1)],
        },
        new TweakDef
        {
            Id = "bc-zero-initial-offering-delay",
            Label = "Eliminate BranchCache Initial Peer Offering Delay",
            Category = "BranchCache",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["branchcache", "caching", "initial", "delay", "optimization"],
            Description =
                "Sets the initial peer offering delay to 0 seconds so BranchCache clients immediately "
                + "offer locally cached segments to peers without any wait. Maximises peer-to-peer "
                + "utilisation within the branch and minimises WAN round-trips for segments already "
                + "present on local machines.",
            ApplyOps = [RegOp.SetDword(Fetch, "InitialOfferDelayInSeconds", 0)],
            RemoveOps = [RegOp.DeleteValue(Fetch, "InitialOfferDelayInSeconds")],
            DetectOps = [RegOp.CheckDword(Fetch, "InitialOfferDelayInSeconds", 0)],
        },
    ];
}
