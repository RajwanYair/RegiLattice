namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from PackageManagement.cs ──
internal static class PackageManagement
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pkg-disable-suggested-apps",
            Label = "Disable Suggested App Installations",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing suggested apps. Default: Enabled. Recommended: Disabled.",
            Tags = ["packages", "suggested", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-disable-winget-auto-update",
            Label = "Disable WinGet Auto-Update",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic WinGet package manager self-updates via policy. Default: Enabled. Recommended: Disabled for controlled environments.",
            Tags = ["packages", "winget", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "pkg-choco-proxy",
            Label = "Set Chocolatey System Proxy",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Configures Chocolatey to use the system proxy for package downloads. Useful in corporate environments behind a proxy. Default: Direct. Recommended: Enabled behind proxy.",
            Tags = ["packages", "chocolatey", "proxy", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy", 1)],
        },
        new TweakDef
        {
            Id = "pkg-source-validation",
            Label = "Enable Package Source Validation",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents WinGet from overriding package hash validation. Ensures integrity checks are enforced for all package sources. Default: Override allowed. Recommended: Disabled (validation enforced).",
            Tags = ["packages", "winget", "hash", "validation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-ms-store",
            Label = "Disable Microsoft Store via Policy",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables access to the Microsoft Store via Group Policy. Default: enabled.",
            Tags = ["package", "msstore", "disable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
        },
        new TweakDef
        {
            Id = "pkg-enable-developer-sideload",
            Label = "Enable Developer Mode Sideloading",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables developer mode to allow sideloading of apps without the Store. Default: disabled.",
            Tags = ["package", "developer", "sideload", "appx"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowDevelopmentWithoutDevLicense", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowDevelopmentWithoutDevLicense")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowDevelopmentWithoutDevLicense", 1)],
        },
        new TweakDef
        {
            Id = "pkg-disable-appinstaller-protocol",
            Label = "Disable ms-appinstaller Protocol",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the ms-appinstaller URI protocol. Prevents drive-by installs from web links. Default: enabled.",
            Tags = ["package", "appinstaller", "protocol", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSAppInstallerProtocol", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSAppInstallerProtocol")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSAppInstallerProtocol", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-auto-repair-apps",
            Label = "Disable Auto-Repair of Windows Apps",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically repairing broken UWP/MSIX apps. Default: enabled.",
            Tags = ["package", "auto-repair", "uwp", "msix"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAutomaticAppArchiving", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAutomaticAppArchiving")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAutomaticAppArchiving", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-shared-experiences",
            Label = "Disable Cross-Device App Experiences",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables shared experiences (app hand-off between devices). Default: enabled.",
            Tags = ["package", "shared-experiences", "cross-device", "cdp"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
        },
        // ── Command-based package management tweaks ────────────────────────
        new TweakDef
        {
            Id = "pkg-trust-psgallery",
            Label = "Trust PSGallery Repository",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the PowerShell Gallery as a trusted repository, eliminating installation prompts for modules.",
            Tags = ["package", "powershell", "psgallery", "trust", "module"],
            KindHint = TweakKind.PowerShell,
            ApplyAction = (_) =>
            {
                ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Trusted");
            },
            RemoveAction = (_) =>
            {
                ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Untrusted");
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.RunPowerShell("(Get-PSRepository -Name PSGallery).InstallationPolicy");
                return code == 0 && stdout.Trim().Equals("Trusted", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pkg-install-scoop",
            Label = "Install Scoop Package Manager",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Installs Scoop — a CLI package manager for Windows. Scoop installs apps to ~/scoop by default and requires no admin.",
            Tags = ["package", "scoop", "install", "cli"],
            KindHint = TweakKind.PackageManager,
            ApplyAction = (_) =>
            {
                ShellRunner.RunPowerShell(
                    "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force; " + "Invoke-RestMethod -Uri https://get.scoop.sh | Invoke-Expression"
                );
            },
            DetectAction = () =>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "shims", "scoop.ps1");
                return File.Exists(path);
            },
        },
        new TweakDef
        {
            Id = "pkg-update-powershellget",
            Label = "Update PowerShellGet to Latest",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Updates PowerShellGet module to the latest version for improved module management.",
            Tags = ["package", "powershell", "powershellget", "update"],
            KindHint = TweakKind.PackageManager,
            ApplyAction = (_) =>
            {
                ShellRunner.RunPowerShell("Install-Module -Name PowerShellGet -Force -AllowClobber -Scope CurrentUser");
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-Module -ListAvailable PowerShellGet | Sort-Object Version -Descending | Select-Object -First 1).Version.ToString()"
                );
                if (code != 0)
                    return false;
                // If version >= 2.2.5, consider "applied"
                return Version.TryParse(stdout.Trim(), out var ver) && ver >= new Version(2, 2, 5);
            },
        },
        new TweakDef
        {
            Id = "pkg-enable-winget",
            Label = "Enable WinGet App Installer",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the WinGet package manager via App Installer policy. Ensures WinGet is available on managed devices. Default: enabled.",
            Tags = ["packages", "winget", "enable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAppInstaller", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAppInstaller")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAppInstaller", 1)],
        },
        new TweakDef
        {
            Id = "pkg-npm-prefer-offline",
            Label = "NPM Prefer Offline Cache",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures npm to prefer cached packages over network requests. Speeds up installs when packages are already cached. Default: online first.",
            Tags = ["packages", "npm", "offline", "cache"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "NPM_CONFIG_PREFER_OFFLINE", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "NPM_CONFIG_PREFER_OFFLINE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "NPM_CONFIG_PREFER_OFFLINE", "true")],
        },
        new TweakDef
        {
            Id = "pkg-pip-disable-version-check",
            Label = "Disable pip Version Check",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pip from checking for newer versions on every run. Speeds up pip operations. Default: checks on every run.",
            Tags = ["packages", "pip", "version-check", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_DISABLE_PIP_VERSION_CHECK", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_DISABLE_PIP_VERSION_CHECK")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_DISABLE_PIP_VERSION_CHECK", "1")],
        },
        new TweakDef
        {
            Id = "pkg-pip-no-cache",
            Label = "Disable pip Cache",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pip download caching. Saves disk space at the cost of re-downloading packages. Default: caching enabled.",
            Tags = ["packages", "pip", "cache", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_NO_CACHE_DIR", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_NO_CACHE_DIR")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_NO_CACHE_DIR", "1")],
        },
        new TweakDef
        {
            Id = "pkg-pip-require-venv",
            Label = "Require Virtualenv for pip Install",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces pip to only install packages inside a virtual environment. Prevents accidental global installs. Default: allows global.",
            Tags = ["packages", "pip", "virtualenv", "safety"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_REQUIRE_VIRTUALENV", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_REQUIRE_VIRTUALENV")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_REQUIRE_VIRTUALENV", "1")],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-index",
            Label = "Set System pip Index URL",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default PyPI index URL for all users at the system level. Useful for corporate mirrors. Default: pypi.org.",
            Tags = ["packages", "pip", "index", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_INDEX_URL",
                    "https://pypi.org/simple"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_INDEX_URL")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_INDEX_URL",
                    "https://pypi.org/simple"
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-no-cache",
            Label = "Disable pip Cache (System)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables pip download caching at the system level for all users. Default: caching enabled.",
            Tags = ["packages", "pip", "cache", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_NO_CACHE_DIR", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_NO_CACHE_DIR")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_NO_CACHE_DIR", "1"),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-require-venv",
            Label = "Require Virtualenv for pip (System)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces pip to only install inside virtual environments at the system level for all users. Default: allows global.",
            Tags = ["packages", "pip", "virtualenv", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_REQUIRE_VIRTUALENV", "1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_REQUIRE_VIRTUALENV"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_REQUIRE_VIRTUALENV", "1"),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-trusted-host",
            Label = "Set pip Trusted Hosts (System)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets trusted pip hosts at the system level to bypass SSL verification. Useful for corporate proxies. Default: none.",
            Tags = ["packages", "pip", "trusted-host", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_TRUSTED_HOST",
                    "pypi.org files.pythonhosted.org"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_TRUSTED_HOST")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_TRUSTED_HOST",
                    "pypi.org files.pythonhosted.org"
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-timeout",
            Label = "Set pip Network Timeout",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the pip network timeout to 30 seconds. Prevents hangs on slow connections while allowing reasonable wait time. Default: 15 seconds.",
            Tags = ["packages", "pip", "timeout", "network"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_TIMEOUT", "30")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_TIMEOUT")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_TIMEOUT", "30")],
        },
        new TweakDef
        {
            Id = "pkg-pip-trusted-host",
            Label = "Set pip Trusted Hosts",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets trusted pip hosts for the current user to bypass SSL verification. Useful for corporate proxies. Default: none.",
            Tags = ["packages", "pip", "trusted-host", "ssl"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_TRUSTED_HOST", "pypi.org files.pythonhosted.org")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_TRUSTED_HOST")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_TRUSTED_HOST", "pypi.org files.pythonhosted.org")],
        },
        new TweakDef
        {
            Id = "pkg-pip-user-default",
            Label = "pip Install to User Site by Default",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures pip to install packages to the user site-packages directory by default. Avoids needing admin for pip install. Default: system site.",
            Tags = ["packages", "pip", "user", "install"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_USER", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_USER")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_USER", "1")],
        },
        new TweakDef
        {
            Id = "pkg-ps-gallery-trust",
            Label = "Trust PowerShell Gallery",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets the PSGallery repository as trusted to suppress install prompts. Default: untrusted.",
            Tags = ["packages", "powershell", "gallery", "trust"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Trusted"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Untrusted"),
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.RunPowerShell("(Get-PSRepository -Name PSGallery).InstallationPolicy");
                return code == 0 && stdout.Trim().Equals("Trusted", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pkg-ps-remotesigned",
            Label = "Set PowerShell ExecutionPolicy to RemoteSigned",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the PowerShell execution policy to RemoteSigned for the current user. Allows local scripts to run. Default: Restricted.",
            Tags = ["packages", "powershell", "execution-policy", "remotesigned"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-scoop-setup",
            Label = "Install Scoop Package Manager",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs the Scoop package manager for Windows. Provides command-line app installation without admin. Default: not installed.",
            Tags = ["packages", "scoop", "install", "setup"],
            ApplyAction = _ => ShellRunner.RunPowerShell("irm get.scoop.sh | iex"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall scoop"),
            DetectAction = () =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "shims", "scoop.ps1");
                return File.Exists(path);
            },
        },
        new TweakDef
        {
            Id = "pkg-winget-disable-auto-update",
            Label = "Disable WinGet Auto-Upgrade",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic package upgrades via WinGet auto-update policy. Prevents unattended app updates. Default: enabled.",
            Tags = ["packages", "winget", "auto-upgrade", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpgrade", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpgrade")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpgrade", 0)],
        },
        new TweakDef
        {
            Id = "pkg-winget-disable-msstore-source",
            Label = "Disable WinGet Microsoft Store Source",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Microsoft Store source in WinGet. Limits installs to winget community repository only. Default: enabled.",
            Tags = ["packages", "winget", "msstore", "source"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSStoreSource", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSStoreSource")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSStoreSource", 0)],
        },
    ];
}

// ── Merged from ScoopTools.cs ──────────────────────────────────────────────────

internal static class ScoopTools
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "scoop-disable-autoupdate",
            Label = "Disable Scoop Auto-Update on Install",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCOOP_NO_AUTO_UPDATE=1 to prevent Scoop from auto-updating itself before every app install. Default: auto-update. Recommended: disabled for speed.",
            Tags = ["scoop", "autoupdate", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
        },
        new TweakDef
        {
            Id = "scoop-parallel-downloads",
            Label = "Enable Scoop Parallel Downloads (aria2)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCOOP_ARIA2_ENABLED=true to enable parallel downloads via aria2 for faster Scoop package installs. Default: disabled. Recommended: enabled.",
            Tags = ["scoop", "parallel", "downloads", "aria2", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true")],
        },
        new TweakDef
        {
            Id = "scoop-set-global-install-dir",
            Label = "Set Scoop Global Install Directory",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the global Scoop install directory to C:\\Scoop via environment variable. Default: C:\\ProgramData\\scoop.",
            Tags = ["scoop", "global", "install", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\Scoop")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\Scoop")],
        },
        new TweakDef
        {
            Id = "scoop-set-cache-dir",
            Label = "Set Scoop Cache Directory",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Scoop download cache to C:\\ScoopCache. Keeps downloads separate from installs. Default: ~\\scoop\\cache.",
            Tags = ["scoop", "cache", "directory", "download"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE", @"C:\ScoopCache")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE", @"C:\ScoopCache")],
        },
        new TweakDef
        {
            Id = "scoop-enable-debug-mode",
            Label = "Enable Scoop Debug Mode",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Scoop debug output for troubleshooting install failures. Default: disabled.",
            Tags = ["scoop", "debug", "verbose", "troubleshooting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG", "true")],
        },
        new TweakDef
        {
            Id = "scoop-set-aria2-max-connections",
            Label = "Set Scoop Aria2 Max Connections to 16",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Scoop Aria2 max connections per server to 16. Speeds up downloads. Default: not set (Aria2 default is 1).",
            Tags = ["scoop", "aria2", "connections", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS", "-x 16 -s 16")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS", "-x 16 -s 16")],
        },
        new TweakDef
        {
            Id = "scoop-set-global-install-path",
            Label = "Set Scoop Global Install Path",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Scoop global apps install directory to C:\\ScoopGlobal. Keeps system programs organised. Default: %ProgramData%\\scoop.",
            Tags = ["scoop", "global", "install-path", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
        },
        new TweakDef
        {
            Id = "scoop-set-virustotal-api-key",
            Label = "Set Scoop VirusTotal API Key Variable",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the SCOOP_VIRUSTOTAL_API_KEY environment variable placeholder. Replace with your actual key for automatic malware scanning. Default: not set.",
            Tags = ["scoop", "virustotal", "security", "scanning"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY", "YOUR_API_KEY_HERE")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY", "YOUR_API_KEY_HERE")],
        },
        new TweakDef
        {
            Id = "scoop-disable-checkver",
            Label = "Disable Scoop Auto-Version Check",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets SCOOP_NO_CHECKVER=1 to skip automatic version checks. Speeds up 'scoop status'. Default: checks versions.",
            Tags = ["scoop", "checkver", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER", "1")],
        },
        new TweakDef
        {
            Id = "scoop-add-extras-bucket",
            Label = "Add Scoop Extras Bucket",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Adds the Scoop 'extras' bucket which contains popular GUI apps. Default: not added.",
            Tags = ["scoop", "extras", "bucket", "apps"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop bucket add extras"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop bucket rm extras"),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("scoop", ["bucket", "list"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "scoop-cleanup-all",
            Label = "Clean Up All Scoop Caches",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Runs scoop cleanup * and scoop cache rm * to free disk space from old versions and downloads.",
            Tags = ["scoop", "cleanup", "cache", "disk-space"],
            ApplyAction = _ =>
            {
                ShellRunner.RunPowerShell("scoop cleanup *");
                ShellRunner.RunPowerShell("scoop cache rm *");
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "scoop-install-aria2",
            Label = "Install Aria2 for Scoop Downloads",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs aria2 download manager for faster parallel Scoop downloads. Default: not installed.",
            Tags = ["scoop", "aria2", "download", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install aria2"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall aria2"),
            DetectAction = () =>
            {
                var scoopDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "aria2");
                return Directory.Exists(scoopDir);
            },
        },
        new TweakDef
        {
            Id = "scoop-7zip",
            Label = "Install 7-Zip via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs 7-Zip file archiver via Scoop. Supports 7z, ZIP, RAR, and many other formats.",
            Tags = ["scoop", "7zip", "archive", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install 7zip"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall 7zip"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "7zip")),
        },
        new TweakDef
        {
            Id = "scoop-bat",
            Label = "Install bat via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs bat — a cat clone with syntax highlighting and Git integration.",
            Tags = ["scoop", "bat", "cat", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install bat"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall bat"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "bat")),
        },
        new TweakDef
        {
            Id = "scoop-btop",
            Label = "Install btop via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs btop — a resource monitor with CPU, memory, disk, network, and process stats.",
            Tags = ["scoop", "btop", "monitor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install btop"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall btop"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "btop")),
        },
        new TweakDef
        {
            Id = "scoop-curl",
            Label = "Install curl via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs curl — command-line tool for transferring data with URLs.",
            Tags = ["scoop", "curl", "http", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install curl"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall curl"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "curl")),
        },
        new TweakDef
        {
            Id = "scoop-delta",
            Label = "Install delta via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs delta — a syntax-highlighting pager for git, diff, and grep output.",
            Tags = ["scoop", "delta", "diff", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install delta"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall delta"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "delta")),
        },
        new TweakDef
        {
            Id = "scoop-duf",
            Label = "Install duf via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs duf — a better df alternative for viewing disk usage with a modern UI.",
            Tags = ["scoop", "duf", "disk", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install duf"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall duf"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "duf")),
        },
        new TweakDef
        {
            Id = "scoop-dust",
            Label = "Install dust via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs dust — a more intuitive version of du showing disk usage as a tree.",
            Tags = ["scoop", "dust", "disk-usage", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install dust"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall dust"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "dust")),
        },
        new TweakDef
        {
            Id = "scoop-everything",
            Label = "Install Everything via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Everything — instant file search engine for Windows with real-time indexing.",
            Tags = ["scoop", "everything", "search", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install everything"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall everything"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "everything")),
        },
        new TweakDef
        {
            Id = "scoop-fd",
            Label = "Install fd via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs fd — a fast and user-friendly alternative to find.",
            Tags = ["scoop", "fd", "find", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install fd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall fd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "fd")),
        },
        new TweakDef
        {
            Id = "scoop-fzf",
            Label = "Install fzf via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs fzf — a general-purpose command-line fuzzy finder.",
            Tags = ["scoop", "fzf", "fuzzy", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install fzf"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall fzf"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "fzf")),
        },
        new TweakDef
        {
            Id = "scoop-git",
            Label = "Install Git via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Git distributed version control system via Scoop.",
            Tags = ["scoop", "git", "vcs", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install git"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall git"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "git")),
        },
        new TweakDef
        {
            Id = "scoop-gsudo",
            Label = "Install gsudo via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gsudo — a sudo equivalent for Windows that elevates commands inline.",
            Tags = ["scoop", "gsudo", "sudo", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gsudo"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gsudo"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gsudo")),
        },
        new TweakDef
        {
            Id = "scoop-hyperfine",
            Label = "Install hyperfine via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs hyperfine — a command-line benchmarking tool with statistical analysis.",
            Tags = ["scoop", "hyperfine", "benchmark", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install hyperfine"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall hyperfine"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "hyperfine")),
        },
        new TweakDef
        {
            Id = "scoop-jq",
            Label = "Install jq via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs jq — a lightweight command-line JSON processor.",
            Tags = ["scoop", "jq", "json", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install jq"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall jq"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "jq")),
        },
        new TweakDef
        {
            Id = "scoop-lazygit",
            Label = "Install lazygit via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs lazygit — a simple terminal UI for Git commands.",
            Tags = ["scoop", "lazygit", "git", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install lazygit"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall lazygit"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "lazygit")),
        },
        new TweakDef
        {
            Id = "scoop-neovim",
            Label = "Install Neovim via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Neovim — a hyperextensible Vim-based text editor.",
            Tags = ["scoop", "neovim", "editor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install neovim"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall neovim"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "neovim")),
        },
        new TweakDef
        {
            Id = "scoop-nodejs",
            Label = "Install Node.js via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Node.js JavaScript runtime via Scoop.",
            Tags = ["scoop", "nodejs", "javascript", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install nodejs"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall nodejs"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "nodejs")),
        },
        new TweakDef
        {
            Id = "scoop-python",
            Label = "Install Python via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Python interpreter via Scoop.",
            Tags = ["scoop", "python", "interpreter", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install python"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall python"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "python")),
        },
        new TweakDef
        {
            Id = "scoop-ripgrep",
            Label = "Install ripgrep via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs ripgrep — a line-oriented search tool that recursively searches directories.",
            Tags = ["scoop", "ripgrep", "search", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install ripgrep"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall ripgrep"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "ripgrep")),
        },
        new TweakDef
        {
            Id = "scoop-set-global-path",
            Label = "Add Scoop Global Apps to PATH",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds the Scoop global apps directory to the system PATH. Allows globally installed Scoop apps to be available to all users. Default: not in PATH.",
            Tags = ["scoop", "global", "path", "environment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetExpandString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "SCOOP_GLOBAL",
                    @"%ProgramData%\scoop"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "SCOOP_GLOBAL")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "SCOOP_GLOBAL",
                    @"%ProgramData%\scoop"
                ),
            ],
        },
        new TweakDef
        {
            Id = "scoop-starship",
            Label = "Install Starship via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Starship — a minimal, blazing-fast cross-shell prompt.",
            Tags = ["scoop", "starship", "prompt", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install starship"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall starship"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "starship")),
        },
        new TweakDef
        {
            Id = "scoop-tldr",
            Label = "Install tldr via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tldr — simplified and community-driven man pages.",
            Tags = ["scoop", "tldr", "man", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tldr"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tldr"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tldr")),
        },
        new TweakDef
        {
            Id = "scoop-wget",
            Label = "Install wget via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs wget — a non-interactive network downloader for HTTP, HTTPS, and FTP.",
            Tags = ["scoop", "wget", "download", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install wget"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall wget"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "wget")),
        },
        new TweakDef
        {
            Id = "scoop-zoxide",
            Label = "Install zoxide via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs zoxide — a smarter cd command that learns your most-used directories and jumps to them intelligently.",
            Tags = ["scoop", "zoxide", "navigation", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install zoxide"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall zoxide"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "zoxide")),
        },
        new TweakDef
        {
            Id = "scoop-lsd",
            Label = "Install lsd via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs lsd — a modern ls replacement with icons, colours, and tree view.",
            Tags = ["scoop", "lsd", "files", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install lsd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall lsd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "lsd")),
        },
        new TweakDef
        {
            Id = "scoop-sd",
            Label = "Install sd via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs sd — a modern sed/awk replacement for intuitive find-and-replace operations.",
            Tags = ["scoop", "sd", "text", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install sd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall sd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "sd")),
        },
        new TweakDef
        {
            Id = "scoop-procs",
            Label = "Install procs via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs procs — a modern replacement for ps that shows processes with colour-coded resource usage.",
            Tags = ["scoop", "procs", "processes", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install procs"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall procs"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "procs")),
        },
        new TweakDef
        {
            Id = "scoop-bottom",
            Label = "Install bottom via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs bottom (btm) — a graphical system monitor for CPU, RAM, disk, and network.",
            Tags = ["scoop", "bottom", "monitor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install bottom"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall bottom"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "bottom")),
        },
        new TweakDef
        {
            Id = "scoop-xh",
            Label = "Install xh via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs xh — a friendly and fast HTTP client similar to HTTPie.",
            Tags = ["scoop", "xh", "http", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install xh"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall xh"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "xh")),
        },
        new TweakDef
        {
            Id = "scoop-gping",
            Label = "Install gping via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gping — a graphical ping tool that displays network latency as a real-time graph.",
            Tags = ["scoop", "gping", "network", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gping"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gping"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gping")),
        },
        new TweakDef
        {
            Id = "scoop-tokei",
            Label = "Install tokei via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tokei — a fast code statistics tool that counts lines of code by language.",
            Tags = ["scoop", "tokei", "code", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tokei"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tokei"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tokei")),
        },
        new TweakDef
        {
            Id = "scoop-tealdeer",
            Label = "Install tealdeer via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tealdeer — a fast Rust-based tldr pages client for quick command summaries.",
            Tags = ["scoop", "tealdeer", "tldr", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tealdeer"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tealdeer"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tealdeer")),
        },
        new TweakDef
        {
            Id = "scoop-carapace-bin",
            Label = "Install carapace-bin via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs carapace-bin — a multi-shell completion engine supporting PowerShell, bash, zsh, and more.",
            Tags = ["scoop", "carapace", "completion", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install carapace-bin"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall carapace-bin"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "carapace-bin")),
        },
        new TweakDef
        {
            Id = "scoop-eza",
            Label = "Install eza via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs eza — a modern, maintained replacement for ls with colour output, icons, and git integration. Replaces the deprecated exa.",
            Tags = ["scoop", "eza", "ls", "files", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install eza"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall eza"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "eza")),
        },
        new TweakDef
        {
            Id = "scoop-yazi",
            Label = "Install yazi via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs yazi — a blazing-fast terminal file manager written in Rust with async I/O and vim-style navigation.",
            Tags = ["scoop", "yazi", "file-manager", "terminal", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install yazi"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall yazi"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "yazi")),
        },
        new TweakDef
        {
            Id = "scoop-helix",
            Label = "Install Helix editor via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs Helix — a post-modern modal text editor inspired by Kakoune and Neovim, with built-in LSP and tree-sitter support.",
            Tags = ["scoop", "helix", "editor", "modal", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install helix"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall helix"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "helix")),
        },
        new TweakDef
        {
            Id = "scoop-nushell",
            Label = "Install Nushell via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Nushell (nu) — a modern shell with structured, type-aware pipelines. Treats output as tables, not plain text.",
            Tags = ["scoop", "nushell", "shell", "nu", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install nu"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall nu"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "nu")),
        },
        new TweakDef
        {
            Id = "scoop-zellij",
            Label = "Install Zellij via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Zellij — a terminal workspace with panes, tabs, and a plugin system. Modern Rust-based tmux alternative.",
            Tags = ["scoop", "zellij", "terminal", "multiplexer", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install zellij"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall zellij"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "zellij")),
        },
        new TweakDef
        {
            Id = "scoop-gitoxide",
            Label = "Install gitoxide (gix) via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gitoxide — a pure Rust git implementation providing the gix CLI tool for fast, safe git operations.",
            Tags = ["scoop", "gitoxide", "git", "rust", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gitoxide"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gitoxide"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gitoxide")),
        },
        new TweakDef
        {
            Id = "scoop-watchexec",
            Label = "Install watchexec via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs watchexec — a tool that watches files for changes and re-runs a command automatically. Ideal for dev and build workflows.",
            Tags = ["scoop", "watchexec", "watch", "automation", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install watchexec"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall watchexec"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "watchexec")),
        },
        new TweakDef
        {
            Id = "scoop-topgrade",
            Label = "Install topgrade via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs topgrade — a one-command upgrade tool for all package managers, shells, plugins, and tools on the system.",
            Tags = ["scoop", "topgrade", "upgrade", "maintenance", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install topgrade"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall topgrade"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "topgrade")),
        },
        new TweakDef
        {
            Id = "scoop-pueue",
            Label = "Install pueue via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs pueue — a background task queue manager for long-running shell commands with pause, abort, and log features.",
            Tags = ["scoop", "pueue", "task-queue", "background", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install pueue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall pueue"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "pueue")),
        },
        new TweakDef
        {
            Id = "scoop-oha",
            Label = "Install oha via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs oha — a fast HTTP load generator written in Rust for benchmarking web endpoints from the command line.",
            Tags = ["scoop", "oha", "http", "load-test", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install oha"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall oha"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "oha")),
        },
    ];
}

