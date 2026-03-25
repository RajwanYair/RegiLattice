#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 251 — Fax Service Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Fax
//       HKCU\Software\Policies\Microsoft\Windows NT\Fax
internal static class FaxServicePolicy
{
    private const string FaxLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Fax";
    private const string FaxCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows NT\Fax";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "faxsvc-disable-fax",
            Label = "Disable Fax Service",
            Category = "Fax Service Policy",
            Description =
                "Sets Fax=1 in the machine Fax policy key under DisabledComponents. "
                + "Configures Windows Group Policy to mark the Fax service component as disabled at the policy level. "
                + "Prevents fax services from being used on machines where faxing functionality is not required. "
                + "Default: absent (Fax service allowed). Recommended: 1 on machines with no fax hardware or requirements.",
            Tags = ["fax", "service", "disable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Fax service restricted at the Group Policy level; fax send/receive operations are blocked.",
            ApplyOps = [RegOp.SetDword(FaxLm, "Fax", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "Fax")],
            DetectOps = [RegOp.CheckDword(FaxLm, "Fax", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-online-fax",
            Label = "Disable Online Fax Service",
            Category = "Fax Service Policy",
            Description =
                "Sets OnlineFax=1 in the machine Fax policy key. "
                + "Prevents users from sending faxes via online fax providers or cloud-based fax services. "
                + "Blocks the 'Connect to a fax modem' and online fax integration from the Windows fax tools UI. "
                + "Default: absent (online fax allowed). Recommended: 1 to prevent unsanctioned cloud fax usage.",
            Tags = ["fax", "online", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Online/cloud fax providers blocked at the policy level; only local modem-based fax permitted.",
            ApplyOps = [RegOp.SetDword(FaxLm, "OnlineFax", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "OnlineFax")],
            DetectOps = [RegOp.CheckDword(FaxLm, "OnlineFax", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-cover-pages",
            Label = "Disable Fax Cover Pages",
            Category = "Fax Service Policy",
            Description =
                "Sets CoverPages=1 in the machine Fax policy key. "
                + "Prevents users from attaching cover pages to faxes sent through the Windows fax tool. "
                + "Useful in environments that use standardised fax headers from the PBX or fax server. "
                + "Default: absent (cover pages allowed). Recommended: 1 when corporate headers are auto-applied by the fax server.",
            Tags = ["fax", "cover-page", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Fax cover page attachment disabled; outgoing faxes are sent without a Windows-generated cover page.",
            ApplyOps = [RegOp.SetDword(FaxLm, "CoverPages", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "CoverPages")],
            DetectOps = [RegOp.CheckDword(FaxLm, "CoverPages", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-personal-cover-pages",
            Label = "Disable Personal Fax Cover Pages",
            Category = "Fax Service Policy",
            Description =
                "Sets PersonalCoverPages=1 in the machine Fax policy key. "
                + "Prevents users from creating or storing personal fax cover page templates ("
                + ".cov files) on their profile, restricting cover page management to IT-distributed templates. "
                + "Default: absent (personal covers allowed). Recommended: 1 to enforce corporate cover page standards.",
            Tags = ["fax", "cover-page", "personal", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Personal fax cover page creation and storage blocked; only shared network templates usable.",
            ApplyOps = [RegOp.SetDword(FaxLm, "PersonalCoverPages", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "PersonalCoverPages")],
            DetectOps = [RegOp.CheckDword(FaxLm, "PersonalCoverPages", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-recipients",
            Label = "Disable Fax Recipient Book",
            Category = "Fax Service Policy",
            Description =
                "Sets DisableRecipients=1 in the machine Fax policy key. "
                + "Removes the 'Select Recipients' feature from the Windows Fax and Scan UI, "
                + "preventing users from building a personal fax contacts book. "
                + "Default: absent (recipient book enabled). Recommended: 1 when fax routing is managed centrally.",
            Tags = ["fax", "recipients", "contacts", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Personal fax recipient book removed from Windows Fax and Scan UI.",
            ApplyOps = [RegOp.SetDword(FaxLm, "DisableRecipients", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "DisableRecipients")],
            DetectOps = [RegOp.CheckDword(FaxLm, "DisableRecipients", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-require-send-tapi",
            Label = "Restrict Fax to TAPI Lines Only",
            Category = "Fax Service Policy",
            Description =
                "Sets TapiOnly=1 in the machine Fax policy key. "
                + "Forces the Windows Fax service to use only TAPI-registered lines for sending faxes, "
                + "preventing direct modem or non-TAPI send paths. Ensures fax traffic flows through audited channels. "
                + "Default: absent (all send paths allowed). Recommended: 1 to ensure audit trail compliance.",
            Tags = ["fax", "tapi", "modem", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Fax transmission restricted to TAPI-registered phone lines; non-TAPI fax paths are blocked.",
            ApplyOps = [RegOp.SetDword(FaxLm, "TapiOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "TapiOnly")],
            DetectOps = [RegOp.CheckDword(FaxLm, "TapiOnly", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-inbound-routing",
            Label = "Disable Inbound Fax Routing",
            Category = "Fax Service Policy",
            Description =
                "Sets InboundRouting=1 in the machine Fax policy key. "
                + "Prevents the Windows fax service from routing incoming faxes to user inboxes or email. "
                + "Forces a passive receive mode where received faxes are not forwarded or archived automatically. "
                + "Default: absent (inbound routing enabled). Recommended: 1 when the PBX or upstream fax server handles routing.",
            Tags = ["fax", "inbound", "routing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Inbound fax auto-routing to user mailboxes or folders is disabled.",
            ApplyOps = [RegOp.SetDword(FaxLm, "InboundRouting", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "InboundRouting")],
            DetectOps = [RegOp.CheckDword(FaxLm, "InboundRouting", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-archive",
            Label = "Disable Fax Archive",
            Category = "Fax Service Policy",
            Description =
                "Sets Archive=1 in the machine Fax policy key. "
                + "Prevents the Windows fax service from automatically archiving copies of sent and received faxes. "
                + "Useful when archiving is handled by a dedicated fax compliance server and client-side archive is redundant. "
                + "Default: absent (archive enabled). Recommended: 1 when a server-side archive is the sole authoritative copy.",
            Tags = ["fax", "archive", "retention", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Client-side fax archive disabled; faxes won't be stored locally in the Windows Fax and Scan archive.",
            ApplyOps = [RegOp.SetDword(FaxLm, "Archive", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "Archive")],
            DetectOps = [RegOp.CheckDword(FaxLm, "Archive", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-fax-user",
            Label = "Disable Fax for Current User",
            Category = "Fax Service Policy",
            Description =
                "Sets Fax=1 in the per-user Fax policy key. "
                + "Applies the fax disable policy for the current user only, without requiring a machine-wide GPO. "
                + "Useful in BYOD or per-user policy environments where fax usage is restricted to specific roles. "
                + "Default: absent (fax allowed for user). Recommended: 1 for non-fax users on a shared machine.",
            Tags = ["fax", "user", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Fax service restricted for the current user profile only.",
            ApplyOps = [RegOp.SetDword(FaxCu, "Fax", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxCu, "Fax")],
            DetectOps = [RegOp.CheckDword(FaxCu, "Fax", 1)],
        },
        new TweakDef
        {
            Id = "faxsvc-disable-new-account",
            Label = "Disable Fax New Account Creation",
            Category = "Fax Service Policy",
            Description =
                "Sets NewAccounts=1 in the machine Fax policy key. "
                + "Prevents users from adding new fax accounts or configuring additional fax connections in Windows. "
                + "Ensures fax account provisioning is controlled by IT and prevents shadow fax connections. "
                + "Default: absent (new account creation allowed). Recommended: 1 to lock down fax account provisioning.",
            Tags = ["fax", "account", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users cannot add new fax accounts; all fax connections must be IT-provisioned.",
            ApplyOps = [RegOp.SetDword(FaxLm, "NewAccounts", 1)],
            RemoveOps = [RegOp.DeleteValue(FaxLm, "NewAccounts")],
            DetectOps = [RegOp.CheckDword(FaxLm, "NewAccounts", 1)],
        },
    ];
}
