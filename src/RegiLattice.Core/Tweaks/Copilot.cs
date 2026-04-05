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
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-bing-chat",
            Label = "Disable Bing Chat in Search",
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-ai-start-suggestions",
            Label = "Disable AI-Powered Suggestions in Settings",
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-copilot-runtime-24h2",
            Label = "Disable Copilot Runtime (24H2)",
            Category = "AI / Copilot 1",
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
            Id = "ai-copilot-ineligible",
            Label = "Block Copilot User Eligibility",
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-copilot-hardware-key",
            Label = "Disable Copilot Hardware Key",
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-activity-history-upload",
            Label = "Disable Activity History Cloud Upload",
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-windows-tips",
            Label = "Disable Windows Tips & Suggestions (GPO)",
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Id = "ai-disable-lock-screen-overlay",
            Label = "Disable Lock Screen Spotlight Overlay Facts",
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
            Category = "AI / Copilot 1",
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
    ];
}
