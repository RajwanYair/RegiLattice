namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint v6.28.0 — PolicyOfficeAccess
// Microsoft Access Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\Access\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Access security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

[TweakModule]
internal static class PolicyOfficeAccess
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Access\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Access\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Access\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offacc-pol-vba-block",
            Label = "Block All Access Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the Access Group Policy security path. Instructs "
                + "Access to disable all macros without notification — the highest security "
                + "level. Since this is a Policy path, users cannot lower the setting in the "
                + "Trust Center. Apply on machines where Access VBA macros are not required.",
            Tags = ["office", "access", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all Access macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offacc-pol-disable-activex",
            Label = "Block All ActiveX Controls in Access via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the Access Group Policy security path. "
                + "Prevents ActiveX controls from running in Access databases regardless "
                + "of their safety marking. Eliminates a common attack vector for "
                + "database-based exploits.",
            Tags = ["office", "access", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in Access — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offacc-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in Access via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the Access Group Policy security path. "
                + "Instructs Access to reject files whose content does not match their "
                + "extension. Level 2 = block mismatches without prompting, preventing "
                + "executable content masquerading as Access database files.",
            Tags = ["office", "access", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types in Access — blocks extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offacc-pol-block-net-exec",
            Label = "Block Internet Content Execution in Access via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets blockcontentexecutionfrominternet=1 in the Access Group Policy "
                + "security path. Prevents Access from executing content embedded in "
                + "database files that were opened from Internet locations.",
            Tags = ["office", "access", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in Access.",
            ApplyOps = [RegOp.SetDword(SecKey, "blockcontentexecutionfrominternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "blockcontentexecutionfrominternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "blockcontentexecutionfrominternet", 1)],
        },
        new TweakDef
        {
            Id = "offacc-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to Access Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the Access Group Policy security path. Prevents "
                + "external applications and scripts from programmatically accessing the "
                + "Access VBA project object model, blocking lateral-movement techniques "
                + "used by macro malware to propagate across Office applications.",
            Tags = ["office", "access", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the Access VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offacc-pol-sandbox-mode",
            Label = "Enforce Access Sandbox Mode via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SandboxMode=3 in the Access Group Policy security path. Forces "
                + "Access to run expressions in sandbox mode, blocking potentially harmful "
                + "expressions and VBA calls that could execute system commands. Value 3 = "
                + "full sandbox restriction for both untrusted and trusted databases.",
            Tags = ["office", "access", "sandbox", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Runs Access expressions in strict sandbox — blocks system command execution.",
            ApplyOps = [RegOp.SetDword(SecKey, "SandboxMode", 3)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "SandboxMode")],
            DetectOps = [RegOp.CheckDword(SecKey, "SandboxMode", 3)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offacc-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in Access via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the Access Group Policy ProtectedView "
                + "path. Explicitly enforces that Protected View remains enabled for "
                + "databases downloaded from the Internet, preventing users from disabling "
                + "this protection in the Trust Center.",
            Tags = ["office", "access", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps Access Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offacc-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in Access via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the Access Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for databases opened "
                + "from unsafe locations (paths not on the Trusted Locations list).",
            Tags = ["office", "access", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps Access Protected View on for databases from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offacc-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in Access via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the Access Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for Access databases "
                + "received as email attachments — a high-risk delivery vector for database "
                + "exploits. Prevents users from disabling this protection.",
            Tags = ["office", "access", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment Access databases always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy file format blocking ───────────────────────────────────────
        new TweakDef
        {
            Id = "offacc-pol-fileblock-mdb",
            Label = "Block Access 2007 and Earlier MDB Files via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Ac2007Files=2 in the Access Group Policy FileBlock path. Instructs "
                + "Access to open older .mdb/.mde binary database files in Protected View "
                + "without allowing editing. Legacy binary formats have larger attack surfaces "
                + "than .accdb/OOXML formats and may contain undetected malicious content.",
            Tags = ["office", "access", "fileblock", "legacy", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Older .mdb files open in Protected View (read-only) by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Ac2007Files", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Ac2007Files")],
            DetectOps = [RegOp.CheckDword(FbKey, "Ac2007Files", 2)],
        },
    ];
}
