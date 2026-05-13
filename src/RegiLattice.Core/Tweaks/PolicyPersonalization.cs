namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyPersonalization
{
    private const string PersonKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "person-disable-lock-screen-image",
            Label = "Prevent Custom Lock Screen Image",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoChangingLockScreen=1 in Personalization policy. Prevents users from changing "
                + "the lock screen background image, enforcing the corporate or default lock screen. "
                + "Useful for kiosk or shared workstation deployments.",
            Tags = ["lock-screen", "personalization", "policy", "kiosk"],
            RegistryKeys = [PersonKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Lock screen image cannot be changed by users; admin default enforced.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoChangingLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoChangingLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoChangingLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-lock-screen-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLockScreenSlideshow=1 in Personalization policy. Prevents the photo slideshow "
                + "from running on the lock screen, reducing GPU wake-ups and power consumption on "
                + "laptops and tablets while the device is locked.",
            Tags = ["lock-screen", "slideshow", "policy", "power"],
            RegistryKeys = [PersonKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Lock screen shows a static image only; no slideshow transitions.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-lock-screen-camera",
            Label = "Disable Lock Screen Camera Access",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLockScreenCamera=1 in Personalization policy. Prevents the camera from being "
                + "activated on the lock screen (e.g., for Windows Hello facial recognition). Useful "
                + "in environments where camera use on locked devices is a privacy concern.",
            Tags = ["lock-screen", "camera", "privacy", "policy"],
            RegistryKeys = [PersonKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Camera disabled on lock screen; Windows Hello face recognition unavailable at lock.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "person-force-default-lock-image",
            Label = "Force Default Lock Screen Image",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceDefaultLockScreen=1 in Personalization policy. Forces the system to display "
                + "only the default Windows lock screen image, overriding any user-configured or "
                + "Spotlight-provided images.",
            Tags = ["lock-screen", "default", "policy", "branding"],
            RegistryKeys = [PersonKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Default Windows lock screen image enforced; Spotlight and user images blocked.",
            ApplyOps = [RegOp.SetDword(PersonKey, "ForceDefaultLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "ForceDefaultLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonKey, "ForceDefaultLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-themes",
            Label = "Prevent Theme Changes",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoThemesTab=1 in Personalization policy. Removes the Themes tab from the "
                + "Personalization settings page, preventing users from changing system themes, colours, "
                + "sounds, and cursors. Enforces a consistent desktop appearance on managed machines.",
            Tags = ["themes", "personalization", "policy", "lockdown"],
            RegistryKeys = [PersonKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users cannot change themes; consistent appearance across managed desktops.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoThemesTab", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoThemesTab")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoThemesTab", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-color-change",
            Label = "Prevent Accent Colour Changes",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoColorChoice=1 in Personalization policy. Prevents users from changing the "
                + "Windows accent colour in Settings > Personalization > Colors. Maintains corporate "
                + "branding consistency across all managed devices.",
            Tags = ["colour", "accent", "personalization", "policy", "branding"],
            RegistryKeys = [PersonKey],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Accent colour locked to default or admin-configured value.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoColorChoice", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoColorChoice")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoColorChoice", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-desktop-wallpaper-change",
            Label = "Prevent Desktop Wallpaper Change",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoChangingWallpaper=1 in Personalization policy. Prevents users from changing "
                + "the desktop wallpaper. Enforces the admin-deployed wallpaper on all managed devices.",
            Tags = ["wallpaper", "desktop", "personalization", "policy", "lockdown"],
            RegistryKeys = [PersonKey],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Desktop wallpaper locked; users cannot modify background.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoChangingWallpaper", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoChangingWallpaper")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoChangingWallpaper", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-screen-saver-change",
            Label = "Prevent Screen Saver Changes",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoDispScrSavPage=1 in Personalization policy. Removes the screen saver page "
                + "from Personalization settings, preventing users from changing or disabling the "
                + "corporate screen saver configuration.",
            Tags = ["screensaver", "personalization", "policy", "lockdown"],
            RegistryKeys = [PersonKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Screen saver settings locked; admin configuration enforced.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoDispScrSavPage", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoDispScrSavPage")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoDispScrSavPage", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-sound-scheme-change",
            Label = "Prevent Sound Scheme Changes",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoSoundSchemeChange=1 in Personalization policy. Prevents users from changing "
                + "the system sound scheme (event sounds, startup sound, etc.). Enforces a quiet or "
                + "corporate sound profile on managed devices.",
            Tags = ["sound", "audio", "personalization", "policy"],
            RegistryKeys = [PersonKey],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "System sound scheme locked; users cannot modify event sounds.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoSoundSchemeChange", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoSoundSchemeChange")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoSoundSchemeChange", 1)],
        },
        new TweakDef
        {
            Id = "person-disable-start-background-change",
            Label = "Prevent Start Menu Background Change",
            Category = "Lock Screen — Personalization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoChangingStartMenuBackground=1 in Personalization policy. Prevents users "
                + "from changing the Start menu background or transparency settings. Maintains a "
                + "uniform Start menu appearance across managed devices.",
            Tags = ["start-menu", "background", "personalization", "policy"],
            RegistryKeys = [PersonKey],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Start menu background locked to default; no user customisation.",
            ApplyOps = [RegOp.SetDword(PersonKey, "NoChangingStartMenuBackground", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonKey, "NoChangingStartMenuBackground")],
            DetectOps = [RegOp.CheckDword(PersonKey, "NoChangingStartMenuBackground", 1)],
        },
    ];
}
