// RegiLattice.Core — Tweaks/EdgeAutoFillPolicy.cs
// Microsoft Edge autofill, form data, and personal data management Group Policy controls (Sprint 616).
// Category: "Edge AutoFill Policy" | Slug: edgeaf
// Key: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeAutoFillPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "edgeaf-disable-address-autofill",
            Label = "Edge AutoFill: Disable AutoFill for Address and Contact Information",
            Category = "Edge AutoFill Policy",
            Description = "Sets AutofillAddressEnabled=0 in Edge policy. Prevents Edge from storing, suggesting, or filling home/work addresses, phone numbers, and contact details in web forms using the browser's autofill profile database. " +
                "Autofill address data is stored in the Edge browser profile directory, which is located within the user's Windows profile. Any process running as the current user can read the profile's 'Web Data' SQLite database and extract all stored addresses and phone numbers in cleartext. Disabling address autofill eliminates this persisted PII from the browser profile, reducing the blast radius of a browser profile data theft.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets AutofillCreditCardEnabled=0 in Edge policy. Prevents Edge from storing, offering to save, or automatically filling credit card numbers, expiry dates, and CVV codes in payment forms using the browser's payment autofill database. " +
                "Credit card numbers stored in the Edge autofill database persist in the browser profile's 'Web Data' file. The file is encrypted at rest using Windows DPAPI, but DPAPI decryption requires only the user's active session context — no additional PIN or authentication. Any script or process running as the user can request DPAPI decryption of the autofill database and recover stored card numbers. Enterprise browsers should never store payment card data.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets FormFillEnabled=0 in Edge policy. Prevents Edge from recording typed text in non-password form fields (names, addresses, search terms, custom input fields) and suggesting previously-entered values as autocomplete options in future form fills. " +
                "Edge's form fill history accumulates text entries from all web forms — including internal expense report form fields, project code inputs, internal tool form submissions, and system prompts asking for passphrases or access codes. This history is stored in the profile database and can reveal sensitive operational information to other processes or when the profile is roamed. Disabling form fill prevents this implicit data capture.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets PaymentMethodQueryEnabled=0 in Edge policy. Disables the Payment Request API in Edge, preventing web pages from programmatically querying whether the user has saved payment methods in Edge and from triggering the Payment Request UX when initiated by JavaScript. " +
                "The Payment Request API allows a web page to enumerate available payment methods and trigger a native payment UI sheet. Malicious or compromised retail web pages can abuse this API to detect whether a user has credit card data stored in Edge, serving this information as a targeting signal for subsequent social engineering attacks. Disabling the API prevents this enumeration and blocks web-initiated payment flows entirely.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets BrowserSignin=0 in Edge policy. Prevents Edge from syncing saved autofill data (addresses, form data, payment methods) to other devices via the user's Microsoft account, keeping browser autofill data isolated to the current managed device. " +
                "Edge browser sync transfers autofill data — including typed form history and saved addresses — to Microsoft's sync service and then to all other devices where the user is signed into Edge with the same Microsoft account. Personal devices may not have DLP, antivirus, or endpoint protection policies. Synced autofill data that originates from work browsing (containing internal site form inputs) may leak to a personal device with weaker security controls.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets AutofillDropdownEnabled=0 in Edge policy. Disables Edge's autofill dropdown overlay that appears above form fields during third-party browser extension autofill interactions, preventing extensions from hijacking the autofill UI rendering layer in sensitive input fields. " +
                "Browser extensions that implement autofill (third-party password managers, form filler tools) render their autofill dropdown UI by injecting DOM elements onto the page. A malicious extension that mimics a legitimate autofill tool can render a convincing autofill dropdown with credentials from a different site, performing a form-autofill phishing attack where the user believes they are filling stored credentials but is actually submitting attacker-controlled values.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets RestrictedSitesListEnabled=0 in Edge policy (using profiles autofill channel). Disables autofill functionality specifically when Edge is operating outside the managed work profile context, ensuring that any personal browsing done in a separate profile does not populate the work profile's autofill store with personal data. " +
                "When users sign into personal accounts within Edge alongside their work profile, form fill data from personal browsing (personal address, personal email, personal shopping sites) can end up in the same autofill database as work browsing data. This mixing creates a data classification problem where personal PII is co-mingled with work data in the managed browser profile store.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets ClearBrowsingDataOnExit=1 in Edge policy. Configures Edge to clear all autofill form history, cached addresses, and saved form data from the browser profile database each time the user closes the browser, ensuring autofill data does not accumulate across sessions. " +
                "On shared workstations or devices with multiple users accessing the same domain-joined Windows account (e.g., shift work, shared kiosk, hotdesking), autofill data accumulated during one user's session may be offered as autofill suggestions to the next user who opens the browser. Clearing autofill data on browser exit ensures each browser session is independent from a data persistence perspective.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets AddressBarEditingEnabled=0 in Edge policy. Prevents Edge from displaying Bing-powered autocomplete suggestions in the address bar that are derived from the user's past typed entries, Bing search history, and previous navigation history. " +
                "Bing autocomplete suggestions in the Edge address bar are populated from multiple sources including cloud-synchronised search history and local browsing history. As the user types in the address bar, keystrokes are sent to Bing's suggestion API — even partial internal URLs or IP addresses typed for network administration may be transmitted as suggestion queries. Disabling address bar suggestions prevents this pre-submission keystroke telemetry from leaving the device.",
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
            Category = "Edge AutoFill Policy",
            Description = "Sets EdgeShoppingAssistantEnabled=0 in Edge policy. Disables Edge's built-in shopping assistant that automatically detects product pages, suggests coupons, compares prices, and activates cashback offers, preventing these features from transmitting purchase intent signals and retail browsing patterns to Microsoft's shopping backend. " +
                "The Edge shopping assistant monitors every page visit and performs URL classification to detect retail product pages in real time. This classification sends a request to Microsoft's shopping API containing the page URL and product context for every retail page visited. In regulated industries (healthcare, finance), browsing on retail product pages that may correlate with personal spending habits constitutes PII data that should not be transmitted to external advertising-adjacent services.",
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
