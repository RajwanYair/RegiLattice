// RegiLattice.Core — Tweaks/KernelDmaProtectionPolicy.cs
// Kernel DMA Protection (PCIe/Thunderbolt/DMA Remapping) policy controls — Sprint 466.
// Category: "Kernel DMA Protection Policy" | Slug: kdmapol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Kernel DMA Protection
//           HKLM\SYSTEM\CurrentControlSet\Control\DmaSecurity

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KernelDmaProtectionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Kernel DMA Protection";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DmaSecurity";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "kdmapol-enable-dma-remapping-policy",
                Label = "Enable DMA Remapping Enforcement Policy",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Enforces the Kernel DMA Protection policy requiring all PCIe devices to support DMA remapping (IOMMU/VT-d) before being granted full DMA access, blocking legacy DMA attack vectors.",
                Tags = ["kernel-dma", "iommu", "vtd", "pcie", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "DMA remapping enforced; legacy PCIe devices without IOMMU support denied full DMA access.",
                ApplyOps = [RegOp.SetDword(Key, "DeviceEnumerationPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeviceEnumerationPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "DeviceEnumerationPolicy", 0)],
            },
            new TweakDef
            {
                Id = "kdmapol-block-pre-boot-dma",
                Label = "Block Pre-Boot DMA Access on Thunderbolt Ports",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Blocks all Thunderbolt DMA access during the pre-boot phase, preventing attacks that attach malicious Thunderbolt devices before the OS IOMMU policy is loaded.",
                Tags = ["kernel-dma", "thunderbolt", "pre-boot", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Pre-boot Thunderbolt DMA blocked; only authorised devices can perform DMA after OS IOMMU policy loads.",
                ApplyOps = [RegOp.SetDword(Key, "AllowFlexibleLinkPowerManagement", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowFlexibleLinkPowerManagement")],
                DetectOps = [RegOp.CheckDword(Key, "AllowFlexibleLinkPowerManagement", 0)],
            },
            new TweakDef
            {
                Id = "kdmapol-enforce-iommu-all-devices",
                Label = "Enforce IOMMU DMA Remapping for All PCIe Devices",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Requires IOMMU DMA remapping to be applied to all PCIe devices regardless of whether they declare DMA support, ensuring legacy storage and network cards are also isolated.",
                Tags = ["kernel-dma", "iommu", "all-devices", "pcie", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "IOMMU applied universally; legacy PCIe cards may show reduced throughput due to remapping overhead.",
                ApplyOps = [RegOp.SetDword(Key2, "EnforceIOMMUForAllDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnforceIOMMUForAllDevices")],
                DetectOps = [RegOp.CheckDword(Key2, "EnforceIOMMUForAllDevices", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-block-dma-resume-attack",
                Label = "Block DMA Attack During Sleep/Resume Transition",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Maintains IOMMU DMA remapping tables across system sleep/hibernate and resume cycles, preventing DMA attacks that exploit the window during which remapping tables are reloaded.",
                Tags = ["kernel-dma", "sleep", "resume", "iommu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "DMA remapping persists across S3/S4 transitions; resume-time DMA attacks blocked.",
                ApplyOps = [RegOp.SetDword(Key2, "MaintainRemappingOnResume", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "MaintainRemappingOnResume")],
                DetectOps = [RegOp.CheckDword(Key2, "MaintainRemappingOnResume", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-restrict-tb-autorisation",
                Label = "Restrict Thunderbolt Authorisation to Admin-Only",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Restricts the authorisation of new Thunderbolt devices (adding to the trusted device store) to administrators only, preventing standard users from approving new DMA-capable Thunderbolt peripherals.",
                Tags = ["kernel-dma", "thunderbolt", "authorisation", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot authorise new Thunderbolt devices; admin approval required for each new TB peripheral.",
                ApplyOps = [RegOp.SetDword(Key2, "RestrictThunderboltAuthToAdmin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RestrictThunderboltAuthToAdmin")],
                DetectOps = [RegOp.CheckDword(Key2, "RestrictThunderboltAuthToAdmin", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-enable-dma-audit-log",
                Label = "Enable DMA Remapping Audit Event Logging",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Enables kernel event logging for DMA remapping policy enforcement actions, recording each blocked or remapped DMA access attempt for forensic analysis.",
                Tags = ["kernel-dma", "audit-log", "iommu", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DMA remapping events logged; blocked DMA attempts visible in Security/System event log.",
                ApplyOps = [RegOp.SetDword(Key2, "EnableDMAAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnableDMAAuditLog")],
                DetectOps = [RegOp.CheckDword(Key2, "EnableDMAAuditLog", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-block-expresscard-dma",
                Label = "Block ExpressCard/PCMCIA DMA Access",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Blocks DMA access for legacy ExpressCard and PCMCIA devices that pre-date IOMMU support, preventing DMA attacks via older expansion card interfaces on laptops.",
                Tags = ["kernel-dma", "expresscard", "pcmcia", "legacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ExpressCard/PCMCIA DMA blocked; legacy expansion cards operate in PIO mode only.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockExpressCardDMA", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockExpressCardDMA")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockExpressCardDMA", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-require-vtd-for-tb4",
                Label = "Require VT-d Active for Thunderbolt 4 Operation",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Requires Intel VT-d (IOMMU) to be active and enforcing before Thunderbolt 4 devices are enumerated and allowed DMA access, blocking TB4 use on systems with IOMMU disabled in BIOS.",
                Tags = ["kernel-dma", "vtd", "thunderbolt-4", "iommu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "TB4 blocked if VT-d disabled; BIOS must enable IOMMU for Thunderbolt 4 to function.",
                ApplyOps = [RegOp.SetDword(Key2, "RequireVTdForTB4", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RequireVTdForTB4")],
                DetectOps = [RegOp.CheckDword(Key2, "RequireVTdForTB4", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-block-usb4-dma-without-auth",
                Label = "Block USB4 DMA Without Device Authorisation",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Requires explicit device authorisation for USB4 (Thunderbolt-tunnelled) DMA access, blocking USB4 tunnelled DMA from unapproved devices until confirmed by an administrator.",
                Tags = ["kernel-dma", "usb4", "thunderbolt", "authorisation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "USB4 DMA blocked until device is admin-authorised; unauthorised USB4 devices cannot perform DMA.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockUSB4DMAWithoutAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockUSB4DMAWithoutAuth")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockUSB4DMAWithoutAuth", 1)],
            },
            new TweakDef
            {
                Id = "kdmapol-set-remapping-timeout",
                Label = "Set DMA Remapping Table Rebuild Timeout to 5 Seconds",
                Category = "Kernel DMA Protection Policy",
                Description =
                    "Limits the DMA remapping table rebuild timeout to 5 seconds, ensuring that if a device fails to initialise IOMMU remapping within the timeout, it is disconnected rather than granted unrestricted DMA.",
                Tags = ["kernel-dma", "remapping", "timeout", "iommu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DMA devices failing IOMMU init are disconnected after 5 seconds; no silent fallback to unrestricted DMA.",
                ApplyOps = [RegOp.SetDword(Key2, "RemappingTableRebuildTimeoutSec", 5)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RemappingTableRebuildTimeoutSec")],
                DetectOps = [RegOp.CheckDword(Key2, "RemappingTableRebuildTimeoutSec", 5)],
            },
        ];
}
