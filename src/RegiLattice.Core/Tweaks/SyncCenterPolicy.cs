#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 241 — Sync Center GP Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\SyncMgr
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\OfflineFiles
internal static class SyncCenterPolicy
{
    private const string SyncMgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SyncMgr";
    private const string OfflineKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OfflineFiles";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "syncctr-disable-sync-center",
            Label = "Disable Sync Center",
            Category = "Sync Center GP Policy",
            Description = "Prevents users from using Windows Sync Center to synchronize files with network share partnerships.",
            Tags = ["sync-center", "offline-files", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Disables Sync Center UI and background sync. Offline file access on synced shares stops working.",
            ApplyOps = [RegOp.SetDword(SyncMgrKey, "DisableSyncMgr", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncMgrKey, "DisableSyncMgr")],
            DetectOps = [RegOp.CheckDword(SyncMgrKey, "DisableSyncMgr", 1)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-setup-wizard",
            Label = "Disable Sync Center Setup Wizard",
            Category = "Sync Center GP Policy",
            Description = "Prevents the Offline Files setup wizard from running, blocking new sync partnership creation.",
            Tags = ["sync-center", "wizard", "offline-files", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks wizard-based setup; existing partnerships are unaffected.",
            ApplyOps = [RegOp.SetDword(SyncMgrKey, "DisableSyncScheduleCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncMgrKey, "DisableSyncScheduleCreation")],
            DetectOps = [RegOp.CheckDword(SyncMgrKey, "DisableSyncScheduleCreation", 1)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-offline-files",
            Label = "Disable Offline Files Feature",
            Category = "Sync Center GP Policy",
            Description = "Turns off the Offline Files feature entirely; files cannot be made available offline.",
            Tags = ["offline-files", "sync-center", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Fully disables Offline Files. Impacts roaming users who rely on network files when disconnected.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-user-configuration",
            Label = "Prevent Users from Configuring Offline Files",
            Category = "Sync Center GP Policy",
            Description = "Removes the ability for users to change Offline Files settings through the Control Panel.",
            Tags = ["offline-files", "user-config", "policy", "lockdown"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Admins retain control; users cannot disable or configure offline files themselves.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "NoConfigChanges", 1)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "NoConfigChanges")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "NoConfigChanges", 1)],
        },
        new TweakDef
        {
            Id = "syncctr-remove-folder-from-offline",
            Label = "Disable 'Make Available Offline' Context Menu Option",
            Category = "Sync Center GP Policy",
            Description = "Hides the 'Make Available Offline' option from the right-click context menu for network files.",
            Tags = ["offline-files", "context-menu", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "UI-only restriction; users cannot initiate offline caching via right-click.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "NoMakeAvailableOffline", 1)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "NoMakeAvailableOffline")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "NoMakeAvailableOffline", 1)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-slow-link-mode",
            Label = "Disable Slow-Link Mode for Offline Files",
            Category = "Sync Center GP Policy",
            Description = "Prevents Windows from automatically switching to offline mode on slow network connections.",
            Tags = ["offline-files", "slow-link", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Forces online mode even on slow links; may degrade performance but prevents unintended offline switching.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "SlowLinkEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "SlowLinkEnabled")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "SlowLinkEnabled", 0)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-background-sync",
            Label = "Disable Background Synchronisation of Offline Files",
            Category = "Sync Center GP Policy",
            Description = "Prevents Offline Files from running background sync jobs when the user is logged on.",
            Tags = ["offline-files", "background-sync", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "No background sync; saves bandwidth and CPU. Manual sync still works.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "BackgroundSyncEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "BackgroundSyncEnabled")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "BackgroundSyncEnabled", 0)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-logon-sync",
            Label = "Disable Logon Synchronisation of Offline Files",
            Category = "Sync Center GP Policy",
            Description = "Prevents Offline Files from performing a sync when the user logs on.",
            Tags = ["offline-files", "logon", "sync", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Faster logons; offline files may be stale until explicitly synced.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "SyncAtLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "SyncAtLogon")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "SyncAtLogon", 0)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-logoff-sync",
            Label = "Disable Logoff Synchronisation of Offline Files",
            Category = "Sync Center GP Policy",
            Description = "Prevents Offline Files from performing a sync when the user logs off.",
            Tags = ["offline-files", "logoff", "sync", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Faster logoffs; changes made offline will not be pushed back automatically on logoff.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "SyncAtLogoff", 0)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "SyncAtLogoff")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "SyncAtLogoff", 0)],
        },
        new TweakDef
        {
            Id = "syncctr-disable-reminders",
            Label = "Disable Offline Files Sync Reminder Notifications",
            Category = "Sync Center GP Policy",
            Description = "Suppresses balloon-tip reminders about Offline Files synchronisation status.",
            Tags = ["offline-files", "notifications", "reminders", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces notification noise. Users will not be reminded to sync.",
            ApplyOps = [RegOp.SetDword(OfflineKey, "GoOfflineAction", 1)],
            RemoveOps = [RegOp.DeleteValue(OfflineKey, "GoOfflineAction")],
            DetectOps = [RegOp.CheckDword(OfflineKey, "GoOfflineAction", 1)],
        },
    ];
}
