// RegiLattice.Core — Tweaks/AzureAdTenantPolicy.cs
// Azure Active Directory Tenant Restriction Group Policy — Sprint 193.
// Controls Azure AD join, email sign-in with personal accounts, tenant
// restrictions, guest account access, and consumer app enrollment via GPO.
// Category: "Azure AD Policy" | Slug: aadtenant
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\AzureADAccount

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AzureAdTenantPolicy
{
    private const string AadKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AzureADAccount";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aadtenant-block-email-signin",
                Label = "Block Azure AD Email (MSA) Sign-In",
                Category = "Azure AD Policy",
                Description =
                    "Sets AllowEmailSignIn=0 to prevent users from signing in with personal Microsoft accounts or email credentials instead of their enterprise Azure AD identity. Enforces corporate identity exclusivity.",
                Tags = ["aad", "signin", "msa", "policy", "identity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks personal MSA/email sign-in; corporate Azure AD credentials unaffected.",
                ApplyOps = [RegOp.SetDword(AadKey, "AllowEmailSignIn", 0)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "AllowEmailSignIn")],
                DetectOps = [RegOp.CheckDword(AadKey, "AllowEmailSignIn", 0)],
            },
            new TweakDef
            {
                Id = "aadtenant-block-non-enterprise-join",
                Label = "Block Non-Enterprise Azure AD Device Join",
                Category = "Azure AD Policy",
                Description =
                    "Sets BlockNonEnterpriseAADUserFromJoining=1. Prevents consumer or personal Azure AD accounts from joining this device, limiting device registration exclusively to managed enterprise tenants.",
                Tags = ["aad", "join", "enterprise", "policy", "device"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks BYOD personal AAD joins; enterprise-managed joins proceed normally.",
                ApplyOps = [RegOp.SetDword(AadKey, "BlockNonEnterpriseAADUserFromJoining", 1)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "BlockNonEnterpriseAADUserFromJoining")],
                DetectOps = [RegOp.CheckDword(AadKey, "BlockNonEnterpriseAADUserFromJoining", 1)],
            },
            new TweakDef
            {
                Id = "aadtenant-disable-consumer-apps",
                Label = "Disable Consumer Azure AD App Enrollment",
                Category = "Azure AD Policy",
                Description =
                    "Sets AllowWindowsConsumerApps=0. Prevents consumer-oriented Azure AD application enrollment on this device, blocking personal-use apps from registering with the user's Microsoft account identity.",
                Tags = ["aad", "consumer", "apps", "policy", "enrollment"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks consumer app AAD enrollment; enterprise LOB app registration unaffected.",
                ApplyOps = [RegOp.SetDword(AadKey, "AllowWindowsConsumerApps", 0)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "AllowWindowsConsumerApps")],
                DetectOps = [RegOp.CheckDword(AadKey, "AllowWindowsConsumerApps", 0)],
            },
            new TweakDef
            {
                Id = "aadtenant-enforce-tenant-restrictions",
                Label = "Enforce Azure AD Tenant Restrictions",
                Category = "Azure AD Policy",
                Description =
                    "Sets EnableTenantRestrictions=1. Activates tenant restriction policy, limiting which Azure AD tenants users can authenticate against. Works with network-level tenant restriction headers to block unauthorised tenant sign-ins.",
                Tags = ["aad", "tenant", "restrictions", "policy", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "May block access to partner tenants not on the allowed list; coordinate with IT before applying.",
                ApplyOps = [RegOp.SetDword(AadKey, "EnableTenantRestrictions", 1)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "EnableTenantRestrictions")],
                DetectOps = [RegOp.CheckDword(AadKey, "EnableTenantRestrictions", 1)],
            },
            new TweakDef
            {
                Id = "aadtenant-block-guest-accounts",
                Label = "Block Azure AD Guest Account Sign-In",
                Category = "Azure AD Policy",
                Description =
                    "Sets AllowGuestAccounts=0. Prevents guest or B2B invited user accounts from signing into this device, restricting access to member accounts belonging to the managed enterprise tenant only.",
                Tags = ["aad", "guest", "b2b", "policy", "access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks guest/B2B sign-in to Windows; no impact on existing enterprise member accounts.",
                ApplyOps = [RegOp.SetDword(AadKey, "AllowGuestAccounts", 0)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "AllowGuestAccounts")],
                DetectOps = [RegOp.CheckDword(AadKey, "AllowGuestAccounts", 0)],
            },
            new TweakDef
            {
                Id = "aadtenant-block-personal-accounts",
                Label = "Block Personal Microsoft Account Links",
                Category = "Azure AD Policy",
                Description =
                    "Sets BlockPersonalMicrosoftAccounts=1. Prevents users from adding or connecting personal Microsoft accounts to this device, ensuring only work/school accounts are provisioned.",
                Tags = ["aad", "personal", "microsoft", "account", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks personal MSA account linking; existing enterprise account unaffected.",
                ApplyOps = [RegOp.SetDword(AadKey, "BlockPersonalMicrosoftAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "BlockPersonalMicrosoftAccounts")],
                DetectOps = [RegOp.CheckDword(AadKey, "BlockPersonalMicrosoftAccounts", 1)],
            },
            new TweakDef
            {
                Id = "aadtenant-require-privacy-consent",
                Label = "Require Azure AD Privacy Consent",
                Category = "Azure AD Policy",
                Description =
                    "Sets RequirePrivacyConsent=1. Forces display of the enterprise privacy consent dialog before Azure AD account setup, ensuring users acknowledge the organisation's data handling policy.",
                Tags = ["aad", "privacy", "consent", "policy", "gdpr"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Adds a consent dialog to AAD provisioning flow; no impact on authenticated sessions.",
                ApplyOps = [RegOp.SetDword(AadKey, "RequirePrivacyConsent", 1)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "RequirePrivacyConsent")],
                DetectOps = [RegOp.CheckDword(AadKey, "RequirePrivacyConsent", 1)],
            },
            new TweakDef
            {
                Id = "aadtenant-disable-shared-device-signin",
                Label = "Disable Shared Azure AD Device Sign-In Mode",
                Category = "Azure AD Policy",
                Description =
                    "Sets AllowSharedLocalAppData=0. Prevents Azure AD shared device mode from allowing local app data to persist between users, protecting data isolation on shared Windows kiosks and shared endpoints.",
                Tags = ["aad", "shared", "kiosk", "policy", "isolation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enforces per-user app data isolation on shared AAD devices; no impact on dedicated devices.",
                ApplyOps = [RegOp.SetDword(AadKey, "AllowSharedLocalAppData", 0)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "AllowSharedLocalAppData")],
                DetectOps = [RegOp.CheckDword(AadKey, "AllowSharedLocalAppData", 0)],
            },
            new TweakDef
            {
                Id = "aadtenant-block-home-edition-join",
                Label = "Block AAD Join on Windows Home Editions",
                Category = "Azure AD Policy",
                Description =
                    "Sets AllowAADPasswordReset=0. Disables self-service password reset sign-in from the Windows lock screen for Azure AD accounts, preventing unauthenticated SSPR attempts that could expose reset links.",
                Tags = ["aad", "password", "reset", "policy", "lockscreen"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes SSPR link from lock screen; users can still reset via browser or IT helpdesk.",
                ApplyOps = [RegOp.SetDword(AadKey, "AllowAADPasswordReset", 0)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "AllowAADPasswordReset")],
                DetectOps = [RegOp.CheckDword(AadKey, "AllowAADPasswordReset", 0)],
            },
            new TweakDef
            {
                Id = "aadtenant-disable-cloud-clipboard-aad",
                Label = "Disable Azure AD Cloud Clipboard Sync",
                Category = "Azure AD Policy",
                Description =
                    "Sets AllowCrossDeviceClipboard=0 in the Azure AD account policy scope. Prevents clipboard history from syncing to other Azure AD-joined or registered devices via the Microsoft cloud clipboard service.",
                Tags = ["aad", "clipboard", "sync", "policy", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables cross-device clipboard roaming for AAD users; local clipboard history unaffected.",
                ApplyOps = [RegOp.SetDword(AadKey, "AllowCrossDeviceClipboard", 0)],
                RemoveOps = [RegOp.DeleteValue(AadKey, "AllowCrossDeviceClipboard")],
                DetectOps = [RegOp.CheckDword(AadKey, "AllowCrossDeviceClipboard", 0)],
            },
        ];
}
