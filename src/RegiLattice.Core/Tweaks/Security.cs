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
            Description = "Sets Data Execution Prevention to AlwaysOn mode via registry flag. Prevents code execution from non-executable memory pages system-wide.",
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
            Description = "Ensures Windows searches the System directory before the current directory when loading DLLs. Prevents DLL hijacking attacks.",
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
            Description = "Reduces the number of cached domain logon credentials to 1 (default: 10). Minimises credential theft risk if the machine is compromised.",
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
            Description = "Restricts network access to the SAM database to Administrators only via SDDL. Blocks remote enumeration of local users and groups.",
            Tags = ["security", "sam", "remote-access", "hardening", "sddl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)")],
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
            Description = "Disables NetBIOS name resolution over TCP/IP via policy registry. Reduces attack surface from legacy name resolution poisoning.",
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
            Description = "Disables WPAD proxy auto-detection, preventing man-in-the-middle attacks via rogue WPAD servers on corporate or public networks.",
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "sec-disable-powershell-v2",
            Label = "Disable PowerShell v2 Engine (Policy)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the legacy PowerShell v2 engine via policy key. PS v2 bypasses modern logging and AMSI, making it a popular attack vector.",
            Tags = ["security", "powershell", "amsi", "hardening", "downgrade-attack"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell", "EnableScripts", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell", "EnableScripts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell", "EnableScripts", 0)],
        },
    ];
}
