// RegiLattice.Core — Tweaks/SecureBootDbxPolicy.cs
// Secure Boot DBX (Forbidden Signatures Database) and Secure Boot Audit Policy
// — Sprint 583.
// Configures Secure Boot DBX update policy, Secure Boot state enforcement,
// boot policy audit logging, and UEFI Secure Boot bypass prevention.
// Category: "Secure Boot DBX Policy" | Slug: sbdbx
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\UEFI
//           HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecureBootDbxPolicy
{
    private const string UefiPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI";

    private const string DeviceGuardKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sbdbx-enable-dbx-automatic-update",
                Label = "Secure Boot DBX: Enable Automatic Secure Boot Forbidden Signatures (DBX) Update",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets EnableDbxUpdate=1 in the UEFI policy hive. Enables automatic updates to the Secure Boot DBX (Forbidden Signatures Database) via Windows Update. The DBX contains hashes of compromised or revoked Secure Boot bootloaders and keys — when a bootloader is found to have a security vulnerability that can bypass Secure Boot (e.g., the BlackLotus bootkit targets CVE-2022-21894), Microsoft adds its hash to the DBX and distributes the update via Windows Update. Without automatic DBX updates, a compromised bootloader that has been publicly revoked can still be used to bypass Secure Boot on unpatched systems.",
                Tags = ["secure-boot", "dbx", "forbidden-signatures", "bootkit", "uefi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Secure Boot DBX updated automatically via Windows Update. Once a bootloader hash is added to the DBX, that specific bootloader cannot be used to boot the device — including older legitimate Windows bootloaders. Do not roll back Windows installation to a state using a revoked bootloader on DBX-updated systems. Ensure backup boot media is also updated before applying DBX updates in datacenter environments.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnableDbxUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnableDbxUpdate")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnableDbxUpdate", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-require-secure-boot-enabled",
                Label = "Secure Boot DBX: Require Secure Boot to be Enabled (Block Boot if Disabled)",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets RequireSecureBoot=1 in the UEFI policy hive. Enforces a policy that Secure Boot must be enabled on this device. If Secure Boot is detected as disabled in the firmware, Windows can generate compliance events and Intune/AAD Conditional Access can block the device from accessing corporate resources until Secure Boot is re-enabled. This policy does not directly enable Secure Boot in firmware (that requires a separate UEFI configuration), but it marks the device as non-compliant for enterprise attestation purposes when Secure Boot is off.",
                Tags = ["secure-boot", "compliance", "firmware", "attestation", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Secure Boot state is monitored; non-compliance triggers Intune/Conditional Access blocking. Devices with Secure Boot disabled appear as non-compliant in Intune. Dual-boot Linux systems that require disabling Secure Boot will become non-compliant. Enterprise HDI (Hardware Device Initiative) deployments must ensure all devices support and have Secure Boot enabled in firmware.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "RequireSecureBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "RequireSecureBoot")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "RequireSecureBoot", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-enable-uefi-boot-event-logging",
                Label = "Secure Boot DBX: Enable UEFI Boot Event and Measurement Logging",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets EnableUefiBootEventLogging=1 in the UEFI policy hive. Enables logging of UEFI boot events and TPM PCR measurements to the Windows event log. During boot, the UEFI firmware measures each boot component (UEFI itself, secure boot db/dbx state, boot manager, OS loader) and extends the TPM PCRs. Enabling event logging records these measurements in the Windows event log (TCG EventLog / Measured Boot). SOC tools and attestation services use these events to detect firmware tampering, kernel module loading of unsigned code, or PCR value anomalies that may indicate a rootkit or bootkit is active.",
                Tags = ["uefi", "boot-events", "tpm-measurement", "tcg", "boot-attestation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "UEFI boot measurement events logged to Windows event log. Events appear in the Microsoft-Windows-TPM-WMI/Operational log. Minimal performance impact — measurements are performed during boot before Windows loads. Enables Measured Boot attestation for Microsoft Defender for Endpoint and third-party boot integrity tools.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnableUefiBootEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnableUefiBootEventLogging")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnableUefiBootEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-block-unsigned-uefi-variables-write",
                Label = "Secure Boot DBX: Block User-Mode Write Access to Unsigned UEFI Variables",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets BlockUnsignedUefiVarsWrite=1 in the UEFI policy hive. Prevents user-mode processes from writing unsigned UEFI variables to the UEFI NVRAM. UEFI variables are persistent firmware storage accessible from both firmware and running OS. Attackers have used arbitrary UEFI variable writes (e.g., FinFisher and the Lojax bootkit) to plant malicious firmware bypass code in UEFI NVRAM. Blocking unsigned variable writes from user-mode requires that any UEFI variable modification be signed with a key in the Secure Boot DB — preventing attacker UEFI variable injection from compromised user accounts.",
                Tags = ["uefi", "variable-write", "nvram", "bootkit", "lojax"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Unsigned UEFI variable writes from user-mode are blocked. Administrators can still write signed UEFI variables. Firmware update utilities and some OEM diagnostic tools that write UEFI variables directly must be updated to use signed variable writes. Test all firmware update tools before enforcing — some BIOSupdate.exe tools will fail.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "BlockUnsignedUefiVarsWrite", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "BlockUnsignedUefiVarsWrite")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "BlockUnsignedUefiVarsWrite", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-enable-vbs-device-guard",
                Label = "Secure Boot DBX: Enable VBS Platform Security Level 2 (Secure Boot + IOMMU)",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets RequirePlatformSecurityFeatures=3 in DeviceGuard (value 3 = Secure Boot AND IOMMU/DMAR required for VBS). Requires both Secure Boot and IOMMU (Input-Output Memory Management Unit, Intel VT-d or AMD-Vi) to be active before Virtualization Based Security will start. Without IOMMU, DMA attacks (Direct Memory Access via PCIe Thunderbolt, FireWire, ExpressCard) can directly read or write the hypervisor memory region, bypassing VBS isolation. Requiring IOMMU for VBS ensures that even DMA-capable physical attacks cannot access the VBS inner partition.",
                Tags = ["vbs", "iommu", "secure-boot", "dma-protection", "device-guard"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "VBS only starts when both Secure Boot and IOMMU are present. Devices without IOMMU support (older platforms without VT-d) will not activate VBS. Verify hardware IOMMU support before enforcing. Relevant for enterprise hardware that is less than 5 years old (Intel 6th Gen+ supports VT-d, AMD Ryzen 2000+ supports AMD-Vi formally).",
                ApplyOps = [RegOp.SetDword(DeviceGuardKey, "RequirePlatformSecurityFeatures", 3)],
                RemoveOps = [RegOp.DeleteValue(DeviceGuardKey, "RequirePlatformSecurityFeatures")],
                DetectOps = [RegOp.CheckDword(DeviceGuardKey, "RequirePlatformSecurityFeatures", 3)],
            },
            new TweakDef
            {
                Id = "sbdbx-enable-signed-boot-chain-policy",
                Label = "Secure Boot DBX: Enforce Complete Signed Boot Chain Policy",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets EnforceSignedBootChain=1 in the UEFI policy hive. Enforces that the complete boot chain (firmware → UEFI boot manager → Windows Boot Manager → OS loader → kernel) must be signed with certificates in the Secure Boot DB. If any component in the chain fails signature verification, the boot process halts. This policy extends beyond the basic Secure Boot enforcement to include the Windows Boot Manager's own chain-of-trust validation — ensuring that even if an attacker manages to inject a signed (but old and vulnerable) bootloader into the DB, the Windows Boot Policy prevents unsigned kernel components from loading.",
                Tags = ["secure-boot", "boot-chain", "signature", "kernel", "integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Complete signed boot chain enforced. Any unsigned component in the boot chain causes a boot failure. Dual-boot scenarios where an alternative OS boot manager is unsigned will fail. All Windows kernel modules loaded via the boot chain (boot-start drivers) must be signed. Enforce only after verifying all boot-start drivers are Microsoft-signed or WHQL-certified.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnforceSignedBootChain", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnforceSignedBootChain")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnforceSignedBootChain", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-set-db-update-policy-microsoft-only",
                Label = "Secure Boot DBX: Restrict Secure Boot DB Updates to Microsoft-Signed Only",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets DbUpdatePolicy=1 in the UEFI policy hive (value 1 = Microsoft PKI only; value 0 = Any signed, value 2 = Unsigned allowed). Restricts Secure Boot DB update signatures to Microsoft-issued certificates only. Third-party software or OEM firmware tools can inject their own signing certificates into the Secure Boot DB — an attacker who compromises an OEM's signing key can add a malicious signing certificate to the DB and then boot any malware signed with that key. Restricting DB updates to Microsoft PKI ensures the Secure Boot DB can only be extended by Microsoft-delivered Windows Update packages.",
                Tags = ["secure-boot", "db-update", "microsoft-pki", "signing-certificate", "uefi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Secure Boot DB updates restricted to Microsoft-signed packages only. OEM firmware tools, custom Secure Boot signing certificates, and Linux distribution bootloaders (shim-signed, GRUB) cannot add their own keys to the Secure Boot DB. Dual-boot and custom boot environments will be unable to add required signing certificates. Test in lab before enterprise deployment.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "DbUpdatePolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "DbUpdatePolicy")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "DbUpdatePolicy", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-prevent-boot-debug-mode",
                Label = "Secure Boot DBX: Prevent Boot-Time Kernel Debug Mode Activation",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets PreventBootDebugMode=1 in the UEFI policy hive. Blocks the ability to activate kernel debug mode (bcdedit /debug on) via boot configuration. Kernel debug mode creates a kernel debugging channel that can be used to attach a physical or kernel debugger — bypassing all user-mode security and directly reading/writing kernel memory. An attacker with physical access (or boot configuration access) who enables kernel debug and attaches a debugger can perform any system modification, including disabling security software, injecting rootkits, and extracting credentials from LSASS. Preventing debug mode activation closes this powerful boot-time bypass.",
                Tags = ["kernel-debug", "boot-debug", "bcdedit", "bypass-prevention", "physical-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Boot-time kernel debug mode cannot be activated via bcdedit. Kernel developer debugging workflows that require boot-time debugging will be blocked on this machine. Use a dedicated development VM or a separate non-policy-bound device for kernel debugging. All production enterprise workstations and servers should have debug mode blocked.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "PreventBootDebugMode", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "PreventBootDebugMode")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "PreventBootDebugMode", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-enable-pre-os-dma-protection",
                Label = "Secure Boot DBX: Enable Pre-OS DMA Protection (Thunderbolt / PCIe)",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets EnablePreBoot_DMA_protection=1 in the DeviceGuard key. Enables pre-OS DMA protection (Kernel DMA Protection) for Thunderbolt and PCIe devices. By default, DMA protection only activates after Windows fully loads. During the Windows boot phase, Thunderbolt or PCIe-attached devices have full DMA access to system memory — enabling pre-boot DMA attacks (PCILeech, Inception) against the boot process. Enabling pre-OS DMA protection activates IOMMU-based isolation during the early boot phase, before Windows loads, protecting the boot process against DMA-based physical attacks.",
                Tags = ["dma-protection", "thunderbolt", "pcie", "kernel-dma", "boot-phase"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Pre-boot DMA protection activates via IOMMU during early boot. Requires IOMMU firmware support and firmware DMA protection capability (Intel VT-d with ACS, AMD-Vi). Some older Thunderbolt peripherals that rely on DMA during boot may not function. PCIe SSD controllers that use DMA in pre-OS must be in the approved DMA driver list.",
                ApplyOps = [RegOp.SetDword(DeviceGuardKey, "EnablePreBoot_DMA_protection", 1)],
                RemoveOps = [RegOp.DeleteValue(DeviceGuardKey, "EnablePreBoot_DMA_protection")],
                DetectOps = [RegOp.CheckDword(DeviceGuardKey, "EnablePreBoot_DMA_protection", 1)],
            },
            new TweakDef
            {
                Id = "sbdbx-enable-hibernate-resume-integrity-check",
                Label = "Secure Boot DBX: Enable Hibernate Resume Integrity Verification",
                Category = "Secure Boot DBX Policy",
                Description =
                    "Sets EnableHibernateResumeIntegrity=1 in the UEFI policy hive. Enables integrity verification of the hibernation file (hiberfil.sys) during resume from hibernate. The hibernation file contains a complete snapshot of system memory at the time of hibernation — including all active keys, credentials in memory, and kernel state. If an attacker can tamper with hiberfil.sys (e.g., by booting a second OS on the same device), they can modify the kernel state or inject code that will execute when the device resumes. Integrity checking ensures that tampering with hiberfil.sys causes a safe boot failure rather than a silent compromise.",
                Tags = ["hibernate", "resume", "integrity", "hiberfil", "cold-boot"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Hibernate resume integrity checked. If hiberfil.sys has been modified since the device last entered hibernate, the resume fails and the device boots fresh. Requires BitLocker TPM protection to be active (BitLocker also seals the hiberfil.sys). Devices without full-drive encryption or TPM protection may not be able to use this feature.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnableHibernateResumeIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnableHibernateResumeIntegrity")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnableHibernateResumeIntegrity", 1)],
            },
        ];
}
