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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
            Category = "Phone Link",
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
