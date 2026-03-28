namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class EdgeProfileSignInPolicy
{
    private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edgeprof-lockdown-browser-signin",
            Label = "Edge Profile & Sign-In Policy: Lock Browser Sign-In to Managed Accounts",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Restricts browser sign-in so users can only sign in with accounts that are managed by the organisation. Setting BrowserSignInLockdownEnabled to 1 prevents users from signing into Edge with personal Microsoft accounts or any account outside the tenant's allowed domain list. This ensures all browser-level sync, password manager, and history data flows through managed cloud storage subject to compliance policy rather than personal cloud accounts.",
            Tags = ["edge", "sign in", "managed accounts", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "BrowserSignInLockdownEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "BrowserSignInLockdownEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "BrowserSignInLockdownEnabled", 1)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Only Azure AD / managed accounts can sign into Edge; personal accounts are rejected.",
        },
        new TweakDef
        {
            Id = "edgeprof-disable-implicit-signin",
            Label = "Edge Profile & Sign-In Policy: Disable Implicit OS-Level Sign-In",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Prevents Microsoft Edge from automatically signing the user into the browser using the Windows OS account credentials. When ImplicitSignInEnabled is 0, Edge will not transparently authenticate against Azure AD using the Windows token obtained at OS login. In shared-device or service-account scenarios, implicit sign-in can silently associate browser sessions with accounts that should not be tied to a browser profile, exposing unexpected sync or cloud data.",
            Tags = ["edge", "sign in", "os credentials", "implicit sign in", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ImplicitSignInEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ImplicitSignInEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ImplicitSignInEnabled", 0)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Edge does not auto-sign users in via Windows OS credentials; explicit sign in required.",
        },
        new TweakDef
        {
            Id = "edgeprof-disable-guided-switch",
            Label = "Edge Profile & Sign-In Policy: Disable Guided Profile Switching Prompts",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Suppresses the prompt that Edge displays to guide users to switch to a different profile when they navigate to a linked account resource. When GuidedSwitchEnabled is 0, Edge will not interrupt the browsing session with suggestions to switch profiles based on account link heuristics. In managed environments where all browsing should occur within a single enterprise profile, these prompts are disruptive and may encourage users to create or log into personal profiles.",
            Tags = ["edge", "profile", "profile switch", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "GuidedSwitchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "GuidedSwitchEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "GuidedSwitchEnabled", 0)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Profile-switch recommendation prompts are hidden; users browse in their current profile without interruption.",
        },
        new TweakDef
        {
            Id = "edgeprof-enable-profile-separation",
            Label = "Edge Profile & Sign-In Policy: Enable Work and Personal Profile Separation",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Enforces logical separation between work and personal browsing profiles in Microsoft Edge. When ProfileSeparationEnabled is 1, Edge maintains distinct cookie jars, credential stores, and sync data between the managed work profile and any personal profiles, preventing data leakage from work sessions into personal storage. This is especially relevant in bring-your-own-device (BYOD) deployments where the same Edge installation must serve both corporate and personal surfing.",
            Tags = ["edge", "profile separation", "work profile", "personal profile", "byod", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ProfileSeparationEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ProfileSeparationEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ProfileSeparationEnabled", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Work and personal Edge profiles are isolated; cookies and credentials do not cross profile boundaries.",
        },
        new TweakDef
        {
            Id = "edgeprof-enable-azure-sso",
            Label = "Edge Profile & Sign-In Policy: Enable Azure AD Single Sign-On in Edge",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Enables Azure Active Directory Single Sign-On for Microsoft Edge so users can access Azure-protected web applications without re-entering their credentials. Setting AzureADSSOEnabled to 1 allows Edge to use the Kerberos/NTLM tokens obtained at Windows login to satisfy Azure AD authentication seamlessly. This reduces credential fatigue, eliminates phishable password prompts for internal apps, and centralises Azure sign-in under the corporate identity provider.",
            Tags = ["edge", "azure ad", "sso", "single sign on", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "AzureADSSOEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "AzureADSSOEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "AzureADSSOEnabled", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Edge uses Windows AD token for Azure AD SSO; users are not re-prompted for Azure sign-in on internal apps.",
        },
        new TweakDef
        {
            Id = "edgeprof-disable-floating-workspace",
            Label = "Edge Profile & Sign-In Policy: Disable Floating Workspace Tab Sync",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Disables the Floating Workspace feature in Microsoft Edge which synchronises open tabs and browser state across devices when a user moves from one machine to another. Setting FloatingWorkspaceEnabled to 0 prevents tab-session data from being transmitted to Microsoft cloud sync services when the workspace floats between devices. In secure or air-gapped environments, floating tab data can expose confidential URLs, internal resource paths, and session tokens via cloud synchronisation.",
            Tags = ["edge", "floating workspace", "tab sync", "cloud sync", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "FloatingWorkspaceEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "FloatingWorkspaceEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "FloatingWorkspaceEnabled", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Tab state is not synced via Floating Workspace; sessions remain local to each device.",
        },
        new TweakDef
        {
            Id = "edgeprof-keep-data-on-new-enterprise-profile",
            Label = "Edge Profile & Sign-In Policy: Retain Browsing Data When Creating Enterprise Profile",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Configures Edge to keep local browsing data (history, passwords, bookmarks) when the user is prompted to create a new enterprise profile from an existing personal profile. Setting EnterpriseProfileCreationKeepBrowsingData to 1 means that on profile creation the user's existing local data is preserved in the new enterprise profile rather than being wiped. This prevents accidental data loss during policy rollout when enterprise sign-in is first enforced.",
            Tags = ["edge", "profile", "data retention", "enterprise profile", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "EnterpriseProfileCreationKeepBrowsingData", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnterpriseProfileCreationKeepBrowsingData")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "EnterpriseProfileCreationKeepBrowsingData", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Browsing data is preserved when converting to an enterprise profile; no data loss on policy rollout.",
        },
        new TweakDef
        {
            Id = "edgeprof-allow-all-domain-machines",
            Label = "Edge Profile & Sign-In Policy: Allow Sign-In on Non-Domain-Joined Machines",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Prevents the OnlyOnPremDomainJoinedMachinesAllowed restriction from limiting Edge sign-in to on-premise domain-joined devices only. Setting OnlyOnPremDomainJoinedMachinesAllowed to 0 (the default) means Azure AD-joined, MDM-managed, and non-domain devices can also use enterprise sign-in. This is the correct value for modern hybrid Azure AD and Intune environments where the entire device fleet may not be on-prem domain-joined.",
            Tags = ["edge", "sign in", "domain join", "azure ad", "intune", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "OnlyOnPremDomainJoinedMachinesAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "OnlyOnPremDomainJoinedMachinesAllowed")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "OnlyOnPremDomainJoinedMachinesAllowed", 0)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Sign-in is not restricted to on-prem domain-joined machines; Azure AD-joined and MDM devices are permitted.",
        },
        new TweakDef
        {
            Id = "edgeprof-hide-acrobat-subscription-button",
            Label = "Edge Profile & Sign-In Policy: Hide Adobe Acrobat Subscription Upsell Button",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Removes the Adobe Acrobat subscription promotion button that Microsoft Edge injects into the PDF toolbar when a PDF is rendered in the built-in viewer. Setting ShowAcrobatSubscriptionButton to 0 suppresses this commercial upsell element from the Edge UI. In enterprise environments where Adobe licensing is managed centrally, this advertising button causes confusion and may lead users to attempt unauthorised software purchases.",
            Tags = ["edge", "adobe acrobat", "pdf", "upsell", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ShowAcrobatSubscriptionButton", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShowAcrobatSubscriptionButton")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ShowAcrobatSubscriptionButton", 0)],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Adobe Acrobat subscription button is hidden from Edge PDF toolbar; no commercial prompts in PDF viewer.",
        },
        new TweakDef
        {
            Id = "edgeprof-disable-inprivate",
            Label = "Edge Profile & Sign-In Policy: Disable InPrivate (Private Browsing) Mode",
            Category = "Edge Profile & Sign-In Policy",
            Description =
                "Prevents users from opening InPrivate browsing windows in Microsoft Edge. InPrivate mode does not save history, cookies, or form data locally but it does bypass some content-filtering and monitoring tools that operate on profile data rather than network traffic. Setting InPrivateModeAvailability to 1 disables InPrivate so all browsing occurs in the user's managed profile, ensuring audit log completeness and consistent policy enforcement across all sessions.",
            Tags = ["edge", "inprivate", "private browsing", "compliance", "audit", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "InPrivateModeAvailability", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "InPrivateModeAvailability")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "InPrivateModeAvailability", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "InPrivate windows cannot be opened; all Edge sessions are in the managed profile and subject to audit.",
        },
    ];
}
