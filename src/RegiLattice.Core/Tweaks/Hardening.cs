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
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
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
            Description = "Restricts outgoing NTLM authentication to audit mode first, then deny all. Improves security posture against relay attacks.",
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
            Description = "Forces Address Space Layout Randomization for all executables, including those not compiled with ASLR. CIS benchmark recommendation.",
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
                    "(Get-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\LanManServer\\Parameters' -Name NullSessionPipes -ErrorAction SilentlyContinue).NullSessionPipes.Count");
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
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictRemoteSAM", @"O:BAG:BAD:(A;;RC;;;BA)")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy", 0)],
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
            ApplyAction = _ => ShellRunner.RunPowerShell(
                "Set-SmbServerConfiguration -EnableSMB1Protocol $false -Force; " +
                "Disable-WindowsOptionalFeature -Online -FeatureName SMB1Protocol -NoRestart -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell(
                "Set-SmbServerConfiguration -EnableSMB1Protocol $true -Force"),
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
            ApplyAction = _ => ShellRunner.RunPowerShell(
                "Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled True"),
            RemoveAction = _ => { }, // Don't disable firewall
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-NetFirewallProfile | Where-Object Enabled -eq $false).Count -eq 0");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}
