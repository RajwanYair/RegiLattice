// RegiLattice.Core — Tweaks/VbsEnforcementPolicy.cs
// Virtualization-Based Security (VBS) enforcement, Memory Integrity, and HVCI prerequisites — Sprint 462.
// Category: "VBS Enforcement Policy" | Slug: vbsenf
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard
//           HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VbsEnforcementPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "vbsenf-enable-vbs",
                Label = "Enable Virtualization-Based Security (VBS)",
                Category = "VBS Enforcement Policy",
                Description =
                    "Enables Virtualization-Based Security, which uses hardware virtualisation to create an isolated execution environment (Secure World) that protects sensitive kernel data and code from OS-level compromises.",
                Tags = ["vbs", "security", "virtualization", "device-guard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "VBS enabled; Secure World isolation active. Requires Hyper-V capable CPU; reboot required.",
                ApplyOps = [RegOp.SetDword(Key, "EnableVirtualizationBasedSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableVirtualizationBasedSecurity")],
                DetectOps = [RegOp.CheckDword(Key, "EnableVirtualizationBasedSecurity", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-enable-hvci",
                Label = "Enable HVCI Memory Integrity via DeviceGuard",
                Category = "VBS Enforcement Policy",
                Description =
                    "Enables Hypervisor-Protected Code Integrity (HVCI / Memory Integrity) which uses VBS to ensure only signed code runs in kernel mode, blocking unsigned kernel drivers and rootkits.",
                Tags = ["hvci", "memory-integrity", "vbs", "device-guard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "HVCI active; unsigned kernel code blocked. Minor performance impact; incompatible drivers fail to load.",
                ApplyOps = [RegOp.SetDword(Key2, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key2, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-enable-hvci-audit",
                Label = "Enable HVCI Audit Mode (Log Before Block)",
                Category = "VBS Enforcement Policy",
                Description =
                    "Puts HVCI into Audit mode which logs policy violations (unsigned kernel-mode code) to the event log without blocking, useful for compatibility testing before enforcing.",
                Tags = ["hvci", "audit-mode", "vbs", "device-guard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "HVCI violations logged but not blocked; use to identify incompatible drivers before enabling enforcement.",
                ApplyOps = [RegOp.SetDword(Key2, "Audit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "Audit")],
                DetectOps = [RegOp.CheckDword(Key2, "Audit", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-require-uefi-lock",
                Label = "Require UEFI Lock for VBS Configuration",
                Category = "VBS Enforcement Policy",
                Description =
                    "Locks VBS configuration via UEFI so it cannot be disabled from the OS, even by an administrator, requiring physical UEFI/BIOS access to turn off — provides boot-time tamper resistance.",
                Tags = ["vbs", "uefi-lock", "security", "tamper-resistance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "VBS locked in UEFI; disabling requires physical BIOS access. Only apply on managed, tested hardware.",
                ApplyOps = [RegOp.SetDword(Key, "RequirePlatformSecurityFeatures", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePlatformSecurityFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePlatformSecurityFeatures", 3)],
            },
            new TweakDef
            {
                Id = "vbsenf-enable-secure-memory-overwrite",
                Label = "Enable Secure Memory Overwrite on Shutdown",
                Category = "VBS Enforcement Policy",
                Description =
                    "Enables the secure memory overwrite function that clears VBS-protected LSASS memory on shutdown, preventing cold-boot attacks from recovering credential material from memory.",
                Tags = ["vbs", "memory-overwrite", "cold-boot", "lsass", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "VBS memory overwritten on shutdown; cold-boot credential theft prevented.",
                ApplyOps = [RegOp.SetDword(Key, "EnableSecureMemoryOverwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSecureMemoryOverwrite")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSecureMemoryOverwrite", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-lock-config-flags",
                Label = "Lock VBS Configuration Flags to Prevent Downgrade",
                Category = "VBS Enforcement Policy",
                Description =
                    "Sets the ConfigFlags registry value to lock VBS in its current secure state, preventing an attacker with registry access from disabling VBS before the next reboot.",
                Tags = ["vbs", "config-lock", "downgrade-protection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "VBS config change requires reboot to take effect; runtime downgrade attacks blocked.",
                ApplyOps = [RegOp.SetDword(Key, "ConfigFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigFlags")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigFlags", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-block-third-party-kernel-drivers",
                Label = "Block Third-Party Unsigned Kernel Drivers via VBS",
                Category = "VBS Enforcement Policy",
                Description =
                    "Configures VBS Code Integrity policy to block third-party unsigned or revoked kernel drivers from loading, ensuring only Microsoft-signed and WHQL-certified drivers run in kernel mode.",
                Tags = ["vbs", "drivers", "code-integrity", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Unsigned/revoked kernel drivers blocked; some legacy hardware drivers may not load.",
                ApplyOps = [RegOp.SetDword(Key, "HVCIBlockThirdPartyKernelDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HVCIBlockThirdPartyKernelDrivers")],
                DetectOps = [RegOp.CheckDword(Key, "HVCIBlockThirdPartyKernelDrivers", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-enable-kernel-dma-protection",
                Label = "Enable Kernel DMA Protection via VBS",
                Category = "VBS Enforcement Policy",
                Description =
                    "Enables Kernel DMA Protection (Thunderbolt/PCIe DMA attack mitigation) via VBS, preventing external devices from reading or writing kernel memory through direct memory access without authorisation.",
                Tags = ["vbs", "kernel-dma", "thunderbolt", "pcie", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "DMA from Thunderbolt/PCIe restricted until device authorised; DMA-based attacks blocked.",
                ApplyOps = [RegOp.SetDword(Key, "EnableKernelDMAProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelDMAProtection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKernelDMAProtection", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-require-tpm-for-vbs",
                Label = "Require TPM 2.0 for VBS Secure World",
                Category = "VBS Enforcement Policy",
                Description =
                    "Requires a TPM 2.0 chip as a prerequisite for activating the VBS Secure World, ensuring the VBS Secure Boot chain is bound to hardware attestation.",
                Tags = ["vbs", "tpm", "secure-boot", "attestation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "VBS requires TPM 2.0; machines without TPM cannot enable VBS.",
                ApplyOps = [RegOp.SetDword(Key, "RequireTPMForVBS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTPMForVBS")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTPMForVBS", 1)],
            },
            new TweakDef
            {
                Id = "vbsenf-enable-credential-guard",
                Label = "Enable Credential Guard via VBS",
                Category = "VBS Enforcement Policy",
                Description =
                    "Enables Windows Credential Guard which uses VBS to protect LSASS credential material (NTLM hashes, Kerberos TGTs) in an isolated Secure World process, preventing Pass-the-Hash attacks.",
                Tags = ["vbs", "credential-guard", "lsass", "pass-the-hash", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Credential Guard active; NTLM hashes and Kerberos TGTs protected; PTH/PTT attacks blocked.",
                ApplyOps = [RegOp.SetDword(Key, "EnableCredentialGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCredentialGuard")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCredentialGuard", 1)],
            },
        ];
}
