// RegiLattice.Core — Tweaks/IntuneDeviceEventPolicy.cs
// Microsoft Intune MDM client event logging and device health reporting Group Policy controls (Sprint 619).
// Category: "Intune Device Event Policy" | Slug: intuneev
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection\MDM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class IntuneDeviceEventPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection\MDM";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "intuneev-enable-device-health-reporting",
            Label = "Intune: Enable Intune Device Health Reporting for Compliance Assessment",
            Category = "Intune Device Event Policy",
            Description = "Sets EnableDeviceHealthReporting=1 in the MDM data collection policy. Enables the Intune client health reporting service which sends device health attestation data — TPM status, Secure Boot state, BitLocker encryption status, ELAM driver state, UEFI firmware version — to the Intune service for compliance policy evaluation. " +
                "Intune's device compliance policies can gate conditional access (blocking Microsoft 365, SharePoint, or other Entra ID protected resources) based on device health. For health-based conditional access to function, the device must send health attestation reports. Disabling health reporting (or leaving it unconfigured) causes compliance status to show as 'Unknown', which depending on conditional access policy settings may either block all access or allow access by default for unknown-state devices.",
            Tags = ["intune", "mdm", "health-reporting", "compliance", "tpm", "conditional-access"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Intune device health reports sent to service; compliance-based conditional access evaluates correct device health state.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDeviceHealthReporting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDeviceHealthReporting")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDeviceHealthReporting", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-disable-mdm-diagnostic-telemetry-upload",
            Label = "Intune: Disable Voluntary MDM Diagnostic Data Upload to Microsoft",
            Category = "Intune Device Event Policy",
            Description = "Sets DisableMDMDiagnosticsTelemetry=1 in the MDM data collection policy. Stops the Intune MDM client from uploading optional diagnostic data about MDM client performance, error rates, and command processing latency to Microsoft's MDM service telemetry pipeline, separate from Windows diagnostic data. " +
                "The MDM client telemetry pipeline transmits information about policy processing durations, enrollment command error codes, and sync performance metrics. While this data is used by Microsoft for service improvement and does not contain policy payload content, it reveals information about the organisation's governance structure: how many MDM commands are failing, which policy types are erroring, and whether device compliance is degrading. Disabling this prevents that metadata from leaving the organisation.",
            Tags = ["intune", "mdm", "telemetry", "diagnostic-data", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "MDM client performance telemetry upload stopped; MDM client metadata stays within the organisation.",
            ApplyOps = [RegOp.SetDword(Key, "DisableMDMDiagnosticsTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMDMDiagnosticsTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMDMDiagnosticsTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-require-enrollment-certificate",
            Label = "Intune: Require PKI Certificate for MDM Enrollment Authentication",
            Category = "Intune Device Event Policy",
            Description = "Sets RequireMDMEnrollmentCertificate=1 in the MDM data collection policy. Configures the MDM client to use a PKI client certificate issued by the internal CA for Intune enrollment authentication, rather than Microsoft Entra ID token-only authentication, providing a hardware-bound credential (certificate stored in TPM) alongside the Entra token. " +
                "Token-based MDM enrollment (Entra ID access token only) is subject to token theft attacks — an attacker who steals an Entra ID access token from a device could initiate MDM enrollment of a hostile device. PKI certificate-based enrollment requires the certificate private key (ideally TPM-bound) in addition to the Entra token, making stolen tokens insufficient to enrol a new device because the certificate is non-exportable from the TPM.",
            Tags = ["intune", "mdm", "enrollment", "certificate", "pki", "tpm"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "PKI certificate required for MDM enrollment; token theft alone insufficient to enrol a hostile device.",
            ApplyOps = [RegOp.SetDword(Key, "RequireMDMEnrollmentCertificate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireMDMEnrollmentCertificate")],
            DetectOps = [RegOp.CheckDword(Key, "RequireMDMEnrollmentCertificate", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-enable-mdm-event-audit-log",
            Label = "Intune: Enable MDM Client Audit Logging for Every Policy Command",
            Category = "Intune Device Event Policy",
            Description = "Sets EnableMDMEventAuditLog=1 in the MDM data collection policy. Enables detailed audit logging in the Windows MDM stack, causing every OMA-DM command received from the Intune service (CSP write, CSP delete, configuration profile apply, compliance check result) to generate an audit event in the Security event log. " +
                "MDM policy delivery happens silently in the background. Without audit logging, there is no on-device record of which policies were applied, when they were applied, which settings were changed, and who authorised the change. This creates a gap in the device's audit trail — changes made via MDM bypass the traditional registry audit trail. With MDM audit logging enabled, all MDM-delivered policy changes generate Security events auditable by SIEM alongside other registry change events.",
            Tags = ["intune", "mdm", "audit-log", "oma-dm", "csp", "siem"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Every MDM OMA-DM policy command generates a Security event; MDM changes included in SIEM correlation.",
            ApplyOps = [RegOp.SetDword(Key, "EnableMDMEventAuditLog", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMDMEventAuditLog")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMDMEventAuditLog", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-block-mdm-unenrollment",
            Label = "Intune: Block User-Initiated MDM Unenrollment from Settings",
            Category = "Intune Device Event Policy",
            Description = "Sets BlockMDMUnenrollment=1 in the MDM data collection policy. Prevents users from manually removing the MDM enrollment from Settings > Accounts > Access work or school, blocking self-service unenrollment that would remove all MDM-delivered policies, compliance baselines, and enterprise configuration from the device. " +
                "A user who unenrols their device from MDM removes all Intune-delivered policies, certificates, and compliance configurations in a single action. This gives users the ability to escape enterprise security enforcement by removing device management. The device continues to function normally but is no longer managed, no longer receives security patches via Intune, no longer reports compliance, and potentially still has access to enterprise resources if conditional access doesn't immediately detect the unenrollment.",
            Tags = ["intune", "mdm", "unenrollment", "lockout", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "User-initiated MDM unenrollment blocked; enterprise management cannot be removed from Settings without admin action.",
            ApplyOps = [RegOp.SetDword(Key, "BlockMDMUnenrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockMDMUnenrollment")],
            DetectOps = [RegOp.CheckDword(Key, "BlockMDMUnenrollment", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-enforce-compliance-check-daily",
            Label = "Intune: Enforce Daily MDM Compliance Check-In Regardless of Network",
            Category = "Intune Device Event Policy",
            Description = "Sets EnforceComplianceCheckCadenceHours=24 in the MDM data collection policy. Forces the Intune MDM client to attempt a compliance status check-in to the Intune service at least once every 24 hours, even if the last successful sync was within the standard 8-hour interval, ensuring compliance policy is always evaluated at least daily. " +
                "MDM sync frequency is typically driven by the Intune service push schedule. Devices that are frequently off the corporate network (remote workers using cellular connections) may go days between Intune syncs if they are not on Wi-Fi and data usage policies are aggressive. A device not syncing for multiple days may have outdated compliance status, allowing it to retain conditional access even after a compliance change (e.g., BitLocker requirement added) that it cannot meet.",
            Tags = ["intune", "mdm", "compliance", "check-in", "cadence", "remote"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "MDM compliance check-in enforced at least daily; compliance status reflects current policy even for remote workers.",
            ApplyOps = [RegOp.SetDword(Key, "EnforceComplianceCheckCadenceHours", 24)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceComplianceCheckCadenceHours")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceComplianceCheckCadenceHours", 24)],
        },
        new TweakDef
        {
            Id = "intuneev-require-signed-mdm-commands",
            Label = "Intune: Require Cryptographic Signing of All OMA-DM Commands",
            Category = "Intune Device Event Policy",
            Description = "Sets RequireSignedMDMCommands=1 in the MDM data collection policy. Requires that all OMA-DM commands received from the MDM server are cryptographically signed with the Intune service certificate, and rejects unsigned or incorrectly signed OMA-DM payloads, protecting against rogue MDM server injection. " +
                "OMA-DM is the protocol that carries MDM policy commands from the Intune service to the client. Without command signing enforcement, an attacker who achieves a man-in-the-middle position between the endpoint and the Intune service endpoint could inject arbitrary OMA-DM commands (which translate to registry writes, file downloads, and application installs). Requiring signed commands ensures only the authentic Intune service can deliver policy changes.",
            Tags = ["intune", "mdm", "oma-dm", "signing", "mitm", "command-integrity"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Unsigned OMA-DM commands rejected; MDM policy injection via man-in-the-middle blocked.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSignedMDMCommands", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedMDMCommands")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSignedMDMCommands", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-enable-mdm-config-lockdown",
            Label = "Intune: Enable MDM Config Lock to Re-Enforce Settings Changed Out-of-Band",
            Category = "Intune Device Event Policy",
            Description = "Sets EnableMDMConfigLockdown=1 in the MDM data collection policy. Enables the MDM config lock feature, which continuously monitors settings delivered by Intune compliance or configuration profiles and automatically reverts any changes made to those settings through other means (GPO that conflicts with MDM, manual registry edits, third-party tools). " +
                "MDM config lock prevents MDM-delivered settings from being overridden by competing configuration mechanisms. Without config lock, other Group Policy settings delivered via domain join, local GPOs applied by elevated users, or malicious registry edits can override MDM-delivered security baselines. Config lock creates a continuous enforcement loop that re-applies MDM settings whenever they deviate from the expected values, functioning as a security posture self-healing mechanism.",
            Tags = ["intune", "mdm", "config-lock", "drift", "enforcement", "security-baseline"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "MDM config lockdown active; out-of-band registry/GPO changes that conflict with Intune profiles are automatically reverted.",
            ApplyOps = [RegOp.SetDword(Key, "EnableMDMConfigLockdown", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMDMConfigLockdown")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMDMConfigLockdown", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-disable-mdm-agent-auto-update-from-store",
            Label = "Intune: Block MDM Agent Auto-Update from Microsoft Store",
            Category = "Intune Device Event Policy",
            Description = "Sets DisableMDMAgentAutoUpdate=1 in the MDM data collection policy. Prevents the Intune Company Portal and MDM management agent components from auto-updating from the Microsoft Store, requiring IT to control agent updates through managed deployment paths (MDM app profiles, SCCM, or Intune Win32 app) rather than consumer Store delivery. " +
                "MDM agent updates delivered through the Microsoft Store follow the Store's release schedule independently of IT's testing and validation calendar. A Store-delivered agent update may change MDM enrollment flow, compliance evaluation behaviour, or Company Portal UI in ways that weren't tested by IT's change management process. Blocking auto-update from Store and using managed deployment paths ensures IT controls when MDM agent updates reach production endpoints.",
            Tags = ["intune", "mdm", "company-portal", "auto-update", "store", "change-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "MDM agent Store updates blocked; Intune Company Portal and agent updates require IT-managed deployment.",
            ApplyOps = [RegOp.SetDword(Key, "DisableMDMAgentAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMDMAgentAutoUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMDMAgentAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "intuneev-enable-remote-wipe-audit-log",
            Label = "Intune: Enable Audit Logging for Remote Wipe Commands Received from MDM Server",
            Category = "Intune Device Event Policy",
            Description = "Sets EnableRemoteWipeAuditLog=1 in the MDM data collection policy. Generates a Security event log entry (and application event log warning) the moment the Intune service delivers a remote wipe command to the client, recording the timestamp and wipe type (quick wipe vs full wipe) before the wipe execution begins. " +
                "Remote wipe is the nuclear security action available through MDM — it erases all device data. Without an audit log entry before execution, there is no on-device evidence that a wipe was initiated via MDM (distinguishable from a local factory reset). In scenarios where a remote wipe was accidental (wrong device targeted in the Intune console) or unauthorised (admin credential compromise), forensic investigation of what happened requires an event record. A pre-wipe audit log event can be captured by a SIEM before the device is erased.",
            Tags = ["intune", "mdm", "remote-wipe", "audit-log", "forensics", "siem"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Security event written when remote wipe command received; SIEM captures wipe initiation before erasure completes.",
            ApplyOps = [RegOp.SetDword(Key, "EnableRemoteWipeAuditLog", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteWipeAuditLog")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRemoteWipeAuditLog", 1)],
        },
    ];
}
