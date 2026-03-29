// RegiLattice.Core — Tweaks/CertificateBasedAuthPolicy.cs
// Certificate-based authentication, smart card policy, and PKI trust controls — Sprint 481.
// Category: "Certificate Based Auth Policy" | Slug: cbapol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CertificateBasedAuthPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cbapol-require-smartcard-login",
                Label = "Require Smart Card for Interactive Logon",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Requires a smart card (or virtual smart card with TPM) for all interactive Windows logon sessions, blocking password-based or biometric-only sign-in and enforcing certificate-based two-factor authentication.",
                Tags = ["smartcard", "cba", "certificate", "authentication", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Smart card logon required; password and biometric login blocked. Requires PKI infrastructure.",
                ApplyOps = [RegOp.SetDword(Key, "ForceSmartCardSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceSmartCardSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "ForceSmartCardSignIn", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-lock-on-smartcard-removal",
                Label = "Lock Workstation on Smart Card Removal",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Automatically locks the Windows workstation when the smart card is removed from the reader, enforcing physical card custody and preventing unattended session access.",
                Tags = ["smartcard", "cba", "lock", "card-removal", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Workstation locks on card removal; session inaccessible without reinserting the card.",
                ApplyOps = [RegOp.SetDword(Key, "LockWorkstationOnSmartCardRemoval", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockWorkstationOnSmartCardRemoval")],
                DetectOps = [RegOp.CheckDword(Key, "LockWorkstationOnSmartCardRemoval", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-require-ocsp-for-crl",
                Label = "Require OCSP Revocation Check for Smart Card Certificates",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Forces OCSP (Online Certificate Status Protocol) revocation checking for smart card certificates during logon, immediately blocking revoked certificates rather than waiting for CRL cache to expire.",
                Tags = ["smartcard", "cba", "ocsp", "revocation", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "OCSP checked on every smart card logon; revoked cards blocked immediately.",
                ApplyOps = [RegOp.SetDword(Key, "RequireOCSPRevocationCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireOCSPRevocationCheck")],
                DetectOps = [RegOp.CheckDword(Key, "RequireOCSPRevocationCheck", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-allow-virtual-smart-card",
                Label = "Allow TPM Virtual Smart Cards",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Enables TPM-backed virtual smart cards (VSCs) as an alternative to physical smart card readers, allowing certificate-based authentication without requiring physical card hardware.",
                Tags = ["smartcard", "virtual-smart-card", "tpm", "cba", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Virtual smart cards enabled; TPM-backed VSCs can be used in lieu of physical smart cards.",
                ApplyOps = [RegOp.SetDword(Key, "AllowVirtualSmartCard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowVirtualSmartCard")],
                DetectOps = [RegOp.CheckDword(Key, "AllowVirtualSmartCard", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-block-pin-caching",
                Label = "Block Smart Card PIN Caching",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Disables caching of smart card PINs in memory after successful authentication, requiring the user to re-enter their PIN for every cryptographic operation instead of using a cached PIN.",
                Tags = ["smartcard", "pin", "caching", "cba", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Smart card PIN caching disabled; PIN re-entry required for every cryptographic use.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePINcaching", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePINcaching")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePINcaching", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-block-root-cert-auto-update",
                Label = "Block Automatic Root Certificate Auto-Update",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Prevents Windows from automatically downloading and installing new root certificates from Windows Update, keeping the trusted root store static and under admin control.",
                Tags = ["cba", "root-certificate", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Root certificate auto-update blocked; trusted roots only updated via manual admin action.",
                ApplyOps = [RegOp.SetDword(Key, "BlockRootCertAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockRootCertAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "BlockRootCertAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-log-all-cert-validation",
                Label = "Enable Audit Logging for Certificate Chain Validation",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Enables event logging for all certificate chain validation operations including OCSP, CRL, and path-building events, providing a PKI audit trail for smart card and TLS authentication.",
                Tags = ["cba", "certificate", "audit-log", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Certificate validation operations logged; PKI health and failures auditable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditCertificateChainValidation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificateChainValidation")],
                DetectOps = [RegOp.CheckDword(Key, "AuditCertificateChainValidation", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-block-expired-cert-auth",
                Label = "Block Authentication with Expired Certificates",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Rejects smart card or certificate-based logon attempts when the authentication certificate has expired, preventing continuation of sessions using stale credentials.",
                Tags = ["cba", "certificate", "expiry", "smartcard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Expired certificates rejected at logon; users must renew PKI certs before sign-in.",
                ApplyOps = [RegOp.SetDword(Key, "BlockExpiredCertificateAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockExpiredCertificateAuth")],
                DetectOps = [RegOp.CheckDword(Key, "BlockExpiredCertificateAuth", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-enforce-strong-key-protection",
                Label = "Enforce Strong Private Key Protection for User Certs",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Requires strong private key protection (PIN or password confirmation) whenever a user certificate's private key is accessed, preventing silent key use by malicious processes.",
                Tags = ["cba", "private-key", "strong-protection", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Strong key protection enforced; PIN confirmation required for every private key operation.",
                ApplyOps = [RegOp.SetDword(Key, "RequireStrongPrivateKeyProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireStrongPrivateKeyProtection")],
                DetectOps = [RegOp.CheckDword(Key, "RequireStrongPrivateKeyProtection", 1)],
            },
            new TweakDef
            {
                Id = "cbapol-require-kdc-cert-valid",
                Label = "Require Valid KDC Certificate for Kerberos PKINIT",
                Category = "Certificate Based Auth Policy",
                Description =
                    "Requires that the Kerberos KDC certificate presented during PKINIT authentication is valid and trusted, blocking use of self-signed or untrusted KDC certificates in Kerberos certificate-based auth.",
                Tags = ["cba", "kerberos", "pkinit", "kdc", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "KDC certificate validity enforced for PKINIT; untrusted KDC certificates rejected.",
                ApplyOps = [RegOp.SetDword(Key, "RequireValidKDCCertificate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireValidKDCCertificate")],
                DetectOps = [RegOp.CheckDword(Key, "RequireValidKDCCertificate", 1)],
            },
        ];
}
