#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class VoiceAccessControl
{
    private const string VaKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VoiceAccess";
    private const string SrKey = @"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings";
    private const string SrRecKey = @"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation";
    private const string PolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VoiceAccess";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "voiacc-enable-on-login",
            Label = "Voice Access: Start Automatically at Login",
            Category = "Accessibility",
            Tags = ["voice-access", "startup", "login", "accessibility", "win11"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Ensures voice control is available immediately after sign-in.",
            Description =
                "Sets OpenOnLogin=1 in VoiceAccess settings. Configures the Windows 11 "
                + "Voice Access accessibility feature to start automatically when the user "
                + "signs in. Essential for users who rely on voice control as their primary "
                + "input method. Requires Windows 11 22H2 or later.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "OpenOnLogin", 1)],
            RemoveOps = [RegOp.SetDword(VaKey, "OpenOnLogin", 0)],
            DetectOps = [RegOp.CheckDword(VaKey, "OpenOnLogin", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-disable-on-login",
            Label = "Voice Access: Do Not Start at Login",
            Category = "Accessibility",
            Tags = ["voice-access", "startup", "login", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces delayed startup time for users who use voice access occasionally.",
            Description =
                "Sets OpenOnLogin=0 in VoiceAccess settings. Prevents Voice Access from "
                + "starting automatically at login. Users who only use voice access "
                + "occasionally can start it manually to avoid startup overhead.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "OpenOnLogin", 0)],
            RemoveOps = [RegOp.DeleteValue(VaKey, "OpenOnLogin")],
            DetectOps = [RegOp.CheckDword(VaKey, "OpenOnLogin", 0)],
        },
        new TweakDef
        {
            Id = "voiacc-enable-voice-shortcuts",
            Label = "Voice Access: Enable Custom Voice Shortcuts",
            Category = "Accessibility",
            Tags = ["voice-access", "shortcuts", "commands", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Custom shortcuts enable personalised voice commands for frequent actions.",
            Description =
                "Sets CustomCommandsEnabled=1 in VoiceAccess settings. Enables the ability "
                + "to create and use custom voice shortcuts in Windows 11 Voice Access. "
                + "Custom commands can trigger keyboard shortcuts or text phrases. Default: 1.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "CustomCommandsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(VaKey, "CustomCommandsEnabled")],
            DetectOps = [RegOp.CheckDword(VaKey, "CustomCommandsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-show-command-list",
            Label = "Voice Access: Show Available Commands Panel",
            Category = "Accessibility",
            Tags = ["voice-access", "commands", "panel", "accessibility", "ui"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Visible command list helps new users learn available voice commands.",
            Description =
                "Sets ShowCommandList=1 in VoiceAccess settings. Displays the command "
                + "reference panel that lists available Voice Access commands. Useful for "
                + "new users to discover commands while learning the system.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "ShowCommandList", 1)],
            RemoveOps = [RegOp.SetDword(VaKey, "ShowCommandList", 0)],
            DetectOps = [RegOp.CheckDword(VaKey, "ShowCommandList", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-hide-command-list",
            Label = "Voice Access: Hide Commands Panel (More Screen Space)",
            Category = "Accessibility",
            Tags = ["voice-access", "commands", "panel", "accessibility", "ui"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hiding the panel reclaims screen space for experienced voice users.",
            Description =
                "Sets ShowCommandList=0 in VoiceAccess settings. Hides the command "
                + "reference panel. Experienced Voice Access users who know the commands "
                + "can free up screen space by hiding this panel.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "ShowCommandList", 0)],
            RemoveOps = [RegOp.SetDword(VaKey, "ShowCommandList", 1)],
            DetectOps = [RegOp.CheckDword(VaKey, "ShowCommandList", 0)],
        },
        new TweakDef
        {
            Id = "voiacc-enable-hint-labels",
            Label = "Voice Access: Enable Overlay Number Labels on UI Elements",
            Category = "Accessibility",
            Tags = ["voice-access", "hints", "labels", "numbers", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Number labels allow saying a number to click any visible element — very efficient.",
            Description =
                "Sets ShowHints=1 in VoiceAccess settings. Enables the 'show numbers' overlay "
                + "where all interactive elements on screen receive a number. Users can say "
                + "the number to click that element without needing to identify its name.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "ShowHints", 1)],
            RemoveOps = [RegOp.DeleteValue(VaKey, "ShowHints")],
            DetectOps = [RegOp.CheckDword(VaKey, "ShowHints", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-enable-dictation-suggestions",
            Label = "Voice Access: Enable Dictation Autocorrect Suggestions",
            Category = "Accessibility",
            Tags = ["voice-access", "dictation", "autocorrect", "suggestions", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Suggestions reduce transcription errors during voice dictation.",
            Description =
                "Sets DictationSuggestionsEnabled=1 in VoiceAccess settings. Enables "
                + "autocorrect suggestions when dictating text. The system displays "
                + "alternative word interpretations that can be accepted or dismissed by voice.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "DictationSuggestionsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(VaKey, "DictationSuggestionsEnabled")],
            DetectOps = [RegOp.CheckDword(VaKey, "DictationSuggestionsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-enable-wake-word",
            Label = "Voice Access: Enable Wake Word Detection ('Hey Windows')",
            Category = "Accessibility",
            Tags = ["voice-access", "wake-word", "hey-windows", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            SideEffects = "Microphone remains partially active while the session is running to detect the wake word.",
            ImpactNote = "Hands-free activation — say 'Hey Windows' to start voice access.",
            Description =
                "Sets WakeWordEnabled=1 in VoiceAccess settings. Enables wake-word (keyword "
                + "spotting) for Voice Access so that saying 'Hey Windows' activates the "
                + "feature without pressing any buttons. Requires always-on mic listening.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "WakeWordEnabled", 1)],
            RemoveOps = [RegOp.SetDword(VaKey, "WakeWordEnabled", 0)],
            DetectOps = [RegOp.CheckDword(VaKey, "WakeWordEnabled", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-enable-click-indicator",
            Label = "Voice Access: Show Visual Click Indicator",
            Category = "Accessibility",
            Tags = ["voice-access", "click", "indicator", "visual", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Visual click feedback confirms the command was executed correctly.",
            Description =
                "Sets ShowClickIndicator=1 in VoiceAccess settings. Displays a brief "
                + "visual animation whenever Voice Access executes a click command, "
                + "confirming the action was performed. Helps users track their voice "
                + "commands and diagnose mis-clicks.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "ShowClickIndicator", 1)],
            RemoveOps = [RegOp.DeleteValue(VaKey, "ShowClickIndicator")],
            DetectOps = [RegOp.CheckDword(VaKey, "ShowClickIndicator", 1)],
        },
        new TweakDef
        {
            Id = "voiacc-enable-audio-feedback",
            Label = "Voice Access: Enable Audio Confirmation Beeps",
            Category = "Accessibility",
            Tags = ["voice-access", "audio", "beep", "feedback", "accessibility"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Audio cues indicate when voice commands are recognised — useful when screen is not visible.",
            Description =
                "Sets AudioFeedbackEnabled=1 in VoiceAccess settings. Enables short auditory "
                + "confirmation tones when Voice Access recognises and executes a command. "
                + "Useful in environments where the screen is not always in direct view.",
            MinBuild = 22621,
            RegistryKeys = [VaKey],
            ApplyOps = [RegOp.SetDword(VaKey, "AudioFeedbackEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(VaKey, "AudioFeedbackEnabled")],
            DetectOps = [RegOp.CheckDword(VaKey, "AudioFeedbackEnabled", 1)],
        },
    ];
}
