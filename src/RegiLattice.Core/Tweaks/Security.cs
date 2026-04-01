namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Security
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-restrict-anonymous-enum",
            Label = "Restrict Anonymous SAM Enumeration",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents anonymous users from enumerating SAM accounts and shares. Hardens against network reconnaissance attacks.",
            Tags = ["security", "sam", "anonymous", "enumeration", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousSAM", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousSAM", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousSAM", 1),
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-enable-dep-always",
            Label = "Enable DEP (Always On)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Data Execution Prevention to AlwaysOn mode via registry flag. Prevents code execution from non-executable memory pages system-wide.",
            Tags = ["security", "dep", "memory", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "MoveImages", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "MoveImages")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "MoveImages", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-safe-dll-search",
            Label = "Enable Safe DLL Search Mode",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures Windows searches the System directory before the current directory when loading DLLs. Prevents DLL hijacking attacks.",
            Tags = ["security", "dll-hijacking", "hardening", "search-order"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 1)],
        },
        new TweakDef
        {
            Id = "sec-reduce-cached-logons",
            Label = "Reduce Cached Logon Count to 1",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Reduces the number of cached domain logon credentials to 1 (default: 10). Minimises credential theft risk if the machine is compromised.",
            Tags = ["security", "cached-logons", "credentials", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "CachedLogonsCount", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "CachedLogonsCount", "10")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "CachedLogonsCount", "1")],
        },
        new TweakDef
        {
            Id = "sec-restrict-sam-remote",
            Label = "Restrict Remote SAM Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts network access to the SAM database to Administrators only via SDDL. Blocks remote enumeration of local users and groups.",
            Tags = ["security", "sam", "remote-access", "hardening", "sddl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)"),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-wpad",
            Label = "Disable WPAD (Web Proxy Auto-Discovery)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables WPAD proxy auto-detection, preventing man-in-the-middle attacks via rogue WPAD servers on corporate or public networks.",
            Tags = ["security", "wpad", "proxy", "network", "mitm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "sec-enforce-smb-signing",
            Label = "Enforce SMB Packet Signing",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires SMB packet signing on the client side. Prevents SMB relay attacks and ensures data integrity for file sharing.",
            Tags = ["security", "smb", "signing", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-powershell-v2",
            Label = "Disable PowerShell v2 Engine (Policy)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the legacy PowerShell v2 engine via policy key. PS v2 bypasses modern logging and AMSI, making it a popular attack vector.",
            Tags = ["security", "powershell", "amsi", "hardening", "downgrade-attack"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell", "EnableScripts", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell", "EnableScripts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell", "EnableScripts", 0)],
        },
        new TweakDef
        {
            Id = "sec-enforce-lsa-ppl",
            Label = "Enforce LSA Protected Process Light",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables LSA protection to run LSASS as a Protected Process Light. Prevents credential dumping tools like Mimikatz from reading memory.",
            Tags = ["security", "lsa", "credential-protection", "hardening", "mimikatz"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "sec-block-wdigest-caching",
            Label = "Block WDigest Credential Caching",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables WDigest authentication which stores plaintext credentials in LSASS memory. Critical hardening against credential theft.",
            Tags = ["security", "wdigest", "credentials", "hardening", "lsass"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id = "sec-enforce-nla",
            Label = "Enforce Network Level Authentication for RDP",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires Network Level Authentication (NLA) before establishing an RDP session. Prevents unauthenticated access to the login screen.",
            Tags = ["security", "rdp", "nla", "hardening", "remote-desktop"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-lm-hash",
            Label = "Disable LM Hash Storage",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from storing LAN Manager (LM) hash values for passwords. LM hashes are trivially crackable.",
            Tags = ["security", "password", "lm-hash", "hardening", "ntlm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoLMHash", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoLMHash", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoLMHash", 1)],
        },
        new TweakDef
        {
            Id = "sec-enforce-sehop",
            Label = "Enforce SEHOP (SEH Chain Validation)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Structured Exception Handler Overwrite Protection. Prevents exploit techniques that abuse SEH chains.",
            Tags = ["security", "sehop", "exploit-protection", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0),
            ],
        },
        new TweakDef
        {
            Id = "sec-force-strong-key-protection",
            Label = "Force Strong Key Protection for Certificates",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires a strong password or hardware token for private key access when using certificate-based authentication.",
            Tags = ["security", "certificates", "key-protection", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography", "ForceKeyProtection", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography", "ForceKeyProtection")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography", "ForceKeyProtection", 2)],
        },
        new TweakDef
        {
            Id = "sec-restrict-null-session-pipes",
            Label = "Restrict Anonymous Named Pipe Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes all null session pipes, preventing anonymous access to named pipes for network reconnaissance.",
            Tags = ["security", "named-pipes", "anonymous", "hardening", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetMultiSz(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "NullSessionPipes", [])],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "NullSessionPipes")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "NullSessionPipes", "")],
        },
        new TweakDef
        {
            Id = "sec-set-ntlmv2-only",
            Label = "Enforce NTLMv2 Only (Disable LMv1/NTLM)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets LAN Manager authentication to NTLMv2 only, refusing weaker LM and NTLMv1 responses. Critical for NTLM relay attack prevention.",
            Tags = ["security", "ntlm", "authentication", "hardening", "relay-attack"],
            SideEffects = "Very old network clients may fail to authenticate.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "sec-enable-credential-guard-vbs",
            Label = "Enable Credential Guard",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Credential Guard to protect NTLM hashes and Kerberos tickets using virtualization-based security. Prevents pass-the-hash attacks.",
            Tags = ["security", "credential-guard", "vbs", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-enforce-audit-policy",
            Label = "Enforce Advanced Audit Policy",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces use of advanced audit policy configuration over legacy audit policies. Ensures granular logging control.",
            Tags = ["security", "audit", "logging", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "SCENoApplyLegacyAuditPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "SCENoApplyLegacyAuditPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "SCENoApplyLegacyAuditPolicy", 1)],
        },
        new TweakDef
        {
            Id = "sec-block-unsigned-drivers",
            Label = "Block Unsigned Driver Installation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks installation of unsigned kernel-mode drivers. Strengthens driver signing enforcement beyond default.",
            Tags = ["security", "drivers", "signing", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Driver Signing"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Driver Signing", "Policy", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Driver Signing", "Policy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Driver Signing", "Policy", 2)],
        },
        new TweakDef
        {
            Id = "sec-disable-ip-source-routing",
            Label = "Disable IP Source Routing",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables IP source routing which allows packets to specify their own route. Prevents source routing-based attacks.",
            Tags = ["security", "network", "source-routing", "ip"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableIPSourceRouting", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableIPSourceRouting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableIPSourceRouting", 2)],
        },
        new TweakDef
        {
            Id = "sec-enable-icmp-redirect-disable",
            Label = "Disable ICMP Redirects",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables processing of ICMP redirect messages. Prevents MITM attacks that could redirect network traffic.",
            Tags = ["security", "network", "icmp", "redirect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableICMPRedirect", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableICMPRedirect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableICMPRedirect", 0)],
        },
        new TweakDef
        {
            Id = "sec-enforce-smb-encryption",
            Label = "Enforce SMB Encryption",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Requires encryption for all SMB 3.0+ connections. Prevents eavesdropping on file share traffic.",
            Tags = ["security", "smb", "encryption", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EncryptData", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EncryptData")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EncryptData", 1)],
        },
        new TweakDef
        {
            Id = "sec-restrict-anonymous-access-shares",
            Label = "Restrict Anonymous Access to Named Pipes and Shares",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts anonymous access to named pipes and shares. Prevents null session enumeration of shared resources.",
            Tags = ["security", "anonymous", "shares", "named-pipes"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-default-admin-shares",
            Label = "Disable Administrative Shares",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic creation of administrative shares (C$, D$, ADMIN$). Prevents lateral movement in compromised networks.",
            Tags = ["security", "admin-shares", "lateral-movement", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-safe-search-mode",
            Label = "Enable Safe DLL Search Mode",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures the system directory is searched before the current directory for DLLs. Mitigates DLL hijacking attacks.",
            Tags = ["security", "dll", "hijacking", "search-order"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 1)],
        },
        new TweakDef
        {
            Id = "sec-require-ldap-signing",
            Label = "Require LDAP Client Signing",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires LDAP clients to request packet signing. Value 2 = require signing. Prevents LDAP relay attacks where a man-in-the-middle could intercept authentication.",
            Tags = ["security", "ldap", "signing", "active-directory", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap", "LDAPClientIntegrity", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap", "LDAPClientIntegrity")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap", "LDAPClientIntegrity", 2)],
        },
        new TweakDef
        {
            Id = "sec-disable-rdp-drive-mapping",
            Label = "Disable RDP Drive Redirection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents local drives from being mapped and accessible in Remote Desktop sessions, preventing file transfer through RDP drive redirection.",
            Tags = ["security", "rdp", "drive", "redirection", "data-loss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm", 1)],
        },
        new TweakDef
        {
            Id = "sec-enforce-smb-ntlmv2-auth",
            Label = "Enforce NTLMv2 Only for SMB Authentication",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the SMB server to accept only NTLMv2 challenge/response authentication, blocking downgrade to LAN Manager or NTLMv1 authentication.",
            Tags = ["security", "smb", "ntlmv2", "authentication", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-printer-spooler-network",
            Label = "Disable Print Spooler Remote Network Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts the Print Spooler from accepting remote print connections, mitigating PrintNightmare-style vulnerabilities (CVE-2021-34527 class). Local printing still works.",
            Tags = ["security", "print-spooler", "printnightmare", "vulnerability", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2),
            ],
        },
        new TweakDef
        {
            Id = "sec-enable-run-as-different-user",
            Label = "Enable Run As Different User in Explorer",
            Category = "Security",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Restores the 'Run as different user' option in Windows Explorer context menus, enabling least-privilege execution for administrative tasks without logging off.",
            Tags = ["security", "run-as", "privilege", "admin", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "ShowRunAsDifferentUserInStart", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "ShowRunAsDifferentUserInStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "ShowRunAsDifferentUserInStart", 1)],
        },
        new TweakDef
        {
            Id = "sec-disable-office-macros-internet",
            Label = "Block Office Macros from Internet Sources",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Office applications from running macros in files downloaded from the internet, closing a major malware delivery vector (macro-based attacks).",
            Tags = ["security", "office", "macros", "malware", "internet", "phishing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Word\Security"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Word\Security", "blockcontentexecutionfrominternet", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Excel\Security", "blockcontentexecutionfrominternet", 1),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\PowerPoint\Security",
                    "blockcontentexecutionfrominternet",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Word\Security", "blockcontentexecutionfrominternet"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Excel\Security", "blockcontentexecutionfrominternet"),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\PowerPoint\Security",
                    "blockcontentexecutionfrominternet"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Word\Security", "blockcontentexecutionfrominternet", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-wsh-scripting",
            Label = "Disable Windows Script Host",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows Script Host (WSH) to prevent execution of .vbs, .js, and .wsf scripts system-wide. Blocks a common malware delivery method.",
            Tags = ["security", "wsh", "vbscript", "jscript", "scripting", "malware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Script Host\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Script Host\Settings", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Script Host\Settings", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Script Host\Settings", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sec-restrict-lsass-credential-dump",
            Label = "Add LSA Additional PPL Run-as-Light Protection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures LSA to run as a Protected Process Light (PPL) supplementary policy, making it significantly harder for credential dumping tools (e.g. Mimikatz) to extract passwords.",
            Tags = ["security", "lsa", "ppl", "credential", "dump", "mimikatz", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\lsass.exe"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\lsass.exe",
                    "AuditLevel",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\lsass.exe",
                    "AuditLevel"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\lsass.exe",
                    "AuditLevel",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-named-pipe-impersonation",
            Label = "Restrict Named Pipe Impersonation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts anonymous access to named pipes and shares, preventing token impersonation attacks via named pipes by unauthenticated processes.",
            Tags = ["security", "named-pipe", "impersonation", "anonymous", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "NullSessionPipes", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "NullSessionPipes")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "NullSessionPipes", 0)],
        },
    ];
}

