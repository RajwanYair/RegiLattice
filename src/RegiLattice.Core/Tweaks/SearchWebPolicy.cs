namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SearchWebPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "srchweb-disable-cloud-search",
            Label = "Disable Cloud-Augmented Search Results",
            Category = "Search Web Policy",
            Description =
                "Prevents Windows Search from combining local results with cloud (Bing/MSN index) results. Search returns only locally-indexed content.",
            Tags = ["search", "cloud", "bing", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes Bing cloud augmentation from Windows Search; fully local results only.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-cortana-policy",
            Label = "Disable Cortana AI Assistant (Machine Policy)",
            Category = "Search Web Policy",
            Description = "Machine-wide Group Policy to disable Cortana integration in Windows Search. Overrides per-user and per-app settings.",
            Tags = ["cortana", "search", "ai", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Machine-wide policy disable for Cortana AI integration.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCortana")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-cortana-above-lock",
            Label = "Disable Cortana on Lock Screen",
            Category = "Search Web Policy",
            Description =
                "Prevents Cortana from responding to voice or text queries when the device is locked. Blocks unauthenticated access to Cortana assistant features.",
            Tags = ["cortana", "lock-screen", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents unauthenticated access to Cortana from locked state.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowCortanaAboveLock", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCortanaAboveLock")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCortanaAboveLock", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-web-results",
            Label = "Disable Web Results in Windows Search",
            Category = "Search Web Policy",
            Description =
                "Prevents Windows Search from including Bing web results alongside local search results. Search is limited to locally-indexed files and apps.",
            Tags = ["search", "web", "bing", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes all Bing web results from Windows Search; local results only.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-web-over-metered",
            Label = "Disable Web Search over Metered Connections",
            Category = "Search Web Policy",
            Description =
                "Blocks web-augmented search results when the device is connected via a metered network. Reduces unexpected data usage during web search on limited plans.",
            Tags = ["search", "metered", "data", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks Bing web search over metered connections.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchUseWebOverMeteredConnections", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchUseWebOverMeteredConnections")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchUseWebOverMeteredConnections", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-search-location",
            Label = "Disable Location Access in Windows Search",
            Category = "Search Web Policy",
            Description =
                "Prevents Windows Search from using the device's current location to improve local search results. Removes a location-data disclosure path.",
            Tags = ["search", "location", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents location data from being used to augment search results.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowSearchToUseLocation", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSearchToUseLocation")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-bing-in-search",
            Label = "Disable Bing Web Search in Start/Taskbar",
            Category = "Search Web Policy",
            Description =
                "Completely disables Bing web search from appearing in the Windows Start menu and taskbar search. All search results come from the local index only.",
            Tags = ["search", "bing", "start", "taskbar", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Completely removes Bing from Start/taskbar search.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWebSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWebSearch")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "srchweb-enforce-safe-search",
            Label = "Enforce Strict SafeSearch for Web Results",
            Category = "Search Web Policy",
            Description =
                "Forces Bing SafeSearch to Strict mode for all web search results delivered through Windows Search. Value 2 = Strict. Recommended for managed shared devices.",
            Tags = ["search", "safesearch", "policy", "content-filter"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Forces Bing SafeSearch to Strict mode for shared/managed devices.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SafeSearchMode", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "SafeSearchMode")],
            DetectOps = [RegOp.CheckDword(Key, "SafeSearchMode", 2)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-dynamic-content-wsb",
            Label = "Disable Dynamic Content in Windows Search Box",
            Category = "Search Web Policy",
            Description =
                "Prevents Windows Search Box from showing dynamic highlights, trending topics, and promotional content. Keeps the search bar focused on user-typed queries.",
            Tags = ["search", "search-box", "highlights", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes trending topics and promotional content from the search box.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "srchweb-disable-indexing-encrypted",
            Label = "Disable Indexing of Encrypted Files",
            Category = "Search Web Policy",
            Description =
                "Prevents Windows Search from indexing encrypted files and stores. Reduces the risk that sensitive encrypted content is extractable via the search index.",
            Tags = ["search", "encryption", "indexing", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents encrypted file contents from appearing in the search index.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowIndexingEncryptedStoresOrItems", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowIndexingEncryptedStoresOrItems")],
            DetectOps = [RegOp.CheckDword(Key, "AllowIndexingEncryptedStoresOrItems", 0)],
        },
    ];
}
