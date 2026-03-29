// RegiLattice.Core — Tweaks/BiometricAuthPolicy.cs
// Windows Hello biometric (face/fingerprint) authentication policy and anti-spoofing controls — Sprint 479.
// Category: "Biometric Auth Policy" | Slug: biometric
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Biometrics

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BiometricAuthPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "biometric-disable-biometrics-service",
                Label = "Disable Windows Biometrics Service",
                Category = "Biometric Auth Policy",
                Description =
                    "Disables the Windows Biometric Service (WBS), preventing any biometric authentication including Windows Hello fingerprint and face recognition. Use on systems where biometrics are not permitted.",
                Tags = ["biometrics", "windows-hello", "fingerprint", "face", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Windows Biometric Service disabled; no fingerprint or face sign-in available. PIN or password required.",
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "biometric-disable-face-recognition",
                Label = "Disable Windows Hello Face Recognition",
                Category = "Biometric Auth Policy",
                Description =
                    "Disables Windows Hello Face Recognition, preventing camera-based biometric sign-in without disabling other biometric factors like fingerprint.",
                Tags = ["biometrics", "windows-hello", "face", "camera", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Face recognition disabled; fingerprint and PIN sign-in still available.",
                ApplyOps = [RegOp.SetDword(Key, "DisableFaceRecognition", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFaceRecognition")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFaceRecognition", 1)],
            },
            new TweakDef
            {
                Id = "biometric-disable-fingerprint",
                Label = "Disable Windows Hello Fingerprint Sign-In",
                Category = "Biometric Auth Policy",
                Description =
                    "Disables Windows Hello fingerprint sign-in without disabling other biometric or WHfB credential types, useful in environments where fingerprint readers present a shared contamination risk.",
                Tags = ["biometrics", "windows-hello", "fingerprint", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Fingerprint sign-in disabled; face recognition and PIN still available.",
                ApplyOps = [RegOp.SetDword(Key, "DisableFingerprintSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFingerprintSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFingerprintSignIn", 1)],
            },
            new TweakDef
            {
                Id = "biometric-require-anti-spoofing",
                Label = "Require Anti-Spoofing for Face Recognition",
                Category = "Biometric Auth Policy",
                Description =
                    "Requires that Windows Hello Face Recognition use enhanced anti-spoofing (infrared liveness detection), rejecting 2D photo attacks and blocking face sign-in from cameras without IR.",
                Tags = ["biometrics", "windows-hello", "anti-spoofing", "face", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Anti-spoofing required; face sign-in only on cameras with IR liveness, defeating photo/mask attacks.",
                ApplyOps = [RegOp.SetDword(Key, "FacialFeaturesUseEnhancedAntiSpoofing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "FacialFeaturesUseEnhancedAntiSpoofing")],
                DetectOps = [RegOp.CheckDword(Key, "FacialFeaturesUseEnhancedAntiSpoofing", 1)],
            },
            new TweakDef
            {
                Id = "biometric-block-domain-users-biometrics",
                Label = "Block Biometric Sign-In for Domain Accounts",
                Category = "Biometric Auth Policy",
                Description =
                    "Disables biometric sign-in for domain (Active Directory) accounts, requiring domain users to authenticate with WHfB PIN or password instead of biometrics for high-security domains.",
                Tags = ["biometrics", "domain-account", "windows-hello", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Domain account biometric sign-in blocked; domain users must use PIN or password.",
                ApplyOps = [RegOp.SetDword(Key, "DomainAccountsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DomainAccountsEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "DomainAccountsEnabled", 0)],
            },
            new TweakDef
            {
                Id = "biometric-disable-biometrics-for-uac",
                Label = "Disable Biometric Authentication for UAC Prompts",
                Category = "Biometric Auth Policy",
                Description =
                    "Prevents biometrics (fingerprint or face) from being used to approve User Account Control elevation prompts, requiring a PIN or password for admin approval dialogs.",
                Tags = ["biometrics", "uac", "elevation", "windows-hello", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Biometrics blocked for UAC; admin elevations require PIN or password approval.",
                ApplyOps = [RegOp.SetDword(Key, "DisableBiometricForUAC", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBiometricForUAC")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBiometricForUAC", 1)],
            },
            new TweakDef
            {
                Id = "biometric-require-admin-enroll",
                Label = "Require Admin Approval to Enroll Biometrics",
                Category = "Biometric Auth Policy",
                Description =
                    "Requires administrator approval to enroll new biometric credentials on managed devices, preventing unauthorized enrollment of biometrics on shared or managed workstations.",
                Tags = ["biometrics", "enrollment", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Biometric enrollment requires admin approval; self-service biometric setup restricted.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminForBiometricEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForBiometricEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminForBiometricEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "biometric-log-biometric-auth-events",
                Label = "Enable Audit Logging for All Biometric Authentication Events",
                Category = "Biometric Auth Policy",
                Description =
                    "Enables Windows event log entries for every biometric authentication attempt (success, failure, lockout), providing an audit trail for biometric sign-in usage.",
                Tags = ["biometrics", "audit-log", "windows-hello", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Biometric auth events logged; both success and failure attempts recorded in Security event log.",
                ApplyOps = [RegOp.SetDword(Key, "AuditBiometricAuthEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditBiometricAuthEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditBiometricAuthEvents", 1)],
            },
            new TweakDef
            {
                Id = "biometric-disable-biometric-third-party",
                Label = "Block Third-Party Biometric Device Drivers",
                Category = "Biometric Auth Policy",
                Description =
                    "Prevents third-party biometric device drivers from registering with the Windows Biometric Framework, restricting biometric authentication to Microsoft-signed and WHQL-certified biometric hardware.",
                Tags = ["biometrics", "third-party", "driver", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Third-party biometric drivers blocked; only WHQL-certified biometric hardware recognised.",
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyBiometricDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyBiometricDrivers")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyBiometricDrivers", 1)],
            },
            new TweakDef
            {
                Id = "biometric-clear-biometrics-on-lock",
                Label = "Clear Biometric Authentication Cache on Screen Lock",
                Category = "Biometric Auth Policy",
                Description =
                    "Clears cached biometric authentication tokens when the screen locks, requiring a fresh biometric scan after every lock event rather than allowing replay of a recently cached biometric match.",
                Tags = ["biometrics", "cache", "screen-lock", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Biometric cache cleared on lock; full biometric scan required after every screen lock.",
                ApplyOps = [RegOp.SetDword(Key, "ClearBiometricCacheOnLock", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClearBiometricCacheOnLock")],
                DetectOps = [RegOp.CheckDword(Key, "ClearBiometricCacheOnLock", 1)],
            },
        ];
}
