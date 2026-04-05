namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
