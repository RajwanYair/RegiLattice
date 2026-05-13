namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Notifications.cs ──
[TweakModule]
internal static class Notifications
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "notif-disable-toast",
            Label = "Disable Toast Notifications",
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Id = "notif-disable-startup-app-notif",
            Label = "Disable Background App Notifications",
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Id = "notif-disable-defender-user-notif",
            Label = "Suppress Windows Defender Notifications",
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Id = "notif-disable-smartscreen-user",
            Label = "Disable SmartScreen Evaluation Notifications (User)",
            Category = "Windows 11 2",
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
            Category = "Windows 11 2",
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
            Category = "Windows 11 3",
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
            Id = "notif-disable-no-logged-users-reboot",
            Label = "Allow Reboot with Logged-On Users (Windows Update)",
            Category = "Windows 11 3",
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
