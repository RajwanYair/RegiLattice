#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// IE Compatibility Policy — controls Internet Explorer mode in Microsoft Edge and
// disables legacy IE Enterprise Mode that can weaken modern browser security posture.
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Main\EnterpriseMode
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge
internal static class IECompatPolicy
{
    private const string IeMainPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Main";
    private const string IeEntMode = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Main\EnterpriseMode";
    private const string IeSecurity = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Security";
    private const string EdgeMain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";
    private const string EdgeCompat = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\IEModeTabUrls";
    private const string IeZones = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "iecompat-disable-ie-enterprise-mode",
            Label = "IE Compat: Disable IE Enterprise Mode Site List",
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
            Category = "IE Compatibility Policy",
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
