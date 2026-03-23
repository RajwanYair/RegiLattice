// RegiLattice.Core — Tweaks/CertificatePolicy.cs
// .NET Framework TLS hardening, certificate padding check, and revocation policies (Sprint 137).
// Slug "certpol" — SchUseStrongCrypto, SystemDefaultTlsVersions, Wintrust padding, revocation.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CertificatePolicy
{
    private const string Net4_64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319";
    private const string Net4_32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319";
    private const string Net2_64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v2.0.50727";
    private const string Net2_32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v2.0.50727";
    private const string WintrustKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config";
    private const string Wintrust32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Cryptography\Wintrust\Config";
    private const string AuthRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates\AuthRoot";
    private const string InternetSettings = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "certpol-dotnet-strong-crypto-64",
            Label = "Enable .NET 4 Strong Cryptography (64-bit)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets SchUseStrongCrypto=1 for 64-bit .NET Framework 4.0.30319, enabling "
                + "TLS 1.2/1.3 by default for all .NET applications and disabling RC4/3DES "
                + "weak cipher usage.",
            Tags = ["tls", "crypto", "dotnet", "security", "certificates"],
            RegistryKeys = [Net4_64],
            ApplyOps = [RegOp.SetDword(Net4_64, "SchUseStrongCrypto", 1)],
            RemoveOps = [RegOp.DeleteValue(Net4_64, "SchUseStrongCrypto")],
            DetectOps = [RegOp.CheckDword(Net4_64, "SchUseStrongCrypto", 1)],
        },
        new TweakDef
        {
            Id = "certpol-dotnet-strong-crypto-32",
            Label = "Enable .NET 4 Strong Cryptography (32-bit / WoW64)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets SchUseStrongCrypto=1 for 32-bit .NET Framework 4.0.30319 (WoW6432Node), "
                + "enabling TLS 1.2+ for 32-bit .NET applications and legacy COM-hosted .NET.",
            Tags = ["tls", "crypto", "dotnet", "wow64", "security"],
            RegistryKeys = [Net4_32],
            ApplyOps = [RegOp.SetDword(Net4_32, "SchUseStrongCrypto", 1)],
            RemoveOps = [RegOp.DeleteValue(Net4_32, "SchUseStrongCrypto")],
            DetectOps = [RegOp.CheckDword(Net4_32, "SchUseStrongCrypto", 1)],
        },
        new TweakDef
        {
            Id = "certpol-dotnet-tls12-default-64",
            Label = "Use System TLS Versions in .NET 4 (64-bit)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets SystemDefaultTlsVersions=1 for 64-bit .NET 4.x, allowing .NET apps "
                + "to inherit the system-wide TLS version from SChannel instead of hardcoding "
                + "a legacy TLS level.",
            Tags = ["tls", "tls12", "dotnet", "schannel", "security"],
            RegistryKeys = [Net4_64],
            ApplyOps = [RegOp.SetDword(Net4_64, "SystemDefaultTlsVersions", 1)],
            RemoveOps = [RegOp.DeleteValue(Net4_64, "SystemDefaultTlsVersions")],
            DetectOps = [RegOp.CheckDword(Net4_64, "SystemDefaultTlsVersions", 1)],
        },
        new TweakDef
        {
            Id = "certpol-dotnet-tls12-default-32",
            Label = "Use System TLS Versions in .NET 4 (32-bit / WoW64)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets SystemDefaultTlsVersions=1 for 32-bit .NET 4.x (WoW6432Node), " + "matching TLS negotiation behaviour of 64-bit .NET apps.",
            Tags = ["tls", "tls12", "dotnet", "wow64", "schannel"],
            RegistryKeys = [Net4_32],
            ApplyOps = [RegOp.SetDword(Net4_32, "SystemDefaultTlsVersions", 1)],
            RemoveOps = [RegOp.DeleteValue(Net4_32, "SystemDefaultTlsVersions")],
            DetectOps = [RegOp.CheckDword(Net4_32, "SystemDefaultTlsVersions", 1)],
        },
        new TweakDef
        {
            Id = "certpol-dotnet2-strong-crypto-64",
            Label = "Enable .NET 2/3.5 Strong Cryptography (64-bit)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets SchUseStrongCrypto=1 for 64-bit .NET Framework 2.0.50727 (used by "
                + ".NET 2.0 and 3.5 applications), enabling modern TLS for legacy .NET code.",
            Tags = ["tls", "crypto", "dotnet", "legacy", "security"],
            RegistryKeys = [Net2_64],
            ApplyOps = [RegOp.SetDword(Net2_64, "SchUseStrongCrypto", 1)],
            RemoveOps = [RegOp.DeleteValue(Net2_64, "SchUseStrongCrypto")],
            DetectOps = [RegOp.CheckDword(Net2_64, "SchUseStrongCrypto", 1)],
        },
        new TweakDef
        {
            Id = "certpol-dotnet2-strong-crypto-32",
            Label = "Enable .NET 2/3.5 Strong Cryptography (32-bit / WoW64)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets SchUseStrongCrypto=1 for 32-bit .NET 2.0.50727 (WoW6432Node), "
                + "ensuring legacy 32-bit .NET 2/3.5 apps use TLS 1.2+ and avoid RC4.",
            Tags = ["tls", "crypto", "dotnet", "legacy", "wow64"],
            RegistryKeys = [Net2_32],
            ApplyOps = [RegOp.SetDword(Net2_32, "SchUseStrongCrypto", 1)],
            RemoveOps = [RegOp.DeleteValue(Net2_32, "SchUseStrongCrypto")],
            DetectOps = [RegOp.CheckDword(Net2_32, "SchUseStrongCrypto", 1)],
        },
        new TweakDef
        {
            Id = "certpol-cert-padding-check-64",
            Label = "Enable Certificate Padding Check (64-bit)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets EnableCertPaddingCheck=1 in Wintrust for 64-bit binaries, enabling "
                + "strict enforcement of X.509 certificate padding to mitigate the Win32/Simda "
                + "and related certificate-spoofing attacks (MS13-098 hardening).",
            Tags = ["certificate", "padding check", "wintrust", "security", "x509"],
            RegistryKeys = [WintrustKey],
            ApplyOps = [RegOp.SetString(WintrustKey, "EnableCertPaddingCheck", "1")],
            RemoveOps = [RegOp.DeleteValue(WintrustKey, "EnableCertPaddingCheck")],
            DetectOps = [RegOp.CheckString(WintrustKey, "EnableCertPaddingCheck", "1")],
        },
        new TweakDef
        {
            Id = "certpol-cert-padding-check-32",
            Label = "Enable Certificate Padding Check (32-bit / WoW64)",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets EnableCertPaddingCheck=1 in the WoW6432Node Wintrust key for 32-bit "
                + "hosts, completing the MS13-098 certificate padding hardening for both "
                + "64-bit and 32-bit process spaces.",
            Tags = ["certificate", "padding check", "wintrust", "wow64", "security"],
            RegistryKeys = [Wintrust32],
            ApplyOps = [RegOp.SetString(Wintrust32, "EnableCertPaddingCheck", "1")],
            RemoveOps = [RegOp.DeleteValue(Wintrust32, "EnableCertPaddingCheck")],
            DetectOps = [RegOp.CheckString(Wintrust32, "EnableCertPaddingCheck", "1")],
        },
        new TweakDef
        {
            Id = "certpol-disable-root-auto-update",
            Label = "Disable Automatic Root Certificate Update",
            Category = "Certificate Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "Prevents Windows from automatically downloading new root certificates from "
                + "the Windows Update endpoint. Useful in air-gapped or strictly controlled "
                + "PKI environments. DisableRootAutoUpdate=1.",
            Tags = ["certificate", "root ca", "pki", "auto-update", "air-gap"],
            RegistryKeys = [AuthRoot],
            ApplyOps = [RegOp.SetDword(AuthRoot, "DisableRootAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(AuthRoot, "DisableRootAutoUpdate")],
            DetectOps = [RegOp.CheckDword(AuthRoot, "DisableRootAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "certpol-ie-cert-revocation",
            Label = "Enable Certificate Revocation Checking (Internet Settings)",
            Category = "Certificate Policy",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Ensures CertificateRevocation=1 in Internet Settings, forcing WinINet and "
                + "IE/legacy-WebBrowser TLS stacks to check OCSP/CRL for revoked certificates "
                + "before trusting a server certificate.",
            Tags = ["certificate", "ocsp", "crl", "revocation", "tls", "internet settings"],
            RegistryKeys = [InternetSettings],
            ApplyOps = [RegOp.SetDword(InternetSettings, "CertificateRevocation", 1)],
            RemoveOps = [RegOp.DeleteValue(InternetSettings, "CertificateRevocation")],
            DetectOps = [RegOp.CheckDword(InternetSettings, "CertificateRevocation", 1)],
        },
    ];
}
