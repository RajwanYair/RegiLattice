// RegiLattice.Core — Tweaks/PushNotificationsPolicy.cs
// Windows Push Notification Service Group Policy controls — Sprint 427.
// Manages toast notifications, lock-screen alerts, app tile updates,
// and cloud-sourced notification delivery via Group Policy registry paths.
// Category: "Push Notifications Policy" | Slug: pnp
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\PushNotifications

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PushNotificationsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\PushNotifications";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "pnp-disable-toast-notifications",
                Label = "Disable All Toast Notifications via Policy",
                Category = "Push Notifications Policy",
                Description =
                    "Sets ToastEnabled=0 to block all toast (pop-up) notifications via Group Policy. "
                    + "No app toast alerts will appear on the desktop or in Action Center regardless of individual app notification settings. "
                    + "This policy-level control takes precedence over per-user and per-app notification preferences configured in Windows Settings.",
                Tags = ["notifications", "toast", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Silences all desktop toast alerts; users may miss time-critical application notifications.",
                ApplyOps = [RegOp.SetDword(Key, "ToastEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ToastEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ToastEnabled", 0)],
            },
            new TweakDef
            {
                Id = "pnp-disable-lockscreen-toasts",
                Label = "Disable Toast Notifications on Lock Screen",
                Category = "Push Notifications Policy",
                Description =
                    "Sets NoToastApplicationNotificationOnLockscreen=1 to prevent app toast notifications from appearing on the Windows lock screen. "
                    + "Protects notification content from physical shoulder-surfing on unattended machines in shared-space environments.",
                Tags = ["notifications", "lock-screen", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides notification content on lock screen; privacy improvement for shared-space devices.",
                ApplyOps = [RegOp.SetDword(Key, "NoToastApplicationNotificationOnLockscreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoToastApplicationNotificationOnLockscreen")],
                DetectOps = [RegOp.CheckDword(Key, "NoToastApplicationNotificationOnLockscreen", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-app-notifications",
                Label = "Disable App Notifications via Group Policy",
                Category = "Push Notifications Policy",
                Description =
                    "Sets NoToastApplicationNotification=1 to block all application-level toast notifications via Group Policy. "
                    + "This machine-wide policy prevents individual users from re-enabling per-app notifications in Windows Settings, "
                    + "ensuring consistent notification suppression across all user accounts on the machine.",
                Tags = ["notifications", "apps", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Enforces notification silence across all apps; overrides per-app user notification settings.",
                ApplyOps = [RegOp.SetDword(Key, "NoToastApplicationNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoToastApplicationNotification")],
                DetectOps = [RegOp.CheckDword(Key, "NoToastApplicationNotification", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-cloud-notifications",
                Label = "Disable Cloud-Sourced Notifications",
                Category = "Push Notifications Policy",
                Description =
                    "Sets NoCloudApplicationNotification=1 to prevent Windows from delivering notifications that originate from cloud services "
                    + "via Windows Push Notification Services (WNS). Reduces notification-related network traffic and prevents cloud-sourced "
                    + "promotional content from being delivered to the Action Center.",
                Tags = ["notifications", "cloud", "wns", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks cloud-pushed alerts; may suppress Microsoft 365 renewal nudges and Store update notifications.",
                ApplyOps = [RegOp.SetDword(Key, "NoCloudApplicationNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoCloudApplicationNotification")],
                DetectOps = [RegOp.CheckDword(Key, "NoCloudApplicationNotification", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-tile-notifications",
                Label = "Disable Live Tile Notifications",
                Category = "Push Notifications Policy",
                Description =
                    "Sets NoTileApplicationNotification=1 to prevent apps from updating live tile badges and content on the Start menu. "
                    + "Eliminates background polling by Start tile update engines, reduces network usage from tile refresh requests, "
                    + "and removes animated content from the Start menu.",
                Tags = ["notifications", "live-tiles", "start-menu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Freezes Start menu tile content; cosmetic change with minor background bandwidth savings.",
                ApplyOps = [RegOp.SetDword(Key, "NoTileApplicationNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoTileApplicationNotification")],
                DetectOps = [RegOp.CheckDword(Key, "NoTileApplicationNotification", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-notification-mirroring",
                Label = "Disable Notification Mirroring to Linked Devices",
                Category = "Push Notifications Policy",
                Description =
                    "Sets DisallowNotificationMirroring=1 to prevent Windows from forwarding notification content to linked devices "
                    + "via Bluetooth pairing or the Phone Link (Your Phone) application. Mirrored notifications bypass per-app "
                    + "notification settings on the receiving device and may expose sensitive notification content to paired hardware.",
                Tags = ["notifications", "mirroring", "bluetooth", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops notification mirroring to paired phones/devices; local notifications unaffected.",
                ApplyOps = [RegOp.SetDword(Key, "DisallowNotificationMirroring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowNotificationMirroring")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowNotificationMirroring", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-cloud-toast-notifications",
                Label = "Disable Cloud Toast Notification Delivery",
                Category = "Push Notifications Policy",
                Description =
                    "Sets DisallowCloudToastNotification=1 to block toast notifications delivered through the Windows Push Notification "
                    + "Services (WNS) cloud infrastructure. Prevents the WNS channel from delivering push content from app backends, "
                    + "closing the WNS delivery path independently of the local toast-enabled setting.",
                Tags = ["notifications", "wns", "cloud", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables WNS push channel; push-enabled apps lose real-time alert capability.",
                ApplyOps = [RegOp.SetDword(Key, "DisallowCloudToastNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowCloudToastNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowCloudToastNotification", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-badge-on-lockscreen",
                Label = "Disable App Badge Counters on Lock Screen",
                Category = "Push Notifications Policy",
                Description =
                    "Sets NoLockScreenApplicationBadge=1 to prevent app badge counters from appearing on the Windows lock screen. "
                    + "Badge numbers (unread email count, missed calls, calendar items) are hidden, reducing information leakage "
                    + "on unattended endpoints that may be visible to passers-by.",
                Tags = ["notifications", "badge", "lock-screen", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides app badge counters on lock screen; minor privacy improvement with no functional impact.",
                ApplyOps = [RegOp.SetDword(Key, "NoLockScreenApplicationBadge", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreenApplicationBadge")],
                DetectOps = [RegOp.CheckDword(Key, "NoLockScreenApplicationBadge", 1)],
            },
            new TweakDef
            {
                Id = "pnp-disable-user-notification-changes",
                Label = "Prevent User Changes to Notification Settings",
                Category = "Push Notifications Policy",
                Description =
                    "Sets DisallowUserChanges=1 to prevent end-users from modifying notification priorities and quiet-hours settings "
                    + "in the Windows Settings app. Policy-enforced notification settings cannot be overridden per-user when this "
                    + "value is set, ensuring consistent notification governance across all accounts on the endpoint.",
                Tags = ["notifications", "quiet-hours", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Locks the notification settings page; users cannot re-enable notifications suppressed by policy.",
                ApplyOps = [RegOp.SetDword(Key, "DisallowUserChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowUserChanges")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowUserChanges", 1)],
            },
            new TweakDef
            {
                Id = "pnp-restrict-push-notification-network",
                Label = "Block Push Notification Network Access",
                Category = "Push Notifications Policy",
                Description =
                    "Sets NoNetworkNotification=1 to prevent Windows from delivering network-status notifications (connection changes, "
                    + "captive portal prompts, VPN status). Reduces notification noise in environments with frequent network transitions "
                    + "and prevents the captive-portal pop-up behavior on corporate or hotel networks.",
                Tags = ["notifications", "network", "captive-portal", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Suppresses network-change toast alerts; captive portal sign-in prompts will not appear automatically.",
                ApplyOps = [RegOp.SetDword(Key, "NoNetworkNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoNetworkNotification")],
                DetectOps = [RegOp.CheckDword(Key, "NoNetworkNotification", 1)],
            },
        ];
}
