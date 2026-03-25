// RegiLattice.Core — Tweaks/PenWorkspaceGpoPolicy.cs
// Sprint 273: Pen & Workspace Group Policy (10 tweaks)
// Category: "Pen Workspace Policy" | Slug: penws
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PenWorkspace

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PenWorkspaceGpoPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PenWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "penws-disable-pen-workspace",
            Label = "Disable Pen Workspace",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets PenWorkspaceButtonDesiredVisibility=0 in the PenWorkspace policy key. "
                + "Hides the Pen Workspace button from the taskbar and prevents the floating "
                + "Pen Workspace panel from launching. Pen Workspace aggregates Windows Ink, "
                + "Sticky Notes, and Screen Sketch into a sidebar. On devices without a "
                + "pen or stylus this button serves no purpose. "
                + "Default: not set (shown on pen-equipped devices). Recommended: 0.",
            Tags = ["pen", "workspace", "taskbar", "ink", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceButtonDesiredVisibility")],
            DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-above-lock",
            Label = "Disable Pen Workspace Above Lock Screen",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets UserEducationInAboveLockAllowed=0 in the PenWorkspace policy key. "
                + "Prevents the Windows Ink Workspace and associated onboarding prompts "
                + "from appearing on the lock screen. Applications shown above the lock "
                + "screen are accessible without authentication, creating a potential "
                + "information-disclosure or bypass surface. "
                + "Default: 1 (allowed). Recommended: 0 on security-hardened systems.",
            Tags = ["pen", "workspace", "lockscreen", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UserEducationInAboveLockAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "UserEducationInAboveLockAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "UserEducationInAboveLockAllowed", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-touch-keyboard-onboarding",
            Label = "Disable Touch Keyboard Onboarding",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets TouchKeyboardOnboardingAllowed=0 in the PenWorkspace policy key. "
                + "Suppresses the promotional 'Try the new touch keyboard' onboarding banner "
                + "that appears in the touch keyboard session. The banner interrupts "
                + "workflow on tablet form-factor devices and is purely marketing-oriented. "
                + "Default: 1 (shown). Recommended: 0.",
            Tags = ["pen", "touch", "keyboard", "onboarding", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TouchKeyboardOnboardingAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TouchKeyboardOnboardingAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "TouchKeyboardOnboardingAllowed", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-handwriting-panel",
            Label = "Disable Handwriting Panel",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets PenWorkspaceHandwritingEnabled=0 in the PenWorkspace policy key. "
                + "Disables the floating handwriting input panel that appears near text "
                + "fields when a stylus approaches the screen. This panel intercepts "
                + "stylus input before the active application and translates strokes to "
                + "text via Windows Ink. Disabling it may improve stylus performance in "
                + "drawing or annotation applications. Default: 1. Recommended: 0.",
            Tags = ["pen", "handwriting", "ink", "input", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceHandwritingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceHandwritingEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceHandwritingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-workspace-telemetry",
            Label = "Disable Pen Workspace Telemetry",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets PenWorkspaceTelemetryAllowed=0 in the PenWorkspace policy key. "
                + "Stops Windows Ink Workspace from transmitting usage analytics covering "
                + "which Ink apps were launched, pen interaction rates, stylus hardware "
                + "model, and session durations to Microsoft's telemetry pipeline. "
                + "These signals accumulate a detailed device-usage profile. "
                + "Default: 1. Recommended: 0.",
            Tags = ["pen", "workspace", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceTelemetryAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceTelemetryAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceTelemetryAllowed", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-ink-replay",
            Label = "Disable Ink Replay Logging",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets InkReplayEnabled=0 in the PenWorkspace policy key. Disables the "
                + "Windows Ink replay feature that records the full sequence of pen strokes "
                + "so they can be animated back at playback speed. Stroke replay data is "
                + "stored as a journal that fully reconstructs handwritten content and can "
                + "expose sensitive notes or signatures if the device is compromised. "
                + "Default: 1. Recommended: 0.",
            Tags = ["pen", "ink", "replay", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "InkReplayEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "InkReplayEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "InkReplayEnabled", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-pen-promo",
            Label = "Disable Pen Workspace Hardware Promo",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets AllowSuggestedAppsInWindowsInkWorkspace=0 in the PenWorkspace policy "
                + "key. Removes the 'Suggested Apps' section from Windows Ink Workspace "
                + "that promotes pen-optimised Store apps. Suggested apps load metadata "
                + "from the Microsoft Store CDN at every Workspace open, adding network "
                + "latency and transmitting device pen-hardware telemetry. "
                + "Default: 1. Recommended: 0.",
            Tags = ["pen", "workspace", "promo", "store", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSuggestedAppsInWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-dictation",
            Label = "Disable Ink Dictation Button",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets AllowWindowsInkWorkspace=0 via the AllowWindowsInkWorkspaceValue "
                + "policy in the PenWorkspace policy key. Removes the microphone-dictation "
                + "shortcut button from the touch keyboard and handwriting panel, preventing "
                + "accidental activation of speech input that streams audio to the Windows "
                + "speech recognition service. "
                + "Default: 2 (only above lock). Recommended: 0.",
            Tags = ["pen", "dictation", "voice", "speech", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-sticky-notes-lock",
            Label = "Disable Sticky Notes on Lock Screen",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets StickyNotesOnLockScreenAllowed=0 in the PenWorkspace policy key. "
                + "Prevents Sticky Notes from appearing on the lock screen, which would "
                + "allow anyone near the device to view note content without authentication. "
                + "Users who store passwords, addresses, or meeting details in Sticky Notes "
                + "are particularly exposed by lock-screen visibility. "
                + "Default: 1. Recommended: 0.",
            Tags = ["pen", "stickynotes", "lockscreen", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "StickyNotesOnLockScreenAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "StickyNotesOnLockScreenAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "StickyNotesOnLockScreenAllowed", 0)],
        },
        new TweakDef
        {
            Id = "penws-disable-pencil-button-shortcut",
            Label = "Disable Pen Button Shortcut to Workspace",
            Category = "Pen Workspace Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets PenButtonDesiredAction=2 in the PenWorkspace policy key. Changes the "
                + "pen barrel-button shortcut from launching Windows Ink Workspace (default) "
                + "to a no-op action, preventing accidental Workspace activations while "
                + "drawing in design and annotation applications. Setting value 2 disables "
                + "the button's system action entirely, leaving it for application-defined "
                + "handling. Default: not set. Recommended: 2 on professional artist workstations.",
            Tags = ["pen", "button", "shortcut", "workspace", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PenButtonDesiredAction", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "PenButtonDesiredAction")],
            DetectOps = [RegOp.CheckDword(Key, "PenButtonDesiredAction", 2)],
        },
    ];
}
