namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeImportPrivacyPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "edgeimp-block-import-favorites",
                Label = "Block Importing Favorites Into Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from importing favorites from other browsers into Microsoft Edge, reducing the risk of accidentally migrating browser data to a managed profile.",
                Tags = ["edge", "browser", "import", "favorites", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keeps managed Edge profiles free of unvetted imported bookmarks.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImportFavorites", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImportFavorites")],
                DetectOps = [RegOp.CheckDword(Key, "ImportFavorites", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-block-import-history",
                Label = "Block Importing Browsing History Into Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from importing browsing history from other browsers into Microsoft Edge, keeping the Edge history store clean and preventing cross-browser data leakage.",
                Tags = ["edge", "browser", "import", "history", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents import of personal or untrusted browsing history into managed Edge.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImportHistory", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImportHistory")],
                DetectOps = [RegOp.CheckDword(Key, "ImportHistory", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-block-import-cookies",
                Label = "Block Importing Cookies Into Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from importing cookies from other browsers into Microsoft Edge, reducing session-hijacking risk and keeping the Edge cookie store isolated.",
                Tags = ["edge", "browser", "import", "cookies", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents stale or malicious cookies from another browser being imported into Edge.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImportCookies", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImportCookies")],
                DetectOps = [RegOp.CheckDword(Key, "ImportCookies", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-block-import-homepage",
                Label = "Block Importing Homepage Settings Into Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from importing homepage and new-tab page settings from other browsers into Microsoft Edge, preserving the enterprise-configured homepage policy.",
                Tags = ["edge", "browser", "import", "homepage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keeps the IT-configured Edge homepage from being overridden by imported settings.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImportHomepage", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImportHomepage")],
                DetectOps = [RegOp.CheckDword(Key, "ImportHomepage", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-block-import-open-tabs",
                Label = "Block Importing Open Tabs Into Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from importing open tabs from other browsers into Microsoft Edge, avoiding unintended opening of external browser sessions inside managed Edge.",
                Tags = ["edge", "browser", "import", "tabs", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents unmanaged tab sessions from being brought into managed Edge.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImportOpenTabs", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImportOpenTabs")],
                DetectOps = [RegOp.CheckDword(Key, "ImportOpenTabs", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-block-import-search-engine",
                Label = "Block Importing Search Engine Settings Into Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from importing search engine settings from other browsers into Microsoft Edge, preserving the enterprise default search provider configuration.",
                Tags = ["edge", "browser", "import", "search", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keeps the IT-configured search provider from being overridden by imported settings.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImportSearchEngine", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImportSearchEngine")],
                DetectOps = [RegOp.CheckDword(Key, "ImportSearchEngine", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-disable-browsing-history",
                Label = "Disable Saving Browsing History in Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents Microsoft Edge from saving the user's browsing history locally, effectively enabling a permanent private-browsing mode for history and reducing local data exposure.",
                Tags = ["edge", "browser", "history", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Eliminates local browsing history; users cannot browse or restore history within Edge.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SavingBrowserHistoryDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SavingBrowserHistoryDisabled")],
                DetectOps = [RegOp.CheckDword(Key, "SavingBrowserHistoryDisabled", 1)],
            },
            new TweakDef
            {
                Id = "edgeimp-disable-user-feedback",
                Label = "Disable Edge User Feedback Submissions",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents users from submitting feedback and usage telemetry to Microsoft via the Edge built-in feedback tool, reducing data exfiltration of browsing context.",
                Tags = ["edge", "browser", "feedback", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes the feedback button and disables the underlying feedback service in Edge.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UserFeedbackAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "UserFeedbackAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "UserFeedbackAllowed", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-prevent-ssl-bypass",
                Label = "Prevent Users From Bypassing SSL Certificate Errors in Edge",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Disables the 'Proceed anyway' option on SSL certificate error pages in Microsoft Edge, forcing users to stop when a certificate warning fires instead of bypassing it.",
                Tags = ["edge", "browser", "ssl", "tls", "certificate", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents MITM bypass via self-signed cert acceptance; may block access to internal sites with bad certs.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SSLErrorOverrideAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SSLErrorOverrideAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "SSLErrorOverrideAllowed", 0)],
            },
            new TweakDef
            {
                Id = "edgeimp-disable-site-info-reporting",
                Label = "Disable Sending Site Info to Microsoft for Edge Improvement",
                Category = "Edge Import & Privacy Policy",
                Description =
                    "Prevents Microsoft Edge from sending website diagnostic information to Microsoft to improve browser services, reducing cross-site browsing telemetry sent to Microsoft.",
                Tags = ["edge", "browser", "telemetry", "site info", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops per-site diagnostic reports from being uploaded to Microsoft.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SendSiteInfoToImproveServices", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SendSiteInfoToImproveServices")],
                DetectOps = [RegOp.CheckDword(Key, "SendSiteInfoToImproveServices", 0)],
            },
        ];
}
