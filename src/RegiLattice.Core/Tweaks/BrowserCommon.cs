namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Cross-browser performance, privacy, and policy tweaks — applies common settings across
/// all browsers via system-wide policies for DNS-over-HTTPS, preloading, tracking prevention,
/// and cache management.
/// </summary>
internal static class BrowserCommon
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string CuKey = @"HKEY_CURRENT_USER";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "browser-disable-dns-prefetch",
            Label = "Disable DNS Prefetching (All Browsers)",
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
            Category = "Browser Common",
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
    ];
}
