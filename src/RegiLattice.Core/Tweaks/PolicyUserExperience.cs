namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyUserExperience
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "uxpol-disable-lock-screen-app-notifications",
            Label = "Disable Lock Screen App Notifications via Policy",
            Category = "System — Windows Reliability",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLockScreenAppNotifications=1 under the CloudContent Group Policy path. "
                + "Prevents app notification banners and badges from displaying on the lock screen. "
                + "Reduces information exposure on unattended machines.",
            Tags = ["lock-screen", "notifications", "policy", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clears notification banners from the lock screen display.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenAppNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenAppNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenAppNotifications", 1)],
        },
    ];
}
