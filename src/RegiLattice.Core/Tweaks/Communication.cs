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
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "comm-disable-teams-read-receipts",
            Label = "Disable Teams Read Receipts",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables read receipt delivery in Microsoft Teams conversations.",
            Tags = ["teams", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "ReadReceiptsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "ReadReceiptsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "ReadReceiptsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-presence-share",
            Label = "Disable Teams Presence Sharing",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Microsoft Teams from sharing your presence/availability status externally.",
            Tags = ["teams", "privacy", "presence"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "SharePresence", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "SharePresence")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "SharePresence", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-survey-bell",
            Label = "Disable Teams Survey Prompts",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Teams survey bell notification requesting feedback after meetings.",
            Tags = ["teams", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "ShowSurveyBell", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "ShowSurveyBell")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "ShowSurveyBell", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-animations",
            Label = "Disable Teams UI Animations",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables animated transitions in Microsoft Teams to improve rendering performance.",
            Tags = ["teams", "performance", "animations"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAnimations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAnimations", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-file-auto-download",
            Label = "Disable Teams File Auto-Download",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Teams from automatically downloading shared files to local storage.",
            Tags = ["teams", "files", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "EnableFileAutoDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "EnableFileAutoDownload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "EnableFileAutoDownload", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-recording-consent",
            Label = "Disable Zoom Recording Consent Alert",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the Zoom recording consent popup that appears when a host starts recording.",
            Tags = ["zoom", "recording"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us", "bDisableRecordingConsentAlert", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us", "bDisableRecordingConsentAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us", "bDisableRecordingConsentAlert", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-meeting-reminder",
            Label = "Disable Zoom Pre-Meeting Reminder",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off the Zoom reminder notification shown before scheduled meetings.",
            Tags = ["zoom", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us", "bEnableMeetingNotification", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us", "bEnableMeetingNotification")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\ZoomUX\zoom.us", "bEnableMeetingNotification", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-discord-rich-presence",
            Label = "Disable Discord Rich Presence",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Blocks Discord from showing game activity and rich presence data in your profile.",
            Tags = ["discord", "privacy", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Discord"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Discord", "IgnoredActivities", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Discord", "IgnoredActivities")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Discord", "IgnoredActivities", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-discord-browser-open",
            Label = "Disable Discord Browser Auto-Open",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Discord from silently launching a browser window for authentication flows.",
            Tags = ["discord"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Discord"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Discord", "browserLaunchAlwaysAsk", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Discord", "browserLaunchAlwaysAsk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Discord", "browserLaunchAlwaysAsk", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-skype-auto-start-run",
            Label = "Disable Skype Auto-Start",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Skype from the Windows startup registry key so it does not launch at login.",
            Tags = ["skype", "startup", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Skype")],
            // NOTE: No RemoveOps — we cannot restore a deleted Run entry; the Skype installer
            // must re-add it. Removal is intentionally one-directional.
            RemoveOps = [],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Skype")],
        },
    ];
}

// === Merged from: PhoneLink.cs ===


internal static class PhoneLink
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "phone-disable-phonelink",
            Label = "Disable Phone Link (Policy)",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft Phone Link (Your Phone) app via Group Policy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["phone-link", "your-phone", "privacy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "PhoneLinkEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "PhoneLinkEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "PhoneLinkEnabled", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-cross-device",
            Label = "Disable Cross-Device Experience",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Connected Devices Platform (CDP) that powers cross-device features. Default: Enabled.",
            Tags = ["phone-link", "cross-device", "cdp", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-cross-clipboard",
            Label = "Disable Cross-Device Clipboard",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops clipboard data from being shared between Windows and linked phone/tablet. Default: Enabled.",
            Tags = ["phone-link", "clipboard", "cross-device", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-phone-svc",
            Label = "Disable Phone Service (PhoneSvc)",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Phone service (telephony state management). Frees resources if Phone Link is not used.",
            Tags = ["phone-link", "service", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "phone-disable-cross-notifications",
            Label = "Disable Cross-Device Notifications",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops phone notifications from appearing on your Windows desktop. Default: Enabled.",
            Tags = ["phone-link", "notifications", "cross-device"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableNotificationSync", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableNotificationSync", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableNotificationSync", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-cdp-policy",
            Label = "Disable CDP Platform (Policy)",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Connected Devices Platform entirely via machine policy. Blocks all cross-device features.",
            Tags = ["phone-link", "cdp", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableCDP", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableCDP")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableCDP", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-app-launch",
            Label = "Disable Cross-Device App Launch",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents apps from being launched remotely across devices. Default: Enabled.",
            Tags = ["phone-link", "app-launch", "cross-device"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteLaunch", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteLaunch", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteLaunch", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-cross-sms",
            Label = "Disable Cross-Device SMS",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables SMS message sync between phone and PC. Default: Enabled.",
            Tags = ["phone-link", "sms", "sync", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableSmsSync", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableSmsSync", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableSmsSync", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-nearby-share",
            Label = "Disable Nearby Share",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Nearby Sharing feature (file/link transfer to nearby devices). Default: Enabled.",
            Tags = ["phone-link", "nearby-share", "sharing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-smartglass",
            Label = "Disable SmartGlass Companion",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Xbox SmartGlass companion features for cross-device gaming.",
            Tags = ["phone-link", "smartglass", "xbox", "cross-device"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass", "UserAuthPolicy", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass", "UserAuthPolicy", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass", "UserAuthPolicy", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-activity-upload",
            Label = "Disable Activity History Upload",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops Windows from uploading activity history to Microsoft cloud for Timeline and cross-device resume. Recommended.",
            Tags = ["phone-link", "activity", "timeline", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableActivityFeed", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "UploadUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableActivityFeed"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "UploadUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "UploadUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-cross-resume",
            Label = "Disable Cross-Device Resume",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the ability to resume activities on other devices. Default: Enabled.",
            Tags = ["phone-link", "cross-device", "resume"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableShare", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableShare", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableShare", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-bt-relay",
            Label = "Disable Bluetooth Phone Relay",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bluetooth relay used for phone-to-PC communication in Phone Link.",
            Tags = ["phone-link", "bluetooth", "relay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "BluetoothLastUsedSend", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "BluetoothLastUsedSend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "BluetoothLastUsedSend", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-wifidirect",
            Label = "Disable Phone Link Wi-Fi Direct",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Wi-Fi Direct transport used by Phone Link for high-speed cross-device data transfer. Reduces background radio use. Default: Enabled.",
            Tags = ["phone-link", "wifi", "wifi-direct", "network", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "WifiDirectEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "WifiDirectEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "WifiDirectEnabled", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-timeline",
            Label = "Disable Windows Timeline Activity Feed",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Timeline activity feed and cross-device clipboard via policy. Stops syncing browsing and app activity to Microsoft cloud. Default: Enabled.",
            Tags = ["phone-link", "timeline", "activity", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-photos-sync",
            Label = "Disable Phone Photos Auto-Sync",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic phone photo synchronisation via the Phone Link app. Prevents background syncing of photos to the PC. Default: Enabled.",
            Tags = ["phone-link", "photos", "sync", "privacy", "storage"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone", "DisablePhotoSync", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone", "DisablePhotoSync", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone", "DisablePhotoSync", 1)],
        },
        new TweakDef
        {
            Id = "phone-disable-phone-link-autostart",
            Label = "Disable Phone Link Autostart",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Phone Link app from starting with Windows. Default: autostart enabled.",
            Tags = ["phone-link", "autostart", "startup", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\PersistedStorageItemTable\ManagedByApp",
                    "AutoStartEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\PersistedStorageItemTable\ManagedByApp",
                    "AutoStartEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\PersistedStorageItemTable\ManagedByApp",
                    "AutoStartEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "phone-disable-cdp-service",
            Label = "Disable Connected Devices Platform Service",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Connected Devices Platform service used by Phone Link, smartwatch, and cross-device features. Default: auto.",
            Tags = ["phone-link", "cdp", "service", "cross-device"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "phone-disable-handoff-notifications",
            Label = "Disable Phone Notification Mirroring",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables mirroring phone notifications to the PC. Default: enabled.",
            Tags = ["phone-link", "notifications", "mirror", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteNotifications", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteNotifications", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteNotifications", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-message-sync",
            Label = "Disable Phone Link Message Sync",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables syncing SMS/MMS messages from phone to PC. Default: enabled.",
            Tags = ["phone-link", "messages", "sms", "sync"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteMessages", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteMessages", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteMessages", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-nearby-sharing",
            Label = "Disable Nearby Sharing",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Nearby sharing over Bluetooth. Prevents file/link sharing with nearby devices. Default: disabled.",
            Tags = ["phone-link", "nearby", "sharing", "bluetooth"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-app-notifications",
            Label = "Disable Phone Link Notifications",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables notification mirroring from Phone Link. Prevents phone notifications from appearing on the PC. Default: enabled.",
            Tags = ["phone-link", "notifications", "mirror", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteNotifications", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteNotifications", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-suggestions",
            Label = "Disable Phone Link Suggestions",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Phone Link from showing app suggestions and promotions. Default: enabled.",
            Tags = ["phone-link", "suggestions", "promotions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-alljoyn-router",
            Label = "Disable AllJoyn Router Service",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the AllJoyn Router Service used for IoT and phone device discovery protocols. Frees resources if Phone Link is unused.",
            Tags = ["phone-link", "alljoyn", "service", "iot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter", "Start", 4)],
        },
        new TweakDef
        {
            Id = "phone-disable-wpd-service",
            Label = "Disable Windows Portable Devices Service",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Portable Devices (WPD) service used for MTP/PTP phone connections. Frees resources if no phone/camera is connected via USB.",
            Tags = ["phone-link", "wpd", "mtp", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WPDSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WPDSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WPDSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WPDSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "phone-disable-link-to-windows-banner",
            Label = "Disable Link to Windows Promo Banner",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the 'Link to Windows' promotional banners displayed in apps and Start via Group Policy.",
            Tags = ["phone-link", "banner", "promotion", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "DisableLinkToWindowsBanner", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "DisableLinkToWindowsBanner")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "DisableLinkToWindowsBanner", 1)],
        },
        new TweakDef
        {
            Id = "phone-disable-continue-on-pc",
            Label = "Disable Continue on PC Feature",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the 'Continue on PC' option in share menus on mobile devices paired with this PC.",
            Tags = ["phone-link", "continue-on-pc", "cross-device", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "ContinueOnPCConversionEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "ContinueOnPCConversionEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "ContinueOnPCConversionEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "phone-disable-phone-activation-policy",
            Label = "Disable Phone Activation via Policy",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks Windows from activating via linked mobile phones (e.g. BYOD scenarios) through Group Policy.",
            Tags = ["phone-link", "activation", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneActivation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneActivation", "AllowPhoneActivation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneActivation", "AllowPhoneActivation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneActivation", "AllowPhoneActivation", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-device-assoc-svc",
            Label = "Disable Device Association Framework Service",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Device Association Framework Provider Host service (DeviceAssociationService) used for pairing phones and peripheral devices.",
            Tags = ["phone-link", "device-pairing", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DAssocSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DAssocSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DAssocSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DAssocSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "phone-disable-cdp-prompt",
            Label = "Disable CDP Activation Prompt",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from showing prompts to activate Connected Device Platform (CDP) features when a new phone is detected.",
            Tags = ["phone-link", "cdp", "prompt", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpPromptBeforeActivation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpPromptBeforeActivation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpPromptBeforeActivation", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-roam-trigger-consent",
            Label = "Disable Cross-Device Roaming Trigger Consent",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Revokes consent for CDP service activation triggers that roam across paired devices.",
            Tags = ["phone-link", "cdp", "roaming", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpServiceActivationTriggerConsented", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpServiceActivationTriggerConsented", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpServiceActivationTriggerConsented", 0),
            ],
        },
        new TweakDef
        {
            Id = "phone-disable-hotspot-auth",
            Label = "Disable Wi-Fi Hotspot Authentication",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic Wi-Fi hotspot authentication between linked devices. Prevents phone hotspot data from being shared automatically.",
            Tags = ["phone-link", "wifi", "hotspot", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication", "AllowHotspot", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication", "AllowHotspot")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication", "AllowHotspot", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-windows-hello-companion",
            Label = "Disable Windows Hello Companion Device",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents using a phone as a Windows Hello companion authentication device (remote unlock). Default: Enabled.",
            Tags = ["phone-link", "windows-hello", "authentication", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\Remote"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\Remote", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\Remote", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\Remote", "Enabled", 0)],
        },
    ];
}


// ── merged from PolicyCommunication.cs ──
// RegiLattice.Core — Tweaks/PolicyCommunication.cs
// Microsoft Teams, conferencing, telephony, NetMeeting legacy, and voice quality policies
// Category: "Communication Policy"
// Consolidated from 8 modules.

internal static class PolicyCommunication
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _ConferencingBandwidthPolicy.Data,
            .. _ConferencingPolicy.Data,
            .. _NetMeetingPolicy.Data,
            .. _TeamsAdvanced.Data,
            .. _TeamsCallingPolicy.Data,
            .. _TeamsMeetingAudioPolicy.Data,
            .. _TeamsMessagingPolicy.Data,
            .. _TelephonyPolicy.Data,
        ];

    // ── ConferencingBandwidthPolicy ──
    private static class _ConferencingBandwidthPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "confbw-cap-max-video-resolution-720p",
                Label = "Conferencing BW: Cap Maximum Video Resolution at 720p",
                Category = "Communication",
                Description = "Sets MaxVideoResolution=540 in Teams policy. Limits outbound camera video to 720p HD (1280x720) per participant rather than allowing uncapped 1080p Full HD. " +
                    "On office networks with multiple concurrent video calls, permitting 1080p per user (3–5 Mbps) versus 720p (1–1.5 Mbps) can triple per-user bandwidth consumption. " +
                    "Capping at 720p maintains good call quality while substantially reducing aggregate bandwidth demand across the org.",
                Tags = ["teams", "video", "resolution", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Caps outbound video at 720p; ~50-65% bandwidth saving per participant versus uncapped 1080p calls.",
                ApplyOps = [RegOp.SetDword(Key, "MaxVideoResolution", 540)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxVideoResolution")],
                DetectOps = [RegOp.CheckDword(Key, "MaxVideoResolution", 540)],
            },
            new TweakDef
            {
                Id = "confbw-set-max-call-bandwidth-1500kbps",
                Label = "Conferencing BW: Set Maximum Per-Call Bandwidth to 1500 Kbps",
                Category = "Communication",
                Description = "Sets MaxCallBitsPerSecond=1500000 (1.5 Mbps) in Teams policy. Sets an absolute ceiling on the total bandwidth consumed by a single Teams audio+video call. " +
                    "At 1.5 Mbps the call can sustain 720p video and high-fidelity audio with comfortable headroom. Without this cap, Teams adaptively scales to fill all available bandwidth including on uncongested gigabit networks, crowding out background file transfers and other services.",
                Tags = ["teams", "bandwidth", "cap", "qos", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Caps per-call total bandwidth at 1.5 Mbps; prevents single calls from consuming excessive capacity.",
                ApplyOps = [RegOp.SetDword(Key, "MaxCallBitsPerSecond", 1500000)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxCallBitsPerSecond")],
                DetectOps = [RegOp.CheckDword(Key, "MaxCallBitsPerSecond", 1500000)],
            },
            new TweakDef
            {
                Id = "confbw-set-content-share-bandwidth-500kbps",
                Label = "Conferencing BW: Set Maximum Content-Sharing Bandwidth to 500 Kbps",
                Category = "Communication",
                Description = "Sets ContentSharingBitsPerSecond=500000 (500 Kbps) in Teams policy. Limits the bandwidth available for desktop and application sharing streams to 500 Kbps. " +
                    "Screen share generates high-frequency updates on busy screens (IDEs, spreadsheets, PowerPoint animations) which can spike to 10+ Mbps without a cap. " +
                    "500 Kbps delivers smooth sharing for most presentation use cases while preventing screen share from saturating call bandwidth.",
                Tags = ["teams", "screenshare", "bandwidth", "cap", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Caps content-sharing at 500 Kbps; prevents screen-share spikes from degrading meeting audio/video.",
                ApplyOps = [RegOp.SetDword(Key, "ContentSharingBitsPerSecond", 500000)],
                RemoveOps = [RegOp.DeleteValue(Key, "ContentSharingBitsPerSecond")],
                DetectOps = [RegOp.CheckDword(Key, "ContentSharingBitsPerSecond", 500000)],
            },
            new TweakDef
            {
                Id = "confbw-disable-hd-1080p-outbound-video",
                Label = "Conferencing BW: Disable 1080p Full HD Outbound Video",
                Category = "Communication",
                Description = "Sets AllowHD1080p=0 in Teams policy. Explicitly disables sending 1080p video from the local camera during Teams meetings. " +
                    "This is a secondary control that works alongside MaxVideoResolution. DisableHD1080p is evaluated at the Teams client layer, while MaxVideoResolution is evaluated by the media negotiation. " +
                    "Setting both prevents 1080p video from being negotiated even on high-capacity links where the resolution cap alone may be overridden.",
                Tags = ["teams", "video", "1080p", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables 1080p video at client layer; complementary control to MaxVideoResolution for dual enforcement.",
                ApplyOps = [RegOp.SetDword(Key, "AllowHD1080p", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowHD1080p")],
                DetectOps = [RegOp.CheckDword(Key, "AllowHD1080p", 0)],
            },
            new TweakDef
            {
                Id = "confbw-set-screen-share-max-framerate-15",
                Label = "Conferencing BW: Cap Screen-Share Frame Rate at 15 FPS",
                Category = "Communication",
                Description = "Sets ScreenSharingFrameRate=15 in Teams policy. Reduces the maximum frame rate for Teams desktop and application sharing from the default (up to 30 FPS) to 15 FPS. " +
                    "For typical presentation and document review use cases, 15 FPS is indistinguishable from 30 FPS. The bandwidth saving is proportional: halving frame rate nearly halves the constant stream bitrate for static content and substantially reduces peak rates during screen transitions.",
                Tags = ["teams", "screenshare", "framerate", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "15 FPS screen share; unnoticeable for presentations, ~40-50% bandwidth saving versus 30 FPS.",
                ApplyOps = [RegOp.SetDword(Key, "ScreenSharingFrameRate", 15)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScreenSharingFrameRate")],
                DetectOps = [RegOp.CheckDword(Key, "ScreenSharingFrameRate", 15)],
            },
            new TweakDef
            {
                Id = "confbw-disable-together-mode-video",
                Label = "Conferencing BW: Disable Together Mode Video Layout",
                Category = "Communication",
                Description = "Sets AllowTogetherMode=0 in Teams policy. Disables the Together Mode virtual background layout that places all participants in a shared scene. " +
                    "Together Mode requires high-resolution video feeds from all participants and performs client-side compositing. On meetings with 10+ participants this doubles effective video bandwidth versus gallery view. " +
                    "Disabling it is a straightforward bandwidth saving for large meetings on constrained networks.",
                Tags = ["teams", "video", "together-mode", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Together Mode; reduces video compositing overhead and bandwidth in large meetings.",
                ApplyOps = [RegOp.SetDword(Key, "AllowTogetherMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTogetherMode")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTogetherMode", 0)],
            },
            new TweakDef
            {
                Id = "confbw-disable-panorama-video",
                Label = "Conferencing BW: Disable Panoramic Room Video",
                Category = "Communication",
                Description = "Sets AllowPanoramaVideo=0 in Teams policy. Disables the panoramic (wide-angle) room video mode available on Teams Rooms devices. " +
                    "Panoramic video streams require significantly higher resolution and frame rates than standard participant video. For most remote participants, a standard camera view of the room is functionally equivalent. " +
                    "Disabling panoramic saves 30–50% of per-room outbound bandwidth.",
                Tags = ["teams", "video", "panorama", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables panoramic room video; ~30-50% outbound bandwidth saving on Teams Rooms devices.",
                ApplyOps = [RegOp.SetDword(Key, "AllowPanoramaVideo", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowPanoramaVideo")],
                DetectOps = [RegOp.CheckDword(Key, "AllowPanoramaVideo", 0)],
            },
            new TweakDef
            {
                Id = "confbw-enable-adaptive-bitrate-control",
                Label = "Conferencing BW: Enable Adaptive Bitrate Control for Calls",
                Category = "Communication",
                Description = "Sets EnableAdaptiveBitrateForCalling=1 in Teams policy. Enables the Teams adaptive bitrate algorithm to dynamically reduce video quality when packet loss or congestion is detected rather than maintaining maximum quality until the call breaks. " +
                    "Without adaptive bitrate the media engine attempts to hold resolution fixed, which causes burst packet loss and call freezes. With it, video gracefully degrades to audio-only before dropping the call.",
                Tags = ["teams", "video", "adaptive-bitrate", "resilience", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables adaptive bitrate; gracefully degrades video on congested links instead of call drops.",
                ApplyOps = [RegOp.SetDword(Key, "EnableAdaptiveBitrateForCalling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAdaptiveBitrateForCalling")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAdaptiveBitrateForCalling", 1)],
            },
            new TweakDef
            {
                Id = "confbw-set-auto-degrade-threshold-50pct",
                Label = "Conferencing BW: Trigger Auto Quality Downgrade at 50% Bandwidth",
                Category = "Communication",
                Description = "Sets AutoDegradeBandwidthThresholdPercent=50 in Teams policy. Configures the Teams media engine to start downgrading video resolution and frame rate once available bandwidth falls below 50% of the negotiated session maximum. " +
                    "An earlier trigger (50% vs. default 75%) gives the adaptive algorithm more headroom to reduce bitrate before packetloss becomes perceptible, resulting in smoother degradation rather than abrupt quality drops.",
                Tags = ["teams", "video", "adaptive-bitrate", "degradation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Earlier 50% threshold for quality degradation; smoother congestion response versus default 75% trigger.",
                ApplyOps = [RegOp.SetDword(Key, "AutoDegradeBandwidthThresholdPercent", 50)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoDegradeBandwidthThresholdPercent")],
                DetectOps = [RegOp.CheckDword(Key, "AutoDegradeBandwidthThresholdPercent", 50)],
            },
            new TweakDef
            {
                Id = "confbw-disable-immersive-spaces",
                Label = "Conferencing BW: Disable Teams Immersive Spaces (3D Metaverse)",
                Category = "Communication",
                Description = "Sets AllowImmersiveSpaces=0 in Teams policy. Disables the Teams Immersive Spaces feature which renders a 3D virtual meeting environment using the Mesh platform. " +
                    "Immersive Spaces require GPU-accelerated 3D rendering and a dedicated high-bandwidth video stream that is typically 2–4× the bandwidth of a standard gallery view call. " +
                    "Disabling this feature is appropriate for organisations where standard HD video meetings are the expected standard and 3D environments are unnecessary.",
                Tags = ["teams", "immersive", "mesh", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Teams 3D Immersive Spaces; saves 2-4× bandwidth versus standard video; GPU load reduced.",
                ApplyOps = [RegOp.SetDword(Key, "AllowImmersiveSpaces", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowImmersiveSpaces")],
                DetectOps = [RegOp.CheckDword(Key, "AllowImmersiveSpaces", 0)],
            },
        ];

    }

    // ── ConferencingPolicy ──
    private static class _ConferencingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Conferencing";
        private const string InvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Conferencing\Invitations";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "confer-disable-meeting-space",
                Label = "Conferencing Policy: Disable Windows Meeting Space",
                Category = "Communication",
                Description =
                    "Disables Windows Meeting Space (the Vista/7 peer-to-peer collaboration platform). Meeting Space connects devices via ad-hoc Wi-Fi or Bluetooth without authentication requirements. Disabling it removes a legacy unmanaged collaboration channel from the endpoint.",
                Tags = ["conferencing", "meeting-space", "p2p", "legacy", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMeetingSpace", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMeetingSpace")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMeetingSpace", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables legacy peer-to-peer Meeting Space; removes unauthenticated ad-hoc collaboration channel.",
            },
            new TweakDef
            {
                Id = "confer-disable-peer-invitations",
                Label = "Conferencing Policy: Disable Peer-to-Peer Meeting Invitations",
                Category = "Communication",
                Description =
                    "Prevents users from sending or receiving Windows Meeting Space invitations. Ad-hoc peer invitations in Windows Conferencing use People Near Me (PNRP) which broadcasts user presence on the local network without per-session authentication.",
                Tags = ["conferencing", "invitations", "p2p", "pnrp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InvKey],
                ApplyOps = [RegOp.SetDword(InvKey, "NoInvitations", 1)],
                RemoveOps = [RegOp.DeleteValue(InvKey, "NoInvitations")],
                DetectOps = [RegOp.CheckDword(InvKey, "NoInvitations", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents PNRP-based peer invitations that broadcast user presence without authentication.",
            },
            new TweakDef
            {
                Id = "confer-disable-session-hosting",
                Label = "Conferencing Policy: Disable Session Hosting for Collaborations",
                Category = "Communication",
                Description =
                    "Prevents the local machine from acting as a host for Windows Meeting Space sessions. Disabling hosting prevents the machine from accepting incoming PNRP peer connections that are used to establish collaboration sessions without requiring inbound firewall rules.",
                Tags = ["conferencing", "host", "pnrp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoHosting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoHosting")],
                DetectOps = [RegOp.CheckDword(Key, "NoHosting", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks machine from accepting inbound PNRP peer collaboration connections.",
            },
            new TweakDef
            {
                Id = "confer-disable-remote-app-sharing",
                Label = "Conferencing Policy: Disable Remote Application Sharing in Conferences",
                Category = "Communication",
                Description =
                    "Disables the remote application sharing capability within Windows conferencing sessions. Application sharing transmits screen content of individual windows to all session participants without per-participant audit logging.",
                Tags = ["conferencing", "app-sharing", "remote", "screen", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRemoteAppSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRemoteAppSharing")],
                DetectOps = [RegOp.CheckDword(Key, "NoRemoteAppSharing", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Closes unaudited application screen sharing in Windows conferencing sessions.",
            },
            new TweakDef
            {
                Id = "confer-disable-document-handouts",
                Label = "Conferencing Policy: Disable Document Handouts in Conferences",
                Category = "Communication",
                Description =
                    "Prevents Windows Meeting Space participants from distributing document handouts to other session members. Document handout distribution bypasses DLP controls because files are transferred over the PNRP peer channel rather than email or SharePoint.",
                Tags = ["conferencing", "documents", "handouts", "dlp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDocumentHandouts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDocumentHandouts")],
                DetectOps = [RegOp.CheckDword(Key, "NoDocumentHandouts", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents file distribution via PNRP channel that bypasses DLP controls.",
            },
            new TweakDef
            {
                Id = "confer-block-bandwidth-unlimited",
                Label = "Conferencing Policy: Enforce Maximum Bandwidth Limit for Sessions",
                Category = "Communication",
                Description =
                    "Enforces a bandwidth ceiling on Windows Conferencing sessions. Without a policy limit, conferencing sessions can saturate available network bandwidth affecting all other services sharing the network segment.",
                Tags = ["conferencing", "bandwidth", "limit", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxBandwidthKbps", 512)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxBandwidthKbps")],
                DetectOps = [RegOp.CheckDword(Key, "MaxBandwidthKbps", 512)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Caps conferencing bandwidth at 512 Kbps to prevent network saturation; adjust for high-bandwidth environments.",
            },
            new TweakDef
            {
                Id = "confer-disable-direct-p2p-connect",
                Label = "Conferencing Policy: Block Direct Peer-to-Peer Connections",
                Category = "Communication",
                Description =
                    "Forces Windows Conferencing to route all traffic through a relay server instead of establishing direct peer-to-peer connections. Direct P2P connections bypass network egress monitoring and expose internal IP addressing information to remote participants.",
                Tags = ["conferencing", "p2p", "relay", "network-monitoring", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDirectP2PConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDirectP2PConnections")],
                DetectOps = [RegOp.CheckDword(Key, "NoDirectP2PConnections", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Forces relay-routing; prevents direct P2P connections that bypass network egress monitoring.",
            },
            new TweakDef
            {
                Id = "confer-disable-people-near-me",
                Label = "Conferencing Policy: Disable People Near Me / PNRP Discovery",
                Category = "Communication",
                Description =
                    "Prevents the machine from broadcasting its presence to other machines on the local network via the People Near Me (PNRP) service used by Windows Conferencing. PNRP presence broadcasts reveal device names, user accounts, and network position to all devices on the subnet.",
                Tags = ["conferencing", "pnrp", "people-near-me", "discovery", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoPeopleNearMe", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoPeopleNearMe")],
                DetectOps = [RegOp.CheckDword(Key, "NoPeopleNearMe", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Stops PNRP presence broadcasts exposing device name and user account on the local subnet.",
            },
            new TweakDef
            {
                Id = "confer-disable-meeting-autostart",
                Label = "Conferencing Policy: Disable Windows Meeting Space Autostart",
                Category = "Communication",
                Description =
                    "Prevents Windows Meeting Space from automatically starting during user logon or when other conferencing-related events are triggered (such as projector connection). Autostart increases the attack surface by leaving the PNRP service active even when the user is not actively collaborating.",
                Tags = ["conferencing", "autostart", "startup", "pnrp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMeetingAutoStart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMeetingAutoStart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMeetingAutoStart", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents PNRP service activation on projector connect or login trigger events.",
            },
            new TweakDef
            {
                Id = "confer-disable-remember-passwords",
                Label = "Conferencing Policy: Disable Password Storage for Meeting Rooms",
                Category = "Communication",
                Description =
                    "Prevents Windows Conferencing from storing meeting room passwords in the credential manager or conference history. Cached meeting passwords can be extracted from the Windows credential store, allowing replay attacks against password-protected legacy meeting rooms.",
                Tags = ["conferencing", "password", "credential-manager", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRememberPasswords", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRememberPasswords")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRememberPasswords", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents meeting room passwords from being cached in Windows credential store.",
            },
        ];

    }

    // ── NetMeetingPolicy ──
    private static class _NetMeetingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\NetMeeting";
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\NetMeeting\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netmeet-disable-netmeeting",
                Label = "NetMeeting Policy: Disable NetMeeting Service",
                Category = "Communication",
                Description =
                    "Disables Microsoft NetMeeting entirely via the policy key. NetMeeting is a legacy Windows collaboration tool that should be disabled in all modern enterprise environments as it uses unencrypted legacy protocols (T.120, H.323) with no modern authentication support.",
                Tags = ["netmeeting", "legacy", "collaboration", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoNetMeeting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoNetMeeting")],
                DetectOps = [RegOp.CheckDword(Key, "NoNetMeeting", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Completely disables legacy NetMeeting; removes unencrypted T.120/H.323 attack surface.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-app-sharing",
                Label = "NetMeeting Policy: Disable Application Sharing",
                Category = "Communication",
                Description =
                    "Prevents NetMeeting from sharing application windows with remote participants. Application sharing over legacy NetMeeting is unencrypted and allows full control of the shared application without Windows authentication, making it a remote code execution risk.",
                Tags = ["netmeeting", "app-sharing", "remote", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoAppSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoAppSharing")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoAppSharing", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks unencrypted application screen sharing over legacy T.120 protocol.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-file-transfer",
                Label = "NetMeeting Policy: Disable File Transfer via NetMeeting",
                Category = "Communication",
                Description =
                    "Blocks the NetMeeting file transfer feature that allows participants to send files to each other during a conference. File transfer over NetMeeting bypasses DLP and AV controls and can be used for data exfiltration or malware delivery.",
                Tags = ["netmeeting", "file-transfer", "dlp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoFileTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoFileTransfer")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoFileTransfer", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Eliminates DLP-bypassing file transfer over NetMeeting peer channel.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-remote-desktop-sharing",
                Label = "NetMeeting Policy: Disable Remote Desktop Sharing",
                Category = "Communication",
                Description =
                    "Disables the Remote Desktop Sharing feature in NetMeeting that allows unattended remote access to a machine. NetMeeting RDS does not require Windows credentials, runs without encryption, and represents a complete remote takeover risk on any network where the port is reachable.",
                Tags = ["netmeeting", "remote-desktop", "rdp", "rds", "unattended", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoRemoteDesktopSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoRemoteDesktopSharing")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoRemoteDesktopSharing", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Closes unencrypted credential-free remote desktop sharing attack vector.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-whiteboard",
                Label = "NetMeeting Policy: Disable NetMeeting Whiteboard",
                Category = "Communication",
                Description =
                    "Disables the shared Whiteboard feature in NetMeeting. The whiteboard transmits screen content without encryption. Disabling it as part of a full NetMeeting hardening profile reduces the attack surface for legacy T.120 data channel exploits.",
                Tags = ["netmeeting", "whiteboard", "legacy", "t120", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoWhiteboard", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoWhiteboard")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoWhiteboard", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes shared whiteboard T.120 data channel; reduces legacy protocol attack surface.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-chat",
                Label = "NetMeeting Policy: Disable NetMeeting Chat",
                Category = "Communication",
                Description =
                    "Disables the chat feature in NetMeeting. Chat transmits messages in plaintext over the T.120 channel. On modern networks, legacy chat channels are potential exfiltration paths that bypass modern DLP solutions monitoring HTTPS or SMTP.",
                Tags = ["netmeeting", "chat", "plaintext", "exfiltration", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoChat", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoChat")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoChat", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Closes plaintext T.120 chat exfiltration path that bypasses modern DLP monitoring.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-directory-service",
                Label = "NetMeeting Policy: Disable ILS Directory Service Registration",
                Category = "Communication",
                Description =
                    "Prevents NetMeeting from registering the current user with an ILS (Internet Locator Service) directory. ILS directories expose the user's IP address and NetMeeting status to anyone querying the directory server, creating a reconnaissance risk.",
                Tags = ["netmeeting", "ils", "directory", "registration", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoDirectoryService", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoDirectoryService")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoDirectoryService", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops ILS directory registration exposing user IP and online presence to directory queries.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-audio",
                Label = "NetMeeting Policy: Disable NetMeeting Audio (VoIP)",
                Category = "Communication",
                Description =
                    "Disables the audio (VoIP) component of NetMeeting. NetMeeting audio uses unencrypted RTP streams, making all voice content trivially interceptable by any network observer. On corporate networks, all voice comms should be routed through encrypted platforms (Teams, Cisco).",
                Tags = ["netmeeting", "audio", "voip", "rtp", "encryption", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoAudio", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoAudio")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoAudio", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables unencrypted RTP VoIP; all voice should route through encrypted platforms (Teams, Cisco).",
            },
            new TweakDef
            {
                Id = "netmeet-disable-video",
                Label = "NetMeeting Policy: Disable NetMeeting Video Conferencing",
                Category = "Communication",
                Description =
                    "Disables the video conferencing feature of NetMeeting. NetMeeting video streams are unencrypted H.263-over-RTP. Video content captured without encryption on a corporate LAN is a significant information disclosure risk.",
                Tags = ["netmeeting", "video", "webcam", "h263", "encryption", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoVideo", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoVideo")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoVideo", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables unencrypted H.263/RTP video conferencing on corporate networks.",
            },
            new TweakDef
            {
                Id = "netmeet-disable-incoming-calls",
                Label = "NetMeeting Policy: Block Incoming NetMeeting Calls",
                Category = "Communication",
                Description =
                    "Prevents the workstation from accepting incoming NetMeeting calls. Even on systems where NetMeeting is not actively used, the service may be listening on ports 1503/1720 if not explicitly blocked. This policy prevents spontaneous incoming session establishment.",
                Tags = ["netmeeting", "incoming", "block", "ports", "h323", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysKey],
                ApplyOps = [RegOp.SetDword(SysKey, "NoIncomingCalls", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "NoIncomingCalls")],
                DetectOps = [RegOp.CheckDword(SysKey, "NoIncomingCalls", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents unsolicited inbound NetMeeting sessions on ports 1503/1720.",
            },
        ];

    }

    // ── TeamsAdvanced ──
    private static class _TeamsAdvanced
    {
        private const string Policy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftTeams";
        private const string PolicyUsers = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "teams-disable-meeting-recording",
                Label = "Disable Teams Meeting Recording",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["teams", "meeting", "recording", "privacy", "policy"],
                Description =
                    "Prevents users from recording Microsoft Teams meetings via GPO policy. "
                    + "Recordings will be disabled for all meetings, including calls and channel meetings.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowMeetingRecording", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowMeetingRecording")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowMeetingRecording", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-anonymous-join",
                Label = "Disable Anonymous Meeting Join",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["teams", "meeting", "anonymous", "security", "policy"],
                Description =
                    "Prevents anonymous (unauthenticated) users from joining Teams meetings. "
                    + "All participants must sign in with an authenticated account.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowAnonymousUsersToJoinMeeting", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowAnonymousUsersToJoinMeeting")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowAnonymousUsersToJoinMeeting", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-anon-start-meeting",
                Label = "Prevent Anonymous Users from Starting Meetings",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["teams", "meeting", "anonymous", "security", "policy"],
                Description =
                    "Prevents anonymous (unauthenticated) users from starting or initiating "
                    + "Teams meetings without an authenticated organizer present.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowAnonymousUsersToStartMeeting", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowAnonymousUsersToStartMeeting")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowAnonymousUsersToStartMeeting", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-giphy",
                Label = "Disable Giphy in Teams Chat",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["teams", "chat", "giphy", "content", "policy"],
                Description =
                    "Disables the Giphy animated GIF integration in Teams chat via policy. "
                    + "Reduces bandwidth usage and enforces professional communication standards.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowGiphy", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowGiphy")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowGiphy", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-stickers",
                Label = "Disable Stickers in Teams",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["teams", "chat", "stickers", "content", "policy"],
                Description =
                    "Disables the sticker tab and sticker sharing in Teams chat via policy. "
                    + "Enforces professional communication in business environments.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowStickers", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowStickers")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowStickers", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-memes",
                Label = "Disable Meme Images in Teams",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["teams", "chat", "memes", "content", "policy"],
                Description =
                    "Disables the meme/image captioning tab in Teams chat via policy. "
                    + "Prevents custom meme creation and sharing in workplace communications.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowMemes", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowMemes")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowMemes", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-discover-private-channels",
                Label = "Hide Private Channels from Search",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["teams", "channels", "privacy", "security", "policy"],
                Description =
                    "Prevents users from discovering private Team channels via the Teams "
                    + "search interface. Only explicit channel members can see private channels.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowDiscoverPrivateChannels", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowDiscoverPrivateChannels")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowDiscoverPrivateChannels", 0)],
            },
            new TweakDef
            {
                Id = "teams-disable-org-wide-team-creation",
                Label = "Restrict Org-Wide Team Creation",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["teams", "team", "creation", "governance", "policy"],
                Description =
                    "Restricts creation of org-wide Teams (visible to all users in the tenant). "
                    + "Only Global Administrators can create org-wide teams.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowOrgWideTeamCreation", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowOrgWideTeamCreation")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowOrgWideTeamCreation", 0)],
            },
            new TweakDef
            {
                Id = "teams-set-giphy-rating-strict",
                Label = "Set Teams Giphy Content to Strict (G-Rated)",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["teams", "giphy", "content", "safe", "policy"],
                Description =
                    "Sets the Giphy content rating to Strict (G-rated only) in Teams chat. "
                    + "Value 1 = Strict. When Giphy is enabled, only family-friendly GIFs are shown.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowGiphyRating", 1)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowGiphyRating")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowGiphyRating", 1)],
            },
            new TweakDef
            {
                Id = "teams-disable-private-calling",
                Label = "Disable Teams Private Calling",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["teams", "calling", "private", "policy"],
                Description =
                    "Disables peer-to-peer private calling in Microsoft Teams via policy. "
                    + "Users can still participate in Team or channel calls, but cannot initiate "
                    + "direct 1:1 PSTN or VoIP calls.",
                ApplyOps = [RegOp.SetDword(Policy, "AllowPrivateCalling", 0)],
                RemoveOps = [RegOp.DeleteValue(Policy, "AllowPrivateCalling")],
                DetectOps = [RegOp.CheckDword(Policy, "AllowPrivateCalling", 0)],
            },
        ];

    }

    // ── TeamsCallingPolicy ──
    private static class _TeamsCallingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftTeams";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "tmscall-enable-voicemail-routing",
                Label = "Teams Calling: Enable Voicemail Routing for Missed Calls",
                Category = "Communication",
                Description = "Sets AllowVoicemail=1 in MicrosoftTeams policy. Enables voicemail as a call routing target when a call is unanswered. " +
                    "Without voicemail routing, unanswered calls drop silently. This is required for Teams Phone users to have a compliant missed-call record and supports call centre audit trails.",
                Tags = ["teams", "calling", "voicemail", "routing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables voicemail for Teams users; unanswered calls are redirected to cloud voicemail instead of dropping.",
                ApplyOps = [RegOp.SetDword(Key, "AllowVoicemail", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowVoicemail")],
                DetectOps = [RegOp.CheckDword(Key, "AllowVoicemail", 1)],
            },
            new TweakDef
            {
                Id = "tmscall-enable-shared-line-delegation",
                Label = "Teams Calling: Enable Boss-Delegate Shared Line Appearance",
                Category = "Communication",
                Description = "Sets AllowDelegation=1 in MicrosoftTeams policy. Enables shared line appearance (SLA) so a delegate (admin assistant) can answer calls on behalf of an executive. " +
                    "Without this, only direct Teams-to-Teams calls are supported and PSTN delegation is not available to non-admin accounts.",
                Tags = ["teams", "calling", "delegation", "sla", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables shared line appearance delegation for Teams Phone; executive-admin call handling workflows.",
                ApplyOps = [RegOp.SetDword(Key, "AllowDelegation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDelegation")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDelegation", 1)],
            },
            new TweakDef
            {
                Id = "tmscall-enable-call-park-feature",
                Label = "Teams Calling: Enable Call Park and Retrieve",
                Category = "Communication",
                Description = "Sets AllowCallPark=1 in MicrosoftTeams policy. Enables the call park feature so users can place an active call on hold and retrieve it from any Teams endpoint. " +
                    "Widely used in healthcare and hospitality environments where calls must be handed off between staff without transferring or dropping.",
                Tags = ["teams", "calling", "call-park", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables call park/retrieve; supports multi-device call handoff scenarios in enterprise environments.",
                ApplyOps = [RegOp.SetDword(Key, "AllowCallPark", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCallPark")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCallPark", 1)],
            },
            new TweakDef
            {
                Id = "tmscall-disable-external-call-forwarding",
                Label = "Teams Calling: Block Call Forwarding to External Numbers",
                Category = "Communication",
                Description = "Sets AllowCallForwardingToExternalNumbers=0 in MicrosoftTeams policy. Prevents users from forwarding Teams calls to external PSTN numbers. " +
                    "This reduces the risk of toll fraud and data exfiltration through forwarded calls, which is a common security concern in regulated financial and legal organisations.",
                Tags = ["teams", "calling", "forwarding", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks PSTN call forwarding; prevents toll-fraud and out-of-band data leakage via forwarded calls.",
                ApplyOps = [RegOp.SetDword(Key, "AllowCallForwardingToExternalNumbers", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCallForwardingToExternalNumbers")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCallForwardingToExternalNumbers", 0)],
            },
            new TweakDef
            {
                Id = "tmscall-enable-busy-on-busy",
                Label = "Teams Calling: Enable Busy-on-Busy for Active Calls",
                Category = "Communication",
                Description = "Sets BusyOnBusyEnabled=1 in MicrosoftTeams policy. When a user is already in a Teams call, additional incoming PSTN calls will hear a busy signal instead of ringing through or diverting to voicemail. " +
                    "This gives callers a clear signal and prevents voicemail from filling up during back-to-back meetings.",
                Tags = ["teams", "calling", "busy", "pstn", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Callers hear busy signal when user is already on a call; reduces voicemail clutter.",
                ApplyOps = [RegOp.SetDword(Key, "BusyOnBusyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BusyOnBusyEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "BusyOnBusyEnabled", 1)],
            },
            new TweakDef
            {
                Id = "tmscall-disable-simultaneous-ring-external",
                Label = "Teams Calling: Block Simultaneous Ring to External Numbers",
                Category = "Communication",
                Description = "Sets AllowSimultaneousRingToExternalNumbers=0 in MicrosoftTeams policy. Prevents Teams calls from simultaneously ringing external PSTN phone numbers. " +
                    "Similar to blocking external forwarding, this eliminates a toll-fraud vector and prevents users from bypassing corporate monitoring by routing calls to personal mobiles.",
                Tags = ["teams", "calling", "simultaneous-ring", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks simultaneous ring on external numbers; reduces toll fraud risk and enforces call recording compliance.",
                ApplyOps = [RegOp.SetDword(Key, "AllowSimultaneousRingToExternalNumbers", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSimultaneousRingToExternalNumbers")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSimultaneousRingToExternalNumbers", 0)],
            },
            new TweakDef
            {
                Id = "tmscall-enable-call-transcription",
                Label = "Teams Calling: Enable Automatic Call Transcription",
                Category = "Communication",
                Description = "Sets AllowTranscriptionForCalling=1 in MicrosoftTeams policy. Enables automatic real-time transcription for Teams PSTN and VoIP calls. " +
                    "Transcripts are stored in Teams call history and can be reviewed for accessibility, compliance, and knowledge capture without requiring a call recorder.",
                Tags = ["teams", "calling", "transcription", "accessibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables real-time call transcription; supports accessibility, compliance review, and action-item capture.",
                ApplyOps = [RegOp.SetDword(Key, "AllowTranscriptionForCalling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTranscriptionForCalling")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTranscriptionForCalling", 1)],
            },
            new TweakDef
            {
                Id = "tmscall-disable-external-call-transfer",
                Label = "Teams Calling: Block Blind Transfer to External PSTN Numbers",
                Category = "Communication",
                Description = "Sets AllowTransferToExternalNumbers=0 in MicrosoftTeams policy. Prevents users from blind-transferring active Teams calls to external PSTN telephone numbers. " +
                    "Complements the external forwarding block. Call transfers bypass recording infrastructure, making this a key control for MiFID II and financial sector compliance.",
                Tags = ["teams", "calling", "transfer", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks PSTN blind transfers; enforces call recording compliance under MiFID II and similar regulations.",
                ApplyOps = [RegOp.SetDword(Key, "AllowTransferToExternalNumbers", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTransferToExternalNumbers")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTransferToExternalNumbers", 0)],
            },
            new TweakDef
            {
                Id = "tmscall-enable-caller-id-override-policy",
                Label = "Teams Calling: Enable Caller ID Override Policy",
                Category = "Communication",
                Description = "Sets CallerIdPolicyEnabled=1 in MicrosoftTeams policy. Allows IT to override the outbound caller ID for Teams PSTN calls. " +
                    "This is needed when multiple departments share a single external number and calls should display a generic department DID rather than the individual user's direct number. Also required to block presentation of personal mobile numbers to external parties.",
                Tags = ["teams", "calling", "caller-id", "pstn", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables IT control over outbound caller ID; privacy protection and department-level DID management.",
                ApplyOps = [RegOp.SetDword(Key, "CallerIdPolicyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "CallerIdPolicyEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "CallerIdPolicyEnabled", 1)],
            },
            new TweakDef
            {
                Id = "tmscall-enable-music-on-hold",
                Label = "Teams Calling: Enable Music on Hold for PSTN Calls",
                Category = "Communication",
                Description = "Sets AllowMusicOnHold=1 in MicrosoftTeams policy. Plays hold music to external PSTN callers when a Teams user places them on hold. " +
                    "Without this, external callers hear silence during hold, leading to call abandonment. Music on hold is a standard business telephony expectation for enterprise PSTN deployments.",
                Tags = ["teams", "calling", "hold", "pstn", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Plays hold music to external callers on hold; reduces call abandonment; improves perceived responsiveness.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMusicOnHold", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMusicOnHold")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMusicOnHold", 1)],
            },
        ];

    }

    // ── TeamsMeetingAudioPolicy ──
    private static class _TeamsMeetingAudioPolicy
    {
        private const string TeamsOfficeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Teams";

        private const string TeamsPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tmsaud-disable-bypass-local-media-optimization",
                    Label = "Teams Audio: Enable Local Media Optimization (Bypass Direct Routing Media Server)",
                    Category = "Communication",
                    Description =
                        "Sets DisableLocalMediaOptimization=0 in the Teams policy key. Enables Local Media Optimization (LMO) for Teams Phone Direct Routing — when enabled, Teams routes media (audio/video) directly between the SBC (Session Border Controller) and client endpoints that are on the same network, bypassing the Teams media relay server in Microsoft Azure. This dramatically reduces latency for on-premises Teams Phone calls by keeping media traffic local instead of routing via Azure data centres thousands of miles away. LMO is the primary quality improvement for enterprises using Teams Phone with on-premises SBC infrastructure.",
                    Tags = ["teams", "local-media-optimization", "direct-routing", "sbc", "latency"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Local Media Optimization enabled for Teams Direct Routing. Requires SBC configuration to support LMO (SBC vendor firmware update and topology configuration). Reduces media latency for on-premises SBC-based PSTN calls. Requires proper NAT/firewall configuration — the SBC must be reachable directly from the Teams client network on media ports.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableLocalMediaOptimization", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableLocalMediaOptimization")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableLocalMediaOptimization", 0)],
                },
                new TweakDef
                {
                    Id = "tmsaud-require-e2e-media-encryption",
                    Label = "Teams Audio: Require End-to-End Media Encryption for All Teams Calls",
                    Category = "Communication",
                    Description =
                        "Sets RequireE2EEncryption=1 in the Teams policy key. Requires end-to-end encrypted (E2EE) audio and video for all Teams one-on-one calls. Standard Teams calls use SRTP encryption in transit (client-to-Microsoft-server), but with E2EE the encryption is applied client-to-client and the Teams server cannot decrypt the media streams. E2EE prevents a man-in-the-middle attack at the Microsoft server layer from intercepting meeting audio. E2EE calls do not support recording, transcription, PSTN access, or conference room devices — it is designed specifically for sensitive bilateral conversations.",
                    Tags = ["teams", "e2e-encryption", "srtp", "media-encryption", "end-to-end"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "E2EE required for Teams calls. E2EE calls cannot use: cloud recording, live transcription, PSTN PSTN transfer, breakout rooms, or conference room (Teams Rooms) devices. Participants on older Teams clients that do not support E2EE will receive a notification that they cannot join. Only use for designated high-sensitivity teams — not as a universal policy.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "RequireE2EEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "RequireE2EEncryption")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "RequireE2EEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "tmsaud-enable-noise-suppression",
                    Label = "Teams Audio: Enable Deep Learning-Based Noise Suppression (AI audio)",
                    Category = "Communication",
                    Description =
                        "Sets DisableNoiseSuppression=0 in the Teams policy key. Enables the Teams AI-powered noise suppression feature that uses a deep neural network (DNN) to filter background sounds during calls and meetings. The DNN model identifies non-speech audio patterns (keyboard typing, HVAC noise, office background chatter, train/plane noise) and removes them in real time before transmitting audio to other participants. Noise suppression significantly improves call quality in open-plan offices, home environments, and noisy locations.",
                    Tags = ["teams", "noise-suppression", "ai-audio", "dnn", "call-quality"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Teams AI noise suppression enabled. Uses CPU cycles for real-time neural network inference (3–8% CPU on modern hardware). On low-end hardware (2-core i5, <8 GB RAM), noise suppression may cause audio processing delays. Teams can be configured per-user to use lighter suppression intensity if CPU impact is a concern.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableNoiseSuppression", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableNoiseSuppression")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableNoiseSuppression", 0)],
                },
                new TweakDef
                {
                    Id = "tmsaud-enable-high-fidelity-audio",
                    Label = "Teams Audio: Enable High Fidelity Music Mode (48 kHz Stereo Audio Codec)",
                    Category = "Communication",
                    Description =
                        "Sets DisableHighFidelityAudio=0 in the Teams policy key. Enables High Fidelity Audio mode — Teams uses a 48 kHz stereo audio codec (OPUS stereo at ~128 kbps) instead of the default 16 kHz mono speech codec. High Fidelity Audio is critical for Teams meetings that include music playback (instrument demos, music education, virtual concerts, media production reviews) — standard speech-optimised codecs process frequencies up to 8 kHz, which makes music sound muffled. High Fidelity mode passes the full audible range to remote participants.",
                    Tags = ["teams", "high-fidelity-audio", "music-mode", "opus", "48khz"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "48 kHz stereo codec enabled. Uses ~128 kbps per participant audio bandwidth (compared to ~20 kbps in standard mode). Noise suppression and echo cancellation are disabled in High Fidelity mode — not intended for regular voice meetings. This mode is only useful for music education, media review, or broadcast-style meetings.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableHighFidelityAudio", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableHighFidelityAudio")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableHighFidelityAudio", 0)],
                },
                new TweakDef
                {
                    Id = "tmsaud-restrict-meeting-recording-auto-retention",
                    Label = "Teams Audio: Restrict Automatic Meeting Recording Retention to 60 Days",
                    Category = "Communication",
                    Description =
                        "Sets MeetingRecordingExpirationDays=60 in the Teams policy key. Sets the automatic expiration period for Teams meeting recordings to 60 days. Without an expiration policy, meeting recordings are retained indefinitely in OneDrive/SharePoint — accumulating storage at 300–500 MB per hour of recording. Recordings of regular team meetings rarely need retention beyond 60 days. Sensitive recordings, compliance recordings, and training recordings can be manually marked to retain beyond the default period. 60 days balances storage cost against ad-hoc lookback access requirements.",
                    Tags = ["teams", "meeting-recording", "retention", "expiration", "storage"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Meeting recordings auto-expire after 60 days. Users receive a notification 14 days before expiry to save recordings they want to keep permanently. Recordings used for compliance, eDiscovery, or HR purposes should be manually tagged for longer retention or moved to an archived location before the 60-day limit.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "MeetingRecordingExpirationDays", 60)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "MeetingRecordingExpirationDays")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "MeetingRecordingExpirationDays", 60)],
                },
                new TweakDef
                {
                    Id = "tmsaud-disable-third-party-audio-device-telemetry",
                    Label = "Teams Audio: Disable Third-Party Audio Device Telemetry in Teams",
                    Category = "Communication",
                    Description =
                        "Sets DisableDeviceTelemetry=1 in the Teams policy key. Prevents Teams from transmitting audio device quality telemetry (audio hardware model, driver version, audio quality metrics, audio device firmware) to Microsoft's Teams Quality Analytics platform. While device telemetry is used by Microsoft to improve Teams audio quality diagnostics, in privacy-sensitive environments the audio hardware inventory data may be subject to data governance controls. Disabling telemetry prevents the Teams client from transmitting hardware details to third-party analytics services.",
                    Tags = ["teams", "telemetry", "audio-device", "privacy", "analytics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Audio device telemetry not transmitted to Microsoft Teams analytics. Microsoft admin centre audio quality diagnostics will have limited data for this organisation. Required Teams call quality logs (per-call QoS data) are separate from optional telemetry and are not affected.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableDeviceTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableDeviceTelemetry")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableDeviceTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "tmsaud-enable-call-quality-reporting",
                    Label = "Teams Audio: Enable per-Call Quality Diagnostics Reporting to Teams Admin Centre",
                    Category = "Communication",
                    Description =
                        "Sets DisableCallQualityReporting=0 in the Teams policy key. Enables per-call quality diagnostics reporting to the Teams admin centre (Call Quality Dashboard — CQD). CQD receives per-call statistics including audio quality metrics (jitter, packet loss, round-trip time), stream quality scores, network path information, and device performance data. The CQD dashboard allows IT to identify poor call quality by building type, user group, network segment, or device model — essential for diagnosing systematic Teams audio quality problems in the enterprise network.",
                    Tags = ["teams", "call-quality", "cqd", "jitter", "packet-loss"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Per-call quality metrics reported to Teams Admin Centre CQD. Call quality data includes network path, quality scores, and device identifiers — no meeting content. IT admins with Teams admin centre access can view CQD reports. CQD data is retained for 28 days by default in Microsoft 365.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableCallQualityReporting", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableCallQualityReporting")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableCallQualityReporting", 0)],
                },
                new TweakDef
                {
                    Id = "tmsaud-block-third-party-meeting-audio-apps",
                    Label = "Teams Audio: Block Third-Party Audio App Integration in Teams Meetings",
                    Category = "Communication",
                    Description =
                        "Sets BlockThirdPartyAudioApps=1 in the Teams policy key. Prevents third-party audio applications (Krisp, RTX Voice, NVIDIA RTX Voice, Dolby Voice) from being registered or used as audio processing filters within Teams meetings. Third-party audio apps integrate with Teams via the Windows AudioGraph API or virtual audio device drivers to process mic/speaker audio. While often beneficial for noise suppression, these apps have system-level access to all audio data — in high-security environments, audio filtering apps from third-party vendors may not meet data handling requirements.",
                    Tags = ["teams", "third-party-audio", "audio-filter", "krisp", "audio-app"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Third-party audio filter apps blocked in Teams. Users who rely on Krisp or NVIDIA RTX Voice for noise suppression must use Teams built-in noise suppression instead. Some users with hearing accessibility needs who use audio enhancement apps may be impacted — evaluate accessibility implications before deploying.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "BlockThirdPartyAudioApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "BlockThirdPartyAudioApps")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "BlockThirdPartyAudioApps", 1)],
                },
                new TweakDef
                {
                    Id = "tmsaud-set-dynamic-emergency-calling",
                    Label = "Teams Audio: Enable Dynamic Emergency Calling (E911 Location Services)",
                    Category = "Communication",
                    Description =
                        "Sets EnableDynamicEmergencyCalling=1 in the Teams policy key. Enables dynamic emergency calling for Teams Phone — when a user dials 911 (or equivalent country emergency service number), Teams automatically determines the user's physical location based on network topology (IP subnet, wireless BSSID, chassis ID from LLDP) and sends it to the emergency service. Without dynamic emergency calling, 911 callers' locations may be registered to the main corporate headquarters address regardless of which office they are in, causing emergency responders to be dispatched to the wrong location.",
                    Tags = ["teams", "e911", "emergency-calling", "location", "dynamic-e911"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Dynamic E911 location enabled for Teams Phone. Requires Teams Phone network configuration in the Teams admin centre (Location Information Service — LIS) with subnet, wireless AP, and switch port mappings to physical addresses. Incomplete LIS configuration results in fallback to organisation address. This is a legal requirement in many jurisdictions for multi-site organisations using Teams Phone.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "EnableDynamicEmergencyCalling", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "EnableDynamicEmergencyCalling")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "EnableDynamicEmergencyCalling", 1)],
                },
                new TweakDef
                {
                    Id = "tmsaud-enable-compliance-recording",
                    Label = "Teams Audio: Enable Compliance Recording Policy Flag (Regulatory Recording Prerequisite)",
                    Category = "Communication",
                    Description =
                        "Sets EnableComplianceRecording=1 in the Teams policy key. Sets the policy flag marking that this organisation uses Microsoft Teams Compliance Recording (a Teams certified compliance recording solution). Compliance Recording differs from regular meeting recording — it captures all calls and meetings automatically, is tamper-proof, and is retained according to the compliance policy rather than user action. In regulated industries (financial services, healthcare, legal), all communications must be recorded for regulatory compliance. Setting this flag enables the compliance recording infrastructure in the Teams client.",
                    Tags = ["teams", "compliance-recording", "regulatory", "financial-services", "tamper-proof"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Compliance recording enabled. Requires a Teams certified compliance recording solution (e.g., Verint, NICE, Nuance, Microsoft Purview Communication Compliance). Users are notified that calls are recorded. Setting this flag alone does not start recording — a compliance recording bot must be configured in Teams Calling Policy in the admin centre.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "EnableComplianceRecording", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "EnableComplianceRecording")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "EnableComplianceRecording", 1)],
                },
            ];

    }

    // ── TeamsMessagingPolicy ──
    private static class _TeamsMessagingPolicy
    {
        private const string TeamsPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tmsmsg-disable-external-chat",
                    Label = "Teams Messaging: Block Chat Messages with External (Federated) Teams Users",
                    Category = "Communication",
                    Description =
                        "Sets AllowExternalChat=0 in the Teams policy key. Prevents Teams users from initiating or receiving chat messages with external Teams users from other organisations (federation). Teams federation allows users in different Microsoft 365 tenants to message each other directly — this capability creates a potential data exfiltration channel where sensitive information can be transmitted to non-corporate Teams users via chat. In high-security environments, all external collaboration should go through approved collaboration channels with proper DLP controls rather than open federation.",
                    Tags = ["teams", "external-chat", "federation", "dlp", "external-messaging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "External (federated) Teams messaging blocked. Users cannot send or receive direct chat messages from external Teams users. Existing external chat conversations are still visible in history but new messages are blocked. Business processes that rely on Teams federation with partners, contractors, or suppliers should be migrated to authorised Guest Access (in-tenant channel) before deploying.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowExternalChat", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowExternalChat")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowExternalChat", 0)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-enable-message-immutability",
                    Label = "Teams Messaging: Enable Message Immutability (Prevent User Delete/Edit of Sent Messages)",
                    Category = "Communication",
                    Description =
                        "Sets AllowUserDeleteChat=0 and AllowUserEditMessage=0 in the Teams policy key. Prevents users from deleting or editing messages after they have been sent in Teams chat and channels. Message immutability is required in regulated industries — in financial services, legal, and healthcare, chat communications must be retained unaltered as a complete record. Allowing message deletion or editing enables users to delete incriminating or non-compliant messages after the fact. This policy ensures that the full chat history is preserved for eDiscovery and compliance review.",
                    Tags = ["teams", "message-immutability", "delete-message", "compliance", "ediscovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users cannot delete or edit sent messages. Messages containing errors or sensitive information cannot be corrected or withdrawn. IT/compliance officers can use Teams content moderation tools to remove flagged content via admin centre. Inform users of this change before deploying — it has a significant impact on messaging behaviour.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowUserDeleteChat", 0), RegOp.SetDword(TeamsPolicyKey, "AllowUserEditMessage", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowUserDeleteChat"), RegOp.DeleteValue(TeamsPolicyKey, "AllowUserEditMessage")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowUserDeleteChat", 0), RegOp.CheckDword(TeamsPolicyKey, "AllowUserEditMessage", 0)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-disable-giphy-in-chat",
                    Label = "Teams Messaging: Disable Giphy GIF Integration in Teams Chat",
                    Category = "Communication",
                    Description =
                        "Sets AllowGiphy=0 in the Teams policy key. Disables the Giphy GIF search and insertion feature in Teams chat. The Giphy integration sends search queries to the Giphy CDN (external service) to retrieve GIF content. This creates an implicit data disclosure: search terms typed in the Teams chat GIF search box are transmitted to Giphy's servers. Additionally, GIF content retrieved from Giphy is subject to Giphy's content policies — in professional environments, inappropriate GIFs inserted in public channels can create a hostile work environment compliance risk.",
                    Tags = ["teams", "giphy", "gif", "external-service", "content-moderation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Giphy GIF integration removed from the Teams chat toolbar. Users cannot search for or insert Giphy GIFs. Standard emoji and built-in Teams stickers are not affected. Custom sticker packs managed via Teams admin centre can still be enabled.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowGiphy", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowGiphy")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowGiphy", 0)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-disable-memes-in-chat",
                    Label = "Teams Messaging: Disable Meme/Praise Card Creation in Teams Chat",
                    Category = "Communication",
                    Description =
                        "Sets AllowMemes=0 combined with AllowPraise=0 in the Teams policy key. Disables the built-in meme editor (Meme Generator) and Praise badge cards in Teams chat. The meme generator allows users to create and send image-overlaid text memes in chat — content that may range from benign to potentially offensive or harassment-enabling. Praise cards are appreciation cards with badge icons. In risk-averse enterprise environments where all chat content is subject to legal hold and compliance review, meme content in corporate chat creates legal and policy exposure.",
                    Tags = ["teams", "memes", "praise", "content-moderation", "enterprise-policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "Meme editor and Praise cards disabled in Teams. No functional impact on work communications. If organisations have a culture initiative using Praise cards, this disables the feature. Consider the cultural impact before deploying in human-focused organisations that actively use Praise for employee recognition.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowMemes", 0), RegOp.SetDword(TeamsPolicyKey, "AllowPraise", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowMemes"), RegOp.DeleteValue(TeamsPolicyKey, "AllowPraise")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowMemes", 0), RegOp.CheckDword(TeamsPolicyKey, "AllowPraise", 0)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-enable-priority-notifications",
                    Label = "Teams Messaging: Enable Priority (Urgent) Notifications with Repeated Alerts",
                    Category = "Communication",
                    Description =
                        "Sets AllowPriorityMessages=1 in the Teams policy key. Enables the Priority Notifications feature in Teams — senders can mark a message as 'Urgent' which causes the notification to repeat every 2 minutes for 20 minutes until the recipient opens the message (or the notification expires). Priority notifications provide a mechanism for genuinely time-critical communications (on-call incidents, security alerts, physical emergency notifications) that need guaranteed attention within minutes. Without priority notifications, all messages are treated equally regardless of urgency.",
                    Tags = ["teams", "priority-notifications", "urgent", "on-call", "incident-response"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Priority (Urgent) messaging enabled. Urgent messages create repeated notifications every 2 minutes for up to 20 minutes. Misuse (sending non-urgent messages as Urgent) can be disruptive. Consider applying a Teams messaging policy in the admin centre that limits who can send Urgent messages (e.g., only on-call and security teams).",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowPriorityMessages", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowPriorityMessages")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowPriorityMessages", 1)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-enable-read-receipts",
                    Label = "Teams Messaging: Enable Read Receipts in Teams Chat (Sent/Read Indicators)",
                    Category = "Communication",
                    Description =
                        "Sets AllowReadReceipts=1 in the Teams policy key. Enables message read receipts in Teams one-on-one chat and small group chats — senders can see which recipients have read their messages (tick mark under message). Read receipts improve communication efficiency by allowing senders to determine whether a recipient has seen a message without needing to ask 'did you see my message?'. This is particularly valuable for hybrid teams where asynchronous communication is common and message delivery confirmation improves workflow coordination.",
                    Tags = ["teams", "read-receipts", "message-delivery", "async-communication", "confirmation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Read receipts enabled for Teams chat. Recipients who read a message will have their read status visible to the sender. In some work cultures, read receipts create pressure to respond immediately. Teams policy can be set to allow users to control whether they show read receipts individually via their Teams settings.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowReadReceipts", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowReadReceipts")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowReadReceipts", 1)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-disable-meeting-chat-during-meeting",
                    Label = "Teams Messaging: Allow Meeting Chat Only During Meeting (Disable Chat After)",
                    Category = "Communication",
                    Description =
                        "Sets AllowMeetingChat=1 (1 = enabled during meeting only) in the Teams policy key. Configures meeting chat to be available only during the meeting session. After the meeting ends, the chat thread closes and becomes read-only. Meeting chat threads that remain open post-meeting become informal communication channels where sensitive discussions from the meeting continue outside the meeting context, potentially without proper retention policies. Closing chat after the meeting ensures the meeting context is preserved in the recording/transcript rather than scattered across a chat thread.",
                    Tags = ["teams", "meeting-chat", "post-meeting", "retention", "governance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Meeting chat available only during active meeting. After the meeting ends, the chat becomes read-only. Meeting participants can still access the full chat history. Post-meeting follow-up conversations should be directed to the team channel or a new chat thread.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowMeetingChat", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowMeetingChat")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowMeetingChat", 1)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-disable-teams-anonymous-join",
                    Label = "Teams Messaging: Disable Anonymous User Join to Teams Meetings",
                    Category = "Communication",
                    Description =
                        "Sets AllowAnonymousUsersToJoinMeeting=0 in the Teams policy key. Prevents anonymous (unauthenticated) users from joining Teams meetings hosted by this organisation. By default, Teams allows anyone with a meeting link to join without signing in — appearing as 'Joseph (Guest)' or similar. Anonymous join poses security risks: meeting links can be forwarded, posted publicly, or guessed, allowing unintended parties to eavesdrop on meetings. Requiring authentication ensures only intentionally invited users can participate.",
                    Tags = ["teams", "anonymous-join", "meeting-security", "unauthenticated", "meeting-link"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous join disabled. External participants (vendors, clients, interview candidates) must sign in with a Microsoft account or Teams account to join meetings. Consider enabling the Teams lobby for external participants so authenticated external users wait for host approval before entering. Web-based Teams join still supported with Microsoft account sign-in.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowAnonymousUsersToJoinMeeting", 0)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowAnonymousUsersToJoinMeeting")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowAnonymousUsersToJoinMeeting", 0)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-enable-supervised-chat",
                    Label = "Teams Messaging: Enable Supervised Chat (Educator/Supervisor Oversight Mode)",
                    Category = "Communication",
                    Description =
                        "Sets SupervisedChatEnabled=1 in the Teams policy key. Enables Supervised Chat mode for Teams — a Teams for Education feature that allows supervisors (teachers, managers) to monitor chat conversations between specific user groups. In supervised chat mode, restricted users (e.g., students, apprentices) can only initiate chats with supervisors — they cannot start direct chats with peers without a supervisor present. For non-education enterprises, supervised chat provides a management layer for compliance monitoring in sensitive departments (trading floors, customer service, healthcare).",
                    Tags = ["teams", "supervised-chat", "education", "monitoring", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Supervised chat mode enabled. Restricted users must initiate chats through supervisors. This significantly restricts peer-to-peer messaging for the restricted user group. Primarily designed for Teams for Education (K-12). For enterprise, consider using Microsoft Purview Communication Compliance for passive monitoring rather than active supervision as it is less disruptive.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "SupervisedChatEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "SupervisedChatEnabled")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "SupervisedChatEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "tmsmsg-disable-teams-consumer-accounts",
                    Label = "Teams Messaging: Block Teams (Free) Personal Consumer Account Chat Federation",
                    Category = "Communication",
                    Description =
                        "Sets DisableConsumerFederation=1 in the Teams policy key. Blocks Teams Work accounts from messaging Teams Personal (Teams Free/consumer) account users. Microsoft introduced consumer-to-work Teams messaging in 2022 — corporate employees can chat with personal 'Teams Free' account holders. This creates a data governance gap: regulatory and compliance controls on Teams work accounts do not extend to the consumer federation path. Blocking consumer federation ensures that all Teams communications into and out of the organisation go through the governed work account channel.",
                    Tags = ["teams", "consumer-federation", "teams-free", "data-governance", "dlp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Teams work accounts cannot message Teams Free (personal) account users. Business processes that involve communicating with consumers or partners who use Teams Free must use an alternative channel (Teams Guest Access, email, or external sharing). Consumer federation was disabled by default in enterprise tenants prior to 2023.",
                    ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableConsumerFederation", 1)],
                    RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableConsumerFederation")],
                    DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableConsumerFederation", 1)],
                },
            ];

    }

    // ── TelephonyPolicy ──
    private static class _TelephonyPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Telephony";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "telpol-disable-call-telemetry",
                Label = "Disable Phone Call Telemetry",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows phone call telemetry collects data about calling application usage, call durations, and telephony API invocations. This telemetry is transmitted to Microsoft for improving the Windows telephony stack and Teams integration. Disabling it prevents call metadata from being transmitted outside the enterprise network. Organizations with strict communication privacy requirements benefit from eliminating call-related telemetry streams. Telephony functionality including PSTN, VoIP, and integrated calling applications is unaffected by disabling telemetry. Enterprise communication analytics are better gathered through centralized UCaaS platform reporting rather than OS-level telemetry.",
                Tags = ["telephony", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneCallTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneCallTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneCallTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "telpol-block-phone-app",
                Label = "Block Phone App",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The Windows Phone app integrates Android and iPhone devices with Windows for call mirroring and cross-device notifications. Blocking the Phone app prevents the Phone Link service from linking personal mobile devices to corporate workstations. Device linking can create data exfiltration channels by mirroring corporate notifications to personal smartphones. Enterprises managing data loss prevention need to prevent uncontrolled device pairing with corporate endpoints. The Phone app is primarily a consumer convenience feature with limited enterprise value and significant security implications. Blocking it enforces a corporate data boundary between managed endpoints and employee personal devices.",
                Tags = ["telephony", "phone-app", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockPhoneApp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPhoneApp")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPhoneApp", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-voice-capture",
                Label = "Disable Voice Capture in Telephony",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Voice capture in the telephony framework allows applications to access microphone input through telephony API calls for recording or analysis. Disabling voice capture prevents telephony-framework applications from accessing recorded audio through the telephony stack. Corporate telephony calls can contain sensitive negotiations, strategic discussions, and confidential information. Preventing unauthorized voice capture reduces the risk of covert recording by applications with telephony access. Enterprise communication recording should be managed through compliant UCaaS platforms with proper consent mechanisms. Disabling this capability ensures only explicitly authorized recording tools can access microphone audio.",
                Tags = ["telephony", "microphone", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVoiceCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVoiceCapture", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-phone-integration",
                Label = "Disable Phone Integration",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Phone integration enables the Windows telephony subsystem to bridge desktop applications with mobile phone calling capabilities through the Phone Link service. Disabling phone integration removes the bridge between desktop workflows and mobile telephony. Enterprise environments with managed VoIP solutions covering desktop calling needs have no requirement for mobile device telephony integration. The integration creates implicit dependencies on personal mobile devices that are outside enterprise IT management scope. Disabling phone integration enforces the separation between personal and corporate communication channels. Standard VOIP and SIP-based enterprise telephony solutions remain fully functional when phone integration is disabled.",
                Tags = ["telephony", "integration", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneIntegration", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-callerid-uploads",
                Label = "Disable Caller ID Upload",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Caller ID upload functionality transmits phone number identifiers from incoming calls to Microsoft or third-party caller identification services. This transmission allows the service to return caller name information but exposes phone numbers from corporate calls to external services. Disabling caller ID uploads prevents corporate contact phone numbers from being disclosed to external lookup services. Financial institutions, law firms, and healthcare organizations have strict obligations to prevent disclosure of client contact information. The caller identification feature provides convenience but creates compliance risks when corporate contacts' numbers are transmitted externally. Disabling uploads eliminates this data exfiltration risk while preserving all local telephony functionality.",
                Tags = ["telephony", "caller-id", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCallerIdUploads", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCallerIdUploads")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCallerIdUploads", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-phone-number",
                Label = "Disable Phone Number Access",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows phone number access allows applications to read the device's cellular or VoIP telephone numbers through the telephony API. Disabling phone number access prevents applications from reading the device's associated telephone numbers without explicit user consent. Phone numbers are unique identifiers that can be used for device fingerprinting and identity correlation across services. Enterprise devices with assigned phone numbers should not expose these identifiers to arbitrary applications through the telephony API. Only explicitly authorized telephony applications with business justification should access device phone number information. Disabling this access aligns with privacy-by-default principles for enterprise endpoint configuration.",
                Tags = ["telephony", "phone-number", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneNumber", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneNumber")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneNumber", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-dialer-app",
                Label = "Disable Dialer Application",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Windows phone dialer application provides an interface for making calls through connected mobile devices or VoIP applications from the desktop. Disabling the dialer app removes this consumer calling experience from corporate workstations. Enterprise users making calls rely on unified communications platforms such as Microsoft Teams, Cisco Webex, or Zoom which provide superior enterprise calling features. The dialer app has limited utility in enterprise environments with managed UCaaS solutions and can create confusion about which calling application to use. Disabling it streamlines the calling experience by directing all calls through the approved enterprise communication platform. The dialer app removal does not affect any enterprise VoIP or PSTN gateway functionality.",
                Tags = ["telephony", "dialer", "applications", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDialerApp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDialerApp")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDialerApp", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-phone-sync-history",
                Label = "Disable Phone Sync History",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Phone sync history stores records of calls, messages, and notifications synced between mobile devices and Windows through Phone Link. This history creates a persistent log of mobile communications on the corporate workstation that can be accessed by the operating system. Disabling phone sync history prevents call and message records from a personal mobile device from being stored on the corporate endpoint. Cross-device synchronization logs should not be created on corporate endpoints as they represent personal data outside enterprise management scope. HR and legal considerations around employee privacy require that personal communication history not be stored on corporate systems. Disabling this feature maintains a clear boundary between personal mobile data and corporate endpoint storage.",
                Tags = ["telephony", "sync", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneSyncHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneSyncHistory")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneSyncHistory", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-phone-book-access",
                Label = "Disable Phone Book Access",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Phone book access through the telephony framework allows applications to read contact lists from paired mobile devices through the Phone Link connection. Disabling phone book access prevents applications from reading personal contact information stored on mobile devices through the desktop telephony bridge. Contact lists can contain sensitive personal and professional relationship information that should not be accessible through untrusted applications. Enterprise DLP policies should prevent personal device contact lists from being read by any application on the corporate endpoint. Personal contacts on mobile devices fall outside corporate data governance and must not be merged with or accessible from corporate systems. Disabling phone book access enforces the boundary between personal mobile data and corporate endpoint application access.",
                Tags = ["telephony", "contacts", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneBookAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneBookAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneBookAccess", 1)],
            },
            new TweakDef
            {
                Id = "telpol-disable-incoming-call-notif",
                Label = "Disable Incoming Call Notification",
                Category = "Communication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Incoming call notifications from paired mobile devices display caller information on the corporate desktop through the Phone Link notification system. Disabling incoming call notifications prevents personal mobile phone calls from being displayed on the corporate workstation screen. Personal call notifications can reveal personal contact names from the employee's mobile device to colleagues who can see the screen. Suppressing phone call notifications from personal devices on corporate endpoints respects employee privacy around personal communications. Enterprise unified communications notifications for business calls should be managed exclusively through the corporate UCaaS platform. Disabling mobile-sourced call notifications does not affect any corporate telephony or UCaaS notification channels.",
                Tags = ["telephony", "notifications", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIncomingCallNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIncomingCallNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIncomingCallNotification", 1)],
            },
        ];

    }

}
