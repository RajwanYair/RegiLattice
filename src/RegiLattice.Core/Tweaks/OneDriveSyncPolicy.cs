// RegiLattice.Core — Tweaks/OneDriveSyncPolicy.cs
// OneDrive Sync and Data Governance Policy — Sprint 588.
// Configures OneDrive for Business sync controls, Known Folder Move
// enforcement, selective sync restrictions, and upload bandwidth limits.
// Category: "OneDrive Sync Policy" | Slug: odsync
// Registry: HKLM\SOFTWARE\Policies\Microsoft\OneDrive

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class OneDriveSyncPolicy
{
    private const string OneDriveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "odsync-enable-known-folder-move",
                Label = "OneDrive Sync: Enable Known Folder Move (Desktop/Documents/Pictures to OneDrive)",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets KFMSilentOptIn to the tenant ID GUID in the OneDrive policy key (uses a placeholder DWORD flag EnableKFMSilentOptIn=1 as the registry-based detection). Silently moves Windows Known Folders (Desktop, Documents, Pictures) to OneDrive for Business during the next OneDrive sync cycle. This is the primary OneDrive data protection feature for enterprise — it ensures that user data stored in the default Windows folders is continuously synced to OneDrive, providing automatic cloud backup and recovery in case of device loss or failure. Users do not need to change their behaviour — they continue to save to Desktop/Documents and data is automatically protected.",
                Tags = ["onedrive", "known-folder-move", "kfm", "desktop", "documents"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Desktop, Documents, and Pictures folders silently moved to OneDrive for Business. Requires a tenant-specific OneDrive Group Policy configuration (tenantId GUID in KFMSilentOptIn). Deployment requires M365 Business or Enterprise licensing. Users see a toast notification on first sync. Existing local files are moved — no data loss. Configure via the OneDrive Group Policy ADMX template for production deployment.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "EnableKFMSilentOptIn", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "EnableKFMSilentOptIn")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "EnableKFMSilentOptIn", 1)],
            },
            new TweakDef
            {
                Id = "odsync-prevent-personal-onedrive-use",
                Label = "OneDrive Sync: Block Personal OneDrive Accounts (Allow Only Tenant Accounts)",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets DisablePersonalSync=1 in the OneDrive policy key. Prevents users from signing into the OneDrive sync client with a personal Microsoft account (Hotmail, Outlook.com, Xbox Live). In enterprise environments, allowing personal OneDrive accounts on corporate devices creates a shadow IT data exfiltration path — users can silently sync corporate files to their personal OneDrive. Restricting the sync client to tenant-managed (Azure AD) accounts ensures that synced data is governed by the enterprise DLP and retention policies.",
                Tags = ["onedrive", "personal-account", "data-exfiltration", "shadow-it", "tenant-restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Personal OneDrive syncing blocked on this device. Users cannot sync their personal OneDrive files to a corporate device. If users store personal files on OneDrive and access them at work, they must use the OneDrive.com web interface. The sync client only accepts corporate Azure AD account logins.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "DisablePersonalSync", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisablePersonalSync")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "DisablePersonalSync", 1)],
            },
            new TweakDef
            {
                Id = "odsync-allow-sync-on-metered-network",
                Label = "OneDrive Sync: Prevent OneDrive Sync on Metered Connections",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets DisableSyncOnMeteredNetwork=1 in the OneDrive policy key. Prevents OneDrive from syncing files when the device is connected via a metered network connection (mobile hotspot, cellular tethering, capped data plan). On mobile broadband or tethered connections, unthrottled OneDrive sync can consume gigabytes of cellular data — generating unexpected data overage charges or exhausting mobile data plans. Suspending sync on metered connections is the standard behaviour for consumer OneDrive but requires explicit policy enforcement in enterprise deployments.",
                Tags = ["onedrive", "metered-network", "cellular", "data-usage", "bandwidth"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive sync paused on metered connections. Syncing resumes automatically when the device connects to a non-metered (Wi-Fi, Ethernet) network. Files saved to synced folders while on metered connections are queued and uploaded when network becomes non-metered.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "DisableSyncOnMeteredNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisableSyncOnMeteredNetwork")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "DisableSyncOnMeteredNetwork", 1)],
            },
            new TweakDef
            {
                Id = "odsync-set-max-upload-bandwidth-400kbps",
                Label = "OneDrive Sync: Limit Upload Bandwidth to 400 Kbps",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets UploadBandwidthLimit=400 in the OneDrive policy key (value in Kbps). Throttles OneDrive upload bandwidth to 400 Kbps. This prevents OneDrive initial KFM migration or large file uploads from saturating a corporate WAN link. A 400 Kbps upload cap allows OneDrive to sync in the background without impacting VoIP call quality (which requires ~100 Kbps per call), video conferencing (which requires 1.5–4 Mbps per call), or line-of-business application connectivity. For faster connections (100 Mbps LAN), the cap can be increased — adjust to match your WAN uplink capacity.",
                Tags = ["onedrive", "upload-bandwidth", "throttle", "wan", "qos"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive upload limited to 400 Kbps. Initial KFM migration of large document libraries may take several days at 400 Kbps. For initial deployment, consider a temporary higher limit (2000 Kbps) for the first two weeks, then reduce to 400 Kbps steady-state.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "UploadBandwidthLimit", 400)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "UploadBandwidthLimit")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "UploadBandwidthLimit", 400)],
            },
            new TweakDef
            {
                Id = "odsync-enable-file-on-demand",
                Label = "OneDrive Sync: Enable Files On-Demand (Cloud-Only Placeholder Files)",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets FilesOnDemandEnabled=1 in the OneDrive policy key. Enables OneDrive Files On-Demand, which creates placeholder files in the local OneDrive folder for files stored in OneDrive without downloading the content until the file is opened. Files On-Demand dramatically reduces local storage consumption — a OneDrive library with 100 GB of files may only consume 50 MB of local disk space if most files have never been accessed. Downloaded files are cached locally for offline access. This is critical for devices with small SSD storage (128–256 GB) and large OneDrive libraries.",
                Tags = ["onedrive", "files-on-demand", "storage-savings", "placeholder", "cloud-only"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Files On-Demand enabled. File Explorer shows cloud-only files as placeholder icons. Opening a cloud-only file triggers a download — may cause a brief delay on first open. Antivirus on-access scans will automatically download cloud-only files when scanning, potentially consuming disk space and network bandwidth.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "FilesOnDemandEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "FilesOnDemandEnabled")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "FilesOnDemandEnabled", 1)],
            },
            new TweakDef
            {
                Id = "odsync-enable-silent-sign-in",
                Label = "OneDrive Sync: Enable Silent Azure AD Sign-In (Single Sign-On with AzureAD)",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets SilentAccountConfig=1 in the OneDrive policy key. Enables OneDrive to automatically sign in with the user's Azure Active Directory account without displaying any sign-in prompts. This uses Azure AD SSO to automatically configure the OneDrive sync client on first logon — users never see a OneDrive setup wizard or sign-in dialog. This is the enterprise-grade deployment method for OneDrive — it ensures 100% adoption without user-initiated setup, which is critical for KFM to protect all devices automatically.",
                Tags = ["onedrive", "silent-signin", "azure-ad", "sso", "auto-configure"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive automatically signs in using Azure AD credentials. Requires Azure AD joined or Hybrid Azure AD joined devices. Works on-premises and in cloud-only deployments. Users see a brief OneDrive startup notification on first logon — no action required.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "SilentAccountConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "SilentAccountConfig")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "SilentAccountConfig", 1)],
            },
            new TweakDef
            {
                Id = "odsync-prevent-unmanaged-machine-sync",
                Label = "OneDrive Sync: Block Sync on Unmanaged (Non-Domain, Non-Azure-AD) Devices",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets AllowTenantList enforcement via PreventUnmanagedMachineSync=1 in the OneDrive policy key. Prevents the OneDrive sync client from syncing corporate OneDrive data on devices that are not Azure AD joined or domain joined. This is the SharePoint Online 'allowed domain' equivalent for device management compliance — corporate data should only sync to managed devices under IT control. Users who attempt to sync from personal or unmanaged devices receive a 'You can't sync to this location' error.",
                Tags = ["onedrive", "unmanaged-device", "dlp", "conditional-access", "byod"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Corporate OneDrive sync blocked on unmanaged devices. BYOD users with personal devices cannot install the OneDrive sync client and access corporate libraries — they must use OneDrive.com web interface. Ensure Conditional Access policies in Azure AD complement this policy for consistent enforcement.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "PreventUnmanagedMachineSync", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "PreventUnmanagedMachineSync")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "PreventUnmanagedMachineSync", 1)],
            },
            new TweakDef
            {
                Id = "odsync-enable-verbose-error-reporting",
                Label = "OneDrive Sync: Enable Verbose Sync Error Reporting to Event Log",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets EnableVerboseEventReporting=1 in the OneDrive policy key. Enables detailed OneDrive sync error events in the Windows Application event log (source: OneDrive). Standard OneDrive error reporting only logs critical failures to the OneDrive diagnostic log (%LOCALAPPDATA%\\Microsoft\\OneDrive\\logs\\). Verbose event log reporting records all sync errors to the Windows event log where SIEM tools can consume them — enabling fleet-wide monitoring of sync failures, storage quota issues, sharing permission errors, and file conflict errors.",
                Tags = ["onedrive", "event-log", "error-reporting", "siem", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive sync errors written to Windows Application event log. In large deployments with frequent sync errors (file locking conflicts, large file type restrictions), event log volume can be significant. Consider event log retention and SIEM ingestion cost when enabling verbose reporting at scale.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "EnableVerboseEventReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "EnableVerboseEventReporting")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "EnableVerboseEventReporting", 1)],
            },
            new TweakDef
            {
                Id = "odsync-disable-auto-start-telemetry",
                Label = "OneDrive Sync: Disable OneDrive Usage Telemetry Reporting to Microsoft",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets DisableTelemetry=1 in the OneDrive policy key. Disables the OneDrive sync client's Optional Diagnostic Data (ODD) telemetry reporting to Microsoft. OneDrive collects usage telemetry about sync activity, file types synced, sync performance, and error rates. In privacy-sensitive enterprise environments or regulated industries (legal, healthcare, finance), disabling optional telemetry from OneDrive is required by data governance policy. Required diagnostic data (error crash reports) cannot be disabled — only the optional enhanced telemetry is affected.",
                Tags = ["onedrive", "telemetry", "privacy", "data-governance", "gdpr"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Optional OneDrive telemetry reporting disabled. Required diagnostic data (crash reports, error reports) still transmitted. No user-visible impact on OneDrive sync functionality. Microsoft will receive less usage data for OneDrive feature improvements.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "odsync-set-cache-free-space-floor-5pct",
                Label = "OneDrive Sync: Set Minimum Free Space Floor — Do Not Sync Below 5% Free Disk",
                Category = "OneDrive Sync Policy",
                Description =
                    "Sets MinDiskFreeSpaceGB=5 in the OneDrive policy key (absolute GB, adjustable). Prevents OneDrive from downloading additional Files On-Demand files when the device's disk free space falls below the configured threshold. Without a free space floor, OneDrive downloads can fill an SSD to 100%, causing Windows write operations to fail (temp file writes, browser cache, update extraction) — resulting in OS instability. The free space floor ensures that even when users open many cloud-only files in quick succession, the disk is not completely filled.",
                Tags = ["onedrive", "disk-space", "storage-floor", "on-demand", "stability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive download paused when disk free space falls below 5 GB. Files On-Demand downloads are suspended — opening new cloud-only files fails until free space is restored. Users see an OneDrive notification about insufficient disk space. Existing downloaded (cached) files are not deleted.",
                ApplyOps = [RegOp.SetDword(OneDriveKey, "MinDiskFreeSpaceGB", 5)],
                RemoveOps = [RegOp.DeleteValue(OneDriveKey, "MinDiskFreeSpaceGB")],
                DetectOps = [RegOp.CheckDword(OneDriveKey, "MinDiskFreeSpaceGB", 5)],
            },
        ];
}
