// RegiLattice.Core — Tweaks/SmartCardCredProvPolicy.cs
// Smart Card Credential Provider machine-scope GPO controls — Sprint 197.
// Enforces smart-card certificate and PIN usage rules for PKI-based logon hardening.
// Category: "Smart Card Credential Provider Policy" | Slug: scprov
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmartCardCredProvPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "scprov-block-no-eku-certs",
                Label = "Block Smart Card Certs Without EKU",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Blocks smart card certificates that lack Extended Key Usage (EKU) extensions from being accepted for logon. Prevents improperly issued certificates from authenticating. Default: 1 (allow). Recommended: 0 (block).",
                Tags = ["smart-card", "pki", "eku", "certificate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents authentication with malformed or incorrectly issued smart card certificates lacking EKU.",
                ApplyOps = [RegOp.SetDword(Key, "AllowCertificatesWithNoEKU", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCertificatesWithNoEKU")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCertificatesWithNoEKU", 0)],
            },
            new TweakDef
            {
                Id = "scprov-block-signature-only-keys",
                Label = "Block Signature-Only Smart Card Keys",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Prevents smart cards with signature-only keys from being used for interactive logon. Signature keys should not be used for authentication. Default: 1 (allow). Recommended: 0 (block).",
                Tags = ["smart-card", "pki", "signature", "key-usage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enforces key usage separation; signature keys cannot be used for authentication.",
                ApplyOps = [RegOp.SetDword(Key, "AllowSignatureOnlyKeys", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSignatureOnlyKeys")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSignatureOnlyKeys", 0)],
            },
            new TweakDef
            {
                Id = "scprov-block-time-invalid-certs",
                Label = "Block Expired Smart Card Certificates",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Prevents authentication using time-invalid (expired or not yet valid) smart card certificates. Enforces certificate lifecycle compliance. Default: 1 (allow). Recommended: 0 (block).",
                Tags = ["smart-card", "pki", "expiry", "certificate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Ensures all authenticating certificates are within their validity period.",
                ApplyOps = [RegOp.SetDword(Key, "AllowTimeInvalidCertificates", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTimeInvalidCertificates")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTimeInvalidCertificates", 0)],
            },
            new TweakDef
            {
                Id = "scprov-enumerate-ecc-certs",
                Label = "Enumerate ECC Certificates by Default",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Enables enumeration of elliptic-curve cryptography certificates on smart cards by default. Required when the organisation uses ECDSA/ECDH smart card certificates. Default: 0. Recommended: 1 when ECC certs are deployed.",
                Tags = ["smart-card", "ecc", "pki", "enumeration", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables ECC-issued certificates on smart cards to appear in the logon picker.",
                ApplyOps = [RegOp.SetDword(Key, "EnumerateECCCerts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnumerateECCCerts")],
                DetectOps = [RegOp.CheckDword(Key, "EnumerateECCCerts", 1)],
            },
            new TweakDef
            {
                Id = "scprov-filter-dup-certs",
                Label = "Filter Duplicate Logon Certificates",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "De-duplicates certificates shown in the smart card logon picker when a card carries multiple identical certificates. Prevents UI confusion during logon. Default: 0. Recommended: 1.",
                Tags = ["smart-card", "duplicate", "certificate", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Cosmetic improvement; removes duplicate certificate entries from the logon picker.",
                ApplyOps = [RegOp.SetDword(Key, "FilterDuplicateCerts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "FilterDuplicateCerts")],
                DetectOps = [RegOp.CheckDword(Key, "FilterDuplicateCerts", 1)],
            },
            new TweakDef
            {
                Id = "scprov-force-read-all-certs",
                Label = "Force Reading All Smart Card Certificates",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Forces the system to read all certificates from a smart card rather than stopping at the first valid one. Ensures complete certificate inventory for logon selection. Default: 0. Recommended: 1 for multi-cert cards.",
                Tags = ["smart-card", "certificate", "enumeration", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Slightly increases smart card logon time; ensures all certs on the card are available.",
                ApplyOps = [RegOp.SetDword(Key, "ForceReadingAllCertificates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceReadingAllCertificates")],
                DetectOps = [RegOp.CheckDword(Key, "ForceReadingAllCertificates", 1)],
            },
            new TweakDef
            {
                Id = "scprov-no-reverse-subject",
                Label = "Normalise Certificate Subject Display Order",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Prevents the credential provider from reversing certificate subject field order in the logon UI. Ensures consistent CN/OU display regardless of CA issuance order. Default: not set. Recommended: 0 (normal order).",
                Tags = ["smart-card", "subject", "display", "certificate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Display normalisation only; no functional security impact on authentication.",
                ApplyOps = [RegOp.SetDword(Key, "ReverseSubject", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ReverseSubject")],
                DetectOps = [RegOp.CheckDword(Key, "ReverseSubject", 0)],
            },
            new TweakDef
            {
                Id = "scprov-suppress-x509-hints",
                Label = "Suppress X.509 Certificate Hint Display",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Suppresses X.509 certificate hint prompts shown when multiple certificates are available during smart card logon. Reduces UI noise in managed environments. Default: 1. Recommended: 0.",
                Tags = ["smart-card", "x509", "hint", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes extra X.509 hint dialogs during smart card authentication.",
                ApplyOps = [RegOp.SetDword(Key, "X509HintsNeeded", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "X509HintsNeeded")],
                DetectOps = [RegOp.CheckDword(Key, "X509HintsNeeded", 0)],
            },
            new TweakDef
            {
                Id = "scprov-disallow-plaintext-pin",
                Label = "Disallow Plaintext Smart Card PIN Transmission",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Prevents smart card PINs from being returned or transmitted in clear text by the Credential Manager. Critical for preventing PIN interception on hosts with memory inspection. Default: 0. Recommended: 1.",
                Tags = ["smart-card", "pin", "plaintext", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Prevents PIN interception; may break legacy applications that depend on plaintext PIN access.",
                ApplyOps = [RegOp.SetDword(Key, "DisallowPlaintextPin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowPlaintextPin")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowPlaintextPin", 1)],
            },
            new TweakDef
            {
                Id = "scprov-logon-hours-notify",
                Label = "Enable Logon Hours Change Notification",
                Category = "Smart Card Credential Provider Policy",
                Description =
                    "Notifies users when their allowed logon hours are about to expire or have changed, using smart card credential context. Helps users save work before forced logoff. Default: 0. Recommended: 1.",
                Tags = ["smart-card", "logon-hours", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Improves user experience in environments with logon-hour restrictions.",
                ApplyOps = [RegOp.SetDword(Key, "LogonHoursNotificationEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogonHoursNotificationEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "LogonHoursNotificationEnabled", 1)],
            },
        ];
}