// ── merged from Java.cs ──
internal static class Java
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "java-security-high",
            Label = "Java: Set Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Raises the Java security level to VERY_HIGH, blocking unsigned applets.",
            Tags = ["java", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-tip-of-day",
            Label = "Disable Java Tip of the Day",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Tip of the Day' pop-up dialog in Java Control Panel.",
            Tags = ["java", "ui", "annoyance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-update-check",
            Label = "Disable Java Auto-Update Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Java's automatic update check at startup. Reduces background network traffic. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["java", "update", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "NotifyDownload", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "NotifyDownload", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
        },
        new TweakDef
        {
            Id = "java-high-perf-graphics",
            Label = "Java High Performance Graphics",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables hardware graphics acceleration for Java/JavaFX applications. Improves rendering performance. Default: Software. Recommended: Hardware.",
            Tags = ["java", "graphics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "java-disable-sponsor-offers",
            Label = "Disable Java Sponsor Offers",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables sponsor/adware offers bundled with Java updates. Default: Enabled. Recommended: Disabled.",
            Tags = ["java", "sponsor", "adware", "offers"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-update-scheduler",
            Label = "Disable Java Update Scheduler Notifications",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java update scheduler download/install notifications. Default: Enabled. Recommended: Disabled.",
            Tags = ["java", "update", "scheduler", "notifications"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "NotifyDownload", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "NotifyDownload", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
        },
        new TweakDef
        {
            Id = "java-security-veryhigh",
            Label = "Set Java Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Java deployment security level to VERY_HIGH via policy. Default: HIGH. Recommended: VERY_HIGH.",
            Tags = ["java", "security", "deployment", "veryhigh"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "HIGH")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-usage-tracking",
            Label = "Disable Java Usage Tracking",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java usage tracker analytics. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["java", "usage", "tracking", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled", "false")],
        },
        new TweakDef
        {
            Id = "java-set-high-security",
            Label = "Set Java Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Java Web Start / applet security level to Very High. Only signed and trusted apps run. Default: High.",
            Tags = ["java", "security", "level", "applet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.level", "HIGH")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH")],
        },
        new TweakDef
        {
            Id = "java-disable-web-plugin",
            Label = "Disable Java Browser Plugin",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Java browser plugin (applets). Reduces browser attack surface. Default: enabled.",
            Tags = ["java", "browser", "plugin", "applet", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
        },
        new TweakDef
        {
            Id = "java-disable-log-file",
            Label = "Disable Java Console Log File",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java console log file creation. Reduces disk writes from Java applications. Default: enabled.",
            Tags = ["java", "console", "log", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.logFileName", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.logFileName")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.logFileName", "")],
        },
        new TweakDef
        {
            Id = "java-set-high-dpi-awareness",
            Label = "Enable Java High DPI Awareness",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables high-DPI awareness for Java applications. Prevents blurry rendering on HiDPI displays. Default: system-aware.",
            Tags = ["java", "dpi", "hidpi", "scaling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javafx.highDPIAware", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javafx.highDPIAware")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javafx.highDPIAware", "true")],
        },
        new TweakDef
        {
            Id = "java-disable-usage-tracker",
            Label = "Disable Java Usage Tracker",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Java Usage Tracker that reports Java runtime usage data to Oracle. Default: enabled.",
            Tags = ["java", "usage", "tracker", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-auto-update",
            Label = "Disable Java Auto-Update",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java automatic update checks. Prevents background update service from consuming resources. Default: enabled.",
            Tags = ["java", "update", "auto-update", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-java-cert-revoke",
            Label = "Disable Java Certificate Revocation Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java certificate revocation list checking. Speeds up Java applet loading but reduces security. Default: enabled.",
            Tags = ["java", "certificate", "revocation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "true")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-error-reporting",
            Label = "Disable Java Error Reporting",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java crash and error reporting to Oracle. Prevents error data from being sent externally. Default: enabled.",
            Tags = ["java", "error", "reporting", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.user.security.exception.sites", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.user.security.exception.sites")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.user.security.exception.sites", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-sponsor",
            Label = "Disable Java Sponsor Offers",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables sponsor offers (toolbars, search engines) during Java updates. Prevents bundled software installation. Default: enabled.",
            Tags = ["java", "sponsor", "offers", "bloatware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-java-tracking",
            Label = "Disable Java Analytics Tracking",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java analytics and tracking features. Prevents collection of usage patterns by Oracle. Default: enabled.",
            Tags = ["java", "analytics", "tracking", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "false")],
        },
        new TweakDef
        {
            Id = "java-disable-java-update",
            Label = "Disable Java Update Service",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Java Update Scheduler (jusched.exe) at the Run key level. Prevents background update checks. Default: enabled.",
            Tags = ["java", "update", "scheduler", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-java-web-plugin",
            Label = "Disable Java Web Browser Plugin",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Java browser plugin for all browsers. Reduces attack surface from browser-based Java exploits. Default: enabled.",
            Tags = ["java", "browser", "plugin", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
        },
        new TweakDef
        {
            Id = "java-high-dpi",
            Label = "Enable Java High DPI Scaling",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables high DPI scaling awareness for Java applications. Prevents blurry rendering on high-resolution displays. Default: not set.",
            Tags = ["java", "dpi", "scaling", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties",
                    "deployment.javaws.jre.platform.version",
                    "sun.java2d.uiScale.enabled=true"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.jre.platform.version")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties",
                    "deployment.javaws.jre.platform.version",
                    "sun.java2d.uiScale.enabled=true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-installer-sponsor",
            Label = "Disable Java Sponsor Offers",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Prevents Java installer from showing third-party sponsor offers (e.g., Ask Toolbar). Default: enabled.",
            Tags = ["java", "sponsor", "ads", "installer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft", "SPONSORS", "DISABLE")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft", "SPONSORS")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft", "SPONSORS", "DISABLE")],
        },
        new TweakDef
        {
            Id = "java-disable-auto-update-notify",
            Label = "Disable Java Auto-Update Notification",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Prevents Java from checking for updates and showing update notifications. Default: enabled.",
            Tags = ["java", "update", "notification"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-tls-10",
            Label = "Disable TLS 1.0 in Java",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables TLS 1.0 in Java deployment properties. TLS 1.0 is deprecated. Default: enabled.",
            Tags = ["java", "tls", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-tls-11",
            Label = "Disable TLS 1.1 in Java",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables TLS 1.1 in Java deployment properties. TLS 1.1 is deprecated. Default: enabled.",
            Tags = ["java", "tls", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1.1", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1.1")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1.1", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-security-very-high",
            Label = "Set Java Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Sets the Java security slider to Very High, requiring all applets to be signed and valid. Default: High.",
            Tags = ["java", "security", "applet", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-browser-plugin",
            Label = "Disable Java Browser Plugin",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables the Java browser plugin. Java applets in browsers are obsolete and a security risk. Default: enabled.",
            Tags = ["java", "browser", "plugin", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-certificate-revocation",
            Label = "Enable Certificate Revocation Checking",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Enables certificate revocation checking via CRL and OCSP in Java. Default: enabled.",
            Tags = ["java", "certificate", "revocation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.crl"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.validation.crl",
                    "true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-ocsp",
            Label = "Enable OCSP Certificate Checking",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Enables Online Certificate Status Protocol (OCSP) checking for Java certificates. Default: enabled.",
            Tags = ["java", "ocsp", "certificate", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.ocsp", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.ocsp"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.validation.ocsp",
                    "true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-jnlp-association",
            Label = "Disable JNLP File Association",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables Java Web Start JNLP file association. Prevents accidental launch of Web Start apps. Default: enabled.",
            Tags = ["java", "jnlp", "web-start", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "NEVER"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "NEVER"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-console-output",
            Label = "Disable Java Console Output",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Hides the Java console for deployed applications. Reduces clutter for end users. Default: show console.",
            Tags = ["java", "console", "output"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "HIDE"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "HIDE"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-proxy-direct",
            Label = "Set Java Proxy to Direct (No Proxy)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Java to use a direct connection (no proxy). Default: uses browser proxy settings.",
            Tags = ["java", "proxy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.proxy.type", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.proxy.type")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.proxy.type", "0")],
        },
        new TweakDef
        {
            Id = "java-set-cache-max-100mb",
            Label = "Set Java Cache Max Size to 100 MB",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the Java deployment cache to 100 MB. Prevents unbounded cache growth on developer systems. Default: unlimited.",
            Tags = ["java", "cache", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.max.size.file.mb", "100"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.max.size.file.mb"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.max.size.file.mb", "100"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-webstart-splash",
            Label = "Disable Java Web Start Splash Screen",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the splash screen shown when launching Java Web Start applications. Default: splash screen shown.",
            Tags = ["java", "webstart", "splash", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.splash.enabled", "false"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.splash.enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.splash.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-connect-timeout-10s",
            Label = "Set Java Socket Connection Timeout to 10s",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Java socket connection timeout to 10 seconds. Prevents indefinite hangs when connecting to unreachable resources.",
            Tags = ["java", "timeout", "socket", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.connect", "10000"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.connect"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.timeout.socket.connect",
                    "10000"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-set-read-timeout-30s",
            Label = "Set Java Socket Read Timeout to 30s",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Java socket read timeout to 30 seconds. Prevents indefinite hangs when reading from slow or hung resources.",
            Tags = ["java", "timeout", "socket", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.read", "30000"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.read")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.read", "30000"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-update-check-interval",
            Label = "Disable Java Update Check Interval",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables periodic Java update check background scheduling. Prevents background processes polling for updates. Default: periodic checks enabled.",
            Tags = ["java", "update", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.update.check.interval.days", "-1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.update.check.interval.days"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.update.check.interval.days",
                    "-1"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-eula-check",
            Label = "Disable Java EULA Check on First Run",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Marks Java EULA as accepted to suppress the first-run EULA dialog in enterprise deployments. Default: shows EULA on first launch.",
            Tags = ["java", "eula", "first-run", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.eula.dismissed", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.eula.dismissed")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.eula.dismissed", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-application-description",
            Label = "Disable Java Application Description Prompt",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the application description tooltip shown when launching Java Web Start applications. Default: shown.",
            Tags = ["java", "ui", "prompt", "silent"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.application.description.shown",
                    "false"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.application.description.shown"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.application.description.shown",
                    "false"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-set-concurrent-downloads-3",
            Label = "Set Java Concurrent Downloads to 3",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows Java to download up to 3 resources concurrently. Improves load time for Java apps with many classpath resources. Default: 1.",
            Tags = ["java", "download", "concurrency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.concurrent.downloads", "3"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.concurrent.downloads")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.concurrent.downloads", "3"),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-strict-security",
            Label = "Enable Java Strict Security Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables strict security validation for Java deployments. Enforces all certificate and permission checks. Default: standard mode.",
            Tags = ["java", "security", "strict", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.strict.mode", "true"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.strict.mode")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.strict.mode", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-web-java",
            Label = "Disable Java in Web Browsers",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Java execution in web browsers via the deployment policy. Prevents Java applets from running in any browser with the Java plugin. Default: web Java enabled.",
            Tags = ["java", "browser", "web", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-lock-security-level",
            Label = "Lock Java Security Level (Prevent User Change)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Locks the Java security level setting so users cannot lower it. Prevents accidental or deliberate reduction of Java security from the Java Control Panel. Default: unlocked.",
            Tags = ["java", "security", "lock", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level.locked", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level.locked"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level.locked", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-console-autostart",
            Label = "Disable Java Console Auto-Start",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Java console startup mode to NEVER so it does not automatically open during applet execution. Reduces UI clutter in production and user environments. Default: HIDE.",
            Tags = ["java", "console", "startup", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "NEVER"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "NEVER"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-revocation-all-certs",
            Label = "Enable Java Revocation Check for All Certificates",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Java to check certificate revocation for all certificates in the chain (not just the end entity). Provides stronger PKI validation. Default: PUBLISHER_ONLY.",
            Tags = ["java", "security", "revocation", "pki"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.revocation.check",
                    "ALL_CERTIFICATES"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.revocation.check"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.revocation.check",
                    "ALL_CERTIFICATES"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-lock-update-check",
            Label = "Lock Java Update Check Setting",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Locks the Java update check setting via deployment policy so users cannot re-enable automatic update checking. Complements java-disable-auto-update. Default: unlocked.",
            Tags = ["java", "update", "lock", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.updatecheck.locked", "true"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.updatecheck.locked")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.updatecheck.locked", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-plugin-session-lifetime",
            Label = "Set Java Plugin Session Lifetime Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Java plugin credential and session lifetime to SESSION mode so temporary data is cleared when the browser exits. Reduces residual data exposure. Default: FOREVER.",
            Tags = ["java", "session", "privacy", "plugin"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.plugin.lifetime", "SESSION"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.plugin.lifetime")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.plugin.lifetime", "SESSION"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-jre-auto-install",
            Label = "Disable Automatic JRE Installation",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic JRE installation triggered by Java Web Start or the browser plugin. Prevents Java from downloading and installing JRE versions without admin consent. Default: auto-install enabled.",
            Tags = ["java", "install", "auto-update", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.jre.install.enabled", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.jre.install.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.jre.install.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-blacklist-revocation",
            Label = "Enable Java Blacklist Revocation Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables checking of Java's built-in certificate blacklist for revoked or compromised certificates during applet launch. Default: enabled (explicit policy reinforces it).",
            Tags = ["java", "security", "blacklist", "revocation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.blacklist.check", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.blacklist.check"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.blacklist.check",
                    "true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-applet-caching",
            Label = "Disable Java Applet Cache",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Java deployment cache for applets and Web Start applications. Prevents local caching of Java class files and reduces disk exposure from cached untrusted code. Default: cache enabled.",
            Tags = ["java", "cache", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.enabled", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-lock-expiration-check",
            Label = "Lock Java JRE Expiration Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Locks the Java JRE expiration check via deployment policy so users cannot disable the warning when running an expired or outdated JRE version. Default: unlocked.",
            Tags = ["java", "expiration", "lock", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.expiration.check.locked", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.expiration.check.locked"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.expiration.check.locked",
                    "true"
                ),
            ],
        },
    ];
}

internal static class WindowsTerminal
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "term-enable-console-v2",
            Label = "Enable Console V2 Host",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces the new Console V2 host with ANSI support, line wrapping, and improved rendering. Default: 1 (enabled). Recommended: 1.",
            Tags = ["terminal", "console", "v2", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-vt-processing",
            Label = "Enable Virtual Terminal (ANSI) Processing",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables VT100/ANSI escape sequence processing in the console. Required for colored output in many CLI tools. Default: 0 (off). Recommended: 1 (on).",
            Tags = ["terminal", "vt100", "ansi", "colors", "escape"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-quick-edit",
            Label = "Disable Quick Edit Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Quick Edit so clicking the console window does not pause running commands. Prevents accidental hangs. Default: 1 (on). Recommended: 0 (off).",
            Tags = ["terminal", "quickedit", "console", "hang"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-insert-mode",
            Label = "Enable Insert Mode by Default",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets insert mode as the default typing mode in consoles. Default: 1 (insert). Recommended: 1.",
            Tags = ["terminal", "insert", "mode", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-line-wrap",
            Label = "Enable Line Wrapping",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables automatic line wrapping when resizing the console. Default: 1. Recommended: 1.",
            Tags = ["terminal", "wrap", "resize", "console"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-legacy-console",
            Label = "Disable Legacy Console Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the legacy console subsystem. Required for Console V2 features like ANSI escape support. Default: 0 (modern). Recommended: 0.",
            Tags = ["terminal", "legacy", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
        },
        new TweakDef
        {
            Id = "term-console-font-cascadia",
            Label = "Set Console Font to Cascadia Mono",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the default console font to Cascadia Mono at 16pt. Bundled with Windows Terminal; supports ligatures. Default: Consolas 14pt. Recommended: Cascadia Mono 16pt.",
            Tags = ["terminal", "font", "cascadia", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Console", "FaceName", "Cascadia Mono"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontFamily", 54),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Console", "FaceName", "Consolas"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontFamily", 54),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Console", "FaceName", "Cascadia Mono")],
        },
        new TweakDef
        {
            Id = "term-enable-ctrl-cv",
            Label = "Enable Ctrl+Shift+C/V Copy-Paste",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Ctrl+Shift+C / Ctrl+Shift+V clipboard shortcuts in the classic console. Also enables filter-on-paste and line selection. Default: enabled. Recommended: enabled.",
            Tags = ["terminal", "copy", "paste", "keyboard", "shortcuts"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FilterOnPaste", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineSelection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "FilterOnPaste"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "LineSelection"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 0)],
        },
        new TweakDef
        {
            Id = "term-set-window-opacity",
            Label = "Set Console Window Opacity (95%)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets console window to 95% opacity for slight transparency. Default: 255 (opaque). Recommended: 242 (95%).",
            Tags = ["terminal", "opacity", "transparency", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
        },
        new TweakDef
        {
            Id = "term-set-default-wt",
            Label = "Set Default Terminal to Windows Terminal (Win11)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows Terminal as the default terminal via the Win11 UseNewTerminal setting. Default: Let Windows decide (0). Recommended: Windows Terminal (1).",
            Tags = ["terminal", "default", "windows-terminal", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationTerminal"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-splash",
            Label = "Disable Terminal Splash Screen",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Terminal splash/startup screen via policy. Default: Enabled. Recommended: Disabled for faster launch.",
            Tags = ["terminal", "splash", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
        },
        new TweakDef
        {
            Id = "term-set-default-terminal-wt",
            Label = "Set Windows Terminal as Default Console App",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows Terminal as the default terminal application for all console programs. Default: Windows Console Host.",
            Tags = ["terminal", "default", "console", "wt"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console\%%Startup"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}")],
        },
        new TweakDef
        {
            Id = "term-enable-acrylic-background",
            Label = "Enable Terminal Acrylic Background via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables acrylic (translucent) background in Windows Terminal via machine policy. Default: disabled.",
            Tags = ["terminal", "acrylic", "background", "appearance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-bell",
            Label = "Disable Terminal Bell Sound via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the bell (beep) sound in Windows Terminal. Default: enabled.",
            Tags = ["terminal", "bell", "beep", "sound"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle", 0)],
        },
        new TweakDef
        {
            Id = "term-set-default-profile-pwsh",
            Label = "Set Default Shell to PowerShell 7 via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default shell profile in Windows Terminal to PowerShell 7 via machine policy. Default: Windows PowerShell 5.1.",
            Tags = ["terminal", "default", "powershell", "profile"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal",
                    "DefaultProfile",
                    "{574e775e-4f2a-5b96-ac1e-a2962a402336}"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DefaultProfile")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal",
                    "DefaultProfile",
                    "{574e775e-4f2a-5b96-ac1e-a2962a402336}"
                ),
            ],
        },
        new TweakDef
        {
            Id = "term-disable-automatic-updates",
            Label = "Disable Windows Terminal Auto-Update",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates for Windows Terminal. Use winget or manual update instead. Default: auto-update.",
            Tags = ["terminal", "update", "auto", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "term-campbell-color-scheme",
            Label = "Set Windows Terminal Campbell Theme",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Windows Terminal default color scheme to Campbell. Provides a consistent dark theme across profiles. Default: system default.",
            Tags = ["terminal", "campbell", "theme", "color"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ColorTable00", unchecked((int)0x0C0C0C))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ColorTable00")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ColorTable00", unchecked((int)0x0C0C0C))],
        },
        new TweakDef
        {
            Id = "term-default-windows-terminal",
            Label = "Set Windows Terminal as Default Console",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows Terminal as the default console host instead of legacy conhost.exe. Enables modern features and tabs. Default: conhost.",
            Tags = ["terminal", "default", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console\%%Startup"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-always-on-top",
            Label = "Enable Terminal Always On Top",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables always-on-top mode for console windows. Terminal stays above other windows. Useful for monitoring output. Default: disabled.",
            Tags = ["terminal", "always-on-top", "window", "pin"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop", 1)],
        },
        new TweakDef
        {
            Id = "term-large-buffer",
            Label = "Set Terminal Large Scrollback Buffer",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the console scrollback buffer to 9999 lines. Allows reviewing more terminal output history. Default: 300 lines.",
            Tags = ["terminal", "buffer", "scrollback", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 9999)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 300)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 9999)],
        },
        new TweakDef
        {
            Id = "term-set-cursor-block",
            Label = "Set Terminal Block Cursor",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the console cursor shape to a solid block. More visible than the default underscore cursor. Default: underscore.",
            Tags = ["terminal", "cursor", "block", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CursorType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
        },
        new TweakDef
        {
            Id = "term-disable-console-quick-edit",
            Label = "Disable Quick Edit Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables Quick Edit mode in the console, preventing accidental selection pauses. Default: enabled.",
            Tags = ["console", "quick-edit", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "QuickEdit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-resize-line-wrap",
            Label = "Enable Line Wrap on Resize",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Enables line wrapping when the console window is resized. Default: disabled (truncate).",
            Tags = ["console", "line-wrap", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "LineWrap")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
        },
        new TweakDef
        {
            Id = "term-set-font-weight-bold",
            Label = "Set Console Font Weight to Bold",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the console font weight to bold (700). Improves readability on high-DPI displays. Default: normal (400).",
            Tags = ["console", "font", "bold", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontWeight", 700)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "FontWeight")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "FontWeight", 700)],
        },
        new TweakDef
        {
            Id = "term-disable-scroll-forward",
            Label = "Disable Forward Scrolling",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables the ability to scroll forward past the current output. Default: enabled.",
            Tags = ["console", "scroll", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForwardScroll", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ForwardScroll")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForwardScroll", 0)],
        },
        new TweakDef
        {
            Id = "term-force-insert-mode",
            Label = "Enable Insert Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Enables insert mode in the console host, so typed text inserts rather than overwrites. Default: enabled.",
            Tags = ["console", "insert", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "InsertMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
        },
        new TweakDef
        {
            Id = "term-set-window-alpha-opaque",
            Label = "Set Console Window Fully Opaque",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the console window transparency to fully opaque (255). Default: 255.",
            Tags = ["console", "transparency", "opacity", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "WindowAlpha")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
        },
        new TweakDef
        {
            Id = "term-disable-ctrl-key-shortcuts",
            Label = "Disable Ctrl Key Shortcuts",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables Ctrl+C/Ctrl+V shortcuts in the legacy console host. Useful when Ctrl+C is needed for SIGINT. Default: enabled.",
            Tags = ["console", "ctrl", "shortcuts", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-trim-leading-zeros",
            Label = "Enable Trim Leading Zeros",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Trims leading zeros when double-clicking to select numbers in the console. Default: disabled.",
            Tags = ["console", "selection", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros", 1)],
        },
        new TweakDef
        {
            Id = "term-set-history-buffer-999",
            Label = "Set History Buffer Size to 999",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Increases the command history buffer to 999 entries (maximum). Default: 50.",
            Tags = ["console", "history", "buffer", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 999)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 999)],
        },
        new TweakDef
        {
            Id = "term-disable-number-of-history-buffers",
            Label = "Set Number of History Buffers to 4",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the number of history buffers to 4 (one per console process). Default: 4.",
            Tags = ["console", "history", "buffer", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers", 4)],
        },
    ];
}

// === Merged from: PowerShellTweaks.cs ===

/// <summary>
/// Tweaks executed via PowerShell cmdlets (Set-Service, Get-Service, Enable-WindowsOptionalFeature, etc.).
/// These use ApplyAction/RemoveAction/DetectAction delegates via ShellRunner.RunPowerShell.
/// </summary>
internal static class PowerShellTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Service control via PowerShell ───────────────────────────────
        new TweakDef
        {
            Id = "ps-disable-print-spooler",
            Label = "Disable Print Spooler Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Stops and disables the Print Spooler service. Closes the PrintNightmare attack vector on machines without printers.",
            Tags = ["powershell", "service", "security", "print"],
            SideEffects = "Printing will be unavailable.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name Spooler -Force -ErrorAction SilentlyContinue; Set-Service -Name Spooler -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name Spooler -StartupType Automatic; Start-Service -Name Spooler"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name Spooler -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-remote-registry",
            Label = "Disable Remote Registry Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Stops and disables the Remote Registry service to prevent remote access to the registry.",
            Tags = ["powershell", "service", "security", "remote"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name RemoteRegistry -Force -ErrorAction SilentlyContinue; Set-Service -Name RemoteRegistry -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name RemoteRegistry -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name RemoteRegistry -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-fax-service",
            Label = "Disable Fax Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Fax service — unnecessary on most modern systems.",
            Tags = ["powershell", "service", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell("Stop-Service -Name Fax -Force -ErrorAction SilentlyContinue; Set-Service -Name Fax -StartupType Disabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name Fax -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name Fax -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-xbox-services",
            Label = "Disable Xbox Live Services",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables Xbox Live Auth, Networking, and Game Save services. Safe if not using Xbox features.",
            Tags = ["powershell", "service", "xbox", "gaming", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($svc in @('XblAuthManager','XblGameSave','XboxNetApiSvc','XboxGipSvc')) { Stop-Service -Name $svc -Force -ErrorAction SilentlyContinue; Set-Service -Name $svc -StartupType Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($svc in @('XblAuthManager','XblGameSave','XboxNetApiSvc','XboxGipSvc')) { Set-Service -Name $svc -StartupType Manual -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name XblAuthManager -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── System optimisation via PowerShell ──────────────────────────
        new TweakDef
        {
            Id = "ps-clear-temp-files",
            Label = "Clear Temporary Files",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes files from %TEMP%, Windows\\Temp, and prefetch folders to free disk space.",
            Tags = ["powershell", "cleanup", "disk", "maintenance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item -Path \"$env:WINDIR\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item -Path \"$env:WINDIR\\Prefetch\\*\" -Recurse -Force -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { }, // No undo for cleanup
            DetectAction = () =>
            {
                // Check if temp folder has minimal content
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ChildItem -Path $env:TEMP -ErrorAction SilentlyContinue | Measure-Object).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count < 10;
            },
        },
        new TweakDef
        {
            Id = "ps-enable-dev-mode",
            Label = "Enable Developer Mode",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables Windows Developer Mode for sideloading apps and using developer features.",
            Tags = ["powershell", "developer", "sideload"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    1
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    0
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "ps-flush-dns-cache",
            Label = "Flush DNS Cache",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Clears the local DNS resolver cache. Useful after changing DNS servers or troubleshooting resolution issues.",
            Tags = ["powershell", "dns", "network", "troubleshooting"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Clear-DnsClientCache"),
            RemoveAction = _ => { }, // No undo for cache flush
            DetectAction = () => false, // Always shows as not applied — it's a one-shot action
        },
        new TweakDef
        {
            Id = "ps-disable-diagnostics-hub",
            Label = "Disable Diagnostics Hub Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Diagnostics Hub Standard Collector service (DiagTrack helper). Reduces telemetry overhead.",
            Tags = ["powershell", "service", "telemetry", "privacy", "performance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name diagnosticshub.standardcollector.service -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name diagnosticshub.standardcollector.service -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-Service -Name diagnosticshub.standardcollector.service -StartupType Manual -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-Service -Name diagnosticshub.standardcollector.service -ErrorAction SilentlyContinue).StartType"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-wmp-network-sharing",
            Label = "Disable WMP Network Sharing Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables Windows Media Player Network Sharing service. Reduces network exposure.",
            Tags = ["powershell", "service", "media", "network", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name WMPNetworkSvc -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name WMPNetworkSvc -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name WMPNetworkSvc -StartupType Manual -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name WMPNetworkSvc -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Geolocation service to prevent apps from tracking your physical location.",
            Tags = ["powershell", "service", "privacy", "location"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name lfsvc -Force -ErrorAction SilentlyContinue; Set-Service -Name lfsvc -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name lfsvc -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name lfsvc -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-connected-user-experience",
            Label = "Disable Connected User Experience (DiagTrack)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the DiagTrack (Connected User Experiences and Telemetry) service. Major Windows telemetry reducer.",
            Tags = ["powershell", "service", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name DiagTrack -Force -ErrorAction SilentlyContinue; Set-Service -Name DiagTrack -StartupType Disabled"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-Service -Name DiagTrack -StartupType Automatic; Start-Service -Name DiagTrack -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name DiagTrack -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-dmwappush-service",
            Label = "Disable Device Management WAP Push Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the dmwappushservice used for telemetry data collection routing.",
            Tags = ["powershell", "service", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name dmwappushservice -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name dmwappushservice -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name dmwappushservice -StartupType Automatic -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name dmwappushservice -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-optimize-network-adapter",
            Label = "Optimize Network Adapter Power Settings",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables power management on all network adapters to prevent them from sleeping and dropping connections.",
            Tags = ["powershell", "network", "power", "performance", "stability"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Disabled -WakeOnPattern Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Enabled -WakeOnPattern Enabled -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-execution-policy-restriction",
            Label = "Set PowerShell Execution Policy to RemoteSigned",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets the machine-scope execution policy to RemoteSigned, allowing local scripts to run without signed status.",
            Tags = ["powershell", "execution-policy", "scripts", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy -ExecutionPolicy Restricted -Scope LocalMachine -Force"),
            DetectAction = () =>
            {
                var (exit, stdout, _) = ShellRunner.Run(
                    "powershell",
                    ["-NoProfile", "-Command", "(Get-ExecutionPolicy -Scope LocalMachine).ToString()"]
                );
                return exit == 0 && stdout.Trim() == "RemoteSigned";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-remoting",
            Label = "Enable PowerShell Remoting",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables PowerShell Remoting (WinRM) for remote session management. Required for remote administration.",
            Tags = ["powershell", "remoting", "winrm", "remote"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Enable-PSRemoting -Force -SkipNetworkProfileCheck"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Disable-PSRemoting -Force"),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("powershell", ["-NoProfile", "-Command", "(Get-Service WinRM).Status"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "ps-disable-telemetry",
            Label = "Disable PowerShell Telemetry",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets POWERSHELL_TELEMETRY_OPTOUT=1 for the current user to opt out of PowerShell telemetry submission to Microsoft.",
            Tags = ["powershell", "telemetry", "privacy"],
            ApplyAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('POWERSHELL_TELEMETRY_OPTOUT','1','User')"),
            RemoveAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('POWERSHELL_TELEMETRY_OPTOUT',$null,'User')"),
            DetectAction = () =>
                System.Environment.GetEnvironmentVariable("POWERSHELL_TELEMETRY_OPTOUT", System.EnvironmentVariableTarget.User) == "1",
        },
        new TweakDef
        {
            Id = "ps-enable-constrained-language-mode",
            Label = "Enable PowerShell Constrained Language Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description =
                "Restricts PowerShell to Constrained Language Mode via environment variable, limiting access to arbitrary .NET types. Hardens against living-off-the-land attacks.",
            Tags = ["powershell", "security", "constrained", "hardening"],
            ApplyAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('__PSLockdownPolicy','4','Machine')"),
            RemoveAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('__PSLockdownPolicy',$null,'Machine')"),
            DetectAction = () => System.Environment.GetEnvironmentVariable("__PSLockdownPolicy", System.EnvironmentVariableTarget.Machine) == "4",
        },
        new TweakDef
        {
            Id = "ps-set-transcript-logging",
            Label = "Disable PowerShell Transcription Logging",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables PowerShell transcript logging which records all session input/output to disk. Reduces privacy exposure.",
            Tags = ["powershell", "transcription", "logging", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription' -Name 'EnableTranscripting' -Value 0 -Type DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription' -Name 'EnableTranscripting' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-enable-protected-event-logging",
            Label = "Enable Protected Event Logging",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Protected Event Logging (PEL) which encrypts event log content using a certificate. Prevents credential exposure in logs.",
            Tags = ["powershell", "event-log", "security", "encryption"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\EventLog\\ProtectedEventLogging' -Name 'EnableProtectedEventLogging' -Value 1 -Type DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\EventLog\\ProtectedEventLogging' -Name 'EnableProtectedEventLogging' -Value 0 -Type DWord -Force"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-clipboard-history-via-ps",
            Label = "Disable Clipboard History via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables Win+V clipboard history via group policy registry key, preventing clipboard contents from being saved.",
            Tags = ["powershell", "clipboard", "privacy", "policy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "New-Item -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\System' -Force | New-ItemProperty -Name 'AllowClipboardHistory' -Value 0 -PropertyType DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\System' -Name 'AllowClipboardHistory' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-optimize-page-file",
            Label = "Set Page File to System-Managed",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the page file to be automatically managed by Windows for optimal memory performance.",
            Tags = ["powershell", "pagefile", "memory", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("$cs = Get-WmiObject Win32_ComputerSystem; $cs.AutomaticManagedPagefile = $true; $cs.Put()"),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell("$cs = Get-WmiObject Win32_ComputerSystem; $cs.AutomaticManagedPagefile = $false; $cs.Put()"),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-enable-tls12",
            Label = "Enable TLS 1.2 for .NET Applications",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the .NET Framework 4.x to use TLS 1.2 by default for all outgoing HTTPS connections.",
            Tags = ["powershell", "tls", "security", "network", "dotnet"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "@('HKLM:\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319','HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\.NETFramework\\v4.0.30319') | "
                        + "ForEach-Object { New-Item -Path $_ -Force | New-ItemProperty -Name 'SystemDefaultTlsVersions' -Value 1 -PropertyType DWord -Force | Out-Null; "
                        + "New-ItemProperty -Path $_ -Name 'SchUseStrongCrypto' -Value 1 -PropertyType DWord -Force | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "@('HKLM:\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319','HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\.NETFramework\\v4.0.30319') | "
                        + "ForEach-Object { Remove-ItemProperty -Path $_ -Name 'SystemDefaultTlsVersions','SchUseStrongCrypto' -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-powershell-v2-engine",
            Label = "Disable PowerShell v2 Engine (Attack Surface Reduction)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Removes the MicrosoftWindowsPowerShellV2Root optional feature. PowerShell v2 lacks logging, constrained language mode, and ScriptBlock logging — keeping it installed exposes a logging bypass attack vector.",
            Tags = ["powershell", "security", "v2", "dism"],
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:MicrosoftWindowsPowerShellV2Root", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:MicrosoftWindowsPowerShellV2Root", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:MicrosoftWindowsPowerShellV2Root"]);
                return stdout.Contains("State : Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-windows-sandbox",
            Label = "Enable Windows Sandbox (Disposable Isolated Environment)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            MinBuild = 18305,
            Description =
                "Enables the Containers-DisposableClientVM optional feature. Provides a lightweight, disposable Windows environment for executing untrusted software safely — no separate licence required.",
            Tags = ["powershell", "sandbox", "isolation", "security"],
            SideEffects = "Requires Hyper-V. Requires a reboot.",
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers-DisposableClientVM", "/All", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers-DisposableClientVM", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers-DisposableClientVM"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-controlled-folder-access",
            Label = "Enable Controlled Folder Access (Ransomware Protection)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Windows Defender Controlled Folder Access via Set-MpPreference. Blocks unauthorised apps from writing to protected user folders (Documents, Desktop, Pictures), providing ransomware protection.",
            Tags = ["powershell", "defender", "ransomware", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableControlledFolderAccess Enabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableControlledFolderAccess Disabled"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).EnableControlledFolderAccess");
                return stdout.Trim() == "1";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-network-protection",
            Label = "Enable Windows Defender Network Protection",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Defender Network Protection via Set-MpPreference. Blocks connections to known malicious IPs, domains, and URLs using the SmartScreen cloud reputation service.",
            Tags = ["powershell", "defender", "network", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableNetworkProtection Enabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableNetworkProtection Disabled"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).EnableNetworkProtection");
                return stdout.Trim() == "1";
            },
        },
        new TweakDef
        {
            Id = "ps-set-defender-scan-cpu-limit",
            Label = "Limit Defender Scans to 50% CPU Average",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets ScanAvgCPULoadFactor=50 via Set-MpPreference. Caps Windows Defender background scan CPU usage at 50%, reducing performance impact on developer workloads during scheduled scans.",
            Tags = ["powershell", "defender", "cpu", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -ScanAvgCPULoadFactor 50"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -ScanAvgCPULoadFactor 80"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).ScanAvgCPULoadFactor");
                return stdout.Trim() == "50";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-signing-server",
            Label = "Require SMB Signing on This Server (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets RequireSecuritySignature=$true via Set-SmbServerConfiguration. Mandates cryptographic signing on all SMB server sessions, preventing man-in-the-middle relay attacks.",
            Tags = ["powershell", "smb", "signing", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -RequireSecuritySignature $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -RequireSecuritySignature $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).RequireSecuritySignature");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-signing-client",
            Label = "Require SMB Signing on This Client (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets RequireSecuritySignature=$true via Set-SmbClientConfiguration. Enforces signing on all outbound SMB connections from this machine, blocking NTLM relay attacks.",
            Tags = ["powershell", "smb", "signing", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -RequireSecuritySignature $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -RequireSecuritySignature $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbClientConfiguration).RequireSecuritySignature");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-smb-guest-fallback",
            Label = "Disable SMB Insecure Guest Logon Fallback",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets EnableInsecureGuestLogons=$false via Set-SmbClientConfiguration. Prevents Windows from falling back to an unauthenticated guest SMB session when credential negotiation fails.",
            Tags = ["powershell", "smb", "guest", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -EnableInsecureGuestLogons $false -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -EnableInsecureGuestLogons $true -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbClientConfiguration).EnableInsecureGuestLogons");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-encryption-server",
            Label = "Enable SMB Encryption on This Server (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets EncryptData=$true via Set-SmbServerConfiguration. Encrypts all SMB3 data in transit on this server, protecting file shares on untrusted networks.",
            Tags = ["powershell", "smb", "encryption", "security"],
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
            Id = "ps-disable-teredo",
            Label = "Disable Teredo IPv6 Tunnelling",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables Teredo via netsh. Teredo is an IPv6-over-UDP-IPv4 tunnelling protocol that can be exploited to bypass firewall rules restricting IPv6 traffic.",
            Tags = ["powershell", "ipv6", "teredo", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["interface", "teredo", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["interface", "teredo", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["interface", "teredo", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-6to4",
            Label = "Disable 6to4 IPv6 Transition Protocol",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables the 6to4 transition mechanism via netsh. 6to4 encapsulates IPv6 packets within IPv4 and can create unexpected outbound routing paths when native IPv6 is absent.",
            Tags = ["powershell", "ipv6", "6to4", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["int", "6to4", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["int", "6to4", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["int", "6to4", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-isatap",
            Label = "Disable ISATAP IPv6 Transition Interface",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables the Intra-Site Automatic Tunnel Addressing Protocol (ISATAP) via netsh. ISATAP is an IPv6-in-IPv4 tunnelling mechanism that creates hidden IPv6 connectivity channels.",
            Tags = ["powershell", "ipv6", "isatap", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["interface", "isatap", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["interface", "isatap", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["interface", "isatap", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-defender-realtime",
            Label = "Ensure Windows Defender Realtime Protection is On",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets DisableRealtimeMonitoring=$false via Set-MpPreference. Confirms that Defender real-time scanning is active — useful as a remediation step when Group Policy or another tool has disabled it.",
            Tags = ["powershell", "defender", "realtime", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -DisableRealtimeMonitoring $false"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -DisableRealtimeMonitoring $true"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).DisableRealtimeMonitoring");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}

// ── Merged from CommandLineTweaks.cs ──────────────────────────────────────────────────

internal static class CommandLineTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── bcdedit tweaks ──────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-hyper-v-hypervisor",
            Label = "Disable Hyper-V Hypervisor (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Uses bcdedit to set hypervisorlaunchtype off. Reduces overhead for non-Hyper-V workloads. Requires reboot.",
            Tags = ["bcdedit", "hypervisor", "performance", "gaming"],
            SideEffects = "Disables Hyper-V, WSL 2, Windows Sandbox, and Credential Guard.",
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "hypervisorlaunchtype", "off"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "hypervisorlaunchtype", "auto"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype    Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-boot-log",
            Label = "Enable Boot Log (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables boot logging to %SystemRoot%\\ntbtlog.txt for troubleshooting driver load order.",
            Tags = ["bcdedit", "boot", "diagnostics"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "{current}", "bootlog", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "{current}", "bootlog", "no"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("bootlog                 Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-increase-tscsyncpolicy",
            Label = "Set TSC Sync Policy to Enhanced (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets TSC synchronisation policy to Enhanced for more accurate timers in gaming and real-time workloads.",
            Tags = ["bcdedit", "performance", "gaming", "timer"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "tscsyncpolicy", "enhanced"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "tscsyncpolicy"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("tscsyncpolicy           Enhanced", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-dynamic-tick",
            Label = "Disable Dynamic Tick (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables dynamic tick to ensure consistent timer resolution. Beneficial for low-latency audio/gaming.",
            Tags = ["bcdedit", "performance", "gaming", "latency"],
            SideEffects = "May slightly increase power consumption.",
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "disabledynamictick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "disabledynamictick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("disabledynamictick      Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-set-platform-tick-high",
            Label = "Force Platform Clock to High Resolution (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Forces the platform clock to use the highest resolution available. Reduces timer jitter.",
            Tags = ["bcdedit", "performance", "latency", "timer"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "useplatformtick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "useplatformtick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("useplatformtick         Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── netsh tweaks ────────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables the NetBIOS name resolution protocol via Windows Firewall inbound rule. Reduces attack surface.",
            Tags = ["netsh", "security", "network"],
            ApplyAction = _ =>
                ShellRunner.Run(
                    "netsh.exe",
                    ["advfirewall", "firewall", "add", "rule", "name=Block NetBIOS", "dir=in", "action=block", "protocol=TCP", "localport=137-139"]
                ),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["advfirewall", "firewall", "delete", "rule", "name=Block NetBIOS"]),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("netsh.exe", ["advfirewall", "firewall", "show", "rule", "name=Block NetBIOS"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-tcp-autotuning",
            Label = "Set TCP Auto-Tuning to Normal (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets TCP receive window auto-tuning level to normal for maximum throughput.",
            Tags = ["netsh", "network", "performance", "tcp"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "autotuninglevel=normal"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "autotuninglevel=default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Receive Window Auto-Tuning Level", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("normal", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-rss",
            Label = "Enable Receive Side Scaling (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables RSS to distribute network processing across multiple CPU cores.",
            Tags = ["netsh", "network", "performance", "rss"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "rss=enabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "rss=disabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Receive-Side Scaling State", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-tcp-timestamps",
            Label = "Disable TCP Timestamps (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables TCP timestamps to reduce packet overhead and prevent OS fingerprinting.",
            Tags = ["netsh", "security", "network", "privacy"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "timestamps=disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "timestamps=enabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Timestamps", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-ecn",
            Label = "Enable ECN Capability (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables Explicit Congestion Notification for better network congestion handling.",
            Tags = ["netsh", "network", "performance"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "ecncapability=enabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "ecncapability=disabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("ECN Capability", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── powercfg tweaks ─────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-set-ultimate-perf-plan",
            Label = "Activate Ultimate Performance Power Plan (powercfg)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Unhides and activates the Ultimate Performance power plan for maximum CPU/GPU performance.",
            Tags = ["powercfg", "power", "performance", "gaming"],
            ApplyAction = _ =>
            {
                // Enable the hidden plan, then set it active
                ShellRunner.Run("powercfg.exe", ["/duplicatescheme", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
                ShellRunner.Run("powercfg.exe", ["/setactive", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
            },
            RemoveAction = _ =>
            {
                // Switch back to Balanced
                ShellRunner.Run("powercfg.exe", ["/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("powercfg.exe", ["/getactivescheme"]);
                return stdout.Contains("e9a42b02-d5df-448d-aa00-03f14749eb61", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-usb-selective-suspend",
            Label = "Disable USB Selective Suspend (powercfg)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables USB selective suspend to prevent USB devices from disconnecting during idle.",
            Tags = ["powercfg", "usb", "power", "stability"],
            ApplyAction = _ =>
            {
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setacvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "0"]
                );
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setdcvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "0"]
                );
                ShellRunner.Run("powercfg.exe", ["/setactive", "SCHEME_CURRENT"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setacvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "1"]
                );
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setdcvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "1"]
                );
                ShellRunner.Run("powercfg.exe", ["/setactive", "SCHEME_CURRENT"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(powercfg /query SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226) -match '0x00000000'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── DISM tweaks ─────────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-ie-feature",
            Label = "Disable Internet Explorer (DISM)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables the Internet Explorer optional feature via DISM. Reduces attack surface.",
            Tags = ["dism", "security", "ie", "legacy"],
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Internet-Explorer-Optional-amd64", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Internet-Explorer-Optional-amd64", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Internet-Explorer-Optional-amd64"]);
                return stdout.Contains("State : Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-sandbox",
            Label = "Enable Windows Sandbox (DISM)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the Windows Sandbox feature for isolated testing environments. Requires Hyper-V support.",
            Tags = ["dism", "security", "sandbox", "virtualization"],
            SideEffects = "Requires reboot after enabling.",
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers-DisposableClientVM", "/All", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers-DisposableClientVM", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers-DisposableClientVM"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-net35",
            Label = "Enable .NET Framework 3.5",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the .NET Framework 3.5 feature (includes .NET 2.0 and 3.0) for legacy application support.",
            Tags = ["dism", "dotnet", "framework", "legacy"],
            SideEffects = "Downloads components from Windows Update if not cached.",
            ApplyAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:NetFx3", "/All", "/NoRestart"]),
            RemoveAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:NetFx3", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:NetFx3"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-ipv6-tunnel-adapters",
            Label = "Disable IPv6 Tunnel Adapters (6to4, ISATAP, Teredo)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables IPv6 transition technologies (6to4, ISATAP, Teredo) to reduce attack surface.",
            Tags = ["netsh", "ipv6", "security", "network"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("netsh.exe", ["interface", "6to4", "set", "state", "disabled"]);
                ShellRunner.Run("netsh.exe", ["interface", "isatap", "set", "state", "disabled"]);
                ShellRunner.Run("netsh.exe", ["interface", "teredo", "set", "state", "disabled"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("netsh.exe", ["interface", "6to4", "set", "state", "default"]);
                ShellRunner.Run("netsh.exe", ["interface", "isatap", "set", "state", "default"]);
                ShellRunner.Run("netsh.exe", ["interface", "teredo", "set", "state", "default"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cmd-enable-ntp-high-freq",
            Label = "Set NTP Polling to High Frequency",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures the Windows Time service to poll NTP servers more frequently (every 256s instead of 3600s).",
            Tags = ["time", "ntp", "synchronisation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 6),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MaxPollInterval", 8),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 10),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MaxPollInterval", 15),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 6)],
        },
        new TweakDef
        {
            Id = "cmd-set-multi-plane-overlay",
            Label = "Enable Multi-Plane Overlay (MPO)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures Multi-Plane Overlay is enabled for GPU composition offloading, reducing CPU usage.",
            Tags = ["gpu", "display", "performance", "mpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
        },
        new TweakDef
        {
            Id = "cmd-disable-game-dvr-background",
            Label = "Disable Background Game Recording (Game DVR)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Xbox Game DVR background recording to free up GPU and disk resources.",
            Tags = ["gaming", "game-dvr", "xbox", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 1),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
    ];
}

