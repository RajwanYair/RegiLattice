namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class OneDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "od-onedrive-upload-throttle",
            Label = "Throttle OneDrive Upload (1 MB/s)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 1000 KB/s to prevent saturating your connection.",
            Tags = ["onedrive", "bandwidth", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal",
            Label = "Disable OneDrive Personal Account Sign-In",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from signing in with a personal Microsoft account in OneDrive.",
            Tags = ["onedrive", "personal", "signin", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-max-upload-rate",
            Label = "Limit OneDrive Upload Rate (125 KB/s)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps OneDrive upload bandwidth at 125 KB/s to minimise network impact.",
            Tags = ["onedrive", "bandwidth", "upload", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125)],
        },
        new TweakDef
        {
            Id = "od-onedrive-max-download-rate",
            Label = "Limit OneDrive Download Rate (1000 KB/s)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps OneDrive download bandwidth at 1000 KB/s to minimise network impact.",
            Tags = ["onedrive", "bandwidth", "download", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-office-collab",
            Label = "Disable Office Collaboration via OneDrive",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Office co-authoring feature that uses OneDrive sync.",
            Tags = ["onedrive", "office", "collaboration", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-silent-config",
            Label = "Enable Silent OneDrive Account Configuration",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Silently configures OneDrive with the user's Windows credentials without prompts.",
            Tags = ["onedrive", "silent", "config", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-reduce-bandwidth",
            Label = "OneDrive Reduce Sync Traffic",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits OneDrive upload bandwidth to 50%. Prevents OneDrive from saturating network connection. Default: Unlimited. Recommended: 50% for shared networks.",
            Tags = ["onedrive", "bandwidth", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-fod-global",
            Label = "Disable OneDrive Files On-Demand (Global)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables OneDrive Files On-Demand globally via policy. Forces all files to be downloaded locally. Default: Enabled. Recommended: Disabled for offline use.",
            Tags = ["onedrive", "files-on-demand", "policy", "offline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal-sync",
            Label = "Disable Personal OneDrive Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables personal OneDrive file sync via DisableFileSyncNGSC policy. Prevents OneDrive from syncing any personal accounts. Default: Enabled. Recommended: Disabled on corporate machines.",
            Tags = ["onedrive", "sync", "personal", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-backup-prompt",
            Label = "Disable OneDrive Folder Backup Prompt",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks the OneDrive Known Folder Move (KFM) opt-in prompt that asks users to back up Desktop, Documents, and Pictures. Default: Enabled. Recommended: Disabled on managed machines.",
            Tags = ["onedrive", "backup", "kfm", "prompt"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-block-business-sync",
            Label = "Block OneDrive for Business Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks OneDrive from syncing with external or business SharePoint organizations. Default: Allowed. Recommended: Blocked on personal machines.",
            Tags = ["onedrive", "business", "sync", "external"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-collaboration",
            Label = "Disable OneDrive File Collaboration",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables OneDrive OCSI co-authoring clients for real-time file collaboration. Default: Enabled. Recommended: Disabled for offline-only workflows.",
            Tags = ["onedrive", "collaboration", "coauthoring", "ocsi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal",
            Label = "Disable OneDrive Personal Account Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from adding personal OneDrive accounts. Only business accounts allowed. Default: allowed.",
            Tags = ["onedrive", "personal", "account", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-limit-upload-bandwidth",
            Label = "Limit OneDrive Upload Bandwidth to 512 KB/s",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 512 KB/s. Prevents OneDrive from saturating the network. Default: unlimited.",
            Tags = ["onedrive", "bandwidth", "upload", "throttle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 512)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 512)],
        },
        new TweakDef
        {
            Id = "od-disable-toast-notifications",
            Label = "Disable OneDrive Toast Notifications",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive sync error and activity toast notifications. Default: enabled.",
            Tags = ["onedrive", "notifications", "toast", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "od-block-external-sync",
            Label = "Block External OneDrive Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive from syncing with external organizations. Data stays in-org. Default: allowed.",
            Tags = ["onedrive", "external", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-set-max-file-size-5gb",
            Label = "Set OneDrive Max Upload File Size to 5 GB",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the maximum file size OneDrive will sync to 5 GB. Default: no limit.",
            Tags = ["onedrive", "file-size", "limit", "upload"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB", 5120)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DiskSpaceCheckThresholdMB", 5120)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-ads",
            Label = "Disable OneDrive Ads & Promotions",
            Category = "OneDrive",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables promotional ads and tips in OneDrive sync client. Prevents upgrade nag popups. Default: enabled.",
            Tags = ["onedrive", "ads", "promotions", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-autostart",
            Label = "Disable OneDrive Auto-Start",
            Category = "OneDrive",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents OneDrive from automatically starting on Windows login. Frees memory and network bandwidth. Default: auto-starts.",
            Tags = ["onedrive", "autostart", "startup", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "OneDrive")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "OneDrive",
                    @"%LOCALAPPDATA%\Microsoft\OneDrive\OneDrive.exe /background"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "OneDrive")],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-fod",
            Label = "Disable OneDrive Files On Demand",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On Demand feature enterprise-wide via policy. All files are kept local. Default: enabled.",
            Tags = ["onedrive", "files-on-demand", "policy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-kfm",
            Label = "Disable OneDrive Known Folder Move",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks OneDrive from silently moving Desktop, Documents, Pictures to cloud. Prevents automatic folder redirection. Default: allowed.",
            Tags = ["onedrive", "kfm", "folder-move", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal-sync",
            Label = "Block OneDrive Personal Account Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive from syncing personal (non-work) accounts via policy. Only enterprise tenants allowed. Default: allowed.",
            Tags = ["onedrive", "personal", "sync", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-files-on-demand",
            Label = "Disable OneDrive Files On Demand (User)",
            Category = "OneDrive",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables OneDrive Files On Demand at user level. Files are always downloaded fully. Avoids placeholder files. Default: enabled.",
            Tags = ["onedrive", "files-on-demand", "user", "local"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-disable-kfm-opt-in-prompt",
            Label = "Block Known Folder Move Opt-In Prompt",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from prompting users to move Desktop/Documents/Pictures to OneDrive (Known Folder Move wizard).",
            Tags = ["onedrive", "known-folder-move", "kfm", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-kfm-silent-redirect",
            Label = "Block Silent Known Folder Redirect",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks OneDrive from silently redirecting known folders (Desktop, Documents, Pictures) to cloud storage without prompting.",
            Tags = ["onedrive", "known-folder-move", "kfm", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptOut", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-delay-update-ring",
            Label = "Delay OneDrive Client Update Ring",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keeps the OneDrive sync client on the Deferred ring to avoid destabilising updates.",
            Tags = ["onedrive", "update", "ring", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DelaySyncClientUpdateRing", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-sharepoint-sync",
            Label = "Disable SharePoint Sync Library",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from syncing SharePoint document libraries to the local machine.",
            Tags = ["onedrive", "sharepoint", "sync", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableSharePointSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-app-sync",
            Label = "Disable OneDrive Application Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from syncing application settings (AppData\\Roaming) to cloud storage.",
            Tags = ["onedrive", "appsync", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableApplicationSync", 1)],
        },
        new TweakDef
        {
            Id = "od-limit-mass-delete-threshold",
            Label = "OneDrive Mass-Delete Warning Threshold (50 files)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the threshold at which OneDrive warns before deleting large numbers of files to 50 (down from default of 200).",
            Tags = ["onedrive", "mass-delete", "safety", "warning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "LocalMassDeleteFileDeleteThreshold", 50)],
        },
        new TweakDef
        {
            Id = "od-disable-hydration-on-access",
            Label = "Disable Auto-Hydration on File Access",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents OneDrive from automatically downloading cloud-only placeholder files when opened by an app. Avoids unexpected bandwidth usage.",
            Tags = ["onedrive", "hydration", "files-on-demand", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "PreventOneDriveFilesOnDemandPreview", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-auto-update",
            Label = "Disable OneDrive Auto-Update",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents OneDrive client from auto-updating in the background. Useful in managed environments where updates are controlled centrally.",
            Tags = ["onedrive", "update", "autoupdate", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-file-explorer-hub",
            Label = "Remove OneDrive from File Explorer Left Panel",
            Category = "OneDrive",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the OneDrive folder entry from the File Explorer navigation pane without disabling the sync process.",
            Tags = ["onedrive", "explorer", "sidebar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "od-block-external-collab",
            Label = "Block OneDrive External Collaboration (Policy)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from sharing OneDrive files with users outside of the organisation via Group Policy.",
            Tags = ["onedrive", "external-sharing", "collaboration", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSharing", 1)],
        },
    ];
}
