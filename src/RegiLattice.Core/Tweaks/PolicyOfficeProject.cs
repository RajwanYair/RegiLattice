namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint v6.28.0 — PolicyOfficeProject
// Microsoft Project Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\MS Project\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Project security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

[TweakModule]
internal static class PolicyOfficeProject
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\MS Project\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\MS Project\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\MS Project\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offprj-pol-vba-block",
            Label = "Block All Project Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the Project Group Policy security path. Instructs "
                + "Project to disable all macros without notification — the highest security "
                + "level. Since this is a Policy path, users cannot lower the setting in "
                + "the Trust Center. Apply on machines where Project VBA macros are not "
                + "required.",
            Tags = ["office", "project", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all Project macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offprj-pol-disable-activex",
            Label = "Block All ActiveX Controls in Project via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the Project Group Policy security path. "
                + "Prevents ActiveX controls from running inside Project files regardless "
                + "of their safety marking. Reduces attack surface for project file "
                + "exploitation.",
            Tags = ["office", "project", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in Project — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offprj-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in Project via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the Project Group Policy security path. "
                + "Instructs Project to reject files whose content does not match their "
                + "extension. Level 2 = block mismatches without prompting, preventing "
                + "malicious files masquerading as .mpp project files.",
            Tags = ["office", "project", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types in Project — blocks extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offprj-pol-block-net-exec",
            Label = "Block Internet Content Execution in Project via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets blockcontentexecutionfrominternet=1 in the Project Group Policy "
                + "security path. Prevents Project from executing embedded scripts or code "
                + "in project files that were opened from Internet locations.",
            Tags = ["office", "project", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in Project.",
            ApplyOps = [RegOp.SetDword(SecKey, "blockcontentexecutionfrominternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "blockcontentexecutionfrominternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "blockcontentexecutionfrominternet", 1)],
        },
        new TweakDef
        {
            Id = "offprj-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to Project Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the Project Group Policy security path. Prevents "
                + "external applications and scripts from programmatically accessing the "
                + "Project VBA project object model, blocking lateral-movement techniques "
                + "used by macro malware in multi-app Office exploit chains.",
            Tags = ["office", "project", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the Project VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offprj-pol-no-macro-programs",
            Label = "Prevent Project Macros from Launching Programs via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RunProgramsInMacros=0 in the Project Group Policy security path. "
                + "Prevents Project macros from using Shell() or other APIs to launch "
                + "external executables, reducing the risk of project-file-based malware "
                + "dropping payloads.",
            Tags = ["office", "project", "macro", "shell", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Macro code in Project cannot launch external programs.",
            ApplyOps = [RegOp.SetDword(SecKey, "RunProgramsInMacros", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "RunProgramsInMacros")],
            DetectOps = [RegOp.CheckDword(SecKey, "RunProgramsInMacros", 0)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offprj-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in Project via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the Project Group Policy ProtectedView "
                + "path. Explicitly enforces that Protected View remains enabled for project "
                + "files downloaded from the Internet, preventing users from disabling this "
                + "protection in the Trust Center.",
            Tags = ["office", "project", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps Project Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offprj-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in Project via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the Project Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for project files opened "
                + "from unsafe locations (paths not on the Trusted Locations list).",
            Tags = ["office", "project", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps Project Protected View on for files from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offprj-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in Project via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the Project Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for Project files "
                + "received as email attachments — a common delivery vector. Prevents users "
                + "from disabling this protection in the Trust Center.",
            Tags = ["office", "project", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment Project files always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy file format blocking ───────────────────────────────────────
        new TweakDef
        {
            Id = "offprj-pol-fileblock-mpp2003",
            Label = "Block Project 2003 Binary Format Files via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Mpp2003Files=2 in the Project Group Policy FileBlock path. Instructs "
                + "Project to open older binary Project 2003 (.mpp/.mpt) files in Protected "
                + "View without allowing editing. Legacy binary project formats carry greater "
                + "risk than modern Open XML-based .mpp files.",
            Tags = ["office", "project", "fileblock", "legacy", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Project 2003 .mpp files open in Protected View (read-only) by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Mpp2003Files", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Mpp2003Files")],
            DetectOps = [RegOp.CheckDword(FbKey, "Mpp2003Files", 2)],
        },
    ];
}
