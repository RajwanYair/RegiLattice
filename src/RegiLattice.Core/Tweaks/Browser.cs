namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Cross-browser performance, privacy, and policy tweaks — applies common settings across
/// all browsers via system-wide policies for DNS-over-HTTPS, preloading, tracking prevention,
/// and cache management.
/// </summary>
[TweakModule]
internal static class Browser
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string CuKey = @"HKEY_CURRENT_USER";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "browser-disable-dns-prefetch",
            Label = "Disable DNS Prefetching (All Browsers)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DNS link prefetching across Chrome, Edge, and Firefox via policy. Prevents background DNS lookups that can leak browsing intent.",
            Tags = ["browser", "privacy", "dns", "prefetch"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DnsOverHttpsMode", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DnsOverHttpsMode"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-background-network",
            Label = "Disable Background Networking (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome and Edge from running background network tasks when the browser appears closed.",
            Tags = ["browser", "privacy", "performance", "background"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-prediction-service",
            Label = "Disable Network Prediction Service (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the network prediction / preconnect service that pre-loads pages and resources you might visit next. Reduces bandwidth and privacy leak.",
            Tags = ["browser", "privacy", "prediction", "preload"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions", 2),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "NetworkPredictionOptions", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "NetworkPredictionOptions"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions", 2)],
        },
        new TweakDef
        {
            Id = "browser-disable-metrics-reporting",
            Label = "Disable Usage Metrics Reporting (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables anonymous usage statistics and crash reporting for both Chrome and Edge.",
            Tags = ["browser", "privacy", "telemetry", "metrics"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "MetricsReportingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "MetricsReportingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-autofill-cc",
            Label = "Disable Credit Card AutoFill (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables credit card autofill across Chrome and Edge via policy. Protects payment information.",
            Tags = ["browser", "privacy", "security", "autofill"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "AutofillCreditCardEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "AutofillCreditCardEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "AutofillCreditCardEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-password-manager",
            Label = "Disable Built-in Password Manager (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in password manager in Chrome and Edge. Use a dedicated password manager like Bitwarden instead.",
            Tags = ["browser", "security", "passwords", "autofill"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-send-do-not-track",
            Label = "Enable Do Not Track Header (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Chrome and Edge to send the Do Not Track (DNT) HTTP header with every request.",
            Tags = ["browser", "privacy", "tracking", "dnt"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "EnableDoNotTrack", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DoNotTrack", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "EnableDoNotTrack"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DoNotTrack"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "EnableDoNotTrack", 1)],
        },
        new TweakDef
        {
            Id = "browser-block-third-party-cookies",
            Label = "Block Third-Party Cookies (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party cookies in Chrome and Edge via group policy. Prevents cross-site tracking.",
            Tags = ["browser", "privacy", "cookies", "tracking"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BlockThirdPartyCookies", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BlockThirdPartyCookies"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BlockThirdPartyCookies", 1)],
        },
        new TweakDef
        {
            Id = "browser-disable-safe-browsing-telemetry",
            Label = "Disable Safe Browsing Extended Reporting (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extended safe browsing reporting which sends URLs and page content to Google/Microsoft for analysis.",
            Tags = ["browser", "privacy", "safe-browsing", "telemetry"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingExtendedReportingEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SmartScreenEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingExtendedReportingEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SmartScreenEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingExtendedReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-translate",
            Label = "Disable Built-in Translation (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in page translation feature in Chrome and Edge via policy.",
            Tags = ["browser", "privacy", "translate", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "TranslateEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "TranslateEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "TranslateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-spell-check",
            Label = "Disable Spell Check (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the browser spell checking service that sends typed text to external servers.",
            Tags = ["browser", "privacy", "spell-check", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SpellcheckEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SpellcheckEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SpellcheckEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SpellcheckEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SpellcheckEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-search-suggestions",
            Label = "Disable Search Suggestions (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables real-time search suggestions that send each keystroke to the search engine.",
            Tags = ["browser", "privacy", "search", "suggestions"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SearchSuggestEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SearchSuggestEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-sync",
            Label = "Disable Browser Sync (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables profile sync across devices in Chrome and Edge via policy.",
            Tags = ["browser", "privacy", "sync", "cloud"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SyncDisabled", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SyncDisabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "browser-disable-browser-sign-in",
            Label = "Disable Forced Browser Sign-In (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome and Edge from requiring or prompting for browser sign-in.",
            Tags = ["browser", "privacy", "sign-in", "account"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BrowserSignin", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BrowserSignin"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BrowserSignin", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-media-router",
            Label = "Disable Media Router / Cast (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Cast/media router icon and functionality in Chrome and Edge.",
            Tags = ["browser", "privacy", "cast", "media", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "EnableMediaRouter", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EnableMediaRouter", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "EnableMediaRouter"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EnableMediaRouter"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "EnableMediaRouter", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-form-fill",
            Label = "Disable Form AutoFill (Chrome/Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables form data autofill suggestions across Chrome and Edge.",
            Tags = ["browser", "privacy", "autofill", "forms"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ImportAutofillFormData", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ImportAutofillFormData", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ImportAutofillFormData"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ImportAutofillFormData"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ImportAutofillFormData", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-cast-icon",
            Label = "Hide Cast Icon in Browser Toolbar",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Google Cast icon from the Chrome and Edge toolbars.",
            Tags = ["browser", "chrome", "edge", "cast", "toolbar", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ShowCastIconInToolbar", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ShowCastIconInToolbar", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ShowCastIconInToolbar"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ShowCastIconInToolbar"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ShowCastIconInToolbar", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-signin-interception",
            Label = "Disable Browser Sign-In Interception (Chrome)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the Chrome prompt to sign into a Google Account when a new browser profile is detected.",
            Tags = ["browser", "chrome", "signin", "account", "popup", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SigninInterceptionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SigninInterceptionEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SigninInterceptionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-edge-shopping-assistant",
            Label = "Disable Edge Shopping Assistant",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge shopping assistant that shows price comparisons on e-commerce websites.",
            Tags = ["browser", "edge", "shopping", "advertising", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-edge-follow",
            Label = "Disable Edge Follow Feature",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge Follow feature that lets users subscribe to creators and topics in the sidebar.",
            Tags = ["browser", "edge", "follow", "social", "sidebar"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-ntp-custom-background",
            Label = "Disable Custom New Tab Page Background (Chrome)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from setting a custom background image on the Chrome new tab page.",
            Tags = ["browser", "chrome", "ntp", "new-tab", "background", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "NTPCustomBackgroundEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "NTPCustomBackgroundEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "NTPCustomBackgroundEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-promotional-tabs",
            Label = "Disable Promotional New Tabs (Chrome)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome from opening promotional content tabs after install or browser updates.",
            Tags = ["browser", "chrome", "ntp", "promotional", "advertising"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-ntp-spotlight-recommendations",
            Label = "Disable Chrome NTP Spotlight Recommendations",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes personalized article recommendations and spotlight content from the Chrome new tab page.",
            Tags = ["browser", "chrome", "ntp", "spotlight", "recommendations", "advertising"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SpotlightExperiencesAndRecommendationsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SpotlightExperiencesAndRecommendationsEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SpotlightExperiencesAndRecommendationsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-first-run-experience",
            Label = "Disable Browser First-Run Experience",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Skips the welcome/first-run setup wizard on the first launch of Chrome and Edge.",
            Tags = ["browser", "chrome", "edge", "first-run", "setup", "ui"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SuppressFirstRunDefaultBrowserPrompt", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "SuppressFirstRunDefaultBrowserPrompt"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1)],
        },
        new TweakDef
        {
            Id = "browser-disable-autofill-address",
            Label = "Disable Address Autofill (All Browsers)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables address and shipping information autofill in Chrome and Edge.",
            Tags = ["browser", "chrome", "edge", "autofill", "privacy", "addresses"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "AutofillAddressEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "AutofillAddressEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-edge-prelaunch",
            Label = "Disable Edge Pre-Launch at Startup",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Edge from pre-launching silently at Windows startup to improve boot time.",
            Tags = ["browser", "edge", "prelaunch", "startup", "performance", "boot"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-geolocation",
            Label = "Block Browser Geolocation Access (Chrome & Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultGeolocationSetting=2 to block all websites from detecting "
                + "the user's physical location via the browser. Applies to Chrome and Edge.",
            Tags = ["browser", "privacy", "geolocation", "location", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultGeolocationSetting", 2),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultGeolocationSetting", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultGeolocationSetting"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultGeolocationSetting"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultGeolocationSetting", 2)],
        },
        new TweakDef
        {
            Id = "browser-disable-notifications",
            Label = "Block Browser Push Notifications (Chrome & Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultNotificationsSetting=2 to block all websites from sending " + "browser push notifications. Applies to Chrome and Edge.",
            Tags = ["browser", "privacy", "notifications", "push", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultNotificationsSetting", 2),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultNotificationsSetting", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultNotificationsSetting"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultNotificationsSetting"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultNotificationsSetting", 2)],
        },
        new TweakDef
        {
            Id = "browser-disable-webusb",
            Label = "Block WebUSB API Access (Chrome & Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultWebUsbGuardSetting=2 to prevent websites from accessing "
                + "USB devices through the browser's WebUSB API. Reduces hardware attack surface.",
            Tags = ["browser", "security", "webusb", "usb", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultWebUsbGuardSetting", 2),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebUsbGuardSetting", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultWebUsbGuardSetting"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebUsbGuardSetting"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultWebUsbGuardSetting", 2)],
        },
        new TweakDef
        {
            Id = "browser-disable-web-bluetooth",
            Label = "Block Web Bluetooth API Access (Chrome & Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultWebBluetoothGuardSetting=2 to block websites from accessing "
                + "Bluetooth devices via the Web Bluetooth API. Reduces wireless attack surface.",
            Tags = ["browser", "security", "bluetooth", "web bluetooth", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultWebBluetoothGuardSetting", 2),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebBluetoothGuardSetting", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultWebBluetoothGuardSetting"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebBluetoothGuardSetting"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "DefaultWebBluetoothGuardSetting", 2)],
        },
        new TweakDef
        {
            Id = "browser-disable-builtin-dns-client",
            Label = "Disable Browser Built-In DNS Client (Chrome)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Chrome's built-in DNS resolver, forcing it to use the OS DNS "
                + "stack instead. Ensures DNS queries respect system-level hosts file "
                + "and DNS settings (BuiltInDnsClientEnabled=0).",
            Tags = ["browser", "chrome", "dns", "privacy", "network", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BuiltInDnsClientEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BuiltInDnsClientEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "BuiltInDnsClientEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-url-keyed-metrics",
            Label = "Disable URL-Keyed Anonymized Metrics (Chrome & Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the collection of statistics tied to specific URLs visited, "
                + "preventing per-URL telemetry being sent to Google/Microsoft "
                + "(UrlKeyedAnonymizedDataCollectionEnabled=0).",
            Tags = ["browser", "privacy", "telemetry", "metrics", "url", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "UrlKeyedAnonymizedDataCollectionEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "UrlKeyedAnonymizedDataCollectionEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "UrlKeyedAnonymizedDataCollectionEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "UrlKeyedAnonymizedDataCollectionEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "UrlKeyedAnonymizedDataCollectionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-webrtc-event-logs",
            Label = "Disable WebRTC Event Log Collection (Chrome & Edge)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the browser from collecting WebRTC event logs that can expose "
                + "call and connection diagnostic data to web applications "
                + "(WebRtcEventLogCollectionAllowed=0).",
            Tags = ["browser", "privacy", "webrtc", "logging", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "WebRtcEventLogCollectionAllowed", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "WebRtcEventLogCollectionAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "WebRtcEventLogCollectionAllowed"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "WebRtcEventLogCollectionAllowed"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "WebRtcEventLogCollectionAllowed", 0)],
        },
        new TweakDef
        {
            Id = "browser-force-safe-search",
            Label = "Force Google SafeSearch (Chrome)",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces SafeSearch to be enabled on Google Search results in Chrome "
                + "via enterprise policy. Useful in managed or family environments "
                + "(ForceGoogleSafeSearch=1 or SafeSearchEnabled=1).",
            Tags = ["browser", "chrome", "safesearch", "content-filter", "policy", "family"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ForceGoogleSafeSearch", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ForceGoogleSafeSearch")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "ForceGoogleSafeSearch", 1)],
        },
        new TweakDef
        {
            Id = "browser-disable-edge-read-aloud",
            Label = "Disable Edge Read Aloud Feature",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Edge Read Aloud feature that reads web page content aloud "
                + "using the device's text-to-speech engine. Reduces unused feature overhead.",
            Tags = ["browser", "edge", "read aloud", "tts", "accessibility"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ReadAloudEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ReadAloudEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ReadAloudEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-edge-wallet",
            Label = "Disable Microsoft Edge Wallet",
            Category = "Browser 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Microsoft Edge Wallet feature (also called 'Microsoft Wallet') "
                + "that promotes payment info, coupons and loyalty cards in the browser sidebar.",
            Tags = ["browser", "edge", "wallet", "payments", "privacy", "advertising"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "WalletDonationEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "WalletDonationEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "WalletDonationEnabled", 0)],
        },
    ];
}
