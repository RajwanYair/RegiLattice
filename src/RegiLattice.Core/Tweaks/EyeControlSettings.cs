#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class EyeControlSettings
{
    private const string EyeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\EyeGaze";
    private const string SensorKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DeviceAccess\EyeGaze";
    private const string PolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EyeTracking";
    private const string AccKey = @"HKEY_CURRENT_USER\Software\Microsoft\Ease of Access\EyeControl";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "eyectrl-enable-gaze-input",
            Label = "Eye Control: Enable Gaze Input Device Support",
            Category = "Accessibility",
            Tags = ["eye-control", "gaze", "accessibility", "motor", "win10"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Essential for users who rely on eye-gaze hardware for PC control.",
            Description =
                "Sets GazeInputEnabled=1 in EyeGaze accessibility settings. Allows the "
                + "Windows Eye Control accessibility feature to accept input from compatible "
                + "eye-tracking hardware (e.g., Tobii, SR Research). "
                + "Default: 0 (disabled). Must be enabled to use Eye Control.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "GazeInputEnabled", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "GazeInputEnabled", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "GazeInputEnabled", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-set-short-dwell-time",
            Label = "Eye Control: Use Short Dwell Time (1 second)",
            Category = "Accessibility",
            Tags = ["eye-control", "dwell", "time", "accessibility", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Faster dwell trigger allows quicker interaction for experienced users.",
            Description =
                "Sets DwellTime=1000 in EyeGaze settings (value in milliseconds). Dwell time "
                + "is how long the user must fixate on an eye-control button before it activates. "
                + "Default: 2000 ms (2 seconds). Reducing to 1000 ms speeds up interaction for "
                + "experienced users with good gaze control.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "DwellTime", 1000)],
            RemoveOps = [RegOp.SetDword(EyeKey, "DwellTime", 2000)],
            DetectOps = [RegOp.CheckDword(EyeKey, "DwellTime", 1000)],
        },
        new TweakDef
        {
            Id = "eyectrl-set-standard-dwell-time",
            Label = "Eye Control: Use Standard Dwell Time (2 seconds)",
            Category = "Accessibility",
            Tags = ["eye-control", "dwell", "time", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Standard 2-second dwell reduces accidental activations.",
            Description =
                "Sets DwellTime=2000 in EyeGaze settings. Restores the default 2-second "
                + "dwell period. Provides a balance between speed and accuracy for most users.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "DwellTime", 2000)],
            RemoveOps = [RegOp.DeleteValue(EyeKey, "DwellTime")],
            DetectOps = [RegOp.CheckDword(EyeKey, "DwellTime", 2000)],
        },
        new TweakDef
        {
            Id = "eyectrl-enable-gaze-cursor",
            Label = "Eye Control: Show Gaze Cursor on Screen",
            Category = "Accessibility",
            Tags = ["eye-control", "cursor", "gaze", "visual", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Visual gaze cursor helps users understand where the system thinks they are looking.",
            Description =
                "Sets ShowGazeCursor=1 in EyeGaze settings. Displays a visual indicator "
                + "showing the current estimated gaze position on screen. Helps users "
                + "calibrate their use of eye-control and understand tracker accuracy.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "ShowGazeCursor", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "ShowGazeCursor", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "ShowGazeCursor", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-enable-smooth-tracking",
            Label = "Eye Control: Enable Gaze Smoothing",
            Category = "Accessibility",
            Tags = ["eye-control", "smooth", "tracking", "gaze", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Smoothing reduces jitter from natural eye tremor, improving control accuracy.",
            Description =
                "Sets GazeSmoothing=1 in EyeGaze settings. Applies a smoothing filter "
                + "to raw gaze data to reduce the effect of natural eye jitter (nystagmus "
                + "and microsaccades). Improves dwell accuracy for most users.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "GazeSmoothing", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "GazeSmoothing", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "GazeSmoothing", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-enable-eye-control-toolbar",
            Label = "Eye Control: Show Launchpad (Eye Control Toolbar)",
            Category = "Accessibility",
            Tags = ["eye-control", "toolbar", "launchpad", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "The launchpad is the primary interface for eye control actions.",
            Description =
                "Sets ShowLaunchpad=1 in EyeGaze settings. Displays the Eye Control "
                + "launchpad toolbar which provides access to mouse functions, keyboard, "
                + "scrolling, and other controls. Must be visible for eye-control navigation.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "ShowLaunchpad", 1)],
            RemoveOps = [RegOp.DeleteValue(EyeKey, "ShowLaunchpad")],
            DetectOps = [RegOp.CheckDword(EyeKey, "ShowLaunchpad", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-enable-blink-to-click",
            Label = "Eye Control: Enable Blink-to-Click Interaction",
            Category = "Accessibility",
            Tags = ["eye-control", "blink", "click", "accessibility", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blink detection as a click signal provides an additional selection method.",
            Description =
                "Sets BlinkToClick=1 in EyeGaze settings. Enables voluntary blink detection "
                + "as an alternative selection trigger. Users can deliberately blink to activate "
                + "the element they are gazing at, in addition to dwell-based selection.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "BlinkToClick", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "BlinkToClick", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "BlinkToClick", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-set-large-launchpad",
            Label = "Eye Control: Use Large Launchpad Buttons",
            Category = "Accessibility",
            Tags = ["eye-control", "launchpad", "large", "buttons", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Larger targets are easier to select accurately with gaze.",
            Description =
                "Sets LaunchpadSize=1 in EyeGaze settings. Sets the Eye Control launchpad "
                + "buttons to the larger size variant. Larger buttons are easier to target "
                + "with gaze, reducing selection errors for users with lower gaze precision.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "LaunchpadSize", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "LaunchpadSize", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "LaunchpadSize", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-enable-shape-writing",
            Label = "Eye Control: Enable Shape Writing in On-Screen Keyboard",
            Category = "Accessibility",
            Tags = ["eye-control", "shape-writing", "keyboard", "typing", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Shape writing allows faster text entry by drawing paths through letters.",
            Description =
                "Sets ShapeWritingEnabled=1 in EyeGaze settings. Enables shape-writing "
                + "mode in the Eye Control on-screen keyboard so that users can form words "
                + "by tracing through letters with their gaze rather than dwelling on each "
                + "letter individually. Significantly increases typing speed.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "ShapeWritingEnabled", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "ShapeWritingEnabled", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "ShapeWritingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "eyectrl-enable-auto-scroll",
            Label = "Eye Control: Enable Automatic Page Scrolling",
            Category = "Accessibility",
            Tags = ["eye-control", "scroll", "automatic", "gaze", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Automatic scroll moves content when gaze reaches screen edge — hands-free reading.",
            Description =
                "Sets AutoScrollEnabled=1 in EyeGaze settings. Configures the system to "
                + "automatically scroll windows and web pages when the user's gaze moves to "
                + "the top or bottom edge of scrollable content. Enables unassisted reading.",
            RegistryKeys = [EyeKey],
            ApplyOps = [RegOp.SetDword(EyeKey, "AutoScrollEnabled", 1)],
            RemoveOps = [RegOp.SetDword(EyeKey, "AutoScrollEnabled", 0)],
            DetectOps = [RegOp.CheckDword(EyeKey, "AutoScrollEnabled", 1)],
        },
    ];
}
