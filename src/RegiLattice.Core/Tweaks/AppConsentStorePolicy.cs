// RegiLattice.Core — Tweaks/AppConsentStorePolicy.cs
// Application Consent Store Group Policy — Sprint 188.
// Controls app consent store access, user consent prompts, and administrative
// consent approval requirements via Group Policy registry settings.
// Category: "App Consent Policy" | Slug: acspol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\AppConsentStore

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppConsentStorePolicy
{
    private const string AcsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppConsentStore";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "acspol-disable-consent-store",
                Label = "Disable App Consent Store",
                Category = "App Consent Policy",
                Description =
                    "Sets Enable=0 to disable the Windows App Consent Store that tracks and manages per-app privacy consent decisions. Apps requiring user consent are denied automatically.",
                Tags = ["consent", "privacy", "apps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "App consent tracking disabled; may silently deny app access to calendar, contacts, etc.",
                ApplyOps = [RegOp.SetDword(AcsKey, "Enable", 0)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "Enable")],
                DetectOps = [RegOp.CheckDword(AcsKey, "Enable", 0)],
            },
            new TweakDef
            {
                Id = "acspol-restrict-app-consent-grants",
                Label = "Restrict Automatic App Consent Grants",
                Category = "App Consent Policy",
                Description =
                    "Sets AllowConsentForApps=0 to prevent apps from receiving automatic consent grants. Every app consent request will require explicit user approval.",
                Tags = ["consent", "privacy", "apps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Auto-consent disabled; users must manually approve each app's permission request.",
                ApplyOps = [RegOp.SetDword(AcsKey, "AllowConsentForApps", 0)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "AllowConsentForApps")],
                DetectOps = [RegOp.CheckDword(AcsKey, "AllowConsentForApps", 0)],
            },
            new TweakDef
            {
                Id = "acspol-block-sensitive-consent",
                Label = "Block Sensitive Information App Consent",
                Category = "App Consent Policy",
                Description =
                    "Sets AllowSensitiveConsentForApps=0 to block apps from requesting consent to access sensitive personal information categories such as health, financial, or communication data.",
                Tags = ["consent", "sensitive", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Apps cannot request sensitive data consent; protects personal health/financial info.",
                ApplyOps = [RegOp.SetDword(AcsKey, "AllowSensitiveConsentForApps", 0)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "AllowSensitiveConsentForApps")],
                DetectOps = [RegOp.CheckDword(AcsKey, "AllowSensitiveConsentForApps", 0)],
            },
            new TweakDef
            {
                Id = "acspol-disable-consent-ux",
                Label = "Disable App Consent User Interface",
                Category = "App Consent Policy",
                Description =
                    "Sets DisableConsentUx=1 to suppress the app consent dialog UI. Consent decisions are handled silently according to current policy without surfacing prompts to the user.",
                Tags = ["consent", "ui", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Consent prompts hidden; apps receive policy-driven responses without user interaction.",
                ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentUx", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentUx")],
                DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentUx", 1)],
            },
            new TweakDef
            {
                Id = "acspol-require-admin-consent-approval",
                Label = "Require Administrator Consent Approval",
                Category = "App Consent Policy",
                Description =
                    "Sets RequireAdminApproval=1 so that all app consent requests must be explicitly approved by an administrator. Standard users cannot grant app permissions independently.",
                Tags = ["consent", "admin", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot grant app permissions; admin approval required for each consent.",
                ApplyOps = [RegOp.SetDword(AcsKey, "RequireAdminApproval", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "RequireAdminApproval")],
                DetectOps = [RegOp.CheckDword(AcsKey, "RequireAdminApproval", 1)],
            },
            new TweakDef
            {
                Id = "acspol-disable-consent-prompts",
                Label = "Disable App Consent Prompts for Standard Users",
                Category = "App Consent Policy",
                Description =
                    "Sets DisableConsentPrompts=1 to prevent consent dialog prompts from appearing for standard users. All consent decisions are handled by Group Policy settings.",
                Tags = ["consent", "prompts", "users", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Consent prompts removed for standard users; policy-defined defaults apply silently.",
                ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentPrompts", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentPrompts")],
                DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentPrompts", 1)],
            },
            new TweakDef
            {
                Id = "acspol-block-third-party-app-consent",
                Label = "Block Third-Party App Consent Requests",
                Category = "App Consent Policy",
                Description =
                    "Sets BlockThirdPartyConsent=1 to prevent sideloaded or third-party applications from requesting access to sensitive resources through the consent store.",
                Tags = ["consent", "third-party", "sideload", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Third-party apps blocked from requesting consent; Store and enterprise apps are unaffected.",
                ApplyOps = [RegOp.SetDword(AcsKey, "BlockThirdPartyConsent", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "BlockThirdPartyConsent")],
                DetectOps = [RegOp.CheckDword(AcsKey, "BlockThirdPartyConsent", 1)],
            },
            new TweakDef
            {
                Id = "acspol-disable-consent-history",
                Label = "Disable App Consent Decision History",
                Category = "App Consent Policy",
                Description =
                    "Sets DisableConsentHistory=1 to prevent the consent store from recording a history of app consent decisions. Improves privacy by not persisting consent audit trails locally.",
                Tags = ["consent", "history", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Consent decision history not stored locally; no audit trail of past app permission grants.",
                ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentHistory")],
                DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentHistory", 1)],
            },
            new TweakDef
            {
                Id = "acspol-restrict-consent-data-collection",
                Label = "Restrict Consent Data Collection by Apps",
                Category = "App Consent Policy",
                Description =
                    "Sets RestrictConsentDataCollection=1 to limit the types of data that applications can be granted consent to collect through the Windows consent store framework.",
                Tags = ["consent", "data-collection", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Apps limited in the scope of data they can collect through consent grants.",
                ApplyOps = [RegOp.SetDword(AcsKey, "RestrictConsentDataCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "RestrictConsentDataCollection")],
                DetectOps = [RegOp.CheckDword(AcsKey, "RestrictConsentDataCollection", 1)],
            },
            new TweakDef
            {
                Id = "acspol-disable-consent-notifications",
                Label = "Disable App Consent Change Notifications",
                Category = "App Consent Policy",
                Description =
                    "Sets DisableConsentNotifications=1 to suppress system notifications when apps are granted or denied consent to access resources. Reduces noise from consent-related toasts.",
                Tags = ["consent", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Consent-related toast notifications suppressed; consent decisions still take effect silently.",
                ApplyOps = [RegOp.SetDword(AcsKey, "DisableConsentNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(AcsKey, "DisableConsentNotifications")],
                DetectOps = [RegOp.CheckDword(AcsKey, "DisableConsentNotifications", 1)],
            },
        ];
}
