namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PersonalizationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
    private const string SysPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prsnlz-disable-lock-screen",
            Label = "Disable Lock Screen (Skip to Login)",
            Category = "Personalization Policy",
            Description =
                "Removes the lock screen entirely, jumping directly to the login prompt on wake or startup. Speeds up access but reduces the secure idle display.",
            Tags = ["lock-screen", "personalization", "policy", "login"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Removes the lock screen; slightly reduces secure idle display posture.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreen")],
            DetectOps = [RegOp.CheckDword(Key, "NoLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-disable-lock-screen-camera",
            Label = "Disable Camera on Lock Screen",
            Category = "Personalization Policy",
            Description = "Removes the camera shortcut from the lock screen, preventing photo or video capture without signing in.",
            Tags = ["lock-screen", "camera", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents unauthenticated camera access from the lock screen.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(Key, "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-disable-lock-screen-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Personalization Policy",
            Description =
                "Disables the lock screen photo slideshow feature, preventing images from a configured source from rotating on the lock screen.",
            Tags = ["lock-screen", "slideshow", "personalization", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables lock screen image rotation.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(Key, "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-disable-lock-screen-overlays",
            Label = "Disable Lock Screen App Notification Overlays",
            Category = "Personalization Policy",
            Description =
                "Removes application notification badges (email count, calendar events, alarms) from the lock screen. Reduces information leakage to unauthenticated observers.",
            Tags = ["lock-screen", "notifications", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents notification badges from leaking information to unauthenticated observers.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LockScreenOverlaysDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LockScreenOverlaysDisabled")],
            DetectOps = [RegOp.CheckDword(Key, "LockScreenOverlaysDisabled", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-force-default-lock-screen",
            Label = "Force Default Lock Screen Image",
            Category = "Personalization Policy",
            Description =
                "Prevents users from customising the lock screen image. Forces the Windows default lock screen, blocking user-selected photos or Windows Spotlight images.",
            Tags = ["lock-screen", "personalization", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Enforces a uniform lock screen image organisation-wide.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceDefaultLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceDefaultLockScreen")],
            DetectOps = [RegOp.CheckDword(Key, "ForceDefaultLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-prevent-wallpaper-change",
            Label = "Prevent Desktop Wallpaper Changes",
            Category = "Personalization Policy",
            Description =
                "Prevents users from changing the desktop wallpaper via Settings or Control Panel. Enforces a consistent corporate desktop appearance.",
            Tags = ["wallpaper", "personalization", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Locks desktop wallpaper to a corporate standard.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoDesktopBackground", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoDesktopBackground")],
            DetectOps = [RegOp.CheckDword(Key, "NoDesktopBackground", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-hide-background-settings",
            Label = "Hide Background/Wallpaper Settings Page",
            Category = "Personalization Policy",
            Description =
                "Removes the background/wallpaper tab from Display Properties in Control Panel, preventing users from accessing wallpaper settings.",
            Tags = ["wallpaper", "control-panel", "policy", "personalization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides the wallpaper tab from Control Panel Display Properties.",
            RegistryKeys = [SysPolicy],
            ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispBackgroundPage", 1)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispBackgroundPage")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispBackgroundPage", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-hide-screensaver-settings",
            Label = "Hide Screensaver Settings Page",
            Category = "Personalization Policy",
            Description =
                "Removes the screensaver tab from Display Properties in Control Panel, preventing users from changing screensaver settings.",
            Tags = ["screensaver", "control-panel", "policy", "personalization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides the screensaver tab from Control Panel Display Properties.",
            RegistryKeys = [SysPolicy],
            ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispScrSavPage", 1)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispScrSavPage")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispScrSavPage", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-hide-appearance-settings",
            Label = "Hide Appearance Settings Page",
            Category = "Personalization Policy",
            Description =
                "Removes the Appearance tab from Display Properties in Control Panel, preventing users from changing colour scheme and system visual style.",
            Tags = ["appearance", "control-panel", "policy", "personalization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides the Appearance tab from Control Panel Display Properties.",
            RegistryKeys = [SysPolicy],
            ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispAppearancePage", 1)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispAppearancePage")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispAppearancePage", 1)],
        },
        new TweakDef
        {
            Id = "prsnlz-prevent-color-change",
            Label = "Prevent System Colour Scheme Changes",
            Category = "Personalization Policy",
            Description =
                "Blocks users from changing the Windows colour scheme (accent colours, dark/light theme selection) via Settings or Control Panel.",
            Tags = ["theme", "colors", "control-panel", "policy", "personalization"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Locks the Windows colour scheme to the admin-set value.",
            RegistryKeys = [SysPolicy],
            ApplyOps = [RegOp.SetDword(SysPolicy, "NoColorChoice", 1)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoColorChoice")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "NoColorChoice", 1)],
        },
    ];
}
