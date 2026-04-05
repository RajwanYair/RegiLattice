namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyAppControl
{
    // ── AppVirtualization ──
    private static class _AppVirtualization
    {
        private const string Client = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client";
        private const string Streaming = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Streaming";
        private const string Integration = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Integration";
        private const string Reporting = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Reporting";
        private const string Virtualization = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Virtualization";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appv-enable-package-scripts",
                Label = "Allow Scripts Inside App-V Packages",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "scripts", "virtualization", "packages", "policy"],
                Description =
                    "Permits PowerShell and batch scripts embedded within App-V packages to execute. "
                    + "Required for complex applications that use scripts for first-run configuration, "
                    + "licence activation, or environment setup. EnablePackageScripts=1.",
                ApplyOps = [RegOp.SetDword(Client, "EnablePackageScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(Client, "EnablePackageScripts")],
                DetectOps = [RegOp.CheckDword(Client, "EnablePackageScripts", 1)],
            },
            new TweakDef
            {
                Id = "appv-block-high-cost-launch",
                Label = "Block App-V Package Launch on Metered Connections",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "metered", "launch", "streaming", "cost"],
                Description =
                    "Prevents App-V packages from streaming content over metered network connections "
                    + "(AllowHighCostLaunch=0). Avoids unexpected data charges when users roam on mobile "
                    + "broadband. Packages that are already fully cached on disk still launch normally.",
                ApplyOps = [RegOp.SetDword(Client, "AllowHighCostLaunch", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowHighCostLaunch")],
                DetectOps = [RegOp.CheckDword(Client, "AllowHighCostLaunch", 0)],
            },
            new TweakDef
            {
                Id = "appv-require-admin-to-publish",
                Label = "Require Admin Rights to Publish App-V Packages Globally",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "publish", "admin", "security", "policy"],
                Description =
                    "Restricts global (all-user) App-V package publication to administrators only "
                    + "(RequirePublishAsAdmin=1). Standard users can still publish packages to their own "
                    + "profile. Prevents employees from self-publishing unvetted virtualised applications.",
                ApplyOps = [RegOp.SetDword(Client, "RequirePublishAsAdmin", 1)],
                RemoveOps = [RegOp.DeleteValue(Client, "RequirePublishAsAdmin")],
                DetectOps = [RegOp.CheckDword(Client, "RequirePublishAsAdmin", 1)],
            },
            new TweakDef
            {
                Id = "appv-autoload-previously-used",
                Label = "Auto-Load Previously Used App-V Packages in Background",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "autoload", "background", "streaming", "performance"],
                Description =
                    "Configures the App-V streaming engine to proactively background-load packages that the "
                    + "user has previously launched (AutoLoad=1, previously-used packages only). Improves "
                    + "subsequent launch times by ensuring packages are fully cached before the user needs them.",
                ApplyOps = [RegOp.SetDword(Streaming, "AutoLoad", 1)],
                RemoveOps = [RegOp.DeleteValue(Streaming, "AutoLoad")],
                DetectOps = [RegOp.CheckDword(Streaming, "AutoLoad", 1)],
            },
            new TweakDef
            {
                Id = "appv-disable-shared-content-store",
                Label = "Disable App-V Shared Content Store Mode",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "content-store", "disk", "streaming", "cache"],
                Description =
                    "Disables Shared Content Store (SCS) mode which streams content directly from the "
                    + "App-V server without local caching (SharedContentStoreMode=0). Enables full local "
                    + "caching for improved offline capability and resiliency when the App-V server is "
                    + "unavailable.",
                ApplyOps = [RegOp.SetDword(Streaming, "SharedContentStoreMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Streaming, "SharedContentStoreMode")],
                DetectOps = [RegOp.CheckDword(Streaming, "SharedContentStoreMode", 0)],
            },
            new TweakDef
            {
                Id = "appv-enable-process-interop",
                Label = "Enable App-V Process Interoperability",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "interop", "process", "integration", "virtual"],
                Description =
                    "Allows virtualised App-V processes to interoperate with natively installed processes "
                    + "outside the virtual environment (EnableProcessInterop=1). Required for scenarios "
                    + "where virtualised applications need to interact with local tools like printers, "
                    + "scanners, or on-device helper utilities.",
                ApplyOps = [RegOp.SetDword(Integration, "EnableProcessInterop", 1)],
                RemoveOps = [RegOp.DeleteValue(Integration, "EnableProcessInterop")],
                DetectOps = [RegOp.CheckDword(Integration, "EnableProcessInterop", 1)],
            },
            new TweakDef
            {
                Id = "appv-block-virtual-com-objects",
                Label = "Block Virtual COM Object Creation from App-V",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "com", "virtual", "objects", "security"],
                Description =
                    "Prevents App-V virtualised applications from creating out-of-process COM objects that "
                    + "would be visible to native (non-virtualised) processes (AllowVirtualCOMObjectCreation=0). "
                    + "Reduces COM-based code injection and isolation boundary escape attack surface.",
                ApplyOps = [RegOp.SetDword(Virtualization, "AllowVirtualCOMObjectCreation", 0)],
                RemoveOps = [RegOp.DeleteValue(Virtualization, "AllowVirtualCOMObjectCreation")],
                DetectOps = [RegOp.CheckDword(Virtualization, "AllowVirtualCOMObjectCreation", 0)],
            },
            new TweakDef
            {
                Id = "appv-enable-reporting",
                Label = "Enable App-V Usage Reporting",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "reporting", "telemetry", "usage", "analytics"],
                Description =
                    "Enables App-V client usage reporting which sends package launch, error, and access "
                    + "telemetry to the App-V management server (EnableReporting=1). Provides IT with "
                    + "application usage visibility for licence compliance and deployment health monitoring.",
                ApplyOps = [RegOp.SetDword(Reporting, "EnableReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(Reporting, "EnableReporting")],
                DetectOps = [RegOp.CheckDword(Reporting, "EnableReporting", 1)],
            },
            new TweakDef
            {
                Id = "appv-reporting-interval-24h",
                Label = "Set App-V Reporting Upload Interval to 24 Hours",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "reporting", "interval", "upload", "schedule"],
                Description =
                    "Sets the App-V client reporting upload interval to 24 hours (ReportingInterval=1440 "
                    + "minutes). Reduces reporting traffic overhead while ensuring daily freshness of usage "
                    + "data on the management server. Requires EnableReporting=1 to take effect.",
                ApplyOps = [RegOp.SetDword(Reporting, "ReportingInterval", 1440)],
                RemoveOps = [RegOp.DeleteValue(Reporting, "ReportingInterval")],
                DetectOps = [RegOp.CheckDword(Reporting, "ReportingInterval", 1440)],
            },
            new TweakDef
            {
                Id = "appv-streaming-timeout-120s",
                Label = "Set App-V Streaming Connection Timeout to 120 Seconds",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["appv", "streaming", "timeout", "network", "performance"],
                Description =
                    "Sets the App-V streaming connection timeout to 120 seconds (StreamingConnectionTimeout=120). "
                    + "On slow WAN links to the server, the default 30-second timeout causes premature failures. "
                    + "A longer timeout prevents 'Application failed to initialize' errors over high-latency links.",
                ApplyOps = [RegOp.SetDword(Streaming, "StreamingConnectionTimeout", 120)],
                RemoveOps = [RegOp.DeleteValue(Streaming, "StreamingConnectionTimeout")],
                DetectOps = [RegOp.CheckDword(Streaming, "StreamingConnectionTimeout", 120)],
            },
        ];
    }

    // ── AppxBundlePolicy ──
    private static class _AppxBundlePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppxBundle";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appxbnd-disable-side-loading",
                Label = "Disable App Side-Loading (AppX Side-loading Policy)",
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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

    // ── AppXPackagingPolicy ──
    private static class _AppXPackagingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppxPackaging";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appxpkg-disable-sideloading",
                Label = "Disable AppX Sideloading",
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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

    // ── AppxPolicy ──
    private static class _AppxPolicy
    {
        private const string AppxPolicy2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";

        private const string MsStorePolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";

        private const string ExplorerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

        private const string InstallerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appx-block-non-admin-install",
                Label = "Block Non-Admin UWP App Installation",
                Category = "Security — App Virtualization",
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
                Id = "appx-restrict-deployment-to-system-volume",
                Label = "Restrict AppX Deployment to System Volume Only",
                Category = "Security — App Virtualization",
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
                Id = "appx-disable-store-auto-update",
                Label = "Disable Automatic App Updates from Microsoft Store",
                Category = "Security — App Virtualization",
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
                Id = "appx-block-elevated-msi-install",
                Label = "Block Always-Install-Elevated MSI Packages",
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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

    // ── AppxProvisioningPolicy ──
    private static class _AppxProvisioningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "appxprov-require-private-store",
                Label = "AppX Provisioning Policy: Require Private Corporate Store Only",
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Id = "appxprov-block-consumer-provision",
                Label = "AppX Provisioning Policy: Block Consumer Experience App Provisioning",
                Category = "Security — App Virtualization",
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
                Id = "appxprov-disable-appx-deployment-service",
                Label = "AppX Provisioning Policy: Restrict APPX Deployment to Admin Only",
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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
                Category = "Security — App Virtualization",
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

    // ── CodeIntegrityAppPolicy ──
    private static class _CodeIntegrityAppPolicy
    {
        private const string DgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        private const string SrpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdacapp-enable-hypervisor-code-integrity",
                    Label = "WDAC: Enable HVCI (Hypervisor-Protected Code Integrity)",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets HypervisorEnforcedCodeIntegrity=1 in DeviceGuard policy. Enables Hypervisor-Protected Code Integrity (HVCI, also called Memory Integrity). HVCI moves kernel code integrity checking into the secure virtual machine backed by the CPU hypervisor, making it impossible for even a kernel-level exploit to modify the code signing enforcement rules. Without HVCI, a kernel exploit that gains ring-0 execution can disable code integrity by patching the CI routines in memory. HVCI requires hardware-enforced virtualisation (SLAT, IOMMU) and may require drivers to be WHQL-compliant. Incompatible drivers cause BSODs.",
                    Tags = ["hvci", "memory-integrity", "hypervisor", "kernel", "code-signing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "HVCI is enabled. Kernel code integrity is enforced from the secure VM. Incompatible drivers (non-WHQL or using deprecated kernel APIs) cause BSODs. Pre-deployment driver compatibility scan is mandatory. 5–15% kernel performance overhead on older hardware due to memory isolation transitions.",
                    ApplyOps = [RegOp.SetDword(DgKey, "HypervisorEnforcedCodeIntegrity", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "HypervisorEnforcedCodeIntegrity")],
                    DetectOps = [RegOp.CheckDword(DgKey, "HypervisorEnforcedCodeIntegrity", 1)],
                },
                new TweakDef
                {
                    Id = "wdacapp-enable-user-mode-code-integrity",
                    Label = "WDAC: Enable User-Mode Code Integrity (UMCI)",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets UsermodeCodeIntegrityPolicyEnforcementMode=1 in DeviceGuard policy. Enables enforcement of WDAC (Windows Defender Application Control) policies in user mode. UMCI extends application whitelisting from kernel-mode drivers to user-mode processes — requiring all executables (.exe, .dll, .ps1, script hosts) to be signed by trusted publishers before they are permitted to run. Without UMCI, application control only blocks untrusted kernel drivers. UMCI is the primary mechanism for application whitelisting that stops malware, ransomware, and living-off-the-land (LOtL) binaries from executing in user space.",
                    Tags = ["umci", "application-control", "whitelisting", "user-mode", "wdac"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "All user-mode executables, DLLs, and scripts must be signed by a trusted publisher. Unsigned or self-signed code is blocked. Legitimate internal applications that are unsigned must be signed or added to the WDAC policy exceptions before enabling enforcement mode. Recommended to run in audit mode for 90+ days before switching to enforcement.",
                    ApplyOps = [RegOp.SetDword(DgKey, "UsermodeCodeIntegrityPolicyEnforcementMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "UsermodeCodeIntegrityPolicyEnforcementMode")],
                    DetectOps = [RegOp.CheckDword(DgKey, "UsermodeCodeIntegrityPolicyEnforcementMode", 1)],
                },
                new TweakDef
                {
                    Id = "wdacapp-enable-srp-exe-control",
                    Label = "WDAC: Enable Software Restriction Policies for Executable Control",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets DefaultLevel=0 in SrpV2 policy. Configures Software Restriction Policies (SRP) to Disallowed mode for executable types not explicitly whitelisted. SRP is the compatibility-layer predecessor to AppLocker and WDAC — it operates as a ring-3 policy enforcement mechanism. In Disallowed mode, all executables are blocked unless a rule explicitly permits them. While WDAC is preferred for modern deployments, SRP provides a fallback enforcement layer for scenarios where WDAC policy is not yet in place or for downlevel OS compatibility within a mixed fleet.",
                    Tags = ["srp", "software-restriction", "disallowed-mode", "application-control", "whitelisting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Software Restriction Policies set to Disallowed by default. All executables are blocked unless explicitly whitelisted by path, hash, or certificate rules. SRP is user-mode only — a kernel exploit bypasses it. This is a complementary, not a replacement, control to WDAC/AppLocker. Extensive whitelisting of legitimate software is required before deploying.",
                    ApplyOps = [RegOp.SetDword(SrpKey, "DefaultLevel", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrpKey, "DefaultLevel")],
                    DetectOps = [RegOp.CheckDword(SrpKey, "DefaultLevel", 0)],
                },
                new TweakDef
                {
                    Id = "wdacapp-enable-wdac-policy-refresh",
                    Label = "WDAC: Enable Policy Refresh for WDAC Code Integrity Rules",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets EnablePolicyRefresh=1 in DeviceGuard policy. Enables the ability to refresh WDAC code integrity policies at runtime without rebooting. Policy refresh allows administrators to push updated WDAC policy files to devices and have the new rules take effect immediately for newly spawned processes, without requiring the device to restart. Without policy refresh, every WDAC policy update requires a reboot — making policy iteration and incident response much more disruptive in production environments. Refresh is a key operational enabler for WDAC managed environments.",
                    Tags = ["wdac", "policy-refresh", "runtime-update", "no-reboot", "operations"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAC policies can be refreshed at runtime without reboot. Updated rules apply to newly launched processes immediately. Running processes are not affected by the refresh until they restart. Requires deploying the new policy file via MDM or Group Policy file copy before triggering refresh.",
                    ApplyOps = [RegOp.SetDword(DgKey, "EnablePolicyRefresh", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "EnablePolicyRefresh")],
                    DetectOps = [RegOp.CheckDword(DgKey, "EnablePolicyRefresh", 1)],
                },
                new TweakDef
                {
                    Id = "wdacapp-enable-ci-audit-event-logging",
                    Label = "WDAC: Enable Code Integrity Audit Event Logging",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets AuditCodeIntegrityPolicyEnabled=1 in DeviceGuard policy. Enables audit event logging for Code Integrity policy violations in audit mode. When a WDAC policy is in audit mode (not enforcement mode), code that would have been blocked is logged as an audit event in the Microsoft-Windows-CodeIntegrity/Operational event log. These events include the binary path, the hash, the signing information, and why the binary would have been blocked. Audit events are essential for building the allow-list before switching to enforcement mode — production traffic can be captured and used to build an accurate whitelist.",
                    Tags = ["wdac", "audit-mode", "event-logging", "code-integrity", "allow-list-building"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Code Integrity policy violations are logged (not blocked). Events written to Microsoft-Windows-CodeIntegrity/Operational channel. Use audit logs to identify all unsigned or untrusted binaries before switching to enforcement. Usually run in audit mode for 30–90 days to capture all legitimate software.",
                    ApplyOps = [RegOp.SetDword(DgKey, "AuditCodeIntegrityPolicyEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "AuditCodeIntegrityPolicyEnabled")],
                    DetectOps = [RegOp.CheckDword(DgKey, "AuditCodeIntegrityPolicyEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wdacapp-block-vulnerable-driver-loading",
                    Label = "WDAC: Enable Vulnerable Driver Blocklist via HVCI",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets MicrosoftVulnerableDriverBlocklistEnabled=1 in DeviceGuard policy. Enables the Microsoft-maintained Vulnerable Driver Blocklist, which is a WDAC policy that prevents known WHQL-signed but vulnerable kernel drivers from loading. Attackers use BYOVD (Bring Your Own Vulnerable Driver) attacks where they load a legitimately signed but exploitable kernel driver and then use its vulnerabilities to escalate to ring-0 and bypass HVCI. The blocklist is updated by Microsoft with newly discovered vulnerable drivers and is applied at the hypervisor layer when HVCI is active.",
                    Tags = ["vulnerable-driver", "byovd", "hvci", "blocklist", "kernel"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Microsoft's vulnerable driver blocklist is enforced. Known exploitable WHQL drivers are blocked. LKD (Legitimate but Vulnerable Driver) attacks are prevented. If your environment legitimately requires a driver that appears on the blocklist, you must add an explicit allow rule. Blocklist is updated via Windows Update.",
                    ApplyOps = [RegOp.SetDword(DgKey, "MicrosoftVulnerableDriverBlocklistEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "MicrosoftVulnerableDriverBlocklistEnabled")],
                    DetectOps = [RegOp.CheckDword(DgKey, "MicrosoftVulnerableDriverBlocklistEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wdacapp-enable-smart-app-control-evaluation",
                    Label = "WDAC: Enable Smart App Control Evaluation Mode",
                    Category = "Security — App Virtualization",
                    Description =
                        "Sets SmartAppControlState=2 in DeviceGuard policy. Sets Smart App Control (SAC) to evaluation mode. SAC uses an AI-based cloud intelligence service combined with WDAC to block malware and potentially unwanted applications without requiring a pre-configured policy. In evaluation mode, SAC silently evaluates whether enforcement mode is feasible without disrupting existing workflows — if no legitimate app blocking would occur, it transitions to enforcement mode automatically. Value 2 = evaluation, 1 = enforcement, 0 = off. Evaluation mode is safe to enable on existing devices without the risk of blocking legitimate software.",
                    Tags = ["smart-app-control", "sac", "ai", "evaluation-mode", "malware-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart App Control enters evaluation mode. No apps are blocked during evaluation. The AI model evaluates whether full enforcement would cause issues. If no blocking would occur, SAC automatically transitions to enforcement. If issues are detected, SAC is turned off. Evaluation mode processes telemetry to the Microsoft cloud service.",
                    ApplyOps = [RegOp.SetDword(DgKey, "SmartAppControlState", 2)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "SmartAppControlState")],
                    DetectOps = [RegOp.CheckDword(DgKey, "SmartAppControlState", 2)],
                },
            ];
    }

    // ── CodeSigningPolicy ──
    private static class _CodeSigningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CodeSigning";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "codesign-require-signed-drivers",
                Label = "Require Signed Kernel Drivers",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Kernel driver signing requirements prevent unsigned or improperly signed drivers from loading into the Windows kernel. Requiring signed drivers ensures that only code vetted and signed by Microsoft or authorized cross-signers can execute in kernel mode. Unsigned drivers are a primary attack vector for rootkits and persistent malware that need kernel-level access to hide their activity. Windows 10 and later systems with Secure Boot enabled enforce driver signing automatically but policy reinforcement provides additional protection. Driver signing has been mandatory for 64-bit Windows since Vista but third-party tools and older drivers may attempt to bypass this requirement. Enforcing driver signing through policy prevents test signing mode and signature validation bypasses.",
                Tags = ["codesigning", "drivers", "kernel", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSignedDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedDrivers")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignedDrivers", 1)],
            },
            new TweakDef
            {
                Id = "codesign-require-cross-cert-chain",
                Label = "Require Cross-Certificate Validation for Drivers",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Cross-certificate chain validation requires that driver signatures trace back through a valid Microsoft-trusted cross-certificate hierarchy. Enabling cross-certificate requirements ensures that driver certificates issued by third-party CAs are chained through Microsoft's approved cross-certification program. Drivers signed with certificates not in the Microsoft cross-certificate program cannot be loaded even if the signature is technically valid. This policy prevents drivers signed by arbitrary commercial CAs or self-signed certificates from gaining kernel access. Cross-certificate validation is part of the Windows Hardware Quality Labs (WHQL) signing requirements for production drivers. Enforcing cross-certificate chains significantly reduces the attack surface for malicious kernel drivers attempting to use rogue or expired certificates.",
                Tags = ["codesigning", "cross-certificate", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireCrossCertificatesChain", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireCrossCertificatesChain")],
                DetectOps = [RegOp.CheckDword(Key, "RequireCrossCertificatesChain", 1)],
            },
            new TweakDef
            {
                Id = "codesign-disable-test-signing",
                Label = "Block Test Signing Mode",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Test signing mode allows unsigned or self-signed drivers to load for development purposes and is a significant security risk on production systems. Blocking test signing mode prevents users and malicious software from enabling bcdedit test signing which bypasses driver signature requirements. Attackers who gain administrator access can enable test signing to load malicious rootkits and drivers that would otherwise be blocked. Test signing mode is a BCD boot configuration option that can be set without UEFI Secure Boot being disabled. Policy enforcement of test signing restrictions prevents persistent configuration changes that would survive reboots. Production endpoints should never run in test signing mode and policy enforcement prevents accidental or malicious enablement.",
                Tags = ["codesigning", "test-signing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockTestSigningMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockTestSigningMode")],
                DetectOps = [RegOp.CheckDword(Key, "BlockTestSigningMode", 1)],
            },
            new TweakDef
            {
                Id = "codesign-require-kernel-ehashes",
                Label = "Enable Enhanced Hash Algorithm for Driver Signing",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enhanced hash algorithms require driver signatures to use SHA-256 or stronger hash algorithms rather than the deprecated SHA-1. Enabling enhanced hash requirements ensures that driver signatures cannot be forged through SHA-1 collision attacks. SHA-1 has been cryptographically broken and certificates or signatures using SHA-1 should be considered untrusted in security-sensitive contexts. Windows has deprecated SHA-1 code signing certificates for drivers but policy enforcement ensures no SHA-1 signed drivers are accepted. Enhanced hash enforcement applies to both kernel-mode and user-mode driver components loaded during system operation. Transitioning entirely to SHA-256 or stronger hash algorithms for driver signing is best practice for all enterprise deployments.",
                Tags = ["codesigning", "sha256", "hash", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireEnhancedKeyHashes", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireEnhancedKeyHashes")],
                DetectOps = [RegOp.CheckDword(Key, "RequireEnhancedKeyHashes", 1)],
            },
            new TweakDef
            {
                Id = "codesign-enable-code-integrity-policy",
                Label = "Enable Code Integrity Policy Enforcement",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Code integrity policy enforcement validates all kernel-mode code before execution against a policy that defines allowed code. Enabling code integrity enforcement prevents any code not matching the allowed code policy from executing in kernel mode. This forms the foundation of Windows HVCI and hypervisor-protected code integrity that protects the kernel from malicious drivers. Code integrity policies can be audit-only initially to identify violations before switching to enforcement mode. Policy-based code integrity is more flexible than simple driver signing as it can enforce specific file hashes and publisher identities. Code integrity policy combined with virtualization-based security provides the highest level of kernel protection available on Windows platforms.",
                Tags = ["codesigning", "integrity", "hvci", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableCodeIntegrityPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCodeIntegrityPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCodeIntegrityPolicy", 1)],
            },
            new TweakDef
            {
                Id = "codesign-block-vulnerable-drivers",
                Label = "Enable Microsoft Vulnerable Driver Blocklist",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The Microsoft Vulnerable Driver Blocklist prevents known vulnerable signed drivers from loading even though they have valid signatures. Enabling the blocklist protects against bring-your-own-vulnerable-driver attacks where attackers load signed but vulnerable drivers to escalate privileges. Signed drivers with known exploitable vulnerabilities have been used in numerous ransomware and APT attacks to bypass security software. The blocklist is maintained by Microsoft and updated through Windows updates to include newly discovered vulnerable drivers. Drivers on the blocklist include those with arbitrary kernel memory read/write capabilities, privilege escalation vulnerabilities, and security bypass functions. Enabling the vulnerable driver blocklist should be standard practice on all enterprise endpoints without compatibility exceptions.",
                Tags = ["codesigning", "blocklist", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableVulnerableDriverBlocklist", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableVulnerableDriverBlocklist")],
                DetectOps = [RegOp.CheckDword(Key, "EnableVulnerableDriverBlocklist", 1)],
            },
            new TweakDef
            {
                Id = "codesign-require-signed-scripts",
                Label = "Require Signed Executable Scripts",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Requiring signed executable scripts prevents malicious or unauthorized scripts from running in the Windows scripting environment. Script signing requirements apply to PowerShell scripts, batch files, and other executable script types depending on the script host configuration. Signed script requirements complement PowerShell execution policy but provide a broader policy mechanism applicable across script hosts. Unsigned scripts are a common delivery mechanism for malware, ransomware, and post-exploitation frameworks in enterprise attacks. Script signing tied to an enterprise PKI ensures that only IT-approved scripts can run on managed endpoints. Requiring signed scripts reduces the risk from phishing-delivered scripts and malicious script injections into legitimate directories.",
                Tags = ["codesigning", "scripts", "powershell", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSignedScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedScripts")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignedScripts", 1)],
            },
            new TweakDef
            {
                Id = "codesign-enable-umci",
                Label = "Enable User Mode Code Integrity (UMCI)",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "User Mode Code Integrity extends code integrity checking to user-mode processes requiring that all executable code be authorized by the code integrity policy. Enabling UMCI prevents execution of unauthorized binaries and DLLs in user space complementing kernel code integrity enforcement. UMCI is the user-mode component of Windows Defender Application Control and provides comprehensive protection against unauthorized code. User mode code integrity can prevent malicious DLL injection, unauthorized process creation, and execution of malware dropped by exploits. Device Guard in full lockdown mode combines HVCI with UMCI for both kernel and user mode protection. UMCI requires careful policy development to avoid blocking legitimate applications and may require an audit period before enforcement.",
                Tags = ["codesigning", "umci", "applocker", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableUMCI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableUMCI")],
                DetectOps = [RegOp.CheckDword(Key, "EnableUMCI", 1)],
            },
            new TweakDef
            {
                Id = "codesign-block-dll-from-temp",
                Label = "Block Code Loading from Temporary Directories",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Blocking code loading from temporary directories prevents malware dropped to TEMP locations from being executed through DLL side-loading or process injection. Code integrity policy rules can block executable and DLL loading from paths like %TEMP%, Downloads, and other writable user directories. Temporary directory-based execution is a hallmark of malware that avoids writing to monitored program directories. Attackers frequently exploit applications with DLL search order vulnerabilities to load malicious DLLs from the application directory or TEMP paths. Blocking code from temporary directories significantly reduces the attack surface for DLL hijacking and self-extracting malware delivery. This protection complements AppLocker and Windows Defender Application Control path-based rules targeting common malware staging locations.",
                Tags = ["codesigning", "temp", "dll", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockCodeFromTempPaths", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCodeFromTempPaths")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCodeFromTempPaths", 1)],
            },
            new TweakDef
            {
                Id = "codesign-audit-code-integrity",
                Label = "Enable Code Integrity Audit Logging",
                Category = "Security — App Virtualization",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Code integrity audit logging records events whenever code is blocked or would have been blocked by code integrity policy in audit mode. Enabling code integrity audit events provides visibility into code integrity violations that can inform policy development and refinement. Audit logs identify applications, drivers, and scripts that would fail enforcement-mode code integrity to prepare for enforcement without disruption. Event ID 3076 and related code integrity events in the Microsoft-Windows-CodeIntegrity log provide detailed blocking information. Code integrity audit data should be collected to SIEM systems for correlation with other security events. Audit logging should always be enabled during the policy development phase before switching from audit to enforcement mode.",
                Tags = ["codesigning", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableAuditCodeIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAuditCodeIntegrity")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAuditCodeIntegrity", 1)],
            },
        ];
    }

    // ── MicrosoftStorePolicy ──
    private static class _MicrosoftStorePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";
        private const string AppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";
        private const string LicKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppLicense";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "storepol-disable-store-in-shelf",
                    Label = "Disable Microsoft Store Suggestions in Taskbar (Shelf)",
                    Category = "Security — App Virtualization",
                    Description =
                        "Prevents the Microsoft Store from displaying app suggestions and promotions in the Windows taskbar shelf and Start menu recommended section, reducing promotional clutter on managed corporate desktops.",
                    Tags = ["store", "shelf", "taskbar", "suggestions", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Store promotional suggestions in taskbar shelf and Start menu disabled; no app promotions displayed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreShelf", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreShelf")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreShelf", 1)],
                },
                new TweakDef
                {
                    Id = "storepol-disable-app-license-acquisition",
                    Label = "Disable Automatic App License Acquisition from Store",
                    Category = "Security — App Virtualization",
                    Description =
                        "Prevents applications from automatically acquiring new or updated licenses from the Microsoft Store License Service in the background, ensuring license state changes are predictable and do not occur without admin approval.",
                    Tags = ["store", "license", "auto-acquisition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Automatic app license acquisition disabled; Store license updates require manual trigger or admin action.",
                    ApplyOps = [RegOp.SetDword(LicKey, "DisableAutoLicenseAcquisition", 1)],
                    RemoveOps = [RegOp.DeleteValue(LicKey, "DisableAutoLicenseAcquisition")],
                    DetectOps = [RegOp.CheckDword(LicKey, "DisableAutoLicenseAcquisition", 1)],
                },
                new TweakDef
                {
                    Id = "storepol-disable-store-update-background",
                    Label = "Disable Background App Update via Microsoft Store",
                    Category = "Security — App Virtualization",
                    Description =
                        "Prevents installed UWP apps from automatically updating in the background via the Store update service, ensuring app version changes go through controlled deployment channels.",
                    Tags = ["store", "auto-update", "background", "uwp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Store background app updates disabled; UWP apps only updated on explicit user or admin trigger.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableOSUpgrade", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableOSUpgrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableOSUpgrade", 0)],
                },
                new TweakDef
                {
                    Id = "storepol-disable-store-telemetry",
                    Label = "Disable Microsoft Store Telemetry to Microsoft",
                    Category = "Security — App Virtualization",
                    Description =
                        "Prevents the Microsoft Store client from sending browsing history, search queries, purchase activity, and app installation statistics to Microsoft.",
                    Tags = ["store", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Store telemetry to Microsoft disabled; browsing, search, and install data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "storepol-log-appx-install-events",
                    Label = "Log Appx Package Installation Events in Security Log",
                    Category = "Security — App Virtualization",
                    Description =
                        "Enables Security event log entries for every Appx/MSIX package installation, update, and removal event, providing a complete audit trail of UWP app deployments on the endpoint.",
                    Tags = ["store", "appx", "audit", "event-log", "install", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Appx package install/update/remove events logged in Security log; full UWP deployment audit trail.",
                    ApplyOps = [RegOp.SetDword(AppKey, "AuditAppxInstallEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(AppKey, "AuditAppxInstallEvents")],
                    DetectOps = [RegOp.CheckDword(AppKey, "AuditAppxInstallEvents", 1)],
                },
            ];
    }

    // ── MsiInstallerPolicy ──
    private static class _MsiInstallerPolicy
    {
        private const string Inst = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "msipol-disable-patch-install",
                Label = "Prevent Users from Patching MSI Packages",
                Category = "Security — App Virtualization",
                Description =
                    "Sets DisablePatch=1 in Windows Installer policy. Prevents users from patching any MSI application by blocking the application of .msp patch files. Only administrators can apply patches. Stops untrusted patches from silently modifying installed applications.",
                Tags = ["msi", "installer", "patch", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "DisablePatch", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "DisablePatch")],
                DetectOps = [RegOp.CheckDword(Inst, "DisablePatch", 1)],
            },
            new TweakDef
            {
                Id = "msipol-disable-source-browsing",
                Label = "Prevent Users from Browsing Install Sources",
                Category = "Security — App Virtualization",
                Description =
                    "Sets DisableBrowse=1 in Windows Installer policy. Prevents the Windows Installer from allowing users to browse for an installation source (e.g., a different CD or network share) when a product is being repaired or re-installed. All installs must use the cached or registered source path.",
                Tags = ["msi", "installer", "browse", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "DisableBrowse", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "DisableBrowse")],
                DetectOps = [RegOp.CheckDword(Inst, "DisableBrowse", 1)],
            },
            new TweakDef
            {
                Id = "msipol-restrict-user-installs",
                Label = "Restrict MSI Installs to Elevated Users Only",
                Category = "Security — App Virtualization",
                Description =
                    "Sets DisableMSI=1 in Windows Installer policy. Restricts Windows Installer so that only administrators can install MSI packages (standard users receive an error). Value 0=allow all, 1=admins only, 2=block all MSI. Setting 1 prevents software installation by standard accounts.",
                Tags = ["msi", "installer", "restrict", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "DisableMSI", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "DisableMSI")],
                DetectOps = [RegOp.CheckDword(Inst, "DisableMSI", 1)],
            },
            new TweakDef
            {
                Id = "msipol-secure-transforms",
                Label = "Secure MSI Transform Files in User Profile",
                Category = "Security — App Virtualization",
                Description =
                    "Sets TransformsSecure=1 in Windows Installer policy. Instructs the Windows Installer to store MSI transform (.mst) files in a secure location in the user profile rather than in the TEMP directory. Prevents other users from tampering with transform files used during product re-installation.",
                Tags = ["msi", "installer", "transforms", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "TransformsSecure", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "TransformsSecure")],
                DetectOps = [RegOp.CheckDword(Inst, "TransformsSecure", 1)],
            },
            new TweakDef
            {
                Id = "msipol-disable-scripting",
                Label = "Disable Unsafe MSI Script Execution",
                Category = "Security — App Virtualization",
                Description =
                    "Sets SafeForScripting=0 in Windows Installer policy. Disables the ability for web-based content or scripts to silently invoke the Windows Installer COM object to install software. Prevents drive-by installations triggered by browser scripts or malicious web pages.",
                Tags = ["msi", "installer", "scripting", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "SafeForScripting", 0)],
                RemoveOps = [RegOp.DeleteValue(Inst, "SafeForScripting")],
                DetectOps = [RegOp.CheckDword(Inst, "SafeForScripting", 0)],
            },
            new TweakDef
            {
                Id = "msipol-enforce-upgrade-component-rules",
                Label = "Enforce MSI Upgrade Component Rules",
                Category = "Security — App Virtualization",
                Description =
                    "Sets EnforceUpgradeComponentRules=1 in Windows Installer policy. Causes the Windows Installer to reject patches that would violate component rules during an upgrade sequence. Prevents improperly authored patches from corrupting installed applications by adding or removing component references outside of the product's authorised upgrade path.",
                Tags = ["msi", "installer", "upgrade", "policy", "integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "EnforceUpgradeComponentRules", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "EnforceUpgradeComponentRules")],
                DetectOps = [RegOp.CheckDword(Inst, "EnforceUpgradeComponentRules", 1)],
            },
            new TweakDef
            {
                Id = "msipol-limit-restore-checkpoints",
                Label = "Limit System Restore Points During MSI Install",
                Category = "Security — App Virtualization",
                Description =
                    "Sets LimitSystemRestoreCheckpointing=1 in Windows Installer policy. Prevents the Windows Installer from creating a System Restore checkpoint before every package installation. Reduces System Restore disk space consumption and write activity on machines where MSI packages are frequently deployed.",
                Tags = ["msi", "installer", "restore", "policy", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "LimitSystemRestoreCheckpointing", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "LimitSystemRestoreCheckpointing")],
                DetectOps = [RegOp.CheckDword(Inst, "LimitSystemRestoreCheckpointing", 1)],
            },
            new TweakDef
            {
                Id = "msipol-disable-lockdown-browse-ui",
                Label = "Restrict Browse UI in Lockdown Mode",
                Category = "Security — App Virtualization",
                Description =
                    "Sets DisableLockdownBrowseUI=1 in Windows Installer policy. When an MSI package runs in locked-down mode (elevated), this setting prevents the installer from displaying any file-browse dialogs that would let the user navigate the file system during setup. Closes a potential path-traversal risk in privileged installer contexts.",
                Tags = ["msi", "installer", "lockdown", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "DisableLockdownBrowseUI", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "DisableLockdownBrowseUI")],
                DetectOps = [RegOp.CheckDword(Inst, "DisableLockdownBrowseUI", 1)],
            },
            new TweakDef
            {
                Id = "msipol-disable-forbidden-patch",
                Label = "Restrict Patching to Authorised Patch Lists",
                Category = "Security — App Virtualization",
                Description =
                    "Sets DisableForbidPatch=0 in Windows Installer policy. Ensures that patch policies (AllowedPatchList / ForbiddenPatchList) are honoured by the Windows Installer, so only administrator-approved patches can be applied to managed MSI products. Value 1 would disable the forbidden list enforcement.",
                Tags = ["msi", "installer", "patch", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "DisableForbidPatch", 0)],
                RemoveOps = [RegOp.DeleteValue(Inst, "DisableForbidPatch")],
                DetectOps = [RegOp.CheckDword(Inst, "DisableForbidPatch", 0)],
            },
            new TweakDef
            {
                Id = "msipol-disable-media-source-fallback",
                Label = "Disable MSI Source Fallback to Removable Media",
                Category = "Security — App Virtualization",
                Description =
                    "Sets DisableMedia=1 in Windows Installer policy. Prevents the Windows Installer from falling back to removable media (CD/DVD/USB) as an installation source when the cached or network source is unavailable. Stops users from introducing software from removable media during repair or re-installation.",
                Tags = ["msi", "installer", "media", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Inst, "DisableMedia", 1)],
                RemoveOps = [RegOp.DeleteValue(Inst, "DisableMedia")],
                DetectOps = [RegOp.CheckDword(Inst, "DisableMedia", 1)],
            },
        ];
    }

    // ── PackagedAppDebugPolicy ──
    private static class _PackagedAppDebugPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PackagedAppXDebug";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "padebug-disable-developer-mode",
                    Label = "Disable Windows Developer Mode",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents enabling Windows Developer Mode, which allows sideloading of unsigned or developer-signed MSIX/AppX packages and activates various debug features. Reduces the attack surface on production endpoints. Default: toggle available to users. Recommended: 1.",
                    Tags = ["developer-mode", "sideloading", "appx", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Developer Mode toggle in Settings is greyed out; unsigned package sideloading is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockDeveloperMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockDeveloperMode")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockDeveloperMode", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-debuggable-package-install",
                    Label = "Block Installation of Debug-Flagged Packages",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents installation of MSIX/AppX packages compiled with the debuggable attribute. Debug-flagged packages may expose app internals to debugger attachment without normal authentication. Default: install allowed. Recommended: 1 on production machines.",
                    Tags = ["developer-mode", "debuggable", "appx", "package", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AppX packages with debuggable=true are rejected during install; reduces debugger injection risk.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockDebuggablePackageInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockDebuggablePackageInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockDebuggablePackageInstall", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-test-signing",
                    Label = "Disable AppX Test Signing Mode",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents Windows from entering AppX test-signing mode that allows packages signed with developer test certificates to execute. Only packages from the Microsoft Store or trusted enterprise signing chains may run. Default: test signing disabled by default on non-dev machines. Recommended: 1.",
                    Tags = ["developer-mode", "test-signing", "certificate", "appx", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Packages signed with test certificates are rejected; only Store-signed or enterprise-signed packages run.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTestSigning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTestSigning")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTestSigning", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-loopback-for-packages",
                    Label = "Disable AppX Network Loopback Exemption",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents packaged apps from using the network loopback (localhost 127.0.0.1), which is normally blocked by AppContainer isolation. Loopback exemption is a common developer workaround that weakens sandbox isolation. Default: loopback blocked by default. Recommended: 1 to lockdown on production.",
                    Tags = ["developer-mode", "loopback", "appcontainer", "sandbox", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AppX packages cannot access localhost; AppContainer network isolation is fully enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockLoopbackExemption", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockLoopbackExemption")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockLoopbackExemption", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-device-portal",
                    Label = "Disable Windows Device Portal",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Blocks enabling the Windows Device Portal (a web-based debug interface accessible over Wi-Fi/Ethernet when Developer Mode is on). Eliminates a remote code execution surface. Default: disabled unless Developer Mode is on. Recommended: 1.",
                    Tags = ["developer-mode", "device-portal", "remote-access", "web", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Windows Device Portal cannot be enabled; remote debug web service is fully blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockDevicePortal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockDevicePortal")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockDevicePortal", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-diagnostics-tracking",
                    Label = "Block AppX Diagnostic Tracking",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Stops packaged apps from submitting debug diagnostics and crash telemetry to the Windows Debug & Diagnostics channel. Prevents app stability data from leaving the device. Default: tracking enabled. Recommended: 1 for data-sovereignty.",
                    Tags = ["developer-mode", "diagnostics", "tracking", "telemetry", "appx", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "AppX crash reports and diagnostic telemetry are blocked from leaving the device.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAppDiagnosticsTracking", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAppDiagnosticsTracking")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAppDiagnosticsTracking", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-background-task-debug",
                    Label = "Block Background Task Debugger Attachment",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents debuggers from attaching to packaged app background task processes. Reduces risk of debugger-based runtime code injection targeting background agents. Default: not restricted. Recommended: 1 on production endpoints.",
                    Tags = ["developer-mode", "background-task", "debugger", "injection", "appx", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Debugger cannot attach to AppX background tasks; protects background agents from runtime code injection.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockBackgroundTaskDebug", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockBackgroundTaskDebug")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockBackgroundTaskDebug", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-disable-fiddler-proxy-debug",
                    Label = "Block HTTP Debug Proxy for AppX Traffic",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents packaged apps from routing their HTTP/HTTPS traffic through a debugging proxy (such as Fiddler). AppContainer typically blocks proxy use; this policy reinforces that restriction. Default: proxy debug blocked. Recommended: 1.",
                    Tags = ["developer-mode", "proxy", "fiddler", "appcontainer", "appx", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "AppX HTTP traffic cannot be intercepted via local debug proxies; AppContainer isolation is reinforced.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockHttpDebugProxy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockHttpDebugProxy")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockHttpDebugProxy", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-enable-package-integrity-check",
                    Label = "Enforce AppX Package Integrity Check on Load",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables cryptographic integrity verification on packaged app binaries at load time. Detects and blocks tampered or patched AppX packages before execution. Default: integrity checks at install time only. Recommended: 1 for high-security deployments.",
                    Tags = ["developer-mode", "integrity", "signature", "appx", "anti-tamper", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Binary integrity is checked on every AppX load; tampered packages are blocked before they execute.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforcePackageIntegrityOnLoad", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforcePackageIntegrityOnLoad")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforcePackageIntegrityOnLoad", 1)],
                },
                new TweakDef
                {
                    Id = "padebug-log-sideload-attempts",
                    Label = "Audit Log All AppX Sideload Attempts",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Records all attempts to sideload (install from outside the Store) AppX/MSIX packages to the Security audit log. Provides forensic visibility into unauthorised package install attempts. Default: not audited. Recommended: 1.",
                    Tags = ["developer-mode", "sideload", "audit", "appx", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sideload install attempts are written to the Security log; detectable by SIEM as potential policy violation.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSideloadAttempts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSideloadAttempts")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSideloadAttempts", 1)],
                },
            ];
    }

    // ── PushToInstallPolicy ──
    private static class _PushToInstallPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PushToInstall";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pti-disable-push-to-install",
                Label = "Disable Push-To-Install Service",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisablePushToInstall=1 in the PushToInstall policy key. Prevents the "
                    + "Push-to-Install feature from delivering apps remotely. Push-to-Install allows "
                    + "apps purchased or selected on one device to be silently installed on another "
                    + "device signed in with the same Microsoft account. Blocking this prevents "
                    + "unexpected app installations. Default: 0. Recommended: 1 for managed estates.",
                Tags = ["push-to-install", "store", "remote", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePushToInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePushToInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePushToInstall", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-remote-push",
                Label = "Disable Remote Push App Delivery",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableRemotePush=1 in the PushToInstall policy key. Blocks delivery of "
                    + "applications to this device initiated from a remote session or another device. "
                    + "Remote push allows an administrator or the account owner to trigger Store app "
                    + "installations on a target machine without local interaction. "
                    + "Default: 0. Recommended: 1 on enterprise endpoints.",
                Tags = ["push-to-install", "remote", "delivery", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRemotePush", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRemotePush")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRemotePush", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-auto-provisioning",
                Label = "Disable Push-To-Install Auto Provisioning",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableAutoProvisioning=1 in the PushToInstall policy key. Prevents "
                    + "automatic app provisioning triggered by the Push-to-Install service when "
                    + "a device is first joined to an account or MDM enrollment. Auto-provisioning "
                    + "can install a large batch of apps without user review. "
                    + "Default: 0. Recommended: 1 on carefully managed devices.",
                Tags = ["push-to-install", "provisioning", "auto", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoProvisioning", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-device-management-push",
                Label = "Disable Device Management Push Installs",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableDeviceManagementPush=1 in the PushToInstall policy key. Blocks "
                    + "the device management channel from using Push-to-Install to deploy Store "
                    + "applications. MDM solutions such as Intune can use this channel to push "
                    + "commercial app packages silently. This policy prevents that silent delivery "
                    + "vector at the OS policy layer. Default: 0. Recommended: 1 when a separate "
                    + "software distribution tool manages app deployment.",
                Tags = ["push-to-install", "mdm", "device-mgmt", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeviceManagementPush", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceManagementPush")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeviceManagementPush", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-store-push-notifications",
                Label = "Disable Push-To-Install Store Notifications",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableStorePushNotifications=1 in the PushToInstall policy key. Disables "
                    + "notification toasts generated by the Push-to-Install service to inform users "
                    + "that an app is being installed or has been successfully delivered from another "
                    + "device. Reduces distraction and prevents disclosure of remote management "
                    + "actions to end users. Default: 0. Recommended: 1.",
                Tags = ["push-to-install", "notifications", "store", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStorePushNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStorePushNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStorePushNotifications", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-install-telemetry",
                Label = "Disable Push-To-Install Telemetry",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableInstallTelemetry=1 in the PushToInstall policy key. Prevents the "
                    + "Push-to-Install service from reporting installation events, success or failure "
                    + "outcomes, and app engagement data back to Microsoft. This telemetry is "
                    + "separate from standard diagnostic data and targets Store usage analytics. "
                    + "Default: 0. Recommended: 1 when minimising data sharing with Microsoft.",
                Tags = ["push-to-install", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInstallTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInstallTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInstallTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "pti-require-admin-approval",
                Label = "Require Admin Approval for Push-To-Install",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RequireAdminApproval=1 in the PushToInstall policy key. Requires local "
                    + "administrator confirmation before any remotely-pushed application may be "
                    + "installed. This adds a UAC-equivalent gate to the push delivery pipeline, "
                    + "preventing silent installs initiated from trusted Microsoft accounts or "
                    + "management channels. Default: 0. Recommended: 1 for shared machines.",
                Tags = ["push-to-install", "admin", "approval", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminApproval", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminApproval")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminApproval", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-unattended-push",
                Label = "Disable Unattended Push-To-Install",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableUnattendedPush=1 in the PushToInstall policy key. Prevents "
                    + "unattended (background, no-user-present) push installations from executing "
                    + "when the device screen is locked or the user is not logged in. Unattended "
                    + "push can silently replace or downgrade apps on locked devices without user "
                    + "knowledge. Default: 0. Recommended: 1.",
                Tags = ["push-to-install", "unattended", "background", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUnattendedPush", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUnattendedPush")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUnattendedPush", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-cross-device-sync",
                Label = "Disable Push-To-Install Cross-Device Sync",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableCrossDeviceSync=1 in the PushToInstall policy key. Prevents "
                    + "synchronisation of the app installation queue across devices in the same "
                    + "Microsoft account family. Without this policy, purchasing an app on a phone "
                    + "or Xbox console can trigger a silent push install to all Windows devices "
                    + "in the account. Default: 0. Recommended: 1 for isolation between devices.",
                Tags = ["push-to-install", "sync", "cross-device", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCrossDeviceSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossDeviceSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCrossDeviceSync", 1)],
            },
            new TweakDef
            {
                Id = "pti-disable-push-service-wake",
                Label = "Disable Push-To-Install Service Wake",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisablePushServiceWake=1 in the PushToInstall policy key. Prevents the "
                    + "Push-to-Install background service from waking the device from sleep or "
                    + "connected standby to complete a pending installation. This can cause "
                    + "unexpected fan spin, battery drain, or network activity while the device "
                    + "is supposedly idle or in a bag. Default: 0. Recommended: 1 for laptops.",
                Tags = ["push-to-install", "sleep", "wake", "power", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePushServiceWake", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePushServiceWake")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePushServiceWake", 1)],
            },
        ];
    }

    // ── SmartAppControlPolicy ──
    private static class _SmartAppControlPolicy
    {
        private const string SacKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartAppControl";
        private const string WdCiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen";
        private const string SacStateKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sac-block-policy-change",
                    Label = "Block User Changes to Smart App Control State",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents users from changing the Smart App Control state (evaluation / on / off) via Windows Security settings. The state set by the administrator via policy is locked in place.",
                    Tags = ["sac", "smart-app-control", "policy", "user-lock", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Ensures Smart App Control remains in its managed state regardless of user preferences.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "ConfigureSmartAppControl", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "ConfigureSmartAppControl")],
                    DetectOps = [RegOp.CheckDword(SacKey, "ConfigureSmartAppControl", 1)],
                },
                new TweakDef
                {
                    Id = "sac-enable-enforcement-mode",
                    Label = "Set Smart App Control to Enforcement Mode",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Forces Smart App Control into full Enforcement mode, blocking unsigned and reputation-negative apps from running. Moves the system out of Evaluation mode.",
                    Tags = ["sac", "smart-app-control", "enforcement", "app-block", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Blocks all apps that do not have a valid code signature or positive Microsoft cloud reputation. Test on a pilot group; may block legitimate unsigned tools.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "SmartAppControlState", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "SmartAppControlState")],
                    DetectOps = [RegOp.CheckDword(SacKey, "SmartAppControlState", 1)],
                },
                new TweakDef
                {
                    Id = "sac-disable-evaluation-mode",
                    Label = "Disable Smart App Control Evaluation Mode",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents Windows from running Smart App Control in Evaluation mode, which silently collects data about apps that would be blocked by enforcement. Requires choosing explicit On or Off state.",
                    Tags = ["sac", "smart-app-control", "evaluation", "policy", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Ensures the device is in a known enforcement state rather than the ambiguous evaluation state.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "DisableEvaluationMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "DisableEvaluationMode")],
                    DetectOps = [RegOp.CheckDword(SacKey, "DisableEvaluationMode", 1)],
                },
                new TweakDef
                {
                    Id = "sac-require-signed-publishers",
                    Label = "Require Signed Publishers for All Executable Content",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Configures Smart App Control to require a valid, traceable code-signing publisher certificate for all PE executables, MSI packages, and scripts. Unsigned content is blocked.",
                    Tags = ["sac", "smart-app-control", "code-signing", "publisher", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Strong protection against unsigned malware; breaks all in-house tools that lack a valid code-signing certificate. Ensure all LOB apps are signed before enabling.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "RequireSignedPublishers", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "RequireSignedPublishers")],
                    DetectOps = [RegOp.CheckDword(SacKey, "RequireSignedPublishers", 1)],
                },
                new TweakDef
                {
                    Id = "sac-block-malicious-script-execution",
                    Label = "Block Script Files Identified as Malicious by SAC",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables Smart App Control to block script execution (JS, VBS, PS1, CMD) when the script file or publisher is identified as malicious by the Microsoft Intelligent Security Graph.",
                    Tags = ["sac", "smart-app-control", "scripts", "malicious", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Stops script-based threats (LotL attacks) that use reputation-negative or anonymous scripts.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "BlockMaliciousScripts", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "BlockMaliciousScripts")],
                    DetectOps = [RegOp.CheckDword(SacKey, "BlockMaliciousScripts", 1)],
                },
                new TweakDef
                {
                    Id = "sac-audit-blocked-file-events",
                    Label = "Enable Audit Events for SAC-Blocked Files",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Configures Smart App Control to write an Windows event for every file that is blocked or audited, including the file hash, publisher, and reason for the block decision.",
                    Tags = ["sac", "smart-app-control", "audit", "event-log", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Provides a forensic record of blocked app attempts, supporting SOC investigation and compliance reporting.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "AuditBlockedFileEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "AuditBlockedFileEvents")],
                    DetectOps = [RegOp.CheckDword(SacKey, "AuditBlockedFileEvents", 1)],
                },
                new TweakDef
                {
                    Id = "sac-disable-cloud-lookup",
                    Label = "Disable Smart App Control Cloud Reputation Lookup",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents SAC from sending file hashes and metadata to the Microsoft Intelligent Security Graph cloud service for reputation evaluation. SAC falls back to local developer-mode checks only.",
                    Tags = ["sac", "smart-app-control", "cloud", "privacy", "network-isolation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Suitable for air-gapped or high-security environments; reduces SAC effectiveness as the cloud model is the primary signal source.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "DisableCloudReputationLookup", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "DisableCloudReputationLookup")],
                    DetectOps = [RegOp.CheckDword(SacKey, "DisableCloudReputationLookup", 1)],
                },
                new TweakDef
                {
                    Id = "sac-extend-to-network-paths",
                    Label = "Apply Smart App Control to Network-Path Executables",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Extends Smart App Control enforcement to executables launched from UNC network paths and mapped drives, not just local storage. Prevents bypass by placing unsigned tools on a file share.",
                    Tags = ["sac", "smart-app-control", "network", "unc-path", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Network-launched binaries are less commonly signed; pilot before enforcing to avoid blocking legitimate admin tools from network shares.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "ExtendToNetworkPaths", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "ExtendToNetworkPaths")],
                    DetectOps = [RegOp.CheckDword(SacKey, "ExtendToNetworkPaths", 1)],
                },
                new TweakDef
                {
                    Id = "sac-block-lolbas-abuse",
                    Label = "Block Known LOLBAS Misuse via Smart App Control",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables additional Smart App Control rules that block known Living-off-the-Land Binaries and Scripts (LOLBAS) from being used in patterns typically associated with attackers (e.g., certutil download, regsvr32 scriptlet).",
                    Tags = ["sac", "smart-app-control", "lolbas", "living-off-land", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "May interfere with legitimate administrative use of tools like certutil, msiexec, or rundll32. Review the specific exclusions needed before enabling.",
                    RegistryKeys = [SacKey],
                    ApplyOps = [RegOp.SetDword(SacKey, "BlockLolbasAbuse", 1)],
                    RemoveOps = [RegOp.DeleteValue(SacKey, "BlockLolbasAbuse")],
                    DetectOps = [RegOp.CheckDword(SacKey, "BlockLolbasAbuse", 1)],
                },
                new TweakDef
                {
                    Id = "sac-enable-intelligent-security-graph",
                    Label = "Enable Intelligent Security Graph Integration for SAC",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables the Microsoft Intelligent Security Graph (ISG) integration for Smart App Control, allowing real-time reputation data from the Microsoft cloud threat intelligence service to inform allow/deny decisions.",
                    Tags = ["sac", "smart-app-control", "isg", "cloud-intelligence", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22621,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "ISG provides continuously updated threat data; keeping it enabled ensures SAC decisions reflect the latest known-bad software intelligence.",
                    RegistryKeys = [WdCiKey],
                    ApplyOps = [RegOp.SetDword(WdCiKey, "EnableIntelligentSecurityGraph", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdCiKey, "EnableIntelligentSecurityGraph")],
                    DetectOps = [RegOp.CheckDword(WdCiKey, "EnableIntelligentSecurityGraph", 1)],
                },
            ];
    }

    // ── SoftwareRestrictionAdvPolicy ──
    private static class _SoftwareRestrictionAdvPolicy
    {
        private const string SrpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";

        private const string AlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppLocker";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "srpx-set-default-security-level-disallowed",
                    Label = "SRP Advanced: Set Default Security Level to Disallowed",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets DefaultLevel=0 in Safer\\CodeIdentifiers policy (Disallowed). Sets the Software Restriction Policy default security level to Disallowed — all software is blocked unless a specific rule permits it. This is the highest-restriction SRP configuration. In contrast to the default Unrestricted level (all software permitted unless explicitly blocked), Disallowed mode provides a default-deny application control stance. Combined with appropriate allow rules for legitimate applications, this prevents any unauthorised executable from running. This is the pre-AppLocker/pre-WDAC approach that still works for all Windows editions without WDAC infrastructure.",
                    Tags = ["srp", "disallowed", "default-deny", "application-control", "whitelist"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 2,
                    ImpactNote =
                        "All applications are blocked by default. Requires comprehensive allow rules for Windows system binaries, Office, line-of-business apps, and all used scripts. High risk of productivity disruption if allow rules are incomplete. Thoroughly test in audit mode before deploying. AppLocker or WDAC is preferred for modern deployments.",
                    ApplyOps = [RegOp.SetDword(SrpKey, "DefaultLevel", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrpKey, "DefaultLevel")],
                    DetectOps = [RegOp.CheckDword(SrpKey, "DefaultLevel", 0)],
                },
                new TweakDef
                {
                    Id = "srpx-block-executable-from-temp-dirs",
                    Label = "SRP Advanced: Block Executables Running from Temp Directories",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets Level=0 (Disallowed) for a SRP path rule on %TEMP% and %LocalAppData%\\Temp. Malware frequently drops its first-stage payload into the user's Temp directory and executes from there because Temp directories are always user-writable and are rarely monitored or blocked by application control. Blocking executable launch from Temp directories is one of the most effective single controls to prevent drive-by-download malware and phishing payload execution — the majority of malware first-stage binaries that arrive via email attachment or browser download land in Temp.",
                    Tags = ["srp", "temp-directory", "malware-stage1", "drive-by", "exe-block"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Executables cannot run from %TEMP% or %LocalAppData%\\Temp. Some legitimate installers that extract and run from Temp will fail (e.g., some MSI bootstrappers). Identify and whitelist by hash any legitimate software that legitimately runs from Temp before enabling. Most modern installers use %ProgramFiles% and are not affected.",
                    ApplyOps = [RegOp.SetDword(SrpKey, "TransparentEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrpKey, "TransparentEnabled")],
                    DetectOps = [RegOp.CheckDword(SrpKey, "TransparentEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-skip-admin-from-srp",
                    Label = "SRP Advanced: Exempt Administrators from SRP Restrictions",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets PolicyScope=1 in Safer\\CodeIdentifiers policy. Configures Software Restriction Policies to apply only to standard users (non-administrators), exempting local administrator accounts from SRP restrictions. This is a pragmatic balance: local admins need to be able to run IT tools, deployment utilities, and diagnostic software that may not be in the SRP whitelist. Standard users (the majority of the workforce) are protected by default-deny SRP. Attackers who successfully elevate to admin circumvent SRP, but standard-user session compromise (the most common scenario) is blocked.",
                    Tags = ["srp", "admin-exempt", "policy-scope", "standard-users", "uac"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "SRP restrictions apply to standard users only. Administrators are exempt. Standard user accounts — which represent the attack surface for phishing and drive-by attacks — are protected. Admin accounts must rely on WDAC or privilege access workstation controls for software restriction.",
                    ApplyOps = [RegOp.SetDword(SrpKey, "PolicyScope", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrpKey, "PolicyScope")],
                    DetectOps = [RegOp.CheckDword(SrpKey, "PolicyScope", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-enable-drm-file-type-checking",
                    Label = "SRP Advanced: Enable DRM and Dangerous File Type Checking in SRP",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets ExecutableTypes=1 in Safer\\CodeIdentifiers policy. Enables Software Restriction Policy evaluation for a broader set of file types beyond .exe — including .dll, .ocx, .cpl, and other executable file extensions. Without this setting, SRP only checks .exe files. Attackers use .dll sideloading, .ocx files registered via regsvr32, and .cpl files opened via the Control Panel as stagers. Expanding SRP to cover all executable types significantly reduces the attack surface.",
                    Tags = ["srp", "dll-checking", "executable-types", "dll-sideloading", "cpl"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "SRP checks are extended to DLLs, OCX, CPL, and other executable types. More aggressive restriction — some legitimate DLL loading scenarios may be blocked. Test thoroughly. DLL enforcement significantly increases performance overhead in SRP-evaluated environments.",
                    ApplyOps = [RegOp.SetDword(SrpKey, "ExecutableTypes", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrpKey, "ExecutableTypes")],
                    DetectOps = [RegOp.CheckDword(SrpKey, "ExecutableTypes", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-log-srp-policy-events",
                    Label = "SRP Advanced: Log All SRP Policy Evaluation Events",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets LogFileName set and verbose logging enabled via AuthenticodeEnabled=1 in Safer\\CodeIdentifiers policy. Enables SRP event logging, which records all policy evaluation decisions: every executable evaluated by SRP, whether it was permitted or blocked, which rule matched (or that the default level applied), and the full path to the evaluated binary. SRP event logs are written to the Application Event Log. This audit trail is essential for policy development (identifying what needs to be whitelisted before switching to Disallowed mode) and for detecting blocked attack attempts.",
                    Tags = ["srp", "logging", "audit", "event-log", "policy-development"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "All SRP policy evaluation decisions are logged. Logs include permitted and blocked binaries with full paths. Log volume can be high in active environments. Useful phase for policy development to identify all software that needs allow rules before enforcing Disallowed mode.",
                    ApplyOps = [RegOp.SetDword(SrpKey, "AuthenticodeEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrpKey, "AuthenticodeEnabled")],
                    DetectOps = [RegOp.CheckDword(SrpKey, "AuthenticodeEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-enable-applocker-dll-rules",
                    Label = "SRP Advanced: Enable AppLocker DLL Rule Enforcement",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets EnforceDLLRules=1 in AppLocker policy. Enables AppLocker DLL rule enforcement. By default, AppLocker only enforces rules for .exe, .msi, .ps1, and .appx files — it does not evaluate DLL loads unless explicitly enabled. Enabling DLL rules means AppLocker evaluates every DLL loaded by every process against the configured rule set, blocking known-bad or untrusted DLLs. This prevents DLL sideloading attacks (T1574.001) where a malicious DLL is placed in a directory from which a trusted executable loads it. DLL rule evaluation has performance overhead — most enterprises only enable it for high-security workloads.",
                    Tags = ["applocker", "dll-rules", "dll-sideloading", "enforcement", "application-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "AppLocker evaluates every DLL load against AppLocker rules. Significant performance impact on DLL-heavy applications. Requires comprehensive DLL allow rules for all legitimate DLLs. Recommended only for high-security workloads (privileged access workstations, domain controllers) due to performance and complexity.",
                    ApplyOps = [RegOp.SetDword(AlKey, "EnforceDLLRules", 1)],
                    RemoveOps = [RegOp.DeleteValue(AlKey, "EnforceDLLRules")],
                    DetectOps = [RegOp.CheckDword(AlKey, "EnforceDLLRules", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-block-untrusted-fonts",
                    Label = "SRP Advanced: Block Untrusted Fonts from Loading in Kernel-Mode",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets BlockUntrustedFonts=1 in System policy path under AppLocker. Enables the Untrusted Font Blocking feature that prevents untrusted fonts from being loaded by the Windows kernel font parsing code. Font parsing has historically been a major source of kernel privilege escalation vulnerabilities (CVE-2015-2426, CVE-2016-0180, etc.). When an untrusted font is loaded in kernel mode, any parsing vulnerability is immediately exploitable at ring-0. Blocking untrusted fonts means only fonts installed in the Windows Fonts directory are parsed in kernel mode — custom or downloaded fonts would need to be installed to system fonts.",
                    Tags = ["fonts", "kernel", "untrusted", "privilege-escalation", "cve-mitigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Untrusted (non-system-installed) fonts cannot be loaded by kernel-mode code. Fonts must be installed to %SystemRoot%\\Fonts to be trusted. Applications that embed or load custom fonts from non-standard paths may fail to render them. Publishing workflows that use custom fonts must install those fonts to the system font directory.",
                    ApplyOps = [RegOp.SetDword(AlKey, "BlockUntrustedFonts", 1)],
                    RemoveOps = [RegOp.DeleteValue(AlKey, "BlockUntrustedFonts")],
                    DetectOps = [RegOp.CheckDword(AlKey, "BlockUntrustedFonts", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-enable-applocker-audit-mode",
                    Label = "SRP Advanced: Enable AppLocker Audit-Only Mode for All Rule Collections",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets AuditAppLockerExe=1, AuditAppLockerScript=1 in AppLocker policy. Places AppLocker in audit mode for executable and script rule collections. In audit mode, AppLocker logs what it would have blocked without actually blocking anything. This is the essential first phase when building AppLocker policies for an environment — run in audit mode for 30–90 days, collect all events from the Microsoft-Windows-AppLocker/EXE and DLL, MSI and Script, and Packaged app-Deployment channels, and build allow rules from the audit events before switching to enforce mode.",
                    Tags = ["applocker", "audit-mode", "policy-development", "event-log", "deployment"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "AppLocker is in audit-only mode — no executables or scripts are blocked. Events are logged to AppLocker channels for policy analysis. Safe to deploy on any machine as a policy development tool. Audit events show exactly what would be blocked in enforcement mode.",
                    ApplyOps = [RegOp.SetDword(AlKey, "AuditAppLockerExe", 1), RegOp.SetDword(AlKey, "AuditAppLockerScript", 1)],
                    RemoveOps = [RegOp.DeleteValue(AlKey, "AuditAppLockerExe"), RegOp.DeleteValue(AlKey, "AuditAppLockerScript")],
                    DetectOps = [RegOp.CheckDword(AlKey, "AuditAppLockerExe", 1)],
                },
                new TweakDef
                {
                    Id = "srpx-restrict-packaged-app-install",
                    Label = "SRP Advanced: Restrict MSIX/AppX Package Deployment to Signed Packages",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets AllowDevelopmentWithoutDevLicense=0 in AppLocker policy for packaged apps. Prevents unsigned MSIX/AppX packages (Developer Mode packages) from being installed and run on production machines. Developer Mode packages can be sideloaded from any source without going through the Microsoft Store signing process. An attacker who packages malware as an .msix file can install it silently on a machine with Developer Mode enabled, bypassing Store malware filtering. Restricting to signed packages only ensures all MSIX deployments go through a trusted signing infrastructure.",
                    Tags = ["msix", "appx", "developer-mode", "sideloading", "package-signing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Developer Mode MSIX/AppX packages (unsigned sideloaded packages) are blocked. Only MSIX packages signed by a trusted certificate (Microsoft Store, enterprise code signing CA, or Microsoft-signed) can be installed. Developers who use sideloaded packages must use an enterprise code signing certificate.",
                    ApplyOps = [RegOp.SetDword(AlKey, "AllowDevelopmentWithoutDevLicense", 0)],
                    RemoveOps = [RegOp.DeleteValue(AlKey, "AllowDevelopmentWithoutDevLicense")],
                    DetectOps = [RegOp.CheckDword(AlKey, "AllowDevelopmentWithoutDevLicense", 0)],
                },
                new TweakDef
                {
                    Id = "srpx-block-office-child-processes",
                    Label = "SRP Advanced: Block Office Applications from Creating Child Processes",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets BlockOfficeChildProcesses=1 in AppLocker policy. Implements an additional rule that prevents Microsoft Office applications (Word, Excel, PowerPoint, Outlook) from directly creating child processes (cmd.exe, powershell.exe, wscript.exe, etc.). This is a complementary enforcement layer to the identical Defender ASR rule and is enforced via AppLocker EXE rules. The vast majority of Office-spawning attacks (phishing macro + PowerShell download cradle) are blocked by preventing Office from creating child processes. This is one of the highest-fidelity attack detections with minimal false positives in enterprise environments.",
                    Tags = ["office", "child-process", "macro", "phishing", "applocker"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Office applications cannot create cmd.exe, PowerShell, or script host child processes. Legitimate Office macros that run shell commands or spawn scripts will fail. Audit Office macro usage and replace shell-spawning macros with COM automation before enabling. High-value security control for environments with heavy Office usage.",
                    ApplyOps = [RegOp.SetDword(AlKey, "BlockOfficeChildProcesses", 1)],
                    RemoveOps = [RegOp.DeleteValue(AlKey, "BlockOfficeChildProcesses")],
                    DetectOps = [RegOp.CheckDword(AlKey, "BlockOfficeChildProcesses", 1)],
                },
            ];
    }

    // ── WdacCodeIntegrity ──
    private static class _WdacCodeIntegrity
    {
        private const string AsrRules = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\ASR\Rules";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wdac-asr-block-office-child",
                Label = "ASR: Block Office Applications from Creating Child Processes",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "office", "child-process", "security", "defender"],
                Description =
                    "Enables ASR rule D4F940AB-401B-4EFC-AADC-AD5F3C50688A in block mode. "
                    + "Prevents Microsoft Office applications (Word, Excel, PowerPoint, Outlook) from spawning "
                    + "child processes such as cmd.exe, powershell.exe, or wscript.exe. "
                    + "Blocks macro-based malware delivery that uses Office as a launch pad.",
                ApplyOps = [RegOp.SetDword(AsrRules, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "D4F940AB-401B-4EFC-AADC-AD5F3C50688A", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-office-injection",
                Label = "ASR: Block Office Applications from Injecting Code into Other Processes",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "office", "code-injection", "security", "defender"],
                Description =
                    "Enables ASR rule 75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84 in block mode. "
                    + "Blocks Office applications from injecting shellcode or DLLs into other processes. "
                    + "Stops process hollowing and DLL injection techniques used by macro malware to evade detection.",
                ApplyOps = [RegOp.SetDword(AsrRules, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "75668C1F-73B5-4CF0-BB93-3ECF5CB7CC84", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-obfuscated-scripts",
                Label = "ASR: Block Execution of Obfuscated Scripts",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Tags = ["wdac", "asr", "obfuscation", "script", "powershell", "security"],
                Description =
                    "Enables ASR rule 5BEB7EFE-FD9A-4556-801D-275E5FFC04CC in block mode. "
                    + "Blocks execution of script files that appear obfuscated (high entropy, character substitution, "
                    + "or encoded content). Effective against fileless malware delivered via PowerShell or VBScript "
                    + "obfuscation. May occasionally trigger on legitimate heavily encoded scripts.",
                SideEffects = "Legitimate heavily obfuscated scripts may be blocked. Audit mode (value=2) first if unsure.",
                ApplyOps = [RegOp.SetDword(AsrRules, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "5BEB7EFE-FD9A-4556-801D-275E5FFC04CC", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-lsass-dump",
                Label = "ASR: Block Credential Stealing from LSASS",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "lsass", "credentials", "mimikatz", "security"],
                Description =
                    "Enables ASR rule 9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2 in block mode. "
                    + "Blocks attempts to extract credential hashes from lsass.exe memory — "
                    + "the technique used by Mimikatz, ProcDump, and similar tools. "
                    + "Complements RunAsPPL by blocking the dump attempt at the process context level.",
                ApplyOps = [RegOp.SetDword(AsrRules, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "9E6C4E1F-7D60-472F-BA1A-A39EF669E4B2", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-ransomware",
                Label = "ASR: Advanced Protection Against Ransomware",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                Tags = ["wdac", "asr", "ransomware", "protection", "security"],
                Description =
                    "Enables ASR rule C1DB55AB-C21A-4637-BB3F-A12568109D35 in block mode. "
                    + "Engages advanced heuristics to detect and block ransomware-like behaviour: mass file encryption, "
                    + "shadow copy deletion (vssadmin.exe), and low-level file I/O patterns matching known ransomware. "
                    + "May produce false positives on backup and compression tools; test in audit mode first.",
                SideEffects = "Backup software and file archivers may be incorrectly flagged. Test in audit mode (value=2) first.",
                ApplyOps = [RegOp.SetDword(AsrRules, "C1DB55AB-C21A-4637-BB3F-A12568109D35", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "C1DB55AB-C21A-4637-BB3F-A12568109D35", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "C1DB55AB-C21A-4637-BB3F-A12568109D35", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-email-executable",
                Label = "ASR: Block Executable Content from Email Client and Webmail",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "email", "phishing", "malware", "security"],
                Description =
                    "Enables ASR rule BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550 in block mode. "
                    + "Blocks execution of executable files (.exe, .dll, .ps1, .vbs, .js, .bat) that arrive as "
                    + "email attachments or are downloaded from webmail clients. "
                    + "Closes one of the most common ransomware and phishing entry vectors (malicious email attachments).",
                ApplyOps = [RegOp.SetDword(AsrRules, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "BE9BA2D9-53EA-4CDC-84E5-9B1EEEE46550", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-wmi-persistence",
                Label = "ASR: Block WMI Event Subscription Persistence",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "wmi", "persistence", "apt", "security"],
                Description =
                    "Enables ASR rule E6DB77E5-3DF2-4CF1-B95A-636979351E5B in block mode. "
                    + "Blocks malware from creating permanent WMI event subscriptions that survive reboots. "
                    + "WMI subscriptions are a widely-used Advanced Persistent Threat (APT) persistence mechanism "
                    + "that allows code to run automatically when system events occur.",
                ApplyOps = [RegOp.SetDword(AsrRules, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "E6DB77E5-3DF2-4CF1-B95A-636979351E5B", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-psexec-wmi",
                Label = "ASR: Block Process Creations from PSExec and WMI Commands",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 4,
                SafetyRating = 3,
                Tags = ["wdac", "asr", "psexec", "wmi", "lateral-movement", "security"],
                SideEffects = "Blocks legitimate IT operations using PSExec or WMI remoting for remote process creation.",
                Description =
                    "Enables ASR rule D1E49AAC-8F56-4280-B9BA-993A6D77406C in block mode. "
                    + "Stops process creation via PSExec and WMI commands — the two most common tools attackers use "
                    + "for lateral movement after initial compromise. Disabling this rule is required if your "
                    + "organisation uses PSExec/WMI for legitimate remote administration.",
                ApplyOps = [RegOp.SetDword(AsrRules, "D1E49AAC-8F56-4280-B9BA-993A6D77406C", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "D1E49AAC-8F56-4280-B9BA-993A6D77406C", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "D1E49AAC-8F56-4280-B9BA-993A6D77406C", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-usb-untrusted",
                Label = "ASR: Block Untrusted and Unsigned Processes from USB",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "usb", "removable", "unsigned", "security"],
                Description =
                    "Enables ASR rule B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4 in block mode. "
                    + "Blocks unsigned or untrusted executables launched from USB/removable drives. "
                    + "Prevents BadUSB-style attacks and malware that auto-runs from inserted removable media.",
                ApplyOps = [RegOp.SetDword(AsrRules, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "B2B3F03D-6A65-4F7B-A9C7-1C7EF74A9BA4", 1)],
            },
            new TweakDef
            {
                Id = "wdac-asr-block-adobe-child",
                Label = "ASR: Block Adobe Reader from Creating Child Processes",
                Category = "Security — Packaged App Debug",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdac", "asr", "adobe", "pdf", "child-process", "security"],
                Description =
                    "Enables ASR rule 7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C in block mode. "
                    + "Prevents Adobe Acrobat and Adobe Reader from spawning child processes. "
                    + "Blocks PDF-based malware delivery using embedded scripts or exploit code that attempts to "
                    + "launch command shells or download secondary payloads through the PDF reader.",
                ApplyOps = [RegOp.SetDword(AsrRules, "7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C", 1)],
                RemoveOps = [RegOp.SetDword(AsrRules, "7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C", 0)],
                DetectOps = [RegOp.CheckDword(AsrRules, "7674BA52-37EB-4A4F-A9A1-F0F9A1619A2C", 1)],
            },
        ];
    }

    // ── WindowsDefenderApplicationControlPolicy ──
    private static class _WindowsDefenderApplicationControlPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";
        private const string SipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SipEngine";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdacpol-disable-test-signing",
                    Label = "Disable Kernel Test Signing Mode (Block Development Bypass)",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents the kernel from loading drivers that are only test-signed (not production WHQL or EV-signed), closing the development bypass mode that allows unsigned driver loading without hardware attestation.",
                    Tags = ["wdac", "test-signing", "driver-signing", "kernel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Kernel test signing disabled; only production-signed drivers load. Development signing bypass blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTestSigning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTestSigning")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTestSigning", 1)],
                },
                new TweakDef
                {
                    Id = "wdacpol-block-vulnerable-driver-loading",
                    Label = "Enable WDAC Vulnerable Driver Blocklist (Microsoft HVCI Blocklist)",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables the Microsoft-maintained vulnerable driver blocklist (applied via HVCI when memory integrity is on), preventing loading of known LOLBAS kernel drivers used for BYOVD (Bring Your Own Vulnerable Driver) kernel exploits.",
                    Tags = ["wdac", "vulnerable-driver", "byovd", "hvci", "blocklist", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Microsoft vulnerable driver blocklist enforced; known BYOVD exploit driver loading blocked.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "EnableWindowsDriverBlocklist", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "EnableWindowsDriverBlocklist")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "EnableWindowsDriverBlocklist", 1)],
                },
                new TweakDef
                {
                    Id = "wdacpol-require-whql-for-new-drivers",
                    Label = "Require WHQL Signature for New Kernel-Mode Drivers",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Configures code integrity to require WHQL (Windows Hardware Quality Lab) signatures on new kernel-mode drivers, blocking loading of drivers signed only with a self-signed or EV code signing certificate without WHQL attestation.",
                    Tags = ["wdac", "whql", "kernel-driver", "signing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "New kernel drivers require WHQL signature; EV-only signed drivers without WHQL attestation blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireWHQLForNewDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireWHQLForNewDrivers")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireWHQLForNewDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "wdacpol-disable-dynamic-code-policy",
                    Label = "Set WDAC Dynamic Code Security Policy to Enforce Mode",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets the WDAC dynamic code policy to enforced mode, protecting dynamically generated code (JIT-compiled scripts, .NET, browsers) from injecting unsigned code pages that bypass static WDAC policy checks.",
                    Tags = ["wdac", "dynamic-code", "jit", "enforcement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WDAC dynamic code security enforced; JIT-injected code pages validated against code integrity policy.",
                    ApplyOps = [RegOp.SetDword(SipKey, "DynamicCodeSecurity", 2)],
                    RemoveOps = [RegOp.DeleteValue(SipKey, "DynamicCodeSecurity")],
                    DetectOps = [RegOp.CheckDword(SipKey, "DynamicCodeSecurity", 2)],
                },
                new TweakDef
                {
                    Id = "wdacpol-log-ci-failures",
                    Label = "Log Code Integrity Violation Events in CodeIntegrity Log",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables logging of code integrity block decisions in the Microsoft-Windows-CodeIntegrity/Operational event log channel, providing audit records of all executables and drivers blocked by WDAC or HVCI policy.",
                    Tags = ["wdac", "event-log", "audit", "ci-failure", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Code integrity violation events logged; all WDAC/HVCI blocked files visible in CodeIntegrity event channel.",
                    ApplyOps = [RegOp.SetDword(Key, "LogCIFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogCIFailures")],
                    DetectOps = [RegOp.CheckDword(Key, "LogCIFailures", 1)],
                },
                new TweakDef
                {
                    Id = "wdacpol-disable-debug-policy",
                    Label = "Disable WDAC Debug/Audit Mode (Enforce Kernel Debugging Disabled)",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents kernel debugging from being enabled on this system via bcdedit /debug, which would disable code integrity checks entirely, ensuring WDAC cannot be bypassed by attaching a kernel debugger.",
                    Tags = ["wdac", "kernel-debug", "debug-mode", "bypass", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kernel debug mode blocked; WDAC/CI cannot be bypassed via kernel debugger attachment.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableKernelDebugPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableKernelDebugPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableKernelDebugPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "wdacpol-disable-wdac-telemetry",
                    Label = "Disable WDAC Code Integrity Telemetry to Microsoft",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents WDAC and Windows Code Integrity from reporting blocked binary hashes, publisher names, violation rates, and policy effectiveness telemetry to Microsoft.",
                    Tags = ["wdac", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WDAC telemetry to Microsoft disabled; blocked binary hashes and policy stats not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "DisableWDACTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "DisableWDACTelemetry")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "DisableWDACTelemetry", 1)],
                },
            ];
    }

    // ── WindowsInstallerAdvPolicy ──
    private static class _WindowsInstallerAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "winstadv-disable-msi-internet-sources",
                    Label = "Installer Adv: Disable MSI Package Installation from Internet Sources",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets DisableWebInstall=1 in Windows Installer policy. Prevents Windows Installer from downloading and installing MSI packages directly from internet URLs (http://, https://, ftp:// paths). Without this restriction, a shortcut or script can trigger an MSI download-and-install directly from an external web server. "
                        + "Internet-sourced MSI installation is an attack vector in phishing campaigns: a click on a malicious email attachment or web link can trigger a Windows Installer URL handler that downloads and executes a malicious MSI from an attacker-controlled server. The MSI runs with the context of the logged-in user and can contain PowerShell/VBScript custom actions. Modern LOLBins-based attacks use MSI download-and-run as a code execution mechanism that bypasses application whitelisting. Blocking internet MSI sources forces all installations to originate from approved internal sources (SCCM, Intune, network shares). ",
                    Tags = ["winstadv", "msi", "internet-install", "url-install", "phishing", "lolbins"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "MSI installation from internet URLs blocked. Enterprise deployment tools (SCCM, Intune, internal network shares) are unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWebInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWebInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWebInstall", 1)],
                },
                new TweakDef
                {
                    Id = "winstadv-disable-advertised-shortcuts",
                    Label = "Installer Adv: Disable Advertised Shortcut Install-on-Demand to Prevent Elevation Abuse",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets DisableAdvertisedShortcuts=1 in Windows Installer policy. Disables the Windows Installer install-on-demand feature triggered by advertised shortcuts. Advertised shortcuts are MSI feature installation triggers — clicking an advertised shortcut to a feature that was not fully installed causes Windows Installer to complete the feature installation on demand, potentially with elevated privileges if the original product was installed elevated. "
                        + "Install-on-demand via advertised shortcut is a privilege escalation vector: if an MSI product was installed with elevated privileges and an advertised shortcut triggers on-demand installation of a not-yet-installed component, the Windows Installer service performs the installation at elevated privilege on behalf of the user. An attacker who can manipulate an advertised shortcut (via shortcut write access to a shared profile directory) can point it at a malicious MSI component ID — causing the Installer service to execute attacker-controlled code at SYSTEM privilege.",
                    Tags = ["winstadv", "msi", "advertised-shortcut", "install-on-demand", "privilege-escalation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Install-on-demand via advertised shortcuts disabled. Some Office features (install-on-demand Office languages, click-to-run components) may require full pre-installation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAdvertisedShortcuts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAdvertisedShortcuts")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAdvertisedShortcuts", 1)],
                },
                new TweakDef
                {
                    Id = "winstadv-disable-msi-in-locked-session",
                    Label = "Installer Adv: Block Elevated MSI Installs When Session is Locked",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets DisableLockdownInstall=1 in Windows Installer policy. Prevents elevation of Windows Installer packages when the user desktop is locked. Without this restriction, a standard user can trigger an elevated MSI installation (via RunAs or Invoke-Item) for a package that has a UI sequence, then lock their desktop — the Installer continues processing and a crafted DLL extraction step can write to system locations while the desktop is locked and unmonitored. "
                        + "Locked desktop MSI exploitation requires a multi-step attack: (1) trigger an elevated MSI with a crafted UI sequence, (2) lock the desktop before the custom action phase, (3) the custom action executes at SYSTEM during the locked desktop window delivering attacker payloads. This works because Windows Installer continues installation even while the session is locked (installation UI is suppressed but custom actions continue). DisableLockdownInstall=1 aborts any pending elevated installation when the desktop is locked.",
                    Tags = ["winstadv", "msi", "locked-session", "custom-action", "elevation-control"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Elevated MSI installations aborted when session locked. Users installing software must keep desktop unlocked until completion.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLockdownInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLockdownInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLockdownInstall", 1)],
                },
            ];
    }

    // ── WindowsInstallerPolicy ──
    private static class _WindowsInstallerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "msipl-disable-user-installs",
                    Label = "Restrict MSI Installation to Administrators",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents standard users from running Windows Installer packages, requiring administrator authorization for all MSI-based software installations.",
                    Tags = ["msi", "installer", "users", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Reduces attack surface; standard users must request IT to deploy software.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "DisableUserInstalls", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUserInstalls")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUserInstalls", 1)],
                },
                new TweakDef
                {
                    Id = "msipl-deny-browsing-elevated-installs",
                    Label = "Deny Source Browsing During Elevated MSI Installs",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents elevated (lockdown) Windows Installer installations from browsing to an alternate source, closing a privilege escalation path where a user redirects an elevated install to a malicious package.",
                    Tags = ["msi", "installer", "browse", "lockdown", "elevation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Closes browse-redirect privilege escalation during lockdown installs.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockdownBrowse", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockdownBrowse")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockdownBrowse", 0)],
                },
                new TweakDef
                {
                    Id = "msipl-deny-media-elevated-installs",
                    Label = "Deny Removable Media Sources During Elevated MSI Installs",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents elevated (lockdown) Windows Installer installations from using removable media as an installation source, blocking disc- or USB-swap attacks on privileged installs.",
                    Tags = ["msi", "installer", "media", "usb", "lockdown", "elevation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents USB/disc swap attacks against elevated MSI sessions.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockdownMedia", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockdownMedia")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockdownMedia", 0)],
                },
                new TweakDef
                {
                    Id = "msipl-deny-patch-elevated-installs",
                    Label = "Deny Patching During Elevated MSI Installs",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents elevated (lockdown) Windows Installer installations from applying patches, ensuring patches cannot be injected during a privileged install session.",
                    Tags = ["msi", "installer", "patch", "lockdown", "elevation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents patch injection during elevated MSI sessions.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockdownPatch", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockdownPatch")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockdownPatch", 0)],
                },
                new TweakDef
                {
                    Id = "msipl-disable-patch-caching",
                    Label = "Disable MSI Patch File Caching",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Sets the maximum patch cache size to zero, preventing Windows Installer from caching patch files on disk and reclaiming storage consumed by stale .msp files.",
                    Tags = ["msi", "installer", "patch", "cache", "disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Reclaims disk space; patch re-application may require the original source.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "MaxPatchCacheSize", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxPatchCacheSize")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxPatchCacheSize", 0)],
                },
                new TweakDef
                {
                    Id = "msipl-disable-user-control",
                    Label = "Disable User Control Over Installation Options",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Prevents users from overriding installation settings defined by system policy, ensuring enterprise MSI configurations remain authoritative and cannot be bypassed.",
                    Tags = ["msi", "installer", "control", "policy", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enforces policy-defined install settings; users cannot override feature selection or directories.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetDword(Key, "EnableUserControl", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableUserControl")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableUserControl", 0)],
                },
                new TweakDef
                {
                    Id = "msipl-restrict-source-search-network",
                    Label = "Restrict MSI Source Search to Network Locations Only",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Configures Windows Installer to search only network locations (n) when resolving missing installation sources, preventing Installer from falling back to local drives or removable media.",
                    Tags = ["msi", "installer", "source", "network", "search", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Ensures managed installs resolve only from authorised network shares.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetString(Key, "SearchOrder", "n")],
                    RemoveOps = [RegOp.DeleteValue(Key, "SearchOrder")],
                    DetectOps = [RegOp.CheckString(Key, "SearchOrder", "n")],
                },
                new TweakDef
                {
                    Id = "msipl-enable-verbose-event-logging",
                    Label = "Enable Verbose MSI Event Logging",
                    Category = "Security — Packaged App Debug",
                    Description =
                        "Enables detailed Windows Installer event logging (voicewarmupx mode) to the Application event log, providing a comprehensive audit trail for all software install and remove operations.",
                    Tags = ["msi", "installer", "logging", "audit", "events", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Improves software audit trail; negligible performance overhead.",
                    RegistryKeys = [Key],
                    ApplyOps = [RegOp.SetString(Key, "Logging", "voicewarmupx")],
                    RemoveOps = [RegOp.DeleteValue(Key, "Logging")],
                    DetectOps = [RegOp.CheckString(Key, "Logging", "voicewarmupx")],
                },
            ];
    }

    // ── WindowsScriptHostPolicy ──
    private static class _WindowsScriptHostPolicy
    {
        private const string WshKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Script Host\Settings";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wsh-disable-wsh",
                Label = "Disable Windows Script Host",
                Category = "Security — Windows Script Host",
                Description = "Blocks all WSH-based script execution (VBScript, JScript, CScript, WScript).",
                Tags = ["script", "wsh", "security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Prevents execution of .vbs/.js/.wsf scripts via WSH. May break legacy admin scripts.",
                ApplyOps = [RegOp.SetDword(WshKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(WshKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-remote-scripts",
                Label = "Disable WSH Remote Script Execution",
                Category = "Security — Windows Script Host",
                Description = "Prevents WSH from executing scripts that originate from remote (network) locations.",
                Tags = ["script", "wsh", "remote", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks scripts run from UNC paths. Local scripts are unaffected.",
                ApplyOps = [RegOp.SetDword(WshKey, "Remote", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "Remote")],
                DetectOps = [RegOp.CheckDword(WshKey, "Remote", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-trustedcert-bypass",
                Label = "Disable Trusted Certificate Script Bypass",
                Category = "Security — Windows Script Host",
                Description = "Prevents scripts with a trusted code-signing certificate from bypassing the WSH Enabled=0 restriction.",
                Tags = ["script", "wsh", "certificate", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Ensures Enabled=0 applies universally regardless of script signing.",
                ApplyOps = [RegOp.SetDword(WshKey, "TrustPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "TrustPolicy")],
                DetectOps = [RegOp.CheckDword(WshKey, "TrustPolicy", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-activex-in-scripts",
                Label = "Block ActiveX Objects in WSH Scripts",
                Category = "Security — Windows Script Host",
                Description = "Prevents WSH scripts from instantiating ActiveX/COM objects via CreateObject or GetObject.",
                Tags = ["script", "wsh", "activex", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Removes a common malware vector; may break legitimate admin scripts using WMI/ADSI.",
                ApplyOps = [RegOp.SetDword(WshKey, "ActiveXScript", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "ActiveXScript")],
                DetectOps = [RegOp.CheckDword(WshKey, "ActiveXScript", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-embedded-scripts",
                Label = "Block WSH Embedded Script Execution",
                Category = "Security — Windows Script Host",
                Description = "Disallows execution of scripts embedded inside other documents (e.g., HTML Application files).",
                Tags = ["script", "wsh", "hta", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks .hta and embedded script execution via WSH. Reduces attack surface.",
                ApplyOps = [RegOp.SetDword(WshKey, "EmbeddedScript", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "EmbeddedScript")],
                DetectOps = [RegOp.CheckDword(WshKey, "EmbeddedScript", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-wscript-host",
                Label = "Disable WScript.exe Interactive Host",
                Category = "Security — Windows Script Host",
                Description = "Prevents WScript.exe (GUI script host) from running scripts interactively.",
                Tags = ["script", "wsh", "wscript", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "WScript.exe is the GUI host for .vbs/.js; disabling it does not block CScript.exe.",
                ApplyOps = [RegOp.SetDword(WshKey, "UseWINSAAPI", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "UseWINSAAPI")],
                DetectOps = [RegOp.CheckDword(WshKey, "UseWINSAAPI", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-script-logging",
                Label = "Enable WSH Script Execution Logging",
                Category = "Security — Windows Script Host",
                Description = "Enables audit logging of every script execution via WSH to the Application event log.",
                Tags = ["script", "wsh", "logging", "audit"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Visible scripts create audit trails; useful for incident detection.",
                ApplyOps = [RegOp.SetDword(WshKey, "LogSecuritySuccesses", 1)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "LogSecuritySuccesses")],
                DetectOps = [RegOp.CheckDword(WshKey, "LogSecuritySuccesses", 1)],
            },
            new TweakDef
            {
                Id = "wsh-disable-script-ui",
                Label = "Suppress WSH Interactive UI Prompts",
                Category = "Security — Windows Script Host",
                Description = "Prevents scripts from displaying interactive dialog boxes (WScript.Echo, MsgBox).",
                Tags = ["script", "wsh", "ui", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses script-generated dialogs; good for server/kiosk environments.",
                ApplyOps = [RegOp.SetDword(WshKey, "SilentTerminate", 1)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "SilentTerminate")],
                DetectOps = [RegOp.CheckDword(WshKey, "SilentTerminate", 1)],
            },
            new TweakDef
            {
                Id = "wsh-disable-legacy-vbscript",
                Label = "Disable Legacy VBScript Engine via WSH",
                Category = "Security — Windows Script Host",
                Description = "Prevents the legacy VBScript engine from being loaded by WSH, mitigating known CVEs.",
                Tags = ["script", "wsh", "vbscript", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks VBScript execution entirely. Many CVEs target VBScript—this reduces attack surface significantly.",
                ApplyOps = [RegOp.SetDword(WshKey, "VBScriptEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "VBScriptEnabled")],
                DetectOps = [RegOp.CheckDword(WshKey, "VBScriptEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wsh-disable-cscript-host",
                Label = "Disable CScript.exe Console Host",
                Category = "Security — Windows Script Host",
                Description = "Restricts CScript.exe (the console WSH host) from executing scripts without administrator approval.",
                Tags = ["script", "wsh", "cscript", "console"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "CScript.exe is widely abused in fileless attacks. Disable on locked-down systems.",
                ApplyOps = [RegOp.SetDword(WshKey, "IgnoreUserSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(WshKey, "IgnoreUserSettings")],
                DetectOps = [RegOp.CheckDword(WshKey, "IgnoreUserSettings", 1)],
            },
        ];
    }

    // ── WindowsStoreForBusinessPolicy ──
    private static class _WindowsStoreForBusinessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wsfb-disable-store-purchase",
                Label = "Windows Store For Business: Disable Store Purchases",
                Category = "Security — Windows Script Host",
                Description =
                    "Prevents users from making paid purchases through the Microsoft Store. "
                    + "Without this policy, users can purchase apps, games, and media using personal or corporate payment methods. "
                    + "On enterprise endpoints, paid Store purchases should be managed through volume licensing, not individual user transactions. "
                    + "Removing this policy re-enables user-initiated Store purchases.",
                Tags = ["store", "purchase", "paid-apps", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStoreApplications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreApplications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStoreApplications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks Store purchases; prevents unauthorized paid app acquisitions on corporate devices.",
            },
            new TweakDef
            {
                Id = "wsfb-block-non-enterprise-apps",
                Label = "Windows Store For Business: Block Non-Enterprise App Sideloading",
                Category = "Security — Windows Script Host",
                Description =
                    "Prevents installation of non-enterprise (consumer) MSIX/AppX packages via sideloading or developer mode. "
                    + "Sideloading allows arbitrary package files to be deployed outside of Store or Intune validation. "
                    + "On managed endpoints this creates a risk of malicious or unlicensed application deployment. "
                    + "Removing this policy allows MSIX sideloading for testing or development purposes.",
                Tags = ["store", "sideloading", "appx", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowAllTrustedApps", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAllTrustedApps")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAllTrustedApps", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks sideloaded MSIX/AppX packages; prevents unauthorized app deployment channels.",
            },
            new TweakDef
            {
                Id = "wsfb-disable-store-implicit-access",
                Label = "Windows Store For Business: Block Store Access for All Users",
                Category = "Security — Windows Script Host",
                Description =
                    "Applies a machine-wide policy blocking all standard (non-admin) user accounts from accessing the Store. "
                    + "This complements user-scope Store restrictions by ensuring the policy is active for any user who logs onto the device. "
                    + "Useful when Intune MDM enrollment has not yet applied or the GPO has been partially applied. "
                    + "Removing this policy removes the machine-wide Store access block.",
                Tags = ["store", "machine-wide", "access-control", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStoreAppsForAllUsers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreAppsForAllUsers")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStoreAppsForAllUsers", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Machine-wide Store block; effective even before per-user policies are applied.",
            },
            new TweakDef
            {
                Id = "wsfb-disable-in-app-purchases",
                Label = "Windows Store For Business: Disable In-App Purchases",
                Category = "Security — Windows Script Host",
                Description =
                    "Prevents in-app purchase (IAP) transactions within Store applications. "
                    + "Many free-to-download Store apps monetize via in-app purchases for premium content or subscriptions. "
                    + "On corporate devices, in-app purchases can lead to unauthorized charges on corporate payment instruments. "
                    + "Removing this policy re-enables in-app purchase capability within Store apps.",
                Tags = ["store", "in-app-purchase", "billing", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInAppPurchases", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInAppPurchases")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInAppPurchases", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks in-app purchases; prevents accidental billing on corporate payment methods.",
            },
            new TweakDef
            {
                Id = "wsfb-disable-gaming-store",
                Label = "Windows Store For Business: Disable Gaming (Xbox) Store Content",
                Category = "Security — Windows Script Host",
                Description =
                    "Hides gaming-related content and Xbox app promotions within the Microsoft Store UX. "
                    + "On enterprise endpoints gaming content is irrelevant and can distract users from productivity applications. "
                    + "This policy suppresses Xbox Live, Game Pass, and other consumer gaming categories from appearing in Store search and recommendations. "
                    + "Removing this policy restores gaming store content visibility.",
                Tags = ["store", "gaming", "xbox", "enterprise", "distraction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HideGamingModeFromStore", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideGamingModeFromStore")],
                DetectOps = [RegOp.CheckDword(Key, "HideGamingModeFromStore", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides gaming/Xbox content from Store; reduces consumer content exposure on work devices.",
            },
        ];
    }
}
