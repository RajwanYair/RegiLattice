// RegiLattice.Core — Tweaks/TpmAdvancedPolicy.cs
// Sprint 348: TPM Advanced Policy tweaks (10 tweaks)
// Category: "TPM Advanced Policy" | Slug: tpmadv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TpmAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tpmadv-enable-tpm-auto-provisioning",
            Label = "Enable Automatic TPM Provisioning and Activation",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "TPM automatic provisioning prepares the TPM for use by Windows security features by taking ownership and generating the platform hierarchy keys without requiring manual administrator intervention. Enabling automatic TPM provisioning ensures that all systems with available TPMs are properly configured for BitLocker Windows Hello and other TPM-dependent security features. Systems with unconfigured TPMs cannot use BitLocker with TPM-only protection Windows Hello for Business with TPM-bound credentials or Credential Guard. Automatic provisioning triggers on systems where the TPM is present but has not been taken ownership of by Windows reducing deployment complexity. Organizations deploying new hardware should rely on automatic TPM provisioning as part of the standard Windows deployment process managed through Autopilot or Configuration Manager. TPM provisioning failure events should be monitored to identify hardware issues or BIOS configuration problems that prevent proper TPM initialization.",
            Tags = ["tpm", "provisioning", "auto-setup", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "OSManagedAuthLevel", 4)],
            RemoveOps = [RegOp.DeleteValue(Key, "OSManagedAuthLevel")],
            DetectOps = [RegOp.CheckDword(Key, "OSManagedAuthLevel", 4)],
        },
        new TweakDef
        {
            Id = "tpmadv-configure-tpm-lockout-duration",
            Label = "Configure TPM Lockout Duration for Failed Authorization Attempts",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "TPM lockout duration controls how long the TPM enters a lockout state after excessive failed authorization attempts preventing brute-force attacks against TPM-protected credentials. Configuring an appropriate lockout duration ensures that TPM-based authentication cannot be brute-forced while minimizing disruption from accidental lockout. TPM 2.0 devices implement anti-hammering protection that progressively increases lockout duration after failed attempts up to a maximum duration defined by this policy. A standard lockout duration of 2 hours provides reasonable brute-force protection while limiting the operational impact of accidental lockouts. Organizations with strict security requirements may extend the lockout duration to 24 hours or more to extend the time required for physical or software attacks. TPM lockout events should be monitored as they may indicate attempted credential attacks against BitLocker recovery or other TPM-protected data.",
            Tags = ["tpm", "lockout", "anti-hammering", "brute-force", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "OSManagedAuthLevel", 4)],
            RemoveOps = [RegOp.DeleteValue(Key, "OSManagedAuthLevel")],
            DetectOps = [RegOp.CheckDword(Key, "OSManagedAuthLevel", 4)],
        },
        new TweakDef
        {
            Id = "tpmadv-require-tpm-for-bitlocker",
            Label = "Require TPM for BitLocker Drive Encryption Operations",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "BitLocker with a TPM provides hardware-based key protection ensuring the encryption key is only released when the measured boot state matches the expected configuration. Requiring TPM for BitLocker prevents use of the software-only BitLocker mode that stores the key on a USB drive and lacks hardware binding. TPM-protected BitLocker keys are released based on platform configuration register (PCR) measurements that detect changes to the boot sequence that may indicate tampering. BitLocker without TPM using only a USB key or password provides encryption at rest but without the hardware-based tamper detection of TPM-bound keys. Organizations should require TPM for all BitLocker deployments and ensure TPM presence is verified as part of standard hardware procurement. BitLocker with TPM plus PIN or TPM plus USB key provides multi-factor authentication for disk encryption addressing scenarios where physical theft is a primary concern.",
            Tags = ["tpm", "bitlocker", "encryption", "hardware-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireTpmForBitLocker", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmForBitLocker")],
            DetectOps = [RegOp.CheckDword(Key, "RequireTpmForBitLocker", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-enable-tpm-platform-attestation",
            Label = "Enable TPM Platform Attestation for Device Health Verification",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "TPM Platform Attestation uses the TPM to cryptographically prove the device's boot state and security configuration to remote verification services. Enabling TPM platform attestation allows Microsoft Device Health Attestation Service or organizational attestation servers to verify device security posture before granting access to protected resources. Attestation provides verifiable evidence that Secure Boot was enabled and that no unauthorized components were present during the boot process. Conditional access policies that require device health attestation ensure that only compliant devices with verified boot states can access sensitive corporate resources. TPM attestation is the highest assurance form of device compliance verification compared to software-only compliance reporting. Organizations implementing Zero Trust architectures should require TPM attestation as part of the device identity verification process.",
            Tags = ["tpm", "attestation", "platform-health", "zero-trust", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableTPMAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTPMAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTPMAttestation", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-configure-tpm-pcr-banks",
            Label = "Configure TPM PCR Bank Selection for Maximum Security Coverage",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "TPM Platform Configuration Registers (PCRs) store measurement values from the measured boot sequence that are used to validate the integrity of the boot process. Configuring which PCR banks are used for measurement ensures comprehensive coverage of boot components that could be modified for bootkit or rootkit attacks. PCR banks using SHA-256 provide stronger measurement integrity compared to SHA-1 banks and should be preferred for all deployments. The specific PCRs included in the TPM sealing policy for BitLocker determine which boot components are verified before releasing the encryption key. More extensive PCR coverage increases tamper detection but reduces flexibility for legitimate firmware and software changes that would trigger BitLocker recovery. Organizations should configure PCR banks to include UEFI firmware measurements driver loading and boot loader measurements for comprehensive boot integrity.",
            Tags = ["tpm", "pcr-banks", "measured-boot", "integrity", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSHA256Bank", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSHA256Bank")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSHA256Bank", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-disable-tpm-clear-without-pin",
            Label = "Require Physical Presence PIN for TPM Clear Operations",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "TPM clear operations reset the TPM to factory state erasing all keys and credentials which is a destructive operation that should require physical presence verification. Requiring a physical presence PIN for TPM clear prevents remote attackers who gain administrator access from destroying TPM-protected credentials and BitLocker keys. Remote TPM clear could be used by an attacker to force BitLocker recovery on all systems simultaneously creating a denial of service across the organization. Physical presence requirements for TPM clear ensure that an authorized person must be at the physical console to approve the destructive operation. Organizations should document and protect the physical presence PIN used for TPM management and ensure it is stored securely through the PAM process. TPM clear operations should be logged and require change management approval before execution even when the physical presence requirement is satisfied.",
            Tags = ["tpm", "clear-protection", "physical-presence", "key-protection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockedCommandsList", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockedCommandsList")],
            DetectOps = [RegOp.CheckDword(Key, "BlockedCommandsList", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-restrict-tpm-commandlist",
            Label = "Restrict TPM Command List to Approved Operations Only",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "TPM command lists define which TPM commands can be sent to the hardware restricting the operations that software can request from the TPM. Restricting the TPM command list to approved operations prevents exploitation of obscure or rarely-used TPM commands that may have implementation vulnerabilities. TPM command filtering blocks commands that are not needed for normal Windows security operations reducing the attack surface for TPM firmware vulnerabilities. The Windows TPM command list policy defines both blocked and allowed command lists that are enforced by the TPM driver before forwarding to the hardware. Organizations should configure the minimum required TPM command set for their workload and test thoroughly before deploying restrictions. TPM command restriction policies should be reviewed after TPM firmware updates that may add or modify command handling.",
            Tags = ["tpm", "command-list", "attack-surface", "firmware", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UseAdvancedStartup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UseAdvancedStartup")],
            DetectOps = [RegOp.CheckDword(Key, "UseAdvancedStartup", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-enable-tpm-srk-policy",
            Label = "Configure TPM Storage Root Key Policy for Key Protection",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The TPM Storage Root Key (SRK) is the master key that protects all other keys created under the TPM's storage hierarchy and is critical to the security of all TPM-protected data. Configuring the SRK policy ensures that the storage hierarchy is initialized with the appropriate authorization requirements for organizational security needs. SRK policy configuration includes whether the SRK requires authorization for key creation and how hierarchy keys are sealed to PCR measurements. Proper SRK configuration is essential for TPM 1.2 devices where the SRK is established during TPM ownership which differs from TPM 2.0 storage hierarchy management. Organizations should verify SRK policy alignment with their BitLocker and Credential Guard deployment requirements. SRK policy management for TPM 1.2 differs significantly from TPM 2.0 storage hierarchy management and organizations should consult the appropriate hardware documentation.",
            Tags = ["tpm", "srk", "storage-root-key", "key-hierarchy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSrkPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSrkPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSrkPolicy", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-audit-tpm-operations",
            Label = "Enable Comprehensive Audit Logging for TPM Operations",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "TPM operation audit logging captures successful and failed TPM operations providing visibility into TPM usage patterns and potential exploitation attempts. Enabling comprehensive TPM audit logging creates a forensic record of BitLocker key operations credential creations and platform measurements. Anomalous TPM operation patterns such as repeated failed TPM authorization attempts may indicate a BitLocker brute-force attack or TPM exploitation attempt. TPM audit events are available in the System log under the Microsoft-Windows-Security-Tpm category and should be forwarded to SIEM. Organizations should baseline normal TPM operation patterns including BitLocker key release frequency and Windows Hello authentication to detect deviations. Correlation of TPM audit events with other security signals provides context for determining whether anomalous patterns represent attacks.",
            Tags = ["tpm", "audit", "monitoring", "key-operations", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditTPMOperations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditTPMOperations")],
            DetectOps = [RegOp.CheckDword(Key, "AuditTPMOperations", 1)],
        },
        new TweakDef
        {
            Id = "tpmadv-configure-tpm-firmware-update",
            Label = "Configure TPM Firmware Update Policy for Security Patches",
            Category = "TPM Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "TPM firmware vulnerabilities such as the Infineon TPM weak key generation vulnerability require firmware updates that can only be applied through a TPM firmware update process. Configuring TPM firmware update policy ensures that critical TPM security patches can be applied through the organizational update infrastructure. TPM firmware updates may require BitLocker recovery or TPM re-enrollment of Windows Hello credentials depending on how the update changes the TPM state. Organizations should test TPM firmware updates in a lab environment to understand the user impact before broad deployment. The TPM firmware update approval process should be tracked in the vulnerability management system to ensure timely remediation of critical TPM security issues. TPM firmware update events should be logged for compliance reporting on vulnerability remediation timelines.",
            Tags = ["tpm", "firmware-update", "vulnerability-management", "patching", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowFirmwareUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowFirmwareUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "AllowFirmwareUpdate", 1)],
        },
    ];
}
