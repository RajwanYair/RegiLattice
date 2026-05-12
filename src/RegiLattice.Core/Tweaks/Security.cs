namespace RegiLattice.Core.Tweaks;

// Sprint B.2: attribute-based module discovery sample

using RegiLattice.Core.Models;

[TweakModule]
internal static class Security
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
        // ── Sprint 21 additions ─────────────────────────────────────────────

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
    ];
}
