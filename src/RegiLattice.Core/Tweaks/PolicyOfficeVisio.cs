namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint v6.28.0 — PolicyOfficeVisio
// Microsoft Visio Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\Visio\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Visio security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

internal static class PolicyOfficeVisio
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Visio\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Visio\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Visio\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offvis-pol-vba-block",
            Label = "Block All Visio Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the Visio Group Policy security path. Instructs "
                + "Visio to disable all macros without notification — the highest security "
                + "level. Since this is a Policy path, users cannot lower the setting in "
                + "the Trust Center. Apply on machines where Visio VBA macros are not "
                + "required.",
            Tags = ["office", "visio", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all Visio macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offvis-pol-disable-activex",
            Label = "Block All ActiveX Controls in Visio via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the Visio Group Policy security path. "
                + "Prevents ActiveX controls from running inside Visio diagrams regardless "
                + "of their safety marking. Reduces attack surface for diagram-based "
                + "exploitation.",
            Tags = ["office", "visio", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in Visio — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offvis-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in Visio via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the Visio Group Policy security path. "
                + "Instructs Visio to reject files whose content does not match their "
                + "extension. Level 2 = block mismatches without prompting, preventing "
                + "executable content masquerading as Visio diagram files.",
            Tags = ["office", "visio", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types in Visio — blocks extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offvis-pol-block-net-exec",
            Label = "Block Internet Content Execution in Visio via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets blockcontentexecutionfrominternet=1 in the Visio Group Policy "
                + "security path. Prevents Visio from executing embedded scripts or code "
                + "in diagrams that were opened from Internet locations.",
            Tags = ["office", "visio", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in Visio.",
            ApplyOps = [RegOp.SetDword(SecKey, "blockcontentexecutionfrominternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "blockcontentexecutionfrominternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "blockcontentexecutionfrominternet", 1)],
        },
        new TweakDef
        {
            Id = "offvis-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to Visio Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the Visio Group Policy security path. Prevents "
                + "external applications and scripts from programmatically accessing the "
                + "Visio VBA project object model, blocking lateral-movement techniques "
                + "used by macro malware.",
            Tags = ["office", "visio", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the Visio VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offvis-pol-no-macro-programs",
            Label = "Prevent Visio Macros from Launching Programs via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RunProgramsInMacros=0 in the Visio Group Policy security path. "
                + "Prevents Visio macros from using Shell() or other APIs to launch "
                + "external executables, reducing the risk of diagram-based malware "
                + "dropping payloads.",
            Tags = ["office", "visio", "macro", "shell", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Macro code in Visio cannot launch external programs.",
            ApplyOps = [RegOp.SetDword(SecKey, "RunProgramsInMacros", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "RunProgramsInMacros")],
            DetectOps = [RegOp.CheckDword(SecKey, "RunProgramsInMacros", 0)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offvis-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in Visio via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the Visio Group Policy ProtectedView "
                + "path. Explicitly enforces that Protected View remains enabled for "
                + "diagrams downloaded from the Internet, preventing users from disabling "
                + "this protection in the Trust Center.",
            Tags = ["office", "visio", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps Visio Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offvis-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in Visio via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the Visio Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for diagrams opened "
                + "from unsafe locations (paths not on the Trusted Locations list).",
            Tags = ["office", "visio", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps Visio Protected View on for diagrams from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offvis-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in Visio via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the Visio Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for Visio diagrams "
                + "received as email attachments — a common delivery vector. Prevents users "
                + "from disabling this protection in the Trust Center.",
            Tags = ["office", "visio", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment Visio diagrams always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy file format blocking ───────────────────────────────────────
        new TweakDef
        {
            Id = "offvis-pol-fileblock-vsd2003",
            Label = "Block Visio 2003 Binary Format Files via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Vis2003Files=2 in the Visio Group Policy FileBlock path. Instructs "
                + "Visio to open older Visio 2003 binary (.vsd/.vss) files in Protected "
                + "View without allowing editing. Legacy binary diagram formats carry "
                + "greater risk than modern .vsdx/Open XML formats.",
            Tags = ["office", "visio", "fileblock", "legacy", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Visio 2003 .vsd files open in Protected View (read-only) by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Vis2003Files", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Vis2003Files")],
            DetectOps = [RegOp.CheckDword(FbKey, "Vis2003Files", 2)],
        },
    ];
}
