// RegiLattice.Core — Tweaks/LanguageOptionsPolicy.cs
// Sprint 277: Language Options Group Policy (10 tweaks)
// Category: "Language Options Policy" | Slug: langopt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanguageOptions

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LanguageOptionsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanguageOptions";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "langopt-disable-language-pack-install",
            Label = "Block User Language Pack Installation",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets BlockUserFromAddingLanguages=1 in the LanguageOptions policy key. "
                + "Prevents standard users from adding new display languages or keyboard "
                + "layouts via Settings > Time & Language. Uncontrolled language additions "
                + "on shared or managed machines can change locale-dependent settings "
                + "and alter application behaviour for other users. "
                + "Default: 0. Recommended: 1 on managed workstations.",
            Tags = ["language", "pack", "locale", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockUserFromAddingLanguages", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockUserFromAddingLanguages")],
            DetectOps = [RegOp.CheckDword(Key, "BlockUserFromAddingLanguages", 1)],
        },
        new TweakDef
        {
            Id = "langopt-restrict-language-change",
            Label = "Restrict Language Display Setting Change",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets RestrictLanguageChange=1 in the LanguageOptions policy key. Prevents "
                + "non-administrative users from changing the Windows display language that "
                + "governs UI strings, dialogs, and menu text. Allowing arbitrary language "
                + "switches on shared terminals create accessibility and compliance issues "
                + "for organisations that require a fixed locale for audit logs. "
                + "Default: 0. Recommended: 1.",
            Tags = ["language", "display", "locale", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLanguageChange", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLanguageChange")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLanguageChange", 1)],
        },
        new TweakDef
        {
            Id = "langopt-disable-ime-telemetry",
            Label = "Disable IME Telemetry Upload",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets ImeTelemetryEnabled=0 in the LanguageOptions policy key. Stops the "
                + "Windows Input Method Editor (IME) from transmitting usage data including "
                + "candidate selection frequency, dictionary look-up patterns, and "
                + "conversion correction events to Microsoft. IME telemetry can indirectly "
                + "reveal content typed when using CJK or other IME-based input. "
                + "Default: 1. Recommended: 0.",
            Tags = ["ime", "telemetry", "privacy", "language", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ImeTelemetryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ImeTelemetryEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "ImeTelemetryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "langopt-disable-cloud-candidate",
            Label = "Disable IME Cloud Candidate Lookup",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets BlockCloudCandidates=1 in the LanguageOptions policy key. Prevents "
                + "the Chinese, Japanese, and Korean IME engines from sending keystrokes "
                + "to Microsoft's cloud candidate service to retrieve extended candidate "
                + "lists. Cloud candidate queries transmit partial words to an external "
                + "endpoint for every keystroke, creating a persistent privacy exposure. "
                + "Default: 0. Recommended: 1.",
            Tags = ["ime", "cloud", "candidate", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockCloudCandidates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockCloudCandidates")],
            DetectOps = [RegOp.CheckDword(Key, "BlockCloudCandidates", 1)],
        },
        new TweakDef
        {
            Id = "langopt-disable-handwriting-recognition-improvement",
            Label = "Disable Language Handwriting Recognition Improvement",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets BlockHandwritingImprovementProgram=1 in the LanguageOptions policy "
                + "key. Prevents Windows from enrolling the device in the handwriting "
                + "recognition improvement programme which collects ink samples. Collected "
                + "samples include handwritten characters from all users on the machine and "
                + "are submitted to Microsoft's recognition training pipeline. "
                + "Default: 0. Recommended: 1.",
            Tags = ["handwriting", "ink", "improvement", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockHandwritingImprovementProgram", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockHandwritingImprovementProgram")],
            DetectOps = [RegOp.CheckDword(Key, "BlockHandwritingImprovementProgram", 1)],
        },
        new TweakDef
        {
            Id = "langopt-disable-ocr-telemetry",
            Label = "Disable Language OCR Telemetry",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets OcrTelemetryEnabled=0 in the LanguageOptions policy key. Prevents "
                + "the Windows OCR engine from submitting recognition quality metrics and "
                + "anonymised document-structure statistics to Microsoft. OCR telemetry "
                + "accumulates document-type patterns that may reveal the nature of "
                + "business documents processed by the machine. "
                + "Default: 1. Recommended: 0.",
            Tags = ["ocr", "telemetry", "language", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "OcrTelemetryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "OcrTelemetryEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "OcrTelemetryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "langopt-disable-speech-recognition-telemetry",
            Label = "Disable Speech Recognition Language Telemetry",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets SpeechRecognitionTelemetryEnabled=0 in the LanguageOptions policy "
                + "key. Stops Windows Speech Recognition from uploading speech-model "
                + "adaptation data and recognition-error patterns. Adaptation data encodes "
                + "voice characteristics unique to the primary user and could be used for "
                + "speaker profiling in combination with other datasets. "
                + "Default: 1. Recommended: 0.",
            Tags = ["speech", "recognition", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SpeechRecognitionTelemetryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SpeechRecognitionTelemetryEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "SpeechRecognitionTelemetryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "langopt-disable-keyboard-telemetry",
            Label = "Disable Keyboard Layout Telemetry",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets KeyboardTelemetryEnabled=0 in the LanguageOptions policy key. "
                + "Prevents Windows from submitting keyboard-layout switch events and "
                + "active-layout usage frequency to Microsoft. Keyboard-layout activity "
                + "data reveals which languages a user is communicating in and at what "
                + "frequency across sessions. "
                + "Default: 1. Recommended: 0.",
            Tags = ["keyboard", "layout", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "KeyboardTelemetryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "KeyboardTelemetryEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "KeyboardTelemetryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "langopt-disable-language-online-update",
            Label = "Disable Automatic Language Pack Online Update",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets BlockLanguagePackUpdatesFromWindowsUpdate=1 in the LanguageOptions "
                + "policy key. Stops Windows from automatically downloading updated language "
                + "packs and locale-data files from Windows Update without administrator "
                + "approval. Unexpected language-pack updates can change font-rendering, "
                + "date formats, and input method behaviour mid-session. "
                + "Default: 0. Recommended: 1.",
            Tags = ["language", "update", "windowsupdate", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockLanguagePackUpdatesFromWindowsUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockLanguagePackUpdatesFromWindowsUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "BlockLanguagePackUpdatesFromWindowsUpdate", 1)],
        },
        new TweakDef
        {
            Id = "langopt-disable-language-sync",
            Label = "Disable Language Settings Sync",
            Category = "Language Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DoNotSyncLanguageSettings=1 in the LanguageOptions policy key. "
                + "Prevents language preferences, keyboard layout order, and regional "
                + "settings from synchronising to the cloud and propagating to other devices "
                + "connected to the same Microsoft account. Language settings sync can "
                + "wrongly reconfigure keyboards on shared machines. "
                + "Default: 0. Recommended: 1.",
            Tags = ["language", "sync", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DoNotSyncLanguageSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotSyncLanguageSettings")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotSyncLanguageSettings", 1)],
        },
    ];
}
