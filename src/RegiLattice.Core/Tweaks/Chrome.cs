namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Chrome.cs ──
[TweakModule]
internal static class Chrome
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "chrome-disable-renderer-code-integrity",
            Label = "Disable Chrome Renderer Code Integrity",
            Category = "Browser 1",
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
            Id = "chrome-disable-safe-browsing-extended",
            Label = "Disable Chrome Enhanced Safe Browsing",
            Category = "Browser 1",
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
            Id = "chrome-disable-spell-check-service",
            Label = "Disable Chrome Spell Check Service",
            Category = "Browser 1",
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
            Id = "chrome-disable-webrtc-leak",
            Label = "Restrict Chrome WebRTC IP Leaking",
            Category = "Browser 1",
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
            Id = "chrome-disable-chrome-update",
            Label = "Disable Chrome Auto-Update",
            Category = "Browser 1",
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
            Id = "chrome-disable-chrome-hwaccel",
            Label = "Disable Chrome Hardware Acceleration",
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Id = "chrome-disable-reporter",
            Label = "Disable Chrome Software Reporter",
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Id = "chrome-enforce-3p-cookie-block",
            Label = "Enforce Third-Party Cookie Block in Chrome",
            Category = "Browser 1",
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
            Id = "chrome-media-autoplay-off",
            Label = "Disable media autoplay in Chrome",
            Category = "Browser 1",
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
            Category = "Browser 1",
            Tags = ["chrome", "pdf", "viewer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AlwaysOpenPdfExternally", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AlwaysOpenPdfExternally")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AlwaysOpenPdfExternally", 1)],
        },
        new TweakDef
        {
            Id = "chrome-cloud-reporting-off",
            Label = "Disable Chrome cloud reporting",
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Category = "Browser 1",
            Tags = ["chrome", "devtools", "developer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeveloperToolsAvailability", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeveloperToolsAvailability")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "DeveloperToolsAvailability", 2)],
        },
        new TweakDef
        {
            Id = "chrome-disable-save-history",
            Label = "Disable Chrome browsing history saving",
            Category = "Browser 1",
            Tags = ["chrome", "history", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SavingBrowserHistoryDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SavingBrowserHistoryDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SavingBrowserHistoryDisabled", 1)],
        },
        new TweakDef
        {
            Id = "chrome-block-camera",
            Label = "Deny camera access in Chrome",
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Category = "Browser 1",
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
            Category = "Browser 1",
            Tags = ["chrome", "feedback", "reporting", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "UserFeedbackAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "UserFeedbackAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "UserFeedbackAllowed", 0)],
        },
    ];
}
