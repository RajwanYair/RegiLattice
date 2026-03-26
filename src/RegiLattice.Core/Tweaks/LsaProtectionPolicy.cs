// RegiLattice.Core — Tweaks/LsaProtectionPolicy.cs
// Local Security Authority (LSA) process protection Group Policy controls — Sprint 377.
// Category: "LSA Protection Policy" | Slug: lsapol
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation
//                 HKLM\SYSTEM\CurrentControlSet\Control\Lsa (LSA protection)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LsaProtectionPolicy
{
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";
    private const string LsaSysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsNT\CurrentVersion\Winlogon";
    private const string LsaCtrl = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\LSA";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "lsapol-enable-lsa-run-as-ppl",
                Label = "Enable LSA Run as Protected Process Light (PPL)",
                Category = "LSA Protection Policy",
                Description =
                    "Configures lsass.exe to run as a Protected Process Light (PPL). PPL enforces ELAM (Early Launch Anti-Malware) restrictions: only Microsoft-signed binaries can inject into or read lsass memory. Directly prevents Mimikatz credential dumping.",
                Tags = ["lsa", "ppl", "credential-dump", "mimikatz", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Most impactful anti-credential-theft control available via policy. Prevents all unsigned in-memory credential extraction tools. Requires Windows 8.1+ and may conflict with unsigned AV products.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "RunAsPPL", 2)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RunAsPPL")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "RunAsPPL", 2)],
            },
            new TweakDef
            {
                Id = "lsapol-audit-lsass-access-attempts",
                Label = "Audit LSASS Memory Access Attempts",
                Category = "LSA Protection Policy",
                Description =
                    "Enables audit logging of all OpenProcess calls that attempt to read lsass.exe memory. Even without PPL enforcement this detects credential-dumping tools (Mimikatz, ProcDump /ma) and logs the calling process for SIEM analysis.",
                Tags = ["lsa", "audit", "credential-dump", "memory-access", "event-log"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Non-disruptive; adds event-log entries only. Essential for detection of credential theft attempts even before PPL is enforced.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "AuditLSASSAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "AuditLSASSAccess")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "AuditLSASSAccess", 1)],
            },
            new TweakDef
            {
                Id = "lsapol-disable-reversible-encryption",
                Label = "Disable Reversible Password Encryption in LSA",
                Category = "LSA Protection Policy",
                Description =
                    "Prevents Windows from storing user passwords in LSA using reversible encryption. Reversible password storage is equivalent to plaintext storage; disabling it ensures only one-way NTLM hashes are retained in the SAM database.",
                Tags = ["lsa", "password-storage", "reversible-encryption", "sam", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Prevents reversible-cleartext password storage in SAM. Users who had reversible passwords must reset after this is applied. No operational impact if reversible encryption was never enabled.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "DisableReversibleEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "DisableReversibleEncryption")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "DisableReversibleEncryption", 1)],
            },
            new TweakDef
            {
                Id = "lsapol-block-credential-delegation-to-unknown",
                Label = "Block Credential Delegation to Unknown or Untrusted Servers",
                Category = "LSA Protection Policy",
                Description =
                    "Denies credential delegation (CredSSP / Kerberos constrained delegation) to servers not explicitly listed in the trusted servers allowlist. Prevents pass-the-hash relay attacks that trick the client into delegating credentials to a rogue server.",
                Tags = ["lsa", "credential-delegation", "credssp", "relay", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Blocks credential delegation to all servers not explicitly allowlisted. Remote Desktop connections to unlisted servers will prompt for credentials rather than passing them. Maintain the delegation allowlist in GPO.",
                RegistryKeys = [LsaKey],
                ApplyOps = [RegOp.SetDword(LsaKey, "AllowDefCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "AllowDefCredentials")],
                DetectOps = [RegOp.CheckDword(LsaKey, "AllowDefCredentials", 0)],
            },
            new TweakDef
            {
                Id = "lsapol-restrict-anonymous-lsa-access",
                Label = "Restrict Anonymous LSA Name and Account Lookups",
                Category = "LSA Protection Policy",
                Description =
                    "Prevents anonymous connections (null sessions) from enumerating LSA account names, SIDs, and local group memberships. Blocks the reconnaissance phase of account enumeration attacks.",
                Tags = ["lsa", "anonymous-access", "null-session", "enumeration", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Prevents unauthenticated users from querying account info via null sessions. Blocks legacy management tools and scanners that rely on anonymous LSA queries.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "RestrictAnonymous", 2)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RestrictAnonymous")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "RestrictAnonymous", 2)],
            },
            new TweakDef
            {
                Id = "lsapol-disable-lm-hash-storage",
                Label = "Disable LM Hash Storage in LSA Credential Store",
                Category = "LSA Protection Policy",
                Description =
                    "Permanently disables storage of LAN Manager (LM) password hashes in the LSA credential cache. LM hashes are solvable in seconds with modern GPUs; this ensures only NTLM and Kerberos hashes are retained.",
                Tags = ["lsa", "lm-hash", "ntlm", "password-hash", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "All future password changes stop producing LM hashes. Existing hashes remain until users change passwords. Eliminates the weakest credential artifact from the credential store.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "NoLmHash", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "NoLmHash")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "NoLmHash", 1)],
            },
            new TweakDef
            {
                Id = "lsapol-enable-securechannel-sealing",
                Label = "Require Secure Channel Data Encryption and Signing",
                Category = "LSA Protection Policy",
                Description =
                    "Forces all Netlogon secure channel traffic to be encrypted and signed. A secure channel is the authenticated tunnel between a domain member and its DC; unsigned channels can be hijacked to inject forged authentication responses.",
                Tags = ["lsa", "netlogon", "secure-channel", "encryption", "signing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Ensures Netlogon/DC communication is tamper-proof. Old DCs that do not support signed secure channels will refuse connections; requires Server 2012+ DCs.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "RequireSignOrSeal", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RequireSignOrSeal")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id = "lsapol-restrict-cached-logons",
                Label = "Restrict Cached Domain Logon Count to 1",
                Category = "LSA Protection Policy",
                Description =
                    "Limits the number of cached domain credential sets stored in the LSA to 1 (minimum). Cached logons allow domain users to authenticate offline; a high cache count means a physical attacker can harvest multiple domain hashes from a stolen laptop.",
                Tags = ["lsa", "cached-logon", "credential-cache", "physical-security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Reduces cached credential exposure from offline attacks. Setting to 1 means only the most recent login is cached; users who have not logged in recently cannot authenticate offline.",
                RegistryKeys = [LsaSysKey],
                ApplyOps = [RegOp.SetString(LsaSysKey, "CachedLogonsCount", "1")],
                RemoveOps = [RegOp.DeleteValue(LsaSysKey, "CachedLogonsCount")],
                DetectOps = [RegOp.CheckString(LsaSysKey, "CachedLogonsCount", "1")],
            },
            new TweakDef
            {
                Id = "lsapol-enable-credential-guard",
                Label = "Enable Windows Defender Credential Guard (Isolated LSA)",
                Category = "LSA Protection Policy",
                Description =
                    "Enables Credential Guard, which runs the LSA credential store inside a secure Hyper-V isolated container (VSM). Even if the host OS is fully compromised lsass cannot be dumped because credentials live in a separate VM-protected memory region.",
                Tags = ["lsa", "credential-guard", "vsm", "secure-enclave", "advanced"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Most powerful LSA credential protection; renders Mimikatz-class attacks ineffective. Requires UEFI Secure Boot + VBS + HVCI. Check hardware compatibility before deploying.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "lsapol-block-wdigest-plaintext-creds",
                Label = "Block WDigest from Storing Plaintext Credentials in LSASS",
                Category = "LSA Protection Policy",
                Description =
                    "Disables WDigest authentication protocol caching in LSASS memory. WDigest was designed for HTTP Digest authentication and cached plaintext-equivalent credentials in LSASS; attackers (Mimikatz sekurlsa::wdigest) can extract these.",
                Tags = ["lsa", "wdigest", "plaintext", "credential-cache", "mimikatz"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Eliminates plaintext credential storage in LSASS with zero functional impact for modern Windows. WDigest is only needed by systems running very old IIS Digest authentication — rare in practice.",
                RegistryKeys = [LsaCtrl],
                ApplyOps = [RegOp.SetDword(LsaCtrl, "UseLogonCredential", 0)],
                RemoveOps = [RegOp.DeleteValue(LsaCtrl, "UseLogonCredential")],
                DetectOps = [RegOp.CheckDword(LsaCtrl, "UseLogonCredential", 0)],
            },
        ];
}
