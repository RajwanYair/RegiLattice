namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Copilot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ai-disable-copilot",
            Label = "Disable Windows Copilot",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Copilot via Group Policy and hides the taskbar button. Prevents AI-powered assistant from running.",
            Tags = ["ai", "copilot", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsCopilot", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-edge-copilot",
            Label = "Disable Copilot in Edge Browser",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Copilot sidebar and page context sharing in Microsoft Edge.",
            Tags = ["ai", "copilot", "edge", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "HubsSidebarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-button",
            Label = "Hide Copilot Taskbar Button",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Copilot button from the Windows 11 taskbar.",
            Tags = ["ai", "copilot", "taskbar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-bing-chat",
            Label = "Disable Bing Chat in Search",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bing Chat / AI suggestions in Windows Search.",
            Tags = ["ai", "bing", "search", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-recall-policy",
            Label = "Disable Windows Recall (AI Screenshot Feature)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Recall AI data analysis via machine-level policy. Prevents the AI screenshot feature from capturing activity.",
            Tags = ["ai", "copilot", "recall", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-taskbar",
            Label = "Remove Copilot from Taskbar",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Copilot button from the Windows taskbar.",
            Tags = ["ai", "copilot", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-ai-start-suggestions",
            Label = "Disable AI-Powered Suggestions in Settings",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AI-powered Iris recommendations in the Start menu and Settings app.",
            Tags = ["ai", "copilot", "suggestions", "start"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-edge",
            Label = "Disable Copilot in Edge Browser",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Copilot CDP page context feature in Microsoft Edge.",
            Tags = ["ai", "copilot", "edge"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-ai-widgets",
            Label = "Disable AI-Enhanced Widgets Feed",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the AI-enhanced News and Interests widgets feed via Dsh policy.",
            Tags = ["ai", "copilot", "widgets"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-recall",
            Label = "Disable Recall Feature",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Recall AI feature that takes periodic screenshots. Prevents privacy-invasive screen capture and analysis. Default: Enabled. Recommended: Disabled.",
            Tags = ["copilot", "recall", "privacy", "ai"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-taskbar-button",
            Label = "Disable Copilot Taskbar Button",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Copilot button from the Windows taskbar. Reduces visual clutter. Default: Shown. Recommended: Hidden.",
            Tags = ["copilot", "taskbar", "ux", "ai"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-recall",
            Label = "Disable Windows Recall (HKLM Policy)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Recall AI data analysis via HKLM Group Policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["ai", "recall", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-keyboard",
            Label = "Disable Copilot Keyboard Shortcut",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Copilot button and disables keyboard shortcut. Default: Shown. Recommended: Hidden.",
            Tags = ["ai", "copilot", "keyboard", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-runtime-24h2",
            Label = "Disable Copilot Runtime (24H2)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Copilot Runtime system app introduced in Windows 11 24H2. Uses the new AllowCopilotRuntime policy path. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["ai", "copilot", "24h2", "runtime", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime"],
            MinBuild = 26100,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime", "AllowCopilotRuntime", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime", "AllowCopilotRuntime"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime", "AllowCopilotRuntime", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-bing-chat-edge-24h2",
            Label = "Disable Bing Chat in Edge (24H2)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = @"Blocks Bing Chat / Copilot in Edge sidebar using the updated 24H2 policy path (BingChat\IsUserEligible). Default: enabled. Recommended: disabled.",
            Tags = ["ai", "copilot", "edge", "bing-chat", "24h2"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\BingChat"],
            MinBuild = 26100,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ai-copilot-ineligible",
            Label = "Block Copilot User Eligibility",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Marks the current user as ineligible for Copilot via the Shell BingChat registry key. Prevents Copilot from activating. Default: eligible. Recommended: ineligible for privacy.",
            Tags = ["ai", "copilot", "eligible", "user", "shell"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserEligible", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserEligible"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserEligible", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-edge-sidebar",
            Label = "Disable Copilot Edge Sidebar (Policy)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Copilot/Bing sidebar in Microsoft Edge via the HubsSidebarEnabled policy. Default: enabled. Recommended: disabled.",
            Tags = ["ai", "copilot", "edge", "sidebar", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-ai-in-settings",
            Label = "Disable AI Suggestions in Windows Settings",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AI data analysis features in the Windows Settings app via the WindowsAI group policy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["ai", "copilot", "settings", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-taskbar-icon",
            Label = "Hide Copilot Taskbar Icon",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Copilot button from the Windows 11 taskbar. Default: visible.",
            Tags = ["ai", "copilot", "taskbar", "icon", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-bing-search-suggestions",
            Label = "Disable Bing Copilot Search Suggestions",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bing-powered AI search suggestions in the Start menu. Default: enabled.",
            Tags = ["ai", "bing", "search", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-cloud-content-suggestions",
            Label = "Disable Cloud Content AI Suggestions",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables cloud-based content suggestions powered by AI in Windows experiences. Default: enabled.",
            Tags = ["ai", "cloud", "content", "suggestions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-hardware-key",
            Label = "Disable Copilot Hardware Key",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the dedicated Copilot hardware key on new keyboards. The key does nothing when pressed. Default: launches Copilot.",
            Tags = ["ai", "copilot", "hardware", "keyboard", "key"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotHardwareKey", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotHardwareKey")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotHardwareKey", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-image-creator",
            Label = "Disable AI Image Creator in Edge",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the AI Image Creator feature in Microsoft Edge via policy. Default: enabled.",
            Tags = ["ai", "image", "creator", "edge", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImageCreatorEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImageCreatorEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "ImageCreatorEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-taskbar-search-ai",
            Label = "Disable AI in Taskbar Search",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AI-powered features in the Windows taskbar search box. Prevents Copilot integration in search results. Default: enabled.",
            Tags = ["ai", "copilot", "taskbar", "search"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-tips-notifications",
            Label = "Disable Copilot Tips & Notifications",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Copilot tips, suggestions, and notification prompts. Prevents AI feature promotion popups. Default: enabled.",
            Tags = ["ai", "copilot", "tips", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338388Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-ai-suggestions",
            Label = "Disable AI Suggestions in Settings",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AI-powered suggestions and recommendations in Windows Settings. Removes intelligent content cards. Default: enabled.",
            Tags = ["ai", "suggestions", "settings", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353694Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353694Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353694Enabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-recall-snapshots",
            Label = "Disable Windows Recall Snapshots",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Recall (AI-powered screen snapshots). Prevents periodic screenshots and AI indexing of desktop activity. Default: varies.",
            Tags = ["ai", "recall", "snapshots", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
    ];
}
