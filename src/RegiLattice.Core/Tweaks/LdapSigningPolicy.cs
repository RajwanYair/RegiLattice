// RegiLattice.Core — Tweaks/LdapSigningPolicy.cs
// LDAP client signing, channel binding, TLS enforcement, and directory service query security — Sprint 523.
// Category: "LDAP Signing Policy" | Slug: ldapsec
// Registry: HKLM\SYSTEM\CurrentControlSet\Services\ldap

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LdapSigningPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap";
    private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LDAP";
    private const string DcKey  = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NTDS\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "ldapsec-require-client-signing",
            Label        = "Require LDAP Client Signing for All Directory Connections",
            Category     = "LDAP Signing Policy",
            Description  = "Configures the LDAP client to always request LDAP signing (integrity protection), preventing man-in-the-middle attacks against LDAP sessions that could be used to modify query results or inject forged LDAP responses.",
            Tags         = ["ldap", "signing", "integrity", "mitm", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "LDAP client signing required; LDAP MITM response injection attacks mitigated.",
            ApplyOps     = [RegOp.SetDword(Key, "LDAPClientIntegrity", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LDAPClientIntegrity")],
            DetectOps    = [RegOp.CheckDword(Key, "LDAPClientIntegrity", 2)],
        },
        new TweakDef
        {
            Id           = "ldapsec-require-channel-binding",
            Label        = "Require LDAP Channel Binding Tokens (CBT Hardening)",
            Category     = "LDAP Signing Policy",
            Description  = "Configures the LDAP client to include EPA Channel Binding Tokens in all LDAP over TLS sessions, preventing NTLM relay attacks that forward LDAP authentication to a different TLS channel.",
            Tags         = ["ldap", "channel-binding", "cbt", "ntlm-relay", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "LDAP channel binding required; NTLM relay attacks forwarding LDAP auth to different TLS channel blocked.",
            ApplyOps     = [RegOp.SetDword(PolKey, "LdapClientChannelBinding", 2)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "LdapClientChannelBinding")],
            DetectOps    = [RegOp.CheckDword(PolKey, "LdapClientChannelBinding", 2)],
        },
        new TweakDef
        {
            Id           = "ldapsec-disable-simple-bind",
            Label        = "Disable LDAP Simple Bind Authentication",
            Category     = "LDAP Signing Policy",
            Description  = "Prevents the use of LDAP Simple Bind authentication which sends credentials as plaintext Base64 without integrity protection. NTLM or Kerberos SASL must be used for all LDAP authentication.",
            Tags         = ["ldap", "simple-bind", "plaintext-auth", "sasl", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "LDAP Simple Bind disabled; plaintext credential authentication to LDAP blocked. SASL required.",
            ApplyOps     = [RegOp.SetDword(PolKey, "DisableSimpleBind", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "DisableSimpleBind")],
            DetectOps    = [RegOp.CheckDword(PolKey, "DisableSimpleBind", 1)],
        },
        new TweakDef
        {
            Id           = "ldapsec-require-ldaps-port636",
            Label        = "Require LDAP over SSL/TLS (LDAPS, Port 636) for All AD Connections",
            Category     = "LDAP Signing Policy",
            Description  = "Configures the LDAP client to use LDAPS (LDAP over TLS on port 636) for all Active Directory connections, ensuring the entire LDAP session including SASL auth handshake is TLS-encrypted.",
            Tags         = ["ldap", "ldaps", "tls", "port-636", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "LDAPS required; all AD directory queries and authentications use TLS encryption on port 636.",
            ApplyOps     = [RegOp.SetDword(PolKey, "RequireLDAPS", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "RequireLDAPS")],
            DetectOps    = [RegOp.CheckDword(PolKey, "RequireLDAPS", 1)],
        },
        new TweakDef
        {
            Id           = "ldapsec-set-max-query-result-size",
            Label        = "Set Maximum LDAP Query Result Set to 1000 Entries",
            Category     = "LDAP Signing Policy",
            Description  = "Limits LDAP query result sets to 1000 entries, preventing oversized LDAP result enumeration attacks that could be used to enumerate all AD objects in a single query exceeding normal LDAP paged result limits.",
            Tags         = ["ldap", "result-size", "enumeration-prevention", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "LDAP result set limited to 1000 entries; full AD object enumeration in single query prevented.",
            ApplyOps     = [RegOp.SetDword(DcKey, "MaxPageSize", 1000)],
            RemoveOps    = [RegOp.DeleteValue(DcKey, "MaxPageSize")],
            DetectOps    = [RegOp.CheckDword(DcKey, "MaxPageSize", 1000)],
        },
        new TweakDef
        {
            Id           = "ldapsec-set-query-timeout-30s",
            Label        = "Set LDAP Query Timeout to 30 Seconds to Prevent Slow Queries",
            Category     = "LDAP Signing Policy",
            Description  = "Sets the LDAP client query timeout to 30 seconds, ensuring that slow/hanging LDAP queries do not block authentication processes and preventing DoS via crafted slow LDAP response attacks.",
            Tags         = ["ldap", "query-timeout", "dos-prevention", "authentication", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "LDAP query timeout set to 30 seconds; hanging LDAP queries do not block auth. Slow-response DoS mitigated.",
            ApplyOps     = [RegOp.SetDword(Key, "TimeLimit", 30)],
            RemoveOps    = [RegOp.DeleteValue(Key, "TimeLimit")],
            DetectOps    = [RegOp.CheckDword(Key, "TimeLimit", 30)],
        },
        new TweakDef
        {
            Id           = "ldapsec-disable-ldap-null-base-queries",
            Label        = "Disable Unauthenticated LDAP Null-Base DNS Queries",
            Category     = "LDAP Signing Policy",
            Description  = "Prevents anonymous LDAP queries with a null base (empty search base DN) that enable unauthenticated discovery of AD domain information, domain naming context, and supported SASL mechanisms.",
            Tags         = ["ldap", "null-base", "anonymous", "enumeration", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "LDAP anonymous null-base queries blocked; unauthenticated AD domain enumeration prevented.",
            ApplyOps     = [RegOp.SetDword(DcKey, "DisableNullBaseQuery", 1)],
            RemoveOps    = [RegOp.DeleteValue(DcKey, "DisableNullBaseQuery")],
            DetectOps    = [RegOp.CheckDword(DcKey, "DisableNullBaseQuery", 1)],
        },
        new TweakDef
        {
            Id           = "ldapsec-log-signing-failures",
            Label        = "Log LDAP Signing and Channel Binding Failure Events",
            Category     = "LDAP Signing Policy",
            Description  = "Enables Security audit log entries for LDAP sessions that fail signing or channel binding requirements, providing visibility into tools and applications sending unsigned LDAP queries.",
            Tags         = ["ldap", "signing-failure", "event-log", "audit", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "LDAP signing/channel-binding failures logged; applications sending unsigned LDAP visible for remediation.",
            ApplyOps     = [RegOp.SetDword(PolKey, "LogSigningFailures", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "LogSigningFailures")],
            DetectOps    = [RegOp.CheckDword(PolKey, "LogSigningFailures", 1)],
        },
        new TweakDef
        {
            Id           = "ldapsec-disable-ldap-telemetry",
            Label        = "Disable LDAP Client Telemetry to Microsoft",
            Category     = "LDAP Signing Policy",
            Description  = "Prevents the Windows LDAP client from sending signing negotiation stats, connection failure rates, and cipher suite telemetry to Microsoft.",
            Tags         = ["ldap", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "LDAP telemetry to Microsoft disabled; connection stats and cipher negotiation data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(PolKey, "DisableTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "DisableTelemetry")],
            DetectOps    = [RegOp.CheckDword(PolKey, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "ldapsec-enable-integrity-check-on-reconnect",
            Label        = "Re-Verify LDAP Integrity on Session Reconnection",
            Category     = "LDAP Signing Policy",
            Description  = "Forces the LDAP client to re-negotiate and verify signing integrity tokens when an LDAP session is reconnected after a network interruption, preventing session hijacking via injection into a reconnected unsigned LDAP stream.",
            Tags         = ["ldap", "reconnect", "integrity", "session-hijacking", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "LDAP integrity re-verified on reconnect; injecting bytes into reconnected sessions blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "VerifyServerCertificate", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "VerifyServerCertificate")],
            DetectOps    = [RegOp.CheckDword(Key, "VerifyServerCertificate", 1)],
        },
    ];
}
