// RegiLattice.Core — Tweaks/VirtualKeyboardPolicy.cs
// Sprint 279: Virtual Keyboard Group Policy (10 tweaks)
// Category: "Virtual Keyboard Policy" | Slug: vkbd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VirtualKeyboard

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VirtualKeyboardPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VirtualKeyboard";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vkbd-disable-touch-keyboard",
            Label = "Disable Automatic Touch Keyboard Pop-Up",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableTouchKeyboard=1 in the VirtualKeyboard policy key. Prevents "
                + "the Windows touch keyboard from appearing automatically when the user "
                + "taps on a text input field in tablet mode or when no physical keyboard "
                + "is detected. On hybrid devices used in docked/keyboard mode the "
                + "automatic pop-up interrupts workflows and requires manual dismissal. "
                + "Default: 0. Recommended: 1 on non-tablet machines.",
            Tags = ["touch", "keyboard", "virtual", "tablet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTouchKeyboard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTouchKeyboard")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTouchKeyboard", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-emoji-panel",
            Label = "Disable Emoji Panel (Win+.)",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableEmojiPanel=1 in the VirtualKeyboard policy key. Removes the "
                + "emoji and special-characters picker that opens via Windows + period (.) "
                + "or Windows + semicolon (;). On production workstations the emoji panel "
                + "is an unnecessary distraction; the keyboard shortcut is easily triggered "
                + "accidentally during fast typing. "
                + "Default: 0. Recommended: 1.",
            Tags = ["emoji", "panel", "keyboard", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEmojiPanel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEmojiPanel")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEmojiPanel", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-keyboard-sound",
            Label = "Disable Virtual Keyboard Key Click Sound",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets DisableKeyboardSound=1 in the VirtualKeyboard policy key. Mutes the "
                + "click sound effect played each time a key on the on-screen touch keyboard "
                + "is pressed. In quiet office or conference environments the click sounds "
                + "are disruptive; the system-wide policy prevents users from re-enabling "
                + "them. Default: 0. Recommended: 1.",
            Tags = ["keyboard", "sound", "click", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardSound", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardSound")],
            DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardSound", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-handwriting-button",
            Label = "Disable Touch Keyboard Handwriting Button",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableHandwritingButton=1 in the VirtualKeyboard policy key. Removes "
                + "the stylus/pen button from the touch keyboard toolbar that switches "
                + "from the key grid to the free-form handwriting input mode. On devices "
                + "without a digitiser pen, the button serves no purpose and confuses "
                + "users who activate it by mistake. "
                + "Default: 0. Recommended: 1.",
            Tags = ["keyboard", "handwriting", "button", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHandwritingButton", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHandwritingButton")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHandwritingButton", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-keyboard-telemetry",
            Label = "Disable Virtual Keyboard Telemetry",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableKeyboardTelemetry=1 in the VirtualKeyboard policy key. Stops "
                + "the touch keyboard from reporting usage statistics including layout "
                + "preference, session duration, and interaction rates to Microsoft's "
                + "telemetry pipeline. Keyboard telemetry is collected continuously and "
                + "contributes to the same diagnostic data pipeline as other Windows "
                + "telemetry even when the user has opted out. "
                + "Default: 0. Recommended: 1.",
            Tags = ["keyboard", "telemetry", "privacy", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-fullscreen-keyboard",
            Label = "Disable Full-Screen Keyboard in Desktop Apps",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableFullScreenKeyboard=1 in the VirtualKeyboard policy key. "
                + "Prevents the touch keyboard from expanding to a full-screen mode when "
                + "a text field gains focus in a desktop (Win32) application. Full-screen "
                + "keyboard mode obscures the application window entirely and requires "
                + "manual collapse, disrupting productivity on hybrid devices. "
                + "Default: 0. Recommended: 1.",
            Tags = ["keyboard", "fullscreen", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFullScreenKeyboard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFullScreenKeyboard")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFullScreenKeyboard", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-keyboard-animations",
            Label = "Disable Touch Keyboard Animations",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets DisableKeyboardAnimations=1 in the VirtualKeyboard policy key. "
                + "Removes the slide and fade animations for touch keyboard show/hide "
                + "transitions. On lower-end hardware or at high refresh rates the "
                + "animation frame budget competes with foreground application rendering. "
                + "Removing animations also improves perceived keyboard responsiveness. "
                + "Default: 0. Recommended: 1.",
            Tags = ["keyboard", "animation", "performance", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardAnimations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardAnimations")],
            DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardAnimations", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-voice-dictation-key",
            Label = "Disable Voice Dictation Key on Touch Keyboard",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableVoiceDictationKey=1 in the VirtualKeyboard policy key. Removes "
                + "the microphone button from the touch keyboard that activates the Windows "
                + "voice dictation mode. Voice dictation streams audio to the Windows "
                + "speech recognition service; disabling the toolbar button prevents "
                + "unintentional activation in environments where microphone use is "
                + "restricted. Default: 0. Recommended: 1.",
            Tags = ["keyboard", "voice", "dictation", "microphone", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVoiceDictationKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceDictationKey")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVoiceDictationKey", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-split-keyboard",
            Label = "Disable Split Touch Keyboard Mode",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets DisableSplitKeyboard=1 in the VirtualKeyboard policy key. Disables "
                + "the split-keyboard layout that separates the keyboard into two thumb-"
                + "typing halves at the screen edges. On non-tablet devices the split "
                + "keyboard is an unneeded variant that users may accidentally activate via "
                + "the keyboard settings menu, requiring manual restoration. "
                + "Default: 0. Recommended: 1 on non-tablet form factors.",
            Tags = ["keyboard", "split", "tablet", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSplitKeyboard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSplitKeyboard")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSplitKeyboard", 1)],
        },
        new TweakDef
        {
            Id = "vkbd-disable-wide-keyboard",
            Label = "Disable Wide Touch Keyboard Layout",
            Category = "Virtual Keyboard Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets DisableWideKeyboard=1 in the VirtualKeyboard policy key. Removes "
                + "the wide (full-width undocked) touch keyboard variant from the layout "
                + "picker. The wide layout is designed for Surface-style devices lying flat; "
                + "on conventional desktops it covers most of the screen without a "
                + "productivity benefit. Removing the option simplifies the layout menu. "
                + "Default: 0. Recommended: 1.",
            Tags = ["keyboard", "wide", "layout", "virtual", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWideKeyboard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWideKeyboard")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWideKeyboard", 1)],
        },
    ];
}
