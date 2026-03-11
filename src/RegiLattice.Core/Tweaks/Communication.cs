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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableGpu", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableGpu"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableGpu", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-autoupdate",
            Label = "Disable Zoom Auto-Update",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Zoom's automatic update mechanism.",
            Tags = ["zoom", "update"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Zoom\Zoom\General", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom\General"],
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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Discord", "DisableAutoStart"),
            ],
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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Discord", "disableHardwareAcceleration"),
            ],
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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Spotify", "DisableAutoStart"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Spotify", "ui.hardware_acceleration", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Spotify", "ui.hardware_acceleration"),
            ],
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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Slack", "DisableAutoStart"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Slack", "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-auto-video",
            Label = "Disable Zoom Auto-Start Video",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Zoom from automatically enabling video when joining meetings.",
            Tags = ["zoom", "video", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Zoom\Zoom\General"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableTelemetry", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "disableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-zoom-chat-notify",
            Label = "Mute Zoom Chat Notifications",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Mutes persistent chat notifications in Zoom.",
            Tags = ["zoom", "chat", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Zoom\Zoom\General"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Slack", "HardwareAccelerationEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Slack", "HardwareAccelerationEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Slack", "HardwareAccelerationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-autostart",
            Label = "Disable Teams Auto-Start (Policy)",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Teams from starting automatically at login. Reduces boot time and memory usage. Default: Auto-start. Recommended: Disabled.",
            Tags = ["communication", "teams", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "com.squirrel.Teams.Teams"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Teams", "DisableAutoStart"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-typing-insights",
            Label = "Disable Typing Insights",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables typing insights and suggestions that analyze your typing patterns. Improves privacy. Default: Enabled. Recommended: Disabled.",
            Tags = ["communication", "typing", "insights", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings", "InsightsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings", "InsightsEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\input\Settings", "InsightsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "comm-disable-online-speech",
            Label = "Disable Online Speech Recognition",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables online speech recognition that sends voice data to Microsoft for processing. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["communication", "speech", "voice", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "comm-prevent-teams-first-launch",
            Label = "Prevent Teams Auto-Start After Install",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Microsoft Teams from automatically launching after Office installation via policy. Default: Auto-launch. Recommended: Disabled.",
            Tags = ["communication", "teams", "autostart", "office", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Teams"],
        },
        new TweakDef
        {
            Id = "comm-disable-skype-telemetry-user",
            Label = "Disable Skype Telemetry (User)",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Skype desktop telemetry data collection at the user level. Reduces background data transmission. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["communication", "skype", "telemetry", "privacy", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Skype\Telemetry"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "comm-disable-teams-background",
            Label = "Disable Teams Background Activity on Login",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Microsoft Teams from running hidden in the background on login by setting OpenAsHidden to 0. Default: Enabled. Recommended: Disabled.",
            Tags = ["communication", "teams", "background", "login", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Teams"],
        },
        new TweakDef
        {
            Id = "comm-disable-skype-feedback",
            Label = "Disable Skype Feedback Survey Prompts",
            Category = "Communication",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Skype feedback survey prompts and data collection at the user level. Default: Enabled. Recommended: Disabled.",
            Tags = ["communication", "skype", "feedback", "privacy", "survey"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Skype\Feedback"],
        },
        new TweakDef
        {
            Id = "comm-block-zoom-auto-update",
            Label = "Block Zoom Auto-Update (Policy)",
            Category = "Communication",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks Zoom desktop client from automatically updating via machine-level group policy. Default: Auto-update enabled. Recommended: Disabled for managed environments.",
            Tags = ["communication", "zoom", "update", "policy", "managed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom\General"],
        },
    ];
}
