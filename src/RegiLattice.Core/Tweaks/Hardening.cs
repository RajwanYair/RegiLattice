namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Advanced system hardening tweaks — security-focused registry and group policy settings
/// that go beyond basic security. Includes CIS benchmark, STIG, and NSA recommendations.
/// </summary>
internal static class Hardening
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "harden-disable-wdigest",
            Label = "Disable WDigest Plaintext Caching",
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Id = "harden-disable-cached-logons",
            Label = "Limit Cached Logon Credentials to 2",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Limits cached domain logons stored on the machine from 10 (default) to 2, reducing credential theft risk.",
            Tags = ["hardening", "security", "credentials", "domain"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "CachedLogonsCount", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "CachedLogonsCount", "10")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "CachedLogonsCount", "2")],
        },
        new TweakDef
        {
            Id = "harden-disable-admin-shares",
            Label = "Disable Administrative Shares (C$, ADMIN$)",
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Id = "harden-disable-remote-registry",
            Label = "Disable Remote Registry Service",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Remote Registry service to prevent remote access to the registry. Default: manual (enabled on demand).",
            Tags = ["hardening", "remote-registry", "service", "security", "attack-surface"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
        },
        new TweakDef
        {
            Id = "harden-enable-structured-exception-handling",
            Label = "Enable SEHOP (Structured Exception Handler Overwrite Protection)",
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Hardening",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
            Category = "Smart App Control",
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
