namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 688 — PolicyOfficeWord
// Microsoft Word Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\Word\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Word security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

internal static class PolicyOfficeWord
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Word\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Word\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Word\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offword-pol-vba-block",
            Label = "Block All Word Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the Word Group Policy security path. Instructs Word "
                + "to disable all macros without notification — the highest security level. "
                + "Since this is a Policy path, users cannot lower the setting in the Trust "
                + "Center. Apply on machines where VBA macros are not required.",
            Tags = ["office", "word", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all Word macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offword-pol-disable-activex",
            Label = "Block All ActiveX Controls in Word via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the Word Group Policy security path. Prevents "
                + "ActiveX controls from running in Word documents regardless of their safety "
                + "marking. Eliminates a common attack vector for document-based exploits.",
            Tags = ["office", "word", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in Word — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offword-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in Word via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the Word Group Policy security path. Instructs "
                + "Word to reject files whose content does not match their extension (e.g., a "
                + "renamed executable masquerading as .docx). Level 2 = block mismatches "
                + "without prompting.",
            Tags = ["office", "word", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types — blocks all extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offword-pol-block-net-exec",
            Label = "Block Internet Content Execution in Word via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets blockcontentexecutionfrominternet=1 in the Word Group Policy security "
                + "path. Prevents Word from executing content (scripts, code) embedded in "
                + "documents that were opened from Internet locations.",
            Tags = ["office", "word", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in Word.",
            ApplyOps = [RegOp.SetDword(SecKey, "blockcontentexecutionfrominternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "blockcontentexecutionfrominternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "blockcontentexecutionfrominternet", 1)],
        },
        new TweakDef
        {
            Id = "offword-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to Word Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the Word Group Policy security path. Prevents external "
                + "applications and scripts from programmatically accessing the Word VBA project "
                + "object model, blocking a common lateral-movement technique used by macro "
                + "malware to propagate across Office applications.",
            Tags = ["office", "word", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the Word VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offword-pol-no-macro-programs",
            Label = "Prevent Word Macros from Launching Programs via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RunProgramsInMacros=0 in the Word Group Policy security path. Prevents "
                + "Word macros from using Shell() or other APIs to launch external executables, "
                + "reducing the risk of macro-based malware dropping and executing payloads.",
            Tags = ["office", "word", "macro", "shell", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Macro code in Word cannot launch external programs.",
            ApplyOps = [RegOp.SetDword(SecKey, "RunProgramsInMacros", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "RunProgramsInMacros")],
            DetectOps = [RegOp.CheckDword(SecKey, "RunProgramsInMacros", 0)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offword-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in Word via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the Word Group Policy ProtectedView path. "
                + "Explicitly enforces that Protected View remains enabled for documents "
                + "downloaded from the Internet, preventing users from disabling this protection "
                + "in the Trust Center.",
            Tags = ["office", "word", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps Word Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offword-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in Word via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the Word Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for documents opened from "
                + "unsafe locations (paths not on the Trusted Locations list), preventing policy "
                + "bypass through location manipulation.",
            Tags = ["office", "word", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps Word Protected View on for documents from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offword-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in Word via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the Word Group Policy ProtectedView path. "
                + "Enforces that Protected View remains enabled for Word documents received as "
                + "email attachments — one of the highest-risk document delivery vectors. "
                + "Prevents users from disabling this protection in the Trust Center.",
            Tags = ["office", "word", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment Word files always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy file format blocking ───────────────────────────────────────
        new TweakDef
        {
            Id = "offword-pol-fileblock-word97",
            Label = "Block Word 97 Binary Format Files via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Word97Files=2 in the Word Group Policy FileBlock path. Instructs Word "
                + "to open Word 97 binary (.doc) files in Protected View without allowing "
                + "editing. Legacy binary formats have larger attack surfaces than docx/oxml "
                + "formats and are commonly used as malware delivery vectors.",
            Tags = ["office", "word", "fileblock", "legacy", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Word 97 .doc files open in Protected View (read-only) by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Word97Files", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Word97Files")],
            DetectOps = [RegOp.CheckDword(FbKey, "Word97Files", 2)],
        },
    ];
}
