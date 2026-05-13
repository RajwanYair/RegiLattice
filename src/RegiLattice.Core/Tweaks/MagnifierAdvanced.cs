#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class MagnifierAdvanced
{
    private const string MagKey = @"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "magnif-enable-smooth-edges",
            Label = "Magnifier: Enable Smooth Edges for Magnified Content",
            Category = "Accessibility",
            Tags = ["magnifier", "smooth", "edges", "accessibility", "display"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Reduces jaggedness when content is zoomed, improving readability.",
            Description =
                "Sets UseBitmapSmoothing=1 in ScreenMagnifier settings. Enables bitmap "
                + "smoothing so that magnified content uses anti-aliasing. "
                + "Default: 1 (enabled). Explicitly ensuring smooth rendering is active.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "UseBitmapSmoothing", 1)],
            RemoveOps = [RegOp.DeleteValue(MagKey, "UseBitmapSmoothing")],
            DetectOps = [RegOp.CheckDword(MagKey, "UseBitmapSmoothing", 1)],
        },
        new TweakDef
        {
            Id = "magnif-enable-invert-colors",
            Label = "Magnifier: Enable Colour Inversion in Magnified View",
            Category = "Accessibility",
            Tags = ["magnifier", "invert", "colour", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Can improve readability for users with certain visual impairments.",
            Description =
                "Sets Invert=1 in ScreenMagnifier settings. Inverts colours in the magnified "
                + "view, useful for low-vision users who prefer high-contrast inverted display. "
                + "Default: 0 (off).",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "Invert", 1)],
            RemoveOps = [RegOp.SetDword(MagKey, "Invert", 0)],
            DetectOps = [RegOp.CheckDword(MagKey, "Invert", 1)],
        },
        new TweakDef
        {
            Id = "magnif-follow-mouse-cursor",
            Label = "Magnifier: Follow Mouse Cursor",
            Category = "Accessibility",
            Tags = ["magnifier", "follow", "mouse", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps the magnified view centred on the mouse — easier navigation.",
            Description =
                "Sets FollowMouse=1 in ScreenMagnifier settings. The magnified view scrolls "
                + "to keep the mouse pointer visible at all times. Default: 1 (enabled). "
                + "Explicitly enforces this behaviour.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "FollowMouse", 1)],
            RemoveOps = [RegOp.DeleteValue(MagKey, "FollowMouse")],
            DetectOps = [RegOp.CheckDword(MagKey, "FollowMouse", 1)],
        },
        new TweakDef
        {
            Id = "magnif-follow-keyboard-focus",
            Label = "Magnifier: Follow Keyboard Focus",
            Category = "Accessibility",
            Tags = ["magnifier", "follow", "keyboard", "focus", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps the magnified view focused on the active keyboard input element.",
            Description =
                "Sets FollowFocus=1 in ScreenMagnifier settings. The magnified region "
                + "tracks the currently focused UI element (form field, button, etc.). "
                + "Essential for keyboard-only users. Default: 1 (enabled).",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "FollowFocus", 1)],
            RemoveOps = [RegOp.DeleteValue(MagKey, "FollowFocus")],
            DetectOps = [RegOp.CheckDword(MagKey, "FollowFocus", 1)],
        },
        new TweakDef
        {
            Id = "magnif-follow-text-insertion",
            Label = "Magnifier: Follow Text Insertion Point (Caret)",
            Category = "Accessibility",
            Tags = ["magnifier", "caret", "text", "insertion", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Magnified view scrolls with the text cursor — easier text editing.",
            Description =
                "Sets FollowCaret=1 in ScreenMagnifier settings. The magnified view tracks "
                + "the text insertion caret as the user types or navigates within text fields. "
                + "Default: 1. Explicit setting ensures consistent caret tracking.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "FollowCaret", 1)],
            RemoveOps = [RegOp.DeleteValue(MagKey, "FollowCaret")],
            DetectOps = [RegOp.CheckDword(MagKey, "FollowCaret", 1)],
        },
        new TweakDef
        {
            Id = "magnif-set-fullscreen-mode",
            Label = "Magnifier: Use Full-Screen Magnification Mode",
            Category = "Accessibility",
            Tags = ["magnifier", "fullscreen", "mode", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Full-screen mode magnifies the entire display — maximum coverage.",
            Description =
                "Sets MagnificationMode=1 in ScreenMagnifier settings. Switches Magnifier "
                + "to full-screen mode (Mode=1). Other modes: 0=lens, 2=docked. "
                + "Default: 1 (full screen). Explicit enforcement.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "MagnificationMode", 1)],
            RemoveOps = [RegOp.DeleteValue(MagKey, "MagnificationMode")],
            DetectOps = [RegOp.CheckDword(MagKey, "MagnificationMode", 1)],
        },
        new TweakDef
        {
            Id = "magnif-set-lens-mode",
            Label = "Magnifier: Use Lens Magnification Mode (Floating Lens)",
            Category = "Accessibility",
            Tags = ["magnifier", "lens", "mode", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Lens mode creates a moveable magnified window following the cursor.",
            Description =
                "Sets MagnificationMode=0 in ScreenMagnifier settings. Switches Magnifier "
                + "to lens mode where a floating magnifier window follows the pointer. "
                + "Useful when users want to inspect specific areas without zooming everything.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "MagnificationMode", 0)],
            RemoveOps = [RegOp.SetDword(MagKey, "MagnificationMode", 1)],
            DetectOps = [RegOp.CheckDword(MagKey, "MagnificationMode", 0)],
        },
        new TweakDef
        {
            Id = "magnif-set-docked-mode",
            Label = "Magnifier: Use Docked Magnification Mode",
            Category = "Accessibility",
            Tags = ["magnifier", "docked", "mode", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Docked mode reserves a permanent area of the screen for the magnified view.",
            Description =
                "Sets MagnificationMode=2 in ScreenMagnifier settings. Switches Magnifier "
                + "to docked mode where it occupies a fixed panel at the top of the screen. "
                + "Leaves the rest of the screen unmagnified while the panel tracks activity.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "MagnificationMode", 2)],
            RemoveOps = [RegOp.SetDword(MagKey, "MagnificationMode", 1)],
            DetectOps = [RegOp.CheckDword(MagKey, "MagnificationMode", 2)],
        },
        new TweakDef
        {
            Id = "magnif-enable-start-minimised",
            Label = "Magnifier: Start Minimised to System Tray",
            Category = "Accessibility",
            Tags = ["magnifier", "startup", "minimise", "tray", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Magnifier runs in background on login without covering the screen.",
            Description =
                "Sets StartMinimized=1 in ScreenMagnifier settings. When Magnifier is "
                + "configured to start automatically at login, it will start minimised to the "
                + "system tray rather than showing a full-screen magnified view immediately.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "StartMinimized", 1)],
            RemoveOps = [RegOp.SetDword(MagKey, "StartMinimized", 0)],
            DetectOps = [RegOp.CheckDword(MagKey, "StartMinimized", 1)],
        },
        new TweakDef
        {
            Id = "magnif-increase-zoom-increment",
            Label = "Magnifier: Increase Zoom Step to 25%",
            Category = "Accessibility",
            Tags = ["magnifier", "zoom", "increment", "step", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Faster zooming with each keyboard shortcut press.",
            Description =
                "Sets ZoomIncrement=25 in ScreenMagnifier settings. The amount magnification "
                + "changes with each Ctrl+Alt+= or Ctrl+Alt+- keypress. Default: 5 (5% per step). "
                + "Setting to 25 allows faster coarse zoom adjustments.",
            RegistryKeys = [MagKey],
            ApplyOps = [RegOp.SetDword(MagKey, "ZoomIncrement", 25)],
            RemoveOps = [RegOp.SetDword(MagKey, "ZoomIncrement", 5)],
            DetectOps = [RegOp.CheckDword(MagKey, "ZoomIncrement", 25)],
        },
    ];
}
