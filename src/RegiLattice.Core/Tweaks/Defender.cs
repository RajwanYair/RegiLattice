namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationAuditOptions", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationAuditOptions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "MitigationAuditOptions", 0)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 50),
            ],
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
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access", "EnableControlledFolderAccess", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access", "EnableControlledFolderAccess", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access", "EnableControlledFolderAccess", 1)],
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
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection", "EnableNetworkProtection", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection", "EnableNetworkProtection", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection", "EnableNetworkProtection", 1)],
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
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR", "ExploitGuard_ASR_Rules", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR", "ExploitGuard_ASR_Rules", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR", "ExploitGuard_ASR_Rules", 1)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0)],
        },
        new TweakDef
        {
            Id = "sec-defender-cpu-limit",
            Label = "Reduce Defender CPU Usage",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits Windows Defender scan CPU usage to 25%. Prevents Defender from slowing down the system during scans. Options: 5-100. Default: 50. Recommended: 25.",
            Tags = ["security", "defender", "cpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 50),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "AvgCPULoadFactor", 25)],
        },
        new TweakDef
        {
            Id = "sec-defender-disable-nis",
            Label = "Disable Defender Network Inspection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Defender Network Inspection System (NIS) protocol analysis. Reduces CPU overhead from deep packet inspection. Default: Enabled. Recommended: Disabled if using third-party firewall.",
            Tags = ["security", "defender", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS", "DisableProtocolRecognition", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS", "DisableProtocolRecognition"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\NIS", "DisableProtocolRecognition", 1)],
        },
        new TweakDef
        {
            Id = "sec-disable-credential-guard",
            Label = "Disable Credential Guard",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Virtualization Based Security / Credential Guard. May improve performance. Default: Enabled. Recommended: Keep enabled.",
            Tags = ["security", "credential-guard", "vbs", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "AuditLogonEvents", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "AuditLogonEvents", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "AuditLogonEvents", 3)],
        },
        new TweakDef
        {
            Id = "sec-disable-pua-protection",
            Label = "Disable PUA Detection",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Potentially Unwanted Application (PUA) detection in Windows Defender via MpEnablePus policy. Default: Enabled. Recommended: Keep enabled for safety.",
            Tags = ["security", "defender", "pua", "detection", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", 1)],
        },
        new TweakDef
        {
            Id = "sec-disable-smbv1",
            Label = "Disable SMBv1 Protocol",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the legacy SMBv1 protocol on the server side. Mitigates WannaCry and EternalBlue vulnerabilities. Default: Enabled. Recommended: Disabled.",
            Tags = ["security", "smb", "smbv1", "protocol", "vulnerability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "SMB1", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-lsa-protection",
            Label = "Enable LSA Protection (RunAsPPL)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Local Security Authority (LSA) protection by running LSASS as a Protected Process Light (PPL). Mitigates credential theft. Default: Disabled. Recommended: Enabled.",
            Tags = ["security", "lsa", "lsass", "ppl", "credential"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "sec-enable-spectre-mitigations",
            Label = "Enable Spectre/Meltdown Mitigations",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures Spectre (variant 2) and Meltdown mitigations are enabled via FeatureSettingsOverride. May reduce performance on older CPUs. Default: usually enabled by Windows Update. Recommended: Enabled.",
            Tags = ["security", "spectre", "meltdown", "cpu", "vulnerability", "mitigations"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "FeatureSettingsOverride", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "FeatureSettingsOverrideMask", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "FeatureSettingsOverride"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "FeatureSettingsOverrideMask"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "FeatureSettingsOverride", 0)],
        },
        new TweakDef
        {
            Id = "sec-uac-always-notify",
            Label = "Set UAC to Always Notify (Highest Level)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets UAC to 'Always notify' (ConsentPromptBehaviorAdmin=2) — prompts for both Windows changes and other program elevation requests. Default: notify only for app changes (5). Recommended: Always notify.",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", 2)],
        },
        new TweakDef
        {
            Id = "sec-restrict-ntlmv1",
            Label = "Require NTLMv2 (Block LM and NTLMv1)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets LmCompatibilityLevel=5 to only use NTLMv2 and refuse LM/NTLMv1 responses. Hardens network authentication. May break legacy devices. Default: 3 (NTLMv2 only send). Recommended: 5 for hardened environments.",
            Tags = ["security", "ntlm", "ntlmv1", "authentication", "network", "lm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LmCompatibilityLevel", 5)],
        },
        new TweakDef
        {
            Id = "sec-disable-wdigest",
            Label = "Disable WDigest Authentication (Credential Hardening)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables WDigest authentication to prevent plain-text password storage in LSASS. Mitigates credential harvesting attacks via Mimikatz. Default: Enabled on older systems. Recommended: Disabled.",
            Tags = ["security", "wdigest", "lsass", "credential", "mimikatz"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest", "UseLogonCredential", 0)],
        },
        new TweakDef
        {
            Id = "sec-enable-cred-guard-policy",
            Label = "Enable Credential Guard via Policy",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Credential Guard via LsaCfgFlags=1, protecting LSASS credential secrets. Requires TPM 2.0 and Secure Boot. Default: Disabled. Recommended: Enabled on modern hardware.",
            Tags = ["security", "credential-guard", "lsa", "tpm", "secureboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1)],
        },
        new TweakDef
        {
            Id = "sec-enable-behavior-monitoring",
            Label = "Enable Defender Behavior Monitoring",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Defender behavior monitoring, which watches processes for suspicious activity patterns beyond signature-based detection. Default: Enabled. Recommended: Enabled.",
            Tags = ["security", "defender", "behavior", "monitoring", "heuristics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", 0)],
        },
        new TweakDef
        {
            Id = "sec-disable-autorun",
            Label = "Disable AutoRun for All Drive Types",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoRun/AutoPlay for all drive types via policy (NoDriveTypeAutoRun=255). Prevents malware spread via USB drives and CD-ROMs. Default: Partial. Recommended: Fully disabled.",
            Tags = ["security", "autorun", "autoplay", "usb", "policy", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
        },
        new TweakDef
        {
            Id = "sec-disable-remote-assistance",
            Label = "Disable Windows Remote Assistance",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Remote Assistance (fAllowToGetHelp=0). Eliminates a remote access vector not typically needed on workstations. Default: Enabled. Recommended: Disabled.",
            Tags = ["security", "remote-assistance", "rdp", "access", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"],
            SideEffects = "Windows Remote Assistance invitations cannot be sent or accepted.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp"),
            ],
        },
        new TweakDef
        {
            Id = "sec-enable-sehop",
            Label = "Enable SEHOP (Exception Chain Validation)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Structured Exception Handler Overwrite Protection (SEHOP). Protects against SEH-based exploitation techniques. Default: Disabled (on client SKUs). Recommended: Enabled.",
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
        },
        new TweakDef
        {
            Id = "sec-disable-admin-shares",
            Label = "Disable Automatic Administrative Shares",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic C$ and ADMIN$ administrative shares. Reduces lateral movement options for attackers on local networks. Default: Enabled. Recommended: Disabled on non-managed workstations.",
            Tags = ["security", "admin-shares", "smb", "lateral-movement", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            SideEffects = "Remote admin tools relying on C$ or ADMIN$ will fail.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "sec-restrict-cd-rom",
            Label = "Restrict CD-ROM to Logged-On User",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts CD-ROM drive access to the currently logged-on user only. Prevents remote users from accessing optical media on this machine. Default: Not restricted. Recommended: Restricted.",
            Tags = ["security", "cd-rom", "optical", "access-control", "winlogon"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AllocateCDRoms", "1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AllocateCDRoms"),
            ],
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
            ApplyOps = [RegOp.SetQword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 0x1000000000000)],
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
            Description = "Enables Windows Defender Credential Guard to isolate secrets using virtualization-based security. Requires Hyper-V. Default: disabled.",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit", "ProcessCreationIncludeCmdLine_Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit", "ProcessCreationIncludeCmdLine_Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit", "ProcessCreationIncludeCmdLine_Enabled", 1)],
        },
    ];
}
