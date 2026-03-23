// Cloud Content & Consumer Experiences Policy — Sprint 149
// Slug "ccpol" — controls Windows cloud content, consumer experience features,
// and lock-screen spotlight values not used by DeviceProvisioningPolicy.cs or Privacy.cs.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent
// HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent  (user-scope variants)
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class CloudContentPolicy
{
    private const string Cloud =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string CloudCu =
        @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ccpol-disable-windows-consumer-features",
            Label = "Cloud Content: Disable Windows consumer features (app suggestions)",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsConsumerFeatures=1 in the CloudContent policy. Turns off the "
                + "'consumer experience' that silently installs promoted apps and shows app suggestions.",
            Tags = ["cloud", "consumer", "suggestions", "debloat", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-disable-third-party-suggestions",
            Label = "Cloud Content: Disable third-party app suggestions in Start and search",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableThirdPartySuggestions=1 in CloudContent policy (machine scope). Prevents "
                + "third-party paid app promotions from appearing in Start menu tiles and search results.",
            Tags = ["cloud", "suggestions", "third-party", "ads", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Cloud, "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(Cloud, "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(Cloud, "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-disable-cloud-optimized-content",
            Label = "Cloud Content: Disable cloud-optimised content delivery",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableCloudOptimizedContent=1 in CloudContent policy. Stops Windows from "
                + "fetching and injecting cloud-optimized UI content (personalized tips, spotlight).",
            Tags = ["cloud", "content", "spotlight", "debloat", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Cloud, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Cloud, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(Cloud, "DisableCloudOptimizedContent", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-disable-spotlight-on-lock-screen",
            Label = "Cloud Content: Disable Windows Spotlight on the lock screen",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsSpotlightFeatures=1 in CloudContent policy (machine scope). "
                + "Prevents Windows Spotlight from downloading and displaying rotating lock-screen images.",
            Tags = ["cloud", "spotlight", "lock-screen", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-disable-spotlight-on-action-center",
            Label = "Cloud Content: Disable Spotlight suggestions in Action Center",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1 in CloudContent policy. "
                + "Suppresses the 'Get to know Windows' / 'What's new' Spotlight popups after updates.",
            Tags = ["cloud", "spotlight", "action-center", "welcome", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightWindowsWelcomeExperience")],
            DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightWindowsWelcomeExperience", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-disable-spotlight-on-settings",
            Label = "Cloud Content: Disable Spotlight content on Settings pages",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsSpotlightOnSettings=1 in CloudContent policy (machine scope). "
                + "Removes cloud-provided spotlight tips and suggestions from Settings app pages.",
            Tags = ["cloud", "spotlight", "settings", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Cloud, "DisableWindowsSpotlightOnSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Cloud, "DisableWindowsSpotlightOnSettings")],
            DetectOps = [RegOp.CheckDword(Cloud, "DisableWindowsSpotlightOnSettings", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-user-disable-third-party-suggestions",
            Label = "Cloud Content (user): Disable third-party suggestions per user",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableThirdPartySuggestions=1 in HKCU CloudContent policy scope. Provides "
                + "per-user enforcement of the third-party app-suggestion block.",
            Tags = ["cloud", "suggestions", "third-party", "ads", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CloudCu, "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(CloudCu, "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-user-disable-spotlight",
            Label = "Cloud Content (user): Disable Windows Spotlight per user",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsSpotlightFeatures=1 in HKCU CloudContent policy scope. Disables "
                + "Spotlight lock-screen rotation for the current signed-in user.",
            Tags = ["cloud", "spotlight", "lock-screen", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "ccpol-user-disable-welcome-experience",
            Label = "Cloud Content (user): Disable Spotlight welcome experience per user",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1 at HKCU scope. Suppresses the "
                + "'What's new' Spotlight welcome popups for the current user after Windows upgrades.",
            Tags = ["cloud", "spotlight", "welcome", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightWindowsWelcomeExperience", 1),
            ],
        },
        new TweakDef
        {
            Id = "ccpol-user-disable-spotlight-on-settings",
            Label = "Cloud Content (user): Disable Spotlight on Settings per user",
            Category = "Cloud Content Policy",
            Description =
                "Sets DisableWindowsSpotlightOnSettings=1 at HKCU CloudContent policy scope. Removes "
                + "cloud-provided spotlight tips from the Settings app for the current user.",
            Tags = ["cloud", "spotlight", "settings", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CloudCu, "DisableWindowsSpotlightOnSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudCu, "DisableWindowsSpotlightOnSettings")],
            DetectOps = [RegOp.CheckDword(CloudCu, "DisableWindowsSpotlightOnSettings", 1)],
        },
    ];
}
