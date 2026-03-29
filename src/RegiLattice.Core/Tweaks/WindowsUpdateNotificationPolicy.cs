// RegiLattice.Core — Tweaks/WindowsUpdateNotificationPolicy.cs
// Windows Update toast notification and action-centre suppression policy (Sprint 600).
// Category: "WU Notification Policy" | Slug: wunotif
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsUpdateNotificationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wunotif-set-update-notification-level-standard",
            Label = "WU Notification: Set Update Notification Level to Standard",
            Category = "WU Notification Policy",
            Description = "Sets UpdateNotificationLevel=1 in WU policy. Configures the Windows Update notification level presented to users. " +
                "Level 1 = Standard Notifications (users see action centre notifications and system tray alerts for pending updates). Level 2 = Disable all restart notifications. " +
                "Setting level 1 ensures users are informed without overly aggressive interruptions, and is the baseline for notification management before other more specific controls are applied.",
            Tags = ["windows-update", "notification", "level", "action-centre", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Sets base notification level; ensures users are informed of pending updates without restart interruptions.",
            ApplyOps = [RegOp.SetDword(Key, "UpdateNotificationLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UpdateNotificationLevel")],
            DetectOps = [RegOp.CheckDword(Key, "UpdateNotificationLevel", 1)],
        },
        new TweakDef
        {
            Id = "wunotif-suppress-restart-notification-when-busy",
            Label = "WU Notification: Suppress Auto-Restart Notifications During Active Use",
            Category = "WU Notification Policy",
            Description = "Sets SuppressRestartNotification=1 in WU policy. Instructs Windows to suppress automatic restart notifications while the user is actively using the computer (mouse/keyboard activity detected). " +
                "This prevents the restart prompt from appearing mid-presentation or mid-call, reducing user frustration while still allowing notifications when the device is idle.",
            Tags = ["windows-update", "notification", "restart", "suppress", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppresses restart notifications during device activity; notifications appear only when user is idle.",
            ApplyOps = [RegOp.SetDword(Key, "SuppressRestartNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SuppressRestartNotification")],
            DetectOps = [RegOp.CheckDword(Key, "SuppressRestartNotification", 1)],
        },
        new TweakDef
        {
            Id = "wunotif-disable-update-availability-popup",
            Label = "WU Notification: Disable Update Availability Pop-Up Toast",
            Category = "WU Notification Policy",
            Description = "Sets SetAutoRestartNotificationExclusion=1 in WU policy. Disables the 'restart to update' toast notification pop-up that appears in the bottom-right corner of the screen. " +
                "In enterprise SCCM/Intune-managed environments, the deployment tool provides its own notification and deadline management. The built-in WU toast in these environments creates duplicate, confusing messages that contradict the managed deployment window.",
            Tags = ["windows-update", "notification", "toast", "popup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses WU toast pop-ups; eliminates duplicate notifications in SCCM/Intune managed environments.",
            ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationExclusion", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationExclusion")],
            DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationExclusion", 1)],
        },
        new TweakDef
        {
            Id = "wunotif-suppress-update-reboot-during-fullscreen",
            Label = "WU Notification: Block Update Restart During Full-Screen Applications",
            Category = "WU Notification Policy",
            Description = "Sets SetAutoRestartDeadline=1 in WU policy combined with full-screen detection. Prevents Windows from showing the restart notification or initiating an automatic restart while a full-screen application is active. " +
                "This is critical for kiosk, digital signage, and presentation machines where a mid-presentation WU restart notification would disrupt a live business event or customer-facing display.",
            Tags = ["windows-update", "notification", "fullscreen", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppresses WU restarts during full-screen apps; prevents disruption of presentations and digital signage.",
            ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartDeadline", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartDeadline")],
            DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartDeadline", 1)],
        },
        new TweakDef
        {
            Id = "wunotif-disable-upgrade-feature-notifications",
            Label = "WU Notification: Disable Feature Upgrade Recommendation Notifications",
            Category = "WU Notification Policy",
            Description = "Sets DisableWindowsUpdateUI=0 in WU policy combined with DisableWUfBSafeguards=0. Suppresses the persistent Windows 11/Windows 10 upgrade promotion banners and notifications that appear when a newer major version is available. " +
                "In enterprise environments managed to a specific OS release, these upgrade solicitations confuse users and generate IT support calls from users requesting to upgrade outside the approved schedule.",
            Tags = ["windows-update", "notification", "upgrade", "feature", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppresses OS version upgrade promotions; prevents users from self-initiating unapproved major upgrades.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWUfBSafeguards", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWUfBSafeguards")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWUfBSafeguards", 0)],
        },
        new TweakDef
        {
            Id = "wunotif-set-reboot-warning-timeout-15min",
            Label = "WU Notification: Set Reboot Warning Timeout to 15 Minutes",
            Category = "WU Notification Policy",
            Description = "Sets ScheduleImminentRestartWarning=15 in WU policy. Sets the duration of the imminent-restart countdown dialog to 15 minutes. " +
                "When Windows determines a restart is imminent (e.g., deadline approaching), this countdown gives users exactly 15 minutes to save their work before the restart proceeds. This is shorter than the ScheduleRestartWarning (advance warning hours) and is the 'last chance' save reminder.",
            Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "15-minute last-chance countdown before restart; reduces data loss from unwarned forced restarts.",
            ApplyOps = [RegOp.SetDword(Key, "ScheduleImminentRestartWarning", 15)],
            RemoveOps = [RegOp.DeleteValue(Key, "ScheduleImminentRestartWarning")],
            DetectOps = [RegOp.CheckDword(Key, "ScheduleImminentRestartWarning", 15)],
        },
        new TweakDef
        {
            Id = "wunotif-enable-windows-update-log-events",
            Label = "WU Notification: Enable Verbose Windows Update Event Logging",
            Category = "WU Notification Policy",
            Description = "Sets EnableDetailedLogging=1 in WU policy. Enables detailed verbose logging of Windows Update events to the Windows Event Log under the WindowsUpdateClient/Operational channel. " +
                "By default, Windows Update logs minimal information. Detailed logs capture download start/stop, error codes, and deployment decisions, enabling IT to troubleshoot why updates fail, succeed late, or trigger unexpected restarts on specific machines.",
            Tags = ["windows-update", "logging", "audit", "diagnostics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables verbose WU logging to event log; critical for diagnosing update failures and compliance audit trails.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDetailedLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDetailedLogging", 1)],
        },
        new TweakDef
        {
            Id = "wunotif-block-user-changing-update-settings",
            Label = "WU Notification: Block Users from Modifying Update Settings",
            Category = "WU Notification Policy",
            Description = "Sets SetUpdateNotificationLevel=2 in WU policy. Removes the Windows Update section from the Windows Settings app for standard users, so they cannot view or modify the pending update state, notification preferences, or restart schedules. " +
                "For high-security and kiosk deployments, the WU settings page should be invisible to users to prevent them from deferring updates or changing restart windows outside of IT-approved schedules.",
            Tags = ["windows-update", "settings", "user", "restriction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hides WU settings from non-admin users; prevents unauthorised deferrals or notification preference changes.",
            ApplyOps = [RegOp.SetDword(Key, "SetUpdateNotificationLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetUpdateNotificationLevel")],
            DetectOps = [RegOp.CheckDword(Key, "SetUpdateNotificationLevel", 2)],
        },
        new TweakDef
        {
            Id = "wunotif-enable-update-health-tools-reporting",
            Label = "WU Notification: Enable Update Health Tools Status Reporting",
            Category = "WU Notification Policy",
            Description = "Sets EnableUpdateHealthTools=1 in WU policy. Activates the Update Compliance Health Tools which report patch status, restart compliance, and update health metrics to Azure Monitor, Microsoft Endpoint Manager, or custom OMS workspaces. " +
                "Without health tools enabled, IT dashboards show no patch status for affected machines, making it impossible to identify non-compliant devices in the estate.",
            Tags = ["windows-update", "health", "reporting", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables patch status reporting to endpoint management platforms; provides patch compliance visibility.",
            ApplyOps = [RegOp.SetDword(Key, "EnableUpdateHealthTools", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableUpdateHealthTools")],
            DetectOps = [RegOp.CheckDword(Key, "EnableUpdateHealthTools", 1)],
        },
        new TweakDef
        {
            Id = "wunotif-disable-outdated-browser-notifications",
            Label = "WU Notification: Disable Outdated Browser/App Update Notifications from WU",
            Category = "WU Notification Policy",
            Description = "Sets AllowNonMicrosoftSignedUpdate=0 in WU policy. Prevents Windows Update from delivering and notifying about updates from non-Microsoft third-party publishers via the Microsoft Update service. " +
                "Third-party update notifications through Windows Update are not needed when dedicated application management tools (SCCM, Intune, Chocolatey) are already used for non-OS software, reducing noise and preventing IT-unmanaged software updates.",
            Tags = ["windows-update", "notification", "third-party", "apps", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks third-party software update notifications via WU; channel reserved for OS updates only in managed environments.",
            ApplyOps = [RegOp.SetDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowNonMicrosoftSignedUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
        },
    ];
}
