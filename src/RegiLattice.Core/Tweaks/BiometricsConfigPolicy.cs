// RegiLattice.Core — Tweaks/BiometricsConfigPolicy.cs
// Windows Biometrics Group Policy — Sprint 187.
// Controls Windows Hello biometric sign-in, domain biometric logon,
// anti-spoofing enforcement, and enrollment restrictions via Group Policy.
// Category: "Biometrics Policy" | Slug: biopol
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows\Biometrics (and subkeys)

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BiometricsConfigPolicy
{
    private const string BioKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics";
    private const string BioDomain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics\DomainAccounts";
    private const string BioFace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics\FacialFeatures";
    private const string BioEnroll = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Biometrics\Enrollment";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "biopol-disable-biometrics",
                Label = "Disable Windows Biometrics Service",
                Category = "Biometrics Policy",
                Description =
                    "Sets Enabled=0 in the Biometrics policy key to disable the Windows biometric framework. Prevents all biometric sign-in methods including fingerprint and facial recognition.",
                Tags = ["biometrics", "security", "policy", "sign-in"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables all Windows Hello biometric sign-in; users must use PIN or password.",
                ApplyOps = [RegOp.SetDword(BioKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(BioKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(BioKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "biopol-block-domain-biometric-logon",
                Label = "Block Biometric Domain Logon",
                Category = "Biometrics Policy",
                Description =
                    "Sets Enabled=0 under the DomainAccounts subkey to block domain-joined users from authenticating with biometrics. Useful in high-security enterprise environments.",
                Tags = ["biometrics", "domain", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Domain users must use passwords or smart cards; no fingerprint/face logon.",
                ApplyOps = [RegOp.SetDword(BioDomain, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(BioDomain, "Enabled")],
                DetectOps = [RegOp.CheckDword(BioDomain, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "biopol-disable-secondary-auth-factor",
                Label = "Disable Biometric Secondary Authentication Factor",
                Category = "Biometrics Policy",
                Description =
                    "Sets SecondaryAuthenticationFactor=0 to prevent biometrics from being used as a secondary authentication factor on top of primary credentials.",
                Tags = ["biometrics", "mfa", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Removes biometric option from secondary authentication; users rely on password/PIN only.",
                ApplyOps = [RegOp.SetDword(BioKey, "SecondaryAuthenticationFactor", 0)],
                RemoveOps = [RegOp.DeleteValue(BioKey, "SecondaryAuthenticationFactor")],
                DetectOps = [RegOp.CheckDword(BioKey, "SecondaryAuthenticationFactor", 0)],
            },
            new TweakDef
            {
                Id = "biopol-disable-domain-secondary-auth",
                Label = "Block Domain Biometric Secondary Authentication",
                Category = "Biometrics Policy",
                Description =
                    "Sets SecondaryAuthenticationFactor=0 under DomainAccounts to prevent domain users from using biometrics as a secondary authentication factor.",
                Tags = ["biometrics", "domain", "mfa", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Domain biometric MFA disabled; aligns with strict enterprise authentication policies.",
                ApplyOps = [RegOp.SetDword(BioDomain, "SecondaryAuthenticationFactor", 0)],
                RemoveOps = [RegOp.DeleteValue(BioDomain, "SecondaryAuthenticationFactor")],
                DetectOps = [RegOp.CheckDword(BioDomain, "SecondaryAuthenticationFactor", 0)],
            },
            new TweakDef
            {
                Id = "biopol-enforce-enhanced-anti-spoofing",
                Label = "Enforce Enhanced Facial Anti-Spoofing",
                Category = "Biometrics Policy",
                Description =
                    "Sets EnhancedAntiSpoofing=1 under FacialFeatures to require cameras with IR depth sensors for facial recognition, blocking photo or video spoofing attempts.",
                Tags = ["biometrics", "face", "anti-spoofing", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces IR-based facial recognition; prevents photo/video spoofing attacks.",
                ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 1)],
                RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
                DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 1)],
            },
            new TweakDef
            {
                Id = "biopol-disable-alternative-auth-factor",
                Label = "Disable Alternative Authentication Factor via Biometrics",
                Category = "Biometrics Policy",
                Description =
                    "Sets AlternativeAuthenticationFactor=0 to disable alternative biometric authentication factors such as vein pattern readers or external biometric devices.",
                Tags = ["biometrics", "security", "policy", "authentication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Blocks non-standard biometric auth factors; standard fingerprint/face are separately controlled.",
                ApplyOps = [RegOp.SetDword(BioKey, "AlternativeAuthenticationFactor", 0)],
                RemoveOps = [RegOp.DeleteValue(BioKey, "AlternativeAuthenticationFactor")],
                DetectOps = [RegOp.CheckDword(BioKey, "AlternativeAuthenticationFactor", 0)],
            },
            new TweakDef
            {
                Id = "biopol-block-biometric-enrollment",
                Label = "Block New Biometric Credential Enrollment",
                Category = "Biometrics Policy",
                Description =
                    "Sets BlockNewEnrollment=1 under the Enrollment subkey to prevent users from enrolling new biometric credentials (fingerprints or face). Existing credentials remain usable.",
                Tags = ["biometrics", "enrollment", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "New biometric registrations are blocked; previously enrolled credentials still work.",
                ApplyOps = [RegOp.SetDword(BioEnroll, "BlockNewEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(BioEnroll, "BlockNewEnrollment")],
                DetectOps = [RegOp.CheckDword(BioEnroll, "BlockNewEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "biopol-disable-biometric-logon-ui",
                Label = "Disable Biometric Sign-In UI",
                Category = "Biometrics Policy",
                Description =
                    "Sets AllowLogon=0 to remove the biometric sign-in option from the Windows sign-in screen, ensuring only PIN, password, or smart card options are presented.",
                Tags = ["biometrics", "logon", "ui", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Biometric option removed from sign-in screen; affects UI only, not underlying credentials.",
                ApplyOps = [RegOp.SetDword(BioKey, "AllowLogon", 0)],
                RemoveOps = [RegOp.DeleteValue(BioKey, "AllowLogon")],
                DetectOps = [RegOp.CheckDword(BioKey, "AllowLogon", 0)],
            },
            new TweakDef
            {
                Id = "biopol-disable-biometric-cred-provider",
                Label = "Disable Biometric Credential Provider",
                Category = "Biometrics Policy",
                Description =
                    "Sets DisableCredentialProviders=1 to disable the Windows biometric credential provider at the system level, removing biometric authentication from all credential prompts.",
                Tags = ["biometrics", "credential", "provider", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "System-wide removal of biometric credential provider; may affect third-party apps relying on Windows Hello.",
                ApplyOps = [RegOp.SetDword(BioKey, "DisableCredentialProviders", 1)],
                RemoveOps = [RegOp.DeleteValue(BioKey, "DisableCredentialProviders")],
                DetectOps = [RegOp.CheckDword(BioKey, "DisableCredentialProviders", 1)],
            },
            new TweakDef
            {
                Id = "biopol-disable-face-id-enrollment",
                Label = "Disable Windows Hello Face Recognition Enrollment",
                Category = "Biometrics Policy",
                Description =
                    "Sets Enabled=0 under the FacialFeatures key to disable facial recognition in Windows Hello entirely, while other biometric methods may remain available.",
                Tags = ["biometrics", "face", "windows-hello", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Face recognition sign-in disabled; fingerprint biometrics may still function if enabled.",
                ApplyOps = [RegOp.SetDword(BioFace, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(BioFace, "Enabled")],
                DetectOps = [RegOp.CheckDword(BioFace, "Enabled", 0)],
            },
        ];
}
