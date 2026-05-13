namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class Startup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "startup-disable-startup-delay",
            Label = "Disable Startup Delay",
            Category = "System 2",
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
            Id = "startup-disable-cortana-startup",
            Label = "Disable Cortana Startup",
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "startup-disable-first-logon-animation",
            Label = "Disable First Login Animation",
            Category = "System 2",
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
            Id = "startup-start-boot-numlock-on",
            Label = "Set Boot-Up Num Lock to On",
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "startup-set-boot-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the multi-boot OS selection timeout to 3 seconds instead of the default 30. Faster boot on single-OS machines.",
            Tags = ["startup", "boot", "timeout", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "startup-disable-boot-logo",
            Label = "Disable Boot Logo Display",
            Category = "System 2",
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
            Id = "startup-disable-narrator-at-login",
            Label = "Disable Narrator at Login Screen",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "startup-disable-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "System 2",
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
