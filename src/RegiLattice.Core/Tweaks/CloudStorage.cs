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
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableAutoStart")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Update", "DisableUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-dropbox-lan-sync",
            Label = "Disable Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Dropbox LAN Sync (peer-to-peer discovery on the local network). Reduces network chatter and improves privacy on shared networks.",
            Tags = ["dropbox", "lan", "network", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Dropbox\Config"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Dropbox\Config", "p2p_enabled", 1)],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GoogleDriveFS")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableAutoStart")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "AutoUpdateDisabled")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud", "DisablePhotoStream")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "MEGAsync")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "pCloud Drive")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Nextcloud")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Tresorit")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Sync.com")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "SpiderOakONE")],
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
            ApplyOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Amazon Drive")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox", "DisableAnalytics")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB", 10240)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "MaxCacheSizeMB")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Mega Limited\MEGAsync", "DisableAutoUpdates")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Box\Box", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-drive",
            Label = "Disable iCloud Drive Integration",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables iCloud Drive Windows integration. Prevents iCloud from syncing files in Explorer. Default: Enabled. Recommended: Disabled if not using Apple devices.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\Internet Services", "iCloudDriveDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-sync",
            Label = "Disable iCloud Auto-Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables iCloud automatic synchronization via Group Policy. Default: Enabled. Recommended: Disabled if not using Apple services.",
            Tags = ["cloud", "icloud", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Apple\iCloud", "DisableSync")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess\cep", "disableSync", 1)],
        },
        new TweakDef
        {
            Id = "cloud-disable-icloud-photo-sync",
            Label = "Disable iCloud Photo Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables iCloud Photo Stream automatic upload to prevent photos from syncing to Apple cloud services. Default: enabled. Recommended: disabled on corporate machines.",
            Tags = ["cloud", "icloud", "photo", "sync", "apple"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Apple Inc.\iCloud\PhotoStream", "AutoUpload", 0)],
        },
        new TweakDef
        {
            Id = "cloud-disable-gdrive-offline",
            Label = "Disable Google Drive Offline Mode",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Google Drive offline mode via policy. Prevents local caching of Drive files, reducing disk usage. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "google-drive", "offline", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\DriveFS", "DisableOfflineMode", 1)],
        },
        new TweakDef
        {
            Id = "cloud-block-dropbox-lan-sync",
            Label = "Block Dropbox LAN Sync",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks Dropbox LAN sync discovery which broadcasts on the local network. Improves security on shared networks. Default: enabled. Recommended: disabled.",
            Tags = ["cloud", "dropbox", "lan", "sync", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Dropbox\Sync", "DisableLanSync")],
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
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "GoogleDriveFS",
                    "\"C:\\Program Files\\Google\\Drive File Stream\\launch.bat\""
                ),
            ],
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
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "MEGAsync",
                    "\"C:\\Users\\%USERNAME%\\AppData\\Local\\MEGAsync\\MEGAsync.exe\""
                ),
            ],
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
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    "iCloudServices",
                    @"%ProgramFiles%\Common Files\Apple\Internet Services\iCloudServices.exe"
                ),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "iCloudServices")],
        },
        new TweakDef
        {
            Id = "cloud-disable-suggestions",
            Label = "Disable Cloud Storage Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows suggestions to use cloud storage services. Prevents Microsoft account and OneDrive promotions. Default: enabled.",
            Tags = ["cloud", "suggestions", "promotions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "cloud-overlay-optimise",
            Label = "Optimise Cloud Sync Overlay Icons",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables cloud-optimized content delivery from Windows. Reduces background data usage and telemetry from cloud storage features. Default: enabled.",
            Tags = ["cloud", "overlay", "sync", "optimise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
        },
    ];
}

// ── Merged from CloudExperience.cs ──────────────────────────────────────────────────

internal static class CloudExperience
{
    private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE";

    private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string ContentDelivery = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";

