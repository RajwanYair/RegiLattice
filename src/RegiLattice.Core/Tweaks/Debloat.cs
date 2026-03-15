namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Debloat tweaks — removes pre-installed apps, disables built-in advertising, optional features,
/// and Windows components that are rarely needed.
/// </summary>
internal static class Debloat
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string ContentDelivery = $@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";
    private const string Policies = $@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "debloat-remove-preinstalled-apps",
            Label = "Remove All Pre-installed Store Apps",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Removes common pre-installed Store apps (Clipchamp, News, Weather, Solitaire, etc.) for all users.",
            Tags = ["debloat", "apps", "store", "bloatware"],
            SideEffects = "Apps can be reinstalled from the Microsoft Store.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$bloat = @('Clipchamp.Clipchamp','Microsoft.BingNews','Microsoft.BingWeather','Microsoft.GamingApp',"
                        + "'Microsoft.GetHelp','Microsoft.Getstarted','Microsoft.MicrosoftSolitaireCollection','Microsoft.People',"
                        + "'Microsoft.PowerAutomateDesktop','Microsoft.Todos','Microsoft.WindowsFeedbackHub','Microsoft.WindowsMaps',"
                        + "'Microsoft.ZuneMusic','Microsoft.ZuneVideo','MicrosoftTeams','Microsoft.MicrosoftOfficeHub',"
                        + "'Microsoft.549981C3F5F10','Microsoft.YourPhone','Microsoft.WindowsAlarms','Microsoft.WindowsSoundRecorder'); "
                        + "foreach ($app in $bloat) { Get-AppxPackage -AllUsers -Name $app -ErrorAction SilentlyContinue | Remove-AppxPackage -AllUsers -ErrorAction SilentlyContinue; "
                        + "Get-AppxProvisionedPackage -Online -ErrorAction SilentlyContinue | Where-Object DisplayName -eq $app | Remove-AppxProvisionedPackage -Online -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ => { }, // Cannot auto-reinstall; user must use Store
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-AppxPackage -AllUsers -Name 'Clipchamp.Clipchamp' -ErrorAction SilentlyContinue) -eq $null"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "debloat-disable-suggested-apps",
            Label = "Disable Suggested Apps in Start Menu",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from showing suggested (advertising) apps in the Start menu.",
            Tags = ["debloat", "advertising", "start-menu"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-314559Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338388Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338389Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-314559Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SystemPaneSuggestionsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "SoftLandingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-auto-app-install",
            Label = "Disable Automatic App Installation",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing promoted apps (Spotify, Disney+, etc.).",
            Tags = ["debloat", "apps", "auto-install", "advertising"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 0),
                RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "PreInstalledAppsEverEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SilentInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "ContentDeliveryAllowed"),
                RegOp.DeleteValue(ContentDelivery, "OemPreInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "PreInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "PreInstalledAppsEverEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-tips-and-tricks",
            Label = "Disable Tips, Tricks & Suggestions",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestions notifications.",
            Tags = ["debloat", "tips", "notifications", "advertising"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338393Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338393Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-353694Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-353696Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338393Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-lock-screen-ads",
            Label = "Disable Lock Screen Tips & Ads",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes fun facts and tips (ads) from the Windows lock screen.",
            Tags = ["debloat", "lock-screen", "advertising"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338387Enabled", 0),
                RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 0),
                RegOp.SetDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338387Enabled"),
                RegOp.DeleteValue(ContentDelivery, "RotatingLockScreenEnabled"),
                RegOp.DeleteValue(ContentDelivery, "RotatingLockScreenOverlayEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338387Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-consumer-features",
            Label = "Disable Consumer Experience Features",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft consumer features that install suggested apps and games.",
            Tags = ["debloat", "consumer", "apps", "advertising"],
            RegistryKeys = [$@"{Policies}\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsConsumerFeatures", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableSoftLanding", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableCloudOptimizedContent", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsConsumerFeatures"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableSoftLanding"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableCloudOptimizedContent"),
            ],
            DetectOps = [RegOp.CheckDword($@"{Policies}\CloudContent", "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-settings-ads",
            Label = "Disable Ads in Settings App",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes promotional content and suggested actions from the Windows Settings app.",
            Tags = ["debloat", "settings", "advertising"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338393Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-353698Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-353698Enabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-xbox-game-bar",
            Label = "Disable Xbox Game Bar Overlay",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Xbox Game Bar overlay (Win+G). Reduces background resource usage.",
            Tags = ["debloat", "xbox", "gaming", "overlay"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\GameBar", $@"{CuKey}\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0),
                RegOp.SetDword($@"{CuKey}\System\GameConfigStore", "GameDVR_Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\GameBar", "AutoGameModeEnabled"),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 1),
                RegOp.SetDword($@"{CuKey}\System\GameConfigStore", "GameDVR_Enabled", 1),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-remove-optional-features",
            Label = "Remove Optional Features (IE, Media Player, etc.)",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Removes rarely-used optional features: Internet Explorer, Windows Media Player, Steps Recorder, and WordPad.",
            Tags = ["debloat", "features", "optional", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$features = @('Browser.InternetExplorer~~~~0.0.11.0','Media.WindowsMediaPlayer~~~~0.0.12.0',"
                        + "'App.StepsRecorder~~~~0.0.1.0','Microsoft.Windows.WordPad~~~~0.0.1.0'); "
                        + "foreach ($f in $features) { "
                        + "Remove-WindowsCapability -Online -Name $f -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "$features = @('Browser.InternetExplorer~~~~0.0.11.0','Media.WindowsMediaPlayer~~~~0.0.12.0'); "
                        + "foreach ($f in $features) { "
                        + "Add-WindowsCapability -Online -Name $f -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-WindowsCapability -Online -Name 'Browser.InternetExplorer*' -ErrorAction SilentlyContinue).State -eq 'NotPresent'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "debloat-disable-windows-ink",
            Label = "Disable Windows Ink Workspace",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace. Frees taskbar space and background processes.",
            Tags = ["debloat", "ink", "workspace", "taskbar"],
            RegistryKeys = [$@"{Policies}\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword($@"{Policies}\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{Policies}\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword($@"{Policies}\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "debloat-unpin-all-start-tiles",
            Label = "Unpin All Start Menu Tiles (Win10)",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes all pinned tiles from the Start menu for a clean layout (Windows 10).",
            Tags = ["debloat", "start-menu", "tiles", "clean"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "(New-Object -ComObject Shell.Application).Namespace('shell:::{4234d49b-0245-4df3-b780-3893943456e1}').Items() | "
                        + "ForEach-Object { $_.Verbs() | Where-Object Name -Match 'Un.*pin' | ForEach-Object { $_.DoIt() } }"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "debloat-disable-app-suggestions",
            Label = "Disable App Suggestions (Finish Setup)",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 'Let's finish setting up your device' nag screen and app suggestions on login.",
            Tags = ["debloat", "setup", "oobe", "nag"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "debloat-disable-cloud-content",
            Label = "Disable Cloud-Delivered Content",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft cloud content that powers Spotlight, suggested content, and feature recommendations.",
            Tags = ["debloat", "cloud", "content", "advertising"],
            RegistryKeys = [$@"{Policies}\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableThirdPartySuggestions", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsSpotlightFeatures", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsSpotlightWindowsWelcomeExperience", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsSpotlightOnActionCenter", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableThirdPartySuggestions"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsSpotlightFeatures"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsSpotlightWindowsWelcomeExperience"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsSpotlightOnActionCenter"),
            ],
            DetectOps = [RegOp.CheckDword($@"{Policies}\CloudContent", "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-start-web-search",
            Label = "Disable Web Search in Start Menu",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Start menu searches from querying Bing web results.",
            Tags = ["debloat", "search", "bing", "start-menu"],
            RegistryKeys = [$@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from using diagnostic data to show personalized ads and recommendations.",
            Tags = ["debloat", "advertising", "privacy", "tailored"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "debloat-disable-feedback-notifications",
            Label = "Disable Feedback Hub Notifications",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Feedback Hub from sending survey requests and notifications.",
            Tags = ["debloat", "feedback", "notifications", "nag"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Siuf\Rules"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Siuf\Rules", "PeriodInNanoSeconds", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod"),
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Siuf\Rules", "PeriodInNanoSeconds"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-windows-hello-prompt",
            Label = "Disable Windows Hello Setup Prompt",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from nagging users to set up Windows Hello biometrics.",
            Tags = ["debloat", "hello", "biometrics", "nag", "setup"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-people-bar",
            Label = "Disable People Bar on Taskbar",
            Category = "Debloat",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the People icon and contact integration from the taskbar.",
            Tags = ["debloat", "taskbar", "people", "contacts"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-getting-started-app",
            Label = "Disable Getting Started App",
            Category = "Debloat",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the 'Get Started' tips app from launching after updates or on first login.",
            Tags = ["debloat", "tips", "oobe", "nag", "startup"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoStartPage", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoStartPage")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoStartPage", 1)],
        },
    ];
}
