namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyCryptography
{
    private const string CryptoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";
    private const string ConfigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "crypto-enforce-strong-key-protection",
            Label = "Enforce Strong Private Key Protection",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceKeyProtection=2 in Cryptography policy. Requires a strong password "
                + "prompt every time a stored private key is used. Prevents silent private key "
                + "access by malware or unauthorised processes.",
            Tags = ["cryptography", "key-protection", "private-key", "policy", "security"],
            RegistryKeys = [CryptoKey],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "User prompted for password on every private key use; some apps may break.",
            ApplyOps = [RegOp.SetDword(CryptoKey, "ForceKeyProtection", 2)],
            RemoveOps = [RegOp.DeleteValue(CryptoKey, "ForceKeyProtection")],
            DetectOps = [RegOp.CheckDword(CryptoKey, "ForceKeyProtection", 2)],
        },
        new TweakDef
        {
            Id = "crypto-disable-auto-root-update",
            Label = "Disable Automatic Root Certificate Update",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableDisallowedCertAutoUpdate=0 under Cryptography\\AuthRoot policy. "
                + "Prevents Windows from automatically downloading new root certificates from "
                + "Microsoft. In air-gapped or high-security environments, root CAs should be "
                + "managed via internal PKI only.",
            Tags = ["cryptography", "root-cert", "auto-update", "policy", "security"],
            RegistryKeys = [$@"{CryptoKey}\AuthRoot"],
            ImpactScore = 4,
            SafetyRating = 2,
            ImpactNote = "No auto-update of root CAs; manually deploy trusted roots via GPO.",
            ApplyOps = [RegOp.SetDword($@"{CryptoKey}\AuthRoot", "DisableRootAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CryptoKey}\AuthRoot", "DisableRootAutoUpdate")],
            DetectOps = [RegOp.CheckDword($@"{CryptoKey}\AuthRoot", "DisableRootAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "crypto-enforce-tls12-minimum",
            Label = "Enforce TLS 1.2 as Minimum Protocol",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Schannel cipher suite order to only include TLS 1.2+ suites. "
                + "Configures the system-wide SSL cipher suite list to exclude TLS 1.0 and 1.1, "
                + "enforcing TLS 1.2 as the minimum protocol version for all Schannel connections.",
            Tags = ["cryptography", "tls", "tls12", "cipher", "policy", "security"],
            RegistryKeys = [ConfigKey],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "TLS 1.0/1.1 disabled system-wide; legacy apps/servers may fail to connect.",
            ApplyOps = [RegOp.SetString(ConfigKey, "Functions",
                "TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384,TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256,"
                + "TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384,TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256,"
                + "TLS_DHE_RSA_WITH_AES_256_GCM_SHA384,TLS_DHE_RSA_WITH_AES_128_GCM_SHA256")],
            RemoveOps = [RegOp.DeleteValue(ConfigKey, "Functions")],
            DetectOps = [RegOp.CheckString(ConfigKey, "Functions",
                "TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384,TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256,"
                + "TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384,TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256,"
                + "TLS_DHE_RSA_WITH_AES_256_GCM_SHA384,TLS_DHE_RSA_WITH_AES_128_GCM_SHA256")],
        },
        new TweakDef
        {
            Id = "crypto-enforce-rsa-2048-minimum",
            Label = "Enforce RSA 2048-bit Minimum Key Length",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MinimumKeyLength=2048 in Cryptography\\MSCEP policy. Enforces a minimum "
                + "RSA key length of 2048 bits for certificate enrollment, blocking 1024-bit and "
                + "shorter keys that are considered cryptographically weak.",
            Tags = ["cryptography", "rsa", "key-length", "policy", "security"],
            RegistryKeys = [$@"{CryptoKey}\MSCEP"],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "RSA keys under 2048 bits rejected during enrollment; strengthens PKI.",
            ApplyOps = [RegOp.SetDword($@"{CryptoKey}\MSCEP", "MinimumKeyLength", 2048)],
            RemoveOps = [RegOp.DeleteValue($@"{CryptoKey}\MSCEP", "MinimumKeyLength")],
            DetectOps = [RegOp.CheckDword($@"{CryptoKey}\MSCEP", "MinimumKeyLength", 2048)],
        },
        new TweakDef
        {
            Id = "crypto-disable-weak-hash-algorithms",
            Label = "Disable Weak Hash Algorithms (MD5/SHA1)",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Enabled=0 for MD5 and SHA1 under the Schannel hashing algorithms policy. "
                + "Disables MD5 and SHA1 for TLS handshake hash computation, forcing SHA-256 or "
                + "stronger algorithms. Prevents weak-hash downgrade attacks.",
            Tags = ["cryptography", "hash", "md5", "sha1", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5"],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "MD5/SHA1 disabled for TLS; legacy servers using weak hashes will fail.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\SHA", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Hashes\MD5", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "crypto-disable-ssl3-protocol",
            Label = "Disable SSL 3.0 Protocol (POODLE mitigation)",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Enabled=0 for SSL 3.0 Client and Server under Schannel Protocols. "
                + "Disables the SSL 3.0 protocol system-wide, mitigating the POODLE vulnerability "
                + "(CVE-2014-3566) and preventing downgrade attacks to SSL 3.0.",
            Tags = ["cryptography", "ssl3", "poodle", "protocol", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client"],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "SSL 3.0 disabled; extremely old clients/servers may fail to connect.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Server", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "crypto-disable-tls10-protocol",
            Label = "Disable TLS 1.0 Protocol",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Enabled=0 for TLS 1.0 Client and Server under Schannel Protocols. "
                + "Disables TLS 1.0 system-wide per NIST SP 800-52 Rev. 2 guidance. TLS 1.0 has "
                + "known weaknesses (BEAST, padding oracle) and should be retired.",
            Tags = ["cryptography", "tls10", "protocol", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client"],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "TLS 1.0 disabled; applications requiring TLS 1.0 will fail to connect.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "crypto-disable-tls11-protocol",
            Label = "Disable TLS 1.1 Protocol",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Enabled=0 for TLS 1.1 Client and Server under Schannel Protocols. "
                + "Disables TLS 1.1 system-wide per NIST SP 800-52 Rev. 2 guidance. TLS 1.1 "
                + "lacks AEAD ciphers and should be retired in favour of TLS 1.2+.",
            Tags = ["cryptography", "tls11", "protocol", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client"],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "TLS 1.1 disabled; applications requiring TLS 1.1 will fail to connect.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Server", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Server", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "crypto-enable-fips-mode",
            Label = "Enable FIPS-Compliant Algorithms Only",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets FIPSAlgorithmPolicy\\Enabled=1 under the local security policy. Forces Windows "
                + "to use only FIPS 140-2 validated cryptographic algorithms for encryption, signing, "
                + "and hashing. Required by some government and defence compliance frameworks.",
            Tags = ["cryptography", "fips", "compliance", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy"],
            ImpactScore = 5,
            SafetyRating = 2,
            ImpactNote = "Only FIPS-validated algorithms allowed; many modern apps may break.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy", "Enabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy", "Enabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy", "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "crypto-enable-certificate-padding-check",
            Label = "Enable Strict Certificate Padding Validation",
            Category = "Cryptography Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableCertPaddingCheck=1 in WinVerifyTrust policy. Enforces strict "
                + "Authenticode certificate padding validation to prevent tampering with signed "
                + "executables. Mitigates attacks that append data after the signature.",
            Tags = ["cryptography", "certificate", "authenticode", "padding", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Strict Authenticode padding check; malformed signed binaries rejected.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config", "EnableCertPaddingCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config", "EnableCertPaddingCheck")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config", "EnableCertPaddingCheck", 1)],
        },
    ];
}
