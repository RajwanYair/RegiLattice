// RegiLattice.Core — Tweaks/EntraDeviceRegistrationPolicy.cs
// Entra ID device registration, MDM auto-enrolment, and NGC key controls — Sprint 458.
// Uses distinct value names from AzureAdConditionalAccessPolicy at the WorkplaceJoin key.
// Category: "Entra Device Registration Policy" | Slug: entrareg
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EntraDeviceRegistrationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "entrareg-disable-auto-registration",
                Label = "Disable Automatic Device Registration with Entra ID",
                Category = "Entra Device Registration Policy",
                Description =
                    "Disables automatic Entra ID (Azure AD) device registration triggered by domain join, preventing unintended hybrid join of machines that should remain unregistered.",
                Tags = ["entra", "device-registration", "azure-ad", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Automatic device registration to Entra disabled; manual registration still possible.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRegistration", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-require-mdm-enrollment",
                Label = "Require MDM Enrollment for Device Registration",
                Category = "Entra Device Registration Policy",
                Description =
                    "Requires MDM (Microsoft Intune or third-party MDM) enrollment as a prerequisite for completing Entra ID device registration, ensuring all registered devices are also managed.",
                Tags = ["entra", "mdm", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Device registration requires MDM enrollment; unmanaged devices cannot fully register with Entra.",
                ApplyOps = [RegOp.SetDword(Key, "RequireMDMEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMDMEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMDMEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-disable-auto-mdm-enroll",
                Label = "Disable Automatic MDM Auto-Enrollment",
                Category = "Entra Device Registration Policy",
                Description =
                    "Disables automatic MDM auto-enrollment that triggers when a device joins Entra ID, giving IT control over which devices are enrolled in mobile device management.",
                Tags = ["entra", "mdm", "auto-enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "MDM auto-enrollment disabled; devices must be enrolled manually or via provisioning package.",
                ApplyOps = [RegOp.SetDword(Key, "AutoEnrollMDM", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoEnrollMDM")],
                DetectOps = [RegOp.CheckDword(Key, "AutoEnrollMDM", 0)],
            },
            new TweakDef
            {
                Id = "entrareg-block-ngc-key-reset",
                Label = "Block NGC Key Reset During Device Registration",
                Category = "Entra Device Registration Policy",
                Description =
                    "Blocks Next Generation Credentials (NGC) key reset operations during device registration events, preventing credential rotation that could lock users out after re-registration.",
                Tags = ["entra", "ngc", "key", "credentials", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "NGC keys not reset on re-registration; PIN/biometric credentials preserved through device re-join.",
                ApplyOps = [RegOp.SetDword(Key, "BlockNGCKeyReset", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNGCKeyReset")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNGCKeyReset", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-disable-user-consent-registration",
                Label = "Disable User-Initiated Device Registration",
                Category = "Entra Device Registration Policy",
                Description =
                    "Disables the ability for standard users to self-initiate Entra ID device registration via the Settings > Accounts > Work or School Account page.",
                Tags = ["entra", "device-registration", "user-consent", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot register the device with Entra; admin or MDM provisioning required.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableUserMDMEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableUserMDMEnrollment")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableUserMDMEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-enforce-hybrid-join-cert",
                Label = "Enforce Certificate Validation in Hybrid Join",
                Category = "Entra Device Registration Policy",
                Description =
                    "Enforces certificate validation during hybrid Entra ID join, ensuring the device identity certificate issued during hybrid join is verified against the enterprise Root CA.",
                Tags = ["entra", "hybrid-join", "certificate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Hybrid join certificate validated; rogue certificates rejected during device registration.",
                ApplyOps = [RegOp.SetDword(Key, "EnforceHybridJoinCertificate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceHybridJoinCertificate")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceHybridJoinCertificate", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-block-stale-device-tokens",
                Label = "Block Stale Device Token Use",
                Category = "Entra Device Registration Policy",
                Description =
                    "Blocks the use of stale or revoked Primary Refresh Tokens (PRT) from deregistered devices, preventing old device registrations from accessing cloud resources.",
                Tags = ["entra", "prt", "stale-tokens", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Stale PRTs rejected; deregistered devices cannot access cloud apps with old tokens.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockStaleDeviceTokens", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockStaleDeviceTokens")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockStaleDeviceTokens", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-set-prt-lifetime",
                Label = "Set Primary Refresh Token Lifetime to 14 Days",
                Category = "Entra Device Registration Policy",
                Description =
                    "Limits the lifetime of Primary Refresh Tokens (PRT) to 14 days, requiring devices to re-authenticate with Entra ID every two weeks and reducing the window for stolen PRT abuse.",
                Tags = ["entra", "prt", "lifetime", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PRTs expire in 14 days; devices offline longer than 14 days must re-authenticate to Entra.",
                ApplyOps = [RegOp.SetDword(Key2, "MaxDeviceTokenLifetimeDays", 14)],
                RemoveOps = [RegOp.DeleteValue(Key2, "MaxDeviceTokenLifetimeDays")],
                DetectOps = [RegOp.CheckDword(Key2, "MaxDeviceTokenLifetimeDays", 14)],
            },
            new TweakDef
            {
                Id = "entrareg-disable-self-service-bjoin",
                Label = "Disable Self-Service BYOD Registration in Settings",
                Category = "Entra Device Registration Policy",
                Description =
                    "Prevents BYOD users from adding personal devices to the organisation by disabling the Add Work or School Account option for non-admin users.",
                Tags = ["entra", "byod", "self-service", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "BYOD self-service registration blocked; personal devices cannot join the tenant without IT approval.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableSelfServiceByodRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableSelfServiceByodRegistration")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableSelfServiceByodRegistration", 1)],
            },
            new TweakDef
            {
                Id = "entrareg-require-compliant-device-for-wu",
                Label = "Require Entra-Compliant Device for Windows Update",
                Category = "Entra Device Registration Policy",
                Description =
                    "Requires the device to maintain a valid Entra ID compliance status (via Intune Compliance Policy) to receive Windows Update policy configurations from the cloud.",
                Tags = ["entra", "windows-update", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Non-compliant devices use default WU settings; only compliant devices get enterprise WU ring config.",
                ApplyOps = [RegOp.SetDword(Key2, "RequireEntraCompliantForUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RequireEntraCompliantForUpdate")],
                DetectOps = [RegOp.CheckDword(Key2, "RequireEntraCompliantForUpdate", 1)],
            },
        ];
}
