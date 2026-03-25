// RegiLattice.Core — Tweaks/DomainIsolationPolicy.cs
// Sprint 322: Domain Isolation Policy tweaks (10 tweaks)
// Category: "Domain Isolation Policy" | Slug: domiso
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DomainIsolationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "domiso-enable-ipsec-policy",
            Label = "Enable IPsec Domain Isolation Policy",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "IPsec domain isolation restricts network communication so that domain-joined computers only accept connections from other authenticated domain members. Enabling domain isolation prevents non-domain-joined devices including compromised guest systems from establishing network connections with managed endpoints. Domain isolation is one of the most effective lateral movement prevention controls in Windows enterprise environments. All network traffic between domain-isolated endpoints is authenticated and optionally encrypted using IPsec transport mode. Implementing domain isolation requires IPsec firewall rules deployed through Group Policy that allow or require authentication. Domain isolation significantly reduces the blast radius of a single compromised endpoint by preventing lateral movement to other domain hosts.",
            Tags = ["ipsec", "domain-isolation", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDomainIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDomainIsolation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDomainIsolation", 1)],
        },
        new TweakDef
        {
            Id = "domiso-require-auth-for-inbound",
            Label = "Require IPsec Authentication for Inbound Connections",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Requiring IPsec authentication for inbound connections ensures that all incoming network requests come from identifiable and domain-authenticated sources. Inbound authentication requirements prevent anonymous network connections and force all sources to be authenticated before services are accessible. IPsec authentication for inbound traffic uses Kerberos tickets from domain-joined computers providing cryptographic proof of identity. Non-domain resources that need access can be granted exemptions through connection security rule exceptions while keeping general isolation. Requiring inbound authentication effectively creates a software-defined perimeter that moves beyond network segmentation. This policy is foundational for server isolation scenarios where critical servers should only accept connections from specific authenticated hosts.",
            Tags = ["ipsec", "inbound", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireAuthForInbound", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthForInbound")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAuthForInbound", 1)],
        },
        new TweakDef
        {
            Id = "domiso-enable-ipsec-encryption",
            Label = "Enable IPsec Traffic Encryption",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "IPsec traffic encryption protects the confidentiality of data transmitted between domain-isolated endpoints beyond providing authentication. Encrypting IPsec traffic ensures that even if network traffic is intercepted the data content remains confidential. IPsec encryption complements authentication by preventing data-in-transit exposure for east-west traffic between domain systems. Enabling encryption in domain isolation scenarios requires negotiating encryption algorithms through IKE and maintaining security associations. AES-128 or AES-256 should be specified as the encryption algorithms in IPsec policy for modern compliance requirements. IPsec encryption for all domain traffic provides confidentiality protection that complements TLS-based application encryption.",
            Tags = ["ipsec", "encryption", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIPsecEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIPsecEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIPsecEncryption", 1)],
        },
        new TweakDef
        {
            Id = "domiso-prefer-aes256",
            Label = "Prefer AES-256 for IPsec Encryption",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "AES-256 provides 256-bit symmetric encryption which exceeds NIST SP 800-57 recommendations for protection beyond 2030. Preferring AES-256 in IPsec policy ensures the strongest available symmetric encryption is negotiated for domain isolation traffic. Weaker algorithms like 3DES or AES-128 should only be used for compatibility when AES-256 is unavailable. AES-256 in IPsec may have slightly higher processing overhead than AES-128 but this is negligible on modern CPUs with AES-NI hardware acceleration. Setting AES-256 as the preferred algorithm ensures IKE negotiation selects the strongest option when both parties support it. Hardcoding preferred algorithms in IPsec policy prevents algorithm downgrade during negotiation to weaker but still technically supported options.",
            Tags = ["ipsec", "aes256", "encryption", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreferAES256", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreferAES256")],
            DetectOps = [RegOp.CheckDword(Key, "PreferAES256", 1)],
        },
        new TweakDef
        {
            Id = "domiso-enable-perfect-forward-secrecy",
            Label = "Enable Perfect Forward Secrecy for IPsec",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Perfect forward secrecy ensures that compromise of long-term keys does not allow decryption of previously recorded encrypted sessions. Enabling PFS for IPsec generates unique session keys for each IPsec security association using ephemeral Diffie-Hellman key exchange. Without PFS an attacker who records encrypted traffic can decrypt it after compromising the long-term keys used in the key exchange. PFS in IPsec requires renegotiation of master keys during IKE Phase 2 which adds some processing overhead. Diffie-Hellman Group 14 (2048-bit) or stronger should be specified for PFS to provide adequate security for the key exchange. PFS is an important property for long-lived secrets and provides cryptographic forward secrecy as part of defense-in-depth.",
            Tags = ["ipsec", "pfs", "forward-secrecy", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePerfectForwardSecrecy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePerfectForwardSecrecy")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePerfectForwardSecrecy", 1)],
        },
        new TweakDef
        {
            Id = "domiso-block-non-ipsec-fallback",
            Label = "Block Fallback to Unprotected Connections",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Connection fallback allows domain-isolated endpoints to fall back to unauthenticated connections when IPsec negotiation fails. Blocking fallback from IPsec-required connections ensures that traffic either uses IPsec protection or is not transmitted. Allowing fallback creates a gap in domain isolation where an attacker can force IPsec negotiation failure and access a target via plain traffic. Connection security rules should specify whether partial authentication fallback is allowed on a per-rule basis. Blocking fallback is appropriate for high-security servers while client endpoints may allow fallback for connections to non-domain resources. The trade-off between security (no fallback) and availability (fallback for resilience) must be evaluated for each deployment context.",
            Tags = ["ipsec", "fallback", "domain-isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNonIPsecFallback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNonIPsecFallback")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNonIPsecFallback", 1)],
        },
        new TweakDef
        {
            Id = "domiso-enable-ike-v2",
            Label = "Require IKEv2 for IPsec Key Exchange",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "IKEv2 is the modern version of the Internet Key Exchange protocol providing improved security, reliability, and mobility compared to IKEv1. Requiring IKEv2 ensures that IPsec connections use the protocol version with MOBIKE support, traffic selectors, and improved negotiation. IKEv2 includes native dead peer detection and eliminates many of the mode negotiation vulnerabilities present in IKEv1 main and aggressive mode. IKEv2 with EAP authentication provides a strong mutual authentication mechanism suitable for remote access and domain isolation scenarios. Windows has supported IKEv2 since Windows 7 so requiring IKEv2 should not cause compatibility issues on modern enterprise endpoints. Requiring IKEv2 eliminates exposure to IKEv1 vulnerabilities including aggressive mode pre-shared key cracking and main mode identity disclosure.",
            Tags = ["ipsec", "ikev2", "key-exchange", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireIKEv2", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireIKEv2")],
            DetectOps = [RegOp.CheckDword(Key, "RequireIKEv2", 1)],
        },
        new TweakDef
        {
            Id = "domiso-log-ipsec-failures",
            Label = "Enable IPsec Negotiation Failure Logging",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "IPsec negotiation failure logging records events when IPsec key exchange fails providing visibility into domain isolation policy enforcement. Enabling failure logging helps diagnose misconfigured clients, rogue devices attempting connections, and policy configuration errors. Persistent IPsec negotiation failures from unexpected source addresses may indicate unauthorized devices attempting to communicate. IPsec failure events in Windows Firewall and Security Auditing logs include source/destination addresses, error codes, and protocol identifiers. SIEM correlation of IPsec failures with other security events enables detection of attempts to circumvent domain isolation. IPsec failure logging is essential during initial domain isolation deployment to identify endpoints that need policy updates.",
            Tags = ["ipsec", "logging", "failures", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LogIPsecFailures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LogIPsecFailures")],
            DetectOps = [RegOp.CheckDword(Key, "LogIPsecFailures", 1)],
        },
        new TweakDef
        {
            Id = "domiso-exempt-icmp",
            Label = "Configure ICMP Exemption from IPsec",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "ICMP exemption from IPsec authentication requirements allows diagnostic network utilities like ping to function without IPsec negotiations. Configuring ICMP exemptions maintains diagnostic capability while requiring IPsec for all other traffic types. ICMP traffic does not carry sensitive data and exempting it simplifies troubleshooting without compromising the security of data-carrying connections. Exempted ICMP traffic still traverses the network in plaintext which is acceptable since ICMP carries diagnostic information not sensitive data. Overly strict IPsec policies that fail ICMP traffic complicate network troubleshooting and may cause connectivity issues with network monitoring tools. ICMP exemption should be combined with ICMP rate limiting and filtering to prevent ICMP-based denial of service attacks.",
            Tags = ["ipsec", "icmp", "exemption", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ExemptICMPFromIPSec", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ExemptICMPFromIPSec")],
            DetectOps = [RegOp.CheckDword(Key, "ExemptICMPFromIPSec", 1)],
        },
        new TweakDef
        {
            Id = "domiso-enable-ipsec-monitoring",
            Label = "Enable IPsec Security Association Monitoring",
            Category = "Domain Isolation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "IPsec security association monitoring tracks active IPsec connections and provides operational visibility into domain isolation health. Enabling SA monitoring exposes IPsec connection state for security operations teams to identify unexpected or problematic security associations. Live IPsec SA monitoring can detect sudden changes in protected connection counts that may indicate domain isolation failures or attacks. Security association data is available through Windows Firewall advanced monitoring and the Get-NetIPsecSA PowerShell cmdlet. Monitoring SA establishment rates can identify DoS attempts targeting IKE negotiation processes. IPsec monitoring data should be incorporated into security dashboards alongside firewall, endpoint, and network telemetry.",
            Tags = ["ipsec", "monitoring", "security-association", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSAMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSAMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSAMonitoring", 1)],
        },
    ];
}
