namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 688 — PolicyOfficeExcel
// Microsoft Excel Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\Excel\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Excel security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

internal static class PolicyOfficeExcel
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Excel\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Excel\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Excel\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offxls-pol-vba-block",
            Label = "Block All Excel Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the Excel Group Policy security path. Instructs Excel "
                + "to disable all macros without notification — the highest macro security "
                + "level. Users cannot lower this setting in the Trust Center because it is "
                + "enforced via Group Policy. Apply on machines where Excel macros are not "
                + "required.",
            Tags = ["office", "excel", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all Excel macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offxls-pol-disable-activex",
            Label = "Block All ActiveX Controls in Excel via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the Excel Group Policy security path. Prevents "
                + "ActiveX controls from running in Excel workbooks regardless of their safety "
                + "marking. Eliminates a common attack vector for document-based exploits.",
            Tags = ["office", "excel", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in Excel — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offxls-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in Excel via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the Excel Group Policy security path. Instructs "
                + "Excel to reject files whose content does not match their file extension. "
                + "Level 2 = block without prompting, closing the vector of renamed-payload "
                + "delivery via .xlsx or .csv files.",
            Tags = ["office", "excel", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types — blocks all extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offxls-pol-block-net-exec",
            Label = "Block Internet Content Execution in Excel via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets BlockContentExecutionFromInternet=1 in the Excel Group Policy security "
                + "path. Prevents Excel from executing embedded scripts or content from "
                + "workbooks that were opened from Internet-zone locations.",
            Tags = ["office", "excel", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in Excel.",
            ApplyOps = [RegOp.SetDword(SecKey, "BlockContentExecutionFromInternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "BlockContentExecutionFromInternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "BlockContentExecutionFromInternet", 1)],
        },
        new TweakDef
        {
            Id = "offxls-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to Excel Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the Excel Group Policy security path. Prevents external "
                + "applications and scripts from programmatically accessing the Excel VBA "
                + "project object model, blocking a common lateral-movement technique used by "
                + "macro malware to propagate across Office applications.",
            Tags = ["office", "excel", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the Excel VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offxls-pol-workbook-link-warn",
            Label = "Warn Before Updating Workbook Links via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets WorkbookLinkWarnings=2 in the Excel Group Policy security path. "
                + "Instructs Excel to warn users before updating links to other workbooks on "
                + "open, preventing silent data refresh from untrusted external sources. "
                + "Value 2 = always warn; 0 = disable update on open.",
            Tags = ["office", "excel", "links", "workbook", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Excel warns before auto-updating external workbook links.",
            ApplyOps = [RegOp.SetDword(SecKey, "WorkbookLinkWarnings", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "WorkbookLinkWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "WorkbookLinkWarnings", 2)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offxls-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in Excel via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the Excel Group Policy ProtectedView path. "
                + "Explicitly enforces that Protected View stays enabled for workbooks "
                + "downloaded from the Internet, preventing users from disabling this protection "
                + "in the Trust Center.",
            Tags = ["office", "excel", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps Excel Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offxls-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in Excel via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the Excel Group Policy ProtectedView "
                + "path. Enforces that Protected View remains active for workbooks from locations "
                + "not on the Trusted Locations list, ensuring risky-origin files open in "
                + "isolation.",
            Tags = ["office", "excel", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps Excel Protected View on for files from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offxls-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in Excel via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the Excel Group Policy ProtectedView path. "
                + "Enforces that Protected View stays active for workbooks received as email "
                + "attachments, preventing users from disabling this default-on protection in "
                + "the Trust Center.",
            Tags = ["office", "excel", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment Excel files always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy format and macro sheet blocking ────────────────────────────
        new TweakDef
        {
            Id = "offxls-pol-fileblock-xl4-macros",
            Label = "Block Excel 4.0 Macro Sheets via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Excel4MacroSheets=2 in the Excel Group Policy FileBlock path. Instructs "
                + "Excel to open files containing Excel 4.0 macro worksheets (XLM macros) in "
                + "Protected View without permitting editing. XLM macros are a common malware "
                + "delivery vector because they bypass many modern security controls targeting "
                + "VBA.",
            Tags = ["office", "excel", "xlm", "macro", "fileblock", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Excel 4.0 macro sheets open in Protected View only — enforced by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Excel4MacroSheets", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Excel4MacroSheets")],
            DetectOps = [RegOp.CheckDword(FbKey, "Excel4MacroSheets", 2)],
        },
    ];
}
