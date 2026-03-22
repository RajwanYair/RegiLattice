// RegiLattice.Core — Tweaks/BitLockerRemovable.cs
// BitLocker To Go (removable drive encryption) policy tweaks (Sprint 106).
// Slug: "btogo" — configures how removable data volumes are protected with BitLocker.
// Distinct from BitLockerAdvanced.cs which covers TPM-auth (UseTPM*), fixed drives (FDV*), and OS drives (OSD*).
// All values use the RDV* prefix (Removable Data Volumes) in the FVE policy key.
// Registry base: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BitLockerRemovable
{
    private const string Fve = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "btogo-deny-write-unencrypted",
            Label = "BitLocker To Go: Deny Write Access to Unencrypted Removable Drives",
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
            Category = "BitLocker To Go",
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
