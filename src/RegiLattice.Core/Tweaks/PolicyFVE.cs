namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyFVE
{
    private const string FveKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fve-require-startup-pin",
            Label = "Require BitLocker Startup PIN on OS Drive",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseAdvancedStartup=1 and UseTPMPIN=1 in FVE policy. Requires a pre-boot "
                + "PIN in addition to the TPM for BitLocker OS drive encryption. Provides "
                + "defence against cold-boot and DMA attacks on sleeping/hibernating machines.",
            Tags = ["bitlocker", "fve", "pin", "tpm", "policy", "security"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "User must enter a PIN at every boot; remote/unattended reboots may fail.",
            ApplyOps =
            [
                RegOp.SetDword(FveKey, "UseAdvancedStartup", 1),
                RegOp.SetDword(FveKey, "UseTPMPIN", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(FveKey, "UseAdvancedStartup"),
                RegOp.DeleteValue(FveKey, "UseTPMPIN"),
            ],
            DetectOps = [RegOp.CheckDword(FveKey, "UseAdvancedStartup", 1)],
        },
        new TweakDef
        {
            Id = "fve-encrypt-full-disk",
            Label = "Enforce Full Disk Encryption (Not Used Space Only)",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EncryptionType=1 in FVE policy. Forces BitLocker to encrypt the entire "
                + "drive rather than only used space. Prevents data recovery from previously "
                + "deleted files in unencrypted free space sectors.",
            Tags = ["bitlocker", "fve", "full-disk", "encryption", "policy", "security"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Initial encryption takes longer but covers all sectors including free space.",
            ApplyOps = [RegOp.SetDword(FveKey, "OSEncryptionType", 1)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "OSEncryptionType")],
            DetectOps = [RegOp.CheckDword(FveKey, "OSEncryptionType", 1)],
        },
        new TweakDef
        {
            Id = "fve-enforce-xts-aes-256",
            Label = "Enforce XTS-AES 256-bit for OS Drives",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EncryptionMethodWithXtsOs=7 in FVE policy. Forces XTS-AES 256-bit "
                + "encryption for the operating system drive. XTS-AES 256 provides the strongest "
                + "available encryption for fixed drives.",
            Tags = ["bitlocker", "fve", "xts-aes", "256bit", "encryption", "policy"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Strongest encryption algorithm selected; negligible performance impact on modern CPUs.",
            ApplyOps = [RegOp.SetDword(FveKey, "EncryptionMethodWithXtsOs", 7)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "EncryptionMethodWithXtsOs")],
            DetectOps = [RegOp.CheckDword(FveKey, "EncryptionMethodWithXtsOs", 7)],
        },
        new TweakDef
        {
            Id = "fve-enforce-xts-aes-256-fixed",
            Label = "Enforce XTS-AES 256-bit for Fixed Data Drives",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EncryptionMethodWithXtsFdv=7 in FVE policy. Forces XTS-AES 256-bit "
                + "encryption for fixed (non-OS) data drives. Ensures all internal data drives "
                + "use the strongest available BitLocker cipher.",
            Tags = ["bitlocker", "fve", "xts-aes", "256bit", "fixed-drive", "policy"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "All fixed data drives encrypted with XTS-AES 256-bit.",
            ApplyOps = [RegOp.SetDword(FveKey, "EncryptionMethodWithXtsFdv", 7)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "EncryptionMethodWithXtsFdv")],
            DetectOps = [RegOp.CheckDword(FveKey, "EncryptionMethodWithXtsFdv", 7)],
        },
        new TweakDef
        {
            Id = "fve-enforce-aes-cbc-256-removable",
            Label = "Enforce AES-CBC 256-bit for Removable Drives",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EncryptionMethodWithXtsRdv=4 in FVE policy. Forces AES-CBC 256-bit "
                + "encryption for removable drives. AES-CBC (not XTS) is used for removable "
                + "drives to maintain compatibility with older Windows versions.",
            Tags = ["bitlocker", "fve", "aes-cbc", "256bit", "removable", "policy"],
            RegistryKeys = [FveKey],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Removable drives use AES-CBC 256; compatible with Windows 7+ readers.",
            ApplyOps = [RegOp.SetDword(FveKey, "EncryptionMethodWithXtsRdv", 4)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "EncryptionMethodWithXtsRdv")],
            DetectOps = [RegOp.CheckDword(FveKey, "EncryptionMethodWithXtsRdv", 4)],
        },
        new TweakDef
        {
            Id = "fve-require-recovery-password",
            Label = "Require BitLocker Recovery Password",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets OSRecoveryPassword=1 in FVE policy. Requires a numeric recovery password "
                + "to be created when BitLocker is enabled on the OS drive. Ensures a recovery "
                + "path exists if the TPM/PIN is lost.",
            Tags = ["bitlocker", "fve", "recovery", "password", "policy"],
            RegistryKeys = [FveKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Recovery password mandatory; securely store or escrow to Active Directory.",
            ApplyOps = [RegOp.SetDword(FveKey, "OSRecoveryPassword", 1)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "OSRecoveryPassword")],
            DetectOps = [RegOp.CheckDword(FveKey, "OSRecoveryPassword", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-standard-user-encryption",
            Label = "Block Standard Users from Enabling BitLocker",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowStandardUserEncryption=0 in FVE policy. Prevents non-administrator "
                + "users from enabling BitLocker on their drives. Ensures that only IT admins "
                + "can initiate drive encryption, maintaining central key management.",
            Tags = ["bitlocker", "fve", "standard-user", "lockdown", "policy"],
            RegistryKeys = [FveKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Only administrators can enable BitLocker; users cannot self-encrypt.",
            ApplyOps = [RegOp.SetDword(FveKey, "AllowStandardUserEncryption", 0)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "AllowStandardUserEncryption")],
            DetectOps = [RegOp.CheckDword(FveKey, "AllowStandardUserEncryption", 0)],
        },
        new TweakDef
        {
            Id = "fve-require-ad-backup-before-enable",
            Label = "Require AD Backup Before BitLocker Enable",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RequireActiveDirectoryBackup=1 and ActiveDirectoryBackup=1 in FVE policy. "
                + "Prevents BitLocker from being enabled until the recovery key is successfully "
                + "escrowed to Active Directory Domain Services.",
            Tags = ["bitlocker", "fve", "active-directory", "backup", "escrow", "policy"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "BitLocker blocked until recovery key is escrowed to AD; ensures recoverability.",
            ApplyOps =
            [
                RegOp.SetDword(FveKey, "RequireActiveDirectoryBackup", 1),
                RegOp.SetDword(FveKey, "ActiveDirectoryBackup", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(FveKey, "RequireActiveDirectoryBackup"),
                RegOp.DeleteValue(FveKey, "ActiveDirectoryBackup"),
            ],
            DetectOps = [RegOp.CheckDword(FveKey, "RequireActiveDirectoryBackup", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-removable-write-without-bl",
            Label = "Deny Write to Unencrypted Removable Drives",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RDVDenyWriteAccess=1 in FVE policy. Prevents writing data to removable "
                + "drives that are not BitLocker-encrypted. Forces all removable media to be "
                + "encrypted before data can be copied, preventing data leakage via USB.",
            Tags = ["bitlocker", "fve", "removable", "write", "deny", "policy", "dlp"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Unencrypted USB drives are read-only; users must encrypt before writing.",
            ApplyOps = [RegOp.SetDword(FveKey, "RDVDenyWriteAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "RDVDenyWriteAccess")],
            DetectOps = [RegOp.CheckDword(FveKey, "RDVDenyWriteAccess", 1)],
        },
        new TweakDef
        {
            Id = "fve-require-secure-boot-validation",
            Label = "Require Secure Boot for BitLocker Integrity",
            Category = "BitLocker FVE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets OSAllowSecureBootForIntegrity=1 in FVE policy. Requires Secure Boot "
                + "validation as part of BitLocker's platform integrity check. Ensures that "
                + "only trusted firmware and boot components can unlock the encrypted OS drive.",
            Tags = ["bitlocker", "fve", "secure-boot", "integrity", "policy", "security"],
            RegistryKeys = [FveKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Secure Boot required for BitLocker; machines without Secure Boot cannot use BL.",
            ApplyOps = [RegOp.SetDword(FveKey, "OSAllowSecureBootForIntegrity", 1)],
            RemoveOps = [RegOp.DeleteValue(FveKey, "OSAllowSecureBootForIntegrity")],
            DetectOps = [RegOp.CheckDword(FveKey, "OSAllowSecureBootForIntegrity", 1)],
        },
    ];
}
