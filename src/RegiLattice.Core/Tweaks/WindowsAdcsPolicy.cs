// RegiLattice.Core — Tweaks/WindowsAdcsPolicy.cs
// Active Directory Certificate Services (ADCS) Security Policy — Sprint 434.
// Controls certificate services security settings via Cryptography and Smart Card
// Group Policy registry paths: algorithm policies, root store protection, CNG provider
// restrictions, smart card PIN lock, and key archival controls.
// Category: "ADCS Policy" | Slug: adcspol
// Registry paths:
//   HKLM\SOFTWARE\Policies\Microsoft\Cryptography         — cryptography policy
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider — smart card
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\CertSvc       — certificate services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsAdcsPolicy
{
    private const string CryptoKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";
    private const string SmartKey    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
    private const string CertSvcKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CertSvc";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id          = "adcspol-protect-root-store",
                Label       = "Protect Root Certificate Store Against Modification",
                Category    = "ADCS Policy",
                Description = "Sets ProtectedRootsAllowedToPerformCRLRetrieval=0 in the Cryptography policy. Prevents non-administrative processes from retrieving CRL (Certificate Revocation List) data through the Protected Roots API. Restricting this access reduces the risk of CRL poisoning attacks that could mark legitimate certificates as revoked or prevent revocation checks from completing correctly.",
                Tags        = ["adcs", "certificate", "root-store", "crl", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Restricts CRL retrieval through Protected Roots; may affect non-admin CRL checking in some scenarios.",
                ApplyOps    = [RegOp.SetDword(CryptoKey, "ProtectedRootsAllowedToPerformCRLRetrieval", 0)],
                RemoveOps   = [RegOp.DeleteValue(CryptoKey, "ProtectedRootsAllowedToPerformCRLRetrieval")],
                DetectOps   = [RegOp.CheckDword(CryptoKey, "ProtectedRootsAllowedToPerformCRLRetrieval", 0)],
            },
            new TweakDef
            {
                Id          = "adcspol-disable-auto-root-update",
                Label       = "Disable Automatic Root Certificate Update",
                Category    = "ADCS Policy",
                Description = "Sets DisableRootAutoUpdate=1 in the Cryptography policy. Prevents Windows from automatically downloading and installing new trusted root certificates from the Windows Update Certificate Distribution Point (CDP). In air-gapped or highly regulated environments this prevents silent addition of untrusted or government-mandated CA certificates. Organisations must manage root store updates manually through their own PKI if this is enabled.",
                Tags        = ["adcs", "certificate", "root-update", "policy", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote  = "Blocks automatic root cert updates from Windows Update; manage root store manually to avoid revocation issues.",
                ApplyOps    = [RegOp.SetDword(CryptoKey, "DisableRootAutoUpdate", 1)],
                RemoveOps   = [RegOp.DeleteValue(CryptoKey, "DisableRootAutoUpdate")],
                DetectOps   = [RegOp.CheckDword(CryptoKey, "DisableRootAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id          = "adcspol-enforce-strong-key-protection",
                Label       = "Enforce Strong Private Key Protection",
                Category    = "ADCS Policy",
                Description = "Sets ForceKeyProtection=2 in the Cryptography policy. Requires a user password confirmation every time a certificate's private key is accessed by an application. Values: 0=no protection, 1=notify on first use, 2=require password for every use. Level 2 ensures that private key operations cannot happen silently in the background, protecting against malware that attempts to sign data or decrypt sensitive content using stored keys.",
                Tags        = ["adcs", "certificate", "private-key", "policy", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Prompts for key password on every private-key operation; may interrupt automated signing workflows.",
                ApplyOps    = [RegOp.SetDword(CryptoKey, "ForceKeyProtection", 2)],
                RemoveOps   = [RegOp.DeleteValue(CryptoKey, "ForceKeyProtection")],
                DetectOps   = [RegOp.CheckDword(CryptoKey, "ForceKeyProtection", 2)],
            },
            new TweakDef
            {
                Id          = "adcspol-smartcard-require-logon",
                Label       = "Require Smart Card for Interactive Logon",
                Category    = "ADCS Policy",
                Description = "Sets RequireSignOrSeal=1 in the SmartCardCredentialProvider policy. Enforces the Require Smart Card for Interactive Logon policy, preventing users from logging on with a password and requiring a physical smart card or Windows Hello for Business credential instead. This is the strongest form of phishing-resistant multi-factor authentication available natively in Windows.",
                Tags        = ["adcs", "smart-card", "mfa", "logon", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote  = "Forces smart card logon; users without cards or Windows Hello devices will be locked out.",
                ApplyOps    = [RegOp.SetDword(SmartKey, "RequireSignOrSeal", 1)],
                RemoveOps   = [RegOp.DeleteValue(SmartKey, "RequireSignOrSeal")],
                DetectOps   = [RegOp.CheckDword(SmartKey, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id          = "adcspol-smartcard-pin-blocking",
                Label       = "Enable Smart Card PIN Lock After 5 Failures",
                Category    = "ADCS Policy",
                Description = "Sets PINLockAfterFailedAttempts=5 in the SmartCardCredentialProvider policy. Automatically locks the smart card after 5 consecutive incorrect PIN entries, requiring an administrative PUK unlock. This prevents brute-force PIN guessing attacks where an attacker physically possesses the card and attempts to guess the PIN. Five attempts is the CIS Benchmark recommended maximum.",
                Tags        = ["adcs", "smart-card", "pin", "lockout", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote  = "Locks card after 5 wrong PINs; legitimate users should not exceed 5 attempts under normal usage.",
                ApplyOps    = [RegOp.SetDword(SmartKey, "PINLockAfterFailedAttempts", 5)],
                RemoveOps   = [RegOp.DeleteValue(SmartKey, "PINLockAfterFailedAttempts")],
                DetectOps   = [RegOp.CheckDword(SmartKey, "PINLockAfterFailedAttempts", 5)],
            },
            new TweakDef
            {
                Id          = "adcspol-disable-keyarchival",
                Label       = "Disable Certificate Key Archival to CA",
                Category    = "ADCS Policy",
                Description = "Sets DisableKeyArchival=1 in the CertSvc policy key. Prevents Windows from automatically archiving private keys to the Certificate Authority during certificate enrollment. Key archival allows CA administrators to recover encrypted data if a user loses their key, but it also means private keys leave the user's device and are stored on (and thus accessible to) the CA server. Disabling archival keeps private keys on the user's device only.",
                Tags        = ["adcs", "certificate", "key-archival", "policy", "privacy"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote  = "No CA-assisted recovery if user loses their private key; weigh against key escrow requirements.",
                ApplyOps    = [RegOp.SetDword(CertSvcKey, "DisableKeyArchival", 1)],
                RemoveOps   = [RegOp.DeleteValue(CertSvcKey, "DisableKeyArchival")],
                DetectOps   = [RegOp.CheckDword(CertSvcKey, "DisableKeyArchival", 1)],
            },
            new TweakDef
            {
                Id          = "adcspol-disable-cert-pub-ldap",
                Label       = "Disable Certificate Auto-Publication to LDAP",
                Category    = "ADCS Policy",
                Description = "Sets DisableLdapCertPublish=1 in the CertSvc policy. Prevents certificate enrollment from automatically publishing user or machine certificates to the LDAP directory (Active Directory). In high-security environments where only specific certificates should be visible in LDAP, disabling auto-publication prevents unnecessary certificate disclosure that could aid LDAP enumeration.",
                Tags        = ["adcs", "certificate", "ldap", "publish", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote  = "Stops LDAP cert publication; smart card logon and S/MIME directory lookup may require manual cert publishing.",
                ApplyOps    = [RegOp.SetDword(CertSvcKey, "DisableLdapCertPublish", 1)],
                RemoveOps   = [RegOp.DeleteValue(CertSvcKey, "DisableLdapCertPublish")],
                DetectOps   = [RegOp.CheckDword(CertSvcKey, "DisableLdapCertPublish", 1)],
            },
            new TweakDef
            {
                Id          = "adcspol-enable-cng-policy",
                Label       = "Enforce CNG Algorithm Policy (Block Legacy Crypto)",
                Category    = "ADCS Policy",
                Description = "Sets EnforceCNGAlgorithmPolicy=1 in the Cryptography policy. Instructs Windows CNG (Cryptography Next Generation) to apply the configured algorithm policy when evaluating certificate signatures and key operations. When combined with a restricted algorithm suite policy, this prevents applications from using deprecated or weak cryptographic algorithms (MD5, SHA-1 RSA-1024) in certificate operations.",
                Tags        = ["adcs", "cryptography", "cng", "algorithm", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Enforces algorithm policy; applications using legacy algorithms for cert operations may fail validation.",
                ApplyOps    = [RegOp.SetDword(CryptoKey, "EnforceCNGAlgorithmPolicy", 1)],
                RemoveOps   = [RegOp.DeleteValue(CryptoKey, "EnforceCNGAlgorithmPolicy")],
                DetectOps   = [RegOp.CheckDword(CryptoKey, "EnforceCNGAlgorithmPolicy", 1)],
            },
            new TweakDef
            {
                Id          = "adcspol-disable-cert-enrollment-ui",
                Label       = "Disable Certificate Enrollment UI for Non-Admins",
                Category    = "ADCS Policy",
                Description = "Sets DisableEnrollmentUI=1 in the CertSvc policy. Prevents the certificate enrollment wizard UI from being accessed by non-administrative users. Restricting certificate enrollment to administrators ensures that only properly authorised and audited enrollment workflows generate new certificates, preventing users from self-enrolling unapproved certificate types from the CA.",
                Tags        = ["adcs", "certificate", "enrollment", "ui", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote  = "Blocks self-service certificate enrollment UI; admin-managed enrolled certs are unaffected.",
                ApplyOps    = [RegOp.SetDword(CertSvcKey, "DisableEnrollmentUI", 1)],
                RemoveOps   = [RegOp.DeleteValue(CertSvcKey, "DisableEnrollmentUI")],
                DetectOps   = [RegOp.CheckDword(CertSvcKey, "DisableEnrollmentUI", 1)],
            },
            new TweakDef
            {
                Id          = "adcspol-require-cert-chain-validation",
                Label       = "Require Full Certificate Chain Validation",
                Category    = "ADCS Policy",
                Description = "Sets RequireChainValidation=1 in the Cryptography policy. Forces Windows to perform complete certificate chain validation (root, intermediates, and end-entity) before accepting any certificate as trusted. Disabling partial chain validation prevents applications from accepting certificates whose intermediate CA is untrusted or expired, closing a common misconfiguration that allows self-signed or invalid certificates to be accepted in some code paths.",
                Tags        = ["adcs", "certificate", "chain", "validation", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote  = "Enforces full chain validation; certificates with missing or expired intermediates will be rejected.",
                ApplyOps    = [RegOp.SetDword(CryptoKey, "RequireChainValidation", 1)],
                RemoveOps   = [RegOp.DeleteValue(CryptoKey, "RequireChainValidation")],
                DetectOps   = [RegOp.CheckDword(CryptoKey, "RequireChainValidation", 1)],
            },
        ];
}
