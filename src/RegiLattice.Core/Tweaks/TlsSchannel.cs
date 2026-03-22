// RegiLattice.Core — Tweaks/TlsSchannel.cs
// Disable legacy SSL/TLS protocol versions and harden SCHANNEL configuration.
// Slug: "tls" — HKLM\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols.
// Disabling weak protocols prevents POODLE, BEAST, DROWN and similar downgrade attacks.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TlsSchannel
{
    // SCHANNEL root for all protocol version keys
    private const string SchannelRoot = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL";

    private const string Ssl20Client = SchannelRoot + @"\Protocols\SSL 2.0\Client";
    private const string Ssl20Server = SchannelRoot + @"\Protocols\SSL 2.0\Server";
    private const string Ssl30Client = SchannelRoot + @"\Protocols\SSL 3.0\Client";
    private const string Ssl30Server = SchannelRoot + @"\Protocols\SSL 3.0\Server";
    private const string Tls10Client = SchannelRoot + @"\Protocols\TLS 1.0\Client";
    private const string Tls10Server = SchannelRoot + @"\Protocols\TLS 1.0\Server";
    private const string Tls11Client = SchannelRoot + @"\Protocols\TLS 1.1\Client";
    private const string Tls11Server = SchannelRoot + @"\Protocols\TLS 1.1\Server";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tls-disable-ssl20",
            Label = "Disable SSL 2.0 (Client + Server)",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["ssl", "ssl2", "tls", "schannel", "security", "protocol"],
            Description =
                "Disables SSL 2.0 on both client and server sides. SSL 2.0 is considered "
                + "completely broken and has been deprecated since RFC 6176 (2011). "
                + "Sets Enabled=0 and DisabledByDefault=1 in SCHANNEL\\Protocols\\SSL 2.0.",
            ApplyOps =
            [
                RegOp.SetDword(Ssl20Client, "Enabled", 0),
                RegOp.SetDword(Ssl20Client, "DisabledByDefault", 1),
                RegOp.SetDword(Ssl20Server, "Enabled", 0),
                RegOp.SetDword(Ssl20Server, "DisabledByDefault", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(Ssl20Client, "Enabled"),
                RegOp.DeleteValue(Ssl20Client, "DisabledByDefault"),
                RegOp.DeleteValue(Ssl20Server, "Enabled"),
                RegOp.DeleteValue(Ssl20Server, "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(Ssl20Client, "Enabled", 0), RegOp.CheckDword(Ssl20Server, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-disable-ssl30",
            Label = "Disable SSL 3.0 (Client + Server)",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["ssl", "ssl3", "poodle", "tls", "schannel", "security", "protocol"],
            Description =
                "Disables SSL 3.0 on both client and server sides. SSL 3.0 is vulnerable to "
                + "POODLE (CVE-2014-3566) and other attacks. Deprecated by RFC 7568 (2015). "
                + "Sets Enabled=0 and DisabledByDefault=1 in SCHANNEL\\Protocols\\SSL 3.0.",
            ApplyOps =
            [
                RegOp.SetDword(Ssl30Client, "Enabled", 0),
                RegOp.SetDword(Ssl30Client, "DisabledByDefault", 1),
                RegOp.SetDword(Ssl30Server, "Enabled", 0),
                RegOp.SetDword(Ssl30Server, "DisabledByDefault", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(Ssl30Client, "Enabled"),
                RegOp.DeleteValue(Ssl30Client, "DisabledByDefault"),
                RegOp.DeleteValue(Ssl30Server, "Enabled"),
                RegOp.DeleteValue(Ssl30Server, "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(Ssl30Client, "Enabled", 0), RegOp.CheckDword(Ssl30Server, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-disable-tls10",
            Label = "Disable TLS 1.0 (Client + Server)",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["tls10", "tls", "schannel", "security", "protocol", "beast"],
            Description =
                "Disables TLS 1.0 on both client and server sides. TLS 1.0 is vulnerable "
                + "to BEAST (CVE-2011-3389) and POODLE-TLS attacks. Deprecated in RFC 8996 (2021). "
                + "WARNING: may break legacy apps that do not support TLS 1.2+.",
            ApplyOps =
            [
                RegOp.SetDword(Tls10Client, "Enabled", 0),
                RegOp.SetDword(Tls10Client, "DisabledByDefault", 1),
                RegOp.SetDword(Tls10Server, "Enabled", 0),
                RegOp.SetDword(Tls10Server, "DisabledByDefault", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(Tls10Client, "Enabled"),
                RegOp.DeleteValue(Tls10Client, "DisabledByDefault"),
                RegOp.DeleteValue(Tls10Server, "Enabled"),
                RegOp.DeleteValue(Tls10Server, "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(Tls10Client, "Enabled", 0), RegOp.CheckDword(Tls10Server, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-disable-tls11",
            Label = "Disable TLS 1.1 (Client + Server)",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["tls11", "tls", "schannel", "security", "protocol"],
            Description =
                "Disables TLS 1.1 on both client and server sides. TLS 1.1 is deprecated "
                + "per RFC 8996 (2021); replaced by TLS 1.2 and TLS 1.3. "
                + "WARNING: may break compatibility with older servers. Apply after confirming "
                + "all services support TLS 1.2 or higher.",
            ApplyOps =
            [
                RegOp.SetDword(Tls11Client, "Enabled", 0),
                RegOp.SetDword(Tls11Client, "DisabledByDefault", 1),
                RegOp.SetDword(Tls11Server, "Enabled", 0),
                RegOp.SetDword(Tls11Server, "DisabledByDefault", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(Tls11Client, "Enabled"),
                RegOp.DeleteValue(Tls11Client, "DisabledByDefault"),
                RegOp.DeleteValue(Tls11Server, "Enabled"),
                RegOp.DeleteValue(Tls11Server, "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(Tls11Client, "Enabled", 0), RegOp.CheckDword(Tls11Server, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-enable-schannel-event-logging",
            Label = "Enable Detailed SCHANNEL Event Logging",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["schannel", "tls", "logging", "security", "audit"],
            Description =
                "Enables verbose SCHANNEL event logging (EventLogging=7) which logs all TLS "
                + "handshake errors, certificate failures, and protocol negotiation details "
                + "to the System event log. Essential for diagnosing TLS configuration issues.",
            ApplyOps = [RegOp.SetDword(SchannelRoot, "EventLogging", 7)],
            RemoveOps = [RegOp.DeleteValue(SchannelRoot, "EventLogging")],
            DetectOps = [RegOp.CheckDword(SchannelRoot, "EventLogging", 7)],
        },
        new TweakDef
        {
            Id = "tls-disable-weak-rc4-cipher",
            Label = "Disable RC4 Cipher Suite",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["rc4", "cipher", "tls", "schannel", "security"],
            Description =
                "Disables the RC4 stream cipher in SCHANNEL. RC4 is cryptographically broken "
                + "(Bar-mitzvah attack, BEAST) and must not be used in any TLS negotiation. "
                + "Sets Enabled=0 in SCHANNEL\\Ciphers\\RC4 128/128.",
            ApplyOps =
            [
                RegOp.SetDword(SchannelRoot + @"\Ciphers\RC4 128/128", "Enabled", 0),
                RegOp.SetDword(SchannelRoot + @"\Ciphers\RC4 64/128", "Enabled", 0),
                RegOp.SetDword(SchannelRoot + @"\Ciphers\RC4 40/128", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(SchannelRoot + @"\Ciphers\RC4 128/128", "Enabled"),
                RegOp.DeleteValue(SchannelRoot + @"\Ciphers\RC4 64/128", "Enabled"),
                RegOp.DeleteValue(SchannelRoot + @"\Ciphers\RC4 40/128", "Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Ciphers\RC4 128/128", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-disable-des-cipher",
            Label = "Disable DES / Triple-DES Cipher Suites",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["des", "3des", "cipher", "tls", "schannel", "security"],
            Description =
                "Disables DES (56-bit) and Triple-DES (168-bit) cipher suites in SCHANNEL. "
                + "DES is trivially brute-forced; 3DES is vulnerable to SWEET32 (CVE-2016-2183). "
                + "Forces SCHANNEL to negotiate AES-128/256 cipher suites only.",
            ApplyOps =
            [
                RegOp.SetDword(SchannelRoot + @"\Ciphers\DES 56/56", "Enabled", 0),
                RegOp.SetDword(SchannelRoot + @"\Ciphers\Triple DES 168", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(SchannelRoot + @"\Ciphers\DES 56/56", "Enabled"),
                RegOp.DeleteValue(SchannelRoot + @"\Ciphers\Triple DES 168", "Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Ciphers\DES 56/56", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-disable-null-cipher",
            Label = "Disable NULL Cipher Suite",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["null cipher", "tls", "schannel", "security", "encryption"],
            Description =
                "Disables the NULL cipher suite which provides authentication with no encryption. "
                + "A NULL cipher connection is completely readable by any network observer. "
                + "Sets Enabled=0 in SCHANNEL\\Ciphers\\NULL.",
            ApplyOps = [RegOp.SetDword(SchannelRoot + @"\Ciphers\NULL", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(SchannelRoot + @"\Ciphers\NULL", "Enabled")],
            DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Ciphers\NULL", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-disable-md5-hash",
            Label = "Disable MD5 Hash Algorithm in SCHANNEL",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["md5", "hash", "tls", "schannel", "security"],
            Description =
                "Disables MD5 as a hash algorithm for TLS MAC and certificate signatures in "
                + "SCHANNEL. MD5 is collision-vulnerable (Flame malware used an MD5 collision). "
                + "Sets Enabled=0 in SCHANNEL\\Hashes\\MD5.",
            ApplyOps = [RegOp.SetDword(SchannelRoot + @"\Hashes\MD5", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(SchannelRoot + @"\Hashes\MD5", "Enabled")],
            DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Hashes\MD5", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "tls-enable-tls12-explicitly",
            Label = "Explicitly Enable TLS 1.2 (Client + Server)",
            Category = "TLS & SCHANNEL",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["tls12", "tls", "schannel", "security", "protocol"],
            Description =
                "Explicitly enables TLS 1.2 on both client and server sides (Enabled=1, "
                + "DisabledByDefault=0). While TLS 1.2 is enabled by default in modern Windows, "
                + "this hardens against GPO or registry changes that might inadvertently disable it. "
                + "Apply after disabling TLS 1.0/1.1 to confirm TLS 1.2 is the minimum floor.",
            ApplyOps =
            [
                RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Client", "Enabled", 1),
                RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Client", "DisabledByDefault", 0),
                RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Server", "Enabled", 1),
                RegOp.SetDword(SchannelRoot + @"\Protocols\TLS 1.2\Server", "DisabledByDefault", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Client", "Enabled"),
                RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Client", "DisabledByDefault"),
                RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Server", "Enabled"),
                RegOp.DeleteValue(SchannelRoot + @"\Protocols\TLS 1.2\Server", "DisabledByDefault"),
            ],
            DetectOps = [RegOp.CheckDword(SchannelRoot + @"\Protocols\TLS 1.2\Client", "Enabled", 1)],
        },
    ];
}
