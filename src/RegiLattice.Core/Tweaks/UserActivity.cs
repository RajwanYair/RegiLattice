namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from UserActivity.cs ──────────────────────────────────────────────────

internal static class UserActivity
{
    private const string ActivityPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    private const string TimelinePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    private const string Privacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy";

    private const string DiagTrackPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "activity-disable-storage",
            Label = "Disable Local Activity History Storage",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["activity", "storage", "timeline", "local", "privacy"],
            Description =
                "Prevents Windows from storing user activity history on this device. "
                + "AllowStoringUserActivities=0. Completely disables Timeline and the "
                + "local activity database.",
            ApplyOps = [RegOp.SetDword(ActivityPolicy, "AllowStoringUserActivities", 0)],
            RemoveOps = [RegOp.SetDword(ActivityPolicy, "AllowStoringUserActivities", 1)],
            DetectOps = [RegOp.CheckDword(ActivityPolicy, "AllowStoringUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-jump-lists",
            Label = "Disable Jump Lists in Taskbar and Start",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "jump lists", "taskbar", "privacy"],
            Description =
                "Prevents Windows from showing Jump Lists (recently used files and tasks) "
                + "when right-clicking taskbar icons or Start tiles. "
                + "NoStartMenuMFUprogramsList=1.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsMenu")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsMenu", 1)],
        },
    ];
}
