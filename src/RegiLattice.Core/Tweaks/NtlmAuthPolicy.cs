#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 238 — NTLM Authentication Policy (10 tweaks)
// Keys: HKLM\SYSTEM\CurrentControlSet\Control\Lsa and Netlogon\Parameters
internal static class NtlmAuthPolicy
{
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
    private const string MsvKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ntlm-deny-ntlmv1-outbound",
            Label = "Deny NTLMv1 Outbound Authentication",
            Category = "NTLM Auth Policy",
            Description = "Restricts outbound NTLM to NTLMv2 only; NTLMv1 responses are refused.",
            Tags = ["ntlm", "authentication", "hardening", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "NTLMv1 is broken; this prevents credential theft via NTLM relay attacks. Test for legacy app compat.",
            ApplyOps = [RegOp.SetDword(LsaKey, "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "LmCompatibilityLevel")],
            DetectOps = [RegOp.CheckDword(LsaKey, "LmCompatibilityLevel", 5)],
        },

        new TweakDef
        {
            Id = "ntlm-disable-lmhash-storage",
            Label = "Disable LM Hash Storage",
            Category = "NTLM Auth Policy",
            Description = "Prevents Windows from storing LAN Manager password hashes in the SAM database.",
            Tags = ["ntlm", "lm-hash", "sam", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "LM hashes are trivially cracked. Removing them from SAM prevents offline password attacks.",
            ApplyOps = [RegOp.SetDword(LsaKey, "NoLMHash", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "NoLMHash")],
            DetectOps = [RegOp.CheckDword(LsaKey, "NoLMHash", 1)],
        },

        new TweakDef
        {
            Id = "ntlm-require-ntlmv2-session-security-128",
            Label = "Require 128-bit NTLMv2 Session Security",
            Category = "NTLM Auth Policy",
            Description = "Forces NTLM session security to use 128-bit encryption and NTLMv2 message integrity.",
            Tags = ["ntlm", "session-security", "encryption", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Ensures NTLM sessions use strong encryption. Breaks connections to legacy systems without 128-bit support.",
            ApplyOps = [RegOp.SetDword(MsvKey, "NTLMMinClientSec", 537395200)],
            RemoveOps = [RegOp.DeleteValue(MsvKey, "NTLMMinClientSec")],
            DetectOps = [RegOp.CheckDword(MsvKey, "NTLMMinClientSec", 537395200)],
        },

        new TweakDef
        {
            Id = "ntlm-require-server-ntlmv2-128",
            Label = "Require 128-bit NTLMv2 Server Session Security",
            Category = "NTLM Auth Policy",
            Description = "Forces the NTLM server to require 128-bit session security and NTLMv2 from clients.",
            Tags = ["ntlm", "server", "session-security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Prevents downgrade of session security on server side. Rejects NTLMv1 client connections.",
            ApplyOps = [RegOp.SetDword(MsvKey, "NTLMMinServerSec", 537395200)],
            RemoveOps = [RegOp.DeleteValue(MsvKey, "NTLMMinServerSec")],
            DetectOps = [RegOp.CheckDword(MsvKey, "NTLMMinServerSec", 537395200)],
        },

        new TweakDef
        {
            Id = "ntlm-restrict-outbound-to-domain",
            Label = "Restrict NTLM Outbound Traffic to Domain Servers Only",
            Category = "NTLM Auth Policy",
            Description = "Denies NTLM authentication to servers outside the local domain (blocks NTLM relay to internet).",
            Tags = ["ntlm", "outbound", "relay", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Prevents NTLM relay to external servers. May break workgroup or cloud SMB scenarios.",
            ApplyOps = [RegOp.SetDword(LsaKey, "RestrictSendingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictSendingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(LsaKey, "RestrictSendingNTLMTraffic", 2)],
        },

        new TweakDef
        {
            Id = "ntlm-deny-inbound-ntlm",
            Label = "Deny All Inbound NTLM Authentication",
            Category = "NTLM Auth Policy",
            Description = "Blocks the local server from accepting NTLM authentication from any client.",
            Tags = ["ntlm", "inbound", "hardening", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 2,
            ImpactNote = "Fully blocks NTLM inbound. Only safe in Kerberos-only environments; breaks SMB file sharing for downlevel clients.",
            ApplyOps = [RegOp.SetDword(LsaKey, "RestrictReceivingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictReceivingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(LsaKey, "RestrictReceivingNTLMTraffic", 2)],
        },

        new TweakDef
        {
            Id = "ntlm-enable-audit-incoming",
            Label = "Enable NTLM Audit for Incoming Authentication",
            Category = "NTLM Auth Policy",
            Description = "Logs all incoming NTLM authentication attempts to the security event log for review.",
            Tags = ["ntlm", "audit", "logging", "monitoring"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables visibility into NTLM usage; essential before blocking NTLM to discover dependencies.",
            ApplyOps = [RegOp.SetDword(LsaKey, "AuditReceivingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "AuditReceivingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(LsaKey, "AuditReceivingNTLMTraffic", 2)],
        },

        new TweakDef
        {
            Id = "ntlm-enable-audit-outgoing",
            Label = "Enable NTLM Audit for Outgoing Authentication",
            Category = "NTLM Auth Policy",
            Description = "Logs all outgoing NTLM authentication attempts to identify apps still using NTLM.",
            Tags = ["ntlm", "audit", "outbound", "logging"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Use before restricting outbound NTLM to identify all callers.",
            ApplyOps = [RegOp.SetDword(LsaKey, "AuditNTLMInDomain", 7)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "AuditNTLMInDomain")],
            DetectOps = [RegOp.CheckDword(LsaKey, "AuditNTLMInDomain", 7)],
        },

        new TweakDef
        {
            Id = "ntlm-disable-null-sessions",
            Label = "Disable NTLM Null Session Access",
            Category = "NTLM Auth Policy",
            Description = "Prevents anonymous (null session) NTLM connections that can enumerate shares and users.",
            Tags = ["ntlm", "null-session", "anonymous", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks null-session enumeration. Some legacy utilities rely on null sessions; test first.",
            ApplyOps = [RegOp.SetDword(LsaKey, "RestrictAnonymous", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictAnonymous")],
            DetectOps = [RegOp.CheckDword(LsaKey, "RestrictAnonymous", 1)],
        },

        new TweakDef
        {
            Id = "ntlm-require-secure-channel-ntlmv2",
            Label = "Require NTLMv2 on Secure Channel",
            Category = "NTLM Auth Policy",
            Description = "Forces domain secure channel authentication to use NTLMv2; prevents downgrade to NTLMv1 in machine authentication.",
            Tags = ["ntlm", "secure-channel", "domain", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Ensures machine-to-DC communications use NTLMv2. Transparent in modern domains.",
            ApplyOps = [RegOp.SetDword(NetlogonKey, "RequireStrongKey", 1)],
            RemoveOps = [RegOp.DeleteValue(NetlogonKey, "RequireStrongKey")],
            DetectOps = [RegOp.CheckDword(NetlogonKey, "RequireStrongKey", 1)],
        },
    ];
}
