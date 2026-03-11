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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
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
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "OemPreInstalledAppsEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 1),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "OemPreInstalledAppsEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0)],
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
        },
        new TweakDef
        {
            Id = "startup-disable-startup-sound",
            Label = "Disable Windows Startup Sound",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Silences the Windows startup sound on boot.",
            Tags = ["startup", "sound", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 1)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen"),
            ],
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-delay",
            Label = "Disable Startup Delay",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the startup delay for programs in the Startup folder. Apps launch immediately at logon. Default: ~10s delay. Recommended: 0 (no delay).",
            Tags = ["startup", "delay", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-app-tracking",
            Label = "Disable Startup App Tracking",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables tracking of which programs are launched from Start menu. Improves privacy and reduces write I/O. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "tracking", "privacy", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1),
            ],
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 0)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "startup-start-disable-startup-delay",
            Label = "Disable Startup Delay for Apps",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the artificial startup delay for desktop applications. Apps launch immediately at logon. Default: ~10s delay. Recommended: 0.",
            Tags = ["startup", "delay", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
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
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "startup-start-disable-tips",
            Label = "Disable Windows Tips on Startup",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestions that appear after login. Reduces startup distractions. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "tips", "suggestions", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "startup-start-disable-app-restart",
            Label = "Disable Automatic App Restart on Login",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from automatically restarting apps that were open before shutdown/restart. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "restart", "apps", "login", "winlogon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
        },
        new TweakDef
        {
            Id = "startup-start-disable-welcome-experience",
            Label = "Disable Windows Welcome Experience",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows welcome experience that shows after updates with feature highlights and suggestions. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "welcome", "experience", "updates", "nag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-310093Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-310093Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-310093Enabled", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-ink-workspace",
            Label = "Hide Windows Ink Workspace Button",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Ink Workspace button from the taskbar. Default: hidden on non-pen devices. Recommended: hidden for clean taskbar.",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-gamebar-capture",
            Label = "Disable Xbox Game Bar App Capture",
            Category = "Startup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Xbox Game Bar app capture (screen recording background task). Reduces startup and background overhead for non-gaming machines. Default: enabled. Recommended: disabled on non-gaming workstations.",
            Tags = ["startup", "gamebar", "xbox", "capture", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR"],
        },
        new TweakDef
        {
            Id = "startup-disable-suggested-app-installs",
            Label = "Disable Suggested App Install Prompts at Startup",
            Category = "Startup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic suggested app installation prompts on startup via system policy. Prevents Store-pushed app suggestions. Default: enabled. Recommended: disabled.",
            Tags = ["startup", "apps", "suggestions", "store", "bloatware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
        },
    ];
}
