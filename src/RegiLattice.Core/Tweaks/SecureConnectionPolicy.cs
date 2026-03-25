// RegiLattice.Core — Tweaks/SecureConnectionPolicy.cs
// Sprint 355: Secure Connection Policy tweaks (10 tweaks)
// Category: "Secure Connection Policy" | Slug: seccxn
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecureConnections

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecureConnectionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecureConnections";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "seccxn-disable-tls-10-protocol",
            Label = "Disable TLS 1.0 Protocol for All System Secure Connections",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "TLS 1.0 is a deprecated protocol with known vulnerabilities including POODLE and BEAST that allow attackers to decrypt TLS-protected traffic under specific conditions. Disabling TLS 1.0 at the system level enforces the use of TLS 1.2 or TLS 1.3 for all Windows Schannel-based connections preventing negotiation of the weak TLS 1.0 protocol. Organizations in regulated industries are required by PCI-DSS HIPAA and other frameworks to disable TLS 1.0 for transmission of regulated data. Compatibility impact should be assessed before disabling TLS 1.0 as some legacy applications and services may only support older TLS versions. Organizations should perform an inventory of all internal and external service dependencies to identify any that require TLS 1.0 and remediate those dependencies before disabling the protocol. Disabling TLS 1.0 should be accompanied by enabling TLS 1.3 support to ensure that the strongest available protocol is used for connections.",
            Tags = ["tls", "tls-1.0", "protocol-security", "encryption", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTls10", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTls10")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTls10", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-disable-tls-11-protocol",
            Label = "Disable TLS 1.1 Protocol for All System Secure Connections",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "TLS 1.1 is deprecated by RFC 8996 alongside TLS 1.0 and shares many of the same weaknesses including CBC cipher mode vulnerabilities that were addressed in TLS 1.2. Disabling TLS 1.1 at the system policy level prevents Windows Schannel from negotiating TLS 1.1 connections ensuring only TLS 1.2 and TLS 1.3 are available. Most modern applications and services have updated to support TLS 1.2 making TLS 1.1 disablement generally feasible in well-maintained environments. Organizations should scan their application portfolio for TLS 1.1 dependencies before enforcing this policy to identify applications that require updates. The IANA has assigned TLS 1.1 as historical status meaning it is no longer maintained and future cryptographic attacks may exploit known but currently impractical weaknesses. Disabling TLS 1.1 together with TLS 1.0 eliminates the most widely exploited historical protocol versions and reduces the attack surface for TLS-based attacks.",
            Tags = ["tls", "tls-1.1", "protocol-security", "deprecated-protocols", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTls11", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTls11")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTls11", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-enable-tls-13-support",
            Label = "Enable TLS 1.3 Protocol Support for Enhanced Connection Security",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "TLS 1.3 provides significant security improvements over TLS 1.2 including perfect forward secrecy by default removal of weak cipher suites an improved handshake and reduced latency. Enabling TLS 1.3 at system policy level ensures that connections use the most secure available transport protocol when both endpoints support it. TLS 1.3 eliminates several classes of downgrade attacks that affected TLS 1.2 including padding oracle attacks CBC mode attacks and certain MITM techniques. The improved TLS 1.3 handshake reduces the number of round trips required to establish a connection improving performance particularly for latency-sensitive applications. Organizations should enable TLS 1.3 support and monitor for any compatibility issues with services that do not yet support TLS 1.3 while retaining TLS 1.2 as a fallback. Perfect forward secrecy in TLS 1.3 is particularly valuable for protecting sensitive communications from future decryption if encryption keys are later compromised.",
            Tags = ["tls", "tls-1.3", "forward-secrecy", "encryption", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableTls13", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTls13")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTls13", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-disable-ssl-30-protocol",
            Label = "Disable SSL 3.0 Protocol to Prevent POODLE Attack Vulnerability",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "SSL 3.0 is vulnerable to the POODLE attack which allows a network attacker to decrypt 1 byte of plaintext per 256 crafted requests making it possible to recover session cookies and HTTPS protected content. Disabling SSL 3.0 eliminates the POODLE vulnerability and prevents any connection from falling back to SSL 3.0 even when TLS is unavailable. SSL 3.0 has been deprecated since 2015 by RFC 7568 and no legitimate modern services should require SSL 3.0 support. The POODLE attack requires network access and an ability to inject requests but is practical against browsers and HTTPS connections to web applications. Organizations that have not explicitly disabled SSL 3.0 may still have it available in Windows Schannel as a legacy fallback option. Disabling SSL 3.0 together with TLS 1.0 and 1.1 should be standard practice and the combination ensures that only TLS 1.2 and 1.3 are available for secure connections.",
            Tags = ["ssl", "ssl-3.0", "poodle", "protocol-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSsl30", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSsl30")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSsl30", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-restrict-cipher-suite-order",
            Label = "Restrict Cipher Suite Selection to Security-Approved Algorithms",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Cipher suite ordering policy restricts TLS connections to use only approved cryptographic algorithm combinations preventing negotiation of weak ciphers that might otherwise be selected by clients or servers. Unapproved cipher suites may use weak key exchange algorithms like static RSA without forward secrecy weak symmetric encryption like RC4 or DES or broken hash algorithms like MD5 or SHA-1. NIST SP 800-52 provides cipher suite selection guidance for government systems that should be referenced when defining the approved cipher list. Organizations should review their cipher suite configuration against current NIST or other applicable guidance to identify suites that should be removed. Cipher suite restrictions apply to all Schannel consumers including IIS RDP SMB and PowerShell Remoting connections reducing the attack surface across multiple protocols simultaneously. Regular review of cipher suite policy is necessary as new vulnerabilities in existing algorithms may require removing previously approved suites.",
            Tags = ["tls", "cipher-suites", "cryptographic-agility", "encryption", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictCipherSuiteOrder", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictCipherSuiteOrder")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictCipherSuiteOrder", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-enable-extended-master-secret",
            Label = "Enable Extended Master Secret Support for TLS Session Resumption",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Extended Master Secret is a TLS extension defined in RFC 7627 that mitigates triple handshake attacks and synchronization attacks on TLS session resumption by binding the master secret to the full handshake transcript. Without Extended Master Secret TLS session resumption is vulnerable to triple handshake attacks that allow a MITM attacker to inject themselves into resumed sessions. Enabling Extended Master Secret support ensures that all TLS connections and session resumptions include the cryptographic binding that prevents these attack classes. The extension is widely supported by modern TLS implementations and enabling it typically does not cause compatibility issues with current services. Organizations should verify that their internal TLS services support Extended Master Secret to avoid session resumption failures when the extension is required for accepting connections. Extended Master Secret is required for TLS 1.3 connections by specification so enabling it for TLS 1.2 connections aligns behavior with TLS 1.3 requirements.",
            Tags = ["tls", "extended-master-secret", "session-resumption", "mitm-prevention", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableExtendedMasterSecret", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableExtendedMasterSecret")],
            DetectOps = [RegOp.CheckDword(Key, "EnableExtendedMasterSecret", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-disable-rc4-cipher",
            Label = "Disable RC4 Stream Cipher for All TLS and Secure Channel Connections",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "RC4 is a stream cipher with multiple known weaknesses including biases in its keystream that allow statistical decryption attacks with sufficient ciphertext samples. RFC 7465 prohibits the use of RC4 cipher suites in TLS and its use in active connections is considered a security vulnerability. Practical attacks against RC4 in TLS have been demonstrated requiring approximately 2 billion samples to recover session cookies with high confidence. Disabling RC4 at the system level ensures that Windows Schannel will not use RC4 in any context even when peer systems offer it as an option. RC4 was historically used as a performance optimization over AES but modern hardware AES-NI instructions make AES block ciphers faster than RC4 eliminating the performance justification for using RC4. Organizations should verify that RC4 is disabled in all cryptographic library configurations and not just the Windows Schannel context.",
            Tags = ["rc4", "cipher-security", "weak-ciphers", "tls-hardening", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRC4Cipher", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRC4Cipher")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRC4Cipher", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-require-certificate-revocation-check",
            Label = "Require Certificate Revocation Status Checks for TLS Connections",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Certificate revocation checking verifies that TLS certificates have not been revoked before accepting them as valid ensuring that compromised or mis-issued certificates are not trusted. Without revocation checking a certificate that has been revoked due to private key compromise can still be used to impersonate a legitimate service. Revocation checking should use OCSP stapling for performance where the server provides current revocation status in the TLS handshake without requiring the client to contact an OCSP responder. Hard-fail revocation checking should be configured to reject connections when revocation status cannot be determined rather than allowing connections with an unknown revocation status. Organizations should ensure that their internal certificate infrastructure provides accessible OCSP and CRL endpoints so that revocation checking does not fail for internal certificates. The performance impact of revocation checking should be measured and OCSP stapling or OCSP response caching should be deployed to minimize latency impact.",
            Tags = ["tls", "certificate-revocation", "ocsp", "pki", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireCertRevocationCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireCertRevocationCheck")],
            DetectOps = [RegOp.CheckDword(Key, "RequireCertRevocationCheck", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-enable-certificate-transparency-audit",
            Label = "Enable Certificate Transparency Verification for Public TLS Certificates",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Certificate Transparency verification checks that TLS certificates are logged in public append-only CT logs before trusting them helping detect mis-issued certificates from compromised or rogue certificate authorities. CT logs provide public accountability for certificate issuance enabling organizations to monitor for unauthorized certificates issued for their domains. Requiring CT compliance helps detect man-in-the-middle attacks that use certificates issued by compromised CAs that might not have been publicly logged. Organizations should monitor Certificate Transparency logs for their domains to detect unauthorized certificate issuance before those certificates are used in attacks. CT verification is enforced by Chrome and other modern browsers for publicly-trusted certificates and policy can extend this requirement to all system connections. Violations of CT requirements where a certificate is trusted but not CT-logged should generate security alerts for investigation.",
            Tags = ["tls", "certificate-transparency", "pki", "man-in-the-middle", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableCertificateTransparencyAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableCertificateTransparencyAudit")],
            DetectOps = [RegOp.CheckDword(Key, "EnableCertificateTransparencyAudit", 1)],
        },
        new TweakDef
        {
            Id = "seccxn-require-minimum-rsa-key-size",
            Label = "Require Minimum 2048-Bit RSA Key Size for TLS Certificate Acceptance",
            Category = "Secure Connection Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Minimum RSA key size enforcement prevents TLS connections from being established using certificates with RSA keys smaller than 2048 bits which are considered insufficiently secure against modern computational resources. RSA 1024-bit keys are factorable with determined effort using modern hardware and cloud computing resources and should not be trusted for security-sensitive connections. NIST recommends RSA 2048-bit as the minimum key size for long-term security and RSA 3072-bit for higher-assurance applications. Policy enforcement of key size minimums applies to all certificates in the TLS certificate chain not just the leaf certificate providing protection against weak intermediate CA certificates. Organizations should inventory their internal certificate infrastructure to identify any certificates with key sizes below 2048 bits and replace them before enforcing the minimum key size policy. The minimum key size policy should be reviewed periodically as computing capabilities increase and minimum recommendations may be raised.",
            Tags = ["tls", "rsa-key-size", "certificate-strength", "cryptographic-agility", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MinimumRsaKeySize", 2048)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinimumRsaKeySize")],
            DetectOps = [RegOp.CheckDword(Key, "MinimumRsaKeySize", 2048)],
        },
    ];
}
