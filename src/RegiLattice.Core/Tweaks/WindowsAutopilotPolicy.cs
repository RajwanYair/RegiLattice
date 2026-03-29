// RegiLattice.Core — Tweaks/WindowsAutopilotPolicy.cs
// Windows Autopilot self-deploying provisioning and OOBE security Group Policy controls (Sprint 617).
// Category: "Windows Autopilot Policy" | Slug: wpautopilot
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Autopilot

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsAutopilotPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Autopilot";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wpautopilot-block-oobe-cortana",
            Label = "Autopilot: Suppress Cortana Voice Assistant During OOBE Provisioning",
            Category = "Windows Autopilot Policy",
            Description = "Sets DisableCortanaInOOBE=1 in Autopilot policy. Prevents Cortana's voice-guided OOBE assistant from launching during the Windows Out-Of-Box Experience on Autopilot-provisioned devices, eliminating unexpected voice output and microphone access during unattended provisioning. " +
                "During self-deploying Autopilot provisioning, the device may go through OOBE phases unattended. Cortana's voice interface launching during an unattended provisioning session can trigger unexpected audio output (speakers active) and request microphone access, which is unnecessary and potentially alarming in secure staging environments. Suppressing Cortana during OOBE ensures silent, predictable provisioning.",
            Tags = ["autopilot", "oobe", "cortana", "provisioning", "silent"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cortana suppressed during OOBE; Autopilot provisioning completes silently without voice prompts.",
            ApplyOps = [RegOp.SetDword(Key, "DisableCortanaInOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCortanaInOOBE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCortanaInOOBE", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-require-tpm-attestation",
            Label = "Autopilot: Require TPM Attestation Before Autopilot Pre-Provisioning Completes",
            Category = "Windows Autopilot Policy",
            Description = "Sets RequireTPMAttestation=1 in Autopilot policy. Requires that the device's TPM chip successfully completes attestation with the Microsoft Attestation Service before Autopilot White Glove pre-provisioning is allowed to complete, ensuring only machines with healthy TPM chips receive the provisioning credential blob. " +
                "Autopilot White Glove pre-provisioning downloads and installs applications and policies during the Technician Phase. If TPM attestation is not required, a device with a non-functional or tampered TPM can still be fully provisioned and shipped to an end user with an enterprise credential blob. Requiring TPM attestation ensures only hardware with a verified, healthy TPM is provisioned, supporting BitLocker and Windows Hello for Business.",
            Tags = ["autopilot", "tpm", "attestation", "white-glove", "hardware-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "TPM attestation required; Autopilot White Glove fails for devices with non-functional or tampered TPM.",
            ApplyOps = [RegOp.SetDword(Key, "RequireTPMAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireTPMAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "RequireTPMAttestation", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-block-language-selection-in-oobe",
            Label = "Autopilot: Skip Language and Region Selection in OOBE (Silent Provisioning)",
            Category = "Windows Autopilot Policy",
            Description = "Sets SkipLanguageAndRegion=1 in Autopilot policy. Skips the language selection, keyboard layout, and region selection screens during OOBE, using the locale settings pre-configured in the Autopilot deployment profile instead of prompting the user or technician during provisioning. " +
                "Self-deploying Autopilot profiles target unattended provisioning. Any OOBE screen that blocks at a user input prompt (language, region) halts the provisioning workflow until answered. In staging environments where devices are provisioned in bulk on racks, unexpected OOBE prompts that require per-device interaction break the automation, requiring manual intervention on each device.",
            Tags = ["autopilot", "oobe", "language", "silent", "unattended"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Language/region OOBE screens skipped; Autopilot provisioning uses profile locale settings without prompt.",
            ApplyOps = [RegOp.SetDword(Key, "SkipLanguageAndRegion", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SkipLanguageAndRegion")],
            DetectOps = [RegOp.CheckDword(Key, "SkipLanguageAndRegion", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-disable-privacy-settings-screen",
            Label = "Autopilot: Skip Privacy Settings Screen in OOBE",
            Category = "Windows Autopilot Policy",
            Description = "Sets DisablePrivacySettingsInOOBE=1 in Autopilot policy. Suppresses the privacy settings configuration screen that appears during OOBE, where Windows presents toggles for diagnostic data, location, speech recognition, and ink/typing personalisation, using enterprise policy defaults instead. " +
                "The OOBE privacy settings screen presents users and technicians with a series of toggle choices that may override enterprise Group Policy settings if the user makes incorrect selections during provisioning. By skipping this screen and applying privacy settings via Group Policy or Intune configuration profiles, the enterprise ensures the device always meets its defined privacy configuration baseline from first boot.",
            Tags = ["autopilot", "oobe", "privacy", "provisioning", "baseline"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "OOBE privacy settings screen skipped; enterprise policy controls privacy toggles rather than OOBE user selection.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePrivacySettingsInOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacySettingsInOOBE")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrivacySettingsInOOBE", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-enable-secure-diagnostics-upload",
            Label = "Autopilot: Enable Secure Diagnostic Log Upload on Provisioning Failure",
            Category = "Windows Autopilot Policy",
            Description = "Sets EnableDiagnosticsUploadOnFailure=1 in Autopilot policy. Enables automatic upload of diagnostic logs to the Microsoft Intune service when Autopilot provisioning fails, allowing IT admins to review failure details in the Intune admin center without physical access to the device. " +
                "Autopilot provisioning failures in the field (enrolled device failing to complete provisioning at an employee's desk) are difficult to diagnose without the detailed log files stored on the device. Without automatic log upload, IT must either collect logs manually (requiring physical access or remote PowerShell) or rely on the user to capture and submit logs. Enabling automatic upload on failure provides actionable failure diagnostics in the admin portal.",
            Tags = ["autopilot", "diagnostics", "failure", "logging", "intune"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Autopilot failure logs uploaded automatically to Intune; no physical access needed for provisioning failure diagnostics.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDiagnosticsUploadOnFailure", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDiagnosticsUploadOnFailure")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDiagnosticsUploadOnFailure", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-block-manual-hardware-hash-upload",
            Label = "Autopilot: Block Manual Hardware Hash Upload by Non-Administrators",
            Category = "Windows Autopilot Policy",
            Description = "Sets DisableManualHardwareHashUpload=1 in Autopilot policy. Prevents standard users from manually running scripts or PowerShell commands that collect the device's hardware hash and upload it to the Autopilot service, restricting hardware hash registration to OEM upload and IT admin-initiated processes. " +
                "Hardware hash registration is the authoritative step that associates a physical device with an Autopilot deployment profile. If standard users can run scripts to upload hardware hashes of arbitrary devices (including virtual machines running on personal hardware), they may register personal devices into the enterprise Autopilot service, bootstrapping them with enterprise policies, certificates, and credentials.",
            Tags = ["autopilot", "hardware-hash", "registration", "unauthorised", "admin"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Manual hardware hash upload blocked for standard users; only OEM/IT admin can register devices.",
            ApplyOps = [RegOp.SetDword(Key, "DisableManualHardwareHashUpload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableManualHardwareHashUpload")],
            DetectOps = [RegOp.CheckDword(Key, "DisableManualHardwareHashUpload", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-enable-provisioning-audit-log",
            Label = "Autopilot: Enable Security Audit Log for Autopilot Provisioning Events",
            Category = "Windows Autopilot Policy",
            Description = "Sets EnableProvisioningAuditLog=1 in Autopilot policy. Causes a Security event log entry to be written at each stage of the Autopilot provisioning workflow (device registration, Entra ID join, MDM enrollment, application installation) including the result and any error codes. " +
                "Without provisioning audit logging, there is no on-device Security event record of what happened during Autopilot provisioning — only the results visible in the Intune admin portal. Having on-device event log entries for each provisioning stage enables post-incident forensics if a device's provisioning state is questioned (e.g., whether a specific application or configuration was applied correctly during the initial setup).",
            Tags = ["autopilot", "audit", "provisioning", "event-log", "forensics"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Security event log entries written at each Autopilot provisioning stage; on-device provisioning history available.",
            ApplyOps = [RegOp.SetDword(Key, "EnableProvisioningAuditLog", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableProvisioningAuditLog")],
            DetectOps = [RegOp.CheckDword(Key, "EnableProvisioningAuditLog", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-require-enrolled-device-for-provisioning",
            Label = "Autopilot: Require Device Pre-Registration Before OOBE Autopilot Profile Download",
            Category = "Windows Autopilot Policy",
            Description = "Sets RequirePreRegistration=1 in Autopilot policy. Enforces that the device must be pre-registered in the Autopilot service (via hardware hash) before the OOBE Autopilot profile download proceeds, blocking provisioning of devices that have not been explicitly registered by IT. " +
                "Without pre-registration enforcement, an unregistered device going through OOBE on the same network as a registered device might accidentally receive an Autopilot profile due to subnet-based profile assignment misconfiguration. Requiring explicit pre-registration ensures that Autopilot profiles are only applied to known, IT-registered hardware and not to devices that are accidentally discoverable.",
            Tags = ["autopilot", "pre-registration", "oobe", "hardware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Autopilot provisioning blocked for non-registered hardware; only pre-enrolled devices receive provisioning profiles.",
            ApplyOps = [RegOp.SetDword(Key, "RequirePreRegistration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePreRegistration")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePreRegistration", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-block-oobe-skip-button",
            Label = "Autopilot: Remove OOBE Skip/Cancel Button to Prevent Provisioning Abandonment",
            Category = "Windows Autopilot Policy",
            Description = "Sets DisableSkipButtonInOOBE=1 in Autopilot policy. Removes the 'Skip' and 'Cancel' buttons from Autopilot OOBE screens that would allow a user or technician to abort the provisioning workflow before it completes, ensuring devices are always fully provisioned before being usable. " +
                "OOBE Skip buttons allow a technician or user to abandon Autopilot provisioning mid-way through, leaving the device in a partially configured state with some apps installed and others not, MDM enrollment incomplete, and security baselines potentially unapplied. A partially provisioned device may appear to work normally while critical security configurations are absent.",
            Tags = ["autopilot", "oobe", "skip", "provisioning", "incomplete"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "OOBE skip/cancel buttons removed; Autopilot provisioning must complete before device becomes usable.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSkipButtonInOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSkipButtonInOOBE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSkipButtonInOOBE", 1)],
        },
        new TweakDef
        {
            Id = "wpautopilot-set-provisioning-timeout-90min",
            Label = "Autopilot: Set Autopilot Enrollment Status Page Timeout to 90 Minutes",
            Category = "Windows Autopilot Policy",
            Description = "Sets EnrollmentStatusPageTimeout=90 in Autopilot policy. Sets the Autopilot Enrollment Status Page (ESP) timeout — the maximum time the ESP will wait for app and policy installation to complete before triggering an error — to 90 minutes. " +
                "The default ESP timeout is 60 minutes. In enterprise environments with large required application sets or slow network segments (branch office with limited bandwidth), the app installation phase can exceed 60 minutes especially for large apps delivered via Intune Win32 app deployment (LOB apps with 500 MB+ installers). An ESP timeout before provisioning completes leaves the device in an error state, triggering a factory reset. A 90-minute timeout accommodates larger app sets.",
            Tags = ["autopilot", "esp", "timeout", "provisioning", "apps"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ESP timeout extended to 90 minutes; large application packages have more time to complete installation during provisioning.",
            ApplyOps = [RegOp.SetDword(Key, "EnrollmentStatusPageTimeout", 90)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnrollmentStatusPageTimeout")],
            DetectOps = [RegOp.CheckDword(Key, "EnrollmentStatusPageTimeout", 90)],
        },
    ];
}
