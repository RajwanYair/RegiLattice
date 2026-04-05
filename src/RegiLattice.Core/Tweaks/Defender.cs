namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
