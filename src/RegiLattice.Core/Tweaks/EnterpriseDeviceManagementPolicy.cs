// RegiLattice.Core — Tweaks/EnterpriseDeviceManagementPolicy.cs
// Enterprise Device Management Policy — Sprint 571.
// Configures Group Policy for enterprise MDM management scope:
// Intune co-management, managed device wipe, device inventory
// sync, and enterprise-level device configuration control.
// Category: "Enterprise Device Management Policy" | Slug: edm
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EnterpriseDeviceManagementPolicy
{
    private const string ErmKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

    private const string MdmKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "edm-enable-comanagement-with-sccm",
                Label = "Enterprise Device Management: Enable Intune/SCCM Co-Management",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets EnableCoManagement=1 in MDM policy. Enables co-management of Windows 10/11 devices by both System Center Configuration Manager (SCCM/ConfigMgr) and Microsoft Intune simultaneously. Co-management allows gradual migration of workloads from SCCM to Intune — starting with compliance evaluation and conditional access in Intune while keeping software deployment in SCCM. Without this policy, organizations must choose one management plane. Co-management is the Microsoft-recommended path for organizations with existing SCCM infrastructure transitioning to cloud-modern management.",
                Tags = ["co-management", "sccm", "configmgr", "intune", "cloud-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Devices are managed by both SCCM and Intune simultaneously. Workload authority (compliance, resource access, app deployment) is configurable per workload. Requires ConfigMgr 1710 or later and Intune subscription. Co-management authority conflicts are resolved by the workload slider settings in the SCCM console.",
                ApplyOps = [RegOp.SetDword(MdmKey, "EnableCoManagement", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableCoManagement")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnableCoManagement", 1)],
            },
            new TweakDef
            {
                Id = "edm-enable-remote-lock-on-compliance-breach",
                Label = "Enterprise Device Management: Enable Remote Lock on Compliance Breach",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets EnableRemoteLockOnComplianceBreach=1 in EnterpriseResourceManager policy. Configures the MDM client to accept remote lock commands from the MDM authority when the device is marked non-compliant AND has not remediated within the grace period. Remote lock sets the device to the lock screen and requires the user to enter their PIN/password to regain access. This prevents a non-compliant device from being used while IT is investigating or while the device is remediating a compliance issue — ensuring that a known-non-compliant device is not being actively used to access corporate resources.",
                Tags = ["remote-lock", "compliance", "non-compliant", "mdm", "incident-response"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "MDM authority can remotely lock a non-compliant device. The device requires the user's credentials to unlock. User may be temporarily unable to complete their work if locked during active use. Ensure a clear remediation process is communicated to users before deploying. Not the same as remote wipe — data is not affected.",
                ApplyOps = [RegOp.SetDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
                RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableRemoteLockOnComplianceBreach")],
                DetectOps = [RegOp.CheckDword(ErmKey, "EnableRemoteLockOnComplianceBreach", 1)],
            },
            new TweakDef
            {
                Id = "edm-enable-selective-wipe-on-unenroll",
                Label = "Enterprise Device Management: Enable Selective Wipe of Corporate Data on Unenroll",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets EnableSelectiveWipeOnUnenroll=1 in EnterpriseResourceManager policy. Enables selective wipe of corporate data when a device unenrolls from MDM. Selective wipe removes only corporate-managed content: corporate email profiles, MDM-deployed certificates, VPN profiles, Wi-Fi profiles, and corporate app data — while preserving personal files, photos, and applications. This is the appropriate default for BYOD scenarios: when an employee leaves and disconnects their personal device from MDM, the corporate data is cleaned up without erasing the employee's personal content.",
                Tags = ["selective-wipe", "unenrollment", "corporate-data", "byod", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Unenrollment from MDM triggers removal of all MDM-deployed profiles, certificates, and managed app data. Personal files and apps are preserved. A corporate AAD-joined device unenrolling may lose domain join state. Not a full device wipe — ensure your users understand what is removed on unenrollment.",
                ApplyOps = [RegOp.SetDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
                RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableSelectiveWipeOnUnenroll")],
                DetectOps = [RegOp.CheckDword(ErmKey, "EnableSelectiveWipeOnUnenroll", 1)],
            },
            new TweakDef
            {
                Id = "edm-require-approved-apps-only",
                Label = "Enterprise Device Management: Restrict App Installation to MDM-Approved Apps Only",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets RequireApprovedAppsOnly=1 in EnterpriseResourceManager policy. Restricts app installation to apps that are deployed or approved by the MDM authority. Users are not permitted to install arbitrary apps from the Microsoft Store or third-party sources unless the MDM administrator has explicitly approved them in the app catalog. This policy is typically layered with AppLocker or Windows Defender Application Control. On its own, it provides an MDM-layer approval gate that blocks app installation from retail Store listings, reducing the attack surface from malicious store apps.",
                Tags = ["approved-apps", "app-control", "mdm", "store", "whitelisting"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Only MDM-approved apps can be installed by users. Non-approved app installation attempts are blocked. Requires maintaining an approved app catalog in the MDM console. Users who need new apps must request IT approval. May disrupt productivity if the approval catalog is not kept up to date.",
                ApplyOps = [RegOp.SetDword(ErmKey, "RequireApprovedAppsOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(ErmKey, "RequireApprovedAppsOnly")],
                DetectOps = [RegOp.CheckDword(ErmKey, "RequireApprovedAppsOnly", 1)],
            },
            new TweakDef
            {
                Id = "edm-sync-device-inventory-every-4h",
                Label = "Enterprise Device Management: Sync Device Inventory to MDM Every 4 Hours",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets InventorySyncIntervalHours=4 in EnterpriseResourceManager policy. Configures the MDM client to push a device inventory update (installed apps, hardware specs, disk space, OS version, installed patches) to the MDM authority every 4 hours. Accurate, fresh device inventory is essential for software license compliance, vulnerability management (detecting devices missing patches), and asset management. A staleinventory (updated less than once daily) may miss a device that has been reformatted, had apps removed, or had OS version changed — leading to false compliance reporting.",
                Tags = ["inventory", "sync-interval", "asset-management", "vulnerability-mgmt", "mdm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Device inventory is uploaded to the MDM authority every 4 hours. Inventory includes installed apps, hardware, and OS state. Slightly increased MDM check-in frequency and bandwidth. Inventory sync data is typically 5–50 KB per cycle.",
                ApplyOps = [RegOp.SetDword(ErmKey, "InventorySyncIntervalHours", 4)],
                RemoveOps = [RegOp.DeleteValue(ErmKey, "InventorySyncIntervalHours")],
                DetectOps = [RegOp.CheckDword(ErmKey, "InventorySyncIntervalHours", 4)],
            },
            new TweakDef
            {
                Id = "edm-block-factory-reset-by-user",
                Label = "Enterprise Device Management: Prevent User-Initiated Factory Reset",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets BlockUserInitiatedFactoryReset=1 in EnterpriseResourceManager policy. Prevents standard users from performing a factory reset (Settings > System > Recovery > Reset this PC, or WinRE recovery). Factory reset bypasses MDM policies, removes all corporate data and certificates, and leaves the device unmanaged. An insider threat actor could use factory reset to wipe evidence before investigation. A regular user could accidentally factory reset, losing both personal and corporate data. IT-initiated remote wipe via the MDM console remains available for authorized operations.",
                Tags = ["factory-reset", "protective", "insider-threat", "data-preservation", "mdm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Standard users cannot initiate factory reset. Local administrators can still reset via elevated permission flows. IT-initiated remote wipe from MDM console is not affected. Users who genuinely need to re-provision their device must contact IT.",
                ApplyOps = [RegOp.SetDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
                RemoveOps = [RegOp.DeleteValue(ErmKey, "BlockUserInitiatedFactoryReset")],
                DetectOps = [RegOp.CheckDword(ErmKey, "BlockUserInitiatedFactoryReset", 1)],
            },
            new TweakDef
            {
                Id = "edm-enable-mdm-certificate-renewal",
                Label = "Enterprise Device Management: Enable Automatic MDM Certificate Renewal",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets EnableMdmCertificateRenewal=1 in MDM policy. Configures the MDM client to automatically renew the MDM enrollment certificate before it expires. The MDM enrollment certificate authenticates the device to the MDM service on every check-in. If this certificate expires without renewal, the device loses the ability to receive new policies, report compliance status, or accept remote management commands — even though it may still appear enrolled in the MDM console. Automatic renewal prevents this silent disconnection, which is especially important for devices in long-term storage or deployed in air-gapped environments.",
                Tags = ["mdm", "certificate", "renewal", "enrollment", "expiry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "MDM enrollment certificates are renewed automatically before expiry. Renewal occurs in the background without user interaction. Prevents devices from silently dropping off MDM management due to certificate expiry. Certificate validity periods are typically 1–2 years — renewal triggers at 80% of the validity period.",
                ApplyOps = [RegOp.SetDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableMdmCertificateRenewal")],
                DetectOps = [RegOp.CheckDword(MdmKey, "EnableMdmCertificateRenewal", 1)],
            },
            new TweakDef
            {
                Id = "edm-enable-managed-device-restrictions",
                Label = "Enterprise Device Management: Enable MDM-Enforced Managed Device Restrictions",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets EnableManagedDeviceRestrictions=1 in EnterpriseResourceManager policy. Enables the enforcement layer for MDM-delivered device restrictions — settings like camera disable, screen capture restriction, clipboard policy, USB disable, and Bluetooth restriction — that are delivered as MDM CSP payloads. Without this flag, MDM restriction payloads are accepted but not enforced at the OS level. This is a master switch that must be enabled for MDM-pushed restrictions to take effect. Relevant for organizations deploying information protection policies that require disabling hardware capabilities on managed devices.",
                Tags = ["mdm", "device-restrictions", "camera-disable", "clipboard", "usb"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "MDM-delivered device restrictions are enforced by the OS. Without this, restrictions are delivered but silently not applied. Restrictions that take effect depend on which CSP payloads the MDM administrator has configured — this policy enables the enforcement mechanism, not specific restrictions.",
                ApplyOps = [RegOp.SetDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
                RemoveOps = [RegOp.DeleteValue(ErmKey, "EnableManagedDeviceRestrictions")],
                DetectOps = [RegOp.CheckDword(ErmKey, "EnableManagedDeviceRestrictions", 1)],
            },
            new TweakDef
            {
                Id = "edm-audit-mdm-policy-changes",
                Label = "Enterprise Device Management: Audit All MDM Policy Application Events",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets AuditMdmPolicyChanges=1 in MDM policy. Enables audit events whenever an MDM policy is applied, updated, or removed on the device. Each audit event records the CSP path that was changed, the old and new values, the MDM authority that issued the change, and the result (success or error code). MDM policy audit events are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel. These events are essential for SIEM correlation: if a device's MDM policy is unexpectedly changed (indicating a rogue MDM push or configuration scope error), the audit trail makes detection possible.",
                Tags = ["mdm", "audit", "policy-changes", "siem", "event-log"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All MDM policy application events are logged with CSP path, values, and origin. Events written to DeviceManagement-Enterprise-Diagnostics-Provider channel. Slightly higher log volume on devices with frequent policy changes (Intune check-in + policy delta). Enables SIEM alerting on unexpected MDM policy modifications.",
                ApplyOps = [RegOp.SetDword(MdmKey, "AuditMdmPolicyChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "AuditMdmPolicyChanges")],
                DetectOps = [RegOp.CheckDword(MdmKey, "AuditMdmPolicyChanges", 1)],
            },
            new TweakDef
            {
                Id = "edm-enable-encrypted-mdm-channel",
                Label = "Enterprise Device Management: Enforce TLS 1.2+ for MDM Communication",
                Category = "Enterprise Device Management Policy",
                Description =
                    "Sets RequireEncryptedMdmChannel=1 in MDM policy. Enforces that all MDM client communication (enrollment, check-in, policy delivery, command receipt) is conducted over TLS 1.2 or higher. MDM payloads include configuration settings, app assignments, certificate payloads, and VPN profiles — all of which are sensitive. An MDM session over TLS 1.0 can be downgrade-attacked using known vulnerabilities (BEAST, POODLE) to intercept policy payloads. Enforcing TLS 1.2+ on the MDM channel ensures that policy delivery is encrypted to modern standards.",
                Tags = ["mdm", "tls", "encrypted-channel", "transport-security", "policy-delivery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "MDM communication is restricted to TLS 1.2 or higher. MDM servers that only support TLS 1.0 or 1.1 will be unable to communicate with the client. All modern MDM services (Intune, SCCM cloud attachment) use TLS 1.2+. On-premises MDM servers must be updated if they are still on legacy TLS.",
                ApplyOps = [RegOp.SetDword(MdmKey, "RequireEncryptedMdmChannel", 1)],
                RemoveOps = [RegOp.DeleteValue(MdmKey, "RequireEncryptedMdmChannel")],
                DetectOps = [RegOp.CheckDword(MdmKey, "RequireEncryptedMdmChannel", 1)],
            },
        ];
}
