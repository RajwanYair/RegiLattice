// RegiLattice.Core — Tweaks/AccountProtection.cs
// Credential storage, LSA protection, and account login hardening.
// Slug: "acctprot" — LSA, WDigest, Winlogon, and local account policy registry keys.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AccountProtection
{
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    private const string WDigestKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

    private const string WinlogonKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

    private const string SystemPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "acctprot-disable-wdigest-plaintext",
            Label = "Disable WDigest Plaintext Password Storage in LSASS",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Blocks Mimikatz-style credential harvesting from LSASS memory.",
            Tags = ["wdigest", "lsass", "credentials", "password", "mimikatz", "security"],
            Description =
                "Prevents Windows from storing plaintext passwords in LSASS memory via the "
                + "WDigest authentication provider. UseLogonCredential=0 is the critical "
                + "counter-measure against Mimikatz-style credential harvesting attacks. "
                + "Effective on Windows 8.1+.",
            ApplyOps = [RegOp.SetDword(WDigestKey, "UseLogonCredential", 0)],
            RemoveOps = [RegOp.DeleteValue(WDigestKey, "UseLogonCredential")],
            DetectOps = [RegOp.CheckDword(WDigestKey, "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id = "acctprot-enable-lsa-protected-mode",
            Label = "Enable LSA Protected Process Light (PPL)",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Prevents LSASS injection attacks; strongly recommended for all Windows 10/11 systems.",
            Tags = ["lsa", "ppl", "protected process", "mimikatz", "credentials", "hardening"],
            Description =
                "Enables LSASS.exe to run as a Protected Process Light (PPL). RunAsPPL=1 "
                + "prevents even admin-level processes from injecting into LSASS, blocking "
                + "Mimikatz and similar credential-dumping tools. Requires Secure Boot. "
                + "A reboot is required for this change to take effect.",
            ApplyOps = [RegOp.SetDword(LsaKey, "RunAsPPL", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "RunAsPPL")],
            DetectOps = [RegOp.CheckDword(LsaKey, "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "acctprot-limit-credential-cache",
            Label = "Limit Cached Domain Credentials to 2 Entries",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["credentials", "domain", "cached logon", "offline", "security"],
            Description =
                "Limits the number of domain credentials cached locally to 2. "
                + "CachedLogonsCount=2 (default is 10). Fewer cached credentials = fewer "
                + "targets for offline hash-cracking attacks on stolen devices. "
                + "WARNING: if the domain is unreachable, cached accounts beyond 2 cannot log in.",
            ApplyOps = [RegOp.SetString(WinlogonKey, "CachedLogonsCount", "2")],
            RemoveOps = [RegOp.DeleteValue(WinlogonKey, "CachedLogonsCount")],
            DetectOps = [RegOp.CheckString(WinlogonKey, "CachedLogonsCount", "2")],
        },
        new TweakDef
        {
            Id = "acctprot-disable-domain-creds-storage",
            Label = "Prevent Domain Credentials from Being Stored in Credential Manager",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["credentials", "credential manager", "domain", "dipaka", "security"],
            Description =
                "Prevents Windows from storing network/domain credentials in Credential Manager "
                + "(Control Panel). DisableDomainCreds=1. Reduces the risk of attackers "
                + "extracting saved domain credentials from the Credential Manager store.",
            ApplyOps = [RegOp.SetDword(LsaKey, "DisableDomainCreds", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "DisableDomainCreds")],
            DetectOps = [RegOp.CheckDword(LsaKey, "DisableDomainCreds", 1)],
        },


        new TweakDef
        {
            Id = "acctprot-display-last-logon-info",
            Label = "Show Last Logon Info at Login (Compliance Visibility)",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["logon", "audit", "security", "last logon"],
            Description =
                "Displays the date, time, and whether the last logon was successful or failed "
                + "after a user logs in. DisplayLastLogonInfo=1. Helps users detect unauthorized "
                + "access attempts to their account.",
            ApplyOps = [RegOp.SetDword(SystemPolicy, "DisplayLastLogonInfo", 1)],
            RemoveOps = [RegOp.DeleteValue(SystemPolicy, "DisplayLastLogonInfo")],
            DetectOps = [RegOp.CheckDword(SystemPolicy, "DisplayLastLogonInfo", 1)],
        },
        new TweakDef
        {
            Id = "acctprot-block-remote-uac-bypass",
            Label = "Block Remote Admin UAC Token Filter Bypass",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["uac", "remote", "token filter", "admin", "lateral movement"],
            Description =
                "Ensures that remote administrator accounts are filtered to standard user tokens "
                + "over the network (LocalAccountTokenFilterPolicy=0). Prevents pass-the-hash "
                + "lateral movement using local admin credentials over remote shares/WMI/WinRM.",
            ApplyOps = [RegOp.SetDword(LsaKey, "LocalAccountTokenFilterPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "LocalAccountTokenFilterPolicy")],
            DetectOps = [RegOp.CheckDword(LsaKey, "LocalAccountTokenFilterPolicy", 0)],
        },

        new TweakDef
        {
            Id = "acctprot-audit-lsa-anonymous",
            Label = "Enable LSA Audit for Anonymous Access Attempts",
            Category = "Account Protection",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["lsa", "audit", "anonymous", "logging", "security"],
            Description =
                "Enables audit logging for anonymous access attempts to the Local Security "
                + "Authority. auditbaseobjects=1 + auditbasedirectories=1 ensures all "
                + "sensitive LSA object accesses are captured in the Security event log.",
            ApplyOps = [RegOp.SetDword(LsaKey, "auditbaseobjects", 1), RegOp.SetDword(LsaKey, "auditbasedirectories", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "auditbaseobjects"), RegOp.DeleteValue(LsaKey, "auditbasedirectories")],
            DetectOps = [RegOp.CheckDword(LsaKey, "auditbaseobjects", 1)],
        },
    ];
}
