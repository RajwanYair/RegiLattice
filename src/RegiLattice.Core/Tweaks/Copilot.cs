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
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsCopilot",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            ],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-recall-policy",
            Label = "Disable Windows Recall (AI Screenshot Feature)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Recall AI data analysis via machine-level policy. Prevents the AI screenshot feature from capturing activity.",
            Tags = ["ai", "copilot", "recall", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-recall",
            Label = "Disable Recall Feature",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Recall AI feature that takes periodic screenshots. Prevents privacy-invasive screen capture and analysis. Default: Enabled. Recommended: Disabled.",
            Tags = ["copilot", "recall", "privacy", "ai"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-runtime-24h2",
            Label = "Disable Copilot Runtime (24H2)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Copilot Runtime system app introduced in Windows 11 24H2. Uses the new AllowCopilotRuntime policy path. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["ai", "copilot", "24h2", "runtime", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime"],
            MinBuild = 26100,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime", "AllowCopilotRuntime", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime", "AllowCopilotRuntime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime", "AllowCopilotRuntime", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-bing-chat-edge-24h2",
            Label = "Disable Bing Chat in Edge (24H2)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                @"Blocks Bing Chat / Copilot in Edge sidebar using the updated 24H2 policy path (BingChat\IsUserEligible). Default: enabled. Recommended: disabled.",
            Tags = ["ai", "copilot", "edge", "bing-chat", "24h2"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\BingChat"],
            MinBuild = 26100,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ai-copilot-ineligible",
            Label = "Block Copilot User Eligibility",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Marks the current user as ineligible for Copilot via the Shell BingChat registry key. Prevents Copilot from activating. Default: eligible. Recommended: ineligible for privacy.",
            Tags = ["ai", "copilot", "eligible", "user", "shell"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserEligible", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserEligible")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserEligible", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-edge-sidebar",
            Label = "Disable Copilot Edge Sidebar (Policy)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Copilot/Bing sidebar in Microsoft Edge via the HubsSidebarEnabled policy. Default: enabled. Recommended: disabled.",
            Tags = ["ai", "copilot", "edge", "sidebar", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "CopilotCDPPageContext", 0)],
        },
        new TweakDef
        {
            Id = "ai-copilot-disable-ai-in-settings",
            Label = "Disable AI Suggestions in Windows Settings",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables AI data analysis features in the Windows Settings app via the WindowsAI group policy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["ai", "copilot", "settings", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
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
            Description =
                "Disables the dedicated Copilot hardware key on new keyboards. The key does nothing when pressed. Default: launches Copilot.",
            Tags = ["ai", "copilot", "hardware", "keyboard", "key"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotHardwareKey", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotHardwareKey"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotHardwareKey", 1),
            ],
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
            Id = "ai-copilot-disable-tips-notifications",
            Label = "Disable Copilot Tips & Notifications",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows Copilot tips, suggestions, and notification prompts. Prevents AI feature promotion popups. Default: enabled.",
            Tags = ["ai", "copilot", "tips", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ai-disable-ai-suggestions",
            Label = "Disable AI Suggestions in Settings",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables AI-powered suggestions and recommendations in Windows Settings. Removes intelligent content cards. Default: enabled.",
            Tags = ["ai", "suggestions", "settings", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ai-disable-recall-snapshots",
            Label = "Disable Windows Recall Snapshots",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Recall (AI-powered screen snapshots). Prevents periodic screenshots and AI indexing of desktop activity. Default: varies.",
            Tags = ["ai", "recall", "snapshots", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-activity-history-upload",
            Label = "Disable Activity History Cloud Upload",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from uploading user activity history to the cloud for AI personalisation. Default: enabled.",
            Tags = ["ai", "activity", "history", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-click-to-do",
            Label = "Disable Click-to-Do AI Feature",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Click-to-Do which offers contextual AI actions on selected content. Default: enabled on Copilot+ PCs.",
            Tags = ["ai", "click-to-do", "copilot-plus", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableClickToDo", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableClickToDo")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableClickToDo", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-auto-super-resolution",
            Label = "Disable AI Automatic Super Resolution",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic AI super resolution that upscales video content. Default: enabled on Copilot+ PCs.",
            Tags = ["ai", "super-resolution", "video", "copilot-plus"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAutomaticSuperResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAutomaticSuperResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAutomaticSuperResolution", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-windows-ai-master",
            Label = "Disable All Windows AI Features (Master Switch)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Master policy switch to disable all Windows AI features system-wide. Default: enabled.",
            Tags = ["ai", "policy", "master", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowWindowsAIFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowWindowsAIFeatures")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowWindowsAIFeatures", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-bingchat-history",
            Label = "Disable Copilot Bing Chat History Upload",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables user activity upload for Bing Chat in Copilot. Prevents chat history from syncing to Microsoft. Default: enabled.",
            Tags = ["ai", "copilot", "bing-chat", "history", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Shell\Copilot\BingChat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserActivityUploadEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserActivityUploadEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Shell\Copilot\BingChat", "IsUserActivityUploadEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-recall-user",
            Label = "Disable Windows Recall (User Level)",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows Recall AI analysis at the user level. Prevents AI from indexing personal screen activity. Default: varies.",
            Tags = ["ai", "recall", "user", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI", "DisableAIDataAnalysis", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-proactive-genai",
            Label = "Disable Proactive GenAI Assistance",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables proactive GenAI suggestions in Windows. Default: enabled on AI-capable hardware.",
            Tags = ["ai", "genai", "proactive", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI", "ProactivelyHelpEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI", "ProactivelyHelpEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsAI", "ProactivelyHelpEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ai-restrict-implicit-text-collection",
            Label = "Restrict Implicit Text Collection for AI",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from implicitly collecting typed text for AI personalisation via Group Policy. Default: collection allowed.",
            Tags = ["ai", "text-collection", "personalization", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
            ],
        },
        new TweakDef
        {
            Id = "ai-disable-account-info-sharing",
            Label = "Disable AI Account Information Sharing",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from sharing account information with AI features for personalisation. Default: sharing enabled.",
            Tags = ["ai", "account", "sharing", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy", "UserInfoSharing", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy", "UserInfoSharing", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy", "UserInfoSharing", 0)],
        },
        new TweakDef
        {
            Id = "ai-disable-copilot-start-button",
            Label = "Hide Copilot Button in Taskbar",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Copilot button from the taskbar. Default: shown in Windows 11 24H2+.",
            Tags = ["ai", "copilot", "taskbar", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },

        new TweakDef
        {
            Id = "ai-disable-windows-tips",
            Label = "Disable Windows Tips & Suggestions (GPO)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows tips, tricks, and suggestions via Group Policy. Prevents AI-powered soft-landing suggestion prompts from appearing. Default: enabled.",
            Tags = ["ai", "tips", "suggestions", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-third-party-suggestions",
            Label = "Disable Third-Party App AI Suggestions (GPO)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables third-party AI-powered app suggestions in the Start menu and lock screen via Group Policy. Default: suggestions enabled on consumer Windows.",
            Tags = ["ai", "suggestions", "third-party", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-start-menu-suggestions",
            Label = "Disable AI Start Menu App Suggestions",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables AI-generated app suggestions shown in the Start menu's Recommended section. Removes personalised app promotion. Default: enabled.",
            Tags = ["ai", "start-menu", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ai-disable-lock-screen-overlay",
            Label = "Disable Lock Screen Spotlight Overlay Facts",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Spotlight overlay info shown on the lock screen (\"Learn more\" text, fun facts). Keeps lock screen clean without AI-curated captions. Default: enabled with Spotlight.",
            Tags = ["ai", "spotlight", "lock-screen", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ai-disable-live-captions",
            Label = "Disable Windows Live Captions AI (GPO)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Disables the Windows Live Captions AI accessibility feature via Group Policy. Prevents the real-time AI speech-to-text caption overlay. Default: available on Win 11 22H2+.",
            Tags = ["ai", "live-captions", "speech", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableLiveCaptions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableLiveCaptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableLiveCaptions", 1)],
        },
        new TweakDef
        {
            Id = "ai-disable-spotlight-action-center",
            Label = "Disable Spotlight in Action Center (GPO)",
            Category = "AI / Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Spotlight features in the Action Center (notification panel) via Group Policy. Removes AI-curated content in the notification area. Default: enabled.",
            Tags = ["ai", "spotlight", "action-center", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnActionCenter", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnActionCenter"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnActionCenter", 1),
            ],
        },
        new TweakDef
        {
            Id = "ai-disable-content-delivery-autoinstall",
            Label = "Disable AI-Driven Silent App Auto-Install",
            Category = "AI / Copilot",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables silent automatic installation of apps recommended by the Content Delivery Manager AI. Prevents Microsoft from quietly installing promoted apps. Default: enabled on consumer Windows.",
            Tags = ["ai", "debloat", "content-delivery", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
    ];
}

// ── Copilot+ PC Features ──────────────────────────────────────────────────────
// Merged from CopilotPlus.cs (NPU, Recall advanced, AI Paint, Live Captions AI)

internal static class CopilotPlus
{
    // Snapshots / Recall advanced controls
    private const string RecallAdv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

    // NPU / AI runtime
    private const string NpuPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI";
    private const string NpuSched = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\AI";

    // Live Captions AI enhancement
    private const string CaptionsPol = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Accessibility\LiveCaptions";

    // AI Writing Assistant in Office/OneNote
    private const string WritingAi = @"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\AI";

    // AI Phonetic Suggestions (IME / Text Input)
    private const string ImeAi = @"HKEY_CURRENT_USER\Software\Microsoft\InputMethod\Settings\CHS";

    // Paint Cocreator (AI image generation inside Paint)
    private const string PaintCo = @"HKEY_CURRENT_USER\Software\Microsoft\Paint\Capabilities";

    // Bing Visual Search / AI enhanced search in Explorer
    private const string ExplorerAi = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Exp\Explorer\Advanced";

    // Copilot+ Key remapping preference (the dedicated Copilot key on new hardware)
    private const string CopilotKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\current\device\WindowsAI";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cplplus-disable-recall-snapshots",
            Label = "Disable Windows Recall Snapshot Storage",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "recall", "privacy", "npu", "ai"],
            Description =
                "Prevents Windows Recall from storing encrypted screenshots "
                + "(snapshots) of your screen activity. The feature remains installed "
                + "but the continuous screen capture pipeline is stopped.",
            ApplyOps = [RegOp.SetDword(RecallAdv, "DisableAIDataAnalysis", 1), RegOp.SetDword(RecallAdv, "AllowRecallEnablement", 0)],
            RemoveOps = [RegOp.DeleteValue(RecallAdv, "DisableAIDataAnalysis"), RegOp.DeleteValue(RecallAdv, "AllowRecallEnablement")],
            DetectOps = [RegOp.CheckDword(RecallAdv, "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-npu-inference-policy",
            Label = "Disable NPU Inference Scheduling",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["copilot+", "npu", "ai", "performance"],
            Description =
                "Disables the Windows AI inference scheduling policy that routes "
                + "workloads to the NPU. Forces all AI inference to the CPU/GPU, "
                + "which may be faster for GPU-accelerated models (e.g., NVIDIA CUDA).",
            ApplyOps = [RegOp.SetDword(NpuPolicy, "AllowNPUInference", 0)],
            RemoveOps = [RegOp.DeleteValue(NpuPolicy, "AllowNPUInference")],
            DetectOps = [RegOp.CheckDword(NpuPolicy, "AllowNPUInference", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-npu-always-on",
            Label = "Disable NPU Always-On Mode",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "npu", "ai", "battery", "energy"],
            Description =
                "Stops the NPU from remaining active in an always-on standby state "
                + "to serve background AI features. Reduces idle power draw on Copilot+ "
                + "PCs with Qualcomm or Intel NPUs.",
            ApplyOps = [RegOp.SetDword(NpuSched, "NPUAlwaysOnEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(NpuSched, "NPUAlwaysOnEnabled")],
            DetectOps = [RegOp.CheckDword(NpuSched, "NPUAlwaysOnEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-ai-enhanced-live-captions",
            Label = "Disable AI-Enhanced Live Captions",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "live captions", "ai", "accessibility"],
            Description =
                "Turns off the AI-powered noise-suppression and speaker-diarization "
                + "enhancements in Live Captions. Reverts to the standard offline "
                + "speech-to-text engine, which uses less CPU and RAM.",
            ApplyOps = [RegOp.SetDword(CaptionsPol, "AIEnhancedCaptions", 0)],
            RemoveOps = [RegOp.DeleteValue(CaptionsPol, "AIEnhancedCaptions")],
            DetectOps = [RegOp.CheckDword(CaptionsPol, "AIEnhancedCaptions", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-ai-writing-suggestions",
            Label = "Disable AI Writing Suggestions in Office Apps",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "office", "writing", "privacy"],
            Description =
                "Prevents Microsoft Office (Word, Outlook, OneNote) from using the "
                + "AI Writing Assistant to auto-complete sentences and suggest edits. "
                + "Also prevents local text from being sent to Copilot cloud services.",
            ApplyOps = [RegOp.SetDword(WritingAi, "InlineAIEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WritingAi, "InlineAIEnabled")],
            DetectOps = [RegOp.CheckDword(WritingAi, "InlineAIEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-paint-cocreator",
            Label = "Disable Paint Cocreator (AI Image Generation)",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "paint", "image generation", "privacy"],
            Description =
                "Removes the Cocreator panel from Microsoft Paint that connects to "
                + "DALL-E / Bing Image Creator for AI-generated images. Prevents Paint "
                + "from making outbound requests during normal use.",
            ApplyOps = [RegOp.SetDword(PaintCo, "CoCreatorEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(PaintCo, "CoCreatorEnabled")],
            DetectOps = [RegOp.CheckDword(PaintCo, "CoCreatorEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-explorer-ai-search",
            Label = "Disable AI-Enhanced Search in File Explorer",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "explorer", "search", "privacy"],
            Description =
                "Disables the AI-powered semantic search feature in File Explorer "
                + "(natural language file searches). Reverts to traditional indexed "
                + "search, which is faster for exact-match queries.",
            ApplyOps = [RegOp.SetDword(ExplorerAi, "EnableAISemanticSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(ExplorerAi, "EnableAISemanticSearch")],
            DetectOps = [RegOp.CheckDword(ExplorerAi, "EnableAISemanticSearch", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-copilot-key-action",
            Label = "Disable Copilot Key Hardware Button",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "keyboard", "copilot key"],
            Description =
                "Disables the system action triggered by the dedicated Copilot key "
                + "present on Copilot+ certified keyboards. The key press produces no "
                + "action (can be remapped separately with PowerToys).",
            ApplyOps = [RegOp.SetDword(CopilotKey, "TurnOffWindowsCopilot", 1)],
            RemoveOps = [RegOp.DeleteValue(CopilotKey, "TurnOffWindowsCopilot")],
            DetectOps = [RegOp.CheckDword(CopilotKey, "TurnOffWindowsCopilot", 1)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-ime-ai-suggestions",
            Label = "Disable AI Phonetic IME Suggestions",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "ime", "keyboard", "input"],
            Description =
                "Turns off the AI-powered phonetic and contextual input suggestions "
                + "in the Windows Chinese (Simplified) IME. Reduces background model "
                + "inference on Copilot+ PCs with East-Asian language packs.",
            ApplyOps = [RegOp.SetDword(ImeAi, "UseAISuggestions", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeAi, "UseAISuggestions")],
            DetectOps = [RegOp.CheckDword(ImeAi, "UseAISuggestions", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-recall-indexing",
            Label = "Disable Recall Search Indexing of Snapshots",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "recall", "privacy", "indexing", "ai"],
            Description =
                "Stops the Recall search pipeline from indexing the encrypted "
                + "snapshot database even if Recall capture is still active. "
                + "Prevents the AI content-extraction process from running on stored "
                + "screenshots, reducing CPU usage and privacy exposure.",
            ApplyOps = [RegOp.SetDword(RecallAdv, "DisableRecallSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(RecallAdv, "DisableRecallSearch")],
            DetectOps = [RegOp.CheckDword(RecallAdv, "DisableRecallSearch", 1)],
        },
    ];
}
