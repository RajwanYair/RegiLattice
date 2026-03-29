// RegiLattice.Core — Tweaks/UpdateAutoRestartPolicy.cs
// Windows Update engaged-restart deadline and grace-period enforcement (Sprint 597).
// Category: "Update Auto-Restart Policy" | Slug: wuarstrt
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UpdateAutoRestartPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wuarstrt-set-engaged-restart-deadline-7days",
            Label = "WU Auto-Restart: Set Engaged Restart Deadline to 7 Days",
            Category = "Update Auto-Restart Policy",
            Description = "Sets EngagedRestartDeadline=7 in WU policy. After a quality update is downloaded, Windows enters 'engaged restart' mode where users are repeatedly notified. " +
                "This value sets the absolute deadline after which Windows will force a restart regardless of user activity. " +
                "7 days is a balance that gives users a full work week to schedule the restart while ensuring machines don't stay un-patched indefinitely.",
            Tags = ["windows-update", "restart", "deadline", "policy", "engaged"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Forces restart after 7 days; ensures machines are patched while giving users a workweek to choose their own restart time.",
            ApplyOps = [RegOp.SetDword(Key, "EngagedRestartDeadline", 7)],
            RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartDeadline")],
            DetectOps = [RegOp.CheckDword(Key, "EngagedRestartDeadline", 7)],
        },
        new TweakDef
        {
            Id = "wuarstrt-set-engaged-restart-snooze-3days",
            Label = "WU Auto-Restart: Set Engaged Restart Snooze Interval to 3 Days",
            Category = "Update Auto-Restart Policy",
            Description = "Sets EngagedRestartSnoozeSchedule=3 in WU policy. Controls how frequently Windows re-displays the engaged restart notification after a user dismisses it. " +
                "Value of 3 means the reminder returns every 3 days, ensuring users don't forget a pending restart while avoiding daily interruptions that lead to notification fatigue and dismissal without action.",
            Tags = ["windows-update", "restart", "snooze", "notification", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "3-day snooze interval for restart reminders; balances user awareness with notification fatigue.",
            ApplyOps = [RegOp.SetDword(Key, "EngagedRestartSnoozeSchedule", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartSnoozeSchedule")],
            DetectOps = [RegOp.CheckDword(Key, "EngagedRestartSnoozeSchedule", 3)],
        },
        new TweakDef
        {
            Id = "wuarstrt-set-engaged-restart-transition-2days",
            Label = "WU Auto-Restart: Set Engaged Restart Transition Schedule to 2 Days",
            Category = "Update Auto-Restart Policy",
            Description = "Sets EngagedRestartTransitionSchedule=2 in WU policy. Controls how many days after an update becomes ready-to-install that Windows transitions from passive notifications to the more prominent 'engaged restart' mode. " +
                "Setting this to 2 days means the first two days show soft notifications, after which the full engaged restart UI (with deadline counter) takes over.",
            Tags = ["windows-update", "restart", "transition", "policy", "engaged"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Transitions to engaged restart mode after 2 days; earlier transition increases restart compliance rate.",
            ApplyOps = [RegOp.SetDword(Key, "EngagedRestartTransitionSchedule", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartTransitionSchedule")],
            DetectOps = [RegOp.CheckDword(Key, "EngagedRestartTransitionSchedule", 2)],
        },
        new TweakDef
        {
            Id = "wuarstrt-set-quality-update-deadline-3days",
            Label = "WU Auto-Restart: Set Quality Update Install Deadline to 3 Days",
            Category = "Update Auto-Restart Policy",
            Description = "Sets ConfigureDeadlineForQualityUpdates=3 in WU policy. Establishes a hard deadline of 3 days from when a quality (security + non-security) update is offered before Windows must restart to install it. " +
                "For security teams managing patch compliance under CIS or NIST 800-53 patch SLAs, a 3-day restart deadline for quality updates ensures critical CVE patches are active within the compliance window.",
            Tags = ["windows-update", "deadline", "quality", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "3-day hard restart deadline for quality updates; supports NIST 800-53 and CIS patch compliance SLAs.",
            ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForQualityUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
        },
        new TweakDef
        {
            Id = "wuarstrt-set-feature-update-deadline-14days",
            Label = "WU Auto-Restart: Set Feature Update Install Deadline to 14 Days",
            Category = "Update Auto-Restart Policy",
            Description = "Sets ConfigureDeadlineForFeatureUpdates=14 in WU policy. Establishes a 14-day hard deadline from when a feature update is offered before Windows must restart to complete installation. " +
                "Feature updates are far more disruptive than quality updates (longer restart time, possible app compatibility breaks), so a longer 14-day window gives users and IT departments time to validate and prepare.",
            Tags = ["windows-update", "deadline", "feature", "upgrade", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "14-day deadline for feature updates; longer window accommodates compatibility validation before forced restart.",
            ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForFeatureUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
        },
        new TweakDef
        {
            Id = "wuarstrt-set-deadline-grace-period-2days",
            Label = "WU Auto-Restart: Set Post-Deadline Grace Period to 2 Days",
            Category = "Update Auto-Restart Policy",
            Description = "Sets ConfigureDeadlineGracePeriod=2 in WU policy. After the restart deadline passes, this grace period gives users an additional 2 days before the machine will restart outside of active hours. " +
                "The grace period prevents the deadline enforcement from causing a disruptive forced restart mid-workday as soon as the deadline hits. The machine will restart during the next scheduled non-active hours window within the grace period.",
            Tags = ["windows-update", "deadline", "grace", "restart", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "2-day grace period post-deadline; restart deferred to next active-hours window reducing in-day disruption.",
            ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineGracePeriod", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineGracePeriod")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineGracePeriod", 2)],
        },
        new TweakDef
        {
            Id = "wuarstrt-disable-no-auto-reboot-after-deadline",
            Label = "WU Auto-Restart: Allow Auto-Reboot After Deadline Expires",
            Category = "Update Auto-Restart Policy",
            Description = "Sets ConfigureDeadlineNoAutoReboot=0 in WU policy. Ensures that once the deadline and grace period pass, Windows WILL automatically restart to apply the update. " +
                "Value=0 means no moratorium on auto-reboot after the deadline. This overrides any 'NoAutoRebootWithLoggedOnUsers' policy for machines that have exceeded their deadline, ensuring patching is never blocked indefinitely by a persistent logged-on session.",
            Tags = ["windows-update", "restart", "deadline", "enforcement", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Post-deadline auto-reboot enabled; overrides logged-on user protection once deadline expires for compliance.",
            ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineNoAutoReboot")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "wuarstrt-set-restart-warning-4hours",
            Label = "WU Auto-Restart: Set Pre-Restart Warning to 4 Hours",
            Category = "Update Auto-Restart Policy",
            Description = "Sets ScheduleRestartWarning=4 in WU policy. When Windows schedules an automatic restart, this setting controls how many hours in advance users receive a prominent restart warning notification. " +
                "A 4-hour advance warning gives users time to save work, close applications, and plan the restart, significantly reducing data loss from unexpected restarts.",
            Tags = ["windows-update", "restart", "warning", "notification", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "4-hour advance restart warning; gives users time to save work and plan restart timing.",
            ApplyOps = [RegOp.SetDword(Key, "ScheduleRestartWarning", 4)],
            RemoveOps = [RegOp.DeleteValue(Key, "ScheduleRestartWarning")],
            DetectOps = [RegOp.CheckDword(Key, "ScheduleRestartWarning", 4)],
        },
        new TweakDef
        {
            Id = "wuarstrt-enable-auto-restart-required-notification",
            Label = "WU Auto-Restart: Enable Mandatory Restart Required Notification",
            Category = "Update Auto-Restart Policy",
            Description = "Sets SetAutoRestartRequiredNotificationDismissal=1 in WU policy. Configures Windows to show a non-dismissable restart required notification when a patch deadline is imminent. " +
                "Without this, users can indefinitely dismiss restart prompts. With value=1, close-to-deadline notifications must be acknowledged with a concrete restart time selection rather than a simple dismiss.",
            Tags = ["windows-update", "restart", "notification", "mandatory", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Non-dismissable restart notification near deadline; forces users to choose restart time, increasing compliance.",
            ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartRequiredNotificationDismissal")],
            DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
        },
        new TweakDef
        {
            Id = "wuarstrt-enable-auto-restart-notification-config",
            Label = "WU Auto-Restart: Enable Automatic Restart Notification Banner",
            Category = "Update Auto-Restart Policy",
            Description = "Sets SetAutoRestartNotificationConfig=1 in WU policy. Enables the automatic restart notification configuration, which shows a system tray and action centre banner when a pending restart is required. " +
                "Without this setting the notification may be suppressed in locked-down enterprise notification policies. Enabling it ensures users are always informed of pending update restarts even in notification-restricted environments.",
            Tags = ["windows-update", "restart", "notification", "banner", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables restart notification banner in action centre; ensures user visibility of pending restarts in locked environments.",
            ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationConfig")],
            DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationConfig", 1)],
        },
    ];
}
