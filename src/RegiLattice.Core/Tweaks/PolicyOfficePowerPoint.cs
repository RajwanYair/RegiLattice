namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint v6.28.0 — PolicyOfficePowerPoint
// Microsoft PowerPoint Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\PowerPoint\Security (and sub-keys)
// These are Group Policy enforcement paths that lock PowerPoint security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

internal static class PolicyOfficePowerPoint
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\PowerPoint\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\PowerPoint\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\PowerPoint\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offppt-pol-vba-block",
            Label = "Block All PowerPoint Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the PowerPoint Group Policy security path. Instructs "
                + "PowerPoint to disable all macros without notification — the highest security "
                + "level. Since this is a Policy path, users cannot lower the setting in the "
                + "Trust Center. Apply on machines where VBA macros are not required.",
            Tags = ["office", "powerpoint", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all PowerPoint macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offppt-pol-disable-activex",
            Label = "Block All ActiveX Controls in PowerPoint via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the PowerPoint Group Policy security path. "
                + "Prevents ActiveX controls from running in PowerPoint presentations "
                + "regardless of their safety marking. Eliminates a common attack vector "
                + "for presentation-based exploits.",
            Tags = ["office", "powerpoint", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in PowerPoint — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offppt-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in PowerPoint via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the PowerPoint Group Policy security path. "
                + "Instructs PowerPoint to reject files whose content does not match their "
                + "extension (e.g., a renamed executable masquerading as .pptx). Level 2 = "
                + "block mismatches without prompting.",
            Tags = ["office", "powerpoint", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types — blocks all extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offppt-pol-block-net-exec",
            Label = "Block Internet Content Execution in PowerPoint via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets blockcontentexecutionfrominternet=1 in the PowerPoint Group Policy "
                + "security path. Prevents PowerPoint from executing content (scripts, code) "
                + "embedded in presentations that were opened from Internet locations.",
            Tags = ["office", "powerpoint", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in PowerPoint.",
            ApplyOps = [RegOp.SetDword(SecKey, "blockcontentexecutionfrominternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "blockcontentexecutionfrominternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "blockcontentexecutionfrominternet", 1)],
        },
        new TweakDef
        {
            Id = "offppt-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to PowerPoint Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the PowerPoint Group Policy security path. Prevents "
                + "external applications and scripts from programmatically accessing the "
                + "PowerPoint VBA project object model, blocking a common lateral-movement "
                + "technique used by macro malware to propagate across Office applications.",
            Tags = ["office", "powerpoint", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the PowerPoint VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offppt-pol-no-macro-programs",
            Label = "Prevent PowerPoint Macros from Launching Programs via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RunProgramsInMacros=0 in the PowerPoint Group Policy security path. "
                + "Prevents PowerPoint macros from using Shell() or other APIs to launch "
                + "external executables, reducing the risk of presentation-based malware "
                + "dropping and executing payloads.",
            Tags = ["office", "powerpoint", "macro", "shell", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Macro code in PowerPoint cannot launch external programs.",
            ApplyOps = [RegOp.SetDword(SecKey, "RunProgramsInMacros", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "RunProgramsInMacros")],
            DetectOps = [RegOp.CheckDword(SecKey, "RunProgramsInMacros", 0)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offppt-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in PowerPoint via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the PowerPoint Group Policy ProtectedView "
                + "path. Explicitly enforces that Protected View remains enabled for "
                + "presentations downloaded from the Internet, preventing users from "
                + "disabling this protection in the Trust Center.",
            Tags = ["office", "powerpoint", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps PowerPoint Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offppt-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in PowerPoint via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the PowerPoint Group Policy "
                + "ProtectedView path. Enforces that Protected View remains enabled for "
                + "presentations opened from unsafe locations (paths not on the Trusted "
                + "Locations list).",
            Tags = ["office", "powerpoint", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps PowerPoint Protected View on for presentations from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offppt-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in PowerPoint via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the PowerPoint Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for PowerPoint "
                + "presentations received as email attachments — one of the highest-risk "
                + "delivery vectors. Prevents users from disabling this protection.",
            Tags = ["office", "powerpoint", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment PowerPoint files always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy file format blocking ───────────────────────────────────────
        new TweakDef
        {
            Id = "offppt-pol-fileblock-ppt97",
            Label = "Block PowerPoint 97 Binary Format Files via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Ppt97Files=2 in the PowerPoint Group Policy FileBlock path. Instructs "
                + "PowerPoint to open PowerPoint 97 binary (.ppt) files in Protected View "
                + "without allowing editing. Legacy binary formats have larger attack surfaces "
                + "than .pptx/OOXML formats and are commonly used as malware delivery vectors.",
            Tags = ["office", "powerpoint", "fileblock", "legacy", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "PowerPoint 97 .ppt files open in Protected View (read-only) by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Ppt97Files", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Ppt97Files")],
            DetectOps = [RegOp.CheckDword(FbKey, "Ppt97Files", 2)],
        },
    ];
}
