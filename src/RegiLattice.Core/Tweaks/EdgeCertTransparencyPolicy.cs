// RegiLattice.Core — Tweaks/EdgeCertTransparencyPolicy.cs
// Microsoft Edge Certificate Transparency, SSL inspection, and PKI policy Group Policy controls (Sprint 615).
// Category: "Edge Certificate Transparency Policy" | Slug: edgect
// Key: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeCertTransparencyPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "edgect-require-certificate-transparency",
            Label = "Edge Cert Transparency: Enforce Certificate Transparency Log Requirement",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets CertificateTransparencyEnforcementDisabledForUrls=0 (enforcement enabled) in Edge policy. Requires that all TLS certificates presented to Edge are logged in a public Certificate Transparency (CT) log, and rejects connections to HTTPS sites whose certificates are not included in a trusted CT log. " +
                "Certificate Transparency logs provide a publicly auditable record of all certificates issued by trusted CAs. Without CT enforcement, a CA that has been compromised or coerced (e.g., by a nation-state issuing a fraudulent wildcard certificate for *.company.com) can issue certificates that Edge trusts without any detection mechanism. CT enforcement means that any certificate not logged in a trusted public log will cause Edge to display a certificate error.",
            Tags = ["edge", "certificate-transparency", "tls", "ca", "pki"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "CT enforcement enabled; HTTPS connections with non-CT-logged certificates blocked.",
            ApplyOps = [RegOp.SetDword(Key, "CertificateTransparencyEnforcementEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "CertificateTransparencyEnforcementEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "CertificateTransparencyEnforcementEnabled", 1)],
        },
        new TweakDef
        {
            Id = "edgect-disable-obsolete-tls-versions",
            Label = "Edge Cert Transparency: Block Connections Using TLS 1.0 and 1.1",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets SSLVersionMin=\"tls1.2\" in Edge policy. Sets the minimum TLS protocol version that Edge will accept for HTTPS connections to TLS 1.2, causing connections to servers that only support TLS 1.0 or 1.1 to fail with a connection error. " +
                "TLS 1.0 and TLS 1.1 contain known protocol weaknesses: BEAST (TLS 1.0), POODLE (TLS 1.0/1.1 can be forced cross-protocol), and SLOTH. The cipher suite negotiation for TLS 1.0/1.1 includes RC4 and CBC-mode AES ciphers that are vulnerable to practical attacks. PCI DSS, NIST SP 800-52 Rev 2, and HIPAA technical safeguards all require TLS 1.2 or higher. Disabling legacy TLS prevents protocol downgrade attacks.",
            Tags = ["edge", "tls", "ssl-version", "protocol-downgrade", "pci-dss"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "TLS 1.0 and 1.1 blocked in Edge; connections require TLS 1.2 minimum.",
            ApplyOps = [RegOp.SetString(Key, "SSLVersionMin", "tls1.2")],
            RemoveOps = [RegOp.DeleteValue(Key, "SSLVersionMin")],
            DetectOps = [RegOp.CheckString(Key, "SSLVersionMin", "tls1.2")],
        },
        new TweakDef
        {
            Id = "edgect-enable-revocation-checking",
            Label = "Edge Cert Transparency: Enable OCSP/CRL Certificate Revocation Checking",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets OnlineRevocationChecksEnabled=1 in Edge policy. Enables Edge to perform online certificate revocation checks via OCSP (Online Certificate Status Protocol) and CRL (Certificate Revocation List) for every TLS certificate it encounters, blocking connections to sites whose certificates have been revoked. " +
                "Certificate revocation exists to allow CAs to invalidate certificates when the corresponding private key is compromised. Without revocation checking, Edge accepts revoked certificates as valid — meaning that if a private key for a trusted certificate is stolen and the CA issues a revocation, Edge will still accept connections authenticated by the stolen key until the site renews its certificate. OCSP checking provides near-real-time revocation status.",
            Tags = ["edge", "ocsp", "crl", "revocation", "certificate"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "OCSP/CRL revocation checked for all edge TLS connections; revoked certificates cause connection failure.",
            ApplyOps = [RegOp.SetDword(Key, "OnlineRevocationChecksEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "OnlineRevocationChecksEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "OnlineRevocationChecksEnabled", 1)],
        },
        new TweakDef
        {
            Id = "edgect-block-sha1-signed-certificates",
            Label = "Edge Cert Transparency: Block Sites with SHA-1 Signed TLS Certificates",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets SHA1CertificateEnabled=0 in Edge policy. Causes Edge to refuse TLS connections to sites whose certificates are signed using the SHA-1 hash algorithm, requiring all accepted certificates to use SHA-256 or stronger. " +
                "SHA-1 was deprecated as a certificate signing hash algorithm in 2017 following the demonstration of practical chosen-prefix collision attacks. A SHA-1 collision allows an attacker to create two different certificates with the same signature — enabling fraudulent certificate creation if a CA still issues SHA-1 certificates. Any SHA-1 certificate remaining in production is non-compliant with modern PKI standards and suggests poor certificate lifecycle management.",
            Tags = ["edge", "sha1", "certificate", "hash", "deprecation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "SHA-1 signed TLS certificates refused by Edge; sites must present SHA-256 or stronger certificates.",
            ApplyOps = [RegOp.SetDword(Key, "SHA1CertificateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SHA1CertificateEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "SHA1CertificateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgect-require-ct-for-local-anchored-certs",
            Label = "Edge Cert Transparency: Require CT Even for Locally-Anchored Enterprise Certificates",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets RequireCTForLocallyAnchoredCerts=1 in Edge policy. Extends Certificate Transparency enforcement to certificates anchored at locally-installed enterprise root CAs (not just public CAs), requiring that internal HTTPS sites served by the enterprise PKI include a Signed Certificate Timestamp (SCT) or be logged in a compatible CT log. " +
                "Enterprise internal CAs can issue certificates for any domain, including external domains. An enterprise CA that has been compromised can issue a certificate for google.com or company-partner.com that Edge would normally trust (since it's anchored at the enterprise root). Requiring CT for locally-anchored certificates means Enterprise CA-issued certificates for unexpected domains will fail CT validation, detecting CA misuse.",
            Tags = ["edge", "certificate-transparency", "enterprise-ca", "internal-pki", "misuse"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "CT required for enterprise PKI certificates; internal CA misuse (issuing certs for external domains) detected.",
            ApplyOps = [RegOp.SetDword(Key, "RequireCTForLocallyAnchoredCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireCTForLocallyAnchoredCerts")],
            DetectOps = [RegOp.CheckDword(Key, "RequireCTForLocallyAnchoredCerts", 1)],
        },
        new TweakDef
        {
            Id = "edgect-block-invalid-certificate-warning-bypass",
            Label = "Edge Cert Transparency: Block User Override of Certificate Error Pages",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets SSLErrorOverrideAllowed=0 in Edge policy. Removes the 'Proceed anyway' / 'Advanced → Proceed to site' bypass button from Edge certificate error pages, preventing users from overriding TLS certificate errors by clicking through the warning. " +
                "The 'Proceed anyway' button on certificate error pages is a well-known social engineering vector. Phishing and adversary-in-the-middle toolkits intentionally generate self-signed certificates for lookalike domains, then display the certificate error page and add persuasive text (in custom '404' page content) asking the user to click through. Removing the bypass button eliminates this click-through vector and forces users to contact IT when they encounter legitimate certificate misconfigurations.",
            Tags = ["edge", "certificate-error", "bypass", "phishing", "mitm"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Certificate error bypass button removed; users cannot click through TLS certificate warnings.",
            ApplyOps = [RegOp.SetDword(Key, "SSLErrorOverrideAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SSLErrorOverrideAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "SSLErrorOverrideAllowed", 0)],
        },
        new TweakDef
        {
            Id = "edgect-enable-safe-browsing-phishing-protection",
            Label = "Edge Cert Transparency: Enable Safe Browsing Phishing URL Protection",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets SafeBrowsingEnabled=1 in Edge policy. Enables Microsoft Defender SmartScreen URL reputation checking for every navigation in Edge that verifies the destination URL against Microsoft's known-phishing, known-malware, and URL threat intelligence database before the page loads. " +
                "SmartScreen URL checking is Edge's first-line defence against phishing and malware distribution sites. When disabled (e.g., by a user who finds the warning pages annoying), navigations to known phishing sites proceed without warning. In enterprise environments where employees receive targeted spear-phishing emails with malicious links, SmartScreen provides automated blocking of known-bad URLs that supplements user security awareness training.",
            Tags = ["edge", "safe-browsing", "smartscreen", "phishing", "url-protection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "SmartScreen URL checking enforced in Edge; known-phishing and malware URLs blocked before page load.",
            ApplyOps = [RegOp.SetDword(Key, "SafeBrowsingEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SafeBrowsingEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "SafeBrowsingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "edgect-enable-enhanced-safe-browsing",
            Label = "Edge Cert Transparency: Enable Enhanced Safe Browsing Deep URL Analysis",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets SafeBrowsingProtectionLevel=2 in Edge policy (value 2 = Enhanced Protection). Enables Edge's Enhanced Safe Browsing mode, which performs deeper URL and download analysis including file hash lookups, URL structure analysis, and real-time page content evaluation to detect novel phishing pages that have not yet been classified in the known-bad URL database. " +
                "Standard SmartScreen uses a hash-compare against a known-bad URL blocklist. Enhanced Protection adds real-time analysis that can detect zero-day phishing pages within minutes of their creation by analysing page structure, visual similarity to known login pages, and URL entropy. This dramatically reduces the window between phishing site creation and first-user protection.",
            Tags = ["edge", "safe-browsing", "enhanced", "real-time", "zero-day"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enhanced SafeBrowsing enabled; real-time zero-day phishing detection augments standard blocklist.",
            ApplyOps = [RegOp.SetDword(Key, "SafeBrowsingProtectionLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "SafeBrowsingProtectionLevel")],
            DetectOps = [RegOp.CheckDword(Key, "SafeBrowsingProtectionLevel", 2)],
        },
        new TweakDef
        {
            Id = "edgect-block-mixed-content-display",
            Label = "Edge Cert Transparency: Block Passive Mixed Content (HTTP Resources on HTTPS Pages)",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets BlockThirdPartyCookies=0 is not the right key; sets MixedContentEnabled=0 in Edge policy. Blocks Edge from loading passive HTTP resources (images, CSS, fonts) on HTTPS pages, preventing mixed content that allows passive network observers to correlate browsing behaviour by monitoring the unencrypted resource requests. " +
                "A device on a network where traffic is monitored (public Wi-Fi, hotel network, corporate proxy with DLP) that visits an HTTPS page with HTTP subresources reveals which specific content elements were loaded via the unencrypted sub-requests. An adversary can build a browser fingerprint and activity log from passive HTTP resource patterns even without breaking the HTTPS connection itself. Blocking all mixed content enforces full HTTPS for the entire page.",
            Tags = ["edge", "mixed-content", "passive-sniffing", "https", "tls"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "HTTP resources on HTTPS pages blocked (mixed content); full page encryption enforced for all navigations.",
            ApplyOps = [RegOp.SetDword(Key, "MixedContentEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MixedContentEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "MixedContentEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgect-require-hsts-preload-for-intranet",
            Label = "Edge Cert Transparency: Enforce HTTPS-Only Mode for All Navigation",
            Category = "Edge Certificate Transparency Policy",
            Description = "Sets HttpsOnlyMode=1 in Edge policy (value 1 = Enabled). Enables Edge's HTTPS-Only mode globally, causing Edge to attempt to upgrade all HTTP navigations to HTTPS automatically, and displaying an interstitial warning if the upgrade fails (i.e., the site only supports HTTP). " +
                "HTTP navigation exposes session cookies, form data, content, and the URL path to passive interception on any network segment between the browser and the server. SSL stripping attacks (BEAST, sslstrip) intercept HTTP requests and prevent HTTPS upgrades transparently. HTTPS-Only mode forces the HTTPS upgrade before any HTTP request is ever sent, making all browser sessions resistant to trivial passive eavesdropping and SSL strip attacks.",
            Tags = ["edge", "https-only", "ssl-stripping", "hsts", "encryption"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "HTTPS-Only mode enforced in Edge; HTTP sites cause a warning interstitial before proceeding.",
            ApplyOps = [RegOp.SetDword(Key, "HttpsOnlyMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HttpsOnlyMode")],
            DetectOps = [RegOp.CheckDword(Key, "HttpsOnlyMode", 1)],
        },
    ];
}
