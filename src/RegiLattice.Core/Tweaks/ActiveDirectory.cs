namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ActiveDirectory
{
    private const string Netlogon = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
    private const string KerbParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";
    private const string KerbPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Kerberos\Parameters";
    private const string AdWinSysPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ad-enable-machine-password-change",
            Label = "Ensure Machine Account Password Changes Are Enabled",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePasswordChange = 0 in Netlogon\\Parameters. Explicitly ensures that the Netlogon "
                + "service does NOT disable automatic domain machine account password rotation. Some misguided "
                + "hardening scripts set this to 1, which prevents the machine credential from ever rotating and "
                + "leaves a permanent compromisable static password in place.",
            Tags = ["ad", "netlogon", "machine-account", "password", "domain"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "DisablePasswordChange", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "DisablePasswordChange")],
            DetectOps = [RegOp.CheckDword(Netlogon, "DisablePasswordChange", 0)],
        },
        new TweakDef
        {
            Id = "ad-kerberos-max-token-size",
            Label = "Set Kerberos Maximum Token Size (65535 bytes)",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxTokenSize = 65535 in SYSTEM\\Control\\Lsa\\Kerberos\\Parameters. "
                + "Raises the Kerberos token buffer to 65535 bytes (from the default 12000 bytes). "
                + "Required on machines where users belong to many AD groups; prevents 'HTTP 400 header too large' "
                + "and Kerberos authentication failures caused by oversized PAC tokens.",
            Tags = ["ad", "kerberos", "token", "authentication", "groups"],
            RegistryKeys = [KerbParams],
            ApplyOps = [RegOp.SetDword(KerbParams, "MaxTokenSize", 65535)],
            RemoveOps = [RegOp.DeleteValue(KerbParams, "MaxTokenSize")],
            DetectOps = [RegOp.CheckDword(KerbParams, "MaxTokenSize", 65535)],
        },
        new TweakDef
        {
            Id = "ad-no-negative-cache-period",
            Label = "Disable Domain-Controller Negative Cache",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NegativeCachePeriod = 0 in Netlogon\\Parameters. Disables the negative cache that causes "
                + "authentication failures to be remembered for a period without retrying the DC. Prevents stale "
                + "DC failure records from blocking valid logins in environments with intermittent DC connectivity.",
            Tags = ["ad", "netlogon", "dc", "cache", "authentication"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "NegativeCachePeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "NegativeCachePeriod")],
            DetectOps = [RegOp.CheckDword(Netlogon, "NegativeCachePeriod", 0)],
        },
        new TweakDef
        {
            Id = "ad-netlogon-scavenge-interval",
            Label = "Set Netlogon SRV Record Scavenge Interval (5 Minutes)",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ScavengeInterval = 300 seconds in Netlogon\\Parameters. Controls how often Netlogon "
                + "rechecks stale DNS SRV records for domain controllers. 300 seconds ensures fresh DC "
                + "data after a failover or site rebalance, reducing the duration of DC discovery failures. Default: 300.",
            Tags = ["ad", "netlogon", "dns", "dc-failover", "scavenge"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "ScavengeInterval", 300)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "ScavengeInterval")],
            DetectOps = [RegOp.CheckDword(Netlogon, "ScavengeInterval", 300)],
        },
        new TweakDef
        {
            Id = "ad-no-nt4-crypto",
            Label = "Disallow NT4-Era Legacy Secure Channel Cryptography",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowNT4Crypto = 0 in Netlogon\\Parameters. Prevents Netlogon from falling back to "
                + "obsolete NT4 cryptographic algorithms on the secure channel when negotiating with older DCs. "
                + "All current domain controllers (Server 2008+) support modern Netlogon crypto. Default: 0.",
            Tags = ["ad", "netlogon", "crypto", "nt4", "secure-channel", "hardening"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "AllowNT4Crypto", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "AllowNT4Crypto")],
            DetectOps = [RegOp.CheckDword(Netlogon, "AllowNT4Crypto", 0)],
        },
        new TweakDef
        {
            Id = "ad-kerberos-aes-encryption",
            Label = "Require AES Kerberos Encryption (Disable RC4/DES)",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SupportedEncryptionTypes = 2147483640 in SYSTEM\\Control\\Lsa\\Kerberos\\Parameters. "
                + "Enables all AES (AES-128-CTS-HMAC-SHA1-96, AES-256-CTS-HMAC-SHA1-96) and disables RC4-HMAC, "
                + "DES-CBC-MD5, and DES-CBC-CRC. Kerberos RC4 is vulnerable to AS-REP roasting and pass-the-hash. "
                + "Requires all DCs and services to support AES (Server 2008+).",
            Tags = ["ad", "kerberos", "aes", "rc4", "encryption", "hardening"],
            RegistryKeys = [KerbParams],
            ApplyOps = [RegOp.SetDword(KerbParams, "SupportedEncryptionTypes", 2147483640)],
            RemoveOps = [RegOp.DeleteValue(KerbParams, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KerbParams, "SupportedEncryptionTypes", 2147483640)],
        },
        new TweakDef
        {
            Id = "ad-kerberos-armoring-fast",
            Label = "Enable Kerberos FAST Armoring (Claim-based)",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFAST = 1 in SOFTWARE\\Policies\\Microsoft\\Kerberos\\Parameters. Enables "
                + "Kerberos Flexible Authentication Secure Tunneling (FAST / RFC 6113). FAST wraps "
                + "authentication exchanges in an encrypted tunnel, protecting pre-authentication data from "
                + "offline password attacks. Requires Server 2012+ DCs.",
            Tags = ["ad", "kerberos", "fast", "armoring", "authentication", "hardening"],
            RegistryKeys = [KerbPol],
            ApplyOps = [RegOp.SetDword(KerbPol, "EnableFAST", 1)],
            RemoveOps = [RegOp.DeleteValue(KerbPol, "EnableFAST")],
            DetectOps = [RegOp.CheckDword(KerbPol, "EnableFAST", 1)],
        },
        new TweakDef
        {
            Id = "ad-no-mailslot-discovery",
            Label = "Disable Netlogon Mailslot DC Discovery",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MailslotDiscovery = 0 in Netlogon\\Parameters. Forces Netlogon to use DNS-based SRV "
                + "record lookups for domain controller discovery instead of legacy NetBIOS mailslot broadcasts. "
                + "Eliminates unnecessary broadcast traffic and reduces NetBIOS attack surface. Default: 1 (mailslot enabled).",
            Tags = ["ad", "netlogon", "dc-discovery", "mailslot", "netbios", "network"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "MailslotDiscovery", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "MailslotDiscovery")],
            DetectOps = [RegOp.CheckDword(Netlogon, "MailslotDiscovery", 0)],
        },
        new TweakDef
        {
            Id = "ad-no-single-label-dns",
            Label = "Disable Single-Label DNS Domain DC Discovery",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowSingleLabelDnsDomain = 0 in Netlogon\\Parameters. Prevents the Netlogon service "
                + "from querying DNS for domain controllers using bare single-label hostnames (e.g. 'corp' rather "
                + "than 'corp.example.com'). Single-label DNS queries can be intercepted or resolved to rogue hosts. Default: not set.",
            Tags = ["ad", "netlogon", "dns", "single-label", "security", "domain"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "AllowSingleLabelDnsDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "AllowSingleLabelDnsDomain")],
            DetectOps = [RegOp.CheckDword(Netlogon, "AllowSingleLabelDnsDomain", 0)],
        },
        new TweakDef
        {
            Id = "ad-no-enumerate-connected-users",
            Label = "Hide Connected/Domain Users on Sign-In Screen",
            Category = "User Account — Passwordless Sign In",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontEnumerateConnectedUsers = 1 via Windows System policy. Prevents the sign-in screen "
                + "from enumerating and displaying accounts of users currently connected over RDP or other "
                + "remote sessions, reducing information disclosure to local attackers or screen-watchers.",
            Tags = ["ad", "logon", "enumeration", "privacy", "policy", "domain"],
            RegistryKeys = [AdWinSysPol],
            ApplyOps = [RegOp.SetDword(AdWinSysPol, "DontEnumerateConnectedUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(AdWinSysPol, "DontEnumerateConnectedUsers")],
            DetectOps = [RegOp.CheckDword(AdWinSysPol, "DontEnumerateConnectedUsers", 1)],
        },
    ];
}
