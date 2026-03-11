namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Encryption
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "enc-bitlocker-require-startup-pin",
            Label = "BitLocker: Require Startup PIN",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Requires a PIN at boot for BitLocker-encrypted OS drives. Adds pre-boot authentication layer. Default: not required.",
            Tags = ["bitlocker", "encryption", "pin", "security", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "UseAdvancedStartup", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EnableBDEWithNoTPM", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "UseTPMPIN", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "UseAdvancedStartup"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EnableBDEWithNoTPM"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "UseTPMPIN"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "UseAdvancedStartup", 1)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-aes256",
            Label = "BitLocker: Force AES-256 Encryption",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces BitLocker to use AES-256 (XTS mode) for OS drives instead of default AES-128. Stronger encryption at minimal cost. Default: AES-128.",
            Tags = ["bitlocker", "encryption", "aes256", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsOs", 7),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsFdv", 7),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsRdv", 4),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsOs"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsFdv"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsRdv"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "EncryptionMethodWithXtsOs", 7)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-full-disk",
            Label = "BitLocker: Encrypt Entire Drive",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces BitLocker to encrypt the entire drive (not just used space). More secure for drives with previously deleted data. Default: used space only.",
            Tags = ["bitlocker", "encryption", "full-disk", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "OSEncryptionType", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "OSEncryptionType"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "OSEncryptionType", 1)],
        },
        new TweakDef
        {
            Id = "enc-disable-efs",
            Label = "Disable Encrypting File System (EFS)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Encrypting File System (EFS) feature. Prevents users from encrypting individual files. Use BitLocker for full-disk encryption instead. Default: enabled.",
            Tags = ["efs", "encryption", "disable", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 1)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-disable-standby-pin",
            Label = "BitLocker: Require PIN After Standby",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic BitLocker unlock after standby/sleep, requiring PIN re-entry. Protects against cold-boot attacks. Default: auto-unlock.",
            Tags = ["bitlocker", "encryption", "standby", "pin", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "DisableStandbyUnlock", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "DisableStandbyUnlock"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "DisableStandbyUnlock", 1)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-backup-to-ad",
            Label = "BitLocker: Backup Recovery Key to AD",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires BitLocker recovery keys to be backed up to Active Directory before encryption. Ensures key recovery in enterprise. Default: not required.",
            Tags = ["bitlocker", "encryption", "recovery", "active-directory", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RequireActiveDirectoryBackup", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "ActiveDirectoryBackup", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RequireActiveDirectoryBackup"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "ActiveDirectoryBackup"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RequireActiveDirectoryBackup", 1)],
        },
        new TweakDef
        {
            Id = "enc-tls12-only",
            Label = "Enforce TLS 1.2 Minimum",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables TLS 1.0 and TLS 1.1, enforcing TLS 1.2 as minimum. Strengthens transport encryption. Default: TLS 1.0+ allowed.",
            Tags = ["tls", "encryption", "security", "protocol", "network"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "DisabledByDefault", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "DisabledByDefault", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "DisabledByDefault"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-ssl3",
            Label = "Disable SSL 3.0",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the insecure SSL 3.0 protocol. Protects against POODLE and other SSL3 attacks. Default: may be enabled.",
            Tags = ["ssl", "encryption", "security", "protocol", "poodle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "DisabledByDefault", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server", "DisabledByDefault", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "DisabledByDefault"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server", "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-rc4-cipher",
            Label = "Disable RC4 Cipher Suites",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the weak RC4 cipher. Prevents downgrade attacks using broken RC4 encryption. Default: may be enabled for compatibility.",
            Tags = ["rc4", "cipher", "encryption", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-removable-require",
            Label = "BitLocker: Require Encryption on Removable Drives",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Denies write access to removable drives not protected by BitLocker. Prevents data leakage via unencrypted USB drives. Default: not required.",
            Tags = ["bitlocker", "encryption", "removable", "usb", "security", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RDVDenyWriteAccess", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RDVDenyWriteAccess"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RDVDenyWriteAccess", 1)],
        },
    ];
}
