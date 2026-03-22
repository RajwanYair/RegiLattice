// RegiLattice.Core — Tweaks/AppxPolicy.cs
// UWP / AppX package deployment, sideloading, and Store policy settings.
// Slug: "appx" — HKLM\SOFTWARE\Policies\Microsoft\Windows\Appx GPO keys.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppxPolicy
{
    private const string AppxPolicy2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";

    private const string MsStorePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";

    private const string ExplorerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    private const string InstallerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appx-block-non-admin-install",
            Label = "Block Non-Admin UWP App Installation",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["appx", "uwp", "install", "policy", "admin"],
            Description =
                "Restricts UWP/AppX app installation to administrator accounts only. "
                + "BlockNonAdminUserInstall=1 prevents standard users from installing apps "
                + "from the Store or via sideloading. Useful on managed/shared PCs.",
            ApplyOps = [RegOp.SetDword(AppxPolicy2, "BlockNonAdminUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(AppxPolicy2, "BlockNonAdminUserInstall")],
            DetectOps = [RegOp.CheckDword(AppxPolicy2, "BlockNonAdminUserInstall", 1)],
        },
        new TweakDef
        {
            Id = "appx-disable-sideloading",
            Label = "Disable AppX Sideloading (Unsigned Packages)",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["appx", "uwp", "sideloading", "policy", "security"],
            Description =
                "Disables sideloading of untrusted AppX packages. AllowAllTrustedApps=0 "
                + "restricts installation to apps from the Microsoft Store or packages signed "
                + "with a trusted enterprise certificate.",
            ApplyOps = [RegOp.SetDword(AppxPolicy2, "AllowAllTrustedApps", 0)],
            RemoveOps = [RegOp.DeleteValue(AppxPolicy2, "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(AppxPolicy2, "AllowAllTrustedApps", 0)],
        },
        new TweakDef
        {
            Id = "appx-disable-dev-mode-sideload",
            Label = "Disable Developer Mode AppX Sideloading",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["appx", "developer mode", "sideloading", "policy"],
            Description =
                "Prevents AppX packages from being sideloaded in Developer Mode (without a "
                + "developer license). AllowDevelopmentWithoutDevLicense=0. Ensures only "
                + "signed packages from the Store can be installed.",
            ApplyOps = [RegOp.SetDword(AppxPolicy2, "AllowDevelopmentWithoutDevLicense", 0)],
            RemoveOps = [RegOp.DeleteValue(AppxPolicy2, "AllowDevelopmentWithoutDevLicense")],
            DetectOps = [RegOp.CheckDword(AppxPolicy2, "AllowDevelopmentWithoutDevLicense", 0)],
        },
        new TweakDef
        {
            Id = "appx-restrict-deployment-to-system-volume",
            Label = "Restrict AppX Deployment to System Volume Only",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["appx", "deployment", "volume", "drive", "policy"],
            Description =
                "Prevents UWP apps from being deployed to secondary drives (D:, E:, etc.). "
                + "DisableDeploymentToNonSystemVolumes=1 ensures all AppX packages are "
                + "installed only on the system drive, simplifying management and imaging.",
            ApplyOps = [RegOp.SetDword(AppxPolicy2, "DisableDeploymentToNonSystemVolumes", 1)],
            RemoveOps = [RegOp.DeleteValue(AppxPolicy2, "DisableDeploymentToNonSystemVolumes")],
            DetectOps = [RegOp.CheckDword(AppxPolicy2, "DisableDeploymentToNonSystemVolumes", 1)],
        },
        new TweakDef
        {
            Id = "appx-block-store-app-installs",
            Label = "Block Microsoft Store App Purchases and Installs",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["appx", "microsoft store", "install", "policy", "enterprise"],
            Description =
                "Prevents users from purchasing or installing new apps via the Microsoft Store. "
                + "RemoveWindowsStore=1. The Store app is still accessible for updates but "
                + "installs are blocked. Standard enterprise lockdown for unmanaged app control.",
            ApplyOps = [RegOp.SetDword(MsStorePolicy, "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(MsStorePolicy, "RemoveWindowsStore")],
            DetectOps = [RegOp.CheckDword(MsStorePolicy, "RemoveWindowsStore", 1)],
        },
        new TweakDef
        {
            Id = "appx-disable-store-auto-update",
            Label = "Disable Automatic App Updates from Microsoft Store",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["appx", "microsoft store", "auto update", "policy"],
            Description =
                "Prevents the Microsoft Store from automatically updating apps in the background. "
                + "AutoDownload=2 (disabled). Useful on bandwidth-constrained networks or when "
                + "app updates must be validated before deployment.",
            ApplyOps = [RegOp.SetDword(MsStorePolicy, "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(MsStorePolicy, "AutoDownload")],
            DetectOps = [RegOp.CheckDword(MsStorePolicy, "AutoDownload", 2)],
        },
        new TweakDef
        {
            Id = "appx-suppress-new-app-notification",
            Label = "Suppress 'New App Installed' Notification in Explorer",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["appx", "notification", "explorer", "new app"],
            Description =
                "Disables the 'You have new apps available' balloon notification that appears "
                + "in Windows Explorer when a new app can open a file type. NoNewAppAlert=1 "
                + "in the Explorer policy keeps the UI uncluttered.",
            ApplyOps = [RegOp.SetDword(ExplorerPolicy, "NoNewAppAlert", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPolicy, "NoNewAppAlert")],
            DetectOps = [RegOp.CheckDword(ExplorerPolicy, "NoNewAppAlert", 1)],
        },
        new TweakDef
        {
            Id = "appx-block-elevated-msi-install",
            Label = "Block Always-Install-Elevated MSI Packages",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["msi", "installer", "elevated", "privilege escalation", "security"],
            Description =
                "Disables the 'always install with elevated privileges' Windows Installer policy. "
                + "AlwaysInstallElevated=0 prevents privilege escalation via crafted MSI packages. "
                + "This setting must be 0 in BOTH HKCU and HKLM to be effective.",
            ApplyOps = [RegOp.SetDword(InstallerPolicy, "AlwaysInstallElevated", 0)],
            RemoveOps = [RegOp.DeleteValue(InstallerPolicy, "AlwaysInstallElevated")],
            DetectOps = [RegOp.CheckDword(InstallerPolicy, "AlwaysInstallElevated", 0)],
        },
        new TweakDef
        {
            Id = "appx-block-user-elevated-msi",
            Label = "Block User-Level Always-Elevated MSI Install",
            Category = "AppX Policy",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["msi", "installer", "elevated", "privilege escalation", "security"],
            Description =
                "Disables 'always install with elevated privileges' from the user-scope Installer "
                + "policy (HKCU). Must be combined with the machine-scope setting appx-block-elevated-msi-install. "
                + "Both keys must be 0 to prevent privilege escalation via malicious MSI packages.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Installer", "AlwaysInstallElevated", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Installer", "AlwaysInstallElevated")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Installer", "AlwaysInstallElevated", 0)],
        },
        new TweakDef
        {
            Id = "appx-disable-shared-local-app-data",
            Label = "Disable Shared LocalAppData Between Users (AppX)",
            Category = "AppX Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["appx", "uwp", "shared data", "privacy", "multi-user"],
            Description =
                "Prevents UWP apps from sharing a common local app data folder between "
                + "multiple users on the same device. AllowSharedLocalAppData=0. "
                + "Ensures each user's app data remains isolated from other accounts.",
            ApplyOps = [RegOp.SetDword(AppxPolicy2, "AllowSharedLocalAppData", 0)],
            RemoveOps = [RegOp.DeleteValue(AppxPolicy2, "AllowSharedLocalAppData")],
            DetectOps = [RegOp.CheckDword(AppxPolicy2, "AllowSharedLocalAppData", 0)],
        },
    ];
}
