// RegiLattice.Core — Tweaks/WindowsSearchIndexingAdvancedPolicy.cs
// Windows Search indexing, remote queries, and content crawl policy — Sprint 632.
// Category: "Windows Search Indexing Advanced Policy" | Slug: wsidx
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsSearchIndexingAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "wsidx-prevent-remote-queries",
            Label        = "Prevent Remote Search Queries via Windows Search",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Blocks remote clients from querying the local Windows Search index over the network. Default: allowed. Recommended: disabled for workstations.",
            Tags         = ["search", "indexing", "remote", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Prevents network-based search queries against local index; local search unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "PreventRemoteQueries", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "PreventRemoteQueries")],
            DetectOps    = [RegOp.CheckDword(Key, "PreventRemoteQueries", 1)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-index-encrypted-stores",
            Label        = "Disable Indexing of Encrypted Files",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents Windows Search from indexing EFS-encrypted files. Reduces index attack surface when encrypted content is present. Default: allowed. Recommended: disable on sensitive systems.",
            Tags         = ["search", "indexing", "encryption", "efs", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Encrypted files excluded from search index; those files won't appear in search results.",
            ApplyOps     = [RegOp.SetDword(Key, "AllowIndexingEncryptedStoresOrItems", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AllowIndexingEncryptedStoresOrItems")],
            DetectOps    = [RegOp.CheckDword(Key, "AllowIndexingEncryptedStoresOrItems", 0)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-location-in-search",
            Label        = "Disable Location Usage in Search Results",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents Windows Search from using device location to refine or personalise search results. Default: allowed. Recommended: disable for privacy.",
            Tags         = ["search", "location", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Location data not used in search ranking; no impact on local file search.",
            ApplyOps     = [RegOp.SetDword(Key, "AllowSearchToUseLocation", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AllowSearchToUseLocation")],
            DetectOps    = [RegOp.CheckDword(Key, "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-removable-media-index",
            Label        = "Disable Indexing of Removable Drives",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents the Windows Search indexer from crawling USB drives, SD cards, and other removable media. Reduces I/O on external devices. Default: allowed.",
            Tags         = ["search", "indexing", "removable-media", "usb", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Removable drives excluded from search index; files still accessible via direct browse.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableRemovableDriveIndexing", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableRemovableDriveIndexing")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableRemovableDriveIndexing", 1)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-bing-web-search",
            Label        = "Disable Bing Web Search in Windows Search",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents Windows Search from sending queries to Bing for web results. Only local results are returned. Default: web search enabled.",
            Tags         = ["search", "bing", "web", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "No web results in Start menu search; all queries stay local.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableWebSearch", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableWebSearch")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableWebSearch", 1)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-connected-search",
            Label        = "Disable Connected Search Suggestions",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Disables connected search suggestions that send partial keystrokes to Microsoft as the user types in the search box. Default: enabled.",
            Tags         = ["search", "suggestions", "privacy", "telemetry", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Search suggestions from the cloud are disabled; local suggestions still work.",
            ApplyOps     = [RegOp.SetDword(Key, "ConnectedSearchUseWeb", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "ConnectedSearchUseWeb")],
            DetectOps    = [RegOp.CheckDword(Key, "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-safe-search",
            Label        = "Set Search SafeSearch to Strict via Policy",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Enforces SafeSearch strict mode for web results in Windows Search. Applies via Group Policy. Default: moderate.",
            Tags         = ["search", "safe-search", "content-filter", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "SafeSearch forced to strict; only affects web result filtering.",
            ApplyOps     = [RegOp.SetDword(Key, "ConnectedSearchSafeSearch", 3)],
            RemoveOps    = [RegOp.DeleteValue(Key, "ConnectedSearchSafeSearch")],
            DetectOps    = [RegOp.CheckDword(Key, "ConnectedSearchSafeSearch", 3)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-search-on-metered",
            Label        = "Disable Cloud Search on Metered Connections",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents Windows Search from sending cloud queries when on a metered network connection. Saves bandwidth and reduces data charges. Default: allowed.",
            Tags         = ["search", "metered", "bandwidth", "network", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Cloud search queries blocked on metered networks; local search unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "ConnectedSearchUseWebOverMeteredConnections", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "ConnectedSearchUseWebOverMeteredConnections")],
            DetectOps    = [RegOp.CheckDword(Key, "ConnectedSearchUseWebOverMeteredConnections", 0)],
        },
        new TweakDef
        {
            Id           = "wsidx-disable-index-backoff",
            Label        = "Disable Indexer Backoff on Battery Power",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents the Windows Search indexer from reducing speed when on battery power. Useful for always-on desktops misidentified as battery-powered. Default: backoff enabled.",
            Tags         = ["search", "indexing", "battery", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 4,
            ImpactNote   = "Indexer runs at full speed on battery; may increase power consumption on laptops.",
            ApplyOps     = [RegOp.SetDword(Key, "PreventIndexOnBattery", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "PreventIndexOnBattery")],
            DetectOps    = [RegOp.CheckDword(Key, "PreventIndexOnBattery", 0)],
        },
        new TweakDef
        {
            Id           = "wsidx-force-directory-indexing",
            Label        = "Allow Indexing Even When Low Disk Space",
            Category     = "Windows Search Indexing Advanced Policy",
            Description  = "Prevents the indexer from stopping when disk space falls below the default threshold. Ensures search continues on nearly-full drives. Default: indexer pauses on low space.",
            Tags         = ["search", "indexing", "disk-space", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 3,
            ImpactNote   = "Indexer keeps running on low-disk systems; could consume remaining space for index data.",
            ApplyOps     = [RegOp.SetDword(Key, "PreventIndexingLowDiskSpaceMB", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "PreventIndexingLowDiskSpaceMB")],
            DetectOps    = [RegOp.CheckDword(Key, "PreventIndexingLowDiskSpaceMB", 0)],
        },
    ];
}
