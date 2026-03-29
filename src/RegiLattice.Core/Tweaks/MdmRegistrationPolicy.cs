// RegiLattice.Core — Tweaks/MdmRegistrationPolicy.cs
// MDM Registration Policy — Sprint 569.
// Configures Group Policy for Mobile Device Management enrollment:
// auto-enrollment triggers, MDM authority URL, unenrollment
// restrictions, diagnostic MDM enrollment, and scope configuration.
// Category: "MDM Registration Policy" | Slug: mdmreg
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MdmRegistrationPolicy
{
    private const string MdmKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

    private const string EnrollKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "mdmreg-enable-aad-auto-enrollment",
                Label = "MDM Registration: Enable Auto-Enrollment for Azure AD Joined Devices",
                Category = "MDM Registration Policy",
                Description =
                    "Sets AutoEnrollMDM=1 in MDM policy. Enables automatic MDM enrollment for devices that join Azure AD (Azure AD Join or Azure AD Hybrid Join). When a device joins Azure AD, the enrollment process automatically provisions the device with an MDM enrolment token and registers it with the configured MDM authority (typically Microsoft Intune). Without this policy, AAD Joined devices are registered in Azure AD but not MDM-managed — group policy, compliance checks, and app deployments via Intune will not work. Auto-enrollment is the standard corporate device onboarding mechanism.",
                Tags = ["mdm", "auto-enrollment", "azure-ad", "intune", "device-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "AAD-joined devices automatically enroll in MDM upon sign-in. The MDM authority URL and scope are read from the tenant's MDM discovery service. Requires an Intune license (or other MDM) assigned to the user. Devices already AAD-joined will not retroactively enroll — only newly joining devices are affected.",
                ApplyOps = [RegOp.SetDword(MdmKey, "AutoEnrollMDM", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "AutoEnrollMDM")],
                DetectOps = [RegOp.CheckDword(MdmKey, "AutoEnrollMDM", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-require-reenrollment-on-rename",
                Label = "MDM Registration: Require Re-Enrollment after Device Rename",
                Category = "MDM Registration Policy",
                Description =
                    "Sets RequireReenrollmentOnRename=1 in MDM policy. Forces the device to re-enroll in MDM when the device name changes. Device renaming is sometimes used as a pivot technique during lateral movement: an attacker renames a managed device to match an expected device name to pass name-based access controls. Forcing re-enrollment on rename ensures the MDM service receives a new enrollment token for the renamed device, which updates the device record in the MDM database and triggers compliance re-evaluation. Any conditional access policies that check the MDM enrollment record are therefore aware of the identity change.",
                Tags = ["mdm", "re-enrollment", "device-rename", "identity", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Device renaming triggers MDM re-enrollment. Re-enrollment is transparent — it occurs in the background without disrupting the user session. Useful in environments where device names are used as identifiers in network access rules or SIEM queries.",
                ApplyOps = [RegOp.SetDword(MdmKey, "RequireReenrollmentOnRename", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "RequireReenrollmentOnRename")],
                DetectOps = [RegOp.CheckDword(MdmKey, "RequireReenrollmentOnRename", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-disable-user-unenrollment",
                Label = "MDM Registration: Prevent Users from Manually Unenrolling Device from MDM",
                Category = "MDM Registration Policy",
                Description =
                    "Sets DisallowUserMdmUnenrollment=1 in MDM policy. Prevents standard users (non-administrators) from unenrolling the device from MDM management through the Settings app. Without this policy, any user with access to Settings > Accounts > Access work or school can disconnect the device from MDM management, effectively removing it from IT control, compliance enforcement, and conditional access scope. While admins can still unenroll via MDM push commands, preventing user-initiated unenrollment ensures the device remains managed.",
                Tags = ["mdm", "unenrollment", "user-restriction", "settings", "tamper-prevention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot disconnect the device from MDM using Settings. Local administrators and MDM push-initiated unenrollment still work. The Settings UI option to disconnect is grayed out or removed for non-admins.",
                ApplyOps = [RegOp.SetDword(MdmKey, "DisallowUserMdmUnenrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "DisallowUserMdmUnenrollment")],
                DetectOps = [RegOp.CheckDword(MdmKey, "DisallowUserMdmUnenrollment", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-use-enterprise-enrollment-only",
                Label = "MDM Registration: Restrict MDM Enrollment to Enterprise Tenants Only",
                Category = "MDM Registration Policy",
                Description =
                    "Sets EnterpriseEnrollmentOnly=1 in MDM policy. Restricts MDM enrollment so that only corporate tenants (as determined by the MDM authority in the Group Policy or the domain's MDM discovery service) can claim management of the device. Without this policy, a device can be enrolled by any MDM provider, including personal Intune accounts. This is relevant in bring-your-own-device (BYOD) scenarios where an employee might accidentally enroll their managed corporate device with their personal Microsoft 365 account's MDM, causing policy conflicts.",
                Tags = ["mdm", "enrollment", "enterprise-only", "byod", "tenant-restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only the corporate MDM authority (set by Group Policy or Windows AutoPilot) can enroll the device. Personal Microsoft account MDM enrollment is rejected. Prevents accidental dual-enrollment or policy conflicts from personal MDM tenants.",
                ApplyOps = [RegOp.SetDword(MdmKey, "EnterpriseEnrollmentOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnterpriseEnrollmentOnly")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnterpriseEnrollmentOnly", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-enable-diagnostic-auto-upload",
                Label = "MDM Registration: Enable Automatic Diagnostic Log Upload to MDM",
                Category = "MDM Registration Policy",
                Description =
                    "Sets EnableDiagnosticUpload=1 in MDM policy. Enables the MDM client to automatically upload MDM diagnostic logs to the MDM server when requested via a remote log collection push from the MDM authority. Without this, IT admins must physically access the device or use complex manual collection procedures to retrieve MDM diagnostic files. With this enabled, an MDM admin can trigger log collection from the Intune console without user interaction — essential for diagnosing enrollment failures, policy application errors, or app deployment problems on devices that are not physically accessible.",
                Tags = ["mdm", "diagnostics", "log-upload", "remote-collection", "intune"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM diagnostic logs are uploaded to the MDM server on remote request. Logs include MDM client logs, Event Log snapshots, and enrollment logs. Only the MDM server can initiate collection — users cannot trigger it. Small bandwidth overhead during collection.",
                ApplyOps = [RegOp.SetDword(MdmKey, "EnableDiagnosticUpload", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableDiagnosticUpload")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnableDiagnosticUpload", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-set-enrollment-check-in-interval-4h",
                Label = "MDM Registration: Set MDM Check-In Interval to 4 Hours",
                Category = "MDM Registration Policy",
                Description =
                    "Sets EnrollmentCheckInIntervalHours=4 in MDM policy. Sets the frequency at which the MDM client checks in with the MDM server to receive new policies, app assignments, compliance commands, and configuration updates. The default check-in interval is 8 hours. A 4-hour interval reduces the lag between MDM policy changes (such as blocking USB, pushing a security update requirement, or revering a credential) and their application on devices. In incident response scenarios, the ability to push a policy change and have it take effect within 4 hours rather than 8 hours is a meaningful response time improvement.",
                Tags = ["mdm", "check-in", "policy-apply", "interval", "response-time"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM client checks in with the MDM server every 4 hours. Reduces policy propagation lag from 8h to 4h. Slightly higher MDM service traffic — negligible for typical enterprise deployments.",
                ApplyOps = [RegOp.SetDword(MdmKey, "EnrollmentCheckInIntervalHours", 4)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnrollmentCheckInIntervalHours")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnrollmentCheckInIntervalHours", 4)],
            },
            new TweakDef
            {
                Id = "mdmreg-enable-conditional-access-notification",
                Label = "MDM Registration: Enable MDM Enrollment Notification for Conditional Access",
                Category = "MDM Registration Policy",
                Description =
                    "Sets NotifyConditionalAccessOnEnrollment=1 in MDM policy. Configures the MDM client to push an enrollment state notification to the Azure AD conditional access service whenever the device's MDM enrollment status changes (enrolled, unenrolled, compliance state changed). Without this notification push, conditional access relies on polling of the Intune device inventory, which has a delay. The push notification significantly reduces the time between an enrollment state change and the conditional access enforcement update — important for scenarios like immediately restoring access after successful compliance remediation.",
                Tags = ["mdm", "conditional-access", "enrollment-notification", "aad", "response-time"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enrollment state changes trigger an immediate push notification to AAD conditional access. Reduces the delay between compliance remediation and access restoration. Requires AAD and Intune integration — has no effect on on-premises MDM without AAD integration.",
                ApplyOps = [RegOp.SetDword(MdmKey, "NotifyConditionalAccessOnEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "NotifyConditionalAccessOnEnrollment")],
                DetectOps = [RegOp.CheckDword(MdmKey, "NotifyConditionalAccessOnEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-block-guest-from-enrollment",
                Label = "MDM Registration: Block Guest Accounts from MDM Enrollment",
                Category = "MDM Registration Policy",
                Description =
                    "Sets BlockGuestAccountEnrollment=1 in MDM policy. Prevents Guest accounts from triggering MDM enrollment or accessing MDM-managed resources. Guest accounts by definition have no AAD identity and should not enroll in MDM. In some configurations, a device with an active Guest session can inadvertently trigger MDM enrollment flows with an empty principal, creating orphaned device records in the MDM tenant. Blocking guest account enrollment eliminates this edge case and prevents Guest-session processes from interacting with the MDM client.",
                Tags = ["mdm", "guest-account", "enrollment-block", "identity", "orphaned-device"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Guest accounts cannot initiate or complete MDM enrollment. Prevents orphaned MDM device records from Guest-triggered enrollment flows. No impact on standard user or administrator enrollment processes.",
                ApplyOps = [RegOp.SetDword(MdmKey, "BlockGuestAccountEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "BlockGuestAccountEnrollment")],
                DetectOps = [RegOp.CheckDword(MdmKey, "BlockGuestAccountEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-enable-silent-enrollment",
                Label = "MDM Registration: Enable Silent (No User Prompt) MDM Enrollment",
                Category = "MDM Registration Policy",
                Description =
                    "Sets EnableSilentEnrollment=1 in MDM policy. Configures MDM enrollment to complete silently without displaying user-facing dialogs, progress indicators, or consent prompts. Silent enrollment is used in corporate provisioning scenarios (Autopilot, bulk enrolment) where the device is pre-configured by IT before delivery to the user. Without silent enrollment, the MDM client shows enrollment progress dialogs that may alarm users who are not expecting them. Silent enrollment also reduces the risk of users cancelling the enrollment process mid-flow, which can leave the device in a partially-enrolled state.",
                Tags = ["mdm", "silent-enrollment", "autopilot", "provisioning", "user-experience"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MDM enrollment completes without user-visible dialogs or prompts. Used in Autopilot and bulk enrollment scenarios. Best combined with Enrollment Status Page (ESP) for user transparency during provisioning.",
                ApplyOps = [RegOp.SetDword(MdmKey, "EnableSilentEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableSilentEnrollment")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnableSilentEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "mdmreg-enable-enrollment-retry-on-failure",
                Label = "MDM Registration: Enable Automatic Retry on MDM Enrollment Failure",
                Category = "MDM Registration Policy",
                Description =
                    "Sets EnableEnrollmentRetryOnFailure=1 in EnrollmentSecurity policy. Enables the MDM client to automatically retry enrollment if the initial enrollment attempt fails due to network connectivity issues, MDM service transient errors, or AAD token acquisition failures. Without retry logic, a single transient failure during Autopilot provisioning (e.g., the device starts enrollment before DNS is fully resolving, or the MDM service returns HTTP 503 during a brief outage) results in a permanently unenrolled device that requires manual remediation. Automatic retry ensures transient failures are recovered without IT intervention.",
                Tags = ["mdm", "enrollment-retry", "resilience", "autopilot", "transient-failure"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Failed MDM enrollment attempts are automatically retried with exponential backoff. Significantly reduces Autopilot and bulk-enrollment failures due to transient connectivity or service errors. Retry schedule is governed by the MDM client's built-in backoff policy.",
                ApplyOps = [RegOp.SetDword(EnrollKey, "EnableEnrollmentRetryOnFailure", 1)],
                RemoveOps = [RegOp.DeleteValue(EnrollKey, "EnableEnrollmentRetryOnFailure")],
                DetectOps = [RegOp.CheckDword(EnrollKey, "EnableEnrollmentRetryOnFailure", 1)],
            },
        ];
}
