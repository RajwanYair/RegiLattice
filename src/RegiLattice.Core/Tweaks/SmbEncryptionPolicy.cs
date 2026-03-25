// RegiLattice.Core — Tweaks/SmbEncryptionPolicy.cs
// Sprint 321: SMB Encryption Policy tweaks (10 tweaks)
// Category: "SMB Encryption Policy" | Slug: smbenc
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SMB

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmbEncryptionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SMB";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smbenc-require-smb-encryption",
            Label = "Require SMB Encryption for All Connections",
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
            Category = "SMB Encryption Policy",
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
