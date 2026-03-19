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
            Id = "harden-enable-credential-guard",
            Label = "Enable Credential Guard (UEFI Lock)",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Windows Credential Guard to protect domain credentials in isolated containers. Requires Secure Boot and Hyper-V.",
            Tags = ["hardening", "credential-guard", "security", "enterprise"],
            SideEffects = "May break some legacy authentication protocols.",
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "RequirePlatformSecurityFeatures", 3),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1)],
        },
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
            Id = "harden-enable-lsa-protection",
            Label = "Enable LSA Protection (RunAsPPL)",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Runs LSASS as a Protected Process Light to prevent credential dumping attacks.",
            Tags = ["hardening", "security", "lsa", "credential"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
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
            Id = "harden-enable-safe-search-mode",
            Label = "Enable Safe DLL Search Mode (CWD Last)",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Places the current directory last in the DLL search order to mitigate DLL preloading/hijacking attacks.",
            Tags = ["hardening", "security", "dll", "hijacking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "SafeDllSearchMode", 1)],
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
            Id = "harden-disable-remote-assistance",
            Label = "Disable Remote Assistance",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Remote Assistance to prevent unauthorised remote sessions.",
            Tags = ["hardening", "security", "remote", "assistance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "harden-enable-smb-signing",
            Label = "Require SMB Signing (Client)",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires SMB packet signing on the client side to prevent relay and MitM attacks.",
            Tags = ["hardening", "security", "smb", "signing"],
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
            Id = "harden-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LLMNR which is vulnerable to poisoning/relay attacks (Responder, Inveigh).",
            Tags = ["hardening", "security", "llmnr", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
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
            Description = "Disables Web Proxy Auto-Discovery Protocol service. WPAD can be exploited for MITM attacks on untrusted networks. Default: enabled.",
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
            Description = "Sets LAN Manager authentication level to 'Send NTLMv2 response only. Refuse LM & NTLM'. Prevents weak hash capture. Default: negotiate.",
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableExceptionChainValidation", 0)],
        },
        new TweakDef
        {
            Id = "harden-enable-aslr-bottom-up",
            Label = "Enable Mandatory ASLR (Bottom-Up Randomisation)",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces all images to be randomised regardless of /DYNAMICBASE flag. Hardens against ROP/JOP attacks. Default: opt-in only.",
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
            Description = "Ensures Control Flow Guard is enabled system-wide. CFG prevents exploitation of indirect call targets. Default: on in modern Windows.",
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
            Description = "Disables AutoPlay/AutoRun on all drive types including USB. Prevents malware auto-execution from removable media. Default: enabled for some drives.",
            Tags = ["hardening", "autoplay", "autorun", "usb", "malware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
        },
        new TweakDef
        {
            Id = "harden-restrict-named-pipe-access",
            Label = "Restrict Anonymous Access to Named Pipes",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts anonymous access to named pipes. Reduces lateral movement and IPC exploitation risk. Default: some pipes accessible.",
            Tags = ["hardening", "named-pipes", "anonymous", "lateral-movement", "ipc"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RestrictNullSessAccess", 1)],
        },
        new TweakDef
        {
            Id = "harden-block-ntlm-outgoing-traffic",
            Label = "Block Outgoing NTLM Traffic to Remote Servers",
            Category = "Hardening",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks outgoing NTLM authentication to remote servers. Forces Kerberos or modern auth. Prevents credential relay. Default: allow all.",
            Tags = ["hardening", "ntlm", "kerberos", "credential-relay", "authentication"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 2)],
        },
    ];
}
