// RegiLattice.Core — Tweaks/CredentialManagerPolicy.cs
// Sprint 341: Credential Manager Policy tweaks (10 tweaks)
// Category: "Credential Manager Policy" | Slug: credmgr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CredentialManagerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "credmgr-restrict-default-credentials-delegation",
            Label = "Restrict Default Credential Delegation to Specific Servers",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Default credential delegation (Allow Delegating Default Credentials) controls whether Windows automatically delegates user credentials to remote systems when services request them. Restricting default credential delegation to specific trusted servers prevents automatic credential forwarding to untrusted or attacker-controlled systems. Unrestricted credential delegation sends user credentials to any server that requests Kerberos delegation creating a pass-the-hash risk for any compromised server in the path. The delegation allowlist should contain only specific server names or use DNS suffixes for controlled environments not wildcard entries that allow all servers. Credential delegation restrictions prevent watering-hole style attacks where a server is compromised specifically to capture delegated credentials from users who connect to it. Organizations using RDP remote administration should configure specific server name allowlists for credential delegation instead of the insecure default wildcard configuration.",
            Tags = ["credentials", "delegation", "rdp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDefaultCredentials", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDefaultCredentials")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDefaultCredentials", 0)],
        },
        new TweakDef
        {
            Id = "credmgr-restrict-fresh-credentials",
            Label = "Restrict Fresh Credential Delegation to Trusted Servers Only",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Fresh credential delegation controls whether newly entered credentials are forwarded to remote servers as part of the authentication process. Restricting fresh credential delegation prevents forwarding of interactive user credentials to servers outside the trusted delegation list. Fresh credentials include passwords entered in Windows Security prompts and other interactive authentication dialogs that should not be transmitted to untrusted systems. Delegation restrictions for fresh credentials are particularly important for privileged users who enter elevated credentials that could be captured by a compromised remote system. The CredSSP protocol used for fresh credential delegation was vulnerable to a man-in-the-middle attack (CVE-2018-0886) before patches were applied making restriction important. Organizations should configure fresh credential delegation lists to include only servers that absolutely require credential forwarding to reduce exposure.",
            Tags = ["credentials", "fresh-credentials", "credssp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowFreshCredentials", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowFreshCredentials")],
            DetectOps = [RegOp.CheckDword(Key, "AllowFreshCredentials", 0)],
        },
        new TweakDef
        {
            Id = "credmgr-deny-remote-desktop-credential-delegation",
            Label = "Deny Credential Delegation through Remote Desktop without NLA",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Credential delegation through Remote Desktop without Network Level Authentication allows attackers to present a rogue RDP server that captures user credentials before showing any connection UI. Denying credential delegation without NLA ensures that credentials are only forwarded after the server's identity has been verified through NLA mutual authentication. Pre-NLA credential delegation is a classic man-in-the-middle attack vector for RDP where a network attacker can harvest credentials from unpatched or misconfigured RDP clients. NLA requires that the client authenticate to the server before establishing the remote desktop session preventing eavesdropping on the initial credential exchange. All organizations should require NLA for all RDP connections and deny credential delegation to servers that do not support NLA. Remote Desktop Gateway solutions provide NLA enforcement for external RDP access and should be used for all external remote access scenarios.",
            Tags = ["credentials", "rdp", "nla", "man-in-the-middle", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DenyDefaultCredentials", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyDefaultCredentials")],
            DetectOps = [RegOp.CheckDword(Key, "DenyDefaultCredentials", 1)],
        },
        new TweakDef
        {
            Id = "credmgr-restrict-saved-rdp-credentials",
            Label = "Prevent Saving of Remote Desktop Connection Credentials",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Saved Remote Desktop credentials stored in Windows Credential Manager can be extracted by attackers with local access providing access to remote systems without knowing the user's password. Preventing saved RDP credentials reduces the credential material available to attackers who compromise a client system. Saved credentials are particularly risky on shared or kiosk systems where multiple users may access the same device. The Windows Credential Manager store can be dumped by tools like Mimikatz if the attacker has local administrator access making prevention more impactful than protection. Organizations with privileged access workstations (PAWs) should disable saved credentials to ensure administrators must enter passwords each session reinforcing awareness of credential use. Preventing saved credentials may require users to re-authenticate for each connection which is an acceptable security trade-off for privileged users.",
            Tags = ["credentials", "rdp", "saved-passwords", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisallowSavedCredentials", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisallowSavedCredentials")],
            DetectOps = [RegOp.CheckDword(Key, "DisallowSavedCredentials", 1)],
        },
        new TweakDef
        {
            Id = "credmgr-enable-restricted-admin-mode",
            Label = "Enable Restricted Admin Mode for Remote Desktop Connections",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Restricted Admin Mode prevents credential delegation when connecting via RDP by using the connecting computer's credentials rather than the user's credentials on the remote system. Enabling Restricted Admin Mode reduces the credential material exposed to the remote system limiting credential theft from a compromised RDP host. In Restricted Admin Mode administrative access is available but the full user credentials are not sent to the remote server making them unavailable for lateral movement from that server. Organizations should enable Restricted Admin Mode for all privileged RDP connections especially to servers that may be compromised. Restricted Admin Mode has the trade-off that network resources accessed from the remote session use the remote computer's credentials not the user's potentially causing access failures. Combining Restricted Admin Mode with privileged access workstations provides defense-in-depth against credential theft through RDP.",
            Tags = ["credentials", "rdp", "restricted-admin", "pass-the-hash", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictedRemoteAdministration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictedRemoteAdministration")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictedRemoteAdministration", 1)],
        },
        new TweakDef
        {
            Id = "credmgr-disable-wdigest-authentication",
            Label = "Disable WDigest Authentication to Prevent Cleartext Password Storage in Memory",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "WDigest authentication stores user credentials in cleartext in LSASS memory on Windows systems making them accessible to credential dumping tools like Mimikatz. Disabling WDigest authentication prevents cleartext passwords from being stored in memory significantly limiting the impact of credential dumping attacks. WDigest was designed for authentication against HTTP Digest services but is rarely required in modern environments that use NTLM or Kerberos. The UseLogonCredential registry value controls whether WDigest stores cleartext credentials in LSASS and should be set to 0 on all domain-joined systems. Microsoft patched WDigest behavior in KB2871997 and WDigest is disabled by default on Windows 8.1 and Windows Server 2012 R2 but must be explicitly disabled on older versions. Organizations should verify WDigest is disabled across their entire fleet as it is commonly re-enabled by attackers who first compromise a system and want to wait for credentials.",
            Tags = ["credentials", "wdigest", "lsass", "cleartext", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UseLogonCredential", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "UseLogonCredential")],
            DetectOps = [RegOp.CheckDword(Key, "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id = "credmgr-enable-lsass-process-protection",
            Label = "Enable LSASS Process Protection to Prevent Credential Dumping",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "LSASS (Local Security Authority Subsystem Service) process protection uses Windows Protected Process Light to prevent user-mode process injection and memory reading by credential dumping tools. Enabling LSASS protection requires that all LSASS plugins including authentication providers be digitally signed by Microsoft preventing Mimikatz-style injection. Protected LSASS with PPL (Protected Process Light) makes it significantly harder for attackers to extract credential hashes from LSASS memory. LSASS process protection requires Credential Guard or the RunAsPPL registry value to be configured as Protected Process Light. Third-party security software that hooks LSASS for monitoring may break when LSASS protection is enabled requiring vendor-specific signed drivers. Organizations should test LSASS protection in their environment before broad deployment as incompatible software will cause authentication failures.",
            Tags = ["credentials", "lsass", "process-protection", "credential-dumping", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RunAsPPL", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RunAsPPL")],
            DetectOps = [RegOp.CheckDword(Key, "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "credmgr-restrict-credential-manager-api-access",
            Label = "Restrict API Access to Windows Credential Manager Store",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows Credential Manager API access controls which applications can read stored generic credentials from the Windows Credential Manager vault. Restricting Credential Manager API access prevents unauthorized applications from reading network credentials and application passwords stored in the credential vault. The Windows Credential Manager can store passwords for websites, servers, and applications which are accessible to applications running as the current user. Malware that runs as the user can enumerate and exfiltrate credentials from Credential Manager without elevated privileges making API restriction an important defense. Credential Manager access restrictions should be complemented by avoiding storing sensitive passwords in Credential Manager on shared or potentially compromised systems. Security audits should check Credential Manager contents on privileged workstations to ensure sensitive administrative credentials are not stored in plain access.",
            Tags = ["credentials", "credential-manager", "api-access", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictCredentialManagerAPI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictCredentialManagerAPI")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictCredentialManagerAPI", 1)],
        },
        new TweakDef
        {
            Id = "credmgr-enable-remote-host-credential-guard",
            Label = "Enable Remote Credential Guard for Protected Credential Delegation",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Remote Credential Guard redirects Kerberos requests back to the requesting device so that the user's credentials are never actually sent to the remote host preventing credential exposure. Enabling Remote Credential Guard for RDP connections ensures that even if the remote server is compromised the attacker cannot steal credentials from it. Remote Credential Guard differs from Restricted Admin Mode in that the full user token remains on the local machine making network resource access work correctly from the remote session. Remote Credential Guard requires Windows 10 1607 or later on both client and server and the user must have Kerberos authentication to the target server. Organizations should prefer Remote Credential Guard over Restricted Admin Mode when possible as it provides stronger security with better compatibility for network resource access. Remote Credential Guard cannot be used with Network Level Authentication for Remote Desktop Gateway connections.",
            Tags = ["credentials", "remote-credential-guard", "rdp", "kerberos", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRemoteCredentialGuard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteCredentialGuard")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRemoteCredentialGuard", 1)],
        },
        new TweakDef
        {
            Id = "credmgr-block-ntlm-credential-delegation",
            Label = "Block NTLM Credential Delegation in Restricted Networks",
            Category = "Credential Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "NTLM credential delegation passes user NTLM credentials to remote systems where they can be captured and used for pass-the-hash attacks against other systems. Blocking NTLM credential delegation prevents NTLM hashes from being forwarded to servers that could be compromised and use them for lateral movement. NTLM is a challenge-response authentication protocol where the hash sent to authenticate can be immediately reused without knowing the original password. Organizations should be moving toward Kerberos authentication everywhere to eliminate NTLM delegation risks but restrictions help harden transitional environments. NTLM relay attacks are a persistent threat where an attacker on the network captures and relays NTLM credentials to authenticate to other services. Blocking NTLM delegation combined with SMB signing and NTLM auditing provides comprehensive protection against NTLM-based attacks.",
            Tags = ["credentials", "ntlm", "delegation", "pass-the-hash", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNTLMDelegation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNTLMDelegation")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNTLMDelegation", 1)],
        },
    ];
}
