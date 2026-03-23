#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Logon Cache Policy — controls how many domain credentials are cached locally,
// smart card / PIN removal behaviour, and logon security warnings.
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System
internal static class LogonCachePolicy
{
    private const string Winlogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string NetlogonParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
    private const string PolicySys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
    private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lgncache-reduce-cached-logons",
            Label = "Logon Cache: Set Cached Domain Logon Count to 2",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Winlogon],
            Tags = ["logon", "cache", "domain", "credential", "security", "policy"],
            Description =
                "Sets CachedLogonsCount=2 in Winlogon. Limits the number of domain credentials cached "
                + "locally to 2. Reduces the credential footprint on the disk. "
                + "Default: 10. Setting to 2 retains minimal offline logon capability while reducing exposure.",
            ApplyOps = [RegOp.SetString(Winlogon, "CachedLogonsCount", "2")],
            RemoveOps = [RegOp.DeleteValue(Winlogon, "CachedLogonsCount")],
            DetectOps = [RegOp.CheckString(Winlogon, "CachedLogonsCount", "2")],
        },
        new TweakDef
        {
            Id = "lgncache-disable-cached-logons",
            Label = "Logon Cache: Disable Cached Domain Logons (0 Cached)",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Winlogon],
            Tags = ["logon", "cache", "domain", "no-cache", "security", "hardening"],
            Description =
                "Sets CachedLogonsCount=0 in Winlogon. Completely disables domain credential caching. "
                + "Users must have network connectivity to log on with domain credentials. "
                + "Default: 10. Hardest security posture — use only in always-connected environments.",
            ApplyOps = [RegOp.SetString(Winlogon, "CachedLogonsCount", "0")],
            RemoveOps = [RegOp.DeleteValue(Winlogon, "CachedLogonsCount")],
            DetectOps = [RegOp.CheckString(Winlogon, "CachedLogonsCount", "0")],
        },
        new TweakDef
        {
            Id = "lgncache-sc-remove-lock",
            Label = "Logon Cache: Lock Workstation on Smart Card Removal",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Winlogon],
            Tags = ["logon", "smart-card", "lock", "security", "pki", "compliance"],
            Description =
                "Sets ScRemoveOption=1 in Winlogon. Locks the workstation immediately when the smart "
                + "card used for authentication is removed from the reader. "
                + "Default: 0 (no action). Value 1=Lock, 2=Force Logoff. Recommended: 1 for secure workstations.",
            ApplyOps = [RegOp.SetString(Winlogon, "ScRemoveOption", "1")],
            RemoveOps = [RegOp.DeleteValue(Winlogon, "ScRemoveOption")],
            DetectOps = [RegOp.CheckString(Winlogon, "ScRemoveOption", "1")],
        },
        new TweakDef
        {
            Id = "lgncache-password-expiry-warning-14d",
            Label = "Logon Cache: Set Password Expiry Warning to 14 Days",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Winlogon],
            Tags = ["logon", "password", "expiry", "warning", "policy", "compliance"],
            Description =
                "Sets PasswordExpiryWarning=14 in Winlogon. Shows password expiry reminder 14 days "
                + "before the password expires at logon time. "
                + "Default: 5 days. 14 days gives users adequate time to change before lockout.",
            ApplyOps = [RegOp.SetDword(Winlogon, "PasswordExpiryWarning", 14)],
            RemoveOps = [RegOp.DeleteValue(Winlogon, "PasswordExpiryWarning")],
            DetectOps = [RegOp.CheckDword(Winlogon, "PasswordExpiryWarning", 14)],
        },
        new TweakDef
        {
            Id = "lgncache-force-unlock-logon",
            Label = "Logon Cache: Require Domain Credential to Unlock Workstation",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Winlogon],
            Tags = ["logon", "unlock", "domain", "credential", "security", "compliance"],
            Description =
                "Sets ForceUnlockLogon=1 in Winlogon. Requires the same domain logon credential that was "
                + "used to lock the workstation. Local password changes are not accepted to unlock. "
                + "Default: 0. Prevents privilege escalation via local account unlocking.",
            ApplyOps = [RegOp.SetDword(Winlogon, "ForceUnlockLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(Winlogon, "ForceUnlockLogon")],
            DetectOps = [RegOp.CheckDword(Winlogon, "ForceUnlockLogon", 1)],
        },
        new TweakDef
        {
            Id = "lgncache-netlogon-require-strong-key",
            Label = "Logon Cache: Require Strong Session Keys for Netlogon",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [NetlogonParams],
            Tags = ["logon", "netlogon", "kerberos", "session-key", "security", "domain"],
            Description =
                "Sets RequireStrongKey=1 in Netlogon Parameters. Requires 128-bit session key for "
                + "Netlogon secure channel communications. Prevents downgrade to weaker encryption. "
                + "Default: 0. Required for hardened AD environments.",
            ApplyOps = [RegOp.SetDword(NetlogonParams, "RequireStrongKey", 1)],
            RemoveOps = [RegOp.DeleteValue(NetlogonParams, "RequireStrongKey")],
            DetectOps = [RegOp.CheckDword(NetlogonParams, "RequireStrongKey", 1)],
        },
        new TweakDef
        {
            Id = "lgncache-netlogon-require-sign-seal",
            Label = "Logon Cache: Require Signing and Sealing of Netlogon Channel",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [NetlogonParams],
            Tags = ["logon", "netlogon", "sign", "seal", "secure-channel", "domain", "security"],
            Description =
                "Sets RequireSignOrSeal=1 in Netlogon Parameters. Requires all domain controllers to "
                + "sign and seal all secure channel data for member machines. "
                + "Default: 0. Protects against Netlogon MITM and downgrade attacks (CVE-2020-1472 pattern).",
            ApplyOps = [RegOp.SetDword(NetlogonParams, "RequireSignOrSeal", 1)],
            RemoveOps = [RegOp.DeleteValue(NetlogonParams, "RequireSignOrSeal")],
            DetectOps = [RegOp.CheckDword(NetlogonParams, "RequireSignOrSeal", 1)],
        },
        new TweakDef
        {
            Id = "lgncache-netlogon-seal-secure-channel",
            Label = "Logon Cache: Seal Netlogon Secure Channel When Possible",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [NetlogonParams],
            Tags = ["logon", "netlogon", "seal", "encrypt", "domain", "security"],
            Description =
                "Sets SealSecureChannel=1 in Netlogon Parameters. Encrypts all data on the Netlogon "
                + "secure channel when supported by the domain controller. "
                + "Default: 0. Ensures confidentiality of domain authentication traffic.",
            ApplyOps = [RegOp.SetDword(NetlogonParams, "SealSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(NetlogonParams, "SealSecureChannel")],
            DetectOps = [RegOp.CheckDword(NetlogonParams, "SealSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "lgncache-netlogon-sign-secure-channel",
            Label = "Logon Cache: Sign Netlogon Secure Channel When Possible",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [NetlogonParams],
            Tags = ["logon", "netlogon", "sign", "integrity", "domain", "security"],
            Description =
                "Sets SignSecureChannel=1 in Netlogon Parameters. Digitally signs all data sent over "
                + "the Netlogon secure channel. Provides integrity verification of DC communications. "
                + "Default: 0. Complements SealSecureChannel for full MITM protection.",
            ApplyOps = [RegOp.SetDword(NetlogonParams, "SignSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(NetlogonParams, "SignSecureChannel")],
            DetectOps = [RegOp.CheckDword(NetlogonParams, "SignSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "lgncache-disable-domain-password-cache",
            Label = "Logon Cache: Disable Domain Password Caching in Credential Manager",
            Category = "Logon Cache Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Lsa],
            Tags = ["logon", "credential-manager", "password", "cache", "lsa", "security"],
            Description =
                "Sets DisableDomainCreds=1 in LSA. Prevents Windows from caching domain credentials "
                + "in the Credential Manager (Windows Vault). Applies to saved network passwords. "
                + "Default: 0 (caching allowed). Disabling reduces persistent credential exposure.",
            ApplyOps = [RegOp.SetDword(Lsa, "DisableDomainCreds", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "DisableDomainCreds")],
            DetectOps = [RegOp.CheckDword(Lsa, "DisableDomainCreds", 1)],
        },
    ];
}
