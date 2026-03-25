// RegiLattice.Core — Tweaks/SpellingAndTypingPolicy.cs
// Sprint 275: Spelling & Typing Group Policy (10 tweaks)
// Category: "Spelling And Typing Policy" | Slug: sptype
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpellingAndTyping

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SpellingAndTypingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpellingAndTyping";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sptype-disable-auto-correct",
            Label = "Disable Autocorrect via Policy",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets AutocorrectMisspelledWords=0 in the SpellingAndTyping policy key. "
                + "Prevents Windows from automatically replacing mis-typed words with "
                + "spell-check suggestions via the system-wide autocorrect engine. "
                + "Autocorrect substitutions in technical fields such as CLI terminals, "
                + "code editors, and database query tools can silently corrupt commands "
                + "and identifiers. Default: 1. Recommended: 0 for power users.",
            Tags = ["spelling", "autocorrect", "typing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutocorrectMisspelledWords", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutocorrectMisspelledWords")],
            DetectOps = [RegOp.CheckDword(Key, "AutocorrectMisspelledWords", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-spell-checking",
            Label = "Disable System-Wide Spell Checking",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets SpellCheckingEnabled=0 in the SpellingAndTyping policy key. Disables "
                + "the Windows spell-checking engine that underlies red-underline annotations "
                + "in text input controls system-wide. Applications using the WinRT "
                + "TextBox or Web-based inputs inside WebView2 share this engine. "
                + "Programmatic text fields used by developers and technical writers "
                + "often benefit from disabling system spell-check in favour of IDE-level checkers. "
                + "Default: 1. Recommended: 0 on developer workstations.",
            Tags = ["spelling", "spellcheck", "typing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SpellCheckingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SpellCheckingEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "SpellCheckingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-text-prediction",
            Label = "Disable Text Prediction (Inline Suggestions)",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets TextPredictionEnabled=0 in the SpellingAndTyping policy key. "
                + "Removes inline word-completion suggestions that appear while typing in "
                + "the touch keyboard, hardware keyboard (Windows 11), and select UWP text "
                + "fields. Text prediction samples typed input stream to populate the "
                + "suggestion bar and may transmit partial words to the cloud language "
                + "model on some configurations. Default: 1. Recommended: 0.",
            Tags = ["text", "prediction", "suggestion", "typing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TextPredictionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TextPredictionEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TextPredictionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-highlight-misspelled",
            Label = "Disable Misspelling Underline Highlight",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets HighlightMisspelledWords=0 in the SpellingAndTyping policy key. "
                + "Stops the system spell engine from drawing red wavy underlines beneath "
                + "unrecognised words in native text controls. In password fields and API "
                + "key editors the underlines can reveal that a typed value was not "
                + "dictionary-recognised, inadvertently confirming password contents to "
                + "shoulder-surfers. Default: 1. Recommended: 0.",
            Tags = ["spelling", "highlight", "underline", "typing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HighlightMisspelledWords", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "HighlightMisspelledWords")],
            DetectOps = [RegOp.CheckDword(Key, "HighlightMisspelledWords", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-typing-insights",
            Label = "Disable Typing Insights",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets InsightsEnabled=0 in the SpellingAndTyping policy key. Disables "
                + "the Windows Typing Insights feature that collects word-frequency data "
                + "to personalise autocorrect, text prediction, and word suggestions over "
                + "time. The insights database is stored per-user and contains an "
                + "implicit record of vocabulary, project names, and identifiers typed on "
                + "the machine. Default: 1. Recommended: 0.",
            Tags = ["typing", "insights", "personalisation", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "InsightsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "InsightsEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "InsightsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-hardware-keyboard-suggestions",
            Label = "Disable Hardware Keyboard Suggestions Bar",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets HardwareKeyboardTextSuggestions=0 in the SpellingAndTyping policy key. "
                + "Removes the candidate-word suggestions bar that Windows 11 shows above "
                + "the caret when typing with a physical keyboard in compatible applications. "
                + "The bar requires per-keystroke processing by the suggestion engine and "
                + "adds visual distraction in fast-typing contexts. "
                + "Default: 1 (Windows 11). Recommended: 0.",
            Tags = ["keyboard", "suggestions", "hardware", "typing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HardwareKeyboardTextSuggestions", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "HardwareKeyboardTextSuggestions")],
            DetectOps = [RegOp.CheckDword(Key, "HardwareKeyboardTextSuggestions", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-swipe-typing",
            Label = "Disable Touch Keyboard Swipe Typing",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets SwipeKeyboardEnabled=0 in the SpellingAndTyping policy key. Disables "
                + "swipe (gesture) typing on the Windows touch keyboard, requiring each "
                + "character to be tapped individually. Swipe typing continuously records "
                + "finger-trajectory paths across the keyboard surface, feeding a gesture "
                + "recognition model that accesses typed words indirectly via path geometry. "
                + "Default: 1. Recommended: 0 on secure devices.",
            Tags = ["swipe", "gesture", "touch", "keyboard", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SwipeKeyboardEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SwipeKeyboardEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "SwipeKeyboardEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-typing-telemetry",
            Label = "Disable Typing Telemetry Upload",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets TypingDataCollectionEnabled=0 in the SpellingAndTyping policy key. "
                + "Prevents Windows from uploading typing-pattern telemetry to Microsoft's "
                + "cloud services. Typing telemetry includes keystroke timing, word-choice "
                + "corrections, and session length, aggregated to improve the shared "
                + "language model. This data constitutes detailed behavioural profiling "
                + "even when individual keystrokes are not transmitted. "
                + "Default: 1. Recommended: 0.",
            Tags = ["typing", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TypingDataCollectionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TypingDataCollectionEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TypingDataCollectionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-handwriting-recognition",
            Label = "Disable Handwriting Recognition Improvement",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets HandwritingAcceptedSamples=0 in the SpellingAndTyping policy key. "
                + "Stops Windows from collecting pen-stroke samples to improve the on-device "
                + "handwriting recognition model via the Windows Feedback infrastructure. "
                + "Sample collection records user handwriting during normal use and "
                + "periodically packages ink data for processing. "
                + "Default: 1. Recommended: 0.",
            Tags = ["handwriting", "recognition", "ink", "samples", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HandwritingAcceptedSamples", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "HandwritingAcceptedSamples")],
            DetectOps = [RegOp.CheckDword(Key, "HandwritingAcceptedSamples", 0)],
        },
        new TweakDef
        {
            Id = "sptype-disable-autocomplete",
            Label = "Disable System Autocomplete",
            Category = "Spelling And Typing Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets AutoCompleteEnabled=0 in the SpellingAndTyping policy key. Disables "
                + "the system-wide autocomplete engine that suggests previously entered "
                + "values in form fields, search boxes, and address bars using the Windows "
                + "Activity History store. Autocomplete draws on stored usage history to "
                + "reconstruct past inputs, and its suggestion pool can disclose previously "
                + "visited URLs or typed content to other users of the same account. "
                + "Default: 1. Recommended: 0.",
            Tags = ["autocomplete", "form", "history", "typing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutoCompleteEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoCompleteEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "AutoCompleteEnabled", 0)],
        },
    ];
}
