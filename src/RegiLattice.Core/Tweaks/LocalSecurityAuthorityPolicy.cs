// RegiLattice.Core — Tweaks/LocalSecurityAuthorityPolicy.cs
// LSA hardening, RunAsPPL, anonymous enumeration, legacy auth package restriction, and audit policy — Sprint 526.
// Category: "Local Security Authority Policy" | Slug: lsapol
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\Lsa

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LocalSecurityAuthorityPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string CfgKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "lsapol-enable-lsa-runasppl",
            Label        = "Enable LSA Protected Process Light (RunAsPPL) Credential Guard",
            Category     = "Local Security Authority Policy",
            Description  = "Enables RunAsPPL for lsass.exe, running the Local Security Authority as a Protected Process Light, preventing credential dumping tools (Mimikatz, procdump lsass) from reading NTLM hashes and Kerberos tickets from the LSASS process.",
            Tags         = ["lsa", "runasppl", "credential-dump", "mimikatz", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "LSA RunAsPPL enabled; Mimikatz and LSASS credential dumping tools blocked from reading process memory.",
            ApplyOps     = [RegOp.SetDword(Key, "RunAsPPL", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RunAsPPL")],
            DetectOps    = [RegOp.CheckDword(Key, "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-disable-anonymous-enumeration-sam",
            Label        = "Disable Anonymous SAM Account and Share Enumeration",
            Category     = "Local Security Authority Policy",
            Description  = "Prevents anonymous network connections from enumerating local SAM accounts and security groups, blocking reconnaissance that discovers usernames for use in password spraying or brute-force attacks.",
            Tags         = ["lsa", "anonymous-enumeration", "sam", "reconnaissance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Anonymous SAM enumeration disabled; usernames not discoverable by unauthenticated network connections.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictAnonymousSAM", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictAnonymousSAM")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictAnonymousSAM", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-restrict-anonymous-access",
            Label        = "Restrict Anonymous Access to Named Pipes and Shares",
            Category     = "Local Security Authority Policy",
            Description  = "Blocks anonymous access to all named pipes and network shares, preventing unauthenticated connections that could be used for pass-the-hash attacks or to access network resources without valid credentials.",
            Tags         = ["lsa", "anonymous-access", "named-pipes", "shares", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Anonymous named pipe and share access blocked; unauthenticated CIFS/RPC connections rejected.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictAnonymous", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictAnonymous")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictAnonymous", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-disable-wdigest-cleartext",
            Label        = "Disable WDigest Cleartext Password Caching in LSASS",
            Category     = "Local Security Authority Policy",
            Description  = "Disables the WDigest authentication provider's cleartext password caching in LSASS memory, preventing credential dumping tools from extracting reversible plaintext passwords from the WDigest cache.",
            Tags         = ["lsa", "wdigest", "cleartext-password", "mimikatz", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "WDigest cleartext caching disabled; plaintext passwords no longer extractable from LSASS memory.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "UseLogonCredential", 0)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "UseLogonCredential")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id           = "lsapol-enable-lsa-audit",
            Label        = "Enable LSA Authentication Audit Logging",
            Category     = "Local Security Authority Policy",
            Description  = "Enables comprehensive Security audit logging for all LSA authentication events, including logon successes, failures, privilege escalations, and token creation, supporting SIEM-based authentication anomaly detection.",
            Tags         = ["lsa", "audit", "authentication", "event-log", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "LSA authentication audit logging enabled; all logon and privilege events recorded for SIEM.",
            ApplyOps     = [RegOp.SetDword(Key, "AuditBaseObjects", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AuditBaseObjects")],
            DetectOps    = [RegOp.CheckDword(Key, "AuditBaseObjects", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-crash-on-audit-fail",
            Label        = "Crash System When Security Audit Log Is Full (CrashOnAuditFail)",
            Category     = "Local Security Authority Policy",
            Description  = "Configures LSA to crash the system with a BSOD when the Security audit log becomes full and events cannot be written, ensuring audit records are never silently dropped on high-security systems that require complete audit trails.",
            Tags         = ["lsa", "audit-fail", "crash-on-full", "compliance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 3,
            ImpactNote   = "System BSOD on Security log full; complete audit trail guaranteed but availability risk if log fills. Use with large log size.",
            ApplyOps     = [RegOp.SetDword(Key, "CrashOnAuditFail", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "CrashOnAuditFail")],
            DetectOps    = [RegOp.CheckDword(Key, "CrashOnAuditFail", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-disable-legacy-auth-packages",
            Label        = "Remove Legacy Security Support Provider Packages from LSA",
            Category     = "Local Security Authority Policy",
            Description  = "Removes legacy SSPI authentication packages (msapsspc, msnsspc) from the LSA Security Packages list, preventing these deprecated packages from being loaded as SSPI providers that could be backdoored or exploited.",
            Tags         = ["lsa", "sspi", "legacy-packages", "authentication", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Legacy LSA SSPI packages removed; deprecated authentication DLLs not loaded in LSASS process.",
            ApplyOps     = [RegOp.SetDword(SecKey, "DisableLegacyLSAPackages", 1)],
            RemoveOps    = [RegOp.DeleteValue(SecKey, "DisableLegacyLSAPackages")],
            DetectOps    = [RegOp.CheckDword(SecKey, "DisableLegacyLSAPackages", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-deny-network-logon-local-accounts",
            Label        = "Deny Network Logon for Local Administrator Accounts",
            Category     = "Local Security Authority Policy",
            Description  = "Blocks local administrator accounts (SID S-1-5-113) from performing network logons (interactive pass-the-hash, NTLM relay), ensuring only domain accounts can authenticate over the network and local creds cannot be used for lateral movement.",
            Tags         = ["lsa", "network-logon", "local-admin", "lateral-movement", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Network logon denied for local administrator accounts; local account pass-the-hash lateral movement blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "DenyNetworkLogonForLocalAccounts", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DenyNetworkLogonForLocalAccounts")],
            DetectOps    = [RegOp.CheckDword(Key, "DenyNetworkLogonForLocalAccounts", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-enable-token-filter-policy",
            Label        = "Enable Local Account Token Filter Policy (Full Token on Network)",
            Category     = "Local Security Authority Policy",
            Description  = "Enables LocalAccountTokenFilterPolicy which allows local admin accounts that authenticate over the network to receive a full elevated token (rather than a filtered one), enabling legitimate remote administration without requiring domain accounts. Counterintuitively named, this is required for tools like PSExec to work over the network to local admin.",
            Tags         = ["lsa", "token-filter", "local-admin", "remote-admin", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 3,
            ImpactNote   = "Local account token filter disabled; local admin gets full elevated token on network logon. Required for PSExec-style remote admin.",
            ApplyOps     = [RegOp.SetDword(Key, "LocalAccountTokenFilterPolicy", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LocalAccountTokenFilterPolicy")],
            DetectOps    = [RegOp.CheckDword(Key, "LocalAccountTokenFilterPolicy", 1)],
        },
        new TweakDef
        {
            Id           = "lsapol-disable-lsa-telemetry",
            Label        = "Disable LSA / Authentication Provider Telemetry to Microsoft",
            Category     = "Local Security Authority Policy",
            Description  = "Prevents the LSA and Windows authentication providers from sending authentication event rates, credential provider selection, and logon failure telemetry to Microsoft.",
            Tags         = ["lsa", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "LSA telemetry to Microsoft disabled; auth event data and failure rates not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableLSATelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableLSATelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableLSATelemetry", 1)],
        },
    ];
}
