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
            Id = "startup-disable-app-tracking",
            Label = "Disable Startup App Tracking",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables tracking of which programs are launched from Start menu. Improves privacy and reduces write I/O. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "tracking", "privacy", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
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
            Id = "startup-verbose-boot",
            Label = "Enable Verbose Boot Messages",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Shows detailed status messages during boot and shutdown. Default: Disabled. Recommended: Enabled for troubleshooting.",
            Tags = ["startup", "boot", "verbose", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
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
            Id = "startup-start-disable-tips",
            Label = "Disable Windows Tips on Startup",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows tips, tricks, and suggestions that appear after login. Reduces startup distractions. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "tips", "suggestions", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
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
            Id = "startup-disable-ink-workspace",
            Label = "Hide Windows Ink Workspace Button",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Hides the Windows Ink Workspace button from the taskbar. Default: hidden on non-pen devices. Recommended: hidden for clean taskbar.",
            Tags = ["startup", "ink", "pen", "taskbar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
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
            Id = "startup-verbose-boot-messages",
            Label = "Enable Verbose Boot Status Messages",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot and shutdown. Useful for troubleshooting slow startups. Default: disabled.",
            Tags = ["startup", "verbose", "boot", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
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
        new TweakDef
        {
            Id = "startup-disable-autoplay",
            Label = "Disable AutoPlay for All Drives",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables AutoPlay for all media and devices. Prevents automatic program execution when connecting USB drives. Security best-practice.",
            Tags = ["startup", "autoplay", "usb", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
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
            Id = "startup-disable-logon-provider-ads",
            Label = "Disable Login Provider Suggestions",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables sign-in provider suggestions and ads (e.g., PIN setup prompts, Microsoft account suggestions) at the login screen.",
            Tags = ["startup", "login", "ads", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
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
