// RegiLattice.Core — Tweaks/NtlmRestrictionPolicy.cs
// NTLM authentication restriction, outbound blocking, NTLMv2 forcing, and relay-attack mitigations — Sprint 524.
// Category: "NTLM Restriction Policy" | Slug: ntlmadv
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NtlmRestrictionPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "ntlmadv-block-ntlmv1",
            Label        = "Block NTLMv1 Authentication (Allow NTLMv2 Only)",
            Category     = "NTLM Restriction Policy",
            Description  = "Configures LAN Manager authentication level to disallow NTLMv1 and LM authentication, accepting only NTLMv2 challenge-response and Kerberos, protecting against pass-the-hash from weak NTLMv1 hashes.",
            Tags         = ["ntlm", "ntlmv1", "ntlmv2", "pass-the-hash", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "NTLMv1 and LM responses blocked; only NTLMv2 accepted. Prevents weak hash capture attacks.",
            ApplyOps     = [RegOp.SetDword(LsaKey, "LmCompatibilityLevel", 5)],
            RemoveOps    = [RegOp.DeleteValue(LsaKey, "LmCompatibilityLevel")],
            DetectOps    = [RegOp.CheckDword(LsaKey, "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-require-ntlmv2-session-security",
            Label        = "Require NTLMv2 Session Security with 128-Bit Encryption",
            Category     = "NTLM Restriction Policy",
            Description  = "Configures NTLM session security to require NTLMv2 session security and 128-bit encryption for all NTLM sessions, preventing NTLM session reduction attacks that downgrade to weaker LM session security.",
            Tags         = ["ntlm", "session-security", "128-bit", "ntlmv2", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "NTLM sessions require NTLMv2 + 128-bit encryption; session downgrade attacks blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "NTLMMinClientSec", 537395200)],
            RemoveOps    = [RegOp.DeleteValue(Key, "NTLMMinClientSec")],
            DetectOps    = [RegOp.CheckDword(Key, "NTLMMinClientSec", 537395200)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-enable-extended-protection",
            Label        = "Enable NTLM Extended Protection for Authentication (EPA)",
            Category     = "NTLM Restriction Policy",
            Description  = "Enables NTLM Extended Protection for Authentication (EPA/CBT) on the client, adding channel binding tokens to NTLM authentication ensuring credentials can only be used on the TLS channel over which they were captured, preventing relay attacks.",
            Tags         = ["ntlm", "epa", "channel-binding", "relay-attack", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "NTLM EPA/CBT enabled; captured NTLM credentials cannot be replayed on a different TLS channel.",
            ApplyOps     = [RegOp.SetDword(Key, "EnableExtendedProtection", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "EnableExtendedProtection")],
            DetectOps    = [RegOp.CheckDword(Key, "EnableExtendedProtection", 2)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-disable-lm-hash-storage",
            Label        = "Disable LAN Manager (LM) Hash Storage in SAM",
            Category     = "NTLM Restriction Policy",
            Description  = "Prevents Windows from storing LAN Manager password hashes in the local SAM database, eliminating the easily cracked LM hash from credential stores that could be extracted by credential dumping tools.",
            Tags         = ["ntlm", "lm-hash", "sam", "credential-dump", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "LM hash storage disabled; SAM/LSASS no longer contains crackable LM password hashes.",
            ApplyOps     = [RegOp.SetDword(Key, "NoLMHash", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "NoLMHash")],
            DetectOps    = [RegOp.CheckDword(Key, "NoLMHash", 1)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-restrict-outbound-ntlm-to-negotiate",
            Label        = "Restrict Outbound NTLM Authentication to Negotiate Only",
            Category     = "NTLM Restriction Policy",
            Description  = "Configures the system to only use NTLM via the Negotiate (SPNEGO) mechanism for outbound authentication, preventing direct NTLM usage that bypasses Kerberos preference and could expose NTLM hashes to non-domain servers.",
            Tags         = ["ntlm", "outbound", "negotiate", "spnego", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Outbound NTLM restricted to Negotiate/SPNEGO; direct NTLM to non-domain servers blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictSendingNTLMTraffic", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictSendingNTLMTraffic")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictSendingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-restrict-incoming-ntlm",
            Label        = "Restrict Incoming NTLM Authentication to Domain Accounts Only",
            Category     = "NTLM Restriction Policy",
            Description  = "Configures the server component to only accept NTLM authentication from accounts in the domain (rejecting local SAM account NTLM), reducing the attack surface from pass-the-hash attacks using local account credentials.",
            Tags         = ["ntlm", "incoming", "domain-only", "pass-the-hash", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Incoming NTLM restricted to domain accounts; local SAM account NTLM authentication blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictReceivingNTLMTraffic", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictReceivingNTLMTraffic")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictReceivingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-enable-ntlm-audit-mode",
            Label        = "Enable NTLM Authentication Audit Logging",
            Category     = "NTLM Restriction Policy",
            Description  = "Enables system audit logging for all NTLM authentication attempts, recording both successful and failed NTLM sessions in the Security event log to support detection of lateral movement and pass-the-hash activity.",
            Tags         = ["ntlm", "audit", "event-log", "lateral-movement", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "NTLM authentication audit logging enabled; all NTLM sessions recorded for lateral movement detection.",
            ApplyOps     = [RegOp.SetDword(Key, "AuditReceivingNTLMTraffic", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AuditReceivingNTLMTraffic")],
            DetectOps    = [RegOp.CheckDword(Key, "AuditReceivingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-enable-outbound-audit",
            Label        = "Enable Audit Logging for Outbound NTLM Authentication",
            Category     = "NTLM Restriction Policy",
            Description  = "Enables audit logging for all outbound NTLM authentication attempts from this machine, providing visibility into which remote servers receive NTLM credentials from applications running on this endpoint.",
            Tags         = ["ntlm", "outbound-audit", "credential-exposure", "logging", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Outbound NTLM audit enabled; all remote servers receiving NTLM credentials from this machine logged.",
            ApplyOps     = [RegOp.SetDword(Key, "AuditSendingNTLMTraffic", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AuditSendingNTLMTraffic")],
            DetectOps    = [RegOp.CheckDword(Key, "AuditSendingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-disable-ntlm-auth-in-smb",
            Label        = "Require Kerberos Authentication for SMB (Block NTLM in SMB)",
            Category     = "NTLM Restriction Policy",
            Description  = "Configures SMB over network shares to require Kerberos authentication rather than NTLM, preventing NTLM relay attacks that capture and forward SMB authentication to remote shares or other services.",
            Tags         = ["ntlm", "smb", "kerberos", "relay-attack", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "SMB NTLM authentication blocked; Kerberos required for network share access. Relay attacks via SMB mitigated.",
            ApplyOps     = [RegOp.SetDword(PolKey, "RequireDomainAuthForSMB", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "RequireDomainAuthForSMB")],
            DetectOps    = [RegOp.CheckDword(PolKey, "RequireDomainAuthForSMB", 1)],
        },
        new TweakDef
        {
            Id           = "ntlmadv-disable-ntlm-telemetry",
            Label        = "Disable NTLM Authentication Telemetry to Microsoft",
            Category     = "NTLM Restriction Policy",
            Description  = "Prevents the Windows NTLM authentication provider from sending authentication success/failure rates, cipher usage, and session fallback telemetry to Microsoft.",
            Tags         = ["ntlm", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "NTLM telemetry to Microsoft disabled; auth stats and session data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableNTLMTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableNTLMTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableNTLMTelemetry", 1)],
        },
    ];
}
