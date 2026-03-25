// RegiLattice.Core — Tweaks/RemoteCredentialGuardPolicy.cs
// Sprint 351: Remote Credential Guard Policy tweaks (10 tweaks)
// Category: "Remote Credential Guard Policy" | Slug: rcgrd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteCredentialGuard

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RemoteCredentialGuardPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteCredentialGuard";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rcgrd-enable-remote-credential-guard",
            Label = "Enable Remote Credential Guard for Remote Desktop Connections",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Remote Credential Guard protects credentials used for Remote Desktop connections by keeping credentials on the originating device rather than sending them to the remote host. Without Remote Credential Guard credentials are sent to the remote host where they are vulnerable to credential-dumping attacks by malware or attackers with elevated privileges on the remote system. Enabling Remote Credential Guard is one of the most effective mitigations against pass-the-hash and pass-the-ticket attacks that use credentials harvested from remote desktop target systems. The feature requires that the client device supports Credential Guard and that the remote host is running Windows 10 1607 or later or Windows Server 2016 or later. Remote Credential Guard is particularly important for privileged administrator accounts that use remote desktop to manage sensitive systems. Organizations should combine Remote Credential Guard with Restricted Admin mode to provide two complementary credential theft prevention approaches.",
            Tags = ["remote-credential-guard", "rdp", "credential-theft", "pass-the-hash", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRemoteCredentialGuard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteCredentialGuard")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRemoteCredentialGuard", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-enforce-strict-kerberos-delegation",
            Label = "Enforce Strict Kerberos Delegation Constraints for RCG Sessions",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Strict Kerberos delegation constraints in Remote Credential Guard sessions limit the ability of remote hosts to use delegated credentials for additional authentication to downstream services. Without delegation constraints an attacker who compromises a remote desktop target system can use the delegated credentials to access additional systems on behalf of the authenticated user. Constrained delegation limits the services that delegated credentials can be used to access to a pre-configured allowlist of validated services. Remote Credential Guard with strict delegation prevents credential abuse through delegation chains that could otherwise allow attackers to move laterally using the initial credential. Organizations should review and restrict delegation settings for accounts that use remote desktop to high-value systems. Strict delegation constraints combined with Remote Credential Guard provide layered protection against credential abuse in remote access scenarios.",
            Tags = ["remote-credential-guard", "kerberos-delegation", "constrained-delegation", "lateral-movement", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceStrictKerberosDelegation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceStrictKerberosDelegation")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceStrictKerberosDelegation", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-restrict-ntlm-in-rcg-sessions",
            Label = "Restrict NTLM Authentication in Remote Credential Guard Sessions",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Restricting NTLM authentication in Remote Credential Guard sessions prevents fallback to NTLM when Kerberos fails which would expose credentials to NTLM relay attacks. Remote Credential Guard relies on Kerberos for its credential protection guarantees and NTLM fallback undermines these protections by allowing credential exposure through NTLM. NTLM relay attacks are a common post-exploitation technique that can be used when NTLM authentication is available even in environments that have prohibited NTLM for general use. Restricting NTLM in RCG sessions enforces Kerberos-only authentication ensuring that the full credential protection benefits of Remote Credential Guard apply. Organizations should verify that Kerberos authentication infrastructure is correctly configured and accessible from all systems where Remote Credential Guard is in use before restricting NTLM. Monitoring for NTLM authentication attempts during RCG sessions helps identify cases where Kerberos is failing and NTLM restriction is causing authentication failures.",
            Tags = ["remote-credential-guard", "ntlm", "kerberos", "credential-protection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictNTLMInRCGSessions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictNTLMInRCGSessions")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictNTLMInRCGSessions", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-audit-rcg-session-events",
            Label = "Enable Audit Logging for Remote Credential Guard Session Events",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Remote Credential Guard session audit logging captures connection establishment authentication events and session termination data for security monitoring and incident response. Audit data from RCG sessions enables detection of unauthorized remote access attempts failed authentication that may indicate brute force and unusual connection patterns. RCG audit events should be forwarded to a central logging system for correlation with other security events from the remote hosts and authentication infrastructure. Connection timing geographical patterns and frequency anomalies in RCG sessions may indicate compromised account credentials attempting to use legitimate remote access channels. Audit logs from RCG sessions provide accountability for administrative actions performed on sensitive systems accessed through remote desktop. Organizations should retain RCG session audit logs for a period appropriate to their regulatory requirements and incident investigation needs.",
            Tags = ["remote-credential-guard", "audit", "remote-desktop", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditRCGSessionEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditRCGSessionEvents")],
            DetectOps = [RegOp.CheckDword(Key, "AuditRCGSessionEvents", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-enable-rcg-for-admin-sessions",
            Label = "Enforce Remote Credential Guard for All Administrator Remote Desktop Sessions",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Enforcing Remote Credential Guard for administrator remote desktop sessions ensures that privileged credentials are never exposed to the remote hosts that administrators manage. Administrator accounts are high-value targets for credential theft and administrator remote desktop sessions to servers are a common source of credential compromise in enterprise intrusions. Enforcing RCG for admin sessions prevents the most common attack pattern where attackers compromise a single server and then dump administrator credentials from RDP sessions connected to it. Policy enforcement ensures that administrators cannot bypass Remote Credential Guard even when connecting from tools that do not use it by default. Organizations should deploy this policy to all systems that administrator accounts use to launch remote desktop sessions rather than to the servers being managed. Combining RCG enforcement with privileged access workstations creates a comprehensive approach to protecting administrator credentials.",
            Tags = ["remote-credential-guard", "admin-accounts", "rdp", "privileged-access", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceRCGForAdminSessions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceRCGForAdminSessions")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceRCGForAdminSessions", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-block-credential-delegation-to-untrusted",
            Label = "Block Credential Delegation to Untrusted Remote Desktop Hosts",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Blocking credential delegation to untrusted remote desktop hosts prevents credentials from being sent to systems that are not explicitly authorized to receive them through Windows Remote Management policy. Untrusted remote hosts that receive delegated credentials pose a significant risk as credentials can be extracted from them by attackers who have already compromised those systems. The trust model for credential delegation must be carefully maintained with only known-good systems in the trusted hosts list. Hosts should be considered trusted only when they have current security baselines applied Endpoint Protection coverage and are monitored for security events. Blocking delegation to untrusted hosts reduces the blast radius of a single system compromise by preventing credential propagation to that compromised system from subsequent remote desktop connections. Organizations should maintain an accurate and current list of trusted hosts and regularly audit which systems are in the trusted list.",
            Tags = ["remote-credential-guard", "credential-delegation", "trusted-hosts", "blast-radius", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockDelegationToUntrustedHosts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockDelegationToUntrustedHosts")],
            DetectOps = [RegOp.CheckDword(Key, "BlockDelegationToUntrustedHosts", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-require-nla-for-rcg-sessions",
            Label = "Require Network Level Authentication for Remote Credential Guard Sessions",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Requiring Network Level Authentication for Remote Credential Guard sessions ensures that authentication must be completed before a remote desktop session is established rather than after the login screen is presented. NLA authentication pre-authenticates the user before session establishment preventing scenarios where the remote host is exposed to the network before the user proves their identity. Without NLA the remote desktop login screen itself is exposed to network attackers who can attempt exploitation before any credentials are involved. Combining NLA with Remote Credential Guard provides defense-in-depth where authentication occurs before session establishment and credentials are protected during the session. NLA is also significantly more efficient for server resources as sessions that fail NLA authentication do not consume remote desktop server session resources. Organizations should verify that all clients that will use Remote Credential Guard support NLA to avoid compatibility issues.",
            Tags = ["remote-credential-guard", "nla", "pre-authentication", "rdp-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireNLAForRCGSessions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireNLAForRCGSessions")],
            DetectOps = [RegOp.CheckDword(Key, "RequireNLAForRCGSessions", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-restrict-rcg-to-domain-joined-hosts",
            Label = "Restrict Remote Credential Guard to Domain-Joined Remote Hosts",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting Remote Credential Guard connections to domain-joined remote hosts ensures that RCG is only used to connect to systems that are subject to organizational security policy and management. Non-domain-joined systems do not have the same security baseline expectations and may not have the protections expected for systems that will receive remote desktop connections. Domain membership provides verification that the remote host has been configured according to organizational security standards and has Group Policy applied. Systems outside the domain like personal computers contractor systems or guest network hosts should not receive domain credentials through remote desktop even with Remote Credential Guard protections. The domain restriction aligns remote desktop credential usage with the formal trust model of the domain ensuring credentials are only used within the authenticated infrastructure. Organizations should enforce this restriction through Group Policy to prevent users from using Remote Credential Guard to access non-domain targets.",
            Tags = ["remote-credential-guard", "domain-joined", "trusted-hosts", "rdp", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictToDomainJoinedHosts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictToDomainJoinedHosts")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictToDomainJoinedHosts", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-enable-rcg-session-encryption",
            Label = "Enforce Strong Encryption for Remote Credential Guard Session Traffic",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enforcing strong encryption for Remote Credential Guard session traffic ensures that all remote desktop session data is protected against network interception and man-in-the-middle attacks. Weak encryption configurations for RDP sessions allow attackers with network access to decrypt and access session content including screen data keystrokes and clipboard data. Modern RDP encryption uses TLS with strong cipher suites and Remote Credential Guard sessions should require TLS 1.2 or higher with strong cipher suites. Legacy encryption modes that provide weaker protection should be explicitly disabled to prevent negotiation of weak encryption even when the client or server supports legacy modes. Organizations should verify the TLS version and cipher suite negotiations used for Remote Credential Guard sessions using network capture analysis. Regular rotation of the RDP server certificate and verification of certificate validity helps maintain the security of the encryption channel.",
            Tags = ["remote-credential-guard", "encryption", "tls", "rdp-encryption", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceStrongEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceStrongEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceStrongEncryption", 1)],
        },
        new TweakDef
        {
            Id = "rcgrd-monitor-rcg-connection-anomalies",
            Label = "Enable Monitoring for Remote Credential Guard Connection Anomalies",
            Category = "Remote Credential Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Monitoring for Remote Credential Guard connection anomalies detects unusual access patterns that may indicate account compromise or unauthorized use of legitimate remote access credentials. Anomaly monitoring tracks connection frequency timing geographic origin and destination patterns to identify sessions that deviate from established baselines. Compromised accounts used for legitimate remote desktop access may be detected when they are used outside normal working hours from unusual source addresses or to access systems they do not normally connect to. Connection anomaly detection for RCG sessions provides an additional detection layer beyond simple authentication success monitoring. Organizations should establish behavioral baselines for each privileged account's remote desktop usage and configure alerting thresholds appropriate for their environment. Anomaly alerts should be integrated with the incident response process to enable rapid investigation and containment of potentially compromised accounts.",
            Tags = ["remote-credential-guard", "anomaly-detection", "monitoring", "behavioral-analysis", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MonitorConnectionAnomalies", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "MonitorConnectionAnomalies")],
            DetectOps = [RegOp.CheckDword(Key, "MonitorConnectionAnomalies", 1)],
        },
    ];
}
