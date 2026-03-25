// RegiLattice.Core — Tweaks/FileSharePolicy.cs
// Sprint 326: File Share Policy tweaks (10 tweaks)
// Category: "File Share Policy" | Slug: filshare
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FileSharePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "filshare-disable-auto-share-wks",
            Label = "Disable Automatic Administrative Workstation Shares",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Automatic administrative shares (C$ D$ ADMIN$ IPC$) are created by Windows Server automatically and provide administrative remote access. Disabling automatic workstation shares prevents these implicit access points from being created without explicit administrator action. Automatic administrative shares are used by attackers for lateral movement, remote file copying, and tools like PsExec and WMI. Many enterprise environments disable automatic shares on workstations where remote administration is handled through dedicated management solutions. Disabling automatic shares reduces the attack surface without impacting endpoint functionality for standard users. Remote management tools should be configured to use alternative mechanisms that do not require default administrative shares.",
            Tags = ["file-share", "admin-shares", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(Key, "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "filshare-disable-auto-share-server",
            Label = "Disable Automatic Administrative Server Shares",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Server administrative shares are automatically created by the LanmanServer service providing full disk access to administrator accounts over the network. Disabling automatic server shares prevents implicit remote access paths that can be exploited if administrator credentials are compromised. Server shares are commonly required for legitimate file server operations but should be explicitly created rather than automatically managed. Explicit administrative share creation provides better visibility into which shares exist and who has access to each. Some management tools including legacy backup software may depend on automatic administrative shares for file system access. Organizations should audit management tool dependencies before disabling automatic server shares to avoid breaking critical operations.",
            Tags = ["file-share", "server-shares", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutoShareServer", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoShareServer")],
            DetectOps = [RegOp.CheckDword(Key, "AutoShareServer", 0)],
        },
        new TweakDef
        {
            Id = "filshare-require-secure-dialect",
            Label = "Set Minimum SMB Server Dialect",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Setting a minimum SMB dialect version for the server component prevents clients from negotiating to use older vulnerable protocol versions. A minimum dialect requirement of SMB 2.0.2 or higher prevents connections from SMB1-only clients that are incompatible with security features. File servers supporting only modern SMB dialects eliminate exposure to SMB1 vulnerabilities like EternalBlue and related exploits. Minimum dialect enforcement may affect legacy clients and network appliances that only speak older SMB versions. Enterprise environments should inventory SMB client versions before enforcing minimum dialect requirements to avoid connectivity disruption. Graceful enforcement with monitoring and logging before hard blocking provides visibility into legacy client dependencies.",
            Tags = ["file-share", "dialect", "smb", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireSecureDialect", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureDialect")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSecureDialect", 1)],
        },
        new TweakDef
        {
            Id = "filshare-enable-server-signing",
            Label = "Require SMB Signing on Server",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "SMB signing on the server component ensures that all incoming SMB connections are packet-signed preventing relay and tampering attacks. Requiring SMB signing eliminates the possibility of SMB relay attacks where an attacker captures and forwards an authenticated connection. SMB relay without signing is the foundation of many lateral movement techniques including NTLM relay and credential forwarding. File servers and domain controllers should always require SMB signing as they are the most valuable targets for relay attacks. Enabling SMB signing on all servers combined with client-side signing requirements creates a fully signed network protecting all SMB communications. Performance impact of SMB signing is minimal on modern hardware and should not be a reason to defer enablement.",
            Tags = ["file-share", "smb-signing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSecuritySignature", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSecuritySignature")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "filshare-set-max-concurrent-sessions",
            Label = "Limit Maximum Concurrent SMB Sessions",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Limiting concurrent SMB sessions prevents resource exhaustion attacks that could make file servers unavailable to legitimate users. Maximum session limits ensure that no single client or group of clients can monopolize file server connection resources. SMB session flooding is a simple denial of service attack that can be performed from within the network by malicious insiders or compromised endpoints. Session limits combined with connection rate throttling provide DoS protection for file sharing infrastructure. Overly low session limits may affect large file servers with many concurrent user connections and should be sized based on actual usage. Monitoring concurrent session counts against defined limits helps detect unusual connection patterns from potentially compromised systems.",
            Tags = ["file-share", "sessions", "dos-protection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxConcurrentConnections", 16384)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxConcurrentConnections")],
            DetectOps = [RegOp.CheckDword(Key, "MaxConcurrentConnections", 16384)],
        },
        new TweakDef
        {
            Id = "filshare-enable-server-encryption",
            Label = "Enable SMB Encryption on File Server",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Enabling SMB encryption on the server component protects all file transfer data from network interception by encrypting SMB traffic. Server-side encryption configuration ensures that all clients connecting to the server receive encrypted data regardless of client-side configuration. SMB encryption is available in SMB3 and provides AES-128-CCM or AES-128-GCM protection for all transferred data. Encrypted SMB prevents passive network capture from exposing file contents, metadata, and authentication data. File servers containing sensitive information should have encryption enabled even if clients are trusted to prevent interception at the network layer. Enabling encryption per-share for sensitive shares allows gradual deployment where only specific high-value shares require encryption.",
            Tags = ["file-share", "encryption", "smb", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EncryptData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EncryptData")],
            DetectOps = [RegOp.CheckDword(Key, "EncryptData", 1)],
        },
        new TweakDef
        {
            Id = "filshare-reject-unencrypted-access",
            Label = "Reject Unencrypted Client Connections",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Rejecting unencrypted client connections ensures that the file server refuses SMB connections from clients that do not support or use encryption. When combined with server encryption requirements, rejecting unencrypted connections enforces end-to-end encryption for all file server access. Clients running Windows 10 or Server 2016 and later all support SMB encryption but legacy clients may connect without encryption support. Rejecting unencrypted connections prevents a mixed-security scenario where some clients use encryption and others do not. Organizations must ensure all file server clients support SMB3 encryption before enforcing rejection of unencrypted connections. Monitoring SMB session negotiations before enforcement helps identify legacy clients that need to be updated or replaced.",
            Tags = ["file-share", "encryption", "reject-unencrypted", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RejectUnencryptedAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RejectUnencryptedAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RejectUnencryptedAccess", 1)],
        },
        new TweakDef
        {
            Id = "filshare-restrict-null-session-shares",
            Label = "Restrict Shares Accessible via Null Sessions",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Null session shares are accessible to unauthenticated network connections and provide an information disclosure path for network enumeration. Restricting null session shares prevents anonymous access to file shares through unauthenticated SMB connections. Null sessions allow remote enumeration of share names from which attackers can identify targets for authenticated access attempts. The NullSessionShares registry value lists which shares can be accessed without authentication and should contain no shares in secure configurations. Legacy applications that require null session access should be replaced with authenticated alternatives. Null session restriction is a fundamental network security control that should be enforced on all Windows systems accessible from the network.",
            Tags = ["file-share", "null-session", "anonymous", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictNullSessionShares", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictNullSessionShares")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictNullSessionShares", 1)],
        },
        new TweakDef
        {
            Id = "filshare-log-unauthorized-access",
            Label = "Enable Unauthorized File Share Access Logging",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Unauthorized file share access logging records failed access attempts to shares where the requestor lacks sufficient permissions. Enabling unauthorized access logging generates security events for share access denials providing visibility into access control boundary events. Failed share access events can indicate configuration errors, misconfigured access controls, or attempted unauthorized access. Security Event Log event 5140 with failure status records share access denials with requestor account and share name. Correlation of repeated share access failures from the same account across multiple servers may indicate lateral movement scanning. Unauthorized access events should be forwarded to SIEM and correlated with authentication events to assess intent and risk.",
            Tags = ["file-share", "access-denied", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LogUnauthorizedAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LogUnauthorizedAccess")],
            DetectOps = [RegOp.CheckDword(Key, "LogUnauthorizedAccess", 1)],
        },
        new TweakDef
        {
            Id = "filshare-disable-oplocks",
            Label = "Configure Opportunistic Locking for Sensitive Shares",
            Category = "File Share Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Opportunistic locking allows clients to cache file data locally for performance optimization but can cause data corruption with multiple concurrent editors. Configuring oplock behavior for sensitive shares prevents data loss scenarios where multiple writers believe they have exclusive access. Oplocks on database files, transactional data, and other sensitive data could cause corruption if network connectivity is interrupted during a cache hold. Disabling oplocks on specific shares forces clients to directly read from and write to the server ensuring data consistency. Oplocks are generally appropriate for read-heavy workloads but should be disabled for shares containing database files, application logs, or frequently modified shared configuration files. Share-level oplock configuration provides granular control without disabling oplocks globally across all shares.",
            Tags = ["file-share", "oplocks", "consistency", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConfigureOplocks", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureOplocks")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureOplocks", 0)],
        },
    ];
}
