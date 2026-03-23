// RegiLattice.Core — Tweaks/Biometrics.cs
// Windows Hello for Business and biometric hardware policies (Sprint 136).
// Slug "bio" — HKLM PassportForWork + Biometrics group-policy paths.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Biometrics
{
    private const string Bio = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics";
    private const string BioCP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics\Credential Provider";
    private const string BioFace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics\FacialFeatures";
    private const string Whfb = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
    private const string WhfbPin = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "bio-disable-biometrics",
            Label = "Disable Windows Biometrics Service",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Disables the Windows Biometric Service (WbioSrvc) via group policy. "
                + "Prevents fingerprint, face, and iris sensors from being used for "
                + "authentication. Enabled=0 in the Biometrics policy key.",
            Tags = ["biometrics", "windows hello", "security", "sign-in"],
            RegistryKeys = [Bio],
            ApplyOps = [RegOp.SetDword(Bio, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Bio, "Enabled")],
            DetectOps = [RegOp.CheckDword(Bio, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "bio-disable-biometrics-domain",
            Label = "Disable Biometrics for Domain / AAD Users",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Blocks domain-joined and Azure AD joined accounts from using biometric "
                + "sign-in. Does not affect local accounts. EnabledOnDomain=0.",
            Tags = ["biometrics", "domain", "aad", "security"],
            RegistryKeys = [Bio],
            ApplyOps = [RegOp.SetDword(Bio, "EnabledOnDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Bio, "EnabledOnDomain")],
            DetectOps = [RegOp.CheckDword(Bio, "EnabledOnDomain", 0)],
        },
        new TweakDef
        {
            Id = "bio-disable-biometric-sign-in",
            Label = "Disable Biometric Sign-In via Credential Provider",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Prevents biometric factors (fingerprint, face) from being offered on the "
                + "Windows sign-in screen. Enabled=0 in the Credential Provider subkey.",
            Tags = ["biometrics", "sign-in", "credential provider", "security"],
            RegistryKeys = [BioCP],
            ApplyOps = [RegOp.SetDword(BioCP, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(BioCP, "Enabled")],
            DetectOps = [RegOp.CheckDword(BioCP, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "bio-enable-facial-anti-spoofing",
            Label = "Enable Windows Hello Facial Anti-Spoofing (ESS)",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Enables Enhanced Anti-Spoofing for Windows Hello facial recognition. "
                + "Requires certified ISO 30107-3 PAD-compliant hardware. "
                + "EnhancedAntiSpoofing=1. Recommended on devices that support it.",
            Tags = ["biometrics", "face recognition", "anti-spoofing", "security", "ess"],
            RegistryKeys = [BioFace],
            ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 1)],
            RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
            DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 1)],
        },
        new TweakDef
        {
            Id = "bio-whfb-require-tpm",
            Label = "Require TPM for Windows Hello for Business Keys",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Forces Windows Hello for Business to store credential keys in the hardware "
                + "TPM chip, blocking software-based key storage. RequireSecurityDevice=1. "
                + "Devices without a TPM 2.0 chip will not be able to enrol.",
            Tags = ["windows hello", "tpm", "security", "whfb", "hardware"],
            RegistryKeys = [Whfb],
            ApplyOps = [RegOp.SetDword(Whfb, "RequireSecurityDevice", 1)],
            RemoveOps = [RegOp.DeleteValue(Whfb, "RequireSecurityDevice")],
            DetectOps = [RegOp.CheckDword(Whfb, "RequireSecurityDevice", 1)],
        },
        new TweakDef
        {
            Id = "bio-whfb-pin-min-length",
            Label = "Set Minimum Windows Hello PIN Length to 8",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Enforces a minimum of 8 characters for the Windows Hello PIN via the "
                + "PINComplexity group policy. Increases brute-force resistance. "
                + "MinimumPINLength=8.",
            Tags = ["windows hello", "pin", "complexity", "security", "whfb"],
            RegistryKeys = [WhfbPin],
            ApplyOps = [RegOp.SetDword(WhfbPin, "MinimumPINLength", 8)],
            RemoveOps = [RegOp.DeleteValue(WhfbPin, "MinimumPINLength")],
            DetectOps = [RegOp.CheckDword(WhfbPin, "MinimumPINLength", 8)],
        },
        new TweakDef
        {
            Id = "bio-whfb-pin-require-digits",
            Label = "Require Digits in Windows Hello PIN",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Requires at least one numeric digit in the Windows Hello PIN. "
                + "Prevents all-letter PINs. Digits=1 in PINComplexity.",
            Tags = ["windows hello", "pin", "complexity", "digits", "security"],
            RegistryKeys = [WhfbPin],
            ApplyOps = [RegOp.SetDword(WhfbPin, "Digits", 1)],
            RemoveOps = [RegOp.DeleteValue(WhfbPin, "Digits")],
            DetectOps = [RegOp.CheckDword(WhfbPin, "Digits", 1)],
        },
        new TweakDef
        {
            Id = "bio-whfb-pin-require-uppercase",
            Label = "Require Uppercase Letters in Windows Hello PIN",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Requires at least one uppercase letter in the Windows Hello PIN. "
                + "UppercaseLetters=1 in PINComplexity.",
            Tags = ["windows hello", "pin", "complexity", "uppercase", "security"],
            RegistryKeys = [WhfbPin],
            ApplyOps = [RegOp.SetDword(WhfbPin, "UppercaseLetters", 1)],
            RemoveOps = [RegOp.DeleteValue(WhfbPin, "UppercaseLetters")],
            DetectOps = [RegOp.CheckDword(WhfbPin, "UppercaseLetters", 1)],
        },
        new TweakDef
        {
            Id = "bio-whfb-pin-require-lowercase",
            Label = "Require Lowercase Letters in Windows Hello PIN",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Requires at least one lowercase letter in the Windows Hello PIN. "
                + "LowercaseLetters=1 in PINComplexity.",
            Tags = ["windows hello", "pin", "complexity", "lowercase", "security"],
            RegistryKeys = [WhfbPin],
            ApplyOps = [RegOp.SetDword(WhfbPin, "LowercaseLetters", 1)],
            RemoveOps = [RegOp.DeleteValue(WhfbPin, "LowercaseLetters")],
            DetectOps = [RegOp.CheckDword(WhfbPin, "LowercaseLetters", 1)],
        },
        new TweakDef
        {
            Id = "bio-whfb-pin-expiry",
            Label = "Set Windows Hello PIN Expiration to 90 Days",
            Category = "Biometrics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Forces Windows Hello PINs to expire after 90 days, requiring users to "
                + "create a new PIN on next sign-in. Expiration=90 in PINComplexity.",
            Tags = ["windows hello", "pin", "expiration", "rotation", "security"],
            RegistryKeys = [WhfbPin],
            ApplyOps = [RegOp.SetDword(WhfbPin, "Expiration", 90)],
            RemoveOps = [RegOp.DeleteValue(WhfbPin, "Expiration")],
            DetectOps = [RegOp.CheckDword(WhfbPin, "Expiration", 90)],
        },
    ];
}
