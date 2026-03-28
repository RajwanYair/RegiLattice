// RegiLattice.Core — Tweaks/FocusAssistPolicy.cs
// Focus Assist (Quiet Hours) Group Policy — Sprint 422.
// Controls Focus Assist / Do Not Disturb mode enforcement, notification
// suppression, and automatic rule behavior via Group Policy registry paths.
// Category: "Focus Assist Policy" | Slug: fa
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\QuietHours

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FocusAssistPolicy
{
    private const string QhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QuietHours";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "fa-disable-quiet-hours",
                Label = "Disable Focus Assist via Policy",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowQuietHours=0 to disable Focus Assist (Quiet Hours) via Group Policy. Prevents users from activating Focus Assist manually or via automatic rules. All notifications are always delivered.",
                Tags = ["focus assist", "quiet hours", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Focus Assist via GPO; all notifications always visible.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-disable-automatic-rules",
                Label = "Disable Focus Assist Automatic Rules",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowScheduledQuietHours=0 to prevent Focus Assist from activating automatically based on time-of-day rules, duplicate display, or gaming/fullscreen detection.",
                Tags = ["focus assist", "automatic rules", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents automatic Focus Assist activation; manual toggle still visible unless PolicyAllowQuietHours=0.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowScheduledQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowScheduledQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowScheduledQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-disable-game-mode-dnd",
                Label = "Disable Focus Assist in Game Mode",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowGameModeQuietHours=0 to prevent Windows from automatically enabling Focus Assist when a game is detected as running in fullscreen. Ensures notifications are visible during gaming sessions.",
                Tags = ["focus assist", "game mode", "gaming", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables auto-DND during gaming; all notifications visible even in fullscreen games.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowGameModeQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowGameModeQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowGameModeQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-disable-presentation-dnd",
                Label = "Disable Focus Assist When Presenting",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowPresentationModeQuietHours=0 to prevent Windows from automatically activating Focus Assist when a duplicate display (projector/second monitor) is detected. Prevents accidental notification suppression in meeting rooms.",
                Tags = ["focus assist", "presentation", "display", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables presentation-mode Focus Assist; important for kiosk or shared display environments.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowPresentationModeQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowPresentationModeQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowPresentationModeQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-disable-summary-notification",
                Label = "Disable Focus Assist Summary Notification",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowSummaryNotification=0 to suppress the 'You missed N notifications while Focus Assist was on' toast that appears after a Focus Assist session ends. Reduces notification clutter on shared machines.",
                Tags = ["focus assist", "summary", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the post-Focus-Assist summary notification toast.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowSummaryNotification", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowSummaryNotification")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowSummaryNotification", 0)],
            },
            new TweakDef
            {
                Id = "fa-disable-fullscreen-dnd",
                Label = "Disable Focus Assist in Fullscreen Apps",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowFullScreenModeQuietHours=0 to prevent Windows from automatically activating Focus Assist when any application is running in fullscreen mode. Ensures notifications are delivered in all display states.",
                Tags = ["focus assist", "fullscreen", "apps", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents auto-DND when any app goes fullscreen; notifications remain visible.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowFullScreenModeQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowFullScreenModeQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowFullScreenModeQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-lock-priority-list",
                Label = "Lock Focus Assist Priority List",
                Category = "Focus Assist Policy",
                Description =
                    "Sets LockPriorityList=1 to prevent users from modifying the Focus Assist priority list that determines which apps and contacts can break through Focus Assist. Enforces a consistent notification priority on managed devices.",
                Tags = ["focus assist", "priority", "policy", "lock"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents end-users from adding personal priority contacts/apps; affects notification reach.",
                ApplyOps = [RegOp.SetDword(QhKey, "LockPriorityList", 1)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "LockPriorityList")],
                DetectOps = [RegOp.CheckDword(QhKey, "LockPriorityList", 1)],
            },
            new TweakDef
            {
                Id = "fa-disable-out-of-hours-rule",
                Label = "Disable Focus Assist Outside Working Hours Rule",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowOutOfHoursQuietHours=0 to prevent Focus Assist from automatically activating during hours outside those defined as 'working hours' in the Windows calendar. Ensures uniform notification delivery.",
                Tags = ["focus assist", "working hours", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables outside-working-hours automatic Focus Assist rule.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowOutOfHoursQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowOutOfHoursQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowOutOfHoursQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-disable-first-hour-rule",
                Label = "Disable Focus Assist First Hour After Resume Rule",
                Category = "Focus Assist Policy",
                Description =
                    "Sets AllowFirstHourQuietHours=0 to prevent Windows from enabling Focus Assist for the first hour after the device resumes from sleep or hibernation. Ensures immediate notification delivery after wake.",
                Tags = ["focus assist", "resume", "sleep", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Notifications delivered immediately after device wakes; no first-hour suppression.",
                ApplyOps = [RegOp.SetDword(QhKey, "AllowFirstHourQuietHours", 0)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "AllowFirstHourQuietHours")],
                DetectOps = [RegOp.CheckDword(QhKey, "AllowFirstHourQuietHours", 0)],
            },
            new TweakDef
            {
                Id = "fa-force-priority-only-mode",
                Label = "Force Focus Assist Priority-Only Mode",
                Category = "Focus Assist Policy",
                Description =
                    "Sets DefaultProfile=1 to configure Focus Assist to only allow notifications from priority contacts and apps through when activated. Prevents the 'Alarms only' level and limits Focus Assist to the least invasive mode.",
                Tags = ["focus assist", "priority", "mode", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Forces priority-only mode when Focus Assist is active; some notifications still delivered.",
                ApplyOps = [RegOp.SetDword(QhKey, "DefaultProfile", 1)],
                RemoveOps = [RegOp.DeleteValue(QhKey, "DefaultProfile")],
                DetectOps = [RegOp.CheckDword(QhKey, "DefaultProfile", 1)],
            },
        ];
}
