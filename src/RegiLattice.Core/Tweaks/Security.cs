namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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

// ── Kiosk & Shared PC ─────────────────────────────────────────────────────────
// Merged from KioskSharedPc.cs (kiosk mode and shared PC configuration)

// ── Active Directory ──────────────────────────────────────────────────────────
// Merged from ActiveDirectory.cs (AD domain membership hardening, Netlogon, Kerberos)

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
        // ── Sprint 18 — 10 new Hardening tweaks ───────────────────────────
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

// ── merged from PolicyCertificate.cs ──
// RegiLattice.Core — Tweaks/PolicyCertificate.cs
// Certificate auto-enrollment, PKI, ADCS, certificate revocation, validation, and smart card credential policies
// Category: "Certificate Policy"
// Consolidated from 8 modules.

internal static class PolicyCertificate
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CertAutoEnrollmentPolicy.Data,
            .. _CertificateBasedAuthPolicy.Data,
            .. _CertificatePolicy.Data,
            .. _CertificateServices.Data,
            .. _CertRevocationPolicy.Data,
            .. _CertValidationPolicy.Data,
            .. _PkiPublicKeyServicesPolicy.Data,
            .. _WindowsAdcsPolicy.Data,
        ];

    // ── CertAutoEnrollmentPolicy ──
    private static class _CertAutoEnrollmentPolicy
    {
        private const string AeLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
        private const string AeCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Cryptography\AutoEnrollment";
        private const string PkiLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\PKI";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "certae-disable-machine-autoenroll",
                Label = "Disable Machine Certificate Auto-Enrollment",
                Category = "Security",
                Description =
                    "Sets AEPolicy=4 in the machine Cryptography AutoEnrollment policy key. "
                    + "Prevents Windows from automatically requesting, renewing, or installing machine "
                    + "certificates from an enterprise CA (Certificate Authority). Machine certificates "
                    + "used for 802.1X, VPN, or device auth will not be auto-issued. "
                    + "Values: 0=off, 4=auto-enroll on, 7=auto-enroll+update+archive. "
                    + "Setting 4 means auto-enrollment is active (reverse: set 0 to disable). "
                    + "Default: 4 (enabled). Recommended: 0 when PKI is managed externally.",
                Tags = ["certificate", "auto-enrollment", "pki", "machine", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Machine certificate auto-enrollment disabled; machine certs must be issued manually.",
                ApplyOps = [RegOp.SetDword(AeLm, "AEPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(AeLm, "AEPolicy")],
                DetectOps = [RegOp.CheckDword(AeLm, "AEPolicy", 0)],
            },
            new TweakDef
            {
                Id = "certae-disable-user-autoenroll",
                Label = "Disable User Certificate Auto-Enrollment",
                Category = "Security",
                Description =
                    "Sets AEPolicy=0 in the user Cryptography AutoEnrollment policy key. "
                    + "Prevents Windows from automatically requesting or renewing user certificates from an "
                    + "enterprise CA on behalf of the logged-in user. User certificates for email signing/encryption "
                    + "or smart card login will not be auto-issued. "
                    + "Default: 4 (enabled). Recommended: 0 in environments where user certs are managed manually.",
                Tags = ["certificate", "auto-enrollment", "user", "pki", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "User certificate auto-enrollment disabled; user certs must be requested manually.",
                ApplyOps = [RegOp.SetDword(AeCu, "AEPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(AeCu, "AEPolicy")],
                DetectOps = [RegOp.CheckDword(AeCu, "AEPolicy", 0)],
            },
            new TweakDef
            {
                Id = "certae-disable-cert-expiry-alerts",
                Label = "Disable Certificate Expiry Balloon Notifications",
                Category = "Security",
                Description =
                    "Sets ExpirationWarning=0 in the machine AutoEnrollment policy key. "
                    + "Prevents Windows from generating balloon notification alerts to users when their "
                    + "personal or machine certificates are approaching their expiry date. "
                    + "Useful in large enterprise deployments where certificate renewal is automated and "
                    + "user notifications create noise. "
                    + "Default: absent (warnings shown). Recommended: 0 when renewal is fully automated.",
                Tags = ["certificate", "expiry", "notification", "balloon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Certificate expiry balloon notifications suppressed; users do not see cert-expiry warnings.",
                ApplyOps = [RegOp.SetDword(AeLm, "ExpirationWarning", 0)],
                RemoveOps = [RegOp.DeleteValue(AeLm, "ExpirationWarning")],
                DetectOps = [RegOp.CheckDword(AeLm, "ExpirationWarning", 0)],
            },
            new TweakDef
            {
                Id = "certae-enable-certificate-logging",
                Label = "Enable Certificate Enrollment Audit Logging",
                Category = "Security",
                Description =
                    "Sets AuditLevel=2 in the machine AutoEnrollment policy key. "
                    + "Enables audit logging for each machine certificate enrollment, renewal, and deletion event. "
                    + "Audit events are written to the Application or Security event log, "
                    + "providing a certificate lifecycle audit trail for compliance purposes. "
                    + "Default: absent (no audit log). Recommended: 2 in PCI-DSS, HIPAA, or NIST-800 environments.",
                Tags = ["certificate", "logging", "audit", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Certificate enrollment/renewal/deletion events written to the Windows Event Log.",
                ApplyOps = [RegOp.SetDword(AeLm, "AuditLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(AeLm, "AuditLevel")],
                DetectOps = [RegOp.CheckDword(AeLm, "AuditLevel", 2)],
            },
            new TweakDef
            {
                Id = "certae-disable-offline-dom-enroll",
                Label = "Disable Certificate Enrollment in Offline Domain Join",
                Category = "Security",
                Description =
                    "Sets OfflineDomainJoinEnrollment=0 in the machine AutoEnrollment policy key. "
                    + "Prevents certificate auto-enrollment from running during an offline domain join operation "
                    + "(djoin.exe /provision). Ensures certificate issuance only occurs after the machine is "
                    + "fully joined and network-connected to the CA. "
                    + "Default: absent (offline enrollment allowed). Recommended: 0 to ensure CA-verified enrollment only.",
                Tags = ["certificate", "offline", "domain-join", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Certificate enrollment suppressed during offline domain join; only online CA-verified enrollment allowed.",
                ApplyOps = [RegOp.SetDword(AeLm, "OfflineDomainJoinEnrollment", 0)],
                RemoveOps = [RegOp.DeleteValue(AeLm, "OfflineDomainJoinEnrollment")],
                DetectOps = [RegOp.CheckDword(AeLm, "OfflineDomainJoinEnrollment", 0)],
            },
            new TweakDef
            {
                Id = "certae-enable-key-based-renewal",
                Label = "Enable Key-Based Certificate Renewal",
                Category = "Security",
                Description =
                    "Sets EnableKeyBasedRenewal=1 in the PKI policy key. "
                    + "Allows Windows to perform key-based certificate renewal (KBR) where the same private key "
                    + "is reused across certificate renewals instead of generating a new key. "
                    + "Reduces PKI overhead in large environments and avoids re-deploying new public keys. "
                    + "Default: absent (KBR disabled). Recommended: 1 when CA supports KBR via SCEP or NDES.",
                Tags = ["certificate", "renewal", "key-reuse", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Key-based renewal allowed; same private key reused during certificate renewal.",
                ApplyOps = [RegOp.SetDword(PkiLm, "EnableKeyBasedRenewal", 1)],
                RemoveOps = [RegOp.DeleteValue(PkiLm, "EnableKeyBasedRenewal")],
                DetectOps = [RegOp.CheckDword(PkiLm, "EnableKeyBasedRenewal", 1)],
            },
            new TweakDef
            {
                Id = "certae-disable-pki-url-retrieval",
                Label = "Disable URL-Based PKI Object Retrieval",
                Category = "Security",
                Description =
                    "Sets EnableCertChainValidation=0 in the PKI policy key. "
                    + "Prevents Windows from automatically fetching CRL, OCSP, and AIA objects from "
                    + "URLs embedded in certificates (when the system cannot reach external URLs). "
                    + "Avoids long validation delays on air-gapped or internet-blocked machines where "
                    + "certificate path building would otherwise time out. "
                    + "Default: absent (URL retrieval enabled). Recommended: 0 on offline/air-gapped systems.",
                Tags = ["certificate", "crl", "ocsp", "url", "pki", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "CRL and OCSP URL lookups disabled; certificate chain validation may not check revocation status.",
                ApplyOps = [RegOp.SetDword(PkiLm, "EnableCertChainValidation", 0)],
                RemoveOps = [RegOp.DeleteValue(PkiLm, "EnableCertChainValidation")],
                DetectOps = [RegOp.CheckDword(PkiLm, "EnableCertChainValidation", 0)],
            },
            new TweakDef
            {
                Id = "certae-set-weak-cert-blocking",
                Label = "Enable Weak Certificate Algorithm Blocking",
                Category = "Security",
                Description =
                    "Sets DisableWeakSignatures=1 in the PKI policy key. "
                    + "Instructs the Windows certificate chain engine to reject certificates that use "
                    + "weak signature algorithms (SHA-1 below certain key lengths, MD5, MD2) when "
                    + "validating certificate chains. Enforces modern cryptographic standards for all "
                    + "certificate operations including auto-enrollment and chain building. "
                    + "Default: absent. Recommended: 1 on systems that must enforce SHA-2/SHA-3 only.",
                Tags = ["certificate", "weak-algorithm", "sha1", "blocking", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Certificates using weak signature algorithms (SHA-1, MD5) rejected during validation.",
                ApplyOps = [RegOp.SetDword(PkiLm, "DisableWeakSignatures", 1)],
                RemoveOps = [RegOp.DeleteValue(PkiLm, "DisableWeakSignatures")],
                DetectOps = [RegOp.CheckDword(PkiLm, "DisableWeakSignatures", 1)],
            },
        ];
    }

    // ── CertificateBasedAuthPolicy ──
    private static class _CertificateBasedAuthPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cbapol-require-smartcard-login",
                    Label = "Require Smart Card for Interactive Logon",
                    Category = "Security",
                    Description =
                        "Requires a smart card (or virtual smart card with TPM) for all interactive Windows logon sessions, blocking password-based or biometric-only sign-in and enforcing certificate-based two-factor authentication.",
                    Tags = ["smartcard", "cba", "certificate", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Smart card logon required; password and biometric login blocked. Requires PKI infrastructure.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceSmartCardSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceSmartCardSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceSmartCardSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-lock-on-smartcard-removal",
                    Label = "Lock Workstation on Smart Card Removal",
                    Category = "Security",
                    Description =
                        "Automatically locks the Windows workstation when the smart card is removed from the reader, enforcing physical card custody and preventing unattended session access.",
                    Tags = ["smartcard", "cba", "lock", "card-removal", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Workstation locks on card removal; session inaccessible without reinserting the card.",
                    ApplyOps = [RegOp.SetDword(Key, "LockWorkstationOnSmartCardRemoval", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LockWorkstationOnSmartCardRemoval")],
                    DetectOps = [RegOp.CheckDword(Key, "LockWorkstationOnSmartCardRemoval", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-require-ocsp-for-crl",
                    Label = "Require OCSP Revocation Check for Smart Card Certificates",
                    Category = "Security",
                    Description =
                        "Forces OCSP (Online Certificate Status Protocol) revocation checking for smart card certificates during logon, immediately blocking revoked certificates rather than waiting for CRL cache to expire.",
                    Tags = ["smartcard", "cba", "ocsp", "revocation", "pki", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "OCSP checked on every smart card logon; revoked cards blocked immediately.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireOCSPRevocationCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireOCSPRevocationCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireOCSPRevocationCheck", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-allow-virtual-smart-card",
                    Label = "Allow TPM Virtual Smart Cards",
                    Category = "Security",
                    Description =
                        "Enables TPM-backed virtual smart cards (VSCs) as an alternative to physical smart card readers, allowing certificate-based authentication without requiring physical card hardware.",
                    Tags = ["smartcard", "virtual-smart-card", "tpm", "cba", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Virtual smart cards enabled; TPM-backed VSCs can be used in lieu of physical smart cards.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowVirtualSmartCard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowVirtualSmartCard")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowVirtualSmartCard", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-block-pin-caching",
                    Label = "Block Smart Card PIN Caching",
                    Category = "Security",
                    Description =
                        "Disables caching of smart card PINs in memory after successful authentication, requiring the user to re-enter their PIN for every cryptographic operation instead of using a cached PIN.",
                    Tags = ["smartcard", "pin", "caching", "cba", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Smart card PIN caching disabled; PIN re-entry required for every cryptographic use.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePINcaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePINcaching")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePINcaching", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-block-root-cert-auto-update",
                    Label = "Block Automatic Root Certificate Auto-Update",
                    Category = "Security",
                    Description =
                        "Prevents Windows from automatically downloading and installing new root certificates from Windows Update, keeping the trusted root store static and under admin control.",
                    Tags = ["cba", "root-certificate", "pki", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Root certificate auto-update blocked; trusted roots only updated via manual admin action.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockRootCertAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockRootCertAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockRootCertAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-log-all-cert-validation",
                    Label = "Enable Audit Logging for Certificate Chain Validation",
                    Category = "Security",
                    Description =
                        "Enables event logging for all certificate chain validation operations including OCSP, CRL, and path-building events, providing a PKI audit trail for smart card and TLS authentication.",
                    Tags = ["cba", "certificate", "audit-log", "pki", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Certificate validation operations logged; PKI health and failures auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditCertificateChainValidation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditCertificateChainValidation")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditCertificateChainValidation", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-block-expired-cert-auth",
                    Label = "Block Authentication with Expired Certificates",
                    Category = "Security",
                    Description =
                        "Rejects smart card or certificate-based logon attempts when the authentication certificate has expired, preventing continuation of sessions using stale credentials.",
                    Tags = ["cba", "certificate", "expiry", "smartcard", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Expired certificates rejected at logon; users must renew PKI certs before sign-in.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockExpiredCertificateAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockExpiredCertificateAuth")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockExpiredCertificateAuth", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-enforce-strong-key-protection",
                    Label = "Enforce Strong Private Key Protection for User Certs",
                    Category = "Security",
                    Description =
                        "Requires strong private key protection (PIN or password confirmation) whenever a user certificate's private key is accessed, preventing silent key use by malicious processes.",
                    Tags = ["cba", "private-key", "strong-protection", "pki", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Strong key protection enforced; PIN confirmation required for every private key operation.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireStrongPrivateKeyProtection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireStrongPrivateKeyProtection")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireStrongPrivateKeyProtection", 1)],
                },
                new TweakDef
                {
                    Id = "cbapol-require-kdc-cert-valid",
                    Label = "Require Valid KDC Certificate for Kerberos PKINIT",
                    Category = "Security",
                    Description =
                        "Requires that the Kerberos KDC certificate presented during PKINIT authentication is valid and trusted, blocking use of self-signed or untrusted KDC certificates in Kerberos certificate-based auth.",
                    Tags = ["cba", "kerberos", "pkinit", "kdc", "pki", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "KDC certificate validity enforced for PKINIT; untrusted KDC certificates rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireValidKDCCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireValidKDCCertificate")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireValidKDCCertificate", 1)],
                },
            ];
    }

    // ── CertificatePolicy ──
    private static class _CertificatePolicy
    {
        private const string Net4_64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319";
        private const string Net4_32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319";
        private const string Net2_64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v2.0.50727";
        private const string Net2_32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v2.0.50727";
        private const string WintrustKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Wintrust\Config";
        private const string Wintrust32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Cryptography\Wintrust\Config";
        private const string AuthRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates\AuthRoot";
        private const string InternetSettings = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "certpol-dotnet-strong-crypto-64",
                Label = "Enable .NET 4 Strong Cryptography (64-bit)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets SchUseStrongCrypto=1 for 64-bit .NET Framework 4.0.30319, enabling "
                    + "TLS 1.2/1.3 by default for all .NET applications and disabling RC4/3DES "
                    + "weak cipher usage.",
                Tags = ["tls", "crypto", "dotnet", "security", "certificates"],
                RegistryKeys = [Net4_64],
                ApplyOps = [RegOp.SetDword(Net4_64, "SchUseStrongCrypto", 1)],
                RemoveOps = [RegOp.DeleteValue(Net4_64, "SchUseStrongCrypto")],
                DetectOps = [RegOp.CheckDword(Net4_64, "SchUseStrongCrypto", 1)],
            },
            new TweakDef
            {
                Id = "certpol-dotnet-strong-crypto-32",
                Label = "Enable .NET 4 Strong Cryptography (32-bit / WoW64)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets SchUseStrongCrypto=1 for 32-bit .NET Framework 4.0.30319 (WoW6432Node), "
                    + "enabling TLS 1.2+ for 32-bit .NET applications and legacy COM-hosted .NET.",
                Tags = ["tls", "crypto", "dotnet", "wow64", "security"],
                RegistryKeys = [Net4_32],
                ApplyOps = [RegOp.SetDword(Net4_32, "SchUseStrongCrypto", 1)],
                RemoveOps = [RegOp.DeleteValue(Net4_32, "SchUseStrongCrypto")],
                DetectOps = [RegOp.CheckDword(Net4_32, "SchUseStrongCrypto", 1)],
            },
            new TweakDef
            {
                Id = "certpol-dotnet-tls12-default-64",
                Label = "Use System TLS Versions in .NET 4 (64-bit)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets SystemDefaultTlsVersions=1 for 64-bit .NET 4.x, allowing .NET apps "
                    + "to inherit the system-wide TLS version from SChannel instead of hardcoding "
                    + "a legacy TLS level.",
                Tags = ["tls", "tls12", "dotnet", "schannel", "security"],
                RegistryKeys = [Net4_64],
                ApplyOps = [RegOp.SetDword(Net4_64, "SystemDefaultTlsVersions", 1)],
                RemoveOps = [RegOp.DeleteValue(Net4_64, "SystemDefaultTlsVersions")],
                DetectOps = [RegOp.CheckDword(Net4_64, "SystemDefaultTlsVersions", 1)],
            },
            new TweakDef
            {
                Id = "certpol-dotnet-tls12-default-32",
                Label = "Use System TLS Versions in .NET 4 (32-bit / WoW64)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets SystemDefaultTlsVersions=1 for 32-bit .NET 4.x (WoW6432Node), " + "matching TLS negotiation behaviour of 64-bit .NET apps.",
                Tags = ["tls", "tls12", "dotnet", "wow64", "schannel"],
                RegistryKeys = [Net4_32],
                ApplyOps = [RegOp.SetDword(Net4_32, "SystemDefaultTlsVersions", 1)],
                RemoveOps = [RegOp.DeleteValue(Net4_32, "SystemDefaultTlsVersions")],
                DetectOps = [RegOp.CheckDword(Net4_32, "SystemDefaultTlsVersions", 1)],
            },
            new TweakDef
            {
                Id = "certpol-dotnet2-strong-crypto-64",
                Label = "Enable .NET 2/3.5 Strong Cryptography (64-bit)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets SchUseStrongCrypto=1 for 64-bit .NET Framework 2.0.50727 (used by "
                    + ".NET 2.0 and 3.5 applications), enabling modern TLS for legacy .NET code.",
                Tags = ["tls", "crypto", "dotnet", "legacy", "security"],
                RegistryKeys = [Net2_64],
                ApplyOps = [RegOp.SetDword(Net2_64, "SchUseStrongCrypto", 1)],
                RemoveOps = [RegOp.DeleteValue(Net2_64, "SchUseStrongCrypto")],
                DetectOps = [RegOp.CheckDword(Net2_64, "SchUseStrongCrypto", 1)],
            },
            new TweakDef
            {
                Id = "certpol-dotnet2-strong-crypto-32",
                Label = "Enable .NET 2/3.5 Strong Cryptography (32-bit / WoW64)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets SchUseStrongCrypto=1 for 32-bit .NET 2.0.50727 (WoW6432Node), "
                    + "ensuring legacy 32-bit .NET 2/3.5 apps use TLS 1.2+ and avoid RC4.",
                Tags = ["tls", "crypto", "dotnet", "legacy", "wow64"],
                RegistryKeys = [Net2_32],
                ApplyOps = [RegOp.SetDword(Net2_32, "SchUseStrongCrypto", 1)],
                RemoveOps = [RegOp.DeleteValue(Net2_32, "SchUseStrongCrypto")],
                DetectOps = [RegOp.CheckDword(Net2_32, "SchUseStrongCrypto", 1)],
            },
            new TweakDef
            {
                Id = "certpol-cert-padding-check-32",
                Label = "Enable Certificate Padding Check (32-bit / WoW64)",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets EnableCertPaddingCheck=1 in the WoW6432Node Wintrust key for 32-bit "
                    + "hosts, completing the MS13-098 certificate padding hardening for both "
                    + "64-bit and 32-bit process spaces.",
                Tags = ["certificate", "padding check", "wintrust", "wow64", "security"],
                RegistryKeys = [Wintrust32],
                ApplyOps = [RegOp.SetString(Wintrust32, "EnableCertPaddingCheck", "1")],
                RemoveOps = [RegOp.DeleteValue(Wintrust32, "EnableCertPaddingCheck")],
                DetectOps = [RegOp.CheckString(Wintrust32, "EnableCertPaddingCheck", "1")],
            },
            new TweakDef
            {
                Id = "certpol-disable-root-auto-update",
                Label = "Disable Automatic Root Certificate Update",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "Prevents Windows from automatically downloading new root certificates from "
                    + "the Windows Update endpoint. Useful in air-gapped or strictly controlled "
                    + "PKI environments. DisableRootAutoUpdate=1.",
                Tags = ["certificate", "root ca", "pki", "auto-update", "air-gap"],
                RegistryKeys = [AuthRoot],
                ApplyOps = [RegOp.SetDword(AuthRoot, "DisableRootAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(AuthRoot, "DisableRootAutoUpdate")],
                DetectOps = [RegOp.CheckDword(AuthRoot, "DisableRootAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "certpol-ie-cert-revocation",
                Label = "Enable Certificate Revocation Checking (Internet Settings)",
                Category = "Security",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Ensures CertificateRevocation=1 in Internet Settings, forcing WinINet and "
                    + "IE/legacy-WebBrowser TLS stacks to check OCSP/CRL for revoked certificates "
                    + "before trusting a server certificate.",
                Tags = ["certificate", "ocsp", "crl", "revocation", "tls", "internet settings"],
                RegistryKeys = [InternetSettings],
                ApplyOps = [RegOp.SetDword(InternetSettings, "CertificateRevocation", 1)],
                RemoveOps = [RegOp.DeleteValue(InternetSettings, "CertificateRevocation")],
                DetectOps = [RegOp.CheckDword(InternetSettings, "CertificateRevocation", 1)],
            },
        ];
    }

    // ── CertificateServices ──
    private static class _CertificateServices
    {
        private const string AutoEnrollMachine = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
        private const string AutoEnrollUser = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
        private const string CertPolicies = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates\Root\ProtectedRoots";
        private const string CertCrlPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates";
        private const string CryptBasePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";
        private const string CaClientsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\AutoEnrollment";
        private const string TrustProviderPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "certpol-protect-root-store",
                Label = "Certificates: Protect Root CA Store from User Modification",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 5,
                RegistryKeys = [CertPolicies],
                Tags = ["certificates", "pki", "root-ca", "security", "hardening"],
                Description =
                    "Sets Flags=1 in SystemCertificates\\Root\\ProtectedRoots. "
                    + "Prevents standard users from adding or removing Root CA certificates from the "
                    + "Trusted Root Certification Authorities store. Blocks MITM attacks that rely on "
                    + "a user silently accepting a malicious root certificate.",
                ApplyOps = [RegOp.SetDword(CertPolicies, "Flags", 1)],
                RemoveOps = [RegOp.DeleteValue(CertPolicies, "Flags")],
                DetectOps = [RegOp.CheckDword(CertPolicies, "Flags", 1)],
            },
            new TweakDef
            {
                Id = "certpol-disable-crl-url-retrieval",
                Label = "Certificates: Disable Automatic CRL/OCSP URL Retrieval for Offline Scenarios",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 4,
                RegistryKeys = [CryptBasePolicy],
                Tags = ["certificates", "pki", "crl", "ocsp", "offline", "security"],
                Description =
                    "Sets DisableOCSP=1 and DisableCRLFetch=1 in the Cryptography policy. "
                    + "Stops Windows from fetching CRL and OCSP responses over the network. "
                    + "Use in air-gapped or offline environments where the CRL distribution point is "
                    + "unreachable and causing 30-second delays.",
                ApplyOps = [RegOp.SetDword(CryptBasePolicy, "DisableOCSP", 1), RegOp.SetDword(CryptBasePolicy, "DisableCRLFetch", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptBasePolicy, "DisableOCSP"), RegOp.DeleteValue(CryptBasePolicy, "DisableCRLFetch")],
                DetectOps = [RegOp.CheckDword(CryptBasePolicy, "DisableOCSP", 1), RegOp.CheckDword(CryptBasePolicy, "DisableCRLFetch", 1)],
            },
            new TweakDef
            {
                Id = "certpol-disable-sha1-for-certs",
                Label = "Certificates: Block SHA-1 Certificates for Server Authentication",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 4,
                RegistryKeys = [CryptBasePolicy],
                Tags = ["certificates", "pki", "sha1", "tls", "security", "hardening"],
                Description =
                    "Sets HashBlockList=\"sha1\" in the Cryptography policy. "
                    + "Instructs Windows' chain-building engine to reject SHA-1-signed certificates "
                    + "for server authentication (TLS). SHA-1 was deprecated by all major CAs in 2017; "
                    + "SHA-1 certificates are now considered insecure for TLS.",
                ApplyOps = [RegOp.SetString(CryptBasePolicy, "HashBlockList", "sha1")],
                RemoveOps = [RegOp.DeleteValue(CryptBasePolicy, "HashBlockList")],
                DetectOps = [RegOp.CheckString(CryptBasePolicy, "HashBlockList", "sha1")],
            },
            new TweakDef
            {
                Id = "certpol-log-cert-chain-errors",
                Label = "Certificates: Enable Certificate Chain Validation Logging",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [CertCrlPolicy],
                Tags = ["certificates", "pki", "audit", "logging", "security"],
                Description =
                    "Sets ChainEngineMonitor=1 in SystemCertificates policy. "
                    + "Enables verbose audit logging for certificate chain validation failures to the "
                    + "Windows Security event log. Useful for diagnosing TLS handshake failures and "
                    + "for detecting certificate-based MITM interception attempts.",
                ApplyOps = [RegOp.SetDword(CertCrlPolicy, "ChainEngineMonitor", 1)],
                RemoveOps = [RegOp.DeleteValue(CertCrlPolicy, "ChainEngineMonitor")],
                DetectOps = [RegOp.CheckDword(CertCrlPolicy, "ChainEngineMonitor", 1)],
            },
            new TweakDef
            {
                Id = "certpol-disable-explicit-user-trust",
                Label = "Certificates: Prevent Users from Trusting Certificates Manually",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [AutoEnrollMachine],
                Tags = ["certificates", "pki", "security", "hardening", "user-trust"],
                Description =
                    "Sets AllowUserCertTrust=0 in the AutoEnrollment policy. "
                    + "Blocks interactive users from adding untrusted certificates to their personal trust "
                    + "store via browser certificate-error dialogs. Certificates must be pushed via GPO "
                    + "or enterprise CA enrollment, preventing social-engineering trust attacks.",
                ApplyOps = [RegOp.SetDword(AutoEnrollMachine, "AllowUserCertTrust", 0)],
                RemoveOps = [RegOp.DeleteValue(AutoEnrollMachine, "AllowUserCertTrust")],
                DetectOps = [RegOp.CheckDword(AutoEnrollMachine, "AllowUserCertTrust", 0)],
            },
        ];
    }

    // ── CertRevocationPolicy ──
    private static class _CertRevocationPolicy
    {
        private const string CertRootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates";

        private const string RevocationKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates\ChainEngine\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "certr-set-max-url-retrieval-timeout",
                    Label = "Certificate Revocation: Set URL Retrieval Timeout to 20 Seconds",
                    Category = "Security",
                    Description =
                        "Sets MaxURLRetrievalTimeout=20000 (ms) in ChainEngine Config. Controls how long the certificate chain validation engine waits for a CRL or OCSP response before treating the revocation check as failed. The default is very long (60 seconds), causing connection delays when a CRL Distribution Point server is offline. Setting to 20 seconds balances security (the revocation check still occurs) against UX (a slow CDP doesn't cause a 60-second hang in certificate operations).",
                    Tags = ["certificate", "crl", "ocsp", "revocation", "timeout"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Reduces revocation retrieval timeout from 60s to 20s. CRL/OCSP servers that respond slowly may be treated as unavailable sooner. Set to a higher value if CRL servers are known to be slow.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "MaxURLRetrievalTimeout", 20000)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "MaxURLRetrievalTimeout")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "MaxURLRetrievalTimeout", 20000)],
                },
                new TweakDef
                {
                    Id = "certr-disable-revocation-on-offline",
                    Label = "Certificate Revocation: Allow Certificate Use When CRL Offline",
                    Category = "Security",
                    Description =
                        "Sets WeakSignatureSettings=0 to allow certificate use when revocation check fails due to network unavailability. Configures the Windows chain engine to treat a revocation check that fails because the CRL/OCSP endpoint is unreachable differently from a positive revocation (certificate is explicitly revoked). When offline revocation is allowed, a certificate passes validation if the revocation server was unreachable (soft fail) rather than blocking the certificate (hard fail). Appropriate for environments where CRL accessibility is inconsistent.",
                    Tags = ["certificate", "crl", "offline", "revocation", "chain"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "Certificates pass validation even when CRL is offline. Combined with short timeout, this is the standard enterprise setting. Hard-fail requires always-on CRL infrastructure.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "WeakSignatureSettings", 0)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "WeakSignatureSettings")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "WeakSignatureSettings", 0)],
                },
                new TweakDef
                {
                    Id = "certr-enable-ocsp-preference",
                    Label = "Certificate Revocation: Enable OCSP Preference Over CRL",
                    Category = "Security",
                    Description =
                        "Sets OCSPPreferEnabled=1 in ChainEngine Config. When a certificate contains both CDP (CRL Distribution Point) and AIA (Authority Information Access / OCSP) extensions, directs Windows to prefer OCSP stapling and online OCSP checks over downloading the full CRL. OCSP provides real-time revocation status for a single certificate without downloading the complete CRL; for single-certificate validation, OCSP is faster and uses less bandwidth than a multi-megabyte CRL download.",
                    Tags = ["certificate", "ocsp", "crl", "revocation", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OCSP checks are preferred. OCSP endpoints must be accessible from client machines. If only CRL infrastructure is deployed, this setting has no effect.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "OCSPPreferEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "OCSPPreferEnabled")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "OCSPPreferEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "certr-set-crl-cache-ttl",
                    Label = "Certificate Revocation: Set CRL Cache Time-to-Live to 4 Hours",
                    Category = "Security",
                    Description =
                        "Sets DefaultOCSPResponderURLRetrievalTimeout=14400 (seconds) in ChainEngine Config to cap the maximum CRL cache lifetime at 4 hours. Windows caches downloaded CRLs for up to their published validity period (often 7 days). A compromised certificate that was revoked after the CRL was cached remains effective for up to 7 days on clients that cached the CRL. Reducing the cache TTL limits the window during which a revoked certificate appears valid to clients that have already cached the pre-revocation CRL.",
                    Tags = ["certificate", "crl", "cache", "revocation", "ttl"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "CRL cache expires after 4 hours, forcing more frequent CRL downloads. Ensure CRL servers can handle the increased download frequency. Alternatively, implement CRL pre-fetch.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "DefaultOCSPResponderURLRetrievalTimeout", 14400)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "DefaultOCSPResponderURLRetrievalTimeout")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "DefaultOCSPResponderURLRetrievalTimeout", 14400)],
                },
                new TweakDef
                {
                    Id = "certr-set-chain-path-length",
                    Label = "Certificate Revocation: Enforce Maximum Certificate Chain Depth of 6",
                    Category = "Security",
                    Description =
                        "Sets ChainEngineEnabledForcePathLengthConstraint=6 in ChainEngine Config. Limits the maximum depth of a certificate chain that Windows will validate. Unlimited chain depth enables issuing certificate chains through an arbitrary number of intermediate CAs, each potentially compromised. Setting a maximum of 6 links (root, 4 intermediates, end-entity) matches industry best practice for enterprise hierarchies and prevents unbounded certificate chain traversal which can be exploited in Name Constraint validation bypass attacks.",
                    Tags = ["certificate", "chain", "depth", "path-length", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Certificate chains deeper than 6 links are rejected. Audit all PKI hierarchies with >1 intermediate CA before enabling. External CAs rarely exceed 3 links.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "ChainEngineEnabledForcePathLengthConstraint", 6)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "ChainEngineEnabledForcePathLengthConstraint")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "ChainEngineEnabledForcePathLengthConstraint", 6)],
                },
                new TweakDef
                {
                    Id = "certr-enable-revocation-for-code-signing",
                    Label = "Certificate Revocation: Enable Revocation Check for Code Signing Certificates",
                    Category = "Security",
                    Description =
                        "Sets CodeSigningRevocationEnabled=1 in SystemCertificates policy. Enables mandatory revocation checking for code signing certificates used when validating Authenticode signatures (EXE, DLL, MSI, PowerShell scripts). Without revocation checking for code signing, a revoked code signing certificate (e.g., stolen by malware author) remains valid for signing malicious code. This is critical for organizations that enforce signature validation before software execution.",
                    Tags = ["certificate", "code-signing", "revocation", "authenticode", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Revocation is checked for code signing certs. Executable files signed with a revoked cert are blocked. Ensure CRL/OCSP is accessible in offline environments or use CRL caching.",
                    ApplyOps = [RegOp.SetDword(CertRootKey, "CodeSigningRevocationEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertRootKey, "CodeSigningRevocationEnabled")],
                    DetectOps = [RegOp.CheckDword(CertRootKey, "CodeSigningRevocationEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "certr-require-delta-crl",
                    Label = "Certificate Revocation: Enable Delta CRL Support for Fresh Revocations",
                    Category = "Security",
                    Description =
                        "Sets EnableDeltaCRL=1 in ChainEngine Config. Enables the processing of Delta CRLs (incremental revocation lists that contain only revocations published since the last base CRL). When a certificate is revoked, the revocation becomes effective with the next published CRL. Base CRLs are typically published weekly; Delta CRLs are published hourly. Using Delta CRLs reduces the window between revocation and client awareness from days to hours, minimising the time a stolen certificate remains valid.",
                    Tags = ["certificate", "delta-crl", "revocation", "freshness", "crl"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Delta CRL support is enabled. Requires the issuing CA to publish Delta CRLs. If the CA does not publish Delta CRLs, this setting has no effect.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "EnableDeltaCRL", 1)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "EnableDeltaCRL")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "EnableDeltaCRL", 1)],
                },
                new TweakDef
                {
                    Id = "certr-deny-untrusted-roots",
                    Label = "Certificate Revocation: Deny Certificates from Untrusted Root CAs",
                    Category = "Security",
                    Description =
                        "Sets AuthRootAutoUpdateEnabled=1 in SystemCertificates to enable automatic trusted root list updates, and sets the chain validation to reject chains with roots not in the Microsoft Trusted Root Programme. When DenyUntrustedRoots=1, Windows blocks TLS connections and application trust for certificates issued by any CA whose root is not in the Microsoft Trusted Root Store and not in the organisation's own Enterprise Trust list. Prevents rogue CA certificates from being trusted.",
                    Tags = ["certificate", "root-ca", "trust", "untrusted", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Certificates from CAs not in the Windows Trusted Root Store are rejected. Internal enterprise CAs must be deployed via Group Policy Enterprise Trust before enabling. Self-signed certificates are rejected.",
                    ApplyOps = [RegOp.SetDword(CertRootKey, "DenyUntrustedRoots", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertRootKey, "DenyUntrustedRoots")],
                    DetectOps = [RegOp.CheckDword(CertRootKey, "DenyUntrustedRoots", 1)],
                },
                new TweakDef
                {
                    Id = "certr-enable-auth-root-auto-update",
                    Label = "Certificate Revocation: Enable Automatic Trusted Root Certificate Updates",
                    Category = "Security",
                    Description =
                        "Sets AuthRootAutoUpdateEnabled=1 in SystemCertificates. Enables automatic download of updates to the Microsoft Trusted Root Certificate Programme from Windows Update. Root certificate stores can become stale if auto-update is disabled: newly cross-signed root CAs cannot be trusted, and distrusted CAs that Microsoft removes (e.g., compromised CAs) may remain trusted. Auto-update ensures the machine's root store reflects current CA trust decisions by the Microsoft PKI team.",
                    Tags = ["certificate", "root-ca", "auto-update", "trust-store", "pki"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Windows Update downloads root certificate list updates. Requires Windows Update access. In isolated environments, push root store updates via Group Policy instead.",
                    ApplyOps = [RegOp.SetDword(CertRootKey, "AuthRootAutoUpdateEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertRootKey, "AuthRootAutoUpdateEnabled")],
                    DetectOps = [RegOp.CheckDword(CertRootKey, "AuthRootAutoUpdateEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "certr-set-ocsp-max-age",
                    Label = "Certificate Revocation: Set OCSP Response Maximum Age to 24 Hours",
                    Category = "Security",
                    Description =
                        "Sets MaxOCSPResponseAge=86400 (seconds = 24 hours) in ChainEngine Config. OCSP stapling allows TLS servers to pre-fetch and embed a signed OCSP response in the TLS handshake, avoiding a separate client-to-OCSP-server round-trip. However, OCSP responses are timestamped and expire; a stale stapled OCSP response must be refreshed. Limiting the maximum acceptable age of an OCSP response to 24 hours ensures clients receive reasonably fresh revocation data without excessive OCSP server load.",
                    Tags = ["certificate", "ocsp", "stapling", "revocation", "ttl"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OCSP responses older than 24 hours trigger a new OCSP request. Ensure OCSP servers are available for certificate validation. Reduce to 4 hours for high-security environments.",
                    ApplyOps = [RegOp.SetDword(RevocationKey, "MaxOCSPResponseAge", 86400)],
                    RemoveOps = [RegOp.DeleteValue(RevocationKey, "MaxOCSPResponseAge")],
                    DetectOps = [RegOp.CheckDword(RevocationKey, "MaxOCSPResponseAge", 86400)],
                },
            ];
    }

    // ── CertValidationPolicy ──
    private static class _CertValidationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CertValidity";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "certvld-disable-auto-root-update",
                Label = "Disable Automatic Root Certificate Update",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Windows automatically downloads updated root certificate trust lists from Microsoft's servers to expand the set of trusted certificate authorities. Disabling automatic root certificate updates prevents new root CAs from being trusted without administrator review. Enterprise certificate trust should be managed through enterprise PKI policies and controlled root certificate distribution. Automatically trusting new root certificates from Microsoft sources could expand the trust domain without security team review. Air-gapped and strictly controlled environments cannot permit outbound connections to certificate infrastructure and must disable this feature. Disabling auto-update requires maintaining the root certificate store manually through Windows Update or offline distribution.",
                Tags = ["certificates", "pki", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoRootCertUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoRootCertUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoRootCertUpdate", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-revocation-check",
                Label = "Enforce Certificate Revocation Checking",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Certificate revocation checking verifies that certificates presented during TLS connections have not been revoked by the issuing certificate authority. Enforcing revocation checking ensures that compromised or misissued certificates cannot be used to impersonate services. Without revocation checking, certificates from compromised private keys remain valid until expiry rather than being immediately blocked. Enterprise environments must enforce revocation checking to detect use of revoked certificates used in MITM attacks. OCSP stapling and CRL caching provide efficient revocation checking without adding significant latency to connections. Disabling this tweak (which enforces checking) reintroduces the risk of accepting revoked certificates.",
                Tags = ["certificates", "revocation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceRevocationChecking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceRevocationChecking")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceRevocationChecking", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-ocsp-staple-bypass",
                Label = "Disable OCSP Stapling Bypass",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "OCSP stapling allows servers to attach pre-fetched revocation status information to TLS handshakes, improving performance for revocation checking. The OCSP staple bypass allows revocation checking to succeed even when stapled OCSP responses are absent or expired. Disabling OCSP staple bypass enforces strict revocation checking that requires a valid stapled response or direct OCSP query. Servers without valid stapled OCSP responses may cause connection failures when strict mode is enforced. This setting should be paired with a reliable OCSP infrastructure to prevent availability disruption from OCSP server outages. Strict OCSP enforcement maximizes revocation checking effectiveness against compromised certificate use.",
                Tags = ["certificates", "ocsp", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOcspStapleBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOcspStapleBypass")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOcspStapleBypass", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-certificate-propagation",
                Label = "Disable Automatic Certificate Propagation",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Certificate propagation automatically distributes user certificates and trusted root certificates from smart cards and external sources into the Windows certificate store. Disabling automatic certificate propagation prevents certificates from being automatically installed from inserted smart cards or other sources. Auto-propagated certificates may expand the enterprise trust domain without deliberate review by the security team. Malicious smart cards or infected media containing certificates could introduce untrusted or backdoored CA roots through propagation. Enterprise certificate management should use Group Policy certificate distribution rather than automatic client-side propagation. Disabling propagation is appropriate for high-security environments where the certificate store must be tightly controlled.",
                Tags = ["certificates", "propagation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCertPropagation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCertPropagation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCertPropagation", 1)],
            },
            new TweakDef
            {
                Id = "certvld-enable-cert-padding-check",
                Label = "Enable Certificate Padding Check",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Certificate padding checks detect and reject certificates with non-standard trailing data appended after the certificate's normal ASN.1 structure. Enabling certificate padding checks prevents use of certificate padding attack techniques that exploit certificate parser flexibility. Some certificate parsing exploits embed malicious code or additional certificate data in the padding area after the valid certificate. Certificate normalization attacks rely on different parsers handling certificate padding differently, creating trust inconsistencies. Enforcing strict padding validation ensures all certificate parsers in the Windows ecosystem handle certificates consistently. Enabling padding checks has no impact on legitimate well-formed certificates that conform to the RFC standard.",
                Tags = ["certificates", "security", "padding", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableCertPaddingCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCertPaddingCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCertPaddingCheck", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-weak-algorithms",
                Label = "Disable Weak Certificate Signature Algorithms",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Weak signature algorithms like MD5 and SHA-1 are cryptographically broken and should not be trusted in certificate chains. Disabling weak signature algorithms prevents Windows from accepting certificates signed with deprecated hash algorithms. MD5-signed certificates have been successfully forged in practical attacks demonstrating the urgency of removing this trust. SHA-1 certificates are no longer considered secure against adversaries with significant compute resources. Enterprise PKI should have migrated to SHA-256 or stronger hash algorithms at all levels of the certificate chain. Enforcement requires that all certificates in the validation path use current approved hash algorithms.",
                Tags = ["certificates", "algorithms", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWeakAlgorithms", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWeakAlgorithms")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWeakAlgorithms", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-expired-cert-bypass",
                Label = "Disable Expired Certificate Bypass",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Certificate expiry bypass allows applications to accept expired certificates in scenarios where the system cannot verify the current time or revocation status. Disabling expired certificate bypass enforces strict expiry checking and rejects all expired certificates regardless of context. Applications that bypass expiry checking may accept certificates that were valid at a past point in time but have since expired. Expired certificates may have been replaced due to private key compromise making their continued acceptance dangerous. Enterprise TLS infrastructure should maintain valid certificates with appropriate expiry margins and not require bypass mechanisms. Disabling expiry bypass hardens the certificate validation path against time-based certificate acceptance attacks.",
                Tags = ["certificates", "expiry", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExpiredCertBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExpiredCertBypass")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExpiredCertBypass", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-cert-chain-building",
                Label = "Restrict Certificate Chain Building to Local Store Only",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Certificate chain building downloads additional intermediate CA certificates from URLs embedded in presented certificates when intermediate CAs are missing. Restricting chain building to the local store only prevents outbound connections to fetch intermediate CA certificates during validation. Certificate download requests reveal connection details to external certificate infrastructure and create a potential availability dependency. In high-security environments certificate chains should be fully pre-distributed through Group Policy rather than dynamically downloaded. Dynamic certificate downloads represent an outbound channel that may be restricted by security controls in air-gapped environments. This setting requires that all necessary intermediate CA certificates be pre-deployed to the enterprise endpoints.",
                Tags = ["certificates", "chain", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictChainBuildingToLocalStore", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictChainBuildingToLocalStore")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictChainBuildingToLocalStore", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-cert-transparency",
                Label = "Enable Certificate Transparency Enforcement",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Certificate Transparency requires TLS certificates to be logged in public append-only logs before browsers and applications trust them. Enabling CT enforcement causes Windows to reject certificates not logged in trusted Certificate Transparency logs. CT enforcement detects misissued certificates from compromised or rogue CAs that were not properly logged. Without CT enforcement, misissued certificates for enterprise domains may go undetected for extended periods. CT logs create a public auditable record of all trusted certificates making unauthorized certificate issuance discoverable. Enforcing CT provides protection against MITM attacks using misissued but otherwise valid-looking certificates.",
                Tags = ["certificates", "transparency", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableCertificateTransparency", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCertificateTransparency")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCertificateTransparency", 1)],
            },
            new TweakDef
            {
                Id = "certvld-disable-cert-telemetry",
                Label = "Disable Certificate Validation Telemetry",
                Category = "Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Certificate validation telemetry reports certificate usage events, validation failures, and chain building statistics to Microsoft. This telemetry helps Microsoft identify problematic certificates and improve certificate infrastructure across the Windows ecosystem. Disabling certificate validation telemetry prevents certificate usage data from being sent from enterprise endpoints. Certificate validation data revealing which certificates are used for enterprise connections constitutes sensitive security configuration information. Enterprise PKI operations and certificate health monitoring should be managed through internal CA management tools. Certificate validation and enforcement functions operate normally regardless of this telemetry setting.",
                Tags = ["certificates", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCertValidationTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCertValidationTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCertValidationTelemetry", 1)],
            },
        ];
    }

    // ── PkiPublicKeyServicesPolicy ──
    private static class _PkiPublicKeyServicesPolicy
    {
        private const string SmartCardKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

        private const string AutoEnrollKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Cryptography\AutoEnrollment";

        private const string PkiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PKI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pki-enable-certificate-auto-enrollment",
                    Label = "PKI: Enable Certificate Auto-Enrollment from Enterprise CA",
                    Category = "Security",
                    Description =
                        "Sets AEPolicy=7 in AutoEnrollment policy (value = AUTOENROLLMENT_ENABLED | UPDATE_PENDING | ENROLL_ON_BEHALF_OF). Enables automatic certificate enrollment from an enterprise CA via Active Directory Certificate Services. Workstations request and renew certificates without user interaction: machine authentication certificates, user signing certificates, and EFS keys are automatically provisioned to domain-joined machines according to certificate templates published in the AD CA. Essential for large-scale PKI deployments.",
                    Tags = ["pki", "auto-enrollment", "certificate", "active-directory", "ca"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Auto-enrollment requires an enterprise CA with published certificate templates. Machines silently enroll for configured templates. No impact if no enterprise CA or templates are configured.",
                    ApplyOps = [RegOp.SetDword(AutoEnrollKey, "AEPolicy", 7)],
                    RemoveOps = [RegOp.DeleteValue(AutoEnrollKey, "AEPolicy")],
                    DetectOps = [RegOp.CheckDword(AutoEnrollKey, "AEPolicy", 7)],
                },
                new TweakDef
                {
                    Id = "pki-disable-smartcard-pin-recovery",
                    Label = "PKI: Disable Smart Card PIN Recovery Mode",
                    Category = "Security",
                    Description =
                        "Sets DisablePINRecovery=1 in SmartCardCredentialProvider policy. Prevents smart card PIN recovery mechanisms that allow an administrator or escrowed key to bypass Smart Card PIN verification. PIN recovery is a usability feature but it weakens the two-factor authentication model of smart cards: if the PIN can be recovered or bypassed administratively, the authentication factor is reduced from 'something you have + something you know' to effectively 'something you have + a password held by IT'. Disabling PIN recovery enforces the full 2FA model.",
                    Tags = ["pki", "smart-card", "pin", "2fa", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Smart card PIN recovery is disabled. Forgotten PINs require re-issuance of the smart card. Ensure lifecycle processes (lost card, forgotten PIN) are documented for users.",
                    ApplyOps = [RegOp.SetDword(SmartCardKey, "DisablePINRecovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartCardKey, "DisablePINRecovery")],
                    DetectOps = [RegOp.CheckDword(SmartCardKey, "DisablePINRecovery", 1)],
                },
                new TweakDef
                {
                    Id = "pki-enable-reverse-subject-name",
                    Label = "PKI: Enable Reversal of Encoded Subject Name in Certificate UI",
                    Category = "Security",
                    Description =
                        "Sets ReverseSubject=1 in PKI policy. Changes the display order of certificate subject Distinguished Name components in the certificate viewer UI from ASN.1-encoded reverse order (dc=net, dc=contoso, cn=Users, cn=JaneExample) to the more intuitive forward-reading order (cn=JaneExample, cn=Users, dc=contoso, dc=net). This purely cosmetic change makes it easier for users and helpdesk staff to verify certificate identity fields without understanding ASN.1 DER encoding conventions.",
                    Tags = ["pki", "certificate", "display", "subject-name", "ui"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Certificate subject names in UI dialogs display in human-readable order. No functional impact — purely cosmetic change in certificate display.",
                    ApplyOps = [RegOp.SetDword(PkiKey, "ReverseSubject", 1)],
                    RemoveOps = [RegOp.DeleteValue(PkiKey, "ReverseSubject")],
                    DetectOps = [RegOp.CheckDword(PkiKey, "ReverseSubject", 1)],
                },
                new TweakDef
                {
                    Id = "pki-force-logon-smartcard",
                    Label = "PKI: Require Smart Card for Interactive Logon",
                    Category = "Security",
                    Description =
                        "Sets ScForceOption=1 in SmartCardCredentialProvider policy. Requires that all interactive logon sessions use a smart card for authentication. When this setting is active, the username/password credential provider is hidden and only the smart card credential provider is visible at the logon screen and UAC prompts. This enforces hardware-backed two-factor authentication for all interactive access: physical smart card (something you have) + PIN (something you know). Cannot be bypassed by users.",
                    Tags = ["pki", "smart-card", "logon", "2fa", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Smart card is required for all interactive logon. Password logon is hidden. ALL administrators must have a working smart card before enabling — lockout risk if smart card infrastructure fails.",
                    ApplyOps = [RegOp.SetDword(SmartCardKey, "ScForceOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartCardKey, "ScForceOption")],
                    DetectOps = [RegOp.CheckDword(SmartCardKey, "ScForceOption", 1)],
                },
                new TweakDef
                {
                    Id = "pki-enable-cert-prop-to-user-store",
                    Label = "PKI: Enable Certificate Propagation from Smart Card to User Store",
                    Category = "Security",
                    Description =
                        "Sets EnableCertPropagation=1 in SmartCardCredentialProvider policy. Activates the Windows Certificate Propagation Service which copies certificates from an inserted smart card into the user's personal certificate store (Cert:\\CurrentUser\\My). Applications that enumerate the user certificate store (email clients for S/MIME, VPN clients, code signing tools) can then find the smart card certificate without requiring explicit application-level smart card support. Certificates are removed from the store when the card is removed.",
                    Tags = ["pki", "smart-card", "certificate", "propagation", "store"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card certificates are copied to user store on card insertion. Required for many applications that read from the certificate store rather than directly querying the smart card.",
                    ApplyOps = [RegOp.SetDword(SmartCardKey, "EnableCertPropagation", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartCardKey, "EnableCertPropagation")],
                    DetectOps = [RegOp.CheckDword(SmartCardKey, "EnableCertPropagation", 1)],
                },
                new TweakDef
                {
                    Id = "pki-enable-root-cert-update",
                    Label = "PKI: Allow Enterprise Trusted Root Certificate Updates via GP",
                    Category = "Security",
                    Description =
                        "Sets EnablePKIUpdates=1 in PKI policy. Allows the Windows PKI infrastructure to process and install enterprise root and intermediate CA certificates that are distributed via the NTAuth certificate store in Active Directory and via Group Policy Objects. Required for domain-joined machines to automatically receive internally issued CA certificate updates. Without this, machines require manual certificate installations when the enterprise CA hierarchy changes (new intermediate, renewed root, distrusted CA).",
                    Tags = ["pki", "certificate", "root-ca", "group-policy", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enterprise CA certificates are automatically propagated from Active Directory. Required for enterprise certificate-based authentication (802.1x, SSTP VPN, IPsec).",
                    ApplyOps = [RegOp.SetDword(PkiKey, "EnablePKIUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(PkiKey, "EnablePKIUpdates")],
                    DetectOps = [RegOp.CheckDword(PkiKey, "EnablePKIUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "pki-enable-pin-change-on-logon",
                    Label = "PKI: Enable Smart Card PIN Change Option at Logon",
                    Category = "Security",
                    Description =
                        "Sets AllowPINChangeAtLogon=1 in SmartCardCredentialProvider. Presents the 'Change PIN' option in the Windows Security screen (Ctrl+Alt+Del) for smart card users, allowing them to update their smart card PIN through the Windows credential interface. Without this option, users must use vendor-specific middleware or management tools to change PINs. Providing PIN change through the familiar Windows interface reduces friction for PIN management, encouraging regular PIN rotation.",
                    Tags = ["pki", "smart-card", "pin", "change", "usability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables PIN change option in Windows security screen. Purely usability improvement — no security impact. Requires compatible smart card middleware.",
                    ApplyOps = [RegOp.SetDword(SmartCardKey, "AllowPINChangeAtLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartCardKey, "AllowPINChangeAtLogon")],
                    DetectOps = [RegOp.CheckDword(SmartCardKey, "AllowPINChangeAtLogon", 1)],
                },
                new TweakDef
                {
                    Id = "pki-disable-smartcard-logon-no-dirsvc",
                    Label = "PKI: Disable Smart Card Logon Without Active Directory Service",
                    Category = "Security",
                    Description =
                        "Sets AllowSmartCardWithoutDirectoryService=0 in SmartCardCredentialProvider policy. Prevents smart card logon when Active Directory is not reachable. In hybrid or cached-credential scenarios, Windows can sometimes allow smart card logon using locally cached credentials even when the AD DC is unavailable. Disabling this prevents smart card authentication from falling back to cached credentials. Ensures all smart card authentications are validated against a live domain controller — preventing stale credential-based access.",
                    Tags = ["pki", "smart-card", "active-directory", "logon", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Smart card logon fails when AD DC is unreachable. Off-network users (VPN users) must be able to reach a DC before authenticating. Do not enable for laptops that frequently work disconnected from the network.",
                    ApplyOps = [RegOp.SetDword(SmartCardKey, "AllowSmartCardWithoutDirectoryService", 0)],
                    RemoveOps = [RegOp.DeleteValue(SmartCardKey, "AllowSmartCardWithoutDirectoryService")],
                    DetectOps = [RegOp.CheckDword(SmartCardKey, "AllowSmartCardWithoutDirectoryService", 0)],
                },
                new TweakDef
                {
                    Id = "pki-enable-cert-transparency-log",
                    Label = "PKI: Enable Certificate Transparency Log Validation",
                    Category = "Security",
                    Description =
                        "Sets EnableCTLog=1 in PKI policy. Enables validation against Certificate Transparency (RFC 9162) logs when verifying TLS server certificates. Certificate Transparency is a public audit mechanism: all publicly trusted CAs are required to submit issued certificates to public CT logs, allowing domain owners to detect mis-issued certificates within hours. When CT validation is enabled, Windows Schannel verifies that a TLS certificate has Signed Certificate Timestamp (SCT) extensions proving inclusion in a CT log.",
                    Tags = ["pki", "certificate", "transparency", "ct-log", "auditing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "TLS certificates without SCTs are rejected. Public CAs include SCTs since 2022. Internal CA certificates may lack SCTs — configure CT whitelist for internal domains.",
                    ApplyOps = [RegOp.SetDword(PkiKey, "EnableCTLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(PkiKey, "EnableCTLog")],
                    DetectOps = [RegOp.CheckDword(PkiKey, "EnableCTLog", 1)],
                },
                new TweakDef
                {
                    Id = "pki-enable-eku-filtering",
                    Label = "PKI: Enable Enhanced Key Usage Filtering in Certificate Validation",
                    Category = "Security",
                    Description =
                        "Sets EKUFiltering=1 in PKI policy. Enables strict Extended Key Usage (EKU) filtering during certificate path validation. The EKU extension in a certificate restricts the cryptographic operations for which the certificate is valid (e.g., serverAuthentication, clientAuthentication, codeSigning, emailProtection). Without EKU filtering, a client authentication certificate could theoretically be misused for server authentication or code signing. EKU filtering enforces the certificate's intended use constraints.",
                    Tags = ["pki", "eku", "certificate", "key-usage", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Certificate EKU constraints are strictly enforced. Certificates issued without an EKU for their intended use are rejected. Audit certificate templates to ensure correct EKU assignments.",
                    ApplyOps = [RegOp.SetDword(PkiKey, "EKUFiltering", 1)],
                    RemoveOps = [RegOp.DeleteValue(PkiKey, "EKUFiltering")],
                    DetectOps = [RegOp.CheckDword(PkiKey, "EKUFiltering", 1)],
                },
            ];
    }

    // ── WindowsAdcsPolicy ──
    private static class _WindowsAdcsPolicy
    {
        private const string CryptoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";
        private const string SmartKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
        private const string CertSvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CertSvc";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "adcspol-protect-root-store",
                    Label = "Protect Root Certificate Store Against Modification",
                    Category = "Security",
                    Description =
                        "Sets ProtectedRootsAllowedToPerformCRLRetrieval=0 in the Cryptography policy. Prevents non-administrative processes from retrieving CRL (Certificate Revocation List) data through the Protected Roots API. Restricting this access reduces the risk of CRL poisoning attacks that could mark legitimate certificates as revoked or prevent revocation checks from completing correctly.",
                    Tags = ["adcs", "certificate", "root-store", "crl", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Restricts CRL retrieval through Protected Roots; may affect non-admin CRL checking in some scenarios.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "ProtectedRootsAllowedToPerformCRLRetrieval", 0)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "ProtectedRootsAllowedToPerformCRLRetrieval")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "ProtectedRootsAllowedToPerformCRLRetrieval", 0)],
                },
                new TweakDef
                {
                    Id = "adcspol-disable-auto-root-update",
                    Label = "Disable Automatic Root Certificate Update",
                    Category = "Security",
                    Description =
                        "Sets DisableRootAutoUpdate=1 in the Cryptography policy. Prevents Windows from automatically downloading and installing new trusted root certificates from the Windows Update Certificate Distribution Point (CDP). In air-gapped or highly regulated environments this prevents silent addition of untrusted or government-mandated CA certificates. Organisations must manage root store updates manually through their own PKI if this is enabled.",
                    Tags = ["adcs", "certificate", "root-update", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Blocks automatic root cert updates from Windows Update; manage root store manually to avoid revocation issues.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "DisableRootAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "DisableRootAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "DisableRootAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "adcspol-enforce-strong-key-protection",
                    Label = "Enforce Strong Private Key Protection",
                    Category = "Security",
                    Description =
                        "Sets ForceKeyProtection=2 in the Cryptography policy. Requires a user password confirmation every time a certificate's private key is accessed by an application. Values: 0=no protection, 1=notify on first use, 2=require password for every use. Level 2 ensures that private key operations cannot happen silently in the background, protecting against malware that attempts to sign data or decrypt sensitive content using stored keys.",
                    Tags = ["adcs", "certificate", "private-key", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prompts for key password on every private-key operation; may interrupt automated signing workflows.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "ForceKeyProtection", 2)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "ForceKeyProtection")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "ForceKeyProtection", 2)],
                },
                new TweakDef
                {
                    Id = "adcspol-smartcard-require-logon",
                    Label = "Require Smart Card for Interactive Logon",
                    Category = "Security",
                    Description =
                        "Sets RequireSignOrSeal=1 in the SmartCardCredentialProvider policy. Enforces the Require Smart Card for Interactive Logon policy, preventing users from logging on with a password and requiring a physical smart card or Windows Hello for Business credential instead. This is the strongest form of phishing-resistant multi-factor authentication available natively in Windows.",
                    Tags = ["adcs", "smart-card", "mfa", "logon", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "Forces smart card logon; users without cards or Windows Hello devices will be locked out.",
                    ApplyOps = [RegOp.SetDword(SmartKey, "RequireSignOrSeal", 1)],
                    RemoveOps = [RegOp.DeleteValue(SmartKey, "RequireSignOrSeal")],
                    DetectOps = [RegOp.CheckDword(SmartKey, "RequireSignOrSeal", 1)],
                },
                new TweakDef
                {
                    Id = "adcspol-smartcard-pin-blocking",
                    Label = "Enable Smart Card PIN Lock After 5 Failures",
                    Category = "Security",
                    Description =
                        "Sets PINLockAfterFailedAttempts=5 in the SmartCardCredentialProvider policy. Automatically locks the smart card after 5 consecutive incorrect PIN entries, requiring an administrative PUK unlock. This prevents brute-force PIN guessing attacks where an attacker physically possesses the card and attempts to guess the PIN. Five attempts is the CIS Benchmark recommended maximum.",
                    Tags = ["adcs", "smart-card", "pin", "lockout", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Locks card after 5 wrong PINs; legitimate users should not exceed 5 attempts under normal usage.",
                    ApplyOps = [RegOp.SetDword(SmartKey, "PINLockAfterFailedAttempts", 5)],
                    RemoveOps = [RegOp.DeleteValue(SmartKey, "PINLockAfterFailedAttempts")],
                    DetectOps = [RegOp.CheckDword(SmartKey, "PINLockAfterFailedAttempts", 5)],
                },
                new TweakDef
                {
                    Id = "adcspol-disable-keyarchival",
                    Label = "Disable Certificate Key Archival to CA",
                    Category = "Security",
                    Description =
                        "Sets DisableKeyArchival=1 in the CertSvc policy key. Prevents Windows from automatically archiving private keys to the Certificate Authority during certificate enrollment. Key archival allows CA administrators to recover encrypted data if a user loses their key, but it also means private keys leave the user's device and are stored on (and thus accessible to) the CA server. Disabling archival keeps private keys on the user's device only.",
                    Tags = ["adcs", "certificate", "key-archival", "policy", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "No CA-assisted recovery if user loses their private key; weigh against key escrow requirements.",
                    ApplyOps = [RegOp.SetDword(CertSvcKey, "DisableKeyArchival", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertSvcKey, "DisableKeyArchival")],
                    DetectOps = [RegOp.CheckDword(CertSvcKey, "DisableKeyArchival", 1)],
                },
                new TweakDef
                {
                    Id = "adcspol-disable-cert-pub-ldap",
                    Label = "Disable Certificate Auto-Publication to LDAP",
                    Category = "Security",
                    Description =
                        "Sets DisableLdapCertPublish=1 in the CertSvc policy. Prevents certificate enrollment from automatically publishing user or machine certificates to the LDAP directory (Active Directory). In high-security environments where only specific certificates should be visible in LDAP, disabling auto-publication prevents unnecessary certificate disclosure that could aid LDAP enumeration.",
                    Tags = ["adcs", "certificate", "ldap", "publish", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Stops LDAP cert publication; smart card logon and S/MIME directory lookup may require manual cert publishing.",
                    ApplyOps = [RegOp.SetDword(CertSvcKey, "DisableLdapCertPublish", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertSvcKey, "DisableLdapCertPublish")],
                    DetectOps = [RegOp.CheckDword(CertSvcKey, "DisableLdapCertPublish", 1)],
                },
                new TweakDef
                {
                    Id = "adcspol-enable-cng-policy",
                    Label = "Enforce CNG Algorithm Policy (Block Legacy Crypto)",
                    Category = "Security",
                    Description =
                        "Sets EnforceCNGAlgorithmPolicy=1 in the Cryptography policy. Instructs Windows CNG (Cryptography Next Generation) to apply the configured algorithm policy when evaluating certificate signatures and key operations. When combined with a restricted algorithm suite policy, this prevents applications from using deprecated or weak cryptographic algorithms (MD5, SHA-1 RSA-1024) in certificate operations.",
                    Tags = ["adcs", "cryptography", "cng", "algorithm", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enforces algorithm policy; applications using legacy algorithms for cert operations may fail validation.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "EnforceCNGAlgorithmPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "EnforceCNGAlgorithmPolicy")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "EnforceCNGAlgorithmPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "adcspol-disable-cert-enrollment-ui",
                    Label = "Disable Certificate Enrollment UI for Non-Admins",
                    Category = "Security",
                    Description =
                        "Sets DisableEnrollmentUI=1 in the CertSvc policy. Prevents the certificate enrollment wizard UI from being accessed by non-administrative users. Restricting certificate enrollment to administrators ensures that only properly authorised and audited enrollment workflows generate new certificates, preventing users from self-enrolling unapproved certificate types from the CA.",
                    Tags = ["adcs", "certificate", "enrollment", "ui", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks self-service certificate enrollment UI; admin-managed enrolled certs are unaffected.",
                    ApplyOps = [RegOp.SetDword(CertSvcKey, "DisableEnrollmentUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(CertSvcKey, "DisableEnrollmentUI")],
                    DetectOps = [RegOp.CheckDword(CertSvcKey, "DisableEnrollmentUI", 1)],
                },
                new TweakDef
                {
                    Id = "adcspol-require-cert-chain-validation",
                    Label = "Require Full Certificate Chain Validation",
                    Category = "Security",
                    Description =
                        "Sets RequireChainValidation=1 in the Cryptography policy. Forces Windows to perform complete certificate chain validation (root, intermediates, and end-entity) before accepting any certificate as trusted. Disabling partial chain validation prevents applications from accepting certificates whose intermediate CA is untrusted or expired, closing a common misconfiguration that allows self-signed or invalid certificates to be accepted in some code paths.",
                    Tags = ["adcs", "certificate", "chain", "validation", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enforces full chain validation; certificates with missing or expired intermediates will be rejected.",
                    ApplyOps = [RegOp.SetDword(CryptoKey, "RequireChainValidation", 1)],
                    RemoveOps = [RegOp.DeleteValue(CryptoKey, "RequireChainValidation")],
                    DetectOps = [RegOp.CheckDword(CryptoKey, "RequireChainValidation", 1)],
                },
            ];
    }
}

// ── merged from Firewall.cs ──
internal static class Firewall
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fw-enable-all-profiles",
            Label = "Enable Firewall on All Profiles",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Defender Firewall on Domain, Private, and Public profiles. Default: enabled. Ensures firewall is not quietly disabled.",
            Tags = ["firewall", "security", "enable", "profiles"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "EnableFirewall",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "EnableFirewall",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "EnableFirewall",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "EnableFirewall"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "EnableFirewall"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "EnableFirewall"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "EnableFirewall",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-inbound-public",
            Label = "Block All Inbound on Public Profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks all inbound connections on the public network profile (including those in the allowed apps list). Maximum security on untrusted networks. Default: not blocked.",
            Tags = ["firewall", "security", "inbound", "public", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DoNotAllowExceptions",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-logging-success",
            Label = "Enable Firewall Logging (Successful Connections)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of successful connections across all profiles. Useful for security auditing and forensics. Default: not logged.",
            Tags = ["firewall", "logging", "security", "audit", "connections"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-increase-log-size",
            Label = "Increase Firewall Log Size to 32 MB",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the firewall log file maximum size to 32768 KB (32 MB) across all profiles. Default: 4096 KB (4 MB).",
            Tags = ["firewall", "logging", "size", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogFileSize",
                    32768
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileSize",
                    32768
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    32768
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogFileSize",
                    4096
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileSize",
                    4096
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    4096
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogFileSize",
                    32768
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-notifications-public",
            Label = "Disable Firewall Notifications on Public",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses firewall notification popups when apps are blocked on the public profile. Reduces noise on servers and kiosks. Default: notifications shown.",
            Tags = ["firewall", "notifications", "public", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableNotifications",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableNotifications",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableNotifications",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-default-inbound-block",
            Label = "Default Inbound Action: Block",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the default inbound action to Block on all profiles. Only explicitly allowed connections pass through. Default: Block (ensures it stays that way).",
            Tags = ["firewall", "security", "inbound", "block", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultInboundAction",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DefaultInboundAction",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultInboundAction",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultInboundAction"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DefaultInboundAction"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultInboundAction"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultInboundAction",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-stealth-mode",
            Label = "Enable Stealth Mode (Ignore Unsolicited)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables stealth mode on the public profile — the computer does not respond to unsolicited requests and is invisible to port scans. Default: varies.",
            Tags = ["firewall", "security", "stealth", "port-scan", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DisableStealthMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DisableStealthMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DisableStealthMode", 0)],
        },
        new TweakDef
        {
            Id = "fw-disable-unicast-response",
            Label = "Disable Unicast Response to Multicast",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the computer from sending unicast responses to multicast or broadcast messages. Reduces attack surface. Default: allowed.",
            Tags = ["firewall", "security", "multicast", "broadcast", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-inbound-domain",
            Label = "Block All Inbound on Domain Profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks all inbound connections on the domain profile. Provides maximum protection but may break domain services. Default: not blocked.",
            Tags = ["firewall", "security", "inbound", "domain", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DoNotAllowExceptions",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-inbound-private",
            Label = "Block All Inbound on Private Profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks all inbound connections on the private/home profile. Stricter than default which allows exceptions. Default: not blocked.",
            Tags = ["firewall", "security", "inbound", "private", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DoNotAllowExceptions",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-notifications-domain",
            Label = "Disable Firewall Notifications (Domain)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables firewall notification popups on the domain profile. Reduces distractions in managed environments. Default: enabled.",
            Tags = ["firewall", "notifications", "domain", "quiet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DisableNotifications",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DisableNotifications",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DisableNotifications",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-stealth-mode-private",
            Label = "Disable Stealth Mode (Private Profile)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables stealth mode on the private profile. Stealth mode prevents the computer from responding to PING and other probing requests. Disabling makes the machine discoverable on home networks. Default: off.",
            Tags = ["firewall", "stealth", "private", "discovery", "ping"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableStealthMode",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableStealthMode",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableStealthMode",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-stealth-mode-public",
            Label = "Enable Stealth Mode (Public Profile)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables stealth mode on the public profile. Makes the computer invisible to port scanners and PING sweeps on untrusted networks. Default: varies.",
            Tags = ["firewall", "stealth", "public", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableStealthMode",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableStealthMode"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableStealthMode",
                    0
                ),
            ],
        },
        // ── Sprint 18 — 10 new Firewall tweaks ────────────────────────────
        new TweakDef
        {
            Id = "fw-disable-multicast-broadcast-response",
            Label = "Disable Multicast/Broadcast Response (All Profiles)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the firewall from responding to multicast and broadcast traffic on all profiles. Reduces network visibility. Default: respond.",
            Tags = ["firewall", "multicast", "broadcast", "stealth", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableUnicastResponsesToMulticastBroadcast"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-domain-profile-logging",
            Label = "Enable Domain Profile Firewall Logging",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables dropped packet and successful connection logging for the domain firewall profile. Default: logging off.",
            Tags = ["firewall", "logging", "domain", "audit", "monitoring"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-outbound-default-public",
            Label = "Default-Block Outbound on Public Profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets default outbound action to block on the public firewall profile. Only explicitly allowed apps can send traffic. Default: allow.",
            Tags = ["firewall", "outbound", "public", "block", "restrictive"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-netbios-inbound",
            Label = "Block NetBIOS Inbound (All Profiles)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks inbound NetBIOS name service (UDP 137-138) via firewall policy. Reduces SMB relay and NBNS poisoning risk. Default: allowed.",
            Tags = ["firewall", "netbios", "inbound", "smb", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockNetBIOS-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=17|LPort=137-138|Name=RegiLattice Block NetBIOS Inbound|"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockNetBIOS-In"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockNetBIOS-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=17|LPort=137-138|Name=RegiLattice Block NetBIOS Inbound|"
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-smb-inbound-public",
            Label = "Block SMB Inbound on Public Profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks inbound SMB (TCP 445) on the public firewall profile. Prevents remote file sharing exploits on untrusted networks. Default: allowed.",
            Tags = ["firewall", "smb", "inbound", "public", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockSMB-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=445|Profile=Public|Name=RegiLattice Block SMB Public Inbound|"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockSMB-Public-In"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockSMB-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=445|Profile=Public|Name=RegiLattice Block SMB Public Inbound|"
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-firewall-notifications",
            Label = "Disable Firewall Notification Pop-ups",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses Windows Firewall notification pop-ups when a program is blocked. Default: notifications shown.",
            Tags = ["firewall", "notifications", "popup", "quiet", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-set-log-max-size",
            Label = "Increase Firewall Log Max Size to 32 MB",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum firewall log file size from 4 MB (default) to 32 MB for better audit trail retention.",
            Tags = ["firewall", "logging", "log-size", "audit", "retention"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileMaxSize",
                    32768
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileMaxSize",
                    4096
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileMaxSize",
                    32768
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-rpc-inbound-public",
            Label = "Block RPC Inbound on Public Profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks inbound RPC (TCP 135) on the public firewall profile. Prevents DCOM/RPC exploits on untrusted networks. Default: allowed.",
            Tags = ["firewall", "rpc", "dcom", "inbound", "public", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockRPC-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=135|Profile=Public|Name=RegiLattice Block RPC Public Inbound|"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockRPC-Public-In"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockRPC-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=135|Profile=Public|Name=RegiLattice Block RPC Public Inbound|"
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-local-policy-merge-domain",
            Label = "Prevent local firewall policy override in Domain profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "domain", "policy-merge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalPolicyMerge", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalPolicyMerge")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-local-policy-merge-private",
            Label = "Prevent local firewall policy override in Private profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "private", "policy-merge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "AllowLocalPolicyMerge", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "AllowLocalPolicyMerge"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "AllowLocalPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-local-policy-merge-public",
            Label = "Prevent local firewall policy override in Public profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "public", "policy-merge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalPolicyMerge", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalPolicyMerge")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-inbound-block-domain",
            Label = "Block all inbound connections in Domain profile via GPO",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "inbound", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "DefaultInboundAction")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "DefaultInboundAction", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-inbound-block-private",
            Label = "Block all inbound connections in Private profile via GPO",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "inbound", "private"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultInboundAction")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultInboundAction", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-size-domain",
            Label = "Maximize firewall log size for Domain profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "LogFileSize", 32767),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "LogFileSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "LogFileSize", 32767),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-size-private",
            Label = "Maximize firewall log size for Private profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "private"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging", "LogFileSize", 32767),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging", "LogFileSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging", "LogFileSize", 32767),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-dropped-domain",
            Label = "Enable logging of dropped packets in Domain profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "dropped", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "EnableLogDroppedPackets", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "EnableLogDroppedPackets"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-dropped-private",
            Label = "Enable logging of dropped packets in Private profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "dropped", "private"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging",
                    "EnableLogDroppedPackets",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging",
                    "EnableLogDroppedPackets"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging",
                    "EnableLogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-success-domain",
            Label = "Enable logging of successful connections in Domain profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "success", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogSuccessfulConnections"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-success-public",
            Label = "Enable logging of successful connections in Public profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "success", "public"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging",
                    "EnableLogSuccessfulConnections"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-outbound-block-private",
            Label = "Block outbound connections by default in Private profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["firewall", "gpo", "outbound", "private"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultOutboundAction", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultOutboundAction"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultOutboundAction", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-exceptions-public",
            Label = "Do not allow firewall exceptions in Public profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "exceptions", "public"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DoNotAllowExceptions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DoNotAllowExceptions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DoNotAllowExceptions", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-exceptions-private",
            Label = "Do not allow firewall exceptions in Private profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "exceptions", "private"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DoNotAllowExceptions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DoNotAllowExceptions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DoNotAllowExceptions", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-ipsec-merge-domain",
            Label = "Prevent local IPsec policy override in Domain profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "ipsec", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalIPsecPolicyMerge"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-ipsec-merge-public",
            Label = "Prevent local IPsec policy override in Public profile",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "ipsec", "public"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalIPsecPolicyMerge"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-enable-domain-profile",
            Label = "Enable Windows Firewall for Domain profile via GPO",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "enable", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "EnableFirewall", 1)],
        },
    ];
}

