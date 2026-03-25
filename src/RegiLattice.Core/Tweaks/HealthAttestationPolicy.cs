// RegiLattice.Core — Tweaks/HealthAttestationPolicy.cs
// Sprint 281: Health Attestation Group Policy (10 tweaks)
// Category: "Health Attestation Policy" | Slug: hltha
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthAttestation

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HealthAttestationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthAttestation";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "hltha-disable-remote-health-attestation",
            Label = "Disable Remote Health Attestation",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 4,
            Description =
                "Sets Enabled=0 in the HealthAttestation policy key. Prevents Windows "
                + "from sending TPM-sealed health attestation reports to a remote Health "
                + "Attestation Service (HAS) that verifies Secure Boot state, BitLocker "
                + "status, and ELAM boot-state measurements. On air-gapped or privacy-"
                + "focused machines, remote attestation transmits boot measurements as "
                + "a side channel for device identification. "
                + "Default: 1. Recommended: 0 on non-MDM standalone machines.",
            Tags = ["health", "attestation", "tpm", "remote", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
            DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "hltha-disable-attestation-telemetry",
            Label = "Disable Health Attestation Telemetry",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableAttestation Telemetry=1 in the HealthAttestation policy key. "
                + "Prevents the HealthAttestation service from emitting diagnostic events "
                + "that log quote generation attempts, PCR measurement values, and "
                + "compliance evaluation results to Windows telemetry. Boot measurement "
                + "sequences are unique to each machine's firmware configuration and "
                + "constitute a hardware fingerprint. "
                + "Default: 0. Recommended: 1.",
            Tags = ["health", "attestation", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAttestationTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAttestationTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAttestationTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "hltha-require-tpm-attestation",
            Label = "Require TPM for Device Health Attestation",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets RequireTpmAttestation=1 in the HealthAttestation policy key. Enforces "
                + "that all device health reports are backed by a hardware TPM quote rather "
                + "than software-only measurements. Without TPM backing, a tampered OS can "
                + "fabricate health reports that falsely appear compliant to the HAS, "
                + "undermining the entire conditional-access chain. "
                + "Default: 0. Recommended: 1 where TPM 2.0 is available.",
            Tags = ["health", "attestation", "tpm", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireTpmAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "RequireTpmAttestation", 1)],
        },
        new TweakDef
        {
            Id = "hltha-set-custom-has-url",
            Label = "Use Private Health Attestation Service",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets UseCustomHasUrl=1 in the HealthAttestation policy key. Signals that "
                + "the device should submit attestation requests to an organisation-"
                + "controlled private HAS endpoint rather than Microsoft's public cloud "
                + "HAS. Using a private endpoint prevents boot measurements, device IDs, "
                + "and TPM endorsement key certificates from transiting Microsoft's "
                + "infrastructure. Default: 0. Recommended: 1 in enterprise deployments.",
            Tags = ["health", "attestation", "private", "enterprise", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UseCustomHasUrl", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UseCustomHasUrl")],
            DetectOps = [RegOp.CheckDword(Key, "UseCustomHasUrl", 1)],
        },
        new TweakDef
        {
            Id = "hltha-disable-has-caching",
            Label = "Disable Health Attestation Report Caching",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableAttestation Caching=1 in the HealthAttestation policy key. "
                + "Forces the HAS client to generate a fresh attestation quote for each "
                + "compliance check rather than reusing a cached signed report. Cached "
                + "reports may reflect an outdated system state (e.g., before a detected "
                + "tamper was remediated) and provide a stale-quote replay surface. "
                + "Default: 0. Recommended: 1.",
            Tags = ["health", "attestation", "cache", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAttestationCaching", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAttestationCaching")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAttestationCaching", 1)],
        },
        new TweakDef
        {
            Id = "hltha-enforce-secureboot-check",
            Label = "Enforce Secure Boot in Health Check",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 5,
            Description =
                "Sets EnforceSecureBootCheck=1 in the HealthAttestation policy key. "
                + "Marks devices as non-compliant in attestation reports if Secure Boot "
                + "is not enabled and in enforcing mode. Secure Boot prevents unsigned "
                + "boot-path code from executing and is a foundational requirement for "
                + "UEFI-level integrity guarantees. Devices without Secure Boot active "
                + "should not be granted conditional access to enterprise resources. "
                + "Default: 0. Recommended: 1.",
            Tags = ["secureboot", "health", "attestation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSecureBootCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureBootCheck")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSecureBootCheck", 1)],
        },
        new TweakDef
        {
            Id = "hltha-enforce-bitlocker-check",
            Label = "Enforce BitLocker in Health Attestation",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 5,
            Description =
                "Sets EnforceBitLockerCheck=1 in the HealthAttestation policy key. Marks "
                + "devices as non-compliant if BitLocker drive encryption is not active "
                + "on the OS volume as reported by the TPM attestation. Unencrypted OS "
                + "volumes expose all credential stores and secrets to offline attacks "
                + "when physical access to the device is obtained. "
                + "Default: 0. Recommended: 1.",
            Tags = ["bitlocker", "health", "attestation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceBitLockerCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceBitLockerCheck")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceBitLockerCheck", 1)],
        },
        new TweakDef
        {
            Id = "hltha-enforce-elam-check",
            Label = "Enforce Early Launch Antimalware in Health Check",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets EnforceElamCheck=1 in the HealthAttestation policy key. Requires "
                + "that an ELAM-certified antimalware driver was active during the boot "
                + "sequence as recorded in the TPM boot log. ELAM drivers load before any "
                + "third-party code and verify kernel driver signatures; machines without "
                + "an active ELAM driver are more vulnerable to rootkit persistence during "
                + "boot. Default: 0. Recommended: 1.",
            Tags = ["elam", "health", "attestation", "boot", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceElamCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceElamCheck")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceElamCheck", 1)],
        },
        new TweakDef
        {
            Id = "hltha-enforce-vsm-check",
            Label = "Enforce Virtualization Based Security in Health Check",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets EnforceVsmCheck=1 in the HealthAttestation policy key. Marks devices "
                + "as non-compliant if Virtualization Based Security (VBS) is not enabled "
                + "and active as measured by the TPM. VBS isolates security-sensitive data "
                + "(LSA Isolated, Credential Guard, HVCI) in a separate Secure World, "
                + "significantly raising the bar for credential theft via kernel exploit. "
                + "Default: 0. Recommended: 1.",
            Tags = ["vbs", "vbs", "health", "attestation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceVsmCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceVsmCheck")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceVsmCheck", 1)],
        },
        new TweakDef
        {
            Id = "hltha-set-attestation-interval",
            Label = "Set Health Attestation Report Interval",
            Category = "Health Attestation Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets AttestationInterval=60 in the HealthAttestation policy key. Instructs "
                + "the HAS client to refresh compliance attestation reports every 60 minutes. "
                + "Shorter intervals detect configuration drift (Secure Boot disabled, "
                + "BitLocker suspended) and revoke conditional access sooner than the "
                + "default 24-hour cycle. "
                + "Default: 1440 (24 hours). Recommended: 60 in high-security environments.",
            Tags = ["health", "attestation", "interval", "compliance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AttestationInterval", 60)],
            RemoveOps = [RegOp.DeleteValue(Key, "AttestationInterval")],
            DetectOps = [RegOp.CheckDword(Key, "AttestationInterval", 60)],
        },
    ];
}
