namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class EdgeSecureBrowsingPolicy
{
    private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edgesec-enable-revocation-checks",
            Label = "Edge Secure Browsing Policy: Enable Online Certificate Revocation Checks",
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Category = "Edge Secure Browsing Policy",
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
            Id = "edgesec-enable-https-only-mode",
            Label = "Edge Secure Browsing Policy: Enable HTTPS-Only Browsing Mode",
            Category = "Edge Secure Browsing Policy",
            Description =
                "Enables HTTPS-Only mode in Microsoft Edge, which configures the browser to require HTTPS for all navigations. When HttpsOnlyMode is set to 1, Edge will attempt to connect via HTTPS by default and will show a warning page before loading any site over plain HTTP, allowing the user to proceed or stay secure. This value 1 enables the optional mode (users can still override per-site), while value 2 would enforce it without override. Use 1 in most enterprise environments as a default-secure posture.",
            Tags = ["edge", "https only", "tls", "secure browsing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "HttpsOnlyMode", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "HttpsOnlyMode")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "HttpsOnlyMode", 1)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "HTTPS-Only mode enabled; HTTP pages show a warning before loading (user can override per-site).",
        },
        new TweakDef
        {
            Id = "edgesec-block-sha1-local-anchors",
            Label = "Edge Secure Browsing Policy: Block SHA-1 Certificates from Locally-Trusted CAs",
            Category = "Edge Secure Browsing Policy",
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
