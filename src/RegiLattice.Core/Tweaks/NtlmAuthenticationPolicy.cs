// RegiLattice.Core — Tweaks/NtlmAuthenticationPolicy.cs
// NTLM Authentication restriction Group Policy controls — Sprint 373.
// Category: "NTLM Restriction Policy" | Slug: ntlm
// Registry paths: HKLM\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters\
//                 HKLM\SOFTWARE\Policies\Microsoft\Windows\System\Audit (NTLM audit)
// MinBuild: 7600 (Windows 7+ / Windows 10+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NtlmAuthenticationPolicy
{
    private const string NtlmWorkKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect\NTLMRestrictions";
    private const string NtlmClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";
    private const string NtlmServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";
    private const string NtlmAuditKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "ntlm-restrict-outgoing-ntlm-to-servers",
            Label = "Restrict Outgoing NTLM Authentication to Remote Servers",
            Category = "NTLM Restriction Policy",
            Description = "Restricts NTLM authentication for outgoing network connections from this machine. Supports deny-all or allowlist modes; prevents NTLM relay and pass-the-hash attacks via outbound credential exposure.",
            Tags = ["ntlm", "authentication", "relay", "pass-the-hash", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Restricting outbound NTLM can break access to older servers or NAS devices that do not support Kerberos or NTLMv2. Audit before blocking.",
            RegistryKeys = [NtlmClientKey],
            ApplyOps  = [RegOp.SetDword(NtlmClientKey, "RestrictSendingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "RestrictSendingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(NtlmClientKey, "RestrictSendingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id = "ntlm-block-incoming-ntlm-auth",
            Label = "Block Incoming NTLM Authentication on This Server",
            Category = "NTLM Restriction Policy",
            Description = "Prevents this machine from accepting NTLM authentication for inbound network connections. Forces clients to negotiate a stronger protocol (Kerberos or NegotiateExt). Effective against NTLM relay to this host.",
            Tags = ["ntlm", "authentication", "server", "relay-protection", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Completely eliminates NTLM relay attack surface on this host. Will break legacy clients that cannot negotiate Kerberos; test extensively.",
            RegistryKeys = [NtlmServerKey],
            ApplyOps  = [RegOp.SetDword(NtlmServerKey, "RestrictReceivingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(NtlmServerKey, "RestrictReceivingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(NtlmServerKey, "RestrictReceivingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id = "ntlm-audit-outgoing-ntlm-traffic",
            Label = "Audit Outgoing NTLM Authentication Requests",
            Category = "NTLM Restriction Policy",
            Description = "Enables audit-only mode for outbound NTLM authentication, logging every server to which this machine sends NTLM credentials. Use audit data to build an allowlist before enforcing restrictions.",
            Tags = ["ntlm", "authentication", "audit", "event-log", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Non-disruptive; only produces event log entries. Essential preparatory step before enforcing NTLM restrictions.",
            RegistryKeys = [NtlmClientKey],
            ApplyOps  = [RegOp.SetDword(NtlmClientKey, "AuditReceivingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "AuditReceivingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(NtlmClientKey, "AuditReceivingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id = "ntlm-disable-ntlmv1",
            Label = "Disable NTLMv1 Authentication (Require NTLMv2 Minimum)",
            Category = "NTLM Restriction Policy",
            Description = "Configures the LAN Manager authentication level to refuse NTLMv1 and LM authentication. Requires NTLMv2 as the minimum level, or Kerberos where available. NTLMv1 is cryptographically weak and bruteforceable.",
            Tags = ["ntlm", "ntlmv1", "lm-hash", "authentication-level", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "NTLMv1 and LM hashes can be cracked in minutes. Setting LmCompatibilityLevel=5 enforces NTLMv2+; legacy Windows 9x clients cannot authenticate.",
            RegistryKeys = [NtlmClientKey],
            ApplyOps  = [RegOp.SetDword(NtlmClientKey, "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "LmCompatibilityLevel")],
            DetectOps = [RegOp.CheckDword(NtlmClientKey, "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id = "ntlm-require-session-security",
            Label = "Require NTLMv2 Session Security (128-bit Encryption)",
            Category = "NTLM Restriction Policy",
            Description = "Enforces 128-bit message confidentiality and integrity for all NTLM-based network sessions. Clients and servers that do not support 128-bit NTLMv2 session security cannot establish NTLM sessions.",
            Tags = ["ntlm", "session-security", "encryption", "ntlmv2", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Requires both message confidentiality and integrity (537395200 = 0x20080000 bitmask). Network monitoring tools and legacy SMB clients may fail.",
            RegistryKeys = [NtlmClientKey],
            ApplyOps  = [RegOp.SetDword(NtlmClientKey, "MinimumSessionSecurity", 537395200)],
            RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "MinimumSessionSecurity")],
            DetectOps = [RegOp.CheckDword(NtlmClientKey, "MinimumSessionSecurity", 537395200)],
        },
        new TweakDef
        {
            Id = "ntlm-block-ntlm-over-http",
            Label = "Block NTLM Authentication over HTTP (IWA Web Authentication)",
            Category = "NTLM Restriction Policy",
            Description = "Prevents Integrated Windows Authentication from presenting NTLM credentials to web proxies and HTTP endpoints. Restricts IWA to Kerberos-capable servers only, preventing NTLM relay via HTTP.",
            Tags = ["ntlm", "http", "iwa", "web-auth", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Can break intranet web apps that rely on NTLM-based IWA. Ensure web servers support Kerberos IWA (SPN registered, constrained delegation configured) before enabling.",
            RegistryKeys = [NtlmWorkKey],
            ApplyOps  = [RegOp.SetDword(NtlmWorkKey, "BlockNTLMOverHTTP", 1)],
            RemoveOps = [RegOp.DeleteValue(NtlmWorkKey, "BlockNTLMOverHTTP")],
            DetectOps = [RegOp.CheckDword(NtlmWorkKey, "BlockNTLMOverHTTP", 1)],
        },
        new TweakDef
        {
            Id = "ntlm-require-extended-protection",
            Label = "Require Extended Protection for NTLM Authentication",
            Category = "NTLM Restriction Policy",
            Description = "Enables Extended Protection for Authentication (EPA), binding NTLM tokens to the TLS channel they're sent over. Prevents cross-protocol NTLM relay even when credentials are intercepted in transit.",
            Tags = ["ntlm", "epa", "extended-protection", "channel-binding", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Most effective defence against NTLM relay. Requires server-side EPA support (IIS, LDAP, SMB). Old application servers may not support EPA.",
            RegistryKeys = [NtlmServerKey],
            ApplyOps  = [RegOp.SetDword(NtlmServerKey, "ExtendedProtectionForAuthentication", 2)],
            RemoveOps = [RegOp.DeleteValue(NtlmServerKey, "ExtendedProtectionForAuthentication")],
            DetectOps = [RegOp.CheckDword(NtlmServerKey, "ExtendedProtectionForAuthentication", 2)],
        },
        new TweakDef
        {
            Id = "ntlm-audit-all-domain-ntlm",
            Label = "Enable Domain-Level NTLM Authentication Audit",
            Category = "NTLM Restriction Policy",
            Description = "Configures domain-level NTLM auditing to log every NTLM authentication event across the domain, including accounts, client names, and server names. Essential for NTLM-reduction baselining.",
            Tags = ["ntlm", "domain", "audit", "event-log", "active-directory"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Generates high-volume events on Active Directory DCs during initial audit; filter before forwarding to SIEM.",
            RegistryKeys = [NtlmAuditKey],
            ApplyOps  = [RegOp.SetDword(NtlmAuditKey, "AuditNTLMAuthenticationInDomain", 7)],
            RemoveOps = [RegOp.DeleteValue(NtlmAuditKey, "AuditNTLMAuthenticationInDomain")],
            DetectOps = [RegOp.CheckDword(NtlmAuditKey, "AuditNTLMAuthenticationInDomain", 7)],
        },
        new TweakDef
        {
            Id = "ntlm-enable-server-allowlist",
            Label = "Enable NTLM Server Allowlist (Exception List) Enforcement",
            Category = "NTLM Restriction Policy",
            Description = "Activates enforcement of the NTLM server exception allowlist, permitting NTLM only to servers explicitly named in the AllowlistedServers value. All other NTLM outbound traffic is blocked.",
            Tags = ["ntlm", "allowlist", "exception", "server-list", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Must define AllowlistedServers (Multi-SZ) before enabling enforcement; otherwise all NTLM outbound traffic is blocked.",
            RegistryKeys = [NtlmClientKey],
            ApplyOps  = [RegOp.SetDword(NtlmClientKey, "EnableNTLMAllowList", 1)],
            RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "EnableNTLMAllowList")],
            DetectOps = [RegOp.CheckDword(NtlmClientKey, "EnableNTLMAllowList", 1)],
        },
        new TweakDef
        {
            Id = "ntlm-block-ntlm-to-ldap",
            Label = "Block NTLM Authentication to LDAP (Require Kerberos for AD)",
            Category = "NTLM Restriction Policy",
            Description = "Prevents LDAP clients on this machine from using NTLM to authenticate to Active Directory Domain Controllers. Forces Kerberos-based LDAP authentication, eliminating LDAP relay attack vectors.",
            Tags = ["ntlm", "ldap", "active-directory", "kerberos", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Can break LDAP queries from scripts or tools that hardcode NTLM. Verify all LDAP clients can use Kerberos before enforcing.",
            RegistryKeys = [NtlmWorkKey],
            ApplyOps  = [RegOp.SetDword(NtlmWorkKey, "BlockNTLMToLDAP", 1)],
            RemoveOps = [RegOp.DeleteValue(NtlmWorkKey, "BlockNTLMToLDAP")],
            DetectOps = [RegOp.CheckDword(NtlmWorkKey, "BlockNTLMToLDAP", 1)],
        },
    ];
}
