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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore")],
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
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload")],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
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
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled"),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenEnabled",
                    0
                ),
            ],
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
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-push-install",
            Label = "Disable Remote Push-to-Install",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables remote push-to-install from Microsoft Store. Prevents apps from being silently installed via the web store. Default: enabled. Recommended: disabled.",
            Tags = ["store", "push", "install", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-consumer-experiences",
            Label = "Disable Windows Consumer Experiences (Policy)",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows consumer experiences via Group Policy. Prevents bloatware, suggested apps, and consumer account content. Default: enabled. Recommended: disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableConsumerAccountStateContent", 1),
            ],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-auto-install-suggested",
            Label = "Disable Auto-Install of Suggested Apps",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables silent auto-install of suggested apps via ContentDeliveryManager. Prevents Microsoft from pushing unwanted app installations. Default: enabled. Recommended: disabled.",
            Tags = ["store", "auto-install", "suggested", "silent", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 0)],
        },
        new TweakDef
        {
            Id = "msstore-oem-apps-disable",
            Label = "Disable OEM Pre-Installed App Delivery",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents ContentDeliveryManager from installing OEM-bundled apps silently on new accounts or upgrades. Default: enabled. Recommended: disabled.",
            Tags = ["store", "oem", "preinstalled", "bloatware", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-feature-mgmt-disable",
            Label = "Disable Store Feature Management Experiments",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables ContentDeliveryManager feature management, preventing Microsoft from running A/B experiments that silently enable new Store and content features. Default: enabled. Recommended: disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "FeatureManagementEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-post-upgrade-apps",
            Label = "Disable Post-Upgrade App Restoration",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from reinstalling Store apps after a feature upgrade or clean install via ContentDeliveryManager. Default: enabled. Recommended: disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "WindowsPostUpgradeEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-start-tips",
            Label = "Disable Cortana/Bing Tips in Start Menu",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables SubscribedContent-280810 delivery which pushes Cortana and Bing tips into the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["store", "cortana", "bing", "tips", "start", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-280810Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-280810Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-280810Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions (Policy)",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks third-party app suggestions from appearing in Windows via the CloudContent group policy key. Prevents promoted apps in Start, Settings, and lock screen. Default: allowed. Recommended: blocked.",
            Tags = ["store", "third-party", "suggestions", "cloud-content", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-push-notifications",
            Label = "Disable Store Push Notifications",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables push notifications from the Microsoft Store. Default: enabled.",
            Tags = ["msstore", "notifications", "push", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisablePushNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisablePushNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisablePushNotifications", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-private-store-only",
            Label = "Restrict to Private Store Only",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts Microsoft Store to only show private store apps (enterprise). Default: all apps visible.",
            Tags = ["msstore", "private-store", "enterprise", "restrict"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePrivateStoreOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePrivateStoreOnly")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePrivateStoreOnly", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-silent-app-installs",
            Label = "Disable Silent App Installations",
            Category = "Microsoft Store",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Store from silently installing suggested apps. Default: enabled.",
            Tags = ["msstore", "silent", "install", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-app-suggestions",
            Label = "Disable App Suggestions in Start Menu",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables app suggestions (ads) in the Start menu from the Microsoft Store. Default: enabled.",
            Tags = ["msstore", "suggestions", "start-menu", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-app-recommendations",
            Label = "Disable Store App Recommendations",
            Category = "Microsoft Store",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables app recommendation popups from the Microsoft Store. Prevents promotional content in the Store app. Default: enabled.",
            Tags = ["msstore", "recommendations", "apps", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-video-autoplay-off",
            Label = "Disable Store video autoplay",
            Category = "Microsoft Store",
            Tags = ["msstore", "video", "autoplay"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "VideoAutoplay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "VideoAutoplay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "VideoAutoplay", 0)],
        },
        new TweakDef
        {
            Id = "msstore-oem-preinstall-off",
            Label = "Disable OEM-preinstalled app recommendations",
            Category = "Microsoft Store",
            Tags = ["msstore", "oem", "preinstall", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-preinstalled-apps-off",
            Label = "Disable pre-installed app reinstallation",
            Category = "Microsoft Store",
            Tags = ["msstore", "preinstall", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-silent-installs-off",
            Label = "Disable silent app installations",
            Category = "Microsoft Store",
            Tags = ["msstore", "silent", "install", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-consumer-features-off",
            Label = "Disable Windows consumer features (Store suggestions)",
            Category = "Microsoft Store",
            Tags = ["msstore", "consumer", "bloat", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
            ],
        },
        new TweakDef
        {
            Id = "msstore-rotating-lock-screen-off",
            Label = "Disable Windows Spotlight on lock screen",
            Category = "Microsoft Store",
            Tags = ["msstore", "spotlight", "lockscreen", "privacy"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-soft-landing-off",
            Label = "Disable Store soft-landing tips on first run",
            Category = "Microsoft Store",
            Tags = ["msstore", "onboarding", "tips"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-content-delivery-off",
            Label = "Disable content delivery manager entirely",
            Category = "Microsoft Store",
            Tags = ["msstore", "content", "delivery", "privacy", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-subscription-content-off",
            Label = "Disable Store subscription content highlights",
            Category = "Microsoft Store",
            Tags = ["msstore", "subscription", "privacy"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContentEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContentEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContentEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-require-purchase-auth",
            Label = "Require admin authorization for Microsoft Store purchases",
            Category = "Microsoft Store",
            Tags = ["msstore", "purchase", "authorization", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePurchaseAuthorization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePurchaseAuthorization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePurchaseAuthorization", 1)],
        },
        new TweakDef
        {
            Id = "msstore-store-autodownload-off",
            Label = "Disable Microsoft Store automatic app downloads via GPO",
            Category = "Microsoft Store",
            Tags = ["msstore", "autodownload", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
        },
        new TweakDef
        {
            Id = "msstore-disable-store-apps-policy",
            Label = "Disable Microsoft Store application access via GPO",
            Category = "Microsoft Store",
            Tags = ["msstore", "disable", "gpo", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisableStoreApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisableStoreApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisableStoreApps", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-contentdelivery-allowed",
            Label = "Disable all Windows content delivery (master CDM switch)",
            Category = "Microsoft Store",
            Tags = ["msstore", "content-delivery", "cdm", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-welcome-app",
            Label = "Disable Windows welcome experience / app suggestion notifications",
            Category = "Microsoft Store",
            Tags = ["msstore", "welcome", "notification", "cdm"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WelcomeAppEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WelcomeAppEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WelcomeAppEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-subscribed-338380",
            Label = "Disable SubscribedContent-338380 (Start menu app suggestions)",
            Category = "Microsoft Store",
            Tags = ["msstore", "subscribed-content", "suggestions", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338380Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338380Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338380Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-subscribed-310091",
            Label = "Disable SubscribedContent-310091 (Windows welcome experience highlights)",
            Category = "Microsoft Store",
            Tags = ["msstore", "subscribed-content", "welcome", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310091Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310091Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310091Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-subscribed-314559",
            Label = "Disable SubscribedContent-314559 (social media / tips highlights)",
            Category = "Microsoft Store",
            Tags = ["msstore", "subscribed-content", "social", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-enterprise-cloud-store",
            Label = "Disable Windows Store for Business / Enterprise cloud integration",
            Category = "Microsoft Store",
            Tags = ["msstore", "enterprise", "business", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "EnableWindowsStoreForBusiness", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "EnableWindowsStoreForBusiness")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "EnableWindowsStoreForBusiness", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-adinfo",
            Label = "Disable Windows personalized advertising ID",
            Category = "Microsoft Store",
            Tags = ["msstore", "ads", "advertising", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
        },
    ];
}
