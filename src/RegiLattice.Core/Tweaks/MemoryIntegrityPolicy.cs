// RegiLattice.Core — Tweaks/MemoryIntegrityPolicy.cs
// Sprint 335: Memory Integrity Policy tweaks (10 tweaks)
// Category: "Memory Integrity Policy" | Slug: memintg
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MemoryIntegrityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "memintg-enable-hvci",
            Label = "Enable Hypervisor-Protected Code Integrity (HVCI/Memory Integrity)",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Hypervisor-Protected Code Integrity uses the Windows Hypervisor to isolate kernel code integrity validation from the operating system preventing kernel-mode rootkits. Enabling HVCI ensures that all kernel-mode code must pass signature validation in an environment that cannot be tampered with by usermode or kernel-mode attacks. HVCI prevents attackers from loading unsigned or modified drivers into kernel memory even when they have obtained SYSTEM-level access. Kernel-mode rootkits are one of the most persistent and difficult to detect forms of malware and HVCI provides strong protection against this attack class. HVCI may cause performance impact on systems with older processors that lack hardware optimizations for virtualization-based security. Organizations should evaluate processor compatibility and driver compatibility before enabling HVCI in production as some legacy drivers are incompatible.",
            Tags = ["hvci", "memory-integrity", "device-guard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HypervisorEnforcedCodeIntegrity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HypervisorEnforcedCodeIntegrity")],
            DetectOps = [RegOp.CheckDword(Key, "HypervisorEnforcedCodeIntegrity", 1)],
        },
        new TweakDef
        {
            Id = "memintg-enable-vbs",
            Label = "Enable Virtualization-Based Security (VBS)",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Virtualization-Based Security establishes a hypervisor-protected enclave that shields sensitive components including Credential Guard and Memory Integrity from the main OS. Enabling VBS creates the security foundation required for Credential Guard, HVCI, and other VBS-dependent security features. VBS uses hardware virtualization to create a secure world that is isolated from the normal operating system environment where malware may be operating. Security components running within VBS are protected from credential-stealing malware even if the main operating system is compromised. VBS requires compatible hardware and may not be compatible with all virtualization scenarios — nested virtualization is required for VMs. Organizations should enable VBS on physical endpoints and evaluate hypervisor-level compatibility for virtual desktop infrastructure deployments.",
            Tags = ["vbs", "virtualization-security", "device-guard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableVirtualizationBasedSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableVirtualizationBasedSecurity")],
            DetectOps = [RegOp.CheckDword(Key, "EnableVirtualizationBasedSecurity", 1)],
        },
        new TweakDef
        {
            Id = "memintg-require-uefi-lock",
            Label = "Require UEFI Lock for Device Guard Configuration",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            Description =
                "UEFI lock for Device Guard configuration embeds the security settings in UEFI non-volatile storage preventing boot-time modification of security configurations. Requiring UEFI lock ensures that Device Guard and VBS configurations cannot be disabled without physical access to UEFI settings. Without UEFI lock an attacker with administrator rights and knowledge of registry paths could disable Device Guard by modifying registry settings and rebooting. UEFI-locked Device Guard configuration is the strongest security posture as it survives operating system reinstallation. UEFI lock is irreversible without the UEFI manufacturer's direct bypass mechanism making it unsuitable for environments where Device Guard may need to be disabled. Organizations should only enable UEFI lock after thoroughly testing Device Guard compatibility and confirming it will not need to be disabled.",
            Tags = ["uefi-lock", "device-guard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePlatformSecurityFeatures", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePlatformSecurityFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePlatformSecurityFeatures", 3)],
        },
        new TweakDef
        {
            Id = "memintg-enable-credential-guard",
            Label = "Enable Windows Defender Credential Guard",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Windows Defender Credential Guard uses VBS to store NTLM credentials, Kerberos tickets, and other credential material in an isolated secure world. Enabling Credential Guard protects against pass-the-hash and pass-the-ticket attacks that steal credentials from LSASS memory. Without Credential Guard domain credentials cached on endpoints can be extracted by Mimikatz and similar credential theft tools with local administrator access. Credential Guard makes lateral movement attacks significantly harder as captured credential material is stored in protected memory inaccessible to kernel-mode code running outside VBS. Credential Guard requires VBS and compatible hardware and may not be compatible with some smart card or federated identity scenarios. Organizations should test Credential Guard thoroughly before deployment as some authentication scenarios may require adjustments.",
            Tags = ["credential-guard", "credentials", "device-guard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LsaCfgFlags", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LsaCfgFlags")],
            DetectOps = [RegOp.CheckDword(Key, "LsaCfgFlags", 1)],
        },
        new TweakDef
        {
            Id = "memintg-enable-system-guard",
            Label = "Enable Windows Defender System Guard Runtime Monitor",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Windows Defender System Guard provides boot integrity attestation and runtime monitoring using hardware root of trust measured boot. Enabling System Guard allows remote attestation of device boot state ensuring that security configurations were properly applied at boot. System Guard uses TPM-measured boot to create cryptographic proof of the system's security configuration that can be verified by management systems. Runtime monitor capabilities detect changes to security-critical kernel components after boot providing ongoing integrity monitoring. System Guard attestation is particularly valuable for zero-trust access decisions where device health verification is required before granting resource access. Organizations using Intune conditional access can leverage System Guard attestation data to enforce device health compliance.",
            Tags = ["system-guard", "attestation", "device-guard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConfigureSystemGuardLaunch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureSystemGuardLaunch")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureSystemGuardLaunch", 1)],
        },
        new TweakDef
        {
            Id = "memintg-block-untrusted-fonts",
            Label = "Block Untrusted Font Loading in Kernel Mode",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Untrusted font blocking prevents loading of fonts from locations outside of system font directories reducing the attack surface for font parsing vulnerabilities. Blocking untrusted kernel-mode font loading prevents exploitation of font parsing vulnerabilities that have been used in privilege escalation attacks. Historical font parsing vulnerabilities in Windows kernel font subsystem have enabled elevation of privilege from limited user accounts. Untrusted font policy restricts font loading to pre-approved system directories where fonts have been validated by administrators. Applications that load fonts from user directories or network shares may be impacted by this policy requiring fonts to be installed to system directories. User mode font loading for document processing applications is not affected by this policy which specifically targets kernel-mode font loading.",
            Tags = ["fonts", "kernel", "memory-integrity", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableBlockUntrustedFonts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBlockUntrustedFonts")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBlockUntrustedFonts", 1)],
        },
        new TweakDef
        {
            Id = "memintg-enable-secure-launch",
            Label = "Enable Secure Launch Measured Boot (DRTM)",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Secure Launch uses Dynamic Root of Trust for Measurement (DRTM) to create a hardware-anchored measurement of the boot process beyond the static root of trust. Enabling Secure Launch creates a cryptographic record of the boot sequence that cannot be falsified even if the BIOS or early boot code has been compromised. DRTM-based attestation provides stronger boot integrity guarantees than TPM-only static measured boot in threat scenarios involving firmware attacks. Secure Launch is available on compatible processors that implement DRTM capabilities such as Intel TXT or AMD SKINIT technologies. System Guard uses Secure Launch to provide the highest level of boot integrity attestation for remote compliance verification. Organizations deploying Secure Launch should verify hardware compatibility and ensure BIOS supports DRTM features required for its operation.",
            Tags = ["secure-launch", "drtm", "boot-integrity", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConfigureSystemGuardLaunchForSMM", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureSystemGuardLaunchForSMM")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureSystemGuardLaunchForSMM", 1)],
        },
        new TweakDef
        {
            Id = "memintg-audit-vbs-incompatible-drivers",
            Label = "Enable Audit Mode for VBS Incompatible Drivers",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "VBS audit mode for incompatible drivers logs driver compatibility issues without blocking operation allowing organizations to identify issues before enforcing HVCI. Enabling VBS audit mode allows IT to discover which driver software would be blocked by HVCI enforcement without impacting system functionality. Audit mode generates events in the Windows Event Log that identify drivers and kernel modules that fail code integrity checks. Audit data helps organizations plan driver updates or vendor engagement before transitioning from audit to enforcement mode. Incompatible drivers identified in audit mode should be updated or replaced before enabling HVCI enforcement to prevent system functionality loss. The transition from audit to enforcement mode should be scheduled during device lifecycle refresh when driver updates can be deployed simultaneously.",
            Tags = ["hvci", "audit", "drivers", "device-guard", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HypervisorEnforcedCodeIntegrityAuditMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HypervisorEnforcedCodeIntegrityAuditMode")],
            DetectOps = [RegOp.CheckDword(Key, "HypervisorEnforcedCodeIntegrityAuditMode", 1)],
        },
        new TweakDef
        {
            Id = "memintg-enable-dma-protection",
            Label = "Enable Kernel DMA Protection for External Devices",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Kernel DMA Protection prevents PCIe devices connected through Thunderbolt, USB4, and other direct memory access capable ports from reading or writing arbitrary kernel memory addresses. Enabling kernel DMA protection blocks DMA-based attacks where a malicious device connected through a physical port can read credentials, encryption keys, or inject code into kernel memory. Thunderbolt DMA attacks have been demonstrated to extract full memory contents from locked Windows systems in minutes using commercial hardware. DMA protection uses the IOMMU to restrict PCIe device access to only the memory explicitly mapped for that device preventing unauthorized memory reads. Kernel DMA protection requires IOMMU hardware support and may impact performance for high-bandwidth DMA devices like external GPUs or capture cards. Organizations should evaluate DMA protection impact for workstations with specialized high-bandwidth PCIe accessories before broad deployment.",
            Tags = ["dma-protection", "thunderbolt", "hardware", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableBootDMAProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBootDMAProtection")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBootDMAProtection", 1)],
        },
        new TweakDef
        {
            Id = "memintg-enforce-kernel-shadow-stacks",
            Label = "Enable Hardware-Enforced Stack Protection for Kernel Code",
            Category = "Memory Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Hardware-enforced stack protection uses processor shadow stacks to detect return-oriented programming attacks that corrupt the call stack in kernel code. Enabling kernel shadow stack protection adds hardware-level verification to kernel code execution flow preventing ROP and JOP attack chains. Return-oriented programming attacks chain small sequences of existing code (gadgets) to achieve arbitrary code execution bypassing traditional control flow restrictions. Shadow stacks maintain a separate CPU-protected copy of return addresses that cannot be modified by software maintaining integrity of control flow. Intel CET (Control-flow Enforcement Technology) and AMD equivalent features provide the hardware shadow stack primitives required for this protection. Kernel shadow stack protection requires compatible processor support and may impact performance for workloads with high interrupt and system call frequency.",
            Tags = ["shadow-stacks", "rop-protection", "kernel", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "KernelShadowStacksEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "KernelShadowStacksEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "KernelShadowStacksEnabled", 1)],
        },
    ];
}
