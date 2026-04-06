namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// User Account Control (UAC) and account policy tweaks — configures UAC behaviour,
/// account lockout policies, auto-login, and credential management.
/// </summary>
internal static class UserAccount
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string UacKey = $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "uac-disable-dimming",
            Label = "Disable UAC Screen Dimming",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the secure-desktop dimming when a UAC prompt appears. Prompts still appear but without switching the desktop.",
            Tags = ["uac", "security", "performance", "desktop"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "PromptOnSecureDesktop", 0)],
            RemoveOps = [RegOp.SetDword(UacKey, "PromptOnSecureDesktop", 1)],
            DetectOps = [RegOp.CheckDword(UacKey, "PromptOnSecureDesktop", 0)],
        },
        new TweakDef
        {
            Id = "uac-set-silent-admin",
            Label = "UAC: Auto-Elevate for Admins (No Prompt)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures UAC to automatically elevate admin users without prompting. Reduces interruptions for single-user systems.",
            Tags = ["uac", "security", "admin", "elevation"],
            SideEffects = "Malware could silently elevate. Use only on personal systems.",
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "ConsentPromptBehaviorAdmin", 0)],
            RemoveOps = [RegOp.SetDword(UacKey, "ConsentPromptBehaviorAdmin", 5)],
            DetectOps = [RegOp.CheckDword(UacKey, "ConsentPromptBehaviorAdmin", 0)],
        },
        new TweakDef
        {
            Id = "uac-set-account-lockout-10",
            Label = "Set Account Lockout After 10 Attempts",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Configures the local account lockout policy to lock out after 10 invalid password attempts.",
            Tags = ["uac", "security", "lockout", "password"],
            ApplyAction = _ => ShellRunner.Run("net.exe", ["accounts", "/lockoutthreshold:10"]),
            RemoveAction = _ => ShellRunner.Run("net.exe", ["accounts", "/lockoutthreshold:0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("net.exe", ["accounts"]);
                return stdout.Contains("10", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "uac-set-password-length-8",
            Label = "Set Minimum Password Length to 8",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets the minimum password length for local accounts to 8 characters.",
            Tags = ["uac", "security", "password", "policy"],
            ApplyAction = _ => ShellRunner.Run("net.exe", ["accounts", "/minpwlen:8"]),
            RemoveAction = _ => ShellRunner.Run("net.exe", ["accounts", "/minpwlen:0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("net.exe", ["accounts"]);
                var lines = stdout.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("Minimum password length", StringComparison.OrdinalIgnoreCase) && line.Contains("8", StringComparison.Ordinal))
                        return true;
                }
                return false;
            },
        },
        new TweakDef
        {
            Id = "uac-set-lockout-duration-30",
            Label = "Set Account Lockout Duration to 30 Minutes",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets the account lockout duration to 30 minutes after the threshold is reached.",
            Tags = ["uac", "security", "lockout", "password", "duration"],
            ApplyAction = _ => ShellRunner.Run("net.exe", ["accounts", "/lockoutduration:30"]),
            RemoveAction = _ => ShellRunner.Run("net.exe", ["accounts", "/lockoutduration:0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("net.exe", ["accounts"]);
                var lines = stdout.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("Lockout duration", StringComparison.OrdinalIgnoreCase) && line.Contains("30", StringComparison.Ordinal))
                        return true;
                }
                return false;
            },
        },
        new TweakDef
        {
            Id = "uac-set-max-password-age-90",
            Label = "Set Maximum Password Age to 90 Days",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets the maximum password age for local accounts to 90 days, requiring periodic password changes.",
            Tags = ["uac", "security", "password", "policy", "expiration"],
            ApplyAction = _ => ShellRunner.Run("net.exe", ["accounts", "/maxpwage:90"]),
            RemoveAction = _ => ShellRunner.Run("net.exe", ["accounts", "/maxpwage:unlimited"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("net.exe", ["accounts"]);
                var lines = stdout.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("Maximum password age", StringComparison.OrdinalIgnoreCase) && line.Contains("90", StringComparison.Ordinal))
                        return true;
                }
                return false;
            },
        },
        new TweakDef
        {
            Id = "uac-elevate-signed-only",
            Label = "Only Elevate Signed and Validated Executables",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures UAC to only elevate digitally signed executables. Blocks unsigned programs from gaining admin rights.",
            Tags = ["uac", "security", "elevation", "signing", "hardening"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "ValidateAdminCodeSignatures", 1)],
            RemoveOps = [RegOp.SetDword(UacKey, "ValidateAdminCodeSignatures", 0)],
            DetectOps = [RegOp.CheckDword(UacKey, "ValidateAdminCodeSignatures", 1)],
        },
        new TweakDef
        {
            Id = "uac-standard-user-prompt-credentials",
            Label = "Standard Users Must Enter Admin Credentials",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures UAC to prompt standard users for admin credentials (not just deny). Allows controlled elevation.",
            Tags = ["uac", "security", "elevation", "standard-user"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "ConsentPromptBehaviorUser", 1)],
            RemoveOps = [RegOp.SetDword(UacKey, "ConsentPromptBehaviorUser", 0)],
            DetectOps = [RegOp.CheckDword(UacKey, "ConsentPromptBehaviorUser", 1)],
        },
        new TweakDef
        {
            Id = "uac-disable-account-picture",
            Label = "Disable Account Picture Change by Users",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents standard users from changing their account picture, keeping a consistent corporate appearance.",
            Tags = ["uac", "account", "policy", "corporate"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoChangingWallPaper", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoChangingWallPaper")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoChangingWallPaper", 1)],
        },
        new TweakDef
        {
            Id = "uac-disable-guest-account",
            Label = "Disable Guest Account",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in Guest account, preventing unauthenticated local access to the machine.",
            Tags = ["uac", "security", "guest", "hardening"],
            ApplyAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.Run("net", ["user", "Guest", "/active:no"]);
            },
            RemoveAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.Run("net", ["user", "Guest", "/active:yes"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("net", ["user", "Guest"]);
                return stdout.Contains("Account active               No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "uac-enforce-password-complexity",
            Label = "Enforce Password Complexity",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces password complexity requirements (uppercase, lowercase, digits, special chars) via local security policy.",
            Tags = ["uac", "password", "security", "policy", "hardening"],
            ApplyAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.RunPowerShell(
                        "$nl = [System.Environment]::NewLine; "
                            + "$inf = \"[Unicode]${nl}Unicode=yes${nl}[System Access]${nl}PasswordComplexity = 1${nl}\"; "
                            + "$path = \"$env:TEMP\\rl_pwcomplex_on.inf\"; "
                            + "[System.IO.File]::WriteAllText($path, $inf, [System.Text.Encoding]::Unicode); "
                            + "secedit /configure /db \"$env:TEMP\\secpol.sdb\" /cfg $path /areas SECURITYPOLICY /quiet; "
                            + "Remove-Item $path -ErrorAction SilentlyContinue"
                    );
            },
            RemoveAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.RunPowerShell(
                        "$nl = [System.Environment]::NewLine; "
                            + "$inf = \"[Unicode]${nl}Unicode=yes${nl}[System Access]${nl}PasswordComplexity = 0${nl}\"; "
                            + "$path = \"$env:TEMP\\rl_pwcomplex_off.inf\"; "
                            + "[System.IO.File]::WriteAllText($path, $inf, [System.Text.Encoding]::Unicode); "
                            + "secedit /configure /db \"$env:TEMP\\secpol.sdb\" /cfg $path /areas SECURITYPOLICY /quiet; "
                            + "Remove-Item $path -ErrorAction SilentlyContinue"
                    );
            },
            DetectAction = () =>
            {
                var cfgPath = Path.Combine(Path.GetTempPath(), "rl_secedit_export.cfg");
                ShellRunner.Run("secedit", ["/export", "/cfg", cfgPath, "/quiet"]);
                if (!File.Exists(cfgPath))
                    return false;
                try
                {
                    var content = File.ReadAllText(cfgPath, System.Text.Encoding.Unicode);
                    return content.Contains("PasswordComplexity = 1", StringComparison.OrdinalIgnoreCase);
                }
                finally
                {
                    File.Delete(cfgPath);
                }
            },
        },
        new TweakDef
        {
            Id = "uac-disable-offline-files",
            Label = "Disable Offline Files (Client-Side Caching)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Offline Files (CSC), reducing background syncing and disk usage for non-domain machines.",
            Tags = ["uac", "offline-files", "csc", "network", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\CSC\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\CSC\Parameters", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\CSC\Parameters", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\CSC\Parameters", "Start", 4)],
        },
        new TweakDef
        {
            Id = "uac-disable-linked-connections",
            Label = "Disable Linked Connections (Admin Share Elevation)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the OS from creating elevated duplicate network connections for admin tokens, stopping credential elevation leakage over the network.",
            Tags = ["uac", "network", "admin", "security"],
            SideEffects = "May break mapped drives when opened from an elevated process.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLinkedConnections", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLinkedConnections")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLinkedConnections", 0)],
        },
        new TweakDef
        {
            Id = "uac-disable-startup-sound",
            Label = "Disable Windows Startup Sound (Policy)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableStartupSound=1 in the System policy key. Permanently silences the Windows boot sound at the Group Policy level, overriding the per-user audio preference.",
            Tags = ["uac", "startup", "sound", "policy"],
            RegistryKeys = [$@"{UacKey}"],
            ApplyOps = [RegOp.SetDword($@"{UacKey}", "DisableStartupSound", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{UacKey}", "DisableStartupSound")],
            DetectOps = [RegOp.CheckDword($@"{UacKey}", "DisableStartupSound", 1)],
        },
        new TweakDef
        {
            Id = "uac-show-last-logon-info",
            Label = "Show Last Logon Information at Login Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisplayLastLogonInfo=1 in Winlogon. Displays the previous logon date/time and failed logon attempts on the Windows logon screen, allowing users to spot unauthorised access attempts.",
            Tags = ["uac", "logon", "security", "audit"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DisplayLastLogonInfo", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DisplayLastLogonInfo", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DisplayLastLogonInfo", 1)],
        },
        new TweakDef
        {
            Id = "uac-restrict-anonymous-lsa",
            Label = "Restrict Anonymous LSA Enumeration",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictAnonymousLSA=1 in the LSA key. Prevents anonymous (unauthenticated) callers from enumerating accounts and enumerable information through the LSA policy interface.",
            Tags = ["uac", "lsa", "anonymous", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousLSA", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousLSA", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymousLSA", 1)],
        },
        new TweakDef
        {
            Id = "uac-enable-print-drivers-admin-only",
            Label = "Restrict Printer Driver Installation to Admins Only",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AddPrinterDrivers=1 in LanMan Print Services Servers. Requires administrator privileges to install new printer drivers, preventing malicious driver installs via the print spooler (PrintNightmare class).",
            Tags = ["uac", "printer", "driver", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Print\Providers\LanMan Print Services\Servers"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Print\Providers\LanMan Print Services\Servers", "AddPrinterDrivers", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Print\Providers\LanMan Print Services\Servers", "AddPrinterDrivers", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Print\Providers\LanMan Print Services\Servers", "AddPrinterDrivers", 1),
            ],
        },
        new TweakDef
        {
            Id = "uac-dont-display-domain-name",
            Label = "Hide Domain Name on Logon Screen",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontDisplayDomainName=1 in the System policy. Removes the domain name prefix from the username field on the Windows logon screen, reducing information leakage about domain membership.",
            Tags = ["uac", "logon", "domain", "privacy"],
            RegistryKeys = [$@"{UacKey}"],
            ApplyOps = [RegOp.SetDword($@"{UacKey}", "DontDisplayDomainName", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{UacKey}", "DontDisplayDomainName")],
            DetectOps = [RegOp.CheckDword($@"{UacKey}", "DontDisplayDomainName", 1)],
        },
    ];
}
