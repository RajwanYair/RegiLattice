#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class LiveCaptions
{
    private const string LcKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\LiveCaptions";
    private const string AccKey = @"HKEY_CURRENT_USER\Software\Microsoft\Accessibility\LiveCaptions";
    private const string UiKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\LiveCaptionsUI";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lcap-enable-profanity-filter",
            Label = "Live Captions: Enable Profanity Filter",
            Category = "Accessibility",
            Tags = ["live-captions", "profanity-filter", "captions", "accessibility", "win11"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Masks profane words with asterisks in spoken-to-caption text.",
            Description =
                "Sets ProfanityFilterEnabled=1 in LiveCaptions settings. Enables built-in "
                + "profanity masking so that offensive words are displayed as asterisks in "
                + "the caption overlay. Requires Windows 11 22H2 or later. Default: 1.",
            MinBuild = 22621,
            RegistryKeys = [LcKey],
            ApplyOps = [RegOp.SetDword(LcKey, "ProfanityFilterEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(LcKey, "ProfanityFilterEnabled")],
            DetectOps = [RegOp.CheckDword(LcKey, "ProfanityFilterEnabled", 1)],
        },
        new TweakDef
        {
            Id = "lcap-disable-profanity-filter",
            Label = "Live Captions: Disable Profanity Filter",
            Category = "Accessibility",
            Tags = ["live-captions", "profanity-filter", "captions", "accessibility", "win11"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Displays all words verbatim including profanity.",
            Description =
                "Sets ProfanityFilterEnabled=0 in LiveCaptions settings. Disables profanity "
                + "masking so that all words are shown exactly as spoken. "
                + "Useful in professional or judicial transcription scenarios.",
            MinBuild = 22621,
            RegistryKeys = [LcKey],
            ApplyOps = [RegOp.SetDword(LcKey, "ProfanityFilterEnabled", 0)],
            RemoveOps = [RegOp.SetDword(LcKey, "ProfanityFilterEnabled", 1)],
            DetectOps = [RegOp.CheckDword(LcKey, "ProfanityFilterEnabled", 0)],
        },
        new TweakDef
        {
            Id = "lcap-use-large-text",
            Label = "Live Captions: Use Large Caption Text Size",
            Category = "Accessibility",
            Tags = ["live-captions", "text-size", "large", "captions", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Larger caption text is easier to read, especially at distance.",
            Description =
                "Sets CaptionTextSize=2 in LiveCaptions UI settings. Selects the largest "
                + "text size for the caption overlay. Size values: 0=small, 1=medium, 2=large. "
                + "Default: 1 (medium). Improves readability for low-vision users.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionTextSize", 2)],
            RemoveOps = [RegOp.SetDword(UiKey, "CaptionTextSize", 1)],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionTextSize", 2)],
        },
        new TweakDef
        {
            Id = "lcap-use-small-text",
            Label = "Live Captions: Use Small Caption Text Size",
            Category = "Accessibility",
            Tags = ["live-captions", "text-size", "small", "captions", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Smaller text covers less screen real estate.",
            Description =
                "Sets CaptionTextSize=0 in LiveCaptions UI settings. Selects the smallest "
                + "text size for the caption overlay. Useful on large displays where screen "
                + "space is at a premium.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionTextSize", 0)],
            RemoveOps = [RegOp.SetDword(UiKey, "CaptionTextSize", 1)],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionTextSize", 0)],
        },
        new TweakDef
        {
            Id = "lcap-position-bottom",
            Label = "Live Captions: Position Caption Overlay at Bottom of Screen",
            Category = "Accessibility",
            Tags = ["live-captions", "position", "bottom", "overlay", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Bottom positioning keeps captions near the natural reading spot.",
            Description =
                "Sets CaptionPosition=0 in LiveCaptions UI settings. Places the caption "
                + "overlay at the bottom of the screen. Position values: 0=bottom, 1=top. "
                + "Default: 0 (bottom). Explicit enforcement of preferred placement.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionPosition", 0)],
            RemoveOps = [RegOp.DeleteValue(UiKey, "CaptionPosition")],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionPosition", 0)],
        },
        new TweakDef
        {
            Id = "lcap-position-top",
            Label = "Live Captions: Position Caption Overlay at Top of Screen",
            Category = "Accessibility",
            Tags = ["live-captions", "position", "top", "overlay", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Top positioning keeps video content visible below the caption bar.",
            Description =
                "Sets CaptionPosition=1 in LiveCaptions UI settings. Places the caption "
                + "overlay at the top of the screen. Useful for video content that occupies "
                + "the bottom portion of the screen.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionPosition", 1)],
            RemoveOps = [RegOp.SetDword(UiKey, "CaptionPosition", 0)],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionPosition", 1)],
        },
        new TweakDef
        {
            Id = "lcap-use-dark-theme",
            Label = "Live Captions: Use Dark Caption Overlay Theme",
            Category = "Accessibility",
            Tags = ["live-captions", "dark", "theme", "overlay", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Dark background with light text — easier to read in bright environments.",
            Description =
                "Sets CaptionColorTheme=0 in LiveCaptions UI settings. Applies a dark "
                + "background with white text to the caption overlay. "
                + "Theme values: 0=dark, 1=light, 2=high contrast. Default: follows system theme.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionColorTheme", 0)],
            RemoveOps = [RegOp.DeleteValue(UiKey, "CaptionColorTheme")],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionColorTheme", 0)],
        },
        new TweakDef
        {
            Id = "lcap-use-high-contrast-theme",
            Label = "Live Captions: Use High-Contrast Caption Overlay Theme",
            Category = "Accessibility",
            Tags = ["live-captions", "high-contrast", "theme", "overlay", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Maximum contrast — yellow text on black for users with low vision.",
            Description =
                "Sets CaptionColorTheme=2 in LiveCaptions UI settings. Applies a "
                + "high-contrast colour scheme (yellow on black) to the caption overlay. "
                + "Designed for users with moderate to severe visual impairments.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionColorTheme", 2)],
            RemoveOps = [RegOp.DeleteValue(UiKey, "CaptionColorTheme")],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionColorTheme", 2)],
        },
        new TweakDef
        {
            Id = "lcap-include-sound-effects",
            Label = "Live Captions: Include Sound Effect Labels",
            Category = "Accessibility",
            Tags = ["live-captions", "sound-effects", "labels", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Displays [music] [applause] labels — essential for deaf/hard of hearing users.",
            Description =
                "Sets SoundEffectsEnabled=1 in LiveCaptions settings. Enables contextual "
                + "sound-effect labels in the caption stream, such as [music playing] or "
                + "[applause]. Provides non-speech audio context for deaf or hard-of-hearing users.",
            MinBuild = 22621,
            RegistryKeys = [LcKey],
            ApplyOps = [RegOp.SetDword(LcKey, "SoundEffectsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(LcKey, "SoundEffectsEnabled")],
            DetectOps = [RegOp.CheckDword(LcKey, "SoundEffectsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "lcap-use-wider-overlay",
            Label = "Live Captions: Use Wider Caption Overlay (75% Screen Width)",
            Category = "Accessibility",
            Tags = ["live-captions", "width", "overlay", "captions", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Wider bar shows more words per line, reducing caption lag on fast speech.",
            Description =
                "Sets CaptionWidth=75 in LiveCaptions UI settings. Sets the caption overlay "
                + "to span 75% of the screen width. Wider overlays hold more text per line, "
                + "reducing the frequency of text scrolling on fast-paced speech.",
            MinBuild = 22621,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "CaptionWidth", 75)],
            RemoveOps = [RegOp.DeleteValue(UiKey, "CaptionWidth")],
            DetectOps = [RegOp.CheckDword(UiKey, "CaptionWidth", 75)],
        },
    ];
}
