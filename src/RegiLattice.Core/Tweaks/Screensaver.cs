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
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1"),
            ],
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
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900"),
            ],
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
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
        },
        new TweakDef
        {
            Id = "ss-require-password",
            Label = "Require Password After Screensaver",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Require login password when resuming from screensaver. Default: not required. Recommended: enabled.",
            Tags = ["screensaver", "password", "lock", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
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
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", "C:\\Windows\\System32\\scrnsave.scr"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE"),
            ],
        },
        new TweakDef
        {
            Id = "ss-force-policy",
            Label = "Force Screensaver (Policy, 10 min)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Force screensaver with 10-min timeout and password via machine policy. Default: not set.",
            Tags = ["screensaver", "policy", "force", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
        },
        new TweakDef
        {
            Id = "ss-disable-user-policy",
            Label = "Disable Screensaver (User Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable screensaver via user-level policy key. Default: not set.",
            Tags = ["screensaver", "policy", "user", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Control Panel\Desktop"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "ss-prevent-screensaver-change",
            Label = "Prevent Screensaver Change",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevent users from changing screensaver settings. Default: allowed.",
            Tags = ["screensaver", "policy", "restrict", "kiosk"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-15min",
            Label = "Set Screensaver Timeout to 15 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the screensaver activation timeout to 15 minutes (900 seconds). Default: 600. Recommended: 900 for balanced security and convenience.",
            Tags = ["screensaver", "timeout", "15min", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-30min",
            Label = "Set Screensaver Timeout to 30 Minutes",
            Category = "Screensaver & Lock",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the screensaver activation timeout to 30 minutes (1800 seconds). Default: 600. Recommended: 1800 for extended-focus workflows.",
            Tags = ["screensaver", "timeout", "30min", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
        },
        new TweakDef
        {
            Id = "ss-scr-timeout-10min",
            Label = "Set Screensaver Timeout to 10 Minutes (Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets screensaver timeout to 10 minutes via machine policy. Enforced across all users. Default: varies. Recommended: 600 seconds.",
            Tags = ["screensaver", "timeout", "10min", "policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
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
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
        },
        new TweakDef
        {
            Id = "ss-scr-password-on-resume",
            Label = "Require Password on Resume (Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires password entry when resuming from screensaver via policy. Enforces lock screen security. Default: varies. Recommended: enabled.",
            Tags = ["screensaver", "password", "resume", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
        },
        new TweakDef
        {
            Id = "ss-scr-disable-screensaver",
            Label = "Disable Screensaver Completely (Policy)",
            Category = "Screensaver & Lock",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the screensaver completely via machine policy. Prevents screensaver from activating on any user. Default: Enabled. Recommended: Disabled only for kiosks.",
            Tags = ["screensaver", "disable", "policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
    ];
}
