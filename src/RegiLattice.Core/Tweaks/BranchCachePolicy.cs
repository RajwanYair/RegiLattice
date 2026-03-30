// RegiLattice.Core — Tweaks/BranchCachePolicy.cs
// BranchCache distributed content caching policy — Sprint 633.
// Category: "BranchCache Policy" | Slug: branchcache
// Registry: HKLM\SOFTWARE\Policies\Microsoft\PeerDist\Service

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BranchCachePolicy
{
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Service";
    private const string SvcCfg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Service\Configuration";
    private const string HostedKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\HostedCache\Connection";
    private const string HashKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PeerDist\Retrieval";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "branchcache-enable-service",
            Label        = "Enable BranchCache Service via Policy",
            Category     = "BranchCache Policy",
            Description  = "Enables the BranchCache distributed caching service via Group Policy. BranchCache caches WAN content locally for faster repeat access. Default: not configured.",
            Tags         = ["branchcache", "caching", "wan", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 4,
            ImpactNote   = "Enables local content caching; reduces WAN bandwidth for repeated downloads.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "Enable", 1)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "Enable")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "Enable", 1)],
        },
        new TweakDef
        {
            Id           = "branchcache-distributed-mode",
            Label        = "Set BranchCache to Distributed Mode",
            Category     = "BranchCache Policy",
            Description  = "Configures BranchCache to operate in distributed (peer-to-peer) mode where clients share cached content with each other. Default: not configured.",
            Tags         = ["branchcache", "distributed", "p2p", "caching", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Clients share cached content directly; reduces server and WAN load.",
            ApplyOps     = [RegOp.SetDword(SvcCfg, "PreferredContentInformationVersion", 2)],
            RemoveOps    = [RegOp.DeleteValue(SvcCfg, "PreferredContentInformationVersion")],
            DetectOps    = [RegOp.CheckDword(SvcCfg, "PreferredContentInformationVersion", 2)],
        },
        new TweakDef
        {
            Id           = "branchcache-set-cache-percent",
            Label        = "Set BranchCache Disk Cache to 10 Percent",
            Category     = "BranchCache Policy",
            Description  = "Limits the BranchCache disk cache size to 10% of the data drive. Prevents runaway cache growth. Default: 5%.",
            Tags         = ["branchcache", "cache-size", "disk", "storage", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Cache limited to 10% of disk; balances caching benefit with storage use.",
            ApplyOps     = [RegOp.SetDword(SvcCfg, "SizePercent", 10)],
            RemoveOps    = [RegOp.DeleteValue(SvcCfg, "SizePercent")],
            DetectOps    = [RegOp.CheckDword(SvcCfg, "SizePercent", 10)],
        },
        new TweakDef
        {
            Id           = "branchcache-set-cache-age",
            Label        = "Set BranchCache Maximum Content Age to 28 Days",
            Category     = "BranchCache Policy",
            Description  = "Sets cached content expiry to 28 days. Content older than this is evicted from the local cache. Default: 28 days (696 hours).",
            Tags         = ["branchcache", "cache-age", "expiry", "retention", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Stale cached content evicted after 28 days; may cause re-download of old files.",
            ApplyOps     = [RegOp.SetDword(SvcCfg, "MaxCacheAge", 672)],
            RemoveOps    = [RegOp.DeleteValue(SvcCfg, "MaxCacheAge")],
            DetectOps    = [RegOp.CheckDword(SvcCfg, "MaxCacheAge", 672)],
        },
        new TweakDef
        {
            Id           = "branchcache-enable-content-discovery",
            Label        = "Enable Automatic Hosted Cache Discovery",
            Category     = "BranchCache Policy",
            Description  = "Enables automatic Service Connection Point (SCP) discovery for hosted cache servers. Clients auto-locate the nearest cache server. Default: disabled.",
            Tags         = ["branchcache", "hosted-cache", "discovery", "scp", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Clients discover hosted cache servers via AD; useful in multi-site deployments.",
            ApplyOps     = [RegOp.SetDword(HostedKey, "AutomaticHostedCacheDiscovery", 1)],
            RemoveOps    = [RegOp.DeleteValue(HostedKey, "AutomaticHostedCacheDiscovery")],
            DetectOps    = [RegOp.CheckDword(HostedKey, "AutomaticHostedCacheDiscovery", 1)],
        },
        new TweakDef
        {
            Id           = "branchcache-enable-latency-detection",
            Label        = "Enable Network Latency Caching Threshold",
            Category     = "BranchCache Policy",
            Description  = "Enables the BranchCache latency threshold — content is cached only when WAN round-trip time exceeds the configured threshold. Default: disabled (cache all).",
            Tags         = ["branchcache", "latency", "wan", "threshold", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Only caches content from slow WAN links; LAN-fetched content bypasses cache.",
            ApplyOps     = [RegOp.SetDword(SvcCfg, "EnableLatencyBasedCaching", 1)],
            RemoveOps    = [RegOp.DeleteValue(SvcCfg, "EnableLatencyBasedCaching")],
            DetectOps    = [RegOp.CheckDword(SvcCfg, "EnableLatencyBasedCaching", 1)],
        },
        new TweakDef
        {
            Id           = "branchcache-set-latency-threshold",
            Label        = "Set BranchCache Latency Threshold to 80ms",
            Category     = "BranchCache Policy",
            Description  = "Sets the round-trip latency threshold at 80ms. WAN content served above this latency is cached; below this it is fetched live. Default: 80ms.",
            Tags         = ["branchcache", "latency", "threshold", "wan", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Caching only triggered for links slower than 80ms round-trip; LAN unaffected.",
            ApplyOps     = [RegOp.SetDword(SvcCfg, "LatencyThreshold", 80)],
            RemoveOps    = [RegOp.DeleteValue(SvcCfg, "LatencyThreshold")],
            DetectOps    = [RegOp.CheckDword(SvcCfg, "LatencyThreshold", 80)],
        },
        new TweakDef
        {
            Id           = "branchcache-enable-http-hash",
            Label        = "Enable HTTP Content Hash Generation",
            Category     = "BranchCache Policy",
            Description  = "Enables hash generation for HTTP-based content served through BranchCache. Required for web-server content offloading. Default: disabled.",
            Tags         = ["branchcache", "http", "hash", "web", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Enables content verification for HTTP-cached files; slight CPU overhead on server.",
            ApplyOps     = [RegOp.SetDword(HashKey, "EnableHTTPHash", 1)],
            RemoveOps    = [RegOp.DeleteValue(HashKey, "EnableHTTPHash")],
            DetectOps    = [RegOp.CheckDword(HashKey, "EnableHTTPHash", 1)],
        },
        new TweakDef
        {
            Id           = "branchcache-enable-smb-hash",
            Label        = "Enable SMB Content Hash Generation",
            Category     = "BranchCache Policy",
            Description  = "Enables hash generation for SMB/CIFS file shares. Required for file-server content caching via BranchCache. Default: disabled.",
            Tags         = ["branchcache", "smb", "file-share", "hash", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "File-share content verified and cached locally; minor CPU overhead on file server.",
            ApplyOps     = [RegOp.SetDword(HashKey, "EnableSMBHash", 1)],
            RemoveOps    = [RegOp.DeleteValue(HashKey, "EnableSMBHash")],
            DetectOps    = [RegOp.CheckDword(HashKey, "EnableSMBHash", 1)],
        },
        new TweakDef
        {
            Id           = "branchcache-enable-bits-hash",
            Label        = "Enable BITS Content Hash for BranchCache",
            Category     = "BranchCache Policy",
            Description  = "Enables hash publication for Background Intelligent Transfer Service (BITS) downloads. WSUS and ConfigMgr content benefits from BranchCache. Default: disabled.",
            Tags         = ["branchcache", "bits", "wsus", "sccm", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Windows Update and ConfigMgr content cached locally via BranchCache.",
            ApplyOps     = [RegOp.SetDword(HashKey, "EnableBITSHash", 1)],
            RemoveOps    = [RegOp.DeleteValue(HashKey, "EnableBITSHash")],
            DetectOps    = [RegOp.CheckDword(HashKey, "EnableBITSHash", 1)],
        },
    ];
}
