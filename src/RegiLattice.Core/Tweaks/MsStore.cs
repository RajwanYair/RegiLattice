namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MsStore
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msstore-disable-store",
            Label = "Disable Microsoft Store",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Microsoft Store via group policy. Default: enabled. Recommended: disabled.",
            Tags = ["store", "microsoft", "policy", "bloat"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-auto-install",
            Label = "Disable Auto-Install of Suggested Apps",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic installation of suggested apps. Default: enabled. Recommended: disabled.",
            Tags = ["store", "auto-install", "suggestions", "bloat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-auto-update",
            Label = "Disable Store App Auto-Updates",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic updates of Store apps. Default: enabled. Recommended: disabled.",
            Tags = ["store", "updates", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
        },
        new TweakDef
        {
            Id = "msstore-disable-tips",
            Label = "Disable Windows Tips About Store",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips and suggestions about the Store. Default: enabled. Recommended: disabled.",
            Tags = ["store", "tips", "suggestions", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-preinstalled",
            Label = "Disable Preinstalled Apps",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables preinstalled apps from being installed on new accounts. Default: enabled. Recommended: disabled.",
            Tags = ["store", "preinstalled", "bloat", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-consumer-features",
            Label = "Disable Consumer Features / App Suggestions",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows consumer features and app suggestions. Default: enabled. Recommended: disabled.",
            Tags = ["store", "consumer", "suggestions", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-feedback",
            Label = "Disable Feedback Notifications",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables feedback notification prompts. Default: enabled. Recommended: disabled.",
            Tags = ["store", "feedback", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-spotlight",
            Label = "Disable Windows Spotlight",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Spotlight on the lock screen. Default: enabled. Recommended: disabled.",
            Tags = ["store", "spotlight", "lockscreen", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenOverlayEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenOverlayEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-app-suggestions-start",
            Label = "Disable App Suggestions in Start",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables app suggestions in the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["store", "start", "suggestions", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SystemPaneSuggestionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SystemPaneSuggestionsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-content-delivery",
            Label = "Disable Content Delivery",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables content delivery entirely. Default: enabled. Recommended: disabled.",
            Tags = ["store", "content", "delivery", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-push-install",
            Label = "Disable Remote Push-to-Install",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables remote push-to-install from Microsoft Store. Prevents apps from being silently installed via the web store. Default: enabled. Recommended: disabled.",
            Tags = ["store", "push", "install", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-consumer-experiences",
            Label = "Disable Windows Consumer Experiences (Policy)",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows consumer experiences via Group Policy. Prevents bloatware, suggested apps, and consumer account content. Default: enabled. Recommended: disabled.",
            Tags = ["store", "consumer", "bloatware", "policy", "experiences"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableConsumerAccountStateContent", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableConsumerAccountStateContent"),
            ],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-auto-install-suggested",
            Label = "Disable Auto-Install of Suggested Apps",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables silent auto-install of suggested apps via ContentDeliveryManager. Prevents Microsoft from pushing unwanted app installations. Default: enabled. Recommended: disabled.",
            Tags = ["store", "auto-install", "suggested", "silent", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-app-recommendations",
            Label = "Disable Store App Recommendations",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables app recommendation content via SubscribedContent policies. Prevents promoted app suggestions in Start and Settings. Default: enabled. Recommended: disabled.",
            Tags = ["store", "recommendations", "subscribed", "content", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-video-autoplay",
            Label = "Disable Store Video Autoplay",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables video autoplay in the Microsoft Store app. Default: enabled. Recommended: disabled.",
            Tags = ["store", "video", "autoplay", "media"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 0)],
        },
        new TweakDef
        {
            Id = "msstore-oem-apps-disable",
            Label = "Disable OEM Pre-Installed App Delivery",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents ContentDeliveryManager from installing OEM-bundled apps silently on new accounts or upgrades. Default: enabled. Recommended: disabled.",
            Tags = ["store", "oem", "preinstalled", "bloatware", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "OemPreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "OemPreInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "OemPreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-feature-mgmt-disable",
            Label = "Disable Store Feature Management Experiments",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables ContentDeliveryManager feature management, preventing Microsoft from running A/B experiments that silently enable new Store and content features. Default: enabled. Recommended: disabled.",
            Tags = ["store", "feature-management", "experiments", "cdm", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-post-upgrade-apps",
            Label = "Disable Post-Upgrade App Restoration",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from reinstalling Store apps after a feature upgrade or clean install via ContentDeliveryManager. Default: enabled. Recommended: disabled.",
            Tags = ["store", "post-upgrade", "apps", "bloatware", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WindowsPostUpgradeEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WindowsPostUpgradeEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WindowsPostUpgradeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-start-tips",
            Label = "Disable Cortana/Bing Tips in Start Menu",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables SubscribedContent-280810 delivery which pushes Cortana and Bing tips into the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["store", "cortana", "bing", "tips", "start", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-280810Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-280810Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-280810Enabled", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions (Policy)",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party app suggestions from appearing in Windows via the CloudContent group policy key. Prevents promoted apps in Start, Settings, and lock screen. Default: allowed. Recommended: blocked.",
            Tags = ["store", "third-party", "suggestions", "cloud-content", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
        },
    ];
}
