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
    ];
}
