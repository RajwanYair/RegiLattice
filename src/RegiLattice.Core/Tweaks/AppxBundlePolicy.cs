// RegiLattice.Core — Tweaks/AppxBundlePolicy.cs
// Sprint 329: AppX Bundle Policy tweaks (10 tweaks)
// Category: "AppX Bundle Policy" | Slug: appxbnd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppxBundle

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppxBundlePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppxBundle";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appxbnd-disable-side-loading",
            Label = "Disable App Side-Loading (AppX Side-loading Policy)",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "App side-loading allows installation of unsigned or enterprise-signed AppX packages outside the Microsoft Store without full code review. Disabling side-loading prevents installation of unofficial AppX packages that have not been validated through Store submission processes. Side-loaded apps bypass the Store's malware scanning and security review processes that reduce malicious package distribution. Enterprise side-loading is a legitimate scenario requiring a specific policy setting to enable it separately from general side-loading. Malicious actors have exploited side-loading to distribute malware disguised as legitimate apps through phishing and drive-by download campaigns. Side-loading policy should be enabled only for verified enterprise packages distributed through MDM or trusted enterprise channels.",
            Tags = ["appx", "side-loading", "packages", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAllTrustedApps", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAllTrustedApps", 0)],
        },
        new TweakDef
        {
            Id = "appxbnd-restrict-app-store-to-private",
            Label = "Restrict Microsoft Store to Private Store Only",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting Microsoft Store to the private store ensures that only IT-approved applications listed in the enterprise private store can be installed. Private store restriction prevents employees from installing consumer apps that may contain spyware, consume bandwidth, or violate acceptable use policies. The Microsoft Store for Business private store allows IT to curate approved applications for enterprise deployment. Endpoints restricted to private store only show only IT-vetted apps preventing unauthorized software installation through Store channels. Private store restriction does not prevent legitimate business applications and provides a controlled software distribution channel. Organizations using Intune or Configuration Manager for app delivery can restrict the Store to ensure consistent endpoint configuration.",
            Tags = ["appx", "store", "private-store", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePrivateStoreOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePrivateStoreOnly")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePrivateStoreOnly", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-disable-automatic-updates",
            Label = "Disable Automatic AppX Package Updates",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Automatic AppX updates replace installed packages with new versions from the Store without IT approval or testing. Disabling automatic AppX updates ensures that app updates go through enterprise testing and validation before deployment to managed endpoints. Automatic updates can introduce breaking changes or new feature behaviors that conflict with enterprise customizations or configurations. Enterprise change management processes require validation of application updates before broad deployment to prevent productivity disruption. Disabling automatic updates shifts update management to IT-controlled channels such as Intune, ConfigMgr, or Windows Update for Business. Organizations should still ensure security patches for AppX apps are applied through the managed update channel promptly.",
            Tags = ["appx", "updates", "store", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStoreOriginatedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreOriginatedApps")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStoreOriginatedApps", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-require-package-signing",
            Label = "Require Digital Signature for AppX Packages",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Digital signature requirements for AppX packages ensure that only code signed by trusted publishers can be installed. Requiring package signing prevents installation of malicious or tampered AppX packages that lack valid digital signatures. Store packages are signed by Microsoft after vetting but side-loaded enterprise apps must be signed with enterprise certificates. Unsigned AppX packages are unverifiable and could contain modified or injected code without signature validation. Enterprise code signing certificates for AppX packages should be managed through the enterprise PKI with appropriate controls. Requiring signatures prevents delivery of unsigned packages through social engineering or drive-by download attacks targeting enterprise users.",
            Tags = ["appx", "signing", "certificates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePackageSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePackageSigning")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePackageSigning", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-block-consumer-apps",
            Label = "Block Consumer Microsoft Apps from Store",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Consumer Microsoft apps including Games, Entertainment, and consumer services are typically inappropriate for enterprise managed endpoints. Blocking consumer app categories from Store installation maintains enterprise focus and prevents distraction applications on corporate devices. Consumer apps may collect telemetry, use corporate bandwidth, or have privacy policies inconsistent with enterprise data handling requirements. Enterprise endpoints should be configured with purpose-specific applications rather than consumer entertainment and social apps. MDM policies can block specific app categories or specific app IDs from installation through Store policy enforcement. Blocking consumer apps is part of overall endpoint purpose-restriction which improves security posture by reducing installed surface area.",
            Tags = ["appx", "consumer-apps", "store", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockConsumerApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockConsumerApps")],
            DetectOps = [RegOp.CheckDword(Key, "BlockConsumerApps", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-disable-shared-user-app-updates",
            Label = "Disable AppX Updates for All Users Context",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "All-user context AppX updates apply package updates across all user profiles which can impact shared multi-user endpoints. Disabling shared user app updates prevents automatic updates from modifying AppX packages in the all-users context without administrator action. All-user context updates can change configurations or introduce new features affecting every user of shared corporate devices. Shared workstation AppX management should be handled through managed deployment channels rather than automatic Store updates. Controlling all-user app updates ensures consistent application state across shared endpoints in environments like call centers and lab systems. Managed update deployments allow IT to test app changes and schedule deployment to minimize disruption to shared endpoint users.",
            Tags = ["appx", "shared-apps", "updates", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSharedUserAppUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSharedUserAppUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSharedUserAppUpdates", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-enable-package-inventory",
            Label = "Enable AppX Package Installation Inventory Reporting",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "AppX package inventory reporting collects data on installed Store and sideloaded apps for compliance monitoring and software asset management. Enabling package inventory ensures that all AppX package installations are recorded and available for security and compliance reporting. App inventory data helps identify unauthorized sideloaded apps and ensure only approved applications are present on managed endpoints. Package inventory feeds into software licensing compliance tracking and endpoint configuration management systems. Security teams can use package inventory to identify potentially malicious or policy-violating apps installed by users. Periodic AppX inventory review should be part of standard endpoint compliance monitoring.",
            Tags = ["appx", "inventory", "compliance", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePackageInventory", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePackageInventory")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePackageInventory", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-disable-user-store-access",
            Label = "Disable User Access to Microsoft Store",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "User access to Microsoft Store allows employees to install any Store application without IT approval consuming bandwidth and potentially installing malicious or inappropriate apps. Disabling user Store access ensures that all app installations go through approved IT channels rather than direct consumer Store downloads. Store restriction prevents impulsive installation of untested applications that may conflict with enterprise configurations. Enterprise software distribution through Intune, ConfigMgr, or private Store maintains consistent endpoint configurations. Users who need specific applications should request them through IT helpdesk or self-service catalogs that enforce approval and compliance. Store access restriction is particularly important for regulated endpoints where unauthorized software installation violates compliance requirements.",
            Tags = ["appx", "store", "user-restriction", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStoreAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreAccess")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStoreAccess", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-audit-app-installations",
            Label = "Enable AppX Installation Audit Logging",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "AppX installation audit logging records all package installation and removal operations providing a complete history of app changes on managed endpoints. Enabling AppX installation auditing generates Windows events for all package install and uninstall operations with timestamp and user identity. Installation audit logs help detect unauthorized app installations that circumvent the approved software deployment process. Security teams can monitor for rapid installations of multiple packages which may indicate automated malware installation. AppX installation events should be forwarded to SIEM and correlated with the approved software catalog for compliance verification. Removal logs are valuable for investigating incidents where key security or compliance applications may have been deliberately uninstalled.",
            Tags = ["appx", "audit", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditAppInstallations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditAppInstallations")],
            DetectOps = [RegOp.CheckDword(Key, "AuditAppInstallations", 1)],
        },
        new TweakDef
        {
            Id = "appxbnd-block-non-store-apps",
            Label = "Block Installation of Apps Not from Store or MDM",
            Category = "AppX Bundle Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Restricting app installation to Store-sourced or MDM-deployed packages prevents installation of apps from unknown or untrusted distribution channels. Blocking non-Store and non-MDM apps ensures that all executable code on managed endpoints comes from Microsoft-vetted or IT-approved sources. Apps distributed through email, web download, or removable media are not subject to Store vetting and may contain malware. MSIX installer packages distributed outside the Store can be blocked through AppX policy to prevent unauthorized app deployments. MDM-based deployment channels like Intune and Configuration Manager enforce enterprise app approval workflows before installation. Restricting installation sources to Store and MDM creates a verifiable audit trail for all installed applications.",
            Tags = ["appx", "installation-source", "mdm", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNonStoreApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNonStoreApps")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNonStoreApps", 1)],
        },
    ];
}
