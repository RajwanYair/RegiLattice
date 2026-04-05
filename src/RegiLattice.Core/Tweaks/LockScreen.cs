namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: LockScreen.cs ===

internal static class LockScreen
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lock-disable-login-blur",
            Label = "Disable Login Background Blur",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the acrylic (frosted glass) blur effect on the sign-in screen background. Shows the full wallpaper. Default: blurred. Recommended: disabled.",
            Tags = ["lockscreen", "login", "blur", "acrylic", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-first-login-animation",
            Label = "Disable First Sign-In Animation",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the 'Hi / We're getting things ready' animation on first login. Speeds up new profile setup. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["lockscreen", "animation", "login", "first-run"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
        },
        new TweakDef
        {
            Id = "lock-disable-network-ui",
            Label = "Disable Network UI on Lock Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the network selection UI on the lock screen. Prevents users from connecting to networks before sign-in. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["lockscreen", "network", "selection", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-password-reveal",
            Label = "Disable Password Reveal Button",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Hides the password reveal (eye) button from credential input fields on the login screen and UAC prompts. Reduces shoulder-surfing risk. Default: shown. Recommended: hidden for shared/kiosk machines.",
            Tags = ["lockscreen", "password", "reveal", "security", "credential"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "lock-hide-sleep-button",
            Label = "Hide Sleep Button from Lock Screen Power Menu",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the Sleep option from the power flyout on the lock screen and Start menu. Prevents accidental sleep on always-on machines. Default: shown. Recommended: hidden for servers/kiosks.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption", 0),
            ],
        },
        new TweakDef
        {
            Id = "lock-hide-hibernate-button",
            Label = "Hide Hibernate Button from Lock Screen Power Menu",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the Hibernate option from the power flyout on the lock screen and Start menu. Prevents accidental hibernation on desktop machines. Default: shown (if hibernate enabled). Recommended: hidden for desktops.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings",
                    "ShowHibernateOption",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "lock-set-lockout-threshold-5",
            Label = "Set Account Lockout Threshold to 5 Attempts",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Locks the account after 5 failed login attempts via registry. Helps mitigate brute-force attacks. Default: 0 (disabled).",
            Tags = ["lockscreen", "lockout", "security", "brute-force"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts", 5),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    "MaxDevicePasswordFailedAttempts",
                    5
                ),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-shutdown-button",
            Label = "Hide Shutdown Button on Login Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the shutdown button from the login screen. Prevents unauthorized shutdowns. Default: shown.",
            Tags = ["lockscreen", "shutdown", "login", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 0),
            ],
        },
        new TweakDef
        {
            Id = "lock-auto-lock-10min",
            Label = "Auto-Lock After 10 Minutes",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the screen saver timeout to 10 minutes with automatic lock. Enhances physical security. Default: no timeout.",
            Tags = ["lock-screen", "auto-lock", "timeout", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs", 600),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs", 600),
            ],
        },
        new TweakDef
        {
            Id = "lock-clear-legal-notice",
            Label = "Clear Legal Notice at Login",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Clears any legal notice caption and text displayed before login. Removes the mandatory 'OK' click before sign-in. Default: none.",
            Tags = ["lock-screen", "legal-notice", "login", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticecaption", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticetext", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticecaption"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticetext"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticecaption", ""),
            ],
        },
        new TweakDef
        {
            Id = "lock-block-picture-password",
            Label = "Block Picture Password for Domain Users",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BlockDomainPicturePassword=1 in System policies. Prevents domain-joined users from using picture gestures as a Windows logon method, ensuring credential-based authentication.",
            Tags = ["lock-screen", "password", "domain", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "BlockDomainPicturePassword", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "BlockDomainPicturePassword"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "BlockDomainPicturePassword", 1),
            ],
        },
        new TweakDef
        {
            Id = "lock-hide-locked-user-display",
            Label = "Do Not Display Locked User Identity",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontDisplayLockedUserId=3 in System policies. Shows a generic icon instead of the user's name and picture on the lock screen when the session is locked.",
            Tags = ["lock-screen", "user", "identity", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLockedUserId", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLockedUserId"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLockedUserId", 3),
            ],
        },
        new TweakDef
        {
            Id = "lock-set-blank-screensaver",
            Label = "Set Screen Saver to Blank Screen",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCRNSAVE.EXE to scrnsave.scr (blank screen) in Desktop. Uses the built-in blank screen saver that simply turns the display black, avoiding GPU usage from animated screen savers.",
            Tags = ["lock-screen", "screensaver", "blank", "power"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"%SystemRoot%\system32\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"%SystemRoot%\system32\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "lock-block-user-info-at-signin",
            Label = "Block Users from Showing Personal Info on Sign-In Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BlockUserFromShowingAccountDetailsOnSignin=1 in System policy. Prevents users from choosing to display their email address, display name, or account picture on the Windows sign-in screen.",
            Tags = ["lock-screen", "sign-in", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockUserFromShowingAccountDetailsOnSignin", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockUserFromShowingAccountDetailsOnSignin"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockUserFromShowingAccountDetailsOnSignin", 1),
            ],
        },
        new TweakDef
        {
            Id = "lock-suppress-user-name-display",
            Label = "Do Not Display Username on Sign-In Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontDisplayUserName=1 in System policies. Hides the user's display name (but not the username entry field) on the Windows sign-in and lock screen tiles.",
            Tags = ["lock-screen", "user", "name", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayUserName", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayUserName")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayUserName", 1)],
        },
        new TweakDef
        {
            Id = "lock-set-logon-async-scripts",
            Label = "Run Logon Scripts Asynchronously",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RunLogonScriptSync=0 in Winlogon. Allows the Windows shell to load before logon scripts finish executing. Significantly speeds up the time from password entry to a usable desktop.",
            Tags = ["lock-screen", "logon", "scripts", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "RunLogonScriptSync", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "RunLogonScriptSync", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "RunLogonScriptSync", 0)],
        },
        new TweakDef
        {
            Id = "lock-disable-spotlight-lock-policy",
            Label = "Disable Windows Spotlight on Lock Screen (Policy)",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightOnLockScreen=1 in the CloudContent user policy key. Prevents the lock screen from fetching and displaying MSN Spotlight background images and facts.",
            Tags = ["lock-screen", "spotlight", "cloud-content", "policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
        },
    ];
}
