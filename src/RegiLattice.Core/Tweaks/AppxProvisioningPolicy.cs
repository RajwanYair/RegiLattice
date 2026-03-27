// AppxProvisioningPolicy.cs — APPX / sideloading / store provisioning Group Policy
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Appx
// Slug: appxprov
// Category: AppX Provisioning Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppxProvisioningPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appxprov-block-sideload",
            Label = "AppX Provisioning Policy: Block Sideloaded App Installation",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents users from installing APPX packages that do not come from the Microsoft Store or a corporate store. "
                + "Sideloading enables installation of unvalidated applications that may contain malware or policy-violating functionality. "
                + "Blocking sideloading is a key control in CIS Level 1 hardening and enterprise security baselines. "
                + "Removing this policy permits sideloaded APPX installations (requires developer mode or allowlist).",
            Tags = ["appx", "sideload", "security", "hardening", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAllTrustedApps", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAllTrustedApps", 0)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks sideloading of untrusted APPX; key security control preventing unvalidated app installation.",
        },
        new TweakDef
        {
            Id = "appxprov-block-developer-mode",
            Label = "AppX Provisioning Policy: Block Developer Mode APPX Install",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents enabling Developer Mode to allow unrestricted APPX sideloading on the device. "
                + "Developer Mode bypasses signature validation and store review, making it a significant security risk on production machines. "
                + "This policy prevents even local administrators from enabling Developer Mode without a corresponding GPO exemption. "
                + "Removing this policy allows administrators to enable Developer Mode.",
            Tags = ["appx", "developer-mode", "security", "hardening", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDevelopmentWithoutDevLicense", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDevelopmentWithoutDevLicense")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDevelopmentWithoutDevLicense", 0)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks Developer Mode activation; prevents bypass of APPX signature validation.",
        },
        new TweakDef
        {
            Id = "appxprov-require-private-store",
            Label = "AppX Provisioning Policy: Require Private Corporate Store Only",
            Category = "AppX Provisioning Policy",
            Description =
                "Restricts the Microsoft Store app to only display and deliver apps from the organisation's private corporate store. "
                + "This prevents employees from browsing and installing consumer apps via the public Microsoft Store on managed devices. "
                + "Only IT-approved apps provisioned in the corporate store are visible and installable. "
                + "Removing this policy allows access to the full public Microsoft Store.",
            Tags = ["appx", "private-store", "enterprise", "microsoft-store", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePrivateStoreOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePrivateStoreOnly")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePrivateStoreOnly", 1)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Restricts Store to corporate apps only; prevents consumer app installation on managed devices.",
        },
        new TweakDef
        {
            Id = "appxprov-disable-store-auto-update",
            Label = "AppX Provisioning Policy: Disable Store App Auto-Updates",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents the Microsoft Store from automatically updating installed applications in the background. "
                + "Uncontrolled auto-updates can introduce incompatible application versions or consume bandwidth during business hours. "
                + "IT should control app versioning through the corporate store with tested, approved versions. "
                + "Removing this policy re-enables Microsoft Store automatic app updates.",
            Tags = ["appx", "auto-update", "microsoft-store", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStoreAppsAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreAppsAutoUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStoreAppsAutoUpdate", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Stops Store from silently updating apps; IT controls app versioning centrally.",
        },
        new TweakDef
        {
            Id = "appxprov-disable-shared-local-appdata",
            Label = "AppX Provisioning Policy: Disable Shared Local AppData for UWP",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents Universal Windows Platform apps from accessing shared local AppData folders that may contain sensitive data from other user sessions. "
                + "Sandboxed UWP apps with shared AppData access risk cross-session data leakage in shared or kiosk environments. "
                + "Removing this policy permits UWP apps to use shared local AppData.",
            Tags = ["appx", "appdata", "sandbox", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowSharedLocalAppData", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSharedLocalAppData")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSharedLocalAppData", 0)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Restricts UWP shared AppData access; prevents cross-session data exposure in shared environments.",
        },
        new TweakDef
        {
            Id = "appxprov-block-consumer-provision",
            Label = "AppX Provisioning Policy: Block Consumer Experience App Provisioning",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents Windows from silently provisioning consumer apps (games, entertainment apps) for new user accounts during first logon. "
                + "Windows periodically pushes consumer APPX packages to endpoints over the air without explicit user action. "
                + "Blocking provisioning keeps the device image clean and reduces surprise network downloads during initial login. "
                + "Removing this policy allows Windows to provision consumer apps for new users.",
            Tags = ["appx", "consumer", "provisioning", "bloat", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableConsumerAccountStateContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableConsumerAccountStateContent")],
            DetectOps = [RegOp.CheckDword(Key, "DisableConsumerAccountStateContent", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks silent consumer app provisioning at logon; maintains a clean enterprise app baseline.",
        },
        new TweakDef
        {
            Id = "appxprov-block-game-dvr-appstore",
            Label = "AppX Provisioning Policy: Block Xbox App Store Integration",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents the Xbox App and Xbox integrated Store components from being provisioned or auto-installed for managed accounts. "
                + "Xbox integration introduces gaming-related network traffic, Xbox Live account prompts, and gaming telemetry on enterprise machines. "
                + "Removing this policy allows Xbox App Store components to be provisioned for user accounts.",
            Tags = ["appx", "xbox", "gaming", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNonAdminUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNonAdminUserInstall")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNonAdminUserInstall", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks non-admin APPX installs including Xbox components; reduces gaming footprint on enterprise endpoints.",
        },
        new TweakDef
        {
            Id = "appxprov-disable-appx-deployment-service",
            Label = "AppX Provisioning Policy: Restrict APPX Deployment to Admin Only",
            Category = "AppX Provisioning Policy",
            Description =
                "Restricts APPX package deployment operations to administrator accounts only, preventing standard users from installing APPX packages. "
                + "Standard user-initiated APPX installs bypass traditional software management tools and can install unauthorised applications. "
                + "Admin-only restriction ensures all UWP app deployment is tracked and authorised. "
                + "Removing this policy allows standard users to install APPX packages.",
            Tags = ["appx", "deployment", "standard-user", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictAppToSystemVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictAppToSystemVolume")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictAppToSystemVolume", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Restricts APPX to system volume only; prevents user-initiated installs to removable media.",
        },
        new TweakDef
        {
            Id = "appxprov-disable-appinstaller",
            Label = "AppX Provisioning Policy: Disable App Installer Protocol Handler",
            Category = "AppX Provisioning Policy",
            Description =
                "Disables the ms-appinstaller:// URI protocol handler that allows websites to trigger APPX installations directly in a browser. "
                + "This protocol handler has been exploited in supply chain attacks where malicious links trigger silent APPX payload delivery. "
                + "Microsoft issued a security advisory (ADV220001) recommending disabling this handler in enterprise environments. "
                + "Removing this policy re-enables the App Installer protocol handler.",
            Tags = ["appx", "app-installer", "protocol", "security", "supply-chain"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMSAppInstallerProtocol", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMSAppInstallerProtocol")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMSAppInstallerProtocol", 0)],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Blocks ms-appinstaller:// protocol; mitigates supply chain APPX delivery attacks (ADV220001 advisory).",
        },
        new TweakDef
        {
            Id = "appxprov-disable-packaged-com",
            Label = "AppX Provisioning Policy: Disable Packaged COM Activation Bypass",
            Category = "AppX Provisioning Policy",
            Description =
                "Prevents APPX-packaged COM servers from activating out-of-process components that bypass standard COM registration security. "
                + "Packaged COM can be used to load protected app components in unprotected contexts, weakening the UWP security sandbox model. "
                + "Removing this policy allows packaged COM activation in UWP applications.",
            Tags = ["appx", "com", "activation", "sandbox", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowPackagedCom", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowPackagedCom")],
            DetectOps = [RegOp.CheckDword(Key, "AllowPackagedCom", 0)],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Blocks packaged COM activation bypass; tightens UWP sandbox isolation. May break some packaged apps.",
        },
    ];
}
