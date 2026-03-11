namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Speech
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "speech-disable-online",
            Label = "Disable Online Speech Recognition",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents speech data from being sent to Microsoft for processing. Forces on-device-only recognition. Recommended.",
            Tags = ["speech", "online", "privacy", "voice"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "speech-mute-narrator",
            Label = "Mute Narrator Navigation Sounds",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Silences Narrator navigation and notification sounds. Default: Enabled.",
            Tags = ["speech", "narrator", "sounds"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "PlayNavigationSounds", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "PlayNavigationSounds", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "PlayNavigationSounds", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-typing-insights",
            Label = "Disable Typing Insights & Ink Collection",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Windows from collecting typing patterns and ink data for personalisation. Recommended: Disabled for privacy.",
            Tags = ["speech", "typing", "ink", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "speech-disable-voice-access",
            Label = "Disable Voice Access",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows 11 Voice Access feature (hands-free PC control). Default: Disabled.",
            Tags = ["speech", "voice-access", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\VoiceAccess"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\VoiceAccess", "VoiceAccessEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\VoiceAccess", "VoiceAccessEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\VoiceAccess", "VoiceAccessEnabled", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-voice-activation",
            Label = "Disable Voice Activation (Wake Words)",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents apps from listening for wake words (Hey Cortana, etc.). Recommended: Disabled for privacy.",
            Tags = ["speech", "voice", "activation", "cortana", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoice", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoice"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoice", 2)],
        },
        new TweakDef
        {
            Id = "speech-disable-voice-above-lock",
            Label = "Disable Voice Activation Above Lock",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents voice activation when the screen is locked. Security measure against wake-word hijacking.",
            Tags = ["speech", "voice", "lock-screen", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoiceAboveLock", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoiceAboveLock"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoiceAboveLock", 2)],
        },
        new TweakDef
        {
            Id = "speech-disable-narrator-cursor",
            Label = "Disable Narrator Cursor Indicator",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the blue Narrator cursor box that highlights the current element.",
            Tags = ["speech", "narrator", "cursor", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "ShowCursor", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "ShowCursor", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "ShowCursor", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-narrator-svc",
            Label = "Disable Narrator Service",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Narrator background service. The service can be re-enabled manually if needed.",
            Tags = ["speech", "narrator", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NarSvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NarSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NarSvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NarSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "speech-disable-narrator-shortcut",
            Label = "Disable Narrator Keyboard Shortcut",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Win+Ctrl+Enter shortcut that accidentally launches Narrator. Policy setting.",
            Tags = ["speech", "narrator", "shortcut", "keyboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Accessibility"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Accessibility", "ForceDisableNarratorShortcutKeys", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Accessibility", "ForceDisableNarratorShortcutKeys"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Accessibility", "ForceDisableNarratorShortcutKeys", 1)],
        },
        new TweakDef
        {
            Id = "speech-disable-model-update",
            Label = "Disable Speech Model & Data Collection",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops automatic speech model downloads and voice data collection. Reduces bandwidth and improves privacy.",
            Tags = ["speech", "model", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechDataCollection", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechDataCollection"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechDataCollection", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-dictation-shortcut",
            Label = "Disable Dictation Shortcut (Win+H)",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Win+H keyboard shortcut that launches Windows speech dictation. Prevents accidental activation during gaming or video playback. Default: Enabled. Recommended: Disabled for non-dictation users.",
            Tags = ["speech", "dictation", "shortcut", "win+h"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\SpeechRecognition"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\SpeechRecognition", "Value", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\SpeechRecognition", "Value", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\SpeechRecognition", "Value", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-narrator-hints",
            Label = "Disable Narrator Hints & Coaching",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off Narrator's spoken hints about keyboard shortcuts and coaching messages. Reduces verbosity for experienced Narrator users. Default: Enabled.",
            Tags = ["speech", "narrator", "hints", "coaching"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "SpeakWindowsHints", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DetailedFeedback", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "SpeakWindowsHints", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DetailedFeedback", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "SpeakWindowsHints", 0)],
        },
        new TweakDef
        {
            Id = "speech-narrator-verbosity-low",
            Label = "Set Narrator Verbosity to Low",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Narrator to only announce essential information (level 1 of 3). Reduces noise for power users of screen readers. Default: Level 2 (some).",
            Tags = ["speech", "narrator", "verbosity", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "VerbosityLevel", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "VerbosityLevel", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "VerbosityLevel", 1)],
        },
        new TweakDef
        {
            Id = "speech-disable-cortana-speech",
            Label = "Revoke Cortana Speech Permissions",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Cortana and assistant speech authorisation. Prevents Windows from using speech data for personalisation and assistant features. Recommended: Disabled for privacy.",
            Tags = ["speech", "cortana", "assistant", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\UserSpeechAuthorization"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\UserSpeechAuthorization", "Value", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\UserSpeechAuthorization", "Value", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\UserSpeechAuthorization", "Value", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-lang-detect",
            Label = "Disable Automatic Language Detection",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opts out of automatic input language detection sent via HTTP Accept-Language headers. A minor privacy improvement for users with multiple input languages. Default: Opted in.",
            Tags = ["speech", "language", "detection", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\International\User Profile"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1)],
        },
        new TweakDef
        {
            Id = "speech-disable-online-speech-recognition",
            Label = "Disable Online Speech Recognition",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the online speech recognition service. Voice data won't be sent to Microsoft. Default: enabled.",
            Tags = ["speech", "online", "recognition", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-dictation",
            Label = "Disable Voice Typing (Win+H Dictation)",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Win+H voice typing (dictation) feature via policy. Default: enabled.",
            Tags = ["speech", "dictation", "voice-typing", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-narrator-hotkey",
            Label = "Disable Narrator Hotkey (Win+Enter)",
            Category = "Voice Access & Speech",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Win+Enter Narrator shortcut. Prevents accidental activation. Default: enabled.",
            Tags = ["speech", "narrator", "hotkey", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "speech-disable-text-to-speech-cloud",
            Label = "Disable Cloud-Based Text-to-Speech Voices",
            Category = "Voice Access & Speech",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from downloading cloud TTS voices. Uses only local voices. Default: enabled.",
            Tags = ["speech", "tts", "cloud", "voices"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
        },
    ];
}
