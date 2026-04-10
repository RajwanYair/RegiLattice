namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint v6.28.0 — PolicyOfficePublisher
// Microsoft Publisher Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\Publisher\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Publisher security settings so
// users cannot weaken them in the Trust Center. Distinct from the user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

internal static class PolicyOfficePublisher
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Publisher\Security";
    private const string PvKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Publisher\Security\ProtectedView";
    private const string FbKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Publisher\Security\FileBlock";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offpub-pol-vba-block",
            Label = "Block All Publisher Macros via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VBAWarnings=4 in the Publisher Group Policy security path. Instructs "
                + "Publisher to disable all macros without notification — the highest security "
                + "level. Since this is a Policy path, users cannot lower the setting in the "
                + "Trust Center. Useful in environments where Publisher is deployed but "
                + "VBA automation is not required.",
            Tags = ["office", "publisher", "macro", "vba", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all Publisher macros system-wide via enforced Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "offpub-pol-disable-activex",
            Label = "Block All ActiveX Controls in Publisher via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAllActiveX=1 in the Publisher Group Policy security path. "
                + "Prevents ActiveX controls from running inside Publisher publications "
                + "regardless of their safety marking. Reduces attack surface for "
                + "document-based exploitation.",
            Tags = ["office", "publisher", "activex", "policy", "security", "hardening"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks all ActiveX controls in Publisher — enforced by Group Policy.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "offpub-pol-extension-hardening",
            Label = "Enforce Strict File–Extension Matching in Publisher via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ExtensionHardening=2 in the Publisher Group Policy security path. "
                + "Instructs Publisher to reject files whose content does not match their "
                + "extension. Level 2 = block mismatches without prompting, preventing "
                + "executable content masquerading as Publisher files.",
            Tags = ["office", "publisher", "extension", "policy", "security", "hardening"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents opening disguised file types in Publisher — blocks extension mismatches.",
            ApplyOps = [RegOp.SetDword(SecKey, "ExtensionHardening", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ExtensionHardening")],
            DetectOps = [RegOp.CheckDword(SecKey, "ExtensionHardening", 2)],
        },
        new TweakDef
        {
            Id = "offpub-pol-block-net-exec",
            Label = "Block Internet Content Execution in Publisher via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets blockcontentexecutionfrominternet=1 in the Publisher Group Policy "
                + "security path. Prevents Publisher from executing embedded scripts or code "
                + "in publications that were opened from Internet locations.",
            Tags = ["office", "publisher", "internet", "execution", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks execution of Internet-sourced embedded content in Publisher.",
            ApplyOps = [RegOp.SetDword(SecKey, "blockcontentexecutionfrominternet", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "blockcontentexecutionfrominternet")],
            DetectOps = [RegOp.CheckDword(SecKey, "blockcontentexecutionfrominternet", 1)],
        },
        new TweakDef
        {
            Id = "offpub-pol-deny-vba-om",
            Label = "Deny Programmatic VBA Access to Publisher Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AccessVBOM=0 in the Publisher Group Policy security path. Prevents "
                + "external applications and scripts from programmatically accessing the "
                + "Publisher VBA project object model, blocking a lateral-movement vector "
                + "used by macro malware in multi-app Office exploit chains.",
            Tags = ["office", "publisher", "vba", "object-model", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks external programmatic access to the Publisher VBA object model.",
            ApplyOps = [RegOp.SetDword(SecKey, "AccessVBOM", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(SecKey, "AccessVBOM", 0)],
        },
        new TweakDef
        {
            Id = "offpub-pol-no-macro-programs",
            Label = "Prevent Publisher Macros from Launching Programs via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RunProgramsInMacros=0 in the Publisher Group Policy security path. "
                + "Prevents Publisher macros from using Shell() or other APIs to launch "
                + "external executables, reducing the risk of publication-based malware "
                + "dropping payloads.",
            Tags = ["office", "publisher", "macro", "shell", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Macro code in Publisher cannot launch external programs.",
            ApplyOps = [RegOp.SetDword(SecKey, "RunProgramsInMacros", 0)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "RunProgramsInMacros")],
            DetectOps = [RegOp.CheckDword(SecKey, "RunProgramsInMacros", 0)],
        },
        // ── Protected View enforcement ────────────────────────────────────────
        new TweakDef
        {
            Id = "offpub-pol-pv-internet",
            Label = "Enforce Protected View for Internet Files in Publisher via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableInternetFilesInPV=0 in the Publisher Group Policy ProtectedView "
                + "path. Explicitly enforces that Protected View remains enabled for "
                + "publications downloaded from the Internet, preventing users from disabling "
                + "this protection in the Trust Center.",
            Tags = ["office", "publisher", "protected-view", "internet", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps Publisher Protected View on for Internet files — enforced by policy.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableInternetFilesInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableInternetFilesInPV", 0)],
        },
        new TweakDef
        {
            Id = "offpub-pol-pv-unsafe-loc",
            Label = "Enforce Protected View for Unsafe Locations in Publisher via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableUnsafeLocationsInPV=0 in the Publisher Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for publications opened "
                + "from unsafe locations (paths not on the Trusted Locations list).",
            Tags = ["office", "publisher", "protected-view", "unsafe", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps Publisher Protected View on for publications from unsafe locations.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableUnsafeLocationsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableUnsafeLocationsInPV", 0)],
        },
        new TweakDef
        {
            Id = "offpub-pol-pv-attachments",
            Label = "Enforce Protected View for Email Attachments in Publisher via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableAttachmentsInPV=0 in the Publisher Group Policy ProtectedView "
                + "path. Enforces that Protected View remains enabled for Publisher files "
                + "received as email attachments — a common delivery vector. Prevents users "
                + "from disabling this protection in the Trust Center.",
            Tags = ["office", "publisher", "protected-view", "attachment", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Email attachment Publisher files always open in Protected View.",
            ApplyOps = [RegOp.SetDword(PvKey, "DisableAttachmentsInPV", 0)],
            RemoveOps = [RegOp.DeleteValue(PvKey, "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(PvKey, "DisableAttachmentsInPV", 0)],
        },
        // ── Legacy file format blocking ───────────────────────────────────────
        new TweakDef
        {
            Id = "offpub-pol-fileblock-pub98",
            Label = "Block Publisher 98 Binary Format Files via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Pub98Files=2 in the Publisher Group Policy FileBlock path. Instructs "
                + "Publisher to open older binary Publisher 98 (.pub) files in Protected "
                + "View without allowing editing. These legacy formats carry greater risk "
                + "than modern Open XML-based publications.",
            Tags = ["office", "publisher", "fileblock", "legacy", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Older Publisher 98 .pub files open in Protected View (read-only) by policy.",
            ApplyOps = [RegOp.SetDword(FbKey, "Pub98Files", 2)],
            RemoveOps = [RegOp.DeleteValue(FbKey, "Pub98Files")],
            DetectOps = [RegOp.CheckDword(FbKey, "Pub98Files", 2)],
        },
    ];
}
