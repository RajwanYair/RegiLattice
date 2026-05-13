namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from CloudExperience.cs ──────────────────────────────────────────────────

[TweakModule]
internal static class CloudExperience
{
    private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE";

    private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string ContentDelivery = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";

    private const string WindowsUpdate = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    private const string OobeUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\OOBE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "oobe-disable-consumer-features",
            Label = "Disable Consumer Cloud Features and Spotlight Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "consumer", "cloud", "suggestions", "privacy"],
            Description =
                "Disables Windows consumer features such as Microsoft Spotlight "
                + "advertisements and app suggestions delivered through cloud content. "
                + "DisableWindowsConsumerFeatures=1.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-ads",
            Label = "Disable Lock Screen Spotlight Ads (Enterprise)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "ads", "policy"],
            Description =
                "Disables Windows Spotlight on the lock screen via the cloud content "
                + "policy key (DisableWindowsSpotlightFeatures=1). Prevents Microsoft "
                + "from rotating lock screen images and showing tips and ads.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-onboarding",
            Label = "Disable Post-OOBE Cloud Onboarding Prompts",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "onboarding", "cloud", "prompts", "enterprise"],
            Description =
                "Disables the post-login onboarding flow that invites users to connect "
                + "OneDrive, set up Microsoft 365, etc. Suitable for pre-imaged enterprise "
                + "deployments. SkipNotHerePrompts=1.",
            ApplyOps = [RegOp.SetDword(Oobe, "SkipNotHerePrompts", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "SkipNotHerePrompts")],
            DetectOps = [RegOp.CheckDword(Oobe, "SkipNotHerePrompts", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-silent-app-install",
            Label = "Disable Silent Background App Installation via CDM",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "silent install", "apps", "consumer"],
            Description =
                "Prevents the Content Delivery Manager from silently installing "
                + "suggested and sponsored apps in the background. "
                + "SilentInstalledAppsEnabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-roaming-profile-setup",
            Label = "Disable Roaming Profile Setup Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "roaming", "profile", "onedrive"],
            Description =
                "Suppresses the prompt to back up the Desktop, Documents, and Pictures "
                + "folders to OneDrive during OOBE. DesktopIconsPreference=1 "
                + "(keep local folders).",
            ApplyOps = [RegOp.SetDword(Oobe, "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(Oobe, "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-on-desktop",
            Label = "Disable Spotlight Wallpaper Rotation on Desktop",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "wallpaper", "desktop", "cloud"],
            Description =
                "Prevents Windows Spotlight from rotating the desktop wallpaper. "
                + "RotatingLockScreenEnabled=0. Keeps a fixed wallpaper instead of "
                + "Microsoft's rotating Bing images.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-welcome-experience",
            Label = "Disable 'What's New' Welcome Experience After Updates",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "welcome", "what's new", "update", "cloud"],
            Description =
                "Prevents Windows from showing the 'What's new in Windows' splash screen "
                + "after feature updates complete. ContentDelivery SubscribedContent-310093Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-oem-preinstalled-apps",
            Label = "Disable OEM Pre-Installed Application Install",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "oem", "preinstalled", "apps", "bloatware"],
            Description =
                "Disables the silent installation of OEM-branded applications delivered "
                + "through the Content Delivery Manager (OemPreInstalledAppsEnabled=0). "
                + "Prevents hardware vendors from adding apps post-setup.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-pre-installed-apps",
            Label = "Disable Pre-Installed App Install via ContentDelivery",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "preinstalled", "apps", "curation", "bloatware"],
            Description =
                "Disables automatic installation of curated pre-installed Windows apps "
                + "delivered via the Content Delivery Manager (PreInstalledAppsEnabled=0). "
                + "Reduces initial bloatware on clean installs.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-start-system-pane-suggestions",
            Label = "Disable System Pane Suggestions in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "system pane", "ui"],
            Description =
                "Disables the rotating suggested app tiles displayed in the Windows Start " + "menu system pane (SystemPaneSuggestionsEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-content-delivery",
            Label = "Disable Content Delivery Manager (All CDM Sources)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "content delivery", "suggestions", "apps"],
            Description =
                "Master switch that disables all Content Delivery Manager activity "
                + "by setting ContentDeliveryAllowed=0. Prevents all app suggestions, "
                + "spotlight ads, and cloud-delivered content from being displayed.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338388",
            Label = "Disable Lock Screen Spotlight (SubscribedContent-338388)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "windows", "ads"],
            Description =
                "Disables Windows Spotlight on the lock screen by setting "
                + "SubscribedContent-338388Enabled=0 in the user's Content Delivery "
                + "Manager keys. Falls back to static wallpaper.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353694",
            Label = "Disable Start Menu App Suggestions (SubscribedContent-353694)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "apps", "ads"],
            Description =
                "Disables the 'Occasionally show suggestions in Start' setting "
                + "(SubscribedContent-353694Enabled=0). Stops ad tiles appearing "
                + "in the Start menu recommendations.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353696",
            Label = "Disable Timeline Suggested Content (SubscribedContent-353696)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "timeline", "suggestions", "content", "ads"],
            Description =
                "Disables cloud-delivered suggested activities in the Windows Timeline "
                + "and 'Recommended' section (SubscribedContent-353696Enabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimized Content in Settings",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "cloud", "content", "settings", "policy"],
            Description =
                "Prevents Windows from showing cloud-optimized content — rotating "
                + "Microsoft-curated links and suggestions — inside the Settings app "
                + "(DisableCloudOptimizedContent=1).",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableCloudOptimizedContent", 1)],
        },
    ];
}
