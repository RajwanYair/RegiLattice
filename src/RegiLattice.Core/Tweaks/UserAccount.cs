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
            Id = "uac-disable-biometrics-policy",
            Label = "Disable Biometric Authentication Policy",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Biometric Service usage through Group Policy, preventing fingerprint and face login.",
            Tags = ["uac", "biometrics", "policy", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Biometrics"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Biometrics", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Biometrics", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Biometrics", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "uac-disable-smartcard-removal-lock",
            Label = "Disable Smart Card Removal Lock",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from locking the workstation when a smart card is removed from the reader.",
            Tags = ["uac", "smartcard", "policy", "lock"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetString($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ScRemoveOption", "0")],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ScRemoveOption")],
            DetectOps = [RegOp.CheckString($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ScRemoveOption", "0")],
        },
        new TweakDef
        {
            Id = "uac-disable-windows-hello-for-business",
            Label = "Disable Windows Hello for Business",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Hello for Business enrollment, preventing PIN/biometric login provisioning via Azure AD or AD.",
            Tags = ["uac", "hello", "pin", "azure-ad", "policy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "uac-disable-microsoft-account-logon",
            Label = "Disable Microsoft Account User Authentication",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks Microsoft (Live) accounts from being used for local logon, enforcing local-only or domain accounts.",
            Tags = ["uac", "microsoft-account", "policy", "security", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoConnectedUser", 3)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoConnectedUser")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoConnectedUser", 3)],
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
                    ShellRunner.Run(
                        "secedit",
                        ["/configure", "/db", "secedit.sdb", "/cfg", "%windir%\\inf\\defltbase.inf", "/areas", "SECURITYPOLICY", "/quiet"]
                    );
            },
            RemoveAction = _ => { },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("secedit", ["/export", "/cfg", "%temp%\\rl_secedit.cfg", "/quiet"]);
                return stdout.Contains("PasswordComplexity = 1", StringComparison.OrdinalIgnoreCase);
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
            Id = "uac-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables fast user switching from the logon screen, preventing users from switching sessions without signing out.",
            Tags = ["uac", "fast-user-switching", "policy", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1)],
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
            Id = "uac-set-machine-pw-max-age",
            Label = "Set Machine Account Password Maximum Age to 30 Days",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaximumPasswordAge=30 in Netlogon Parameters. Limits machine account password lifetime to 30 days, ensuring domain-joined computers regularly rotate their machine credentials.",
            Tags = ["uac", "machine-account", "password", "rotation"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "MaximumPasswordAge", 30)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "MaximumPasswordAge", 30)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "MaximumPasswordAge", 30)],
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
            Id = "uac-set-ntlm-min-server-security",
            Label = "Set NTLM Minimum Server Session Security (128-bit + NTLMv2)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NTLMMinServerSec=537395200 in LSA\\MSV1_0. Requires NTLMv2 session security and 128-bit encryption for all inbound NTLM server sessions, blocking weaker LM/NTLMv1 connections.",
            Tags = ["uac", "ntlm", "security", "encryption"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "NTLMMinServerSec", 537395200)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "NTLMMinServerSec", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "NTLMMinServerSec", 537395200)],
        },
        new TweakDef
        {
            Id = "uac-set-ntlm-min-client-security",
            Label = "Set NTLM Minimum Client Session Security (128-bit + NTLMv2)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NTLMMinClientSec=537395200 in LSA\\MSV1_0. Requires NTLMv2 session security and 128-bit encryption for all outbound NTLM client sessions.",
            Tags = ["uac", "ntlm", "security", "encryption"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "NTLMMinClientSec", 537395200)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "NTLMMinClientSec", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "NTLMMinClientSec", 537395200)],
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
        new TweakDef
        {
            Id = "uac-disable-credential-caching",
            Label = "Disable Domain Credential Caching (DisableDomainCreds)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets DisableDomainCreds=1 in the LSA key. Prevents Windows from caching domain credentials in the Credential Manager, so previous logon tokens cannot be reused if the machine is compromised.",
            Tags = ["uac", "lsa", "credentials", "security"],
            SideEffects = "Users will be prompted for credentials on every network resource access if domain controllers are unavailable.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "DisableDomainCreds", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "DisableDomainCreds", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "DisableDomainCreds", 1)],
        },
        new TweakDef
        {
            Id = "uac-enable-domain-channel-signing",
            Label = "Require Domain Secure Channel Signing",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RequireSignOrSeal=1 in Netlogon Parameters. Mandates digital signing or encryption on all domain secure channel communications, protecting against session hijacking attacks.",
            Tags = ["uac", "domain", "netlogon", "signing", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireSignOrSeal", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireSignOrSeal", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireSignOrSeal", 1)],
        },
        new TweakDef
        {
            Id = "uac-enable-domain-channel-seal",
            Label = "Require Domain Secure Channel Message Sealing",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SealSecureChannel=1 in Netlogon Parameters. Enforces encryption (sealing) on all domain secure channel data, preventing eavesdropping on machine-to-DC communications.",
            Tags = ["uac", "domain", "netlogon", "encryption", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SealSecureChannel", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SealSecureChannel", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "SealSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "uac-enable-domain-strong-key",
            Label = "Require Strong Session Key for Domain Secure Channel",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RequireStrongKey=1 in Netlogon Parameters. Forces the use of a strong (128-bit) session key for all domain secure channel traffic, rejecting machines that can only negotiate weaker keys.",
            Tags = ["uac", "domain", "netlogon", "key", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireStrongKey", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireStrongKey", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", "RequireStrongKey", 1)],
        },
    ];
}

// ── Merged from UserActivity.cs ──────────────────────────────────────────────────

internal static class UserActivity
{
    private const string ActivityPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    private const string TimelinePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    private const string Privacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy";

    private const string DiagTrackPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "activity-disable-publishing",
            Label = "Disable User Activity Publishing (No Timeline Sync to Cloud)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["activity", "timeline", "privacy", "cloud sync"],
            Description =
                "Prevents Windows from publishing user activity to Microsoft's cloud "
                + "Activity History service. EnableActivityFeed=0 stops Timeline data "
                + "from leaving the device.",
            ApplyOps = [RegOp.SetDword(ActivityPolicy, "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.SetDword(ActivityPolicy, "EnableActivityFeed", 1)],
            DetectOps = [RegOp.CheckDword(ActivityPolicy, "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-cloud-sync",
            Label = "Disable Activity History Cloud Sync Between Devices",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["activity", "cloud", "sync", "timeline", "privacy"],
            Description =
                "Stops Windows from uploading activity history to Microsoft's servers "
                + "for cross-device sync. AllowCrossDeviceClipboard=0 + "
                + "UploadUserActivities=0. Keep activity history local-only.",
            ApplyOps = [RegOp.SetDword(ActivityPolicy, "UploadUserActivities", 0)],
            RemoveOps = [RegOp.SetDword(ActivityPolicy, "UploadUserActivities", 1)],
            DetectOps = [RegOp.CheckDword(ActivityPolicy, "UploadUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-storage",
            Label = "Disable Local Activity History Storage",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["activity", "storage", "timeline", "local", "privacy"],
            Description =
                "Prevents Windows from storing user activity history on this device. "
                + "AllowStoringUserActivities=0. Completely disables Timeline and the "
                + "local activity database.",
            ApplyOps = [RegOp.SetDword(ActivityPolicy, "AllowStoringUserActivities", 0)],
            RemoveOps = [RegOp.SetDword(ActivityPolicy, "AllowStoringUserActivities", 1)],
            DetectOps = [RegOp.CheckDword(ActivityPolicy, "AllowStoringUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-cross-device-clipboard",
            Label = "Disable Cross-Device Clipboard Sync",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "clipboard", "cross-device", "privacy", "sync"],
            Description =
                "Disables the cross-device clipboard that syncs copied content through "
                + "Microsoft Account between Windows devices. Clipboard contents remain "
                + "local only. Enabled=0.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableClipboardHistory", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableClipboardHistory", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "activity-disable-recent-docs",
            Label = "Disable Recent Document Tracking in File Explorer",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "recent docs", "explorer", "privacy"],
            Description =
                "Stops File Explorer from tracking recently opened files and displaying "
                + "them in Quick Access. NoRecentDocsHistory=1. Keeps document access "
                + "history private and reduces Start/Quick Access clutter.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1),
            ],
        },
        new TweakDef
        {
            Id = "activity-disable-jump-lists",
            Label = "Disable Jump Lists in Taskbar and Start",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "jump lists", "taskbar", "privacy"],
            Description =
                "Prevents Windows from showing Jump Lists (recently used files and tasks) "
                + "when right-clicking taskbar icons or Start tiles. "
                + "NoStartMenuMFUprogramsList=1.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsMenu")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsMenu", 1)],
        },
        new TweakDef
        {
            Id = "activity-clear-recent-on-exit",
            Label = "Clear Recent Files History on Logoff",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "clear on exit", "recent files", "privacy"],
            Description =
                "Configures Windows to clear the list of recently used files and "
                + "programs from the Start menu every time the user logs off. "
                + "ClearRecentDocsOnExit=1.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1),
            ],
        },
        new TweakDef
        {
            Id = "activity-disable-cdp",
            Label = "Disable Connected Device Platform (CDP) — Timeline Backend",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["activity", "cdp", "connected devices", "timeline", "privacy"],
            Description =
                "Disables the Connected Device Platform (CDP) service back-end that "
                + "powers Windows Timeline and cross-device activity roaming. "
                + "EnableCdp=0. Complements EnableActivityFeed/AllowStoringUserActivities "
                + "for complete Timeline disablement.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
        },
    ];
}

