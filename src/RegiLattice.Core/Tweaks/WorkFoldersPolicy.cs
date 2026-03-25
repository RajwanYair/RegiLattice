#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 247 — Work Folders Sync Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\WorkFolders
//       HKCU\Software\Policies\Microsoft\Windows\WorkFolders
internal static class WorkFoldersPolicy
{
    private const string WfLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkFolders";
    private const string WfCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WorkFolders";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wf-disable-work-folders",
            Label = "Disable Work Folders Sync (Machine)",
            Category = "Work Folders Policy",
            Description =
                "Sets SyncDisabled=1 in the machine-side Work Folders policy key. "
                + "Prevents Work Folders sync from running for all users on this machine, "
                + "blocking the sync client from connecting to corporate Work Folders servers. "
                + "Default: absent (sync allowed). Recommended: 1 on machines where cloud sync must be fully controlled.",
            Tags = ["work-folders", "sync", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "All Work Folders sync disabled machine-wide; existing synced content remains but is not updated.",
            ApplyOps = [RegOp.SetDword(WfLm, "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(WfLm, "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "wf-disable-work-folders-user",
            Label = "Disable Work Folders Sync (Current User)",
            Category = "Work Folders Policy",
            Description =
                "Sets SyncDisabled=1 in the per-user Work Folders policy key. "
                + "Prevents Work Folders sync for the current user account without a machine-wide restriction. "
                + "Default: absent. Recommended: 1 for individual user profiles on shared machines.",
            Tags = ["work-folders", "sync", "user", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Work Folders sync disabled for the current user only; other users on the machine are unaffected.",
            ApplyOps = [RegOp.SetDword(WfCu, "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WfCu, "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(WfCu, "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "wf-force-automatic-setup",
            Label = "Force Work Folders Setup Automatically",
            Category = "Work Folders Policy",
            Description =
                "Sets AutoProvision=1 in the machine Work Folders policy key. "
                + "Forces Work Folders to be set up automatically using a server URL configured via MDM or GP, "
                + "without requiring the user to manually configure the sync connection. "
                + "Default: absent (manual setup). Recommended: 1 in enterprise deployments using MDM provisioning.",
            Tags = ["work-folders", "auto-provision", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Automatically provisions Work Folders on first logon if a ServerUrl is configured.",
            ApplyOps = [RegOp.SetDword(WfLm, "AutoProvision", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "AutoProvision")],
            DetectOps = [RegOp.CheckDword(WfLm, "AutoProvision", 1)],
        },
        new TweakDef
        {
            Id = "wf-block-server-url-change",
            Label = "Prevent Users Changing Work Folders Server URL",
            Category = "Work Folders Policy",
            Description =
                "Sets UserServerAddrLocked=1 in the machine Work Folders policy key. "
                + "Locks the Work Folders server address, preventing users from reconfiguring or redirecting "
                + "their sync client to a different server. Useful for enforcing corporate sync server usage. "
                + "Default: absent. Recommended: 1 in managed environments with a designated Work Folders server.",
            Tags = ["work-folders", "server-url", "lock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users cannot change the Work Folders server URL; admin-configured URL is enforced.",
            ApplyOps = [RegOp.SetDword(WfLm, "UserServerAddrLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "UserServerAddrLocked")],
            DetectOps = [RegOp.CheckDword(WfLm, "UserServerAddrLocked", 1)],
        },
        new TweakDef
        {
            Id = "wf-require-encryption",
            Label = "Require Work Folders Content Encryption",
            Category = "Work Folders Policy",
            Description =
                "Sets LocalFolderEncryptionEnabled=1 in the machine Work Folders policy key. "
                + "Requires that all locally synced Work Folders content be encrypted at rest "
                + "using EFS (Encrypting File System). Protects sensitive data in case of device theft. "
                + "Default: absent. Recommended: 1 on portable devices (laptops) with sensitive data.",
            Tags = ["work-folders", "encryption", "efs", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Work Folders content encrypted with EFS; requires user profile EFS certificate and may slow file access.",
            ApplyOps = [RegOp.SetDword(WfLm, "LocalFolderEncryptionEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "LocalFolderEncryptionEnabled")],
            DetectOps = [RegOp.CheckDword(WfLm, "LocalFolderEncryptionEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wf-disable-work-folders-ui",
            Label = "Hide Work Folders from Navigation Pane",
            Category = "Work Folders Policy",
            Description =
                "Sets ExplorerNavigationPaneHideWorkFolders=1 in the machine Work Folders policy key. "
                + "Removes Work Folders entry from the File Explorer navigation pane, "
                + "preventing users from browsing or accessing the sync folder via Explorer's left panel. "
                + "Default: absent. Recommended: 1 when Work Folders is deployed but the UI should not be visible.",
            Tags = ["work-folders", "explorer", "navigation", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Hides Work Folders from Explorer navigation; the sync folder still exists on disk.",
            ApplyOps = [RegOp.SetDword(WfLm, "ExplorerNavigationPaneHideWorkFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "ExplorerNavigationPaneHideWorkFolders")],
            DetectOps = [RegOp.CheckDword(WfLm, "ExplorerNavigationPaneHideWorkFolders", 1)],
        },
        new TweakDef
        {
            Id = "wf-prevent-use-work-folders",
            Label = "Prevent Users from Configuring Work Folders",
            Category = "Work Folders Policy",
            Description =
                "Sets PreventWorkFolderFromUse=1 in the machine Work Folders policy key. "
                + "Blocks users from setting up or enrolling in Work Folders from PC Settings or Explorer. "
                + "Differs from SyncDisabled in that it prevents initial setup rather than halting an existing sync. "
                + "Default: absent. Recommended: 1 on machines where Work Folders must not be used.",
            Tags = ["work-folders", "prevent", "setup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks users from enrolling in Work Folders; does not affect already-synced folders.",
            ApplyOps = [RegOp.SetDword(WfLm, "PreventWorkFolderFromUse", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "PreventWorkFolderFromUse")],
            DetectOps = [RegOp.CheckDword(WfLm, "PreventWorkFolderFromUse", 1)],
        },
        new TweakDef
        {
            Id = "wf-prevent-change-sync-settings",
            Label = "Prevent Users Changing Work Folders Sync Settings",
            Category = "Work Folders Policy",
            Description =
                "Sets SyncSettingsLocked=1 in the per-user Work Folders policy key. "
                + "Locks Work Folders sync settings for the current user, preventing changes to "
                + "sync frequency, bandwidth usage, and folder location from the Settings UI. "
                + "Default: absent. Recommended: 1 in managed deployments with standardised sync policies.",
            Tags = ["work-folders", "sync-settings", "lock", "user", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "User cannot change Work Folders sync settings; current settings remain in effect.",
            ApplyOps = [RegOp.SetDword(WfCu, "SyncSettingsLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(WfCu, "SyncSettingsLocked")],
            DetectOps = [RegOp.CheckDword(WfCu, "SyncSettingsLocked", 1)],
        },
        new TweakDef
        {
            Id = "wf-disable-background-sync",
            Label = "Disable Work Folders Background Sync",
            Category = "Work Folders Policy",
            Description =
                "Sets BackgroundSyncDisabled=1 in the machine Work Folders policy key. "
                + "Prevents Work Folders from syncing in the background while the user is away, "
                + "reducing network traffic and battery usage on mobile devices. "
                + "Default: absent (background sync enabled). Recommended: 1 on metered connections or battery-sensitive devices.",
            Tags = ["work-folders", "background-sync", "battery", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Work Folders only syncs on user demand; background sync bandwidth and battery usage eliminated.",
            ApplyOps = [RegOp.SetDword(WfLm, "BackgroundSyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "BackgroundSyncDisabled")],
            DetectOps = [RegOp.CheckDword(WfLm, "BackgroundSyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "wf-set-sync-interval",
            Label = "Set Work Folders Minimum Sync Interval to 15 Minutes",
            Category = "Work Folders Policy",
            Description =
                "Sets MinSyncInterval=15 in the machine Work Folders policy key. "
                + "Configures the minimum time between automatic sync cycles to 15 minutes, "
                + "reducing sync frequency to lower network utilisation on busy or metered connections. "
                + "Default: absent (OS default ~1 minute). Recommended: 15 on bandwidth-constrained networks.",
            Tags = ["work-folders", "sync-interval", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Sync runs at most every 15 minutes; reduces background network traffic on metered links.",
            ApplyOps = [RegOp.SetDword(WfLm, "MinSyncInterval", 15)],
            RemoveOps = [RegOp.DeleteValue(WfLm, "MinSyncInterval")],
            DetectOps = [RegOp.CheckDword(WfLm, "MinSyncInterval", 15)],
        },
    ];
}
