// RegiLattice.Core — Tweaks/SecureLaunchDrtmPolicy.cs
// Secure Launch (DRTM), Trusted Launch, and Dynamic Root of Trust Measurement policy — Sprint 464.
// Category: "Secure Launch DRTM Policy" | Slug: sldrtm
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\SecureLaunch
//           HKLM\SYSTEM\CurrentControlSet\Control\TpmBootEntropy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecureLaunchDrtmPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\SecureLaunch";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TpmBootEntropy";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sldrtm-enable-secure-launch",
                Label = "Enable Secure Launch (DRTM) for Boot Integrity",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Enables Secure Launch using Dynamic Root of Trust for Measurement (DRTM), which uses CPU SKINIT/SENTER instructions and TPM to establish a fresh chain of trust independent of firmware prior to OS load.",
                Tags = ["secure-launch", "drtm", "tpm", "boot-integrity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "DRTM-based Secure Launch active; firmware compromise isolated from OS boot chain. Requires Intel TXT or AMD SKINIT.",
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-require-txt-active",
                Label = "Require Intel TXT Active for Secure Launch",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Requires Intel Trusted Execution Technology (TXT) to be active and verified for Secure Launch to proceed, aborting boot if TXT is disabled or in error state.",
                Tags = ["secure-launch", "intel-txt", "drtm", "tpm", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Boot halted if TXT inactive; ensures DRTM chain is enforced on every boot.",
                ApplyOps = [RegOp.SetDword(Key, "RequireIntelTXTActive", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIntelTXTActive")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIntelTXTActive", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-seal-tpm-to-secure-launch",
                Label = "Seal TPM PCR Values to Secure Launch Measurements",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Configures the TPM to seal key material to the PCR values produced by the DRTM Secure Launch measurement, ensuring sealed secrets are only released when the boot chain is unmodified.",
                Tags = ["secure-launch", "tpm", "pcr", "sealed-storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "TPM-sealed secrets only released if boot measurements match; tampered firmware locks out DRTM-sealed data.",
                ApplyOps = [RegOp.SetDword(Key, "SealTPMToSecureLaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SealTPMToSecureLaunch")],
                DetectOps = [RegOp.CheckDword(Key, "SealTPMToSecureLaunch", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-enable-tpm-boot-entropy",
                Label = "Enable TPM Boot Entropy for DRTM",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Enables TPM-based boot entropy collection during the DRTM phase, seeding the OS CSPRNG with hardware-attested randomness from the TPM prior to key generation.",
                Tags = ["secure-launch", "tpm", "entropy", "csprng", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "TPM boot entropy enabled; OS CSPRNG seeded with hardware RNG before cryptographic keys generated.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableBootEntropy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableBootEntropy")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableBootEntropy", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-require-verified-acm",
                Label = "Require Verified Authenticated Code Module for DRTM",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Requires that the Intel TXT/AMD SKINIT Authenticated Code Module (ACM) used in DRTM is cryptographically verified before execution, preventing use of revoked or tampered ACMs.",
                Tags = ["secure-launch", "acm", "drtm", "code-integrity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ACM verified before DRTM proceeds; revoked ACMs rejected, maintaining DRTM chain integrity.",
                ApplyOps = [RegOp.SetDword(Key, "RequireVerifiedACM", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireVerifiedACM")],
                DetectOps = [RegOp.CheckDword(Key, "RequireVerifiedACM", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-block-bootkit-execution",
                Label = "Block Bootkit Execution via DRTM Measurement",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Enables the DRTM measurement of the MBR and VBR sectors to detect bootkits that modify the boot record, causing the boot to fail if the MBR/VBR measurements do not match the expected policy.",
                Tags = ["secure-launch", "bootkit", "mbr", "vbr", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "MBR/VBR measured by DRTM; bootkit infection detected and boot halted.",
                ApplyOps = [RegOp.SetDword(Key, "BlockBootkitExecution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockBootkitExecution")],
                DetectOps = [RegOp.CheckDword(Key, "BlockBootkitExecution", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-enable-slat-enforcement",
                Label = "Enforce Second Level Address Translation (SLAT) for VBS",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Requires Second Level Address Translation (Intel EPT / AMD RVI) to be active and used by VBS before allowing the Secure Launch environment to initialise.",
                Tags = ["secure-launch", "slat", "ept", "rvi", "vbs", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SLAT required; without EPT/RVI the Secure Launch environment will not initialise.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSLAT", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSLAT")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSLAT", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-enable-sl-logging",
                Label = "Enable Secure Launch Audit Logging",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Enables event logging for all Secure Launch DRTM measurement and verification events, providing a forensic record of the boot chain measurements on each startup.",
                Tags = ["secure-launch", "logging", "audit", "drtm", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DRTM events logged on boot; measurements and any anomalies visible in event log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableSecureLaunchLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSecureLaunchLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSecureLaunchLogging", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-block-intel-me-exploit",
                Label = "Block Intel ME/AMT Exploit Path via TXT Lockdown",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Configures TXT policy to lock down the Intel Management Engine (ME/AMT) memory space during the DRTM launch phase, mitigating SMM handler exploits that target ME-accessible addresses.",
                Tags = ["secure-launch", "intel-me", "amt", "smm", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "ME/AMT memory space locked during DRTM; SMM attacks via ME mitigated.",
                ApplyOps = [RegOp.SetDword(Key, "BlockIntelMEExploitPath", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockIntelMEExploitPath")],
                DetectOps = [RegOp.CheckDword(Key, "BlockIntelMEExploitPath", 1)],
            },
            new TweakDef
            {
                Id = "sldrtm-require-tpm-pcr17-attestation",
                Label = "Require TPM PCR17 Attestation for Secure Launch",
                Category = "Secure Launch DRTM Policy",
                Description =
                    "Requires TPM PCR17 to be populated by DRTM measurements and attestation-verified before Windows allows the Secure Launch environment to proceed, ensuring an unbroken hardware attestation chain.",
                Tags = ["secure-launch", "tpm", "pcr17", "attestation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PCR17 attestation required; Secure Launch halts if PCR17 is not set by DRTM.",
                ApplyOps = [RegOp.SetDword(Key, "RequireTPMPCR17Attestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTPMPCR17Attestation")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTPMPCR17Attestation", 1)],
            },
        ];
}
