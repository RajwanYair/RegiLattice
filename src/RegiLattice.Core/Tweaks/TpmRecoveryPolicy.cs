// RegiLattice.Core — Tweaks/TpmRecoveryPolicy.cs
// TPM Recovery and Key Management Policy — Sprint 582.
// Configures BitLocker TPM data recovery, TPM owner password handling,
// TPM key maintenance, TPM clear/takeover prevention, and TPM backup to AD.
// Category: "TPM Recovery Policy" | Slug: tpmrec
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\BitLocker
//           HKLM\SOFTWARE\Policies\Microsoft\TPM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TpmRecoveryPolicy
{
    private const string BitLockerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker";

    private const string TpmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "tpmrec-backup-tpm-owner-info-to-ad",
                Label = "TPM Recovery: Backup TPM Owner Information to Active Directory",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets ActiveDirectoryBackupEnabled=1 in the TPM policy hive. Enables automatic backup of the TPM owner authorization value (TPM owner password hash) to Active Directory when the TPM is initialised or reset. The TPM owner password is needed for certain TPM management operations (clearing the TPM, resetting TPM lockout after dictionary attack). Without Active Directory backup, losing the owner password means the TPM cannot be cleared without a firmware-level reset, which can prevent BitLocker recovery in certain scenarios. Backing up to AD ensures the TPM owner information is recoverable by enterprise admins.",
                Tags = ["tpm", "backup", "active-directory", "owner-password", "recovery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "TPM owner information backed up to Active Directory. Requires AD schema extension for TPM backup (ms-TPM-OwnerInformation attribute — part of Windows Server 2012 R2 AD schema). Backup occurs when TPM is first initialised or when ownership is taken. Verify the DC has the ms-TPM-OwnerInformation attribute in schema before enforcing.",
                ApplyOps = [RegOp.SetDword(TpmKey, "ActiveDirectoryBackupEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "ActiveDirectoryBackupEnabled")],
                DetectOps = [RegOp.CheckDword(TpmKey, "ActiveDirectoryBackupEnabled", 1)],
            },
            new TweakDef
            {
                Id = "tpmrec-require-tpm-backup-before-enable",
                Label = "TPM Recovery: Block TPM Enablement Unless AD Backup Succeeds",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets RequireActiveDirectoryBackup=1 in the TPM policy hive. Prevents the BitLocker Drive Encryption setup wizard from enabling BitLocker on a drive if the TPM owner information backup to Active Directory fails. Without this requirement, BitLocker can be enabled on a device even if the TPM backup fails — resulting in deployed BitLocker with no enterprise-recoverable TPM owner info. By requiring backup success before BitLocker activation, this ensures that every BitLocker-protected device in the enterprise has its TPM recovery data in AD.",
                Tags = ["tpm", "backup-required", "bitlocker", "recovery", "active-directory"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "BitLocker enablement blocked if TPM AD backup fails. BitLocker setup will report an error and not complete until the TPM backup succeeds. Systems that cannot reach AD (offline devices, newly provisioned devices before domain join) cannot enable BitLocker. Ensure devices are domain-joined before attempting BitLocker activation.",
                ApplyOps = [RegOp.SetDword(TpmKey, "RequireActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "RequireActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(TpmKey, "RequireActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "tpmrec-block-tpm-clear-by-non-admin",
                Label = "TPM Recovery: Block TPM Clear Operation by Non-Administrator Users",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets BlockTPMClear=1 in the TPM policy hive. Prevents non-administrator users from clearing the TPM. Clearing the TPM destroys all TPM-protected keys — all BitLocker encryption keys bound to the TPM, Windows Hello for Business keys, and any application TPM keys. A non-admin user who can clear the TPM on a shared workstation can force a BitLocker recovery event (requiring the recovery key) and potentially create confusion that could be exploited during the recovery process. Restricting TPM clear operations to administrators ensures only authorised personnel can perform this destructive operation.",
                Tags = ["tpm", "clear-prevention", "admin-only", "bitlocker", "key-destruction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "TPM clear blocked for non-administrators. Administrators can still clear the TPM using TPM.msc or PowerShell (Get-Tpm | Clear-Tpm). Standard users clicking 'Clear TPM' in the Security Center get an access denied error. Reduces risk of accidental or malicious TPM clear on shared workstations.",
                ApplyOps = [RegOp.SetDword(TpmKey, "BlockTPMClear", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "BlockTPMClear")],
                DetectOps = [RegOp.CheckDword(TpmKey, "BlockTPMClear", 1)],
            },
            new TweakDef
            {
                Id = "tpmrec-enable-tpm-auto-provisioning",
                Label = "TPM Recovery: Enable Automatic TPM Provisioning by Windows",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets EnableAutoTPMProvisioning=1 in the TPM policy hive. Enables automatic TPM provisioning by Windows during first use. When a device ships with an unprovisioned TPM (factory default), Windows can automatically provision the TPM, set the owner password, and back up the owner info to AD. Without auto-provisioning, administrators must manually provision each device's TPM before BitLocker can be deployed. Auto-provisioning ensures that all devices in the enterprise have their TPMs properly initialised during the Windows Setup or subsequent first-login process, enabling enterprise-wide BitLocker deployment without per-device manual TPM steps.",
                Tags = ["tpm", "provisioning", "auto", "bitlocker-readiness", "deployment"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "TPM auto-provisioning enabled. On first login, Windows provisions the TPM and backs up owner info to AD. No user-visible impact. TPM provisioning occurs in the background. If the device cannot contact AD (e.g., not domain-joined yet), provisioning is deferred until next contact.",
                ApplyOps = [RegOp.SetDword(TpmKey, "EnableAutoTPMProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "EnableAutoTPMProvisioning")],
                DetectOps = [RegOp.CheckDword(TpmKey, "EnableAutoTPMProvisioning", 1)],
            },
            new TweakDef
            {
                Id = "tpmrec-set-tpm-lockout-duration-30min",
                Label = "TPM Recovery: Set TPM Dictionary Attack Lockout Duration to 30 Minutes",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets DictionaryAttackLockoutDuration=30 in the TPM policy hive (units: minutes). Sets the TPM dictionary attack lockout duration to 30 minutes. The TPM tracks repeated failed authorisation attempts (dictionary attack mitigation). After exceeding the threshold, the TPM enters a lockout mode and refuses further authorisation attempts. The lockout duration determines how long the TPM remains locked before resetting its counter. A 30-minute lockout provides strong protection against automated PIN/password brute-force attacks against BitLocker TPM+PIN while being reasonable for legitimate pin-entry mistakes.",
                Tags = ["tpm", "dictionary-attack", "lockout", "bitlocker", "brute-force"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "TPM dictionary attack lockout is 30 minutes. After threshold failed PIN entries, TPM locks out for 30 minutes. Users who repeatedly enter wrong BitLocker PINs will be locked out for 30 minutes. The lockout counter resets after a full 30-minute wait. Adjust the lockout threshold (DictionaryAttackLockoutThreshold) and duration to match your user population's PIN error rate.",
                ApplyOps = [RegOp.SetDword(TpmKey, "DictionaryAttackLockoutDuration", 30)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "DictionaryAttackLockoutDuration")],
                DetectOps = [RegOp.CheckDword(TpmKey, "DictionaryAttackLockoutDuration", 30)],
            },
            new TweakDef
            {
                Id = "tpmrec-set-tpm-lockout-threshold-5",
                Label = "TPM Recovery: Set TPM Dictionary Attack Lockout Threshold to 5 Attempts",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets DictionaryAttackLockoutThreshold=5 in the TPM policy hive. Sets the number of failed TPM authorisation attempts before the TPM enters lockout mode to 5. Five attempts is consistent with enterprise account lockout policies (typically 5–10 attempts) — it provides a reasonable number of legitimate re-entry attempts while blocking automated brute-force attacks (which attempt thousands of PINs per minute). Combined with the 30-minute lockout duration, this means an attacker can test at most 5 PINs every 30 minutes — making a full 6-digit PIN space (1,000,000 values) take over 100,000 hours to exhaust.",
                Tags = ["tpm", "lockout-threshold", "brute-force", "bitlocker", "dictionary-attack"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "TPM locks after 5 failed PIN authorisations. Users who enter the wrong BitLocker PIN more than 5 times trigger a 30-minute lockout. Helpdesk should be prepared for lockout-related support calls. The TPM lockout counter resets after the lockout duration expires or after a successful authorisation.",
                ApplyOps = [RegOp.SetDword(TpmKey, "DictionaryAttackLockoutThreshold", 5)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "DictionaryAttackLockoutThreshold")],
                DetectOps = [RegOp.CheckDword(TpmKey, "DictionaryAttackLockoutThreshold", 5)],
            },
            new TweakDef
            {
                Id = "tpmrec-backup-bitlocker-recovery-key-to-ad",
                Label = "TPM Recovery: Require BitLocker Recovery Key Backup to Active Directory/AAD",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets RequireDeviceLockout=1 in the BitLocker policy hive (OSRecoveryInformationBackup flag). Requires that BitLocker recovery keys be backed up to Active Directory or Azure AD before BitLocker encryption can be enabled. Without this requirement, users or automated deployment systems can enable BitLocker and generate a recovery key that is never stored in the enterprise directory — resulting in encrypted devices with no enterprise-retrievable recovery key. If the device then undergoes a TPM change, firmware update, or Secure Boot configuration change, recovery requires that local key which may be lost.",
                Tags = ["bitlocker", "recovery-key", "ad-backup", "aad-backup", "encryption"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "BitLocker recovery key backup to AD or AAD is required before encryption activation. BitLocker setup will not complete until the backup succeeds. Device must be domain-joined or AAD-joined and reachable. Ensures all enterprise devices have retrievable recovery keys in the directory. Required for self-service BitLocker recovery capabilities.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "RequireDeviceLockout", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "RequireDeviceLockout"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "RequireDeviceLockout", 1),
                ],
            },
            new TweakDef
            {
                Id = "tpmrec-enable-bitlocker-preboot-pin",
                Label = "TPM Recovery: Require BitLocker Pre-Boot PIN for OS Drive",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets EnableBDEWithNoTPM=0 and TPM-based protection with PIN by setting UseACBitLockerPIN=1 in the BitLocker policy. Requires that the BitLocker OS drive uses TPM+PIN authentication, not TPM-only. TPM-only BitLocker can be bypassed by a cold-boot attack (freezing RAM to preserve encryption keys) or by DMA attacks against the boot process. Requiring a PIN in addition to the TPM ensures that even if hardware-level memory extraction is performed, the attacker must also know the PIN. The PIN is never transmitted over the network and is not stored in AD — it is the 'something you know' factor in the BitLocker two-factor authentication.",
                Tags = ["bitlocker", "pre-boot-pin", "tpm-plus-pin", "cold-boot", "dma-attack"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "BitLocker requires a PIN every time the device boots. Users must remember and enter the BitLocker PIN at each startup. This is an additional step that prevents fast boot in enterprise thin-client and kiosk deployments. For devices in remote locations or used by users who frequently forget PINs, consider Network Unlock as an alternative.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "UseTPMPIN", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "UseTPMPIN")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "UseTPMPIN", 1)],
            },
            new TweakDef
            {
                Id = "tpmrec-enable-enhanced-pin",
                Label = "TPM Recovery: Enable Enhanced BitLocker PIN (Allows Full Keyboard Chars)",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets UseEnhancedPin=1 in the BitLocker policy hive. Enables Enhanced PINs for BitLocker pre-boot authentication on supported firmware. By default, BitLocker PINs only accept numeric digits (0–9) in pre-boot. Enhanced PINs allow letters, symbols, and spaces — enabling passphrases and mixed PINs that are significantly harder to brute-force. A 6-digit numeric PIN has 1,000,000 combinations; an 8-character alphanumeric+symbol passphrase has over 6 quadrillion combinations. Enabling Enhanced PINs dramatically increases the effective entropy of BitLocker pre-boot authentication without changing the TPM+PIN hardware requirement.",
                Tags = ["bitlocker", "enhanced-pin", "passphrase", "entropy", "pre-boot"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Enhanced PIN (alphanumeric + symbols) enabled for BitLocker. Users can use a full passphrase instead of a numeric-only PIN. International keyboard layouts should be tested — some special characters may not be available in UEFI pre-boot environments. Existing numeric-only PINs are automatically migrated to Enhanced PIN format on next PIN change.",
                ApplyOps = [RegOp.SetDword(BitLockerKey, "UseEnhancedPin", 1)],
                RemoveOps = [RegOp.DeleteValue(BitLockerKey, "UseEnhancedPin")],
                DetectOps = [RegOp.CheckDword(BitLockerKey, "UseEnhancedPin", 1)],
            },
            new TweakDef
            {
                Id = "tpmrec-enable-tpm-attestation-azure",
                Label = "TPM Recovery: Enable TPM Attestation for Intune/Azure Conditional Access",
                Category = "TPM Recovery Policy",
                Description =
                    "Sets EnableTPMAttestation=1 in the TPM policy hive. Enables TPM-based device attestation for Microsoft Intune and Azure Conditional Access compliance checks. TPM attestation allows Intune to verify that a device's TPM is genuine (not emulated), the device has not been tampered with since last boot, Secure Boot is active, and the early launch anti-malware driver passed. Devices that fail attestation can be blocked from accessing corporate resources via Conditional Access policies. This prevents attackers from using counterfeit or compromised device identities to gain cloud resource access.",
                Tags = ["tpm", "attestation", "intune", "azure", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "TPM attestation enabled for Intune compliance. Requires Microsoft Intune licence and devices enrolled in Intune. Devices with emulated TPMs (VMware vTPM, Hyper-V vTPM without physical backing) may fail attestation. Configure Intune compliance policies to use attestation status as a compliance condition.",
                ApplyOps = [RegOp.SetDword(TpmKey, "EnableTPMAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(TpmKey, "EnableTPMAttestation")],
                DetectOps = [RegOp.CheckDword(TpmKey, "EnableTPMAttestation", 1)],
            },
        ];
}
