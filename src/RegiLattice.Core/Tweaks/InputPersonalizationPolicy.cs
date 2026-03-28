// RegiLattice.Core — Tweaks/InputPersonalizationPolicy.cs
// Input Personalization Group Policy — Sprint 423.
// Controls Windows ink collection, text personalization, and
// inking-based learning features via Group Policy registry paths.
// Category: "Input Personalization Policy" | Slug: inpp
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\InputPersonalization

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class InputPersonalizationPolicy
{
    private const string IpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "inpp-deny-input-personalization",
                Label = "Block Input Personalization (Policy)",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowInputPersonalization=0 to disable cloud-based input personalization across the device. Prevents Windows from sending typing, inking, and voice input data to Microsoft to improve personalized recognisers.",
                Tags = ["input", "personalization", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables cloud input personalization; local recognition still functional.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowInputPersonalization", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInputPersonalization")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowInputPersonalization", 0)],
            },
            new TweakDef
            {
                Id = "inpp-restrict-ink-collection",
                Label = "Restrict Implicit Ink Collection",
                Category = "Input Personalization Policy",
                Description =
                    "Sets RestrictImplicitInkCollection=1 to prevent Windows from collecting ink strokes silently in the background for personalisation purposes. Default: 0 (collection allowed). Recommended for privacy: 1.",
                Tags = ["input", "ink", "collection", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops background ink data collection; handwriting recognition remains functional.",
                ApplyOps = [RegOp.SetDword(IpKey, "RestrictImplicitInkCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "RestrictImplicitInkCollection")],
                DetectOps = [RegOp.CheckDword(IpKey, "RestrictImplicitInkCollection", 1)],
            },
            new TweakDef
            {
                Id = "inpp-restrict-text-collection",
                Label = "Restrict Implicit Text Collection",
                Category = "Input Personalization Policy",
                Description =
                    "Sets RestrictImplicitTextCollection=1 to prevent Windows from silently collecting text samples (from keyboard input) for improving personalised language models and autocorrect.. Default: 0. Recommended: 1.",
                Tags = ["input", "text", "collection", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents silent collection of typed text for ML models; autocorrect dictionary unaffected.",
                ApplyOps = [RegOp.SetDword(IpKey, "RestrictImplicitTextCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "RestrictImplicitTextCollection")],
                DetectOps = [RegOp.CheckDword(IpKey, "RestrictImplicitTextCollection", 1)],
            },
            new TweakDef
            {
                Id = "inpp-disable-inking-keyboard-personalization",
                Label = "Disable Inking and Typing Personalization",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowInkingAndTypingPersonalization=0 to block the inking and typing personalisation feature that builds a personal dictionary from writing samples. Prevents sharing of handwriting/typing data with Microsoft.",
                Tags = ["input", "inking", "typing", "personalisation", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables inking and typing personalisation; no personal word list built from user data.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowInkingAndTypingPersonalization", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInkingAndTypingPersonalization")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowInkingAndTypingPersonalization", 0)],
            },
            new TweakDef
            {
                Id = "inpp-disable-user-dictionary",
                Label = "Disable Cloud User Dictionary Sync",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowUserDictionary=0 to prevent the personalised input dictionary from being synced with Microsoft servers. Local dictionary still functions; cloud backup and cross-device sync are blocked.",
                Tags = ["input", "dictionary", "sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables dictionary cloud sync; local autocorrect and custom words still work on this device.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowUserDictionary", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowUserDictionary")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowUserDictionary", 0)],
            },
            new TweakDef
            {
                Id = "inpp-disable-ink-learning",
                Label = "Disable Ink Recognition Learning",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowInkRecognitionLearning=0 to prevent the handwriting recogniser from learning and adapting to this user's writing style over time. Useful on shared or kiosk devices where per-user learning is undesirable.",
                Tags = ["input", "ink", "recognition", "learning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Static handwriting recogniser; does not adapt to individual writing style.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowInkRecognitionLearning", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInkRecognitionLearning")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowInkRecognitionLearning", 0)],
            },
            new TweakDef
            {
                Id = "inpp-disable-text-prediction",
                Label = "Disable Cloud-Based Text Prediction",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowTextPrediction=0 to disable cloud-assisted text prediction and autocomplete features. Local offline prediction is unaffected. Reduces data transmission associated with keyboard input.",
                Tags = ["input", "text prediction", "autocomplete", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud-based prediction disabled; offline autocomplete may still function if enabled locally.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowTextPrediction", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowTextPrediction")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowTextPrediction", 0)],
            },
            new TweakDef
            {
                Id = "inpp-disable-linguistic-collection",
                Label = "Disable Linguistic Data Collection",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowLinguisticDataCollection=0 to block Windows from sending linguistic data (autocorrect feedback, text samples) to Microsoft for improving language models. Complementary to RestrictImplicitTextCollection.",
                Tags = ["input", "linguistic", "collection", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No linguistic samples sent to cloud; comprehensive typing privacy measure.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowLinguisticDataCollection", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowLinguisticDataCollection")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowLinguisticDataCollection", 0)],
            },
            new TweakDef
            {
                Id = "inpp-disable-handwriting-telemetry",
                Label = "Disable Handwriting Error Reporting",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowHandwritingErrorReports=0 to prevent the handwriting recognition engine from sending error reports and misrecognition samples to Microsoft. Reduces telemetry from pen-enabled devices.",
                Tags = ["input", "handwriting", "telemetry", "reporting", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Handwriting error data not sent to Microsoft; local recognition unaffected.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowHandwritingErrorReports", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowHandwritingErrorReports")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowHandwritingErrorReports", 0)],
            },
            new TweakDef
            {
                Id = "inpp-disable-input-data-upload",
                Label = "Disable Input Data Upload to Microsoft",
                Category = "Input Personalization Policy",
                Description =
                    "Sets AllowInputDataUpload=0 to prevent Windows from uploading any collected input personalisation data (ink, text, voice) to Microsoft's servers. Applies a blanket block on all input-related cloud data transmission.",
                Tags = ["input", "upload", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Comprehensive block on all input data uploads; all local input features remain functional.",
                ApplyOps = [RegOp.SetDword(IpKey, "AllowInputDataUpload", 0)],
                RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInputDataUpload")],
                DetectOps = [RegOp.CheckDword(IpKey, "AllowInputDataUpload", 0)],
            },
        ];
}
