namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Encryption
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
