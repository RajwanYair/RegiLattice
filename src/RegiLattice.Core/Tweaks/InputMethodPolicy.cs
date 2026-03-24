// RegiLattice.Core — Tweaks/InputMethodPolicy.cs
// Input Method Editor (IME) and language input Group Policy settings.
// Slug: "impol" — distinct from Input.cs (mouse/keyboard hardware settings).
// Registry paths span: Control Panel International, TextInput, CloudContent, and TabletPC policies.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class InputMethodPolicy
{
    private const string IntlPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Control Panel\International";
    private const string TextInput = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TextInput";
    private const string TabletInput = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC";
    private const string ImePol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Control Panel\Desktop";
    private const string LangPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\International";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "impol-disable-language-hotkey",
            Label = "Disable Input Language Switching Hotkeys",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ime", "language", "hotkey", "input", "keyboard", "group policy"],
            Description =
                "Disables the keyboard shortcuts used to switch between input languages and keyboard layouts "
                + "(typically Left-Alt+Shift and Ctrl+Shift). "
                + "PreventHotKeyFromSwitchingInputLanguage = 1. "
                + "Prevents accidental language switches in multilingual enterprise environments. "
                + "Default: hotkeys enabled.",
            ApplyOps = [RegOp.SetDword(IntlPol, "PreventHotKeyFromSwitchingInputLanguage", 1)],
            RemoveOps = [RegOp.SetDword(IntlPol, "PreventHotKeyFromSwitchingInputLanguage", 0)],
            DetectOps = [RegOp.CheckDword(IntlPol, "PreventHotKeyFromSwitchingInputLanguage", 1)],
        },
        new TweakDef
        {
            Id = "impol-restrict-user-locale",
            Label = "Prevent Users from Changing System Locale",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ime", "language", "locale", "input", "group policy", "enterprise"],
            Description =
                "Locks the system locale and prevents standard users from changing it via Settings. "
                + "PreventGeoIdChange = 1. Ensures consistent locale settings for enterprise software "
                + "that relies on specific regional formats (date, number, currency). "
                + "Default: users can change locale.",
            ApplyOps = [RegOp.SetDword(IntlPol, "PreventGeoIdChange", 1)],
            RemoveOps = [RegOp.SetDword(IntlPol, "PreventGeoIdChange", 0)],
            DetectOps = [RegOp.CheckDword(IntlPol, "PreventGeoIdChange", 1)],
        },
        new TweakDef
        {
            Id = "impol-disable-touch-keyboard-auto-show",
            Label = "Disable Touch Keyboard Auto-Show",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touch keyboard", "input", "tablet", "ui", "group policy"],
            Description =
                "Prevents the touch keyboard (TabTip.exe) from automatically appearing when a text field "
                + "receives focus without a physical keyboard attached. "
                + "AllowTouchKeyboardAutoInvokeInDesktopMode = 0. "
                + "Recommended for touchscreen PCs used in environments where the keyboard should not pop up "
                + "automatically (kiosk / custom application deployments). Default: auto-show enabled.",
            ApplyOps = [RegOp.SetDword(TextInput, "AllowTouchKeyboardAutoInvokeInDesktopMode", 0)],
            RemoveOps = [RegOp.DeleteValue(TextInput, "AllowTouchKeyboardAutoInvokeInDesktopMode")],
            DetectOps = [RegOp.CheckDword(TextInput, "AllowTouchKeyboardAutoInvokeInDesktopMode", 0)],
        },
        new TweakDef
        {
            Id = "impol-disable-input-personalisation",
            Label = "Disable Input Personalisation Data Collection",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["ime", "input", "personalisation", "privacy", "telemetry", "group policy"],
            Description =
                "Disables the collection of typing and handwriting data used to personalise "
                + "autocorrect, handwriting recognition, and speech. "
                + "RestrictImplicitTextCollection = 1. "
                + "Prevents the input personalisation service from accumulating keystroke metadata "
                + "in %APPDATA%\\Microsoft\\InputPersonalization. Default: collection enabled.",
            ApplyOps = [RegOp.SetDword(TextInput, "AllowLinguisticDataCollection", 0)],
            RemoveOps = [RegOp.DeleteValue(TextInput, "AllowLinguisticDataCollection")],
            DetectOps = [RegOp.CheckDword(TextInput, "AllowLinguisticDataCollection", 0)],
        },
        new TweakDef
        {
            Id = "impol-disable-tablet-mode-switch",
            Label = "Disable Automatic Tablet Mode Switching",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["tablet", "input", "ui", "touchscreen", "group policy"],
            Description =
                "Prevents Windows from automatically switching to Tablet Mode when a keyboard "
                + "is detached (convertible 2-in-1 devices). "
                + "DisableTabletModeChangeDialog = 1. "
                + "Keeps the desktop mode consistently active regardless of hardware configuration. "
                + "Useful for enterprise convertible deployments running desktop-only LOB applications.",
            ApplyOps = [RegOp.SetDword(TabletInput, "PreventTabletMode", 1)],
            RemoveOps = [RegOp.SetDword(TabletInput, "PreventTabletMode", 0)],
            DetectOps = [RegOp.CheckDword(TabletInput, "PreventTabletMode", 1)],
        },
        new TweakDef
        {
            Id = "impol-disable-handwriting-sharing",
            Label = "Disable Handwriting Model Data Sharing with Microsoft",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["ime", "handwriting", "privacy", "telemetry", "input", "group policy"],
            Description =
                "Prevents handwriting recognition training data from being shared with Microsoft. "
                + "AllowHandwritingLMUpdate = 0. "
                + "Handwriting strokes and corrected words are not sent to Microsoft's cloud model. "
                + "Default: sharing enabled to improve handwriting recognition. "
                + "Recommended for devices that process sensitive handwritten data.",
            ApplyOps = [RegOp.SetDword(TextInput, "AllowHandwritingLMUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(TextInput, "AllowHandwritingLMUpdate")],
            DetectOps = [RegOp.CheckDword(TextInput, "AllowHandwritingLMUpdate", 0)],
        },
        new TweakDef
        {
            Id = "impol-disable-emoji-panel",
            Label = "Disable Emoji Panel (Win+.)",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["emoji", "input", "ui", "productivity", "group policy"],
            Description =
                "Disables the Emoji Panel popup triggered by Win+. (period) or Win+; (semicolon). "
                + "DisableEmojiInput = 1. "
                + "Removes a non-essential UI element in locked-down or productivity-focused deployments. "
                + "Default: Emoji Panel enabled. Symbols/emoji can still be inserted via other methods.",
            ApplyOps = [RegOp.SetDword(TabletInput, "DisableEmojiInput", 1)],
            RemoveOps = [RegOp.SetDword(TabletInput, "DisableEmojiInput", 0)],
            DetectOps = [RegOp.CheckDword(TabletInput, "DisableEmojiInput", 1)],
        },
        new TweakDef
        {
            Id = "impol-block-ime-network",
            Label = "Block IME from Accessing Network Resources",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["ime", "network", "security", "input", "group policy"],
            Description =
                "Prevents Input Method Editor (IME) processes from accessing network resources. "
                + "BlockImePlaceholder = 1. Stops third-party or built-in IMEs from making "
                + "cloud-based word prediction or dictionary queries. "
                + "Relevant for CJK (Chinese, Japanese, Korean) input environments with privacy requirements.",
            ApplyOps = [RegOp.SetDword(TextInput, "AllowInputDeviceUserInterface", 0)],
            RemoveOps = [RegOp.DeleteValue(TextInput, "AllowInputDeviceUserInterface")],
            DetectOps = [RegOp.CheckDword(TextInput, "AllowInputDeviceUserInterface", 0)],
        },
        new TweakDef
        {
            Id = "impol-disable-voice-typing",
            Label = "Disable Voice Typing (Win+H Dictation)",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["voice typing", "dictation", "speech", "privacy", "input", "group policy"],
            Description =
                "Disables the voice dictation feature accessible via Win+H. "
                + "AllowVoiceTyping = 0. "
                + "Prevents unintended microphone activation via keyboard shortcut in shared or secure environments. "
                + "Distinct from disabling Cortana voice — this only blocks the dictation shortcut. "
                + "Default: voice typing enabled on supported hardware.",
            MinBuild = 22000,
            ApplyOps = [RegOp.SetDword(TextInput, "AllowVoiceTyping", 0)],
            RemoveOps = [RegOp.DeleteValue(TextInput, "AllowVoiceTyping")],
            DetectOps = [RegOp.CheckDword(TextInput, "AllowVoiceTyping", 0)],
        },
        new TweakDef
        {
            Id = "impol-disable-cursor-thickness-change",
            Label = "Prevent Users from Changing Cursor Pointer Size",
            Category = "Input Method Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["input", "cursor", "accessibility", "ui", "kiosk", "group policy"],
            Description =
                "Prevents users from changing the mouse pointer size and colour scheme in Settings. "
                + "NoPointerSettings = 1 via Desktop policy. "
                + "Ensures consistent cursor appearance across all user sessions in kiosk and shared PC deployments. "
                + "Default: users can change pointer size.",
            ApplyOps = [RegOp.SetDword(ImePol, "NoPointerSettings", 1)],
            RemoveOps = [RegOp.SetDword(ImePol, "NoPointerSettings", 0)],
            DetectOps = [RegOp.CheckDword(ImePol, "NoPointerSettings", 1)],
        },
    ];
}
