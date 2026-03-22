// RegiLattice.Core — Tweaks/NtlmAuthentication.cs
// NTLM session security and authentication policy tweaks (Sprint 106).
// Slug: "ntlma" — tightens NTLM session security beyond LmCompatibilityLevel (Security.cs).
// Distinct from Security.cs (LmCompatibilityLevel, RunAsPPL, NoLMHash, WDigest).
// Registry paths:
//   HKLM\SYSTEM\CurrentControlSet\Control\Lsa  → NTLMMin*Sec, Restrict*, Audit*
//   HKLM\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0 → MSV1_0 NTLM authentication settings
//   HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters → secure channel settings

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NtlmAuthentication
{
    private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string Msv10 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";
    private const string Netlogon =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ntlma-enforce-client-session-security",
            Label = "NTLM: Enforce NTLMv2 and 128-Bit Encryption for Client Sessions",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["ntlm", "session-security", "encryption", "hardening", "authentication"],
            Description =
                "Sets NTLMMinClientSec=537395200 (0x20080000) in the LSA key. "
                + "Requires NTLM client sessions to use NTLMv2 session keys and 128-bit session encryption. "
                + "Bit flags: 0x00080000 = require NTLM2 session security, 0x20000000 = require 128-bit key. "
                + "Complements LmCompatibilityLevel by enforcing the session security level independently.",
            ApplyOps = [RegOp.SetDword(Lsa, "NTLMMinClientSec", 537395200)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "NTLMMinClientSec")],
            DetectOps = [RegOp.CheckDword(Lsa, "NTLMMinClientSec", 537395200)],
        },
        new TweakDef
        {
            Id = "ntlma-enforce-server-session-security",
            Label = "NTLM: Enforce NTLMv2 and 128-Bit Encryption for Server Sessions",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["ntlm", "session-security", "encryption", "hardening", "server"],
            Description =
                "Sets NTLMMinServerSec=537395200 (0x20080000) in the LSA key. "
                + "Requires NTLM server sessions to accept only NTLMv2 session keys with 128-bit encryption. "
                + "Prevents clients using weaker LM or NTLMv1 session security from negotiating a session "
                + "with this server, even if the client attempts to downgrade.",
            ApplyOps = [RegOp.SetDword(Lsa, "NTLMMinServerSec", 537395200)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "NTLMMinServerSec")],
            DetectOps = [RegOp.CheckDword(Lsa, "NTLMMinServerSec", 537395200)],
        },
        new TweakDef
        {
            Id = "ntlma-sign-netlogon-channel",
            Label = "NTLM: Require Digital Signing for Netlogon Secure Channel",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["ntlm", "netlogon", "signing", "domain", "secure-channel"],
            Description =
                "Sets SignSecureChannel=1 in the Netlogon Parameters key. "
                + "Requires the Netlogon secure channel (between domain member and domain controller) to be "
                + "digitally signed. Prevents man-in-the-middle attacks against the Netlogon protocol. "
                + "Related to CVE-2020-1472 (Zerologon) mitigations — enables signing as a baseline.",
            ApplyOps = [RegOp.SetDword(Netlogon, "SignSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "SignSecureChannel")],
            DetectOps = [RegOp.CheckDword(Netlogon, "SignSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "ntlma-seal-netlogon-channel",
            Label = "NTLM: Require Encryption (Seal) for Netlogon Secure Channel",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["ntlm", "netlogon", "encryption", "domain", "secure-channel"],
            Description =
                "Sets SealSecureChannel=1 in the Netlogon Parameters key. "
                + "Requires the Netlogon secure channel to be both signed and encrypted. "
                + "Sealing (encryption) is stronger than signing alone and prevents interception of "
                + "authentication traffic between this domain member and its domain controller.",
            ApplyOps = [RegOp.SetDword(Netlogon, "SealSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "SealSecureChannel")],
            DetectOps = [RegOp.CheckDword(Netlogon, "SealSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "ntlma-deny-all-outgoing-ntlm",
            Label = "NTLM: Deny All Outgoing NTLM Authentication Requests",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 5,
            SafetyRating = 2,
            Tags = ["ntlm", "restriction", "outgoing", "block", "lateral-movement"],
            SideEffects = "Blocks all outgoing NTLM authentication. Services using NTLM for remote connections will fail. Enable only where Kerberos is exclusively available.",
            Description =
                "Sets RestrictSendingNTLMTraffic=2 in the LSA key. "
                + "Completely blocks all outgoing NTLM authentication from this machine. "
                + "Values: 0=allow all, 1=audit and allow, 2=deny all. "
                + "Eliminates NTLM relay attack exposure entirely. Requires that all services use "
                + "Kerberos for authentication; not suitable for standalone or workgroup machines.",
            ApplyOps = [RegOp.SetDword(Lsa, "RestrictSendingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictSendingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(Lsa, "RestrictSendingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id = "ntlma-audit-all-domain-ntlm",
            Label = "NTLM: Audit All NTLM Authentication in Active Directory Domain",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ntlm", "audit", "domain", "logging", "monitoring"],
            Description =
                "Sets AuditNTLMInDomain=7 in the LSA key. "
                + "Enables comprehensive auditing of all inbound NTLM authentication requests processed "
                + "by a domain controller. Value 7 audits all NTLM (pass-through, domain accounts, and "
                + "non-domain accounts). Generates Event ID 4776 entries in the Security log. "
                + "Critical for identifying legacy systems still using NTLM before enforcing NTLM blocks.",
            ApplyOps = [RegOp.SetDword(Lsa, "AuditNTLMInDomain", 7)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "AuditNTLMInDomain")],
            DetectOps = [RegOp.CheckDword(Lsa, "AuditNTLMInDomain", 7)],
        },
        new TweakDef
        {
            Id = "ntlma-restrict-ntlm-in-domain",
            Label = "NTLM: Restrict NTLM In-Domain Authentication to Specific Servers",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Tags = ["ntlm", "restriction", "domain", "whitelist"],
            Description =
                "Sets RestrictNTLMInDomain=1 in the LSA key. "
                + "Restricts NTLM authentication for domain users to an allowlist of specific servers. "
                + "Values: 0=disabled, 1=audit NTLM, 2=deny all domain NTLM. "
                + "Setting to 1 allows auditing without blocking — safe to deploy as a monitoring step "
                + "before moving to 2 (enforce). Combined with AuditNTLMInDomain for full visibility.",
            ApplyOps = [RegOp.SetDword(Lsa, "RestrictNTLMInDomain", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictNTLMInDomain")],
            DetectOps = [RegOp.CheckDword(Lsa, "RestrictNTLMInDomain", 1)],
        },
        new TweakDef
        {
            Id = "ntlma-audit-logon-events-msv",
            Label = "NTLM: Enable NTLM Logon Auditing in MSV1_0",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ntlm", "audit", "logon", "msv10", "monitoring"],
            Description =
                "Sets NTLMAuditUserAccountLogonEvents=2 in the MSV1_0 subkey. "
                + "Enables auditing of all NTLM authentication attempts at the MSV1_0 level "
                + "(the local SAM authentication provider). Value 2 audits all events. "
                + "Generates additional detail in the Security event log alongside AuditNTLMInDomain.",
            ApplyOps = [RegOp.SetDword(Msv10, "NTLMAuditUserAccountLogonEvents", 2)],
            RemoveOps = [RegOp.DeleteValue(Msv10, "NTLMAuditUserAccountLogonEvents")],
            DetectOps = [RegOp.CheckDword(Msv10, "NTLMAuditUserAccountLogonEvents", 2)],
        },
        new TweakDef
        {
            Id = "ntlma-block-null-session-fallback",
            Label = "NTLM: Block Null Session NTLM Authentication Fallback",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["ntlm", "null-session", "anonymous", "security", "msv10"],
            Description =
                "Sets allownullsessionfallback=0 in the MSV1_0 subkey. "
                + "Disables the fallback that allows NTLM to authenticate as a null (anonymous) session "
                + "when no credentials are provided. Null sessions are used by legacy tools for anonymous "
                + "enumeration of shares, user accounts, and group memberships. Blocking this closes "
                + "a common Windows network reconnaissance pathway.",
            ApplyOps = [RegOp.SetDword(Msv10, "allownullsessionfallback", 0)],
            RemoveOps = [RegOp.DeleteValue(Msv10, "allownullsessionfallback")],
            DetectOps = [RegOp.CheckDword(Msv10, "allownullsessionfallback", 0)],
        },
        new TweakDef
        {
            Id = "ntlma-require-ntlmv2-msv",
            Label = "NTLM: Require NTLMv2 Session Security at MSV1_0 Level",
            Category = "NTLM Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["ntlm", "ntlmv2", "session-security", "msv10", "hardening"],
            Description =
                "Sets NtlmMinClientSec=536870912 (0x20000000) in the MSV1_0 subkey. "
                + "Requires NTLMv2 session security at the MSV1_0 (local authentication) level. "
                + "The flag 0x20000000 (NTLMSSP_NEGOTIATE_128) forces 128-bit session key negotiation "
                + "in all local NTLM authentication handled by the MSV1_0 package, "
                + "complementing NTLMMinClientSec set at the top-level LSA key.",
            ApplyOps = [RegOp.SetDword(Msv10, "NtlmMinClientSec", 536870912)],
            RemoveOps = [RegOp.DeleteValue(Msv10, "NtlmMinClientSec")],
            DetectOps = [RegOp.CheckDword(Msv10, "NtlmMinClientSec", 536870912)],
        },
    ];
}
