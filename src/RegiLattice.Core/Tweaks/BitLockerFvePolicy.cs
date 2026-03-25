#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 258 — BitLocker FVE Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\FVE  (Full Volume Encryption / BitLocker Group Policy)
//       HKLM\SOFTWARE\Policies\Microsoft\FVE\OSVolume
//       HKLM\SOFTWARE\Policies\Microsoft\FVE\RemovableDrives
// Complements BitLockerAdvanced.cs (SYSTEM path) and Encryption.cs (activation tweaks)
// All IDs use slug "blfve-" to avoid collision with bitlocker-* and enc-* slugs.
internal static class BitLockerFvePolicy
{
    private const string FveKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string FveOs   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\OSVolume";
    private const string FveRem  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\RemovableDrives";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "blfve-disable-recovery-console-dra",
            Label = "Disable BitLocker Recovery via Data Recovery Agent",
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
            Category = "BitLocker FVE Policy",
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
