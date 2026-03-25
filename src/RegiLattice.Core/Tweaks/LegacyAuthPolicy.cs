// RegiLattice.Core — Tweaks/LegacyAuthPolicy.cs
// Sprint 331: Legacy Auth Policy tweaks (10 tweaks)
// Category: "Legacy Auth Policy" | Slug: legauth
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LegacyAuthPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "legauth-disable-lm-response",
            Label = "Disable LAN Manager Hash Response (LM Authentication)",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "LAN Manager authentication is a decades-old protocol that uses weak DES-encrypted password hashes that are trivially cracked with modern hardware. Disabling LM authentication responses prevents Windows from responding to LM authentication challenge requests from legacy systems. LM hashes can be cracked offline in minutes using dictionary attacks or rainbow tables making stored LM credentials immediately exploitable. Windows systems should be configured to use NTLMv2 or Kerberos instead of LM for all network authentication. LM authentication may be required for compatibility with very old systems like Windows 95/98 but these systems should not be present in modern enterprise networks. Disabling LM responses forces all authentication to use stronger NTLM or Kerberos protocols that provide superior security.",
            Tags = ["lm", "authentication", "legacy", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLmResponse", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLmResponse")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLmResponse", 1)],
        },
        new TweakDef
        {
            Id = "legauth-disable-ntlm-v1",
            Label = "Disable NTLMv1 Authentication Protocol",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "NTLMv1 is an older version of the NTLM authentication protocol that lacks the security improvements added in NTLMv2. Disabling NTLMv1 forces upgrading to NTLMv2 which includes session nonces preventing credential replay attacks that work against NTLMv1. NTLMv1 is vulnerable to pass-the-hash attacks where captured NTLM hashes are replayed without knowing the actual password. Microsoft has recommended disabling NTLMv1 since Windows Vista and its continued use represents unnecessary authentication risk. NTLMv1 responses can be downgr- forced by attackers in man-in-the-middle positions to capture more easily cracked credential material. Enterprise environments should audit for remaining NTLMv1 usage and migrate legacy applications to Kerberos or NTLMv2 before disabling.",
            Tags = ["ntlm", "ntlmv1", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNtlmV1", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNtlmV1")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNtlmV1", 1)],
        },
        new TweakDef
        {
            Id = "legauth-require-ntlmv2",
            Label = "Require NTLMv2 Response Only for Network Authentication",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Requiring NTLMv2-only responses ensures that Windows systems only send NTLMv2 credentials rejecting all older LM and NTLMv1 authentication requests. NTLMv2 includes session security features like mutual authentication keys and per-session nonces that reduce credential theft and replay risks. Requiring NTLMv2 only is an effective defense against downgrade attacks that force less secure authentication protocols. Systems requiring NTLMv2-only should be tested for compatibility with servers and applications using older NTLM versions before policy enforcement. NTLMv2 requirement should be combined with session security settings to maximize the security improvement. Organizations should prefer Kerberos over NTLM where possible with NTLMv2 as the fallback for legacy compatibility scenarios.",
            Tags = ["ntlm", "ntlmv2", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireNtlmV2", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireNtlmV2")],
            DetectOps = [RegOp.CheckDword(Key, "RequireNtlmV2", 1)],
        },
        new TweakDef
        {
            Id = "legauth-disable-weak-hash-storing",
            Label = "Prevent Storage of LAN Manager Hashes in SAM Database",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "The Security Accounts Manager database can store both NTLM and LAN Manager password hashes with LM hashes being significantly weaker. Preventing LM hash storage ensures that even if the SAM database is extracted attackers only obtain NTLM hashes rather than trivially crackable LM hashes. LM hashes split passwords into 7-character chunks that can be brute-forced independently reducing cracking complexity dramatically. Removing LM hash storage means that new password changes will not generate LM hashes but existing LM hashes persist until passwords change. A password change cycle should be initiated after enabling this policy to eliminate stored LM hashes from the SAM database. This policy is effective when combined with the NTLMv2-only response requirement and LM authentication disablement.",
            Tags = ["lm-hash", "sam", "password", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoLmHash", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoLmHash")],
            DetectOps = [RegOp.CheckDword(Key, "NoLmHash", 1)],
        },
        new TweakDef
        {
            Id = "legauth-disable-ntlm-outbound",
            Label = "Restrict Outbound NTLM Authentication Requests",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Outbound NTLM authentication allows Windows systems to authenticate to remote servers using NTLM which can be exploited to capture NTLM credentials. Restricting outbound NTLM prevents Windows systems from sending NTLM responses to rogue servers set up by attackers for credential capture. NTLM credential capture attacks involve an attacker triggering authentication requests to a server they control and capturing the NTLM response for offline cracking. Restricting outbound NTLM to allowed server lists forces explicit whitelisting of servers that require NTLM authentication. Organizations should identify all servers requiring NTLM authentication before enabling this restriction to prevent service disruptions. The restriction should be set to audit mode first to identify NTLM usage before switching to denial mode.",
            Tags = ["ntlm", "outbound", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictOutboundNtlm", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictOutboundNtlm")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictOutboundNtlm", 2)],
        },
        new TweakDef
        {
            Id = "legauth-restrict-ntlm-inbound",
            Label = "Restrict Inbound NTLM Authentication to Domain Accounts",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Inbound NTLM authentication allows external systems to authenticate to this Windows server using NTLM which can be exploited in pass-the-hash attacks. Restricting inbound NTLM to domain accounts prevents local account NTLM authentication which is commonly exploited for lateral movement. Domain account NTLM authentication is subject to Kerberos validation in domain environments providing stronger authentication guarantees. Local account NTLM authentication bypasses domain controller validation making it useful for attackers with captured local credentials. Restricting inbound NTLM to domain accounts forces attackers to use Kerberos authentication which provides better monitoring and audit capabilities. Organizations should test inbound NTLM restrictions in audit mode before enforcement to identify local account dependencies.",
            Tags = ["ntlm", "inbound", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictInboundNtlm", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictInboundNtlm")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictInboundNtlm", 1)],
        },
        new TweakDef
        {
            Id = "legauth-enable-ntlm-audit",
            Label = "Enable NTLM Authentication Event Auditing",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "NTLM authentication auditing generates Windows event log entries for all NTLM authentication attempts providing visibility into NTLM usage. Enabling NTLM auditing allows security teams to identify which applications and systems are still using NTLM authentication. NTLM audit logs reveal authentication patterns that indicate pass-the-hash attacks or unauthorized lateral movement using NTLM credentials. Audit data is necessary to identify NTLM dependencies before implementing NTLM restrictions that could disrupt production services. NTLM authentication events should be forwarded to SIEM for correlation with threat intelligence and anomaly detection. Regular review of NTLM audit data helps drive migration from NTLM to Kerberos over time as dependencies are identified and resolved.",
            Tags = ["ntlm", "audit", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditNtlm", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditNtlm")],
            DetectOps = [RegOp.CheckDword(Key, "AuditNtlm", 1)],
        },
        new TweakDef
        {
            Id = "legauth-disable-basic-auth",
            Label = "Disable Basic HTTP Authentication for Network Providers",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Basic HTTP authentication transmits credentials in Base64 encoding which is trivially decoded providing plaintext username and password to network observers. Disabling basic authentication for network providers prevents credentials from being transmitted in a format that exposes them to network sniffing. Basic authentication is insecure even over HTTPS as logs and proxies may capture the authentication header containing credentials. Network providers including WebDAV implementations that support basic authentication should be updated to use Negotiate or modern token-based authentication. Disabling basic auth may break legacy applications and web services that use basic authentication but safer alternatives are available. Organizations must identify all basic authentication dependencies and migrate them before enforcing this restriction.",
            Tags = ["basic-auth", "authentication", "credentials", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBasicAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBasicAuth")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBasicAuth", 1)],
        },
        new TweakDef
        {
            Id = "legauth-disable-digest-auth",
            Label = "Disable Digest Authentication for Network Connections",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Digest authentication is a challenge-response authentication scheme that provides limited protection compared to modern authentication protocols. Disabling digest authentication prevents Windows network providers from using this older authentication mechanism for WebDAV and similar connections. Digest authentication stores passwords in a reversible format on servers requiring it making server compromise expose all user credentials. Modern web applications should use OAuth2, SAML, or Negotiate authentication rather than Basic or Digest schemes. Digest authentication is vulnerable to man-in-the-middle attacks where an attacker can downgrade or capture authentication sequences. Organizations using IIS or other web servers that rely on digest authentication should migrate to Windows Integrated Authentication or modern tokens.",
            Tags = ["digest-auth", "authentication", "credentials", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDigestAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDigestAuth")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDigestAuth", 1)],
        },
        new TweakDef
        {
            Id = "legauth-enable-extended-protection",
            Label = "Enable Extended Protection for Authentication (EPA)",
            Category = "Legacy Auth Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Extended Protection for Authentication binds authentication tokens to the channel establishment preventing credential forwarding to unauthorized channels. Enabling EPA prevents NTLM credential relay attacks where an attacker intercepts and forwards authentication tokens to a different server. Channel binding tokens ensure that authentication credentials cannot be used against a server other than the one the client intended to authenticate to. EPA is particularly important for protecting against cross-protocol relay attacks such as NTLM relay to SMB or LDAP. Enabling EPA may impact older applications that do not support channel binding so compatibility testing is required. Microsoft recommends enabling EPA on all servers that support it as a defense-in-depth control against credential relay attacks.",
            Tags = ["epa", "authentication", "relay-protection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableExtendedProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableExtendedProtection")],
            DetectOps = [RegOp.CheckDword(Key, "EnableExtendedProtection", 1)],
        },
    ];
}
