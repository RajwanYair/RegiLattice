#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class TabletPcInputPolicy
{
    private const string TabPC = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC";
    private const string TabWin = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tabpol-prevent-handwriting-data-sharing",
            Label = "Prevent Handwriting Data Sharing with Microsoft",
            Category = "Tablet PC & Input Policy",
            Description = "Prevents Windows from sharing handwriting recognition data with Microsoft to improve handwriting recognition.",
            Tags = ["tablet", "privacy", "handwriting", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabPC, "PreventHandwritingDataSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(TabPC, "PreventHandwritingDataSharing")],
            DetectOps = [RegOp.CheckDword(TabPC, "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-prevent-handwriting-error-reports",
            Label = "Prevent Handwriting Error Reporting",
            Category = "Tablet PC & Input Policy",
            Description = "Stops Windows from sending handwriting recognition error reports to Microsoft.",
            Tags = ["tablet", "privacy", "handwriting", "group-policy", "telemetry"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabPC, "PreventHandwritingErrorReports", 1)],
            RemoveOps = [RegOp.DeleteValue(TabPC, "PreventHandwritingErrorReports")],
            DetectOps = [RegOp.CheckDword(TabPC, "PreventHandwritingErrorReports", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-disable-ink-ball-game",
            Label = "Disable InkBall Game",
            Category = "Tablet PC & Input Policy",
            Description = "Removes the InkBall game from the Start menu and blocks access via Group Policy.",
            Tags = ["tablet", "debloat", "group-policy", "games"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabPC, "DisableInkBall", 1)],
            RemoveOps = [RegOp.DeleteValue(TabPC, "DisableInkBall")],
            DetectOps = [RegOp.CheckDword(TabPC, "DisableInkBall", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-turn-off-passwordless",
            Label = "Turn Off Tablet PC Passwordless Experience",
            Category = "Tablet PC & Input Policy",
            Description = "Disables the passwordless login experience on Tablet PC, requiring a password for sign-in.",
            Tags = ["tablet", "security", "group-policy", "login"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabPC, "TurnOffPwdlessExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(TabPC, "TurnOffPwdlessExperience")],
            DetectOps = [RegOp.CheckDword(TabPC, "TurnOffPwdlessExperience", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-prevent-handwriting-personalization",
            Label = "Prevent Handwriting Personalization Collection",
            Category = "Tablet PC & Input Policy",
            Description = "Blocks Windows from collecting typed and handwriting data to build a personalized dictionary for handwriting recognition.",
            Tags = ["tablet", "privacy", "handwriting", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabWin, "PreventHandwritingPersonalization", 1)],
            RemoveOps = [RegOp.DeleteValue(TabWin, "PreventHandwritingPersonalization")],
            DetectOps = [RegOp.CheckDword(TabWin, "PreventHandwritingPersonalization", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-disable-pen-training-support",
            Label = "Disable Pen Training and Support",
            Category = "Tablet PC & Input Policy",
            Description = "Turns off the Tablet PC pen training and pen support documentation from the Tablet PC optional components.",
            Tags = ["tablet", "debloat", "group-policy", "pen"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabWin, "DisablePenTrainingAndSupport", 1)],
            RemoveOps = [RegOp.DeleteValue(TabWin, "DisablePenTrainingAndSupport")],
            DetectOps = [RegOp.CheckDword(TabWin, "DisablePenTrainingAndSupport", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-turn-off-pen-feedback",
            Label = "Turn Off Pen Haptic Feedback",
            Category = "Tablet PC & Input Policy",
            Description = "Disables haptic and visual ink feedback when using a digital pen on a touch display.",
            Tags = ["tablet", "pen", "group-policy", "input"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabWin, "TurnOffPenFeedback", 1)],
            RemoveOps = [RegOp.DeleteValue(TabWin, "TurnOffPenFeedback")],
            DetectOps = [RegOp.CheckDword(TabWin, "TurnOffPenFeedback", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-disable-touch-input",
            Label = "Disable Touch Input (Tablet PC Policy)",
            Category = "Tablet PC & Input Policy",
            Description = "Disables all touch-based input processing via Group Policy. Useful for kiosk or hardened deployments without touch screens.",
            Tags = ["tablet", "touch", "group-policy", "input", "kiosk"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(TabWin, "DisableTouchInput", 1)],
            RemoveOps = [RegOp.DeleteValue(TabWin, "DisableTouchInput")],
            DetectOps = [RegOp.CheckDword(TabWin, "DisableTouchInput", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-disable-touchscreen-scroll",
            Label = "Disable Touchscreen Panning and Scrolling Inertia",
            Category = "Tablet PC & Input Policy",
            Description = "Disables momentum scrolling (inertia) and panning on touchscreens to reduce accidental scrolling in productivity apps.",
            Tags = ["tablet", "touch", "group-policy", "input"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabWin, "DisablePanningFeedback", 1)],
            RemoveOps = [RegOp.DeleteValue(TabWin, "DisablePanningFeedback")],
            DetectOps = [RegOp.CheckDword(TabWin, "DisablePanningFeedback", 1)],
        },
        new TweakDef
        {
            Id = "tabpol-disable-flick-gestures",
            Label = "Disable Pen and Touch Flick Gestures",
            Category = "Tablet PC & Input Policy",
            Description = "Disables pen and touch flick gestures (quick swipe shortcuts) system-wide via Group Policy.",
            Tags = ["tablet", "touch", "pen", "group-policy", "input"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(TabWin, "DisableFlicksFeature", 1)],
            RemoveOps = [RegOp.DeleteValue(TabWin, "DisableFlicksFeature")],
            DetectOps = [RegOp.CheckDword(TabWin, "DisableFlicksFeature", 1)],
        },
    ];
}
