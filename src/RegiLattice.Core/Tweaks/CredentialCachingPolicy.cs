#nullable enable
using RegiLattice.Core.Models;
using System.Collections.Generic;

namespace RegiLattice.Core.Tweaks;

// Slug "credcache" — Credential caching hardening: CredSSP oracle, WDigest, LSA protection,
// anonymous enumeration restrictions, and LM hash storage.
// HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\CredSSP\Parameters
// HKLM\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation
// HKLM\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest
// HKLM\SYSTEM\CurrentControlSet\Control\Lsa
internal static class CredentialCachingPolicy
{
    private const string CredSSP =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\CredSSP\Parameters";

    private const string CredDel =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";

    private const string WDigest =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

    private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "credcache-mitigate-credssp-oracle",
            Label = "Mitigate CredSSP oracle vulnerability (CVE-2018-0886)",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CredSSP to 'Mitigated' to prevent credential forging via the encryption oracle. "
                + "AllowEncryptionOracle=1. Values: 0=Vulnerable, 1=Mitigated, 2=Force Updated. "
                + "Blocks connections to unpatched RDP servers (requires KB4103723 on remote).",
            Tags = ["credssp", "oracle", "rdp", "cve", "hardening"],
            ApplyOps = [RegOp.SetDword(CredSSP, "AllowEncryptionOracle", 1)],
            RemoveOps = [RegOp.DeleteValue(CredSSP, "AllowEncryptionOracle")],
            DetectOps = [RegOp.CheckDword(CredSSP, "AllowEncryptionOracle", 1)],
        },
        new TweakDef
        {
            Id = "credcache-restrict-rdp-admin-delegation",
            Label = "Restrict remote admin credential delegation",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts remote administration credential delegation to prevent pass-the-hash attacks. "
                + "RestrictedRemoteAdministration=1. Requires RDP clients to use RestrictedAdmin mode.",
            Tags = ["rdp", "delegation", "restricted-admin", "hardening"],
            ApplyOps = [RegOp.SetDword(CredDel, "RestrictedRemoteAdministration", 1)],
            RemoveOps = [RegOp.DeleteValue(CredDel, "RestrictedRemoteAdministration")],
            DetectOps = [RegOp.CheckDword(CredDel, "RestrictedRemoteAdministration", 1)],
        },
        new TweakDef
        {
            Id = "credcache-rdp-admin-type-protect",
            Label = "Require Protected Users or Restricted Admin for RDP",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Constrains remote RDP sessions to Protected Users group / Restricted Admin / Remote Credential Guard. "
                + "RestrictedRemoteAdministrationType=3. Prevents clear-text credential exposure.",
            Tags = ["rdp", "protected-users", "delegation", "hardening"],
            ApplyOps = [RegOp.SetDword(CredDel, "RestrictedRemoteAdministrationType", 3)],
            RemoveOps = [RegOp.DeleteValue(CredDel, "RestrictedRemoteAdministrationType")],
            DetectOps = [RegOp.CheckDword(CredDel, "RestrictedRemoteAdministrationType", 3)],
        },
        new TweakDef
        {
            Id = "credcache-disable-wdigest",
            Label = "Disable WDigest plaintext credential caching",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from storing credentials in plain text in memory for WDigest authentication. "
                + "UseLogonCredential=0. Mitigates Mimikatz and similar credential-dumping attacks.",
            Tags = ["wdigest", "credentials", "lsass", "mimikatz", "hardening"],
            ApplyOps = [RegOp.SetDword(WDigest, "UseLogonCredential", 0)],
            RemoveOps = [RegOp.DeleteValue(WDigest, "UseLogonCredential")],
            DetectOps = [RegOp.CheckDword(WDigest, "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id = "credcache-enable-lsa-protection",
            Label = "Enable LSA Protected Process Light (PPL)",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Runs LSASS as a Protected Process Light to block credential-dumping tools. "
                + "RunAsPPL=1. Requires Secure Boot and a system reboot. Default: disabled.",
            Tags = ["lsa", "lsass", "ppl", "credentials", "hardening"],
            ApplyOps = [RegOp.SetDword(Lsa, "RunAsPPL", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "RunAsPPL")],
            DetectOps = [RegOp.CheckDword(Lsa, "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "credcache-disable-domain-creds",
            Label = "Block storing network authentication credentials",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from storing credentials for network authentication to domain resources. "
                + "DisableDomainCreds=1. Network passwords will not be saved in Credential Manager.",
            Tags = ["credentials", "domain", "storage", "hardening"],
            ApplyOps = [RegOp.SetDword(Lsa, "DisableDomainCreds", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "DisableDomainCreds")],
            DetectOps = [RegOp.CheckDword(Lsa, "DisableDomainCreds", 1)],
        },
        new TweakDef
        {
            Id = "credcache-restrict-anonymous",
            Label = "Restrict anonymous enumeration of accounts and shares",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents anonymous users from enumerating SAM accounts and network shares. "
                + "RestrictAnonymous=1. Default: 0. Recommended baseline: 1 or 2.",
            Tags = ["anonymous", "sam", "enumeration", "hardening"],
            ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymous", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymous")],
            DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymous", 1)],
        },
        new TweakDef
        {
            Id = "credcache-restrict-anonymous-sam",
            Label = "Restrict anonymous enumeration of SAM account names",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents anonymous connections from enumerating SAM account names. "
                + "RestrictAnonymousSAM=1. Ensures this setting is policy-enforced and cannot be cleared.",
            Tags = ["anonymous", "sam", "policy", "hardening"],
            ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymousSAM", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymousSAM")],
            DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymousSAM", 1)],
        },
        new TweakDef
        {
            Id = "credcache-disable-everyone-anonymous",
            Label = "Remove Anonymous from the Everyone group",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the Anonymous user from being a member of the Everyone group. "
                + "EveryoneIncludesAnonymous=0. Restricts access to resources that grant permissions to Everyone.",
            Tags = ["anonymous", "everyone", "access-control", "hardening"],
            ApplyOps = [RegOp.SetDword(Lsa, "EveryoneIncludesAnonymous", 0)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "EveryoneIncludesAnonymous")],
            DetectOps = [RegOp.CheckDword(Lsa, "EveryoneIncludesAnonymous", 0)],
        },
        new TweakDef
        {
            Id = "credcache-disable-lm-hash",
            Label = "Disable LM hash storage for passwords",
            Category = "Credential Caching Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from storing an LM hash on the next password change. "
                + "NoLmHash=1. LM hashes are trivially crackable; disabling prevents offline attacks.",
            Tags = ["lm", "hash", "password", "credentials", "hardening"],
            ApplyOps = [RegOp.SetDword(Lsa, "NoLmHash", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "NoLmHash")],
            DetectOps = [RegOp.CheckDword(Lsa, "NoLmHash", 1)],
        },
    ];
}
