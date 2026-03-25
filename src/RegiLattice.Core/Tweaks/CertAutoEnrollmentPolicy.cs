#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 256 — Certificate Auto-Enrollment Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment
//       HKCU\Software\Policies\Microsoft\Cryptography\AutoEnrollment
//       HKLM\SOFTWARE\Policies\Microsoft\Cryptography\PKI
internal static class CertAutoEnrollmentPolicy
{
    private const string AeLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
    private const string AeCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Cryptography\AutoEnrollment";
    private const string PkiLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\PKI";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "certae-disable-machine-autoenroll",
            Label = "Disable Machine Certificate Auto-Enrollment",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets AEPolicy=4 in the machine Cryptography AutoEnrollment policy key. "
                + "Prevents Windows from automatically requesting, renewing, or installing machine "
                + "certificates from an enterprise CA (Certificate Authority). Machine certificates "
                + "used for 802.1X, VPN, or device auth will not be auto-issued. "
                + "Values: 0=off, 4=auto-enroll on, 7=auto-enroll+update+archive. "
                + "Setting 4 means auto-enrollment is active (reverse: set 0 to disable). "
                + "Default: 4 (enabled). Recommended: 0 when PKI is managed externally.",
            Tags = ["certificate", "auto-enrollment", "pki", "machine", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Machine certificate auto-enrollment disabled; machine certs must be issued manually.",
            ApplyOps = [RegOp.SetDword(AeLm, "AEPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(AeLm, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AeLm, "AEPolicy", 0)],
        },
        new TweakDef
        {
            Id = "certae-enable-machine-autoenroll",
            Label = "Enable Machine Certificate Auto-Enrollment with Renewal",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets AEPolicy=7 in the machine Cryptography AutoEnrollment policy key. "
                + "Enables machine certificate auto-enrollment, automatic renewal of expiring certs, "
                + "and archiving of the old cert private key after renewal. Provides the most complete "
                + "automated machine certificate lifecycle management. "
                + "Recommended in enterprise environments using NDES or enterprise CA with 802.1X or VPN. "
                + "Default: 4 (no auto-renewal). Recommended: 7 if the CA supports key archiving.",
            Tags = ["certificate", "auto-enrollment", "renewal", "pki", "machine", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Machine certificates auto-enrolled and auto-renewed with key archiving enabled.",
            ApplyOps = [RegOp.SetDword(AeLm, "AEPolicy", 7)],
            RemoveOps = [RegOp.DeleteValue(AeLm, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AeLm, "AEPolicy", 7)],
        },
        new TweakDef
        {
            Id = "certae-disable-user-autoenroll",
            Label = "Disable User Certificate Auto-Enrollment",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets AEPolicy=0 in the user Cryptography AutoEnrollment policy key. "
                + "Prevents Windows from automatically requesting or renewing user certificates from an "
                + "enterprise CA on behalf of the logged-in user. User certificates for email signing/encryption "
                + "or smart card login will not be auto-issued. "
                + "Default: 4 (enabled). Recommended: 0 in environments where user certs are managed manually.",
            Tags = ["certificate", "auto-enrollment", "user", "pki", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "User certificate auto-enrollment disabled; user certs must be requested manually.",
            ApplyOps = [RegOp.SetDword(AeCu, "AEPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(AeCu, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AeCu, "AEPolicy", 0)],
        },
        new TweakDef
        {
            Id = "certae-enable-user-autoenroll",
            Label = "Enable User Certificate Auto-Enrollment with Renewal",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets AEPolicy=7 in the user Cryptography AutoEnrollment policy key. "
                + "Enables user certificate auto-enrollment, automatic renewal, and key archiving "
                + "for the current user. Appropriate in enterprise environments where users need "
                + "S/MIME email or VPN user certificates that are managed by an enterprise CA. "
                + "Default: 4. Recommended: 7 when full user certificate lifecycle management is desired.",
            Tags = ["certificate", "auto-enrollment", "user", "renewal", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "User certificates auto-enrolled and auto-renewed with key archiving for this user.",
            ApplyOps = [RegOp.SetDword(AeCu, "AEPolicy", 7)],
            RemoveOps = [RegOp.DeleteValue(AeCu, "AEPolicy")],
            DetectOps = [RegOp.CheckDword(AeCu, "AEPolicy", 7)],
        },
        new TweakDef
        {
            Id = "certae-disable-cert-expiry-alerts",
            Label = "Disable Certificate Expiry Balloon Notifications",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets ExpirationWarning=0 in the machine AutoEnrollment policy key. "
                + "Prevents Windows from generating balloon notification alerts to users when their "
                + "personal or machine certificates are approaching their expiry date. "
                + "Useful in large enterprise deployments where certificate renewal is automated and "
                + "user notifications create noise. "
                + "Default: absent (warnings shown). Recommended: 0 when renewal is fully automated.",
            Tags = ["certificate", "expiry", "notification", "balloon", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Certificate expiry balloon notifications suppressed; users do not see cert-expiry warnings.",
            ApplyOps = [RegOp.SetDword(AeLm, "ExpirationWarning", 0)],
            RemoveOps = [RegOp.DeleteValue(AeLm, "ExpirationWarning")],
            DetectOps = [RegOp.CheckDword(AeLm, "ExpirationWarning", 0)],
        },
        new TweakDef
        {
            Id = "certae-enable-certificate-logging",
            Label = "Enable Certificate Enrollment Audit Logging",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets AuditLevel=2 in the machine AutoEnrollment policy key. "
                + "Enables audit logging for each machine certificate enrollment, renewal, and deletion event. "
                + "Audit events are written to the Application or Security event log, "
                + "providing a certificate lifecycle audit trail for compliance purposes. "
                + "Default: absent (no audit log). Recommended: 2 in PCI-DSS, HIPAA, or NIST-800 environments.",
            Tags = ["certificate", "logging", "audit", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Certificate enrollment/renewal/deletion events written to the Windows Event Log.",
            ApplyOps = [RegOp.SetDword(AeLm, "AuditLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(AeLm, "AuditLevel")],
            DetectOps = [RegOp.CheckDword(AeLm, "AuditLevel", 2)],
        },
        new TweakDef
        {
            Id = "certae-disable-offline-dom-enroll",
            Label = "Disable Certificate Enrollment in Offline Domain Join",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets OfflineDomainJoinEnrollment=0 in the machine AutoEnrollment policy key. "
                + "Prevents certificate auto-enrollment from running during an offline domain join operation "
                + "(djoin.exe /provision). Ensures certificate issuance only occurs after the machine is "
                + "fully joined and network-connected to the CA. "
                + "Default: absent (offline enrollment allowed). Recommended: 0 to ensure CA-verified enrollment only.",
            Tags = ["certificate", "offline", "domain-join", "enrollment", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Certificate enrollment suppressed during offline domain join; only online CA-verified enrollment allowed.",
            ApplyOps = [RegOp.SetDword(AeLm, "OfflineDomainJoinEnrollment", 0)],
            RemoveOps = [RegOp.DeleteValue(AeLm, "OfflineDomainJoinEnrollment")],
            DetectOps = [RegOp.CheckDword(AeLm, "OfflineDomainJoinEnrollment", 0)],
        },
        new TweakDef
        {
            Id = "certae-enable-key-based-renewal",
            Label = "Enable Key-Based Certificate Renewal",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets EnableKeyBasedRenewal=1 in the PKI policy key. "
                + "Allows Windows to perform key-based certificate renewal (KBR) where the same private key "
                + "is reused across certificate renewals instead of generating a new key. "
                + "Reduces PKI overhead in large environments and avoids re-deploying new public keys. "
                + "Default: absent (KBR disabled). Recommended: 1 when CA supports KBR via SCEP or NDES.",
            Tags = ["certificate", "renewal", "key-reuse", "pki", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Key-based renewal allowed; same private key reused during certificate renewal.",
            ApplyOps = [RegOp.SetDword(PkiLm, "EnableKeyBasedRenewal", 1)],
            RemoveOps = [RegOp.DeleteValue(PkiLm, "EnableKeyBasedRenewal")],
            DetectOps = [RegOp.CheckDword(PkiLm, "EnableKeyBasedRenewal", 1)],
        },
        new TweakDef
        {
            Id = "certae-disable-pki-url-retrieval",
            Label = "Disable URL-Based PKI Object Retrieval",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets EnableCertChainValidation=0 in the PKI policy key. "
                + "Prevents Windows from automatically fetching CRL, OCSP, and AIA objects from "
                + "URLs embedded in certificates (when the system cannot reach external URLs). "
                + "Avoids long validation delays on air-gapped or internet-blocked machines where "
                + "certificate path building would otherwise time out. "
                + "Default: absent (URL retrieval enabled). Recommended: 0 on offline/air-gapped systems.",
            Tags = ["certificate", "crl", "ocsp", "url", "pki", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "CRL and OCSP URL lookups disabled; certificate chain validation may not check revocation status.",
            ApplyOps = [RegOp.SetDword(PkiLm, "EnableCertChainValidation", 0)],
            RemoveOps = [RegOp.DeleteValue(PkiLm, "EnableCertChainValidation")],
            DetectOps = [RegOp.CheckDword(PkiLm, "EnableCertChainValidation", 0)],
        },
        new TweakDef
        {
            Id = "certae-set-weak-cert-blocking",
            Label = "Enable Weak Certificate Algorithm Blocking",
            Category = "Certificate Auto-Enrollment Policy",
            Description =
                "Sets DisableWeakSignatures=1 in the PKI policy key. "
                + "Instructs the Windows certificate chain engine to reject certificates that use "
                + "weak signature algorithms (SHA-1 below certain key lengths, MD5, MD2) when "
                + "validating certificate chains. Enforces modern cryptographic standards for all "
                + "certificate operations including auto-enrollment and chain building. "
                + "Default: absent. Recommended: 1 on systems that must enforce SHA-2/SHA-3 only.",
            Tags = ["certificate", "weak-algorithm", "sha1", "blocking", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Certificates using weak signature algorithms (SHA-1, MD5) rejected during validation.",
            ApplyOps = [RegOp.SetDword(PkiLm, "DisableWeakSignatures", 1)],
            RemoveOps = [RegOp.DeleteValue(PkiLm, "DisableWeakSignatures")],
            DetectOps = [RegOp.CheckDword(PkiLm, "DisableWeakSignatures", 1)],
        },
    ];
}
