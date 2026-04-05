namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from Screensaver.cs ──────────────────────────────────────────────────

internal static class Screensaver
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ss-disable-screensaver",
            Label = "Disable Screensaver",
            Category = "User Account",
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
            Category = "User Account",
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
            Id = "ss-require-password-on-resume",
            Label = "Require Password on Screensaver Resume",
            Category = "User Account",
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
            Id = "ss-lock-screen-timeout-60",
            Label = "Set Lock Screen Timeout to 60 Seconds",
            Category = "User Account",
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
            Id = "ss-set-monitor-timeout-10min",
            Label = "Set Monitor Power Off to 10 Minutes",
            Category = "User Account",
            NeedsAdmin = false,
            Description = "Sets the monitor to turn off after 10 minutes of inactivity. Saves energy. Default: 15 minutes.",
            Tags = ["monitor", "power", "timeout", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\PowerCfg\PowerPolicies\0"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff", "600")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff", "600")],
        },
    ];
}
