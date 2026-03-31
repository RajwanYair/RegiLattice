namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Notifications
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "notif-disable-action-center",
            Label = "Disable Action Center",
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Notifications",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
            Category = "Widgets & News",
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
