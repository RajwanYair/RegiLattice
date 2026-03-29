// RegiLattice.Core — Tweaks/EdgeEarlyHintsPolicy.cs
// Edge network hinting, HSTS preload, DNS prefetch, and preconnect controls — Sprint 456.
// Category: "Edge Early Hints Policy" | Slug: edgehint
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeEarlyHintsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "edgehint-enable-hsts-preloading",
                Label = "Enable HSTS Preload List in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Enables Edge to use the HSTS (HTTP Strict Transport Security) preload list, automatically using HTTPS for sites known to enforce it before the first request.",
                Tags = ["edge", "hsts", "preload", "https", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "HTTPS automatically used for HSTS-enrolled sites; first-connection downgrades prevented.",
                ApplyOps = [RegOp.SetDword(Key, "HSTSPolicyBypassList", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "HSTSPolicyBypassList")],
                DetectOps = [RegOp.CheckDword(Key, "HSTSPolicyBypassList", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-disable-dns-prefetch",
                Label = "Disable DNS Prefetching in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Disables DNS prefetching in Edge which resolves domain names of links on a page before the user navigates to them, eliminating DNS-based pre-browsing data leakage.",
                Tags = ["edge", "dns-prefetch", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "DNS prefetch disabled; slightly slower navigation, no pre-navigation DNS leakage.",
                ApplyOps = [RegOp.SetDword(Key, "DNSPrefetchingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DNSPrefetchingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "DNSPrefetchingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-disable-preconnect",
                Label = "Disable Speculative Preconnect in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Disables speculative TCP/TLS preconnection to link destinations in Edge, reducing background network connections initiated without user interaction.",
                Tags = ["edge", "preconnect", "privacy", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Preconnect background sockets not opened; network idle time improved, slight navigation latency.",
                ApplyOps = [RegOp.SetDword(Key, "SpeculativePreconnectEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SpeculativePreconnectEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SpeculativePreconnectEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-enforce-cert-transparency",
                Label = "Enforce Certificate Transparency in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Enforces Certificate Transparency (CT) log checking for all TLS certificates in Edge, ensuring all served certificates are publicly auditable and undisclosed certificates are rejected.",
                Tags = ["edge", "certificate-transparency", "tls", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "CT enforcement blocks rogue certificates not in public CT logs; enterprise HTTPS inspection certs need CT logging.",
                ApplyOps = [RegOp.SetDword(Key, "CertificateTransparencyEnforcementDisabledForUrls", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "CertificateTransparencyEnforcementDisabledForUrls")],
                DetectOps = [RegOp.CheckDword(Key, "CertificateTransparencyEnforcementDisabledForUrls", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-enable-doh-secure-mode",
                Label = "Enable Secure DNS (DoH) in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Enables DNS-over-HTTPS (Secure DNS) in Edge, protecting DNS queries from eavesdropping and manipulation by using encrypted DNS resolution.",
                Tags = ["edge", "doh", "dns", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "DoH active in Edge; DNS queries encrypted even if OS-level DoH is not configured.",
                ApplyOps = [RegOp.SetDword(Key, "DnsOverHttpsMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DnsOverHttpsMode")],
                DetectOps = [RegOp.CheckDword(Key, "DnsOverHttpsMode", 1)],
            },
            new TweakDef
            {
                Id = "edgehint-disable-early-hints-header",
                Label = "Disable HTTP 103 Early Hints Processing in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Disables processing of HTTP 103 Early Hints response headers in Edge, which pre-load resources before the final 200 response arrives. Reduces speculative pre-fetching.",
                Tags = ["edge", "early-hints", "http103", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "HTTP 103 hints ignored; no premature resource loading on hint signals.",
                ApplyOps = [RegOp.SetDword(Key, "EarlyHintsModeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EarlyHintsModeEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "EarlyHintsModeEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-block-external-protocol-dialogs",
                Label = "Block External Protocol Launch Dialogs in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Suppresses the permission dialog for external protocol launches (e.g., opening apps via custom URI schemes from web pages) in Edge, blocking malicious app-launch attempts.",
                Tags = ["edge", "protocol-launch", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "External URI scheme prompts blocked; web pages cannot auto-launch installed apps without explicit allow-list.",
                ApplyOps = [RegOp.SetDword(Key, "ExternalProtocolDialogShowAlwaysOpenCheckbox", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExternalProtocolDialogShowAlwaysOpenCheckbox")],
                DetectOps = [RegOp.CheckDword(Key, "ExternalProtocolDialogShowAlwaysOpenCheckbox", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-disable-address-bar-prediction",
                Label = "Disable Address Bar Search/URL Prediction in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Disables the Edge address bar prediction feature that sends partially typed URLs to Microsoft search services for autocomplete suggestions.",
                Tags = ["edge", "address-bar", "prediction", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Address bar suggestions disabled; typed URLs not sent to Microsoft until Enter is pressed.",
                ApplyOps = [RegOp.SetDword(Key, "SearchSuggestEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SearchSuggestEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SearchSuggestEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-disable-browser-sign-in",
                Label = "Disable Browser Sign-In in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Disables signing into Microsoft Edge with a personal or work Microsoft account, preventing browser sync of history, passwords, bookmarks, and browsing data.",
                Tags = ["edge", "sign-in", "sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Browser sign-in disabled; no sync to Microsoft account or Entra ID for personal profiles.",
                ApplyOps = [RegOp.SetDword(Key, "BrowserSignin", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "BrowserSignin")],
                DetectOps = [RegOp.CheckDword(Key, "BrowserSignin", 0)],
            },
            new TweakDef
            {
                Id = "edgehint-disable-smart-actions",
                Label = "Disable Bing Smart Actions in Edge",
                Category = "Edge Early Hints Policy",
                Description =
                    "Disables Bing-powered Smart Actions in Edge that analyse page content and selected text to offer contextual services (definitions, translations, purchases).",
                Tags = ["edge", "smart-actions", "bing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Smart Actions disabled; selected text not analysed by Bing for suggestions.",
                ApplyOps = [RegOp.SetDword(Key, "EdgeDisableExplicitMicrosoftServicesIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EdgeDisableExplicitMicrosoftServicesIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "EdgeDisableExplicitMicrosoftServicesIntegration", 1)],
            },
        ];
}
