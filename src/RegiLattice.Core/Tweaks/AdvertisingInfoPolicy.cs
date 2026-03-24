// RegiLattice.Core — Tweaks/AdvertisingInfoPolicy.cs
// Advertising ID and targeted advertising machine-scope GPO controls — Sprint 207.
// Controls the Windows Advertising ID (RUID), targeted ad delivery, and per-app ad consent.
// Category: "Advertising Info Policy" | Slug: advinfo
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AdvertisingInfoPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "advinfo-disable-advertising-id",
                Label = "Disable Windows Advertising ID (RUID)",
                Category = "Advertising Info Policy",
                Description =
                    "Disables the Windows Advertising ID (also called Resettable Unique Identifier / RUID) that apps use to deliver targeted advertising across sessions. Prevents cross-app tracking of user behaviour. Default: enabled. Recommended: 1 for all privacy-conscious deployments.",
                Tags = ["advertising", "adid", "tracking", "privacy", "ruid", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Advertising ID is disabled; apps cannot correlate ad impressions across sessions or users.",
                ApplyOps = [RegOp.SetDword(Key, "DisabledByGroupPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisabledByGroupPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "DisabledByGroupPolicy", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-personalised-ads",
                Label = "Disable Personalised Ad Delivery",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents Windows from delivering personalised ads based on browsing history, app usage, and interest profiles. Ads shown in apps and the OS are non-personalised. Default: personalised ads on. Recommended: 1.",
                Tags = ["advertising", "personalised", "targeting", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Personalised ad targeting is disabled; only generic, non-profiled ads may appear.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePersonalisedAds", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalisedAds")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePersonalisedAds", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-block-ad-id-reset",
                Label = "Block User from Resetting / Re-Enabling Advertising ID",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents users from navigating to Settings → Privacy → General and re-enabling or resetting the Advertising ID. Ensures the enterprise policy remains in effect. Default: users can change. Recommended: 1 on managed endpoints.",
                Tags = ["advertising", "adid", "user-restriction", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Settings page advertising ID toggle is greyed out; users cannot re-enable or reset it.",
                ApplyOps = [RegOp.SetDword(Key, "PreventUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventUserOverride")],
                DetectOps = [RegOp.CheckDword(Key, "PreventUserOverride", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-interest-profile",
                Label = "Disable Interest Profile Building for Ads",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents Windows from building an interest and behaviour profile based on usage patterns to improve ad targeting. Stops data collection that feeds the Microsoft ad platform. Default: profiling active. Recommended: 1.",
                Tags = ["advertising", "profiling", "interests", "privacy", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Interest profile for ad purposes is not built; Microsoft's ad platform receives no usage pattern data.",
                ApplyOps = [RegOp.SetDword(Key, "DisableInterestProfile", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInterestProfile")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInterestProfile", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-cross-device-ad-sync",
                Label = "Disable Cross-Device Advertising ID Sync",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents synchronisation of the Advertising ID across devices signed in with the same Microsoft Account. Eliminates cross-device ad targeting linking a user's phone, tablet, and PC. Default: sync enabled. Recommended: 1.",
                Tags = ["advertising", "cross-device", "sync", "privacy", "account", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Advertising ID is not shared across devices; ad targeting cannot link behaviour across a user's device fleet.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCrossDeviceSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossDeviceSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCrossDeviceSync", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-location-for-ads",
                Label = "Block Location Data for Advertising",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents Windows from using geographic location data to serve location-targeted advertisements. Default: location can be used by ad platform. Recommended: 1 for privacy.",
                Tags = ["advertising", "location", "privacy", "geotargeting", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Location data is not used by the advertising platform; geotargeted ads cannot be served.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocationForAds", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationForAds")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocationForAds", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-app-ad-consent-requests",
                Label = "Block Apps from Requesting Ad Consent",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents apps from displaying permission dialogs asking the user to consent to advertising-related data collection. Returns 'denied' to all such requests without prompting. Default: consent prompts allowed. Recommended: 1.",
                Tags = ["advertising", "consent", "apps", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Ad consent dialogs from apps are suppressed; system silently denies all ad-data consent requests.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAdConsentRequests", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAdConsentRequests")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAdConsentRequests", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-ad-activity-history",
                Label = "Disable Ad Activity History Collection",
                Category = "Advertising Info Policy",
                Description =
                    "Stops Windows from collecting and retaining a history of ad impression events (which ads were shown, clicked, or dismissed). Removes a persistent data trail used for attribution and retargeting. Default: history retained. Recommended: 1.",
                Tags = ["advertising", "history", "impressions", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "No ad activity history is stored; retargeting and frequency-capping features in ad platforms are degraded.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAdActivityHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAdActivityHistory")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAdActivityHistory", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-hide-advertising-settings-page",
                Label = "Hide Advertising Privacy Settings page",
                Category = "Advertising Info Policy",
                Description =
                    "Removes the Advertising ID sub-page from Settings → Privacy → General. Prevents users from discovering or interacting with advertising-related privacy controls. Default: page visible. Recommended: 1 on locked-down kiosk endpoints.",
                Tags = ["advertising", "settings", "ui-restriction", "kiosk", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Advertising ID section is hidden from Settings; purely cosmetic — does not change underlying ID state.",
                ApplyOps = [RegOp.SetDword(Key, "HideAdvertisingSettingsPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideAdvertisingSettingsPage")],
                DetectOps = [RegOp.CheckDword(Key, "HideAdvertisingSettingsPage", 1)],
            },
            new TweakDef
            {
                Id = "advinfo-disable-diagnostic-ad-feedback",
                Label = "Disable Diagnostic Feedback for Ad Measurement",
                Category = "Advertising Info Policy",
                Description =
                    "Prevents Windows from sending diagnostic events (install, launch, purchase, uninstall) to Microsoft for use in advertising measurement and attribution. Reduces silently collected conversion data. Default: feedback sent. Recommended: 1.",
                Tags = ["advertising", "diagnostics", "attribution", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "App install/launch/purchase conversion events are not sent to Microsoft's ad measurement service.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticAdFeedback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticAdFeedback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticAdFeedback", 1)],
            },
        ];
}
