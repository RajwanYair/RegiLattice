// RegiLattice.Core — Tweaks/WindowsSearchAdv.cs
// Advanced Windows Search indexer, search results, and Cortana separation (Sprint 83).
// Slug "search" — search feature flags distinct from cortana (Cortana.cs) and
// IndexingSearch.cs (which covers index locations and file type exclusions).
// These are search UX policy settings and behaviour flags.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsSearchAdv
{
    private const string SearchPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    private const string SearchUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search";

    private const string SearchInternal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search";

    private const string SearchResults = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "search-disable-web-results",
            Label = "Disable Web Search Results in Start/Search",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["search", "web", "start menu", "privacy", "bing"],
            Description =
                "Disables the web search results that appear in the Start menu and "
                + "Windows Search alongside local results. Prevents queries from being "
                + "sent to Bing/Microsoft and speeds up local search results.",
            ApplyOps = [RegOp.SetDword(SearchUser, "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.SetDword(SearchUser, "BingSearchEnabled", 1)],
            DetectOps = [RegOp.CheckDword(SearchUser, "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "search-disable-web-results-policy",
            Label = "Disable Web Results in Search via Policy",
            Category = "Windows Search Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["search", "web", "policy", "bing", "privacy"],
            Description =
                "Enforces disabling of internet search results in Windows Search via "
                + "Group Policy. Persists across user profile changes and applies to all "
                + "users on the system.",
            ApplyOps = [RegOp.SetDword(SearchPolicy, "DisableWebSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(SearchPolicy, "DisableWebSearch")],
            DetectOps = [RegOp.CheckDword(SearchPolicy, "DisableWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "search-disable-search-highlights",
            Label = "Disable Search Highlights (Search Box Spotlight)",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["search", "highlights", "spotlight", "taskbar"],
            Description =
                "Disables Windows Search Highlights (the animated search box and daily "
                + "spotlight content in the taskbar search). Reduces visual noise and "
                + "removes Microsoft content promotions from the taskbar.",
            ApplyOps = [RegOp.SetDword(SearchUser, "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(SearchUser, "SearchboxTaskbarMode", 1)],
            DetectOps = [RegOp.CheckDword(SearchUser, "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "search-disable-safe-search",
            Label = "Disable SafeSearch Filter in Windows Search",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["search", "safesearch", "web", "bing"],
            Description =
                "Disables the SafeSearch filter (adult content filtering) from Windows "
                + "Search web results. Value 0 = Off. Requires web search to be enabled. "
                + "Only affects Windows Search Bing integration.",
            ApplyOps = [RegOp.SetDword(SearchUser, "SafeSearchMode", 0)],
            RemoveOps = [RegOp.SetDword(SearchUser, "SafeSearchMode", 1)],
            DetectOps = [RegOp.CheckDword(SearchUser, "SafeSearchMode", 0)],
        },
        new TweakDef
        {
            Id = "search-disable-find-my-files",
            Label = "Disable Enhanced 'Find My Files' Deep Search",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["search", "find my files", "enhanced", "indexing"],
            Description =
                "Disables the 'Find My Files' enhanced indexing mode that deeply indexes "
                + "all files including non-indexed locations. Reduces background disk I/O "
                + "from extensive indexing sweeps.",
            ApplyOps = [RegOp.SetDword(SearchUser, "DeviceHistoryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(SearchUser, "DeviceHistoryEnabled")],
            DetectOps = [RegOp.CheckDword(SearchUser, "DeviceHistoryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "search-disable-recent-activities-search",
            Label = "Disable Recent Activities in Search Results",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["search", "recent", "activity", "privacy"],
            Description =
                "Disables recently opened files and apps from appearing in Windows Search "
                + "results. Prevents search from surfacing your recent activity to other "
                + "users on shared machines.",
            ApplyOps = [RegOp.SetDword(SearchUser, "History", 0)],
            RemoveOps = [RegOp.SetDword(SearchUser, "History", 1)],
            DetectOps = [RegOp.CheckDword(SearchUser, "History", 0)],
        },
        new TweakDef
        {
            Id = "search-disable-search-taskbar-icon",
            Label = "Hide Search Icon from Taskbar",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["search", "taskbar", "icon", "clean"],
            Description =
                "Hides the Search icon/box from the taskbar. Windows Search is still "
                + "accessible via the Win key. Saves taskbar space on smaller screens.",
            ApplyOps = [RegOp.SetDword(SearchUser, "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(SearchUser, "SearchboxTaskbarMode", 1)],
            DetectOps = [RegOp.CheckDword(SearchUser, "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "search-set-indexing-performance-mode",
            Label = "Enable Search Indexer Backoff Mode (Low I/O)",
            Category = "Windows Search Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["search", "indexer", "performance", "io", "background"],
            Description =
                "Sets the Windows Search indexer to backoff mode, reducing its disk I/O "
                + "priority when the system is under load. Prevents indexing from "
                + "degrading foreground application performance.",
            ApplyOps = [RegOp.SetDword(SearchInternal, "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.DeleteValue(SearchInternal, "SetupCompletedSuccessfully")],
            DetectOps = [RegOp.CheckDword(SearchInternal, "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "search-disable-device-sync-search",
            Label = "Disable Cross-Device Search Sync",
            Category = "Windows Search Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["search", "sync", "device", "privacy", "cloud"],
            Description =
                "Disables Windows Search syncing query history and results across "
                + "devices connected to the same Microsoft account. Keeps search history "
                + "local to this machine only.",
            ApplyOps = [RegOp.SetDword(SearchPolicy, "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(SearchPolicy, "AllowCortana")],
            DetectOps = [RegOp.CheckDword(SearchPolicy, "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "search-enable-classic-search-box",
            Label = "Use Compact Search Box (Icon Only)",
            Category = "Windows Search Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["search", "taskbar", "compact", "search box", "icon"],
            Description =
                "Sets the taskbar search to compact icon-only mode (no text box). "
                + "Value 1 = Search icon only, value 2 = full search box. "
                + "Saves taskbar space while keeping search accessible.",
            ApplyOps = [RegOp.SetDword(SearchUser, "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(SearchUser, "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(SearchUser, "SearchboxTaskbarMode", 1)],
        },
    ];
}
