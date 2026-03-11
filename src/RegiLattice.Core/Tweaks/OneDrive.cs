namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class OneDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "od-disable-onedrive-autostart",
            Label = "Disable OneDrive Auto-Start",
            Category = "OneDrive",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents OneDrive from starting automatically at login.",
            Tags = ["onedrive", "autostart", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-fod",
            Label = "Disable OneDrive Files On-Demand",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Files On-Demand — all files are downloaded locally instead of being cloud-only placeholders.",
            Tags = ["onedrive", "sync", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-ads",
            Label = "Disable OneDrive Ads / Upsell",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides OneDrive promotional and upsell notifications.",
            Tags = ["onedrive", "privacy", "ads"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
        },
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 1000),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit"),
            ],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-personal-sync",
            Label = "Disable OneDrive Personal Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks OneDrive consumer (personal) account sync via policy.",
            Tags = ["onedrive", "sync", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
        },
        new TweakDef
        {
            Id = "od-disable-onedrive-kfm",
            Label = "Block OneDrive Known Folder Move",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents OneDrive from prompting to move Desktop, Documents & Pictures.",
            Tags = ["onedrive", "kfm", "folders"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 125),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit", 1000),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DownloadBandwidthLimit"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "SilentAccountConfig", 1)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-files-on-demand",
            Label = "Disable OneDrive Files On-Demand",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On-Demand (cloud-only placeholders). All files will be downloaded locally. Default: Enabled. Recommended: Disabled for offline reliability.",
            Tags = ["onedrive", "files-on-demand", "offline", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
        },
        new TweakDef
        {
            Id = "od-onedrive-reduce-bandwidth",
            Label = "OneDrive Reduce Sync Traffic",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits OneDrive upload bandwidth to 50%. Prevents OneDrive from saturating network connection. Default: Unlimited. Recommended: 50% for shared networks.",
            Tags = ["onedrive", "bandwidth", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "UploadBandwidthLimit", 50)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-fod-global",
            Label = "Disable OneDrive Files On-Demand (Global)",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables OneDrive Files On-Demand globally via policy. Forces all files to be downloaded locally. Default: Enabled. Recommended: Disabled for offline use.",
            Tags = ["onedrive", "files-on-demand", "policy", "offline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "od-onedrive-disable-personal-sync",
            Label = "Disable Personal OneDrive Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables personal OneDrive file sync via DisableFileSyncNGSC policy. Prevents OneDrive from syncing any personal accounts. Default: Enabled. Recommended: Disabled on corporate machines.",
            Tags = ["onedrive", "sync", "personal", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OneDrive"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-backup-prompt",
            Label = "Disable OneDrive Folder Backup Prompt",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks the OneDrive Known Folder Move (KFM) opt-in prompt that asks users to back up Desktop, Documents, and Pictures. Default: Enabled. Recommended: Disabled on managed machines.",
            Tags = ["onedrive", "backup", "kfm", "prompt"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "od-block-business-sync",
            Label = "Block OneDrive for Business Sync",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks OneDrive from syncing with external or business SharePoint organizations. Default: Allowed. Recommended: Blocked on personal machines.",
            Tags = ["onedrive", "business", "sync", "external"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "BlockExternalSync", 1)],
        },
        new TweakDef
        {
            Id = "od-disable-collaboration",
            Label = "Disable OneDrive File Collaboration",
            Category = "OneDrive",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables OneDrive OCSI co-authoring clients for real-time file collaboration. Default: Enabled. Recommended: Disabled for offline-only workflows.",
            Tags = ["onedrive", "collaboration", "coauthoring", "ocsi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "EnableAllOcsiClients", 0)],
        },
    ];
}
