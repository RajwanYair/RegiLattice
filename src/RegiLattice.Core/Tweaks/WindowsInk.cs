namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class WindowsInk
{
    private const string InkWorkspace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    private const string InkUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace";

    private const string PenSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen";

    private const string InkPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC";

    private const string SuggestionPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

    private const string CursorFeedback = @"HKEY_CURRENT_USER\Control Panel\Cursors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ink-disable-handwriting-panel",
            Label = "Disable Touch Keyboard / Handwriting Panel Auto-Show",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "handwriting", "touch keyboard", "panel"],
            Description =
                "Disables the touch keyboard and handwriting panel from automatically "
                + "appearing when a text field is tapped. Prevents TABLETPC features "
                + "from activating on non-tablet devices.",
            ApplyOps = [RegOp.SetDword(InkPolicy, "DisableHandwritingPanel", 1)],
            RemoveOps = [RegOp.DeleteValue(InkPolicy, "DisableHandwritingPanel")],
            DetectOps = [RegOp.CheckDword(InkPolicy, "DisableHandwritingPanel", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-text-prediction",
            Label = "Disable Ink Text Prediction and Recommendations",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "text prediction", "recommendations", "privacy"],
            Description =
                "Disables text prediction and autocorrect suggestions in the Windows "
                + "handwriting and ink text input panel. Keeps ink recognition "
                + "as-is without real-time suggestions.",
            ApplyOps = [RegOp.SetDword(PenSettings, "TextInputLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(PenSettings, "TextInputLocked")],
            DetectOps = [RegOp.CheckDword(PenSettings, "TextInputLocked", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-flicks",
            Label = "Disable Pen Flicks (Swipe Gestures)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "flicks", "gestures", "pen", "swipe"],
            Description =
                "Disables pen flick gestures (quick swipe actions that trigger scroll, "
                + "delete, copy etc.). Prevents accidental flick detection when using "
                + "a stylus for precision drawing or writing.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Mouse", "FlicksDisabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Mouse", "FlicksDisabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Mouse", "FlicksDisabled", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-press-and-hold",
            Label = "Disable Pen Press-and-Hold for Right-Click",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "press and hold", "right-click", "stylus"],
            Description =
                "Disables the press-and-hold gesture that triggers a right-click when "
                + "using a stylus. Useful for artists and designers who hold the pen tip "
                + "on the canvas for extended periods without wanting a context menu.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "DisablePressAndHold", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "DisablePressAndHold", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "DisablePressAndHold", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-learn-from-this-device",
            Label = "Disable 'Learn from This Device' for Input Personalization",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "personalization", "learn", "inking", "privacy"],
            Description =
                "Disables the 'Get to know me' input personalization feature that "
                + "builds a personal inking and typing model from your inputs. "
                + "Prevents local model accumulation and cloud sync.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-visual-feedback",
            Label = "Disable Pen Visual Feedback",
            Category = "Peripherals — Virtual Disk Service",
            Description =
                "Removes the cursor/halo effects shown when using a stylus. "
                + "Useful on high-DPI tablets where the glow overlay obscures fine linework.",
            Tags = ["pen", "stylus", "visual", "tablet"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(CursorFeedback, "PenVisualization", 0)],
            RemoveOps = [RegOp.SetDword(CursorFeedback, "PenVisualization", 1)],
            DetectOps = [RegOp.CheckDword(CursorFeedback, "PenVisualization", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-workspace-app-launch",
            Label = "Block App Launches from Pen Workspace",
            Category = "Peripherals — Virtual Disk Service",
            Description =
                "Disables the ability to launch apps from the Pen Workspace panel. "
                + "Reduces attack surface on shared devices where pen-launched apps should be restricted.",
            Tags = ["pen", "workspace", "launch", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(InkUser, "PenWorkspaceAppLaunchAllowed", 0)],
            RemoveOps = [RegOp.SetDword(InkUser, "PenWorkspaceAppLaunchAllowed", 1)],
            DetectOps = [RegOp.CheckDword(InkUser, "PenWorkspaceAppLaunchAllowed", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-right-click-hold",
            Label = "Disable Pen Press-and-Hold for Right-Click",
            Category = "Peripherals — Virtual Disk Service",
            Description =
                "Disables the 'press and hold' pen gesture that simulates a right-click. "
                + "Prevents accidental context menus when resting the pen on the screen.",
            Tags = ["pen", "gesture", "right-click", "tablet"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch", "TouchMode_hold", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch", "TouchMode_hold")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch", "TouchMode_hold", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-tablet-ink-policy",
            Label = "Disable Ink Programs via Group Policy",
            Category = "Peripherals — Virtual Disk Service",
            Description =
                "Applies the TabletPC policy to disable InkBall and other built-in ink-based games/apps. "
                + "Recommended for corporate tablets to prevent access to entertainment apps.",
            Tags = ["pen", "tablet", "policy", "games"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(InkPolicy, "DisableInkball", 1)],
            RemoveOps = [RegOp.DeleteValue(InkPolicy, "DisableInkball")],
            DetectOps = [RegOp.CheckDword(InkPolicy, "DisableInkball", 1)],
        },
        new TweakDef
        {
            Id = "ink-set-pen-double-tap-speed",
            Label = "Set Pen Double-Tap Speed to 400 ms",
            Category = "Peripherals — Virtual Disk Service",
            Description =
                "Increases the pen double-tap recognition window to 400 ms (default 200 ms). "
                + "Helps users with motor impairment or those using thick-nib styluses to register double-taps reliably.",
            Tags = ["pen", "accessibility", "double-tap", "input"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(PenSettings, "DoubleTapTime", 400)],
            RemoveOps = [RegOp.DeleteValue(PenSettings, "DoubleTapTime")],
            DetectOps = [RegOp.CheckDword(PenSettings, "DoubleTapTime", 400)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-customization-page",
            Label = "Disable Pen Customization Settings Page",
            Category = "Peripherals — Virtual Disk Service",
            Description =
                "Hides the Pen Customization settings page from the Settings app via Group Policy. "
                + "Prevents users from rebinding pen buttons on managed/shared devices.",
            Tags = ["pen", "policy", "settings", "managed"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(InkPolicy, "DisablePenCustomization", 1)],
            RemoveOps = [RegOp.DeleteValue(InkPolicy, "DisablePenCustomization")],
            DetectOps = [RegOp.CheckDword(InkPolicy, "DisablePenCustomization", 1)],
        },
    ];
}
