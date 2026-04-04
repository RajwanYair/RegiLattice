namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// Sprint 652-656 — v6.5.0
// 5 new policy modules × 10 tweaks = 50 tweaks
// All tweaks target HKLM machine-wide Group Policy paths.

/// <summary>
/// Sprint 652 — Windows Search machine-wide policy settings (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search
/// These keys configure Windows Search behaviour (Cortana, web search,
/// privacy, battery impact) via Group Policy for all users on the machine.
/// </summary>
internal static class PolicyWindowsSearch
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsepol-disable-cortana",
            Label = "Disable Cortana via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowCortana=0 under the Windows Search Group Policy path. "
                + "Prevents Cortana from being used as the search assistant. "
                + "Reduces background network activity and telemetry associated with Cortana queries.",
            Tags = ["cortana", "search", "policy", "privacy"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables Cortana voice assistant and reduces search-related telemetry.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCortana")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "wsepol-disable-web-search",
            Label = "Disable Web Search in Start Menu via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWebSearch=1 under the Windows Search Group Policy path. "
                + "Prevents web results from appearing in the Start menu search box. "
                + "Keeps local search results only and stops search queries from being sent to Bing.",
            Tags = ["search", "bing", "web", "policy", "privacy"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes web results from Start search; stops Bing queries.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWebSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWebSearch")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-disable-connected-search-web",
            Label = "Disable Connected Search Web Results via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ConnectedSearchUseWeb=0 under the Windows Search Group Policy path. "
                + "Disables the Connected Search feature that sends search queries to Microsoft "
                + "web services. Local search only; no outbound web search requests.",
            Tags = ["search", "web", "policy", "privacy", "connected-search"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks outbound connected search queries to Microsoft servers.",
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id = "wsepol-disable-cloud-search",
            Label = "Disable Cloud Search via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowCloudSearch=0 under the Windows Search Group Policy path. "
                + "Prevents Windows Search from including results from cloud sources (OneDrive, Outlook, etc.). "
                + "Search results are limited to local files and indexed content.",
            Tags = ["search", "cloud", "policy", "privacy", "onedrive"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes cloud content from search results; local search only.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "wsepol-disable-search-location",
            Label = "Block Location Access in Search via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowSearchToUseLocation=0 under the Windows Search Group Policy path. "
                + "Prevents Windows Search from using the device location to tailor search results. "
                + "Improves privacy by stopping location-based query enrichment.",
            Tags = ["search", "location", "policy", "privacy"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks search from accessing device location data.",
            ApplyOps = [RegOp.SetDword(Key, "AllowSearchToUseLocation", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSearchToUseLocation")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id = "wsepol-enforce-safe-search-strict",
            Label = "Enforce Strict SafeSearch via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ConnectedSearchSafeSearch=2 (Strict) under the Windows Search Group Policy path. "
                + "Forces the strictest SafeSearch level for web-connected search queries, "
                + "filtering explicit content from any search results returned via Cortana or Bing integration.",
            Tags = ["search", "safesearch", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Forces strict SafeSearch; filters explicit content from web search.",
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchSafeSearch", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchSafeSearch")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchSafeSearch", 2)],
        },
        new TweakDef
        {
            Id = "wsepol-disable-dynamic-search-content",
            Label = "Disable Dynamic Content in Search Box via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableDynamicContentInWSB=0 under the Windows Search Group Policy path. "
                + "Stops the search box from displaying dynamic promotional content, news tiles, "
                + "or trending topics pulled from Microsoft servers.",
            Tags = ["search", "dynamic-content", "policy", "privacy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides dynamic news/trending content in the search box.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "wsepol-block-remote-query",
            Label = "Block Remote Cortana Query via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRemoteQuery=1 under the Windows Search Group Policy path. "
                + "Prevents Cortana from querying remote services for information when invoked. "
                + "Queries are processed locally only, reducing data exfiltration risk.",
            Tags = ["search", "cortana", "remote", "policy", "security"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Cortana remote queries; local processing only.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRemoteQuery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteQuery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRemoteQuery", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-opt-out-search-privacy",
            Label = "Opt Out of Connected Search Privacy Sharing via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ConnectedSearchPrivacy=3 (No history) under the Windows Search Group Policy path. "
                + "Configures the strictest privacy level for connected search, preventing "
                + "search query history from being stored or shared with Microsoft.",
            Tags = ["search", "privacy", "policy", "history"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents search query history from being stored or shared.",
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchPrivacy", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchPrivacy")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchPrivacy", 3)],
        },
        new TweakDef
        {
            Id = "wsepol-prevent-battery-indexing",
            Label = "Prevent Search Indexing on Battery via Policy",
            Category = "Windows Search Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexOnBattery=1 under the Windows Search Group Policy path. "
                + "Stops the Windows Search indexer from running when the device is on battery power. "
                + "Preserves battery life on laptops by deferring index updates until AC power is available.",
            Tags = ["search", "indexer", "battery", "policy", "performance"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Halts background indexing on battery; extends laptop battery life.",
            ApplyOps = [RegOp.SetDword(Key, "PreventIndexOnBattery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventIndexOnBattery")],
            DetectOps = [RegOp.CheckDword(Key, "PreventIndexOnBattery", 1)],
        },
    ];
}

/// <summary>
/// Sprint 653 — App privacy access via Group Policy (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy
/// Controls which app capabilities (camera, microphone, contacts, etc.)
/// apps may access on the machine. LetApps* = 2 forces access off for all apps.
/// </summary>
internal static class PolicyAppPrivacy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "apppriv-block-camera-access",
            Label = "Block App Camera Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessCamera=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents all apps from accessing the camera. "
                + "Reduces attack surface for rogue apps attempting to record video without user consent.",
            Tags = ["camera", "privacy", "policy", "app-access"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all app camera access; may break video conferencing apps.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessCamera", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessCamera")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessCamera", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-microphone-access",
            Label = "Block App Microphone Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessMicrophone=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents all apps from accessing the microphone. "
                + "Stops background audio capture by apps not whitelisted by the administrator.",
            Tags = ["microphone", "audio", "privacy", "policy", "app-access"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all app microphone access; may affect voice chat apps.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessMicrophone", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessMicrophone")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessMicrophone", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-contacts-access",
            Label = "Block App Contacts Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessContacts=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from reading the user contact list. "
                + "Mitigates data harvesting by apps that attempt to exfiltrate address book entries.",
            Tags = ["contacts", "privacy", "policy", "app-access"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all app access to contacts list.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessContacts", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessContacts")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessContacts", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-calendar-access",
            Label = "Block App Calendar Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessCalendar=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from reading or modifying the user calendar. "
                + "Protects meeting schedules and personal appointments from third-party app access.",
            Tags = ["calendar", "privacy", "policy", "app-access"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks all app calendar read/write access.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessCalendar", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessCalendar")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessCalendar", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-call-history-access",
            Label = "Block App Call History Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessCallHistory=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from reading call history records. "
                + "Protects communication history from being accessed by data broker apps.",
            Tags = ["call-history", "privacy", "policy", "app-access"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks all app access to call history records.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessCallHistory", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessCallHistory")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessCallHistory", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-messaging-access",
            Label = "Block App Messaging Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessMessaging=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from sending or reading SMS/MMS messages. "
                + "Stops apps from using the messaging capability for data exfiltration.",
            Tags = ["messaging", "sms", "privacy", "policy", "app-access"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks all app SMS/messaging access.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessMessaging", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessMessaging")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessMessaging", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-voice-activation",
            Label = "Block App Voice Activation via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsActivateWithVoice=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from being activated by voice commands. "
                + "Stops always-on microphone listening used for wake-word detection.",
            Tags = ["voice", "privacy", "policy", "app-access", "microphone"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables app wake-word/voice activation features.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsActivateWithVoice", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsActivateWithVoice")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsActivateWithVoice", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-account-info-access",
            Label = "Block App Account Info Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessAccountInfo=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from accessing account information such as the user's name, "
                + "display picture, or username. Reduces personal information exposure.",
            Tags = ["account", "privacy", "policy", "app-access", "identity"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks app access to account name, photo, and username.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessAccountInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessAccountInfo")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessAccountInfo", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-background-apps",
            Label = "Block Apps Running in Background via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsRunInBackground=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from running as background tasks while not in focus. "
                + "Reduces CPU and battery consumption from idle background app activity.",
            Tags = ["background", "apps", "policy", "performance", "battery"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Stops apps from running background tasks; saves CPU and battery.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsRunInBackground", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsRunInBackground")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsRunInBackground", 2)],
        },
        new TweakDef
        {
            Id = "apppriv-block-diagnostics-access",
            Label = "Block App Diagnostic Info Access via Policy",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsGetDiagnosticInfo=2 (Force Deny) under the AppPrivacy Group Policy path. "
                + "Prevents apps from reading diagnostic information about other running apps. "
                + "Blocks potential information disclosure via the diagnostics API.",
            Tags = ["diagnostics", "privacy", "policy", "app-access"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks app access to diagnostic info about other processes.",
            ApplyOps = [RegOp.SetDword(Key, "LetAppsGetDiagnosticInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsGetDiagnosticInfo")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsGetDiagnosticInfo", 2)],
        },
    ];
}

/// <summary>
/// Sprint 654 — Windows CloudContent / User Experience policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent
/// These Group Policy keys disable Microsoft promotional content,
/// Spotlight features, lock screen suggestions, and tailored experiences
/// that are delivered via cloud services.
/// </summary>
internal static class PolicyUserExperience
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "uxpol-disable-consumer-features",
            Label = "Disable Windows Consumer Experiences via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsConsumerFeatures=1 under the CloudContent Group Policy path. "
                + "Prevents Windows from automatically installing sponsored apps and showing "
                + "promotional content to users. Essential for enterprise deployments.",
            Tags = ["consumer", "bloatware", "policy", "enterprise"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Stops Windows from installing sponsored/suggested apps automatically.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-spotlight",
            Label = "Disable Windows Spotlight via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightFeatures=1 under the CloudContent Group Policy path. "
                + "Disables all Windows Spotlight features including dynamic lock screen images, "
                + "fun facts, Start suggestions, and action center tips from Microsoft.",
            Tags = ["spotlight", "lock-screen", "policy", "privacy"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables all Windows Spotlight cloud content features.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableThirdPartySuggestions=1 under the CloudContent Group Policy path. "
                + "Blocks third-party app recommendations from appearing in Spotlight, "
                + "the Start menu, and Settings suggestions.",
            Tags = ["suggestions", "third-party", "policy", "privacy"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes third-party app ads from Spotlight and Start menu.",
            ApplyOps = [RegOp.SetDword(Key, "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(Key, "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-lock-screen-app-notifications",
            Label = "Disable Lock Screen App Notifications via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLockScreenAppNotifications=1 under the CloudContent Group Policy path. "
                + "Prevents app notification banners and badges from displaying on the lock screen. "
                + "Reduces information exposure on unattended machines.",
            Tags = ["lock-screen", "notifications", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clears notification banners from the lock screen display.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenAppNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenAppNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenAppNotifications", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-welcome-experience",
            Label = "Disable Windows Welcome Experience via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1 under the CloudContent Group Policy path. "
                + "Hides the full-screen Windows welcome experience that displays after major updates "
                + "highlighting new features. Reduces sign-in friction.",
            Tags = ["welcome", "oobe", "policy", "spotlight"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents full-screen welcome/feature highlight page after updates.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsSpotlightWindowsWelcomeExperience")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-soft-landing",
            Label = "Disable Feature Tips and Soft-Landing via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSoftLanding=1 under the CloudContent Group Policy path. "
                + "Prevents Windows from showing feature introductory tips, fun facts, and tooltip "
                + "overlays that guide new users. Suitable for controlled enterprise environments.",
            Tags = ["tips", "soft-landing", "policy", "enterprise"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes introductory tips and feature hint overlays from Windows.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSoftLanding", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-tailored-diagnostic-experiences",
            Label = "Disable Tailored Experiences with Diagnostic Data via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableTailoredExperiencesWithDiagnosticData=1 under the CloudContent Group Policy path. "
                + "Prevents Windows from using diagnostic data to show personalized tips, ads, "
                + "and recommendations. Reduces the feedback loop between telemetry and targeted content.",
            Tags = ["diagnostic-data", "tailored", "policy", "privacy", "telemetry"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Stops diagnostic data from being used to personalise Windows content.",
            ApplyOps = [RegOp.SetDword(Key, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTailoredExperiencesWithDiagnosticData")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTailoredExperiencesWithDiagnosticData", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-spotlight-action-center",
            Label = "Disable Spotlight Suggestions in Action Center via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightOnActionCenter=1 under the CloudContent Group Policy path. "
                + "Prevents Microsoft Spotlight from injecting promotional or feature suggestions "
                + "into the Windows Action Center notification pane.",
            Tags = ["spotlight", "action-center", "policy", "notifications"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes Spotlight ads and suggestions from Action Center.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsSpotlightOnActionCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsSpotlightOnActionCenter")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsSpotlightOnActionCenter", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-spotlight-on-settings",
            Label = "Disable Spotlight in Settings App via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightOnSettings=1 under the CloudContent Group Policy path. "
                + "Stops Windows Spotlight from inserting promotional content and feature highlights "
                + "into the Settings app pages.",
            Tags = ["spotlight", "settings", "policy", "privacy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes Spotlight banners from within the Settings app.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsSpotlightOnSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsSpotlightOnSettings")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsSpotlightOnSettings", 1)],
        },
        new TweakDef
        {
            Id = "uxpol-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimized Content via Policy",
            Category = "User Experience Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableCloudOptimizedContent=1 under the CloudContent Group Policy path. "
                + "Prevents Windows from downloading and displaying cloud-optimized promotional "
                + "content, advertisements, and feature spotlights on the lock screen and Start menu.",
            Tags = ["cloud", "content", "policy", "privacy", "enterprise"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks cloud-sourced promotional content on lock screen and Start.",
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudOptimizedContent", 1)],
        },
    ];
}

/// <summary>
/// Sprint 655 — Windows Event Log sizing and access policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog\{Application|Security|System|Setup}
/// These Group Policy keys configure maximum event log sizes and guest access
/// restrictions for the four primary Windows event log channels.
/// </summary>
internal static class PolicyEventLogAudit
{
    private const string AppLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
    private const string SecLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
    private const string SysLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";
    private const string SetupLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Setup";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evtpol-app-log-max-size",
            Label = "Set Application Event Log to 64 MB via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxSize=65536 (64 MB in KB) under the Application EventLog Group Policy path. "
                + "Ensures the Application event log retains sufficient history for diagnostics "
                + "and compliance reviews. The value is in kilobytes.",
            Tags = ["event-log", "application", "policy", "audit", "sizing"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Grows Application log capacity to 64 MB for better event retention.",
            ApplyOps = [RegOp.SetDword(AppLog, "MaxSize", 65536)],
            RemoveOps = [RegOp.DeleteValue(AppLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(AppLog, "MaxSize", 65536)],
        },
        new TweakDef
        {
            Id = "evtpol-sec-log-max-size",
            Label = "Set Security Event Log to 192 MB via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxSize=196608 (192 MB in KB) under the Security EventLog Group Policy path. "
                + "Aligns with NIST SP 800-92 guidance for security audit log capacity. "
                + "Ensures logon and privilege events are retained for incident response.",
            Tags = ["event-log", "security", "policy", "audit", "nist", "sizing"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Grows Security log to 192 MB; NIST-recommended for audit retention.",
            ApplyOps = [RegOp.SetDword(SecLog, "MaxSize", 196608)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(SecLog, "MaxSize", 196608)],
        },
        new TweakDef
        {
            Id = "evtpol-sys-log-max-size",
            Label = "Set System Event Log to 64 MB via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxSize=65536 (64 MB in KB) under the System EventLog Group Policy path. "
                + "Provides adequate capacity for driver, service, and OS-level events "
                + "used in troubleshooting and change auditing.",
            Tags = ["event-log", "system", "policy", "audit", "sizing"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Grows System log to 64 MB for OS event history retention.",
            ApplyOps = [RegOp.SetDword(SysLog, "MaxSize", 65536)],
            RemoveOps = [RegOp.DeleteValue(SysLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(SysLog, "MaxSize", 65536)],
        },
        new TweakDef
        {
            Id = "evtpol-setup-log-max-size",
            Label = "Set Setup Event Log to 32 MB via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxSize=32768 (32 MB in KB) under the Setup EventLog Group Policy path. "
                + "Retains enough Windows update and component installation events "
                + "to support post-deployment validation and rollback analysis.",
            Tags = ["event-log", "setup", "policy", "audit", "sizing"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Grows Setup log to 32 MB for Windows Update event retention.",
            ApplyOps = [RegOp.SetDword(SetupLog, "MaxSize", 32768)],
            RemoveOps = [RegOp.DeleteValue(SetupLog, "MaxSize")],
            DetectOps = [RegOp.CheckDword(SetupLog, "MaxSize", 32768)],
        },
        new TweakDef
        {
            Id = "evtpol-app-log-retention",
            Label = "Set Application Log to Overwrite as Needed via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Retention=0 under the Application EventLog Group Policy path. "
                + "Configures the Application event log to overwrite old events when full "
                + "rather than stopping logging or requiring manual clearance.",
            Tags = ["event-log", "application", "policy", "audit", "retention"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Application log overwrites oldest events when full; no logging interruption.",
            ApplyOps = [RegOp.SetDword(AppLog, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(AppLog, "Retention")],
            DetectOps = [RegOp.CheckDword(AppLog, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtpol-sec-log-retention",
            Label = "Set Security Log to Overwrite as Needed via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Retention=0 under the Security EventLog Group Policy path. "
                + "Configures the Security event log to overwrite old events when full. "
                + "Prevents audit failure conditions caused by a full security log.",
            Tags = ["event-log", "security", "policy", "audit", "retention"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Security log overwrites when full; prevents audit-failure shutdown.",
            ApplyOps = [RegOp.SetDword(SecLog, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "Retention")],
            DetectOps = [RegOp.CheckDword(SecLog, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtpol-sys-log-retention",
            Label = "Set System Log to Overwrite as Needed via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Retention=0 under the System EventLog Group Policy path. "
                + "Configures the System event log to overwrite old entries when full, "
                + "maintaining continuous logging of OS and driver events.",
            Tags = ["event-log", "system", "policy", "audit", "retention"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "System log overwrites when full; continuous OS event capture.",
            ApplyOps = [RegOp.SetDword(SysLog, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(SysLog, "Retention")],
            DetectOps = [RegOp.CheckDword(SysLog, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtpol-app-restrict-guest-access",
            Label = "Restrict Guest Access to Application Log via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictGuestAccess=1 under the Application EventLog Group Policy path. "
                + "Prevents guest accounts from reading Application event log entries. "
                + "Reduces information disclosure to unauthenticated or low-privilege sessions.",
            Tags = ["event-log", "application", "policy", "security", "access-control"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Denies guest accounts from reading the Application log.",
            ApplyOps = [RegOp.SetDword(AppLog, "RestrictGuestAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(AppLog, "RestrictGuestAccess")],
            DetectOps = [RegOp.CheckDword(AppLog, "RestrictGuestAccess", 1)],
        },
        new TweakDef
        {
            Id = "evtpol-sec-restrict-guest-access",
            Label = "Restrict Guest Access to Security Log via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictGuestAccess=1 under the Security EventLog Group Policy path. "
                + "Prevents guest accounts from viewing security audit records. "
                + "Protects logon and privilege audit trails from low-privilege access.",
            Tags = ["event-log", "security", "policy", "security", "access-control"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Denies guest accounts from reading security audit events.",
            ApplyOps = [RegOp.SetDword(SecLog, "RestrictGuestAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(SecLog, "RestrictGuestAccess")],
            DetectOps = [RegOp.CheckDword(SecLog, "RestrictGuestAccess", 1)],
        },
        new TweakDef
        {
            Id = "evtpol-sys-restrict-guest-access",
            Label = "Restrict Guest Access to System Log via Policy",
            Category = "Event Log Audit",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictGuestAccess=1 under the System EventLog Group Policy path. "
                + "Prevents guest accounts from viewing system-level events including "
                + "driver loads, service starts, and hardware errors.",
            Tags = ["event-log", "system", "policy", "security", "access-control"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Denies guest accounts from reading the System event log.",
            ApplyOps = [RegOp.SetDword(SysLog, "RestrictGuestAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(SysLog, "RestrictGuestAccess")],
            DetectOps = [RegOp.CheckDword(SysLog, "RestrictGuestAccess", 1)],
        },
    ];
}

/// <summary>
/// Sprint 656 — Windows Settings Sync policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SettingSync
/// Disables roaming sync of various Windows settings categories via
/// Group Policy. Suitable for environments where settings should remain
/// machine-local and not roam through Microsoft accounts or Azure AD.
/// </summary>
internal static class PolicySyncSettings
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "syncpol-disable-all-sync",
            Label = "Disable All Settings Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSettingSync=2 under the SettingSync Group Policy path. "
                + "Turns off roaming sync for all Windows settings categories. "
                + "Prevents user settings from being uploaded to or downloaded from Microsoft Account/Azure AD.",
            Tags = ["sync", "roaming", "policy", "privacy", "enterprise"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All Windows settings stay machine-local; no cloud sync roaming.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-prevent-user-sync-override",
            Label = "Prevent User from Overriding Sync Settings via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSettingSyncUserOverride=1 under the SettingSync Group Policy path. "
                + "Locks sync policy settings so users cannot re-enable syncing in the "
                + "Settings app even if they have a linked Microsoft Account.",
            Tags = ["sync", "policy", "enterprise", "lock"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents users from enabling sync in Settings when blocked by policy.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSettingSyncUserOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSettingSyncUserOverride")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSettingSyncUserOverride", 1)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-credentials-sync",
            Label = "Disable Credentials/Password Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableCredentialsSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents roaming sync of saved passwords, Wi-Fi credentials, and stored account "
                + "tokens. Keeps credentials machine-local and reduces credential exposure.",
            Tags = ["sync", "credentials", "passwords", "policy", "security"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Prevents cloud sync of saved passwords and Wi-Fi credentials.",
            ApplyOps = [RegOp.SetDword(Key, "DisableCredentialsSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCredentialsSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCredentialsSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-personalization-sync",
            Label = "Disable Personalization Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePersonalizationSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents roaming sync of desktop wallpaper, colors, and lock screen images. "
                + "Keeps machine appearance independent of the user's Microsoft Account profile.",
            Tags = ["sync", "personalization", "wallpaper", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Desktop wallpaper and colors stay machine-specific; no cloud roaming.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePersonalizationSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalizationSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePersonalizationSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-app-settings-sync",
            Label = "Disable App Settings Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableApplicationSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents roaming sync of per-app settings for Microsoft Store apps. "
                + "App data stays local and is not replicated to other devices via the cloud.",
            Tags = ["sync", "apps", "store", "policy"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Store app settings remain local; no cross-device app state syncing.",
            ApplyOps = [RegOp.SetDword(Key, "DisableApplicationSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableApplicationSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableApplicationSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-start-layout-sync",
            Label = "Disable Start Layout Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableStartLayoutSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents the Start menu tile layout from syncing across devices. "
                + "Useful when deploying customised Start layouts that must not be overwritten.",
            Tags = ["sync", "start-menu", "layout", "policy", "enterprise"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Start menu layout stays machine-specific; no cloud override.",
            ApplyOps = [RegOp.SetDword(Key, "DisableStartLayoutSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStartLayoutSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStartLayoutSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-theme-sync",
            Label = "Disable Theme Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableThemeSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents Windows theme (colors, sounds, cursors) from roaming to other devices. "
                + "Maintains machine-specific branding or standardised appearance.",
            Tags = ["sync", "themes", "colors", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows theme remains local; corporate theme cannot be overridden by sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisableThemeSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableThemeSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableThemeSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-language-sync",
            Label = "Disable Language Preferences Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLanguageSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents roaming sync of language settings, regional formats, and keyboard layouts. "
                + "Ensures locale settings are set per machine or via Group Policy, not user preference.",
            Tags = ["sync", "language", "locale", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Language and keyboard layout stays machine-local; no cross-device sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLanguageSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLanguageSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLanguageSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-accessibility-sync",
            Label = "Disable Accessibility Settings Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableAccessibilitySettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents roaming sync of accessibility settings such as high contrast, "
                + "Narrator preferences, and Magnifier state.",
            Tags = ["sync", "accessibility", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Accessibility settings stay machine-local; no cross-device sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAccessibilitySettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAccessibilitySettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAccessibilitySettingSync", 2)],
        },
        new TweakDef
        {
            Id = "syncpol-disable-desktop-theme-sync",
            Label = "Disable Desktop Theme Sync via Policy",
            Category = "Settings Sync Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableDesktopThemeSettingSync=2 under the SettingSync Group Policy path. "
                + "Prevents desktop-specific theme settings (background slideshow, accent colour) "
                + "from being synced independently of the main theme sync setting.",
            Tags = ["sync", "desktop", "theme", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Desktop-specific theme settings stay local; no cloud background sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDesktopThemeSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDesktopThemeSettingSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDesktopThemeSettingSync", 2)],
        },
    ];
}
