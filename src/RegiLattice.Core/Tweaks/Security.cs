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
            Id = "sec-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LLMNR protocol, preventing man-in-the-middle and credential relay attacks on local networks. Use DNS instead.",
            Tags = ["security", "llmnr", "network", "mitm", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "sec-disable-netbios",
            Label = "Disable NetBIOS over TCP/IP (Policy)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables NetBIOS name resolution over TCP/IP via policy registry. Reduces attack surface from legacy name resolution poisoning.",
            Tags = ["security", "netbios", "network", "hardening", "legacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
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
            Id = "sec-disable-autorun-all",
            Label = "Disable AutoRun for All Drives",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoRun for all drive types. Prevents malware from auto-executing when removable media is inserted.",
            Tags = ["security", "autorun", "usb", "hardening", "malware"],
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
