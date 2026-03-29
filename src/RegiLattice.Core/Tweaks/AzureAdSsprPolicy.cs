// RegiLattice.Core — Tweaks/AzureAdSsprPolicy.cs
// Azure AD Self-Service Password Reset (SSPR) authentication method and policy controls — Sprint 460.
// Category: "Azure AD SSPR Policy" | Slug: aadsspr
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SSPR

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AzureAdSsprPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SSPR";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aadsspr-require-two-methods",
                Label = "Require Two Methods for SSPR Authentication",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Configures Azure AD Self-Service Password Reset to require two authentication methods (e.g., email + phone, or authenticator app + security questions) before a password can be reset.",
                Tags = ["azure-ad", "sspr", "mfa", "password-reset", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Two SSPR methods required; phishing a single factor is insufficient for account takeover via SSPR.",
                ApplyOps = [RegOp.SetDword(Key, "NumberOfMethodsRequired", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "NumberOfMethodsRequired")],
                DetectOps = [RegOp.CheckDword(Key, "NumberOfMethodsRequired", 2)],
            },
            new TweakDef
            {
                Id = "aadsspr-block-sms-method",
                Label = "Block SMS as SSPR Authentication Method",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Disables SMS (one-time PIN via text message) as an allowed authentication method for SSPR, preventing SIM-swapping attacks from enabling account takeover via password reset.",
                Tags = ["azure-ad", "sspr", "sms", "sim-swap", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SMS SSPR blocked; users must use authenticator app, email, or FIDO2 for reset.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSMSMethod", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSMSMethod")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSMSMethod", 1)],
            },
            new TweakDef
            {
                Id = "aadsspr-block-email-method",
                Label = "Block External Email as SSPR Authentication Method",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Disables external email address (non-corporate) as an allowed authentication method for SSPR, forcing use of corporate email or authenticator apps.",
                Tags = ["azure-ad", "sspr", "email", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "External email SSPR method blocked; reduces attack surface through personal email accounts.",
                ApplyOps = [RegOp.SetDword(Key, "DisableExternalEmailMethod", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExternalEmailMethod")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExternalEmailMethod", 1)],
            },
            new TweakDef
            {
                Id = "aadsspr-require-security-questions-count",
                Label = "Require Minimum 5 Security Questions for SSPR",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Requires users to answer at least 5 security questions for SSPR if the security questions method is enabled, increasing the difficulty of social engineering attacks.",
                Tags = ["azure-ad", "sspr", "security-questions", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "5 security questions required for SSPR; harder to socially engineer an account reset.",
                ApplyOps = [RegOp.SetDword(Key, "NumberOfSecurityQuestionsRequired", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "NumberOfSecurityQuestionsRequired")],
                DetectOps = [RegOp.CheckDword(Key, "NumberOfSecurityQuestionsRequired", 5)],
            },
            new TweakDef
            {
                Id = "aadsspr-enforce-registration-at-logon",
                Label = "Enforce SSPR Registration at Next Logon",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Forces users who have not registered SSPR methods to register at their next logon, ensuring all accounts have recovery methods configured before they need them.",
                Tags = ["azure-ad", "sspr", "registration", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Unregistered users prompted to register SSPR methods at logon; one-time interruption.",
                ApplyOps = [RegOp.SetDword(Key, "RequireRegistrationAtLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireRegistrationAtLogon")],
                DetectOps = [RegOp.CheckDword(Key, "RequireRegistrationAtLogon", 1)],
            },
            new TweakDef
            {
                Id = "aadsspr-set-reconfirm-interval",
                Label = "Set SSPR Auth Method Reconfirmation to 180 Days",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Configures the SSPR method reconfirmation interval to 180 days, requiring users to verify their registered authentication methods are still valid every six months.",
                Tags = ["azure-ad", "sspr", "reconfirmation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SSPR methods verified every 6 months; stale phone numbers or emails are caught and updated.",
                ApplyOps = [RegOp.SetDword(Key, "ReconfirmAuthMethodsIntervalDays", 180)],
                RemoveOps = [RegOp.DeleteValue(Key, "ReconfirmAuthMethodsIntervalDays")],
                DetectOps = [RegOp.CheckDword(Key, "ReconfirmAuthMethodsIntervalDays", 180)],
            },
            new TweakDef
            {
                Id = "aadsspr-require-authenticator-app",
                Label = "Require Microsoft Authenticator App for SSPR",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Requires the Microsoft Authenticator app notification or code as an allowed (and preferred) SSPR method, providing stronger MFA-equivalent strength for password resets.",
                Tags = ["azure-ad", "sspr", "authenticator", "mfa", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Authenticator app required for SSPR; users without the app need to register alternative strong methods.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAuthenticatorApp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthenticatorApp")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAuthenticatorApp", 1)],
            },
            new TweakDef
            {
                Id = "aadsspr-block-writeback-on-premises",
                Label = "Block SSPR Password Writeback to On-Premises",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Disables SSPR Password Writeback which synchronises cloud reset passwords back to the on-premises Active Directory, preventing cloud-initiated account takeover from affecting on-prem.",
                Tags = ["azure-ad", "sspr", "writeback", "on-premises", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Password Writeback disabled; cloud SSPR resets do not propagate to on-prem AD.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordWriteback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordWriteback")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordWriteback", 1)],
            },
            new TweakDef
            {
                Id = "aadsspr-notify-admin-on-reset",
                Label = "Notify Admins on SSPR Use",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Enables administrator notification when a user uses SSPR to reset their password, creating an audit trail and alerting IT to potential account compromise events.",
                Tags = ["azure-ad", "sspr", "notification", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IT admins notified on each SSPR reset event; allows rapid response to suspicious resets.",
                ApplyOps = [RegOp.SetDword(Key, "NotifyAdminsOnUserPasswordReset", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NotifyAdminsOnUserPasswordReset")],
                DetectOps = [RegOp.CheckDword(Key, "NotifyAdminsOnUserPasswordReset", 1)],
            },
            new TweakDef
            {
                Id = "aadsspr-limit-self-service-unlock",
                Label = "Limit SSPR Self-Service Account Unlock Attempts",
                Category = "Azure AD SSPR Policy",
                Description =
                    "Limits the number of self-service account unlock attempts via SSPR per hour to prevent brute-force enumeration of SSPR methods against locked accounts.",
                Tags = ["azure-ad", "sspr", "account-unlock", "brute-force", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SSPR unlock attempts rate-limited; brute-force account enumeration via unlock is prevented.",
                ApplyOps = [RegOp.SetDword(Key, "MaxUnlockAttemptsPerHour", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxUnlockAttemptsPerHour")],
                DetectOps = [RegOp.CheckDword(Key, "MaxUnlockAttemptsPerHour", 3)],
            },
        ];
}
