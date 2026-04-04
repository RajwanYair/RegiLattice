namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from MsStore.cs ────────────────────────────────────────
internal static class MsStore
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msstore-disable-store",
            Label = "Disable Microsoft Store",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "msstore-disable-app-suggestions-start",
            Label = "Disable App Suggestions in Start",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "msstore-soft-landing-off",
            Label = "Disable Store soft-landing tips on first run",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
            Tags = ["msstore", "ads", "advertising", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
        },
    ];
}

// ── merged from SnapMultitasking.cs ────────────────────────────────────────
internal static class SnapMultitasking
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "snap-disable-snap-assist",
            Label = "Disable Snap Assist",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable showing suggestions when snapping windows. Default: enabled. Recommended: personal preference.",
            Tags = ["snap", "assist", "window", "suggestion"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-layouts",
            Label = "Disable Snap Layouts Flyout",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable the hover-over maximize button Snap Layouts flyout (Win11). Default: enabled.",
            Tags = ["snap", "layouts", "flyout", "maximize", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-groups",
            Label = "Disable Snap Groups in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable grouping Snap windows in Alt+Tab and taskbar. Default: enabled.",
            Tags = ["snap", "groups", "alt-tab", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 0)],
        },
        new TweakDef
        {
            Id = "snap-alttab-windows-only",
            Label = "Alt+Tab: Open Windows Only",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Show only open windows in Alt+Tab, not browser tabs. Default: includes Edge tabs.",
            Tags = ["alt-tab", "tabs", "edge", "windows"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-aero-shake",
            Label = "Disable Aero Shake",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable shaking a window title bar to minimise all others. Default: enabled.",
            Tags = ["aero", "shake", "minimize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "snap-vd-all-monitors",
            Label = "Show Desktops on All Monitors",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Show virtual desktop windows on all monitors in taskbar. Default: current monitor only.",
            Tags = ["virtual-desktop", "monitor", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-auto-arrange",
            Label = "Disable Auto-Arrange on Dock",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatic window rearrangement when docking/undocking. Default: enabled.",
            Tags = ["dock", "arrange", "resize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-fill",
            Label = "Disable Snap Fill Available Space",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatically filling available space when snapping a window. Default: enabled.",
            Tags = ["snap", "fill", "space", "resize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-suggestions",
            Label = "Disable Snap Window Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable AI/suggested windows when snapping. Default: enabled.",
            Tags = ["snap", "suggestion", "ai"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0),
            ],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-dwm-anim-policy",
            Label = "Disable DWM Animations (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Machine-wide policy to disable Desktop Window Manager animations. Default: enabled.",
            Tags = ["dwm", "animation", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations", 1)],
        },
        new TweakDef
        {
            Id = "snap-disable-edge-swipe-nav",
            Label = "Disable Edge Swipe Navigation",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables edge swipe navigation gestures on touchscreens. Default: Enabled. Recommended: Disabled on desktops.",
            Tags = ["snap", "edge", "swipe", "gesture", "touch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-flyout",
            Label = "Disable Snap Fly-Out Overlay",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the snap bar fly-out overlay when dragging windows. Reduces visual clutter during window arrangement. Default: Enabled. Recommended: Disabled.",
            Tags = ["snap", "flyout", "overlay", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-resize-snap",
            Label = "Disable Window Resize Snap Assist",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables joint resize when dragging the border between two snapped windows. Prevents accidental resizing of adjacent snapped windows. Default: Enabled. Recommended: Disabled.",
            Tags = ["snap", "resize", "joint", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-fly-out",
            Label = "Disable Snap Layouts Fly-Out",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the snap layouts fly-out shown when hovering over maximize button. Default: enabled.",
            Tags = ["snap", "layouts", "fly-out", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-edge-snapping",
            Label = "Disable Window Edge Snap (Aero Snap)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Aero Snap (dragging windows to screen edges). Default: enabled.",
            Tags = ["snap", "aero", "edge", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
        },
        new TweakDef
        {
            Id = "snap-set-virtual-desktop-show-all-taskbar",
            Label = "Show All Virtual Desktop Windows in Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows windows from all virtual desktops in the taskbar, instead of only the current desktop. Default: current desktop only.",
            Tags = ["snap", "virtual-desktop", "taskbar", "windows"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-alt-tab-edge-tabs",
            Label = "Disable Edge Tabs in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Microsoft Edge tabs from appearing in the Alt+Tab switcher. Shows only open windows. Default: recent 5 tabs.",
            Tags = ["snap", "alt-tab", "edge", "tabs"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-desktop-peek",
            Label = "Disable Desktop Peek",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the desktop peek feature when hovering over the Show Desktop button. Prevents accidental window hiding. Default: enabled.",
            Tags = ["snap", "desktop", "peek", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop", 1),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-vd-edge-swipe",
            Label = "Disable Virtual Desktop Edge Swipe",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the touchpad edge swipe gesture for switching virtual desktops. Prevents accidental desktop switches. Default: enabled.",
            Tags = ["snap", "virtual-desktop", "swipe", "gesture"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableEdgeSwipe", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableEdgeSwipe")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "snap-vd-switch-anim-speed",
            Label = "Speed Up Virtual Desktop Switch Animation",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces the virtual desktop switch animation duration. Makes workspace switching feel more responsive. Default: standard speed.",
            Tags = ["snap", "virtual-desktop", "animation", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VDDesktopIconsEnabled", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VDDesktopIconsEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VDDesktopIconsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-edge-snap",
            Label = "Disable Edge Snapping",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables automatic window snapping when dragging to screen edges. Default: enabled.",
            Tags = ["snap", "edge", "window", "drag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
        },
        new TweakDef
        {
            Id = "snap-disable-shake-minimize",
            Label = "Disable Aero Shake to Minimize",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables Aero Shake — shaking a window no longer minimizes all other windows. Default: enabled.",
            Tags = ["snap", "shake", "minimize", "aero"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "snap-disable-corner-snap",
            Label = "Disable Corner Snap",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables window snapping to corners (quarter-screen layout). Default: enabled.",
            Tags = ["snap", "corner", "quarter", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-auto-snap-fill",
            Label = "Disable Snap Fill",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables automatic filling of available space when snapping a window. Default: enabled.",
            Tags = ["snap", "fill", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-across-monitors",
            Label = "Disable Snap Across Monitors",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Prevents windows from snapping across monitor boundaries in multi-monitor setups. Default: enabled.",
            Tags = ["snap", "monitor", "multi-display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiMonSnap", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiMonSnap")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiMonSnap", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-task-view-button",
            Label = "Disable Task View Button",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Removes the Task View button from the taskbar. Task View is still accessible via Win+Tab. Default: shown.",
            Tags = ["taskview", "taskbar", "button", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-timeline",
            Label = "Disable Activity History / Timeline",
            Category = "Windows 11",
            NeedsAdmin = true,
            Description = "Disables Windows Timeline and activity history collection. Default: enabled.",
            Tags = ["timeline", "activity", "history", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-recent-apps-multitasking",
            Label = "Disable Recent Apps in Multitasking View",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Hides recent applications from the Alt+Tab and Task View multitasking interfaces. Default: shown.",
            Tags = ["snap", "recent", "alt-tab", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "snap-hide-edge-tabs-alt-tab",
            Label = "Hide Edge Tabs in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description =
                "Prevents Microsoft Edge browser tabs from appearing in the Alt+Tab window switcher. Shows only windows. Default: 5 recent tabs.",
            Tags = ["alt-tab", "edge", "tabs", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-suggest",
            Label = "Disable Snap Layout Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables the automatic suggestion overlay when snapping a window. Default: enabled.",
            Tags = ["snap", "suggest", "layout", "overlay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
        },
    ];
}

// ── Merged from VirtualDesktops.cs ──────────────────────────────────────────────────

internal static class VirtualDesktops
{
    private const string VdKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VirtualDesktops";

    private const string TaskView = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string TaskViewPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    private const string AltTabKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string DwmKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vd-show-all-windows-in-alt-tab",
            Label = "Show All VD Windows in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "task view", "windows"],
            Description =
                "Configures Alt+Tab to show windows from all virtual desktops (default "
                + "Win11 behavior). Value 1 = all desktops. Useful if a previous policy "
                + "restricted Alt+Tab to the current desktop only.",
            ApplyOps = [RegOp.SetDword(AltTabKey, "VirtualDesktopAltTabFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(AltTabKey, "VirtualDesktopAltTabFilter")],
            DetectOps = [RegOp.CheckDword(AltTabKey, "VirtualDesktopAltTabFilter", 1)],
        },
        new TweakDef
        {
            Id = "vd-show-current-desktop-in-alt-tab",
            Label = "Show Only Current VD Windows in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "focus", "productivity"],
            Description =
                "Restricts Alt+Tab to only show windows from the current virtual desktop. "
                + "Value 2 = current desktop only. Reduces clutter when using many desktops.",
            ApplyOps = [RegOp.SetDword(AltTabKey, "VirtualDesktopAltTabFilter", 2)],
            RemoveOps = [RegOp.DeleteValue(AltTabKey, "VirtualDesktopAltTabFilter")],
            DetectOps = [RegOp.CheckDword(AltTabKey, "VirtualDesktopAltTabFilter", 2)],
        },
        new TweakDef
        {
            Id = "vd-show-all-taskbar-buttons",
            Label = "Show Taskbar Buttons from All Virtual Desktops",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "buttons", "all desktops"],
            Description =
                "Configures the taskbar to show app buttons from all virtual desktops "
                + "instead of only the current one. Value 1 = show all. Useful for quick "
                + "cross-desktop app switching without Task View.",
            ApplyOps = [RegOp.SetDword(TaskView, "VirtualDesktopTaskbarFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "VirtualDesktopTaskbarFilter")],
            DetectOps = [RegOp.CheckDword(TaskView, "VirtualDesktopTaskbarFilter", 1)],
        },
        new TweakDef
        {
            Id = "vd-show-current-taskbar-buttons",
            Label = "Show Only Current VD Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "current desktop", "focus"],
            Description =
                "Restricts the taskbar to only show buttons for apps open on the current "
                + "virtual desktop. Value 2 = current desktop only. Keeps the taskbar clean "
                + "when using many virtual desktops.",
            ApplyOps = [RegOp.SetDword(TaskView, "VirtualDesktopTaskbarFilter", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "VirtualDesktopTaskbarFilter")],
            DetectOps = [RegOp.CheckDword(TaskView, "VirtualDesktopTaskbarFilter", 2)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-button",
            Label = "Hide Task View Button from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "task view", "taskbar", "button", "clean"],
            Description =
                "Hides the Task View (multi-desktop) button from the taskbar. "
                + "Virtual desktops remain functional via Win+Tab or Win+Ctrl+D. "
                + "Reduces taskbar clutter on single-user desktops.",
            ApplyOps = [RegOp.SetDword(TaskView, "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-system",
            Label = "Disable Task View Feature via Policy",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["virtual desktops", "task view", "policy", "disable"],
            Description =
                "Disables the Task View (Win+Tab) feature entirely via system policy. "
                + "Users cannot access virtual desktops, and the shortcut is disabled. "
                + "Intended for kiosk/corporate lockdown environments.",
            ApplyOps = [RegOp.SetDword(TaskViewPolicy, "DisableTaskView", 1)],
            RemoveOps = [RegOp.DeleteValue(TaskViewPolicy, "DisableTaskView")],
            DetectOps = [RegOp.CheckDword(TaskViewPolicy, "DisableTaskView", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-timeline",
            Label = "Disable Windows Timeline",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "timeline", "activity history", "privacy"],
            Description =
                "Disables the Windows Timeline feature in Task View that tracks your "
                + "recent activity and documents. Prevents activity history collection "
                + "and removes the timeline from Task View.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-timeline-upload",
            Label = "Disable Timeline Activity Sync to Cloud",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "timeline", "cloud sync", "privacy", "microsoft account"],
            Description =
                "Prevents Timeline activity history from being uploaded to Microsoft cloud "
                + "servers (requires a Microsoft account). Activity remains local only even "
                + "if Timeline is still enabled.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-assist-flyout",
            Label = "Disable Snap Assist Desktop Flyout on Switch",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "flyout", "animation", "ux"],
            Description =
                "Disables the snap group flyout that appears when hovering over taskbar "
                + "buttons during virtual desktop operations. Reduces UI clutter for power "
                + "users who prefer compact taskbar behavior.",
            ApplyOps = [RegOp.SetDword(TaskView, "EnableSnapAssistFlyout", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "EnableSnapAssistFlyout", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "EnableSnapAssistFlyout", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-desktops-on-taskbar",
            Label = "Disable Virtual Desktop Previews on Taskbar Hover",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "preview", "hover"],
            Description =
                "Disables the virtual desktop thumbnail previews that appear when hovering "
                + "over the Task View button. Saves screen space and reduces compositor load "
                + "on hover-intensive workflows.",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskView", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "TaskView", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskView", 0)],
        },
        new TweakDef
        {
            Id = "vd-set-taskbar-multimonitor-all",
            Label = "Show All Desktop Windows on All Monitor Taskbars",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "windows"],
            Description =
                "Configures multi-monitor taskbars to show buttons for windows "
                + "from all virtual desktops (MMTaskbarMode=0). The default shows "
                + "only the current desktop on each monitor's taskbar.",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarMode", 0)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "MMTaskbarMode")],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "vd-set-taskbar-multimonitor-local-only",
            Label = "Show Only Local Monitor Windows on Each Monitor Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "focus"],
            Description =
                "Configures multi-monitor taskbars to show only windows that are "
                + "open on that specific monitor (MMTaskbarMode=2). Reduces clutter "
                + "on multi-monitor setups.",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarMode", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "MMTaskbarMode")],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarMode", 2)],
        },
        new TweakDef
        {
            Id = "vd-disable-aero-peek",
            Label = "Disable Aero Peek (Desktop Peek Overlay)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "aero", "peek", "dwm", "transparency"],
            Description =
                "Disables the Aero Peek feature that toggles window transparency to "
                + "show the desktop when hovering over the 'Show Desktop' corner button. "
                + "Removes the glass overlay effect (EnableAeroPeek=0).",
            ApplyOps = [RegOp.SetDword(DwmKey, "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.SetDword(DwmKey, "EnableAeroPeek", 1)],
            DetectOps = [RegOp.CheckDword(DwmKey, "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-fill",
            Label = "Disable Snap Fill (Auto-Fill Adjacent Window After Snap)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "fill", "layout"],
            Description =
                "Prevents Windows from prompting you to fill the remaining screen area "
                + "after snapping a window. Only the snapped window moves; no assist popup "
                + "appears for the other half (SnapFill=0).",
            ApplyOps = [RegOp.SetDword(TaskView, "SnapFill", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "SnapFill", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "SnapFill", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-revert",
            Label = "Disable Snap Revert (Don't Move Partner Window When Moving Dragged Window)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "revert", "layout"],
            Description =
                "When dragging a snapped window away from its position, disables the "
                + "automatic reverting of the paired window to its pre-snap size and position "
                + "(SnapRevert=0). Gives more predictable behavior.",
            ApplyOps = [RegOp.SetDword(TaskView, "SnapRevert", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "SnapRevert", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "SnapRevert", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-taskbar-grouping",
            Label = "Never Group Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "grouping", "buttons"],
            Description =
                "Prevents the taskbar from grouping multiple windows of the same app "
                + "into a single button. Each open window gets its own dedicated button "
                + "regardless of how many are open (TaskbarGlomLevel=2).",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "TaskbarGlomLevel")],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskbarGlomLevel", 2)],
        },
        new TweakDef
        {
            Id = "vd-enable-background-per-desktop",
            Label = "Enable Unique Wallpaper Per Virtual Desktop",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "wallpaper", "background", "personalization"],
            Description =
                "Allows each virtual desktop to have its own wallpaper that changes "
                + "automatically when you switch desktops. Enables the per-desktop background "
                + "feature (BackgroundChangesOnDesktopSwitch=1).",
            ApplyOps = [RegOp.SetDword(VdKey, "BackgroundChangesOnDesktopSwitch", 1)],
            RemoveOps = [RegOp.SetDword(VdKey, "BackgroundChangesOnDesktopSwitch", 0)],
            DetectOps = [RegOp.CheckDword(VdKey, "BackgroundChangesOnDesktopSwitch", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-alt-tab-thumbnails",
            Label = "Delay Alt+Tab Thumbnail Preview (Effectively Disable Hover Thumbnails)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "thumbnails", "hover", "performance"],
            Description =
                "Sets the taskbar thumbnail hover delay to 30 seconds, effectively "
                + "preventing thumbnail previews from appearing while still keeping the "
                + "feature technically enabled (ExtendedUIHoverTime=30000 ms).",
            ApplyOps = [RegOp.SetDword(TaskView, "ExtendedUIHoverTime", 30000)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "ExtendedUIHoverTime")],
            DetectOps = [RegOp.CheckDword(TaskView, "ExtendedUIHoverTime", 30000)],
        },
        new TweakDef
        {
            Id = "vd-show-taskbar-on-secondary-monitors",
            Label = "Show Taskbar on All Monitors (Multi-Monitor)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "secondary"],
            Description =
                "Enables the extended taskbar on secondary monitors so each display "
                + "shows its own taskbar. Useful when the multi-monitor taskbar was "
                + "previously disabled (MMTaskbarEnabled=1).",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarEnabled", 1)],
            RemoveOps = [RegOp.SetDword(TaskView, "MMTaskbarEnabled", 0)],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarEnabled", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-taskbar-end-task-button",
            Label = "Disable End Task Button on Taskbar (Windows 11 23H2+)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "end-task", "windows 11"],
            Description =
                "Removes the 'End Task' button that appears in the right-click context "
                + "menu for taskbar buttons in Windows 11 version 23H2 and later. "
                + "Prevents accidental process termination (TaskbarEndTask=0).",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskbarEndTask", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "TaskbarEndTask", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskbarEndTask", 0)],
        },
    ];
}

// ── merged from Notifications.cs ──
internal static class Notifications
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "notif-disable-action-center",
            Label = "Disable Action Center",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows Action Center sidebar. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "action-center", "sidebar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter", 1)],
        },
        new TweakDef
        {
            Id = "notif-disable-toast",
            Label = "Disable Toast Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pop-up toast notifications from all applications. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "toast", "popup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications",
                    "NoToastApplicationNotification",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications",
                    "NoToastApplicationNotification"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications",
                    "NoToastApplicationNotification",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-lock-screen",
            Label = "Disable Lock Screen Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents notifications from appearing on the lock screen. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "lock-screen", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-sounds",
            Label = "Disable Notification Sounds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Silences all notification sounds system-wide. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "sounds", "audio"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-suggestions",
            Label = "Disable Windows Suggestions / Tips",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Windows from showing tips, tricks, and suggestion notifications. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "suggestions", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-welcome",
            Label = "Disable Windows Welcome Experience",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows welcome experience shown after updates. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "welcome", "updates"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-finish-setup",
            Label = "Disable Finish Setting Up Reminders",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the recurring 'finish setting up your device' reminders. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "setup", "reminders"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement",
                    "ScoobeSystemSettingEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-app-suggestions",
            Label = "Disable Suggested Apps in Start",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from suggesting apps in the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "suggestions", "start-menu"],
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
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    1
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
            Id = "notif-quiet-hours-auto",
            Label = "Auto-enable Quiet Hours",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables quiet hours (focus assist) to suppress all toast notifications. Default: disabled. Recommended: enabled.",
            Tags = ["notifications", "quiet-hours", "focus-assist"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-startup-app-notif",
            Label = "Disable Background App Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables 'apps are running in the background' system toast notifications. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "background", "startup"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-quiet-hours",
            Label = "Disable Push Toast Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all push toast notifications globally. Default: Enabled. Recommended: Disabled for focus.",
            Tags = ["notifications", "toast", "push", "quiet-hours"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-display-time-3s",
            Label = "Set Notification Display Time to 3 Seconds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets notification display duration to 3 seconds instead of the default 5. Reduces visual distraction. Default: 5s. Recommended: 3s.",
            Tags = ["notifications", "display-time", "duration", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 5)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 3)],
        },
        new TweakDef
        {
            Id = "notif-disable-security-center",
            Label = "Disable Windows Security Center Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables toast notifications from the Windows Security and Maintenance center. Reduces interruptions from security alerts. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "security", "maintenance", "center"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-autoconnect",
            Label = "Disable Auto Connect Network Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables notifications from the AutoConnect (hotspot) system toast. Reduces Wi-Fi connection prompt interruptions. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "network", "autoconnect", "wifi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-account-notif",
            Label = "Disable Microsoft Account Connected Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables notification toasts from Microsoft account connected services. Stops account sync prompts and MSA-linked notifications. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "account", "microsoft", "msa"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-lock-screen-toasts",
            Label = "Disable Notifications on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents app toast notifications from displaying on the lock screen. Protects notification content from shoulder-surfers. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "lock-screen", "privacy", "toast"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-notification-sounds",
            Label = "Disable Notification Sounds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all Windows notification sounds. Toasts still appear silently. Default: enabled.",
            Tags = ["notifications", "sounds", "mute", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-reduce-toast-duration",
            Label = "Reduce Toast Notification Duration",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the time toast notifications are displayed from 5 seconds to 3 seconds. Default: 5000ms.",
            Tags = ["notifications", "toast", "duration", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", "5")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", "3")],
        },
        new TweakDef
        {
            Id = "notif-disable-suggested-notifications",
            Label = "Disable Suggested Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows suggesting you finish setting up your device via notifications. Default: enabled.",
            Tags = ["notifications", "suggested", "setup", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement",
                    "ScoobeSystemSettingEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-suggested-actions",
            Label = "Disable Suggested Actions Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested actions that appear after copying phone numbers or dates. Default: enabled.",
            Tags = ["notifications", "suggested-actions", "clipboard", "popup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-banners",
            Label = "Disable Notification Banners",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables notification banner popups on the desktop. Notifications go silently to Action Center only. Default: banners shown.",
            Tags = ["notifications", "banners", "toast", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0)],
        },
        new TweakDef
        {
            Id = "notif-silence-global-sounds",
            Label = "Silence All Notification Sounds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Silences all Windows notification sounds globally. Visual notifications still appear but without audio. Default: sounds enabled.",
            Tags = ["notifications", "sounds", "silence", "mute"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-low-disk-alert",
            Label = "Disable Low Disk Space Alert",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows popup warnings about low disk space on drives. Default: Enabled.",
            Tags = ["notifications", "disk", "low-disk", "alert"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-defender-user-notif",
            Label = "Suppress Windows Defender Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Suppresses Windows Defender security notifications that appear during scans and threat detections.",
            Tags = ["notifications", "defender", "antivirus", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 1)],
        },
        new TweakDef
        {
            Id = "notif-disable-reboot-required",
            Label = "Disable Windows Update Reboot Required Notification",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows Update from nagging users to reboot when an update is pending installation.",
            Tags = ["notifications", "windows-update", "reboot", "nagging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationDisable", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationDisable"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationDisable", 1),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-balloon-tips",
            Label = "Disable System Tray Balloon Tips",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables legacy balloon tips from system tray icons. Default: enabled. Modern toast notifications are not affected.",
            Tags = ["notifications", "balloon", "tray", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 0)],
        },
        new TweakDef
        {
            Id = "notif-disable-smartscreen-user",
            Label = "Disable SmartScreen Evaluation Notifications (User)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables per-user SmartScreen web content evaluation, suppressing SmartScreen block and warning notifications.",
            Tags = ["notifications", "smartscreen", "browser", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost", "EnableWebContentEvaluation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost", "EnableWebContentEvaluation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost", "EnableWebContentEvaluation", 0)],
        },
        new TweakDef
        {
            Id = "notif-disable-taskbar-suggestions",
            Label = "Disable Taskbar & Start Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft-promoted content appearing in the taskbar system pane. Removes commercial suggestions from the system tray area.",
            Tags = ["notifications", "taskbar", "suggestions", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "SystemPaneSuggestionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "SystemPaneSuggestionsEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "SystemPaneSuggestionsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-oem-preinstall-suggestions",
            Label = "Disable OEM Preinstalled App Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from suggesting OEM preinstalled applications and cloud app links via notification banners.",
            Tags = ["notifications", "oem", "preinstall", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "OemPreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "OemPreInstalledAppsEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "OemPreInstalledAppsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-clear-recent-on-exit",
            Label = "Clear Recent Items List on Exit",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically clears Recent Documents (MRU) lists each time the user logs off. Prevents leaving access trail.",
            Tags = ["notifications", "recent-docs", "privacy", "mru"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-no-logged-users-reboot",
            Label = "Allow Reboot with Logged-On Users (Windows Update)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows Update from rebooting while a user is logged on. Eliminates surprise forced-reboot notifications.",
            Tags = ["notifications", "windows-update", "reboot", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "NoAutoRebootWithLoggedOnUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "NoAutoRebootWithLoggedOnUsers")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
        },
    ];
}

// === Merged from: Widgets.cs ===

internal static class Widgets
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "widgets-news-disable-widgets-panel",
            Label = "Disable Widgets Panel (Win11)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows 11 Widgets panel (Weather/News/Sports). Removes the Widgets button from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "taskbar", "win11"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0)],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-news-interests",
            Label = "Disable News and Interests (Win10)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the News and Interests taskbar widget in Win10. Removes the weather/news flyout from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "interests", "taskbar", "win10"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-welcome-experience",
            Label = "Disable Welcome Experience",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows Welcome Experience page that opens after updates to show new features. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "welcome", "update", "experience"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-get-more",
            Label = "Disable 'Get Even More Out of Windows'",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the 'Get Even More Out of Windows' popup and similar Microsoft 365 / OneDrive nag prompts. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "suggestions", "nag", "onedrive", "m365"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353696Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353696Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-start-suggestions",
            Label = "Disable Suggested Apps in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables suggested (promoted) apps in the Start menu. Stops Microsoft Store app recommendations. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "start", "suggestions", "apps", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
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
                    "SubscribedContent-338388Enabled"
                ),
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
            Id = "widgets-news-disable-settings-suggestions",
            Label = "Disable Suggested Content in Settings",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested content and feature highlights in the Windows Settings app. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "settings", "suggestions", "content"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353698Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353698Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353698Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-finish-setup",
            Label = "Disable 'Finish Setting Up' Reminder",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the recurring 'Let's finish setting up your device' nag screen after updates. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "finish-setup", "nag", "reminder"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-feeds-taskbar",
            Label = "Disable News and Interests on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables news and interests feed on the Windows taskbar via policy. Removes the weather/news widget from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "feeds", "taskbar"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-widget-disable-news-feed",
            Label = "Disable News Feed Content",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the news feed content delivery in Widgets panel via policy. Stops news articles from loading in the background. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "feed", "content"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-widget-remove-weather-taskbar",
            Label = "Remove Weather from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the weather widget from the taskbar by setting view mode to hidden. Default: shown. Recommended: hidden.",
            Tags = ["widgets", "weather", "taskbar", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2)],
        },
        new TweakDef
        {
            Id = "widgets-disable-machine-feeds",
            Label = "Disable Windows Feeds via Machine Policy",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Feeds (News and Interests) via machine-level policy, applying to all users on the system. Default: Enabled. Recommended: Disabled.",
            Tags = ["widgets", "feeds", "news", "policy", "machine"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows from showing suggestions for third-party apps in search and feeds. Reduces promotional content. Default: Enabled. Recommended: Disabled.",
            Tags = ["widgets", "suggestions", "third-party", "ads", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "ThirdPartySuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "ThirdPartySuggestionsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "ThirdPartySuggestionsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-widget-board",
            Label = "Disable Widgets Board Completely",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Fully disables the Windows 11 Widgets board via Group Policy. Default: enabled.",
            Tags = ["widgets", "board", "disable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-open-on-hover",
            Label = "Disable Widgets Open on Hover",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Widgets panel from opening when hovering over the taskbar icon. Default: open on hover.",
            Tags = ["widgets", "hover", "taskbar", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight",
            Label = "Disable Windows Spotlight on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Spotlight images and tips on the lock screen. Default: enabled.",
            Tags = ["widgets", "spotlight", "lock-screen", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-tips-tricks-suggestions",
            Label = "Disable Tips, Tricks, and Suggestions",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Tips, Tricks, and Suggestions notifications from Windows. Default: enabled.",
            Tags = ["widgets", "tips", "suggestions", "notifications"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
        },
        new TweakDef
        {
            Id = "widgets-disable-start-personalization",
            Label = "Disable Start Menu Personalization",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables personalized content recommendations in the Start menu. Removes suggested apps and content cards. Default: enabled.",
            Tags = ["widgets", "start", "personalization", "recommendations"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-subscribed-content",
            Label = "Disable Subscribed Content Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft subscribed content suggestions shown in Settings and lock screen. Removes premium service promotions. Default: enabled.",
            Tags = ["widgets", "subscribed", "content", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-feeds-policy",
            Label = "Disable Windows Feeds / News Bar (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFeeds=0 in the Windows Feeds policy key. Disables the news and interests feed bar at the group-policy level, covering devices where HKCU-level tweaks are overridden.",
            Tags = ["widgets", "news", "feeds", "gpo", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimised Content in Widgets",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableCloudOptimizedContent=1 in CloudContent policy. Stops Windows from fetching and displaying cloud-optimised widget content including personalised tiles.",
            Tags = ["widgets", "cloud", "content", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
        },
        new TweakDef
        {
            Id = "widgets-disable-lock-app-notifications",
            Label = "Disable App Notifications on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLockScreenAppNotifications=1 in CloudContent policy. Prevents app toast notifications from appearing on the lock screen.",
            Tags = ["widgets", "lock-screen", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableLockScreenAppNotifications", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableLockScreenAppNotifications"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableLockScreenAppNotifications", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-in-settings-gpo",
            Label = "Disable Windows Spotlight in Settings App (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightInSettings=1. Prevents the Settings app from showing Spotlight-sourced feature suggestions and background images.",
            Tags = ["widgets", "spotlight", "settings", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSettings", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSettings"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSettings", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-welcome-gpo",
            Label = "Disable Windows Spotlight Welcome Experience (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1. Disables the full-screen Spotlight highlight shown after major Windows updates.",
            Tags = ["widgets", "spotlight", "welcome", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableWindowsSpotlightWindowsWelcomeExperience",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableWindowsSpotlightWindowsWelcomeExperience"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableWindowsSpotlightWindowsWelcomeExperience",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-action-center-gpo",
            Label = "Disable Windows Spotlight in Action Center (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightOnActionCenter=1. Stops Spotlight suggestions from being injected into the Windows Action Center notification area.",
            Tags = ["widgets", "spotlight", "action-center", "gpo"],
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
            Id = "widgets-disable-tailored-diag-experiences",
            Label = "Disable Tailored Experiences from Diagnostic Data (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableTailoredExperiencesWithDiagnosticData=1. Prevents Windows from using collected diagnostic data to show personalised tips, advertising, and feature recommendations.",
            Tags = ["widgets", "tailored", "diagnostic", "privacy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-taskbar-meet-now",
            Label = "Hide Teams Meet Now Button from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets TaskbarMn=0 in Explorer settings. Removes the Teams \"Meet Now\" button from the Windows 10/11 system tray area.",
            Tags = ["widgets", "taskbar", "teams", "meet-now", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "TaskbarMn", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "TaskbarMn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "TaskbarMn", 0)],
        },
        new TweakDef
        {
            Id = "widgets-gpo-disable-third-party-spotlight",
            Label = "Disable Third-Party Suggestions in Spotlight (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableThirdPartySuggestions=1 in CloudContent policy. Prevents third-party apps and advertisers from appearing in Windows Spotlight lock-screen and Start suggestions.",
            Tags = ["widgets", "spotlight", "third-party", "ads", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-in-search-gpo",
            Label = "Disable Windows Spotlight in Search Interface (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightInSearch=1. Removes Spotlight-sourced background images and suggestions from the Windows Search home panel.",
            Tags = ["widgets", "spotlight", "search", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSearch", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSearch"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSearch", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-soft-landing-tips",
            Label = "Disable Windows Soft-Landing Feature Tips (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSoftLanding=1 in CloudContent policy. Prevents the initial \"tip overlay\" that appears over new features after Windows is installed or updated.",
            Tags = ["widgets", "tips", "soft-landing", "debloat", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
        },
    ];
}

