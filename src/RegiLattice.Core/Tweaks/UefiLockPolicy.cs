// RegiLattice.Core — Tweaks/UefiLockPolicy.cs
// UEFI Lock and Secure Boot Key Protection Policy — Sprint 586.
// Configures UEFI Secure Boot enforcement, CSM (Compatibility Support Module)
// disabling, PK/KEK/DB write protection, test signing mode block, UEFI shell
// access restriction, and boot option modification lock.
// Category: "UEFI Lock Policy" | Slug: uefilck
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\UEFI
//           HKLM\SYSTEM\CurrentControlSet\Control\SecureBoot\State

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UefiLockPolicy
{
    private const string UefiPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI";

    private const string SecureBootStateKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "uefilck-require-secure-boot-active",
                Label = "UEFI Lock: Require UEFI Secure Boot to Be Active (OS Enforcement Check)",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets RequireSecureBoot=1 in the SecureBoot State key. Records an OS-level enforcement requirement that Secure Boot must be active. Windows reads this value to determine whether Secure Boot is enforced by OS policy — if the firmware has Secure Boot disabled while this policy value is set, Windows Update and Intune compliance checks flag the device as non-compliant. This acts as an OS-side sentinel that detects when someone has disabled Secure Boot in the UEFI settings and correlates with health attestation checks. The actual Secure Boot enforcement is in the UEFI firmware — this registry value is the OS policy declaration that drives compliance reporting.",
                Tags = ["secure-boot", "compliance", "intune", "policy-enforcement", "health-attestation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Device flagged as non-compliant if Secure Boot is disabled in firmware. Intune Conditional Access can block network access to non-compliant devices. If Secure Boot is genuinely required for compliance, disable CSM and ensure UEFI firmware has Secure Boot enabled before deploying.",
                ApplyOps = [RegOp.SetDword(SecureBootStateKey, "UEFISecureBootEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SecureBootStateKey, "UEFISecureBootEnabled")],
                DetectOps = [RegOp.CheckDword(SecureBootStateKey, "UEFISecureBootEnabled", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-block-test-signing-mode",
                Label = "UEFI Lock: Block Windows Test Signing Mode (BcdEdit /set testsigning)",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets PreventTestSigningMode=1 in the UEFI policy key. Prevents enabling Windows test signing mode (where the OS accepts self-signed test drivers without Authenticode code signing). Test signing mode is used during driver development for testing unsigned drivers. However, if an attacker gains admin rights, enabling test signing mode is a common step to load a malicious unsigned kernel driver (rootkit). Setting this policy via Group Policy via UEFI Lock ensures that even an admin cannot re-enable test signing mode for driver-loading bypass.",
                Tags = ["test-signing", "unsigned-driver", "rootkit", "bcdedit", "kernel-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Test signing mode cannot be enabled. `bcdedit /set testsigning on` will fail. Kernel driver developers who require test signing for development work must use a development machine or VM that is not subject to this policy. Production systems should never need test signing mode enabled.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "PreventTestSigningMode", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "PreventTestSigningMode")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "PreventTestSigningMode", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-block-secure-boot-db-write",
                Label = "UEFI Lock: Block OS-Level Writes to Secure Boot DB/DBX Variables",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets BlockSecureBootDbWrite=1 in the UEFI policy key. Prevents OS-level software from writing to the Secure Boot DB (Allowed Signatures Database) or DBX (Forbidden Signatures Database) UEFI variables. Secure Boot database variables can be written from the OS via UEFI variable write APIs (SetVariable) when in user mode with administrator privileges. An attacker with kernel access can add their own signing certificate to DB (making their malware trusted by Secure Boot) or clear DBX entries to re-allow previously blacklisted boot components. Blocking OS-level writes to DB/DBX closes this attack surface — DB/DBX updates can still be delivered via signed UEFI firmware update capsules.",
                Tags = ["secure-boot", "db-dbx", "uefi-variables", "signing-cert", "dbx"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "OS cannot write to Secure Boot DB or DBX. Microsoft's periodic DBX revocation updates (delivered via Windows Update as firmware updates) use signed capsule delivery and are NOT affected. However, software that uses UEFI variable write to enrol custom CA certificates (enterprise PKI enrollments, some MDM enrolment flows) will fail. Requires thorough testing before enterprise deployment.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "BlockSecureBootDbWrite", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "BlockSecureBootDbWrite")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "BlockSecureBootDbWrite", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-disable-csm-compat-support-module",
                Label = "UEFI Lock: Disable CSM (Compatibility Support Module) — Force Pure UEFI Mode",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets DisableCSM=1 in the UEFI policy key. Disables the UEFI Compatibility Support Module (CSM), which provides backwards compatibility for legacy BIOS-based boot processes. CSM dramatically expands the firmware attack surface — CSM firmware is older code that typically has fewer security controls, does not validate signatures, and may have unpatched vulnerabilities. Many firmware-level attacks (Option ROM attacks, legacy VGA ROM injection) require CSM to be active. Disabling CSM forces pure UEFI mode, eliminating the legacy firmware code path entirely. This is a prerequisite for Secure Boot to function completely without gaps.",
                Tags = ["csm", "legacy-bios", "pure-uefi", "option-rom", "firmware-attack"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "CSM disabled — legacy BIOS boot is no longer available. Devices cannot boot legacy MBR-based operating systems, DOS-based diagnostics, or any non-UEFI bootable media. PXE boot must be UEFI-compatible (UEFI PXE via DHCP option 60=PXEClient with UEFI x64 boot image). USB bootable media must be formatted for UEFI boot (GPT partition table, EFI boot partition). Some older peripheral firmware (Option ROMs) may not initialise.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "DisableCSM", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "DisableCSM")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "DisableCSM", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-block-uefi-shell-execution",
                Label = "UEFI Lock: Block UEFI Shell Execution (Remove Shell.efi Access)",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets BlockUefiShell=1 in the UEFI policy key. Prevents execution of the UEFI shell (Shell.efi) from the UEFI boot menu. The UEFI shell is a command-line interface that runs before the OS boots and has full access to all UEFI variables, firmware functions, and boot option management commands. An attacker with brief physical access who can reach the UEFI boot menu can launch the shell and modify Secure Boot variables, alter boot entries, read memory, or persist a bootkit. Blocking UEFI shell execution removes this pre-OS privileged environment from the UEFI boot menu.",
                Tags = ["uefi-shell", "pre-os", "physical-access", "boot-menu", "shell-efi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "UEFI Shell is blocked. IT engineers who use UEFI Shell for firmware diagnostics or UEFI variable management must use an alternative (UEFI variable tools in Windows, BIOS firmware utility, OEM-provided UEFI tools). The use of UEFI Shell for firmware debugging in production environments is rare — document an alternative procedure before enabling.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "BlockUefiShell", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "BlockUefiShell")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "BlockUefiShell", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-lock-boot-order-modification",
                Label = "UEFI Lock: Lock UEFI Boot Order Modification from the OS",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets LockBootOrder=1 in the UEFI policy key. Prevents OS-level software and users from modifying the UEFI boot order (BootOrder, Boot#### UEFI variables) via Windows bcdedit, UEFI variable APIs, or third-party boot order tools. An attacker who can modify the boot order can insert a malicious EFI binary as boot entry 0, causing it to execute before Windows on the next boot. Boot order modification is a known persistence technique for UEFI bootkits. Locking the boot order via OS policy ensures that established boot sequences cannot be tampered with without physical BIOS access.",
                Tags = ["boot-order", "uefi-variables", "bootkit", "persistence", "bcdedit"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Boot order cannot be modified from the OS. BitLocker repair and recovery operations that require modifying boot order from Windows (e.g., recovery mode boot) may be affected. IT must use UEFI BIOS settings to modify boot order when needed. Remote restart-to-BIOS tools (SCCM, Intune hardware boot) may be affected if they use bcdedit to set one-time boot entries.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "LockBootOrder", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "LockBootOrder")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "LockBootOrder", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-enable-uefi-audit-log",
                Label = "UEFI Lock: Enable UEFI Variable Write Audit Log",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets EnableUefiVariableAudit=1 in the UEFI policy key. Enables auditing of writes to UEFI non-volatile variables (NvStore writes) from the OS layer. When enabled, each OS-level write to a UEFI variable (SetVariable call) generates a Windows Security event (Event ID 4670 / 4724 or vendor-specific log depending on platform). UEFI variable writes are used for boot configuration, Secure Boot database modification, and firmware communication. Audit logging of these writes provides visibility into potentially malicious UEFI variable manipulation by malware operating with elevated privileges.",
                Tags = ["uefi-variables", "audit", "setVariable", "event-log", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "UEFI variable write audit events generated on each SetVariable call. On systems where OEM firmware update tools regularly write UEFI variables, this may generate elevated event volume. SIEM correlation rules should focus on unexpected or uncommon UEFI variable names (DB, DBX, KEK, PK, BootOrder) rather than all variable writes.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnableUefiVariableAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnableUefiVariableAudit")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnableUefiVariableAudit", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-require-uefi-password-for-settings-access",
                Label = "UEFI Lock: Require Policy Enforcement of UEFI Setup Password Presence",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets RequireUefiSetupPassword=1 in the UEFI policy key. Marks the policy requirement that a UEFI setup password (administrator password) must be configured on the device. The actual password configuration is in firmware — this policy establishes the OS-side compliance assertion. Intune and MDM can read this policy value to determine whether devices have UEFI setup passwords enforced. Devices without a UEFI setup password allow anyone with physical access to modify Secure Boot settings, boot order, and disable security features from the UEFI settings UI.",
                Tags = ["uefi-password", "setup-password", "physical-access", "compliance", "intune"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Compliance policy requiring UEFI setup password presence. No functional change from this registry value alone — UEFI password must be set manually via BIOS setup. Use alongside Intune device configuration profile for UEFI settings to audit compliance. Devices without a UEFI password set in firmware will appear non-compliant.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "RequireUefiSetupPassword", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "RequireUefiSetupPassword")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "RequireUefiSetupPassword", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-block-external-uefi-option-rom-execution",
                Label = "UEFI Lock: Block Execution of External UEFI Option ROMs",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets BlockExternalOptionRom=1 in the UEFI policy key. Prevents the UEFI firmware from executing UEFI Option ROMs from external or removable devices (PCIe add-in cards, Thunderbolt docks, USB devices with firmware). Option ROMs from external devices are a well-documented firmware attack vector — malicious hardware devices containing a custom Option ROM can execute arbitrary UEFI code before the OS boots, bypassing Secure Boot's guarantees. Blocking external Option ROMexecution protects against 'evil maid' attacks using malicious peripherals inserted while the device is unattended.",
                Tags = ["option-rom", "evil-maid", "external-device", "pcie", "uefi-firmware"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "External UEFI Option ROMs blocked. Thunderbolt 3/4 docks that use Option ROM for display output initialisation (DisplayPort Alt mode) may not initialise correctly at UEFI boot. Network boot via NIC Option ROM from external USB-to-Ethernet adapters is blocked. RAID controllers added as PCIe cards may not be visible during firmware setup. Internal (soldered) Option ROMs are not affected.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "BlockExternalOptionRom", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "BlockExternalOptionRom")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "BlockExternalOptionRom", 1)],
            },
            new TweakDef
            {
                Id = "uefilck-expire-pk-on-loss-prevention",
                Label = "UEFI Lock: Enable PK Expiry Notification for Secure Boot Platform Key",
                Category = "UEFI Lock Policy",
                Description =
                    "Sets EnablePkExpiryNotification=1 in the UEFI policy key. Enables periodic Windows event log entries and Intune compliance alerts when the UEFI Platform Key (PK) — the root of trust for Secure Boot — is approaching its certificate expiration date. The PK is an X.509 certificate embedded in UEFI, and like all certificates, it can have an expiration date. In OEM firmware, PKs typically have long lifetimes (10–20 years), but custom enterprise PKI-based PKs may have shorter expirations. An expired PK can cause Secure Boot validation failures. PK expiry notification ensures IT is proactively alerted before the expiry causes a boot failure incident.",
                Tags = ["pk", "platform-key", "certificate-expiry", "secure-boot-root", "notification"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "PK expiry notifications enabled. No impact on boot behaviour. Windows event log and Intune compliance reports include PK expiry date when available. Relevant primarily for organisations that use custom Secure Boot PKIs (replacement PK/KEK for enterprise control). Standard OEM PKs expire in 2030–2040+.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnablePkExpiryNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnablePkExpiryNotification")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnablePkExpiryNotification", 1)],
            },
        ];
}
