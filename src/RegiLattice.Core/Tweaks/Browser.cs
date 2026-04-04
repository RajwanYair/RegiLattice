namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Cross-browser performance, privacy, and policy tweaks — applies common settings across
/// all browsers via system-wide policies for DNS-over-HTTPS, preloading, tracking prevention,
/// and cache management.
/// </summary>
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Id = "browser-disable-autofill-addresses",
            Label = "Disable Address AutoFill (Chrome/Edge)",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables address autofill across Chrome and Edge via policy. Protects personal address information.",
            Tags = ["browser", "privacy", "security", "autofill"],
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
            Id = "browser-disable-password-manager",
            Label = "Disable Built-in Password Manager (Chrome/Edge)",
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Id = "browser-disable-shopping-features",
            Label = "Disable Shopping Assistant (Edge)",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge's shopping assistant, price comparison, and coupon features.",
            Tags = ["browser", "privacy", "shopping", "edge"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ConfigureDoNotTrack", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "ConfigureDoNotTrack"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-preloading",
            Label = "Disable Tab Preloading (Chrome/Edge)",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables preloading of new tab and startup pages to save memory and network bandwidth.",
            Tags = ["browser", "performance", "memory", "preload"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Google\Chrome", $@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "TabPreloadingEnabled", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "TabPreloadingEnabled"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Google\Chrome", "TabPreloadingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "browser-disable-form-fill",
            Label = "Disable Form AutoFill (Chrome/Edge)",
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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
            Category = "Browser",
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

// ── merged from Chrome.cs ──
internal static class Chrome
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "chrome-disable-renderer-code-integrity",
            Label = "Disable Chrome Renderer Code Integrity",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables renderer code integrity checks in Chrome. Fixes compatibility issues with certain security software. Default: enabled.",
            Tags = ["chrome", "renderer", "code-integrity", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "RendererCodeIntegrityEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "RendererCodeIntegrityEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "RendererCodeIntegrityEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-preloading",
            Label = "Disable Chrome Page Preloading",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome from preloading pages it predicts you might visit. Saves bandwidth. Default: enabled.",
            Tags = ["chrome", "preloading", "bandwidth", "prediction"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions", 2)],
        },
        new TweakDef
        {
            Id = "chrome-disable-autofill-addresses",
            Label = "Disable Chrome Address Autofill",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome's autofill suggestions for addresses. Prevents address data storage. Default: enabled.",
            Tags = ["chrome", "autofill", "addresses", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-search-suggestions",
            Label = "Disable Chrome Search Suggestions",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables search and URL suggestions in the Chrome address bar. Prevents keystrokes from being sent to Google. Default: enabled.",
            Tags = ["chrome", "search", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-background-mode",
            Label = "Disable Chrome Background Mode",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome from running in background after all windows are closed. Frees system resources. Default: enabled.",
            Tags = ["chrome", "background", "mode", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-metrics-reporting",
            Label = "Disable Chrome Metrics Reporting",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome usage and crash-related data reporting to Google. Default: enabled.",
            Tags = ["chrome", "metrics", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-safe-browsing-extended",
            Label = "Disable Chrome Enhanced Safe Browsing",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables enhanced Safe Browsing which sends URLs to Google in real-time. Standard protection remains active. Default: standard.",
            Tags = ["chrome", "safe-browsing", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingProtectionLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingProtectionLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingProtectionLevel", 1)],
        },
        new TweakDef
        {
            Id = "chrome-disable-translate",
            Label = "Disable Chrome Built-in Translation",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in page translation prompt in Chrome. Default: enabled.",
            Tags = ["chrome", "translate", "language", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "TranslateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "TranslateEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "TranslateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-autofill-credit-cards",
            Label = "Disable Chrome Credit Card Autofill",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome from saving and auto-filling credit card information. Default: enabled.",
            Tags = ["chrome", "autofill", "credit-card", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillCreditCardEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillCreditCardEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillCreditCardEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-password-manager",
            Label = "Disable Chrome Built-in Password Manager",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome's built-in password manager. Use a dedicated password manager instead. Default: enabled.",
            Tags = ["chrome", "password", "manager", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-spell-check-service",
            Label = "Disable Chrome Spell Check Service",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome's online spell check service that sends text to Google. Local spell check still works. Default: enabled.",
            Tags = ["chrome", "spell-check", "privacy", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SpellCheckServiceEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SpellCheckServiceEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SpellCheckServiceEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-block-third-party-cookies",
            Label = "Block Third-Party Cookies in Chrome",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party cookies in Chrome to prevent cross-site tracking. Default: allowed.",
            Tags = ["chrome", "cookies", "tracking", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BlockThirdPartyCookies", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BlockThirdPartyCookies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BlockThirdPartyCookies", 1)],
        },
        new TweakDef
        {
            Id = "chrome-disable-webrtc-leak",
            Label = "Restrict Chrome WebRTC IP Leaking",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts WebRTC from exposing local IP addresses. Enhances VPN privacy. Default: unrestricted.",
            Tags = ["chrome", "webrtc", "ip", "leak", "vpn", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "WebRtcIPHandling", "disable_non_proxied_udp")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "WebRtcIPHandling")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "WebRtcIPHandling", "disable_non_proxied_udp")],
        },
        new TweakDef
        {
            Id = "chrome-disable-signin-promo",
            Label = "Disable Chrome Sign-In Promotion",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses Chrome sign-in prompts and promotional popups. Default: shown.",
            Tags = ["chrome", "sign-in", "promotion", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-force-dns-over-https",
            Label = "Enable Chrome DNS-over-HTTPS",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNS-over-HTTPS in Chrome with Cloudflare resolver. Encrypts DNS queries for privacy. Default: disabled.",
            Tags = ["chrome", "dns", "doh", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode", "automatic"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome",
                    "DnsOverHttpsTemplates",
                    "https://cloudflare-dns.com/dns-query"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsTemplates"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode", "automatic")],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-update",
            Label = "Disable Chrome Auto-Update",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Chrome from auto-updating via Google Update policy. Useful for version-pinned environments.",
            Tags = ["chrome", "browser", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update", "UpdateDefault", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update", "AutoUpdateCheckPeriodMinutes", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update", "UpdateDefault"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update", "AutoUpdateCheckPeriodMinutes"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update", "UpdateDefault", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-telemetry",
            Label = "Disable Chrome Telemetry",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Chrome usage statistics and crash reporting via policy.",
            Tags = ["chrome", "browser", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeviceMetricsReportingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeviceMetricsReportingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-signin",
            Label = "Disable Chrome Sign-In",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome browser sign-in with Google account via policy.",
            Tags = ["chrome", "browser", "signin", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BrowserSignin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BrowserSignin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BrowserSignin", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-hwaccel",
            Label = "Disable Chrome Hardware Acceleration",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables hardware acceleration in Chrome via policy. Reduces GPU usage at the cost of rendering performance.",
            Tags = ["chrome", "browser", "gpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "HardwareAccelerationModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "HardwareAccelerationModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "HardwareAccelerationModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-background",
            Label = "Disable Chrome Background Apps",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome background apps from running when Chrome is closed. Saves memory and CPU.",
            Tags = ["chrome", "browser", "background", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundProcessingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundProcessingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundProcessingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-signin",
            Label = "Disable Chrome Browser Sign-In Prompt",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Chrome sign-in prompt via policy. Prevents nagging to sign into Google.",
            Tags = ["chrome", "browser", "signin", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PromotionalTabsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-sync",
            Label = "Disable Chrome Sync",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome data synchronization across devices via policy.",
            Tags = ["chrome", "browser", "sync", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "chrome-disable-autofill-passwords",
            Label = "Disable Chrome Password Autofill",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome built-in password manager and autofill.",
            Tags = ["chrome", "browser", "password", "autofill"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-reporter",
            Label = "Disable Chrome Software Reporter",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome Software Reporter Tool (CleanUp tool) that scans for unwanted software.",
            Tags = ["chrome", "browser", "reporter", "privacy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ChromeCleanupEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ChromeCleanupEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ChromeCleanupEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-leak-detection",
            Label = "Disable Chrome Password Leak Detection",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome password leak detection that checks passwords against breach databases.",
            Tags = ["chrome", "browser", "leak", "password", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordLeakDetectionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordLeakDetectionEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "PasswordLeakDetectionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-default-browser-check",
            Label = "Disable Chrome Default Browser Check",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome from checking/prompting to be the default browser.",
            Tags = ["chrome", "browser", "default", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultBrowserSettingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultBrowserSettingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultBrowserSettingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-media-recommendations",
            Label = "Disable Chrome Media Recommendations",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome media recommendations on the new tab page.",
            Tags = ["chrome", "browser", "media", "recommendations"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MediaRecommendationsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MediaRecommendationsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MediaRecommendationsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-hardware-accel-policy",
            Label = "Disable Chrome GPU Acceleration Policy",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome GPU hardware acceleration at policy level.",
            Tags = ["chrome", "browser", "gpu", "acceleration", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "HardwareAccelerationModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "HardwareAccelerationModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "HardwareAccelerationModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-enforce-3p-cookie-block",
            Label = "Enforce Third-Party Cookie Block in Chrome",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Chrome default cookie setting to block third-party cookies (value 1=allow all, 2=block 3p, 4=block all).",
            Tags = ["chrome", "browser", "cookies", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultCookiesSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultCookiesSetting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultCookiesSetting", 2)],
        },
        new TweakDef
        {
            Id = "chrome-secure-dns",
            Label = "Enable Chrome Secure DNS",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces Chrome DNS-over-HTTPS in secure mode with Cloudflare as provider.",
            Tags = ["chrome", "browser", "dns", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode", "secure"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome",
                    "DnsOverHttpsTemplates",
                    "https://cloudflare-dns.com/dns-query"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsTemplates"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DnsOverHttpsMode", "secure")],
        },
        new TweakDef
        {
            Id = "chrome-background-run-off",
            Label = "Disable Chrome background running after close",
            Category = "Browser",
            Tags = ["chrome", "background", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-notifications-block-default",
            Label = "Block website notifications by default in Chrome",
            Category = "Browser",
            Tags = ["chrome", "notifications", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultNotificationsSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultNotificationsSetting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultNotificationsSetting", 2)],
        },
        new TweakDef
        {
            Id = "chrome-media-autoplay-off",
            Label = "Disable media autoplay in Chrome",
            Category = "Browser",
            Tags = ["chrome", "autoplay", "media", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutoplayAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutoplayAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutoplayAllowed", 0)],
        },
        new TweakDef
        {
            Id = "chrome-pdf-external",
            Label = "Open PDFs in external app instead of Chrome",
            Category = "Browser",
            Tags = ["chrome", "pdf", "viewer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AlwaysOpenPdfExternally", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AlwaysOpenPdfExternally")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AlwaysOpenPdfExternally", 1)],
        },
        new TweakDef
        {
            Id = "chrome-safe-browsing-enhanced",
            Label = "Enable Chrome Enhanced Safe Browsing",
            Category = "Browser",
            Tags = ["chrome", "safe-browsing", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingProtectionLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingProtectionLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SafeBrowsingProtectionLevel", 2)],
        },
        new TweakDef
        {
            Id = "chrome-cloud-reporting-off",
            Label = "Disable Chrome cloud reporting",
            Category = "Browser",
            Tags = ["chrome", "cloud", "reporting", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "CloudReportingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "CloudReportingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "CloudReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-incognito-mode-off",
            Label = "Disable Chrome incognito mode",
            Category = "Browser",
            Tags = ["chrome", "incognito", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "IncognitoModeAvailability", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "IncognitoModeAvailability")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "IncognitoModeAvailability", 1)],
        },
        new TweakDef
        {
            Id = "chrome-dev-tools-off",
            Label = "Disallow Chrome developer tools access",
            Category = "Browser",
            Tags = ["chrome", "devtools", "developer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeveloperToolsAvailability", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeveloperToolsAvailability")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeveloperToolsAvailability", 2)],
        },
        new TweakDef
        {
            Id = "chrome-cast-off",
            Label = "Disable Chrome Cast / media router",
            Category = "Browser",
            Tags = ["chrome", "cast", "media-router", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "EnableMediaRouter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "EnableMediaRouter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "EnableMediaRouter", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-save-history",
            Label = "Disable Chrome browsing history saving",
            Category = "Browser",
            Tags = ["chrome", "history", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SavingBrowserHistoryDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SavingBrowserHistoryDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SavingBrowserHistoryDisabled", 1)],
        },
        new TweakDef
        {
            Id = "chrome-block-geolocation",
            Label = "Block geolocation access in Chrome by default",
            Category = "Browser",
            Tags = ["chrome", "geolocation", "location", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultGeolocationSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultGeolocationSetting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DefaultGeolocationSetting", 2)],
        },
        new TweakDef
        {
            Id = "chrome-block-camera",
            Label = "Deny camera access in Chrome",
            Category = "Browser",
            Tags = ["chrome", "camera", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "VideoCaptureAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "VideoCaptureAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "VideoCaptureAllowed", 0)],
        },
        new TweakDef
        {
            Id = "chrome-block-microphone",
            Label = "Deny microphone access in Chrome",
            Category = "Browser",
            Tags = ["chrome", "microphone", "audio", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AudioCaptureAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AudioCaptureAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AudioCaptureAllowed", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-ads-gpo",
            Label = "Disable Chrome advertising features",
            Category = "Browser",
            Tags = ["chrome", "ads", "advertising", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AdsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AdsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AdsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-feedback-off",
            Label = "Disable Chrome user feedback / issue reporting",
            Category = "Browser",
            Tags = ["chrome", "feedback", "reporting", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "UserFeedbackAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "UserFeedbackAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "UserFeedbackAllowed", 0)],
        },
        new TweakDef
        {
            Id = "chrome-force-safe-search",
            Label = "Force Google SafeSearch in Chrome",
            Category = "Browser",
            Tags = ["chrome", "safesearch", "google", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ForceGoogleSafeSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ForceGoogleSafeSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ForceGoogleSafeSearch", 1)],
        },
    ];
}

// ── merged from Firefox.cs ──
internal static class Firefox
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "firefox-dns-over-https",
            Label = "Enable DNS-over-HTTPS",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNS-over-HTTPS in Firefox using the Cloudflare resolver.",
            Tags = ["firefox", "browser", "dns", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled", 1),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS",
                    "ProviderURL",
                    "https://mozilla.cloudflare-dns.com/dns-query"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "ProviderURL"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-form-history",
            Label = "Disable Firefox Form History",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Firefox form auto-fill history via enterprise policy. Prevents saving of form data and search history. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["firefox", "form", "history", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFormHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFormHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFormHistory", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-profile-import",
            Label = "Disable Firefox Profile Import",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the profile import wizard in Firefox. Prevents importing data from other browsers. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["firefox", "import", "profile", "managed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableProfileImport", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableProfileImport")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableProfileImport", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-content-analysis",
            Label = "Disable Firefox Content Analysis",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox content analysis features that scan downloads and form data. Default: enabled.",
            Tags = ["firefox", "content", "analysis", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ContentAnalysisEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ContentAnalysisEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ContentAnalysisEnabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-normandy",
            Label = "Disable Firefox Normandy/Shield",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Mozilla Normandy/Shield system used for remote experiments and preference rollouts. Default: enabled.",
            Tags = ["firefox", "normandy", "shield", "experiments"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "app.normandy.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "app.normandy.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "app.normandy.enabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-beacon-api",
            Label = "Disable Firefox Beacon API",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Beacon API in Firefox. Prevents sites from sending analytics data asynchronously on page unload. Default: enabled.",
            Tags = ["firefox", "beacon", "api", "tracking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "beacon.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "beacon.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "beacon.enabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-webrtc-leak",
            Label = "Disable Firefox WebRTC IP Leak",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WebRTC from leaking local IP addresses. Enhances VPN privacy. Default: leaks IP.",
            Tags = ["firefox", "webrtc", "ip", "leak", "vpn"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "media.peerconnection.ice.no_host", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "media.peerconnection.ice.no_host")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "media.peerconnection.ice.no_host", 1),
            ],
        },
        new TweakDef
        {
            Id = "firefox-disable-speculative-connections",
            Label = "Disable Firefox Speculative Connections",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from pre-connecting to sites when hovering over links. Reduces network requests. Default: enabled.",
            Tags = ["firefox", "speculative", "connections", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.http.speculative-parallel-limit", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.http.speculative-parallel-limit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.http.speculative-parallel-limit", 0),
            ],
        },
        new TweakDef
        {
            Id = "firefox-disable-telemetry",
            Label = "Disable Firefox Telemetry",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox telemetry reporting via enterprise policy. Default: enabled.",
            Tags = ["firefox", "telemetry", "privacy", "reporting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-pocket",
            Label = "Disable Firefox Pocket",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Pocket save-for-later service integration in Firefox. Default: enabled.",
            Tags = ["firefox", "pocket", "service", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-default-browser-check",
            Label = "Disable Firefox Default Browser Check",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from prompting to set itself as default browser on launch. Default: prompts.",
            Tags = ["firefox", "default-browser", "prompt", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DontCheckDefaultBrowser", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DontCheckDefaultBrowser")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DontCheckDefaultBrowser", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-crash-reporter",
            Label = "Disable Firefox Crash Reporter",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Firefox crash reporter that sends crash data to Mozilla. Default: enabled.",
            Tags = ["firefox", "crash", "reporter", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-password-manager",
            Label = "Disable Firefox Password Manager",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in Firefox password manager. Use a dedicated password manager. Default: enabled.",
            Tags = ["firefox", "password", "manager", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "signon.rememberSignons", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "signon.rememberSignons")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "signon.rememberSignons", 0)],
        },
        new TweakDef
        {
            Id = "firefox-enable-tracking-protection",
            Label = "Enable Firefox Enhanced Tracking Protection (Strict)",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables strict Enhanced Tracking Protection in Firefox. Blocks trackers, cryptominers, fingerprinters. Default: standard.",
            Tags = ["firefox", "tracking", "protection", "privacy", "strict"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "privacy.trackingprotection.enabled", 1),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences",
                    "privacy.trackingprotection.cryptomining.enabled",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences",
                    "privacy.trackingprotection.fingerprinting.enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "privacy.trackingprotection.enabled"),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences",
                    "privacy.trackingprotection.cryptomining.enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences",
                    "privacy.trackingprotection.fingerprinting.enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "privacy.trackingprotection.enabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "firefox-disable-prefetch",
            Label = "Disable Firefox Link Prefetching",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables link prefetching and DNS prefetching in Firefox. Saves bandwidth and improves privacy. Default: enabled.",
            Tags = ["firefox", "prefetch", "bandwidth", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.prefetch-next", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.dns.disablePrefetch", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.prefetch-next"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.dns.disablePrefetch"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.prefetch-next", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-telemetry",
            Label = "Disable Firefox Telemetry",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Firefox telemetry and data collection via policy.",
            Tags = ["firefox", "browser", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-update",
            Label = "Disable Firefox Auto-Update",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Firefox from auto-updating via policy. For controlled environments.",
            Tags = ["firefox", "browser", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableAppUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableAppUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableAppUpdate", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-pocket",
            Label = "Disable Firefox Pocket",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Pocket read-later integration in Firefox via policy.",
            Tags = ["firefox", "browser", "pocket"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-default-check",
            Label = "Disable Firefox Default Browser Check",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from checking if it's the default browser on startup.",
            Tags = ["firefox", "browser", "default", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DontCheckDefaultBrowser", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DontCheckDefaultBrowser")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DontCheckDefaultBrowser", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-crash-reporter",
            Label = "Disable Firefox Crash Reporter",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Firefox crash reporter from sending reports to Mozilla.",
            Tags = ["firefox", "browser", "crash", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-auto-update",
            Label = "Disable Firefox Background Update Service",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Firefox background update service that checks for updates.",
            Tags = ["firefox", "browser", "update", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "BackgroundAppUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "BackgroundAppUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "BackgroundAppUpdate", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-captive-portal",
            Label = "Disable Firefox Captive Portal Detection",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox captive portal detection that makes network requests on startup.",
            Tags = ["firefox", "browser", "captive-portal", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "CaptivePortal", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "CaptivePortal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "CaptivePortal", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-default-check",
            Label = "Disable Firefox Override Default Browser Check",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from overriding default browser settings on startup.",
            Tags = ["firefox", "browser", "override", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "OverrideFirstRunPage", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "OverrideFirstRunPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "OverrideFirstRunPage", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-extension-recommendations",
            Label = "Disable Firefox Extension Recommendations",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox extension and feature recommendations on about:addons.",
            Tags = ["firefox", "browser", "extensions", "recommendations"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ExtensionRecommendations", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ExtensionRecommendations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ExtensionRecommendations", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-feedback",
            Label = "Disable Firefox Feedback Prompts",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox feedback and survey prompts via policy.",
            Tags = ["firefox", "browser", "feedback", "surveys"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFeedbackCommands", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFeedbackCommands")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFeedbackCommands", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-studies",
            Label = "Disable Firefox Shield Studies",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox Shield studies and experiments via policy. Prevents Mozilla from testing features on your browser.",
            Tags = ["firefox", "studies", "experiments", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFirefoxStudies", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFirefoxStudies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFirefoxStudies", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-password-reveal",
            Label = "Disable Firefox Password Reveal Button",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the password reveal (eye) button in Firefox login forms via policy.",
            Tags = ["firefox", "password", "reveal", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "firefox-ff-disable-password-autosave",
            Label = "Disable Firefox Password Auto-Save",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from prompting to save passwords. Use a dedicated password manager instead.",
            Tags = ["firefox", "password", "autosave", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "signon.rememberSignons", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "signon.rememberSignons")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "signon.rememberSignons", 0)],
        },
        new TweakDef
        {
            Id = "firefox-ff-disable-safe-mode",
            Label = "Disable Firefox Safe Mode",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Firefox safe mode to prevent accidental profile resets. For managed environments.",
            Tags = ["firefox", "safe-mode", "managed", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableSafeMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableSafeMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableSafeMode", 1)],
        },
        new TweakDef
        {
            Id = "firefox-ff-disable-screenshots",
            Label = "Disable Firefox Screenshots",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in Firefox Screenshots feature.",
            Tags = ["firefox", "screenshots", "feature"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "extensions.screenshots.disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "extensions.screenshots.disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "extensions.screenshots.disabled", 1)],
        },
        new TweakDef
        {
            Id = "firefox-telemetry-off",
            Label = "Disable Firefox telemetry",
            Category = "Browser",
            Tags = ["firefox", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "firefox-crash-reporter-off",
            Label = "Disable Firefox crash reporter",
            Category = "Browser",
            Tags = ["firefox", "crash", "reporter", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableCrashReporter", 1)],
        },
        new TweakDef
        {
            Id = "firefox-pocket-off",
            Label = "Disable Pocket integration in Firefox",
            Category = "Browser",
            Tags = ["firefox", "pocket", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePocket", 1)],
        },
        new TweakDef
        {
            Id = "firefox-doh-off",
            Label = "Disable DNS-over-HTTPS in Firefox",
            Category = "Browser",
            Tags = ["firefox", "dns", "doh", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-hw-accel-on",
            Label = "Enable hardware acceleration in Firefox",
            Category = "Browser",
            Tags = ["firefox", "hardware", "acceleration", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "HardwareAcceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "HardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "HardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "firefox-mozilla-accounts-off",
            Label = "Disable Mozilla accounts / Firefox Sync",
            Category = "Browser",
            Tags = ["firefox", "accounts", "sync", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFirefoxAccounts", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFirefoxAccounts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFirefoxAccounts", 1)],
        },
        new TweakDef
        {
            Id = "firefox-save-login-prompt-off",
            Label = "Disable \"Save login\" prompts in Firefox",
            Category = "Browser",
            Tags = ["firefox", "login", "password", "prompt", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "OfferToSaveLoginsDefault", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "OfferToSaveLoginsDefault")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "OfferToSaveLoginsDefault", 0)],
        },
        new TweakDef
        {
            Id = "firefox-default-bookmarks-off",
            Label = "Remove default Firefox bookmarks",
            Category = "Browser",
            Tags = ["firefox", "bookmarks", "default", "cleanup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "NoDefaultBookmarks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "NoDefaultBookmarks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "NoDefaultBookmarks", 1)],
        },
        new TweakDef
        {
            Id = "firefox-system-addon-update-off",
            Label = "Disable system add-on auto-update in Firefox",
            Category = "Browser",
            Tags = ["firefox", "addon", "update", "system", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableSystemAddonUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableSystemAddonUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableSystemAddonUpdate", 1)],
        },
        new TweakDef
        {
            Id = "firefox-app-update-off",
            Label = "Disable Firefox automatic application updates",
            Category = "Browser",
            Tags = ["firefox", "update", "automatic", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableAppUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableAppUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableAppUpdate", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-private-browsing-gpo",
            Label = "Disable Firefox private browsing mode via GPO",
            Category = "Browser",
            Tags = ["firefox", "private-browsing", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePrivateBrowsing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePrivateBrowsing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisablePrivateBrowsing", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-extension-update-gpo",
            Label = "Disable Firefox extension auto-update via GPO",
            Category = "Browser",
            Tags = ["firefox", "extension", "update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ExtensionUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ExtensionUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ExtensionUpdate", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-sync-gpo",
            Label = "Disable Firefox Sync (Mozilla account sync) via GPO",
            Category = "Browser",
            Tags = ["firefox", "sync", "mozilla", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-dev-tools-gpo",
            Label = "Disable Firefox developer tools via GPO",
            Category = "Browser",
            Tags = ["firefox", "devtools", "developer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableDeveloperTools", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableDeveloperTools")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableDeveloperTools", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-pdf-viewer-gpo",
            Label = "Disable Firefox built-in PDF viewer (use external PDF app)",
            Category = "Browser",
            Tags = ["firefox", "pdf", "viewer", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableBuiltinPDFViewer", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableBuiltinPDFViewer")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableBuiltinPDFViewer", 1)],
        },
        new TweakDef
        {
            Id = "firefox-more-from-mozilla-off",
            Label = "Disable Firefox 'More from Mozilla' in settings",
            Category = "Browser",
            Tags = ["firefox", "mozilla", "recommendations", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\UserMessaging", "MoreFromMozilla", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\UserMessaging", "MoreFromMozilla")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\UserMessaging", "MoreFromMozilla", 0)],
        },
        new TweakDef
        {
            Id = "firefox-skip-onboarding",
            Label = "Skip Firefox first-run onboarding screen",
            Category = "Browser",
            Tags = ["firefox", "onboarding", "first-run", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\UserMessaging", "SkipOnboarding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\UserMessaging", "SkipOnboarding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\UserMessaging", "SkipOnboarding", 1)],
        },
        new TweakDef
        {
            Id = "firefox-home-sponsored-top-sites-off",
            Label = "Disable Firefox Home sponsored top sites",
            Category = "Browser",
            Tags = ["firefox", "home", "sponsored", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "SponsoredTopSites", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "SponsoredTopSites")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "SponsoredTopSites", 0)],
        },
        new TweakDef
        {
            Id = "firefox-home-pocket-off",
            Label = "Disable Firefox Home Pocket content",
            Category = "Browser",
            Tags = ["firefox", "home", "pocket", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "Pocket", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "Pocket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "Pocket", 0)],
        },
        new TweakDef
        {
            Id = "firefox-home-sponsored-pocket-off",
            Label = "Disable Firefox Home sponsored Pocket stories",
            Category = "Browser",
            Tags = ["firefox", "home", "pocket", "sponsored", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "SponsoredPocket", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "SponsoredPocket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\FirefoxHome", "SponsoredPocket", 0)],
        },
    ];
}

// ── merged from Edge.cs ──
internal static class Edge
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edge-disable-collections",
            Label = "Disable Edge Collections",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Edge Collections feature used for organizing web content. Reduces UI clutter and memory usage. Default: Enabled. Recommended: Disabled if not used.",
            Tags = ["edge", "collections", "ux", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-mini-menu",
            Label = "Disable Edge Mini Context Menu",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the mini context menu that appears on text selection in Edge. Removes the floating toolbar with search/copy/etc. Default: Enabled. Recommended: Disabled for cleaner UX.",
            Tags = ["edge", "mini-menu", "context-menu", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-shopping-assistant",
            Label = "Disable Edge Shopping Assistant",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge shopping/coupon assistant that appears on retail sites. Default: enabled.",
            Tags = ["edge", "shopping", "coupons", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-startup-boost",
            Label = "Disable Edge Startup Boost",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge Startup Boost that preloads Edge processes at Windows startup. Saves RAM. Default: enabled.",
            Tags = ["edge", "startup", "boost", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-personalization",
            Label = "Disable Edge Personalization and Advertising",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables personalization and advertising features in Edge that track browsing behavior. Default: enabled.",
            Tags = ["edge", "personalization", "advertising", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-browser-sign-in",
            Label = "Disable Automatic Browser Sign-In",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Edge from automatically signing into the browser using the Windows account. Default: auto-sign-in.",
            Tags = ["edge", "sign-in", "account", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-sidebar",
            Label = "Disable Edge Sidebar",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge sidebar panel (Bing Chat, tools, games). Reduces distractions and memory. Default: enabled.",
            Tags = ["edge", "sidebar", "ux", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-first-run-experience",
            Label = "Disable Edge First Run Experience",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Skips the Edge first run experience and import wizard on new profiles. Default: shown.",
            Tags = ["edge", "first-run", "import", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1)],
        },
        new TweakDef
        {
            Id = "edge-disable-password-manager",
            Label = "Disable Edge Password Manager",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge built-in password manager. Use a dedicated password manager. Default: enabled.",
            Tags = ["edge", "password", "manager", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-translate",
            Label = "Disable Edge Translation",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic page translation prompts in Edge. Default: enabled.",
            Tags = ["edge", "translate", "language", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-copilot-sidebar",
            Label = "Disable Edge Copilot Sidebar",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Copilot AI sidebar in Edge. Reduces telemetry and resource usage. Default: enabled.",
            Tags = ["edge", "copilot", "ai", "sidebar", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-sync",
            Label = "Disable Edge Sync",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge data synchronization across devices. Keeps browsing data local. Default: enabled.",
            Tags = ["edge", "sync", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "edge-block-third-party-cookies",
            Label = "Block Third-Party Cookies in Edge",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party cookies in Edge to prevent cross-site tracking. Default: allowed.",
            Tags = ["edge", "cookies", "tracking", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies", 1)],
        },
        new TweakDef
        {
            Id = "edge-disable-metrics",
            Label = "Disable Edge Diagnostic Data Collection",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Edge diagnostic data to off. Reduces telemetry sent to Microsoft. Default: optional.",
            Tags = ["edge", "diagnostics", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiagnosticData", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiagnosticData")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiagnosticData", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-autofill-credit-cards",
            Label = "Disable Edge Credit Card Autofill",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge from saving and auto-filling credit card information. Default: enabled.",
            Tags = ["edge", "autofill", "credit-card", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-startup-boost",
            Label = "Disable Edge Startup Boost (Background Mode)",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Edge from pre-launching at login and running background processes, saving memory and CPU for users who don't use Edge as primary browser.",
            Tags = ["edge", "browser", "startup", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-sidebar",
            Label = "Disable Edge Sidebar & Shopping",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Edge sidebar (Discover), shopping assistant, and collections panel for a cleaner browsing experience.",
            Tags = ["edge", "browser", "sidebar", "shopping"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowMicrosoftRewards", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowMicrosoftRewards"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowMicrosoftRewards", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-telemetry",
            Label = "Disable Edge Telemetry",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Edge metrics, diagnostics, personalisation reporting, follow, spotlight and recommendation features.",
            Tags = ["edge", "browser", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MetricsReportingEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SpotlightExperiencesAndRecommendationsEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MetricsReportingEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SpotlightExperiencesAndRecommendationsEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MetricsReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-update",
            Label = "Disable Edge Auto-Update",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Edge from auto-updating. Useful for controlled environments or when pinning to a specific version.",
            Tags = ["edge", "browser", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "UpdateDefault", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "AutoUpdateCheckPeriodMinutes", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "UpdateDefault"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "AutoUpdateCheckPeriodMinutes"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "UpdateDefault", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-first-run",
            Label = "Disable Edge First-Run Experience",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Skips Edge first-run wizard and hides default top sites on new tab.",
            Tags = ["edge", "browser", "ux", "first-run"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageHideDefaultTopSites", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageHideDefaultTopSites"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageHideDefaultTopSites", 1)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-password-manager",
            Label = "Disable Edge Password Manager & Autofill",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge built-in password manager and address autofill via policy.",
            Tags = ["edge", "browser", "password", "security", "autofill"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillAddressEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillAddressEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillAddressEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-shopping",
            Label = "Disable Edge Shopping Features",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge shopping assistant, price tracking, and coupons. Reduces CPU and network usage.",
            Tags = ["edge", "shopping", "performance", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-rewards",
            Label = "Disable Microsoft Rewards Prompts",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft Rewards prompts in Edge via policy.",
            Tags = ["edge", "browser", "rewards"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowMicrosoftRewards", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowMicrosoftRewards")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowMicrosoftRewards", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-sidebar-hub",
            Label = "Disable Edge Sidebar Hub",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Edge sidebar (Hubs) panel via enterprise policy. Removes the sidebar icon and panel.",
            Tags = ["edge", "sidebar", "hubs", "ux", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SideSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SideSearchEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SideSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-first-run",
            Label = "Disable Edge First Run & Default Browser Check",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Hides the Edge first run experience and default browser prompt via enterprise policy.",
            Tags = ["edge", "first-run", "welcome", "policy", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultBrowserSettingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultBrowserSettingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultBrowserSettingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-workspaces",
            Label = "Disable Edge Workspaces",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Edge Workspaces feature for shared browsing sessions via enterprise policy. Reduces background sync overhead.",
            Tags = ["edge", "workspaces", "collaboration", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeWorkspacesEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeWorkspacesEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeWorkspacesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-drop",
            Label = "Disable Edge Drop",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Edge Drop feature used for cross-device file sharing via enterprise policy. Reduces cloud sync and network usage.",
            Tags = ["edge", "drop", "file-sharing", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeEDropEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeEDropEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeEDropEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-discover",
            Label = "Disable Edge Discover Button",
            Category = "Browser",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Edge Discover (compass) button and page context features via enterprise policy. Reduces Copilot integration.",
            Tags = ["edge", "discover", "copilot", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiscoverPageContextEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiscoverPageContextEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiscoverPageContextEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-startup-boost-off",
            Label = "Disable Edge startup boost",
            Category = "Browser",
            Tags = ["edge", "startup", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-tab-preload-off",
            Label = "Disable Edge new tab page preloading",
            Category = "Browser",
            Tags = ["edge", "tab", "preload", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PreloadNewTabPageEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PreloadNewTabPageEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PreloadNewTabPageEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-media-autoplay-off",
            Label = "Disable media autoplay in Edge",
            Category = "Browser",
            Tags = ["edge", "autoplay", "media", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutoplayAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutoplayAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutoplayAllowed", 0)],
        },
        new TweakDef
        {
            Id = "edge-password-manager-off",
            Label = "Disable Edge built-in password manager",
            Category = "Browser",
            Tags = ["edge", "password", "manager", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-shopping-assistant-off",
            Label = "Disable Edge shopping assistant / Bing price compare",
            Category = "Browser",
            Tags = ["edge", "shopping", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShoppingAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShoppingAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-pdf-external",
            Label = "Open PDFs in external viewer instead of Edge",
            Category = "Browser",
            Tags = ["edge", "pdf", "viewer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AlwaysOpenPdfExternally", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AlwaysOpenPdfExternally")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AlwaysOpenPdfExternally", 1)],
        },
        new TweakDef
        {
            Id = "edge-default-search-provider-lock",
            Label = "Lock default search provider in Edge",
            Category = "Browser",
            Tags = ["edge", "search", "provider", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultSearchProviderEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultSearchProviderEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultSearchProviderEnabled", 1)],
        },
        new TweakDef
        {
            Id = "edge-ie-mode-off",
            Label = "Disable Internet Explorer integration mode in Edge",
            Category = "Browser",
            Tags = ["edge", "ie", "compatibility", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "InternetExplorerIntegrationLevel", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "InternetExplorerIntegrationLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "InternetExplorerIntegrationLevel", 0)],
        },
        new TweakDef
        {
            Id = "edge-wallet-off",
            Label = "Disable Edge Wallet / payment methods",
            Category = "Browser",
            Tags = ["edge", "wallet", "payment", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PaymentMethodQueryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PaymentMethodQueryEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PaymentMethodQueryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-follow-feature-off",
            Label = "Disable Edge Follow / social tracking feature",
            Category = "Browser",
            Tags = ["edge", "follow", "social", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeFollowEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-immersive-reader",
            Label = "Disable Edge Immersive Reader grammar tools",
            Category = "Browser",
            Tags = ["edge", "immersive-reader", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImmersiveReaderGrammarToolsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImmersiveReaderGrammarToolsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImmersiveReaderGrammarToolsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-read-aloud",
            Label = "Disable Edge Read Aloud feature",
            Category = "Browser",
            Tags = ["edge", "read-aloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ReadAloudEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ReadAloudEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ReadAloudEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-cast-icon",
            Label = "Hide Edge Cast toolbar icon (media router)",
            Category = "Browser",
            Tags = ["edge", "cast", "media", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowCastIconInToolbar", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowCastIconInToolbar")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShowCastIconInToolbar", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-hubs-sidebar",
            Label = "Disable Edge Hubs sidebar panel",
            Category = "Browser",
            Tags = ["edge", "hubs", "sidebar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-copilot-page-context",
            Label = "Block Edge Copilot from reading page context",
            Category = "Browser",
            Tags = ["edge", "copilot", "ai", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotPageContext", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotPageContext")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotPageContext", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-news-feed",
            Label = "Disable Edge new tab page news feed / content",
            Category = "Browser",
            Tags = ["edge", "news", "new-tab", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageContentEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageContentEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageContentEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-ntp-recommendations",
            Label = "Disable Edge new tab page recommended content",
            Category = "Browser",
            Tags = ["edge", "recommendations", "new-tab", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageRecommendedContentEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageRecommendedContentEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageRecommendedContentEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-bar-gpo",
            Label = "Disable Edge Bar floating browser window",
            Category = "Browser",
            Tags = ["edge", "edge-bar", "floating", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeBarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeBarEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-insider-promo",
            Label = "Disable Edge insider/beta channel promotions",
            Category = "Browser",
            Tags = ["edge", "insider", "promo", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MicrosoftEdgeInsiderPromotionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MicrosoftEdgeInsiderPromotionEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MicrosoftEdgeInsiderPromotionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-web-widget",
            Label = "Disable Edge web widget (desktop/taskbar search bar)",
            Category = "Browser",
            Tags = ["edge", "web-widget", "search-bar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "WebWidgetAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "WebWidgetAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "WebWidgetAllowed", 0)],
        },
    ];
}

