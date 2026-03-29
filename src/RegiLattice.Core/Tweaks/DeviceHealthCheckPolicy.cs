// RegiLattice.Core — Tweaks/DeviceHealthCheckPolicy.cs
// Device Health Check Policy — Sprint 568.
// Configures Group Policy for Windows Device Health reporting:
// health attestation service URL, TPM evaluation, ELAM driver
// state, Secure Boot measurement, and remediation actions.
// Category: "Device Health Check Policy" | Slug: devhc
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceHealthCheckPolicy
{
    private const string HcKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

    private const string TpmKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "devhc-enable-tpm-health-check",
                Label = "Device Health: Enable TPM Health State Evaluation",
                Category = "Device Health Check Policy",
                Description =
                    "Sets EnableTpmHealthCheck=1 in DeviceHealthAttestation policy. Enables evaluation of TPM health state as part of the device health check. The TPM health check evaluates whether the TPM is enabled, activated, owned, and in a known-good state. TPMs can enter a reduced-functionality mode (e.g., after detecting too many failed PIN attempts or a firmware update that changes the platform configuration registers). A TPM in degraded state cannot attest the boot chain, which can silently cause attestation failures unless the health check actively reports the degraded status.",
                Tags = ["tpm", "health-check", "attestation", "degraded-state", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "TPM health is evaluated on every DHA cycle. Degraded or disabled TPM is reported as a health issue. Enables IT to detect TPM lockout or firmware-changed PCR states before they cause silent attestation failures.",
                ApplyOps = [RegOp.SetDword(HcKey, "EnableTpmHealthCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "EnableTpmHealthCheck")],
                DetectOps = [RegOp.CheckDword(HcKey, "EnableTpmHealthCheck", 1)],
            },
            new TweakDef
            {
                Id = "devhc-require-elam-driver-for-health",
                Label = "Device Health: Require ELAM Driver Active for Healthy State",
                Category = "Device Health Check Policy",
                Description =
                    "Sets RequireElamDriverForHealth=1 in DeviceHealthAttestation policy. Reports the device as unhealthy if an Early Launch Anti-Malware (ELAM) driver is not loaded and active at boot. ELAM drivers are loaded before all other non-Microsoft drivers, giving them the ability to evaluate and classify boot drivers as trusted, untrusted, or unknown before they are allowed to initialize. Without an active ELAM driver, the device's pre-OS environment cannot be assessed for rootkits or boot drivers installed by malware. Windows Defender is an ELAM-registered product and satisfies this requirement.",
                Tags = ["elam", "health", "boot-security", "early-launch", "malware-prevention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Devices without an active ELAM driver are reported as unhealthy by DHA. Windows Defender satisfies this requirement by default. Third-party ELAM-registered AV products also satisfy it. Devices with all AV disabled will fail this check.",
                ApplyOps = [RegOp.SetDword(HcKey, "RequireElamDriverForHealth", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "RequireElamDriverForHealth")],
                DetectOps = [RegOp.CheckDword(HcKey, "RequireElamDriverForHealth", 1)],
            },
            new TweakDef
            {
                Id = "devhc-evaluate-secure-boot-measurement",
                Label = "Device Health: Evaluate Secure Boot PCR Measurement Consistency",
                Category = "Device Health Check Policy",
                Description =
                    "Sets EvaluateSecureBootMeasurement=1 in DeviceHealthAttestation policy. Enables DHA to evaluate the consistency of Secure Boot Platform Configuration Register (PCR) measurements. TPM PCR values record hashes of every component in the boot chain. If the PCR values in the most recent health certificate differ from the baseline (e.g., a firmware update changed a boot component hash), the attestation service can detect this deviation and flag the device. This catches scenarios where a firmware update inadvertently introduced an unsigned component or where a bootkit altered a measured value.",
                Tags = ["secure-boot", "pcr", "measurement", "attestation", "boot-chain"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Secure Boot PCR measurements are included in DHA health certificates. Changes to PCR values (firmware update, boot component change) are detected. Legitimate firmware updates may transiently mark the device as unhealthy until the DHA baseline is updated.",
                ApplyOps = [RegOp.SetDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "EvaluateSecureBootMeasurement")],
                DetectOps = [RegOp.CheckDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
            },
            new TweakDef
            {
                Id = "devhc-set-health-report-retention-30days",
                Label = "Device Health: Retain Health Reports for 30 Days",
                Category = "Device Health Check Policy",
                Description =
                    "Sets HealthReportRetentionDays=30 in DeviceHealthAttestation policy. Sets the number of days that device health reports are retained locally before being purged. Retaining health reports for 30 days provides a rolling audit of the device's health state history. This is useful for post-incident forensics: if a device was compromised, the health report history can show the exact point at which the TPM measurements changed, when Secure Boot was disabled, or when the ELAM driver was removed — correlating health state changes with suspicious events in the device's event log.",
                Tags = ["health-report", "retention", "forensics", "audit", "30-days"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Health report data is retained for 30 days locally. Provides 30 days of health state history for forensic investigation. Small disk footprint — health reports are compact JSON structures, typically a few KB each.",
                ApplyOps = [RegOp.SetDword(HcKey, "HealthReportRetentionDays", 30)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "HealthReportRetentionDays")],
                DetectOps = [RegOp.CheckDword(HcKey, "HealthReportRetentionDays", 30)],
            },
            new TweakDef
            {
                Id = "devhc-disable-health-check-bypass",
                Label = "Device Health: Disable Health Check Bypass for Non-Compliant State",
                Category = "Device Health Check Policy",
                Description =
                    "Sets DisableHealthCheckBypass=1 in DeviceHealthAttestation policy. Prevents clients (including local administrators) from bypassing or suppressing the device health check. Without this policy, a sophisticated user or malware with admin privileges can modify the health state cache or suppress health certificate requests, causing the device to appear healthy to conditional access systems while actually being compromised. Disabling the bypass ensures that the DHA client cannot be locally tampered with to present a false healthy state.",
                Tags = ["health-check", "bypass-prevention", "anti-tampering", "admin-restriction", "integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Health check processes cannot be bypassed or suppressed by local admins. Prevents malware or sophisticated users from spoofing a healthy state to conditional access systems. May complicate debugging of attestation issues in development environments.",
                ApplyOps = [RegOp.SetDword(HcKey, "DisableHealthCheckBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "DisableHealthCheckBypass")],
                DetectOps = [RegOp.CheckDword(HcKey, "DisableHealthCheckBypass", 1)],
            },
            new TweakDef
            {
                Id = "devhc-enable-health-check-auto-remediation",
                Label = "Device Health: Enable Automatic Remediation for Known Health Issues",
                Category = "Device Health Check Policy",
                Description =
                    "Sets EnableHealthAutoRemediation=1 in DeviceHealthAttestation policy. Enables the Device Health agent to attempt automatic remediation for known, non-critical health issues. Remediable issues include re-enabling Windows Defender real-time protection that was automatically disabled by a third-party AV (after that AV was uninstalled), re-enrolling the TPM endorsement key if the certificate expired, or restarting stalled health service processes. Automatic remediation reduces helpdesk tickets for transient compliance failures caused by installation or configuration drift.",
                Tags = ["health", "auto-remediation", "defender", "tpm", "service-restart"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "The health agent automatically resolves known fixable issues (re-enables AV, restarts health services, re-provisions TPM EK). Only remediates known, low-risk issues — it will never force-enable BitLocker or change user-configured settings. Review the list of supported remediations for your OS build.",
                ApplyOps = [RegOp.SetDword(HcKey, "EnableHealthAutoRemediation", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "EnableHealthAutoRemediation")],
                DetectOps = [RegOp.CheckDword(HcKey, "EnableHealthAutoRemediation", 1)],
            },
            new TweakDef
            {
                Id = "devhc-enable-tpm-endorsement-key-validation",
                Label = "Device Health: Enable TPM Endorsement Key Validation",
                Category = "Device Health Check Policy",
                Description =
                    "Sets ValidateTpmEndorsementKey=1 in TPM policy. Enables validation that the TPM's Endorsement Key (EK) certificate is in a known-valid certificate chain rooted at a trusted TPM manufacturer CA. The EK uniquely identifies the physical TPM chip. If EK validation is disabled or skipped, software-based fake TPM implementations (used in virtual machines without vTPM, or malicious virtual TPM drivers) can pass attestation checks. EK validation ensures the attestation chain is anchored to a real hardware chip with a manufacturer-issued certificate.",
                Tags = ["tpm", "endorsement-key", "ek-validation", "hardware-anchor", "attestation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "TPM endorsement key certificates are validated against the manufacturer CA chain. VMs with software vTPM (Hyper-V vTPM, VMware vTPM) have EK certificates signed by Microsoft or the platform vendor and will pass if those CAs are trusted. Non-certified TPMs in custom hardware may fail.",
                ApplyOps = [RegOp.SetDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "ValidateTpmEndorsementKey")],
                DetectOps = [RegOp.CheckDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
            },
            new TweakDef
            {
                Id = "devhc-require-tpm-version-20",
                Label = "Device Health: Require TPM 2.0 for Health Attestation",
                Category = "Device Health Check Policy",
                Description =
                    "Sets RequireTpm20ForHealthAttestation=1 in TPM policy. Marks devices as unable to provide health attestation if they only have a TPM 1.2 chip (as opposed to a TPM 2.0). TPM 1.2 supports SHA-1 algorithm measurement banks. TPM 2.0 adds SHA-256 banks, algorithm agility, and enhanced authorization structures. Modern DHA services require TPM 2.0's enhanced capabilities for accurate, tamper-resistant attestation. TPM 1.2 attestation can be spoofed more easily and lacks support for Credential Guard, Device Guard, and Virtualization-Based Security measurements.",
                Tags = ["tpm", "tpm-20", "attestation", "sha256", "vbs"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Devices with only TPM 1.2 cannot provide health attestation and are treated as unhealthy. Hardware manufactured before 2016 may only have TPM 1.2. Devices with no TPM are already unable to attest. Review device fleet hardware compatibility before enforcing.",
                ApplyOps = [RegOp.SetDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "RequireTpm20ForHealthAttestation")],
                DetectOps = [RegOp.CheckDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
            },
            new TweakDef
            {
                Id = "devhc-enable-code-integrity-measurement",
                Label = "Device Health: Enable Code Integrity State in Health Reports",
                Category = "Device Health Check Policy",
                Description =
                    "Sets IncludeCodeIntegrityInReport=1 in DeviceHealthAttestation policy. Includes Windows Code Integrity (CI) enforcement state in the DHA health certificate. Code Integrity state records whether Windows Defender Application Control (WDAC) or Device Guard is active, whether CI is in audit vs. enforcement mode, and whether User-Mode Code Integrity (UMCI) is enabled in addition to HVCI (Hypervisor-Protected Code Integrity). Including CI state in the attestation report allows conditional access systems to require not just that the device is healthy but that it is actively enforcing application whitelisting.",
                Tags = ["code-integrity", "wdac", "device-guard", "hvci", "attestation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Code Integrity (WDAC/HVCI) state is included in the DHA health certificate. Conditional access can now require that a device have CI enforcement mode active. Devices in CI audit-only mode can be flagged as less secure than those in enforcement mode.",
                ApplyOps = [RegOp.SetDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeCodeIntegrityInReport")],
                DetectOps = [RegOp.CheckDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
            },
            new TweakDef
            {
                Id = "devhc-enable-vbs-state-measurement",
                Label = "Device Health: Include VBS/Credential Guard State in Health Reports",
                Category = "Device Health Check Policy",
                Description =
                    "Sets IncludeVbsStateInReport=1 in DeviceHealthAttestation policy. Includes Virtualization-Based Security (VBS) and Credential Guard state in the DHA health certificate. VBS isolates critical OS components (LSA, UEFI variable writes) inside a secure virtual machine backed by the CPU hypervisor, making credential theft attacks (Pass-the-Hash, Pass-the-Ticket) significantly harder. Including VBS state in attestation reports allows conditional access to enforce that only VBS-enabled devices handle sensitive workloads — for example, requiring VBS for devices that access privileged admin consoles.",
                Tags = ["vbs", "credential-guard", "hypervisor", "attestation", "lsa-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "VBS and Credential Guard state is included in DHA health certificates. Conditional access can require VBS/Credential Guard for high-privilege resource access. Devices without hardware VBS support (no hardware-enforced DEP, SLAT, or IOMMU) cannot satisfy this requirement.",
                ApplyOps = [RegOp.SetDword(HcKey, "IncludeVbsStateInReport", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeVbsStateInReport")],
                DetectOps = [RegOp.CheckDword(HcKey, "IncludeVbsStateInReport", 1)],
            },
        ];
}
