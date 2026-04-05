namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Sprint 657-661 ────────────────────────────────────────────────────────────
// RegiLattice.Core — Tweaks/PolicyMisc2.cs
// Windows Feeds, Compressed Folders, Windows Chat (Teams), extended Speech input,
// and extended Text Input policies.
// Category varies per module — see individual class headers.
// All tweaks: NeedsAdmin = true, CorpSafe = true, declarative RegOps.

// ── Sprint 657 — PolicyWindowsFeeds ──────────────────────────────────────────
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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
            Category = "Windows Feeds Policy",
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

// ── Sprint 658 — PolicyCompressedFolders ─────────────────────────────────────
internal static class PolicyCompressedFolders
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Explorer — controls
    // ZIP/compressed folder integration in File Explorer and shell.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders — dedicated key.

    private const string ZipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders";
    private const string ExplKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "zipfld-disable-compressed-folders",
            Label = "Disable ZIP Compressed Folder Support in Explorer",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Removes the native ZIP/compressed folder handler from File Explorer. "
                + "Users can no longer double-click a ZIP file to browse it as a folder within Explorer. "
                + "Useful when a third-party archiver (7-Zip, WinRAR) is the preferred tool on managed machines.",
            Tags = ["zip", "compressed", "explorer", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ZIP files no longer open as virtual folders in Explorer; requires a third-party archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-extract-all",
            Label = "Remove 'Extract All' Context-Menu Option",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableExtractAll=1 in the CompressedFolders Group Policy key. "
                + "Hides the 'Extract All' entry from the right-click context menu on ZIP files. "
                + "Combined with a managed archiver deployment, this enforces the corporate tool for archive extraction.",
            Tags = ["zip", "extract", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "'Extract All' is removed from ZIP context menus; users must use an installed archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableExtractAll", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableExtractAll")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableExtractAll", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-compress-selected-files",
            Label = "Remove 'Compress to ZIP' Context-Menu Option",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableNewCompressedFolder=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compress to ZIP file' entry from the File Explorer shell context menu. "
                + "Prevents users from creating ZIP files directly from Explorer, directing archive operations to managed tools.",
            Tags = ["zip", "compress", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "ZIP creation from Explorer context menu is hidden; archiver tool required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNewCompressedFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNewCompressedFolder")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNewCompressedFolder", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-block-network-archive-open",
            Label = "Block Opening Remote ZIP Files as Virtual Folders",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableNetworkCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents users from browsing ZIP archives located on network shares as virtual folders. "
                + "Reduces risk of data exfiltration via archive browsing of network resources and prevents "
                + "potential path-traversal attacks embedded in malicious remote ZIP files.",
            Tags = ["zip", "network", "security", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ZIP files on network drives cannot be browsed as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNetworkCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-cab-browsing",
            Label = "Disable CAB File Browsing in Explorer",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableCabFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents File Explorer from opening Microsoft Cabinet (.cab) files as virtual folders. "
                + "CAB files are used as installers and update containers — browsing them directly can "
                + "expose sensitive setup binaries. Forcing use of proper extraction tools adds an audit layer.",
            Tags = ["cab", "cabinet", "compressed", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = ".cab files no longer open as virtual folders; dedicated extraction required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCabFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCabFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCabFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-autorun-in-archive",
            Label = "Block AutoRun Execution Inside Archive Folders",
            Category = "Compressed Folders Policy",
            Description =
                "Sets BlockArchiveAutoRun=1 in the CompressedFolders Group Policy key. "
                + "Prevents autorun.inf scripts embedded in ZIP/CAB archives from executing when the archive "
                + "is browsed as a virtual folder. Removes a potential initial-access vector for malware "
                + "distributed via weaponised archives delivered over email or USB.",
            Tags = ["zip", "autorun", "security", "malware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AutoRun scripts inside archives are blocked from executing within Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "BlockArchiveAutoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "BlockArchiveAutoRun")],
            DetectOps = [RegOp.CheckDword(ZipKey, "BlockArchiveAutoRun", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-zip-sendto",
            Label = "Remove 'Send To Compressed Folder' from Right-Click",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableSendToCompressed=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compressed (zipped) folder' destination from the Send To context menu entry. "
                + "Prevents casual in-place ZIP creation that bypasses DLP scanning on managed endpoints.",
            Tags = ["zip", "sendto", "context-menu", "dlp", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Send To > Compressed Folder is hidden; users must use an explicit archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableSendToCompressed", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableSendToCompressed")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableSendToCompressed", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-archive-max-size",
            Label = "Enforce Maximum Archive Size Limit",
            Category = "Compressed Folders Policy",
            Description =
                "Sets MaxArchiveSizeMB=512 in the CompressedFolders Group Policy key. "
                + "Limits the maximum size of archives that Explorer will open as virtual folders to 512 MB. "
                + "Prevents ZIP-bomb denial-of-service attacks and runaway memory consumption when users "
                + "accidentally open decompression-ratio-maximised archives.",
            Tags = ["zip", "size-limit", "security", "dos", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archives larger than 512 MB will not open as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "MaxArchiveSizeMB", 512)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "MaxArchiveSizeMB")],
            DetectOps = [RegOp.CheckDword(ZipKey, "MaxArchiveSizeMB", 512)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-archive-preview-handler",
            Label = "Disable Archive Preview Handler in Reading Pane",
            Category = "Compressed Folders Policy",
            Description =
                "Sets DisableArchivePreviewHandler=1 in the CompressedFolders Group Policy key. "
                + "Prevents the Explorer Reading Pane from rendering a ZIP/CAB file preview when it is selected. "
                + "Preview rendering parses archive headers in-process; disabling it reduces attack surface for "
                + "vulnerabilities in the compressed-folders shell handler.",
            Tags = ["zip", "preview", "reading-pane", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archive files show no preview in Explorer Reading Pane.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableArchivePreviewHandler", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableArchivePreviewHandler")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableArchivePreviewHandler", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-enforce-archive-scan-on-open",
            Label = "Enforce Antivirus Scan Before Opening Archive Content",
            Category = "Compressed Folders Policy",
            Description =
                "Sets RequireScanBeforeArchiveOpen=1 in the CompressedFolders Group Policy key. "
                + "Forces Windows Defender or the registered antivirus to scan archive contents before "
                + "the virtual folder view is presented to the user. Prevents deferred-scan gaps where "
                + "malicious payloads inside archives reach the desktop before AV inspection completes.",
            Tags = ["zip", "antivirus", "scan", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Archive contents are AV-scanned before being displayed in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "RequireScanBeforeArchiveOpen")],
            DetectOps = [RegOp.CheckDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
        },
    ];
}

// ── Sprint 659 — PolicyWindowsChat ───────────────────────────────────────────
internal static class PolicyWindowsChat
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Chat — controls the Teams
    // consumer chat integration pinned to the Windows 11 taskbar.
    // Additional key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Calling — voice calling integration.

    private const string ChatKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Chat";
    private const string CallingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Calling";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wschat-set-chat-icon-hidden",
            Label = "Hide Chat Icon from Taskbar via Policy",
            Category = "Chat Integration Policy",
            Description =
                "Sets ChatIcon=2 in the Windows Chat Group Policy key. "
                + "Value 2 hides the Teams Chat / Meet Now icon from the Windows 11 taskbar. "
                + "Removes the consumer collaboration entry point on managed enterprise workstations "
                + "where the full Microsoft Teams Professional client is deployed instead.",
            Tags = ["chat", "teams", "taskbar", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Teams consumer chat icon is hidden from taskbar; Teams business client unaffected.",
            ApplyOps = [RegOp.SetDword(ChatKey, "ChatIcon", 2)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "ChatIcon")],
            DetectOps = [RegOp.CheckDword(ChatKey, "ChatIcon", 2)],
        },
        new TweakDef
        {
            Id = "wschat-disable-consumer-teams",
            Label = "Block Consumer Microsoft Teams Chat",
            Category = "Chat Integration Policy",
            Description =
                "Sets AllowTeamsChat=0 in the Windows Chat Group Policy key. "
                + "Blocks the consumer/personal Microsoft Teams integration built into Windows 11. "
                + "Prevents first-run wizard, account linking, and persistent notification badge for "
                + "Teams consumer on managed endpoints where personal accounts must not be used.",
            Tags = ["chat", "teams", "consumer", "policy", "enterprise", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Consumer Teams chat cannot be launched or configured from Windows 11.",
            ApplyOps = [RegOp.SetDword(ChatKey, "AllowTeamsChat", 0)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "AllowTeamsChat")],
            DetectOps = [RegOp.CheckDword(ChatKey, "AllowTeamsChat", 0)],
        },
        new TweakDef
        {
            Id = "wschat-disable-chat-notification-badge",
            Label = "Remove Chat Notification Badge on Taskbar",
            Category = "Chat Integration Policy",
            Description =
                "Sets HideChatBadge=1 in the Windows Chat Group Policy key. "
                + "Removes the unread-message badge overlaid on the Chat taskbar button. "
                + "Reduces distraction on workstations and prevents consumer-chat notifications "
                + "from drawing attention during corporate presentations or screen shares.",
            Tags = ["chat", "teams", "notification", "taskbar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Chat notification badge (unread count) no longer shown on taskbar.",
            ApplyOps = [RegOp.SetDword(ChatKey, "HideChatBadge", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "HideChatBadge")],
            DetectOps = [RegOp.CheckDword(ChatKey, "HideChatBadge", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-first-launch-experience",
            Label = "Suppress Teams Chat First-Launch Welcome Screen",
            Category = "Chat Integration Policy",
            Description =
                "Sets SuppressFirstLaunchExperience=1 in the Windows Chat Group Policy key. "
                + "Skips the consumer Teams onboarding flow (sign-in prompt, terms screen, EULA) "
                + "when a user launches Chat for the first time. On managed machines the consumer "
                + "profile wizard interferes with standard provisioning and MDM enrollment flows.",
            Tags = ["chat", "teams", "onboarding", "first-run", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Consumer Teams first-run welcome flow is suppressed.",
            ApplyOps = [RegOp.SetDword(ChatKey, "SuppressFirstLaunchExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "SuppressFirstLaunchExperience")],
            DetectOps = [RegOp.CheckDword(ChatKey, "SuppressFirstLaunchExperience", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-personal-account-linking",
            Label = "Block Personal Account Linking to Chat",
            Category = "Chat Integration Policy",
            Description =
                "Sets BlockPersonalAccountLinking=1 in the Windows Chat Group Policy key. "
                + "Prevents users from signing the built-in Chat with a personal Microsoft account. "
                + "Enforces the boundary between consumer and enterprise identity on shared and BYOD machines.",
            Tags = ["chat", "teams", "personal-account", "identity", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Personal Microsoft accounts cannot be linked to the Windows Chat taskbar widget.",
            ApplyOps = [RegOp.SetDword(ChatKey, "BlockPersonalAccountLinking", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "BlockPersonalAccountLinking")],
            DetectOps = [RegOp.CheckDword(ChatKey, "BlockPersonalAccountLinking", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-calling-integration",
            Label = "Disable Windows 11 Calling Integration",
            Category = "Chat Integration Policy",
            Description =
                "Sets AllowWindowsCalling=0 in the Calling Group Policy key. "
                + "Removes the Windows 11 calling integration that allows PSTN-linked mobile phone "
                + "numbers to be used directly from the Windows Start menu and taskbar call panel. "
                + "Reduces data-sharing with the linked mobile device on managed workstations.",
            Tags = ["calling", "phone", "integration", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows 11 taskbar Calling panel is disabled; desk phone/VOIP tools unaffected.",
            ApplyOps = [RegOp.SetDword(CallingKey, "AllowWindowsCalling", 0)],
            RemoveOps = [RegOp.DeleteValue(CallingKey, "AllowWindowsCalling")],
            DetectOps = [RegOp.CheckDword(CallingKey, "AllowWindowsCalling", 0)],
        },
        new TweakDef
        {
            Id = "wschat-disable-calling-auto-start",
            Label = "Prevent Windows Calling Service Auto-Start",
            Category = "Chat Integration Policy",
            Description =
                "Sets DisableCallingAutoStart=1 in the Calling Group Policy key. "
                + "Stops the Windows Calling background service from starting automatically at user login. "
                + "Reduces login latency and background network activity from the calling integration agent.",
            Tags = ["calling", "startup", "background", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Calling service does not auto-start; initiated only if user manually opens Calling.",
            ApplyOps = [RegOp.SetDword(CallingKey, "DisableCallingAutoStart", 1)],
            RemoveOps = [RegOp.DeleteValue(CallingKey, "DisableCallingAutoStart")],
            DetectOps = [RegOp.CheckDword(CallingKey, "DisableCallingAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "wschat-disable-caller-id-lookup",
            Label = "Disable Caller ID Lookup via Microsoft Services",
            Category = "Chat Integration Policy",
            Description =
                "Sets AllowCallerIdLookup=0 in the Calling Group Policy key. "
                + "Prevents Windows from sending caller phone numbers to Microsoft Cloud to resolve "
                + "caller identity and display name before a call is answered. "
                + "Caller ID numbers remain local-only; no data leaves the device for name resolution.",
            Tags = ["calling", "caller-id", "privacy", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Phone numbers from incoming calls are not sent to Microsoft for lookup.",
            ApplyOps = [RegOp.SetDword(CallingKey, "AllowCallerIdLookup", 0)],
            RemoveOps = [RegOp.DeleteValue(CallingKey, "AllowCallerIdLookup")],
            DetectOps = [RegOp.CheckDword(CallingKey, "AllowCallerIdLookup", 0)],
        },
        new TweakDef
        {
            Id = "wschat-disable-chat-history-sync",
            Label = "Block Cross-Device Chat History Sync",
            Category = "Chat Integration Policy",
            Description =
                "Sets DisableChatHistorySync=1 in the Windows Chat Group Policy key. "
                + "Prevents the consumer Teams chat history from synchronising across all devices linked "
                + "to the same personal Microsoft account. On shared terminals or kiosk devices, chat "
                + "history from other devices must not appear and could expose personal/confidential conversations.",
            Tags = ["chat", "history", "sync", "privacy", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Chat history from other devices is not synced to this machine.",
            ApplyOps = [RegOp.SetDword(ChatKey, "DisableChatHistorySync", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "DisableChatHistorySync")],
            DetectOps = [RegOp.CheckDword(ChatKey, "DisableChatHistorySync", 1)],
        },
        new TweakDef
        {
            Id = "wschat-restrict-chat-file-transfer",
            Label = "Block File Transfer via Consumer Chat",
            Category = "Chat Integration Policy",
            Description =
                "Sets DisableChatFileTransfer=1 in the Windows Chat Group Policy key. "
                + "Prevents users from sending or receiving files through the consumer Teams chat integration. "
                + "Removes an unmonitored side channel for data exfiltration that bypasses corporate DLP "
                + "policies configured on the enterprise Teams deployment.",
            Tags = ["chat", "file-transfer", "dlp", "security", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "File sharing in consumer chat is blocked; corporate Teams file sharing unaffected.",
            ApplyOps = [RegOp.SetDword(ChatKey, "DisableChatFileTransfer", 1)],
            RemoveOps = [RegOp.DeleteValue(ChatKey, "DisableChatFileTransfer")],
            DetectOps = [RegOp.CheckDword(ChatKey, "DisableChatFileTransfer", 1)],
        },
    ];
}

// ── Sprint 660 — PolicyTextInputExt ──────────────────────────────────────────
internal static class PolicyTextInputExt
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\TextInput — additional values beyond
    // the 5 already covered in existing modules (AllowHandwritingLMUpdate,
    // AllowInputDeviceUserInterface, AllowLinguisticDataCollection,
    // AllowTouchKeyboardAutoInvokeInDesktopMode, AllowVoiceTyping).

    private const string TiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TextInput";
    private const string ImeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IME";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "txtin2-disable-text-prediction",
            Label = "Disable Text Prediction for Physical Keyboards via Policy",
            Category = "Text Input Policy",
            Description =
                "Sets AllowHardwareKeyboardTextSuggestions=0 in the TextInput Group Policy key. "
                + "Prevents Windows from showing inline word-completion suggestions on hardware (physical) keyboards. "
                + "Text prediction sends keystroke patterns to the language model; disabling preserves "
                + "the privacy of typed content on corporate devices.",
            Tags = ["text-input", "keyboard", "prediction", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Word suggestions no longer appear when typing on a physical keyboard.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowHardwareKeyboardTextSuggestions", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowHardwareKeyboardTextSuggestions")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowHardwareKeyboardTextSuggestions", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-user-settings-override",
            Label = "Lock Text Input Settings from User Override",
            Category = "Text Input Policy",
            Description =
                "Sets AllowUserSettings=0 in the TextInput Group Policy key. "
                + "Prevents users from modifying text input settings (autocorrect, prediction thresholds, "
                + "handwriting personalisation) via Windows Settings. "
                + "All text-input behaviour is controlled exclusively by Group Policy on managed machines.",
            Tags = ["text-input", "settings", "lockdown", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Text Input settings page is read-only for standard users.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowUserSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowUserSettings")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowUserSettings", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-autocorrect",
            Label = "Disable Hardware Keyboard Autocorrect via Policy",
            Category = "Text Input Policy",
            Description =
                "Sets AllowKeyboardAutocorrect=0 in the TextInput Group Policy key. "
                + "Disables automatic spelling correction on hardware keyboard input. "
                + "Prevents autocorrect from silently changing intended technical terms, passwords, "
                + "or code identifiers in document editors and developer tools.",
            Tags = ["text-input", "autocorrect", "keyboard", "policy", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Keyboard autocorrect is disabled; all typed text is preserved verbatim.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowKeyboardAutocorrect", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowKeyboardAutocorrect")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowKeyboardAutocorrect", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-user-feedback-submission",
            Label = "Block Text Input User Feedback Telemetry",
            Category = "Text Input Policy",
            Description =
                "Sets AllowUserFeedback=0 in the TextInput Group Policy key. "
                + "Prevents the Text Input (handwriting, touch keyboard, voice typing) subsystem from "
                + "prompting users for satisfaction ratings or submitting diagnostic feedback data "
                + "to Microsoft for language model improvement.",
            Tags = ["text-input", "telemetry", "feedback", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Text input feedback prompts are disabled; no telemetry submitted from input panel.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowUserFeedback", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowUserFeedback")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowUserFeedback", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ink-personalization-upload",
            Label = "Block Handwriting Personalisation Data Upload",
            Category = "Text Input Policy",
            Description =
                "Sets AllowHandwritingPersonalizationUpload=0 in the TextInput Group Policy key. "
                + "Prevents handwriting recognition data (pen strokes and corrections) from being "
                + "transmitted to Microsoft servers for model personalisation. "
                + "Handwriting recognition continues to function using on-device models only.",
            Tags = ["text-input", "handwriting", "personalisation", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ink/handwriting training data is not uploaded; on-device recognition continues.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowHandwritingPersonalizationUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowHandwritingPersonalizationUpload")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowHandwritingPersonalizationUpload", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-internet-access",
            Label = "Block IME Access to External Prediction Services",
            Category = "Text Input Policy",
            Description =
                "Sets AllowIMENetworkAccess=0 in the TextInput Group Policy key. "
                + "Prevents Input Method Editors (IME) for CJK and other scripts from accessing the "
                + "internet for cloud-based candidate suggestions, emoji recommendations, and dictionary updates. "
                + "Eliminates keystroke exfiltration risk from cloud-connected IME prediction services.",
            Tags = ["text-input", "ime", "network", "privacy", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME prediction is limited to local/offline dictionaries; cloud suggestions disabled.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowIMENetworkAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowIMENetworkAccess")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowIMENetworkAccess", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-cloud-ime-candidates",
            Label = "Disable Cloud-Based IME Candidate Suggestions",
            Category = "Text Input Policy",
            Description =
                "Sets AllowCloudCandidates=0 in the IME Group Policy key. "
                + "Prevents the Windows IME from fetching real-time cloud candidate improvements. "
                + "Cloud IME candidates require sending the current input context to Microsoft servers, "
                + "posing a risk of partial document or credential context disclosure in enterprise environments.",
            Tags = ["ime", "cloud", "candidates", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME uses only installed local candidate dictionaries; no cloud lookups.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowCloudCandidates", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowCloudCandidates")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowCloudCandidates", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-update",
            Label = "Block IME Automatic Dictionary Updates",
            Category = "Text Input Policy",
            Description =
                "Sets AllowIMEUpdate=0 in the IME Group Policy key. "
                + "Prevents the Windows IME from automatically downloading dictionary and language model "
                + "updates from Microsoft Graph or Update servers. "
                + "Stabilises IME behaviour in controlled environments where software changes must be sanctioned.",
            Tags = ["ime", "update", "dictionary", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "IME dictionaries are frozen; language model updates require IT-approved deployment.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowIMEUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowIMEUpdate")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowIMEUpdate", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-telemetry",
            Label = "Disable IME Typing Telemetry",
            Category = "Text Input Policy",
            Description =
                "Sets AllowIMETelemetry=0 in the IME Group Policy key. "
                + "Blocks the Windows IME from transmitting typing pattern, candidate selection, "
                + "and correction data to Microsoft for telemetry and model improvement. "
                + "Typing content is a high-sensitivity data stream; telemetry must be controlled in regulated sectors.",
            Tags = ["ime", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME typing telemetry is disabled; no input pattern data transmitted to Microsoft.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowIMETelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowIMETelemetry")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowIMETelemetry", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-touch-keyboard-auto-invoke",
            Label = "Prevent Touch Keyboard Auto-Invoke in Tablet Mode",
            Category = "Text Input Policy",
            Description =
                "Sets AllowTouchKeyboardAutoInvoke=0 in the TextInput Group Policy key. "
                + "Disables the automatic appearance of the on-screen touch keyboard when a text field "
                + "gains focus in tablet mode. Users must manually invoke the touch keyboard, preventing "
                + "layout interference on large-screen kiosk and hybrid devices.",
            Tags = ["text-input", "touch-keyboard", "tablet", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Touch keyboard no longer pops up automatically when tapping text fields.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowTouchKeyboardAutoInvoke", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowTouchKeyboardAutoInvoke")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowTouchKeyboardAutoInvoke", 0)],
        },
    ];
}

// ── Sprint 661 — PolicySpeechInput ────────────────────────────────────────────
internal static class PolicySpeechInput
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\Speech — Speech Recognition / Voice Access.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\SpeechModel — online speech model policies.

    private const string SpeechKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Speech";
    private const string ModelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpeechModel";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "spkinput-disable-online-speech-recognition",
            Label = "Disable Online Speech Recognition via Policy",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowSpeechRecognition=0 in the Speech Group Policy key. "
                + "Prevents the cloud speech recognition service from being used for Windows speech features. "
                + "Voice data is only processed on-device; no audio is transmitted to Microsoft Speech servers. "
                + "Applies broadly to Cortana voice queries, Voice Typing, and Voice Access cloud enhancement.",
            Tags = ["speech", "recognition", "cloud", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Cloud speech recognition disabled; on-device speech processing continues.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechRecognition", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechRecognition")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechRecognition", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-activation",
            Label = "Block Always-On Voice Activation",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowVoiceActivation=0 in the Speech Group Policy key. "
                + "Prevents applications from using the always-on voice listening hook (keyword detection). "
                + "Eliminates the continuous microphone monitoring required for wake words ('Hey Cortana', etc.), "
                + "removing a permanent audio capture pipeline from the endpoint.",
            Tags = ["speech", "voice-activation", "wake-word", "microphone", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Always-on wake word detection is disabled; microphone not continuously monitored.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceActivation", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceActivation")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceActivation", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-model-update",
            Label = "Block Automatic Speech Model Updates",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowSpeechModelUpdate=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from automatically downloading and applying updated cloud or on-device "
                + "speech recognition model files. Stabilises speech behaviour in validated regulated "
                + "environments where untested model changes could affect accessibility tools.",
            Tags = ["speech", "model-update", "policy", "enterprise", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Speech model files are frozen; updates require IT-managed deployment.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-telemetry",
            Label = "Disable Speech Input Telemetry",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowSpeechTelemetry=0 in the Speech Group Policy key. "
                + "Blocks the Speech subsystem from sending diagnostic voice data, recognition accuracy "
                + "metrics, and corrected text snippets to Microsoft for model improvement. "
                + "Audio utterances and transcription corrections are classified as personal data under GDPR/HIPAA.",
            Tags = ["speech", "telemetry", "privacy", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Speech telemetry suppressed; no voice samples or transcription data transmitted.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechTelemetry")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-typing",
            Label = "Disable Voice Typing via Policy",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowVoiceTyping=0 in the Speech Group Policy key. "
                + "Disables the Voice Typing feature (Win+H) systemwide via Group Policy. "
                + "Prevents users from dictating text into any application, stopping the microphone "
                + "activation path associated with dictation on shared and kiosk workstations.",
            Tags = ["speech", "voice-typing", "dictation", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Voice Typing (Win+H) is disabled; microphone not used for dictation.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceTyping", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceTyping")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceTyping", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-cortana-voice",
            Label = "Disable Cortana Voice Interaction via Policy",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowCortanaVoice=0 in the Speech Group Policy key. "
                + "Prevents Cortana from accepting voice input and responding to spoken queries. "
                + "Complements the Cortana keyboard disable by also closing the audio/microphone channel "
                + "used for Cortana's voice assistant functionality.",
            Tags = ["speech", "cortana", "voice", "microphone", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cortana no longer accepts voice queries; keyboard interaction unaffected.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowCortanaVoice", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowCortanaVoice")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowCortanaVoice", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-personalization",
            Label = "Block Speech Personalisation Data Collection",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowSpeechPersonalization=0 in the Speech Group Policy key. "
                + "Stops Windows from collecting contacts, calendar events, frequently typed words, "
                + "and app usage patterns to personalise speech recognition accuracy. "
                + "This dataset would stay on-device but its aggregation represents a privacy concern "
                + "in regulated environments where data minimisation principles apply.",
            Tags = ["speech", "personalisation", "privacy", "policy", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Speech personalisation disabled; recognition accuracy unchanged.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechPersonalization")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-access-start",
            Label = "Prevent Voice Access from Starting at Login",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowVoiceAccessStartup=0 in the Speech Group Policy key. "
                + "Prevents the Windows Voice Access feature from automatically starting when a user logs "
                + "into Windows. Voice Access requires persistent microphone access; letting it auto-start "
                + "runs an unnecessary audio capture pipeline on workstations not requiring accessibility.",
            Tags = ["speech", "voice-access", "startup", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Voice Access does not auto-start; users can still launch it manually.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceAccessStartup")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-restrict-online-speech-model",
            Label = "Block Online Speech Model Download",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowOnlineSpeechModel=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from downloading an enhanced online speech recognition model "
                + "that improves accuracy beyond the locally installed model. "
                + "Disabling the download removes a background network data transfer and pins "
                + "speech processing to on-device models vetted by the organisation.",
            Tags = ["speech", "model", "download", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Online speech model is not downloaded; on-device model used exclusively.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowOnlineSpeechModel", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowOnlineSpeechModel")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowOnlineSpeechModel", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-access-across-lock",
            Label = "Disable Speech Recognition on Lock Screen",
            Category = "Speech Input Policy",
            Description =
                "Sets AllowSpeechOnLockScreen=0 in the Speech Group Policy key. "
                + "Prevents voice assistants and speech recognition from accepting voice input when "
                + "the workstation screen is locked. Eliminates the attack surface where an attacker "
                + "with physical access to a locked machine can issue voice commands to local assistants.",
            Tags = ["speech", "lock-screen", "security", "policy", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Voice assistant cannot be invoked from locked screen; prevents audio-based attacks.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechOnLockScreen")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
        },
    ];
}
