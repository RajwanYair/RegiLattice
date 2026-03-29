// RegiLattice.Core — Tweaks/CloudFileSyncPolicy.cs
// Cloud File Sync Policy — Sprint 557.
// Configures Group Policy for cloud file sync clients (OneDrive, Work Folders,
// enterprise sync agents): sync throttling, conflict resolution, encrypted
// sync enforcement, and selective sync restrictions.
// Category: "Cloud File Sync Policy" | Slug: cfsync
// Registry: HKLM\SOFTWARE\Policies\Microsoft\OneDrive
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\WorkFolders

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudFileSyncPolicy
{
    private const string OdKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\OneDrive";

    private const string WfKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkFolders";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cfsync-require-sync-encryption",
                Label = "Cloud File Sync: Require Encryption for Synced Files",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets RequireEncryption=1 in WorkFolders policy. Requires that all files managed by Windows Work Folders are stored in an encrypted state on the device's local sync cache. When encryption is required, Work Folders integrates with BitLocker or EFS to ensure the local copy of synced files is encrypted at rest. A device that loses BitLocker protection (TPM not present, BitLocker disabled) cannot sync Work Folders files. Ensures cloud-synced corporate files remain protected even if the device storage is physically accessed.",
                Tags = ["file-sync", "encryption", "work-folders", "data-protection", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Synced files require local encryption. Work Folders clients on unencrypted devices cannot sync. Verify BitLocker or EFS is deployed before enabling — sync stops on non-compliant devices.",
                ApplyOps = [RegOp.SetDword(WfKey, "RequireEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(WfKey, "RequireEncryption")],
                DetectOps = [RegOp.CheckDword(WfKey, "RequireEncryption", 1)],
            },
            new TweakDef
            {
                Id = "cfsync-disable-onedrive-personal",
                Label = "Cloud File Sync: Disable Personal OneDrive Account Sync",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets DisablePersonalSync=1 in OneDrive policy. Prevents users from signing in to their personal (non-Microsoft 365 commercial) OneDrive accounts through the OneDrive sync client. This is a data loss prevention control: without this restriction, users can drag corporate files from their SharePoint-connected OneDrive work library into their personal OneDrive folder and sync them to personal cloud storage that has no corporate DLP controls. Only Microsoft 365 work/school accounts are permitted.",
                Tags = ["onedrive", "personal-sync", "dlp", "data-loss", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Personal OneDrive sync is blocked. Users can only sync OneDrive for Business accounts. Files cannot be moved from work OneDrive to personal OneDrive via the sync client.",
                ApplyOps = [RegOp.SetDword(OdKey, "DisablePersonalSync", 1)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "DisablePersonalSync")],
                DetectOps = [RegOp.CheckDword(OdKey, "DisablePersonalSync", 1)],
            },
            new TweakDef
            {
                Id = "cfsync-enable-known-folder-move",
                Label = "Cloud File Sync: Enable Known Folder Move to OneDrive for Business",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets KFMSilentOptIn=<TenantID> equivalent as a policy flag via EnableKnownFolderMove=1. Enables silent migration of Desktop, Documents, and Pictures from local storage to the user's OneDrive for Business account. Files in Windows known folders are moved to OneDrive and folder redirection is updated automatically without prompting the user. This provides cloud backup for user data on all managed devices without requiring users to manually configure OneDrive — the most common cause of data loss is users who never configured backup.",
                Tags = ["onedrive", "known-folder-move", "backup", "enterprise", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Desktop, Documents, Pictures are silently moved to OneDrive for Business. Requires M365 licences with OneDrive for Business. Users see their folders unchanged but data is now synced to cloud.",
                ApplyOps = [RegOp.SetDword(OdKey, "EnableKnownFolderMove", 1)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "EnableKnownFolderMove")],
                DetectOps = [RegOp.CheckDword(OdKey, "EnableKnownFolderMove", 1)],
            },
            new TweakDef
            {
                Id = "cfsync-set-upload-bandwidth-limit",
                Label = "Cloud File Sync: Set OneDrive Upload Bandwidth Limit to 50%",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets UploadBandwidthLimit=50 in OneDrive policy. Caps the OneDrive sync client's upload bandwidth to 50% of the detected available bandwidth. Without a cap, OneDrive can saturate the uplink during large initial sync operations (e.g., after Known Folder Move, or when a new large document is added), degrading performance for all other network-dependent applications and services. The percentage-based cap is adaptive: on a fast connection, OneDrive uses 50% of a large bandwidth allocation; on a slow connection, it is proportionally throttled.",
                Tags = ["onedrive", "bandwidth", "throttle", "network", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OneDrive uploads are throttled to 50% of available bandwidth. Initial sync after KFM or large library additions takes longer. Network performance for other applications is preserved.",
                ApplyOps = [RegOp.SetDword(OdKey, "UploadBandwidthLimit", 50)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "UploadBandwidthLimit")],
                DetectOps = [RegOp.CheckDword(OdKey, "UploadBandwidthLimit", 50)],
            },
            new TweakDef
            {
                Id = "cfsync-disable-wf-auto-setup",
                Label = "Cloud File Sync: Disable Automatic Work Folders Setup",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets AutoSetup=0 in WorkFolders policy. Prevents Windows from automatically configuring Work Folders when a user signs in to a domain with an SRV record for Work Folders discovery. Automatic Work Folders setup creates local sync directories and begins syncing corporate content without user awareness. In environments that have migrated to OneDrive for Business, phantom Work Folders sync clients create duplicate data paths and storage overhead. Disabling auto-setup ensures Work Folders is only provisioned by explicit IT configuration.",
                Tags = ["work-folders", "auto-setup", "enterprise", "sync", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Work Folders does not auto-configure on domain join. Work Folders must be configured explicitly by IT. Prevents unintended dual-sync (Work Folders + OneDrive) on migrated environments.",
                ApplyOps = [RegOp.SetDword(WfKey, "AutoSetup", 0)],
                RemoveOps = [RegOp.DeleteValue(WfKey, "AutoSetup")],
                DetectOps = [RegOp.CheckDword(WfKey, "AutoSetup", 0)],
            },
            new TweakDef
            {
                Id = "cfsync-enable-files-on-demand",
                Label = "Cloud File Sync: Enable OneDrive Files On-Demand",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets FilesOnDemandEnabled=1 in OneDrive policy. Enables Files On-Demand for all OneDrive for Business sync clients: files appear in Explorer with placeholder icons but are not downloaded until accessed. Only files the user explicitly opens or marks for offline use occupy local disk space. For large OneDrive libraries (25 GB+), Files On-Demand prevents disk exhaustion: without it, enabling KFM on a device with a 128 GB boot drive and a 50 GB OneDrive library fills the drive. Required for Known Folder Move in large environments.",
                Tags = ["onedrive", "files-on-demand", "disk-space", "sync", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "OneDrive files are placeholders until opened. Files are downloaded on access. Offline access requires explicit marking. Essential for large OneDrive libraries on limited-storage devices.",
                ApplyOps = [RegOp.SetDword(OdKey, "FilesOnDemandEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "FilesOnDemandEnabled")],
                DetectOps = [RegOp.CheckDword(OdKey, "FilesOnDemandEnabled", 1)],
            },
            new TweakDef
            {
                Id = "cfsync-disable-onedrive-auto-start",
                Label = "Cloud File Sync: Disable OneDrive from Starting Automatically",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets Enabled=0 in OneDrive policy's auto-start key. Prevents the OneDrive sync client from starting automatically at user logon. In environments where OneDrive is not provisioned as the corporate sync solution (e.g., Work Folders or third-party DMS is used instead), having the OneDrive client start in every user session wastes resources and prompts users to configure personal accounts. When OneDrive deployment is managed through Intune or dedicated onboarding workflows, auto-start is unnecessary.",
                Tags = ["onedrive", "auto-start", "startup", "resource", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OneDrive does not start at logon. Users must launch OneDrive manually or it is deployed with auto-start via Intune. No impact on Work Folders or other sync clients.",
                ApplyOps = [RegOp.SetDword(OdKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(OdKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "cfsync-block-sync-to-unmanaged-domains",
                Label = "Cloud File Sync: Block OneDrive Sync to Unmanaged Azure AD Tenants",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets TenantRestriction=1 in OneDrive policy. Restricts OneDrive sync client connections to only the organisation's Azure AD tenant. Users cannot sync SharePoint data from external tenants or guest accounts that reside in other organisations' Azure AD tenants. This is a data exfiltration prevention control: an employee who has been invited as a guest to an external organisation's Azure AD can otherwise use the OneDrive sync client to download the external organisation's SharePoint data to the corporate machine.",
                Tags = ["onedrive", "tenant-restriction", "data-exfiltration", "guest", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "OneDrive sync is restricted to the organisation's Azure AD tenant. Employees who are guests in external Azure AD tenants cannot sync external SharePoint data. B2B collaboration via browser-based SharePoint is unaffected.",
                ApplyOps = [RegOp.SetDword(OdKey, "TenantRestriction", 1)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "TenantRestriction")],
                DetectOps = [RegOp.CheckDword(OdKey, "TenantRestriction", 1)],
            },
            new TweakDef
            {
                Id = "cfsync-enable-silent-account-config",
                Label = "Cloud File Sync: Enable Silent OneDrive Account Configuration",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets SilentAccountConfig=1 in OneDrive policy. Allows OneDrive to configure itself silently using the signed-in user's Azure AD credentials when the user logs into a Hybrid Azure AD Joined or Azure AD Joined device. Without silent configuration, users are prompted to manually set up OneDrive by entering their email address and signing in. With silent configuration, OneDrive picks up the user's Microsoft 365 identity from the device's AAD join state and configures sync automatically — essential for seamless onboarding at scale.",
                Tags = ["onedrive", "silent-config", "aad", "enterprise", "onboarding"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "OneDrive configures automatically on Azure AD Joined / Hybrid AAD Joined devices. Users see OneDrive already configured at first logon. Requires Azure AD Join or Hybrid Azure AD Join.",
                ApplyOps = [RegOp.SetDword(OdKey, "SilentAccountConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(OdKey, "SilentAccountConfig")],
                DetectOps = [RegOp.CheckDword(OdKey, "SilentAccountConfig", 1)],
            },
            new TweakDef
            {
                Id = "cfsync-require-lock-on-wf-idle",
                Label = "Cloud File Sync: Require Device Lock When Work Folders in Use",
                Category = "Cloud File Sync Policy",
                Description =
                    "Sets LockDriveOnIdle=1 in WorkFolders policy. Configures Work Folders to require device screen lock after the device idle timeout when Work Folders are configured. An unlocked device with Work Folders gives an unattended attacker access to synced corporate files without authentication. This policy enforces the screen lock policy as a prerequisite for Work Folders access: if the device screen lock is not configured (no timeout, no PIN on lock), Work Folders displays a warning and may suspend sync until lock is enabled.",
                Tags = ["work-folders", "screen-lock", "security", "idle", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Screen lock is required when Work Folders are configured. Devices without screen lock display a compliance warning. Does not forcibly lock the screen — it enforces existing screen lock policy configuration.",
                ApplyOps = [RegOp.SetDword(WfKey, "LockDriveOnIdle", 1)],
                RemoveOps = [RegOp.DeleteValue(WfKey, "LockDriveOnIdle")],
                DetectOps = [RegOp.CheckDword(WfKey, "LockDriveOnIdle", 1)],
            },
        ];
}
