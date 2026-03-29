// RegiLattice.Core — Tweaks/PasswordlessSignInPolicy.cs
// Passwordless sign-in, FIDO2 security key policy, and Microsoft Authenticator controls — Sprint 478.
// Category: "Passwordless Sign-In Policy" | Slug: pwdless
// Registry: HKLM\SOFTWARE\Policies\Microsoft\PassportForWork

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PasswordlessSignInPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "pwdless-enforce-whfb",
                Label = "Enforce Windows Hello for Business Enrollment",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Requires all users to enroll in Windows Hello for Business during first sign-in, enforcing passwordless primary authentication and deprecating the use of traditional passwords for Windows sign-in.",
                Tags = ["whfb", "passwordless", "windows-hello", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WHfB enrollment required; users must set up biometric or PIN to complete first sign-in after enrollment period.",
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-require-tpm",
                Label = "Require TPM for Windows Hello for Business",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Mandates that WHfB private keys are protected by the device TPM, preventing WHfB credentials from being stored in software (file-based storage) where they could be exported.",
                Tags = ["whfb", "tpm", "passwordless", "windows-hello", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WHfB requires TPM; devices without TPM 2.0 cannot enroll in WHfB.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSecurityDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSecurityDevice")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSecurityDevice", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-disable-password-fallback",
                Label = "Disable Password Fallback for WHfB Sign-In",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Prevents users from falling back to password authentication when WHfB is available, forcing passwordless primary sign-in and eliminating the password as a secondary path attackers could target.",
                Tags = ["whfb", "passwordless", "password-fallback", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Password fallback blocked for WHfB; users must use biometric or PIN even if they remember their password.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordFallback")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordFallback", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-enable-fido2-keys",
                Label = "Enable FIDO2 Security Key Sign-In",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Enables FIDO2 hardware security keys (YubiKey, Titan key, etc.) as a credential type for Windows sign-in alongside WHfB, allowing phishing-resistant passwordless authentication from the lock screen.",
                Tags = ["fido2", "security-key", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "FIDO2 security keys accepted at Windows lock screen; users can log in with a hardware key.",
                ApplyOps = [RegOp.SetDword(Key, "EnableFIDO2SecurityKeys", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableFIDO2SecurityKeys")],
                DetectOps = [RegOp.CheckDword(Key, "EnableFIDO2SecurityKeys", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-disable-convenience-pin",
                Label = "Disable Convenience PIN (Non-WHfB) for Local Accounts",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Disables the simple convenience PIN for local accounts that does not benefit from WHfB's asymmetric key protection, forcing WHfB PIN (TPM-backed) for all PIN sign-in scenarios.",
                Tags = ["whfb", "convenience-pin", "local-account", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Convenience PIN disabled; PIN sign-in only available through WHfB with TPM backing.",
                ApplyOps = [RegOp.SetDword(Key, "AllowUnprovisionedPins", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUnprovisionedPins")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUnprovisionedPins", 0)],
            },
            new TweakDef
            {
                Id = "pwdless-block-phone-sign-in",
                Label = "Block Phone/Companion Device Sign-In",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Disables the companion device (phone-based Windows Hello sign-in) framework, preventing authentication via Bluetooth phone approval where physical possession of the phone is the only factor.",
                Tags = ["whfb", "phone-sign-in", "companion-device", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Phone/companion-device sign-in disabled; must use device-local WHfB or security key.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneSignIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneSignIn")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneSignIn", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-require-mfa-for-whfb-provision",
                Label = "Require MFA During WHfB Provisioning",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Requires multi-factor authentication during the WHfB provisioning ceremony, ensuring that only users who have already authenticated with a second factor can register WHfB credentials.",
                Tags = ["whfb", "mfa", "provisioning", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "MFA required during WHfB setup; single-factor users cannot provision WHfB credentials.",
                ApplyOps = [RegOp.SetDword(Key, "RequireMFAForProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMFAForProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMFAForProvisioning", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-block-whfb-on-unmanaged",
                Label = "Block WHfB Enrollment on Unmanaged Devices",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Prevents Windows Hello for Business enrollment on devices not enrolled in an MDM policy (Intune/SCCM), ensuring WHfB credentials are only provisioned on corp-managed endpoints.",
                Tags = ["whfb", "mdm", "managed-device", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WHfB blocked on unmanaged devices; enrollment only succeeds after MDM enrolment.",
                ApplyOps = [RegOp.SetDword(Key, "BlockEnrollmentOnUnmanagedDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockEnrollmentOnUnmanagedDevice")],
                DetectOps = [RegOp.CheckDword(Key, "BlockEnrollmentOnUnmanagedDevice", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-disable-whfb-personal",
                Label = "Disable WHfB for Personal Microsoft Accounts",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Prevents Windows Hello from being used for personal Microsoft account sign-in, restricting WHfB to work or school accounts only and preventing personal-account credential leakage.",
                Tags = ["whfb", "personal-account", "microsoft-account", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WHfB disabled for personal MSA; Windows Hello sign-in restricted to Azure AD accounts.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePersonalAccountPassport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalAccountPassport")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePersonalAccountPassport", 1)],
            },
            new TweakDef
            {
                Id = "pwdless-audit-whfb-provisioning",
                Label = "Enable Audit Logging for WHfB Provisioning Events",
                Category = "Passwordless Sign-In Policy",
                Description =
                    "Enables security audit events for all WHfB provisioning, re-provisioning, and credential deletion events, providing traceability for passwordless credential lifecycle management.",
                Tags = ["whfb", "audit-log", "provisioning", "passwordless", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WHfB credential lifecycle events logged in Security event log.",
                ApplyOps = [RegOp.SetDword(Key, "AuditProvisioningEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditProvisioningEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditProvisioningEvents", 1)],
            },
        ];
}
