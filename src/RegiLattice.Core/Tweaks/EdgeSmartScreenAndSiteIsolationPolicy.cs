namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class EdgeSmartScreenAndSiteIsolationPolicy
{
    private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edgessf-enable-smartscreen",
            Label = "Edge SmartScreen & Site Isolation Policy: Enable Microsoft Defender SmartScreen",
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
            Category = "Edge SmartScreen & Site Isolation Policy",
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
