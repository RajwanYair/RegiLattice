// RegiLattice.Core — Tweaks/TrustProviderPolicy.cs
// Sprint 320: Trust Provider Policy tweaks (10 tweaks)
// Category: "Trust Provider Policy" | Slug: trustprov
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TrustProvider

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TrustProviderPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TrustProvider";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "trustprov-require-trust-chain",
            Label = "Require Complete Certificate Trust Chain",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Complete certificate trust chain validation ensures that every certificate in a chain traces back to a trusted root CA without gaps. Requiring complete trust chains prevents acceptance of certificates with missing intermediate CAs or broken trust paths. Incomplete certificate chains are a common misconfiguration that can allow man-in-the-middle attacks when clients accept partial chains. Windows Trust Provider APIs are used by Authenticode, WinHTTP, Schannel, and other Windows security components for certificate validation. Trust chain requirements help ensure that all security components validate certificates consistently through a configurable policy. Enterprise deployments should ensure their internal PKI certificates deploy complete chains to avoid legitimate certificate acceptance failures.",
            Tags = ["trust", "certificates", "pki", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireCompleteTrustChain", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireCompleteTrustChain")],
            DetectOps = [RegOp.CheckDword(Key, "RequireCompleteTrustChain", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-enable-revocation-check",
            Label = "Enable Certificate Revocation Checking",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Certificate revocation checking validates that a certificate has not been revoked by the CA before trusting it for signature verification. Enabling revocation checking through OCSP or CRL prevents accepting certificates that have been revoked due to compromise or policy violation. Revocation checking is critical for catching certificates that were stolen or issued to unauthorized parties after the CA discovered the issue. Windows Trust Provider supports both Online Certificate Status Protocol and Certificate Revocation List distribution point revocation checks. Revocation checking requires network connectivity to CRL distribution points or OCSP responders which should be accessible from enterprise endpoints. Failing to check revocation allows system compromise even after a certificate has been reported as revoked.",
            Tags = ["trust", "revocation", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRevocationChecking", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRevocationChecking")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRevocationChecking", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-block-expired-certificates",
            Label = "Block Expired Authenticode Certificates",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Expired Authenticode certificates should not be trusted for code signing as the CA can no longer guarantee the integrity of the certificate holder. Blocking expired certificates prevents software signed with certificates past their validity period from being accepted without timestamp countersignatures. Windows by default accepts PE files signed with expired certificates if signed before expiry and timestamped but policy can enforce stricter requirements. Many malware families use expired or revoked certificates to avoid detection while appearing to be legitimately signed. Expired certificate blocking forces vendors to keep certificates current and reduces the window of malicious exploitation of certificate compromise. Organizations should inventory software signed with near-expiry certificates before enabling expired certificate blocking to prevent deployment disruption.",
            Tags = ["trust", "expired", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockExpiredCertificates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockExpiredCertificates")],
            DetectOps = [RegOp.CheckDword(Key, "BlockExpiredCertificates", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-require-timestamping",
            Label = "Require Timestamp Countersignature for Code Signing",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Timestamp countersignatures provide a trusted time binding for code signatures ensuring they remain valid even after the signing certificate expires. Requiring timestamping prevents distribution of signed code without a verifiable trust anchor to authentication events before certificate expiry. Authenticode timestamps from trusted TSAs embed the signing time in the signature and allow validation of signatures made before certificate revocation. Code signing without timestamps becomes invalid when the signing certificate expires which can trigger false security alerts on legitimate software. RFC 3161 timestamp authorities from trusted providers should be used and are required for Extended Validation code signing compliance. Timestamp requirements reduce the practical window for attacks using stolen certificates by limiting their usefulness to the TSA-recorded window.",
            Tags = ["trust", "timestamp", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireTimestampCountersig", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireTimestampCountersig")],
            DetectOps = [RegOp.CheckDword(Key, "RequireTimestampCountersig", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-restrict-trust-to-enterprise-ca",
            Label = "Restrict Code Trust to Enterprise CA",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Restricting code trust to enterprise CAs limits which certificate authorities can issue certificates accepted for code signing on managed endpoints. Enterprise CA restriction ensures that internally deployed software must be signed by the enterprise PKI rather than arbitrary commercial CAs. This policy supports zero-trust models for application execution where only IT-managed code signing authorities are recognized. Restricting to enterprise CAs prevents attackers who obtain certificates from public CAs from deploying signed malware on restricted endpoints. PKI pinning through enterprise certificate stores is the technical mechanism for implementing enterprise CA code signing restrictions. Enterprise CA restriction may require re-signing vendor software with enterprise certificates which adds PKI management overhead.",
            Tags = ["trust", "enterprise-ca", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictTrustToEnterpriseCA", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictTrustToEnterpriseCA")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictTrustToEnterpriseCA", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-block-md5-signatures",
            Label = "Block MD5-Based Certificate Signatures",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "MD5-based certificate signatures are cryptographically broken and should not be trusted for any security-sensitive operation. Blocking MD5 signatures prevents certificates signed with the MD5 hash algorithm from being accepted by Windows Trust Provider. MD5 collisions are computationally feasible and have been demonstrated in attacks against certificate authorities that resulted in fraudulent CA certificates. Windows deprecated MD5 in certificate chains in 2009 but policy enforcement ensures no legacy applications re-enable MD5 trust. Any certificates in the enterprise environment still using MD5 should be identified and replaced immediately. MD5 blocking is defense-in-depth ensuring that even if a deprecated component attempts to accept MD5 certificates trust provider policy prevents acceptance.",
            Tags = ["trust", "md5", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockMD5Signatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockMD5Signatures")],
            DetectOps = [RegOp.CheckDword(Key, "BlockMD5Signatures", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-block-sha1-code-signing",
            Label = "Block SHA-1 Authenticode Code Signatures",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "SHA-1 is cryptographically weak and has demonstrated practical collision vulnerabilities that can be exploited to forge signatures. Blocking SHA-1 Authenticode signatures ensures that all code signing certificates use SHA-256 or stronger algorithms. Microsoft announced deprecation of SHA-1 certificate chain validation in Authenticode starting in January 2016 but policy enforcement provides explicit control. Legacy software signed with SHA-1 certificates should be re-signed with SHA-256 before this policy is enforced. SHA-1 blocking for Authenticode is separate from SHA-1 blocking for TLS certificates and must be configured specifically for code signing trust. Enterprise environments with legacy signed software must audit and replace SHA-1 signed executables before enabling blocking to prevent application disruption.",
            Tags = ["trust", "sha1", "authenticode", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockSHA1CodeSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockSHA1CodeSigning")],
            DetectOps = [RegOp.CheckDword(Key, "BlockSHA1CodeSigning", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-audit-revocation-failures",
            Label = "Audit Certificate Revocation Check Failures",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Certificate revocation check failures occur when revocation status cannot be determined due to network issues or unavailable CRL distribution points. Auditing revocation failures provides visibility into potential certificate validation problems that could indicate network blocking or misconfiguration. Persistent revocation check failures may indicate that CRL distribution point URLs are inaccessible from the endpoint or OCSP responders are down. Logging revocation failures to the security event log enables SIEM correlation to identify compromised endpoints blocking revocation checks. An attacker who gains network-level access might attempt to block revocation checking to maintain access via revoked certificates. Revocation failure auditing should be combined with a policy decision about whether to fail-open or fail-closed when revocation checking is unavailable.",
            Tags = ["trust", "revocation", "audit", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditRevocationFailures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditRevocationFailures")],
            DetectOps = [RegOp.CheckDword(Key, "AuditRevocationFailures", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-enable-ev-code-signing",
            Label = "Prefer Extended Validation Code Signing Certificates",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Extended Validation code signing certificates require rigorous identity verification by the CA providing higher assurance than standard Organization Validation certificates. Preferring EV code signing provides higher trust signals in SmartScreen and Windows security UIs for EV-signed software. EV certificates immediately unlock SmartScreen reputation whereas OV certificates require building reputation over time through downloads. Malware operators rarely obtain EV certificates due to the identity verification requirements making EV an effective higher-trust signal. Enterprise software distribution should prefer EV signing for critical infrastructure components and software distributed externally. EV certificate preference policy provides behavioral guidance to users and security tooling that can differentiate between EV and non-EV signatures.",
            Tags = ["trust", "ev-certificate", "authenticode", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreferEVCodeSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreferEVCodeSigning")],
            DetectOps = [RegOp.CheckDword(Key, "PreferEVCodeSigning", 1)],
        },
        new TweakDef
        {
            Id = "trustprov-log-trust-decisions",
            Label = "Enable Trust Decision Audit Logging",
            Category = "Trust Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Trust decision audit logging records all certificate verification decisions made by Windows Trust Provider components during normal system operation. Enabling trust decision logging provides a comprehensive audit trail of all code signing verifications, TLS validations, and certificate trust decisions. Trust decision logs help identify attempts to run untrusted software, access systems with invalid certificates, and trust policy violations. Security teams can use trust decision events to detect lateral movement techniques that involve running unsigned tools across the network. Trust logging events can be correlated with other security events to understand the full scope of an intrusion or attack campaign. Trust decision audit logs should be collected and retained according to enterprise log retention policies for post-incident forensic analysis.",
            Tags = ["trust", "audit", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableTrustDecisionLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTrustDecisionLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTrustDecisionLogging", 1)],
        },
    ];
}
