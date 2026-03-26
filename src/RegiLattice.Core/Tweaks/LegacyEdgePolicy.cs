namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LegacyEdgePolicy
{
    private const string MainKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main";
    private const string PhishingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter";
    private const string TabKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\TabPreloader";
    private const string InprivateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\InPrivate";
    private const string ServiceUiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\ServiceUI";
    private const string InternetSettingsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Internet Settings";
    private const string ExtensionsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Extensions";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ledge-block-about-flags",
                Label = "Block Access to edge://flags in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Prevents access to the edge://flags page in the legacy Microsoft Edge (EdgeHTML) browser, stopping users from enabling experimental features that may bypass security controls.",
                Tags = ["edge", "legacy edge", "flags", "experimental", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks experimental feature toggles in legacy EdgeHTML; no effect on Chromium Edge.",
                RegistryKeys = [MainKey],
                ApplyOps = [RegOp.SetDword(MainKey, "PreventAccessToAboutFlagsInMicrosoftEdge", 1)],
                RemoveOps = [RegOp.DeleteValue(MainKey, "PreventAccessToAboutFlagsInMicrosoftEdge")],
                DetectOps = [RegOp.CheckDword(MainKey, "PreventAccessToAboutFlagsInMicrosoftEdge", 1)],
            },
            new TweakDef
            {
                Id = "ledge-disable-address-bar-dropdown",
                Label = "Disable Address Bar Drop-Down List in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Disables the drop-down suggestion list that appears when the user types in the legacy Edge address bar, preventing URL history and search suggestion exposure.",
                Tags = ["edge", "legacy edge", "address bar", "suggestions", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes URL history exposure in the address bar; users still navigate but without autocomplete.",
                RegistryKeys = [MainKey],
                ApplyOps = [RegOp.SetDword(MainKey, "AllowAddressBarDropdown", 0)],
                RemoveOps = [RegOp.DeleteValue(MainKey, "AllowAddressBarDropdown")],
                DetectOps = [RegOp.CheckDword(MainKey, "AllowAddressBarDropdown", 0)],
            },
            new TweakDef
            {
                Id = "ledge-disable-tab-preloading",
                Label = "Disable Tab Preloading at Windows Startup in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Prevents legacy Microsoft Edge from preloading tabs in the background when Windows starts, reducing RAM usage and startup overhead on managed systems.",
                Tags = ["edge", "legacy edge", "tab", "preload", "startup", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Reduces startup RAM consumption; Edge still loads normally when launched by the user.",
                RegistryKeys = [TabKey],
                ApplyOps = [RegOp.SetDword(TabKey, "PreventTabPreloading", 1)],
                RemoveOps = [RegOp.DeleteValue(TabKey, "PreventTabPreloading")],
                DetectOps = [RegOp.CheckDword(TabKey, "PreventTabPreloading", 1)],
            },
            new TweakDef
            {
                Id = "ledge-enable-phishing-filter",
                Label = "Enable SmartScreen Phishing Filter in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Enforces the SmartScreen phishing and malware filter in legacy Microsoft Edge, ensuring it cannot be disabled by the user and providing baseline threat protection.",
                Tags = ["edge", "legacy edge", "smartscreen", "phishing", "malware", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Ensures SmartScreen is always active in legacy Edge; critical for phishing protection.",
                RegistryKeys = [PhishingKey],
                ApplyOps = [RegOp.SetDword(PhishingKey, "EnabledV9", 1)],
                RemoveOps = [RegOp.DeleteValue(PhishingKey, "EnabledV9")],
                DetectOps = [RegOp.CheckDword(PhishingKey, "EnabledV9", 1)],
            },
            new TweakDef
            {
                Id = "ledge-prevent-smartscreen-bypass",
                Label = "Prevent Bypassing SmartScreen Warnings in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Prevents users from ignoring or bypassing SmartScreen phishing and malware warnings in legacy Microsoft Edge, enforcing the block when a threat is detected.",
                Tags = ["edge", "legacy edge", "smartscreen", "security", "bypass", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Prevents click-through on SmartScreen threat warnings — users cannot override the block.",
                RegistryKeys = [PhishingKey],
                ApplyOps = [RegOp.SetDword(PhishingKey, "PreventOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(PhishingKey, "PreventOverride")],
                DetectOps = [RegOp.CheckDword(PhishingKey, "PreventOverride", 1)],
            },
            new TweakDef
            {
                Id = "ledge-disable-inprivate-browsing",
                Label = "Disable InPrivate Browsing in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Disables InPrivate browsing mode in legacy Microsoft Edge, ensuring all sessions are tracked in history so that browsing can be audited on managed devices.",
                Tags = ["edge", "legacy edge", "inprivate", "private browsing", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks private mode; all browsing sessions are retained for audit purposes.",
                RegistryKeys = [InprivateKey],
                ApplyOps = [RegOp.SetDword(InprivateKey, "DisableInPrivateBrowsing", 1)],
                RemoveOps = [RegOp.DeleteValue(InprivateKey, "DisableInPrivateBrowsing")],
                DetectOps = [RegOp.CheckDword(InprivateKey, "DisableInPrivateBrowsing", 1)],
            },
            new TweakDef
            {
                Id = "ledge-prevent-flip-ahead",
                Label = "Disable Flip Ahead Page Prediction in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Disables the Flip Ahead feature in legacy Microsoft Edge that pre-fetches the next page in a series, preventing unsolicited network requests and reducing data sent to Microsoft.",
                Tags = ["edge", "legacy edge", "flip ahead", "prefetch", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops speculative page prefetching; no user-visible behaviour change except removal of the swipe gesture.",
                RegistryKeys = [InternetSettingsKey],
                ApplyOps = [RegOp.SetDword(InternetSettingsKey, "PreventFlipAheadEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(InternetSettingsKey, "PreventFlipAheadEnabled")],
                DetectOps = [RegOp.CheckDword(InternetSettingsKey, "PreventFlipAheadEnabled", 1)],
            },
            new TweakDef
            {
                Id = "ledge-hide-first-run-prompt",
                Label = "Hide the First-Run Welcome Page in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Suppresses the first-run welcome page and setup wizard in legacy Microsoft Edge, streamlining deployment on managed machines where browser configuration is set by policy.",
                Tags = ["edge", "legacy edge", "first run", "onboarding", "deployment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes the welcome wizard on first Edge launch; profile settings come from policy instead.",
                RegistryKeys = [ServiceUiKey],
                ApplyOps = [RegOp.SetDword(ServiceUiKey, "AllowWebContentOnNewTabPage", 0)],
                RemoveOps = [RegOp.DeleteValue(ServiceUiKey, "AllowWebContentOnNewTabPage")],
                DetectOps = [RegOp.CheckDword(ServiceUiKey, "AllowWebContentOnNewTabPage", 0)],
            },
            new TweakDef
            {
                Id = "ledge-prevent-extension-dev-tools",
                Label = "Prevent Loading Unpacked Extensions in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Blocks loading of extensions that are not from the Microsoft Store in legacy Microsoft Edge, preventing unpacked or sideloaded extensions from running on managed devices.",
                Tags = ["edge", "legacy edge", "extensions", "developer mode", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks sideloaded extensions; only Store-approved extensions can be installed.",
                RegistryKeys = [ExtensionsKey],
                ApplyOps = [RegOp.SetDword(ExtensionsKey, "AllowExtensionSideloading", 0)],
                RemoveOps = [RegOp.DeleteValue(ExtensionsKey, "AllowExtensionSideloading")],
                DetectOps = [RegOp.CheckDword(ExtensionsKey, "AllowExtensionSideloading", 0)],
            },
            new TweakDef
            {
                Id = "ledge-disable-home-button",
                Label = "Disable the Home Button in Legacy Edge",
                Category = "Legacy Edge Policy",
                Description =
                    "Removes the home button from the legacy Microsoft Edge toolbar, preventing users from quickly navigating to a home page that may not comply with enterprise navigation policies.",
                Tags = ["edge", "legacy edge", "home button", "toolbar", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Removes the home button from the Edge toolbar; enterprise start page is still enforced via other policy.",
                RegistryKeys = [MainKey],
                ApplyOps = [RegOp.SetDword(MainKey, "HomeButtonEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(MainKey, "HomeButtonEnabled")],
                DetectOps = [RegOp.CheckDword(MainKey, "HomeButtonEnabled", 0)],
            },
        ];
}
