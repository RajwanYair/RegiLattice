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
        new TweakDef
        {
            Id = "enc-disable-null-cipher",
            Label = "Disable NULL Cipher Suite",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the NULL cipher suite to prevent unencrypted TLS connections. Default: may be enabled.",
            Tags = ["cipher", "null", "tls", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-rc4",
            Label = "Disable RC4 Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the weak RC4 cipher across all key lengths. Protects against RC4-based attacks. Default: may be enabled.",
            Tags = ["cipher", "rc4", "security", "encryption"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128",
            ],
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
            Id = "enc-disable-des",
            Label = "Disable DES Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the weak DES cipher. DES is considered insecure since the late 1990s. Default: may be enabled.",
            Tags = ["cipher", "des", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-enable-tls13",
            Label = "Enable TLS 1.3",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 20170,
            Description = "Explicitly enables TLS 1.3 support. TLS 1.3 provides faster handshakes and stronger security. Requires Windows 10 20H1+. Default: enabled on supported builds.",
            Tags = ["tls", "encryption", "security", "protocol", "tls13"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client", "Enabled", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client", "DisabledByDefault", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server", "Enabled", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server", "DisabledByDefault", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client", "DisabledByDefault"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server", "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client", "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "enc-disable-3des",
            Label = "Disable Triple DES (3DES) Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the weak Triple DES cipher. Protects against Sweet32 birthday attack. Default: enabled.",
            Tags = ["cipher", "3des", "security", "encryption", "sweet32"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-rc4-128",
            Label = "Disable RC4 128-bit Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the RC4 128-bit cipher suite. RC4 is considered broken and vulnerable to multiple attacks. Default: enabled.",
            Tags = ["cipher", "rc4", "security", "encryption", "weak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-rc4-56",
            Label = "Disable RC4 56-bit Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the RC4 56-bit cipher suite. Very weak cipher with known vulnerabilities. Default: enabled.",
            Tags = ["cipher", "rc4", "security", "encryption", "weak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-rc4-40",
            Label = "Disable RC4 40-bit Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the RC4 40-bit export cipher. Extremely weak, trivially breakable. Default: enabled.",
            Tags = ["cipher", "rc4", "security", "encryption", "export"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-des-56",
            Label = "Disable DES 56-bit Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the weak DES 56-bit cipher. Easily brute-forced by modern hardware. Default: enabled.",
            Tags = ["cipher", "des", "security", "encryption", "weak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-null-encryption",
            Label = "Disable NULL Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the NULL cipher which provides no encryption at all. Should always be disabled. Default: disabled on modern systems.",
            Tags = ["cipher", "null", "security", "encryption", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-disable-md5-hash",
            Label = "Disable MD5 Hash Algorithm",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the MD5 hash algorithm in SCHANNEL. MD5 is collision-prone and should not be used for TLS. Default: enabled.",
            Tags = ["hash", "md5", "security", "encryption", "weak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "enc-enable-sha256-hash",
            Label = "Ensure SHA-256 Hash Enabled",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Explicitly enables the SHA-256 hash algorithm in SCHANNEL. SHA-256 is the recommended hash for modern TLS connections. Default: enabled.",
            Tags = ["hash", "sha256", "security", "encryption", "tls"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256", "Enabled", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256", "Enabled", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "enc-enable-sha384-hash",
            Label = "Ensure SHA-384 Hash Enabled",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Explicitly enables the SHA-384 hash algorithm in SCHANNEL. Used by TLS 1.3 cipher suites. Default: enabled.",
            Tags = ["hash", "sha384", "security", "encryption", "tls"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384", "Enabled", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384", "Enabled", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "enc-enable-diffie-hellman",
            Label = "Ensure Diffie-Hellman Key Exchange Enabled",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Explicitly enables the Diffie-Hellman key exchange algorithm in SCHANNEL. Required for DHE cipher suites. Default: enabled.",
            Tags = ["key-exchange", "diffie-hellman", "security", "encryption", "tls"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman", "Enabled", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman", "Enabled", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "enc-set-dh-min-key-2048",
            Label = "Set Min Diffie-Hellman Key to 2048-bit",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the minimum Diffie-Hellman key size to 2048 bits. Prevents use of weak 1024-bit keys that are vulnerable to Logjam attack. Default: 1024.",
            Tags = ["key-exchange", "diffie-hellman", "security", "logjam", "key-size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman", "ServerMinKeyBitLength", 2048)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman", "ServerMinKeyBitLength")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman", "ServerMinKeyBitLength", 2048)],
        },
    ];
}
