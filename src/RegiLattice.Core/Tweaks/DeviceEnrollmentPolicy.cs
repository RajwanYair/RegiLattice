// RegiLattice.Core — Tweaks/DeviceEnrollmentPolicy.cs
// Sprint 334: Device Enrollment Policy tweaks (10 tweaks)
// Category: "Device Enrollment Policy" | Slug: devenrl
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceEnrollmentPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "devenrl-disable-mdm-enrollment",
            Label = "Disable Automatic MDM Enrollment with Azure AD Join",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Automatic MDM enrollment triggers when a device joins Azure Active Directory and automatically enrolls it in the linked Intune Mobile Device Management tenant. Disabling automatic MDM enrollment prevents devices from auto-enrolling in MDM when users join their Azure AD accounts to devices. In managed environments where all devices should be enrolled, automatic enrollment is desirable, but in specialized scenarios enrollment may need to be controlled. Specialized devices like developer workstations, lab systems, or shared equipment may have specific reasons to avoid automatic MDM enrollment. Disabling auto-enrollment does not prevent manual IT-initiated enrollment which can still occur through IT-directed processes. Organizations should carefully evaluate this setting as it can create unmanaged device gaps in environments expecting universal MDM coverage.",
            Tags = ["mdm", "enrollment", "azure-ad", "device-management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutoEnrollMDM", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoEnrollMDM")],
            DetectOps = [RegOp.CheckDword(Key, "AutoEnrollMDM", 0)],
        },
        new TweakDef
        {
            Id = "devenrl-disable-bulk-enrollment",
            Label = "Disable Bulk MDM Enrollment via Provisioning Packages",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Bulk enrollment provisioning packages allow an administrator to enroll multiple devices in MDM simultaneously using a pre-configured package. Disabling bulk enrollment prevents provisioning packages from enrolling devices without interactive authentication preventing unauthorized mass enrollment. Bulk enrollment packages contain authentication credentials and if the package is captured it could be used to enroll unauthorized devices into the MDM tenant. IT administrators should use certificate-based bulk enrollment with short-validity certificates rather than username and password provisioning packages. Disabling bulk enrollment forces all device enrollment to use individual authenticated enrollment preventing bulk package replay attacks. Organizations that need bulk enrollment should use Windows Autopilot instead which provides stronger enrollment authentication.",
            Tags = ["mdm", "bulk-enrollment", "provisioning", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBulkEnrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBulkEnrollment")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBulkEnrollment", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-enable-enrollment-status-page",
            Label = "Enable MDM Enrollment Status Page During Autopilot",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The MDM Enrollment Status Page blocks user access to the device until all required MDM policies and applications are successfully applied during Autopilot provisioning. Enabling the enrollment status page ensures that users cannot bypass required security configurations by accessing the device before MDM setup is complete. Without the enrollment status page users can log on before required security applications like endpoint protection are installed creating a window of vulnerability. The enrollment status page prevents devices from entering use without all compliance and security configurations required by MDM policy. Blocking access during enrollment is particularly important for security-critical configurations like full disk encryption that must complete before data is created. Organizations should configure a meaningful timeout and error handling to prevent enrollment failures from permanently blocking device access.",
            Tags = ["mdm", "enrollment-status", "autopilot", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableEnrollmentStatusPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableEnrollmentStatusPage")],
            DetectOps = [RegOp.CheckDword(Key, "EnableEnrollmentStatusPage", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-block-unknown-unenrollment",
            Label = "Block User-Initiated MDM Unenrollment",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "User-initiated MDM unenrollment allows end users to remove corporate management from their devices through the Settings application. Blocking user-initiated unenrollment prevents employees from removing corporate MDM management to evade security policies or monitoring. Unenrollment from MDM would remove all deployed security policies, applications, and configurations leaving the device non-compliant. Disabling unenrollment ensures that devices remain under corporate management for their operational lifetime without requiring IT intervention to re-enroll. Blocking unenrollment is particularly important for CYOD and COPE scenarios where corporate data must remain protected at all times. IT processes for legitimate device retirement or reassignment should include formal MDM unenrollment through administrative procedures rather than user self-service.",
            Tags = ["mdm", "unenrollment", "device-management", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisallowMDMUnenrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisallowMDMUnenrollment")],
            DetectOps = [RegOp.CheckDword(Key, "DisallowMDMUnenrollment", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-require-enrollment-compliance",
            Label = "Require MDM Enrollment Compliance Before Resource Access",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "MDM enrollment compliance requirements block access to corporate resources from devices that are not enrolled in MDM management. Requiring enrollment compliance implements zero-trust access principles by ensuring only managed devices can access corporate email, applications, and data. Non-enrolled devices lack the security configuration baselines, endpoint protection, and monitoring that managed endpoints provide. Compliance-gated resource access forces all devices seeking corporate data to register under management before receiving access. Organizations should combine enrollment requirements with conditional access policies in Azure AD and Intune for comprehensive enforcement. Compliance requirements should be communicated to users during device provisioning so they understand the enrollment requirement for resource access.",
            Tags = ["mdm", "compliance", "access-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireComplianceCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireComplianceCheck")],
            DetectOps = [RegOp.CheckDword(Key, "RequireComplianceCheck", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-enable-enrollment-certificate-auth",
            Label = "Require Certificate Authentication for MDM Enrollment",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Certificate authentication for MDM enrollment replaces username and password credentials with device certificates that are more resistant to phishing and credential theft. Requiring certificate authentication ensures that only devices with valid enterprise certificates issued by the organizational PKI can enroll in MDM. Strong device identity through certificates ensures that MDM enrollment is limited to devices that have gone through the IT provisioning process. Certificate-based enrollment prevents attackers from enrolling unauthorized devices using stolen credentials. Enterprise certificates for device enrollment should be issued with appropriate validity periods and revocation capabilities. Certificate authentication for MDM enrollment aligns with zero-trust principles of verified device identity before granting management access.",
            Tags = ["mdm", "certificate-auth", "enrollment", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireCertificateAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireCertificateAuth")],
            DetectOps = [RegOp.CheckDword(Key, "RequireCertificateAuth", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-audit-enrollment-events",
            Label = "Enable Audit Logging for MDM Enrollment Events",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "MDM enrollment audit logging records all enrollment actions including successful enrollments, failed attempts, and unenrollment operations. Enabling enrollment audit logging provides a complete record of device management changes for security investigation and compliance reporting. Enrollment logs help detect unauthorized enrollment attempts by unauthorized users trying to add managed credentials to devices they should not access. Unenrollment events in audit logs can alert security teams when devices are removed from management unexpectedly. MDM enrollment audit events should be forwarded to SIEM for correlation with other device and identity events. Regular review of enrollment audit logs helps identify devices that have been enrolled multiple times which may indicate credential theft or device cloning attempts.",
            Tags = ["mdm", "audit", "enrollment-logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditEnrollmentEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditEnrollmentEvents")],
            DetectOps = [RegOp.CheckDword(Key, "AuditEnrollmentEvents", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-set-enrollment-retry-limit",
            Label = "Set Maximum MDM Enrollment Retry Limit",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "MDM enrollment retry limits prevent automated brute-force enrollment attempts by limiting the number of consecutive enrollment failures before locking the enrollment channel. Setting enrollment retry limits reduces the effectiveness of automated attacks against MDM enrollment endpoints using credential stuffing or brute force. Excessive enrollment failures may indicate a misconfigured provisioning package or an unauthorized device attempting to enroll using stolen credentials. After reaching the retry limit the device should require IT intervention to reset before enrollment can be attempted again. The retry limit should be set high enough to accommodate legitimate transient network failures but low enough to detect automated attack patterns. Enrollment retry events should be monitored and alerts triggered when retry limits are approached or reached.",
            Tags = ["mdm", "retry-limit", "brute-force-protection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxEnrollmentRetries", 5)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxEnrollmentRetries")],
            DetectOps = [RegOp.CheckDword(Key, "MaxEnrollmentRetries", 5)],
        },
        new TweakDef
        {
            Id = "devenrl-enforce-enrollment-encryption",
            Label = "Enforce BitLocker Before MDM Enrollment Completion",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "BitLocker enforcement during MDM enrollment ensures that full disk encryption is enabled before the device is classified as enrolled and compliant. Requiring BitLocker during enrollment ensures that corporate data cannot be created on unencrypted devices that could bypass data loss prevention policies. Enrollment-time BitLocker enforcement prevents devices from accessing corporate resources until encryption is fully enabled and the recovery key is escrowed to Azure AD or Active Directory. Without enrollment-time BitLocker enforcement a device could access corporate data with a temporary compliance bypass before encryption is configured. BitLocker Silent Encryption using key escrow to Azure AD provides zero-touch encryption during Autopilot enrollment without user interaction. BitLocker enforcement requirements should be defined in the MDM compliance policy and verified before granting access to sensitive resources.",
            Tags = ["mdm", "bitlocker", "encryption", "enrollment", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireBitLockerAtEnrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireBitLockerAtEnrollment")],
            DetectOps = [RegOp.CheckDword(Key, "RequireBitLockerAtEnrollment", 1)],
        },
        new TweakDef
        {
            Id = "devenrl-restrict-enrollment-to-approved-tenant",
            Label = "Restrict MDM Enrollment to Approved Tenant Only",
            Category = "Device Enrollment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Tenant enrollment restriction limits MDM enrollment to a specific Azure AD tenant preventing devices from being enrolled in unauthorized or attacker-controlled tenants. Restricting enrollment to the approved tenant prevents adversaries from enrolling corporate devices in a rogue MDM tenant to gain management control. Tenant-restricted enrollment is particularly important for corporate shared devices and lab equipment that may be accessed by multiple users. Without tenant restrictions a user with global administrator rights in a different tenant could enroll a device they have physical access to. Enrollment tenant restrictions should be configured through Windows Registry as a machine-wide policy that applies regardless of the currently signed-in user. Organizations should combine tenant enrollment restrictions with Windows Defender ATP device enrollment to ensure all devices are enrolled in the correct tenant.",
            Tags = ["mdm", "tenant-restriction", "enrollment", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictToApprovedTenant", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictToApprovedTenant")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictToApprovedTenant", 1)],
        },
    ];
}
