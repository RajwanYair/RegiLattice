// RegiLattice.Core — Tweaks/DeviceEnrollmentLimitPolicy.cs
// Device Enrollment Limit Policy — Sprint 570.
// Configures Group Policy for controlling the number and type of
// devices a user can enroll in MDM: per-user device limits, BYOD
// enrollment restrictions, corporate device tagging requirements,
// and enrollment scope filtering.
// Category: "Device Enrollment Limit Policy" | Slug: devenl
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceEnrollment

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceEnrollmentLimitPolicy
{
    private const string EnlKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceEnrollment";

    private const string MdmKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "devenl-set-max-devices-per-user-5",
                Label = "Device Enrollment Limit: Set Maximum Devices per User to 5",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets MaxDevicesPerUser=5 in DeviceEnrollment policy. Limits the number of devices a single user account can enroll in MDM to 5. Without per-user limits, a single compromised account can be used to enroll large numbers of devices into the MDM tenant, consuming Intune licenses, polluting the device inventory, and potentially using the MDM service to push malware to enrolled devices. A limit of 5 is generous enough for users with a phone, tablet, laptop, home PC, and a spare device, while preventing bulk enrollment abuse.",
                Tags = ["enrollment", "device-limit", "per-user", "abuse-prevention", "inventory"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Each user can enroll a maximum of 5 devices. Attempts to enroll a 6th device are rejected until an existing device is unenrolled. Adjust the limit if your organisation has users with more than 5 managed devices (e.g., kiosk operators managing multiple shared devices with a single service account).",
                ApplyOps = [RegOp.SetDword(EnlKey, "MaxDevicesPerUser", 5)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "MaxDevicesPerUser")],
                DetectOps = [RegOp.CheckDword(EnlKey, "MaxDevicesPerUser", 5)],
            },
            new TweakDef
            {
                Id = "devenl-block-byod-personal-enrollment",
                Label = "Device Enrollment Limit: Block Personal BYOD Devices from Enrolling",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets BlockPersonalDeviceEnrollment=1 in DeviceEnrollment policy. Prevents devices that are registered as personal devices (not Azure AD Joined or Hybrid Joined) from enrolling in corporate MDM. A personally-owned device that enrolls in corporate MDM becomes subject to remote wipe commands — which could irreversibly delete personal data. Blocking personal device enrollment prevents accidental enrollment of personal hardware into MDM while protecting users' personal devices from corporate management actions. Users who need BYOD access should use Workplace Join with limited MDM (MAM without device enrollment) instead.",
                Tags = ["byod", "personal-device", "enrollment-block", "remote-wipe-protection", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Personal (non-AAD-Joined) devices cannot enroll in corporate MDM. Users who attempt to add a work account on a personal device get a generic failure. BYOD users should sign in with MAM-only (app-level management) via Outlook or Teams apps instead. Requires AAD Join for full MDM enrollment.",
                ApplyOps = [RegOp.SetDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "BlockPersonalDeviceEnrollment")],
                DetectOps = [RegOp.CheckDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenl-require-device-category-on-enroll",
                Label = "Device Enrollment Limit: Require Device Category Assignment at Enrollment",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets RequireDeviceCategoryOnEnrollment=1 in DeviceEnrollment policy. Requires administrators to assign a device category (e.g., Corporate Laptop, Kiosk, Shared Workstation) at enrollment time. Device categories in Intune are used to automatically assign devices to dynamic groups, which in turn receive different policy sets. Without mandatory category assignment, all devices land in the uncategorised default group and receive a single policy set. Mandatory categories ensure that kiosk devices, shared workstations, and executive laptops each receive appropriately scoped policies from the moment of enrollment.",
                Tags = ["enrollment", "device-category", "dynamic-group", "policy-scoping", "intune"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Device category must be assigned before enrollment completes. Enrollment fails if no category is selected. Category assignment is performed by the enrolling admin or in automated flows by the Autopilot assignment group. No user-facing UI change for standard users.",
                ApplyOps = [RegOp.SetDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireDeviceCategoryOnEnrollment")],
                DetectOps = [RegOp.CheckDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenl-block-unused-enrollment-profiles",
                Label = "Device Enrollment Limit: Block Devices Without an Enrollment Profile",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets RequireEnrollmentProfile=1 in DeviceEnrollment policy. Prevents devices from enrolling unless they match a pre-configured Intune enrollment profile (Device Enrollment Program, Autopilot profile, or bulk enrollment token). Without this restriction, any device that has credentials for a licensed user can self-enroll in MDM using the standard Settings > Accounts flow. Pre-requiring an enrollment profile means that only devices that IT has explicitly authorized for enrollment (by creating or assigning a profile) can join MDM — unknown or unauthorized devices are rejected.",
                Tags = ["enrollment", "enrollment-profile", "autopilot", "authorization", "unknown-devices"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only devices that match a pre-configured enrollment profile can enroll. Devices without a matching profile are rejected at enrollment. Devices must be registered in Intune/Autopilot before attempting enrollment. Prevents rogue devices from enrolling with valid user credentials.",
                ApplyOps = [RegOp.SetDword(EnlKey, "RequireEnrollmentProfile", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireEnrollmentProfile")],
                DetectOps = [RegOp.CheckDword(EnlKey, "RequireEnrollmentProfile", 1)],
            },
            new TweakDef
            {
                Id = "devenl-restrict-enrollment-to-aad-join",
                Label = "Device Enrollment Limit: Restrict MDM Enrollment to AAD Joined Devices Only",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets RestrictEnrollmentToAadJoin=1 in MDM policy. Prevents MDM enrollment from completing unless the device is Azure AD Joined (not just Workplace Joined). Workplace Join provides a limited form of registration that does not require the device to be AAD-joined — this allows personal devices to register without a full AAD Join. By restricting enrollment to AAD Join, this policy ensures that enrolled devices are fully registered in Azure AD with a machine account, which is required for Hybrid Join, Conditional Access device trust, and all domain-level group policies backed by AAD.",
                Tags = ["enrollment", "aad-join", "device-trust", "conditional-access", "hybrid-join"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enrollment is restricted to devices that complete Azure AD Join. Workplace Join-only devices cannot enroll. Hybrid Joined devices (on-premises AD + AAD) satisfy the AAD Join requirement. Purely on-premises AD-joined devices without AAD sync must Hybrid-Join before they can enroll.",
                ApplyOps = [RegOp.SetDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "RestrictEnrollmentToAadJoin")],
                DetectOps = [RegOp.CheckDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
            },
            new TweakDef
            {
                Id = "devenl-enable-enrollment-status-page",
                Label = "Device Enrollment Limit: Enable MDM Enrollment Status Page During OOBE",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets ShowEnrollmentStatusPage=1 in DeviceEnrollment policy. Enables the Enrollment Status Page (ESP) during Autopilot or standard OOBE enrollment. The ESP shows the user (and IT) the real-time progress of device setup: account provisioning, app installations, policy applications, and certificate enrollments. Without the ESP, the user is deposited at the desktop while apps are still installing or policies are still applying — the device may appear functional but actually be in an incomplete configuration state. The ESP holds the user at the setup screen until all critical configurations are complete.",
                Tags = ["enrollment", "esp", "oobe", "autopilot", "setup-progress"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enrollment Status Page is shown during Autopilot/OOBE. Users are blocked at the ESP until all required apps and policies are applied. Prevents users from using a partially-configured device. Increases initial setup time by the duration of app installations.",
                ApplyOps = [RegOp.SetDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "ShowEnrollmentStatusPage")],
                DetectOps = [RegOp.CheckDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
            },
            new TweakDef
            {
                Id = "devenl-block-enrollment-from-unknown-networks",
                Label = "Device Enrollment Limit: Block Enrollment Attempts from Non-Corporate Networks",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets RequireCorporateNetworkForEnrollment=1 in DeviceEnrollment policy. Restricts MDM enrollment to devices on corporate networks (defined by the network location awareness profile). Enrollment attempts from unclassified or public networks are blocked. This prevents bulk enrollment of devices by an attacker using stolen credentials from outside the corporate network perimeter. While this is most relevant for legacy MDM setups without Azure AD conditional access, it adds network perimeter enforcement as an extra enrollment control — enrollment over public networks requires re-evaluation of the risk posture.",
                Tags = ["enrollment", "network-restriction", "corporate-network", "nla", "perimeter"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "MDM enrollment is only permitted from networks classified as corporate (domain controller reachable, NLA domain profile active). Devices on guest, public, or unclassified networks cannot enroll. This may prevent legitimate remote onboarding — coordinate with VPN policies to ensure remote enrollment is still possible via corporate VPN.",
                ApplyOps = [RegOp.SetDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireCorporateNetworkForEnrollment")],
                DetectOps = [RegOp.CheckDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenl-log-enrollment-failures",
                Label = "Device Enrollment Limit: Enable Detailed Logging of Enrollment Failures",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets LogEnrollmentFailures=1 in DeviceEnrollment policy. Enables detailed logging of MDM enrollment failure events to the Windows Event Log. Enrollment failures are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel with structured error codes (HRESULT), the enrollment phase that failed (token acquisition, DRS discovery, enrollment registration, certificate acquisition), and whether the failure was a network error, authentication error, or server error. This significantly accelerates helpdesk troubleshooting of Autopilot and enrollment failures.",
                Tags = ["enrollment", "failure-logging", "event-log", "diagnostics", "helpdesk"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM enrollment failures are logged with structured error codes and phase information. Logs written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider channel. No performance impact — logging only occurs on failure paths.",
                ApplyOps = [RegOp.SetDword(EnlKey, "LogEnrollmentFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "LogEnrollmentFailures")],
                DetectOps = [RegOp.CheckDword(EnlKey, "LogEnrollmentFailures", 1)],
            },
            new TweakDef
            {
                Id = "devenl-require-mfa-for-enrollment",
                Label = "Device Enrollment Limit: Require MFA at MDM Enrollment Time",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets RequireMfaForEnrollment=1 in DeviceEnrollment policy. Requires multi-factor authentication at the time of MDM enrollment in addition to the standard password credential. Without MFA at enrollment, a stolen password is sufficient to enroll an attacker's device into the corporate MDM tenant. With enrollment MFA enforced, the attacker must also have the victim's second factor (phone, hardware key) to complete enrollment. MDM enrollment grants the device significant privileges (policy application, certificate issuance, resource access upon compliance) — requiring MFA at this critical step is essential.",
                Tags = ["mfa", "enrollment", "authentication", "conditional-access", "identity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Users must complete MFA during MDM enrollment. Requires Azure AD MFA or equivalent. Autopilot deployments using device-identity-based enrollment (PPKG or DEM account) may need exemption. Test Autopilot flows before broad deployment.",
                ApplyOps = [RegOp.SetDword(EnlKey, "RequireMfaForEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireMfaForEnrollment")],
                DetectOps = [RegOp.CheckDword(EnlKey, "RequireMfaForEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenl-audit-enrollment-activity",
                Label = "Device Enrollment Limit: Audit All Device Enrollment Activity",
                Category = "Device Enrollment Limit Policy",
                Description =
                    "Sets AuditEnrollmentActivity=1 in DeviceEnrollment policy. Enables audit logging for all device enrollment activity: successful enrollments, failed enrollment attempts, enrollment profile matching and rejection, and unenrollment events. Audit records include the user UPN that initiated the enrollment, the device serial number and hardware ID, the enrollment profile matched (or lack thereof), and the outcome. Enrollment audit logs are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel and can be forwarded to SIEM for detection of rogue enrollment attempts.",
                Tags = ["enrollment", "audit", "siem", "monitoring", "security-event"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All enrollment attempts are logged with user, device, and outcome details. Audit events can be forwarded to SIEM. Detection: unusually high enrollment failures from a single user may indicate credential stuffing. No performance overhead — logging is asynchronous.",
                ApplyOps = [RegOp.SetDword(EnlKey, "AuditEnrollmentActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(EnlKey, "AuditEnrollmentActivity")],
                DetectOps = [RegOp.CheckDword(EnlKey, "AuditEnrollmentActivity", 1)],
            },
        ];
}
