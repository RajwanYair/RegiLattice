// RegiLattice.Core — Tweaks/BitLockerAdvanced.cs
// Advanced BitLocker Drive Encryption configuration (Win10 1607+ / Win11).
// Slug: "bitlocker" — complements Encryption.cs without duplicating its IDs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BitLockerAdvanced
{
    private const string FvePol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string FveRec = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string BitlockerSys = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BitLocker";
    private const string TpmPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";
    private const string DmaPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Kernel DMA Protection";
    private const string MdmPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Policies\PassportForWork";
    private const string SmbPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "bitlocker-require-preboot-pin",
            Label = "Require BitLocker Pre-Boot PIN",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "encryption", "preboot", "tpm"],
            Description =
                "Configures BitLocker to require a PIN at pre-boot in addition to the TPM chip. "
                + "This 'TPM+PIN' mode prevents cold-boot and DMA attacks that can bypass TPM-only protection.",
            ApplyOps = [RegOp.SetDword(FvePol, "UseAdvancedStartup", 1), RegOp.SetDword(FvePol, "UseTPMPIN", 1)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "UseAdvancedStartup"), RegOp.DeleteValue(FvePol, "UseTPMPIN")],
            DetectOps = [RegOp.CheckDword(FvePol, "UseTPMPIN", 1)],
        },
        new TweakDef
        {
            Id = "bitlocker-require-preboot-key",
            Label = "Require BitLocker Pre-Boot USB Key",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "encryption", "preboot", "usb"],
            Description =
                "Configures BitLocker to accept a startup key stored on a USB drive alongside the TPM. "
                + "Useful as a hardware token factor for high-security workstations without a numeric keypad.",
            ApplyOps = [RegOp.SetDword(FvePol, "UseAdvancedStartup", 1), RegOp.SetDword(FvePol, "UseTPMKey", 1)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "UseAdvancedStartup"), RegOp.DeleteValue(FvePol, "UseTPMKey")],
            DetectOps = [RegOp.CheckDword(FvePol, "UseTPMKey", 1)],
        },
        new TweakDef
        {
            Id = "bitlocker-force-256bit-encryption",
            Label = "Force BitLocker 256-Bit AES Encryption",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "encryption", "aes", "strength"],
            Description =
                "Configures BitLocker to use AES-256 (without XTS mode, downlevel compat) for new volumes. "
                + "Higher encryption strength at a small CPU overhead; requires BitLocker to be re-encrypted if already active.",
            ApplyOps = [RegOp.SetDword(FvePol, "EncryptionMethodWithXts", 4)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "EncryptionMethodWithXts")],
            DetectOps = [RegOp.CheckDword(FvePol, "EncryptionMethodWithXts", 4)],
        },
        new TweakDef
        {
            Id = "bitlocker-force-xts-aes256",
            Label = "Force BitLocker XTS-AES-256 Encryption",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "encryption", "xts", "aes256"],
            Description =
                "Enforces XTS-AES-256 (the strongest BitLocker cipher, Win10 1511+) for all new OS and fixed data drives. "
                + "XTS mode provides better diffusion and is immune to cipher-text manipulation attacks.",
            ApplyOps = [RegOp.SetDword(FvePol, "EncryptionMethodWithXtsOs", 7), RegOp.SetDword(FvePol, "EncryptionMethodWithXtsFdv", 7)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "EncryptionMethodWithXtsOs"), RegOp.DeleteValue(FvePol, "EncryptionMethodWithXtsFdv")],
            DetectOps = [RegOp.CheckDword(FvePol, "EncryptionMethodWithXtsOs", 7)],
        },
        new TweakDef
        {
            Id = "bitlocker-require-recovery-to-ad",
            Label = "Require BitLocker Recovery Key Backup to AD",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "recovery", "active directory", "enterprise"],
            Description =
                "Prevents enabling BitLocker unless the recovery key has been escrowed to Active Directory. "
                + "Ensures enterprise IT always has recovery access without requiring USB key distribution.",
            ApplyOps = [RegOp.SetDword(FvePol, "RequireActiveDirectoryBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "RequireActiveDirectoryBackup")],
            DetectOps = [RegOp.CheckDword(FvePol, "RequireActiveDirectoryBackup", 1)],
        },
        new TweakDef
        {
            Id = "bitlocker-disable-recovery-password-user",
            Label = "Prevent Users from Generating BitLocker Recovery Passwords",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "recovery", "policy", "enterprise"],
            Description =
                "Prevents standard users from generating or printing BitLocker recovery passwords. "
                + "Recovery must be managed through central IT tools (AD, Intune), reducing the risk of key exfiltration.",
            ApplyOps = [RegOp.SetDword(FvePol, "OsRecoveryPassword", 0)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "OsRecoveryPassword")],
            DetectOps = [RegOp.CheckDword(FvePol, "OsRecoveryPassword", 0)],
        },
        new TweakDef
        {
            Id = "bitlocker-disable-recovery-key-user",
            Label = "Prevent Users from Saving BitLocker Recovery Keys to USB",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "recovery", "usb", "policy"],
            Description =
                "Blocks users from saving BitLocker recovery keys to a USB drive. "
                + "Keys must be saved to AD, Azure AD, or a managed location to prevent loss or misuse.",
            ApplyOps = [RegOp.SetDword(FvePol, "OsRecoveryKey", 0)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "OsRecoveryKey")],
            DetectOps = [RegOp.CheckDword(FvePol, "OsRecoveryKey", 0)],
        },
        new TweakDef
        {
            Id = "bitlocker-enable-dma-protection",
            Label = "Enable Kernel DMA Protection Override",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "dma", "thunderbolt", "kernel"],
            Description =
                "Enables Kernel DMA Protection policy to block external DMA-capable devices (Thunderbolt) "
                + "from accessing system memory until a user is logged in. Mitigates DMA cold-boot attacks against BitLocker.",
            ApplyOps = [RegOp.SetDword(DmaPol, "DeviceEnumerationPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(DmaPol, "DeviceEnumerationPolicy")],
            DetectOps = [RegOp.CheckDword(DmaPol, "DeviceEnumerationPolicy", 0)],
        },
        new TweakDef
        {
            Id = "bitlocker-disable-used-space-only",
            Label = "Disable 'Used Space Only' BitLocker Encryption",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "encryption", "compliance", "forensics"],
            Description =
                "Requires BitLocker to encrypt the entire drive rather than only used space. "
                + "Full encryption prevents forensic tools from recovering deleted files on drives that haven't used used-space-only mode.",
            ApplyOps = [RegOp.SetDword(FvePol, "OSEncryptionType", 2)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "OSEncryptionType")],
            DetectOps = [RegOp.CheckDword(FvePol, "OSEncryptionType", 2)],
        },
        new TweakDef
        {
            Id = "bitlocker-enable-enhanced-pin",
            Label = "Enable Enhanced BitLocker PIN (Alphanumeric)",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "pin", "authentication"],
            Description =
                "Allows users to set an alphanumeric PIN for BitLocker pre-boot authentication, "
                + "increasing PIN entropy compared to numeric-only PINs. Requires BIOS/UEFI keyboard support.",
            ApplyOps = [RegOp.SetDword(FvePol, "UseEnhancedPin", 1)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "UseEnhancedPin")],
            DetectOps = [RegOp.CheckDword(FvePol, "UseEnhancedPin", 1)],
        },
        new TweakDef
        {
            Id = "bitlocker-set-min-pin-length",
            Label = "Set BitLocker Minimum PIN Length to 8",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "pin", "length", "policy"],
            Description =
                "Enforces a minimum BitLocker startup PIN length of 8 characters. "
                + "Longer PINs significantly increase brute-force resistance before the TPM anti-hammering lockout activates.",
            ApplyOps = [RegOp.SetDword(FvePol, "MinimumPIN", 8)],
            RemoveOps = [RegOp.DeleteValue(FvePol, "MinimumPIN")],
            DetectOps = [RegOp.CheckDword(FvePol, "MinimumPIN", 8)],
        },
        new TweakDef
        {
            Id = "bitlocker-disable-sleep-resumption",
            Label = "Disable Sleep Mode to Prevent BitLocker Bypass",
            Category = "BitLocker Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["bitlocker", "security", "sleep", "power", "hibernation"],
            Description =
                "Disables system sleep (S3) and allows only hibernate (S4) on BitLocker-enabled systems. "
                + "Prevents attacks where memory-resident encryption keys could be extracted from a sleeping system.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\abfc2519-3608-4c2a-94ea-171b0ed546ab",
                    "ACSettingIndex",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\abfc2519-3608-4c2a-94ea-171b0ed546ab",
                    "DCSettingIndex",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\abfc2519-3608-4c2a-94ea-171b0ed546ab",
                    "ACSettingIndex"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\abfc2519-3608-4c2a-94ea-171b0ed546ab",
                    "DCSettingIndex"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\abfc2519-3608-4c2a-94ea-171b0ed546ab",
                    "ACSettingIndex",
                    0
                ),
            ],
        },
    ];
}
