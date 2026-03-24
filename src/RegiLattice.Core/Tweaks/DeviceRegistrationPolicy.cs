// RegiLattice.Core — Tweaks/DeviceRegistrationPolicy.cs
// Device registration and enrollment machine-scope GPO controls — Sprint 210.
// Controls device join, MDM enrollment, registration tokens, and workplace registration.
// Category: "Device Registration Policy" | Slug: devreg
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceRegistration

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceRegistrationPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceRegistration";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "devreg-disable-auto-device-registration",
                Label = "Disable Automatic Azure AD Device Registration",
                Category = "Device Registration Policy",
                Description =
                    "Prevents the device from automatically registering with Azure Active Directory / Entra ID during domain join or user sign-in. Gives IT full control over when and how devices are registered. Default: auto-register on domain join. Recommended: 1 when phased registration is required.",
                Tags = ["device-registration", "azure-ad", "entra", "mdm", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Device does not automatically register with Azure AD/Entra on domain join; manual or scripted registration is required.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoDeviceRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDeviceRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoDeviceRegistration", 1)],
            },
            new TweakDef
            {
                Id = "devreg-require-tpm-for-registration",
                Label = "Require TPM for Device Registration",
                Category = "Device Registration Policy",
                Description =
                    "Mandates that a TPM 2.0 chip is present and functional before the device can complete Azure AD registration. Ensures only hardware-attested devices can enrol; blocks VMs and devices without TPM. Default: TPM not required. Recommended: 1 for Zero Trust deployments.",
                Tags = ["device-registration", "tpm", "hardware-attestation", "zero-trust", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Devices without TPM 2.0 cannot register with Azure AD; hardware attestation is mandatory.",
                ApplyOps = [RegOp.SetDword(Key, "RequireTpmForRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmForRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTpmForRegistration", 1)],
            },
            new TweakDef
            {
                Id = "devreg-set-registration-retry-3",
                Label = "Set Device Registration Retry Count to 3",
                Category = "Device Registration Policy",
                Description =
                    "Limits the number of automatic re-registration attempts when initial Azure AD registration fails (e.g., due to network error) to 3 before stopping. Prevents persistent registration loops. Default: unlimited retries. Recommended: 3.",
                Tags = ["device-registration", "retry", "enrollment", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Device stops attempting re-registration after 3 failures; reduces background registration loop network noise.",
                ApplyOps = [RegOp.SetDword(Key, "MaxRegistrationRetries", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxRegistrationRetries")],
                DetectOps = [RegOp.CheckDword(Key, "MaxRegistrationRetries", 3)],
            },
            new TweakDef
            {
                Id = "devreg-block-personal-account-registration",
                Label = "Block Personal MSA Device Registration",
                Category = "Device Registration Policy",
                Description =
                    "Prevents users from registering the device with their personal Microsoft Account (MSA). Only corporate Azure AD / Entra accounts can register the device. Default: MSA registration allowed. Recommended: 1 on managed corporate endpoints.",
                Tags = ["device-registration", "msa", "personal-account", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Personal MSA device registration is blocked; only Entra ID / Azure AD corporate accounts can register.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPersonalAccountDeviceRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPersonalAccountDeviceRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPersonalAccountDeviceRegistration", 1)],
            },
            new TweakDef
            {
                Id = "devreg-disable-user-initiated-registration",
                Label = "Block Users from Initiating Device Registration",
                Category = "Device Registration Policy",
                Description =
                    "Prevents standard users from accessing the 'Join this device to Azure AD' and 'Connect to work or school' flows in Settings. Only administrators can register the device. Default: users allowed. Recommended: 1 on shared/kiosk endpoints.",
                Tags = ["device-registration", "user-restriction", "settings", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Settings → Accounts → Access work or school registration flows are hidden for standard users.",
                ApplyOps = [RegOp.SetDword(Key, "BlockUserInitiatedDeviceRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUserInitiatedDeviceRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUserInitiatedDeviceRegistration", 1)],
            },
            new TweakDef
            {
                Id = "devreg-enable-registration-audit-log",
                Label = "Enable Device Registration Audit Logging",
                Category = "Device Registration Policy",
                Description =
                    "Enables Security audit events for device registration and de-registration actions. Allows SOC/SIEM correlation of device lifecycle events with user authentication. Default: not audited. Recommended: 1 in SOC-monitored environments.",
                Tags = ["device-registration", "audit", "logging", "security", "siem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Device join/leave events are written to the Security event log; consumable by SIEM platforms.",
                ApplyOps = [RegOp.SetDword(Key, "EnableRegistrationAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRegistrationAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRegistrationAudit", 1)],
            },
            new TweakDef
            {
                Id = "devreg-enforce-compliant-device-only",
                Label = "Require Device Compliance for Registration",
                Category = "Device Registration Policy",
                Description =
                    "Enforces that the device must meet Intune / Endpoint Manager compliance policies before completing Azure AD Hybrid registration. Non-compliant devices are blocked until they satisfy the compliance posture. Default: not enforced. Recommended: 1 for Conditional Access deployments.",
                Tags = ["device-registration", "compliance", "intune", "conditional-access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Non-compliant devices (missing patches, disabled Defender) cannot complete registration; gate for Conditional Access.",
                ApplyOps = [RegOp.SetDword(Key, "RequireDeviceCompliance", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceCompliance")],
                DetectOps = [RegOp.CheckDword(Key, "RequireDeviceCompliance", 1)],
            },
            new TweakDef
            {
                Id = "devreg-certificate-validity-days-365",
                Label = "Set Device Certificate Validity to 365 Days",
                Category = "Device Registration Policy",
                Description =
                    "Configures the maximum validity period for the device authentication certificate issued during Azure AD registration to 365 days. Forces annual certificate renewal, reducing the window of credential exposure. Default: 180 days. Recommended: 365 for balance.",
                Tags = ["device-registration", "certificate", "validity", "renewal", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Device certificates are valid for 1 year; renewal is required annually to maintain device trust.",
                ApplyOps = [RegOp.SetDword(Key, "DeviceCertValidityDays", 365)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeviceCertValidityDays")],
                DetectOps = [RegOp.CheckDword(Key, "DeviceCertValidityDays", 365)],
            },
            new TweakDef
            {
                Id = "devreg-block-stale-device-reuse",
                Label = "Block Re-Registration of Already-Registered Device Record",
                Category = "Device Registration Policy",
                Description =
                    "Prevents a device from creating a new Azure AD registration record if a record for the same device already exists (stale object). Requires IT to clean up the old object before re-registration. Default: new record created silently. Recommended: 1.",
                Tags = ["device-registration", "stale", "reuse", "hygiene", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Device will not create a duplicate Azure AD object; IT must retire the stale record before re-registration.",
                ApplyOps = [RegOp.SetDword(Key, "BlockStaleDeviceReRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockStaleDeviceReRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockStaleDeviceReRegistration", 1)],
            },
            new TweakDef
            {
                Id = "devreg-disable-registration-status-page-skip",
                Label = "Block Skipping Device Registration Status Page (OOBE)",
                Category = "Device Registration Policy",
                Description =
                    "Prevents Autopilot/OOBE from skipping the device registration status page (ESP — Enrollment Status Page). Ensures the device fully completes registration before the user can log in. Default: ESP may be skipped. Recommended: 1 during Autopilot deployments.",
                Tags = ["device-registration", "oobe", "autopilot", "esp", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OOBE/Autopilot ESP is not skipped; device is fully enrolled before the first user login.",
                ApplyOps = [RegOp.SetDword(Key, "BlockEnrollmentStatusPageSkip", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockEnrollmentStatusPageSkip")],
                DetectOps = [RegOp.CheckDword(Key, "BlockEnrollmentStatusPageSkip", 1)],
            },
        ];
}
