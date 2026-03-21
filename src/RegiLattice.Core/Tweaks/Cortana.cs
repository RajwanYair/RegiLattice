namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Cortana
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cortana-disable-cortana-lockscreen",
            Label = "Disable Cortana on Lock Screen",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Cortana voice assistant on the lock screen.",
            Tags = ["cortana", "privacy", "lockscreen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-web-search-start",
            Label = "Disable Web Search in Start Menu",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bing web results in Start menu search via CurrentUser registry keys.",
            Tags = ["search", "privacy", "bing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "CortanaConsent", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "CortanaConsent"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-highlights-policy",
            Label = "Disable Search Highlights",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Bing-curated 'Search Highlights' content from the Windows search box.",
            Tags = ["search", "bing", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-hide-search-box",
            Label = "Hide Taskbar Search Box",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the search box / icon from the taskbar.",
            Tags = ["search", "taskbar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SearchboxTaskbarMode", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SearchboxTaskbarMode", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-cortana-completely",
            Label = "Disable Cortana Entirely",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Completely disables Cortana via Group Policy.",
            Tags = ["cortana", "privacy", "assistant"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing",
            Label = "Disable Windows Search Indexing Service",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Search indexing service entirely.",
            Tags = ["search", "cortana", "indexing", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-highlights-dynamic",
            Label = "Disable Dynamic Search Highlights",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables dynamic search highlights and tips in the search box.",
            Tags = ["search", "cortana", "highlights", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-search-aadmsa",
            Label = "Disable AAD/MSA Cloud Search",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AAD and MSA cloud search content in Windows Search results.",
            Tags = ["search", "cortana", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-find-my-files",
            Label = "Disable Enhanced Search (Find My Files)",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets search mode to classic, disabling enhanced Find My Files indexing.",
            Tags = ["search", "cortana", "indexing", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "SearchMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "SearchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "SearchMode", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-location",
            Label = "Disable Windows Search Location Access",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Search from using device location.",
            Tags = ["search", "cortana", "location", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-highlights",
            Label = "Disable Search Highlights",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables search highlights (trending searches, news) in the Windows Search box. Reduces distractions and network traffic. Default: Enabled. Recommended: Disabled.",
            Tags = ["cortana", "search", "highlights", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-search",
            Label = "Disable Cloud Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables cloud content in Windows Search results. Only shows local files and settings. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-web-search",
            Label = "Disable Search Box Web Suggestions",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables web suggestions and results in the Windows Search box. Only shows local results. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "web", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "CortanaConsent", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "CortanaConsent"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-bing-search",
            Label = "Disable Bing Search in Start Menu",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Bing search results integration in the Start menu and taskbar search. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "bing", "search", "start-menu", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-personalization",
            Label = "Disable Cloud Content Personalization",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables cloud-based content personalization in Windows Search via HKLM policy (AllowCloudSearch=0). Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "cloud", "personalization", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-metered",
            Label = "Disable Search Over Metered Connections",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows Search from using web search over metered or pay-per-use network connections. Default: Enabled. Recommended: Disabled to save bandwidth.",
            Tags = ["cortana", "search", "metered", "bandwidth", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "ConnectedSearchUseWebOverMeteredConnections",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "ConnectedSearchUseWebOverMeteredConnections"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "ConnectedSearchUseWebOverMeteredConnections",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-voice-activation",
            Label = "Disable Voice Activation",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables voice activation for Cortana and speech services. Prevents the microphone from listening for wake words. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "voice", "activation", "microphone", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences", "VoiceActivationOn", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences", "VoiceActivationOn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences", "VoiceActivationOn", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-device-search-history",
            Label = "Disable Device Search History",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables local device search history storage. Prevents Windows from saving search queries on the device. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "history", "device", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-battery",
            Label = "Disable Search Indexing on Battery",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the search indexer from running on battery power. Saves battery on laptops. Default: continue on battery.",
            Tags = ["cortana", "search", "indexer", "battery", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-safe-search",
            Label = "Disable Safe Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Safe Search content filtering in Windows Search results. Default: moderate filtering.",
            Tags = ["cortana", "search", "safe-search", "filter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings", "SafeSearchMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings", "SafeSearchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings", "SafeSearchMode", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-bing-search-in-start",
            Label = "Disable Bing Search in Start Menu",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bing web search results from appearing in Start menu searches. Only local results shown. Default: enabled.",
            Tags = ["cortana", "bing", "search", "start-menu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-outlook",
            Label = "Disable Search Indexing of Outlook",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Search from indexing Microsoft Outlook data. Reduces CPU and disk usage. Default: indexed.",
            Tags = ["cortana", "search", "index", "outlook"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
        },
        new TweakDef
        {
            Id = "cortana-block-bing-in-start",
            Label = "Block Bing Results in Start Menu",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Bing web results from appearing in Windows Start menu search. Local-only search results. Default: Bing results shown.",
            Tags = ["cortana", "bing", "start", "search"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "cortana-block-web-results-policy",
            Label = "Block Web Search via Group Policy",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables web search in Windows Search via Group Policy. Strongest method to prevent web queries from desktop search. Default: allowed.",
            Tags = ["cortana", "web", "search", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-content-search",
            Label = "Disable Cloud Content Search",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables searching cloud content (OneDrive, Outlook, SharePoint) from Windows Search. Local files only. Default: cloud content included.",
            Tags = ["cortana", "cloud", "search", "content"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-searchbox-suggestions",
            Label = "Disable Search Box Suggestions",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables search suggestions and recommendations in the Windows Search box. Reduces network traffic and distractions. Default: enabled.",
            Tags = ["cortana", "searchbox", "suggestions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 0),
            ],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "cortana-disable-search-in-store",
            Label = "Disable Search the Microsoft Store",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Search the Microsoft Store' link in Windows Search results. Removes promotional app suggestions.",
            Tags = ["cortana", "search", "store", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoUseStoreOpenWith", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoUseStoreOpenWith")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoUseStoreOpenWith", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-history-local",
            Label = "Disable Local Search History",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables local search history in the Windows Search box. Prevents recent searches from appearing as suggestions.",
            Tags = ["cortana", "search", "history", "local"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-cortana-consent",
            Label = "Disable Cortana Consent Required",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Cortana consent popup that appears on first use. Prevents Cortana from requesting permissions.",
            Tags = ["cortana", "consent", "popup", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "cortana-show-search-icon-only",
            Label = "Show Search Icon Only (Hide Search Box)",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows only the search icon on the taskbar instead of the full search box. Saves taskbar space while keeping search accessible.",
            Tags = ["cortana", "search", "icon", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-windows-copilot",
            Label = "Disable Windows Copilot in Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot integration in Windows Search. Prevents AI-powered suggestions and Bing Chat results from appearing.",
            Tags = ["cortana", "copilot", "ai", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-transparency",
            Label = "Disable Search Box Transparency",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables transparency effects in the Windows Search flyout. Improves rendering performance on integrated GPUs.",
            Tags = ["cortana", "search", "transparency", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTransparencyEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTransparencyEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTransparencyEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-removable",
            Label = "Disable Search Indexing on Removable Drives",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Search from indexing removable drives (USB sticks, external HDDs). Reduces I/O on external media.",
            Tags = ["cortana", "indexing", "removable", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableRemovableDriveIndexing", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableRemovableDriveIndexing"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableRemovableDriveIndexing", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-encrypted",
            Label = "Disable Indexing of Encrypted Files",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Search from indexing encrypted (EFS) files. Reduces indexing overhead and potential data exposure in the search index.",
            Tags = ["cortana", "indexing", "encrypted", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-limit-search-indexer-throttle",
            Label = "Throttle Search Indexer CPU Usage",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the Search Indexer to reduced performance mode. Caps CPU usage during indexing, trading speed for lower system impact.",
            Tags = ["cortana", "indexer", "throttle", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-preview-pane",
            Label = "Disable Search Preview Pane",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the preview pane in Windows Search that shows file contents and web results. Speeds up search UI rendering.",
            Tags = ["cortana", "search", "preview", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarPreview", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarPreview", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-can-enable",
            Label = "Prevent Cortana from Being Enabled (Policy)",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CanCortanaBeEnabled=0 via Windows Search policy. Blocks the Cortana service from being enabled even by users with admin rights.",
            Tags = ["cortana", "search", "policy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "CanCortanaBeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "CanCortanaBeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "CanCortanaBeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-lock-screen-search",
            Label = "Disable Search Box on Lock Screen",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets IsLockScreenSearchEnabled=0 in SearchSettings. Removes the search field from the lock screen, reducing the attack surface for unauthenticated searches.",
            Tags = ["cortana", "search", "lock-screen", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsLockScreenSearchEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsLockScreenSearchEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsLockScreenSearchEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-personalized-search",
            Label = "Disable Personalised Search Results",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets IsPersonalSearchEnabled=0 in SearchSettings. Turns off personalised ranking of search results based on past activity.",
            Tags = ["cortana", "search", "personalization", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsPersonalSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsPersonalSearchEnabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsPersonalSearchEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-copilot-in-search",
            Label = "Disable Copilot / AI Assistant in Search",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets IsAssistantEnabled=0 in SearchSettings. Removes the Bing Copilot AI answer panel from Windows Search results.",
            Tags = ["cortana", "search", "copilot", "ai", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-hide-copilot-taskbar-button",
            Label = "Hide Copilot Button from Taskbar",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets ShowCopilotButton=0 in Explorer Advanced. Removes the Copilot launch button from the Windows 11 taskbar.",
            Tags = ["cortana", "copilot", "taskbar", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-spelling-in-search",
            Label = "Disable Spelling Correction in Search",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EnableSpellingCorrection=0 in SearchSettings. Stops Windows Search from auto-correcting query spelling, which would otherwise expand or alter the intended search.",
            Tags = ["cortana", "search", "spelling", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "EnableSpellingCorrection", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "EnableSpellingCorrection"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "EnableSpellingCorrection", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-online-tips-search",
            Label = "Disable Online Tips in Search Results",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets OnlineTipsEnabled=0 in SearchSettings. Prevents Windows Search from appending online troubleshooting tips to local search results.",
            Tags = ["cortana", "search", "tips", "online"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "OnlineTipsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "OnlineTipsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "OnlineTipsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-office-indexing",
            Label = "Disable Search Indexing of Microsoft Office Files (Policy)",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets PreventIndexingMicrosoftOffice=1 in Windows Search policy. Stops the indexer from crawling Office documents, reducing background I/O on systems where Office file search is not needed.",
            Tags = ["cortana", "search", "indexer", "office", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingMicrosoftOffice", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingMicrosoftOffice"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingMicrosoftOffice", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-unc-indexing",
            Label = "Disable Search Indexing of UNC / Network Paths",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingUNCCrawledPaths=1 in Windows Search policy. Stops the indexer from traversing UNC shares, eliminating network bandwidth consumption from background crawl.",
            Tags = ["cortana", "search", "indexer", "unc", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingUNCCrawledPaths", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingUNCCrawledPaths"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingUNCCrawledPaths", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-public-folder-indexing",
            Label = "Disable Search Indexing of Public Folders",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingPublicFolders=1 in Windows Search policy. Stops the indexer from crawling shared Public folders, reducing unnecessary read I/O.",
            Tags = ["cortana", "search", "indexer", "public-folders"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingPublicFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingPublicFolders")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingPublicFolders", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-dynamic-wsb-content",
            Label = "Disable Dynamic Content in Windows Search Bar",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableDynamicContentInWSB=0 via Windows Search policy. Prevents the search bar from displaying rotating news, trending searches, or other dynamic web content.",
            Tags = ["cortana", "search", "dynamic-content", "news", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-gpo-block-bing-answers",
            Label = "Disable Bing Answers in Windows Search (Policy)",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableBingAnswers=0 in Windows Search policy. Prevents Bing from returning inline answer cards (weather, calculations, sports scores) in the local search panel.",
            Tags = ["cortana", "search", "bing", "answers", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableBingAnswers", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableBingAnswers")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableBingAnswers", 0)],
        },
    ];
}
