// RegiLattice.Core — Tweaks/EdgeWorkProfilePolicy.cs
// Microsoft Edge work profile and identity management Group Policy controls (Sprint 612).
// Category: "Edge Work Profile Policy" | Slug: edgewp
// Key: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeWorkProfilePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "edgewp-force-work-profile-sign-in",
            Label = "Edge Work Profile: Force Sign-In with Work or School Account",
            Category = "Edge Work Profile Policy",
            Description = "Sets ForceSyncTypes=1 in Edge policy. Forces Edge to require the user to sign in with a work or school account (Entra ID / Microsoft 365) before browsing can begin, ensuring the browser session is always associated with a managed identity and data protection policies are applied. " +
                "Unauthenticated Edge browsing sessions (guest mode, personal profile, no-sign-in mode) do not inherit the user's Conditional Access, DLP, or browser policy configurations. A user who bypasses the sign-in prompt has a browser session without enterprise policies applied, including without SSL inspection, information barrier enforcement, or Purview data classification labels. Forcing sign-in ensures all browser activity is attributable to a managed identity.",
            Tags = ["edge", "profile", "sign-in", "work-account", "identity"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Edge requires work account sign-in before browsing; unauthenticated sessions not permitted.",
            ApplyOps = [RegOp.SetDword(Key, "ForceSyncTypes", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceSyncTypes")],
            DetectOps = [RegOp.CheckDword(Key, "ForceSyncTypes", 1)],
        },
        new TweakDef
        {
            Id = "edgewp-disable-personal-browsing-in-work-profile",
            Label = "Edge Work Profile: Disable Switching to Personal Browse Session in Work Profile",
            Category = "Edge Work Profile Policy",
            Description = "Sets PersonalBrowsingAllowed=0 in Edge policy. Prevents users from switching from the managed work profile to an unmanaged personal browsing context within the same Edge browser window, keeping all browsing within the enforceable work profile context. " +
                "Edge's 'Browse without your data' and 'Personal profile' features allow users to open an unmanaged browser context without enterprise DLP, SSL inspection, and proxy policies. These personal contexts, despite running in the same process, do not inherit the work profile's Conditional Access tokens or data protection rules. A user can copy sensitive data from a managed tab to an unmanaged personal tab, then upload it to a personal cloud service.",
            Tags = ["edge", "profile", "personal", "dlp", "data-protection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Personal browsing contexts disabled in Edge; all sessions remain in the managed work profile.",
            ApplyOps = [RegOp.SetDword(Key, "PersonalBrowsingAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PersonalBrowsingAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "PersonalBrowsingAllowed", 0)],
        },
        new TweakDef
        {
            Id = "edgewp-disable-work-profile-data-sync",
            Label = "Edge Work Profile: Disable Work Profile Data Sync to Microsoft Account",
            Category = "Edge Work Profile Policy",
            Description = "Sets SyncDisabled=1 in Edge policy. Prevents the Edge work profile from syncing browsing history, saved passwords, extensions, and settings to the user's Microsoft cloud account (personal or enterprise), keeping work profile data resident on the managed device only. " +
                "Edge sync uploads work browsing history, internal URL patterns, saved credentials for internal web apps, and installed extension lists to Microsoft's cloud sync service. In high-sensitivity environments, the browsing history itself may constitute proprietary information. Disabling sync ensures that work profile data stays on the managed device and any downstream device the user signs into using the same account cannot access the work browser data.",
            Tags = ["edge", "sync", "cloud", "data-residency", "profile"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Edge work profile data not synced to Microsoft cloud; browsing history and credentials remain local.",
            ApplyOps = [RegOp.SetDword(Key, "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(Key, "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "edgewp-block-non-work-account-sign-in",
            Label = "Edge Work Profile: Block Signing In with Non-Work / Personal Accounts",
            Category = "Edge Work Profile Policy",
            Description = "Sets RestrictSigninToPattern=1 in Edge policy. Restricts the Edge browser sign-in to accounts that match the organisation's verified domain pattern only, preventing users from signing into Edge with personal Microsoft accounts, Google accounts (via linking), or third-party identity providers. " +
                "Allowing personal account sign-in into Edge bypasses Conditional Access evaluation because the personal account token does not flow through the organisation's IdP. A user signed in with a personal account in a work browser context can install unvetted extensions, sync personal bookmarks containing personal cloud credentials, and access personal services without DLP policy enforcement.",
            Tags = ["edge", "profile", "account-restriction", "conditional-access", "idp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Edge sign-in restricted to work domain accounts; personal Microsoft accounts blocked.",
            ApplyOps = [RegOp.SetDword(Key, "RestrictSigninToPattern", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSigninToPattern")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSigninToPattern", 1)],
        },
        new TweakDef
        {
            Id = "edgewp-enable-mandatory-workplace-access",
            Label = "Edge Work Profile: Enable Mandatory Workplace (MAM) Access Enforcement in Edge",
            Category = "Edge Work Profile Policy",
            Description = "Sets MandatoryBrowserWorkplaceAccess=1 in Edge policy. Enables Microsoft Edge's Mobile Application Management (MAM)-style policy enforcement when the user is authenticated with a work account, automatically applying Intune app protection policies to the Edge browser session. " +
                "Without MAM/workplace access enforcement, a user on a non-compliant device (e.g., a personal device not enrolled in Intune) can sign into Edge with a work account and access M365 resources without device compliance checks, DLP policies applying to copy/paste, or data transfer restrictions. Workplace access enforcement applies app-level protection policies to the browser session independent of device enrollment state.",
            Tags = ["edge", "mam", "intune", "app-protection", "conditional-access"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Intune MAM app protection policies applied to Edge; data transfer restrictions active even on non-enrolled devices.",
            ApplyOps = [RegOp.SetDword(Key, "MandatoryBrowserWorkplaceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "MandatoryBrowserWorkplaceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "MandatoryBrowserWorkplaceAccess", 1)],
        },
        new TweakDef
        {
            Id = "edgewp-disable-guest-mode-browsing",
            Label = "Edge Work Profile: Disable Guest Mode Browsing",
            Category = "Edge Work Profile Policy",
            Description = "Sets GuestModeEnabled=0 in Edge policy. Disables the Edge 'Guest' profile mode, which launches an ephemeral browser context with no policies, no identity, no browsing history persistence, and no enterprise configuration applied. " +
                "Guest mode is commonly used to access the browser without any profile's policies applying. On a managed enterprise device, a user opening Edge in guest mode bypasses every configured Edge Group Policy — with no managed account bound to the session. SSL inspection, extension allow-listing, DLP policies, and URL filtering policies all become ineffective in a guest mode session.",
            Tags = ["edge", "guest-mode", "policy-bypass", "managed-browser"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Edge Guest mode disabled; no ephemeral unmanaged browser sessions available.",
            ApplyOps = [RegOp.SetDword(Key, "GuestModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "GuestModeEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "GuestModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgewp-disable-inprivate-browsing",
            Label = "Edge Work Profile: Disable InPrivate Browsing Mode",
            Category = "Edge Work Profile Policy",
            Description = "Sets InPrivateModeAvailability=1 in Edge policy. Disables InPrivate browsing mode (value 1 = Disabled) in Edge, preventing users from opening sessions where browsing history and cookies are not retained. " +
                "InPrivate mode, while legitimate for privacy, prevents the browsing session from being recorded in Edge history and from being subject to certain telemetry and audit policies that depend on session context. In regulated industries where all web access must be logged for compliance, InPrivate sessions create unlogged access points. Additionally, some DLP inspection products are not applied to InPrivate sessions by default.",
            Tags = ["edge", "inprivate", "logging", "compliance", "audit"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "InPrivate mode disabled; all Edge sessions are history-tracked and compliance-logged.",
            ApplyOps = [RegOp.SetDword(Key, "InPrivateModeAvailability", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "InPrivateModeAvailability")],
            DetectOps = [RegOp.CheckDword(Key, "InPrivateModeAvailability", 1)],
        },
        new TweakDef
        {
            Id = "edgewp-block-edge-profile-discovery",
            Label = "Edge Work Profile: Block Automatic Discovery and Sign-In of New Work Profiles",
            Category = "Edge Work Profile Policy",
            Description = "Sets EdgeWorkspacesEnabled=0 in Edge policy. Disables the Edge profile discovery feature that automatically prompts users to create new work profiles when it detects a new Microsoft Entra ID sign-in event, preventing uncontrolled proliferation of managed profiles on shared or shared-use devices. " +
                "On devices shared among multiple users (shift workers, kiosk-style workstations), automatic profile creation means each user who signs into a Microsoft service triggers Edge to create a new managed profile bound to their work account. On machines not managed for roaming profiles, these additional profiles accumulate with cached policy configurations, create additional disk usage, and cause confusing multi-profile scenarios.",
            Tags = ["edge", "profile-discovery", "shared-device", "profile-management"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Auto-profile discovery disabled; Edge does not offer to create new work profiles on detected sign-in.",
            ApplyOps = [RegOp.SetDword(Key, "EdgeWorkspacesEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EdgeWorkspacesEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "EdgeWorkspacesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgewp-enforce-managed-profile-on-startup",
            Label = "Edge Work Profile: Enforce Managed Work Profile at Browser Startup",
            Category = "Edge Work Profile Policy",
            Description = "Sets ManagedBrowserStartEnabled=1 in Edge policy. Ensures that Edge always opens to the managed work profile on startup, rather than offering a profile picker or defaulting to the most recently used (possibly personal) profile. " +
                "On multi-profile Edge installations, the browser may start in a personal or unmanaged profile if that was the last one used. Users habitually working in a personal profile while corporate data is accessible in internal apps may inadvertently paste or upload work data from a personal-profile session where DLP is not enforced. Ensuring startup always opens the managed work profile makes the default context the compliant one.",
            Tags = ["edge", "startup", "managed-profile", "default", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Edge always opens managed work profile on startup; personal profile not the default context.",
            ApplyOps = [RegOp.SetDword(Key, "ManagedBrowserStartEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ManagedBrowserStartEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "ManagedBrowserStartEnabled", 1)],
        },
        new TweakDef
        {
            Id = "edgewp-disable-profile-sharing-across-windows",
            Label = "Edge Work Profile: Disable Profile Data Sharing Across Edge Windows",
            Category = "Edge Work Profile Policy",
            Description = "Sets ShareSessionCookiesWithExternalApps=0 in Edge policy. Prevents Edge from sharing session state, cookies, and profile context with Edge WebView2-embedded browser controls in third-party applications, ensuring the managed work profile's authentication tokens are isolated to the Edge browser process. " +
                "Edge WebView2-based applications (such as Teams, Office web views, and third-party Electron apps) can access the Edge user profile's cookie jar and session tokens if profile sharing is enabled. A compromised Electron application running with Edge WebView2 can silently extract the authenticated session tokens for SharePoint, Exchange, and other M365 services from the shared Edge profile, enabling token theft without the user's knowledge.",
            Tags = ["edge", "webview2", "session-sharing", "token-theft", "cookie"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Edge profile session cookies not shared with WebView2 apps; M365 tokens isolated to Edge browser process.",
            ApplyOps = [RegOp.SetDword(Key, "ShareSessionCookiesWithExternalApps", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ShareSessionCookiesWithExternalApps")],
            DetectOps = [RegOp.CheckDword(Key, "ShareSessionCookiesWithExternalApps", 0)],
        },
    ];
}
