// RegiLattice.Core — Tweaks/DeviceCompliancePolicy.cs
// Device Compliance Policy — Sprint 567.
// Configures Group Policy for device compliance assessment: security
// health checks, compliance state reporting, conditional access
// prerequisites, and non-compliant device isolation controls.
// Category: "Device Compliance Policy" | Slug: devcpl
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\HealthCenter

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceCompliancePolicy
{
    private const string DhaKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

    private const string HcKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthCenter";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "devcpl-enable-health-attestation",
                Label = "Device Compliance: Enable Device Health Attestation",
                Category = "Device Compliance Policy",
                Description =
                    "Sets EnableHealthAttestation=1 in DeviceHealthAttestation policy. Enables the Windows Device Health Attestation (DHA) service which uses the device's TPM to cryptographically attest its boot sequence. The DHA service generates a health certificate that can be consumed by MDM providers (Intune, SCCM) and conditional access systems to verify that the device booted without tampering: Secure Boot was enabled, BitLocker is active, the boot path was not modified, and no ELAM-detected malware was present. Without DHA, conditional access can only rely on OS-reported state — which malware can spoof.",
                Tags = ["dha", "health-attestation", "tpm", "conditional-access", "boot-integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Device boot integrity is cryptographically attested using the TPM. Requires TPM 2.0. Health certificates are generated and periodically sent to the configured DHA server. Enables hardware-backed conditional access decisions.",
                ApplyOps = [RegOp.SetDword(DhaKey, "EnableHealthAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(DhaKey, "EnableHealthAttestation")],
                DetectOps = [RegOp.CheckDword(DhaKey, "EnableHealthAttestation", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-require-bitlocker-for-compliance",
                Label = "Device Compliance: Require BitLocker Encryption for Compliance",
                Category = "Device Compliance Policy",
                Description =
                    "Sets RequireBitLockerForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if BitLocker Drive Encryption is not enabled on the system drive. Compliance status is reported to MDM (Intune/SCCM) and can trigger conditional access policies that block the device from connecting to corporate resources until BitLocker is enabled. Data loss from stolen or lost unencrypted laptops is one of the most common sources of data breaches. Requiring BitLocker for compliance ensures all mobile devices connecting to corporate resources are encrypted.",
                Tags = ["compliance", "bitlocker", "encryption", "conditional-access", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Devices without BitLocker on the system drive report as non-compliant. Non-compliant devices may be blocked from corporate resources via conditional access. Requires MDM enrolment and conditional access policies to enforce the compliance gate.",
                ApplyOps = [RegOp.SetDword(HcKey, "RequireBitLockerForCompliance", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "RequireBitLockerForCompliance")],
                DetectOps = [RegOp.CheckDword(HcKey, "RequireBitLockerForCompliance", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-require-antivirus-for-compliance",
                Label = "Device Compliance: Require Active Antivirus for Compliance",
                Category = "Device Compliance Policy",
                Description =
                    "Sets RequireAntivirusForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if a registered and up-to-date antivirus product is not detected by the Security Center. Real-time protection must be active and signatures cannot be critically outdated. Devices that have disabled antivirus, have expired protection subscriptions, or have antivirus that is consuming no CPU (indicative of process termination by malware) are flagged. Security Center status is checked periodically and on every MDM compliance check cycle.",
                Tags = ["compliance", "antivirus", "security-center", "real-time-protection", "endpoint"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Devices without active, up-to-date antivirus report as non-compliant. Devices with disabled or expired AV may lose access to corporate resources. Requires MDM and conditional access to enforce. Windows Defender Antivirus or any ELAM-registered product satisfies the requirement.",
                ApplyOps = [RegOp.SetDword(HcKey, "RequireAntivirusForCompliance", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "RequireAntivirusForCompliance")],
                DetectOps = [RegOp.CheckDword(HcKey, "RequireAntivirusForCompliance", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-set-compliance-check-interval-4h",
                Label = "Device Compliance: Set Compliance Check Interval to 4 Hours",
                Category = "Device Compliance Policy",
                Description =
                    "Sets ComplianceCheckIntervalHours=4 in HealthCenter policy. Sets the interval at which Windows re-evaluates device compliance state and sends the current status to the MDM provider. A default compliance check interval that is too long (24+ hours) means a device that becomes non-compliant (user disables BitLocker, AV signs expire, firewall turned off) continues to access corporate resources for up to a day before its compliance status is updated. 4 hours ensures compliance violations are detected and reflected in conditional access within the business day after they occur.",
                Tags = ["compliance", "check-interval", "mdm", "detection", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Compliance state is evaluated every 4 hours. A device that becomes non-compliant is detected within 4 hours. Slightly higher MDM service check-in frequency — negligible network overhead.",
                ApplyOps = [RegOp.SetDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceCheckIntervalHours")],
                DetectOps = [RegOp.CheckDword(HcKey, "ComplianceCheckIntervalHours", 4)],
            },
            new TweakDef
            {
                Id = "devcpl-require-secure-boot-for-compliance",
                Label = "Device Compliance: Require Secure Boot for Compliance",
                Category = "Device Compliance Policy",
                Description =
                    "Sets RequireSecureBootForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if UEFI Secure Boot is not enabled. Secure Boot prevents bootkit malware and rootkits from replacing the boot path with untrusted code — without Secure Boot, an attacker with brief physical access can boot from a USB drive to bypass Windows authentication or install a persistent bootkit. Devices with Secure Boot disabled cannot be trusted to run an uncompromised OS. This check complements DHA attestation with a policy-layer enforcement.",
                Tags = ["compliance", "secure-boot", "uefi", "bootkit", "physical-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Devices without Secure Boot enabled report as non-compliant. Very old hardware (pre-2012) may not support Secure Boot. Devices that were deliberately configured without Secure Boot for BIOS compatibility reasons must be re-evaluated.",
                ApplyOps = [RegOp.SetDword(HcKey, "RequireSecureBootForCompliance", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "RequireSecureBootForCompliance")],
                DetectOps = [RegOp.CheckDword(HcKey, "RequireSecureBootForCompliance", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-enable-compliance-grace-period-7days",
                Label = "Device Compliance: Enable 7-Day Grace Period for Non-Compliant Devices",
                Category = "Device Compliance Policy",
                Description =
                    "Sets ComplianceGracePeriodDays=7 in HealthCenter policy. Grants newly enrolled devices or devices that first become non-compliant a 7-day grace period before conditional access blocks are enforced. Without a grace period, a device that enrolls in MDM but has not yet completed all compliance remediation (BitLocker encrypting, definitions updating) is immediately blocked from corporate resources — creating a chicken-and-egg problem. The grace period allows IT to remediate the device before it loses access. After 7 days without remediation, access restrictions are enforced.",
                Tags = ["compliance", "grace-period", "enrolment", "remediation", "mdm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Non-compliant devices have 7 days to reach compliance before access restrictions are applied. Provides IT time for remediation without disrupting new enrolments. After 7 days, non-compliant devices are subject to conditional access blocks.",
                ApplyOps = [RegOp.SetDword(HcKey, "ComplianceGracePeriodDays", 7)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceGracePeriodDays")],
                DetectOps = [RegOp.CheckDword(HcKey, "ComplianceGracePeriodDays", 7)],
            },
            new TweakDef
            {
                Id = "devcpl-require-minimum-os-build",
                Label = "Device Compliance: Require Minimum OS Build for Compliance",
                Category = "Device Compliance Policy",
                Description =
                    "Sets RequireMinimumOsBuild=1 in HealthCenter policy. Enables minimum OS build checking as a compliance criterion. When enabled, devices running OS builds older than the configured minimum (set separately as MinimumBuildNumber) report as non-compliant. This policy ensures that devices running versions of Windows that are out of Microsoft's support cycle (no security patches) or that have known unpatched critical vulnerabilities are flagged before they access corporate resources. Combined with Windows Update policies, this creates an enforced minimum security baseline.",
                Tags = ["compliance", "os-build", "patch-level", "security-baseline", "outdated-os"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Devices below the minimum OS build report as non-compliant. Requires configuring MinimumBuildNumber separately. Devices on unsupported or unpatched OS build are blocked pending upgrade. Coordinate with Windows Update deadline policies.",
                ApplyOps = [RegOp.SetDword(HcKey, "RequireMinimumOsBuild", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "RequireMinimumOsBuild")],
                DetectOps = [RegOp.CheckDword(HcKey, "RequireMinimumOsBuild", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-require-firewall-for-compliance",
                Label = "Device Compliance: Require Windows Firewall Active for Compliance",
                Category = "Device Compliance Policy",
                Description =
                    "Sets RequireFirewallForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if Windows Defender Firewall (or a registered third-party firewall) is not active on all network profiles (domain, private, public). The Windows Firewall is a critical network-based attack prevention control. Users may disable the firewall when troubleshooting connection issues and forget to re-enable it. A device with no host firewall on a public network is exposed to direct network attacks. This compliance check ensures firewalls stay active.",
                Tags = ["compliance", "firewall", "network-protection", "security-center", "perimeter"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Devices with disabled Windows Defender Firewall or no registered firewall are non-compliant. Third-party firewalls registered with Security Center satisfy the requirement. Devices that turned off the firewall for temporary diagnostics and forgot to restore will be flagged.",
                ApplyOps = [RegOp.SetDword(HcKey, "RequireFirewallForCompliance", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "RequireFirewallForCompliance")],
                DetectOps = [RegOp.CheckDword(HcKey, "RequireFirewallForCompliance", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-block-noncompliant-resource-access",
                Label = "Device Compliance: Block Non-Compliant Devices from Joining AD Resources",
                Category = "Device Compliance Policy",
                Description =
                    "Sets BlockNonCompliantNetworkAccess=1 in HealthCenter policy. Enables a local enforcement hook that checks compliance state before allowing the device to connect to protected network resources. When this is enabled and the device is marked non-compliant by the health centre, outbound connections to domain-classified resources can be blocked at the Windows Filtering Platform (WFP) layer. This provides local enforcement independent of whether external conditional access (AAD, MFA, proxy) is in place — useful as defence-in-depth for environments where some legacy resources lack conditional access support.",
                Tags = ["compliance", "network-access", "block", "conditional-access", "wfp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Non-compliant devices are blocked from accessing domain network resources at the WFP layer. This is a local enforcement on the device itself — not a network-layer block. A device misidentifying its compliance state may block its own legitimate access. Test thoroughly before broad deployment.",
                ApplyOps = [RegOp.SetDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(HcKey, "BlockNonCompliantNetworkAccess")],
                DetectOps = [RegOp.CheckDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "devcpl-enable-tpm-attestation-logging",
                Label = "Device Compliance: Enable TPM Health Attestation Event Logging",
                Category = "Device Compliance Policy",
                Description =
                    "Sets TpmAttestationLogging=1 in DeviceHealthAttestation policy. Enables event log entries for TPM health attestation operations: TPM measurement capture, health certificate request, health certificate delivery, and health attestation failures. Without attestation logging, diagnosing why a device cannot obtain a health certificate (TPM in reduced functionality mode, endorsement key provisioning failure, attestation service unreachable) is difficult. Log entries enable IT helpdesk to diagnose attestation failures and restore compliance without escalating to infrastructure teams.",
                Tags = ["tpm", "attestation", "logging", "compliance", "diagnostics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "TPM attestation events are logged. Events include certificate request, success, failure, and failure reasons. Negligible disk overhead. Enables rapid helpdesk diagnosis of attestation failures without advanced tooling.",
                ApplyOps = [RegOp.SetDword(DhaKey, "TpmAttestationLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(DhaKey, "TpmAttestationLogging")],
                DetectOps = [RegOp.CheckDword(DhaKey, "TpmAttestationLogging", 1)],
            },
        ];
}
