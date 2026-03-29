// RegiLattice.Core — Tweaks/TpmAttestationPolicy.cs
// TPM attestation, TPM lockout, measured boot, and device health verification policy — Sprint 520.
// Category: "TPM Attestation Policy" | Slug: tpmpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\TPM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TpmAttestationPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";
    private const string MbKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";
    private const string HaKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthAttestation";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "tpmpol-require-tpm-for-bitlocker",
            Label        = "Require TPM for All BitLocker Encrypted Volumes",
            Category     = "TPM Attestation Policy",
            Description  = "Requires that all BitLocker encrypted volumes use a TPM protector, preventing BitLocker in password-only mode which does not provide pre-boot hardware attestation or protection against direct memory access attacks.",
            Tags         = ["tpm", "bitlocker", "hardware-security", "attestation", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "TPM required for BitLocker; password-only mode blocked. DMA attacks against BitLocker volumes prevented.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireTPMForBitLocker", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireTPMForBitLocker")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireTPMForBitLocker", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-enable-device-health-attestation",
            Label        = "Enable Device Health Attestation (DHA) Service",
            Category     = "TPM Attestation Policy",
            Description  = "Enables the Windows Device Health Attestation service which uses TPM measurements to produce a signed boot health certificate, allowing MDM policies to verify device security posture before granting access.",
            Tags         = ["tpm", "dha", "health-attestation", "mdm", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Device Health Attestation enabled; TPM boot measurements sent to DHA service for MDM compliance checking.",
            ApplyOps     = [RegOp.SetDword(HaKey, "EnableDeviceHealthAttestationService", 1)],
            RemoveOps    = [RegOp.DeleteValue(HaKey, "EnableDeviceHealthAttestationService")],
            DetectOps    = [RegOp.CheckDword(HaKey, "EnableDeviceHealthAttestationService", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-set-lockout-threshold-10",
            Label        = "Set TPM Anti-Hammering Lockout Threshold to 10 Failures",
            Category     = "TPM Attestation Policy",
            Description  = "Configures the TPM anti-hammering lockout threshold to 10 failed authorisation attempts, after which the TPM enters lockout mode requiring administrative reset, protecting against brute-force TPM dictionary attacks.",
            Tags         = ["tpm", "lockout", "anti-hammering", "brute-force", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "TPM lockout after 10 failed auth attempts; brute-force dictionary attacks against TPM mitigated.",
            ApplyOps     = [RegOp.SetDword(Key, "StandardUserAuthorizationFailureDuration_Name", 10)],
            RemoveOps    = [RegOp.DeleteValue(Key, "StandardUserAuthorizationFailureDuration_Name")],
            DetectOps    = [RegOp.CheckDword(Key, "StandardUserAuthorizationFailureDuration_Name", 10)],
        },
        new TweakDef
        {
            Id           = "tpmpol-block-tpm-clear-by-standard-user",
            Label        = "Block Standard Users from Clearing the TPM",
            Category     = "TPM Attestation Policy",
            Description  = "Prevents standard (non-administrator) users from clearing the TPM chip, which would destroy all TPM-protected keys and could be used to defeat BitLocker and Credential Guard protections.",
            Tags         = ["tpm", "clear-tpm", "standard-user", "bitlocker", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "TPM clear blocked for standard users; only admins can clear TPM. Prevents BitLocker key destruction.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockStandardUserClearTPM", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockStandardUserClearTPM")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockStandardUserClearTPM", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-require-tpm2-minimum",
            Label        = "Require TPM 2.0 Minimum for Secure Device Operations",
            Category     = "TPM Attestation Policy",
            Description  = "Enforces that all security operations requiring TPM attestation (BitLocker, Credential Guard, Device Guard, Windows Hello) use TPM 2.0, blocking fallback to the weaker TPM 1.2 specification.",
            Tags         = ["tpm", "tpm-2.0", "version-requirement", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "TPM 2.0 required minimum; security features that fall back to TPM 1.2 are blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireTPM20", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireTPM20")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireTPM20", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-enable-measured-boot",
            Label        = "Enable Windows Measured Boot with TPM PCR Logging",
            Category     = "TPM Attestation Policy",
            Description  = "Enables Measured Boot on the Windows bootloader, ensuring that each boot component hash is recorded in TPM PCR registers, creating an immutable tamper-evident boot measurement log.",
            Tags         = ["tpm", "measured-boot", "pcr", "boot-security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Measured Boot with TPM PCR logging enabled; boot chain tamper evident and verifiable via attestation.",
            ApplyOps     = [RegOp.SetDword(MbKey, "EnableMeasuredBoot", 1)],
            RemoveOps    = [RegOp.DeleteValue(MbKey, "EnableMeasuredBoot")],
            DetectOps    = [RegOp.CheckDword(MbKey, "EnableMeasuredBoot", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-disable-tpm-auto-provisioning",
            Label        = "Disable Automatic TPM Provisioning by Windows",
            Category     = "TPM Attestation Policy",
            Description  = "Prevents Windows from automatically taking ownership of the TPM during first boot provisioning, requiring explicit administrative TPM initialisation and ensuring TPM ownership is a deliberate IT action.",
            Tags         = ["tpm", "provisioning", "ownership", "admin", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Automatic TPM provisioning disabled; TPM ownership requires explicit admin initialisation.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableAutoProvisioning", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableAutoProvisioning")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableAutoProvisioning", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-log-tpm-events",
            Label        = "Log TPM Attestation and Lockout Events in Security Log",
            Category     = "TPM Attestation Policy",
            Description  = "Enables Security event log entries for TPM lockout events, attestation failures, and TPM provisioning changes, providing audit visibility into hardware security chip state changes.",
            Tags         = ["tpm", "event-log", "audit", "lockout", "attestation", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "TPM lockout and attestation events logged in Security log; hardware security chip state visible for auditing.",
            ApplyOps     = [RegOp.SetDword(Key, "LogTPMEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogTPMEvents")],
            DetectOps    = [RegOp.CheckDword(Key, "LogTPMEvents", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-disable-tpm-remote-management",
            Label        = "Disable Remote TPM Management via DCOM",
            Category     = "TPM Attestation Policy",
            Description  = "Prevents remote administration of the TPM chip via the DCOM TPM management interface, ensuring all TPM configuration changes require local administrator access to the physical or virtual machine.",
            Tags         = ["tpm", "remote-management", "dcom", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Remote TPM management via DCOM disabled; TPM configuration requires local admin access only.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableRemoteTPMManagement", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableRemoteTPMManagement")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableRemoteTPMManagement", 1)],
        },
        new TweakDef
        {
            Id           = "tpmpol-disable-tpm-telemetry",
            Label        = "Disable TPM Telemetry Reporting to Microsoft",
            Category     = "TPM Attestation Policy",
            Description  = "Prevents Windows from sending TPM chip model, firmware version, PCR configuration, and health attestation result telemetry to Microsoft.",
            Tags         = ["tpm", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "TPM telemetry to Microsoft disabled; chip model, firmware, and attestation data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableTPMTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableTPMTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableTPMTelemetry", 1)],
        },
    ];
}
