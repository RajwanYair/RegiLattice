namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "PhoneLinkEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink", "PhoneLinkEnabled"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 3),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableNotificationSync", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableNotificationSync", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableCDP", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDP", "EnableCDP"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteLaunch", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableRemoteLaunch", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableSmsSync", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableSmsSync", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 2),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass", "UserAuthPolicy", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartGlass", "UserAuthPolicy", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableShare", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableShare", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "EnableShare", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-suggestions",
            Label = "Disable Phone Link Suggestions",
            Category = "Phone Link",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Phone Link promotional suggestions and tips in Windows. Default: Enabled.",
            Tags = ["phone-link", "suggestions", "ads"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhoneLink"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "BluetoothLastUsedSend", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "BluetoothLastUsedSend"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "BluetoothLastUsedSend", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-wifidirect",
            Label = "Disable Phone Link Wi-Fi Direct",
            Category = "Phone Link",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Wi-Fi Direct transport used by Phone Link for high-speed cross-device data transfer. Reduces background radio use. Default: Enabled.",
            Tags = ["phone-link", "wifi", "wifi-direct", "network", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "WifiDirectEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "WifiDirectEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "WifiDirectEnabled", 0)],
        },
        new TweakDef
        {
            Id = "phone-disable-timeline",
            Label = "Disable Windows Timeline Activity Feed",
            Category = "Phone Link",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Timeline activity feed and cross-device clipboard via policy. Stops syncing browsing and app activity to Microsoft cloud. Default: Enabled.",
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
            Description = "Disables automatic phone photo synchronisation via the Phone Link app. Prevents background syncing of photos to the PC. Default: Enabled.",
            Tags = ["phone-link", "photos", "sync", "privacy", "storage"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone", "DisablePhotoSync", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone", "DisablePhotoSync", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\YourPhone", "DisablePhotoSync", 1)],
        },
        new TweakDef
        {
            Id = "phone-disable-app-notifications",
            Label = "Disable Phone Link App Notifications",
            Category = "Phone Link",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables cross-device app notifications relayed from mobile to PC via Phone Link. Reduces notification noise and background connectivity. Default: Enabled.",
            Tags = ["phone-link", "notifications", "apps", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP\SettingsPage"],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\PersistedStorageItemTable\ManagedByApp", "AutoStartEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\PersistedStorageItemTable\ManagedByApp", "AutoStartEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\PersistedStorageItemTable\ManagedByApp", "AutoStartEnabled", 0)],
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
    ];
}
