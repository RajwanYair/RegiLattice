#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class EfsEncryptionPolicy
{
    private const string Efs    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\EFS";
    private const string EfsAdv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnhancedStorageDevices";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "efspol-disable-efs",
            Label = "Disable EFS (Encrypting File System)",
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
            Category = "EFS Encryption Policy",
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
