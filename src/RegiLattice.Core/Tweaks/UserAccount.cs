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
            Id = "uac-disable-for-built-in-admin",
            Label = "Disable UAC for Built-in Administrator",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Runs the built-in Administrator account in Admin Approval Mode without UAC prompts.",
            Tags = ["uac", "security", "admin"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "FilterAdministratorToken", 0)],
            RemoveOps = [RegOp.SetDword(UacKey, "FilterAdministratorToken", 1)],
            DetectOps = [RegOp.CheckDword(UacKey, "FilterAdministratorToken", 0)],
        },
        new TweakDef
        {
            Id = "uac-enable-admin-approval-mode",
            Label = "Enable Admin Approval Mode",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Admin Approval Mode for all admin accounts. This is the standard UAC setting recommended by Microsoft.",
            Tags = ["uac", "security", "admin", "best-practice"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "EnableLUA", 1)],
            RemoveOps = [RegOp.SetDword(UacKey, "EnableLUA", 0)],
            DetectOps = [RegOp.CheckDword(UacKey, "EnableLUA", 1)],
        },
        new TweakDef
        {
            Id = "uac-virtualise-file-registry",
            Label = "Enable UAC File/Registry Virtualisation",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables UAC virtualisation which redirects write failures for legacy apps to per-user locations instead of System32 or HKLM.",
            Tags = ["uac", "compatibility", "virtualisation"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "EnableVirtualization", 1)],
            RemoveOps = [RegOp.SetDword(UacKey, "EnableVirtualization", 0)],
            DetectOps = [RegOp.CheckDword(UacKey, "EnableVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "uac-disable-auto-admin-logon",
            Label = "Disable Automatic Admin Logon",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically logging in as administrator. Requires manual credential entry at sign-in.",
            Tags = ["uac", "security", "logon", "admin"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetString($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoAdminLogon", "0")],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoAdminLogon")],
            DetectOps = [RegOp.CheckString($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoAdminLogon", "0")],
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
            Id = "uac-hide-last-username",
            Label = "Hide Last Logged-On Username at Sign-In",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the last user name on the login screen. Users must type both username and password.",
            Tags = ["uac", "security", "logon", "privacy"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLastUserName", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLastUserName", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLastUserName", 1)],
        },
        new TweakDef
        {
            Id = "uac-disable-credential-guard-lock-timeout",
            Label = "Disable Credential Guard Lock Timeout",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Credential Guard automatic lock timeout. Useful for development machines where frequent re-auth is annoying.",
            Tags = ["uac", "security", "credential-guard", "timeout"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "LsaCfgFlags", 0)],
        },
        new TweakDef
        {
            Id = "uac-require-ctrl-alt-del",
            Label = "Require Ctrl+Alt+Del at Logon",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires pressing Ctrl+Alt+Del before the logon screen appears. Prevents spoofed login screens.",
            Tags = ["uac", "security", "logon", "hardening"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableCAD", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableCAD", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableCAD", 0)],
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
            Id = "uac-restrict-blank-password",
            Label = "Restrict Blank Password to Console Only",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents accounts with blank passwords from being used for network logons. Only local console access is allowed.",
            Tags = ["uac", "security", "password", "blank", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "LimitBlankPasswordUse", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "LimitBlankPasswordUse", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "LimitBlankPasswordUse", 1)],
        },
        new TweakDef
        {
            Id = "uac-enable-installer-detection",
            Label = "Enable Installer Detection for UAC",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows installer detection which prompts for elevation when setup programs are detected.",
            Tags = ["uac", "security", "installer", "elevation"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "EnableInstallerDetection", 1)],
            RemoveOps = [RegOp.SetDword(UacKey, "EnableInstallerDetection", 0)],
            DetectOps = [RegOp.CheckDword(UacKey, "EnableInstallerDetection", 1)],
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
            Id = "uac-disable-remote-uac",
            Label = "Disable Remote UAC Filtering",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables remote UAC filtering so that remote admin connections get full admin tokens. Needed for some remote management tools.",
            Tags = ["uac", "remote", "admin", "management"],
            SideEffects = "Reduces security for remote admin sessions.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "LocalAccountTokenFilterPolicy", 1)],
        },
        new TweakDef
        {
            Id = "uac-enable-secure-desktop",
            Label = "Enable Secure Desktop for UAC Prompts",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures UAC prompts are displayed on the secure desktop, preventing other applications from interfering with the prompt.",
            Tags = ["uac", "security", "secure-desktop", "hardening"],
            RegistryKeys = [UacKey],
            ApplyOps = [RegOp.SetDword(UacKey, "PromptOnSecureDesktop", 1)],
            RemoveOps = [RegOp.SetDword(UacKey, "PromptOnSecureDesktop", 0)],
            DetectOps = [RegOp.CheckDword(UacKey, "PromptOnSecureDesktop", 1)],
        },
    ];
}
