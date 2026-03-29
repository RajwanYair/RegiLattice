// RegiLattice.Core — Tweaks/BitLockerNetworkUnlockPolicy.cs
// BitLocker Network Unlock, pre-boot authentication, recovery, PIN complexity, and BitLocker Drive Encryption policy — Sprint 528.
// Category: "BitLocker Drive Encryption Policy" | Slug: blnetun
// Registry: HKLM\SOFTWARE\Policies\Microsoft\FVE

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class BitLockerNetworkUnlockPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string OsKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\OSVolume";
    private const string NuKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\NetworkUnlock";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "blnetun-require-tpm-plus-pin",
            Label        = "Require TPM + PIN Pre-Boot Authentication for BitLocker OS Volumes",
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
            Category     = "BitLocker Drive Encryption Policy",
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
