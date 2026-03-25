// RegiLattice.Core — Tweaks/AppXPackagingPolicy.cs
// Sprint 303: AppX Packaging Policy tweaks (10 tweaks)
// Category: "AppX Packaging Policy" | Slug: appxpkg
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppxPackaging

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppXPackagingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppxPackaging";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appxpkg-disable-sideloading",
            Label = "Disable AppX Sideloading",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "AppX sideloading allows installation of packaged applications from sources other than the Microsoft Store through locally provided package files. Disabling sideloading prevents installation of AppX packages that are not signed by a trusted certificate or distributed through the Store. Sideloaded applications bypass Microsoft Store security review and code signing certificate requirements. Malicious parties can distribute sideloaded packages that appear legitimate but contain embedded malicious payloads. Enterprise application deployment should use Intune, SCCM, or the Microsoft Store for Business rather than manual sideloading. Disabling sideloading reduces the attack surface for untrusted application introduction through package file delivery.",
            Tags = ["appx", "sideloading", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAllTrustedApps", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAllTrustedApps", 0)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-developer-mode",
            Label = "Disable Developer Mode for AppX Installation",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Developer Mode enables installation of unsigned AppX packages and disables various security checks designed for production use. Disabling Developer Mode prevents the loosened security settings it activates from being applied to managed endpoints. Developer Mode is intended for application development workflows and should not be active on production or end-user systems. Unsigned package installation enabled by Developer Mode bypasses code signing requirements designed to verify software origin. Enterprise endpoints do not require Developer Mode for any production applications and its presence represents unnecessary risk. Developers requiring Developer Mode should use dedicated development workstations separate from the managed endpoint fleet.",
            Tags = ["appx", "developer-mode", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDevelopmentWithoutDevLicense", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDevelopmentWithoutDevLicense")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDevelopmentWithoutDevLicense", 0)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-package-update",
            Label = "Disable Automatic AppX Package Updates",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Windows Store and UWP applications automatically update to new versions in the background without user intervention. Disabling automatic AppX package updates prevents new application versions from being installed without administrator review. Automatic application updates bypass change management processes and may introduce untested functionality or breaking changes. Enterprise application updates should be tested against the business environment before being deployed to production endpoints. Automatic updates may consume significant bandwidth on metered connections and during business hours. Controlled update deployment through managed channels ensures compatibility and compliance before new versions reach production.",
            Tags = ["appx", "updates", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundAutoUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-optional-components",
            Label = "Disable AppX Optional Package Installation",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "AppX optional packages allow modular additions to be installed on top of a base UWP application to extend its functionality. Disabling optional package installation prevents new optional components from being added to installed UWP applications. Optional packages can significantly expand the functionality and attack surface of base applications without being subject to the same review as the original application. Enterprise application governance should assess application capabilities including all component additions. Optional packages downloaded from the Store may introduce new code paths not reviewed as part of the original deployment approval. Disabling optional components is appropriate for locked-down environments where scope of installed applications must be fixed.",
            Tags = ["appx", "optional-packages", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOptionalPackages", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOptionalPackages")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOptionalPackages", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-shared-pkg-container",
            Label = "Disable AppX Shared Package Container",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Shared package containers allow multiple AppX applications to share a common container context, facilitating inter-application data sharing. Disabling shared package containers prevents multiple packaged applications from sharing an isolated container context. Container sharing reduces isolation between applications and allows data to be accessed across application boundaries. Applications sharing a container can influence each other's state, potentially enabling a compromised component to access another component's data. Strict application isolation requires that each AppX application operate in its own fully independent container. Disabling shared containers enforces a stronger application isolation model at the cost of potential functionality limitations in apps designed to share state.",
            Tags = ["appx", "isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSharedPackageContainer", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSharedPackageContainer")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSharedPackageContainer", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-hosted-app",
            Label = "Disable Hosted AppX Applications",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Hosted apps allow web applications to be packaged and installed as AppX applications using a trusted host process that provides the runtime environment. Disabling hosted app functionality prevents web-based content from being installed and run as managed AppX packages. Hosted apps blur the boundary between web content and native applications and may enable web-based attacks to leverage native packaging capabilities. Enterprise web application deployments should use browsers with appropriate security configurations rather than packaged hosted apps. The hosted app model can potentially bypass security controls that differentiate between native and web application execution contexts. Disabling hosted apps maintains a clear separation between web content execution and native application installation.",
            Tags = ["appx", "hosted-apps", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHostedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHostedApps")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHostedApps", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-dynamic-content",
            Label = "Disable AppX Package Dynamic Content Loading",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Dynamic content loading allows packaged applications to download and execute additional content or code modules from external sources at runtime. Disabling dynamic content loading prevents AppX applications from pulling and executing externally sourced code after deployment. Runtime dynamic content represents an uncontrolled code execution path that bypasses the AppX code signing and packaging review processes. Applications that dynamically load content can transform from approved base applications into arbitrary code execution vehicles. Enterprise application governance must include all code executed by applications, not only the initially deployed package. Disabling dynamic content loading enforces a closed application model where all executable code must be present at deployment time.",
            Tags = ["appx", "dynamic-content", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDynamicContentLoading", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDynamicContentLoading")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDynamicContentLoading", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-package-telemetry",
            Label = "Disable AppX Packaging Telemetry",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "AppX packaging telemetry collects data about installed packages, installation events, and application usage statistics. This data helps Microsoft improve the packaging and Store infrastructure as well as identify compatibility issues. Disabling packaging telemetry prevents information about installed packaged applications from being transmitted to Microsoft. Installed application lists represent sensitive security information revealing which tools and applications are available on enterprise endpoints. Application inventory telemetry from endpoints should be managed through enterprise MDM and SCCM tools rather than consumer telemetry channels. AppX application functionality continues to operate normally regardless of this telemetry setting.",
            Tags = ["appx", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePackagingTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePackagingTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePackagingTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-uap5",
            Label = "Disable UAP5 AppX Protocol Extensions",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "UAP5 protocol extensions expand the capabilities available to packaged applications through extended Windows Runtime APIs and integration points. Disabling UAP5 extensions prevents packaged applications from accessing the expanded set of Windows integration capabilities introduced in UAP version 5. Extended protocol capabilities increase the attack surface available to compromised packaged applications. Applications using UAP5 features can register deep OS integration points that persist across reboots and user sessions. Enterprise application deployments should use the minimum capability set required for functional operation. Disabling extended protocol capabilities limits the persistent integration footprint of packaged applications on managed endpoints.",
            Tags = ["appx", "protocols", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableUAP5Extensions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUAP5Extensions")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUAP5Extensions", 1)],
        },
        new TweakDef
        {
            Id = "appxpkg-disable-staged-removal",
            Label = "Disable AppX Package Staged Removal Delay",
            Category = "AppX Packaging Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "AppX package staged removal delays the deletion of package files after uninstallation to allow background cleanup operations to complete. Disabling staged removal delays causes package files to be removed immediately upon uninstallation without a deferred cleanup period. Staged removal delays mean uninstalled application data persists on disk for a period after removal, occupying storage space. Immediate removal ensures that uninstalled application data and files are promptly cleared from the endpoint filesystem. Prompt cleanup is particularly important for compliance with data retention policies requiring timely disposal of data. Disabling staged removal improves disk space reclamation speed after package uninstallation on endpoints with limited storage.",
            Tags = ["appx", "cleanup", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStagedRemoval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStagedRemoval")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStagedRemoval", 1)],
        },
    ];
}
