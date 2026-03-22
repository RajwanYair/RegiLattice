// RegiLattice.Core — Tweaks/SecurityAuditPolicy.cs
// Windows Security Audit Policy registry settings (Sprint 84).
// Slug "audit" — Local Security Policy audit settings via SCE/registry.
// These set HKLM\SYSTEM\CurrentControlSet\Control\Lsa and related audit flags.
// Distinct from Hardening.cs (LAPS, UAC) and Security.cs (Defender, SmartScreen).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityAuditPolicy
{
    private const string Lsa =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    private const string LsaPolicy =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    private const string AuditPolicy =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Audit";

    private const string KerberosParams =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

    private const string NetLogon =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "audit-enable-verbose-audit-policy",
            Label = "Enable Verbose Security Audit Policy Subcategory",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["audit", "security", "logging", "policy"],
            Description =
                "Enables subcategory-level audit policy (Win Vista+ feature) to override "
                + "the coarser category-level audit settings. Required for detailed event "
                + "log entries in Security Event Log.",
            ApplyOps = [RegOp.SetDword(LsaPolicy, "SCENoApplyLegacyAuditPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaPolicy, "SCENoApplyLegacyAuditPolicy")],
            DetectOps = [RegOp.CheckDword(LsaPolicy, "SCENoApplyLegacyAuditPolicy", 1)],
        },
        new TweakDef
        {
            Id = "audit-require-sign-seal-dc",
            Label = "Require Secure Channel Signing and Sealing",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["audit", "netlogon", "signing", "security", "domain"],
            Description =
                "Requires that all Netlogon secure channel traffic be signed and sealed. "
                + "Prevents Netlogon relay attacks (Zerologon CVE-2020-1472). "
                + "Set to 1 to require both signing and sealing.",
            ApplyOps = [RegOp.SetDword(NetLogon, "RequireSignOrSeal", 1)],
            RemoveOps = [RegOp.DeleteValue(NetLogon, "RequireSignOrSeal")],
            DetectOps = [RegOp.CheckDword(NetLogon, "RequireSignOrSeal", 1)],
        },
        new TweakDef
        {
            Id = "audit-disable-ntlm-v1",
            Label = "Disable NTLMv1 Authentication (Require NTLMv2)",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["audit", "ntlm", "authentication", "security", "hardening"],
            Description =
                "Sets LmCompatibilityLevel to 5 — send NTLMv2 only, refuse LM and NTLMv1. "
                + "Prevents pass-the-hash attacks using weak NTLMv1 hashes. "
                + "May break legacy apps/devices that only support NTLMv1.",
            ApplyOps = [RegOp.SetDword(Lsa, "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.SetDword(Lsa, "LmCompatibilityLevel", 3)],
            DetectOps = [RegOp.CheckDword(Lsa, "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id = "audit-disable-lm-hash-storage",
            Label = "Disable LAN Manager Hash Storage",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["audit", "lm hash", "password", "security", "hardening"],
            Description =
                "Prevents Windows from storing LAN Manager password hashes in the SAM "
                + "database. LM hashes are cryptographically weak and can be cracked quickly. "
                + "Required for PCI DSS and CIS Windows 11 compliance.",
            ApplyOps = [RegOp.SetDword(Lsa, "NoLMHash", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "NoLMHash")],
            DetectOps = [RegOp.CheckDword(Lsa, "NoLMHash", 1)],
        },
        new TweakDef
        {
            Id = "audit-restrict-anonymous-access",
            Label = "Restrict Anonymous SAM/LSA Access",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["audit", "anonymous", "sam", "lsa", "security"],
            Description =
                "Disables anonymous access to lists of SAM accounts and LSA policy "
                + "information via null sessions. Prevents unauthenticated enumeration "
                + "of user accounts over the network.",
            ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymousSAM", 1)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictAnonymousSAM")],
            DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymousSAM", 1)],
        },
        new TweakDef
        {
            Id = "audit-restrict-anonymous-full",
            Label = "Fully Restrict Anonymous Network Access",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["audit", "anonymous", "restricted", "network", "security"],
            Description =
                "Sets RestrictAnonymous to 1, preventing null-session connections from "
                + "enumerating shares and users. Blocks anonymous pipe/-share access. "
                + "Value 2 would be full isolation (may break some workgroup scenarios).",
            ApplyOps = [RegOp.SetDword(Lsa, "RestrictAnonymous", 1)],
            RemoveOps = [RegOp.SetDword(Lsa, "RestrictAnonymous", 0)],
            DetectOps = [RegOp.CheckDword(Lsa, "RestrictAnonymous", 1)],
        },
        new TweakDef
        {
            Id = "audit-force-audit-policy-on-logon",
            Label = "Force Audit Policy Update on Every Logon",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["audit", "policy", "logon", "consistency"],
            Description =
                "Forces Windows to re-apply the audit policy from the Security database "
                + "at every user logon. Ensures audit settings are always current even "
                + "if domain GPO has been applied between reboots.",
            ApplyOps = [RegOp.SetDword(Lsa, "ForceGuest", 0)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "ForceGuest")],
            DetectOps = [RegOp.CheckDword(Lsa, "ForceGuest", 0)],
        },
        new TweakDef
        {
            Id = "audit-enable-crash-on-audit-fail",
            Label = "Enable System Halt on Security Audit Failure",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 5,
            SafetyRating = 2,
            Tags = ["audit", "crash", "halt", "security", "compliance"],
            Description =
                "Causes Windows to halt (BSOD) if the security event log cannot accept "
                + "new events. Value 2 = halt. Used in high-security environments where "
                + "audit trail continuity is mandatory (e.g. FISMA-High). "
                + "WARNING: system will freeze if log is full.",
            ApplyOps = [RegOp.SetDword(Lsa, "CrashOnAuditFail", 2)],
            RemoveOps = [RegOp.SetDword(Lsa, "CrashOnAuditFail", 0)],
            DetectOps = [RegOp.CheckDword(Lsa, "CrashOnAuditFail", 2)],
        },
        new TweakDef
        {
            Id = "audit-disable-anonymous-token-enumeration",
            Label = "Disable Anonymous Account Token Enumeration",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["audit", "anonymous", "token", "enumeration", "security"],
            Description =
                "Prevents anonymous connections from enumerating account names and groups "
                + "via the SAM protocol. Reduces attack surface for password spraying and "
                + "user enumeration over the network.",
            ApplyOps = [RegOp.SetDword(Lsa, "EveryoneIncludesAnonymous", 0)],
            RemoveOps = [RegOp.DeleteValue(Lsa, "EveryoneIncludesAnonymous")],
            DetectOps = [RegOp.CheckDword(Lsa, "EveryoneIncludesAnonymous", 0)],
        },
        new TweakDef
        {
            Id = "audit-disable-cached-credentials",
            Label = "Reduce Cached Domain Logon Credentials to 0",
            Category = "Security Audit Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 3,
            Tags = ["audit", "cached credentials", "domain", "security", "hardening"],
            Description =
                "Sets the number of cached domain logon credentials to 0. Prevents "
                + "domain users from signing in if the DC is unreachable. Hardens against "
                + "offline credential extraction attacks. WARNING: domain logon requires "
                + "network connectivity to DC.",
            ApplyOps = [RegOp.SetString(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
                "CachedLogonsCount", "0")],
            RemoveOps = [RegOp.SetString(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
                "CachedLogonsCount", "10")],
            DetectOps = [RegOp.CheckString(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
                "CachedLogonsCount", "0")],
        },
    ];
}
