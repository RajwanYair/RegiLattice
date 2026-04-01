namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Startup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "startup-disable-startup-delay",
            Label = "Disable Startup Delay",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the artificial startup delay for Run-key programs, allowing them to launch immediately at login.",
            Tags = ["startup", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-skype-autostart",
            Label = "Disable Skype Auto-Start",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Skype from the HKCU Run key to prevent auto-start.",
            Tags = ["startup", "skype"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Skype"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Skype for Desktop"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Skype")],
        },
        new TweakDef
        {
            Id = "startup-disable-edge-autostart",
            Label = "Disable Edge Startup Boost & Background",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge's Startup Boost pre-launch and background mode to free memory and reduce startup load.",
            Tags = ["startup", "edge", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BackgroundModeEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-store-autoinstall",
            Label = "Disable Store Suggested App Install",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing suggested apps and OEM bloatware from the Microsoft Store.",
            Tags = ["startup", "bloatware", "store"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled"
                ),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled"),
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
            Id = "startup-disable-teams",
            Label = "Disable Teams Auto-Start",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Microsoft Teams from the HKCU Run key to prevent auto-start.",
            Tags = ["startup", "teams", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "com.squirrel.Teams.Teams"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MicrosoftTeams"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "com.squirrel.Teams.Teams")],
        },
        new TweakDef
        {
            Id = "startup-disable-cortana-startup",
            Label = "Disable Cortana Startup",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Cortana from the HKCU Run key to prevent auto-start at login.",
            Tags = ["startup", "cortana", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CortanaUI"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Cortana"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CortanaUI")],
        },
        new TweakDef
        {
            Id = "startup-disable-login-background",
            Label = "Use Solid Color Login Background",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Replaces the Windows Spotlight / hero image on the login screen with a plain solid color background.",
            Tags = ["startup", "login", "appearance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-lock-screen",
            Label = "Skip Lock Screen (Go Straight to Login)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Bypasses the lock screen so the machine goes directly to the password / PIN prompt on wake or boot.",
            Tags = ["startup", "lockscreen", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-first-logon-animation",
            Label = "Disable First Login Animation",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Hi / We're getting things ready' first-logon animation shown after a new user profile is created.",
            Tags = ["startup", "animation", "login", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-last-known-good",
            Label = "Disable Last Known Good Boot Option",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Last Known Good Configuration boot option. Default: Enabled. Recommended: Disabled for advanced users.",
            Tags = ["startup", "boot", "last-known-good"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-start-boot-numlock-on",
            Label = "Set Boot-Up Num Lock to On",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Num Lock at the Windows login screen by default. Default: Off. Recommended: On for desktop keyboards.",
            Tags = ["startup", "numlock", "keyboard", "boot"],
            RegistryKeys = [@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "startup-start-disable-app-restart",
            Label = "Disable Automatic App Restart on Login",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically restarting apps that were open before shutdown/restart. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "restart", "apps", "login", "winlogon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
        },
        new TweakDef
        {
            Id = "startup-start-disable-welcome-experience",
            Label = "Disable Windows Welcome Experience",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows welcome experience that shows after updates with feature highlights and suggestions. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "welcome", "experience", "updates", "nag"],
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
            Id = "startup-disable-auto-restart-apps",
            Label = "Disable Auto Restart of Apps After Sign-In",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from reopening apps that were open before restart/sign-out. Default: enabled.",
            Tags = ["startup", "auto-restart", "apps", "sign-in"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-lock-screen-app-notifications",
            Label = "Disable Lock Screen App Notifications on Startup",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables app notifications on the lock screen during startup. Reduces startup resource usage. Default: enabled.",
            Tags = ["startup", "lock-screen", "notifications", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-login-background-image",
            Label = "Disable Login Screen Background Image",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Uses a solid colour instead of the Windows Spotlight image on the login screen. Faster render. Default: image.",
            Tags = ["startup", "login", "background", "spotlight"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-gamebar-capture",
            Label = "Disable Game Bar Startup Capture",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Game Bar background capture service from running at startup. Saves CPU and memory if game recording is not needed. Default: enabled.",
            Tags = ["startup", "gamebar", "capture", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-suggested-app-installs",
            Label = "Disable Suggested App Auto-Installs",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from silently installing suggested apps from the Microsoft Store. Stops bloatware from appearing after updates. Default: enabled.",
            Tags = ["startup", "suggested", "apps", "bloatware"],
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
            Id = "startup-disable-welcome-experience",
            Label = "Disable Windows Welcome Experience",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows Welcome Experience that shows after updates to highlight new features. Faster post-update boot.",
            Tags = ["startup", "welcome", "experience", "updates"],
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
            Id = "startup-disable-tips-and-suggestions",
            Label = "Disable Tips and Suggestions Notifications",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestion notifications that appear periodically. Reduces distractions.",
            Tags = ["startup", "tips", "suggestions", "notifications"],
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
            Id = "startup-disable-get-even-more-out-of-windows",
            Label = "Disable 'Get Even More Out of Windows' Nag",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 'Get even more out of Windows' full-screen nag that promotes Microsoft services. Default: enabled.",
            Tags = ["startup", "nag", "promotion", "microsoft"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled"),
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
            Id = "startup-disable-prelaunch-apps",
            Label = "Disable App Pre-Launch at Login",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows pre-launching UWP apps in the background at login. Reduces memory usage and speeds up login.",
            Tags = ["startup", "prelaunch", "uwp", "performance"],
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
            Id = "startup-disable-background-apps-policy",
            Label = "Disable Background Apps via Policy",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables all UWP background apps via Group Policy. Saves CPU and battery. Individual app permissions still apply. Default: enabled.",
            Tags = ["startup", "background", "uwp", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
        },
        new TweakDef
        {
            Id = "startup-set-boot-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the multi-boot OS selection timeout to 3 seconds instead of the default 30. Faster boot on single-OS machines.",
            Tags = ["startup", "boot", "timeout", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
        },
        new TweakDef
        {
            Id = "startup-disable-firstlogon-animation",
            Label = "Disable First Sign-In Animation",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Skips the 'Hi, we're getting things ready' animation after first login or profile creation. Boots straight to desktop.",
            Tags = ["startup", "animation", "firstlogon", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-windows-tips-finish-setup",
            Label = "Disable Suggested Content in Settings",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from showing suggested content and ads in the Settings app. Reduces promotional distractions.",
            Tags = ["startup", "settings", "suggestions", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled"
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
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "startup-disable-tablet-mode-prompt",
            Label = "Disable Tablet Mode Switch Prompt",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the 'Do you want to switch to tablet mode?' prompt on login for convertible devices.",
            Tags = ["startup", "tablet", "login", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-signin-info-reopen",
            Label = "Disable Sign-In Info for App Reopen",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from using sign-in info to automatically finish setting up and reopen apps after restart.",
            Tags = ["startup", "signin", "privacy", "reopen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-boot-logo",
            Label = "Disable Boot Logo Display",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the Windows boot logo animation for faster POST-to-desktop times.",
            Tags = ["startup", "boot", "logo", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-auto-maintenance",
            Label = "Disable Automatic Maintenance at Startup",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows automatic maintenance that runs background tasks on startup. Reduces early boot CPU/disk load.",
            Tags = ["startup", "maintenance", "performance", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-narrator-at-login",
            Label = "Disable Narrator at Login Screen",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Narrator auto-start at the Windows login screen.",
            Tags = ["startup", "narrator", "accessibility", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger",
                    @"%SystemRoot%\System32\systray.exe"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger",
                    @"%SystemRoot%\System32\systray.exe"
                ),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables fast user switching at login. Simplifies the login screen and slightly reduces memory usage on shared PCs.",
            Tags = ["startup", "login", "user-switching", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-edge-prelaunch",
            Label = "Disable Edge Pre-Launch at Login",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Edge from pre-launching in the background at login. Reduces startup memory and CPU usage.",
            Tags = ["startup", "edge", "prelaunch", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-prefetch-on-ssd",
            Label = "Disable Prefetch for SSD Boot Drives",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables boot and application prefetch on SSD boot drives. SSDs have fast random access, so prefetch adds unnecessary write wear.",
            Tags = ["startup", "prefetch", "ssd", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Program Compatibility Assistant that checks applications for compatibility issues at launch.",
            Tags = ["startup", "compatibility", "performance", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
    ];
}

// ── merged from Boot.cs ────────────────────────────────────────
internal static class Boot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "boot-numlock-on-boot",
            Label = "Enable NumLock at Boot",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Turns on NumLock automatically at the login screen. Options: 0=Off, 2=On. Default: 0 (Off). Recommended: On.",
            Tags = ["boot", "keyboard", "numlock"],
            RegistryKeys = [@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "boot-disable-secboot-check",
            Label = "Suppress Secure Boot Status Check",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses the Secure Boot status notification in Windows by setting UEFISecureBootEnabled to 0 in the registry.",
            Tags = ["boot", "security", "uefi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-anim",
            Label = "Disable Boot Animation/Spinner",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows boot animation/spinner for a faster perceived boot. The boot process skips the animated dots. Default: enabled. Recommended: disabled for faster boot.",
            Tags = ["boot", "animation", "performance", "spinner"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-enable-fast-startup",
            Label = "Enable Fast Startup (Hiberboot)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Windows Fast Startup which uses a hybrid shutdown with hibernation to speed up boot time. Default: Usually enabled. Recommended: Enabled for fast boot.",
            Tags = ["boot", "fast-startup", "hiberboot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-prefetch-optimized",
            Label = "Set Prefetch to Optimized Mode",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables both boot and application prefetching for optimal performance. Value 3 = boot + app prefetch. Default: 3. Recommended: 3 for SSDs and HDDs.",
            Tags = ["boot", "prefetch", "performance", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-full-bsod-info",
            Label = "Show Full BSOD Parameters",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Shows detailed crash parameters on the Blue Screen of Death. Displays bug-check code and arguments for troubleshooting. Default: Hidden. Recommended: Shown for diagnostics.",
            Tags = ["boot", "bsod", "parameters", "crash", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
        },


        new TweakDef
        {
            Id = "boot-clear-pagefile",
            Label = "Clear Pagefile at Shutdown",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Clears the virtual memory pagefile at every shutdown. Prevents sensitive data from being recovered from pagefile.sys. Note: significantly increases shutdown time on large systems. Default: not cleared. Recommended: Apply on secure workstations.",
            Tags = ["boot", "security", "pagefile", "shutdown", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-enable-boot-log",
            Label = "Enable Boot Log",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the boot log file (ntbtlog.txt) that records all drivers loaded during startup. Useful for diagnosing boot issues. Default: disabled.",
            Tags = ["boot", "log", "diagnostics", "drivers"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-ux",
            Label = "Disable Boot UI Animation",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows boot animation (spinning dots). Shows a simple progress bar instead. Default: animated.",
            Tags = ["boot", "animation", "ui", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-timeout-5s",
            Label = "Set Boot Menu Timeout to 5 Seconds",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager menu timeout to 5 seconds for dual-boot systems. Default: 30 seconds.",
            Tags = ["boot", "timeout", "dual-boot", "menu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 30),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
        },
        new TweakDef
        {
            Id = "boot-verbose-status-messages",
            Label = "Enable Verbose Boot Status Messages",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot, shutdown, logon, and logoff. Default: hidden.",
            Tags = ["boot", "verbose", "status", "messages"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-windows-recovery",
            Label = "Disable Windows Recovery on Boot Failure",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic recovery attempts after boot failures. Prevents boot repair loops. Default: recovery enabled.",
            Tags = ["boot", "recovery", "auto-repair", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
        },
        // ── Command-based boot tweaks (bcdedit) ────────────────────────────
        new TweakDef
        {
            Id = "boot-bcd-quiet-boot",
            Label = "Enable Quiet Boot (bcdedit)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows quiet boot mode via bcdedit — suppresses the boot logo and status messages for faster boot appearance.",
            Tags = ["boot", "bcdedit", "quiet", "logo"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds (bcdedit)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager timeout to 3 seconds via bcdedit. Speeds up boot when multi-boot options exist.",
            Tags = ["boot", "bcdedit", "timeout", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "3"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("3", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-disable-recovery",
            Label = "Disable Automatic Recovery (bcdedit)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic recovery/repair environment via bcdedit. Prevents boot loops but removes automatic repair capability.",
            Tags = ["boot", "bcdedit", "recovery", "repair", "server"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Disables automatic repair on boot failure.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "no"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "yes"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("recoveryenabled", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-driver-verifier-reset",
            Label = "Reset Driver Verifier (verifier)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Resets Driver Verifier settings to none. Useful after debugging driver issues when verifier was left enabled.",
            Tags = ["boot", "verifier", "driver", "diagnostic", "reset"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("verifier", ["/reset"]);
            },
            // NOTE: No RemoveAction — "reset" is a one-shot diagnostic action. There is no
            // meaningful inverse; re-enabling verifier requires choosing specific drivers.
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("verifier", ["/query"]);
                return stdout.Contains("No drivers", StringComparison.OrdinalIgnoreCase)
                    || stdout.Contains("not loaded", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── Restored stubs with real operations ──────────────────

        new TweakDef
        {
            Id = "boot-disable-auto-repair",
            Label = "Disable Automatic Startup Repair",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows from launching Automatic Repair after consecutive boot failures. Use with caution.",
            Tags = ["boot", "auto-repair", "recovery", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "System will not auto-recover from boot failures.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootstatuspolicy", "IgnoreAllFailures"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "bootstatuspolicy"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootstatuspolicy", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("IgnoreAllFailures", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-boot-logo",
            Label = "Disable Boot Logo (bcdedit)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows boot logo via bcdedit for a minimalist boot screen.",
            Tags = ["boot", "logo", "bcdedit", "ux"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "quietboot"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-driver-verifier",
            Label = "Disable Driver Verifier Flags",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears Driver Verifier flags in the registry. Useful after debugging when verifier causes boot loops.",
            Tags = ["boot", "verifier", "driver", "registry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-logo",
            Label = "Disable OEM Boot Logo",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the OEM manufacturer logo during boot via bcdedit nologo option.",
            Tags = ["boot", "logo", "oem", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{globalsettings}", "custom:16000067", "true"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{globalsettings}", "custom:16000067"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{globalsettings}"]);
                return stdout.Contains("16000067", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-winre",
            Label = "Disable WinRE Partition",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Recovery Environment. Frees recovery partition but removes repair tools.",
            Tags = ["boot", "winre", "recovery", "disk-space"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Removes access to Windows Recovery tools.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/disable"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/enable"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("reagentc", ["/info"]);
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-boot-timeout",
            Label = "Set Boot Timeout to 0 Seconds",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets BCD boot menu timeout to 0 seconds for instant boot-through. No OS selection screen shown.",
            Tags = ["boot", "timeout", "bcdedit", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "0"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("0", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-ignore-boot-failures",
            Label = "Ignore All Boot Failures",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures Windows to ignore all boot failures and skip the recovery screen. Use on stable systems only.",
            Tags = ["boot", "failures", "policy", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            SideEffects = "Boot failures will not trigger automatic repair.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-log",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables boot logging via bcdedit. Writes driver load info to %%SystemRoot%%\\ntbtlog.txt.",
            Tags = ["boot", "logging", "bcdedit", "diagnostic"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootlog", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-max-proc-count",
            Label = "Use All CPU Cores at Boot",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures msconfig-equivalent setting to use all processor cores during boot.",
            Tags = ["boot", "cpu", "cores", "performance", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "numproc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return !stdout.Contains("numproc", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-menu-timeout",
            Label = "Set Boot Menu Timeout to 10s",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot menu display timeout to 10 seconds. Useful for dual-boot systems.",
            Tags = ["boot", "timeout", "menu", "dual-boot", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "10"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("10", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-verbose-boot",
            Label = "Enable Verbose Boot Messages",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot instead of the logo. Useful for debugging slow boot.",
            Tags = ["boot", "verbose", "diagnostic", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "on"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "off"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("sos", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-startup-gpo",
            Label = "Enable Fast Startup via Group Policy",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HiberbootEnabled=1 in the Windows System policy key to enforce fast startup at GPO level. Complements the standard fast startup registry setting. Default: not set.",
            Tags = ["boot", "fast-startup", "policy", "hibernate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-global-wait-timeout",
            Label = "Set Global Shutdown Wait Timeout to 5s",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WaitForIdleState=5 in the system Timeout key. Controls how long Windows waits for the system to become idle before shutdown completes. Default: 2.",
            Tags = ["boot", "shutdown", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
        },
        new TweakDef
        {
            Id = "boot-menu-timeout-policy",
            Label = "Set Boot Menu Display Timeout Policy to 10s",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BootTimeoutSeconds=10 in the Windows System policy key. Controls the boot menu display time at policy level. Default: not set (uses BCD value).",
            Tags = ["boot", "menu", "timeout", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
        },
        new TweakDef
        {
            Id = "boot-hyperv-launch-off",
            Label = "Disable Hyper-V Hypervisor Launch",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Runs 'bcdedit /set hypervisorlaunchtype off' to disable the Hyper-V hypervisor at boot. Improves native performance on bare-metal gaming/workstation installs. Default: auto.",
            Tags = ["boot", "hyper-v", "bcd", "performance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "auto"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-test-signing-off",
            Label = "Disable Test Signing Mode",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set testsigning off' to disable test-signing mode. Prevents unsigned test drivers from loading. Default: off.",
            Tags = ["boot", "bcd", "security", "test-signing"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "on"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("testsigning", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-report-ok",
            Label = "Enable Boot-OK Reporting to Winlogon",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ReportBootOk=1 in Winlogon to signal that the current boot is clean and should be saved as the last known good configuration. Default: 1.",
            Tags = ["boot", "winlogon", "last-known-good", "recovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
        },
        new TweakDef
        {
            Id = "boot-kernel-debug-filter",
            Label = "Suppress Kernel Debug Print Filter",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DEFAULT=0x0 in the Debug Print Filter to suppress kernel debug messages, reducing DbgPrint overhead on retail builds. Default: 0x8 or not set.",
            Tags = ["boot", "kernel", "debug", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
        },
        new TweakDef
        {
            Id = "boot-winre-policy-allow",
            Label = "Allow Windows Recovery Environment Policy",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWinRE=0 in WinRE policy to ensure the Windows Recovery Environment remains accessible. Prevents accidental policy lockout of recovery tools. Default: 0.",
            Tags = ["boot", "recovery", "winre", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
        },
        new TweakDef
        {
            Id = "boot-legacy-f8-menu",
            Label = "Enable Legacy F8 Boot Menu",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set {bootmgr} displaybootmenu yes' to enable the legacy F8 boot menu. Allows access to safe mode and other startup options. Default: off on modern Windows.",
            Tags = ["boot", "bcd", "safe-mode", "f8"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "yes"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("displaybootmenu", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-nx-optin",
            Label = "Set Data Execution Prevention to OptIn",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set nx OptIn' to enable DEP (Data Execution Prevention) only for OS-protected processes. Balances security and compatibility. Default: OptIn.",
            Tags = ["boot", "bcd", "dep", "security"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("nx", StringComparison.OrdinalIgnoreCase) && stdout.Contains("OptIn", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-startup-app-delay",
            Label = "Disable Startup App Launch Delay",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets StartupDelayInMSec=0 to eliminate the artificial delay Windows introduces before launching registered startup applications. Speeds up the post-login experience. Default: 10-second delay.",
            Tags = ["boot", "startup", "delay", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-livedump",
            Label = "Disable Kernel Live Dump Collection",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Kernel Live Dump collection (EnableLiveDump=0). Live dumps are taken by heuristics without a full crash; disabling reduces unexpected disk I/O and performance spikes. Default: enabled.",
            Tags = ["boot", "dump", "kernel", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-crash-alert",
            Label = "Disable Admin Alert on System Crash",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the administrator alert notification (SendAlert=0) that Windows generates on a fatal system crash. Reduces noise in environments where crashes are monitored externally. Default: 0 (disabled by default on most builds).",
            Tags = ["boot", "crash", "alert", "admin"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
        new TweakDef
        {
            Id = "boot-enable-nmi-crash-dump",
            Label = "Enable NMI-Triggered Crash Dump",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables triggering a crash dump via a Non-Maskable Interrupt (NMI) button or debugger. Useful for generating a dump on a completely hung system that cannot respond to other input. Default: disabled.",
            Tags = ["boot", "nmi", "dump", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-bsod-beep",
            Label = "Disable System Beep on BSOD",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the PC speaker beep that Windows emits when a BSOD (blue screen of death) occurs. Reduces noise in server rooms or overnight unattended machines. Default: 1 (beep enabled).",
            Tags = ["boot", "bsod", "beep", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-always-keep-dump",
            Label = "Do Not Permanently Keep Memory Dump",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AlwaysKeepMemoryDump=0 so Windows does not permanently retain the memory dump even when low on disk. Lets the pagefile cleanup process remove the dump to free space. Default: 0.",
            Tags = ["boot", "dump", "disk", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-system-eventlog-size",
            Label = "Increase System Event Log Size to 50 MB",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the System event log maximum size to 50 MB (52428800 bytes). Allows retention of more historical system events before wrapping. Default: 20 MB.",
            Tags = ["boot", "event-log", "system", "size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-status-display",
            Label = "Disable Boot Status / Spinner Display",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the display of boot status messages (spinner/dots) during startup by clearing DisplayStatusMessages. Produces a cleaner, faster-feeling boot sequence. Default: enabled.",
            Tags = ["boot", "ui", "spinner", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
        },

    ];
}
