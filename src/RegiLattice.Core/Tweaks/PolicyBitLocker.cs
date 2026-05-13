namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyBitLocker
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string OsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\OSVolume";
    private const string FdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\FDVDenyWriteAccess";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fve-require-device-encryption",
            Label = "Require BitLocker Device Encryption",
            Category = "Encryption — Personal Data Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces BitLocker device encryption on OS volumes via Group Policy. Devices that are not encrypted will be flagged non-compliant by MDM/Intune.",
            Tags = ["bitlocker", "encryption", "fve", "policy", "compliance"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Full-disk encryption enforced on OS drive; maximum data protection at rest.",
            ApplyOps = [RegOp.SetDword(Key, "RequireDeviceEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "RequireDeviceEncryption", 1)],
        },
        new TweakDef
        {
            Id = "fve-deny-write-fixed-notprotected",
            Label = "Block Write Access to Unencrypted Fixed Drives",
            Category = "Encryption — Personal Data Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Denies write access to fixed data drives that are not protected by BitLocker. Users can still read unencrypted drives but cannot write to them until encryption is applied.",
            Tags = ["bitlocker", "fixed-drive", "write-protect", "fve", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Unencrypted fixed drives become read-only; forces encryption before use.",
            ApplyOps = [RegOp.SetDword(Key, "FDVDenyWriteAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "FDVDenyWriteAccess")],
            DetectOps = [RegOp.CheckDword(Key, "FDVDenyWriteAccess", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-network-unlock",
            Label = "Disable BitLocker Network Unlock",
            Category = "Encryption — Personal Data Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables BitLocker Network Unlock, which automatically unlocks drives when the computer boots on a trusted corporate network. Eliminates network-based bypass of pre-boot authentication.",
            Tags = ["bitlocker", "network-unlock", "fve", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Network-based auto-unlock disabled; PIN/password required even on trusted networks.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkUnlock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkUnlock")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkUnlock", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-recovery-to-ad",
            Label = "Disable BitLocker Recovery Key Storage in AD",
            Category = "Encryption — Personal Data Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents BitLocker recovery keys from being automatically backed up to Active Directory / Entra ID. Useful when a separate, higher-security key escrow solution is mandated.",
            Tags = ["bitlocker", "recovery-key", "active-directory", "fve", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Recovery keys not stored in AD/Entra; use only when alternate escrow exists.",
            ApplyOps = [RegOp.SetDword(Key, "DoNotBackupToAD", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotBackupToAD")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotBackupToAD", 1)],
        },
        new TweakDef
        {
            Id = "fve-allow-bitlocker-without-tpm",
            Label = "Allow BitLocker Without TPM Chip",
            Category = "Encryption — Personal Data Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows BitLocker to encrypt drives on computers without a TPM chip, using a startup password or USB key instead. Default Windows policy requires TPM; this allows legacy/virtual machines to use BitLocker.",
            Tags = ["bitlocker", "tpm", "no-tpm", "startup-key", "fve", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Non-TPM machines can use BitLocker with password/USB key; useful for VMs.",
            ApplyOps = [RegOp.SetDword(Key, "EnableNonTPM", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNonTPM")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNonTPM", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-used-space-only",
            Label = "Enforce Full Drive Encryption (Not Used-Space Only)",
            Category = "Encryption — Personal Data Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces BitLocker to encrypt the entire drive including free space, not just used space. Prevents forensic recovery of previously deleted files from unencrypted free space sectors.",
            Tags = ["bitlocker", "full-encryption", "used-space", "fve", "policy", "forensics"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All sectors encrypted including free space; forensic recovery of deleted files blocked.",
            ApplyOps = [RegOp.SetDword(Key, "OSAllowedHardwareEncryptionAlgorithms", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "OSAllowedHardwareEncryptionAlgorithms")],
            DetectOps = [RegOp.CheckDword(Key, "OSAllowedHardwareEncryptionAlgorithms", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 643 — PolicyWindowsInk (Windows Ink Workspace Group Policy)
