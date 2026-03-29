// RegiLattice.Core — Tweaks/AiAccessibilityPolicy.cs
// AI-powered accessibility features (Narrator, voice access, live captions, OCR) policy — Sprint 486.
// Category: "AI Accessibility Policy" | Slug: aiacc
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Accessibility

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AiAccessibilityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Accessibility";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aiacc-disable-voice-access",
                Label = "Disable AI Voice Access Feature",
                Category = "AI Accessibility Policy",
                Description =
                    "Disables the Windows Voice Access feature that allows full hands-free PC control using natural language voice commands processed by on-device AI, preventing continuous microphone monitoring.",
                Tags = ["ai", "voice-access", "accessibility", "microphone", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Voice Access disabled; AI voice control feature unavailable. No continuous mic monitoring.",
                ApplyOps = [RegOp.SetDword(Key, "DisableVoiceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVoiceAccess", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-ai-narrator-natural-voice",
                Label = "Disable Narrator AI Natural Voice Downloads",
                Category = "AI Accessibility Policy",
                Description =
                    "Prevents automatic downloading of Narrator AI-powered natural voices from the internet, keeping Narrator using built-in synthetic TTS voices and preventing background bandwidth consumption.",
                Tags = ["ai", "narrator", "tts", "natural-voice", "accessibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Narrator AI natural voice downloads blocked; only pre-installed TTS voices available.",
                ApplyOps = [RegOp.SetDword(Key, "DisableNarratorNaturalVoiceDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNarratorNaturalVoiceDownload")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNarratorNaturalVoiceDownload", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-live-captions-training",
                Label = "Disable Live Captions Usage Data Collection for AI Training",
                Category = "AI Accessibility Policy",
                Description =
                    "Opts out of sending Live Captions audio and transcription accuracy data to Microsoft for AI model improvement, preventing audio samples from being used for speech recognition training.",
                Tags = ["ai", "live-captions", "privacy", "training-data", "accessibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Live Captions training data collection disabled; audio transcription data not sent to Microsoft.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLiveCaptionsTrainingData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLiveCaptionsTrainingData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLiveCaptionsTrainingData", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-block-voice-access-command-logging",
                Label = "Block Voice Access Command History Logging",
                Category = "AI Accessibility Policy",
                Description =
                    "Prevents Windows Voice Access from maintaining a local or cloud log of spoken commands, protecting the spoken command history from being stored and reviewed.",
                Tags = ["ai", "voice-access", "command-history", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Voice Access command history not stored; no log of spoken commands retained.",
                ApplyOps = [RegOp.SetDword(Key, "DisableVoiceCommandHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceCommandHistory")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVoiceCommandHistory", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-ai-mouse-pointer-prediction",
                Label = "Disable AI Mouse Pointer Prediction (Smart Precision)",
                Category = "AI Accessibility Policy",
                Description =
                    "Disables the AI-powered mouse pointer prediction that adjusts pointer acceleration using activity-learned models, restoring standard linear or default pointer precision behaviour.",
                Tags = ["ai", "mouse", "pointer-prediction", "accessibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AI mouse pointer prediction disabled; mouse uses standard acceleration profile.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAIPointerPrediction", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAIPointerPrediction")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAIPointerPrediction", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-focus-assist-ai-rules",
                Label = "Disable AI-Based Focus Assist Automatic Rules",
                Category = "AI Accessibility Policy",
                Description =
                    "Disables focus assist rules that are automatically activated by the AI based on detected user activity (game detected, presentation mode, user working quietly), reverting to manual-only focus assist control.",
                Tags = ["ai", "focus-assist", "do-not-disturb", "automation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AI automatic focus assist rules disabled; Do Not Disturb must be toggled manually.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAIFocusAssistRules", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAIFocusAssistRules")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAIFocusAssistRules", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-ai-text-suggestions",
                Label = "Disable AI-Powered Keyboard Text Suggestions",
                Category = "AI Accessibility Policy",
                Description =
                    "Disables AI-powered predictive text suggestions displayed above the software keyboard and in text input fields, preventing on-device AI from learning typing patterns and offering word completions.",
                Tags = ["ai", "text-suggestions", "keyboard", "autocomplete", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AI text suggestions disabled; autocomplete word suggestions not shown during typing.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAITextSuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAITextSuggestions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAITextSuggestions", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-block-ai-accessibility-telemetry",
                Label = "Block AI Accessibility Feature Telemetry",
                Category = "AI Accessibility Policy",
                Description =
                    "Prevents AI-powered accessibility features (Narrator, Voice Access, Live Captions) from sending usage telemetry, feature performance data, and error reports to Microsoft.",
                Tags = ["ai", "accessibility", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Accessibility AI feature telemetry blocked; usage statistics and error data not sent to Microsoft.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAccessibilityAITelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAccessibilityAITelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAccessibilityAITelemetry", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-narrator-image-description",
                Label = "Disable Narrator AI Image Description (Cloud OCR)",
                Category = "AI Accessibility Policy",
                Description =
                    "Disables the Narrator AI image description feature that sends screenshots to Microsoft cloud for AI-powered image-to-text descriptions for visually impaired users, preventing image data from leaving the device.",
                Tags = ["ai", "narrator", "image-description", "accessibility", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Narrator cloud image description disabled; screenshots not sent to Microsoft for AI alt-text generation.",
                ApplyOps = [RegOp.SetDword(Key, "DisableNarratorCloudImageDescription", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNarratorCloudImageDescription")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNarratorCloudImageDescription", 1)],
            },
            new TweakDef
            {
                Id = "aiacc-disable-accessibility-ai-suggestions-oobe",
                Label = "Disable Accessibility AI Feature Suggestions during OOBE",
                Category = "AI Accessibility Policy",
                Description =
                    "Suppresses the Out-of-Box Experience (OOBE) accessibility page that uses AI to detect and suggest accessibility features based on observed pointer, typing, and sight patterns during first setup.",
                Tags = ["ai", "accessibility", "oobe", "setup", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "OOBE AI accessibility feature detection and suggestions suppressed during Windows first setup.",
                ApplyOps = [RegOp.SetDword(Key, "DisableOOBEAccessibilitySuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOOBEAccessibilitySuggestions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOOBEAccessibilitySuggestions", 1)],
            },
        ];
}
