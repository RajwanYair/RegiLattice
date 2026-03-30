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
            Id = "sec-disable-remote-registry",
            Label = "Disable Remote Registry Service",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the Remote Registry service to disabled. Prevents remote access to the Windows registry, reducing attack surface.",
            Tags = ["security", "remote-registry", "service", "attack-surface"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
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
            Id = "sec-disable-rdp-clipboard-sync",
            Label = "Disable RDP Clipboard Redirection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard contents from being redirected between RDP client and server sessions, reducing data exfiltration risk over remote connections.",
            Tags = ["security", "rdp", "clipboard", "data-loss", "redirection"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Kiosk & Shared PC",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
            Category = "Active Directory",
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
