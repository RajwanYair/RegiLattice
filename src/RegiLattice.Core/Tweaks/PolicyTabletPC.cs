namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyTabletPC
{
    private const string TabletKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC";
    private const string HandwritingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tablet-disable-touch-input",
            Label = "Disable Touch Input (Policy)",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableTouchInput=1 in TabletPC policy. Disables touchscreen input at the "
                + "system level, preventing accidental touch interactions on devices used as "
                + "traditional desktops or kiosks.",
            Tags = ["tablet", "touch", "input", "policy", "kiosk"],
            RegistryKeys = [TabletKey],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Touch input completely disabled; use mouse/keyboard only.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisableTouchInput", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisableTouchInput")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisableTouchInput", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-flicks",
            Label = "Disable Pen Flicks (Policy)",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableFlicks=1 in TabletPC policy. Disables pen flick gestures (quick pen "
                + "strokes that trigger actions like back, forward, scroll). Prevents accidental "
                + "navigation when using the pen for precise drawing or annotation.",
            Tags = ["tablet", "pen", "flicks", "gesture", "policy"],
            RegistryKeys = [TabletKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Pen flick gestures disabled; pen works for drawing/annotation only.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisableFlicks", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisableFlicks")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisableFlicks", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-tip",
            Label = "Disable Tablet Input Panel (TIP)",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableTIP=1 in TabletPC policy. Prevents the Tablet Input Panel (handwriting "
                + "recognition and soft keyboard) from launching automatically when a text field is "
                + "tapped with a pen or finger.",
            Tags = ["tablet", "tip", "input-panel", "handwriting", "policy"],
            RegistryKeys = [TabletKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Tablet Input Panel does not auto-launch; use physical keyboard.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisableTIP", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisableTIP")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisableTIP", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-snipping-tool",
            Label = "Disable Snipping Tool Launch via Pen Button",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSnippingTool=1 in TabletPC policy. Prevents the Snipping Tool from "
                + "launching when the pen button is pressed or a screen-edge gesture is performed. "
                + "Users must open Snipping Tool manually.",
            Tags = ["tablet", "snipping-tool", "pen-button", "policy"],
            RegistryKeys = [TabletKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Pen button no longer launches Snipping Tool; manual launch still available.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisableSnippingTool", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisableSnippingTool")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisableSnippingTool", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-press-and-hold",
            Label = "Disable Press-and-Hold Right-Click",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePressAndHold=1 in TabletPC policy. Disables the press-and-hold gesture "
                + "that triggers a right-click context menu. Reduces accidental right-click events "
                + "when using the pen for drawing applications.",
            Tags = ["tablet", "press-hold", "right-click", "gesture", "policy"],
            RegistryKeys = [TabletKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Press-and-hold right-click disabled; use pen barrel button or keyboard for context menu.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisablePressAndHold", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisablePressAndHold")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisablePressAndHold", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-handwriting-data-sharing",
            Label = "Disable Handwriting Data Sharing",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventHandwritingDataSharing=1 in InputPersonalization policy. Prevents "
                + "Windows from sending handwriting recognition data to Microsoft for improvement. "
                + "Protects handwritten notes and sketches from being uploaded as telemetry.",
            Tags = ["handwriting", "data-sharing", "privacy", "policy", "telemetry"],
            RegistryKeys = [HandwritingKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Handwriting data stays local; recognition accuracy may not improve over time.",
            ApplyOps = [RegOp.SetDword(HandwritingKey, "PreventHandwritingDataSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(HandwritingKey, "PreventHandwritingDataSharing")],
            DetectOps = [RegOp.CheckDword(HandwritingKey, "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-handwriting-error-reports",
            Label = "Disable Handwriting Error Reports",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventHandwritingErrorReports=1 in InputPersonalization policy. Prevents "
                + "Windows from sending handwriting recognition error reports to Microsoft. "
                + "Error reports may contain fragments of handwritten text.",
            Tags = ["handwriting", "error-report", "privacy", "policy"],
            RegistryKeys = [HandwritingKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Handwriting error reports not sent; privacy protected.",
            ApplyOps = [RegOp.SetDword(HandwritingKey, "PreventHandwritingErrorReports", 1)],
            RemoveOps = [RegOp.DeleteValue(HandwritingKey, "PreventHandwritingErrorReports")],
            DetectOps = [RegOp.CheckDword(HandwritingKey, "PreventHandwritingErrorReports", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowWindowsInkWorkspace=0 in the WindowsInkWorkspace policy. Completely "
                + "disables the Windows Ink Workspace, including Sketchpad and Screen Sketch. "
                + "The Ink Workspace icon is removed from the taskbar.",
            Tags = ["ink", "workspace", "pen", "sketchpad", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Windows Ink Workspace disabled; Sketchpad unavailable.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "tablet-disable-pen-feedback",
            Label = "Disable Pen Visual Feedback",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePenFeedback=1 in TabletPC policy. Disables the visual feedback "
                + "animations (ripples, cursors) shown when using a pen on the screen. Slightly "
                + "improves rendering performance and reduces visual distractions.",
            Tags = ["pen", "feedback", "visual", "animation", "policy"],
            RegistryKeys = [TabletKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "No visual pen feedback; pen still works normally for input.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisablePenFeedback", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisablePenFeedback")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisablePenFeedback", 1)],
        },
        new TweakDef
        {
            Id = "tablet-disable-touch-keyboard-auto-invoke",
            Label = "Disable Touch Keyboard Auto-Invoke",
            Category = "Input — Tablet & Pen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableTouchKeyboardAutoInvoke=1 in TabletPC policy. Prevents the touch "
                + "keyboard from automatically appearing when a text field gains focus on a "
                + "touchscreen device. The keyboard can still be opened manually from the taskbar.",
            Tags = ["touch-keyboard", "auto-invoke", "tablet", "policy"],
            RegistryKeys = [TabletKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Touch keyboard does not auto-appear; manual invoke from taskbar still available.",
            ApplyOps = [RegOp.SetDword(TabletKey, "DisableTouchKeyboardAutoInvoke", 1)],
            RemoveOps = [RegOp.DeleteValue(TabletKey, "DisableTouchKeyboardAutoInvoke")],
            DetectOps = [RegOp.CheckDword(TabletKey, "DisableTouchKeyboardAutoInvoke", 1)],
        },
    ];
}
