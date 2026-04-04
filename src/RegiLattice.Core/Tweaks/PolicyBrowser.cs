namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from PolicyBrowser.cs ──
// RegiLattice.Core — Tweaks/PolicyBrowser.cs
// Microsoft Edge, Internet Explorer compatibility, and web browser enterprise policies
// Category: "Browser Policy"
// Consolidated from 28 modules.

internal static class PolicyBrowser
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
            new TweakDef
            {
                Id = "defbrowser-suppress-os-feature-update-browser-reset",
                Label = "Default Browser Policy: Preserve Default Browser Across Feature Updates",
                Category = "Browser",
                Description =
                    "Ensures that the default browser association is preserved across Windows feature updates. Major Windows releases (e.g., 22H2 -> 23H2) frequently reset per-user file and protocol associations to their default (Edge) values. This policy marker ensures the default browser is locked and not reset by the OS upgrade process.",
                Tags = ["browser", "default", "update", "preserve", "association", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AssocKey],
                ApplyOps = [RegOp.SetDword(AssocKey, "EnableSmartScreen", 0)],
                RemoveOps = [RegOp.DeleteValue(AssocKey, "EnableSmartScreen")],
                DetectOps = [RegOp.CheckDword(AssocKey, "EnableSmartScreen", 0)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables SmartScreen system filter; use only if SmartScreen is managed via another policy.",
            },
            new TweakDef
            {
                Id = "defbrowser-disable-edge-side-panel-web",
                Label = "Default Browser Policy: Disable Edge Side Panel Web Content",
                Category = "Browser",
                Description =
                    "Prevents the Microsoft Edge side panel (Bing Chat, Shopping, etc.) from loading web content in Explorer and third-party applications. The Edge side panel can activate and pull in browser content within non-browser windows when Edge is installed even if it is not the default browser.",
                Tags = ["browser", "edge", "side panel", "web content", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "HubsSidebarEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "HubsSidebarEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "HubsSidebarEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Edge side panel (Bing Chat, Shopping, Tools) from appearing in any context.",
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
                    Id = "eaguard-enable-app-guard",
                    Label = "Enable Application Guard for Edge",
                    Category = "Browser",
                    Description =
                        "Enables Microsoft Defender Application Guard for Microsoft Edge, isolating untrusted web sessions in a hardware-based Hyper-V container.",
                    Tags = ["edge", "app-guard", "isolation", "hyper-v", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Untrusted sites opened in isolated Hyper-V containers; requires Hyper-V and Windows Pro/Enterprise.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAppHVSI_ProviderSet", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAppHVSI_ProviderSet")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAppHVSI_ProviderSet", 1)],
                },
                new TweakDef
                {
                    Id = "eaguard-block-clipboard-host-to-container",
                    Label = "Block Clipboard Copy Host → App Guard Container",
                    Category = "Browser",
                    Description =
                        "Blocks clipboard copy operations from the host to the Application Guard container, preventing data injection into the isolated browsing session.",
                    Tags = ["edge", "app-guard", "clipboard", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cannot paste host clipboard content into App Guard session; data enters container via other means only.",
                    ApplyOps = [RegOp.SetDword(Key, "AppHVSIClipboardSettings", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIClipboardSettings")],
                    DetectOps = [RegOp.CheckDword(Key, "AppHVSIClipboardSettings", 0)],
                },
                new TweakDef
                {
                    Id = "eaguard-block-printing",
                    Label = "Block Printing from Application Guard Sessions",
                    Category = "Browser",
                    Description =
                        "Prevents users from printing from within Application Guard container sessions, blocking data exfiltration via printed documents from isolated browsing sessions.",
                    Tags = ["edge", "app-guard", "printing", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Printing from App Guard session blocked; all print operations must occur from trusted host context.",
                    ApplyOps = [RegOp.SetDword(Key, "AppHVSIPrintingSettings", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIPrintingSettings")],
                    DetectOps = [RegOp.CheckDword(Key, "AppHVSIPrintingSettings", 0)],
                },
                new TweakDef
                {
                    Id = "eaguard-block-data-persistence",
                    Label = "Block Data Persistence in Application Guard",
                    Category = "Browser",
                    Description =
                        "Disables data persistence in Application Guard containers so all browsing data (cookies, cached files, saved passwords) is discarded when the container terminates.",
                    Tags = ["edge", "app-guard", "persistence", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Container data wiped on session close; no session data survives for attackers to access.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowPersistence", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowPersistence")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowPersistence", 0)],
                },
                new TweakDef
                {
                    Id = "eaguard-block-camera-mic",
                    Label = "Block Camera and Microphone in App Guard",
                    Category = "Browser",
                    Description =
                        "Blocks camera and microphone access for Application Guard container sessions, preventing untrusted web content from capturing audio or video from host hardware.",
                    Tags = ["edge", "app-guard", "camera", "microphone", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Camera and mic blocked in isolated sessions; media capture by untrusted sites impossible.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCameraMicrophoneRedirection", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCameraMicrophoneRedirection")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCameraMicrophoneRedirection", 0)],
                },
                new TweakDef
                {
                    Id = "eaguard-enable-enterprise-mode",
                    Label = "Enable Enterprise Management of Application Guard",
                    Category = "Browser",
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
                    Category = "Browser",
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
                new TweakDef
                {
                    Id = "eaguard-block-download-host",
                    Label = "Block Downloads from App Guard to Host",
                    Category = "Browser",
                    Description =
                        "Prevents files downloaded inside the Application Guard container from being saved or transferred to the host file system, preventing malware delivery via trusted-site download.",
                    Tags = ["edge", "app-guard", "download", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Container downloads stay isolated; files cannot be moved from App Guard to host.",
                    ApplyOps = [RegOp.SetDword(Key, "SaveFilesToHost", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SaveFilesToHost")],
                    DetectOps = [RegOp.CheckDword(Key, "SaveFilesToHost", 0)],
                },
                new TweakDef
                {
                    Id = "eaguard-enable-audit-events",
                    Label = "Enable Application Guard Audit Events",
                    Category = "Browser",
                    Description =
                        "Enables audit event logging for Application Guard operations, recording container creation, clipboard events, and policy violations to the event log.",
                    Tags = ["edge", "app-guard", "audit", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "App Guard events logged; security monitoring and forensics improve.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditApplicationGuard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditApplicationGuard")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditApplicationGuard", 1)],
                },
                new TweakDef
                {
                    Id = "eaguard-enforce-network-isolation",
                    Label = "Enforce Network Isolation in Application Guard",
                    Category = "Browser",
                    Description =
                        "Enforces strict network isolation on Application Guard containers, ensuring they can only access the internet and cannot reach internal corporate network resources.",
                    Tags = ["edge", "app-guard", "network-isolation", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "App Guard containers isolated from intranet; compromised sessions cannot pivot to internal resources.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNonEnterpriseContent", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNonEnterpriseContent")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNonEnterpriseContent", 1)],
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
                    Id = "edgeaf-disable-address-autofill",
                    Label = "Edge AutoFill: Disable AutoFill for Address and Contact Information",
                    Category = "Browser",
                    Description =
                        "Sets AutofillAddressEnabled=0 in Edge policy. Prevents Edge from storing, suggesting, or filling home/work addresses, phone numbers, and contact details in web forms using the browser's autofill profile database. "
                        + "Autofill address data is stored in the Edge browser profile directory, which is located within the user's Windows profile. Any process running as the current user can read the profile's 'Web Data' SQLite database and extract all stored addresses and phone numbers in cleartext. Disabling address autofill eliminates this persisted PII from the browser profile, reducing the blast radius of a browser profile data theft.",
                    Tags = ["edge", "autofill", "address", "pii", "profile"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge address autofill disabled; no home/work addresses or phone numbers stored in browser profile.",
                    ApplyOps = [RegOp.SetDword(Key, "AutofillAddressEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutofillAddressEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AutofillAddressEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-disable-credit-card-autofill",
                    Label = "Edge AutoFill: Disable AutoFill for Payment / Credit Card Information",
                    Category = "Browser",
                    Description =
                        "Sets AutofillCreditCardEnabled=0 in Edge policy. Prevents Edge from storing, offering to save, or automatically filling credit card numbers, expiry dates, and CVV codes in payment forms using the browser's payment autofill database. "
                        + "Credit card numbers stored in the Edge autofill database persist in the browser profile's 'Web Data' file. The file is encrypted at rest using Windows DPAPI, but DPAPI decryption requires only the user's active session context — no additional PIN or authentication. Any script or process running as the user can request DPAPI decryption of the autofill database and recover stored card numbers. Enterprise browsers should never store payment card data.",
                    Tags = ["edge", "autofill", "credit-card", "payment", "pci"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Edge credit card autofill disabled; no card numbers stored in browser profile database.",
                    ApplyOps = [RegOp.SetDword(Key, "AutofillCreditCardEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutofillCreditCardEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AutofillCreditCardEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-disable-form-data-saving",
                    Label = "Edge AutoFill: Disable Saving and Remembering of Typed Form Data",
                    Category = "Browser",
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
                    Id = "edgeaf-disable-payment-request-api",
                    Label = "Edge AutoFill: Disable Web Payment Request API Access",
                    Category = "Browser",
                    Description =
                        "Sets PaymentMethodQueryEnabled=0 in Edge policy. Disables the Payment Request API in Edge, preventing web pages from programmatically querying whether the user has saved payment methods in Edge and from triggering the Payment Request UX when initiated by JavaScript. "
                        + "The Payment Request API allows a web page to enumerate available payment methods and trigger a native payment UI sheet. Malicious or compromised retail web pages can abuse this API to detect whether a user has credit card data stored in Edge, serving this information as a targeting signal for subsequent social engineering attacks. Disabling the API prevents this enumeration and blocks web-initiated payment flows entirely.",
                    Tags = ["edge", "payment-request-api", "payment", "enumeration", "web-api"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Payment Request API disabled; web pages cannot query payment methods or trigger native payment UX in Edge.",
                    ApplyOps = [RegOp.SetDword(Key, "PaymentMethodQueryEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PaymentMethodQueryEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "PaymentMethodQueryEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-disable-browser-sign-in-for-autofill",
                    Label = "Edge AutoFill: Disable Cross-Device AutoFill Sync via Browser Sign-In",
                    Category = "Browser",
                    Description =
                        "Sets BrowserSignin=0 in Edge policy. Prevents Edge from syncing saved autofill data (addresses, form data, payment methods) to other devices via the user's Microsoft account, keeping browser autofill data isolated to the current managed device. "
                        + "Edge browser sync transfers autofill data — including typed form history and saved addresses — to Microsoft's sync service and then to all other devices where the user is signed into Edge with the same Microsoft account. Personal devices may not have DLP, antivirus, or endpoint protection policies. Synced autofill data that originates from work browsing (containing internal site form inputs) may leak to a personal device with weaker security controls.",
                    Tags = ["edge", "sync", "autofill", "cross-device", "data-residency"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge autofill sync disabled; form data and addresses stay on the local managed device only.",
                    ApplyOps = [RegOp.SetDword(Key, "BrowserSignin", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BrowserSignin")],
                    DetectOps = [RegOp.CheckDword(Key, "BrowserSignin", 0)],
                },
                new TweakDef
                {
                    Id = "edgeaf-block-third-party-autofill-tools",
                    Label = "Edge AutoFill: Block Third-Party AutoFill Extensions from Injecting into Sensitive Forms",
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                new TweakDef
                {
                    Id = "edgeaf-disable-coupon-and-shopping-autofill",
                    Label = "Edge AutoFill: Disable Shopping / Coupon AutoFill and Price Comparison",
                    Category = "Browser",
                    Description =
                        "Sets EdgeShoppingAssistantEnabled=0 in Edge policy. Disables Edge's built-in shopping assistant that automatically detects product pages, suggests coupons, compares prices, and activates cashback offers, preventing these features from transmitting purchase intent signals and retail browsing patterns to Microsoft's shopping backend. "
                        + "The Edge shopping assistant monitors every page visit and performs URL classification to detect retail product pages in real time. This classification sends a request to Microsoft's shopping API containing the page URL and product context for every retail page visited. In regulated industries (healthcare, finance), browsing on retail product pages that may correlate with personal spending habits constitutes PII data that should not be transmitted to external advertising-adjacent services.",
                    Tags = ["edge", "shopping", "coupon", "price-comparison", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Edge shopping assistant and price comparison disabled; retail page visits not reported to Microsoft shopping API.",
                    ApplyOps = [RegOp.SetDword(Key, "EdgeShoppingAssistantEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EdgeShoppingAssistantEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "EdgeShoppingAssistantEnabled", 0)],
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgedl-disable-media-router",
                Label = "Edge Download & History Policy: Disable Cast (Media Router) Feature",
                Category = "Browser",
                Description =
                    "Disables the Cast/Media Router infrastructure in Microsoft Edge that discovers nearby Chromecast and Miracast display devices and allows browser tabs or media to be cast to them wirelessly. Setting EnableMediaRouter to 0 removes the Cast button from the Edge toolbar and prevents network scanning for cast targets. Cast device discovery sends mDNS probe packets to the local network, creating unsolicited network traffic and potentially leaking device identity information to local network listeners.",
                Tags = ["edge", "cast", "media router", "chromecast", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EnableMediaRouter", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnableMediaRouter")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EnableMediaRouter", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cast button and media router removed from Edge; tab or video casting to display devices is unavailable.",
            },
            new TweakDef
            {
                Id = "edgedl-enable-auto-update",
                Label = "Edge Download & History Policy: Ensure Microsoft Edge Automatic Updates are Enabled",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgeext-disable-edge-shopping-assistant",
                Label = "Edge Extension Policy: Disable Edge Shopping and Price Comparison Assistant",
                Category = "Browser",
                Description =
                    "Disables the Microsoft Edge built-in shopping assistant that automatically activates on e-commerce websites to show price comparisons, coupons, and cashback offers from partner retailers. The shopping assistant shares product browsing data with Microsoft partner networks. Many organizations prohibit this data sharing on corporate devices, especially in financial and healthcare sectors.",
                Tags = ["edge", "shopping", "coupons", "privacy", "assistant", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EdgeShoppingAssistantEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EdgeShoppingAssistantEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EdgeShoppingAssistantEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables shopping assistant and coupons feature; browsing data is not shared with partner retailers.",
            },
            new TweakDef
            {
                Id = "edgeext-disable-edge-wallet",
                Label = "Edge Extension Policy: Disable Edge Wallet (Autofill / Payment Cards)",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Id = "edgeimp-prevent-ssl-bypass",
                    Label = "Prevent Users From Bypassing SSL Certificate Errors in Edge",
                    Category = "Browser",
                    Description =
                        "Disables the 'Proceed anyway' option on SSL certificate error pages in Microsoft Edge, forcing users to stop when a certificate warning fires instead of bypassing it.",
                    Tags = ["edge", "browser", "ssl", "tls", "certificate", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents MITM bypass via self-signed cert acceptance; may block access to internal sites with bad certs.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "SSLErrorOverrideAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SSLErrorOverrideAllowed")],
                    DetectOps = [RegOp.CheckDword(Key, "SSLErrorOverrideAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "edgeimp-disable-site-info-reporting",
                    Label = "Disable Sending Site Info to Microsoft for Edge Improvement",
                    Category = "Browser",
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
                Id = "iemode-disable-ie-integration",
                Label = "Edge IE Mode Policy: Disable Internet Explorer Integration Mode",
                Category = "Browser",
                Description =
                    "Configures Microsoft Edge to disable Internet Explorer integration mode entirely. Setting InternetExplorerIntegrationLevel to 0 means Edge will not render any pages in the Internet Explorer rendering engine (Trident/MSHTML), even if those pages match an Enterprise Mode Site List. IE mode is a legacy compatibility shunt that activates the deprecated IE11 engine inside Edge. Disabling it forces all web content through the modern Chromium renderer and eliminates the IE11 attack surface.",
                Tags = ["edge", "internet explorer", "ie mode", "legacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "InternetExplorerIntegrationLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "InternetExplorerIntegrationLevel")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "InternetExplorerIntegrationLevel", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "IE mode tab rendering disabled; legacy intranet apps requiring Trident/MSHTML will no longer render in Edge.",
            },
            new TweakDef
            {
                Id = "iemode-block-reload-in-ie",
                Label = "Edge IE Mode Policy: Block User Reload in IE Mode for Standard Pages",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Id = "edgentp-disable-news-feed-on-new-tab",
                    Label = "Edge New Tab Page: Disable Microsoft News Feed and Sponsored Content",
                    Category = "Browser",
                    Description =
                        "Sets NewTabPageContentEnabled=0 in Edge policy. Removes the news feed, sponsored content tiles, and 'Microsoft Start' MSN content from the Edge New Tab Page, leaving only the search bar and customisable quick-access shortcuts. "
                        + "The Microsoft News feed on the Edge New Tab Page makes network requests to news CDN endpoints on every new tab open, sending the user's browsing context and telemetry to the MSN/Microsoft Start advertising network. Every new tab opened invites a network round-trip that may be captured by enterprise proxy logs as an apparent outbound data transfer. Removing the feed eliminates this unwanted telemetry and reduces browser startup time.",
                    Tags = ["edge", "new-tab", "news-feed", "telemetry", "msn"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge New Tab news feed disabled; no MSN/Microsoft Start content requests on new tab open.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPageContentEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPageContentEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPageContentEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgentp-set-new-tab-layout-focused-mode",
                    Label = "Edge New Tab Page: Set New Tab Page to Focused Layout (Search Only)",
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Id = "edgenotif-block-notifications",
                Label = "Edge Notifications & Popup Policy: Block All Web Push Notifications",
                Category = "Browser",
                Description =
                    "Configures Microsoft Edge to block all websites from sending push notification requests to the user. Setting DefaultNotificationsSetting to 2 means no site can prompt the user for notification permission and no notifications are delivered via the Web Notifications API. Web push notifications are a significant phishing and social engineering vector, used by malicious sites to display alarming security messages, fake prize alerts, or to maintain persistent contact with the browser even when the site tab is closed.",
                Tags = ["edge", "notifications", "push notifications", "web push", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultNotificationsSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultNotificationsSetting")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultNotificationsSetting", 2)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All websites are blocked from requesting notification permission; zero push notifications delivered by the browser.",
            },
            new TweakDef
            {
                Id = "edgenotif-block-popups",
                Label = "Edge Notifications & Popup Policy: Block All Website Popup Windows",
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgenotif-block-geolocation",
                Label = "Edge Notifications & Popup Policy: Block Websites from Accessing Geolocation",
                Category = "Browser",
                Description =
                    "Prevents any website from requesting or obtaining the user's physical location through the browser Geolocation API. Setting DefaultGeolocationSetting to 2 globally blocks geolocation permission so no site can query GPS/Wi-Fi/cell tower location. Geolocation can be combined with other telemetry to build precise user-movement profiles. Blocking it at the browser policy level prevents accidental or silent location sharing through map pages, check-in apps, and advertising networks.",
                Tags = ["edge", "geolocation", "location", "privacy", "gps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultGeolocationSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultGeolocationSetting")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultGeolocationSetting", 2)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All websites are denied geolocation access; map and location-based web apps may require manual site exceptions.",
            },
            new TweakDef
            {
                Id = "edgenotif-disable-shopping-list",
                Label = "Edge Notifications & Popup Policy: Disable Shopping List Feature",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Id = "edgepwm-disable-built-in-password-manager",
                    Label = "Edge Password Manager: Disable Edge's Built-In Password Save Prompts",
                    Category = "Browser",
                    Description =
                        "Sets PasswordManagerEnabled=0 in Edge policy. Disables the Edge built-in password manager's offer to save new credentials, preventing Edge from storing work account passwords in the browser's local credential store. "
                        + "The Edge password manager stores credentials in a file encrypted with the Windows DPAPI (Data Protection API) encryption key, which is bound to the user's Windows login credentials. If an unprivileged process on the same machine gains access to the browser's LocalState file (e.g., via a malicious script running as the same user), it can request DPAPI decryption of the stored passwords without any additional authentication, recovering plaintext credentials for all saved sites.",
                    Tags = ["edge", "password-manager", "credential-storage", "dpapi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Edge will not offer to save passwords; users must use an approved enterprise password manager instead.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordManagerEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordManagerEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordManagerEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-disable-password-reveal-button",
                    Label = "Edge Password Manager: Disable Show-Password Reveal Button in Input Fields",
                    Category = "Browser",
                    Description =
                        "Sets PasswordRevealEnabled=0 in Edge policy. Removes the 'eye' icon reveal button that appears in password input fields in Edge, preventing users from visually revealing the entered password text in a password field. "
                        + "The password reveal button, while intended for usability, is a security risk in shared workspace environments: a screen-sharing session (Teams, Zoom, remote support) that shows the browser window while a user is entering a password could inadvertently reveal the masked password text if the user or a collaborator clicks the reveal button. Disabling the reveal button removes this inadvertent exposure channel.",
                    Tags = ["edge", "password", "reveal", "screen-sharing", "shoulder-surfing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Password reveal button removed from Edge password fields; entered passwords cannot be un-masked by button click.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordRevealEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordRevealEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordRevealEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgepwm-disable-primary-password-bypass",
                    Label = "Edge Password Manager: Require Primary Password (Master Password) Protection",
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgepdp-open-pdf-externally",
                Label = "Edge Print & PDF Policy: Open PDF Files with External Application",
                Category = "Browser",
                Description =
                    "Forces Microsoft Edge to open PDF files using the operating system's default PDF application (e.g., Adobe Acrobat Reader, Foxit) instead of the built-in Edge PDF viewer. This is useful in environments where the standard PDF tool provides additional features such as digital signature validation, form filling, or DRM support that the browser's viewer does not provide.",
                Tags = ["edge", "pdf", "viewer", "external", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "AlwaysOpenPdfExternally", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "AlwaysOpenPdfExternally")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "AlwaysOpenPdfExternally", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PDF files downloaded via Edge are opened with the OS default PDF app instead of the built-in viewer.",
            },
            new TweakDef
            {
                Id = "edgepdp-disable-pdf-annotations",
                Label = "Edge Print & PDF Policy: Disable PDF Annotation Tools in Edge Viewer",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgesrch-disable-search-suggest",
                Label = "Edge Search & Address Bar Policy: Disable Address Bar Search Suggestions",
                Category = "Browser",
                Description =
                    "Prevents Microsoft Edge from displaying predictive search suggestions in the address bar as the user types. Search suggestions are sent keystroke-by-keystroke to the configured search provider (typically Bing). Disabling this eliminates real-time data leakage of partially typed URLs and search queries to Microsoft or third-party search engines over the network.",
                Tags = ["edge", "search", "suggestions", "address bar", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SearchSuggestEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SearchSuggestEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SearchSuggestEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "No search suggestions appear in the address bar; each search is only submitted when Enter is pressed.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-bing-address-bar-provider",
                Label = "Edge Search & Address Bar Policy: Remove Microsoft Search in Bing from Address Bar",
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgesrch-disable-network-prediction",
                Label = "Edge Search & Address Bar Policy: Disable Network Prediction and Prefetch",
                Category = "Browser",
                Description =
                    "Disables all network prediction and DNS prefetching in Microsoft Edge. With prediction enabled (the default), Edge pre-resolves DNS and pre-connects to the likely destinations of links visible on the page and the current address bar entry, even before the user clicks. This pre-warming creates network connections to destinations the user has never explicitly visited, which violates strict outbound traffic controls and generates noise in network monitoring.",
                Tags = ["edge", "network prediction", "prefetch", "dns", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "NetworkPredictionOptions", 2)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "NetworkPredictionOptions")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "NetworkPredictionOptions", 2)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Value 2 disables DNS prefetch and TCP preconnect; may slightly increase navigation time on first-visit pages.",
            },
            new TweakDef
            {
                Id = "edgesrch-disable-dns-interception-check",
                Label = "Edge Search & Address Bar Policy: Disable DNS Interception Detection",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Id = "edgiso-enable-site-isolation",
                    Label = "Enable Full Site Isolation in Edge",
                    Category = "Browser",
                    Description =
                        "Enables full Site Isolation in Microsoft Edge, running every site origin in a dedicated renderer process to mitigate Spectre/Meltdown cross-origin data leaks.",
                    Tags = ["edge", "site-isolation", "security", "spectre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Each site runs in isolated renderer; memory use increases but Spectre-class leaks are effectively blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "SitePerProcess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SitePerProcess")],
                    DetectOps = [RegOp.CheckDword(Key, "SitePerProcess", 1)],
                },
                new TweakDef
                {
                    Id = "edgiso-enable-strict-origin-isolation",
                    Label = "Enable Strict Origin Isolation in Edge",
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Id = "edgiso-enable-enhanced-tracking-protection",
                    Label = "Enable Strict Tracking Prevention in Edge",
                    Category = "Browser",
                    Description =
                        "Configures Edge Tracking Prevention to Strict mode, blocking known trackers and fingerprinting scripts from all sites including first-party contexts.",
                    Tags = ["edge", "tracking-prevention", "privacy", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Strict tracking prevention active; some social widgets and embedded content may fail to load.",
                    ApplyOps = [RegOp.SetDword(Key, "TrackingPrevention", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TrackingPrevention")],
                    DetectOps = [RegOp.CheckDword(Key, "TrackingPrevention", 3)],
                },
                new TweakDef
                {
                    Id = "edgiso-disable-webrtc-leak",
                    Label = "Disable WebRTC IP Address Leak in Edge",
                    Category = "Browser",
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
                    Id = "edgsleep-enable-sleeping-tabs",
                    Label = "Enable Sleeping Tabs in Edge",
                    Category = "Browser",
                    Description =
                        "Enables the Sleeping Tabs feature in Microsoft Edge, which puts inactive tabs to sleep after a configurable timeout to reduce memory and CPU usage.",
                    Tags = ["edge", "sleeping-tabs", "performance", "memory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Inactive tabs suspended; significant memory savings on machines with many open tabs.",
                    ApplyOps = [RegOp.SetDword(Key, "SleepingTabsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SleepingTabsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "SleepingTabsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "edgsleep-set-timeout-300",
                    Label = "Set Sleeping Tabs Timeout to 5 Minutes",
                    Category = "Browser",
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
                    Id = "edgsleep-disable-startup-boost",
                    Label = "Disable Edge Startup Boost",
                    Category = "Browser",
                    Description =
                        "Disables Edge Startup Boost which pre-launches Edge browser processes at Windows startup to improve launch speed at the cost of persistent background memory consumption.",
                    Tags = ["edge", "startup-boost", "background", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge no longer pre-runs in background; slightly slower first launch, reduced idle RAM.",
                    ApplyOps = [RegOp.SetDword(Key, "StartupBoostEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "StartupBoostEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "StartupBoostEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgsleep-disable-background-run",
                    Label = "Disable Edge Background Running After All Windows Closed",
                    Category = "Browser",
                    Description =
                        "Prevents Edge from running background processes after all browser windows are closed, fully releasing resources when the user is done browsing.",
                    Tags = ["edge", "background", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Edge processes terminate on last window close; push notifications and extensions stop when closed.",
                    ApplyOps = [RegOp.SetDword(Key, "BackgroundModeEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BackgroundModeEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "BackgroundModeEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgsleep-enable-efficiency-mode",
                    Label = "Enable Edge Efficiency Mode",
                    Category = "Browser",
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
                    Id = "edgsleep-disable-tab-preloading",
                    Label = "Disable Edge New Tab Page Preloading",
                    Category = "Browser",
                    Description =
                        "Disables new tab page preloading in Edge which pre-fetches the new tab page in the background to reduce perceived open time at the cost of memory and network usage.",
                    Tags = ["edge", "preloading", "new-tab", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "New tab page not pre-loaded; slightly slower new tab but lower background resource use.",
                    ApplyOps = [RegOp.SetDword(Key, "NewTabPagePrerenderEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NewTabPagePrerenderEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "NewTabPagePrerenderEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "edgsleep-disable-speculative-prerendering",
                    Label = "Disable Speculative Prerendering in Edge",
                    Category = "Browser",
                    Description =
                        "Disables speculative prerendering of links in Edge, which pre-loads pages you might navigate to. Reduces network and memory usage at the cost of navigation latency.",
                    Tags = ["edge", "prerender", "privacy", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Pages not pre-fetched speculatively; slightly slower navigation, less background network traffic.",
                    ApplyOps = [RegOp.SetDword(Key, "NetworkPredictionOptions", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NetworkPredictionOptions")],
                    DetectOps = [RegOp.CheckDword(Key, "NetworkPredictionOptions", 2)],
                },
                new TweakDef
                {
                    Id = "edgsleep-disable-tab-thumbnail-capture",
                    Label = "Disable Tab Thumbnail Capture for Inactive Tabs",
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Id = "edgessf-enable-smartscreen",
                Label = "Edge SmartScreen & Site Isolation Policy: Enable Microsoft Defender SmartScreen",
                Category = "Browser",
                Description =
                    "Enforces Microsoft Defender SmartScreen in Microsoft Edge, preventing users from disabling the feature. Setting SmartScreenEnabled to 1 ensures Edge checks URLs and downloads against Microsoft's threat-intelligence cloud and displays a warning page when a site is identified as phishing or malware-hosting. SmartScreen is a first-line browser defence that blocks a significant proportion of credential-phishing and drive-by-download attacks. Per CIS Benchmark L1, this policy must be set to Enabled on all enterprise machines.",
                Tags = ["edge", "smartscreen", "malware", "phishing", "security", "cis", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "SmartScreenEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "SmartScreenEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "SmartScreenEnabled", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "SmartScreen enforced via policy; visits to phishing and malware-hosting URLs are blocked with a warning page.",
            },
            new TweakDef
            {
                Id = "edgessf-enable-pua-detection",
                Label = "Edge SmartScreen & Site Isolation Policy: Enable SmartScreen Potentially Unwanted Application Detection",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgessf-block-legacy-extension-entry-points",
                Label = "Edge SmartScreen & Site Isolation Policy: Block Legacy Browser Extension Entry Points",
                Category = "Browser",
                Description =
                    "Prevents third-party software from injecting hooks, DLLs, or code into the Microsoft Edge browser process through legacy extension entry points that were used by older Internet Explorer BHOs (Browser Helper Objects) and similar plug-in infrastructure. Setting BrowserLegacyExtensionPointsBlockingEnabled to 1 closes these low-level hooks that can be exploited by malware to intercept browser traffic, inject content into HTTPS pages, or bypass Edge's sandbox. Legitimate Edge extensions use the WebExtensions (CRX) API and are unaffected by this policy.",
                Tags = ["edge", "extension injection", "bho", "dll injection", "security", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "BrowserLegacyExtensionPointsBlockingEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "BrowserLegacyExtensionPointsBlockingEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "BrowserLegacyExtensionPointsBlockingEnabled", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Legacy browser Hook entry points blocked; third-party DLL injection into Edge processes is prevented.",
            },
            new TweakDef
            {
                Id = "edgessf-disable-edge-discover",
                Label = "Edge SmartScreen & Site Isolation Policy: Disable Edge Discover Pane",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgestart-disable-preload-home-page",
                Label = "Edge Startup Policy: Disable Preloading of Home Page on Startup",
                Category = "Browser",
                Description =
                    "Prevents Microsoft Edge from preloading the home page or startup pages in the background during Windows startup or user logon. Edge aggressively preloads its startup page to shorten the perceived time-to-interactive, but this consumes RAM and CPU on logon, delays desktop readiness, and causes unnecessary network activity even when the user does not intend to open Edge.",
                Tags = ["edge", "startup", "preload", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "StartupBoostEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "StartupBoostEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "StartupBoostEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Edge startup boost; eliminates Edge background preloading from Windows logon sequence.",
            },
            new TweakDef
            {
                Id = "edgestart-disable-sleeping-tabs",
                Label = "Edge Startup Policy: Disable Sleeping Tabs Background CPU Throttle",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
            new TweakDef
            {
                Id = "edgestart-disable-edge-workspaces",
                Label = "Edge Startup Policy: Disable Edge Workspaces",
                Category = "Browser",
                Description =
                    "Disables Microsoft Edge Workspaces, the collaborative tab-sharing feature that allows users to share a live set of browser tabs with colleagues via a shared link. Workspaces sync tab lists to Microsoft cloud services and allow external parties to view or join an active workspace. In corporate environments with data-classification or DLP requirements, this feature could inadvertently expose internal URLs to unauthorized users.",
                Tags = ["edge", "workspaces", "sharing", "collaboration", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "EdgeWorkspacesEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "EdgeWorkspacesEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "EdgeWorkspacesEnabled", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Edge Workspaces tab sharing; prevents internal URLs from being shared via workspace links.",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgetrack-disable-user-feedback",
                Label = "Edge Tracking Protection Policy: Disable User Feedback and Error Reporting",
                Category = "Browser",
                Description =
                    "Blocks the built-in Edge user feedback mechanism and telemetric crash/error reporting. When UserFeedbackAllowed is set to 0, the Help > Send Feedback option is removed and Edge will not submit user-generated feedback reports or automatic crash diagnostics to Microsoft. Crash reports may contain browser session state, visited URL data, and clipboard content captured at the time of the crash.",
                Tags = ["edge", "feedback", "telemetry", "crash reporting", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "UserFeedbackAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "UserFeedbackAllowed")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "UserFeedbackAllowed", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Feedback menu entry removed and automatic crash reporting disabled; no browser session data sent in feedback reports.",
            },
            new TweakDef
            {
                Id = "edgetrack-disable-signed-http-exchange",
                Label = "Edge Tracking Protection Policy: Disable Signed HTTP Exchange Support",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Id = "edgetrack-disable-immersive-reader-grammar",
                Label = "Edge Tracking Protection Policy: Disable Immersive Reader Grammar Tools",
                Category = "Browser",
                Description =
                    "Disables the Grammar Tools option within the Edge Immersive Reader panel. When Immersive Reader is opened, Grammar Tools provides syllable highlighting, part-of-speech color-coding, and text analysis powered by Microsoft language services. The grammar analysis requires sending page text content to Microsoft cloud language endpoints. Disabling this feature prevents reading-mode content from being transmitted to external language processing services.",
                Tags = ["edge", "immersive reader", "grammar tools", "privacy", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeKey],
                ApplyOps = [RegOp.SetDword(EdgeKey, "ImmersiveReaderGrammarToolsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeKey, "ImmersiveReaderGrammarToolsEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeKey, "ImmersiveReaderGrammarToolsEnabled", 0)],
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Grammar analysis panel removed from Immersive Reader; syllable and part-of-speech highlighting unavailable.",
            },
            new TweakDef
            {
                Id = "edgetrack-block-intrusive-ads",
                Label = "Edge Tracking Protection Policy: Block Ads on Sites with Intrusive Ad Experiences",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Id = "edgewp-disable-work-profile-data-sync",
                    Label = "Edge Work Profile: Disable Work Profile Data Sync to Microsoft Account",
                    Category = "Browser",
                    Description =
                        "Sets SyncDisabled=1 in Edge policy. Prevents the Edge work profile from syncing browsing history, saved passwords, extensions, and settings to the user's Microsoft cloud account (personal or enterprise), keeping work profile data resident on the managed device only. "
                        + "Edge sync uploads work browsing history, internal URL patterns, saved credentials for internal web apps, and installed extension lists to Microsoft's cloud sync service. In high-sensitivity environments, the browsing history itself may constitute proprietary information. Disabling sync ensures that work profile data stays on the managed device and any downstream device the user signs into using the same account cannot access the work browser data.",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Id = "edgewp-disable-inprivate-browsing",
                    Label = "Edge Work Profile: Disable InPrivate Browsing Mode",
                    Category = "Browser",
                    Description =
                        "Sets InPrivateModeAvailability=1 in Edge policy. Disables InPrivate browsing mode (value 1 = Disabled) in Edge, preventing users from opening sessions where browsing history and cookies are not retained. "
                        + "InPrivate mode, while legitimate for privacy, prevents the browsing session from being recorded in Edge history and from being subject to certain telemetry and audit policies that depend on session context. In regulated industries where all web access must be logged for compliance, InPrivate sessions create unlogged access points. Additionally, some DLP inspection products are not applied to InPrivate sessions by default.",
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
                    Category = "Browser",
                    Description =
                        "Sets EdgeWorkspacesEnabled=0 in Edge policy. Disables the Edge profile discovery feature that automatically prompts users to create new work profiles when it detects a new Microsoft Entra ID sign-in event, preventing uncontrolled proliferation of managed profiles on shared or shared-use devices. "
                        + "On devices shared among multiple users (shift workers, kiosk-style workstations), automatic profile creation means each user who signs into a Microsoft service triggers Edge to create a new managed profile bound to their work account. On machines not managed for roaming profiles, these additional profiles accumulate with cached policy configurations, create additional disk usage, and cause confusing multi-profile scenarios.",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Category = "Browser",
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
                Id = "iecompat-disable-ie-mode-in-edge",
                Label = "IE Compat: Disable IE Mode in Microsoft Edge",
                Category = "Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeMain],
                Tags = ["ie", "ie-mode", "edge", "compatibility", "security", "policy"],
                Description =
                    "Sets InternetExplorerIntegrationLevel=0 in Edge policy. Disables IE mode integration "
                    + "in Edge, preventing the legacy Trident rendering engine from loading. "
                    + "Default: 1 (IE Mode available). Setting to 0 enforces modern rendering only.",
                ApplyOps = [RegOp.SetDword(EdgeMain, "InternetExplorerIntegrationLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeMain, "InternetExplorerIntegrationLevel")],
                DetectOps = [RegOp.CheckDword(EdgeMain, "InternetExplorerIntegrationLevel", 0)],
            },
            new TweakDef
            {
                Id = "iecompat-disable-ie-first-run",
                Label = "IE Compat: Disable IE First-Run Wizard",
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
            new TweakDef
            {
                Id = "iecompat-disable-edge-password-manager",
                Label = "IE Compat: Disable Edge Built-In Password Manager",
                Category = "Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EdgeMain],
                Tags = ["edge", "password-manager", "security", "policy", "credentials"],
                Description =
                    "Sets PasswordManagerEnabled=0 in Edge policy. Prevents Edge from offering to save "
                    + "or filling saved passwords. Intended for environments using enterprise password vaults. "
                    + "Default: 1 (enabled). Disabling forces use of dedicated PAM/credential vault solutions.",
                ApplyOps = [RegOp.SetDword(EdgeMain, "PasswordManagerEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeMain, "PasswordManagerEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeMain, "PasswordManagerEnabled", 0)],
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Id = "ledge-enable-phishing-filter",
                    Label = "Enable SmartScreen Phishing Filter in Legacy Edge",
                    Category = "Browser",
                    Description =
                        "Enforces the SmartScreen phishing and malware filter in legacy Microsoft Edge, ensuring it cannot be disabled by the user and providing baseline threat protection.",
                    Tags = ["edge", "legacy edge", "smartscreen", "phishing", "malware", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Ensures SmartScreen is always active in legacy Edge; critical for phishing protection.",
                    RegistryKeys = [PhishingKey],
                    ApplyOps = [RegOp.SetDword(PhishingKey, "EnabledV9", 1)],
                    RemoveOps = [RegOp.DeleteValue(PhishingKey, "EnabledV9")],
                    DetectOps = [RegOp.CheckDword(PhishingKey, "EnabledV9", 1)],
                },
                new TweakDef
                {
                    Id = "ledge-prevent-smartscreen-bypass",
                    Label = "Prevent Bypassing SmartScreen Warnings in Legacy Edge",
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
                    Category = "Browser",
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
