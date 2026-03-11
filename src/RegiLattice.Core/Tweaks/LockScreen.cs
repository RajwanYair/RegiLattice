namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LockScreen
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lock-disable-lock-screen",
            Label = "Disable Lock Screen Entirely",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Completely disables the lock screen, going straight to the password/PIN prompt. Default: enabled. Recommended: disabled (home PCs).",
            Tags = ["lockscreen", "disable", "bypass", "login"],
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
            Id = "lock-disable-login-blur",
            Label = "Disable Login Background Blur",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the acrylic (frosted glass) blur effect on the sign-in screen background. Shows the full wallpaper. Default: blurred. Recommended: disabled.",
            Tags = ["lockscreen", "login", "blur", "acrylic", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-first-login-animation",
            Label = "Disable First Sign-In Animation",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Hi / We're getting things ready' animation on first login. Speeds up new profile setup. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["lockscreen", "animation", "login", "first-run"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
        },
        new TweakDef
        {
            Id = "lock-hide-last-username",
            Label = "Hide Last Logged-In Username",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the last logged-in username on the login screen. Users must type their username manually. Default: 0 (show). Recommended: 1 (hide) for security.",
            Tags = ["lockscreen", "username", "security", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLastUserName", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLastUserName", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLastUserName", 1)],
        },
        new TweakDef
        {
            Id = "lock-verbose-login-messages",
            Label = "Enable Verbose Logon Status Messages",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during logon/logoff instead of generic 'Please wait'. Default: not set. Recommended: 1 (verbose).",
            Tags = ["lockscreen", "verbose", "status", "login", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "VerboseStatus", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "VerboseStatus"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-lock-screen-ads",
            Label = "Disable Lock Screen Ads/Tips",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables rotating lock screen tips and advertising overlays. Default: Enabled. Recommended: Disabled.",
            Tags = ["lockscreen", "ads", "tips", "spotlight"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
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
            Id = "lock-disable-tips",
            Label = "Disable Lock Screen Content Tips",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables lock screen content tips and soft-landing suggestions. Default: Enabled. Recommended: Disabled.",
            Tags = ["lockscreen", "tips", "content", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338387Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338387Enabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338387Enabled", 0),
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-network-ui",
            Label = "Disable Network UI on Lock Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the network selection UI on the lock screen. Prevents users from connecting to networks before sign-in. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["lockscreen", "network", "selection", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
        },
        new TweakDef
        {
            Id = "lock-require-ctrl-alt-del",
            Label = "Require Ctrl+Alt+Del on Login Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Requires users to press Ctrl+Alt+Del before the login dialog appears. Prevents keystroke loggers from intercepting credentials. Default: not required. Recommended: required for high-security environments.",
            Tags = ["lockscreen", "ctrl-alt-del", "security", "login", "credentials"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DisableCAD", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DisableCAD", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DisableCAD", 0)],
        },
        new TweakDef
        {
            Id = "lock-disable-password-reveal",
            Label = "Disable Password Reveal Button",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Hides the password reveal (eye) button from credential input fields on the login screen and UAC prompts. Reduces shoulder-surfing risk. Default: shown. Recommended: hidden for shared/kiosk machines.",
            Tags = ["lockscreen", "password", "reveal", "security", "credential"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "lock-hide-sleep-button",
            Label = "Hide Sleep Button from Lock Screen Power Menu",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Sleep option from the power flyout on the lock screen and Start menu. Prevents accidental sleep on always-on machines. Default: shown. Recommended: hidden for servers/kiosks.",
            Tags = ["lockscreen", "sleep", "power", "flyout", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption", 0)],
        },
        new TweakDef
        {
            Id = "lock-hide-hibernate-button",
            Label = "Hide Hibernate Button from Lock Screen Power Menu",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Hibernate option from the power flyout on the lock screen and Start menu. Prevents accidental hibernation on desktop machines. Default: shown (if hibernate enabled). Recommended: hidden for desktops.",
            Tags = ["lockscreen", "hibernate", "power", "flyout", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowHibernateOption", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowHibernateOption"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowHibernateOption", 0)],
        },
        new TweakDef
        {
            Id = "lock-set-lockout-threshold-5",
            Label = "Set Account Lockout Threshold to 5 Attempts",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Locks the account after 5 failed login attempts via registry. Helps mitigate brute-force attacks. Default: 0 (disabled).",
            Tags = ["lockscreen", "lockout", "security", "brute-force"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts", 5)],
        },
        new TweakDef
        {
            Id = "lock-verbose-logon-messages",
            Label = "Enable Verbose Logon Messages",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during logon/logoff (e.g., 'Applying computer settings'). Useful for troubleshooting. Default: disabled.",
            Tags = ["lockscreen", "logon", "verbose", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-shutdown-button",
            Label = "Hide Shutdown Button on Login Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the shutdown button from the login screen. Prevents unauthorized shutdowns. Default: shown.",
            Tags = ["lockscreen", "shutdown", "login", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 0)],
        },
        new TweakDef
        {
            Id = "lock-disable-camera-on-lockscreen",
            Label = "Disable Camera on Lock Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents access to the camera from the lock screen. Enhances physical security. Default: accessible.",
            Tags = ["lock-screen", "camera", "disable", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the slideshow feature on the lock screen. Reduces memory and GPU usage. Default: user-configurable.",
            Tags = ["lock-screen", "slideshow", "disable", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },
    ];
}
