namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: TouchPen.cs ===

// ── Merged from WindowsInk.cs ──────────────────────────────────────────────────

[TweakModule]
internal static class PolicyTextInputExt
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\TextInput — additional values beyond
    // the 5 already covered in existing modules (AllowHandwritingLMUpdate,
    // AllowInputDeviceUserInterface, AllowLinguisticDataCollection,
    // AllowTouchKeyboardAutoInvokeInDesktopMode, AllowVoiceTyping).

    private const string TiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TextInput";
    private const string ImeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IME";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "txtin2-disable-text-prediction",
            Label = "Disable Text Prediction for Physical Keyboards via Policy",
            Category = "Input",
            Description =
                "Sets AllowHardwareKeyboardTextSuggestions=0 in the TextInput Group Policy key. "
                + "Prevents Windows from showing inline word-completion suggestions on hardware (physical) keyboards. "
                + "Text prediction sends keystroke patterns to the language model; disabling preserves "
                + "the privacy of typed content on corporate devices.",
            Tags = ["text-input", "keyboard", "prediction", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Word suggestions no longer appear when typing on a physical keyboard.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowHardwareKeyboardTextSuggestions", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowHardwareKeyboardTextSuggestions")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowHardwareKeyboardTextSuggestions", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-user-settings-override",
            Label = "Lock Text Input Settings from User Override",
            Category = "Input",
            Description =
                "Sets AllowUserSettings=0 in the TextInput Group Policy key. "
                + "Prevents users from modifying text input settings (autocorrect, prediction thresholds, "
                + "handwriting personalisation) via Windows Settings. "
                + "All text-input behaviour is controlled exclusively by Group Policy on managed machines.",
            Tags = ["text-input", "settings", "lockdown", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Text Input settings page is read-only for standard users.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowUserSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowUserSettings")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowUserSettings", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-autocorrect",
            Label = "Disable Hardware Keyboard Autocorrect via Policy",
            Category = "Input",
            Description =
                "Sets AllowKeyboardAutocorrect=0 in the TextInput Group Policy key. "
                + "Disables automatic spelling correction on hardware keyboard input. "
                + "Prevents autocorrect from silently changing intended technical terms, passwords, "
                + "or code identifiers in document editors and developer tools.",
            Tags = ["text-input", "autocorrect", "keyboard", "policy", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Keyboard autocorrect is disabled; all typed text is preserved verbatim.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowKeyboardAutocorrect", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowKeyboardAutocorrect")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowKeyboardAutocorrect", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-user-feedback-submission",
            Label = "Block Text Input User Feedback Telemetry",
            Category = "Input",
            Description =
                "Sets AllowUserFeedback=0 in the TextInput Group Policy key. "
                + "Prevents the Text Input (handwriting, touch keyboard, voice typing) subsystem from "
                + "prompting users for satisfaction ratings or submitting diagnostic feedback data "
                + "to Microsoft for language model improvement.",
            Tags = ["text-input", "telemetry", "feedback", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Text input feedback prompts are disabled; no telemetry submitted from input panel.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowUserFeedback", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowUserFeedback")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowUserFeedback", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ink-personalization-upload",
            Label = "Block Handwriting Personalisation Data Upload",
            Category = "Input",
            Description =
                "Sets AllowHandwritingPersonalizationUpload=0 in the TextInput Group Policy key. "
                + "Prevents handwriting recognition data (pen strokes and corrections) from being "
                + "transmitted to Microsoft servers for model personalisation. "
                + "Handwriting recognition continues to function using on-device models only.",
            Tags = ["text-input", "handwriting", "personalisation", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ink/handwriting training data is not uploaded; on-device recognition continues.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowHandwritingPersonalizationUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowHandwritingPersonalizationUpload")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowHandwritingPersonalizationUpload", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-internet-access",
            Label = "Block IME Access to External Prediction Services",
            Category = "Input",
            Description =
                "Sets AllowIMENetworkAccess=0 in the TextInput Group Policy key. "
                + "Prevents Input Method Editors (IME) for CJK and other scripts from accessing the "
                + "internet for cloud-based candidate suggestions, emoji recommendations, and dictionary updates. "
                + "Eliminates keystroke exfiltration risk from cloud-connected IME prediction services.",
            Tags = ["text-input", "ime", "network", "privacy", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME prediction is limited to local/offline dictionaries; cloud suggestions disabled.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowIMENetworkAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowIMENetworkAccess")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowIMENetworkAccess", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-cloud-ime-candidates",
            Label = "Disable Cloud-Based IME Candidate Suggestions",
            Category = "Input",
            Description =
                "Sets AllowCloudCandidates=0 in the IME Group Policy key. "
                + "Prevents the Windows IME from fetching real-time cloud candidate improvements. "
                + "Cloud IME candidates require sending the current input context to Microsoft servers, "
                + "posing a risk of partial document or credential context disclosure in enterprise environments.",
            Tags = ["ime", "cloud", "candidates", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME uses only installed local candidate dictionaries; no cloud lookups.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowCloudCandidates", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowCloudCandidates")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowCloudCandidates", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-update",
            Label = "Block IME Automatic Dictionary Updates",
            Category = "Input",
            Description =
                "Sets AllowIMEUpdate=0 in the IME Group Policy key. "
                + "Prevents the Windows IME from automatically downloading dictionary and language model "
                + "updates from Microsoft Graph or Update servers. "
                + "Stabilises IME behaviour in controlled environments where software changes must be sanctioned.",
            Tags = ["ime", "update", "dictionary", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "IME dictionaries are frozen; language model updates require IT-approved deployment.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowIMEUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowIMEUpdate")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowIMEUpdate", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-ime-telemetry",
            Label = "Disable IME Typing Telemetry",
            Category = "Input",
            Description =
                "Sets AllowIMETelemetry=0 in the IME Group Policy key. "
                + "Blocks the Windows IME from transmitting typing pattern, candidate selection, "
                + "and correction data to Microsoft for telemetry and model improvement. "
                + "Typing content is a high-sensitivity data stream; telemetry must be controlled in regulated sectors.",
            Tags = ["ime", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IME typing telemetry is disabled; no input pattern data transmitted to Microsoft.",
            ApplyOps = [RegOp.SetDword(ImeKey, "AllowIMETelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeKey, "AllowIMETelemetry")],
            DetectOps = [RegOp.CheckDword(ImeKey, "AllowIMETelemetry", 0)],
        },
        new TweakDef
        {
            Id = "txtin2-disable-touch-keyboard-auto-invoke",
            Label = "Prevent Touch Keyboard Auto-Invoke in Tablet Mode",
            Category = "Input",
            Description =
                "Sets AllowTouchKeyboardAutoInvoke=0 in the TextInput Group Policy key. "
                + "Disables the automatic appearance of the on-screen touch keyboard when a text field "
                + "gains focus in tablet mode. Users must manually invoke the touch keyboard, preventing "
                + "layout interference on large-screen kiosk and hybrid devices.",
            Tags = ["text-input", "touch-keyboard", "tablet", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Touch keyboard no longer pops up automatically when tapping text fields.",
            ApplyOps = [RegOp.SetDword(TiKey, "AllowTouchKeyboardAutoInvoke", 0)],
            RemoveOps = [RegOp.DeleteValue(TiKey, "AllowTouchKeyboardAutoInvoke")],
            DetectOps = [RegOp.CheckDword(TiKey, "AllowTouchKeyboardAutoInvoke", 0)],
        },
    ];
}
