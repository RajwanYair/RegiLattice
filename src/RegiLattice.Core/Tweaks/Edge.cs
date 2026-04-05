namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Edge.cs ──
internal static class Edge
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edge-disable-collections",
            Label = "Disable Edge Collections",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Edge Collections feature used for organizing web content. Reduces UI clutter and memory usage. Default: Enabled. Recommended: Disabled if not used.",
            Tags = ["edge", "collections", "ux", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-mini-menu",
            Label = "Disable Edge Mini Context Menu",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the mini context menu that appears on text selection in Edge. Removes the floating toolbar with search/copy/etc. Default: Enabled. Recommended: Disabled for cleaner UX.",
            Tags = ["edge", "mini-menu", "context-menu", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-personalization",
            Label = "Disable Edge Personalization and Advertising",
            Category = "Browser 2",
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
            Id = "edge-disable-sidebar",
            Label = "Disable Edge Sidebar",
            Category = "Browser 2",
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
            Id = "edge-disable-copilot-sidebar",
            Label = "Disable Edge Copilot Sidebar",
            Category = "Browser 2",
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
            Id = "edge-disable-metrics",
            Label = "Disable Edge Diagnostic Data Collection",
            Category = "Browser 2",
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
            Id = "edge-disable-edge-update",
            Label = "Disable Edge Auto-Update",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Edge from auto-updating. Useful for controlled environments or when pinning to a specific version.",
            Tags = ["edge", "browser", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "UpdateDefault", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "AutoUpdateCheckPeriodMinutes", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "UpdateDefault"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "AutoUpdateCheckPeriodMinutes"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate", "UpdateDefault", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-sidebar-hub",
            Label = "Disable Edge Sidebar Hub",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Edge sidebar (Hubs) panel via enterprise policy. Removes the sidebar icon and panel.",
            Tags = ["edge", "sidebar", "hubs", "ux", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SideSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SideSearchEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SideSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-workspaces",
            Label = "Disable Edge Workspaces",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Edge Workspaces feature for shared browsing sessions via enterprise policy. Reduces background sync overhead.",
            Tags = ["edge", "workspaces", "collaboration", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeWorkspacesEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeWorkspacesEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeWorkspacesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-drop",
            Label = "Disable Edge Drop",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Edge Drop feature used for cross-device file sharing via enterprise policy. Reduces cloud sync and network usage.",
            Tags = ["edge", "drop", "file-sharing", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeEDropEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeEDropEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeEDropEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-discover",
            Label = "Disable Edge Discover Button",
            Category = "Browser 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Edge Discover (compass) button and page context features via enterprise policy. Reduces Copilot integration.",
            Tags = ["edge", "discover", "copilot", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiscoverPageContextEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiscoverPageContextEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DiscoverPageContextEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-tab-preload-off",
            Label = "Disable Edge new tab page preloading",
            Category = "Browser 2",
            Tags = ["edge", "tab", "preload", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PreloadNewTabPageEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PreloadNewTabPageEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PreloadNewTabPageEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-media-autoplay-off",
            Label = "Disable media autoplay in Edge",
            Category = "Browser 2",
            Tags = ["edge", "autoplay", "media", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutoplayAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutoplayAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AutoplayAllowed", 0)],
        },
        new TweakDef
        {
            Id = "edge-shopping-assistant-off",
            Label = "Disable Edge shopping assistant / Bing price compare",
            Category = "Browser 2",
            Tags = ["edge", "shopping", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShoppingAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShoppingAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-pdf-external",
            Label = "Open PDFs in external viewer instead of Edge",
            Category = "Browser 2",
            Tags = ["edge", "pdf", "viewer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AlwaysOpenPdfExternally", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AlwaysOpenPdfExternally")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AlwaysOpenPdfExternally", 1)],
        },
        new TweakDef
        {
            Id = "edge-default-search-provider-lock",
            Label = "Lock default search provider in Edge",
            Category = "Browser 2",
            Tags = ["edge", "search", "provider", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultSearchProviderEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultSearchProviderEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultSearchProviderEnabled", 1)],
        },
        new TweakDef
        {
            Id = "edge-ie-mode-off",
            Label = "Disable Internet Explorer integration mode in Edge",
            Category = "Browser 2",
            Tags = ["edge", "ie", "compatibility", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "InternetExplorerIntegrationLevel", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "InternetExplorerIntegrationLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "InternetExplorerIntegrationLevel", 0)],
        },
        new TweakDef
        {
            Id = "edge-wallet-off",
            Label = "Disable Edge Wallet / payment methods",
            Category = "Browser 2",
            Tags = ["edge", "wallet", "payment", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PaymentMethodQueryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PaymentMethodQueryEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PaymentMethodQueryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-immersive-reader",
            Label = "Disable Edge Immersive Reader grammar tools",
            Category = "Browser 2",
            Tags = ["edge", "immersive-reader", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImmersiveReaderGrammarToolsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImmersiveReaderGrammarToolsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImmersiveReaderGrammarToolsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-copilot-page-context",
            Label = "Block Edge Copilot from reading page context",
            Category = "Browser 2",
            Tags = ["edge", "copilot", "ai", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotPageContext", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotPageContext")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotPageContext", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-news-feed",
            Label = "Disable Edge new tab page news feed / content",
            Category = "Browser 2",
            Tags = ["edge", "news", "new-tab", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageContentEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageContentEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageContentEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-ntp-recommendations",
            Label = "Disable Edge new tab page recommended content",
            Category = "Browser 2",
            Tags = ["edge", "recommendations", "new-tab", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageRecommendedContentEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageRecommendedContentEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "NewTabPageRecommendedContentEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-edge-bar-gpo",
            Label = "Disable Edge Bar floating browser window",
            Category = "Browser 2",
            Tags = ["edge", "edge-bar", "floating", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeBarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeBarEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-insider-promo",
            Label = "Disable Edge insider/beta channel promotions",
            Category = "Browser 2",
            Tags = ["edge", "insider", "promo", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MicrosoftEdgeInsiderPromotionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MicrosoftEdgeInsiderPromotionEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MicrosoftEdgeInsiderPromotionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-web-widget",
            Label = "Disable Edge web widget (desktop/taskbar search bar)",
            Category = "Browser 2",
            Tags = ["edge", "web-widget", "search-bar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "WebWidgetAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "WebWidgetAllowed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "WebWidgetAllowed", 0)],
        },
    ];
}
