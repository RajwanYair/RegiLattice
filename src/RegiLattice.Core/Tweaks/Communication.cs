namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Communication
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "comm-disable-teams-autostart-user",
            Label = "Disable Teams Auto-Start",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Microsoft Teams from starting automatically at login.",
            Tags = ["teams", "autostart", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", @"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "com.squirrel.Teams.Teams"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart", 1),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-gpu",
            Label = "Disable Teams GPU Acceleration",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables GPU hardware acceleration in Teams to reduce resource usage.",
            Tags = ["teams", "performance", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableGpu", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableGpu")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableGpu", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-discord-autostart",
            Label = "Disable Discord Auto-Start",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Discord from starting automatically at login.",
            Tags = ["discord", "autostart", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", @"HKEY_CURRENT_USER\Software\Discord"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Discord"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Discord", "DisableAutoStart", 1),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Discord", "DisableAutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Discord", "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-discord-hwaccel",
            Label = "Disable Discord HW Acceleration",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables hardware acceleration in Discord to reduce GPU usage.",
            Tags = ["discord", "performance", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Discord"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Discord", "DANGEROUS_ENABLE_DEVTOOLS_ONLY_ENABLE_IF_YOU_KNOW_WHAT_YOURE_DOING", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Discord", "disableHardwareAcceleration", 1),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Discord", "disableHardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Discord", "disableHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-spotify-autostart",
            Label = "Disable Spotify Auto-Start",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Spotify from starting automatically at login.",
            Tags = ["spotify", "autostart", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", @"HKEY_CURRENT_USER\Software\Spotify"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Spotify"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Spotify", "DisableAutoStart", 1),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Spotify", "DisableAutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Spotify", "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-spotify-hwaccel",
            Label = "Disable Spotify HW Acceleration",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables hardware acceleration in Spotify.",
            Tags = ["spotify", "performance", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Spotify"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Spotify", "ui.hardware_acceleration", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Spotify", "ui.hardware_acceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Spotify", "ui.hardware_acceleration", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-slack-autostart",
            Label = "Disable Slack Auto-Start",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Slack from starting automatically at login.",
            Tags = ["slack", "autostart", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", @"HKEY_CURRENT_USER\Software\Slack"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "com.squirrel.slack.slack"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Slack", "DisableAutoStart", 1),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Slack", "DisableAutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Slack", "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-telemetry",
            Label = "Disable Teams Telemetry",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Microsoft Teams telemetry and diagnostic data collection.",
            Tags = ["teams", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-slack-hwaccel",
            Label = "Disable Slack HW Acceleration",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables hardware acceleration in Slack desktop client.",
            Tags = ["slack", "performance", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Slack"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Slack", "HardwareAccelerationEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Slack", "HardwareAccelerationEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Slack", "HardwareAccelerationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-autostart",
            Label = "Disable Teams Auto-Start (Policy)",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Microsoft Teams from starting automatically at login. Reduces boot time and memory usage. Default: Auto-start. Recommended: Disabled.",
            Tags = ["communication", "teams", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "com.squirrel.Teams.Teams"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart", 1),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-skype-telemetry",
            Label = "Disable Skype Telemetry",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Skype for Business telemetry and diagnostic data collection. Default: Enabled. Recommended: Disabled.",
            Tags = ["communication", "skype", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-typing-insights",
            Label = "Disable Typing Insights",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables typing insights and suggestions that analyze your typing patterns. Improves privacy. Default: Enabled. Recommended: Disabled.",
            Tags = ["communication", "typing", "insights", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings", "InsightsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings", "InsightsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings", "InsightsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-online-speech",
            Label = "Disable Online Speech Recognition",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables online speech recognition that sends voice data to Microsoft for processing. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["communication", "speech", "voice", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-skype-telemetry-user",
            Label = "Disable Skype Telemetry (User)",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Skype desktop telemetry data collection at the user level. Reduces background data transmission. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["communication", "skype", "telemetry", "privacy", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Skype\Telemetry"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-webex-autostart",
            Label = "Disable Webex Autostart",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Cisco Webex from starting automatically at login. Default: enabled.",
            Tags = ["communication", "webex", "autostart", "cisco"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CiscoWebExStart")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run",
                    "CiscoWebExStart",
                    @"%LOCALAPPDATA%\WebEx\WebexHost.exe"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CiscoWebExStart")],
        },
        new TweakDef
        {
            Id = "comm-disable-discord-open-links",
            Label = "Disable Discord Open on URL Click",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Discord from registering as the default handler for discord:// links. Default: registered.",
            Tags = ["communication", "discord", "url", "handler"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Discord"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Discord", "OPEN_ON_STARTUP", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Discord", "OPEN_ON_STARTUP")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Discord", "OPEN_ON_STARTUP", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-gpu-acceleration",
            Label = "Disable Teams GPU Acceleration",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables GPU hardware acceleration in Microsoft Teams. Can fix display glitches. Default: enabled.",
            Tags = ["communication", "teams", "gpu", "hardware-acceleration"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Teams", "disableGpu", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Teams", "disableGpu")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Teams", "disableGpu", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-autostart",
            Label = "Disable Zoom Auto-Start",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Zoom from starting automatically at Windows login. Default: starts with Windows.",
            Tags = ["communication", "zoom", "autostart", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "ZoomAutoUpdater")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "ZoomAutoUpdater",
                    "\"C:\\Users\\%USERNAME%\\AppData\\Roaming\\Zoom\\bin\\ZoomAutoUpdater.exe\""
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "ZoomAutoUpdater")],
        },
        new TweakDef
        {
            Id = "comm-disable-slack-hardware-accel",
            Label = "Disable Slack Hardware Acceleration",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables hardware acceleration in Slack. Reduces GPU usage on integrated graphics. Default: enabled.",
            Tags = ["communication", "slack", "hardware", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Slack"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Slack", "HardwareAcceleration", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Slack", "HardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Slack", "HardwareAcceleration", 0)],
        },
        new TweakDef
        {
            Id = "comm-block-zoom-auto-update",
            Label = "Block Zoom Auto-Update",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Zoom from automatically updating. Prevents background download and installation of Zoom updates. Default: auto-updates.",
            Tags = ["communication", "zoom", "update", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom Meetings\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom Meetings\General", "EnableSilentAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom Meetings\General", "EnableSilentAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom Meetings\General", "EnableSilentAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-skype-feedback",
            Label = "Disable Skype Feedback Surveys",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Skype call quality feedback survey prompts. Prevents post-call rating popups. Default: enabled.",
            Tags = ["communication", "skype", "feedback", "survey"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Skype"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Skype", "DisableFeedback", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Skype", "DisableFeedback")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Skype", "DisableFeedback", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-background",
            Label = "Disable Teams Background Startup",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Microsoft Teams from starting in the background on Windows startup. Frees memory and bandwidth. Default: starts on login.",
            Tags = ["communication", "teams", "background", "startup"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\MSTeams_8wekyb3d8bbwe\TeamsTfwStartupTask",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\MSTeams_8wekyb3d8bbwe\TeamsTfwStartupTask",
                    "State",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\MSTeams_8wekyb3d8bbwe\TeamsTfwStartupTask",
                    "State",
                    2
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\MSTeams_8wekyb3d8bbwe\TeamsTfwStartupTask",
                    "State",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-auto-video",
            Label = "Disable Zoom Auto-Start Video",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic video start when joining Zoom meetings. Camera stays off until manually enabled. Default: auto-start.",
            Tags = ["communication", "zoom", "video", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General", "EnableAutoTurnOffVideo", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General", "EnableAutoTurnOffVideo")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General", "EnableAutoTurnOffVideo", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-autoupdate",
            Label = "Disable Zoom Auto-Update (User)",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Zoom auto-update checks at the user level. Stops automatic download prompts. Default: enabled.",
            Tags = ["communication", "zoom", "autoupdate", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General", "EnableAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General", "EnableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\General", "EnableAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-chat-notify",
            Label = "Disable Zoom Chat Notifications",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Zoom chat message notifications. Reduces distractions during meetings. Default: enabled.",
            Tags = ["communication", "zoom", "chat", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\Chat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\Chat", "EnableChatNotification", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\Chat", "EnableChatNotification")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Zoom\Zoom Meetings\Chat", "EnableChatNotification", 0)],
        },
        new TweakDef
        {
            Id = "comm-prevent-teams-first-launch",
            Label = "Prevent Teams Auto-Install on First Login",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Microsoft Teams from being automatically installed on first user login. Blocks the Teams Chat icon provisioning. Default: auto-installs.",
            Tags = ["communication", "teams", "install", "provisioning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Communications"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Communications", "ConfigureChatAutoInstall", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Communications", "ConfigureChatAutoInstall"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Communications", "ConfigureChatAutoInstall", 0),
            ],
        },
    ];
}
