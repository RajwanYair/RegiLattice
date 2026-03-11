namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Edge
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edge-disable-collections",
            Label = "Disable Edge Collections",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge Collections feature used for organizing web content. Reduces UI clutter and memory usage. Default: Enabled. Recommended: Disabled if not used.",
            Tags = ["edge", "collections", "ux", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-mini-menu",
            Label = "Disable Edge Mini Context Menu",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the mini context menu that appears on text selection in Edge. Removes the floating toolbar with search/copy/etc. Default: Enabled. Recommended: Disabled for cleaner UX.",
            Tags = ["edge", "mini-menu", "context-menu", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-shopping-assistant",
            Label = "Disable Edge Shopping Assistant",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge shopping/coupon assistant that appears on retail sites. Default: enabled.",
            Tags = ["edge", "shopping", "coupons", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-startup-boost",
            Label = "Disable Edge Startup Boost",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge Startup Boost that preloads Edge processes at Windows startup. Saves RAM. Default: enabled.",
            Tags = ["edge", "startup", "boost", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-personalization",
            Label = "Disable Edge Personalization and Advertising",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables personalization and advertising features in Edge that track browsing behavior. Default: enabled.",
            Tags = ["edge", "personalization", "advertising", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-browser-sign-in",
            Label = "Disable Automatic Browser Sign-In",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Edge from automatically signing into the browser using the Windows account. Default: auto-sign-in.",
            Tags = ["edge", "sign-in", "account", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-sidebar",
            Label = "Disable Edge Sidebar",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge sidebar panel (Bing Chat, tools, games). Reduces distractions and memory. Default: enabled.",
            Tags = ["edge", "sidebar", "ux", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-first-run-experience",
            Label = "Disable Edge First Run Experience",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Skips the Edge first run experience and import wizard on new profiles. Default: shown.",
            Tags = ["edge", "first-run", "import", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HideFirstRunExperience", 1)],
        },
        new TweakDef
        {
            Id = "edge-disable-password-manager",
            Label = "Disable Edge Password Manager",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge built-in password manager. Use a dedicated password manager. Default: enabled.",
            Tags = ["edge", "password", "manager", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-translate",
            Label = "Disable Edge Translation",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic page translation prompts in Edge. Default: enabled.",
            Tags = ["edge", "translate", "language", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "TranslateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-copilot-sidebar",
            Label = "Disable Edge Copilot Sidebar",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Copilot AI sidebar in Edge. Reduces telemetry and resource usage. Default: enabled.",
            Tags = ["edge", "copilot", "ai", "sidebar", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-sync",
            Label = "Disable Edge Sync",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge data synchronization across devices. Keeps browsing data local. Default: enabled.",
            Tags = ["edge", "sync", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "edge-block-third-party-cookies",
            Label = "Block Third-Party Cookies in Edge",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party cookies in Edge to prevent cross-site tracking. Default: allowed.",
            Tags = ["edge", "cookies", "tracking", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BlockThirdPartyCookies", 1)],
        },
        new TweakDef
        {
            Id = "edge-disable-metrics",
            Label = "Disable Edge Diagnostic Data Collection",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Edge diagnostic data to off. Reduces telemetry sent to Microsoft. Default: optional.",
            Tags = ["edge", "diagnostics", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiagnosticData", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiagnosticData")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiagnosticData", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-autofill-credit-cards",
            Label = "Disable Edge Credit Card Autofill",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge from saving and auto-filling credit card information. Default: enabled.",
            Tags = ["edge", "autofill", "credit-card", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutofillCreditCardEnabled", 0)],
        },
    ];
}
