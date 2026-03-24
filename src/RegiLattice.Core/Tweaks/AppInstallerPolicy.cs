namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppInstallerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appins-disable-app-installer",
            Label = "Disable Windows Package Manager (winget)",
            Category = "App Installer Policy",
            Description =
                "Disables the Windows Package Manager (winget / AppInstaller) entirely via machine-wide policy. Users cannot install or manage packages. Default: enabled.",
            Tags = ["winget", "app-installer", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks all winget package operations; deploy only in locked-down environments.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableAppInstaller", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAppInstaller")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAppInstaller", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-settings",
            Label = "Disable WinGet Settings Modification",
            Category = "App Installer Policy",
            Description =
                "Prevents users from modifying Windows Package Manager settings via 'winget settings'. Configuration remains at machine defaults.",
            Tags = ["winget", "app-installer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents user modification of winget configuration settings.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSettings")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSettings", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-experimental-features",
            Label = "Disable WinGet Experimental Features",
            Category = "App Installer Policy",
            Description =
                "Blocks use of experimental (preview) features in Windows Package Manager. Ensures only stable, supported behaviour is used.",
            Tags = ["winget", "app-installer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks unstable experimental winget features from running.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableExperimentalFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableExperimentalFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "EnableExperimentalFeatures", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-local-manifests",
            Label = "Require Repository Manifests Only",
            Category = "App Installer Policy",
            Description =
                "Prevents installing packages from local manifest files. All installs must originate from an approved repository source. Reduces risk of unapproved package installs.",
            Tags = ["winget", "app-installer", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents unauthorized local package installs via custom manifests.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableLocalManifestFiles", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableLocalManifestFiles")],
            DetectOps = [RegOp.CheckDword(Key, "EnableLocalManifestFiles", 0)],
        },
        new TweakDef
        {
            Id = "appins-require-hash-validation",
            Label = "Require Installer Hash Validation",
            Category = "App Installer Policy",
            Description =
                "Disables the ability to bypass SHA-256 hash validation for package installers. Packages with unknown or unverified hashes are blocked.",
            Tags = ["winget", "app-installer", "security", "integrity"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enforces SHA-256 hash integrity verification for all package installs.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableHashOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableHashOverride")],
            DetectOps = [RegOp.CheckDword(Key, "EnableHashOverride", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-ms-installer-protocol",
            Label = "Block ms-appinstaller: URI Protocol",
            Category = "App Installer Policy",
            Description =
                "Disables the ms-appinstaller: URI protocol handler, which has been exploited in CVE-based campaigns to directly launch package installs from web links.",
            Tags = ["winget", "app-installer", "security", "cve", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Closes the ms-appinstaller URI exploit vector for web-triggered package installs.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMSAppInstallerProtocol", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMSAppInstallerProtocol")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMSAppInstallerProtocol", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-additional-sources",
            Label = "Block Addition of Custom Package Sources",
            Category = "App Installer Policy",
            Description =
                "Prevents users from adding custom (non-approved) package sources to Windows Package Manager. All source management requires admin approval.",
            Tags = ["winget", "app-installer", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Limits package installs to admin-approved sources only.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableAdditionalSources", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAdditionalSources")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAdditionalSources", 0)],
        },
        new TweakDef
        {
            Id = "appins-restrict-to-allowed-sources",
            Label = "Restrict Installs to Allowed Sources Only",
            Category = "App Installer Policy",
            Description =
                "Enforces an allowlist of approved package sources. Any source not on the allowed list is blocked for package installation.",
            Tags = ["winget", "app-installer", "policy", "allowlist"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enforces allowlist-only installs; blocks all unauthorised sources.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableAllowedSources", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAllowedSources")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAllowedSources", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-default-source",
            Label = "Disable WinGet Default Source (winget.pkgs.com)",
            Category = "App Installer Policy",
            Description =
                "Disables the default winget community repository (winget.pkgs.com). Package installs are restricted to enterprise-approved sources.",
            Tags = ["winget", "app-installer", "policy", "source"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks the default winget community repository; may disrupt personal package installs.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDefaultSource", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDefaultSource")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDefaultSource", 0)],
        },
        new TweakDef
        {
            Id = "appins-disable-store-source",
            Label = "Disable Microsoft Store as WinGet Source",
            Category = "App Installer Policy",
            Description =
                "Removes the Microsoft Store as an available package source within Windows Package Manager. Store-sourced installs must go through the Microsoft Store application directly.",
            Tags = ["winget", "app-installer", "store", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes Microsoft Store as a winget install source.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMicrosoftStoreSource", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMicrosoftStoreSource")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMicrosoftStoreSource", 0)],
        },
    ];
}
