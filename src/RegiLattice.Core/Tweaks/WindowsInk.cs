// RegiLattice.Core — Tweaks/WindowsInk.cs
// Windows Ink Workspace and digital pen/stylus integration settings (Sprint 92).
// Slug "ink" — HKCU Windows Ink Workspace and pen suggestion settings.
// Distinct from Touch.cs (touch input) and Input.cs (keyboard/mouse basics).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsInk
{
    private const string InkWorkspace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    private const string InkUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace";

    private const string PenSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen";

    private const string InkPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC";

    private const string SuggestionPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ink-disable-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Windows Ink",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "workspace", "pen", "stylus", "disable"],
            Description =
                "Disables the Windows Ink Workspace button in the system tray and the "
                + "entire Ink Workspace feature. AllowWindowsInkWorkspace=0. "
                + "Useful on non-pen devices where it just adds clutter.",
            ApplyOps = [RegOp.SetDword(InkWorkspace, "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(InkWorkspace, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(InkWorkspace, "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-workspace-above-lock",
            Label = "Disable Windows Ink Workspace Access Above Lock Screen",
            Category = "Windows Ink",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["ink", "workspace", "lock screen", "security"],
            Description =
                "Prevents the Windows Ink Workspace from being opened from the lock screen "
                + "(AllowWindowsInkWorkspace=1, accessible above lock=no). "
                + "Enforces that ink features require a signed-in session.",
            ApplyOps = [RegOp.SetDword(InkWorkspace, "AllowWindowsInkWorkspace", 1)],
            RemoveOps = [RegOp.DeleteValue(InkWorkspace, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(InkWorkspace, "AllowWindowsInkWorkspace", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-workspace-button",
            Label = "Disable Pen and Ink Workspace Taskbar Button",
            Category = "Windows Ink",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "taskbar", "pen button", "ui"],
            Description = "Hides the Pen Workspace button from the Windows taskbar notification area. " + "PenWorkspaceButtonDesiredVisibility=0.",
            ApplyOps = [RegOp.SetDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.SetDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 1)],
            DetectOps = [RegOp.CheckDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-handwriting-panel",
            Label = "Disable Touch Keyboard / Handwriting Panel Auto-Show",
            Category = "Windows Ink",
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
            Id = "ink-disable-ink-personalization",
            Label = "Disable Ink Personalization Data Collection",
            Category = "Windows Ink",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "personalization", "privacy", "telemetry"],
            Description =
                "Prevents Windows from collecting handwriting and ink input samples "
                + "for improving the recognition engine. "
                + "RestrictImplicitInkCollection=1.",
            ApplyOps = [RegOp.SetDword(SuggestionPolicy, "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(SuggestionPolicy, "RestrictImplicitInkCollection")],
            DetectOps = [RegOp.CheckDword(SuggestionPolicy, "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-text-prediction",
            Label = "Disable Ink Text Prediction and Recommendations",
            Category = "Windows Ink",
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
            Category = "Windows Ink",
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
            Category = "Windows Ink",
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
            Id = "ink-disable-typing-data-collection",
            Label = "Disable Typing and Text Input Data Collection",
            Category = "Windows Ink",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "typing data", "privacy", "text input", "collection"],
            Description =
                "Prevents Windows from collecting typing and text input data to improve "
                + "personalised features. RestrictImplicitTextCollection=1 via policy.",
            ApplyOps = [RegOp.SetDword(SuggestionPolicy, "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(SuggestionPolicy, "RestrictImplicitTextCollection")],
            DetectOps = [RegOp.CheckDword(SuggestionPolicy, "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-learn-from-this-device",
            Label = "Disable 'Learn from This Device' for Input Personalization",
            Category = "Windows Ink",
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
    ];
}
