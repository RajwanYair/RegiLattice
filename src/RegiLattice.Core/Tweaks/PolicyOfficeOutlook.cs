namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 688 — PolicyOfficeOutlook
// Microsoft Outlook Group Policy security enforcement (10 tweaks).
// Registry: HKCU\Software\Policies\Microsoft\Office\16.0\Outlook\Security (and sub-keys)
// These are Group Policy enforcement paths that lock Outlook security settings so
// users cannot weaken them in the Trust Center / Options. Distinct from user-preference
// paths in Office.cs (HKCU\Software\Microsoft\Office\16.0\...).

[TweakModule]
internal static class PolicyOfficeOutlook
{
    private const string SecKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Outlook\Security";
    private const string MailKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Outlook\Options\Mail";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Macro and script security ─────────────────────────────────────────
        new TweakDef
        {
            Id = "offolt-pol-security-level",
            Label = "Set Outlook Macro Security to High via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Level=3 in the Outlook Group Policy security path. Enforces the 'High' "
                + "security level for Outlook scripts and add-ins — macros must be signed by a "
                + "trusted publisher or they are disabled. Users cannot lower this setting in "
                + "the Trust Center when set via Group Policy.",
            Tags = ["office", "outlook", "macro", "security", "policy", "level"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Outlook requires signed macros — unsigned scripts are blocked.",
            ApplyOps = [RegOp.SetDword(SecKey, "Level", 3)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "Level")],
            DetectOps = [RegOp.CheckDword(SecKey, "Level", 3)],
        },
        new TweakDef
        {
            Id = "offolt-pol-om-guard-deny",
            Label = "Deny Programmatic Access to Outlook Object Model via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ObjectModelGuard=2 in the Outlook Group Policy security path. Instructs "
                + "Outlook to deny all programmatic access to the Outlook object model from "
                + "external scripts and automation — the most secure setting. Prevents malware "
                + "and untrusted automation from reading email, accessing contacts, or sending "
                + "messages via the Outlook COM object.",
            Tags = ["office", "outlook", "object-model", "com", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks external automation from accessing Outlook email and contacts.",
            ApplyOps = [RegOp.SetDword(SecKey, "ObjectModelGuard", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "ObjectModelGuard")],
            DetectOps = [RegOp.CheckDword(SecKey, "ObjectModelGuard", 2)],
        },
        new TweakDef
        {
            Id = "offolt-pol-min-enc-bits",
            Label = "Enforce 256-Bit Minimum S/MIME Encryption Key via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MinEncKeyBits=256 in the Outlook Group Policy security path. Enforces "
                + "a minimum S/MIME message encryption key length of 256 bits, preventing the "
                + "use of weak 40-bit or 56-bit legacy encryption algorithms for secure email.",
            Tags = ["office", "outlook", "encryption", "smime", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "S/MIME messages require at least 256-bit encryption keys.",
            ApplyOps = [RegOp.SetDword(SecKey, "MinEncKeyBits", 256)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "MinEncKeyBits")],
            DetectOps = [RegOp.CheckDword(SecKey, "MinEncKeyBits", 256)],
        },
        new TweakDef
        {
            Id = "offolt-pol-vba-warnings",
            Label = "Enable VBA Security Warnings in Outlook via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EnableVBAWarnings=1 in the Outlook Group Policy security path. Ensures "
                + "that Outlook shows security warnings for VBA-related actions and untrusted "
                + "macro activities, keeping users informed when automation attempts to access "
                + "sensitive Outlook data.",
            Tags = ["office", "outlook", "vba", "warning", "policy", "security"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Outlook shows VBA security warnings — users are alerted to automation.",
            ApplyOps = [RegOp.SetDword(SecKey, "EnableVBAWarnings", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "EnableVBAWarnings")],
            DetectOps = [RegOp.CheckDword(SecKey, "EnableVBAWarnings", 1)],
        },
        new TweakDef
        {
            Id = "offolt-pol-invalid-sig-warn",
            Label = "Warn on Invalid Digital Signatures in Outlook via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets WarnAboutInvalidSignatures=1 in the Outlook Group Policy security path. "
                + "Instructs Outlook to generate a visible warning when a signed email message "
                + "has an invalid, expired, or untrusted digital signature, protecting against "
                + "spoofed secure-message indicators.",
            Tags = ["office", "outlook", "signature", "smime", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Users see a clear warning when a digital email signature is invalid.",
            ApplyOps = [RegOp.SetDword(SecKey, "WarnAboutInvalidSignatures", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "WarnAboutInvalidSignatures")],
            DetectOps = [RegOp.CheckDword(SecKey, "WarnAboutInvalidSignatures", 1)],
        },
        new TweakDef
        {
            Id = "offolt-pol-no-addin-customization",
            Label = "Prevent Add-in Customization Changes in Outlook via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableCustomizations=1 in the Outlook Group Policy security path. "
                + "Prevents users and untrusted add-ins from modifying the trusted add-in list "
                + "and other security customizations. Locks the current security configuration "
                + "against tampering by rogue or compromised add-ins.",
            Tags = ["office", "outlook", "addin", "customization", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Outlook security customizations cannot be changed by add-ins or users.",
            ApplyOps = [RegOp.SetDword(SecKey, "DisableCustomizations", 1)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "DisableCustomizations")],
            DetectOps = [RegOp.CheckDword(SecKey, "DisableCustomizations", 1)],
        },
        // ── External content and automation prompts ───────────────────────────
        new TweakDef
        {
            Id = "offolt-pol-block-ext-content",
            Label = "Block Auto-Download of External Images in Outlook via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets BlockExternalContent=1 in the Outlook Group Policy Options\\Mail path. "
                + "Prevents Outlook from automatically downloading images and other content "
                + "hosted on external servers when messages are viewed. Blocks read-receipt "
                + "pixel tracking and reduces exposure to drive-by web content.",
            Tags = ["office", "outlook", "tracking", "images", "policy", "privacy"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "External images are not downloaded — blocks email pixel tracking.",
            ApplyOps = [RegOp.SetDword(MailKey, "BlockExternalContent", 1)],
            RemoveOps = [RegOp.DeleteValue(MailKey, "BlockExternalContent")],
            DetectOps = [RegOp.CheckDword(MailKey, "BlockExternalContent", 1)],
        },
        new TweakDef
        {
            Id = "offolt-pol-prompt-addr-book",
            Label = "Warn When Macros Access Outlook Address Book via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PromptOOMAddressBookAccess=2 in the Outlook Group Policy security path. "
                + "Configures Outlook to prompt the user each time a macro or external program "
                + "attempts to access the address book. Value 2 = always show the access prompt "
                + "(user must approve or deny each time), unlike value 0 which auto-approves.",
            Tags = ["office", "outlook", "address-book", "macro", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "User sees a warning every time automation tries to access contacts.",
            ApplyOps = [RegOp.SetDword(SecKey, "PromptOOMAddressBookAccess", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "PromptOOMAddressBookAccess")],
            DetectOps = [RegOp.CheckDword(SecKey, "PromptOOMAddressBookAccess", 2)],
        },
        new TweakDef
        {
            Id = "offolt-pol-prompt-addr-info",
            Label = "Warn When Macros Access Outlook Address Information via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PromptOOMAddressInformationAccess=2 in the Outlook Group Policy security "
                + "path. Prompts the user each time a macro or external application attempts to "
                + "read recipient address information (To/CC/BCC fields, contact details). "
                + "Prevents silent address harvesting by malicious automation.",
            Tags = ["office", "outlook", "address", "macro", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "User sees a warning whenever automation reads email address data.",
            ApplyOps = [RegOp.SetDword(SecKey, "PromptOOMAddressInformationAccess", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "PromptOOMAddressInformationAccess")],
            DetectOps = [RegOp.CheckDword(SecKey, "PromptOOMAddressInformationAccess", 2)],
        },
        new TweakDef
        {
            Id = "offolt-pol-prompt-send-mail",
            Label = "Warn When Macros Send Email via Outlook via Group Policy",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PromptOOMSendMail=2 in the Outlook Group Policy security path. "
                + "Prompts the user each time a macro, script, or external application attempts "
                + "to send an email through Outlook. Prevents malware from silently using "
                + "Outlook as a relay to send spam, exfiltrate data, or spread via email.",
            Tags = ["office", "outlook", "send", "macro", "policy", "security"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "User must approve each time automation tries to send email via Outlook.",
            ApplyOps = [RegOp.SetDword(SecKey, "PromptOOMSendMail", 2)],
            RemoveOps = [RegOp.DeleteValue(SecKey, "PromptOOMSendMail")],
            DetectOps = [RegOp.CheckDword(SecKey, "PromptOOMSendMail", 2)],
        },
    ];
}
