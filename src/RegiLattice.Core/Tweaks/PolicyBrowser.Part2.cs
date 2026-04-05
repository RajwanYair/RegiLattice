namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyBrowser
{
    // ── EdgeProfileSignInPolicy ──
    private static class _EdgeProfileSignInPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgeprof-lockdown-browser-signin",
                Label = "Edge Profile & Sign-In Policy: Lock Browser Sign-In to Managed Accounts",
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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
                Category = "Browser — Edge New Tab Page",
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

    // ── EdgeSearchAddressBarPolicy ──
    private static class _EdgeSearchAddressBarPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgesrch-disable-bing-address-bar-provider",
                Label = "Edge Search & Address Bar Policy: Remove Microsoft Search in Bing from Address Bar",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Removes the Microsoft Search in Bing suggestion provider from the Microsoft Edge address bar. When enabled, this provider queries the Microsoft Search enterprise index and Bing whenever the user types in the address bar, even for single words or partial terms. In environments where all internet search is routed through an approved proxy or where Bing query telemetry is unwanted, removing this provider reduces unsolicited outbound traffic.",
                Tags = ["edge", "search", "bing", "address bar", "microsoft search", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AddressBarMicrosoftSearchInBingProviderEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AddressBarMicrosoftSearchInBingProviderEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AddressBarMicrosoftSearchInBingProviderEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Bing suggestion provider from address bar; reduces background search queries to Microsoft.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-local-providers",
                Label = "Edge Search & Address Bar Policy: Disable Local and Intranet Search Suggestions",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables local suggestion providers in the Microsoft Edge address bar, including previously visited intranet URLs, bookmarks from the managed profile, and file:// path completions. On shared workstations, browser kiosk sessions, and temporary accounts, address bar history exposure (even of intranet URLs) could reveal which internal systems and resources the previous user visited.",
                Tags = ["edge", "search", "local providers", "intranet", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "LocalProvidersEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "LocalProvidersEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "LocalProvidersEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Intranet URL completions and local history suggestions no longer appear in the Edge address bar.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-dns-interception-check",
                Label = "Edge Search & Address Bar Policy: Disable DNS Interception Detection",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the DNS interception detection feature in Microsoft Edge. When enabled, Edge periodically sends probe DNS requests to non-existent hostnames and checks whether the DNS resolver returns NXDOMAIN (expected) or a real IP address (indicating intercepting DNS). In enterprise environments with transparent DNS proxies, split-horizon DNS, or captive portal infrastructure, this probe generates false positives and triggers browser warnings that confuse users.",
                Tags = ["edge", "dns", "interception", "detection", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DNSInterceptionChecksEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DNSInterceptionChecksEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DNSInterceptionChecksEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "DNS interception probe requests are suppressed; no false-positive warnings from split-horizon DNS environments.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-error-page-web-service",
                Label = "Edge Search & Address Bar Policy: Disable Web Service for Navigation Error Pages",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from using a Microsoft-hosted web service to generate alternative navigation error pages when a site is unreachable. When this policy is enabled, Edge sends the unreachable URL to Microsoft's servers to retrieve a custom error page with suggestions. Disabling it keeps Edge using its built-in static error page and prevents the URL of failed navigation attempts from being submitted to Microsoft.",
                Tags = ["edge", "error page", "navigation", "web service", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ResolveNavigationErrorsUseWebService", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ResolveNavigationErrorsUseWebService")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ResolveNavigationErrorsUseWebService", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Failed navigation attempts show Edge's built-in error page; failed URLs are not sent to Microsoft.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-alternate-error-pages",
                Label = "Edge Search & Address Bar Policy: Disable Alternate Error Page Web Service",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the alternate error page feature in Microsoft Edge, which contacts a Microsoft web service to display rich error pages with suggestions, links, and diagnostics for unreachable URLs. The alternate error page service sends the unreachable hostname to Microsoft even if the hostname is a private intranet address. Disabling it keeps failed navigations private and uses only Edge's built-in static error content.",
                Tags = ["edge", "error page", "alternate", "web service", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AlternateErrorPagesEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AlternateErrorPagesEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AlternateErrorPagesEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "No Microsoft-hosted error page content; Edge shows its own static error page for unreachable sites.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-cloud-related-matches",
                Label = "Edge Search & Address Bar Policy: Disable Cloud-Based Related Matches in Address Bar",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the cloud-based Related Matches provider in the Microsoft Edge address bar. Related Matches is a Microsoft service that offers AI-enhanced cross-domain URL completions based on browsing behavior, intent signals, and trending content signals from the Bing cloud index. Unlike local completions, Related Matches sends real-time query data to Microsoft cloud even for partial input.",
                Tags = ["edge", "search", "cloud", "related matches", "ai", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "RelatedMatchesCloudServiceEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "RelatedMatchesCloudServiceEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "RelatedMatchesCloudServiceEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Related Matches AI cloud provider disabled; address bar suggestions come only from local history and bookmarks.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-sidebar-search",
                Label = "Edge Search & Address Bar Policy: Disable Search in Sidebar Panel",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the Edge Search in Sidebar feature, which opens a search panel on the right side of the browser when the user right-clicks and selects 'Search using web side panel', or when the user selects text and triggers a sidebar search action. This feature creates a split-screen search experience that shares the selected text with Microsoft's search service.",
                Tags = ["edge", "sidebar", "search", "side panel", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SearchInSidebarEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SearchInSidebarEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SearchInSidebarEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Right-click 'Search web side panel' option removed; selected text no longer sent to Edge sidebar search.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-typosquatting-checker",
                Label = "Edge Search & Address Bar Policy: Disable URL Typosquatting Checker",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the Edge typosquatting checker that compares navigated URLs against a list of commonly mistyped domain names to warn the user when they may have typed a typosquatting site. While the checker improves user safety, it performs cloud lookups for URLs navigated via the address bar and sends the navigated hostname to Microsoft for comparison, creating outbound communication for every navigation.",
                Tags = ["edge", "typosquatting", "url", "safety", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "TyposquattingCheckerEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "TyposquattingCheckerEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "TyposquattingCheckerEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Typosquatting URL warnings disabled; users will not be warned about similar-looking domains.",
            },
        ];
    }

    // ── EdgeSecureBrowsingPolicy ──
    private static class _EdgeSecureBrowsingPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgesec-enable-revocation-checks",
                Label = "Edge Secure Browsing Policy: Enable Online Certificate Revocation Checks",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Forces Microsoft Edge to perform online certificate revocation checks (OCSP and CRL) for every TLS connection. By default Edge uses a soft-fail model where revocation checks are skipped if the responder is unreachable. Setting EnableOnlineRevocationChecks to 1 switches to hard-fail revocation checking, so Edge refuses connections when the revocation status of a server certificate cannot be confirmed. This prevents browser connections to hosts presenting revoked certificates caused by key compromise or CA incident.",
                Tags = ["edge", "tls", "certificate", "revocation", "ocsp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EnableOnlineRevocationChecks", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnableOnlineRevocationChecks")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EnableOnlineRevocationChecks", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Edge performs hard-fail CRL/OCSP checks; connections with revoked certificates are blocked.",
            },
            new TweakDef
            {
                Id = "edgesec-revocation-for-local-anchors",
                Label = "Edge Secure Browsing Policy: Require Revocation Checks for Locally-Trusted Certificates",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Extends online certificate revocation checking to certificates issued by locally-trusted (enterprise) Certificate Authorities. Without this policy Edge skips revocation checks for certs signed by CAs in the local machine trust store. Setting RequireOnlineRevocationChecksForLocalAnchors to 1 is essential in enterprise environments where internal PKI is used, as a compromised internal CA should still be subject to revocation enforcement.",
                Tags = ["edge", "pki", "certificate", "revocation", "enterprise ca", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "RequireOnlineRevocationChecksForLocalAnchors", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "RequireOnlineRevocationChecksForLocalAnchors")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "RequireOnlineRevocationChecksForLocalAnchors", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Revocation is also checked for enterprise CA-signed certs; revoked internal certs are blocked.",
            },
            new TweakDef
            {
                Id = "edgesec-autoupgrade-mixed-content",
                Label = "Edge Secure Browsing Policy: Auto-Upgrade Mixed Content to HTTPS",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Configures Microsoft Edge to automatically upgrade mixed HTTP sub-resources (images, audio, video) to HTTPS without user intervention. Mixed content occurs when an HTTPS page loads resources over plain HTTP. Without MixedContentAutoupgradeEnabled, passive mixed content is displayed with a warning. Setting this to 1 makes Edge silently retry the resource over HTTPS, eliminating the mixed-content downgrade attack surface and the confusing browser security warning.",
                Tags = ["edge", "mixed content", "https", "upgrade", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "MixedContentAutoupgradeEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "MixedContentAutoupgradeEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "MixedContentAutoupgradeEnabled", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "HTTP sub-resources on HTTPS pages are silently upgraded to HTTPS; broken images only if server has no HTTPS.",
            },
            new TweakDef
            {
                Id = "edgesec-enable-https-upgrades",
                Label = "Edge Secure Browsing Policy: Enable Automatic HTTP-to-HTTPS Navigation Upgrades",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Enables the Edge HTTP URL upgrader, which rewrites HTTP navigation URLs to HTTPS before the request is made. HttpsUpgradesEnabled instructs Edge to speculatively upgrade HTTP URLs to HTTPS. If the HTTPS version is unavailable, Edge falls back to HTTP. This provides opportunistic HTTPS for sites that support it without requiring HSTS headers or HTTPS-only mode and eliminates cleartext first-hops for navigations to HTTPS-capable sites.",
                Tags = ["edge", "https", "http upgrade", "navigation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "HttpsUpgradesEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "HttpsUpgradesEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "HttpsUpgradesEnabled", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "HTTP navigations that have HTTPS available are automatically promoted; minimal risk of fallback.",
            },
            new TweakDef
            {
                Id = "edgesec-block-private-network-requests",
                Label = "Edge Secure Browsing Policy: Block Cross-Origin Requests to Private Network Resources",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents public websites from issuing fetch/XHR requests to resources on the local network or loopback addresses (private IP ranges). Setting InsecurePrivateNetworkRequestsAllowed to 0 enforces the Private Network Access specification. Without this policy, a malicious or compromised external web page could send requests to internal servers (e.g., routers, printers, IoT devices) using the browser as an unwitting proxy.",
                Tags = ["edge", "private network access", "csrf", "internal network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InsecurePrivateNetworkRequestsAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InsecurePrivateNetworkRequestsAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InsecurePrivateNetworkRequestsAllowed", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "External sites cannot access local/intranet resources via the browser; internal web apps on localhost may be affected.",
            },
            new TweakDef
            {
                Id = "edgesec-disable-dino-game",
                Label = "Edge Secure Browsing Policy: Disable Offline Dinosaur Easter Egg Game",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the offline dinosaur/T-Rex game that appears in Microsoft Edge when the device has no internet connection. The game activates on chrome://dino and on error pages when the network is unavailable. Setting AllowDinosaurEasterEgg to 0 suppresses the game. In managed kiosk or enterprise environments, the Easter egg may be considered distracting, and disabling it reinforces that the browser is a business tool where idle game sessions are not permitted.",
                Tags = ["edge", "offline", "easter egg", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AllowDinosaurEasterEgg", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AllowDinosaurEasterEgg")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AllowDinosaurEasterEgg", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "T-Rex offline game is disabled; the offline error page shows the standard error instead.",
            },
            new TweakDef
            {
                Id = "edgesec-disable-guest-mode",
                Label = "Edge Secure Browsing Policy: Disable Guest Browsing Mode",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents users from opening a Guest browsing window in Microsoft Edge. Guest mode creates an isolated profile that does not save browsing history, cookies, or form data but also bypasses enterprise policy enforcement in some cases. Setting BrowserGuestModeEnabled to 0 ensures all browser sessions are subject to the configured enterprise policy controls and prevents data from being accessed through a less-controlled browsing session.",
                Tags = ["edge", "guest mode", "profile", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "BrowserGuestModeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "BrowserGuestModeEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "BrowserGuestModeEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Guest browsing sessions are unavailable; all Edge sessions use managed profiles.",
            },
            new TweakDef
            {
                Id = "edgesec-disable-clickonce",
                Label = "Edge Secure Browsing Policy: Disable ClickOnce Application Launch from Browser",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from launching ClickOnce (.application) packages directly from the browser. ClickOnce is a legacy Microsoft technology that allows .NET applications to be installed and launched from a web server. When ClickOnceEnabled is 0, Edge will not attempt to activate .application files and instead treats them as downloads. This closes a drive-by installation vector where a malicious or compromised site could deliver malware packaged as a ClickOnce app.",
                Tags = ["edge", "clickonce", "application launch", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ClickOnceEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ClickOnceEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ClickOnceEnabled", 0)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ClickOnce web deployments are blocked; users must install apps via approved channels.",
            },
            new TweakDef
            {
                Id = "edgesec-block-sha1-local-anchors",
                Label = "Edge Secure Browsing Policy: Block SHA-1 Certificates from Locally-Trusted CAs",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from trusting certificates signed with the SHA-1 hash algorithm when they are issued by locally-trusted (enterprise) Certificate Authorities. SHA-1 is cryptographically broken and was deprecated by major CAs in 2017. Setting EnableSha1ForLocalAnchors to 0 ensures Edge applies the same SHA-1 deprecation to enterprise certificates as it does to public CA certificates. This forces enterprise PKI administrators to migrate to SHA-256 or better signing algorithms.",
                Tags = ["edge", "sha1", "certificate", "pki", "cryptography", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EnableSha1ForLocalAnchors", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnableSha1ForLocalAnchors")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EnableSha1ForLocalAnchors", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "SHA-1 signed enterprise certificates are rejected; PKI must use SHA-256+ signing algorithms.",
            },
        ];
    }

    // ── EdgeSiteIsolationPolicy ──
    private static class _EdgeSiteIsolationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgiso-enable-strict-origin-isolation",
                    Label = "Enable Strict Origin Isolation in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Enables strict origin isolation so every unique origin (scheme+host+port) runs in its own renderer process instead of grouping origins by site.",
                    Tags = ["edge", "origin-isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Each origin gets dedicated process; more granular isolation than site-level.",
                    ApplyOps = [RegOp.SetDword(Key, "IsolateOrigins", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IsolateOrigins")],
                    DetectOps = [RegOp.CheckDword(Key, "IsolateOrigins", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-enable-renderer-sandbox",
                    Label = "Enable Renderer Process Sandbox in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Enables the renderer process sandbox in Edge, restricting renderer processes' access to the OS to reduce the impact of renderer compromises.",
                    Tags = ["edge", "sandbox", "renderer", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Renderer processes sandboxed; OS-level attacks from compromised renderer are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RendererCodeIntegrityEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RendererCodeIntegrityEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RendererCodeIntegrityEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-block-cross-origin-reads",
                    Label = "Enable Cross-Origin Read Blocking (CORB) in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Enables Cross-Origin Read Blocking (CORB) to prevent sensitive cross-origin responses (HTML, JSON, XML) from being readable by cross-origin scripts, mitigating Spectre side-channel attacks.",
                    Tags = ["edge", "corb", "cross-origin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cross-origin sensitive response bodies blocked from scripts; Spectre leaks via network data mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "CrossOriginReadBlocking", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "CrossOriginReadBlocking")],
                    DetectOps = [RegOp.CheckDword(Key, "CrossOriginReadBlocking", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-disable-shared-memory",
                    Label = "Disable Cross-Process Shared Memory in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Disables shared memory IPC between renderer processes and the browser process, reducing cross-process information leakage vectors in Edge.",
                    Tags = ["edge", "shared-memory", "ipc", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Shared memory for renderer IPC disabled; slight performance overhead for cross-process messages.",
                    ApplyOps = [RegOp.SetDword(Key, "SharedMemoryDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SharedMemoryDisabled")],
                    DetectOps = [RegOp.CheckDword(Key, "SharedMemoryDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-enable-gpu-sandbox",
                    Label = "Enable GPU Process Sandbox in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Enables sandboxing of the Edge GPU process to restrict GPU process access to OS resources, reducing the impact of GPU driver exploits.",
                    Tags = ["edge", "gpu", "sandbox", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GPU process sandboxed; GPU driver exploit impact limited to renderer context.",
                    ApplyOps = [RegOp.SetDword(Key, "GpuSandboxEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "GpuSandboxEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "GpuSandboxEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-block-mixed-content",
                    Label = "Block Mixed Active Content in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Blocks loading of active mixed content (scripts, stylesheets from HTTP on HTTPS pages), preventing downgrade and man-in-the-middle injection on secure pages.",
                    Tags = ["edge", "mixed-content", "https", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "HTTP scripts/styles blocked on HTTPS pages; legacy intranet sites with mixed content may break.",
                    ApplyOps = [RegOp.SetDword(Key, "InsecureContentAllowedForUrls", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "InsecureContentAllowedForUrls")],
                    DetectOps = [RegOp.CheckDword(Key, "InsecureContentAllowedForUrls", 0)],
                },
                new TweakDef
                {
                    Id = "edgiso-force-https-first",
                    Label = "Force HTTPS-First Mode in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Forces Edge to attempt HTTPS connections before HTTP, automatically upgrading site navigation to HTTPS where supported.",
                    Tags = ["edge", "https", "upgrade", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Navigation attempts HTTPS first; HTTP-only sites show warning or fail to load.",
                    ApplyOps = [RegOp.SetDword(Key, "HttpsFirstModeEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HttpsFirstModeEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "HttpsFirstModeEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-disable-webrtc-leak",
                    Label = "Disable WebRTC IP Address Leak in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Configures WebRTC to use only public-facing IP addresses for ICE candidate generation, preventing local and VPN tunnel IP address leakage via WebRTC API.",
                    Tags = ["edge", "webrtc", "ip-leak", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Local/VPN IP addresses not exposed via WebRTC; improves privacy for VPN users.",
                    ApplyOps = [RegOp.SetDword(Key, "WebRtcIPHandling", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WebRtcIPHandling")],
                    DetectOps = [RegOp.CheckDword(Key, "WebRtcIPHandling", 2)],
                },
            ];
    }

    // ── EdgeSleepingTabsPolicy ──
    private static class _EdgeSleepingTabsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgsleep-set-timeout-300",
                    Label = "Set Sleeping Tabs Timeout to 5 Minutes",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets the Sleeping Tabs inactivity timeout to 300 seconds (5 minutes), after which idle tabs are suspended to reclaim memory.",
                    Tags = ["edge", "sleeping-tabs", "timeout", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Tabs inactive for 5+ minutes suspended; shorter timeout = more aggressive memory reclaim.",
                    ApplyOps = [RegOp.SetDword(Key, "SleepingTabsTimeout", 300)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SleepingTabsTimeout")],
                    DetectOps = [RegOp.CheckDword(Key, "SleepingTabsTimeout", 300)],
                },
                new TweakDef
                {
                    Id = "edgsleep-enable-efficiency-mode",
                    Label = "Enable Edge Efficiency Mode",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Enables Edge Efficiency Mode which reduces CPU usage when running on battery or when device resources are constrained, improving battery life on laptops.",
                    Tags = ["edge", "efficiency", "battery", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge scales back CPU when resources are low; better battery life at slight performance cost.",
                    ApplyOps = [RegOp.SetDword(Key, "EfficiencyModeEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EfficiencyModeEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "EfficiencyModeEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgsleep-disable-tab-thumbnail-capture",
                    Label = "Disable Tab Thumbnail Capture for Inactive Tabs",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Disables background thumbnail capture for inactive tabs which wakes sleeping tabs unnecessarily to generate preview images for the tab overview.",
                    Tags = ["edge", "thumbnail", "sleeping-tabs", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sleeping tabs not woken for thumbnail capture; memory savings preserved.",
                    ApplyOps = [RegOp.SetDword(Key, "TabPreviewEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TabPreviewEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "TabPreviewEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgsleep-set-memory-saver-threshold",
                    Label = "Enable Memory Saver Mode in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Enables Edge Memory Saver mode which aggressively frees memory from background tabs when system memory is constrained.",
                    Tags = ["edge", "memory-saver", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Background tabs freed from memory under pressure; page reloads needed when switching to freed tabs.",
                    ApplyOps = [RegOp.SetDword(Key, "MemorySaverEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MemorySaverEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "MemorySaverEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgsleep-disable-reader-mode-preload",
                    Label = "Disable Reader Mode Preloading in Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Disables automatic Reader Mode preloading that parses every article page in the background to prepare a distraction-free view, consuming CPU and memory unnecessarily.",
                    Tags = ["edge", "reader-mode", "preload", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Reader Mode not pre-parsed in background; manual Reader Mode activation still works.",
                    ApplyOps = [RegOp.SetDword(Key, "ReaderModeEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ReaderModeEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ReaderModeEnabled", 0)],
                },
            ];
    }

    // ── EdgeSmartScreenAndSiteIsolationPolicy ──
    private static class _EdgeSmartScreenAndSiteIsolationPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgessf-enable-pua-detection",
                Label = "Edge SmartScreen & Site Isolation Policy: Enable SmartScreen Potentially Unwanted Application Detection",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Enables Microsoft Defender SmartScreen's additional Potentially Unwanted Application (PUA) detection layer in Microsoft Edge. Setting SmartScreenPuaEnabled to 1 makes SmartScreen block downloads of adware, bundleware, and other borderline-unwanted software that passes virus scanning but still exhibits intrusive behaviour. PUA downloads include free tool bundles that silently install toolbars, browser hijackers, and registry cleaners with opaque uninstallers. Enabling PUA detection significantly reduces support burden from accidental installs of bundled software.",
                Tags = ["edge", "smartscreen", "pua", "potentially unwanted", "adware", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SmartScreenPuaEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SmartScreenPuaEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SmartScreenPuaEnabled", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PUA detection enabled alongside standard SmartScreen; bundled adware and browser-hijacker downloads are blocked.",
            },
            new TweakDef
            {
                Id = "edgessf-prevent-smartscreen-override",
                Label = "Edge SmartScreen & Site Isolation Policy: Prevent Users from Overriding SmartScreen Warnings for Sites",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Prevents users from clicking through Microsoft Defender SmartScreen warning pages to visit sites identified as phishing or malware-distributing. Setting PreventSmartScreenPromptOverride to 1 removes the 'Continue anyway' option from SmartScreen's 'This site is not safe' warning page. Without this policy, a determined or socially-engineered user can dismiss any SmartScreen warning with one click. Locking the block removes the user as a weak link in the safety chain and is a CIS Benchmark Level 1 recommendation for enterprise deployments.",
                Tags = ["edge", "smartscreen", "override", "phishing", "security", "cis", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PreventSmartScreenPromptOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PreventSmartScreenPromptOverride")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PreventSmartScreenPromptOverride", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Users cannot click through SmartScreen site warnings; access to flagged phishing/malware sites is hard-blocked.",
            },
            new TweakDef
            {
                Id = "edgessf-prevent-smartscreen-file-override",
                Label = "Edge SmartScreen & Site Isolation Policy: Prevent Users from Overriding SmartScreen Warnings for File Downloads",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Prevents users from bypassing Microsoft Defender SmartScreen download warnings to proceed with a download that SmartScreen has identified as malicious or unrecognised. Setting PreventSmartScreenPromptOverrideForFiles to 1 removes the 'Download anyway' option from SmartScreen's download warning panel. Without this policy, SmartScreen file warnings can be clicked past by any user regardless of IT policy intent. This control is complementary to PreventSmartScreenPromptOverride (for sites) and closes the most common vector for malware delivery via the browser: malicious file downloads.",
                Tags = ["edge", "smartscreen", "download", "malware", "override", "security", "cis", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PreventSmartScreenPromptOverrideForFiles", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PreventSmartScreenPromptOverrideForFiles")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PreventSmartScreenPromptOverrideForFiles", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "SmartScreen file download blocks cannot be bypassed by users; malicious downloads are hard-blocked.",
            },
            new TweakDef
            {
                Id = "edgessf-block-clipboard-api",
                Label = "Edge SmartScreen & Site Isolation Policy: Block Clipboard Access for Web Pages by Default",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Configures Microsoft Edge to block all web page requests to read from or write to the system Clipboard API, with no automatic permissions granted. Setting DefaultClipboardSetting to 2 denies clipboard access to all websites by default; users are not shown a permission prompt. Without this policy, websites can request clipboard permission and then silently read the contents of the clipboard (passwords, PINs, financial data) or inject content, which is a common vector in web-based phishing and session-hijack attacks. Legitimate web applications requiring clipboard access can be allow-listed via ClipboardAllowedForUrls.",
                Tags = ["edge", "clipboard", "permissions", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultClipboardSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultClipboardSetting")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultClipboardSetting", 2)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Websites denied Clipboard API access by default; clipboard content cannot be read or written by untrusted web pages.",
            },
            new TweakDef
            {
                Id = "edgessf-force-site-isolation",
                Label = "Edge SmartScreen & Site Isolation Policy: Force Site Isolation per Origin",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Enables strict site-isolation in Microsoft Edge, ensuring that each distinct website origin is rendered in a separate OS-level process. Setting SitePerProcess to 1 prevents cross-site process sharing, which is the main requirement for defending against Spectre/Meltdown side-channel attacks that attempt to extract data from one origin's renderer process into another's. Site-per-process is the foundational mitigation for cross-site information-leakage attacks at the CPU speculation level and is recommended by Google and Microsoft security teams as an unconditional hardening measure.",
                Tags = ["edge", "site isolation", "spectre", "meltdown", "process isolation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SitePerProcess", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SitePerProcess")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SitePerProcess", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Every origin rendered in a dedicated process; eliminates cross-site renderer sharing required for Spectre-class leaks.",
            },
            new TweakDef
            {
                Id = "edgessf-disable-edge-discover",
                Label = "Edge SmartScreen & Site Isolation Policy: Disable Edge Discover Pane",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the Edge Discover pane (the Copilot/AI side panel entry point that was previously branded 'Discover'). Setting EdgeDiscoverEnabled to 0 removes the Discover/Copilot feature-entry button from the Edge toolbar and prevents the sidebar pane from opening. The Discover pane communicates page context from the active tab to Microsoft's cloud AI services, which represents an unsolicited data transmission for each page visited while the pane is active. Enterprise data-classification policies may prohibit sending intranet or confidential page content to public AI endpoints.",
                Tags = ["edge", "discover", "ai", "copilot", "sidebar", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EdgeDiscoverEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EdgeDiscoverEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EdgeDiscoverEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Discover/Copilot pane button removed from toolbar; no page context is sent to Microsoft AI services via this path.",
            },
            new TweakDef
            {
                Id = "edgessf-disable-vertical-tabs",
                Label = "Edge SmartScreen & Site Isolation Policy: Disable Vertical Tabs Feature",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Prevents users from switching to Edge's vertical tab layout. Setting VerticalTabsAllowed to 0 removes the option to re-orient the tab strip from the top of the browser window to a collapsible list on the left side. In managed environments where desktop screenshots are used for compliance auditing, standardising the browser layout to horizontal tabs makes visual reviews consistent and predictable. Vertical tabs is a UI preference feature with no security implication, but organisations choosing to standardise the interface experience can enforce it via this policy.",
                Tags = ["edge", "vertical tabs", "ui standardisation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "VerticalTabsAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "VerticalTabsAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "VerticalTabsAllowed", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Vertical tabs layout option removed; browser tab strip remains in standard horizontal orientation at top of window.",
            },
            new TweakDef
            {
                Id = "edgessf-disable-adfs",
                Label = "Edge SmartScreen & Site Isolation Policy: Disable ADFS (Active Directory Federation Services) Integration",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables Microsoft Edge's built-in Active Directory Federation Services (ADFS) authentication integration, which automatically attempts federated sign-in to on-premises ADFS endpoints when Microsoft Entra ID (Azure AD) credentials are present. Setting ADFSEnabled to 0 prevents Edge from silently authenticating to ADFS relying parties without explicit user interaction. Organisations that have fully migrated to cloud-only Entra ID or that use a different federation provider (Okta, Ping, ADFS v3+) may wish to disable this integration to avoid unexpected authentication flows and reduce reliance on legacy ADFS infrastructure within the browser.",
                Tags = ["edge", "adfs", "federation", "authentication", "sso", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ADFSEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ADFSEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ADFSEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote =
                    "ADFS automatic sign-in integration disabled in Edge; federated authentication to ADFS relying parties requires explicit user action.",
            },
        ];
    }

    // ── EdgeStartupPolicy ──
    private static class _EdgeStartupPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgestart-set-startup-to-new-tab",
                Label = "Edge Startup Policy: Set Startup Action to Open New Tab Page",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Configures Microsoft Edge to always open the New Tab Page on startup, discarding any previously open tabs and ignoring the 'Continue where you left off' option. This ensures a clean browser state on every launch, which is important for shared workstations, kiosk deployments, and compliance environments where session continuity between logins must not occur.",
                Tags = ["edge", "startup", "new tab", "session", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "RestoreOnStartup", 5)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "RestoreOnStartup")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "RestoreOnStartup", 5)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "RestoreOnStartup=5 opens New Tab Page; Edge discards previous session on every launch.",
            },
            new TweakDef
            {
                Id = "edgestart-disable-sleeping-tabs",
                Label = "Edge Startup Policy: Disable Sleeping Tabs Background CPU Throttle",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the Edge Sleeping Tabs feature that automatically sends unused background tabs into a low-power throttled state after a period of inactivity. While intended to reduce CPU and memory usage, the Sleeping Tabs feature sometimes causes web applications (dashboards, real-time monitoring tools, auto-paging enterprise apps) to lose their session state unexpectedly.",
                Tags = ["edge", "sleeping tabs", "performance", "tabs", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SleepingTabsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SleepingTabsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SleepingTabsEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents background tabs from being throttled; live web dashboards stay active.",
            },
            new TweakDef
            {
                Id = "edgestart-disable-performance-detector",
                Label = "Edge Startup Policy: Disable Edge Performance Detector",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the Edge Performance Detector, which monitors browser and system performance to advise users about slow extensions, resource-heavy tabs, and memory pressure. While informational, the Performance Detector runs a background profiling service and generates telemetry reports sent to Microsoft. In managed environments where performance baselines are set by IT, this unsolicited advisor is unnecessary.",
                Tags = ["edge", "performance detector", "telemetry", "profiling", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PerformanceDetectorEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PerformanceDetectorEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PerformanceDetectorEnabled", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Disables Edge Performance Detector; no impact on actual browser performance.",
            },
            new TweakDef
            {
                Id = "edgestart-disable-new-tab-prerender",
                Label = "Edge Startup Policy: Disable New Tab Page Prerendering",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Prevents Edge from pre-rendering the New Tab Page (NTP) before the user explicitly opens a new tab. The NTP prerender fetches background images, news feed content, and Bing search suggestions in advance. Disabling prerendering reduces background network activity, decreases memory usage, and prevents prefetch requests from appearing in corporate network monitoring tools.",
                Tags = ["edge", "new tab", "prerender", "performance", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "NewTabPagePrerenderEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "NewTabPagePrerenderEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "NewTabPagePrerenderEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables NTP preload; reduces background network calls triggered by opening a new Edge tab.",
            },
            new TweakDef
            {
                Id = "edgestart-force-bing-search-on-ntp",
                Label = "Edge Startup Policy: Lock New Tab Page Search Box to Bing (Prevent Override)",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Configures the New Tab Page search box type to Bing and prevents users from changing it to a third-party or intranet search engine. In enterprise deployments where Bing is the approved search provider (or where the Enterprise New Tab Page is set to a corporate portal), this prevents inconsistency in the search experience and avoids accidental data submission to unapproved search services.",
                Tags = ["edge", "new tab", "search box", "bing", "search engine", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "NewTabPageSearchBox", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "NewTabPageSearchBox")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "NewTabPageSearchBox", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NTP search box set to Bing (value 0); set to 1 to use Edge URL bar search provider instead.",
            },
            new TweakDef
            {
                Id = "edgestart-set-homepage-to-new-tab",
                Label = "Edge Startup Policy: Set Home Page to New Tab Page",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Configures the Edge home page button to navigate to the New Tab Page rather than a custom URL. This ensures the home button is functional and consistent on all managed devices. When combined with the startup action policy, the home button and startup both go to the NTP, providing a consistent entry point across all managed Edge profiles.",
                Tags = ["edge", "homepage", "new tab", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "HomepageIsNewTabPage", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "HomepageIsNewTabPage")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "HomepageIsNewTabPage", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Home button always navigates to the New Tab Page; custom HomepageLocation URLs are ignored.",
            },
            new TweakDef
            {
                Id = "edgestart-disable-experimentation-service",
                Label = "Edge Startup Policy: Disable Edge Experimentation and A/B Testing Service",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Prevents Microsoft Edge from contacting the Experimentation and Configuration Service (ECS) that enrolls the browser in A/B feature experiments and delivers remote feature flag overrides. ECS can silently enable or disable browser features without a version update. In enterprise environments, uncontrolled feature experiments can change behaviour, break web app compatibility, or activate preview features not approved by IT.",
                Tags = ["edge", "experimentation", "ab testing", "feature flags", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ExperimentationAndConfigurationServiceControl", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ExperimentationAndConfigurationServiceControl")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ExperimentationAndConfigurationServiceControl", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables ECS; Edge feature experiments are turned off; browser behavior is determined by installed version only.",
            },
            new TweakDef
            {
                Id = "edgestart-disable-tab-groups",
                Label = "Edge Startup Policy: Disable Edge Tab Groups",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the Edge Tab Groups feature that lets users organize browser tabs into named, colored groups. On kiosk and locked-down devices where the number of open tabs is restricted by policy, and where session restoration of tab groups between logins would create persistent state, disabling tab groups simplifies the browser UX and prevents group state from persisting across logons.",
                Tags = ["edge", "tab groups", "kiosk", "simplification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "TabServicesEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "TabServicesEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "TabServicesEnabled", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Disables Edge tab services and tab groups; only affects tab organization UI.",
            },
        ];
    }

    // ── EdgeTrackingProtectionPolicy ──
    private static class _EdgeTrackingProtectionPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgetrack-strict-tracking-prevention",
                Label = "Edge Tracking Protection Policy: Enforce Strict Tracking Prevention",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Forces Microsoft Edge to use Strict tracking prevention mode, which blocks all known trackers regardless of whether they are from sites the user has previously visited. Strict mode blocks trackers that cause compatibility issues and prevents tracking from any source — including cross-site embedded trackers, fingerprinting scripts, and crypto mining scripts. In privacy-focused corporate environments, strict mode reduces enterprise data leakage to advertising networks.",
                Tags = ["edge", "tracking prevention", "strict", "privacy", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "TrackingPrevention", 3)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "TrackingPrevention")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "TrackingPrevention", 3)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Strict tracking prevention (value 3) blocks all known trackers; some web apps with third-party embedded content may break.",
            },
            new TweakDef
            {
                Id = "edgetrack-clear-cache-on-exit",
                Label = "Edge Tracking Protection Policy: Clear Cached Images and Files on Exit",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Configures Microsoft Edge to automatically delete all cached images and files from the browser cache when the browser closes. The disk cache is a persistent storage mechanism that survives browser restarts and can be used as a covert channel for tracking users across sessions (cache timing attacks, ETag tracking, and cache element counting). Clearing the cache on exit prevents this class of cross-session tracking.",
                Tags = ["edge", "cache", "privacy", "exit", "tracking", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ClearCachedImagesAndFilesOnExit", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ClearCachedImagesAndFilesOnExit")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ClearCachedImagesAndFilesOnExit", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All cached media cleared at browser close; subsequent page loads will re-download previously cached resources.",
            },
            new TweakDef
            {
                Id = "edgetrack-disable-signed-http-exchange",
                Label = "Edge Tracking Protection Policy: Disable Signed HTTP Exchange Support",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables Signed HTTP Exchange (SXG) support in Microsoft Edge. SXG is a web packaging format that allows content to be pre-signed by the original publisher and served from a CDN or cache while appearing to originate from the publisher's domain. While this improves performance for AMP and pre-fetched content, it can break referrer-origin alignment, complicate certificate revocation for distributed content, and interfere with corporate content inspection proxies.",
                Tags = ["edge", "signed http exchange", "sxg", "security", "proxy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SignedHTTPExchangeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SignedHTTPExchangeEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SignedHTTPExchangeEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "SXG pre-signed content disabled; some AMP and pre-cached pages may fall back to regular HTTP fetching.",
            },
            new TweakDef
            {
                Id = "edgetrack-disable-shared-array-buffer-unrestricted",
                Label = "Edge Tracking Protection Policy: Enforce Cross-Origin Isolation for SharedArrayBuffer",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Prevents websites from using the SharedArrayBuffer (SAB) API without proper cross-origin isolation headers (COOP/COEP). SharedArrayBuffer enables high-resolution timer attacks and Spectre-class side-channel exploits when used from pages that are not properly isolated. Disabling unrestricted SAB access forces all websites that require SharedArrayBuffer to declare cross-origin isolation, which significantly reduces Spectre exploitation potential.",
                Tags = ["edge", "sharedarraybuffer", "spectre", "security", "cross-origin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SharedArrayBufferUnrestrictedAccessAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SharedArrayBufferUnrestrictedAccessAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SharedArrayBufferUnrestrictedAccessAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "SAB usage requires COOP/COEP headers; sites lacking cross-origin isolation will lose SAB access.",
            },
            new TweakDef
            {
                Id = "edgetrack-disable-surf-game",
                Label = "Edge Tracking Protection Policy: Disable Surf Easter Egg Game on Error Pages",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the Surf easter egg game that appears on Edge error pages (similar to Chrome's T-Rex game). When AllowSurfGame is set to 0, the game cannot be activated on the no-internet or ERR_CONNECTION_REFUSED error page. In managed enterprise environments and kiosk deployments, the hidden game represents an uncontrolled interactive application running within the browser, which may distract users or enable unintended browser activity on locked-down systems.",
                Tags = ["edge", "surf game", "easter egg", "error page", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AllowSurfGame", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AllowSurfGame")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AllowSurfGame", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Surf easter egg disabled on Edge error pages; no impact on normal browser behavior.",
            },
            new TweakDef
            {
                Id = "edgetrack-block-intrusive-ads",
                Label = "Edge Tracking Protection Policy: Block Ads on Sites with Intrusive Ad Experiences",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Configures Microsoft Edge to block all ads on websites that have been flagged by the Better Ads Standards initiative for running intrusive ad experiences (auto-playing video with sound, countdown interstitials, prestitial ads with countdown). Setting AdsSettingForIntrusiveAdsSites to 2 activates the integrated ad-blocking on flagged domains. This reduces the number of ad trackers loaded, improves page load performance, and removes disruptive content on violating sites.",
                Tags = ["edge", "ads", "intrusive ads", "better ads", "tracking", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AdsSettingForIntrusiveAdsSites", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AdsSettingForIntrusiveAdsSites")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AdsSettingForIntrusiveAdsSites", 2)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ads blocked on Better Ads violating sites (value 2); only affects flagged sites, not all advertising.",
            },
            new TweakDef
            {
                Id = "edgetrack-disable-builtin-dns-client",
                Label = "Edge Tracking Protection Policy: Disable Edge Built-In DNS Client (Use OS DNS)",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the built-in DNS client in Microsoft Edge and forces the browser to use the operating system's DNS resolver for all name resolution. Edge's built-in DNS client can use different DNS servers, timeout settings, and resolution strategies than the OS-configured DNS — potentially bypassing corporate DNS policies, split-horizon DNS configurations, and DNS-based filtering. Using the OS DNS ensures Edge resolution goes through the same monitored and filtered DNS path as all other applications.",
                Tags = ["edge", "dns", "built-in dns", "network", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "BuiltInDnsClientEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "BuiltInDnsClientEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "BuiltInDnsClientEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge uses OS DNS resolver; browser DNS queries follow corporate DNS policy and filtering rules.",
            },
            new TweakDef
            {
                Id = "edgetrack-disable-lens-search",
                Label = "Edge Tracking Protection Policy: Disable Image Lens Region Search",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                Description =
                    "Disables the Lens Region Search feature in Microsoft Edge, which adds a camera/Bing icon to the browser toolbar and context menu allowing users to select a region of any web page and submit it as an image search query to Bing. Lens Region Search uploads a screenshot of the selected page region to Microsoft Bing's image search service. This creates an implicit image-based data exfiltration path for sensitive content displayed on internal corporate web pages or classified documents.",
                Tags = ["edge", "lens search", "bing", "image search", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "LensRegionSearchEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "LensRegionSearchEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "LensRegionSearchEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Lens/image region search removed from Edge context menu; no page screenshots sent to Bing image search.",
            },
        ];
    }

    // ── EdgeWebView2Policy ──
    private static class _EdgeWebView2Policy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\WebView2";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeWebView";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wv2pol-disable-telemetry",
                    Label = "Disable WebView2 Telemetry",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Disables usage and diagnostic telemetry in the Edge WebView2 runtime, preventing embedded browser telemetry from third-party apps that use WebView2 for rendering.",
                    Tags = ["edge", "webview2", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WebView2 telemetry disabled across all hosted apps; no rendered web content telemetry sent.",
                    ApplyOps = [RegOp.SetDword(Key, "MetricsReportingEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MetricsReportingEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "MetricsReportingEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "wv2pol-disable-crash-reporting",
                    Label = "Disable WebView2 Crash Reporting",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Disables automatic crash report uploads from WebView2-hosted applications, preventing crash dump data from being sent to Microsoft.",
                    Tags = ["edge", "webview2", "crash-report", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WebView2 crash data not uploaded; reduce telemetry exposure.",
                    ApplyOps = [RegOp.SetDword(Key, "SendSiteInfoToImproveServices", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SendSiteInfoToImproveServices")],
                    DetectOps = [RegOp.CheckDword(Key, "SendSiteInfoToImproveServices", 0)],
                },
                new TweakDef
                {
                    Id = "wv2pol-disable-auto-update",
                    Label = "Disable WebView2 Runtime Auto-Update",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Disables automatic updates for the Edge WebView2 runtime, ensuring the runtime version is managed by WSUS or MDM rather than auto-updated from the internet.",
                    Tags = ["edge", "webview2", "update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WebView2 runtime version locked; update via managed deployment only.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "wv2pol-block-third-party-cookies",
                    Label = "Block Third-Party Cookies in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Blocks third-party cookies in all WebView2 instances, preventing cross-origin tracking via embedded browser controls in desktop applications.",
                    Tags = ["edge", "webview2", "cookies", "tracking", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Third-party cookies blocked in WebView2; embedded login flows using cross-domain cookies may break.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyCookies", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyCookies")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyCookies", 1)],
                },
                new TweakDef
                {
                    Id = "wv2pol-disable-geolocation",
                    Label = "Disable Geolocation API in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Disables the Geolocation API in WebView2 instances, preventing embedded browser controls from accessing location data.",
                    Tags = ["edge", "webview2", "geolocation", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WebView2 cannot access location data; location-dependent WebView2 features blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultGeolocationSetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultGeolocationSetting")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultGeolocationSetting", 2)],
                },
                new TweakDef
                {
                    Id = "wv2pol-block-file-system-access",
                    Label = "Block File System API Access in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Blocks web content in WebView2 from accessing the local file system via the File System Access API, preventing read/write of arbitrary host files.",
                    Tags = ["edge", "webview2", "file-system", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "File System Access API blocked in WebView2; embedded web content cannot access host files.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultFileSystemReadGuardSetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultFileSystemReadGuardSetting")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultFileSystemReadGuardSetting", 2)],
                },
                new TweakDef
                {
                    Id = "wv2pol-disable-notifications",
                    Label = "Block Notification Requests in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Blocks web push notification permission requests in WebView2 instances, preventing embedded browser controls from requesting notification access.",
                    Tags = ["edge", "webview2", "notifications", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WebView2 push notification prompts suppressed.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultNotificationsSetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultNotificationsSetting")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultNotificationsSetting", 2)],
                },
                new TweakDef
                {
                    Id = "wv2pol-force-safe-browsing",
                    Label = "Force Safe Browsing in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Forces Safe Browsing URL reputation checking in WebView2 instances, ensuring all URLs loaded in embedded browser controls are checked against the Safe Browsing database.",
                    Tags = ["edge", "webview2", "safe-browsing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Safe Browsing active in all WebView2 controls; known malicious URLs blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "SafeBrowsingProtectionLevel", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SafeBrowsingProtectionLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "SafeBrowsingProtectionLevel", 2)],
                },
                new TweakDef
                {
                    Id = "wv2pol-disable-speech-api",
                    Label = "Disable Speech Recognition API in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Disables the Web Speech API in WebView2 instances, preventing embedded browser controls from accessing the microphone for speech recognition.",
                    Tags = ["edge", "webview2", "speech", "microphone", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Speech API disabled in WebView2; microphone access from embedded controls blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSpeechApiFeature", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSpeechApiFeature")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSpeechApiFeature", 1)],
                },
                new TweakDef
                {
                    Id = "wv2pol-block-protocol-handlers",
                    Label = "Block Custom Protocol Handlers in WebView2",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Prevents web content in WebView2 from registering custom URI protocol handlers, blocking potential protocol exploitation vectors in embedded browser controls.",
                    Tags = ["edge", "webview2", "protocol-handlers", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Custom protocol handler registration blocked in WebView2 controls.",
                    ApplyOps = [RegOp.SetDword(Key, "RegisteredProtocolHandlers", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RegisteredProtocolHandlers")],
                    DetectOps = [RegOp.CheckDword(Key, "RegisteredProtocolHandlers", 0)],
                },
            ];
    }

    // ── EdgeWorkProfilePolicy ──
    private static class _EdgeWorkProfilePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgewp-force-work-profile-sign-in",
                    Label = "Edge Work Profile: Force Sign-In with Work or School Account",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets ForceSyncTypes=1 in Edge policy. Forces Edge to require the user to sign in with a work or school account (Entra ID / Microsoft 365) before browsing can begin, ensuring the browser session is always associated with a managed identity and data protection policies are applied. "
                        + "Unauthenticated Edge browsing sessions (guest mode, personal profile, no-sign-in mode) do not inherit the user's Conditional Access, DLP, or browser policy configurations. A user who bypasses the sign-in prompt has a browser session without enterprise policies applied, including without SSL inspection, information barrier enforcement, or Purview data classification labels. Forcing sign-in ensures all browser activity is attributable to a managed identity.",
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
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets PersonalBrowsingAllowed=0 in Edge policy. Prevents users from switching from the managed work profile to an unmanaged personal browsing context within the same Edge browser window, keeping all browsing within the enforceable work profile context. "
                        + "Edge's 'Browse without your data' and 'Personal profile' features allow users to open an unmanaged browser context without enterprise DLP, SSL inspection, and proxy policies. These personal contexts, despite running in the same process, do not inherit the work profile's Conditional Access tokens or data protection rules. A user can copy sensitive data from a managed tab to an unmanaged personal tab, then upload it to a personal cloud service.",
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
                    Id = "edgewp-block-non-work-account-sign-in",
                    Label = "Edge Work Profile: Block Signing In with Non-Work / Personal Accounts",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets RestrictSigninToPattern=1 in Edge policy. Restricts the Edge browser sign-in to accounts that match the organisation's verified domain pattern only, preventing users from signing into Edge with personal Microsoft accounts, Google accounts (via linking), or third-party identity providers. "
                        + "Allowing personal account sign-in into Edge bypasses Conditional Access evaluation because the personal account token does not flow through the organisation's IdP. A user signed in with a personal account in a work browser context can install unvetted extensions, sync personal bookmarks containing personal cloud credentials, and access personal services without DLP policy enforcement.",
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
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets MandatoryBrowserWorkplaceAccess=1 in Edge policy. Enables Microsoft Edge's Mobile Application Management (MAM)-style policy enforcement when the user is authenticated with a work account, automatically applying Intune app protection policies to the Edge browser session. "
                        + "Without MAM/workplace access enforcement, a user on a non-compliant device (e.g., a personal device not enrolled in Intune) can sign into Edge with a work account and access M365 resources without device compliance checks, DLP policies applying to copy/paste, or data transfer restrictions. Workplace access enforcement applies app-level protection policies to the browser session independent of device enrollment state.",
                    Tags = ["edge", "mam", "intune", "app-protection", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Intune MAM app protection policies applied to Edge; data transfer restrictions active even on non-enrolled devices.",
                    ApplyOps = [RegOp.SetDword(Key, "MandatoryBrowserWorkplaceAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MandatoryBrowserWorkplaceAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "MandatoryBrowserWorkplaceAccess", 1)],
                },
                new TweakDef
                {
                    Id = "edgewp-disable-guest-mode-browsing",
                    Label = "Edge Work Profile: Disable Guest Mode Browsing",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets GuestModeEnabled=0 in Edge policy. Disables the Edge 'Guest' profile mode, which launches an ephemeral browser context with no policies, no identity, no browsing history persistence, and no enterprise configuration applied. "
                        + "Guest mode is commonly used to access the browser without any profile's policies applying. On a managed enterprise device, a user opening Edge in guest mode bypasses every configured Edge Group Policy — with no managed account bound to the session. SSL inspection, extension allow-listing, DLP policies, and URL filtering policies all become ineffective in a guest mode session.",
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
                    Id = "edgewp-enforce-managed-profile-on-startup",
                    Label = "Edge Work Profile: Enforce Managed Work Profile at Browser Startup",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets ManagedBrowserStartEnabled=1 in Edge policy. Ensures that Edge always opens to the managed work profile on startup, rather than offering a profile picker or defaulting to the most recently used (possibly personal) profile. "
                        + "On multi-profile Edge installations, the browser may start in a personal or unmanaged profile if that was the last one used. Users habitually working in a personal profile while corporate data is accessible in internal apps may inadvertently paste or upload work data from a personal-profile session where DLP is not enforced. Ensuring startup always opens the managed work profile makes the default context the compliant one.",
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
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets ShareSessionCookiesWithExternalApps=0 in Edge policy. Prevents Edge from sharing session state, cookies, and profile context with Edge WebView2-embedded browser controls in third-party applications, ensuring the managed work profile's authentication tokens are isolated to the Edge browser process. "
                        + "Edge WebView2-based applications (such as Teams, Office web views, and third-party Electron apps) can access the Edge user profile's cookie jar and session tokens if profile sharing is enabled. A compromised Electron application running with Edge WebView2 can silently extract the authenticated session tokens for SharePoint, Exchange, and other M365 services from the shared Edge profile, enabling token theft without the user's knowledge.",
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

    // ── IECompatPolicy ──
    private static class _IECompatPolicy
    {
        private const string IeMainPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Main";
        private const string IeEntMode = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Main\EnterpriseMode";
        private const string IeSecurity = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Security";
        private const string EdgeMain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";
        private const string EdgeCompat = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\IEModeTabUrls";
        private const string IeZones = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "iecompat-disable-ie-enterprise-mode",
                Label = "IE Compat: Disable IE Enterprise Mode Site List",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeEntMode],
                Tags = ["ie", "enterprise-mode", "edge", "compatibility", "security"],
                Description =
                    "Sets Enabled=0 in IE EnterpriseMode policy. Prevents Edge from loading a site list "
                    + "that forces legacy IE rendering mode for specific URLs. "
                    + "Default: can be enabled by policy. Disabling closes a legacy rendering bypass.",
                ApplyOps = [RegOp.SetDword(IeEntMode, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(IeEntMode, "Enabled")],
                DetectOps = [RegOp.CheckDword(IeEntMode, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "iecompat-disable-ie-first-run",
                Label = "IE Compat: Disable IE First-Run Wizard",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeMainPolicy],
                Tags = ["ie", "first-run", "wizard", "policy", "lockdown"],
                Description =
                    "Sets DisableFirstRunCustomize=1 in IE Main policy. Suppresses the Internet Explorer "
                    + "first-run configuration wizard. "
                    + "Default: wizard shown on first launch. Disabling provides a consistent enterprise baseline.",
                ApplyOps = [RegOp.SetDword(IeMainPolicy, "DisableFirstRunCustomize", 1)],
                RemoveOps = [RegOp.DeleteValue(IeMainPolicy, "DisableFirstRunCustomize")],
                DetectOps = [RegOp.CheckDword(IeMainPolicy, "DisableFirstRunCustomize", 1)],
            },
            new TweakDef
            {
                Id = "iecompat-prevent-deleting-ie-cookies",
                Label = "IE Compat: Prevent Users Deleting IE Cookies",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeMainPolicy],
                Tags = ["ie", "cookies", "policy", "compliance"],
                Description =
                    "Sets PreventDeleteCookies=1 in IE policy. Blocks users from deleting IE cookies via "
                    + "browser tools. Useful in compliance environments where cookie retention is required. "
                    + "Default: users can delete cookies freely.",
                ApplyOps = [RegOp.SetDword(IeMainPolicy, "PreventDeleteCookies", 1)],
                RemoveOps = [RegOp.DeleteValue(IeMainPolicy, "PreventDeleteCookies")],
                DetectOps = [RegOp.CheckDword(IeMainPolicy, "PreventDeleteCookies", 1)],
            },
            new TweakDef
            {
                Id = "iecompat-disable-changing-homepage",
                Label = "IE Compat: Prevent Changing IE Start Page",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeMainPolicy],
                Tags = ["ie", "homepage", "lockdown", "policy"],
                Description =
                    "Sets HomePage restriction policy to prevent users from changing the IE start page. "
                    + "Sets PreventHomePage=1. Ensures all users access a consistent enterprise home page. "
                    + "Default: users can change the home page freely.",
                ApplyOps = [RegOp.SetDword(IeMainPolicy, "PreventChangingHomePageURL", 1)],
                RemoveOps = [RegOp.DeleteValue(IeMainPolicy, "PreventChangingHomePageURL")],
                DetectOps = [RegOp.CheckDword(IeMainPolicy, "PreventChangingHomePageURL", 1)],
            },
            new TweakDef
            {
                Id = "iecompat-disable-ie-autocomplete",
                Label = "IE Compat: Disable IE AutoComplete for Forms",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeMainPolicy],
                Tags = ["ie", "autocomplete", "forms", "privacy", "security"],
                Description =
                    "Sets FormSuggest Passwords=no (REG_SZ) in IE policy. Disables AutoComplete for "
                    + "forms in Internet Explorer, preventing credential caching in legacy browser. "
                    + "Default: AutoComplete enabled. Disabling reduces credential exposure from cached form data.",
                ApplyOps = [RegOp.SetString(IeMainPolicy, "FormSuggest Passwords", "no")],
                RemoveOps = [RegOp.DeleteValue(IeMainPolicy, "FormSuggest Passwords")],
                DetectOps = [RegOp.CheckString(IeMainPolicy, "FormSuggest Passwords", "no")],
            },
            new TweakDef
            {
                Id = "iecompat-disable-ie-zone-elevation",
                Label = "IE Compat: Disable Zone Elevation for IE Process",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeSecurity],
                Tags = ["ie", "zone", "elevation", "security", "policy"],
                Description =
                    "Sets IEHarden=1 in IE Security policy. Activates IE Enhanced Security Configuration "
                    + "which assigns all sites to the restricted zone unless explicitly trusted. "
                    + "Default: disabled. Strongly recommended on servers and kiosk machines.",
                ApplyOps = [RegOp.SetDword(IeSecurity, "IEHarden", 1)],
                RemoveOps = [RegOp.DeleteValue(IeSecurity, "IEHarden")],
                DetectOps = [RegOp.CheckDword(IeSecurity, "IEHarden", 1)],
            },
            new TweakDef
            {
                Id = "iecompat-disable-ie-addon-install-prompt",
                Label = "IE Compat: Suppress IE Add-on Install Prompts",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeMainPolicy],
                Tags = ["ie", "addon", "prompt", "lockdown", "policy"],
                Description =
                    "Sets DisableAddonLoadTimePerformanceNotifications=1 in IE policy. Suppresses "
                    + "performance prompts related to add-on load time in Internet Explorer. "
                    + "Default: notifications shown. Suppressing reduces user-side policy bypass paths.",
                ApplyOps = [RegOp.SetDword(IeMainPolicy, "DisableAddonLoadTimePerformanceNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(IeMainPolicy, "DisableAddonLoadTimePerformanceNotifications")],
                DetectOps = [RegOp.CheckDword(IeMainPolicy, "DisableAddonLoadTimePerformanceNotifications", 1)],
            },
            new TweakDef
            {
                Id = "iecompat-enforce-edge-https-upgrades",
                Label = "IE Compat: Enforce HTTPS Upgrades in Edge",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeMain],
                Tags = ["edge", "https", "upgrade", "security", "tls", "policy"],
                Description =
                    "Sets AutomaticHttpsDefault=2 in Edge policy. Forces Edge to upgrade all HTTP "
                    + "navigations to HTTPS automatically. Value 2=enabled with strict upgrade. "
                    + "Default: 0 (disabled). Recommended for zero-trust browsing.",
                ApplyOps = [RegOp.SetDword(EdgeMain, "AutomaticHttpsDefault", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeMain, "AutomaticHttpsDefault")],
                DetectOps = [RegOp.CheckDword(EdgeMain, "AutomaticHttpsDefault", 2)],
            },
        ];
    }

    // ── InternetExplorerRestrictionsPolicy ──
    private static class _InternetExplorerRestrictionsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Restrictions";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ierest-disable-context-menu",
                    Label = "Disable IE Right-Click Context Menu",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoBrowserContextMenu=1 to disable the right-click context menu in Internet Explorer and Edge IE Mode tabs. "
                        + "Prevents users from accessing context-menu options such as Save As, View Source, and Print from within "
                        + "the browser frame, reducing information exfiltration vectors.",
                    Tags = ["ie", "context-menu", "restriction", "edge-ie-mode", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Removes right-click menu in IE/IE Mode; keyboard shortcuts for copy/paste remain functional.",
                    ApplyOps = [RegOp.SetDword(Key, "NoBrowserContextMenu", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoBrowserContextMenu")],
                    DetectOps = [RegOp.CheckDword(Key, "NoBrowserContextMenu", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-browser-options",
                    Label = "Disable IE Internet Options Dialog",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoBrowserOptions=1 to remove access to the Internet Options dialog from IE and Edge IE Mode. "
                        + "Prevents users from modifying proxy settings, security zone configurations, privacy controls, "
                        + "and cached data through the browser settings interface.",
                    Tags = ["ie", "options", "settings", "restriction", "edge-ie-mode", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks Internet Options access; zone and proxy settings can only be changed by an administrator.",
                    ApplyOps = [RegOp.SetDword(Key, "NoBrowserOptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoBrowserOptions")],
                    DetectOps = [RegOp.CheckDword(Key, "NoBrowserOptions", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-view-source",
                    Label = "Disable IE View Source",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoViewSource=1 to prevent users from viewing the HTML source code of web pages in IE and Edge IE Mode. "
                        + "Removing view-source access discourages extraction of embedded credentials, internal URLs, and application "
                        + "structure from intranet and web application pages rendered in IE Mode.",
                    Tags = ["ie", "view-source", "security", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables View Source; developers using IE Mode for legacy apps lose quick HTML inspection.",
                    ApplyOps = [RegOp.SetDword(Key, "NoViewSource", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoViewSource")],
                    DetectOps = [RegOp.CheckDword(Key, "NoViewSource", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-favorites",
                    Label = "Disable IE Favorites Menu",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoFavorites=1 to remove the Favorites menu and prevent users from adding or accessing "
                        + "bookmarked sites in Internet Explorer and Edge IE Mode. Favorites-based URL access creates "
                        + "persistent local references to sites that may bypass proxy policies if bookmarks are stale.",
                    Tags = ["ie", "favorites", "bookmarks", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Removes Favorites menu from IE and IE Mode; existing bookmarks are not deleted.",
                    ApplyOps = [RegOp.SetDword(Key, "NoFavorites", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoFavorites")],
                    DetectOps = [RegOp.CheckDword(Key, "NoFavorites", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-select-download-dir",
                    Label = "Prevent Changing IE Download Folder",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoSelectDownloadDir=1 to prevent users from changing the download destination folder in IE. "
                        + "Forces all file downloads to use the administrator-configured download directory, "
                        + "enabling consistent DLP monitoring of the download path.",
                    Tags = ["ie", "download", "folder", "dlp", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Locks IE download directory; all browser downloads go to the policy-specified folder.",
                    ApplyOps = [RegOp.SetDword(Key, "NoSelectDownloadDir", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoSelectDownloadDir")],
                    DetectOps = [RegOp.CheckDword(Key, "NoSelectDownloadDir", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-find-files",
                    Label = "Disable IE Find Files Command",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoFindFiles=1 to disable the Find > Files or Folders command within Internet Explorer. "
                        + "Prevents users from using the built-in file search capability that can expose the local filesystem "
                        + "from within the browser interface.",
                    Tags = ["ie", "find", "files", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Disables the Find Files menu entry in IE; file search via Explorer and other tools unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "NoFindFiles", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoFindFiles")],
                    DetectOps = [RegOp.CheckDword(Key, "NoFindFiles", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-open-in-new-window",
                    Label = "Prevent IE Links Opening in New Windows",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoOpenInNewWnd=1 to prevent hyperlinks and scripts in Internet Explorer from opening content "
                        + "in new browser windows. Stops script-driven window spawning used by pop-up ads and potentially "
                        + "malicious redirects rendered via IE Mode.",
                    Tags = ["ie", "new-window", "pop-up", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Prevents link-in-new-window; may break legacy IE Mode apps that use popup dialogs.",
                    ApplyOps = [RegOp.SetDword(Key, "NoOpenInNewWnd", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoOpenInNewWnd")],
                    DetectOps = [RegOp.CheckDword(Key, "NoOpenInNewWnd", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-browser-toolbar",
                    Label = "Remove IE Browser Toolbar",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoToolBar=1 to remove the toolbar from Internet Explorer and Edge IE Mode. "
                        + "Prevents access to toolbar controls, add-ons, and navigation shortcuts from the toolbar area. "
                        + "Reduces the visible browser surface area for kiosk-style IE Mode deployments.",
                    Tags = ["ie", "toolbar", "restriction", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Removes toolbar from IE and IE Mode; address bar and navigation controls remain available.",
                    ApplyOps = [RegOp.SetDword(Key, "NoToolBar", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoToolBar")],
                    DetectOps = [RegOp.CheckDword(Key, "NoToolBar", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-theater-mode",
                    Label = "Disable IE Theater / Full-Screen Mode",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoTheaterMode=1 to disable the Theater Mode (full-screen F11 view) in Internet Explorer. "
                        + "Prevents users from entering full-screen presentation mode, which hides the taskbar and system indicators "
                        + "and can be exploited for phishing overlays that mimic OS dialogs.",
                    Tags = ["ie", "theater-mode", "full-screen", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Disables F11 full-screen mode in IE; minor usability change for normal browser operation.",
                    ApplyOps = [RegOp.SetDword(Key, "NoTheaterMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoTheaterMode")],
                    DetectOps = [RegOp.CheckDword(Key, "NoTheaterMode", 1)],
                },
                new TweakDef
                {
                    Id = "ierest-disable-close-browser",
                    Label = "Disable IE Close Browser Button",
                    Category = "Browser — Edge Smart Screen And Site Isolation",
                    Description =
                        "Sets NoBrowserClose=1 to prevent users from closing the Internet Explorer window via the X button or "
                        + "File > Close. Used in kiosk and locked-down browsing scenarios where IE is the only interface "
                        + "and the browser must remain open for the session.",
                    Tags = ["ie", "close", "kiosk", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "Prevents closing IE window; intended for kiosk deployments only — may frustrate users on normal endpoints.",
                    ApplyOps = [RegOp.SetDword(Key, "NoBrowserClose", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoBrowserClose")],
                    DetectOps = [RegOp.CheckDword(Key, "NoBrowserClose", 1)],
                },
            ];
    }

    // ── InternetZonePolicy ──
    private static class _InternetZonePolicy
    {
        // Root Internet Settings policy — applies machine-wide
        private const string InetPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

        // Zone 3 = Internet (HKLM policy version overrides HKCU)
        private const string Zone3 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3";

        // IE SmartScreen / Phishing Filter
        private const string PhishFilter = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\PhishingFilter";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "izone-lock-zones-to-machine",
                Label = "Lock Security Zones to Machine Policy",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "security", "zones", "policy", "hardening"],
                Description =
                    "Forces all Internet security zone settings to be read from HKLM machine policy. "
                    + "Users cannot change zone configurations via the Internet Options dialog. "
                    + "Essential in managed environments to enforce uniform zone security. "
                    + "Security_HKLM_only=1.",
                ApplyOps = [RegOp.SetDword(InetPol, "Security_HKLM_only", 1)],
                RemoveOps = [RegOp.DeleteValue(InetPol, "Security_HKLM_only")],
                DetectOps = [RegOp.CheckDword(InetPol, "Security_HKLM_only", 1)],
            },
            new TweakDef
            {
                Id = "izone-block-activex-internet",
                Label = "Disable ActiveX Controls in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "activex", "security", "zone", "hardening"],
                Description =
                    "Disables execution of ActiveX controls and plug-ins in the Internet security zone "
                    + "(zone 3, action code 1200 = 3). Prevents drive-by download and ActiveX exploitation. "
                    + "Legacy LOB apps using ActiveX must be moved to the Trusted Sites zone.",
                ApplyOps = [RegOp.SetDword(Zone3, "1200", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1200")],
                DetectOps = [RegOp.CheckDword(Zone3, "1200", 3)],
            },
            new TweakDef
            {
                Id = "izone-block-activescript-internet",
                Label = "Disable Active Scripting in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "scripting", "javascript", "zone", "security"],
                Description =
                    "Disables JavaScript and VBScript execution in the Internet security zone for MSHTML "
                    + "applications (action code 1400 = 3). Reduces XSS and script-injection attack surface "
                    + "for WebView2/MSHTML-hosted content in legacy line-of-business applications.",
                ApplyOps = [RegOp.SetDword(Zone3, "1400", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1400")],
                DetectOps = [RegOp.CheckDword(Zone3, "1400", 3)],
            },
            new TweakDef
            {
                Id = "izone-prevent-cert-error-bypass",
                Label = "Prevent Users Bypassing Certificate Errors",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "certificate", "tls", "security", "ssl"],
                Description =
                    "Prevents users from clicking through TLS/SSL certificate errors to continue to HTTPS "
                    + "sites with invalid certificates. PreventIgnoreCertErrors=1. Effective defence against "
                    + "certificate spoofing and man-in-the-middle downgrade attacks.",
                ApplyOps = [RegOp.SetDword(InetPol, "PreventIgnoreCertErrors", 1)],
                RemoveOps = [RegOp.DeleteValue(InetPol, "PreventIgnoreCertErrors")],
                DetectOps = [RegOp.CheckDword(InetPol, "PreventIgnoreCertErrors", 1)],
            },
            new TweakDef
            {
                Id = "izone-block-auto-file-download",
                Label = "Block Automatic File Download Prompts in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "download", "zone", "security", "policy"],
                Description =
                    "Blocks automatic file download prompts triggered by Internet zone content in MSHTML apps "
                    + "(action 1803 = 3, disable automatic prompting for file downloads). Targets legacy LOB "
                    + "apps — modern browsers manage downloads independently of zone settings.",
                ApplyOps = [RegOp.SetDword(Zone3, "1803", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1803")],
                DetectOps = [RegOp.CheckDword(Zone3, "1803", 3)],
            },
            new TweakDef
            {
                Id = "izone-enable-smartscreen-legacy",
                Label = "Enable SmartScreen Phishing Filter for Legacy Apps",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "smartscreen", "phishing", "filter", "security"],
                Description =
                    "Forces the SmartScreen phishing filter on for all zones in the legacy MSHTML engine "
                    + "and Office WebBrowser controls. EnabledV9=1 ensures the filter is active regardless "
                    + "of per-user settings. Relevant on systems with LOB apps using WebBrowser control.",
                ApplyOps = [RegOp.SetDword(PhishFilter, "EnabledV9", 1)],
                RemoveOps = [RegOp.DeleteValue(PhishFilter, "EnabledV9")],
                DetectOps = [RegOp.CheckDword(PhishFilter, "EnabledV9", 1)],
            },
            new TweakDef
            {
                Id = "izone-block-form-submit-unencrypted",
                Label = "Block Unencrypted Form Submission in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "form", "https", "encryption", "data"],
                Description =
                    "Prevents MSHTML-based apps from submitting form data to HTTP (non-HTTPS) endpoints "
                    + "from the Internet zone (action 1601 = 3). Stops accidental credential submission to "
                    + "unencrypted servers from legacy application WebBrowser controls.",
                ApplyOps = [RegOp.SetDword(Zone3, "1601", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1601")],
                DetectOps = [RegOp.CheckDword(Zone3, "1601", 3)],
            },
            new TweakDef
            {
                Id = "izone-block-mixed-content",
                Label = "Block Mixed HTTP/HTTPS Content in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "mixed-content", "https", "http", "security"],
                Description =
                    "Disables loading of mixed content (HTTP resources inside HTTPS pages) in the Internet "
                    + "zone for MSHTML/WebBrowser-hosted content (action 1609 = 3). Prevents downgrade and "
                    + "protocol confusion attacks without just prompting.",
                ApplyOps = [RegOp.SetDword(Zone3, "1609", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1609")],
                DetectOps = [RegOp.CheckDword(Zone3, "1609", 3)],
            },
            new TweakDef
            {
                Id = "izone-block-unsafe-activex-init",
                Label = "Block Unsafe ActiveX Initialisation in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "activex", "signed", "unsafe", "zone"],
                Description =
                    "Disables initialisation and scripting of ActiveX controls not marked as safe for "
                    + "scripting (APTCA) in the Internet zone (action 1201 = 3). Reduces exploitation "
                    + "of legacy ActiveX controls while allowing properly marked-safe controls.",
                ApplyOps = [RegOp.SetDword(Zone3, "1201", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1201")],
                DetectOps = [RegOp.CheckDword(Zone3, "1201", 3)],
            },
            new TweakDef
            {
                Id = "izone-block-script-clipboard-internet",
                Label = "Block Script Access to Clipboard in Internet Zone",
                Category = "Browser — Edge Smart Screen And Site Isolation",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["internet", "clipboard", "script", "zone", "security"],
                Description =
                    "Prevents scripts running in the Internet security zone from accessing the clipboard "
                    + "programmatically (action 1406 = 3 — disable cut/copy/paste via script). Prevents "
                    + "malicious web content from stealing clipboard data such as passwords or auth tokens.",
                ApplyOps = [RegOp.SetDword(Zone3, "1406", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone3, "1406")],
                DetectOps = [RegOp.CheckDword(Zone3, "1406", 3)],
            },
        ];
    }

    // ── LegacyEdgePolicy ──
    private static class _LegacyEdgePolicy
    {
        private const string MainKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main";
        private const string PhishingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter";
        private const string TabKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\TabPreloader";
        private const string InprivateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\InPrivate";
        private const string ServiceUiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\ServiceUI";
        private const string InternetSettingsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Internet Settings";
        private const string ExtensionsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Extensions";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ledge-block-about-flags",
                    Label = "Block Access to edge://flags in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Prevents access to the edge://flags page in the legacy Microsoft Edge (EdgeHTML) browser, stopping users from enabling experimental features that may bypass security controls.",
                    Tags = ["edge", "legacy edge", "flags", "experimental", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks experimental feature toggles in legacy EdgeHTML; no effect on Chromium Edge.",
                    RegistryKeys = [MainKey],
                    ApplyOps = [RegOp.SetDword(MainKey, "PreventAccessToAboutFlagsInMicrosoftEdge", 1)],
                    RemoveOps = [RegOp.DeleteValue(MainKey, "PreventAccessToAboutFlagsInMicrosoftEdge")],
                    DetectOps = [RegOp.CheckDword(MainKey, "PreventAccessToAboutFlagsInMicrosoftEdge", 1)],
                },
                new TweakDef
                {
                    Id = "ledge-disable-address-bar-dropdown",
                    Label = "Disable Address Bar Drop-Down List in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Disables the drop-down suggestion list that appears when the user types in the legacy Edge address bar, preventing URL history and search suggestion exposure.",
                    Tags = ["edge", "legacy edge", "address bar", "suggestions", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes URL history exposure in the address bar; users still navigate but without autocomplete.",
                    RegistryKeys = [MainKey],
                    ApplyOps = [RegOp.SetDword(MainKey, "AllowAddressBarDropdown", 0)],
                    RemoveOps = [RegOp.DeleteValue(MainKey, "AllowAddressBarDropdown")],
                    DetectOps = [RegOp.CheckDword(MainKey, "AllowAddressBarDropdown", 0)],
                },
                new TweakDef
                {
                    Id = "ledge-disable-tab-preloading",
                    Label = "Disable Tab Preloading at Windows Startup in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Prevents legacy Microsoft Edge from preloading tabs in the background when Windows starts, reducing RAM usage and startup overhead on managed systems.",
                    Tags = ["edge", "legacy edge", "tab", "preload", "startup", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Reduces startup RAM consumption; Edge still loads normally when launched by the user.",
                    RegistryKeys = [TabKey],
                    ApplyOps = [RegOp.SetDword(TabKey, "PreventTabPreloading", 1)],
                    RemoveOps = [RegOp.DeleteValue(TabKey, "PreventTabPreloading")],
                    DetectOps = [RegOp.CheckDword(TabKey, "PreventTabPreloading", 1)],
                },
                new TweakDef
                {
                    Id = "ledge-prevent-smartscreen-bypass",
                    Label = "Prevent Bypassing SmartScreen Warnings in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Prevents users from ignoring or bypassing SmartScreen phishing and malware warnings in legacy Microsoft Edge, enforcing the block when a threat is detected.",
                    Tags = ["edge", "legacy edge", "smartscreen", "security", "bypass", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Prevents click-through on SmartScreen threat warnings — users cannot override the block.",
                    RegistryKeys = [PhishingKey],
                    ApplyOps = [RegOp.SetDword(PhishingKey, "PreventOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(PhishingKey, "PreventOverride")],
                    DetectOps = [RegOp.CheckDword(PhishingKey, "PreventOverride", 1)],
                },
                new TweakDef
                {
                    Id = "ledge-disable-inprivate-browsing",
                    Label = "Disable InPrivate Browsing in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Disables InPrivate browsing mode in legacy Microsoft Edge, ensuring all sessions are tracked in history so that browsing can be audited on managed devices.",
                    Tags = ["edge", "legacy edge", "inprivate", "private browsing", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks private mode; all browsing sessions are retained for audit purposes.",
                    RegistryKeys = [InprivateKey],
                    ApplyOps = [RegOp.SetDword(InprivateKey, "DisableInPrivateBrowsing", 1)],
                    RemoveOps = [RegOp.DeleteValue(InprivateKey, "DisableInPrivateBrowsing")],
                    DetectOps = [RegOp.CheckDword(InprivateKey, "DisableInPrivateBrowsing", 1)],
                },
                new TweakDef
                {
                    Id = "ledge-prevent-flip-ahead",
                    Label = "Disable Flip Ahead Page Prediction in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Disables the Flip Ahead feature in legacy Microsoft Edge that pre-fetches the next page in a series, preventing unsolicited network requests and reducing data sent to Microsoft.",
                    Tags = ["edge", "legacy edge", "flip ahead", "prefetch", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops speculative page prefetching; no user-visible behaviour change except removal of the swipe gesture.",
                    RegistryKeys = [InternetSettingsKey],
                    ApplyOps = [RegOp.SetDword(InternetSettingsKey, "PreventFlipAheadEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(InternetSettingsKey, "PreventFlipAheadEnabled")],
                    DetectOps = [RegOp.CheckDword(InternetSettingsKey, "PreventFlipAheadEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "ledge-hide-first-run-prompt",
                    Label = "Hide the First-Run Welcome Page in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Suppresses the first-run welcome page and setup wizard in legacy Microsoft Edge, streamlining deployment on managed machines where browser configuration is set by policy.",
                    Tags = ["edge", "legacy edge", "first run", "onboarding", "deployment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes the welcome wizard on first Edge launch; profile settings come from policy instead.",
                    RegistryKeys = [ServiceUiKey],
                    ApplyOps = [RegOp.SetDword(ServiceUiKey, "AllowWebContentOnNewTabPage", 0)],
                    RemoveOps = [RegOp.DeleteValue(ServiceUiKey, "AllowWebContentOnNewTabPage")],
                    DetectOps = [RegOp.CheckDword(ServiceUiKey, "AllowWebContentOnNewTabPage", 0)],
                },
                new TweakDef
                {
                    Id = "ledge-prevent-extension-dev-tools",
                    Label = "Prevent Loading Unpacked Extensions in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Blocks loading of extensions that are not from the Microsoft Store in legacy Microsoft Edge, preventing unpacked or sideloaded extensions from running on managed devices.",
                    Tags = ["edge", "legacy edge", "extensions", "developer mode", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks sideloaded extensions; only Store-approved extensions can be installed.",
                    RegistryKeys = [ExtensionsKey],
                    ApplyOps = [RegOp.SetDword(ExtensionsKey, "AllowExtensionSideloading", 0)],
                    RemoveOps = [RegOp.DeleteValue(ExtensionsKey, "AllowExtensionSideloading")],
                    DetectOps = [RegOp.CheckDword(ExtensionsKey, "AllowExtensionSideloading", 0)],
                },
                new TweakDef
                {
                    Id = "ledge-disable-home-button",
                    Label = "Disable the Home Button in Legacy Edge",
                    Category = "Browser — Legacy Edge",
                    Description =
                        "Removes the home button from the legacy Microsoft Edge toolbar, preventing users from quickly navigating to a home page that may not comply with enterprise navigation policies.",
                    Tags = ["edge", "legacy edge", "home button", "toolbar", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Removes the home button from the Edge toolbar; enterprise start page is still enforced via other policy.",
                    RegistryKeys = [MainKey],
                    ApplyOps = [RegOp.SetDword(MainKey, "HomeButtonEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(MainKey, "HomeButtonEnabled")],
                    DetectOps = [RegOp.CheckDword(MainKey, "HomeButtonEnabled", 0)],
                },
            ];
    }
}
