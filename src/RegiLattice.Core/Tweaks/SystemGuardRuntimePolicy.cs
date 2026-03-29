// RegiLattice.Core — Tweaks/SystemGuardRuntimePolicy.cs
// System Guard Runtime Monitor (SGRM), attestation, and runtime measurement controls — Sprint 465.
// Category: "System Guard Runtime Policy" | Slug: sgrm
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\SystemGuardRuntime

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SystemGuardRuntimePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\SystemGuardRuntime";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sgrm-enable-system-guard-runtime",
                Label = "Enable System Guard Runtime Monitor",
                Category = "System Guard Runtime Policy",
                Description =
                    "Enables System Guard Runtime Monitor (SGRM) which continuously monitors the OS security state during runtime using VBS, detecting live kernel patching, rootkits, and hypervisor attacks.",
                Tags = ["system-guard", "runtime-monitor", "vbs", "attestation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "SGRM active; runtime kernel integrity monitored; rootkits and hypervisor attacks detected.",
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-require-firmware-attestation",
                Label = "Require Firmware Attestation in System Guard",
                Category = "System Guard Runtime Policy",
                Description =
                    "Requires System Guard to include firmware measurement (SMBIOS, ACPI tables, UEFI variables) in its attestation report, detecting firmware-level tampering via Microsoft AAP cloud verification.",
                Tags = ["system-guard", "firmware", "attestation", "uefi", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Firmware attestation included; unauthorized UEFI/BIOS changes detectable via cloud attestation.",
                ApplyOps = [RegOp.SetDword(Key, "RequireFirmwareAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireFirmwareAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "RequireFirmwareAttestation", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-enable-kernel-dma-runtime-check",
                Label = "Enable Runtime DMA Remapping Check in System Guard",
                Category = "System Guard Runtime Policy",
                Description =
                    "Enables System Guard to continuously verify that the Intel VT-d / AMD-Vi DMA remapping tables are not tampered with at runtime, detecting DMA remap attacks from existing PCIe devices.",
                Tags = ["system-guard", "dma", "vtd", "pcie", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "DMA remap table integrity checked at runtime; live DMA table tamper attacks detected.",
                ApplyOps = [RegOp.SetDword(Key, "EnableKernelDMARuntimeCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelDMARuntimeCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKernelDMARuntimeCheck", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-enable-memory-scan",
                Label = "Enable System Guard Memory Scan for Rootkits",
                Category = "System Guard Runtime Policy",
                Description =
                    "Enables System Guard runtime memory scanning to detect kernel rootkits and bootkits that modify kernel data structures, leveraging VBS isolation to inspect kernel memory safely.",
                Tags = ["system-guard", "memory-scan", "rootkit", "vbs", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Kernel memory scanned at runtime; rootkits targeting SSDT/DKOM/EPROCESS detected.",
                ApplyOps = [RegOp.SetDword(Key, "EnableRuntimeMemoryScan", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRuntimeMemoryScan")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRuntimeMemoryScan", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-set-attestation-interval",
                Label = "Set System Guard Attestation Report Interval to 60 Minutes",
                Category = "System Guard Runtime Policy",
                Description =
                    "Configures System Guard to generate and upload attestation reports every 60 minutes, ensuring near-real-time security state visibility in Microsoft Defender for Endpoint.",
                Tags = ["system-guard", "attestation", "interval", "mde", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Attestation reports every 60 minutes; security state visible in MDE within 1 hour of a compromise.",
                ApplyOps = [RegOp.SetDword(Key, "AttestationIntervalMinutes", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "AttestationIntervalMinutes")],
                DetectOps = [RegOp.CheckDword(Key, "AttestationIntervalMinutes", 60)],
            },
            new TweakDef
            {
                Id = "sgrm-block-anti-cheat-bypass",
                Label = "Block Anti-Cheat Driver from Bypassing System Guard",
                Category = "System Guard Runtime Policy",
                Description =
                    "Blocks kernel-mode anti-cheat and DRM drivers from acquiring write access to kernel memory regions protected by System Guard, preventing anti-cheat tools from weakening system integrity.",
                Tags = ["system-guard", "anti-cheat", "drm", "kernel", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Anti-cheat drivers with kernel write access blocked; some games with invasive anti-cheat may not run.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAntiCheatDriverBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAntiCheatDriverBypass")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAntiCheatDriverBypass", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-enable-hypervisor-integrity-check",
                Label = "Enable Hypervisor Page Table Integrity Check",
                Category = "System Guard Runtime Policy",
                Description =
                    "Enables System Guard to verify the integrity of the Hyper-V hypervisor page table entries at runtime, detecting attacks that modify the hypervisor's own memory mappings.",
                Tags = ["system-guard", "hypervisor", "page-table", "integrity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Hypervisor page table verified; attacks targeting VBS isolation layers detected.",
                ApplyOps = [RegOp.SetDword(Key, "EnableHypervisorIntegrityCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableHypervisorIntegrityCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnableHypervisorIntegrityCheck", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-secure-world-crash-dump-policy",
                Label = "Restrict Crash Dump Access from Outside Secure World",
                Category = "System Guard Runtime Policy",
                Description =
                    "Configures System Guard to prevent the normal OS kernel dump process from reading VBS Secure World memory, ensuring credential material and kernel secrets are not exposed in crash dumps.",
                Tags = ["system-guard", "crash-dump", "secure-world", "credential-protection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Secure World memory excluded from crash dumps; credentials not leaked in BSOD minidumps.",
                ApplyOps = [RegOp.SetDword(Key, "RestrictCrashDumpFromSecureWorld", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCrashDumpFromSecureWorld")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCrashDumpFromSecureWorld", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-require-modern-standby-isolation",
                Label = "Require Hardware Isolation During Modern Standby",
                Category = "System Guard Runtime Policy",
                Description =
                    "Requires System Guard to maintain hardware VBS isolation during Modern Standby (DRIPS) low-power states, preventing attacks that exploit relaxed memory permissions during sleep transitions.",
                Tags = ["system-guard", "modern-standby", "sleepstate", "vbs", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "VBS isolation maintained in Modern Standby; sleep-state memory attacks mitigated.",
                ApplyOps = [RegOp.SetDword(Key, "RequireIsolationDuringModernStandby", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIsolationDuringModernStandby")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIsolationDuringModernStandby", 1)],
            },
            new TweakDef
            {
                Id = "sgrm-enable-post-boot-runtime-check",
                Label = "Enable Post-Boot Runtime Integrity Check",
                Category = "System Guard Runtime Policy",
                Description =
                    "Enables a periodic post-boot integrity check by System Guard that verifies critical kernel structures have not been modified since boot, catching late-stage kernel tampering.",
                Tags = ["system-guard", "post-boot", "integrity", "kernel", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Kernel structures periodically re-checked post-boot; late-stage kernel rootkits detected.",
                ApplyOps = [RegOp.SetDword(Key, "EnablePostBootRuntimeCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePostBootRuntimeCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePostBootRuntimeCheck", 1)],
            },
        ];
}
