namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyWindowsFeeds
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsFeeds — Windows RSS Feeds/
    // Ticker controlled via Group Policy (distinct from the Dsh/News widgets path).

    private const string FeedsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsFeeds";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsfeed-disable-windows-feeds",
            Label = "Disable Windows Feeds via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets DisableWindowsFeeds=1 in the WindowsFeeds Group Policy key. "
                + "Prevents the Windows Feeds (RSS/Atom) integration from running in the taskbar and File Explorer. "
                + "Eliminates background network polling for feed updates and removes the news ticker UI.",
            Tags = ["feeds", "rss", "news", "taskbar", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables Windows Feed subscriptions and background RSS polling.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "DisableWindowsFeeds", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "DisableWindowsFeeds")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "DisableWindowsFeeds", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-background-sync",
            Label = "Disable Background Feed Synchronisation",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets BackgroundSyncEnabled=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from silently synchronising feed content in the background. "
                + "Reduces network traffic and CPU wakeups from feed polling tasks.",
            Tags = ["feeds", "sync", "background", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed content no longer syncs in the background; pages only update when manually opened.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "BackgroundSyncEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "BackgroundSyncEnabled")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "BackgroundSyncEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-prevent-feed-subscription",
            Label = "Prevent Users from Subscribing to Feeds",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets PreventSubscription=1 in the WindowsFeeds Group Policy key. "
                + "Blocks users from subscribing to new RSS/Atom feeds via Internet Explorer or Feed Discovery. "
                + "Useful in controlled environments where feed subscriptions must be centrally managed.",
            Tags = ["feeds", "subscription", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users cannot add new RSS feed subscriptions in browsers or via auto-discovery.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "PreventSubscription", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "PreventSubscription")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "PreventSubscription", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-prevent-feed-discovery",
            Label = "Prevent Automatic Feed Discovery",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets PreventAutoDiscovery=1 in the WindowsFeeds Group Policy key. "
                + "Stops Internet Explorer and other browsers from automatically discovering available feeds "
                + "on visited web pages. Eliminates the feed icon in the toolbar and related network probes.",
            Tags = ["feeds", "discovery", "browser", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed discovery in browsers is disabled; no auto-detection of RSS/Atom links.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "PreventAutoDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "PreventAutoDiscovery")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "PreventAutoDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-unlocked-feeds",
            Label = "Lock Feed List to Prevent User Modifications",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets FeedListLocked=1 in the WindowsFeeds Group Policy key. "
                + "Prevents users from adding, removing, or modifying feed subscriptions. "
                + "Administrators retain full control over what feed sources are available systemwide.",
            Tags = ["feeds", "lockdown", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed list is read-only for standard users; only admins can change subscriptions.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "FeedListLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "FeedListLocked")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "FeedListLocked", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-feed-download",
            Label = "Block Feed Content Download",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowFeedDownload=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from downloading feed content to the local machine, stopping "
                + "offline reading caches and news-article pre-fetch from consuming bandwidth and storage.",
            Tags = ["feeds", "download", "bandwidth", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed articles are not pre-fetched; online access required to view feed content.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowFeedDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowFeedDownload")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowFeedDownload", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-third-party-feeds",
            Label = "Block Third-Party Feed Providers",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowThirdPartyFeeds=0 in the WindowsFeeds Group Policy key. "
                + "Restricts feed aggregation to Windows-native sources only, preventing third-party "
                + "feed aggregators or browser extensions from registering as system-level feed providers.",
            Tags = ["feeds", "third-party", "policy", "enterprise", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Only Windows-native feed mechanisms are permitted; third-party aggregators are blocked.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowThirdPartyFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowThirdPartyFeeds")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowThirdPartyFeeds", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-feed-reading-pane",
            Label = "Disable Feed Reading Pane",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets DisableReadingPane=1 in the WindowsFeeds Group Policy key. "
                + "Removes the feed reading pane from Internet Explorer and Windows RSS Platform view. "
                + "Reduces distraction and prevents previewing unapproved news content inside the browser.",
            Tags = ["feeds", "reading-pane", "browser", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Reading pane in feed viewer is hidden; feeds show in list-only mode.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "DisableReadingPane", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "DisableReadingPane")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "DisableReadingPane", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-enclosure-download",
            Label = "Block Feed Enclosure (Podcast) Auto-Download",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowEnclosureDownload=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from automatically downloading podcast and media enclosures "
                + "attached to RSS feed items. Eliminates background large-file downloads triggered by feed updates.",
            Tags = ["feeds", "podcast", "enclosure", "download", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Podcast/media enclosures in RSS feeds are not auto-downloaded.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowEnclosureDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowEnclosureDownload")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowEnclosureDownload", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-restrict-feed-secure-only",
            Label = "Restrict Feeds to HTTPS Sources Only",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets SecureFeedsOnly=1 in the WindowsFeeds Group Policy key. "
                + "Enforces that only feeds served over HTTPS are accepted by the Windows RSS Platform. "
                + "Blocks plain HTTP feed URLs which could be subject to man-in-the-middle injection and content tampering.",
            Tags = ["feeds", "https", "security", "policy", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "HTTP feed URLs are rejected; all feed sources must use HTTPS.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "SecureFeedsOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "SecureFeedsOnly")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "SecureFeedsOnly", 1)],
        },
    ];
}
