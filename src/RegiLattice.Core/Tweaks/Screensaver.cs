namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Screensaver
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ss-disable-screensaver",
            Label = "Disable Screensaver",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable the screensaver. Default: enabled. Recommended: keep enabled with password.",
            Tags = ["screensaver", "disable", "screen"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-timeout-5m",
            Label = "Screensaver Timeout: 5 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Set screensaver to activate after 5 minutes. Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "5min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
        },
        new TweakDef
        {
            Id = "ss-timeout-10m",
            Label = "Screensaver Timeout: 10 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Set screensaver to activate after 10 minutes. Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "10min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
        },
        new TweakDef
        {
            Id = "ss-blank-screensaver",
            Label = "Set Blank (Black) Screensaver",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Set screensaver to plain black screen. Default: none. Recommended: blank for OLED.",
            Tags = ["screensaver", "blank", "black", "oled"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", "C:\\Windows\\System32\\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", "C:\\Windows\\System32\\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "ss-enable-secure-desktop",
            Label = "Enable Secure Desktop for UAC",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Show UAC prompts on secure desktop (anti-spoofing). Default: enabled. Recommended: enabled.",
            Tags = ["uac", "secure-desktop", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 0)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1),
            ],
        },
        new TweakDef
        {
            Id = "ss-disable-lock-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disable slideshow on the lock screen. Default: enabled.",
            Tags = ["lock", "slideshow", "screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "ss-enable-transparency",
            Label = "Enable Transparency Effects",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enable window transparency/acrylic effects. Default: enabled.",
            Tags = ["transparency", "acrylic", "effects", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1),
            ],
        },
        new TweakDef
        {
            Id = "ss-scr-timeout-10min",
            Label = "Set Screensaver Timeout to 10 Minutes (Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets screensaver timeout to 10 minutes via machine policy. Enforced across all users. Default: varies. Recommended: 600 seconds.",
            Tags = ["screensaver", "timeout", "10min", "policy"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Control Panel\Desktop",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
            ],
        },
        new TweakDef
        {
            Id = "ss-scr-disable-screensaver",
            Label = "Disable Screensaver Completely (Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the screensaver completely via machine policy. Prevents screensaver from activating on any user. Default: Enabled. Recommended: Disabled only for kiosks.",
            Tags = ["screensaver", "disable", "policy"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Control Panel\Desktop",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-set-screensaver-timeout-300",
            Label = "Set Screensaver Timeout to 5 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets screensaver activation timeout to 300 seconds (5 minutes). Default: 900 (15 min).",
            Tags = ["screensaver", "timeout", "lock", "idle"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
        },
        new TweakDef
        {
            Id = "ss-require-password-on-resume",
            Label = "Require Password on Screensaver Resume",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Requires password when resuming from screensaver. Critical for security. Default: varies.",
            Tags = ["screensaver", "password", "lock", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
        },
        new TweakDef
        {
            Id = "ss-set-screensaver-blank",
            Label = "Set Screensaver to Blank (Most Efficient)",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the screensaver to the blank (black screen) option. Lowest power usage. Default: none.",
            Tags = ["screensaver", "blank", "power", "efficiency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "ss-disable-screen-saver-policy",
            Label = "Disable Screen Saver via Group Policy",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the screen saver using Group Policy. Overrides user settings. Default: not configured.",
            Tags = ["screensaver", "policy", "gpo", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-lock-screen-timeout-60",
            Label = "Set Lock Screen Timeout to 60 Seconds",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the console lock display-off timeout to 60 seconds. Screen turns off faster on lock screen. Default: 60 (Windows default, but often changed by OEMs).",
            Tags = ["screensaver", "lock-screen", "timeout", "display"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
                    "Attributes"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "ss-disable-user-policy",
            Label = "Disable Screen Saver (User)",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the screen saver for the current user. Screen will stay on until manually locked or display timeout triggers. Default: enabled.",
            Tags = ["screensaver", "disable", "user", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-force-policy",
            Label = "Force Screen Saver via Policy",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces screen saver activation via Group Policy. Overrides user preferences. Default: not enforced.",
            Tags = ["screensaver", "force", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "1")],
        },
        new TweakDef
        {
            Id = "ss-prevent-screensaver-change",
            Label = "Prevent Screen Saver Changes",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from changing the screen saver settings. Locks the current screen saver configuration. Default: allowed.",
            Tags = ["screensaver", "prevent", "change", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispScrSavPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispScrSavPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispScrSavPage", 1)],
        },
        new TweakDef
        {
            Id = "ss-require-password",
            Label = "Require Password on Screen Saver Resume",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Requires a password to unlock after screen saver activates. Enhances physical security. Default: not required.",
            Tags = ["screensaver", "password", "resume", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
        },
        new TweakDef
        {
            Id = "ss-scr-password-on-resume",
            Label = "Enforce Password on Resume (Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces password requirement on screen saver resume via Group Policy. Machine-wide enforcement. Default: not enforced.",
            Tags = ["screensaver", "password", "resume", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure", "1"),
            ],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-15min",
            Label = "Set Screen Saver Timeout to 15 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the screen saver activation timeout to 15 minutes (900 seconds). Balances security with usability. Default: 10 minutes.",
            Tags = ["screensaver", "timeout", "15min", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-30min",
            Label = "Set Screen Saver Timeout to 30 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the screen saver activation timeout to 30 minutes (1800 seconds). Longer timeout for active use. Default: 10 minutes.",
            Tags = ["screensaver", "timeout", "30min", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "1800")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "1800")],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-5min",
            Label = "Set Screensaver Timeout to 5 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            Description = "Sets the screensaver activation timeout to 5 minutes (300 seconds). Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "lock", "5-min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-10min",
            Label = "Set Screensaver Timeout to 10 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            Description = "Sets the screensaver activation timeout to 10 minutes (600 seconds). Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "lock", "10-min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
        },
        new TweakDef
        {
            Id = "ss-enable-password-on-resume",
            Label = "Require Password on Resume",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Requires password after screensaver deactivation. Security best practice. Default: disabled.",
            Tags = ["screensaver", "password", "lock", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
        },
        new TweakDef
        {
            Id = "ss-force-blank-screensaver",
            Label = "Set Blank Screen Screensaver",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            Description = "Sets the screensaver to a blank (black) screen. Lowest resource usage. Default: none.",
            Tags = ["screensaver", "blank", "black", "power"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "ss-disable-lock-screen-camera",
            Label = "Disable Lock Screen Camera",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            Description = "Disables the camera shortcut on the lock screen. Default: enabled.",
            Tags = ["lock", "camera", "privacy", "lock-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "ss-disable-lock-screen-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            Description = "Disables the Windows Spotlight/slideshow on the lock screen. Default: enabled.",
            Tags = ["lock", "slideshow", "spotlight", "lock-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "ss-set-monitor-timeout-10min",
            Label = "Set Monitor Power Off to 10 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            Description = "Sets the monitor to turn off after 10 minutes of inactivity. Saves energy. Default: 15 minutes.",
            Tags = ["monitor", "power", "timeout", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\PowerCfg\PowerPolicies\0"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff", "600")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff", "600")],
        },
        new TweakDef
        {
            Id = "ss-enable-lock-workstation",
            Label = "Enable Lock Workstation (Ctrl+Alt+Del)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures the Lock Workstation option is available on Ctrl+Alt+Del. Default: enabled.",
            Tags = ["lock", "workstation", "security", "ctrl-alt-del"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation", 0),
            ],
        },
        new TweakDef
        {
            Id = "ss-disable-logon-screen-animation",
            Label = "Disable Logon Screen Animation",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            Description = "Disables the first-logon animation shown after Windows installation or major updates. Default: enabled.",
            Tags = ["logon", "animation", "first-run", "lock-screen"],
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
    ];
}
