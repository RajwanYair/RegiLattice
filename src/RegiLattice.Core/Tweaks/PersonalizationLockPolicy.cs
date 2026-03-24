// RegiLattice.Core — Tweaks/PersonalizationLockPolicy.cs
// Lock Screen and Desktop Personalization machine-scope GPO controls — Sprint 202.
// Restricts lock-screen images, wallpaper policies, and personalisation features.
// Category: "Personalization Lock Policy" | Slug: plock
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PersonalizationLockPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "plock-disable-lock-screen",
                Label = "Disable Interactive Lock Screen",
                Category = "Personalization Lock Policy",
                Description =
                    "Removes the interactive lock screen and bypasses Cortana, search, and media controls on the lock screen. Users must enter credentials immediately. Default: enabled. Recommended: 1 (disabled) in kiosk or high-security environments.",
                Tags = ["lock-screen", "personalization", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Lock screen removed; sign-in prompt shown immediately. Cortana and media controls on lock screen are unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "NoLockScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "NoLockScreen", 1)],
            },
            new TweakDef
            {
                Id = "plock-enforce-lock-screen-image",
                Label = "Enforce Corporate Lock Screen Image",
                Category = "Personalization Lock Policy",
                Description =
                    "Forces a specific corporate lock screen image path and prevents users from changing it. Ensures brand-consistent or security-warning lock screens. Default: not enforced. Recommended: set path in LockScreenImage value.",
                Tags = ["lock-screen", "image", "corporate", "branding", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "All users see the same lock screen image; individual customisation is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "LockScreenImageEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockScreenImageEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "LockScreenImageEnabled", 1)],
            },
            new TweakDef
            {
                Id = "plock-disable-lockscreen-app-ads",
                Label = "Disable Lock Screen App Spotlight Ads",
                Category = "Personalization Lock Policy",
                Description =
                    "Disables Windows Spotlight suggestions and app advertisements shown on the lock screen. Prevents Microsoft from delivering ads to the lock screen via cloud content. Default: enabled. Recommended: 1 (disabled).",
                Tags = ["lock-screen", "spotlight", "ads", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Lock screen shows static image only; no cloud-delivered spotlight images or app suggestions.",
                ApplyOps = [RegOp.SetDword(Key, "NoChangingLockScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoChangingLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "NoChangingLockScreen", 1)],
            },
            new TweakDef
            {
                Id = "plock-block-user-lock-screen-change",
                Label = "Block User From Changing Lock Screen",
                Category = "Personalization Lock Policy",
                Description =
                    "Prevents non-admin users from changing the lock screen image or slide show. Enforces IT-managed lock screen content. Default: not controlled. Recommended: 1 in managed environments.",
                Tags = ["lock-screen", "user-restriction", "personalization", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Users cannot change lock screen via Settings; the IT-set lock screen image persists.",
                ApplyOps = [RegOp.SetDword(Key, "PreventChangingLockScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventChangingLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "PreventChangingLockScreen", 1)],
            },
            new TweakDef
            {
                Id = "plock-disable-lock-screen-camera",
                Label = "Disable Camera on Lock Screen",
                Category = "Personalization Lock Policy",
                Description =
                    "Prevents the camera from being activated from the lock screen without unlocking. Closes the camera-access-without-authentication attack surface. Default: 0. Recommended: 1 (disabled).",
                Tags = ["lock-screen", "camera", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Camera cannot be accessed from lock screen; must unlock first.",
                ApplyOps = [RegOp.SetDword(Key, "AllowLockScreenCamera", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLockScreenCamera")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLockScreenCamera", 0)],
            },
            new TweakDef
            {
                Id = "plock-disable-lock-screen-toast",
                Label = "Disable Toast Notifications on Lock Screen",
                Category = "Personalization Lock Policy",
                Description =
                    "Prevents toast notifications from displaying on the lock screen, hiding message previews and alert content from unauthenticated view. Default: enabled. Recommended: 1 (disabled) for data protection.",
                Tags = ["lock-screen", "notifications", "toast", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Notification content not visible from lock screen; users must log in to see notifications.",
                ApplyOps = [RegOp.SetDword(Key, "AllowLockScreenToastNotifications", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLockScreenToastNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLockScreenToastNotifications", 0)],
            },
            new TweakDef
            {
                Id = "plock-set-auto-slideshow",
                Label = "Disable Lock Screen Slideshow",
                Category = "Personalization Lock Policy",
                Description =
                    "Disables the lock screen slideshow feature that cycles through user-selected photos. Enforces a static lock screen image and prevents unintended photo disclosure on unattended PCs. Default: enabled. Recommended: 1 (disabled).",
                Tags = ["lock-screen", "slideshow", "photos", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Lock screen does not rotate photos from the user's pictures library.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenSlideshow", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenSlideshow")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenSlideshow", 1)],
            },
            new TweakDef
            {
                Id = "plock-block-desktop-theme-change",
                Label = "Block Users From Changing Desktop Theme",
                Category = "Personalization Lock Policy",
                Description =
                    "Prevents standard users from applying custom desktop themes, wallpapers, or colour schemes. Enforces consistent corporate visual identity. Default: not controlled. Recommended: 1 in kiosk/call-centre environments.",
                Tags = ["desktop", "theme", "personalization", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Users cannot customise wallpaper or theme via Settings; admin-set theme is enforced.",
                ApplyOps = [RegOp.SetDword(Key, "NoChangingTheme", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoChangingTheme")],
                DetectOps = [RegOp.CheckDword(Key, "NoChangingTheme", 1)],
            },
            new TweakDef
            {
                Id = "plock-disable-color-change",
                Label = "Disable User Windows Accent Colour Change",
                Category = "Personalization Lock Policy",
                Description =
                    "Prevents users from changing the Windows accent colour used in title bars, taskbar, and UI highlights. Enforces brand-consistent UI colouring set by IT policy. Default: not controlled. Recommended: 1 in corporate environments.",
                Tags = ["accent-color", "personalization", "ui", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Accent colour picker in Settings is disabled; IT-defined colour is enforced.",
                ApplyOps = [RegOp.SetDword(Key, "NoColorChange", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoColorChange")],
                DetectOps = [RegOp.CheckDword(Key, "NoColorChange", 1)],
            },
            new TweakDef
            {
                Id = "plock-disable-transparency-effects",
                Label = "Disable Windows Transparency Effects via Policy",
                Category = "Personalization Lock Policy",
                Description =
                    "Disables the acrylic transparency effects in Windows title bars and taskbar via Group Policy. Reduces GPU compositing overhead on resource-constrained hardware. Default: enabled. Recommended: 1 (disabled) on VMs and thin clients.",
                Tags = ["transparency", "acrylic", "performance", "personalization", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows UI renders without blur/transparency; minor performance improvement on low-end hardware.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTransparencyEffects", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTransparencyEffects")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTransparencyEffects", 1)],
            },
        ];
}
