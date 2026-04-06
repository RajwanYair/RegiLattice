namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityLsaHardening
{
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    private const string LsaNetKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";

    private const string SecParamsKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurePipeServers\winreg";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "lsaharden-enable-ppl",
                Label = "Enable LSA Protected Process Light (PPL)",
                Category = "Security — LSA Protection",
                Description =
                    "Configures the Local Security Authority (LSA) process to run as a Protected Process Light. "
                    + "Prevents non-protected processes (including many malware tools) from injecting code or reading memory from lsass.exe. "
                    + "Default: not protected. Recommended: enabled (requires UEFI Secure Boot on some configurations).",
                Tags = ["lsa", "protected-process", "ppl", "credential-theft", "mimikatz", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(LsaKey, "RunAsPPL", 1)],
                RemoveOps = [RegOp.SetDword(LsaKey, "RunAsPPL", 0)],
                DetectOps = [RegOp.CheckDword(LsaKey, "RunAsPPL", 1)],
            },
            new TweakDef
            {
                Id = "lsaharden-enable-ppl-audit",
                Label = "Enable LSA PPL Audit Mode",
                Category = "Security — LSA Protection",
                Description =
                    "Sets LSA protection to audit mode (value 2): potential violations are logged without enforcing the restriction. "
                    + "Use to test Protected Process Light compatibility before enabling enforcement. "
                    + "Default: not set. Recommended: audit before enforcing PPL (RunAsPPL=1).",
                Tags = ["lsa", "protected-process", "audit", "ppl", "testing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(LsaKey, "RunAsPPLBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "RunAsPPLBoot")],
                DetectOps = [RegOp.CheckDword(LsaKey, "RunAsPPLBoot", 1)],
            },
            new TweakDef
            {
                Id = "lsaharden-disable-anonymous-enumeration-sam",
                Label = "Block Anonymous Enumeration of SAM Accounts",
                Category = "Security — LSA Protection",
                Description =
                    "Prevents anonymous (unauthenticated) network users from enumerating SAM accounts and shared resources. "
                    + "Mitigates information gathering for lateral movement. "
                    + "Default: 0 (anonymous can enumerate). Recommended: 1 (no enumeration).",
                Tags = ["lsa", "sam", "anonymous-enumeration", "hardening", "information-disclosure"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(LsaKey, "RestrictAnonymousSAM", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictAnonymousSAM")],
                DetectOps = [RegOp.CheckDword(LsaKey, "RestrictAnonymousSAM", 1)],
            },
            new TweakDef
            {
                Id = "lsaharden-disable-anonymous-enumeration-shares",
                Label = "Block Anonymous Enumeration of Shares and Accounts",
                Category = "Security — LSA Protection",
                Description =
                    "Restricts anonymous users from enumerating SAM accounts and network shares (RestrictAnonymous=1). "
                    + "Prevents unauthenticated reconnaissance of the system. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["lsa", "anonymous", "shares", "enumeration", "information-disclosure"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(LsaKey, "RestrictAnonymous", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictAnonymous")],
                DetectOps = [RegOp.CheckDword(LsaKey, "RestrictAnonymous", 1)],
            },
            new TweakDef
            {
                Id = "lsaharden-disable-null-session-pipes",
                Label = "Disable Null Session Access to Named Pipes",
                Category = "Security — LSA Protection",
                Description =
                    "Prevents null-session (unauthenticated) connections to named pipes over the network. "
                    + "Closes a lateral movement vector used to access inter-process communication without credentials. "
                    + "Default: some pipes accessible. Recommended: disabled.",
                Tags = ["lsa", "null-session", "named-pipes", "network", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(LsaKey, "DisableRestrictedAdmin", 0)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "DisableRestrictedAdmin")],
                DetectOps = [RegOp.CheckDword(LsaKey, "DisableRestrictedAdmin", 0)],
            },
            new TweakDef
            {
                Id = "lsaharden-ntlm-force-v2",
                Label = "Force NTLMv2 Authentication Only",
                Category = "Security — LSA Protection",
                Description =
                    "Requires LM and NTLM clients to use NTLMv2 authentication and to refuse legacy LM/NTLM responses. "
                    + "LmCompatibilityLevel=5 rejects all LM and NTLMv1 challenges from servers. "
                    + "Default: level 3 on modern Windows. Recommended: level 5.",
                Tags = ["lsa", "ntlm", "ntlmv2", "lm-hash", "authentication", "pass-the-hash"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(LsaKey, "LmCompatibilityLevel", 5)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "LmCompatibilityLevel")],
                DetectOps = [RegOp.CheckDword(LsaKey, "LmCompatibilityLevel", 5)],
            },
            new TweakDef
            {
                Id = "lsaharden-disable-lm-hash-storage",
                Label = "Disable LAN Manager Hash Storage",
                Category = "Security — LSA Protection",
                Description =
                    "Prevents Windows from storing the LAN Manager (LM) password hash in the SAM database and Active Directory. "
                    + "LM hashes are cryptographically weak and trivially cracked; removing them eliminates this attack vector. "
                    + "Default: LM hash may be stored. Recommended: disabled.",
                Tags = ["lsa", "lm-hash", "sam", "password-hash", "credential-cracking"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(LsaNetKey, "NoLMHash", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaNetKey, "NoLMHash")],
                DetectOps = [RegOp.CheckDword(LsaNetKey, "NoLMHash", 1)],
            },
            new TweakDef
            {
                Id = "lsaharden-enable-ntlm-session-security-128",
                Label = "Require 128-bit NTLMv2 Session Security",
                Category = "Security — LSA Protection",
                Description =
                    "Forces NTLMv2 session key negotiation to use 128-bit encryption and message integrity. "
                    + "MinimumSessionSecurity=537395200 (0x20080000) combines both NTLMv2 session and 128-bit keys. "
                    + "Default: no minimum. Recommended: 128-bit enabled.",
                Tags = ["lsa", "ntlm", "128-bit", "session-security", "encryption"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(LsaNetKey, "NTLMMinClientSec", 537395200)],
                RemoveOps = [RegOp.DeleteValue(LsaNetKey, "NTLMMinClientSec")],
                DetectOps = [RegOp.CheckDword(LsaNetKey, "NTLMMinClientSec", 537395200)],
            },
            new TweakDef
            {
                Id = "lsaharden-require-ntlm-server-128",
                Label = "Require 128-bit NTLMv2 Session Security on Servers",
                Category = "Security — LSA Protection",
                Description =
                    "Forces NTLM server-side session negotiation to accept only 128-bit encrypted sessions. "
                    + "Prevents downgrade attacks where a MiTM forces weaker DES-based session keys. "
                    + "Default: no minimum. Recommended: 128-bit enforced.",
                Tags = ["lsa", "ntlm", "server", "session-security", "encryption", "downgrade"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(LsaNetKey, "NTLMMinServerSec", 537395200)],
                RemoveOps = [RegOp.DeleteValue(LsaNetKey, "NTLMMinServerSec")],
                DetectOps = [RegOp.CheckDword(LsaNetKey, "NTLMMinServerSec", 537395200)],
            },
            new TweakDef
            {
                Id = "lsaharden-disable-force-guest",
                Label = "Disable Force Guest Account on Network Logon",
                Category = "Security — LSA Protection",
                Description =
                    "Prevents forcing all network logon attempts to the Guest account. "
                    + "ForceGuest=0 ensures authenticated users get their actual account permissions rather than guest access. "
                    + "Default: variable. Recommended: 0 (disabled).",
                Tags = ["lsa", "guest", "network-logon", "authentication", "access-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(LsaKey, "ForceGuest", 0)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "ForceGuest")],
                DetectOps = [RegOp.CheckDword(LsaKey, "ForceGuest", 0)],
            },
        ];
}
