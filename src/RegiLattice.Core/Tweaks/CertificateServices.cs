// RegiLattice.Core — Tweaks/CertificateServices.cs
// Windows PKI / Certificate Services policy tweaks (Sprint 107).
// Slug: "certpol-*" — controls auto-enrollment, root store policy,
// OCSP/CRL caching, and trusted-publisher enforcement.
// Registry bases:
//   HKLM\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment
//   HKLM\SOFTWARE\Policies\Microsoft\SystemCertificates
//   HKLM\SOFTWARE\Microsoft\Cryptography\OID\EncodingType 0\CertDllOpenStoreProv\Ldap
//   HKCU\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CertificateServices
{
    private const string AutoEnrollMachine = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
    private const string AutoEnrollUser = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
    private const string CertPolicies = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates\Root\ProtectedRoots";
    private const string CertCrlPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates";
    private const string CryptBasePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";
    private const string CaClientsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
    private const string TrustProviderPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "certpol-enable-machine-autoenroll",
            Label = "Certificates: Enable Machine Auto-Enrollment",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [AutoEnrollMachine],
            Tags = ["certificates", "pki", "auto-enrollment", "security"],
            Description =
                "Sets AEPolicy=7 in the Cryptography\\AutoEnrollment machine policy. "
                + "Value 7 enables full auto-enrollment: automatically request, renew, and publish "
                + "machine certificates to the Personal store when templates are available in AD. "
                + "Requires domain membership and an enterprise CA. Reduces certificate expiry surprises.",
            ApplyOps = [RegOp.SetDword(AutoEnrollMachine, "AEPolicy", 7)],
            RemoveOps = [RegOp.DeleteValue(AutoEnrollMachine, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AutoEnrollMachine, "AEPolicy", 7)],
        },
        new TweakDef
        {
            Id = "certpol-disable-machine-autoenroll",
            Label = "Certificates: Disable Machine Auto-Enrollment",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [AutoEnrollMachine],
            Tags = ["certificates", "pki", "auto-enrollment", "security"],
            Description =
                "Sets AEPolicy=0 in the Cryptography\\AutoEnrollment machine policy. "
                + "Disables automatic certificate enrollment and renewal on the machine. "
                + "Use on standalone workstations not joined to a PKI-enabled AD domain.",
            ApplyOps = [RegOp.SetDword(AutoEnrollMachine, "AEPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(AutoEnrollMachine, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AutoEnrollMachine, "AEPolicy", 0)],
        },
        new TweakDef
        {
            Id = "certpol-enable-user-autoenroll",
            Label = "Certificates: Enable User Auto-Enrollment",
            Category = "Certificate Services",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [AutoEnrollUser],
            Tags = ["certificates", "pki", "auto-enrollment", "user", "security"],
            Description =
                "Sets AEPolicy=7 in the Cryptography\\AutoEnrollment user policy. "
                + "Enables per-user auto-enrollment so that user certificates (e.g. email signing, "
                + "smart-card logon) are automatically requested and renewed against the CA. "
                + "Requires domain membership and matching certificate templates.",
            ApplyOps = [RegOp.SetDword(AutoEnrollUser, "AEPolicy", 7)],
            RemoveOps = [RegOp.DeleteValue(AutoEnrollUser, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AutoEnrollUser, "AEPolicy", 7)],
        },
        new TweakDef
        {
            Id = "certpol-protect-root-store",
            Label = "Certificates: Protect Root CA Store from User Modification",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 5,
            RegistryKeys = [CertPolicies],
            Tags = ["certificates", "pki", "root-ca", "security", "hardening"],
            Description =
                "Sets Flags=1 in SystemCertificates\\Root\\ProtectedRoots. "
                + "Prevents standard users from adding or removing Root CA certificates from the "
                + "Trusted Root Certification Authorities store. Blocks MITM attacks that rely on "
                + "a user silently accepting a malicious root certificate.",
            ApplyOps = [RegOp.SetDword(CertPolicies, "Flags", 1)],
            RemoveOps = [RegOp.DeleteValue(CertPolicies, "Flags")],
            DetectOps = [RegOp.CheckDword(CertPolicies, "Flags", 1)],
        },
        new TweakDef
        {
            Id = "certpol-disable-crl-url-retrieval",
            Label = "Certificates: Disable Automatic CRL/OCSP URL Retrieval for Offline Scenarios",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 4,
            RegistryKeys = [CryptBasePolicy],
            Tags = ["certificates", "pki", "crl", "ocsp", "offline", "security"],
            Description =
                "Sets DisableOCSP=1 and DisableCRLFetch=1 in the Cryptography policy. "
                + "Stops Windows from fetching CRL and OCSP responses over the network. "
                + "Use in air-gapped or offline environments where the CRL distribution point is "
                + "unreachable and causing 30-second delays.",
            ApplyOps = [RegOp.SetDword(CryptBasePolicy, "DisableOCSP", 1), RegOp.SetDword(CryptBasePolicy, "DisableCRLFetch", 1)],
            RemoveOps = [RegOp.DeleteValue(CryptBasePolicy, "DisableOCSP"), RegOp.DeleteValue(CryptBasePolicy, "DisableCRLFetch")],
            DetectOps = [RegOp.CheckDword(CryptBasePolicy, "DisableOCSP", 1), RegOp.CheckDword(CryptBasePolicy, "DisableCRLFetch", 1)],
        },
        new TweakDef
        {
            Id = "certpol-require-publishers-approval",
            Label = "Certificates: Require Trusted Publishers Approval for Unsigned Code",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 4,
            RegistryKeys = [TrustProviderPolicy],
            Tags = ["certificates", "pki", "trusted-publisher", "software-restriction", "security"],
            Description =
                "Sets PolicyScope=1 in Software Restriction Policies. "
                + "Requires that software is signed by a trusted publisher before execution. "
                + "Raises the bar for malware that relies on self-signed or unsigned executables. "
                + "Users can add exceptions via the Trusted Publishers store.",
            ApplyOps = [RegOp.SetDword(TrustProviderPolicy, "PolicyScope", 1)],
            RemoveOps = [RegOp.DeleteValue(TrustProviderPolicy, "PolicyScope")],
            DetectOps = [RegOp.CheckDword(TrustProviderPolicy, "PolicyScope", 1)],
        },
        new TweakDef
        {
            Id = "certpol-strong-key-protection",
            Label = "Certificates: Enforce Strong Private-Key Protection",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [CryptBasePolicy],
            Tags = ["certificates", "pki", "private-key", "security", "hardening"],
            Description =
                "Sets ForceKeyProtection=2 in the Cryptography policy. "
                + "Forces a PIN/password dialog each time a private key is used, preventing "
                + "background applications from silently accessing stored private keys. "
                + "0=no protection, 1=warn on first use, 2=require password always.",
            ApplyOps = [RegOp.SetDword(CryptBasePolicy, "ForceKeyProtection", 2)],
            RemoveOps = [RegOp.DeleteValue(CryptBasePolicy, "ForceKeyProtection")],
            DetectOps = [RegOp.CheckDword(CryptBasePolicy, "ForceKeyProtection", 2)],
        },
        new TweakDef
        {
            Id = "certpol-disable-sha1-for-certs",
            Label = "Certificates: Block SHA-1 Certificates for Server Authentication",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 4,
            RegistryKeys = [CryptBasePolicy],
            Tags = ["certificates", "pki", "sha1", "tls", "security", "hardening"],
            Description =
                "Sets HashBlockList=\"sha1\" in the Cryptography policy. "
                + "Instructs Windows' chain-building engine to reject SHA-1-signed certificates "
                + "for server authentication (TLS). SHA-1 was deprecated by all major CAs in 2017; "
                + "SHA-1 certificates are now considered insecure for TLS.",
            ApplyOps = [RegOp.SetString(CryptBasePolicy, "HashBlockList", "sha1")],
            RemoveOps = [RegOp.DeleteValue(CryptBasePolicy, "HashBlockList")],
            DetectOps = [RegOp.CheckString(CryptBasePolicy, "HashBlockList", "sha1")],
        },
        new TweakDef
        {
            Id = "certpol-log-cert-chain-errors",
            Label = "Certificates: Enable Certificate Chain Validation Logging",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [CertCrlPolicy],
            Tags = ["certificates", "pki", "audit", "logging", "security"],
            Description =
                "Sets ChainEngineMonitor=1 in SystemCertificates policy. "
                + "Enables verbose audit logging for certificate chain validation failures to the "
                + "Windows Security event log. Useful for diagnosing TLS handshake failures and "
                + "for detecting certificate-based MITM interception attempts.",
            ApplyOps = [RegOp.SetDword(CertCrlPolicy, "ChainEngineMonitor", 1)],
            RemoveOps = [RegOp.DeleteValue(CertCrlPolicy, "ChainEngineMonitor")],
            DetectOps = [RegOp.CheckDword(CertCrlPolicy, "ChainEngineMonitor", 1)],
        },
        new TweakDef
        {
            Id = "certpol-disable-explicit-user-trust",
            Label = "Certificates: Prevent Users from Trusting Certificates Manually",
            Category = "Certificate Services",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [AutoEnrollMachine],
            Tags = ["certificates", "pki", "security", "hardening", "user-trust"],
            Description =
                "Sets AllowUserCertTrust=0 in the AutoEnrollment policy. "
                + "Blocks interactive users from adding untrusted certificates to their personal trust "
                + "store via browser certificate-error dialogs. Certificates must be pushed via GPO "
                + "or enterprise CA enrollment, preventing social-engineering trust attacks.",
            ApplyOps = [RegOp.SetDword(AutoEnrollMachine, "AllowUserCertTrust", 0)],
            RemoveOps = [RegOp.DeleteValue(AutoEnrollMachine, "AllowUserCertTrust")],
            DetectOps = [RegOp.CheckDword(AutoEnrollMachine, "AllowUserCertTrust", 0)],
        },
    ];
}
