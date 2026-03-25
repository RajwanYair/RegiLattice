// RegiLattice.Core — Tweaks/UserProfilesPolicy.cs
// Sprint 262: User Profiles Group Policy (10 tweaks)
// Category: "User Profiles Policy" | Slug: uprof
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UserProfilesPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "uprof-disable-roaming-profiles",
            Label = "Disable Roaming User Profiles",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets LocalProfile=1 in the System policy key. Forces all domain users on this "
                + "machine to use local profiles rather than roaming profiles synced from a "
                + "network share. Reduces logon time and prevents stale roaming profile issues. "
                + "Default: roaming profiles enabled when configured by AD. Recommended: 1 for "
                + "machines that should always use local state.",
            Tags = ["user-profiles", "roaming", "domain", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LocalProfile", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LocalProfile")],
            DetectOps = [RegOp.CheckDword(Key, "LocalProfile", 1)],
        },
        new TweakDef
        {
            Id = "uprof-disable-slow-link-detection",
            Label = "Disable Slow Network Link Detection for User Profiles",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets SlowLinkDetect=0 in the System policy key. Prevents Windows from "
                + "detecting a slow network link and switching to local profile mode. Avoids "
                + "unexpected profile behaviour on VPN or high-latency connections. Default: 1 "
                + "(detection enabled). Recommended: 0 when local profiles are always preferred.",
            Tags = ["user-profiles", "slow-link", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SlowLinkDetect", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SlowLinkDetect")],
            DetectOps = [RegOp.CheckDword(Key, "SlowLinkDetect", 0)],
        },
        new TweakDef
        {
            Id = "uprof-delete-cached-copies",
            Label = "Delete Cached Copies of Roaming Profiles at Logoff",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets DeleteRoamingCache=1 in the System policy key. Removes the locally "
                + "cached copy of each roaming profile when the user logs off. Prevents profile "
                + "data accumulation on shared/kiosk machines and ensures each logon fetches a "
                + "fresh copy from the server. Default: 0. Recommended: 1 on multi-user machines.",
            Tags = ["user-profiles", "roaming", "cache", "policy", "cleanup"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DeleteRoamingCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DeleteRoamingCache")],
            DetectOps = [RegOp.CheckDword(Key, "DeleteRoamingCache", 1)],
        },
        new TweakDef
        {
            Id = "uprof-prevent-profile-size-limit",
            Label = "Disable User Profile Size Limit Warning Dialog",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets ProfileDlgTimeOut=0 in the System policy key. Sets the timeout for the "
                + "profile size warning dialog to zero, preventing it from appearing. Removes a "
                + "source of user interruption on managed machines where profile disk quotas are "
                + "enforced by other means. Default: 15 seconds. Recommended: 0 on managed desktops.",
            Tags = ["user-profiles", "dialog", "quota", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ProfileDlgTimeOut", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ProfileDlgTimeOut")],
            DetectOps = [RegOp.CheckDword(Key, "ProfileDlgTimeOut", 0)],
        },
        new TweakDef
        {
            Id = "uprof-wait-on-logoff",
            Label = "Wait for Remote Profile Upload at Logoff",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets WaitForNetwork=0 in the System policy key. Prevents Windows from waiting "
                + "for a network connection to upload a roaming profile on logoff. Speeds up "
                + "logoff on machines with intermittent or no network connectivity. Default: 1. "
                + "Recommended: 0 when roaming profiles are not used.",
            Tags = ["user-profiles", "logoff", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "WaitForNetwork", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "WaitForNetwork")],
            DetectOps = [RegOp.CheckDword(Key, "WaitForNetwork", 0)],
        },
        new TweakDef
        {
            Id = "uprof-disable-profile-error-notify",
            Label = "Disable User Profile Load Error Notification",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoProfileErrorNotification=1 in the System policy key. Suppresses the "
                + "desktop notification shown when a user profile fails to load and a temporary "
                + "profile is created instead. Prevents confusing pop-ups on kiosk machines where "
                + "temporary profiles are expected. Default: 0 (notification shown). "
                + "Recommended: 1 for kiosk/shared machines.",
            Tags = ["user-profiles", "error", "notification", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoProfileErrorNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoProfileErrorNotification")],
            DetectOps = [RegOp.CheckDword(Key, "NoProfileErrorNotification", 1)],
        },
        new TweakDef
        {
            Id = "uprof-disable-guest-logon",
            Label = "Disable Guest Account Profile Creation",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets NoGuestAccount=1 in the System policy key. Prevents the built-in Guest "
                + "account from creating a user profile on the machine. Closes an attack surface "
                + "where temporary guest sessions accumulate profile data or are used for lateral "
                + "movement. Default: 0. Recommended: 1 on domain-joined and managed devices.",
            Tags = ["user-profiles", "guest", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoGuestAccount", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoGuestAccount")],
            DetectOps = [RegOp.CheckDword(Key, "NoGuestAccount", 1)],
        },
        new TweakDef
        {
            Id = "uprof-apply-gpo-at-logon",
            Label = "Force Synchronous Group Policy Processing at Logon",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets EnableSlowLinkUI=0 in the System policy key. Disables the slow-link UI "
                + "that defers Group Policy processing to the background, ensuring all GPOs are "
                + "fully applied before the desktop appears. Guarantees policies are in effect "
                + "from the first moment of user access. Default: 1. Recommended: 0 on secure "
                + "managed machines.",
            Tags = ["user-profiles", "group-policy", "gpo", "logon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSlowLinkUI", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSlowLinkUI")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSlowLinkUI", 0)],
        },
        new TweakDef
        {
            Id = "uprof-disable-user-tracking",
            Label = "Disable User Profile Tracking for Shell Namespace",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoUserFolderRedirection=1 in the System policy key. Prevents the shell "
                + "from tracking redirected user folders in the namespace extension. Reduces "
                + "overhead from folder-tracking checks during shell operations. Default: 0. "
                + "Recommended: 1 on machines without folder redirection configured.",
            Tags = ["user-profiles", "tracking", "shell", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoUserFolderRedirection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoUserFolderRedirection")],
            DetectOps = [RegOp.CheckDword(Key, "NoUserFolderRedirection", 1)],
        },
        new TweakDef
        {
            Id = "uprof-limit-profile-size",
            Label = "Disable User Profile Disk Quota Enforcement",
            Category = "User Profiles Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets EnableProfileQuota=0 in the System policy key. Turns off the built-in "
                + "profile size quota that can force logoff or prevent profile sync when the "
                + "disk quota is exceeded. Avoids unexpected user disruption on machines where "
                + "storage is managed by other means (disk quotas, FSRM). Default: 1 (quota "
                + "enforcement active if configured). Recommended: 0 when not using profile quotas.",
            Tags = ["user-profiles", "quota", "disk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableProfileQuota", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableProfileQuota")],
            DetectOps = [RegOp.CheckDword(Key, "EnableProfileQuota", 0)],
        },
    ];
}
