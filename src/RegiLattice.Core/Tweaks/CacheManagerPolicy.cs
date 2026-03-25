// RegiLattice.Core — Tweaks/CacheManagerPolicy.cs
// Sprint 323: Cache Manager Policy tweaks (10 tweaks)
// Category: "Cache Manager Policy" | Slug: cachemgr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CacheManager

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CacheManagerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CacheManager";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cachemgr-disable-offline-cache",
            Label = "Disable Offline Files Caching (Client-Side Caching)",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Client-side caching stores network file share contents locally for offline access which creates copies of potentially sensitive corporate data. Disabling offline caching prevents synchronized copies of network files from being stored on endpoint local disks. Cached files can persist after employees leave the organization or on endpoints that may be lost or stolen. Offline caching can store sensitive documents outside of DLP controls and monitoring systems. Large offline caches can consume significant disk space and may contain data that would otherwise remain on protected file servers. Organizations with DLP requirements for data-at-rest should evaluate whether offline caching is consistent with their data handling policies.",
            Tags = ["cache", "offline-files", "csc", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOfflineCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOfflineCache")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOfflineCache", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-encrypt-offline-files",
            Label = "Encrypt Offline Files Cache",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Encrypting the offline files cache protects locally stored copies of network files if the endpoint disk is accessed without proper authentication. Offline file cache encryption uses EFS or BitLocker protection to ensure that cached sensitive files are not accessible if the disk is removed. Unencrypted offline caches of sensitive documents can be accessed by attackers who gain physical access or offline disk access. Cache encryption ensures that even if an endpoint is lost or stolen the offline file cache cannot be read without the correct user credentials. Encrypting offline caches complements full-disk encryption by providing file-level protection within the cache directory. Organizations deploying BitLocker should also encrypt offline caches to prevent data exposure through cache files during pre-boot states.",
            Tags = ["cache", "offline-files", "encryption", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EncryptOfflineFilesCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EncryptOfflineFilesCache")],
            DetectOps = [RegOp.CheckDword(Key, "EncryptOfflineFilesCache", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-limit-cache-size",
            Label = "Restrict Offline Files Cache Size",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Limiting the offline files cache size prevents excessive accumulation of cached network files that consume disk space and expand the attack surface. A restricted cache size ensures that only the most recently accessed network files are retained in the local cache. Unlimited caches can consume gigabytes of disk space on endpoints with large offline file synchronization policies. Cache size limits encourage users to rely on network access rather than local copies which reduces data leakage risk. Monitoring cache utilization against defined limits helps identify endpoints with unusual offline file activity indicating policy violations. Organizations should set cache size limits based on endpoint disk capacity and typical offline file usage requirements.",
            Tags = ["cache", "offline-files", "disk-space", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LimitCacheSize", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitCacheSize")],
            DetectOps = [RegOp.CheckDword(Key, "LimitCacheSize", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-disable-transparent-cache",
            Label = "Disable Transparent Caching of Remote Files",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Transparent caching improves performance for frequently accessed network files by storing them locally without explicit offline synchronization. Disabling transparent caching prevents implicit local copies of network files from being created on endpoint storage. Transparent caching can create unintended local copies of sensitive files accessed from network shares without user awareness. Files in the transparent cache bypass DLP policies that monitor network file access and apply to only the network path. Transparent caching was designed for WAN optimization but creates data residency concerns when sensitive files should remain on controlled file servers. Disabling transparent caching ensures that all access to network files goes through the network share with proper access controls and monitoring.",
            Tags = ["cache", "transparent-cache", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTransparentCaching", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTransparentCaching")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTransparentCaching", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-disable-browser-cache-sharing",
            Label = "Disable IE/Edge Browser Cache External Sharing",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Browser cache sharing with external applications can allow third-party tools to access cached browser content including session tokens and cookies. Disabling external cache sharing prevents applications other than the browser from reading the browser's local cache. Browser caches can contain sensitive information including recently accessed URLs, form data, cached credentials, and session tokens. External access to browser cache files can be exploited by malicious applications to harvest credentials without process injection. Browser cache isolation is part of overall browser security hardening alongside profile separation and sandbox controls. This policy restricts which processes can access browser cache directories through Windows filesystem permissions and cache API controls.",
            Tags = ["cache", "browser", "cookies", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBrowserCacheExternalSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBrowserCacheExternalSharing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBrowserCacheExternalSharing", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-clear-cache-on-logoff",
            Label = "Clear Offline Files Cache on User Logoff",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Clearing the offline files cache on user logoff removes locally cached network file copies when the user session ends. Cache clearing on logoff ensures that sensitive cached files do not persist on shared or temporary workstations between user sessions. Kiosk endpoints and shared workstations commonly cache credentials and files from previous users when sessions are improperly handled. Logoff cache clearing combined with user profile deletion provides clean slate isolation between user sessions. Clearing caches on logoff may increase network load as files need to be re-synchronized on next logon but ensures fresh data access. This policy is most important for high-turnover environments like call centers, shared workstations, and terminal services environments.",
            Tags = ["cache", "logoff", "cleanup", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ClearCacheOnLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClearCacheOnLogoff")],
            DetectOps = [RegOp.CheckDword(Key, "ClearCacheOnLogoff", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-disable-pin-for-readonly",
            Label = "Disable Pinning of Read-Only Files for Offline",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Pinning read-only network files for offline access creates persistent local copies of files that may contain sensitive information. Disabling read-only file pinning prevents users from creating offline copies of files marked as accessible to their accounts. Read-only access to sensitive documents should remain controlled at the file server level through access controls not through offline copies. Pinned offline files create a persistent local copy that survives beyond the period when the user's access should be valid. Restricting pinning permissions reduces the risk of data exposure from offline copies on endpoints that are later lost or repurposed. Users requiring offline access to specific documents should follow formal data handling procedures with approved encryption and controls.",
            Tags = ["cache", "pinning", "offline-files", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePinForReadOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePinForReadOnly")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePinForReadOnly", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-require-cache-encryption-before-sync",
            Label = "Require Cache Encryption Before Offline Sync",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Requiring cache encryption before offline synchronization ensures that the local cache directory is protected before files are synchronized into it. Pre-sync encryption requirement prevents offline file synchronization on endpoints that do not have the cache encryption configured and verified. This policy prevents unencrypted offline caches from being created on endpoints that lack proper encryption key setup. Ensuring cache encryption is enforced before sync prevents a window of exposure between cache creation and encryption activation. The policy provides a mandatory checkpoint that blocks offline file caching on non-compliant endpoints until encryption is confirmed. Organizations with mandatory encryption policies benefit from this pre-sync encryption check as a technical enforcement mechanism.",
            Tags = ["cache", "encryption", "sync", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireEncryptionBeforeSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireEncryptionBeforeSync")],
            DetectOps = [RegOp.CheckDword(Key, "RequireEncryptionBeforeSync", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-disable-admin-pin",
            Label = "Disable Administrator-Pinned Offline Files",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Administrator-pinned files allow IT administrators to force files to be available offline for all users of a computer through Group Policy. Disabling administrator-pinned offline files prevents IT policies from pushing potentially large or sensitive file sets to local endpoint caches. Admin-pinned files can be used to distribute configuration files but also create local copies of sensitive data outside user control. Inadvertent admin pinning of large directory trees can consume significant disk space on managed endpoints. Organizations should review admin-pinned file policies to ensure only appropriate files are being forced offline and disable bulk pinning. Removing admin-pinned file policies reduces the chance of unintended data accumulation on endpoint local storage.",
            Tags = ["cache", "admin-pinning", "offline-files", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAdminPinning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAdminPinning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAdminPinning", 1)],
        },
        new TweakDef
        {
            Id = "cachemgr-audit-offline-access",
            Label = "Enable Offline Files Access Audit Logging",
            Category = "Cache Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Offline files access audit logging records when users access cached files providing visibility into offline data access patterns. Enabling offline cache access auditing generates security events for cache file reads and writes during sessions when network connectivity is unavailable. Audit records of offline access help identify access to sensitive cached files from endpoints in unexpected locations. Offline access auditing supports DLP investigations by providing evidence of which files were accessed during disconnected periods. Security auditing of offline files should be synchronized with the audit data collected on the corresponding file servers. Offline access audit events should be forwarded to SIEM infrastructure when the endpoint reconnects to the network.",
            Tags = ["cache", "audit", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditOfflineFileAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditOfflineFileAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AuditOfflineFileAccess", 1)],
        },
    ];
}
