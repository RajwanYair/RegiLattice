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
