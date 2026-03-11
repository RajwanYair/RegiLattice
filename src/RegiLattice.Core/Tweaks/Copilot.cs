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
            Id = "ai-disable-recall-snapshots",
            Label = "Disable Recall (AI Snapshots)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Recall (AI-powered activity snapshots) which periodically screenshots your activity. Privacy-critical.",
            Tags = ["ai", "recall", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsAI", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
        },
        new TweakDef
        {
            Id = "ai-disable-ai-suggestions",
            Label = "Disable AI Tips in Settings/Start",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AI-powered suggestions and tips in the Start menu and Settings.",
            Tags = ["ai", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
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
            Id = "ai-copilot-disable-taskbar-search-ai",
            Label = "Disable Copilot in Taskbar Search",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AI-powered suggestions in the taskbar search box via user-level Explorer policy. Default: Enabled. Recommended: Disabled for focused search.",
            Tags = ["ai", "copilot", "taskbar", "search", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-tips-notifications",
            Label = "Disable Tips & Suggestions Notifications",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, suggestions, and soft-landing notification prompts from ContentDeliveryManager. Default: Enabled. Recommended: Disabled.",
            Tags = ["ai", "copilot", "tips", "suggestions", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
        },
    ];
}
