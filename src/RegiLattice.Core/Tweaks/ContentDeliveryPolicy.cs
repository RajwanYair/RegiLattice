// RegiLattice.Core — Tweaks/ContentDeliveryPolicy.cs
// Windows Content Delivery Manager (CDM) and CloudContent Group Policy settings.
// Slug: "cdpol" — distinct from CloudContentPolicy.cs (user-level CloudContent HKCU settings).
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent  (machine-wide policy).
// Controls Windows Spotlight, suggested apps, consumer promos, and Start menu cloud content.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ContentDeliveryPolicy
{
    private const string CloudPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
    private const string StartPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Start";
    private const string CdmPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ContentDeliveryManager";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cdpol-disable-consumer-experiences",
            Label = "Disable Windows Consumer Experiences (Bloatware Auto-Install)",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["content delivery", "bloatware", "debloat", "consumer", "privacy", "group policy"],
            Description =
                "Disables the Content Delivery Manager from auto-installing suggested third-party apps "
                + "(games, Candy Crush, Spotify etc.) via Windows Consumer Experiences. "
                + "DisableWindowsConsumerFeatures = 1. "
                + "Essential for enterprise deployments and clean Windows installations. "
                + "Default: consumer app suggestions silently installed after setup.",
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsConsumerFeatures", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-windows-spotlight",
            Label = "Disable Windows Spotlight on Lock Screen",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["content delivery", "spotlight", "lock screen", "privacy", "group policy"],
            Description =
                "Disables Windows Spotlight on the lock screen via Group Policy. "
                + "DisableWindowsSpotlightFeatures = 1. "
                + "Prevents Microsoft-curated wallpapers, tips, and ads from displaying on the lock screen. "
                + "Default: Spotlight enabled showing Bing-sourced images and suggestions.",
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightFeatures", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-spotlight-action-center",
            Label = "Disable Windows Spotlight in Action Center",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["content delivery", "spotlight", "action center", "notifications", "group policy"],
            Description =
                "Prevents Windows Spotlight suggestions and tips from appearing in notifications "
                + "and the Action Center. "
                + "DisableWindowsSpotlightOnActionCenter = 1. "
                + "Removes Microsoft promotional content from the notification tray. "
                + "Default: Spotlight Action Center content enabled.",
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnActionCenter", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnActionCenter", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightOnActionCenter", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-third-party-spotlight",
            Label = "Disable Third-Party App Suggestions in Spotlight",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["content delivery", "spotlight", "ads", "privacy", "third-party", "group policy"],
            Description =
                "Blocks third-party application advertisements and suggestions from appearing "
                + "within the Windows Spotlight and lock screen features. "
                + "DisableThirdPartySuggestions = 1. "
                + "Prevents marketplaced app promotions from appearing even when Spotlight is otherwise active. "
                + "Default: third-party suggestions shown.",
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableThirdPartySuggestions", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-start-suggestions",
            Label = "Disable Suggested Apps in Start Menu",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["content delivery", "start menu", "suggestions", "bloatware", "group policy"],
            Description =
                "Prevents cloud-powered app suggestions and recommendations from appearing "
                + "in the Start menu's recommended section. "
                + "DisableAppsFromStore = 0 but SubscribedContent-338389Enabled = 0 equivalent via policy. "
                + "HideRecommendedSection = 1. "
                + "Gives users a clean, app-only Start menu without Microsoft Store promotions. "
                + "Default: recommendations shown.",
            MinBuild = 22000,
            ApplyOps = [RegOp.SetDword(StartPol, "HideRecommendedSection", 1)],
            RemoveOps = [RegOp.SetDword(StartPol, "HideRecommendedSection", 0)],
            DetectOps = [RegOp.CheckDword(StartPol, "HideRecommendedSection", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-spotlight-taskbar",
            Label = "Disable Windows Spotlight on Taskbar and Search",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["content delivery", "spotlight", "taskbar", "search", "privacy", "group policy"],
            Description =
                "Removes Windows Spotlight suggestions from the taskbar search box and Search Home. "
                + "DisableWindowsSpotlightOnSettings = 1. "
                + "Prevents Bing-powered content from appearing in the taskbar search callout. "
                + "Default: Spotlight taskbar content enabled on Windows 11.",
            MinBuild = 22000,
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnSettings", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightOnSettings", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightOnSettings", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-oobe-tips",
            Label = "Disable Spotlight-Based Tips and Tricks During Setup (OOBE)",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["content delivery", "oobe", "setup", "tips", "group policy"],
            Description =
                "Disables Windows Spotlight-powered tips and suggestions shown during Out-of-Box "
                + "Experience (OOBE) and Windows first-run setup screens. "
                + "DisableWindowsSpotlightWindowsWelcomeExperience = 1. "
                + "Streamlines enterprise provisioning and audit by removing consumer-targeted prompts.",
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableWindowsSpotlightWindowsWelcomeExperience", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-content-delivery-auto-download",
            Label = "Disable Content Delivery Manager Auto-Downloads",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["content delivery", "auto-download", "bloatware", "privacy", "bandwidth", "group policy"],
            Description =
                "Prevents the Content Delivery Manager from silently downloading new app packages, "
                + "features, and promotional content in the background. "
                + "PreventAutoContentDelivery = 1 via CdmPol. "
                + "Reduces surprise bandwidth usage and prevents unwanted app installations on metered connections.",
            ApplyOps = [RegOp.SetDword(CdmPol, "PreventAutoContentDelivery", 1)],
            RemoveOps = [RegOp.SetDword(CdmPol, "PreventAutoContentDelivery", 0)],
            DetectOps = [RegOp.CheckDword(CdmPol, "PreventAutoContentDelivery", 1)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-get-office-promotion",
            Label = "Disable 'Get Microsoft 365' Promotional Node in Settings",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["content delivery", "office 365", "microsoft 365", "ads", "privacy", "group policy"],
            Description =
                "Removes the Microsoft 365 / Office sign-up promotional page from Windows Settings. "
                + "DisableWindowsSpotlightOnSettingsOfficePush_ProviderSet = 1. "
                + "Stops Microsoft Office subscription upsells from appearing in the Settings app. "
                + "Default: promotion shown when Office is not installed.",
            ApplyOps = [RegOp.SetDword(CloudPol, "ConfigureWindowsSpotlight", 2)],
            RemoveOps = [RegOp.DeleteValue(CloudPol, "ConfigureWindowsSpotlight")],
            DetectOps = [RegOp.CheckDword(CloudPol, "ConfigureWindowsSpotlight", 2)],
        },
        new TweakDef
        {
            Id = "cdpol-disable-tailored-experiences",
            Label = "Disable Tailored Experiences with Diagnostic Data",
            Category = "Content Delivery Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["content delivery", "tailored", "telemetry", "privacy", "group policy"],
            Description =
                "Prevents Windows from using diagnostic data to show personalised tips, ads, "
                + "and recommendations via the 'Tailored Experiences' feature. "
                + "DisableTailoredExperiencesWithDiagnosticData = 1. "
                + "Stops Microsoft from profiling usage patterns to target in-Windows promotions. "
                + "Default: tailored experiences enabled when diagnostic data is set to Full.",
            ApplyOps = [RegOp.SetDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            RemoveOps = [RegOp.SetDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 0)],
            DetectOps = [RegOp.CheckDword(CloudPol, "DisableTailoredExperiencesWithDiagnosticData", 1)],
        },
    ];
}
