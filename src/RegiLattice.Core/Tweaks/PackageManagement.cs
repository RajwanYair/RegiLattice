namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PackageManagement
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pkg-ps-remotesigned",
            Label = "PowerShell RemoteSigned Policy",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Sets PowerShell execution policy to RemoteSigned for the current user, enabling local scripts.",
            Tags = ["powershell", "security", "scripting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"],
        },
        new TweakDef
        {
            Id = "pkg-ps-gallery-trust",
            Label = "Trust PSGallery Repository",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets PSGallery as a trusted repository for Install-Module.",
            Tags = ["powershell", "packages", "gallery"],
        },
        new TweakDef
        {
            Id = "pkg-scoop-setup",
            Label = "Install & Configure Scoop",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Installs Scoop package manager (if missing) and adds extras, versions, and nerd-fonts buckets.",
            Tags = ["scoop", "packages", "installer"],
        },
        new TweakDef
        {
            Id = "pkg-enable-winget",
            Label = "Enable Winget (App Installer)",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables winget (App Installer), experimental features, and hash override via Group Policy.",
            Tags = ["winget", "packages", "installer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
        },
        new TweakDef
        {
            Id = "pkg-pip-user-default",
            Label = "Pip Default --user Install",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets PIP_USER=1 environment variable so pip installs to user site-packages by default (no admin required).",
            Tags = ["python", "pip", "packages"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-no-cache",
            Label = "Pip Disable Cache (Save Disk)",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets PIP_NO_CACHE_DIR=1 to avoid storing downloaded package caches.",
            Tags = ["python", "pip", "disk"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-npm-prefer-offline",
            Label = "npm Prefer Offline Cache",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets npm to prefer local cache for faster installs.",
            Tags = ["npm", "node", "packages", "offline"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-require-venv",
            Label = "Pip Require Virtualenv",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets PIP_REQUIRE_VIRTUALENV=1 — prevents accidental installs into system/global Python. Forces explicit virtualenv usage.",
            Tags = ["python", "pip", "virtualenv", "safety"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-disable-version-check",
            Label = "Pip Disable Version Nag",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets PIP_DISABLE_PIP_VERSION_CHECK=1 to suppress 'new pip version available' warnings.",
            Tags = ["python", "pip", "nag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-timeout",
            Label = "Pip Timeout 60s (Slow Networks)",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets PIP_TIMEOUT=60 for more reliable installs on slow or corporate networks (default: 15s).",
            Tags = ["python", "pip", "network", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-trusted-host",
            Label = "Pip Trusted Hosts (PyPI)",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds pypi.org and pythonhosted.org as trusted hosts so pip works behind corporate TLS-inspecting proxies.",
            Tags = ["python", "pip", "proxy", "corporate"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-index",
            Label = "Pip System Index URL (HKLM)",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets system-wide PIP_INDEX_URL to official PyPI for all users (HKLM environment variable).",
            Tags = ["python", "pip", "system", "index"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-no-cache",
            Label = "Pip System Disable Cache (HKLM)",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets system-wide PIP_NO_CACHE_DIR=1 to prevent pip cache accumulation for all users.",
            Tags = ["python", "pip", "system", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-trusted-host",
            Label = "Pip System Trusted Hosts (HKLM)",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets system-wide PIP_TRUSTED_HOST for all users — useful for fleet-wide corporate proxy configurations.",
            Tags = ["python", "pip", "system", "proxy", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-require-venv",
            Label = "Pip System Require Virtualenv (HKLM)",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "System-wide PIP_REQUIRE_VIRTUALENV=1 — blocks any pip install outside a virtualenv for all users on the machine.",
            Tags = ["python", "pip", "system", "virtualenv", "safety"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
        },
        new TweakDef
        {
            Id = "pkg-winget-disable-auto-update",
            Label = "Disable WinGet Auto-Upgrade",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables WinGet automatic package upgrades via Group Policy. Packages must be upgraded manually with 'winget upgrade'. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["winget", "packages", "update", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
        },
        new TweakDef
        {
            Id = "pkg-winget-disable-msstore-source",
            Label = "Disable WinGet MS Store Source",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Microsoft Store as a WinGet package source. Only the winget community repository is used. Default: Enabled. Recommended: Disabled for developer workflows.",
            Tags = ["winget", "packages", "source", "msstore"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
        },
        new TweakDef
        {
            Id = "pkg-disable-suggested-apps",
            Label = "Disable Suggested App Installations",
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing suggested apps. Default: Enabled. Recommended: Disabled.",
            Tags = ["packages", "suggested", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-winget-auto-update",
            Label = "Disable WinGet Auto-Update",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic WinGet package manager self-updates via policy. Default: Enabled. Recommended: Disabled for controlled environments.",
            Tags = ["packages", "winget", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "pkg-choco-proxy",
            Label = "Set Chocolatey System Proxy",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures Chocolatey to use the system proxy for package downloads. Useful in corporate environments behind a proxy. Default: Direct. Recommended: Enabled behind proxy.",
            Tags = ["packages", "chocolatey", "proxy", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy", 1)],
        },
        new TweakDef
        {
            Id = "pkg-source-validation",
            Label = "Enable Package Source Validation",
            Category = "Package Management",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents WinGet from overriding package hash validation. Ensures integrity checks are enforced for all package sources. Default: Override allowed. Recommended: Disabled (validation enforced).",
            Tags = ["packages", "winget", "hash", "validation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-ms-store",
            Label = "Disable Microsoft Store via Policy",
            Category = "Package Management",
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
            Category = "Package Management",
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
            Category = "Package Management",
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
            Category = "Package Management",
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
            Category = "Package Management",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables shared experiences (app hand-off between devices). Default: enabled.",
            Tags = ["package", "shared-experiences", "cross-device", "cdp"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
        },
    ];
}
