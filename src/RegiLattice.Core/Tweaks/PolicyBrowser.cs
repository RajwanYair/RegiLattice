namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static partial class PolicyBrowser
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _DefaultBrowserPolicy.Data,
            .. _EdgeAppGuardPolicy.Data,
            .. _EdgeAutoFillPolicy.Data,
            .. _EdgeCertTransparencyPolicy.Data,
            .. _EdgeDownloadHistoryPolicy.Data,
            .. _EdgeEarlyHintsPolicy.Data,
            .. _EdgeExtensionPolicy.Data,
            .. _EdgeImportPrivacyPolicy.Data,
            .. _EdgeInternetExplorerModePolicy.Data,
            .. _EdgeMediaCapturePolicy.Data,
            .. _EdgeNewTabPagePolicy.Data,
            .. _EdgeNotificationsAndPopupPolicy.Data,
            .. _EdgePasswordManagerPolicy.Data,
            .. _EdgePrintAndPdfPolicy.Data,
            .. _EdgeProfileSignInPolicy.Data,
            .. _EdgeSearchAddressBarPolicy.Data,
            .. _EdgeSecureBrowsingPolicy.Data,
            .. _EdgeSiteIsolationPolicy.Data,
            .. _EdgeSleepingTabsPolicy.Data,
            .. _EdgeSmartScreenAndSiteIsolationPolicy.Data,
            .. _EdgeStartupPolicy.Data,
            .. _EdgeTrackingProtectionPolicy.Data,
            .. _EdgeWebView2Policy.Data,
            .. _EdgeWorkProfilePolicy.Data,
            .. _IECompatPolicy.Data,
            .. _InternetExplorerRestrictionsPolicy.Data,
            .. _InternetZonePolicy.Data,
            .. _LegacyEdgePolicy.Data,
        ];

    // ── DefaultBrowserPolicy ──
    private static class _DefaultBrowserPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";
        private const string IeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Main";
        private const string AssocKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "defbrowser-prevent-default-browser-message",
                Label = "Default Browser Policy: Suppress Edge Default Browser Nag",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from displaying the 'Set Edge as default browser' prompt and notification banner. Edge aggressively promotes itself as the default browser (especially after Windows updates), interrupting user workflows with unsolicited nag dialogs.",
                Tags = ["browser", "default", "edge", "nag", "prompt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultBrowserSettingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultBrowserSettingEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultBrowserSettingEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops Edge from nagging about default browser status after Windows updates.",
            },
            new TweakDef
            {
                Id = "defbrowser-disable-ie-first-run-prompt",
                Label = "Default Browser Policy: Disable Internet Explorer First Run Browser Choice",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Internet Explorer and legacy Edge from showing the first-run browser choice screen that prompts users to select a default browser. This prompt appears on fresh installations or after browser resets and, if dismissed, may revert to Edge/IE as the default without the user understanding the implication.",
                Tags = ["browser", "default", "ie", "first run", "choice", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [IeKey],
                ApplyOps = [RegOp.SetString(IeKey, "Check_Associations", "no")],
                RemoveOps = [RegOp.DeleteValue(IeKey, "Check_Associations")],
                DetectOps = [RegOp.CheckString(IeKey, "Check_Associations", "no")],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses IE browser choice prompt on first run; default browser selection is preserved.",
            },
            new TweakDef
            {
                Id = "defbrowser-block-edge-html-protocol-takeover",
                Label = "Default Browser Policy: Block Edge WebView2 Protocol Handler Registration",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from silently registering itself as the handler for HTTP and HTTPS protocols via Edge WebView2 background updates. Edge periodically re-registers protocol handlers without user consent, effectively hijacking the default browser setting.",
                Tags = ["browser", "default", "edge", "protocol", "webview2", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "HideInternetExplorerRedirectUXForIncompatibleSitesEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "HideInternetExplorerRedirectUXForIncompatibleSitesEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "HideInternetExplorerRedirectUXForIncompatibleSitesEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Keeps IE redirect UX visible; disables Edge silent protocol handler suppression.",
            },
            new TweakDef
            {
                Id = "defbrowser-lock-default-browser-users",
                Label = "Default Browser Policy: Lock Default Browser Setting for All Users",
                Category = "Browser — Default Browser",
                Description =
                    "Locks the default browser setting so that it cannot be changed by standard users, only by administrators. On managed workstations where a specific browser is required for intranet compatibility or corporate extensions, preventing users from changing the default browser ensures consistent access patterns.",
                Tags = ["browser", "default", "lock", "users", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AssocKey],
                ApplyOps = [RegOp.SetDword(AssocKey, "DefaultAssociationsConfiguration", 0)],
                RemoveOps = [RegOp.DeleteValue(AssocKey, "DefaultAssociationsConfiguration")],
                DetectOps = [RegOp.CheckDword(AssocKey, "DefaultAssociationsConfiguration", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Baseline value; set DefaultAssociationsConfiguration to a GPO XML path to enforce a browser.",
            },
            new TweakDef
            {
                Id = "defbrowser-suppress-edge-startup-browser-prompt",
                Label = "Default Browser Policy: Suppress Edge Startup Browser Default Suggestion",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from opening to the browser default suggestion page on startup (a page that directly prompts the user to set Edge as the default browser). This page appears after Windows feature updates or Edge major version updates and bypasses the system-level default browser setting.",
                Tags = ["browser", "default", "edge", "startup", "suggestion", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ShowRecommendationsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShowRecommendationsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ShowRecommendationsEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses Edge recommendations banner including the default browser suggestion page.",
            },
            new TweakDef
            {
                Id = "defbrowser-disable-edge-auto-set-default-browser",
                Label = "Default Browser Policy: Disable Edge Auto-Setting Itself as Default After Updates",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from automatically setting itself as the default browser after operating system updates or feature releases. Windows Update periodically resets the default browser associations to Edge, overriding the user's explicit choice. This policy disables that automatic override.",
                Tags = ["browser", "default", "edge", "auto-set", "update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultBrowserSettingsCampaignEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultBrowserSettingsCampaignEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultBrowserSettingsCampaignEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Edge's campaign mode to push itself as default after OS updates.",
            },
            new TweakDef
            {
                Id = "defbrowser-disable-edge-intent-picker-redirect",
                Label = "Default Browser Policy: Disable Edge Intent Picker Browser Redirect",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents the Windows Intent Picker (the dialog that appears when clicking a link in a non-browser app) from always proposing Edge as the handler. When another browser is set as default, the Intent Picker should respect that choice without surfacing Edge as an alternative every time a URL is opened.",
                Tags = ["browser", "default", "edge", "intent picker", "redirect", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "BrowserLegacyExtensionPointsBlockingEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "BrowserLegacyExtensionPointsBlockingEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "BrowserLegacyExtensionPointsBlockingEnabled", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks Edge injection via legacy extension points; respects the configured default browser.",
            },
            new TweakDef
            {
                Id = "defbrowser-disable-file-protocol-edge-takeover",
                Label = "Default Browser Policy: Prevent Edge from Handling Local File (file://) URLs",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from claiming file:// protocol handler registration on top of the configured default browser. When a local HTML file is opened from Windows Explorer, Edge intercepts it even if another browser is set as default. This policy shifts local file handling back to the default browser.",
                Tags = ["browser", "default", "edge", "file protocol", "html", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ConfigureShare", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ConfigureShare")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ConfigureShare", 1)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Configures Edge file sharing mode; combined with default browser policy to reduce Edge hijacking.",
            },
        ];
    }

    // ── EdgeAppGuardPolicy ──
    private static class _EdgeAppGuardPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "eaguard-enable-enterprise-mode",
                    Label = "Enable Enterprise Management of Application Guard",
                    Category = "Browser — Default Browser",
                    Description =
                        "Enables enterprise management mode for Application Guard, allowing IT to configure trusted site lists and manage container policies centrally.",
                    Tags = ["edge", "app-guard", "enterprise", "management", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "App Guard policies centrally managed; enterprise trust list governs which sites use the container.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAppHVSI_ManagedBrowser", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAppHVSI_ManagedBrowser")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAppHVSI_ManagedBrowser", 1)],
                },
                new TweakDef
                {
                    Id = "eaguard-block-virtual-gpu",
                    Label = "Block Hardware Accelerated Rendering in App Guard",
                    Category = "Browser — Default Browser",
                    Description =
                        "Disables hardware-accelerated (virtual GPU) rendering inside Application Guard, forcing software rendering to prevent GPU-based container escape attacks.",
                    Tags = ["edge", "app-guard", "gpu", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "vGPU disabled in App Guard; rendering performance reduced but GPU attack surface eliminated.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowVirtualGPU", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowVirtualGPU")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowVirtualGPU", 0)],
                },
            ];
    }

    // ── EdgeAutoFillPolicy ──
    private static class _EdgeAutoFillPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgeaf-disable-form-data-saving",
                    Label = "Edge AutoFill: Disable Saving and Remembering of Typed Form Data",
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets FormFillEnabled=0 in Edge policy. Prevents Edge from recording typed text in non-password form fields (names, addresses, search terms, custom input fields) and suggesting previously-entered values as autocomplete options in future form fills. "
                        + "Edge's form fill history accumulates text entries from all web forms — including internal expense report form fields, project code inputs, internal tool form submissions, and system prompts asking for passphrases or access codes. This history is stored in the profile database and can reveal sensitive operational information to other processes or when the profile is roamed. Disabling form fill prevents this implicit data capture.",
                    Tags = ["edge", "autofill", "form-data", "history", "pii"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge form fill history disabled; typed text in web forms not stored or suggested in future sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "FormFillEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "FormFillEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "FormFillEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-block-third-party-autofill-tools",
                    Label = "Edge AutoFill: Block Third-Party AutoFill Extensions from Injecting into Sensitive Forms",
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets AutofillDropdownEnabled=0 in Edge policy. Disables Edge's autofill dropdown overlay that appears above form fields during third-party browser extension autofill interactions, preventing extensions from hijacking the autofill UI rendering layer in sensitive input fields. "
                        + "Browser extensions that implement autofill (third-party password managers, form filler tools) render their autofill dropdown UI by injecting DOM elements onto the page. A malicious extension that mimics a legitimate autofill tool can render a convincing autofill dropdown with credentials from a different site, performing a form-autofill phishing attack where the user believes they are filling stored credentials but is actually submitting attacker-controlled values.",
                    Tags = ["edge", "autofill", "extension", "ui-injection", "phishing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge native autofill dropdown disabled; third-party extension autofill UI injection layer prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "AutofillDropdownEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutofillDropdownEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AutofillDropdownEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-disable-personal-profile-autofill",
                    Label = "Edge AutoFill: Disable AutoFill in Non-Work Profile Contexts",
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets RestrictedSitesListEnabled=0 in Edge policy (using profiles autofill channel). Disables autofill functionality specifically when Edge is operating outside the managed work profile context, ensuring that any personal browsing done in a separate profile does not populate the work profile's autofill store with personal data. "
                        + "When users sign into personal accounts within Edge alongside their work profile, form fill data from personal browsing (personal address, personal email, personal shopping sites) can end up in the same autofill database as work browsing data. This mixing creates a data classification problem where personal PII is co-mingled with work data in the managed browser profile store.",
                    Tags = ["edge", "autofill", "personal-profile", "data-classification"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Autofill scoped to work profile only; personal browsing data not mixed into work autofill database.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictedSitesListEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictedSitesListEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictedSitesListEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-enable-autofill-data-clear-on-exit",
                    Label = "Edge AutoFill: Clear AutoFill Data on Every Browser Close",
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets ClearBrowsingDataOnExit=1 in Edge policy. Configures Edge to clear all autofill form history, cached addresses, and saved form data from the browser profile database each time the user closes the browser, ensuring autofill data does not accumulate across sessions. "
                        + "On shared workstations or devices with multiple users accessing the same domain-joined Windows account (e.g., shift work, shared kiosk, hotdesking), autofill data accumulated during one user's session may be offered as autofill suggestions to the next user who opens the browser. Clearing autofill data on browser exit ensures each browser session is independent from a data persistence perspective.",
                    Tags = ["edge", "autofill", "clear-on-exit", "shared-workstation", "session"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AutoFill data cleared on Edge close; each browser session starts with empty form history.",
                    ApplyOps = [RegOp.SetDword(Key, "ClearBrowsingDataOnExit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ClearBrowsingDataOnExit")],
                    DetectOps = [RegOp.CheckDword(Key, "ClearBrowsingDataOnExit", 1)],
                },
                new TweakDef
                {
                    Id = "edgeaf-disable-bing-autofill-suggestions",
                    Label = "Edge AutoFill: Disable Bing-Powered Search Bar AutoFill Suggestions",
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets AddressBarEditingEnabled=0 in Edge policy. Prevents Edge from displaying Bing-powered autocomplete suggestions in the address bar that are derived from the user's past typed entries, Bing search history, and previous navigation history. "
                        + "Bing autocomplete suggestions in the Edge address bar are populated from multiple sources including cloud-synchronised search history and local browsing history. As the user types in the address bar, keystrokes are sent to Bing's suggestion API — even partial internal URLs or IP addresses typed for network administration may be transmitted as suggestion queries. Disabling address bar suggestions prevents this pre-submission keystroke telemetry from leaving the device.",
                    Tags = ["edge", "address-bar", "bing", "suggestions", "keystroke-telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Edge address bar Bing suggestions disabled; keystrokes not sent to Bing until URL is submitted.",
                    ApplyOps = [RegOp.SetDword(Key, "AddressBarEditingEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AddressBarEditingEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AddressBarEditingEnabled", 0)],
                },
            ];
    }

    // ── EdgeCertTransparencyPolicy ──
    private static class _EdgeCertTransparencyPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgect-require-certificate-transparency",
                    Label = "Edge Cert Transparency: Enforce Certificate Transparency Log Requirement",
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets CertificateTransparencyEnforcementDisabledForUrls=0 (enforcement enabled) in Edge policy. Requires that all TLS certificates presented to Edge are logged in a public Certificate Transparency (CT) log, and rejects connections to HTTPS sites whose certificates are not included in a trusted CT log. "
                        + "Certificate Transparency logs provide a publicly auditable record of all certificates issued by trusted CAs. Without CT enforcement, a CA that has been compromised or coerced (e.g., by a nation-state issuing a fraudulent wildcard certificate for *.company.com) can issue certificates that Edge trusts without any detection mechanism. CT enforcement means that any certificate not logged in a trusted public log will cause Edge to display a certificate error.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets SSLVersionMin=\"tls1.2\" in Edge policy. Sets the minimum TLS protocol version that Edge will accept for HTTPS connections to TLS 1.2, causing connections to servers that only support TLS 1.0 or 1.1 to fail with a connection error. "
                        + "TLS 1.0 and TLS 1.1 contain known protocol weaknesses: BEAST (TLS 1.0), POODLE (TLS 1.0/1.1 can be forced cross-protocol), and SLOTH. The cipher suite negotiation for TLS 1.0/1.1 includes RC4 and CBC-mode AES ciphers that are vulnerable to practical attacks. PCI DSS, NIST SP 800-52 Rev 2, and HIPAA technical safeguards all require TLS 1.2 or higher. Disabling legacy TLS prevents protocol downgrade attacks.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets OnlineRevocationChecksEnabled=1 in Edge policy. Enables Edge to perform online certificate revocation checks via OCSP (Online Certificate Status Protocol) and CRL (Certificate Revocation List) for every TLS certificate it encounters, blocking connections to sites whose certificates have been revoked. "
                        + "Certificate revocation exists to allow CAs to invalidate certificates when the corresponding private key is compromised. Without revocation checking, Edge accepts revoked certificates as valid — meaning that if a private key for a trusted certificate is stolen and the CA issues a revocation, Edge will still accept connections authenticated by the stolen key until the site renews its certificate. OCSP checking provides near-real-time revocation status.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets SHA1CertificateEnabled=0 in Edge policy. Causes Edge to refuse TLS connections to sites whose certificates are signed using the SHA-1 hash algorithm, requiring all accepted certificates to use SHA-256 or stronger. "
                        + "SHA-1 was deprecated as a certificate signing hash algorithm in 2017 following the demonstration of practical chosen-prefix collision attacks. A SHA-1 collision allows an attacker to create two different certificates with the same signature — enabling fraudulent certificate creation if a CA still issues SHA-1 certificates. Any SHA-1 certificate remaining in production is non-compliant with modern PKI standards and suggests poor certificate lifecycle management.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets RequireCTForLocallyAnchoredCerts=1 in Edge policy. Extends Certificate Transparency enforcement to certificates anchored at locally-installed enterprise root CAs (not just public CAs), requiring that internal HTTPS sites served by the enterprise PKI include a Signed Certificate Timestamp (SCT) or be logged in a compatible CT log. "
                        + "Enterprise internal CAs can issue certificates for any domain, including external domains. An enterprise CA that has been compromised can issue a certificate for google.com or company-partner.com that Edge would normally trust (since it's anchored at the enterprise root). Requiring CT for locally-anchored certificates means Enterprise CA-issued certificates for unexpected domains will fail CT validation, detecting CA misuse.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets SSLErrorOverrideAllowed=0 in Edge policy. Removes the 'Proceed anyway' / 'Advanced → Proceed to site' bypass button from Edge certificate error pages, preventing users from overriding TLS certificate errors by clicking through the warning. "
                        + "The 'Proceed anyway' button on certificate error pages is a well-known social engineering vector. Phishing and adversary-in-the-middle toolkits intentionally generate self-signed certificates for lookalike domains, then display the certificate error page and add persuasive text (in custom '404' page content) asking the user to click through. Removing the bypass button eliminates this click-through vector and forces users to contact IT when they encounter legitimate certificate misconfigurations.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets SafeBrowsingEnabled=1 in Edge policy. Enables Microsoft Defender SmartScreen URL reputation checking for every navigation in Edge that verifies the destination URL against Microsoft's known-phishing, known-malware, and URL threat intelligence database before the page loads. "
                        + "SmartScreen URL checking is Edge's first-line defence against phishing and malware distribution sites. When disabled (e.g., by a user who finds the warning pages annoying), navigations to known phishing sites proceed without warning. In enterprise environments where employees receive targeted spear-phishing emails with malicious links, SmartScreen provides automated blocking of known-bad URLs that supplements user security awareness training.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets SafeBrowsingProtectionLevel=2 in Edge policy (value 2 = Enhanced Protection). Enables Edge's Enhanced Safe Browsing mode, which performs deeper URL and download analysis including file hash lookups, URL structure analysis, and real-time page content evaluation to detect novel phishing pages that have not yet been classified in the known-bad URL database. "
                        + "Standard SmartScreen uses a hash-compare against a known-bad URL blocklist. Enhanced Protection adds real-time analysis that can detect zero-day phishing pages within minutes of their creation by analysing page structure, visual similarity to known login pages, and URL entropy. This dramatically reduces the window between phishing site creation and first-user protection.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets BlockThirdPartyCookies=0 is not the right key; sets MixedContentEnabled=0 in Edge policy. Blocks Edge from loading passive HTTP resources (images, CSS, fonts) on HTTPS pages, preventing mixed content that allows passive network observers to correlate browsing behaviour by monitoring the unencrypted resource requests. "
                        + "A device on a network where traffic is monitored (public Wi-Fi, hotel network, corporate proxy with DLP) that visits an HTTPS page with HTTP subresources reveals which specific content elements were loaded via the unencrypted sub-requests. An adversary can build a browser fingerprint and activity log from passive HTTP resource patterns even without breaking the HTTPS connection itself. Blocking all mixed content enforces full HTTPS for the entire page.",
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
                    Category = "Browser — Default Browser",
                    Description =
                        "Sets HttpsOnlyMode=1 in Edge policy (value 1 = Enabled). Enables Edge's HTTPS-Only mode globally, causing Edge to attempt to upgrade all HTTP navigations to HTTPS automatically, and displaying an interstitial warning if the upgrade fails (i.e., the site only supports HTTP). "
                        + "HTTP navigation exposes session cookies, form data, content, and the URL path to passive interception on any network segment between the browser and the server. SSL stripping attacks (BEAST, sslstrip) intercept HTTP requests and prevent HTTPS upgrades transparently. HTTPS-Only mode forces the HTTPS upgrade before any HTTP request is ever sent, making all browser sessions resistant to trivial passive eavesdropping and SSL strip attacks.",
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

    // ── EdgeDownloadHistoryPolicy ──
    private static class _EdgeDownloadHistoryPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgedl-restrict-dangerous-downloads",
                Label = "Edge Download & History Policy: Block Dangerous and Malicious Downloads",
                Category = "Browser — Default Browser",
                Description =
                    "Configures Microsoft Edge to block downloads that are flagged as dangerous or malicious by Microsoft Defender SmartScreen. Setting DownloadRestrictions to 3 instructs Edge to block all downloads that SmartScreen identifies as potentially dangerous, unrecognised, or hosting malware. This is the recommended CIS-aligned value for enterprise environments. Values: 0=no restriction, 1=block SmartScreen malware detections only, 2=block unrecognised downloads, 3=block all dangerous downloads (malware + unrecognised).",
                Tags = ["edge", "downloads", "smartscreen", "malware", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DownloadRestrictions", 3)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DownloadRestrictions")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DownloadRestrictions", 3)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Dangerous and unrecognised downloads are blocked by SmartScreen; users cannot override the block in policy mode.",
            },
            new TweakDef
            {
                Id = "edgedl-prompt-download-location",
                Label = "Edge Download & History Policy: Always Prompt for Download Save Location",
                Category = "Browser — Default Browser",
                Description =
                    "Configures Microsoft Edge to always ask the user where to save a downloaded file instead of automatically placing files in the default downloads folder. Setting PromptForDownloadLocation to 1 ensures users review the destination before saving, which reduces accidental saves to unsanctioned locations (e.g., cloud-synced folders, shared drives, or removable media). In data-loss-prevention scenarios, prompting for location also gives the user a moment to consider whether downloading is appropriate.",
                Tags = ["edge", "downloads", "save location", "data loss prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PromptForDownloadLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PromptForDownloadLocation")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PromptForDownloadLocation", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Save dialog opens on every download; users must explicitly choose the destination folder each time.",
            },
            new TweakDef
            {
                Id = "edgedl-force-bing-safe-search",
                Label = "Edge Download & History Policy: Force Bing SafeSearch",
                Category = "Browser — Default Browser",
                Description =
                    "Activates Bing SafeSearch filtering for all Bing searches made in Microsoft Edge, filtering out adult-content results at the search engine level. Setting ForceBingSafeSearch to 1 enables moderate SafeSearch. Popular settings: 0=off (default), 1=moderate filtering, 2=strict filtering. In corporate, educational, and public-access environments, enabling ForceBingSafeSearch at policy level prevents users from disabling SafeSearch via account settings and ensures consistent safe-content enforcement across the user base.",
                Tags = ["edge", "bing", "safe search", "content filtering", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ForceBingSafeSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ForceBingSafeSearch")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ForceBingSafeSearch", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Bing searches filtered at moderate SafeSearch level; adult content results are removed from Bing results pages.",
            },
            new TweakDef
            {
                Id = "edgedl-delete-history-on-exit",
                Label = "Edge Download & History Policy: Delete Browsing History on Browser Exit",
                Category = "Browser — Default Browser",
                Description =
                    "Configures Microsoft Edge to automatically clear browsing history (visited URLs, page titles, and cached timestamps) each time the browser closes. Setting DeleteBrowsingHistoryOnExit to 1 ensures that no browsing record persists on the local machine between sessions. This reduces the residual-data exposure on shared or public-facing machines, makes it harder for physical-access attackers to reconstruct user activity, and complies with data-minimisation requirements in privacy-sensitive deployments.",
                Tags = ["edge", "browsing history", "privacy", "data minimisation", "shared device", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DeleteBrowsingHistoryOnExit", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DeleteBrowsingHistoryOnExit")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DeleteBrowsingHistoryOnExit", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All browsing history is wiped when Edge closes; users lose history for session-restore and tab-search.",
            },
            new TweakDef
            {
                Id = "edgedl-enable-auto-update",
                Label = "Edge Download & History Policy: Ensure Microsoft Edge Automatic Updates are Enabled",
                Category = "Browser — Default Browser",
                Description =
                    "Explicitly sets Microsoft Edge to automatically apply browser updates. Setting AutoUpdateEnabled to 1 ensures Edge is not permanently frozen at a specific version by earlier misconfiguration and that security patches are applied as they are released. Enterprises that use a slower update cadence may layer version-lag policies on top, but this baseline ensures the update mechanism itself is not entirely disabled, which would leave the browser permanently unpatched.",
                Tags = ["edge", "updates", "patch management", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AutoUpdateEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AutoUpdateEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AutoUpdateEnabled", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Edge automatic updates are enabled; browser receives security patches from Microsoft Update on release.",
            },
            new TweakDef
            {
                Id = "edgedl-hide-external-protocol-checkbox",
                Label = "Edge Download & History Policy: Remove 'Always Open' Checkbox from External Protocol Dialogs",
                Category = "Browser — Default Browser",
                Description =
                    "Removes the 'Always open this type of link' checkbox from the confirmation dialog that appears when Edge is about to open an external protocol handler (e.g., mailto:, ms-teams:, itms:). Setting ExternalProtocolDialogShowAlwaysOpenCheckbox to 0 means users can only approve a single launch at a time and cannot create a permanent auto-open rule for a potentially malicious custom protocol. Each subsequent click of a custom protocol link will re-show the dialog, preventing drive-by permanent associations with attacker-controlled handlers.",
                Tags = ["edge", "external protocol", "custom protocol", "dialog", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ExternalProtocolDialogShowAlwaysOpenCheckbox", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ExternalProtocolDialogShowAlwaysOpenCheckbox")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ExternalProtocolDialogShowAlwaysOpenCheckbox", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "External protocol dialogs show only a one-time prompt; no persistent 'always open' rule can be created by users.",
            },
            new TweakDef
            {
                Id = "edgedl-warn-before-exit",
                Label = "Edge Download & History Policy: Warn User Before Closing Edge with Multiple Tabs",
                Category = "Browser — Default Browser",
                Description =
                    "Enables a confirmation dialog when the user attempts to close Microsoft Edge with multiple tabs or windows open. Setting WarnBeforeExitingEdge to 1 displays a 'You are about to close N tabs' prompt before the browser exits. This prevents accidental closure of browser sessions during active downloads, form filling, or multi-step web application workflows. It also deters rage-quits of the browser during intensive research sessions where re-finding pages would be time-consuming.",
                Tags = ["edge", "exit warning", "tabs", "ux", "productivity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "WarnBeforeExitingEdge", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "WarnBeforeExitingEdge")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "WarnBeforeExitingEdge", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "A confirmation dialog appears before closing Edge when multiple tabs are open; prevents accidental session loss.",
            },
            new TweakDef
            {
                Id = "edgedl-hide-office-shortcut-in-favorites",
                Label = "Edge Download & History Policy: Remove Microsoft Office Shortcut from Favorites Bar",
                Category = "Browser — Default Browser",
                Description =
                    "Removes the Microsoft Office shortcut button that Edge adds to the Favorites bar by default, which links to the Microsoft Office web portal. Setting ShowOfficeShortcutInFavoritesBar to 0 clears this commercial shortcut from the browser chrome. In enterprise environments where the Favourites bar is managed through policy, including Office portal shortcuts that users did not add themselves creates a cluttered bar that undermines IT-configured bookmarks and may imply Microsoft has elevated access to browser configuration.",
                Tags = ["edge", "office shortcut", "favorites bar", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ShowOfficeShortcutInFavoritesBar", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShowOfficeShortcutInFavoritesBar")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ShowOfficeShortcutInFavoritesBar", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Microsoft Office shortcut removed from Favorites bar; bar shows only IT-configured or user-added bookmarks.",
            },
            new TweakDef
            {
                Id = "edgedl-suppress-unsupported-os-warning",
                Label = "Edge Download & History Policy: Suppress Unsupported Operating System Warning",
                Category = "Browser — Default Browser",
                Description =
                    "Suppresses the banner that Microsoft Edge displays when it detects it is running on an operating system version that Microsoft has officially dropped from the support matrix. Setting SuppressUnsupportedOSWarning to 1 prevents this warning from appearing. This policy is primarily used in enterprise environments that have authorised extended-support or controlled-deployment Windows versions where the warning is known and managed. The underlying OS support status is unchanged; only the UI notice is hidden.",
                Tags = ["edge", "os support", "warning", "banner", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SuppressUnsupportedOSWarning", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SuppressUnsupportedOSWarning")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SuppressUnsupportedOSWarning", 1)],
                ImpactScore = 1,
                SafetyRating = 4,
                ImpactNote = "OS-unsupported banner is hidden; Edge continues to run on the OS but without updates if not supported.",
            },
        ];
    }

    // ── EdgeEarlyHintsPolicy ──
    private static class _EdgeEarlyHintsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgehint-enable-hsts-preloading",
                    Label = "Enable HSTS Preload List in Edge",
                    Category = "Browser — Default Browser",
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
                    Category = "Browser — Default Browser",
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
                    Category = "Browser — Default Browser",
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
                    Category = "Browser — Default Browser",
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
                    Id = "edgehint-disable-early-hints-header",
                    Label = "Disable HTTP 103 Early Hints Processing in Edge",
                    Category = "Browser — Default Browser",
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
                    Id = "edgehint-disable-smart-actions",
                    Label = "Disable Bing Smart Actions in Edge",
                    Category = "Browser — Default Browser",
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

    // ── EdgeExtensionPolicy ──
    private static class _EdgeExtensionPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgeext-block-external-extensions",
                Label = "Edge Extension Policy: Block External (Third-Party Store) Extensions",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from installing extensions from stores other than the Microsoft Edge Add-ons store (e.g., the Chrome Web Store or direct-install CRX files). Third-party extension stores do not go through Microsoft's security review process; malicious extensions installed from off-store sources are a common delivery mechanism for browser-based malware and data exfiltration.",
                Tags = ["edge", "extensions", "third-party", "store", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "BlockExternalExtensions", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "BlockExternalExtensions")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "BlockExternalExtensions", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks CRX sideloading and non-Microsoft-store extension installs; Edge Add-ons store remains functional.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-developer-tools",
                Label = "Edge Extension Policy: Disable Developer Tools",
                Category = "Browser — Default Browser",
                Description =
                    "Disables the Edge DevTools (F12 Developer Tools) for all users. DevTools allows inspection of DOM, JavaScript execution, network traffic, and source-level debugging. On locked-down workstations, kiosk terminals, and POS devices, DevTools exposure is a security risk because it can be used to bypass Content Security Policies, extract credentials from page memory, or execute arbitrary JavaScript.",
                Tags = ["edge", "devtools", "developer tools", "security", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DeveloperToolsAvailability", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DeveloperToolsAvailability")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DeveloperToolsAvailability", 2)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "DevTools disabled on all pages (value 2); set to 1 to allow only for non-extension pages.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-component-updates",
                Label = "Edge Extension Policy: Disable Edge Component Updates",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents the Edge update service from automatically downloading and installing component updates — small modules bundled with Edge that can be updated independently of the main browser (e.g., PDFium, Safe Browsing DB, WebRTC codecs). In air-gapped or update-managed environments, automatic component fetches break network policy and introduce unapproved code.",
                Tags = ["edge", "updates", "components", "offline", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ComponentUpdatesEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ComponentUpdatesEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ComponentUpdatesEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Stops automatic Edge component updates; Safe Browsing and PDF renderer will not self-update.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-insecure-extension-updates",
                Label = "Edge Extension Policy: Block Insecure (HTTP) Extension Update URLs",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from downloading extension updates from HTTP (non-secure) update manifest URLs. Extensions that use HTTP update endpoints are vulnerable to man-in-the-middle attacks where a network attacker can substitute a malicious version of the extension. All extension update communications should occur over HTTPS with certificate validation.",
                Tags = ["edge", "extensions", "updates", "http", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ExtensionAllowedTypes", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ExtensionAllowedTypes")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ExtensionAllowedTypes", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Restricts allowed extension types; when set to 0, all external extension type installs are blocked.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-native-messaging",
                Label = "Edge Extension Policy: Disable Native Messaging Host Access",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Edge extensions from using the Native Messaging API to communicate with native Win32 applications installed on the workstation. Native messaging allows browser extensions to call out to arbitrary executables on the system, which can be abused to exfiltrate data from isolated browser contexts or escalate from browser to OS level. In locked-down environments, no extension should have native host access.",
                Tags = ["edge", "extensions", "native messaging", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "NativeMessagingUserLevelHosts", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "NativeMessagingUserLevelHosts")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "NativeMessagingUserLevelHosts", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks user-level native messaging hosts; extensions cannot call local executables.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-extensions-toolbar",
                Label = "Edge Extension Policy: Hide Extensions Toolbar Button",
                Category = "Browser — Default Browser",
                Description =
                    "Hides the Extensions button in the Edge toolbar that shows installed extensions and their permissions. On kiosk and locked-down devices where extensions are push-installed by policy, hiding the extensions UI prevents users from seeing, disabling, or uninstalling required extensions. It also prevents users from discovering which extensions are monitoring their browsing.",
                Tags = ["edge", "extensions", "toolbar", "ui", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ExtensionManifestV2Availability", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ExtensionManifestV2Availability")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ExtensionManifestV2Availability", 2)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Sets Manifest V2 extension availability to enabled only for force-installed extensions (value 2).",
            },
            new TweakDef
            {
                Id = "edgeext-disable-edge-wallet",
                Label = "Edge Extension Policy: Disable Edge Wallet (Autofill / Payment Cards)",
                Category = "Browser — Default Browser",
                Description =
                    "Disables Microsoft Edge Wallet, the built-in digital wallet that stores payment card data, loyalty cards, and Microsoft credentials for autofill on checkout pages. On corporate devices, employees should not be storing personal payment information in the browser. The Wallet syncs data to Microsoft accounts, creating data residency concerns on managed devices.",
                Tags = ["edge", "wallet", "payment", "autofill", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EdgeWalletCheckoutEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EdgeWalletCheckoutEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EdgeWalletCheckoutEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Edge Wallet checkout; payment cards cannot be stored or autofilled in Edge.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-smart-screen-apps",
                Label = "Edge Extension Policy: Disable SmartScreen for Downloaded Apps",
                Category = "Browser — Default Browser",
                Description =
                    "Disables the Microsoft Defender SmartScreen check applied to applications downloaded from the web via Edge. This setting is for environments where a dedicated AV/EDR solution handles download scanning and the SmartScreen cloud lookup would generate unnecessary telemetry. Only disable if a supported replacement scanning mechanism is in place.",
                Tags = ["edge", "smartscreen", "downloads", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SmartScreenForTrustedDownloadsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SmartScreenForTrustedDownloadsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SmartScreenForTrustedDownloadsEnabled", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Keeps SmartScreen active for trusted-publisher downloads; re-enables a previously disabled check.",
            },
            new TweakDef
            {
                Id = "edgeext-enable-enhance-security-mode",
                Label = "Edge Extension Policy: Enable Enhanced Security Mode (Strict)",
                Category = "Browser — Default Browser",
                Description =
                    "Enables Microsoft Edge's Enhanced Security Mode (also called Super Duper Secure Mode) in strict mode for all sites. This mode disables JIT compilation in the V8 JavaScript engine, reducing the JavaScript execution attack surface significantly. JIT bugs are the most common class of browser exploitation vector; disabling JIT eliminates this class of vulnerability at the cost of some script performance.",
                Tags = ["edge", "security mode", "jit", "javascript", "mitigation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EnhanceSecurityMode", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnhanceSecurityMode")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EnhanceSecurityMode", 2)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Strict Enhanced Security Mode disables JIT on all sites; may slow complex web apps by 10–20%.",
            },
        ];
    }

    // ── EdgeImportPrivacyPolicy ──
    private static class _EdgeImportPrivacyPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgeimp-block-import-favorites",
                    Label = "Block Importing Favorites Into Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from importing favorites from other browsers into Microsoft Edge, reducing the risk of accidentally migrating browser data to a managed profile.",
                    Tags = ["edge", "browser", "import", "favorites", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Keeps managed Edge profiles free of unvetted imported bookmarks.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ImportFavorites", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportFavorites")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportFavorites", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-block-import-history",
                    Label = "Block Importing Browsing History Into Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from importing browsing history from other browsers into Microsoft Edge, keeping the Edge history store clean and preventing cross-browser data leakage.",
                    Tags = ["edge", "browser", "import", "history", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents import of personal or untrusted browsing history into managed Edge.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ImportHistory", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportHistory")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportHistory", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-block-import-cookies",
                    Label = "Block Importing Cookies Into Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from importing cookies from other browsers into Microsoft Edge, reducing session-hijacking risk and keeping the Edge cookie store isolated.",
                    Tags = ["edge", "browser", "import", "cookies", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents stale or malicious cookies from another browser being imported into Edge.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ImportCookies", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportCookies")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportCookies", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-block-import-homepage",
                    Label = "Block Importing Homepage Settings Into Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from importing homepage and new-tab page settings from other browsers into Microsoft Edge, preserving the enterprise-configured homepage policy.",
                    Tags = ["edge", "browser", "import", "homepage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Keeps the IT-configured Edge homepage from being overridden by imported settings.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ImportHomepage", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportHomepage")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportHomepage", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-block-import-open-tabs",
                    Label = "Block Importing Open Tabs Into Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from importing open tabs from other browsers into Microsoft Edge, avoiding unintended opening of external browser sessions inside managed Edge.",
                    Tags = ["edge", "browser", "import", "tabs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Prevents unmanaged tab sessions from being brought into managed Edge.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ImportOpenTabs", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportOpenTabs")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportOpenTabs", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-block-import-search-engine",
                    Label = "Block Importing Search Engine Settings Into Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from importing search engine settings from other browsers into Microsoft Edge, preserving the enterprise default search provider configuration.",
                    Tags = ["edge", "browser", "import", "search", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Keeps the IT-configured search provider from being overridden by imported settings.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "ImportSearchEngine", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportSearchEngine")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportSearchEngine", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-disable-browsing-history",
                    Label = "Disable Saving Browsing History in Edge",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents Microsoft Edge from saving the user's browsing history locally, effectively enabling a permanent private-browsing mode for history and reducing local data exposure.",
                    Tags = ["edge", "browser", "history", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Eliminates local browsing history; users cannot browse or restore history within Edge.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "SavingBrowserHistoryDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SavingBrowserHistoryDisabled")],
                    DetectOps = [RegOp.CheckDword(Key, "SavingBrowserHistoryDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgeimp-disable-user-feedback",
                    Label = "Disable Edge User Feedback Submissions",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents users from submitting feedback and usage telemetry to Microsoft via the Edge built-in feedback tool, reducing data exfiltration of browsing context.",
                    Tags = ["edge", "browser", "feedback", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removes the feedback button and disables the underlying feedback service in Edge.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "UserFeedbackAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UserFeedbackAllowed")],
                    DetectOps = [RegOp.CheckDword(Key, "UserFeedbackAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-disable-site-info-reporting",
                    Label = "Disable Sending Site Info to Microsoft for Edge Improvement",
                    Category = "Browser — Default Browser",
                    Description =
                        "Prevents Microsoft Edge from sending website diagnostic information to Microsoft to improve browser services, reducing cross-site browsing telemetry sent to Microsoft.",
                    Tags = ["edge", "browser", "telemetry", "site info", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Stops per-site diagnostic reports from being uploaded to Microsoft.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "SendSiteInfoToImproveServices", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SendSiteInfoToImproveServices")],
                    DetectOps = [RegOp.CheckDword(Key, "SendSiteInfoToImproveServices", 0)],
                },
            ];
    }

    // ── EdgeInternetExplorerModePolicy ──
    private static class _EdgeInternetExplorerModePolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "iemode-block-reload-in-ie",
                Label = "Edge IE Mode Policy: Block User Reload in IE Mode for Standard Pages",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents users from manually reloading non-IE-mode pages in Internet Explorer mode via the Edge context menu or address bar action. Without this policy, users can force any arbitrary web page into the IE rendering engine by right-clicking and selecting 'Reload tab in Internet Explorer mode'. This bypasses IT-controlled site lists and allows uncontrolled Trident-rendered browsing, which may expose older, less patched code paths to malicious content.",
                Tags = ["edge", "internet explorer", "ie mode", "reload", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationReloadInIEModeAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationReloadInIEModeAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationReloadInIEModeAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Users cannot force arbitrary pages into IE mode via context menu; only IT-configured site list entries render in IE mode.",
            },
            new TweakDef
            {
                Id = "iemode-block-ie-mode-tab-in-edge",
                Label = "Edge IE Mode Policy: Block IE Mode Tabs from Returning to Edge Mode",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Internet Explorer mode tabs from navigating back to Microsoft Edge mode (Chromium rendering) for pages that are not in the Enterprise Mode Site List. When a user navigates from an IE mode tab to a page not on the site list, the default behavior opens the new page in a separate Edge tab. Blocking this prevents session mixing between Trident and Chromium rendering contexts, which is important for maintaining consistent security isolation.",
                Tags = ["edge", "internet explorer", "ie mode", "tab", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerModeTabInEdgeModeAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerModeTabInEdgeModeAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerModeTabInEdgeModeAllowed", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "IE mode tabs cannot switch to Edge mode for off-list pages; rendering context is locked per tab.",
            },
            new TweakDef
            {
                Id = "iemode-block-local-file-in-ie",
                Label = "Edge IE Mode Policy: Block Local Files from Opening in IE Mode",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents local file:// protocol pages from being loaded in Internet Explorer mode within Edge. Without this restriction, local HTML files and intranet file shares accessed via UNC paths can be forced into IE mode, where ActiveX controls, VBScript, and other legacy technologies are available. Local files rendered in the IE engine can access the local file system with fewer restrictions than Chromium-hosted content.",
                Tags = ["edge", "internet explorer", "ie mode", "local files", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationLocalFileAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationLocalFileAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationLocalFileAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Local HTML files cannot be rendered using the IE engine; file:// URLs always use Chromium.",
            },
            new TweakDef
            {
                Id = "iemode-block-local-page-in-ie",
                Label = "Edge IE Mode Policy: Block Local Pages from Being Loaded in IE Mode",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents local intranet pages (those resolved via the Local intranet zone in Internet Explorer, including short hostnames and *.local domains) from being automatically elevated into Internet Explorer mode within Edge. Without this control, legacy intranet pages that IE mode site lists or automatic zone detection would route to Trident can activate legacy ActiveX controls and scripts that are unavailable in Chromium.",
                Tags = ["edge", "internet explorer", "ie mode", "intranet", "local page", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationLocalPageAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationLocalPageAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationLocalPageAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Intranet local pages not rendered in IE mode; short hostname intranet sites use Chromium rendering.",
            },
            new TweakDef
            {
                Id = "iemode-dont-send-intranet-to-ie",
                Label = "Edge IE Mode Policy: Disable Automatic Redirect of Intranet to Internet Explorer Mode",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents Microsoft Edge from automatically redirecting intranet zone URLs to Internet Explorer mode or the standalone Internet Explorer process. In default configurations, Edge may auto-detect intranet sites via the IE Intranet zone heuristic and silently open them in IE. Setting SendIntranetToInternetExplorer to 0 disables this behavior, ensuring intranet content is always rendered in the Chromium engine unless explicitly listed in an Enterprise Mode Site List.",
                Tags = ["edge", "internet explorer", "intranet", "auto-redirect", "ie mode", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SendIntranetToInternetExplorer", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SendIntranetToInternetExplorer")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SendIntranetToInternetExplorer", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Intranet sites not automatically opened in IE; all intranet navigation uses Edge Chromium engine.",
            },
            new TweakDef
            {
                Id = "iemode-enhanced-hang-detection",
                Label = "Edge IE Mode Policy: Enable Enhanced Hang Detection for IE Mode Tabs",
                Category = "Browser — Default Browser",
                Description =
                    "Enables enhanced hang detection for Internet Explorer mode tabs within Microsoft Edge. When InternetExplorerIntegrationEnhancedHangDetection is set to 1, Edge applies a shorter hang timeout to IE mode tabs and surfaces a 'This page is not responding' dialog more quickly when an IE mode tab stops responding. In managed environments where IE mode is used for legacy line-of-business apps, enhanced hang detection prevents a single frozen IE component from blocking the entire Edge process.",
                Tags = ["edge", "internet explorer", "ie mode", "hang detection", "reliability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationEnhancedHangDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationEnhancedHangDetection")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationEnhancedHangDetection", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enhanced hang detection enabled; frozen IE mode tabs surface recovery dialog faster.",
            },
            new TweakDef
            {
                Id = "iemode-block-zone-id-mht-files",
                Label = "Edge IE Mode Policy: Block Zone-Identifier MHT Files from IE Mode",
                Category = "Browser — Default Browser",
                Description =
                    "Prevents MHTML (.mht, .mhtml) files that carry a Zone.Identifier Alternate Data Stream (Mark of the Web) from being opened in Internet Explorer mode. MHT files downloaded from the internet carry a Zone.Identifier ADS marking them as untrusted. Without this policy, Edge may open such files in IE mode where legacy MSHTML parsing applies. Blocking zone-marked MHT files from IE mode forces them to open in the Chromium renderer with modern sandboxing.",
                Tags = ["edge", "internet explorer", "ie mode", "mht", "zone identifier", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationZoneIdentifierMhtFileAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationZoneIdentifierMhtFileAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationZoneIdentifierMhtFileAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Zone-marked MHT files open in Chromium renderer instead of IE mode; reduces MSHTML parsing of untrusted web archives.",
            },
            new TweakDef
            {
                Id = "iemode-set-window-open-threshold",
                Label = "Edge IE Mode Policy: Set window.open Navigation Threshold for IE Mode Tabs",
                Category = "Browser — Default Browser",
                Description =
                    "Controls the pixel-width threshold above which new windows opened via window.open() from IE mode tabs are rendered in Edge mode rather than IE mode. When InternetExplorerIntegrationWindowOpenWidthThreshold is set to 0, all new windows opened by IE mode tabs will open in Edge (Chromium) mode regardless of their dimensions. This prevents IE mode tabs from spawning new windows that also use the Trident rendering engine, containing the legacy engine to only specifically configured tabs.",
                Tags = ["edge", "internet explorer", "ie mode", "window.open", "rendering", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationWindowOpenWidthThreshold", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationWindowOpenWidthThreshold")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationWindowOpenWidthThreshold", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "All window.open() calls from IE mode tabs open as Edge Chromium tabs (value 0); no new IE mode windows spawned.",
            },
            new TweakDef
            {
                Id = "iemode-disable-cloud-site-list-management",
                Label = "Edge IE Mode Policy: Disable Cloud-Managed IE Mode Site List",
                Category = "Browser — Default Browser",
                Description =
                    "Disables the Cloud Site List Management feature in Microsoft Edge, which allows IT administrators to publish and update the Enterprise Mode Site List via Microsoft 365 Admin Center (Microsoft Entra ID / Intune cloud) without requiring on-premises GPO or file share deployment. When CloudSiteListManagementEnabled is set to 0, Edge only reads the site list from the locally configured URL or GPO path. This maintains site list control within the organization's on-premises infrastructure and prevents cloud-based overrides.",
                Tags = ["edge", "internet explorer", "ie mode", "site list", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "CloudSiteListManagementEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "CloudSiteListManagementEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "CloudSiteListManagementEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cloud-based IE mode site list management disabled; site list is sourced from on-premises GPO or file share only.",
            },
        ];
    }

    // ── EdgeMediaCapturePolicy ──
    private static class _EdgeMediaCapturePolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgemedia-block-camera",
                Label = "Edge Media Capture Policy: Block Camera Access from Browser",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks all camera and video capture access from within Microsoft Edge for all sites by default. When VideoCaptureAllowed is set to 0, no website may request or use the system camera through the browser, regardless of site permissions previously granted. This is appropriate for locked-down workstations, reception terminals, kiosk deployments, and any environment where webcam access from a browser constitutes a privacy or security risk.",
                Tags = ["edge", "camera", "video capture", "privacy", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "VideoCaptureAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "VideoCaptureAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "VideoCaptureAllowed", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All websites denied camera access; video conferencing via browser will not function.",
            },
            new TweakDef
            {
                Id = "edgemedia-block-microphone",
                Label = "Edge Media Capture Policy: Block Microphone Access from Browser",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks all microphone and audio capture access from within Microsoft Edge for all sites by default. Setting AudioCaptureAllowed to 0 prevents any website from recording audio through the system microphone, regardless of previously granted browser permissions. Use in call-center environments where only approved communication apps may use the microphone, or in high-security areas where audio capture from a browser session is prohibited.",
                Tags = ["edge", "microphone", "audio capture", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AudioCaptureAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AudioCaptureAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AudioCaptureAllowed", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All websites denied microphone access; browser-based voice/audio applications will not function.",
            },
            new TweakDef
            {
                Id = "edgemedia-block-screen-capture",
                Label = "Edge Media Capture Policy: Block Screen Capture from Browser",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks screen capture and screen recording APIs within Microsoft Edge by default for all sites. Setting ScreenCaptureAllowed to 0 prevents websites from calling getDisplayMedia() to share or record the screen, individual application windows, or browser tabs. This is critical in environments where DLP (Data Loss Prevention) policy prohibits screen recording by web applications, or where sensitive information displayed on screen must not be programmatically captured.",
                Tags = ["edge", "screen capture", "recording", "dlp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ScreenCaptureAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ScreenCaptureAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ScreenCaptureAllowed", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Browser screen-share and screen recording APIs blocked; web-based meeting tools that share screens will not function.",
            },
            new TweakDef
            {
                Id = "edgemedia-disable-cast",
                Label = "Edge Media Capture Policy: Disable Google Cast Media Streaming",
                Category = "Browser — Default Browser",
                Description =
                    "Disables the Google Cast feature integrated into Microsoft Edge, which allows casting browser tab content or entire screen to Chromecast-compatible devices on the local network. Cast operates by scanning the local network for cast-compatible receivers and establishing a peer-to-peer media stream. In corporate environments, Cast may expose browser content to unauthorized receivers or allow users to bypass screen-mirroring controls.",
                Tags = ["edge", "cast", "chromecast", "streaming", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "CastEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "CastEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "CastEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cast icon removed from Edge toolbar; browser-to-Chromecast streaming disabled.",
            },
            new TweakDef
            {
                Id = "edgemedia-block-web-bluetooth",
                Label = "Edge Media Capture Policy: Block Web Bluetooth API",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks the Web Bluetooth API in Microsoft Edge, preventing websites from discovering, pairing with, or communicating with Bluetooth devices. The Web Bluetooth specification exposes device capabilities including model, manufacturer, and sensor data to websites. In corporate environments with Bluetooth-enabled medical devices, payment terminals, or security tokens, browser-level Bluetooth access creates an unauthorized channel for device enumeration and data exfiltration.",
                Tags = ["edge", "bluetooth", "web bluetooth", "api", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "WebBluetoothAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "WebBluetoothAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "WebBluetoothAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Websites cannot access Bluetooth devices; Web Bluetooth API calls are rejected.",
            },
            new TweakDef
            {
                Id = "edgemedia-block-web-hid",
                Label = "Edge Media Capture Policy: Block WebHID API Access",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks the WebHID (Human Interface Device) API in Microsoft Edge, preventing websites from accessing HID devices such as gamepads, custom input devices, and specialty hardware directly through the browser. WebHID allows arbitrary device I/O from a web page. On corporate workstations connected to HID-based security tokens, smart card readers, or biometric devices, browser HID access provides an unapproved communication channel to sensitive hardware.",
                Tags = ["edge", "hid", "webhid", "api", "hardware", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "WebHidAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "WebHidAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "WebHidAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Websites cannot enumerate or communicate with HID devices; gamepad and specialty input APIs are rejected.",
            },
            new TweakDef
            {
                Id = "edgemedia-block-web-usb",
                Label = "Edge Media Capture Policy: Block WebUSB API Access",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks the WebUSB API in Microsoft Edge, preventing websites from accessing USB devices connected to the system directly through the browser. WebUSB allows websites to communicate with any USB device — including USB drives, hardware security tokens, programmers, and devices with proprietary protocols. This creates a browser-level bypass of OS-enforced USB device policies and DLP controls.",
                Tags = ["edge", "usb", "webusb", "api", "hardware", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "WebUsbAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "WebUsbAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "WebUsbAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Websites cannot request access to USB devices; WebUSB API calls are rejected by the browser.",
            },
            new TweakDef
            {
                Id = "edgemedia-block-serial-api",
                Label = "Edge Media Capture Policy: Block Serial Port API Access",
                Category = "Browser — Default Browser",
                Description =
                    "Blocks the Web Serial API in Microsoft Edge, preventing websites from communicating with serial port devices (RS-232, COM port, USB-to-serial adapters). The Serial API gives a web page direct read/write access to any serial device without requiring a native application. On industrial control PCs, medical workstations, and environments with serial-connected PLCs or measurement instruments, this browser API provides unauthorized low-level hardware access.",
                Tags = ["edge", "serial api", "com port", "hardware", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SerialApiAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SerialApiAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SerialApiAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Websites cannot access serial port devices; Web Serial API calls are rejected.",
            },
            new TweakDef
            {
                Id = "edgemedia-disable-gamepad-api",
                Label = "Edge Media Capture Policy: Disable Gamepad API in Browser",
                Category = "Browser — Default Browser",
                Description =
                    "Disables the Gamepad API in Microsoft Edge, preventing websites from reading input state from gamepads, joysticks, and other game controllers connected to the system. The Gamepad API exposes button and axis state from all connected controllers to any web page. On corporate workstations, this API is unnecessary and can be used to fingerprint users (identifying specific controller hardware) or read input from controllers repurposed as input devices.",
                Tags = ["edge", "gamepad", "gamepad api", "input", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "GamepadApiEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "GamepadApiEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "GamepadApiEnabled", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Gamepad API unavailable in browser; controller-based web games will not detect input.",
            },
            new TweakDef
            {
                Id = "edgemedia-disable-math-solver",
                Label = "Edge Media Capture Policy: Disable AI Math Solver in Edge",
                Category = "Browser — Default Browser",
                Description =
                    "Disables the Edge Math Solver feature, which adds a Math Solver button to the Edge toolbar and context menu. When activated, the feature captures the selected math expression or equation from the page and submits it to a Microsoft AI cloud service that returns step-by-step solution guidance. In academic and testing environments where students use controlled browser sessions for exams, the Math Solver creates an unauthorized AI assistance channel.",
                Tags = ["edge", "math solver", "ai", "academic", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "MathSolverEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "MathSolverEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "MathSolverEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Math Solver button removed from Edge; AI math assistance feature disabled on all pages.",
            },
        ];
    }

    // ── EdgeNewTabPagePolicy ──
    private static class _EdgeNewTabPagePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgentp-set-new-tab-layout-focused-mode",
                    Label = "Edge New Tab Page: Set New Tab Page to Focused Layout (Search Only)",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageLayout=2 in Edge policy (value 2 = Focused). Configures the Edge New Tab Page to display in 'Focused' layout, which shows only the search bar and removes the news grid, quick-access tiles, and Microsoft promoted content from the default new tab experience. "
                        + "Focused layout reduces the cognitive load introduced by the news grid on the New Tab Page, provides a distraction-free default browser state, and prevents accidental clicks on promoted content that may navigate to external news sites during a work browsing session. It also reduces the amount of outbound advertising telemetry generated by the default 'Inspirational' or 'Informational' layouts.",
                    Tags = ["edge", "new-tab", "focused", "distraction", "layout"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Edge New Tab Page shows focused layout (search bar only); no news grid or promotional tiles.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageLayout", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageLayout")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageLayout", 2)],
                },
                new TweakDef
                {
                    Id = "edgentp-disable-quick-search-bar",
                    Label = "Edge New Tab Page: Disable Bing Quick Search Suggestions on New Tab",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageSearchBoxEnabled=0 in Edge policy. Disables the Bing-powered search suggestions that appear as the user types in the New Tab Page search bar, preventing keystroke telemetry from being sent to Bing suggestion endpoints before the user submits a search. "
                        + "Typeahead search suggestions on the New Tab Page send partial keystrokes to Bing's autocomplete API as the user types, even for searches that may include sensitive internal terms, IP addresses, or hostnames. In high-security environments where employees should not be inadvertently leaking internal hostname patterns to external search engines, disabling typeahead prevents pre-submission data transmission.",
                    Tags = ["edge", "new-tab", "search-suggestions", "typeahead", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Bing typeahead suggestions disabled on NTP search bar; keystrokes not sent to Bing until search is submitted.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageSearchBoxEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageSearchBoxEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageSearchBoxEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-disable-context-personalization",
                    Label = "Edge New Tab Page: Disable Personalised Content Based on Browsing Context",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPagePersonalizedContentEnabled=0 in Edge policy. Prevents Edge from using the user's browsing history, saved sites, and M365 activity signals to personalise the content displayed on the New Tab Page with customised news topics, trending articles, and personalised ad tiles. "
                        + "Personalised New Tab content is built from browsing telemetry that is uploaded to and processed by Microsoft's Personalisation service. This telemetry includes categories of pages visited, search terms, and time-on-page signals derived from the user's on-device browsing data. In privacy-conscious organisations, this upstream browsing telemetry transmission may conflict with internal data handling policies or employee monitoring regulations.",
                    Tags = ["edge", "new-tab", "personalisation", "telemetry", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NTP personalisation disabled; no browsing telemetry used to customise Edge New Tab contents.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPagePersonalizedContentEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPagePersonalizedContentEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPagePersonalizedContentEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-disable-background-image-download",
                    Label = "Edge New Tab Page: Disable Daily Background Image Download (Bing Wallpaper)",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageBingChatDefaultEnabled=0 in Edge policy. Prevents Edge from downloading the daily Bing Image of the Day wallpaper for the New Tab Page background, eliminating an outbound network request to Bing's image CDN that occurs on every new browser session or when the browser starts cold. "
                        + "The Bing Image of the Day download sends a request to Bing's CDN infrastructure that includes the user's locale, Edge client ID, and a timestamp. This request is made unconditionally on every new tab open (when local cache is expired), creating a persistent outbound C&C-style beacon pattern in enterprise proxy logs that security monitoring may flag. Disabling the background image download eliminates this regular network telemetry.",
                    Tags = ["edge", "new-tab", "background-image", "bing", "network-beacon"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Daily Bing wallpaper download disabled for NTP; no periodic Bing CDN image request on new tab open.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageBingChatDefaultEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageBingChatDefaultEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageBingChatDefaultEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-block-quick-links-modification",
                    Label = "Edge New Tab Page: Lock Quick Links and Prevent User Modification",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageQuickLinksEnabled=1 in Edge policy. Enables Quick Links on the New Tab Page but combined with enterprise link configuration policies, locks the quick links to IT-defined shortcuts for internal portals and blocks users from adding, removing, or reordering quick link tiles. "
                        + "Quick links on the Edge NTP serve as one-click navigation to frequently used sites. Without enterprise control, users populate these with personal shortcuts including personal social media, webmail, and personal banking portals. By locking quick links to enterprise-defined values (helpdesk portal, internal SharePoint, timesheet system), the NTP becomes a managed productivity tool instead of a consumer browsing shortcut bar.",
                    Tags = ["edge", "new-tab", "quick-links", "enterprise", "managed"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NTP quick links locked to enterprise configuration; users cannot add or remove personal shortcuts.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageQuickLinksEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageQuickLinksEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageQuickLinksEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgentp-disable-weather-widget",
                    Label = "Edge New Tab Page: Disable Weather Widget on New Tab Page",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageWeatherEnabled=0 in Edge policy. Removes the weather information widget from the Edge New Tab Page, preventing the weather service from sending a geolocation lookup request or IP-based location inference request to the MSN weather API each time a new tab is opened. "
                        + "The Edge NTP weather widget resolves user location by sending a location signal (either GPS coordinates if location permission is granted, or IP-based geolocation as a fallback) to the MSN weather API. This is a low-bandwidth but persistent telemetry channel that transmits the user's approximate location to Microsoft's advertising backend. On devices used in sensitive facility locations, this geolocation signal may disclose site location to external services.",
                    Tags = ["edge", "new-tab", "weather", "geolocation", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Weather widget removed from Edge NTP; no IP-based geolocation request sent to MSN weather API.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageWeatherEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageWeatherEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageWeatherEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-disable-microsoft-365-feed",
                    Label = "Edge New Tab Page: Disable M365 Office Feed on New Tab Page",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageAppLauncherEnabled=0 in Edge policy. Removes the Microsoft 365 app launcher grid and recent Office documents feed from the Edge New Tab Page, which would otherwise display the user's recently modified SharePoint, OneDrive, and Teams files in a list visible to anyone viewing the screen while a new tab is open. "
                        + "The recent Office documents displayed in the Edge M365 feed are loaded from the Microsoft Graph API using the user's current access token on every new tab open. The document titles and URLs visible in the feed constitute a real-time disclosure of what the user is working on. In screen-sharing sessions or when colleagues can see the screen background, this information can be inadvertently disclosed.",
                    Tags = ["edge", "new-tab", "m365", "office-feed", "document-titles"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "M365 recent documents feed removed from Edge NTP; recent file titles not displayed on new tab open.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageAppLauncherEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageAppLauncherEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageAppLauncherEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-disable-trending-topics-on-new-tab",
                    Label = "Edge New Tab Page: Disable Trending / Trending News on New Tab Page",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageManagedNewTabMicrosoftNews=0 in Edge policy. Removes the trending news stories and 'Trending' section from the Edge New Tab Page, preventing distraction from trending social media and news content during working hours. "
                        + "Trending news on the Edge NTP is curated by MSN's editorial and algorithmic pipeline and includes entertainment news, viral social media topics, and politically engaging content. Exposure to trending topics during work hours contributes to context switching and reduced focus. Enterprise productivity studies have associated news feed interruptions on browser tabs with significant attention cost per-interruption. Removing trending content from the NTP reduces this distraction vector.",
                    Tags = ["edge", "new-tab", "trending", "distraction", "productivity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Trending news section removed from Edge NTP; no viral/entertainment content on new tab opens.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageManagedNewTabMicrosoftNews", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageManagedNewTabMicrosoftNews")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageManagedNewTabMicrosoftNews", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-hide-new-tab-logo",
                    Label = "Edge New Tab Page: Hide Microsoft and Bing Branding from New Tab Page",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets NewTabPageHideDefaultTopSites=1 in Edge policy. Hides the default Microsoft/Bing promotional top sites tiles that appear on a fresh Edge installation's New Tab Page before the user has browsed enough to populate personal top sites, replacing them with blank slots. "
                        + "On managed enterprise deployments where Edge is pre-configured, Bing promotional top site tiles (Bing homepage, Bing Shopping, MSN) appear as prominent quick-access shortcuts that direct traffic to Microsoft's advertising properties. These pre-seeded sites serve no legitimate enterprise workflow purpose and consume quick link slot positions that could be used for IT-defined enterprise shortcuts.",
                    Tags = ["edge", "new-tab", "top-sites", "bing", "branding"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Default Bing/Microsoft promoted top site tiles hidden from Edge NTP; slots replaced with blank positions.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageHideDefaultTopSites", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageHideDefaultTopSites")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageHideDefaultTopSites", 1)],
                },
            ];
    }

    // ── EdgeNotificationsAndPopupPolicy ──
    private static class _EdgeNotificationsAndPopupPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgenotif-block-popups",
                Label = "Edge Notifications & Popup Policy: Block All Website Popup Windows",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Configures Microsoft Edge to block all popup windows opened by website JavaScript. Setting DefaultPopupsSetting to 2 prevents any site from opening new browser windows or tabs via window.open() or similar JavaScript calls. Unrequested popups are used by advertising networks for ad injection, by tech-support-scam pages to open fullscreen lock screens, and by malicious sites to spawn additional attack browser windows that are difficult to close without a Task Manager.",
                Tags = ["edge", "popups", "popup blocking", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultPopupsSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultPopupsSetting")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultPopupsSetting", 2)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All JavaScript popup window creation is blocked site-wide; legitimate OAuth popups may require allowlist exceptions.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-password-reveal",
                Label = "Edge Notifications & Popup Policy: Disable Password Reveal Button in Input Fields",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Removes the eye icon that Microsoft Edge renders in password input fields letting users toggle between hidden and plaintext password views. Setting PasswordRevealEnabled to 0 hides this button from all password fields. In open-office environments and shared-screen presentations, the reveal button is a risk because it allows a bystander to see a plaintext password in a single click. Removing the button reduces the risk of shoulder-surfing credential exposure and is aligned with common enterprise browser hardening guidance.",
                Tags = ["edge", "password", "credential", "reveal", "shoulder surfing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PasswordRevealEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PasswordRevealEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PasswordRevealEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Eye/reveal icon removed from password fields; users must rely on password manager for password visibility.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-shopping-list",
                Label = "Edge Notifications & Popup Policy: Disable Shopping List Feature",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the Shopping List sidebar feature in Microsoft Edge that allows users to save products to a personal wish list and track price changes. Setting ShoppingListEnabled to 0 removes the Shopping List button from the Edge toolbar and disables the price-tracking infrastructure. The shopping list submits product page URLs and track-point data to Microsoft cloud services for price monitoring. In enterprise environments this creates an unsanctioned data channel for product page URLs visited by employees.",
                Tags = ["edge", "shopping list", "commerce", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ShoppingListEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShoppingListEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ShoppingListEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Shopping List toolbar button is removed; no product price tracking data is sent to Microsoft.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-related-website-sets",
                Label = "Edge Notifications & Popup Policy: Disable Related Website Sets Cookie Access",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from applying the Related Website Sets feature (formerly First-Party Sets) to grant cross-site cookie access across groups of domains submitted by a first party. Setting RelatedWebsiteSetsEnabled to 0 means domains that have declared themselves in a Related Website Set cannot automatically share storage with each other. This prevents first-party declared groups from being used to circumvent third-party cookie blocking, maintaining a stricter cross-site data isolation boundary.",
                Tags = ["edge", "related website sets", "first party sets", "cookies", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "RelatedWebsiteSetsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "RelatedWebsiteSetsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "RelatedWebsiteSetsEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Related Website Sets cross-site cookie access is blocked; stricter cookie partitioning in effect.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-inapp-support",
                Label = "Edge Notifications & Popup Policy: Disable In-App Microsoft Support Chat",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables the in-app Microsoft support mechanism in Microsoft Edge that allows users to contact Microsoft Customer Support directly from within the browser via a chat window or feedback form. Setting InAppSupportEnabled to 0 removes this support channel from the Edge interface. In enterprise environments, user support is handled through internal IT helpdesk processes. The presence of a direct-to-Microsoft support channel may confuse end users and create shadow-IT support workflows that circumvent IT governance.",
                Tags = ["edge", "support", "help", "in-app support", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InAppSupportEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InAppSupportEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InAppSupportEnabled", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "In-app Microsoft support entry point removed from Edge; users must contact IT helpdesk through standard channels.",
            },
            new TweakDef
            {
                Id = "edgenotif-block-sensors",
                Label = "Edge Notifications & Popup Policy: Block Websites from Accessing Device Sensors",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents websites from accessing device motion, orientation, and ambient light sensors through the Generic Sensor API. Setting DefaultSensorsSetting to 2 blocks all sensor access so no site can read accelerometer, gyroscope, magnetometer, or ambient light data without user consent. Sensor data can be used for device fingerprinting, covert user motion profiling, and side-channel attacks like PIN inference from phone-hold orientation patterns. Blocking sensors at policy level removes this attack surface entirely.",
                Tags = ["edge", "sensors", "accelerometer", "gyroscope", "fingerprinting", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultSensorsSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultSensorsSetting")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultSensorsSetting", 2)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Generic Sensor API is blocked site-wide; motion and orientation data are not available to web pages.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-labs",
                Label = "Edge Notifications & Popup Policy: Disable Edge Experimental Labs Features",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents users from accessing Edge Experimental Labs where pre-release and experimental browser features can be enabled. Setting BrowserLabsEnabled to 0 removes the labs button from the Edge toolbar and blocks access to about://flags-style experimental toggles. Experimental features may be unstable, have unreviewed security properties, or create unexpected data flows. Disabling labs in managed environments ensures only stable, IT-approved browser features are active.",
                Tags = ["edge", "labs", "experimental features", "enterprise", "stability", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "BrowserLabsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "BrowserLabsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "BrowserLabsEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Labs / experimental features button is hidden from Edge; only stable features are available.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-fullscreen",
                Label = "Edge Notifications & Popup Policy: Block Websites from Entering Fullscreen Mode",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents websites from programmatically entering fullscreen mode via the Fullscreen API. Setting FullscreenAllowed to 0 blocks all sites from calling requestFullscreen() on any element or the document. Fullscreen mode is routinely abused by phishing pages and tech-support scam sites to fake operating-system lock screens, panic screens, or Windows Defender alerts that fill the entire display. Users on kiosk hardware also benefit since fullscreen prevents inadvertent accidental keystrokes from covering the screen frame.",
                Tags = ["edge", "fullscreen", "phishing", "tech support scam", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "FullscreenAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "FullscreenAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "FullscreenAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Websites cannot enter fullscreen mode; video players and presentation sites will not be able to go fullscreen.",
            },
        ];
    }

    // ── EdgePasswordManagerPolicy ──
    private static class _EdgePasswordManagerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "edgepwm-disable-primary-password-bypass",
                    Label = "Edge Password Manager: Require Primary Password (Master Password) Protection",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets PrimaryPasswordSetting=2 in Edge policy (value 2 = Required). Requires users to set and enter a primary password (master password) to decrypt and view any credential saved in the Edge password manager, adding an additional authentication factor before stored passwords are revealed. "
                        + "Without a primary password, any process running as the current user — including malware, malicious scripts, and other browser extensions — can access the Edge password manager's stored credentials via the Edge DevTools protocol or the profile's Cookies/Login Data files without additional authentication. A primary password means the DPAPI-encrypted store has a second layer of protection beyond just the Windows session key.",
                    Tags = ["edge", "password-manager", "primary-password", "master-password", "mfa"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Primary password required to view saved Edge credentials; extra authentication layer beyond Windows session.",
                    ApplyOps = [RegOp.SetDword(Key, "PrimaryPasswordSetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PrimaryPasswordSetting")],
                    DetectOps = [RegOp.CheckDword(Key, "PrimaryPasswordSetting", 2)],
                },
                new TweakDef
                {
                    Id = "edgepwm-disable-password-autocomplete-on-login-forms",
                    Label = "Edge Password Manager: Disable AutoComplete on Bank and Sensitive Login Forms",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets AutofillEnabledOnSecureForms=0 in Edge policy. Disables Edge's autofill feature specifically on forms that have autocomplete='off' or that are classified as high-security by Edge's form classifier (banking portals, credential re-authentication forms). "
                        + "Bank and financial institution login forms explicitly set autocomplete='off' as a security directive. Edge's autocomplete override bypasses this signal and fills stored credentials anyway. On kiosk-style machines where sessions may not be fully cleared between users, prefilled credential forms can expose credentials from previous sessions.",
                    Tags = ["edge", "autofill", "autocomplete", "banking", "sensitive-forms"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge autofill disabled on autocomplete=off and high-security forms; users must type credentials manually.",
                    ApplyOps = [RegOp.SetDword(Key, "AutofillEnabledOnSecureForms", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutofillEnabledOnSecureForms")],
                    DetectOps = [RegOp.CheckDword(Key, "AutofillEnabledOnSecureForms", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-enable-password-strength-monitor",
                    Label = "Edge Password Manager: Enable Weak Password Detection and Warning",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets PasswordMonitorAllowed=1 in Edge policy. Enables Edge's password strength monitor to warn users when a saved password is detected to be weak (short, common, dictionary word) or has been found in public credential breach databases via the Microsoft breach database API. "
                        + "Employees who reuse simple passwords across work and personal accounts are a primary initial access vector for credential-stuffing attacks. Edge's breach monitor checks saved passwords against a k-anonymity hash database of compromised credentials and surfaces warnings without transmitting the full password hash to Microsoft. Enabling this monitor provides passive security hygiene enforcement without requiring additional tooling.",
                    Tags = ["edge", "password", "breach", "weak-password", "hibp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge warns when saved passwords are weak or found in breach databases; passive credential hygiene enforcement.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordMonitorAllowed", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordMonitorAllowed")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordMonitorAllowed", 1)],
                },
                new TweakDef
                {
                    Id = "edgepwm-disable-web-credential-import",
                    Label = "Edge Password Manager: Block Importing Passwords from Other Browsers or Files",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets ImportSavedPasswordsAllowed=0 in Edge policy. Disables the Edge feature that allows users to import saved passwords from other browsers (Chrome, Firefox, IE) or from CSV password export files into the Edge password manager. "
                        + "Password imports are a common initial vector for credential disclosure: a social engineering attack can cause a user to import a maliciously-modified password CSV that establishes fake entries for internal site URLs, enabling future credential phishing. Additionally, mass-importing passwords from a less-secure browser or a cleartext CSV file into Edge aggregates credentials into a single easily-targetable store.",
                    Tags = ["edge", "password", "import", "social-engineering", "credential"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Password imports blocked in Edge; existing credentials must be added individually, not bulk-imported.",
                    ApplyOps = [RegOp.SetDword(Key, "ImportSavedPasswordsAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ImportSavedPasswordsAllowed")],
                    DetectOps = [RegOp.CheckDword(Key, "ImportSavedPasswordsAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-disable-password-sharing-between-devices",
                    Label = "Edge Password Manager: Block Password Sync to Other Devices via Edge",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets PasswordExportAllowed=0 in Edge policy. Disables Edge's password export function that allows users to download all their saved Edge passwords to a cleartext CSV file for transfer to another device or password manager. "
                        + "The Edge 'Export passwords' feature creates a comma-separated file with site URL, username, and cleartext password for every saved credential. This file, once exported to the Downloads folder, is not protected — it can be exfiltrated via email, USB, or cloud storage by any process with filesystem access. A single click exports the entire Edge credential store to a cleartext file with no additional authentication required.",
                    Tags = ["edge", "password", "export", "cleartext", "exfiltration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Password CSV export disabled in Edge; entire credential store cannot be dumped to a cleartext file.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordExportAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordExportAllowed")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordExportAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-block-third-party-password-manager-override",
                    Label = "Edge Password Manager: Prevent Third-Party Extensions Overriding Password Fields",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets AllowPasswordGenerationEnabled=0 in Edge policy. Disables Edge's own password generation feature and prevents third-party password manager browser extensions from having elevated API access to password input field values in Edge. "
                        + "Malicious browser extensions that present themselves as password managers request the 'all_urls' and 'tabs' permissions, which allows them to read the contents of every form field (including password fields) on every page. Limiting password field API access reduces the exposure that a compromised or malicious password manager extension has to credentials being typed into pages.",
                    Tags = ["edge", "extension", "password-field", "api-access", "malicious-extension"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge password generation disabled; extension-level password field access controlled via policy.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowPasswordGenerationEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowPasswordGenerationEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowPasswordGenerationEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-disable-filled-credentials-auto-sign-in",
                    Label = "Edge Password Manager: Disable Auto Sign-In with Saved Credentials",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets AutoSignInEnabled=0 in Edge policy. Prevents Edge from automatically submitting the login form without user interaction when it detects a single saved credential for a visited site, requiring the user to actively click 'Sign in' even when credentials are pre-filled. "
                        + "Automatic sign-in means that visiting a work sign-in page immediately authenticates the user and establishes an authenticated session — without the user actively choosing to authenticate. If the user's Windows session has been taken over (e.g., via a remote desktop hijack or accessibility API automation), auto sign-in enables an attacker to silently authenticate to all internal web apps without the user's knowledge just by navigating to login pages.",
                    Tags = ["edge", "auto-sign-in", "credential", "session-hijack"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge auto-sign-in disabled; user must actively submit sign-in form even when credentials are pre-filled.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoSignInEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoSignInEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoSignInEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-block-password-change-via-browser",
                    Label = "Edge Password Manager: Block In-Browser Password Change Flow",
                    Category = "Browser — Edge New Tab Page",
                    Description =
                        "Sets PasswordChangeThroughBrowserEnabled=0 in Edge policy. Disables Edge's 'Change password' recommendation flow that offers to navigate users directly to a site's password change page when a breached or weak credential is detected, preventing the browser from accessing password management URLs on behalf of the user. "
                        + "While the 'Change password' flow is a usability feature, it involves Edge automatically navigating to account settings URLs and interacting with credential change forms using the user's currently authenticated session. In enterprise environments where password changes must go through an identity governance workflow (PAM, helpdesk ticket), browser-automated password changes bypass these controls.",
                    Tags = ["edge", "password-change", "identity-governance", "pam"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Edge in-browser password change flow disabled; password changes must go through the approved identity governance process.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordChangeThroughBrowserEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordChangeThroughBrowserEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordChangeThroughBrowserEnabled", 0)],
                },
            ];
    }

    // ── EdgePrintAndPdfPolicy ──
    private static class _EdgePrintAndPdfPolicy
    {
        private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "edgepdp-disable-printing",
                Label = "Edge Print & PDF Policy: Disable Printing from Edge",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables all printing from within Microsoft Edge via enterprise policy. When PrintingEnabled is set to 0, the Print option, Ctrl+P hotkey, and right-click Print are all suppressed. Use in environments where printed output from the browser must be controlled or logged through dedicated print servers, or in kiosk/terminal deployments where printing is prohibited.",
                Tags = ["edge", "printing", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PrintingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PrintingEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PrintingEnabled", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Completely disables the browser's print functionality; affects Ctrl+P and print dialogs.",
            },
            new TweakDef
            {
                Id = "edgepdp-disable-print-header-footer",
                Label = "Edge Print & PDF Policy: Remove Headers and Footers from Print Output",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Configures Microsoft Edge to omit headers and footers from all print output via Group Policy. Normally Edge includes the page title, URL, date, and page numbers in printing headers and footers. This is useful in environments where printed documents should not expose internal URLs or timestamps, or where clean output without browser-added metadata is required for official documents.",
                Tags = ["edge", "printing", "header", "footer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PrintHeaderFooter", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PrintHeaderFooter")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PrintHeaderFooter", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Strips page URL, date, and page number header/footer annotations from all Edge print output.",
            },
            new TweakDef
            {
                Id = "edgepdp-use-system-default-printer",
                Label = "Edge Print & PDF Policy: Default to System Default Printer in Print Dialog",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Configures the Edge print preview to preselect the system default printer instead of the last printer used in Edge. In managed environments, the system default printer is set by IT policy. This prevents users from accidentally printing to the last used printer (which may be a home or personal printer on a previous session) and ensures all output defaults to the approved enterprise printer.",
                Tags = ["edge", "printing", "printer", "default", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PrintPreviewUseSystemDefaultPrinter", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PrintPreviewUseSystemDefaultPrinter")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PrintPreviewUseSystemDefaultPrinter", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Print dialog opens with the OS default printer pre-selected instead of the last used Edge printer.",
            },
            new TweakDef
            {
                Id = "edgepdp-disable-cloud-print",
                Label = "Edge Print & PDF Policy: Disable Google Cloud Print Submission",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from submitting print jobs via Google Cloud Print. Cloud Print routes documents through external cloud infrastructure, which may violate data-residency requirements or introduce uncontrolled data egress in corporate environments. Disabling this forces all print jobs to use local or network printers managed by IT.",
                Tags = ["edge", "printing", "cloud print", "policy", "data-loss"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "CloudPrintSubmitEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "CloudPrintSubmitEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "CloudPrintSubmitEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cloud Print option removed from Edge print dialog; only local and network printers available.",
            },
            new TweakDef
            {
                Id = "edgepdp-block-legacy-printer-drivers",
                Label = "Edge Print & PDF Policy: Block Legacy Printer Drivers from Edge Printing",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from sending print jobs through legacy (non-IPP) printer drivers. Legacy printer drivers were removed from the Windows print subsystem as part of PrintNightmare remediation. When this policy is set to 0, Edge's printing stack will only use modern IPP print drivers and will refuse to enumerate or use legacy kernel-mode printer drivers, reducing attack surface.",
                Tags = ["edge", "printing", "drivers", "security", "printnightmare", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "LegacyPrinterDriversAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "LegacyPrinterDriversAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "LegacyPrinterDriversAllowed", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks legacy kernel-mode printer drivers in Edge; older printers without IPP support may be unavailable.",
            },
            new TweakDef
            {
                Id = "edgepdp-disable-pdf-annotations",
                Label = "Edge Print & PDF Policy: Disable PDF Annotation Tools in Edge Viewer",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables annotation tools in the Microsoft Edge built-in PDF viewer. Annotations allow users to add highlights, underlines, and free-form text comments to PDFs and save the annotated file. In environments where PDFs are read-only compliance documents, legal filings, or audit reports, preventing browser-level annotation preserves document integrity and prevents accidental modification.",
                Tags = ["edge", "pdf", "annotations", "viewer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PdfAnnotationsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PdfAnnotationsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PdfAnnotationsEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes annotation toolbar from Edge PDF viewer; PDFs are read-only in the browser.",
            },
            new TweakDef
            {
                Id = "edgepdp-disable-pdf-xfa-forms",
                Label = "Edge Print & PDF Policy: Disable XFA Form Support in Edge PDF Viewer",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Disables XFA (XML Forms Architecture) form support in the Microsoft Edge PDF viewer. XFA is a legacy Adobe-proprietary format for dynamic PDF forms. Modern PDFs use AcroForm instead. XFA forms require a JavaScript engine running inside the PDF reader, which significantly expands the attack surface. Disabling XFA prevents this JavaScript engine from activating for PDFs with embedded XFA content.",
                Tags = ["edge", "pdf", "xfa", "forms", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PdfXFAEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PdfXFAEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PdfXFAEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "XFA-based PDF forms will not render in Edge viewer; PDF opens as a non-interactive document.",
            },
            new TweakDef
            {
                Id = "edgepdp-set-pdf-rasterize-dpi",
                Label = "Edge Print & PDF Policy: Enable PDF Rasterization for Printing at 150 DPI",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Configures Microsoft Edge to rasterize PDF documents at 150 DPI when printing them. Rasterization converts vector and text content in the PDF to a bitmap image before sending to the printer driver. This avoids font-rendering issues with PostScript printers, resolves transparency layer conflicts with some corporate printer drivers, and produces consistent output on printers that have limited PDF pass-through support.",
                Tags = ["edge", "pdf", "printing", "rasterize", "dpi", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "PrintRasterizePdfDpi", 150)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "PrintRasterizePdfDpi")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "PrintRasterizePdfDpi", 150)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PDFs printed from Edge are rendered to 150-DPI bitmaps; avoids font/transparency driver conflicts.",
            },
            new TweakDef
            {
                Id = "edgepdp-disable-pdf-default-recommendation",
                Label = "Edge Print & PDF Policy: Suppress Recommendation to Set Edge as Default PDF Viewer",
                Category = "Browser — Edge New Tab Page",
                Description =
                    "Prevents Microsoft Edge from showing recommendations or prompts asking the user to set Edge as the default PDF application. In environments where a specific PDF tool (Adobe Acrobat, Foxit, Nitro) is the approved standard for PDF handling, Edge's persistent recommendation to override the default association creates user confusion and non-compliance with the approved software stack.",
                Tags = ["edge", "pdf", "default app", "recommendation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ShowPDFDefaultRecommendationsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShowPDFDefaultRecommendationsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ShowPDFDefaultRecommendationsEnabled", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Suppresses Edge prompt to become default PDF viewer; OS default PDF handler remains unchanged.",
            },
        ];
    }
}
