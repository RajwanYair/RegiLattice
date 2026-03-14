namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Firefox
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "firefox-dns-over-https",
            Label = "Enable DNS-over-HTTPS",
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
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
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in Firefox Screenshots feature.",
            Tags = ["firefox", "screenshots", "feature"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "extensions.screenshots.disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "extensions.screenshots.disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "extensions.screenshots.disabled", 1)],
        },
    ];
}
