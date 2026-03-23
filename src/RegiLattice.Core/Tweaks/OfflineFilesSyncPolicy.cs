// RegiLattice.Core — Tweaks/OfflineFilesSyncPolicy.cs
// Category: "Offline Files Sync Policy" — Slug "offsync"
// Paths:
//   A) HKLM\SOFTWARE\Policies\Microsoft\Windows\NetCache  (Offline Files policy)
//   B) HKLM\SOFTWARE\Policies\Microsoft\Windows\SyncMgr   (Sync Center policy)
// Controls offline file caching behaviour, cache limits, and Sync Center activity.

#nullable enable

using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class OfflineFilesSyncPolicy
{
    private const string NetCache = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetCache";
    private const string SyncMgr = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SyncMgr";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "offsync-no-make-available-offline",
            Label = "Prevent Making Files Available Offline",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets NoMakeAvailableOffline=1 in NetCache policy. Blocks users from right-clicking shared files and selecting 'Always Available Offline'. Prevents uncontrolled growth of the offline cache on laptops and ensures only IT-assigned offline content is cached.",
            Tags = ["offline", "sync", "files", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(NetCache, "NoMakeAvailableOffline", 1)],
            RemoveOps = [RegOp.DeleteValue(NetCache, "NoMakeAvailableOffline")],
            DetectOps = [RegOp.CheckDword(NetCache, "NoMakeAvailableOffline", 1)],
        },
        new TweakDef
        {
            Id = "offsync-purge-at-logoff",
            Label = "Purge Offline Cache at Logoff",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets PurgeAtLogoff=1 in NetCache policy. Causes all locally cached offline files to be deleted when the user logs off. Ensures sensitive documents synced from file servers are not retained on shared or kiosk machines between sessions.",
            Tags = ["offline", "sync", "files", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(NetCache, "PurgeAtLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(NetCache, "PurgeAtLogoff")],
            DetectOps = [RegOp.CheckDword(NetCache, "PurgeAtLogoff", 1)],
        },
        new TweakDef
        {
            Id = "offsync-disable-background-sync",
            Label = "Disable Automatic Background Sync",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets BackgroundSyncEnabled=0 in NetCache policy. Stops the Offline Files CSC service from performing background synchronisation of the offline cache. Prevents unexpected I/O bursts and network traffic from silent sync operations, without disabling offline access entirely.",
            Tags = ["offline", "sync", "background", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(NetCache, "BackgroundSyncEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(NetCache, "BackgroundSyncEnabled")],
            DetectOps = [RegOp.CheckDword(NetCache, "BackgroundSyncEnabled", 0)],
        },
        new TweakDef
        {
            Id = "offsync-cache-disk-limit-5pct",
            Label = "Limit Offline Cache to 5% of Disk",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets DefaultCacheSize=5 in NetCache policy (percentage of disk). Restricts the maximum space the Offline Files cache may consume to 5% of the volume. Prevents the CSC cache from silently consuming large amounts of disk space on smaller system drives.",
            Tags = ["offline", "sync", "disk", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(NetCache, "DefaultCacheSize", 5)],
            RemoveOps = [RegOp.DeleteValue(NetCache, "DefaultCacheSize")],
            DetectOps = [RegOp.CheckDword(NetCache, "DefaultCacheSize", 5)],
        },
        new TweakDef
        {
            Id = "offsync-go-offline-manual",
            Label = "Set Go-Offline Action to Manual",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets GoOfflineAction=0 (manual) in NetCache policy. Controls what happens when a network connection to a file server is lost: 0=work offline silently, 1=notify and ask. Setting 0 prevents disruptive dialogs on unstable connections while relying on manual sync on reconnect.",
            Tags = ["offline", "sync", "notification", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(NetCache, "GoOfflineAction", 0)],
            RemoveOps = [RegOp.DeleteValue(NetCache, "GoOfflineAction")],
            DetectOps = [RegOp.CheckDword(NetCache, "GoOfflineAction", 0)],
        },
        new TweakDef
        {
            Id = "offsync-minimal-event-logging",
            Label = "Reduce Offline Files Event Log Verbosity",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets EventLoggingLevel=1 in NetCache policy. Reduces the Offline Files event log from informational (2) to warnings-only (1). Eliminates high-frequency informational events from the CSC service in the System event log on machines with many network shares.",
            Tags = ["offline", "sync", "eventlog", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(NetCache, "EventLoggingLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(NetCache, "EventLoggingLevel")],
            DetectOps = [RegOp.CheckDword(NetCache, "EventLoggingLevel", 1)],
        },
        new TweakDef
        {
            Id = "offsync-disable-sync-activity-display",
            Label = "Disable Sync Center Activity Display",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets DisableSyncActivity=1 in SyncMgr policy. Prevents the Sync Center from displaying sync progress and activity in the notification area and the Sync Center dialog. Reduces UI clutter from background sync operations on shared desktops.",
            Tags = ["synccenter", "offline", "sync", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SyncMgr, "DisableSyncActivity", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncMgr, "DisableSyncActivity")],
            DetectOps = [RegOp.CheckDword(SyncMgr, "DisableSyncActivity", 1)],
        },
        new TweakDef
        {
            Id = "offsync-disable-metered-sync",
            Label = "Disable Sync on Metered Connections",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets TurnOffSyncOnCostedNetwork=1 in SyncMgr policy. Prevents Sync Center from initiating any synchronisation when the active network connection is marked as metered (mobile hotspot, LTE, or manually flagged as metered). Prevents unexpected data charges.",
            Tags = ["synccenter", "offline", "sync", "metered", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SyncMgr, "TurnOffSyncOnCostedNetwork", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncMgr, "TurnOffSyncOnCostedNetwork")],
            DetectOps = [RegOp.CheckDword(SyncMgr, "TurnOffSyncOnCostedNetwork", 1)],
        },
        new TweakDef
        {
            Id = "offsync-disable-file-sync-client",
            Label = "Disable Sync Center File Sync Client",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets DisableFileSyncClient=1 in SyncMgr policy. Fully disables the Sync Center file synchronisation client component. This stops the CSC service from registering as a sync provider in the Sync Center UI, effectively turning off user-initiated and scheduled offline sync.",
            Tags = ["synccenter", "offline", "sync", "policy", "disable"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SyncMgr, "DisableFileSyncClient", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncMgr, "DisableFileSyncClient")],
            DetectOps = [RegOp.CheckDword(SyncMgr, "DisableFileSyncClient", 1)],
        },
        new TweakDef
        {
            Id = "offsync-hide-in-sync-ui",
            Label = "Hide Offline Files from Sync Center UI",
            Category = "Offline Files Sync Policy",
            Description =
                "Sets HideOptionsForSyncProvider=1 in SyncMgr policy. Removes the options and settings icon for the Offline Files sync provider from the Sync Center window, preventing users from modifying sync provider configuration while still allowing the provider to run.",
            Tags = ["synccenter", "offline", "sync", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SyncMgr, "HideOptionsForSyncProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncMgr, "HideOptionsForSyncProvider")],
            DetectOps = [RegOp.CheckDword(SyncMgr, "HideOptionsForSyncProvider", 1)],
        },
    ];
}
