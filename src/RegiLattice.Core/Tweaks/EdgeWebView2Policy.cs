// RegiLattice.Core — Tweaks/EdgeWebView2Policy.cs
// Microsoft Edge WebView2 telemetry, update, crash reporting, and isolation controls — Sprint 452.
// Category: "Edge WebView2 Policy" | Slug: wv2pol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Edge\WebView2
//           HKLM\SOFTWARE\Policies\Microsoft\EdgeWebView

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeWebView2Policy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\WebView2";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeWebView";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wv2pol-disable-telemetry",
                Label = "Disable WebView2 Telemetry",
                Category = "Edge WebView2 Policy",
                Description =
                    "Disables usage and diagnostic telemetry in the Edge WebView2 runtime, preventing embedded browser telemetry from third-party apps that use WebView2 for rendering.",
                Tags = ["edge", "webview2", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WebView2 telemetry disabled across all hosted apps; no rendered web content telemetry sent.",
                ApplyOps = [RegOp.SetDword(Key, "MetricsReportingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MetricsReportingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "MetricsReportingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wv2pol-disable-crash-reporting",
                Label = "Disable WebView2 Crash Reporting",
                Category = "Edge WebView2 Policy",
                Description =
                    "Disables automatic crash report uploads from WebView2-hosted applications, preventing crash dump data from being sent to Microsoft.",
                Tags = ["edge", "webview2", "crash-report", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WebView2 crash data not uploaded; reduce telemetry exposure.",
                ApplyOps = [RegOp.SetDword(Key, "SendSiteInfoToImproveServices", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SendSiteInfoToImproveServices")],
                DetectOps = [RegOp.CheckDword(Key, "SendSiteInfoToImproveServices", 0)],
            },
            new TweakDef
            {
                Id = "wv2pol-disable-auto-update",
                Label = "Disable WebView2 Runtime Auto-Update",
                Category = "Edge WebView2 Policy",
                Description =
                    "Disables automatic updates for the Edge WebView2 runtime, ensuring the runtime version is managed by WSUS or MDM rather than auto-updated from the internet.",
                Tags = ["edge", "webview2", "update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WebView2 runtime version locked; update via managed deployment only.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "wv2pol-block-third-party-cookies",
                Label = "Block Third-Party Cookies in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Blocks third-party cookies in all WebView2 instances, preventing cross-origin tracking via embedded browser controls in desktop applications.",
                Tags = ["edge", "webview2", "cookies", "tracking", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Third-party cookies blocked in WebView2; embedded login flows using cross-domain cookies may break.",
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyCookies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyCookies")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyCookies", 1)],
            },
            new TweakDef
            {
                Id = "wv2pol-disable-geolocation",
                Label = "Disable Geolocation API in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Disables the Geolocation API in WebView2 instances, preventing embedded browser controls from accessing location data.",
                Tags = ["edge", "webview2", "geolocation", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WebView2 cannot access location data; location-dependent WebView2 features blocked.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultGeolocationSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultGeolocationSetting")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultGeolocationSetting", 2)],
            },
            new TweakDef
            {
                Id = "wv2pol-block-file-system-access",
                Label = "Block File System API Access in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Blocks web content in WebView2 from accessing the local file system via the File System Access API, preventing read/write of arbitrary host files.",
                Tags = ["edge", "webview2", "file-system", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "File System Access API blocked in WebView2; embedded web content cannot access host files.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultFileSystemReadGuardSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultFileSystemReadGuardSetting")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultFileSystemReadGuardSetting", 2)],
            },
            new TweakDef
            {
                Id = "wv2pol-disable-notifications",
                Label = "Block Notification Requests in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Blocks web push notification permission requests in WebView2 instances, preventing embedded browser controls from requesting notification access.",
                Tags = ["edge", "webview2", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WebView2 push notification prompts suppressed.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultNotificationsSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultNotificationsSetting")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultNotificationsSetting", 2)],
            },
            new TweakDef
            {
                Id = "wv2pol-force-safe-browsing",
                Label = "Force Safe Browsing in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Forces Safe Browsing URL reputation checking in WebView2 instances, ensuring all URLs loaded in embedded browser controls are checked against the Safe Browsing database.",
                Tags = ["edge", "webview2", "safe-browsing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Safe Browsing active in all WebView2 controls; known malicious URLs blocked.",
                ApplyOps = [RegOp.SetDword(Key, "SafeBrowsingProtectionLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "SafeBrowsingProtectionLevel")],
                DetectOps = [RegOp.CheckDword(Key, "SafeBrowsingProtectionLevel", 2)],
            },
            new TweakDef
            {
                Id = "wv2pol-disable-speech-api",
                Label = "Disable Speech Recognition API in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Disables the Web Speech API in WebView2 instances, preventing embedded browser controls from accessing the microphone for speech recognition.",
                Tags = ["edge", "webview2", "speech", "microphone", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Speech API disabled in WebView2; microphone access from embedded controls blocked.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSpeechApiFeature", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSpeechApiFeature")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSpeechApiFeature", 1)],
            },
            new TweakDef
            {
                Id = "wv2pol-block-protocol-handlers",
                Label = "Block Custom Protocol Handlers in WebView2",
                Category = "Edge WebView2 Policy",
                Description =
                    "Prevents web content in WebView2 from registering custom URI protocol handlers, blocking potential protocol exploitation vectors in embedded browser controls.",
                Tags = ["edge", "webview2", "protocol-handlers", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Custom protocol handler registration blocked in WebView2 controls.",
                ApplyOps = [RegOp.SetDword(Key, "RegisteredProtocolHandlers", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "RegisteredProtocolHandlers")],
                DetectOps = [RegOp.CheckDword(Key, "RegisteredProtocolHandlers", 0)],
            },
        ];
}