// ── Kiosk & Shared PC ─────────────────────────────────────────────────────────
// Merged from KioskSharedPc.cs (kiosk mode and shared PC configuration)

internal static class KioskSharedPc
{
    private const string SharedPc = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SharedPC";
    private const string WinSysPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string LockPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
    private const string LogonPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kiosk-enable-shared-pc-mode",
            Label = "Enable Shared PC Mode",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableSharedPCMode = 1 in the SharedPC registry key. Activates Windows Shared PC mode, "
                + "which auto-manages accounts, disk, and sign-in for multi-user shared devices such as school "
                + "or library computers. Default: 0 (disabled).",
            Tags = ["kiosk", "shared-pc", "education", "public", "multi-user"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "EnableSharedPCMode", 1)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "EnableSharedPCMode")],
            DetectOps = [RegOp.CheckDword(SharedPc, "EnableSharedPCMode", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-account-model-guest",
            Label = "Use Guest-Only Account Model for Shared PC",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AccountModel = 0 (guest only) in the SharedPC key. In guest mode, users sign in with "
                + "a temporary guest account that is deleted on sign-out, ensuring no profile data persists. Default: 0.",
            Tags = ["kiosk", "shared-pc", "guest", "account", "privacy"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "AccountModel", 0)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "AccountModel")],
            DetectOps = [RegOp.CheckDword(SharedPc, "AccountModel", 0)],
        },
        new TweakDef
        {
            Id = "kiosk-delete-on-signout",
            Label = "Delete Guest Profiles on Sign-Out",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DeletionPolicy = 1 (delete immediately on sign-out) in the SharedPC key. "
                + "Guest profiles are removed as soon as the user signs out, keeping disk clear on shared devices.",
            Tags = ["kiosk", "shared-pc", "profile", "cleanup", "privacy"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "DeletionPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "DeletionPolicy")],
            DetectOps = [RegOp.CheckDword(SharedPc, "DeletionPolicy", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disk-level-deletion-25",
            Label = "Auto-Delete Profiles at 25% Free Disk",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DiskLevelDeletion = 25 in the SharedPC key. When free disk falls below 25% of total disk, "
                + "SharedPC policy begins deleting the oldest cached accounts, reclaiming space automatically.",
            Tags = ["kiosk", "shared-pc", "disk", "cleanup", "automatic"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "DiskLevelDeletion", 25)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "DiskLevelDeletion")],
            DetectOps = [RegOp.CheckDword(SharedPc, "DiskLevelDeletion", 25)],
        },
        new TweakDef
        {
            Id = "kiosk-disk-level-caching-50",
            Label = "Stop Caching New Profiles at 50% Free Disk",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DiskLevelCaching = 50 in the SharedPC key. When free disk drops below 50%, Shared PC mode "
                + "stops caching new user profiles to prevent the drive from filling up.",
            Tags = ["kiosk", "shared-pc", "disk", "caching", "profile"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "DiskLevelCaching", 50)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "DiskLevelCaching")],
            DetectOps = [RegOp.CheckDword(SharedPc, "DiskLevelCaching", 50)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HideFastUserSwitching = 1 via Windows System policy. Removes the user account switcher "
                + "button from the lock screen and Start menu. Useful for kiosk or single-user session scenarios. Default: 0.",
            Tags = ["kiosk", "shared-pc", "user-switching", "lock-screen", "policy"],
            RegistryKeys = [WinSysPol],
            ApplyOps = [RegOp.SetDword(WinSysPol, "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSysPol, "HideFastUserSwitching")],
            DetectOps = [RegOp.CheckDword(WinSysPol, "HideFastUserSwitching", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-no-local-password-reset",
            Label = "Block Local Password Reset from Lock Screen",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePasswordReveal = 1 via Windows System policy. Prevents users on lock screen "
                + "from clicking 'Forgot my PIN / password' to initiate a self-service reset, important for "
                + "kiosk machines using fixed managed accounts.",
            Tags = ["kiosk", "password", "lock-screen", "reset", "policy"],
            RegistryKeys = [WinSysPol],
            ApplyOps = [RegOp.SetDword(WinSysPol, "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSysPol, "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(WinSysPol, "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-enable-edu-policies",
            Label = "Apply Education / Shared PC Baseline Policies",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SetEduPolicies = 1 in the SharedPC key. Enables the full set of Education-mode policies: "
                + "Start menu simplification, sign-in type restriction, and content filtering baselines "
                + "recommended for classroom and lab deployments.",
            Tags = ["kiosk", "shared-pc", "education", "policy", "classroom"],
            RegistryKeys = [SharedPc],
            ApplyOps = [RegOp.SetDword(SharedPc, "SetEduPolicies", 1)],
            RemoveOps = [RegOp.DeleteValue(SharedPc, "SetEduPolicies")],
            DetectOps = [RegOp.CheckDword(SharedPc, "SetEduPolicies", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-lock-screen-camera",
            Label = "Disable Camera Access on Lock Screen",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLockScreenCamera = 1 via the Personalization policy key. Prevents apps and the system "
                + "from activating the camera while the device is locked. Reduces physical surveillance risk "
                + "in public kiosk settings. Default: 0 (camera may be used on lock screen).",
            Tags = ["kiosk", "camera", "lock-screen", "privacy", "policy"],
            RegistryKeys = [LockPol],
            ApplyOps = [RegOp.SetDword(LockPol, "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(LockPol, "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(LockPol, "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-lock-screen-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLockScreenSlideshow = 1 via the Personalization policy key. Stops the lock screen "
                + "from cycling through user photos or Spotlight images, ensuring a static and controlled "
                + "appearance on kiosk or shared devices. Default: 0.",
            Tags = ["kiosk", "lock-screen", "slideshow", "appearance", "policy"],
            RegistryKeys = [LockPol],
            ApplyOps = [RegOp.SetDword(LockPol, "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(LockPol, "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(LockPol, "NoLockScreenSlideshow", 1)],
        },
    ];
}

// ── Active Directory ──────────────────────────────────────────────────────────
// Merged from ActiveDirectory.cs (AD domain membership hardening, Netlogon, Kerberos)

internal static class ActiveDirectory
{
    private const string Netlogon = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
    private const string KerbParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";
    private const string KerbPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Kerberos\Parameters";
    private const string AdWinSysPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ad-enable-machine-password-change",
            Label = "Ensure Machine Account Password Changes Are Enabled",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePasswordChange = 0 in Netlogon\\Parameters. Explicitly ensures that the Netlogon "
                + "service does NOT disable automatic domain machine account password rotation. Some misguided "
                + "hardening scripts set this to 1, which prevents the machine credential from ever rotating and "
                + "leaves a permanent compromisable static password in place.",
            Tags = ["ad", "netlogon", "machine-account", "password", "domain"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "DisablePasswordChange", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "DisablePasswordChange")],
            DetectOps = [RegOp.CheckDword(Netlogon, "DisablePasswordChange", 0)],
        },
        new TweakDef
        {
            Id = "ad-kerberos-max-token-size",
            Label = "Set Kerberos Maximum Token Size (65535 bytes)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxTokenSize = 65535 in SYSTEM\\Control\\Lsa\\Kerberos\\Parameters. "
                + "Raises the Kerberos token buffer to 65535 bytes (from the default 12000 bytes). "
                + "Required on machines where users belong to many AD groups; prevents 'HTTP 400 header too large' "
                + "and Kerberos authentication failures caused by oversized PAC tokens.",
            Tags = ["ad", "kerberos", "token", "authentication", "groups"],
            RegistryKeys = [KerbParams],
            ApplyOps = [RegOp.SetDword(KerbParams, "MaxTokenSize", 65535)],
            RemoveOps = [RegOp.DeleteValue(KerbParams, "MaxTokenSize")],
            DetectOps = [RegOp.CheckDword(KerbParams, "MaxTokenSize", 65535)],
        },
        new TweakDef
        {
            Id = "ad-no-negative-cache-period",
            Label = "Disable Domain-Controller Negative Cache",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NegativeCachePeriod = 0 in Netlogon\\Parameters. Disables the negative cache that causes "
                + "authentication failures to be remembered for a period without retrying the DC. Prevents stale "
                + "DC failure records from blocking valid logins in environments with intermittent DC connectivity.",
            Tags = ["ad", "netlogon", "dc", "cache", "authentication"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "NegativeCachePeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "NegativeCachePeriod")],
            DetectOps = [RegOp.CheckDword(Netlogon, "NegativeCachePeriod", 0)],
        },
        new TweakDef
        {
            Id = "ad-netlogon-scavenge-interval",
            Label = "Set Netlogon SRV Record Scavenge Interval (5 Minutes)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ScavengeInterval = 300 seconds in Netlogon\\Parameters. Controls how often Netlogon "
                + "rechecks stale DNS SRV records for domain controllers. 300 seconds ensures fresh DC "
                + "data after a failover or site rebalance, reducing the duration of DC discovery failures. Default: 300.",
            Tags = ["ad", "netlogon", "dns", "dc-failover", "scavenge"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "ScavengeInterval", 300)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "ScavengeInterval")],
            DetectOps = [RegOp.CheckDword(Netlogon, "ScavengeInterval", 300)],
        },
        new TweakDef
        {
            Id = "ad-no-nt4-crypto",
            Label = "Disallow NT4-Era Legacy Secure Channel Cryptography",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowNT4Crypto = 0 in Netlogon\\Parameters. Prevents Netlogon from falling back to "
                + "obsolete NT4 cryptographic algorithms on the secure channel when negotiating with older DCs. "
                + "All current domain controllers (Server 2008+) support modern Netlogon crypto. Default: 0.",
            Tags = ["ad", "netlogon", "crypto", "nt4", "secure-channel", "hardening"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "AllowNT4Crypto", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "AllowNT4Crypto")],
            DetectOps = [RegOp.CheckDword(Netlogon, "AllowNT4Crypto", 0)],
        },
        new TweakDef
        {
            Id = "ad-kerberos-aes-encryption",
            Label = "Require AES Kerberos Encryption (Disable RC4/DES)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SupportedEncryptionTypes = 2147483640 in SYSTEM\\Control\\Lsa\\Kerberos\\Parameters. "
                + "Enables all AES (AES-128-CTS-HMAC-SHA1-96, AES-256-CTS-HMAC-SHA1-96) and disables RC4-HMAC, "
                + "DES-CBC-MD5, and DES-CBC-CRC. Kerberos RC4 is vulnerable to AS-REP roasting and pass-the-hash. "
                + "Requires all DCs and services to support AES (Server 2008+).",
            Tags = ["ad", "kerberos", "aes", "rc4", "encryption", "hardening"],
            RegistryKeys = [KerbParams],
            ApplyOps = [RegOp.SetDword(KerbParams, "SupportedEncryptionTypes", 2147483640)],
            RemoveOps = [RegOp.DeleteValue(KerbParams, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KerbParams, "SupportedEncryptionTypes", 2147483640)],
        },
        new TweakDef
        {
            Id = "ad-kerberos-armoring-fast",
            Label = "Enable Kerberos FAST Armoring (Claim-based)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFAST = 1 in SOFTWARE\\Policies\\Microsoft\\Kerberos\\Parameters. Enables "
                + "Kerberos Flexible Authentication Secure Tunneling (FAST / RFC 6113). FAST wraps "
                + "authentication exchanges in an encrypted tunnel, protecting pre-authentication data from "
                + "offline password attacks. Requires Server 2012+ DCs.",
            Tags = ["ad", "kerberos", "fast", "armoring", "authentication", "hardening"],
            RegistryKeys = [KerbPol],
            ApplyOps = [RegOp.SetDword(KerbPol, "EnableFAST", 1)],
            RemoveOps = [RegOp.DeleteValue(KerbPol, "EnableFAST")],
            DetectOps = [RegOp.CheckDword(KerbPol, "EnableFAST", 1)],
        },
        new TweakDef
        {
            Id = "ad-no-mailslot-discovery",
            Label = "Disable Netlogon Mailslot DC Discovery",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MailslotDiscovery = 0 in Netlogon\\Parameters. Forces Netlogon to use DNS-based SRV "
                + "record lookups for domain controller discovery instead of legacy NetBIOS mailslot broadcasts. "
                + "Eliminates unnecessary broadcast traffic and reduces NetBIOS attack surface. Default: 1 (mailslot enabled).",
            Tags = ["ad", "netlogon", "dc-discovery", "mailslot", "netbios", "network"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "MailslotDiscovery", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "MailslotDiscovery")],
            DetectOps = [RegOp.CheckDword(Netlogon, "MailslotDiscovery", 0)],
        },
        new TweakDef
        {
            Id = "ad-no-single-label-dns",
            Label = "Disable Single-Label DNS Domain DC Discovery",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowSingleLabelDnsDomain = 0 in Netlogon\\Parameters. Prevents the Netlogon service "
                + "from querying DNS for domain controllers using bare single-label hostnames (e.g. 'corp' rather "
                + "than 'corp.example.com'). Single-label DNS queries can be intercepted or resolved to rogue hosts. Default: not set.",
            Tags = ["ad", "netlogon", "dns", "single-label", "security", "domain"],
            RegistryKeys = [Netlogon],
            ApplyOps = [RegOp.SetDword(Netlogon, "AllowSingleLabelDnsDomain", 0)],
            RemoveOps = [RegOp.DeleteValue(Netlogon, "AllowSingleLabelDnsDomain")],
            DetectOps = [RegOp.CheckDword(Netlogon, "AllowSingleLabelDnsDomain", 0)],
        },
        new TweakDef
        {
            Id = "ad-no-enumerate-connected-users",
            Label = "Hide Connected/Domain Users on Sign-In Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontEnumerateConnectedUsers = 1 via Windows System policy. Prevents the sign-in screen "
                + "from enumerating and displaying accounts of users currently connected over RDP or other "
                + "remote sessions, reducing information disclosure to local attackers or screen-watchers.",
            Tags = ["ad", "logon", "enumeration", "privacy", "policy", "domain"],
            RegistryKeys = [AdWinSysPol],
            ApplyOps = [RegOp.SetDword(AdWinSysPol, "DontEnumerateConnectedUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(AdWinSysPol, "DontEnumerateConnectedUsers")],
            DetectOps = [RegOp.CheckDword(AdWinSysPol, "DontEnumerateConnectedUsers", 1)],
        },
    ];
}

// ── merged from Defender.cs ────────────────────────────────────────
internal static class Defender
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-enable-pua-protection",
            Label = "Enable PUA / Adware Protection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Potentially Unwanted Application (PUA) detection in Windows Defender.",
            Tags = ["defender", "security", "adware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1)],
        },
        new TweakDef
        {
            Id = "sec-harden-smartscreen",
            Label = "Harden SmartScreen (Warn + Block)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets SmartScreen to warn and block unrecognized apps and downloads.",
            Tags = ["smartscreen", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 2),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "ShellSmartScreenLevel", "Block"),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 1),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "ShellSmartScreenLevel"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 2)],
        },
        new TweakDef
        {
            Id = "sec-disable-exploit-telemetry",
            Label = "Disable Exploit Protection Telemetry",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables audit telemetry from Windows exploit mitigations.",
            Tags = ["security", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationAuditOptions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationAuditOptions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationAuditOptions", 0),
            ],
        },
        new TweakDef
        {
            Id = "sec-defender-scan-cpu-limit",
            Label = "Limit Defender Scan CPU to 25%",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits Windows Defender scheduled-scan CPU usage to 25% to reduce impact during scans.",
            Tags = ["defender", "performance", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 50)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25)],
        },
        new TweakDef
        {
            Id = "sec-enable-controlled-folder-access",
            Label = "Enable Controlled Folder Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Controlled Folder Access (ransomware protection) which blocks unauthorized changes to protected folders.",
            Tags = ["defender", "ransomware", "security"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access",
                    "EnableControlledFolderAccess",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access",
                    "EnableControlledFolderAccess",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access",
                    "EnableControlledFolderAccess",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-enable-network-protection",
            Label = "Enable Network Protection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Defender Network Protection to block connections to malicious domains and IP addresses.",
            Tags = ["defender", "network", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection",
                    "EnableNetworkProtection",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection",
                    "EnableNetworkProtection",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection",
                    "EnableNetworkProtection",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-enable-asr-rules",
            Label = "Enable Attack Surface Reduction",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Defender ASR rules which block common attack vectors like Office macro exploits and script-based threats.",
            Tags = ["defender", "asr", "security", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR",
                    "ExploitGuard_ASR_Rules",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR",
                    "ExploitGuard_ASR_Rules",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR",
                    "ExploitGuard_ASR_Rules",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-edge-smartscreen",
            Label = "Disable SmartScreen for Edge",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables SmartScreen phishing filter specifically for Microsoft Edge.",
            Tags = ["defender", "smartscreen", "edge"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0)],
        },
        new TweakDef
        {
            Id = "sec-defender-cpu-limit",
            Label = "Reduce Defender CPU Usage",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits Windows Defender scan CPU usage to 25%. Prevents Defender from slowing down the system during scans. Options: 5-100. Default: 50. Recommended: 25.",
            Tags = ["security", "defender", "cpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 50)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25)],
        },
        new TweakDef
        {
            Id = "sec-defender-disable-nis",
            Label = "Disable Defender Network Inspection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Defender Network Inspection System (NIS) protocol analysis. Reduces CPU overhead from deep packet inspection. Default: Enabled. Recommended: Disabled if using third-party firewall.",
            Tags = ["security", "defender", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS", "DisableProtocolRecognition", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS", "DisableProtocolRecognition")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS", "DisableProtocolRecognition", 1)],
        },

        new TweakDef
        {
            Id = "sec-enable-audit-logon",
            Label = "Enable Logon Event Auditing",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables auditing of logon success and failure events. Default: Disabled. Recommended: Enabled for security monitoring.",
            Tags = ["security", "audit", "logon", "monitoring"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "AuditLogonEvents", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "AuditLogonEvents", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "AuditLogonEvents", 3)],
        },
        new TweakDef
        {
            Id = "sec-disable-pua-protection",
            Label = "Disable PUA Detection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Potentially Unwanted Application (PUA) detection in Windows Defender via MpEnablePus policy. Default: Enabled. Recommended: Keep enabled for safety.",
            Tags = ["security", "defender", "pua", "detection", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1)],
        },
        new TweakDef
        {
            Id = "sec-disable-smbv1",
            Label = "Disable SMBv1 Protocol",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the legacy SMBv1 protocol on the server side. Mitigates WannaCry and EternalBlue vulnerabilities. Default: Enabled. Recommended: Disabled.",
            Tags = ["security", "smb", "smbv1", "protocol", "vulnerability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 0)],
        },

        new TweakDef
        {
            Id = "sec-enable-spectre-mitigations",
            Label = "Enable Spectre/Meltdown Mitigations",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures Spectre (variant 2) and Meltdown mitigations are enabled via FeatureSettingsOverride. May reduce performance on older CPUs. Default: usually enabled by Windows Update. Recommended: Enabled.",
            Tags = ["security", "spectre", "meltdown", "cpu", "vulnerability", "mitigations"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverrideMask",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverrideMask"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-uac-always-notify",
            Label = "Set UAC to Always Notify (Highest Level)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UAC to 'Always notify' (ConsentPromptBehaviorAdmin=2) — prompts for both Windows changes and other program elevation requests. Default: notify only for app changes (5). Recommended: Always notify.",
            Tags = ["security", "uac", "elevation", "prompt", "consent"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", 5),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", 2),
            ],
        },
        new TweakDef
        {
            Id = "sec-restrict-ntlmv1",
            Label = "Require NTLMv2 (Block LM and NTLMv1)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets LmCompatibilityLevel=5 to only use NTLMv2 and refuse LM/NTLMv1 responses. Hardens network authentication. May break legacy devices. Default: 3 (NTLMv2 only send). Recommended: 5 for hardened environments.",
            Tags = ["security", "ntlm", "ntlmv1", "authentication", "network", "lm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id = "sec-disable-wdigest",
            Label = "Disable WDigest Authentication (Credential Hardening)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables WDigest authentication to prevent plain-text password storage in LSASS. Mitigates credential harvesting attacks via Mimikatz. Default: Enabled on older systems. Recommended: Disabled.",
            Tags = ["security", "wdigest", "lsass", "credential", "mimikatz"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-cred-guard-policy",
            Label = "Enable Credential Guard via Policy",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Credential Guard via LsaCfgFlags=1, protecting LSASS credential secrets. Requires TPM 2.0 and Secure Boot. Default: Disabled. Recommended: Enabled on modern hardware.",
            Tags = ["security", "credential-guard", "lsa", "tpm", "secureboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1)],
        },
        new TweakDef
        {
            Id = "sec-enable-behavior-monitoring",
            Label = "Enable Defender Behavior Monitoring",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Defender behavior monitoring, which watches processes for suspicious activity patterns beyond signature-based detection. Default: Enabled. Recommended: Enabled.",
            Tags = ["security", "defender", "behavior", "monitoring", "heuristics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableBehaviorMonitoring",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableBehaviorMonitoring",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableBehaviorMonitoring",
                    0
                ),
            ],
        },


        new TweakDef
        {
            Id = "sec-enable-sehop",
            Label = "Enable SEHOP (Exception Chain Validation)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Structured Exception Handler Overwrite Protection (SEHOP). Protects against SEH-based exploitation techniques. Default: Disabled (on client SKUs). Recommended: Enabled.",
            Tags = ["security", "sehop", "exploit", "mitigation", "kernel", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            SideEffects = "May break old 16-bit apps that mis-use SEH.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-admin-shares",
            Label = "Disable Automatic Administrative Shares",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic C$ and ADMIN$ administrative shares. Reduces lateral movement options for attackers on local networks. Default: Enabled. Recommended: Disabled on non-managed workstations.",
            Tags = ["security", "admin-shares", "smb", "lateral-movement", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            SideEffects = "Remote admin tools relying on C$ or ADMIN$ will fail.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "sec-restrict-cd-rom",
            Label = "Restrict CD-ROM to Logged-On User",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts CD-ROM drive access to the currently logged-on user only. Prevents remote users from accessing optical media on this machine. Default: Not restricted. Recommended: Restricted.",
            Tags = ["security", "cd-rom", "optical", "access-control", "winlogon"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AllocateCDRoms", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AllocateCDRoms")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AllocateCDRoms", "1")],
        },
        new TweakDef
        {
            Id = "sec-block-untrusted-fonts",
            Label = "Block Untrusted Fonts",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks processing of untrusted fonts loaded from the network. Mitigates font-based exploits. Default: allowed.",
            Tags = ["security", "fonts", "untrusted", "exploit-mitigation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel"],
            ApplyOps =
            [
                RegOp.SetQword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 0x1000000000000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-exploit-protection-dep",
            Label = "Enable DEP for All Programs",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Data Execution Prevention for all programs, not just essential Windows services. Default: opt-in only.",
            Tags = ["security", "dep", "exploit", "mitigation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "MoveImages", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "MoveImages")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "MoveImages", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-credential-guard",
            Label = "Enable Credential Guard",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Defender Credential Guard to isolate secrets using virtualization-based security. Requires Hyper-V. Default: disabled.",
            Tags = ["security", "credential-guard", "vbs", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-smartscreen-for-edge",
            Label = "Disable SmartScreen Filter for Edge",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the SmartScreen filter specifically for Microsoft Edge. Reduces download scan delays. Default: enabled.",
            Tags = ["security", "smartscreen", "edge", "filter"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-audit-process-creation",
            Label = "Enable Audit Process Creation Events",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Includes command-line data in process creation audit events (Event ID 4688). Aids forensic analysis. Default: disabled.",
            Tags = ["security", "audit", "process", "forensics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-block-exclusion-local-merge",
            Label = "Block Defender Exclusion Local Merge",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents local administrators from adding Defender exclusions that override policy. Only policy-managed exclusions apply. Default: allowed.",
            Tags = ["security", "defender", "exclusions", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableLocalAdminMerge", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableLocalAdminMerge")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableLocalAdminMerge", 1)],
        },
        new TweakDef
        {
            Id = "sec-defender-dev-exclusions",
            Label = "Add Defender Dev Folder Exclusions",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Adds common developer folders as Defender exclusions to reduce build/compile scan overhead. Excludes src, .git, node_modules patterns. Default: no exclusions.",
            Tags = ["security", "defender", "exclusions", "developer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Paths"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Paths", @"%USERPROFILE%\source", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Paths", @"%USERPROFILE%\source"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Paths", @"%USERPROFILE%\source", 0),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-auto-sample-submission",
            Label = "Disable Defender Auto Sample Submission",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic sample submission when Defender detects a suspicious file. Requires manual approval before sending files to Microsoft. Default: auto-submit.",
            Tags = ["security", "defender", "samples", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SubmitSamplesConsent", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SubmitSamplesConsent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SubmitSamplesConsent", 2)],
        },
        new TweakDef
        {
            Id = "sec-disable-defender-cloud-samples",
            Label = "Disable Defender Cloud Sample Upload",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables cloud-delivered sample uploads for Windows Defender. Prevents file content from being sent to Microsoft cloud. Default: enabled.",
            Tags = ["security", "defender", "cloud", "samples"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SpynetReporting", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SpynetReporting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "SpynetReporting", 0)],
        },
        new TweakDef
        {
            Id = "sec-disable-defender-notifications",
            Label = "Disable Defender Notifications",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses Windows Defender toast notifications. Scans and protection still operate silently. Default: notifications shown.",
            Tags = ["security", "defender", "notifications", "quiet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\UX Configuration"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 1),
            ],
        },
        new TweakDef
        {
            Id = "sec-disable-defender-realtime",
            Label = "Disable Defender Real-Time Protection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Defender real-time monitoring via policy. WARNING: Leaves system without active antimalware protection. Default: enabled.",
            Tags = ["security", "defender", "realtime", "disable"],
            SideEffects = "System will have no active antimalware protection until re-enabled.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "sec-scan-not-idle-only",
            Label = "Allow Defender Scan While Not Idle",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows Defender scheduled scans to run even when the system is not idle. Ensures scans complete on systems that rarely go idle. Default: idle only.",
            Tags = ["security", "defender", "scan", "idle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableRestorePoint", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableRestorePoint")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableRestorePoint", 0)],
        },
        new TweakDef
        {
            Id = "sec-scan-schedule-weekly",
            Label = "Set Defender Scan to Weekly Schedule",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Schedules Defender scans to run weekly (day 1 = Sunday) instead of daily. Reduces performance impact from daily scans. Default: daily.",
            Tags = ["security", "defender", "scan", "schedule"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "ScheduleDay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "ScheduleDay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "ScheduleDay", 1)],
        },
    ];
}

// ── merged from Hardening.cs ────────────────────────────────────────
internal static class Hardening
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "harden-disable-wdigest",
            Label = "Disable WDigest Plaintext Caching",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WDigest from storing credentials in plaintext memory. Mitigates Mimikatz-style attacks.",
            Tags = ["hardening", "security", "credential", "mimikatz"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
        },

        new TweakDef
        {
            Id = "harden-restrict-ntlm-outgoing",
            Label = "Restrict Outgoing NTLM Traffic",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Restricts outgoing NTLM authentication to audit mode first, then deny all. Improves security posture against relay attacks.",
            Tags = ["hardening", "security", "ntlm", "authentication"],
            SideEffects = "May break legacy apps requiring NTLM.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 1)],
        },
        new TweakDef
        {
            Id = "harden-enable-aslr-force",
            Label = "Force ASLR for All Images",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Address Space Layout Randomization for all executables, including those not compiled with ASLR. CIS benchmark recommendation.",
            Tags = ["hardening", "security", "aslr", "exploit-mitigation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationOptions", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationOptions", 256)],
        },
        new TweakDef
        {
            Id = "harden-disable-null-session-pipes",
            Label = "Restrict Anonymous Named Pipe Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears the list of named pipes accessible via anonymous/null sessions. STIG recommendation.",
            Tags = ["hardening", "security", "anonymous", "null-session"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters"],
            ApplyOps = [RegOp.SetMultiSz(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters", "NullSessionPipes", [])],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters", "NullSessionPipes")],
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\LanManServer\\Parameters' -Name NullSessionPipes -ErrorAction SilentlyContinue).NullSessionPipes.Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count == 0;
            },
        },
        new TweakDef
        {
            Id = "harden-restrict-remote-sam",
            Label = "Restrict Remote SAM Access (SDDL)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts remote SAM enumeration to administrators only. Prevents enumeration of local users and groups.",
            Tags = ["hardening", "security", "sam", "enumeration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)"),
            ],
        },
        new TweakDef
        {
            Id = "harden-disable-remote-uac-filter",
            Label = "Enable Remote UAC (LocalAccountTokenFilterPolicy)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures remote UAC filtering is enabled (value 0). Prevents pass-the-hash attacks via local administrator accounts.",
            Tags = ["hardening", "security", "uac", "pass-the-hash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy", 0),
            ],
        },
        new TweakDef
        {
            Id = "harden-enable-smb-encryption",
            Label = "Enable SMB Encryption (Server)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables SMB encryption on the server side to protect file share traffic from eavesdropping.",
            Tags = ["hardening", "security", "smb", "encryption"],
            SideEffects = "Older SMB clients that don't support encryption will be unable to connect.",
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EncryptData $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EncryptData $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).EncryptData");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "harden-disable-smb1",
            Label = "Disable SMBv1 Protocol",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables the legacy SMBv1 protocol which is vulnerable to EternalBlue/WannaCry exploits.",
            Tags = ["hardening", "security", "smb", "wannacry"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-SmbServerConfiguration -EnableSMB1Protocol $false -Force; "
                        + "Disable-WindowsOptionalFeature -Online -FeatureName SMB1Protocol -NoRestart -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EnableSMB1Protocol $true -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).EnableSMB1Protocol -eq $false");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "harden-enable-secure-boot-check",
            Label = "Verify Secure Boot Status",
            Category = "Security",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Checks if Secure Boot is enabled. Detection-only tweak for security auditing.",
            Tags = ["hardening", "security", "secureboot", "audit"],
            ApplyAction = _ => { }, // Read-only check
            RemoveAction = _ => { },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("Confirm-SecureBootUEFI -ErrorAction SilentlyContinue");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "harden-enable-audit-logon-events",
            Label = "Enable Audit Logon Events",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables auditing of logon success and failure events for security monitoring.",
            Tags = ["hardening", "security", "audit", "logon"],
            ApplyAction = _ => ShellRunner.Run("auditpol.exe", ["/set", "/subcategory:Logon", "/success:enable", "/failure:enable"]),
            RemoveAction = _ => ShellRunner.Run("auditpol.exe", ["/set", "/subcategory:Logon", "/success:disable", "/failure:disable"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("auditpol.exe", ["/get", "/subcategory:Logon"]);
                return stdout.Contains("Success and Failure", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "harden-set-password-policy",
            Label = "Enforce Minimum Password Length (12 chars)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets the local security policy to require minimum 12-character passwords.",
            Tags = ["hardening", "security", "password", "policy"],
            ApplyAction = _ => ShellRunner.Run("net.exe", ["accounts", "/minpwlen:12"]),
            RemoveAction = _ => ShellRunner.Run("net.exe", ["accounts", "/minpwlen:0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("net.exe", ["accounts"]);
                return stdout.Contains("Minimum password length          12", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "harden-enable-firewall-all-profiles",
            Label = "Enable Windows Firewall (All Profiles)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Ensures Windows Firewall is enabled for Domain, Private, and Public profiles.",
            Tags = ["hardening", "security", "firewall", "network"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled True"),
            RemoveAction = _ => { }, // Don't disable firewall
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-NetFirewallProfile | Where-Object Enabled -eq $false).Count -eq 0");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "harden-disable-autorun",
            Label = "Disable AutoRun / AutoPlay for All Drives",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoRun and AutoPlay for all drive types to prevent malware execution via USB or optical media.",
            Tags = ["hardening", "security", "autorun", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
        },
        new TweakDef
        {
            Id = "harden-block-remote-sam",
            Label = "Restrict Remote SAM Enumeration",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts remote enumeration of SAM accounts and groups (CIS L1 benchmark).",
            Tags = ["hardening", "security", "sam", "cis"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)"),
            ],
        },


        new TweakDef
        {
            Id = "harden-enable-smb-signing-server",
            Label = "Require SMB Signing (Server)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires SMB packet signing on the server side to prevent relay and MitM attacks.",
            Tags = ["hardening", "security", "smb", "signing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 1),
            ],
        },

        new TweakDef
        {
            Id = "harden-enforce-smb-encryption",
            Label = "Enforce SMB 3.0 Encryption",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Requires SMB 3.0 encryption on the server, preventing eavesdropping on file share traffic.",
            Tags = ["hardening", "security", "smb", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EncryptData", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EncryptData", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EncryptData", 1)],
        },
        new TweakDef
        {
            Id = "harden-disable-admin-shares",
            Label = "Disable Administrative Shares (C$, ADMIN$)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables default administrative shares (C$, ADMIN$, IPC$) to reduce lateral movement risk.",
            Tags = ["hardening", "security", "shares", "lateral-movement"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        // ── Sprint 18 — 10 new Hardening tweaks ───────────────────────────
        new TweakDef
        {
            Id = "harden-disable-wpad",
            Label = "Disable WPAD (Web Proxy Auto-Discovery)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Web Proxy Auto-Discovery Protocol service. WPAD can be exploited for MITM attacks on untrusted networks. Default: enabled.",
            Tags = ["hardening", "wpad", "proxy", "mitm", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "harden-disable-lanman-auth",
            Label = "Disable LM Authentication (NTLMv2 Only)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LAN Manager authentication level to 'Send NTLMv2 response only. Refuse LM & NTLM'. Prevents weak hash capture. Default: negotiate.",
            Tags = ["hardening", "ntlm", "lanman", "authentication", "hash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id = "harden-enable-structured-exception-handling",
            Label = "Enable SEHOP (Structured Exception Handler Overwrite Protection)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Structured Exception Handler Overwrite Protection. Prevents SEH-based buffer overflow exploits. Default: varies.",
            Tags = ["hardening", "sehop", "exploit", "buffer-overflow", "mitigation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0),
            ],
        },
        new TweakDef
        {
            Id = "harden-enable-aslr-bottom-up",
            Label = "Enable Mandatory ASLR (Bottom-Up Randomisation)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces all images to be randomised regardless of /DYNAMICBASE flag. Hardens against ROP/JOP attacks. Default: opt-in only.",
            Tags = ["hardening", "aslr", "mitigation", "exploit", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationOptions", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationOptions", 256)],
        },
        new TweakDef
        {
            Id = "harden-restrict-anonymous-sam",
            Label = "Restrict Anonymous SAM Account Enumeration",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts anonymous enumeration of SAM accounts and shares. Prevents reconnaissance of user accounts. Default: allowed.",
            Tags = ["hardening", "sam", "anonymous", "enumeration", "recon"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousSAM", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousSAM")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousSAM", 1)],
        },
        new TweakDef
        {
            Id = "harden-enable-cfg",
            Label = "Enable Control Flow Guard (CFG) System-Wide",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures Control Flow Guard is enabled system-wide. CFG prevents exploitation of indirect call targets. Default: on in modern Windows.",
            Tags = ["hardening", "cfg", "control-flow", "mitigation", "exploit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "EnableCfg", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "EnableCfg")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "EnableCfg", 1)],
        },
        new TweakDef
        {
            Id = "harden-disable-autoplay-all-drives",
            Label = "Disable AutoPlay on All Drives",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables AutoPlay/AutoRun on all drive types including USB. Prevents malware auto-execution from removable media. Default: enabled for some drives.",
            Tags = ["hardening", "autoplay", "autorun", "usb", "malware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
        },
        new TweakDef
        {
            Id = "harden-restrict-named-pipe-access",
            Label = "Restrict Anonymous Access to Named Pipes",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts anonymous access to named pipes. Reduces lateral movement and IPC exploitation risk. Default: some pipes accessible.",
            Tags = ["hardening", "named-pipes", "anonymous", "lateral-movement", "ipc"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess", 1),
            ],
        },
        new TweakDef
        {
            Id = "harden-block-ntlm-outgoing-traffic",
            Label = "Block Outgoing NTLM Traffic to Remote Servers",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks outgoing NTLM authentication to remote servers. Forces Kerberos or modern auth. Prevents credential relay. Default: allow all.",
            Tags = ["hardening", "ntlm", "kerberos", "credential-relay", "authentication"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 2)],
        },
        new TweakDef
        {
            Id = "harden-ntlm-v2-only",
            Label = "Enforce NTLMv2 authentication only (LmCompatibilityLevel=5)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hardening", "ntlm", "ntlmv2", "lm", "authentication"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id = "harden-no-lm-hash-stored",
            Label = "Do not store LAN Manager password hash",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "lm", "password", "hash", "authentication"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoLMHash", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoLMHash")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoLMHash", 1)],
        },
        new TweakDef
        {
            Id = "harden-restrict-anonymous-connections",
            Label = "Fully restrict anonymous connections to SAM and shares",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hardening", "anonymous", "sam", "shares", "null-session"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 2)],
        },
        new TweakDef
        {
            Id = "harden-no-anonymous-in-everyone",
            Label = "Exclude anonymous users from Everyone group",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "anonymous", "everyone", "group", "security"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "EveryoneIncludesAnonymous", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "EveryoneIncludesAnonymous")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "EveryoneIncludesAnonymous", 0)],
        },
        new TweakDef
        {
            Id = "harden-netlogon-require-sign-seal",
            Label = "Require Netlogon secure channel signing and sealing",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "netlogon", "signing", "sealing", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireSignOrSeal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireSignOrSeal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireSignOrSeal", 1)],
        },
        new TweakDef
        {
            Id = "harden-netlogon-seal-secure-channel",
            Label = "Require Netlogon secure channel to be sealed (encrypted)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "netlogon", "sealing", "encryption", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SealSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SealSecureChannel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SealSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "harden-netlogon-sign-secure-channel",
            Label = "Require Netlogon secure channel to be signed",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "netlogon", "signing", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SignSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SignSecureChannel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SignSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "harden-netlogon-require-strong-key",
            Label = "Require strong session key for Netlogon",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "netlogon", "key", "domain", "security"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireStrongKey", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireStrongKey")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireStrongKey", 1)],
        },
        new TweakDef
        {
            Id = "harden-no-blank-password-network",
            Label = "Limit blank password use to console logon only",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "password", "blank", "network", "security"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LimitBlankPasswordUse", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LimitBlankPasswordUse")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LimitBlankPasswordUse", 1)],
        },
        new TweakDef
        {
            Id = "harden-no-default-admin-owner",
            Label = "Prevent administrators from being default object owners",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "admin", "owner", "object", "acl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoDefaultAdminOwner", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoDefaultAdminOwner")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "NoDefaultAdminOwner", 1)],
        },
        new TweakDef
        {
            Id = "harden-force-logon-on-unlock",
            Label = "Force password entry when unlocking workstation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "logon", "unlock", "password", "security"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ForceUnlockLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ForceUnlockLogon")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ForceUnlockLogon", 1)],
        },
        new TweakDef
        {
            Id = "harden-rpc-restrict-remote-clients",
            Label = "Restrict unauthenticated RPC clients via GPO",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "rpc", "remote", "unauthenticated", "policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc", "RestrictRemoteClients", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc", "RestrictRemoteClients")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc", "RestrictRemoteClients", 1)],
        },
        new TweakDef
        {
            Id = "harden-machine-password-max-age",
            Label = "Set maximum machine account password age to 30 days",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "password", "machine", "domain", "rotation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "MaximumPasswordAge", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "MaximumPasswordAge")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "MaximumPasswordAge", 30)],
        },
        new TweakDef
        {
            Id = "harden-audit-ntlm-traffic",
            Label = "Enable NTLM traffic auditing on server",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hardening", "ntlm", "audit", "traffic", "logging"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "AuditReceivingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "AuditReceivingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "AuditReceivingNTLMTraffic", 2)],
        },
    ];
}

// ── Smart App Control ─────────────────────────────────────────────────────────
// Merged from SmartAppControl.cs (SAC, HVCI/Memory Integrity, VBS controls)

internal static class SmartAppControl
{
    // SAC state values: 0=Off, 1=Enforce, 2=Evaluate
    private const string SacState = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
    private const string VbsPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";
    private const string HvciPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";
    private const string IsgPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";
    private const string WdagPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Hvsi\EnableVirtualizationBasedSecurity";
    private const string AppGuard = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\HVSI";
    private const string SacFeedPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModel\StateChangeNotifications";
    private const string KdmaPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string CiDbg = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sac-set-evaluation-mode",
            Label = "Set Smart App Control to Evaluation Mode",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "security", "wdac"],
            Description =
                "Puts Smart App Control into evaluation (audit) mode — it monitors "
                + "apps but does not block anything. Useful to assess impact before "
                + "enabling enforcement. Value: 2=Evaluate, 1=Enforce, 0=Off.",
            ApplyOps = [RegOp.SetDword(SacState, "VerifiedAndReputablePolicyState", 2)],
            RemoveOps = [RegOp.DeleteValue(SacState, "VerifiedAndReputablePolicyState")],
            DetectOps = [RegOp.CheckDword(SacState, "VerifiedAndReputablePolicyState", 2)],
        },
        new TweakDef
        {
            Id = "sac-disable-smart-app-control",
            Label = "Disable Smart App Control",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["smart app control", "sac", "security", "gaming", "performance"],
            Description =
                "Turns off Smart App Control entirely. "
                + "Required for running unsigned apps, custom scripts, or tools with"
                + " unknown publishers. Also avoids false-positive latency during app launch.",
            ApplyOps = [RegOp.SetDword(SacState, "VerifiedAndReputablePolicyState", 0)],
            RemoveOps = [RegOp.SetDword(SacState, "VerifiedAndReputablePolicyState", 2)], // revert to evaluate
            DetectOps = [RegOp.CheckDword(SacState, "VerifiedAndReputablePolicyState", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-hvci",
            Label = "Disable Memory Integrity (HVCI)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hvci", "memory integrity", "gaming", "performance", "vbs"],
            Description =
                "Disables Hypervisor-Protected Code Integrity (HVCI / Memory Integrity). "
                + "Can improve gaming frame rates (3–10%) by enabling drivers that are "
                + "incompatible with HVCI. Reduces protection against driver-level malware. "
                + "Requires reboot.",
            ApplyOps = [RegOp.SetDword(HvciPol, "Enabled", 0), RegOp.SetDword(HvciPol, "WasEnabledBy", 0)],
            RemoveOps = [RegOp.SetDword(HvciPol, "Enabled", 1), RegOp.SetDword(HvciPol, "WasEnabledBy", 1)],
            DetectOps = [RegOp.CheckDword(HvciPol, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-virtualization-based-security",
            Label = "Disable Virtualization-Based Security (VBS)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["vbs", "virtualization", "gaming", "performance", "security"],
            Description =
                "Disables Virtualization-Based Security — the hypervisor layer that "
                + "hosts HVCI and Credential Guard. Frees CPU/memory reserved for the "
                + "hypervisor. Significant performance impact on some Ryzen systems. "
                + "Requires reboot.",
            ApplyOps = [RegOp.SetDword(VbsPol, "EnableVirtualizationBasedSecurity", 0)],
            RemoveOps = [RegOp.SetDword(VbsPol, "EnableVirtualizationBasedSecurity", 1)],
            DetectOps = [RegOp.CheckDword(VbsPol, "EnableVirtualizationBasedSecurity", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-intelligent-security-graph",
            Label = "Disable ISG Cloud Lookup for Smart App Control",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["smart app control", "sac", "privacy", "cloud", "telemetry"],
            Description =
                "Prevents Smart App Control from querying Microsoft's Intelligent "
                + "Security Graph (ISG) cloud service to validate app reputation. "
                + "Improves privacy and prevents launch-time network calls, at the cost "
                + "of reduced SAC accuracy.",
            ApplyOps = [RegOp.SetDword(IsgPol, "EnableISGState", 0)],
            RemoveOps = [RegOp.DeleteValue(IsgPol, "EnableISGState")],
            DetectOps = [RegOp.CheckDword(IsgPol, "EnableISGState", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-wdag",
            Label = "Disable Windows Defender Application Guard (WDAG)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["wdag", "application guard", "security", "performance", "browser"],
            Description =
                "Disables WDAG — the containerised browser/Office environment that "
                + "uses Hyper-V to isolate untrusted documents. Frees significant RAM "
                + "and hypervisor overhead if you don't use Office or Edge WDAG mode.",
            ApplyOps = [RegOp.SetDword(AppGuard, "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(AppGuard, "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(AppGuard, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sac-enable-audit-only-code-integrity",
            Label = "Enable Code Integrity Audit Mode (Log Without Block)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "security", "audit", "wdac"],
            Description =
                "Switches Code Integrity enforcement to audit-only mode: violations "
                + "are logged to the event log but not blocked. Useful for assessing "
                + "impact before enforcing WDAC policies.",
            ApplyOps = [RegOp.SetDword(CiDbg, "AuditMode", 1)],
            RemoveOps = [RegOp.SetDword(CiDbg, "AuditMode", 0)],
            DetectOps = [RegOp.CheckDword(CiDbg, "AuditMode", 1)],
        },
        new TweakDef
        {
            Id = "sac-disable-state-change-notifications",
            Label = "Disable SAC State-Change Notifications",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "notifications"],
            Description =
                "Suppresses toast notifications that appear when Smart App Control "
                + "changes state (e.g., switches from Evaluation to Off). "
                + "Prevents distracting pop-ups on managed systems.",
            ApplyOps = [RegOp.SetDword(SacFeedPol, "DisableSacTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(SacFeedPol, "DisableSacTelemetry")],
            DetectOps = [RegOp.CheckDword(SacFeedPol, "DisableSacTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "sac-disable-secure-boot-policy-refresh",
            Label = "Disable Secure Boot Policy Refresh at Boot",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["secure boot", "sac", "boot", "performance"],
            Description =
                "Prevents Windows from re-reading the Secure Boot policy from UEFI "
                + "at every boot, shaving a few milliseconds off POST time on slow "
                + "firmware. Has no security impact after initial policy load.",
            ApplyOps = [RegOp.SetDword(VbsPol, "RequirePlatformSecurityFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(VbsPol, "RequirePlatformSecurityFeatures")],
            DetectOps = [RegOp.CheckDword(VbsPol, "RequirePlatformSecurityFeatures", 0)],
        },
        new TweakDef
        {
            Id = "sac-disable-bitlocker-check",
            Label = "Disable BitLocker Status Check for SAC",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["smart app control", "sac", "bitlocker", "security"],
            Description =
                "Stops Smart App Control from querying BitLocker encryption status "
                + "on every app launch, removing a redundant security check that "
                + "adds latency when BitLocker is already confirmed as active.",
            ApplyOps = [RegOp.SetDword(KdmaPol, "DisableSACBitLockerCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(KdmaPol, "DisableSACBitLockerCheck")],
            DetectOps = [RegOp.CheckDword(KdmaPol, "DisableSACBitLockerCheck", 1)],
        },
    ];
}
