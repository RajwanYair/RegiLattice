// RegiLattice.Core — Tweaks/TrustedLaunchPolicy.cs
// Trusted Launch (vTPM + Secure Boot + vNUMA) VM Security Policy — Sprint 584.
// Configures Azure Trusted Launch, vTPM provisioning, Secure Boot for VMs,
// UEFI variables for Hyper-V guest VMs, and Measured Boot reporting.
// Category: "Trusted Launch Policy" | Slug: trlnch
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\UEFI
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\Hyper-V

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TrustedLaunchPolicy
{
    private const string UefiPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI";

    private const string HyperVPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Hyper-V";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "trlnch-enable-hyperv-vm-secure-boot",
                Label = "Trusted Launch: Enable Secure Boot for Hyper-V Generation 2 VMs",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets VmSecureBootEnabled=1 in the Hyper-V policy key. Enforces Secure Boot on all Generation 2 Hyper-V virtual machines created or managed by this host. Secure Boot for VMs uses a virtualised Secure Boot DB/DBX to validate the VM's boot chain — the virtual firmware (UEFI), virtual boot manager, and VM guest OS loader must all be signed. This prevents rootkit-class malware from persisting across a VM save/restore or snapshot operation inside a Hyper-V host by ensuring the VM's boot chain is integrity-validated on every boot.",
                Tags = ["hyperv", "vm-secure-boot", "generation-2", "guest-integrity", "uefi-vm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Secure Boot enforced for Hyper-V Gen2 VMs. VMs using unsigned bootloaders (e.g., custom Linux distributions with non-shim bootloaders, custom kernel builds, or legacy BIOS-mode VM configurations) will fail to boot. Gen2 VMs must be configured with the Microsoft UEFI CA or Linux UEFI CA in the virtual Secure Boot DB.",
                ApplyOps = [RegOp.SetDword(HyperVPolicyKey, "VmSecureBootEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(HyperVPolicyKey, "VmSecureBootEnabled")],
                DetectOps = [RegOp.CheckDword(HyperVPolicyKey, "VmSecureBootEnabled", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-hyperv-vtpm-provisioning",
                Label = "Trusted Launch: Enable Hyper-V vTPM 2.0 for All Generation 2 VMs",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets VmVirtualTPMEnabled=1 in the Hyper-V policy key. Enables virtual TPM 2.0 for all Generation 2 Hyper-V VMs. A vTPM provides guest VMs with a virtualised TPM that supports all TPM 2.0 operations — BitLocker encryption keyed to the vTPM, Windows Hello for Business, and TPM-backed account protection in the guest OS. The vTPM is backed by the host's physical TPM via Hyper-V's key storage driver. VMs with vTPM can use BitLocker and attestation services, ensuring that guest VM data is encrypted even if the host storage is compromised.",
                Tags = ["hyperv", "vtpm", "tpm-2.0", "bitlocker-vm", "attestation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "vTPM 2.0 enabled for Hyper-V Gen2 VMs. Requires host physical TPM 2.0. VM configs that do not have vTPM configuration in their XML will need to be updated (Edit-VMSecurity). Guest VM BitLocker encrypted with vTPM is backed by the host TPM — if the host is rebuilt, vTPM keys must be backed up or BitLocker recovery keys must be stored in AD.",
                ApplyOps = [RegOp.SetDword(HyperVPolicyKey, "VmVirtualTPMEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(HyperVPolicyKey, "VmVirtualTPMEnabled")],
                DetectOps = [RegOp.CheckDword(HyperVPolicyKey, "VmVirtualTPMEnabled", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-measured-boot-reporting",
                Label = "Trusted Launch: Enable Measured Boot Attestation Reporting",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets EnableMeasuredBootReporting=1 in the UEFI policy hive. Enables the Windows Health Attestation Service (HAS) to report Measured Boot results — the chain of TPM PCR measurements recorded during each boot. When reporting is enabled, the device regularly submits a Measured Boot report to the Windows Health Attestation Service, which compares the PCR measurements against known-good baselines. Microsoft Intune and Microsoft Endpoint Manager can use these reports to detect anomalous boot states that may indicate a rootkit or modified kernel component.",
                Tags = ["measured-boot", "health-attestation", "tpm-pcr", "intune", "reporting"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Measured Boot PCR data reported to Health Attestation Service. Requires Windows Health Attestation Service connectivity (has.spserv.microsoft.com). Data transmitted includes boot measurement logs and PCR values — no PII. Reports are used for Intune Conditional Access compliance decisions. Reports transmitted on device startup and periodically.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnableMeasuredBootReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnableMeasuredBootReporting")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnableMeasuredBootReporting", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-block-uefi-setupmode",
                Label = "Trusted Launch: Block UEFI Setup Mode (Prevent DB Key Replacement)",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets PreventUEFISetupMode=1 in the UEFI policy hive. Prevents the device from entering UEFI Setup Mode. Setup Mode is a state where the Secure Boot key databases (PK, KEK, DB, DBX) can be replaced without signature verification — used during initial platform setup. In Setup Mode, any user with physical access to the machine can clear the existing keys, install their own PK, and create a custom Secure Boot chain that trusts only their malware. Preventing Setup Mode entry from an OS-controlled policy ensures that even an administrator who can reach the UEFI settings cannot wipe the production Secure Boot keys.",
                Tags = ["uefi", "setup-mode", "pk", "secure-boot-keys", "physical-attack"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "UEFI Setup Mode blocked by OS policy. Cannot undo without physical UEFI reset via CMOS clear or recovery firmware mode. If the production Secure Boot PK needs to be updated due to compromise, physical access to the UEFI reset procedure is required. Only deploy in high-security environments with controlled physical access and documented recovery procedures.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "PreventUEFISetupMode", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "PreventUEFISetupMode")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "PreventUEFISetupMode", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-credential-guard-vbs",
                Label = "Trusted Launch: Enable Credential Guard via Virtualization Based Security",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets LsaCfgFlags=1 in the DeviceGuard key (value 1 = Enabled without UEFI lock; value 2 = Enabled with UEFI lock). Enables Windows Credential Guard — which moves LSASS (Local Security Authority Subsystem Service) and its credential storage into a VBS-protected isolated partition. LSASS contains NTLM password hashes, Kerberos tickets, and other credentials for authenticated users. Tools like Mimikatz that dump credentials from LSASS cannot access the isolated LSASS memory — all credential material is inside the VBS hypervisor partition, inaccessible to the normal OS kernel. This is one of the most impactful credential theft mitigations available.",
                Tags = ["credential-guard", "vbs", "lsass", "mimikatz", "credential-theft"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Credential Guard enabled (VBS-backed LSASS isolation). NTLM v2 hashes and Kerberos tickets in LSASS cannot be extracted by Mimikatz or similar tools. Requires Hyper-V platform (VBS prerequisite). Some legacy applications that access LSASS directly (certain smart-card middleware, third-party SSO) may break — test before enterprise rollout. Digest authentication is disabled when Credential Guard is active.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-memory-integrity-hvci",
                Label = "Trusted Launch: Enable Memory Integrity (Hypervisor-Protected Code Integrity)",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets HypervisorEnforcedCodeIntegrity=1 in the DeviceGuard key. Enables Memory Integrity (HVCI — Hypervisor-Protected Code Integrity), which uses VBS to run Kernel Mode Code Integrity (KMCI) checks inside the hypervisor. HVCI prevents unsigned or malicious kernel drivers from loading — even kernel exploits that normally allow unsigned kernel code injection are blocked because the kernel memory is managed from inside the hypervisor and cannot be directly written by the kernel itself. HVCI is one of the strongest kernel-level attack mitigations in Windows.",
                Tags = ["hvci", "memory-integrity", "kernel-protection", "unsigned-driver", "vbs"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "HVCI (Memory Integrity) enabled. Unsigned or improperly signed kernel drivers cannot load. Some older third-party drivers (hardware vendor utilities, older antivirus kernel drivers, certain network drivers) are not HVCI-compatible and will prevent this from enabling. Use Device Manager and Windows Security Center to identify incompatible drivers before enabling. Incompatible drivers must be updated or removed.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                        "Enabled",
                        1
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                        "Enabled"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                        "Enabled",
                        1
                    ),
                ],
            },
            new TweakDef
            {
                Id = "trlnch-enable-system-guard-secure-launch",
                Label = "Trusted Launch: Enable System Guard Secure Launch (Runtime Firmware Protection)",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets Enabled=1 in the SystemGuard\\Scenarios\\SystemGuard\\Enabled key. Enables System Guard Secure Launch (also known as DRTM — Dynamic Root of Trust for Measurement). DRTM uses Intel TXT or AMD SKINIT hardware extensions to establish a clean, verifiable root of trust at runtime, independently of the system firmware. This protects against firmware compromise — even if the UEFI firmware is modified by a sophisticated firmware implant, DRTM creates a new root of trust without relying on the compromised firmware. System Guard then measures the Windows kernel startup environment from this trusted state.",
                Tags = ["system-guard", "secure-launch", "drtm", "intel-txt", "firmware-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "System Guard Secure Launch (DRTM) enabled. Requires Intel TXT or AMD SKINIT hardware support. Intel TXT requires TXT-enabled CPU (most Intel vPro platforms) and TXT-enabled BIOS settings. On unsupported hardware, setting this has no effect. Provides the strongest available protection against firmware-level compromise.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SystemGuard\Scenarios\SystemGuard", "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SystemGuard\Scenarios\SystemGuard", "Enabled")],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SystemGuard\Scenarios\SystemGuard", "Enabled", 1),
                ],
            },
            new TweakDef
            {
                Id = "trlnch-set-vbs-launch-type-auto",
                Label = "Trusted Launch: Set VBS to Auto-Enable (All Platforms — No Manual Init Required)",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets EnableVirtualizationBasedSecurity=1 in the DeviceGuard key. Enables Virtualization Based Security (VBS) globally. VBS is the hypervisor-based security infrastructure that underpins Credential Guard, HVCI, System Guard, and Secure Launch. Enabling VBS creates a secured kernel environment isolated from the normal OS. This is the prerequisite setting that enables all other VBS-dependent features. On supported hardware (Hyper-V capable, Secure Boot, IOMMU), VBS activates on boot and the Hyper-V hypervisor is loaded to create the VTL1 (lower Virtual Trust Level) secure partition.",
                Tags = ["vbs", "virtualization-security", "hyperv", "vtl", "device-guard"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "VBS enabled. Hyper-V hypervisor is loaded even on editions that normally do not use Hyper-V (Home, Pro). On some hardware with Hyper-V driver incompatibilities, VBS may cause boot delays or compatibility issues. VBS is enabled by default on modern Windows 11 hardware; this setting ensures it is also enforced via policy.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
                ],
            },
            new TweakDef
            {
                Id = "trlnch-enable-hyperv-vm-encrypted-state",
                Label = "Trusted Launch: Enable Encrypted State for Hyper-V VMs (Shielded VM Support)",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets VMEncryptedStateEnabled=1 in the Hyper-V policy key. Enables the encrypted state feature for Hyper-V VMs — part of the Shielded VM infrastructure. Shielded VMs (Generation 2 only) encrypt the VM state files (memory snapshot, save states) with BitLocker and use a vTPM sealed key. Without encrypted state, Hyper-V VM save files and checkpoint state files stored on-disk are in plaintext — an attacker who gains access to the Hyper-V host storage can extract credentials from a saved VM snapshot using memory analysis tools. Encrypted VM state prevents this class of offline VM memory forensics.",
                Tags = ["hyperv", "shielded-vm", "encrypted-state", "vm-snapshot", "vTPM"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "VM save state and checkpoint files are encrypted. Requires vTPM enabled on VMs. Slightly higher I/O overhead during save/restore operations due to encryption. Shielded VM policy and Host Guardian Service (HGS) infrastructure is recommended for full Shielded VM deployment but is not required for encrypted state only.",
                ApplyOps = [RegOp.SetDword(HyperVPolicyKey, "VMEncryptedStateEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(HyperVPolicyKey, "VMEncryptedStateEnabled")],
                DetectOps = [RegOp.CheckDword(HyperVPolicyKey, "VMEncryptedStateEnabled", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-block-kernel-debug-vtl0",
                Label = "Trusted Launch: Block Kernel Debugging From VTL0 (Normal OS) to VTL1 (Secure World)",
                Category = "Trusted Launch Policy",
                Description =
                    "Sets BlockDebuggerForVTL1=1 in the DeviceGuard key. Prevents normal OS (VTL0) kernel debugger sessions from accessing the VTL1 (Secure World) memory or state. In a VBS environment, VTL0 contains the normal Windows kernel and VTL1 contains the secure kernel (Credential Guard, HVCI agent). A kernel debugger attached to VTL0 should not be able to read VTL1 memory — but without this protection, certain kernel debug commands can inadvertently expose VTL boundary-crossing information. Blocking this ensures a hard separation between debug sessions and the secure world.",
                Tags = ["vbs", "vtl1", "kernel-debug", "secure-world", "isolation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "VTL0 kernel debugger cannot access VTL1 state. Normal WinDbg kernel debug sessions are unaffected for debugging regular kernel (VTL0) operations. VTL1-specific debugging requires a separate secure kernel debugger connection. No user-visible impact.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "BlockDebuggerForVTL1", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "BlockDebuggerForVTL1")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "BlockDebuggerForVTL1", 1)],
            },
        ];
}
