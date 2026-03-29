// RegiLattice.Core — Tweaks/FileShareWitnessPolicy.cs
// File Share Witness Policy — Sprint 558.
// Configures Group Policy for SMB file share witness services, Server Message
// Block auditing, cluster shared volumes, and distributed file share failover.
// Category: "File Share Witness Policy" | Slug: fswitness
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\LanmanServer
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FileShareWitnessPolicy
{
    private const string SrvKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

    private const string WrkKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "fswitness-enable-smb-signing-server",
                Label = "File Share Witness: Require SMB Signing on Server",
                Category = "File Share Witness Policy",
                Description =
                    "Sets RequireSecuritySignature=1 in LanmanServer policy. Requires that all SMB connections to this machine as a server must use SMB packet signing. Without signing, SMB traffic is vulnerable to relay attacks (NTLM relay, SMB relay) where an attacker positioned between client and server can intercept and reuse SMB authentication tokens to authenticate to other services. SMB signing ensures each packet is cryptographically bound to the session — tampered or replayed packets are detected and rejected by both client and server.",
                Tags = ["smb", "signing", "relay-attack", "security", "server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "All SMB clients must negotiate signing when connecting to this server. Legacy SMB clients that do not support signing (rare; pre-Vista) will be rejected. Slight CPU overhead per packet on high-throughput file servers.",
                ApplyOps = [RegOp.SetDword(SrvKey, "RequireSecuritySignature", 1)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "RequireSecuritySignature")],
                DetectOps = [RegOp.CheckDword(SrvKey, "RequireSecuritySignature", 1)],
            },
            new TweakDef
            {
                Id = "fswitness-enable-smb-signing-client",
                Label = "File Share Witness: Require SMB Signing on Client",
                Category = "File Share Witness Policy",
                Description =
                    "Sets RequireSecuritySignature=1 in LanmanWorkstation policy. Requires that all outbound SMB connections from this machine as a client use SMB packet signing. The complementary client-side SMB signing policy ensures this device cannot connect to a rogue or unpatched server that does not sign packets — closing the attack path where an attacker deploys a malicious SMB server to capture NTLMv2 hashes. Together with server-side signing (fswitness-enable-smb-signing-server), both ends of every SMB connection are protected.",
                Tags = ["smb", "signing", "client", "relay-attack", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "This client only connects to SMB servers that support signing. Servers that do not support SMB signing are refused connections. Unpatched or misconfigured legacy file servers may become unreachable.",
                ApplyOps = [RegOp.SetDword(WrkKey, "RequireSecuritySignature", 1)],
                RemoveOps = [RegOp.DeleteValue(WrkKey, "RequireSecuritySignature")],
                DetectOps = [RegOp.CheckDword(WrkKey, "RequireSecuritySignature", 1)],
            },
            new TweakDef
            {
                Id = "fswitness-disable-guestaccess-smb",
                Label = "File Share Witness: Disable SMB Guest Access on Client",
                Category = "File Share Witness Policy",
                Description =
                    "Sets AllowInsecureGuestAuth=0 in LanmanWorkstation policy. Prevents the SMB client from connecting to a file share using unauthenticated guest access. When a Windows SMB client cannot authenticate with the credentials it has (wrong username/password), it may fall back to connecting as 'Guest' — an anonymous account with no password. Rogue SMB servers exploit this fallback to man-in-the-middle legitimate connections. Microsoft disabled guest access by default in Windows 10 1709; this policy enforces the setting via Group Policy to prevent local overrides.",
                Tags = ["smb", "guest", "unauthenticated", "security", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "SMB guest connections are blocked. Shares with misconfigured permissions that previously allowed guest may become inaccessible. SMB connections require valid credentials. Users with wrong passwords receive an authentication error rather than a guest session.",
                ApplyOps = [RegOp.SetDword(WrkKey, "AllowInsecureGuestAuth", 0)],
                RemoveOps = [RegOp.DeleteValue(WrkKey, "AllowInsecureGuestAuth")],
                DetectOps = [RegOp.CheckDword(WrkKey, "AllowInsecureGuestAuth", 0)],
            },
            new TweakDef
            {
                Id = "fswitness-disable-smb1-server",
                Label = "File Share Witness: Disable SMB1 Protocol on Server",
                Category = "File Share Witness Policy",
                Description =
                    "Sets SMB1=0 in LanmanServer policy. Disables the SMBv1 protocol on the server component. SMBv1 is a 1980s-era protocol with no encryption, no pre-authentication integrity, no signing by default, and numerous unfixed vulnerabilities including EternalBlue (CVE-2017-0144) which was exploited by WannaCry and NotPetya ransomware. Microsoft deprecated SMBv1 in 2014. Any operating system newer than Windows XP/Server 2003 supports SMBv2+. Disabling SMBv1 on the server prevents legacy client connections but eliminates the most dangerous attack surface in Windows networking.",
                Tags = ["smb1", "smb", "eternalblue", "ransomware", "protocol"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "SMBv1 server is disabled. Clients that can only use SMBv1 (Windows XP, Server 2003, early SAMBA versions, some legacy NAS appliances) cannot connect. Verify no SMBv1-only clients exist before applying.",
                ApplyOps = [RegOp.SetDword(SrvKey, "SMB1", 0)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "SMB1")],
                DetectOps = [RegOp.CheckDword(SrvKey, "SMB1", 0)],
            },
            new TweakDef
            {
                Id = "fswitness-set-smb-max-connections",
                Label = "File Share Witness: Set Maximum SMB Simultaneous Open Files Limit",
                Category = "File Share Witness Policy",
                Description =
                    "Sets MaxWorkItems=16384 in LanmanServer policy. Sets the maximum number of SMB work items (pending I/O operations per connection) the server will process simultaneously. The default value (64 on some configurations) can cause server-side SMB queuing under heavy load from many concurrent clients (e.g., login storms or VDI deployments). Increasing to 16384 allows more concurrent file operations without queuing delay. This setting must be balanced against available memory — each work item consumes non-paged pool memory.",
                Tags = ["smb", "performance", "work-items", "concurrency", "file-server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Server processes up to 16384 simultaneous SMB work items. Improves throughput under high-concurrency file access. Consumes additional non-paged pool memory on servers with many concurrent SMB clients.",
                ApplyOps = [RegOp.SetDword(SrvKey, "MaxWorkItems", 16384)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "MaxWorkItems")],
                DetectOps = [RegOp.CheckDword(SrvKey, "MaxWorkItems", 16384)],
            },
            new TweakDef
            {
                Id = "fswitness-enable-smb-encryption",
                Label = "File Share Witness: Enable SMB3 Encryption on Server",
                Category = "File Share Witness Policy",
                Description =
                    "Sets EncryptData=1 in LanmanServer policy. Enables mandatory AES-CCM or AES-GCM encryption for all SMB3 data transfers between client and server. SMB signing (enforced separately) provides integrity but not confidentiality — signing prevents tampering but a network observer can still read file contents in plaintext. SMB encryption wraps the data payload in a cryptographic envelope. Required for environments where file shares carry sensitive data (PII, financial, health records) and the network is not fully trusted (branch offices, cloud-hosted file servers).",
                Tags = ["smb3", "encryption", "confidentiality", "data-protection", "aes"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All SMB3 connections to this server use encryption. Clients must support SMB3 encryption (Windows 8+ / Server 2012+). CPU overhead for encryption; significant impact on high-bandwidth file copy operations on CPU-constrained servers.",
                ApplyOps = [RegOp.SetDword(SrvKey, "EncryptData", 1)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "EncryptData")],
                DetectOps = [RegOp.CheckDword(SrvKey, "EncryptData", 1)],
            },
            new TweakDef
            {
                Id = "fswitness-set-idle-connection-timeout",
                Label = "File Share Witness: Set SMB Server Idle Connection Timeout to 15 Minutes",
                Category = "File Share Witness Policy",
                Description =
                    "Sets AutoDisconnect=15 in LanmanServer policy. Sets the idle timeout for SMB server connections to 15 minutes. By default, SMB servers disconnect idle clients after 15 minutes, but the policy value can be changed. An excessively long idle timeout keeps network connections open and server-side session state allocated for users who have walked away from their desks. Idle sessions can also be reused after token expiry, enabling authentication bypass with stale credentials. 15 minutes matches the optimal balance between session reuse costs and reconnection overhead.",
                Tags = ["smb", "idle-timeout", "session-management", "server", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Idle SMB sessions are disconnected after 15 minutes. Users reconnect transparently when accessing mapped drives after idle disconnection. Short-lived applications that hold SMB connections open but rarely use them may silently reconnect.",
                ApplyOps = [RegOp.SetDword(SrvKey, "AutoDisconnect", 15)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoDisconnect")],
                DetectOps = [RegOp.CheckDword(SrvKey, "AutoDisconnect", 15)],
            },
            new TweakDef
            {
                Id = "fswitness-restrict-anonymous-share-enum",
                Label = "File Share Witness: Restrict Anonymous Network Share Enumeration",
                Category = "File Share Witness Policy",
                Description =
                    "Sets RestrictNullSessAccess=1 in LanmanServer policy. Prevents unauthenticated (null session) enumeration of network shares over the IPC$ pipe. Without this restriction, an attacker who discovers a Windows server's IP address can use null session connections to enumerate all shared folder names, providing the attacker with a map of file shares they can then target with brute-force credential attacks. This setting is enabled by default but can be disabled by administrators or overwrote by other policies; enforcing it via policy ensures it cannot be changed.",
                Tags = ["smb", "anonymous", "null-session", "enumeration", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Anonymous (null session) network share enumeration is blocked. Legitimate client connections with credentials are unaffected. Tools like NetScan and LanSweeper that enumerate shares with null sessions may not discover shares on this server.",
                ApplyOps = [RegOp.SetDword(SrvKey, "RestrictNullSessAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "RestrictNullSessAccess")],
                DetectOps = [RegOp.CheckDword(SrvKey, "RestrictNullSessAccess", 1)],
            },
            new TweakDef
            {
                Id = "fswitness-enable-smb-hardened-unc",
                Label = "File Share Witness: Enable Hardened UNC Path Requirements",
                Category = "File Share Witness Policy",
                Description =
                    "Sets HardenedUNCPathsEnabled=1 in LanmanWorkstation policy. Enables hardened UNC path processing, which requires mutual authentication and integrity for connections to UNC paths matching patterns registered in the HardenedUNCPaths registry list (\\\\*\\NETLOGON, \\\\*\\SYSVOL, etc). Without hardened UNC paths, a man-in-the-middle attacker can serve a rogue SYSVOL or NETLOGON share to deliver malicious Group Policy objects or logon scripts. Hardened UNC paths were introduced as the main mitigation for MS15-011 (JASBUG).",
                Tags = ["smb", "unc", "hardening", "gpo", "ms15-011"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "UNC paths to NETLOGON and SYSVOL require mutual authentication and signing. Man-in-the-middle attacks against Group Policy delivery are blocked. No impact on normal AD-joined clients with a working domain connection.",
                ApplyOps = [RegOp.SetDword(WrkKey, "HardenedUNCPathsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WrkKey, "HardenedUNCPathsEnabled")],
                DetectOps = [RegOp.CheckDword(WrkKey, "HardenedUNCPathsEnabled", 1)],
            },
            new TweakDef
            {
                Id = "fswitness-disable-admin-shares",
                Label = "File Share Witness: Disable Automatic Administrative Shares",
                Category = "File Share Witness Policy",
                Description =
                    "Sets AutoShareWks=0 in LanmanServer policy. Disables the automatic creation of administrative hidden shares (C$, D$, ADMIN$) on workstations. Administrative shares allow remote administrative access to root drive paths over SMB. While useful for IT management tools and legacy remote administration scripts, these shares provide an attackers with direct filesystem access if a privileged account is compromised. In environments using modern management tools (Intune, SCCM, WinRM), the administrative shares are rarely needed and present unnecessary lateral movement surface.",
                Tags = ["smb", "admin-shares", "attack-surface", "lateral-movement", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "C$, D$, ADMIN$ administrative shares are removed. Remote administration tools that rely on these shares (PsExec, legacy SCCM push, RoboCopy to C$) stop working. Verify that all management tools use WinRM, WMI, or agent-based access before applying.",
                ApplyOps = [RegOp.SetDword(SrvKey, "AutoShareWks", 0)],
                RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoShareWks")],
                DetectOps = [RegOp.CheckDword(SrvKey, "AutoShareWks", 0)],
            },
        ];
}
