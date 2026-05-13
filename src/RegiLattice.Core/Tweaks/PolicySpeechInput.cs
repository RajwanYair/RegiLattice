namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicySpeechInput
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\Speech — Speech Recognition / Voice Access.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\SpeechModel — online speech model policies.

    private const string SpeechKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Speech";
    private const string ModelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpeechModel";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "spkinput-disable-online-speech-recognition",
            Label = "Disable Online Speech Recognition via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechRecognition=0 in the Speech Group Policy key. "
                + "Prevents the cloud speech recognition service from being used for Windows speech features. "
                + "Voice data is only processed on-device; no audio is transmitted to Microsoft Speech servers. "
                + "Applies broadly to Cortana voice queries, Voice Typing, and Voice Access cloud enhancement.",
            Tags = ["speech", "recognition", "cloud", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Cloud speech recognition disabled; on-device speech processing continues.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechRecognition", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechRecognition")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechRecognition", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-activation",
            Label = "Block Always-On Voice Activation",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowVoiceActivation=0 in the Speech Group Policy key. "
                + "Prevents applications from using the always-on voice listening hook (keyword detection). "
                + "Eliminates the continuous microphone monitoring required for wake words ('Hey Cortana', etc.), "
                + "removing a permanent audio capture pipeline from the endpoint.",
            Tags = ["speech", "voice-activation", "wake-word", "microphone", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Always-on wake word detection is disabled; microphone not continuously monitored.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceActivation", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceActivation")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceActivation", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-model-update",
            Label = "Block Automatic Speech Model Updates",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechModelUpdate=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from automatically downloading and applying updated cloud or on-device "
                + "speech recognition model files. Stabilises speech behaviour in validated regulated "
                + "environments where untested model changes could affect accessibility tools.",
            Tags = ["speech", "model-update", "policy", "enterprise", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Speech model files are frozen; updates require IT-managed deployment.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-telemetry",
            Label = "Disable Speech Input Telemetry",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechTelemetry=0 in the Speech Group Policy key. "
                + "Blocks the Speech subsystem from sending diagnostic voice data, recognition accuracy "
                + "metrics, and corrected text snippets to Microsoft for model improvement. "
                + "Audio utterances and transcription corrections are classified as personal data under GDPR/HIPAA.",
            Tags = ["speech", "telemetry", "privacy", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Speech telemetry suppressed; no voice samples or transcription data transmitted.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechTelemetry")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-typing",
            Label = "Disable Voice Typing via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowVoiceTyping=0 in the Speech Group Policy key. "
                + "Disables the Voice Typing feature (Win+H) systemwide via Group Policy. "
                + "Prevents users from dictating text into any application, stopping the microphone "
                + "activation path associated with dictation on shared and kiosk workstations.",
            Tags = ["speech", "voice-typing", "dictation", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Voice Typing (Win+H) is disabled; microphone not used for dictation.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceTyping", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceTyping")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceTyping", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-cortana-voice",
            Label = "Disable Cortana Voice Interaction via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowCortanaVoice=0 in the Speech Group Policy key. "
                + "Prevents Cortana from accepting voice input and responding to spoken queries. "
                + "Complements the Cortana keyboard disable by also closing the audio/microphone channel "
                + "used for Cortana's voice assistant functionality.",
            Tags = ["speech", "cortana", "voice", "microphone", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cortana no longer accepts voice queries; keyboard interaction unaffected.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowCortanaVoice", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowCortanaVoice")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowCortanaVoice", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-personalization",
            Label = "Block Speech Personalisation Data Collection",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechPersonalization=0 in the Speech Group Policy key. "
                + "Stops Windows from collecting contacts, calendar events, frequently typed words, "
                + "and app usage patterns to personalise speech recognition accuracy. "
                + "This dataset would stay on-device but its aggregation represents a privacy concern "
                + "in regulated environments where data minimisation principles apply.",
            Tags = ["speech", "personalisation", "privacy", "policy", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Speech personalisation disabled; recognition accuracy unchanged.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechPersonalization")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-access-start",
            Label = "Prevent Voice Access from Starting at Login",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowVoiceAccessStartup=0 in the Speech Group Policy key. "
                + "Prevents the Windows Voice Access feature from automatically starting when a user logs "
                + "into Windows. Voice Access requires persistent microphone access; letting it auto-start "
                + "runs an unnecessary audio capture pipeline on workstations not requiring accessibility.",
            Tags = ["speech", "voice-access", "startup", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Voice Access does not auto-start; users can still launch it manually.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceAccessStartup")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-restrict-online-speech-model",
            Label = "Block Online Speech Model Download",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowOnlineSpeechModel=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from downloading an enhanced online speech recognition model "
                + "that improves accuracy beyond the locally installed model. "
                + "Disabling the download removes a background network data transfer and pins "
                + "speech processing to on-device models vetted by the organisation.",
            Tags = ["speech", "model", "download", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Online speech model is not downloaded; on-device model used exclusively.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowOnlineSpeechModel", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowOnlineSpeechModel")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowOnlineSpeechModel", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-access-across-lock",
            Label = "Disable Speech Recognition on Lock Screen",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechOnLockScreen=0 in the Speech Group Policy key. "
                + "Prevents voice assistants and speech recognition from accepting voice input when "
                + "the workstation screen is locked. Eliminates the attack surface where an attacker "
                + "with physical access to a locked machine can issue voice commands to local assistants.",
            Tags = ["speech", "lock-screen", "security", "policy", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Voice assistant cannot be invoked from locked screen; prevents audio-based attacks.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechOnLockScreen")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
        },
    ];
}
