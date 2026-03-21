// RegiLattice.Core — Tweaks/KioskSharedPc.cs
// Windows Kiosk mode and Shared PC configuration tweaks.
// Slug: "kiosk" — targets unattended / public / classroom deployments.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KioskSharedPc
{
    private const string SharedPc = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SharedPC";
    private const string WinSysPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string LockPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
    private const string LogonPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kiosk-enable-shared-pc-mode",
            Label = "Enable Shared PC Mode",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableSharedPCMode = 1 in the SharedPC registry key. Activates Windows Shared PC mode, "
                + "which auto-manages accounts, disk, and sign-in for multi-user shared devices such as school "
                + "or library computers. Default: 0 (disabled).",
            Tags = ["kiosk", "shared-pc", "education", "public", "multi-user"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "EnableSharedPCMode", 1)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "EnableSharedPCMode")],
            DetectOps = [RegOp.CheckDword(SharedPc, "EnableSharedPCMode", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-account-model-guest",
            Label = "Use Guest-Only Account Model for Shared PC",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AccountModel = 0 (guest only) in the SharedPC key. In guest mode, users sign in with "
                + "a temporary guest account that is deleted on sign-out, ensuring no profile data persists. Default: 0.",
            Tags = ["kiosk", "shared-pc", "guest", "account", "privacy"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "AccountModel", 0)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "AccountModel")],
            DetectOps = [RegOp.CheckDword(SharedPc, "AccountModel", 0)],
        },
        new TweakDef
        {
            Id = "kiosk-delete-on-signout",
            Label = "Delete Guest Profiles on Sign-Out",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DeletionPolicy = 1 (delete immediately on sign-out) in the SharedPC key. "
                + "Guest profiles are removed as soon as the user signs out, keeping disk clear on shared devices.",
            Tags = ["kiosk", "shared-pc", "profile", "cleanup", "privacy"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "DeletionPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "DeletionPolicy")],
            DetectOps = [RegOp.CheckDword(SharedPc, "DeletionPolicy", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disk-level-deletion-25",
            Label = "Auto-Delete Profiles at 25% Free Disk",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DiskLevelDeletion = 25 in the SharedPC key. When free disk falls below 25% of total disk, "
                + "SharedPC policy begins deleting the oldest cached accounts, reclaiming space automatically.",
            Tags = ["kiosk", "shared-pc", "disk", "cleanup", "automatic"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "DiskLevelDeletion", 25)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "DiskLevelDeletion")],
            DetectOps = [RegOp.CheckDword(SharedPc, "DiskLevelDeletion", 25)],
        },
        new TweakDef
        {
            Id = "kiosk-disk-level-caching-50",
            Label = "Stop Caching New Profiles at 50% Free Disk",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DiskLevelCaching = 50 in the SharedPC key. When free disk drops below 50%, Shared PC mode "
                + "stops caching new user profiles to prevent the drive from filling up.",
            Tags = ["kiosk", "shared-pc", "disk", "caching", "profile"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "DiskLevelCaching", 50)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "DiskLevelCaching")],
            DetectOps = [RegOp.CheckDword(SharedPc, "DiskLevelCaching", 50)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HideFastUserSwitching = 1 via Windows System policy. Removes the user account switcher "
                + "button from the lock screen and Start menu. Useful for kiosk or single-user session scenarios. Default: 0.",
            Tags = ["kiosk", "shared-pc", "user-switching", "lock-screen", "policy"],
            RegistryKeys = [WinSysPol],
            ApplyOps = [RegOp.SetDword(WinSysPol, "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSysPol, "HideFastUserSwitching")],
            DetectOps = [RegOp.CheckDword(WinSysPol, "HideFastUserSwitching", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-no-local-password-reset",
            Label = "Block Local Password Reset from Lock Screen",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePasswordReveal = 1 via Windows System policy. Prevents users on lock screen "
                + "from clicking 'Forgot my PIN / password' to initiate a self-service reset, important for "
                + "kiosk machines using fixed managed accounts.",
            Tags = ["kiosk", "password", "lock-screen", "reset", "policy"],
            RegistryKeys = [WinSysPol],
            ApplyOps = [RegOp.SetDword(WinSysPol, "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSysPol, "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(WinSysPol, "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-enable-edu-policies",
            Label = "Apply Education / Shared PC Baseline Policies",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SetEduPolicies = 1 in the SharedPC key. Enables the full set of Education-mode policies: "
                + "Start menu simplification, sign-in type restriction, and content filtering baselines "
                + "recommended for classroom and lab deployments.",
            Tags = ["kiosk", "shared-pc", "education", "policy", "classroom"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "SetEduPolicies", 1)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "SetEduPolicies")],
            DetectOps = [RegOp.CheckDword(SharedPc, "SetEduPolicies", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-lock-screen-camera",
            Label = "Disable Camera Access on Lock Screen",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLockScreenCamera = 1 via the Personalization policy key. Prevents apps and the system "
                + "from activating the camera while the device is locked. Reduces physical surveillance risk "
                + "in public kiosk settings. Default: 0 (camera may be used on lock screen).",
            Tags = ["kiosk", "camera", "lock-screen", "privacy", "policy"],
            RegistryKeys = [LockPol],
            ApplyOps = [RegOp.SetDword(LockPol, "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(LockPol, "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(LockPol, "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-lock-screen-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Kiosk & Shared PC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLockScreenSlideshow = 1 via the Personalization policy key. Stops the lock screen "
                + "from cycling through user photos or Spotlight images, ensuring a static and controlled "
                + "appearance on kiosk or shared devices. Default: 0.",
            Tags = ["kiosk", "lock-screen", "slideshow", "appearance", "policy"],
            RegistryKeys = [LockPol],
            ApplyOps = [RegOp.SetDword(LockPol, "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(LockPol, "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(LockPol, "NoLockScreenSlideshow", 1)],
        },
    ];
}
