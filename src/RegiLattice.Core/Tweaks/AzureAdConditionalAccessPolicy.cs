// RegiLattice.Core — Tweaks/AzureAdConditionalAccessPolicy.cs
// Azure AD Conditional Access enforcement, MFA requirements, and AAD account policy controls — Sprint 457.
// Category: "Azure AD Conditional Access Policy" | Slug: aadca
// Registry: HKLM\SOFTWARE\Policies\Microsoft\AzureADAccount
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AzureAdConditionalAccessPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AzureADAccount";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aadca-require-mfa-device-compliance",
                Label = "Require MFA and Device Compliance for AAD Sign-In",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Enforces that Azure AD sign-in sessions require multi-factor authentication and device compliance status checks before granting access to cloud resources.",
                Tags = ["azure-ad", "mfa", "conditional-access", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "AAD sign-in requires MFA + compliance; non-compliant devices cannot access cloud apps.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMFAForSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMFAForSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMFAForSignIn", 1)],
            },
            new TweakDef
            {
                Id = "aadca-disable-legacy-auth",
                Label = "Disable Legacy Authentication Protocols for AAD",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Blocks legacy authentication protocols (Basic Auth, NTLM over AAD) that cannot enforce MFA, preventing bypass of Conditional Access policies through legacy clients.",
                Tags = ["azure-ad", "legacy-auth", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Legacy auth blocked; older mail clients and apps using Basic Auth to AAD will fail.",
                ApplyOps = [RegOp.SetDword(Key, "BlockLegacyAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockLegacyAuthentication")],
                DetectOps = [RegOp.CheckDword(Key, "BlockLegacyAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "aadca-enforce-tap-policy",
                Label = "Enforce Temporary Access Pass Policy for AAD",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Enforces the Temporary Access Pass (TAP) policy for Azure AD, ensuring one-time codes for account recovery are time-limited and comply with organisational policy.",
                Tags = ["azure-ad", "tap", "recovery", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "TAP issuance and use governed by policy; emergency recovery codes follow compliance rules.",
                ApplyOps = [RegOp.SetDword(Key, "EnforceTemporaryAccessPassPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceTemporaryAccessPassPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceTemporaryAccessPassPolicy", 1)],
            },
            new TweakDef
            {
                Id = "aadca-block-personal-accounts",
                Label = "Block Personal Microsoft Accounts from AAD Sign-In",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Blocks personal (consumer) Microsoft accounts from being used for Azure AD sign-in on managed devices, preventing account mixing between personal and corporate identities.",
                Tags = ["azure-ad", "personal-accounts", "account-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "MSA (personal) accounts blocked for AAD scenarios on this device.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMicrosoftAccountSignIn", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMicrosoftAccountSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMicrosoftAccountSignIn", 0)],
            },
            new TweakDef
            {
                Id = "aadca-restrict-tenant-access",
                Label = "Restrict AAD Sign-In to Approved Tenants Only",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Restricts Azure AD authentication on this device to specific approved tenant IDs, preventing credential phishing attacks that redirect users to rogue AAD tenants.",
                Tags = ["azure-ad", "tenant-restriction", "phishing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Only approved AAD tenants accessible; sign-in to unknown tenants is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "RestrictAADSignInToApprovedTenants", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAADSignInToApprovedTenants")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAADSignInToApprovedTenants", 1)],
            },
            new TweakDef
            {
                Id = "aadca-block-workplace-join",
                Label = "Block Workplace Join for Non-Compliant Devices",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Blocks Azure AD Workplace Join (soft join) for devices that do not meet compliance requirements, preventing non-compliant devices from obtaining SSO tokens.",
                Tags = ["azure-ad", "workplace-join", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Non-compliant devices cannot Workplace Join; SSO access to AAD resources requires MDM enrolment.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockAADWorkplaceJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockAADWorkplaceJoin")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockAADWorkplaceJoin", 1)],
            },
            new TweakDef
            {
                Id = "aadca-disable-workplace-join-telemetry",
                Label = "Disable Workplace Join Telemetry",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Disables telemetry collection during and after Azure AD Workplace Join operations, preventing registration events and diagnostic data from being sent to Microsoft.",
                Tags = ["azure-ad", "workplace-join", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Workplace Join telemetry disabled; join process completes without sending diagnostic data.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableWorkplaceJoinTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableWorkplaceJoinTelemetry")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableWorkplaceJoinTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "aadca-disable-wj-cert-prompt",
                Label = "Disable Workplace Join Certificate Notification",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Disables the certificate-related notification prompts that appear during Workplace Join, preventing user interaction with certificate provisioning dialogs.",
                Tags = ["azure-ad", "workplace-join", "certificate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Certificate notifications during WJ suppressed; provisioning is fully silent.",
                ApplyOps = [RegOp.SetDword(Key2, "WorkplaceJoinCertNotificationEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key2, "WorkplaceJoinCertNotificationEnabled")],
                DetectOps = [RegOp.CheckDword(Key2, "WorkplaceJoinCertNotificationEnabled", 0)],
            },
            new TweakDef
            {
                Id = "aadca-block-guest-access",
                Label = "Block Guest Account AAD Sign-In",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Prevents guest and B2B collaboration accounts from signing into Azure AD resources on this device, restricting access to direct organisational members only.",
                Tags = ["azure-ad", "guest", "b2b", "access-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Guest/B2B accounts blocked; only licensed internal users can access AAD resources.",
                ApplyOps = [RegOp.SetDword(Key, "BlockGuestAADSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockGuestAADSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "BlockGuestAADSignIn", 1)],
            },
            new TweakDef
            {
                Id = "aadca-require-intune-compliance",
                Label = "Require Intune Compliance for AAD Token Issuance",
                Category = "Azure AD Conditional Access Policy",
                Description =
                    "Requires that the device passes Microsoft Intune compliance policy checks before AAD issues access tokens, blocking cloud resource access from unmanaged or non-compliant devices.",
                Tags = ["azure-ad", "intune", "compliance", "conditional-access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Intune compliance required for token issuance; non-enrolled devices lose cloud app access.",
                ApplyOps = [RegOp.SetDword(Key, "RequireIntuneCompliance", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIntuneCompliance")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIntuneCompliance", 1)],
            },
        ];
}
