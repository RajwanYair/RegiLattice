// RegiLattice.Core — Tweaks/SkyDrivePolicy.cs
// Legacy OneDrive/SkyDrive Synchronisation Policy — Sprint 629.
// Category: "SkyDrive Policy" | Slug: skydrive
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\SkyDrive

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SkyDrivePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SkyDrive";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "skydrive-disable-file-sync",
            Label = "SkyDrive: Disable OneDrive File Synchronisation via Legacy SkyDrive Policy Key",
            Category = "SkyDrive Policy",
            Description = "Sets DisableFileSync=1 in the SkyDrive legacy policy key. Disables OneDrive file synchronisation at the machine policy level, preventing all users on this computer from syncing files with their OneDrive cloud storage. The SkyDrive registry key is the original legacy path (Windows 8.1/RT era) that is still read by the current Windows OneDrive client for backwards compatibility with Group Policy deployed to WS2012R2 and Win 8.1 machines. " +
                "In organisations that prohibit users from uploading corporate files to personal cloud storage, the SkyDrive legacy policy key ensures policy coverage extends to legacy Windows versions where the OneDrive-specific policy path did not yet exist. The SkyDrive and OneDrive policy keys are both evaluated — having both set ensures no gap in policy enforcement across heterogeneous Windows version environments. Without both keys set, a corporate laptop running the current OneDrive client on Win 8.1 would check the SkyDrive key first; if missing, OneDrive sync proceeds unblocked.",
            Tags = ["skydrive", "onedrive", "file-sync", "cloud-storage", "policy", "disable"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "OneDrive file sync disabled via legacy SkyDrive policy key. Corporate files blocked from uploading to personal OneDrive accounts.",
            ApplyOps = [RegOp.SetDword(Key, "DisableFileSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFileSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFileSync", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-disable-library-default-save",
            Label = "SkyDrive: Prevent Libraries from Defaulting Save Location to SkyDrive/OneDrive",
            Category = "SkyDrive Policy",
            Description = "Sets DisableLibrariesDefaultSaveToSkyDrive=1 in the SkyDrive policy key. Prevents Windows from configuring OneDrive's local folder as the default save location for Windows Libraries (Documents, Pictures, Music). Without this policy, Windows 8.1+ suggests OneDrive as the default save target — any document saved without explicitly choosing a location is uploaded to the user's personal OneDrive account. " +
                "In corporate environments where DLP (Data Loss Prevention) policies prohibit saving corporate IP to personal cloud storage, the auto-save to OneDrive/SkyDrive default library path is a subtle leakage vector — users who click 'Save' without inspecting the save dialogue may unknowingly sync sensitive documents to personal storage. Enforcing a corporate-managed default save location (a file server or SharePoint UNC path configured by Group Policy Folder Redirection) ensures all undirected file saves stay within managed storage boundaries.",
            Tags = ["skydrive", "onedrive", "library", "default-save", "dlp", "cloud-storage"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Libraries no longer default to SkyDrive/OneDrive as save location. Users who manually navigate to OneDrive folder can still save there until sync is also disabled.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLibrariesDefaultSaveToSkyDrive", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLibrariesDefaultSaveToSkyDrive")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLibrariesDefaultSaveToSkyDrive", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-disable-metered-sync",
            Label = "SkyDrive: Disable OneDrive Sync on Metered Network Connections",
            Category = "SkyDrive Policy",
            Description = "Sets NeverSyncOnMeteredConnection=1 in the SkyDrive policy key. Prevents OneDrive from synchronising files when the active network connection is metered (mobile data, LTE hotspot, satellite). Without this policy, OneDrive will attempt background synchronisation on metered connections, consuming potentially expensive cellular data allowances and degrading application performance for users on mobile hotspots. " +
                "Windows marks mobile hotspot connections, tethered cellular connections, and some Wi-Fi networks as metered to signal to applications that data usage should be minimised. OneDrive respects the metered status for foreground sync but continues background sync by default. For road warriors using laptop hotspot tethering on international trips with expensive roaming data plans, an unconstrained OneDrive background sync can silently consume gigabytes of mobile data. Disabling sync on metered connections prevents this scenario without requiring manual sync suspension.",
            Tags = ["skydrive", "onedrive", "metered-connection", "mobile-data", "bandwidth", "roaming"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "OneDrive sync paused on metered connections (cellular hotspot, LTE). Manual sync is still available. Files upload when non-metered connection is available.",
            ApplyOps = [RegOp.SetDword(Key, "NeverSyncOnMeteredConnection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NeverSyncOnMeteredConnection")],
            DetectOps = [RegOp.CheckDword(Key, "NeverSyncOnMeteredConnection", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-disable-desktop-shortcut",
            Label = "SkyDrive: Disable Automatic OneDrive Desktop Shortcut Creation",
            Category = "SkyDrive Policy",
            Description = "Sets DisableSkyDriveDesktopIcon=1 in the SkyDrive policy key. Prevents OneDrive from adding a shortcut icon to the user's desktop during initial setup or after updates. On managed enterprise desktops where the shortcut layout is standardised by Group Policy (no unmanaged shortcuts on desktop), automatic OneDrive shortcut creation violates desktop policy and confuses users who may not be aware of cloud sync being installed. " +
                "Desktop shortcut proliferation on managed endpoints is a minor but persistent administrative annoyance. Each major OneDrive update can re-create the desktop shortcut if it was manually deleted, causing the shortcut to reappear after each update. Policy-driven suppression ensures the shortcut is never created, remaining consistent across updates without requiring GPO-applied shortcut deletion scripts.",
            Tags = ["skydrive", "onedrive", "desktop-shortcut", "icon", "managed-desktop"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "OneDrive desktop icon not created. Users can still access OneDrive via the system tray icon or File Explorer navigation.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSkyDriveDesktopIcon", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSkyDriveDesktopIcon")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSkyDriveDesktopIcon", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-prevent-usage-of-onedrive",
            Label = "SkyDrive: Prevent All OneDrive Usage via Legacy SkyDrive Machine Policy",
            Category = "SkyDrive Policy",
            Description = "Sets PreventNetworkTrafficPreUserSignIn=1 in the SkyDrive policy key. Prevents OneDrive from generating any network traffic before the user signs in. During the Windows startup sequence, OneDrive pre-caches metadata and checks for updates before user login completes. This pre-sign-in network activity consumes bandwidth, adds to boot time, and generates outbound connections from a system in an unauthenticated state — which some network security monitoring tools flag as suspicious. " +
                "Pre-authentication network connections from Microsoft services are a known privacy concern: OneDrive network activity during boot can leak the device's presence, IP address, and tenant association to Microsoft servers before the user has consented to connected services for that session. In high-security environments that enforce a zero-trust model where no application should generate network traffic until after full user authentication, pre-sign-in OneDrive connections violate this control. Blocking pre-sign-in network activity ensures OneDrive only connects after a user is fully authenticated.",
            Tags = ["skydrive", "onedrive", "pre-signin", "network-traffic", "zero-trust", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "OneDrive pre-login network activity blocked. No user-visible impact. OneDrive connects normally after user authentication completes.",
            ApplyOps = [RegOp.SetDword(Key, "PreventNetworkTrafficPreUserSignIn", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventNetworkTrafficPreUserSignIn")],
            DetectOps = [RegOp.CheckDword(Key, "PreventNetworkTrafficPreUserSignIn", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-disable-personal-sync",
            Label = "SkyDrive: Block Sync of Personal Accounts on Domain-Joined Machines",
            Category = "SkyDrive Policy",
            Description = "Sets DisablePersonalSync=1 in the SkyDrive policy key. Prevents users from adding and syncing personal (non-corporate) Microsoft accounts with OneDrive on domain-joined or Entra ID-joined machines. Allows corporate OneDrive for Business (Entra ID accounts) to function normally while blocking personal @hotmail.com, @outlook.com, and @gmail.com accounts from syncing. " +
                "On corporate endpoints, personal OneDrive accounts present a data exfiltration risk: a user can drag corporate documents into their personal OneDrive sync folder and those files are immediately uploaded to their personal account, bypassing corporate DLP policies that only monitor corporate OneDrive tenants. The DisablePersonalSync policy removes the option to add personal accounts from the OneDrive settings UI while allowing the corporate account configuration to proceed normally — enabling corporate OneDrive features while blocking personal sync.",
            Tags = ["skydrive", "onedrive", "personal-account", "corporate-policy", "dlp", "exfiltration"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Personal Microsoft account OneDrive sync blocked. Corporate OneDrive for Business accounts unaffected. Requires Entra ID-joined device for corporate sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePersonalSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePersonalSync", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-require-domain-joined-to-sync",
            Label = "SkyDrive: Require Domain Membership Before Allowing OneDrive Sync",
            Category = "SkyDrive Policy",
            Description = "Sets RequireAccountFolderLocation=1 in the SkyDrive policy key. Requires that the user's OneDrive folder location is within a domain-accessible path before synchronisation begins. This ensures users cannot configure OneDrive to sync to a USB drive, external HDD, or a path on a non-domain-joined volume, which would bypass file auditing and DLP policies that monitor domain-accessible file paths. " +
                "OneDrive's default folder location is %USERPROFILE%\\OneDrive — on a domain-joined machine this is within the user profile path which may be redirected to a file server. If a user changes the OneDrive local folder to an external USB drive, sync continues to the external drive but audit policies monitoring the user profile path no longer capture OneDrive file activities. By requiring the account folder to be in an approved location, this policy prevents sync rerouting to unmonitored storage media.",
            Tags = ["skydrive", "onedrive", "folder-location", "domain", "audit", "data-governance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "OneDrive sync folder must be on a monitored domain-accessible path. Users cannot redirect sync to USB drives or external storage.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAccountFolderLocation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAccountFolderLocation")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAccountFolderLocation", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-disable-tutorialicon",
            Label = "SkyDrive: Suppress OneDrive First-Run Tutorial and Balloon Notifications",
            Category = "SkyDrive Policy",
            Description = "Sets DisableTutorial=1 in the SkyDrive policy key. Suppresses the OneDrive first-run tutorial wizard and taskbar balloon notification tooltips that appear on first login or after updates. On enterprise-deployed endpoints, the OneDrive tutorial interrupts user productivity during logins, and repetitive balloon tooltips post-update create distraction and support desk calls from users who assume the notifications indicate a problem. " +
                "First-run wizard suppression is a routine enterprise deployment cleanliness policy — the tutorial is designed for retail consumers who have never configured OneDrive. In corporate environments where OneDrive policy is centrally managed (folder protection, retention policies, tenant binding), the tutorial presents options the user cannot change (they are set by policy) and provides misleading information about sync customisation capabilities. Suppressing the tutorial ensures users see only the relevant corporate-configured sync state without conflicting consumer-oriented guidance.",
            Tags = ["skydrive", "onedrive", "tutorial", "notification", "enterprise-deployment"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "OneDrive first-run tutorial and balloon tips suppressed. No functional impact — OneDrive operates normally with tutorial hidden.",
            ApplyOps = [RegOp.SetDword(Key, "DisableTutorial", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTutorial")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTutorial", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-block-known-folder-move",
            Label = "SkyDrive: Block Known Folder Move to Prevent Forced Desktop/Documents Redirect to OneDrive",
            Category = "SkyDrive Policy",
            Description = "Sets KFMBlockOptIn=1 in the SkyDrive policy key. Blocks OneDrive's Known Folder Move (KFM) feature from prompting users or automatically moving the Windows Known Folders (Desktop, Documents, Pictures) from their local profile path to the OneDrive folder. KFM can be deployed silently by IT to redirect these folders to OneDrive cloud storage — but without advance user notification, users may be surprised to find their desktop files suddenly synchronised to the cloud. " +
                "Known Folder Move can have significant consequences when deployed without proper planning: large local Desktop and Documents folders (100+ GB) begin uploading to OneDrive immediately, consuming bandwidth. Folders that contain sensitive data subject to GDPR or HIPAA retention policies may inadvertently be moved to a Microsoft-operated cloud service without completing required Data Processing Agreement reviews. By blocking KFM opt-in via this policy, organisations can plan and deploy folder redirection deliberately rather than having it trigger based on defaults.",
            Tags = ["skydrive", "onedrive", "known-folder-move", "kfm", "desktop-redirect", "cloud-redirect"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "OneDrive Known Folder Move blocked. Desktop, Documents, Pictures remain in local profile. IT-managed folder redirection to file server is unaffected.",
            ApplyOps = [RegOp.SetDword(Key, "KFMBlockOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "KFMBlockOptIn")],
            DetectOps = [RegOp.CheckDword(Key, "KFMBlockOptIn", 1)],
        },
        new TweakDef
        {
            Id = "skydrive-disable-teamsync",
            Label = "SkyDrive: Disable OneDrive SharePoint-Backed Team Site Sync",
            Category = "SkyDrive Policy",
            Description = "Sets DisableSharepointSync=1 in the SkyDrive policy key. Prevents OneDrive from synchronising SharePoint Online-backed team site document libraries to the local machine. SharePoint team site sync makes the full content of shared team library folders available for offline editing — potentially storing large volumes of multi-user shared data locally on a laptop endpoint. " +
                "SharePoint team site sync on secure endpoints creates data sovereignty risk: when a full team document library (containing files created by all team members) is synced locally, those files are stored in an endpoint protected only by the laptop's local encryption. If the laptop is stolen or compromised, all team documents are accessible to the attacker — not just the individual user's Documents but the entire team library. Disabling SharePoint sync ensures team content remains in the cloud and is only accessible via the browser with valid MFA credentials, not from the local disk.",
            Tags = ["skydrive", "onedrive", "sharepoint", "team-site", "offline-sync", "data-sovereignty"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "SharePoint Online team site document library sync to local machine disabled. Team files accessed via browser/SharePoint only. Personal OneDrive sync unaffected if DisableFileSync not set.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSharepointSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSharepointSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSharepointSync", 1)],
        },
    ];
}