    private const string WindowsUpdate = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    private const string OobeUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\OOBE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "oobe-disable-consumer-features",
            Label = "Disable Consumer Cloud Features and Spotlight Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "consumer", "cloud", "suggestions", "privacy"],
            Description =
                "Disables Windows consumer features such as Microsoft Spotlight "
                + "advertisements and app suggestions delivered through cloud content. "
                + "DisableWindowsConsumerFeatures=1.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-ads",
            Label = "Disable Lock Screen Spotlight Ads (Enterprise)",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "ads", "policy"],
            Description =
                "Disables Windows Spotlight on the lock screen via the cloud content "
                + "policy key (DisableWindowsSpotlightFeatures=1). Prevents Microsoft "
                + "from rotating lock screen images and showing tips and ads.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsSpotlightFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsSpotlightFeatures", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-onboarding",
            Label = "Disable Post-OOBE Cloud Onboarding Prompts",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "onboarding", "cloud", "prompts", "enterprise"],
            Description =
                "Disables the post-login onboarding flow that invites users to connect "
                + "OneDrive, set up Microsoft 365, etc. Suitable for pre-imaged enterprise "
                + "deployments. SkipNotHerePrompts=1.",
            ApplyOps = [RegOp.SetDword(Oobe, "SkipNotHerePrompts", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "SkipNotHerePrompts")],
            DetectOps = [RegOp.CheckDword(Oobe, "SkipNotHerePrompts", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-silent-app-install",
            Label = "Disable Silent Background App Installation via CDM",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "silent install", "apps", "consumer"],
            Description =
                "Prevents the Content Delivery Manager from silently installing "
                + "suggested and sponsored apps in the background. "
                + "SilentInstalledAppsEnabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-tips",
            Label = "Disable OOBE and Start Tips (Welcome Messages)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "tips", "welcome", "onboarding"],
            Description =
                "Disables the 'Did you know' and welcome tips in the Start menu and "
                + "after Windows updates. SoftLandingEnabled=0 in Content Delivery Manager.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-roaming-profile-setup",
            Label = "Disable Roaming Profile Setup Prompt",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "roaming", "profile", "onedrive"],
            Description =
                "Suppresses the prompt to back up the Desktop, Documents, and Pictures "
                + "folders to OneDrive during OOBE. DesktopIconsPreference=1 "
                + "(keep local folders).",
            ApplyOps = [RegOp.SetDword(Oobe, "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(Oobe, "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-spotlight-on-desktop",
            Label = "Disable Spotlight Wallpaper Rotation on Desktop",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "wallpaper", "desktop", "cloud"],
            Description =
                "Prevents Windows Spotlight from rotating the desktop wallpaper. "
                + "RotatingLockScreenEnabled=0. Keeps a fixed wallpaper instead of "
                + "Microsoft's rotating Bing images.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "RotatingLockScreenEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscription-content",
            Label = "Disable Subscription-Based Cloud Content in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "subscription", "content", "start menu", "ads"],
            Description =
                "Disables subscription-based recommended and promoted content in the "
                + "Start menu. SubscribedContent-338388Enabled=0. Removes the "
                + "'Get the most out of Windows' suggestions.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions in Start and Store",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "third party", "suggestions", "start", "ads"],
            Description =
                "Disables third-party sponsored app suggestions in the Start menu and " + "Microsoft Store. SubscribedContent-338389Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-welcome-experience",
            Label = "Disable 'What's New' Welcome Experience After Updates",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "welcome", "what's new", "update", "cloud"],
            Description =
                "Prevents Windows from showing the 'What's new in Windows' splash screen "
                + "after feature updates complete. ContentDelivery SubscribedContent-310093Enabled=0.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-oem-preinstalled-apps",
            Label = "Disable OEM Pre-Installed Application Install",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "oem", "preinstalled", "apps", "bloatware"],
            Description =
                "Disables the silent installation of OEM-branded applications delivered "
                + "through the Content Delivery Manager (OemPreInstalledAppsEnabled=0). "
                + "Prevents hardware vendors from adding apps post-setup.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-pre-installed-apps",
            Label = "Disable Pre-Installed App Install via ContentDelivery",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["oobe", "preinstalled", "apps", "curation", "bloatware"],
            Description =
                "Disables automatic installation of curated pre-installed Windows apps "
                + "delivered via the Content Delivery Manager (PreInstalledAppsEnabled=0). "
                + "Reduces initial bloatware on clean installs.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "PreInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-soft-landing",
            Label = "Disable Start Layout Soft-Landing Suggestions",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "soft-landing", "layout"],
            Description =
                "Disables 'soft-landing' content — clickable tips and suggestions injected "
                + "into the Start menu and notification area after first login "
                + "(SoftLandingEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-start-system-pane-suggestions",
            Label = "Disable System Pane Suggestions in Start",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "system pane", "ui"],
            Description =
                "Disables the rotating suggested app tiles displayed in the Windows Start " + "menu system pane (SystemPaneSuggestionsEnabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-content-delivery",
            Label = "Disable Content Delivery Manager (All CDM Sources)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["oobe", "cdm", "content delivery", "suggestions", "apps"],
            Description =
                "Master switch that disables all Content Delivery Manager activity "
                + "by setting ContentDeliveryAllowed=0. Prevents all app suggestions, "
                + "spotlight ads, and cloud-delivered content from being displayed.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "ContentDeliveryAllowed", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338388",
            Label = "Disable Lock Screen Spotlight (SubscribedContent-338388)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "windows", "ads"],
            Description =
                "Disables Windows Spotlight on the lock screen by setting "
                + "SubscribedContent-338388Enabled=0 in the user's Content Delivery "
                + "Manager keys. Falls back to static wallpaper.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338388Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-338389",
            Label = "Disable Lock Screen Spotlight Tips (SubscribedContent-338389)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "spotlight", "lock screen", "tips", "ads"],
            Description =
                "Disables the rotating lock screen tips/suggestions from Windows Spotlight "
                + "(SubscribedContent-338389Enabled=0). Stops Microsoft from delivering "
                + "promotional messages on the lock screen.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353694",
            Label = "Disable Start Menu App Suggestions (SubscribedContent-353694)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "start", "suggestions", "apps", "ads"],
            Description =
                "Disables the 'Occasionally show suggestions in Start' setting "
                + "(SubscribedContent-353694Enabled=0). Stops ad tiles appearing "
                + "in the Start menu recommendations.",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353694Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-subscribed-content-353696",
            Label = "Disable Timeline Suggested Content (SubscribedContent-353696)",
            Category = "Cloud Storage",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["oobe", "timeline", "suggestions", "content", "ads"],
            Description =
                "Disables cloud-delivered suggested activities in the Windows Timeline "
                + "and 'Recommended' section (SubscribedContent-353696Enabled=0).",
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-353696Enabled", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimized Content in Settings",
            Category = "Cloud Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["oobe", "cloud", "content", "settings", "policy"],
            Description =
                "Prevents Windows from showing cloud-optimized content — rotating "
                + "Microsoft-curated links and suggestions — inside the Settings app "
                + "(DisableCloudOptimizedContent=1).",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableCloudOptimizedContent", 1)],
        },
    ];
}

// === Merged from: OneDrive.cs ===


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
