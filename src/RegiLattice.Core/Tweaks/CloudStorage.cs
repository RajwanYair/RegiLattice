namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudStorage
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cloud-disable-dropbox-autostart",
            Label = "Disable Dropbox Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Dropbox from starting automatically at login.",
            Tags = ["dropbox", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Dropbox"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "DropboxUpdate"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableAutoStart"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Dropbox")],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-update",
            Label = "Disable Dropbox Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Dropbox from automatically checking for and installing updates.",
            Tags = ["dropbox", "update", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-lan-sync",
            Label = "Disable Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Dropbox LAN Sync (peer-to-peer discovery on the local network). Reduces network chatter and improves privacy on shared networks.",
            Tags = ["dropbox", "lan", "network", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-autostart",
            Label = "Disable Google Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Google Drive (DriveFS) from starting at login.",
            Tags = ["gdrive", "google", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableAutoStart"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-update",
            Label = "Disable Google Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Google Drive from auto-updating via policy.",
            Tags = ["gdrive", "google", "update", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-gdrive-bandwidth-limit",
            Label = "Limit Google Drive Upload (1 MB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps Google Drive upload bandwidth at 1 MB/s to prevent saturating your internet connection during large syncs.",
            Tags = ["gdrive", "google", "bandwidth", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthRxKBPS", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS", 1024),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthRxKBPS"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "BandwidthTxKBPS", 1024)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-autostart",
            Label = "Disable iCloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents iCloud Drive and iCloud Services from starting at login.",
            Tags = ["icloud", "apple", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudDrive"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudServices"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "iCloudDrive")],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photos",
            Label = "Disable iCloud Photo Stream Upload",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic photo stream uploads via iCloud for Windows.",
            Tags = ["icloud", "apple", "photos", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-autostart",
            Label = "Disable Box Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Box / Box Drive from starting automatically at login.",
            Tags = ["box", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Box"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "BoxDrive"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Box")],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-autostart",
            Label = "Disable MEGA Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGAsync from starting automatically at login.",
            Tags = ["mega", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
        },
        new TweakDef
        {
            Id = "cloud-disable-pcloud-autostart",
            Label = "Disable pCloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents pCloud Drive from starting automatically at login.",
            Tags = ["pcloud", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive")],
        },
        new TweakDef
        {
            Id = "cloud-disable-nextcloud-autostart",
            Label = "Disable Nextcloud Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Nextcloud desktop client from starting at login.",
            Tags = ["nextcloud", "autostart", "cloud", "opensource"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud")],
        },
        new TweakDef
        {
            Id = "cloud-disable-tresorit-autostart",
            Label = "Disable Tresorit Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Tresorit from starting automatically at login.",
            Tags = ["tresorit", "autostart", "cloud", "encrypted"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit")],
        },
        new TweakDef
        {
            Id = "cloud-disable-synccom-autostart",
            Label = "Disable Sync.com Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Sync.com desktop client from starting at login.",
            Tags = ["sync.com", "autostart", "cloud", "encrypted"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com")],
        },
        new TweakDef
        {
            Id = "cloud-disable-spideroak-autostart",
            Label = "Disable SpiderOak ONE Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents SpiderOak ONE backup from starting at login.",
            Tags = ["spideroak", "autostart", "cloud", "backup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE")],
        },
        new TweakDef
        {
            Id = "cloud-disable-amazondrive-autostart",
            Label = "Disable Amazon Drive Auto-Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Amazon Drive from starting automatically at login.",
            Tags = ["amazon", "autostart", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive")],
        },
        new TweakDef
        {
            Id = "cloud-dropbox-upload-throttle",
            Label = "Throttle Dropbox Upload (512 KB/s)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Caps Dropbox upload bandwidth at 512 KB/s to prevent saturating your internet connection.",
            Tags = ["dropbox", "bandwidth", "throttle", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate", 512),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_style", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_style"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "throttle_upload_rate", 512)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-telemetry",
            Label = "Disable Dropbox Telemetry",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Dropbox analytics and telemetry data collection.",
            Tags = ["dropbox", "telemetry", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1)],
        },
        new TweakDef
        {
            Id = "cloud-gdrive-cache-limit",
            Label = "Limit Google Drive Cache (10 GB)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the Google Drive File Stream local cache at 10 GB to recover disk space on smaller SSDs.",
            Tags = ["gdrive", "google", "cache", "disk", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-telemetry",
            Label = "Disable Google Drive Telemetry",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables crash reporting and usage stats for Google Drive.",
            Tags = ["gdrive", "google", "telemetry", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableUsageStats", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableUsageStats"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableCrashReporting", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-update",
            Label = "Disable MEGA Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGAsync from automatically checking for updates.",
            Tags = ["mega", "update", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-update",
            Label = "Disable Box Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Box Drive from automatically installing updates.",
            Tags = ["box", "update", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Box\Box"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-drive",
            Label = "Disable iCloud Drive Integration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables iCloud Drive Windows integration. Prevents iCloud from syncing files in Explorer. Default: Enabled. Recommended: Disabled if not using Apple devices.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync",
            Label = "Disable iCloud Auto-Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables iCloud automatic synchronization via Group Policy. Default: Enabled. Recommended: Disabled if not using Apple services.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-creative-cloud-startup",
            Label = "Disable Adobe Creative Cloud Startup",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Adobe Creative Cloud startup sync via policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["cloud", "adobe", "creative-cloud", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photo-sync",
            Label = "Disable iCloud Photo Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables iCloud Photo Stream automatic upload to prevent photos from syncing to Apple cloud services. Default: enabled. Recommended: disabled on corporate machines.",
            Tags = ["cloud", "icloud", "photo", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-offline",
            Label = "Disable Google Drive Offline Mode",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Google Drive offline mode via policy. Prevents local caching of Drive files, reducing disk usage. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "google-drive", "offline", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
        },
        new TweakDef
        {
            Id = "cloud-block-dropbox-lan-sync",
            Label = "Block Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks Dropbox LAN sync discovery which broadcasts on the local network. Improves security on shared networks. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "dropbox", "lan", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-onedrive-files-on-demand",
            Label = "Disable OneDrive Files On-Demand Policy",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables OneDrive Files On-Demand feature via Group Policy. All files download fully. Default: on-demand.",
            Tags = ["cloud", "onedrive", "files-on-demand", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive", "FilesOnDemandEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-google-drive-autostart",
            Label = "Disable Google Drive Autostart",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Google Drive from starting automatically at logon. Default: starts with Windows.",
            Tags = ["cloud", "google-drive", "startup", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS", "\"C:\\Program Files\\Google\\Drive File Stream\\launch.bat\"")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
        },
        new TweakDef
        {
            Id = "cloud-disable-box-auto-update",
            Label = "Disable Box Drive Auto-Update",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Box Drive from automatically checking for updates. Default: auto-update enabled.",
            Tags = ["cloud", "box", "update", "auto-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Box\Box", "AutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-mega-sync-autostart",
            Label = "Disable MEGA Sync Autostart",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents MEGA Sync client from starting automatically at logon. Default: starts with Windows.",
            Tags = ["cloud", "mega", "sync", "startup", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync", "\"C:\\Users\\%USERNAME%\\AppData\\Local\\MEGAsync\\MEGAsync.exe\"")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync-on-startup",
            Label = "Disable iCloud Sync on Startup",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Apple iCloud from starting automatically at login. Saves bandwidth and resources. Default: enabled.",
            Tags = ["cloud", "icloud", "sync", "autostart"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices", @"%ProgramFiles%\Common Files\Apple\Internet Services\iCloudServices.exe")],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
        },
        new TweakDef
        {
            Id = "cloud-disable-suggestions",
            Label = "Disable Cloud Storage Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows suggestions to use cloud storage services. Prevents Microsoft account and OneDrive promotions. Default: enabled.",
            Tags = ["cloud", "suggestions", "promotions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338388Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "cloud-overlay-optimise",
            Label = "Optimise Cloud Sync Overlay Icons",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables cloud-optimized content delivery from Windows. Reduces background data usage and telemetry from cloud storage features. Default: enabled.",
            Tags = ["cloud", "overlay", "sync", "optimise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
        },
    ];
}
