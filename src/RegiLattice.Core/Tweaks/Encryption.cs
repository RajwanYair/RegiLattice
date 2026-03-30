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
            Description =
                "Forces BitLocker to use AES-256 (XTS mode) for OS drives instead of default AES-128. Stronger encryption at minimal cost. Default: AES-128.",
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
            Description =
                "Forces BitLocker to encrypt the entire drive (not just used space). More secure for drives with previously deleted data. Default: used space only.",
            Tags = ["bitlocker", "encryption", "full-disk", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "OSEncryptionType", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "OSEncryptionType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "OSEncryptionType", 1)],
        },
        new TweakDef
        {
            Id = "enc-disable-efs",
            Label = "Disable Encrypting File System (EFS)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Encrypting File System (EFS) feature. Prevents users from encrypting individual files. Use BitLocker for full-disk encryption instead. Default: enabled.",
            Tags = ["efs", "encryption", "disable", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\EFS", "EfsConfiguration", 1)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-disable-standby-pin",
            Label = "BitLocker: Require PIN After Standby",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic BitLocker unlock after standby/sleep, requiring PIN re-entry. Protects against cold-boot attacks. Default: auto-unlock.",
            Tags = ["bitlocker", "encryption", "standby", "pin", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "DisableStandbyUnlock", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "DisableStandbyUnlock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "DisableStandbyUnlock", 1)],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-backup-to-ad",
            Label = "BitLocker: Backup Recovery Key to AD",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires BitLocker recovery keys to be backed up to Active Directory before encryption. Ensures key recovery in enterprise. Default: not required.",
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
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "DisabledByDefault",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "DisabledByDefault",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "DisabledByDefault"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "Enabled",
                    0
                ),
            ],
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
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client",
                    "Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client",
                    "DisabledByDefault",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server",
                    "Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server",
                    "DisabledByDefault",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client",
                    "Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client",
                    "DisabledByDefault"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server",
                    "Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-rc4-cipher",
            Label = "Disable RC4 Cipher Suites",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the weak RC4 cipher. Prevents downgrade attacks using broken RC4 encryption. Default: may be enabled for compatibility.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-bitlocker-removable-require",
            Label = "BitLocker: Require Encryption on Removable Drives",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Denies write access to removable drives not protected by BitLocker. Prevents data leakage via unencrypted USB drives. Default: not required.",
            Tags = ["bitlocker", "encryption", "removable", "usb", "security", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RDVDenyWriteAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE", "RDVDenyWriteAccess")],
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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0),
            ],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-enable-tls13",
            Label = "Enable TLS 1.3",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 20170,
            Description =
                "Explicitly enables TLS 1.3 support. TLS 1.3 provides faster handshakes and stronger security. Requires Windows 10 20H1+. Default: enabled on supported builds.",
            Tags = ["tls", "encryption", "security", "protocol", "tls13"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                    "Enabled",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                    "DisabledByDefault",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server",
                    "Enabled",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server",
                    "DisabledByDefault",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                    "Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                    "DisabledByDefault"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server",
                    "Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client",
                    "Enabled",
                    1
                ),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168",
                    "Enabled",
                    0
                ),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 128/128", "Enabled", 0),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 56/128", "Enabled", 0),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-null-encryption",
            Label = "Disable NULL Cipher",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the NULL cipher which provides no encryption at all. Should always be disabled. Default: disabled on modern systems.",
            Tags = ["cipher", "null", "security", "encryption", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\NULL", "Enabled", 0),
            ],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-enable-sha256-hash",
            Label = "Ensure SHA-256 Hash Enabled",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Explicitly enables the SHA-256 hash algorithm in SCHANNEL. SHA-256 is the recommended hash for modern TLS connections. Default: enabled.",
            Tags = ["hash", "sha256", "security", "encryption", "tls"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256",
                    "Enabled",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA256",
                    "Enabled",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384",
                    "Enabled",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA384",
                    "Enabled",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-enable-diffie-hellman",
            Label = "Ensure Diffie-Hellman Key Exchange Enabled",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Explicitly enables the Diffie-Hellman key exchange algorithm in SCHANNEL. Required for DHE cipher suites. Default: enabled.",
            Tags = ["key-exchange", "diffie-hellman", "security", "encryption", "tls"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman",
                    "Enabled",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman",
                    "Enabled",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-set-dh-min-key-2048",
            Label = "Set Min Diffie-Hellman Key to 2048-bit",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the minimum Diffie-Hellman key size to 2048 bits. Prevents use of weak 1024-bit keys that are vulnerable to Logjam attack. Default: 1024.",
            Tags = ["key-exchange", "diffie-hellman", "security", "logjam", "key-size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman",
                    "ServerMinKeyBitLength",
                    2048
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman",
                    "ServerMinKeyBitLength"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\KeyExchangeAlgorithms\Diffie-Hellman",
                    "ServerMinKeyBitLength",
                    2048
                ),
            ],
        },
        // ── Sprint 18 — 10 new Encryption tweaks ──────────────────────────
        new TweakDef
        {
            Id = "enc-disable-des-cipher",
            Label = "Disable DES Cipher Suite",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the insecure DES cipher in SChannel. Prevents use of weak 56-bit encryption. Default: enabled.",
            Tags = ["encryption", "des", "cipher", "weak", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\DES 56/56", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-force-strong-key-usage",
            Label = "Enforce Strong Key Usage for SChannel",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires strong cryptographic key usage for all SChannel connections. Default: not enforced.",
            Tags = ["encryption", "schannel", "strong-key", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "ForceStrongKeyProtection", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "ForceStrongKeyProtection"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "ForceStrongKeyProtection", 2),
            ],
        },
        new TweakDef
        {
            Id = "enc-set-session-ticket-lifetime",
            Label = "Set TLS Session Ticket Lifetime to 1 Hour",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits TLS session ticket lifetime to 3600 seconds (1 hour). Enhances forward secrecy. Default: 36000s (10 hours).",
            Tags = ["encryption", "tls", "session-ticket", "forward-secrecy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "ServerCacheTime", 3600)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "ServerCacheTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "ServerCacheTime", 3600),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-export-ciphers",
            Label = "Disable Export-Grade Ciphers (40/56-bit)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables weak export-grade 40-bit and 56-bit RC2/RC4 cipher suites. Prevents FREAK/Logjam-style attacks. Default: enabled.",
            Tags = ["encryption", "export", "cipher", "freak", "logjam", "weak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC4 40/128", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-enable-ocsp-stapling",
            Label = "Enable OCSP Stapling for TLS",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables OCSP stapling to improve TLS certificate revocation checking performance. Default: depends on server.",
            Tags = ["encryption", "ocsp", "tls", "certificate", "revocation"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\OID\EncodingType 0\CertDllCreateCertificateChainEngine\Config",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\OID\EncodingType 0\CertDllCreateCertificateChainEngine\Config",
                    "CryptnetCachedOcspSwitchToCrlCount",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\OID\EncodingType 0\CertDllCreateCertificateChainEngine\Config",
                    "CryptnetCachedOcspSwitchToCrlCount"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\OID\EncodingType 0\CertDllCreateCertificateChainEngine\Config",
                    "CryptnetCachedOcspSwitchToCrlCount",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-md5-signatures",
            Label = "Disable MD5 in TLS Signatures",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables MD5 hash algorithm for TLS signature verification. MD5 is cryptographically broken. Default: may be enabled for compatibility.",
            Tags = ["encryption", "md5", "tls", "hash", "broken", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-enforce-cert-padding",
            Label = "Enforce Certificate Padding Check",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces strict certificate padding verification in WinVerifyTrust. Prevents CVE-2013-3900 binary planting attacks. Default: not enforced.",
            Tags = ["encryption", "certificate", "padding", "winverifytrust", "cve"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config", "EnableCertPaddingCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config", "EnableCertPaddingCheck")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config", "EnableCertPaddingCheck", 1)],
        },
        new TweakDef
        {
            Id = "enc-disable-legacy-renegotiation",
            Label = "Disable Insecure TLS Renegotiation",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables legacy TLS renegotiation to prevent man-in-the-middle prefix injection attacks (CVE-2009-3555). Default: allowed.",
            Tags = ["encryption", "tls", "renegotiation", "mitm", "cve"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "AllowInsecureRenegoClients", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "AllowInsecureRenegoClients"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "AllowInsecureRenegoClients", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-enable-extended-master-secret",
            Label = "Enable TLS Extended Master Secret",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the TLS Extended Master Secret extension (RFC 7627). Strengthens session key derivation. Default: may be disabled.",
            Tags = ["encryption", "tls", "master-secret", "rfc7627", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "DisableExtendedMasterSecret", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "DisableExtendedMasterSecret"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL", "DisableExtendedMasterSecret", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-triple-des-cipher",
            Label = "Disable Triple DES (3DES) Cipher Suite",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the 3DES/Triple DES cipher suite. 3DES is vulnerable to SWEET32 birthday attacks. Default: enabled for compatibility.",
            Tags = ["encryption", "3des", "cipher", "sweet32", "weak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\Triple DES 168",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-tls10-client",
            Label = "Disable TLS 1.0 for clients (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls10", "schannel", "weak"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-tls10-server",
            Label = "Disable TLS 1.0 for servers (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls10", "schannel", "server", "weak"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-tls11-client",
            Label = "Disable TLS 1.1 for clients (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls11", "schannel", "weak"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-tls11-server",
            Label = "Disable TLS 1.1 for servers (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls11", "schannel", "server", "weak"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Server",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Server",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Server",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-ssl20-client",
            Label = "Disable SSL 2.0 for clients (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "ssl", "ssl20", "schannel", "weak", "legacy"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-ssl20-server",
            Label = "Disable SSL 2.0 for servers (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "ssl", "ssl20", "schannel", "server", "weak"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Server",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Server",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Server",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-tls10-disabled-by-default-client",
            Label = "Mark TLS 1.0 as disabled by default for clients",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls10", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "DisabledByDefault",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                    "DisabledByDefault",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-tls11-disabled-by-default-client",
            Label = "Mark TLS 1.1 as disabled by default for clients",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls11", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "DisabledByDefault",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
                    "DisabledByDefault",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-rc2-cipher",
            Label = "Disable RC2 128/128 cipher (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "rc2", "cipher", "weak", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 128/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 128/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 128/128", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-rc2-56-cipher",
            Label = "Disable RC2 56/128 cipher (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "rc2", "cipher", "weak", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 56/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 56/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 56/128", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-rc2-40-cipher",
            Label = "Disable RC2 40/128 cipher (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "rc2", "cipher", "weak", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 40/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 40/128", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\RC2 40/128", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-md4-hash",
            Label = "Disable MD4 hash algorithm (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "md4", "hash", "weak", "schannel"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD4", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD4", "Enabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD4", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-md2-hash",
            Label = "Disable MD2 hash algorithm (SCHANNEL)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "md2", "hash", "weak", "schannel"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD2", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD2", "Enabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD2", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "enc-disable-pct10-client",
            Label = "Disable PCT 1.0 legacy protocol for clients",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "pct", "legacy", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\PCT 1.0\Client",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\PCT 1.0\Client",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\PCT 1.0\Client",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-tls12-default-on-client",
            Label = "Ensure TLS 1.2 is not disabled by default for clients",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls12", "schannel"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.2\Client",
                    "DisabledByDefault",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.2\Client",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.2\Client",
                    "DisabledByDefault",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-tls12-default-on-server",
            Label = "Ensure TLS 1.2 is not disabled by default for servers",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls12", "schannel", "server"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.2\Server",
                    "DisabledByDefault",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.2\Server",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.2\Server",
                    "DisabledByDefault",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "enc-tls10-disabled-by-default-server",
            Label = "Mark TLS 1.0 as disabled by default for servers",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["encryption", "tls", "tls10", "schannel", "server"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server",
                    "DisabledByDefault",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server",
                    "DisabledByDefault"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server",
                    "DisabledByDefault",
                    1
                ),
            ],
        },
    ];
}

// ── Merged from BitLockerAdvanced.cs ──────────────────────────────────────────────────

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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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
            Category = "Encryption",
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

// ── Merged from TrustedLaunchPolicy.cs ──────────────────────────────────────────────────

internal static class TrustedLaunchPolicy
{
    private const string UefiPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI";

    private const string HyperVPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Hyper-V";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "trlnch-enable-hyperv-vm-secure-boot",
                Label = "Trusted Launch: Enable Secure Boot for Hyper-V Generation 2 VMs",
                Category = "Encryption",
                Description =
                    "Sets VmSecureBootEnabled=1 in the Hyper-V policy key. Enforces Secure Boot on all Generation 2 Hyper-V virtual machines created or managed by this host. Secure Boot for VMs uses a virtualised Secure Boot DB/DBX to validate the VM's boot chain — the virtual firmware (UEFI), virtual boot manager, and VM guest OS loader must all be signed. This prevents rootkit-class malware from persisting across a VM save/restore or snapshot operation inside a Hyper-V host by ensuring the VM's boot chain is integrity-validated on every boot.",
                Tags = ["hyperv", "vm-secure-boot", "generation-2", "guest-integrity", "uefi-vm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Secure Boot enforced for Hyper-V Gen2 VMs. VMs using unsigned bootloaders (e.g., custom Linux distributions with non-shim bootloaders, custom kernel builds, or legacy BIOS-mode VM configurations) will fail to boot. Gen2 VMs must be configured with the Microsoft UEFI CA or Linux UEFI CA in the virtual Secure Boot DB.",
                ApplyOps = [RegOp.SetDword(HyperVPolicyKey, "VmSecureBootEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(HyperVPolicyKey, "VmSecureBootEnabled")],
                DetectOps = [RegOp.CheckDword(HyperVPolicyKey, "VmSecureBootEnabled", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-hyperv-vtpm-provisioning",
                Label = "Trusted Launch: Enable Hyper-V vTPM 2.0 for All Generation 2 VMs",
                Category = "Encryption",
                Description =
                    "Sets VmVirtualTPMEnabled=1 in the Hyper-V policy key. Enables virtual TPM 2.0 for all Generation 2 Hyper-V VMs. A vTPM provides guest VMs with a virtualised TPM that supports all TPM 2.0 operations — BitLocker encryption keyed to the vTPM, Windows Hello for Business, and TPM-backed account protection in the guest OS. The vTPM is backed by the host's physical TPM via Hyper-V's key storage driver. VMs with vTPM can use BitLocker and attestation services, ensuring that guest VM data is encrypted even if the host storage is compromised.",
                Tags = ["hyperv", "vtpm", "tpm-2.0", "bitlocker-vm", "attestation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "vTPM 2.0 enabled for Hyper-V Gen2 VMs. Requires host physical TPM 2.0. VM configs that do not have vTPM configuration in their XML will need to be updated (Edit-VMSecurity). Guest VM BitLocker encrypted with vTPM is backed by the host TPM — if the host is rebuilt, vTPM keys must be backed up or BitLocker recovery keys must be stored in AD.",
                ApplyOps = [RegOp.SetDword(HyperVPolicyKey, "VmVirtualTPMEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(HyperVPolicyKey, "VmVirtualTPMEnabled")],
                DetectOps = [RegOp.CheckDword(HyperVPolicyKey, "VmVirtualTPMEnabled", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-measured-boot-reporting",
                Label = "Trusted Launch: Enable Measured Boot Attestation Reporting",
                Category = "Encryption",
                Description =
                    "Sets EnableMeasuredBootReporting=1 in the UEFI policy hive. Enables the Windows Health Attestation Service (HAS) to report Measured Boot results — the chain of TPM PCR measurements recorded during each boot. When reporting is enabled, the device regularly submits a Measured Boot report to the Windows Health Attestation Service, which compares the PCR measurements against known-good baselines. Microsoft Intune and Microsoft Endpoint Manager can use these reports to detect anomalous boot states that may indicate a rootkit or modified kernel component.",
                Tags = ["measured-boot", "health-attestation", "tpm-pcr", "intune", "reporting"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Measured Boot PCR data reported to Health Attestation Service. Requires Windows Health Attestation Service connectivity (has.spserv.microsoft.com). Data transmitted includes boot measurement logs and PCR values — no PII. Reports are used for Intune Conditional Access compliance decisions. Reports transmitted on device startup and periodically.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "EnableMeasuredBootReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "EnableMeasuredBootReporting")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "EnableMeasuredBootReporting", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-block-uefi-setupmode",
                Label = "Trusted Launch: Block UEFI Setup Mode (Prevent DB Key Replacement)",
                Category = "Encryption",
                Description =
                    "Sets PreventUEFISetupMode=1 in the UEFI policy hive. Prevents the device from entering UEFI Setup Mode. Setup Mode is a state where the Secure Boot key databases (PK, KEK, DB, DBX) can be replaced without signature verification — used during initial platform setup. In Setup Mode, any user with physical access to the machine can clear the existing keys, install their own PK, and create a custom Secure Boot chain that trusts only their malware. Preventing Setup Mode entry from an OS-controlled policy ensures that even an administrator who can reach the UEFI settings cannot wipe the production Secure Boot keys.",
                Tags = ["uefi", "setup-mode", "pk", "secure-boot-keys", "physical-attack"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "UEFI Setup Mode blocked by OS policy. Cannot undo without physical UEFI reset via CMOS clear or recovery firmware mode. If the production Secure Boot PK needs to be updated due to compromise, physical access to the UEFI reset procedure is required. Only deploy in high-security environments with controlled physical access and documented recovery procedures.",
                ApplyOps = [RegOp.SetDword(UefiPolicyKey, "PreventUEFISetupMode", 1)],
                RemoveOps = [RegOp.DeleteValue(UefiPolicyKey, "PreventUEFISetupMode")],
                DetectOps = [RegOp.CheckDword(UefiPolicyKey, "PreventUEFISetupMode", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-credential-guard-vbs",
                Label = "Trusted Launch: Enable Credential Guard via Virtualization Based Security",
                Category = "Encryption",
                Description =
                    "Sets LsaCfgFlags=1 in the DeviceGuard key (value 1 = Enabled without UEFI lock; value 2 = Enabled with UEFI lock). Enables Windows Credential Guard — which moves LSASS (Local Security Authority Subsystem Service) and its credential storage into a VBS-protected isolated partition. LSASS contains NTLM password hashes, Kerberos tickets, and other credentials for authenticated users. Tools like Mimikatz that dump credentials from LSASS cannot access the isolated LSASS memory — all credential material is inside the VBS hypervisor partition, inaccessible to the normal OS kernel. This is one of the most impactful credential theft mitigations available.",
                Tags = ["credential-guard", "vbs", "lsass", "mimikatz", "credential-theft"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Credential Guard enabled (VBS-backed LSASS isolation). NTLM v2 hashes and Kerberos tickets in LSASS cannot be extracted by Mimikatz or similar tools. Requires Hyper-V platform (VBS prerequisite). Some legacy applications that access LSASS directly (certain smart-card middleware, third-party SSO) may break — test before enterprise rollout. Digest authentication is disabled when Credential Guard is active.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-enable-memory-integrity-hvci",
                Label = "Trusted Launch: Enable Memory Integrity (Hypervisor-Protected Code Integrity)",
                Category = "Encryption",
                Description =
                    "Sets HypervisorEnforcedCodeIntegrity=1 in the DeviceGuard key. Enables Memory Integrity (HVCI — Hypervisor-Protected Code Integrity), which uses VBS to run Kernel Mode Code Integrity (KMCI) checks inside the hypervisor. HVCI prevents unsigned or malicious kernel drivers from loading — even kernel exploits that normally allow unsigned kernel code injection are blocked because the kernel memory is managed from inside the hypervisor and cannot be directly written by the kernel itself. HVCI is one of the strongest kernel-level attack mitigations in Windows.",
                Tags = ["hvci", "memory-integrity", "kernel-protection", "unsigned-driver", "vbs"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "HVCI (Memory Integrity) enabled. Unsigned or improperly signed kernel drivers cannot load. Some older third-party drivers (hardware vendor utilities, older antivirus kernel drivers, certain network drivers) are not HVCI-compatible and will prevent this from enabling. Use Device Manager and Windows Security Center to identify incompatible drivers before enabling. Incompatible drivers must be updated or removed.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                        "Enabled",
                        1
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                        "Enabled"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                        "Enabled",
                        1
                    ),
                ],
            },
            new TweakDef
            {
                Id = "trlnch-enable-system-guard-secure-launch",
                Label = "Trusted Launch: Enable System Guard Secure Launch (Runtime Firmware Protection)",
                Category = "Encryption",
                Description =
                    "Sets Enabled=1 in the SystemGuard\\Scenarios\\SystemGuard\\Enabled key. Enables System Guard Secure Launch (also known as DRTM — Dynamic Root of Trust for Measurement). DRTM uses Intel TXT or AMD SKINIT hardware extensions to establish a clean, verifiable root of trust at runtime, independently of the system firmware. This protects against firmware compromise — even if the UEFI firmware is modified by a sophisticated firmware implant, DRTM creates a new root of trust without relying on the compromised firmware. System Guard then measures the Windows kernel startup environment from this trusted state.",
                Tags = ["system-guard", "secure-launch", "drtm", "intel-txt", "firmware-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "System Guard Secure Launch (DRTM) enabled. Requires Intel TXT or AMD SKINIT hardware support. Intel TXT requires TXT-enabled CPU (most Intel vPro platforms) and TXT-enabled BIOS settings. On unsupported hardware, setting this has no effect. Provides the strongest available protection against firmware-level compromise.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SystemGuard\Scenarios\SystemGuard", "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SystemGuard\Scenarios\SystemGuard", "Enabled")],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SystemGuard\Scenarios\SystemGuard", "Enabled", 1),
                ],
            },
            new TweakDef
            {
                Id = "trlnch-enable-hyperv-vm-encrypted-state",
                Label = "Trusted Launch: Enable Encrypted State for Hyper-V VMs (Shielded VM Support)",
                Category = "Encryption",
                Description =
                    "Sets VMEncryptedStateEnabled=1 in the Hyper-V policy key. Enables the encrypted state feature for Hyper-V VMs — part of the Shielded VM infrastructure. Shielded VMs (Generation 2 only) encrypt the VM state files (memory snapshot, save states) with BitLocker and use a vTPM sealed key. Without encrypted state, Hyper-V VM save files and checkpoint state files stored on-disk are in plaintext — an attacker who gains access to the Hyper-V host storage can extract credentials from a saved VM snapshot using memory analysis tools. Encrypted VM state prevents this class of offline VM memory forensics.",
                Tags = ["hyperv", "shielded-vm", "encrypted-state", "vm-snapshot", "vTPM"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "VM save state and checkpoint files are encrypted. Requires vTPM enabled on VMs. Slightly higher I/O overhead during save/restore operations due to encryption. Shielded VM policy and Host Guardian Service (HGS) infrastructure is recommended for full Shielded VM deployment but is not required for encrypted state only.",
                ApplyOps = [RegOp.SetDword(HyperVPolicyKey, "VMEncryptedStateEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(HyperVPolicyKey, "VMEncryptedStateEnabled")],
                DetectOps = [RegOp.CheckDword(HyperVPolicyKey, "VMEncryptedStateEnabled", 1)],
            },
            new TweakDef
            {
                Id = "trlnch-block-kernel-debug-vtl0",
                Label = "Trusted Launch: Block Kernel Debugging From VTL0 (Normal OS) to VTL1 (Secure World)",
                Category = "Encryption",
                Description =
                    "Sets BlockDebuggerForVTL1=1 in the DeviceGuard key. Prevents normal OS (VTL0) kernel debugger sessions from accessing the VTL1 (Secure World) memory or state. In a VBS environment, VTL0 contains the normal Windows kernel and VTL1 contains the secure kernel (Credential Guard, HVCI agent). A kernel debugger attached to VTL0 should not be able to read VTL1 memory — but without this protection, certain kernel debug commands can inadvertently expose VTL boundary-crossing information. Blocking this ensures a hard separation between debug sessions and the secure world.",
                Tags = ["vbs", "vtl1", "kernel-debug", "secure-world", "isolation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "VTL0 kernel debugger cannot access VTL1 state. Normal WinDbg kernel debug sessions are unaffected for debugging regular kernel (VTL0) operations. VTL1-specific debugging requires a separate secure kernel debugger connection. No user-visible impact.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "BlockDebuggerForVTL1", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "BlockDebuggerForVTL1")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "BlockDebuggerForVTL1", 1)],
            },
        ];
}
