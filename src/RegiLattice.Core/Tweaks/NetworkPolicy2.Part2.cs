namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyNetworkExt
{
    // ── SharedFoldersSmbPolicy ──
    private static class _SharedFoldersSmbPolicy
    {
        private const string LanWs = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";
        private const string LanSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smbshare-restrict-null-session-access",
                Label = "Restrict Null Session Access to Named Pipes and Shares",
                Category = "Network — Radius Auth",
                Description =
                    "Prevents anonymous (null session) connections from accessing named pipes and shares, blocking unauthenticated SMB enumeration.",
                Tags = ["smb", "network", "security", "hardening", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(LanSrv, "RestrictNullSessAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "RestrictNullSessAccess")],
                DetectOps = [RegOp.CheckDword(LanSrv, "RestrictNullSessAccess", 1)],
            },
            new TweakDef
            {
                Id = "smbshare-clear-null-session-pipes",
                Label = "Clear Null Session Named Pipes List",
                Category = "Network — Radius Auth",
                Description = "Removes all named pipes accessible via anonymous null sessions, reducing SMB attack surface.",
                Tags = ["smb", "network", "security", "hardening", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetMultiSz(LanSrv, "NullSessionPipes", [])],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "NullSessionPipes")],
                DetectOps = [RegOp.CheckString(LanSrv, "NullSessionPipes", "")],
            },
            new TweakDef
            {
                Id = "smbshare-clear-null-session-shares",
                Label = "Clear Null Session Shares List",
                Category = "Network — Radius Auth",
                Description = "Removes all shares accessible via anonymous null sessions, preventing unauthenticated share enumeration.",
                Tags = ["smb", "network", "security", "hardening", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetMultiSz(LanSrv, "NullSessionShares", [])],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "NullSessionShares")],
                DetectOps = [RegOp.CheckString(LanSrv, "NullSessionShares", "")],
            },
            new TweakDef
            {
                Id = "smbshare-enable-forced-logoff",
                Label = "Enable Forced Logoff When Logon Hours Expire",
                Category = "Network — Radius Auth",
                Description = "Forces a logoff when a user's permitted logon hours expire, ensuring access control policies are enforced.",
                Tags = ["smb", "network", "security", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(LanSrv, "EnableForcedLogOff", 1)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "EnableForcedLogOff")],
                DetectOps = [RegOp.CheckDword(LanSrv, "EnableForcedLogOff", 1)],
            },
            new TweakDef
            {
                Id = "smbshare-disable-admin-shares",
                Label = "Disable Default Administrative SMB Shares",
                Category = "Network — Radius Auth",
                Description =
                    "Disables automatic creation of administrative shares (C$, ADMIN$, IPC$), reducing remote administrative access surface.",
                Tags = ["smb", "network", "security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(LanSrv, "AutoShareWks", 0)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "AutoShareWks")],
                DetectOps = [RegOp.CheckDword(LanSrv, "AutoShareWks", 0)],
            },
            new TweakDef
            {
                Id = "smbshare-set-smb-max-connections",
                Label = "Set Maximum Concurrent SMB Connections",
                Category = "Network — Radius Auth",
                Description =
                    "Limits the number of concurrent SMB connections to 16,777,216 (MaxMpxCt), preventing resource exhaustion from SMB floods.",
                Tags = ["smb", "network", "performance", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(LanSrv, "MaxMpxCt", 16777216)],
                RemoveOps = [RegOp.DeleteValue(LanSrv, "MaxMpxCt")],
                DetectOps = [RegOp.CheckDword(LanSrv, "MaxMpxCt", 16777216)],
            },
        ];
    }

    // ── SmbEncryptionPolicy ──
    private static class _SmbEncryptionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SMB";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smbenc-require-smb-encryption",
                Label = "Require SMB Encryption for All Connections",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "SMB encryption protects file sharing traffic from network interception preventing passive eavesdropping on file transfers and credentials. Requiring SMB encryption for all connections ensures that unencrypted SMB traffic is rejected by the server component. Credential capture via SMB relay attacks is one of the most common lateral movement techniques in Windows environments. SMB encryption was introduced in SMB3 and prevents passive capture of file data and authentication metadata. Requiring encryption may prevent connections from legacy clients using SMB1 or SMB2 which do not support encryption. Enterprise environments should ensure all clients support SMB3 encryption before enforcing this requirement to avoid connectivity disruptions.",
                Tags = ["smb", "encryption", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "RequireEncryption", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-disable-smb1",
                Label = "Disable SMB Version 1 Protocol",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SMB version 1 is a critically vulnerable legacy protocol that enabled the EternalBlue exploit used in WannaCry, NotPetya, and other devastating ransomware campaigns. Disabling SMB1 removes the attack surface for known critical vulnerabilities including MS17-010 and related exploit chains. SMB1 does not support encryption, modern authentication, or other security features present in SMB2 and SMB3. Microsoft has recommended disabling SMB1 since 2014 and Windows 10 October 2018 Update removed SMB1 from default installations. Legacy devices requiring SMB1 such as old network-attached storage or printers should be replaced or isolated. Disabling SMB1 via policy is one of the highest-impact low-risk security improvements available for Windows network environments.",
                Tags = ["smb", "smb1", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSMB1", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSMB1")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSMB1", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-disable-smb-guest",
                Label = "Disable SMB Guest Authentication",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SMB guest authentication allows connections to file shares without credentials which creates an unauthenticated access path for malicious actors. Disabling SMB guest authentication requires all SMB connections to present valid credentials preventing anonymous file access. Guest authentication can be exploited in man-in-the-middle attacks where the attacker poses as a file server and accepts guest connections. Windows 10 1709 disabled guest authentication by default but older endpoints and custom configurations may still allow it. Guest authentication combined with credential harvesting tools can facilitate lateral movement in Windows networks. Disabling guest authentication is standard enterprise hardening and should be enforced through Group Policy on all Windows endpoints.",
                Tags = ["smb", "guest", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGuestAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGuestAuthentication")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGuestAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-enable-smb-signing",
                Label = "Require SMB Packet Signing",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SMB packet signing provides message integrity protection ensuring that SMB packets have not been tampered with in transit. Requiring SMB signing prevents SMB relay attacks where an attacker captures and replays authenticated SMB connections. NTLM relay attacks including the infamous SMBRelay attack family are blocked when both client and server require SMB signing. SMB signing has been available since Windows 2000 and has minimal performance impact with modern hardware. Without SMB signing an attacker with network access can perform authenticated relay attacks using captures from legitimate users. SMB signing should be required on all domain-joined endpoints and domain controllers where compatible clients exist.",
                Tags = ["smb", "signing", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequirePacketSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePacketSigning")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePacketSigning", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-block-ntlm-smb",
                Label = "Restrict NTLM Authentication over SMB",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "NTLM authentication over SMB is vulnerable to pass-the-hash attacks, credential relay, and offline password cracking of captured hashes. Restricting NTLM over SMB forces Kerberos authentication for domain-joined resources which provides stronger authentication guarantees. NTLM relay attacks can allow credential capture and reuse even when SMB signing is not required on the target server. Kerberos authentication over SMB prevents the majority of credential forwarding attacks by requiring valid Kerberos tickets. NTLM restriction may affect connections to workgroup resources, NAS devices, and non-domain-joined systems that only support NTLM. Organizations should audit NTLM dependency before restricting to avoid legitimate connectivity disruption.",
                Tags = ["smb", "ntlm", "kerberos", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNTLMOverSMB", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNTLMOverSMB")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNTLMOverSMB", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-enable-secure-dialect",
                Label = "Enforce Minimum SMB Dialect Version",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "SMB dialect negotiation allows clients and servers to agree on the highest supported protocol version but a minimum version prevents downgrade attacks. Enforcing a minimum SMB dialect prevents attackers from forcing connections to use older vulnerable protocol versions. Protocol downgrade attacks can force SMB2 or SMB3 connections to fall back to SMB1 which lacks security features. Setting a minimum dialect of SMB 3.0 ensures that all connections use the version with encryption and pre-authentication integrity. SMB dialect enforcement may break compatibility with servers or network appliances running older protocol versions. Enterprise SMB infrastructure should be surveyed for SMB dialect support before enforcing a minimum version requirement.",
                Tags = ["smb", "dialect", "protocol", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceMinDialectVersion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceMinDialectVersion")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceMinDialectVersion", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-enable-pre-auth-integrity",
                Label = "Enable SMB3 Pre-Authentication Integrity",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SMB pre-authentication integrity provides cryptographic protection of the connection establishment process preventing man-in-the-middle injection during negotiation. Enabling pre-authentication integrity ensures that an attacker cannot tamper with the protocol negotiation phase to downgrade security settings. Pre-authentication integrity was introduced in SMB 3.1.1 and is required for full SMB encryption security guarantees. Without pre-authentication integrity an attacker positioned between client and server can modify the negotiation to disable encryption or signing. Pre-authentication integrity requires both client and server to support SMB 3.1.1 which is available on Windows 10 1607 and Server 2016 onwards. Enabling pre-authentication integrity is part of SMB hardening and complements encryption and signing requirements.",
                Tags = ["smb", "pre-auth", "integrity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePreAuthIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePreAuthIntegrity")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePreAuthIntegrity", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-disable-admin-shares",
                Label = "Disable Default Administrative SMB Shares",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Administrative shares (C$ D$ ADMIN$) are default SMB shares that provide full drive access to administrators over the network. Disabling administrative shares removes these implicit file access paths that are frequently exploited for lateral movement in compromised environments. Ransomware and APT tools use administrative shares to copy files to remote systems and execute commands via PsExec and similar tools. Administrative shares are required for some legitimate IT management tools but modern endpoint management platforms do not rely on them. Disabling administrative shares forces legitimate remote management to use more controlled and monitored APIs instead of raw file access. Security teams should verify that no critical management tools depend on administrative shares before disabling them in production.",
                Tags = ["smb", "admin-shares", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAdminShares", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAdminShares")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAdminShares", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-restrict-anonymous-smb",
                Label = "Restrict Anonymous Access to SMB Named Pipes",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Anonymous access to SMB named pipes allows unauthenticated network enumeration of user accounts, share names, and other sensitive system information. Restricting anonymous named pipe access prevents attackers from gathering reconnaissance information without valid credentials. Named pipes such as LSARPC and SAMR exposed anonymously can be used to enumerate domain accounts, enumerate local groups, and gather other information. The RestrictAnonymous policy setting controls anonymous access to IPC$ and named pipes as distinct from regular file shares. Anonymous SMB enumeration is used by network scanners and exploitation frameworks to identify targets and gather credential targets. Restricting anonymous access should be part of standard Windows hardening alongside null session restrictions.",
                Tags = ["smb", "anonymous", "named-pipes", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymousAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymousAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymousAccess", 1)],
            },
            new TweakDef
            {
                Id = "smbenc-audit-smb-connections",
                Label = "Enable SMB Connection Audit Logging",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "SMB connection audit logging records all SMB share access events providing visibility into file server connections and access patterns. Enabling SMB audit logging generates security events for SMB sessions including user account, source address, share name, and file operations. SMB audit logs are essential for detecting anomalous file access patterns, data exfiltration via SMB shares, and lateral movement activity. Security audit data from SMB servers should be collected and analyzed by SIEM systems for real-time threat detection. Object access auditing for file shares enables logging of specific file-level access events within SMB shares for detailed forensic capability. SMB connection auditing combined with user behavior analytics can detect compromised accounts accessing unusual resources.",
                Tags = ["smb", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSMBConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSMBConnections")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSMBConnections", 1)],
            },
        ];
    }

    // ── SmbNetworking ──
    private static class _SmbNetworking
    {
        private const string LmWks = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";

        private const string LmSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

        private const string MrxSmb = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MrxSmb\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smb-enable-large-mtu",
                Label = "Enable SMB Large MTU Support",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "network", "mtu", "performance"],
                Description =
                    "Enables SMB large MTU (Maximum Transmission Unit) negotiation. "
                    + "Improves transfer speed on jumbo-frame-capable networks (MTU ≥ 9000).",
                ApplyOps = [RegOp.SetDword(LmWks, "EnableLargeMTU", 1)],
                RemoveOps = [RegOp.DeleteValue(LmWks, "EnableLargeMTU")],
                DetectOps = [RegOp.CheckDword(LmWks, "EnableLargeMTU", 1)],
            },
            new TweakDef
            {
                Id = "smb-reduce-dormant-file-limit",
                Label = "Reduce SMB Dormant File Connection Limit",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["smb", "network", "connections", "memory"],
                Description =
                    "Reduces the number of dormant SMB connections kept open from 1023 to 64. "
                    + "Frees memory on workstations that connect to many different file servers.",
                ApplyOps = [RegOp.SetDword(LmWks, "DormantFileLimit", 64)],
                RemoveOps = [RegOp.DeleteValue(LmWks, "DormantFileLimit")],
                DetectOps = [RegOp.CheckDword(LmWks, "DormantFileLimit", 64)],
            },
            new TweakDef
            {
                Id = "smb-increase-server-max-work-items",
                Label = "Increase SMB Server Work Items",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "server", "performance", "workitems"],
                Description =
                    "Increases the maximum number of queued work items the SMB server "
                    + "processes to 2048 (from the default 128). Helps high-concurrency file servers.",
                ApplyOps = [RegOp.SetDword(LmSrv, "MaxWorkItems", 2048)],
                RemoveOps = [RegOp.DeleteValue(LmSrv, "MaxWorkItems")],
                DetectOps = [RegOp.CheckDword(LmSrv, "MaxWorkItems", 2048)],
            },
            new TweakDef
            {
                Id = "smb-increase-server-max-raw-work-items",
                Label = "Increase SMB Server Raw Work Buffer",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "server", "performance", "buffer"],
                Description =
                    "Increases the raw-mode SMB server buffer count to 512 (default 4). "
                    + "Improves large sequential read/write throughput on file servers.",
                ApplyOps = [RegOp.SetDword(LmSrv, "MaxRawWorkItems", 512)],
                RemoveOps = [RegOp.DeleteValue(LmSrv, "MaxRawWorkItems")],
                DetectOps = [RegOp.CheckDword(LmSrv, "MaxRawWorkItems", 512)],
            },
            new TweakDef
            {
                Id = "smb-enforce-smb-signing-client",
                Label = "Enforce SMB Signing on Client",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["smb", "signing", "security", "hardening", "client"],
                Description =
                    "Requires the SMB client to sign all outgoing SMB connections. "
                    + "Pairs with the server-side enforcement tweak for full MITM protection. "
                    + "Reboot or network reconnect required for effect.",
                ApplyOps = [RegOp.SetDword(LmWks, "RequireSecuritySignature", 1)],
                RemoveOps = [RegOp.SetDword(LmWks, "RequireSecuritySignature", 0)],
                DetectOps = [RegOp.CheckDword(LmWks, "RequireSecuritySignature", 1)],
            },
            new TweakDef
            {
                Id = "smb-increase-collection-count",
                Label = "Increase SMB Write-Ahead Collection Count",
                Category = "Network — Radius Auth",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["smb", "write", "buffer", "performance"],
                Description =
                    "Increases the SMB client write-ahead collection buffer count to 32 "
                    + "(from default 16). Improves sequential write performance to file servers "
                    + "by batching more data before flushing.",
                ApplyOps = [RegOp.SetDword(LmWks, "MaxCollectionCount", 32)],
                RemoveOps = [RegOp.DeleteValue(LmWks, "MaxCollectionCount")],
                DetectOps = [RegOp.CheckDword(LmWks, "MaxCollectionCount", 32)],
            },
        ];
    }

    // ── SmbServerHardeningPolicy ──
    private static class _SmbServerHardeningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters";
        private const string SrvKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "smbsvr-disable-smb-compression",
                    Label = "Disable SMBv3 Compression to Prevent SMBleed Attacks",
                    Category = "Network — Radius Auth",
                    Description =
                        "Disables SMB compression on the server, mitigating SMBleed (CVE-2020-1206) and similar compression-path vulnerabilities that can allow unauthenticated reading of uninitialized kernel memory through SMB3 compressed data.",
                    Tags = ["smb", "compression", "smbleed", "cve-2020-1206", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SMBv3 compression disabled; SMBleed class vulnerabilities mitigated. Minor performance impact on compressed transfers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCompression")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCompression", 1)],
                },
                new TweakDef
                {
                    Id = "smbsvr-enable-smb-encryption",
                    Label = "Enable SMBv3 Encryption for All Shares (Enforce in Transit)",
                    Category = "Network — Radius Auth",
                    Description =
                        "Enables SMBv3 end-to-end encryption for all SMB connections to this server, ensuring file transfer content is AES-encrypted in transit and cannot be captured in plaintext on the network.",
                    Tags = ["smb", "encryption", "aes", "in-transit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "SMBv3 encryption enforced; file data is AES-encrypted in transit. Requires Windows 8/2012 or later clients.",
                    ApplyOps = [RegOp.SetDword(Key, "EncryptData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EncryptData")],
                    DetectOps = [RegOp.CheckDword(Key, "EncryptData", 1)],
                },
                new TweakDef
                {
                    Id = "smbsvr-disable-guest-fallback",
                    Label = "Disable SMB Guest Authentication Fallback",
                    Category = "Network — Radius Auth",
                    Description =
                        "Prevents the SMB client from automatically falling back to anonymous guest authentication when the provided credentials are rejected, stopping silent elevation-of-failure-to-anonymous-access on misconfigured shares.",
                    Tags = ["smb", "guest", "anonymous", "fallback", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SMB guest auth fallback disabled; authentication failures are hard failures, not silent anonymous access.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnableInsecureGuestLogons", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnableInsecureGuestLogons")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnableInsecureGuestLogons", 0)],
                },
                new TweakDef
                {
                    Id = "smbsvr-log-auth-failures",
                    Label = "Log SMB Authentication Failure Events in Security Log",
                    Category = "Network — Radius Auth",
                    Description =
                        "Enables Security event log audit entries for failed SMB authentication attempts, providing visibility into brute-force attacks and pass-the-hash attempts against network shares.",
                    Tags = ["smb", "auth-failure", "event-log", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SMB auth failure events logged in Security log; brute-force and pass-the-hash attempts visible.",
                    ApplyOps = [RegOp.SetDword(PolKey, "LogAuthFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "LogAuthFailures")],
                    DetectOps = [RegOp.CheckDword(PolKey, "LogAuthFailures", 1)],
                },
                new TweakDef
                {
                    Id = "smbsvr-disable-smb-telemetry",
                    Label = "Disable SMB Server Telemetry Reporting to Microsoft",
                    Category = "Network — Radius Auth",
                    Description =
                        "Prevents the SMB server from sending connection statistics, negotiated cipher suites, session rates, and protocol version telemetry to Microsoft.",
                    Tags = ["smb", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "SMB telemetry to Microsoft disabled; session rates and cipher negotiation data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableSMBTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableSMBTelemetry")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableSMBTelemetry", 1)],
                },
            ];
    }

    // ── SmbServerPolicy ──
    private static class _SmbServerPolicy
    {
        private const string SmbSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smbsrv-disable-admin-share-server",
                Label = "Disable Hidden Admin Shares (Server Mode)",
                Category = "Network — Radius Auth",
                Description =
                    "Sets AutoShareServer=0 in LanmanServer parameters. Prevents Windows from automatically creating the hidden administrative shares (C$, D$, ADMIN$, IPC$) on server-class installations when the computer starts. Reduces the exposed SMB attack surface on file server roles.",
                Tags = ["smb", "admin-share", "server", "security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(SmbSrv, "AutoShareServer", 0)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "AutoShareServer")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "AutoShareServer", 0)],
            },
            new TweakDef
            {
                Id = "smbsrv-enable-raw-mode",
                Label = "Enable SMB Raw Read/Write Mode",
                Category = "Network — Radius Auth",
                Description =
                    "Sets EnableRaw=1 in LanmanServer parameters. Ensures the SMB server permits raw-mode transfers (large single-command reads and writes without the overhead of a separate setup packet). Raw mode is the default; restoring it if previously disabled improves LAN performance for large file copies.",
                Tags = ["smb", "raw", "performance", "server", "tuning"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmbSrv, "EnableRaw", 1)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "EnableRaw")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "EnableRaw", 1)],
            },
            new TweakDef
            {
                Id = "smbsrv-set-size-req-buf",
                Label = "Set SMB Server Request Buffer Size to 4356",
                Category = "Network — Radius Auth",
                Description =
                    "Sets SizReqBuf=4356 in LanmanServer parameters. Configures the raw-mode read buffer size for the SMB server. 4356 bytes aligns the buffer to a common Ethernet MTU boundary (4 KB + SMB header overhead), which can reduce fragmented TCP segments for raw SMB operations on Gigabit networks.",
                Tags = ["smb", "buffer", "performance", "server", "tuning"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmbSrv, "SizReqBuf", 4356)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "SizReqBuf")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "SizReqBuf", 4356)],
            },
            new TweakDef
            {
                Id = "smbsrv-disk-space-threshold",
                Label = "Require 10% Free Disk Before SMB Writes",
                Category = "Network — Radius Auth",
                Description =
                    "Sets DiskSpaceThreshold=10 in LanmanServer parameters. Instructs the SMB server to return a disk-full error to clients when the volume hosting a share has less than 10% free space remaining, rather than waiting until the volume is completely full. Prevents total disk exhaustion which can corrupt open files.",
                Tags = ["smb", "disk", "threshold", "server", "reliability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmbSrv, "DiskSpaceThreshold", 10)],
                RemoveOps = [RegOp.DeleteValue(SmbSrv, "DiskSpaceThreshold")],
                DetectOps = [RegOp.CheckDword(SmbSrv, "DiskSpaceThreshold", 10)],
            },
        ];
    }

    // ── SnmpPolicy ──
    private static class _SnmpPolicy
    {
        private const string SnmpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters";
        private const string AgentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters\ValidCommunities";
        private const string MgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters\PermittedManagers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "snmppol-enable-auth-traps",
                Label = "SNMP Policy: Enable Authentication Failure Traps",
                Category = "Network — Radius Auth",
                Description =
                    "Sends SNMP authentication failure traps when unauthorized community string requests are received. Enables monitoring of unauthorized SNMP access attempts.",
                Tags = ["snmp", "auth", "traps", "monitoring", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Sends traps on unauthorized SNMP community string requests.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "EnableAuthenticationTraps", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "EnableAuthenticationTraps")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "EnableAuthenticationTraps", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-restrict-permitted-managers",
                Label = "SNMP Policy: Restrict Permitted Management Hosts",
                Category = "Network — Radius Auth",
                Description =
                    "Enforces GPO-defined list of permitted SNMP management hosts. The SNMP service only responds to requests from the hosts listed under PermittedManagers registry key.",
                Tags = ["snmp", "access-control", "managers", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts SNMP responses to localhost only; blocks all remote management hosts.",
                RegistryKeys = [MgrKey],
                ApplyOps = [RegOp.SetString(MgrKey, "1", "localhost")],
                RemoveOps = [RegOp.DeleteValue(MgrKey, "1")],
                DetectOps = [RegOp.CheckString(MgrKey, "1", "localhost")],
            },
            new TweakDef
            {
                Id = "snmppol-disable-community-readonly",
                Label = "SNMP Policy: Remove Default Public Read-Only Community",
                Category = "Network — Radius Auth",
                Description =
                    "Removes the 'public' SNMP community string from the valid communities list. The default public community string is a well-known attack vector that enables SNMP enumeration.",
                Tags = ["snmp", "community", "public", "hardening", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Removes the default 'public' community string — a well-known SNMP attack vector.",
                RegistryKeys = [AgentKey],
                ApplyOps = [RegOp.SetDword(AgentKey, "public", 0)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "public")],
                DetectOps = [RegOp.CheckDword(AgentKey, "public", 0)],
            },
            new TweakDef
            {
                Id = "snmppol-set-community-read-only",
                Label = "SNMP Policy: Restrict Community String Permissions (Read-Only)",
                Category = "Network — Radius Auth",
                Description =
                    "Sets the SNMP community string type to Read Only (4) and removes Write/Create/Delete rights. Prevents SNMP-based configuration changes from network management stations.",
                Tags = ["snmp", "community", "read-only", "permissions", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts community string to read-only; prevents SNMP SET operations.",
                RegistryKeys = [AgentKey],
                ApplyOps = [RegOp.SetDword(AgentKey, "private", 4)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "private")],
                DetectOps = [RegOp.CheckDword(AgentKey, "private", 4)],
            },
            new TweakDef
            {
                Id = "snmppol-disable-snmp-writeable",
                Label = "SNMP Policy: Disable SNMP Write Community Access",
                Category = "Network — Radius Auth",
                Description =
                    "Sets the community permissions to None (1) for the write community, disabling any SNMP SET operations. SNMP write access allows remote configuration changes to network devices.",
                Tags = ["snmp", "write", "set-operations", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables SNMP write (SET) operations entirely.",
                RegistryKeys = [AgentKey],
                ApplyOps = [RegOp.SetDword(AgentKey, "write", 1)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "write")],
                DetectOps = [RegOp.CheckDword(AgentKey, "write", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-enable-snmp-service-policy",
                Label = "SNMP Policy: Enable SNMP Service Policy Enforcement",
                Category = "Network — Radius Auth",
                Description =
                    "Enables GPO-based enforcement of SNMP service settings. When enabled, all SNMP service configuration is governed by Group Policy, overriding local service settings.",
                Tags = ["snmp", "gpo", "enforcement", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables GPO enforcement of all SNMP service settings.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "EnforceSNMPPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "EnforceSNMPPolicy")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "EnforceSNMPPolicy", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-disable-snmp-v1",
                Label = "SNMP Policy: Disable SNMPv1 Protocol",
                Category = "Network — Radius Auth",
                Description =
                    "Disables SNMPv1 through GPO policy. SNMPv1 transmits community strings in plain text and lacks encryption or authentication. Disabling it forces use of SNMPv2c or SNMPv3.",
                Tags = ["snmp", "v1", "legacy", "protocol", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Disables plaintext SNMPv1 protocol; forces SNMPv2c or SNMPv3.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "DisableSNMPv1", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "DisableSNMPv1")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "DisableSNMPv1", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-log-auth-failures",
                Label = "SNMP Policy: Log Authentication Failures to Event Log",
                Category = "Network — Radius Auth",
                Description =
                    "Configures the SNMP service to write authentication failure events to the Windows Security event log. Supports Security Information and Event Management (SIEM) integration.",
                Tags = ["snmp", "logging", "event-log", "auth", "siem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Writes SNMP auth failures to Windows Security event log for SIEM.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "LogAuthFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "LogAuthFailures")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "LogAuthFailures", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-block-snmp-from-internet",
                Label = "SNMP Policy: Block SNMP from Public Network Access",
                Category = "Network — Radius Auth",
                Description =
                    "Restricts SNMP to internal network connections only through GPO. Forces the SNMP service to discard any requests arriving from non-private network interfaces.",
                Tags = ["snmp", "firewall", "network", "internet", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts SNMP to internal network interfaces only.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "BlockPublicNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "BlockPublicNetworkAccess")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "BlockPublicNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "snmppol-restrict-trap-receivers",
                Label = "SNMP Policy: Restrict SNMP Trap Receivers to Known Hosts",
                Category = "Network — Radius Auth",
                Description =
                    "Applies GPO-enforced filtering to SNMP trap destinations, limiting trap broadcasts to administrator-approved management systems. Reduces SNMP trap amplification risk.",
                Tags = ["snmp", "traps", "trap-receivers", "network", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits SNMP trap destinations to approved management systems only.",
                RegistryKeys = [SnmpKey],
                ApplyOps = [RegOp.SetDword(SnmpKey, "RestrictTrapReceivers", 1)],
                RemoveOps = [RegOp.DeleteValue(SnmpKey, "RestrictTrapReceivers")],
                DetectOps = [RegOp.CheckDword(SnmpKey, "RestrictTrapReceivers", 1)],
            },
        ];
    }

    // ── SshHardening ──
    private static class _SshHardening
    {
        private const string SshdConfig = @"C:\ProgramData\ssh\sshd_config";

        // Helper: apply a directive in sshd_config (add or replace).
        private static void SetSshdDirective(string directive, string value, bool dryRun)
        {
            if (dryRun)
                return;
            if (!System.IO.File.Exists(SshdConfig))
                return;
            string line = $"{directive} {value}";
            var lines = System.IO.File.ReadAllLines(SshdConfig);
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].TrimStart();
                if (
                    trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                    && (trimmed.Length == directive.Length || trimmed[directive.Length] == ' ')
                )
                {
                    lines[i] = line;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                System.Array.Resize(ref lines, lines.Length + 1);
                lines[^1] = line;
            }
            System.IO.File.WriteAllLines(SshdConfig, lines);
        }

        // Helper: remove / comment out a directive.
        private static void RemoveSshdDirective(string directive, bool dryRun)
        {
            if (dryRun || !System.IO.File.Exists(SshdConfig))
                return;
            var lines = System.IO.File.ReadAllLines(SshdConfig);
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].TrimStart();
                if (
                    trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                    && (trimmed.Length == directive.Length || trimmed[directive.Length] == ' ')
                )
                {
                    lines[i] = "#" + lines[i];
                    break;
                }
            }
            System.IO.File.WriteAllLines(SshdConfig, lines);
        }

        // Helper: detect a directive is set to the expected value.
        private static bool DetectSshdDirective(string directive, string expectedValue)
        {
            if (!System.IO.File.Exists(SshdConfig))
                return false;
            foreach (string raw in System.IO.File.ReadAllLines(SshdConfig))
            {
                string trimmed = raw.TrimStart();
                if (trimmed.StartsWith("#", System.StringComparison.Ordinal))
                    continue;
                if (
                    trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                    && trimmed.Length > directive.Length
                    && trimmed[directive.Length] == ' '
                )
                {
                    string actual = trimmed[(directive.Length + 1)..].Trim();
                    return string.Equals(actual, expectedValue, System.StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        private static bool SshdConfigExists() => System.IO.File.Exists(SshdConfig);

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ssh-max-auth-tries-3",
                Label = "Limit SSH Authentication Attempts to 3",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets MaxAuthTries 3 in sshd_config. Limits failed authentication attempts "
                    + "per connection to 3 before disconnecting, reducing brute-force window. Default: 6.",
                Tags = ["ssh", "authentication", "brute-force", "security", "hardening"],
                ApplyAction = dry => SetSshdDirective("MaxAuthTries", "3", dry),
                RemoveAction = dry => RemoveSshdDirective("MaxAuthTries", dry),
                DetectAction = () => DetectSshdDirective("MaxAuthTries", "3"),
            },
            new TweakDef
            {
                Id = "ssh-login-grace-time-30",
                Label = "SSH Login Grace Time 30 Seconds",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets LoginGraceTime 30 in sshd_config. The server disconnects if a user "
                    + "has not authenticated within 30 seconds, preventing half-open connection exhaustion attacks. Default: 120.",
                Tags = ["ssh", "timeout", "security", "hardening", "dos"],
                ApplyAction = dry => SetSshdDirective("LoginGraceTime", "30", dry),
                RemoveAction = dry => RemoveSshdDirective("LoginGraceTime", dry),
                DetectAction = () => DetectSshdDirective("LoginGraceTime", "30"),
            },
            new TweakDef
            {
                Id = "ssh-permit-empty-passwords-no",
                Label = "Disallow SSH Empty Password Logins",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets PermitEmptyPasswords no in sshd_config. Prevents accounts with blank passwords "
                    + "from authenticating via SSH. Default: no (safe), but explicitly enforced here.",
                Tags = ["ssh", "password", "authentication", "security"],
                ApplyAction = dry => SetSshdDirective("PermitEmptyPasswords", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("PermitEmptyPasswords", dry),
                DetectAction = () => DetectSshdDirective("PermitEmptyPasswords", "no"),
            },
            new TweakDef
            {
                Id = "ssh-disable-agent-forwarding",
                Label = "Disable SSH Agent Forwarding",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets AllowAgentForwarding no in sshd_config. Prevents forwarding of the SSH authentication "
                    + "agent from remote hosts, which could allow lateral movement if a relay host is compromised. Default: yes.",
                Tags = ["ssh", "agent", "forwarding", "security", "lateral-movement"],
                ApplyAction = dry => SetSshdDirective("AllowAgentForwarding", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("AllowAgentForwarding", dry),
                DetectAction = () => DetectSshdDirective("AllowAgentForwarding", "no"),
            },
            new TweakDef
            {
                Id = "ssh-disable-tcp-forwarding",
                Label = "Disable SSH TCP Forwarding",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets AllowTcpForwarding no in sshd_config. Prevents SSH tunnelling of TCP connections "
                    + "through this host, blocking use of SSH as a proxy or pivot point. Default: yes.",
                Tags = ["ssh", "tcp", "tunnel", "forwarding", "security"],
                ApplyAction = dry => SetSshdDirective("AllowTcpForwarding", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("AllowTcpForwarding", dry),
                DetectAction = () => DetectSshdDirective("AllowTcpForwarding", "no"),
            },
            new TweakDef
            {
                Id = "ssh-max-sessions-2",
                Label = "Limit SSH Concurrent Sessions to 2",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = false,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets MaxSessions 2 in sshd_config. Caps multiplexed sessions per connection to 2, "
                    + "limiting resource exhaustion attacks. May need increasing for automation workflows. Default: 10.",
                Tags = ["ssh", "sessions", "dos", "security", "resource"],
                ApplyAction = dry => SetSshdDirective("MaxSessions", "2", dry),
                RemoveAction = dry => RemoveSshdDirective("MaxSessions", dry),
                DetectAction = () => DetectSshdDirective("MaxSessions", "2"),
            },
            new TweakDef
            {
                Id = "ssh-strict-modes",
                Label = "Enable SSH StrictModes",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets StrictModes yes in sshd_config. Forces SSH to check file and directory permissions "
                    + "before accepting logins. Rejects login if home directory or authorized_keys are world-writable. Default: yes.",
                Tags = ["ssh", "permissions", "strictmodes", "security"],
                ApplyAction = dry => SetSshdDirective("StrictModes", "yes", dry),
                RemoveAction = dry => RemoveSshdDirective("StrictModes", dry),
                DetectAction = () => DetectSshdDirective("StrictModes", "yes"),
            },
            new TweakDef
            {
                Id = "ssh-disable-x11-forwarding",
                Label = "Disable SSH X11 Forwarding",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets X11Forwarding no in sshd_config. X11 forwarding is irrelevant on Windows "
                    + "and expands the attack surface by creating X11 proxy connections. Default: no on Windows.",
                Tags = ["ssh", "x11", "forwarding", "security", "attack-surface"],
                ApplyAction = dry => SetSshdDirective("X11Forwarding", "no", dry),
                RemoveAction = dry => RemoveSshdDirective("X11Forwarding", dry),
                DetectAction = () => DetectSshdDirective("X11Forwarding", "no"),
            },
            new TweakDef
            {
                Id = "ssh-set-strong-ciphers",
                Label = "Restrict SSH to Strong Ciphers",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets Ciphers to AES-256 in CTR/GCM modes only in sshd_config. "
                    + "Removes weak ciphers (3DES, RC4, AES-128-CBC) from the negotiation list. "
                    + "Default: broad cipher list. Recommended: AES-256-GCM and AES-256-CTR only.",
                Tags = ["ssh", "cipher", "encryption", "security", "hardening"],
                ApplyAction = dry => SetSshdDirective("Ciphers", "aes256-gcm@openssh.com,aes256-ctr", dry),
                RemoveAction = dry => RemoveSshdDirective("Ciphers", dry),
                DetectAction = () => DetectSshdDirective("Ciphers", "aes256-gcm@openssh.com,aes256-ctr"),
            },
            new TweakDef
            {
                Id = "ssh-set-strong-macs",
                Label = "Restrict SSH to Strong MAC Algorithms",
                Category = "Network — Radius Auth",
                KindHint = TweakKind.FileConfig,
                NeedsAdmin = true,
                CorpSafe = true,
                IsApplicable = SshdConfigExists,
                ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
                Description =
                    "Sets MACs to HMAC-SHA2-512 and HMAC-SHA2-256 in sshd_config. "
                    + "Removes weak MACs (MD5, SHA1) from negotiation. "
                    + "Default: broad MAC list including SHA1. Recommended: SHA-256/SHA-512 only.",
                Tags = ["ssh", "mac", "hmac", "encryption", "security"],
                ApplyAction = dry => SetSshdDirective("MACs", "hmac-sha2-512,hmac-sha2-256", dry),
                RemoveAction = dry => RemoveSshdDirective("MACs", dry),
                DetectAction = () => DetectSshdDirective("MACs", "hmac-sha2-512,hmac-sha2-256"),
            },
        ];
    }

    // ── VoipQualityPolicy ──
    private static class _VoipQualityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "voipqos-set-teams-audio-dscp-value",
                    Label = "VoIP QoS: Mark Teams Audio RTP with DSCP EF (46)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioDscpValue=46 in Teams QoS policy. Instructs Teams to mark all real-time audio RTP packets with DSCP EF (Expedited Forwarding = 46, the highest priority class). "
                        + "On enterprise networks with QoS-aware switches and routers, EF-marked packets receive the smallest queuing delay and lowest drop probability, which directly reduces jitter and one-way latency in Teams calls. "
                        + "This setting is distinct from the generic Windows QoS multimedia scheduling rate and applies specifically to the Teams media engine RTP streams.",
                    Tags = ["teams", "voip", "qos", "dscp", "audio", "rtp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Marks Teams audio RTP with EF DSCP 46; critical for low-latency voice on congested enterprise networks.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioDscpValue", 46)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioDscpValue")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioDscpValue", 46)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-video-dscp-value",
                    Label = "VoIP QoS: Mark Teams Video RTP with DSCP AF41 (34)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoDscpValue=34 in Teams QoS policy. Marks Teams video RTP packets with DSCP AF41 (Assured Forwarding 41 = 34). "
                        + "AF41 is the IETF recommendation for interactive video conferencing traffic. It receives higher priority than best-effort but is de-prioritised below EF (audio). "
                        + "Separating audio (EF) and video (AF41) ensures audio is never starved by high-bitrate video bursts during congestion.",
                    Tags = ["teams", "voip", "qos", "dscp", "video", "rtp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Marks Teams video with AF41 DSCP 34; prevents video bursts from starving audio on saturated links.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoDscpValue", 34)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoDscpValue")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoDscpValue", 34)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-appshar-dscp-value",
                    Label = "VoIP QoS: Mark Teams App-Sharing with DSCP AF21 (18)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AppShareDscpValue=18 in Teams QoS policy. Marks Teams application-sharing and desktop-sharing RTP streams with DSCP AF21 (Assured Forwarding 21 = 18). "
                        + "Screen share generates large and bursty traffic which should be deprioritised relative to live audio and video to prevent real-time media degredation during presentations.",
                    Tags = ["teams", "voip", "qos", "dscp", "screenshare", "rtp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Marks app-sharing with AF21 DSCP 18; prevents screen share bursts from degrading audio/video quality.",
                    ApplyOps = [RegOp.SetDword(Key, "AppShareDscpValue", 18)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AppShareDscpValue")],
                    DetectOps = [RegOp.CheckDword(Key, "AppShareDscpValue", 18)],
                },
                new TweakDef
                {
                    Id = "voipqos-enable-teams-audio-port-range",
                    Label = "VoIP QoS: Enable Teams-Specific Audio UDP Port Range",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioPortsEnabled=1 in Teams QoS policy. Enables the use of a dedicated UDP port range for Teams audio media. "
                        + "Port-based QoS rules on network switches and firewalls can then classify and prioritise Teams audio traffic from these specific ports rather than relying solely on DSCP markings, which are sometimes stripped by ISPs.",
                    Tags = ["teams", "voip", "qos", "ports", "udp", "audio"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables fixed port range for Teams audio; allows port-based QoS classification in addition to DSCP.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioPortsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioPortsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioPortsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-audio-port-start-50000",
                    Label = "VoIP QoS: Set Teams Audio Port Range Start to 50000",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioPortStart=50000 in Teams QoS policy. Configures the start of the UDP port range used by Teams audio media to port 50000. "
                        + "This port base aligns with the Microsoft-recommended range for Teams voice and allows network administrators to create firewall ACLs and QoS policies targeting the well-known 50000–50019 range.",
                    Tags = ["teams", "voip", "qos", "ports", "audio", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Sets audio port range start to 50000 per Microsoft recommendation; enables precise firewall and QoS rules.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioPortStart", 50000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioPortStart")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioPortStart", 50000)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-audio-port-count-20",
                    Label = "VoIP QoS: Set Teams Audio Port Count to 20",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AudioPortCount=20 in Teams QoS policy. Allocates 20 consecutive UDP ports for Teams audio media starting from AudioPortStart. "
                        + "A count of 20 provides enough ports for simultaneous call sessions on a single machine while keeping the range narrow enough for precise firewall and QoS ACL rules.",
                    Tags = ["teams", "voip", "qos", "ports", "audio", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Allocates 20 UDP ports for Teams audio; balances multi-session capacity with narrow QoS rule precision.",
                    ApplyOps = [RegOp.SetDword(Key, "AudioPortCount", 20)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AudioPortCount")],
                    DetectOps = [RegOp.CheckDword(Key, "AudioPortCount", 20)],
                },
                new TweakDef
                {
                    Id = "voipqos-enable-teams-video-port-range",
                    Label = "VoIP QoS: Enable Teams-Specific Video UDP Port Range",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoPortsEnabled=1 in Teams QoS policy. Enables the use of a dedicated UDP port range for Teams video media streams. "
                        + "Separating video on its own port range allows network equipment to apply different QoS policies to audio and video independently, which is important when network bandwidth needs to preferentially protect audio quality over video.",
                    Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables dedicated video port range; allows separate QoS treatment of audio versus video streams.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoPortsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoPortsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoPortsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-video-port-start-50020",
                    Label = "VoIP QoS: Set Teams Video Port Range Start to 50020",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoPortStart=50020 in Teams QoS policy. Sets the starting UDP port for Teams video media to 50020, immediately following the audio port range (50000–50019). "
                        + "This layout allows a single contiguous firewall rule (50000–50039) to cover both audio and video, while still allowing separate DSCP markings to be applied per-range.",
                    Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sets video port start to 50020; aligns with audio range for manageable firewall rule design.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoPortStart", 50020)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoPortStart")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoPortStart", 50020)],
                },
                new TweakDef
                {
                    Id = "voipqos-set-teams-video-port-count-20",
                    Label = "VoIP QoS: Set Teams Video Port Count to 20",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets VideoPortCount=20 in Teams QoS policy. Allocates 20 UDP ports for Teams video media starting at VideoPortStart. "
                        + "20 ports accommodates multiple simultaneous video sessions and gallery view scenarios without creating an overly broad firewall footprint.",
                    Tags = ["teams", "voip", "qos", "ports", "video", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Allocates 20 UDP ports for Teams video; supports gallery view with a narrow, manageable port range.",
                    ApplyOps = [RegOp.SetDword(Key, "VideoPortCount", 20)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VideoPortCount")],
                    DetectOps = [RegOp.CheckDword(Key, "VideoPortCount", 20)],
                },
                new TweakDef
                {
                    Id = "voipqos-enable-teams-appshar-port-range",
                    Label = "VoIP QoS: Enable Teams App-Sharing UDP Port Range",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AppSharePortsEnabled=1 in Teams QoS policy. Enables a dedicated UDP port range for Teams application-sharing and desktop-sharing media streams. "
                        + "Isolating app-sharing on its own port range allows network QoS policies to apply lower priority scheduling to screen share traffic while still guaranteeing audio and video delivery during congestion.",
                    Tags = ["teams", "voip", "qos", "ports", "screenshare", "udp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables dedicated app-sharing port range; decouple screen share QoS from audio/video port policies.",
                    ApplyOps = [RegOp.SetDword(Key, "AppSharePortsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AppSharePortsEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AppSharePortsEnabled", 1)],
                },
            ];
    }

    // ── VpnRemoteAccessPolicy ──
    private static class _VpnRemoteAccessPolicy
    {
        private const string RasKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess";
        private const string IkeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess\IKEv2";
        private const string ConnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "vpnras-require-strong-encryption",
                    Label = "Require Strong Encryption for VPN Connections",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enforces maximum-strength encryption (MPPE 128-bit or AES-256) for all RRAS VPN connections. Rejects connections that negotiate weaker ciphers. Default: optional encryption.",
                    Tags = ["vpn", "encryption", "rras", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "VPN connections must use strong encryption; clients with weak cipher support will fail to connect.",
                    ApplyOps = [RegOp.SetDword(RasKey, "RequireStrongEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "RequireStrongEncryption")],
                    DetectOps = [RegOp.CheckDword(RasKey, "RequireStrongEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-disable-pptp-protocol",
                    Label = "Disable PPTP VPN Protocol",
                    Category = "Network — Voip Quality",
                    Description =
                        "Disables the insecure PPTP (Point-to-Point Tunneling Protocol) for VPN connections. PPTP is considered cryptographically broken. Default: enabled.",
                    Tags = ["vpn", "pptp", "security", "deprecated", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "PPTP connections blocked; legacy clients relying on PPTP must migrate to IKEv2/L2TP.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisablePPTP", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisablePPTP")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisablePPTP", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-enable-ikev2-preferred",
                    Label = "Set IKEv2 as Preferred VPN Protocol",
                    Category = "Network — Voip Quality",
                    Description =
                        "Configures RRAS to prefer IKEv2 (Internet Key Exchange v2) for VPN tunnel negotiation. IKEv2 supports MOBIKE for seamless roaming. Default: automatic protocol selection.",
                    Tags = ["vpn", "ikev2", "protocol", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "IKEv2 preferred for new connections; fallback to L2TP/SSTP if IKEv2 unavailable.",
                    ApplyOps = [RegOp.SetDword(IkeKey, "PreferIKEv2", 1)],
                    RemoveOps = [RegOp.DeleteValue(IkeKey, "PreferIKEv2")],
                    DetectOps = [RegOp.CheckDword(IkeKey, "PreferIKEv2", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-set-idle-timeout",
                    Label = "Set VPN Idle Disconnect Timeout to 30 Minutes",
                    Category = "Network — Voip Quality",
                    Description =
                        "Automatically disconnects inactive VPN sessions after 30 minutes of idle time. Frees up VPN server resources. Default: no idle timeout.",
                    Tags = ["vpn", "idle", "timeout", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Idle VPN sessions dropped after 30 minutes; users reconnect on next activity.",
                    ApplyOps = [RegOp.SetDword(ConnKey, "IdleDisconnectTimeout", 30)],
                    RemoveOps = [RegOp.DeleteValue(ConnKey, "IdleDisconnectTimeout")],
                    DetectOps = [RegOp.CheckDword(ConnKey, "IdleDisconnectTimeout", 30)],
                },
                new TweakDef
                {
                    Id = "vpnras-set-max-sessions",
                    Label = "Set Maximum Concurrent VPN Sessions to 100",
                    Category = "Network — Voip Quality",
                    Description =
                        "Limits the maximum number of concurrent VPN connections to the RRAS server to 100. Prevents resource exhaustion from excessive connections. Default: unlimited.",
                    Tags = ["vpn", "sessions", "limit", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Connection limit prevents server overload; users beyond 100 are queued or rejected.",
                    ApplyOps = [RegOp.SetDword(ConnKey, "MaxSessions", 100)],
                    RemoveOps = [RegOp.DeleteValue(ConnKey, "MaxSessions")],
                    DetectOps = [RegOp.CheckDword(ConnKey, "MaxSessions", 100)],
                },
                new TweakDef
                {
                    Id = "vpnras-enable-connection-logging",
                    Label = "Enable VPN Connection Audit Logging",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enables audit logging for all VPN connection attempts (successful and failed). Logs are written to the Windows Security event log. Default: disabled.",
                    Tags = ["vpn", "logging", "audit", "security", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All VPN connection events logged for audit; slight increase in event log volume.",
                    ApplyOps = [RegOp.SetDword(RasKey, "EnableConnectionLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "EnableConnectionLogging")],
                    DetectOps = [RegOp.CheckDword(RasKey, "EnableConnectionLogging", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-disable-split-tunneling",
                    Label = "Disable VPN Split Tunneling",
                    Category = "Network — Voip Quality",
                    Description =
                        "Forces all client traffic through the VPN tunnel (full-tunnel mode). Prevents clients from accessing the internet directly while connected. Default: split tunneling allowed.",
                    Tags = ["vpn", "split-tunnel", "security", "rras", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "All traffic routed through VPN; increases VPN server bandwidth but eliminates bypass risk.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableSplitTunnel", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableSplitTunnel")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableSplitTunnel", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-set-sa-lifetime",
                    Label = "Set IKEv2 SA Lifetime to 8 Hours",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets the IKEv2 security association (SA) lifetime to 8 hours (480 minutes). After expiry, the tunnel renegotiates keys. Default: 8 hours (may vary).",
                    Tags = ["vpn", "ikev2", "sa-lifetime", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VPN tunnel renegotiates keys every 8 hours; brief reconnection on rekey.",
                    ApplyOps = [RegOp.SetDword(IkeKey, "SALifeTimeMinutes", 480)],
                    RemoveOps = [RegOp.DeleteValue(IkeKey, "SALifeTimeMinutes")],
                    DetectOps = [RegOp.CheckDword(IkeKey, "SALifeTimeMinutes", 480)],
                },
                new TweakDef
                {
                    Id = "vpnras-enable-nap-enforcement",
                    Label = "Enable Network Access Protection for VPN",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enables NAP (Network Access Protection) health checks for VPN clients. Clients must meet health requirements (AV, firewall, updates) before being granted full access. Default: no NAP enforcement.",
                    Tags = ["vpn", "nap", "health-check", "security", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "VPN clients undergo health validation; non-compliant devices get restricted access.",
                    ApplyOps = [RegOp.SetDword(RasKey, "EnableNAP", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "EnableNAP")],
                    DetectOps = [RegOp.CheckDword(RasKey, "EnableNAP", 1)],
                },
                new TweakDef
                {
                    Id = "vpnras-disable-saved-credentials",
                    Label = "Prevent Saving VPN Credentials",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents users from saving VPN connection credentials. Users must enter credentials each time they connect. Reduces credential theft risk. Default: saving allowed.",
                    Tags = ["vpn", "credentials", "security", "credential-theft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Users re-enter VPN credentials each session; reduces risk of stored credential theft.",
                    ApplyOps = [RegOp.SetDword(RasKey, "DisableSavedCredentials", 1)],
                    RemoveOps = [RegOp.DeleteValue(RasKey, "DisableSavedCredentials")],
                    DetectOps = [RegOp.CheckDword(RasKey, "DisableSavedCredentials", 1)],
                },
            ];
    }

    // ── WcmConnectionPolicy ──
    private static class _WcmConnectionPolicy
    {
        private const string Wcm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wcmpol-disable-auto-connect",
                Label = "Disable WCM Auto-Connect to Non-Internet Networks",
                Category = "Network — Voip Quality",
                Description =
                    "Disables Windows Connection Manager automatic connection to networks when already connected to internet. Prevents unexpected Wi-Fi/mobile broadband connections that could create dual-homed exposure. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "auto-connect", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableAutoConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableAutoConnect")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableAutoConnect", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-minimize-connections",
                Label = "Minimize Simultaneous WCM Connections",
                Category = "Network — Voip Quality",
                Description =
                    "Instructs Windows Connection Manager to minimize the number of simultaneous connections to the internet, a domain, or a network. Prevents multi-homing unless required. Default: 0. Recommended: 3 (minimize, but allow manual overrides).",
                Tags = ["connection-manager", "network", "multi-home"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fMinimizeConnections", 3)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fMinimizeConnections")],
                DetectOps = [RegOp.CheckDword(Wcm, "fMinimizeConnections", 3)],
            },
            new TweakDef
            {
                Id = "wcmpol-prefer-wired-network",
                Label = "Prefer Wired over Wireless in WCM",
                Category = "Network — Voip Quality",
                Description =
                    "Instructs Windows Connection Manager to prefer wired Ethernet connections over Wi-Fi when both are available. Improves stability and throughput without forcing disconnect from Wi-Fi. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "wired", "wifi"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fPreferWiredNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fPreferWiredNetwork")],
                DetectOps = [RegOp.CheckDword(Wcm, "fPreferWiredNetwork", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-soft-disconnect",
                Label = "Enable WCM Soft Disconnect on Wireless",
                Category = "Network — Voip Quality",
                Description =
                    "Enables soft-disconnect behavior in WCM: instead of immediately dropping a wireless connection, the system waits for applications to switch before disconnecting. Reduces connection-drop disruptions. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "wifi", "disconnect"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fSoftDisconnectConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fSoftDisconnectConnections")],
                DetectOps = [RegOp.CheckDword(Wcm, "fSoftDisconnectConnections", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-disable-wlan-connectivity",
                Label = "Disable WLAN Connectivity via WCM Policy",
                Category = "Network — Voip Quality",
                Description =
                    "Disables WLAN (Wi-Fi) connections through the Windows Connection Manager policy. For wired-only or air-gapped workstations where wireless should be locked out at policy level. Default: 0. Recommended: 1 for restricted machines.",
                Tags = ["connection-manager", "wifi", "disable", "security", "wlan"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableWlanConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableWlanConnectivity")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableWlanConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-disable-wwan-connectivity",
                Label = "Disable WWAN/Mobile Broadband via WCM Policy",
                Category = "Network — Voip Quality",
                Description =
                    "Disables WWAN (mobile broadband/cellular) connections through the Windows Connection Manager policy. Prevents unexpected cellular data charges on enterprise devices. Default: 0. Recommended: 1 for non-mobile workstations.",
                Tags = ["connection-manager", "wwan", "mobile", "disable", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableWwanConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableWwanConnectivity")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableWwanConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-access-restrictions-on-reconnect",
                Label = "Apply WCM Access Restrictions on Reconnect",
                Category = "Network — Voip Quality",
                Description =
                    "Re-applies WCM connection-policy access restrictions when a managed network reconnects after being temporarily unavailable. Ensures policy enforcement is not bypassed by reconnection events. Default: 0. Recommended: 1.",
                Tags = ["connection-manager", "network", "policy", "reconnect"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fApplyAccessRestrictionsOnReconnect", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fApplyAccessRestrictionsOnReconnect")],
                DetectOps = [RegOp.CheckDword(Wcm, "fApplyAccessRestrictionsOnReconnect", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-block-wifi-when-ethernet",
                Label = "Block Wi-Fi When Ethernet Connected via WCM",
                Category = "Network — Voip Quality",
                Description =
                    "Prevents Windows from maintaining active Wi-Fi connections when a wired Ethernet connection is available. Reduces dual-homed exposure and possible split-tunnel routing issues. Default: not set. Recommended: 1.",
                Tags = ["connection-manager", "wifi", "ethernet", "network", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fDisableConnectivityForEthernet", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fDisableConnectivityForEthernet")],
                DetectOps = [RegOp.CheckDword(Wcm, "fDisableConnectivityForEthernet", 1)],
            },
            new TweakDef
            {
                Id = "wcmpol-no-local-policy-merge",
                Label = "Prevent Local WCM Policy Merge",
                Category = "Network — Voip Quality",
                Description =
                    "Prevents local administrator-configured WCM policies from being merged with domain Group Policy settings for WCM. Ensures only domain policy governs connection management. Default: 0. Recommended: 1 for managed environments.",
                Tags = ["connection-manager", "network", "group-policy", "management"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Wcm],
                ApplyOps = [RegOp.SetDword(Wcm, "fBlockLocalPolicyMerge", 1)],
                RemoveOps = [RegOp.DeleteValue(Wcm, "fBlockLocalPolicyMerge")],
                DetectOps = [RegOp.CheckDword(Wcm, "fBlockLocalPolicyMerge", 1)],
            },
        ];
    }

    // ── WcmWifiPolicy ──
    private static class _WcmWifiPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wcmpol-disable-soft-disconnect",
                    Label = "Disable WCM Soft-Disconnect from Wired",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from softly disconnecting from wired Ethernet when a preferred Wi-Fi connection becomes available. Keeps wired connections stable and avoids unexpected bandwidth switches. Default: soft-disconnect enabled. Recommended: 1 on workstations.",
                    Tags = ["wcm", "wifi", "wired", "disconnect", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wired connections stay active; WCM will not automatically switch to Wi-Fi when a preferred Wi-Fi is detected.",
                    ApplyOps = [RegOp.SetDword(Key, "fSoftDisconnectConnections", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fSoftDisconnectConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "fSoftDisconnectConnections", 0)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-simultaneous-connections",
                    Label = "Disable Simultaneous Wired+Wi-Fi Connections",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents Windows from maintaining simultaneous wired and Wi-Fi connections. When both are active, WCM disconnects the lower-priority adapter. Reduces split-routing and unintended traffic leakage. Default: simultaneous allowed. Recommended: 1.",
                    Tags = ["wcm", "wifi", "wired", "simultaneous", "routing", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Only one connection type (wired or Wi-Fi) is active at a time; eliminates multi-homed routing confusion.",
                    ApplyOps = [RegOp.SetDword(Key, "fMinimizeConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fMinimizeConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "fMinimizeConnections", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-wifi-hotspot-auto",
                    Label = "Disable Auto-Connect to Wi-Fi Hotspots",
                    Category = "Network — Voip Quality",
                    Description =
                        "Stops WCM from automatically connecting to Wi-Fi hotspots (e.g., paid hotspots, Wi-Fi Sense networks). Prevents unexpected connections to unvetted open networks. Default: auto-connect enabled. Recommended: 1.",
                    Tags = ["wcm", "wifi", "hotspot", "auto-connect", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows will not automatically join Wi-Fi hotspot networks; users must select manually.",
                    ApplyOps = [RegOp.SetDword(Key, "fBlockHotspotAutoConnect", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fBlockHotspotAutoConnect")],
                    DetectOps = [RegOp.CheckDword(Key, "fBlockHotspotAutoConnect", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-allow-manual-wifi-connect",
                    Label = "Allow Manual Wi-Fi Connection Despite Wired",
                    Category = "Network — Voip Quality",
                    Description =
                        "Permits users to manually connect to a Wi-Fi network even when an active wired Ethernet connection exists. Allows intentional dual-homing when needed (e.g., out-of-band management). Default: restricted. Recommended: 1 for power users.",
                    Tags = ["wcm", "wifi", "manual", "wired", "dual-home", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Users can manually join Wi-Fi while wired; WCM will not block the manual connection.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowManualConnectionWhileWiredConnected", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowManualConnectionWhileWiredConnected")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowManualConnectionWhileWiredConnected", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-preferred-wired-over-wifi",
                    Label = "Prefer Wired Connection Over Wi-Fi (Priority)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Configures WCM to always prefer wired Ethernet over Wi-Fi when both are available. Wired connections are prioritized in routing tables. Default: WCM balances based on cost and speed. Recommended: 1 for desktop workstations.",
                    Tags = ["wcm", "wired", "priority", "routing", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wired adapter routes are preferred; Wi-Fi remains as fallback only.",
                    ApplyOps = [RegOp.SetDword(Key, "PreferWiredOverWireless", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreferWiredOverWireless")],
                    DetectOps = [RegOp.CheckDword(Key, "PreferWiredOverWireless", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-cellular-as-fallback",
                    Label = "Disable Cellular as Wi-Fi Fallback",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from switching to a cellular data connection when Wi-Fi or wired connections become unavailable. Avoids unexpected mobile data consumption when tethered. Default: cellular allowed as fallback. Recommended: 1 on Wi-Fi-only policies.",
                    Tags = ["wcm", "cellular", "fallback", "mobile", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cellular data is never used as a fallback; connectivity drops if Wi-Fi/wired fail.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCellularFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCellularFallback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCellularFallback", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-block-non-domain-connections",
                    Label = "Block Non-Domain Network Connections on Domain Endpoints",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents domain-joined machines from connecting to non-domain (public/home) networks while connected to the corporate domain network. Stops traffic leakage to unmanaged networks. Default: not restricted. Recommended: 1 on domain endpoints.",
                    Tags = ["wcm", "domain", "network", "security", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Domain-joined machines cannot join public/home Wi-Fi while on the corporate network; strong defence against bridging attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNonDomainNetworks", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNonDomainNetworks")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNonDomainNetworks", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-auto-select-network-profile",
                    Label = "Disable Auto-Selection of Network Profile on Connect",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from automatically selecting the best network profile (Public/Private/Domain) upon connection. Requires users to explicitly choose the profile, reducing risk of miscategorising corporate networks as Public. Default: auto-select enabled.",
                    Tags = ["wcm", "network-profile", "public", "private", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Network profile is not auto-assigned; reduces risk of corporate network being set to Public with open file sharing.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoNetworkProfileSelection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoNetworkProfileSelection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoNetworkProfileSelection", 1)],
                },
                new TweakDef
                {
                    Id = "wcmpol-set-polling-interval-60s",
                    Label = "Set WCM Connection Polling Interval to 60 Seconds",
                    Category = "Network — Voip Quality",
                    Description =
                        "Adjusts the WCM service polling interval for connectivity changes to 60 seconds. Reduces WCM CPU wakeups on battery-powered laptops without significantly delaying reconnection. Default: ~5 seconds. Recommended: 60 for battery savings.",
                    Tags = ["wcm", "polling", "battery", "performance", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "WCM polls every 60s instead of ~5s; reduces wakeups and battery drain; reconnect after network switch takes up to 60s.",
                    ApplyOps = [RegOp.SetDword(Key, "ConnectionPollingIntervalSec", 60)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConnectionPollingIntervalSec")],
                    DetectOps = [RegOp.CheckDword(Key, "ConnectionPollingIntervalSec", 60)],
                },
                new TweakDef
                {
                    Id = "wcmpol-disable-managed-wifi-offload",
                    Label = "Disable WCM Managed Wi-Fi Offload",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents WCM from offloading Wi-Fi management to a cellular companion device or managed Wi-Fi radio. Keeps all connection decisions on the primary Windows networking stack. Default: offload allowed. Recommended: 1 on standard hardware.",
                    Tags = ["wcm", "wifi", "offload", "managed", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi management stays on Windows networking stack; no offloading to companion devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableManagedWifiOffload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableManagedWifiOffload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableManagedWifiOffload", 1)],
                },
            ];
    }

    // ── WebProxyAutoDiscoveryPolicy ──
    private static class _WebProxyAutoDiscoveryPolicy
    {
        private const string InetKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

        private const string WpadKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpad-disable-auto-detect",
                    Label = "WPAD: Disable Automatic Proxy Detection (WPAD Protocol)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AutoDetect=0 in WPAD policy. Disables Web Proxy Auto-Discovery Protocol (WPAD) which broadcasts DHCP/DNS queries to discover proxy configuration servers on the local network. WPAD is exploited in PoisonTap and similar attacks where an attacker's rogue DHCP or DNS server responds to WPAD queries, redirecting all HTTP/HTTPS traffic through an attacker-controlled proxy. Disabling WPAD and using explicit PAC file URLs or manual proxy configuration eliminates this attack surface.",
                    Tags = ["wpad", "proxy", "auto-detect", "security", "mitm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disables WPAD. Proxy configuration must be supplied via PAC file URL, manual proxy settings, or Group Policy. Breaks environments relying on WPAD for zero-config proxy discovery.",
                    ApplyOps = [RegOp.SetDword(WpadKey, "WpadOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(WpadKey, "WpadOverride")],
                    DetectOps = [RegOp.CheckDword(WpadKey, "WpadOverride", 1)],
                },
                new TweakDef
                {
                    Id = "wpad-disable-pac-script-download-prompt",
                    Label = "WPAD: Suppress PAC File Download Confirmation Prompt",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DisableProxyAutoConfigUrlRequest=0 in Internet Settings. Suppresses the Internet Explorer / WinINet PAC file download confirmation prompt that asks users to allow or deny the download. In enterprise proxy environments, the PAC file is a managed IT component; user confirmation prompts are unnecessary and cause initial connection delays. This setting prevents the prompt and allows PAC file auto-download without user interaction.",
                    Tags = ["wpad", "pac", "prompt", "enterprise", "ux"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "PAC file is downloaded silently. Security benefit of the prompt is marginal since PAC URL comes from Group Policy in managed environments.",
                    ApplyOps = [RegOp.SetDword(InetKey, "DisableProxyAutoConfigUrlRequest", 0)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "DisableProxyAutoConfigUrlRequest")],
                    DetectOps = [RegOp.CheckDword(InetKey, "DisableProxyAutoConfigUrlRequest", 0)],
                },
                new TweakDef
                {
                    Id = "wpad-enable-auto-configuration",
                    Label = "WPAD: Enable Automatic Configuration Script (PAC) Support",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets AutoConfigUrl=1 in Internet Settings policy. Enables enforced application of an automatic configuration script (PAC file) URL from Group Policy. This ensures the PAC file URL is deployed to all managed workstations and cannot be overridden by end users. Managed PAC enforcement is the standard enterprise proxy deployment mechanism: all applications using the WinHTTP/WinINet stack will use the centrally managed PAC file for proxy decisions.",
                    Tags = ["wpad", "pac", "auto-config", "proxy", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables PAC file enforcement. The PAC URL must be separately configured via the Proxy GPO. This setting only enables the PAC mechanism, not a specific URL.",
                    ApplyOps = [RegOp.SetDword(InetKey, "AutoConfigUrl", 1)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "AutoConfigUrl")],
                    DetectOps = [RegOp.CheckDword(InetKey, "AutoConfigUrl", 1)],
                },
                new TweakDef
                {
                    Id = "wpad-enable-winhttp-proxy",
                    Label = "WPAD: Enable WinHTTP Proxy Inheritance from IE Settings",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets EnableLegacyAutoProxyFeatures=1 in Internet Settings. Enables WinHTTP applications (background services, .NET, PowerShell, Windows Update) to inherit the proxy configuration from the IE/WinINet machine proxy settings. Without this setting, WinHTTP applications (which don't read from the IE proxy registry directly) may bypass the corporate proxy entirely. Enabling inheritance ensures background system processes also route through the corporate proxy.",
                    Tags = ["wpad", "winhttp", "proxy", "inheritance", "background"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WinHTTP services will use the corporate proxy. Applications with hardcoded direct access (e.g., Windows Update might bypass proxy) are unaffected.",
                    ApplyOps = [RegOp.SetDword(InetKey, "EnableLegacyAutoProxyFeatures", 1)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "EnableLegacyAutoProxyFeatures")],
                    DetectOps = [RegOp.CheckDword(InetKey, "EnableLegacyAutoProxyFeatures", 1)],
                },
                new TweakDef
                {
                    Id = "wpad-set-proxy-timeout",
                    Label = "WPAD: Set Proxy Connection Timeout to 10 Seconds",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets ConnectTimeout=10000 in Internet Settings. Sets the proxy server connection timeout to 10,000 ms (10 seconds). The default WinINet proxy connection timeout is 60 seconds. On a failed or unavailable proxy server, applications wait 60 seconds before failing over to direct connection or returning a timeout error. Reducing to 10 seconds allows applications to detect proxy failures faster and improves user experience when the proxy server is temporarily unreachable.",
                    Tags = ["wpad", "proxy", "timeout", "performance", "failover"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Proxy timeout is reduced to 10 seconds. On slow proxy servers, connections taking >10s to establish may time out unnecessarily. Adjust based on proxy infrastructure latency.",
                    ApplyOps = [RegOp.SetDword(InetKey, "ConnectRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "ConnectRetries")],
                    DetectOps = [RegOp.CheckDword(InetKey, "ConnectRetries", 3)],
                },
                new TweakDef
                {
                    Id = "wpad-disable-ftp-proxy",
                    Label = "WPAD: Disable FTP Proxy Support in WinINet",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets FtpProxyEnable=0 in Internet Settings. Disables FTP proxy support in WinINet, preventing HTTP-tunneled FTP transfers through the corporate proxy. FTP is unencrypted and transmits credentials in plaintext. Using FTP through a proxy allows users to bypass download controls (corporate proxies can't inspect FTP payload). Modern FTP use cases should be replaced by HTTPS/SFTP. Disabling FTP proxy prevents FTP traffic from appearing to be authorized by routing through the proxy.",
                    Tags = ["wpad", "ftp", "proxy", "security", "plaintext"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "FTP proxy is disabled. FTP connections (already insecure by design) will be blocked by the proxy. Users needing FTP for legacy transfer should use SFTP instead.",
                    ApplyOps = [RegOp.SetDword(InetKey, "FtpProxyEnable", 0)],
                    RemoveOps = [RegOp.DeleteValue(InetKey, "FtpProxyEnable")],
                    DetectOps = [RegOp.CheckDword(InetKey, "FtpProxyEnable", 0)],
                },
            ];
    }

    // ── WifiConnectionPolicy ──
    private static class _WifiConnectionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wificonn-disable-softap",
                    Label = "Disable Windows Wi-Fi SoftAP (Software Access Point)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents the creation of a software access point using the Wireless Hosted Network API (SoftAP), blocking use of this machine as a wireless hotspot by applications or user scripts.",
                    Tags = ["wi-fi", "soft-ap", "hosted-network", "hotspot", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SoftAP / Wireless Hosted Network disabled; Wi-Fi adapter cannot be used as a software hotspot.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableSoftAP", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableSoftAP")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableSoftAP", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-wifi-sense-open",
                    Label = "Disable Wi-Fi Sense Connectivity to Open Suggested Hotspots",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents Wi-Fi Sense from automatically connecting this machine to open hotspots recommended by Microsoft's crowd-sourced network database, eliminating silent connections to unknown public Wi-Fi.",
                    Tags = ["wi-fi", "wifi-sense", "open-hotspot", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi Sense open hotspot auto-connect disabled; machine does not join unknown crowd-sourced networks.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoConnectAllowedOEM", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoConnectAllowedOEM")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoConnectAllowedOEM", 0)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-profile-sync-to-cloud",
                    Label = "Disable Wi-Fi Profile Synchronisation to Microsoft Cloud",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents Wi-Fi profiles (network names, credentials) from being synchronised to a Microsoft account in the cloud, ensuring saved Wi-Fi passwords remain local-only and are not accessible from other devices.",
                    Tags = ["wi-fi", "profile-sync", "microsoft-account", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi profile sync to Microsoft cloud disabled; credentials stay local-only.",
                    ApplyOps = [RegOp.SetDword(Key, "WiFiConfigSyncDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WiFiConfigSyncDisabled")],
                    DetectOps = [RegOp.CheckDword(Key, "WiFiConfigSyncDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-block-forget-network",
                    Label = "Block Standard Users from Forgetting Wi-Fi Networks",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents standard users from using the Forget Network option in the Wi-Fi settings flyout, preserving IT-configured enterprise Wi-Fi profiles from being accidentally deleted.",
                    Tags = ["wi-fi", "forget-network", "profile", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi 'Forget' blocked for standard users; IT-managed profiles cannot be deleted by end users.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockForgetNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockForgetNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockForgetNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-require-wpa2-minimum",
                    Label = "Require WPA2 or Higher Authentication Standard",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enforces a minimum WPA2 authentication standard for all Wi-Fi connections, blocking connections to WEP or open networks that can be trivially intercepted.",
                    Tags = ["wi-fi", "wpa2", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WPA2 minimum enforced; WEP and open Wi-Fi networks blocked from connection.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireWPA2Minimum", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA2Minimum")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireWPA2Minimum", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-random-mac",
                    Label = "Disable Randomised MAC Address for Wi-Fi (Enterprise Mode)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Disables MAC address randomisation on wireless connections to ensure consistent hardware MAC address presentation on corporate networks, which is required by 802.1X/RADIUS authentication and MAC-based network admission control.",
                    Tags = ["wi-fi", "mac-randomisation", "802.1x", "radius", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MAC randomisation disabled; real hardware MAC used — required for 802.1X and RADIUS MAC-based auth.",
                    ApplyOps = [RegOp.SetDword(Key, "RandomizeHardwareAddresses", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RandomizeHardwareAddresses")],
                    DetectOps = [RegOp.CheckDword(Key, "RandomizeHardwareAddresses", 0)],
                },
                new TweakDef
                {
                    Id = "wificonn-disable-wcm-telemetry",
                    Label = "Disable Wireless Connection Manager Telemetry",
                    Category = "Network — Voip Quality",
                    Description =
                        "Prevents the Windows Connection Manager (WCM) from sending Wi-Fi connection quality metrics and network preference telemetry to Microsoft, protecting corporate network topology information from cloud disclosure.",
                    Tags = ["wi-fi", "wcm", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WCM telemetry to Microsoft disabled; connection metrics and network preference data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWCMTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWCMTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWCMTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "wificonn-log-connection-events",
                    Label = "Log Wireless Connection and Authentication Events",
                    Category = "Network — Voip Quality",
                    Description =
                        "Enables detailed logging of wireless connection establishment, authentication success/failure, and disconnection events in the Microsoft-Windows-WLAN-AutoConfig operational log for security auditing.",
                    Tags = ["wi-fi", "audit", "connection-log", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi connection and auth events logged in WLAN-AutoConfig operational log; auditable SSID history.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableWifiConnectionEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableWifiConnectionEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableWifiConnectionEventLogging", 1)],
                },
            ];
    }

    // ── WifiNetworking ──
    private static class _WifiNetworking
    {
        private const string WifiPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy";

        private const string WifiService = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WlanSvc\Parameters";

        private const string WiFiSense = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config";

        private const string WiFiSensePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager";

        private const string WlanProfile = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\DefaultMediaCost";

        private const string NdisTcp = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wifi-disable-wifi-sense-policy",
                Label = "Disable Wi-Fi Sense via Group Policy",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wifi", "wi-fi sense", "policy", "auto connect"],
                Description =
                    "Enforces Wi-Fi Sense disable through the machine policy path, preventing "
                    + "users or OEMs from re-enabling it through device settings.",
                ApplyOps = [RegOp.SetDword(WiFiSensePolicy, "AutoConnectAllowedOEM", 0)],
                RemoveOps = [RegOp.DeleteValue(WiFiSensePolicy, "AutoConnectAllowedOEM")],
                DetectOps = [RegOp.CheckDword(WiFiSensePolicy, "AutoConnectAllowedOEM", 0)],
            },
            new TweakDef
            {
                Id = "wifi-disable-hotspot2-roaming",
                Label = "Disable Hotspot 2.0 / Passpoint Auto-Connect",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wifi", "hotspot 2.0", "passpoint", "auto connect", "roaming"],
                Description =
                    "Disables automatic connection to Hotspot 2.0 (Passpoint) networks. "
                    + "These networks authenticate using your Microsoft Account credentials "
                    + "without user confirmation. fBlockNonDomain=0 preserves corporate policies.",
                ApplyOps = [RegOp.SetDword(WiFiSense, "WiFiSharingEnabled", 0)],
                RemoveOps = [RegOp.SetDword(WiFiSense, "WiFiSharingEnabled", 1)],
                DetectOps = [RegOp.CheckDword(WiFiSense, "WiFiSharingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wifi-disable-random-mac",
                Label = "Disable Random Hardware MAC Address per Network",
                Category = "Network — Voip Quality",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wifi", "mac randomization", "mac address", "privacy"],
                Description =
                    "Disables MAC address randomisation for Wi-Fi connections. Some network "
                    + "infrastructure (captive portals, MAC-filtered access points) breaks "
                    + "when the MAC changes between connections.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WlanSvc\Interfaces", "RandomMacAddressEnabled", 0)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WlanSvc\Interfaces", "RandomMacAddressEnabled", 1)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WlanSvc\Interfaces", "RandomMacAddressEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wifi-disable-nla-wifi",
                Label = "Disable Network Location Awareness Auto-Detect for Wi-Fi",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wifi", "nla", "network awareness", "location"],
                Description =
                    "Disables the NLA service probing HTTPS connectivity to identify the "
                    + "network type. NLA probes can cause delays on startup and are sometimes "
                    + "mistaken for telemetry. Reduces connection establishment time.",
                ApplyOps = [RegOp.SetDword(NdisTcp, "DisabledComponents", 0x20)],
                RemoveOps = [RegOp.DeleteValue(NdisTcp, "DisabledComponents")],
                DetectOps = [RegOp.CheckDword(NdisTcp, "DisabledComponents", 0x20)],
            },
            new TweakDef
            {
                Id = "wifi-set-wifi-as-metered",
                Label = "Set Wi-Fi Connections as Metered (Save Data)",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wifi", "metered", "data usage", "background"],
                Description =
                    "Marks Wi-Fi connections as metered by default, restricting background "
                    + "data usage, Windows Update downloads, and app background refresh. "
                    + "Useful on mobile hotspots or limited data plans.",
                ApplyOps = [RegOp.SetDword(WlanProfile, "WiFi", 2)],
                RemoveOps = [RegOp.SetDword(WlanProfile, "WiFi", 1)],
                DetectOps = [RegOp.CheckDword(WlanProfile, "WiFi", 2)],
            },
            new TweakDef
            {
                Id = "wifi-disable-bluetooth-interference-avoidance",
                Label = "Disable Wi-Fi / Bluetooth Coexistence Mode",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wifi", "bluetooth", "coexistence", "2.4ghz", "performance"],
                Description =
                    "Disables the 2.4 GHz band Wi-Fi / Bluetooth coexistence mitigation that "
                    + "reduces Wi-Fi throughput when Bluetooth is active. Useful when Bluetooth "
                    + "is never or rarely used, preventing unnecessary throttling.",
                ApplyOps = [RegOp.SetDword(WifiService, "EnableWiFiCoexistenceOptimization", 0)],
                RemoveOps = [RegOp.DeleteValue(WifiService, "EnableWiFiCoexistenceOptimization")],
                DetectOps = [RegOp.CheckDword(WifiService, "EnableWiFiCoexistenceOptimization", 0)],
            },
            new TweakDef
            {
                Id = "wifi-enable-802-11d",
                Label = "Enable 802.11d Multi-Country Regulatory Info",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wifi", "802.11d", "regulatory", "country", "channels"],
                Description =
                    "Enables 802.11d extension so the Wi-Fi adapter can read and honour "
                    + "regulatory domain information broadcast by access points. "
                    + "Improves channel availability in multi-country environments.",
                ApplyOps = [RegOp.SetDword(WifiService, "Enable80211d", 1)],
                RemoveOps = [RegOp.DeleteValue(WifiService, "Enable80211d")],
                DetectOps = [RegOp.CheckDword(WifiService, "Enable80211d", 1)],
            },
            new TweakDef
            {
                Id = "wifi-disable-peer-to-peer",
                Label = "Disable Wi-Fi Peer-to-Peer (Wi-Fi Direct Sharing)",
                Category = "Network — Voip Quality",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wifi", "peer-to-peer", "wifi direct", "security"],
                Description =
                    "Disables Wi-Fi Direct peer-to-peer connections used by features like "
                    + "Nearby Sharing and Cast. Reduces attack surface on public networks where "
                    + "malicious P2P requests could target the system.",
                ApplyOps = [RegOp.SetDword(WifiPolicy, "fAllowAutoConnectToWiFiSenseHotspots", 0)],
                RemoveOps = [RegOp.DeleteValue(WifiPolicy, "fAllowAutoConnectToWiFiSenseHotspots")],
                DetectOps = [RegOp.CheckDword(WifiPolicy, "fAllowAutoConnectToWiFiSenseHotspots", 0)],
            },
        ];
    }

    // ── WinHttpProxyPolicy ──
    private static class _WinHttpProxyPolicy
    {
        private const string WhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinHttp";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "whttp-disable-wpad",
                    Label = "Disable WPAD Auto-Detection",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DisableWpad=1 to disable Web Proxy Auto-Discovery (WPAD) for WinHTTP connections system-wide. Prevents the WPAD DNS and DHCP queries that can leak internal network topology. Default: 0 (WPAD enabled).",
                    Tags = ["winhttp", "wpad", "proxy", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "WPAD disabled; system will not send WPAD DNS queries. May break auto-proxy environments.",
                    ApplyOps = [RegOp.SetDword(WhKey, "DisableWpad", 1)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "DisableWpad")],
                    DetectOps = [RegOp.CheckDword(WhKey, "DisableWpad", 1)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-auto-proxy",
                    Label = "Disable WinHTTP Automatic Proxy",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets EnableAutoProxyResultCaching=0 to disable automatic proxy detection and result caching in WinHTTP. Forces applications using WinHTTP to use only explicitly configured proxies, blocking all auto-proxy behaviour.",
                    Tags = ["winhttp", "auto proxy", "caching", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Auto-proxy caching disabled; no automatic proxy discovery on WinHTTP calls.",
                    ApplyOps = [RegOp.SetDword(WhKey, "EnableAutoProxyResultCaching", 0)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "EnableAutoProxyResultCaching")],
                    DetectOps = [RegOp.CheckDword(WhKey, "EnableAutoProxyResultCaching", 0)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-proxy-bypass-local",
                    Label = "Prevent Bypassing Proxy for Local Addresses",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets ProxyBypassLocal=0 to ensure all connections, including those to local network hosts, go through the configured proxy. Default: 1 (local addresses bypass proxy). Useful for strict audit trails.",
                    Tags = ["winhttp", "proxy bypass", "local network", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Local Windows communication routed through proxy; may slow intranet access.",
                    ApplyOps = [RegOp.SetDword(WhKey, "ProxyBypassLocal", 0)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "ProxyBypassLocal")],
                    DetectOps = [RegOp.CheckDword(WhKey, "ProxyBypassLocal", 0)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-proxy-auto-config-url",
                    Label = "Block WinHTTP Auto-Config URL",
                    Category = "Network — Voip Quality",
                    Description =
                        "Removes the AutoConfigURL value under WinHttp policy to ensure no Proxy Auto-Configuration (PAC) file URL is enforced through Group Policy. Clears any admin-deployed auto-config URL that might route traffic unexpectedly.",
                    Tags = ["winhttp", "pac file", "auto config", "proxy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "PAC file URL removed from policy; proxy detection falls back to manual or DHCP.",
                    ApplyOps = [RegOp.DeleteValue(WhKey, "AutoConfigURL")],
                    RemoveOps = [],
                    DetectOps = [RegOp.CheckMissing(WhKey, "AutoConfigURL")],
                },
                new TweakDef
                {
                    Id = "whttp-set-connection-timeout",
                    Label = "Set WinHTTP Connection Timeout (30 s)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DefaultConnectionSettings to enforce a 30-second connection timeout for WinHTTP calls via policy. Prevents hung proxy connections from blocking system services indefinitely. Default: no policy-enforced timeout.",
                    Tags = ["winhttp", "timeout", "connection", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinHTTP calls time out after 30 s; prevents indefinite stalls on broken proxy paths.",
                    ApplyOps = [RegOp.SetDword(WhKey, "ConnectionTimeOut", 30000)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "ConnectionTimeOut")],
                    DetectOps = [RegOp.CheckDword(WhKey, "ConnectionTimeOut", 30000)],
                },
                new TweakDef
                {
                    Id = "whttp-set-receive-timeout",
                    Label = "Set WinHTTP Receive Timeout (30 s)",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets ReceiveTimeOut=30000 (ms) to enforce a 30-second receive timeout for WinHTTP responses. Prevents system services from waiting indefinitely for a slow or unresponsive proxy to deliver a response body.",
                    Tags = ["winhttp", "timeout", "receive", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinHTTP receive operations time out after 30 s; protects against stalled downloads.",
                    ApplyOps = [RegOp.SetDword(WhKey, "ReceiveTimeOut", 30000)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "ReceiveTimeOut")],
                    DetectOps = [RegOp.CheckDword(WhKey, "ReceiveTimeOut", 30000)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-ssl-vulnerability-check",
                    Label = "Disable SSL Renegotiation Downgrade in WinHTTP",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets StaticProxyFirewall=1 to tell WinHTTP to treat the proxy connection as a static firewall proxy, disabling reflective SSL renegotiation probes that can expose protocol downgrade vulnerabilities.",
                    Tags = ["winhttp", "ssl", "security", "proxy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Static proxy mode; prevents SSL downgrade probe via proxy renegotiation.",
                    ApplyOps = [RegOp.SetDword(WhKey, "StaticProxyFirewall", 1)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "StaticProxyFirewall")],
                    DetectOps = [RegOp.CheckDword(WhKey, "StaticProxyFirewall", 1)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-auth-scheme-ntlm",
                    Label = "Restrict WinHTTP to Secure Auth Schemes",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets HardCodedProxySetting=2 to prevent WinHTTP from negotiating weaker proxy authentication schemes (e.g., Basic) and limits it to NTLM/Negotiate. Reduces credential exposure across untrusted proxies.",
                    Tags = ["winhttp", "auth", "ntlm", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "WinHTTP uses only NTLM/Negotiate auth with proxy; Basic auth rejected.",
                    ApplyOps = [RegOp.SetDword(WhKey, "HardCodedProxySetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "HardCodedProxySetting")],
                    DetectOps = [RegOp.CheckDword(WhKey, "HardCodedProxySetting", 2)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-redirect-follow",
                    Label = "Disable WinHTTP Automatic Redirect Follow",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets MaxConnections=0 under proxy policy to prevent WinHTTP from automatically following HTTP redirects through the proxy. Forces applications to handle redirects explicitly, reducing proxy-traversal SSRF exposure.",
                    Tags = ["winhttp", "redirect", "security", "ssrf", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Automatic redirects blocked at WinHTTP layer; apps must handle redirect responses.",
                    ApplyOps = [RegOp.SetDword(WhKey, "EnableProxyAuthorization", 0)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "EnableProxyAuthorization")],
                    DetectOps = [RegOp.CheckDword(WhKey, "EnableProxyAuthorization", 0)],
                },
                new TweakDef
                {
                    Id = "whttp-disable-wpad-dns-lookup",
                    Label = "Disable WPAD DNS Lookup Fallback",
                    Category = "Network — Voip Quality",
                    Description =
                        "Sets DisableWpadLookup=1 to disable the DNS-based fallback mechanism used by WPAD (queries for 'wpad.<domain>') when DHCP-based WPAD fails. Prevents DNS-based WPAD name collision attacks.",
                    Tags = ["winhttp", "wpad", "dns", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "DNS WPAD queries blocked; eliminates WPAD name-collision DNS hijack vector.",
                    ApplyOps = [RegOp.SetDword(WhKey, "DisableWpadLookup", 1)],
                    RemoveOps = [RegOp.DeleteValue(WhKey, "DisableWpadLookup")],
                    DetectOps = [RegOp.CheckDword(WhKey, "DisableWpadLookup", 1)],
                },
            ];
    }

    // ── WinInetPolicy ──
    private static class _WinInetPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wininet-enable-enhanced-protected-mode",
                Label = "Enable Enhanced Protected Mode for Internet Explorer and WinInet Clients",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enhanced Protected Mode extends the standard Protected Mode sandbox by running browser tab processes in a 64-bit AppContainer with additional restrictions on access to sensitive user files network resources and system components. Enhanced Protected Mode significantly increases the effort required for malicious web content to escape the browser sandbox and access system resources or user data. WinInet applications that use the Internet Explorer rendering engine or MSHTML benefit from Enhanced Protected Mode when it is enabled through policy. The 64-bit AppContainer used by Enhanced Protected Mode prevents many exploit techniques that rely on 32-bit process assumptions and low-integrity level bypass methods. Organizations should test web application compatibility with Enhanced Protected Mode before deploying it as some ActiveX controls and legacy add-ins may not function in the AppContainer sandbox. Security vendors analyze Enhanced Protected Mode bypass techniques as high-severity findings recognizing the importance of the protection it provides against web-based exploitation.",
                Tags = ["enhanced-protected-mode", "wininet", "sandbox", "ie-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableEnhancedProtectedMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEnhancedProtectedMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEnhancedProtectedMode", 1)],
            },
            new TweakDef
            {
                Id = "wininet-enforce-tls-protocol-restriction",
                Label = "Enforce TLS Protocol Version Restrictions to Disable Legacy SSL and TLS Versions",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Restricting TLS protocol versions through WinInet policy disables SSL 3.0 TLS 1.0 and TLS 1.1 while enabling TLS 1.2 and TLS 1.3 preventing connections to servers that use cryptographically weak protocols vulnerable to known attacks. The EnabledProtocols policy value uses a bitmask where bit 2048 (0x800) enables TLS 1.2 and bit 640 (0x280) combined gives TLS 1.2 only; the value 2688 (0xA80) enables both TLS 1.2 and TLS 1.3 exclusively. SSL 3.0 is vulnerable to POODLE attacks and all TLS 1.0 implementations are vulnerable to BEAST attacks that allow active network interception to decrypt session traffic. Disabling TLS 1.0 and 1.1 through WinInet policy affects all applications that use the WinInet or WinHTTP API stack ensuring consistent protocol restrictions across the user mode networking layer. Organizations must verify that all internal web services and application dependencies support TLS 1.2 before deploying TLS 1.0 and 1.1 restrictions to avoid breaking legitimate service connectivity. External service dependencies should also be audited for TLS protocol support with a migration plan for services that do not yet support TLS 1.2.",
                Tags = ["tls", "protocol-restriction", "ssl-disable", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnabledProtocols", 2688)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnabledProtocols")],
                DetectOps = [RegOp.CheckDword(Key, "EnabledProtocols", 2688)],
            },
            new TweakDef
            {
                Id = "wininet-configure-proxy-bypass-for-local",
                Label = "Configure Proxy Bypass List to Allow Direct Access to Local Intranet Resources",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Configuring the proxy bypass list to include the local intranet bypass designator ensures that connections to local intranet hosts do not traverse the external proxy server preserving local network performance and avoiding round-trip latency for intranet resources. The ProxyOverride value with the string value of `<local>` instructs WinInet to bypass the configured proxy server for all hosts resolved to private IP address ranges and single-label DNS names. Without the local bypass designation all intranet traffic is routed through the proxy server which can cause failures for intranet applications that are not designed for proxy traversal and increases proxy server load unnecessarily. The proxy bypass configuration should be consistent with the organization's network topology ensuring that intranet resources are correctly identified as local. Custom bypass entries for specific intranet domain names or IP subnets can be added to the ProxyOverride value in addition to the `<local>` designation for more granular control. Proxy configuration testing should verify that internal resource access is not routed through external proxy infrastructure to prevent inadvertent data exposure to proxy service providers.",
                Tags = ["proxy-bypass", "local-intranet", "wininet", "network-configuration", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "ProxyOverride", "<local>")],
                RemoveOps = [RegOp.DeleteValue(Key, "ProxyOverride")],
                DetectOps = [RegOp.CheckString(Key, "ProxyOverride", "<local>")],
            },
            new TweakDef
            {
                Id = "wininet-disable-certificate-revocation-soft-fail",
                Label = "Disable Soft-Fail Certificate Revocation Checking to Enforce Hard Revocation Policy",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Soft-fail certificate revocation checking allows TLS connections to proceed even when the certificate revocation check cannot be completed due to an unavailable OCSP responder or CRL distribution point creating a vulnerability window where connections using revoked certificates are not blocked. Enforcing hard revocation checking ensures that TLS connections are refused when certificate revocation status cannot be verified giving attackers no benefit from making the revocation infrastructure unavailable. Hard revocation checking may cause connectivity failures when revocation infrastructure is unreachable so organizations should ensure OCSP responders and CRL distribution points are highly available before deploying hard revocation policy. Certificate pinning and OCSP stapling provide alternatives to traditional revocation checking that are not subject to soft-fail vulnerabilities and should be preferred for high-security applications. The combination of soft-fail revocation and short-lived certificates is a common approach to managing high-availability requirements while maintaining security guarantees. Organizations should evaluate whether OCSP stapling support in their web infrastructure allows hard revocation checking without availability impacts.",
                Tags = ["certificate-revocation", "hard-fail", "tls-security", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "CertificateRevocationHardFail", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "CertificateRevocationHardFail")],
                DetectOps = [RegOp.CheckDword(Key, "CertificateRevocationHardFail", 1)],
            },
            new TweakDef
            {
                Id = "wininet-enable-https-strict-transport-security",
                Label = "Enable HTTP Strict Transport Security Enforcement in WinInet Stack",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "HTTP Strict Transport Security enforcement ensures that HSTS headers received from web servers are respected by WinInet-based applications preventing downgrade attacks that redirect HTTPS connections to HTTP before the server-side redirect can enforce HTTPS. HSTS protection eliminates a class of SSL stripping attacks in which a network adversary intercepts the initial HTTP request before the server-side 301 redirect upgrades the connection to HTTPS. WinInet HSTS enforcement builds the transport security list from HSTS headers and preloaded HSTS site lists to provide protection for sites whether or not the browser has previously visited them. Applications that use WinInet for HTTPS communication benefit from HSTS enforcement consistently across all sites that have deployed HSTS including sites on the HSTS preload list. Organizations running internal HTTPS services should deploy HSTS headers on those services to benefit from HSTS caching on client devices. HSTS should be combined with HTTPS-only policies on internal web servers to create a comprehensive transport security posture.",
                Tags = ["hsts", "https-enforcement", "tls-downgrade", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableHSTS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableHSTS")],
                DetectOps = [RegOp.CheckDword(Key, "EnableHSTS", 1)],
            },
            new TweakDef
            {
                Id = "wininet-block-mixed-content-navigation",
                Label = "Block Navigation to Mixed HTTP and HTTPS Content in WinInet Applications",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Mixed content navigation occurs when HTTPS pages include or link to HTTP content creating security weaknesses where the unencrypted HTTP content can be intercepted or tampered with by network adversaries even though the page is nominally loaded over HTTPS. Blocking mixed content prevents HTTPS pages from loading insecure sub-resources including scripts stylesheets and images over HTTP which could enable cross-site scripting via content injection. Mixed active content including scripts and iframes is strictly blocked by default in modern browsers but mixed passive content including images and audio is often allowed with warnings. Enforcing strict mixed content blocking through WinInet policy ensures that all WinInet-based applications not just modern browsers refuse to load HTTP sub-resources on HTTPS pages. Organizations running internal HTTPS web applications should ensure all referenced assets are served over HTTPS to avoid compatibility issues when mixed content blocking is enforced. Mixed content blocking policy should be tested against all critical web applications before enforcement to identify applications that need to be updated to serve all content over HTTPS.",
                Tags = ["mixed-content", "https-enforcement", "wininet", "content-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockMixedContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMixedContent")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMixedContent", 1)],
            },
            new TweakDef
            {
                Id = "wininet-disable-automatic-proxy-detection",
                Label = "Disable Automatic Proxy Detection and WPAD Protocol in WinInet Stack",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Automatic proxy detection using Web Proxy Auto-Discovery Protocol allows network infrastructure to automatically configure proxy settings for client systems by broadcasting proxy configuration file locations through DNS and DHCP which can be exploited by attackers. WPAD attacks allow an attacker on the local network to provide a malicious proxy auto-configuration script that redirects traffic through a controlled proxy for interception and credential harvesting. Disabling automatic proxy detection prevents WPAD-based proxy configuration attacks while requiring that proxy settings be explicitly configured through Group Policy ensuring that proxy configuration is under organizational control. WPAD is particularly dangerous on untrusted networks such as conference WiFi or hotel networks where attackers can respond to WPAD DNS queries with malicious proxy configurations. Organizations should configure proxy settings through Group Policy using explicit proxy server addresses or PAC file URLs rather than relying on WPAD auto-detection. Systems that connect to guest WiFi or external networks should be specifically evaluated for WPAD attack exposure if automatic proxy detection is not disabled.",
                Tags = ["wpad", "proxy-detection", "proxy-auto-config", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoProxyDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProxyDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoProxyDetection", 1)],
            },
            new TweakDef
            {
                Id = "wininet-enforce-certificate-error-handling",
                Label = "Enforce Strict Certificate Error Handling to Prevent User Override of TLS Errors",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Strict certificate error handling prevents users from clicking through TLS certificate errors including expired certificates invalid hostnames and untrusted certificate authority errors which are common indicators of man-in-the-middle attacks. Allowing user override of certificate errors creates a human-factor vulnerability where social engineering can convince users to accept illegitimate certificates by training them that certificate errors are acceptable to bypass. Organizations should enforce certificate error handling through policy to ensure that TLS certificate validation failures result in connection refusal rather than user prompts. Certificate transparency monitoring and certificate pinning are complementary controls that detect certificate authority misissuance which would not be caught by standard certificate validation. Internal applications should use certificates issued by the organization's trusted PKI infrastructure to avoid generating false-positive certificate errors on endpoints configured with enterprise certificate authority trust. The strictness of certificate error handling should be calibrated to the organization's risk tolerance with stricter enforcement appropriate for high-security environments.",
                Tags = ["certificate-errors", "tls-enforcement", "user-bypass", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceCertificateErrorHandling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceCertificateErrorHandling")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceCertificateErrorHandling", 1)],
            },
            new TweakDef
            {
                Id = "wininet-restrict-third-party-cookie-access",
                Label = "Restrict Third-Party Cookie Access to Reduce Cross-Site Tracking in WinInet",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Restricting third-party cookie access through WinInet policy prevents advertising networks and tracking services from setting and reading cookies across different websites significantly reducing cross-site tracking of user browsing behavior. Third-party cookies are used to build user profiles based on browsing behavior across multiple sites and the data collected may include sensitive information about user interests health conditions or financial situations. Blocking third-party cookies is consistent with increasing regulatory requirements under GDPR and similar privacy regulations that require consent for tracking technologies. Browser vendors have already begun restrictive third-party cookie policies and WinInet policy enforcement ensures consistent behavior across applications that use the WinInet API stack. Organizations should evaluate third-party cookie restrictions against web application single sign-on functionality as some authentication flows use third-party cookies for session management. The impact on SAML and OAuth authentication flows should be tested specifically when implementing third-party cookie restrictions.",
                Tags = ["third-party-cookies", "cross-site-tracking", "privacy", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictThirdPartyCookies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictThirdPartyCookies")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictThirdPartyCookies", 1)],
            },
            new TweakDef
            {
                Id = "wininet-disable-legacy-security-zones-modification",
                Label = "Prevent User Modification of WinInet Security Zone Configuration",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Preventing user modification of security zone configuration ensures that the organization's WinInet security zone policies remain in effect and cannot be weakened by users who move sites to less restrictive zones to make blocked content accessible. Security zones control what capabilities web content has when executing in WinInet-based applications and user-added trusted sites with low security settings create exploitation opportunities for malicious websites. Malware and phishing attacks sometimes instruct victims to add malicious sites to the Trusted Sites zone to enable ActiveX installation or bypass security prompts. Locking security zone configuration through policy ensures that only administrators can add sites to the Trusted Sites zone and modify zone security settings. Organizations should audit the zone configuration regularly to ensure that the sites in the Trusted Sites zone are legitimate and still require trusted access. User requests to add sites to the Trusted Sites zone should go through a security review process before being added through centrally managed Group Policy settings.",
                Tags = ["security-zones", "trusted-sites", "user-restriction", "wininet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableZoneModification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableZoneModification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableZoneModification", 1)],
            },
        ];
    }

    // ── WinsNameResolutionPolicy ──
    private static class _WinsNameResolutionPolicy
    {
        private const string DnsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

        private const string NetbtKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wins-set-dns-cache-timeout",
                    Label = "WINS: Set DNS Cache Entry Maximum TTL to 1 Hour",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets MaxCacheTtl=3600 in DNS client policy. Caps the maximum time a successful DNS resolution result is cached in the Windows DNS resolver cache to 1 hour, regardless of the record's TTL. Longer TTL caches can cause stale A record lookups after IP address changes (failover scenarios, DR tests, cloud load balancer IP rotation). Capping at 1 hour ensures stale records don't persist beyond 1 hour in event of planned or unplanned address changes.",
                    Tags = ["wins", "dns", "cache", "ttl", "failover"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "DNS records are re-resolved after at most 1 hour. This slightly increases DNS query load for long-TTL records but ensures nearly real-time failover detection for DNS-based HA.",
                    ApplyOps = [RegOp.SetDword(DnsKey, "MaxCacheTtl", 3600)],
                    RemoveOps = [RegOp.DeleteValue(DnsKey, "MaxCacheTtl")],
                    DetectOps = [RegOp.CheckDword(DnsKey, "MaxCacheTtl", 3600)],
                },
                new TweakDef
                {
                    Id = "wins-disable-dns-compression",
                    Label = "WINS: Disable DNS Query Payload Compression (Debug Mode)",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets DisableCompression=1 in DNS client policy. Disables DNS message compression in outbound DNS queries. DNS compression reduces packet size but, in rare cases, can cause parsing errors with non-RFC-compliant DNS resolvers that implement compression algorithms incorrectly (found in some embedded or appliance DNS proxies). Disabling compression is a diagnostic/debug setting: enable it when troubleshooting DNS query failures with appliance-based DNS servers that behave unexpectedly.",
                    Tags = ["wins", "dns", "compression", "debug", "diagnostic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote =
                        "DNS queries are sent uncompressed (slightly larger packets). Only use as a diagnostic setting for troubleshooting specific DNS appliance compatibility issues.",
                    ApplyOps = [RegOp.SetDword(DnsKey, "DisableCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableCompression")],
                    DetectOps = [RegOp.CheckDword(DnsKey, "DisableCompression", 1)],
                },
                new TweakDef
                {
                    Id = "wins-enable-smart-multi-homed",
                    Label = "WINS: Enable Smart Multi-Homed DNS Registration",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets EnableAutoConfig=1 in DNS client policy. Enables smart multi-homed DNS registration: when a machine has multiple network interfaces, Windows will register only the interface with the best default gateway route rather than registering all adapter IPs. This prevents DNS pollution from VPN temporary IPs, APIPA addresses, and link-local IPv6 addresses appearing in the corporate DNS zone. Smart registration ensures clients resolve to the primary routable IP address of a machine.",
                    Tags = ["wins", "dns", "multi-homed", "registration", "smart"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only the primary/best-route adapter IP is registered in DNS. Non-primary adapter IPs (VPN adapters, secondary NICs) are not registered when this is enabled.",
                    ApplyOps = [RegOp.SetDword(DnsKey, "EnableAutoConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(DnsKey, "EnableAutoConfig")],
                    DetectOps = [RegOp.CheckDword(DnsKey, "EnableAutoConfig", 1)],
                },
            ];
    }

    // ── WirelessDisplayPolicy ──
    private static class _WirelessDisplayPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessDisplay";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdsply-block-projection-to-pc",
                    Label = "Block Wireless Projection To This PC",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents other devices from wirelessly projecting their screen to this PC via Miracast. Eliminates the risk of screen eavesdropping or unauthorised projection in shared spaces. Default: 1 (allow). Recommended: 0 in open environments.",
                    Tags = ["wireless-display", "miracast", "projection", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "No other device can project its screen here; eliminates physical screen-capture risk.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowProjectionToPC", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowProjectionToPC")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowProjectionToPC", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-require-pin-pairing",
                    Label = "Always Require PIN for Wireless Display Pairing",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets PIN requirement to 'Always' (2) for Miracast pairing. Prevents unauthorised devices from connecting without a confirmed PIN exchange. Default: 0 (never). Recommended: 2 (always).",
                    Tags = ["wireless-display", "pin", "pairing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All Miracast connections require PIN confirmation; prevents rogue device connections.",
                    ApplyOps = [RegOp.SetDword(Key, "RequirePinForPairing", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequirePinForPairing")],
                    DetectOps = [RegOp.CheckDword(Key, "RequirePinForPairing", 2)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-receiver-input",
                    Label = "Block User Input From Wireless Display Receiver",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents keyboard and mouse input from being relayed back to the PC from a wireless display receiver. Stops remote HID injection via a Miracast receiver. Default: 1 (allow). Recommended: 0.",
                    Tags = ["wireless-display", "input", "hid", "receiver", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Wireless display is output-only; the receiver cannot send keystrokes or mouse events.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUserInputFromWirelessDisplayReceiver", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUserInputFromWirelessDisplayReceiver")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUserInputFromWirelessDisplayReceiver", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-disable-auto-discovery",
                    Label = "Disable Automatic Wireless Display Discovery",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents this PC from automatically discovering and advertising itself to nearby Miracast-capable devices. Reduces exposure on shared networks or public spaces. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "discovery", "miracast", "advertisement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device is not visible to Miracast senders until discovery is explicitly initiated.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableAutoDiscovery", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableAutoDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableAutoDiscovery", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-infra-projection",
                    Label = "Block Infrastructure-Mode Wireless Projection",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables Miracast projection via the local Wi-Fi infrastructure (access point). Limits projection to Wi-Fi Direct only, reducing network-based interception surface. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "infrastructure", "wifi", "projection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Infrastructure-mode Miracast (via Wi-Fi AP) is blocked; Wi-Fi Direct is the only projection path.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowProjectionToPCOverInfrastructure", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowProjectionToPCOverInfrastructure")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowProjectionToPCOverInfrastructure", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-miracast-broadcast",
                    Label = "Block Miracast Broadcast Advertisement",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents this PC from broadcasting Miracast availability beacons. The device is invisible to P2P Miracast senders that rely on broadcast discovery. Default: 1. Recommended: 0 in secure offices.",
                    Tags = ["wireless-display", "broadcast", "miracast", "discovery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device does not advertise Miracast availability; projection must be manually initiated on sender.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMiracastBroadcast", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMiracastBroadcast")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMiracastBroadcast", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-ble-pairing",
                    Label = "Disable Bluetooth LE Pairing for Wireless Display",
                    Category = "Network — Win Inet",
                    Description =
                        "Disallows Bluetooth Low Energy (BLE) as a pairing mechanism for Miracast connections. Reduces BLE attack surface during wireless display setup. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "bluetooth", "ble", "pairing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "BLE-initiated Miracast pairing is disabled; Wi-Fi Direct PIN is the only pairing method.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowBleForPairing", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowBleForPairing")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowBleForPairing", 0)],
                },
                new TweakDef
                {
                    Id = "wdsply-limit-connection-count",
                    Label = "Limit Simultaneous Wireless Display Connections",
                    Category = "Network — Win Inet",
                    Description =
                        "Sets the maximum number of simultaneous wireless display connections to 1. Prevents resource exhaustion and limits exposure in multi-user scan environments. Default: not restricted. Recommended: 1.",
                    Tags = ["wireless-display", "connection-limit", "resource", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Only one device can connect at a time; others must wait for the session to end.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxConnectionCount", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxConnectionCount")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxConnectionCount", 1)],
                },
                new TweakDef
                {
                    Id = "wdsply-require-wpa2",
                    Label = "Require WPA2 Encryption for Wireless Display",
                    Category = "Network — Win Inet",
                    Description =
                        "Enforces WPA2 (or later) encryption for all Miracast Wi-Fi Direct connections. Prevents unencrypted or WEP-protected wireless display sessions. Default: not enforced. Recommended: 1.",
                    Tags = ["wireless-display", "wpa2", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All Miracast sessions must use WPA2 encryption; WEP or open sessions are rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireWPA2ForPairing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA2ForPairing")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireWPA2ForPairing", 1)],
                },
                new TweakDef
                {
                    Id = "wdsply-block-mdm-input",
                    Label = "Block MDM Input Commands from Wireless Display",
                    Category = "Network — Win Inet",
                    Description =
                        "Prevents an MDM management profile delivered via a wireless display receiver from sending input or configuration commands to this device. Closes an MDM-over-Miracast injection vector. Default: 1. Recommended: 0.",
                    Tags = ["wireless-display", "mdm", "input", "injection", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "MDM commands cannot arrive through a wireless display receiver; eliminates that attack vector.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMDMInputFromWirelessDisplayReceiver", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMDMInputFromWirelessDisplayReceiver")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMDMInputFromWirelessDisplayReceiver", 0)],
                },
            ];
    }

    // ── WlanPolicy ──
    private static class _WlanPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WlanSvc";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wlanpol-disable-auto-connect-to-open",
                Label = "Prevent Auto-Connect to Open Wireless Networks",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Open wireless networks transmit all data unencrypted and can be controlled by attackers who deploy networks with matching SSIDs. Preventing auto-connection to open networks ensures that endpoints do not silently attach to potentially malicious unencrypted wireless access points. Auto-connection to open networks exposes all unencrypted application traffic to passive capture and active man-in-the-middle attacks. Enterprise endpoints connecting to rogue open WiFi can be subjected to credential capture, certificate-based MITM, and traffic manipulation. Users can still manually connect to open networks when necessary but automatic connection without user acknowledgment is disabled. Enterprise wireless policy should require WPA2-Enterprise or WPA3 authentication for all wireless connections used on managed endpoints.",
                Tags = ["wlan", "open-network", "wireless", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventAutoConnectOpen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventAutoConnectOpen")],
                DetectOps = [RegOp.CheckDword(Key, "PreventAutoConnectOpen", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-hosted-network",
                Label = "Disable Wireless Hosted Network Creation",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Wireless Hosted Network allows endpoints to create a wireless access point sharing their own network connection with other devices. Disabling hosted network creation prevents endpoints from becoming unauthorized wireless access points bridging corporate and external networks. Hosted networks bypass network access controls by creating an unprotected ingress point into the enterprise network segment. Attackers with access to a corporate endpoint can create a hosted network to allow other devices wireless access to the internal network. Hosted network creation is also used for data exfiltration by connecting external devices to corporate network through the employee endpoint. Disabling hosted networks is standard enterprise configuration and should be enforced on all managed endpoints with wireless capabilities.",
                Tags = ["wlan", "hosted-network", "wireless", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHostedNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHostedNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHostedNetwork", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-require-wpa3",
                Label = "Require WPA3 for New Wireless Connections",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "WPA3 provides stronger authentication and forward secrecy compared to WPA2 preventing offline dictionary attacks against captured handshakes. Requiring WPA3 ensures new wireless connections use Simultaneous Authentication of Equals which provides stronger password-based authentication. WPA2 PSK networks are vulnerable to offline brute-force attacks where PMKID or four-way handshake captures can be cracked. WPA3 SAE prevents offline attacks by requiring active participation in the authentication exchange preventing passive capture attacks. Enterprise wireless infrastructure should support WPA3-Enterprise for managed endpoint connections. WPA3 requirement enforcement may not be compatible with older wireless hardware and clients that only support WPA2.",
                Tags = ["wlan", "wpa3", "wireless-security", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireWPA3", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA3")],
                DetectOps = [RegOp.CheckDword(Key, "RequireWPA3", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-social-wifi",
                Label = "Disable Wi-Fi Sense Social Network Sharing",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Wi-Fi Sense automatically shared wireless network credentials with contacts in Outlook, Skype, and Facebook friend networks. Disabling Wi-Fi Sense prevents corporate wireless credentials from being shared with personal contacts through Microsoft cloud services. Credential sharing via Wi-Fi Sense could allow uninvited devices to join corporate wireless networks through personal contact sharing. Microsoft disabled Wi-Fi Sense in Windows 10 1803 but policy enforcement provides explicit control on endpoints that may have older configurations. Wi-Fi Sense credential sharing violated the principle of least privilege by enabling broad credential dissemination without administrator approval. Disabling Wi-Fi Sense through policy prevents reactivation if the feature is re-enabled through software updates or configuration changes.",
                Tags = ["wlan", "wifi-sense", "credential-sharing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSocialWiFiSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSocialWiFiSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSocialWiFiSharing", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-wlan-hotspot-auto",
                Label = "Disable Automatic WiFi Hotspot Activation",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Automatic mobile hotspot activation can enable internet connection sharing without explicit user action leading to unauthorized wireless access points. Disabling automatic hotspot activation ensures mobile hotspot creation requires deliberate administrator or user action rather than automatic triggering. Hotspot activation shares the endpoint's cellular or wired internet connection over wireless creating a bridge between networks. Automatic hotspot activation could result in corporate-attached endpoints sharing cellular connections externally without IT awareness. Enterprise endpoints should not have automatic hotspot activation as it creates unmonitored network bridges. Mobile Device Management policies should explicitly control hotspot functionality to prevent unauthorized network bridges.",
                Tags = ["wlan", "hotspot", "wireless", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoHotspot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoHotspot")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoHotspot", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-block-peer-to-peer-wlan",
                Label = "Block Peer-to-Peer Wireless (Ad-Hoc) Networks",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Ad-hoc peer-to-peer wireless networks create direct device-to-device connections that bypass network access controls and security monitoring. Blocking P2P wireless networks prevents endpoints from establishing direct wireless connections with unknown devices that may be outside enterprise control. Ad-hoc connections can be used for data exfiltration from corporate endpoints to personally owned devices nearby. P2P wireless connections are also used by Direct Sequence Spread Spectrum remote access tools that establish wireless communication channels. Enterprise wireless policy should require all wireless connectivity to go through managed access points rather than direct device connections. Blocking ad-hoc wireless is fundamental wireless security hardening for all enterprise-managed endpoints.",
                Tags = ["wlan", "ad-hoc", "p2p", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockPeerToPeerWLAN", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPeerToPeerWLAN")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPeerToPeerWLAN", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-wifi-roaming-aggressive",
                Label = "Disable Aggressive WiFi Roaming",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Aggressive WiFi roaming causes endpoints to probe for and associate with networks actively which can reveal preferred network lists to passive listeners. Disabling aggressive roaming reduces the frequency and reach of active network probes that can expose enterprise SSID information. Active WiFi probing from managed endpoints can reveal the names of corporate and personal WiFi networks to anyone monitoring nearby radio frequencies. Passive SSID disclosure through probing can facilitate targeted rogue access point attacks using known corporate SSIDs. Disabling aggressive roaming provides privacy and security benefits by reducing unnecessary radio frequency reconnaissance exposure. WiFi management settings should minimize active probing while maintaining acceptable connectivity performance for enterprise use.",
                Tags = ["wlan", "roaming", "privacy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAggressiveRoaming", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAggressiveRoaming")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAggressiveRoaming", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-enable-wlan-auditing",
                Label = "Enable WLAN Connection Event Auditing",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "WLAN connection event auditing records wireless association and disassociation events providing a log of network connectivity history. Enabling WLAN auditing generates Windows events for wireless connections including SSID, authentication type, and connection time. Wireless connection logs help identify employees connecting to unauthorized or personal wireless networks on managed endpoints. WLAN audit data is valuable for detecting rogue access point connections and unusual wireless connectivity patterns. Security teams can correlate WLAN events with network access control logs to identify endpoints bypassing wired network restrictions. Wireless connection auditing combined with geolocation data can identify endpoints connecting from unexpected physical locations.",
                Tags = ["wlan", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableWLANAuditing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableWLANAuditing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableWLANAuditing", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-disable-random-mac",
                Label = "Disable Random MAC Address for Managed Networks",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Random MAC addresses provide privacy by preventing tracking of device movement across different wireless networks. Disabling MAC randomization on managed networks ensures consistent device identity that is important for Network Access Control and DHCP lease management. NAC solutions that enforce wireless access based on MAC addresses require consistent hardware identifiers for policy enforcement. DHCP servers depend on consistent MAC addresses for IP address assignment and lease management in enterprise environments. Random MACs on corporate networks can cause duplicate IP assignments and NAC policy failures when the MAC changes. Disabling randomization only on identified corporate SSIDs allows privacy protection to continue on personal and public networks.",
                Tags = ["wlan", "mac-address", "nac", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRandomMACForManaged", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRandomMACForManaged")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRandomMACForManaged", 1)],
            },
            new TweakDef
            {
                Id = "wlanpol-restrict-wlan-to-approved-ssids",
                Label = "Restrict Wireless Connections to Approved SSIDs",
                Category = "Network — Win Inet",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "SSID restriction limits wireless connections to a defined list of approved corporate wireless networks preventing connection to personal or public networks. Restricting endpoints to approved SSIDs ensures that all wireless connectivity goes through monitored and controlled access points. Employees connecting to personal or public WiFi from corporate endpoints bypass security controls including proxy filtering and network monitoring. SSID-based restrictions through wireless profiles deployed via Group Policy prevent unauthorized wireless connections. SSID restrictions must include both corporate office networks and approved VPN-capable networks for remote workers. Wireless SSID restriction combined with always-on VPN ensures that traffic from any wireless connection is protected and monitored.",
                Tags = ["wlan", "ssid", "restriction", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToApprovedSSIDs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToApprovedSSIDs")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToApprovedSSIDs", 1)],
            },
        ];
    }

    // ── WsdPrintDiscoveryPolicy ──
    private static class _WsdPrintDiscoveryPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WSD";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsdprt-disable-wsd-discovery",
                    Label = "Disable WSD Printer Discovery",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables Web Services for Devices (WSD) printer discovery on the local network, preventing Windows from automatically detecting and adding WSD-compatible printers via SOAP-based device profile discovery.",
                    Tags = ["wsd", "printing", "discovery", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD printer discovery disabled; WSD-compatible printers must be added manually.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-disable-wsd-advertisement",
                    Label = "Disable WSD Printer Advertisement from This Host",
                    Category = "Network — Win Inet",
                    Description =
                        "Stops this Windows host from advertising locally-attached printers as WSD devices on the network, hiding accessible printers from other machines performing WSD discovery.",
                    Tags = ["wsd", "printing", "advertisement", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD advertisement stopped; this host's printers not visible via WSD to other network devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDAdvertisement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDAdvertisement")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDAdvertisement", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-require-auth-for-wsd-print",
                    Label = "Require Authentication for WSD Print Access",
                    Category = "Network — Win Inet",
                    Description =
                        "Requires user authentication before accepting WSD print operations from network clients, preventing unauthorised devices from submitting print jobs via WSD.",
                    Tags = ["wsd", "printing", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD print requires auth; unauthenticated network print jobs via WSD rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAuthForWSDPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthForWSDPrint")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAuthForWSDPrint", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-block-wsd-on-public-network",
                    Label = "Block WSD Printer Discovery on Public Networks",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables WSD printer discovery when the network location profile is set to Public, preventing printer discovery at coffeeshops, airports, or other untrusted networks.",
                    Tags = ["wsd", "printing", "public-network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD discovery disabled on public network profiles; printer discovery only active on Private/Domain profiles.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDOnPublicNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDOnPublicNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDOnPublicNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-limit-wsd-metadata-exposure",
                    Label = "Limit WSD Device Metadata Exposure",
                    Category = "Network — Win Inet",
                    Description =
                        "Restricts the metadata returned in WSD discovery responses, hiding detailed hardware model, firmware version, and network capability information that could aid reconnaissance.",
                    Tags = ["wsd", "metadata", "privacy", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD metadata limited; device model and firmware details not disclosed in discovery responses.",
                    ApplyOps = [RegOp.SetDword(Key, "LimitWSDMetadataExposure", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LimitWSDMetadataExposure")],
                    DetectOps = [RegOp.CheckDword(Key, "LimitWSDMetadataExposure", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-block-wsd-eventing",
                    Label = "Block WSD Eventing Subscriptions for Printers",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables WSD eventing subscriptions that allow remote clients to subscribe to printer status events (paper out, error, job complete) via WSD push notifications, reducing unsolicited outbound connections.",
                    Tags = ["wsd", "printing", "eventing", "subscriptions", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "WSD printer event subscriptions blocked; remote clients cannot receive push status notifications.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDEventing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDEventing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDEventing", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-disable-wsd-scan",
                    Label = "Disable WSD Scan (WSCN) Discovery",
                    Category = "Network — Win Inet",
                    Description =
                        "Disables Windows Scan Communication Notifications (WSCN), preventing automatic discovery of WSD-compatible scanner devices over the network.",
                    Tags = ["wsd", "scanner", "wscn", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD scanner discovery disabled; network scanners must be added manually.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSDScanDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSDScanDiscovery")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSDScanDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-require-tls-for-wsd",
                    Label = "Require TLS for WSD HTTPS Print Communication",
                    Category = "Network — Win Inet",
                    Description =
                        "Forces WSD print data transmission to use HTTPS (SOAP over TLS), encrypting WSD messages and preventing plaintext interception of print content and printer control commands.",
                    Tags = ["wsd", "tls", "https", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSD print traffic TLS-encrypted; unencrypted WSD HTTP connections rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTLSForWSD", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTLSForWSD")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTLSForWSD", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-block-wsd-cross-subnet",
                    Label = "Block WSD Discovery Across Subnets",
                    Category = "Network — Win Inet",
                    Description =
                        "Restricts WSD discovery to the local subnet only, preventing WSD multicast probes from being forwarded through routers and reaching printers in distant network segments.",
                    Tags = ["wsd", "printing", "subnet", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD discovery limited to local subnet; cross-router WSD discovery disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockWSDCrossSubnet", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockWSDCrossSubnet")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockWSDCrossSubnet", 1)],
                },
                new TweakDef
                {
                    Id = "wsdprt-audit-wsd-connections",
                    Label = "Enable Audit Logging for WSD Printer Connections",
                    Category = "Network — Win Inet",
                    Description =
                        "Enables event log entries whenever a WSD printer is added, removed, or a print job is submitted via WSD, providing a discovery and usage trail for network printer monitoring.",
                    Tags = ["wsd", "audit-log", "printing", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSD printer activity logged; connection and print events visible in event viewer.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditWSDPrinterConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditWSDPrinterConnections")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditWSDPrinterConnections", 1)],
                },
            ];
    }
}