// ── Merged from WindowsHello.cs ──────────────────────────────────────────────────

internal static class WindowsHello
{
    private const string BioPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics";
    private const string CredProvPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string PinCplx = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";
    private const string HelloPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
    private const string DynLock = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string CredProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI";
    private const string BioFace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics\FacialFeatures";
    private const string NgcPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "hello-disable-hello-for-work",
            Label = "Disable Windows Hello for Business Provisioning",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "biometrics", "security", "policy"],
            Description =
                "Prevents Windows Hello for Business from provisioning on this device "
                + "via Group Policy. Useful on personal machines enrolled in Intune by mistake.",
            ApplyOps = [RegOp.SetDword(HelloPol, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "Enabled")],
            DetectOps = [RegOp.CheckDword(HelloPol, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-biometric-enrollment",
            Label = "Disable Biometric Enrollment (GPO)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "biometrics", "fingerprint", "policy"],
            Description = "Blocks all biometric enrollment via GPO. Prevents Windows from " + "prompting to add a fingerprint or face after sign-in.",
            ApplyOps = [RegOp.SetDword(BioPol, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(BioPol, "Enabled")],
            DetectOps = [RegOp.CheckDword(BioPol, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-facial-recognition-enhanced",
            Label = "Disable Enhanced Anti-Spoofing for Facial Recognition",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "biometrics", "face recognition"],
            Description =
                "Disables the enhanced anti-spoofing check for facial recognition, "
                + "useful when using a basic webcam that doesn't support 3D scanning.",
            ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 0)],
            RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
            DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-dynamic-lock",
            Label = "Disable Dynamic Lock (Phone Proximity Auto-Lock)",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["windows hello", "lock screen", "bluetooth", "dynamic lock"],
            Description =
                "Disables Dynamic Lock, which automatically locks the PC when your "
                + "paired phone walks away. Stops false positives from brief Bluetooth drops.",
            ApplyOps = [RegOp.SetDword(DynLock, "EnableGoodbye", 0)],
            RemoveOps = [RegOp.DeleteValue(DynLock, "EnableGoodbye")],
            DetectOps = [RegOp.CheckDword(DynLock, "EnableGoodbye", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-pin-recovery",
            Label = "Disable PIN Recovery via Microsoft Account",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "pin", "privacy", "microsoft account"],
            Description =
                "Prevents Windows from uploading a PIN recovery key to your " + "Microsoft account, keeping the PIN credential fully local.",
            ApplyOps = [RegOp.SetDword(HelloPol, "EnablePinRecovery", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "EnablePinRecovery")],
            DetectOps = [RegOp.CheckDword(HelloPol, "EnablePinRecovery", 0)],
        },
        new TweakDef
        {
            Id = "hello-enforce-strong-pin-length",
            Label = "Enforce Minimum PIN Length (6 Digits)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "security", "hardening"],
            Description =
                "Sets the minimum PIN length to 6 digits via Group Policy, " + "improving resistance to brute-force attacks on the local login.",
            ApplyOps = [RegOp.SetDword(PinCplx, "MinimumPINLength", 6)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "MinimumPINLength")],
            DetectOps = [RegOp.CheckDword(PinCplx, "MinimumPINLength", 6)],
        },
        new TweakDef
        {
            Id = "hello-enable-pin-expiry",
            Label = "Enable PIN Expiry (90-Day Rotation)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "security", "policy"],
            Description = "Requires the PIN to be changed every 90 days, matching common " + "enterprise password rotation policies.",
            ApplyOps = [RegOp.SetDword(PinCplx, "Expiration", 90)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "Expiration")],
            DetectOps = [RegOp.CheckDword(PinCplx, "Expiration", 90)],
        },
        new TweakDef
        {
            Id = "hello-disable-companion-device-unlock",
            Label = "Disable Companion Device Unlock (Phone as Key)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "companion device", "bluetooth", "security"],
            Description =
                "Disables the Companion Device Framework used by Phone Link and "
                + "other apps to unlock Windows with a paired device, reducing the "
                + "Bluetooth attack surface.",
            ApplyOps = [RegOp.SetDword(CredProvPol, "AllowDomainPINLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(CredProvPol, "AllowDomainPINLogon")],
            DetectOps = [RegOp.CheckDword(CredProvPol, "AllowDomainPINLogon", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-picture-password",
            Label = "Disable Picture Password Logon",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "login", "security", "picture password"],
            Description =
                "Blocks picture-password (gesture on photo) from being set as a "
                + "sign-in option. Picture passwords are weaker than PINs due to pattern guessing.",
            ApplyOps = [RegOp.SetDword(CredProvPol, "BlockDomainPicturePassword", 1)],
            RemoveOps = [RegOp.DeleteValue(CredProvPol, "BlockDomainPicturePassword")],
            DetectOps = [RegOp.CheckDword(CredProvPol, "BlockDomainPicturePassword", 1)],
        },
        new TweakDef
        {
            Id = "hello-disable-pin-on-device-provisioning",
            Label = "Disable Forced PIN Creation During Setup",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "pin", "setup", "oobe"],
            Description =
                "Prevents Windows from prompting to create a Windows Hello PIN " + "during the Out-of-Box Experience. Allows password-only accounts.",
            ApplyOps = [RegOp.SetDword(HelloPol, "DisablePostLogonProvisioning", 1)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "DisablePostLogonProvisioning")],
            DetectOps = [RegOp.CheckDword(HelloPol, "DisablePostLogonProvisioning", 1)],
        },
        new TweakDef
        {
            Id = "hello-enforce-anti-spoofing",
            Label = "Enforce Enhanced Anti-Spoofing for Facial Recognition",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "face recognition", "biometric", "anti-spoofing", "security"],
            Description =
                "Requires Windows Hello facial recognition to use enhanced liveness "
                + "detection, blocking login attempts using photos or masks. "
                + "Only compatible cameras (IR depth-sensing) with certified drivers can satisfy this policy.",
            ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 1)],
            RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
            DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 1)],
        },
        new TweakDef
        {
            Id = "hello-set-min-pin-length-8",
            Label = "Set Minimum Windows Hello PIN Length to 8 Digits",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "complexity", "policy", "security"],
            Description =
                "Raises the minimum Windows Hello for Business PIN length to 8 characters "
                + "via Group Policy, replacing the insecure 4-digit default. "
                + "Applies to domain-joined and Entra ID-joined devices.",
            ApplyOps = [RegOp.SetDword(PinCplx, "MinimumPINLength", 8)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "MinimumPINLength")],
            DetectOps = [RegOp.CheckDword(PinCplx, "MinimumPINLength", 8)],
        },
        new TweakDef
        {
            Id = "hello-set-max-pin-length-20",
            Label = "Set Maximum Windows Hello PIN Length to 20 Digits",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "complexity", "policy", "length"],
            Description =
                "Caps the maximum Windows Hello PIN length at 20 characters. "
                + "Prevents trivially long PINs that bypass complexity checking "
                + "while still allowing strong passphrases.",
            ApplyOps = [RegOp.SetDword(PinCplx, "MaximumPINLength", 20)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "MaximumPINLength")],
            DetectOps = [RegOp.CheckDword(PinCplx, "MaximumPINLength", 20)],
        },
        new TweakDef
        {
            Id = "hello-require-digits-in-pin",
            Label = "Require Digits in Windows Hello PIN",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "complexity", "digits", "policy"],
            Description =
                "Enforces that at least one digit (0–9) must appear in the "
                + "Windows Hello PIN, preventing all-alpha PINs on devices that have "
                + "expanded the PIN character set.",
            ApplyOps = [RegOp.SetDword(PinCplx, "Digits", 1)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "Digits")],
            DetectOps = [RegOp.CheckDword(PinCplx, "Digits", 1)],
        },
        new TweakDef
        {
            Id = "hello-require-uppercase-in-pin",
            Label = "Require Uppercase Letters in Windows Hello PIN",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "complexity", "uppercase", "policy"],
            Description =
                "Requires at least one uppercase letter in the Windows Hello PIN. "
                + "Effective only when the device policy allows alphanumeric PINs "
                + "(MinimumPINLength ≥ 4 with letters permitted).",
            ApplyOps = [RegOp.SetDword(PinCplx, "UppercaseLetters", 1)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "UppercaseLetters")],
            DetectOps = [RegOp.CheckDword(PinCplx, "UppercaseLetters", 1)],
        },
        new TweakDef
        {
            Id = "hello-require-special-chars-in-pin",
            Label = "Require Special Characters in Windows Hello PIN",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "complexity", "special characters", "policy"],
            Description =
                "Requires the PIN to include at least one special character. "
                + "Combined with digit and uppercase requirements this enforces "
                + "a password-grade PIN on supported Win11 policies.",
            ApplyOps = [RegOp.SetDword(PinCplx, "SpecialCharacters", 1)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "SpecialCharacters")],
            DetectOps = [RegOp.CheckDword(PinCplx, "SpecialCharacters", 1)],
        },
        new TweakDef
        {
            Id = "hello-disable-cloud-trust-kerberos",
            Label = "Disable Cloud Kerberos Trust for Hybrid Hello",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "kerberos", "cloud trust", "azure ad", "hybrid"],
            Description =
                "Disables the Cloud Trust authentication model for Windows Hello "
                + "for Business on Entra ID hybrid-joined devices. Forces the "
                + "traditional on-premises PKI trust path instead of the cloud "
                + "Kerberos ticket flow.",
            ApplyOps = [RegOp.SetDword(HelloPol, "UseCloudTrustForOnPremAuth", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "UseCloudTrustForOnPremAuth")],
            DetectOps = [RegOp.CheckDword(HelloPol, "UseCloudTrustForOnPremAuth", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-phone-sign-in",
            Label = "Disable Phone (Companion Device) Sign-In for Hello",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "phone", "companion device", "unlock", "security"],
            Description =
                "Blocks Windows Hello companion device (phone/wearable) unlock "
                + "via the Microsoft Account + Bluetooth proximity mechanism. "
                + "Enforces that each sign-in requires on-device biometric or PIN.",
            ApplyOps = [RegOp.SetDword(HelloPol, "AllowPhoneLinkDevice", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "AllowPhoneLinkDevice")],
            DetectOps = [RegOp.CheckDword(HelloPol, "AllowPhoneLinkDevice", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-web-sign-in",
            Label = "Disable Web Sign-In for Windows (Hello MSA Flow)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "web sign-in", "msa", "fido2", "account"],
            Description =
                "Disables the Web Sign-In credential provider which shows an "
                + "embedded browser for FIDO2/MSA login. Prevents web-based "
                + "phishing flows from appearing on the lock screen.",
            ApplyOps = [RegOp.SetDword(CredProvPol, "EnableWebSignIn", 0)],
            RemoveOps = [RegOp.DeleteValue(CredProvPol, "EnableWebSignIn")],
            DetectOps = [RegOp.CheckDword(CredProvPol, "EnableWebSignIn", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-biometric-domain-users",
            Label = "Disable Biometrics for Domain Users",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "biometric", "domain", "policy", "security"],
            Description =
                "Prevents domain-joined users from enrolling in or using Windows "
                + "Hello biometrics (fingerprint, iris, face) for domain account "
                + "authentication. Enforces PIN or smart-card only for domain logins.",
            ApplyOps = [RegOp.SetDword(BioPol, "DomainAccountsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(BioPol, "DomainAccountsEnabled")],
            DetectOps = [RegOp.CheckDword(BioPol, "DomainAccountsEnabled", 0)],
        },
    ];
}

// === Merged from: LockScreen.cs ===


internal static class LockScreen
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lock-disable-lock-screen",
            Label = "Disable Lock Screen Entirely",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Completely disables the lock screen, going straight to the password/PIN prompt. Default: enabled. Recommended: disabled (home PCs).",
            Tags = ["lockscreen", "disable", "bypass", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-login-blur",
            Label = "Disable Login Background Blur",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the acrylic (frosted glass) blur effect on the sign-in screen background. Shows the full wallpaper. Default: blurred. Recommended: disabled.",
            Tags = ["lockscreen", "login", "blur", "acrylic", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableAcrylicBackgroundOnLogon", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-first-login-animation",
            Label = "Disable First Sign-In Animation",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the 'Hi / We're getting things ready' animation on first login. Speeds up new profile setup. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["lockscreen", "animation", "login", "first-run"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFirstLogonAnimation", 0)],
        },

        new TweakDef
        {
            Id = "lock-disable-lock-screen-ads",
            Label = "Disable Lock Screen Ads/Tips",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables rotating lock screen tips and advertising overlays. Default: Enabled. Recommended: Disabled.",
            Tags = ["lockscreen", "ads", "tips", "spotlight"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
        },

        new TweakDef
        {
            Id = "lock-disable-network-ui",
            Label = "Disable Network UI on Lock Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the network selection UI on the lock screen. Prevents users from connecting to networks before sign-in. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["lockscreen", "network", "selection", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
        },

        new TweakDef
        {
            Id = "lock-disable-password-reveal",
            Label = "Disable Password Reveal Button",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Hides the password reveal (eye) button from credential input fields on the login screen and UAC prompts. Reduces shoulder-surfing risk. Default: shown. Recommended: hidden for shared/kiosk machines.",
            Tags = ["lockscreen", "password", "reveal", "security", "credential"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI", "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "lock-hide-sleep-button",
            Label = "Hide Sleep Button from Lock Screen Power Menu",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the Sleep option from the power flyout on the lock screen and Start menu. Prevents accidental sleep on always-on machines. Default: shown. Recommended: hidden for servers/kiosks.",
            Tags = ["lockscreen", "sleep", "power", "flyout", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowSleepOption", 0),
            ],
        },
        new TweakDef
        {
            Id = "lock-hide-hibernate-button",
            Label = "Hide Hibernate Button from Lock Screen Power Menu",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the Hibernate option from the power flyout on the lock screen and Start menu. Prevents accidental hibernation on desktop machines. Default: shown (if hibernate enabled). Recommended: hidden for desktops.",
            Tags = ["lockscreen", "hibernate", "power", "flyout", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowHibernateOption", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings", "ShowHibernateOption"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FlyoutMenuSettings",
                    "ShowHibernateOption",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "lock-set-lockout-threshold-5",
            Label = "Set Account Lockout Threshold to 5 Attempts",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Locks the account after 5 failed login attempts via registry. Helps mitigate brute-force attacks. Default: 0 (disabled).",
            Tags = ["lockscreen", "lockout", "security", "brute-force"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts", 5),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "MaxDevicePasswordFailedAttempts"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    "MaxDevicePasswordFailedAttempts",
                    5
                ),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-shutdown-button",
            Label = "Hide Shutdown Button on Login Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the shutdown button from the login screen. Prevents unauthorized shutdowns. Default: shown.",
            Tags = ["lockscreen", "shutdown", "login", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ShutdownWithoutLogon", 0),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-camera-on-lockscreen",
            Label = "Disable Camera on Lock Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents access to the camera from the lock screen. Enhances physical security. Default: accessible.",
            Tags = ["lock-screen", "camera", "disable", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the slideshow feature on the lock screen. Reduces memory and GPU usage. Default: user-configurable.",
            Tags = ["lock-screen", "slideshow", "disable", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "lock-auto-lock-10min",
            Label = "Auto-Lock After 10 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the screen saver timeout to 10 minutes with automatic lock. Enhances physical security. Default: no timeout.",
            Tags = ["lock-screen", "auto-lock", "timeout", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs", 600),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs", 600),
            ],
        },
        new TweakDef
        {
            Id = "lock-auto-lock-5min",
            Label = "Auto-Lock After 5 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the inactivity timeout to 5 minutes with automatic lock. Stricter security policy. Default: no timeout.",
            Tags = ["lock-screen", "auto-lock", "timeout", "strict"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs", 300),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "InactivityTimeoutSecs", 300),
            ],
        },
        new TweakDef
        {
            Id = "lock-auto-restart-signon",
            Label = "Disable Auto-Restart Sign-On (ARSO)",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Automatic Restart Sign-On. Prevents Windows from automatically signing in after updates. Default: enabled.",
            Tags = ["lock-screen", "arso", "restart", "sign-on"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1),
            ],
        },
        new TweakDef
        {
            Id = "lock-clear-legal-notice",
            Label = "Clear Legal Notice at Login",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Clears any legal notice caption and text displayed before login. Removes the mandatory 'OK' click before sign-in. Default: none.",
            Tags = ["lock-screen", "legal-notice", "login", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticecaption", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticetext", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticecaption"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticetext"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "legalnoticecaption", ""),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-ads",
            Label = "Disable Lock Screen Advertisements",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables fun facts, tips, and tricks (ads) on the lock screen. Prevents Microsoft from showing promotional content. Default: enabled.",
            Tags = ["lock-screen", "ads", "tips", "spotlight"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338387Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338387Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-camera",
            Label = "Disable Lock Screen Camera",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the camera shortcut on the lock screen. Prevents photo access without unlocking. Default: enabled.",
            Tags = ["lock-screen", "camera", "privacy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Fast User Switching. Only one user at a time; other users must log off first. Saves memory and resources. Default: enabled.",
            Tags = ["lock-screen", "fast-user-switching", "security", "resources"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1),
            ],
        },
        new TweakDef
        {
            Id = "lock-disable-sign-in-animation",
            Label = "Disable First Sign-In Animation",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Getting things ready' first sign-in animation. Speeds up new user profile creation. Default: enabled.",
            Tags = ["lock-screen", "animation", "first-login", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
        },
        new TweakDef
        {
            Id = "lock-hide-network-icon",
            Label = "Hide Network Icon on Lock Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hides the network icon from the Windows lock screen. Prevents users from changing Wi-Fi or seeing network status before login. Default: visible.",
            Tags = ["lock-screen", "network", "icon", "hide"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DontDisplayNetworkSelectionUI", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-lockscreen-app-notif",
            Label = "Disable App Notifications on Lock Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLockScreenAppNotifications=1 in the System policy. Prevents any app from showing toast notification content on the lock screen, reducing information leakage.",
            Tags = ["lock-screen", "notifications", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications", 1)],
        },
        new TweakDef
        {
            Id = "lock-block-picture-password",
            Label = "Block Picture Password for Domain Users",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BlockDomainPicturePassword=1 in System policies. Prevents domain-joined users from using picture gestures as a Windows logon method, ensuring credential-based authentication.",
            Tags = ["lock-screen", "password", "domain", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "BlockDomainPicturePassword", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "BlockDomainPicturePassword"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "BlockDomainPicturePassword", 1),
            ],
        },
        new TweakDef
        {
            Id = "lock-hide-locked-user-display",
            Label = "Do Not Display Locked User Identity",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontDisplayLockedUserId=3 in System policies. Shows a generic icon instead of the user's name and picture on the lock screen when the session is locked.",
            Tags = ["lock-screen", "user", "identity", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLockedUserId", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLockedUserId"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayLockedUserId", 3),
            ],
        },
        new TweakDef
        {
            Id = "lock-force-unlock-reauth",
            Label = "Require Re-Authentication When Unlocking Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceUnlockLogon=1 in Winlogon. Forces the machine to always require credentials (not just a cached unlock token) when returning from the lock screen.",
            Tags = ["lock-screen", "unlock", "authentication", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ForceUnlockLogon", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ForceUnlockLogon", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ForceUnlockLogon", 1)],
        },
        new TweakDef
        {
            Id = "lock-disable-spotlight-rotation",
            Label = "Disable Windows Spotlight Rotating Lock Screen Images",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RotatingLockScreenEnabled=0 in the ContentDeliveryManager key. Stops the lock screen background from cycling through Microsoft Spotlight images downloaded from the internet.",
            Tags = ["lock-screen", "spotlight", "background", "images"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "RotatingLockScreenEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "lock-set-blank-screensaver",
            Label = "Set Screen Saver to Blank Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCRNSAVE.EXE to scrnsave.scr (blank screen) in Desktop. Uses the built-in blank screen saver that simply turns the display black, avoiding GPU usage from animated screen savers.",
            Tags = ["lock-screen", "screensaver", "blank", "power"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"%SystemRoot%\system32\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"%SystemRoot%\system32\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "lock-block-user-info-at-signin",
            Label = "Block Users from Showing Personal Info on Sign-In Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BlockUserFromShowingAccountDetailsOnSignin=1 in System policy. Prevents users from choosing to display their email address, display name, or account picture on the Windows sign-in screen.",
            Tags = ["lock-screen", "sign-in", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockUserFromShowingAccountDetailsOnSignin", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockUserFromShowingAccountDetailsOnSignin"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockUserFromShowingAccountDetailsOnSignin", 1),
            ],
        },
        new TweakDef
        {
            Id = "lock-suppress-user-name-display",
            Label = "Do Not Display Username on Sign-In Screen",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontDisplayUserName=1 in System policies. Hides the user's display name (but not the username entry field) on the Windows sign-in and lock screen tiles.",
            Tags = ["lock-screen", "user", "name", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayUserName", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayUserName")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DontDisplayUserName", 1)],
        },
        new TweakDef
        {
            Id = "lock-set-logon-async-scripts",
            Label = "Run Logon Scripts Asynchronously",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RunLogonScriptSync=0 in Winlogon. Allows the Windows shell to load before logon scripts finish executing. Significantly speeds up the time from password entry to a usable desktop.",
            Tags = ["lock-screen", "logon", "scripts", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "RunLogonScriptSync", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "RunLogonScriptSync", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "RunLogonScriptSync", 0)],
        },
        new TweakDef
        {
            Id = "lock-disable-spotlight-lock-policy",
            Label = "Disable Windows Spotlight on Lock Screen (Policy)",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightOnLockScreen=1 in the CloudContent user policy key. Prevents the lock screen from fetching and displaying MSN Spotlight background images and facts.",
            Tags = ["lock-screen", "spotlight", "cloud-content", "policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
        },
    ];
}

// ── Merged from Screensaver.cs ──────────────────────────────────────────────────

internal static class Screensaver
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ss-disable-screensaver",
            Label = "Disable Screensaver",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable the screensaver. Default: enabled. Recommended: keep enabled with password.",
            Tags = ["screensaver", "disable", "screen"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-timeout-5m",
            Label = "Screensaver Timeout: 5 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Set screensaver to activate after 5 minutes. Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "5min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
        },
        new TweakDef
        {
            Id = "ss-timeout-10m",
            Label = "Screensaver Timeout: 10 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Set screensaver to activate after 10 minutes. Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "10min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
        },
        new TweakDef
        {
            Id = "ss-blank-screensaver",
            Label = "Set Blank (Black) Screensaver",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Set screensaver to plain black screen. Default: none. Recommended: blank for OLED.",
            Tags = ["screensaver", "blank", "black", "oled"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", "C:\\Windows\\System32\\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", "C:\\Windows\\System32\\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "ss-disable-lock-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disable slideshow on the lock screen. Default: enabled.",
            Tags = ["lock", "slideshow", "screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },

        new TweakDef
        {
            Id = "ss-scr-timeout-10min",
            Label = "Set Screensaver Timeout to 10 Minutes (Policy)",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets screensaver timeout to 10 minutes via machine policy. Enforced across all users. Default: varies. Recommended: 600 seconds.",
            Tags = ["screensaver", "timeout", "10min", "policy"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Control Panel\Desktop",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveTimeOut", "600"),
            ],
        },
        new TweakDef
        {
            Id = "ss-scr-disable-screensaver",
            Label = "Disable Screensaver Completely (Policy)",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the screensaver completely via machine policy. Prevents screensaver from activating on any user. Default: Enabled. Recommended: Disabled only for kiosks.",
            Tags = ["screensaver", "disable", "policy"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Control Panel\Desktop",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-set-screensaver-timeout-300",
            Label = "Set Screensaver Timeout to 5 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets screensaver activation timeout to 300 seconds (5 minutes). Default: 900 (15 min).",
            Tags = ["screensaver", "timeout", "lock", "idle"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
        },
        new TweakDef
        {
            Id = "ss-require-password-on-resume",
            Label = "Require Password on Screensaver Resume",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Requires password when resuming from screensaver. Critical for security. Default: varies.",
            Tags = ["screensaver", "password", "lock", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
        },
        new TweakDef
        {
            Id = "ss-set-screensaver-blank",
            Label = "Set Screensaver to Blank (Most Efficient)",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the screensaver to the blank (black screen) option. Lowest power usage. Default: none.",
            Tags = ["screensaver", "blank", "power", "efficiency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "ss-disable-screen-saver-policy",
            Label = "Disable Screen Saver via Group Policy",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the screen saver using Group Policy. Overrides user settings. Default: not configured.",
            Tags = ["screensaver", "policy", "gpo", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-lock-screen-timeout-60",
            Label = "Set Lock Screen Timeout to 60 Seconds",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the console lock display-off timeout to 60 seconds. Screen turns off faster on lock screen. Default: 60 (Windows default, but often changed by OEMs).",
            Tags = ["screensaver", "lock-screen", "timeout", "display"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
                    "Attributes"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\8EC4B3A5-6868-48c2-BE75-4F3044BE88A7",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "ss-disable-user-policy",
            Label = "Disable Screen Saver (User)",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the screen saver for the current user. Screen will stay on until manually locked or display timeout triggers. Default: enabled.",
            Tags = ["screensaver", "disable", "user", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "ss-force-policy",
            Label = "Force Screen Saver via Policy",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces screen saver activation via Group Policy. Overrides user preferences. Default: not enforced.",
            Tags = ["screensaver", "force", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "1")],
        },
        new TweakDef
        {
            Id = "ss-prevent-screensaver-change",
            Label = "Prevent Screen Saver Changes",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from changing the screen saver settings. Locks the current screen saver configuration. Default: allowed.",
            Tags = ["screensaver", "prevent", "change", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispScrSavPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispScrSavPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispScrSavPage", 1)],
        },
        new TweakDef
        {
            Id = "ss-require-password",
            Label = "Require Password on Screen Saver Resume",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Requires a password to unlock after screen saver activates. Enhances physical security. Default: not required.",
            Tags = ["screensaver", "password", "resume", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
        },
        new TweakDef
        {
            Id = "ss-scr-password-on-resume",
            Label = "Enforce Password on Resume (Policy)",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces password requirement on screen saver resume via Group Policy. Machine-wide enforcement. Default: not enforced.",
            Tags = ["screensaver", "password", "resume", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure", "1"),
            ],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-15min",
            Label = "Set Screen Saver Timeout to 15 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the screen saver activation timeout to 15 minutes (900 seconds). Balances security with usability. Default: 10 minutes.",
            Tags = ["screensaver", "timeout", "15min", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "900")],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-30min",
            Label = "Set Screen Saver Timeout to 30 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the screen saver activation timeout to 30 minutes (1800 seconds). Longer timeout for active use. Default: 10 minutes.",
            Tags = ["screensaver", "timeout", "30min", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "1800")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "1800")],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-5min",
            Label = "Set Screensaver Timeout to 5 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            Description = "Sets the screensaver activation timeout to 5 minutes (300 seconds). Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "lock", "5-min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "300")],
        },
        new TweakDef
        {
            Id = "ss-set-timeout-10min",
            Label = "Set Screensaver Timeout to 10 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            Description = "Sets the screensaver activation timeout to 10 minutes (600 seconds). Default: 15 minutes.",
            Tags = ["screensaver", "timeout", "lock", "10-min"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveTimeOut", "600")],
        },
        new TweakDef
        {
            Id = "ss-enable-password-on-resume",
            Label = "Require Password on Resume",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Requires password after screensaver deactivation. Security best practice. Default: disabled.",
            Tags = ["screensaver", "password", "lock", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaverIsSecure", "1")],
        },
        new TweakDef
        {
            Id = "ss-force-blank-screensaver",
            Label = "Set Blank Screen Screensaver",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            Description = "Sets the screensaver to a blank (black) screen. Lowest resource usage. Default: none.",
            Tags = ["screensaver", "blank", "black", "power"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SCRNSAVE.EXE", @"C:\Windows\System32\scrnsave.scr")],
        },
        new TweakDef
        {
            Id = "ss-disable-lock-screen-camera",
            Label = "Disable Lock Screen Camera",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            Description = "Disables the camera shortcut on the lock screen. Default: enabled.",
            Tags = ["lock", "camera", "privacy", "lock-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "ss-disable-lock-screen-slideshow",
            Label = "Disable Lock Screen Slideshow",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            Description = "Disables the Windows Spotlight/slideshow on the lock screen. Default: enabled.",
            Tags = ["lock", "slideshow", "spotlight", "lock-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "ss-set-monitor-timeout-10min",
            Label = "Set Monitor Power Off to 10 Minutes",
            Category = "Lock Screen & Login",
            NeedsAdmin = false,
            Description = "Sets the monitor to turn off after 10 minutes of inactivity. Saves energy. Default: 15 minutes.",
            Tags = ["monitor", "power", "timeout", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\PowerCfg\PowerPolicies\0"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff", "600")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaveTimeOut", "MonitorPowerOff", "600")],
        },
        new TweakDef
        {
            Id = "ss-enable-lock-workstation",
            Label = "Enable Lock Workstation (Ctrl+Alt+Del)",
            Category = "Lock Screen & Login",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures the Lock Workstation option is available on Ctrl+Alt+Del. Default: enabled.",
            Tags = ["lock", "workstation", "security", "ctrl-alt-del"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation", 0),
            ],
        },
    ];
}

// ── merged from Accessibility.cs ────────────────────────────────────────
internal static class Accessibility
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "acc-disable-accessibility-shortcuts",
            Label = "Disable Sticky/Toggle/Filter Keys",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the Sticky Keys, Toggle Keys, and Filter Keys popups triggered by repeated key presses.",
            Tags = ["accessibility", "keyboard", "gaming"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys",
                @"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys",
                @"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "58"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "122"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "126"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
        },



        new TweakDef
        {
            Id = "acc-wide-scrollbar",
            Label = "Increase Scroll Bar Width",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases scroll bar width from default (17px) to 25px for easier targeting with mouse or touch.",
            Tags = ["accessibility", "ui", "scrollbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-400"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-400"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-255"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-255"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-400")],
        },
        new TweakDef
        {
            Id = "acc-disable-narrator",
            Label = "Disable Narrator Auto-Start",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Narrator from starting at login or via Win+Enter.",
            Tags = ["accessibility", "narrator", "screen-reader"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-high-contrast-mode",
            Label = "Enable High Contrast Mode",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables high contrast mode for improved visual clarity.",
            Tags = ["accessibility", "contrast", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "127")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "127")],
        },
        new TweakDef
        {
            Id = "acc-disable-magnifier-hotkey",
            Label = "Disable Magnifier Win+Plus Hotkey",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Win+Plus hotkey that launches Magnifier and prevents it from running.",
            Tags = ["accessibility", "magnifier", "hotkey"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "RunningState", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableMagnifier", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableMagnifier"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "RunningState", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "RunningState", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-osk-auto-launch",
            Label = "Disable On-Screen Keyboard Auto-Launch",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the On-Screen Keyboard from automatically launching at startup or when entering tablet mode.",
            Tags = ["accessibility", "keyboard", "osk", "tablet"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Osk",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Osk", "ShowStartupPanel", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode", "OpenStandby", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Osk", "ShowStartupPanel", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode", "OpenStandby", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Osk", "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-underline-shortcuts",
            Label = "Disable Menu Access Key Underlines",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the underline indicators on menu access keys (keyboard shortcuts) for a cleaner UI.",
            Tags = ["accessibility", "keyboard", "menu", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-sound-sentry",
            Label = "Disable Visual Sound Alerts",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables SoundSentry visual alerts that flash the screen or window when a system sound plays.",
            Tags = ["accessibility", "sound", "visual-alert"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "Flags", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-access-disable-narrator-autostart",
            Label = "Disable Narrator Autostart",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Narrator from launching with Win+Enter shortcut. Default: Enabled. Recommended: Disabled if not needed.",
            Tags = ["accessibility", "narrator", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-filter-keys",
            Label = "Disable Filter Keys Shortcut",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Filter Keys shortcut, preventing accidental activation that can interfere with typing and gaming.",
            Tags = ["accessibility", "filter-keys", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-toggle-keys",
            Label = "Disable Toggle Keys Shortcut",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Toggle Keys shortcut that plays a tone when Caps Lock, Num Lock, or Scroll Lock is pressed.",
            Tags = ["accessibility", "toggle-keys", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-increase-hover-time",
            Label = "Increase Tooltip Hover Time",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the mouse hover delay before tooltips appear from 400ms to 1000ms. Reduces accidental tooltip popups. Default: 400ms. Recommended: 1000ms for accessibility.",
            Tags = ["accessibility", "mouse", "hover", "tooltip", "delay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MouseHoverTime", "1000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MouseHoverTime", "1000")],
        },
        new TweakDef
        {
            Id = "acc-increase-focus-border",
            Label = "Increase Focus Border Width",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the focus rectangle border from 1px to 3px wide. Makes keyboard-focused controls easier to identify visually. Default: 1px. Recommended: 3px for low vision.",
            Tags = ["accessibility", "focus", "border", "keyboard", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderWidth", 3),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderHeight", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderWidth", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderHeight", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderWidth", 3)],
        },
        new TweakDef
        {
            Id = "acc-disable-narrator-auto-start",
            Label = "Disable Narrator Auto-Start",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Narrator from starting automatically at sign-in. Default: not auto-started. Useful if accidentally enabled.",
            Tags = ["accessibility", "narrator", "auto-start", "screen-reader"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam", "RunNarratorOnLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam", "RunNarratorOnLogon")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam", "RunNarratorOnLogon", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-filter-keys-shortcut",
            Label = "Disable Filter Keys Shortcut",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the keyboard shortcut (hold right Shift 8 sec) that activates Filter Keys. Prevents accidental activation during gaming. Default: enabled.",
            Tags = ["accessibility", "filter-keys", "shortcut", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "122")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "122")],
        },
        new TweakDef
        {
            Id = "acc-enable-underline-access-keys",
            Label = "Always Underline Access Keys",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows underlined keyboard shortcuts in menus and dialogs at all times, not just when Alt is pressed. Default: hidden until Alt. Recommended: visible.",
            Tags = ["accessibility", "access-keys", "underline", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "1")],
        },
        new TweakDef
        {
            Id = "acc-disable-toggle-keys-sound",
            Label = "Disable Toggle Keys Sound",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the audible tone when pressing Caps Lock, Num Lock, or Scroll Lock. Default: enabled. Recommended: disabled.",
            Tags = ["accessibility", "toggle-keys", "sound", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "58")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "58")],
        },
        new TweakDef
        {
            Id = "acc-disable-magnifier-follows-keyboard",
            Label = "Disable Magnifier Follows Keyboard",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Stops the magnifier view from following the keyboard cursor. Useful for users who prefer manual control. Default: follows keyboard.",
            Tags = ["accessibility", "magnifier", "keyboard", "follow"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "FollowCaret", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
        },
        new TweakDef
        {
            Id = "acc-access-disable-magnifier",
            Label = "Disable Magnifier Auto-Start",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents the Magnifier from auto-starting with Windows. Disables the magnifier startup accessibility feature. Default: depends on accessibility settings.",
            Tags = ["accessibility", "magnifier", "autostart", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "IsAutoStartEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "IsAutoStartEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "IsAutoStartEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-mouse-keys",
            Label = "Disable Mouse Keys",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Mouse Keys accessibility feature that lets you control the cursor with the numeric keypad. Default: user setting.",
            Tags = ["accessibility", "mouse-keys", "numpad", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys", "Flags", "62")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys", "Flags", "0")],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "acc-disable-sticky-keys",
            Label = "Disable Sticky Keys",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Sticky Keys which allows modifier keys to remain active after being released. Eliminates the common 5× Shift annoyance during gaming.",
            Tags = ["accessibility", "sticky-keys", "gaming", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-bounce-keys",
            Label = "Disable Bounce Keys",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Bounce Keys which ignores repeated keystrokes within a short time. Removes unwanted key filtering for fast typists.",
            Tags = ["accessibility", "bounce-keys", "keyboard", "filter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "BounceTime", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "BounceTime", "100")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "BounceTime", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-ease-of-access-hotkey",
            Label = "Disable Ease of Access Center Hotkey",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Win+U hotkey that opens the Ease of Access Center. Frees the shortcut for other uses and prevents accidental opening.",
            Tags = ["accessibility", "hotkey", "ease-of-access", "keyboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispEasyAccessCPL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispEasyAccessCPL")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispEasyAccessCPL", 1)],
        },
        new TweakDef
        {
            Id = "acc-disable-auto-correct",
            Label = "Disable Auto-Correct for Touch Keyboard",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic word correction in the Windows touch keyboard. Prevents unwanted text changes during touch input.",
            Tags = ["accessibility", "auto-correct", "touch-keyboard", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableAutocorrection", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableAutocorrection")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableAutocorrection", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-high-contrast-hotkey",
            Label = "Disable High Contrast Hotkey",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Left Alt+Left Shift+Print Screen hotkey for toggling high contrast mode. Prevents accidental theme changes.",
            Tags = ["accessibility", "high-contrast", "hotkey", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "0")],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "acc-disable-narrator-key-echo",
            Label = "Disable Narrator Key Echo",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Narrator from reading aloud each key you press while typing.",
            Tags = ["accessibility", "narrator", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "StringKeyEcho", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "StringKeyEcho")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "StringKeyEcho", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-magnifier-caret-follow",
            Label = "Disable Magnifier Caret Following",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Magnifier lens from following the text cursor as you type.",
            Tags = ["accessibility", "magnifier"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "FollowCaret")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-mouse-trails",
            Label = "Disable Mouse Pointer Trails",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes mouse cursor trail effect that can be distracting or cause visual issues.",
            Tags = ["accessibility", "mouse", "cursor"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "-1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "-1")],
        },
        new TweakDef
        {
            Id = "acc-disable-pointer-shadow",
            Label = "Disable Mouse Pointer Shadow",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the drop shadow effect rendered under the mouse pointer.",
            Tags = ["accessibility", "mouse", "cursor", "visuals"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-pointer-precision",
            Label = "Disable Enhanced Pointer Precision",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off Enhanced Pointer Precision (mouse acceleration). Recommended for gaming and precise work.",
            Tags = ["accessibility", "mouse", "gaming", "precision"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold1", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold2", "0"),
            ],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-color-filters",
            Label = "Disable Colour Filters",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off colour filter overlays (grayscale, red-green, blue-yellow) used for colour-blind support.",
            Tags = ["accessibility", "colour-filters", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-color-filter-hotkey",
            Label = "Disable Colour Filter Hotkey",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows+Ctrl+C hotkey that toggles colour filters on/off.",
            Tags = ["accessibility", "colour-filters", "hotkey", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "FilterType", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "HotkeyRegistered", 0),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "HotkeyRegistered")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "HotkeyRegistered", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-audio-description",
            Label = "Disable Audio Descriptions",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off automatic audio descriptions for videos in supported apps.",
            Tags = ["accessibility", "audio", "media"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DuckAudio", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DuckAudio")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DuckAudio", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-visual-notifications",
            Label = "Disable Visual Sound Notifications",
            Category = "User Account",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the flash visual alerts that substitute for sound cues (SoundSentry).",
            Tags = ["accessibility", "sound", "visuals", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "WindowsEffect", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "WindowsEffect")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "WindowsEffect", "0")],
        },
    ];
}
