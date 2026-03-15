namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Windows Recall / AI features — disables Recall snapshots, AI-powered search,
/// Copilot+ features, and related telemetry in Windows 11 24H2+.
/// </summary>
internal static class WindowsRecall
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string CuKey = @"HKEY_CURRENT_USER";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "recall-disable-recall",
            Label = "Disable Windows Recall",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Disables Windows Recall (AI-powered timeline snapshots) system-wide via Group Policy.",
            Tags = ["recall", "ai", "privacy", "copilot-plus", "snapshots"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-saving-snapshots",
            Label = "Disable Recall Snapshot Saving",
            Category = "Windows Recall",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Disables saving of Recall snapshots for the current user.",
            Tags = ["recall", "ai", "privacy", "snapshots"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableRecallSaving", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableRecallSaving")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableRecallSaving", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-ai-suggestions",
            Label = "Disable AI Suggestions in Start",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered suggested content and recommendations in Start menu.",
            Tags = ["recall", "ai", "start-menu", "suggestions"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "TurnOffWindowsCopilot", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "TurnOffWindowsCopilot")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "TurnOffWindowsCopilot", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-semantic-indexing",
            Label = "Disable Semantic Indexing",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Disables the AI semantic indexing component used by Recall and enhanced search.",
            Tags = ["recall", "ai", "indexing", "performance", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowSemanticIndexing", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowSemanticIndexing")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowSemanticIndexing", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-cocreator",
            Label = "Disable Cocreator in Paint",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the AI Cocreator feature in Microsoft Paint.",
            Tags = ["recall", "ai", "paint", "cocreator"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableCocreator", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableCocreator")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableCocreator", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-image-creator",
            Label = "Disable Image Creator in Paint",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the AI Image Creator feature in Microsoft Paint.",
            Tags = ["recall", "ai", "paint", "image-creator"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableImageCreator", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableImageCreator")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableImageCreator", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-generative-fill",
            Label = "Disable Generative Fill in Photos",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the AI generative fill/erase feature in Microsoft Photos.",
            Tags = ["recall", "ai", "photos", "generative"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos", "DisableGenerativeFill", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos", "DisableGenerativeFill")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos", "DisableGenerativeFill", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-ai-in-notepad",
            Label = "Disable AI Features in Notepad",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered rewrite and summarize features in Windows Notepad.",
            Tags = ["recall", "ai", "notepad", "copilot"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad", "DisableAI", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad", "DisableAI")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad", "DisableAI", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-web-content-eval",
            Label = "Disable Web Content Evaluation",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Prevents Windows from sending web content to Microsoft for AI analysis.",
            Tags = ["recall", "ai", "privacy", "web", "telemetry"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableWebContentEvaluation", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableWebContentEvaluation")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableWebContentEvaluation", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-cross-device-resume",
            Label = "Disable Cross-Device Resume (AI)",
            Category = "Windows Recall",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered cross-device activity resume and handoff.",
            Tags = ["recall", "ai", "cross-device", "privacy", "sync"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpSessionUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpSessionUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpSessionUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-ai-search-highlights",
            Label = "Disable AI Search Highlights",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-generated search highlights and trending content in Windows Search.",
            Tags = ["recall", "ai", "search", "highlights", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-inking-and-typing-personalization",
            Label = "Disable Inking & Typing AI Personalization",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AI-powered inking and typing personalization that sends data to Microsoft.",
            Tags = ["recall", "ai", "privacy", "typing", "inking", "personalization"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-activity-history",
            Label = "Disable Activity History",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables activity history collection and timeline features used by Recall.",
            Tags = ["recall", "ai", "privacy", "activity", "timeline"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-voice-activation",
            Label = "Disable Voice Activation for AI",
            Category = "Windows Recall",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Prevents AI assistants from listening for voice activation keywords.",
            Tags = ["recall", "ai", "privacy", "voice", "microphone"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps",
                    "AgentActivationEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps",
                    "AgentActivationEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps",
                    "AgentActivationEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "recall-disable-online-tips",
            Label = "Disable Online Tips & Suggestions",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables cloud-based tips and suggestions throughout Windows.",
            Tags = ["recall", "ai", "tips", "suggestions", "cloud"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "AllowOnlineTips", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer", "AllowOnlineTips")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "AllowOnlineTips", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-copilot-key",
            Label = "Disable Copilot Keyboard Key",
            Category = "Windows Recall",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the dedicated Copilot key on supported keyboards.",
            Tags = ["recall", "ai", "copilot", "keyboard", "hardware"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotKey", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotKey")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotKey", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-suggested-actions",
            Label = "Disable AI Suggested Actions",
            Category = "Windows Recall",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered suggested actions when copying dates, phone numbers, or addresses.",
            Tags = ["recall", "ai", "clipboard", "suggestions"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1)],
        },
    ];
}
