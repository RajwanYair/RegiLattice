// RegiLattice.Core — Tweaks/HolographicDevicePolicy.cs
// Sprint 357: Holographic Device Policy tweaks (10 tweaks)
// Category: "Holographic Device Policy" | Slug: holodv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HolographicDevices

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HolographicDevicePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HolographicDevices";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "holodv-restrict-holographic-device-pairing",
            Label = "Restrict Holographic Device Pairing to Authorized Devices Only",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting holographic device pairing to authorized devices prevents unauthorized HoloLens and Mixed Reality headsets from connecting to enterprise networks and accessing corporate resources. Unauthorized holographic devices connecting to enterprise networks represent a difficult-to-monitor endpoint that may not have endpoint security software installed. HoloLens devices that pair to enterprise networks can potentially access shared resources and data visible to the user identity used for authentication. Device pairing restrictions should require that holographic devices be enrolled in the organization's mobile device management platform before being granted network access. MDM enrollment ensures that holographic devices have security baselines applied including PIN requirements remote wipe capability and application control. Organizations should evaluate the business justification for holographic device usage and implement device-level certificate authentication for authorized devices.",
            Tags = ["hololens", "holographic", "device-pairing", "mixed-reality", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDevicePairing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDevicePairing")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDevicePairing", 1)],
        },
        new TweakDef
        {
            Id = "holodv-disable-developer-mode-hololens",
            Label = "Disable Developer Mode on HoloLens Mixed Reality Devices",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Developer mode on HoloLens devices enables Device Portal access side-loading of unsigned applications and remote debugging capabilities that significantly increase attack surface. Disabling developer mode on enterprise HoloLens deployments ensures that devices can only run applications that have been deployed through the organization's approved application management channels. Developer mode allows remote access to the device file system and processes through Device Portal which is a significant security risk for devices that can record audio visual information and spatial mapping data. Side-loading capabilities in developer mode allow unapproved applications to be installed bypassing the organizational application control policies. Enterprise HoloLens deployments using Commercial Suite and MDM management should have developer mode disabled from the initial device configuration. Organizations should periodically verify that developer mode has not been re-enabled on managed devices through configuration compliance checks.",
            Tags = ["hololens", "developer-mode", "side-loading", "mixed-reality", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeveloperMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeveloperMode")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeveloperMode", 1)],
        },
        new TweakDef
        {
            Id = "holodv-require-pin-for-holographic-access",
            Label = "Require PIN or Iris Authentication for HoloLens Device Access",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Requiring PIN or biometric authentication for HoloLens device access prevents unauthorized use of a misplaced or stolen device that could otherwise be accessed without authentication. HoloLens devices store authentication credentials cached application state and potentially recorded spatial data that should be protected against unauthorized physical access. Authentication requirements for holographic devices should align with the organizational policy for mobile device access controls including minimum PIN complexity and biometric fallback policies. Iris recognition on HoloLens provides a convenient and relatively secure biometric authentication method that is appropriate for devices worn on the head. Organizations should configure automatic lock timeouts that require re-authentication after periods of inactivity reducing window of opportunity for unauthorized access to an unattended device. MDM policies that enforce authentication requirements on HoloLens should also configure remote wipe capabilities for devices reported as lost or stolen.",
            Tags = ["hololens", "pin", "authentication", "biometrics", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePinForAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePinForAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePinForAccess", 1)],
        },
        new TweakDef
        {
            Id = "holodv-restrict-holographic-telemetry",
            Label = "Restrict Holographic Device Telemetry and Diagnostic Data Transmission",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Holographic device telemetry includes spatial mapping data usage patterns sensor data and system diagnostic information that is transmitted to Microsoft cloud services. Restricting holographic telemetry prevents spatial mapping data which contains detailed three-dimensional representations of the environments where the device is used from leaving organizational control. Spatial mapping data from HoloLens devices used in sensitive manufacturing research or classified environments could reveal facility layouts and equipment configurations. Enterprise deployments using the Commercial Suite should configure telemetry restriction through MDM alongside other data governance policies. The minimum telemetry level configured for standard enterprise devices should also apply to holographic devices as they are subject to the same data governance requirements. Organizations should understand what specific data is transmitted at each telemetry level and select the minimum level that maintains device functionality.",
            Tags = ["hololens", "telemetry", "spatial-data", "data-governance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictHolographicTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictHolographicTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictHolographicTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "holodv-block-unknown-app-installation",
            Label = "Block Installation of Applications from Unknown Sources on HoloLens",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Blocking application installation from unknown sources on HoloLens prevents side-loading of unapproved applications that could introduce malicious code or data exfiltration capabilities. Unknown source applications bypass the organizational application vetting process and may have access to the HoloLens sensors including cameras microphones and spatial mapping capabilities. Applications from unknown sources can capture sensitive environmental data from wherever the device is used which is a significant privacy and security risk for enterprise deployments. Enterprise HoloLens deployments should only allow applications deployed through Microsoft Store for Business or through MDM application deployment to ensure all applications have been reviewed and approved. Application allowlisting for HoloLens should complement the block on unknown sources by defining which applications from approved sources are allowed on enterprise devices. Policy changes that allow unknown sources should be treated as high-risk changes requiring security review and approval.",
            Tags = ["hololens", "application-control", "side-loading", "allowlist", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockUnknownAppInstallation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockUnknownAppInstallation")],
            DetectOps = [RegOp.CheckDword(Key, "BlockUnknownAppInstallation", 1)],
        },
        new TweakDef
        {
            Id = "holodv-restrict-camera-access-policy",
            Label = "Restrict HoloLens Camera Access to Authorized Applications",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "HoloLens camera systems including the visible light cameras depth sensors and front-facing cameras can capture video audio and spatial data from the user's environment making camera access control critical for sensitive deployments. Restricting camera access to authorized applications prevents unapproved applications from capturing visual recordings of secure facilities classified equipment or sensitive meetings. Camera access control policy should define the specific applications that are authorized to use each camera type separately since different applications may have different sensor requirements. Environments where HoloLens is used for industrial or research applications may have specific camera access requirements that differ from general enterprise usage policies. Organizations should implement technical controls that prevent camera access revocation from being overridden by applications at runtime. Audit logging for camera access by applications provides visibility into which applications are using camera capabilities and at what times.",
            Tags = ["hololens", "camera-access", "sensor-security", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictCameraAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictCameraAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictCameraAccess", 1)],
        },
        new TweakDef
        {
            Id = "holodv-enable-remote-management-policy",
            Label = "Enable Remote Management and MDM Enrollment for HoloLens Devices",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Remote management enrollment ensures that HoloLens devices are enrolled in the organizational Mobile Device Management platform enabling policy enforcement remote configuration and remote wipe capabilities. MDM enrollment is the foundation for enterprise HoloLens management providing the control plane for all other security policy enforcement on the device. Without MDM enrollment HoloLens devices operate without organizational policy oversight and cannot have security policies applied or verified remotely. Remote wipe capability through MDM is critical for HoloLens devices which are mobile and can be lost or stolen with organizational data and credentials present. MDM enrollment should be configured to auto-enroll devices upon first setup to ensure that all enterprise devices are enrolled before they are deployed to users. Organizations should define the MDM authority server URL and certificate configuration as part of the holographic device onboarding process.",
            Tags = ["hololens", "mdm", "remote-management", "enrollment", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRemoteManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteManagement")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRemoteManagement", 1)],
        },
        new TweakDef
        {
            Id = "holodv-configure-holographic-update-policy",
            Label = "Configure Update Deferral Policy for Holographic Device Operating System",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Update deferral policy for HoloLens devices controls when operating system updates are applied ensuring that updates are tested before deployment to production devices. Immediate uncontrolled update application to holographic devices used in production workflows can cause unexpected behavior changes that disrupt sensitive operations. A deferral period of 7 to 14 days allows organizations to verify that updates do not cause compatibility issues with enterprise applications before they are applied to production devices. Security update deferrals should be minimized as HoloLens security patches address vulnerabilities in a device with significant sensor capabilities. Organizations should maintain a test group of HoloLens devices that receive updates on the standard timeline to detect compatibility issues before the production fleet updates. Update deferral policy should balance security patch velocity against operational stability requirements.",
            Tags = ["hololens", "updates", "deferral", "patch-management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConfigureUpdateDeferral", 7)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureUpdateDeferral")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureUpdateDeferral", 7)],
        },
        new TweakDef
        {
            Id = "holodv-restrict-cross-device-experiences",
            Label = "Restrict Cross-Device Experience Sharing on Holographic Platforms",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Cross-device experience sharing on holographic platforms enables activity and clipboard sharing between HoloLens devices and paired Windows computers which can expose sensitive data if the paired device is not managed. Restricting cross-device experiences prevents holographic device activity data including application usage state and clipboard content from being shared with external devices. Activity sharing with unmanaged devices creates a data leakage path where content from managed HoloLens sessions can be transferred to personal devices outside organizational control. Cross-device sharing should only be permitted between managed organizational devices that have the same security policy applied. Organizations should define which cross-device experience features are permissible and configure policy to allow only those specific features while blocking others. Monitoring for cross-device sharing events can help detect attempts to circumvent data governance through holographic device cross-device features.",
            Tags = ["hololens", "cross-device", "data-sharing", "data-governance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictCrossDeviceExperiences", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictCrossDeviceExperiences")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictCrossDeviceExperiences", 1)],
        },
        new TweakDef
        {
            Id = "holodv-enable-holographic-audit-events",
            Label = "Enable Audit Event Logging for Holographic Device Operations",
            Category = "Holographic Device Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Holographic device audit event logging captures device operations including authentication events application launches sensor access and policy changes providing visibility for security monitoring. Audit events from HoloLens devices should be forwarded to centralized log management for correlation with network and identity events. Sensor access audit events are particularly important for HoloLens devices as they reveal which applications are using camera microphone and spatial mapping capabilities at what times. Authentication audit events for holographic devices help detect unauthorized access attempts including brute force attacks on PIN codes. Organizations should configure the audit event forwarding to ensure that log data is not retained solely on the device where it might be lost if the device is damaged or reset. Holographic device audit data combined with network access logs provides comprehensive visibility into how and when enterprise holographic devices are used.",
            Tags = ["hololens", "audit", "event-logging", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableHolographicAuditEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableHolographicAuditEvents")],
            DetectOps = [RegOp.CheckDword(Key, "EnableHolographicAuditEvents", 1)],
        },
    ];
}
