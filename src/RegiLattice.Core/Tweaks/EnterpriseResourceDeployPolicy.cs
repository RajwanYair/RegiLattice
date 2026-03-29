// RegiLattice.Core — Tweaks/EnterpriseResourceDeployPolicy.cs
// Enterprise Resource Manager app deployment ring and application control Group Policy controls (Sprint 620).
// Category: "Enterprise Resource Deploy Policy" | Slug: erdeploy
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EnterpriseResourceDeployPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "erdeploy-set-default-deploy-ring-broad",
            Label = "Enterprise Deploy: Set Default Application Deployment Ring to 'Broad' Stable Channel",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets DefaultDeployRing=2 in EnterpriseResourceManager policy. Configures the default application deployment ring for this endpoint to the 'Broad' (stable) deployment ring, ensuring the device receives application updates only after full release validation has been completed across the Pilot and Early Majority rings. " +
                "Enterprise application deployments using modern ring-based rollout (Intune or ConfigMgr ring filtering) gate updates through sequenced rings before broad deployment. Endpoints that are miscategorised as 'Pilot' receive updates intended for testing and may encounter pre-release application bugs. Explicitly setting the deployment ring to 'Broad' (ring 2) prevents endpoints from accidentally receiving early-ring deployments due to misconfigured ring assignment logic.",
            Tags = ["enterprise-deploy", "deployment-ring", "app-update", "staging", "rollout"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Endpoint assigned to Broad (stable) deployment ring; receives application updates only after full validation.",
            ApplyOps = [RegOp.SetDword(Key, "DefaultDeployRing", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "DefaultDeployRing")],
            DetectOps = [RegOp.CheckDword(Key, "DefaultDeployRing", 2)],
        },
        new TweakDef
        {
            Id = "erdeploy-require-admin-for-app-removal",
            Label = "Enterprise Deploy: Require Administrator Approval to Remove Managed Applications",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets RequireAdminForAppRemoval=1 in EnterpriseResourceManager policy. Blocks standard users from uninstalling applications that were deployed by the enterprise (via Intune, ConfigMgr, or Group Policy Software Installation), requiring administrative credentials for removal even though the application was installed in user context. " +
                "Required enterprise applications (endpoint detection and response agents, certificate management tools, identity protection software) must remain installed once deployed. A standard user who can uninstall enterprise-managed apps can remove security tooling from their device, creating a gap in protection that may persist until the next compliance check triggers a remediation deployment. Blocking user-initiated uninstall of managed apps prevents intentional or accidental removal of critical security tools.",
            Tags = ["enterprise-deploy", "app-removal", "security-tools", "admin-required", "lockdown"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Managed application removal requires admin approval; users cannot uninstall security tools deployed by IT.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAdminForAppRemoval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForAppRemoval")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAdminForAppRemoval", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-block-user-initiated-install",
            Label = "Enterprise Deploy: Block User-Initiated Application Installation Outside of Managed Channels",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets BlockUserInitiatedInstall=1 in EnterpriseResourceManager policy. Prevents users from initiating the installation of new applications through any mechanism other than IT-managed deployment channels (Intune, ConfigMgr, Software Center) — blocking double-click installer execution, Windows Installer (MSI) invocation, and MSIX/APPX package sideloading by standard users. " +
                "The majority of enterprise malware infections arrive as LOB-disguised executables or malicious MSI packages that a user is socially engineered into running. If users can execute arbitrary installers, the application allowlist maintained by IT is bypassed — even if the endpoint has Microsoft Defender WDAC policy configured, a sufficiently permissive WDAC policy allows signed MSI files from any vendor. Blocking user-initiated installation removes the primary vector for user-driven software installation.",
            Tags = ["enterprise-deploy", "user-install", "msi", "lockdown", "wdac", "applocker"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "User-initiated application installs blocked; all software installations require IT-managed deployment channel.",
            ApplyOps = [RegOp.SetDword(Key, "BlockUserInitiatedInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockUserInitiatedInstall")],
            DetectOps = [RegOp.CheckDword(Key, "BlockUserInitiatedInstall", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-enforce-maintenance-window",
            Label = "Enterprise Deploy: Enforce Deployment Maintenance Window Compliance",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets EnforceMaintenanceWindow=1 in EnterpriseResourceManager policy. Restricts deployment execution by the enterprise resource manager to within the configured maintenance window schedule, preventing deployments from triggering application installs, updates, or reboots during business hours and confining disruptive deployments to the approved maintenance period. " +
                "Without maintenance window enforcement, a deployment configured as 'Available as soon as possible' may start an application install or triggered reboot at any time, including during an end-user presentation or in the middle of a running workflow. Maintenance windows define agreed low-impact periods (after hours, weekends) for deployments. Enforcing the maintenance window prevents IT from accidentally or intentionally bypassing the agreed change window, which is often an ITIL or change management process requirement.",
            Tags = ["enterprise-deploy", "maintenance-window", "deployment-schedule", "change-management", "itil"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Deployments confined to maintenance window; no installs or reboots triggered outside approved maintenance period.",
            ApplyOps = [RegOp.SetDword(Key, "EnforceMaintenanceWindow", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceMaintenanceWindow")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceMaintenanceWindow", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-cap-max-install-retries-3",
            Label = "Enterprise Deploy: Cap Application Installation Retry Attempts at 3",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets MaxInstallRetries=3 in EnterpriseResourceManager policy. Limits the number of times the enterprise resource manager retries a failed application installation to 3 attempts before marking the deployment as failed and triggering an alert, rather than retrying indefinitely. " +
                "A deployment that retries an application installation indefinitely will continually consume CPU, disk I/O, and network bandwidth on the endpoint for days or weeks. On endpoints with transient installation failures (antivirus blocking the installer, required service temporarily unavailable), unlimited retries create ongoing performance degradation. Capping retries at 3 ensures failed deployments are surfaced as failures in the management console rather than silently retrying without ever succeeding.",
            Tags = ["enterprise-deploy", "install-retry", "deployment-failure", "performance", "alert"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Deployment install retries capped at 3; repeat failures surface as deployment failures rather than silent perpetual retry.",
            ApplyOps = [RegOp.SetDword(Key, "MaxInstallRetries", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxInstallRetries")],
            DetectOps = [RegOp.CheckDword(Key, "MaxInstallRetries", 3)],
        },
        new TweakDef
        {
            Id = "erdeploy-enable-deployment-audit-log",
            Label = "Enterprise Deploy: Enable Security Audit Log for All Deployment Operations",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets EnableDeploymentAuditLog=1 in EnterpriseResourceManager policy. Causes each application installation, update, and removal operation completed by the enterprise resource manager to generate a Security event log entry, recording the application name, version, deployment source, requesting authority, and outcome code. " +
                "Application deployment audit logs are required in PCI-DSS, HIPAA, and SOC2 regulated environments where all software changes on in-scope endpoints must be tracked in a tamper-evident audit log. Without deployment audit logging, an attacker who compromises the management channel and installs a malicious application through the enterprise deployment infrastructure would have no on-device trace of the install (as the standard registry Uninstall key is easily manipulated). Security event log entries are tamper-resistant to local manipulation.",
            Tags = ["enterprise-deploy", "audit-log", "deployment", "pci", "hipaa", "soc2"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All enterprise deployment operations generate Security event entries; compliance audit trail for software changes.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDeploymentAuditLog", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDeploymentAuditLog")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDeploymentAuditLog", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-disable-sideloaded-appx-packages",
            Label = "Enterprise Deploy: Disable Sideloading of APPX Packages from Unmanaged Sources",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets DisableSideloadedApps=1 in EnterpriseResourceManager policy. Prevents installation of APPX/MSIX application packages from unsigned or unmanaged sources (USB drives, SharePoint file shares, developer sideloading) and restricts APPX installation to managed channels only (Microsoft Store for Business, Intune managed app, or enterprise signed MSIX bundles). " +
                "MSIX sideloading is the primary vector for distributing trojanised or repackaged application packages disguised as legitimate enterprise tools. An attacker who sends a malicious MSIX package via email or file share (and the user's developer mode is enabled) can have arbitrary code run in a package context with the package's declared capabilities. Disabling sideloading from unmanaged sources blocks this vector without affecting Store and Intune-delivered MSIX packages.",
            Tags = ["enterprise-deploy", "sideloading", "appx", "msix", "developer-mode", "trojan"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "APPX/MSIX sideloading from unmanaged sources blocked; only Store and IT-signed packages install.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSideloadedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSideloadedApps")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSideloadedApps", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-require-signed-deployment-packages",
            Label = "Enterprise Deploy: Require Cryptographic Signing for All Deployment Packages",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets RequireSignedPackages=1 in EnterpriseResourceManager policy. Requires that every application package deployed through the enterprise resource manager is digitally signed by a certificate in the enterprise trusted publisher store before the installation is allowed to proceed, blocking unsigned or improperly signed packages from executing. " +
                "Unsigned deployment packages can be tampered with between the time they are created and the time they are deployed. An attacker who compromises a Distribution Point or content staging server can replace a legitimate installer package with a trojanised version. Without package signing verification, the deployment infrastructure distributes the malicious version to all targeted endpoints without any integrity check. Requiring signed packages ensures only packages that passed code signing (and therefore were authenticated at signing time) are installed.",
            Tags = ["enterprise-deploy", "package-signing", "integrity", "distribution-point", "code-signing"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Unsigned deployment packages blocked; content integrity verified via code signing before installation.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSignedPackages", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedPackages")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSignedPackages", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-block-store-install-during-maintenance",
            Label = "Enterprise Deploy: Block Microsoft Store Application Updates During Active Deployment Window",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets BlockStoreInstallDuringDeployment=1 in EnterpriseResourceManager policy. Suspends automatic Microsoft Store application updates from downloading and installing during active enterprise deployment windows, preventing Store-initiated background installs from competing with enterprise deployment bandwidth and CPU allocations. " +
                "Large enterprise deployments (OS feature updates, security patches for hundreds of applications) consume significant bandwidth from Distribution Points. If the Microsoft Store simultaneously triggers background app updates across the same endpoints during the deployment window, both processes compete for disk I/O, network bandwidth, and Windows Installer service locking. This can cause enterprise deployments to fail with 'service busy' errors or time out due to resource contention. Blocking Store updates during scheduled deployment windows eliminates this interference.",
            Tags = ["enterprise-deploy", "store", "bandwidth", "contention", "deployment-window"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Store app updates paused during enterprise deployment windows; no resource contention with managed deployments.",
            ApplyOps = [RegOp.SetDword(Key, "BlockStoreInstallDuringDeployment", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockStoreInstallDuringDeployment")],
            DetectOps = [RegOp.CheckDword(Key, "BlockStoreInstallDuringDeployment", 1)],
        },
        new TweakDef
        {
            Id = "erdeploy-enable-prerequisite-check-enforcement",
            Label = "Enterprise Deploy: Enforce Prerequisite Dependency Checks Before Application Deployment",
            Category = "Enterprise Resource Deploy Policy",
            Description = "Sets EnforcePrerequisiteChecks=1 in EnterpriseResourceManager policy. Enforces that the installation of a dependent application is verified as successfully installed and functional before the enterprise resource manager proceeds with a higher-level application deployment that requires it as a prerequisite, rather than attempting the deployment and failing at runtime. " +
                "Enterprise application deployments often have prerequisite chains: a LOB application may require a specific .NET runtime version, a specific redistributable, and a specific licence management service to be installed before it will work. Without prerequisite enforcement, all packages attempt installation in parallel, and the LOB application may fail (or partially install) because its prerequisites aren't available yet. Enforcing prerequisite checks runs the dependency chain in the correct order and stops the deployment if any prerequisite fails.",
            Tags = ["enterprise-deploy", "prerequisites", "dependency", "deployment-order", "reliability"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prerequisite dependency verification enforced; deployment packages install in correct dependency order.",
            ApplyOps = [RegOp.SetDword(Key, "EnforcePrerequisiteChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrerequisiteChecks")],
            DetectOps = [RegOp.CheckDword(Key, "EnforcePrerequisiteChecks", 1)],
        },
    ];
}
