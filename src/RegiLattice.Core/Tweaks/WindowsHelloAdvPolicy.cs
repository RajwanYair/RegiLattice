// RegiLattice.Core — Tweaks/WindowsHelloAdvPolicy.cs
// Sprint 344: Windows Hello Advanced Policy tweaks (10 tweaks)
// Category: "Windows Hello Advanced Policy" | Slug: helloadv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsHelloAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "helloadv-require-hello-for-domain-auth",
            Label = "Require Windows Hello as Primary Domain Authentication Method",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Windows Hello for Business uses asymmetric key cryptography to replace password-based domain authentication providing phishing-resistant authentication that cannot be replayed. Requiring Windows Hello as the primary authentication method eliminates NTLM and Kerberos password hash exposure for domain authentication. Windows Hello for Business credentials are bound to the device and protected by the TPM making credential theft through standard techniques ineffective. Password-based authentication hashes can be captured from network traffic or from LSASS memory while Hello credentials cannot be harvested and replayed. Organizations should deploy Windows Hello for Business as part of a phishing-resistant MFA strategy aligned with NIST 800-63-3 AAL3 requirements. Hello for Business requires compatible hardware with TPM 2.0 and Windows 10 1703 or later for cloud or hybrid deployments.",
            Tags = ["windows-hello", "phishing-resistant", "domain-auth", "passwordless", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
            DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "helloadv-require-tpm-for-hello",
            Label = "Require TPM Chip for Windows Hello Key Storage",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Windows Hello for Business keys stored in the TPM are protected by hardware-bound key storage that cannot be exported even with administrative access to the operating system. Requiring TPM for Hello key storage ensures that credentials cannot be extracted from the system and used on another device. Software-based Hello keys stored only in the OS credential store can potentially be extracted through privilege escalation attacks making TPM storage the required configuration for high-security deployments. TPM-bound credentials require both the specific device and the user's PIN or biometric to authenticate combining something you have and something you know or are. Organizations should require TPM 2.0 for all new device purchases to support TPM-protected Hello credentials and other security features like Measured Boot and Device Health Attestation. The RequireSecurityDevice policy ensures that software fallback for Hello keys is not permitted when a TPM is unavailable.",
            Tags = ["windows-hello", "tpm", "hardware-security", "key-protection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireSecurityDevice", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSecurityDevice")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSecurityDevice", 1)],
        },
        new TweakDef
        {
            Id = "helloadv-set-minimum-pin-length",
            Label = "Enforce Minimum PIN Length for Windows Hello Authentication",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows Hello PIN minimum length requirements prevent trivially guessable short PINs that could be brute-forced through shoulder surfing or trial and error. Enforcing a minimum PIN length of 6 or more digits significantly increases the difficulty of guessing or brute-forcing a Hello PIN. Hello PINs are protected by account lockout after a configurable number of failed attempts limiting brute-force attack effectiveness. Unlike passwords Hello PINs are device-specific meaning a leaked PIN cannot be used to authenticate from any device except the specific registered device. Organizations should set minimum PIN lengths to at least 6 characters and consider using enhanced PIN complexity requirements that include alphabetic and special characters for higher security requirements. PIN length requirements should be communicated clearly to users during Windows Hello enrollment to ensure users create appropriate PINs.",
            Tags = ["windows-hello", "pin", "complexity", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MinimumPINLength", 6)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinimumPINLength")],
            DetectOps = [RegOp.CheckDword(Key, "MinimumPINLength", 6)],
        },
        new TweakDef
        {
            Id = "helloadv-enable-biometric-authentication",
            Label = "Enable Biometric Authentication for Windows Hello Sign-in",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Windows Hello biometric authentication allows users to sign in using facial recognition or fingerprint which provides both convenience and strong multi-factor authentication. Enabling biometric authentication for Hello gives users a fast and secure alternative to PIN-based authentication that is resistant to shoulder surfing and observation attacks. Windows Hello enhanced anti-spoofing requires compatible depth-sensing cameras that can distinguish a real face from a photograph. Biometric data collected by Windows Hello is stored only on the device and is protected by the TPM never leaving the device or being sent to Microsoft. Organizations should enable biometric authentication to improve user adoption of Windows Hello as users prefer biometric over PIN authentication when available. Fallback to PIN should be available for situations where biometric authentication fails to ensure users maintain secure access to their systems.",
            Tags = ["windows-hello", "biometric", "fingerprint", "face-recognition", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UseBiometrics", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UseBiometrics")],
            DetectOps = [RegOp.CheckDword(Key, "UseBiometrics", 1)],
        },
        new TweakDef
        {
            Id = "helloadv-disable-hello-provisioning-on-shared-pcs",
            Label = "Disable Windows Hello Provisioning on Shared or Kiosk Devices",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows Hello provisioning on shared devices creates security concerns because multiple users sharing the same device may inadvertently expose each other's biometric data or PIN-protected credentials. Disabling Hello provisioning on shared PCs ensures that the authentication method is appropriate for the shared use context. Kiosk devices with a single fixed user identity should not provision individual Hello credentials as the device is not used for personalized authentication. Shared workstation scenarios in call centers or shared office spaces require careful evaluation of whether Hello provisioning is appropriate and beneficial. Devices configured in shared PC mode through the Shared PC CSP automatically disable Hello provisioning as part of the shared PC configuration. Organizations should identify all shared devices and apply appropriate Hello configuration rather than applying organization-wide provisioning settings.",
            Tags = ["windows-hello", "shared-pc", "kiosk", "provisioning", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePostLogonProvisioning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePostLogonProvisioning")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePostLogonProvisioning", 1)],
        },
        new TweakDef
        {
            Id = "helloadv-enable-certificate-trust",
            Label = "Enable Certificate Trust Model for Windows Hello for Business",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Windows Hello for Business certificate trust model issues certificate-based credentials from an enterprise PKI enabling authentication to systems and services that require certificate-based authentication. Certificate trust Hello for Business provides compatibility with existing PKI infrastructure and supports scenarios that require X.509 certificates for authentication. Key trust Hello for Business is simpler to deploy but does not support all legacy authentication scenarios that certificate trust supports. Organizations with complex PKI infrastructure and certificate-based authentication requirements should deploy certificate trust Hello for Business. Certificate trust requires AD FS for on-premises deployments or Azure AD for cloud deployments to issue certificates during Hello provisioning. The choice between key trust and certificate trust Hello should align with the organization's authentication requirements and PKI capabilities.",
            Tags = ["windows-hello", "certificate-trust", "pki", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UseCertificateForOnPremAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UseCertificateForOnPremAuth")],
            DetectOps = [RegOp.CheckDword(Key, "UseCertificateForOnPremAuth", 1)],
        },
        new TweakDef
        {
            Id = "helloadv-set-pin-expiry",
            Label = "Set Maximum PIN Expiry Period for Windows Hello Authentication",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "PIN expiry for Windows Hello forces users to regularly change their device PINs ensuring that captured or observed PINs have limited useful lifetime. Setting PIN expiry to 60 days aligns with common password policy rotation schedules while acknowledging that PIN security differs from password security. Hello PINs are device-specific making them inherently more secure than domain passwords since a captured PIN cannot be used remotely from other devices. FIDO2 security frameworks generally do not recommend PIN expiry for Hello as the device binding provides sufficient security but organizations with compliance requirements may need to enforce rotation. PIN expiry requirements should be balanced against user friction to ensure users do not choose shorter or simpler PINs to ease memorization of frequently changing credentials. Organizations should evaluate whether the security benefit of PIN rotation outweighs the usability cost for their specific risk profile.",
            Tags = ["windows-hello", "pin-expiry", "rotation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ExpirationDays", 60)],
            RemoveOps = [RegOp.DeleteValue(Key, "ExpirationDays")],
            DetectOps = [RegOp.CheckDword(Key, "ExpirationDays", 60)],
        },
        new TweakDef
        {
            Id = "helloadv-block-simple-pins",
            Label = "Block Simple or Sequential PINs for Windows Hello Authentication",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Simple PINs like 1234, 0000, or sequential digit patterns are the Hello equivalent of weak passwords that are trivially guessable by observers or through systematic guessing. Blocking simple PINs enforces PIN complexity requirements that prevent sequentially or repeatedly patterned digit strings. Windows Hello PIN complexity can require the prevention of consecutive and repeating digits to ensure PINs have sufficient entropy for security. Organizations should configure PIN complexity requirements that prevent the top 100 most common PINs used according to security research on password patterns. The combination of PIN length requirements and simple PIN blocking creates a meaningful security baseline for Hello authentication. Users should be educated about choosing unpredictable PINs and informed that PINs should not use dates or memorable number patterns that could be guessed by social engineering.",
            Tags = ["windows-hello", "pin-complexity", "simple-pins", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Digits", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "Digits")],
            DetectOps = [RegOp.CheckDword(Key, "Digits", 2)],
        },
        new TweakDef
        {
            Id = "helloadv-enable-remote-unlock",
            Label = "Enable Remote Unlock Capability for Windows Hello Registered Devices",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Windows Hello remote unlock uses proximity-based authentication from a companion device allowing a locked computer to be unlocked when the user's phone is nearby. Enabling remote unlock provides a more convenient unlock experience for users while maintaining the security properties of Windows Hello authentication. Remote unlock requires the Windows Hello companion app installed on a paired mobile device and uses Bluetooth for proximity detection. Organizations should evaluate the security implications of remote unlock in their environment considering whether phone-based proximity is appropriate for their physical security controls. Remote unlock can be beneficial for users who work across multiple devices and need to frequently lock and unlock their workstations. The security of remote unlock depends on the security of the paired mobile device so mobile device management policies must be strong enough to protect this authentication factor.",
            Tags = ["windows-hello", "remote-unlock", "companion-device", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableUnlockFromPhone", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableUnlockFromPhone")],
            DetectOps = [RegOp.CheckDword(Key, "EnableUnlockFromPhone", 1)],
        },
        new TweakDef
        {
            Id = "helloadv-require-enhanced-anti-spoofing",
            Label = "Require Enhanced Anti-Spoofing for Windows Hello Facial Recognition",
            Category = "Windows Hello Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enhanced anti-spoofing requires IR camera hardware with depth sensing capabilities that can distinguish a real face from a photograph video or 3D mask presentation attack. Requiring enhanced anti-spoofing for Windows Hello facial recognition ensures that biometric authentication cannot be bypassed using a photograph of the user. Basic facial recognition without anti-spoofing can be defeated by holding a photograph in front of the camera making enhanced anti-spoofing critical for high-security environments. Organizations deploying Windows Hello with facial recognition should verify that enrolled devices have IR cameras that meet the enhanced anti-spoofing specifications before enabling the feature. Enhanced anti-spoofing is the default on devices certified for Windows Hello facial recognition but should be explicitly required for devices that may support facial recognition without certified hardware. Anti-spoofing requirements protect against the most common attack vectors for biometric authentication including photographs and video attacks.",
            Tags = ["windows-hello", "anti-spoofing", "biometric", "facial-recognition", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnhancedAntiSpoofingForFacialFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnhancedAntiSpoofingForFacialFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "EnhancedAntiSpoofingForFacialFeatures", 1)],
        },
    ];
}
