// RegiLattice.Core — Tweaks/CertValidationPolicy.cs
// Sprint 306: Certificate Validation Policy tweaks (10 tweaks)
// Category: "Certificate Validation Policy" | Slug: certvld
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CertValidity

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CertValidationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CertValidity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "certvld-disable-auto-root-update",
            Label = "Disable Automatic Root Certificate Update",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Windows automatically downloads updated root certificate trust lists from Microsoft's servers to expand the set of trusted certificate authorities. Disabling automatic root certificate updates prevents new root CAs from being trusted without administrator review. Enterprise certificate trust should be managed through enterprise PKI policies and controlled root certificate distribution. Automatically trusting new root certificates from Microsoft sources could expand the trust domain without security team review. Air-gapped and strictly controlled environments cannot permit outbound connections to certificate infrastructure and must disable this feature. Disabling auto-update requires maintaining the root certificate store manually through Windows Update or offline distribution.",
            Tags = ["certificates", "pki", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoRootCertUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoRootCertUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoRootCertUpdate", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-revocation-check",
            Label = "Enforce Certificate Revocation Checking",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Certificate revocation checking verifies that certificates presented during TLS connections have not been revoked by the issuing certificate authority. Enforcing revocation checking ensures that compromised or misissued certificates cannot be used to impersonate services. Without revocation checking, certificates from compromised private keys remain valid until expiry rather than being immediately blocked. Enterprise environments must enforce revocation checking to detect use of revoked certificates used in MITM attacks. OCSP stapling and CRL caching provide efficient revocation checking without adding significant latency to connections. Disabling this tweak (which enforces checking) reintroduces the risk of accepting revoked certificates.",
            Tags = ["certificates", "revocation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceRevocationChecking", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceRevocationChecking")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceRevocationChecking", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-ocsp-staple-bypass",
            Label = "Disable OCSP Stapling Bypass",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "OCSP stapling allows servers to attach pre-fetched revocation status information to TLS handshakes, improving performance for revocation checking. The OCSP staple bypass allows revocation checking to succeed even when stapled OCSP responses are absent or expired. Disabling OCSP staple bypass enforces strict revocation checking that requires a valid stapled response or direct OCSP query. Servers without valid stapled OCSP responses may cause connection failures when strict mode is enforced. This setting should be paired with a reliable OCSP infrastructure to prevent availability disruption from OCSP server outages. Strict OCSP enforcement maximizes revocation checking effectiveness against compromised certificate use.",
            Tags = ["certificates", "ocsp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOcspStapleBypass", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOcspStapleBypass")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOcspStapleBypass", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-certificate-propagation",
            Label = "Disable Automatic Certificate Propagation",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Certificate propagation automatically distributes user certificates and trusted root certificates from smart cards and external sources into the Windows certificate store. Disabling automatic certificate propagation prevents certificates from being automatically installed from inserted smart cards or other sources. Auto-propagated certificates may expand the enterprise trust domain without deliberate review by the security team. Malicious smart cards or infected media containing certificates could introduce untrusted or backdoored CA roots through propagation. Enterprise certificate management should use Group Policy certificate distribution rather than automatic client-side propagation. Disabling propagation is appropriate for high-security environments where the certificate store must be tightly controlled.",
            Tags = ["certificates", "propagation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCertPropagation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCertPropagation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCertPropagation", 1)],
        },
        new TweakDef
        {
            Id = "certvld-enable-cert-padding-check",
            Label = "Enable Certificate Padding Check",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Certificate padding checks detect and reject certificates with non-standard trailing data appended after the certificate's normal ASN.1 structure. Enabling certificate padding checks prevents use of certificate padding attack techniques that exploit certificate parser flexibility. Some certificate parsing exploits embed malicious code or additional certificate data in the padding area after the valid certificate. Certificate normalization attacks rely on different parsers handling certificate padding differently, creating trust inconsistencies. Enforcing strict padding validation ensures all certificate parsers in the Windows ecosystem handle certificates consistently. Enabling padding checks has no impact on legitimate well-formed certificates that conform to the RFC standard.",
            Tags = ["certificates", "security", "padding", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableCertPaddingCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableCertPaddingCheck")],
            DetectOps = [RegOp.CheckDword(Key, "EnableCertPaddingCheck", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-weak-algorithms",
            Label = "Disable Weak Certificate Signature Algorithms",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Weak signature algorithms like MD5 and SHA-1 are cryptographically broken and should not be trusted in certificate chains. Disabling weak signature algorithms prevents Windows from accepting certificates signed with deprecated hash algorithms. MD5-signed certificates have been successfully forged in practical attacks demonstrating the urgency of removing this trust. SHA-1 certificates are no longer considered secure against adversaries with significant compute resources. Enterprise PKI should have migrated to SHA-256 or stronger hash algorithms at all levels of the certificate chain. Enforcement requires that all certificates in the validation path use current approved hash algorithms.",
            Tags = ["certificates", "algorithms", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWeakAlgorithms", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWeakAlgorithms")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWeakAlgorithms", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-expired-cert-bypass",
            Label = "Disable Expired Certificate Bypass",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Certificate expiry bypass allows applications to accept expired certificates in scenarios where the system cannot verify the current time or revocation status. Disabling expired certificate bypass enforces strict expiry checking and rejects all expired certificates regardless of context. Applications that bypass expiry checking may accept certificates that were valid at a past point in time but have since expired. Expired certificates may have been replaced due to private key compromise making their continued acceptance dangerous. Enterprise TLS infrastructure should maintain valid certificates with appropriate expiry margins and not require bypass mechanisms. Disabling expiry bypass hardens the certificate validation path against time-based certificate acceptance attacks.",
            Tags = ["certificates", "expiry", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableExpiredCertBypass", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableExpiredCertBypass")],
            DetectOps = [RegOp.CheckDword(Key, "DisableExpiredCertBypass", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-cert-chain-building",
            Label = "Restrict Certificate Chain Building to Local Store Only",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Certificate chain building downloads additional intermediate CA certificates from URLs embedded in presented certificates when intermediate CAs are missing. Restricting chain building to the local store only prevents outbound connections to fetch intermediate CA certificates during validation. Certificate download requests reveal connection details to external certificate infrastructure and create a potential availability dependency. In high-security environments certificate chains should be fully pre-distributed through Group Policy rather than dynamically downloaded. Dynamic certificate downloads represent an outbound channel that may be restricted by security controls in air-gapped environments. This setting requires that all necessary intermediate CA certificates be pre-deployed to the enterprise endpoints.",
            Tags = ["certificates", "chain", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictChainBuildingToLocalStore", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictChainBuildingToLocalStore")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictChainBuildingToLocalStore", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-cert-transparency",
            Label = "Enable Certificate Transparency Enforcement",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Certificate Transparency requires TLS certificates to be logged in public append-only logs before browsers and applications trust them. Enabling CT enforcement causes Windows to reject certificates not logged in trusted Certificate Transparency logs. CT enforcement detects misissued certificates from compromised or rogue CAs that were not properly logged. Without CT enforcement, misissued certificates for enterprise domains may go undetected for extended periods. CT logs create a public auditable record of all trusted certificates making unauthorized certificate issuance discoverable. Enforcing CT provides protection against MITM attacks using misissued but otherwise valid-looking certificates.",
            Tags = ["certificates", "transparency", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableCertificateTransparency", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableCertificateTransparency")],
            DetectOps = [RegOp.CheckDword(Key, "EnableCertificateTransparency", 1)],
        },
        new TweakDef
        {
            Id = "certvld-disable-cert-telemetry",
            Label = "Disable Certificate Validation Telemetry",
            Category = "Certificate Validation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Certificate validation telemetry reports certificate usage events, validation failures, and chain building statistics to Microsoft. This telemetry helps Microsoft identify problematic certificates and improve certificate infrastructure across the Windows ecosystem. Disabling certificate validation telemetry prevents certificate usage data from being sent from enterprise endpoints. Certificate validation data revealing which certificates are used for enterprise connections constitutes sensitive security configuration information. Enterprise PKI operations and certificate health monitoring should be managed through internal CA management tools. Certificate validation and enforcement functions operate normally regardless of this telemetry setting.",
            Tags = ["certificates", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCertValidationTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCertValidationTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCertValidationTelemetry", 1)],
        },
    ];
}
