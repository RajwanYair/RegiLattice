// RegiLattice.Core — Tweaks/PolicyEncryption.cs
// BitLocker, EFS, FIPS, HVCI, memory integrity, Secure Boot, TLS/SCHANNEL, VBS, and data encryption policies
// Category: "Encryption Policy"
// Consolidated from 16 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyEncryption
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _BackupEncryptionPolicy.Data,
            .. _BitLockerFvePolicy.Data,
            .. _BitLockerNetworkUnlockPolicy.Data,
            .. _BitLockerPolicy.Data,
            .. _BitLockerRemovable.Data,
            .. _CryptographicOperationsPolicy.Data,
            .. _EfsEncryptionPolicy.Data,
            .. _FipsCompliancePolicy.Data,
            .. _HvciPolicy.Data,
            .. _MemoryIntegrityPolicy.Data,
            .. _PersonalDataEncryptionPolicy.Data,
            .. _SecureBootDbxPolicy.Data,
            .. _SecureBootPolicy.Data,
            .. _TlsSchannel.Data,
            .. _UefiLockPolicy.Data,
            .. _VbsEnforcementPolicy.Data,
        ];

    // ── BackupEncryptionPolicy ──
    private static class _BackupEncryptionPolicy
    {
        private const string FveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

        private const string BackupServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Server";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "bkpenc-require-bitlocker-os-drive",
                    Label = "Backup Encryption: Require BitLocker Encryption on OS Drive Before Backup",
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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

    // ── BitLockerFvePolicy ──
    private static class _BitLockerFvePolicy
    {
        private const string FveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
        private const string FveOs = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\OSVolume";
        private const string FveRem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\RemovableDrives";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "blfve-disable-recovery-console-dra",
                Label = "Disable BitLocker Recovery via Data Recovery Agent",
                Category = "Encryption",
                Description = "Sets DisableDRA=1 in the FVE policy key. "
                    + "Prevents the use of a Data Recovery Agent (DRA) certificate to unlock BitLocker-protected "
                    + "OS or fixed drives. DRA keys are sometimes required in enterprise environments where the "
                    + "IT department maintains a master recovery certificate. Disabling DRA means only recovery "
                    + "passwords or Trusted Platform Module (TPM) can unlock the drive. "
                    + "Default: absent (DRA allowed). Recommended: 0 in environments that use DRA for recovery, "
                    + "or 1 in TPM-only/recovery-key-only deployments.",
                Tags = ["bitlocker", "fve", "dra", "recovery", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Data Recovery Agent certificates cannot unlock BitLocker volumes; TPM or recovery key only.",
                ApplyOps  = [RegOp.SetDword(FveKey, "DisableDRA", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "DisableDRA")],
                DetectOps = [RegOp.CheckDword(FveKey, "DisableDRA", 1)],
            },
            new TweakDef
            {
                Id = "blfve-require-tpm-for-os-drive",
                Label = "Require TPM for OS Drive BitLocker",
                Category = "Encryption",
                Description = "Sets OSRequireTPM=1 in the FVE policy key. "
                    + "Requires the machine to have a Trusted Platform Module (TPM) present and enabled before "
                    + "BitLocker can be activated on the OS drive. Prevents BitLocker from being activated in "
                    + "TPM-passthrough or software-only mode, ensuring hardware-backed key protection. "
                    + "On machines without a TPM, BitLocker will be unavailable for the OS drive. "
                    + "Default: absent (TPM not required). Recommended: 1 on corporate endpoints.",
                Tags = ["bitlocker", "fve", "tpm", "os-drive", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "BitLocker on OS drive requires TPM hardware; software-only mode blocked.",
                ApplyOps  = [RegOp.SetDword(FveKey, "OSRequireTPM", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "OSRequireTPM")],
                DetectOps = [RegOp.CheckDword(FveKey, "OSRequireTPM", 1)],
            },
            new TweakDef
            {
                Id = "blfve-set-os-encryption-aes256",
                Label = "Set OS Drive BitLocker Encryption to AES-256-XTS",
                Category = "Encryption",
                Description = "Sets OSEncryptionType=7 in the FVE policy key. "
                    + "Forces BitLocker on the OS drive to use AES-256 with XTS mode (the strongest "
                    + "BitLocker cipher suite available on Windows 10/11). The EncryptionMethod values: "
                    + "3=AES-128, 4=AES-256, 6=XTS-AES-128, 7=XTS-AES-256. XTS mode provides additional "
                    + "protection against ciphertext-manipulation attacks on disk sectors. "
                    + "Default: absent (typically XTS-AES-128). Recommended: 7 on sensitive-data endpoints.",
                Tags = ["bitlocker", "fve", "aes-256", "xts", "encryption-strength", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "OS drive BitLocker uses XTS-AES-256; stronger encryption for new BitLocker activations.",
                ApplyOps  = [RegOp.SetDword(FveKey, "OSEncryptionType", 7)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "OSEncryptionType")],
                DetectOps = [RegOp.CheckDword(FveKey, "OSEncryptionType", 7)],
            },
            new TweakDef
            {
                Id = "blfve-require-recovery-key-os",
                Label = "Require Recovery Key for OS BitLocker",
                Category = "Encryption",
                Description = "Sets OSRecoveryKey=1 in the FVE OSVolume policy key. "
                    + "Requires that a recovery key (48-digit password or .bek file) be generated and saved "
                    + "when BitLocker is enabled on the OS drive. Ensures that IT helpdesk or the user always "
                    + "has a fallback path to recover the drive if TPM/PIN authentication fails. "
                    + "Default: absent (recovery key optional). Recommended: 1 for all enterprise deployments.",
                Tags = ["bitlocker", "fve", "recovery-key", "os-drive", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Recovery key generation mandatory when enabling BitLocker on OS drive.",
                ApplyOps  = [RegOp.SetDword(FveOs, "OSRecoveryKey", 1)],
                RemoveOps = [RegOp.DeleteValue(FveOs, "OSRecoveryKey")],
                DetectOps = [RegOp.CheckDword(FveOs, "OSRecoveryKey", 1)],
            },
            new TweakDef
            {
                Id = "blfve-deny-write-removable-unprotected",
                Label = "Deny Write Access to Unprotected Removable Drives",
                Category = "Encryption",
                Description = "Sets RDVDenyWriteAccess=1 in the FVE RemovableDrives policy key. "
                    + "Prevents the Windows file system from granting write access to removable drives "
                    + "(USB flash, external HDD, SD cards) that are not BitLocker-protected. "
                    + "Read access is still allowed; only write operations are blocked. "
                    + "Protects against accidental or intentional data exfiltration via unencrypted USB drives. "
                    + "Default: absent (write allowed). Recommended: 1 for data-loss-prevention enforcement.",
                Tags = ["bitlocker", "fve", "removable-drive", "write-access", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Write access to unencrypted removable drives blocked; encrypted drives still writable.",
                ApplyOps  = [RegOp.SetDword(FveRem, "RDVDenyWriteAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(FveRem, "RDVDenyWriteAccess")],
                DetectOps = [RegOp.CheckDword(FveRem, "RDVDenyWriteAccess", 1)],
            },
            new TweakDef
            {
                Id = "blfve-enable-preboot-input-protectors",
                Label = "Enable Pre-Boot Input Protectors for BitLocker",
                Category = "Encryption",
                Description = "Sets OSEnablePreBootInputProtectors=1 in the FVE policy key. "
                    + "Allows BitLocker to use pre-boot input protectors (PIN or passphrase) even on "
                    + "systems with touch-only or non-standard input (Surface tablets, kiosk machines). "
                    + "Without this, BitLocker may refuse to set a PIN on devices it cannot detect a "
                    + "standard keyboard for. Enabling this overrides the device-type detection heuristic. "
                    + "Default: absent. Recommended: 1 when deploying BitLocker with PIN on tablets.",
                Tags = ["bitlocker", "fve", "pre-boot", "pin", "tablet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Pre-boot PIN/passphrase allowed even on touch-only or non-standard keyboard devices.",
                ApplyOps  = [RegOp.SetDword(FveKey, "OSEnablePreBootInputProtectors", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "OSEnablePreBootInputProtectors")],
                DetectOps = [RegOp.CheckDword(FveKey, "OSEnablePreBootInputProtectors", 1)],
            },
            new TweakDef
            {
                Id = "blfve-disable-standby-bitlocker",
                Label = "Disable Standby Mode When BitLocker Is Active",
                Category = "Encryption",
                Description = "Sets DisallowStandbyWithBitLocker=1 in the FVE policy key. "
                    + "Prevents the machine from entering S1-S3 standby sleep states while a "
                    + "BitLocker-protected OS drive is active and not locked. Standby states "
                    + "preserve RAM (including encryption keys) in a low-power state, and sophisticated "
                    + "cold-boot attacks can recover these keys if the machine is left in standby. "
                    + "This policy forces hibernate (S4) or full shutdown instead. "
                    + "Default: absent. Recommended: 1 on high-security endpoints.",
                Tags = ["bitlocker", "fve", "standby", "cold-boot", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Standby disabled when BitLocker is active; forces hibernate or shutdown to protect encryption keys.",
                ApplyOps  = [RegOp.SetDword(FveKey, "DisallowStandbyWithBitLocker", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "DisallowStandbyWithBitLocker")],
                DetectOps = [RegOp.CheckDword(FveKey, "DisallowStandbyWithBitLocker", 1)],
            },
            new TweakDef
            {
                Id = "blfve-backup-recovery-to-ad",
                Label = "Backup BitLocker Recovery Key to Active Directory",
                Category = "Encryption",
                Description = "Sets OSRecoveryBackupToAD=1 in the FVE OSVolume policy key. "
                    + "Requires BitLocker to back up the OS drive recovery key to Active Directory Domain "
                    + "Services (ADDS) before finishing encryption. Prevents scenarios where the recovery "
                    + "key is stored only on the user's device or paper, ensuring IT admins can always "
                    + "recover a locked machine via ADDS. "
                    + "Default: absent. Recommended: 1 in domain-joined enterprise environments.",
                Tags = ["bitlocker", "fve", "recovery", "active-directory", "backup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "BitLocker recovery key backed up to Active Directory automatically.",
                ApplyOps  = [RegOp.SetDword(FveOs, "OSRecoveryBackupToAD", 1)],
                RemoveOps = [RegOp.DeleteValue(FveOs, "OSRecoveryBackupToAD")],
                DetectOps = [RegOp.CheckDword(FveOs, "OSRecoveryBackupToAD", 1)],
            },
            new TweakDef
            {
                Id = "blfve-set-fixed-drive-aes256",
                Label = "Set Fixed Drive BitLocker Encryption to AES-256-XTS",
                Category = "Encryption",
                Description = "Sets FDVEncryptionType=7 in the FVE policy key. "
                    + "Forces BitLocker on fixed data drives (secondary internal HDDs/SSDs) to use "
                    + "XTS-AES-256, the strongest available cipher. Fixed data drives often store sensitive "
                    + "user data (Documents, Downloads) that benefits from stronger encryption. "
                    + "Values: 3=AES-128, 4=AES-256, 6=XTS-AES-128, 7=XTS-AES-256. "
                    + "Default: absent (typically XTS-AES-128). Recommended: 7 on compliance-critical endpoints.",
                Tags = ["bitlocker", "fve", "fixed-drive", "aes-256", "xts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Fixed data drives encrypted with XTS-AES-256 on new BitLocker activations.",
                ApplyOps  = [RegOp.SetDword(FveKey, "FDVEncryptionType", 7)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "FDVEncryptionType")],
                DetectOps = [RegOp.CheckDword(FveKey, "FDVEncryptionType", 7)],
            },
            new TweakDef
            {
                Id = "blfve-set-removable-drive-aes128",
                Label = "Set Removable Drive BitLocker Encryption to AES-128-XTS",
                Category = "Encryption",
                Description = "Sets RDVEncryptionType=6 in the FVE RemovableDrives policy key. "
                    + "Forces BitLocker To Go on removable drives to use XTS-AES-128 rather than the "
                    + "default AES-128 (non-XTS). XTS mode adds ciphertext-manipulation protection on "
                    + "removable media. AES-128 (not 256) is recommended for removable drives to maintain "
                    + "cross-device compatibility with older Windows 10 machines that can read BitLocker "
                    + "To Go drives. For maximum security, use value 7 (XTS-AES-256). "
                    + "Default: absent (typically AES-128). Recommended: 6 for balanced security and compat.",
                Tags = ["bitlocker", "fve", "removable-drive", "aes-128", "xts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removable drives use XTS-AES-128 encryption; compatible with older Windows devices.",
                ApplyOps  = [RegOp.SetDword(FveRem, "RDVEncryptionType", 6)],
                RemoveOps = [RegOp.DeleteValue(FveRem, "RDVEncryptionType")],
                DetectOps = [RegOp.CheckDword(FveRem, "RDVEncryptionType", 6)],
            },
        ];

    }

    // ── BitLockerNetworkUnlockPolicy ──
    private static class _BitLockerNetworkUnlockPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
        private const string OsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\OSVolume";
        private const string NuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\NetworkUnlock";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id           = "blnetun-require-tpm-plus-pin",
                Label        = "Require TPM + PIN Pre-Boot Authentication for BitLocker OS Volumes",
                Category = "Encryption",
                Description  = "Configures BitLocker to require both TPM attestation and a user-supplied PIN for OS volume unlock at pre-boot, providing two-factor pre-boot authentication that protects against direct memory access and cold boot attacks even on stolen hardware.",
                Tags         = ["bitlocker", "tpm", "pin", "pre-boot", "two-factor", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "BitLocker TPM+PIN required; cold boot and DMA attacks on stolen hardware mitigated by pre-boot second factor.",
                ApplyOps     = [RegOp.SetDword(OsKey, "RequirePinForOSVolume", 1)],
                RemoveOps    = [RegOp.DeleteValue(OsKey, "RequirePinForOSVolume")],
                DetectOps    = [RegOp.CheckDword(OsKey, "RequirePinForOSVolume", 1)],
            },
            new TweakDef
            {
                Id           = "blnetun-set-minimum-pin-length-8",
                Label        = "Set Minimum BitLocker Pre-Boot PIN Length to 8 Digits",
                Category = "Encryption",
                Description  = "Sets the minimum pre-boot PIN length to 8 digits, preventing trivially short 4-digit PINs that could be brute-forced even against the TPM anti-hammering lockout threshold.",
                Tags         = ["bitlocker", "pin-length", "minimum", "brute-force", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "BitLocker pre-boot PIN minimum 8 digits; trivially short 4-digit PINs no longer accepted.",
                ApplyOps     = [RegOp.SetDword(Key, "MinimumPIN", 8)],
                RemoveOps    = [RegOp.DeleteValue(Key, "MinimumPIN")],
                DetectOps    = [RegOp.CheckDword(Key, "MinimumPIN", 8)],
            },
            new TweakDef
            {
                Id           = "blnetun-disable-recovery-to-ad-storage",
                Label        = "Disable BitLocker Recovery Key Storage in Active Directory by Default",
                Category = "Encryption",
                Description  = "Prevents BitLocker from automatically storing the 48-digit recovery key in Active Directory by default, ensuring recovery key storage is a deliberate IT action rather than an automatic operation that could be mass-enumerated.",
                Tags         = ["bitlocker", "recovery-key", "active-directory", "key-storage", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "BitLocker auto-AD recovery key escrow disabled; deliberate admin action required to store recovery keys.",
                ApplyOps     = [RegOp.SetDword(Key, "ActiveDirectoryBackup", 0)],
                RemoveOps    = [RegOp.DeleteValue(Key, "ActiveDirectoryBackup")],
                DetectOps    = [RegOp.CheckDword(Key, "ActiveDirectoryBackup", 0)],
            },
            new TweakDef
            {
                Id           = "blnetun-block-recovery-password-print",
                Label        = "Block Printing BitLocker Recovery Passwords",
                Category = "Encryption",
                Description  = "Prevents users from printing the 48-digit BitLocker recovery password, ensuring recovery passwords are not output on physical paper that could be shoulder-surfed, photographed, or left in a printer output tray.",
                Tags         = ["bitlocker", "recovery-password", "print", "physical-security", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "BitLocker recovery password printing blocked; paper copies of 48-digit recovery keys prevented.",
                ApplyOps     = [RegOp.SetDword(Key, "BlockRecoveryPasswordPrinting", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "BlockRecoveryPasswordPrinting")],
                DetectOps    = [RegOp.CheckDword(Key, "BlockRecoveryPasswordPrinting", 1)],
            },
            new TweakDef
            {
                Id           = "blnetun-deny-write-without-bitlocker",
                Label        = "Deny Write Access to Removable Drives Without BitLocker",
                Category = "Encryption",
                Description  = "Prevents write operations to removable USB drives and portable storage that are not BitLocker-protected, ensuring sensitive data cannot be exfiltrated to unencrypted USB devices.",
                Tags         = ["bitlocker", "removable-drive", "usb", "write-protection", "data-exfiltration", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 5,
                SafetyRating = 5,
                ImpactNote   = "Unencrypted USB write access blocked; data can only be written to BitLocker-encrypted removable drives.",
                ApplyOps     = [RegOp.SetDword(Key, "RDVDenyWriteAccess", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "RDVDenyWriteAccess")],
                DetectOps    = [RegOp.CheckDword(Key, "RDVDenyWriteAccess", 1)],
            },
            new TweakDef
            {
                Id           = "blnetun-enable-network-unlock",
                Label        = "Enable BitLocker Network Unlock for Domain-Joined Systems",
                Category = "Encryption",
                Description  = "Enables BitLocker Network Unlock, allowing domain-joined systems connected to a trusted corporate network at boot time to automatically unlock the OS volume without requiring a PIN, simplifying remote management while maintaining offline protection.",
                Tags         = ["bitlocker", "network-unlock", "domain", "remote-management", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "BitLocker Network Unlock enabled; domain-joined systems auto-unlock on corporate network. PIN required off-network.",
                ApplyOps     = [RegOp.SetDword(NuKey, "EnableNetworkUnlock", 1)],
                RemoveOps    = [RegOp.DeleteValue(NuKey, "EnableNetworkUnlock")],
                DetectOps    = [RegOp.CheckDword(NuKey, "EnableNetworkUnlock", 1)],
            },
            new TweakDef
            {
                Id           = "blnetun-use-aes-256-xts",
                Label        = "Use AES-256-XTS Encryption for New BitLocker OS Volumes",
                Category = "Encryption",
                Description  = "Sets the BitLocker encryption algorithm for new OS volume encryptions to AES-256 in XTS mode, which is the strongest available encryption in Windows 11, replacing the default AES-128-XTS.",
                Tags         = ["bitlocker", "aes-256", "xts", "encryption-strength", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "New BitLocker OS volumes encrypted with AES-256-XTS; maximum available encryption strength enforced.",
                ApplyOps     = [RegOp.SetDword(Key, "EncryptionMethodWithXtsOs", 7)],
                RemoveOps    = [RegOp.DeleteValue(Key, "EncryptionMethodWithXtsOs")],
                DetectOps    = [RegOp.CheckDword(Key, "EncryptionMethodWithXtsOs", 7)],
            },
            new TweakDef
            {
                Id           = "blnetun-use-aes-256-xts-fixed",
                Label        = "Use AES-256-XTS Encryption for New BitLocker Fixed Data Volumes",
                Category = "Encryption",
                Description  = "Sets the BitLocker encryption algorithm for new fixed (internal) data drive encryptions to AES-256-XTS, ensuring the same maximum-strength encryption is applied to non-OS data drives.",
                Tags         = ["bitlocker", "aes-256", "xts", "data-volume", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 4,
                SafetyRating = 5,
                ImpactNote   = "New BitLocker fixed data volumes encrypted with AES-256-XTS; maximum encryption on internal data drives.",
                ApplyOps     = [RegOp.SetDword(Key, "EncryptionMethodWithXtsFdv", 7)],
                RemoveOps    = [RegOp.DeleteValue(Key, "EncryptionMethodWithXtsFdv")],
                DetectOps    = [RegOp.CheckDword(Key, "EncryptionMethodWithXtsFdv", 7)],
            },
            new TweakDef
            {
                Id           = "blnetun-log-unlock-events",
                Label        = "Log BitLocker Volume Unlock and Lock Events",
                Category = "Encryption",
                Description  = "Enables Security audit log entries for BitLocker volume unlock and lock events, providing visibility into drive decryption activity for forensics and compliance auditing.",
                Tags         = ["bitlocker", "unlock-audit", "event-log", "compliance", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "BitLocker unlock/lock events logged; drive decryption activity visible for forensics and compliance.",
                ApplyOps     = [RegOp.SetDword(Key, "LogUnlockEvents", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "LogUnlockEvents")],
                DetectOps    = [RegOp.CheckDword(Key, "LogUnlockEvents", 1)],
            },
            new TweakDef
            {
                Id           = "blnetun-disable-bitlocker-telemetry",
                Label        = "Disable BitLocker Telemetry Reporting to Microsoft",
                Category = "Encryption",
                Description  = "Prevents BitLocker from reporting encryption algorithm usage, PIN complexity, recovery key storage method, and drive unlock events to Microsoft via Windows telemetry.",
                Tags         = ["bitlocker", "telemetry", "privacy", "microsoft", "policy"],
                NeedsAdmin   = true,
                CorpSafe     = true,
                ImpactScore  = 3,
                SafetyRating = 5,
                ImpactNote   = "BitLocker telemetry to Microsoft disabled; encryption config and unlock event data not sent to cloud.",
                ApplyOps     = [RegOp.SetDword(Key, "DisableBitLockerTelemetry", 1)],
                RemoveOps    = [RegOp.DeleteValue(Key, "DisableBitLockerTelemetry")],
                DetectOps    = [RegOp.CheckDword(Key, "DisableBitLockerTelemetry", 1)],
            },
        ];

    }

    // ── BitLockerPolicy ──
    private static class _BitLockerPolicy
    {
        private const string FveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "bde-require-tpm",
                Label = "Require TPM for BitLocker Encryption",
                Category = "Encryption",
                Description = "Prevents BitLocker from being configured without a compatible Trusted Platform Module (TPM).",
                Tags = ["bitlocker", "tpm", "fde", "encryption", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "BitLocker requires TPM; USB-key-only startup is blocked. Devices without TPM cannot enable BitLocker.",
                ApplyOps = [RegOp.SetDword(FveKey, "EnableBDEWithNoTPM", 0)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "EnableBDEWithNoTPM")],
                DetectOps = [RegOp.CheckDword(FveKey, "EnableBDEWithNoTPM", 0)],
            },
            new TweakDef
            {
                Id = "bde-allow-enhanced-pin",
                Label = "Allow Enhanced BitLocker TPM PINs",
                Category = "Encryption",
                Description = "Permits the use of enhanced PINs (letters, symbols, and spaces) instead of only digits for BitLocker TPM startup.",
                Tags = ["bitlocker", "tpm", "pin", "security", "encryption"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Increases PIN entropy; compatible with all modern UEFI firmware; users must set a new enhanced PIN.",
                ApplyOps = [RegOp.SetDword(FveKey, "UseEnhancedPin", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "UseEnhancedPin")],
                DetectOps = [RegOp.CheckDword(FveKey, "UseEnhancedPin", 1)],
            },
            new TweakDef
            {
                Id = "bde-set-minimum-pin-8",
                Label = "Set Minimum BitLocker TPM PIN Length to 8",
                Category = "Encryption",
                Description = "Requires the BitLocker TPM PIN to be at least 8 characters long.",
                Tags = ["bitlocker", "tpm", "pin", "length", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "8-character minimum; longer PINs are allowed. Users must reset PINs shorter than 8 characters.",
                ApplyOps = [RegOp.SetDword(FveKey, "MinimumPIN", 8)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "MinimumPIN")],
                DetectOps = [RegOp.CheckDword(FveKey, "MinimumPIN", 8)],
            },
            new TweakDef
            {
                Id = "bde-require-recovery-password",
                Label = "Require BitLocker Recovery Password",
                Category = "Encryption",
                Description = "Mandates that a 48-digit numerical recovery password is generated and saved before BitLocker can be enabled.",
                Tags = ["bitlocker", "recovery", "password", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Recovery passwords must be saved before encryption begins; prevents lockout without a recovery path.",
                ApplyOps = [RegOp.SetDword(FveKey, "UseRecoveryPassword", 2)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "UseRecoveryPassword")],
                DetectOps = [RegOp.CheckDword(FveKey, "UseRecoveryPassword", 2)],
            },
            new TweakDef
            {
                Id = "bde-backup-to-ad",
                Label = "Back Up BitLocker Recovery Key to Active Directory",
                Category = "Encryption",
                Description = "Automatically backs up BitLocker recovery passwords and key packages to Active Directory DS.",
                Tags = ["bitlocker", "recovery", "active-directory", "backup", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Recovery keys are escrowed in AD; requires domain-join and AD DS with BitLocker schema extension.",
                ApplyOps = [RegOp.SetDword(FveKey, "ActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "ActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(FveKey, "ActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "bde-block-without-ad-backup",
                Label = "Block BitLocker if AD Recovery Backup Fails",
                Category = "Encryption",
                Description = "Prevents BitLocker from completing setup if the recovery password cannot be backed up to Active Directory.",
                Tags = ["bitlocker", "recovery", "active-directory", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Encryption fails if AD backup is unreachable; ensures no device is encrypted without a recovery path.",
                ApplyOps = [RegOp.SetDword(FveKey, "RequireActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "RequireActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(FveKey, "RequireActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "bde-store-password-and-key-package",
                Label = "Store BitLocker Recovery Password and Key Package",
                Category = "Encryption",
                Description = "Configures AD backup to store both the recovery password and the full key package for maximum recovery options.",
                Tags = ["bitlocker", "recovery", "active-directory", "key-package", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Value 3 = password + key package; provides both quick recovery and full forensic key reconstruction.",
                ApplyOps = [RegOp.SetDword(FveKey, "ActiveDirectoryInfoToStore", 3)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "ActiveDirectoryInfoToStore")],
                DetectOps = [RegOp.CheckDword(FveKey, "ActiveDirectoryInfoToStore", 3)],
            },
            new TweakDef
            {
                Id = "bde-require-tpm-and-pin",
                Label = "Require TPM + PIN for BitLocker OS Drive Startup",
                Category = "Encryption",
                Description = "Mandates both TPM presence and a user-supplied PIN for pre-boot BitLocker authentication on the OS drive.",
                Tags = ["bitlocker", "tpm", "pin", "startup", "security", "mfa"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Two-factor startup (something you have + know); PIN required at every boot; remote start requires workarounds.",
                ApplyOps = [RegOp.SetDword(FveKey, "UseTPMPIN", 2)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "UseTPMPIN")],
                DetectOps = [RegOp.CheckDword(FveKey, "UseTPMPIN", 2)],
            },
            new TweakDef
            {
                Id = "bde-disable-recovery-usb",
                Label = "Disable USB Recovery Key for BitLocker OS Drive",
                Category = "Encryption",
                Description = "Prevents a USB recovery key flash drive from being used as a recovery method for the BitLocker OS drive.",
                Tags = ["bitlocker", "recovery", "usb", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Recovery via USB is blocked; only the 48-digit recovery password can be used for OS drive recovery.",
                ApplyOps = [RegOp.SetDword(FveKey, "UseRecoveryDrive", 0)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "UseRecoveryDrive")],
                DetectOps = [RegOp.CheckDword(FveKey, "UseRecoveryDrive", 0)],
            },
            new TweakDef
            {
                Id = "bde-set-xts-aes-256",
                Label = "Set BitLocker Encryption to XTS-AES-256",
                Category = "Encryption",
                Description = "Configures BitLocker to use XTS-AES-256 encryption for all new volumes, providing maximum encryption strength.",
                Tags = ["bitlocker", "encryption", "aes-256", "xts", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Value 7 = XTS-AES-256; applies to new volumes; existing volumes retain their encryption method.",
                ApplyOps = [RegOp.SetDword(FveKey, "EncryptionMethodWithXtsOs", 7)],
                RemoveOps = [RegOp.DeleteValue(FveKey, "EncryptionMethodWithXtsOs")],
                DetectOps = [RegOp.CheckDword(FveKey, "EncryptionMethodWithXtsOs", 7)],
            },
        ];

    }

    // ── BitLockerRemovable ──
    private static class _BitLockerRemovable
    {
        private const string Fve = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "btogo-deny-write-unencrypted",
                Label = "BitLocker To Go: Deny Write Access to Unencrypted Removable Drives",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "usb", "encryption", "policy"],
                Description =
                    "Sets RDVDenyWriteAccess=1 in the FVE policy. "
                    + "Prevents writing any data to removable drives (USB flash drives, external HDDs, SD cards) "
                    + "unless they are protected with BitLocker. Read access remains available. "
                    + "Enforces data-at-rest encryption for all removable media leaving the organisation.",
                SideEffects = "All removable drives without BitLocker become read-only. Existing un-encrypted drives must be encrypted before writing.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVDenyWriteAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVDenyWriteAccess")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVDenyWriteAccess", 1)],
            },
            new TweakDef
            {
                Id = "btogo-enable-rdv",
                Label = "BitLocker To Go: Enable BitLocker on Removable Data Volumes",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "usb", "encryption", "policy"],
                Description =
                    "Sets RDVAllowBDE=1 in the FVE policy. "
                    + "Explicitly enables BitLocker protection for removable data volumes via Group Policy. "
                    + "This is a prerequisite for applying other BitLocker To Go policies such as passphrase "
                    + "requirements, recovery key backup, and encryption method selection.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVAllowBDE", 1)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVAllowBDE")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVAllowBDE", 1)],
            },
            new TweakDef
            {
                Id = "btogo-require-passphrase",
                Label = "BitLocker To Go: Require Passphrase for Removable Drive Unlock",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "passphrase", "authentication", "encryption"],
                Description =
                    "Sets RDVPassphrase=1 in the FVE policy. "
                    + "Requires a passphrase (as opposed to only recovery key or smart card) as an unlock method "
                    + "when configuring BitLocker on removable drives. "
                    + "Ensures users can unlock drives on any Windows PC without smart card hardware.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVPassphrase", 1)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVPassphrase")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVPassphrase", 1)],
            },
            new TweakDef
            {
                Id = "btogo-passphrase-complexity",
                Label = "BitLocker To Go: Require Complex Passphrase for Removable Drives",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "passphrase", "complexity", "security"],
                Description =
                    "Sets RDVPassphraseComplexity=2 in the FVE policy. "
                    + "Enforces passphrase complexity requirements for removable drive encryption: "
                    + "the passphrase must meet Windows complexity rules (lowercase, uppercase, digits, symbols). "
                    + "Values: 0=no complexity, 1=cannot meet complexity, 2=must meet complexity.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVPassphraseComplexity", 2)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVPassphraseComplexity")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVPassphraseComplexity", 2)],
            },
            new TweakDef
            {
                Id = "btogo-passphrase-min-length",
                Label = "BitLocker To Go: Set Minimum Passphrase Length to 12 Characters",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "passphrase", "length", "security"],
                Description =
                    "Sets RDVPassphraseLength=12 in the FVE policy. "
                    + "Requires a minimum of 12 characters for the BitLocker To Go passphrase. "
                    + "The default minimum is 8 characters. A 12-character minimum substantially improves "
                    + "resistance to brute-force and dictionary attacks against offline encryption.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVPassphraseLength", 12)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVPassphraseLength")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVPassphraseLength", 12)],
            },
            new TweakDef
            {
                Id = "btogo-aes128-xts",
                Label = "BitLocker To Go: Set AES-128-XTS Encryption for Removable Drives",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "aes", "xts", "encryption-method"],
                Description =
                    "Sets RDVEncryptionMethodWithXts=3 in the FVE policy. "
                    + "Configures the encryption algorithm for new removable drive BitLocker volumes. "
                    + "Value 3 = AES-128-XTS (recommended for removable drives shared with older Windows versions). "
                    + "Values: 3=AES-128-XTS, 4=AES-256-XTS. XTS mode provides better diffusion than CBC.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVEncryptionMethodWithXts", 3)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVEncryptionMethodWithXts")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVEncryptionMethodWithXts", 3)],
            },
            new TweakDef
            {
                Id = "btogo-disable-hardware-encryption",
                Label = "BitLocker To Go: Disable Hardware Encryption (Force Software AES)",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["bitlocker", "removable", "hardware-encryption", "software-encryption", "security"],
                Description =
                    "Sets RDVHardwareEncryption=0 in the FVE policy. "
                    + "Disables self-encrypting drive (SED) hardware encryption for removable drives and forces "
                    + "Windows to use software-based AES encryption instead. "
                    + "Recommended following the 2018 vulnerabilities found in SED implementations (CVE-2018-12037, CVE-2018-12038) "
                    + "that rendered hardware encryption in popular SSDs ineffective.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVHardwareEncryption", 0)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVHardwareEncryption")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVHardwareEncryption", 0)],
            },
            new TweakDef
            {
                Id = "btogo-allow-smart-card",
                Label = "BitLocker To Go: Allow Smart Card Authentication for Removable Drives",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "smart-card", "authentication", "pki"],
                Description =
                    "Sets RDVAllowSmartCard=1 in the FVE policy. "
                    + "Allows smart cards to be used as an authentication method when configuring BitLocker on "
                    + "removable drives. Required when organisational policy mandates smart card (CAC/PIV) for all "
                    + "encrypted drive access in high-security environments.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVAllowSmartCard", 1)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVAllowSmartCard")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVAllowSmartCard", 1)],
            },
            new TweakDef
            {
                Id = "btogo-require-recovery-password",
                Label = "BitLocker To Go: Require Recovery Password Backup for Removable Drives",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "recovery", "password", "backup"],
                Description =
                    "Sets RDVRecoveryPassword=2 in the FVE policy. "
                    + "Requires generation and backup of a 48-digit BitLocker recovery password when enabling "
                    + "BitLocker on removable drives. Values: 0=do not allow, 1=allow, 2=require.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVRecoveryPassword", 2)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVRecoveryPassword")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVRecoveryPassword", 2)],
            },
            new TweakDef
            {
                Id = "btogo-require-recovery-key",
                Label = "BitLocker To Go: Require Recovery Key File Backup for Removable Drives",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["bitlocker", "removable", "recovery", "key", "backup"],
                Description =
                    "Sets RDVRecoveryKey=2 in the FVE policy. "
                    + "Requires a recovery key file (.BEK) to be saved alongside a recovery password when "
                    + "enabling BitLocker on removable drives. "
                    + "Values: 0=do not allow, 1=allow, 2=require. "
                    + "Ensures data recovery is possible even if the passphrase is forgotten.",
                ApplyOps = [RegOp.SetDword(Fve, "RDVRecoveryKey", 2)],
                RemoveOps = [RegOp.DeleteValue(Fve, "RDVRecoveryKey")],
                DetectOps = [RegOp.CheckDword(Fve, "RDVRecoveryKey", 2)],
            },
        ];

    }

    // ── CryptographicOperationsPolicy ──
    private static class _CryptographicOperationsPolicy
    {
        private const string CryptoKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";

        private const string CngKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002";

        private const string FipsKey =
            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cryptops-enable-fips-mode",
                    Label = "Cryptographic: Enable FIPS 140-2 Compliant Algorithm Mode",
                    Category = "Encryption",
                    Description =
                        "Sets Enabled=1 in FipsAlgorithmPolicy. Activates Windows FIPS 140-2 compliant algorithm mode. FIPS mode restricts all cryptographic operations to NIST-validated algorithms: AES (128/192/256 bit), 3DES (112-bit effective), SHA-1/SHA-256/SHA-384/SHA-512, RSA, and ECDH. It disables non-FIPS algorithms including RC4, MD5, DES, and any non-validated implementations. Required by US Federal Government agencies, DoD, HIPAA-compliant healthcare, and certain financial institutions.",
                    Tags = ["fips", "cryptography", "compliance", "federal", "algorithm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "FIPS mode breaks some applications using non-FIPS algorithms (RC4, MD5, non-validated TLS). Test all business applications before enabling in production. Known to break: some Java apps, older .NET apps, certain VPN clients that use RC4.",
                    ApplyOps = [RegOp.SetDword(FipsKey, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(FipsKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(FipsKey, "Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "cryptops-disable-rc4-cipher",
                    Label = "Cryptographic: Disable RC4 Cipher in TLS/DTLS Negotiation",
                    Category = "Encryption",
                    Description =
                        "Sets RC4Enabled=0 in Cryptography policy. Removes RC4 from the TLS cipher suite negotiation list. RC4 is broken: statistical biases in its keystream have been exploited in the RC4 NOMORE attack (86 hours to decrypt a cookie in RC4-protected TLS). IETF prohibited RC4 in TLS in RFC 7465 (2015). Despite the RFC prohibition, RC4 remains enabled on Windows by default for backwards compatibility. Explicitly disabling RC4 enforces the RFC 7465 prohibition in the Windows Schannel TLS provider.",
                    Tags = ["cryptography", "rc4", "tls", "cipher", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "RC4 is removed from TLS negotiation. Very old clients that offer only RC4 cannot connect. All TLS 1.2+ clients support AES-based cipher suites.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "RC4Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "RC4Enabled")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "RC4Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "cryptops-disable-md5-signature",
                    Label = "Cryptographic: Disable MD5 for Digital Signature Verification",
                    Category = "Encryption",
                    Description =
                        "Sets MD5SignatureEnabled=0 in Cryptography policy. Disables MD5 signatures in the Windows cryptographic infrastructure's digital signature verification pipeline. MD5 is cryptographically broken (collision attacks are practical): forged X.509 certificates signed with MD5 have been demonstrated in academic research (the Rogue CA attack in 2008). Any certificate in the chain using MD5 is treated as invalid. Windows already rejects MD5 certificates in many contexts; this policy extends the restriction.",
                    Tags = ["cryptography", "md5", "signature", "certificate", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "MD5-signed certificates are rejected. Verify that no internal CA certs or code-signing certs in the environment use MD5. The public PKI has been using SHA-256 since 2017.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "MD5SignatureEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "MD5SignatureEnabled")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "MD5SignatureEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "cryptops-set-min-rsa-key-length",
                    Label = "Cryptographic: Enforce Minimum 2048-bit RSA Key Length",
                    Category = "Encryption",
                    Description =
                        "Sets MinRsaKeyLength=2048 in Cryptography policy. Rejects any RSA cryptographic operation (key generation, signature verification, key exchange) using keys shorter than 2048 bits. NIST deprecated 1024-bit RSA in 2010 (Special Publication 800-131A); keys of this length are attackable by well-resourced adversaries using GNFS factoring. 2048-bit RSA provides approximately 112 bits of security and is the current minimum for new deployments through 2030.",
                    Tags = ["cryptography", "rsa", "key-length", "pki", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Rejects operations with RSA keys <2048 bits. Verify that no legacy certificates in the environment use 1024-bit RSA. Old code signing, S/MIME, or VPN certs may need renewal.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "MinRsaKeyLength", 2048)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "MinRsaKeyLength")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "MinRsaKeyLength", 2048)],
                },
                new TweakDef
                {
                    Id = "cryptops-enable-crl-check",
                    Label = "Cryptographic: Enable CRL Check on All Certificate Verification",
                    Category = "Encryption",
                    Description =
                        "Sets CertificateRevocationEnabled=1 in Cryptography policy. Enables CRL (Certificate Revocation List) checking for all certificate verification operations. When a certificate is presented for authentication or TLS, Windows checks the issuing CA's CRL distribution point to verify the certificate has not been revoked. Without CRL checking, revoked certificates (e.g., from a compromised private key) remain functional. This is a mandatory control in PKI-secured environments.",
                    Tags = ["cryptography", "crl", "certificate", "revocation", "pki"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "CRL download failures (network unavailable during cert verification) cause authentication failure. Ensure CDP/OCSP endpoints are accessible or implement OCSP caching.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "CertificateRevocationEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "CertificateRevocationEnabled")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "CertificateRevocationEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "cryptops-disable-null-ciphers",
                    Label = "Cryptographic: Disable NULL Cipher Suite in TLS",
                    Category = "Encryption",
                    Description =
                        "Sets NullCipherEnabled=0 in Cryptography policy. Removes all NULL encryption cipher suites from TLS/SSL negotiation. NULL cipher suites (TLS_RSA_WITH_NULL_SHA, etc.) perform authentication and integrity checking but transmit payload data in plaintext. While uncommon in practice, NULL ciphers in the cipher suite list represent a denial-of-confidentiality risk: a man-in-the-middle that can manipulate TLS negotiation could force both parties to select a NULL cipher, establishing an authenticated but unencrypted channel.",
                    Tags = ["cryptography", "null-cipher", "tls", "plaintext", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "NULL ciphers are removed. No legitimate application should use NULL ciphers; this setting should have no operational impact.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "NullCipherEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "NullCipherEnabled")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "NullCipherEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "cryptops-require-strong-key-protection",
                    Label = "Cryptographic: Require Strong Key Protection UI for Private Keys",
                    Category = "Encryption",
                    Description =
                        "Sets ForceKeyProtection=2 in Cryptography policy. Configures the CSP to require explicit user interaction (a password prompt) every time a private key is accessed from the Windows certificate store. With strong protection, private keys cannot be silently accessed by malware running in the user context without triggering a UI prompt that requires button click or password entry. This detects and prevents silent RSA private key use by credential-theft tools (e.g., mimikatz crypto module).",
                    Tags = ["cryptography", "private-key", "protection", "key-guard", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Private key use prompts appear at every signature or decryption operation (email signing, VPN auth). Users with S/MIME-signed email or certificate-based auth will see frequent prompts.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "ForceKeyProtection", 2)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "ForceKeyProtection")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "ForceKeyProtection", 2)],
                },
                new TweakDef
                {
                    Id = "cryptops-disable-sha1-server-auth",
                    Label = "Cryptographic: Disable SHA-1 for Server Authentication Certificates",
                    Category = "Encryption",
                    Description =
                        "Sets SHA1ServerAuthEnabled=0 in Cryptography policy. Rejects SHA-1 signed server authentication certificates from TLS handshakes. SHA-1 has been practically broken since 2017 (Google's SHAttered attack demonstrated a SHA-1 collision for $75,000 in cloud compute). Major CAs stopped issuing SHA-1 certs in 2016; public trust anchors no longer accept SHA-1. Internal PKI CAs that still issue SHA-1 server certs should be upgraded to SHA-256.",
                    Tags = ["cryptography", "sha1", "tls", "certificate", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "SHA-1 server certs are rejected. Internal web applications with SHA-1 certs will fail TLS. Audit internal CA to identify SHA-1 certs before enabling.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "SHA1ServerAuthEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "SHA1ServerAuthEnabled")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "SHA1ServerAuthEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "cryptops-enable-pkcs11-interface",
                    Label = "Cryptographic: Enable PKCS#11 Hardware Token Interface",
                    Category = "Encryption",
                    Description =
                        "Sets EnablePkcs11=1 in Cryptography policy. Registers the Windows PKCS#11 bridge layer, enabling applications that use the PKCS#11 (Cryptoki) standard hardware security module API to use Windows-managed smart cards and Trusted Platform Module (TPM) key storage via a unified interface. This is required in environments deploying hardware security tokens for code signing, SSH key storage, or network authentication (e.g., PIV smart cards, YubiKey HSM).",
                    Tags = ["cryptography", "pkcs11", "smart-card", "hsm", "token"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables PKCS#11 bridge. Required for hardware token integration. No impact if no PKCS#11 applications are deployed.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "EnablePkcs11", 1)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "EnablePkcs11")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "EnablePkcs11", 1)],
                },
                new TweakDef
                {
                    Id = "cryptops-disable-export-of-user-keys",
                    Label = "Cryptographic: Prevent Export of User Private Keys from Key Store",
                    Category = "Encryption",
                    Description =
                        "Sets AllowKeyExport=0 in Cryptography policy. Prevents the export of user private keys from the Windows certificate store to PFX files. Private key export is a credential theft vector: an attacker with user-level access can export the user's private email signing key, code signing key, or authentication certificate to a PFX file and exfiltrate it. Keys stored in non-exportable containers provide in-place security; the private key cannot be removed from the machine's key storage even by the key owner.",
                    Tags = ["cryptography", "key-export", "private-key", "dlp", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Private keys cannot be exported. Users cannot back up their private keys or move them to another device. Ensure key archival is handled by the enterprise CA (key recovery) before enabling.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "AllowKeyExport", 0)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "AllowKeyExport")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "AllowKeyExport", 0)],
                },
            ];

    }

    // ── EfsEncryptionPolicy ──
    private static class _EfsEncryptionPolicy
    {
        private const string Efs = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\EFS";
        private const string EfsAdv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnhancedStorageDevices";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "efspol-disable-efs",
                Label = "Disable EFS (Encrypting File System)",
                Category = "Encryption",
                Description = "Disables the Encrypting File System (EFS) on all NTFS volumes. Prevents users from encrypting files with EFS — useful when BitLocker is the mandated encryption solution and EFS would create conflicting or unmanaged encryption. Default: 0 (enabled). Recommended: 1 for BitLocker-only environments.",
                Tags = ["efs", "encryption", "filesystem", "ntfs", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Efs],
                ApplyOps   = [RegOp.SetDword(Efs, "EfsConfiguration", 1)],
                RemoveOps  = [RegOp.DeleteValue(Efs, "EfsConfiguration")],
                DetectOps  = [RegOp.CheckDword(Efs, "EfsConfiguration", 1)],
            },
            new TweakDef
            {
                Id = "efspol-disable-cert-request",
                Label = "Disable EFS Certificate Request UI",
                Category = "Encryption",
                Description = "Suppresses the EFS certificate request dialog box when a user encrypts a file and no valid EFS certificate exists. Prevents ad-hoc self-signed EFS certificates from being created outside of PKI control. Default: 0. Recommended: 1.",
                Tags = ["efs", "certificate", "pki", "encryption"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Efs],
                ApplyOps   = [RegOp.SetDword(Efs, "NoCertRequest", 1)],
                RemoveOps  = [RegOp.DeleteValue(Efs, "NoCertRequest")],
                DetectOps  = [RegOp.CheckDword(Efs, "NoCertRequest", 1)],
            },
            new TweakDef
            {
                Id = "efspol-enable-page-file-encryption",
                Label = "Encrypt Page File via EFS Policy",
                Category = "Encryption",
                Description = "Enforces page file encryption at system level, preventing sensitive data in virtual memory from being read from the page file on disk after shutdown or hibernation. Default: 0. Recommended: 1.",
                Tags = ["efs", "page-file", "encryption", "memory", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Efs],
                ApplyOps   = [RegOp.SetDword(Efs, "EfsEncryptPageFiles", 1)],
                RemoveOps  = [RegOp.DeleteValue(Efs, "EfsEncryptPageFiles")],
                DetectOps  = [RegOp.CheckDword(Efs, "EfsEncryptPageFiles", 1)],
            },
            new TweakDef
            {
                Id = "efspol-set-cache-timeout",
                Label = "Set EFS Key Cache Timeout to 8 Hours",
                Category = "Encryption",
                Description = "Sets the EFS key cache timeout to 28 800 seconds (8 hours). After this period of inactivity the EFS private key is evicted from memory, requiring re-authentication before encrypted files can be opened. Default: not set. Recommended: 28800.",
                Tags = ["efs", "cache", "key", "security", "timeout"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Efs],
                ApplyOps   = [RegOp.SetDword(Efs, "CacheTimeOut", 28800)],
                RemoveOps  = [RegOp.DeleteValue(Efs, "CacheTimeOut")],
                DetectOps  = [RegOp.CheckDword(Efs, "CacheTimeOut", 28800)],
            },
            new TweakDef
            {
                Id = "efspol-require-smart-card",
                Label = "Require Smart Card for EFS Key Storage",
                Category = "Encryption",
                Description = "Forces EFS to use hardware-backed smart card key storage instead of software keys. Ensures EFS encryption keys are protected by hardware rather than being stored in the software key store. Default: 0. Recommended: 1 for high-security PKI environments.",
                Tags = ["efs", "smart-card", "pki", "hardware", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Efs],
                ApplyOps   = [RegOp.SetDword(Efs, "FIPSRequired", 1)],
                RemoveOps  = [RegOp.DeleteValue(Efs, "FIPSRequired")],
                DetectOps  = [RegOp.CheckDword(Efs, "FIPSRequired", 1)],
            },
            new TweakDef
            {
                Id = "efspol-disable-enhanced-storage-legacy",
                Label = "Disallow Legacy Devices in Enhanced Storage",
                Category = "Encryption",
                Description = "Blocks non-IEEE-1667–compliant (legacy) USB storage devices from being used as enhanced storage targets. Forces use of only certified IEEE-1667 hardware-encrypted storage devices. Default: 0. Recommended: 1.",
                Tags = ["efs", "enhanced-storage", "usb", "hardware", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EfsAdv],
                ApplyOps   = [RegOp.SetDword(EfsAdv, "DisallowLegacyDevices", 1)],
                RemoveOps  = [RegOp.DeleteValue(EfsAdv, "DisallowLegacyDevices")],
                DetectOps  = [RegOp.CheckDword(EfsAdv, "DisallowLegacyDevices", 1)],
            },
            new TweakDef
            {
                Id = "efspol-disable-enhanced-storage-1394",
                Label = "Disallow IEEE 1394 Enhanced Storage Devices",
                Category = "Encryption",
                Description = "Denies use of IEEE 1394 (FireWire) enhanced-storage devices as encryption targets. Eliminates a legacy port-based attack surface available through IEEE 1394 DMA. Default: 0. Recommended: 1.",
                Tags = ["efs", "enhanced-storage", "firewire", "ieee1394", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EfsAdv],
                ApplyOps   = [RegOp.SetDword(EfsAdv, "Deny1394Devices", 1)],
                RemoveOps  = [RegOp.DeleteValue(EfsAdv, "Deny1394Devices")],
                DetectOps  = [RegOp.CheckDword(EfsAdv, "Deny1394Devices", 1)],
            },
            new TweakDef
            {
                Id = "efspol-require-password-silo",
                Label = "Require Password Silo Certificate for Enhanced Storage",
                Category = "Encryption",
                Description = "Requires a password silo certificate (from organizational CA) before access to enhanced storage devices is granted. Prevents use of consumer/personal enhanced storage in enterprise environments. Default: 0. Recommended: 1.",
                Tags = ["efs", "enhanced-storage", "certificate", "silo", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EfsAdv],
                ApplyOps   = [RegOp.SetDword(EfsAdv, "RootHubConnectedEnStorDevices", 0)],
                RemoveOps  = [RegOp.DeleteValue(EfsAdv, "RootHubConnectedEnStorDevices")],
                DetectOps  = [RegOp.CheckDword(EfsAdv, "RootHubConnectedEnStorDevices", 0)],
            },
            new TweakDef
            {
                Id = "efspol-lock-enhanced-storage-on-lock",
                Label = "Lock Enhanced Storage on Workstation Lock",
                Category = "Encryption",
                Description = "Locks (re-locks) all connected enhanced storage devices when the workstation is locked (Win+L, screensaver, idle). Ensures encrypted USB storage is inaccessible without re-authentication after lock. Default: 0. Recommended: 1.",
                Tags = ["efs", "enhanced-storage", "lock", "usb", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EfsAdv],
                ApplyOps   = [RegOp.SetDword(EfsAdv, "LockDeviceOnMachineLock", 1)],
                RemoveOps  = [RegOp.DeleteValue(EfsAdv, "LockDeviceOnMachineLock")],
                DetectOps  = [RegOp.CheckDword(EfsAdv, "LockDeviceOnMachineLock", 1)],
            },
            new TweakDef
            {
                Id = "efspol-disable-enhanced-storage-device-list",
                Label = "Restrict Enhanced Storage to Approved Devices Only",
                Category = "Encryption",
                Description = "When set, only enhanced storage devices whose identity matches organizational approved entries are allowed. All unapproved hardware-encrypted USB drives are blocked. Default: 0. Recommended: 1 for controlled hardware environments.",
                Tags = ["efs", "enhanced-storage", "allowlist", "usb", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [EfsAdv],
                ApplyOps   = [RegOp.SetDword(EfsAdv, "TCGSecurityActivationDisabled", 0)],
                RemoveOps  = [RegOp.DeleteValue(EfsAdv, "TCGSecurityActivationDisabled")],
                DetectOps  = [RegOp.CheckDword(EfsAdv, "TCGSecurityActivationDisabled", 0)],
            },
        ];

    }

    // ── FipsCompliancePolicy ──
    private static class _FipsCompliancePolicy
    {
        private const string FipsKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy";
        private const string CryptoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "fips-enable-fips-algorithm-policy",
                Label = "FIPS Compliance: Enable FIPS 140-2 Compliant Algorithms",
                Category = "Encryption",
                Description = "Enables the FIPS 140-2 compliant algorithm policy (FipsAlgorithmPolicy\\Enabled=1), which forces Windows security subsystems including TLS, Schannel, BitLocker, and Remote Desktop to use only FIPS-approved cryptographic algorithms. Required by NIST SP 800-53, FedRAMP, DoD STIGs, and PCI-DSS environments. Note: some older COM components and third-party applications may fail when this is enforced.",
                Tags = ["fips", "cryptography", "compliance", "security", "nist"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [FipsKey],
                ApplyOps = [RegOp.SetDword(FipsKey, "Enabled", 1)],
                RemoveOps = [RegOp.SetDword(FipsKey, "Enabled", 0)],
                DetectOps = [RegOp.CheckDword(FipsKey, "Enabled", 1)],
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Enforces FIPS-approved algorithms system-wide; may break non-FIPS-compliant legacy applications.",
            },
            new TweakDef
            {
                Id = "fips-disable-machine-key-caching",
                Label = "FIPS Compliance: Disable Machine Key Caching",
                Category = "Encryption",
                Description = "Disables caching of machine-level CNG (Cryptography Next Generation) private keys in memory. When enabled, machine keys can remain in memory after their first use for performance reasons, but this creates a window where a privileged attacker or malicious driver could extract cached key material. Disabling caching forces fresh key derivation on each use, aligning with zero-trust key management practices.",
                Tags = ["fips", "cryptography", "machine key", "caching", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "MachineKeyCachingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "MachineKeyCachingEnabled")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "MachineKeyCachingEnabled", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Removes in-memory key caching; minor performance penalty on crypto-heavy workloads.",
            },
            new TweakDef
            {
                Id = "fips-force-strong-key-protection",
                Label = "FIPS Compliance: Force Strong Key Protection for User Keys",
                Category = "Encryption",
                Description = "Configures CNG to require strong key protection (ForceKeyProtection=2), which means private keys stored in the user's personal certificate store can only be accessed after the user explicitly provides their password. This prevents silent background access to private keys by automated processes, scheduled tasks, or potentially compromised processes running in the user context without the user's consent.",
                Tags = ["fips", "cryptography", "key protection", "certificates", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "ForceKeyProtection", 2)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "ForceKeyProtection")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "ForceKeyProtection", 2)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Requires user password for each private key access; S/MIME signing and code signing may prompt more frequently.",
            },
            new TweakDef
            {
                Id = "fips-disable-dpapi-auto-protection",
                Label = "FIPS Compliance: Restrict DPAPI Automatic Data Protection",
                Category = "Encryption",
                Description = "Restricts the Data Protection API (DPAPI) from automatically enrolling new data blobs with default key generation settings that may not be FIPS-compliant. DPAPI is used by browsers (Chrome/Edge), mail clients, and credential managers to protect saved passwords and tokens. When FIPS mode is enabled but DPAPI is not restricted, legacy blobs may still be created using non-FIPS algorithms.",
                Tags = ["fips", "cryptography", "dpapi", "data protection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "UseDPAPIForProtection", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "UseDPAPIForProtection")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "UseDPAPIForProtection", 0)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Restricts default DPAPI auto-protection; applications that depend on DPAPI for secret storage may need reconfiguration.",
            },
            new TweakDef
            {
                Id = "fips-require-sha2-minimum",
                Label = "FIPS Compliance: Require SHA-2 Minimum for Code Integrity",
                Category = "Encryption",
                Description = "Sets the minimum hash algorithm for Authenticode code signing to SHA-2 (SHA-256 or better), blocking execution of binaries signed only with SHA-1 or MD5. SHA-1 signatures have been deprecated by NIST since 2011 and are considered vulnerable to collision attacks. This policy aligns code integrity checking with FIPS 140-2 Annex A requirements.",
                Tags = ["fips", "cryptography", "sha2", "code signing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "SHA1DeprecationPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "SHA1DeprecationPolicy")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "SHA1DeprecationPolicy", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks SHA-1 signed code from executing; older drivers or software signed before 2016 may not load.",
            },
            new TweakDef
            {
                Id = "fips-disable-rc4-tls",
                Label = "FIPS Compliance: Disable RC4 Cipher in TLS",
                Category = "Encryption",
                Description = "Disables the RC4 stream cipher from the TLS negotiation cipher suite list at the Schannel policy level. RC4 is not FIPS-approved (NSA Suite B) and has been proven vulnerable to statistical attacks (BEAST, RC4NOMORE). Windows Server 2012 R2 and newer disable it by default, but this policy explicitly sets the registry value to ensure RC4 cannot be re-enabled by custom application cipher suite negotiation.",
                Tags = ["fips", "cryptography", "rc4", "tls", "schannel"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "DisableRC4InTLS", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "DisableRC4InTLS")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "DisableRC4InTLS", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Explicitly blocks RC4 in TLS; RC4 was already deprecated in modern Windows, so no behavioral change on up-to-date systems.",
            },
            new TweakDef
            {
                Id = "fips-disable-weak-hash-algorithms",
                Label = "FIPS Compliance: Disable MD5 and MD4 Hash Algorithms",
                Category = "Encryption",
                Description = "Disables MD5 and MD4 hash algorithms from being used by the Windows CNG key storage provider for new operations. MD5 and MD4 are not FIPS-approved; both have known collision vulnerabilities. While MD5 is still widely used in non-security contexts (file checksums), its use in cryptographic operations (certificate fingerprints, HMAC) must be blocked in FIPS-compliant environments.",
                Tags = ["fips", "cryptography", "md5", "hash algorithms", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "DisableWeakHashAlgorithms", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "DisableWeakHashAlgorithms")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "DisableWeakHashAlgorithms", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks MD5/MD4 in cryptographic operations; non-crypto checksums (file integrity comparison) are unaffected.",
            },
            new TweakDef
            {
                Id = "fips-disable-des-3des-cipher",
                Label = "FIPS Compliance: Restrict DES and 3DES Ciphers",
                Category = "Encryption",
                Description = "Restricts use of DES (56-bit) and Triple-DES (3DES/TDEA) block ciphers in new cryptographic sessions. DES has been disallowed by NIST since 2005. 3DES (with 112-bit effective security) was deprecated by NIST SP 800-131A in 2023 for new use and is only approved through 2023 for legacy compatibility. This policy aligns the cipher suite with the requirement for AES-256 minimum.",
                Tags = ["fips", "cryptography", "des", "3des", "cipher policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "RestrictDESAlgorithms", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "RestrictDESAlgorithms")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "RestrictDESAlgorithms", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Restricts DES/3DES ciphers; may break connections to legacy servers that do not support AES.",
            },
            new TweakDef
            {
                Id = "fips-require-tls-certificate-validation",
                Label = "FIPS Compliance: Require Full Certificate Chain Validation",
                Category = "Encryption",
                Description = "Requires that all TLS connections validate the complete certificate chain including OCSP stapling verification and CRL distribution point checks. In FIPS environments, certificate validation must be comprehensive — weak pinning, expired status checks, or bypassed revocation checks can introduce attack vectors. This policy sets the strict validation mode for the Windows Schannel certificate validation path.",
                Tags = ["fips", "cryptography", "certificate", "tls validation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "RequireFullCertChainValidation", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "RequireFullCertChainValidation")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "RequireFullCertChainValidation", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Requires full OCSP/CRL chain validation; connections to servers with incomplete chains will fail.",
            },
            new TweakDef
            {
                Id = "fips-enforce-secure-channel-minimum",
                Label = "FIPS Compliance: Enforce Minimum Secure Channel Protocol Version",
                Category = "Encryption",
                Description = "Enforces a minimum TLS 1.2 protocol version for all Windows Schannel connections, preventing fallback to SSL 3.0, TLS 1.0, or TLS 1.1. All three older versions have documented protocol-level vulnerabilities (POODLE, BEAST, DROWN, BEAST) that allow decryption of traffic by an active network attacker. FIPS 140-2 references NIST SP 800-52 which mandates TLS 1.2 as the minimum for federal systems.",
                Tags = ["fips", "cryptography", "tls", "secure channel", "minimum version"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CryptoKey],
                ApplyOps = [RegOp.SetDword(CryptoKey, "EnforceMinimumTlsVersion", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "EnforceMinimumTlsVersion")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "EnforceMinimumTlsVersion", 1)],
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Enforces TLS 1.2+ minimum; connections to servers that only support TLS 1.0/1.1 or SSL 3.0 will fail.",
            },
        ];

    }

    // ── HvciPolicy ──
    private static class _HvciPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "hvci-set-policy-level-strict",
                    Label = "Set HVCI Code Integrity Policy to Strict",
                    Category = "Encryption",
                    Description =
                        "Sets the Code Integrity policy level to Strict via the CI\\Policy key, blocking DLL injections and kernel-mode payloads that exploit unsigned code paths not caught by the default policy.",
                    Tags = ["hvci", "code-integrity", "strict", "driver", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "Strict CI policy; unsigned kernel/user-mode injections blocked. Test pre-rollout; may break old software.",
                    ApplyOps = [RegOp.SetDword(Key, "SkipInvalidUnattendSpecPass", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SkipInvalidUnattendSpecPass")],
                    DetectOps = [RegOp.CheckDword(Key, "SkipInvalidUnattendSpecPass", 0)],
                },
                new TweakDef
                {
                    Id = "hvci-block-driver-vulnerability-list",
                    Label = "Enable Vulnerable Driver Blocklist",
                    Category = "Encryption",
                    Description =
                        "Enables the Microsoft Vulnerable Driver Blocklist (also built into Windows Security Center) via the CI policy, preventing known exploitable drivers from loading regardless of signature status.",
                    Tags = ["hvci", "driver-blocklist", "vulnerable-drivers", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Vulnerable drivers on the Microsoft blocklist cannot load; prevents BYOVD kernel exploits.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPolicyUpdateTaskEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPolicyUpdateTaskEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPolicyUpdateTaskEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-enable-ci-flight-check",
                    Label = "Enable Code Integrity Flight Signing Check",
                    Category = "Encryption",
                    Description =
                        "Enables flight signing checks in CI policy, ensuring Windows Insider / pre-release kernel updates still pass code integrity verification while on production builds.",
                    Tags = ["hvci", "flight-signing", "ci", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Flight signing verified; CI policy does not break on pre-release or insider kernel updates.",
                    ApplyOps = [RegOp.SetDword(Key, "FlightSigningEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "FlightSigningEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "FlightSigningEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-block-dev-mode-km-bypass",
                    Label = "Block Developer Mode Kernel Bypass of HVCI",
                    Category = "Encryption",
                    Description =
                        "Prevents Developer Mode (Sideload Apps / Test Signing) from bypassing HVCI code integrity enforcement, ensuring HVCI cannot be defeated by enabling Developer Mode.",
                    Tags = ["hvci", "developer-mode", "test-signing", "bypass", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Developer Mode cannot disable HVCI; test-signed drivers still blocked on locked-down machines.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockDevModeHVCIBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockDevModeHVCIBypass")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockDevModeHVCIBypass", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-disable-kernel-debug-bypass",
                    Label = "Disable Kernel Debugging Bypass of Code Integrity",
                    Category = "Encryption",
                    Description =
                        "Prevents kernel debugger attachment from disabling HVCI code integrity checks, ensuring that even a live kernel debug session cannot load unsigned drivers.",
                    Tags = ["hvci", "kernel-debug", "code-integrity", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Kernel debug mode cannot bypass CI; active kernel debugging will not load unsigned code.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableKernelDebugCIBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableKernelDebugCIBypass")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableKernelDebugCIBypass", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-enable-user-mode-ci",
                    Label = "Enable User-Mode Code Integrity (UMCI)",
                    Category = "Encryption",
                    Description =
                        "Extends code integrity enforcement to user mode via UMCI, requiring all user-mode executables and DLLs to be signed, providing application whitelisting at the OS policy level.",
                    Tags = ["hvci", "umci", "user-mode", "code-integrity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "UMCI enforced; all user-mode binaries require signatures. Breaks most unsigned applications.",
                    ApplyOps = [RegOp.SetDword(Key2, "UMCIEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "UMCIEnabled")],
                    DetectOps = [RegOp.CheckDword(Key2, "UMCIEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-block-hmac-degradation",
                    Label = "Block HMAC Algorithm Downgrade in CI Validation",
                    Category = "Encryption",
                    Description =
                        "Prevents CI signature validation from falling back to weak legacy HMAC algorithms, ensuring code integrity checks always use strong cryptographic hashing.",
                    Tags = ["hvci", "hmac", "cryptography", "downgrade", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Weak HMAC fallback in CI disabled; old binaries signed with MD5/SHA1 only may fail verification.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockHMACDowngrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockHMACDowngrade")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockHMACDowngrade", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-enforce-efi-boot-driver-check",
                    Label = "Enforce EFI Boot Driver Code Integrity Check",
                    Category = "Encryption",
                    Description =
                        "Extends HVCI enforcement to EFI boot-time drivers loaded by the firmware, ensuring CI policy covers the entire boot chain and not just post-HORM drivers.",
                    Tags = ["hvci", "efi", "boot-driver", "secure-boot", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "EFI boot driver integrity verified; unsigned EFI modules blocked before OS handoff.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnforceEFIBootDriverCI", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnforceEFIBootDriverCI")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnforceEFIBootDriverCI", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-block-ci-opt-out-for-drivers",
                    Label = "Block Per-Driver CI Opt-Out Flag",
                    Category = "Encryption",
                    Description =
                        "Prevents individual drivers from setting a CI opt-out flag in their INF to bypass HVCI verification, ensuring the CI policy cannot be weakened driver-by-driver.",
                    Tags = ["hvci", "driver", "opt-out", "bypass-prevention", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Per-driver CI opt-out disallowed; all drivers must comply with CI policy uniformly.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockCIOptOutForDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockCIOptOutForDrivers")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockCIOptOutForDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "hvci-enable-ci-policy-telemetry",
                    Label = "Enable CI Policy Violation Telemetry Reporting",
                    Category = "Encryption",
                    Description =
                        "Enables reporting of Code Integrity policy violations to Windows Defender ATP / Microsoft Defender for Endpoint, supporting cloud-based detection of BYOVD and LOLBAS attack patterns.",
                    Tags = ["hvci", "telemetry", "defender", "atp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "CI violation telemetry sent to MDE; security team can detect driver-based attacks from the cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCIPolicyTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCIPolicyTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCIPolicyTelemetry", 1)],
                },
            ];

    }

    // ── MemoryIntegrityPolicy ──
    private static class _MemoryIntegrityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "memintg-enable-hvci",
                Label = "Enable Hypervisor-Protected Code Integrity (HVCI/Memory Integrity)",
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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
                Category = "Encryption",
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

    // ── PersonalDataEncryptionPolicy ──
    private static class _PersonalDataEncryptionPolicy
    {
        private const string PdeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PDE";
        private const string PdeFoldersKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PDE\ProtectedFolders";
        private const string PdeDeviceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PDE\Device";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "pde-enable-personal-data-encryption",
                Label = "Enable Personal Data Encryption",
                Category = "Encryption",
                Description = "Enables Personal Data Encryption (PDE) on the device, protecting user files in selected folders with keys tied to the signed-in user identity. Requires Windows Hello for Business.",
                Tags = ["pde", "encryption", "personal-data", "security", "windows-11"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Files in protected folders become inaccessible until the user authenticates via Windows Hello; improves data security at rest.",
                RegistryKeys = [PdeKey],
                ApplyOps  = [RegOp.SetDword(PdeKey, "EnablePersonalDataEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeKey, "EnablePersonalDataEncryption")],
                DetectOps = [RegOp.CheckDword(PdeKey, "EnablePersonalDataEncryption", 1)],
            },
            new TweakDef
            {
                Id = "pde-require-device-encryption-prereq",
                Label = "Require BitLocker as PDE Prerequisite",
                Category = "Encryption",
                Description = "Requires BitLocker drive encryption to be active before Personal Data Encryption can be applied to user folders. Ensures defense-in-depth for protected content.",
                Tags = ["pde", "encryption", "bitlocker", "prerequisite", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces layered encryption: BitLocker protects the drive, PDE protects individual user files.",
                RegistryKeys = [PdeKey],
                ApplyOps  = [RegOp.SetDword(PdeKey, "RequireDeviceEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeKey, "RequireDeviceEncryption")],
                DetectOps = [RegOp.CheckDword(PdeKey, "RequireDeviceEncryption", 1)],
            },
            new TweakDef
            {
                Id = "pde-block-network-content-access",
                Label = "Block PDE Content Access from Network Accounts",
                Category = "Encryption",
                Description = "Prevents network service accounts and remote processes from accessing folders protected by Personal Data Encryption, limiting access to the locally signed-in user.",
                Tags = ["pde", "encryption", "network", "access-control", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops backup agents or network-based tools from reading PDE-protected files when the owning user is not signed in.",
                RegistryKeys = [PdeKey],
                ApplyOps  = [RegOp.SetDword(PdeKey, "BlockNetworkAccessToPDEContent", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeKey, "BlockNetworkAccessToPDEContent")],
                DetectOps = [RegOp.CheckDword(PdeKey, "BlockNetworkAccessToPDEContent", 1)],
            },
            new TweakDef
            {
                Id = "pde-wipe-keys-on-lock",
                Label = "Wipe PDE Keys on Device Lock",
                Category = "Encryption",
                Description = "Instructs Windows to purge in-memory Personal Data Encryption keys when the device screen locks. Files remain encrypted and inaccessible until the user unlocks with Windows Hello.",
                Tags = ["pde", "encryption", "lock-screen", "key-management", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Reduces the window during which PDE keys are resident in memory after the device is left unattended.",
                RegistryKeys = [PdeKey],
                ApplyOps  = [RegOp.SetDword(PdeKey, "WipeKeysOnLock", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeKey, "WipeKeysOnLock")],
                DetectOps = [RegOp.CheckDword(PdeKey, "WipeKeysOnLock", 1)],
            },
            new TweakDef
            {
                Id = "pde-protect-desktop-folder",
                Label = "Enable PDE Protection for Desktop Folder",
                Category = "Encryption",
                Description = "Applies Personal Data Encryption to the user's Desktop folder, ensuring files placed on the desktop are encrypted with the user's Windows Hello identity key.",
                Tags = ["pde", "encryption", "desktop", "folder-protection", "windows-11"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Desktop files are frequently targeted; encrypting the Desktop folder prevents offline access by attackers with physical access.",
                RegistryKeys = [PdeFoldersKey],
                ApplyOps  = [RegOp.SetDword(PdeFoldersKey, "ProtectDesktopFolder", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeFoldersKey, "ProtectDesktopFolder")],
                DetectOps = [RegOp.CheckDword(PdeFoldersKey, "ProtectDesktopFolder", 1)],
            },
            new TweakDef
            {
                Id = "pde-protect-documents-folder",
                Label = "Enable PDE Protection for Documents Folder",
                Category = "Encryption",
                Description = "Applies Personal Data Encryption to the user's Documents folder. Files are encrypted with user identity keys tied to Windows Hello, preventing offline access without user authentication.",
                Tags = ["pde", "encryption", "documents", "folder-protection", "windows-11"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Documents folder often contains sensitive business data; PDE protection prevents access when the device is lost or stolen.",
                RegistryKeys = [PdeFoldersKey],
                ApplyOps  = [RegOp.SetDword(PdeFoldersKey, "ProtectDocumentsFolder", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeFoldersKey, "ProtectDocumentsFolder")],
                DetectOps = [RegOp.CheckDword(PdeFoldersKey, "ProtectDocumentsFolder", 1)],
            },
            new TweakDef
            {
                Id = "pde-protect-pictures-folder",
                Label = "Enable PDE Protection for Pictures Folder",
                Category = "Encryption",
                Description = "Applies Personal Data Encryption to the user's Pictures folder, protecting images and media from offline access on lost or stolen devices.",
                Tags = ["pde", "encryption", "pictures", "folder-protection", "windows-11"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Protects personal images from exposure during device repairs or after physical theft.",
                RegistryKeys = [PdeFoldersKey],
                ApplyOps  = [RegOp.SetDword(PdeFoldersKey, "ProtectPicturesFolder", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeFoldersKey, "ProtectPicturesFolder")],
                DetectOps = [RegOp.CheckDword(PdeFoldersKey, "ProtectPicturesFolder", 1)],
            },
            new TweakDef
            {
                Id = "pde-audit-access-events",
                Label = "Enable PDE Access Audit Events",
                Category = "Encryption",
                Description = "Enables Windows to generate audit events when PDE-protected content is accessed or when PDE encryption/decryption operations occur, supporting security monitoring and compliance.",
                Tags = ["pde", "encryption", "audit", "compliance", "event-log"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Provides visibility into who accesses PDE-protected files and when, aiding incident investigation.",
                RegistryKeys = [PdeKey],
                ApplyOps  = [RegOp.SetDword(PdeKey, "EnablePDEAuditEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeKey, "EnablePDEAuditEvents")],
                DetectOps = [RegOp.CheckDword(PdeKey, "EnablePDEAuditEvents", 1)],
            },
            new TweakDef
            {
                Id = "pde-restrict-key-backup",
                Label = "Restrict PDE Key Backup to Organization",
                Category = "Encryption",
                Description = "Limits Personal Data Encryption key backup to organization-controlled Microsoft Entra ID (Azure AD) accounts only, preventing personal Microsoft account key escrow.",
                Tags = ["pde", "encryption", "key-backup", "azure-ad", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ensures only the organization's IT department can facilitate key recovery, not personal Microsoft accounts.",
                RegistryKeys = [PdeDeviceKey],
                ApplyOps  = [RegOp.SetDword(PdeDeviceKey, "RestrictKeyBackupToOrganization", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeDeviceKey, "RestrictKeyBackupToOrganization")],
                DetectOps = [RegOp.CheckDword(PdeDeviceKey, "RestrictKeyBackupToOrganization", 1)],
            },
            new TweakDef
            {
                Id = "pde-require-windows-hello-enrolment",
                Label = "Require Windows Hello Enrollment for PDE",
                Category = "Encryption",
                Description = "Enforces that users must be enrolled in Windows Hello for Business before Personal Data Encryption can be activated on their device. Prevents PDE deployment without modern authentication.",
                Tags = ["pde", "encryption", "windows-hello", "enrollment", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ensures PDE keys are always bound to strong authentication rather than password-only accounts.",
                RegistryKeys = [PdeDeviceKey],
                ApplyOps  = [RegOp.SetDword(PdeDeviceKey, "RequireWindowsHelloEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(PdeDeviceKey, "RequireWindowsHelloEnrollment")],
                DetectOps = [RegOp.CheckDword(PdeDeviceKey, "RequireWindowsHelloEnrollment", 1)],
            },
        ];

    }

    // ── SecureBootDbxPolicy ──
    private static class _SecureBootDbxPolicy
    {
        private const string UefiPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI";

        private const string DeviceGuardKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sbdbx-enable-dbx-automatic-update",
                    Label = "Secure Boot DBX: Enable Automatic Secure Boot Forbidden Signatures (DBX) Update",
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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

    // ── SecureBootPolicy ──
    private static class _SecureBootPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecureBoot";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "secboot-enable-db-update",
                Label = "Enable Secure Boot DB Update",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description =
                    "Sets AllowUpdateOfSecureBootDb=1 in the SecureBoot policy key. Permits "
                    + "Windows Update to deliver updated Secure Boot Allowed (db) and "
                    + "Revocation (dbx) signature databases to the UEFI firmware store. "
                    + "Microsoft periodically revokes compromised boot loaders (e.g., "
                    + "BlackLotus / Storm-0558) via dbx updates; blocking these updates "
                    + "leaves the device vulnerable to known bootkit exploits. "
                    + "Default: 1. Keep at 1 (enabled).",
                Tags = ["secureboot", "db", "dbx", "uefi", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowUpdateOfSecureBootDb", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUpdateOfSecureBootDb")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUpdateOfSecureBootDb", 1)],
            },
            new TweakDef
            {
                Id = "secboot-disable-test-signing",
                Label = "Disable Test-Signing Mode Boot",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 5,
                Description =
                    "Sets DisableTestSigning=1 in the SecureBoot policy key. Prevents the "
                    + "system from booting with the test-signing BCD flag enabled, which "
                    + "would allow loading of drivers signed with test certificates not "
                    + "chained to a trusted production CA. Test-signing mode is a "
                    + "common bypass technique for loading rootkits and unsigned malicious "
                    + "kernel drivers. Default: 0. Recommended: 1.",
                Tags = ["secureboot", "test-signing", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTestSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTestSigning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTestSigning", 1)],
            },
            new TweakDef
            {
                Id = "secboot-enable-user-mode-ci",
                Label = "Enable User-Mode Code Integrity (UMCI)",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 4,
                Description =
                    "Sets EnableUMCI=1 in the SecureBoot policy key. Activates User-Mode "
                    + "Code Integrity enforcement, ensuring that user-mode binaries and "
                    + "scripts are validated against the Windows Driver Certificate policy "
                    + "before execution. UMCI blocks execution of unsigned or revoked "
                    + "binaries and is a key component of Windows Device Guard. "
                    + "Default: 0. Recommended: 1 where an allow-list policy is in place.",
                Tags = ["secureboot", "umci", "device-guard", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableUMCI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableUMCI")],
                DetectOps = [RegOp.CheckDword(Key, "EnableUMCI", 1)],
            },
            new TweakDef
            {
                Id = "secboot-enable-kernel-ci",
                Label = "Enable Kernel Code Integrity Enforcement",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 4,
                Description =
                    "Sets EnableKernelCI=1 in the SecureBoot policy key. Enforces that only "
                    + "WHQL or EV-code-signed kernel-mode drivers are permitted to load "
                    + "during and after the boot sequence. Kernel code integrity blocks "
                    + "unsigned, test-signed, and revoked drivers including legacy "
                    + "rootkits and BYOVD (Bring Your Own Vulnerable Driver) techniques "
                    + "that attackers use as privilege-escalation primitives. Default: 0.",
                Tags = ["secureboot", "kernel-ci", "hvci", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableKernelCI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelCI")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKernelCI", 1)],
            },
            new TweakDef
            {
                Id = "secboot-enable-secinitrd",
                Label = "Enable Secure Initial Ramdisk (secInitrd)",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description =
                    "Sets EnableSecInitRd=1 in the SecureBoot policy key. Requires the "
                    + "Windows Recovery Environment and boot-critical drivers compressed "
                    + "into the initial ramdisk to be signed and validated by the bootmgr "
                    + "before they are decompressed and executed. An unsigned initial "
                    + "ramdisk is a pre-OS persistence vector for bootkits that cannot "
                    + "survive signed ramdisk validation. Default: 0.",
                Tags = ["secureboot", "initrd", "winre", "boot", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSecInitRd", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSecInitRd")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSecInitRd", 1)],
            },
            new TweakDef
            {
                Id = "secboot-disable-network-unlock",
                Label = "Disable Secure Boot Network Unlock",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets DisableNetworkUnlock=1 in the SecureBoot policy key. Prevents "
                    + "BitLocker Network Unlock from releasing the volume encryption key "
                    + "based solely on network presence of a WDS/NPS unlock server, "
                    + "without any user-interactive PIN or key protector challenge. "
                    + "Network Unlock convenience can undermine pre-boot authentication "
                    + "goals if an attacker clones the network identity. Default: 0.",
                Tags = ["secureboot", "network-unlock", "bitlocker", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetworkUnlock", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkUnlock")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetworkUnlock", 1)],
            },
            new TweakDef
            {
                Id = "secboot-enforce-secure-mos-policy",
                Label = "Enforce Secure Managed OS Policy",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 4,
                Description =
                    "Sets EnforceManagedOsPolicy=1 in the SecureBoot policy key. Requires "
                    + "that the device's Secure Boot configuration matches a defined managed "
                    + "OS policy baseline, which includes allowed boot-path signatures, "
                    + "revocation list freshness, and HVCI/VBS state. Deviation from the "
                    + "policy baseline marks the device as out-of-compliance for conditional "
                    + "access gatekeeping. Default: 0.",
                Tags = ["secureboot", "managed-os", "compliance", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceManagedOsPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceManagedOsPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceManagedOsPolicy", 1)],
            },
            new TweakDef
            {
                Id = "secboot-disable-custom-pk",
                Label = "Disable Custom Platform Key Enrollment",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description =
                    "Sets DisableCustomPk=1 in the SecureBoot policy key. Prevents users "
                    + "or local administrators from replacing the UEFI Platform Key (PK) "
                    + "with a custom or self-signed certificate, which would allow the "
                    + "installation of a custom Secure Boot database permitting arbitrary "
                    + "unsigned boot-path code. Custom PK enrollment is the first step in "
                    + "most bootkit persistence chains on managed devices. Default: 0.",
                Tags = ["secureboot", "platform-key", "pk", "uefi", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCustomPk", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCustomPk")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCustomPk", 1)],
            },
            new TweakDef
            {
                Id = "secboot-require-bootloader-revocation",
                Label = "Require Bootloader Revocation Check",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description =
                    "Sets RequireBootloaderRevocationCheck=1 in the SecureBoot policy key. "
                    + "Forces the Windows boot manager to verify the dbx (Forbidden "
                    + "Signature Database) revocation list before launching the OS loader, "
                    + "blocking boot loaders that have been revoked due to known "
                    + "vulnerabilities (e.g., old signed shim binaries used in BlackLotus "
                    + "and similar bootkit campaigns). Default: 0. Recommended: 1.",
                Tags = ["secureboot", "revocation", "dbx", "bootloader", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireBootloaderRevocationCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireBootloaderRevocationCheck")],
                DetectOps = [RegOp.CheckDword(Key, "RequireBootloaderRevocationCheck", 1)],
            },
            new TweakDef
            {
                Id = "secboot-disable-secure-boot-telemetry",
                Label = "Disable Secure Boot Telemetry",
                Category = "Encryption",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets DisableSecureBootTelemetry=1 in the SecureBoot policy key. Stops "
                    + "the Windows boot manager and Secure Boot runtime from emitting "
                    + "telemetry events that report db/dbx database versions, UEFI firmware "
                    + "vendor, and Secure Boot validation outcomes to Microsoft's telemetry "
                    + "pipeline. Firmware vendor identifiers and signing certificate "
                    + "thumbprints constitute device fingerprint data. Default: 0.",
                Tags = ["secureboot", "telemetry", "privacy", "uefi", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSecureBootTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSecureBootTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSecureBootTelemetry", 1)],
            },
        ];

    }

    // ── TlsSchannel ──
    private static class _TlsSchannel
    {
        // SCHANNEL root for all protocol version keys
        private const string SchannelRoot = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL";

        private const string Ssl20Client = SchannelRoot + @"\Protocols\SSL 2.0\Client";
        private const string Ssl20Server = SchannelRoot + @"\Protocols\SSL 2.0\Server";
        private const string Ssl30Client = SchannelRoot + @"\Protocols\SSL 3.0\Client";
        private const string Ssl30Server = SchannelRoot + @"\Protocols\SSL 3.0\Server";
        private const string Tls10Client = SchannelRoot + @"\Protocols\TLS 1.0\Client";
        private const string Tls10Server = SchannelRoot + @"\Protocols\TLS 1.0\Server";
        private const string Tls11Client = SchannelRoot + @"\Protocols\TLS 1.1\Client";
        private const string Tls11Server = SchannelRoot + @"\Protocols\TLS 1.1\Server";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tls-disable-ssl20",
                Label = "Disable SSL 2.0 (Client + Server)",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["ssl", "ssl2", "tls", "schannel", "security", "protocol"],
                Description =
                    "Disables SSL 2.0 on both client and server sides. SSL 2.0 is considered "
                    + "completely broken and has been deprecated since RFC 6176 (2011). "
                    + "Sets Enabled=0 and DisabledByDefault=1 in SCHANNEL\\Protocols\\SSL 2.0.",
                ApplyOps =
                [
                    RegOp.SetDword(Ssl20Client, "Enabled", 0),
                    RegOp.SetDword(Ssl20Client, "DisabledByDefault", 1),
                    RegOp.SetDword(Ssl20Server, "Enabled", 0),
                    RegOp.SetDword(Ssl20Server, "DisabledByDefault", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(Ssl20Client, "Enabled"),
                    RegOp.DeleteValue(Ssl20Client, "DisabledByDefault"),
                    RegOp.DeleteValue(Ssl20Server, "Enabled"),
                    RegOp.DeleteValue(Ssl20Server, "DisabledByDefault"),
                ],
                DetectOps = [RegOp.CheckDword(Ssl20Client, "Enabled", 0), RegOp.CheckDword(Ssl20Server, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-disable-ssl30",
                Label = "Disable SSL 3.0 (Client + Server)",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["ssl", "ssl3", "poodle", "tls", "schannel", "security", "protocol"],
                Description =
                    "Disables SSL 3.0 on both client and server sides. SSL 3.0 is vulnerable to "
                    + "POODLE (CVE-2014-3566) and other attacks. Deprecated by RFC 7568 (2015). "
                    + "Sets Enabled=0 and DisabledByDefault=1 in SCHANNEL\\Protocols\\SSL 3.0.",
                ApplyOps =
                [
                    RegOp.SetDword(Ssl30Client, "Enabled", 0),
                    RegOp.SetDword(Ssl30Client, "DisabledByDefault", 1),
                    RegOp.SetDword(Ssl30Server, "Enabled", 0),
                    RegOp.SetDword(Ssl30Server, "DisabledByDefault", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(Ssl30Client, "Enabled"),
                    RegOp.DeleteValue(Ssl30Client, "DisabledByDefault"),
                    RegOp.DeleteValue(Ssl30Server, "Enabled"),
                    RegOp.DeleteValue(Ssl30Server, "DisabledByDefault"),
                ],
                DetectOps = [RegOp.CheckDword(Ssl30Client, "Enabled", 0), RegOp.CheckDword(Ssl30Server, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-disable-tls10",
                Label = "Disable TLS 1.0 (Client + Server)",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["tls10", "tls", "schannel", "security", "protocol", "beast"],
                Description =
                    "Disables TLS 1.0 on both client and server sides. TLS 1.0 is vulnerable "
                    + "to BEAST (CVE-2011-3389) and POODLE-TLS attacks. Deprecated in RFC 8996 (2021). "
                    + "WARNING: may break legacy apps that do not support TLS 1.2+.",
                ApplyOps =
                [
                    RegOp.SetDword(Tls10Client, "Enabled", 0),
                    RegOp.SetDword(Tls10Client, "DisabledByDefault", 1),
                    RegOp.SetDword(Tls10Server, "Enabled", 0),
                    RegOp.SetDword(Tls10Server, "DisabledByDefault", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(Tls10Client, "Enabled"),
                    RegOp.DeleteValue(Tls10Client, "DisabledByDefault"),
                    RegOp.DeleteValue(Tls10Server, "Enabled"),
                    RegOp.DeleteValue(Tls10Server, "DisabledByDefault"),
                ],
                DetectOps = [RegOp.CheckDword(Tls10Client, "Enabled", 0), RegOp.CheckDword(Tls10Server, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-disable-tls11",
                Label = "Disable TLS 1.1 (Client + Server)",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["tls11", "tls", "schannel", "security", "protocol"],
                Description =
                    "Disables TLS 1.1 on both client and server sides. TLS 1.1 is deprecated "
                    + "per RFC 8996 (2021); replaced by TLS 1.2 and TLS 1.3. "
                    + "WARNING: may break compatibility with older servers. Apply after confirming "
                    + "all services support TLS 1.2 or higher.",
                ApplyOps =
                [
                    RegOp.SetDword(Tls11Client, "Enabled", 0),
                    RegOp.SetDword(Tls11Client, "DisabledByDefault", 1),
                    RegOp.SetDword(Tls11Server, "Enabled", 0),
                    RegOp.SetDword(Tls11Server, "DisabledByDefault", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(Tls11Client, "Enabled"),
                    RegOp.DeleteValue(Tls11Client, "DisabledByDefault"),
                    RegOp.DeleteValue(Tls11Server, "Enabled"),
                    RegOp.DeleteValue(Tls11Server, "DisabledByDefault"),
                ],
                DetectOps = [RegOp.CheckDword(Tls11Client, "Enabled", 0), RegOp.CheckDword(Tls11Server, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-enable-schannel-event-logging",
                Label = "Enable Detailed SCHANNEL Event Logging",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["schannel", "tls", "logging", "security", "audit"],
                Description =
                    "Enables verbose SCHANNEL event logging (EventLogging=7) which logs all TLS "
                    + "handshake errors, certificate failures, and protocol negotiation details "
                    + "to the System event log. Essential for diagnosing TLS configuration issues.",
                ApplyOps = [RegOp.SetDword(SchannelRoot, "EventLogging", 7)],
                RemoveOps = [RegOp.DeleteValue(SchannelRoot, "EventLogging")],
                DetectOps = [RegOp.CheckDword(SchannelRoot, "EventLogging", 7)],
            },
            new TweakDef
            {
                Id = "tls-disable-weak-rc4-cipher",
                Label = "Disable RC4 Cipher Suite",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["rc4", "cipher", "tls", "schannel", "security"],
                Description =
                    "Disables the RC4 stream cipher in SCHANNEL. RC4 is cryptographically broken "
                    + "(Bar-mitzvah attack, BEAST) and must not be used in any TLS negotiation. "
                    + "Sets Enabled=0 in SCHANNEL\\Ciphers\\RC4 128/128.",
                ApplyOps =
                [
                    RegOp.SetDword(SchannelRoot + @"\Ciphers\RC4 128/128", "Enabled", 0),
                    RegOp.SetDword(SchannelRoot + @"\Ciphers\RC4 64/128", "Enabled", 0),
                    RegOp.SetDword(SchannelRoot + @"\Ciphers\RC4 40/128", "Enabled", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(SchannelRoot + @"\Ciphers\RC4 128/128", "Enabled"),
                    RegOp.DeleteValue(SchannelRoot + @"\Ciphers\RC4 64/128", "Enabled"),
                    RegOp.DeleteValue(SchannelRoot + @"\Ciphers\RC4 40/128", "Enabled"),
                ],
                DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Ciphers\RC4 128/128", "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-disable-des-cipher",
                Label = "Disable DES / Triple-DES Cipher Suites",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["des", "3des", "cipher", "tls", "schannel", "security"],
                Description =
                    "Disables DES (56-bit) and Triple-DES (168-bit) cipher suites in SCHANNEL. "
                    + "DES is trivially brute-forced; 3DES is vulnerable to SWEET32 (CVE-2016-2183). "
                    + "Forces SCHANNEL to negotiate AES-128/256 cipher suites only.",
                ApplyOps =
                [
                    RegOp.SetDword(SchannelRoot + @"\Ciphers\DES 56/56", "Enabled", 0),
                    RegOp.SetDword(SchannelRoot + @"\Ciphers\Triple DES 168", "Enabled", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(SchannelRoot + @"\Ciphers\DES 56/56", "Enabled"),
                    RegOp.DeleteValue(SchannelRoot + @"\Ciphers\Triple DES 168", "Enabled"),
                ],
                DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Ciphers\DES 56/56", "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-disable-null-cipher",
                Label = "Disable NULL Cipher Suite",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["null cipher", "tls", "schannel", "security", "encryption"],
                Description =
                    "Disables the NULL cipher suite which provides authentication with no encryption. "
                    + "A NULL cipher connection is completely readable by any network observer. "
                    + "Sets Enabled=0 in SCHANNEL\\Ciphers\\NULL.",
                ApplyOps = [RegOp.SetDword(SchannelRoot + @"\Ciphers\NULL", "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(SchannelRoot + @"\Ciphers\NULL", "Enabled")],
                DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Ciphers\NULL", "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-disable-md5-hash",
                Label = "Disable MD5 Hash Algorithm in SCHANNEL",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["md5", "hash", "tls", "schannel", "security"],
                Description =
                    "Disables MD5 as a hash algorithm for TLS MAC and certificate signatures in "
                    + "SCHANNEL. MD5 is collision-vulnerable (Flame malware used an MD5 collision). "
                    + "Sets Enabled=0 in SCHANNEL\\Hashes\\MD5.",
                ApplyOps = [RegOp.SetDword(SchannelRoot + @"\Hashes\MD5", "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(SchannelRoot + @"\Hashes\MD5", "Enabled")],
                DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Hashes\MD5", "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "tls-enable-tls12-explicitly",
                Label = "Explicitly Enable TLS 1.2 (Client + Server)",
                Category = "Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["tls12", "tls", "schannel", "security", "protocol"],
                Description =
                    "Explicitly enables TLS 1.2 on both client and server sides (Enabled=1, "
                    + "DisabledByDefault=0). While TLS 1.2 is enabled by default in modern Windows, "
                    + "this hardens against GPO or registry changes that might inadvertently disable it. "
                    + "Apply after disabling TLS 1.0/1.1 to confirm TLS 1.2 is the minimum floor.",
                ApplyOps =
                [
                    RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Client", "Enabled", 1),
                    RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Client", "DisabledByDefault", 0),
                    RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Server", "Enabled", 1),
                    RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Server", "DisabledByDefault", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Client", "Enabled"),
                    RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Client", "DisabledByDefault"),
                    RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Server", "Enabled"),
                    RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Server", "DisabledByDefault"),
                ],
                DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Protocols\TLS 1.2\Client", "Enabled", 1)],
            },
        ];

    }

    // ── UefiLockPolicy ──
    private static class _UefiLockPolicy
    {
        private const string UefiPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI";

        private const string SecureBootStateKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "uefilck-require-secure-boot-active",
                    Label = "UEFI Lock: Require UEFI Secure Boot to Be Active (OS Enforcement Check)",
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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

    // ── VbsEnforcementPolicy ──
    private static class _VbsEnforcementPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "vbsenf-enable-vbs",
                    Label = "Enable Virtualization-Based Security (VBS)",
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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
                    Category = "Encryption",
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

}
