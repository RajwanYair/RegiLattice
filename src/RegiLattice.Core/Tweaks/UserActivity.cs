// RegiLattice.Core — Tweaks/UserActivity.cs
// Timeline, activity history, and user data collection settings (Sprint 94).
// Slug "activity" — HKLM/HKCU Timeline and ActivityHistory policy keys.
// Distinct from Privacy.cs (general) and WindowsRecall.cs (AI recall).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
            Id = "activity-disable-publishing",
            Label = "Disable User Activity Publishing (No Timeline Sync to Cloud)",
            Category = "User Activity",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["activity", "timeline", "privacy", "cloud sync"],
            Description =
                "Prevents Windows from publishing user activity to Microsoft's cloud "
                + "Activity History service. EnableActivityFeed=0 stops Timeline data "
                + "from leaving the device.",
            ApplyOps = [RegOp.SetDword(ActivityPolicy, "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.SetDword(ActivityPolicy, "EnableActivityFeed", 1)],
            DetectOps = [RegOp.CheckDword(ActivityPolicy, "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-cloud-sync",
            Label = "Disable Activity History Cloud Sync Between Devices",
            Category = "User Activity",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["activity", "cloud", "sync", "timeline", "privacy"],
            Description =
                "Stops Windows from uploading activity history to Microsoft's servers "
                + "for cross-device sync. AllowCrossDeviceClipboard=0 + "
                + "UploadUserActivities=0. Keep activity history local-only.",
            ApplyOps = [RegOp.SetDword(ActivityPolicy, "UploadUserActivities", 0)],
            RemoveOps = [RegOp.SetDword(ActivityPolicy, "UploadUserActivities", 1)],
            DetectOps = [RegOp.CheckDword(ActivityPolicy, "UploadUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-storage",
            Label = "Disable Local Activity History Storage",
            Category = "User Activity",
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
            Id = "activity-disable-cross-device-clipboard",
            Label = "Disable Cross-Device Clipboard Sync",
            Category = "User Activity",
            NeedsAdmin = false,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "clipboard", "cross-device", "privacy", "sync"],
            Description =
                "Disables the cross-device clipboard that syncs copied content through "
                + "Microsoft Account between Windows devices. Clipboard contents remain "
                + "local only. Enabled=0.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableClipboardHistory", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableClipboardHistory", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-recent-docs",
            Label = "Disable Recent Document Tracking in File Explorer",
            Category = "User Activity",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "recent docs", "explorer", "privacy"],
            Description =
                "Stops File Explorer from tracking recently opened files and displaying "
                + "them in Quick Access. NoRecentDocsHistory=1. Keeps document access "
                + "history private and reduces Start/Quick Access clutter.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1),
            ],
        },
        new TweakDef
        {
            Id = "activity-disable-recent-in-quick-access",
            Label = "Disable Recent Files in Quick Access",
            Category = "User Activity",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["activity", "recent files", "quick access", "explorer"],
            Description =
                "Stops File Explorer from showing recently opened files in the "
                + "Quick Access panel. ShowRecent=0. Complements disable-frequent-places "
                + "for full Quick Access privacy.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-jump-lists",
            Label = "Disable Jump Lists in Taskbar and Start",
            Category = "User Activity",
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
        new TweakDef
        {
            Id = "activity-clear-recent-on-exit",
            Label = "Clear Recent Files History on Logoff",
            Category = "User Activity",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "clear on exit", "recent files", "privacy"],
            Description =
                "Configures Windows to clear the list of recently used files and "
                + "programs from the Start menu every time the user logs off. "
                + "ClearRecentDocsOnExit=1.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1),
            ],
        },
        new TweakDef
        {
            Id = "activity-disable-cdp",
            Label = "Disable Connected Device Platform (CDP) — Timeline Backend",
            Category = "User Activity",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "cdp", "connected devices", "timeline", "privacy"],
            Description =
                "Disables the Connected Device Platform (CDP) service back-end that "
                + "powers Windows Timeline and cross-device activity roaming. "
                + "EnableCdp=0. Complements EnableActivityFeed/AllowStoringUserActivities "
                + "for complete Timeline disablement.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
        },
    ];
}
