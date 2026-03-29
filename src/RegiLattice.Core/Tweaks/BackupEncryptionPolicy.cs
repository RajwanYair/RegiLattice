// RegiLattice.Core — Tweaks/BackupEncryptionPolicy.cs
// Backup Encryption and Key Management Policy — Sprint 591.
// Configures encryption requirements for backup sets, protected
// key management, algorithm selection, and BitLocker recovery
// information archival for backup infrastructure.
// Category: "Backup Encryption Policy" | Slug: bkpenc
// Registry: HKLM\SOFTWARE\Policies\Microsoft\FVE
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\Backup\Server

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BackupEncryptionPolicy
{
    private const string FveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

    private const string BackupServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Server";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "bkpenc-require-bitlocker-os-drive",
                Label = "Backup Encryption: Require BitLocker Encryption on OS Drive Before Backup",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets OSRequireActiveEncryption=1 in the BitLocker FVE policy key. Requires that the OS drive is encrypted with BitLocker before a system backup job can proceed. An unencrypted OS drive backup creates a physical-access vulnerability — the backup media (USB, NAS, cloud) contains a fully readable copy of the OS and all its files. If the backup media is stolen or improperly secured, all data on the device is exposed. Requiring BitLocker on the OS drive ensures that backup images of the OS partition are also effectively encrypted at the data level.",
                Tags = ["bitlocker", "os-drive", "encryption", "backup-prerequisite", "fve"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "System backup requires BitLocker OS drive encryption. Devices without BitLocker enabled on the OS drive cannot run the backup job — backup job fails with an encryption prerequisite error. Ensure BitLocker is deployed fleet-wide before enabling this requirement. Can be enforced alongside Intune's BitLocker compliance policy.",
                ApplyOps = [RegOp.SetDword(FveKey, "OSRequireActiveEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "OSRequireActiveEncryption")],
                DetectOps = [RegOp.CheckDword(FveKey, "OSRequireActiveEncryption", 1)],
            },
            new TweakDef
            {
                Id = "bkpenc-require-bitlocker-fixed-drives",
                Label = "Backup Encryption: Require BitLocker on Fixed Data Drives",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets FDVRequireEncryption=1 in the BitLocker FVE policy key. Requires that all fixed (non-removable) data drives are encrypted with BitLocker. Secondary data drives (D:, E:) used for data storage often contain the organisation's most sensitive data — project files, database files, archive documents. Without BitLocker on data drives, stealing the drive (or the laptop) gives physical access to all stored data without any authentication barrier. Requiring BitLocker on all fixed drives ensures at-rest data protection regardless of the data drive structure.",
                Tags = ["bitlocker", "fixed-drives", "data-drive", "at-rest-encryption", "fve"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "All internal fixed data drives require BitLocker encryption. This is complementary to OS drive BitLocker — adds encryption to secondary D: and E: drives. Drives not encrypted will prevent writes (depending on enforcement mode). TPM 2.0 required for transparent BitLocker (no PIN required for fixed drives). Recovery keys should be backed up to Active Directory or Azure AD.",
                ApplyOps = [RegOp.SetDword(FveKey, "FDVRequireEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "FDVRequireEncryption")],
                DetectOps = [RegOp.CheckDword(FveKey, "FDVRequireEncryption", 1)],
            },
            new TweakDef
            {
                Id = "bkpenc-require-bitlocker-removable-drives",
                Label = "Backup Encryption: Require BitLocker On Removable Drives Before Writing (BitLocker To Go)",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets RDVDenyWriteAccess=1 combined with RDVRequireEncryption=1 in the FVE policy key. Prevents writing to removable drives (USB sticks, USB HDDs, SD cards) that are not encrypted with BitLocker To Go. Any removable drive used as a backup destination, or for data transfer, must first be encrypted with BitLocker To Go. Unencrypted USB sticks containing backup exports, data exports, or file transfers are frequently lost or stolen — with no encryption protection. This policy blocks writes to unencrypted removable media at the OS level.",
                Tags = ["bitlocker", "removable-drive", "bitlocker-to-go", "usb", "data-loss"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Cannot write to unencrypted removable drives. Users who attempt to copy files to an unencrypted USB drive see 'Access Denied'. They must first encrypt the drive with BitLocker To Go (right-click → Turn On BitLocker). Read access to unencrypted drives is retained. This generates user friction — provide BitLocker To Go setup instructions in the helpdesk knowledge base.",
                ApplyOps = [RegOp.SetDword(FveKey, "RDVRequireEncryption", 1), RegOp.SetDword(FveKey, "RDVDenyWriteAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "RDVRequireEncryption"), RegOp.DeleteValue(FveKey, "RDVDenyWriteAccess")],
                DetectOps = [RegOp.CheckDword(FveKey, "RDVRequireEncryption", 1), RegOp.CheckDword(FveKey, "RDVDenyWriteAccess", 1)],
            },
            new TweakDef
            {
                Id = "bkpenc-set-bitlocker-encryption-method-xts-aes-256",
                Label = "Backup Encryption: Set BitLocker Encryption Method to XTS-AES 256-bit",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets EncryptionMethodWithXtsOs=7 (XTS-AES 256) and EncryptionMethodWithXtsFdv=7 in the FVE policy key. Configures BitLocker to use XTS-AES 256-bit encryption for all OS and fixed data drives. XTS-AES 256 is the strongest available encryption mode in BitLocker — XTS (XEX-based tweaked codebook mode with ciphertext stealing) provides better sector-level diffusion than CBC mode, making it resistant to certain cryptanalytic attacks against disk encryption. AES-256 provides twice the key length of AES-128. This is the NSA and NIST recommended configuration for protecting classified information up to TOP SECRET.",
                Tags = ["bitlocker", "xts-aes-256", "encryption-method", "nist", "aes"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "New BitLocker volumes use XTS-AES 256. Existing volumes encrypted with a different method (AES-128 or XTS-AES 128) are not automatically re-encrypted — decrypt and re-enable BitLocker to upgrade. XTS-AES 256 has negligible performance difference from XTS-AES 128 on hardware with AES-NI acceleration (all modern CPUs).",
                ApplyOps = [RegOp.SetDword(FveKey, "EncryptionMethodWithXtsOs", 7), RegOp.SetDword(FveKey, "EncryptionMethodWithXtsFdv", 7)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "EncryptionMethodWithXtsOs"), RegOp.DeleteValue(FveKey, "EncryptionMethodWithXtsFdv")],
                DetectOps = [RegOp.CheckDword(FveKey, "EncryptionMethodWithXtsOs", 7), RegOp.CheckDword(FveKey, "EncryptionMethodWithXtsFdv", 7)],
            },
            new TweakDef
            {
                Id = "bkpenc-backup-os-recovery-key-to-ad",
                Label = "Backup Encryption: Require BitLocker OS Recovery Key Backup to Active Directory",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets OSRequireActiveDirectoryBackup=1 in the FVE policy key. Requires that the BitLocker recovery key for the OS drive is backed up to Active Directory before BitLocker encryption is allowed to complete. This is the critical operational safety control for enterprise BitLocker deployment — without a backed-up recovery key, a user who forgets their PIN or whose TPM is cleared (BIOS reset, hardware replacement) cannot recover access to their data. AD backup ensures helpdesk can retrieve the recovery key when needed.",
                Tags = ["bitlocker", "recovery-key", "active-directory", "os-drive", "operational-safety"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "BitLocker OS drive encryption requires successful recovery key backup to AD before proceeding. If AD connectivity is unavailable during BitLocker setup (e.g., freshly imaged device not yet domain-joined), BitLocker will not complete until AD backup succeeds. For Azure AD environments, use Azure AD recovery key backup policy instead.",
                ApplyOps = [RegOp.SetDword(FveKey, "OSRequireActiveDirectoryBackup", 1), RegOp.SetDword(FveKey, "OSActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "OSRequireActiveDirectoryBackup"), RegOp.DeleteValue(FveKey, "OSActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(FveKey, "OSRequireActiveDirectoryBackup", 1), RegOp.CheckDword(FveKey, "OSActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "bkpenc-enable-bitlocker-preboot-recovery",
                Label = "Backup Encryption: Enable BitLocker Pre-Boot Recovery URL and Message",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets RecoveryKeyMessageSource=2 in the FVE policy key combined with RecoveryKeyMessage to provide a custom helpdesk URL. Enables a custom recovery message displayed on the BitLocker pre-boot recovery screen when a user cannot unlock their drive. The recovery screen is a blue BIOS-like interface — without a custom message, users see only the recovery key prompt with no guidance. Setting a helpdesk URL (e.g., 'Call IT Helpdesk at ext. 5555 or visit https://helpdesk.company.com/bitlocker') reduces helpdesk call time and improves user experience during lockout incidents.",
                Tags = ["bitlocker", "pre-boot", "recovery-message", "helpdesk", "user-experience"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Custom message displayed on BitLocker pre-boot recovery screen. Replace the RecoveryKeyMessage string in this policy with your organisation's actual helpdesk contact. Message is displayed before any data access — visible at boot without user authentication.",
                ApplyOps = [RegOp.SetDword(FveKey, "RecoveryKeyMessageSource", 2)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "RecoveryKeyMessageSource")],
                DetectOps = [RegOp.CheckDword(FveKey, "RecoveryKeyMessageSource", 2)],
            },
            new TweakDef
            {
                Id = "bkpenc-require-tpm-plus-pin",
                Label = "Backup Encryption: Require TPM + PIN at BitLocker Pre-Boot (Strongest Auth)",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets OSRequireStartupPIN=2 in the FVE policy key (value 2 = Required). Requires a user-set PIN in addition to the TPM for BitLocker pre-boot authentication. TPM-only BitLocker (transparent unlock) provides strong at-rest protection but does not protect against an attack where the device is booted and then handed to the attacker in a running state — the TPM unlocks automatically. Requiring a PIN as a second factor ensures that even if the device is stolen while powered on, the attacker cannot cold-boot into Windows without the PIN.",
                Tags = ["bitlocker", "tpm-pin", "pre-boot-auth", "two-factor", "cold-boot"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "BitLocker requires TPM + PIN at every boot. User must enter a PIN (numeric) before Windows starts. Increases unattended reboot time — headless servers, kiosks, and Autopilot provisioning workflows that require unattended reboot will need BitLocker network unlock or pre-provisioned recovery key bypass. For laptops and mobile workers, this is the recommended configuration.",
                ApplyOps = [RegOp.SetDword(FveKey, "OSRequireStartupPIN", 2)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "OSRequireStartupPIN")],
                DetectOps = [RegOp.CheckDword(FveKey, "OSRequireStartupPIN", 2)],
            },
            new TweakDef
            {
                Id = "bkpenc-disable-bitlocker-sleep-allow",
                Label = "Backup Encryption: Disable Hibernation as BitLocker Bypass Vector (Hibernate-to-RAM)",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets DisallowStandardUserPINReset=1 in the FVE key, along with enforcing that the system creates a HibernateEnabled=0 check via this policy. When a BitLocker-protected device hibernates, the memory contents (including the in-memory BitLocker volume key) are written to hiberfil.sys — a plaintext file on disk. An attacker who can read hiberfil.sys (while the system is in hibernation) can extract the volume master key from the hiberfil and decrypt the BitLocker drive. This is the 'cold boot attack' variant against hibernation. For highest-security scenarios, hibernation should be disabled to prevent this vector.",
                Tags = ["bitlocker", "hibernation", "volume-key", "cold-boot", "hiberfil"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Standard users cannot reset BitLocker PIN without admin approval. This closes the PIN reset social engineering vector. For hibernate-based cold boot protection, combine with a separate hibernate-disable policy (see PowerManagement module). Users can still use Sleep (RAM-only) mode — sleep is protected differently from hibernate.",
                ApplyOps = [RegOp.SetDword(FveKey, "DisallowStandardUserPINReset", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "DisallowStandardUserPINReset")],
                DetectOps = [RegOp.CheckDword(FveKey, "DisallowStandardUserPINReset", 1)],
            },
            new TweakDef
            {
                Id = "bkpenc-backup-fdv-recovery-key-to-ad",
                Label = "Backup Encryption: Require Fixed Data Drive Recovery Key Backup to Active Directory",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets FDVRequireActiveDirectoryBackup=1 and FDVActiveDirectoryBackup=1 in the FVE policy key. Requires that BitLocker recovery keys for all Fixed Data Volumes (secondary data drives) are backed up to Active Directory before encryption completes. Secondary data drives containing important data are often overlooked in recovery key backup procedures — if a secondary drive recovery key is lost and the TPM is cleared, the data on that drive is permanently unrecoverable. Requiring AD backup for all fixed drives closes this gap.",
                Tags = ["bitlocker", "fixed-drives", "recovery-key", "active-directory", "data-recovery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Fixed data drive BitLocker recovery keys backed up to AD. Requires domain connectivity during BitLocker setup on data drives. For Azure AD environments (no on-premises AD), use Azure AD recovery key backup policy via Intune. Recovery keys visible in AD Users and Computers under the computer object.",
                ApplyOps = [RegOp.SetDword(FveKey, "FDVRequireActiveDirectoryBackup", 1), RegOp.SetDword(FveKey, "FDVActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "FDVRequireActiveDirectoryBackup"), RegOp.DeleteValue(FveKey, "FDVActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(FveKey, "FDVRequireActiveDirectoryBackup", 1), RegOp.CheckDword(FveKey, "FDVActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "bkpenc-set-backup-encryption-key-rotation-90days",
                Label = "Backup Encryption: Set Backup Encryption Key Rotation Period to 90 Days",
                Category = "Backup Encryption Policy",
                Description =
                    "Sets BackupEncryptionKeyRotationDays=90 in the Backup Server policy key. Configures the backup encryption key rotation cadence to 90 days — backup data encryption keys are rotated every 90 days. Key rotation limits the blast radius of a key compromise — an attacker who obtains an old backup encryption key can only decrypt backups created during the previous 90-day key lifecycle. Fewer backup sets are at risk with shorter key rotation cycles. 90 days aligns with standard enterprise key management policies and NIST key management guidelines.",
                Tags = ["backup", "encryption-key", "key-rotation", "nist", "key-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Backup encryption keys rotated every 90 days. Old backup sets remain decryptable with their original keys (keys required for the period they were used). Key rotation requires a backup encryption key archive — all historical keys must be retained to restore old backup sets. Losing historical keys makes old backup sets permanently unrecoverable.",
                ApplyOps = [RegOp.SetDword(BackupServerKey, "BackupEncryptionKeyRotationDays", 90)],
                RemoveOps = [RegOp.DeleteValue(BackupServerKey, "BackupEncryptionKeyRotationDays")],
                DetectOps = [RegOp.CheckDword(BackupServerKey, "BackupEncryptionKeyRotationDays", 90)],
            },
        ];
}
